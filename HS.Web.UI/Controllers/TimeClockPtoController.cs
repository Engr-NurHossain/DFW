using HS.Entities;
using HS.Framework;
using HS.Web.UI.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using NLog;
using Newtonsoft.Json; 
using HS.Facade;

namespace HS.Web.UI.Controllers
{
    public class TimeClockPtoController : BaseController
    {
        public TimeClockPtoController()
        {
            logger=LogManager.GetCurrentClassLogger();
        }
        public string PushNotification(EmployeeTimeClock model, Guid UserId, bool result)
        {
            string str = "";
            try
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                //string applicationID = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ApplicationID").ToString();
                var applicationID = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ApplicationID").Value;

                //string senderId = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "SenderId").ToString();
                var senderId = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "SenderId").Value;

                var deviceId = _Util.Facade.UserCompanyFacade.GetDeviceIdForChangeCompany(CurrentUser.CompanyId.Value, UserId);

                List<string> DeviceIdList = new List<string>();

                foreach (var item in deviceId)
                {
                    DeviceIdList.Add(item.DeviceId.ToString());
                }

                if (result == true)
                {
                    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                    tRequest.Method = "post";

                    tRequest.ContentType = "application/json";

                    var data = new

                    {

                        to = string.Join(",", DeviceIdList),//deviceId,

                        data = new

                        {
                            NotificationType = "clock_in",

                            ClockInTime = model.ClockInTime.ToString(),

                            UserId = UserId.ToString(),

                            CompanyId = CurrentUser.CompanyId.Value,
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
                else
                {

                    WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                    tRequest.Method = "post";

                    tRequest.ContentType = "application/json";

                    var data = new

                    {

                        to = string.Join(",", DeviceIdList),

                        data = new

                        {
                            NotificationType = "clock_out",

                            Flag = true,

                            UserId = UserId.ToString(),

                            CompanyId = CurrentUser.CompanyId.Value,
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

            }

            catch (Exception ex)
            {
                return str = ex.Message;
            }
        }

        // GET: TimeClockPto
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }

        [Authorize]
        public PartialViewResult TimeClockHome()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;


            if (base.IsPermitted(UserPermissions.ReportMenuPermission.TimeClockTimeClockTab))
            {
                ViewBag.StartTab = "TimeClockTimeClockTab";
            }
            else if (base.IsPermitted(UserPermissions.ReportMenuPermission.TimeClockPTOTab))
            {
                ViewBag.StartTab = "TimeClockPTOTab";
            }
            else if (base.IsPermitted(UserPermissions.ReportMenuPermission.TimeClockEmployeesTimeClockTab))
            {
                ViewBag.StartTab = "TimeClockEmployeesTimeClockTab";
            }
            else if (base.IsPermitted(UserPermissions.ReportMenuPermission.TimeClockEmployeesPTOTab))
            {
                ViewBag.StartTab = "TimeClockEmployeesPTOTab";
            }
            else if (base.IsPermitted(UserPermissions.ReportMenuPermission.PayrollTab))
            {
                ViewBag.StartTab = "PayrollTab";
            }
            else if (base.IsPermitted(UserPermissions.ReportMenuPermission.AccrualPTOTab))
            {
                ViewBag.StartTab = "AccrualPTOTab";
            }
            else
            {
                //ViewBag.StartTab = "TimeClockEmployeesTimeClockTab";
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }



            ViewBag.IsSupervisor = CurrentUser.IsSupervisor;
            ViewBag.ispayroll = _Util.Facade.EmployeeFacade.GetEmployeeByUsername(CurrentUser.Identity.Name).IsPayroll;
            return PartialView("_TimeClockHome");
        }
        #region Accrual Pto
        [Authorize]
        public PartialViewResult AccrualPTOReportView()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<Employee> empLsit = new List<Employee>();

            empLsit = _Util.Facade.EmployeeFacade.GetCurrentEmployeeListByCompanyId(CurrentUser.CompanyId.Value);

            List<SelectListItem> listemp = new List<SelectListItem>();
            listemp.Insert(0, new SelectListItem()
            {
                Text = "Select User",
                Value = new Guid().ToString()
            });
            listemp.AddRange(empLsit.OrderBy(x => x.FirstName + " " + x.LastName).Select(x =>
                  new SelectListItem()
                  {
                      Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                      Value = x.UserId.ToString()
                  }).ToList());


            ViewBag.EmployeeList = listemp;
            return PartialView("_AccrualPTOReportView");
        }
        [Authorize]
        public ActionResult GetAllEmploployeeAccrualPTOReport(PayrollFilterModel filter)
        {
            double TotalPtoHour = 0.0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            EmployeeAccrualPtoAndApprovePtohourModel model = _Util.Facade.EmployeeFacade.GetEmployeeAccrualPTOList(filter);
            ViewBag.pagesize = filter.PageSize;
            ViewBag.startdate = filter.StrStartDate;
            ViewBag.enddate = filter.StrEndDate;
            ViewBag.order = filter.order;
            return PartialView("_AccrualPTOReport", model); 
        }

        [Authorize]
        public ActionResult EmployeeTimeClockListAccrualPto(string UserId, string StrStartDate, string StrEndDate)
        {
            DateTime fstartdate = new DateTime();
            DateTime fenddate = new DateTime();
            if (!string.IsNullOrWhiteSpace(StrStartDate) && !string.IsNullOrWhiteSpace(StrEndDate))
            {
                 fstartdate = Convert.ToDateTime(StrStartDate).SetZeroHour();
                 fenddate = Convert.ToDateTime(StrEndDate).SetMaxHour();
            }
            //else
            //{
            //    fstartdate = new DateTime();
            //    fenddate = new DateTime();
            //}
            EmployeeAccrualPtoAndApprovePtohourModel model = _Util.Facade.EmployeeTimeClockFacade.GetLastClocksAccrualPTOByUserIdAndTimePeriod(UserId, fstartdate, fenddate);
            if (model != null)
            {
                if(model.EmployeePTOHourLogList != null && model.EmployeePTOHourLogList.Count() > 0)
                {
                    model.EmployeePTOHourLogList = model.EmployeePTOHourLogList;
                }
                else
                {
                    model.EmployeePTOHourLogList = new List<EmployeePTOHourLog>();
                }
                if (model.schedulerList != null && model.schedulerList.Count() > 0)
                {
                    model.schedulerList = model.schedulerList;
                }
                else
                {
                    model.schedulerList = new List<EmployeePTOHourLog>();
                }
                if (model.approveLogList != null && model.approveLogList.Count() > 0)
                {
                    model.approveLogList = model.approveLogList;
                }
                else
                {
                    model.approveLogList = new List<EmployeePTOHourLog>();
                }

            }
            else
            {
                model = new EmployeeAccrualPtoAndApprovePtohourModel();
                model.EmployeePTOHourLogList = new List<EmployeePTOHourLog>();
                model.schedulerList = new List<EmployeePTOHourLog>();
                model.approveLogList = new List<EmployeePTOHourLog>();
            }

            ViewBag.PageNumber = 1;
            ViewBag.pagesize = 10;
            ViewBag.startdate = fstartdate;
            ViewBag.enddate = fenddate;
            ViewBag.order = "";
            return View(model);
        }
        #endregion Accrual Pto

            #region Employee Time Clocks Home

        public PartialViewResult EmployeeTimeClocksHome(int? Id)
        {
            //Id = 1068;
            List<Employee> empLsit = new List<Employee>();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!CurrentUser.IsSupervisor && CurrentUser.UserTags.ToLower().IndexOf("hrmanager") == -1)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            //if (!string.IsNullOrWhiteSpace(CurrentUser.UserTags))
            //{
            //    if (CurrentUser.UserTags.ToLower().IndexOf("hrmanager") > -1 || CurrentUser.UserTags.ToLower().IndexOf("admin") > -1 || CurrentUser.UserTags.ToLower().IndexOf("sysadmin") > -1)
            //    {
            //        empLsit = _Util.Facade.EmployeeFacade.GetEmployeeListByUserTag(CurrentUser.CompanyId.Value, CurrentUser.UserId);

            //    }
            //    else
            //    {
            //        empLsit = _Util.Facade.EmployeeFacade.GetEmployeeListBySupervisorId(CurrentUser.UserId);
            //    }
            //}
            //else
            //{
            //    empLsit = _Util.Facade.EmployeeFacade.GetEmployeeListBySupervisorId(CurrentUser.UserId);
            //}

            //if (Id.HasValue && Id > 0)
            //{
            //    Employee emp = empLsit.Where(x => x.Id == Id.Value).FirstOrDefault();
            //    if (emp != null)
            //    {
            //        empLsit.RemoveAt(empLsit.FindIndex(a => a.Id == Id.Value));
            //        empLsit.Insert(0, emp);
            //    }
            //}

            empLsit = _Util.Facade.EmployeeFacade.GetCurrentEmployeeListByCompanyId(CurrentUser.CompanyId.Value);

            if(!IsPermitted(UserPermissions.TimeClockPtoPermission.ShowAllEmployeeInEmployeeTimeClock))
            {
                empLsit = empLsit.Where(x => x.UserId == CurrentUser.UserId || (!string.IsNullOrWhiteSpace(x.SuperVisorId) && x.SuperVisorId!="-1" && new Guid(x.SuperVisorId) == CurrentUser.UserId)).ToList();
            }

            List<SelectListItem> employlist = new List<SelectListItem>();
            if (!IsPermitted(UserPermissions.TimeClockPtoPermission.EmployeesTimeClockListAll))
            {
                employlist = empLsit.OrderBy(x => x.FirstName + " " + x.LastName).Select(x =>
                 new SelectListItem()
                 {
                     Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                     Value = x.UserId.ToString(),
                     Selected = true
                 }).ToList();
                if (empLsit != null && empLsit.Count > 0)
                {
                    employlist.Insert(0, new SelectListItem()
                    {
                        Text = "All",
                        Value = "All",
                        Selected = true
                    });
                }
            }
            else
            {
                employlist = empLsit.OrderBy(x => x.FirstName + " " + x.LastName).Select(x =>
                 new SelectListItem()
                 {
                     Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                     Value = x.UserId.ToString(),
                 }).ToList();
            }

            ViewBag.empLsit = employlist;
            return PartialView("_EmployeeTimeClocksHome", empLsit);
        }

        #endregion

        #region Employee TimeClock
        [Authorize]
        public PartialViewResult EmployeeTimeClock(string UserId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.UserId = UserId;
            return PartialView("_EmployeeTimeClock");
        }

        [Authorize]
        public PartialViewResult EmployeeTimeClockList(string UserId, string StrStartDate, string StrEndDate, string order, int pageno, int pagesize)
        {
            List<string> userlist = new List<string>();

            TimeClockFilterModel model = new TimeClockFilterModel();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Customer customer = new Customer();
            UserLogin user = new UserLogin();
            var systemCheckOut = "22222222-2222-2222-2222-222222222222";
            if (!string.IsNullOrWhiteSpace(UserId) && UserId != "null")
            {
                string[] splituser = UserId.Split(',');
                if (splituser.Length > 0)
                {
                    if (splituser[0] == "All")
                    {
                        splituser[0] = new Guid().ToString();
                    }
                    UserId = string.Format("{0}", string.Join("','", splituser.Select(i => i.Replace("'", "''"))));
                    foreach (var item in splituser)
                    {
                        userlist.Add(item);
                    }
                }
                GlobalSetting gs = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsByCompanyId(CurrentUser.CompanyId.Value).Where(x => x.Tag == "UtilitySetting" && x.SearchKey == "CompanyStartDate").FirstOrDefault();
                var fstartdate = Convert.ToDateTime(StrStartDate).SetZeroHour().ClientToUTCTime();
                var fenddate = Convert.ToDateTime(StrEndDate).SetMaxHour().ClientToUTCTime();
                if (fstartdate == new DateTime() && fenddate == new DateTime())
                {
                    fstartdate = Convert.ToDateTime(gs.Value).ClientToUTCTime();
                    var todaysdate = DateTime.Now.AddYears(1);
                    var dateend = "12/31/" + todaysdate.Year.ToString() + " 00:00:00.000";
                    fenddate = Convert.ToDateTime(dateend).SetMaxHour().ClientToUTCTime();
                }
              
                model = _Util.Facade.EmployeeTimeClockFacade.GetLastClocksByUserIdAndTimePeriod(UserId, fstartdate, fenddate, order, pageno, pagesize);
            }
            if (model.ListTimeClock != null && model.ListTimeClock.Count > 0)
            {
                foreach (var item in model.ListTimeClock)
                {

                    if (item.LastUpdateBy != CurrentUser.UserId)
                    {
                        if (item.LastUpdateBy != new Guid(systemCheckOut))
                        {
                            user = _Util.Facade.UserLoginFacade.GetUserLoginByUserId(item.LastUpdateBy);
                            customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(item.LastUpdateBy);
                            if (customer != null)
                            {
                                item.LastUpdatedName = customer.FirstName + " " + customer.LastName;
                            }
                            //else if (user != null)
                            //{
                            //    item.LastUpdatedName = user.UserName;
                            //}
                        }
                    }
                }
            }

            ViewBag.UserId = UserId;
            if (model.TotalCount != null && model.TotalCount.CountTotal == 0)
            {
                pageno = 1;
            }

            ViewBag.PageNumber = pageno;
            ViewBag.OutOfNumber = 0;


            if (model.ListTimeClock != null && model.ListTimeClock.Count() > 0)
            {
                ViewBag.OutOfNumber = model.TotalCount.CountTotal;
            }

            if ((int)ViewBag.PageNumber * pagesize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * pagesize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / pagesize);
            ViewBag.pagesize = pagesize;
            ViewBag.order = order;

            return PartialView("_EmployeeTimeClockList", model);
        }

        #endregion


        #region Personal PTO
        [Authorize]
        public PartialViewResult PTOPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(CurrentUser.UserId);
            ViewBag.HoursRemaining = emp.PtoRemain.HasValue ? emp.PtoRemain.Value : 0;

            return PartialView("_PTOPartial");
        }
        [Authorize]
        public PartialViewResult PTOListPartial(PayrollFilterModel filter, bool? getreport)
        {
            PtoFilterModel model = new PtoFilterModel();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            model = _Util.Facade.PtoFacade.GetAllPtoByUserId(CurrentUser.UserId, filter, getreport);

            List<string> status = new List<string>();
            if (!string.IsNullOrWhiteSpace(filter.PtoStatus))
            {
                string[] splituser = filter.PtoStatus.Split(',');
                if (splituser.Length > 0)
                {
                    filter.PtoStatus = string.Format("{0}", string.Join(",", splituser));
                    foreach (var item in splituser)
                    {
                        status.Add(item);
                    }
                }
            }

            if (model.ListPto.Count > 0)
            {
                foreach (var item in model.ListPto)
                {
                    int DaysCount = 0;
                    if (item.StartDate.HasValue
                        && item.EndDate.HasValue
                        && item.EndDate > item.StartDate)
                    {
                        DaysCount = (item.EndDate - item.StartDate).Value.Days;
                    }
                    DaysCount++;

                    if (item.Type == "FullDay")
                    {
                        item.PTOMinutes = DaysCount * 8 * 60;
                    }
                    else if (item.Type == "HalfDay")
                    {
                        item.PTOMinutes = DaysCount * 4 * 60;
                    }
                    else if (item.Type == "CustomTime")
                    {
                        if (item.TimeFrom != "-1" && item.TimeTo != "-1"
                            && !string.IsNullOrWhiteSpace(item.TimeFrom) && !string.IsNullOrWhiteSpace(item.TimeTo))
                        {

                            DateTime d1 = new DateTime();
                            d1 = Convert.ToDateTime(item.TimeTo);

                            DateTime d2 = new DateTime();
                            d2 = Convert.ToDateTime(item.TimeFrom);

                            TimeSpan ts = d1.Subtract(d2);
                            double customHour = ts.TotalMinutes;
                            item.PTOMinutes = Convert.ToInt32(customHour);

                        }
                    }
                    else if (item.Type == "MultipleDay")
                    {
                        int Totalcount = 0;
                        int TotalDays = (int)(Convert.ToDateTime(item.EndDate.Value) - Convert.ToDateTime(item.StartDate)).TotalDays;
                        double customHour = (TotalDays + 1) * 8;
                        Totalcount = Convert.ToInt32(customHour * 60);
                    }

                    else
                    {
                        item.PTOMinutes = 0;
                    }
                }
            }
            if (model.TotalCountPto.CountTotal == 0)
            {
                filter.pageno1 = 1;
            }

            ViewBag.PageNumber = filter.pageno1;
            ViewBag.OutOfNumber = 0;


            if (model.ListPto.Count() > 0)
            {
                ViewBag.OutOfNumber = model.TotalCountPto.CountTotal;
            }

            if ((int)ViewBag.PageNumber * filter.pagesize1 > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.pagesize1;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.pagesize1);
            ViewBag.pagesize = filter.pagesize1;
            _Util.Facade.LookupFacade.GetLookupByKey("AbsenceType");
            ViewBag.order = filter.order;
            ViewBag.liststatus = status;
            return PartialView("_PTOListPartial", model);
        }

        [Authorize]
        public PartialViewResult AddPtoPartial(int? Id)
        {
            var CurrenUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Pto Model = new Pto();

            if (Id.HasValue && Id > 0)
            {
                Model = _Util.Facade.PtoFacade.GetPtoById(Id);
                if (Model == null || Model.UserId != CurrenUser.UserId)
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
            ViewBag.LeaveType = _Util.Facade.LookupFacade.GetLookupByKey("AbsenceType").OrderBy(x => x.DisplayText != "Select Absence Type").ThenBy(x => x.DisplayText).Select(x =>
                 new SelectListItem()
                 {
                     Text = x.DisplayText.ToString(),
                     Value = x.DataValue.ToString()
                 }).ToList();

            ViewBag.ArrivalTime = _Util.Facade.LookupFacade.GetLookupByKey("AbsenceCustomTime").Where(x => x.IsActive == true).Select(x =>
                    new SelectListItem()
                    {
                        Text = x.DisplayText.ToString(),
                        Value = x.DataValue.ToString()
                    }).ToList();
            #endregion

            return PartialView("_AddPtoPartial", Model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddPto(Pto Model, bool? getreport)
        {
            PtoFilterModel model = new PtoFilterModel();
            bool result = false;
            bool warning = false;
            string message = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Guid UserId = new Guid();
            if (Model.UserId != new Guid())
            {
                UserId = Model.UserId;
            }
            else
            {
                UserId = CurrentUser.UserId;
            }
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(UserId);
            PayrollFilterModel payrol = new PayrollFilterModel()
            {
                StartDate = Model.StartDate.Value,
                EndDate = Model.EndDate.Value

            };
            string StartDay = Model.StartDate.Value.ToString("ddd");
            string EndDay = Model.EndDate.Value.ToString("ddd");

            model = _Util.Facade.PtoFacade.GetAllPtoByUserId(UserId, payrol, getreport);
            if (model.ListPto.Count == 0)
            {
                //if (StartDay == "Sat" || StartDay == "Sun" || EndDay == "Sat" || EndDay == "Sun")
                //{
                //    message = "You can not apply PTO on weekend.";
                //    result = false;
                //}
                //else
                //{

                //}
                Model.CreatedBy = CurrentUser.UserId;
                Model.CreatedDate = DateTime.Now.UTCCurrentTime();
                Model.Status = LabelHelper.PTOStatus.SentToSupervisor;
                Model.LastUpdatedBy = CurrentUser.UserId;
                Model.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                Model.UserId = UserId;
                double hour = 0;
                double RemainigPtoHour = 0;
                double TotalPtoHour = 0;


                if (Model.Type == "FullDay")
                {
                    hour = 8;
                }
                else if (Model.Type == "HalfDay")
                {
                    hour = 4;
                }
                else if (Model.Type == "CustomTime")
                {
                    DateTime d1 = new DateTime();
                    d1 = Convert.ToDateTime(Model.TimeTo);

                    DateTime d2 = new DateTime();
                    d2 = Convert.ToDateTime(Model.TimeFrom);

                    TimeSpan ts = d1.Subtract(d2);

                    //DateTime date1 = DateTime.ParseExact("11:30 PM", "HH:mm tt", new DateTimeFormatInfo());
                    //DateTime date2 = DateTime.ParseExact(Model.TimeFrom, "HH:mm tt", new DateTimeFormatInfo());
                    //TimeSpan time = date1.Subtract(date2);

                    double customHour = ts.TotalMinutes;
                    Model.PTOMinutes = Convert.ToInt32(customHour);

                    hour = customHour / 60;


                }
                else if (Model.Type == "MultipleDay" && Model.Status == "Accepted")
                {
                    int TotalDays = (int)(Convert.ToDateTime(Model.EndDate.Value) - Convert.ToDateTime(Model.StartDate)).TotalDays;
                    double customHour = (TotalDays + 1) * 8;
                    hour = Convert.ToInt32(customHour * 60);

                }

                if (emp.PtoHour.HasValue)
                {
                    double totalMinute = 0.0;
                    List<Pto> AcceptedPto = _Util.Facade.PtoFacade.GetAllPtoListByUserId(CurrentUser.UserId).Where(x => x.Status == "Accepted").ToList();
                    foreach (var item in AcceptedPto)
                    {
                        totalMinute = item.Minute.HasValue ? Convert.ToDouble(item.Minute.Value) / 60 : 0;
                        TotalPtoHour = TotalPtoHour + totalMinute;
                    }

                    if (TotalPtoHour < 0)
                    {
                        RemainigPtoHour = emp.PtoHour.Value + (TotalPtoHour - hour);
                    }
                    else
                    {
                        RemainigPtoHour = emp.PtoHour.Value - (TotalPtoHour + hour);
                    }
                }
                else
                {
                    emp.PtoHour = 0;
                    RemainigPtoHour = emp.PtoHour.Value - hour;
                }
                if (Model.Type == "CustomTime")
                {
                    DateTime d1 = new DateTime();
                    d1 = Convert.ToDateTime(Model.TimeTo);

                    DateTime d2 = new DateTime();
                    d2 = Convert.ToDateTime(Model.TimeFrom);

                    TimeSpan ts = d1.Subtract(d2);

                    int customMinute = Convert.ToInt32(ts.TotalMinutes);
                    if (customMinute > 480)
                    {
                        message = "Custom time is greater than 8 hours";
                        result = false;
                    }
                    else
                    {

                        //if (RemainigPtoHour > 0)
                        //{

                        Model.Id = _Util.Facade.PtoFacade.InsertPto(Model);
                        message = "Send to supervisor successfully.";
                        result = true;
                        //}
                        //else
                        //{
                        //    Model.Id = _Util.Facade.PtoFacade.InsertPto(Model);
                        //    message = "Absence time will exceed your pto hour";
                        //    result = false;
                        //    warning = true;
                        //}
                        Guid SupervisorId = new Guid();
                        if (Guid.TryParse(emp.SuperVisorId, out SupervisorId) && SupervisorId != new Guid())
                        {
                            #region Insert notification
                            Notification notification = new Notification()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                NotificationId = Guid.NewGuid(),
                                Type = LabelHelper.NotificationType.Employee,
                                Who = CurrentUser.UserId,
                                What = string.Format(@"{0} applied for a PTO.", "{0}"),
                                NotificationUrl = "/TimeClock/#EmployeesPTO"

                            };
                            //message = "Send to supervisor successfully.";
                            _Util.Facade.NotificationFacade.InsertNotification(notification);
                            #endregion

                            #region set user to notification
                            NotificationUser nu = new NotificationUser()
                            {
                                NotificationId = notification.NotificationId,
                                IsRead = false,
                                NotificationPerson = SupervisorId,
                            };
                            _Util.Facade.NotificationFacade.InsertNotificationUser(nu);


                        }
                        #endregion
                    }
                }
                else
                {
                    //if (RemainigPtoHour > 0)
                    //{

                    Model.Id = _Util.Facade.PtoFacade.InsertPto(Model);
                    message = "Send to supervisor successfully.";
                    result = true;
                    //}
                    //else
                    //{

                    //    Model.Id = _Util.Facade.PtoFacade.InsertPto(Model);
                    //    message = "Absence time will exceed your pto hour";
                    //    result = false;
                    //    warning = true;
                    //}



                    Guid SupervisorId = new Guid();
                    if (Guid.TryParse(emp.SuperVisorId, out SupervisorId) && SupervisorId != new Guid())
                    {
                        #region Insert notification
                        Notification notification = new Notification()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            NotificationId = Guid.NewGuid(),
                            Type = LabelHelper.NotificationType.Employee,
                            Who = CurrentUser.UserId,
                            What = string.Format(@"{0} applied for a PTO.", "{0}"),
                            NotificationUrl = "/TimeClock/#EmployeesPTO"

                        };

                        _Util.Facade.NotificationFacade.InsertNotification(notification);
                        #endregion

                        #region set user to notification
                        NotificationUser nu = new NotificationUser()
                        {
                            NotificationId = notification.NotificationId,
                            IsRead = false,
                            NotificationPerson = SupervisorId,
                        };
                        _Util.Facade.NotificationFacade.InsertNotificationUser(nu);


                    }

                    #endregion
                }
            }
            else
            {
                message = "You already added PTO for this date";
                result = false;
            }

            return Json(new { result = result, message = message, warning = warning });
        }
        #endregion

        #region Accept/Reject PTO
        [Authorize]
        [HttpGet]
        public ActionResult AddClock(Guid UserId, int? TimeClockId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            #region Security Check
            if (!CurrentUser.IsSupervisor && CurrentUser.UserTags.ToLower().IndexOf("hrmanager") == -1)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(UserId);
            Guid SupervisorId = new Guid();

            PermissionGroup pg = _Util.Facade.PermissionFacade.GetPermissionGroupByUserId(CurrentUser.UserId);
            #endregion

            EmployeeTimeClock tc = new EmployeeTimeClock();
            //tc.UserId = UserId;
            ViewBag.Intime = "00:00:01";
            ViewBag.Outtime = "00:00:01";
            if (TimeClockId.HasValue && TimeClockId > 0)
            {
                tc = _Util.Facade.EmployeeTimeClockFacade.GetEmployeeTimeClockById(TimeClockId.Value);
                string strClockIntime = tc.ClockInTime.UTCToClientTime().ToString("HH:mm");
                ViewBag.Intime = strClockIntime;
                if (tc.ClockOutTime.HasValue)
                {
                    string strClockOuttime = tc.ClockOutTime.Value.UTCToClientTime().ToString("HH:mm");
                   
                    ViewBag.Outtime = strClockOuttime;
                }
                #region Security Check
                if (tc == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                #endregion
                ViewBag.EditClock = "Edit Clock In/Out";
            }

            if (!tc.ClockedInSeconds.HasValue)
            {
                tc.ClockedInSeconds = 0;
            }
            #region ViewBag
            ViewBag.SelectType = _Util.Facade.LookupFacade.GetLookupByKey("ClockType").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString()
               }).ToList();
            //ViewBag.UserId = UserId;
            #endregion
            List<Employee> empLsit = new List<Employee>();
            //if (!string.IsNullOrWhiteSpace(CurrentUser.UserTags))
            //{
            //    if (CurrentUser.UserTags.ToLower().IndexOf("hrmanager") > -1 || CurrentUser.UserTags.ToLower().IndexOf("admin") > -1 || CurrentUser.UserTags.ToLower().IndexOf("sysadmin") > -1)
            //    {
            //        empLsit = _Util.Facade.EmployeeFacade.GetEmployeeListByUserTag(CurrentUser.CompanyId.Value, CurrentUser.UserId);

            //    }
            //    else
            //    {
            //        empLsit = _Util.Facade.EmployeeFacade.GetEmployeeListBySupervisorId(CurrentUser.UserId);
            //    }
            //}
            //else
            //{
            //    empLsit = _Util.Facade.EmployeeFacade.GetEmployeeListBySupervisorId(CurrentUser.UserId);
            //}

            empLsit= _Util.Facade.EmployeeFacade.GetCurrentEmployeeListByCompanyId(CurrentUser.CompanyId.Value);

            //if (emp != null)
            //{
            //    empLsit.RemoveAt(empLsit.FindIndex(a => a.Id == emp.Id));
            //    empLsit.Insert(0, emp);
            //}
            List<SelectListItem> listemp = new List<SelectListItem>();
            listemp.Insert(0, new SelectListItem()
            {
                Text = "Select User",
                Value = new Guid().ToString()
            });
            listemp.AddRange(empLsit.OrderBy(x => x.FirstName + " "+x.LastName).Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                    Value = x.UserId.ToString()
                }).ToList());
            ViewBag.empLsit = listemp;
            ViewBag.validtimeclock = TimeClockId.HasValue ? TimeClockId.Value : 0;
            return View(tc);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddEmployeesClock(EmployeeTimeClock ClockAdd)
        {
            string Message = "";
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var ClockInDateTime = ClockAdd.ClockInTime;
            string IntimeDate = ClockAdd.ClockInTime.Date.ToString("MM/dd/yyyy");
            ClockInDateTime = DateTime.Parse(IntimeDate + " " + ClockAdd.Intime);
            ClockAdd.ClockInTime = ClockInDateTime;
            var ClockOutDateTime = new DateTime();
            ClockAdd.LastUpdateBy = CurrentUser.UserId;
            ClockAdd.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            ClockAdd.ClockInTime = ClockAdd.ClockInTime.ClientToUTCTime();
            if (ClockAdd.ClockOutTime.HasValue && ClockAdd.ClockOutTime != new DateTime())
            {
                ClockOutDateTime = ClockAdd.ClockOutTime.Value;
                string OuttimeDate = ClockAdd.ClockOutTime.HasValue ? ClockAdd.ClockOutTime.Value.Date.ToString("MM/dd/yyyy"):"";
                ClockOutDateTime = DateTime.Parse(OuttimeDate + " " + ClockAdd.Outtime);
                ClockAdd.ClockOutTime = ClockOutDateTime;
                ClockAdd.ClockOutTime = ClockAdd.ClockOutTime.Value.ClientToUTCTime();
            }
            #region validations
            if (ClockAdd.UserId == Guid.Empty)
            {
                return Json(new { result = result, message = "User Id not found." });
            }
            else if (ClockInDateTime > ClockOutDateTime)
            {
                return Json(new { result = result, message = "Clock in and out time mismatch found." });
            }
            if (ClockAdd.Id > 0)
            {
                var objemptimeclocklistInner = _Util.Facade.EmployeeTimeClockFacade.GetAllEmployeeTimeClockListByDateFilterAndId(ClockAdd.ClockInTime, ClockAdd.ClockOutTime.HasValue ? ClockAdd.ClockOutTime.Value : ClockAdd.ClockInTime, ClockAdd.UserId, ClockAdd.Id);
                if (objemptimeclocklistInner.Count > 0)
                {
                    return Json(new { result = result, message = "Clock in and out time should not overlap this user." });
                }
            }
            else
            {
                var objemptimeclocklist = _Util.Facade.EmployeeTimeClockFacade.GetAllEmployeeTimeClockListByDateFilter(ClockAdd.ClockInTime, ClockAdd.ClockOutTime.HasValue ? ClockAdd.ClockOutTime.Value : ClockAdd.ClockInTime, ClockAdd.UserId);
                if (objemptimeclocklist.Count > 0)
                {
                    return Json(new { result = result, message = "Clock in and out time should not overlap this user." });
                }
            }

            #endregion
            if (ClockAdd.Id > 0)
            {
                EmployeeTimeClock previousTimeClock = _Util.Facade.EmployeeTimeClockFacade.GetEmployeeTimeClockById(ClockAdd.Id);
                if (previousTimeClock != null)
                {
                    previousTimeClock.ClockInTime = ClockAdd.ClockInTime;
                    previousTimeClock.ClockInNote = ClockAdd.ClockInNote;

                    previousTimeClock.ClockOutTime = ClockAdd.ClockOutTime;
                    previousTimeClock.ClockOutNote = ClockAdd.ClockOutNote;

                    previousTimeClock.LastUpdateBy = CurrentUser.UserId;
                    previousTimeClock.LastUpdatedDate = DateTime.Now.UTCCurrentTime();

                    if (ClockAdd.ClockInTime != null && ClockAdd.ClockInTime != new DateTime() && ClockAdd.ClockOutTime.HasValue && ClockAdd.ClockOutTime != new DateTime())
                    {
                        previousTimeClock.ClockedInSeconds = (int)ClockAdd.ClockOutTime.Value.Subtract(ClockAdd.ClockInTime).TotalSeconds;
                    }
                    result = _Util.Facade.EmployeeTimeClockFacade.UpdateEmployeeTimeClock(previousTimeClock);
                }
            }
            else
            {
                ClockAdd.ClockInLat = ClockAdd.Lat;
                ClockAdd.ClockInLng = ClockAdd.Lng;
                ClockAdd.ClockInCreatedBy = CurrentUser.UserId;

                ClockAdd.ClockOutLat = ClockAdd.Lat;
                ClockAdd.ClockOutLng = ClockAdd.Lng;
                ClockAdd.ClockOutCreatedBy = CurrentUser.UserId;

                if (ClockAdd.ClockOutTime.HasValue && ClockAdd.ClockOutTime != new DateTime() && ClockAdd.ClockInTime != null)
                {
                    ClockAdd.ClockedInSeconds = (int)ClockAdd.ClockOutTime.Value.Subtract(ClockAdd.ClockInTime).TotalSeconds;
                }
                _Util.Facade.EmployeeTimeClockFacade.InsertEmployeeTimeClock(ClockAdd);

            }
            return Json(new { result = true, message = Message });
        }
        [Authorize]
        [HttpPost]

        public JsonResult AcceptRejectPTO(int PtoId, bool Accept, string Notes)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!CurrentUser.IsSupervisor && CurrentUser.UserTags.ToLower().IndexOf("hrmanager") == -1 && CurrentUser.UserTags.ToLower().IndexOf("admin") == -1)
            {
                return Json(new { result = false, message = "Permission denied." });
            }
            Pto pto = _Util.Facade.PtoFacade.GetPtoById(PtoId);
            if (pto == null)
            {
                return Json(new { result = false, message = "PTO not found" });
            }
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(pto.UserId);
            if (emp.SuperVisorId != CurrentUser.UserId.ToString() && CurrentUser.UserTags.ToLower().IndexOf("admin") == -1)
            {
                return Json(new { result = false, message = "Permission denied." });
            }
            string Message = "";
            if (Accept)
            {
                double hour = 0;
                double RemainigPtoHour = 0;
                double TotalPtoHour = 0;

                if (pto.Type == "FullDay")
                {
                    hour = 8;
                    pto.Minute = 8 * 60;
                }
                else if (pto.Type == "HalfDay")
                {
                    hour = 4;
                    pto.Minute = 4 * 60;
                }
                else if (pto.Type == "CustomTime")
                {
                    DateTime d1 = new DateTime();
                    d1 = Convert.ToDateTime(pto.TimeTo);

                    DateTime d2 = new DateTime();
                    d2 = Convert.ToDateTime(pto.TimeFrom);

                    TimeSpan ts = d1.Subtract(d2);

                    //DateTime date1 = DateTime.ParseExact("11:30 PM", "HH:mm tt", new DateTimeFormatInfo());
                    //DateTime date2 = DateTime.ParseExact(Model.TimeFrom, "HH:mm tt", new DateTimeFormatInfo());
                    //TimeSpan time = date1.Subtract(date2);

                    double customHour = ts.TotalMinutes;
                    pto.Minute = Convert.ToInt32(customHour);
                    hour = customHour / 60;


                }
                else if (pto.Type == "MultipleDay")
                {
                    pto.Minute = 0;
                    if (pto.StartDate.HasValue && pto.EndDate.HasValue)
                    {
                        int TotalDays = (int)(Convert.ToDateTime(pto.EndDate.Value) - Convert.ToDateTime(pto.StartDate)).TotalDays;
                        double customHour = (TotalDays + 1) * 8;
                        hour = Convert.ToInt32(customHour);
                        pto.Minute = Convert.ToInt32(customHour) * 60;
                    }



                }

                if (emp.PtoHour.HasValue)
                {
                    double totalMinute = 0.0;
                    List<Pto> AcceptedPto = _Util.Facade.PtoFacade.GetAllPtoListByUserId(pto.UserId).Where(x => x.Status == "Accepted").ToList();
                    foreach (var item in AcceptedPto)
                    {
                        totalMinute = item.Minute.HasValue ? Convert.ToDouble(item.Minute.Value) / 60 : 0;
                        TotalPtoHour = TotalPtoHour + totalMinute;
                    }

                    if (TotalPtoHour < 0)
                    {
                        RemainigPtoHour = emp.PtoHour.Value + (TotalPtoHour - hour);
                    }
                    else
                    {
                        RemainigPtoHour = emp.PtoHour.Value - (TotalPtoHour + hour);
                    }
                }
                else
                {
                    emp.PtoHour = 0;
                    RemainigPtoHour = emp.PtoHour.Value - hour;
                }
                if (RemainigPtoHour > 0)
                {
                    emp.PtoRemain = RemainigPtoHour;
                    _Util.Facade.EmployeeFacade.UpdateEmployee(emp);
                    pto.Status = LabelHelper.PTOStatus.Accepted;
                    Message = "PTO accepted successfully.";
                }
                else
                {
                    emp.PtoRemain = RemainigPtoHour;
                    _Util.Facade.EmployeeFacade.UpdateEmployee(emp);
                    pto.Status = LabelHelper.PTOStatus.Accepted;
                    Message = "Absence time will exceed pto hour";
                }

            }
            else
            {
                pto.Status = LabelHelper.PTOStatus.Rejected;
                pto.Notes = Notes;
                Message = "PTO rejected successfully.";
            }
            pto.LastUpdatedBy = CurrentUser.UserId;
            pto.LastUpdatedDate = DateTime.Now.UTCCurrentTime();

            _Util.Facade.PtoFacade.UpdatePTO(pto);


            #region Insert notification
            Notification notification = new Notification()
            {
                CompanyId = CurrentUser.CompanyId.Value,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                NotificationId = Guid.NewGuid(),
                Type = LabelHelper.NotificationType.Employee,
                Who = CurrentUser.UserId,
                What = string.Format(@"{0} {1} a PTO of yours.", "{0}", (Accept ? "accepted" : "rejected")),
                NotificationUrl = "/TimeClock/#PTO"
            };

            _Util.Facade.NotificationFacade.InsertNotification(notification);
            #endregion

            #region set user to notification
            NotificationUser nu = new NotificationUser()
            {
                NotificationId = notification.NotificationId,
                IsRead = false,
                NotificationPerson = pto.UserId,
            };
            _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
            #endregion

            return Json(new { result = true, message = Message });
        }
        public JsonResult GetEmpRemainingPto(int Id, int RequestHour)
        {
            string Message = "Pto remaining hour";
            double requestHour = RequestHour / 60;
            double remainingHour = 0;
            bool Status = false;
            double result = 0.0;
            Pto pto = _Util.Facade.PtoFacade.GetPtoById(Id);
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(pto.UserId);

            remainingHour = emp.PtoRemain.HasValue ? emp.PtoRemain.Value - requestHour : 0;

            if (remainingHour < 0)
            {
                Message = "Warning: Hours requested exceeds available PTO hours.Current Pto hour:";
                result = emp.PtoRemain.Value;
                Status = true;
            }

            return Json(new { result = result, message = Message, status = Status });
        }
        #endregion

        #region Employee PTO

        [Authorize]
        public PartialViewResult EmployeesPto()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!CurrentUser.IsSupervisor && CurrentUser.UserTags.ToLower().IndexOf("hrmanager") == -1)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<Employee> empLsit = new List<Employee>();

            empLsit = _Util.Facade.EmployeeFacade.GetCurrentEmployeeListByCompanyId(CurrentUser.CompanyId.Value);

            List<SelectListItem> listemp = new List<SelectListItem>();
            listemp.Insert(0, new SelectListItem()
            {
                Text = "Select User",
                Value = new Guid().ToString()
            });
            listemp.AddRange(empLsit.OrderBy(x => x.FirstName + " " + x.LastName).Select(x =>
                  new SelectListItem()
                  {
                      Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                      Value = x.UserId.ToString()
                  }).ToList());


            ViewBag.EmployeeList = listemp;

            //List<Pto> Model = _Util.Facade.PtoFacade.GetAllEmployeesPtoBySupervisorId(CurrentUser.UserId);
            System.Web.HttpContext.Current.Session["Path"] = "";
            return PartialView("_EmployeesPto");
        }
        [Authorize]
        public PartialViewResult AddEmployeesPto(int? Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!CurrentUser.IsSupervisor)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            Pto Model = new Pto();
            if (Id.HasValue && Id > 0)
            {
                Model = _Util.Facade.PtoFacade.GetPtoById(Id);
            }
            else
            {
                Model.StartDate = DateTime.Now.AddDays(1);
                Model.EndDate = DateTime.Now.AddDays(1);
            }
            
            //ViewBag.url = System.Web.HttpContext.Current.Session["Path"].ToString();

            #region ViewBags
            List<Employee> empLsit = new List<Employee>();
            
            empLsit = _Util.Facade.EmployeeFacade.GetCurrentEmployeeListByCompanyId(CurrentUser.CompanyId.Value);

            List<SelectListItem> listemp = new List<SelectListItem>();
            listemp.Insert(0, new SelectListItem()
            {
                Text = "Select User",
                Value = new Guid().ToString()
            });
            listemp.AddRange(empLsit.OrderBy(x => x.FirstName + " " + x.LastName).Select(x =>
                  new SelectListItem()
                  {
                      Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                      Value = x.UserId.ToString()
                  }).ToList());


            ViewBag.EmployeeList = listemp;

            ViewBag.LeaveType = _Util.Facade.LookupFacade.GetLookupByKey("AbsenceType").Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();

            ViewBag.ArrivalTime = _Util.Facade.LookupFacade.GetLookupByKey("AbsenceCustomTime").Where(x => x.IsActive == true).Select(x =>
                    new SelectListItem()
                    {
                        Text = x.DisplayText.ToString(),
                        Value = x.DataValue.ToString()
                    }).ToList();
            #endregion

            return PartialView("_AddEmployeesPto", Model);
        }
        [Authorize]
        public PartialViewResult EmployeesPtoListPartial(PayrollFilterModel filter, bool? getreport)
        {
            PtoFilterModel model = new PtoFilterModel();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!CurrentUser.IsSupervisor && CurrentUser.UserTags.ToLower().IndexOf("hrmanager") == -1)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            bool isHrMgr = CurrentUser.UserTags.ToLower().IndexOf("hrmanager") != -1;
            model = _Util.Facade.PtoFacade.GetAllEmployeesPtoBySupervisorId(CurrentUser.UserId, filter, isHrMgr, getreport);

            List<string> status = new List<string>();
            if (!string.IsNullOrWhiteSpace(filter.PtoStatus))
            {
                string[] splituser = filter.PtoStatus.Split(',');
                if (splituser.Length > 0)
                {
                    filter.PtoStatus = string.Format("{0}", string.Join(",", splituser));
                    foreach (var item in splituser)
                    {
                        status.Add(item);
                    }
                }
            }

            #region Counting for indevidual pto munites
            if (model.ListPto.Count > 0)
            {
                foreach (var item in model.ListPto)
                {
                    int DaysCount = 0;
                    if (item.StartDate.HasValue
                        && item.EndDate.HasValue
                        && item.EndDate > item.StartDate)
                    {
                        DaysCount = (item.EndDate - item.StartDate).Value.Days;
                    }
                    DaysCount++;

                    if (item.Type == "FullDay")
                    {
                        item.PTOMinutes = DaysCount * 8 * 60;
                    }
                    else if (item.Type == "HalfDay")
                    {
                        item.PTOMinutes = DaysCount * 4 * 60;
                    }
                    else if (item.Type == "CustomTime")
                    {
                        if (item.TimeFrom != "-1" && item.TimeTo != "-1"
                            && !string.IsNullOrWhiteSpace(item.TimeFrom) && !string.IsNullOrWhiteSpace(item.TimeTo))
                        {

                            DateTime d1 = new DateTime();
                            d1 = Convert.ToDateTime(item.TimeTo);

                            DateTime d2 = new DateTime();
                            d2 = Convert.ToDateTime(item.TimeFrom);

                            TimeSpan ts = d1.Subtract(d2);
                            double customHour = ts.TotalMinutes;
                            item.PTOMinutes = Convert.ToInt32(customHour);

                        }
                    }
                    else if (item.Type == "MultipleDay")
                    {
                        int Totalcount = 0;
                        int TotalDays = (int)(Convert.ToDateTime(item.EndDate.Value) - Convert.ToDateTime(item.StartDate)).TotalDays;
                        double customHour = (TotalDays + 1) * 8;
                        Totalcount = Convert.ToInt32(customHour * 60);
                        item.PTOMinutes = Totalcount;
                    }

                    else
                    {
                        item.PTOMinutes = 0;
                    }
                }
            }
            #endregion

            if (model.TotalCountPto.CountTotal == 0)
            {
                filter.pageno1 = 1;
            }

            ViewBag.PageNumber = filter.pageno1;
            ViewBag.OutOfNumber = 0;


            if (model.ListPto.Count() > 0)
            {
                ViewBag.OutOfNumber = model.TotalCountPto.CountTotal;
            }

            if ((int)ViewBag.PageNumber * filter.pagesize1 > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.pagesize1;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.pagesize1);
            ViewBag.pagesize = filter.pagesize1;
            ViewBag.order = filter.order;
            ViewBag.liststatus = status;
            return PartialView("_EmployeesPtoListPartial", model);
        }


        [Authorize]
        [HttpPost]
        public JsonResult AddEmployeesPto(Pto Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Model.CreatedBy = CurrentUser.UserId;
            Model.CreatedDate = DateTime.Now.UTCCurrentTime();
            Model.Status = LabelHelper.PTOStatus.SentToSupervisor;
            Model.LastUpdatedBy = CurrentUser.UserId;
            Model.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            Model.Id = _Util.Facade.PtoFacade.InsertPto(Model);

            #region Insert notification
            Notification notification = new Notification()
            {
                CompanyId = CurrentUser.CompanyId.Value,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                NotificationId = Guid.NewGuid(),
                Type = LabelHelper.NotificationType.Employee,
                Who = CurrentUser.UserId,
                What = string.Format(@"{0} Created a PTO for you.", "{0}"),
                NotificationUrl = "/TimeClock/#PTO",
            };

            _Util.Facade.NotificationFacade.InsertNotification(notification);
            #endregion

            #region set user to notification
            NotificationUser nu = new NotificationUser()
            {
                NotificationId = notification.NotificationId,
                IsRead = false,
                NotificationPerson = Model.UserId,
            };
            _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
            #endregion


            return Json(new { result = true, message = "PTO Created successfully." });
        }

        public JsonResult CheckPtoRemainingHour(Guid EmployeeId)
        {
            bool result = false;

            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmployeeId);
            if(emp != null && emp.PtoRemain != null)
            {
                if(emp.PtoRemain > 0)
                {
                    result = true;
                }
            }
            return Json(new {result = result });
        }
        #endregion

        #region Time Clock 
        [Authorize]
        public PartialViewResult EmployeeClockInOut()
        {
            EmployeeClockIO model = new EmployeeClockIO();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            model.IsClockedIn = CurrentUser.IsClockedIn;
            model.ClockInOutTime = CurrentUser.ClockInOutTime;
            return PartialView("_EmployeeClockInOut", model);
        }

        [Authorize]
        public PartialViewResult TimeClockPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.IsClockedIn = CurrentUser.IsClockedIn;

            return PartialView("_TimeClockPartial");
        }

        [Authorize]
        public PartialViewResult TimeClockPartialList(PayrollFilterModel filter)
        {
            TimeClockFilterModel model = new TimeClockFilterModel();

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            model = _Util.Facade.EmployeeTimeClockFacade.GetLastClocksByUserIdAndTimePeriod(CurrentUser.UserId.ToString(), Convert.ToDateTime(filter.StrStartDate).ClientToUTCTime(), filter.EndDate.SetMaxHour().ClientToUTCTime(), filter.order, filter.pageno1, filter.pagesize1);
            ViewBag.order = filter.order;
            if (model.TotalCount.CountTotal == 0)
            {
                filter.pageno1 = 1;
            }

            ViewBag.PageNumber = filter.pageno1;
            ViewBag.OutOfNumber = 0;


            if (model.ListTimeClock.Count() > 0)
            {
                ViewBag.OutOfNumber = model.TotalCount.CountTotal;
            }

            if ((int)ViewBag.PageNumber * filter.pagesize1 > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.pagesize1;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.pagesize1);
            ViewBag.pagesize = filter.pagesize1;
            var systemCheckOut = "22222222-2222-2222-2222-222222222222";
            Customer customer = new Customer();
            UserLogin user = new UserLogin();
            foreach (var item in model.ListTimeClock)
            {

                if (item.LastUpdateBy != CurrentUser.UserId)
                {
                    if (item.LastUpdateBy != new Guid(systemCheckOut))
                    {
                        user = _Util.Facade.UserLoginFacade.GetUserLoginByUserId(item.LastUpdateBy);
                        customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(item.LastUpdateBy);
                        if (customer != null)
                        {
                            item.LastUpdatedName = customer.FirstName + " " + customer.LastName;
                        }
                        else if (user != null)
                        {
                            item.LastUpdatedName = user.UserName;
                        }
                    }
                }
            }

            return PartialView("_TimeClockPartialList", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult ClockInOut(EmployeeTimeClock Model)
        {
            string responseMessage=string.Empty;
            bool Status = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Model.LastUpdateBy = CurrentUser.UserId;
            Model.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(CurrentUser.UserId);
            if (!string.IsNullOrWhiteSpace(emp.ClockInIP))
            {
                if (Request.IsLocal)
                {

                }
                else
                {
                    var IpList = emp.ClockInIP.Split(';');
                    string IPAddress = Request.UserHostAddress;
                    bool IpMatched = false;
                    foreach (var item in IpList)
                    {
                        if (item.Trim() == IPAddress)
                        {
                            IpMatched = true;
                            break;
                        }
                    }
                    if (!IpMatched)
                    {
                        responseMessage = string.Format("Your Ip {0} is not allowed for Clock In/Out please contact your supervisor.", Request.UserHostAddress);
                        Status = false;
                        return Json(new { result = Status, message = responseMessage});
                    }
                }
            }

            #region ClockIN/Out Changes
            Session[SessionKeys.CurrentLoggedInUser] = null;
            UserLogin AppUser = new Facade.UserInitializer().GetCurrentUser(User.Identity.Name, CurrentUser.CompanyId.Value);
            CustomPrincipal UserPrincipal = new CustomPrincipal(AppUser, User.Identity);
            CurrentUser = UserPrincipal;
            bool DoClockin = false;
            Model.UserId = CurrentUser.UserId;
            if (CurrentUser.IsClockedIn)
            {
                Model.Type = LabelHelper.TimeClockType.ClockOut;
                var EmployeeTimeCloctDetail = _Util.Facade.EmployeeTimeClockFacade.GetEmployeeLastTimeClockByUserId(CurrentUser.UserId);
                if (EmployeeTimeCloctDetail != null)
                {
                    EmployeeTimeCloctDetail.ClockOutLat = Model.Lat;
                    EmployeeTimeCloctDetail.ClockOutLng = Model.Lng;
                    EmployeeTimeCloctDetail.ClockOutTime = DateTime.Now.UTCCurrentTime();
                    EmployeeTimeCloctDetail.ClockOutNote = Model.Note;
                    EmployeeTimeCloctDetail.ClockOutCreatedBy = CurrentUser.UserId;
                    EmployeeTimeCloctDetail.ClockedInSeconds = (int)DateTime.Now.UTCCurrentTime().Subtract(CurrentUser.ClockInOutTime.Value).TotalSeconds;
                    
                    if (EmployeeTimeCloctDetail.ClockedInSeconds>300)
                    {
                        _Util.Facade.EmployeeTimeClockFacade.UpdateEmployeeTimeClock(EmployeeTimeCloctDetail);
                        Status = true;
                    }
                    else
                    {
                        Status = false;
                        responseMessage = "Not clocked in for sufficient time. Need to clock minimum 5 minutes to Clock Out.";
                        logger.WithProperty("tags", "hr,timeclock,clockout,issue")
                        .WithProperty("params", JsonConvert.SerializeObject(EmployeeTimeCloctDetail))
                        .Trace($"Clocked out early by {CurrentUser.GetFullName()}");
                    }

                    PushNotification(Model, CurrentUser.UserId, false);

                }
            }
            else
            {
                Model.Type = LabelHelper.TimeClockType.ClockIn;
                DoClockin = true;
                Status = true;

                Model.ClockInLat = Model.Lat;
                Model.ClockInLng = Model.Lng;
                Model.ClockInTime = DateTime.Now.UTCCurrentTime();
                Model.ClockInNote = Model.Note;
                Model.ClockInCreatedBy = CurrentUser.UserId;
                _Util.Facade.EmployeeTimeClockFacade.InsertEmployeeTimeClock(Model);

                PushNotification(Model, CurrentUser.UserId, true);
            }
            #endregion

            #region Update CurrentUser
            AppUser.ClockedInOutStatus = Model.Type;
            AppUser.ClockedInOutTime = DateTime.Now.UTCCurrentTime();
            Session[SessionKeys.CurrentLoggedInUser] = AppUser;
            #endregion

            Guid SupervisorId = new Guid();
            if ((!emp.IsSupervisor.HasValue || !emp.IsSupervisor.Value)
                && !string.IsNullOrWhiteSpace(emp.SuperVisorId)
                && Guid.TryParse(emp.SuperVisorId, out SupervisorId)
                && SupervisorId != new Guid())
            {
                #region Insert notification
                string NotificationMessage = "";
                if (!string.IsNullOrWhiteSpace(Model.Note))
                {
                    NotificationMessage = string.Format(@"{0} attached a note when {1}", "{0}", DoClockin ? "Clocking In" : "Clocking Out");
                }
                else
                {
                    NotificationMessage = string.Format(@"{0} just {1}.", "{0}", DoClockin ? "Clocked In" : "Clocked Out");
                }
                Notification notification = new Notification()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    NotificationId = Guid.NewGuid(),
                    Type = LabelHelper.NotificationType.Employee,
                    Who = CurrentUser.UserId,
                    What = NotificationMessage,
                    NotificationUrl = "/TimeClock/#EmployeesTimeClock?empid=" + emp.Id
                };
                _Util.Facade.NotificationFacade.InsertNotification(notification);
                #endregion

                #region set user to notification
                NotificationUser nu = new NotificationUser()
                {
                    NotificationId = notification.NotificationId,
                    IsRead = false,
                    NotificationPerson = SupervisorId,
                };
                _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                #endregion
            }

            if (string.IsNullOrEmpty(responseMessage))
            {
                responseMessage = string.Format("{0} successful.", Model.Type);
            }
            
            return Json(new { result = Status, message = responseMessage, Time = AppUser.ClockedInOutTime.Value.UTCToClientTime().ToString("hh:mm tt"), Type = Model.Type });
        }

        [Authorize]
        public ActionResult AddClockInOut()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            ViewBag.MapAPIKey = _Util.Facade.GlobalSettingsFacade.GetGoogleMapAPIKeyByCompanyId(CurrentUser.CompanyId.Value);
            ViewBag.IsClockedIn = CurrentUser.IsClockedIn;

            return View();
        }
        [Authorize]
        public ActionResult RejectPtoPopUp(int id)
        {
            ViewBag.ptoId = id;
            return View();
        }
        #endregion

        [Authorize]
        public PartialViewResult TimeClockDefaultFilter(bool? IsTimeClock,string from)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            #region Date Offset
            string FirstDayOfWeek = _Util.Facade.GlobalSettingsFacade.GetStartDayOfWeek(CurrentUser.CompanyId.Value);
            int DateOffset = 0;
            if (FirstDayOfWeek == "Saturday")
            {
                DateOffset = -1;
            }
            if (FirstDayOfWeek == "Monday" && (!string.IsNullOrWhiteSpace(from)) && from == "AccrualPto")
            {
                DateOffset = 0;
            }
            else if (FirstDayOfWeek == "Monday")
            {
                DateOffset = 1;
            }
            CultureInfo ci = new CultureInfo("en-US");
            #endregion

            #region Week Filter Dropdown
            //Copany start date will be retrive from global settings
            DateTime CompanyStartDate = new DateTime(2018, 7, 1);
            int Week = GetIso8601WeekOfYear(CompanyStartDate);

            int CurrentWeek = GetIso8601WeekOfYear(DateTime.Now);
            CompanyStartDate = FirstDateOfWeek(CompanyStartDate.Year, Week, ci, DateOffset);
            List<SelectListItem> WeekList = new List<SelectListItem>();
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            string PtoFilter = "-1";
            WeekList.Add(new SelectListItem()
            {
                Text = "Select One",
                Value = ""
            });
            #region CookieJobs
          
            bool fromCookie = false;
            string newCookie = "";
            if (Request.Cookies[CookieKeys.PtoFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.PtoFilter].Value))
            {
                if((!string.IsNullOrWhiteSpace(from)) && from == "AccrualPto")
                {
                    Request.Cookies[CookieKeys.PtoFilter].Value = null;
                }
                newCookie = Request.Cookies[CookieKeys.PtoFilter].Value;
                newCookie = Server.UrlDecode(newCookie);
                var CookieVals = newCookie.Split(',');
                if (CookieVals.Length == 4)
                {
                    StartDate = CookieVals[0].ToDateTime();
                    EndDate = CookieVals[1].ToDateTime();
                    string SelectedWeek = CookieVals[2];
                    if (SelectedWeek.Split('/').Length == 2)
                    {
                        int.TryParse(SelectedWeek.Split('/')[1], out CurrentWeek);
                    }
                    PtoFilter = CookieVals[3];

                    fromCookie = true;
                }
            }
            
            
            #endregion

            while (CompanyStartDate < DateTime.Now.AddDays(7))
            {
                string suffix = "th";
                if (CompanyStartDate.Day == 1 || CompanyStartDate.Day % 20 == 1 || CompanyStartDate.Day % 30 == 1)
                {
                    suffix = "st";
                }
                else if (CompanyStartDate.Day == 2 || CompanyStartDate.Day % 20 == 2)
                {
                    suffix = "nd";
                }
                else if (CompanyStartDate.Day == 3 || CompanyStartDate.Day % 20 == 3)
                {
                    suffix = "rd";
                }
                
                if (CompanyStartDate.Month == 12 && (CompanyStartDate.Day == 30 || CompanyStartDate.Day == 31))
                {
                    WeekList.Add(new SelectListItem()
                    {
                        Text = string.Format(CompanyStartDate.ToString("dd{0} MMMM yy"), suffix),
                        Value = CompanyStartDate.Year + 1 + "/" + Week,
                        Selected = Week == CurrentWeek
                    });
                }
                else
                {
                    WeekList.Add(new SelectListItem()
                    {
                        Text = string.Format(CompanyStartDate.ToString("dd{0} MMMM yy"), suffix),
                        Value = CompanyStartDate.Year + "/" + Week,
                        Selected = Week == CurrentWeek
                    });
                }
                CompanyStartDate = CompanyStartDate.AddDays(7);
                Week = GetIso8601WeekOfYear(CompanyStartDate);
            }
            if (StartDate == new DateTime() && EndDate == new DateTime())
            {
                ViewBag.EndDate = DateTime.Now;
                ViewBag.StartDate = DateTime.Now.AddDays(-7);
            }
            if((!string.IsNullOrWhiteSpace(from)) &&  from == "AccrualPto")
            {
                DateTime currenttime = DateTime.Now;
                DateTime FromDay = currenttime.AddDays(-(int)currenttime.DayOfWeek - 14).SetZeroHour();
                DateTime EndDay = FromDay.AddDays(6).SetZeroHour();
                ViewBag.EndDate = EndDay;
                ViewBag.StartDate = FromDay;
            }
            else
            {
                ViewBag.EndDate = EndDate;
                ViewBag.StartDate = StartDate;
            }
            WeekList.Add(new SelectListItem()
            {
                Text = "Custom",
                Value = ""
            });
            ViewBag.WeekList = WeekList;
            ViewBag.FirstDayOfWeek = FirstDayOfWeek;
            ViewBag.from = from;
            #endregion

            #region Common filter
            ViewBag.PTOFilterOptions = _Util.Facade.LookupFacade.GetLookupByKey("PTOFilterOptions").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString(),
                   Selected = x.DataValue == PtoFilter
               }).ToList();
            #endregion
            ViewBag.PTOStutasList = _Util.Facade.LookupFacade.GetLookupByKey("PTORequest").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            GlobalSetting gs = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsByCompanyId(CurrentUser.CompanyId.Value).Where(x => x.Tag == "UtilitySetting" && x.SearchKey == "CompanyStartDate").FirstOrDefault();
            ViewBag.FilterStartDate =gs!=null && !string.IsNullOrWhiteSpace(gs.Value)? gs.Value:"";
            ViewBag.ShowWeekList = false;
            if (IsTimeClock.HasValue && IsTimeClock.Value)
            {
                ViewBag.ShowWeekList = true;
            }
            ViewBag.PayrollFilterWeek = _Util.Facade.GlobalSettingsFacade.GetPayrollFilterWeek(CurrentUser.CompanyId.Value);
            return PartialView("_TimeClockDefaultFilter");
        }

        #region Pyroll report
        [Authorize]
        public PartialViewResult PayrollReportView()
        {
            ViewBag.EmployeeStatus = _Util.Facade.LookupFacade.GetLookupByKey("EmployeeStatus").OrderBy(x => x.DisplayText != "Employee Status").ThenBy(x => x.DisplayText).Select(x =>
                         new SelectListItem()
                         {
                             //x.IsDefaultItem?currentemp=x.IsDefaultItem.ToString():currentemp=null
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString(),
                             Selected = x.IsDefaultItem == true
                         }).ToList();
            return PartialView("_PayrollReportView");
        }

        [Authorize]
        public PartialViewResult PayrollReport(PayrollFilterModel filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (filter.StartDate == new DateTime() && filter.EndDate == new DateTime())
            {

            }

            if (!filter.PageNo.HasValue)
            {
                filter.PageNo = 1;
            }
            filter.PageSize = _Util.Facade.GlobalSettingsFacade.GetPayrollMaxLoad(CurrentUser.CompanyId.Value);

            TimeClockViewModel model = new TimeClockViewModel();
            model = _Util.Facade.TimeClockFacade.GetAllUsersInOutreportByFilter(filter);
            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = 0;


            if (model.TimeClockList.Count() > 0)
            {
                ViewBag.OutOfNumber = model.TotalCount;
            }

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize.Value);
            ViewBag.order = filter.order;
            return PartialView("_PayrollReport", model);
        }
        #endregion
        [Authorize]
        public ActionResult GetAllEmploployeePayrollReport(PayrollFilterModel filter)
        {
            EmpPayrollFilter model = new EmpPayrollFilter();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CultureInfo ci = new CultureInfo("en-US");
            //2018 / 32
            //int Year = 2018;
            //int Week = 33;


            #region Date Offset
            string FirstDayOfWeek = _Util.Facade.GlobalSettingsFacade.GetStartDayOfWeek(CurrentUser.CompanyId.Value);
            int DateOffset = 0;
            if (FirstDayOfWeek == "Saturday")
            {
                DateOffset = -1;
            }
            else if (FirstDayOfWeek == "Monday")
            {
                DateOffset = 1;
            }

            #endregion

            //if (FilterWeek.Split('/').Length == 2)
            //{
            //    int.TryParse(FilterWeek.Split('/')[0], out Year);
            //    int.TryParse(FilterWeek.Split('/')[1], out Week);
            //}
            DateTime FilterStartDate = filter.StrStartDate.ToDateTime();
            DateTime FilterEndDate = filter.StrEndDate.ToDateTime();
            //FilterStartDate = FilterStartDate;
            //FilterEndDate = FilterEndDate;
            GlobalSetting gs = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsByCompanyId(CurrentUser.CompanyId.Value).Where(x => x.Tag == "UtilitySetting" && x.SearchKey == "CompanyStartDate").FirstOrDefault();

            var fstartdate = new DateTime();
            var fenddate = new DateTime();

            if (FilterStartDate != DateTime.MinValue)
            {
                fstartdate = FilterStartDate;
            }
            if (FilterEndDate != DateTime.MinValue)
            {
                fenddate = FilterEndDate;
            }

            if (gs != null)
            {


                if (fstartdate == new DateTime() && fenddate == new DateTime())
                {
                    fstartdate = Convert.ToDateTime(gs.Value).SetZeroHour().ClientToUTCTime();

                    var todaysdate = DateTime.Now.AddYears(1);
                    var dateend = "12/31/" + todaysdate.Year.ToString() + " 00:00:00.000";
                    fenddate = Convert.ToDateTime(dateend).SetMaxHour().ClientToUTCTime();
                }
                else
                {
                    fstartdate = fstartdate.SetZeroHour().ClientToUTCTime();
                    fenddate = fenddate.SetMaxHour().ClientToUTCTime();
                }
            }


            //PayrollFilterModel payrolFilter = new PayrollFilterModel();
            //payrolFilter.StartDate = FilterStartDate;
            //payrolFilter.EndDate = FilterEndDate;
            if (!IsPermitted(UserPermissions.CustomerPermissions.ShowAllPayroll))
            {
                filter.UserId = CurrentUser.UserId;
            }
            model = _Util.Facade.TimeClockFacade.GetAllPayrollReport(CurrentUser.UserId, fstartdate, fenddate, filter.order, filter.PageNo.Value, filter.PageSize.Value, filter.UserId, filter.CurrentEmployee);
            if (model.ListEmpPayrollReport.Count > 0)
            {
                model.ListEmpPayrollReport = model.ListEmpPayrollReport.OrderBy(x => x.EmpName).ToList();
                foreach (var item in model.ListEmpPayrollReport)
                {
                    if (item.OTOHours < 0)
                    {
                        item.OTOHours = 0;
                    }
                }
            }

            ViewBag.order = filter.order; ;
            if (model.PayrollTotalCount.CountTotal == 0)
            {
                filter.PageNo = 1;
            }

            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = 0;


            if (model.ListEmpPayrollReport.Count() > 0)
            {
                ViewBag.OutOfNumber = model.PayrollTotalCount.CountTotal;
            }

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize.Value);
            ViewBag.pagesize = filter.PageSize;
            ViewBag.startdate = filter.StrStartDate;
            ViewBag.enddate = filter.StrEndDate;
            ViewBag.order = filter.order;
            return PartialView("_PayrollReport", model);
        }

        public JsonResult DeleteEmployeeTimeClock(int? id)
        {
            bool result = false;
            Guid empid = new Guid();
            if (id.HasValue && id.Value > 0)
            {
                var objemptimeclock = _Util.Facade.EmployeeTimeClockFacade.GetEmployeeTimeClockById(id.Value);
                if (objemptimeclock != null)
                {
                    empid = objemptimeclock.UserId;
                    result = _Util.Facade.EmployeeTimeClockFacade.DeleteEmployeeTimeClock(objemptimeclock.Id);
                }
            }
            return Json(new { result = result, empid = empid });
        }

        public JsonResult DeleteEmployeePto(int? id)
        {
            bool result = false;
            string message = "Pto is not deleted";
            if (id.HasValue && id.Value > 0)
            {     
                result = _Util.Facade.PtoFacade.DeletePto(id.Value);
            }
            return Json(new { result = result,message = "Deleted Successfully."});
        }
        #region Private 

        private static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci, int DateOffSet = 0)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7).AddDays(DateOffSet);
        }

        private static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        #endregion

        public ActionResult EmployeeTimeClockListPayroll(string UserId, string StrStartDate, string StrEndDate, string order, int pageno, int pagesize)
        {
            List<string> userlist = new List<string>();

            TimeClockFilterModel model = new TimeClockFilterModel();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Customer customer = new Customer();
            UserLogin user = new UserLogin();
            var systemCheckOut = "22222222-2222-2222-2222-222222222222";
            if (!string.IsNullOrWhiteSpace(UserId) && UserId != "null")
            {
                string[] splituser = UserId.Split(',');
                if (splituser.Length > 0)
                {
                    if (splituser[0] == "All")
                    {
                        splituser[0] = new Guid().ToString();
                    }
                    UserId = string.Format("{0}", string.Join("','", splituser.Select(i => i.Replace("'", "''"))));
                    foreach (var item in splituser)
                    {
                        userlist.Add(item);
                    }
                }
                GlobalSetting gs = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsByCompanyId(CurrentUser.CompanyId.Value).Where(x => x.Tag == "UtilitySetting" && x.SearchKey == "CompanyStartDate").FirstOrDefault();
                var fstartdate = new DateTime();
                var fenddate = new DateTime();
                if (!string.IsNullOrEmpty(StrStartDate))
                {
                    fstartdate = Convert.ToDateTime(StrStartDate);
                }
                if (!string.IsNullOrEmpty(StrEndDate))
                {
                    fenddate = Convert.ToDateTime(StrEndDate);
                }

                if (gs != null)
                {


                    if (fstartdate == new DateTime() && fenddate == new DateTime())
                    {
                        fstartdate = Convert.ToDateTime(gs.Value).SetZeroHour().ClientToUTCTime();

                        var todaysdate = DateTime.Now.AddYears(1);
                        var dateend = "12/31/" + todaysdate.Year.ToString() + " 00:00:00.000";
                        fenddate = Convert.ToDateTime(dateend).SetMaxHour().ClientToUTCTime();
                    }
                    else
                    {
                        fstartdate = fstartdate.SetZeroHour().ClientToUTCTime();
                        fenddate = fenddate.SetMaxHour().ClientToUTCTime();
                    }
                }
                model = _Util.Facade.EmployeeTimeClockFacade.GetLastClocksByUserIdAndTimePeriod(UserId, fstartdate, fenddate, order, pageno, pagesize);
            }
            if (model.ListTimeClock != null && model.ListTimeClock.Count > 0)
            {
                foreach (var item in model.ListTimeClock)
                {

                    if (item.LastUpdateBy != CurrentUser.UserId)
                    {
                        if (item.LastUpdateBy != new Guid(systemCheckOut))
                        {
                            user = _Util.Facade.UserLoginFacade.GetUserLoginByUserId(item.LastUpdateBy);
                            customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(item.LastUpdateBy);
                            if (customer != null)
                            {
                                item.LastUpdatedName = customer.FirstName + " " + customer.LastName;
                            }
                            //else if (user != null)
                            //{
                            //    item.LastUpdatedName = user.UserName;
                            //}
                        }
                    }
                }
            }

            ViewBag.UserId = UserId;
            var userobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(new Guid(UserId));
            if (userobj != null)
            {
                ViewBag.empid = userobj.Id;
            }
            if (model.TotalCount != null && model.TotalCount.CountTotal == 0)
            {
                pageno = 1;
            }

            ViewBag.PageNumber = pageno;
            ViewBag.OutOfNumber = 0;


            if (model.ListTimeClock != null && model.ListTimeClock.Count() > 0)
            {
                ViewBag.OutOfNumber = model.TotalCount.CountTotal;
            }

            if ((int)ViewBag.PageNumber * pagesize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * pagesize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / pagesize);
            ViewBag.pagesize = pagesize;
            ViewBag.order = order;

            return View(model);
        }
    }
}