using DocumentFormat.OpenXml.Drawing;
using EO.Internal;
using HS.Alarm.AlarmCustomer;
using HS.Entities;
using HS.Facade;
using HS.Framework;
using HS.Web.UI.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using HS.Framework.Utils;

namespace HS.Web.UI.Controllers
{
    public class CalendarController : BaseController
    {
        [Authorize]
        public ActionResult Index(string TicketDate, int? TicketId, Guid? CustomerId)
        {
            try
            {
                #region Validation Check
                if (!base.SetLayoutCommons())
                {
                    return RedirectToAction("Logout", "Login");
                }
                if (!base.IsPermitted(UserPermissions.MenuPermissions.LeftMenuCalendar))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
                if (currentLoggedIn.UserId == null || currentLoggedIn.UserId == Guid.Empty)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                Employee Emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                if (Emp == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                #endregion
                ViewBag.currentLoggedIn = currentLoggedIn.UserRole.Trim().ToLower();

                string GoogleMapAPIKey = _Util.Facade.GlobalSettingsFacade.GetGoogleMapAPIKeyByCompanyId(currentLoggedIn.CompanyId.Value);
                ViewBag.GoogleMapAPIKey = GoogleMapAPIKey;

                CustomCalendarScheduleCalendarList model = new CustomCalendarScheduleCalendarList();
                System.Web.HttpContext.Current.Session["Path"] = "Calendar";
                bool IsAllAndNoneSelectionPermission = IsPermitted(UserPermissions.SchedulePermission.ShowSelectAllAndNoneTechnicianListInCalendar);
                bool IsViewCalendar = IsPermitted(UserPermissions.SchedulePermission.ShowAllUsersOwnCalendar);                
                model = _Util.Facade.ScheduleFacade.GetCustomCalendarIndexPageDataByCompanyId(currentLoggedIn.CompanyId.Value);
                string viewName = "";
                string DynamicValue = "";
                List<string> suser = new List<string>();
                List<string> stype = new List<string>();
                List<Employee> EmpList = new List<Employee>();
                List<SelectListItem> EmployeeList = new List<SelectListItem>();
                List<CalendarEmployeeDataMapper> ListOfSaveData = _Util.Facade.ScheduleFacade.GetAllActiveCalendarMappingByUserId(currentLoggedIn.UserId);
                if (Session[SessionKeys.CalendarSelectedTime] != null)
                {
                    MapSessionModel map = new MapSessionModel();
                    map = (MapSessionModel)Session[SessionKeys.CalendarSelectedTime];
                    if(map.ViewName == "MapView")
                    {
                        TicketDate = map.SelectedDate;
                    }                    
                    map.ViewName = "";
                    Session[SessionKeys.CalendarSelectedTime] = map;
                }

                foreach (var item in model.CalendarGlobalRecords)
                {
                    if (item.SearchKey == "ScheduleCalendarMinTimeRange" && item.IsActive == true) { ViewBag.ScheduleCalendarMinTimeRange = item.Value; }
                    else if (item.SearchKey == "ScheduleCalendarMaxTimeRange" && item.IsActive == true) { ViewBag.ScheduleCalendarMaxTimeRange = item.Value; }
                    else if (item.SearchKey == "FirstDayOfWeek" && item.IsActive == true) { ViewBag.FirstDayOfWeek = item.Value; }
                    else if (item.SearchKey == "CustomCalendarFontSize" && item.IsActive == true) { ViewBag.CustomCalendarFontSize = item.Value; }
                    else if (item.SearchKey == "CustomCalendarTopRowEmployee" && item.IsActive == true) { ViewBag.CustomCalendarTopRowEmployee = item.Value; ViewBag.SystemUserPosition = item.OptionalValue; ViewBag.IsBottom = item.IsBottom; }
                    else if (item.SearchKey == "CustomCalendarRMRReportShow" && item.IsActive == true) { ViewBag.CustomCalendarRMRReportShow = item.Value; }
                    else if (item.SearchKey == "CustomCalendarScheduleBorderShow" && item.IsActive == true) { ViewBag.CustomCalendarScheduleBorderShow = item.Value; }
                    else if (item.SearchKey == "CustomCalendarScheduleShadowShow" && item.IsActive == true) { ViewBag.CustomCalendarScheduleShadowShow = item.Value; }
                    else if (item.SearchKey == "CustomCalendarTableHeaderColor" && item.IsActive == true) { ViewBag.CustomCalendarTableHeaderColor = item.Value; }
                    else if (item.SearchKey == "CustomCalendarColumnHourDuration" && item.IsActive == true) { ViewBag.CustomCalendarColumnHourDuration = item.Value; }
                    else if (item.SearchKey == "ScheduleCalendarDefaultView" && item.IsActive == true) { ViewBag.defaultView = item.Value; viewName = item.Value; }
                    else if (item.SearchKey == "EnableSelectedDateReload" && item.IsActive == true) { ViewBag.SelectedDateReload = item.Value; }
                    else if (item.SearchKey == "CalendarEventIconResizer" && item.IsActive == true) { ViewBag.IconResizer = item.Value; }
                    else if (item.SearchKey == "CalendarEventTicketResize" && item.IsActive == true) { ViewBag.TicketResizer = item.Value; DynamicValue = item.OptionalValue; }
                    else if (item.SearchKey == "CalendarViewShow" && item.IsActive == true) { ViewBag.CalendarViewPosition = item.Value; }
                }
                
                List<SelectListItem> TicketTypeList = model.CalendarViewTicketType.OrderBy(x => x.DisplayText).ToList().Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();
                ViewBag.TicketType = TicketTypeList;
                List<CalendarEmployeeDataMapper> SelectedTktTypelist = new List<CalendarEmployeeDataMapper>();
                if (ListOfSaveData != null && ListOfSaveData.Count > 0)
                {
                    SelectedTktTypelist = ListOfSaveData.Where(x => !string.IsNullOrWhiteSpace(x.MapType) && x.EmplyeeSelectedId == Guid.Empty).ToList();
                }
                if (SelectedTktTypelist != null && SelectedTktTypelist.Count > 0)
                {
                    stype.Clear();
                    stype = SelectedTktTypelist.Select(x => x.MapType).ToList();
                }
                //else if (model.CalendarViewTicketType != null && model.CalendarViewTicketType.Count > 0)
                //{
                //    foreach (var type in model.CalendarViewTicketType)
                //    {
                //        if (type.IsDefaultItem)
                //        {
                //            stype.Add(type.DataValue);
                //        }
                //    }
                //}
                var LocalTodate = DateTime.UtcNow.UTCToClientTime();
                if (viewName == "Daily")
                {
                    var datetimeMonth = LocalTodate.ToString("MMMM", CultureInfo.InvariantCulture);
                    var datetimeDay = LocalTodate.ToString("dd", CultureInfo.InvariantCulture);
                    var DayName = LocalTodate.ToString("ddd", CultureInfo.InvariantCulture);
                    var datetimeYear = LocalTodate.ToString("yyyy", CultureInfo.InvariantCulture);
                    ViewBag.DateTitle = DayName + " " + datetimeMonth + " " + datetimeDay + ", " + datetimeYear;
                }
                else if (viewName == "Weekly" || viewName == "List")
                {
                    var datetimesMonth = LocalTodate.ToString("MMM", CultureInfo.InvariantCulture);
                    var datetimesDay = LocalTodate.ToString("dd", CultureInfo.InvariantCulture);
                    var datetimeYear = LocalTodate.ToString("yyyy", CultureInfo.InvariantCulture);
                    DateTime LastDate = LocalTodate.AddDays(6);
                    var datetimelMonth = LastDate.ToString("MMM", CultureInfo.InvariantCulture);
                    var datetimelDay = LastDate.ToString("dd", CultureInfo.InvariantCulture);
                    ViewBag.DateTitle = datetimesMonth + " " + datetimesDay + " - " + datetimelMonth + " " + datetimelDay + ", " + datetimeYear;
                }
                else if (viewName == "Monthly")
                {
                    ViewBag.DateTitle = LocalTodate.ToString("MMMM") + " " + LocalTodate.Year;
                }

                string todate = LocalTodate.ToString("yyyy-MM-dd");
                if (!string.IsNullOrWhiteSpace(TicketDate) && !string.IsNullOrEmpty(TicketDate))
                {
                    DateTime DateFromTicket = new DateTime();
                    bool convertResult = DateTime.TryParse(TicketDate, out DateFromTicket);
                    if (convertResult)
                    {
                        todate = DateFromTicket.ToString("yyyy-MM-dd");
                        ViewBag.defaultView = "Daily";
                        ViewBag.TicketIntId = TicketId;
                        ViewBag.CustomerGiudId = CustomerId;
                    }
                }
                if (IsPermitted(UserPermissions.SchedulePermission.ShowUsersHaveEventCalendar))
                {
                    var TechnicianList = _Util.Facade.ScheduleFacade.GetUserListByCompanyIdHaveEvent(currentLoggedIn.CompanyId.Value, todate, viewName);
                    if (TechnicianList != null && TechnicianList.ListUserIdHaveEvent != null && TechnicianList.ListUserIdHaveEvent.Count > 0)
                    {
                        foreach (var item in TechnicianList.ListUserIdHaveEvent)
                        {
                            suser.Add(item.UserId.ToString().ToLower());
                            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item.UserId);
                            if (objemp != null)
                            {
                                EmpList.Add(objemp);
                            }
                        }
                    }
                }
                if (!IsViewCalendar)
                {
                    EmployeeList.Clear();
                    suser.Clear();
                    SelectListItem LoggedInUser = new SelectListItem()
                    {
                        Text = currentLoggedIn.GetFullName(),
                        Value = currentLoggedIn.UserId.ToString(),
                        Selected = true
                    };
                    EmployeeList.Add(LoggedInUser);
                    suser.Add(currentLoggedIn.UserId.ToString());
                }
                else
                {
                    EmployeeList.AddRange(model.CalendarEmployeeList.OrderBy(x => x.ResourceName).Select(x => new SelectListItem()
                    {
                        Text = x.ResourceName.ToString(),
                        Value = x.UserId.ToString().ToLower(),
                        Selected = EmpList.Count(y => y.UserId == Guid.Parse(x.UserId)) > 0
                    }).ToList());
                }

                List<CalendarEmployeeDataMapper> SelectedEmplist = new List<CalendarEmployeeDataMapper>();
                if (ListOfSaveData != null && ListOfSaveData.Count > 0)
                {
                    SelectedEmplist = ListOfSaveData.Where(x => string.IsNullOrWhiteSpace(x.MapType) && x.EmplyeeSelectedId != Guid.Empty).ToList();
                }
                if (SelectedEmplist != null && SelectedEmplist.Count > 0)
                {
                    suser.Clear();
                    suser = SelectedEmplist.Select(x => x.EmplyeeSelectedId.ToString()).ToList();
                }
                //else if (suser == null || suser.Count == 0)
                //{
                //    suser.Clear();
                //    List<Employee> EmpListModel = _Util.Facade.EmployeeFacade.GetAllEmployeeDefaultForCalendar(currentLoggedIn.CompanyId.Value);
                //    if (EmpListModel != null && EmpListModel.Count > 0)
                //    {
                //        foreach (var item in EmpListModel)
                //        {
                //            suser.Add(item.UserId.ToString());
                //        }
                //    }
                //}
                #region View Bag section
                ViewBag.IsSupervisor = Emp.IsSupervisor.HasValue ? Emp.IsSupervisor.Value.ToString() : false.ToString();
                ViewBag.IsCalendar = Emp.IsCalendar.HasValue ? Emp.IsCalendar.Value.ToString() : false.ToString();
                ViewBag.IsViewCalendar = IsViewCalendar.ToString();
                ViewBag.IsEventDragDrop = IsPermitted(UserPermissions.SchedulePermission.EnableEventDragDropPermission).ToString();
                ViewBag.TicketStatus = model.CalendarViewTicketType.ToList();
                ViewBag.StatusTicket = model.CalendarTicketStatus.ToList();
                ViewBag.IsAllPermited = IsAllAndNoneSelectionPermission;
                ViewBag.Currentday = todate;
                ViewBag.UserValValueList = suser;
                ViewBag.ListEmployee = EmployeeList;
                ViewBag.typevalList = stype;
                if (DynamicValue == "false" && ViewBag.CalendarViewPosition == "vertical")
                {
                    ViewBag.TicketResizer = "0";
                }
                #endregion
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }
        }
        public ActionResult SchedulePartial(string CurrentDate, string CurrentView, string UserVal, string typeval, string status, string skills)
        {
            try
            {
                var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
                List<Employee> EmpList = new List<Employee>();
                List<string> suser = new List<string>();
                List<string> stype = new List<string>();
                List<string> ESkills = new List<string>();
                List<SelectListItem> EmployeeList = new List<SelectListItem>();
                bool IsAllSelected = false, IsNoneselected = false, IsClicked = false, IsAllAndNoneSelectionPermission = false;
                bool IsViewCalendar = IsPermitted(UserPermissions.SchedulePermission.ShowAllUsersOwnCalendar);
                if (currentLoggedIn.UserId == null || currentLoggedIn.UserId == Guid.Empty)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                if (status == "all")
                {
                    IsAllSelected = true;
                    IsNoneselected = false;
                }
                else if (status == "none")
                {
                    IsNoneselected = true;
                    IsAllSelected = false;
                }
                else if (status == "clickeduser")
                {
                    IsClicked = true;
                }

                if (!string.IsNullOrWhiteSpace(UserVal) && UserVal != "null" && UserVal != "none" && UserVal != "undefined")
                {
                    string[] splituser = UserVal.Split(',');
                    if (splituser.Length > 0)
                    {
                        foreach (var item in splituser)
                        {
                            if (item != "all" && item != "none" && item != "undefined")
                            {
                                suser.Add(item.ToLower());
                                var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(new Guid(item));
                                if (objemp != null)
                                {
                                    EmpList.Add(objemp);
                                }
                            }
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(typeval) && typeval != "null")
                {
                    typeval = System.Web.HttpUtility.UrlDecode(typeval);
                    string[] splituser = typeval.Split(',');
                    if (splituser.Length > 0)
                    {
                        foreach (var item in splituser)
                        {
                            stype.Add(item);
                        }
                    }
                }
                List<Lookup> TicketList = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Where(x => x.DataValue != "-1" && x.AlterDisplayText1 != null && x.AlterDisplayText1 != "" && x.AlterDisplayText1 == "True").ToList();
                if (Session[SessionKeys.SelectedCalendarTicketType] != null)
                {
                    stype.Clear();
                    stype = (List<string>)Session[SessionKeys.SelectedCalendarTicketType];
                }
                else if (TicketList != null && TicketList.Count > 0)
                {
                    foreach (var type in TicketList)
                    {
                        if (type.IsDefaultItem.HasValue && type.IsDefaultItem.Value)
                        {
                            stype.Add(type.DataValue);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(skills) && skills != "null")
                {
                    skills = System.Web.HttpUtility.UrlDecode(skills);
                    string[] splitskills = skills.Split(',');
                    if (splitskills.Length > 0)
                    {
                        foreach (var item in splitskills)
                        {
                            ESkills.Add(item);
                        }
                    }
                }
                List<Lookup> SkillsList = _Util.Facade.LookupFacade.GetLookupByKey("BookingServiceItems").ToList();
                if (Session[SessionKeys.SelectedCalendarSkills] != null)
                {
                    ESkills.Clear();
                    ESkills = (List<string>)Session[SessionKeys.SelectedCalendarSkills];
                }
                else if (SkillsList != null && SkillsList.Count > 0)
                {
                    foreach (var type in SkillsList)
                    {
                        if (type.IsDefaultItem.HasValue && type.IsDefaultItem.Value)
                        {
                            ESkills.Add(type.DataValue);
                        }
                    }
                }

                IsAllAndNoneSelectionPermission = IsPermitted(UserPermissions.SchedulePermission.ShowSelectAllAndNoneTechnicianListInCalendar);
                if (!IsClicked && IsPermitted(UserPermissions.SchedulePermission.ShowUsersHaveEventCalendar))
                {
                    var TechnicianList = _Util.Facade.ScheduleFacade.GetUserListByCompanyIdHaveEvent(currentLoggedIn.CompanyId.Value, CurrentDate, CurrentView);
                    if (TechnicianList != null && TechnicianList.ListUserIdHaveEvent != null && TechnicianList.ListUserIdHaveEvent.Count > 0)
                    {
                        foreach (var item in TechnicianList.ListUserIdHaveEvent)
                        {
                            suser.Add(item.UserId.ToString().ToLower());
                            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item.UserId);
                            if (objemp != null)
                            {
                                EmpList.Add(objemp);
                            }
                        }
                    }
                }

                if (!IsViewCalendar)
                {
                    EmployeeList.Clear();
                    suser.Clear();
                    SelectListItem LoggedInUser = new SelectListItem()
                    {
                        Text = currentLoggedIn.GetFullName(),
                        Value = currentLoggedIn.UserId.ToString(),
                        Selected = true
                    };
                    EmployeeList.Add(LoggedInUser);
                    suser.Add(currentLoggedIn.UserId.ToString());
                }
                else
                {
                    var EmpDataList = _Util.Facade.EmployeeFacade.GetEmployeeListCalendarSchedule(new Guid());
                    if (IsAllAndNoneSelectionPermission && EmpDataList != null && EmpDataList.Count > 0)
                    {
                        if (IsNoneselected)
                        {
                            EmpList.Clear();
                            suser.Clear();
                            SelectListItem selectnone = new SelectListItem()
                            {
                                Text = "None",
                                Value = "none",
                                Selected = true
                            };
                            EmployeeList.Add(selectnone);
                            suser.Add("none");
                            SelectListItem selectall = new SelectListItem()
                            {
                                Text = "All",
                                Value = "all",
                                Selected = false
                            };
                            EmployeeList.Add(selectall);
                            EmployeeList.AddRange(EmpDataList.OrderBy(x => x.ResourceName).Select(x => new SelectListItem()
                            {
                                Text = x.ResourceName.ToString(),
                                Value = x.UserId.ToString().ToLower(),
                                Selected = EmpList.Count(y => y.UserId == x.UserId) > 0
                            }).ToList());
                        }
                        else if (IsAllSelected)
                        {
                            EmpList.Clear();
                            suser.Clear();
                            foreach (var item in EmpDataList)
                            {
                                suser.Add(item.UserId.ToString().ToLower());
                                var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item.UserId);
                                if (objemp != null)
                                {
                                    EmpList.Add(objemp);
                                }
                            }
                            SelectListItem selectnone = new SelectListItem()
                            {
                                Text = "None",
                                Value = "none",
                                Selected = false
                            };
                            EmployeeList.Add(selectnone);
                            //suser.Add("none");
                            SelectListItem selectall = new SelectListItem()
                            {
                                Text = "All",
                                Value = "all",
                                Selected = true
                            };
                            EmployeeList.Add(selectall);
                            suser.Add("all");
                            EmployeeList.AddRange(EmpDataList.OrderBy(x => x.ResourceName).Select(x => new SelectListItem()
                            {
                                Text = x.ResourceName.ToString(),
                                Value = x.UserId.ToString().ToLower(),
                                Selected = EmpList.Count(y => y.UserId == x.UserId) > 0
                            }).ToList());
                        }
                        else
                        {
                            SelectListItem selectnone = new SelectListItem()
                            {
                                Text = "None",
                                Value = "none",
                                Selected = false
                            };
                            EmployeeList.Add(selectnone);
                            SelectListItem selectall = new SelectListItem()
                            {
                                Text = "All",
                                Value = "all",
                                Selected = false
                            };
                            EmployeeList.Add(selectall);
                            EmployeeList.AddRange(EmpDataList.OrderBy(x => x.ResourceName).Select(x => new SelectListItem()
                            {
                                Text = x.ResourceName.ToString(),
                                Value = x.UserId.ToString().ToLower(),
                                Selected = EmpList.Count(y => y.UserId == x.UserId) > 0
                            }).ToList());
                        }
                    }
                    else
                    {
                        EmployeeList.AddRange(EmpDataList.OrderBy(x => x.ResourceName).Select(x => new SelectListItem()
                        {
                            Text = x.ResourceName.ToString(),
                            Value = x.UserId.ToString().ToLower(),
                            Selected = EmpList.Count(y => y.UserId == x.UserId) > 0
                        }).ToList());
                    }
                }
                if (Session[SessionKeys.SelectedCalendarEmployee] != null)
                {
                    suser.Clear();
                    suser = (List<string>)Session[SessionKeys.SelectedCalendarEmployee];
                }
                //EmployeeList.AddRange(model.CalendarEmployeeList.OrderBy(x => x.ResourceName).Select(x => new SelectListItem()
                //{
                //    Text = x.ResourceName.ToString(),
                //    Value = x.UserId.ToString().ToLower(),
                //    Selected = EmpList.Count(y => y.UserId == Guid.Parse(x.UserId)) > 0
                //}).ToList());
                ViewBag.UserValValue = suser;
                ViewBag.ListEmployee = EmployeeList;
                if (IsAllAndNoneSelectionPermission)
                {
                    ViewBag.EmpCount = EmployeeList.Count - 2;
                }
                else
                {
                    ViewBag.EmpCount = EmployeeList.Count;
                }
                ViewBag.Ispermission = IsAllAndNoneSelectionPermission;
                ViewBag.typeval = stype;
                ViewBag.ESkill = ESkills;
                ViewBag.TicketType = TicketList.OrderBy(x => x.DisplayText).ToList().Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();
                return PartialView("ScheduleUserListPartial");
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
        }

        [HttpPost]
        [Authorize]
        //public JsonResult AllScheduleCalendar(List<string> Default, string startdate, string defaultView, List<string> typeval, string CalendarStartTime, string CalendarEndTime, string FirstDayOfWeek, string TopRowEmployee, string HolidayCount, bool IsDeactive = false)
        public JsonResult AllScheduleCalendar(List<string> Default, string startdate, string defaultView, List<string> skills, List<string> typeval, string CalendarStartTime, string CalendarEndTime, string FirstDayOfWeek, string TopRowEmployee, string HolidayCount, string SearchText)
        {
            CustomCalendarAllTaskList model = new CustomCalendarAllTaskList();
            InActiveTicketsModel ActiveModel = new InActiveTicketsModel();
            List<CustomerBillAccoutType> statusmodel = new List<CustomerBillAccoutType>();
            try
            {
                if (!string.IsNullOrWhiteSpace(startdate))
                {
                    MapSessionModel map = new MapSessionModel();
                    map.SelectedDate = startdate;
                    map.ViewName = defaultView;
                    Session[SessionKeys.CalendarSelectedTime] = map;
                }                
                var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
                string ValUser = "";
                string eventtype = "";
                string UserSkills = "";
                int TicketIntId = 0;
                string CustomerName = ""; 
                if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "undefined")
                {
                    SearchText = System.Web.HttpUtility.UrlDecode(SearchText);
                    if(int.TryParse(SearchText, out TicketIntId))
                    {
                        TicketIntId = TicketIntId;
                    }
                    else
                    {
                        CustomerName = SearchText;
                    }
                }
                
                List<string> defaultval = new List<string>();
                bool IsType = false;
                bool IsViewCalendar = IsPermitted(UserPermissions.SchedulePermission.ShowAllUsersOwnCalendar);
                if (currentLoggedIn.UserId == null || currentLoggedIn.UserId == Guid.Empty)
                {
                    return Json(new { Model = model, Defaultinfo = Default, Startdate = startdate, View = defaultView, Typeval = typeval, Calstatus = statusmodel, Active = ActiveModel });
                }

                DeactiveListOfCalendarMappingData(currentLoggedIn.UserId, "Employee");
                DeactiveListOfCalendarMappingData(currentLoggedIn.UserId, "TicketType");
                if (Default != null && Default.Count > 0)
                {
                    //bool AddDefaultEmp = true;
                    //if (Session[SessionKeys.SelectedCalendarEmployee] != null)
                    //{
                    //    List<string> SessionValue = (List<string>)Session[SessionKeys.SelectedCalendarEmployee];
                    //    if (SessionValue.Count == Default.Count)
                    //    {
                    //        int count = 0;
                    //        foreach (string ses in SessionValue)
                    //        {
                    //            foreach (string def in Default)
                    //            {
                    //                if (def.ToLower() == ses.ToLower()) { count++; break; }
                    //            }
                    //        }
                    //        if (count == Default.Count) { AddDefaultEmp = false; }
                    //    }
                    //}
                    foreach (var item in Default)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && item != "null" && item != "all")
                        {
                            defaultval.Add(item);
                            UpdateCalendarMappingData(currentLoggedIn.UserId, null, true, item);
                        }
                        System.Web.HttpContext.Current.Session[SessionKeys.SelectedCalendarEmployee] = defaultval;
                    }
                    ValUser = string.Format("'{0}'", string.Join("','", defaultval.Select(i => i.Replace("'", "''"))));
                }
                else { System.Web.HttpContext.Current.Session[SessionKeys.SelectedCalendarEmployee] = null; }
                List<string> defaulttype = new List<string>();
                if (typeval != null && typeval.Count > 0)
                {
                    List<SelectListItem> TicketTypeList = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Where(x => x.DataValue != "-1" && x.AlterDisplayText1 != null && x.AlterDisplayText1 != "" && x.AlterDisplayText1 == "True").OrderBy(x => x.DisplayText).ToList().Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList();
                    foreach (SelectListItem type in TicketTypeList)
                    {
                        if (typeval.Count == 1 && typeval[0] == type.Text || typeval[0] == type.Value)
                        {
                            System.Web.HttpContext.Current.Session[SessionKeys.SelectedCalendarTicketType] = typeval;
                            IsType = true;
                            break;
                        }
                    }
                    foreach (var item in typeval)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && item != "null")
                        {
                            defaulttype.Add(item);
                            UpdateCalendarMappingData(currentLoggedIn.UserId, item, true, null);
                        }
                    }
                    eventtype = string.Format("'{0}'", string.Join("','", defaulttype.Select(i => i.Replace("'", "''"))));
                }
                else { System.Web.HttpContext.Current.Session[SessionKeys.SelectedCalendarTicketType] = null; }
                #region skills
                List<string> defaultskill = new List<string>(); 
                if (skills != null && skills.Count > 0)
                {
                    List<SelectListItem> SkillsList = _Util.Facade.LookupFacade.GetAllLookupByKey("BookingServiceItems").Select(x => new SelectListItem()
                    {
                        Text = x.DisplayText,
                        Value = x.DataValue
                    }).ToList();
                    foreach (SelectListItem skl in SkillsList)
                    {
                        if (skills.Count == 1 && skills[0] == skl.Text || skills[0] == skl.Value)
                        {
                            System.Web.HttpContext.Current.Session[SessionKeys.SelectedCalendarSkills] = skills;
                            IsType = true;
                            break;
                        }
                    }
                    foreach (var item in skills)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && item != "null")
                        {
                            defaultskill.Add(item);
                        }
                    }
                    UserSkills = string.Format("'{0}'", string.Join("','", defaultskill.Select(i => i.Replace("'", "''"))));
                }
                else { System.Web.HttpContext.Current.Session[SessionKeys.SelectedCalendarSkills] = null; }
                #endregion
                if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrWhiteSpace(startdate) && !string.IsNullOrEmpty(defaultView) && !string.IsNullOrWhiteSpace(defaultView) && !string.IsNullOrEmpty(FirstDayOfWeek) && !string.IsNullOrWhiteSpace(FirstDayOfWeek))
                {
                    if (defaultView == "Weekly")
                    {
                        var weekstart = Convert.ToDateTime(startdate);
                        string day = weekstart.ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
                        if (FirstDayOfWeek.ToLower() == "saturday")
                        {
                            if (day.ToLower() == "sunday") { startdate = (weekstart.AddDays(-1)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "monday") { startdate = (weekstart.AddDays(-2)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "tuesday") { startdate = (weekstart.AddDays(-3)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "wednesday") { startdate = (weekstart.AddDays(-4)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "thursday") { startdate = (weekstart.AddDays(-5)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "friday") { startdate = (weekstart.AddDays(-6)).ToString("yyyy/MM/dd"); }
                            else { startdate = weekstart.ToString("yyyy/MM/dd"); }
                        }
                        else if (FirstDayOfWeek.ToLower() == "sunday")
                        {
                            if (day.ToLower() == "monday") { startdate = (weekstart.AddDays(-1)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "tuesday") { startdate = (weekstart.AddDays(-2)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "wednesday") { startdate = (weekstart.AddDays(-3)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "thursday") { startdate = (weekstart.AddDays(-4)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "friday") { startdate = (weekstart.AddDays(-5)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "saturday") { startdate = (weekstart.AddDays(-6)).ToString("yyyy/MM/dd"); }
                            else { startdate = weekstart.ToString("yyyy/MM/dd"); }
                        }
                        else if (FirstDayOfWeek.ToLower() == "monday")
                        {
                            if (day.ToLower() == "tuesday") { startdate = (weekstart.AddDays(-1)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "wednesday") { startdate = (weekstart.AddDays(-2)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "thursday") { startdate = (weekstart.AddDays(-3)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "friday") { startdate = (weekstart.AddDays(-4)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "saturday") { startdate = (weekstart.AddDays(-5)).ToString("yyyy/MM/dd"); }
                            else if (day.ToLower() == "sunday") { startdate = (weekstart.AddDays(-6)).ToString("yyyy/MM/dd"); }
                            else { startdate = weekstart.ToString("yyyy/MM/dd"); }
                        }
                    }
                }

                if (!IsViewCalendar)
                {
                    ValUser = "";
                    ValUser = string.Format("'{0}'", currentLoggedIn.UserId.ToString());
                    //model = _Util.Facade.ScheduleFacade.GetCustomCalendarScheduleListByCompanyId(currentLoggedIn.CompanyId.Value, startdate, defaultView, eventtype, ValUser, IsType, IsDeactive);
                    model = _Util.Facade.ScheduleFacade.GetCustomCalendarScheduleListByCompanyId(currentLoggedIn.CompanyId.Value, startdate, defaultView, eventtype, ValUser, IsType, "Calendar", UserSkills, TicketIntId, CustomerName);
                }
                else
                {
                    //model = _Util.Facade.ScheduleFacade.GetCustomCalendarScheduleListByCompanyId(currentLoggedIn.CompanyId.Value, startdate, defaultView, eventtype, ValUser, IsType, IsDeactive);              
                    model = _Util.Facade.ScheduleFacade.GetCustomCalendarScheduleListByCompanyId(currentLoggedIn.CompanyId.Value, startdate, defaultView, eventtype, ValUser, IsType, "Calendar", UserSkills, TicketIntId, CustomerName);
                }
                if (model.CalendarEmployeeList != null && model.CalendarTaskList != null)
                {
                    ActiveModel.TotalCount = model.DeactiveTicketCount;
                    //ActiveModel.IsActive = IsDeactive;
                    string TitleDate = "";
                    DateTime SelectedDate = DateTime.UtcNow.UTCToClientTime();
                    if (model.CalendarEmployeeList.Count > 0)
                    {
                        model.CalendarEmployeeList = model.CalendarEmployeeList.OrderByDescending(x => x.Rank).ThenBy(x => x.ResourceName).ToList();
                    }
                    if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrWhiteSpace(startdate))
                    {
                        SelectedDate = DateTime.Parse(startdate);
                    }

                    List<CustomCalendarTicketStatus> StatusList = _Util.Facade.ScheduleFacade.GetCustomCalenderStatusByCompanyId(currentLoggedIn.CompanyId.Value);
                    if(StatusList != null && StatusList.Count() > 0 && model.CalendarTaskList.Count() > 0)
                    {
                        foreach(var stt in StatusList)
                        {
                            CustomerBillAccoutType tt = new CustomerBillAccoutType();
                            tt.CustomerBillID = model.CalendarTaskList.Where(x => x.EventStatus.ToLower() == stt.TicketStatus.ToLower()).Count();
                            tt.Type = stt.TicketStatus;
                            statusmodel.Add(tt);
                        }
                    }

                    if (defaultView == "Daily" || string.IsNullOrEmpty(defaultView))
                    {
                        DailyInfoListModel data = dailyinfo(model.CalendarTaskList, CalendarStartTime, CalendarEndTime, model.CalendarEmployeeList, TopRowEmployee, startdate, FirstDayOfWeek, HolidayCount);
                        var datetimeMonth = SelectedDate.ToString("MMMM", CultureInfo.InvariantCulture);
                        var datetimeDay = SelectedDate.ToString("dd", CultureInfo.InvariantCulture);
                        var datetimeYear = SelectedDate.ToString("yyyy", CultureInfo.InvariantCulture);
                        var DayName = SelectedDate.ToString("ddd", CultureInfo.InvariantCulture);
                        TitleDate = DayName + " " + datetimeMonth + " " + datetimeDay + ", " + datetimeYear;
                        return Json(new { Datalist = data, Defaultinfo = Default, Startdate = startdate, View = defaultView, Typeval = typeval, Calstatus = statusmodel, DateTitle = TitleDate, Active = ActiveModel });
                    }
                    else if (defaultView == "Weekly")
                    {
                        WeeklyInfoListModel data = weeklyinfo(model.CalendarTaskList, startdate, model.CalendarEmployeeList, FirstDayOfWeek, HolidayCount);
                        var datetimesMonth = SelectedDate.ToString("MMM", CultureInfo.InvariantCulture);
                        var datetimesDay = SelectedDate.ToString("dd", CultureInfo.InvariantCulture);
                        var datetimeYear = SelectedDate.ToString("yyyy", CultureInfo.InvariantCulture);
                        DateTime LastDate = SelectedDate.AddDays(6);
                        var datetimelMonth = LastDate.ToString("MMM", CultureInfo.InvariantCulture);
                        var datetimelDay = LastDate.ToString("dd", CultureInfo.InvariantCulture);
                        TitleDate = datetimesMonth + " " + datetimesDay + " - " + datetimelMonth + " " + datetimelDay + ", " + datetimeYear;
                        return Json(new { Datalist = data, Defaultinfo = Default, Startdate = startdate, View = defaultView, Typeval = typeval, Calstatus = statusmodel, DateTitle = TitleDate, Active = ActiveModel });
                    }
                    else if (defaultView == "Monthly")
                    {
                        MonthlyInfoListModel data = monthlyinfo(model.CalendarTaskList, startdate, FirstDayOfWeek);
                        TitleDate = SelectedDate.ToString("MMMM") + " " + SelectedDate.Year;
                        return Json(new { Datalist = data, Defaultinfo = Default, Startdate = startdate, View = defaultView, Typeval = typeval, Calstatus = statusmodel, DateTitle = TitleDate, Active = ActiveModel });
                    }
                }
                return Json(new { Model = model, Defaultinfo = Default, Startdate = startdate, View = defaultView, Typeval = typeval, Calstatus = statusmodel, Active = ActiveModel });
            }
            catch (Exception)
            {
                return Json(new { Model = model, Defaultinfo = Default, Startdate = startdate, View = defaultView, Typeval = typeval, Calstatus = statusmodel, Active = ActiveModel });
            }
        }

        private DailyInfoListModel dailyinfo(List<CustomCalendarAllRecords> model, string CalendarStartTime, string CalendarEndTime, List<CustomCalendarEmployees> EmployeeList, string TopRowEmployee, string startdate, string FirstDayOfWeek, string HolidayCount)
        {
            try
            {
                GlobalSetting ticketcolor = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("CalendarTicketColor");
                string SelectedColor = "";
                if(ticketcolor != null && !string.IsNullOrWhiteSpace(ticketcolor.Value))
                {
                    SelectedColor = ticketcolor.Value;
                }
                string date = DateTime.UtcNow.UTCToClientTime().ToString("yyyy-MM-dd");
                string StartTime = DateTime.Parse(date + " " + CalendarStartTime).ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("en-US"));
                string EndTime = DateTime.Parse(date + " " + CalendarEndTime).ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("en-US"));
                string startingtime = _Util.Facade.GlobalSettingsFacade.GetOnlyStrValueFromGlobalSettingByKey("EmployeeAvailabilityStartTime");
                string endingtime = _Util.Facade.GlobalSettingsFacade.GetOnlyStrValueFromGlobalSettingByKey("EmployeeAvailabilityEndTime");
                int interval = 15;
                var testId = new Guid().ToString();

                int Headinterval = 60;
                List<CalListModel> timelist = new List<CalListModel>();
                DateTime start = Convert.ToDateTime(date + " " + StartTime);
                DateTime end = Convert.ToDateTime(date + " " + EndTime);
                if (end < start)
                {
                    end = end.AddDays(1);
                }
                int count = 1;
                for (DateTime i = start; i < end; i = i.AddMinutes(Headinterval))
                {
                    string time = i.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("en-US"));
                    CalListModel list = new CalListModel() { SL = count, TimeName = time };
                    timelist.Add(list);
                    count += 1;
                }

                DateTime dt;
                bool successfullyParsed = DateTime.TryParse(startdate, out dt);
                if (string.IsNullOrEmpty(startdate) || !successfullyParsed)
                {
                    dt = DateTime.UtcNow.UTCToClientTime();
                }
                CompanyHoliday comHoliday = _Util.Facade.CompanyHolidayFacade.GetCompanyHoliday(dt);
                string ValUser = "";
                List<string> defaultval = new List<string>();
                if (EmployeeList.Count > 0)
                {
                    foreach (var item in EmployeeList)
                    {
                        defaultval.Add(item.UserId.ToString());
                    }
                    ValUser = string.Format("'{0}'", string.Join("','", defaultval.Select(i => i.Replace("'", "''"))));
                }
                var HolidayList = _Util.Facade.PtoFacade.GetAllEmployeesHoliday(ValUser, dt, "Daily");
                List<HolidayEmployeeInfoModel> Holiday = new List<HolidayEmployeeInfoModel>();
                if (HolidayList.Count > 0)
                {
                    Holiday = HolidayChecker(defaultval, HolidayList, dt, "Daily");
                }

                List<CalAppointmentInfoList> AppList = new List<CalAppointmentInfoList>();
                List<OverLoaddingModel> StoreList = new List<OverLoaddingModel>();
                List<TimeNotMatchedTicketInfoCount> ErrorCountList = new List<TimeNotMatchedTicketInfoCount>();
                CalEmpModel employee = new CalEmpModel();
                int TopUserEventCount = 0, sl = 1;
                List<EmployeeOperations> Avalibality = _Util.Facade.EmployeeFacade.GetAllEmployeeOperationByDay(dt.ToString("MM-dd-yyyy"));
                if(model != null && model.Count() > 0)
                {
                    model = model.OrderBy(x => x.EmployeeIntId).ThenBy(x => x.EventStartDate).ToList();
                }
                foreach (var info in model)
                {
                    DateTime CalendarStartDateTime = DateTime.Parse((DateTime.Parse(info.EventDate)).ToString("yyyy-MM-dd") + " " + CalendarStartTime);
                    DateTime CalendarEndDateTime = DateTime.Parse((DateTime.Parse(info.EventDate)).ToString("yyyy-MM-dd") + " " + CalendarEndTime);
                    DateTime EventStartDateTime = DateTime.Parse((DateTime.Parse(info.EventDate)).ToString("yyyy-MM-dd") + " " + (DateTime.Parse(info.EventStartDate)).ToString("hh:mm tt"));
                    if (!string.IsNullOrWhiteSpace(info.EventSubject))
                    {
                        info.EventDisplayType = info.EventDisplayType + " - " + info.EventSubject;
                    }
                    if (info.EventEmployeeGuidId != TopRowEmployee && (EventStartDateTime < CalendarStartDateTime || EventStartDateTime >= CalendarEndDateTime))
                    {
                        TimeNotMatchedTicketInfoCount ticketInfoCount = new TimeNotMatchedTicketInfoCount();
                        ticketInfoCount.EmpId = info.EventEmployeeGuidId;
                        ticketInfoCount.StrTitle = "Date & Time - " + (DateTime.Parse(info.EventDate)).ToString("yyyy-MM-dd") + " " + (DateTime.Parse(info.EventStartDate)).ToString("hh:mm tt") + " - " + (DateTime.Parse(info.EventEndDate)).ToString("hh:mm tt") + "&#013;Ticket# " + info.EventLeadId.ToString() + "&#013;Booking# " + info.BookingId + "&#013;Type - " + info.EventDisplayType + "&#013;Status - " + info.EventStatus;
                        ticketInfoCount.ErrorCount = 1;
                        ticketInfoCount.StrEditList = string.Format("<span class='cus-anchor' onclick='editErrorTicket({0})'>{0}</span>", info.EventLeadId);
                        ErrorCountList.Add(ticketInfoCount);
                        continue;
                    }
                    CalAppointmentInfoList CAI = new CalAppointmentInfoList();
                    string[] infoAddress = info.EventLocate.Split(',');
                    string city = info.EventLocate.Split(',')[0];
                    string zip = "";
                    if (infoAddress.Length == 2)
                    {
                        zip = infoAddress[1];
                    }                    
                    if (string.IsNullOrWhiteSpace(city) && string.IsNullOrWhiteSpace(zip))
                    {
                        info.EventLocate = "";
                    }
                    else if (string.IsNullOrWhiteSpace(city))
                    {
                        info.EventLocate = zip;
                    }
                    else if (string.IsNullOrWhiteSpace(zip))
                    {
                        info.EventLocate = city;
                    }
                    CAI.Address = info.EventLocate;
                    CAI.IsCalled = info.IsCalled;
                    CAI.CellNo = info.EventPhone;
                    CAI.TicketAddress = info.EventTicketAddress;
                    CAI.StreetAddress = info.EventStreet;
                    CAI.AppointmentDate = (DateTime.Parse(info.EventDate)).ToString("yyyy-MM-dd");
                    CAI.AppointmentEndTime = (DateTime.Parse(info.EventEndDate)).ToString("hh:mm tt");
                    CAI.AppointmentId = info.EventAppointmentId;
                    CAI.AppointmentStartTime = (DateTime.Parse(info.EventStartDate)).ToString("hh:mm tt");
                    CAI.AppointmentType = info.EventType;
                    CAI.CustomerId = info.EventCustomerId;
                    CAI.Name = info.EventCustomerName;
                    CAI.EmpId = info.EmployeeIntId;
                    CAI.TicketId = info.EventLeadId.ToString();
                    CAI.UserId = info.EventEmployeeGuidId;
                    CAI.IsAllDay = info.EventAllDay;
                    CAI.CustomerIntId = info.CustomerIntId;
                    CAI.DayStartTime = StartTime;
                    CAI.Status = info.EventStatus;
                    CAI.Subject = info.EventSubject;
                    CAI.DayEndTime = EndTime;
                    CAI.BookingId = info.BookingId;
                    CAI.TicketAmount = info.TicketAmount;
                    CAI.EmployeeName = info.EmployeeName;
                    CAI.TicketTypeDisplayText = info.EventDisplayType;
                    if (SelectedColor == "Employee")
                    {
                        CAI.BGColor = !string.IsNullOrWhiteSpace(info.EmpColor) ? "#" + info.EmpColor : "#ccc";
                    }
                    else if (SelectedColor == "TicketStatus")
                    {
                        CAI.BGColor = !string.IsNullOrWhiteSpace(info.StatusColor) ? info.StatusColor : "#ccc";
                    }
                    else
                    {
                        CAI.BGColor = !string.IsNullOrWhiteSpace(info.EventColor) ? "#" + info.EventColor : "#ccc";
                    }
                    CAI.AdditionalMember = info.EventAdditionalMember;
                    CAI.SL = sl;
                    if (!string.IsNullOrEmpty(info.StatusColor) && !string.IsNullOrWhiteSpace(info.StatusColor))
                    {
                        CAI.StatusColor = info.StatusColor;
                    }
                    else
                    {
                        CAI.StatusColor = "#454545";
                    }
                    CAI.LeftIcon = info.EventIcon;

                    List<string> dl = new List<string>();
                    List<string> Startdiv = new List<string>();
                    if (info.EventAllDay == false)
                    {
                        DateTime starttime = Convert.ToDateTime(info.EventStartDate);
                        DateTime endtime = Convert.ToDateTime(info.EventEndDate);
                        if (endtime < starttime)
                        {
                            endtime = endtime.AddDays(1);
                        }

                        for (DateTime i = starttime; i < endtime; i = i.AddMinutes(interval))
                        {
                            string stime = i.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("en-US"));
                            dl.Add(stime);
                        }
                        DateTime st = Convert.ToDateTime(info.EventDate + " " + StartTime);
                        DateTime et = Convert.ToDateTime(info.EventStartDate);
                        if (et < st)
                        {
                            et = et.AddDays(1);
                        }
                        for (DateTime i = st; i < et; i = i.AddMinutes(interval))
                        {
                            string time = i.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("en-US"));
                            Startdiv.Add(time);
                        }
                    }
                    CAI.Time = dl;
                    CAI.StartDivCount = Startdiv.Count;
                    CAI.EndDivCount = dl.Count;
                    string AddressInfo = "";
                    string AddressInfo2 = "";
                    if (!string.IsNullOrWhiteSpace(CAI.Address))
                    {
                        AddressInfo = "<br><a href='javascript:void(0)' class='cus-anchor' onclick='GetDirection(event)' data-address = '" + CAI.StreetAddress + ' ' + CAI.Address + "' id = 'getDirection'  >" + CAI.StreetAddress + "<br>" + CAI.Address + "</a>";
                        AddressInfo2 = "<br><a href='javascript:void(0)' onclick='GetDirection(event)' class='cus-anchor' data-address = '" + CAI.StreetAddress + ' ' + CAI.Address + "' id = 'getDirection'  >" + CAI.StreetAddress + "<br>" + CAI.Address + "</a>";
                    }
                    string TitleAmt = string.Empty, PopupAmt = string.Empty;
                    if(info.TicketAmount > 0) {
                        TitleAmt = "Amount: $" + CAI.TicketAmount.ToString("N2") + "&#013;";
                        PopupAmt = "Amount: $" + CAI.TicketAmount.ToString("N2") + "<br />";
                    }
                    if (info.EventAllDay)
                    {
                        if (!string.IsNullOrWhiteSpace(CAI.BookingId))
                        {
                            int BkId = 0;
                            int.TryParse(CAI.BookingId.Remove(0, 3), out BkId);
                            CAI.TitleString = "Ticket# " + CAI.TicketId + "&#013;" + TitleAmt + CAI.EmployeeName + "&#013;" + CAI.TicketTypeDisplayText + "&#013;" + CAI.Status + "&#013;" + (DateTime.Parse(CAI.AppointmentDate)).ToString("M/d/yy") + "-FullDay&#013;" + CAI.BookingId + "&#013;" + CAI.Name + "&#013;" + CAI.StreetAddress + "&#013;" + CAI.Address;
                            CAI.PopupDetails = "<p>Ticket# " + CAI.TicketId + "<br />" + PopupAmt + CAI.EmployeeName + "<br>" + CAI.TicketTypeDisplayText + "<br>" + CAI.Status + "<br>" + (DateTime.Parse(CAI.AppointmentDate)).ToString("M/d/yy") + "-FullDay<br><a class='cus-anchor' onclick='OpenBkById(" + BkId + "," + CAI.CustomerIntId + ")'>" + CAI.BookingId + "</a><br><a class='cus-anchor' href='/Customer/CustomerDetail/?id=" + CAI.CustomerIntId + "' target='_blank'>" + CAI.Name + "</a>" + AddressInfo;
                        }
                        else
                        {
                            CAI.TitleString = "Ticket# " + CAI.TicketId + "&#013;" + TitleAmt + CAI.EmployeeName + "&#013;" + CAI.TicketTypeDisplayText + "&#013;" + CAI.Status + "&#013;" + (DateTime.Parse(CAI.AppointmentDate)).ToString("M/d/yy") + "-FullDay&#013;" + CAI.Name + "&#013;" + CAI.StreetAddress + "&#013;" + CAI.Address;
                            CAI.PopupDetails = "<p>Ticket# " + CAI.TicketId + "<br />" + PopupAmt + CAI.EmployeeName + "<br>" + CAI.TicketTypeDisplayText + "<br>" + CAI.Status + "<br>" + (DateTime.Parse(CAI.AppointmentDate)).ToString("M/d/yy") + "-FullDay<br><a class='cus-anchor' href='/Customer/CustomerDetail/?id=" + CAI.CustomerIntId + "' target='_blank'>" + CAI.Name + "</a>" + AddressInfo2;
                        }

                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(CAI.BookingId))
                        {
                            int BkId = 0;
                            int.TryParse(CAI.BookingId.Remove(0, 3), out BkId);
                            CAI.TitleString = "Ticket# " + CAI.TicketId + "&#013;" + TitleAmt + CAI.EmployeeName + "&#013;" + CAI.TicketTypeDisplayText + "&#013;" + CAI.Status + "&#013;" + (DateTime.Parse(CAI.AppointmentDate)).ToString("M/d/yy ") + (DateTime.Parse(info.EventStartDate)).ToString("h:mm tt") + "-" + (DateTime.Parse(info.EventEndDate)).ToString("h:mm tt") + "&#013;" + CAI.BookingId + "&#013;" + CAI.Name + "&#013;" + CAI.StreetAddress + "&#013;" + CAI.Address;
                            CAI.PopupDetails = "<p>Ticket# " + CAI.TicketId + "<br />" + PopupAmt + CAI.EmployeeName + "<br>" + CAI.TicketTypeDisplayText + "<br>" + CAI.Status + "<br>" + (DateTime.Parse(CAI.AppointmentDate)).ToString("M/d/yy ") + (DateTime.Parse(info.EventStartDate)).ToString("h:mm tt") + "-" + (DateTime.Parse(info.EventEndDate)).ToString("h:mm tt") + "<br><a class='cus-anchor' onclick='OpenBkById(" + BkId + "," + CAI.CustomerIntId + ")'>" + CAI.BookingId + "</a><br><a class='cus-anchor' href='/Customer/CustomerDetail/?id=" + CAI.CustomerIntId + "' target='_blank'>" + CAI.Name + "</a>" + AddressInfo;
                        }
                        else
                        {
                            CAI.TitleString = "Ticket# " + CAI.TicketId + "&#013;" + TitleAmt + CAI.EmployeeName + "&#013;" + CAI.TicketTypeDisplayText + "&#013;" + CAI.Status + "&#013;" + (DateTime.Parse(CAI.AppointmentDate)).ToString("M/d/yy ") + (DateTime.Parse(info.EventStartDate)).ToString("h:mm tt") + "-" + (DateTime.Parse(info.EventEndDate)).ToString("h:mm tt") + "&#013;" + CAI.Name + "&#013;" + CAI.StreetAddress + "&#013;" + CAI.Address;
                            CAI.PopupDetails = "<p>Ticket# " + CAI.TicketId + "<br />" + PopupAmt + CAI.EmployeeName + "<br>" + CAI.TicketTypeDisplayText + "<br>" + CAI.Status + "<br>" + (DateTime.Parse(CAI.AppointmentDate)).ToString("M/d/yy ") + (DateTime.Parse(info.EventStartDate)).ToString("h:mm tt") + "-" + (DateTime.Parse(info.EventEndDate)).ToString("h:mm tt") + "<br><a class='cus-anchor' href='/Customer/CustomerDetail/?id=" + CAI.CustomerIntId + "' target='_blank'>" + CAI.Name + "</a>" + AddressInfo2;
                        }

                    }
                    if (!string.IsNullOrWhiteSpace(TopRowEmployee))
                    {
                        if (info.EventEmployeeGuidId == TopRowEmployee)
                        {
                            TopUserEventCount += 1;
                        }
                    }
                    List<CalAppointmentInfoList> FilterInfoList = AppList.Where(x => x.EmpId == info.EmployeeIntId).ToList();

                    if (FilterInfoList != null && FilterInfoList.Count > 0 && info.EventEmployeeGuidId != TopRowEmployee && !info.EventAllDay)
                    {
                        foreach (CalAppointmentInfoList filter in FilterInfoList)
                        {
                            DateTime OldStartTime = new DateTime();
                            DateTime OldEndTime = new DateTime();
                            DateTime NewStartTime = new DateTime();
                            DateTime NewEndTime = new DateTime();
                            if (!filter.IsAllDay)
                            {
                                DateTime.TryParse(filter.AppointmentDate + " " + filter.AppointmentStartTime, out OldStartTime);
                                DateTime.TryParse(filter.AppointmentDate + " " + filter.AppointmentEndTime, out OldEndTime);
                            }
                            DateTime.TryParse(info.EventStartDate, out NewStartTime);
                            DateTime.TryParse(info.EventEndDate, out NewEndTime);
                            if (OldStartTime != new DateTime() && OldEndTime != new DateTime() && NewStartTime != new DateTime() && NewEndTime != new DateTime() && (OldStartTime <= NewStartTime && NewStartTime < OldEndTime) || (OldStartTime < NewEndTime && NewEndTime <= OldEndTime))
                            {
                                List<OverLoaddingModel> InfoList = StoreList.Where(x => x.SL == filter.SL).ToList();
                                if (InfoList == null || InfoList.Count == 0)
                                {

                                    StoreList.Add(new OverLoaddingModel()
                                    {
                                        SL = filter.SL,
                                        EmpId = filter.EmpId,
                                        OverLoad = 0,
                                        StartTime = OldStartTime,
                                        EndTime = OldEndTime
                                    });
                                    StoreList.Add(new OverLoaddingModel()
                                    {
                                        SL = CAI.SL,
                                        EmpId = CAI.EmpId,
                                        OverLoad = 1,
                                        StartTime = NewStartTime,
                                        EndTime = NewEndTime
                                    });
                                    CAI.OverLodding = 1;
                                }
                                else
                                {
                                    int i = 0;
                                    List<OverLoaddingModel> List = StoreList.Where(x => x.EmpId == CAI.EmpId && x.SL != CAI.SL).ToList();
                                    if (List != null && List.Count > 0)
                                    {
                                        List<OverLoaddingModel> flag = new List<OverLoaddingModel>();
                                        foreach (OverLoaddingModel item in List)
                                        {
                                            if ((item.StartTime <= NewStartTime && NewStartTime < item.EndTime) || (item.StartTime < NewEndTime && NewEndTime <= item.EndTime))
                                            {
                                                flag.Add(item);
                                            }
                                        }
                                        if (flag != null && flag.Count > 0)
                                        {
                                            flag = flag.GroupBy(x => x.OverLoad).Select(y => y.FirstOrDefault()).OrderBy(z => z.OverLoad).ToList();
                                            bool loop = true;
                                            do
                                            {
                                                if (flag.Count > i && i == flag[i].OverLoad)
                                                {
                                                    i++;
                                                }
                                                else
                                                {
                                                    CAI.OverLodding = i;
                                                    loop = false;
                                                }
                                            }
                                            while (loop);
                                        }
                                        else
                                        {
                                            i = filter.OverLodding + 1;
                                            CAI.OverLodding = i;
                                        }
                                    }
                                    StoreList.Add(new OverLoaddingModel()
                                    {
                                        SL = CAI.SL,
                                        EmpId = CAI.EmpId,
                                        OverLoad = i,
                                        StartTime = NewStartTime,
                                        EndTime = NewEndTime
                                    });
                                }
                            }
                        }

                    }
                    AppList.Add(CAI);
                    sl++;
                }
                employee.TaskCount = TopUserEventCount;
                #region Time Not Matching Error Count
                List<TimeNotMatchedTicketInfoCount> ErrorList = new List<TimeNotMatchedTicketInfoCount>();
                if (ErrorCountList.Count > 0)
                {
                    ErrorList = ErrorCountList.GroupBy(x => x.EmpId)
                          .Select(y => new TimeNotMatchedTicketInfoCount
                          {
                              EmpId = y.Key,
                              ErrorCount = y.Sum(a => a.ErrorCount),
                              StrTitle = string.Join("&#013; - - - - - - - - - - - - - &#013;", from e in y select e.StrTitle),
                              StrEditList = string.Join("<br />", from e in y select e.StrEditList)
                          }).ToList();
                }
                #endregion
                List<CalEmpModel> EmpList = new List<CalEmpModel>();
                string day = dt.ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
                string EmpIds = string.Join(",", EmployeeList.Select(x => "'" + x.UserId.ToString() + "'"));
                
                #region Weekly Holiday Calculation
                string WeekEndDay = "";
                if (FirstDayOfWeek.ToLower() == "saturday")
                {

                    if (HolidayCount == "0") { WeekEndDay = ""; }
                    else if (HolidayCount == "1") { WeekEndDay = "friday"; }
                    else if (HolidayCount == "2") { WeekEndDay = "friday,thursday"; }
                    else if (HolidayCount == "3") { WeekEndDay = "friday,thursday,wednesday"; }
                    else if (HolidayCount == "4") { WeekEndDay = "friday,thursday,wednesday,tuesday"; }
                    else if (HolidayCount == "5") { WeekEndDay = "friday,thursday,wednesday,tuesday,monday"; }
                    else if (HolidayCount == "6") { WeekEndDay = "friday,thursday,wednesday,tuesday,monday,sunday"; }
                }
                else if (FirstDayOfWeek.ToLower() == "sunday")
                {
                    if (HolidayCount == "0") { WeekEndDay = ""; }
                    else if (HolidayCount == "1") { WeekEndDay = "saturday"; }
                    else if (HolidayCount == "2") { WeekEndDay = "saturday,friday"; }
                    else if (HolidayCount == "3") { WeekEndDay = "saturday,friday,thursday"; }
                    else if (HolidayCount == "4") { WeekEndDay = "saturday,friday,thursday,wednesday"; }
                    else if (HolidayCount == "5") { WeekEndDay = "saturday,friday,thursday,wednesday,tuesday"; }
                    else if (HolidayCount == "6") { WeekEndDay = "saturday,friday,thursday,wednesday,tuesday,monday"; }
                }
                else if (FirstDayOfWeek.ToLower() == "monday")
                {
                    if (HolidayCount == "0") { WeekEndDay = ""; }
                    else if (HolidayCount == "1") { WeekEndDay = "sunday"; }
                    else if (HolidayCount == "2") { WeekEndDay = "sunday,saturday"; }
                    else if (HolidayCount == "3") { WeekEndDay = "sunday,saturday,friday"; }
                    else if (HolidayCount == "4") { WeekEndDay = "sunday,saturday,friday,thursday"; }
                    else if (HolidayCount == "5") { WeekEndDay = "sunday,saturday,friday,thursday,wednesday"; }
                    else if (HolidayCount == "6") { WeekEndDay = "sunday,saturday,friday,thursday,wednesday,tuesday"; }
                }
                #endregion
                foreach (var item in EmployeeList)
                {
                    CalEmpModel emp = new CalEmpModel();
                    emp.HolidayStatus = "";
                    string grpName = item.GroupName;
                    if (!string.IsNullOrWhiteSpace(item.GroupName))
                    {
                        string lastlatter = item.GroupName.Substring(item.GroupName.Length - 1);
                        if(lastlatter.ToLower() == "s")
                        {
                            grpName = item.GroupName.Remove(item.GroupName.Length - 1);
                        }
                    }
                    emp.GroupName = grpName;
                    if (WeekEndDay != "")
                    {
                        string days = dt.ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
                        string[] dayArr = WeekEndDay.Split(',');
                        if (dayArr.Length > 0)
                        {
                            foreach (string da in dayArr)
                            {
                                if (da == days.ToLower())
                                {
                                    emp.HolidayStatus = "WeekEnd,";
                                }
                            }
                        }
                    }
                    if (Holiday.Count > 0)
                    {
                        foreach (var hd in Holiday)
                        {
                            if (hd.StartDate == dt.ToString("yyyy-MM-dd") && hd.UserId.ToString() == item.UserId)
                            {
                                if (hd.Type == "FullDay")
                                {
                                    emp.HolidayStatus = "FullDay,";
                                }
                                else if (hd.Type == "HalfDay")
                                {
                                    double fullday = timelist.Count;
                                    string floatday = (fullday / 2).ToString("N2");
                                    int halfday = Convert.ToInt32(floatday.Split('.')[0]);
                                    string decimalval = floatday.Split('.')[1];
                                    if (int.Parse(decimalval) > 0)
                                    {
                                        halfday = halfday + 1;
                                    }
                                    emp.HolidayStatus = "HalfDay," + halfday;
                                }
                                else if (hd.Type == "CustomTime")
                                {
                                    int customStart = 0, customEnd = 0;
                                    foreach (var time in timelist)
                                    {
                                        string hhmm = time.TimeName.Split(' ')[0];
                                        string tt = time.TimeName.Split(' ')[1];
                                        int hh = int.Parse(hhmm.Split(':')[0]);
                                        if (tt.ToLower() == "pm" && hh < 12)
                                        {
                                            hh += 12;
                                        }
                                        int hdStartHH = 0, hdEndHH = 0;
                                        if (!string.IsNullOrEmpty(hd.EndTime) && !string.IsNullOrEmpty(hd.EndTime) && int.TryParse(hd.StartTime.Substring(0, 2), out hdStartHH) && int.TryParse(hd.EndTime.Substring(0, 2), out hdEndHH))
                                        {
                                            if (hd.StartTime.ToLower().Contains("pm") && hdStartHH < 12)
                                            {
                                                hdStartHH += 12;
                                            }
                                            if (hd.EndTime.ToLower().Contains("pm") && hdEndHH < 12)
                                            {
                                                hdEndHH += 12;
                                            }
                                        }
                                        if (hdStartHH == hh)
                                        {
                                            customStart = time.SL;
                                        }
                                        if (hdEndHH == hh)
                                        {
                                            customEnd = time.SL;
                                        }
                                    }
                                    emp.HolidayStatus = "CustomTime," + customStart + "," + customEnd;
                                }
                            }
                        }
                    }

                    #region Employee Availablity
                    int AvaileStart = 0, AvaileEnd = 0;
                    if (Avalibality != null && Avalibality.Count() > 0 && item.UserId != "22222222-2222-2222-2222-222222222222")
                    {
                        EmployeeOperations tempavable = Avalibality.Where(x => x.EmployeeId.ToString() == item.UserId.ToString()).FirstOrDefault();
                        if (tempavable == null)
                        {
                            int AvaStartHH = !string.IsNullOrWhiteSpace(startingtime) && startingtime.Contains(":") ? int.Parse(startingtime.Split(':')[0]) : 9;
                            int AvaEndHH = !string.IsNullOrWhiteSpace(endingtime) && endingtime.Contains(":") ? int.Parse(endingtime.Split(':')[0]) : 17;
                            foreach (var time in timelist)
                            {
                                string hhmm = time.TimeName.Split(' ')[0];
                                string tt = time.TimeName.Split(' ')[1];
                                int hh = 0;
                                if (int.TryParse(hhmm.Split(':')[0], out hh) && tt.ToLower() == "pm" && hh < 12)
                                {
                                    hh += 12;
                                }
                                if (AvaStartHH == hh)
                                {
                                    AvaileStart = time.SL;
                                }
                                if (AvaEndHH == hh)
                                {
                                    AvaileEnd = time.SL;
                                }
                            }
                        }
                        else if (tempavable != null && tempavable.IsDayOff.HasValue && tempavable.IsDayOff.Value)
                        {
                            AvaileStart = timelist.Count();
                        }
                        else
                        {
                            int AvaStartHH = !string.IsNullOrWhiteSpace(tempavable.OperationStartTime) && tempavable.OperationStartTime.Contains(":") ? int.Parse(tempavable.OperationStartTime.Split(':')[0]) : 0;
                            int AvaEndHH = !string.IsNullOrWhiteSpace(tempavable.OperationEndTime) && tempavable.OperationEndTime.Contains(":") ? int.Parse(tempavable.OperationEndTime.Split(':')[0]) : 0;
                            foreach (var time in timelist)
                            {
                                string hhmm = time.TimeName.Split(' ')[0];
                                string tt = time.TimeName.Split(' ')[1];
                                int hh = 0;
                                if (int.TryParse(hhmm.Split(':')[0], out hh) && tt.ToLower() == "pm" && hh < 12)
                                {
                                    hh += 12;
                                }
                                if (AvaStartHH == hh)
                                {
                                    AvaileStart = time.SL;
                                }
                                if (AvaEndHH == hh)
                                {
                                    AvaileEnd = time.SL;
                                }
                            }
                        }                       
                    }
                    else if(Avalibality == null || Avalibality.Count == 0)
                    {
                        int AvaStartHH = !string.IsNullOrWhiteSpace(startingtime) && startingtime.Contains(":") ? int.Parse(startingtime.Split(':')[0]) : 9;
                        int AvaEndHH = !string.IsNullOrWhiteSpace(endingtime) && endingtime.Contains(":") ? int.Parse(endingtime.Split(':')[0]) : 17;
                        foreach (var time in timelist)
                        {
                            string hhmm = time.TimeName.Split(' ')[0];
                            string tt = time.TimeName.Split(' ')[1];
                            int hh = 0;
                            if (int.TryParse(hhmm.Split(':')[0], out hh) && tt.ToLower() == "pm" && hh < 12)
                            {
                                hh += 12;
                            }
                            if (AvaStartHH == hh)
                            {
                                AvaileStart = time.SL;
                            }
                            if (AvaEndHH == hh)
                            {
                                AvaileEnd = time.SL;
                            }
                        }
                    }
                    emp.AvailablityTime = "AvailableStart," + AvaileStart + "," + AvaileEnd;
                    #endregion

                    #region Company Holi day
                    if (comHoliday != null)
                    {
                        emp.IsCompanyHoliday = true;
                    }
                    #endregion
                    if (!string.IsNullOrWhiteSpace(TopRowEmployee))
                    {
                        if (item.UserId == TopRowEmployee)
                        {
                            employee.EmpGuidId = item.UserId;
                            employee.EmpIntId = item.Id;
                            employee.EmpName = item.ResourceName;
                            employee.GroupName = grpName;
                        }
                        else
                        {
                            if (ErrorList.Count > 0)
                            {
                                foreach (var error in ErrorList)
                                {
                                    if (error.EmpId == item.UserId)
                                    {
                                        emp.ErrorCount = error.ErrorCount;
                                        emp.ErrorTitleStatus = error.StrTitle;
                                        emp.ErrorTicketIdEditList = error.StrEditList;
                                    }
                                }
                            }
                            emp.EmpGuidId = item.UserId;
                            emp.EmpName = item.ResourceName;
                            emp.EmpIntId = item.Id;
                            EmpList.Add(emp);
                        }
                    }
                    else
                    {
                        if (ErrorList.Count > 0)
                        {
                            foreach (var error in ErrorList)
                            {
                                if (error.EmpId == item.UserId)
                                {
                                    emp.ErrorCount = error.ErrorCount;
                                    emp.ErrorTitleStatus = error.StrTitle;
                                    emp.ErrorTicketIdEditList = error.StrEditList;
                                }
                            }
                        }
                        emp.EmpGuidId = item.UserId;
                        emp.EmpName = item.ResourceName;
                        emp.EmpIntId = item.Id;
                        EmpList.Add(emp);
                    }

                }
                var result = new DailyInfoListModel() { AppList = AppList, EmpList = EmpList, Timelist = timelist, SystemUser = employee };
                return result;
            }
            catch (Exception ex)
            {
                var result = new DailyInfoListModel();
                return result;
            }
        }
        private WeeklyInfoListModel weeklyinfo(List<CustomCalendarAllRecords> model, string date, List<CustomCalendarEmployees> EmployeeList, string FirstDayOfWeek, string HolidayCount)
        {
            try
            {
                DateTime dt;
                bool successfullyParsed = DateTime.TryParse(date, out dt);
                if (string.IsNullOrEmpty(date) || !successfullyParsed)
                {
                    dt = DateTime.UtcNow.UTCToClientTime();
                }

                string ValUser = "";
                List<string> defaultval = new List<string>();
                if (EmployeeList.Count > 0)
                {
                    foreach (var item in EmployeeList)
                    {
                        defaultval.Add(item.UserId.ToString());
                    }
                    ValUser = string.Format("'{0}'", string.Join("','", defaultval.Select(i => i.Replace("'", "''"))));
                }
                List<HolidayEmployeeInfoModel> Holiday = new List<HolidayEmployeeInfoModel>();
                var HolidayList = _Util.Facade.PtoFacade.GetAllEmployeesHoliday(ValUser, dt, "Weekly");
                if (HolidayList.Count > 0)
                {
                    Holiday = HolidayChecker(defaultval, HolidayList, dt, "Weekly");
                }
                List<CalListModel> timelist = new List<CalListModel>();
                string WeekEndDay = "";
                if (FirstDayOfWeek.ToLower() == "saturday")
                {

                    if (HolidayCount == "0") { WeekEndDay = ""; }
                    else if (HolidayCount == "1") { WeekEndDay = "friday"; }
                    else if (HolidayCount == "2") { WeekEndDay = "friday,thursday"; }
                    else if (HolidayCount == "3") { WeekEndDay = "friday,thursday,wednesday"; }
                    else if (HolidayCount == "4") { WeekEndDay = "friday,thursday,wednesday,tuesday"; }
                    else if (HolidayCount == "5") { WeekEndDay = "friday,thursday,wednesday,tuesday,monday"; }
                    else if (HolidayCount == "6") { WeekEndDay = "friday,thursday,wednesday,tuesday,monday,sunday"; }
                }
                else if (FirstDayOfWeek.ToLower() == "sunday")
                {
                    if (HolidayCount == "0") { WeekEndDay = ""; }
                    else if (HolidayCount == "1") { WeekEndDay = "saturday"; }
                    else if (HolidayCount == "2") { WeekEndDay = "saturday,friday"; }
                    else if (HolidayCount == "3") { WeekEndDay = "saturday,friday,thursday"; }
                    else if (HolidayCount == "4") { WeekEndDay = "saturday,friday,thursday,wednesday"; }
                    else if (HolidayCount == "5") { WeekEndDay = "saturday,friday,thursday,wednesday,tuesday"; }
                    else if (HolidayCount == "6") { WeekEndDay = "saturday,friday,thursday,wednesday,tuesday,monday"; }
                }
                else if (FirstDayOfWeek.ToLower() == "monday")
                {
                    if (HolidayCount == "0") { WeekEndDay = ""; }
                    else if (HolidayCount == "1") { WeekEndDay = "sunday"; }
                    else if (HolidayCount == "2") { WeekEndDay = "sunday,saturday"; }
                    else if (HolidayCount == "3") { WeekEndDay = "sunday,saturday,friday"; }
                    else if (HolidayCount == "4") { WeekEndDay = "sunday,saturday,friday,thursday"; }
                    else if (HolidayCount == "5") { WeekEndDay = "sunday,saturday,friday,thursday,wednesday"; }
                    else if (HolidayCount == "6") { WeekEndDay = "sunday,saturday,friday,thursday,wednesday,tuesday"; }
                }

                //DateTime dt = Convert.ToDateTime(date);
                for (int i = 1; i < 8; i++)
                {
                    List<HolidayEmployeeInfoModel> PtoList = new List<HolidayEmployeeInfoModel>();
                    if (Holiday.Count > 0)
                    {
                        foreach (var item in Holiday)
                        {
                            if (item.StartDate == dt.ToString("yyyy-MM-dd"))
                            {
                                PtoList.Add(item);
                            }
                        }
                    }
                    string day = dt.ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
                    string[] dayArr = WeekEndDay.Split(',');
                    string WeeklyLeave = "";
                    if (dayArr.Length > 0)
                    {
                        foreach (string da in dayArr)
                        {
                            if (da == day.ToLower())
                            {
                                WeeklyLeave = "Week End Off";
                            }
                        }
                    }
                    string todate = dt.ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("en-US"));
                    var adddate = new CalListModel() { SL = i, DateName = todate, TimeName = day, WeekEnd = WeeklyLeave, Holidays = PtoList };
                    timelist.Add(adddate);
                    dt = dt.AddDays(1);
                }
                //  var collection = model.ListFollowUpSchedule.GroupBy(item => item.EventDate).Select(group => new { keyId = group.Key, Items = group.ToList() }).ToList();
                var testId = new Guid().ToString();
                List<SelectListItem> TicketTypeList = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Where(x => x.DataValue != "-1" && x.AlterDisplayText1 != null && x.AlterDisplayText1 != "" && x.AlterDisplayText1 == "True").OrderBy(x => x.DisplayText).ToList().Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();
                List<CalAppointmentInfoList> AppList = new List<CalAppointmentInfoList>();
                List<CalEmpModel> EmpList = new List<CalEmpModel>();
                foreach (var item in EmployeeList)
                {
                    CalEmpModel emp = new CalEmpModel();
                    emp.EmpGuidId = item.UserId;
                    emp.EmpName = item.ResourceName;
                    emp.EmpIntId = item.Id;
                    EmpList.Add(emp);
                }
                List<ServiceCount> SCList = new List<ServiceCount>();
                if (model != null)
                {

                    foreach (var info in model)
                    {
                        CalAppointmentInfoList CAI = new CalAppointmentInfoList();
                        ServiceCount SC = new ServiceCount();
                        CAI.Address = info.EventLocate;
                        CAI.StreetAddress = info.EventStreet;
                        CAI.AppointmentDate = (DateTime.Parse(info.EventDate)).ToString("yyyy-MM-dd");
                        CAI.AppointmentEndTime = (DateTime.Parse(info.EventEndDate)).ToString("hh:mm tt");
                        CAI.AppointmentId = info.EventAppointmentId;
                        CAI.AppointmentStartTime = (DateTime.Parse(info.EventStartDate)).ToString("hh:mm tt");
                        CAI.AppointmentType = info.EventType;
                        CAI.CustomerId = info.EventCustomerId;
                        CAI.Name = info.EventCustomerName;
                        CAI.EmpId = info.EmployeeIntId;
                        CAI.TicketId = info.EventLeadId.ToString();
                        CAI.UserId = info.EventEmployeeGuidId;
                        CAI.IsAllDay = info.EventAllDay;
                        CAI.Status = info.EventStatus;
                        CAI.BookingId = info.BookingId;
                        CAI.EmployeeName = info.EmployeeName;
                        CAI.TicketTypeDisplayText = info.EventDisplayType;
                        CAI.AdditionalMember = info.EventAdditionalMember;
                        if (SCList.Count > 0)
                        {
                            int num = SCList.Count;
                            var edt = info.EventDate;
                            var val = info.EventType;
                            bool x = false;
                            for (int n = 0; n < num; n++)
                            {
                                if (edt == SCList[n].ServiceDate)
                                {
                                    if (val == SCList[n].ServiceName && info.EventEmployeeGuidId == SCList[n].EmpId)
                                    {
                                        SCList[n].CountNumber += 1;
                                        x = true;
                                    }
                                }
                            }
                            if (!x)
                            {
                                SC.CountNumber = 1;
                                SC.ServiceName = info.EventType;
                                SC.ServiceDate = info.EventDate;
                                SC.EmpId = info.EventEmployeeGuidId;
                                SCList.Add(SC);
                            }
                        }
                        else
                        {
                            SC.CountNumber = 1;
                            SC.ServiceName = info.EventType;
                            SC.ServiceDate = info.EventDate;
                            SC.EmpId = info.EventEmployeeGuidId;
                            SCList.Add(SC);
                        }
                        AppList.Add(CAI);
                    }
                }
                List<DailyInfoShow> showList = new List<DailyInfoShow>();
                List<string> TypeList = new List<string>();
                TotalServiceInfoCountCollection TSICC = new TotalServiceInfoCountCollection();
                List<DailyServiceInfoCountList> DAilyList = new List<DailyServiceInfoCountList>();
                List<WeeklyServiceInfoCount> WeeklyserviceCountList = new List<WeeklyServiceInfoCount>();
                for (int d = 0; d < timelist.Count; d++)
                {
                    List<DailyServiceInfoCount> dsicList = new List<DailyServiceInfoCount>();
                    string daily = (DateTime.Parse(timelist[d].DateName)).ToString("yyyy-MM-dd");
                    var daynumber = SCList.Where(x => x.ServiceDate == daily).Select(y => y.CountNumber).ToList();
                    for (int st = 0; st < TicketTypeList.Count; st++)
                    {
                        DailyServiceInfoCount dsic = new DailyServiceInfoCount();
                        var type = TicketTypeList[st].Value;
                        if (daily == (DateTime.Parse(timelist[0].DateName)).ToString("yyyy-MM-dd"))
                        {
                            TypeList.Add(type);
                        }
                        var servicedaynumber = SCList.Where(x => x.ServiceDate == daily && x.ServiceName == type).Select(y => y.CountNumber).ToList();
                        if (servicedaynumber.Count > 0)
                        {
                            dsic.Service = type;
                            dsic.ServiceName = TicketTypeList[st].Text;
                            dsic.SCount = servicedaynumber.Sum();
                            dsicList.Add(dsic);
                        }
                    }
                    var avg = SCList.Where(x => x.ServiceDate == daily).Select(y => y.EmpId).ToList();
                    var resultList = new DailyServiceInfoCountList() { DailyTotal = daynumber.Sum(), DailyServiceTotal = dsicList, AppDate = (DateTime.Parse(daily)).ToString("dd-MMM-yyyy"), DailyAvgCount = avg.Count };
                    DAilyList.Add(resultList);
                }
                List<WeeklyServiceInfoCountList> weeklyServices = new List<WeeklyServiceInfoCountList>();
                List<EmpServiceInfoCount> esicList = new List<EmpServiceInfoCount>();
                for (int e = 0; e < EmpList.Count; e++)
                {
                    List<WeeklyServiceInfoCount> wsicList = new List<WeeklyServiceInfoCount>();
                    var eid = EmpList[e].EmpGuidId;
                    var weeknumber = SCList.Where(x => x.EmpId == eid).Select(y => y.CountNumber).ToList();
                    for (int st = 0; st < TicketTypeList.Count; st++)
                    {
                        WeeklyServiceInfoCount dsic = new WeeklyServiceInfoCount();
                        var type = TicketTypeList[st].Value;
                        var serviceweeknumber = SCList.Where(x => x.EmpId == eid && x.ServiceName == type).Select(y => y.CountNumber).ToList();
                        if (serviceweeknumber.Count > 0)
                        {
                            dsic.SCount = serviceweeknumber.Sum();
                            dsic.Service = type;
                            dsic.ServiceName = TicketTypeList[st].Text;
                            wsicList.Add(dsic);
                            WeeklyserviceCountList.Add(dsic);
                        }
                    }
                    var avg = SCList.Where(x => x.EmpId == eid).Select(y => y.ServiceDate).ToList();
                    var resultList = new WeeklyServiceInfoCountList() { WeeklyTotal = weeknumber.Sum(), WeeklyServiceTotal = wsicList, Empid = eid, WeeklyAvgCount = avg.Count };
                    weeklyServices.Add(resultList);
                    for (int d = 0; d < timelist.Count; d++)
                    {
                        string daily = (DateTime.Parse(timelist[d].DateName)).ToString("yyyy-MM-dd");
                        for (int t = 0; t < TicketTypeList.Count; t++)
                        {
                            EmpServiceInfoCount esic = new EmpServiceInfoCount();
                            var type = TicketTypeList[t].Value;
                            var servicenumberday = SCList.Where(x => x.EmpId == eid && x.ServiceDate == daily && x.ServiceName == type).Select(y => y.CountNumber).ToList();
                            if (servicenumberday.Count > 0)
                            {
                                esic.AppDate = daily;
                                esic.Empid = eid;
                                esic.ServiceName = TicketTypeList[t].Text;
                                esic.Service = type;
                                esic.SCount = servicenumberday.Sum();
                                esicList.Add(esic);
                            }
                        }
                    }
                }
                TSICC.DailyServiceTotalList = DAilyList;
                TSICC.WeeklyServiceTotalList = weeklyServices;
                TSICC.EmpServiceTotal = esicList;
                TSICC.TicketTypeList = TypeList;

                for (int d = 0; d < timelist.Count; d++)
                {
                    string daily = (DateTime.Parse(timelist[d].DateName)).ToString("yyyy-MM-dd");
                    DailyInfoShow show = new DailyInfoShow();
                    var data = AppList.Where(x => x.AppointmentDate == daily).ToList().GroupBy(it => it.UserId).Select(group => new { keyId = group.Key, Items = group.ToList() }).ToList();
                    var num = data.Count;
                    if (num > 0)
                    {
                        List<DailyInfoList> multiList = new List<DailyInfoList>();
                        for (int v = 0; v < num; v++)
                        {
                            DailyInfoList list = new DailyInfoList();
                            List<DailyInfo> dfList = new List<DailyInfo>();
                            foreach (var app in data[v].Items)
                            {
                                DailyInfo df = new DailyInfo();
                                df.AppDate = (DateTime.Parse(app.AppointmentDate)).ToString("dd-MMM-yyyy");
                                df.AppointmentId = app.AppointmentId;
                                if (!app.IsAllDay)
                                {
                                    df.NameInfo = app.Name + " " + app.AppointmentStartTime + " - " + app.AppointmentEndTime;
                                    df.PopUpString = "Date & Time - " + app.AppointmentDate + " " + app.AppointmentStartTime + " - " + app.AppointmentEndTime + "&#013;Ticket# " + app.TicketId + "&#013;Booking# " + app.BookingId + "&#013;Type - " + app.TicketTypeDisplayText + "&#013;Status - " + app.Status + "&#013;Assigned - " + app.EmployeeName + "&#013;Customer - " + app.Name + "&#013;" + app.StreetAddress + "&#013;" + app.Address;
                                }
                                else
                                {
                                    df.NameInfo = app.Name + " Full Day";
                                    df.PopUpString = "Date & Time - " + app.AppointmentDate + " full day &#013;Ticket# " + app.TicketId + "&#013;Booking# " + app.BookingId + "&#013;Type - " + app.TicketTypeDisplayText + "&#013;Status - " + app.Status + "&#013;Assigned - " + app.EmployeeName + "&#013;Customer - " + app.Name + "&#013;" + app.StreetAddress + "&#013;" + app.Address;
                                }

                                df.CountNumber = 1;
                                df.EmpId = app.UserId;
                                df.ServiceName = app.AppointmentType;
                                dfList.Add(df);
                            }
                            list.dailyInfos = dfList;
                            show.AppointmentDate = timelist[d].DateName;
                            if (dfList.Count > 0)
                            {
                                show.BGColor = "#ccccff";
                                show.DayName = timelist[d].TimeName;
                                //show.Border = "2px solid #cc5200";
                                show.LeftIcon = "fa fa-map-o";
                            }
                            else
                            {
                                show.BGColor = "#ccccff";
                                show.DayName = timelist[d].TimeName;
                                show.LeftIcon = "fa fa-map-o";
                            }
                            multiList.Add(list);
                        }
                        show.dailyInfolist = multiList;
                        showList.Add(show);
                    }
                    else
                    {
                        show.AppointmentDate = timelist[d].DateName;
                        show.DayName = timelist[d].TimeName;
                        show.BGColor = "#ccccff";
                        show.Border = "2px solid #ccc";
                        show.LeftIcon = "fa fa-map-o";
                        showList.Add(show);
                    }

                }
                List<DailyServiceInfoCount> WeeklyserviceListInfo = WeeklyserviceCountList.GroupBy(x => x.Service).Select(y => new DailyServiceInfoCount
                {
                    Service = y.First().Service,
                    ServiceName = y.First().ServiceName,
                    SCount = y.Sum(sc => sc.SCount)
                }).ToList();

                var result = new WeeklyInfoListModel() { AppList = AppList, EmpList = EmpList.OrderBy(x => x.EmpName).ToList(), Timelist = timelist, WeeklyInfo = showList, ServiceCount = TSICC, WeeklyTotalInfo = WeeklyserviceListInfo };
                return result;
            }
            catch (Exception)
            {
                var result = new WeeklyInfoListModel();
                return result;
            }
        }
        private MonthlyInfoListModel monthlyinfo(List<CustomCalendarAllRecords> model, string date, string FirstDayOfWeek)
        {
            try
            {
                var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
                DateTime dt;
                bool successfullyParsed = DateTime.TryParse(date, out dt);
                if (string.IsNullOrEmpty(date) || !successfullyParsed)
                {
                    dt = DateTime.UtcNow.UTCToClientTime();
                }
                var dateend = new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
                int daycount = Convert.ToInt32(dateend.ToString("dd"));
                List<WeeklyServiceInfoCount> ServiceTypeList = new List<WeeklyServiceInfoCount>();
                List<CalListModelList> callist = new List<CalListModelList>();
                List<CalListModel> timelist = new List<CalListModel>();
                List<CalListModel> tlist = new List<CalListModel>();
                int j = 1;
                if (!string.IsNullOrEmpty(FirstDayOfWeek) && !string.IsNullOrWhiteSpace(FirstDayOfWeek))
                {
                    for (int i = 1; i <= daycount; i++)
                    {
                        var startday = new DateTime(dt.Year, dt.Month, i);
                        string day = startday.ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
                        string todate = startday.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en-US"));
                        CalListModel adddate = new CalListModel();
                        if (FirstDayOfWeek.ToLower() == "sunday")
                        {
                            if (day == "Monday") { adddate.SL = (int)DaysSunday.Monday; }
                            else if (day == "Tuesday") { adddate.SL = (int)DaysSunday.Tuesday; }
                            else if (day == "Wednesday") { adddate.SL = (int)DaysSunday.Wednesday; }
                            else if (day == "Thursday") { adddate.SL = (int)DaysSunday.Thursday; }
                            else if (day == "Friday") { adddate.SL = (int)DaysSunday.Friday; }
                            else if (day == "Saturday") { adddate.SL = (int)DaysSunday.Saturday; }
                            else { adddate.SL = (int)DaysSunday.Sunday; }
                        }
                        else if (FirstDayOfWeek.ToLower() == "saturday")
                        {
                            if (day == "Monday") { adddate.SL = (int)DaysSaturday.Monday; }
                            else if (day == "Tuesday") { adddate.SL = (int)DaysSaturday.Tuesday; }
                            else if (day == "Wednesday") { adddate.SL = (int)DaysSaturday.Wednesday; }
                            else if (day == "Thursday") { adddate.SL = (int)DaysSaturday.Thursday; }
                            else if (day == "Friday") { adddate.SL = (int)DaysSaturday.Friday; }
                            else if (day == "Saturday") { adddate.SL = (int)DaysSaturday.Saturday; }
                            else { adddate.SL = (int)DaysSaturday.Sunday; }
                        }
                        else
                        {
                            if (day == "Monday") { adddate.SL = (int)DaysMonday.Monday; }
                            else if (day == "Tuesday") { adddate.SL = (int)DaysMonday.Tuesday; }
                            else if (day == "Wednesday") { adddate.SL = (int)DaysMonday.Wednesday; }
                            else if (day == "Thursday") { adddate.SL = (int)DaysMonday.Thursday; }
                            else if (day == "Friday") { adddate.SL = (int)DaysMonday.Friday; }
                            else if (day == "Saturday") { adddate.SL = (int)DaysMonday.Saturday; }
                            else { adddate.SL = (int)DaysMonday.Sunday; }
                        }
                        adddate.DateName = todate;
                        adddate.TimeName = day;
                        adddate.DayNumber = i;
                        timelist.Add(adddate);
                        tlist.Add(adddate);
                        if (adddate.SL == 7 || i == daycount)
                        {
                            int DCount = callist.Count;
                            int tcount = tlist.Count;
                            var mn = startday.ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("en-US"));
                            if (DCount == 0)
                            {
                                var previousend = new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month)).AddMonths(-1);

                                for (int d = 7; d > tcount; d--)
                                {
                                    CalListModel add = new CalListModel();
                                    string preday = previousend.ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
                                    string predate = previousend.ToString("dd-MMM", CultureInfo.CreateSpecificCulture("en-US"));
                                    add.DateName = predate;
                                    add.TimeName = preday;
                                    add.SL = (d - tcount);
                                    tlist.Add(add);
                                    previousend = new DateTime(previousend.Year, previousend.Month, previousend.Day).AddDays(-1);
                                }
                            }
                            if (DCount == 4 && tcount < 7 && i == daycount)
                            {
                                var nextmonth = new DateTime(dt.Year, dt.Month, 1).AddMonths(1);
                                for (int d = 1; d <= 7 - tcount; d++)
                                {
                                    CalListModel add = new CalListModel();
                                    string preday = nextmonth.ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
                                    string predate = nextmonth.ToString("dd-MMM", CultureInfo.CreateSpecificCulture("en-US"));
                                    add.DateName = predate;
                                    add.TimeName = preday;
                                    add.SL = (d + tcount);
                                    tlist.Add(add);
                                    nextmonth = new DateTime(nextmonth.Year, nextmonth.Month, nextmonth.Day).AddDays(1);
                                }
                            }
                            else if (DCount == 5 && i == daycount)
                            {
                                var nextmonth = new DateTime(dt.Year, dt.Month, 1).AddMonths(1);
                                for (int d = 1; d <= 7 - tcount; d++)
                                {
                                    CalListModel add = new CalListModel();
                                    string preday = nextmonth.ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
                                    string predate = nextmonth.ToString("dd-MMM", CultureInfo.CreateSpecificCulture("en-US"));
                                    add.DateName = predate;
                                    add.TimeName = preday;
                                    add.SL = (d + tcount);
                                    tlist.Add(add);
                                    nextmonth = new DateTime(nextmonth.Year, nextmonth.Month, nextmonth.Day).AddDays(1);
                                }
                            }
                            var asc = tlist.OrderBy(x => x.SL).ToList();
                            var weekinfo = new CalListModelList() { MonthName = mn, WeekCount = j, WeekList = asc };
                            callist.Add(weekinfo);
                            j++;
                            tlist = new List<CalListModel>();
                        }
                    }
                }
                var testId = new Guid().ToString();
                List<SelectListItem> TicketTypeList = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Where(x => x.DataValue != "-1" && x.AlterDisplayText1 != null && x.AlterDisplayText1 != "" && x.AlterDisplayText1 == "True").OrderBy(x => x.DisplayText).ToList().Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();
                List<MonthlyServiceInfoCount> monthList = new List<MonthlyServiceInfoCount>();
                List<ServiceCount> SCList = new List<ServiceCount>();
                if (model != null)
                {
                    foreach (var info in model)
                    {
                        ServiceCount SC = new ServiceCount();
                        if (SCList.Count > 0)
                        {
                            int num = SCList.Count;
                            var edt = info.EventDate;
                            var val = info.EventType;
                            bool x = false;
                            for (int n = 0; n < num; n++)
                            {
                                if (edt == SCList[n].ServiceDate)
                                {
                                    if (val == SCList[n].ServiceName && info.EventEmployeeGuidId == SCList[n].EmpId)
                                    {
                                        SCList[n].CountNumber += 1;
                                        x = true;
                                    }
                                }
                            }
                            if (!x)
                            {
                                SC.CountNumber = 1;
                                SC.ServiceName = info.EventType;
                                SC.ServiceDate = info.EventDate;
                                SC.EmpId = info.EventEmployeeGuidId;
                                SCList.Add(SC);
                            }
                        }
                        else
                        {
                            SC.CountNumber = 1;
                            SC.ServiceName = info.EventType;
                            SC.ServiceDate = info.EventDate;
                            SC.EmpId = info.EventEmployeeGuidId;
                            SCList.Add(SC);
                        }
                    }
                }
                List<DailyServiceInfoCountList> DailyList = new List<DailyServiceInfoCountList>();
                for (int d = 0; d < timelist.Count; d++)
                {
                    List<DailyServiceInfoCount> dsicList = new List<DailyServiceInfoCount>();
                    string daily = (DateTime.Parse(timelist[d].DateName)).ToString("yyyy-MM-dd");
                    var daynumber = SCList.Where(x => x.ServiceDate == daily).Select(y => y.CountNumber).ToList();
                    for (int st = 0; st < TicketTypeList.Count; st++)
                    {
                        DailyServiceInfoCount dsic = new DailyServiceInfoCount();
                        var type = TicketTypeList[st].Value;
                        var servicedaynumber = SCList.Where(x => x.ServiceDate == daily && x.ServiceName == type).Select(y => y.CountNumber).ToList();
                        if (servicedaynumber.Count > 0)
                        {
                            dsic.Service = type;
                            dsic.ServiceName = TicketTypeList[st].Text;
                            dsic.SCount = servicedaynumber.Sum();
                            dsicList.Add(dsic);
                        }
                    }
                    var resultList = new DailyServiceInfoCountList() { DailyTotal = daynumber.Sum(), DailyServiceTotal = dsicList, AppDate = daily, DailyAvgCount = dsicList.Count };
                    DailyList.Add(resultList);
                }

                for (int a = 0; a < callist.Count; a++)
                {
                    int end = 6, start = 0;
                    var dayarr = callist[a].WeekList;
                    if (a == 0)
                    {
                        for (int b = 0; b < dayarr.Count; b++)
                        {
                            if (dayarr[b].DayNumber > 0)
                            {
                                if (start == 0)
                                {
                                    start = b;
                                    break;
                                }
                            }
                        }
                    }
                    if (a == callist.Count - 1)
                    {
                        for (int b = 0; b < dayarr.Count; b++)
                        {
                            if (dayarr[b].DayNumber == 0)
                            {
                                end = b;
                                break;
                            }
                        }
                    }
                    for (int st = 0; st < TicketTypeList.Count; st++)
                    {
                        var weeklytotal = SCList.Where(x => DateTime.Parse(x.ServiceDate) >= DateTime.Parse(dayarr[start].DateName) && DateTime.Parse(x.ServiceDate) <= DateTime.Parse(dayarr[end].DateName) && x.ServiceName == TicketTypeList[st].Value).Select(y => y.CountNumber).ToList();
                        var resultList = new MonthlyServiceInfoCount() { Week = a + 1, ServiceName = TicketTypeList[st].Text, ServiceValue = TicketTypeList[st].Value, ServiceTotal = weeklytotal.Sum(), AvgCount = weeklytotal.Count };
                        monthList.Add(resultList);
                    }
                }
                // var data = AppList.Where(x => x.AppointmentDate == daily).ToList().GroupBy(it => it.UserId).Select(group => new { keyId = group.Key, Items = group.ToList() }).ToList();
                var weekinfoList = monthList.GroupBy(x => x.Week).Select(group => new MonthlyGroupbyInfo { keyValue = group.Key, MonthInfo = group.ToList() }).ToList();
                foreach (var item in TicketTypeList)
                {
                    var type = new WeeklyServiceInfoCount() { Service = item.Value, ServiceName = item.Text };
                    ServiceTypeList.Add(type);
                }
                var result = new MonthlyInfoListModel() { DayForCalendar = callist, DayDataShow = DailyList, WeeklyTotalDataShow = weekinfoList, TicketTypeList = ServiceTypeList };
                return result;
            }
            catch (Exception)
            {
                var result = new MonthlyInfoListModel();
                return result;
            }
        }


        [HttpPost]
        public ActionResult ScheduleUserListPartial()
        {
            try
            {
                var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
                List<SelectListItem> ListEmployee = new List<SelectListItem>();
                ListEmployee.Add(new SelectListItem()
                {
                    Text = "Select User",
                    Value = ""
                });
                ListEmployee.AddRange(_Util.Facade.EmployeeFacade.GetAllEmployee(currentLoggedIn.CompanyId.Value).Select(x => new SelectListItem()
                {
                    Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                    Value = x.UserId.ToString()
                }).ToList());
                //ViewBag.ListEmployee = ListEmployee.OrderBy(x => x.Text != "Select User").ThenBy(x => x.Text).ToList();
                List<SelectListItem> TicketTypeList = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString()
               }).ToList();
                if (TicketTypeList.Count() > 0)
                {
                    TicketTypeList[0].Text = "Select Ticket Type";
                }
                ViewBag.TicketType = TicketTypeList;

                return PartialView("ScheduleUserListPartial");
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
        }


        [Authorize]
        [HttpPost]
        public JsonResult DraggingScheduleCalendar(string eventType, int? eventId, string eventDate, string eventAppid, bool eventAllDay, string eventEndDate, string Eventresid, string dragresid, string ViewName, string CustomId, string eventticketid, string additional, bool? chkassign, bool? isexist)
        {
            try
            {
                var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
                bool result = false;
                bool dropres = false;
                string message = "";
                string LogMessage = "";
                string LogUserName = "";
                Guid customerId = Guid.Empty;
                DateTime Endremdate = new DateTime();
                var reminderEndTime = "";
                bool tryparseResult = DateTime.TryParse(eventDate, out Endremdate);
                if (!tryparseResult) { return Json(new { result = false, message = "Invalid appointment start date and time" }); }
                List<string> spliteventDate = new List<string>();
                List<string> spliteventEndDate = new List<string>();
                if (!string.IsNullOrWhiteSpace(eventDate))
                {
                    spliteventDate = eventDate.Split('T').ToList();
                }
                if (!string.IsNullOrWhiteSpace(eventEndDate))
                {
                    spliteventEndDate = eventEndDate.Split('T').ToList();
                }
                if (eventAllDay)
                {
                    string TicketStartTime = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarMinTimeRange(currentLoggedIn.CompanyId.Value);
                    string TicketEndTime = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarMaxTimeRange(currentLoggedIn.CompanyId.Value);
                    if (!string.IsNullOrWhiteSpace(TicketStartTime) && !string.IsNullOrEmpty(TicketStartTime) && TicketStartTime.Length == 5)
                    {
                        DateTime FullDayStartDateAndTime = Convert.ToDateTime(spliteventDate[0] + " " + (TicketStartTime + ":00").Replace(".000Z", ""));
                        eventDate = FullDayStartDateAndTime.ToString("yyyy-MM-dd") + "T" + FullDayStartDateAndTime.ToString("HH:mm:ss");
                        spliteventDate = eventDate.Split('T').ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(TicketEndTime) && !string.IsNullOrEmpty(TicketEndTime) && TicketEndTime.Length == 5)
                    {
                        DateTime FullDayEndDateAndTime = Convert.ToDateTime(spliteventDate[0] + " " + (TicketEndTime + ":00").Replace(".000Z", ""));
                        eventEndDate = FullDayEndDateAndTime.ToString("yyyy-MM-dd") + "T" + FullDayEndDateAndTime.ToString("HH:mm:ss");
                        spliteventEndDate = eventEndDate.Split('T').ToList();
                    }

                }
                if (!string.IsNullOrWhiteSpace(eventDate) && !string.IsNullOrWhiteSpace(eventEndDate) && eventDate == eventEndDate)
                {
                    if (spliteventDate.Count == 2)
                    {
                        DateTime StartDateAndTime = Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1].Replace(".000Z", ""));
                        StartDateAndTime = StartDateAndTime.AddHours(2);
                        eventEndDate = StartDateAndTime.ToString("yyyy-MM-dd") + "T" + StartDateAndTime.ToString("HH:mm:ss");
                        spliteventEndDate = eventEndDate.Split('T').ToList();
                    }
                }
                if (!string.IsNullOrWhiteSpace(ViewName) && ViewName == "month" && !string.IsNullOrWhiteSpace(eventType))
                {
                    if (!string.IsNullOrWhiteSpace(eventticketid) && eventticketid != new Guid().ToString())
                    {
                        var objticketuser = _Util.Facade.TicketFacade.GetTicketUserListByUserId(new Guid(eventticketid), System.Web.HttpUtility.UrlDecode(eventType));
                        if (objticketuser.Count > 0)
                        {
                            foreach (var item in objticketuser)
                            {
                                var objapp = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentidByAppoinmentId(item.TiketId);
                                var emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item.UserId);
                                var obticket = _Util.Facade.TicketFacade.GetTicketByTicketId(item.TiketId);
                                if (objapp != null && obticket != null)
                                {
                                    objapp.AppointmentDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                    obticket.CompletionDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                    _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinment(objapp);
                                    _Util.Facade.TicketFacade.UpdateTicket(obticket);

                                    //customerId = obticket.CustomerId;
                                    //LogMessage = "Ticket#" + obticket.Id + " is assigned " + emp != null ? emp.FirstName + " " + emp.LastName : "" + " scheduled " + obticket.CompletionDate.ToString("MM/dd/yy") + " from " + objapp.AppointmentStartTime + " to " + objapp.AppointmentEndTime;
                                }
                                var objappadditional = _Util.Facade.CustomerAppoinmentFacade.GetAdditionalMembersAppointmentByAppointmentIdAndUserId(item.TiketId, item.UserId);
                                if (objappadditional != null)
                                {
                                    objappadditional.AppointmentDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                    _Util.Facade.CustomerAppoinmentFacade.UpdateAdditionalMembersAppointment(objappadditional);
                                }


                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(eventType) && eventAllDay == false)
                    {
                        if (eventType == "Reminder")
                        {
                            if (!string.IsNullOrWhiteSpace(Eventresid))
                            {
                                var userobj = _Util.Facade.EmployeeFacade.GetEmployeeById(Convert.ToInt32(Eventresid));
                                if (userobj != null)
                                {
                                    var remdate = (Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1]));
                                    if (!string.IsNullOrWhiteSpace(eventEndDate))
                                    {
                                        Endremdate = (Convert.ToDateTime(spliteventEndDate[0] + " " + spliteventEndDate[1]));
                                    }
                                    else
                                    {
                                        Endremdate = remdate.AddHours(2);
                                    }
                                    var remlist = _Util.Facade.NotesFacade.GetReminderScheduleByReminderDateAndEmployeeId(remdate.ToString("yyyy-MM-dd HH:mm:ss"), userobj.UserId, Endremdate.ToString("yyyy-MM-dd HH:mm:ss"));
                                    var remlist1 = _Util.Facade.NotesFacade.GetReminderSchedule1ByReminderDateAndEmployeeId(remdate.ToString("yyyy-MM-dd HH:mm:ss"), userobj.UserId, Endremdate.ToString("yyyy-MM-dd HH:mm:ss"));
                                    if (eventId.HasValue)
                                    {
                                        var objreminder = _Util.Facade.NotesFacade.GetNotesById(eventId.Value);
                                        if (objreminder != null)
                                        {
                                            string datetime = spliteventDate[0] + " " + objreminder.ReminderDate.Value.Hour + ":" + objreminder.ReminderDate.Value.Minute + ":00";
                                            objreminder.IsAllDay = false;
                                            if (spliteventDate.Count > 0)
                                            {
                                                var reminderTime = Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1]).ToString("HH:mm", CultureInfo.InvariantCulture);
                                                objreminder.ReminderDate = Convert.ToDateTime(spliteventDate[0] + " " + reminderTime);
                                                if (!string.IsNullOrWhiteSpace(eventEndDate))
                                                {
                                                    reminderEndTime = Convert.ToDateTime(spliteventEndDate[0] + " " + spliteventEndDate[1]).ToString("HH:mm", CultureInfo.InvariantCulture);
                                                    objreminder.ReminderEndDate = Convert.ToDateTime(spliteventEndDate[0] + " " + reminderEndTime);
                                                }
                                                else
                                                {
                                                    reminderEndTime = Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1]).AddHours(2).ToString("HH:mm", CultureInfo.InvariantCulture);
                                                    objreminder.ReminderEndDate = Convert.ToDateTime(spliteventDate[0] + " " + reminderTime).AddHours(2);
                                                }
                                            }
                                            else
                                            {
                                                objreminder.ReminderDate = Convert.ToDateTime(datetime);
                                                objreminder.ReminderEndDate = objreminder.ReminderDate.Value.AddHours(2);
                                            }
                                            result = _Util.Facade.NotesFacade.UpdateNotes(objreminder);
                                            var timeval = objreminder.ReminderDate.Value.ToString("HH:mm");
                                            var objLookup = _Util.Facade.LookupFacade.GetLookupByKeyAndValue("Arrival", timeval);
                                            if (objLookup != null)
                                            {
                                                objLookup.IsActive = true;
                                                result = _Util.Facade.LookupFacade.UpdateLookUp(objLookup);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (isexist.HasValue && isexist.Value)
                            {
                                if (chkassign.HasValue && chkassign.Value)
                                {
                                    if (!string.IsNullOrWhiteSpace(eventticketid) && !string.IsNullOrWhiteSpace(Eventresid))
                                    {
                                        var objticket = _Util.Facade.TicketFacade.GetTicketByTicketId(new Guid(eventticketid));
                                        var empobj = _Util.Facade.EmployeeFacade.GetEmployeeById(Convert.ToInt32(Eventresid));
                                        if (objticket != null && empobj != null)
                                        {
                                            var userpermission = _Util.Facade.PermissionFacade.GetPermissionGroupByUserId(empobj.UserId);
                                            if (userpermission != null && (userpermission.Name.ToLower().IndexOf("technician") > -1 || userpermission.Name.ToLower().IndexOf("installation") > -1 || userpermission.Tag.ToLower().IndexOf("technician") > -1 || userpermission.Tag.ToLower().IndexOf("installation") > -1))
                                            {
                                                _Util.Facade.TicketFacade.DeleteTicketUserByTicketId(objticket.TicketId, true, false);
                                                TicketUser TicketUser = new TicketUser()
                                                {
                                                    TiketId = objticket.TicketId,
                                                    UserId = empobj.UserId,
                                                    IsPrimary = true,
                                                    AddedDate = DateTime.Now.UTCCurrentTime(),
                                                    AddedBy = currentLoggedIn.UserId,
                                                    NotificationOnly = false,
                                                    IsReschedulePay = false
                                                };
                                                dropres = _Util.Facade.TicketFacade.InsertTicketUser(TicketUser) > 0;
                                            }
                                            else if (empobj.UserId == new Guid("22222222-2222-2222-2222-222222222222"))
                                            {
                                                _Util.Facade.TicketFacade.DeleteTicketUserByTicketId(objticket.TicketId, true, false);
                                                TicketUser TicketUser = new TicketUser()
                                                {
                                                    TiketId = objticket.TicketId,
                                                    UserId = empobj.UserId,
                                                    IsPrimary = true,
                                                    AddedDate = DateTime.Now.UTCCurrentTime(),
                                                    AddedBy = currentLoggedIn.UserId,
                                                    NotificationOnly = false,
                                                    IsReschedulePay = false
                                                };
                                                dropres = _Util.Facade.TicketFacade.InsertTicketUser(TicketUser) > 0;
                                            }
                                            else
                                            {
                                                message = "User is not a technician, so this user not assigned as an assign member";
                                            }
                                            if (string.IsNullOrWhiteSpace(message))
                                            {
                                                LogUserName = empobj.FirstName + " " + empobj.LastName;
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    bool objsetting = Convert.ToBoolean(_Util.Facade.GlobalSettingsFacade.GetTicketAdditionalMemberOnlyTechnicianByCompanyId(currentLoggedIn.CompanyId.Value));
                                    if (objsetting)
                                    {
                                        bool reschedulePay = false;
                                        var objticket = _Util.Facade.TicketFacade.GetTicketByTicketId(new Guid(eventticketid));
                                        var empobj = _Util.Facade.EmployeeFacade.GetEmployeeById(Convert.ToInt32(Eventresid));
                                        if (objticket != null && empobj != null)
                                        {
                                            var userpermission = _Util.Facade.PermissionFacade.GetPermissionGroupByUserId(empobj.UserId);
                                            if (userpermission != null && (userpermission.Name.ToLower().IndexOf("technician") > -1 || userpermission.Name.ToLower().IndexOf("installation") > -1 || userpermission.Tag.ToLower().IndexOf("technician") > -1 || userpermission.Tag.ToLower().IndexOf("installation") > -1))
                                            {
                                                var objtiketuser = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndUserIdAndNotification(empobj.UserId, new Guid(eventticketid));
                                                if (objtiketuser != null)
                                                {
                                                    reschedulePay = objtiketuser.IsReschedulePay.HasValue ? objtiketuser.IsReschedulePay.Value : false;
                                                    _Util.Facade.TicketFacade.DeleteTicketUserById(objtiketuser.Id);
                                                }
                                                TicketUser TicketUser = new TicketUser()
                                                {
                                                    TiketId = objticket.TicketId,
                                                    UserId = empobj.UserId,
                                                    IsPrimary = false,
                                                    AddedDate = DateTime.Now.UTCCurrentTime(),
                                                    AddedBy = currentLoggedIn.UserId,
                                                    NotificationOnly = false,
                                                    IsReschedulePay = reschedulePay
                                                };
                                                dropres = _Util.Facade.TicketFacade.InsertTicketUser(TicketUser) > 0;
                                                var objadditionalmember = _Util.Facade.CustomerAppoinmentFacade.GetAdditionalMembersAppointmentByAppointmentIdAndUserId(new Guid(eventticketid), empobj.UserId);
                                                if (objadditionalmember != null)
                                                {
                                                    _Util.Facade.CustomerAppoinmentFacade.DeleteAdditionalMembersAppointment(objadditionalmember.Id);
                                                }
                                                AdditionalMembersAppointment AdditionalMembersAppointment = new AdditionalMembersAppointment()
                                                {
                                                    AppointmentId = objticket.TicketId,
                                                    CompanyId = currentLoggedIn.CompanyId.Value,
                                                    CustomerId = objticket.CustomerId,
                                                    EmployeeId = empobj.UserId,
                                                    AppointmentDate = DateTime.Now,
                                                    AppointmentStartTime = "-1",
                                                    AppointmentEndTime = "-1",
                                                    CreatedBy = currentLoggedIn.UserId,
                                                    LastUpdatedBy = currentLoggedIn.UserId,
                                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                                    MemberAppointmentId = new Guid(),
                                                    IsAllDay = true
                                                };
                                                dropres = _Util.Facade.CustomerAppoinmentFacade.InsertAdditionalMembersAppointment(AdditionalMembersAppointment) > 0;
                                            }
                                            else
                                            {
                                                message = "User is not a technician, so this user not assigned as an additional member";

                                            }
                                            if (string.IsNullOrWhiteSpace(message))
                                            {
                                                LogUserName = empobj.FirstName + " " + empobj.LastName;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        bool reschedulePay = false;
                                        var objticket = _Util.Facade.TicketFacade.GetTicketByTicketId(new Guid(eventticketid));
                                        var empobj = _Util.Facade.EmployeeFacade.GetEmployeeById(Convert.ToInt32(Eventresid));
                                        if (objticket != null && empobj != null)
                                        {
                                            var objtiketuser = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndUserIdAndNotification(empobj.UserId, new Guid(eventticketid));
                                            if (objtiketuser != null)
                                            {
                                                reschedulePay = objtiketuser.IsReschedulePay.HasValue ? objtiketuser.IsReschedulePay.Value : false;
                                                _Util.Facade.TicketFacade.DeleteTicketUserById(objtiketuser.Id);
                                            }
                                            TicketUser TicketUser = new TicketUser()
                                            {
                                                TiketId = objticket.TicketId,
                                                UserId = empobj.UserId,
                                                IsPrimary = false,
                                                AddedDate = DateTime.Now.UTCCurrentTime(),
                                                AddedBy = currentLoggedIn.UserId,
                                                NotificationOnly = false,
                                                IsReschedulePay = reschedulePay
                                            };
                                            dropres = _Util.Facade.TicketFacade.InsertTicketUser(TicketUser) > 0;
                                            var objadditionalmember = _Util.Facade.CustomerAppoinmentFacade.GetAdditionalMembersAppointmentByAppointmentIdAndUserId(new Guid(eventticketid), empobj.UserId);
                                            if (objadditionalmember != null)
                                            {
                                                _Util.Facade.CustomerAppoinmentFacade.DeleteAdditionalMembersAppointment(objadditionalmember.Id);
                                            }
                                            AdditionalMembersAppointment AdditionalMembersAppointment = new AdditionalMembersAppointment()
                                            {
                                                AppointmentId = objticket.TicketId,
                                                CompanyId = currentLoggedIn.CompanyId.Value,
                                                CustomerId = objticket.CustomerId,
                                                EmployeeId = empobj.UserId,
                                                AppointmentDate = DateTime.Now,
                                                AppointmentStartTime = "-1",
                                                AppointmentEndTime = "-1",
                                                CreatedBy = currentLoggedIn.UserId,
                                                LastUpdatedBy = currentLoggedIn.UserId,
                                                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                                MemberAppointmentId = new Guid(),
                                                IsAllDay = true
                                            };
                                            dropres = _Util.Facade.CustomerAppoinmentFacade.InsertAdditionalMembersAppointment(AdditionalMembersAppointment) > 0;
                                            if (string.IsNullOrWhiteSpace(message))
                                            {
                                                LogUserName = empobj.FirstName + " " + empobj.LastName;
                                            }
                                        }
                                    }
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(eventAppid))
                            {
                                if (chkassign.HasValue && chkassign.Value == false)
                                {
                                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeById(Convert.ToInt32(Eventresid));
                                    if (empobj != null)
                                    {
                                        var objadditionalapp = _Util.Facade.CustomerAppoinmentFacade.GetAdditionalMembersAppointmentByAppointmentIdAndUserId(new Guid(eventAppid), empobj.UserId);
                                        var obticket = _Util.Facade.TicketFacade.GetTicketByTicketId(new Guid(eventAppid));
                                        if (objadditionalapp != null && obticket != null)
                                        {
                                            objadditionalapp.AppointmentDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                            var startdatetime = Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1].Replace(":00Z", ""));
                                            objadditionalapp.AppointmentStartTime = startdatetime.ToString("HH:mm");
                                            if (spliteventEndDate.Count > 0)
                                            {
                                                var enddatetime = Convert.ToDateTime(spliteventEndDate[0] + " " + spliteventEndDate[1].Replace(":00.000Z", ""));
                                                objadditionalapp.AppointmentEndTime = enddatetime.ToString("HH:mm");
                                            }
                                            else
                                            {
                                                var enddatetime = Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1].Replace(":00Z", "")).AddHours(2);
                                                objadditionalapp.AppointmentEndTime = enddatetime.ToString("HH:mm");
                                            }
                                            objadditionalapp.IsAllDay = false;
                                            _Util.Facade.CustomerAppoinmentFacade.UpdateAdditionalMembersAppointment(objadditionalapp);
                                        }

                                    }
                                }
                                else
                                {
                                    var objapp = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentidByAppoinmentId(new Guid(eventAppid));
                                    var obticket = _Util.Facade.TicketFacade.GetTicketByTicketId(new Guid(eventAppid));
                                    if (objapp != null && obticket != null)
                                    {
                                        objapp.AppointmentDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                        var startdatetime = Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1].Replace(":00Z", ""));
                                        objapp.AppointmentStartTime = startdatetime.ToString("HH:mm");
                                        if (spliteventEndDate.Count > 0)
                                        {
                                            var enddatetime = Convert.ToDateTime(spliteventEndDate[0] + " " + spliteventEndDate[1].Replace(":00.000Z", ""));
                                            objapp.AppointmentEndTime = enddatetime.ToString("HH:mm");
                                        }
                                        else
                                        {
                                            var enddatetime = Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1].Replace(":00Z", "")).AddHours(2);
                                            objapp.AppointmentEndTime = enddatetime.ToString("HH:mm");
                                        }
                                        obticket.CompletionDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                        objapp.IsAllDay = false;
                                        _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinment(objapp);
                                        _Util.Facade.TicketFacade.UpdateTicket(obticket);
                                        #region Log for ticket update from schedule calendar
                                        if (!string.IsNullOrWhiteSpace(LogUserName))
                                        {
                                            customerId = obticket.CustomerId;
                                            LogMessage = "Ticket#" + obticket.Id + " is updated assigned to " + LogUserName + " scheduled on " + obticket.CompletionDate.ToString("M/d/yyyy") + " from " + Convert.ToDateTime(objapp.AppointmentStartTime).ToString("h:mm tt") + " to " + Convert.ToDateTime(objapp.AppointmentEndTime).ToString("h:mm tt");
                                            base.AddUserActivityForCustomer(LogMessage, LabelHelper.ActivityAction.Update, customerId, null, null);
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(eventType) && eventAllDay == true)
                    {
                        if (eventType == "Reminder")
                        {
                            if (eventId.HasValue)
                            {
                                var objreminder = _Util.Facade.NotesFacade.GetNotesById(eventId.Value);
                                if (objreminder != null)
                                {
                                    string datetime = spliteventDate[0] + " " + objreminder.ReminderDate.Value.Hour + ":" + objreminder.ReminderDate.Value.Minute + ":00";
                                    objreminder.IsAllDay = true;
                                    result = _Util.Facade.NotesFacade.UpdateNotes(objreminder);
                                }
                            }
                        }
                        else
                        {
                            if (eventId.HasValue)
                            {
                                if (!string.IsNullOrWhiteSpace(eventAppid))
                                {
                                    if (chkassign.HasValue && chkassign.Value == false)
                                    {
                                        bool objsetting = Convert.ToBoolean(_Util.Facade.GlobalSettingsFacade.GetTicketAdditionalMemberOnlyTechnicianByCompanyId(currentLoggedIn.CompanyId.Value));
                                        if (objsetting)
                                        {
                                            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeById(Convert.ToInt32(Eventresid));
                                            var userpermission = _Util.Facade.PermissionFacade.GetPermissionGroupByUserId(objemp.UserId);
                                            if (userpermission != null && (userpermission.Name.ToLower().IndexOf("technician") > -1 || userpermission.Name.ToLower().IndexOf("installation") > -1 || objemp.UserId == new Guid("22222222-2222-2222-2222-222222222222") || userpermission.Tag.ToLower().IndexOf("technician") > -1 || userpermission.Tag.ToLower().IndexOf("installation") > -1))
                                            {
                                                var objticket = _Util.Facade.TicketFacade.GetTicketByTicketId(new Guid(eventAppid));
                                                if (objemp != null)
                                                {
                                                    var objadditionalapp = _Util.Facade.CustomerAppoinmentFacade.GetAdditionalMembersAppointmentByAppointmentIdAndUserId(new Guid(eventAppid), objemp.UserId);
                                                    if (objadditionalapp != null)
                                                    {
                                                        objadditionalapp.AppointmentDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                                        objadditionalapp.IsAllDay = true;
                                                        _Util.Facade.CustomerAppoinmentFacade.UpdateAdditionalMembersAppointment(objadditionalapp);
                                                    }
                                                    else
                                                    {
                                                        TicketUser TicketUser = new TicketUser()
                                                        {
                                                            TiketId = new Guid(eventAppid),
                                                            UserId = objemp.UserId,
                                                            IsPrimary = false,
                                                            AddedDate = DateTime.Now.UTCCurrentTime(),
                                                            AddedBy = currentLoggedIn.UserId,
                                                            NotificationOnly = false,
                                                            IsReschedulePay = false
                                                        };
                                                        dropres = _Util.Facade.TicketFacade.InsertTicketUser(TicketUser) > 0;
                                                        AdditionalMembersAppointment AdditionalMembersAppointment = new AdditionalMembersAppointment()
                                                        {
                                                            AppointmentId = new Guid(eventAppid),
                                                            CompanyId = currentLoggedIn.CompanyId.Value,
                                                            CustomerId = objticket.CustomerId,
                                                            EmployeeId = objemp.UserId,
                                                            AppointmentDate = DateTime.Now,
                                                            AppointmentStartTime = "-1",
                                                            AppointmentEndTime = "-1",
                                                            CreatedBy = currentLoggedIn.UserId,
                                                            LastUpdatedBy = currentLoggedIn.UserId,
                                                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                                            MemberAppointmentId = new Guid(),
                                                            IsAllDay = true
                                                        };
                                                        dropres = _Util.Facade.CustomerAppoinmentFacade.InsertAdditionalMembersAppointment(AdditionalMembersAppointment) > 0;
                                                        var objadditionalappnew = _Util.Facade.CustomerAppoinmentFacade.GetAdditionalMembersAppointmentByAppointmentIdAndUserId(new Guid(eventAppid), objemp.UserId);
                                                        var obticket = _Util.Facade.TicketFacade.GetTicketByTicketId(new Guid(eventAppid));
                                                        if (objadditionalappnew != null && obticket != null)
                                                        {
                                                            objadditionalappnew.AppointmentDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                                            _Util.Facade.CustomerAppoinmentFacade.UpdateAdditionalMembersAppointment(objadditionalappnew);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                message = "User is not a technician, so this user not assigned as an additional member";
                                            }
                                        }
                                        else
                                        {
                                            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeById(Convert.ToInt32(Eventresid));
                                            var objticket = _Util.Facade.TicketFacade.GetTicketByTicketId(new Guid(eventAppid));
                                            if (objemp != null)
                                            {
                                                var objadditionalapp = _Util.Facade.CustomerAppoinmentFacade.GetAdditionalMembersAppointmentByAppointmentIdAndUserId(new Guid(eventAppid), objemp.UserId);
                                                if (objadditionalapp != null)
                                                {
                                                    objadditionalapp.AppointmentDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                                    objadditionalapp.IsAllDay = true;
                                                    _Util.Facade.CustomerAppoinmentFacade.UpdateAdditionalMembersAppointment(objadditionalapp);
                                                }
                                                else
                                                {
                                                    TicketUser TicketUser = new TicketUser()
                                                    {
                                                        TiketId = new Guid(eventAppid),
                                                        UserId = objemp.UserId,
                                                        IsPrimary = false,
                                                        AddedDate = DateTime.Now.UTCCurrentTime(),
                                                        AddedBy = currentLoggedIn.UserId,
                                                        NotificationOnly = false,
                                                        IsReschedulePay = false
                                                    };
                                                    dropres = _Util.Facade.TicketFacade.InsertTicketUser(TicketUser) > 0;
                                                    AdditionalMembersAppointment AdditionalMembersAppointment = new AdditionalMembersAppointment()
                                                    {
                                                        AppointmentId = new Guid(eventAppid),
                                                        CompanyId = currentLoggedIn.CompanyId.Value,
                                                        CustomerId = objticket.CustomerId,
                                                        EmployeeId = objemp.UserId,
                                                        AppointmentDate = DateTime.Now,
                                                        AppointmentStartTime = "-1",
                                                        AppointmentEndTime = "-1",
                                                        CreatedBy = currentLoggedIn.UserId,
                                                        LastUpdatedBy = currentLoggedIn.UserId,
                                                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                                        MemberAppointmentId = new Guid(),
                                                        IsAllDay = true
                                                    };
                                                    dropres = _Util.Facade.CustomerAppoinmentFacade.InsertAdditionalMembersAppointment(AdditionalMembersAppointment) > 0;
                                                    var objadditionalappnew = _Util.Facade.CustomerAppoinmentFacade.GetAdditionalMembersAppointmentByAppointmentIdAndUserId(new Guid(eventAppid), objemp.UserId);
                                                    var obticket = _Util.Facade.TicketFacade.GetTicketByTicketId(new Guid(eventAppid));
                                                    if (objadditionalappnew != null && obticket != null)
                                                    {
                                                        objadditionalappnew.AppointmentDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                                        _Util.Facade.CustomerAppoinmentFacade.UpdateAdditionalMembersAppointment(objadditionalappnew);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var objemp = _Util.Facade.EmployeeFacade.GetEmployeeById(Convert.ToInt32(Eventresid));
                                        var objticket = _Util.Facade.TicketFacade.GetTicketByTicketId(new Guid(eventAppid));
                                        if (objemp != null)
                                        {
                                            var userpermission = _Util.Facade.PermissionFacade.GetPermissionGroupByUserId(objemp.UserId);
                                            if (userpermission != null && (userpermission.Name.ToLower().IndexOf("technician") > -1 || userpermission.Name.ToLower().IndexOf("installation") > -1 || objemp.UserId == new Guid("22222222-2222-2222-2222-222222222222") || userpermission.Tag.ToLower().IndexOf("technician") > -1 || userpermission.Tag.ToLower().IndexOf("installation") > -1))
                                            {
                                                var objapp = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentidByAppoinmentId(new Guid(eventAppid));
                                                var tikuser = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndIsPrimary(objemp.UserId, objticket.TicketId);
                                                if (objapp != null && tikuser != null)
                                                {
                                                    tikuser.UserId = objemp.UserId;
                                                    objapp.AppointmentDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                                    objapp.IsAllDay = true;
                                                    if (spliteventDate.Count == 2)
                                                    {
                                                        var startdatetime = Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1].Replace(".000Z", ""));
                                                        objapp.AppointmentStartTime = startdatetime.ToString("HH:mm");
                                                    }
                                                    if (spliteventEndDate.Count == 2)
                                                    {
                                                        var enddatetime = Convert.ToDateTime(spliteventEndDate[0] + " " + spliteventEndDate[1].Replace(".000Z", ""));
                                                        objapp.AppointmentEndTime = enddatetime.ToString("HH:mm");
                                                    }
                                                    _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinment(objapp);
                                                    _Util.Facade.TicketFacade.UpdateTicketUser(tikuser);
                                                }
                                            }
                                            else
                                            {
                                                message = "User is not a technician, so this user not assigned as an assign member";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return Json(new { result = result, message = message });
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Error" });
            }
        }

        [HttpPost]
        public JsonResult ScheduleEventDetails(Guid? userid, string type, string date)
        {
            bool result = false;
            List<TicketUser> model = new List<TicketUser>();
            if (userid.HasValue && userid.Value != new Guid() && !string.IsNullOrWhiteSpace(type) && !string.IsNullOrWhiteSpace(date))
            {
                type = System.Web.HttpUtility.UrlDecode(type);
                var mindate = date + " 00:00:00.000";
                var maxdate = date + " 23:59:59.999";
                var ticketuser = _Util.Facade.TicketFacade.GetTicketUserListAndCustomerAppointmentByUserId(userid.Value, type, mindate, maxdate);
                if (ticketuser != null && ticketuser.Count > 0)
                {
                    model = ticketuser;
                    result = true;
                }
            }
            return Json(new { result = result, model = model });
        }

        public JsonResult DroppingPermissionScheduleCalendar(string eventType, int? eventId, string eventDate, string eventAppid, bool eventAllDay, string eventEndDate, string Eventresid, string dragresid, string ViewName, string CustomId, string eventticketid)
        {
            try
            {
                bool result = false;
                bool ExistUserAssign = false;
                bool ExistUserAdditional = false;
                if (eventId.HasValue)
                {
                    if (!string.IsNullOrWhiteSpace(eventticketid) && !string.IsNullOrWhiteSpace(Eventresid))
                    {
                        var objticket = _Util.Facade.TicketFacade.GetTicketByTicketId(new Guid(eventticketid));
                        var empobj = _Util.Facade.EmployeeFacade.GetEmployeeById(Convert.ToInt32(Eventresid));
                        if (objticket != null && empobj != null)
                        {
                            var objticketuser = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndUserIdAndNotification(objticket.TicketId, empobj.UserId);
                            var objpriticket = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndUserIdAndIsPrimary(empobj.UserId, objticket.TicketId);
                            if (objpriticket != null)
                            {
                                ExistUserAssign = true;
                            }
                            else if (objticketuser != null)
                            {
                                ExistUserAdditional = true;
                            }
                            else
                            {
                                result = true;
                            }
                        }
                    }
                }
                return Json(new { result = result, ExistUserAssign = ExistUserAssign, ExistUserAdditional = ExistUserAdditional });
            }
            catch (Exception)
            {
                return Json(new { result = false });
            }
        }
        [Authorize]
        public ActionResult ScheduleGoogleMap(string date, string type, string user)
        {
            bool IsType = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            string GoogleMapAPIKey = _Util.Facade.GlobalSettingsFacade.GetGoogleMapAPIKeyByCompanyId(currentLoggedIn.CompanyId.Value);
            ViewBag.GoogleMapAPIKey = GoogleMapAPIKey;
            List<PreCustomerNote> model = new List<PreCustomerNote>();
            try
            {
                if (!string.IsNullOrWhiteSpace(date))
                {
                    var spdate = date.Split('T');
                    if (spdate.Length > 0)
                    {
                        date = spdate[0];
                    }
                    if (!string.IsNullOrWhiteSpace(type) && type != "null")
                    {
                        var sptype = type.Split(',');
                        if (sptype.Length > 0)
                        {
                            List<SelectListItem> TicketTypeList = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Where(x => x.DataValue != "-1" && x.AlterDisplayText1 != null && x.AlterDisplayText1 != "" && x.AlterDisplayText1 == "True").OrderBy(x => x.DisplayText).ToList().Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
                            foreach(var itemType in TicketTypeList)
                            {
                                if(sptype[0] == itemType.Text)
                                {
                                    IsType = true;
                                }
                                break;
                            }
                            type = string.Format("'{0}'", string.Join("','", sptype.Select(i => i.Replace("'", "''"))));
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(user) && user != "null")
                    {
                        var spuser = user.Split(',');
                        if (spuser.Length > 0)
                        {
                            user = string.Format("'{0}'", string.Join("','", spuser.Select(i => i.Replace("'", "''"))));
                        }
                    }
                    bool ispermit = false;
                    if (IsPermitted(UserPermissions.CustomerTicketPermission.ShowLostTicket))
                    {
                        ispermit = true;
                    }
                    model = _Util.Facade.ScheduleFacade.GetSchedulingByCompanyIdAndFilterForGoogleMap(currentLoggedIn.CompanyId.Value, date, type, user, ispermit, IsType);
                }
                ViewBag.ZoomLevel = _Util.Facade.GlobalSettingsFacade.GetMapZoomLevel(currentLoggedIn.CompanyId.Value);
                if (model.Count > 0)
                {
                    foreach (var data in model)
                    {
                        if (!string.IsNullOrWhiteSpace(data.EventLocationFlag))
                        {
                            data.EventLatitude = data.EventLocationFlag.Split(',')[0];
                            data.EventLongitude = data.EventLocationFlag.Split(',')[1];
                        }
                        else
                        {
                            var address = "";
                            if (!string.IsNullOrWhiteSpace(data.EventStreet)) { address += data.EventStreet + " "; }
                            if (!string.IsNullOrWhiteSpace(data.EventLocate)) { address += data.EventLocate; }
                            var map = getGoogleMapInfo(address, GoogleMapAPIKey);
                            if (map.status.ToLower() == "ok" && map.results != null && map.results.Count > 0)
                            {
                                var Latitude = map.results[0].geometry.location.lat.ToString();
                                var Longitude = map.results[0].geometry.location.lng.ToString();
                                data.EventLatitude = Latitude;
                                data.EventLongitude = Longitude;
                                #region Update customer by latitude and longitude value
                                Customer _customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Guid.Parse(data.EventCusId));
                                _customer.Latlng = Latitude + ',' + Longitude;
                                _Util.Facade.CustomerFacade.UpdateCustomer(_customer);
                                #endregion
                            }
                        }

                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult OnlyEventResizeHandlerCalendar(string eventType, int? eventId, string eventDate, string eventAppid, bool eventAllDay, string eventEndDate, string Eventresid, string dragresid, string ViewName, string CustomId, string eventticketid, string additional, bool? chkassign, bool? isexist)
        {
            try
            {
                var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
                bool result = false;
                bool dropres = false;
                string message = "";
                DateTime Endremdate = new DateTime();
                var reminderEndTime = "";
                List<string> spliteventDate = new List<string>();
                List<string> spliteventEndDate = new List<string>();
                bool tryparseResult = DateTime.TryParse(eventDate, out Endremdate);
                if (!tryparseResult) { return Json(new { result = false, message = "Invalid appointment start date and time" }); }
                if (!string.IsNullOrWhiteSpace(eventDate))
                {
                    spliteventDate = eventDate.Split('T').ToList();
                }
                if (!string.IsNullOrWhiteSpace(eventEndDate))
                {
                    spliteventEndDate = eventEndDate.Split('T').ToList();
                }
                if (!string.IsNullOrWhiteSpace(eventType) && eventAllDay == false)
                {
                    if (eventType == "Reminder")
                    {
                        if (!string.IsNullOrWhiteSpace(Eventresid))
                        {
                            var userobj = _Util.Facade.EmployeeFacade.GetEmployeeById(Convert.ToInt32(Eventresid));
                            if (userobj != null)
                            {
                                var remdate = (Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1]));
                                if (!string.IsNullOrWhiteSpace(eventEndDate))
                                {
                                    Endremdate = (Convert.ToDateTime(spliteventEndDate[0] + " " + spliteventEndDate[1]));
                                }
                                else
                                {
                                    Endremdate = remdate.AddHours(2);
                                }
                                var remlist = _Util.Facade.NotesFacade.GetReminderScheduleByReminderDateAndEmployeeId(remdate.ToString("yyyy-MM-dd HH:mm:ss"), userobj.UserId, Endremdate.ToString("yyyy-MM-dd HH:mm:ss"));
                                var remlist1 = _Util.Facade.NotesFacade.GetReminderSchedule1ByReminderDateAndEmployeeId(remdate.ToString("yyyy-MM-dd HH:mm:ss"), userobj.UserId, Endremdate.ToString("yyyy-MM-dd HH:mm:ss"));
                                if (eventId.HasValue)
                                {
                                    var objreminder = _Util.Facade.NotesFacade.GetNotesById(eventId.Value);
                                    if (objreminder != null)
                                    {
                                        string datetime = spliteventDate[0] + " " + objreminder.ReminderDate.Value.Hour + ":" + objreminder.ReminderDate.Value.Minute + ":00";
                                        objreminder.IsAllDay = false;
                                        if (spliteventDate.Count > 0)
                                        {
                                            var reminderTime = Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1]).ToString("HH:mm", CultureInfo.InvariantCulture);
                                            objreminder.ReminderDate = Convert.ToDateTime(spliteventDate[0] + " " + reminderTime);
                                            if (!string.IsNullOrWhiteSpace(eventEndDate))
                                            {
                                                reminderEndTime = Convert.ToDateTime(spliteventEndDate[0] + " " + spliteventEndDate[1]).ToString("HH:mm", CultureInfo.InvariantCulture);
                                                objreminder.ReminderEndDate = Convert.ToDateTime(spliteventEndDate[0] + " " + reminderEndTime);
                                            }
                                            else
                                            {
                                                reminderEndTime = Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1]).AddHours(2).ToString("HH:mm", CultureInfo.InvariantCulture);
                                                objreminder.ReminderEndDate = Convert.ToDateTime(spliteventDate[0] + " " + reminderTime).AddHours(2);
                                            }
                                        }
                                        else
                                        {
                                            objreminder.ReminderDate = Convert.ToDateTime(datetime);
                                            objreminder.ReminderEndDate = objreminder.ReminderDate.Value.AddHours(2);
                                        }
                                        result = _Util.Facade.NotesFacade.UpdateNotes(objreminder);
                                        var timeval = objreminder.ReminderDate.Value.ToString("HH:mm");
                                        var objLookup = _Util.Facade.LookupFacade.GetLookupByKeyAndValue("Arrival", timeval);
                                        if (objLookup != null)
                                        {
                                            objLookup.IsActive = true;
                                            result = _Util.Facade.LookupFacade.UpdateLookUp(objLookup);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(eventticketid) && !string.IsNullOrWhiteSpace(Eventresid))
                        {
                            var objticket = _Util.Facade.TicketFacade.GetTicketByTicketId(new Guid(eventticketid));
                            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeById(Convert.ToInt32(Eventresid));
                            if (objticket != null && objemp != null)
                            {
                                var objticketuser = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndUserIdAndNotification(new Guid(eventAppid), objemp.UserId);
                                if (objticketuser != null)
                                {
                                    var objadditionalapp = _Util.Facade.CustomerAppoinmentFacade.GetAdditionalMembersAppointmentByAppointmentIdAndUserId(new Guid(eventAppid), objemp.UserId);
                                    if (objadditionalapp != null)
                                    {
                                        objadditionalapp.AppointmentDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                        var startdatetime = Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1].Replace(":00Z", ""));
                                        objadditionalapp.AppointmentStartTime = startdatetime.ToString("HH:mm");
                                        if (spliteventEndDate.Count > 0)
                                        {
                                            var enddatetime = Convert.ToDateTime(spliteventEndDate[0] + " " + spliteventEndDate[1].Replace(":00.000Z", ""));
                                            objadditionalapp.AppointmentEndTime = enddatetime.ToString("HH:mm");
                                        }
                                        else
                                        {
                                            var enddatetime = Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1].Replace(":00Z", "")).AddHours(2);
                                            objadditionalapp.AppointmentEndTime = enddatetime.ToString("HH:mm");
                                        }
                                        objadditionalapp.IsAllDay = false;
                                        _Util.Facade.CustomerAppoinmentFacade.UpdateAdditionalMembersAppointment(objadditionalapp);
                                    }
                                }
                                else
                                {
                                    var objappointment = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentidByAppoinmentId(new Guid(eventAppid));
                                    if (objappointment != null)
                                    {
                                        objappointment.AppointmentDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                        var startdatetime = Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1].Replace(":00Z", ""));
                                        objappointment.AppointmentStartTime = startdatetime.ToString("HH:mm");
                                        if (spliteventEndDate.Count > 0)
                                        {
                                            var enddatetime = Convert.ToDateTime(spliteventEndDate[0] + " " + spliteventEndDate[1].Replace(":00.000Z", ""));
                                            objappointment.AppointmentEndTime = enddatetime.ToString("HH:mm");
                                        }
                                        else
                                        {
                                            var enddatetime = Convert.ToDateTime(spliteventDate[0] + " " + spliteventDate[1].Replace(":00Z", "")).AddHours(2);
                                            objappointment.AppointmentEndTime = enddatetime.ToString("HH:mm");
                                        }
                                        objticket.CompletionDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                        objappointment.IsAllDay = false;
                                        _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinment(objappointment);
                                        _Util.Facade.TicketFacade.UpdateTicket(objticket);
                                    }
                                }
                            }
                        }
                    }
                }

                return Json(new { result = result, message = message });
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Error" });
            }

        }

        [HttpPost]
        public JsonResult UserPermissionForCreateTicket(string Eventresid, string eventloaddate, string customerid, string ticketid)
        {
            try
            {
                bool result = false;
                string message = "";
                if (!string.IsNullOrWhiteSpace(Eventresid))
                {
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeById(Convert.ToInt32(Eventresid));
                    if (empobj != null)
                    {
                        var userpermission = _Util.Facade.PermissionFacade.GetPermissionGroupByUserId(empobj.UserId);
                        if (userpermission != null && (userpermission.Name.ToLower().IndexOf("sysadmin") > -1 || userpermission.Name.ToLower().IndexOf("admin") > -1 || userpermission.Name.ToLower().IndexOf("technician") > -1 || userpermission.Name.ToLower().IndexOf("installation") > -1 || empobj.UserId == new Guid("22222222-2222-2222-2222-222222222222") || userpermission.Tag.ToLower().IndexOf("technician") > -1 || userpermission.Tag.ToLower().IndexOf("installation") > -1))
                        {
                            result = true;
                        }
                        else
                        {
                            message = "user has not permission to create ticket";
                        }
                    }
                }
                return Json(new { result = result, message = message });
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "Error" });
            }
        }

        public List<HolidayEmployeeInfoModel> HolidayChecker(List<string> employeeList, List<Pto> dataList, DateTime date, string status)
        {
            List<HolidayEmployeeInfoModel> HolidayList = new List<HolidayEmployeeInfoModel>();
            List<HolidayEmployeeInfoModel> PtoList = new List<HolidayEmployeeInfoModel>();
            try
            {
                if (employeeList.Count > 0 && dataList.Count > 0)
                {
                    foreach (var item in dataList)
                    {
                        if (item.Type.ToLower() == "fullday" && !string.IsNullOrEmpty(item.StartDate.ToString()) && !string.IsNullOrWhiteSpace(item.StartDate.ToString()) && !string.IsNullOrEmpty(item.EndDate.ToString()) && !string.IsNullOrWhiteSpace(item.EndDate.ToString()))
                        {
                            if (PtoList.Count > 0)
                            {
                                bool Flag = false;
                                int index = 0, number = -1;
                                foreach (var str in PtoList)
                                {
                                    if (str.StartDate == Convert.ToDateTime(item.StartDate).ToString("yyyy-MM-dd") && str.UserId == item.UserId && str.Type.ToLower() == "fullday")
                                    {
                                        Flag = true;
                                    }
                                    else if (str.StartDate == Convert.ToDateTime(item.StartDate).ToString("yyyy-MM-dd") && str.UserId == item.UserId && str.Type.ToLower() != "fullday")
                                    {
                                        Flag = true;
                                        number = index;
                                    }
                                    index += 1;
                                }
                                if (!Flag)
                                {
                                    var pto = new HolidayEmployeeInfoModel();
                                    pto.UserId = item.UserId;
                                    pto.StartDate = Convert.ToDateTime(item.StartDate).ToString("yyyy-MM-dd");
                                    pto.EndDate = Convert.ToDateTime(item.EndDate).ToString("yyyy-MM-dd");
                                    pto.StartTime = item.TimeFrom;
                                    pto.EndTime = item.TimeTo;
                                    pto.PaidStatus = item.Payable.ToString();
                                    pto.Status = item.Status;
                                    pto.Type = item.Type;
                                    PtoList.Add(pto);
                                }
                                else if (Flag && number > -1)
                                {
                                    var pto = new HolidayEmployeeInfoModel();
                                    pto.UserId = item.UserId;
                                    pto.StartDate = Convert.ToDateTime(item.StartDate).ToString("yyyy-MM-dd");
                                    pto.EndDate = Convert.ToDateTime(item.EndDate).ToString("yyyy-MM-dd");
                                    pto.StartTime = item.TimeFrom;
                                    pto.EndTime = item.TimeTo;
                                    pto.PaidStatus = item.Payable.ToString();
                                    pto.Status = item.Status;
                                    pto.Type = item.Type;
                                    PtoList[number] = pto;
                                    //PtoList.Add(pto);
                                }
                            }
                            else
                            {
                                var pto = new HolidayEmployeeInfoModel();
                                pto.UserId = item.UserId;
                                pto.StartDate = Convert.ToDateTime(item.StartDate).ToString("yyyy-MM-dd");
                                pto.EndDate = Convert.ToDateTime(item.EndDate).ToString("yyyy-MM-dd");
                                pto.StartTime = item.TimeFrom;
                                pto.EndTime = item.TimeTo;
                                pto.PaidStatus = item.Payable.ToString();
                                pto.Status = item.Status;
                                pto.Type = item.Type;
                                PtoList.Add(pto);
                            }

                        }
                        else if (item.Type.ToLower() == "multipleday" && !string.IsNullOrEmpty(item.StartDate.ToString()) && !string.IsNullOrWhiteSpace(item.StartDate.ToString()) && !string.IsNullOrEmpty(item.EndDate.ToString()) && !string.IsNullOrWhiteSpace(item.EndDate.ToString()))
                        {
                            var stDate = Convert.ToDateTime(item.StartDate);
                            var edDate = Convert.ToDateTime(item.EndDate);
                            if (stDate <= edDate)
                            {
                                edDate = edDate.AddDays(1);
                                while (stDate != edDate)
                                {
                                    if (PtoList.Count > 0)
                                    {
                                        bool Flag = false;
                                        int index = 0, number = -1;
                                        foreach (var str in PtoList)
                                        {
                                            if (str.StartDate == stDate.ToString("yyyy-MM-dd") && str.UserId == item.UserId && str.Type.ToLower() == "fullday")
                                            {
                                                Flag = true;
                                            }
                                            else if (str.StartDate == Convert.ToDateTime(item.StartDate).ToString("yyyy-MM-dd") && str.UserId == item.UserId && str.Type.ToLower() != "fullday")
                                            {
                                                Flag = true;
                                                number = index;
                                            }
                                            index += 1;
                                        }
                                        if (!Flag)
                                        {
                                            var pto = new HolidayEmployeeInfoModel();
                                            pto.UserId = item.UserId;
                                            pto.StartDate = stDate.ToString("yyyy-MM-dd");
                                            pto.EndDate = stDate.ToString("yyyy-MM-dd");
                                            pto.StartTime = item.TimeFrom;
                                            pto.EndTime = item.TimeTo;
                                            pto.PaidStatus = item.Payable.ToString();
                                            pto.Status = item.Status;
                                            pto.Type = "FullDay";
                                            PtoList.Add(pto);
                                        }
                                        else if (Flag && number > -1)
                                        {
                                            var pto = new HolidayEmployeeInfoModel();
                                            pto.UserId = item.UserId;
                                            pto.StartDate = stDate.ToString("yyyy-MM-dd");
                                            pto.EndDate = stDate.ToString("yyyy-MM-dd");
                                            pto.StartTime = item.TimeFrom;
                                            pto.EndTime = item.TimeTo;
                                            pto.PaidStatus = item.Payable.ToString();
                                            pto.Status = item.Status;
                                            pto.Type = "FullDay";
                                            PtoList[number] = pto;
                                            //PtoList.Add(pto);
                                        }
                                    }
                                    else
                                    {
                                        var pto = new HolidayEmployeeInfoModel();
                                        pto.UserId = item.UserId;
                                        pto.StartDate = stDate.ToString("yyyy-MM-dd");
                                        pto.EndDate = stDate.ToString("yyyy-MM-dd");
                                        pto.StartTime = item.TimeFrom;
                                        pto.EndTime = item.TimeTo;
                                        pto.PaidStatus = item.Payable.ToString();
                                        pto.Status = item.Status;
                                        pto.Type = "FullDay";
                                        PtoList.Add(pto);
                                    }
                                    stDate = stDate.AddDays(1);
                                }
                            }
                        }
                        else
                        {
                            var pto = new HolidayEmployeeInfoModel();
                            pto.UserId = item.UserId;
                            if (!string.IsNullOrEmpty(item.StartDate.ToString()) && !string.IsNullOrWhiteSpace(item.StartDate.ToString()))
                            {
                                pto.StartDate = Convert.ToDateTime(item.StartDate).ToString("yyyy-MM-dd");
                            }
                            if (!string.IsNullOrEmpty(item.EndDate.ToString()) && !string.IsNullOrWhiteSpace(item.EndDate.ToString()))
                            {
                                pto.EndDate = Convert.ToDateTime(item.EndDate).ToString("yyyy-MM-dd");
                            }
                            pto.StartTime = item.TimeFrom;
                            pto.EndTime = item.TimeTo;
                            pto.PaidStatus = item.Payable.ToString();
                            pto.Status = item.Status;
                            pto.Type = item.Type;
                            PtoList.Add(pto);
                        }
                    }
                    if (PtoList.Count > 0)
                    {
                        PtoList = PtoList.OrderBy(x => x.Type == "FullDay").ToList();
                        foreach (var pto in PtoList)
                        {
                            if (status == "Daily")
                            {
                                if (pto.StartDate == date.ToString("yyyy-MM-dd") || pto.EndDate == date.ToString("yyyy-MM-dd"))
                                {
                                    foreach (string user in employeeList)
                                    {
                                        if (user == pto.UserId.ToString())
                                        {
                                            if (HolidayList.Count > 0)
                                            {
                                                bool flag = false;
                                                int indexNumber = -1;
                                                int index = 0;
                                                foreach (var check in HolidayList)
                                                {
                                                    if (check.UserId == pto.UserId && check.StartDate == pto.StartDate && check.Type == "FullDay")
                                                    {
                                                        flag = true;
                                                    }
                                                    else if (check.UserId == pto.UserId && check.StartDate == pto.StartDate && check.Type != "FullDay")
                                                    {
                                                        flag = true;
                                                        indexNumber = index;
                                                    }
                                                    index += 1;
                                                }
                                                if (!flag)
                                                {
                                                    HolidayList.Add(pto);
                                                }
                                                else if (flag && indexNumber > -1)
                                                {
                                                    HolidayList[indexNumber] = pto;
                                                }

                                            }
                                            else
                                            {
                                                HolidayList.Add(pto);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (status == "Weekly")
                            {
                                var endDate = date.AddDays(6);
                                if (Convert.ToDateTime(pto.StartDate) >= date && Convert.ToDateTime(pto.StartDate) <= endDate || Convert.ToDateTime(pto.EndDate) >= date && Convert.ToDateTime(pto.EndDate) <= endDate)
                                {
                                    foreach (string user in employeeList)
                                    {
                                        if (user == pto.UserId.ToString())
                                        {
                                            if (HolidayList.Count > 0)
                                            {
                                                bool flag = false;
                                                int indexNumber = -1;
                                                int index = 0;
                                                foreach (var check in HolidayList)
                                                {
                                                    if (check.UserId == pto.UserId && check.StartDate == pto.StartDate && check.Type == "FullDay")
                                                    {
                                                        flag = true;
                                                    }
                                                    else if (check.UserId == pto.UserId && check.StartDate == pto.StartDate && check.Type != "FullDay")
                                                    {
                                                        flag = true;
                                                        indexNumber = index;
                                                    }
                                                    index += 1;
                                                }
                                                if (!flag)
                                                {
                                                    HolidayList.Add(pto);
                                                }
                                                else if (flag && indexNumber > -1)
                                                {
                                                    HolidayList[indexNumber] = pto;
                                                }

                                            }
                                            else
                                            {
                                                HolidayList.Add(pto);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return HolidayList;
            }
            catch (Exception)
            {
                return HolidayList;
            }
        }
        public JsonResult EmployeeHolidayChecker(string employeeId, string date)
        {
            try
            {
                bool result = false;
                string message = "";
                string disableDate = "";
                string operationsdisableDate = "";
                List<HolidayEmployeeInfoModel> PtoList = new List<HolidayEmployeeInfoModel>();
                HolidayEmployeeInfoModel employee = new HolidayEmployeeInfoModel();
                List<string> list = new List<string>();
                if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrEmpty(date) && !string.IsNullOrWhiteSpace(employeeId) && !string.IsNullOrWhiteSpace(date))
                {
                    DateTime selecteddate = date!="null" && !string.IsNullOrEmpty(date) ? DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture) : new DateTime();
                    
                    var employeelist = _Util.Facade.PtoFacade.GetAllPtoListByUserIdForTicket(Guid.Parse(employeeId));
                    var EmployeeOperationsList = _Util.Facade.PtoFacade.GetAllEmployeeOperationsListByUserIdForTicket(Guid.Parse(employeeId), selecteddate);
                    if (EmployeeOperationsList.Count > 0)
                    {
                        foreach (var item in EmployeeOperationsList)
                        {
                            if(item.SelectedDate.HasValue && item.SelectedDate.Value != new DateTime())
                            {

                           
                            list.Add(item.SelectedDate.Value.ToString("yyyy-MM-dd"));
                            foreach (var items in list)
                            {
                                if (!string.IsNullOrEmpty(operationsdisableDate) && !string.IsNullOrWhiteSpace(operationsdisableDate))
                                {
                                        operationsdisableDate = operationsdisableDate + "," + items;
                                }
                                else
                                {
                                        operationsdisableDate = items;
                                }

                                }
                            }
                        } 
                    }
                    if (employeelist.Count > 0)
                    {
                        foreach (var item in employeelist)
                        {
                            if (item.Status.ToLower() != "rejected")
                            {
                                if (item.Type.ToLower() == "fullday")
                                {
                                    if (list.Count > 0)
                                    {
                                        bool Flag = false;
                                        foreach (var str in list)
                                        {
                                            if (str == (Convert.ToDateTime(item.StartDate)).ToString("yyyy-MM-dd"))
                                            {
                                                Flag = true;
                                            }
                                        }
                                        if (!Flag)
                                        {
                                            list.Add((Convert.ToDateTime(item.StartDate)).ToString("yyyy-MM-dd"));
                                            var pto = new HolidayEmployeeInfoModel();
                                            pto.UserId = item.UserId;
                                            if (!string.IsNullOrEmpty(item.StartDate.ToString()) && !string.IsNullOrWhiteSpace(item.StartDate.ToString()))
                                            {
                                                pto.StartDate = Convert.ToDateTime(item.StartDate).ToString("MM/dd/yyyy");
                                            }
                                            if (!string.IsNullOrEmpty(item.EndDate.ToString()) && !string.IsNullOrWhiteSpace(item.EndDate.ToString()))
                                            {
                                                pto.EndDate = Convert.ToDateTime(item.EndDate).ToString("MM/dd/yyyy");
                                            }
                                            pto.StartTime = item.TimeFrom;
                                            pto.EndTime = item.TimeTo;
                                            pto.PaidStatus = item.Payable.ToString();
                                            pto.Status = item.Status;
                                            pto.Type = item.Type;
                                            PtoList.Add(pto);
                                        }
                                    }
                                    else
                                    {
                                        list.Add((Convert.ToDateTime(item.StartDate)).ToString("yyyy-MM-dd"));
                                        var pto = new HolidayEmployeeInfoModel();
                                        pto.UserId = item.UserId;
                                        if (!string.IsNullOrEmpty(item.StartDate.ToString()) && !string.IsNullOrWhiteSpace(item.StartDate.ToString()))
                                        {
                                            pto.StartDate = Convert.ToDateTime(item.StartDate).ToString("MM/dd/yyyy");
                                        }
                                        if (!string.IsNullOrEmpty(item.EndDate.ToString()) && !string.IsNullOrWhiteSpace(item.EndDate.ToString()))
                                        {
                                            pto.EndDate = Convert.ToDateTime(item.EndDate).ToString("MM/dd/yyyy");
                                        }
                                        pto.StartTime = item.TimeFrom;
                                        pto.EndTime = item.TimeTo;
                                        pto.PaidStatus = item.Payable.ToString();
                                        pto.Status = item.Status;
                                        pto.Type = item.Type;
                                        PtoList.Add(pto);
                                    }

                                }
                                else if (item.Type.ToLower() == "multipleday")
                                {
                                    if (!string.IsNullOrEmpty(item.StartDate.ToString()) && !string.IsNullOrWhiteSpace(item.StartDate.ToString()) && !string.IsNullOrEmpty(item.EndDate.ToString()) && !string.IsNullOrWhiteSpace(item.EndDate.ToString()))
                                    {
                                        var stDate = Convert.ToDateTime(item.StartDate);
                                        var edDate = Convert.ToDateTime(item.EndDate);
                                        if (stDate <= edDate)
                                        {
                                            edDate = edDate.AddDays(1);
                                            while (stDate != edDate)
                                            {
                                                if (list.Count > 0)
                                                {
                                                    bool Flag = false;
                                                    foreach (var str in list)
                                                    {
                                                        if (str == stDate.ToString("yyyy-MM-dd"))
                                                        {
                                                            Flag = true;
                                                        }
                                                    }
                                                    if (!Flag)
                                                    {
                                                        list.Add(stDate.ToString("yyyy-MM-dd"));
                                                        var pto = new HolidayEmployeeInfoModel();
                                                        pto.UserId = item.UserId;
                                                        pto.StartDate = stDate.ToString("MM/dd/yyyy");
                                                        pto.EndDate = stDate.ToString("MM/dd/yyyy");
                                                        pto.StartTime = item.TimeFrom;
                                                        pto.EndTime = item.TimeTo;
                                                        pto.PaidStatus = item.Payable.ToString();
                                                        pto.Status = item.Status;
                                                        pto.Type = "FullDay";
                                                        PtoList.Add(pto);
                                                    }
                                                }
                                                else
                                                {
                                                    list.Add(stDate.ToString("yyyy-MM-dd"));
                                                    var pto = new HolidayEmployeeInfoModel();
                                                    pto.UserId = item.UserId;
                                                    pto.StartDate = stDate.ToString("MM/dd/yyyy");
                                                    pto.EndDate = stDate.ToString("MM/dd/yyyy");
                                                    pto.StartTime = item.TimeFrom;
                                                    pto.EndTime = item.TimeTo;
                                                    pto.PaidStatus = item.Payable.ToString();
                                                    pto.Status = item.Status;
                                                    pto.Type = "FullDay";
                                                    PtoList.Add(pto);
                                                }
                                                stDate = stDate.AddDays(1);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var pto = new HolidayEmployeeInfoModel();
                                    pto.UserId = item.UserId;
                                    if (!string.IsNullOrEmpty(item.StartDate.ToString()) && !string.IsNullOrWhiteSpace(item.StartDate.ToString()))
                                    {
                                        pto.StartDate = Convert.ToDateTime(item.StartDate).ToString("MM/dd/yyyy");
                                    }
                                    if (!string.IsNullOrEmpty(item.EndDate.ToString()) && !string.IsNullOrWhiteSpace(item.EndDate.ToString()))
                                    {
                                        pto.EndDate = Convert.ToDateTime(item.EndDate).ToString("MM/dd/yyyy");
                                    }
                                    pto.StartTime = item.TimeFrom;
                                    pto.EndTime = item.TimeTo;
                                    pto.PaidStatus = item.Payable.ToString();
                                    pto.Status = item.Status;
                                    pto.Type = item.Type;
                                    PtoList.Add(pto);
                                }
                            }
                        }
                         
                        foreach (var item in list)
                        {
                            if (!string.IsNullOrEmpty(disableDate) && !string.IsNullOrWhiteSpace(disableDate))
                            {
                                disableDate = disableDate + "," + item;
                            }
                            else
                            {
                                disableDate = item;
                            }
                        }

                        if (PtoList.Count > 0)
                        {
                            string emp = _Util.Facade.EmployeeFacade.GetEmployeeNumByEmployeeId(Guid.Parse(employeeId));
                            foreach (var pto in PtoList)
                            {
                                if (pto.StartDate == date || pto.EndDate == date)
                                {
                                    if (pto.Type.ToLower() == "fullday")
                                    {
                                        employee.Name = emp;
                                        employee.Type = pto.Type;
                                        employee.StartDate = pto.StartDate;
                                        employee.EndDate = pto.EndDate;
                                        employee.StartTime = pto.StartTime;
                                        employee.EndTime = pto.EndTime;
                                        employee.Status = pto.Status;
                                        if (pto.PaidStatus.ToLower() == "true")
                                        {
                                            employee.PaidStatus = "paid";
                                        }
                                        else
                                        {
                                            employee.PaidStatus = "unpaid";
                                        }
                                        message = "Employee Information Found";
                                        result = true;
                                        break;
                                    }
                                    else
                                    {
                                        employee.Name = emp;
                                        employee.Type = pto.Type;
                                        employee.StartDate = pto.StartDate;
                                        employee.EndDate = pto.EndDate;
                                        employee.StartTime = pto.StartTime;
                                        employee.EndTime = pto.EndTime;
                                        employee.Status = pto.Status;
                                        if (pto.PaidStatus.ToLower() == "true")
                                        {
                                            employee.PaidStatus = "paid";
                                        }
                                        else
                                        {
                                            employee.PaidStatus = "unpaid";
                                        }
                                        message = "Employee Information Found";
                                        result = true;
                                    }
                                }
                            }
                        }
                    }
                }
                return Json(new { result = result, message = message, calendarlist = disableDate, operationsdisableDate = operationsdisableDate, employeeinfo = employee });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = "Error" });
            }
        }
        #region Calendar Settings
        [Authorize]
        public PartialViewResult PTOPartial()
        {
            try
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(CurrentUser.UserId);
                ViewBag.HoursRemaining = emp.PtoRemain.HasValue ? emp.PtoRemain.Value : 0;
                return PartialView("_PTOPartial");
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
        }
        [Authorize]
        public PartialViewResult AddPtoPartial(int? Id)
        {
            try
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                Pto Model = new Pto();

                if (Id.HasValue && Id > 0)
                {
                    Model = _Util.Facade.PtoFacade.GetPtoById(Id);
                    if (Model == null || Model.UserId != CurrentUser.UserId)
                    {
                        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                    }
                }
                else
                {
                    Model.StartDate = DateTime.Now.AddDays(1);
                    Model.EndDate = DateTime.Now.AddDays(1);
                }
                #region ViewBags
                List<SelectListItem> list = new List<SelectListItem>();
                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(CurrentUser.UserId);
                ViewBag.HoursRemaining = emp.PtoRemain.HasValue ? emp.PtoRemain.Value : 0;
                var LeaveType = _Util.Facade.LookupFacade.GetLookupByKey("AbsenceType").OrderBy(x => x.DisplayText != "Select Absence Type").ThenBy(x => x.DisplayText).Select(x =>
                     new SelectListItem()
                     {
                         Text = x.DisplayText.ToString(),
                         Value = x.DataValue.ToString()
                     }).ToList();
                var result = LeaveType.OrderBy(x => x.Value != "FullDay").ThenBy(i => i.Value).ToList();
                ViewBag.LeaveType = result;
                ViewBag.ArrivalTime = _Util.Facade.LookupFacade.GetLookupByKey("AbsenceCustomTime").Where(x => x.IsActive == true).Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList();
                #endregion

                return PartialView("_AddPtoPartial", Model);
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
        }

        [Authorize]
        public ActionResult CalendarSettings(int? id)
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            if (id.HasValue)
            {
                ViewBag.id = id.Value;
            }
            return View();
        }

        [Authorize]
        public ActionResult CalendarSetup()
        {
            return PartialView("_CalendarSetupDetails");
        }
        [Authorize]
        public PartialViewResult CalendarSettingsPartial()
        {
            if (!base.IsPermitted(UserPermissions.SchedulePermission.CustomCalendarSettingsPermission))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            try
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                List<GlobalSetting> SettingList = new List<GlobalSetting>();
                List<GlobalSetting> Settings = _Util.Facade.GlobalSettingsFacade.GetCalendarGlobalSettingsByCompanyId(CurrentUser.CompanyId.Value);
                foreach (var item in Settings)
                {
                    GlobalSetting model = new GlobalSetting();
                    if (item.SearchKey == "CustomCalendarTopRowEmployee")
                    {
                        var emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(Guid.Parse(item.Value));
                        item.Value = emp.FirstName + " " + emp.LastName;
                        item.SearchKey = "Admin Employee";
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "ScheduleCalendarDefaultView")
                    {
                        item.SearchKey = "Calendar Default View";
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "ScheduleCalendarMinTimeRange")
                    {
                        item.SearchKey = "Calendar Start Time";
                        int hh = 0;
                        int mm = 0;
                        int.TryParse(item.Value.Split(':')[0], out hh);
                        int.TryParse(item.Value.Split(':')[1], out mm);
                        if (hh > 11)
                        {
                            if (hh > 12) { hh = hh - 12; }
                            if (mm > 0)
                            {
                                item.Value = hh + ":" + item.Value.Split(':')[1] + " PM";
                            }
                            else
                            {
                                item.Value = hh + " PM";
                            }
                        }
                        else
                        {
                            if (mm > 0)
                            {
                                item.Value = hh + ":" + item.Value.Split(':')[1] + " AM";
                            }
                            else
                            {
                                item.Value = hh + " AM";
                            }
                        }
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "ScheduleCalendarMaxTimeRange")
                    {
                        item.SearchKey = "Calendar End Time";
                        int hh = 0;
                        int mm = 0;
                        int.TryParse(item.Value.Split(':')[0], out hh);
                        int.TryParse(item.Value.Split(':')[1], out mm);
                        if (hh > 11)
                        {
                            if (hh > 12) { hh = hh - 12; }
                            if (mm > 0)
                            {
                                item.Value = hh + ":" + item.Value.Split(':')[1] + " PM";
                            }
                            else
                            {
                                item.Value = hh + " PM";
                            }
                        }
                        else
                        {
                            if (mm > 0)
                            {
                                item.Value = hh + ":" + item.Value.Split(':')[1] + " AM";
                            }
                            else
                            {
                                item.Value = hh + " AM";
                            }
                        }
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "FirstDayOfWeek")
                    {
                        item.SearchKey = "First Day Of Week";
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "CustomCalendarFontSize")
                    {
                        item.SearchKey = "Font Size";
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "CustomCalendarRMRReportShow")
                    {
                        item.SearchKey = "RMR Report";
                        if (item.Value == "true") { item.Value = "True"; }
                        else { item.Value = "False"; }
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "CustomCalendarScheduleBorderShow")
                    {
                        item.SearchKey = "Task Border";
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "CustomCalendarScheduleShadowShow")
                    {
                        item.SearchKey = "Task Shadow";
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "CustomCalendarTableHeaderColor")
                    {
                        item.SearchKey = "Calendar Header Color";
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "CustomCalendarColumnHourDuration")
                    {
                        item.SearchKey = "Weekly Day Off";
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "EnableSelectedDateReload")
                    {
                        item.SearchKey = "Enable Reload Selected Date";
                        if (item.Value == "true") { item.Value = "True"; }
                        else { item.Value = "False"; }
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "CalendarEventIconResizer")
                    {
                        item.SearchKey = "Calendar Event Icon Resize";
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "CalendarEventTicketResize")
                    {
                        item.SearchKey = "Calendar Event Ticket Resize";
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "CalendarViewShow")
                    {
                        item.SearchKey = "Calendar View";
                        if (item.Value == "vertical") { item.Value = "Vertical"; }
                        else { item.Value = "Horizontal"; }
                        SettingList.Add(item);
                    }
                    else if (item.SearchKey == "CalendarTicketColor")
                    {
                        item.SearchKey = "Ticket Color Selection";
                        SettingList.Add(item);
                    }
                    else
                    {
                        SettingList.Add(item);
                    }
                }
                return PartialView("_SettingsCalendar", SettingList);
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
        }
        [Authorize]
        public PartialViewResult EditSettings(int? id)
        {
            try
            {
                GlobalSetting model;
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                if (CurrentUser == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model = new GlobalSetting();
                if (id.HasValue && id > 0)
                {
                    if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.GlobalSettingsEdit))
                    {
                        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                    }
                    model = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsDetailsById(id.Value);
                }
                if (model.SearchKey == "ScheduleCalendarDefaultView")
                {
                    List<SelectListItem> defaultView = new List<SelectListItem>();
                    defaultView.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("DefaultView").Select(x => new SelectListItem()
                    {
                        Text = x.DisplayText.ToString(),
                        Value = x.DataValue.ToString()
                    }).ToList());
                    ViewBag.DefaultView = defaultView;
                }
                else if (model.SearchKey == "ScheduleCalendarMinTimeRange" || model.SearchKey == "ScheduleCalendarMaxTimeRange")
                {
                    ViewBag.SchedularTime = _Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", null, null).Select(x =>
                    new SelectListItem()
                    {
                        Text = x.DisplayText.ToString(),
                        Value = x.DataValue.ToString()
                    }).ToList();
                    var mintime = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarMinTimeRange(CurrentUser.CompanyId.Value).Split(':');
                    if (mintime.Length > 1)
                    {
                        ViewBag.mintime = mintime[0] + mintime[1];
                    }
                    var maxtime = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarMaxTimeRange(CurrentUser.CompanyId.Value).Split(':');
                    if (maxtime.Length > 1)
                    {
                        ViewBag.maxtime = maxtime[0] + maxtime[1];
                    }
                }
                else if (model.SearchKey == "FirstDayOfWeek")
                {
                    ViewBag.FirstDayOfWeek = _Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("FirstDayOfWeek", null, null).Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()

                }).ToList();
                }
                else if (model.SearchKey == "CustomCalendarTopRowEmployee")
                {
                    List<SelectListItem> EmployeeList = new List<SelectListItem>();
                    var Employees = _Util.Facade.EmployeeFacade.GetEmployeeListCalendarSchedule(new Guid()).OrderBy(x => x.ResourceName).ToList();
                    if (Employees.Count > 0)
                    {
                        foreach (var emp in Employees)
                        {
                            SelectListItem item = new SelectListItem();
                            string[] stringSeparators = new string[] { " (T)" };
                            item.Text = emp.ResourceName.Split(stringSeparators, StringSplitOptions.None)[0];
                            item.Value = emp.UserId.ToString();
                            EmployeeList.Add(item);
                        }
                    }
                    ViewBag.EmployeeList = EmployeeList;
                    List<SelectListItem> PositionList = new List<SelectListItem>();
                    var Calendarview = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("CalendarViewShow");
                    if (Calendarview != null && Calendarview.Value == "vertical")
                    {
                        PositionList.Add(new SelectListItem()
                        {
                            Text = "Left",
                            Value = "topleft"
                        });
                        PositionList.Add(new SelectListItem()
                        {
                            Text = "Right",
                            Value = "bottomright"
                        });
                    }
                    else
                    {
                        PositionList.Add(new SelectListItem()
                        {
                            Text = "Top",
                            Value = "topleft"
                        });
                        PositionList.Add(new SelectListItem()
                        {
                            Text = "Bottom",
                            Value = "bottomright"
                        });
                    }
                    ViewBag.SystemUserPosition = PositionList.ToList();
                }
                else if (model.SearchKey == "CalendarEventIconResizer")
                {
                    ViewBag.ResizingNumber = _Util.Facade.LookupFacade.GetLookupByKey("CalendarEventIconResize").Select(x =>
                     new SelectListItem()
                     {
                         Text = x.DisplayText.ToString(),
                         Value = x.DataValue.ToString()
                     }).ToList();
                }
                else if (model.SearchKey == "CalendarEventTicketResize")
                {
                    var Calendarview = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("CalendarViewShow");
                    ViewBag.Calendarview = "horizontal";
                    if (Calendarview != null && !string.IsNullOrWhiteSpace(Calendarview.Value)) { ViewBag.Calendarview = Calendarview.Value; }
                    ViewBag.ResizingNumber = _Util.Facade.LookupFacade.GetLookupByKey("CalendarEventResizing").Select(x =>
                     new SelectListItem()
                     {
                         Text = x.DisplayText.ToString(),
                         Value = x.DataValue.ToString()
                     }).ToList();
                }
                else if (model.SearchKey == "CalendarViewShow")
                {
                    List<SelectListItem> PositionList = new List<SelectListItem>();
                    PositionList.Add(new SelectListItem()
                    {
                        Text = "Horizontal view",
                        Value = "horizontal"
                    });
                    PositionList.Add(new SelectListItem()
                    {
                        Text = "Vertical view",
                        Value = "vertical"
                    });
                    ViewBag.CalendarPosition = PositionList.ToList();
                }
                else if (model.SearchKey == "CalendarTicketColor")
                {
                    List<SelectListItem> PositionList = new List<SelectListItem>();
                    PositionList.Add(new SelectListItem()
                    {
                        Text = "Ticket Type",
                        Value = "TicketType"
                    });
                    PositionList.Add(new SelectListItem()
                    {
                        Text = "Ticket Status",
                        Value = "TicketStatus"
                    });
                    PositionList.Add(new SelectListItem()
                    {
                        Text = "Employee",
                        Value = "Employee"
                    });
                    ViewBag.CalendarPosition = PositionList.ToList();
                }
                return PartialView("_EditSettings", model);
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
        }

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public JsonResult EditSettings(GlobalSetting globalSetting)
        {
            var result = false;
            try
            {
                var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
                if (currentLoggedIn == null)
                {
                    return Json(result);
                }
                globalSetting.CompanyId = currentLoggedIn.CompanyId.Value;
                if (!currentLoggedIn.CompanyId.HasValue)
                {
                    return Json(result);
                }
                if (globalSetting.Id > 0)
                {
                    GlobalSetting OldGlobalSettingModel = new GlobalSetting();
                    OldGlobalSettingModel = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsDetailsById(globalSetting.Id);
                    if (OldGlobalSettingModel != null)
                    {
                        OldGlobalSettingModel.Value = globalSetting.Value;
                        if (globalSetting.IsActive.HasValue)
                        {
                            OldGlobalSettingModel.IsActive = globalSetting.IsActive.Value;
                        }
                        if (!string.IsNullOrWhiteSpace(globalSetting.OptionalValue))
                        {
                            OldGlobalSettingModel.OptionalValue = globalSetting.OptionalValue;
                        }
                        if (globalSetting.SearchKey == "CustomCalendarTopRowEmployee")
                        {
                            OldGlobalSettingModel.Description = globalSetting.Description;
                        }
                        result = _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(OldGlobalSettingModel);
                    }
                    ClearCache(currentLoggedIn, OldGlobalSettingModel);
                    if (globalSetting.SearchKey == "SetDateFilterRange")
                    {
                        if (Request.Cookies["_DateViewFilter"] != null)
                        {

                            HttpCookie myCookie = new HttpCookie("_DateViewFilter");

                            myCookie.Expires = DateTime.Now;
                            Response.Cookies.Add(myCookie);

                        }
                    }

                }
                return Json(result);
            }
            catch (Exception)
            {
                return Json(result);
            }
        }


        public ActionResult TicketStatusImageSettingPartial(string status)
        {
            try
            {
                var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
                StatusImageSetting model = new StatusImageSetting();
                if (!string.IsNullOrWhiteSpace(status))
                {
                    model.TicketStatusImageSetting = _Util.Facade.TicketFacade.GetStatusImageSettingByCompanyIdAndStatus(currentLoggedIn.CompanyId.Value, status);
                    if (model.TicketStatusImageSetting == null)
                    {
                        model.StatusTicket = status;
                    }
                }
                else
                {
                    model.StatusTicket = "Created";
                }
                List<SelectListItem> TicketList = _Util.Facade.LookupFacade.GetLookupByKey("TicketStatus").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString()
               }).ToList();
                if (TicketList != null && TicketList.Count > 0)
                {
                    ViewBag.TicketStatus = TicketList.OrderBy(x => x.Text).ToList();
                }
                else { ViewBag.TicketStatus = TicketList; }
                return View(model);
            }
            catch (Exception)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }
        }
        public ActionResult TicketTypeColorChange(string status)
        {
            try
            {
                Lookup model = new Lookup();
                if (!string.IsNullOrEmpty(status) && !string.IsNullOrWhiteSpace(status))
                {
                    model = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Where(x => x.DataValue == status).FirstOrDefault();
                }
                else
                {
                    model = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Where(x => x.DataValue != "-1" && x.AlterDisplayText1 != null && x.AlterDisplayText1 != "" && x.AlterDisplayText1 == "True").OrderBy(x => x.DisplayText).FirstOrDefault();
                }
                List<SelectListItem> TicketTypeList = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Where(x => x.DataValue != "-1" && x.AlterDisplayText1 != null && x.AlterDisplayText1 != "" && x.AlterDisplayText1 == "True").OrderBy(x => x.DisplayText).ToList().Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();
                ViewBag.TicketType = TicketTypeList;
                return View(model);
            }
            catch (Exception)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }
        }
        [HttpPost]
        public JsonResult SaveTicketTypeColorChange(Lookup TicketTypeColor)
        {
            bool result = false;
            try
            {
                if (TicketTypeColor.Id > 0)
                {
                    var objtype = _Util.Facade.LookupFacade.GetLookUpById(TicketTypeColor.Id);
                    if (objtype != null)
                    {
                        objtype.AlterDisplayText = TicketTypeColor.AlterDisplayText.Split('#')[1].ToUpper();
                        result = _Util.Facade.LookupFacade.UpdateLookUp(objtype);
                    }
                }
                return Json(result);
            }
            catch (Exception)
            {
                return Json(result);
            }
        }
        [HttpPost]
        public JsonResult SaveTicketStatusImageSetting(TicketStatusImageSetting TicketStatusImageSetting)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            try
            {
                if (TicketStatusImageSetting.Id > 0)
                {
                    var objstatusimage = _Util.Facade.TicketFacade.GetImageSettingById(TicketStatusImageSetting.Id);
                    if (objstatusimage != null)
                    {
                        objstatusimage.Filename = TicketStatusImageSetting.Filename;
                        objstatusimage.TicketStatusColor = TicketStatusImageSetting.TicketStatusColor;
                        result = _Util.Facade.TicketFacade.UpdateStatusImageSetting(objstatusimage);
                    }
                }
                else
                {
                    TicketStatusImageSetting.CompanyId = currentLoggedIn.CompanyId.Value;
                    TicketStatusImageSetting.Uploadeddate = DateTime.Now.UTCCurrentTime();
                    TicketStatusImageSetting.IsActive = true;
                    result = _Util.Facade.TicketFacade.InsertStatusImageSetting(TicketStatusImageSetting) > 0;
                }
                return Json(result);
            }
            catch (Exception)
            {
                return Json(result);
            }
        }
        [HttpPost]
        public JsonResult SaveTicketStatusColor(TicketStatusImageSetting TicketStatusImageSetting)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            try
            {
                if (TicketStatusImageSetting.Id > 0)
                {
                    var objstatusimage = _Util.Facade.TicketFacade.GetImageSettingById(TicketStatusImageSetting.Id);
                    if (objstatusimage != null)
                    {
                        objstatusimage.TicketStatusColor = TicketStatusImageSetting.TicketStatusColor;
                        result = _Util.Facade.TicketFacade.UpdateStatusImageSetting(objstatusimage);
                    }
                }
                return Json(result);
            }
            catch (Exception)
            {
                return Json(result);
            }
        }
        [HttpGet]
        public JsonResult GetAllTechList()
        {
            List<SelectListItem> EmployeeList = new List<SelectListItem>();
            try
            {
                var EmpDataList = _Util.Facade.EmployeeFacade.GetEmployeeListCalendarSchedule(new Guid());
                EmployeeList.AddRange(EmpDataList.OrderBy(x => x.ResourceName).Select(x => new SelectListItem()
                {
                    Text = x.ResourceName.ToString(),
                    Value = x.UserId.ToString().ToLower()
                }).ToList());
                return Json(EmployeeList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(EmployeeList, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateTicketUser(int TicketId, string UserId)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            string result = "";
            try
            {
                if (TicketId > 0 && !string.IsNullOrWhiteSpace(UserId))
                {
                    Ticket TicketData = _Util.Facade.TicketFacade.GetTicketById(TicketId);
                    if (TicketData != null)
                    {
                        Guid AssignedTo = Guid.Empty;

                        if (Guid.TryParse(UserId, out AssignedTo))
                        {
                            var empuser = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(AssignedTo);
                            if (empuser != null)
                            {
                                result = empuser.FirstName + " " + empuser.LastName;
                                TicketReply TR = new TicketReply()
                                {
                                    Message = CurrentUser.GetFullName() + " assigned to " + result + " for this ticket ",
                                    TicketId = TicketData.TicketId,
                                    RepliedDate = DateTime.Now.UTCCurrentTime(),
                                    IsPrivate = true,
                                    UserId = CurrentUser.UserId
                                };
                                TR.Id = _Util.Facade.TicketFacade.InsertTicketReply(TR);
                                var TicketUserInfo = _Util.Facade.TicketFacade.GetTicketUserByTicketId(TicketData.TicketId);
                                if (TicketUserInfo != null && TicketUserInfo.Count > 0)
                                {
                                    var TicketUser = TicketUserInfo.Where(x => x.IsPrimary == true).FirstOrDefault();
                                    TicketUser.UserId = AssignedTo;
                                    TicketUser.AddedBy = CurrentUser.UserId;
                                    TicketUser.AddedDate = DateTime.UtcNow;
                                    _Util.Facade.TicketFacade.UpdateTicketUser(TicketUser);
                                }
                                _Util.Facade.CustomerAppoinmentFacade.UpdateInstalledByCustomerAppointmentEqp(TicketData.TicketId, AssignedTo);
                                Notification notification = new Notification()
                                {
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    NotificationId = Guid.NewGuid(),
                                    Type = LabelHelper.NotificationType.Employee,
                                    Who = CurrentUser.UserId,
                                    What = string.Format(@"{0} (<a class=""cus-anchor"" href=""/UserInformation/?id={2}"">{2}</a>) created a new Ticket Ticket #{1}", "{0}", TicketData.Id, empuser.UserLoginId),
                                    NotificationUrl = AppConfig.DomainSitePath + "/Ticket/AddTicket/?Id=" + TicketData.TicketId,
                                };
                                _Util.Facade.NotificationFacade.InsertNotification(notification);
                                if (AssignedTo != CurrentUser.UserId)
                                {
                                    #region set user to notification
                                    NotificationUser nu = new NotificationUser()
                                    {
                                        NotificationId = notification.NotificationId,
                                        IsRead = false,
                                        NotificationPerson = AssignedTo,
                                    };
                                    _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                                    #endregion
                                }
                            }
                        }
                    }
                }
                return Json(result);
            }
            catch (Exception)
            {
                return Json(result);
            }
        }
        #endregion
        #region Private Method
        private static void ClearCache(CustomPrincipal currentLoggedIn, GlobalSetting OldGlobalSettingModel)
        {
            if (HttpRuntime.Cache[OldGlobalSettingModel.SearchKey + currentLoggedIn.CompanyId.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(OldGlobalSettingModel.SearchKey + currentLoggedIn.CompanyId.ToString());
            }
            if (OldGlobalSettingModel.SearchKey == "ScheduleCalendarDefaultView" && HttpRuntime.Cache[RMRCacheKey.ScheduleCalendarDefaultView + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ScheduleCalendarDefaultView + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ScheduleCalendarMinTimeRange" && HttpRuntime.Cache[RMRCacheKey.ScheduleCalendarMinTimeRange + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ScheduleCalendarMinTimeRange + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ScheduleCalendarMaxTimeRange" && HttpRuntime.Cache[RMRCacheKey.ScheduleCalendarMaxTimeRange + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ScheduleCalendarMaxTimeRange + currentLoggedIn.CompanyId.Value.ToString());
            }
        }
        #endregion
        public static RootObject getGoogleMapInfo(string address, string GoogleMapAPIKey)
        {
            address = address.ReplaceSpecialChar();
            var root = new RootObject();
            var url = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}", address, GoogleMapAPIKey);
            var req = (HttpWebRequest)WebRequest.Create(url);
            var res = (HttpWebResponse)req.GetResponse();
            using (var streamreader = new StreamReader(res.GetResponseStream()))
            {
                var result = streamreader.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(result))
                {
                    root = JsonConvert.DeserializeObject<RootObject>(result);
                }
            }
            return root;
        }

        #region Map Section
        public ActionResult LoadMap(string TicketDate)
        {
            try
            {
                #region Validation Check
                if (!base.SetLayoutCommons())
                {
                    return RedirectToAction("Logout", "Login");
                }
                if (!base.IsPermitted(UserPermissions.MenuPermissions.LeftMenuCalendar) && !base.IsPermitted(UserPermissions.SchedulePermission.ShowMapBtn))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
                if (currentLoggedIn.UserId == null || currentLoggedIn.UserId == Guid.Empty)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                Employee Emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                if (Emp == null || Emp.Id < 1)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                #endregion  
                ViewBag.GoogleMapAPIKey = _Util.Facade.GlobalSettingsFacade.GetGoogleMapAPIKeyByCompanyId(currentLoggedIn.CompanyId.Value);
                List<CalendarEmployeeDataMapper> ListOfSaveData = _Util.Facade.ScheduleFacade.GetAllActiveCalendarMappingByUserId(currentLoggedIn.UserId);
                ViewBag.currentLoggedIn = currentLoggedIn.UserRole.Trim().ToLower();
                CustomCalendarScheduleCalendarList model = new CustomCalendarScheduleCalendarList();
                bool IsAllAndNoneSelectionPermission = IsPermitted(UserPermissions.SchedulePermission.ShowSelectAllAndNoneTechnicianListInCalendar);
                bool IsViewCalendar = IsPermitted(UserPermissions.SchedulePermission.ShowAllUsersOwnCalendar);
                model = _Util.Facade.ScheduleFacade.GetCustomCalendarIndexPageDataByCompanyId(currentLoggedIn.CompanyId.Value);
                string viewName = "Daily";
                DateTime LocalTodate = DateTime.UtcNow.UTCToClientTime();                              
                if (string.IsNullOrWhiteSpace(TicketDate) && Session[SessionKeys.CalendarSelectedTime] != null)
                {
                    MapSessionModel map = new MapSessionModel();
                    map = (MapSessionModel)Session[SessionKeys.CalendarSelectedTime];
                    TicketDate = map.SelectedDate;
                    map.ViewName = "MapView";
                    Session[SessionKeys.CalendarSelectedTime] = map;
                }
                DateTime.TryParse(TicketDate, out LocalTodate);
                List<string> suser = new List<string>();
                List<string> stype = new List<string>();
                List<Employee> EmpList = new List<Employee>();
                List<SelectListItem> EmployeeList = new List<SelectListItem>();
                List<SelectListItem> TicketTypeList = model.CalendarViewTicketType.OrderBy(x => x.DisplayText).ToList().Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();
                ViewBag.TicketType = TicketTypeList;
                List<CalendarEmployeeDataMapper> SelectedTktTypelist = new List<CalendarEmployeeDataMapper>();
                if (ListOfSaveData != null && ListOfSaveData.Count > 0)
                {
                    SelectedTktTypelist = ListOfSaveData.Where(x => !string.IsNullOrWhiteSpace(x.MapType) && x.EmplyeeSelectedId == Guid.Empty).ToList();
                }
                if (SelectedTktTypelist != null && SelectedTktTypelist.Count > 0)
                {
                    stype.Clear();
                    stype = SelectedTktTypelist.Select(x => x.MapType).ToList();
                }
                //else if (model.CalendarViewTicketType != null && model.CalendarViewTicketType.Count > 0)
                //{
                //    foreach (var type in model.CalendarViewTicketType)
                //    {
                //        if (type.IsDefaultItem)
                //        {
                //            stype.Add(type.DataValue);
                //        }
                //    }
                //}
                if (viewName == "Daily")
                {
                    var datetimeMonth = LocalTodate.ToString("MMMM", CultureInfo.InvariantCulture);
                    var datetimeDay = LocalTodate.ToString("dd", CultureInfo.InvariantCulture);
                    var datetimeYear = LocalTodate.ToString("yyyy", CultureInfo.InvariantCulture);
                    var DayName = LocalTodate.ToString("ddd", CultureInfo.InvariantCulture);
                    ViewBag.DateTitle = DayName + " " + datetimeMonth + " " + datetimeDay + ", " + datetimeYear;
                }

                string todate = LocalTodate.ToString("yyyy-MM-dd");
                if (IsPermitted(UserPermissions.SchedulePermission.ShowUsersHaveEventCalendar))
                {
                    var TechnicianList = _Util.Facade.ScheduleFacade.GetUserListByCompanyIdHaveEvent(currentLoggedIn.CompanyId.Value, todate, viewName);
                    if (TechnicianList != null && TechnicianList.ListUserIdHaveEvent != null && TechnicianList.ListUserIdHaveEvent.Count > 0)
                    {
                        foreach (var item in TechnicianList.ListUserIdHaveEvent)
                        {
                            suser.Add(item.UserId.ToString().ToLower());
                            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item.UserId);
                            if (objemp != null)
                            {
                                EmpList.Add(objemp);
                            }
                        }
                    }
                }
                if (!IsViewCalendar)
                {
                    EmployeeList.Clear();
                    suser.Clear();
                    SelectListItem LoggedInUser = new SelectListItem()
                    {
                        Text = currentLoggedIn.GetFullName(),
                        Value = currentLoggedIn.UserId.ToString(),
                        Selected = true
                    };
                    EmployeeList.Add(LoggedInUser);
                    suser.Add(currentLoggedIn.UserId.ToString());
                }
                else
                {
                    EmployeeList.AddRange(model.CalendarEmployeeList.OrderBy(x => x.ResourceName).Select(x => new SelectListItem()
                    {
                        Text = x.ResourceName.ToString(),
                        Value = x.UserId.ToString().ToLower(),
                        Selected = EmpList.Count(y => y.UserId == Guid.Parse(x.UserId)) > 0
                    }).ToList());
                }
                List<CalendarEmployeeDataMapper> SelectedEmplist = new List<CalendarEmployeeDataMapper>();
                if (ListOfSaveData != null && ListOfSaveData.Count > 0)
                {
                    SelectedEmplist = ListOfSaveData.Where(x => string.IsNullOrWhiteSpace(x.MapType) && x.EmplyeeSelectedId != Guid.Empty).ToList();
                }
                if (SelectedEmplist != null && SelectedEmplist.Count > 0)
                {
                    suser.Clear();
                    suser = SelectedEmplist.Select(x => x.EmplyeeSelectedId.ToString()).ToList();
                }

                #region View Bag section
                var settings = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("EnableSelectedDateReload", currentLoggedIn.CompanyId.Value);
                if (settings != null && !string.IsNullOrWhiteSpace(settings.Value))
                {
                    ViewBag.SelectedDateReload = settings.Value;
                }
                ViewBag.TicketStatus = model.CalendarViewTicketType.ToList();
                ViewBag.StatusTicket = model.CalendarTicketStatus.ToList();
                ViewBag.Currentday = todate;
                ViewBag.UserValValueList = suser;
                ViewBag.ListEmployee = EmployeeList;
                ViewBag.typevalList = stype;
                #endregion
                return View();
            }
            catch (Exception)
            {
                return View("~/Views/Shared/_NotFound.cshtml");
            }
        }
        [HttpPost]
        [Authorize]
        public JsonResult AllLoadMapCalendar(List<string> Default, string startdate, List<string> typeval)
        {

            List<PreCustomerNote> addressList = new List<PreCustomerNote>();
            PreCustomerNote companyInfo = new PreCustomerNote();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            string GoogleMapAPIKey = _Util.Facade.GlobalSettingsFacade.GetGoogleMapAPIKeyByCompanyId(currentLoggedIn.CompanyId.Value);

            string loadDomainUrl = AppConfig.DomainSitePath;
            try
            {
                
                string struser = "";
                string strtype = "";
                List<string> defaultval = new List<string>();
                GlobalSetting ticketcolor = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("CalendarTicketColor");
                string SelectedColor = "";
                if (ticketcolor != null && !string.IsNullOrWhiteSpace(ticketcolor.Value))
                {
                    SelectedColor = ticketcolor.Value;
                }
                if (currentLoggedIn.UserId == null || currentLoggedIn.UserId == Guid.Empty)
                {
                    return Json(new { Result = false, Model = addressList });
                }
                Company _company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(currentLoggedIn.CompanyId.Value);
                if (_company == null)
                {
                    return Json(new { Result = false, Model = addressList });
                }
                RootObject commap = new RootObject();
                string CompanyAddress = "";
                companyInfo.HoverTitle = _company.CompanyName;
                companyInfo.EventStreet = _company.Street;
                companyInfo.EventLocate = _company.City + "," + _company.State + " " + _company.ZipCode;
                companyInfo.EventColor = "FF0000";
                companyInfo.EventType = "Company";
                if (!string.IsNullOrWhiteSpace(companyInfo.EventStreet) && !string.IsNullOrWhiteSpace(companyInfo.EventLocate))
                {
                    CompanyAddress = companyInfo.EventStreet + " " + companyInfo.EventLocate;
                    commap = getGoogleMapInfo(CompanyAddress, GoogleMapAPIKey);
                }
                else if (!string.IsNullOrWhiteSpace(companyInfo.EventLocate))
                {
                    CompanyAddress = companyInfo.EventLocate;
                    commap =  getGoogleMapInfo(CompanyAddress, GoogleMapAPIKey);
                }
                companyInfo.EventResourceName = "<b>" + _company.CompanyName + "</b><br><span><a href='javascript:void(0)' class='cus-anchor' onclick='GetDirection(event)' data-address = '" + CompanyAddress + "' id = 'getDirection'  >" + companyInfo.EventLocate + "</a></span>";

                if (commap.status.ToLower() == "ok" && commap.results != null && commap.results.Count > 0)
                {
                    var Latitude = commap.results[0].geometry.location.lat.ToString();
                    var Longitude = commap.results[0].geometry.location.lng.ToString();
                    companyInfo.EventLatitude = Latitude;
                    companyInfo.EventLongitude = Longitude;
                }

                string todate = DateTime.UtcNow.UTCToClientTime().ToString("yyyy/MM/dd");
                if (!string.IsNullOrWhiteSpace(startdate))
                {
                    todate = startdate;
                }
                if (Default != null && Default.Count > 0)
                {
                    foreach (var item in Default)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && item != "null" && item != "all" && item != "undefined")
                        {
                            defaultval.Add(item);
                        }
                    }
                    struser = string.Format("'{0}'", string.Join("','", defaultval.Select(i => i.Replace("'", "''"))));
                }
                List<string> defaulttype = new List<string>();
                if (typeval != null && typeval.Count > 0)
                {
                    foreach (var item in typeval)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && item != "null" && item != "undefined")
                        {
                            defaulttype.Add(item);
                        }
                    }
                    strtype = string.Format("'{0}'", string.Join("','", defaulttype.Select(i => i.Replace("'", "''"))));
                }                
                addressList = _Util.Facade.ScheduleFacade.GetSchedulingByCompanyIdAndFilterForGoogleMap(currentLoggedIn.CompanyId.Value, todate, strtype, struser, false, false);
                var ZoomLevel = _Util.Facade.GlobalSettingsFacade.GetMapZoomLevel(currentLoggedIn.CompanyId.Value);
                if (addressList != null && addressList.Count > 0)
                {
                    int i = 0;
                    DateTime time = new DateTime();                    
                    foreach (var data in addressList)
                    {
                        DateTime SDate = new DateTime();
                        DateTime.TryParse(data.EventStartDate, out SDate);
                        string AddressInfo = "";
                        string EditUser = "";
                        if (SelectedColor == "Employee")
                        {
                            data.EventColor = !string.IsNullOrWhiteSpace(data.EventEmployeeColor) ? data.EventEmployeeColor : "ccc";
                        }
                        else if (SelectedColor == "TicketStatus")
                        {

                            data.EventColor = !string.IsNullOrWhiteSpace(data.EventStatusColor) ? data.EventStatusColor.Substring(1) : "ccc";
                        }
                        else
                        {
                            data.EventColor = !string.IsNullOrWhiteSpace(data.EventColor) ? data.EventColor : "ccc";
                        }
                        if (!string.IsNullOrWhiteSpace(data.EventStreet) && !string.IsNullOrWhiteSpace(data.EventLocate))
                        {
                            AddressInfo = "<br><span><a href='javascript:void(0)' class='cus-anchor' onclick='GetDirection(event)' data-address = '" + data.EventStreet + ' ' + data.EventLocate + "' id = 'getDirection'  >" + data.EventStreet + "<br>" + data.EventLocate + "</a></span>";
                        }
                        else if (!string.IsNullOrWhiteSpace(data.EventLocate))
                        {
                            AddressInfo = "<br><span><a href='javascript:void(0)' class='cus-anchor' onclick='GetDirection(event)' data-address = '" + data.EventLocate + "' id = 'getDirection'  >" + data.EventLocate + "</a></span>";
                        }
                        if (IsPermitted(UserPermissions.SchedulePermission.MapPointerEditPermission))
                        {
                            EditUser = "<span class='mar-left-5'><a id='EventEdit_" + data.EventLeadId + "' href='javascript: void(0)' onclick='TicketEdit(" + data.EventLeadId + ");'  title='Edit'><i class='fa fa-pencil-square-o' aria-hidden='true'></i></a><a id='EventUpdate_" + data.EventLeadId + "' style='display: none;' href='javascript: void(0)' onclick='TicketSave(" + data.EventLeadId + ");'  title='Update'><i class='fa fa-floppy-o' aria-hidden='true'></i></a></span><br><span data-userid='" + data.EventCustomId + "' id= 'span_" + data.EventLeadId + "'>" + data.EventTitle + "</span>";
                        }
                        else
                        {
                            EditUser = "<br><span>" + data.EventTitle + "</span>";
                        }
                        if (data.EventAllDay)
                        {
                            if (!string.IsNullOrWhiteSpace(data.EventBookingId))
                            {
                                int BkId = 0;
                                int.TryParse(data.EventBookingId.Remove(0, 3), out BkId);
                                data.HoverTitle = "Ticket# " + data.EventLeadId + "\n" + data.EventTitle + "\n" + data.EventType + "\n" + data.EventStatus + "\n" + SDate.ToString("M/d/yy ") + "- FullDay" + "\n" + data.EventBookingId + "\n" + data.EventCustomerName + "\n" + data.EventStreet + "\n" + data.EventLocate;
                                data.EventResourceName = "<span>Ticket# <a class='cus-anchor' onclick='OpenTicketById(" + data.EventLeadId + ")' href='javascript: void(0)'>" + data.EventLeadId + "</a></span>" + EditUser + "<br><span>" + data.EventType + "</span><br><span>" + data.EventStatus + "</span><br><span>" + SDate.ToString("M/d/yy ") + " - FullDay</span><br><span><a class='cus-anchor' onclick='OpenBkById(" + BkId + "," + data.EventCustomerIntId + ")'>" + data.EventBookingId + "</a></span><br><span><a  style='color: #2ca01c;' href='" + loadDomainUrl + "/Customer/CustomerDetail/?id=" + data.EventCustomerIntId + "' target='_blank'  title='Customer'>" + data.EventCustomerName + "</a></span>" + AddressInfo;
                            }
                            else
                            {
                                data.HoverTitle = "Ticket# " + data.EventLeadId + "\n" + data.EventTitle + "\n" + data.EventType + "\n" + data.EventStatus + "\n" + SDate.ToString("M/d/yy ") + "- FullDay" + "\n" + data.EventCustomerName + "\n" + data.EventStreet + "\n" + data.EventLocate;
                                data.EventResourceName = "<span>Ticket#  <a class='cus-anchor' onclick='OpenTicketById(" + data.EventLeadId + ")' href='javascript: void(0)'>" + data.EventLeadId + "</a></span>" + EditUser + "<br><span>" + data.EventType + "</span><br><span>" + data.EventStatus + "</span><br><span>" + SDate.ToString("M/d/yy ") + " - FullDay</span><br><span><a  style='color: #2ca01c;' href='" + loadDomainUrl + "/Customer/CustomerDetail/?id=" + data.EventCustomerIntId + "' target='_blank'  title='Customer'>" + data.EventCustomerName + "</a></span>" + AddressInfo;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(data.EventBookingId))
                            {
                                int BkId = 0;
                                int.TryParse(data.EventBookingId.Remove(0, 3), out BkId);
                                data.HoverTitle = "Ticket# " + data.EventLeadId + "\n" + data.EventTitle + "\n" + data.EventType + "\n" + data.EventStatus + "\n" + SDate.ToString("M/d/yy ") + SDate.ToString("h:mm tt") + "-" + (DateTime.Parse(data.EventEndDate)).ToString("h:mm tt") + "\n" + data.EventBookingId + "\n" + data.EventCustomerName + "\n" + data.EventStreet + "\n" + data.EventLocate;
                                data.EventResourceName = "<span>Ticket#  <a class='cus-anchor' onclick='OpenTicketById(" + data.EventLeadId + ")' href='javascript: void(0)'>" + data.EventLeadId + "</a></span>" + EditUser + "<br><span>" + data.EventType + "</span><br><span>" + data.EventStatus + "</span><br><span>" + SDate.ToString("M/d/yy ") + SDate.ToString("h:mm tt") + "-" + (DateTime.Parse(data.EventEndDate)).ToString("h:mm tt") + "</span><br><span><a class='cus-anchor' onclick='OpenBkById(" + BkId + "," + data.EventCustomerIntId + ")'>" + data.EventBookingId + "</a></span><br><span><a  style='color: #2ca01c;' href='" + loadDomainUrl + "/Customer/CustomerDetail/?id=" + data.EventCustomerIntId + "' target='_blank'  title='Customer'>" + data.EventCustomerName + "</a></span>" + AddressInfo;
                            }
                            else
                            {
                                data.HoverTitle = "Ticket# " + data.EventLeadId + "\n" + data.EventTitle + "\n" + data.EventType + "\n" + data.EventStatus + "\n" + SDate.ToString("M/d/yy ") + SDate.ToString("h:mm tt") + "-" + (DateTime.Parse(data.EventEndDate)).ToString("h:mm tt") + "\n" + data.EventCustomerName + "\n" + data.EventStreet + "\n" + data.EventLocate;
                                data.EventResourceName = "<span>Ticket#  <a class='cus-anchor' onclick='OpenTicketById(" + data.EventLeadId + ")' href='javascript: void(0)'>" + data.EventLeadId + "</a></span>" + EditUser + "<br><span>" + data.EventType + "</span><br><span>" + data.EventStatus + "</span><br><span>" + SDate.ToString("M/d/yy ") + SDate.ToString("h:mm tt") + "-" + (DateTime.Parse(data.EventEndDate)).ToString("h:mm tt") + "</span><br><span><a  style='color: #2ca01c;' href='" + loadDomainUrl + "/Customer/CustomerDetail/?id=" + data.EventCustomerIntId + "' target='_blank'  title='Customer'>" + data.EventCustomerName + "</a></span>" + AddressInfo;
                            }
                        }

                        if (data.EventAllDay)
                        {
                            data.EventCalendarCount = "A";                                                       
                            time = SDate;

                        }
                        else if (time != SDate)
                        {
                            i++;
                            data.EventCalendarCount = i.ToString();
                            time = SDate;
                        }
                        else
                        {
                            data.EventCalendarCount = i.ToString();
                        }

                        if (!string.IsNullOrWhiteSpace(data.EventLocationFlag))
                        {
                            data.EventLatitude = data.EventLocationFlag.Split(',')[0];
                            data.EventLongitude = data.EventLocationFlag.Split(',')[1];
                        }
                        else
                        {
                            var address = "";
                            if (!string.IsNullOrWhiteSpace(data.EventStreet)) { address += data.EventStreet + " "; }
                            if (!string.IsNullOrWhiteSpace(data.EventLocate)) { address += data.EventLocate; }
                            var map = getGoogleMapInfo(address, GoogleMapAPIKey);
                            if (map.status.ToLower() == "ok" && map.results != null && map.results.Count > 0)
                            {
                                var Latitude = map.results[0].geometry.location.lat.ToString();
                                var Longitude = map.results[0].geometry.location.lng.ToString();
                                data.EventLatitude = Latitude;
                                data.EventLongitude = Longitude;
                                #region Update customer by latitude and longitude value
                                Customer _customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Guid.Parse(data.EventCusId));
                                _customer.Latlng = Latitude + ',' + Longitude;
                                _Util.Facade.CustomerFacade.UpdateCustomer(_customer);
                                #endregion
                            }
                        }
                    }                    
                }
                addressList.Add(companyInfo);
                return Json(new { Result = true, Model = addressList, ZoomVal = ZoomLevel });
            }
            catch (Exception)
            {
                return Json(new { Result = false, Model = addressList });
            }
        }
        #endregion


        private bool UpdateCalendarMappingData(Guid UserId, string Type, bool Isactive, string EmpId)
        {
            bool result = false;
            Guid GuidId = Guid.Empty;
            if(UserId != Guid.Empty && !string.IsNullOrWhiteSpace(EmpId) && Guid.TryParse(EmpId, out GuidId) && GuidId  != Guid.Empty)
            {
                CalendarEmployeeDataMapper Emp = _Util.Facade.ScheduleFacade.GetCalendarMappingByUserIdandEmpId(UserId, GuidId);
                if (Emp != null && Emp.Id > 0)
                {
                    Emp.IsActive = Isactive;
                    result = _Util.Facade.ScheduleFacade.UpdateCalendarMapping(Emp);      
                }
                else
                {
                    CalendarEmployeeDataMapper model = new CalendarEmployeeDataMapper();
                    model.UserId = UserId;
                    model.MapType = "";
                    model.IsActive = Isactive;
                    model.CreatedDate = DateTime.UtcNow;
                    model.EmplyeeSelectedId = GuidId;
                    result = _Util.Facade.ScheduleFacade.InsertCalendarMapping(model);
                }
            }
          else if (UserId != Guid.Empty && !string.IsNullOrWhiteSpace(Type))
            {
                CalendarEmployeeDataMapper Emp = _Util.Facade.ScheduleFacade.GetCalendarMappingByUserIdandType(UserId, Type);
                if (Emp != null && Emp.Id > 0)
                {
                    Emp.IsActive = Isactive;
                    result = _Util.Facade.ScheduleFacade.UpdateCalendarMapping(Emp);
                }
                else
                {
                    CalendarEmployeeDataMapper model = new CalendarEmployeeDataMapper();
                    model.UserId = UserId;
                    model.MapType = Type;
                    model.IsActive = Isactive;
                    model.CreatedDate = DateTime.UtcNow;
                    model.EmplyeeSelectedId = Guid.Empty;
                    result = _Util.Facade.ScheduleFacade.InsertCalendarMapping(model);
                }
            }
            return result;
        }
        private bool DeactiveListOfCalendarMappingData(Guid UserId, string Type)
        {
            bool result = false;            
            if(!string.IsNullOrWhiteSpace(Type) && UserId != Guid.Empty)
            {
                List<CalendarEmployeeDataMapper> list = _Util.Facade.ScheduleFacade.GetAllCalendarMappingByUserId(UserId);
                if(Type == "Employee" && list != null && list.Count > 0)
                {
                    List<CalendarEmployeeDataMapper> Emplist = list.Where(x => string.IsNullOrWhiteSpace(x.MapType) && x.EmplyeeSelectedId != Guid.Empty).ToList();
                    if(Emplist != null && Emplist.Count > 0)
                    {
                        foreach (var item in Emplist)
                        {
                            item.IsActive = false;
                            result = _Util.Facade.ScheduleFacade.UpdateCalendarMapping(item);
                        }
                    }
                }
              else  if (Type == "TicketType" && list != null && list.Count > 0)
                {
                    List<CalendarEmployeeDataMapper> Emplist = list.Where(x => !string.IsNullOrWhiteSpace(x.MapType) && x.EmplyeeSelectedId == Guid.Empty).ToList();
                    if (Emplist != null && Emplist.Count > 0)
                    {
                        foreach (var item in Emplist)
                        {
                            item.IsActive = false;
                            result = _Util.Facade.ScheduleFacade.UpdateCalendarMapping(item);
                        }
                    }
                }
            }
            return result;
        }
    }
}