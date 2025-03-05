using HS.Entities;
using HS.Web.UI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HS.Framework;
using Rotativa;
using Rotativa.Options;
using System.Collections;
using HS.Framework.Utils;
using System.Data;
using System.Drawing;
using System.IO;
using System.Configuration;
using System.Net;
using HS.Entities.Custom;
using HS.Web.UI.Models;
using System.Net.Mail;
using System.Net.Mime;
using EO.Internal;
using System.Globalization;
using System.Text.RegularExpressions;
using PermissionList = HS.Framework.UserPermissions;
using PermissionChecker = HS.Web.UI.Helper.PermissionHelper;
using LabelHelper = HS.Web.UI.Helper.LabelHelper;
using Newtonsoft.Json;
using NLog;
using OS.AWS.S3;
using OS.AWS.S3.Services;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNet.Identity;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace HS.Web.UI.Controllers
{
    public class TicketController : BaseController
    {
        public TicketController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Ticket
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }
        public ActionResult TicketDashboard()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (currentLoggedIn != null)
            {
                return PartialView("_TicketDashboard");
            }
            else
            {
                return PartialView();
            }
        }
        public ActionResult TicketLiteEdit()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (currentLoggedIn != null)
            {
                return PartialView("_TicketLiteEdit");
            }
            else
            {
                return PartialView();
            }
        }
        #region Make address
        private string MakeAddress(string street, string city, string state, string zipcode, string country)
        {
            string address = "";
            if (!string.IsNullOrWhiteSpace(street))
            {
                address += street + ",";
            }
            if (!string.IsNullOrWhiteSpace(city))
            {
                if (city != "-1")
                {
                    address += city + ",";
                }
            }
            if (!string.IsNullOrWhiteSpace(state))
            {
                if (state != "-1")
                {
                    address += state + ",";
                }
            }
            if (!string.IsNullOrWhiteSpace(zipcode))
            {
                address += zipcode + ",";
            }
            if (!string.IsNullOrWhiteSpace(country))
            {
                address += country + ",";
            }
            return address.TrimEnd(',');
        }
        #endregion

        public PartialViewResult TicketListPartial(TicketFilter Filters)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string techid = "";
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
            //logger.Error(CurrentUser.UserTags + " | " + CurrentUser.UserRole);
            if (!CurrentUser.UserTags.Contains("admin") && !CurrentUser.UserTags.Contains("SalesPerson") && CurrentUser.UserRole != "Technician" && CurrentUser.UserRole != "Sales Manager")
            {

                techid = CurrentUser.UserId.ToString();

            }
            if (!IsPermitted(UserPermissions.CustomerTicketPermission.ShowAllTicket))
            {
                Filters.Assigned = CurrentUser.UserId;
            }
            TicketListModel Model = _Util.Facade.TicketFacade.GetTicketListByCustomerIdAndFilter(Filters, techid);
            ViewBag.PageNumber = Filters.PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = Filters.order;

            if (Model.Tickets.Count() > 0)
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
            ViewBag.ImportedTicket = "";
            if (Model.Tickets.Find(x => x.IsImportedTicket == true) != null)
            {
                ViewBag.ImportedTicket = "true";
            }
            List<SelectListItem> Items = _Util.Facade.LookupFacade.GetLookupByKey("TicketStatus").Where(x => x.DataValue != "-1").OrderBy(x => x.DataValue.ToString() != "-1").ThenBy(x => x.DisplayText).ToList().Select(x =>
                  new SelectListItem()
                  {
                      Text = x.DisplayText.ToString(),
                      Value = x.DataValue.ToString()
                  }).ToList();
            var strStatus = HS.Web.UI.Helper.LanguageHelper.T("Ticket");

            Items.Insert(0, new SelectListItem()
            {
                Text = strStatus + " Status",
                Value = "-1"
            });

            ViewBag.TicketStatus = Items;

            Items = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Where(x => x.DataValue != "-1").OrderBy(x => x.DisplayText).ToList().Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString()
               }).ToList();

            Items.Insert(0, new SelectListItem()
            {
                Text = strStatus + " Type",
                Value = "-1"
            });
            ViewBag.TicketType = Items;
            List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value).OrderBy(x => x.FirstName).ToList();
            var emplst = EmpList.Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                }).ToList();
            emplst.Insert(0, new SelectListItem()
            {
                Text = "Assigned User",
                Value = new Guid().ToString()
            });
            ViewBag.EmployeeList = emplst;

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
                    Text = "Created By Me",
                    Value = "Created"
                });
            MyTicketList.Add(
               new SelectListItem
               {
                   Text = "None",
                   Value = "None"
               });
            ViewBag.MyTicketList = MyTicketList;

            #endregion


            return PartialView("_TicketListPartial", Model);
        }

        public ActionResult AgemniTicketPupup(Guid TicketId)
        {
            Ticket ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);

            return View(ticket);
        }
        public ActionResult GetCustomerList()
        {
            string query = Request.QueryString["q[term]"] != null ? Request.QueryString["q[term]"].ToString() : "";
            var customerList = _Util.Facade.CustomerAgreementFacade.GetAllCustomerList(query);
            List<Dictionary<string, object>>
             lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in customerList.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in customerList.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }
            var jsonResult = Json(lstRows, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult GetContactList()
        {
            string query = Request.QueryString["q[term]"] != null ? Request.QueryString["q[term]"].ToString() : "";
            var customerList = _Util.Facade.ContactFacade.GetAllContactList(query);
            List<Dictionary<string, object>>
             lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in customerList.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in customerList.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }

            return Json(lstRows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLeadList()
        {
            string query = Request.QueryString["q[term]"] != null ? Request.QueryString["q[term]"].ToString() : "";
            var leadList = _Util.Facade.CustomerAgreementFacade.GetAllLeadList(query);
            List<Dictionary<string, object>>
             lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in leadList.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in leadList.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }

            return Json(lstRows, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOpportunityList()
        {
            string query = Request.QueryString["q[term]"] != null ? Request.QueryString["q[term]"].ToString() : "";
            var leadList = _Util.Facade.OpportunityFacade.GetAllOpportunityList(query);
            List<Dictionary<string, object>>
             lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in leadList.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in leadList.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }

            return Json(lstRows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteTicketReply(int id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerTicketDelete))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return Json(_Util.Facade.TicketFacade.DeleteTicketReplyById(id));
        }

        [Authorize]
        public PartialViewResult AddTicket(int? Id, string loadDate, Guid? CustomerId, string startTime, int? UserId, string order)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.UserRole = CurrentUser.UserRole;
            CreateTicketModel Model = new CreateTicketModel();
            List<string> RefTicketList = new List<string>();
            if (Id.HasValue && Id.Value > 0)
            {
                #region Existing Ticket
                ViewBag.Order = order;
                Model.Ticket = _Util.Facade.TicketFacade.GetTicketById(Id.Value);
                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(Model.Ticket.CreatedBy);
                if (emp != null)
                {
                    Model.ProfilePicture = emp.ProfilePicture;
                }
                if (Model.Ticket != null)
                {
                    #region Ticket is not null

                    #region ReferenceTicketId
                    if (Model.Ticket.ReferenceTicketId > 0)
                    {
                        RefTicketList.Add(Model.Ticket.ReferenceTicketId.ToString());
                        var objtik = _Util.Facade.TicketFacade.GetTicketById(Model.Ticket.ReferenceTicketId.Value);
                        if (objtik != null && objtik.ReferenceTicketId > 0)
                        {
                            var objreftik = _Util.Facade.TicketFacade.GetTicketById(objtik.ReferenceTicketId.Value);
                            if (objreftik != null)
                            {
                                RefTicketList.Add(objreftik.Id.ToString());
                            }
                        }
                        Model.Ticket.ReferenceTicketList = string.Join(",", RefTicketList);
                    }
                    #endregion

                    #region Commission Data
                    Guid CommissionUserId = Guid.Empty;
                    if (IsPermitted(UserPermissions.CustomerTicketPermission.ShowAllCommission))
                    {
                        CommissionUserId = Guid.Empty;
                    }
                    else
                    {
                        CommissionUserId = CurrentUser.UserId;
                    }
                    Model.SalesCommissionList = _Util.Facade.TicketFacade.GetSalesCommissionByTicketId(Model.Ticket.TicketId, CommissionUserId);
                    Model.TechCommissionList = _Util.Facade.TicketFacade.GetTechCommissionByTicketId(Model.Ticket.TicketId, CommissionUserId);
                    Model.AddMemberCommissionList = _Util.Facade.TicketFacade.GetAddMemberCommissionByTicketId(Model.Ticket.TicketId, CommissionUserId);
                    Model.FinRepCommissionList = _Util.Facade.TicketFacade.GetFinRepCommissionListByTicketId(Model.Ticket.TicketId, CommissionUserId);
                    Model.ServiceCallCommissionList = _Util.Facade.TicketFacade.GetServiceCallCommissionByTicketId(Model.Ticket.TicketId, CommissionUserId);
                    Model.FollowUpCommissionList = _Util.Facade.TicketFacade.GetFollowUpCommissionByTicketId(Model.Ticket.TicketId, CommissionUserId);
                    Model.RescheduleCommissionList = _Util.Facade.TicketFacade.GetRescheduleCommissionByTicketId(Model.Ticket.TicketId, CommissionUserId);
                    #endregion

                    Model.Ticket.CreatedDateVal = string.Format(HS.Framework.Utils.ConvertDatetimeToAgo.TimeAgo(Model.Ticket.CreatedDate).ToString("MM/dd/yyyy hh:mm tt"));
                    //Model.Ticket.CreatedDateVal = string.Format(Model.Ticket.CreatedDate.UTCToClientTime().ToString("MM/dd/yyyy {0} hh:mm tt"), "at");
                    CustomerId = Model.Ticket.CustomerId;
                    Model.PackageCustomermodel = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerIdandCompanyId(CustomerId.Value, CurrentUser.CompanyId.Value);
                    if (Model.Ticket.CompanyId != CurrentUser.CompanyId.Value)
                    {
                        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                    }

                    List<TicketUser> UserList = _Util.Facade.TicketFacade.GetTicketUserListByTicketId(Model.Ticket.TicketId);
                    if (UserList.Count() > 0)
                    {
                        Model.TicketAssignedUserList = UserList.Where(x => x.IsPrimary == true && (!x.NotificationOnly.HasValue || !x.NotificationOnly.Value)).ToList();
                        Model.TicketUserList = UserList.Where(x => x.IsPrimary == false && (!x.NotificationOnly.HasValue || !x.NotificationOnly.Value)).ToList();
                        Model.NotifyingUserList = UserList.Where(x => x.IsPrimary == false && x.NotificationOnly.HasValue && x.NotificationOnly.Value).ToList();
                    }

                    Model.TicketReplyList = _Util.Facade.TicketFacade.GetAllTicketReplyByTicketId(Model.Ticket.TicketId, null);
                    foreach (TicketReply item in Model.TicketReplyList)
                    {

                        item.RepliedDateVal = string.Format(HS.Framework.Utils.ConvertDatetimeToAgo.TimeAgo(item.RepliedDate).ToString("MM/dd/yy hh:mm tt"));
                    }
                    Model.CustomerAppointment = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentByAppIdCusId(Model.Ticket.TicketId, Model.Ticket.CustomerId);
                    if (Model.CustomerAppointment != null && Model.CustomerAppointment.AppointmentId != new Guid())
                    {
                        List<CustomerAppointmentEquipment> AppoinmentEqpList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(Model.Ticket.TicketId);
                        if (AppoinmentEqpList != null && AppoinmentEqpList.Count() > 0)
                        {
                            Model.CustomerAppointmentEquipmentList = AppoinmentEqpList.Where(x => x.EquipmentClassId != 2).ToList();
                            Model.CustomerAppointmentServiceList = AppoinmentEqpList.Where(x => x.EquipmentClassId == 2).ToList();
                        }
                        Model.CustomerAppointmentEquipmentPointList = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentEquipmentPointByAppointmentId(Model.Ticket.TicketId);
                        Model.Ticket.AppointmentStartTime = Model.CustomerAppointment.AppointmentStartTime;
                        Model.Ticket.AppointmentEndTime = Model.CustomerAppointment.AppointmentEndTime;

                    }
                    if (!string.IsNullOrWhiteSpace(Model.Ticket.BookingId))
                    {
                        Model.TicketBookingExtraItem = _Util.Facade.BookingFacade.GetTicketBookingExtraItemListByBookingId(Model.Ticket.BookingId);
                        Model.TicketBookingDetails = _Util.Facade.BookingFacade.GetTicketBookingDetailsByBookingId(Model.Ticket.BookingId);
                    }

                    #endregion


                }
                #endregion


                #region AdditionalMemberAppointment
                List<AdditionalMembersAppointment> memberAppointment = new List<AdditionalMembersAppointment>();

                var memberApp = _Util.Facade.TicketFacade.GetAllAdditionalMembersAppointmenByTicketIdAndEmpIdwithname(Model.Ticket.TicketId, new Guid());
                if (memberApp != null)
                {
                    memberAppointment.AddRange(memberApp);
                }
                Model.memberAppointment = memberAppointment;
                #endregion
                Model.Customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Ticket.CustomerId);
                if (Model.Customer != null)
                {
                    ViewBag.IsRecreate = _Util.Facade.CustomerSignatureFacade.GetRecreateCustomerSignatureByCustomerId(Model.Customer.CustomerId);
                    ViewBag.IsFirstPage = _Util.Facade.CustomerSignatureFacade.GetFirstPageCustomerSignatureByCustomerId(Model.Customer.CustomerId);
                }
            }


            #region if null assign new variable
            Model.TicketDefaultTimeDuration = _Util.Facade.GlobalSettingsFacade.GetTicketDefaultTimeDuration(CurrentUser.CompanyId.Value);

            List<SelectListItem> CustomerList = new List<SelectListItem>();
            CustomerList.Add(new SelectListItem
            {
                Text = LanguageHelper.T("Customer"),
                Value = "-1"
            });
            ViewBag.CustomerList = CustomerList;

            if (Model.Ticket == null)
            {
                Model.Ticket = new Ticket();
                Model.Ticket.Status = LabelHelper.TicketStatus.Init;
                Model.Ticket.CompletionDate = DateTime.Now.AddDays(1);
            }
            if (Model.TicketUserList == null)
            {
                Model.TicketUserList = new List<TicketUser>();
            }
            if (Model.TicketAssignedUserList == null)
            {
                Model.TicketAssignedUserList = new List<TicketUser>();
            }
            if (Model.NotifyingUserList == null)
            {
                Model.NotifyingUserList = new List<TicketUser>();
            }
            if (Model.TicketReplyList == null)
            {
                Model.TicketReplyList = new List<TicketReply>();
            }
            if (Model.CustomerAppointment == null)
            {
                Model.CustomerAppointment = new CustomerAppointment();
            }
            if (Model.CustomerAppointmentEquipmentList == null)
            {
                Model.CustomerAppointmentEquipmentList = new List<CustomerAppointmentEquipment>();
            }
            if (!CustomerId.HasValue || CustomerId == new Guid())
            {
                Model.Customer = new Customer();
            }
            else
            {
                Model.Customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId.Value);
            }
            if (Model.Customer != null)
            {
                Model.Customer.CustomerExtended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(Model.Customer.CustomerId);
                Model.Ticket.CustomerId = Model.Customer.CustomerId;
                if (!string.IsNullOrEmpty(Model.Customer.Soldby))
                {
                    var EmployeeDetails = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(new Guid(Model.Customer.Soldby));
                    if (EmployeeDetails != null)
                    {
                        Model.SoldBy = EmployeeDetails.Title + " " + EmployeeDetails.FirstName + " " + EmployeeDetails.LastName;
                    }
                }
                Model.InvoiceBalanceDue = _Util.Facade.InvoiceFacade.GetInvoiceBalanceDueByCustomerId(Model.Ticket.CustomerId);
            }
            else
            {
                Model.Customer = new Customer();
            }
            #endregion

            #region If value comes from schedule
            if (!string.IsNullOrWhiteSpace(startTime) && startTime != "-1")
            {
                Model.Ticket.AppointmentStartTime = startTime;
                int hour = ((int.Parse(startTime.Split(':')[0]) + Model.TicketDefaultTimeDuration)) % 24;
                string time = "";
                if (hour < 10)
                {
                    time = '0' + hour.ToString();
                }
                else
                {
                    time = hour.ToString();
                }
                time = time + ":" + startTime.Split(':')[1];
                Model.Ticket.AppointmentEndTime = time;
                if (!string.IsNullOrWhiteSpace(loadDate))
                {
                    Model.Ticket.CompletionDate = Convert.ToDateTime(loadDate);
                }

                if (Session["TempTicket"] != null)
                {
                    Ticket TempTicket = (Ticket)Session["TempTicket"];
                    if (Model.Ticket.Id == 0)
                    {
                        Model.Ticket.TicketType = TempTicket.TicketType;
                        Model.Ticket.Message = TempTicket.Message;
                    }
                    ViewBag.TempTicketStatusval = TempTicket.Status;

                    Session["TempTicket"] = null;
                }
            }
            if (UserId.HasValue && UserId > 0)
            {
                Model.TicketAssignedUserList = new List<TicketUser>();
                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeById(UserId.Value);
                if (emp != null)
                {
                    Model.TicketAssignedUserList.Add(new TicketUser()
                    {
                        TiketId = Model.Ticket.TicketId,
                        IsPrimary = true,
                        FullName = emp.FirstName + " " + emp.LastName,
                        UserId = emp.UserId
                    });
                }
            }
            #endregion

            #region TicketJobTime
            List<TicketTimeClock> ticketTimeList = _Util.Facade.TicketClockFacade.GetTicketTimeClockByTicketId(Model.Ticket.TicketId);
            if (ticketTimeList.Count > 0)
            {
                TicketTimeClock LastticketTime = ticketTimeList.OrderByDescending(x => x.Id).FirstOrDefault();
                double Totaltime = 0;
                if (ticketTimeList.Count == 1)
                {
                    Totaltime = (int)DateTime.Now.UTCCurrentTime().Subtract(LastticketTime.Time).TotalMilliseconds;
                }
                else
                {

                    foreach (var item in ticketTimeList)
                    {
                        Totaltime = Totaltime + (item.ClockedInMinutes.HasValue ? item.ClockedInMinutes.Value : 0);
                    }
                    if (LastticketTime.Type != LabelHelper.TicketTimeClockType.End)
                    {
                        Totaltime = (int)DateTime.Now.UTCCurrentTime().Subtract(LastticketTime.Time).TotalMilliseconds + Totaltime;
                    }

                }

                TimeSpan t = TimeSpan.FromMilliseconds(Totaltime);
                ViewBag.Minute = t.Minutes > 0 ? t.Minutes : 0;
                ViewBag.Second = t.Seconds;
                ViewBag.Hour = t.Hours > 0 ? t.Hours : 0;
                if (LastticketTime.Type == LabelHelper.TicketTimeClockType.End)
                {
                    ViewBag.IsClockedIn = true;
                }
                else
                {
                    ViewBag.IsClockedIn = false;
                    ViewBag.Second += 2;
                }
            }
            else
            {
                ViewBag.Minute = 0;
                ViewBag.Second = 0;
                ViewBag.Hour = 0;
                ViewBag.IsClockedIn = true;
            }

            #endregion


            #region ViewBags.
            List<SmartPackage> PackageListItem = new List<SmartPackage>();
            if (CustomerId.HasValue && CustomerId != new Guid())
            {
                #region Invoice and estimate list
                var InvList = _Util.Facade.InvoiceFacade.GetAllOpenInvoiceByCustomerId(CustomerId.Value);
                List<SelectListItem> InvoiceList = InvList.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.InvoiceId.ToString(),
                            Value = x.InvoiceId.ToString()
                        }).ToList();

                InvoiceList.Insert(0, new SelectListItem()
                {
                    Text = "Select One",
                    Value = "-1"
                });
                ViewBag.InvoiceList = InvoiceList;

                var EstList = _Util.Facade.InvoiceFacade.GetAllOpenEstimateByCustomerId(CustomerId.Value);
                List<SelectListItem> EstimateList = EstList.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.InvoiceId.ToString(),
                            Value = x.InvoiceId.ToString()
                        }).ToList();
                EstimateList.Insert(0, new SelectListItem()
                {
                    Text = "Select One",
                    Value = "-1"
                });
                ViewBag.EstimateList = EstimateList;
                #endregion

                var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId.Value);
                if (Model.PackageCustomermodel != null && Model.PackageCustomermodel.SmartSystemTypeId != null && Model.PackageCustomermodel.SmartInstallTypeId != null)
                {
                    var CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(CurrentUser.CompanyId.Value);
                    PackageListItem = _Util.Facade.PackageFacade.GetAllPackageListByCompanyIdSystemIdAndInstallTypeId(CurrentUser.CompanyId.Value, Convert.ToInt32(Model.PackageCustomermodel.SmartSystemTypeId), Convert.ToInt32(Model.PackageCustomermodel.SmartInstallTypeId), Model.PackageCustomermodel.ManufacturerId).ToList();
                    if (CustomerDetails != null && CustomerDetails.Type == "Residential")
                    {
                        PackageListItem = PackageListItem.Where(m => m.UserType == "Residential").ToList();
                    }
                }
            }
            else
            {
                List<SelectListItem> EstimateList = new List<SelectListItem>();
                EstimateList.Insert(0, new SelectListItem()
                {
                    Text = "Select One",
                    Value = "-1"
                });
                ViewBag.EstimateList = EstimateList;
                List<SelectListItem> InvoiceList = new List<SelectListItem>();
                InvoiceList.Insert(0, new SelectListItem()
                {
                    Text = "Select One",
                    Value = "-1"
                });
                ViewBag.InvoiceList = InvoiceList;
            }
            if (Model.Ticket.IsAgreementTicket == true && Model.PackageCustomermodel != null)
            {
                List<SelectListItem> PackageList = new List<SelectListItem>();
                PackageList.Add(new SelectListItem()
                {
                    Text = "Please Select One",
                    Value = "-1"
                });
                var CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(CurrentUser.CompanyId.Value);
                PackageList.AddRange(PackageListItem.Select(x =>
                        new SelectListItem()
                        {
                            Text = ((string.IsNullOrWhiteSpace(x.UserType) || x.UserType == "-1") ? "" : string.Format("{0} -> ", x.UserType)) + x.PackageName.ToString() + (x.ActivationFee == 0 ? "" : string.Format(" [{0}{1}]", CurrentCurrency, LabelHelper.FormatAmount(x.ActivationFee))),
                            Value = x.PackageId.ToString()
                        }).ToList());
                if (PackageList.Where(x => x.Value == Model.PackageCustomermodel.PackageId.ToString()) != null && PackageList.Where(x => x.Value == Model.PackageCustomermodel.PackageId.ToString()).Count() > 0)
                {
                    Model.PackageCustomermodel.PackageName = PackageList.Where(x => x.Value == Model.PackageCustomermodel.PackageId.ToString()).FirstOrDefault().Text;

                }
                ViewBag.PackageList = PackageList;
            }
            //ViewBag.TicketPriority = _Util.Facade.LookupFacade.GetLookupByKey("TicketPriority").OrderBy(x => x.DisplayText).ToList().Select(x =>
            //         new SelectListItem()
            //         {
            //             Text = x.DisplayText.ToString(),
            //             Value = x.DataValue.ToString()
            //         }).ToList();
            #region Is Show Notifying Member
            bool NotifyingMember = false;
            GlobalSetting notifyingGolbal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ShowNotifyingMember");
            if (notifyingGolbal != null)
            {
                if (notifyingGolbal.Value == "true")
                {
                    ViewBag.IsShowNotifyingMember = notifyingGolbal.Value;
                    NotifyingMember = true;
                }
            }

            #endregion
            if (Model.Ticket.Id > 0)
            {
                string PerGrpAssgnTicketId = "";
                GlobalSetting PerGrpAssgnTicket = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "TicketAssignPermissionGroup");
                if (PerGrpAssgnTicket != null && !string.IsNullOrWhiteSpace(PerGrpAssgnTicket.Value))
                {
                    PerGrpAssgnTicketId = PerGrpAssgnTicket.Value;
                }
                var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);
                ViewBag.TechnicianList = TechnicianList.OrderBy(x => x.FirstName).ToList();
                if (PermissionChecker.IsPermitted(PermissionList.CustomerPermissions.CustomerTicketEquipmentSoldBy) || PermissionChecker.IsPermitted(PermissionList.CustomerPermissions.CustomerTicketServiceSoldBy) || NotifyingMember)
                {
                    List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value).OrderBy(x => x.FirstName).ToList();

                    #region EmpListCopy
                    List<SelectListItem> EmpListCopy = new List<SelectListItem>();
                    EmpListCopy.Add(new SelectListItem()
                    {
                        Text = "Select One",
                        Value = "-1"
                    });
                    EmpListCopy.AddRange(EmpList.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.FirstName + " " + x.LastName,
                            Value = x.UserId.ToString(),
                            Selected = Model.TicketUserList.Count(y => y.UserId == x.UserId) > 0
                        }).OrderBy(x => x.Value.ToString() != "-1").ThenBy(x => x.Text).ToList());
                    ViewBag.EmpListCopy = EmpListCopy;
                    #endregion

                    #region Notifying Employee
                    ViewBag.NotifyingUserList = EmpList.Select(x =>
                       new SelectListItem()
                       {
                           Text = x.FirstName + " " + x.LastName,
                           Value = x.UserId.ToString(),
                           Selected = Model.NotifyingUserList.Count(y => y.UserId == x.UserId) > 0
                       }).ToList();
                    #endregion
                }
                if (PermissionChecker.IsPermitted(PermissionList.CustomerPermissions.TicketSurvey) || (Model.Ticket.IsClosed == true && Model.Ticket.HasSurvey == false))
                {
                    #region Survey
                    List<CustomSurvey> surveyUser = _Util.Facade.CustomSurveyFacade.GetAllCustomSurvey();
                    List<SelectListItem> surveyList = surveyUser.Select(x =>
                       new SelectListItem()
                       {
                           Text = x.SurveyName,
                           Value = x.SurveyId.ToString(),
                       }).ToList();

                    surveyList.Insert(0, new SelectListItem()
                    {
                        Text = "Select One",
                        Value = "-1"
                    });
                    ViewBag.SurveyList = surveyList.OrderBy(x => x.Text != "Select One").ThenBy(x => x.Text).ToList();
                }
                #endregion               

                //#region assigned employee
                //List<SelectListItem> AssignedEmployeeList = TechnicianList.OrderBy(x => x.FirstName).Select(x =>
                //    new SelectListItem()
                //    {
                //        Text = x.FirstName + " " + x.LastName,
                //        Value = x.UserId.ToString(),
                //        Selected = Model.TicketAssignedUserList.Count(y => y.UserId == x.UserId) > 0
                //    }).ToList();
                //AssignedEmployeeList.Insert(0, new SelectListItem()
                //{
                //    Text = "Please Select",
                //    Value = Guid.Empty.ToString()
                //});
                //AssignedEmployeeList.Insert(1, new SelectListItem()
                //{
                //    Text = "System User",
                //    Value = "22222222-2222-2222-2222-222222222222",
                //    Selected = Model.TicketAssignedUserList.Count(y => y.UserId == new Guid("22222222-2222-2222-2222-222222222222")) > 0
                //});
                //ViewBag.AssignedEmployee = AssignedEmployeeList;
                //#endregion


            }
            else
            {
                List<SelectListItem> SelectList = new List<SelectListItem>();
                List<Employee> TechEmpList = new List<Employee>();
                if (UserId > 0)
                {
                    string PerGrpAssgnTicketId = "";
                    GlobalSetting PerGrpAssgnTicket = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "TicketAssignPermissionGroup");
                    if (PerGrpAssgnTicket != null && !string.IsNullOrWhiteSpace(PerGrpAssgnTicket.Value))
                    {
                        PerGrpAssgnTicketId = PerGrpAssgnTicket.Value;
                    }
                    TechEmpList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);
                    ViewBag.TechnicianList = TechEmpList.OrderBy(x => x.FirstName).ToList();
                }
                else
                {
                    ViewBag.TechnicianList = TechEmpList;
                }
                SelectList.Add(new SelectListItem()
                {
                    Text = "Select One",
                    Value = "-1"
                });
                ViewBag.EmpListCopy = SelectList;
                ViewBag.SurveyList = SelectList;
                ViewBag.NotifyingUserList = SelectList;

            }
            #region Ticket Reason
            //String TicketReason = "Reason 1,Reason 2";
            string[] TicketReason2;
            if (!string.IsNullOrWhiteSpace(Model.Ticket.Reason))
            {
                TicketReason2 = Model.Ticket.Reason.Split(',');
            }
            else
            {
                TicketReason2 = new string[0];
            }
            #endregion

            #region Location 
            ViewBag.TicketLocationList = _Util.Facade.LookupFacade.GetLookupByKey("TicketLocation").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString()
               }).ToList();
            #endregion

            #region ReasonList
            List<Lookup> TicketReasonList = _Util.Facade.LookupFacade.GetLookupByKey("TicketReason");
            ViewBag.ReasonList = TicketReasonList.Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString(),
                   Selected = TicketReason2.Count(y => y.ToString() == x.DisplayText) > 0
               }).ToList();
            #endregion

            #region TicketStatus
            if (Id > 0)
            {
                List<SelectListItem> TicketStatus = _Util.Facade.LookupFacade.GetLookupByKey("TicketStatus").Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();//.OrderBy(x => x.DisplayText).ToList() has been removed as per requirement of rugtracker - 7-23-2019

                if (base.IsPermitted(HS.Framework.UserPermissions.CustomerPermissions.CompleteTicketPermission))
                {
                    ViewBag.TicketStatus = TicketStatus;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Model.Customer.Singature))
                    {
                        ViewBag.TicketStatus = TicketStatus;
                    }
                    else
                    {
                        ViewBag.TicketStatus = TicketStatus.Where(x => x.Value != LabelHelper.TicketStatus.Completed).ToList();
                    }
                }
            }
            else
            {
                ViewBag.TicketStatus = _Util.Facade.LookupFacade.GetLookupByKey("TicketStatus").Select(x =>
                 new SelectListItem()
                 {
                     Text = x.DisplayText.ToString(),
                     Value = x.DataValue.ToString()
                 }).ToList().Where(x => x.Text == "Created").ToList(); //.OrderBy(x => x.DisplayText).ToList() no need
            }
            #endregion

            #region TicketType
            List<SelectListItem> typeticket = new List<SelectListItem>();
            typeticket.Insert(0, new SelectListItem()
            {
                Text = "Ticket Type",
                Value = "-1"
            });
            typeticket.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("TicketType").Where(x => x.DataValue != "-1").OrderBy(x => x.DisplayText).ToList().Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString()
               }).ToList());
            ViewBag.TicketType = typeticket;
            #endregion

            #region Schedule times
            var arrivaltime = _Util.Facade.LookupFacade.GetLookupByKey("TicketScheduleTime").ToList().Where(x => x.IsActive == true).Select(x =>

            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();


            ViewBag.AppointmentTime = arrivaltime;
            #endregion

            #region View for TaxList

            List<SelectListItem> TaxListItem = new List<SelectListItem>();
            var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(Model.Ticket.CustomerId, CurrentUser.CompanyId.Value);
            if (GetCityTaxList.Count > 0)
            {
                foreach (var item in GetCityTaxList)
                {
                    TaxListItem.Add(new SelectListItem()
                    {
                        Text = " [" + item.City.ToString() + "-" + item.State.ToString() + "]",
                        Value = item.Rate.ToString()
                    });
                }
            }
            else
            {
                Guid tempCustomerId = new Guid();
                if (Model != null)
                {
                    tempCustomerId = Model.Ticket.CustomerId;
                }
                var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, tempCustomerId);
                if (GetSalesTax != null)
                {
                    TaxListItem.Add(new SelectListItem()
                    {
                        Text = GetSalesTax.SearchKey.ToString(),
                        Value = GetSalesTax.Value.ToString()
                    });
                    ViewBag.SalesTax = GetSalesTax.Value;
                }
                else
                {
                    ViewBag.SalesTax = 0;
                }
            }
            TaxListItem.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("NonTaxValue").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            var GetOutOfStateTax = _Util.Facade.GlobalSettingsFacade.GetOutOfStateTax(CurrentUser.CompanyId.Value);
            if (GetOutOfStateTax != null)
            {
                TaxListItem.Add(new SelectListItem()
                {
                    Text = GetOutOfStateTax.SearchKey.ToString(),
                    Value = GetOutOfStateTax.Value.ToString()
                });
            }
            var GetNonProfitTax = _Util.Facade.GlobalSettingsFacade.GetNonProfitTax(CurrentUser.CompanyId.Value);
            if (GetNonProfitTax != null)
            {
                TaxListItem.Add(new SelectListItem()
                {
                    Text = GetNonProfitTax.SearchKey.ToString(),
                    Value = GetNonProfitTax.Value.ToString()
                });
            }
            ViewBag.TaxListItem = TaxListItem;
            #endregion

            #region Ticket Log/Note Tab ordering
            GlobalSetting ShowNoteFirst = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ShowNoteTabFirstOnTicket");
            Model.ShowNoteFirst = true;
            if (ShowNoteFirst != null && ShowNoteFirst.Value.ToLower() == "false")
            {
                Model.ShowNoteFirst = false;
            }
            #endregion
            #region Ticket UpdateEqpServiceBtn
            GlobalSetting UpdateEqpServiceBtn = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UpdateEqpServiceBtnTicket");
            bool UpdateEqpService = false;
            if (UpdateEqpServiceBtn != null && UpdateEqpServiceBtn.Value.ToLower() == "true")
            {
                UpdateEqpService = true;
            }
            ViewBag.UpdateEqpServiceBtn = UpdateEqpService;
            #endregion

            #endregion

            #region RUG Ticket Payment
            if (Model != null && Model.Ticket != null && Model.Ticket.TicketId != new Guid())
            {
                var objticketpay = _Util.Facade.TicketFacade.GetTicketPaymentByTicketId(Model.Ticket.TicketId);
                if (objticketpay != null && objticketpay.IsPaid.HasValue)
                {
                    Model.Ticket.IsTicketPayment = objticketpay.IsPaid.Value;
                }
                else
                {
                    Model.Ticket.IsTicketPayment = false;
                }
            }
            else
            {
                Model.Ticket.IsTicketPayment = false;
            }
            #endregion

            #region PickupAddress and DropofAddress
            CustomerAddress cusPickupAddress = new CustomerAddress();
            CustomerAddress cusDropOfAddress = new CustomerAddress();
            CustomerAddress cusServiceAddress = new CustomerAddress();
            if (CustomerId != null && CustomerId != new Guid())
            {
                if (!string.IsNullOrEmpty(Model.Ticket.BookingId))
                {
                    cusPickupAddress = _Util.Facade.CustomerFacade.GetCustomerAddressByCustomerIdRefIdAddressType(CustomerId.Value, Model.Ticket.BookingId, "PickUpLocation");
                    cusDropOfAddress = _Util.Facade.CustomerFacade.GetCustomerAddressByCustomerIdRefIdAddressType(CustomerId.Value, Model.Ticket.BookingId, "DropOffLocation");
                    cusServiceAddress = _Util.Facade.CustomerFacade.GetCustomerAddressByCustomerIdRefIdAddressType(CustomerId.Value, Model.Ticket.BookingId, "BillingAddress");
                    Model.PickupAddress = cusPickupAddress;
                    Model.DropofAddress = cusDropOfAddress;
                    Model.ServiceAddress = cusServiceAddress;
                }

            }
            #endregion            
            bool IsCommissionCalculation = false;
            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "CommissionCalculationInTicket");
            if (glob != null && glob.Value == "true")
            {
                IsCommissionCalculation = true;
            }
            ViewBag.IsCommissionCalculation = IsCommissionCalculation;
            bool AfterCompleteTicketItemAdd = false;
            GlobalSetting glob2 = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AfterCompleteTicketItemAdd");
            if (glob2 != null && glob2.Value == "true")
            {
                AfterCompleteTicketItemAdd = true;
            }
            ViewBag.AfterCompleteTicketItemAdd = AfterCompleteTicketItemAdd;
            return PartialView("_AddTicket", Model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveMisc(Guid TicketId, string MiscName, decimal MiscValue)
        {
            bool result = false;

            if (TicketId != Guid.Empty && !string.IsNullOrEmpty(MiscName) && MiscValue > 0)
            {
                var ticketDetails = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);
                if (ticketDetails != null)
                {
                    ticketDetails.MiscName = MiscName;
                    ticketDetails.MiscValue = MiscValue;
                    result = _Util.Facade.TicketFacade.UpdateTicket(ticketDetails);
                }
            }

            return Json(new { result = result });
        }


        [Authorize]
        [HttpPost]
        public JsonResult UpdateContractSigned(int Id, int TicketId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            bool? IsContractSignedTrue = true;
            bool? IsContractSignedFalse = false;
            var CustomerContractSigned = _Util.Facade.CustomerFacade.GetById(Id);
            if (CustomerContractSigned != null)
            {
                if (CustomerContractSigned.IsContractSigned == false)
                {

                    CustomerContractSigned.IsContractSigned = IsContractSignedTrue;
                }
                else
                {
                    CustomerContractSigned.IsContractSigned = IsContractSignedFalse;
                }
                result = _Util.Facade.CustomerFacade.UpdateCustomer(CustomerContractSigned);
            }

            return Json(new { result = result, ticketId = TicketId });
        }
        [Authorize]
        [HttpGet]
        public JsonResult LoadAddTicketDropdownListValue()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            string PerGrpAssgnTicketId = "";
            GlobalSetting PerGrpAssgnTicket = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "TicketAssignPermissionGroup");
            if (PerGrpAssgnTicket != null && !string.IsNullOrWhiteSpace(PerGrpAssgnTicket.Value))
            {
                PerGrpAssgnTicketId = PerGrpAssgnTicket.Value;
            }
            List<Employee> TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);
            List<Employee> TechnicianListValue = TechnicianList.OrderBy(x => x.FirstName).ToList();
            List<SelectListItem> EmpListCopy = new List<SelectListItem>();
            List<SelectListItem> NotifyingUserList = new List<SelectListItem>();

            bool NotifyingMember = false;
            GlobalSetting notifyingGolbal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ShowNotifyingMember");
            if (notifyingGolbal != null)
            {
                if (notifyingGolbal.Value == "true")
                {
                    NotifyingMember = true;
                }
            }
            if (PermissionChecker.IsPermitted(PermissionList.CustomerPermissions.CustomerTicketEquipmentSoldBy) || PermissionChecker.IsPermitted(PermissionList.CustomerPermissions.CustomerTicketServiceSoldBy) || NotifyingMember)
            {
                List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value).OrderBy(x => x.FirstName).ToList();

                #region EmpListCopy                
                EmpListCopy.Add(new SelectListItem()
                {
                    Text = "Select One",
                    Value = "-1"
                });
                EmpListCopy.AddRange(EmpList.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.FirstName + " " + x.LastName,
                        Value = x.UserId.ToString()
                    }).OrderBy(x => x.Value.ToString() != "-1").ThenBy(x => x.Text).ToList());
                #endregion

                #region Notifying Employee
                NotifyingUserList = EmpList.Select(x =>
                  new SelectListItem()
                  {
                      Text = x.FirstName + " " + x.LastName,
                      Value = x.UserId.ToString()
                  }).ToList();
                #endregion
            }

            #region Survey
            List<CustomSurvey> surveyUser = _Util.Facade.CustomSurveyFacade.GetAllCustomSurvey();
            List<SelectListItem> surveyList = surveyUser.Select(x =>
               new SelectListItem()
               {
                   Text = x.SurveyName,
                   Value = x.SurveyId.ToString(),
               }).ToList();

            surveyList.Insert(0, new SelectListItem()
            {
                Text = "Select One",
                Value = "-1"
            });
            List<SelectListItem> SurveyListValue = surveyList.OrderBy(x => x.Text != "Select One").ThenBy(x => x.Text).ToList();
            #endregion

            return Json(new { result = true, EmpList = EmpListCopy, SurveyList = SurveyListValue, TechnicianUserList = TechnicianListValue, NotifyingList = NotifyingUserList }, JsonRequestBehavior.AllowGet);
        }
        //[Authorize]
        //public PartialViewResult TicketBooking(List<BookingDetails> BookingDetails, List<BookingExtraItem> BookingExtraItem)
        //{
        //    return PartialView();
        //}
        [Authorize]
        [HttpPost]
        public JsonResult UpdateSalesCommission(Guid CustomerId, Guid UserId, double SalesCommission)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            var EmployeeCommission = _Util.Facade.EmployeeFacade.GetEmployeeComissionByCustomerIdUserId(CustomerId, UserId);
            if (EmployeeCommission != null)
            {
                EmployeeCommission.Amount = SalesCommission;
                result = _Util.Facade.EmployeeFacade.UpdateEmployeeComission(EmployeeCommission);
            }
            else
            {
                EmployeeCommission empCommission = new EmployeeCommission()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    EmployeeCommissionId = Guid.NewGuid(),
                    UserId = UserId,
                    CustomerId = CustomerId,
                    Amount = SalesCommission,
                    CreatedDate = DateTime.Now.UTCCurrentTime()
                };
                result = _Util.Facade.EmployeeFacade.InsertEmployeeComission(empCommission);
            }
            return Json(new { result = result });
        }
        [Authorize]
        [HttpPost]
        public JsonResult UpdateTechCommission()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = true;
            return Json(new { result = result });
        }
        [Authorize]
        public ActionResult TicketPreviewPopup(int? Id, Guid? TicketId)
        {
            ViewBag.Id = Id;
            ViewBag.TicketId = TicketId;

            return View();
        }

        [Authorize]
        public ActionResult PrintTicket(int? Id, Guid? TicketId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Random rand = new Random();
            CreateTicketModel CreateTicket = new CreateTicketModel();
            #region Retrive and validations
            if (Id.HasValue && Id > 0)
            {
                CreateTicket.Ticket = _Util.Facade.TicketFacade.GetTicketById(Id.Value);
            }
            else if (TicketId.HasValue && TicketId.Value != new Guid())
            {
                CreateTicket.Ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId.Value);
            }
            if (CreateTicket.Ticket == null || CreateTicket.Ticket.CompanyId != CurrentUser.CompanyId.Value)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #endregion
            #region RetriveData
            List<TicketUser> UserList = _Util.Facade.TicketFacade.GetTicketUserListByTicketId(CreateTicket.Ticket.TicketId);
            if (UserList.Count() > 0)
            {
                CreateTicket.TicketAssignedUserList = UserList.Where(x => x.IsPrimary == true).ToList();
                CreateTicket.TicketUserList = UserList.Where(x => x.IsPrimary == false && x.NotificationOnly == false).ToList();
            }
            CreateTicket.TicketReplyList = _Util.Facade.TicketFacade.GetAllTicketReplyByTicketId(CreateTicket.Ticket.TicketId, null);

            CreateTicket.CustomerAppointment = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentByAppIdCusId(CreateTicket.Ticket.TicketId, CreateTicket.Ticket.CustomerId);
            if (CreateTicket.CustomerAppointment != null && CreateTicket.CustomerAppointment.AppointmentId != new Guid())
            {
                CreateTicket.CustomerAppointmentEquipmentList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(CreateTicket.Ticket.TicketId);
                CreateTicket.Ticket.AppointmentStartTime = CreateTicket.CustomerAppointment.AppointmentStartTime;
                CreateTicket.Ticket.AppointmentEndTime = CreateTicket.CustomerAppointment.AppointmentEndTime;
                CreateTicket.Ticket.AppointmentStartTimeVal = CreateTicket.CustomerAppointment.AppointmentStartTimeVal;
                CreateTicket.Ticket.AppointmentEndTimeVal = CreateTicket.CustomerAppointment.AppointmentEndTimeVal;

                // = _Util.Facade.CustomerAppoinmentFacade.IsAppointmentEquipmentExistCheck(AppointmentId);
            }
            #region Booking Data
            if (!string.IsNullOrWhiteSpace(CreateTicket.Ticket.BookingId))
            {
                CreateTicket.TicketBookingDetails = _Util.Facade.BookingFacade.GetTicketBookingDetailsByBookingId(CreateTicket.Ticket.BookingId);
                CreateTicket.TicketBookingExtraItem = _Util.Facade.BookingFacade.GetTicketBookingExtraItemListByBookingId(CreateTicket.Ticket.BookingId);

                if (CreateTicket.TicketBookingDetails != null && CreateTicket.TicketBookingDetails.Count > 0)
                {
                    CreateTicket.RugCondtions = _Util.Facade.LookupFacade.GetLookupByKey("RugCondition").Where(x => x.DataValue != "-1").OrderBy(x => x.DataOrder).ToList();
                    foreach (var item in CreateTicket.TicketBookingDetails)
                    {
                        item.RugFiles = _Util.Facade.TicketFacade.GetTicketFilesByDetailsId(item.Id);
                    }
                }
            }
            #endregion
            #region GetAllNotes
            CreateTicket.CustomerNotes = _Util.Facade.NotesFacade.GetNotesListByCustomerId(CreateTicket.Ticket.CustomerId, CurrentUser.CompanyId.Value).Where(x => x.IsFollowUp == false && x.IsShedule == false).ToList();

            #endregion
            #region Company
            CreateTicket.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            CreateTicket.Company.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            CreateTicket.CompanyAddressFormat = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
            #endregion
            #region Customer
            CreateTicket.Customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CreateTicket.Ticket.CustomerId);
            CreateTicket.CustomerAddressFormat = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
            #endregion 
            #region Address formating 
            Hashtable datatemplate = new Hashtable();

            #region CompanyAddress Info
            datatemplate.Add("ComapnyName", CreateTicket.Company.CompanyName);
            datatemplate.Add("Address", CreateTicket.Company.Address);
            datatemplate.Add("Street", CreateTicket.Company.Street);
            datatemplate.Add("City", CreateTicket.Company.City);
            datatemplate.Add("State", CreateTicket.Company.State);
            datatemplate.Add("Zip", CreateTicket.Company.ZipCode);
            datatemplate.Add("CompanyPhone", CreateTicket.Company.Phone);
            datatemplate.Add("EmailAddress", CreateTicket.Company.EmailAdress);
            datatemplate.Add("WebAddress", CreateTicket.Company.Website);
            #endregion

            #region Customer Address info
            datatemplate.Add("CustomerName", CreateTicket.Customer.FirstName + " " + CreateTicket.Customer.LastName);
            datatemplate.Add("CustomerStreet", CreateTicket.Customer.Street);
            datatemplate.Add("CustomerCity", CreateTicket.Customer.City);
            datatemplate.Add("CustomerState", CreateTicket.Customer.State);
            datatemplate.Add("CustomerZip", CreateTicket.Customer.ZipCode);
            datatemplate.Add("CustomerAccountNo", CreateTicket.Customer.CustomerNo);
            CreateTicket.CompanyAddressFormat = HS.Web.UI.Helper.LabelHelper.ParserHelper(CreateTicket.CompanyAddressFormat, datatemplate);
            CreateTicket.CustomerAddressFormat = HS.Web.UI.Helper.LabelHelper.ParserHelper(CreateTicket.CustomerAddressFormat, datatemplate);
            #endregion
            #endregion

            ViewBag.SiteDomain = ConfigurationManager.AppSettings["SiteDomain"];
            #endregion

            if (Request.Browser.IsMobileDevice)
            {
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Ticket/TicketPdf.cshtml", CreateTicket)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                string filename = ConfigurationManager.AppSettings["File.InvoiceFiles"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                filename = string.Format(filename, comname);
                filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + rand.Next().ToString() + "___" + Id + "TicketPDF" + ".pdf";
                string Serverfilename = FileHelper.GetFileFullPath(filename);
                FileHelper.SaveFile(applicationPDFData, Serverfilename);
                var dlfilename = Path.GetFileName(Serverfilename);
                byte[] fileBytes = System.IO.File.ReadAllBytes(Serverfilename);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Serverfilename);

            }


            return new ViewAsPdf("TicketPdf", CreateTicket);
        }
        public ActionResult SendTicketEmail(int id, Guid TicketId)
        {
            bool result = false;
            result = SendTicketAgreementWithSignature(id, TicketId);
            return Json(new { result = result });
        }
        private bool SendTicketAgreementWithSignature(int? Id, Guid? TicketId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Random rand = new Random();
            CreateTicketModel CreateTicket = new CreateTicketModel();
            #region Retrive and validations
            if (Id.HasValue && Id > 0)
            {
                CreateTicket.Ticket = _Util.Facade.TicketFacade.GetTicketById(Id.Value);
            }
            else if (TicketId.HasValue && TicketId.Value != new Guid())
            {
                CreateTicket.Ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId.Value);
            }
            if (CreateTicket.Ticket == null || CreateTicket.Ticket.CompanyId != CurrentUser.CompanyId.Value)
            {
                return false;
            }
            #endregion
            #region RetriveData
            List<TicketUser> UserList = _Util.Facade.TicketFacade.GetTicketUserListByTicketId(CreateTicket.Ticket.TicketId);
            if (UserList.Count() > 0)
            {
                CreateTicket.TicketAssignedUserList = UserList.Where(x => x.IsPrimary == true).ToList();
                CreateTicket.TicketUserList = UserList.Where(x => x.IsPrimary == false && x.NotificationOnly == false).ToList();
            }
            CreateTicket.TicketReplyList = _Util.Facade.TicketFacade.GetAllTicketReplyByTicketId(CreateTicket.Ticket.TicketId, null);

            CreateTicket.CustomerAppointment = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentByAppIdCusId(CreateTicket.Ticket.TicketId, CreateTicket.Ticket.CustomerId);
            if (CreateTicket.CustomerAppointment != null && CreateTicket.CustomerAppointment.AppointmentId != new Guid())
            {
                CreateTicket.CustomerAppointmentEquipmentList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(CreateTicket.Ticket.TicketId);
                CreateTicket.Ticket.AppointmentStartTime = CreateTicket.CustomerAppointment.AppointmentStartTime;
                CreateTicket.Ticket.AppointmentEndTime = CreateTicket.CustomerAppointment.AppointmentEndTime;
                CreateTicket.Ticket.AppointmentStartTimeVal = CreateTicket.CustomerAppointment.AppointmentStartTimeVal;
                CreateTicket.Ticket.AppointmentEndTimeVal = CreateTicket.CustomerAppointment.AppointmentEndTimeVal;

                // = _Util.Facade.CustomerAppoinmentFacade.IsAppointmentEquipmentExistCheck(AppointmentId);
            }
            #region Booking Data
            if (!string.IsNullOrWhiteSpace(CreateTicket.Ticket.BookingId))
            {
                CreateTicket.TicketBookingDetails = _Util.Facade.BookingFacade.GetTicketBookingDetailsByBookingId(CreateTicket.Ticket.BookingId);
                CreateTicket.TicketBookingExtraItem = _Util.Facade.BookingFacade.GetTicketBookingExtraItemListByBookingId(CreateTicket.Ticket.BookingId);

                if (CreateTicket.TicketBookingDetails != null && CreateTicket.TicketBookingDetails.Count > 0)
                {
                    CreateTicket.RugCondtions = _Util.Facade.LookupFacade.GetLookupByKey("RugCondition").Where(x => x.DataValue != "-1").OrderBy(x => x.DataOrder).ToList();
                    foreach (var item in CreateTicket.TicketBookingDetails)
                    {
                        item.RugFiles = _Util.Facade.TicketFacade.GetTicketFilesByDetailsId(item.Id);
                    }
                }
            }
            #endregion
            #region GetAllNotes
            CreateTicket.CustomerNotes = _Util.Facade.NotesFacade.GetAssignedNotesListByCustomerId(CreateTicket.Ticket.CustomerId).Where(x => x.IsFollowUp == false && x.IsShedule == false).ToList();

            #endregion
            #region Company
            CreateTicket.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            CreateTicket.Company.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            CreateTicket.CompanyAddressFormat = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
            #endregion
            #region Customer
            CreateTicket.Customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CreateTicket.Ticket.CustomerId);
            CreateTicket.CustomerAddressFormat = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
            #endregion 
            #region Address formating 
            Hashtable datatemplate = new Hashtable();

            #region CompanyAddress Info
            datatemplate.Add("ComapnyName", CreateTicket.Company.CompanyName);
            datatemplate.Add("Address", CreateTicket.Company.Address);
            datatemplate.Add("Street", CreateTicket.Company.Street);
            datatemplate.Add("City", CreateTicket.Company.City);
            datatemplate.Add("State", CreateTicket.Company.State);
            datatemplate.Add("Zip", CreateTicket.Company.ZipCode);
            datatemplate.Add("CompanyPhone", CreateTicket.Company.Phone);
            datatemplate.Add("EmailAddress", CreateTicket.Company.EmailAdress);
            datatemplate.Add("WebAddress", CreateTicket.Company.Website);
            #endregion

            #region Customer Address info
            datatemplate.Add("CustomerName", CreateTicket.Customer.FirstName + " " + CreateTicket.Customer.LastName);
            datatemplate.Add("CustomerStreet", CreateTicket.Customer.Street);
            datatemplate.Add("CustomerCity", CreateTicket.Customer.City);
            datatemplate.Add("CustomerState", CreateTicket.Customer.State);
            datatemplate.Add("CustomerZip", CreateTicket.Customer.ZipCode);
            datatemplate.Add("CustomerAccountNo", CreateTicket.Customer.CustomerNo);
            CreateTicket.CompanyAddressFormat = HS.Web.UI.Helper.LabelHelper.ParserHelper(CreateTicket.CompanyAddressFormat, datatemplate);
            CreateTicket.CustomerAddressFormat = HS.Web.UI.Helper.LabelHelper.ParserHelper(CreateTicket.CustomerAddressFormat, datatemplate);
            #endregion
            #endregion

            ViewBag.SiteDomain = ConfigurationManager.AppSettings["SiteDomain"];
            #endregion

            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Ticket/TicketPdf.cshtml", CreateTicket)
            {
                //FileName = "TestView.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            string filename = ConfigurationManager.AppSettings["File.InvoiceFiles"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            filename = string.Format(filename, comname);
            filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + rand.Next().ToString() + "___" + Id + "TicketPDF" + ".pdf";
            string Serverfilename = FileHelper.GetFileFullPath(filename);

            FileHelper.SaveFile(applicationPDFData, Serverfilename);
            var dlfilename = Path.GetFileName(Serverfilename);
            byte[] fileBytes = System.IO.File.ReadAllBytes(Serverfilename);
            CustomerFile CustomerFile = new CustomerFile()
            {
                CompanyId = CurrentUser.CompanyId.Value,
                FileId = Guid.NewGuid(),
                CustomerId = CreateTicket.Customer.CustomerId,
                Filename = "/" + filename,
                FileDescription = CreateTicket.Customer.Id + "_" + dlfilename,
                IsActive = true,
                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                CreatedBy = CurrentUser.UserId,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                UpdatedBy = CurrentUser.UserId,
                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                WMStatus = LabelHelper.WatermarkStatus.Pending,
                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
            };
            _Util.Facade.CustomerFileFacade.InsertCustomerFile(CustomerFile);

            if (IsPermitted(UserPermissions.CustomerTicketPermission.SendTicketPDFEmail))
            {
                TicketPDFEmailModel TicketPDFEmailModel = new TicketPDFEmailModel();
                TicketPDFEmailModel.Name = CreateTicket.Customer.FirstName + " " + CreateTicket.Customer.LastName;
                TicketPDFEmailModel.ToEmail = CreateTicket.Customer.EmailAddress;
                TicketPDFEmailModel.attachment = new Attachment(Serverfilename, MediaTypeNames.Application.Octet);
                return _Util.Facade.MailFacade.SendTicketPDFEmail(TicketPDFEmailModel, CurrentUser.CompanyId.Value);
            }
            else
            {
                return true;
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult AttachTicketInvoie(Guid TicketId, string InvoiceId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            #region validation check
            if (TicketId == new Guid() || string.IsNullOrWhiteSpace(InvoiceId))
            {
                return Json(new { result = true, message = "Invalid data." });
            }
            Ticket Ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);
            if (Ticket == null || Ticket.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = true, message = "Invalid data." });
            }
            Customer Customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Ticket.CustomerId);
            Invoice Invoice = _Util.Facade.InvoiceFacade.GetInvoiceByInvoiceId(InvoiceId);
            if (Invoice.CustomerId != Customer.CustomerId)
            {
                return Json(new { result = true, message = "Access denied." });
            }
            #endregion
            List<Employee> employee = _Util.Facade.EmployeeFacade.GetEmployeeByuserIdandCompanyId(CurrentUser.CompanyId.Value, CurrentUser.UserId);

            string InvoiceType = "Invoice";
            string InvoiceOpenFunction = "OpenTicketInvoice";
            if (Ticket != null && Ticket.CompanyId == CurrentUser.CompanyId.Value && Invoice != null)
            {
                InvoiceType = Invoice.IsEstimate == true ? "Estimate" : "Invoice";
                InvoiceOpenFunction = Invoice.IsEstimate == true ? "OpenTicketEstimate" : "OpenTicketInvoice";
                TicketReply tkrply = new TicketReply()
                {
                    RepliedDate = DateTime.Now.UTCCurrentTime(),
                    TicketId = TicketId,
                    UserId = CurrentUser.UserId,
                    Message = string.Format(@"<span>Attached an {1}</span> <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""{2}('{0}')"">{0}</a>", InvoiceId, InvoiceType, InvoiceOpenFunction)
                };
                tkrply.Id = _Util.Facade.TicketFacade.InsertTicketReply(tkrply);

                List<InvoiceDetail> detlist = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(InvoiceId);
                string CustomerAppointmentEquipmentName = "";
                foreach (var item in detlist)
                {
                    CustomerAppointmentEquipment cae = new CustomerAppointmentEquipment()
                    {
                        AppointmentId = TicketId,
                        EquipmentId = item.EquipmentId,
                        CreatedBy = CurrentUser.GetFullName(),
                        EquipDetail = item.EquipDetail,
                        EquipName = item.EquipName,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        Quantity = item.Quantity.HasValue ? item.Quantity.Value : 0,
                        UnitPrice = item.UnitPrice.HasValue ? item.UnitPrice.Value : 0,
                        TotalPrice = item.TotalPrice.HasValue ? item.TotalPrice.Value : 0,
                        IsAgreementItem = false,
                        CreatedByUid = CurrentUser.UserId,
                    };
                    _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppointmentEquipmentDetail(cae);
                    CustomerAppointmentEquipmentName += item.EquipName + " and quantity " + item.Quantity + ",";
                }
                CustomerAppointmentEquipmentName = CustomerAppointmentEquipmentName.TrimEnd(',');
                base.AddUserActivityForCustomer("Equipment is added " + CustomerAppointmentEquipmentName, LabelHelper.ActivityAction.UpdateTicket, Ticket.CustomerId, null, null);

                #region Send Ticket Reply Notification Email

                List<TicketUser> UserList = _Util.Facade.TicketFacade.GetTicketUserListByTicketId(TicketId);

                string ToEmailList = "";
                if (UserList != null && UserList.Count() > 0)
                {
                    //List<Guid> assigned = UserList.Select(x => x.UserId).GroupBy(x => x).Select(x => x.Key).ToList();
                    bool SendNotification = _Util.Facade.GlobalSettingsFacade.SendNotificationToTicketAdditionalMembers(CurrentUser.CompanyId.Value);
                    List<Guid> assigned = new List<Guid>();
                    if (SendNotification)
                    {
                        UserList.Select(x => x.UserId).GroupBy(x => x).Select(x => x.Key).ToList();
                    }
                    else
                    {
                        UserList.Where(x => x.IsPrimary || (x.NotificationOnly.HasValue && x.NotificationOnly.Value)).Select(x => x.UserId).GroupBy(x => x).Select(x => x.Key).ToList();
                    }


                    if (assigned.Where(x => x == Ticket.CreatedBy).Count() == 0)
                    {
                        if (Ticket.CreatedBy != CurrentUser.UserId)
                        {
                            assigned.Add(Ticket.CreatedBy);
                        }
                    }

                    #region Insert notification
                    Notification notification = new Notification()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        NotificationId = Guid.NewGuid(),
                        Type = LabelHelper.NotificationType.Employee,
                        Who = CurrentUser.UserId,
                        //What = string.Format(@"{0} attached <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""{2}('{1}')"">{1}</a> to <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenTicketById('{3}')"">Ticket #{3}</a>", "{0}" , InvoiceId, InvoiceOpenFunction,Ticket.Id),
                        What = string.Format(@"{0} (<a class=""cus-anchor"" href=""/UserInformation/?id={3}"">{3}</a>) attached {1} to Ticket #{2}", "{0}", InvoiceId, Ticket.Id, employee.FirstOrDefault().UserLoginId),
                        NotificationUrl = AppConfig.DomainSitePath + "/Ticket/AddTicket/?Id=" + Ticket.Id,

                    };
                    _Util.Facade.NotificationFacade.InsertNotification(notification);

                    #endregion

                    foreach (Guid item in assigned)
                    {
                        if (item != CurrentUser.UserId)
                        {
                            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(item);
                            if (emp != null && emp.Email.IsValidEmailAddress())
                            {
                                ToEmailList += string.Format("{0}>{1} {2};", emp.Email, emp.FirstName, emp.LastName);
                            }
                            #region set user to notification
                            NotificationUser nu = new NotificationUser()
                            {
                                NotificationId = notification.NotificationId,
                                IsRead = false,
                                NotificationPerson = item,
                            };
                            _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                            #endregion
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(ToEmailList))
                {
                    string CustomerName = Customer.FirstName + " " + Customer.LastName;
                    if (Customer.Type == LabelHelper.CustomerType.Commercial && !string.IsNullOrWhiteSpace(Customer.BusinessName))
                    {
                        CustomerName = Customer.BusinessName;
                    }
                    TicketNotificationEmails TicketReplyNotificationEmail = new TicketNotificationEmails()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedByName = CurrentUser.GetFullName(),
                        TicketMessage = string.Format("Attached an {0} {1}", InvoiceType, InvoiceId),
                        CreatedForCustomerName = CustomerName,
                        TicketNumber = string.Format("Ticket #{0}", Ticket.Id),
                        ToEmail = ToEmailList,
                        HeaderMessage = "A new reply has been added",
                        Subject = string.Format("An {1} has been added to Ticket #{0}", Ticket.Id, InvoiceType),
                        BodyMessage = string.Format("An {3} has been attached by {0} on Ticket #{1} for Customer {2}", CurrentUser.GetFullName(), Ticket.Id, Customer.FirstName + " " + Customer.LastName, InvoiceType),
                        TicketUrl = string.Format("{2}{4}/Public/Open/?TicketId={0}&CompanyId={1}&Type={3}", Ticket.TicketId, CurrentUser.CompanyId.Value, AppConfig.SiteDomain, LabelHelper.PublicOpenTypes.Ticket, AppConfig.DomainSitePath)
                    };
                    _Util.Facade.MailFacade.SendTicketCreatedNotificationEmail(TicketReplyNotificationEmail);
                }
                #endregion

                return Json(new { result = true, message = string.Format("{0} attached successfully.", InvoiceType) });
            }
            return Json(new { result = false, message = "Ticket/Invoice not found." });
        }

        [Authorize]
        [HttpPost]
        public JsonResult DispatchTicket(Guid TicketId)
        {
            Ticket Tick = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);
            if (Tick != null)
            {
                Tick.IsDispatch = true;
                Tick.Status = LabelHelper.TicketStatus.InProgress;
                _Util.Facade.TicketFacade.UpdateTicket(Tick);
                return Json(new { result = true, message = "Rug dispatched successfully." });
            }
            return Json(new { result = false, message = "Ticket not found." });
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddTicket(AddTicketModel Model)
        {
            bool Proceed = true;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool IsNewEqp = false;
            int invId = 0;
            int TicketIdStore = Model.Ticket.Id;
            bool SendSmsStatusToCustomer = false;
            string Message = "";
            string PreviousTicketStatus = "";
            string InstallEquipmentsMsg = "";
            string reason = "";
            List<Employee> employee = _Util.Facade.EmployeeFacade.GetEmployeeByuserIdandCompanyId(CurrentUser.CompanyId.Value, CurrentUser.UserId);

            #region Check Default Billing Tax
            bool defaultBillTaxVal = true;
            GlobalSetting defaultBillTax = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("DefaultCustomerBillingTax");
            if (defaultBillTax != null)
            {
                if (defaultBillTax.Value.ToLower() == "true")
                {
                    defaultBillTaxVal = true;
                }
                else
                {
                    defaultBillTaxVal = false;
                }
            }
            #endregion
            if (Model.Ticket.CustomerId == new Guid())
            {
                Message = "Please select customer";
                return Json(new { result = false, message = Message });
            }
            Customer customerobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Ticket.CustomerId);
            CustomerExtended extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(customerobj.CustomerId);
            if (customerobj != null && string.IsNullOrWhiteSpace(customerobj.City) && string.IsNullOrWhiteSpace(customerobj.State) && string.IsNullOrWhiteSpace(customerobj.ZipCode))
            {
                Message = "Please update customer address";
                return Json(new { result = false, message = Message });
            }
            if (Model != null && Model.ReasonList != null && Model.ReasonList.Count() > 0)
            {
                foreach (var item in Model.ReasonList)
                {
                    reason += item + ",";
                }
            }
            #region Schedule Confilct checker
            //Message = ScheduleConflictChecker(Ticket, Assigned, UserList);

            //if (!string.IsNullOrWhiteSpace(Message))
            //{
            //    return Json(new { result = false, Message = Message });
            //}
            #endregion

            #region Init Values
            int CustomerId = 0;
            Customer TicketCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Ticket.CustomerId);
            ////Please remove the code call

            if (TicketCustomer != null && TicketCustomer.Id > 0)
            {
                CustomerId = TicketCustomer.Id;
            }
            bool IsNewTicket = !(Model.Ticket.Id > 0);
            Model.Ticket.HasInvoice = false;
            #endregion

            if (Model.Ticket.Id > 0)
            {
                #region Update Ticket 
                Ticket TempTicket = _Util.Facade.TicketFacade.GetTicketById(Model.Ticket.Id);
                PreviousTicketStatus = TempTicket.Status;

                #region Send SMS Status TO Customer

                if ((TempTicket.TicketType == LabelHelper.TicketType.PickUp || TempTicket.TicketType == LabelHelper.TicketType.DropOff)
                    && Model.Ticket.Status == "En Route"
                     //&& TempTicket.Status != Ticket.Status
                     && Model.NotifyCustomer.HasValue && Model.NotifyCustomer.Value)
                {
                    SendSmsStatusToCustomer = true;
                }
                #endregion

                string ticketStatus = TempTicket.Status;
                string ticketStatusVal = TempTicket.StatusVal;

                if (TempTicket == null || TempTicket.TicketId != Model.Ticket.TicketId)
                {
                    return Json(new { result = false, message = "Access denied." });
                }
                if (Model.Ticket.CustomerId != Guid.Empty)
                {
                    TempTicket.CustomerId = Model.Ticket.CustomerId;
                }
                if (!string.IsNullOrEmpty(Model.Ticket.TicketType))
                {
                    TempTicket.TicketType = Model.Ticket.TicketType;
                }
                TempTicket.Priority = Model.Ticket.Priority;
                TempTicket.Status = Model.Ticket.Status;
                TempTicket.LastUpdatedBy = CurrentUser.UserId;
                TempTicket.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                TempTicket.AppointmentEndTime = Model.Ticket.AppointmentEndTime;
                TempTicket.AppointmentStartTime = Model.Ticket.AppointmentStartTime;
                TempTicket.IsClosed = Model.Ticket.IsClosed;
                TempTicket.Reason = reason;
                if (Model.Ticket.Status == "On Site")
                {
                    if (TempTicket.TechOnsiteDate == new DateTime() || TempTicket.TechOnsiteDate == null)
                    {
                        TempTicket.TechOnsiteDate = DateTime.Now.UTCCurrentTime();
                    }
                }
                if (Model.Ticket.CompletionDate != new DateTime()
                    && TempTicket.CompletionDate != Model.Ticket.CompletionDate
                    && TempTicket.TicketType == LabelHelper.TicketType.PickUp
                    && !string.IsNullOrWhiteSpace(TempTicket.BookingId))
                {
                    List<Ticket> tickets = _Util.Facade.TicketFacade.GetAllTicketByBookingId(TempTicket.BookingId);
                    if (tickets != null && tickets.Count == 3)
                    {
                        Ticket ServiceTicket = tickets.Where(x => x.TicketType == LabelHelper.TicketType.Service).FirstOrDefault();
                        Ticket DropOffTicket = tickets.Where(x => x.TicketType == LabelHelper.TicketType.DropOff).FirstOrDefault();
                        if (ServiceTicket != null && DropOffTicket != null)
                        {
                            DateTime PickUpDate = Model.Ticket.CompletionDate;
                            DateTime NextWorkingDayPickUp = PickUpDate;//DateTime.Now.UTCCurrentTime().UTCToClientTime().AddDays(1);
                                                                       //Sun=0,Mon=1,Tue=2,Wed=3,Thu=4,Fri=5,Sat=6
                            if (NextWorkingDayPickUp.DayOfWeek == 0)
                            {
                                NextWorkingDayPickUp = NextWorkingDayPickUp.AddDays(1);
                            }
                            DateTime DropOffDate = PickUpDate.AddDays(5);
                            DateTime NextWorkingDayService = NextWorkingDayPickUp.AddDays(1);//DateTime.Now.UTCCurrentTime().UTCToClientTime().AddDays(2);
                            DateTime NextWorkingDayDropOff = DropOffDate;//DateTime.Now.UTCCurrentTime().UTCToClientTime().AddDays(6);

                            if (NextWorkingDayService.DayOfWeek == 0)
                            {
                                NextWorkingDayService = NextWorkingDayService.AddDays(1);
                            }
                            if (NextWorkingDayDropOff.DayOfWeek == 0)
                            {
                                NextWorkingDayDropOff = NextWorkingDayDropOff.AddDays(1);
                            }
                            ServiceTicket.CompletionDate = NextWorkingDayService;
                            DropOffTicket.CompletionDate = NextWorkingDayDropOff;

                            _Util.Facade.TicketFacade.UpdateTicket(ServiceTicket);
                            _Util.Facade.TicketFacade.UpdateTicket(DropOffTicket);

                            ///10,00,000 Record
                            CustomerAppointment customerAppointmentService = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentDetailByAppointmentId(ServiceTicket.TicketId);
                            CustomerAppointment customerAppointmentDropOff = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentDetailByAppointmentId(DropOffTicket.TicketId);
                            //Optimize required

                            if (customerAppointmentService != null)
                            {
                                customerAppointmentService.AppointmentDate = NextWorkingDayService;
                                _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinment(customerAppointmentService);
                            }

                            if (customerAppointmentDropOff != null)
                            {
                                customerAppointmentDropOff.AppointmentDate = NextWorkingDayDropOff;
                                _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinment(customerAppointmentDropOff);
                            }
                        }
                    }
                }
                TempTicket.CompletionDate = Model.Ticket.CompletionDate;
                TempTicket.RackNo = Model.Ticket.RackNo;
                TempTicket.Locations = Model.Ticket.Locations;
                TempTicket.MiscName = Model.Ticket.MiscName;
                TempTicket.MiscValue = Model.Ticket.MiscValue;
                if (Model.Ticket.CompletionDate == null)
                {
                    Model.Ticket.CompletionDate = new DateTime();
                }

                if (Model.Ticket.Status == LabelHelper.TicketStatus.Completed || Model.Ticket.Status == LabelHelper.TicketStatus.Closed)
                {
                    if (Model.Ticket.CompletedDate == null)
                    {
                        TempTicket.CompletedDate = DateTime.Now;
                    }
                    else
                    {
                        TempTicket.CompletedDate = Model.Ticket.CompletedDate;
                    }
                }
                else
                {
                    TempTicket.CompletedDate = null;
                }

                if (Model.Ticket.Status == LabelHelper.TicketStatus.Lost && !string.IsNullOrWhiteSpace(TempTicket.BookingId))
                {
                    #region If customer is lost
                    List<Ticket> tickets = _Util.Facade.TicketFacade.GetAllTicketByBookingId(TempTicket.BookingId);
                    if (tickets != null && tickets.Count > 0)
                    {
                        foreach (var TicketItem in tickets)
                        {
                            if (TicketItem.TicketId != TempTicket.TicketId)
                            {
                                TicketItem.Status = LabelHelper.TicketStatus.Lost;
                                _Util.Facade.TicketFacade.UpdateTicket(TicketItem);
                            }
                        }
                    }
                    Booking booking = _Util.Facade.BookingFacade.GetByBookingId(TempTicket.BookingId);
                    if (booking != null)
                    {
                        booking.Status = LabelHelper.BookingStatus.Cancelled;
                        _Util.Facade.BookingFacade.UpdateBooking(booking);
                    }

                    List<Invoice> invoice = _Util.Facade.InvoiceFacade.GetInvoicebyBookingId(TempTicket.BookingId);


                    if (invoice != null && invoice.Count > 0)
                    {
                        foreach (var invoiceItem in invoice)
                        {
                            if (invoiceItem.Status != LabelHelper.InvoiceStatus.Partial && invoiceItem.Status != LabelHelper.InvoiceStatus.Paid)
                            {
                                string TempStatus = "";
                                TempStatus = invoiceItem.Status;
                                invoiceItem.Status = LabelHelper.InvoiceStatus.Cancelled;
                                _Util.Facade.InvoiceFacade.UpdateInvoice(invoiceItem);
                                if (invoiceItem != null && TempStatus != invoiceItem.Status)
                                {
                                    #region log
                                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                                    bool newBool = invoiceItem.IsARBInvoice ?? false;


                                    base.AddUserActivityForCustomer("Invoice Status Changed from " + TempStatus + " To " + invoiceItem.Status + "#InvoiceId: " + invoiceItem.InvoiceId, lineNumber + "," + actionName + "/" + controllerName, invoiceItem.CustomerId, null, null, newBool);
                                    #endregion
                                }
                            }
                        }
                    }

                    #endregion 
                }
                else if (TempTicket.Status == LabelHelper.TicketStatus.Lost && Model.Ticket.Status != LabelHelper.TicketStatus.Lost && !string.IsNullOrWhiteSpace(TempTicket.BookingId))
                {
                    List<Invoice> invoice = _Util.Facade.InvoiceFacade.GetInvoicebyBookingId(TempTicket.BookingId);
                    if (invoice != null && invoice.Count > 0)
                    {
                        foreach (var invoiceItem in invoice)
                        {
                            if (invoiceItem.Status != LabelHelper.InvoiceStatus.Partial && invoiceItem.Status != LabelHelper.InvoiceStatus.Paid)
                            {
                                string TempStatus = "";
                                TempStatus = invoiceItem.Status;
                                invoiceItem.Status = LabelHelper.InvoiceStatus.Cancelled;
                                _Util.Facade.InvoiceFacade.UpdateInvoice(invoiceItem);
                                if (invoiceItem != null && TempStatus != invoiceItem.Status)
                                {
                                    #region log
                                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                                    bool newBool = invoiceItem.IsARBInvoice ?? false;


                                    base.AddUserActivityForCustomer("Invoice Status Changed from " + TempStatus + " To " + invoiceItem.Status + "#InvoiceId: " + invoiceItem.InvoiceId, lineNumber + "," + actionName + "/" + controllerName, invoiceItem.CustomerId, null, null, newBool);
                                    #endregion
                                }
                            }
                        }
                    }
                    Booking booking = _Util.Facade.BookingFacade.GetByBookingId(TempTicket.BookingId);
                    if (booking != null)
                    {
                        booking.Status = LabelHelper.BookingStatus.Created;
                        _Util.Facade.BookingFacade.UpdateBooking(booking);
                    }

                    List<Ticket> tickets = _Util.Facade.TicketFacade.GetAllTicketByBookingId(TempTicket.BookingId);
                    if (tickets != null && tickets.Count > 0)
                    {
                        foreach (var TicketItem in tickets)
                        {
                            if (TicketItem.TicketId != TempTicket.TicketId)
                            {
                                TicketItem.Status = LabelHelper.TicketStatus.Created;
                                _Util.Facade.TicketFacade.UpdateTicket(TicketItem);
                            }
                        }
                    }

                }
                #region Is Agreement Ticket Change
                if (Model.Ticket.IsAgreementTicket != null)
                {
                    TempTicket.IsAgreementTicket = Model.Ticket.IsAgreementTicket;
                    if (Model.Ticket.IsAgreementTicket == true)
                    {
                        _Util.Facade.TicketFacade.UpdateAllTicketIsAgreementFalseByCustomerId(Model.Ticket.CustomerId);
                    }
                }
                #endregion
                AddUserActivityForUpdateTicket(Model.Ticket, Model.Assigned, Model.UserList, Model.NotifyingUserList);
                _Util.Facade.TicketFacade.UpdateTicket(TempTicket);
                var newStatus = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyandDataValueFromLLookup("TicketStatus", TempTicket.Status);
                
                if (Model.Ticket != null && TicketCustomer != null)
                {
                    GlobalSetting GlobalSettingModel = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("TicketStatusSMS");
                    if (GlobalSettingModel != null && GlobalSettingModel.Value.ToLower() == "true")
                    {
                        SendSMSToChangeTicketStatus(TicketCustomer.Id, Model.Ticket.TicketId, Model.Ticket.Id, ticketStatusVal, newStatus); 
                    } 
                }  
                //base.AddUserActivityForCustomer("Ticket is updated", LabelHelper.ActivityAction.UpdateTicket, TempTicket.CustomerId, null);


                Customer CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Ticket.CustomerId);
                //Find the multiple call - and call data from database single time.
                //Optimization required.

                #region UpdateCustomer
                if (TempTicket.IsAgreementTicket == true
                    && (CustomerDetails != null && CustomerDetails.SalesDate == null)
                    && Model.Assigned != null
                    && Model.Assigned[0] != new Guid("22222222-2222-2222-2222-222222222222"))
                {
                    List<PaymentInfoCustomer> PICLIst = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentInfoCustomerByCustomerId(CustomerDetails.CustomerId);
                    if (PICLIst != null && PICLIst.Count > 0)
                    {
                        bool AllInvoicePaid = true;
                        foreach (var item in PICLIst)
                        {
                            if (!string.IsNullOrWhiteSpace(item.InvoiceId))
                            {
                                Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceByInvoiceId(item.InvoiceId);
                                if (inv != null && inv.BalanceDue > 0)
                                {
                                    AllInvoicePaid = false;
                                    break;
                                }
                            }
                        }
                        if (AllInvoicePaid)
                        {
                            CustomerDetails.SalesDate = DateTime.Now.UTCCurrentTime().UTCToClientTime();
                            _Util.Facade.CustomerFacade.UpdateCustomer(CustomerDetails);
                        }
                    }

                }
                if (TempTicket.TicketType != LabelHelper.TicketType.Service && (Model.Ticket.Status == LabelHelper.TicketStatus.Completed || Model.Ticket.Status == LabelHelper.TicketStatus.Closed))
                {
                    if (CustomerDetails != null)
                    {
                        if (TempTicket.CompletedDate != null)
                        {
                            if (CustomerDetails.InstallDate == null || CustomerDetails.InstallDate == new DateTime())
                            {

                                if (extended != null)
                                {
                                    extended.CustomerSince = TempTicket.CompletedDate.Value;
                                    _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                                }
                                else
                                {
                                    extended = new CustomerExtended();
                                    extended.CustomerId = CustomerDetails.CustomerId;
                                    extended.CustomerSince = TempTicket.CompletedDate.Value;
                                    _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
                                }
                            }
                            else if (extended.CustomerSince == null || extended.CustomerSince == new DateTime())
                            {
                                if (extended != null)
                                {
                                    extended.CustomerSince = TempTicket.CompletedDate.Value;
                                    _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                                }
                                else
                                {
                                    extended = new CustomerExtended();
                                    extended.CustomerId = CustomerDetails.CustomerId;
                                    extended.CustomerSince = TempTicket.CompletedDate.Value;
                                    _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
                                }
                            }
                            CustomerDetails.InstallDate = TempTicket.CompletedDate.Value;
                        }
                        if (Model.Assigned != null && Model.Assigned.Count() > 0)
                        {
                            CustomerDetails.Installer = Model.Assigned[0].ToString();
                        }

                        #region if ticket status is completed for the first time
                        if (PreviousTicketStatus != LabelHelper.TicketStatus.Completed
                            && PreviousTicketStatus != LabelHelper.TicketStatus.Closed
                            && (Model.Ticket.Status == LabelHelper.TicketStatus.Completed || Model.Ticket.Status == LabelHelper.TicketStatus.Closed))
                        {

                            List<PaymentInfoCustomer> PICList = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentInfoCustomerByCustomerId(CustomerDetails.CustomerId);
                            if (PICList != null && PICList.Count > 0)
                            {
                                PaymentInfoCustomer PIC = PICList.Where(x => x.Payfor == "Service").FirstOrDefault();
                                if (PIC != null)
                                {
                                    #region ACH discount
                                    double discountAmount = 0;
                                    var objpayinfocus = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentInfoCustomerByCustomerIdAndPayFor(CustomerDetails.CustomerId);
                                    if (objpayinfocus != null)
                                    {
                                        var objpayprofile = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentProfileByPaymentInfoId(objpayinfocus.PaymentInfoId);
                                        if (objpayprofile != null && objpayprofile.Type.ToLower().IndexOf("ach") > -1)
                                        {
                                            var objglobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ACHDiscount");
                                            if (objglobal != null)
                                            {
                                                discountAmount = Convert.ToDouble(objglobal.Value);
                                            }
                                        }
                                    }
                                    #endregion
                                    List<CustomerPackageService> CustomerPackageServiceList = _Util.Facade.CustomerFacade.IsLeadAppointmentServiceExistCheckCustomerPackageEqp(CustomerDetails.CustomerId, CurrentUser.CompanyId.Value);
                                    double? ServiceCost = CustomerPackageServiceList.Where(x => x.IsARBEnabled).Sum(x => x.Total);
                                    double ServiceCostTax = 0;
                                    if (ServiceCost.HasValue && ServiceCost.Value > 0)
                                    {
                                        ServiceCost = ServiceCost - discountAmount;
                                        #region Tax Calculations
                                        Guid tempCustomerId = new Guid();
                                        if (CustomerDetails != null)
                                        {
                                            tempCustomerId = CustomerDetails.CustomerId;
                                        }
                                        GlobalSetting GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, tempCustomerId);
                                        if (GetSalesTax != null)
                                        {
                                            ServiceCostTax = Math.Round((ServiceCost.Value * Convert.ToDouble(GetSalesTax.Value)) / 100, 2);
                                        }
                                        #endregion

                                        //Update Customer Subscription details
                                        CustomerDetails.BillCycle = LabelHelper.BillCycle.Monthly;
                                        CustomerDetails.BillDay = DateTime.Now.UTCCurrentTime().Day;
                                        CustomerDetails.FirstBilling = DateTime.Now.UTCCurrentTime().AddMonths(PIC.ForMonths.Value);
                                        if (CustomerDetails.BillDay > 28)
                                        {
                                            CustomerDetails.BillDay = 28;
                                            CustomerDetails.FirstBilling = new DateTime(CustomerDetails.FirstBilling.Value.Year, CustomerDetails.FirstBilling.Value.Month, 28);
                                        }
                                        CustomerDetails.MonthlyMonitoringFee = ServiceCost.Value.ToString();
                                        CustomerDetails.BillAmount = defaultBillTaxVal ? (ServiceCost.Value + ServiceCostTax) : ServiceCost.Value;
                                        CustomerDetails.TotalTax = defaultBillTaxVal ? ServiceCostTax : 0;
                                        CustomerDetails.BillTax = defaultBillTaxVal;
                                    }
                                }
                            }
                        }
                        #endregion

                        _Util.Facade.CustomerFacade.UpdateCustomer(CustomerDetails);
                    }

                }
                #endregion

                Model.Ticket = TempTicket;

                var strMsg = HS.Web.UI.Helper.LanguageHelper.T("Ticket");
                Message = strMsg + " added successfully.";
                if (SendSmsStatusToCustomer)
                {
                    #region Send Notification SMS to customer
                    string DriverName = "Driver";
                    if (Model.Assigned != null && Model.Assigned.Length > 0)
                    {
                        Guid AssignedEmployee = new Guid();
                        AssignedEmployee = Model.Assigned[0];
                        Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(AssignedEmployee);
                        if (emp != null)
                        {
                            DriverName = emp.FirstName;
                        }
                    }
                    string CompanyName = "";
                    Company Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
                    if (Company != null)
                    {
                        CompanyName = Company.CompanyName;
                    }
                    string SMSMessage = @"{0}
{1} is in route to pick up your fine rug.";
                    SMSMessage = string.Format(SMSMessage, CompanyName, DriverName);
                    string CellNumber = "";
                    if (Model.Ticket.TicketType == "Drop Off")
                    {
                        SMSMessage = @"{0}
{1} is in route to drop your fine rug.";
                        SMSMessage = string.Format(SMSMessage, CompanyName, DriverName);
                    }
                    if (!string.IsNullOrWhiteSpace(CustomerDetails.CellNo))
                    {
                        CellNumber = CustomerDetails.CellNo;
                    }
                    else if (!string.IsNullOrWhiteSpace(CustomerDetails.PrimaryPhone))
                    {
                        CellNumber = CustomerDetails.PrimaryPhone;
                    }
                    else if (!string.IsNullOrWhiteSpace(CustomerDetails.SecondaryPhone))
                    {
                        CellNumber = CustomerDetails.SecondaryPhone;
                    }

                    _Util.Facade.SMSFacade.SendTicketStatusToCustomer(CurrentUser.CompanyId.Value, CurrentUser.UserId, SMSMessage, CellNumber);

                    #endregion
                }

                #region Ticket Status Updated Log
                Ticket TempTicketUpdated = _Util.Facade.TicketFacade.GetTicketById(TempTicket.Id);
                ///Find the multiple times call 
                ///Optimization required

                if (ticketStatus != Model.Ticket.Status)
                {
                    TicketReply TR = new TicketReply()
                    {
                        Message = CurrentUser.FirstName + " updated ticket status from " + ticketStatusVal + " to " + TempTicketUpdated.StatusVal,
                        TicketId = Model.Ticket.TicketId,
                        RepliedDate = DateTime.Now.UTCCurrentTime(),
                        IsPrivate = true,
                        UserId = CurrentUser.UserId,
                        LatLng = Model.Lat + "," + Model.Lng,
                    };

                    TR.Id = _Util.Facade.TicketFacade.InsertTicketReply(TR);
                }
                #endregion

                #region Insert/ Update Customer Appoinment
                CustomerAppointment ca = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentDetailinfoByAppointmentId(TempTicket.TicketId);

                if (ca != null)
                {
                    bool IsAllDay = true;
                    if (!string.IsNullOrWhiteSpace(Model.Ticket.AppointmentStartTime) && Model.Ticket.AppointmentStartTime != "-1"
                        && !string.IsNullOrWhiteSpace(Model.Ticket.AppointmentEndTime) && Model.Ticket.AppointmentEndTime != "-1")
                    {
                        IsAllDay = false;
                    }
                    if (Model.Ticket.CustomerId != Guid.Empty)
                    {
                        ca.CustomerId = Model.Ticket.CustomerId;
                    }
                    ca.LastUpdatedBy = User.Identity.Name;
                    ca.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    ca.AppointmentStartTime = Model.Ticket.AppointmentStartTime;
                    ca.AppointmentEndTime = Model.Ticket.AppointmentEndTime;
                    ca.AppointmentType = Model.Ticket.TicketType;
                    ca.AppointmentDate = Model.Ticket.CompletionDate;
                    ca.IsAllDay = IsAllDay;
                    _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinment(ca);
                }
                else
                {
                    bool IsAllDay = true;
                    if (!string.IsNullOrWhiteSpace(Model.Ticket.AppointmentStartTime) && Model.Ticket.AppointmentStartTime != "-1"
                        && !string.IsNullOrWhiteSpace(Model.Ticket.AppointmentEndTime) && Model.Ticket.AppointmentEndTime != "-1")
                    {
                        IsAllDay = false;
                    }

                    ca = new CustomerAppointment()
                    {
                        AppointmentDate = Model.Ticket.CompletionDate,
                        AppointmentId = Model.Ticket.TicketId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedBy = CurrentUser.GetFullName(),
                        CustomerId = Model.Ticket.CustomerId,
                        IsAllDay = IsAllDay,
                        EmployeeId = new Guid(),//From now on customer appoinment will be check from ticket user;
                        LastUpdatedBy = User.Identity.Name,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        Notes = Model.Ticket.Message,
                        AppointmentStartTime = Model.Ticket.AppointmentStartTime,
                        AppointmentEndTime = Model.Ticket.AppointmentEndTime,
                        AppointmentType = Model.Ticket.TicketType,

                    };
                    ca.Id = (int)_Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(ca);
                }
                #endregion
                #endregion
                #region Insert on Resign date
                if (TempTicket.TicketType == LabelHelper.TicketType.InstallResign && TempTicket.Status == LabelHelper.TicketStatus.Completed)
                {
                    if (extended != null)
                    {
                        extended.ResignedBy = TempTicket.CreatedBy;
                        extended.ResignDate = TempTicket.CompletedDate.Value;
                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                    }
                    else
                    {
                        extended = new CustomerExtended();
                        extended.ResignedBy = TempTicket.CreatedBy;
                        extended.CustomerId = CustomerDetails.CustomerId;
                        extended.ResignDate = TempTicket.CompletedDate.Value;
                        _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
                    }
                }
                #endregion

                #region Send Email From ticket Email Settings
                if (PreviousTicketStatus != Model.Ticket.Status)
                {
                    TicketNotificationEmail email = _Util.Facade.TicketFacade.GetTicketNotificationEmailByStatus(Model.Ticket.Status);
                    if (email != null)
                    {
                        string CustomerName = "Self";

                        if (TicketCustomer != null)
                        {
                            CustomerName = TicketCustomer.FirstName + " " + TicketCustomer.LastName;
                            if (TicketCustomer.Type == LabelHelper.CustomerType.Commercial && !string.IsNullOrWhiteSpace(TicketCustomer.BusinessName))
                            {
                                CustomerName = TicketCustomer.BusinessName;
                            }
                        }
                        TicketNotificationEmails TicketCreatedNotificationEmail = new TicketNotificationEmails()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            CreatedByName = CurrentUser.GetFullName(),
                            TicketMessage = Model.Ticket.Message,
                            CreatedForCustomerName = CustomerName,
                            AppointmentEndTime = Model.Ticket.AppointmentEndTime,
                            CustomerAddress = MakeAddress(TicketCustomer.Street, TicketCustomer.City, TicketCustomer.State, TicketCustomer.ZipCode, TicketCustomer.Country),
                            AppointmentStartTime = Model.Ticket.AppointmentStartTime,
                            CompletionDate = Model.Ticket.CompletionDate,
                            ToEmail = email.Email,
                            Subject = string.Format("A ticket has been updated (Ticket #{0})", Model.Ticket.Id),
                            HeaderMessage = "A new ticket has been updated",
                            TicketStatus = email.TicketStatusVal,
                            TicketNumber = Model.Ticket.Id.ToString(),

                        };
                        _Util.Facade.MailFacade.SendTicketUpdatedNotificationEmail(TicketCreatedNotificationEmail);
                    }
                }


                #endregion

            }
            else
            {
                #region Insert Ticket 
                Model.Ticket.CompletedDate = null;
                Model.Ticket.CreatedBy = CurrentUser.UserId;
                Model.Ticket.CreatedDate = DateTime.Now.UTCCurrentTime();
                Model.Ticket.LastUpdatedBy = CurrentUser.UserId;
                Model.Ticket.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                Model.Ticket.CompanyId = CurrentUser.CompanyId.Value;
                Model.Ticket.TicketId = Guid.NewGuid();
                Model.Ticket.Reason = reason;
                if (Model.Ticket.Status == "On Site")
                {
                    Model.Ticket.TechOnsiteDate = DateTime.Now.UTCCurrentTime();
                }
                if (Model.Ticket.CompletionDate == null)
                {
                    Model.Ticket.CompletionDate = new DateTime();
                }
                if (Model.Ticket.TicketType == "Installation" ||
                    Model.Ticket.TicketType == "Install NFT")
                {
                    Model.Ticket.IsAgreementTicket = true;
                    _Util.Facade.TicketFacade.UpdateAllTicketIsAgreementFalseByCustomerId(Model.Ticket.CustomerId);
                }
                Model.Ticket.Id = _Util.Facade.TicketFacade.InsertTicket(Model.Ticket);
                logger.WithProperty("tags", "ticket,insert").WithProperty("params", JsonConvert.SerializeObject(Model.Ticket)).Trace("Ticket Id {Id}", Model.Ticket.Id);
                AddUserActivityForNewTicket(Model.Ticket, Model.Assigned, Model.UserList, Model.NotifyingUserList);

                //base.AddUserActivityForCustomer("New Ticket added", LabelHelper.ActivityAction.AddTicket, Model.Ticket.CustomerId, null);

                var strMsg = HS.Web.UI.Helper.LanguageHelper.T("Ticket");
                Message = strMsg + " added successfully.";


                bool IsAllDay = true;
                if (!string.IsNullOrWhiteSpace(Model.Ticket.AppointmentStartTime) && Model.Ticket.AppointmentStartTime != "-1"
                    && !string.IsNullOrWhiteSpace(Model.Ticket.AppointmentEndTime) && Model.Ticket.AppointmentEndTime != "-1")
                {
                    IsAllDay = false;
                }
                CustomerAppointment ca = new CustomerAppointment()
                {
                    AppointmentDate = Model.Ticket.CompletionDate,
                    AppointmentId = Model.Ticket.TicketId,
                    CompanyId = CurrentUser.CompanyId.Value,
                    CreatedBy = User.Identity.Name,
                    CustomerId = Model.Ticket.CustomerId,
                    IsAllDay = IsAllDay,
                    EmployeeId = new Guid(),//From now on customer appoinment will be check from ticket user;
                    LastUpdatedBy = User.Identity.Name,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    Notes = Model.Ticket.Message,
                    AppointmentStartTime = Model.Ticket.AppointmentStartTime,
                    AppointmentEndTime = Model.Ticket.AppointmentEndTime,
                    AppointmentType = Model.Ticket.TicketType,

                };
                ca.Id = (int)_Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(ca);

                #region Default service add
                var ServiceName = "";
                if (Model.Ticket.TicketType == LabelHelper.TicketType.Service)
                {
                    ServiceName = "Service Fee";
                }
                if (!string.IsNullOrEmpty(ServiceName))
                {
                    var DefaultService = _Util.Facade.EquipmentFacade.GetDefaultService(ServiceName);
                    if (DefaultService != null)
                    {
                        CustomerAppointmentEquipment cae = new CustomerAppointmentEquipment()
                        {
                            AppointmentId = Model.Ticket.TicketId,
                            EquipmentId = DefaultService.EquipmentId,
                            Quantity = 1,
                            UnitPrice = Convert.ToDouble(DefaultService.Retail),
                            TotalPrice = Convert.ToDouble(DefaultService.Retail),
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            CreatedBy = CurrentUser.FirstName + " " + CurrentUser.LastName,
                            EquipName = DefaultService.Name,
                            EquipDetail = DefaultService.Description,
                            IsEquipmentRelease = false,
                            IsService = true,
                            CreatedByUid = CurrentUser.UserId,
                            IsAgreementItem = false,
                            IsDefaultService = true
                        };
                        _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppointmentEquipmentDetail(cae);
                    }
                }
                #endregion
                #endregion
                #region InsertOnNoteRemainder
                if (!String.IsNullOrWhiteSpace(Model.Ticket.Message))
                {
                    CustomerNote cusNote = new CustomerNote()
                    {
                        Notes = Model.Ticket.Message,
                        NoteType = "Ticket",
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        CreatedBy = CurrentUser.UserId.ToString(),
                        CreatedByUid = CurrentUser.UserId,
                        CustomerId = Model.Ticket.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        IsOverview = false,
                        ReferenceTicketId = Model.Ticket.Id,
                        IsActive = false
                    };


                    _Util.Facade.NotesFacade.InsertCustomerNote(cusNote);
                }

                #endregion
                #region Only Create Addendum
                var objtiklist = _Util.Facade.TicketFacade.GetTicketListByCustomerIdAndCompanyIdAndNewTicketId(Model.Ticket.CustomerId, CurrentUser.CompanyId.Value, Model.Ticket.TicketId);
                if (objtiklist != null && objtiklist.Count > 0)
                {
                    foreach (var item in objtiklist)
                    {
                        var objappeqp = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentEquipmentByAppointmentIdAndBilling(item.TicketId);
                        if (objappeqp != null && objappeqp.Count > 0)
                        {
                            foreach (var eqp in objappeqp)
                            {
                                eqp.IsBilling = true;
                                if (item.Status.ToLower() != "completed")
                                {
                                    eqp.IsBillingProcess = true;
                                }
                                else
                                {
                                    eqp.IsBillingProcess = false;
                                }
                                logger.WithProperty("tags", "ticket,save,CAE").WithProperty("params", JsonConvert.SerializeObject(eqp))
                                .Trace($"The CAE record for {eqp.Id} {eqp.EquipmentName} in ticket #{item.Id} | 2509");
                                _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(eqp);
                                if (item.Status.ToLower() == "completed")
                                {
                                    CustomerAppointmentEquipment cae = new CustomerAppointmentEquipment()
                                    {
                                        AppointmentId = Model.Ticket.TicketId,
                                        EquipmentId = eqp.EquipmentId,
                                        Quantity = eqp.Quantity,
                                        UnitPrice = eqp.UnitPrice,
                                        TotalPrice = eqp.TotalPrice,
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        CreatedBy = CurrentUser.FirstName + " " + CurrentUser.LastName,
                                        EquipName = eqp.EquipName,
                                        EquipDetail = string.IsNullOrEmpty(eqp.EquipDetail) ? "??? TT 2520" : eqp.EquipDetail,
                                        IsEquipmentRelease = false,
                                        IsService = true,
                                        CreatedByUid = CurrentUser.UserId,
                                        IsAgreementItem = false,
                                        IsBilling = true,
                                        IsBillingProcess = true,
                                        IsCopied = true
                                    };
                                    _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppointmentEquipmentDetail(cae);
                                    CustomerPackageService CustomerPackageService = new CustomerPackageService()
                                    {
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        CustomerId = Model.Ticket.CustomerId,
                                        PackageId = new Guid(),
                                        EquipmentId = eqp.EquipmentId,
                                        MonthlyRate = eqp.UnitPrice,
                                        DiscountRate = 0,
                                        Total = eqp.TotalPrice,
                                        ManufacturerId = new Guid(),
                                        LocationId = new Guid(),
                                        TypeId = new Guid(),
                                        ModelId = new Guid(),
                                        FinishId = new Guid(),
                                        CapacityId = new Guid(),
                                        IsPackageService = false,
                                        IsNonCommissionable = false,
                                        AppointmentIntId = Model.Ticket.Id,
                                        AppointmentEquipmentIntId = cae.Id
                                    };
                                    _Util.Facade.PackageFacade.InsertCustomerPackageService(CustomerPackageService);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Insert on Resign date
                if (Model.Ticket.TicketType == LabelHelper.TicketType.InstallResign)
                {

                    if (extended != null)
                    {
                        extended.ResignedBy = Model.Ticket.CreatedBy;
                        extended.ResignDate = Model.Ticket.CreatedDate;
                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                    }
                    else
                    {
                        extended = new CustomerExtended();
                        extended.ResignedBy = Model.Ticket.CreatedBy;
                        extended.CustomerId = customerobj.CustomerId;
                        extended.ResignDate = Model.Ticket.CreatedDate;
                        _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
                    }
                }

                #endregion

            }

            #region Insert/Update TicketUser 
            Employee empuser = new Employee();
            string NotifyMember = "";
            string AdditionalMember = "";
            string AssignedMember = "";
            bool isAssingedChanged = false;
            Guid AssignedTo = Guid.Empty;
            bool isNotifyChanged = false;
            bool isAdditionalChanged = false;

            #region Assigned User
            List<TicketUser> ticketUser = _Util.Facade.TicketFacade.GetTicketAssignedUserListByTicketId(Model.Ticket.TicketId);
            List<string> assinguser = new List<string>();
            if (ticketUser.Count > 0)
            {

                foreach (var item in ticketUser)
                {
                    assinguser.Add(item.UserId.ToString());

                }
            }

            _Util.Facade.TicketFacade.DeleteTicketUserByTicketId(Model.Ticket.TicketId, true);
            if (Model.Assigned != null && Model.Assigned.Count() > 0)
            {
                foreach (var item in Model.Assigned)
                {
                    if (item != Guid.Empty)
                    {
                        AssignedTo = item;
                        var TicketUser = new TicketUser()
                        {
                            AddedBy = CurrentUser.UserId,
                            AddedDate = DateTime.Now,
                            UserId = item,
                            IsPrimary = true,
                            TiketId = Model.Ticket.TicketId,
                            NotificationOnly = false,
                        };
                        _Util.Facade.TicketFacade.InsertTicketUser(TicketUser);
                        empuser = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item);
                        int index = assinguser.FindIndex(s => s.Contains(item.ToString()));
                        if (index == -1)
                        {
                            isAssingedChanged = true;
                        }
                        AssignedMember += empuser.FirstName + " " + empuser.LastName + ",";
                    }
                }
                if (isAssingedChanged == true)
                {
                    #region All Equipment Installed By Change
                    if (AssignedTo != Guid.Empty)
                    {
                        _Util.Facade.CustomerAppoinmentFacade.UpdateInstalledByCustomerAppointmentEqp(Model.Ticket.TicketId, AssignedTo);
                    }
                    #endregion
                    AssignedMember = AssignedMember.Remove(AssignedMember.Length - 1, 1); ;
                    TicketReply TR = new TicketReply()
                    {
                        Message = CurrentUser.FirstName + " assigned to " + AssignedMember + " for this ticket ",
                        TicketId = Model.Ticket.TicketId,
                        RepliedDate = DateTime.Now.UTCCurrentTime(),
                        IsPrivate = true,
                        UserId = CurrentUser.UserId
                    };

                    TR.Id = _Util.Facade.TicketFacade.InsertTicketReply(TR);
                }

            }
            #endregion

            #region Additional User
            List<TicketUser> additionalUser = _Util.Facade.TicketFacade.GetTicketAddtionalUsersByTicketId(Model.Ticket.TicketId);
            List<string> additionaluser = new List<string>();
            if (additionalUser.Count > 0)
            {

                foreach (var item in additionalUser)
                {
                    additionaluser.Add(item.UserId.ToString());

                }
            }
            _Util.Facade.TicketFacade.DeleteTicketUserByTicketId(Model.Ticket.TicketId, false);
            if (Model.UserList != null && Model.UserList.Count() > 0)
            {
                foreach (var item in Model.UserList)
                {
                    if (item == new Guid())
                        continue;

                    var TicketUser = new TicketUser()
                    {
                        AddedBy = CurrentUser.UserId,
                        AddedDate = DateTime.Now.UTCCurrentTime(),
                        UserId = item,
                        IsPrimary = false,
                        TiketId = Model.Ticket.TicketId,
                        NotificationOnly = false,
                    };
                    _Util.Facade.TicketFacade.InsertTicketUser(TicketUser);
                    empuser = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item);
                    int index = additionaluser.FindIndex(s => s.Contains(item.ToString()));
                    if (index == -1)
                    {
                        isAdditionalChanged = true;
                    }
                    AdditionalMember += empuser.FirstName + " " + empuser.LastName + ",";
                }
                if (isAdditionalChanged == true)
                {
                    AdditionalMember = AdditionalMember.Remove(AdditionalMember.Length - 1, 1); ;
                    TicketReply TR = new TicketReply()
                    {
                        Message = CurrentUser.FirstName + " assigned " + AdditionalMember + " as additional member ",
                        TicketId = Model.Ticket.TicketId,
                        RepliedDate = DateTime.Now.UTCCurrentTime(),
                        IsPrivate = true,
                        UserId = CurrentUser.UserId
                    };

                    TR.Id = _Util.Facade.TicketFacade.InsertTicketReply(TR);
                }
            }
            #endregion

            #region Notifying User
            List<TicketUser> NotifyUser = _Util.Facade.TicketFacade.GetTicketUserListByTicketId(Model.Ticket.TicketId).Where(x => x.NotificationOnly == true).ToList();
            List<string> NotifyUserList = new List<string>();
            if (NotifyUser.Count > 0)
            {

                foreach (var item in NotifyUser)
                {
                    NotifyUserList.Add(item.UserId.ToString());

                }
            }
            _Util.Facade.TicketFacade.DeleteTicketUserByTicketId(Model.Ticket.TicketId, null, true);
            if (Model.NotifyingUserList != null && Model.NotifyingUserList.Count() > 0)
            {
                foreach (var item in Model.NotifyingUserList)
                {
                    if (item == new Guid())
                        continue;

                    var TicketUser = new TicketUser()
                    {
                        AddedBy = CurrentUser.UserId,
                        AddedDate = DateTime.Now.UTCCurrentTime(),
                        UserId = item,
                        IsPrimary = false,
                        TiketId = Model.Ticket.TicketId,
                        NotificationOnly = true,
                    };
                    _Util.Facade.TicketFacade.InsertTicketUser(TicketUser);
                    empuser = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item);
                    int index = NotifyUserList.FindIndex(s => s.Contains(item.ToString()));
                    if (index == -1)
                    {
                        isNotifyChanged = true;
                    }
                    NotifyMember += empuser.FirstName + " " + empuser.LastName + ",";
                }
                if (isNotifyChanged == true)
                {
                    NotifyMember = NotifyMember.Remove(NotifyMember.Length - 1, 1); ;
                    TicketReply TR = new TicketReply()
                    {
                        Message = CurrentUser.FirstName + " assigned " + NotifyMember + " as notifying member ",
                        TicketId = Model.Ticket.TicketId,
                        RepliedDate = DateTime.Now.UTCCurrentTime(),
                        IsPrivate = true,
                        UserId = CurrentUser.UserId
                    };

                    TR.Id = _Util.Facade.TicketFacade.InsertTicketReply(TR);
                }

            }
            #endregion

            #endregion

            if (IsNewTicket)
            {
                //#region Ticket Assign Send Email
                //if (IsPermitted(UserPermissions.CustomerTicketPermission.SendAssignedEmail))
                //{
                //    SendEmailToAssignUser(Model.Ticket.Id);
                //}
                //#endregion
                #region Send Ticket Created Email
                string ToEmailList = "";
                bool SendNotification = _Util.Facade.GlobalSettingsFacade.SendNotificationToTicketAdditionalMembers(CurrentUser.CompanyId.Value);
                List<Guid> AllUser = new List<Guid>();
                string IsAssignUserTurnOn = "";

                GlobalSetting GlobalSettingDetails = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "SendNotificationToTicketAssignedUser");
                if (GlobalSettingDetails != null)
                {
                    IsAssignUserTurnOn = GlobalSettingDetails.Value;
                }

                if (IsAssignUserTurnOn == "true")
                {
                    if (Model.Assigned != null)
                    {
                        AllUser.AddRange(Model.Assigned);
                    }
                }

                if (SendNotification)
                {
                    if (Model.UserList != null)
                    {
                        AllUser.AddRange(Model.UserList);
                    }
                }
                if (Model.NotifyingUserList != null)
                {
                    AllUser.AddRange(Model.NotifyingUserList);
                }
                //AllUser = AllUser.GroupBy(x => x).Select(x => x.Key).ToList();

                #region Insert notification
                Notification notification = new Notification()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    NotificationId = Guid.NewGuid(),
                    Type = LabelHelper.NotificationType.Employee,
                    Who = CurrentUser.UserId,
                    What = string.Format(@"{0} (<a class=""cus-anchor"" href=""/UserInformation/?id={2}"">{2}</a>) created a new Ticket Ticket #{1}", "{0}", Model.Ticket.Id, employee.FirstOrDefault().UserLoginId),
                    NotificationUrl = AppConfig.DomainSitePath + "/Ticket/AddTicket/?Id=" + Model.Ticket.Id,
                };
                _Util.Facade.NotificationFacade.InsertNotification(notification);

                #endregion


                foreach (Guid item in AllUser)
                {
                    //if (item != CurrentUser.UserId)
                    //{
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(item);
                    if (emp != null && emp.Email.IsValidEmailAddress())
                    {
                        var userpermission = _Util.Facade.PermissionFacade.GetUserPermissionGroupByUserId(emp.UserId);
                        if (userpermission != null)
                        {
                            var ispermit = _Util.Facade.PermissionFacade.GetPermissionGroupMapByPermissionIdAndGroupId((userpermission.PermissionGroupId.HasValue ? userpermission.PermissionGroupId.Value : 0), UserPermissions.CustomerTicketPermission.SendAssignedEmail);
                            if (ispermit != null && ispermit.IsActive == true)
                            {
                                ToEmailList += string.Format("{0};", emp.Email);
                            }
                        }
                    }
                    #region set user to notification
                    NotificationUser nu = new NotificationUser()
                    {
                        NotificationId = notification.NotificationId,
                        IsRead = false,
                        NotificationPerson = item,
                    };
                    _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                    #endregion
                    //}

                }
                if (!string.IsNullOrWhiteSpace(ToEmailList))
                {
                    string CustomerName = "Self";

                    if (TicketCustomer != null)
                    {
                        CustomerName = TicketCustomer.FirstName + " " + TicketCustomer.LastName;
                        if (TicketCustomer.Type == LabelHelper.CustomerType.Commercial && !string.IsNullOrWhiteSpace(TicketCustomer.BusinessName))
                        {
                            CustomerName = TicketCustomer.BusinessName;
                        }
                    }
                    Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Ticket.CustomerId);

                    if (!String.IsNullOrWhiteSpace(Model.Ticket.Message))
                    {
                        Model.Ticket.Message = LabelHelper.HtmlToPlainText(Model.Ticket.Message);
                    }

                    TicketNotificationEmails TicketCreatedNotificationEmail = new TicketNotificationEmails()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedByName = CurrentUser.GetFullName(), 
                        TicketMessage = Model.Ticket.Message,
                        CreatedForCustomerName = CustomerName,
                        AppointmentEndTime = Model.Ticket.AppointmentEndTime,
                        AppointmentStartTime = Model.Ticket.AppointmentStartTime,
                        CompletionDate = Model.Ticket.CompletionDate,
                        CustomerAddress = MakeAddress(customer.Street, " " + customer.City, " " + customer.State, " " + customer.ZipCode, " " + customer.Country),

                        TicketNumber = string.Format("Ticket #{0}", Model.Ticket.Id),
                        ToEmail = ToEmailList,
                        Subject = string.Format("A new ticket has been created (Ticket #{0})", Model.Ticket.Id),
                        HeaderMessage = "A new ticket has been created",
                        BodyMessage = string.Format("A new ticket (Ticket #{0}) has been created by {1} for customer {2}", Model.Ticket.Id, CurrentUser.GetFullName(), CustomerName),
                        TicketUrl = string.Format("{2}{4}/Public/Open/?TicketId={0}&CompanyId={1}&Type={3}", Model.Ticket.TicketId, CurrentUser.CompanyId.Value, AppConfig.SiteDomain, LabelHelper.PublicOpenTypes.Ticket, AppConfig.DomainSitePath)
                    };
                    if (TicketCreatedNotificationEmail.AppointmentStartTime == "-1")
                    {
                        TicketCreatedNotificationEmail.AppointmentStartTime = "";
                    }
                    if (TicketCreatedNotificationEmail.AppointmentEndTime == "-1")
                    {
                        TicketCreatedNotificationEmail.AppointmentEndTime = "";
                    }
                    List<Lookup> lkk = new List<Lookup>();
                    if (TicketCreatedNotificationEmail.AppointmentStartTime != "-1" && TicketCreatedNotificationEmail.AppointmentStartTime != "" && !String.IsNullOrWhiteSpace(TicketCreatedNotificationEmail.AppointmentStartTime))
                    {
                        lkk = _Util.Facade.EmployeeFacade.GetLookupDisplaytext(TicketCreatedNotificationEmail.AppointmentStartTime);
                        TicketCreatedNotificationEmail.AppointmentStartTime = lkk.FirstOrDefault().DisplayText;
                    }
                    if (TicketCreatedNotificationEmail.AppointmentEndTime != "-1" && TicketCreatedNotificationEmail.AppointmentEndTime != "" && !String.IsNullOrWhiteSpace(TicketCreatedNotificationEmail.AppointmentEndTime))
                    {
                        lkk = _Util.Facade.EmployeeFacade.GetLookupDisplaytext(TicketCreatedNotificationEmail.AppointmentEndTime);
                        TicketCreatedNotificationEmail.AppointmentEndTime = lkk.FirstOrDefault().DisplayText;
                    }
                    _Util.Facade.MailFacade.SendTicketCreatedNotificationEmail(TicketCreatedNotificationEmail);
                }
                #endregion


            }
            else if (isAssingedChanged == true)
            {
                #region Insert notification
                Notification notification = new Notification()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    NotificationId = Guid.NewGuid(),
                    Type = LabelHelper.NotificationType.Employee,
                    Who = CurrentUser.UserId,
                    What = string.Format(@"{0} (<a class=""cus-anchor"" href=""/UserInformation/?id={2}"">{2}</a>) created a new Ticket Ticket #{1}", "{0}", Model.Ticket.Id, employee.FirstOrDefault().UserLoginId),
                    NotificationUrl = AppConfig.DomainSitePath + "/Ticket/AddTicket/?Id=" + Model.Ticket.Id,
                };
                _Util.Facade.NotificationFacade.InsertNotification(notification);
                #endregion
                foreach (Guid item in Model.Assigned)
                {
                    if (item != CurrentUser.UserId)
                    {
                        #region set user to notification
                        NotificationUser nu = new NotificationUser()
                        {
                            NotificationId = notification.NotificationId,
                            IsRead = false,
                            NotificationPerson = item,
                        };
                        _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                        #endregion
                    }
                    #region Set Assigned user email
                    var EmployeeDetails = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item);
                    if (EmployeeDetails != null)
                    {
                        string CustomerName = "";
                        if (TicketCustomer != null)
                        {
                            CustomerName = TicketCustomer.FirstName + " " + TicketCustomer.LastName;
                            if (TicketCustomer.Type == LabelHelper.CustomerType.Commercial && !string.IsNullOrWhiteSpace(TicketCustomer.BusinessName))
                            {
                                CustomerName = TicketCustomer.BusinessName;
                            }
                        }
                        Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Ticket.CustomerId);
                        if (!String.IsNullOrWhiteSpace(Model.Ticket.Message))
                        {
                            Model.Ticket.Message = LabelHelper.HtmlToPlainText(Model.Ticket.Message);
                        }
                        TicketNotificationEmails TicketCreatedNotificationEmail = new TicketNotificationEmails()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            CreatedByName = CurrentUser.GetFullName(),
                            TicketMessage = Model.Ticket.Message,
                            CreatedForCustomerName = CustomerName,
                            AppointmentEndTime = Model.Ticket.AppointmentEndTime,
                            AppointmentStartTime = Model.Ticket.AppointmentStartTime,
                            CompletionDate = Model.Ticket.CompletionDate,
                            CustomerAddress = MakeAddress(customer.Street, " " + customer.City, " " + customer.State, " " + customer.ZipCode, " " + customer.Country),
                            TicketNumber = string.Format("Ticket #{0}", Model.Ticket.Id),
                            ToEmail = EmployeeDetails.Email,
                            Subject = string.Format("A new ticket has been assigned (Ticket #{0})", Model.Ticket.Id),
                            HeaderMessage = "A new ticket has been created",
                            BodyMessage = string.Format("A new ticket (Ticket #{0}) has been created by {1} for customer {2}", Model.Ticket.Id, CurrentUser.GetFullName(), CustomerName),
                            TicketUrl = string.Format("{2}{4}/Public/Open/?TicketId={0}&CompanyId={1}&Type={3}", Model.Ticket.TicketId, CurrentUser.CompanyId.Value, AppConfig.SiteDomain, LabelHelper.PublicOpenTypes.Ticket, AppConfig.DomainSitePath)
                        };
                        if (TicketCreatedNotificationEmail.AppointmentStartTime == "-1")
                        {
                            TicketCreatedNotificationEmail.AppointmentStartTime = "";
                        }
                        if (TicketCreatedNotificationEmail.AppointmentEndTime == "-1")
                        {
                            TicketCreatedNotificationEmail.AppointmentEndTime = "";
                        }
                        List<Lookup> lkk = new List<Lookup>();
                        if (TicketCreatedNotificationEmail.AppointmentStartTime != "-1" && TicketCreatedNotificationEmail.AppointmentStartTime != "" && !String.IsNullOrWhiteSpace(TicketCreatedNotificationEmail.AppointmentStartTime))
                        {
                            lkk = _Util.Facade.EmployeeFacade.GetLookupDisplaytext(TicketCreatedNotificationEmail.AppointmentStartTime);
                            TicketCreatedNotificationEmail.AppointmentStartTime = lkk.FirstOrDefault().DisplayText;
                        }
                        if (TicketCreatedNotificationEmail.AppointmentEndTime != "-1" && TicketCreatedNotificationEmail.AppointmentEndTime != "" && !String.IsNullOrWhiteSpace(TicketCreatedNotificationEmail.AppointmentEndTime))
                        {
                            lkk = _Util.Facade.EmployeeFacade.GetLookupDisplaytext(TicketCreatedNotificationEmail.AppointmentEndTime);
                            TicketCreatedNotificationEmail.AppointmentEndTime = lkk.FirstOrDefault().DisplayText;
                        }
                        _Util.Facade.MailFacade.SendTicketCreatedNotificationEmail(TicketCreatedNotificationEmail);
                    }
                    #endregion
                }
            }

            #region EquipmentAddRelease
            List<CustomerAppointmentEquipment> TicketItemList = new List<CustomerAppointmentEquipment>();

            if (TicketIdStore != 0 && Model.CustomerAppointment != null && (Model.CustomerAppointment.CustomerAppointmentEquipmentList == null || Model.CustomerAppointment.CustomerAppointmentEquipmentList.Count() == 0))
            {
                _Util.Facade.CustomerAppoinmentFacade.DeleteAllCustomerAppointmentEquipmentByAppointmentId(Model.Ticket.TicketId);
                _Util.Facade.CustomerAppoinmentFacade.DeleteAllCustomerAppointmentServiceByAppointmentId(Model.Ticket.TicketId);
            }
            else
            {
                TicketItemList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipmentByTicketId(CurrentUser.CompanyId.Value, Model.Ticket.TicketId);
            }


             List<string> AddedEquipmentList = new List<string>();
            List<string> RemovedEquipmentList = new List<string>();

            if (Model.CustomerAppointment != null && Model.CustomerAppointment.CustomerAppointmentEquipmentList != null && Model.CustomerAppointment.CustomerAppointmentEquipmentList.Count() > 0)
            {
                var PackageEqpList = _Util.Facade.SmartPackageFacade.GetAllPackageEqpListByCustomerIdAndCompanyIdAndAppointmentId(Model.Ticket.CustomerId, CurrentUser.CompanyId.Value, Model.Ticket.Id);
                var PackageServiceList = _Util.Facade.SmartPackageFacade.GetAllPackageServiceListByCustomerIdAndCompanyIdAndAppointmentId(Model.Ticket.CustomerId, CurrentUser.CompanyId.Value, Model.Ticket.Id);
                if (PackageEqpList != null && PackageEqpList.Count > 0)
                {
                    foreach (var eqp in PackageEqpList)
                    {
                        _Util.Facade.PackageFacade.DeleteCustomerPackageEqpById(eqp.Id);
                    }
                }
                if (PackageServiceList != null && PackageServiceList.Count > 0)
                {
                    foreach (var service in PackageServiceList)
                    {
                        _Util.Facade.PackageFacade.DeleteCustomerPackageServiceById(service.Id);
                    }
                }
                foreach (CustomerAppointmentEquipment item in Model.CustomerAppointment.CustomerAppointmentEquipmentList)
                {
                    IsNewEqp = false;
                    if (item.Id > 0 && TicketItemList.Where(x => x.Id == item.Id).Count() > 0)
                    {
                        #region Update CustomerAppointmentEquipment
                        CustomerAppointmentEquipment tempitem = TicketItemList.Where(x => x.Id == item.Id).FirstOrDefault();
                        item.CreatedBy = tempitem.CreatedBy;
                        if (item.InstalledByUid == Guid.Empty)
                        {
                            item.InstalledByUid = tempitem.InstalledByUid;
                        }
                        if (item.CreatedByUid == Guid.Empty)
                        {
                            item.CreatedByUid = tempitem.CreatedByUid;
                        }
                        else if (item.CreatedByUid != tempitem.CreatedByUid)
                        {
                            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item.CreatedByUid);
                            Employee Previous = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(tempitem.CreatedByUid);
                            if (emp != null)
                            {
                                item.CreatedBy = emp.FirstName + " " + emp.LastName;

                                Equipment eqp = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(item.EquipmentId);
                                if (eqp != null)
                                {
                                    item.CreatedByUid = emp.UserId;
                                    string AddedItem = "<span data='{3}'><a class='cus-anchor' href='{0}'>{1}</a><br/> <span>Sales Person Changed From: <b>{2}</b> <br/>TO: <b>{4}</b> By: <b>{5}</b></span> <br/></span>";
                                    if (eqp.EquipmentClassId == 2)
                                    {
                                        //service///Inventory/ServiceDetail/2049
                                        AddedItem = string.Format(AddedItem,
                                            "/Inventory/ServiceDetail/" + eqp.Id, //0
                                            item.EquipName, //1
                                            Previous.FirstName + " " + Previous.LastName, //2
                                            "servicesold",//3
                                            item.CreatedBy,//4
                                            CurrentUser.GetFullName()
                                            );
                                    }
                                    else
                                    {
                                        //equipment/Inventory/EquipmentDetail/1631//itemssold
                                        AddedItem = string.Format(AddedItem,
                                           "/Inventory/EquipmentDetail/" + eqp.Id, //0
                                           item.EquipName, //1
                                           Previous.FirstName + " " + Previous.LastName, //2
                                           "itemssold",//3
                                           item.CreatedBy,//4
                                           CurrentUser.GetFullName()
                                           );
                                    }

                                    TicketReply tkrply = new TicketReply()
                                    {
                                        RepliedDate = DateTime.Now,
                                        TicketId = Model.Ticket.TicketId,
                                        UserId = CurrentUser.UserId,
                                        Message = AddedItem //string.Format(@"{0}", AddedItem, (EquipmentType == LabelHelper.EquipmentType.Service ? "servicesadded" : "itemsadded"))
                                    };
                                    tkrply.Id = _Util.Facade.TicketFacade.InsertTicketReply(tkrply);
                                }

                            }

                        }
                        if (item.QuantityLeftEquipment == null)
                        {
                            logger.WithProperty("tags", "CAE,update,Installed,IsNull").WithProperty("params", JsonConvert.SerializeObject(item)).Trace("Ticket Id {Id}", Model.Ticket.Id);
                            item.QuantityLeftEquipment = tempitem.QuantityLeftEquipment;
                        }
                        if (item.QuantityLeftEquipment < tempitem.QuantityLeftEquipment)
                        {
                            logger.WithProperty("tags", "CAE,update,Installed,Reduced").WithProperty("params", JsonConvert.SerializeObject(tempitem)).Trace("Ticket Id {Id}", item.TicketIntId);
                        }
                        if (string.IsNullOrEmpty(item.EquipName))
                        {
                            item.EquipName = item.EquipmentName;
                            item.EquipName = "??? TC 3121";
                            logger.WithProperty("tags", "CAE,update,Equipment,Name,IsNull").WithProperty("params", JsonConvert.SerializeObject(item)).Trace("Ticket Id {Id}", item.TicketIntId);
                        }
                        

                        item.CreatedDate = tempitem.CreatedDate;
                        item.IsAgreementItem = tempitem.IsAgreementItem;
                        item.IsBaseItem = tempitem.IsBaseItem;
                        item.AppointmentId = tempitem.AppointmentId;
                        item.IsBadInventory = tempitem.IsBadInventory;
                        item.IsDefaultService = tempitem.IsDefaultService;
                        item.OriginalUnitPrice = tempitem.OriginalUnitPrice;
                        item.IsEquipmentExist = tempitem.IsEquipmentExist;
                        item.IsNonCommissionable = tempitem.IsNonCommissionable;
                        logger.WithProperty("tags", "ticket,save,CAE").WithProperty("params", JsonConvert.SerializeObject(item))
                        .Trace($"The CAE record for {tempitem.Id} {tempitem.EquipmentName} in ticket #{item.TicketIntId} | 3138");
                        _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(item);

                        TicketItemList.Remove(tempitem);
                        #endregion

                        #region Update Installed Ticket Report
                        IndividualInstalledEquipment IndividualInstalled = _Util.Facade.CustomerFacade.GetIndividualInstalledEquipmentByTicketIdAndEquipmentIdAndAppointmentEquipmentId(Model.Ticket.Id, item.EquipmentId, item.Id);
                        if (IndividualInstalled != null)
                        {
                            _Util.Facade.CustomerFacade.DeleteIndividualInstalledEquipmentById(Model.Ticket.Id, item.EquipmentId, item.Id);
                        }

                        CustomerAppointmentEquipment CustomerApp = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentEquipmentByAppointmentIdAndEquipmentIdAndId(Model.Ticket.TicketId, item.EquipmentId, item.Id);
                        if (CustomerApp != null)
                        {
                            EquipmentType EquType = _Util.Facade.CustomerAppoinmentFacade.GetEquipmentTypeByAppointmentIdAndEquipmentId(Model.Ticket.TicketId, item.EquipmentId);
                            if (EquType == null)
                            {
                                EquType.Name = "";
                            }
                            Manufacturer Manufacturer = _Util.Facade.CustomerAppoinmentFacade.GetManufacturerByAppointmentIdAndEquipmentId(Model.Ticket.TicketId, item.EquipmentId);
                            if (Manufacturer == null)
                            {
                                Manufacturer.Name = "";
                            }
                            TicketType TicketType = _Util.Facade.CustomerAppoinmentFacade.GetTicketTypeByAppointmentIdAndEquipmentId(Model.Ticket.TicketId, item.EquipmentId);
                            if (TicketType == null)
                            {
                                TicketType.Type = "";
                            }
                            InstallerModel Installer = _Util.Facade.CustomerAppoinmentFacade.GetInstallerByAppointmentIdAndEquipmentId(Model.Ticket.TicketId, item.EquipmentId);
                            if (Installer == null)
                            {
                                Installer.Name = "";
                                Installer.Id = Guid.Empty;
                            }
                            CompanyCost CC = _Util.Facade.CustomerAppoinmentFacade.GetCompanyCostByAppointmentIdAndEquipmentId(Model.Ticket.TicketId, item.EquipmentId);
                            if (CC == null)
                            {
                                CC.Cost = 0.0;
                            }
                            EquipmentSKU Equipment = _Util.Facade.CustomerAppoinmentFacade.GetEquipmentSKUByAppointmentIdAndEquipmentId(Model.Ticket.TicketId, item.EquipmentId);
                            if (Equipment == null)
                            {
                                Equipment.SKU = "";
                                Equipment.Point = 0.0;
                            }
                            Count Attachments = _Util.Facade.CustomerAppoinmentFacade.GetAttachmentsCountAppointmentId(Model.Ticket.TicketId);
                            if (Attachments == null)
                            {
                                Attachments.TotalCount = 0;
                            }
                            Count Replies = _Util.Facade.CustomerAppoinmentFacade.GetRepliesCountAppointmentId(Model.Ticket.TicketId);
                            if (Replies == null)
                            {
                                Replies.TotalCount = 0;
                            }
                            Ticket tk = _Util.Facade.TicketFacade.GetTicketByTicketId(Model.Ticket.TicketId);
                            if (tk == null)
                            {
                                return Json(new { result = true, message = "Invalid data." });
                            }
                            Customer cus = _Util.Facade.CustomerAppoinmentFacade.GetCustomerByAppointmentId(Model.Ticket.TicketId);
                            if (cus == null)
                            {
                                return Json(new { result = true, message = "Invalid data." });
                            }
                            int i = 0;
                            while (i < CustomerApp.QuantityLeftEquipment)
                            {
                                IndividualInstalledEquipment Eq = new IndividualInstalledEquipment();
                                Eq.AppointmentEquipmentId = CustomerApp.Id;
                                Eq.Category = EquType.Name;
                                Eq.Manufacturer = Manufacturer.Name;
                                Eq.Description = item.EquipmentName;
                                Eq.TicketType = TicketType.Type;
                                Eq.EmpUser = Installer.Name;
                                Eq.TicketId = tk.Id;
                                Eq.RepliesCount = Replies.TotalCount;
                                Eq.AttachmentsCount = Attachments.TotalCount;
                                Eq.CusIdInt = cus.Id;
                                Eq.CustomerName = cus.Name;
                                Eq.CompletionDate = tk.CompletionDate;
                                Eq.SKU = Equipment.SKU;
                                Eq.TotalPoint = Equipment.Point;
                                Eq.IsClosed = tk.IsClosed.Value;
                                Eq.CompanyCost = CC.Cost;
                                Eq.CustomerCost = CustomerApp.UnitPrice;
                                Eq.Quantity = CustomerApp.Quantity;
                                Eq.InstalledEquipment = CustomerApp.QuantityLeftEquipment.Value;
                                Eq.Qty = 1;
                                Eq.Status = tk.Status;
                                Eq.CreatedBy = CurrentUser.UserId;
                                Eq.CreatedDate = DateTime.Now.UTCCurrentTime();
                                Eq.EquipmentId = item.EquipmentId;
                                Eq.InstalledByUid = Installer.Id;
                                _Util.Facade.CustomerFacade.InsertInIndividualInstalledEquipmentObj(Eq);

                                i++;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        IsNewEqp = true;
                        #region Insert CustomerAppointmentEquipment
                        item.CreatedBy = CurrentUser.GetFullName();
                        if (item.InstalledByUid == Guid.Empty)
                        {
                            item.InstalledByUid = item.TechnicianId;
                        }
                        if (item.CreatedByUid == Guid.Empty)
                        {
                            item.CreatedByUid = CurrentUser.UserId;
                        }
                        else
                        {
                            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item.CreatedByUid);
                            if (emp != null)
                            {
                                item.CreatedBy = emp.FirstName + " " + emp.LastName;
                            }
                        }
                        item.EquipmentName = string.IsNullOrEmpty(item.EquipmentName) ? "??? TC 3258" : item.EquipmentName;
                        item.CreatedDate = DateTime.Now.UTCCurrentTime();
                        item.AppointmentId = Model.Ticket.TicketId;
                        item.IsBaseItem = false;
                        item.IsAgreementItem = false;
                        _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppointmentEquipmentDetail(item);

                        #region Insert Installed Equipment Report
                        CustomerAppointmentEquipment CustomerApp = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentEquipmentByAppointmentIdAndEquipmentIdAndId(Model.Ticket.TicketId, item.EquipmentId, item.Id);
                        if (CustomerApp != null)
                        {
                            EquipmentType EquType = _Util.Facade.CustomerAppoinmentFacade.GetEquipmentTypeByAppointmentIdAndEquipmentId(Model.Ticket.TicketId, item.EquipmentId);
                            if (EquType == null)
                            {
                                EquType.Name = "";
                            }
                            Manufacturer Manufacturer = _Util.Facade.CustomerAppoinmentFacade.GetManufacturerByAppointmentIdAndEquipmentId(Model.Ticket.TicketId, item.EquipmentId);
                            if (Manufacturer == null)
                            {
                                Manufacturer.Name = "";
                            }
                            TicketType TicketType = _Util.Facade.CustomerAppoinmentFacade.GetTicketTypeByAppointmentIdAndEquipmentId(Model.Ticket.TicketId, item.EquipmentId);
                            if (TicketType == null)
                            {
                                TicketType.Type = "";
                            }
                            InstallerModel Installer = _Util.Facade.CustomerAppoinmentFacade.GetInstallerByAppointmentIdAndEquipmentId(Model.Ticket.TicketId, item.EquipmentId);
                            if (Installer == null)
                            {
                                Installer.Name = "";
                                Installer.Id = Guid.Empty;
                            }
                            CompanyCost CC = _Util.Facade.CustomerAppoinmentFacade.GetCompanyCostByAppointmentIdAndEquipmentId(Model.Ticket.TicketId, item.EquipmentId);
                            if (CC == null)
                            {
                                CC.Cost = 0.0;
                            }
                            EquipmentSKU Equipment = _Util.Facade.CustomerAppoinmentFacade.GetEquipmentSKUByAppointmentIdAndEquipmentId(Model.Ticket.TicketId, item.EquipmentId);
                            if (Equipment == null)
                            {
                                Equipment.SKU = "";
                                Equipment.Point = 0.0;
                            }
                            Count Attachments = _Util.Facade.CustomerAppoinmentFacade.GetAttachmentsCountAppointmentId(Model.Ticket.TicketId);
                            if (Attachments == null)
                            {
                                Attachments.TotalCount = 0;
                            }
                            Count Replies = _Util.Facade.CustomerAppoinmentFacade.GetRepliesCountAppointmentId(Model.Ticket.TicketId);
                            if (Replies == null)
                            {
                                Replies.TotalCount = 0;
                            }
                            Ticket tk = _Util.Facade.TicketFacade.GetTicketByTicketId(Model.Ticket.TicketId);
                            if (tk == null)
                            {
                                return Json(new { result = true, message = "Invalid data." });
                            }
                            Customer cus = _Util.Facade.CustomerAppoinmentFacade.GetCustomerByAppointmentId(Model.Ticket.TicketId);
                            if (cus == null)
                            {
                                return Json(new { result = true, message = "Invalid data." });
                            }

                            int i = 0;
                            while (i < CustomerApp.QuantityLeftEquipment)
                            {
                                IndividualInstalledEquipment Eq = new IndividualInstalledEquipment();
                                Eq.AppointmentEquipmentId = CustomerApp.Id;
                                Eq.Category = EquType.Name;
                                Eq.Manufacturer = Manufacturer.Name;
                                Eq.Description = item.EquipmentName;
                                Eq.TicketType = TicketType.Type;
                                Eq.EmpUser = Installer.Name;
                                Eq.TicketId = tk.Id;
                                Eq.RepliesCount = Replies.TotalCount;
                                Eq.AttachmentsCount = Attachments.TotalCount;
                                Eq.CusIdInt = cus.Id;
                                Eq.CustomerName = cus.Name;
                                Eq.CompletionDate = tk.CompletionDate;
                                Eq.SKU = Equipment.SKU;
                                Eq.TotalPoint = Equipment.Point;
                                Eq.IsClosed = tk.IsClosed.Value;
                                Eq.CompanyCost = CC.Cost;
                                Eq.CustomerCost = CustomerApp.UnitPrice;
                                Eq.Quantity = CustomerApp.Quantity;
                                Eq.InstalledEquipment = CustomerApp.QuantityLeftEquipment.Value;
                                Eq.Qty = 1;
                                Eq.Status = tk.Status;
                                Eq.CreatedBy = CurrentUser.UserId;
                                Eq.CreatedDate = DateTime.Now.UTCCurrentTime();
                                Eq.EquipmentId = item.EquipmentId;
                                Eq.InstalledByUid = Installer.Id;
                                _Util.Facade.CustomerFacade.InsertInIndividualInstalledEquipmentObj(Eq);

                                i++;
                            }
                        }

                        #endregion

                        #region AddedItems In ticket reply
                        string AddedItem = "";
                        Equipment eqp = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(item.EquipmentId);
                        if (eqp != null)
                        {
                            AddedItem = "<a class='cus-anchor' href='{0}'>{1}</a><br/> <span>Sold By: {2}</span> <br/>";
                            if (eqp.EquipmentClassId == 2)
                            {
                                //service///Inventory/ServiceDetail/2049
                                AddedItem = string.Format(AddedItem, "/Inventory/ServiceDetail/" + eqp.Id, item.EquipName, item.CreatedBy);
                            }
                            else
                            {
                                //equipment/Inventory/EquipmentDetail/1631
                                AddedItem = string.Format(AddedItem, "/Inventory/EquipmentDetail/" + eqp.Id, item.EquipName, item.CreatedBy);
                            }
                        }
                        else
                        {
                            AddedItem = item.EquipName;
                        }
                        AddedEquipmentList.Add(AddedItem);
                        #endregion

                        #endregion
                    }

                    var objpackagecustomer = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerId(Model.Ticket.CustomerId);
                    if (objpackagecustomer != null)
                    {
                        if (item.IsService == false)
                        {
                            CustomerPackageEqp CustomerPackageEqp = new CustomerPackageEqp()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                CustomerId = Model.Ticket.CustomerId,
                                PackageId = objpackagecustomer.CustomerId,
                                EquipmentId = item.EquipmentId,
                                IsIncluded = false,
                                IsDevice = false,
                                IsOptionalEqp = false,
                                Quantity = item.Quantity,
                                UnitPrice = item.UnitPrice,
                                DiscountUnitPricce = 0.0,
                                DiscountPckage = 0.0,
                                Total = item.TotalPrice,
                                IsServiceEquipment = false,
                                ServiceId = new Guid(),
                                IsTransfered = false,
                                IsEqpExist = false,
                                IsPackageEqp = false,
                                IsNonCommissionable = false,
                                AppointmentIntId = Model.Ticket.Id,
                                AppointmentEquipmentIntId = item.Id
                            };
                            _Util.Facade.PackageFacade.InsertCustomerPackageEqp(CustomerPackageEqp);
                        }
                        else
                        {
                            CustomerPackageService CustomerPackageService = new CustomerPackageService()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                CustomerId = Model.Ticket.CustomerId,
                                PackageId = objpackagecustomer.PackageId,
                                EquipmentId = item.EquipmentId,
                                MonthlyRate = item.UnitPrice,
                                DiscountRate = 0,
                                Total = item.TotalPrice,
                                ManufacturerId = new Guid(),
                                LocationId = new Guid(),
                                TypeId = new Guid(),
                                ModelId = new Guid(),
                                FinishId = new Guid(),
                                CapacityId = new Guid(),
                                IsPackageService = false,
                                IsNonCommissionable = false,
                                AppointmentIntId = Model.Ticket.Id,
                                AppointmentEquipmentIntId = item.Id
                            };
                            _Util.Facade.PackageFacade.InsertCustomerPackageService(CustomerPackageService);
                        }
                    }
                    else
                    {
                        if (item.IsService == false)
                        {
                            CustomerPackageEqp CustomerPackageEqp = new CustomerPackageEqp()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                CustomerId = Model.Ticket.CustomerId,
                                PackageId = new Guid(),
                                EquipmentId = item.EquipmentId,
                                IsIncluded = false,
                                IsDevice = false,
                                IsOptionalEqp = false,
                                Quantity = item.Quantity,
                                UnitPrice = item.UnitPrice,
                                DiscountUnitPricce = 0.0,
                                DiscountPckage = 0.0,
                                Total = item.TotalPrice,
                                IsServiceEquipment = false,
                                ServiceId = new Guid(),
                                IsTransfered = false,
                                IsEqpExist = false,
                                IsPackageEqp = false,
                                IsNonCommissionable = false,
                                AppointmentIntId = Model.Ticket.Id,
                                AppointmentEquipmentIntId = item.Id
                            };
                            _Util.Facade.PackageFacade.InsertCustomerPackageEqp(CustomerPackageEqp);
                        }
                        else
                        {
                            CustomerPackageService CustomerPackageService = new CustomerPackageService()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                CustomerId = Model.Ticket.CustomerId,
                                PackageId = new Guid(),
                                EquipmentId = item.EquipmentId,
                                MonthlyRate = item.UnitPrice,
                                DiscountRate = 0,
                                Total = item.TotalPrice,
                                ManufacturerId = new Guid(),
                                LocationId = new Guid(),
                                TypeId = new Guid(),
                                ModelId = new Guid(),
                                FinishId = new Guid(),
                                CapacityId = new Guid(),
                                IsPackageService = false,
                                IsNonCommissionable = false,
                                AppointmentIntId = Model.Ticket.Id,
                                AppointmentEquipmentIntId = item.Id
                            };
                            _Util.Facade.PackageFacade.InsertCustomerPackageService(CustomerPackageService);
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(Model.Ticket.Status) && Model.Ticket.Status == "Completed")
                {
                    var objticketlist = _Util.Facade.TicketFacade.GetTicketListByCustomerIdAndCompanyIdAndNotCompleted(Model.Ticket.CustomerId, CurrentUser.CompanyId.Value);
                    if (objticketlist != null && objticketlist.Count > 0)
                    {
                        foreach (var item in objticketlist)
                        {
                            var objappeqplist = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipmentByAppointmentIdAndIsBilling(item.TicketId);
                            if (objappeqplist != null && objappeqplist.Count > 0)
                            {
                                foreach (var app in objappeqplist)
                                {
                                    app.IsBilling = true;
                                    app.IsBillingProcess = true;
                                    logger.WithProperty("tags", "ticket,save,CAE").WithProperty("params", JsonConvert.SerializeObject(app))
                                    .Trace($"The CAE record for {app.Id} {app.EquipmentName} in ticket #{app.TicketIntId} | 3516");
                                    _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(app);
                                }
                            }
                        }
                    }
                    var objcompleteticketlist = _Util.Facade.TicketFacade.GetTicketListByCustomerIdAndCompanyIdAndIsCompleted(Model.Ticket.CustomerId, CurrentUser.CompanyId.Value);
                    if (objcompleteticketlist != null && objcompleteticketlist.Count > 0)
                    {
                        foreach (var item in objcompleteticketlist)
                        {
                            var objappeqplist1 = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipmentByAppointmentIdAndIsBilling(item.TicketId);
                            if (objappeqplist1 != null && objappeqplist1.Count > 0)
                            {
                                foreach (var app in objappeqplist1)
                                {
                                    app.IsBilling = false;
                                    logger.WithProperty("tags", "ticket,save,CAE").WithProperty("params", JsonConvert.SerializeObject(app))
                                    .Trace($"The CAE record for {app.Id} {app.EquipmentName} in ticket #{app.TicketIntId} | 3534");
                                    _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(app);
                                }
                            }
                        }
                    }
                    var totalbillamount = 0.0;
                    var objeqpappoint = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipmentByTicketId(CurrentUser.CompanyId.Value, Model.Ticket.TicketId).Where(m => m.IsService == true && m.IsARBEnabled == true && m.IsDefaultService == false);
                    if (objeqpappoint != null)
                    {
                        totalbillamount = objeqpappoint.Sum(m => m.TotalPrice);
                        foreach (var item in objeqpappoint)
                        {
                            var caeItem = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentEquipmentById(item.Id);
                            if (caeItem != null)
                            {
                                item.IsBilling = true;
                                item.IsBillingProcess = false;
                                logger.WithProperty("tags", "ticket,save,CAE").WithProperty("params", JsonConvert.SerializeObject(item))
                                .Trace($"The CAE record for {item.Id} {item.EquipmentName} in ticket #{item.TicketIntId} | 3553");
                                _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(item);
                            }
                        }
                    }
                    var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Ticket.CustomerId);
                    if (objcus != null && totalbillamount > 0)
                    {
                        #region ACH discount
                        double discountAmount = 0;
                        var objpayinfocus = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentInfoCustomerByCustomerIdAndPayFor(Model.Ticket.CustomerId);
                        if (objpayinfocus != null)
                        {
                            var objpayprofile = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentProfileByPaymentInfoId(objpayinfocus.PaymentInfoId);
                            if (objpayprofile != null && objpayprofile.Type.ToLower().IndexOf("ach") > -1)
                            {
                                var objglobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ACHDiscount");
                                if (objglobal != null)
                                {
                                    discountAmount = Convert.ToDouble(objglobal.Value);
                                }
                            }
                        }
                        #endregion
                        var totalbillamountTax = 0.0;
                        totalbillamount = totalbillamount - discountAmount;
                        GlobalSetting GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, Model.Ticket.CustomerId);
                        if (GetSalesTax != null)
                        {
                            totalbillamountTax = Math.Round((totalbillamount * Convert.ToDouble(GetSalesTax.Value)) / 100, 2);
                        }
                        objcus.MonthlyMonitoringFee = totalbillamount.ToString("#.##");
                        objcus.BillAmount = defaultBillTaxVal ? totalbillamount + totalbillamountTax : totalbillamount;
                        objcus.TotalTax = defaultBillTaxVal ? totalbillamountTax : 0;
                        objcus.BillTax = defaultBillTaxVal;
                        _Util.Facade.CustomerFacade.UpdateCustomer(objcus);
                    }
                }
            }

            #region If item still exists delete
            if (TicketItemList.Count() > 0)
            {
                bool IsServiceItems = Model.EquipmentType == LabelHelper.EquipmentType.Service;

                foreach (var item in TicketItemList)
                {
                    if (IsServiceItems == item.IsService)
                    {
                        _Util.Facade.CustomerAppoinmentFacade.DeleteCustomerAppoinmentEquipment(item.Id);
                        #region Removed Item In ticket reply
                        string RemovedItem = "";
                        Equipment eqp = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(item.EquipmentId);
                        if (eqp != null)
                        {
                            RemovedItem = "<a class='cus-anchor' href='{0}'>{1}</a>";
                            if (eqp.EquipmentClassId == 2)
                            {
                                //service///Inventory/ServiceDetail/2049
                                RemovedItem = string.Format(RemovedItem, "/Inventory/ServiceDetail/" + eqp.Id, item.EquipName);
                            }
                            else
                            {
                                //equipment/Inventory/EquipmentDetail/1631
                                RemovedItem = string.Format(RemovedItem, "/Inventory/EquipmentDetail/" + eqp.Id, item.EquipName);
                            }
                        }
                        else
                        {
                            RemovedItem = item.EquipName;
                        }
                        RemovedEquipmentList.Add(RemovedItem);
                        #endregion
                    }
                }
            }
            #endregion

            #endregion

            #region Added/Removed Equipment to Ticket Reply
            if (AddedEquipmentList.Count() > 0)
            {
                //AddedEquipmentList.RemoveAt(0);
                string EquipmentList = string.Join("", AddedEquipmentList);
                TicketReply tkrply = new TicketReply()
                {
                    RepliedDate = DateTime.Now.UTCCurrentTime(),
                    TicketId = Model.Ticket.TicketId,
                    UserId = CurrentUser.UserId,
                    Message = string.Format(@"<span data='{1}'>{0}</span>", EquipmentList, (Model.EquipmentType == LabelHelper.EquipmentType.Service ? "servicesadded" : "itemsadded"))
                };
                tkrply.Id = _Util.Facade.TicketFacade.InsertTicketReply(tkrply);
            }
            if (RemovedEquipmentList.Count() > 0)
            {
                //RemovedEquipmentList.RemoveAt(0);
                string EquipmentList = string.Join(", ", RemovedEquipmentList);
                TicketReply tkrply = new TicketReply()
                {
                    RepliedDate = DateTime.Now.UTCCurrentTime(),
                    TicketId = Model.Ticket.TicketId,
                    UserId = CurrentUser.UserId,
                    Message = string.Format(@"<span data='{1}'>{0}</span>", EquipmentList, (Model.EquipmentType == LabelHelper.EquipmentType.Service ? "servicesremoved" : "itemsremoved"))
                };
                tkrply.Id = _Util.Facade.TicketFacade.InsertTicketReply(tkrply);
            }
            #endregion

            #region Tech inventory
            if (Model.CustomerAppointment != null && Model.CustomerAppointment.CustomerAppointmentEquipmentList != null && Model.CustomerAppointment.CustomerAppointmentEquipmentList.Count > 0)
            {
                #region Completed Ticket
                GlobalSetting globset = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ReleaseAllEquipmentForTicketCompleted");
                if (globset != null && globset.Value.ToLower() == "true" && PreviousTicketStatus != LabelHelper.TicketStatus.Completed && Model.Ticket.Status == LabelHelper.TicketStatus.Completed)
                {
                    // List<CustomerAppointmentEquipment> CustomerAppointmentEquipmentList = GetCustomerAppointmentEquipment(Model.Ticket.TicketId, Model.CustomerAppointment.CustomerAppointmentEquipmentList.EquipmentIdList ,
                    // Model.CustomerAppointment.CustomerAppointmentEquipmentList.IdList)
                    // Total 1,000,000
                    // 8
                    //  List<CustomerAppointmentEquipment> result 8... 
                    // Call Database for single

                    ///Same as availableCountTech, alreadyReleaseCount
                    /// Fast Coding Logic



                    ///List<TicketUser> TicketUserList = GetTicketUser( 
                    TicketUser tikuser = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndPrimary(Model.Ticket.TicketId);
                    foreach (var item in Model.CustomerAppointment.CustomerAppointmentEquipmentList)
                    {
                        //8 Times ... 

                        item.QuantityLeftEquipment = item.Quantity;

                        int availableCountTech = 0;
                        int alreadyReleaseCount = 0;
                        int releaseCount = 0;
                        bool eqpRelease = false;
                        CustomerAppointmentEquipment CusAptEqpListDetails = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentEquipmentByAppoinmentIdAndEquipmentIdAndId(Model.Ticket.TicketId, item.EquipmentId, item.Id);
                        //CustomerAppointmentEquipmentList use this using LINQ
                        if (CusAptEqpListDetails != null && CusAptEqpListDetails.IsEquipmentRelease == false)
                        {

                            availableCountTech = _Util.Facade.InventoryFacade.InventoryTechAvailableCount(tikuser.UserId, item.EquipmentId);
                            alreadyReleaseCount = _Util.Facade.InventoryFacade.AlreadyReleaseCountByCAEId(item.Id);

                            releaseCount = item.QuantityLeftEquipment.Value - alreadyReleaseCount;
                            if (releaseCount > availableCountTech)
                            {
                                releaseCount = availableCountTech;
                                item.QuantityLeftEquipment = availableCountTech + alreadyReleaseCount;

                                //CusAptEqpListDetails.IsEquipmentRelease = false;
                                //CusAptEqpListDetails.QuantityLeftEquipment = item.QuantityLeftEquipment;
                                //_Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(CusAptEqpListDetails);
                            }
                            if (item.QuantityLeftEquipment != alreadyReleaseCount && releaseCount > 0)
                            {
                                if (item.QuantityLeftEquipment >= item.Quantity)
                                {
                                    eqpRelease = true;
                                }
                                CusAptEqpListDetails.IsEquipmentRelease = eqpRelease;
                                CusAptEqpListDetails.QuantityLeftEquipment = item.QuantityLeftEquipment;
                                logger.WithProperty("tags", "ticket,save,CAE").WithProperty("params", JsonConvert.SerializeObject(CusAptEqpListDetails))
                                .Trace($"The CAE record for {CusAptEqpListDetails.Id} {CusAptEqpListDetails.EquipmentName} in ticket #{CusAptEqpListDetails.TicketIntId} | 3516");
                                _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(CusAptEqpListDetails);

                                if (CusAptEqpListDetails.IsEquipmentExist.Value)
                                {
                                    InventoryTech invtech = new InventoryTech()
                                    {
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        TechnicianId = item.TechnicianId,
                                        EquipmentId = item.EquipmentId,
                                        Type = LabelHelper.InventoryType.Release,
                                        Quantity = Math.Abs(releaseCount),
                                        LastUpdatedBy = CurrentUser.UserId,
                                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                        Description = "Release from technician by ticket",
                                        CustomerAppointmentEquipmentId = item.Id
                                    };
                                    _Util.Facade.InventoryFacade.InsertInventoryTech(invtech);
                                }

                                #region User Taged Email
                                Equipment eqp = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(item.EquipmentId);
                                if (eqp != null && !string.IsNullOrEmpty(eqp.TaggedEmail))
                                {
                                    string CustomerName = customerobj.FirstName + " " + customerobj.LastName;
                                    if (customerobj.Type == LabelHelper.CustomerType.Commercial && !string.IsNullOrWhiteSpace(customerobj.BusinessName))
                                    {
                                        CustomerName = customerobj.BusinessName;
                                    }
                                    TicketNotificationEmails TicketCreatedNotificationEmail = new TicketNotificationEmails()
                                    {
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        CreatedByName = CurrentUser.GetFullName(),
                                        TicketMessage = Model.Ticket.Message,
                                        CreatedForCustomerName = CustomerName,
                                        AppointmentEndTime = Model.Ticket.AppointmentEndTime,
                                        CustomerAddress = MakeAddress(TicketCustomer.Street, TicketCustomer.City, TicketCustomer.State, TicketCustomer.ZipCode, TicketCustomer.Country),
                                        AppointmentStartTime = Model.Ticket.AppointmentStartTime,
                                        CompletionDate = Model.Ticket.CompletionDate,
                                        ToEmail = eqp.TaggedEmail,
                                        Subject = string.Format("{0} has Released from technician by ticket(#{1})", item.EquipmentName, Model.Ticket.Id),
                                        HeaderMessage = item.QuantityLeftEquipment + " " + item.EquipmentName + " has released from technician.",

                                        TicketNumber = Model.Ticket.Id.ToString(),
                                        BodyMessage = string.Format(" {0}  {1} has released from technician by ticket(#{2}) for {3}", item.QuantityLeftEquipment, item.EquipmentName, Model.Ticket.Id.ToString(), CustomerName)

                                    };
                                    _Util.Facade.MailFacade.SendEqpReleasedNotificationEmail(TicketCreatedNotificationEmail);
                                }
                                #endregion
                            }
                        }
                    }
                }
                #endregion
                foreach (var item in Model.CustomerAppointment.CustomerAppointmentEquipmentList)
                {
                    string InventoryTechDesc = "Release from technician by ticket";
                    CustomerAppointmentEquipment CusAptEqpListDetails = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentEquipmentByAppoinmentIdAndEquipmentIdAndId(Model.Ticket.TicketId, item.EquipmentId, item.Id);
                    TicketUser tikuser = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndPrimary(Model.Ticket.TicketId);
                    int alreadyReleaseCount = _Util.Facade.InventoryFacade.AlreadyReleaseCountByCAEId(item.Id);
                    if ((item.QuantityLeftEquipment != null && item.QuantityLeftEquipment > 0) || alreadyReleaseCount > 0)
                    {
                        int availableCountTech = 0;

                        int releaseCount = 0;
                        bool eqpRelease = false;

                        if (string.IsNullOrEmpty(item.EquipmentName))
                        {
                            item.EquipmentName = "❓=== Unknown Equipment Name ===";
                        }

                        int multipleEQentries = Model.CustomerAppointment.CustomerAppointmentEquipmentList.Count(x => x.EquipmentId == item.EquipmentId);

                        if (multipleEQentries > 1)
                        {
                            //List<CustomerAppointmentEquipment> customerAppointmentEquipment = _Util.Facade.CustomerAppoinmentFacade.GetAllAppointmentEquipmentByAppointmentIdandEquipmentId(item.EquipmentId, Model.Ticket.TicketId);
                            //foreach (var caeItem in customerAppointmentEquipment)
                            //{

                            //}

                            //item.EquipmentName = $"*️⃣ {item.EquipmentName} {CurrentUser.GetFullName()}";
                            logger.WithProperty("tags", "ticket,save,multipleEQentries").WithProperty("params", item.EquipmentId)
                            .Trace($"Multiple entries for equipment {item.EquipmentName} in ticket #{item.TicketIntId}");
                        }

                        //CustomerAppointmentEquipmentList use this using LINQ
                        //if (CusAptEqpListDetails != null && CusAptEqpListDetails.IsEquipmentRelease == false)
                        if (CusAptEqpListDetails != null)
                        {
                            availableCountTech = _Util.Facade.InventoryFacade.InventoryTechAvailableCount(tikuser.UserId, item.EquipmentId);
                            //alreadyReleaseCount = _Util.Facade.InventoryFacade.AlreadyReleaseCountByCAEId(item.Id);
                            releaseCount = item.QuantityLeftEquipment.Value - alreadyReleaseCount;


                            #region DG Autofix Inventory installed but not removed from truck
                            //if (CusAptEqpListDetails.IsEquipmentRelease == true)
                            //{
                            //    //if (item.QuantityLeftEquipment.Value == null) item.QuantityLeftEquipment.Value = 0;

                            //}
                            if (availableCountTech < 0)
                            {
                                availableCountTech = 0;
                            }
                            if (CusAptEqpListDetails.IsEquipmentExist != true)
                            {
                                if (releaseCount > availableCountTech)
                                {
                                    Message += $"{item.EquipmentName} has less qty in stock then installed. {releaseCount}|{availableCountTech} <br/>";
                                    Proceed = false;
                                }
                            }
                            //InventoryTech objinvtech = _Util.Facade.EquipmentFacade.NewGetAllInventoryTechByTechnicianIdAndEquipmentIdAndType(item.TechnicianId, item.EquipmentId);
                            //if (objinvtech != null)
                            //{
                            //    if (item.QuantityLeftEquipment > objinvtech.Quantity)
                            //    {
                            //        item.QuantityLeftEquipment = objinvtech.Quantity;
                            //    }
                            //    _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(item);
                            //}
                            
                            //If quantity is reduced after installation then recover installed
                            if (CusAptEqpListDetails.QuantityLeftEquipment > item.Quantity)
                            {
                                releaseCount = item.Quantity - alreadyReleaseCount;
                                InventoryTechDesc = "Quantity adjustment by technician in ticket";
                            }
                            if (item.QuantityLeftEquipment > item.Quantity)
                            {
                                item.QuantityLeftEquipment = item.Quantity;
                            }

                            if (item.QuantityLeftEquipment == item.Quantity)
                            {
                                eqpRelease = true;
                            }
                            if (item.QuantityLeftEquipment.Value < item.Quantity || item.Quantity == 0 || item.QuantityLeftEquipment.Value == 0)
                            {
                                eqpRelease = false;
                            }

                            if (item.QuantityLeftEquipment.Value < alreadyReleaseCount)
                            {

                            }
                            #endregion

                            CusAptEqpListDetails.IsEquipmentRelease = eqpRelease;
                            CusAptEqpListDetails.QuantityLeftEquipment = item.QuantityLeftEquipment;
                            //_Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(CusAptEqpListDetails);

                            //Optimization required

                            //if (releaseCount > availableCountTech)
                            //{
                            //    releaseCount = availableCountTech;
                            //    item.QuantityLeftEquipment = alreadyReleaseCount + releaseCount ;
                            //}

                            if (item.QuantityLeftEquipment != alreadyReleaseCount && Proceed)
                            {


                                //if (item.QuantityLeftEquipment > item.Quantity)
                                //{
                                //    //CusAptEqpListDetails.Quantity = item.QuantityLeftEquipment.Value;

                                //    //If QuantityLeftEquipment is more than Quantity then
                                //    //Check if QuantityLeftEquipment saved is more than Quantity



                                //    //if (CusAptEqpListDetails.QuantityLeftEquipment > item.QuantityLeftEquipment)
                                //    //{
                                //    //    item.QuantityLeftEquipment = item.Quantity;
                                //    //}
                                //    item.QuantityLeftEquipment = item.Quantity;

                                //}
                                if (CusAptEqpListDetails.IsEquipmentExist != true)
                                {
                                    if (releaseCount > 0)
                                    {
                                        InventoryTechDesc = InventoryTechDesc ?? "Release from technician by ticket";
                                        InventoryTech invtech = new InventoryTech()
                                        {
                                            CompanyId = CurrentUser.CompanyId.Value,
                                            TechnicianId = item.TechnicianId,
                                            EquipmentId = item.EquipmentId,
                                            Type = LabelHelper.InventoryType.Release,
                                            Quantity = Math.Abs(releaseCount),
                                            LastUpdatedBy = CurrentUser.UserId,
                                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                            Description = InventoryTechDesc,
                                            CustomerAppointmentEquipmentId = item.Id
                                        };
                                        _Util.Facade.InventoryFacade.InsertInventoryTech(invtech);
                                    }
                                    else
                                    {
                                        InventoryTechDesc = InventoryTechDesc ?? "Adjustment by technician in ticket";
                                        InventoryTech invtech = new InventoryTech()
                                        {
                                            CompanyId = CurrentUser.CompanyId.Value,
                                            TechnicianId = item.TechnicianId,
                                            EquipmentId = item.EquipmentId,
                                            Type = LabelHelper.InventoryType.Add,
                                            Quantity = Math.Abs(releaseCount),
                                            LastUpdatedBy = CurrentUser.UserId,
                                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                            Description = InventoryTechDesc,
                                            CustomerAppointmentEquipmentId = item.Id
                                        };
                                        _Util.Facade.InventoryFacade.InsertInventoryTech(invtech);
                                    }
                                }


                                #region User Taged Email
                                Equipment eqp = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(item.EquipmentId);
                                if (eqp != null && !string.IsNullOrEmpty(eqp.TaggedEmail))
                                {
                                    string CustomerName = customerobj.FirstName + " " + customerobj.LastName;
                                    if (customerobj.Type == LabelHelper.CustomerType.Commercial && !string.IsNullOrWhiteSpace(customerobj.BusinessName))
                                    {
                                        CustomerName = customerobj.BusinessName;
                                    }
                                    TicketNotificationEmails TicketCreatedNotificationEmail = new TicketNotificationEmails()
                                    {
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        CreatedByName = CurrentUser.GetFullName(),
                                        TicketMessage = Model.Ticket.Message,
                                        CreatedForCustomerName = CustomerName,
                                        AppointmentEndTime = Model.Ticket.AppointmentEndTime,
                                        CustomerAddress = MakeAddress(TicketCustomer.Street, TicketCustomer.City, TicketCustomer.State, TicketCustomer.ZipCode, TicketCustomer.Country),
                                        AppointmentStartTime = Model.Ticket.AppointmentStartTime,
                                        CompletionDate = Model.Ticket.CompletionDate,
                                        ToEmail = eqp.TaggedEmail,
                                        Subject = string.Format("{0} has Released from technician by ticket(#{1})", item.EquipmentName, Model.Ticket.Id),
                                        HeaderMessage = item.QuantityLeftEquipment + " " + item.EquipmentName + " has released from technician.",

                                        TicketNumber = Model.Ticket.Id.ToString(),
                                        BodyMessage = string.Format(" {0}  {1} has released from technician by ticket(#{2}) for {3}", item.QuantityLeftEquipment, item.EquipmentName, Model.Ticket.Id.ToString(), CustomerName)

                                    };
                                    _Util.Facade.MailFacade.SendEqpReleasedNotificationEmail(TicketCreatedNotificationEmail);
                                }
                                #endregion
                            }
                            //if (item.QuantityLeftEquipment == item.Quantity)
                            //{
                            //    CusAptEqpListDetails.IsEquipmentRelease = true;

                            //}
                            logger.WithProperty("tags", "ticket,save,CAE").WithProperty("params", JsonConvert.SerializeObject(CusAptEqpListDetails))
                            .Trace($"The CAE record for {item.Id} {item.EquipmentName} in ticket #{item.TicketIntId} | 3942");
                            if (Proceed)
                            {
                                _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(CusAptEqpListDetails);
                            }

                        }
                    }
                }
            }
            #endregion

            #region When Ticket is Completed 
            if (!string.IsNullOrEmpty(Model.amount))
            {
                #region Add Invoice for ticket
                double TaxAmount = 0;
                Guid tempCustomerId = new Guid();
                if (Model != null)
                {
                    tempCustomerId = Model.Ticket.CustomerId;
                }
                GlobalSetting TaxGlobal = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, tempCustomerId);
                string TaxPercentage = TaxGlobal.Value;
                Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Ticket.CustomerId);
                TaxAmount = Math.Round((Convert.ToDouble(Model.amount) * (Convert.ToDouble(TaxPercentage) / 100)), 2);

                Invoice inv = new Invoice()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    InvoiceFor = cus.PaymentMethod,
                    Amount = Convert.ToDouble(Model.amount),
                    TotalAmount = Convert.ToDouble(Model.amount) + TaxAmount,
                    Tax = TaxAmount,
                    Balance = Convert.ToDouble(Model.amount) + TaxAmount,
                    RefType = Model.Ticket.Id.ToString(),
                    Status = LabelHelper.InvoiceStatus.Open,
                    InvoiceDate = DateTime.Today,
                    DueDate = DateTime.Today.AddMonths(1).AddDays(-1),
                    CreatedBy = CurrentUser.UserId.ToString(),
                    CreatedDate = DateTime.Today,
                    LastUpdatedDate = DateTime.Now,
                    IsBill = false,
                    IsEstimate = false,
                    LateAmount = 0,
                    LateFee = 0,
                    BalanceDue = Convert.ToDouble(Model.amount) + TaxAmount,
                    CustomerName = cus.FirstName + " " + cus.LastName,
                    CustomerId = cus.CustomerId,

                    DiscountAmount = 0,

                    Description = Model.desc,

                    CreatedByUid = cus.CustomerId,
                    LastUpdatedByUid = CurrentUser.UserId,

                };
                inv.Id = _Util.Facade.InvoiceFacade.InsertInvoice(inv);
                inv.InvoiceId = inv.Id.GenerateInvoiceNo();
                _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
                #region Invoice note insert
                InvoiceNote invNote = new InvoiceNote()
                {
                    AddedBy = CurrentUser.UserId,
                    AddedDate = DateTime.Now.UTCCurrentTime(),
                    CompanyId = CurrentUser.CompanyId.Value,
                    InvoiceId = inv.Id,
                    Note = string.Format("Invoice created from Ticket#{0}", Model.Ticket.Id),

                };
                _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(invNote);
                #endregion
                #endregion

                Model.Ticket.HasInvoice = true;
                _Util.Facade.TicketFacade.UpdateTicket(Model.Ticket);
                invId = inv.Id;
                double Amount = 0;
                string description = "installation expense";
                #region Add invoice details
                List<CustomerAppointmentEquipment> cusEqpList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(Model.Ticket.TicketId);
                InvoiceDetail invDetail = new InvoiceDetail();
                if (cusEqpList.Count > 0)
                {
                    foreach (var item in cusEqpList)
                    {

                        invDetail = new InvoiceDetail()
                        {
                            InvoiceId = inv.InvoiceId,
                            CompanyId = CurrentUser.CompanyId.Value,
                            InventoryId = Guid.Empty,
                            EquipmentId = item.EquipmentId,
                            EquipName = item.EquipName,
                            Quantity = item.Quantity,
                            EquipDetail = item.EquipDetail,
                            UnitPrice = 0,
                            TotalPrice = 0,
                            CreatedBy = CurrentUser.FirstName + " " + CurrentUser.LastName,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            Taxable = true
                        };
                        _Util.Facade.InvoiceFacade.InsertInvoiceDetails(invDetail);
                        Equipment equip = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(item.EquipmentId);
                        if (equip != null)
                        {
                            if (equip.Cost == null)
                            {
                                equip.Cost = 0;
                            }
                            Amount = Amount + Convert.ToDouble(equip.Cost);
                        }
                    }
                    #region Add Expense
                    TransactionExpense trans = new TransactionExpense()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = Model.Ticket.CustomerId,
                        Amount = Amount,
                        Status = Model.Ticket.Status,
                        ExpenseDate = DateTime.Now.UTCCurrentTime(),
                        ReferenceNo = "TICKETNO" + Model.Ticket.Id,
                        Description = description,
                        CreatedBy = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        RefType = Model.Ticket.Id.ToString(),
                        ExpenseType = "Ticket",
                        Type = LabelHelper.TransactionExpenseType.Automated
                    };
                    _Util.Facade.TransactionFacade.InsertTransactionExpense(trans);
                    #endregion
                }
                #endregion

                #region Manual Cost entry 
                invDetail = new InvoiceDetail()
                {
                    InvoiceId = inv.InvoiceId,
                    CompanyId = CurrentUser.CompanyId.Value,
                    InventoryId = Guid.Empty,
                    EquipmentId = Guid.Empty,
                    EquipName = Model.Ticket.TicketType,
                    EquipDetail = Model.desc,
                    Quantity = 1,
                    UnitPrice = Convert.ToDouble(Model.amount),
                    TotalPrice = Convert.ToDouble(Model.amount),
                    CreatedBy = CurrentUser.FirstName + " " + CurrentUser.LastName,
                    CreatedDate = DateTime.Now.UTCToClientTime(),
                    Taxable = true
                };
                _Util.Facade.InvoiceFacade.InsertInvoiceDetails(invDetail);
                #endregion

            }
            #endregion

            #region Insert into Expense
            if (Model.Ticket.Status == LabelHelper.TicketStatus.Closed)
            {
                string EmployeeIds = "";
                if (Model.Assigned != null)
                {
                    foreach (var item in Model.Assigned)
                    {
                        EmployeeIds += string.Format("'{0}',", item.ToString());
                    }
                }
                if (Model.UserList != null)
                {
                    foreach (var item in Model.UserList)
                    {
                        EmployeeIds += string.Format("'{0}',", item.ToString());
                    }
                }
                EmployeeIds = EmployeeIds.Remove(EmployeeIds.Length - 1);
                List<Employee> empList = _Util.Facade.EmployeeFacade.GetAllEmployeeByEmpIdList(EmployeeIds);
                if (empList.Count > 0)
                {
                    TicketTimeClock tclock = _Util.Facade.TicketClockFacade.GetLastClockOutByUserIdAndTicketId(Model.Ticket.CustomerId, Model.Ticket.TicketId).FirstOrDefault();
                    if (tclock != null)
                    {
                        double TotalTimeInHour = Convert.ToDouble(tclock.ClockedInMinutes.Value) / (1000 * 60 * 60);
                        double TotalExpense = 0;
                        TransactionExpense tempExp = new TransactionExpense();

                        double HourlyRate = 0;
                        foreach (var item in empList)
                        {
                            HourlyRate = item.HourlyRate.HasValue ? item.HourlyRate.Value : 0;

                            tempExp.Description = Model.Ticket.TicketType + " Expense For Ticket#" + Model.Ticket.Id + "<br>" + item.FirstName + " " + item.LastName + " $" + Math.Round((HourlyRate * TotalTimeInHour), 2) + "(Hourly $" + Math.Round(HourlyRate, 2) + " rate)";
                            tempExp.Amount = Math.Round((HourlyRate * TotalTimeInHour), 2);
                            tempExp.Status = "Pending";
                            tempExp.Type = LabelHelper.TransactionExpenseType.Automated;
                            tempExp.ExpenseDate = DateTime.Now.UTCCurrentTime();
                            tempExp.CreatedDate = DateTime.Now.UTCCurrentTime();
                            tempExp.CreatedBy = CurrentUser.UserId;
                            tempExp.CustomerId = Model.Ticket.CustomerId;
                            tempExp.CompanyId = CurrentUser.CompanyId.Value;
                            tempExp.UserId = item.UserId;
                            _Util.Facade.TransactionFacade.InsertTransactionExpense(tempExp);
                        }
                    }
                }
            }
            #endregion

            bool IsCommissionCalculation = false;
            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "CommissionCalculationInTicket");
            if (glob != null && glob.Value == "true")
            {
                IsCommissionCalculation = true;
            }
            ViewBag.IsCommissionCalculation = IsCommissionCalculation;

            bool IsBrinksCommissionCalculation = false;
            GlobalSetting globBrinks = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "CommissionCalculationBrinks");
            if (globBrinks != null && globBrinks.Value == "true")
            {
                IsBrinksCommissionCalculation = true;
            }
            ViewBag.IsBrinksCommissionCalculation = IsBrinksCommissionCalculation;
            #region End Calculations
            //If the ticket is created from booking, there is no need to calculate any commission
            var ticketDetails = _Util.Facade.TicketFacade.GetTicketById(Model.Ticket.Id);
            if (string.IsNullOrWhiteSpace(Model.BookingId) && IsCommissionCalculation && ticketDetails.IsPayrollClosed != true)
            {
                if (Model.Ticket.ReferenceTicketId > 0)
                {
                    CalculateFollowUpCommission(Model.Ticket);
                }
                else
                {
                    CalculateSalesCommission(Model.Ticket);
                    CalculateTechCommission(Model.Ticket);
                    CalculateAddMemberCommission(Model.Ticket);
                    CalculateFinRepCommission(Model.Ticket);
                    CalculateServiceCallCommission(Model.Ticket);
                }
            }
            else if (IsBrinksCommissionCalculation)
            {
                CalculatePayrollBrinks(Model.Ticket);
            }
            if (Model.Ticket.IsClosed == true)
            {
                ticketDetails.IsPayrollClosed = true;
                _Util.Facade.TicketFacade.UpdateTicket(ticketDetails);
            }
            #endregion

            #region For Rug only

            #region Ticket Customer Notification for RUG
            if (Model.IsStatusChange.HasValue && Model.IsStatusChange.Value == true)
            {
                if (Model.Ticket != null)
                {
                    #region Technician Name
                    string TechnicianName = "";
                    if (Model.Assigned != null && Model.Assigned.Length > 0)
                    {
                        Guid AssignedEmployee = new Guid();
                        AssignedEmployee = Model.Assigned[0];
                        Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(AssignedEmployee);
                        if (emp != null)
                        {
                            TechnicianName = emp.FirstName + " " + emp.LastName;
                        }
                    }
                    #endregion
                    var objnotification = _Util.Facade.TicketFacade.GetTicketCustomerNotificationByStatusAndType(Model.Ticket.Status.Replace(" ", ""), Model.Ticket.TicketType.Replace(" ", ""));
                    if (objnotification != null)
                    {
                        var objcustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Ticket.CustomerId);
                        if (objcustomer != null)
                        {
                            if (Model.NotifyCustomer.HasValue && Model.NotifyCustomer.Value == true && objcustomer.PreferedEmail == true && !string.IsNullOrWhiteSpace(objcustomer.EmailAddress) && objcustomer.EmailAddress.IsValidEmailAddress() && !string.IsNullOrWhiteSpace(objnotification.Email))
                            {
                                TicketCustomerNotificationEmail TicketCustomerNotificationEmail = new TicketCustomerNotificationEmail();
                                TicketCustomerNotificationEmail.CustomerName = objcustomer.FirstName + " " + objcustomer.LastName;
                                TicketCustomerNotificationEmail.ToEmail = objcustomer.EmailAddress;
                                TicketCustomerNotificationEmail.Body = objnotification.Email;
                                _Util.Facade.MailFacade.TicketCustomerNotificationmail(TicketCustomerNotificationEmail, CurrentUser.CompanyId.Value);
                                Message += " And send notification email successfully.";
                            }
                            //xvsd
                            string textMessage = "";
                            Hashtable datatemplate = new Hashtable();
                            datatemplate.Add("CompanyName", CurrentUser.CompanyName);
                            datatemplate.Add("TechnicianName", TechnicianName);
                            #region Text message parse
                            if (!string.IsNullOrWhiteSpace(objnotification.Text))
                            {
                                if (objnotification.Text.IndexOf("##") > -1)
                                {
                                    EmailParser BodyParser = new EmailParser(objnotification.Text, datatemplate, false);
                                    textMessage = BodyParser.Parse();
                                }
                                else
                                {
                                    textMessage = objnotification.Text;
                                }
                            }
                            #endregion
                            if (Model.NotifyCustomer.HasValue && Model.NotifyCustomer.Value == true && objcustomer.PreferedSms == true && !string.IsNullOrWhiteSpace(objnotification.Text) && !string.IsNullOrWhiteSpace(objcustomer.PrimaryPhone))
                            {
                                List<string> ReceiverNumberList = new List<string>();
                                ReceiverNumberList.Add(objcustomer.PrimaryPhone);
                                _Util.Facade.SMSFacade.SendSMS(CurrentUser.CompanyId.Value, CurrentUser.UserId, textMessage, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName));
                                Message += " And send notification text successfully.";
                            }
                            if (Model.NotifyCustomer.HasValue && Model.NotifyCustomer.Value == true && objcustomer.PreferedSms == true && !string.IsNullOrWhiteSpace(objnotification.Text) && !string.IsNullOrWhiteSpace(objcustomer.SecondaryPhone))
                            {
                                List<string> ReceiverNumberList = new List<string>();
                                ReceiverNumberList.Add(objcustomer.SecondaryPhone);
                                _Util.Facade.SMSFacade.SendSMS(CurrentUser.CompanyId.Value, CurrentUser.UserId, textMessage, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName));
                                Message += " And send notification text successfully.";
                            }
                            if (Model.NotifyCustomer.HasValue && Model.NotifyCustomer.Value == true && objcustomer.PreferedSms == true && !string.IsNullOrWhiteSpace(objnotification.Text) && !string.IsNullOrWhiteSpace(objcustomer.CellNo))
                            {
                                List<string> ReceiverNumberList = new List<string>();
                                ReceiverNumberList.Add(objcustomer.CellNo);
                                _Util.Facade.SMSFacade.SendSMS(CurrentUser.CompanyId.Value, CurrentUser.UserId, textMessage, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName));
                                Message += " And send notification text successfully.";
                            }
                        }
                    }
                }
            }
            #endregion

            #region Dispatch message
            if (Model.IsDispatch.HasValue && Model.IsDispatch.Value && Model.Assigned != null && Model.Assigned.Length > 0)
            {
                Guid AssignedUser = Model.Assigned.FirstOrDefault();
                if (AssignedUser != Guid.Empty && AssignedUser != new Guid("22222222-2222-2222-2222-222222222222"))
                {
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(AssignedUser);
                    if (emp != null)
                    {
                        string SMSMessage = @"Ticket#{0} has been dispatched to you.";
                        SMSMessage = string.Format(SMSMessage, Model.Ticket.Id);

                        _Util.Facade.SMSFacade.SendTicketStatusToCustomer(CurrentUser.CompanyId.Value, CurrentUser.UserId, SMSMessage, emp.Phone);
                    }
                }
            }
            #endregion

            #region Rug Items Update

            #endregion

            SaveTicketBooking(Model.BookingId, Model.TicketBookingDetails, Model.TicketBookingExtraItems, Model.RecreateInvoice, Model.Ticket);
            #endregion

            #region Additional Member Appointment
            var appdate = new DateTime();
            List<TicketUser> objticketuser = _Util.Facade.TicketFacade.GetAllAdditionalTicketUserByTicketId(Model.Ticket.TicketId);
            CustomerAppointment objcusappointment = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentidByAppoinmentId(Model.Ticket.TicketId);
            List<AdditionalMembersAppointment> objadditionalMem = _Util.Facade.CustomerAppoinmentFacade.GetAllAdditionalMembersAppointmentByAppointmentId(Model.Ticket.TicketId);
            if (objadditionalMem != null && objadditionalMem.Count > 0)
            {
                List<string> UserIdList = new List<string>();
                foreach (var item in objadditionalMem)
                {
                    appdate = Model.Ticket.CompletionDate;
                    _Util.Facade.CustomerAppoinmentFacade.DeleteAdditionalMembersAppointment(item.Id);
                    if (objticketuser != null && objticketuser.Count > 0)
                    {
                        foreach (var item1 in objticketuser)
                        {
                            if (item.EmployeeId == item1.UserId)
                            {
                                UserIdList.Add(item.EmployeeId.ToString());
                                AdditionalMembersAppointment objadditional = new AdditionalMembersAppointment()
                                {
                                    AppointmentId = Model.Ticket.TicketId,
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    CustomerId = Model.Ticket.CustomerId,
                                    EmployeeId = item1.UserId,
                                    AppointmentDate = item.AppointmentDate,
                                    AppointmentStartTime = item.AppointmentStartTime,
                                    AppointmentEndTime = item.AppointmentEndTime,
                                    CreatedBy = CurrentUser.UserId,
                                    LastUpdatedBy = CurrentUser.UserId,
                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                    MemberAppointmentId = new Guid(),
                                    IsAllDay = ((string.IsNullOrWhiteSpace(item.AppointmentStartTime) || item.AppointmentStartTime == "-1") ? true : false)
                                };
                                _Util.Facade.CustomerAppoinmentFacade.InsertAdditionalMembersAppointment(objadditional);
                            }
                        }
                    }
                }

                if (objticketuser != null && objticketuser.Count > 0 && objcusappointment != null)
                {
                    var RestOfUser = objticketuser.Where(x => UserIdList.All(y => y != x.UserId.ToString())).ToList();
                    if (RestOfUser != null && RestOfUser.Count > 0)
                    {
                        foreach (var RestItems in RestOfUser)
                        {
                            AdditionalMembersAppointment objadditional = new AdditionalMembersAppointment()
                            {
                                AppointmentId = Model.Ticket.TicketId,
                                CompanyId = CurrentUser.CompanyId.Value,
                                CustomerId = Model.Ticket.CustomerId,
                                EmployeeId = RestItems.UserId,
                                AppointmentDate = objcusappointment.AppointmentDate,
                                AppointmentStartTime = objcusappointment.AppointmentStartTime,
                                AppointmentEndTime = objcusappointment.AppointmentEndTime,
                                CreatedBy = CurrentUser.UserId,
                                LastUpdatedBy = CurrentUser.UserId,
                                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                MemberAppointmentId = new Guid(),
                                IsAllDay = ((string.IsNullOrWhiteSpace(objcusappointment.AppointmentStartTime) || objcusappointment.AppointmentStartTime == "-1") ? true : false)
                            };
                            _Util.Facade.CustomerAppoinmentFacade.InsertAdditionalMembersAppointment(objadditional);
                        }
                    }
                }
            }
            else
            {
                if (objticketuser != null && objcusappointment != null && objticketuser.Count > 0)
                {
                    foreach (var item in objticketuser)
                    {
                        if (!string.IsNullOrWhiteSpace(objcusappointment.AppointmentStartTime) && objcusappointment.AppointmentStartTime != "-1" && !string.IsNullOrWhiteSpace(objcusappointment.AppointmentEndTime) && objcusappointment.AppointmentEndTime != "-1")
                        {
                            AdditionalMembersAppointment objadditional = new AdditionalMembersAppointment()
                            {
                                AppointmentId = Model.Ticket.TicketId,
                                CompanyId = CurrentUser.CompanyId.Value,
                                CustomerId = Model.Ticket.CustomerId,
                                EmployeeId = item.UserId,
                                AppointmentDate = objcusappointment.AppointmentDate,
                                AppointmentStartTime = objcusappointment.AppointmentStartTime,
                                AppointmentEndTime = objcusappointment.AppointmentEndTime,
                                CreatedBy = CurrentUser.UserId,
                                LastUpdatedBy = CurrentUser.UserId,
                                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                MemberAppointmentId = new Guid(),
                                IsAllDay = ((string.IsNullOrWhiteSpace(objcusappointment.AppointmentStartTime) || objcusappointment.AppointmentStartTime == "-1") ? true : false)
                            };
                            _Util.Facade.CustomerAppoinmentFacade.InsertAdditionalMembersAppointment(objadditional);
                        }
                        else
                        {
                            AdditionalMembersAppointment objadditional = new AdditionalMembersAppointment()
                            {
                                AppointmentId = Model.Ticket.TicketId,
                                CompanyId = CurrentUser.CompanyId.Value,
                                CustomerId = Model.Ticket.CustomerId,
                                EmployeeId = item.UserId,
                                AppointmentDate = objcusappointment.AppointmentDate,
                                AppointmentStartTime = objcusappointment.AppointmentStartTime,
                                AppointmentEndTime = objcusappointment.AppointmentEndTime,
                                CreatedBy = CurrentUser.UserId,
                                LastUpdatedBy = CurrentUser.UserId,
                                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                MemberAppointmentId = new Guid(),
                                IsAllDay = ((string.IsNullOrWhiteSpace(objcusappointment.AppointmentStartTime) || objcusappointment.AppointmentStartTime == "-1") ? true : false)
                            };
                            _Util.Facade.CustomerAppoinmentFacade.InsertAdditionalMembersAppointment(objadditional);
                        }
                    }
                }
            }
            #endregion

            CustomerSalesDateUpdate(Model.Ticket.CustomerId);

            //if (Model.Ticket.TicketType != LabelHelper.TicketType.Service && (Model.Ticket.Status == LabelHelper.TicketStatus.Completed || Model.Ticket.Status == LabelHelper.TicketStatus.Closed))
            //{
            //    // Here is my recurring billing code   
            //    var IsRMRActice = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "IsRMRActive");
            //    if (IsRMRActice != null && IsRMRActice.Value.ToLower() == "true")
            //    {
            //        var controller = DependencyResolver.Current.GetService<RecurringBillingController>();
            //        controller.UpdateRecurringBillingInformationByCustomerModification(Model.Ticket.CustomerId, Model.Ticket.TicketId, CurrentUser.UserId, CurrentUser.CompanyId.Value, "", "Ticket");
            //    }
            //}

            return Json(new { result = Proceed, message = Message, CustomerId = CustomerId, TicketId = Model.Ticket.Id, InvId = invId, InstallEquipmentsMsg = InstallEquipmentsMsg });
        }
        public bool SendSMSToChangeTicketStatus(int? leadid,Guid TicketId, int IntTicketId,string OldStatus, string TicketNewstatus)
        { 
            if (!leadid.HasValue)
                return false;
            Guid CompanyId = new Guid();
            Guid UserId = Guid.Empty;
            if (User.Identity.IsAuthenticated)
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CompanyId = CurrentUser.CompanyId.Value;
                UserId = CurrentUser.UserId;
            }
            else
            {
                CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(leadid.Value);
                CompanyId = custommerCompany.CompanyId;
            }
            Company _Company = new Company { CompanyId = CompanyId };

            List<string> ReceiverNumberList = new List<string>();

            if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(leadid.Value, CompanyId))
            {
                return false;
            }

            var Cus = _Util.Facade.CustomerFacade.GetCustomerById(leadid.Value);

            #region Total RMR Calculation, Lead Source, Sales Location, Ticket type And Sales Group
             
            string PrefferedNO = "";
            string ReceiverNumber = "";
            string[] PrefferedNOList = PrefferedNO.Split(',');
            Employee _emp = new Employee();
            //if (Cus != null && TicketId != new Guid())
            //{ 
            //    List<Guid> assigned = new List<Guid>();
            //    List<TicketUser> UserList = _Util.Facade.TicketFacade.GetTicketUserListByTicketId(TicketId);
            //    if(UserList != null && UserList.Count()>0)
            //    {
            //        foreach (var item in UserList)
            //        {
            //            assigned.Add(item.UserId);
            //        } 
            //    } 
            //    foreach (Guid item in assigned)
            //    { 
            //         _emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(item);
            //        if (_emp != null)
            //        {
            //            if (!string.IsNullOrWhiteSpace(_emp.Phone) && _emp.Phone != "administrator")
            //            {
            //                ReceiverNumber = _emp.Phone.TrimPhoneNum(); //_emp.Phone.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            //            }
            //            if (!string.IsNullOrWhiteSpace(ReceiverNumber) && ReceiverNumber != "administrator")
            //            {
            //                ReceiverNumberList.Add(ReceiverNumber);
            //            } 
            //        }  
            //    } 
            //}
            #endregion 
            #region ReceiverNumber Setup

            GlobalSetting GlobalSettingModel = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("TicketStatusSMS");
            if (GlobalSettingModel != null && GlobalSettingModel.Value.ToLower() == "true")
            {
                PrefferedNO = GlobalSettingModel.OptionalValue.Replace(" ", "");
            }
            else
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(PrefferedNO))
            {
                PrefferedNO = PrefferedNO.TrimPhoneNum();//.Replace("-", "").Replace("(", "").Replace(")", "");
            } 
            PrefferedNOList = PrefferedNO.Split(',');
            if (PrefferedNOList != null && PrefferedNOList.Length > 0)
            {
                for (int i = 0; i < PrefferedNOList.Length; i++)
                {
                    if (PrefferedNOList[i].Count() == 10)
                    {
                        PrefferedNOList[i] = PrefferedNOList[i];
                    }
                    ReceiverNumberList.Add(PrefferedNOList[i]);
                }
            }
            if (!string.IsNullOrWhiteSpace(ReceiverNumber))
            {
                ReceiverNumberList.Add(ReceiverNumber);
            } 
            ReceiverNumberList = ReceiverNumberList.Distinct().ToList();
            #endregion  
            string phonenumber = string.Join(";", ReceiverNumberList);

            string CustomerLink = AppConfig.DomainSitePath + string.Format("/Customer/Customerdetail/?id={0}", Cus.Id);
            string shortUrl2 = "";
            ShortUrl ShortUrl2 = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(CustomerLink, null);
            shortUrl2 = string.Concat(AppConfig.SiteDomain, "/shrt/", ShortUrl2.Code);

            if (ReceiverNumberList.Count() > 0)
            {
                bool sendResult = _Util.Facade.SMSFacade.SendTicketStatusSMS(IntTicketId, CompanyId, ReceiverNumberList, false, string.Empty, UserId, Cus.Id, shortUrl2, OldStatus, TicketNewstatus);
                return sendResult;
            }
            else
            {
                return false;
            }
        }
        [Authorize]
        [HttpPost]
        public JsonResult UndoThisEquipment(int Id, Guid AppointmentId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            if (Id > 0)
            {
                var CAEDetails = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentEquipmentById(Id);
                var TicketAssignedToDetails = _Util.Facade.TicketFacade.GetTicketAssignedUserListByTicketId(AppointmentId).FirstOrDefault();
                if (CAEDetails != null && TicketAssignedToDetails != null)
                {
                    var GetTicket = _Util.Facade.TicketFacade.GetTicketByTicketId(CAEDetails.AppointmentId);
                    if (GetTicket != null)
                    {
                        var UndoInstalledReport = _Util.Facade.CustomerFacade.GetIndividualInstalledEquipmentByTicketIdAndEquipmentId(GetTicket.Id, CAEDetails.EquipmentId);
                        if (UndoInstalledReport != null)
                        {
                            result = _Util.Facade.CustomerFacade.DeleteIndividualInstalledEquipment(UndoInstalledReport.TicketId, UndoInstalledReport.EquipmentId);
                        }
                    }
                    if (CAEDetails.IsEquipmentExist != true)
                    {
                        if(CAEDetails.IsEquipmentRelease==false)
                        {
                            var qty =   CAEDetails.Quantity - CAEDetails.QuantityLeftEquipment;
                            InventoryTech invtechx = new InventoryTech()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                TechnicianId = TicketAssignedToDetails.UserId,
                                EquipmentId = CAEDetails.EquipmentId,
                                Type = LabelHelper.InventoryType.Release,
                                Quantity = qty ?? 0,

                                LastUpdatedBy = CurrentUser.UserId,
                                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                Description = "Release from technician by ticket",
                                CustomerAppointmentEquipmentId = CAEDetails.Id
                            };

                            _Util.Facade.InventoryFacade.InsertInventoryTech(invtechx);

                        }
                        InventoryTech invtech = new InventoryTech()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            TechnicianId = TicketAssignedToDetails.UserId,
                            EquipmentId = CAEDetails.EquipmentId,
                            Type = LabelHelper.InventoryType.Add,
                            Quantity = CAEDetails.Quantity,
                            LastUpdatedBy = CurrentUser.UserId,
                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            Description = "Add to technician from ticket(Undo)",
                            CustomerAppointmentEquipmentId = CAEDetails.Id
                        };

                        _Util.Facade.InventoryFacade.InsertInventoryTech(invtech);
                    }
                    CAEDetails.IsEquipmentRelease = false;
                    CAEDetails.QuantityLeftEquipment = 0;
         
                    result = _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(CAEDetails);
                }
            }
            return Json(new { result = result, ticketId = AppointmentId });
        }
        public void AddUserActivityForNewTicket(Ticket TicketModel, Guid[] Assigned, Guid[] UserList, Guid[] NotifyingUserList)
        {
            string Detail = "New Ticket #" + TicketModel.Id + " is added";
            if (!string.IsNullOrWhiteSpace(TicketModel.TicketType))
            {
                Detail = Detail + "</br>Ticket Type: " + TicketModel.TicketType;
            }
            if (!string.IsNullOrWhiteSpace(TicketModel.Message))
            {
                Detail = Detail + "</br>Description: " + TicketModel.Message;
            }
            if (TicketModel.CompletionDate != null && TicketModel.CompletionDate != new DateTime())
            {
                Detail = Detail + "</br>Scheduled On: " + TicketModel.CompletionDate;
            }
            if (!string.IsNullOrWhiteSpace(TicketModel.AppointmentStartTime) && TicketModel.AppointmentStartTime != "-1" && !string.IsNullOrWhiteSpace(TicketModel.AppointmentEndTime) && TicketModel.AppointmentEndTime != "-1")
            {
                Detail = Detail + "</br>Start Time: " + TicketModel.AppointmentStartTime + ",End Time: " + TicketModel.AppointmentEndTime;
            }
            if (Assigned != null && Assigned.Count() > 0)
            {
                Detail = Detail + "</br>Assigned To: ";
                foreach (var item in Assigned)
                {
                    if (item != Guid.Empty)
                    {
                        Employee empuser = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item);
                        Detail += empuser.FirstName + " " + empuser.LastName + ",";
                    }
                }
                Detail = Detail.TrimEnd(',');
            }
            if (UserList != null && UserList.Count() > 0)
            {
                Detail = Detail + "</br>Additional Members: ";
                foreach (var item in UserList)
                {
                    Employee empuser = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item);
                    Detail += empuser.FirstName + " " + empuser.LastName + ",";
                }
                Detail = Detail.TrimEnd(',');
            }

            if (NotifyingUserList != null && NotifyingUserList.Count() > 0)
            {
                Detail = Detail + "</br>Notifying Members: ";
                foreach (var item in NotifyingUserList)
                {
                    if (item == new Guid())
                        continue;
                    Employee empuser = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item);
                    Detail += empuser.FirstName + " " + empuser.LastName + ",";
                }
                Detail = Detail.TrimEnd(',');
            }
            if (!string.IsNullOrWhiteSpace(TicketModel.Status))
            {
                Detail = Detail + "</br>Status To: " + TicketModel.Status;
            }
            base.AddUserActivityForCustomer(Detail, LabelHelper.ActivityAction.AddTicket, TicketModel.CustomerId, null, null);
        }


        public void AddUserActivityForUpdateTicket(Ticket TicketModel, Guid[] Assigned, Guid[] UserList, Guid[] NotifyingUserList)
        {
            string Detail = "Ticket #" + TicketModel.Id + " is updated";

            Ticket OldTicket = _Util.Facade.TicketFacade.GetTicketById(TicketModel.Id);
            //if (OldTicket.TicketType != TicketModel.TicketType)
            //{
            //    Detail = Detail + "</br>Ticket type is updated: " + OldTicket.TicketType + " to " + TicketModel.TicketType;
            //}

            if (OldTicket.Message != TicketModel.Message)
            {
                if (string.IsNullOrWhiteSpace(OldTicket.Message))
                {
                    if (!string.IsNullOrWhiteSpace(TicketModel.Message))
                    {
                        Detail = Detail + "</br>Description added: " + TicketModel.Message;
                    }
                }
                else
                {
                    Detail = Detail + "</br>Description is updated: " + OldTicket.Message + " to " + TicketModel.Message;
                }
            }

            if (OldTicket.CompletionDate != TicketModel.CompletionDate)
            {
                if (OldTicket.CompletionDate == null || OldTicket.CompletionDate == new DateTime())
                {
                    Detail = Detail + "</br>Scheduled is added on: " + TicketModel.CompletionDate;
                }
                else
                {
                    Detail = Detail + "</br>Scheduled is updated: " + OldTicket.CompletionDate + " to " + TicketModel.CompletionDate;
                }

            }

            //AppointmentId
            CustomerAppointment CustomerAppointmentModel = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentDetailByAppointmentId(TicketModel.TicketId);
            OldTicket.AppointmentStartTime = CustomerAppointmentModel.AppointmentStartTime;

            if (OldTicket.AppointmentStartTime != TicketModel.AppointmentStartTime)
            {
                if ((string.IsNullOrWhiteSpace(OldTicket.AppointmentStartTime) || OldTicket.AppointmentStartTime == "-1") && (!string.IsNullOrWhiteSpace(TicketModel.AppointmentStartTime) && TicketModel.AppointmentStartTime != "-1"))
                {
                    Detail = Detail + "</br>Start Time is added on: " + TicketModel.AppointmentStartTime;
                }
                else if (!string.IsNullOrWhiteSpace(TicketModel.AppointmentStartTime))
                {
                    if (TicketModel.AppointmentStartTime == "-1")
                    {
                        TicketModel.AppointmentStartTime = "Please Select One";
                    }
                    Detail = Detail + "</br>Start Time is updated: " + OldTicket.AppointmentStartTime + " to " + TicketModel.AppointmentStartTime;
                }

            }
            OldTicket.AppointmentEndTime = CustomerAppointmentModel.AppointmentEndTime;
            if (OldTicket.AppointmentEndTime != TicketModel.AppointmentEndTime)
            {
                if ((string.IsNullOrWhiteSpace(OldTicket.AppointmentEndTime) || OldTicket.AppointmentEndTime == "-1") && (!string.IsNullOrWhiteSpace(TicketModel.AppointmentEndTime) && TicketModel.AppointmentEndTime != "-1"))
                {

                    Detail = Detail + "</br>End Time is added on: " + TicketModel.AppointmentEndTime;
                }
                else if (!string.IsNullOrWhiteSpace(TicketModel.AppointmentEndTime))
                {
                    if (TicketModel.AppointmentEndTime == "-1")
                    {
                        TicketModel.AppointmentEndTime = "Please Select One";
                    }
                    Detail = Detail + "</br>End Time is updated: " + OldTicket.AppointmentEndTime + " to " + TicketModel.AppointmentEndTime;
                }
            }



            #region Assigned User
            List<TicketUser> ticketUser = _Util.Facade.TicketFacade.GetTicketAssignedUserListByTicketId(TicketModel.TicketId);
            string OldAssigned = "";
            string NewAssigned = "";
            if (ticketUser.Count > 0)
            {
                foreach (var item in ticketUser)
                {
                    Employee empuser = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item.UserId);
                    OldAssigned += empuser.FirstName + " " + empuser.LastName + ",";
                }
            }
            if (Assigned != null && Assigned.Count() > 0)
            {
                foreach (var item in Assigned)
                {
                    if (item != Guid.Empty)
                    {
                        Employee empuser = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item);
                        NewAssigned += empuser.FirstName + " " + empuser.LastName + ",";
                    }
                }
            }
            if (OldAssigned != NewAssigned)
            {
                if (string.IsNullOrWhiteSpace(OldAssigned))
                {
                    Detail = Detail + "</br>Assigned To: " + NewAssigned;
                }
                else
                {
                    Detail = Detail + "</br>Assigned is updated: " + OldAssigned + " to " + NewAssigned;
                }
            }
            Detail = Detail.TrimEnd(',');
            #endregion

            #region Additional User
            List<TicketUser> additionalUser = _Util.Facade.TicketFacade.GetOnlyTicketAddtionalUsersByTicketId(TicketModel.TicketId);
            String OldAdditional = "";
            String NewAdditional = "";
            if (additionalUser.Count > 0)
            {
                foreach (var item in additionalUser)
                {
                    Employee empuser = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item.UserId);
                    OldAdditional += empuser.FirstName + " " + empuser.LastName + ",";
                    //additionaluser.Add(item.UserId.ToString());

                }
            }
            if (UserList != null && UserList.Count() > 0)
            {
                foreach (var item in UserList)
                {
                    Employee empuser = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item);
                    NewAdditional += empuser.FirstName + " " + empuser.LastName + ",";
                }
            }
            if (OldAdditional != NewAdditional)
            {
                if (string.IsNullOrWhiteSpace(OldAdditional))
                {
                    Detail = Detail + "</br>Additional Members: " + NewAdditional;
                }
                else
                {
                    Detail = Detail + "</br>Additional Members is updated: " + OldAdditional + " to " + NewAdditional;
                }
            }
            Detail = Detail.TrimEnd(',');
            #endregion

            #region Notifying User
            List<TicketUser> NotifyUser = _Util.Facade.TicketFacade.GetTicketUserListByTicketId(TicketModel.TicketId).Where(x => x.NotificationOnly == true).ToList();
            String OldNotifyUser = "";
            String NewNotifyUser = "";
            if (NotifyUser.Count > 0)
            {
                foreach (var item in NotifyUser)
                {
                    Employee empuser = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item.UserId);
                    OldNotifyUser += empuser.FirstName + " " + empuser.LastName + ",";
                }
            }
            if (NotifyingUserList != null && NotifyingUserList.Count() > 0)
            {
                foreach (var item in NotifyingUserList)
                {

                    Employee empuser = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item);
                    NewNotifyUser += empuser.FirstName + " " + empuser.LastName + ",";
                }

            }
            if (OldNotifyUser != NewNotifyUser)
            {
                if (string.IsNullOrWhiteSpace(OldNotifyUser))
                {
                    Detail = Detail + "</br>Notifying Members added: " + NewNotifyUser;
                }
                else
                {
                    Detail = Detail + "</br>Notifying Members is updated: " + OldNotifyUser + " to " + NewNotifyUser;
                }
            }
            Detail = Detail.TrimEnd(',');
            #endregion



            if (OldTicket.Status != TicketModel.Status)
            {
                if (string.IsNullOrWhiteSpace(OldTicket.Status))
                {
                    Detail = Detail + "</br>Status is added: " + TicketModel.Status;
                }
                else
                {
                    Detail = Detail + "</br>Status is updated: " + OldTicket.Status + " to " + TicketModel.Status;
                }
            }
            if (Detail != "Ticket #" + TicketModel.Id + " is updated")
            {
                base.AddUserActivityForCustomer(Detail, LabelHelper.ActivityAction.UpdateTicket, TicketModel.CustomerId, null, null);
            }


        }


        public JsonResult CloneTicket(int ticketId)
        {
            bool result = false;
            var ticketIntId = 0;
            var NewTicketId = Guid.NewGuid();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (ticketId > 0)
            {
                Ticket ticket = _Util.Facade.TicketFacade.GetTicketById(ticketId);
                Ticket ticketOld = _Util.Facade.TicketFacade.GetTicketById(ticketId);
                if (ticket != null)
                {
                    #region Insert Ticket
                    ticket.Status = LabelHelper.TicketStatus.Created;
                    ticket.TicketId = NewTicketId;
                    ticket.IsAgreementTicket = false;
                    ticket.CreatedDate = ticket.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    ticketIntId = _Util.Facade.TicketFacade.InsertTicket(ticket);
                    logger.WithProperty("tags", "ticket,insert").WithProperty("params", JsonConvert.SerializeObject(ticket)).Trace("Ticket Id {Id}", ticket.Id);
                    #endregion
                    #region Insert Ticket User
                    List<TicketUser> ticketUser = _Util.Facade.TicketFacade.GetTicketUserByTicketId(ticketOld.TicketId);
                    if (ticketUser.Count > 0)
                    {
                        foreach (var item in ticketUser)
                        {
                            item.TiketId = NewTicketId;
                            item.AddedDate = DateTime.Now.UTCCurrentTime();
                            result = _Util.Facade.TicketFacade.InsertTicketUser(item) > 0;
                        }
                    }
                    #endregion
                    #region Insert Customer Appointment
                    CustomerAppointment ca = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentDetailinfoByAppointmentId(ticketOld.TicketId);
                    if (ca != null)
                    {
                        ca.AppointmentId = NewTicketId;
                        ca.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        result = _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(ca) > 0;
                    }
                    #endregion
                    #region Insert Equipment Service
                    List<CustomerAppointmentEquipment> cusEqpList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(ticketOld.TicketId);
                    if (cusEqpList.Count > 0)
                    {
                        foreach (var item in cusEqpList)
                        {
                            item.AppointmentId = NewTicketId;
                            result = _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppointmentEquipmentDetail(item) > 0;
                        }
                    }
                    #endregion
                    #region Insert Ticket Replay
                    var replayMessage = string.Format("This ticket clone from <a onclick='OpenTicketById({0})'>Ticket #{1}</a>", ticketIntId, ticketId);
                    TicketReply TR = new TicketReply()
                    {
                        Message = replayMessage,
                        TicketId = NewTicketId,
                        RepliedDate = DateTime.Now.UTCCurrentTime(),
                        IsPrivate = false,
                        UserId = CurrentUser.UserId
                    };
                    result = _Util.Facade.TicketFacade.InsertTicketReply(TR) > 0;
                    #endregion
                }
            }
            return Json(new { result = result, ticketId = ticketIntId });
        }

        public JsonResult RescheduleTicket(int ticketId, List<CustomerAppointmentEquipment> EquipmentDetailList)
        {
            bool result = false;
            var ticketIntId = 0;
            var NewTicketId = Guid.NewGuid();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (ticketId > 0)
            {
                Ticket ticket = _Util.Facade.TicketFacade.GetTicketById(ticketId);
                Ticket ticketOld = _Util.Facade.TicketFacade.GetTicketById(ticketId);
                if (ticket != null)
                {
                    #region Insert Ticket
                    ticket.Status = LabelHelper.TicketStatus.Created;
                    ticket.TicketId = NewTicketId;
                    ticket.IsAgreementTicket = false;
                    ticket.CreatedDate = ticket.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    ticket.ReferenceTicketId = ticketId;
                    ticket.TicketType = LabelHelper.TicketType.Service;
                    ticketIntId = _Util.Facade.TicketFacade.InsertTicket(ticket);
                    logger.WithProperty("tags", "ticket,insert").WithProperty("params", JsonConvert.SerializeObject(ticket)).Trace("Ticket Id {Id}", ticket.Id);
                    #endregion
                    #region Insert Ticket User
                    List<TicketUser> ticketUser = _Util.Facade.TicketFacade.GetTicketUserByTicketId(ticketOld.TicketId);
                    if (ticketUser.Count > 0)
                    {
                        foreach (var item in ticketUser)
                        {
                            item.TiketId = NewTicketId;
                            item.AddedDate = DateTime.Now.UTCCurrentTime();
                            result = _Util.Facade.TicketFacade.InsertTicketUser(item) > 0;
                        }
                    }
                    #endregion
                    #region Insert Customer Appointment
                    CustomerAppointment ca = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentDetailinfoByAppointmentId(ticketOld.TicketId);
                    if (ca != null)
                    {
                        ca.AppointmentId = NewTicketId;
                        ca.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        ca.AppointmentType = LabelHelper.TicketType.Service;
                        result = _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(ca) > 0;
                    }
                    #endregion
                    #region Insert Equipment Service
                    if (EquipmentDetailList != null && EquipmentDetailList.Count > 0)
                    {
                        foreach (var item in EquipmentDetailList)
                        {
                            var objcusequip = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentEquipmentById(item.Id);
                            if (objcusequip != null)
                            {
                                objcusequip.AppointmentId = NewTicketId;
                                objcusequip.IsCheckedEquipment = item.CheckedEqp;
                                objcusequip.Quantity = item.Quantity;
                                objcusequip.QuantityLeftEquipment = item.QuantityLeftEquipment;
                                result = _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppointmentEquipmentDetail(objcusequip) > 0;
                            }
                        }
                    }
                    #endregion
                    #region Ticket Replay
                    var objticket = _Util.Facade.TicketFacade.GetTicketById(ticketId);
                    if (objticket != null)
                    {
                        List<TicketReply> ListTicketReply = _Util.Facade.TicketFacade.GetTicketReplyListByTicketId(objticket.TicketId);
                        if (ListTicketReply != null && ListTicketReply.Count > 0)
                        {
                            foreach (var item in ListTicketReply)
                            {
                                item.TicketId = NewTicketId;
                                _Util.Facade.TicketFacade.InsertTicketReply(item);
                            }
                        }
                    }
                    #endregion
                    //CalculateRescheduleCommission(ticket);
                }
            }
            return Json(new { result = result, ticketId = ticketIntId });
        }

        #region RecreateAgreement
        [Authorize]
        [HttpPost]
        public JsonResult ConfirmPackageChange(AddSmartLeadPackage ModelAddleadPackage)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<CustomerPackageEqp> PackageEqpList = new List<CustomerPackageEqp>();

            bool result = false;
            #region Insert and Update Package settings and Equipment Settings
            if (ModelAddleadPackage.LeadId > 0)
            {
                #region PackageSetup
                Customer LeadInfo = _Util.Facade.CustomerFacade.GetCustomersById(ModelAddleadPackage.LeadId);
                if (LeadInfo != null)
                {
                    bool DeleteSuccessOldPackageData = _Util.Facade.PackageFacade.DeleteAllCustomerPackageEqpByCompanyIdCustomerId(currentLoggedIn.CompanyId.Value, LeadInfo.CustomerId);
                    #region Service ADDED EQUIPMENT s
                    var DeleteSuccessOldServiceData = _Util.Facade.PackageFacade.DeleteCustomerPackageServiceByCompanyIdCustomerId(currentLoggedIn.CompanyId.Value, LeadInfo.CustomerId);

                    if (ModelAddleadPackage.ServiceList != null)
                    {
                        #region Service ADDED EQUIPMENT s
                        foreach (var item in ModelAddleadPackage.ServiceList)
                        {
                            if (item.EquipmentId != new Guid())
                            {
                                Equipment ServiceInfo = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(item.EquipmentId, currentLoggedIn.CompanyId.Value);
                                if (ServiceInfo != null)
                                {
                                    CustomerPackageService customerPackageService = new CustomerPackageService()
                                    {
                                        CompanyId = currentLoggedIn.CompanyId.Value,
                                        CustomerId = LeadInfo.CustomerId,
                                        PackageId = item.PackageId,
                                        EquipmentId = ServiceInfo.EquipmentId,
                                        MonthlyRate = item.MonthlyRate,
                                        DiscountRate = 0,
                                        Total = item.Total
                                    };
                                    result = _Util.Facade.PackageFacade.InsertCustomerPackageService(customerPackageService) > 0;
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region EquipmentList
                    if (ModelAddleadPackage.EquipmentList != null)
                    {
                        foreach (var item in ModelAddleadPackage.EquipmentList)
                        {
                            if (item.SelectedEquipmentId != new Guid())
                            {
                                Equipment ProductInfo = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(item.SelectedEquipmentId, currentLoggedIn.CompanyId.Value);
                                if (ProductInfo != null)
                                {
                                    if (item.SelectedEquipmentIsFree == true || ProductInfo.Retail == null)
                                    {
                                        ProductInfo.Retail = 0.0;
                                    }
                                    CustomerPackageEqp customerPackage = new CustomerPackageEqp()
                                    {
                                        CompanyId = currentLoggedIn.CompanyId.Value,
                                        CustomerId = LeadInfo.CustomerId,
                                        PackageId = ModelAddleadPackage.PackageId,
                                        EquipmentId = ProductInfo.EquipmentId,
                                        IsIncluded = item.IsIncluded,
                                        IsDevice = item.IsDevice,
                                        IsOptionalEqp = item.IsOptionalEqp,
                                        IsServiceEquipment = false,
                                        Quantity = item.NumOfEquipments,
                                        UnitPrice = ProductInfo.Retail.Value,
                                        DiscountUnitPricce = 0,
                                        DiscountPckage = 0,
                                        Total = ProductInfo.Retail.Value
                                    };
                                    result = _Util.Facade.PackageFacade.InsertCustomerPackageEqp(customerPackage) > 0;
                                }
                            }
                        }
                    }
                    #endregion

                    #region ServiceEquipmentList
                    if (ModelAddleadPackage.SmartPackageEquipmentServiceEquipmentList != null)
                    {
                        foreach (var item in ModelAddleadPackage.SmartPackageEquipmentServiceEquipmentList)
                        {
                            if (item.EquipmentId != new Guid())
                            {
                                var ProductInfo = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(item.EquipmentId, currentLoggedIn.CompanyId.Value);
                                if (ProductInfo != null)
                                {
                                    CustomerPackageEqp customerPackage = new CustomerPackageEqp()
                                    {
                                        CompanyId = currentLoggedIn.CompanyId.Value,
                                        CustomerId = LeadInfo.CustomerId,
                                        PackageId = ModelAddleadPackage.PackageId,
                                        EquipmentId = ProductInfo.EquipmentId,
                                        IsIncluded = false,
                                        IsDevice = false,
                                        IsOptionalEqp = false,
                                        IsServiceEquipment = true,
                                        Quantity = item.Quantity,
                                        UnitPrice = item.EquipmentPrice,
                                        DiscountUnitPricce = 0,
                                        DiscountPckage = 0,
                                        Total = item.EquipmentPrice * item.Quantity
                                    };
                                    result = _Util.Facade.PackageFacade.InsertCustomerPackageEqp(customerPackage) > 0;
                                }
                            }
                        }
                    }
                    #endregion

                    #region Package Related 
                    var PackageCustomerDeails = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerIdandCompanyId(LeadInfo.CustomerId, currentLoggedIn.CompanyId.Value);
                    if (PackageCustomerDeails != null)
                    {
                        PackageCustomerDeails.PackageId = ModelAddleadPackage.PackageId;
                        result = _Util.Facade.PackageFacade.UpdatePackageCustomer(PackageCustomerDeails);
                    }
                    if (ModelAddleadPackage.PackageId != Guid.Empty)
                    {
                        var PackageDetails = _Util.Facade.SmartPackageFacade.GetPackageByPackageIdAndCompanyId(ModelAddleadPackage.PackageId, currentLoggedIn.CompanyId.Value);
                        if (PackageDetails != null)
                        {
                            LeadInfo.ActivationFee = PackageDetails.ActivationFee;
                            _Util.Facade.CustomerFacade.UpdateCustomer(LeadInfo);
                        }
                    }
                    #endregion

                    #region Customer Appointment Equipment
                    bool DeleteCustomerAppointmentEquipment = _Util.Facade.CustomerAppoinmentFacade.DeleteCustomerAppoinmentEquipmentByTicketId(ModelAddleadPackage.TicketId);
                    List<CustomerPackageEqp> CustomerPackageEqpList = _Util.Facade.PackageFacade.GetCustomerPackageEqpListbyCustomerId(currentLoggedIn.CompanyId.Value, LeadInfo.CustomerId);
                    if (CustomerPackageEqpList != null)
                    {
                        foreach (var eqp in CustomerPackageEqpList)
                        {
                            Equipment EquipmentDetails = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(eqp.EquipmentId);
                            CustomerAppointmentEquipment caEquipment = new CustomerAppointmentEquipment()
                            {
                                AppointmentId = ModelAddleadPackage.TicketId,
                                EquipmentId = eqp.EquipmentId,
                                Quantity = eqp.Quantity != null ? eqp.Quantity.Value : 0,
                                UnitPrice = eqp.UnitPrice != null ? eqp.UnitPrice.Value : 0,
                                TotalPrice = eqp.Total != null ? eqp.Total.Value : 0,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CreatedBy = currentLoggedIn.GetFullName(),
                                EquipName = EquipmentDetails.Name,
                                EquipDetail = EquipmentDetails.Description,
                                IsEquipmentRelease = false,
                                IsService = false,
                                IsAgreementItem = true,
                                CreatedByUid = currentLoggedIn.UserId
                            };
                            _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppointmentEquipmentDetail(caEquipment);
                        }
                    }
                    List<CustomerPackageService> CustomerPackageServiceList = _Util.Facade.CustomerFacade.IsLeadAppointmentServiceExistCheckCustomerPackageEqp(LeadInfo.CustomerId, currentLoggedIn.CompanyId.Value);
                    if (CustomerPackageServiceList != null)
                    {
                        foreach (var eqp in CustomerPackageServiceList)
                        {
                            Equipment EquipmentDetails = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(eqp.EquipmentId);
                            CustomerAppointmentEquipment caEquipment = new CustomerAppointmentEquipment()
                            {
                                AppointmentId = ModelAddleadPackage.TicketId,
                                EquipmentId = eqp.EquipmentId,
                                Quantity = 1,
                                UnitPrice = eqp.UnitPrice,
                                TotalPrice = eqp.Total != null ? eqp.Total.Value : 0,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CreatedBy = currentLoggedIn.GetFullName(),
                                EquipName = EquipmentDetails.Name,
                                EquipDetail = EquipmentDetails.Description,
                                IsEquipmentRelease = false,
                                IsService = true,
                                IsAgreementItem = true,
                                CreatedByUid = currentLoggedIn.UserId
                            };
                            _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppointmentEquipmentDetail(caEquipment);
                        }
                    }
                    #endregion
                }
                return Json(new { result = result });
                #endregion
            }
            else
            {
                return Json(false);
            }
            #endregion
        }

        [Authorize]
        [HttpPost]
        public ActionResult RecreateAgreement(Guid TicketId, Guid CustomerId)
        {

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            #region Delete equipmentservice
            if (CustomerId != null && CustomerId != Guid.Empty)
            {
                bool DeleteSuccessOldEqp = _Util.Facade.PackageFacade.DeleteAllCustomerPackageEqpByCompanyIdCustomerId(CurrentUser.CompanyId.Value, CustomerId);
                var DeleteSuccessOldService = _Util.Facade.PackageFacade.DeleteCustomerPackageServiceByCompanyIdCustomerId(CurrentUser.CompanyId.Value, CustomerId);
            }
            #endregion
            #region Adjust equipmentservice
            if (TicketId != null && TicketId != Guid.Empty)
            {
                var AppointmentList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipmentByTicketId(CurrentUser.CompanyId.Value, TicketId);
                var AppointmentEqpList = AppointmentList.Where(m => m.EquipmentClassId == 1).ToList();
                var AppointmentServiceList = AppointmentList.Where(m => m.EquipmentClassId == 2).ToList();
                var PackageDetails = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerIdandCompanyId(CustomerId, CurrentUser.CompanyId.Value);

                #region Equipment Adjust
                foreach (var eqpList in AppointmentEqpList)
                {
                    CustomerPackageEqp cpe = new CustomerPackageEqp
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = CustomerId,
                        PackageId = PackageDetails != null ? PackageDetails.PackageId : new Guid(),
                        EquipmentId = eqpList.EquipmentId,
                        IsIncluded = false,
                        IsDevice = false,
                        IsOptionalEqp = false,
                        Quantity = eqpList.Quantity,
                        UnitPrice = eqpList.UnitPrice,
                        DiscountUnitPricce = 0,
                        DiscountPckage = 0,
                        Total = eqpList.TotalPrice,
                        IsServiceEquipment = false
                    };
                    _Util.Facade.PackageFacade.InsertCustomerPackageEqp(cpe);
                }
                #endregion
                #region Equipment Adjust
                foreach (var eqpList in AppointmentServiceList)
                {
                    CustomerPackageService cps = new CustomerPackageService
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = CustomerId,
                        PackageId = PackageDetails != null ? PackageDetails.PackageId : new Guid(),
                        EquipmentId = eqpList.EquipmentId,
                        MonthlyRate = eqpList.UnitPrice,
                        DiscountRate = 0,
                        Total = eqpList.TotalPrice,
                        ManufacturerId = Guid.Empty,
                        LocationId = Guid.Empty,
                        TypeId = Guid.Empty,
                        ModelId = Guid.Empty,
                        FinishId = Guid.Empty,
                        CapacityId = Guid.Empty,
                    };
                    _Util.Facade.PackageFacade.InsertCustomerPackageService(cps);
                }
                #endregion
            }
            #endregion
            #region UpdateCustomer
            if (CustomerId != null && CustomerId != Guid.Empty)
            {
                var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                if (CustomerDetails != null)
                {
                    CustomerDetails.Singature = "";
                    CustomerDetails.AgreementEmail = CustomerDetails.EmailAddress;
                    CustomerDetails.AgreementPhoneNo = !string.IsNullOrWhiteSpace(CustomerDetails.PrimaryPhone) ? CustomerDetails.PrimaryPhone : !string.IsNullOrWhiteSpace(CustomerDetails.SecondaryPhone) ? CustomerDetails.SecondaryPhone : !string.IsNullOrWhiteSpace(CustomerDetails.CellNo) ? CustomerDetails.CellNo : "";
                    _Util.Facade.CustomerFacade.UpdateCustomer(CustomerDetails);
                }
            }
            #endregion
            return Json(new { result = true });
        }

        public ActionResult GetSmartLeadsForPopUp(int? LeadId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (LeadId.HasValue)
            {
                ViewBag.LeadId = LeadId;
            }
            ViewBag.AgreementDocumentHeight = _Util.Facade.GlobalSettingsFacade.GetAgreementDocumentHeightByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            ViewBag.StringHeight = ViewBag.AgreementDocumentHeight + "px";
            return View();
        }

        #region Number to Words for contract term
        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            words = textInfo.ToTitleCase(words);
            return words;
        }
        #endregion
        public ActionResult SmartInstallationAgreement(int? LeadId, int? agreementtempid)
        {
            DateTime FixDate = DateTime.Now.UTCCurrentTime();
            InstallationAgreementModel Model = new InstallationAgreementModel();
            Model.EmergencyContactList = new List<EmergencyContact>();
            Model.ListAgreementAnswer = new List<AgreementAnswer>();
            Model.EquipmentList = new List<Equipment>();
            Model.CustomerAgreement = new List<CustomerAgreement>();
            Model.SingleCustomerAgreement = new CustomerAgreement();
            var taxtotal = 0.0;
            Guid CompanyId = new Guid();
            if (User.Identity.IsAuthenticated)
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CompanyId = CurrentUser.CompanyId.Value;
            }
            else
            {
                CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(LeadId.Value);
                CompanyId = custommerCompany.CompanyId;
            }
            Customer Cus = new Customer();
            Company Com = new Company();
            if (LeadId.HasValue)
            {
                if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(LeadId.Value, CompanyId))
                {
                    return null;
                }

                Cus = _Util.Facade.CustomerFacade.GetCustomerById(LeadId.Value);
                Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);

                string ContractTerm = "";
                string ContractTermInWord = "";
                if (!string.IsNullOrWhiteSpace(Cus.ContractTeam) && Cus.ContractTeam != "-1")
                {
                    if (Cus.ContractTeam.ToLower() == "month to month")
                    {
                        ContractTerm = Cus.ContractTeam;
                        ContractTermInWord = Cus.ContractTeam;
                    }
                    else
                    {
                        ContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))).ToString();
                        ContractTermInWord = NumberToWords((Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))));
                    }

                }
                var objCusAgree = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByComIdAndCusIsAndLoadAgreement(Com.CompanyId, Cus.CustomerId);
                if (objCusAgree == null)
                {
                    CustomerAgreement objCustomerAgreement = new CustomerAgreement()
                    {
                        CompanyId = Com.CompanyId,
                        CustomerId = Cus.CustomerId,
                        IP = AppConfig.GetIP,
                        UserAgent = AppConfig.GetUserAgent,
                        Type = LabelHelper.CustomerAgreementLog.LoadAgreement,
                        AddedDate = DateTime.UtcNow
                    };
                    _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objCustomerAgreement);
                }
                var UpfrontAddOnTotal = 0.0;
                var UpfrontAddOnTotalPromo = 0.0;
                bool IsUpfrontPromo = false;
                bool IsServicePromo = false;
                var MonthlyServiceFeeTotal = 0.0;
                var TotalMonthlyMonitoring = 0.0;
                var NewSubTotal = 0.0;
                var TotalDueAtSigning = 0.0;
                var EquipmentTotalPrice = 0.0;
                var ServiceTotalPrice = 0.0;
                var AgreementSubtotal = 0.0;
                var AgreementTotal = 0.0;
                var AgreementTax = 0.0;
                string InstallTypeName = "";
                bool IsNonConfirming = false;
                var NonConfirmingFee = 0.0;
                var AdvanceServiceFeeTotal = 0.0;
                if (Cus.CreditScoreValue == null)
                {
                    Cus.CreditScoreValue = 0;
                }
                var PackageCustomer = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerId(Cus.CustomerId);
                if (PackageCustomer.NonConforming && PackageCustomer.NonConformingFee > 0 && (Cus.CreditScoreValue < PackageCustomer.MinCredit || Cus.CreditScoreValue > PackageCustomer.MaxCredit))
                {
                    IsNonConfirming = true;
                    NonConfirmingFee = PackageCustomer.NonConformingFee.Value;
                }

                var SmartPackageEquipmentServiceList = new List<SmartPackageEquipmentService>();
                var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(Cus.CustomerId, Com.CompanyId);
                if (GetCityTaxList.Count > 0)
                {
                    foreach (var item1 in GetCityTaxList)
                    {
                        AgreementTax = item1.Rate;
                    }
                }
                else
                {
                    Guid tempCustomerId = new Guid();
                    if (Cus != null)
                    {
                        tempCustomerId = Cus.CustomerId;
                    }
                    var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(Com.CompanyId, tempCustomerId);
                    if (GetSalesTax != null)
                    {
                        AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                    }
                }
                var CustomEquipmentList = _Util.Facade.EquipmentFacade.GetSmartEquipmentListByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId, false, 0);
                if (CustomEquipmentList.Count > 0)
                {
                    foreach (var item in CustomEquipmentList)
                    {
                        EquipmentTotalPrice += item.Total;
                        UpfrontAddOnTotal += item.Total;
                    }
                }

                var CustomServiceList = _Util.Facade.EquipmentFacade.GetSmartServiceListByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId, false, 0);
                if (CustomServiceList.Count > 0)
                {
                    foreach (var item in CustomServiceList)
                    {
                        EquipmentTotalPrice += item.Total;
                        ServiceTotalPrice += item.Total;
                        MonthlyServiceFeeTotal += item.Total;
                    }
                }
                #region Advance Monitoring Service Month

                PaymentInfoCustomer paycus = new PaymentInfoCustomer();
                paycus = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentInfoCustomerByCustomerIdAndPayForService(Cus.CustomerId);
                int ForMonth = 1;
                if (paycus != null && paycus.ForMonths.HasValue)
                {
                    ForMonth = paycus.ForMonths.Value;
                }
                if (ForMonth > 1)
                {
                    AdvanceServiceFeeTotal = MonthlyServiceFeeTotal * (ForMonth - 1);

                }
                #endregion
                Cus.MonthlyMonitoringFee = Convert.ToString(ServiceTotalPrice);
                TotalMonthlyMonitoring = MonthlyServiceFeeTotal;
                NewSubTotal = TotalMonthlyMonitoring + UpfrontAddOnTotal;

                if (CustomServiceList.Count > 0 || CustomEquipmentList.Count > 0)
                {
                    if (Cus.ActivationFee.HasValue)
                    {
                        AgreementSubtotal = Cus.ActivationFee.Value + EquipmentTotalPrice;
                        NewSubTotal = Cus.ActivationFee.Value + EquipmentTotalPrice;
                    }
                    else
                    {
                        AgreementSubtotal = EquipmentTotalPrice;
                        NewSubTotal = EquipmentTotalPrice;
                    }
                }
                if (IsNonConfirming && NonConfirmingFee > 0)
                {
                    AgreementSubtotal = AgreementSubtotal + NonConfirmingFee;
                    NewSubTotal = NewSubTotal + NonConfirmingFee;
                }
                if (AgreementTax != 0.0)
                {
                    taxtotal = (AgreementSubtotal / 100) * AgreementTax;
                    Model.TaxTotal = taxtotal;
                    AgreementTotal = AgreementSubtotal + taxtotal;
                    TotalDueAtSigning = NewSubTotal + taxtotal;
                }
                else
                {
                    Model.TaxTotal = 0.0;
                    AgreementTotal = AgreementSubtotal;
                    TotalDueAtSigning = NewSubTotal;
                }
                var PackageCustomerDetails = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerIdandCompanyId(Cus.CustomerId, CompanyId);
                if (PackageCustomerDetails != null)
                {
                    InstallTypeName = _Util.Facade.PackageFacade.SmartInstallTypeNameByInstallTypeId(Convert.ToInt32(PackageCustomerDetails.SmartInstallTypeId));
                    SmartPackageEquipmentServiceList = _Util.Facade.PackageFacade.GetAllSmartPackageIncludeEquipmentByPackageIdAndCompanyId(PackageCustomerDetails.PackageId, CompanyId);
                }
                var PaymentDetails = _Util.Facade.PaymentInfoFacade.GetAllPaymentInfoByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId).Where(m => m.PayFor == "First Month").FirstOrDefault();
                #region For Promo Pyment Method
                List<PaymentInfoCustomer> paycusList = new List<PaymentInfoCustomer>();
                PaymentProfileCustomer paymentProfile = new PaymentProfileCustomer();
                paycusList = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentInfoCustomerByCustomerId(Cus.CustomerId);
                if (paycusList != null && paycusList.Count > 0)
                {
                    foreach (var item in paycusList)
                    {
                        paymentProfile = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentProfileByPaymentInfoId(item.PaymentInfoId);
                        if (paymentProfile != null && paymentProfile.Type == LabelHelper.PaymentMethod.Promo)
                        {
                            if (item.Payfor == "Activation Fee")
                            {
                                NonConfirmingFee = 0.0;

                                if (PackageCustomer != null)
                                {
                                    PackageCustomer.ActivationFee = 0.0;
                                }
                                NewSubTotal = NewSubTotal - (PackageCustomer.AdditionFee + NonConfirmingFee);
                            }
                            else if (item.Payfor == "Equipment")
                            {
                                IsUpfrontPromo = true;
                                NewSubTotal = NewSubTotal - UpfrontAddOnTotal;
                            }
                            else if (item.Payfor == "Service")
                            {
                                NewSubTotal = NewSubTotal - TotalMonthlyMonitoring;
                                IsServicePromo = true;

                            }


                        }

                    }

                }
                #endregion
                Model = new InstallationAgreementModel()
                {
                    CSIDNumber = Cus.Id,
                    IsNonConfirming = IsNonConfirming,
                    NonConfirmingFee = NonConfirmingFee,
                    InstallDate = Cus.InstallDate != null ? Convert.ToDateTime(Cus.InstallDate).ToShortDateString() : "",
                    AccountType = Cus.Type,
                    Referredby = Cus.ReferringCustomer != Guid.Empty ? _Util.Facade.CustomerFacade.GetCustomerNameById(Cus.ReferringCustomer) : "",
                    SocialSecurityNumber = Cus.SSN,
                    Owner2ndPhone = Cus.SecondaryPhone,
                    InitialCity = Cus.City,
                    InitialCountry = Cus.Country,
                    InitialState = Cus.State,
                    InitialZip = Cus.ZipCode,
                    BillingCity = Cus.CityPrevious,
                    BillingState = Cus.StatePrevious,
                    BillingZip = Cus.ZipCodePrevious,
                    InstallTypeName = InstallTypeName,
                    SmartPackageEquipmentServiceList = SmartPackageEquipmentServiceList,
                    UpfrontAddOnTotal = UpfrontAddOnTotal,
                    UpfrontAddOnTotalPromo = UpfrontAddOnTotalPromo,
                    IsUpfrontPromo = IsUpfrontPromo,
                    IsServicePromo = IsServicePromo,
                    MonthlyServiceFeeTotal = MonthlyServiceFeeTotal,
                    TotalMonthlyMonitoring = TotalMonthlyMonitoring,
                    NewSubTotal = NewSubTotal,
                    TotalDueAtSigning = TotalDueAtSigning,
                    PaymentDetails = PaymentDetails,

                    BillingAddress = Cus.Street + " " + Cus.City + ", " + Cus.State + " " + Cus.ZipCode,
                    OwnerAddress = Cus.Street + " " + Cus.City + ", " + Cus.State + " " + Cus.ZipCode,
                    OwnerEmail = Cus.EmailAddress,
                    OwnerPhone = Cus.PrimaryPhone,
                    OwnerName = Cus.FirstName + " " + Cus.LastName,
                    EmergencyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId),
                    CompanyName = Com.CompanyName,
                    CompanySate = string.Format("{0}, {1} {2}", Com.City, Com.State, Com.ZipCode),
                    CompanyStreet = Com.Street,
                    CompanyWebsite = Com.Website,
                    CompanyPhone = Com.Phone,
                    SubscribedMonths = ContractTerm,
                    SubscribedMonthsInWord = ContractTermInWord,
                    Password = Cus.Passcode,
                    DateOfTransaction = FixDate.UTCToClientTime(),
                    CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyEmailLogoByCompanyId(CompanyId),
                    EquipmentList = CustomEquipmentList.ToList(),
                    ServiceList = CustomServiceList.ToList(),
                    ActivationFee = Cus.ActivationFee.HasValue ? Cus.ActivationFee.Value : 0,
                    BusinessName = Cus.BusinessName,
                    MonthlyMonitoringFee = Cus.MonthlyMonitoringFee,
                    EffectiveDate = FixDate.UTCToClientTime(),
                    CustomerSignature = Cus.Singature,
                    CustomerSignatureDate = null,
                    CustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByCompanyIdAndCustomerId1(CompanyId, Cus.CustomerId),
                    Subtotal = AgreementSubtotal,
                    Tax = AgreementTax,
                    TaxTotal = taxtotal,
                    Total = AgreementTotal,
                    EContractId = Cus.Id,
                    ListAgreementAnswer = _Util.Facade.AgreementFacade.GetAllAgreementAnswerByCustomerId(Cus.CustomerId),
                    SalesRepresentative = !string.IsNullOrEmpty(Cus.Soldby) ? _Util.Facade.EmployeeFacade.GetEmployeeNumByEmployeeId(new Guid(Cus.Soldby)).ToString() : "",
                    TotalPayments = (!string.IsNullOrWhiteSpace(Cus.MonthlyMonitoringFee) ? Convert.ToDouble(Cus.MonthlyMonitoringFee) : 1) * Convert.ToDouble(ContractTerm),
                    SingleCustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetIpAndUserAgentByCustomerIdAndCompanyId(CompanyId, Cus.CustomerId),
                    AdvanceServiceFeeTotal = AdvanceServiceFeeTotal
                };
            }
            else
            {
                Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CompanyId);
            }

            //  return View(Model);
            ViewBag.CompanyId = Com.CompanyId.ToString();
            Model.CompanyId = Com.CompanyId.ToString();
            Model.CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(Com.CompanyId);
            string body = _Util.Facade.AgreementFacade.MakeSmartAgreementPdf(Model, agreementtempid.HasValue ? agreementtempid.Value : 0);
            ViewBag.Body = body;
            return new ViewAsPdf()
            {
                // FileName = flightPlan.ListingItemDetailsModel.FlightDetails + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Orientation.Portrait,
                //PageMargins = new Margins(10, 0, 0, 0),
                PageMargins = new Margins(10, 2, 10, 3)
            };
        }
        #endregion

        [Authorize]
        [HttpPost]
        public JsonResult CheckScheduleConflict(Ticket Ticket, Guid[] Assigned, Guid[] UserList)
        {
            string Message = "";
            //Message = ScheduleConflictChecker(Ticket, Assigned, UserList);

            //if (!string.IsNullOrWhiteSpace(Message))
            //{
            //    return Json(new { result = false, message = Message });
            //}
            return Json(new { result = true, message = Message });
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddTicketReply(Guid TicketId, string Message, bool isPrivet, bool showoverview, string AppointmentStartTime, string AppointmentEndTime)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            #region validation Check
            if (TicketId == new Guid() || string.IsNullOrWhiteSpace(Message))
            {
                return Json(new { result = true, message = "Invalid data." });
            }
            Ticket Ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);
            if (Ticket == null || Ticket.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = true, message = "Invalid data." });

            }
            if (!string.IsNullOrWhiteSpace(Ticket.BookingId) && (Ticket.TicketType == LabelHelper.TicketType.PickUp
                || Ticket.TicketType == LabelHelper.TicketType.Service || Ticket.TicketType == LabelHelper.TicketType.DropOff))
            {
                List<Ticket> tickets = _Util.Facade.TicketFacade.GetAllTicketByBookingId(Ticket.BookingId);
                foreach (var item in tickets)
                {
                    if (item.TicketId != TicketId)
                    {
                        TicketReply TRTMP = new TicketReply()
                        {
                            Message = Message + @" 
Added from Ticket #" + Ticket.Id,
                            TicketId = item.TicketId,
                            RepliedDate = DateTime.Now.UTCCurrentTime(),
                            IsPrivate = isPrivet,
                            UserId = CurrentUser.UserId,
                            IsOverview = showoverview
                        };

                        TRTMP.Id = _Util.Facade.TicketFacade.InsertTicketReply(TRTMP);
                    }
                }
            }

            Customer Customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Ticket.CustomerId);
            #endregion
            List<Employee> employee = _Util.Facade.EmployeeFacade.GetEmployeeByuserIdandCompanyId(CurrentUser.CompanyId.Value, CurrentUser.UserId);

            TicketReply TR = new TicketReply()
            {
                Message = Message,
                TicketId = TicketId,
                RepliedDate = DateTime.Now.UTCCurrentTime(),
                IsPrivate = isPrivet,
                UserId = CurrentUser.UserId,
                IsOverview = showoverview
            };

            TR.Id = _Util.Facade.TicketFacade.InsertTicketReply(TR);
            #region InsertOnNoteRemainder
            CustomerNote cusNote = new CustomerNote()
            {
                Notes = Message,
                NoteType = "Ticket",
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                CreatedBy = CurrentUser.UserId.ToString(),
                CreatedByUid = CurrentUser.UserId,
                CustomerId = Ticket.CustomerId,
                CompanyId = CurrentUser.CompanyId.Value,
                IsOverview = showoverview,
                ReferenceTicketId = Ticket.Id

            };
            if (TR.IsPrivate == true)
            {
                cusNote.IsActive = false;
            }
            else
            {
                cusNote.IsActive = true;
            }

            _Util.Facade.NotesFacade.InsertCustomerNote(cusNote);
            #endregion

            #region Send Ticket Reply Notification Email

            List<TicketUser> UserList = _Util.Facade.TicketFacade.GetTicketUserListByTicketId(TicketId);

            string ToEmailList = "";
            if (UserList != null && UserList.Count() > 0)
            {
                bool SendNotification = _Util.Facade.GlobalSettingsFacade.SendNotificationToTicketAdditionalMembers(CurrentUser.CompanyId.Value);

                List<Guid> assigned = new List<Guid>();
                if (SendNotification)
                {
                    UserList.Select(x => x.UserId).GroupBy(x => x).Select(x => x.Key).ToList();
                }
                else
                {
                    UserList.Where(x => x.IsPrimary || (x.NotificationOnly.HasValue && x.NotificationOnly.Value)).Select(x => x.UserId).GroupBy(x => x).Select(x => x.Key).ToList();
                }

                // Creator will also get notification - 8/27/2020 requested by Alif Sec.
                //if (assigned.Where(x => x == Ticket.CreatedBy).Count() == 0)
                //{
                //    if (Ticket.CreatedBy != CurrentUser.UserId)
                //    {
                //        assigned.Add(Ticket.CreatedBy);
                //    }
                //}
                foreach (var item in UserList)
                {
                    assigned.Add(item.UserId);
                }

                string CustomerNameforNotification = Customer.FirstName + " " + Customer.LastName;
                if (Customer.Type == LabelHelper.CustomerType.Commercial && !string.IsNullOrWhiteSpace(Customer.BusinessName))
                {
                    CustomerNameforNotification = Customer.BusinessName;
                }
                #region Insert notification
                Notification notification = new Notification()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    NotificationId = Guid.NewGuid(),
                    Type = CurrentUser.UserTags == "Customer" ? LabelHelper.NotificationType.Customer : LabelHelper.NotificationType.Employee,
                    Who = CurrentUser.UserId,
                    What = string.Format(@"A new reply has been added by {0} (<a class=""cus-anchor"" href=""/UserInformation/?id={2}"">{2}</a>) on Ticket #{1} For Customer {3}  (<a class=""cus-anchor"" href=""/Customer/Customerdetail/?id={4}"">{4}</a>)", "{0}", Ticket.Id, employee.FirstOrDefault().UserLoginId, CustomerNameforNotification, Customer.Id),
                    NotificationUrl = AppConfig.DomainSitePath + "/Ticket/AddTicket/?Id=" + Ticket.Id,

                };
                _Util.Facade.NotificationFacade.InsertNotification(notification);

                #endregion
                foreach (Guid item in assigned)
                {
                    //if (item != CurrentUser.UserId)
                    //{
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(item);
                    if (emp != null && emp.Email.IsValidEmailAddress())
                    {
                        ToEmailList += string.Format("{0}>{1} {2};", emp.Email, emp.FirstName, emp.LastName);
                    }
                    #region set user to notification
                    NotificationUser nu = new NotificationUser()
                    {
                        NotificationId = notification.NotificationId,
                        IsRead = false,
                        NotificationPerson = item,
                    };
                    _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                    #endregion
                    //}
                }
            }

            if (!string.IsNullOrWhiteSpace(ToEmailList))
            {
                string CustomerName = Customer.FirstName + " " + Customer.LastName;
                if (Customer.Type == LabelHelper.CustomerType.Commercial && !string.IsNullOrWhiteSpace(Customer.BusinessName))
                {
                    CustomerName = Customer.BusinessName;
                }


                #region Create Body

                CreateTicketModel model = new CreateTicketModel();
                model.Ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);
                model.CustomerAppointmentEquipmentList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(TicketId);
                model.TicketReplyList = _Util.Facade.TicketFacade.GetAllTicketReplyByTicketId(TicketId, null);
                model.TicketUserList = _Util.Facade.TicketFacade.GetTicketUserListByTicketId(model.Ticket.TicketId);
                model.TicketAssignedUserList = model.TicketUserList.Where(x => x.IsPrimary == true).ToList();

                string Contents = "";

                #region Top blue boxes
                Contents += @"<p style='width:50%;float:left; background-color:#4f90bb;height:30px;padding-top:10px;padding-left:10px; color:white'><b>" + model.Ticket.TicketTypeVal + "</b> #" + model.Ticket.Id + " </p> ";
                Contents += @"<p style='width:50%;float:left; background-color:#4f90bb;height:30px;padding-top:10px;padding-left:10px; color:white'><b>Scheduled On: </b>" + model.Ticket.CompletionDate.ToString("MM/dd/yyyy") + ((!string.IsNullOrWhiteSpace(model.Ticket.AppointmentStartTimeVal) && model.Ticket.AppointmentStartTime != "-1") ? "at " + model.Ticket.AppointmentStartTimeVal : "") + "</p>";
                if (model.TicketAssignedUserList != null && model.TicketAssignedUserList.Count() > 0)
                    Contents += @"<p style='width:50%;float:left; background-color:#4f90bb;min-height:30px;padding-top:10px;padding-bottom:10px;padding-left:10px;color:white'><b>Assigned To: </b>" + string.Join(", ", model.TicketAssignedUserList.Select(x => x.FullName)) + "</p>";

                if (model.TicketUserList.Count > 0)
                {
                    Contents += @"<p style='width:50%;float:left;background-color:#4f90bb;min-height:30px;padding-top:10px;padding-bottom:10px;padding-left:10px;color:white'><b> Additional Members: </b>" + string.Join(", ", model.TicketUserList.Select(x => x.FullName)) + "</p>";
                }
                #endregion

                #region EquipmentList 
                if (model.CustomerAppointmentEquipmentList != null && model.CustomerAppointmentEquipmentList.Count() > 0)
                {
                    Contents += "<br /><br />" +
                        "<div style='margin-top:20px;width:100%;'>" +
                            "<table style='width:100%;'>" +
                                "<thead>" +
                                    "<tr style='background-color:#4f90bb; color:white;width:100%'>" +
                                        "<th style='width:30%;text-align:left;padding-left:40px'>Products/Services</th>" +
                                        "<th style='width:60%;'>Description</th>" +
                                        "<th style='width:10%;'>QTY</th>" +
                                    "</tr>" +
                                "</thead>" +
                            "<tbody>";
                    string data = "";
                    foreach (var item in model.CustomerAppointmentEquipmentList)
                    {
                        data += "<tr>" +
                            "<td style='padding-left:40px;'>" +
                                 "<b>" + item.EquipName + "</b><br />" +
                            "</td>" +
                            "<td>" +
                                "<span>" + item.EquipDetail + "</span>" +
                            "</td>" +
                            "<td style='text-align:center'>" + item.Quantity + "</td>" +
                          "</tr>";
                    }
                    Contents += data + "</tbody>" + "</table>" + "</div>";
                    Contents += "<br /><br />";

                }

                #endregion

                #region Body
                string Body = "<br /><br />";
                Body += "<table style='width:100%;'>" +
                    "<thead>" +
                        "<tr style='background-color:#4f90bb; color:white;width:100%'>" +
                            "<th style='width:30%;text-align:left;padding-left:40px'>Action</th>" +
                            "<th style='width: 70 %;'>Description</th>" +
                        "</tr>" +
                    "</thead>" +
                    "<tbody>" +
                    "<tr>" +
                        "<td valign='top'>" +
                            "<b> <p>" +
                                "<span>  Created By " + model.Ticket.CreatedByVal + "</span><br />" +
                                "<span>on " + string.Format(model.Ticket.CreatedDate.UTCToClientTime().ToString("MM/dd/yy hh:mm tt")) + "</span>" +
                            "</p> </b>" +
                        "</td>" +
                        "<td>" + model.Ticket.Message + "</td>"
                    + "</tr>";
                string TicketReplies = "";
                foreach (var reply in model.TicketReplyList)
                {
                    TicketReplies +=
                        "<tr>" +
                        "<td valign='top'>" + "<p>" +
                            "<span>";
                    if (reply.ReplyType == "File")
                    {
                        TicketReplies += "<span>Uploaded by </span>";
                    }
                    else if (reply.Message.IndexOf("<span>Attached an") == 0)
                    {
                        TicketReplies += "<span>Attached by </span>";
                    }
                    else if (reply.Message.IndexOf("<span data='itemsremoved'>") == 0)
                    {
                        TicketReplies += "<span>Products Removed by </span>";
                    }
                    else if (reply.Message.IndexOf("<span data='itemsadded'>") == 0)
                    {
                        TicketReplies += "<span>Products Added by </span>";
                    }
                    else
                    {
                        TicketReplies += "<span>Reply from </span>";
                    }
                    TicketReplies += "<b>" + reply.CreatedByVal + "</b>" +
                "</span><br />" +
                "<span>on " + string.Format(reply.RepliedDate.UTCToClientTime().ToString("MM/dd/yy hh:mm tt")) + "</span>" +
                "</p>" + "</td>" +
                "<td>" +
                "<p>" + reply.Message + "</p>" +
            "</td>" +
            "</tr>";
                }
                Body += TicketReplies +
                "</tbody>" +
        "</table>" +
        "<br />";
                Contents += Body;
                #endregion

                #endregion


                TicketNotificationEmails TicketReplyNotificationEmail = new TicketNotificationEmails()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    CreatedByName = CurrentUser.GetFullName(),
                    TicketMessage = TR.Message,
                    CreatedForCustomerName = CustomerName,
                    TicketNumber = string.Format("Ticket #{0}", Ticket.Id),
                    ToEmail = ToEmailList,
                    Subject = string.Format("A new reply has been added to Ticket #{0}", Ticket.Id),
                    CompletionDate = model.Ticket.CompletionDate,
                    CustomerAddress = MakeAddress(Customer.Street, " " + Customer.City, " " + Customer.State, " " + Customer.ZipCode, " " + Customer.Country),
                    AppointmentEndTime = AppointmentEndTime,
                    AppointmentStartTime = AppointmentStartTime,

                    //BodyMessage = string.Format("A new reply has been added by {0} on Ticket #{1} for Customer {2}.", CurrentUser.GetFullName(), Ticket.Id, CustomerName),
                    BodyMessage = Contents,
                    TicketUrl = string.Format("{2}{4}/Public/Open/?TicketId={0}&CompanyId={1}&Type={3}", Ticket.TicketId, CurrentUser.CompanyId.Value, AppConfig.SiteDomain, LabelHelper.PublicOpenTypes.Ticket, AppConfig.DomainSitePath)
                };

                if (!String.IsNullOrWhiteSpace(TicketReplyNotificationEmail.TicketMessage))
                {
                    TicketReplyNotificationEmail.TicketMessage = LabelHelper.HtmlToPlainText(TicketReplyNotificationEmail.TicketMessage);
                }


                if (TicketReplyNotificationEmail.AppointmentStartTime == "-1")
                {
                    TicketReplyNotificationEmail.AppointmentStartTime = "";
                }
                if (TicketReplyNotificationEmail.AppointmentEndTime == "-1")
                {
                    TicketReplyNotificationEmail.AppointmentEndTime = "";
                }
                List<Lookup> lkk = new List<Lookup>();
                if (TicketReplyNotificationEmail.AppointmentStartTime != "-1" && TicketReplyNotificationEmail.AppointmentStartTime != "" && !String.IsNullOrWhiteSpace(TicketReplyNotificationEmail.AppointmentStartTime))
                {
                    lkk = _Util.Facade.EmployeeFacade.GetLookupDisplaytext(TicketReplyNotificationEmail.AppointmentStartTime);
                    TicketReplyNotificationEmail.AppointmentStartTime = lkk.FirstOrDefault().DisplayText;
                }
                if (TicketReplyNotificationEmail.AppointmentEndTime != "-1" && TicketReplyNotificationEmail.AppointmentEndTime != "" && !String.IsNullOrWhiteSpace(TicketReplyNotificationEmail.AppointmentEndTime))
                {
                    lkk = _Util.Facade.EmployeeFacade.GetLookupDisplaytext(TicketReplyNotificationEmail.AppointmentEndTime);
                    TicketReplyNotificationEmail.AppointmentEndTime = lkk.FirstOrDefault().DisplayText;
                }
                _Util.Facade.MailFacade.SendTicketCreatedNotificationEmail(TicketReplyNotificationEmail);
            }
            #endregion

            return Json(new { result = true, message = "Reply added successfully." });
        }

        public JsonResult UpdateCustomerAppoinment(CustomerAppointment CustomerAppointment,
            //List<string> AddedEquipmentList, List<string> RemovedEquipmentList, 
            Ticket Ticket, Guid[] Assigned, string EquipmentType)
        {
            bool IsNewEqp = false;
            bool EquipmentQTYChanged = false;
            bool EquipmentpriceChanged = false;
            bool ServiceQTYChanged = false;
            bool ServicePriceChanged = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var TempTicket = _Util.Facade.TicketFacade.GetTicketByTicketId(Ticket.TicketId);
            #region TicketUser
            _Util.Facade.TicketFacade.DeleteTicketUserByTicketId(Ticket.TicketId, true);
            if (Assigned != null && Assigned.Count() > 0)
            {
                foreach (var item in Assigned)
                {
                    var TicketUser = new TicketUser()
                    {
                        AddedBy = CurrentUser.UserId,
                        AddedDate = DateTime.Now.UTCCurrentTime(),
                        UserId = item,
                        IsPrimary = true,
                        TiketId = Ticket.TicketId,
                        NotificationOnly = false,
                    };
                    _Util.Facade.TicketFacade.InsertTicketUser(TicketUser);
                }
            }
            #endregion
            CustomerAppointment Temp = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentDetailByAppointmentId(CustomerAppointment.AppointmentId);
            if (Temp != null)
            {
                Temp.TaxPercent = CustomerAppointment.TaxPercent;
                Temp.TaxTotal = CustomerAppointment.TaxTotal;
                Temp.TaxType = CustomerAppointment.TaxType;
                Temp.TotalAmountTax = CustomerAppointment.TotalAmountTax;
                Temp.TotalAmount = CustomerAppointment.TotalAmount;
                Temp.LastUpdatedBy = User.Identity.Name;
                Temp.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinment(Temp);
                List<CustomerAppointmentEquipment> TicketItemList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipmentByTicketId(CurrentUser.CompanyId.Value, Temp.AppointmentId);
                #region Insert Equipment/Service

                List<string> AddedEquipmentList = new List<string>();
                List<string> RemovedEquipmentList = new List<string>();
                var PackageEqpList = _Util.Facade.SmartPackageFacade.GetAllPackageEqpListByCustomerIdAndCompanyIdAndAppointmentId(TempTicket.CustomerId, CurrentUser.CompanyId.Value, TempTicket.Id);
                var PackageServiceList = _Util.Facade.SmartPackageFacade.GetAllPackageServiceListByCustomerIdAndCompanyIdAndAppointmentId(TempTicket.CustomerId, CurrentUser.CompanyId.Value, TempTicket.Id);
                if (PackageEqpList != null && PackageEqpList.Count > 0)
                {
                    foreach (var eqp in PackageEqpList)
                    {
                        _Util.Facade.PackageFacade.DeleteCustomerPackageEqpById(eqp.Id);
                    }
                }
                if (PackageServiceList != null && PackageServiceList.Count > 0)
                {
                    foreach (var service in PackageServiceList)
                    {
                        _Util.Facade.PackageFacade.DeleteCustomerPackageServiceById(service.Id);
                    }
                }
                if (CustomerAppointment.CustomerAppointmentEquipmentList != null && CustomerAppointment.CustomerAppointmentEquipmentList.Count() > 0)
                {
                    foreach (CustomerAppointmentEquipment item in CustomerAppointment.CustomerAppointmentEquipmentList)
                    {
                        IsNewEqp = false;
                        if (item.Id > 0 && TicketItemList.Where(x => x.Id == item.Id).Count() > 0)
                        {
                            #region Update CustomerAppointmentEquipment
                            CustomerAppointmentEquipment tempitem = TicketItemList.Where(x => x.Id == item.Id).FirstOrDefault();
                            item.CreatedBy = tempitem.CreatedBy;

                            if (item.CreatedByUid == Guid.Empty)
                            {
                                item.CreatedByUid = tempitem.CreatedByUid;
                            }
                            else if (item.CreatedByUid != tempitem.CreatedByUid)
                            {
                                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item.CreatedByUid);
                                Employee Previous = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(tempitem.CreatedByUid);
                                if (emp != null)
                                {
                                    item.CreatedBy = emp.FirstName + " " + emp.LastName;

                                    Equipment eqp = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(item.EquipmentId);
                                    if (eqp != null)
                                    {
                                        item.CreatedByUid = emp.UserId;
                                        string AddedItem = "<span data='{3}'><a class='cus-anchor' href='{0}'>{1}</a><br/> <span>Sales Person Changed From: <b>{2}</b> <br/>TO: <b>{4}</b> By: <b>{5}</b></span> <br/></span>";
                                        if (eqp.EquipmentClassId == 2)
                                        {
                                            //service///Inventory/ServiceDetail/2049
                                            AddedItem = string.Format(AddedItem,
                                                "/Inventory/ServiceDetail/" + eqp.Id, //0
                                                item.EquipName, //1
                                                Previous.FirstName + " " + Previous.LastName, //2
                                                "servicesold",//3
                                                item.CreatedBy,//4
                                                CurrentUser.GetFullName()
                                                );
                                        }
                                        else
                                        {
                                            //equipment/Inventory/EquipmentDetail/1631//itemssold
                                            AddedItem = string.Format(AddedItem,
                                               "/Inventory/EquipmentDetail/" + eqp.Id, //0
                                               item.EquipName, //1
                                               Previous.FirstName + " " + Previous.LastName, //2
                                               "itemssold",//3
                                               item.CreatedBy,//4
                                               CurrentUser.GetFullName()
                                               );
                                        }

                                        TicketReply tkrply = new TicketReply()
                                        {
                                            RepliedDate = DateTime.Now.UTCCurrentTime(),
                                            TicketId = Temp.AppointmentId,
                                            UserId = CurrentUser.UserId,
                                            Message = AddedItem //string.Format(@"{0}", AddedItem, (EquipmentType == LabelHelper.EquipmentType.Service ? "servicesadded" : "itemsadded"))
                                        };
                                        tkrply.Id = _Util.Facade.TicketFacade.InsertTicketReply(tkrply);
                                    }

                                }

                            }
                            item.CreatedDate = tempitem.CreatedDate;
                            item.IsAgreementItem = tempitem.IsAgreementItem;
                            item.IsBaseItem = tempitem.IsBaseItem;
                            item.AppointmentId = tempitem.AppointmentId;
                            item.IsBadInventory = tempitem.IsBadInventory;
                            item.IsDefaultService = tempitem.IsDefaultService;
                            item.OriginalUnitPrice = tempitem.OriginalUnitPrice;
                            item.IsEquipmentExist = tempitem.IsEquipmentExist;
                            item.IsNonCommissionable = tempitem.IsNonCommissionable;
                            _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(item);

                            TicketItemList.Remove(tempitem);
                            #endregion

                        }
                        else
                        {
                            IsNewEqp = true;
                            #region Insert CustomerAppointmentEquipment
                            item.CreatedBy = CurrentUser.GetFullName();
                            if (item.CreatedByUid == Guid.Empty)
                            {
                                item.CreatedByUid = CurrentUser.UserId;
                            }
                            else
                            {
                                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item.CreatedByUid);
                                if (emp != null)
                                {
                                    item.CreatedBy = emp.FirstName + " " + emp.LastName;
                                }
                            }
                            item.CreatedDate = DateTime.Now.UTCCurrentTime();
                            item.AppointmentId = Temp.AppointmentId;
                            item.IsBaseItem = false;
                            item.IsAgreementItem = false;
                            _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppointmentEquipmentDetail(item);

                            #region AddedItems In ticket reply
                            string AddedItem = "";
                            Equipment eqp = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(item.EquipmentId);
                            if (eqp != null)
                            {
                                AddedItem = "<a class='cus-anchor' href='{0}'>{1}</a><br/> <span>Sold By: {2}</span> <br/>";
                                if (eqp.EquipmentClassId == 2)
                                {
                                    //service///Inventory/ServiceDetail/2049
                                    AddedItem = string.Format(AddedItem, "/Inventory/ServiceDetail/" + eqp.Id, item.EquipName, item.CreatedBy);
                                }
                                else
                                {
                                    //equipment/Inventory/EquipmentDetail/1631
                                    AddedItem = string.Format(AddedItem, "/Inventory/EquipmentDetail/" + eqp.Id, item.EquipName, item.CreatedBy);
                                }
                            }
                            else
                            {
                                AddedItem = item.EquipName;
                            }
                            AddedEquipmentList.Add(AddedItem);
                            #endregion

                            #endregion
                        }

                        #region No longer in use
                        //var objpackagecustomer = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerId(TempTicket.CustomerId);
                        //if (objpackagecustomer != null)
                        //{
                        //    if (item.IsService == false)
                        //    {
                        //        var objeqp = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(item.EquipmentId);
                        //        var packageeqp = _Util.Facade.PackageFacade.GetPackageEqpByCustomerIdAndAppointmentIdAndPackageIdAndEquipmentId(TempTicket.CustomerId, TempTicket.Id, objpackagecustomer.PackageId, item.EquipmentId);
                        //        if (packageeqp != null)
                        //        {
                        //            if (IsNewEqp == true)
                        //            {
                        //                CustomerPackageEqp CustomerPackageEqp = new CustomerPackageEqp()
                        //                {
                        //                    CompanyId = CurrentUser.CompanyId.Value,
                        //                    CustomerId = TempTicket.CustomerId,
                        //                    PackageId = objpackagecustomer.PackageId,
                        //                    EquipmentId = item.EquipmentId,
                        //                    IsIncluded = false,
                        //                    IsDevice = false,
                        //                    IsOptionalEqp = false,
                        //                    Quantity = item.Quantity,
                        //                    UnitPrice = item.UnitPrice,
                        //                    DiscountUnitPricce = 0.0,
                        //                    DiscountPckage = 0.0,
                        //                    Total = item.TotalPrice,
                        //                    IsServiceEquipment = false,
                        //                    ServiceId = new Guid(),
                        //                    IsTransfered = false,
                        //                    IsEqpExist = false,
                        //                    IsPackageEqp = false,
                        //                    IsNonCommissionable = false,
                        //                    AppointmentIntId = TempTicket.Id,
                        //                    AppointmentEquipmentIntId = item.Id
                        //                };
                        //                _Util.Facade.PackageFacade.InsertCustomerPackageEqp(CustomerPackageEqp);
                        //            }
                        //            else
                        //            {
                        //                packageeqp.UnitPrice = item.UnitPrice;
                        //                packageeqp.Quantity = item.Quantity;
                        //                packageeqp.DiscountUnitPricce = 0.0;
                        //                packageeqp.DiscountPckage = 0.0;
                        //                packageeqp.Total = item.TotalPrice;
                        //                _Util.Facade.SmartPackageFacade.UpdateCustomerPackageEqp(packageeqp);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            var objpackageeqp = _Util.Facade.PackageFacade.GetPackageCustomerEqpByCustomerIdAndPackageIdAndEquipmentId(objpackagecustomer.PackageId, TempTicket.CustomerId, item.EquipmentId);
                        //            if (objpackageeqp == null)
                        //            {
                        //                CustomerPackageEqp CustomerPackageEqp = new CustomerPackageEqp()
                        //                {
                        //                    CompanyId = CurrentUser.CompanyId.Value,
                        //                    CustomerId = TempTicket.CustomerId,
                        //                    PackageId = objpackagecustomer.PackageId,
                        //                    EquipmentId = item.EquipmentId,
                        //                    IsIncluded = false,
                        //                    IsDevice = false,
                        //                    IsOptionalEqp = false,
                        //                    Quantity = item.Quantity,
                        //                    UnitPrice = item.UnitPrice,
                        //                    DiscountUnitPricce = 0.0,
                        //                    DiscountPckage = 0.0,
                        //                    Total = item.TotalPrice,
                        //                    IsServiceEquipment = false,
                        //                    ServiceId = new Guid(),
                        //                    IsTransfered = false,
                        //                    IsEqpExist = false,
                        //                    IsPackageEqp = false,
                        //                    IsNonCommissionable = false,
                        //                    AppointmentIntId = TempTicket.Id,
                        //                    AppointmentEquipmentIntId = item.Id
                        //                };
                        //                _Util.Facade.PackageFacade.InsertCustomerPackageEqp(CustomerPackageEqp);
                        //            }
                        //            else if (IsNewEqp == true)
                        //            {
                        //                CustomerPackageEqp CustomerPackageEqp = new CustomerPackageEqp()
                        //                {
                        //                    CompanyId = CurrentUser.CompanyId.Value,
                        //                    CustomerId = TempTicket.CustomerId,
                        //                    PackageId = objpackagecustomer.PackageId,
                        //                    EquipmentId = item.EquipmentId,
                        //                    IsIncluded = false,
                        //                    IsDevice = false,
                        //                    IsOptionalEqp = false,
                        //                    Quantity = item.Quantity,
                        //                    UnitPrice = item.UnitPrice,
                        //                    DiscountUnitPricce = 0.0,
                        //                    DiscountPckage = 0.0,
                        //                    Total = item.TotalPrice,
                        //                    IsServiceEquipment = false,
                        //                    ServiceId = new Guid(),
                        //                    IsTransfered = false,
                        //                    IsEqpExist = false,
                        //                    IsPackageEqp = false,
                        //                    IsNonCommissionable = false,
                        //                    AppointmentIntId = TempTicket.Id,
                        //                    AppointmentEquipmentIntId = item.Id
                        //                };
                        //                _Util.Facade.PackageFacade.InsertCustomerPackageEqp(CustomerPackageEqp);
                        //            }
                        //            else
                        //            {
                        //                objpackageeqp.UnitPrice = item.UnitPrice;
                        //                objpackageeqp.Quantity = item.Quantity;
                        //                objpackageeqp.DiscountUnitPricce = 0.0;
                        //                objpackageeqp.DiscountPckage = 0.0;
                        //                objpackageeqp.Total = item.TotalPrice;
                        //                _Util.Facade.SmartPackageFacade.UpdateCustomerPackageEqp(objpackageeqp);
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        var packageservice = _Util.Facade.PackageFacade.GetPackageServiceByCustomerIdAndAppointmentIdAndPackageIdAndEquipmentId(TempTicket.CustomerId, TempTicket.Id, objpackagecustomer.PackageId, item.EquipmentId);
                        //        if (packageservice != null)
                        //        {
                        //            if (IsNewEqp == true)
                        //            {
                        //                CustomerPackageService CustomerPackageService = new CustomerPackageService()
                        //                {
                        //                    CompanyId = CurrentUser.CompanyId.Value,
                        //                    CustomerId = TempTicket.CustomerId,
                        //                    PackageId = objpackagecustomer.PackageId,
                        //                    EquipmentId = item.EquipmentId,
                        //                    MonthlyRate = item.UnitPrice,
                        //                    DiscountRate = 0,
                        //                    Total = item.TotalPrice,
                        //                    ManufacturerId = new Guid(),
                        //                    LocationId = new Guid(),
                        //                    TypeId = new Guid(),
                        //                    ModelId = new Guid(),
                        //                    FinishId = new Guid(),
                        //                    CapacityId = new Guid(),
                        //                    IsPackageService = false,
                        //                    IsNonCommissionable = false,
                        //                    AppointmentIntId = TempTicket.Id,
                        //                    AppointmentEquipmentIntId = item.Id
                        //                };
                        //                _Util.Facade.PackageFacade.InsertCustomerPackageService(CustomerPackageService);
                        //            }
                        //            else
                        //            {
                        //                packageservice.MonthlyRate = item.UnitPrice;
                        //                packageservice.Total = item.TotalPrice;
                        //                _Util.Facade.SmartPackageFacade.UpdateCustomerPackageService(packageservice);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            var objpackageservice = _Util.Facade.PackageFacade.GetPackageCustomerServiceByCustomerIdAndPackageIdAndEquipmentId(objpackagecustomer.PackageId, TempTicket.CustomerId, item.EquipmentId);
                        //            if (objpackageservice == null)
                        //            {
                        //                CustomerPackageService CustomerPackageService = new CustomerPackageService()
                        //                {
                        //                    CompanyId = CurrentUser.CompanyId.Value,
                        //                    CustomerId = TempTicket.CustomerId,
                        //                    PackageId = objpackagecustomer.PackageId,
                        //                    EquipmentId = item.EquipmentId,
                        //                    MonthlyRate = item.UnitPrice,
                        //                    DiscountRate = 0,
                        //                    Total = item.TotalPrice,
                        //                    ManufacturerId = new Guid(),
                        //                    LocationId = new Guid(),
                        //                    TypeId = new Guid(),
                        //                    ModelId = new Guid(),
                        //                    FinishId = new Guid(),
                        //                    CapacityId = new Guid(),
                        //                    IsPackageService = false,
                        //                    IsNonCommissionable = false,
                        //                    AppointmentIntId = TempTicket.Id,
                        //                    AppointmentEquipmentIntId = item.Id
                        //                };
                        //                _Util.Facade.PackageFacade.InsertCustomerPackageService(CustomerPackageService);
                        //            }
                        //            else if (IsNewEqp == true)
                        //            {
                        //                CustomerPackageService CustomerPackageService = new CustomerPackageService()
                        //                {
                        //                    CompanyId = CurrentUser.CompanyId.Value,
                        //                    CustomerId = TempTicket.CustomerId,
                        //                    PackageId = objpackagecustomer.PackageId,
                        //                    EquipmentId = item.EquipmentId,
                        //                    MonthlyRate = item.UnitPrice,
                        //                    DiscountRate = 0,
                        //                    Total = item.TotalPrice,
                        //                    ManufacturerId = new Guid(),
                        //                    LocationId = new Guid(),
                        //                    TypeId = new Guid(),
                        //                    ModelId = new Guid(),
                        //                    FinishId = new Guid(),
                        //                    CapacityId = new Guid(),
                        //                    IsPackageService = false,
                        //                    IsNonCommissionable = false,
                        //                    AppointmentIntId = TempTicket.Id,
                        //                    AppointmentEquipmentIntId = item.Id
                        //                };
                        //                _Util.Facade.PackageFacade.InsertCustomerPackageService(CustomerPackageService);
                        //            }
                        //            else
                        //            {
                        //                objpackageservice.MonthlyRate = item.UnitPrice;
                        //                objpackageservice.Total = item.TotalPrice;
                        //                _Util.Facade.SmartPackageFacade.UpdateCustomerPackageService(objpackageservice);
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion

                        var objpackagecustomer = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerId(TempTicket.CustomerId);
                        if (objpackagecustomer != null)
                        {
                            if (item.IsService == false)
                            {
                                CustomerPackageEqp CustomerPackageEqp = new CustomerPackageEqp()
                                {
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    CustomerId = TempTicket.CustomerId,
                                    PackageId = objpackagecustomer.CustomerId,
                                    EquipmentId = item.EquipmentId,
                                    IsIncluded = false,
                                    IsDevice = false,
                                    IsOptionalEqp = false,
                                    Quantity = item.Quantity,
                                    UnitPrice = item.UnitPrice,
                                    DiscountUnitPricce = 0.0,
                                    DiscountPckage = 0.0,
                                    Total = item.TotalPrice,
                                    IsServiceEquipment = false,
                                    ServiceId = new Guid(),
                                    IsTransfered = false,
                                    IsEqpExist = false,
                                    IsPackageEqp = false,
                                    IsNonCommissionable = false,
                                    AppointmentIntId = TempTicket.Id,
                                    AppointmentEquipmentIntId = item.Id
                                };
                                _Util.Facade.PackageFacade.InsertCustomerPackageEqp(CustomerPackageEqp);
                            }
                            else
                            {
                                CustomerPackageService CustomerPackageService = new CustomerPackageService()
                                {
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    CustomerId = TempTicket.CustomerId,
                                    PackageId = objpackagecustomer.PackageId,
                                    EquipmentId = item.EquipmentId,
                                    MonthlyRate = item.UnitPrice,
                                    DiscountRate = 0,
                                    Total = item.TotalPrice,
                                    ManufacturerId = new Guid(),
                                    LocationId = new Guid(),
                                    TypeId = new Guid(),
                                    ModelId = new Guid(),
                                    FinishId = new Guid(),
                                    CapacityId = new Guid(),
                                    IsPackageService = false,
                                    IsNonCommissionable = false,
                                    AppointmentIntId = TempTicket.Id,
                                    AppointmentEquipmentIntId = item.Id
                                };
                                _Util.Facade.PackageFacade.InsertCustomerPackageService(CustomerPackageService);
                            }
                        }
                    }
                }

                #region If item still exists delete
                if (TicketItemList.Count() > 0)
                {
                    bool IsServiceItems = EquipmentType == LabelHelper.EquipmentType.Service;

                    foreach (var item in TicketItemList)
                    {
                        if (IsServiceItems == item.IsService)
                        {
                            _Util.Facade.CustomerAppoinmentFacade.DeleteCustomerAppoinmentEquipment(item.Id);
                            #region Removed Item In ticket reply
                            string RemovedItem = "";
                            Equipment eqp = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(item.EquipmentId);
                            if (eqp != null)
                            {
                                RemovedItem = "<a class='cus-anchor' href='{0}'>{1}</a>";
                                if (eqp.EquipmentClassId == 2)
                                {
                                    //service///Inventory/ServiceDetail/2049
                                    RemovedItem = string.Format(RemovedItem, "/Inventory/ServiceDetail/" + eqp.Id, item.EquipName);
                                }
                                else
                                {
                                    //equipment/Inventory/EquipmentDetail/1631
                                    RemovedItem = string.Format(RemovedItem, "/Inventory/EquipmentDetail/" + eqp.Id, item.EquipName);
                                }
                            }
                            else
                            {
                                RemovedItem = item.EquipName;
                            }
                            RemovedEquipmentList.Add(RemovedItem);
                            #endregion
                        }
                    }
                }
                #endregion

                #endregion
                if (AddedEquipmentList.Count() > 0)
                {
                    //AddedEquipmentList.RemoveAt(0);
                    string EquipmentList = string.Join("", AddedEquipmentList);
                    TicketReply tkrply = new TicketReply()
                    {
                        RepliedDate = DateTime.Now.UTCCurrentTime(),
                        TicketId = Temp.AppointmentId,
                        UserId = CurrentUser.UserId,
                        Message = string.Format(@"<span data='{1}'>{0}</span>", EquipmentList, (EquipmentType == LabelHelper.EquipmentType.Service ? "servicesadded" : "itemsadded"))
                    };
                    tkrply.Id = _Util.Facade.TicketFacade.InsertTicketReply(tkrply);
                }
                if (RemovedEquipmentList.Count() > 0)
                {
                    //RemovedEquipmentList.RemoveAt(0);
                    string EquipmentList = string.Join(", ", RemovedEquipmentList);
                    TicketReply tkrply = new TicketReply()
                    {
                        RepliedDate = DateTime.Now.UTCCurrentTime(),
                        TicketId = Temp.AppointmentId,
                        UserId = CurrentUser.UserId,
                        Message = string.Format(@"<span data='{1}'>{0}</span>", EquipmentList, (EquipmentType == LabelHelper.EquipmentType.Service ? "servicesremoved" : "itemsremoved"))
                    };
                    tkrply.Id = _Util.Facade.TicketFacade.InsertTicketReply(tkrply);
                }
                return Json(new { result = true, message = string.Format("{0} list updated successfully.", (EquipmentType == LabelHelper.EquipmentType.Service ? "Service" : "Equipment")) });
            }
            return Json(new { result = false, message = "Customer appoinment not found." });
        }


        #region JobStartStop
        [Authorize]
        [HttpPost]
        public JsonResult JobTimeStartStop(TicketTimeClock Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Model.LastUpdateBy = CurrentUser.UserId;
            Model.LastUpdatedDate = DateTime.Now.UTCCurrentTime();

            ViewBag.Minute = "";
            ViewBag.Second = "";
            ViewBag.Hour = "";

            #region Match Ip
            //Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(CurrentUser.UserId);
            //if (!string.IsNullOrWhiteSpace(emp.ClockInIP))
            //{
            //    if (Request.IsLocal)
            //    {

            //    }
            //    else
            //    {
            //        var IpList = emp.ClockInIP.Split(';');
            //        string IPAddress = Request.UserHostAddress;
            //        bool IpMatched = false;
            //        foreach (var item in IpList)
            //        {
            //            if (item.Trim() == IPAddress)
            //            {
            //                IpMatched = true;
            //                break;
            //            }
            //        }
            //        if (!IpMatched)
            //        {
            //            return Json(new { result = false, message = string.Format("Your Ip {0} is not allowed for Clock In/Out please contact your supervisor.", Request.UserHostAddress) });
            //        }
            //    }
            //}
            #endregion

            #region ClockIN/Out Changes

            //CustomPrincipal UserPrincipal = new CustomPrincipal(AppUser, User.Identity);
            //CurrentUser = UserPrincipal;

            if (Model.Type == LabelHelper.TicketTimeClockType.End)
            {
                //TicketTimeClock tclock = _Util.Facade.TicketClockFacade.GetLastClockInByUserIdAndTicketId(Model.UserId, Model.TicketId);
                TicketTimeClock tclock = _Util.Facade.TicketClockFacade.GetLastTicketTimeClockByTicketId(Model.TicketId);
                if (tclock != null && tclock.Type == LabelHelper.TicketTimeClockType.End)
                {
                    return Json(new { result = true, message = "already clockedout" });
                }
                else if (tclock != null)
                {
                    Model.ClockedInMinutes = (int)DateTime.Now.UTCCurrentTime().Subtract(tclock.Time).TotalMilliseconds + 600;
                    Model.Type = LabelHelper.TicketTimeClockType.End;
                }

            }
            else
            {
                Model.Type = LabelHelper.TicketTimeClockType.Start;
                Model.ClockedInMinutes = null;
                //Ticket ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(Model.TicketId);
                List<TicketTimeClock> ticketTimeList = _Util.Facade.TicketClockFacade.GetTicketTimeClockByTicketId(Model.TicketId);
                if (ticketTimeList.Count > 0)
                {
                    TicketTimeClock LastticketTime = ticketTimeList.OrderByDescending(x => x.Id).FirstOrDefault();
                    int Totaltime = 0;
                    if (ticketTimeList.Count == 1)
                    {
                        Totaltime = (int)DateTime.Now.UTCCurrentTime().Subtract(LastticketTime.Time).TotalMilliseconds;
                    }
                    else
                    {

                        foreach (var item in ticketTimeList)
                        {
                            Totaltime = Totaltime + (item.ClockedInMinutes.HasValue ? item.ClockedInMinutes.Value : 0);
                        }
                        if (LastticketTime.Type != LabelHelper.TicketTimeClockType.End)
                        {
                            Totaltime = (int)DateTime.Now.UTCCurrentTime().Subtract(LastticketTime.Time).TotalMilliseconds + Totaltime;
                        }
                    }
                    TimeSpan t = TimeSpan.FromMilliseconds(Convert.ToDouble(Totaltime));
                    ViewBag.Minute = t.Minutes > 0 ? t.Minutes : 0;
                    ViewBag.Second = t.Seconds;
                    ViewBag.Hour = t.Hours > 0 ? t.Hours : 0;

                }
                else
                {
                    ViewBag.Minute = 0;
                    ViewBag.Second = 0;
                    ViewBag.Hour = 0;
                    ViewBag.IsClockedIn = true;
                }

            }
            #endregion


            #region Clock IN/Out Insert
            Model.CreatedBy = CurrentUser.UserId;

            Model.Time = DateTime.Now.UTCCurrentTime();

            //Model.Type = HS.Web.UI.Helper.LabelHelper.TimeClockType.ClockIn;
            _Util.Facade.TicketClockFacade.InsertTimeClock(Model);
            #endregion



            return Json(new { result = true, message = string.Format("{0} successful.", Model.Type), Minute = ViewBag.Minute, Second = ViewBag.Second, Hour = ViewBag.Hour });

        }
        #endregion

        public JsonResult SaveTicketSession(Ticket Ticket)
        {
            Session["TempTicket"] = Ticket;

            return Json(new { result = true, message = "saved to session" });
        }
        public JsonResult SaveSurveyForTicket(CustomSurveyUser SurveyTicket, Guid TicketId, string Email, string SMS)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            string message = "";
            Ticket ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);
            CustomSurvey customSurvey = _Util.Facade.CustomSurveyFacade.GetCustomSurveyBySurveyId(SurveyTicket.SurveyId);
            if (SurveyTicket.SurveyId == Guid.Empty)
            {
                result = false;
                message = "Please select a survey name.";
            }
            else
            {
                ShortUrl ShortUrl = new ShortUrl();
                Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(SurveyTicket.UserId);
                TicketReply TR = new TicketReply()
                {
                    Message = CurrentUser.FirstName + " send " + customSurvey.SurveyName + " survey to " + cus.FirstName,
                    TicketId = TicketId,
                    RepliedDate = DateTime.Now.UTCCurrentTime(),
                    IsPrivate = true,
                    UserId = CurrentUser.UserId
                };
                TR.Id = _Util.Facade.TicketFacade.InsertTicketReply(TR);
                if (cus != null)
                {
                    SurveyTicket.SurveyUserId = Guid.NewGuid();
                    SurveyTicket.Status = LabelHelper.CustomSurveyStatus.Created;
                    SurveyTicket.AddedBy = CurrentUser.UserId;
                    SurveyTicket.AddedDate = DateTime.Now.UTCCurrentTime();
                    SurveyTicket.ReferenceId = TicketId.ToString();
                    _Util.Facade.CustomSurveyFacade.InsertCustomSurveyUser(SurveyTicket);
                    ticket.HasSurvey = true;
                    _Util.Facade.TicketFacade.UpdateTicket(ticket);
                    result = true;
                    string TicketUrl = string.Format("{2}{4}/Public/Open/?TicketId={0}&CompanyId={1}&Type={3}", SurveyTicket.SurveyUserId, CurrentUser.CompanyId.Value, AppConfig.SiteDomain, LabelHelper.PublicOpenTypes.CustomSurvey, AppConfig.DomainSitePath);
                    ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(TicketUrl, Guid.Empty);
                }
                if (!string.IsNullOrWhiteSpace(Email))
                {
                    if (Email.IndexOf(";") > -1)
                    {
                        string[] EmailList = Email.Split(";");
                        if (EmailList.Length > 0)
                        {
                            foreach (var item in EmailList)
                            {
                                SendSurveyEmail email = new SendSurveyEmail()
                                {

                                    Name = cus.FirstName + " " + cus.LastName,
                                    shortLink = ShortUrl.Url,
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    ToEmail = item,
                                    SenderName = CurrentUser.FirstName + " " + CurrentUser.LastName,
                                    CompanyName = CurrentUser.CompanyName,
                                    Subject = string.Format("Survey for Ticket#{0}", ticket.Id),
                                    Header = string.Format("Survey for Ticket#{0}", ticket.Id),

                                };
                                try
                                {
                                    _Util.Facade.MailFacade.SendSurveyEmail(email);
                                    message = "A survey email";
                                    result = true;
                                }
                                catch (Exception ex)
                                {
                                    message = "Internal error. Please contact system admin.";
                                }
                            }
                        }
                    }
                    else
                    {
                        SendSurveyEmail email = new SendSurveyEmail()
                        {

                            Name = cus.FirstName + " " + cus.LastName,
                            shortLink = ShortUrl.Url,
                            CompanyId = CurrentUser.CompanyId.Value,
                            ToEmail = Email,
                            SenderName = CurrentUser.FirstName + " " + CurrentUser.LastName,
                            CompanyName = CurrentUser.CompanyName,
                            Subject = string.Format("Survey for Ticket#{0}", ticket.Id),
                            Header = string.Format("Survey for Ticket#{0}", ticket.Id),

                        };
                        try
                        {
                            _Util.Facade.MailFacade.SendSurveyEmail(email);
                            message = "A survey email";
                            result = true;
                        }
                        catch (Exception ex)
                        {
                            message = "Internal error. Please contact system admin.";
                        }
                    }

                    //ViewAsPdf actionPDF;

                    //    actionPDF = new Rotativa.ViewAsPdf("FileInstallation")
                    //    {
                    //        PageSize = Rotativa.Options.Size.Legal,
                    //        PageOrientation = Rotativa.Options.Orientation.Portrait,
                    //        PageMargins = { Left = 1, Right = 1 },

                    //    };
                    //byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                    //Stream stream = new MemoryStream(applicationPDFData);

                    //#region File Save
                    //Random rand = new Random();
                    //string filename = ConfigurationManager.AppSettings["File.CustomerFiles"];
                    //var saveComname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                    //var pdfTempFold = string.Format(filename, saveComname);
                    //var pdftempFolderName = cus.Id.ToString() + customSurvey.Id.ToString() + "CustomerFile";
                    //string cusFileName = cus.Id.ToString() + customSurvey.Id.ToString() + "-___" + "File.pdf";
                    //string cusfilePath = string.Concat("/", pdfTempFold, "/", pdftempFolderName, "/", cusFileName);
                    //string Serverfilename = FileHelper.GetFileFullPath(cusfilePath);
                    ////FileHelper.SaveFile(applicationPDFData, Serverfilename);
                    //#endregion
                    //#region file save to customer file
                    //if (result)
                    //{
                    //    CustomerFile cfs = new CustomerFile()
                    //    {
                    //        FileDescription = Regex.Replace(customSurvey.SurveyName, @"\s+", String.Empty) + ".pdf",
                    //        Filename = AppConfig.DomainSitePath + cusfilePath,
                    //        FileFullName = Regex.Replace(customSurvey.SurveyName, @"\s+", String.Empty) + ".pdf",
                    //        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                    //        CustomerId = cus.CustomerId,
                    //        CompanyId = CurrentUser.CompanyId.Value,
                    //        IsActive = true
                    //    };
                    //    _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                    //    string logMessage = Regex.Replace(customSurvey.SurveyName, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                    //    base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cus.Id, null);
                    //}
                    //#endregion
                    base.AddUserActivityForCustomer(customSurvey.SurveyName + " Sent Successfully", LabelHelper.ActivityAction.AddDocumentFileManagement, cus.CustomerId, cus.Id, null);
                }
                if (!string.IsNullOrWhiteSpace(SMS))
                {
                    if (SMS.IndexOf(";") > -1)
                    {
                        List<string> ReceiverNumberList = SMS.Split(";").ToList();
                        _Util.Facade.SMSFacade.SendSMS(CurrentUser.CompanyId.Value, CurrentUser.UserId, ShortUrl.Url, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName));
                        if (!string.IsNullOrWhiteSpace(Email))
                        {
                            message += " and sms";
                        }
                        else
                        {
                            message += "A survey sms";
                        }
                    }
                    else
                    {
                        List<string> ReceiverNumberList = new List<string>();
                        ReceiverNumberList.Add(SMS);
                        _Util.Facade.SMSFacade.SendSMS(CurrentUser.CompanyId.Value, CurrentUser.UserId, ShortUrl.Url, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName));
                        if (!string.IsNullOrWhiteSpace(Email))
                        {
                            message += " and sms";
                        }
                        else
                        {
                            message += "A survey sms";
                        }
                    }
                }
                message = " Sent successfully.";
            }
            return Json(new { result = result, message = message });
        }

        private string ScheduleConflictChecker(Ticket Ticket, Guid[] Assigned, Guid[] UserList)
        {
            #region Check
            string Result = "";
            //UserList Check
            List<Guid> AssignedList = new List<Guid>();
            if (Assigned != null)
            {
                AssignedList.AddRange(Assigned);
            }
            if (UserList != null)
            {
                AssignedList.AddRange(UserList);
            }
            if (AssignedList.Count() > 0)
            {
                List<TicketSchedule> TicketSchedules = _Util.Facade.TicketFacade.GetTicketSchedulesByUserListAndAppoinmentDate(AssignedList, Ticket.CompletionDate);

                int ticketStartTime = -1;
                int ticketEndTime = -1;

                int.TryParse(Ticket.AppointmentStartTime.Replace(":", ""), out ticketStartTime);
                int.TryParse(Ticket.AppointmentEndTime.Replace(":", ""), out ticketEndTime);

                if (TicketSchedules.Where(x => x.TicketId != Ticket.TicketId && (x.StartTime == "-1" || x.EndTime == "-1")).Count() > 0)
                {
                    return string.Format("One or more user has schedules on that day. You can't schedule for all day.");
                }
                foreach (var item in TicketSchedules.Where(x => x.TicketId != Ticket.TicketId))
                {
                    int startTime = -1;
                    int endTime = -1;

                    int.TryParse(item.StartTime.Replace(":", ""), out startTime);
                    int.TryParse(item.EndTime.Replace(":", ""), out endTime);

                    if (startTime == -1 || endTime == -1)
                    {
                        Result += string.Format("{0} has all day schedule. ", item.EmployeeName);
                    }
                    else if (startTime == ticketStartTime || (endTime > ticketStartTime && startTime < ticketStartTime))
                    {
                        Result += string.Format("{0} has scheduling conflict. Please check to make sure time is not overlapping. ", item.EmployeeName);
                    }
                    else if (endTime == ticketEndTime || (endTime > ticketEndTime && startTime < ticketEndTime))
                    {
                        Result += string.Format("{0} has scheduling conflict. Please check to make sure time is not overlapping. ", item.EmployeeName);
                    }
                }
            }
            return Result;

            #endregion
        }

        /*
        [Authorize]
        [HttpPost]
        public JsonResult AddTicketFile(TicketFile TicketFile)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //TicketFile TicketFile = new TicketFile()
            //{
            //    FileAddedBy = CurrentUser.UserId,
            //    FileAddedDate = DateTime.Now.UTCCurrentTime(),
            //    FileLocation = "",
            //    FileName ="",
            //    Filesize =0,
            //    TicketId =new Guid(),

            //};
            TicketFile.FileAddedBy = CurrentUser.UserId;
            TicketFile.FileAddedDate = DateTime.Now.UTCCurrentTime();
            _Util.Facade.TicketFacade.InsertTicketFile(TicketFile);

            return Json(true);
        }*/

        public ActionResult CustomerOverviewTicketList(int? customerid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<Ticket> ticketlist = new List<Ticket>();
            if (customerid.HasValue && customerid.Value > 0)
            {
                var objcus = _Util.Facade.CustomerFacade.GetCustomerById(customerid.Value);
                if (objcus != null)
                {
                    ticketlist = _Util.Facade.TicketFacade.GetCustomerOverviewInformation(CurrentUser.CompanyId.Value, objcus.CustomerId);
                }
            }
            return View(ticketlist);
        }
        #region Calculate Commission
        public void CalculatePayrollBrinks(Ticket ticket)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string Currency = HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency();
            if (ticket != null && ticket.TicketId != Guid.Empty && ticket.CustomerId != Guid.Empty)
            {
                var MonthlyMonitoringFee = _Util.Facade.TicketFacade.GetTicketServiceFeeTotal(ticket.TicketId);
                var payrollBrinksDetail = _Util.Facade.PayrollFacade.GetPayrollBrinksByTicketId(ticket.TicketId);
                var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(ticket.CustomerId);
                if (CustomerDetails != null)
                {
                    var SalesPayCalculation = _Util.Facade.TicketFacade.GetSalesPayCalculationByTicketId(ticket.TicketId);
                    if (SalesPayCalculation != null)
                    {
                        SalesPayCalculation.Deductions += SalesPayCalculation.PassThrus * SalesPayCalculation.TotalMultiple;
                        if (payrollBrinksDetail != null)
                        {
                            payrollBrinksDetail.SalesPersonId = CustomerDetails.Soldby1;
                            payrollBrinksDetail.MMR = MonthlyMonitoringFee;
                            payrollBrinksDetail.Multiple = SalesPayCalculation.TotalMultiple;
                            payrollBrinksDetail.GrossPay = MonthlyMonitoringFee * SalesPayCalculation.TotalMultiple;
                            payrollBrinksDetail.Deductions = SalesPayCalculation.Deductions;
                            payrollBrinksDetail.Adjustments = 0;
                            payrollBrinksDetail.NetPay = MonthlyMonitoringFee * SalesPayCalculation.TotalMultiple - SalesPayCalculation.Deductions;
                            payrollBrinksDetail.LastUpdateBy = CurrentUser.UserId;
                            payrollBrinksDetail.LastUpdateDate = DateTime.Now.UTCCurrentTime();
                            payrollBrinksDetail.PassThrus = SalesPayCalculation.PassThrus * SalesPayCalculation.TotalMultiple;
                            payrollBrinksDetail.HoldBack = SalesPayCalculation.HoldBack;
                            _Util.Facade.PayrollFacade.UpdatePayrollBrinks(payrollBrinksDetail);
                        }
                        else
                        {
                            PayrollBrinks model = new PayrollBrinks()
                            {
                                PayrollBrinksId = Guid.NewGuid(),
                                CustomerId = CustomerDetails.CustomerId,
                                SalesPersonId = CustomerDetails.Soldby1,
                                TicketId = ticket.TicketId,
                                MMR = MonthlyMonitoringFee,
                                Multiple = SalesPayCalculation.TotalMultiple,
                                GrossPay = MonthlyMonitoringFee * SalesPayCalculation.TotalMultiple,
                                Deductions = SalesPayCalculation.Deductions,
                                Adjustments = 0,
                                NetPay = MonthlyMonitoringFee * SalesPayCalculation.TotalMultiple - SalesPayCalculation.Deductions,
                                CreatedBy = CurrentUser.UserId,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                LastUpdateBy = CurrentUser.UserId,
                                LastUpdateDate = DateTime.Now.UTCCurrentTime(),
                                PassThrus = SalesPayCalculation.PassThrus * MonthlyMonitoringFee,
                                HoldBack = SalesPayCalculation.HoldBack,
                                FundingStatus = "Pending"
                            };
                            _Util.Facade.PayrollFacade.InsertPayrollBrinks(model);
                        }

                        #region Manager Payment
                        var DeleteManagerPayroll = _Util.Facade.PayrollFacade.DeleteManagerPayrollBrinksByTicketId(ticket.TicketId);
                        var managerList = _Util.Facade.PayrollFacade.GetPayrollTermSheetManagerList(SalesPayCalculation.TermSheetId);
                        if (managerList != null)
                        {
                            foreach (var manager in managerList)
                            {
                                if (manager.Type == "Multiple")
                                {
                                    PayrollBrinks model = new PayrollBrinks()
                                    {
                                        PayrollBrinksId = Guid.NewGuid(),
                                        CustomerId = CustomerDetails.CustomerId,
                                        SalesPersonId = manager.ManagerId,
                                        TicketId = ticket.TicketId,
                                        MMR = MonthlyMonitoringFee,
                                        Multiple = 0,
                                        GrossPay = MonthlyMonitoringFee,
                                        Deductions = 0,
                                        Adjustments = 0,
                                        NetPay = (MonthlyMonitoringFee - SalesPayCalculation.PassThrus) * manager.Value,
                                        CreatedBy = CurrentUser.UserId,
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        LastUpdateBy = CurrentUser.UserId,
                                        LastUpdateDate = DateTime.Now.UTCCurrentTime(),
                                        PassThrus = SalesPayCalculation.PassThrus,
                                        HoldBack = 0,
                                        FundingStatus = "Pending",
                                        IsManagerPayroll = true
                                    };
                                    _Util.Facade.PayrollFacade.InsertPayrollBrinks(model);
                                }
                                else
                                {
                                    PayrollBrinks model = new PayrollBrinks()
                                    {
                                        PayrollBrinksId = Guid.NewGuid(),
                                        CustomerId = CustomerDetails.CustomerId,
                                        SalesPersonId = manager.ManagerId,
                                        TicketId = ticket.TicketId,
                                        MMR = 0,
                                        Multiple = 0,
                                        GrossPay = 0,
                                        Deductions = 0,
                                        Adjustments = 0,
                                        NetPay = manager.Value,
                                        CreatedBy = CurrentUser.UserId,
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        LastUpdateBy = CurrentUser.UserId,
                                        LastUpdateDate = DateTime.Now.UTCCurrentTime(),
                                        PassThrus = 0,
                                        HoldBack = 0,
                                        FundingStatus = "Pending",
                                        IsManagerPayroll = true
                                    };
                                    _Util.Facade.PayrollFacade.InsertPayrollBrinks(model);
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
        }
        public void CalculateSalesCommission(Ticket ticket)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string Currency = HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency();
            var ExistUserCommission = "";
            if (ticket != null && ticket.TicketId != Guid.Empty && ticket.CustomerId != Guid.Empty)
            {
                #region Per Equipment Commission
                var PerEquipmentCommission = 0;
                var PerEquipmentCommissionSales = 0;
                GlobalSetting GlobalSettingDetails = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PerEquipmentCommission");
                if (GlobalSettingDetails != null)
                {
                    Int32.TryParse(GlobalSettingDetails.Value, out PerEquipmentCommission);
                }
                GlobalSetting GlobalSettingDetailsSales = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PerEquipmentCommissionSales");
                if (GlobalSettingDetailsSales != null)
                {
                    Int32.TryParse(GlobalSettingDetailsSales.Value, out PerEquipmentCommissionSales);
                }
                #endregion
                #region Moved Customer Commission
                var MovedCustomerCommission = 0.0;
                GlobalSetting GlobalSettingDetailsMoved = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "MovedCustomerCommission");
                if (GlobalSettingDetailsMoved != null)
                {
                    double.TryParse(GlobalSettingDetailsMoved.Value, out MovedCustomerCommission);
                }
                #endregion
                var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(ticket.CustomerId);
                if (CustomerDetails != null)
                {
                    if (CustomerDetails.MoveCustomerId != new Guid() && ticket.TicketTypeVal == "Install-Move")
                    {
                        //bool deleteAllSalesCommission = _Util.Facade.TicketFacade.DeleteSalesCommissionByTicketId(ticket.TicketId);
                        Guid SoldBy = new Guid();
                        Guid.TryParse(CustomerDetails.Soldby, out SoldBy);
                        var SalesMoveComissionExist = _Util.Facade.TicketFacade.GetSalesMoveCommissionByTicketIdUserId(ticket.TicketId, SoldBy);
                        if (SalesMoveComissionExist == null)
                        {
                            SalesCommission salesCom = new SalesCommission()
                            {
                                SalesCommissionId = Guid.NewGuid(),
                                TicketId = ticket.TicketId,
                                CustomerId = CustomerDetails.CustomerId,
                                UserId = SoldBy,
                                CompletionDate = ticket.CompletionDate,
                                RMRSold = 0,
                                RMRCommission = MovedCustomerCommission,
                                NoOfEquipment = 0,
                                EquipmentCommission = 0,
                                TotalCommission = MovedCustomerCommission,
                                IsPaid = false,
                                CreatedBy = CurrentUser.UserId,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                RMRCommissionCalculation = "Moved",
                                EquipmentCommissionCalculation = ""
                            };
                            try
                            {
                                _Util.Facade.TicketFacade.InsertSalesCommission(salesCom);
                            }
                            catch (Exception)
                            {

                            }
                        }
                        else
                        {
                            if (SalesMoveComissionExist.Adjustment != null && SalesMoveComissionExist.Adjustment != 0)
                            {
                                MovedCustomerCommission = MovedCustomerCommission + SalesMoveComissionExist.Adjustment.Value;
                            }
                            SalesMoveComissionExist.CompletionDate = ticket.CompletionDate;
                            SalesMoveComissionExist.RMRSold = 0;
                            SalesMoveComissionExist.RMRCommission = MovedCustomerCommission;
                            SalesMoveComissionExist.NoOfEquipment = 0;
                            SalesMoveComissionExist.EquipmentCommission = 0;
                            SalesMoveComissionExist.TotalCommission = MovedCustomerCommission;
                            SalesMoveComissionExist.OriginalPoint = 0;
                            SalesMoveComissionExist.TotalPoint = 0;
                            SalesMoveComissionExist.CreatedBy = CurrentUser.UserId;
                            SalesMoveComissionExist.CreatedDate = DateTime.Now.UTCCurrentTime();
                            SalesMoveComissionExist.RMRCommissionCalculation = "Moved";
                            SalesMoveComissionExist.EquipmentCommissionCalculation = "";
                            _Util.Facade.TicketFacade.UpdateSalesCommission(SalesMoveComissionExist);
                        }
                    }
                    else
                    {
                        var cusAptItem = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentIdForCommission(ticket.TicketId).Where(m => m.IsDefaultService != true && m.IsCopied != true).ToList();
                        var cusAptItemForPoint = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentIdForPoint(ticket.TicketId).Where(m => m.IsDefaultService != true && m.IsCopied != true).ToList();

                        if (cusAptItem != null)
                        {
                            var CreatedByList = cusAptItem.Select(m => m.CreatedByUid).Distinct();
                            foreach (var createdBy in CreatedByList)
                            {
                                ExistUserCommission += "'" + createdBy + "',";
                                var RmrSoldAgreement = 0.0;
                                var RmrSoldAdded = 0.0;
                                var RmrSold = 0.0;
                                var RmrSoldAgreementCommission = 0.0;
                                var RmrSoldAddedCommission = 0.0;
                                var RmrCommission = 0.0;
                                var NoOfEquipment = 0;
                                var EquipmentCommission = 0.0;
                                var TotalCommission = 0.0;
                                var OriginalPoint = 0.0;
                                var TotalPoint = 0.0;
                                var RMRCommissionCalculation = "";
                                var EquipmentCommissionCalculation = "";
                                if (createdBy != Guid.Empty)
                                {
                                    var SalesComissionExist = _Util.Facade.TicketFacade.GetSalesCommissionByTicketIdUserId(ticket.TicketId, createdBy);

                                    TotalPoint = cusAptItemForPoint.Where(m => m.CreatedByUid == createdBy && m.CreatedByUid != m.InstalledByUid).Sum(m => m.Point * m.Quantity);
                                    OriginalPoint = cusAptItemForPoint.Where(m => m.CreatedByUid == createdBy && m.CreatedByUid != m.InstalledByUid && m.IsEquipmentExist != true).Sum(m => m.Point * m.Quantity);
                                    var EmployeeDetails = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(createdBy);

                                    if (EmployeeDetails != null)
                                    {
                                        var EmployeePermissionGroup = _Util.Facade.EmployeeFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(EmployeeDetails.UserId, EmployeeDetails.CompanyId);
                                        if (SalesComissionExist != null && SalesComissionExist.IsSealed == true)
                                        {
                                            RmrSold = SalesComissionExist.RMRSold.HasValue ? SalesComissionExist.RMRSold.Value : 0;
                                            RmrCommission = SalesComissionExist.RMRCommission.HasValue ? SalesComissionExist.RMRCommission.Value : 0;
                                        }
                                        else
                                        {
                                            #region Calculate RMR
                                            if (!string.IsNullOrEmpty(EmployeeDetails.SalesCommissionStructure) && EmployeeDetails.SalesCommissionStructure == "-1")
                                            {
                                                EmployeeDetails.SalesCommissionStructure = "Employee";
                                            }
                                            var PackageCustomerDetails = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerId(CustomerDetails.CustomerId);
                                            if (PackageCustomerDetails != null)
                                            {
                                                var PackageRmrCommission = _Util.Facade.SmartPackageFacade.GetPackageCommissionByAll(LabelHelper.CommissionType.PackageRMR, PackageCustomerDetails.UserType, PackageCustomerDetails.PackageType, EmployeeDetails.SalesCommissionStructure);
                                                if (PackageRmrCommission != null)
                                                {
                                                    var cusAptAgreementService = cusAptItem.Where(m => m.CreatedByUid == createdBy && m.IsService == true && m.IsBaseItem == true).ToList();
                                                    if (cusAptAgreementService != null && PackageRmrCommission != null)
                                                    {
                                                        foreach (var iService in cusAptAgreementService)
                                                        {
                                                            if (iService.TotalPrice > 0 && iService.OriginalUnitPrice.HasValue && iService.OriginalUnitPrice > 0)
                                                            {
                                                                RmrSoldAgreement += iService.OriginalUnitPrice.Value * iService.Quantity;
                                                            }
                                                            else
                                                            {
                                                                RmrSoldAgreement += iService.TotalPrice;
                                                            }
                                                        }
                                                        RmrSoldAgreementCommission = RmrSoldAgreement * PackageRmrCommission.Commission;
                                                        if (EmployeePermissionGroup.Name == "Inside Sales" && EmployeeDetails.UserXComission > 0 && RmrSoldAgreementCommission > 0)
                                                        {
                                                            RmrSoldAgreementCommission = RmrSoldAgreementCommission * Convert.ToDouble(EmployeeDetails.UserXComission);
                                                            RMRCommissionCalculation += EmployeeDetails.UserXComission + "*";
                                                        }
                                                        if (RmrSoldAgreementCommission > 0)
                                                        {
                                                            RMRCommissionCalculation += Currency + LabelHelper.FormatAmount(RmrSoldAgreement) + "*" + Currency + LabelHelper.FormatAmount(PackageRmrCommission.Commission) + "</br>";
                                                        }
                                                    }
                                                }
                                            }
                                            var AddedRmrCommission = _Util.Facade.SmartPackageFacade.GetPackageCommissionByAll(LabelHelper.CommissionType.AddedRMR, "", "", EmployeeDetails.SalesCommissionStructure);
                                            if (AddedRmrCommission != null)
                                            {
                                                var cusAptAddedService = cusAptItem.Where(m => m.CreatedByUid == createdBy && m.IsService == true && m.IsBaseItem == false).ToList();
                                                if (cusAptAddedService != null && AddedRmrCommission != null)
                                                {
                                                    foreach (var iAddedService in cusAptAddedService)
                                                    {
                                                        if (iAddedService.OriginalUnitPrice.HasValue && iAddedService.OriginalUnitPrice > 0)
                                                        {
                                                            RmrSoldAdded += iAddedService.OriginalUnitPrice.Value * iAddedService.Quantity;
                                                        }
                                                        else
                                                        {
                                                            RmrSoldAdded += iAddedService.TotalPrice;
                                                        }
                                                    }
                                                    RmrSoldAddedCommission = RmrSoldAdded * AddedRmrCommission.Commission;
                                                    if (EmployeeDetails.UserXComission > 0 && RmrSoldAddedCommission > 0)
                                                    {
                                                        RmrSoldAddedCommission = RmrSoldAddedCommission * Convert.ToDouble(EmployeeDetails.UserXComission);
                                                        RMRCommissionCalculation += EmployeeDetails.UserXComission + "*";
                                                    }
                                                    if (RmrSoldAddedCommission > 0)
                                                    {
                                                        RMRCommissionCalculation += Currency + LabelHelper.FormatAmount(RmrSoldAdded) + "*" + Currency + LabelHelper.FormatAmount(AddedRmrCommission.Commission) + "</br>";
                                                    }
                                                }
                                            }
                                            RmrSold = RmrSoldAgreement + RmrSoldAdded;
                                            RmrCommission = RmrSoldAgreementCommission + RmrSoldAddedCommission;
                                            #endregion
                                        }

                                        //#region Calculate Equipment
                                        //var cusAptEquipment = cusAptItem.Where(m => m.CreatedByUid == createdBy && m.IsService == false && m.IsBaseItem == false).ToList();
                                        //foreach (var cusEqp in cusAptEquipment)
                                        //{
                                        //    var UnitPrice = cusEqp.TotalPrice / cusEqp.Quantity;
                                        //    var RepCost = cusEqp.RepCost;
                                        //    var TempPerEquipmentCommission = UnitPrice - RepCost;
                                        //    if (EmployeePermissionGroup.Name.Contains("Sales"))
                                        //    {
                                        //        if (TempPerEquipmentCommission > PerEquipmentCommissionSales)
                                        //        {
                                        //            TempPerEquipmentCommission = PerEquipmentCommissionSales;
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        if (TempPerEquipmentCommission > PerEquipmentCommission)
                                        //        {
                                        //            TempPerEquipmentCommission = PerEquipmentCommission;
                                        //        }
                                        //    }
                                        //    NoOfEquipment += cusEqp.Quantity;
                                        //    EquipmentCommission += TempPerEquipmentCommission * cusEqp.Quantity;
                                        //    EquipmentCommissionCalculation += "(" + Currency + LabelHelper.FormatAmount(TempPerEquipmentCommission) + "*" + cusEqp.Quantity + ")+";
                                        //}
                                        //if (EquipmentCommissionCalculation.Length > 0)
                                        //{
                                        //    EquipmentCommissionCalculation = EquipmentCommissionCalculation.Substring(0, EquipmentCommissionCalculation.Length - 1);
                                        //}
                                        //#endregion

                                        TotalCommission = RmrCommission + EquipmentCommission;
                                        #region Insert/update Sales Commission
                                        // for taylor request(DFW00002) shakil turn off this code on 5/7/2024
                                        //if (SalesComissionExist == null)
                                        //{
                                        //    if (TotalCommission != 0)
                                        //    {
                                        //        SalesCommission salesCom = new SalesCommission()
                                        //        {
                                        //            SalesCommissionId = Guid.NewGuid(),
                                        //            TicketId = ticket.TicketId,
                                        //            CustomerId = CustomerDetails.CustomerId,
                                        //            UserId = createdBy,
                                        //            CompletionDate = ticket.CompletionDate,
                                        //            RMRSold = RmrSold,
                                        //            RMRCommission = RmrCommission,
                                        //            NoOfEquipment = NoOfEquipment,
                                        //            EquipmentCommission = EquipmentCommission,
                                        //            TotalCommission = TotalCommission,
                                        //            OriginalPoint = OriginalPoint,
                                        //            TotalPoint = TotalPoint,
                                        //            IsPaid = false,
                                        //            CreatedBy = CurrentUser.UserId,
                                        //            CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        //            RMRCommissionCalculation = RMRCommissionCalculation,
                                        //            EquipmentCommissionCalculation = EquipmentCommissionCalculation
                                        //        };
                                        //        try
                                        //        {
                                        //            _Util.Facade.TicketFacade.InsertSalesCommission(salesCom);
                                        //        }
                                        //        catch (Exception)
                                        //        {

                                        //        }

                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    if (SalesComissionExist.Adjustment != null && SalesComissionExist.Adjustment != 0)
                                        //    {
                                        //        TotalCommission = TotalCommission + SalesComissionExist.Adjustment.Value;
                                        //    }
                                        //    if (SalesComissionExist.AdjustablePoint != null && SalesComissionExist.AdjustablePoint != 0)
                                        //    {
                                        //        OriginalPoint = OriginalPoint + SalesComissionExist.AdjustablePoint.Value;
                                        //        TotalPoint = TotalPoint + SalesComissionExist.AdjustablePoint.Value;
                                        //    }
                                        //    SalesComissionExist.CompletionDate = ticket.CompletionDate;
                                        //    SalesComissionExist.RMRSold = RmrSold;
                                        //    SalesComissionExist.RMRCommission = RmrCommission;
                                        //    SalesComissionExist.NoOfEquipment = NoOfEquipment;
                                        //    SalesComissionExist.EquipmentCommission = EquipmentCommission;
                                        //    SalesComissionExist.TotalCommission = TotalCommission;
                                        //    SalesComissionExist.OriginalPoint = OriginalPoint;
                                        //    SalesComissionExist.TotalPoint = TotalPoint;
                                        //    SalesComissionExist.CreatedBy = CurrentUser.UserId;
                                        //    SalesComissionExist.CreatedDate = DateTime.Now.UTCCurrentTime();
                                        //    SalesComissionExist.RMRCommissionCalculation = RMRCommissionCalculation;
                                        //    SalesComissionExist.EquipmentCommissionCalculation = EquipmentCommissionCalculation;
                                        //    _Util.Facade.TicketFacade.UpdateSalesCommission(SalesComissionExist);
                                        //}
                                        #endregion
                                    }
                                }
                            }
                            if (CreatedByList.Count() > 0)
                            {
                                ExistUserCommission = ExistUserCommission.Substring(0, (ExistUserCommission.Length - 1));
                                var SalesCommissionDelete = _Util.Facade.TicketFacade.DeleteExtraSalesCommission(ticket.TicketId, ExistUserCommission);
                            }
                        }
                    }
                }
            }
        }
        [HttpPost]
        public JsonResult UpdateSalesRmrCommission(int Id, double CommissionValue)
        {
            bool res = false;
            double totalcomis = 0;
            if (Id > 0 && CommissionValue > 0)
            {
                var objsalescom = _Util.Facade.TicketFacade.GetSalesComissionById(Id);
                if (objsalescom != null)
                {
                    totalcomis = CommissionValue;
                    if (objsalescom.EquipmentCommission.HasValue)
                    {
                        totalcomis += objsalescom.EquipmentCommission.Value;
                    }
                    if (objsalescom.Adjustment.HasValue)
                    {
                        totalcomis += objsalescom.Adjustment.Value;
                    }
                    objsalescom.IsSealed = true;
                    objsalescom.RMRCommissionCalculation = "";
                    objsalescom.TotalCommission = totalcomis;
                    objsalescom.RMRCommission = CommissionValue;
                    _Util.Facade.TicketFacade.UpdateSalesCommission(objsalescom);
                    res = true;
                }
            }
            return Json(new { res = res, CommissionValue = string.Format("{0:#,##0.00}", CommissionValue) });
        }
        public void CalculateTechCommission(Ticket ticket)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string Currency = HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency();
            if (ticket != null && ticket.TicketId != Guid.Empty && ticket.CustomerId != Guid.Empty)
            {
                var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(ticket.CustomerId);
                var cusAptItem = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentIdForCommission(ticket.TicketId).Where(m => m.IsDefaultService != true && m.IsCopied != true).ToList();
                var cusAptItemForPoint = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentIdForPoint(ticket.TicketId).Where(m => m.IsDefaultService != true && m.IsCopied != true).ToList();

                if (CustomerDetails != null && cusAptItem != null)
                {
                    var TicketAssigned = _Util.Facade.TicketFacade.GetTicketAssignedUserListByTicketId(ticket.TicketId).FirstOrDefault();
                    var InstalledByList = cusAptItem.Select(m => m.InstalledByUid).Distinct();
                    string UserIdString = "(";
                    foreach (var InstalledBy in InstalledByList)
                    {
                        if (InstalledBy != Guid.Empty)
                        {
                            var TechComissionExist = _Util.Facade.TicketFacade.GetTechCommissionByTicketIdUserId(ticket.TicketId, InstalledBy);

                            UserIdString += "'" + InstalledBy + "',";
                            var BaseRMR = 0.0;
                            var BaseRMRCommission = 0.0;
                            var AddedRMR = 0.0;
                            var AddedRMRCommission = 0.0;
                            var TotalCommission = 0.0;
                            var OriginalPoint = 0.0;
                            var TotalPoint = 0.0;
                            var BaseRMRCommissionCalculation = "";
                            var AddedRMRCommissionCalculation = "";
                            OriginalPoint = cusAptItemForPoint.Where(m => m.InstalledByUid == InstalledBy && m.IsEquipmentExist != true).Sum(m => m.Point * m.Quantity);
                            TotalPoint = cusAptItemForPoint.Where(m => m.InstalledByUid == InstalledBy).Sum(m => m.Point * m.Quantity);

                            if (TicketAssigned != null && TicketAssigned.UserId == InstalledBy)
                            {
                                if (TechComissionExist != null && TechComissionExist.IsSealed == true)
                                {
                                    BaseRMRCommission = TechComissionExist.BaseRMRCommission.HasValue ? TechComissionExist.BaseRMRCommission.Value : 0;
                                }
                                else
                                {
                                    #region Calculate Base RMR
                                    var PackageCustomerDetails = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerId(CustomerDetails.CustomerId);
                                    if (PackageCustomerDetails != null)
                                    {
                                        var TechCommission = _Util.Facade.SmartPackageFacade.GetPackageCommissionByAll(LabelHelper.CommissionType.TechCommission, PackageCustomerDetails.UserType, PackageCustomerDetails.PackageType, "");
                                        if (TechCommission != null)
                                        {
                                            var cusAptAgreementService = cusAptItem.Where(m => m.IsService == true && m.IsBaseItem == true).ToList();
                                            if (cusAptAgreementService != null && cusAptAgreementService.Count > 0 && TechCommission != null)
                                            {
                                                foreach (var iService in cusAptAgreementService)
                                                {
                                                    if (iService.OriginalUnitPrice.HasValue && iService.OriginalUnitPrice > 0)
                                                    {
                                                        BaseRMR += iService.OriginalUnitPrice.Value * iService.Quantity;
                                                    }
                                                    else
                                                    {
                                                        BaseRMR += iService.TotalPrice;
                                                    }
                                                }
                                                BaseRMRCommission = BaseRMR * TechCommission.Commission;
                                                if (BaseRMRCommission > 0)
                                                {
                                                    BaseRMRCommissionCalculation += Currency + LabelHelper.FormatAmount(BaseRMR) + "*" + Currency + LabelHelper.FormatAmount(TechCommission.Commission);
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                }

                                #region Calculate Added RMR
                                //var AddedRmrCommission = _Util.Facade.SmartPackageFacade.GetPackageCommissionByAll(LabelHelper.CommissionType.AddedRMRTech, "", "", "");
                                //if (AddedRmrCommission != null)
                                //{
                                //    var cusAptAddedService = cusAptItem.Where(m => m.IsService == true && m.IsBaseItem == false).ToList();
                                //    if (cusAptAddedService != null && cusAptAddedService.Count > 0 && AddedRmrCommission != null)
                                //    {
                                //        foreach (var iService in cusAptAddedService)
                                //        {
                                //            if (iService.OriginalUnitPrice.HasValue && iService.OriginalUnitPrice > 0)
                                //            {
                                //                AddedRMR += iService.OriginalUnitPrice.Value * iService.Quantity;
                                //            }
                                //            else
                                //            {
                                //                AddedRMR += iService.TotalPrice;
                                //            }
                                //        }
                                //        AddedRMRCommission = AddedRMR * AddedRmrCommission.Commission;
                                //        if (AddedRMRCommission > 0)
                                //        {
                                //            AddedRMRCommissionCalculation += Currency + LabelHelper.FormatAmount(AddedRMR) + "*" + Currency + LabelHelper.FormatAmount(AddedRmrCommission.Commission);
                                //        }
                                //    }
                                //}
                                #endregion
                            }
                            TotalCommission = BaseRMRCommission /*+ AddedRMRCommission*/;
                            var EmployeeDetails = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(InstalledBy);
                            #region Insert/update Tech Commission
                            if (TechComissionExist == null)
                            {
                                if (TotalCommission > 0 || OriginalPoint > 0)
                                {
                                    TechCommission techCom = new TechCommission()
                                    {
                                        TechCommissionId = Guid.NewGuid(),
                                        TicketId = ticket.TicketId,
                                        CustomerId = CustomerDetails.CustomerId,
                                        UserId = InstalledBy,
                                        CompletionDate = ticket.CompletionDate,
                                        BaseRMR = BaseRMR,
                                        BaseRMRCommission = BaseRMRCommission,
                                        AddedRMR = AddedRMR,
                                        AddedRMRCommission = AddedRMRCommission,
                                        TotalCommission = TotalCommission,
                                        OriginalPoint = OriginalPoint,
                                        TotalPoint = TotalPoint,
                                        IsPaid = false,
                                        CreatedBy = CurrentUser.UserId,
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        BaseRMRCommissionCalculation = BaseRMRCommissionCalculation,
                                        AddedRMRCommissionCalculation = AddedRMRCommissionCalculation
                                    };
                                    _Util.Facade.TicketFacade.InsertTechCommission(techCom);
                                }
                            }
                            else
                            {
                                if (TechComissionExist.Adjustment != null && TechComissionExist.Adjustment != 0)
                                {
                                    TotalCommission = TotalCommission + TechComissionExist.Adjustment.Value;
                                }
                                if (TechComissionExist.AdjustablePoint != null && TechComissionExist.AdjustablePoint != 0)
                                {
                                    OriginalPoint = OriginalPoint + TechComissionExist.AdjustablePoint.Value;
                                    TotalPoint = TotalPoint + TechComissionExist.AdjustablePoint.Value;
                                }
                                TechComissionExist.UserId = InstalledBy;
                                TechComissionExist.CompletionDate = ticket.CompletionDate;
                                TechComissionExist.BaseRMR = BaseRMR;
                                TechComissionExist.BaseRMRCommission = BaseRMRCommission;
                                TechComissionExist.AddedRMR = AddedRMR;
                                TechComissionExist.AddedRMRCommission = AddedRMRCommission;
                                TechComissionExist.TotalCommission = TotalCommission;
                                TechComissionExist.OriginalPoint = OriginalPoint;
                                TechComissionExist.TotalPoint = TotalPoint;
                                TechComissionExist.CreatedBy = CurrentUser.UserId;
                                TechComissionExist.CreatedDate = DateTime.Now.UTCCurrentTime();
                                TechComissionExist.BaseRMRCommissionCalculation = BaseRMRCommissionCalculation;
                                TechComissionExist.AddedRMRCommissionCalculation = AddedRMRCommissionCalculation;
                                _Util.Facade.TicketFacade.UpdateTechCommission(TechComissionExist);
                            }
                            #endregion
                        }
                    }
                    if (!string.IsNullOrEmpty(UserIdString) && UserIdString != "(")
                    {
                        try
                        {
                            #region Delete Extra Techfounder
                            UserIdString = UserIdString.Remove(UserIdString.Length - 1, 1);
                            UserIdString += ")";
                            var TechCommissionDelete = _Util.Facade.TicketFacade.DeleteExtraTechCommission(ticket.TicketId, UserIdString);
                            #endregion
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
        }
        [HttpPost]
        public JsonResult UpdateTechRmrCommission(int Id, double CommissionValue)
        {
            bool res = false;
            double totalcomis = 0;
            if (Id > 0 && CommissionValue > 0)
            {
                var objtechcom = _Util.Facade.TicketFacade.GetTechCommissionById(Id);
                if (objtechcom != null)
                {
                    totalcomis = CommissionValue + objtechcom.AddedRMRCommission.Value;
                    objtechcom.IsSealed = true;
                    objtechcom.BaseRMRCommissionCalculation = "";
                    objtechcom.TotalCommission = totalcomis;
                    objtechcom.BaseRMRCommission = CommissionValue;
                    _Util.Facade.TicketFacade.UpdateTechCommission(objtechcom);
                    res = true;
                }
            }
            return Json(new { res = res, CommissionValue = string.Format("{0:#,##0.00}", CommissionValue) });
        }
        public void CalculateAddMemberCommission(Ticket ticket)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (ticket != null && ticket.TicketId != Guid.Empty && ticket.CustomerId != Guid.Empty)
            {
                var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(ticket.CustomerId);
                var cusAptItem = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentIdForCommission(ticket.TicketId).Where(m => m.IsDefaultService != true && m.IsCopied != true).ToList();

                GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AddMemberCommission");

                if (CustomerDetails != null && cusAptItem != null && cusAptItem.Count > 0)
                {
                    Guid CommissionUserId = Guid.Empty;
                    if (CurrentUser.UserTags.ToLower().IndexOf("admin") > -1)
                    {
                        CommissionUserId = Guid.Empty;
                    }
                    else
                    {
                        CommissionUserId = CurrentUser.UserId;
                    }
                    var ExistAddedMemberCommissionList = _Util.Facade.TicketFacade.GetAddMemberCommissionByTicketId(ticket.TicketId, CommissionUserId);
                    bool delete = _Util.Facade.TicketFacade.DeleteAddMemberCommissionByTicketId(ticket.TicketId);
                    #region assign member commission
                    //var TicketAssigned = _Util.Facade.TicketFacade.GetTicketAssignedUserListByTicketId(ticket.TicketId).FirstOrDefault();
                    //if (TicketAssigned != null && TicketAssigned.UserId != null)
                    //{
                    //    var TotalCommission = 0.0;
                    //    var Adjustment = 0.0;
                    //    var TotalCommissionCalculation = "";
                    //    if (glob != null)
                    //    {
                    //        TotalCommission = Convert.ToDouble(glob.Value);
                    //    }
                    //    var ServiceItemCount = cusAptItem.Where(m => m.IsService == true).Count();
                    //    if (ServiceItemCount == 0 && TotalCommission > 0)
                    //    {
                    //        #region Insert AddMember Commission
                    //        var ExistAddCommission = ExistAddedMemberCommissionList.Where(m => m.UserId == TicketAssigned.UserId).FirstOrDefault();
                    //        if (ExistAddCommission != null && ExistAddCommission.Adjustment.HasValue)
                    //        {
                    //            Adjustment = ExistAddCommission.Adjustment.Value;
                    //            TotalCommission += Adjustment;
                    //        }
                    //        AddMemberCommission addmemberCom = new AddMemberCommission()
                    //        {
                    //            AddMemberCommissionId = Guid.NewGuid(),
                    //            TicketId = ticket.TicketId,
                    //            CustomerId = CustomerDetails.CustomerId,
                    //            UserId = TicketAssigned.UserId,
                    //            CompletionDate = ticket.CompletionDate,
                    //            Commission = TotalCommission,
                    //            Adjustment = Adjustment,
                    //            IsPaid = false,
                    //            CreatedBy = CurrentUser.UserId,
                    //            CreatedDate = DateTime.Now.UTCCurrentTime(),
                    //            CommissionCalculation = TotalCommissionCalculation
                    //        };
                    //        _Util.Facade.TicketFacade.InsertAddMemberCommission(addmemberCom);
                    //        #endregion
                    //    }
                    //}
                    #endregion
                    #region additional member commission
                    List<TicketUser> additionalUser = _Util.Facade.TicketFacade.GetOnlyTicketAddtionalUsersByTicketId(ticket.TicketId);
                    if (additionalUser != null)
                    {
                        foreach (var user in additionalUser)
                        {
                            var TotalCommission = 0.0;
                            var Adjustment = 0.0;
                            var OriginalPoint = 0.0;
                            var AdjustablePoint = 0.0;
                            var TotalCommissionCalculation = "";
                            if (glob != null)
                            {
                                TotalCommission = Convert.ToDouble(glob.Value);
                            }
                            #region Insert AddMember Commission
                            var ExistAddCommission = ExistAddedMemberCommissionList.Where(m => m.UserId == user.UserId).FirstOrDefault();
                            if (ExistAddCommission != null && ExistAddCommission.Adjustment.HasValue)
                            {
                                Adjustment = ExistAddCommission.Adjustment.Value;
                                TotalCommission += Adjustment;
                            }
                            if (ExistAddCommission != null && ExistAddCommission.OriginalPoint.HasValue && ExistAddCommission.AdjustablePoint.HasValue)
                            {
                                OriginalPoint = ExistAddCommission.OriginalPoint.Value;
                                AdjustablePoint = ExistAddCommission.AdjustablePoint.Value;
                            }
                            AddMemberCommission addmemberCom = new AddMemberCommission()
                            {
                                AddMemberCommissionId = Guid.NewGuid(),
                                TicketId = ticket.TicketId,
                                CustomerId = CustomerDetails.CustomerId,
                                UserId = user.UserId,
                                CompletionDate = ticket.CompletionDate,
                                Commission = TotalCommission,
                                Adjustment = Adjustment,
                                OriginalPoint = OriginalPoint,
                                AdjustablePoint = AdjustablePoint,
                                IsPaid = false,
                                CreatedBy = CurrentUser.UserId,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CommissionCalculation = TotalCommissionCalculation
                            };
                            _Util.Facade.TicketFacade.InsertAddMemberCommission(addmemberCom);
                            #endregion
                        }
                    }
                    #endregion
                }
            }
        }
        public void CalculateFinRepCommission(Ticket ticket)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            double Commission = 0;
            double Adjustment = 0;
            string CommissionCalculation = "";
            //double Percentage = 0;
            if (ticket != null && ticket.TicketId != Guid.Empty && ticket.CustomerId != Guid.Empty && ticket.IsAgreementTicket == true)
            {
                var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(ticket.CustomerId);
                var CustomerExtDetails = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(ticket.CustomerId);
                //GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "FinRepCommission");
                //if (glob != null)
                //{
                //    double.TryParse(glob.Value, out Percentage);
                //}

                //var cusAptItem = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentIdForCommission(ticket.TicketId).Where(m => m.IsDefaultService != true && m.IsCopied != true && m.IsService != true).ToList();
                //if (cusAptItem != null)
                //{
                //    var totalSupplierCost = cusAptItem.Sum(m => m.SupplierCost);
                //    Commission = totalSupplierCost * Percentage / 100;
                //}

                if (CustomerDetails != null && CustomerExtDetails.FinanceRep != Guid.Empty)
                {
                    var ExistFinRepCommission = _Util.Facade.TicketFacade.GetFinRepCommissionByTicketId(ticket.TicketId);
                    if (ExistFinRepCommission == null)
                    {
                        FinRepCommission finRepCom = new FinRepCommission()
                        {
                            FinRepCommissionId = Guid.NewGuid(),
                            TicketId = ticket.TicketId,
                            CustomerId = CustomerDetails.CustomerId,
                            UserId = CustomerExtDetails.FinanceRep,
                            CompletionDate = ticket.CompletionDate,
                            Commission = Commission,
                            Adjustment = Adjustment,
                            OriginalPoint = 0,
                            AdjustablePoint = 0,
                            IsPaid = false,
                            CreatedBy = CurrentUser.UserId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            CommissionCalculation = CommissionCalculation
                        };
                        _Util.Facade.TicketFacade.InsertFinRepCommission(finRepCom);
                    }
                    //else
                    //{
                    //    ExistFinRepCommission.UserId = CustomerExtDetails.FinanceRep;
                    //    ExistFinRepCommission.CompletionDate = ticket.CompletionDate;
                    //    ExistFinRepCommission.Commission = Commission + ExistFinRepCommission.Adjustment;
                    //    ExistFinRepCommission.Adjustment = ExistFinRepCommission.Adjustment;
                    //    ExistFinRepCommission.CommissionCalculation = CommissionCalculation;
                    //    _Util.Facade.TicketFacade.UpdateFinRepCommission(ExistFinRepCommission);
                    //}
                }
            }
        }
        public void CalculateServiceCallCommission(Ticket ticket)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (ticket != null && ticket.TicketId != Guid.Empty && ticket.CustomerId != Guid.Empty)
            {
                var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(ticket.CustomerId);
                //var cusDefaultServiceItem = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentIdForCommission(ticket.TicketId).Where(m => m.IsDefaultService == true).ToList();
                GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ServiceCallCommission");

                Guid CommissionUserId = Guid.Empty;
                if (CurrentUser.UserTags.ToLower().IndexOf("admin") > -1)
                {
                    CommissionUserId = Guid.Empty;
                }
                else
                {
                    CommissionUserId = CurrentUser.UserId;
                }
                var ExistServiceCallCommissionList = _Util.Facade.TicketFacade.GetServiceCallCommissionByTicketId(ticket.TicketId, CommissionUserId);
                bool delete = _Util.Facade.TicketFacade.DeleteServiceCallCommissionByTicketId(ticket.TicketId);

                if (CustomerDetails != null && ticket.TicketType == "Service")
                {
                    #region assign member commission
                    var TicketAssigned = _Util.Facade.TicketFacade.GetTicketAssignedUserListByTicketId(ticket.TicketId).FirstOrDefault();
                    if (TicketAssigned != null && TicketAssigned.UserId != null)
                    {
                        var TotalCommission = 0.0;
                        var Adjustment = 0.0;
                        var TotalCommissionCalculation = "";
                        if (glob != null)
                        {
                            TotalCommission = Convert.ToDouble(glob.Value);
                        }
                        if (TotalCommission > 0)
                        {
                            #region Insert AddMember Commission
                            var ExistServiceCallCommission = ExistServiceCallCommissionList.Where(m => m.UserId == TicketAssigned.UserId).FirstOrDefault();
                            if (ExistServiceCallCommission != null && ExistServiceCallCommission.Adjustment.HasValue)
                            {
                                Adjustment = ExistServiceCallCommission.Adjustment.Value;
                                TotalCommission += Adjustment;
                            }
                            ServiceCallCommission serviceCallmemberCom = new ServiceCallCommission()
                            {
                                ServiceCallCommissionId = Guid.NewGuid(),
                                TicketId = ticket.TicketId,
                                CustomerId = CustomerDetails.CustomerId,
                                UserId = TicketAssigned.UserId,
                                CompletionDate = ticket.CompletionDate,
                                Commission = TotalCommission,
                                Adjustment = Adjustment,
                                IsPaid = false,
                                CreatedBy = CurrentUser.UserId,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CommissionCalculation = TotalCommissionCalculation
                            };
                            _Util.Facade.TicketFacade.InsertServiceCallCommission(serviceCallmemberCom);
                            #endregion
                        }
                    }
                    #endregion
                }
            }
        }

        public void CalculateFollowUpCommission(Ticket ticket)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (ticket != null && ticket.TicketId != Guid.Empty && ticket.CustomerId != Guid.Empty)
            {
                var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(ticket.CustomerId);
                GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "FollowUpCommission");

                if (CustomerDetails != null)
                {
                    Guid CommissionUserId = Guid.Empty;
                    if (CurrentUser.UserTags.ToLower().IndexOf("admin") > -1)
                    {
                        CommissionUserId = Guid.Empty;
                    }
                    else
                    {
                        CommissionUserId = CurrentUser.UserId;
                    }
                    var ExistFollowUpCommissionList = _Util.Facade.TicketFacade.GetFollowUpCommissionByTicketId(ticket.TicketId, CommissionUserId);
                    bool delete = _Util.Facade.TicketFacade.DeleteFollowUpCommissionByTicketId(ticket.TicketId);
                    #region assign member commission
                    var TicketAssigned = _Util.Facade.TicketFacade.GetTicketAssignedUserListByTicketId(ticket.TicketId).FirstOrDefault();
                    if (TicketAssigned != null && TicketAssigned.UserId != null)
                    {
                        var TotalCommission = 0.0;
                        var Adjustment = 0.0;
                        var TotalCommissionCalculation = "";
                        if (glob != null)
                        {
                            TotalCommission = Convert.ToDouble(glob.Value);
                        }
                        if (TotalCommission > 0)
                        {
                            #region Insert FollowUp Commission
                            var ExistFollowUpCommission = ExistFollowUpCommissionList.Where(m => m.UserId == TicketAssigned.UserId).FirstOrDefault();
                            if (ExistFollowUpCommission != null && ExistFollowUpCommission.Adjustment.HasValue)
                            {
                                Adjustment = ExistFollowUpCommission.Adjustment.Value;
                                TotalCommission += Adjustment;
                            }
                            FollowUpCommission followUpCommission = new FollowUpCommission()
                            {
                                FollowUpCommissionId = Guid.NewGuid(),
                                TicketId = ticket.TicketId,
                                CustomerId = CustomerDetails.CustomerId,
                                UserId = TicketAssigned.UserId,
                                CompletionDate = ticket.CompletionDate,
                                Commission = TotalCommission,
                                Adjustment = Adjustment,
                                IsPaid = false,
                                CreatedBy = CurrentUser.UserId,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CommissionCalculation = TotalCommissionCalculation
                            };
                            _Util.Facade.TicketFacade.InsertFollowUpCommission(followUpCommission);
                            #endregion
                        }
                    }
                    #endregion
                    #region additional member commission
                    List<TicketUser> additionalUser = _Util.Facade.TicketFacade.GetOnlyTicketAddtionalUsersByTicketId(ticket.TicketId);
                    if (additionalUser != null)
                    {
                        foreach (var user in additionalUser)
                        {
                            var TotalCommission = 0.0;
                            var Adjustment = 0.0;
                            var TotalCommissionCalculation = "";
                            if (glob != null)
                            {
                                TotalCommission = Convert.ToDouble(glob.Value);
                            }
                            #region Insert FollowUp Commission
                            var ExistFollowUpCommission = ExistFollowUpCommissionList.Where(m => m.UserId == user.UserId).FirstOrDefault();
                            if (ExistFollowUpCommission != null && ExistFollowUpCommission.Adjustment.HasValue)
                            {
                                Adjustment = ExistFollowUpCommission.Adjustment.Value;
                                TotalCommission += Adjustment;
                            }
                            FollowUpCommission followUpCommission = new FollowUpCommission()
                            {
                                FollowUpCommissionId = Guid.NewGuid(),
                                TicketId = ticket.TicketId,
                                CustomerId = CustomerDetails.CustomerId,
                                UserId = user.UserId,
                                CompletionDate = ticket.CompletionDate,
                                Commission = TotalCommission,
                                Adjustment = Adjustment,
                                IsPaid = false,
                                CreatedBy = CurrentUser.UserId,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CommissionCalculation = TotalCommissionCalculation
                            };
                            _Util.Facade.TicketFacade.InsertFollowUpCommission(followUpCommission);
                            #endregion
                        }
                    }
                    #endregion
                }
            }
        }

        public void CalculateRescheduleCommission(Ticket ticket)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (ticket != null && ticket.TicketId != Guid.Empty && ticket.CustomerId != Guid.Empty)
            {
                var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(ticket.CustomerId);
                GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "RescheduleCommission");

                if (CustomerDetails != null)
                {
                    string Reason = string.Format("Reschedule Ticket. Ref <span class='cus-anchor' onclick='OpenTicketById({0})'>#{0}</span>", ticket.Id);
                    //bool delete = _Util.Facade.TicketFacade.DeleteRescheduleCommissionByTicketId(ticket.TicketId);
                    #region assign member commission
                    var TicketAssigned = _Util.Facade.TicketFacade.GetTicketAssignedUserListByTicketId(ticket.TicketId).Where(m => m.IsReschedulePay == true).FirstOrDefault();
                    if (TicketAssigned != null && TicketAssigned.UserId != null)
                    {
                        var TotalCommission = 0.0;
                        var TotalCommissionCalculation = "";
                        if (glob != null)
                        {
                            TotalCommission = Convert.ToDouble(glob.Value);
                        }
                        if (TotalCommission > 0)
                        {
                            //#region Insert Reschedule Commission
                            //RescheduleCommission rescheduleCommission = new RescheduleCommission()
                            //{
                            //    RescheduleCommissionId = Guid.NewGuid(),
                            //    TicketId = ticket.TicketId,
                            //    CustomerId = CustomerDetails.CustomerId,
                            //    UserId = TicketAssigned.UserId,
                            //    CompletionDate = ticket.CompletionDate,
                            //    Commission = TotalCommission,
                            //    IsPaid = false,
                            //    CreatedBy = CurrentUser.UserId,
                            //    CreatedDate = DateTime.Now.UTCCurrentTime(),
                            //    CommissionCalculation = TotalCommissionCalculation
                            //};
                            //_Util.Facade.TicketFacade.InsertRescheduleCommission(rescheduleCommission);
                            //#endregion
                            #region Insert Reschedule Adjustment
                            AdjustmentFunding adjustmentFunding = new AdjustmentFunding()
                            {
                                AdjustmentId = Guid.NewGuid(),
                                UserId = TicketAssigned.UserId,
                                Reason = Reason,
                                Amount = TotalCommission,
                                Date = DateTime.Now.UTCCurrentTime(),
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CreatedBy = CurrentUser.UserId
                            };
                            _Util.Facade.TicketFacade.InsertAddjustmentFunding(adjustmentFunding);
                            #endregion
                        }
                    }
                    #endregion
                    #region additional member commission
                    List<TicketUser> additionalUser = _Util.Facade.TicketFacade.GetOnlyTicketAddtionalUsersByTicketId(ticket.TicketId).Where(m => m.IsReschedulePay == true).ToList();
                    if (additionalUser != null)
                    {
                        foreach (var user in additionalUser)
                        {
                            var TotalCommission = 0.0;
                            var TotalCommissionCalculation = "";
                            if (glob != null)
                            {
                                TotalCommission = Convert.ToDouble(glob.Value);
                            }
                            //#region Insert Reschedule Commission
                            //RescheduleCommission rescheduleCommission = new RescheduleCommission()
                            //{
                            //    RescheduleCommissionId = Guid.NewGuid(),
                            //    TicketId = ticket.TicketId,
                            //    CustomerId = CustomerDetails.CustomerId,
                            //    UserId = user.UserId,
                            //    CompletionDate = ticket.CompletionDate,
                            //    Commission = TotalCommission,
                            //    IsPaid = false,
                            //    CreatedBy = CurrentUser.UserId,
                            //    CreatedDate = DateTime.Now.UTCCurrentTime(),
                            //    CommissionCalculation = TotalCommissionCalculation
                            //};
                            //_Util.Facade.TicketFacade.InsertRescheduleCommission(rescheduleCommission);
                            //#endregion
                            #region Insert Reschedule Adjustment
                            AdjustmentFunding adjustmentFunding = new AdjustmentFunding()
                            {
                                AdjustmentId = Guid.NewGuid(),
                                UserId = user.UserId,
                                Reason = Reason,
                                Amount = TotalCommission,
                                Date = DateTime.Now.UTCCurrentTime(),
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CreatedBy = CurrentUser.UserId
                            };
                            _Util.Facade.TicketFacade.InsertAddjustmentFunding(adjustmentFunding);
                            #endregion
                        }
                    }
                    #endregion
                }
            }
        }
        #endregion

        public ActionResult TechnesianTicketPartial(string status, string key)
        {
            ViewBag.Status = "";
            if (!string.IsNullOrEmpty(status))
            {
                ViewBag.Status = status;
            }
            if (!string.IsNullOrEmpty(key))
            {
                ViewBag.key = key;
            }

            return View();
        }
        public ActionResult LoadTechnesianTicketList(TicketFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (filter.PageNo == 0)
            {
                filter.PageNo = 1;
            }
            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ActivityListPageSize");
            if (glob != null)
            {
                filter.PageSize = Convert.ToInt32(glob.Value);
            }
            else
            {
                filter.PageSize = 30;
            }

            if (filter.PageNo == null || filter.PageNo == 0)
            {
                filter.PageNo = 1;
            }
            TicketListModel tickets = new TicketListModel();
            filter.CustomerId = CurrentUser.UserId;
            filter.CompanyId = CurrentUser.CompanyId.Value;
            tickets = _Util.Facade.TicketFacade.GetTechTicketListByCustomerIdAndFilter(filter);
            if (filter.StartDate != new DateTime() && filter.EndDate != new DateTime())
            {
                tickets.StartDate = filter.StartDate;
                tickets.EndDate = filter.EndDate;
            }
            if (filter.TicketStatus != null)
            {
                tickets.TicketStatus = filter.TicketStatus;
            }



            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = tickets.TotalCount;
            ViewBag.order = filter.order;
            if ((int)ViewBag.OutOfNumber == 0)
            {
                ViewBag.Message = "No Content Available !";
            }


            if (ViewBag.OutOfNumber == 0)
            {
                filter.PageNo = 1;
            }
            ViewBag.PageNumber = filter.PageNo;

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }

            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);



            return View(tickets);
        }
        #region ticket booking items
        [HttpPost]
        public JsonResult DeleteBookingItems(int id, int ticketid)
        {
            bool res = false;
            if (id > 0)
            {

                res = _Util.Facade.BookingFacade.DeleteTicketBookingDetailsById(id);

            }

            return Json(new { res = res });
        }
        [HttpPost]
        public JsonResult DeleteBookingExtraItems(int id, int ticketid)
        {
            bool res = false;
            if (id > 0)
            {

                res = _Util.Facade.BookingFacade.DeleteTicketBookingExtraItemById(id);

            }

            return Json(new { res = res });
        }
        #endregion

        #region Commission Adjustment & Delete
        [HttpPost]
        public JsonResult DeleteCommission(string Type, int CommissionId)
        {
            bool res = false;
            if (CommissionId > 0)
            {
                if (Type == "SalesCommission")
                {
                    res = _Util.Facade.TicketFacade.DeleteSalesCommissionById(CommissionId);
                }
                else if (Type == "TechCommission")
                {
                    res = _Util.Facade.TicketFacade.DeleteTechCommissionById(CommissionId);
                }
                else if (Type == "AddMemberCommission")
                {
                    res = _Util.Facade.TicketFacade.DeleteAddMemberCommissionById(CommissionId);
                }
                else if (Type == "FinRepCommission")
                {
                    res = _Util.Facade.TicketFacade.DeleteFinRepCommissionById(CommissionId);
                }
                else if (Type == "RescheduleCommission")
                {
                    res = _Util.Facade.TicketFacade.DeleteRescheduleCommissionById(CommissionId);
                }
                else if (Type == "FollowUpCommission")
                {
                    res = _Util.Facade.TicketFacade.DeleteFollowUpCommissionById(CommissionId);
                }
                else if (Type == "ServiceCallCommission")
                {
                    res = _Util.Facade.TicketFacade.DeleteServiceCallCommissionById(CommissionId);
                }
            }
            return Json(new { res = res });
        }
        [HttpPost]
        public JsonResult AdjustmentSalesComission(Guid? comissionid, string val)
        {
            bool res = false;
            double totalcomis = 0;
            if (comissionid.HasValue && comissionid.Value != new Guid())
            {
                var objsalescom = _Util.Facade.TicketFacade.GetSalesComissionByComissionId(comissionid.Value);
                if (objsalescom != null && !string.IsNullOrWhiteSpace(val))
                {
                    var spval = val.Split('-');
                    if (spval.Length > 1)
                    {
                        totalcomis = (objsalescom.RMRCommission.Value + objsalescom.EquipmentCommission.Value) - Convert.ToDouble(spval[1]);
                        objsalescom.TotalCommission = totalcomis;
                        objsalescom.Adjustment = Convert.ToDouble(val);
                        _Util.Facade.TicketFacade.UpdateSalesCommission(objsalescom);
                        res = true;
                    }
                    else
                    {
                        totalcomis = objsalescom.RMRCommission.Value + objsalescom.EquipmentCommission.Value + Convert.ToDouble(val);
                        objsalescom.TotalCommission = totalcomis;
                        objsalescom.Adjustment = Convert.ToDouble(val);
                        _Util.Facade.TicketFacade.UpdateSalesCommission(objsalescom);
                        res = true;
                    }
                }
            }
            return Json(new { res = res, totalcomis = string.Format("{0:#,##0.00}", totalcomis) });
        }
        [HttpPost]
        public JsonResult AdjustmentPointSalesComission(Guid? comissionid, string val)
        {
            bool res = false;
            double totalpoint = 0;
            double withNonCoTotalPoint = 0;
            double mainOriginalPoint = 0.0;
            double mainDBTotalPoint = 0.0;
            double mainAdjustablePoint = 0.0;
            if (comissionid.HasValue && comissionid.Value != new Guid())
            {
                var objsalescom = _Util.Facade.TicketFacade.GetSalesComissionByComissionId(comissionid.Value);
                if (objsalescom != null && !string.IsNullOrWhiteSpace(val))
                {
                    var OriginalPoint = 0.0;
                    var DBTotalPoint = 0.0;
                    var AdjustablePoint = 0.0;
                    if (objsalescom.OriginalPoint.HasValue)
                    {
                        OriginalPoint = objsalescom.OriginalPoint.Value;
                        DBTotalPoint = objsalescom.TotalPoint.Value;
                    }
                    if (objsalescom.AdjustablePoint.HasValue)
                    {
                        AdjustablePoint = objsalescom.AdjustablePoint.Value;
                    }
                    var adjustablePointval = Convert.ToString(AdjustablePoint).Split('-');
                    if (adjustablePointval.Length > 1)
                    {
                        mainOriginalPoint = OriginalPoint + Convert.ToDouble(adjustablePointval[1]);
                        mainDBTotalPoint = DBTotalPoint + Convert.ToDouble(adjustablePointval[1]);
                    }
                    else
                    {
                        mainOriginalPoint = OriginalPoint - AdjustablePoint;
                        mainDBTotalPoint = DBTotalPoint - AdjustablePoint;
                    }

                    var spval = val.Split('-');
                    if (spval.Length > 1)
                    {
                        mainAdjustablePoint = AdjustablePoint - Convert.ToDouble(spval[1]);
                        totalpoint = mainOriginalPoint + mainAdjustablePoint;
                        withNonCoTotalPoint = mainDBTotalPoint + mainAdjustablePoint;
                        objsalescom.OriginalPoint = totalpoint;
                        objsalescom.TotalPoint = withNonCoTotalPoint;
                        objsalescom.AdjustablePoint = mainAdjustablePoint;
                        _Util.Facade.TicketFacade.UpdateSalesCommission(objsalescom);
                        res = true;
                    }
                    else
                    {
                        mainAdjustablePoint = AdjustablePoint + Convert.ToDouble(val);
                        totalpoint = mainOriginalPoint + mainAdjustablePoint;
                        withNonCoTotalPoint = mainDBTotalPoint + mainAdjustablePoint;
                        objsalescom.OriginalPoint = totalpoint;
                        objsalescom.TotalPoint = withNonCoTotalPoint;
                        objsalescom.AdjustablePoint = mainAdjustablePoint;
                        _Util.Facade.TicketFacade.UpdateSalesCommission(objsalescom);
                        res = true;
                    }
                }
            }
            return Json(new { res = res, totalpoint = string.Format("{0:#,##0.00}", totalpoint), mainadjustablepoint = string.Format("{0:#,##0.00}", mainAdjustablePoint) });
        }
        [HttpPost]
        public JsonResult AdjustmentTechComission(Guid? comissionid, string val)
        {
            bool res = false;
            double totaltechcomis = 0;
            if (comissionid.HasValue && comissionid.Value != new Guid())
            {
                var objsalescom = _Util.Facade.TicketFacade.GetTechComissionByComissionId(comissionid.Value);
                if (objsalescom != null && !string.IsNullOrWhiteSpace(val))
                {
                    var spval = val.Split('-');
                    if (spval.Length > 1)
                    {
                        totaltechcomis = (objsalescom.BaseRMRCommission.Value + objsalescom.AddedRMRCommission.Value) - Convert.ToDouble(spval[1]);
                        objsalescom.TotalCommission = totaltechcomis;
                        objsalescom.Adjustment = Convert.ToDouble(val);
                        _Util.Facade.TicketFacade.UpdateTechCommission(objsalescom);
                        res = true;
                    }
                    else
                    {
                        totaltechcomis = objsalescom.BaseRMRCommission.Value + objsalescom.AddedRMRCommission.Value + Convert.ToDouble(val);
                        objsalescom.TotalCommission = totaltechcomis;
                        objsalescom.Adjustment = Convert.ToDouble(val);
                        _Util.Facade.TicketFacade.UpdateTechCommission(objsalescom);
                        res = true;
                    }
                }
            }
            return Json(new { res = res, totaltechcomis = string.Format("{0:#,##0.00}", totaltechcomis) });
        }
        [HttpPost]
        public JsonResult AdjustmentPointTechComission(Guid? comissionid, string val)
        {
            bool res = false;
            double totalpoint = 0;
            double withNonCoTotalPoint = 0;
            double mainOriginalPoint = 0.0;
            double mainDBTotalPoint = 0.0;
            double mainAdjustablePoint = 0.0;
            if (comissionid.HasValue && comissionid.Value != new Guid())
            {
                var objsalescom = _Util.Facade.TicketFacade.GetTechComissionByComissionId(comissionid.Value);
                if (objsalescom != null && !string.IsNullOrWhiteSpace(val))
                {
                    var OriginalPoint = 0.0;
                    var DBTotalPoint = 0.0;
                    var AdjustablePoint = 0.0;
                    if (objsalescom.OriginalPoint.HasValue)
                    {
                        OriginalPoint = objsalescom.OriginalPoint.Value;
                        DBTotalPoint = objsalescom.TotalPoint.Value;
                    }
                    if (objsalescom.AdjustablePoint.HasValue)
                    {
                        AdjustablePoint = objsalescom.AdjustablePoint.Value;
                    }
                    var adjustablePointval = Convert.ToString(AdjustablePoint).Split('-');
                    if (adjustablePointval.Length > 1)
                    {
                        mainOriginalPoint = OriginalPoint + Convert.ToDouble(adjustablePointval[1]);
                        mainDBTotalPoint = DBTotalPoint + Convert.ToDouble(adjustablePointval[1]);
                    }
                    else
                    {
                        mainOriginalPoint = OriginalPoint - AdjustablePoint;
                        mainDBTotalPoint = DBTotalPoint - AdjustablePoint;
                    }

                    var spval = val.Split('-');
                    if (spval.Length > 1)
                    {
                        mainAdjustablePoint = AdjustablePoint - Convert.ToDouble(spval[1]);
                        totalpoint = mainOriginalPoint + mainAdjustablePoint;
                        withNonCoTotalPoint = mainDBTotalPoint + mainAdjustablePoint;
                        objsalescom.OriginalPoint = totalpoint;
                        objsalescom.TotalPoint = withNonCoTotalPoint;
                        objsalescom.AdjustablePoint = mainAdjustablePoint;
                        _Util.Facade.TicketFacade.UpdateTechCommission(objsalescom);
                        res = true;
                    }
                    else
                    {
                        mainAdjustablePoint = AdjustablePoint + Convert.ToDouble(val);
                        totalpoint = mainOriginalPoint + mainAdjustablePoint;
                        withNonCoTotalPoint = mainDBTotalPoint + mainAdjustablePoint;
                        objsalescom.OriginalPoint = totalpoint;
                        objsalescom.TotalPoint = withNonCoTotalPoint;
                        objsalescom.AdjustablePoint = mainAdjustablePoint;
                        _Util.Facade.TicketFacade.UpdateTechCommission(objsalescom);
                        res = true;
                    }
                }
            }
            return Json(new { res = res, totalpoint = string.Format("{0:#,##0.00}", totalpoint), mainadjustablepoint = string.Format("{0:#,##0.00}", mainAdjustablePoint) });
        }
        [HttpPost]
        public JsonResult AdjustmentadditionalComission(Guid? comissionid, string val)
        {
            bool res = false;
            double totaladdcomis = 0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (comissionid.HasValue && comissionid.Value != new Guid())
            {
                var objsalescom = _Util.Facade.TicketFacade.GetAdditionalComissionByComissionId(comissionid.Value);
                if (objsalescom != null && !string.IsNullOrWhiteSpace(val))
                {
                    var OriginalCommission = objsalescom.Commission.Value - (objsalescom.Adjustment.HasValue ? objsalescom.Adjustment.Value : 0);
                    var spval = val.Split('-');
                    if (spval.Length > 1)
                    {
                        totaladdcomis = OriginalCommission - Convert.ToDouble(spval[1]);
                        objsalescom.Commission = totaladdcomis;
                        objsalescom.Adjustment = Convert.ToDouble(val);
                        _Util.Facade.TicketFacade.UpdateAddMemberCommission(objsalescom);
                        res = true;
                    }
                    else
                    {
                        totaladdcomis = OriginalCommission + Convert.ToDouble(val);
                        objsalescom.Commission = totaladdcomis;
                        objsalescom.Adjustment = Convert.ToDouble(val);
                        _Util.Facade.TicketFacade.UpdateAddMemberCommission(objsalescom);
                        res = true;
                    }
                }
            }
            return Json(new { res = res, totaladdcomis = string.Format("{0:#,##0.00}", totaladdcomis) });
        }
        [HttpPost]
        public JsonResult AdjustmentFinRepCommission(Guid? comissionid, string val)
        {
            bool res = false;
            double totaladdcomis = 0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (comissionid.HasValue && comissionid.Value != new Guid())
            {
                var objsalescom = _Util.Facade.TicketFacade.GetFinRepComissionByComissionId(comissionid.Value);
                if (objsalescom != null && !string.IsNullOrWhiteSpace(val))
                {
                    var OriginalCommission = objsalescom.Commission.Value - (objsalescom.Adjustment.HasValue ? objsalescom.Adjustment.Value : 0);
                    var spval = val.Split('-');
                    if (spval.Length > 1)
                    {
                        totaladdcomis = OriginalCommission - Convert.ToDouble(spval[1]);
                        objsalescom.Commission = totaladdcomis;
                        objsalescom.Adjustment = Convert.ToDouble(val);
                        _Util.Facade.TicketFacade.UpdateFinRepCommission(objsalescom);
                        res = true;
                    }
                    else
                    {
                        totaladdcomis = OriginalCommission + Convert.ToDouble(val);
                        objsalescom.Commission = totaladdcomis;
                        objsalescom.Adjustment = Convert.ToDouble(val);
                        _Util.Facade.TicketFacade.UpdateFinRepCommission(objsalescom);
                        res = true;
                    }
                }
            }
            return Json(new { res = res, totaladdcomis = string.Format("{0:#,##0.00}", totaladdcomis) });
        }
        [HttpPost]
        public JsonResult AdjustmentPointAdditionalComission(Guid? comissionid, string val)
        {
            bool res = false;
            double totalpoint = 0;
            double mainOriginalPoint = 0;
            double mainAdjustablePoint = 0;
            if (comissionid.HasValue && comissionid.Value != new Guid())
            {
                var objsalescom = _Util.Facade.TicketFacade.GetAdditionalComissionByComissionId(comissionid.Value);
                if (objsalescom != null && !string.IsNullOrWhiteSpace(val))
                {
                    var OriginalPoint = 0.0;
                    var AdjustablePoint = 0.0;
                    if (objsalescom.OriginalPoint.HasValue)
                    {
                        OriginalPoint = objsalescom.OriginalPoint.Value;
                    }
                    if (objsalescom.AdjustablePoint.HasValue)
                    {
                        AdjustablePoint = objsalescom.AdjustablePoint.Value;
                    }
                    var adjustablePointval = Convert.ToString(AdjustablePoint).Split('-');
                    if (adjustablePointval.Length > 1)
                    {
                        mainOriginalPoint = OriginalPoint + Convert.ToDouble(adjustablePointval[1]);
                    }
                    else
                    {
                        mainOriginalPoint = OriginalPoint - AdjustablePoint;
                    }

                    var spval = val.Split('-');
                    if (spval.Length > 1)
                    {
                        mainAdjustablePoint = AdjustablePoint - Convert.ToDouble(spval[1]);
                        totalpoint = mainOriginalPoint + mainAdjustablePoint;
                        objsalescom.OriginalPoint = totalpoint;
                        objsalescom.AdjustablePoint = mainAdjustablePoint;
                        _Util.Facade.TicketFacade.UpdateAddMemberCommission(objsalescom);
                        res = true;
                    }
                    else
                    {
                        mainAdjustablePoint = AdjustablePoint + Convert.ToDouble(val);
                        totalpoint = mainOriginalPoint + mainAdjustablePoint;
                        objsalescom.OriginalPoint = totalpoint;
                        objsalescom.AdjustablePoint = mainAdjustablePoint;
                        _Util.Facade.TicketFacade.UpdateAddMemberCommission(objsalescom);
                        res = true;
                    }
                }
            }
            return Json(new { res = res, totalpoint = string.Format("{0:#,##0.00}", totalpoint), mainadjustablepoint = string.Format("{0:#,##0.00}", mainAdjustablePoint) });
        }
        [HttpPost]
        public JsonResult AdjustmentFollowupComission(Guid? comissionid, string val)
        {
            bool res = false;
            double totaladdcomis = 0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (comissionid.HasValue && comissionid.Value != new Guid())
            {
                var objsalescom = _Util.Facade.TicketFacade.GetFollowupComissionByComissionId(comissionid.Value);
                if (objsalescom != null && !string.IsNullOrWhiteSpace(val))
                {
                    var OriginalCommission = objsalescom.Commission.Value - (objsalescom.Adjustment.HasValue ? objsalescom.Adjustment.Value : 0);
                    var spval = val.Split('-');
                    if (spval.Length > 1)
                    {
                        totaladdcomis = OriginalCommission - Convert.ToDouble(spval[1]);
                        objsalescom.Commission = totaladdcomis;
                        objsalescom.Adjustment = Convert.ToDouble(val);
                        _Util.Facade.TicketFacade.UpdateFollowUpCommission(objsalescom);
                        res = true;
                    }
                    else
                    {
                        totaladdcomis = OriginalCommission + Convert.ToDouble(val);
                        objsalescom.Commission = totaladdcomis;
                        objsalescom.Adjustment = Convert.ToDouble(val);
                        _Util.Facade.TicketFacade.UpdateFollowUpCommission(objsalescom);
                        res = true;
                    }
                }
            }
            return Json(new { res = res, totaladdcomis = string.Format("{0:#,##0.00}", totaladdcomis) });
        }

        [HttpPost]
        public JsonResult AdjustmentReScheduleComission(Guid? comissionid, string val)
        {
            bool res = false;
            double totaladdcomis = 0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (comissionid.HasValue && comissionid.Value != new Guid())
            {
                var objsalescom = _Util.Facade.TicketFacade.GetReScheduleCommissionByComissionId(comissionid.Value);
                if (objsalescom != null && !string.IsNullOrWhiteSpace(val))
                {
                    var OriginalCommission = objsalescom.Commission.Value - (objsalescom.Adjustment.HasValue ? objsalescom.Adjustment.Value : 0);
                    var spval = val.Split('-');
                    if (spval.Length > 1)
                    {
                        totaladdcomis = OriginalCommission - Convert.ToDouble(spval[1]);
                        objsalescom.Commission = totaladdcomis;
                        objsalescom.Adjustment = Convert.ToDouble(val);
                        _Util.Facade.TicketFacade.UpdateRescheduleCommission(objsalescom);
                        res = true;
                    }
                    else
                    {
                        totaladdcomis = OriginalCommission + Convert.ToDouble(val);
                        objsalescom.Commission = totaladdcomis;
                        objsalescom.Adjustment = Convert.ToDouble(val);
                        _Util.Facade.TicketFacade.UpdateRescheduleCommission(objsalescom);
                        res = true;
                    }
                }
            }
            return Json(new { res = res, totaladdcomis = string.Format("{0:#,##0.00}", totaladdcomis) });
        }

        [HttpPost]
        public JsonResult AdjustmentServiceCallComission(Guid? comissionid, string val)
        {
            bool res = false;
            double totaladdcomis = 0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (comissionid.HasValue && comissionid.Value != new Guid())
            {
                var objsalescom = _Util.Facade.TicketFacade.GetServiceCallCommissionByComissionId(comissionid.Value);
                if (objsalescom != null && !string.IsNullOrWhiteSpace(val))
                {
                    var OriginalCommission = objsalescom.Commission.Value - (objsalescom.Adjustment.HasValue ? objsalescom.Adjustment.Value : 0);
                    var spval = val.Split('-');
                    if (spval.Length > 1)
                    {
                        totaladdcomis = OriginalCommission - Convert.ToDouble(spval[1]);
                        objsalescom.Commission = totaladdcomis;
                        objsalescom.Adjustment = Convert.ToDouble(val);
                        _Util.Facade.TicketFacade.UpdateServiceCallCommission(objsalescom);
                        res = true;
                    }
                    else
                    {
                        totaladdcomis = OriginalCommission + Convert.ToDouble(val);
                        objsalescom.Commission = totaladdcomis;
                        objsalescom.Adjustment = Convert.ToDouble(val);
                        _Util.Facade.TicketFacade.UpdateServiceCallCommission(objsalescom);
                        res = true;
                    }
                }
            }
            return Json(new { res = res, totaladdcomis = string.Format("{0:#,##0.00}", totaladdcomis) });
        }
        #endregion

        public ActionResult LoadTicketSignature(int? id)
        {
            Ticket model = new Ticket();
            if (id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.TicketFacade.GetTicketById(id.Value);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult LoadTicketSignatureImage(string data, int ticketid, string tickettype, string doctext, bool? SentEmail, string emailaddress, string doctext2)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool uploadImage = false;
            string Message = "";
            string filePath = "";
            try
            {
                string[] datasplit = data.Split(',');
                byte[] bytes = Convert.FromBase64String(datasplit[1]);
                Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = Image.FromStream(ms);
                    string tempFolder = ConfigurationManager.AppSettings["File.CustomerSignatureFile"];
                    var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                    var FtempFolderName = string.Format(tempFolder, comname) + ticketid + "Signature";
                    Random rand = new Random();
                    string FileName = rand.Next().ToString();
                    FileName += "-___" + "Signature.png";
                    string tempFolderPath = Server.MapPath("~/" + FtempFolderName);

                    if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                    {
                        image.Save(Path.Combine(tempFolderPath, FileName));
                        uploadImage = true;
                        Message = "Agreement saved successfully.";
                    }
                    filePath = string.Concat("/", FtempFolderName, "/", FileName);
                }
                var serverFile = Server.MapPath(filePath);

                if (!System.IO.File.Exists(serverFile))
                {
                    return Json(new { result = true, message = "File not exsists" });
                }
                Ticket Ticket = _Util.Facade.TicketFacade.GetTicketById(ticketid);
                if (Ticket != null)
                {
                    Ticket.Signature = filePath;
                    Ticket.TicketSignatureDate = DateTime.Now.UTCCurrentTime();
                    _Util.Facade.TicketFacade.UpdateTicket(Ticket);
                    SendTicketAgreementWithSignature(Ticket.Id, Ticket.TicketId);
                }
                RUGTicketAgreementModel RUGTicketAgreementModel = new RUGTicketAgreementModel();
                string rugfilepath = "";
                string rugfilename = "";
                string Serverfilename = "";
                Company com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
                Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Ticket.CustomerId);
                var objcomlogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
                var AgreementPdfCreatePermission = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "EnableTicketCompletedAgreementSignedPdfCreation");
                bool IsPdfCreated = false;
                if (AgreementPdfCreatePermission != null && bool.TryParse(AgreementPdfCreatePermission.Value, out IsPdfCreated)) { }
                if (!string.IsNullOrWhiteSpace(tickettype) && !string.IsNullOrWhiteSpace(doctext) && (tickettype == "Pick Up" || tickettype == "Drop Off"))
                {
                    RUGTicketAgreementModel.Text = doctext;
                    RUGTicketAgreementModel.Signature = Ticket.Signature;
                    RUGTicketAgreementModel.Text2 = doctext2;
                    string ComCity = "";
                    string ComState = "";
                    if (!string.IsNullOrWhiteSpace(com.City))
                    {
                        ComCity = com.City.UppercaseFirst() + ", ";
                    }
                    if (!string.IsNullOrWhiteSpace(com.State))
                    {
                        ComState = com.State + " ";
                    }

                    RUGTicketAgreementModel.CompanyInfo = "<img src=" + objcomlogo + " style='width:100px;height:100px;' /><br>" + com.Street + "<br>" + ComCity + ComState + com.ZipCode + "<br>" + com.EmailAdress;
                    RUGTicketAgreementModel.SubmitInfo = "<span>Date: " + DateTime.Now.ToString("MM/dd/yy") + "</span><br><span>Ticket ID: #" + Ticket.Id + "</span>";
                    RUGTicketAgreementModel.SignDate = DateTime.UtcNow.UTCToClientTime().ToString("MM/dd/yyyy");
                    RUGTicketAgreementModel.CustomerName = customer.FirstName + " " + customer.LastName;
                    if (string.IsNullOrWhiteSpace(RUGTicketAgreementModel.CustomerName))
                    {
                        RUGTicketAgreementModel.CustomerName = customer.BusinessName;
                    }

                    #region Save Agreement
                    string FileExtention = "_PickUpDocument.pdf";
                    if (tickettype == "Drop Off")
                    {
                        FileExtention = "_DropOffDocument.pdf";
                    }
                    ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("CreateRUGTicketAgreement", RUGTicketAgreementModel)
                    {
                        PageSize = Rotativa.Options.Size.A4,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 },

                    };
                    byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                    Random rand = new Random();
                    string filename = ConfigurationManager.AppSettings["File.Ticket"];
                    var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                    var pdftempFolderName = string.Format(filename, comname) + Ticket.Id + FileExtention; //"_PickUpDocument.pdf";
                    Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
                    FileHelper.SaveFile(applicationPDFData, Serverfilename);
                    rugfilepath = "/" + pdftempFolderName;
                    rugfilename = Ticket.Id + FileExtention;//"_PickUpDocument.pdf";
                    #endregion

                    #region CustomerFile
                    var objcusfile = _Util.Facade.CustomerFileFacade.getCustomerFileCompanyIdAndCustomerId(CurrentUser.CompanyId.Value, Ticket.CustomerId, rugfilename);
                    if (objcusfile != null)
                    {
                        _Util.Facade.CustomerFileFacade.DeleteCustomerFile(objcusfile.Id);
                    }
                    CustomerFile CustomerFile = new CustomerFile()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        FileId = Guid.NewGuid(),
                        CustomerId = Ticket.CustomerId,
                        Filename = rugfilepath,
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        IsActive = true,
                        FileDescription = rugfilename,
                        CreatedBy = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = CurrentUser.UserId,
                        UpdatedDate = DateTime.Now.UTCCurrentTime(),
                        WMStatus = LabelHelper.WatermarkStatus.Pending,
                        AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                    };
                    _Util.Facade.CustomerFileFacade.InsertCustomerFile(CustomerFile);
                    #endregion

                    #region Send Email
                    bool EmailSent = false;
                    if (SentEmail.HasValue && SentEmail.Value == true)
                    {
                        if (!string.IsNullOrWhiteSpace(tickettype) && !string.IsNullOrWhiteSpace(doctext) && (tickettype == "Pick Up" || tickettype == "Drop Off"))
                        {
                            if (!string.IsNullOrWhiteSpace(emailaddress))
                            {
                                RUGTicketAgreementEmail RUGTicketAgreementEmail = new RUGTicketAgreementEmail();
                                RUGTicketAgreementEmail.Name = customer.FirstName + " " + customer.LastName;
                                RUGTicketAgreementEmail.ToEmail = emailaddress;
                                RUGTicketAgreementEmail.TicketAgreementPdf = new Attachment(Serverfilename, MediaTypeNames.Application.Octet);
                                RUGTicketAgreementEmail.Body = "Here is your agreement.";
                                EmailSent = _Util.Facade.MailFacade.SendRUGTicketAgreementEmail(RUGTicketAgreementEmail, CurrentUser.CompanyId.Value);
                            }
                        }
                    }
                    #endregion

                    #region AddLog
                    TicketReply tkrply = new TicketReply()
                    {
                        RepliedDate = DateTime.Now.UTCCurrentTime(),
                        TicketId = Ticket.TicketId,
                        UserId = CurrentUser.UserId,
                        Message = "<span>Customer signed on the agreement.</span>",
                        IsPrivate = false
                    };
                    if (EmailSent)
                    {
                        tkrply.Message += string.Format(" <span>Sent agreement file to: {0}</span> ", emailaddress);
                    }
                    tkrply.Id = _Util.Facade.TicketFacade.InsertTicketReply(tkrply);

                    #endregion

                    #region message text 
                    if (uploadImage)
                    {
                        Message = "Agreement saved successfully.";
                        if (EmailSent)
                        {
                            Message = "Agreement saved successfully and sent to: " + emailaddress;
                        }
                    }
                    else
                    {
                        Message = "Error Please Try again.";
                    }
                    #endregion

                }
                else if (IsPdfCreated)
                {
                    if (Ticket != null && !string.IsNullOrWhiteSpace(Ticket.TicketType))
                    {
                        RUGTicketAgreementModel.Text = doctext;
                        RUGTicketAgreementModel.Signature = Ticket.Signature;
                        RUGTicketAgreementModel.Text2 = doctext2;
                        string ComCity = "";
                        string ComState = "";
                        if (!string.IsNullOrWhiteSpace(com.City))
                        {
                            ComCity = com.City.UppercaseFirst() + ", ";
                        }
                        if (!string.IsNullOrWhiteSpace(com.State))
                        {
                            ComState = com.State + " ";
                        }

                        RUGTicketAgreementModel.CompanyInfo = "<img src=" + objcomlogo + " style='width:100px;height:100px;' /><br>" + com.Street + "<br>" + ComCity + ComState + com.ZipCode + "<br>" + com.EmailAdress;
                        RUGTicketAgreementModel.SubmitInfo = "<span>Date: " + DateTime.Now.ToString("MM/dd/yy") + "</span><br><span>Agreement for Ticket #" + Ticket.Id + " (" + Ticket.TicketType + ")</span>";
                        RUGTicketAgreementModel.SignDate = DateTime.UtcNow.UTCToClientTime().ToString("MM/dd/yyyy 'at' hh:mm tt");
                        RUGTicketAgreementModel.CustomerName = customer.FirstName + " " + customer.LastName;
                        if (string.IsNullOrWhiteSpace(RUGTicketAgreementModel.CustomerName))
                        {
                            RUGTicketAgreementModel.CustomerName = customer.BusinessName;
                        }
                        if (Ticket.TicketType == "Service")
                        {
                            RUGTicketAgreementModel.CustomerName = CurrentUser.GetFullName();
                            if (string.IsNullOrWhiteSpace(doctext)) { RUGTicketAgreementModel.Text = "Service of ticket number " + Ticket.Id + " has been completed."; }
                        }
                        #region Save Agreement
                        string FileExtention = "WorkAuthorization_Agreement_Signed_" + Ticket.Id + ".pdf";
                        ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("RUGTicketAgreementPdf", RUGTicketAgreementModel)
                        {
                            PageSize = Rotativa.Options.Size.A4,
                            PageOrientation = Rotativa.Options.Orientation.Portrait,
                            PageMargins = { Left = 1, Right = 1 },

                        };
                        byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                        Random rand = new Random();
                        string filename = ConfigurationManager.AppSettings["File.Ticket"];
                        var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                        var pdftempFolderName = string.Format(filename, comname) + FileExtention;
                        Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
                        FileHelper.SaveFile(applicationPDFData, Serverfilename);
                        rugfilepath = "/" + pdftempFolderName;
                        rugfilename = FileExtention;
                        #endregion

                        #region CustomerFile
                        var objcusfile = _Util.Facade.CustomerFileFacade.getCustomerFileCompanyIdAndCustomerId(CurrentUser.CompanyId.Value, Ticket.CustomerId, rugfilename);
                        if (objcusfile != null)
                        {
                            _Util.Facade.CustomerFileFacade.DeleteCustomerFile(objcusfile.Id);
                        }
                        CustomerFile CustomerFile = new CustomerFile()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            FileId = Guid.NewGuid(),
                            CustomerId = Ticket.CustomerId,
                            Filename = rugfilepath,
                            Uploadeddate = DateTime.Now.UTCCurrentTime(),
                            IsActive = true,
                            FileDescription = rugfilename,
                            CreatedBy = CurrentUser.UserId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            UpdatedBy = CurrentUser.UserId,
                            UpdatedDate = DateTime.Now.UTCCurrentTime(),
                            WMStatus = LabelHelper.WatermarkStatus.Pending,
                            AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                        };
                        _Util.Facade.CustomerFileFacade.InsertCustomerFile(CustomerFile);
                        #endregion
                        #region AddLog
                        TicketReply tkrply = new TicketReply()
                        {
                            RepliedDate = DateTime.Now.UTCCurrentTime(),
                            TicketId = Ticket.TicketId,
                            UserId = CurrentUser.UserId,
                            Message = "<span>Customer signed on the agreement.</span>",
                            IsPrivate = false
                        };
                        tkrply.Id = _Util.Facade.TicketFacade.InsertTicketReply(tkrply);

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Error Please Try again.";
                uploadImage = false;
            }
            return Json(new { result = uploadImage, message = Message });
        }

        public ActionResult CreateAddendumPdf(Guid CustomerId, Guid TicketId)
        {
            CustomerCompany custommerCompany = new CustomerCompany();
            Guid CompanyId = new Guid();
            var AgreementTax = 0.0;

            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            Ticket ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);
            List<CustomerAppointmentEquipment> AppoinmentEqpList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(TicketId);
            List<CustomerAppointmentEquipment> EqpmentList = new List<CustomerAppointmentEquipment>();
            List<CustomerAppointmentEquipment> ServiceList = new List<CustomerAppointmentEquipment>();

            if (AppoinmentEqpList != null && AppoinmentEqpList.Count() > 0)
            {
                EqpmentList = AppoinmentEqpList.Where(x => x.EquipmentClassId != 2 && x.IsEquipmentExist != true && x.IsEquipmentRelease != true).ToList();
                ServiceList = AppoinmentEqpList.Where(x => x.EquipmentClassId == 2 && x.IsARBEnabled == true).ToList();
            }
            Company com = new Company();
            CustomerAddendumModel cusAddendum = new CustomerAddendumModel();

            if (cus != null && ticket != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                    CompanyId = CurrentUser.CompanyId.Value;
                }
                else
                {
                    custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                    CompanyId = custommerCompany.CompanyId;
                }
                com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                var objcusaddendum = _Util.Facade.CustomerFacade.GetCustomerAddendumByCustomerIdAndTicketId(ticket.TicketId, cus.CustomerId);
                GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "CompanySignature");

                #region Tax
                //var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(cus.CustomerId, com.CompanyId);
                //if (GetCityTaxList.Count > 0)
                //{
                //    foreach (var item1 in GetCityTaxList)
                //    {
                //        AgreementTax = item1.Rate;
                //    }
                //}
                //else
                //{
                //    Guid CustomerId = new Guid();
                //    if (cus != null)
                //    {
                //        CustomerId = cus.CustomerId;
                //    }
                var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(com.CompanyId, cus.CustomerId);
                if (GetSalesTax != null)
                {
                    AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                }
                //}
                CustomerSignature _cusSign = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByCustomerId(cus.CustomerId);
                if (_cusSign != null)
                {
                    cus.CustomerSignatureDate = _cusSign.CreatedDate;
                }
                if (cus.CustomerSignatureDate == null)
                {
                    Invoice _inv = _Util.Facade.InvoiceFacade.GetInvoiceByCustomerIdAndStatus(cus.CustomerId, LabelHelper.EstimateStatus.Signed);
                    if (_inv != null)
                        cus.CustomerSignatureDate = _inv.SignatureDate.HasValue ? _inv.SignatureDate : new DateTime();
                }
                #endregion

                cusAddendum = new CustomerAddendumModel()
                {
                    CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(com.CompanyId),
                    KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG"),
                    KazarLogoIcon = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo_icon.png"),
                    CompanyPhone = com.Phone,
                    CompanyId = com.CompanyId,
                    CompanyAddress = com.Address,
                    CompanyCity = com.City,
                    CompanyState = com.City,
                    CompanyZip = com.ZipCode,
                    FirstName = cus.FirstName,
                    LastName = cus.LastName,
                    EmailAddress = cus.EmailAddress,
                    CellPhone = cus.CellNo,
                    SitePhone = cus.PrimaryPhone,
                    AgreementSignDate = cus.CustomerSignatureDate.HasValue ? cus.CustomerSignatureDate.Value : new DateTime(),
                    AddendumCreateDate = DateTime.Now,
                    CustomerStreet = cus.Street,
                    CustomerCity = cus.City,
                    Tax = AgreementTax,
                    CustomerState = cus.State,
                    CustomerZip = cus.ZipCode,
                    InstallAddress = AddressHelper.MakeAddress(cus),
                    SalesRepresentative = !string.IsNullOrEmpty(cus.Soldby) ? _Util.Facade.EmployeeFacade.GetEmployeeNumByEmployeeId(new Guid(cus.Soldby)).ToString() : "",
                    TicketId = ticket.Id,
                    WorkToBePerformed = ticket.WorkToBePerformed,
                    RecurringAmount = cus.MonthlyMonitoringFee,
                    ScheduleOn = ticket.CompletionDate,
                    CustomerId = cus.CustomerId,
                    TicketGuidId = ticket.TicketId,
                    CustomerAddendumSignature = objcusaddendum != null ? objcusaddendum.Signature : ""
                    //CustomerAddendumSignatureDate = objcusaddendum != null ? objcusaddendum.CreatedDate : DateTime.Now.UTCCurrentTime()
                };
                if (objcusaddendum != null && !string.IsNullOrWhiteSpace(objcusaddendum.Signature))
                {
                    if (objcusaddendum.CreatedDate != null && objcusaddendum.CreatedDate != new DateTime())
                    {
                        cusAddendum.CustomerAddendumStringSignatureDate = objcusaddendum.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    }
                    if (glbs != null && !string.IsNullOrWhiteSpace(glbs.Value))
                    {
                        cusAddendum.CompanySignature = glbs.Value;
                        if (objcusaddendum.CreatedDate != null && objcusaddendum.CreatedDate != new DateTime())
                        {
                            cusAddendum.CompanySignatureDate = objcusaddendum.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                }
                cusAddendum.ServiceEqpList = ServiceList;
                cusAddendum.EquipmentList = EqpmentList;
                cusAddendum.CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(com.CompanyId);
            }
            string body = _Util.Facade.TicketFacade.MakeCustomerAddendumPdf(cusAddendum);
            ViewBag.BodyContent = body;
            return new ViewAsPdf()
            {
                // FileName = flightPlan.ListingItemDetailsModel.FlightDetails + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Orientation.Portrait,
                //PageMargins = new Margins(10, 0, 0, 0),
                PageMargins = new Margins(10, 4, 10, 3)
            };
        }

        [HttpPost]
        public JsonResult SentEmailSMSAddendum_v2(Guid CustomerId, Guid TicketId, bool? SentEmail, bool? SentSms, string email, string phone)
        {
            bool result = false;
            string message = "";
            var AgreementTax = 0.0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            Ticket ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);
            List<CustomerAppointmentEquipment> AppoinmentEqpList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(TicketId);
            List<CustomerAppointmentEquipment> EqpmentList = new List<CustomerAppointmentEquipment>();
            List<CustomerAppointmentEquipment> ServiceList = new List<CustomerAppointmentEquipment>();
            string file = "";
            if (AppoinmentEqpList != null && AppoinmentEqpList.Count() > 0)
            {
                EqpmentList = AppoinmentEqpList.Where(x => x.EquipmentClassId != 2 && x.IsEquipmentExist != true && x.IsEquipmentRelease != true).ToList();
                ServiceList = AppoinmentEqpList.Where(x => x.EquipmentClassId == 2).ToList();
            }
            Company com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            CustomerAddendumModel cusAddendum = new CustomerAddendumModel();
            List<string> ValidPrefferedEmail = new List<string>();
            if (!string.IsNullOrWhiteSpace(email))
            {
                string[] Emailadd = email.Split(';');
                if (Emailadd != null)
                {
                    foreach (var item in Emailadd)
                    {
                        if (item.IsValidEmailAddress())
                        {
                            ValidPrefferedEmail.Add(item);
                        }
                    }
                }
                if (ValidPrefferedEmail.Count == 0)
                {
                    return Json(new { result = false, message = "Invalid email address." });
                }

            }
            if (cus != null && ticket != null)
            {
                var objcusaddendum = _Util.Facade.CustomerFacade.GetCustomerAddendumByCustomerIdAndTicketId(ticket.TicketId, cus.CustomerId);
                GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "CompanySignature");

                #region Tax
                //var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(cus.CustomerId, com.CompanyId);
                //if (GetCityTaxList.Count > 0)
                //{
                //    foreach (var item1 in GetCityTaxList)
                //    {
                //        AgreementTax = item1.Rate;
                //    }
                //}
                //else
                //{
                //    Guid CustomerId = new Guid();
                //    if (cus != null)
                //    {
                //        CustomerId = cus.CustomerId;
                //    }
                var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(com.CompanyId, cus.CustomerId);
                if (GetSalesTax != null)
                {
                    AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                }
                //}
                CustomerSignature _cusSign = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByCustomerId(cus.CustomerId);
                if (_cusSign != null)
                {
                    cus.CustomerSignatureDate = _cusSign.CreatedDate;
                }
                if (cus.CustomerSignatureDate == null)
                {
                    Invoice _inv = _Util.Facade.InvoiceFacade.GetInvoiceByCustomerIdAndStatus(cus.CustomerId, LabelHelper.EstimateStatus.Signed);
                    if (_inv != null)
                        cus.CustomerSignatureDate = _inv.SignatureDate.HasValue ? _inv.SignatureDate : new DateTime();
                }
                #endregion

                cusAddendum = new CustomerAddendumModel()
                {
                    CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(com.CompanyId),
                    KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG"),
                    KazarLogoIcon = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo_icon.png"),
                    CompanyPhone = com.Phone,
                    CompanyId = com.CompanyId,
                    CompanyAddress = com.Address,
                    CompanyCity = com.City,
                    CompanyState = com.City,
                    CompanyZip = com.ZipCode,
                    FirstName = cus.FirstName,
                    LastName = cus.LastName,
                    EmailAddress = cus.EmailAddress,
                    CellPhone = cus.CellNo,
                    SitePhone = cus.PrimaryPhone,
                    AgreementSignDate = cus.CustomerSignatureDate.HasValue ? cus.CustomerSignatureDate.Value : new DateTime(),
                    AddendumCreateDate = DateTime.Now,
                    CustomerStreet = cus.Street,
                    CustomerCity = cus.City,
                    CustomerState = cus.State,
                    CustomerZip = cus.ZipCode,
                    Tax = AgreementTax,
                    InstallAddress = AddressHelper.MakeAddress(cus),
                    TicketId = ticket.Id,
                    SalesRepresentative = !string.IsNullOrEmpty(cus.Soldby) ? _Util.Facade.EmployeeFacade.GetEmployeeNumByEmployeeId(new Guid(cus.Soldby)).ToString() : "",
                    WorkToBePerformed = ticket.WorkToBePerformed,
                    RecurringAmount = cus.MonthlyMonitoringFee,
                    ScheduleOn = ticket.CompletionDate,
                    CustomerId = cus.CustomerId,
                    TicketGuidId = ticket.TicketId,
                    CustomerAddendumSignature = objcusaddendum != null ? objcusaddendum.Signature : "",
                    //CustomerAddendumSignatureDate = objcusaddendum != null ? objcusaddendum.CreatedDate : DateTime.Now.UTCCurrentTime()
                };
                if (objcusaddendum != null && !string.IsNullOrWhiteSpace(objcusaddendum.Signature))
                {
                    if (objcusaddendum.CreatedDate != null && objcusaddendum.CreatedDate != new DateTime())
                    {
                        cusAddendum.CustomerAddendumStringSignatureDate = objcusaddendum.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    }
                    if (glbs != null && !string.IsNullOrWhiteSpace(glbs.Value))
                    {
                        cusAddendum.CompanySignature = glbs.Value;
                        if (objcusaddendum.CreatedDate != null && objcusaddendum.CreatedDate != new DateTime())
                        {
                            cusAddendum.CompanySignatureDate = objcusaddendum.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                }
                cusAddendum.ServiceEqpList = ServiceList;
                cusAddendum.EquipmentList = EqpmentList;
                cusAddendum.CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(com.CompanyId);
                string body = _Util.Facade.TicketFacade.MakeCustomerAddendumPdf(cusAddendum);
                ViewBag.BodyContent = body;
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("CreateAddendumPdf")
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                Random rand = new Random();
                string filename = ConfigurationManager.AppSettings["File.Ticket"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                var pdftempFolderName = string.Format(filename, comname) + ticket.Id + "_AddendumDocument.pdf";
                string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
                FileHelper.SaveFile(applicationPDFData, Serverfilename);
                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(ticket.Id + "#" + cus.Id + "#" + CurrentUser.CompanyId.Value.ToString());
                string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Ticket/", encryptedurl);

                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, cus.CustomerId);
                string shortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code);
                if (SentEmail.HasValue && SentEmail.Value)
                {
                    if (ValidPrefferedEmail.Count == 0)
                    {
                        email = cus.EmailAddress;
                    }
                    else
                    {
                        email = string.Join(";", ValidPrefferedEmail);
                    }
                    if (ValidPrefferedEmail.Count > 0 || (!string.IsNullOrWhiteSpace(cus.EmailAddress) && cus.EmailAddress.IsValidEmailAddress()))
                    {
                        CustomerTicketAddendumEmail CustomerTicketAddendumEmail = new CustomerTicketAddendumEmail
                        {
                            CustomerName = cus.FirstName + " " + cus.LastName,
                            ToEmail = email,
                            BodyLink = shortUrl
                        };
                        result = _Util.Facade.MailFacade.EmailOnlyCustomerTicketAddendumDocument(CustomerTicketAddendumEmail, CurrentUser.CompanyId.Value);
                        if (result)
                        {
                            message += "Addendum sent to " + email;

                            LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                CustomerId = cus.CustomerId,
                                Type = "Email",
                                ToEmail = email,
                                BodyContent = "Addendum Document",
                                SentDate = DateTime.Now.UTCCurrentTime(),
                                LastUpdatedDate = DateTime.Now,
                                SentBy = CurrentUser.UserId,
                                Subject = "Addendum Document"
                            };
                            _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(LeadCorrespondence);

                            #region file save to customer file
                            if (SentSms == null || (SentSms.HasValue && SentSms.Value == false))
                            {
                                file = "Customer_Addendum";
                                CustomerFile cfs = new CustomerFile()
                                {
                                    FileDescription = cus.Id + "_" + Regex.Replace(file, @"\s+", String.Empty) + "_Mail" + ".pdf",
                                    Filename = "/" + pdftempFolderName,
                                    FileId = Guid.NewGuid(),
                                    FileFullName = Regex.Replace(file, @"\s+", String.Empty) + ".pdf",
                                    Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                    CustomerId = cus.CustomerId,
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    IsActive = true,
                                    CreatedBy = CurrentUser.UserId,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    UpdatedBy = CurrentUser.UserId,
                                    UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                    WMStatus = LabelHelper.WatermarkStatus.Pending,
                                    AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                                };
                                _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                                string logMessage = Regex.Replace(file, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                                base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cus.Id, null);
                            }

                            #endregion
                        }

                    }
                    else
                    {
                        message = "Invalid email address.";
                    }
                }
                if (SentSms.HasValue && SentSms.Value)
                {
                    string ReceiverNumber = "";
                    List<string> ReceiverNumberList = new List<string>();
                    if (!string.IsNullOrWhiteSpace(phone))
                    {
                        ReceiverNumber = phone.Replace("-", "");
                    }
                    else if (!string.IsNullOrWhiteSpace(cus.PrimaryPhone))
                    {
                        ReceiverNumber = cus.PrimaryPhone.Replace("-", "");
                    }
                    else if (!string.IsNullOrWhiteSpace(cus.SecondaryPhone))
                    {
                        ReceiverNumber = cus.SecondaryPhone.Replace("-", "");
                    }
                    else
                    {
                        return Json(new { result = false, message = message + " and no phone number available." });
                    }
                    ReceiverNumberList.Add(ReceiverNumber);
                    SMSAddendum SMSAddendum = new SMSAddendum();

                    SMSAddendum.ShortUrl = shortUrl;
                    SMSAddendum.ToNumber = ReceiverNumberList;
                    string phonenumber = string.Join(";", ReceiverNumberList);
                    if (_Util.Facade.SMSFacade.SendAddendumSMS(SMSAddendum, CurrentUser.UserId, CurrentUser.CompanyId.Value, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName)) == true)
                    {
                        result = true;
                        LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            CustomerId = cus.CustomerId,
                            Type = "SMS",
                            ToMobileNo = phonenumber,
                            BodyContent = "Addendum Document",
                            SentDate = DateTime.Now.UTCCurrentTime(),
                            LastUpdatedDate = DateTime.Now,
                            SentBy = CurrentUser.UserId
                        };
                        _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(LeadCorrespondence);
                        #region file save to customer file
                        string pdfLastExt = "_SMS";
                        if (SentEmail.HasValue && SentEmail.Value)
                        {
                            pdfLastExt = "_Mail_SMS";
                        }
                        file = "Customer_Addendum";
                        CustomerFile cfs = new CustomerFile()
                        {
                            FileDescription = cus.Id + "_" + Regex.Replace(file, @"\s+", String.Empty) + pdfLastExt + ".pdf",
                            Filename = "/" + pdftempFolderName,
                            FileId = Guid.NewGuid(),
                            FileFullName = Regex.Replace(file, @"\s+", String.Empty) + ".pdf",
                            Uploadeddate = DateTime.Now.UTCCurrentTime(),
                            CustomerId = cus.CustomerId,
                            CompanyId = CurrentUser.CompanyId.Value,
                            IsActive = true,
                            CreatedBy = CurrentUser.UserId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            UpdatedBy = CurrentUser.UserId,
                            UpdatedDate = DateTime.Now.UTCCurrentTime(),
                            WMStatus = LabelHelper.WatermarkStatus.Pending,
                            AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                        };
                        _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                        string logMessage = Regex.Replace(file, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                        base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cus.Id, null);

                        #endregion
                        message += "Addendum Sent to" + phonenumber;
                    }
                    else
                    {
                        result = false;

                        message += "Addendum Sending failed to" + phonenumber;

                    }
                }
                cus.Singature = "";
                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
            }
            return Json(new { result = result, message = message });
        }


        public JsonResult SentEmailSMSAddendum(Guid CustomerId, Guid TicketId, bool? SentEmail, bool? SentSms, string email, string phone)
        {
            string FilePath = "";
            string FileKey = "";
            bool isUploaded = false;
            string tempFolderName = "";
            string FileName = "";
            var returnurl = "";
            byte[] data;
            decimal _fileSize = 1.00m;

            bool result = false;
            string message = "";
            var AgreementTax = 0.0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            Ticket ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);
            List<CustomerAppointmentEquipment> AppoinmentEqpList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(TicketId);
            List<CustomerAppointmentEquipment> EqpmentList = new List<CustomerAppointmentEquipment>();
            List<CustomerAppointmentEquipment> ServiceList = new List<CustomerAppointmentEquipment>();
            string file = "";
            if (AppoinmentEqpList != null && AppoinmentEqpList.Count() > 0)
            {
                EqpmentList = AppoinmentEqpList.Where(x => x.EquipmentClassId != 2 && x.IsEquipmentExist != true && x.IsEquipmentRelease != true).ToList();
                ServiceList = AppoinmentEqpList.Where(x => x.EquipmentClassId == 2).ToList();
            }
            Company com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            CustomerAddendumModel cusAddendum = new CustomerAddendumModel();
            List<string> ValidPrefferedEmail = new List<string>();
            if (!string.IsNullOrWhiteSpace(email))
            {
                string[] Emailadd = email.Split(';');
                if (Emailadd != null)
                {
                    foreach (var item in Emailadd)
                    {
                        if (item.IsValidEmailAddress())
                        {
                            ValidPrefferedEmail.Add(item);
                        }
                    }
                }
                if (ValidPrefferedEmail.Count == 0)
                {
                    return Json(new { result = false, message = "Invalid email address." });
                }

            }
            if (cus != null && ticket != null)
            {
                var objcusaddendum = _Util.Facade.CustomerFacade.GetCustomerAddendumByCustomerIdAndTicketId(ticket.TicketId, cus.CustomerId);
                GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "CompanySignature");

                #region Tax
                //var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(cus.CustomerId, com.CompanyId);
                //if (GetCityTaxList.Count > 0)
                //{
                //    foreach (var item1 in GetCityTaxList)
                //    {
                //        AgreementTax = item1.Rate;
                //    }
                //}
                //else
                //{
                //    Guid CustomerId = new Guid();
                //    if (cus != null)
                //    {
                //        CustomerId = cus.CustomerId;
                //    }
                var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(com.CompanyId, cus.CustomerId);
                if (GetSalesTax != null)
                {
                    AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                }
                //}
                CustomerSignature _cusSign = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByCustomerId(cus.CustomerId);
                if (_cusSign != null)
                {
                    cus.CustomerSignatureDate = _cusSign.CreatedDate;
                }
                if (cus.CustomerSignatureDate == null)
                {
                    Invoice _inv = _Util.Facade.InvoiceFacade.GetInvoiceByCustomerIdAndStatus(cus.CustomerId, LabelHelper.EstimateStatus.Signed);
                    if (_inv != null)
                        cus.CustomerSignatureDate = _inv.SignatureDate.HasValue ? _inv.SignatureDate : new DateTime();
                }
                #endregion

                cusAddendum = new CustomerAddendumModel()
                {
                    CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(com.CompanyId),
                    KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG"),
                    KazarLogoIcon = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo_icon.png"),
                    CompanyPhone = com.Phone,
                    CompanyId = com.CompanyId,
                    CompanyAddress = com.Address,
                    CompanyCity = com.City,
                    CompanyState = com.City,
                    CompanyZip = com.ZipCode,
                    FirstName = cus.FirstName,
                    LastName = cus.LastName,
                    EmailAddress = cus.EmailAddress,
                    CellPhone = cus.CellNo,
                    SitePhone = cus.PrimaryPhone,
                    AgreementSignDate = cus.CustomerSignatureDate.HasValue ? cus.CustomerSignatureDate.Value : new DateTime(),
                    AddendumCreateDate = DateTime.Now,
                    CustomerStreet = cus.Street,
                    CustomerCity = cus.City,
                    CustomerState = cus.State,
                    CustomerZip = cus.ZipCode,
                    Tax = AgreementTax,
                    InstallAddress = AddressHelper.MakeAddress(cus),
                    TicketId = ticket.Id,
                    SalesRepresentative = !string.IsNullOrEmpty(cus.Soldby) ? _Util.Facade.EmployeeFacade.GetEmployeeNumByEmployeeId(new Guid(cus.Soldby)).ToString() : "",
                    WorkToBePerformed = ticket.WorkToBePerformed,
                    RecurringAmount = cus.MonthlyMonitoringFee,
                    ScheduleOn = ticket.CompletionDate,
                    CustomerId = cus.CustomerId,
                    TicketGuidId = ticket.TicketId,
                    CustomerAddendumSignature = objcusaddendum != null ? objcusaddendum.Signature : "",
                    //CustomerAddendumSignatureDate = objcusaddendum != null ? objcusaddendum.CreatedDate : DateTime.Now.UTCCurrentTime()
                };
                if (objcusaddendum != null && !string.IsNullOrWhiteSpace(objcusaddendum.Signature))
                {
                    if (objcusaddendum.CreatedDate != null && objcusaddendum.CreatedDate != new DateTime())
                    {
                        cusAddendum.CustomerAddendumStringSignatureDate = objcusaddendum.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    }
                    if (glbs != null && !string.IsNullOrWhiteSpace(glbs.Value))
                    {
                        cusAddendum.CompanySignature = glbs.Value;
                        if (objcusaddendum.CreatedDate != null && objcusaddendum.CreatedDate != new DateTime())
                        {
                            cusAddendum.CompanySignatureDate = objcusaddendum.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                }
                cusAddendum.ServiceEqpList = ServiceList;
                cusAddendum.EquipmentList = EqpmentList;
                cusAddendum.CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(com.CompanyId);
                string body = _Util.Facade.TicketFacade.MakeCustomerAddendumPdf(cusAddendum);
                ViewBag.BodyContent = body;
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("CreateAddendumPdf")
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };

                #region File Save Old

                //byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                //Random rand = new Random();
                //string filename = ConfigurationManager.AppSettings["File.Ticket"];
                //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                //var pdftempFolderName = string.Format(filename, comname) + ticket.Id + "_AddendumDocument.pdf";
                //string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
                //FileHelper.SaveFile(applicationPDFData, Serverfilename);

                #endregion 

                #region File Save on AWS S3

                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                tempFolderName = ConfigurationManager.AppSettings["File.Ticket"];

                var pdfTempFold = string.Format(tempFolderName, comname);
                tempFolderName = string.Format(tempFolderName, comname);
                tempFolderName = tempFolderName.TrimEnd('/');

                FileName = ticket.Id + "_AddendumDocument.pdf";


                FilePath = tempFolderName;
                FileKey = string.Format($"{FilePath}/{FileName}");

                var task = Task.Run(async () =>
                {
                    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                    await AWSobject.UploadFile(FileKey, applicationPDFData);
                    await AWSobject.MakePublic(FileName, FilePath);
                });

                task.Wait();

                /// "mayur" used thread for async s3 methods : start

                //Thread thread = new Thread(async () => {

                //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                //    await AWSobject.UploadFile(FileKey, applicationPDFData);
                //    await AWSobject.MakePublic(FileName, FilePath);

                //});
                //thread.Start();
                //  Thread.Sleep(5000);

                /// "mayur" used thread for async s3 methods : End


                returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
                returnurl = returnurl + FileKey;



                isUploaded = true;

                ViewBag.ReturnUrl = returnurl;
                ViewBag.FileName = FileName;
                ViewBag.FileKey = FileKey;


                _fileSize = (decimal)applicationPDFData.Length / 1024;
                _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);


                #endregion




                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(ticket.Id + "#" + cus.Id + "#" + CurrentUser.CompanyId.Value.ToString());
                string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Ticket/", encryptedurl);

                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, cus.CustomerId);
                string shortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code);


                if (SentEmail.HasValue && SentEmail.Value)
                {
                    if (ValidPrefferedEmail.Count == 0)
                    {
                        email = cus.EmailAddress;
                    }
                    else
                    {
                        email = string.Join(";", ValidPrefferedEmail);
                    }
                    if (ValidPrefferedEmail.Count > 0 || (!string.IsNullOrWhiteSpace(cus.EmailAddress) && cus.EmailAddress.IsValidEmailAddress()))
                    {
                        CustomerTicketAddendumEmail CustomerTicketAddendumEmail = new CustomerTicketAddendumEmail
                        {
                            CustomerName = cus.FirstName + " " + cus.LastName,
                            ToEmail = email,
                            BodyLink = shortUrl
                        };
                        result = _Util.Facade.MailFacade.EmailOnlyCustomerTicketAddendumDocument(CustomerTicketAddendumEmail, CurrentUser.CompanyId.Value);
                        if (result)
                        {
                            message += "Addendum sent to " + email;

                            LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                CustomerId = cus.CustomerId,
                                Type = "Email",
                                ToEmail = email,
                                BodyContent = "Addendum Document",
                                SentDate = DateTime.Now.UTCCurrentTime(),
                                LastUpdatedDate = DateTime.Now,
                                SentBy = CurrentUser.UserId,
                                Subject = "Addendum Document"
                            };
                            _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(LeadCorrespondence);

                            #region file save to customer file
                            if (SentSms == null || (SentSms.HasValue && SentSms.Value == false))
                            {
                                file = "Customer_Addendum";
                                CustomerFile cfs = new CustomerFile()
                                {
                                    FileDescription = cus.Id + "_" + Regex.Replace(file, @"\s+", String.Empty) + "_Mail" + ".pdf",
                                    Filename = "/" + FileKey,
                                    FileId = Guid.NewGuid(),
                                    FileSize = (double)_fileSize,
                                    FileFullName = Regex.Replace(file, @"\s+", String.Empty) + ".pdf",
                                    Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                    CustomerId = cus.CustomerId,
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    IsActive = true,
                                    CreatedBy = CurrentUser.UserId,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    UpdatedBy = CurrentUser.UserId,
                                    UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                    WMStatus = LabelHelper.WatermarkStatus.Pending,
                                    AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                                };
                                _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                                string logMessage = Regex.Replace(file, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                                base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cus.Id, null);
                            }

                            #endregion
                        }

                    }
                    else
                    {
                        message = "Invalid email address.";
                    }
                }
                if (SentSms.HasValue && SentSms.Value)
                {
                    string ReceiverNumber = "";
                    List<string> ReceiverNumberList = new List<string>();
                    if (!string.IsNullOrWhiteSpace(phone))
                    {
                        ReceiverNumber = phone.Replace("-", "");
                    }
                    else if (!string.IsNullOrWhiteSpace(cus.PrimaryPhone))
                    {
                        ReceiverNumber = cus.PrimaryPhone.Replace("-", "");
                    }
                    else if (!string.IsNullOrWhiteSpace(cus.SecondaryPhone))
                    {
                        ReceiverNumber = cus.SecondaryPhone.Replace("-", "");
                    }
                    else
                    {
                        return Json(new { result = false, message = message + " and no phone number available." });
                    }
                    ReceiverNumberList.Add(ReceiverNumber);
                    SMSAddendum SMSAddendum = new SMSAddendum();

                    SMSAddendum.ShortUrl = shortUrl;
                    SMSAddendum.ToNumber = ReceiverNumberList;
                    string phonenumber = string.Join(";", ReceiverNumberList);
                    if (_Util.Facade.SMSFacade.SendAddendumSMS(SMSAddendum, CurrentUser.UserId, CurrentUser.CompanyId.Value, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName)) == true)
                    {
                        result = true;
                        LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            CustomerId = cus.CustomerId,
                            Type = "SMS",
                            ToMobileNo = phonenumber,
                            BodyContent = "Addendum Document",
                            SentDate = DateTime.Now.UTCCurrentTime(),
                            LastUpdatedDate = DateTime.Now,
                            SentBy = CurrentUser.UserId
                        };
                        _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(LeadCorrespondence);
                        #region file save to customer file
                        string pdfLastExt = "_SMS";
                        if (SentEmail.HasValue && SentEmail.Value)
                        {
                            pdfLastExt = "_Mail_SMS";
                        }
                        file = "Customer_Addendum";
                        CustomerFile cfs = new CustomerFile()
                        {
                            FileDescription = cus.Id + "_" + Regex.Replace(file, @"\s+", String.Empty) + pdfLastExt + ".pdf",
                            Filename = "/" + FileKey,
                            FileSize = (double)_fileSize,
                            FileId = Guid.NewGuid(),
                            FileFullName = Regex.Replace(file, @"\s+", String.Empty) + ".pdf",
                            Uploadeddate = DateTime.Now.UTCCurrentTime(),
                            CustomerId = cus.CustomerId,
                            CompanyId = CurrentUser.CompanyId.Value,
                            IsActive = true,
                            CreatedBy = CurrentUser.UserId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            UpdatedBy = CurrentUser.UserId,
                            UpdatedDate = DateTime.Now.UTCCurrentTime(),
                            WMStatus = LabelHelper.WatermarkStatus.Pending,
                            AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                        };
                        _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                        string logMessage = Regex.Replace(file, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                        base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cus.Id, null);

                        #endregion
                        message += "Addendum Sent to" + phonenumber;
                    }
                    else
                    {
                        result = false;

                        message += "Addendum Sending failed to" + phonenumber;

                    }
                }
                cus.Singature = "";
                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
            }
            return Json(new { result = result, message = message });
        }

        public ActionResult GetCustomerAddendumPopUp(Guid CustomerId, Guid TicketId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            ViewBag.CustoemrId = CustomerId;
            Customer model = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (model != null)
            {
                model.Singature = "";
            }
            ViewBag.TicketId = TicketId;
            return View(model);
        }

        public ActionResult GetWorkToBePerformedAddendumPopUp(Guid CustomerId, Guid TicketId)
        {
            ViewBag.CustoemrId = CustomerId;
            ViewBag.TicketId = TicketId;
            return View();
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddWorkToBePerformedInTicket(Guid TicketId, Guid CustomerId, string WorkToBePerformed)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            Ticket _tic = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);
            if (_tic != null)
            {
                _tic.WorkToBePerformed = WorkToBePerformed;
                _Util.Facade.TicketFacade.UpdateTicket(_tic);
                result = true;
            }
            return Json(new { result = result, ticketid = TicketId, customerid = CustomerId });
        }

        [HttpPost]
        public JsonResult EquipmentMoveBadInventory(EquipmentReturn eqreturn, Ticket Ticket)
        {
            bool result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var objeqpreturn = _Util.Facade.EquipmentFacade.GetEquipmentReturnByCustomerIdAndTechIdAndEquipmentId(eqreturn.CustomerId, eqreturn.TechnicianId, eqreturn.EquipmentId);
            var objappointmenteqp = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentEquipmentByAppointmentIdAndEquipmentId(Ticket.TicketId, eqreturn.EquipmentId);
            var objinventorytech = _Util.Facade.InventoryFacade.GetInventoryTechByTechnicianIdAndEquipmentId(eqreturn.TechnicianId, eqreturn.EquipmentId);
            EquipmentReturn objreturn = new EquipmentReturn()
            {
                CompanyId = CurrentLoggedInUser.CompanyId.Value,
                ReturnId = Guid.NewGuid(),
                CustomerId = eqreturn.CustomerId,
                TechnicianId = eqreturn.TechnicianId,
                EquipmentId = eqreturn.EquipmentId,
                Quantity = eqreturn.Quantity,
                PurchaseDate = DateTime.Now,
                WanrantyAvailable = false,
                Status = "Created",
                LastUpdatedBy = CurrentLoggedInUser.UserId,
                LastUpdatedDate = DateTime.Now,
                Description = eqreturn.Description,
                Reason = eqreturn.Reason
            };
            result = _Util.Facade.EquipmentFacade.InsertEquipmentReturn(objreturn) > 0;
            if (objinventorytech != null)
            {
                objinventorytech.Quantity = objinventorytech.Quantity - eqreturn.Quantity;
                _Util.Facade.InventoryFacade.UpdateInventoryTech(objinventorytech);
            }
            return Json(result);
        }

        public ActionResult LoadLogTicketReplyPartial(Guid? ticketid, string search)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.UserRole = CurrentUser.UserRole;
            CreateTicketModel Model = new CreateTicketModel();
            if (ticketid.HasValue && ticketid.Value != new Guid())
            {
                Model.Ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(ticketid.Value);
                Model.Customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Ticket.CustomerId);
                if (Model.Ticket != null && Model.Ticket.BookingId != null && Model.Ticket.TicketType != null)
                {
                    CustomerAddress CustomerAddressModel = new CustomerAddress();
                    if (Model.Ticket.TicketType == "Drop Off")
                    {
                        CustomerAddressModel = _Util.Facade.CustomerFacade.GetCustomerAddressByCustomerIdRefIdAddressType(Model.Ticket.CustomerId, Model.Ticket.BookingId, "DropOffLocation");
                        if (CustomerAddressModel != null)
                        {
                            Model.Customer.Street = CustomerAddressModel.Street;
                            Model.Customer.City = CustomerAddressModel.City;
                            Model.Customer.State = CustomerAddressModel.State;
                            Model.Customer.ZipCode = CustomerAddressModel.ZipCode;
                            Model.Ticket.CustomerAddressId = CustomerAddressModel.Id;
                        }
                    }
                    else if (Model.Ticket.TicketType == "Pick Up")
                    {
                        CustomerAddressModel = _Util.Facade.CustomerFacade.GetCustomerAddressByCustomerIdRefIdAddressType(Model.Ticket.CustomerId, Model.Ticket.BookingId, "PickUpLocation");
                        if (CustomerAddressModel != null)
                        {
                            Model.Customer.Street = CustomerAddressModel.Street;
                            Model.Customer.City = CustomerAddressModel.City;
                            Model.Customer.State = CustomerAddressModel.State;
                            Model.Customer.ZipCode = CustomerAddressModel.ZipCode;
                            Model.Ticket.CustomerAddressId = CustomerAddressModel.Id;
                        }
                    }

                }
                if (Model.Ticket != null)
                {
                    Model.TicketReplyList = _Util.Facade.TicketFacade.GetAllTicketReplyByTicketId(ticketid.Value, search);
                    if (Model.TicketReplyList != null && Model.TicketReplyList.Count > 0)
                    {
                        foreach (TicketReply item in Model.TicketReplyList)
                        {
                            if(!string.IsNullOrWhiteSpace(item.LatLng) && item.LatLng != "0,0")
                            {
                                string[] PrefferdLatLong = item.LatLng.Split(',');
                                if (PrefferdLatLong != null && PrefferdLatLong.Length > 0)
                                {
                                    item.Lat = PrefferdLatLong[0];
                                    item.Lng = PrefferdLatLong[1]; 
                                }
                            } 
                            item.RepliedDateVal = string.Format(HS.Framework.Utils.ConvertDatetimeToAgo.TimeAgo(item.RepliedDate).ToString("M/d/yy h:mm tt"));
                        } 
                    } 
                }
            }
            Model.SearchType = search;
            return View(Model);
        }

        public ActionResult LoadNoteTicketReplyPartial(Guid? ticketid, string search)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.UserRole = CurrentUser.UserRole;
            CreateTicketModel Model = new CreateTicketModel();
            if (ticketid.HasValue && ticketid.Value != new Guid())
            {
                Model.Ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(ticketid.Value);
                if (Model.Ticket != null)
                {
                    Model.TicketReplyList = _Util.Facade.TicketFacade.GetAllTicketReplyByTicketId(ticketid.Value, search);
                    foreach (TicketReply item in Model.TicketReplyList)
                    {

                        item.RepliedDateVal = string.Format(HS.Framework.Utils.ConvertDatetimeToAgo.TimeAgo(item.RepliedDate).ToString("M/d/yy h:mm tt"));
                    }
                }
            }
            Model.SearchType = search;
            return View(Model);
        }

        public ActionResult LoadTicketAdditionalMembers(int? ticketid, Guid assigned)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateTicketModel Model = new CreateTicketModel();
            if (ticketid.HasValue && ticketid.Value > 0)
            {
                Model.Ticket = _Util.Facade.TicketFacade.GetTicketById(ticketid.Value);
                if (Model.Ticket != null)
                {
                    List<TicketUser> UserList = _Util.Facade.TicketFacade.GetTicketUserListByTicketId(Model.Ticket.TicketId);
                    if (UserList.Count() > 0)
                    {
                        Model.TicketAssignedUserList = UserList.Where(x => x.IsPrimary == true && (!x.NotificationOnly.HasValue || !x.NotificationOnly.Value)).ToList();
                        Model.TicketUserList = UserList.Where(x => x.IsPrimary == false && (!x.NotificationOnly.HasValue || !x.NotificationOnly.Value)).ToList();
                        Model.NotifyingUserList = UserList.Where(x => x.IsPrimary == false && x.NotificationOnly.HasValue && x.NotificationOnly.Value).ToList();
                    }

                }
                if (assigned != new Guid())
                {
                    List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployeeByTicketAssigned(CurrentUser.CompanyId.Value, assigned).OrderBy(x => x.FirstName).ToList();

                    var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTagAndTechnician(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, assigned);

                    bool chkadditionalmembertech = new bool();
                    bool.TryParse(_Util.Facade.GlobalSettingsFacade.GetTicketAdditionalMemberOnlyTechnicianByCompanyId(CurrentUser.CompanyId.Value), out chkadditionalmembertech);

                    if (chkadditionalmembertech)
                    {
                        ViewBag.EmployeeList = TechnicianList.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.FirstName + " " + x.LastName,
                            Value = x.UserId.ToString(),
                            Selected = Model.TicketUserList.Count(y => y.UserId == x.UserId) > 0
                        }).OrderBy(x => x.Text != "Nothing selected").ThenBy(x => x.Text).ToList();
                    }
                    else
                    {
                        ViewBag.EmployeeList = EmpList.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.FirstName + " " + x.LastName,
                            Value = x.UserId.ToString(),
                            Selected = Model.TicketUserList.Count(y => y.UserId == x.UserId) > 0
                        }).OrderBy(x => x.Text != "Nothing selected").ThenBy(x => x.Text).ToList();
                    }
                }
                else
                {
                    List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value).OrderBy(x => x.FirstName).ToList();
                    var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTagAndTechnician(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid());
                    bool chkadditionalmembertech = Convert.ToBoolean(_Util.Facade.GlobalSettingsFacade.GetTicketAdditionalMemberOnlyTechnicianByCompanyId(CurrentUser.CompanyId.Value));
                    if (chkadditionalmembertech)
                    {
                        ViewBag.EmployeeList = TechnicianList.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.FirstName + " " + x.LastName,
                            Value = x.UserId.ToString()
                        }).OrderBy(x => x.Text != "Nothing selected").ThenBy(x => x.Text).ToList();
                    }
                    else
                    {
                        ViewBag.EmployeeList = EmpList.Select(x =>
                        new SelectListItem()
                        {
                            Text = x.FirstName + " " + x.LastName,
                            Value = x.UserId.ToString()
                        }).OrderBy(x => x.Text != "Nothing selected").ThenBy(x => x.Text).ToList();
                    }
                }
            }
            else
            {
                List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value).OrderBy(x => x.FirstName).ToList();
                var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTagAndTechnician(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid());
                bool chkadditionalmembertech = Convert.ToBoolean(_Util.Facade.GlobalSettingsFacade.GetTicketAdditionalMemberOnlyTechnicianByCompanyId(CurrentUser.CompanyId.Value));
                if (chkadditionalmembertech)
                {
                    ViewBag.EmployeeList = TechnicianList.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.FirstName + " " + x.LastName,
                        Value = x.UserId.ToString()
                    }).OrderBy(x => x.Text != "Nothing selected").ThenBy(x => x.Text).ToList();
                }
                else
                {
                    ViewBag.EmployeeList = EmpList.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.FirstName + " " + x.LastName,
                        Value = x.UserId.ToString()
                    }).OrderBy(x => x.Text != "Nothing selected").ThenBy(x => x.Text).ToList();
                }
            }
            return View(Model);
        }

        public JsonResult SaveCompletedDateTicket(int? ticketid)
        {
            bool result = false;
            string compDate = "";
            if (ticketid.HasValue && ticketid.Value > 0)
            {
                var objticket = _Util.Facade.TicketFacade.GetTicketById(ticketid.Value);
                if (objticket != null)
                {
                    objticket.CompletedDate = DateTime.UtcNow.UTCToClientTime().SetZeroHour();
                    _Util.Facade.TicketFacade.UpdateTicket(objticket);
                    compDate = objticket.CompletedDate.Value.ToString("MM/dd/yyyy");
                    result = true;
                }
            }
            return Json(new { result = result, compDate = compDate });
        }

        public ActionResult LoadReferenceTicketList(int? id)
        {
            CreateTicketModel Model = new CreateTicketModel();
            List<Ticket> RefTicketList = new List<Ticket>();
            if (id.HasValue && id.Value > 0)
            {
                Model.Ticket = _Util.Facade.TicketFacade.GetTicketById(id.Value);
                if (Model.Ticket != null && Model.Ticket.ReferenceTicketId > 0)
                {
                    var objticket = _Util.Facade.TicketFacade.GetTicketById(Model.Ticket.ReferenceTicketId.Value);
                    if (objticket != null)
                    {
                        RefTicketList.Add(objticket);
                    }
                    if (objticket != null && objticket.ReferenceTicketId > 0)
                    {
                        var objreftik = _Util.Facade.TicketFacade.GetTicketById(objticket.ReferenceTicketId.Value);
                        if (objreftik != null)
                        {
                            RefTicketList.Add(objreftik);
                        }
                    }
                }
                Model.RefTicketTable = RefTicketList;
            }
            return View(Model);
        }

        public ActionResult AddRescheduleTicket(RescheduleTicket reticket)
        {
            bool result = true;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            RescheduleTicket resticket = new Entities.RescheduleTicket();
            int newTicketId = 0;
            resticket.TicketId = reticket.TicketId;
            resticket.Reason = reticket.Reason;
            resticket.IsPay = reticket.IsPay;
            resticket.CreatedBy = CurrentLoggedInUser.UserId;
            resticket.CreatedDate = DateTime.Now;
            resticket.RescheduleId = Guid.NewGuid();
            try
            {
                TicketUser tikuser = new TicketUser();
                if (reticket.AdditionalMemberList != null && reticket.AdditionalMemberList.Count > 0)
                {
                    foreach (var item in reticket.AdditionalMemberList)
                    {
                        tikuser = _Util.Facade.TicketFacade.GetTicketMemberByTicketIdAndUserId(resticket.TicketId, item.UserId);
                        if (tikuser != null)
                        {
                            if (item.IsReschedulePay == true)
                            {
                                tikuser.IsReschedulePay = true;
                            }
                            else
                            {
                                tikuser.IsReschedulePay = false;
                            }
                            _Util.Facade.TicketFacade.UpdateTicketUser(tikuser);
                        }
                    }
                }
                _Util.Facade.TicketFacade.InsertRescheduleTicket(resticket);


                //var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
                //if (empobj != null)
                //{
                //    result = _Util.Facade.TicketFacade.CloneRescheduleTicketConfirmation(resticket.TicketId, empobj.FirstName + " " + empobj.LastName, empobj.UserId);
                //}

                DataSet dsNewTicketId = _Util.Facade.TicketFacade.CloneRescheduleTicketConfirmation(resticket.TicketId, CurrentLoggedInUser.FirstName + " " + CurrentLoggedInUser.LastName, CurrentLoggedInUser.UserId, reticket.CompletionDate);
                Ticket ogTicket = new Ticket();
                if (dsNewTicketId != null)
                {
                    ogTicket = _Util.Facade.TicketFacade.GetTicketByTicketId(resticket.TicketId);
                    if (ogTicket != null)
                    {
                        newTicketId = ogTicket.RescheduleTicketId ?? 0;
                        if (reticket.AdditionalMemberList != null && reticket.AdditionalMemberList.Count > 0)
                        {
                            CalculateRescheduleCommission(ogTicket);
                        }
                    }
                }

                //string Reason = string.Format("Reason: {0}", resticket.Reason);

                TicketReply TR = new TicketReply()
                {
                    Message = $"Rescheduled Ticket #{ogTicket.Id} from {ogTicket.CompletionDate} to {resticket.CompletionDate}, reason: {resticket.Reason}",
                    TicketId = reticket.TicketId,
                    RepliedDate = DateTime.Now,
                    IsPrivate = true,
                    UserId = CurrentLoggedInUser.UserId
                };
                TR.Id = _Util.Facade.TicketFacade.InsertTicketReply(TR);

                //if (result)
                //{
                //    if (objticket != null)
                //    {
                //        newTicketId = objticket.RescheduleTicketId.HasValue ? objticket.RescheduleTicketId.Value : 0;
                //        objticket.IsAgreementTicket = false;
                //        _Util.Facade.TicketFacade.UpdateTicket(objticket);
                //    }
                //}
                var objticket = _Util.Facade.TicketFacade.GetTicketByTicketId(resticket.TicketId);
                //if (objticket != null)
                //{
                //    newTicketId = objticket.Id;
                //    if (reticket.AdditionalMemberList != null && reticket.AdditionalMemberList.Count > 0)
                //    {
                //        CalculateRescheduleCommission(objticket);
                //    }
                //}
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(reticket));
            }

            return Json(new { result = result, newTicketId = newTicketId });
        }

        public ActionResult AddMemberAppointmentTime(AdditionalMembersAppointment model)
        {
            string result = "false";
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            List<AdditionalMembersAppointment> AppointmentList = _Util.Facade.TicketFacade.GetAllAdditionalMembersAppointmenByTicketIdAndEmpId(model.AppointmentId, model.EmployeeId);
            if (AppointmentList != null)
            {
                foreach (var item in AppointmentList)
                {
                    _Util.Facade.TicketFacade.DeleteAdditionalMemberAppointment(item.Id);
                }


            }
            try
            {
                model.LastUpdatedBy = CurrentLoggedInUser.UserId;
                model.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                model.LastUpdatedDate = DateTime.Now;
                model.CreatedBy = CurrentLoggedInUser.UserId;

                _Util.Facade.TicketFacade.InsertAdditionalAppointment(model);


                result = "true";

            }
            catch (Exception ex)
            {

            }

            return Json(new { result = result });
        }
        public ActionResult RescheduleTicketPupup(Guid TicketId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            else
            {

                Ticket model = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);
                model.AdditionalMemberList = _Util.Facade.TicketFacade.GetAllTicketAdditionalMembersByTicketId(TicketId);
                if (model == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                else
                {
                    return PartialView("_RescheduleTicketPopup", model);
                }

            }
        }


        public ActionResult MemberAppointmentPupup(AdditionalMemberAppointmentModel model)
        {

            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            AdditionalMembersAppointment appointmentModel = new AdditionalMembersAppointment();
            ViewBag.HasUser = "false";
            if (!string.IsNullOrEmpty(model.UserList) && model.UserList != "00000000-0000-0000-0000-000000000000" && model.UserList != "null")
            {
                string EmpIdList = "";
                string[] userIds = model.UserList.Split(',');
                foreach (var item in userIds)
                {
                    EmpIdList += string.Format("'{0}',", item);
                }

                EmpIdList = EmpIdList.Substring(0, EmpIdList.Length - 1);
                ViewBag.EmpList = _Util.Facade.EmployeeFacade.GetAllEmployeeByEmpIdList(EmpIdList).OrderBy(x => x.UserId.ToString() != "-1").ThenBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Select(x => new SelectListItem()
                {
                    Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                    Value = x.UserId.ToString()
                }).ToList();
                ViewBag.HasUser = "true";
                appointmentModel.AppointmentId = model.TicketId;
                appointmentModel.AppointmentDate = model.AppointmentDate;
                appointmentModel.CustomerId = model.CustomerId;
            }
            #region Schedule times
            var arrivaltime = _Util.Facade.LookupFacade.GetLookupByKey("TicketScheduleTime").ToList().Where(x => x.IsActive == true).Select(x =>
                    new SelectListItem()
                    {
                        Text = x.DisplayText.ToString(),
                        Value = x.DataValue.ToString()
                    }).ToList();

            ViewBag.AppointmentTime = arrivaltime;
            #endregion
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            else
            {
                return View(appointmentModel);
            }
        }
        [HttpPost]
        public JsonResult DeleteCustomerTicket(int? id, Guid? CustomerId, string TicketId)
        {
            bool result = false;
            List<Ticket> model = new List<Ticket>();
            if (id.HasValue && id.Value > 0)
            {
                var objticket = _Util.Facade.TicketFacade.GetTicketById(id.Value);
                if (objticket != null)
                {
                    var objreferenceticketList = _Util.Facade.TicketFacade.GetTicketListByReferenceTicketId(objticket.Id);
                    if (objreferenceticketList != null && objreferenceticketList.Count > 0)
                    {
                        foreach (var item in objreferenceticketList)
                        {
                            model.Add(item);
                            var objrefticketlist = _Util.Facade.TicketFacade.GetTicketListByReferenceTicketId(item.Id);
                            if (objrefticketlist != null && objrefticketlist.Count > 0)
                            {
                                foreach (var item1 in objrefticketlist)
                                {
                                    model.Add(item1);
                                    var objrefticketlist1 = _Util.Facade.TicketFacade.GetTicketListByReferenceTicketId(item1.Id);
                                    if (objrefticketlist1 != null && objrefticketlist1.Count > 0)
                                    {
                                        foreach (var item2 in objrefticketlist1)
                                        {
                                            model.Add(item2);
                                        }
                                    }
                                }
                            }
                        }
                        model.Add(objticket);
                    }
                    else
                    {
                        model.Add(objticket);
                    }
                }
                if (model != null && model.Count > 0)
                {
                    foreach (var item in model)
                    {
                        result = _Util.Facade.TicketFacade.DeleteTicket(item.Id);
                    }
                    base.AddUserActivityForCustomer("Ticket is deleted #Ref: Ticket#" + TicketId, LabelHelper.ActivityAction.Delete, CustomerId, null, TicketId);

                }
            }
            return Json(result);
        }

        public ActionResult TicketNotifySetting()
        {
            TicketNotificationModel model = new TicketNotificationModel();
            model.TicketStatusList = _Util.Facade.LookupFacade.GetLookupByKey("TicketStatus").Where(x => x.DataValue != "-1").ToList();
            model.TicketTypeList = _Util.Facade.LookupFacade.GetLookupByKey("TicketType");
            model.ListNotification = _Util.Facade.TicketFacade.GetAllTicketCustomerNotifications();
            return View(model);
        }

        public JsonResult SaveTicketNotification(List<TicketCustomerNotification> notify)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if (notify != null && notify.Count > 0)
            {
                foreach (var item in notify)
                {
                    var objnoti = _Util.Facade.TicketFacade.GetTicketCustomerNotificationByStatusAndType(item.TicketStatus, item.TicketType);
                    if (objnoti != null)
                    {
                        objnoti.LastUpdatedBy = CurrentLoggedInUser.UserId;
                        objnoti.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        objnoti.Email = item.Email;
                        objnoti.Text = item.Text;
                        result = _Util.Facade.TicketFacade.UpdateTicketNotification(objnoti);
                    }
                    else
                    {
                        item.LastUpdatedBy = CurrentLoggedInUser.UserId;
                        item.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        item.CreatedBy = CurrentLoggedInUser.UserId;
                        item.CreatedDate = DateTime.Now.UTCCurrentTime();
                        result = _Util.Facade.TicketFacade.InsertTicketNotification(item) > 0;
                    }
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetTicketNotificationData(string status, string type)
        {
            TicketCustomerNotification model = new TicketCustomerNotification();
            string email = "";
            string text = "";
            string notifyid = "0";
            if (!string.IsNullOrWhiteSpace(status) && !string.IsNullOrWhiteSpace(type))
            {
                model = _Util.Facade.TicketFacade.GetTicketCustomerNotificationByStatusAndType(status, type);
                if (model != null)
                {
                    email = model.Email;
                    text = model.Text;
                    notifyid = model.Id.ToString();
                }
            }
            return Json(new { result = true, email = email, text = text, notifyid = notifyid });
        }
        public ActionResult LoadBadInventoryAndUserAssignPopup(Guid? appid, int? caeIntId, Guid? cusid, int count, int qty)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            BadInventoryUserAssignModel model = new BadInventoryUserAssignModel();
            if (appid.HasValue && appid.Value != new Guid() && caeIntId.HasValue && caeIntId.Value > 0 && cusid.HasValue && cusid.Value != new Guid())
            {
                model.Ticket = _Util.Facade.TicketFacade.GetTicketByTicketIdAndCustomerId(appid.Value, cusid.Value);
                model.TicketUser = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndPrimary(new Guid(), appid.Value).FirstOrDefault();
                model.CustomerAppointmentEquipment = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentEquipmentById(caeIntId.Value);
            }
            var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid());
            ViewBag.TechnicianList = TechnicianList.OrderBy(x => x.FirstName).ToList();
            ViewBag.UserList = _Util.Facade.EmployeeFacade.GetAllEmployeeUserAssign().OrderBy(x => x.FirstName).Select(x => new SelectListItem()
            {
                Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                Value = x.UserId.ToString()
            }).ToList();
            ViewBag.count = count;
            ViewBag.qty = qty;
            ViewBag.BadInvReason = _Util.Facade.LookupFacade.GetLookupByKey("BadInventoryReason").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult ChangeCreatedByUser(int? id, Guid? changeid, Guid? InstalledBy, bool? chkExist, bool? chkIsNonCommissionable)
        {
            bool result = false;
            if (id.HasValue && id.Value > 0)
            {
                if (changeid == null)
                {
                    changeid = Guid.Empty;
                }
                if (InstalledBy == null)
                {
                    InstalledBy = Guid.Empty;
                }
                var objappeqp = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentEquipmentById(id.Value);
                if (objappeqp != null)
                {
                    var emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(InstalledBy.Value);
                    if (emp != null)
                    {
                        List<AppointmentEquipmentIdList> ObjInstalledBy = _Util.Facade.CustomerFacade.GetAllIndividualInstalledEquipmentIdById(id.Value);
                        if (ObjInstalledBy.Count() > 0 && ObjInstalledBy != null)
                        {
                            foreach (var item in ObjInstalledBy)
                            {
                                IndividualInstalledEquipment Exist = _Util.Facade.CustomerFacade.GetIndividualInstalledEquipmentById(item.Id);
                                if (Exist != null)
                                {
                                    Exist.EmpUser = emp.FirstName + ' ' + emp.LastName;
                                    Exist.InstalledByUid = emp.UserId;
                                    result = _Util.Facade.CustomerFacade.UpdateIndividualInstalledEquipment(Exist);
                                }
                            }
                        }
                    }
                    var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(changeid.Value);
                    if (objemp != null)
                    {
                        objappeqp.CreatedBy = objemp.FirstName + " " + objemp.LastName;
                    }
                    objappeqp.InstalledByUid = InstalledBy.Value;
                    objappeqp.CreatedByUid = changeid.Value;
                    if (chkExist.HasValue && chkExist.Value == true)
                    {
                        objappeqp.IsEquipmentExist = true;
                        objappeqp.IsEquipmentRelease = true;
                        //objappeqp.QuantityLeftEquipment = objappeqp.Quantity;

                        objappeqp.UnitPrice = 0;
                        objappeqp.TotalPrice = 0;
                    }
                    else
                    {
                        objappeqp.IsEquipmentExist = false;
                    }
                    objappeqp.IsNonCommissionable = chkIsNonCommissionable;
                    result = _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(objappeqp);
                }
            }
            return Json(result);
        }


        [Authorize]
        [HttpPost]
        public JsonResult SaveTicketBookingItems(string BookingId, List<TicketBookingDetails> TicketBookingDetails, List<TicketBookingExtraItem> TicketBookingExtraItems, bool? RecreateInvoice)
        {
            CommonJesonresponse result = new CommonJesonresponse();
            try
            {
                result = SaveTicketBooking(BookingId, TicketBookingDetails, TicketBookingExtraItems, RecreateInvoice);
            }
            catch (Exception e)
            {
                result = new CommonJesonresponse()
                {
                    result = false,
                    message = "Internal server error. Please contact system admin."
                };
            }
            return Json(result);
        }


        [HttpPost]
        public JsonResult CreateInvoiceTicketAddItem(Ticket ticket, CustomerAppointment CustomerAppointment)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            double amount = 0.0;
            double serviceamount = 0.0;
            double taxamount = 0.0;
            double totalamount = 0.0;
            Invoice Invoice = new Invoice();
            string message = "";
            string InvoiceFor = "";
            if (ticket.Id > 0)
            {
                var objticket = _Util.Facade.TicketFacade.GetTicketById(ticket.Id);
                if (objticket != null)
                {
                    var tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(objticket.CustomerId);
                    if (tempCustomer != null)
                    {
                        if (CustomerAppointment != null && CustomerAppointment.CustomerAppointmentEquipmentList != null && CustomerAppointment.CustomerAppointmentEquipmentList.Count > 0)
                        {
                            foreach (var item in CustomerAppointment.CustomerAppointmentEquipmentList)
                            {
                                if (item.IsInvoiceCreate == false && item.IsService == true)
                                {
                                    InvoiceFor = LabelHelper.InvoiceFor.Service;
                                }
                                else
                                {
                                    InvoiceFor = LabelHelper.InvoiceFor.Equipment;
                                }
                            }

                            var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
                            Invoice = new Invoice()
                            {
                                CustomerId = objticket.CustomerId,
                                CustomerName = tempCustomer.FirstName + " " + tempCustomer.LastName,
                                IsEstimate = false,
                                IsBill = false,
                                CompanyId = CurrentUser.CompanyId.Value,
                                Amount = 0,
                                BalanceDue = 0,
                                Balance = 0,
                                Deposit = 0,
                                Tax = 0,
                                LateFee = 0,
                                LateAmount = 0,
                                Status = "Init",
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CreatedBy = User.Identity.Name,
                                InvoiceFor = InvoiceFor,//LabelHelper.InvoiceFor.Service,//"Ticket",
                                InvoiceDate = DateTime.Now.UTCCurrentTime(),
                                DueDate = DateTime.Now.UTCCurrentTime(),
                                CreatedByUid = CurrentUser.UserId,
                                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                LastUpdatedByUid = CurrentUser.UserId,
                                DiscountAmount = 0,
                                RefType = objticket.Id.ToString(),
                                TicketId = objticket.TicketId,
                                ItemType = ""
                            };
                            Invoice.BillingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "BillingAddress", AddressTemplate);
                            Invoice.ShippingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "ShippingAddress", AddressTemplate);
                            Invoice.LastUpdatedByUid = CurrentUser.UserId;
                            Invoice.Id = _Util.Facade.InvoiceFacade.InsertInvoice(Invoice);
                            Invoice.InvoiceId = Invoice.Id.GenerateInvoiceNo();
                            //Invoice.InvoiceFor = "Ticket";
                            Invoice.Status = "Open";
                            _Util.Facade.InvoiceFacade.UpdateInvoice(Invoice);
                            Guid tempCustomerId = new Guid();
                            if (tempCustomer != null)
                            {
                                tempCustomerId = tempCustomer.CustomerId;
                            }
                            var objsetting = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, tempCustomerId);
                            foreach (var item in CustomerAppointment.CustomerAppointmentEquipmentList)
                            {
                                if (item.IsBilling == false || (item.IsBilling == true && item.IsBillingProcess == true))
                                {
                                    amount += item.TotalPrice;
                                    InvoiceDetail InvoiceDetail = new InvoiceDetail()
                                    {
                                        InvoiceId = Invoice.InvoiceId,
                                        InventoryId = new Guid(),
                                        EquipmentId = item.EquipmentId,
                                        EquipName = item.EquipName,
                                        EquipDetail = item.EquipDetail,
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        Quantity = item.Quantity,
                                        UnitPrice = item.UnitPrice,
                                        TotalPrice = item.TotalPrice,
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        CreatedBy = CurrentUser.Identity.Name,
                                        Taxable = true
                                    };
                                    if (item.IsService == true)
                                    {
                                        serviceamount += item.TotalPrice;
                                        InvoiceDetail.Taxable = false;
                                    }
                                    _Util.Facade.InvoiceFacade.InsertInvoiceDetails(InvoiceDetail);

                                    var objappointmenteqp = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentEquipmentById(item.Id);
                                    if (objappointmenteqp != null)
                                    {
                                        objappointmenteqp.IsInvoiceCreate = true;
                                        objappointmenteqp.ReferenceInvoiceId = Invoice.InvoiceId;
                                        objappointmenteqp.ReferenceInvDetailId = InvoiceDetail.Id;
                                        _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(objappointmenteqp);
                                    }

                                }
                            }
                            var TaxGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "IsServiceTaxable");
                            var ServiceTax = 0.0;
                            ServiceTax = (serviceamount * (objsetting != null ? Convert.ToDouble(objsetting.Value) : 0.0)) / 100;

                            if (TaxGlobal != null && TaxGlobal.Value.ToLower() == "true")
                            {
                                taxamount = (amount * (objsetting != null ? Convert.ToDouble(objsetting.Value) : 0.0)) / 100;
                            }
                            else
                            {

                                taxamount = (amount * (objsetting != null ? Convert.ToDouble(objsetting.Value) : 0.0)) / 100;
                                taxamount = taxamount - ServiceTax;
                            }


                            totalamount = amount + taxamount;
                            Invoice.Amount = amount;
                            Invoice.Balance = totalamount;
                            Invoice.BalanceDue = totalamount;
                            Invoice.Tax = taxamount;
                            Invoice.TotalAmount = totalamount;
                            result = _Util.Facade.InvoiceFacade.UpdateInvoice(Invoice);
                            message = "Invoice created successfully";
                        }
                        else
                        {
                            message = "No new equipments or services added";
                        }
                    }
                }
            }
            return Json(new { result = result, message = message, Invoiceid = Invoice.Id, TicketId = ticket.Id });
        }

        [HttpPost]
        public JsonResult CustomerCreditForTicketInvoice(int? appointeqpid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            double taxamount = 0.0;
            double totalcreditamount = 0.0;
            if (appointeqpid.HasValue && appointeqpid.Value > 0)
            {
                var obappointeqp = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentEquipmentById(appointeqpid.Value);
                if (obappointeqp != null)
                {
                    var objinv = _Util.Facade.InvoiceFacade.GetInvoiceByInvoiceId(obappointeqp.ReferenceInvoiceId);
                    if (objinv != null)
                    {
                        if (objinv.Status == "Open")
                        {
                            var objinvdetail = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(objinv.InvoiceId);
                            if (objinvdetail != null && objinvdetail.Count == 1)
                            {
                                foreach (var item in objinvdetail)
                                {
                                    if (item.EquipmentId == obappointeqp.EquipmentId && item.Id == obappointeqp.ReferenceInvDetailId)
                                    {
                                        result = _Util.Facade.InvoiceFacade.DeleteInvoiceById(objinv.Id);
                                    }
                                }
                            }
                            else if (objinvdetail != null && objinvdetail.Count > 1)
                            {
                                foreach (var item in objinvdetail)
                                {
                                    if (item.EquipmentId == obappointeqp.EquipmentId && item.Id == obappointeqp.ReferenceInvDetailId)
                                    {
                                        objinv.Amount = objinv.Amount - (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
                                        Guid tempCustomerId = new Guid();
                                        if (obappointeqp != null)
                                        {
                                            tempCustomerId = objinv.CustomerId;
                                        }
                                        var objsetting = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, tempCustomerId);
                                        objinv.Tax = (objinv.Amount * (objsetting != null ? Convert.ToDouble(objsetting.Value) : 0.0)) / 100;
                                        objinv.TotalAmount = objinv.Amount + objinv.Tax;
                                        objinv.Balance = objinv.TotalAmount;
                                        objinv.BalanceDue = objinv.TotalAmount;
                                        _Util.Facade.InvoiceFacade.UpdateInvoice(objinv);
                                        result = _Util.Facade.InvoiceFacade.DeleteInvoiceDetailById(item.Id);
                                    }
                                }
                            }
                        }
                        else if (objinv.Status == "Paid")
                        {
                            var objinvdetail = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(objinv.InvoiceId);
                            if (objinvdetail != null && objinvdetail.Count > 0)
                            {
                                foreach (var item in objinvdetail)
                                {
                                    if (item.EquipmentId == obappointeqp.EquipmentId && item.Id == obappointeqp.ReferenceInvDetailId)
                                    {
                                        var objtrans = _Util.Facade.TransactionFacade.GetTransactionByReferenceId(objinv.InvoiceId);
                                        if (objtrans != null && obappointeqp.TotalPrice > 0)
                                        {
                                            Guid tempCustomerId = new Guid();
                                            if (obappointeqp != null)
                                            {
                                                tempCustomerId = objinv.CustomerId;
                                            }
                                            var objsetting = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, tempCustomerId);
                                            taxamount = (obappointeqp.TotalPrice * (!string.IsNullOrWhiteSpace(objsetting.Value) ? Convert.ToDouble(objsetting.Value) : 0.0)) / 100;
                                            totalcreditamount = obappointeqp.TotalPrice + taxamount;
                                            string CreditNote = string.Format(@"Invoice# <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{0}')"">{1}</a>", objinv.Id, objinv.InvoiceId);
                                            CustomerCredit CustomerCredit = new CustomerCredit()
                                            {
                                                CompanyId = CurrentUser.CompanyId.Value,
                                                CustomerId = objinv.CustomerId,
                                                TransactionId = objtrans.Id,
                                                Type = "Credit",
                                                Amount = Math.Round(totalcreditamount, 2),
                                                CreatedBy = CurrentUser.UserId,
                                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                                Note = CreditNote
                                            };
                                            result = _Util.Facade.CustomerFacade.InsertCustomerCredit(CustomerCredit);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Ticket ticketModel = _Util.Facade.TicketFacade.GetTicketByTicketId(obappointeqp.AppointmentId);
                    result = _Util.Facade.CustomerAppoinmentFacade.DeleteCustomerAppoinmentEquipment(obappointeqp.Id);
                    List<AppointmentEquipmentIdList> ObjInstalledBy = _Util.Facade.CustomerFacade.GetAllIndividualInstalledEquipmentIdById(appointeqpid.Value);
                    if (ObjInstalledBy.Count() > 0 && ObjInstalledBy != null)
                    {
                        foreach (var item in ObjInstalledBy)
                        {
                            IndividualInstalledEquipment Exist = _Util.Facade.CustomerFacade.GetIndividualInstalledEquipmentById(item.Id);
                            if (Exist != null)
                            {
                                result = _Util.Facade.CustomerFacade.DeleteIndividualInstalledEquipmentById(Exist.Id);
                            }
                        }
                    }
                    if (result && ticketModel != null)
                    {
                        var objpackageeqp = _Util.Facade.PackageFacade.GetPackageCustomerEqpByCustomerIdAndEquipmentIdAndAppointmentEqpId(ticketModel.CustomerId, obappointeqp.EquipmentId, appointeqpid.Value);
                        if (objpackageeqp != null)
                        {
                            _Util.Facade.PackageFacade.DeleteCustomerPackageEqpById(objpackageeqp.Id);
                        }
                        var objpackageservice = _Util.Facade.PackageFacade.GetPackageCustomerServiceByCustomerIdAndEquipmentIdAndAppointmentEqpId(ticketModel.CustomerId, obappointeqp.EquipmentId, appointeqpid.Value);
                        if (objpackageservice != null)
                        {
                            _Util.Facade.PackageFacade.DeleteCustomerPackageServiceById(objpackageservice.Id);
                        }
                        base.AddUserActivityForCustomer("Ticket #" + ticketModel.Id + " " + obappointeqp.EquipName + " is deleted, quantity is " + obappointeqp.Quantity, LabelHelper.ActivityAction.UpdateTicket, ticketModel.CustomerId, null, null);
                    }
                }
            }
            return Json(new { result = result });
        }

        private CommonJesonresponse SaveTicketBooking(string BookingId, List<TicketBookingDetails> TicketBookingDetails, List<TicketBookingExtraItem> TicketBookingExtraItems, bool? RecreateInvoice, Ticket TicketInfo = null)
        {
            CommonJesonresponse result = new CommonJesonresponse();

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Booking booking = new Booking();
            Invoice invoiceInfo = new Invoice();
            Invoice PaidInv = new Invoice();
            double Amount = 0;
            double PaidAmount = 0;
            double DiscountAmount = 0;
            #region Validations 
            if (string.IsNullOrWhiteSpace(BookingId) || string.IsNullOrEmpty(BookingId))
            {
                result.result = false; result.message = "Booking ID not found.";
                return result;
            }
            booking = _Util.Facade.BookingFacade.GetBookingByBookingId(BookingId);
            invoiceInfo = _Util.Facade.InvoiceFacade.GetInvoicebyBookingId(BookingId).FirstOrDefault();
            var transactioninfo = _Util.Facade.TransactionFacade.GetTransactionHistoryByInvoiceId(invoiceInfo != null ? invoiceInfo.Id : 0);

            if (booking == null)
            {
                result.result = false; result.message = "Booking not found."; ;
                return result;
            }
            #endregion

            #region Check if Ticket already received any payment
            if (transactioninfo.Count > 0)
            {
                foreach (var item in transactioninfo)
                {
                    PaidAmount += item.Amout;
                }
            }

            if (invoiceInfo != null)
            {
                if (TicketBookingDetails != null && TicketBookingDetails.Count > 0)
                {
                    foreach (TicketBookingDetails item in TicketBookingDetails)
                    {
                        if (item.TotalPrice.HasValue)
                        {
                            Amount += item.TotalPrice.Value;
                        }
                    }
                }
                if (TicketBookingExtraItems != null && TicketBookingExtraItems.Count > 0)
                {
                    foreach (TicketBookingExtraItem item in TicketBookingExtraItems)
                    {
                        if (item.TotalPrice.HasValue)
                        {
                            Amount += item.TotalPrice.Value;
                        }
                    }
                }
                double TaxPercentage = 8.25;
                GlobalSetting globalSetting = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, booking.CustomerId);
                if (globalSetting != null)
                {
                    double.TryParse(globalSetting.Value, out TaxPercentage);
                }
                double Tax = Amount * TaxPercentage / 100;
                double TotalAmount = Tax + Amount;
                if (invoiceInfo.Status.ToLower() == "paid")
                {
                    if (invoiceInfo.TotalAmount == TotalAmount)
                    {
                        result.result = false;
                        result.message = "You have already collected payment for this booking: " + BookingId;
                        return result;
                    }
                    if (invoiceInfo.TotalAmount > TotalAmount)
                    {
                        result.result = false;
                        result.message = "Sorry! You cannot change item total amount less than amount: $" + ((double)invoiceInfo.TotalAmount).ToString("N2");
                        return result;
                    }
                }
                if (PaidAmount > 0)
                {
                    if (PaidAmount > TotalAmount)
                    {
                        result.result = false;
                        result.message = "Sorry! You cannot change item total amount less than amount: $" + ((double)invoiceInfo.TotalAmount).ToString("N2");
                        return result;
                    }
                }
            }
            #endregion
            //_Util.Facade.BookingFacade.DeleteTicketBookingDetailsByBookingId(BookingId);
            //_Util.Facade.BookingFacade.DeleteTicketBookingExtraItemByBookingId(BookingId);
            Amount = 0;
            #region Update Booking details
            if (TicketBookingDetails != null && TicketBookingDetails.Count > 0)
            {
                //string IdList = string.Join(",", TicketBookingDetails.Select(x => x.Id));
                //if (!string.IsNullOrWhiteSpace(IdList))
                //{
                //    _Util.Facade.BookingFacade.DeleteTicketBookingDetailsExcludingIdByBookingId(BookingId, IdList);
                //}
                foreach (TicketBookingDetails item in TicketBookingDetails)
                {
                    if (item.Id > 0)
                    {
                        TicketBookingDetails tbd = _Util.Facade.BookingFacade.GetTicketBookingDetailsById(item.Id);
                        if (tbd != null)
                        {
                            #region Update
                            tbd.RugType = item.RugType;
                            tbd.Length = item.Length;
                            tbd.LengthInch = item.LengthInch;
                            tbd.Width = item.Width;
                            tbd.WidthInch = item.WidthInch;
                            tbd.Radius = item.Radius;
                            tbd.RadiusInch = item.RadiusInch;
                            tbd.TotalSize = item.TotalSize;
                            tbd.Package = item.Package;
                            tbd.Included = item.Included;
                            tbd.Extras = item.Extras;
                            tbd.UnitPrice = item.UnitPrice;
                            tbd.DiscountType = item.DiscountType;
                            tbd.TaxType = item.TaxType;
                            tbd.Quantity = item.Quantity;
                            tbd.Discount = item.Discount;
                            tbd.TotalPrice = item.TotalPrice;
                            _Util.Facade.BookingFacade.UpdateTicketBookingDetails(tbd);
                            #endregion
                        }
                        else
                        {
                            #region Insert
                            item.CompanyId = CurrentUser.CompanyId.Value;
                            item.AddedBy = CurrentUser.UserId;
                            item.AddedDate = DateTime.Now.UTCCurrentTime();
                            _Util.Facade.BookingFacade.InsertTicketBookingDetails(item);
                            #endregion
                        }
                    }
                    else
                    {
                        #region Insert
                        item.CompanyId = CurrentUser.CompanyId.Value;
                        item.AddedBy = CurrentUser.UserId;
                        item.AddedDate = DateTime.Now.UTCCurrentTime();
                        _Util.Facade.BookingFacade.InsertTicketBookingDetails(item);
                        #endregion
                    }
                    if (item.TotalPrice.HasValue)
                    {
                        Amount += item.TotalPrice.Value;
                    }
                    if (item.Discount.HasValue)
                    {
                        DiscountAmount += item.Discount.Value;

                    }

                }
            }
            else
            {
                _Util.Facade.BookingFacade.DeleteTicketBookingDetailsByBookingId(BookingId);
            }
            #endregion

            #region Update Booking Extra Items
            if (TicketBookingExtraItems != null && TicketBookingExtraItems.Count > 0)
            {
                int BookingIntId = 0;
                string[] tokens = BookingId.Split(new[] { "BK" }, StringSplitOptions.None);
                if (tokens.Length == 2)
                {
                    if (int.TryParse(tokens[1], out BookingIntId) && BookingIntId > 0)
                    {
                        //string IdList = string.Join(",", TicketBookingExtraItems.Select(x => x.Id));
                        //if (!string.IsNullOrWhiteSpace(IdList))
                        //{
                        //    _Util.Facade.BookingFacade.DeleteTicketBookingExtraItemExcludingIdByBookingId(BookingIntId, IdList);
                        //}

                        foreach (TicketBookingExtraItem item in TicketBookingExtraItems)
                        {
                            if (item.Id > 0)
                            {
                                TicketBookingExtraItem TBEI = _Util.Facade.BookingFacade.GetTicketBookingExtraItemById(item.Id);
                                if (TBEI != null)
                                {
                                    #region Update
                                    TBEI.EquipmentId = item.EquipmentId;
                                    TBEI.EquipName = item.EquipName;
                                    TBEI.EquipDetail = item.EquipDetail;
                                    TBEI.Quantity = item.Quantity;
                                    TBEI.UnitPrice = item.UnitPrice;
                                    TBEI.Discount = item.Discount;
                                    TBEI.TotalPrice = item.TotalPrice;

                                    _Util.Facade.BookingFacade.UpdateTicketBookingExtraItem(TBEI);
                                    #endregion
                                }
                                else
                                {
                                    #region Insert
                                    item.CreatedBy = CurrentUser.GetFullName();
                                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                                    item.BookingId = BookingIntId;
                                    _Util.Facade.BookingFacade.InsertTicketBookingExtraItem(item);
                                    #endregion
                                }
                            }
                            else
                            {
                                #region Insert
                                item.CreatedBy = CurrentUser.GetFullName();
                                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                                item.BookingId = BookingIntId;
                                _Util.Facade.BookingFacade.InsertTicketBookingExtraItem(item);
                                #endregion
                            }
                            if (item.TotalPrice.HasValue)
                            {
                                Amount += item.TotalPrice.Value;
                            }
                            if (item.Discount.HasValue)
                            {
                                DiscountAmount += item.Discount.Value;
                            }
                        }
                    }
                }
            }
            else
            {
                _Util.Facade.BookingFacade.DeleteTicketBookingExtraItemByBookingId(BookingId);
            }
            #endregion




            if (RecreateInvoice.HasValue && RecreateInvoice.Value)
            {
                //what will hapen to the paid invoices?
                //Need to talk about this
                double TaxPercentage = 8.25;
                GlobalSetting globalSetting = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, booking.CustomerId);
                if (globalSetting != null)
                {
                    double.TryParse(globalSetting.Value, out TaxPercentage);
                }
                double Tax = Amount * TaxPercentage / 100;
                double TotalAmount = Tax + Amount;
                if (invoiceInfo != null)
                {
                    string TempStatus = invoiceInfo.Status;

                    #region Update Invoice
                    Invoice inv = new Invoice()
                    {
                        BookingId = BookingId,
                        LastUpdatedByUid = CurrentUser.UserId,
                        LastUpdatedDate = DateTime.Now.UTCToClientTime(),
                        CreatedBy = User.Identity.Name,
                        CreatedByUid = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        DiscountType = "amount",
                        Description = "Update for " + BookingId,
                        InvoiceFor = "Invoice",
                        TaxType = "Sales Tax",
                        Deposit = 0,
                        DiscountAmount = DiscountAmount,
                        Tax = Tax,
                        Amount = Amount,
                        TotalAmount = TotalAmount,
                        ShippingAddress = booking.BillingAddress,
                        BillingAddress = booking.BillingAddress,
                        DueDate = DateTime.Now.UTCCurrentTime().SetZeroHour().AddDays(30),
                        InvoiceDate = DateTime.Now.UTCCurrentTime().UTCToClientTime().SetZeroHour(),
                        IsEstimate = false,
                        IsBill = false,
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = booking.CustomerId,
                    };
                    if (invoiceInfo.Status.ToLower() == "paid")
                    {
                        inv.Status = LabelHelper.InvoiceStatus.Partial;
                    }
                    else
                    {
                        inv.Status = invoiceInfo.Status;
                    }
                    if (transactioninfo.Count > 0)
                    {
                        inv.BalanceDue = TotalAmount - PaidAmount;
                    }
                    else
                    {
                        inv.BalanceDue = TotalAmount;
                    }
                    if (TicketInfo != null && TicketInfo.TicketType == "Drop Off" && invoiceInfo.DueDate != TicketInfo.CompletionDate)
                    {
                        inv.DueDate = TicketInfo.CompletionDate;
                        inv.Terms = "Custom";
                    }
                    if (TicketInfo.TicketType == "Pick Up" || TicketInfo.TicketType == "Service")
                    {
                        inv.DueDate = invoiceInfo.DueDate;
                        inv.Terms = "Custom";
                    }

                    inv.Id = invoiceInfo.Id;
                    inv.InvoiceId = invoiceInfo.InvoiceId;
                    _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
                    if (TempStatus != inv.Status)
                    {
                        #region log
                        int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                        bool newBool = inv.IsARBInvoice ?? false;


                        base.AddUserActivityForCustomer("Invoice Status Changed from " + TempStatus + " To " + inv.Status + "#InvoiceId: " + inv.InvoiceId, lineNumber + "," + actionName + "/" + controllerName, inv.CustomerId, null, null, newBool);
                        #endregion
                    }
                    #endregion

                    var InvoiceDetailsInfoList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(invoiceInfo.InvoiceId);
                    int ItemCount = 0;
                    #region Update Invoice Detail
                    if (TicketBookingDetails != null && TicketBookingDetails.Count > 0)
                    {
                        foreach (var item in TicketBookingDetails)
                        {
                            string EquipmentName = "Package {0}, Shape: {1} ({2} = {3}sf)";
                            string RugShape = string.Format(@"{0}'{1}""", item.Radius.HasValue ? item.Radius.Value : 0, item.RadiusInch.HasValue ? item.RadiusInch.Value : 0);
                            if (item.RugType != "Circle")
                            {
                                RugShape = string.Format(@"{0}'{1}""X{2}'{3}""",
                                    item.Length.HasValue ? item.Length.Value : 0,
                                        item.LengthInch.HasValue ? item.LengthInch.Value : 0,
                                        item.Width.HasValue ? item.Width.Value : 0,
                                        item.WidthInch.HasValue ? item.WidthInch.Value : 0);
                            }
                            EquipmentName = string.Format(EquipmentName, item.Package, item.RugType, RugShape, Math.Round(item.TotalSize.HasValue ? item.TotalSize.Value : 0, 2));

                            string EquipmentDetails = "Included Items: {0}";
                            EquipmentDetails = string.Format(EquipmentDetails, string.IsNullOrWhiteSpace(item.Included) ? "" : item.Included);

                            InvoiceDetail invedet = new InvoiceDetail()
                            {
                                CompanyId = booking.CompanyId,
                                CreatedBy = User.Identity.Name,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                InvoiceId = invoiceInfo.InvoiceId,
                                Taxable = true,
                                Quantity = item.Quantity,
                                EquipDetail = EquipmentDetails,
                                EquipName = EquipmentName,
                                UnitPrice = item.UnitPrice * item.TotalSize,
                                TotalPrice = item.TotalPrice,
                                EquipmentId = Guid.Empty,
                                DiscountAmount = item.Discount
                            };
                            if (InvoiceDetailsInfoList.Count > ItemCount)
                            {
                                invedet.Id = InvoiceDetailsInfoList[ItemCount].Id;
                                _Util.Facade.InvoiceFacade.UpdateInvoiceDetails(invedet);
                                ItemCount += 1;
                            }
                            else
                            {
                                _Util.Facade.InvoiceFacade.InsertInvoiceDetails(invedet);
                                ItemCount += 1;
                            }
                        }
                    }
                    if (TicketBookingExtraItems != null && TicketBookingExtraItems.Count > 0)
                    {
                        foreach (var item in TicketBookingExtraItems)
                        {
                            InvoiceDetail invedet = new InvoiceDetail()
                            {
                                CompanyId = booking.CompanyId,
                                CreatedBy = User.Identity.Name,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                InvoiceId = invoiceInfo.InvoiceId,
                                Taxable = true,
                                Quantity = item.Quantity,
                                EquipDetail = item.EquipDetail,
                                EquipName = item.EquipName,
                                UnitPrice = item.UnitPrice,
                                TotalPrice = item.TotalPrice,
                                EquipmentId = item.EquipmentId,
                                DiscountAmount = item.Discount
                            };
                            if (InvoiceDetailsInfoList.Count > ItemCount)
                            {
                                invedet.Id = InvoiceDetailsInfoList[ItemCount].Id;
                                _Util.Facade.InvoiceFacade.UpdateInvoiceDetails(invedet);
                                ItemCount += 1;
                            }
                            else
                            {
                                _Util.Facade.InvoiceFacade.InsertInvoiceDetails(invedet);
                            }
                        }
                    }
                    #endregion

                    result.result = true; result.message = "Invoice created successfully.";
                    return result;
                }
                else
                {
                    #region Insert Invoice
                    Invoice inv = new Invoice()
                    {
                        BookingId = BookingId,
                        LastUpdatedByUid = CurrentUser.UserId,
                        LastUpdatedDate = DateTime.Now.UTCToClientTime(),
                        CreatedBy = User.Identity.Name,
                        CreatedByUid = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        DiscountType = "amount",
                        Description = "Recreated for " + BookingId,
                        InvoiceFor = "Invoice",
                        TaxType = "Sales Tax",

                        Deposit = 0,
                        BalanceDue = TotalAmount,
                        DiscountAmount = DiscountAmount,
                        Tax = Tax,
                        Amount = Amount,
                        TotalAmount = TotalAmount,

                        ShippingAddress = booking.BillingAddress,
                        BillingAddress = booking.BillingAddress,
                        DueDate = DateTime.Now.UTCCurrentTime().SetZeroHour().AddDays(30),
                        IsEstimate = false,
                        IsBill = false,
                        InvoiceDate = DateTime.Now.UTCCurrentTime().UTCToClientTime().SetZeroHour(),
                        Status = LabelHelper.InvoiceStatus.Open,
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = booking.CustomerId,
                    };
                    inv.Id = _Util.Facade.InvoiceFacade.InsertInvoice(inv);
                    inv.InvoiceId = inv.Id.GenerateInvoiceNo();
                    _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
                    #endregion

                    #region Insert Invoice Detail
                    if (TicketBookingDetails != null && TicketBookingDetails.Count > 0)
                    {
                        foreach (var item in TicketBookingDetails)
                        {
                            string EquipmentName = "Package {0}, Shape: {1} ({2} = {3}sf)";
                            string RugShape = string.Format(@"{0}'{1}""", item.Radius.HasValue ? item.Radius.Value : 0, item.RadiusInch.HasValue ? item.RadiusInch.Value : 0);
                            if (item.RugType != "Circle")
                            {
                                RugShape = string.Format(@"{0}'{1}""X{2}'{3}""",
                                    item.Length.HasValue ? item.Length.Value : 0,
                                        item.LengthInch.HasValue ? item.LengthInch.Value : 0,
                                        item.Width.HasValue ? item.Width.Value : 0,
                                        item.WidthInch.HasValue ? item.WidthInch.Value : 0);
                            }
                            EquipmentName = string.Format(EquipmentName, item.Package, item.RugType, RugShape, Math.Round(item.TotalSize.HasValue ? item.TotalSize.Value : 0, 2));

                            string EquipmentDetails = "Included Items: {0}";
                            EquipmentDetails = string.Format(EquipmentDetails, string.IsNullOrWhiteSpace(item.Included) ? "" : item.Included);

                            InvoiceDetail invedet = new InvoiceDetail()
                            {
                                CompanyId = booking.CompanyId,
                                CreatedBy = User.Identity.Name,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                InvoiceId = inv.InvoiceId,
                                Taxable = true,
                                Quantity = item.Quantity,
                                EquipDetail = EquipmentDetails,
                                EquipName = EquipmentName,
                                UnitPrice = item.UnitPrice * item.TotalSize,
                                TotalPrice = item.TotalPrice,
                                EquipmentId = Guid.Empty,
                                DiscountAmount = item.Discount
                            };
                            _Util.Facade.InvoiceFacade.InsertInvoiceDetails(invedet);
                        }
                    }
                    if (TicketBookingExtraItems != null && TicketBookingExtraItems.Count > 0)
                    {
                        foreach (var item in TicketBookingExtraItems)
                        {
                            InvoiceDetail invedet = new InvoiceDetail()
                            {
                                CompanyId = booking.CompanyId,
                                CreatedBy = User.Identity.Name,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                InvoiceId = inv.InvoiceId,
                                Taxable = true,
                                Quantity = item.Quantity,
                                EquipDetail = item.EquipDetail,
                                EquipName = item.EquipName,
                                UnitPrice = item.UnitPrice,
                                TotalPrice = item.TotalPrice,
                                EquipmentId = item.EquipmentId,
                                DiscountAmount = item.Discount
                            };
                            _Util.Facade.InvoiceFacade.InsertInvoiceDetails(invedet);
                        }
                    }
                    #endregion

                    result.result = true; result.message = "Invoice created successfully.";
                    return result;
                }
            }

            result.result = true; result.message = "Items saved successfully.";
            return result;
        }

        [HttpPost]
        public JsonResult SaveAddendumSignature(Guid ticketid, Guid customerid, string data)
        {
            bool result = false;
            string filePath = "";
            Guid CompanyId = new Guid();
            if (User.Identity.IsAuthenticated)
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CompanyId = CurrentUser.CompanyId.Value;
            }
            else
            {
                var cusobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid);
                if (cusobj != null)
                {
                    CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cusobj.Id);
                    CompanyId = custommerCompany.CompanyId;
                }
                else
                {
                    return Json(result);
                }
            }
            if (ticketid != new Guid() && customerid != new Guid())
            {
                var ticketobj = _Util.Facade.TicketFacade.GetTicketByTicketIdAndCustomerId(ticketid, customerid);
                if (ticketobj != null)
                {
                    string[] datasplit = data.Split(',');
                    byte[] bytes = Convert.FromBase64String(datasplit[1]);
                    Image image;
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        image = Image.FromStream(ms);
                        string tempFolder = ConfigurationManager.AppSettings["File.CustomerSignatureFile"];
                        var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                        var FtempFolderName = string.Format(tempFolder, comname) + ticketobj.Id + "AddendumSignature";
                        Random rand = new Random();
                        string FileName = rand.Next().ToString();
                        FileName += "-___" + "Signature.png";
                        string tempFolderPath = Server.MapPath("~/" + FtempFolderName);

                        if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                        {
                            try
                            {
                                image.Save(Path.Combine(tempFolderPath, FileName));
                            }
                            catch (Exception)
                            {

                            }
                        }
                        filePath = string.Concat("/", FtempFolderName, "/", FileName);
                    }
                    var objcusaddendum = _Util.Facade.CustomerFacade.GetCustomerAddendumByCustomerIdAndTicketId(ticketid, customerid);
                    if (objcusaddendum != null)
                    {
                        _Util.Facade.CustomerFacade.DeleteCustomerAddendum(objcusaddendum.Id);
                    }
                    CustomerAddendum model = new CustomerAddendum()
                    {
                        CustomerId = customerid,
                        TicketId = ticketid,
                        Signature = filePath,
                        CreatedBy = new Guid(),
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        IsSigned = true
                    };
                    result = _Util.Facade.CustomerFacade.InsertCustomerAddendum(model) > 0;
                    if (result && customerid != Guid.Empty)
                    {
                        var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid);
                        if (CustomerDetails != null)
                        {
                            CustomerDetails.IsContractSigned = true;
                            _Util.Facade.CustomerFacade.UpdateCustomer(CustomerDetails);
                        }
                    }
                }
            }
            return Json(new { result = result, filePath = filePath });
        }
        [Authorize]
        public ActionResult LoadTicketAgreement(string ticketType, int? id)
        {
            Ticket model = new Ticket();
            if (id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.TicketFacade.GetTicketById(id.Value);
                var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.CustomerId);
                ViewBag.Email = objcus.EmailAddress;
                ViewBag.Signature = model.Signature;
            }
            ViewBag.TicketType = ticketType;

            return View("LoadTicketAgreement", model);
        }
        public JsonResult SubmitCustomerTicketAddendum(Guid ticketid, Guid customerid, string filePath)
        {

            string FilePath = "";
            string FileKey = "";
            bool isUploaded = false;
            string tempFolderName = "";
            string FileName = "";
            var returnurl = "";
            byte[] data;
            decimal _fileSize = 1.00m;

            bool result = false;
            Guid CompanyId = new Guid();
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var AgreementTax = 0.0;
            //if (User.Identity.IsAuthenticated)
            //{
            //    CompanyId = CurrentUser.CompanyId.Value;
            //}
            //else
            //{
            var cusobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid);
            if (cusobj != null)
            {
                CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cusobj.Id);
                CompanyId = custommerCompany.CompanyId;
            }
            else
            {
                return Json(result);
            }
            //}
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid);
            CustomerExtended extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
            if (extended != null)
            {
                extended.ContractStartDate = DateTime.Now.UTCCurrentTime();
                _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
            }
            else
            {
                extended = new CustomerExtended();
                extended.CustomerId = cus.CustomerId;
                extended.ContractStartDate = DateTime.Now.UTCCurrentTime();
                _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
            }
            Ticket ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(ticketid);
            List<CustomerAppointmentEquipment> AppoinmentEqpList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(ticketid);
            List<CustomerAppointmentEquipment> EqpmentList = new List<CustomerAppointmentEquipment>();
            List<CustomerAppointmentEquipment> ServiceList = new List<CustomerAppointmentEquipment>();

            if (AppoinmentEqpList != null && AppoinmentEqpList.Count() > 0)
            {
                EqpmentList = AppoinmentEqpList.Where(x => x.EquipmentClassId != 2 && x.IsEquipmentExist != true && x.IsEquipmentRelease != true).ToList();
                ServiceList = AppoinmentEqpList.Where(x => x.EquipmentClassId == 2).ToList();
            }
            Company com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
            CustomerAddendumModel cusAddendum = new CustomerAddendumModel();
            if (cus != null && ticket != null)
            {
                var objcusaddendum = _Util.Facade.CustomerFacade.GetCustomerAddendumByCustomerIdAndTicketId(ticket.TicketId, cus.CustomerId);
                GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "CompanySignature");

                #region Tax
                //var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(cus.CustomerId, com.CompanyId);
                //if (GetCityTaxList.Count > 0)
                //{
                //    foreach (var item1 in GetCityTaxList)
                //    {
                //        AgreementTax = item1.Rate;
                //    }
                //}
                //else
                //{
                //    Guid CustomerId = new Guid();
                //    if (cus != null)
                //    {
                //        CustomerId = cus.CustomerId;
                //    }
                var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(com.CompanyId, cus.CustomerId);
                if (GetSalesTax != null)
                {
                    AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                }
                //}
                CustomerSignature _cusSign = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByCustomerId(cus.CustomerId);
                if (_cusSign != null)
                {
                    cus.CustomerSignatureDate = _cusSign.CreatedDate;
                }
                if (cus.CustomerSignatureDate == null)
                {
                    Invoice _inv = _Util.Facade.InvoiceFacade.GetInvoiceByCustomerIdAndStatus(cus.CustomerId, LabelHelper.EstimateStatus.Signed);
                    if (_inv != null)
                        cus.CustomerSignatureDate = _inv.SignatureDate.HasValue ? _inv.SignatureDate : new DateTime();
                }
                #endregion

                cusAddendum = new CustomerAddendumModel()
                {
                    CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(com.CompanyId),
                    KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG"),
                    KazarLogoIcon = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo_icon.png"),
                    CompanyPhone = com.Phone,
                    CompanyId = com.CompanyId,
                    CompanyAddress = com.Address,
                    CompanyCity = com.City,
                    CompanyState = com.City,
                    CompanyZip = com.ZipCode,
                    FirstName = cus.FirstName,
                    LastName = cus.LastName,
                    EmailAddress = cus.EmailAddress,
                    CellPhone = cus.CellNo,
                    SitePhone = cus.PrimaryPhone,
                    AgreementSignDate = cus.CustomerSignatureDate.HasValue ? cus.CustomerSignatureDate.Value : new DateTime(),
                    AddendumCreateDate = DateTime.Now,
                    CustomerStreet = cus.Street,
                    CustomerCity = cus.City,
                    CustomerState = cus.State,
                    CustomerZip = cus.ZipCode,
                    InstallAddress = AddressHelper.MakeAddress(cus),
                    Tax = AgreementTax,
                    TicketId = ticket.Id,
                    SalesRepresentative = !string.IsNullOrEmpty(cus.Soldby) ? _Util.Facade.EmployeeFacade.GetEmployeeNumByEmployeeId(new Guid(cus.Soldby)).ToString() : "",
                    WorkToBePerformed = ticket.WorkToBePerformed,
                    RecurringAmount = cus.MonthlyMonitoringFee,
                    ScheduleOn = ticket.CompletionDate,
                    CustomerId = cus.CustomerId,
                    TicketGuidId = ticket.TicketId,
                    CustomerAddendumSignature = objcusaddendum != null ? objcusaddendum.Signature : "",
                    //CustomerAddendumSignatureDate = objcusaddendum != null ? objcusaddendum.CreatedDate : DateTime.Now.UTCCurrentTime()
                };
                if (objcusaddendum != null && !string.IsNullOrWhiteSpace(objcusaddendum.Signature))
                {
                    if (objcusaddendum.CreatedDate != null && objcusaddendum.CreatedDate != new DateTime())
                    {
                        cusAddendum.CustomerAddendumStringSignatureDate = objcusaddendum.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    }
                    if (glbs != null && !string.IsNullOrWhiteSpace(glbs.Value))
                    {
                        cusAddendum.CompanySignature = glbs.Value;
                        if (objcusaddendum.CreatedDate != null && objcusaddendum.CreatedDate != new DateTime())
                        {
                            cusAddendum.CompanySignatureDate = objcusaddendum.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                }
                cusAddendum.ServiceEqpList = ServiceList;
                cusAddendum.EquipmentList = EqpmentList;
                cusAddendum.CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(com.CompanyId);
                string body = _Util.Facade.TicketFacade.MakeCustomerAddendumPdf(cusAddendum);
                ViewBag.BodyContent = body;
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("CreateAddendumPdf")
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };

                #region File Save old
                //byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                //Random rand = new Random();
                //string filename = ConfigurationManager.AppSettings["File.Ticket"];
                //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                //var pdftempFolderName = string.Format(filename, comname) + ticket.Id + "_AddendumDocument.pdf";
                //string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
                //FileHelper.SaveFile(applicationPDFData, Serverfilename);
                #endregion


                #region File Save on AWS S3

                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                tempFolderName = ConfigurationManager.AppSettings["File.Ticket"];


                tempFolderName = string.Format(tempFolderName, comname);
                tempFolderName = tempFolderName.TrimEnd('/');

                FileName = ticket.Id + "_AddendumDocument_Signed.pdf";


                FilePath = tempFolderName;
                FileKey = string.Format($"{FilePath}/{FileName}");


                /// "mayur" used thread for async s3 methods : start

                var task = Task.Run(async () =>
                {
                    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                    await AWSobject.UploadFile(FileKey, applicationPDFData);
                    await AWSobject.MakePublic(FileName, FilePath);
                });

                task.Wait();

                //Thread thread = new Thread(async () => {

                //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                //    await AWSobject.UploadFile(FileKey, applicationPDFData);
                //    await AWSobject.MakePublic(FileName, FilePath);

                //});
                //thread.Start();
                //Thread.Sleep(5000);

                /// "mayur" used thread for async s3 methods : End



                returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
                returnurl = returnurl + FileKey;



                isUploaded = true;

                ViewBag.ReturnUrl = returnurl;
                ViewBag.FileName = FileName;
                ViewBag.FileKey = FileKey;


                _fileSize = (decimal)applicationPDFData.Length / 1024;
                _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);


                #endregion

                CustomerFile cusfile = new CustomerFile()
                {
                    FileDescription = cus.Id + "_" + "Customer_Addendum_Signed.pdf",
                    Filename = "/" + FileKey,
                    FileSize = (double)_fileSize,
                    FileId = Guid.NewGuid(),
                    FileFullName = ticket.Id + "_AddendumDocument.pdf",
                    Uploadeddate = DateTime.Now.UTCCurrentTime(),
                    CustomerId = customerid,
                    CompanyId = CompanyId,
                    IsActive = true,
                    CreatedBy = cusobj.CustomerId,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    UpdatedBy = cusobj.CustomerId,
                    UpdatedDate = DateTime.Now.UTCCurrentTime(),
                    WMStatus = LabelHelper.WatermarkStatus.Pending,
                    AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                };
                result = _Util.Facade.CustomerFileFacade.InsertCustomerFile(cusfile) > 0;

                string Cusname = "";
                if (!String.IsNullOrWhiteSpace(cus.DBA))
                {
                    Cusname = cus.DBA;
                }
                else if (!String.IsNullOrWhiteSpace(cus.BusinessName))
                {
                    Cusname = cus.BusinessName;
                }
                else
                {
                    Cusname = cus.FirstName + ' ' + cus.LastName;
                }

                // base.AddUserActivityForCustomer("Customer Addendum Is Signed", LabelHelper.ActivityAction.AddFile, customerid, null, null);
                UserActivity ua = new UserActivity()
                {
                    ActivityId = Guid.NewGuid(),
                    PageUrl = Request.Url.AbsoluteUri != null ? Request.Url.AbsoluteUri : "",
                    ReferrerUrl = Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : "",
                    // new paramiter
                    Action = "File Signed",
                    StatsDate = DateTime.UtcNow,
                    UserId = cus.CustomerId != null ? cus.CustomerId : Guid.NewGuid(),
                    UserName = Cusname,
                    ActionDisplyText = "Customer Addendum Is Signed",


                    UserAgent = AppConfig.GetUserAgent != null ? AppConfig.GetUserAgent : "",
                    UserIp = AppConfig.GetIP != null ? AppConfig.GetIP : ""
                };
                Guid ActivityID = ua.ActivityId;
                _Util.Facade.UserActivityFacade.InsertUserActivity(ua);
                UserActivityCustomer uac = new UserActivityCustomer()
                {
                    ActivityId = ActivityID != null ? ActivityID : Guid.NewGuid(),

                    CustomerId = cus.CustomerId != null ? cus.CustomerId : Guid.NewGuid(),
                    RefId = cus.Id.ToString(),

                };
                _Util.Facade.UserActivityCustomerFacade.InsertUserActivityCustomer(uac);
            }
            return Json(result);
        }

        public JsonResult SubmitCustomerTicketAddendum_v2(Guid ticketid, Guid customerid, string filePath)
        {
            bool result = false;
            Guid CompanyId = new Guid();
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var AgreementTax = 0.0;
            //if (User.Identity.IsAuthenticated)
            //{
            //    CompanyId = CurrentUser.CompanyId.Value;
            //}
            //else
            //{
            var cusobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid);
            if (cusobj != null)
            {
                CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cusobj.Id);
                CompanyId = custommerCompany.CompanyId;
            }
            else
            {
                return Json(result);
            }
            //}
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid);
            CustomerExtended extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
            if (extended != null)
            {
                extended.ContractStartDate = DateTime.Now.UTCCurrentTime();
                _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
            }
            else
            {
                extended = new CustomerExtended();
                extended.CustomerId = cus.CustomerId;
                extended.ContractStartDate = DateTime.Now.UTCCurrentTime();
                _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
            }
            Ticket ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(ticketid);
            List<CustomerAppointmentEquipment> AppoinmentEqpList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(ticketid);
            List<CustomerAppointmentEquipment> EqpmentList = new List<CustomerAppointmentEquipment>();
            List<CustomerAppointmentEquipment> ServiceList = new List<CustomerAppointmentEquipment>();

            if (AppoinmentEqpList != null && AppoinmentEqpList.Count() > 0)
            {
                EqpmentList = AppoinmentEqpList.Where(x => x.EquipmentClassId != 2 && x.IsEquipmentExist != true && x.IsEquipmentRelease != true).ToList();
                ServiceList = AppoinmentEqpList.Where(x => x.EquipmentClassId == 2).ToList();
            }
            Company com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
            CustomerAddendumModel cusAddendum = new CustomerAddendumModel();
            if (cus != null && ticket != null)
            {
                var objcusaddendum = _Util.Facade.CustomerFacade.GetCustomerAddendumByCustomerIdAndTicketId(ticket.TicketId, cus.CustomerId);
                GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "CompanySignature");

                #region Tax
                //var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(cus.CustomerId, com.CompanyId);
                //if (GetCityTaxList.Count > 0)
                //{
                //    foreach (var item1 in GetCityTaxList)
                //    {
                //        AgreementTax = item1.Rate;
                //    }
                //}
                //else
                //{
                //    Guid CustomerId = new Guid();
                //    if (cus != null)
                //    {
                //        CustomerId = cus.CustomerId;
                //    }
                var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(com.CompanyId, cus.CustomerId);
                if (GetSalesTax != null)
                {
                    AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                }
                //}
                CustomerSignature _cusSign = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByCustomerId(cus.CustomerId);
                if (_cusSign != null)
                {
                    cus.CustomerSignatureDate = _cusSign.CreatedDate;
                }
                if (cus.CustomerSignatureDate == null)
                {
                    Invoice _inv = _Util.Facade.InvoiceFacade.GetInvoiceByCustomerIdAndStatus(cus.CustomerId, LabelHelper.EstimateStatus.Signed);
                    if (_inv != null)
                        cus.CustomerSignatureDate = _inv.SignatureDate.HasValue ? _inv.SignatureDate : new DateTime();
                }
                #endregion

                cusAddendum = new CustomerAddendumModel()
                {
                    CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(com.CompanyId),
                    KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG"),
                    KazarLogoIcon = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo_icon.png"),
                    CompanyPhone = com.Phone,
                    CompanyId = com.CompanyId,
                    CompanyAddress = com.Address,
                    CompanyCity = com.City,
                    CompanyState = com.City,
                    CompanyZip = com.ZipCode,
                    FirstName = cus.FirstName,
                    LastName = cus.LastName,
                    EmailAddress = cus.EmailAddress,
                    CellPhone = cus.CellNo,
                    SitePhone = cus.PrimaryPhone,
                    AgreementSignDate = cus.CustomerSignatureDate.HasValue ? cus.CustomerSignatureDate.Value : new DateTime(),
                    AddendumCreateDate = DateTime.Now,
                    CustomerStreet = cus.Street,
                    CustomerCity = cus.City,
                    CustomerState = cus.State,
                    CustomerZip = cus.ZipCode,
                    InstallAddress = AddressHelper.MakeAddress(cus),
                    Tax = AgreementTax,
                    TicketId = ticket.Id,
                    SalesRepresentative = !string.IsNullOrEmpty(cus.Soldby) ? _Util.Facade.EmployeeFacade.GetEmployeeNumByEmployeeId(new Guid(cus.Soldby)).ToString() : "",
                    WorkToBePerformed = ticket.WorkToBePerformed,
                    RecurringAmount = cus.MonthlyMonitoringFee,
                    ScheduleOn = ticket.CompletionDate,
                    CustomerId = cus.CustomerId,
                    TicketGuidId = ticket.TicketId,
                    CustomerAddendumSignature = objcusaddendum != null ? objcusaddendum.Signature : "",
                    //CustomerAddendumSignatureDate = objcusaddendum != null ? objcusaddendum.CreatedDate : DateTime.Now.UTCCurrentTime()
                };
                if (objcusaddendum != null && !string.IsNullOrWhiteSpace(objcusaddendum.Signature))
                {
                    if (objcusaddendum.CreatedDate != null && objcusaddendum.CreatedDate != new DateTime())
                    {
                        cusAddendum.CustomerAddendumStringSignatureDate = objcusaddendum.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    }
                    if (glbs != null && !string.IsNullOrWhiteSpace(glbs.Value))
                    {
                        cusAddendum.CompanySignature = glbs.Value;
                        if (objcusaddendum.CreatedDate != null && objcusaddendum.CreatedDate != new DateTime())
                        {
                            cusAddendum.CompanySignatureDate = objcusaddendum.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                }
                cusAddendum.ServiceEqpList = ServiceList;
                cusAddendum.EquipmentList = EqpmentList;
                cusAddendum.CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(com.CompanyId);
                string body = _Util.Facade.TicketFacade.MakeCustomerAddendumPdf(cusAddendum);
                ViewBag.BodyContent = body;
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("CreateAddendumPdf")
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };

                #region File Save old
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                Random rand = new Random();
                string filename = ConfigurationManager.AppSettings["File.Ticket"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                var pdftempFolderName = string.Format(filename, comname) + ticket.Id + "_AddendumDocument.pdf";
                string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
                FileHelper.SaveFile(applicationPDFData, Serverfilename);
                #endregion


                CustomerFile cusfile = new CustomerFile()
                {
                    FileDescription = cus.Id + "_" + "Customer_Addendum_Signed.pdf",
                    Filename = "/" + pdftempFolderName,
                    FileId = Guid.NewGuid(),
                    FileFullName = ticket.Id + "_AddendumDocument.pdf",
                    Uploadeddate = DateTime.Now.UTCCurrentTime(),
                    CustomerId = customerid,
                    CompanyId = CompanyId,
                    IsActive = true,
                    CreatedBy = cusobj.CustomerId,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    UpdatedBy = cusobj.CustomerId,
                    UpdatedDate = DateTime.Now.UTCCurrentTime(),
                    WMStatus = LabelHelper.WatermarkStatus.Pending,
                    AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                };
                result = _Util.Facade.CustomerFileFacade.InsertCustomerFile(cusfile) > 0;

                string Cusname = "";
                if (!String.IsNullOrWhiteSpace(cus.DBA))
                {
                    Cusname = cus.DBA;
                }
                else if (!String.IsNullOrWhiteSpace(cus.BusinessName))
                {
                    Cusname = cus.BusinessName;
                }
                else
                {
                    Cusname = cus.FirstName + ' ' + cus.LastName;
                }

                // base.AddUserActivityForCustomer("Customer Addendum Is Signed", LabelHelper.ActivityAction.AddFile, customerid, null, null);
                UserActivity ua = new UserActivity()
                {
                    ActivityId = Guid.NewGuid(),
                    PageUrl = Request.Url.AbsoluteUri != null ? Request.Url.AbsoluteUri : "",
                    ReferrerUrl = Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : "",
                    // new paramiter
                    Action = "File Signed",
                    StatsDate = DateTime.UtcNow,
                    UserId = cus.CustomerId != null ? cus.CustomerId : Guid.NewGuid(),
                    UserName = Cusname,
                    ActionDisplyText = "Customer Addendum Is Signed",


                    UserAgent = AppConfig.GetUserAgent != null ? AppConfig.GetUserAgent : "",
                    UserIp = AppConfig.GetIP != null ? AppConfig.GetIP : ""
                };
                Guid ActivityID = ua.ActivityId;
                _Util.Facade.UserActivityFacade.InsertUserActivity(ua);
                UserActivityCustomer uac = new UserActivityCustomer()
                {
                    ActivityId = ActivityID != null ? ActivityID : Guid.NewGuid(),

                    CustomerId = cus.CustomerId != null ? cus.CustomerId : Guid.NewGuid(),
                    RefId = cus.Id.ToString(),

                };
                _Util.Facade.UserActivityCustomerFacade.InsertUserActivityCustomer(uac);
            }
            return Json(result);
        }

        #region Add Commission
        public ActionResult AddSalesCommission(Guid TicketId, Guid CustomerId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            #region Viewbag
            List<SelectListItem> SalesPersons = new List<SelectListItem>();
            SalesPersons.Add(new SelectListItem()
            {
                Text = "Select Sales Person",
                Value = "-1"
            });
            List<Employee> EmployeeDropDown = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentLoggedInUser.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid(), LabelHelper.UserTags.Partner);
            if (EmployeeDropDown != null && EmployeeDropDown.Count > 0)
            {
                SalesPersons.AddRange(EmployeeDropDown.OrderBy(x => x.FirstName).Select(x => new SelectListItem()
                {
                    Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                    Value = x.UserId.ToString()
                }).ToList());
            }
            ViewBag.SalesPersonList = SalesPersons;
            #endregion

            SalesCommission salesCommission = new SalesCommission();
            salesCommission.TicketId = TicketId;
            salesCommission.CustomerId = CustomerId;
            return PartialView("_AddSalesCommission", salesCommission);
        }
        public JsonResult SaveSalesCommission(SalesCommission salesCommission)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            bool result = false;
            if (salesCommission != null)
            {
                if (salesCommission.RMRCommission == null)
                {
                    salesCommission.RMRCommission = 0;
                }
                if (salesCommission.Adjustment == null)
                {
                    salesCommission.Adjustment = 0;
                }
                if (salesCommission.EquipmentCommission == null)
                {
                    salesCommission.EquipmentCommission = 0;
                }
                salesCommission.SalesCommissionId = Guid.NewGuid();
                salesCommission.CompletionDate = DateTime.Now.UTCCurrentTime();
                salesCommission.TotalCommission = salesCommission.RMRCommission + salesCommission.Adjustment;
                salesCommission.IsPaid = false;
                salesCommission.CreatedBy = CurrentLoggedInUser.UserId;
                salesCommission.CreatedDate = DateTime.Now.UTCCurrentTime();
                result = _Util.Facade.TicketFacade.InsertSalesCommission(salesCommission);
            }
            return Json(new { result = result });
        }

        public ActionResult AddTechCommission(Guid TicketId, Guid CustomerId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            #region Viewbag
            List<SelectListItem> Technician = new List<SelectListItem>();
            Technician.Add(new SelectListItem()
            {
                Text = "Select Tech",
                Value = "-1"
            });
            var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentLoggedInUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid());
            if (TechnicianList != null && TechnicianList.Count > 0)
            {
                Technician.AddRange(TechnicianList.OrderBy(x => x.FirstName).Select(x => new SelectListItem()
                {
                    Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                    Value = x.UserId.ToString()
                }).ToList());
            }
            ViewBag.TechnicianList = Technician;
            #endregion

            TechCommission techCommission = new TechCommission();
            techCommission.TicketId = TicketId;
            techCommission.CustomerId = CustomerId;
            return PartialView("_AddTechCommission", techCommission);
        }
        public JsonResult SaveTechCommission(TechCommission techCommission)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            bool result = false;
            if (techCommission != null)
            {
                if (techCommission.BaseRMRCommission == null)
                {
                    techCommission.BaseRMRCommission = 0;
                }
                //if (techCommission.AddedRMRCommission == null)
                //{
                //    techCommission.AddedRMRCommission = 0;
                //}
                techCommission.TechCommissionId = Guid.NewGuid();
                techCommission.CompletionDate = DateTime.Now.UTCCurrentTime();
                techCommission.TotalCommission = techCommission.BaseRMRCommission /*+ techCommission.AddedRMRCommission*/;
                techCommission.IsPaid = false;
                techCommission.CreatedBy = CurrentLoggedInUser.UserId;
                techCommission.CreatedDate = DateTime.Now.UTCCurrentTime();
                result = _Util.Facade.TicketFacade.InsertTechCommission(techCommission);
            }
            return Json(new { result = result });
        }

        public ActionResult AddAddMemberCommission(Guid TicketId, Guid CustomerId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            #region Viewbag
            List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value).OrderBy(x => x.FirstName).ToList();
            ViewBag.EmployeeList = EmpList.Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString()
                }).ToList();
            #endregion

            AddMemberCommission addMemberCommission = new AddMemberCommission();
            addMemberCommission.TicketId = TicketId;
            addMemberCommission.CustomerId = CustomerId;
            return PartialView("_AddAddMemberCommission", addMemberCommission);
        }
        public JsonResult SaveAddMemberCommission(AddMemberCommission addMemberCommission)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            bool result = false;
            if (addMemberCommission != null)
            {
                addMemberCommission.AddMemberCommissionId = Guid.NewGuid();
                addMemberCommission.CompletionDate = DateTime.Now.UTCCurrentTime();
                addMemberCommission.IsPaid = false;
                addMemberCommission.CreatedBy = CurrentLoggedInUser.UserId;
                addMemberCommission.CreatedDate = DateTime.Now.UTCCurrentTime();
                addMemberCommission.IsManual = true;
                result = _Util.Facade.TicketFacade.InsertAddMemberCommission(addMemberCommission);
            }
            return Json(new { result = result });
        }

        public ActionResult AddFinRepCommission(Guid TicketId, Guid CustomerId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            #region Viewbag
            List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value).OrderBy(x => x.FirstName).ToList();
            ViewBag.EmployeeList = EmpList.Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString()
                }).ToList();
            #endregion

            FinRepCommission finRepCommission = new FinRepCommission();
            finRepCommission.TicketId = TicketId;
            finRepCommission.CustomerId = CustomerId;
            return PartialView("_AddFinRepCommission", finRepCommission);
        }
        public JsonResult SaveFinRepCommission(FinRepCommission finRepCommission)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            bool result = false;
            if (finRepCommission != null)
            {
                finRepCommission.FinRepCommissionId = Guid.NewGuid();
                finRepCommission.CompletionDate = DateTime.Now.UTCCurrentTime();
                finRepCommission.IsPaid = false;
                finRepCommission.CreatedBy = CurrentLoggedInUser.UserId;
                finRepCommission.CreatedDate = DateTime.Now.UTCCurrentTime();
                finRepCommission.IsManual = true;
                result = _Util.Facade.TicketFacade.InsertFinRepCommission(finRepCommission);
            }
            return Json(new { result = result });
        }

        public ActionResult AddFollowUpCommission(Guid TicketId, Guid CustomerId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            #region Viewbag
            List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value).OrderBy(x => x.FirstName).ToList();
            ViewBag.EmployeeList = EmpList.Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString()
                }).ToList();
            #endregion

            FollowUpCommission followUpCommission = new FollowUpCommission();
            followUpCommission.TicketId = TicketId;
            followUpCommission.CustomerId = CustomerId;
            return PartialView("_AddFollowUpCommission", followUpCommission);
        }
        public JsonResult SaveFollowUpCommission(FollowUpCommission followUpCommission)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            bool result = false;
            if (followUpCommission != null)
            {
                followUpCommission.FollowUpCommissionId = Guid.NewGuid();
                followUpCommission.CompletionDate = DateTime.Now.UTCCurrentTime();
                followUpCommission.IsPaid = false;
                followUpCommission.CreatedBy = CurrentLoggedInUser.UserId;
                followUpCommission.CreatedDate = DateTime.Now.UTCCurrentTime();
                followUpCommission.IsManual = true;
                result = _Util.Facade.TicketFacade.InsertFollowUpCommission(followUpCommission);
            }
            return Json(new { result = result });
        }

        public ActionResult AddServiceCallCommission(Guid TicketId, Guid CustomerId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            #region Viewbag
            List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value).OrderBy(x => x.FirstName).ToList();
            ViewBag.EmployeeList = EmpList.Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString()
                }).ToList();
            #endregion

            ServiceCallCommission serviceCallCommission = new ServiceCallCommission();
            serviceCallCommission.TicketId = TicketId;
            serviceCallCommission.CustomerId = CustomerId;
            return PartialView("_AddServiceCallCommission", serviceCallCommission);
        }
        public JsonResult SaveServiceCallCommission(ServiceCallCommission serviceCallCommission)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            bool result = false;
            if (serviceCallCommission != null)
            {
                serviceCallCommission.ServiceCallCommissionId = Guid.NewGuid();
                serviceCallCommission.CompletionDate = DateTime.Now.UTCCurrentTime();
                serviceCallCommission.IsPaid = false;
                serviceCallCommission.CreatedBy = CurrentLoggedInUser.UserId;
                serviceCallCommission.CreatedDate = DateTime.Now.UTCCurrentTime();
                serviceCallCommission.IsManual = true;
                result = _Util.Facade.TicketFacade.InsertServiceCallCommission(serviceCallCommission);
            }
            return Json(new { result = result });
        }

        public ActionResult AddRescheduleCommission(Guid TicketId, Guid CustomerId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            #region Viewbag
            List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value).OrderBy(x => x.FirstName).ToList();
            ViewBag.EmployeeList = EmpList.Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString()
                }).ToList();
            #endregion

            RescheduleCommission rescheduleCommission = new RescheduleCommission();
            rescheduleCommission.TicketId = TicketId;
            rescheduleCommission.CustomerId = CustomerId;
            return PartialView("_AddRescheduleCommission", rescheduleCommission);
        }
        public JsonResult SaveRescheduleCommission(RescheduleCommission rescheduleCommission)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            bool result = false;
            if (rescheduleCommission != null)
            {
                rescheduleCommission.RescheduleCommissionId = Guid.NewGuid();
                rescheduleCommission.CompletionDate = DateTime.Now.UTCCurrentTime();
                rescheduleCommission.IsPaid = false;
                rescheduleCommission.CreatedBy = CurrentLoggedInUser.UserId;
                rescheduleCommission.CreatedDate = DateTime.Now.UTCCurrentTime();
                rescheduleCommission.IsManual = true;
                result = _Util.Facade.TicketFacade.InsertRescheduleCommission(rescheduleCommission);
            }
            return Json(new { result = result });
        }
        #endregion

        public ActionResult OpenPopUpAssignedQty(int count)
        {
            ViewBag.count = count;
            return View();
        }

        public ActionResult RugConditionPopup(int DataId)
        {
            TicketBookingDetails TBD = _Util.Facade.BookingFacade.GetTicketBookingDetailsById(DataId);
            if (TBD == null)
            {
                return null;
            }
            RugCondtionsModel rugCondtionsModel = new RugCondtionsModel();
            rugCondtionsModel.RugCondtitions = _Util.Facade.LookupFacade.GetLookupByKey("RugCondition").Where(x => x.DataValue != "-1").OrderBy(x => x.DataOrder).ToList().Select(x =>
                  new SelectListItem()
                  {
                      Text = x.DisplayText.ToString(),
                      Value = x.DataValue.ToString(),
                      Selected = TBD.RugConditions.IndexOf(x.DataValue) > -1
                  }).ToList();
            rugCondtionsModel.RugImages = _Util.Facade.TicketFacade.GetTicketFilesByDetailsId(TBD.Id);
            rugCondtionsModel.Comment = TBD.Comments;
            rugCondtionsModel.DetailId = DataId;

            return View(rugCondtionsModel);
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveRugCondition(List<string> Conditions, List<RugFile> Files, string Comments, int TicketBookingDetailId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string Codition = "";
            if (Conditions != null && Conditions.Count > 0)
            {
                Codition = string.Join(",", Conditions);
            }
            if (TicketBookingDetailId > 0)
            {
                TicketBookingDetails TBD = _Util.Facade.BookingFacade.GetTicketBookingDetailsById(TicketBookingDetailId);
                if (TBD != null)
                {
                    TBD.RugConditions = Codition;
                    TBD.Comments = Comments;
                    _Util.Facade.BookingFacade.UpdateTicketBookingDetails(TBD);
                    //return Json(new { result = true, message = "Saved successfully." });
                }

                _Util.Facade.TicketFacade.DeleteTicketFilesByBookingDetailsId(TBD.Id);
                if (Files != null && Files.Count > 0)
                {
                    foreach (var item in Files)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Location))
                        {
                            TicketFile tf = new TicketFile()
                            {
                                FileLocation = item.Location,
                                Description = item.Description,
                                FileAddedBy = CurrentUser.UserId,
                                FileAddedDate = DateTime.Now.UTCCurrentTime(),
                                FileName = "",
                                Filesize = 0,
                                TicketBookingDetailsId = TicketBookingDetailId,
                                TicketId = Guid.Empty,

                            };
                            _Util.Facade.TicketFacade.InsertTicketFile(tf);
                        }
                    }
                }
            }
            return Json(new { result = false, message = "Error." });
        }

        [HttpPost]
        public JsonResult SaveTicketPayment(TicketPayment TicketPayment)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            var objtikpay = _Util.Facade.TicketFacade.GetTicketPaymentByTicketId(TicketPayment.TicketId);
            if (objtikpay != null)
            {
                _Util.Facade.TicketFacade.DeleteTicketPaymentByTicketId(objtikpay.Id);
            }
            TicketPayment.CreatedBy = CurrentUser.UserId;
            TicketPayment.CreatedDate = DateTime.Now.UTCCurrentTime();
            TicketPayment.IsPaid = TicketPayment.IsPaid;
            result = _Util.Facade.TicketFacade.InsertTicketPayment(TicketPayment) > 0;

            #region Booking Invoice
            Ticket ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketPayment.TicketId);
            if (ticket != null && !string.IsNullOrWhiteSpace(ticket.BookingId))
            {
                Invoice invoice = _Util.Facade.InvoiceFacade.GetOpenInvoicebyBookingId(ticket.BookingId);
                if (invoice != null && invoice.BalanceDue > 0)
                {
                    string TempStatus = invoice.Status;

                    Transaction transaction = new Transaction()
                    {
                        AddedBy = User.Identity.Name,
                        AddedDate = DateTime.Now.UTCCurrentTime(),
                        Amount = invoice.BalanceDue.Value,
                        CreatedBy = CurrentUser.UserId,
                        PaymentInfoId = 0,
                        PaymentMethod = TicketPayment.PaymentMethod,
                        Type = "Payment",
                        Status = "Closed",
                        TransacationDate = DateTime.Now.UTCCurrentTime(),
                        CardTransactionId = TicketPayment.ConfirmationNo,
                        CheckNo = "",
                        ReferenceNo = invoice.InvoiceId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = invoice.CustomerId,


                    };

                    transaction.Id = _Util.Facade.TransactionFacade.InsertTransaction(transaction);
                    TransactionHistory transactionHistory = new TransactionHistory()
                    {
                        TransactionId = transaction.Id,
                        InvoiceId = invoice.Id,
                        Amout = transaction.Amount,
                        Balance = transaction.Amount,
                        ReceivedBy = CurrentUser.UserId
                    };

                    transactionHistory.Id = _Util.Facade.TransactionFacade.InsertTransactionHistory(transactionHistory);
                    invoice.Status = LabelHelper.InvoiceStatus.Paid;
                    invoice.BalanceDue = 0;
                    invoice.LastUpdatedByUid = CurrentUser.UserId;
                    invoice.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    _Util.Facade.InvoiceFacade.UpdateInvoice(invoice);

                    if (invoice != null && TempStatus != invoice.Status)
                    {
                        #region log
                        int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                        bool newBool = invoice.IsARBInvoice ?? false;


                        base.AddUserActivityForCustomer("Invoice Status Changed from " + TempStatus + " To " + invoice.Status + "#InvoiceId: " + invoice.InvoiceId, lineNumber + "," + actionName + "/" + controllerName, invoice.CustomerId, null, null, newBool);
                        #endregion
                    }

                }

            }
            #endregion


            return Json(new { result = result });
        }

        private bool CustomerSalesDateUpdate(Guid customerid)
        {
            if (customerid != new Guid())
            {
                var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid);
                if (objcus != null)
                {
                    var objticket = _Util.Facade.TicketFacade.GetTicketByCustomerIdAndIsAgreementTicket(customerid);
                    var objinvoice = _Util.Facade.InvoiceFacade.GetInvoiceByCustomerIdAndInstallationType(customerid, "Service");
                    if (objinvoice != null && objticket != null && objinvoice.Status == LabelHelper.InvoiceStatus.Paid && objcus.IsAgreementSend.HasValue && objcus.IsAgreementSend.Value)
                    {
                        var objticketuser = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndIsPrimary(new Guid(), objticket.TicketId);
                        if (objticketuser != null && objticketuser.UserId.ToString() != "22222222-2222-2222-2222-222222222222")
                        {
                            if (!objcus.SalesDate.HasValue)
                            {
                                objcus.SalesDate = DateTime.Now.UTCCurrentTime();
                                _Util.Facade.CustomerFacade.UpdateCustomer(objcus);
                            }
                        }
                    }
                }
            }
            return true;
        }

        public ActionResult GetCodeSafetyPopup(int id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult CreateCodeDocument(int? ticketid)
        {
            Ticket Model = new Ticket();
            if (ticketid.HasValue && ticketid.Value > 0)
            {
                Model = _Util.Facade.TicketFacade.GetTicketById(ticketid.Value);
            }
            string body = _Util.Facade.AgreementFacade.MakeCodeSafetyDocument(Model);
            ViewBag.Body = body;
            return new ViewAsPdf()
            {
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Orientation.Portrait,
                PageMargins = { Left = 17, Right = 17, Top = 15, Bottom = 17 },
            };
        }

        public ActionResult CreateRUGTicketAgreement()
        {
            return View();
        }
        public ActionResult RUGTicketAgreementPdf()
        {
            return View();
        }

        public ActionResult LoadMyTickets()
        {
            return View();
        }

        public ActionResult LoadMyTicketsList(int pageno, int pagesize)
        {
            TicketListModel model = new TicketListModel();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            model = _Util.Facade.TicketFacade.GetAllAssignedTicketListByUserId(CurrentUser.UserId, pageno, pagesize);
            ViewBag.PageNumber = pageno;
            ViewBag.OutOfNumber = 0;

            if (model.Tickets.Count() > 0)
            {
                ViewBag.OutOfNumber = model.TotalCount;
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
            return View(model);
        }

        public bool SendEmailToAssignUser(int? ticketid)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (ticketid.HasValue && ticketid.Value > 0)
            {
                var Ticket = _Util.Facade.TicketFacade.GetTicketById(ticketid.Value);
                if (Ticket != null)
                {
                    var TicketUser = _Util.Facade.TicketFacade.GetTicketUserByUserIdAndTicketIdForMail(Ticket.TicketId);
                    var objcustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Ticket.CustomerId);
                    if (objcustomer != null && TicketUser != null)
                    {
                        var User = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(TicketUser.UserId);
                        if (User != null)
                        {
                            SendEmailTicketAssignModel SendEmailTicketAssignModel = new SendEmailTicketAssignModel();
                            SendEmailTicketAssignModel.Name = User.FirstName + " " + User.LastName;
                            SendEmailTicketAssignModel.ToEmail = User.Email;
                            SendEmailTicketAssignModel.Subject = "Ticket Assign Email";
                            SendEmailTicketAssignModel.Body = "You are a ticket assign user for #" + Ticket.Id + " from " + (!string.IsNullOrWhiteSpace(objcustomer.BusinessName) ? objcustomer.BusinessName : objcustomer.FirstName + " " + objcustomer.LastName);
                            result = _Util.Facade.MailFacade.SendEmailToTicketAssignUser(SendEmailTicketAssignModel, CurrentUser.CompanyId.Value);
                        }
                    }
                }
            }
            return result;
        }

        public ActionResult GetPopUpTicketSurvey(Guid? cusid, Guid ticketid)
        {
            Customer model = new Customer();
            if (cusid.HasValue && cusid.Value != new Guid())
            {
                model = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(cusid.Value);
                model.Ticket = _Util.Facade.TicketFacade.GetTicketByTicketIdAndCustomerId(ticketid, cusid.Value);
            }
            return View(model);
        }

        public ActionResult DiffrentAddressPopup(Guid CustomerId)
        {
            List<CustomerAddress> addressList = new List<CustomerAddress>();
            addressList = _Util.Facade.CustomerFacade.GetDiffrentAddressByCustomerId(CustomerId);
            return View(addressList);
        }

        public ActionResult SendAgreementPopup(Guid CustomerId, int TicketId, string from)
        {
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            CustomerExtended cusExt = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(CustomerId);
            PackageCustomer pc = new PackageCustomer();
            pc = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerId(cus.CustomerId);
            SendAgreementPopup agreementPopup = new SendAgreementPopup();
            agreementPopup.ContractType = cusExt.ContractType;
            agreementPopup.CustomerId = cus.Id;
            agreementPopup.TicketId = TicketId;
            agreementPopup.ContractTerm = cus.ContractTeam;
            agreementPopup.VerbalPassword = cus.Passcode;
            agreementPopup.RenewalTerm = cus.RenewalTerm != null ? cus.RenewalTerm.Value : 0;
            agreementPopup.OrginalContractDate = cus.OriginalContactDate;
            if (pc != null)
            {
                agreementPopup.ActivationFee = pc.ActivationFee;
                agreementPopup.NonConfirmingFee = pc.NonConformingFee;
            }
            List<SelectListItem> ContactTerms = new List<SelectListItem>();
            ContactTerms.AddRange(_Util.Facade.LookupFacade.GetDropdownsByKey("ContractTerm"));
            ViewBag.ContractTermList = ContactTerms;
            ViewBag.From = from;
            ViewBag.ContractType = _Util.Facade.LookupFacade.GetDropdownsByKey("ContractTypeSummary");
            return View(agreementPopup);
        }
        public JsonResult UpdateAgreementInfo(SendAgreementPopup model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            string message = "";
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerById(model.CustomerId);
            cus.ContractTeam = model.ContractTerm;
            cus.Passcode = model.VerbalPassword;
            cus.ActivationFee = model.ActivationFee;
            cus.RenewalTerm = model.RenewalTerm;
            //cus.OriginalContactDate = model.OrginalContractDate;
            DateTime? OG_ContractDate = cus.OriginalContactDate;
            _Util.Facade.CustomerFacade.UpdateCustomer(cus);

            if (cus != null)
            {
                var cusExt = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                if (cusExt != null)
                {
                    cusExt.ContractType = model.ContractType;
                    OG_ContractDate = OG_ContractDate ?? cusExt.ContractStartDate;
                    cusExt.ContractStartDate = model.OrginalContractDate ?? DateTime.Now;
                    _Util.Facade.CustomerFacade.UpdateCustomerExtended(cusExt);
                }
            }

            PackageCustomer pc = new PackageCustomer();
            pc = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerId(cus.CustomerId);
            if (pc != null)
            {
                pc.ActivationFee = model.ActivationFee;
                pc.NonConformingFee = model.NonConfirmingFee;
                pc.NonConforming = true;
                _Util.Facade.PackageFacade.UpdatePackageCustomer(pc);
                result = true;
            }
            else
            {
                pc = new PackageCustomer()
                {
                    CustomerId = cus.CustomerId,
                    CompanyId = CurrentUser.CompanyId.Value,
                    PackageId = new Guid(),
                    NonConformingFee = model.NonConfirmingFee,
                    ActivationFee = model.ActivationFee,
                    MinCredit = 0,
                    MaxCredit = 0,
                    NonConforming = true
                };
                _Util.Facade.PackageFacade.InsertPackageCustomer(pc);
                result = true;
            }

            return Json(new { result = result, message = message });
        }
        public ActionResult TicketEmailSetupPartial()
        {
            return View();
        }
        [Authorize]
        public ActionResult ShowTicketEmailList()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<TicketNotificationEmail> model = new List<TicketNotificationEmail>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.TicketFacade.GetAllTicketNotificationEmailList();
            }
            return View(model);
        }
        [Authorize]
        public ActionResult AddTicketNotificationEmail(int? id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            TicketNotificationEmail model = new TicketNotificationEmail();
            List<SelectListItem> statusList = _Util.Facade.LookupFacade.GetLookupByKey("TicketStatus").Where(x => x.DataValue != "-1").OrderBy(x => x.DataValue.ToString() != "-1").ThenBy(x => x.DisplayText).ToList().Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();
            ViewBag.TicketStatusList = statusList;
            if (id.HasValue && id > 0)
            {
                model = _Util.Facade.TicketFacade.GetTicketNotificationEmailbyId(id.Value);
            }
            return View(model);
        }


        [Authorize]
        [HttpPost]
        public JsonResult AddTicketEmailNotification(TicketNotificationEmail emailNofification)
        {
            var result = false;
            var message = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            try
            {
                if (emailNofification.Id > 0)
                {
                    //var SurveyName = _Util.Facade.CustomSurveyFacade.GetCustomSurveyById(customSurvey.Id);

                    TicketNotificationEmail notificationEmail = _Util.Facade.TicketFacade.GetTicketNotificationEmailbyId(emailNofification.Id);

                    notificationEmail.Email = emailNofification.Email;
                    notificationEmail.TicketStatus = emailNofification.TicketStatus;
                    _Util.Facade.TicketFacade.UpdateTicketNotificationEmail(emailNofification);
                    message = "Ticket notification eamil updated successfully.";
                    result = true;
                }
                else
                {


                    emailNofification.Email = emailNofification.Email;
                    emailNofification.TicketStatus = emailNofification.TicketStatus;
                    emailNofification.CreatedBy = currentLoggedIn.UserId;
                    emailNofification.CreatedDate = DateTime.Now.UTCCurrentTime();


                    _Util.Facade.TicketFacade.InsertTicketNotificationEmail(emailNofification);
                    message = "Ticket notification eamil saved successfully.";
                    result = true;
                }

            }
            catch (Exception ex)
            {
                message = "Ticket notification eamil not saved";
                result = true;
            }
            return Json(new { result = result, message = message });
        }

        [HttpPost]
        public JsonResult DeleteTicketNotification(int? Id)
        {
            bool result = false;
            if (Id.HasValue && Id.Value > 0)
            {
                var objemail = _Util.Facade.TicketFacade.GetTicketNotificationEmailbyId(Id.Value);
                if (objemail != null)
                {
                    result = _Util.Facade.TicketFacade.DeleteTicketNotificationEmail(objemail.Id);
                }
            }
            return Json(result);
        }
        public ActionResult NotificationSendPopUp(int TicketId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.TicketId = TicketId;
            ViewBag.ShowAdditionalMemberList = "false";
            ViewBag.NotifyingMembers = "false";
            Ticket tk = _Util.Facade.TicketFacade.GetTicketById(TicketId);
            List<TicketUser> AdditionalMemberList = _Util.Facade.TicketFacade.GetOnlyTicketAddtionalUsersByTicketId(tk.TicketId);
            if (AdditionalMemberList.Count() > 0)
            {
                ViewBag.ShowAdditionalMemberList = "true";
            }
            List<TicketUser> AssignedTo = _Util.Facade.TicketFacade.GetAllNotificationTicketUserListByTicketId(tk.TicketId);
            if (AssignedTo.Count() > 0)
            {
                ViewBag.NotifyingMembers = "true";
            }
            return View();
        }
        public JsonResult NotificationSendToSelectedUser(int TicketId, bool SendToCustomer, bool SendToTech, bool SendToAdditionalMembers, bool SendToNotifyingMembers)
        {
            bool result = false;
            string recevierlist = "";
            Ticket tk = _Util.Facade.TicketFacade.GetTicketById(TicketId);
            if (tk != null)
            {
                Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(tk.CustomerId);
                if (SendToCustomer)
                {
                    List<string> Email = new List<string>();
                    if (cus.EmailAddress != null)
                    {
                        Email.Add(cus.EmailAddress);
                        result = SendNotificationEmailToUser(Email, TicketId);
                    }
                    if (string.IsNullOrWhiteSpace(recevierlist))
                    {
                        recevierlist += cus.FirstName + ' ' + cus.LastName;
                    }
                    else
                    {
                        recevierlist += ", " + cus.FirstName + ' ' + cus.LastName;
                    }
                }
                if (SendToTech)
                {
                    List<string> TechEmail = new List<string>();
                    TicketUser AssignedTo = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndPrimary(tk.TicketId);
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(AssignedTo.UserId);
                    if (emp.Email != null)
                    {
                        TechEmail.Add(emp.Email);
                        result = SendNotificationEmailToUser(TechEmail, TicketId);
                    }
                    if (string.IsNullOrWhiteSpace(recevierlist))
                    {
                        recevierlist += emp.FirstName + ' ' + emp.LastName;
                    }
                    else
                    {
                        recevierlist += ", " + emp.FirstName + ' ' + emp.LastName;
                    }
                }
                if (SendToAdditionalMembers)
                {
                    List<string> AdditionalMembers = new List<string>();
                    List<TicketUser> AdditionalMemberList = _Util.Facade.TicketFacade.GetOnlyTicketAddtionalUsersByTicketId(tk.TicketId);
                    if (AdditionalMemberList.Count() > 0)
                    {
                        foreach (var item in AdditionalMemberList)
                        {
                            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(item.UserId);
                            if (emp.Email != null)
                            {
                                AdditionalMembers.Add(emp.Email);
                            }
                            if (string.IsNullOrWhiteSpace(recevierlist))
                            {
                                recevierlist += emp.FirstName + ' ' + emp.LastName;
                            }
                            else
                            {
                                recevierlist += ", " + emp.FirstName + ' ' + emp.LastName;
                            }
                        }
                    }
                    result = SendNotificationEmailToUser(AdditionalMembers, TicketId);
                }
                if (SendToNotifyingMembers)
                {
                    List<string> NotifyingMembers = new List<string>();
                    List<TicketUser> AssignedTo = _Util.Facade.TicketFacade.GetAllNotificationTicketUserListByTicketId(tk.TicketId);
                    if (AssignedTo.Count() > 0)
                    {
                        foreach (var item in AssignedTo)
                        {
                            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(item.UserId);
                            if (emp.Email != null)
                            {
                                NotifyingMembers.Add(emp.Email);
                            }
                            if (string.IsNullOrWhiteSpace(recevierlist))
                            {
                                recevierlist += emp.FirstName + ' ' + emp.LastName;
                            }
                            else
                            {
                                recevierlist += ", " + emp.FirstName + ' ' + emp.LastName;
                            }
                        }
                    }
                    result = SendNotificationEmailToUser(NotifyingMembers, TicketId);
                }
                #region Log
                if (!string.IsNullOrWhiteSpace(recevierlist))
                {
                    string logMessage = "A Notification Has Been Sent To " + recevierlist + " For Appointment #" + TicketId + ".";
                    base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.TicketNotification, cus.CustomerId, cus.Id, null);
                }
                #endregion
            }
            return Json(new { result = result, message = "Failed" });
        }
        public bool SendNotificationEmailToUser(List<string> Email, int TicketId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(currentLoggedIn.CompanyId.Value);

            if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                //return RedirectToAction("Index", "Login");
            }
            else
            {
                Guid CompanyId = CC.CompanyId;
                string ConnectionString = CC.ConnectionString;
                if (!string.IsNullOrWhiteSpace(ConnectionString))
                {
                    ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);

                    Session[SessionKeys.CompanyConnectionString] = ConnectionString;
                }
                else
                {
                    //return RedirectToAction("Index", "Login");
                }
            }

            bool result = false;
            Ticket tk = _Util.Facade.TicketFacade.GetTicketById(TicketId);
            string AppointmentType = string.Empty;
            Lookup ticketType = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyAndDataValue("TicketType", tk.TicketType);
            if (!string.IsNullOrWhiteSpace(ticketType.DisplayText))
            {
                AppointmentType = string.Format(EmailTemplateHelper.AppointmentType, ticketType.DisplayText);
            }

            string AssignedToName = string.Empty;
            TicketUser AssignedTo = _Util.Facade.TicketFacade.GetTicketTechByTicketId(tk.TicketId);
            if (!string.IsNullOrWhiteSpace(AssignedTo.EMPNAME))
            {
                AssignedToName = string.Format(EmailTemplateHelper.AssignedTo, AssignedTo.EMPNAME);
            }

            string AdditionalMember = string.Empty;
            string AdditionalMemberName = string.Empty;
            List<AdditionalMember> AdditionalMemberList = _Util.Facade.TicketFacade.GetAllTicketAdditionalMembersByTicketId(tk.TicketId);
            if (AdditionalMemberList.Count() > 0)
            {
                foreach (var item in AdditionalMemberList)
                {
                    if (!string.IsNullOrWhiteSpace(AdditionalMemberName))
                    {
                        AdditionalMemberName += ", " + item.FullName;
                    }
                    else
                    {
                        AdditionalMemberName += item.FullName;
                    }
                }
                AdditionalMember = string.Format(EmailTemplateHelper.AdditionalMember, AdditionalMemberName);
            }

            string ScheduleDate = string.Empty;
            string ScheduleStartDate = string.Empty;
            string ScheduleEndDate = string.Empty;
            CustomerAppointment cusapp = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentByAppIdCusId(tk.TicketId, tk.CustomerId);
            if (cusapp != null)
            {
                if (cusapp.AppointmentDate.HasValue)
                {
                    ScheduleDate = string.Format(EmailTemplateHelper.ScheduleDate, cusapp.AppointmentDate.Value.ToString("M/d/yy"));
                }
                if (!string.IsNullOrWhiteSpace(cusapp.AppointmentStartTime) && cusapp.AppointmentStartTime != "-1")
                {
                    ScheduleStartDate = string.Format(EmailTemplateHelper.ScheduleStartDate, cusapp.AppointmentStartTimeVal);
                }
                if (!string.IsNullOrWhiteSpace(cusapp.AppointmentEndTime) && cusapp.AppointmentEndTime != "-1")
                {
                    ScheduleEndDate = string.Format(EmailTemplateHelper.AppointmentEndTime, cusapp.AppointmentEndTimeVal);
                }
            }

            string CustomerName = string.Empty;
            string Addresstr = string.Empty;
            string add = string.Empty;
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(tk.CustomerId);
            if (!string.IsNullOrWhiteSpace(cus.BusinessName))
            {
                CustomerName = string.Format(EmailTemplateHelper.CustomerName, cus.BusinessName);
            }
            else
            {
                CustomerName = string.Format(EmailTemplateHelper.CustomerName, cus.FirstName + ' ' + cus.LastName);
            }

            if (string.IsNullOrWhiteSpace(cus.City) || string.IsNullOrWhiteSpace(cus.State) || string.IsNullOrWhiteSpace(cus.ZipCode))
            {
                add = cus.Address;
            }
            else
            {
                add = cus.Street + "<br/>" + cus.City + ", " + cus.State + ", " + cus.ZipCode + ".";
            }

            Addresstr = string.Format(EmailTemplateHelper.Address, add);

            string TkReply = string.Empty;
            List<TicketReply> TicketReplyList = _Util.Facade.TicketFacade.GetAllPublicTicketReplyByTicketId(tk.TicketId);
            if (TicketReplyList.Count() > 0)
            {
                foreach (var reply in TicketReplyList)
                {
                    TkReply += string.Format(EmailTemplateHelper.TicketReply, reply.CreatedByVal, string.Format(HS.Framework.Utils.ConvertDatetimeToAgo.TimeAgo(reply.RepliedDate).ToString("M/d/yy h:mm tt")), LabelHelper.HtmlToPlainText(reply.Message));
                }

            }
            if (Email.Count > 0)
            {
                string Subject = "A Notification Has Been Send By " + currentLoggedIn.FirstName + ' ' + currentLoggedIn.LastName;
                foreach (string recevieremail in Email)
                {
                    string ToWhom = "";
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmailAddress(recevieremail);
                    if (emp != null)
                    {
                        ToWhom = emp.FirstName + " " + emp.LastName;
                    }
                    else
                    {
                        Customer cusname = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(tk.CustomerId);
                        ToWhom = cusname.FirstName + " " + cusname.LastName;
                    }

                    NotificationEmail EmailNotifiaction = new NotificationEmail()
                    {
                        ToWhom = ToWhom,
                        Subject = Subject,
                        ToEmail = recevieremail,
                        EmailBody = string.Format(EmailTemplateHelper.TicketNotificationEmailBody, currentLoggedIn.FirstName + ' ' + currentLoggedIn.LastName, TicketId, CustomerName, AppointmentType, AssignedToName, AdditionalMember, ScheduleDate, ScheduleStartDate, ScheduleEndDate, Addresstr, TkReply)
                    };
                    result = _Util.Facade.MailFacade.SendTicketNotificationEmail(EmailNotifiaction, currentLoggedIn.CompanyId.Value);

                    #region Correspondence
                    if (result)
                    {
                        EmailHistory mail = _Util.Facade.MailFacade.GetLastEmailHistoryByTemplateKeyAndSubjectAndToEmail("EstimateNotificationSendTemplate", Subject, recevieremail);
                        LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            CustomerId = cus.CustomerId,
                            Type = "Email",
                            ToEmail = recevieremail,
                            BodyContent = mail.EmailBodyContent,
                            SentDate = DateTime.Now.UTCCurrentTime(),
                            LastUpdatedDate = DateTime.Now,
                            SentBy = currentLoggedIn.UserId,
                            Subject = Subject,
                            IsSystemAutoSent = true,
                            TemplateKey = "EstimateNotificationSendTemplate"
                        };
                        _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(LeadCorrespondence);
                    }
                    #endregion
                }
            }
            return result;
        }

    }
}