using HS.Entities;
using HS.Framework;
using HS.Web.UI.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class ScheduleController : BaseController
    {
        // GET: Schedule
        public ActionResult Index(int? id)
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            if (id.HasValue)
            {
                ViewBag.id = id.Value;
            }
            else
            {
                ViewBag.id = 0;
            }
            return View();
        }
        public ActionResult ScheduleUserListPartial(string CurrentDate, string CurrentView, string UserVal, string typeval, string status)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<Employee> EmpList = new List<Employee>();
            List<string> suser = new List<string>();
            List<string> stype = new List<string>();
            bool IsAllSelected = false, IsNoneselected = false, IsClicked = false;
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
            bool IsAllAndNoneSelectionPermission = IsPermitted(UserPermissions.SchedulePermission.ShowSelectAllAndNoneTechnicianListInCalendar);
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

            List<SelectListItem> EmployeeList = new List<SelectListItem>();
            if (!IsPermitted(UserPermissions.SchedulePermission.ShowAllUsersOwnCalendar))
            {
                EmployeeList.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeListCalendarSchedule(currentLoggedIn.UserId).OrderBy(x => x.ResourceName).Select(x => new SelectListItem()
                {
                    Text = x.ResourceName.ToString(),
                    Value = x.UserId.ToString().ToLower(),
                    Selected = EmpList.Count(y => y.UserId == x.UserId) > 0
                }).ToList());
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
            //EmployeeList.AddRange(model.CalendarEmployeeList.OrderBy(x => x.ResourceName).Select(x => new SelectListItem()
            //{
            //    Text = x.ResourceName.ToString(),
            //    Value = x.UserId.ToString().ToLower(),
            //    Selected = EmpList.Count(y => y.UserId == Guid.Parse(x.UserId)) > 0
            //}).ToList());
            if (!IsPermitted(UserPermissions.SchedulePermission.ShowAllUsersOwnCalendar))
            {
                suser.Clear();
                suser.Add(currentLoggedIn.UserId.ToString());
            }
            ViewBag.strUserListVal = suser;
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
            ViewBag.strTypeListval = stype;
            ViewBag.TicketType = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Where(x => x.DataValue != "-1" && x.AlterDisplayText1 != null && x.AlterDisplayText1 != "" && x.AlterDisplayText1 == "True").OrderBy(x => x.DisplayText).ToList().Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            return PartialView("ScheduleUserListPartial");
        }
        [Authorize]
        public ActionResult ScheduleCalendar(string parent, string UserVal, string pageno, string ReminderResource, string pageno2, string viewname, string viewstartdate, string typeval, string EventUserId, string TicketId, string FilterWithSearch, string SelectedEmpOnly)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            int pageno1 = 1;
            int pageno3 = 0;
            string ValUser = "";
            string eventtype = "";
            List<string> suser = new List<string>();
            List<string> stype = new List<string>();
            List<Employee> EmpList = new List<Employee>();
            bool IsClicked = false;
            if (SelectedEmpOnly == "clickeduser")
            {
                IsClicked = true;
            }
            if (!string.IsNullOrWhiteSpace(UserVal) && UserVal != "null" && UserVal != "none" && UserVal != "all" && UserVal != "undefined")
            {
                string[] splituser = UserVal.Split(',');                
                if(splituser.Length > 0)
                {
                    ValUser = string.Format("'{0}'", string.Join("','", splituser.Select(i => i.Replace("'", "''"))));
                    foreach(var item in splituser)
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
            if (!string.IsNullOrWhiteSpace(typeval))
            {
                typeval = System.Web.HttpUtility.UrlDecode(typeval);
                string[] splituser = typeval.Split(',');
                if (splituser.Length > 0)
                {
                    eventtype = string.Format("'{0}'", string.Join("','", splituser));
                    foreach (var item in splituser)
                    {
                        stype.Add(item);
                    }
                }
            }
            ScheduleCalendarList model = new ScheduleCalendarList();
            string empid = "00000000-0000-0000-0000-000000000000";
            if (!string.IsNullOrWhiteSpace(parent))
            {
                parent = "." + parent;
                ViewBag.Parent = parent;
            }
            if (!string.IsNullOrWhiteSpace(viewname))
            {
                ViewBag.defaultView = viewname;
            }
            else
            {
                ViewBag.defaultView = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarDefaultView(currentLoggedIn.CompanyId.Value);
            }
            if (!string.IsNullOrWhiteSpace(viewstartdate))
            {
                var splitdate = viewstartdate.Split('T');
                var datetime = Convert.ToDateTime(splitdate[0]).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                ViewBag.StartDate = datetime;
            }
            
            List<SelectListItem> TicketTypeList = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Where(x => x.DataValue != "-1" && x.AlterDisplayText1 != null && x.AlterDisplayText1 != "" && x.AlterDisplayText1 == "True").OrderBy(x =>x.DisplayText).ToList().Select(x =>
           new SelectListItem()
           {
               Text = x.DisplayText.ToString(),
               Value = x.DataValue.ToString()
           }).ToList();
            ViewBag.TicketType = TicketTypeList;
            
            
            ViewBag.typeval = stype;
            var ResourceLim = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarResourceLimit(currentLoggedIn.CompanyId.Value);
            if (!string.IsNullOrWhiteSpace(pageno) && Convert.ToInt32(pageno) > 0)
            {
                ViewBag.pageno = Convert.ToInt32(pageno);
            }
            else
            {
                ViewBag.pageno = pageno1;
            }
            if (!string.IsNullOrWhiteSpace(pageno2))
            {
                pageno3 = Convert.ToInt32(pageno2);
            }
            ViewBag.ResourceLimit = ResourceLim;
            bool ispermit = false;
            if (IsPermitted(UserPermissions.CustomerTicketPermission.ShowLostTicket))
            {
                ispermit = true;
            }
            ViewBag.ScheduleCalendarMinTimeRange = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarMinTimeRange(currentLoggedIn.CompanyId.Value);
            ViewBag.ScheduleCalendarMaxTimeRange = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarMaxTimeRange(currentLoggedIn.CompanyId.Value);
            ViewBag.EventUserId = EventUserId;
            ViewBag.TicketId = TicketId;
            List<TicketStatusImageModel> ListImage = new List<TicketStatusImageModel>();
            var objStatusImage = _Util.Facade.TicketFacade.GetAllStatusImageSettingByCompanyId(currentLoggedIn.CompanyId.Value);
            if(objStatusImage != null && objStatusImage.Count > 0)
            {
                foreach(var item in objStatusImage)
                {
                    ListImage.Add(new TicketStatusImageModel()
                    {
                        status = item.TicketStatus,
                        image = item.Filename
                    });
                }
            }
            ViewBag.ListImage = ListImage;
            if((currentLoggedIn.UserRole.ToLower().IndexOf("technician") > -1 || currentLoggedIn.UserRole.ToLower().IndexOf("installation") > -1) && currentLoggedIn.UserId != new Guid())
            {
                ViewBag.TechUserId = currentLoggedIn.UserId;
                ViewBag.CalendarUserTag = "Tech";
            }
            else
            {
                ViewBag.CalendarUserTag = "Admin";
            }
            if(!string.IsNullOrWhiteSpace(viewname) && viewname == "Daily")
            {
                if (!string.IsNullOrWhiteSpace(viewstartdate))
                {
                    var splitdate = viewstartdate.Split('T');
                    var datetimeMonth = Convert.ToDateTime(splitdate[0]).ToString("MMMM", CultureInfo.InvariantCulture);
                    var datetimeDay = Convert.ToDateTime(splitdate[0]).ToString("dd", CultureInfo.InvariantCulture);
                    var datetimeYear = Convert.ToDateTime(splitdate[0]).ToString("yyyy", CultureInfo.InvariantCulture);
                    ViewBag.DateTitle = datetimeMonth + " " + datetimeDay + ", " + datetimeYear;
                }
                else
                {
                    var datetimeMonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
                    var datetimeDay = DateTime.Now.ToString("dd", CultureInfo.InvariantCulture);
                    var datetimeYear = DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture);
                    ViewBag.DateTitle = datetimeMonth + " " + datetimeDay + ", " + datetimeYear;
                }
            }
            else if (!string.IsNullOrWhiteSpace(viewname) && (viewname == "Weekly" || viewname == "List"))
            {
                if (!string.IsNullOrWhiteSpace(viewstartdate))
                {
                    var splitdate = viewstartdate.Split('T');
                    var datetimesMonth = Convert.ToDateTime(splitdate[0]).ToString("MMM", CultureInfo.InvariantCulture);
                    var datetimesDay = Convert.ToDateTime(splitdate[0]).ToString("dd", CultureInfo.InvariantCulture);
                    var datetimeYear = Convert.ToDateTime(splitdate[0]).ToString("yyyy", CultureInfo.InvariantCulture);
                    DateTime LastDate = Convert.ToDateTime(splitdate[0]).AddDays(6);
                    var datetimelMonth = LastDate.ToString("MMM", CultureInfo.InvariantCulture);
                    var datetimelDay = LastDate.ToString("dd", CultureInfo.InvariantCulture);
                    ViewBag.DateTitle = datetimesMonth + " " + datetimesDay + " - " + datetimelMonth + " " + datetimelDay + ", " + datetimeYear;
                }
                else
                {
                    var datetimesMonth = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
                    var datetimesDay = DateTime.Now.ToString("dd", CultureInfo.InvariantCulture);
                    var datetimeYear = DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture);
                    DateTime LastDate = DateTime.Now.AddDays(6);
                    var datetimelMonth = LastDate.ToString("MMM", CultureInfo.InvariantCulture);
                    var datetimelDay = LastDate.ToString("dd", CultureInfo.InvariantCulture);
                    ViewBag.DateTitle = datetimesMonth + " " + datetimesDay + " - " + datetimelMonth + " " + datetimelDay + ", " + datetimeYear;
                }
            }
            else if (!string.IsNullOrWhiteSpace(viewname) && viewname == "Monthly")
            {
                if (!string.IsNullOrWhiteSpace(viewstartdate))
                {
                    var splitdate = viewstartdate.Split('T');
                    var spdate = Convert.ToDateTime(splitdate[0]);
                    ViewBag.DateTitle = spdate.ToString("MMMM") + " " + spdate.Year;
                }
                else
                {
                    ViewBag.DateTitle = DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year;
                }
            }
            else
            {
                var objglobal = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarDefaultView(currentLoggedIn.CompanyId.Value);
                if(objglobal == "Daily")
                {
                    var datetimeMonth = DateTime.Now.ToString("MMMM", CultureInfo.InvariantCulture);
                    var datetimeDay = DateTime.Now.ToString("dd", CultureInfo.InvariantCulture);
                    var datetimeYear = DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture);
                    ViewBag.DateTitle = datetimeMonth + " " + datetimeDay + ", " + datetimeYear;
                }
                else if(objglobal == "Weekly" || objglobal == "List")
                {
                    var datetimesMonth = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
                    var datetimesDay = DateTime.Now.ToString("dd", CultureInfo.InvariantCulture);
                    var datetimeYear = DateTime.Now.ToString("yyyy", CultureInfo.InvariantCulture);
                    DateTime LastDate = DateTime.Now.AddDays(6);
                    var datetimelMonth = LastDate.ToString("MMM", CultureInfo.InvariantCulture);
                    var datetimelDay = LastDate.ToString("dd", CultureInfo.InvariantCulture);
                    ViewBag.DateTitle = datetimesMonth + " " + datetimesDay + " - " + datetimelMonth + " " + datetimelDay + ", " + datetimeYear;
                }
                else if(objglobal == "Monthly")
                {
                    ViewBag.DateTitle = DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Year;
                }
            }
            if(!IsClicked && IsPermitted(UserPermissions.SchedulePermission.ShowUsersHaveEventCalendar))
            {
                if(string.IsNullOrWhiteSpace(FilterWithSearch) || FilterWithSearch=="")
                {
                    EmpList.Clear();
                    suser.Clear();
                }
                model = _Util.Facade.ScheduleFacade.GetUserListByCompanyIdHaveEvent(currentLoggedIn.CompanyId.Value, ViewBag.StartDate, ViewBag.defaultView);
                if (model != null && model.ListUserIdHaveEvent !=null && model.ListUserIdHaveEvent.Count>0)
                {
                    foreach (var item in model.ListUserIdHaveEvent)
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
            List<SelectListItem> EmployeeList = new List<SelectListItem>();
            if (!IsPermitted(UserPermissions.SchedulePermission.ShowAllUsersOwnCalendar))
            {
                EmployeeList.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeListCalendarSchedule(currentLoggedIn.UserId).OrderBy(x => x.ResourceName).Select(x => new SelectListItem()
                {
                    Text = x.ResourceName.ToString(),
                    Value = x.UserId.ToString().ToLower(),
                    Selected = EmpList.Count(y => y.UserId == x.UserId) > 0
                }).ToList());
            }
            else
            {
                EmployeeList.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeListCalendarSchedule(new Guid()).OrderBy(x => x.ResourceName).Select(x => new SelectListItem()
                {
                    Text = x.ResourceName.ToString(),
                    Value = x.UserId.ToString().ToLower(),
                    Selected = EmpList.Count(y => y.UserId == x.UserId) > 0
                }).ToList());
            }
            if (!IsPermitted(UserPermissions.SchedulePermission.ShowAllUsersOwnCalendar))
            {
                suser = new List<string>();
                suser.Add(currentLoggedIn.UserId.ToString());
                ValUser = string.Format("'{0}'", string.Join("','", currentLoggedIn.UserId.ToString().Replace("'", "''")));
            }
            ViewBag.UserValValue = suser;
            //int UserCount = 0;
            //foreach (var user in suser)
            //{
            //    if (!string.IsNullOrWhiteSpace(user) && user != "null")
            //    {
            //        var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeIdAndIsCalendar(new Guid(user));
            //        if (empobj != null)
            //        {
            //            UserCount = UserCount + 1;
            //        }
            //    }
            //}
            ViewBag.ListEmployee = EmployeeList;
            ViewBag.ispermit = IsPermitted(UserPermissions.SchedulePermission.ScheduleCalendarPermission);
            ViewBag.isSelectAllpermit = IsPermitted(UserPermissions.SchedulePermission.ShowSelectAllAndNoneTechnicianListInCalendar); ;
            return PartialView("ScheduleCalendar");
        }
        [HttpPost]
        [Authorize]
        public JsonResult AllScheduleCalendar(List<string> Default, string ResourceLim, string pageno, string ReminderResource, string pageno1, string startdate, string defaultView, List<string> typeval, List<string> UserVal, string EventUserId, string TicketId, string NoneEmp)
        {
            ScheduleCalendarList model = new ScheduleCalendarList();
            bool ReminderResult = false;
            int ResourceLimit = 0;
            int page3 = 0;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            string EmployeeTag = "";
            string empid = "00000000-0000-0000-0000-000000000000";
            string ValUser = "";
            string eventtype = "";
            bool IsNotNone = true;
            if(NoneEmp.ToLower() == "none")
            {
                IsNotNone = false;
            }
            List<string> defaultval = new List<string>();
            List<string> CompareTicketId = new List<string>();
            if (IsNotNone)
            {
                if (Default != null && Default.Count > 0)
                {
                    foreach (var item in Default)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && item != "null")
                        {
                            defaultval.Add(item);
                        }
                        else
                        {
                            defaultval.Add(empid);
                        }
                    }
                    ValUser = string.Format("'{0}'", string.Join("','", defaultval.Select(i => i.Replace("'", "''"))));
                }
                if (typeval != null)
                {
                    eventtype = string.Format("'{0}'", string.Join("','", typeval));
                }

                if (currentLoggedIn.CompanyId.Value != new Guid())
                {
                    if (!string.IsNullOrWhiteSpace(ValUser))
                    {
                        EmployeeTag = "Others";
                        empid = ValUser;
                    }
                    //else
                    //{
                    //    EmployeeTag = currentLoggedIn.UserTags;
                    //    empid = string.Format("'{0}'", currentLoggedIn.UserId.ToString()); 
                    //}
                }
                if (!string.IsNullOrWhiteSpace(ResourceLim))
                {
                    ResourceLimit = Convert.ToInt32(ResourceLim);
                }
                if (!string.IsNullOrWhiteSpace(ReminderResource) && ReminderResource == "true")
                {
                    ReminderResult = true;
                }
                if (!string.IsNullOrWhiteSpace(pageno1))
                {
                    page3 = Convert.ToInt32(pageno1);
                }
                bool ispermit = false;
                if (IsPermitted(UserPermissions.CustomerTicketPermission.ShowLostTicket))
                {
                    ispermit = true;
                }
                model = _Util.Facade.ScheduleFacade.GetScheduleListByCompanyId(currentLoggedIn.CompanyId.Value, EmployeeTag, empid, ResourceLimit, Convert.ToInt32(pageno), ReminderResult, page3, startdate, defaultView, eventtype, ValUser, EventUserId, TicketId, ispermit);
                int dropoffday = 0;
                int serviceday = 0;
                int compareday = 0;                
                string compareTicketString = "";
                if (model != null && model.ListFollowUpSchedule != null && model.ListFollowUpSchedule.Count > 0)
                {
                    foreach (var item in model.ListFollowUpSchedule)
                    {
                        if (!string.IsNullOrWhiteSpace(item.EventBookingId))
                        {
                            var objappointment = _Util.Facade.TicketFacade.GetTicketListByCustomerIdAndBookingIdAndTicketType(new Guid(item.EventCusId), item.EventBookingId);
                            if (objappointment != null && objappointment.Count > 0)
                            {
                                foreach (var type in objappointment)
                                {
                                    if (type.TicketType == "Drop Off")
                                    {
                                        var objticketapp = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentObjectByAppointmentId(type.TicketId);
                                        if (objticketapp != null)
                                        {
                                            dropoffday = objticketapp.AppointmentDate.HasValue ? objticketapp.AppointmentDate.Value.Day : 0;
                                        }
                                    }
                                    if (type.TicketType == "Service")
                                    {
                                        var objticketapp = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentObjectByAppointmentId(type.TicketId);
                                        if (objticketapp != null)
                                        {
                                            serviceday = objticketapp.AppointmentDate.HasValue ? objticketapp.AppointmentDate.Value.Day : 0;
                                        }
                                    }
                                }
                                compareday = dropoffday - serviceday;
                                if (compareday <= 1)
                                {
                                    var objticket = _Util.Facade.TicketFacade.GetTicketListByCustomerIdAndBookingIdAndTicketTypeOnly(new Guid(item.EventCusId), item.EventBookingId);
                                    if (objticket != null)
                                    {
                                        if (!string.IsNullOrWhiteSpace(compareTicketString))
                                        {
                                            if (compareTicketString.IndexOf(objticket.TicketId.ToString()) > -1)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                CompareTicketId.Add(objticket.TicketId.ToString());
                                                compareTicketString = string.Join(",", CompareTicketId);
                                            }
                                        }
                                        else
                                        {
                                            CompareTicketId.Add(objticket.TicketId.ToString());
                                            compareTicketString = string.Join(",", CompareTicketId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Json(new { model = model.ListTechnicianSchedule, model1= model.ListFollowUpSchedule, Default = Default, startdate = startdate, modelEmp = model.ListEmployeeCalendar, View = defaultView, typeval = typeval, datacount = model.EventFilterCount, CompareTicketId = CompareTicketId });
        }

        public ActionResult SchedulePartial(string date, string viewtype,int? TicketId,Guid? CustomerId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ViewBag.date = date;
            ViewBag.viewtype = viewtype;
            if (!TicketId.HasValue)
            {
                TicketId = 0;
            }
            if (!CustomerId.HasValue)
            {
                CustomerId = new Guid();
            }
            ViewBag.TicketId = TicketId;
            ViewBag.TicketCustomerId = CustomerId;
            ViewBag.TicketStatus = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Where(x => x.AlterDisplayText1 != null && x.AlterDisplayText1 != "" && x.AlterDisplayText1 == "True").ToList();
            ViewBag.StatusTicket = _Util.Facade.TicketFacade.GetAllStatusImageSettingByCompanyId(currentLoggedIn.CompanyId.Value);
            return PartialView("SchedulePartial");
        }

        [HttpPost]
        public ActionResult ScheduleUserListPartial()
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
            List<SelectListItem>  TicketTypeList = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Select(x => 
            new SelectListItem ()
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

        [Authorize]
        [HttpPost]
        public JsonResult DraggingScheduleCalendar(string eventType, int? eventId, string eventDate, string eventAppid, bool eventAllDay, string eventEndDate, string Eventresid, string dragresid, string ViewName, string CustomId, string eventticketid, string additional, bool? chkassign, bool? isexist)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            bool dropres = false;
            string message = "";
            string LogMessage = "";
            string LogUserName = "";
            Guid customerId = Guid.Empty;
            var Endremdate = new DateTime();
            var reminderEndTime = "";
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
            if(!string.IsNullOrWhiteSpace(ViewName) && ViewName == "month" && !string.IsNullOrWhiteSpace(eventType))
            {
                if(!string.IsNullOrWhiteSpace(eventticketid) && eventticketid != new Guid().ToString())
                {
                    var objticketuser = _Util.Facade.TicketFacade.GetTicketUserListByUserId(new Guid(eventticketid), System.Web.HttpUtility.UrlDecode(eventType));
                    if(objticketuser.Count > 0)
                    {
                        foreach(var item in objticketuser)
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
                            if(objappadditional != null)
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
                        if(isexist.HasValue && isexist.Value)
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
                                        if(string.IsNullOrWhiteSpace(message))
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
                                if(chkassign.HasValue && chkassign.Value == false)
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
                                    if(objemp != null)
                                    {
                                        var userpermission = _Util.Facade.PermissionFacade.GetPermissionGroupByUserId(objemp.UserId);
                                        if(userpermission != null && (userpermission.Name.ToLower().IndexOf("technician") > -1 || userpermission.Name.ToLower().IndexOf("installation") > -1 || objemp.UserId == new Guid("22222222-2222-2222-2222-222222222222") || userpermission.Tag.ToLower().IndexOf("technician") > -1 || userpermission.Tag.ToLower().IndexOf("installation") > -1))
                                        {
                                            var objapp = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentidByAppoinmentId(new Guid(eventAppid));
                                            var tikuser = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndIsPrimary(objemp.UserId, objticket.TicketId);
                                            if (objapp != null && tikuser != null)
                                            {
                                                tikuser.UserId = objemp.UserId;
                                                objapp.AppointmentDate = Convert.ToDateTime(spliteventDate[0]).SetZeroHour();
                                                objapp.IsAllDay = true;
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

        [HttpPost]
        public JsonResult ScheduleEventDetails(Guid? userid, string type, string date)
        {
            bool result = false;
            List<TicketUser> model = new List<TicketUser>();
            if(userid.HasValue && userid.Value != new Guid() && !string.IsNullOrWhiteSpace(type) && !string.IsNullOrWhiteSpace(date))
            {
                type = System.Web.HttpUtility.UrlDecode(type);
                var mindate = date + " 00:00:00.000";
                var maxdate = date + " 23:59:59.999";
                var ticketuser = _Util.Facade.TicketFacade.GetTicketUserListAndCustomerAppointmentByUserId(userid.Value, type, mindate, maxdate);
                if(ticketuser != null && ticketuser.Count > 0)
                {
                    model = ticketuser;
                    result = true;
                }
            }
            return Json(new { result = result, model = model });
        }

        public JsonResult DroppingPermissionScheduleCalendar(string eventType, int? eventId, string eventDate, string eventAppid, bool eventAllDay, string eventEndDate, string Eventresid, string dragresid, string ViewName, string CustomId, string eventticketid)
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
                        if(objpriticket != null)
                        {
                            ExistUserAssign = true;
                        }
                        else if(objticketuser != null)
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

        public ActionResult ScheduleGoogleMap(string date, string type, string user)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            string GoogleMapAPIKey = _Util.Facade.GlobalSettingsFacade.GetGoogleMapAPIKeyByCompanyId(currentLoggedIn.CompanyId.Value);
            ViewBag.GoogleMapAPIKey = GoogleMapAPIKey;

            List<PreCustomerNote> model = new List<PreCustomerNote>();
            if (!string.IsNullOrWhiteSpace(date))
            {
                var spdate = date.Split('T');
                if(spdate.Length > 0)
                {
                    date = spdate[0];
                }
                if(!string.IsNullOrWhiteSpace(type) && type != "null")
                {
                    var sptype = type.Split(',');
                    if(sptype.Length > 0)
                    {
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
                model = _Util.Facade.ScheduleFacade.GetSchedulingByCompanyIdAndFilterForGoogleMap(currentLoggedIn.CompanyId.Value, date, type, user, ispermit, false);
            }
            ViewBag.ZoomLevel = _Util.Facade.GlobalSettingsFacade.GetMapZoomLevel(currentLoggedIn.CompanyId.Value);
            return View(model);
        }

        public ActionResult TicketStatusImageSettingPartial(string status)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            StatusImageSetting model = new StatusImageSetting();
            if (!string.IsNullOrWhiteSpace(status))
            {
                model.TicketStatusImageSetting = _Util.Facade.TicketFacade.GetStatusImageSettingByCompanyIdAndStatus(currentLoggedIn.CompanyId.Value, status);
                if(model.TicketStatusImageSetting == null)
                {
                    model.StatusTicket = status;
                }
            }
            ViewBag.TicketStatus = _Util.Facade.LookupFacade.GetLookupByKey("TicketStatus").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveTicketStatusImageSetting(TicketStatusImageSetting TicketStatusImageSetting)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if(TicketStatusImageSetting.Id > 0)
            {
                var objstatusimage = _Util.Facade.TicketFacade.GetImageSettingById(TicketStatusImageSetting.Id);
                if(objstatusimage != null)
                {
                    objstatusimage.Filename = TicketStatusImageSetting.Filename;
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

        [HttpPost]
        public JsonResult OnlyEventResizeHandlerCalendar(string eventType, int? eventId, string eventDate, string eventAppid, bool eventAllDay, string eventEndDate, string Eventresid, string dragresid, string ViewName, string CustomId, string eventticketid, string additional, bool? chkassign, bool? isexist)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            bool dropres = false;
            string message = "";
            var Endremdate = new DateTime();
            var reminderEndTime = "";
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
                    if(!string.IsNullOrWhiteSpace(eventticketid) && !string.IsNullOrWhiteSpace(Eventresid))
                    {
                        var objticket = _Util.Facade.TicketFacade.GetTicketByTicketId(new Guid(eventticketid));
                        var objemp = _Util.Facade.EmployeeFacade.GetEmployeeById(Convert.ToInt32(Eventresid));
                        if(objticket != null && objemp != null)
                        {
                            var objticketuser = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndUserIdAndNotification(new Guid(eventAppid), objemp.UserId);
                            if (objticketuser != null)
                            {
                                var objadditionalapp = _Util.Facade.CustomerAppoinmentFacade.GetAdditionalMembersAppointmentByAppointmentIdAndUserId(new Guid(eventAppid), objemp.UserId);
                                if(objadditionalapp != null)
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
                                if(objappointment != null)
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

        [HttpPost]
        public JsonResult UserPermissionForCreateTicket(string Eventresid, string eventloaddate, string customerid, string ticketid)
        {
            bool result = false;
            string message = "";
            if (!string.IsNullOrWhiteSpace(Eventresid))
            {
                var empobj = _Util.Facade.EmployeeFacade.GetEmployeeById(Convert.ToInt32(Eventresid));
                if(empobj != null)
                {
                    var userpermission = _Util.Facade.PermissionFacade.GetPermissionGroupByUserId(empobj.UserId);
                    if(userpermission != null && (userpermission.Name.ToLower().IndexOf("sysadmin") > -1 || userpermission.Name.ToLower().IndexOf("admin") > -1 || userpermission.Name.ToLower().IndexOf("technician") > -1 || userpermission.Name.ToLower().IndexOf("installation") > -1 || empobj.UserId == new Guid("22222222-2222-2222-2222-222222222222") || userpermission.Tag.ToLower().IndexOf("technician") > -1 || userpermission.Tag.ToLower().IndexOf("installation") > -1))
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
    }
}