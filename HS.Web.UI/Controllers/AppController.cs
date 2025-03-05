using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HS.Entities.AppPermission;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using HS.Web.UI.Helper;
using HS.Framework;
using HS.Entities.List;
using System.Web.Security;
using HS.Web.UI.App_Start;
using System.Net;
using HtmlAgilityPack;
using System.Data;
using System.Globalization;

namespace HS.Web.UI.Controllers
{
    public class AppController : BaseController
    {
        [Authorize]
        // GET: Home
        public ActionResult Index(string Key)
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            if (!string.IsNullOrWhiteSpace(Key))
            {
                ViewBag.Key = Key;
            }
            //if (id != null)
            //{
            //    ViewBag.id = id;
            //}

            if (Session["RunScriptOnDocReady"] != null)
            {
                ViewBag.RunScriptOnLogin = Session["RunScriptOnDocReady"].ToString();
                Session["RunScriptOnDocReady"] = null;
            }

            return View();
        }

        public ActionResult MobileSize()
        {
            return View();
        }

        public ActionResult Sync()
        {
            List<CustomerVault> vault = _Util.Facade.CustomerFacade.GetCustomerVaultList().ToList();
            Customer customer = new Customer();
            foreach (CustomerVault cv in vault)
            {
                customer = new Customer();
                customer.FirstName = cv.CustomerName;

            }
            return View();
        }

        public ActionResult Trials()
        {

            return View();
        }

        public ActionResult CICDAssist()
        {

            return View();
        }

        public JsonResult SessionChecker()
        {
            if (!User.Identity.IsAuthenticated || Session[SessionKeys.CompanyConnectionString] == null)
            {
                FormsAuthentication.SignOut();
                SessionHelper hs = new SessionHelper();
                hs.ClearCurrentSession();

                return Json(new { result = false, message = "Your session has been timed out. Please login." });
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser.UserId == Guid.Empty)
            {
                return Json(new { result = false, message = "Your session has been timed out. Please login." });
            }
            int NotificationCount = 0;
            int AnnouncementCount = 0;
            var NotificationDetails = _Util.Facade.NotificationFacade.GetNotificationCountByUserId(CurrentUser.UserId);
            if (NotificationDetails != null)
            {
                NotificationCount = NotificationDetails.NotificationCount;
                AnnouncementCount = NotificationDetails.AnnouncementCount;
            }
            bool IsClockedIn = CurrentUser.IsClockedIn;
            return Json(new { result = true, message = "ok", notificationCount = NotificationCount, AnnouncementCount= AnnouncementCount, IsClockedIn = IsClockedIn });
        }
        [Authorize]
        public ActionResult ResetpasswordPartial()
        {
            return PartialView("_Resetpassword");
        }
        [Authorize]
        public ActionResult DashBoardPartial()
        {
            UserActivity model = new UserActivity();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            if(empobj != null)
            {
                model = _Util.Facade.UserActivityFacade.GetUserActivityByLoginAction(empobj.UserName);
            }
            var DashboardReportGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ShowDashBoardReport");
            if (DashboardReportGlobal != null && DashboardReportGlobal.Value != null && DashboardReportGlobal.Value.ToLower() == "true")
            {
                ViewBag.ShowReport = true;
            }
            else
            {
                ViewBag.ShowReport = false;
            }
            return PartialView("_DashBoard", model);
        }
        [Authorize]
        public PartialViewResult EmployeeDashboard()
        {
            bool orderpermit = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            DashboardModel model = new DashboardModel();

            if (CurrentUser.UserId != new Guid())
            {
                var objemployee = _Util.Facade.EmployeeFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(CurrentUser.UserId, CurrentUser.CompanyId.Value);
                if (objemployee != null)
                {
                    model.EmployeeTag = objemployee.Tag;
                }

            }
            string AccountOwnerId = null;
            if (!string.IsNullOrWhiteSpace(model.EmployeeTag) && !model.EmployeeTag.Contains("admin"))
            {

                AccountOwnerId = CurrentUser.UserId.ToString();


            }
            if (IsPermitted(UserPermissions.MenuPermissions.LeftMenuOrders))
            {
                orderpermit = true;
            }
            model = _Util.Facade.DashboardFacade.GetDashBoardData(CurrentUser.CompanyId.Value, model.EmployeeTag, CurrentUser.UserId, null, null, null, AccountOwnerId, orderpermit);
            if (model == null)
            {
                model = new DashboardModel()
                {
                    EstimateCount = 0,
                    InvoiceCount = 0,
                    FirstMonthLeadCount = 0,
                    LastMonthLeadCount = 0,
                    FirstMonthActivitiesCount = 0,
                    LastMonthActivitiesCount = 0,
                    FirstMonthOpportunitiesCount = 0,
                    LastMonthOpportunitiesCount = 0,
                    MMRCount = 0.0,
                    TotalPaid = 0,
                    TotalRevenue = 0,
                    FirstMonthCustomerCount = 0,
                    LastMonthCustomerCount = 0,
                    EstimateAmount = 0.0,
                    InvoiceAmount = 0.0,
                    UnpaidAmount = 0.0,
                    UnpaidCount = 0,
                    TotaltTransactions = 0,
                    CountMMR = 0,
                    FirstMonthOrder = 0,
                    LastMonthOrder = 0,
                    FirstMonthRevenueCount = 0,
                    LastMonthRevenueCount = 0
                };
            }
            List<SelectListItem> StatussList = new List<SelectListItem>();
            StatussList.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("EstimatorStatus").OrderBy(x => x.DisplayText).Where(x => x.DataValue != "-1").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DisplayText.ToString()
            }).ToList());
            ViewBag.StatussList = StatussList;
            //List<SelectListItem> ReminderActiveInActiveList = new List<SelectListItem>();
            //ReminderActiveInActiveList.Add(
            //    new SelectListItem
            //    {
            //        Text = "Active",
            //        Value = "1"
            //    });
            //ReminderActiveInActiveList.Add(
            //    new SelectListItem
            //    {
            //        Text = "Inactive",
            //        Value = "0"
            //    });
            //ViewBag.ReminderActiveInActiveList = ReminderActiveInActiveList;
            return PartialView("_EmployeeDashboard", model);
        }

        [Authorize]
        public PartialViewResult CustomerDashboard()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Customer model = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CurrentUser.UserId);
            string Announcement = "";
            Guid assignuser = new Guid();
            int count = 1;
            if (model == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (!IsPermitted(UserPermissions.CustomerTicketPermission.ShowAllTicket))
            {
                assignuser = CurrentUser.UserId;
            }
            bool isorderpermit = false;
            if (IsPermitted(UserPermissions.MenuPermissions.LeftMenuOrders))
            {
                isorderpermit = true;
            }
            model.CustomerTabCounts = _Util.Facade.CustomerFacade.GetCustomerTabCountsByCustomerId(model.CustomerId, null, CurrentUser.CompanyId.Value, assignuser, isorderpermit);
            List<Announcement> ancList = _Util.Facade.CustomerFacade.GetAllAnnouncement();
            ViewBag.Announcement = "";
            foreach (var item in ancList)
            {
                if ((item.StartTime < DateTime.Now && item.EndTime > DateTime.Now) || (item.StartTime.Date == DateTime.Now.Date || item.EndTime.Date == DateTime.Now.Date))
                {

                    Announcement += "Notice" + count + ":" + item.Message + " ";
                    count++;
                }
            }
            ViewBag.Announcement = Announcement;
            ViewBag.Count = count - 1;
            List<CustomerRefer> referList = _Util.Facade.CustomerDraftFacade.GetAllReferedFriend();
            ViewBag.FriendCount = referList.Count;
            #region Customer Header Reports
            bool isDeclinedAdded = true;
            GlobalSetting isDeclined = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "IsDeclinedInvoiceAddedInUnpaidAmount");
            if (isDeclined != null)
            {
                isDeclinedAdded = !string.IsNullOrWhiteSpace(isDeclined.Value) && isDeclined.Value.ToLower() == "false" ? false : true;
            }
            EstimateStatusDetail EstimateStatusDetail = _Util.Facade.CustomerFacade.GetAllEstimateStatusDetailByCustomerId(model.CustomerId, isDeclinedAdded);

            ViewBag.DueAmountDetail = EstimateStatusDetail.DueAmountDetail;
            ViewBag.EstimateAmountDetail = EstimateStatusDetail.EstimateAmountDetail;
            ViewBag.PaidAmountDetail = EstimateStatusDetail.PaidAmountDetail;
            ViewBag.UnpaidInvoice = EstimateStatusDetail.UnpaidAmount;

            #endregion
            return PartialView("_CustomerDashboard", model);
        }

        [Authorize]
        public PartialViewResult TechnicianDashboard()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            DashboardModelTechnician model = new DashboardModelTechnician();
            model = _Util.Facade.DashboardFacade.GetDashBoardDataTechnician(CurrentUser.CompanyId.Value, CurrentUser.UserId);
            if(model != null)
            {
                model.CustomerAppointmentEquipment = _Util.Facade.DashboardFacade.GetDashBoardDataTechnicianUpsold(CurrentUser.UserId);
                model.DashboardBarModelTechnician = _Util.Facade.DashboardFacade.GetDashBoardBarDataTechnician(CurrentUser.CompanyId.Value, CurrentUser.UserId);
                if(model.DashboardBarModelTechnician != null)
                {
                    model.DashboardBarModelTechnician.AllTotalCommission = model.DashboardBarModelTechnician.TotalCommissionAC
                                                                            + model.DashboardBarModelTechnician.TotalCommissionFC
                                                                            + model.DashboardBarModelTechnician.TotalCommissionRC
                                                                            + model.DashboardBarModelTechnician.TotalCommissionSC
                                                                            + model.DashboardBarModelTechnician.TotalCommissionTC;
                }
            }
            //TicketFilter Filters = new TicketFilter();
            //Filters.CompanyId = CurrentUser.CompanyId.Value;
            //Filters.UserId = CurrentUser.UserId;
            // TicketListModel Model = _Util.Facade.DashboardFacade.GetAllTicketReportByFilter(Filters);
            //if (model == null)
            //{
            //    model = new DashboardModelTechnician();
            //}
            ViewBag.AssignTicketStatus = _Util.Facade.LookupFacade.GetLookupByKey("AssignTicketStatus").Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString(),
                    Selected=x.DataValue.ToString()== "Assigned"?true:false
                }).ToList();

            //ViewBag.TicketTypeJobList = _Util.Facade.LookupFacade.GetLookupByKey("TicketTypeJob").Select(x =>
            //        new SelectListItem()
            //        {
            //            Text = x.DisplayText.ToString(),
            //            Value = x.DataValue.ToString()
            //        }).ToList();

            List<Lookup> TicketTypeList = new List<Lookup>();
            TicketTypeList.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("TicketTypeJob"));
            TicketTypeList.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("TicketTypeTask"));


            ViewBag.TicketTypeTaskList = TicketTypeList.Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();

            //ViewBag.TicketTypeTaskList = _Util.Facade.LookupFacade.GetLookupByKey("TicketTypeTask").Select(x =>
            //        new SelectListItem()
            //        {
            //            Text = x.DisplayText.ToString(),
            //            Value = x.DataValue.ToString()
            //        }).ToList();


            //#region Cookee Jobs
            //string newCookie = "";
            //string newCookie1 = "";

            //DateTime StartDate = DateTime.Now;
            //DateTime EndDate = DateTime.Now;
            //DateTime StartDate1 = DateTime.Now;
            //DateTime EndDate1 = DateTime.Now;
            //if (Request.Cookies[CookieKeys.TechTicketFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.TechTicketFilter].Value))
            //{
            //    newCookie = Request.Cookies[CookieKeys.TechTicketFilter].Value;
            //    newCookie = Server.UrlDecode(newCookie);
            //    var CookieVals = newCookie.Split(',');
            //    if (CookieVals.Length == 2)
            //    {
            //        StartDate = CookieVals[0].ToDateTime();
            //        EndDate = CookieVals[1].ToDateTime();

            //    }
            //}
            //ViewBag.StartDate = StartDate.ToString("MM/dd/yyyy");
            //ViewBag.EndDate = EndDate.ToString("MM/dd/yyyy");

            //if (Request.Cookies[CookieKeys.TechJobTicketFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.TechJobTicketFilter].Value))
            //{
            //    newCookie1 = Request.Cookies[CookieKeys.TechJobTicketFilter].Value;
            //    newCookie1 = Server.UrlDecode(newCookie1);
            //    var CookieVals = newCookie1.Split(',');
            //    if (CookieVals.Length == 2)
            //    {
            //        StartDate1 = CookieVals[0].ToDateTime();
            //        EndDate1 = CookieVals[1].ToDateTime();

            //    }
            //}
            //ViewBag.StartDate1 = StartDate1.ToString("MM/dd/yyyy");
            //ViewBag.EndDate1 = EndDate1.ToString("MM/dd/yyyy");
            //#endregion

            ViewBag.StartDate = DateTime.Now.ToString("MM/dd/yyyy");
            ViewBag.EndDate = DateTime.Now.ToString("MM/dd/yyyy");
            return PartialView("_TechnicianDashboard", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DashboardDataWithDateFiltering(string firstdate, string lastdate, string previousfirstdate, string SearchKey)
        {
            bool orderpermit = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            DashboardModel model = new DashboardModel();

            if (CurrentUser.UserId != new Guid())
            {
                var objemployee = _Util.Facade.EmployeeFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(CurrentUser.UserId, CurrentUser.CompanyId.Value);
                if (objemployee != null)
                {
                    model.EmployeeTag = objemployee.Tag;
                }
            }
            string AccountOwnerId = null;
            if (model!=null && model.EmployeeTag!=null && !model.EmployeeTag.Contains("admin"))
            {

                AccountOwnerId = CurrentUser.UserId.ToString();


            }
            if (IsPermitted(UserPermissions.MenuPermissions.LeftMenuOrders))
            {
                orderpermit = true;
            }
            model = _Util.Facade.DashboardFacade.GetDashBoardData(CurrentUser.CompanyId.Value, model.EmployeeTag, CurrentUser.UserId, firstdate, lastdate, previousfirstdate, AccountOwnerId, orderpermit);
            if (model == null)
            {
                model = new DashboardModel();
                model.EstimateCount = 0;
                model.InvoiceCount = 0;
                model.FirstMonthLeadCount = 0;
                model.LastMonthLeadCount = 0;
                model.MMRCount = 0.0;
                model.TotalPaid = 0;
                model.TotalRevenue = 0;
                model.FirstMonthCustomerCount = 0;
                model.LastMonthCustomerCount = 0;
                model.EstimateAmount = 0.0;
                model.InvoiceAmount = 0.0;
                model.UnpaidAmount = 0.0;
                model.UnpaidCount = 0;
                model.TotaltTransactions = 0;
                model.CountMMR = 0;
                model.FirstMonthActivitiesCount = 0;
                model.LastMonthActivitiesCount = 0;

                model.FirstMonthOpportunitiesCount = 0;
                model.LastMonthOpportunitiesCount = 0;
                model.FirstMonthOrder = 0;
                model.LastMonthOrder = 0;
                model.FirstMonthRevenueCount = 0;
                model.FirstMonthRevenueCount = 0;
            }
            //return Json(new
            //{
            //    result = true,
            //    dailylead = LabelHelper.FormatCount(model.FirstMonthLeadCount),
            //    lastlead = LabelHelper.FormatCount(model.LastMonthLeadCount),
            //    SearchKey = SearchKey,
            //    firstdate = firstdate,
            //    lastdate = lastdate,
            //    firstcustomer = LabelHelper.FormatCount(model.FirstMonthCustomerCount),
            //    lastcustomer = LabelHelper.FormatCount(model.LastMonthCustomerCount),
            //    thisestcount = LabelHelper.FormatCount(model.EstimateCount),
            //    thisestamount = string.Format("{0:0,0}", model.EstimateAmount),
            //    thisinvcount = LabelHelper.FormatCount(model.InvoiceCount),
            //    thisinvamount = string.Format("{0:0,0}", model.InvoiceAmount),
            //    thisrmramount = string.Format("{0:0,0}", model.MMRCount),
            //    thisrmrcount = LabelHelper.FormatCount(model.CountMMR),
            //    FirstMonthActivitiesCount = LabelHelper.FormatCount(model.FirstMonthActivitiesCount),
            //    LastMonthActivitiesCount = LabelHelper.FormatCount(model.LastMonthActivitiesCount),
            //    FirstMonthOpportunitiesCount = LabelHelper.FormatCount(model.FirstMonthOpportunitiesCount),
            //    LastMonthOpportunitiesCount = LabelHelper.FormatCount(model.LastMonthOpportunitiesCount),
            //    firstmonthorder = LabelHelper.FormatCount(model.FirstMonthOrder),
            //    lastmonthorder = LabelHelper.FormatCount(model.LastMonthOrder),
            //    firstmonthrevenue = LabelHelper.FormatAmount(model.FirstMonthRevenueCount),
            //    lastmonthrevenue = LabelHelper.FormatAmount(model.LastMonthRevenueCount)
            //});
            return Json(new
            {
                result = true,
                dailylead = LabelHelper.FormatCount(model.FirstMonthLeadCount),
                lastlead = LabelHelper.FormatCount(model.LastMonthLeadCount),
                SearchKey = SearchKey,
                firstdate = firstdate,
                lastdate = lastdate,
                firstcustomer = LabelHelper.FormatCount(model.FirstMonthCustomerCount),
                lastcustomer = LabelHelper.FormatCount(model.LastMonthCustomerCount),
                thisestcount = LabelHelper.FormatCount(model.EstimateCount),
                thisestamount = string.Format("{0:0,0}", model.EstimateAmount),
                thisinvcount = LabelHelper.FormatCount(model.InvoiceCount),
                thisinvamount = string.Format("{0:0,0}", model.InvoiceAmount),
                thisrmramount = string.Format("{0:0,0}", model.MMRCount),
                thisrmrcount = LabelHelper.FormatCount(model.CountMMR),
                FirstMonthActivitiesCount = LabelHelper.FormatCount(model.FirstMonthActivitiesCount),
                LastMonthActivitiesCount = LabelHelper.FormatCount(model.LastMonthActivitiesCount),
                FirstMonthOpportunitiesCount = LabelHelper.FormatCount(model.FirstMonthOpportunitiesCount),
                LastMonthOpportunitiesCount = LabelHelper.FormatCount(model.LastMonthOpportunitiesCount),
                firstmonthorder = LabelHelper.FormatCount(model.FirstMonthOrder),
                lastmonthorder = LabelHelper.FormatCount(model.LastMonthOrder),
                firstmonthrevenue = LabelHelper.FormatAmount(model.FirstMonthRevenueCount),
                lastmonthrevenue = LabelHelper.FormatAmount(model.LastMonthRevenueCount)
            });
        }

        [Authorize]
        [HttpPost]
        public JsonResult TechnicianDashboardDataWithDateFiltering(string firstdate, string lastdate, string previousfirstdate, string SearchKey)
        {
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            DashboardModelTechnician model = new DashboardModelTechnician();
            model = _Util.Facade.DashboardFacade.GetDashBoardDataTechnicianByDate(CurrentUser.CompanyId.Value, CurrentUser.UserId, firstdate, lastdate);

            if(model != null)
            {
                model.CustomerAppointmentEquipment = _Util.Facade.DashboardFacade.GetDashBoardDataTechnicianUpsoldByDate(CurrentUser.UserId, firstdate, lastdate);
            }
            
           
            if (model == null)
            {
                model = new DashboardModelTechnician();
                model.OpenInstallationTicket = 0;
                model.OpenServiceTicket = 0;
                model.ClosedInstallationTicket = 0;
                model.ClosedServiceTicket = 0;
                model.CustomerAppointmentEquipment.TotalUpsold = 0;
                model.CustomerAppointmentEquipment.UpsoldServices = 0;
                model.CustomerAppointmentEquipment.UpsoldEquipments= 0;
                model.CustomerAppointmentEquipment.UpsoldServicesTotalPrice = 0;
                model.CustomerAppointmentEquipment.UpsoldEquipmentsTotalPrice = 0;
            }
            int TotalOpenTicket = LabelHelper.FormatCount(model.OpenInstallationTicket).ToInt() + LabelHelper.FormatCount(model.OpenServiceTicket).ToInt();
            int TotalClosedTicket = LabelHelper.FormatCount(model.ClosedInstallationTicket).ToInt() + LabelHelper.FormatCount(model.ClosedServiceTicket).ToInt();
            return Json(new
            {
                result = true,
                TotalOpenInstallationTicket = TotalOpenTicket,
                OpenInstallationTicket = LabelHelper.FormatCount(model.OpenInstallationTicket),
                OpenServiceTicket = LabelHelper.FormatCount(model.OpenServiceTicket),
                SearchKey = SearchKey,
                firstdate = firstdate,
                lastdate = lastdate,
                TotalClosedInstallationTicket = TotalClosedTicket,
                ClosedInstallationTicket = LabelHelper.FormatCount(model.ClosedInstallationTicket),
                ClosedServiceTicket = LabelHelper.FormatCount(model.ClosedServiceTicket),
                //thisestcount = LabelHelper.FormatCount(model.EstimateCount),
                //thisestamount = string.Format("{0:0,0}", model.EstimateAmount),
                //thisinvcount = LabelHelper.FormatCount(model.InvoiceCount),
                //thisinvamount = string.Format("{0:0,0}", model.InvoiceAmount),
                //thisrmramount = string.Format("{0:0,0}", model.MMRCount),
                //thisrmrcount = LabelHelper.FormatCount(model.CountMMR),
                TotalUpsold = LabelHelper.FormatCount(model.CustomerAppointmentEquipment.TotalUpsold),
                UpsoldServices = LabelHelper.FormatCount(model.CustomerAppointmentEquipment.UpsoldServices),
                UpsoldEquipments = LabelHelper.FormatCount(model.CustomerAppointmentEquipment.UpsoldEquipments),
                UpsoldServicesTotalPrice = LabelHelper.FormatCount(model.CustomerAppointmentEquipment.UpsoldServicesTotalPrice),
                UpsoldEquipmentsTotalPrice = LabelHelper.FormatCount(model.CustomerAppointmentEquipment.UpsoldEquipmentsTotalPrice),
                TotalUpsoldTotalPrice = LabelHelper.FormatCount(model.CustomerAppointmentEquipment.UpsoldEquipmentsTotalPrice + model.CustomerAppointmentEquipment.UpsoldServicesTotalPrice)//,
                //LastMonthOpportunitiesCount = LabelHelper.FormatCount(model.LastMonthOpportunitiesCount)
            });
        }



        [Authorize]
        public PartialViewResult DashBoardCustomerLeadGraph(DateTime StartDateTime, DateTime EndDateTime, string keyvalue, string labelvalue)
        {
            /*
             keyvalue = "yearly","monthly","today","weekly"

            labelvalue = "lead","customer","mmr","invest"

             */
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<DashboardSalesAreaChart> model = new List<DashboardSalesAreaChart>();
            model = _Util.Facade.InvoiceFacade.GetDashboardSalesAreaChartData(CurrentUser.CompanyId.Value, StartDateTime, EndDateTime, labelvalue, CurrentUser.UserTags, CurrentUser.UserId);

            if (model.Count > 0 && model != null)
            {
                if (labelvalue == "invest")
                {
                    string result = "[";
                    string temp = "\"Total Sale Amount\":{0}, \"Sale Date\":\"{1}\",\"Sale Quantity\":{2} ";
                    foreach (var item in model)
                    {
                        string Amount = " " + item.TotalSaleAmount.ToString();
                        result += "{";
                        result += string.Format(temp, item.TotalSaleAmount, item.SaleDate.ToString("MM-dd-yy"), item.SaleQuantity);
                        result += "},";
                    }
                    result = result.Remove(result.Length - 1);
                    result += "]";
                    ViewBag.DBLeadGraph = result;
                }
                else
                {
                    string result = "[";
                    string temp = "\"Total Sale Amount\":{0}, \"Join Date\":\"{1}\",\"Count\":{2} ";
                    foreach (var item in model)
                    {
                        string Amount = " " + item.TotalSaleAmount.ToString();
                        result += "{";
                        result += string.Format(temp, item.TotalSaleAmount, item.SaleDate.ToString("MM-dd-yy"), item.SaleQuantity);
                        result += "},";
                    }
                    result = result.Remove(result.Length - 1);
                    result += "]";
                    ViewBag.DBLeadGraph = result;
                }
            }

            ViewBag.keyvalue = keyvalue;
            ViewBag.labelvalue = labelvalue;
            return PartialView("_DashBoardCustomerLeadGraph", model);
        }

        [Authorize]
        public PartialViewResult DashBoardFollowupReminder()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string EmployeeTag = "";
            Guid empid = new Guid("00000000-0000-0000-0000-000000000000");
            if (CurrentUser.CompanyId.Value != new Guid())
            {
                var objemployee = _Util.Facade.EmployeeFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(CurrentUser.UserId, CurrentUser.CompanyId.Value);
                if (objemployee != null)
                {
                    EmployeeTag = objemployee.Tag;
                    empid = CurrentUser.UserId;
                }
            }
            List<DashBoardReminderFollowUpsModel> model = _Util.Facade.DashboardFacade.GetDashBoardReminderFollowUpsData(CurrentUser.CompanyId.Value, EmployeeTag, empid);
            foreach (var item in model)
            {
                if (item.Note != "" && item.Note != null)
                {
                    item.Note = WebUtility.HtmlDecode(item.Note);
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(item.Note);
                    item.Note = htmlDoc.DocumentNode.InnerText;
                    item.ReminderDate = item.ReminderDate.AddHours(6);
                }
            }
            return PartialView("_DashBoardFollowupReminder", model);
        }

        [Authorize]
        public PartialViewResult DashBoardCurrentUserFollowupReminder(string SelectedActiveInactive)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //string EmployeeTag = "";
            Guid empid = new Guid("00000000-0000-0000-0000-000000000000");
            if (CurrentUser.CompanyId.Value != new Guid())
            {
                var objemployee = _Util.Facade.EmployeeFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(CurrentUser.UserId, CurrentUser.CompanyId.Value);
                if (objemployee != null)
                {
                    //EmployeeTag = objemployee.Tag;
                    empid = CurrentUser.UserId;
                }
            }
            List<string> selectreminder = new List<string>();
            if (!string.IsNullOrWhiteSpace(SelectedActiveInactive))
            {
                string[] splituser = SelectedActiveInactive.Split(',');
                if (splituser.Length > 0)
                {
                    SelectedActiveInactive = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectreminder.Add(item);
                    }
                }
            }
            if (selectreminder == null)
            {
                selectreminder.Add("1");
            }
            List<DashBoardReminderFollowUpsModel> model = _Util.Facade.DashboardFacade.GetDashBoardCurrentUserReminderFollowUpsData(CurrentUser.CompanyId.Value, empid, SelectedActiveInactive);
            foreach (var item in model)
            {
                if (item.Note != "" && item.Note != null)
                {
                    item.Note = WebUtility.HtmlDecode(item.Note);
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(item.Note);
                    item.Note = htmlDoc.DocumentNode.InnerText;
                    item.ReminderDate = item.ReminderDate.UTCToClientTime();
                    item.ReminderEndDate = item.ReminderEndDate.UTCToClientTime();
                }
            }
            
            List<SelectListItem> ReminderActiveInActiveList = new List<SelectListItem>();
            ReminderActiveInActiveList.Add(
                new SelectListItem
                {
                    Text = "Active",
                    Value = "1"
                });
            ReminderActiveInActiveList.Add(
                new SelectListItem
                {
                    Text = "Inactive",
                    Value = "0"
                });
            ViewBag.ReminderActiveInActiveList = ReminderActiveInActiveList;
            ViewBag.selectreminder = selectreminder;
            ViewBag.totaluserreminder = model.Count();
            return PartialView("_DashBoardCurrentUserFollowupReminder", model);
        }
        [Authorize]
        public ActionResult DashBoardAssignedTicket(AssignTicketFilter filter )
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            filter.UserId = CurrentUser.UserId;
            List<Lookup> TaskLookupList = _Util.Facade.LookupFacade.GetLookupByKey("TicketTypeTask").ToList();
            List<Lookup> JobLookupList = _Util.Facade.LookupFacade.GetLookupByKey("TicketTypeJob").ToList();
            filter.TaskLookupList = TaskLookupList;
            filter.JobLookupList = JobLookupList;
            List<AssignTicket> ticket = _Util.Facade.TicketFacade.GetAllAssignedTicketByUserId(filter);
            return View(ticket);
        }
        [Authorize]
        public ActionResult DashBoardAllTicket(AssignTicketFilter filter,int pageno,int pagesize)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            filter.UserId = CurrentUser.UserId;
            List<Lookup> TaskLookupList = _Util.Facade.LookupFacade.GetLookupByKey("TicketTypeTask").ToList();
            List<Lookup> JobLookupList = _Util.Facade.LookupFacade.GetLookupByKey("TicketTypeJob").ToList();
            filter.TaskLookupList = TaskLookupList;
            filter.JobLookupList = JobLookupList;
            ViewBag.ScheduleMinDate = filter.ScheduleMinDate;
            ViewBag.ScheduleMaxDate = filter.ScheduleMaxDate;
            if (pageno == 0)
            {
                pageno = 1;
            }
            AssignAllTicket ticket = _Util.Facade.TicketFacade.GetAllTicketByUserId(filter, pageno, pagesize);
            ViewBag.TotalEstimator = ticket.TotalCount;

            ViewBag.PageNumber = pageno;
            ViewBag.OutOfNumber = 0;
          
            if (ticket.AssignTicketList.Count > 0)
            {
                ViewBag.OutOfNumber = ticket.TotalCount;
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
            return View(ticket);
        }
        [Authorize]
        public ActionResult DashBoardEstimator(EstimatorFilter filter, int pageno, int pagesize,string status,string overxprice,string startdate,string enddate)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            filter.UserId = CurrentUser.UserId;
            EstimatorDashboard estimators = _Util.Facade.EstimatorFacade.GetAllEstimatorListForDashboard(CurrentUser.CompanyId.Value,filter,pageno,pagesize,status,overxprice,startdate,enddate);
            ViewBag.startdate = startdate;
            ViewBag.enddate = enddate;
            ViewBag.Price = overxprice;
            List<SelectListItem> StatussList = new List<SelectListItem>();
            StatussList.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("EstimatorStatus").OrderBy(x => x.DisplayText).Where(x => x.DataValue != "-1").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DisplayText.ToString()
            }).ToList());
            ViewBag.StatussList = StatussList;

            ViewBag.TotalEstimator = estimators.TotalCount;
             
            ViewBag.PageNumber = pageno;
            ViewBag.OutOfNumber = 0;

            if (estimators.EstimatorList.Count > 0)
            {
                ViewBag.OutOfNumber = estimators.TotalCount;
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

            return View(estimators);
        }
        [Authorize]
        public ActionResult EmployeeBirthdayList()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            DateTime StartDate = DateTime.Now.UTCCurrentTime().AddDays(-1).SetZeroHour();
            DateTime EndDate = DateTime.Now.UTCCurrentTime().AddDays(7).SetMaxHour();
            List<Employee> employee = _Util.Facade.EmployeeFacade.GetAllEmployeeByBirthDate(CurrentUser.CompanyId.Value, StartDate, EndDate).OrderBy(x => x.DOB).ToList();
            return View(employee);
        }
        [Authorize]
        public ActionResult EmployeeAnniversaryList()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            DateTime StartDate = DateTime.Now.UTCCurrentTime().AddDays(-1).SetZeroHour();
            DateTime EndDate = DateTime.Now.UTCCurrentTime().AddDays(7).SetMaxHour();
            List<Employee> employee = _Util.Facade.EmployeeFacade.GetAllEmployeeByAnniversaryDate(CurrentUser.CompanyId.Value, StartDate, EndDate).OrderBy(x => x.AnniversaryDate).ToList();
            return View(employee);
        }

        //public PartialViewResult LeadsPartial()
        //{
        //    return PartialView("_Leads");
        //}
        public PartialViewResult SalesPartial()
        {
            return PartialView("_Sales");
        }
        public PartialViewResult EmployeePartial()
        {
            return PartialView("_Employee");
        }
        public PartialViewResult AdmintrationPartial()
        {
            return PartialView("_Admintration");
        }
        //public PartialViewResult CustomerPartial()
        //{
        //    return PartialView("_Customer");
        //}
        public PartialViewResult BillPartial()
        {
            return PartialView("_Bill");
        }
        public PartialViewResult CheckPartial()
        {
            return PartialView("_Check");
        }
        public PartialViewResult ReportPartial()
        {
            return PartialView("_Report");
        }
        public PartialViewResult BankingPartial()
        {
            return PartialView("_Banking");
        }
        public PartialViewResult CompanyPartial()
        {
            return PartialView("_Company");
        }
        public PartialViewResult ThirdPartyApiPartial()
        {
            return PartialView("_ThirdPartyApi");
        }
        public PartialViewResult ReportSalesPartial()
        {
            return PartialView("_ReportSales");
        }
        public PartialViewResult ReportCustomerPartial()
        {
            return PartialView("_ReportCustomer");
        }
        public PartialViewResult ReportSchedulePartial()
        {
            return PartialView("_ReportSchedule");
        }
        public PartialViewResult ReportEmployeePartial()
        {
            return PartialView("_ReportEmployee");
        }
        public PartialViewResult VendorCreditPartial()
        {
            return PartialView("_VendorCredit");
        }
        public PartialViewResult ExpencePartial()
        {
            return PartialView("_Expence");
        }
        public PartialViewResult PayrollPartial()
        {
            return PartialView("_Payroll");
        }
        public PartialViewResult TimeAttendancePartial()
        {
            return PartialView("_TimeAttendance");
        }
        public PartialViewResult InventoryPartial(string Id)
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.LeftMenuInventory))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var BranchList = _Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(CurrentUser.CompanyId.Value);
            ViewBag.BranchCount = BranchList.Count;
            if (!string.IsNullOrWhiteSpace(Id))
            {
                ViewBag.Id = Id;
            }
            return PartialView("_Inventory");
        }
        public PartialViewResult EmployeeSchedulePartial()
        {
            return PartialView("_EmployeeSchedule");
        }
        public PartialViewResult VendorPartial()
        {
            return PartialView("_Vendor");
        }
        public PartialViewResult SchedulePartial()
        {
            return PartialView("_Schedule");
        }

        public PartialViewResult AnalyticsPartial()
        {
            return PartialView("_Analytics");
        }
        [Authorize]
        public PartialViewResult ManufacturersPartial()
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.QuickMenuProductsManufacturers))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            //List<Manufacturer> manufacturers =_Util.Facade.ManufacturerFacade.GetAllManufacturers();
            return PartialView("_Manufacturers");
        }
        [Authorize]
        public PartialViewResult ManufaturersListPartial()
        {

            if (!base.IsPermitted(UserPermissions.MenuPermissions.QuickMenuProductsManufacturers))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<Manufacturer> manufacturers = _Util.Facade.ManufacturerFacade.GetAllManufacturers();
            //ManufacturerModelWithPaging manufacturer = new ManufacturerModelWithPaging();
            //manufacturer = _Util.Facade.ManufacturerFacade.GetAllManufacturer(pageno,pagesize);
            //ViewBag.TotalLeads = manufacturer.TotalCount;

         


            //ViewBag.PageNumber = pageno;
            //ViewBag.OutOfNumber = 0;

            //if (manufacturer.Manufacturerlist.Count > 0)
            //{
            //    ViewBag.OutOfNumber = manufacturer.TotalCount;
            //}

            //if ((int)ViewBag.PageNumber * pagesize > (int)ViewBag.OutOfNumber)
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            //}
            //else
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.PageNumber * pagesize;
            //}
            //ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / pagesize);

            return PartialView("_ManufaturersList", manufacturers);
        }

    
        public PartialViewResult MarchentsPartial()
        {
            List<Marchant> Marchants = _Util.Facade.MarchantFacade.GetAllMarchants();
            return PartialView("_Marchents", Marchants);
        }
        [Authorize]
        public PartialViewResult AddManufacturer(int? id)
        {
            Manufacturer model;
            if (id.HasValue)
            {
                if (!base.IsPermitted(UserPermissions.ProductsPermissions.ManufacturersEdit))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }

                model = _Util.Facade.ManufacturerFacade.GetById(id.Value);
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.ProductsPermissions.ManufacturersAdd))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model = new Manufacturer();
            }
            return PartialView("_AddManufacturer", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteManufacturer(int Id)
        {
            if (!base.IsPermitted(UserPermissions.ProductsPermissions.ManufacturersDelete))
            {
                return Json(new { result = false, message = "Permission denied" });
            }
            Manufacturer manu = _Util.Facade.ManufacturerFacade.GetById(Id);
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (manu.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false });
            }
            _Util.Facade.ManufacturerFacade.DeleteManufatcurerById(Id);

            return Json(new { result = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddManufacturer(Manufacturer manu)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(new { result=false,message =""});
            }
            manu.IsActive = true;
            if (manu.Id > 0)
            {
                if (!base.IsPermitted(UserPermissions.ProductsPermissions.ManufacturersEdit))
                {
                    return Json(false);
                }
                if (manu.ManufacturerId == Guid.Empty)
                {
                    var tempManu = _Util.Facade.ManufacturerFacade.GetById(manu.Id);
                    manu.ManufacturerId = tempManu.ManufacturerId;
                }

                manu.CompanyId = currentLoggedIn.CompanyId.Value;
                _Util.Facade.ManufacturerFacade.UpdateManufacturer(manu);
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.ProductsPermissions.ManufacturersAdd))
                {
                    return Json(new { result = false, message = "Access denied" });
                }
                manu.CompanyId = currentLoggedIn.CompanyId.Value;
                manu.ManufacturerId = Guid.NewGuid();
                _Util.Facade.ManufacturerFacade.InsertManufacturer(manu);
            }

            return Json(new { result = true, message = "Successful.", ManufacturerId= manu.ManufacturerId, ManufacturerName = manu.Name });
        }
        [Authorize]
        public PartialViewResult AddMarchant(int? id)
        {
            Marchant model;
            if (id.HasValue)
            {
                model = _Util.Facade.MarchantFacade.GetById(id.Value);
            }
            else
            {
                model = new Marchant();
            }

            return PartialView("_AddMarchant", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddMarchant(Marchant manu)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(false);
            }
            manu.IsActive = true;
            if (manu.Id > 0)
            {
                manu.CompanyId = currentLoggedIn.CompanyId.Value;
                _Util.Facade.MarchantFacade.UpdateMarchant(manu);
            }
            else
            {
                manu.CompanyId = currentLoggedIn.CompanyId.Value;
                _Util.Facade.MarchantFacade.InsertMarchant(manu);
            }
            return Json(true);
        }

        [Authorize]
        public JsonResult GetGlobalSearchByKey(string key, string type)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //if (!string.IsNullOrWhiteSpace(key))
            //{
            //    key = key.Replace("'", "`");
            //}
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                bool ContactPermission = IsPermitted(UserPermissions.MenuPermissions.LeftMenuContacts);
                bool OpportunityPermission = IsPermitted(UserPermissions.MenuPermissions.LeftMenuOpportunities);
                string Currency = LabelHelper.CurrentTransMakeCurrency.MakeCurrency();
                List<GlobalSearchModel> SearchList = _Util.Facade.CustomerFacade.GetGlobalSearchByKeyAndCompanyId(key, CurrentUser.CompanyId.Value, CurrentUser.UserTags,CurrentUser.UserRole, CurrentUser.UserId, Currency, ContactPermission, OpportunityPermission);
                if (SearchList.Count > 0)
                    result = JsonConvert.SerializeObject(SearchList);
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public PartialViewResult GetGlobalSearchResultAll(string key, string SearchFor, string type)
        {
            //if (!string.IsNullOrWhiteSpace(key))
            //{
            //    key = key.Replace("'", "`");
            //}
            //key = HttpUtility.HtmlDecode(key);

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var ispermit = IsPermitted(UserPermissions.CustomerPermissions.ShowAllCustomerList);
            var ispermitLead = IsPermitted(UserPermissions.CustomerPermissions.ShowAllLeadList);
            string EmployeeTag = "";
            Guid empid = Guid.Empty;
            if (CurrentUser.CompanyId.Value != new Guid())
            {
                var objemployee = _Util.Facade.EmployeeFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(CurrentUser.UserId, CurrentUser.CompanyId.Value);
                if (objemployee != null)
                {
                    EmployeeTag = objemployee.Tag;

                    empid = CurrentUser.UserId;
                }
            }
            GlobalSearchViewModel model = new GlobalSearchViewModel();
            if (string.IsNullOrWhiteSpace(SearchFor))
            {
                //_Util.Facade.CustomerFacade.GetCustomerByLiteFilter
                //model.Customers = _Util.Facade.CustomerFacade.GetCustomersByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag,CurrentUser.UserRole, empid, ispermit);
                // _Util.Facade.CustomerFacade.GetLeadByLiteFilter
                //model.Leads = _Util.Facade.CustomerFacade.GetLeadsByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag,CurrentUser.UserRole, empid,ispermitLead);
                model.Invoices = _Util.Facade.InvoiceFacade.GetInvoiceByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag, empid);
                model.Tickets = _Util.Facade.TicketFacade.GetTicketByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag, empid);
                int invCount = model.Invoices.Count();
                for (int i = 0; i < invCount; i++)
                {
                    model.Invoices[i].CreatedDate = model.Invoices[i].CreatedDate.UTCToClientTime();
                }

                if (IsPermitted(UserPermissions.MenuPermissions.LeftMenuContacts))
                {
                    model.Contacts = _Util.Facade.ContactFacade.GetContactsBySearchKey(key, EmployeeTag, empid, type);
                }
                if (IsPermitted(UserPermissions.MenuPermissions.LeftMenuOpportunities))
                {
                    model.Opportunities = _Util.Facade.OpportunityFacade.GetOpportunitiesBySearchKey(key, EmployeeTag, empid);
                }

                model.Estimates = _Util.Facade.InvoiceFacade.GetEstimateByCompnayIdAndKey(CurrentUser.CompanyId.Value, CurrentUser.UserId, key, EmployeeTag, empid);
            }
            else
            {
                if (SearchFor == "Customer")
                {
                    model.Customers = _Util.Facade.CustomerFacade.GetCustomersByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag, CurrentUser.UserRole, empid, ispermit);
                }
                else if (SearchFor == "Leads")
                {
                    model.Leads = _Util.Facade.CustomerFacade.GetLeadsByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag, CurrentUser.UserRole, empid, ispermitLead);

                }
                else if (SearchFor == "Invoice")
                {
                    model.Invoices = _Util.Facade.InvoiceFacade.GetInvoiceByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag, empid);
                    int invCount = model.Invoices.Count();
                    for (int i = 0; i < invCount; i++)
                    {
                        model.Invoices[i].CreatedDate = model.Invoices[i].CreatedDate.UTCToClientTime();
                    }
                }
                else if (SearchFor == "Estimate")
                {
                    model.Estimates = _Util.Facade.InvoiceFacade.GetEstimateByCompnayIdAndKey(CurrentUser.CompanyId.Value, CurrentUser.UserId, key, EmployeeTag, empid);
                }
                else if (SearchFor == "Contact" && IsPermitted(UserPermissions.MenuPermissions.LeftMenuContacts))
                {
                    model.Contacts = _Util.Facade.ContactFacade.GetContactsBySearchKey(key, EmployeeTag, empid, type);
                }
                else if (SearchFor == "Opportunity" && IsPermitted(UserPermissions.MenuPermissions.LeftMenuOpportunities))
                {
                    model.Opportunities = _Util.Facade.OpportunityFacade.GetOpportunitiesBySearchKey(key, EmployeeTag, empid);
                }
                else if (SearchFor == "Ticket")
                {
                    model.Tickets = _Util.Facade.TicketFacade.GetTicketByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag, empid);
                }
            }
            ViewBag.SearchText = key;
            return PartialView("_GetGlobalSearchResultAll",model);
        }

        [Authorize]
        public PartialViewResult GlobalSearchResult(string key, string SearchFor, string type)
        {
            key = HttpUtility.HtmlDecode(key);

            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //var ispermit = IsPermitted(UserPermissions.CustomerPermissions.ShowAllCustomerList);
            //var ispermitLead = IsPermitted(UserPermissions.CustomerPermissions.ShowAllLeadList);
            //string EmployeeTag = "";
            //Guid empid = Guid.Empty;
            //if (CurrentUser.CompanyId.Value != new Guid())
            //{
            //    var objemployee = _Util.Facade.EmployeeFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(CurrentUser.UserId, CurrentUser.CompanyId.Value);
            //    if (objemployee != null)
            //    {
            //        EmployeeTag = objemployee.Tag;
                    
            //        empid = CurrentUser.UserId;
            //    }
            //}
            GlobalSearchViewModel model = new GlobalSearchViewModel();
            //if (string.IsNullOrWhiteSpace(SearchFor))
            //{
            //     //_Util.Facade.CustomerFacade.GetCustomerByLiteFilter
            //    //model.Customers = _Util.Facade.CustomerFacade.GetCustomersByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag,CurrentUser.UserRole, empid, ispermit);
            //    // _Util.Facade.CustomerFacade.GetLeadByLiteFilter
            //    //model.Leads = _Util.Facade.CustomerFacade.GetLeadsByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag,CurrentUser.UserRole, empid,ispermitLead);
            //    model.Invoices = _Util.Facade.InvoiceFacade.GetInvoiceByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag, empid);
            //    model.Tickets = _Util.Facade.TicketFacade.GetTicketByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag, empid);
            //    int invCount = model.Invoices.Count();
            //    for (int i = 0; i < invCount; i++)
            //    {
            //        model.Invoices[i].CreatedDate = model.Invoices[i].CreatedDate.UTCToClientTime();
            //    }

            //    if (IsPermitted(UserPermissions.MenuPermissions.LeftMenuContacts))
            //    {
            //        model.Contacts = _Util.Facade.ContactFacade.GetContactsBySearchKey(key, EmployeeTag, empid, type);
            //    }
            //    if (IsPermitted(UserPermissions.MenuPermissions.LeftMenuOpportunities))
            //    {
            //        model.Opportunities = _Util.Facade.OpportunityFacade.GetOpportunitiesBySearchKey(key, EmployeeTag, empid);
            //    }

            //    model.Estimates = _Util.Facade.InvoiceFacade.GetEstimateByCompnayIdAndKey(CurrentUser.CompanyId.Value, CurrentUser.UserId, key, EmployeeTag, empid);
            //}
            //else
            //{
            //    if (SearchFor == "Customer")
            //    {
            //        model.Customers = _Util.Facade.CustomerFacade.GetCustomersByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag,CurrentUser.UserRole, empid, ispermit);
            //    }
            //    else if (SearchFor == "Leads")
            //    {
            //        model.Leads = _Util.Facade.CustomerFacade.GetLeadsByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag,CurrentUser.UserRole, empid, ispermitLead);

            //    }
            //    else if (SearchFor == "Invoice")
            //    {
            //        model.Invoices = _Util.Facade.InvoiceFacade.GetInvoiceByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag, empid);
            //        int invCount = model.Invoices.Count();
            //        for (int i = 0; i < invCount; i++)
            //        {
            //            model.Invoices[i].CreatedDate = model.Invoices[i].CreatedDate.UTCToClientTime();
            //        }
            //    }
            //    else if (SearchFor == "Estimate")
            //    {
            //        model.Estimates = _Util.Facade.InvoiceFacade.GetEstimateByCompnayIdAndKey(CurrentUser.CompanyId.Value, CurrentUser.UserId, key, EmployeeTag, empid);
            //    }
            //    else if (SearchFor == "Contact" && IsPermitted(UserPermissions.MenuPermissions.LeftMenuContacts))
            //    {
            //        model.Contacts = _Util.Facade.ContactFacade.GetContactsBySearchKey(key, EmployeeTag, empid, type);
            //    }
            //    else if (SearchFor == "Opportunity" && IsPermitted(UserPermissions.MenuPermissions.LeftMenuOpportunities))
            //    {
            //        model.Opportunities = _Util.Facade.OpportunityFacade.GetOpportunitiesBySearchKey(key, EmployeeTag, empid);
            //    }
            //    else if (SearchFor == "Ticket")
            //    {
            //        model.Tickets = _Util.Facade.TicketFacade.GetTicketByKeyAndCompanyId(CurrentUser.CompanyId.Value, key, EmployeeTag, empid);
            //    }
            //}
            ViewBag.SearchText = key;
            ViewBag.SearchFor = SearchFor;
            ViewBag.Type = type;
            return PartialView("_GlobalSearchResult", model);
        }

        public PartialViewResult GlobalSearchCustomerResult(string key)
        {

            return PartialView();
        }

        [Authorize]
        [HttpGet]
        public JsonResult RabDataMigration()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //this method used for migrating data for rab...
            //may need in future thats why not removing
            //List<Customer> cuslist = _Util.Facade.CustomerFacade.RabDataMigration();
            #region Invoice
            //List<Invoice> invlist = _Util.Facade.InvoiceFacade.RabDataMigrationNewInvoiceList();
            //foreach(var item in invlist)
            //{
            //    List<InvoiceDetail> invdetList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsRabMigration(item.InvoiceId);

            //    foreach(var item2 in invdetList)
            //    {
            //        if (item.IsEstimate)
            //        {
            //            item2.InvoiceId = item.Id.GenerateEstimateNo();
            //        }
            //        else
            //        {
            //            item2.InvoiceId = item.Id.GenerateInvoiceNo();
            //        }
            //        _Util.Facade.InvoiceFacade.UpdateInvoiceDetails(item2); 
            //    }
            //    if (item.IsEstimate)
            //    {
            //        item.InvoiceId = item.Id.GenerateEstimateNo();
            //    }
            //    else
            //    {
            //        item.InvoiceId = item.Id.GenerateInvoiceNo();
            //    }
            //    _Util.Facade.InvoiceFacade.UpdateInvoice(item);

            //}
            #endregion

            #region CustomerSystemNo

            List<Customer> AllCustomer = _Util.Facade.CustomerFacade.GetAllCustomerByCompanyId(CurrentUser.CompanyId.Value);
            foreach (var item in AllCustomer)
            {
                if (!string.IsNullOrWhiteSpace(item.CustomerNo))
                {
                    CustomerSystemNo sys = new CustomerSystemNo()
                    {
                        CustomerId = item.Id,
                        CompanyId = CurrentUser.CompanyId.Value,
                        IsReserved = true,
                        IsUsed = true,
                        UsedDate = item.CreatedDate,
                        ReserveDate = item.CreatedDate,
                        //GenerateDate = item.CreatedDate.HasValue? item.CreatedDate.Value: DateTime.Now,
                        //GenerateDate = item.CreatedDate,
                        CustomerNo = item.CustomerNo,
                    };
                    _Util.Facade.CustomerSystemNoFacade.InsertCustomerSystemNo(sys);
                }
                if (!string.IsNullOrWhiteSpace(item.SecondCustomerNo))
                {
                    CustomerSystemNo sys = new CustomerSystemNo()
                    {
                        CustomerId = item.Id,
                        CompanyId = CurrentUser.CompanyId.Value,
                        IsReserved = true,
                        IsUsed = true,
                        UsedDate = item.CreatedDate,
                        ReserveDate = item.CreatedDate,
                        //GenerateDate = item.CreatedDate.HasValue ? item.CreatedDate.Value : DateTime.Now,
                        //GenerateDate = item.CreatedDate,
                        CustomerNo = item.CustomerNo,
                    };
                    _Util.Facade.CustomerSystemNoFacade.InsertCustomerSystemNo(sys);
                }
                if (!string.IsNullOrWhiteSpace(item.AdditionalCustomerNo))
                {
                    CustomerSystemNo sys = new CustomerSystemNo()
                    {
                        CustomerId = item.Id,
                        CompanyId = CurrentUser.CompanyId.Value,
                        IsReserved = true,
                        IsUsed = true,
                        UsedDate = item.CreatedDate,
                        //ReserveDate = item.CreatedDate,
                        //GenerateDate = item.CreatedDate,
                        CustomerNo = item.CustomerNo,
                    };
                    _Util.Facade.CustomerSystemNoFacade.InsertCustomerSystemNo(sys);
                }
            }
            #endregion
            return Json(true, JsonRequestBehavior.AllowGet);
        }



        [Authorize]
        public PartialViewResult DashBoardServiceBoardList(DateTime StartDateRange, DateTime EndDateRange)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<DashboardServiceBoardListViewModel> model = new List<DashboardServiceBoardListViewModel>();
            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.CustomerAppoinmentFacade.GetDashBoardServiceBoardList(CurrentUser.CompanyId.Value, StartDateRange, EndDateRange);
            }
            return PartialView("_DashBoardServiceBoardList", model);
        }
        [Authorize]
        public PartialViewResult DashBoardInstallationList(DateTime StartDateRange, DateTime EndDateRange)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<DashboardInstallationListViewModel> model = new List<DashboardInstallationListViewModel>();
            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.CustomerAppoinmentFacade.GetDashBoardInstallationList(CurrentUser.CompanyId.Value, StartDateRange, EndDateRange);
            }
            return PartialView("_DashBoardInstallationList", model);
        }
        [Authorize]
        public ActionResult OpenPosition(string lat, string lng)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string MapAPI = _Util.Facade.GlobalSettingsFacade.GetGoogleMapAPIKeyByCompanyId(CurrentUser.CompanyId.Value);
            ViewBag.MapAPIKey = MapAPI;
            if (string.IsNullOrWhiteSpace(lat))
            {
                lat = "0";
            }
            if (string.IsNullOrWhiteSpace(lng))
            {
                lng = "0";
            }
            ViewBag.lat = lat;
            ViewBag.lng = lng;

            return View();
        }

        public ActionResult Error()
        {

            return View();
        }

        [Authorize]
        public ActionResult TagManagementPartial()
        {
            return View();
        } 
        [Authorize]
        public ActionResult AddRMRTag(int? id)
        {
            KnowledgebaseRMRTag model;
            if(id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.CredentialSettingFacade.GetKnowledgebaseRMRTagById(id.Value);
            }
            else
            {
                model = new KnowledgebaseRMRTag();
            }
            return View(model);
        }
        
        [HttpPost]
        public JsonResult SaveRMRTag(RMRTag RMRTag)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            RMRTag.LastUpdatedBy = CurrentUser.UserId;
            RMRTag.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            if (RMRTag.Id > 0)
            {
                var objtag = _Util.Facade.CredentialSettingFacade.GetRMRTagById(RMRTag.Id);
                RMRTag.CreatedDate = objtag.CreatedDate;
                RMRTag.CreatedBy = objtag.CreatedBy;
                result = _Util.Facade.CredentialSettingFacade.UpdateRMRTag(RMRTag);
            }
            else
            {
                RMRTag.TagIdentifier = Guid.NewGuid();
                RMRTag.CreatedDate = DateTime.Now.UTCCurrentTime();
                RMRTag.CreatedBy = CurrentUser.UserId;
                result = _Util.Facade.CredentialSettingFacade.InsertRMRTag(RMRTag) > 0;
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult SaveKnowledgebaseRMRTag(KnowledgebaseRMRTag RMRTag)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            RMRTag.LastUpdatedBy = CurrentUser.UserId;
            RMRTag.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            if (RMRTag.Id > 0)
            {
                var objtag = _Util.Facade.CredentialSettingFacade.GetKnowledgebaseRMRTagById(RMRTag.Id);
                if(objtag != null)
                {
                    objtag.CreatedDate = RMRTag.CreatedDate;
                    objtag.CreatedBy = RMRTag.CreatedBy;
                    result = _Util.Facade.CredentialSettingFacade.UpdateKnowledgebaseRMRTag(objtag);
                } 
            }
            else
            {   
                RMRTag.CreatedDate = DateTime.Now.UTCCurrentTime();
                RMRTag.CreatedBy = CurrentUser.UserId;
                result = _Util.Facade.CredentialSettingFacade.InsertKnowledgebaseRMRTag(RMRTag) > 0;
            }
            return Json(result);
        }

        #region Build Version Log
        [Authorize]
        public PartialViewResult BuildVersionPartial()
        {
            return PartialView();
        }
        [Authorize]
        public ActionResult AddBuilLog(int? id)
        {
            BuildLog model;
            if (id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.CredentialSettingFacade.GetBuilLogById(id.Value);
            }
            else
            {
                model = new BuildLog();
            }
            return View(model);
        }
        public ActionResult FilterBuildVersionList(string search, string order)
        {
            List<BuildLog> model = new List<BuildLog>();
            model = _Util.Facade.CredentialSettingFacade.GetAllBuildLog(search, order);
            return View(model);
        }
        public JsonResult GetMaxBuildVersion()
        {
            bool result = false;
            BuildLog model = new BuildLog();
            long maxversion = _Util.Facade.CredentialSettingFacade.GetMaxVersion();
            if (maxversion != null)
            {
                model = _Util.Facade.CredentialSettingFacade.GetBuilLogByMaxId(maxversion);
                result = true;
            }
            return Json(new { result = result, maxversion = model.Version, maxbuilddate = model.BuildDate });
        }
        [HttpPost]
        public JsonResult SaveBuildLog(BuildLog build)
        { 
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            if (build.Id > 0)
            {
                var obj = _Util.Facade.CredentialSettingFacade.GetBuilLogById(build.Id);
                if (obj != null)
                {
                    obj.CreatedDate = build.CreatedDate;
                    obj.CreatedBy = build.CreatedBy;
                    obj.Version = build.Version;
                    obj.BuildDate = build.BuildDate;
                    result = _Util.Facade.CredentialSettingFacade.UpdateBuildLog(obj);
                }
            }
            else
            {
                build.CreatedDate = DateTime.Now.UTCCurrentTime();
                build.CreatedBy = CurrentUser.UserId; 
                result = _Util.Facade.CredentialSettingFacade.InsertBuildLog(build) > 0;
            }
            return Json(result);
        }

        
        #endregion Build Log


        [Authorize]
        public ActionResult FilterRMRTagList(string search)
        {
            List<RMRTag> model = new List<RMRTag>();
            model = _Util.Facade.CredentialSettingFacade.GetAllRMRTag(search);
            return View(model);
        }
        [Authorize]
        public ActionResult FilterKnowledgebaseRMRTagList(string search, string order)
        {
            List<KnowledgebaseRMRTag> model = new List<KnowledgebaseRMRTag>();
            model = _Util.Facade.CredentialSettingFacade.GetAllKnowledgebaseRMRTag(search, order);
            return View(model);
        }
        
        [Authorize]
        public ActionResult TagedKnowledgebaseList(string search, string order)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<Knowledgebase> model = new List<Knowledgebase>();
            string UserRole = CurrentUser.UserRole;
            model = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebaseList(search, order, UserRole);
            ViewBag.Tag = search;
            return View(model);
        }
        public JsonResult GetTagListByKey(string key)
        {
            List<KnowledgebaseRMRTag> model = new List<KnowledgebaseRMRTag>();
            if (!string.IsNullOrWhiteSpace(key))
            {
                key = HttpUtility.UrlDecode(key);
                model = _Util.Facade.CredentialSettingFacade.GetAllKnowledgebaseRMRTag(key, null);
            }
            return Json(new { result = true, model = model });
        }
        [HttpPost]
        public JsonResult DeleteKnowledgebaseRMRTag(int? Id)
        {
            bool result = false;
            if(Id.HasValue && Id.Value > 0)
            {
                result = _Util.Facade.CredentialSettingFacade.DeleteKnowledgebaseRMRTag(Id.Value);
            }
            return Json(result);
        }
        [HttpPost]
        public JsonResult DeleteBuildLog(int? Id)
        {
            bool result = false;
            if (Id.HasValue && Id.Value > 0)
            {
                result = _Util.Facade.CredentialSettingFacade.DeleteBuilLog(Id.Value);
            }
            return Json(result);
        }

        [Authorize]
        public PartialViewResult LoadTechEquipmentList(string key)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            Employee model = new Employee();
            model = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            model.JobTitle = key;
            return PartialView("~/Views/Technician/TechnicianEquipmentList.cshtml", model);
        }

        [Authorize]
        public PartialViewResult LoadTechUpsoldList(string key)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            Employee model = new Employee();
            model = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            model.JobTitle = key;
            return PartialView("~/Views/Technician/TechnicianUpsoldList.cshtml", model);
        }


        [Authorize]
        public PartialViewResult LoadTechEstimatePartialList(MassRestockFilter massFilter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            //MassRestockFilter massFilter = new MassRestockFilter();
            ViewBag.TechnicianId = CurrentUser.UserId;
            massFilter.TechnicianId = CurrentUser.UserId;
            massFilter.CompanyId = CurrentUser.CompanyId.Value;
            List<CustomerAppointmentEquipment> model = new List<CustomerAppointmentEquipment>();
            model = _Util.Facade.EquipmentFacade.GetCAEquipmentListByCompanyIdTechnicianId(massFilter);
            ViewBag.Id = massFilter.Id;
            ViewBag.isService = massFilter.Searchtext;
            return PartialView("~/Views/Technician/TechnicianEquipmentListPartial.cshtml", model);
        }

        [Authorize]
        public PartialViewResult LoadTechUpsoldPartialList(MassRestockFilter massFilter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            //MassRestockFilter massFilter = new MassRestockFilter();
            ViewBag.TechnicianId = CurrentUser.UserId;
            massFilter.TechnicianId = CurrentUser.UserId;
            massFilter.CompanyId = CurrentUser.CompanyId.Value;
            List<CustomerAppointmentEquipment> model = new List<CustomerAppointmentEquipment>();
            model = _Util.Facade.EquipmentFacade.GetCAUpsoldListByCompanyIdTechnicianId(massFilter);
            ViewBag.Id = massFilter.Id;
            ViewBag.isService = massFilter.Searchtext;
            return PartialView("~/Views/Technician/TechnicianUpsoldListPartial.cshtml", model);
        }

        [Authorize]
        public PartialViewResult LoadGobacklistList(string key)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            //Employee model = new Employee();
            //model = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            //model.JobTitle = key;
            return PartialView("~/Views/Technician/TechnicianGoBackListPartial.cshtml");

        }

        public ActionResult LoadTicketListGoBack(TicketFilter Filters)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<string> typeticket = new List<string>();
            List<string> statusticket = new List<string>();
            List<string> assignticket = new List<string>();
            if (!string.IsNullOrWhiteSpace(Filters.TicketType))
            {
                string[] splituser = Filters.TicketType.Split(',');
                if (splituser.Length > 0)
                {
                    Filters.TicketType = string.Format("{0}", string.Join("','", splituser));
                    foreach (var item in splituser)
                    {
                        typeticket.Add(item);
                    }
                }
            }
            //if (!string.IsNullOrWhiteSpace(Filters.TicketStatus))
            //{
            //    string[] splituser = Filters.TicketStatus.Split(',');
            //    if (splituser.Length > 0)
            //    {
            //        Filters.TicketStatus = string.Format("{0}", string.Join("','", splituser));
            //        foreach (var item in splituser)
            //        {
            //            statusticket.Add(item);
            //        }
            //    }
            //}
            if (!string.IsNullOrWhiteSpace(Filters.AssignedUserTicket))
            {
                string[] splituser = Filters.AssignedUserTicket.Split(',');
                if (splituser.Length > 0)
                {
                    Filters.AssignedUserTicket = string.Format("{0}", string.Join("','", splituser.Select(i => i.Replace("'", "''"))));
                    foreach (var item in splituser)
                    {
                        assignticket.Add(item);
                    }
                }
            }
            if (Filters.StartDate == new DateTime())
            {
                Filters.StartDate = DateTime.Today.AddDays(-90).Date;
            }
            if (Filters.EndDate == new DateTime())
            {
                Filters.EndDate = DateTime.Today.Date;
            }
            if (!Filters.PageNo.HasValue || Filters.PageNo.Value < 1)
            {
                Filters.PageNo = 1;
            }
            if (Filters.SearchText == "undefined" || Filters.SearchText == null)
            {
                Filters.SearchText = "";
            }
            else
            {
                Filters.SearchText = Filters.SearchText.Replace(" ", "");
            }
            Filters.PageSize = _Util.Facade.GlobalSettingsFacade.GetTicketPageLimit(CurrentUser.CompanyId.Value);
            Filters.CompanyId = CurrentUser.CompanyId.Value;
            Filters.UserId = CurrentUser.UserId;
            TicketListModel Model = _Util.Facade.EquipmentFacade.GetAllTicketReportByFilter(Filters);

            //TicketListModel Model = _Util.Facade.EmployeeFacade.GetAllTicketReportByFilter(Filters);
            ViewBag.PageNumber = Filters.PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = Filters.order;
            if (ViewBag.order == null)
            {
                ViewBag.order = 0;
            }
            if (Model.Tickets != null && Model.Tickets.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCount;
            }

            if ((int)ViewBag.PageNumber * Filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * Filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / Filters.PageSize);


            #region Viewbags
            //List<SelectListItem> Items = _Util.Facade.LookupFacade.GetLookupByKey("TicketStatus").OrderBy(x => x.DataOrder).ToList().Select(x =>
            //     new SelectListItem()
            //     {
            //         Text = x.DisplayText.ToString(),
            //         Value = x.DataValue.ToString()
            //     }).ToList();

            //Items.RemoveAt(0);

            //ViewBag.TicketStatus = Items;

            //Items = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").OrderBy(x => x.DataOrder).ToList().Select(x =>
            //   new SelectListItem()
            //   {
            //       Text = x.DisplayText.ToString(),
            //       Value = x.DataValue.ToString()
            //   }).ToList();
            //Items.RemoveAt(0);
            //ViewBag.TicketType = Items;
            //List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value).OrderBy(x => x.FirstName).ToList();
            //var emplst = EmpList.Select(x =>
            //    new SelectListItem()
            //    {
            //        Text = x.FirstName + " " + x.LastName,
            //        Value = x.UserId.ToString(),
            //    }).ToList();

            //ViewBag.EmployeeList = emplst;

            List<SelectListItem> MyTicketList = new List<SelectListItem>();

            MyTicketList.Add(
                new SelectListItem
                {
                    Text = "Other Filters",
                    Value = "-1"
                });
            MyTicketList.Add(
                new SelectListItem
                {
                    Text = "Created By Me",
                    Value = "Created"
                });
            MyTicketList.Add(
                new SelectListItem
                {
                    Text = "Assigned to Me",
                    Value = "Assigned"
                });
            MyTicketList.Add(
                new SelectListItem
                {
                    Text = "Both",
                    Value = "Both"
                });
            MyTicketList.Add(
               new SelectListItem
               {
                   Text = "None",
                   Value = "None"
               });
            ViewBag.MyTicketList = MyTicketList;

            #endregion
            ViewBag.tikettype = typeticket;
            ViewBag.statustiket = statusticket;
            ViewBag.assignticket = assignticket;
            return View("~/Views/Technician/TechnicianGoBackList.cshtml", Model);
            //return View(Model);
        }

        [HttpPost]
        public JsonResult NewOrderNotificationCount(Guid? companyid, string startdate, string enddate)
        {
            var listneworder = new List<ResturantOrder>();
            bool result = false;
            if(companyid.HasValue && companyid.Value != new Guid())
            {
                listneworder = _Util.Facade.MenuFacade.GetAllNewOrdersByCompanyId(companyid.Value, startdate, enddate);
                result = true;
            }
            return Json(new { result = result, neworder = listneworder.Count });
        }

        [HttpPost]
        public JsonResult CheckRestaurantSetup(Guid? companyid)
        {
            bool result = false;
            bool isitem = false;
            string weburl = "";
            if(companyid.HasValue && companyid.Value != new Guid())
            {
                var objitem = _Util.Facade.MenuFacade.GetAllMenuItemByCompanyId(companyid.Value);
                var objwebloc = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(companyid.Value);
                if(objwebloc != null && !string.IsNullOrWhiteSpace(objwebloc.OperationStartTime) && !string.IsNullOrWhiteSpace(objwebloc.OperationEndTime))
                {
                    result = true;
                    weburl = objwebloc.WebsiteURL;
                }
                if(objitem != null && objitem.Count > 0)
                {
                    isitem = true;
                }
            }
            return Json(new { result = result, isitem = isitem, weburl = weburl });
        }
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
        public ActionResult DashboardReport()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
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

            #region CookieJobs
            bool fromCookie = false;
            string newCookie = "";
            if (Request.Cookies[CookieKeys.PtoFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.PtoFilter].Value))
            {
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
                ViewBag.EndDate = CompanyStartDate;
                ViewBag.StartDate = CompanyStartDate.AddDays(-7);
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
            #endregion
            return View();
        }
        public ActionResult ShowAllDashboardCustomerReport(DashboardReportDateFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            DateTime startdate = Convert.ToDateTime(filter.StartDateThisWeek);
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            CustomerDashboardReportModel reportList = new CustomerDashboardReportModel();
            reportList = _Util.Facade.DashboardFacade.GetCustomerDashboardReprot(filter);
            //surveyUserList = _Util.Facade.CustomSurveyFacade.GetAllCustomSurveyUserWithPagination(Convert.ToInt32(PageNumber), Convert.ToInt32(UnitPerPage), SearchText);
            return View(reportList);
        }

        public ActionResult DashboardInsideSalesReport()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
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

            #region CookieJobs
            bool fromCookie = false;
            string newCookie = "";
            if (Request.Cookies[CookieKeys.PtoFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.PtoFilter].Value))
            {
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

            List<SelectListItem> SalesPersonList = new List<SelectListItem>();
            List<Employee> Employeelist = _Util.Facade.EmployeeFacade.GetAllEmployee().Where(x => x.IsCurrentEmployee == true && x.IsSalesMatrix == true).OrderBy(x => x.FirstName).ToList();
            SalesPersonList.Add(new SelectListItem()
            {
                Text = "Select One",
                Value = "-1"
            });
           SalesPersonList.AddRange(Employeelist.Select(x =>
                      new SelectListItem()
                      {
                          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                          Value = x.UserId.ToString()
                      }).ToList());

            ViewBag.SalesPersonList = SalesPersonList;
            if (StartDate == new DateTime() && EndDate == new DateTime())
            {
                ViewBag.EndDate = CompanyStartDate;
                ViewBag.StartDate = CompanyStartDate.AddDays(-7);
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
            #endregion
            return View();
        }
        [Authorize]
        public JsonResult UnassignedLeadsCount()
        {
            string newCookie = "";
            string SelectedFilter = "";
            DateTime Start = new DateTime();
            DateTime End = new DateTime();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            UnassignedLeadCount model = new UnassignedLeadCount();
            if (Request.Cookies[CookieKeys.DateViewFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.DateViewFilter].Value))
            {
                newCookie = Request.Cookies[CookieKeys.DateViewFilter].Value;
                newCookie = Server.UrlDecode(newCookie);
                var CookieVals = newCookie.Split(',');

                if (CookieVals.Length == 3)
                {
                    Start = CookieVals[0].ToDateTime();
                    End = CookieVals[1].ToDateTime();
                    SelectedFilter = CookieVals[2];
                }
            }
            model.TotalLeads = _Util.Facade.CustomerFacade.GetTotalUnassignedLeadsCount(CurrentUser.CompanyId.Value, Start, End);
            return Json(new { result = true, count = model });
        }
        public JsonResult UnreadArticleCount()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            UnassignedLeadCount model = new UnassignedLeadCount();
            //if (CurrentUser.UserRole.ToLower().Contains("admin"))
            //{
            //model.TotalLeads = _Util.Facade.QtiManageFacade.GetKnowledgebaseAccountabilityByAssignedUserForAdmin(Guid.Empty).Count();
            //}
            //else
            //{
            model.TotalLeads = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebaseAccountabilityByAssignedUserForAdmin(CurrentUser.UserId).Count();
            //}

            return Json(new { result = true, count = model });
        }
        public ActionResult ShowInsideSalesDashboardReport(DashboardReportDateFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
   
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            filter.StartDateYesterday = filter.StartdateToday.AddDays(-1);
            filter.EndDateYesterday = filter.StartdateToday.AddDays(-1);

            var weekdaycount = Math.Abs(filter.StartDateThisWeek.Day - filter.EndDateToday.Day);
            DateTime EndDateLastWeek = filter.StartDateLastWeek.AddDays(weekdaycount);
            if (filter.EndDateLastWeek >= EndDateLastWeek)
            {
                filter.EndDateLastWeek = EndDateLastWeek;
            }

            var monthdaycount = Math.Abs(filter.StartDateThisMonth.Day - filter.EndDateToday.Day);
            DateTime EndDateLastMonth = filter.StartDateLastMonth.AddDays(monthdaycount);
            if (filter.EndDateLastMonth >= EndDateLastMonth)
            {
                filter.EndDateLastMonth = EndDateLastMonth;
                
            }


            DashboardReportModel model = new DashboardReportModel();
            CustomerDashboardReportModel reportList = new CustomerDashboardReportModel();
            CustomerPackageDashboardReportModel packageReportList = new CustomerPackageDashboardReportModel();
            model.reoportList = _Util.Facade.DashboardFacade.GetCustomerDashboardReprot(filter);
            model.packageReoportList = _Util.Facade.DashboardFacade.GetPackageDashboardReprot(filter);
            model.paymentReoportList = _Util.Facade.DashboardFacade.GetPaymentDashboardReprot(filter);
            //surveyUserList = _Util.Facade.CustomSurveyFacade.GetAllCustomSurveyUserWithPagination(Convert.ToInt32(PageNumber), Convert.ToInt32(UnitPerPage), SearchText);
            return View(model);
        }
        private void WeeklyViewBag()
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
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
            CultureInfo ci = new CultureInfo("en-US");
            DateTime CompanyStartDate = new DateTime(2018, 7, 1);
            int Week = GetIso8601WeekOfYear(CompanyStartDate);
            int CurrentWeek = GetIso8601WeekOfYear(DateTime.Now);
            CompanyStartDate = FirstDateOfWeek(CompanyStartDate.Year, Week, ci, DateOffset);
            List<SelectListItem> WeekList = new List<SelectListItem>();
            while (CompanyStartDate < DateTime.Now)
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

                WeekList.Add(new SelectListItem()
                {
                    Text = string.Format(CompanyStartDate.ToString("dd{0} MMMM yy"), suffix),
                    Value = CompanyStartDate.Year + "/" + Week,
                    Selected = Week == CurrentWeek
                });
                CompanyStartDate = CompanyStartDate.AddDays(7);
                Week = GetIso8601WeekOfYear(CompanyStartDate);
            }
            ViewBag.WeekList = WeekList;
            ViewBag.FirstDayOfWeek = FirstDayOfWeek;

            ViewBag.EndDate = CompanyStartDate.AddDays(-1);
            ViewBag.StartDate = CompanyStartDate.AddDays(-7);
            ViewBag.Today = DateTime.Now.UTCCurrentTime().ToString("MM/dd/yyyy");
        }
        public ActionResult SoldToFunded(Guid EmpId)
        {
            WeeklyViewBag();
            ViewBag.EmployeeId = EmpId;
            return View();
        }
        public ActionResult ShowCustomerFundedDashboardReport(DashboardFundedReportFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            CustomerFundedDashboardReport reportList = new CustomerFundedDashboardReport();

            reportList = _Util.Facade.DashboardFacade.GetCustomerFundedReprot(filter.StartDate, filter.EndDate, filter.EmployeeId);
            //surveyUserList = _Util.Facade.CustomSurveyFacade.GetAllCustomSurveyUserWithPagination(Convert.ToInt32(PageNumber), Convert.ToInt32(UnitPerPage), SearchText);
            return View(reportList);
        }

        public ActionResult ShowAllFinancedCustomerReport(DashboardReportDateFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            DateTime startdate = Convert.ToDateTime(filter.StartDateThisWeek);
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            CustomerFinancedReportModel reportList = new CustomerFinancedReportModel();
            reportList = _Util.Facade.DashboardFacade.GetCustomerFinancedReprot(filter);
            //surveyUserList = _Util.Facade.CustomSurveyFacade.GetAllCustomSurveyUserWithPagination(Convert.ToInt32(PageNumber), Convert.ToInt32(UnitPerPage), SearchText);
            return View(reportList);
        }


        public ActionResult ShowPackageDashboardReport(DashboardReportDateFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            filter.StartDateYesterday = filter.StartdateToday.AddDays(-1);
            filter.EndDateYesterday = filter.StartdateToday.AddDays(-1);

            var weekdaycount = Math.Abs(filter.StartDateThisWeek.Day - filter.EndDateToday.Day);
            DateTime EndDateLastWeek = filter.StartDateLastWeek.AddDays(weekdaycount);
            if (filter.EndDateLastWeek >= EndDateLastWeek)
            {
                filter.EndDateLastWeek = EndDateLastWeek;
            }

            var monthdaycount = Math.Abs(filter.StartDateThisMonth.Day - filter.EndDateToday.Day);
            DateTime EndDateLastMonth = filter.StartDateLastMonth.AddDays(monthdaycount);
            if (filter.EndDateLastMonth >= EndDateLastMonth)
            {
                filter.EndDateLastMonth = EndDateLastMonth;
            }
            CustomerPackageDashboardReportModel reportList = new CustomerPackageDashboardReportModel();
            reportList = _Util.Facade.DashboardFacade.GetPackageDashboardReprot(filter);
            //surveyUserList = _Util.Facade.CustomSurveyFacade.GetAllCustomSurveyUserWithPagination(Convert.ToInt32(PageNumber), Convert.ToInt32(UnitPerPage), SearchText);
            return View(reportList);
        }
        [Authorize]
        public ActionResult ShowPaymentDashboardReport(DashboardReportDateFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            filter.StartDateYesterday = filter.StartdateToday.AddDays(-1);
            filter.EndDateYesterday = filter.StartdateToday.AddDays(-1);

            var weekdaycount = Math.Abs(filter.StartDateThisWeek.Day - filter.EndDateToday.Day);
            DateTime EndDateLastWeek = filter.StartDateLastWeek.AddDays(weekdaycount);
            if (filter.EndDateLastWeek >= EndDateLastWeek)
            {
                filter.EndDateLastWeek = EndDateLastWeek;
            }

            var monthdaycount = Math.Abs(filter.StartDateThisMonth.Day - filter.EndDateToday.Day);
            DateTime EndDateLastMonth = filter.StartDateLastMonth.AddDays(monthdaycount);
            if (filter.EndDateLastMonth >= EndDateLastMonth)
            {
                filter.EndDateLastMonth = EndDateLastMonth;
            }
            CustomerPaymentDashboardReportModel reportList = new CustomerPaymentDashboardReportModel();
            reportList = _Util.Facade.DashboardFacade.GetPaymentDashboardReprot(filter);
            //surveyUserList = _Util.Facade.CustomSurveyFacade.GetAllCustomSurveyUserWithPagination(Convert.ToInt32(PageNumber), Convert.ToInt32(UnitPerPage), SearchText);
            return View(reportList);
        }
        
        public ActionResult GeeseActivityLog()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            DateTime Today = DateTime.Now.UTCCurrentTime().SetMaxHour();
            DateTime EndDay = DateTime.Now.UTCCurrentTime().AddDays(-30).SetZeroHour();

            GeeseActivityLogModel model = new GeeseActivityLogModel();
            model.TotalCoustomer = _Util.Facade.CustomerFacade.GetTotalCustomerCountByCompanyId(CurrentUser.CompanyId.Value);
            model.TotalEmployee = _Util.Facade.EmployeeFacade.GetTotalEmployeeCountByCompanyId(CurrentUser.CompanyId.Value);
            if(CurrentUser.UserTags == "Employee")
            {
                model.TotalRoute = _Util.Facade.CustomerFacade.GetTotalRouteCountByUserId(CurrentUser.UserId);
                model.TotalCheckIn = _Util.Facade.CustomerFacade.GetTotalCheckInCountByUserId(CurrentUser.UserId, Today, EndDay);
                model.TotalGeese = _Util.Facade.CustomerFacade.GetTotalGeeseCount(CurrentUser.UserId, Today, EndDay);
            }
            else
            {
                model.TotalRoute = _Util.Facade.CustomerFacade.GetTotalRouteCountByUserId(new Guid());
                model.TotalCheckIn = _Util.Facade.CustomerFacade.GetTotalCheckInCountByUserId(new Guid(), Today, EndDay);
                model.TotalGeese = _Util.Facade.CustomerFacade.GetTotalGeeseCount(new Guid(), Today, EndDay);
            }
            return View(model);
        }
    }
}