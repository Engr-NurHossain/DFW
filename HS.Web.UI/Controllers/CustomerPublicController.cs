using HS.Entities;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa.Options;
using HS.Web.UI.Helper;
using System.Configuration;
using HS.Payments.RecurringBilling;
using HS.Payments.CustomerProfiles;
using AuthorizeNet.Api.Contracts.V1;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using HS.Framework;
using HS.Alarm.CustomerManager;
using HtmlAgilityPack;
using System.Net;
using HS.Framework.Utils;
using System.Reflection;
using HS.Web.UI.Controllers;


namespace HS.Web.UI.Controllers
{
    public class CustomerPublicController : BaseController
    {
        // GET: CustomerPublic
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult TicketListPartial(TicketFilter Filters)
        {
            //TicketFilter Filters = new TicketFilter();

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

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
            Filters.CustomerId = CurrentUser.UserId;
            Filters.TicketType = "-1";
            Filters.TicketStatus = "-1";
            Filters.Assigned = Guid.Empty;
            Filters.MyTicket = "-1";

            Filters.PageSize = _Util.Facade.GlobalSettingsFacade.GetTicketPageLimit(CurrentUser.CompanyId.Value);
            Filters.CompanyId = CurrentUser.CompanyId.Value;
            Filters.UserId = CurrentUser.UserId;

            TicketListModel Model = _Util.Facade.TicketFacade.GetTicketListByCustomerIdAndFilter(Filters, null);
            ViewBag.PageNumber = Filters.PageNo;
            ViewBag.OutOfNumber = 0;


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

            return View("_TicketListPartial", Model);
        }
        [Authorize]
        public ActionResult AddTicket(int? Id)
        {

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.UserRole = CurrentUser.UserRole;
            Guid CustomerId = CurrentUser.UserId;
            CreateTicketModel Model = new CreateTicketModel();

            if (Id.HasValue && Id.Value > 0)
            {
                #region Existing Ticket
                Model.Ticket = _Util.Facade.TicketFacade.GetTicketById(Id.Value);
                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(CurrentUser.UserId);
                if (emp != null)
                {
                    Model.ProfilePicture = emp.ProfilePicture;
                }
                if (Model.Ticket != null)
                {

                    Model.Ticket.CreatedDateVal = string.Format(HS.Framework.Utils.ConvertDatetimeToAgo.TimeAgo(Model.Ticket.CreatedDate).ToString("MM/dd/yy hh:mm tt"));

                    if (Model.Ticket.CompanyId != CurrentUser.CompanyId.Value
                        || Model.Ticket.CustomerId != CurrentUser.UserId)
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
                        Model.CustomerAppointmentEquipmentList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(Model.Ticket.TicketId);
                        Model.Ticket.AppointmentStartTime = Model.CustomerAppointment.AppointmentStartTime;
                        Model.Ticket.AppointmentEndTime = Model.CustomerAppointment.AppointmentEndTime;
                        // = _Util.Facade.CustomerAppoinmentFacade.IsAppointmentEquipmentExistCheck(AppointmentId);
                    }
                }
                #endregion
            }


            #region if null assign new variable
            Model.TicketDefaultTimeDuration = _Util.Facade.GlobalSettingsFacade.GetTicketDefaultTimeDuration(CurrentUser.CompanyId.Value);

            List<SelectListItem> CustomerList = new List<SelectListItem>();
            CustomerList.Add(new SelectListItem
            {
                Text = "Customer",
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

            Model.Customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);

            if (Model.Customer != null)
            {
                Model.Ticket.CustomerId = Model.Customer.CustomerId;
            }
            else
            {
                Model.Customer = new Customer();
            }
            #endregion

            #region If value comes from schedule
            //if (!string.IsNullOrWhiteSpace(startTime) && startTime != "-1")
            //{
            //    Model.Ticket.AppointmentStartTime = startTime;
            //    int hour = ((int.Parse(startTime.Split(':')[0]) + Model.TicketDefaultTimeDuration)) % 24;
            //    string time = "";
            //    if (hour < 10)
            //    {
            //        time = '0' + hour.ToString();
            //    }
            //    else
            //    {
            //        time = hour.ToString();
            //    }
            //    time = time + ":" + startTime.Split(':')[1];
            //    Model.Ticket.AppointmentEndTime = time;
            //    if (!string.IsNullOrWhiteSpace(loadDate))
            //    {
            //        Model.Ticket.CompletionDate = Convert.ToDateTime(loadDate);
            //    }

            //    if (Session["TempTicket"] != null)
            //    {
            //        Ticket TempTicket = (Ticket)Session["TempTicket"];
            //        if (Model.Ticket.Id == 0)
            //        {
            //            Model.Ticket.TicketType = TempTicket.TicketType;
            //            Model.Ticket.Message = TempTicket.Message;
            //        }
            //        ViewBag.TempTicketStatusval = TempTicket.Status;

            //        Session["TempTicket"] = null;
            //    }
            //}
            //if (UserId.HasValue && UserId > 0)
            //{
            //    Model.TicketAssignedUserList = new List<TicketUser>();
            //    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeById(UserId.Value);
            //    if (emp != null)
            //    {
            //        Model.TicketAssignedUserList.Add(new TicketUser()
            //        {
            //            TiketId = Model.Ticket.TicketId,
            //            IsPrimary = true,
            //            FullName = emp.FirstName + " " + emp.LastName,
            //            UserId = emp.UserId
            //        });
            //    }
            //}
            #endregion

            #region ViewBags

            #region Invoice and estimate list
            var InvList = _Util.Facade.InvoiceFacade.GetAllOpenInvoiceByCustomerId(CustomerId);
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

            var EstList = _Util.Facade.InvoiceFacade.GetAllOpenEstimateByCustomerId(CustomerId);
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


            ViewBag.TicketPriority = _Util.Facade.LookupFacade.GetLookupByKey("TicketPriority").OrderBy(x => x.DisplayText).ToList().Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();
            List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value).OrderBy(x => x.FirstName).ToList();
            ViewBag.EmployeeList = EmpList.Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                    Selected = Model.TicketUserList.Count(y => y.UserId == x.UserId) > 0
                }).ToList();
            ViewBag.AssignedEmployee = EmpList.Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                    Selected = Model.TicketAssignedUserList.Count(y => y.UserId == x.UserId) > 0
                }).ToList();

            ViewBag.NotifyingUserList = EmpList.Select(x =>
               new SelectListItem()
               {
                   Text = x.FirstName + " " + x.LastName,
                   Value = x.UserId.ToString(),
                   Selected = Model.NotifyingUserList.Count(y => y.UserId == x.UserId) > 0
               }).ToList();

            ViewBag.TicketStatus = _Util.Facade.LookupFacade.GetLookupByKey("TicketStatus").OrderBy(x => x.DisplayText).ToList().Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();
            ViewBag.TicketType = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").OrderBy(x => x.DisplayText).ToList().Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString()
               }).ToList();


            var arrivaltime = _Util.Facade.LookupFacade.GetLookupByKey("Arrival").OrderBy(x => x.DisplayText).ToList().Where(x => x.IsActive == true).Select(x =>
                    new SelectListItem()
                    {
                        Text = x.DisplayText.ToString(),
                        Value = x.DataValue.ToString()
                    }).ToList();

            ViewBag.AppointmentTime = arrivaltime;


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
                Guid TicCustomerId = new Guid();
                if(Model != null)
                {
                    TicCustomerId = Model.Ticket.CustomerId;
                }
                
                var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, TicCustomerId);
                if (GetSalesTax != null)
                {
                    TaxListItem.Add(new SelectListItem()
                    {
                        Text = GetSalesTax.SearchKey.ToString(),
                        Value = GetSalesTax.Value.ToString()
                    });
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
            #endregion  

            return View("_AddCustomerTicket", Model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddTicket(Ticket Ticket, Guid[] Assigned, Guid[] UserList, Guid[] NotifyingUserList)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string Message = "";

            #region Check 
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
                    return Json(new { result = false, message = string.Format("One or more user has schedules on that day. You can't schedule for all day.") });
                }
                foreach (var item in TicketSchedules.Where(x => x.TicketId != Ticket.TicketId))
                {
                    int startTime = -1;
                    int endTime = -1;

                    int.TryParse(item.StartTime.Replace(":", ""), out startTime);
                    int.TryParse(item.EndTime.Replace(":", ""), out endTime);

                    if (startTime == -1 || endTime == -1)
                    {
                        return Json(new { result = false, message = string.Format("{0} has all day schedule.", item.EmployeeName) });
                    }
                    if (startTime == ticketStartTime || (endTime > ticketStartTime && startTime < ticketStartTime))
                    {
                        return Json(new { result = false, message = string.Format("{0} has scheduling conflict. Please check to make sure time is not overlapping.", item.EmployeeName) });
                    }
                    if (endTime == ticketEndTime || (endTime > ticketEndTime && startTime < ticketEndTime))
                    {
                        return Json(new { result = false, message = string.Format("{0} has scheduling conflict. Please check to make sure time is not overlapping.", item.EmployeeName) });
                    }
                }
            }

            #endregion
            #region Init Values
            int CustomerId = 0;
            Customer TicketCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Ticket.CustomerId);
            if (TicketCustomer != null && TicketCustomer.Id > 0)
            {
                CustomerId = TicketCustomer.Id;
            }
            bool IsNewTicket = !(Ticket.Id > 0);
            #endregion
            if (Ticket.Id > 0)
            {
                #region Update Ticket 
                Ticket TempTicket = _Util.Facade.TicketFacade.GetTicketById(Ticket.Id);
                if (TempTicket == null || TempTicket.TicketId != Ticket.TicketId)
                {
                    return Json(new { result = false, message = "Access denied." });
                }
                if (Ticket.CustomerId != Guid.Empty)
                {
                    TempTicket.CustomerId = Ticket.CustomerId;
                }
                TempTicket.Priority = Ticket.Priority;
                TempTicket.CompletionDate = Ticket.CompletionDate;
                TempTicket.Status = Ticket.Status;
                TempTicket.LastUpdatedBy = CurrentUser.UserId;
                TempTicket.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                TempTicket.AppointmentEndTime = Ticket.AppointmentEndTime;
                TempTicket.AppointmentStartTime = Ticket.AppointmentStartTime;

                if (Ticket.CompletionDate == null)
                {
                    Ticket.CompletionDate = new DateTime();
                }
                _Util.Facade.TicketFacade.UpdateTicket(TempTicket);
                Ticket = TempTicket;
                var strMsg = HS.Web.UI.Helper.LanguageHelper.T("Ticket");
                Message = strMsg + " added successfully.";

                CustomerAppointment ca = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentDetailinfoByAppointmentId(TempTicket.TicketId);

                if (ca != null)
                {
                    bool IsAllDay = true;
                    if (!string.IsNullOrWhiteSpace(Ticket.AppointmentStartTime) && Ticket.AppointmentStartTime != "-1"
                        && !string.IsNullOrWhiteSpace(Ticket.AppointmentEndTime) && Ticket.AppointmentEndTime != "-1")
                    {
                        IsAllDay = false;
                    }
                    if (Ticket.CustomerId != Guid.Empty)
                    {
                        ca.CustomerId = Ticket.CustomerId;
                    }
                    ca.LastUpdatedBy = User.Identity.Name;
                    ca.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    ca.AppointmentStartTime = Ticket.AppointmentStartTime;
                    ca.AppointmentEndTime = Ticket.AppointmentEndTime;
                    ca.AppointmentType = Ticket.TicketType;
                    ca.AppointmentDate = Ticket.CompletionDate;
                    ca.IsAllDay = IsAllDay;
                    _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinment(ca);
                }
                else
                {
                    bool IsAllDay = true;
                    if (!string.IsNullOrWhiteSpace(Ticket.AppointmentStartTime) && Ticket.AppointmentStartTime != "-1"
                        && !string.IsNullOrWhiteSpace(Ticket.AppointmentEndTime) && Ticket.AppointmentEndTime != "-1")
                    {
                        IsAllDay = false;
                    }

                    ca = new CustomerAppointment()
                    {
                        AppointmentDate = Ticket.CompletionDate,
                        AppointmentId = Ticket.TicketId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedBy = User.Identity.Name,
                        CustomerId = Ticket.CustomerId,
                        IsAllDay = IsAllDay,
                        EmployeeId = new Guid(),//From now on customer appoinment will be check from ticket user;
                        LastUpdatedBy = User.Identity.Name,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        Notes = Ticket.Message,
                        AppointmentStartTime = Ticket.AppointmentStartTime,
                        AppointmentEndTime = Ticket.AppointmentEndTime,
                        AppointmentType = Ticket.TicketType,

                    };
                    ca.Id = (int)_Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(ca);
                }
                //}
                #endregion
            }
            else
            {
                #region Insert Ticket 
                Ticket.CreatedBy = CurrentUser.UserId;
                Ticket.CreatedDate = DateTime.Now.UTCCurrentTime();
                Ticket.LastUpdatedBy = CurrentUser.UserId;
                Ticket.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                Ticket.CompanyId = CurrentUser.CompanyId.Value;
                Ticket.TicketId = Guid.NewGuid();
                Ticket.Status = "Created";
                if (Ticket.CompletionDate == null || Ticket.CompletionDate == new DateTime())
                {
                    Ticket.CompletionDate = new DateTime(1970, 1, 1);

                }
                Ticket.Id = _Util.Facade.TicketFacade.InsertTicket(Ticket);
                Message = "Ticket added successfully.";

                bool IsAllDay = true;
                if (!string.IsNullOrWhiteSpace(Ticket.AppointmentStartTime) && Ticket.AppointmentStartTime != "-1"
                    && !string.IsNullOrWhiteSpace(Ticket.AppointmentEndTime) && Ticket.AppointmentEndTime != "-1")
                {
                    IsAllDay = false;
                }
                CustomerAppointment ca = new CustomerAppointment()
                {
                    AppointmentDate = Ticket.CompletionDate,
                    AppointmentId = Ticket.TicketId,
                    CompanyId = CurrentUser.CompanyId.Value,
                    CreatedBy = User.Identity.Name,
                    CustomerId = Ticket.CustomerId,
                    IsAllDay = IsAllDay,
                    EmployeeId = new Guid(),//From now on customer appoinment will be check from ticket user;
                    LastUpdatedBy = User.Identity.Name,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    Notes = Ticket.Message,
                    AppointmentStartTime = Ticket.AppointmentStartTime,
                    AppointmentEndTime = Ticket.AppointmentEndTime,
                    AppointmentType = Ticket.TicketType,

                };
                ca.Id = (int)_Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(ca);
                #endregion
            }

            #region Insert/Update TicketUser 
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
            _Util.Facade.TicketFacade.DeleteTicketUserByTicketId(Ticket.TicketId, false);
            if (UserList != null && UserList.Count() > 0)
            {
                foreach (var item in UserList)
                {
                    if (item == new Guid())
                        continue;

                    var TicketUser = new TicketUser()
                    {
                        AddedBy = CurrentUser.UserId,
                        AddedDate = DateTime.Now.UTCCurrentTime(),
                        UserId = item,
                        IsPrimary = false,
                        TiketId = Ticket.TicketId,
                        NotificationOnly = false,
                    };
                    _Util.Facade.TicketFacade.InsertTicketUser(TicketUser);
                }
            }
            _Util.Facade.TicketFacade.DeleteTicketUserByTicketId(Ticket.TicketId, null, true);
            if (NotifyingUserList != null && NotifyingUserList.Count() > 0)
            {
                foreach (var item in NotifyingUserList)
                {
                    if (item == new Guid())
                        continue;

                    var TicketUser = new TicketUser()
                    {
                        AddedBy = CurrentUser.UserId,
                        AddedDate = DateTime.Now.UTCCurrentTime(),
                        UserId = item,
                        IsPrimary = false,
                        TiketId = Ticket.TicketId,
                        NotificationOnly = true,
                    };
                    _Util.Facade.TicketFacade.InsertTicketUser(TicketUser);
                }
            }

            #endregion

            if (IsNewTicket)
            {
                #region Send Ticket Created Email


                string ToEmailList = "";
                List<Guid> AllUser = new List<Guid>();
                if (Assigned != null)
                {
                    AllUser.AddRange(Assigned);
                }
                if (UserList != null)
                {
                    AllUser.AddRange(UserList);
                }
                if (NotifyingUserList != null)
                {
                    AllUser.AddRange(NotifyingUserList);
                }
                AllUser = AllUser.GroupBy(x => x).Select(x => x.Key).ToList();

                #region Insert notification
                Notification notification = new Notification()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    NotificationId = Guid.NewGuid(),
                    Type = LabelHelper.NotificationType.Employee,
                    Who = CurrentUser.UserId,
                    What = string.Format(@"{0} created a new Ticket <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenTicketById('{1}')"">Ticket #{1}</a>", "{0}", Ticket.Id),

                };
                _Util.Facade.NotificationFacade.InsertNotification(notification);

                #endregion

                foreach (Guid item in AllUser)
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
                    TicketNotificationEmails TicketCreatedNotificationEmail = new TicketNotificationEmails()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedByName = CurrentUser.GetFullName(),
                        TicketMessage = Ticket.Message,
                        CreatedForCustomerName = CustomerName,
                        TicketNumber = string.Format("Ticket #{0}", Ticket.Id),
                        ToEmail = ToEmailList,
                        Subject = string.Format("A new ticket has been created (Ticket #{0})", Ticket.Id),
                        HeaderMessage = "A new ticket has been created",
                        BodyMessage = string.Format("A new ticket (Ticket #{0}) has been created by {1} for customer {2}", Ticket.Id, CurrentUser.GetFullName(), CustomerName),
                        TicketUrl = string.Format("{2}{4}/Public/Open/?TicketId={0}&CompanyId={1}&Type={3}", Ticket.TicketId, CurrentUser.CompanyId.Value, AppConfig.SiteDomain, LabelHelper.PublicOpenTypes.Ticket, AppConfig.DomainSitePath)
                    };
                    _Util.Facade.MailFacade.SendTicketCreatedNotificationEmail(TicketCreatedNotificationEmail);
                }
                #endregion
            }

            return Json(new { result = true, message = Message, CustomerId = CustomerId, TicketId = Ticket.Id });
        }

        public ActionResult UpdateCustomerChange(Guid CustomerId, string ColumnName, string NewValue)
        {
            bool result = false;
            string message = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            try
            {
                if (ColumnName == "MonthlyMonitoringFee")
                {
                    bool isValue = false;
                    MMR mmr = new MMR();
                    List<MMR> mmrList = _Util.Facade.MmrFacade.GetAllMMR();
                    foreach (var item in mmrList)
                    {
                        if (item.Value == Convert.ToDouble(NewValue))
                        {
                            isValue = true;

                        }
                    }
                    if (isValue == false)
                    {
                        mmr = new MMR()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            Name = NewValue,
                            Value = Convert.ToDouble(NewValue),
                            IsActivve = true

                        };
                        _Util.Facade.MmrFacade.InsertMMR(mmr);
                    }
                }
                DateTime LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                Guid LastUpdatedByUid = currentLoggedIn.UserId;
                string LastUpdatedBy = User.Identity.Name;

                _Util.Facade.CustomerFacade.UpdateCustomerChange(CustomerId, ColumnName, NewValue, LastUpdatedDate, LastUpdatedByUid, LastUpdatedBy);
                Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                UpdateCustomerConfirmation email = new UpdateCustomerConfirmation()
                {
                    ToEmail = cus.EmailAddress,
                    Name = cus.FirstName + cus.LastName,
                    ColumnName = ColumnName,
                    CompanyId = currentLoggedIn.CompanyId.Value

                };
                try
                {
                    _Util.Facade.MailFacade.SendUpdateCustomerEmail(email);
                }
                catch (Exception ex)
                {

                }
                result = true;
                message = "Customer Updated Successfully.";
            }
            catch (Exception ex)
            {
                result = false;
                message = "Customer Not Updated.";
            }
            return Json(new
            {
                result = result,
                message = message
            });
        }

        public ActionResult UpdateCustomerPaymentChange(Guid CustomerId, string ColumnName, string NewValue)
        {
            bool result = false;
            string message = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            try
            {
                DateTime LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                Guid LastUpdatedByUid = currentLoggedIn.UserId;
                string LastUpdatedBy = User.Identity.Name;
                PaymentInfoCustomerDraft pcDraft = new PaymentInfoCustomerDraft();
                PaymentInfoCustomer pc = new PaymentInfoCustomer();
                if (ColumnName == "PaymentMethod" && NewValue == "ACH" || ColumnName == "PaymentMethod" && NewValue == "Credit Card")
                {
                    pcDraft = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentInfoCustomerDraftByCustomerId(CustomerId);
                    if (pcDraft != null)
                    {
                        pc = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentInfoCustomerByCustomerIdAndPaymentInfoId(CustomerId, pcDraft.PaymentInfoId);
                        if (pc == null)
                        {
                            pc = new PaymentInfoCustomer()
                            {
                                CustomerId = pcDraft.CustomerId,
                                CompanyId = pcDraft.CompanyId,
                                PaymentInfoId = pcDraft.PaymentInfoId,
                                Type = pcDraft.Type,
                                Payfor = pcDraft.Payfor

                            };
                            _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(pc);
                            _Util.Facade.CustomerFacade.UpdateCustomerChange(CustomerId, ColumnName, NewValue, LastUpdatedDate, LastUpdatedByUid, LastUpdatedBy);

                            result = true;
                            message = "Customer Updated Successfully.";
                        }
                        else
                        {
                            pc.CustomerId = pcDraft.CustomerId;
                            pc.CompanyId = pcDraft.CompanyId;
                            pc.PaymentInfoId = pcDraft.PaymentInfoId;
                            pc.Type = pcDraft.Type;
                            pc.Payfor = pcDraft.Payfor;
                            _Util.Facade.PaymentInfoCustomerFacade.UpdatePaymentInfoCustomer(pc);
                            _Util.Facade.CustomerFacade.UpdateCustomerChange(CustomerId, ColumnName, NewValue, LastUpdatedDate, LastUpdatedByUid, LastUpdatedBy);
                            result = true;
                            message = "Customer Updated Successfully.";
                        }


                    }
                    else
                    {
                        _Util.Facade.CustomerFacade.UpdateCustomerChange(CustomerId, ColumnName, NewValue, LastUpdatedDate, LastUpdatedByUid, LastUpdatedBy);
                        result = true;
                        message = "Customer Updated Successfully.";
                    }
                }
                else
                {
                    _Util.Facade.CustomerFacade.UpdateCustomerChange(CustomerId, ColumnName, NewValue, LastUpdatedDate, LastUpdatedByUid, LastUpdatedBy);
                    result = true;
                    message = "Customer Updated Successfully.";
                }

            }
            catch (Exception ex)
            {
                result = false;
                message = "Customer Not Updated.";
            }
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            UpdateCustomerConfirmation email = new UpdateCustomerConfirmation()
            {
                ToEmail = cus.EmailAddress,
                Name = cus.FirstName + cus.LastName,
                ColumnName = "Payment Method",
                CompanyId = currentLoggedIn.CompanyId.Value

            };
            try
            {
                _Util.Facade.MailFacade.SendUpdateCustomerEmail(email);
            }
            catch (Exception ex)
            {

            }
            return Json(new
            {
                result = result,
                message = message
            });
        }
        public ActionResult UpdateChangeSysteminfo(Guid CustomerId, string ColumnName, string NewValue)
        {
            bool result = false;
            string message = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            try
            {
                _Util.Facade.CustomerFacade.UpdateCustomerChangeSysinfo(CustomerId, ColumnName, NewValue);
                result = true;
                message = "Customer Updated Successfully.";
            }
            catch (Exception ex)
            {
                result = false;
                message = "Customer Not Updated.";
            }
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            UpdateCustomerConfirmation email = new UpdateCustomerConfirmation()
            {
                ToEmail = cus.EmailAddress,
                Name = cus.FirstName + cus.LastName,
                ColumnName = "System information",
                CompanyId = currentLoggedIn.CompanyId.Value

            };
            try
            {
                _Util.Facade.MailFacade.SendUpdateCustomerEmail(email);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return Json(new
            {
                result = result,
                message = message
            });
        }
        public ActionResult UpdateChangeEmergencyContact(Guid CustomerId)
        {
            bool result = false;
            string message = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            try
            {
                EmergencyContact em = new EmergencyContact();
                List<EmergencyContactDraft> emDraft = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactDraftByCustomerIdAndCompanyId(CustomerId, currentLoggedIn.CompanyId.Value);
                foreach (var item in emDraft)
                {
                    em.CompanyId = item.CompanyId;
                    em.CustomerId = item.CustomerId;
                    em.CrossSteet = item.CrossSteet;
                    em.Email = item.Email;
                    em.FirstName = item.FirstName;
                    em.LastName = item.LastName;
                    em.RelationShip = item.RelationShip;
                    em.Phone = item.Phone;
                    em.PhoneType = item.PhoneType;
                    _Util.Facade.EmergencyContactFacade.InsertEmergencyContact(em);
                    _Util.Facade.EmergencyContactFacade.DeleteEmergencyContactDraftById(item.Id);
                }



                result = true;
                message = "Emergency contact Updated Successfully.";
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                result = false;
                message = "Customer Not Updated.";
            }
            return Json(new
            {
                result = result,
                message = message
            });
        }


        [Authorize]
        public PartialViewResult AddCustomerDraft(int? id, bool? IsGotoBill, bool? IsSystemInfo, bool? IsAccountActivity, bool? IsEmergency)
        {
            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerCreate))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateTicketModel Model = new CreateTicketModel();
            Customer model;
            List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value);
            if (id.HasValue)
            {
                model = _Util.Facade.CustomerFacade.GetById(id.Value);
                if (IsGotoBill.HasValue && IsGotoBill == true)
                {
                    ViewBag.IsGotoBill = 1;
                }
                if (IsSystemInfo.HasValue && IsSystemInfo == true)
                {
                    ViewBag.IsSystemInfo = 1;
                }
                if (IsAccountActivity.HasValue && IsAccountActivity == true)
                {
                    ViewBag.IsAccountActivity = 1;
                }
                if (IsEmergency.HasValue && IsEmergency == true)
                {
                    ViewBag.IsEmergency = 1;
                }
                List<PaymentInfo> AllCustomerPaymentInfo = _Util.Facade.PaymentInfoFacade.GetAllPaymentInfoByCustomerIdAndCompanyId(model.CustomerId, CurrentLoggedInUser.CompanyId.Value);
                var CreditCardInfo = AllCustomerPaymentInfo.Where(x => x.CardSecurityCode != "" && x.CardNumber != "" && x.CardExpireDate != "").FirstOrDefault();
                if (CreditCardInfo == null)
                {
                    CreditCardInfo = new PaymentInfo();
                }
                ViewBag.CreditCardInfo = CreditCardInfo;

                var ACHInfo = AllCustomerPaymentInfo.Where(x => x.BankAccountType != "" && x.EcheckType != "" && x.RoutingNo != "").FirstOrDefault();
                if (ACHInfo == null)
                {
                    ACHInfo = new PaymentInfo();
                }


                ViewBag.ACHInfo = ACHInfo;
                ViewBag.QA1Percentageval = _Util.Facade.CustomerFacade.GetCustomerQA1StatusByCompanyIdAndCustomerId(CurrentLoggedInUser.CompanyId.Value, model.CustomerId);
                ViewBag.GlobalSettingPercentage = _Util.Facade.CustomerFacade.GetQA1PercentageValueByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                ViewBag.AssignedEmployee = EmpList.Select(x => new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                    Selected = Model.TicketAssignedUserList != null ? Model.TicketAssignedUserList.Count(y => y.UserId == x.UserId) > 0 : false
                }).ToList();
                bool IsQa1Done = false;
                int Qval = 0;
                int Gval = 0;
                if (ViewBag.QA1Percentageval != "")
                {
                    Qval = Convert.ToInt32(Math.Floor(Convert.ToDouble(ViewBag.QA1Percentageval)));
                }
                if (ViewBag.GlobalSettingPercentage != "")
                {
                    Gval = Convert.ToInt32(ViewBag.GlobalSettingPercentage);
                }

                if (Qval <= Gval && Qval != Gval)
                {
                    ViewBag.QA1FinalResult = IsQa1Done;
                }
                else
                {
                    IsQa1Done = true;
                    ViewBag.QA1FinalResult = IsQa1Done;
                }
                bool Tresult = _Util.Facade.CustomerFacade.GetQATechCallValueBycompanyId(CurrentLoggedInUser.CompanyId.Value, model.CustomerId);
                var QATechcallGlobSetting = _Util.Facade.GlobalSettingsFacade.GetQATechCallGlobSettingsBycompanyId(CurrentLoggedInUser.CompanyId.Value);
                model.IsQA2Done = _Util.Facade.CustomerFacade.GetCustomerQA2StatusByCompanyIdAndCustomerId(CurrentLoggedInUser.CompanyId.Value, model.CustomerId);

                if (Tresult != false)
                {
                    Tresult = true;
                    ViewBag.TechFinalResult = Tresult;
                }
                else
                {
                    ViewBag.TechFinalResult = Tresult;
                }
                if (QATechcallGlobSetting.Count > 0 && QATechcallGlobSetting.Count == 3)
                {
                    model.QA1GlobalSettings = (QATechcallGlobSetting[0].Value.ToLower() == "true") ? true : false;
                    model.TechCallGlobalSettings = (QATechcallGlobSetting[1].Value.ToLower() == "true") ? true : false;
                    model.QA2GlobalSettings = (QATechcallGlobSetting[2].Value.ToLower() == "true") ? true : false;
                }


                if (string.IsNullOrWhiteSpace(model.BillNotes)
                    && !string.IsNullOrWhiteSpace(model.Note))
                {
                    model.Note = WebUtility.HtmlDecode(model.Note);
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(model.Note);
                    model.BillNotes = htmlDoc.DocumentNode.InnerText;

                }
            }
            else
            {
                model = new Customer();
                model.CustomerId = Guid.NewGuid();
                model.systemNo = new List<CustomerSystemNo>();
                ViewBag.CreditCardInfo = new PaymentInfo();
                ViewBag.ACHInfo = new PaymentInfo();
                EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value);
                ViewBag.AssignedEmployee = EmpList.Select(x => new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                    Selected = Model.TicketAssignedUserList != null ? Model.TicketAssignedUserList.Count(y => y.UserId == x.UserId) > 0 : false
                }).ToList();
            }
            ViewBag.CustomerUiSetting = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsByCompanyId(CurrentLoggedInUser.CompanyId.Value).Where(x => x.Tag == "CustomerUiSettings").ToList();
            CustomerListInit();

            #region ViewBags
            TicketFilter ticketFilter = new TicketFilter();
            ticketFilter.CompanyId = CurrentLoggedInUser.CompanyId.Value;
            ticketFilter.CustomerId = model.CustomerId;
            ticketFilter.TicketType = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsByCompanyId(CurrentLoggedInUser.CompanyId.Value).Where(x => x.SearchKey == "DefaultTicketType").FirstOrDefault().Value;
            //ticketFilter.TicketStatus = "Created";
            ticketFilter.PageNo = 1;
            ticketFilter.PageSize = 50;
            ticketFilter.SearchText = "";
            //ticketFilter.
            TicketListModel ticketListModel = _Util.Facade.TicketFacade.GetTicketListByCustomerIdAndFilter(ticketFilter, null);
            Ticket tickets = ticketListModel.Tickets.FirstOrDefault();
            if (tickets != null)
            {
                if (tickets.Id > 0)
                {
                    #region Existing Ticket
                    Model.Ticket = tickets;

                    if (Model.Ticket != null)
                    {
                        if (Model.Ticket.CompanyId != CurrentLoggedInUser.CompanyId.Value)
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
                        Model.CustomerAppointment = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentByAppIdCusId(Model.Ticket.TicketId, Model.Ticket.CustomerId);
                        if (Model.CustomerAppointment != null && Model.CustomerAppointment.AppointmentId != new Guid())
                        {
                            Model.CustomerAppointmentEquipmentList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(Model.Ticket.TicketId);
                            Model.Ticket.AppointmentStartTime = Model.CustomerAppointment.AppointmentStartTime;
                            Model.Ticket.AppointmentEndTime = Model.CustomerAppointment.AppointmentEndTime;
                            // = _Util.Facade.CustomerAppoinmentFacade.IsAppointmentEquipmentExistCheck(AppointmentId);
                        }
                    }
                    #endregion
                }
            }
            else
            {
                tickets = new Ticket();
            }
            if (Model.TicketAssignedUserList == null)
            {
                Model.TicketAssignedUserList = new List<TicketUser>();
            }
            ViewBag.AssignedEmployee = EmpList.Select(x => new SelectListItem()
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.UserId.ToString(),
                Selected = Model.TicketAssignedUserList.Count(y => y.UserId == x.UserId) > 0
            }).ToList();
            List<SelectListItem> CustomerList = new List<SelectListItem>();
            if (model.ReferringCustomer == new Guid())
            {
                CustomerList.Add(new SelectListItem
                {
                    Text = "Customer",
                    Value = "00000000-0000-0000-0000-000000000000"
                });
            }
            else
            {
                Customer RefCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.ReferringCustomer);
                if (RefCustomer != null)
                {
                    CustomerList.Add(new SelectListItem
                    {
                        Text = string.IsNullOrWhiteSpace(RefCustomer.BusinessName) ? (RefCustomer.FirstName + " " + RefCustomer.LastName) : RefCustomer.BusinessName,
                        Value = RefCustomer.CustomerId.ToString()
                    });
                }
            }

            ViewBag.CustomerList = CustomerList;

            ViewBag.OwnerShipList = _Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("OwnerShip", null, null).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            List<SelectListItem> MonitoringList = new List<SelectListItem>();
            MonitoringList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = String.Empty
            });
            List<MMR> MMRDropDown = _Util.Facade.CustomerFacade.GetMMRValueListByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            if (MMRDropDown != null && MMRDropDown.Count > 0)
            {
                MonitoringList.AddRange(MMRDropDown.OrderBy(x => x.Name).Select(x =>
                            new SelectListItem()
                            {
                                Text = x.Name.ToString(),
                                Value = x.Value.ToString(),
                                Selected = (model.MonthlyMonitoringFee == x.Value.ToString())
                            }).ToList());
            }

            ViewBag.MonthlyMonitoringFee = MonitoringList;


            //Guid cusid = model.CustomerId;
            ViewBag.sysid = _Util.Facade.CustomerFacade.GetCustomerSystemInfoIdByCompanyId(CurrentLoggedInUser.CompanyId.Value, model.CustomerId);
            ViewBag.sysLastDate = _Util.Facade.CustomerFacade.GetCustomerSystemInfoLastUpdateDateByCompanyIdandCustomerId(CurrentLoggedInUser.CompanyId.Value, model.CustomerId);
            //bool bitvalue = bool.Parse(ViewBag.sysLastDate);
            if (ViewBag.sysid != "")
            {
                model.CustomerSystemId = Int32.Parse(ViewBag.sysid);
            }
            if (ViewBag.sysLastDate != "true")
            {
                model.CustomerSystemCompanyId = CurrentLoggedInUser.CompanyId.Value;
                model.CustomerSystemCustomerId = model.CustomerId;
            }

            ViewBag.salesTaxAmount = "0.0";
            Guid CustoemrId = new Guid();
            if(model != null)
            {
                CustoemrId = model.CustomerId;
            }
            var salesTaxAmountObj = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentLoggedInUser.CompanyId.Value, CustoemrId);
            if (salesTaxAmountObj != null)
            {
                ViewBag.salesTaxAmount = salesTaxAmountObj.Value;
            }

            ViewBag.BillCycle = _Util.Facade.LookupFacade.GetLookupByKey("BillCycle").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            ViewBag.BussinessAccountType = _Util.Facade.LookupFacade.GetLookupByKey("BussinessAccountType").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            List<SelectListItem> EmployeeNameList = new List<SelectListItem>();
            EmployeeNameList.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = ""
            });
            EmployeeNameList.AddRange(_Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value).Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                                      Value = x.UserId.ToString()
                                  }).ToList());
            ViewBag.EmployeeNameList = EmployeeNameList;
            List<SelectListItem> QualityAssuranceList1 = new List<SelectListItem>();
            QualityAssuranceList1.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = ""
            });
            List<Employee> EmployeeDropDown = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value);
            if (EmployeeDropDown != null && EmployeeDropDown.Count > 0)
            {
                QualityAssuranceList1.AddRange(EmployeeDropDown.OrderBy(x => x.FirstName).Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                                      Value = x.UserId.ToString()
                                  }).ToList());
            }

            ViewBag.QualityAssuranceList1 = QualityAssuranceList1;
            List<SelectListItem> QualityAssuranceList2 = new List<SelectListItem>();
            QualityAssuranceList2.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = ""
            });
            List<Employee> EmployeeDropDown2 = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value);
            if (EmployeeDropDown2 != null && EmployeeDropDown2.Count > 0)
            {
                QualityAssuranceList2.AddRange(EmployeeDropDown2.OrderBy(x => x.FirstName).Select(x =>
                                 new SelectListItem()
                                 {
                                     Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                                     Value = x.UserId.ToString()
                                 }).ToList());
            }


            ViewBag.QualityAssuranceList2 = QualityAssuranceList2;
            ViewBag.LeadStreetType = _Util.Facade.LookupFacade.GetLookupByKey("LeadStreetType").OrderBy(x => x.DataValue).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
            ViewBag.BillingDay = _Util.Facade.LookupFacade.GetLookupByKey("BillingDay").Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.DisplayText.ToString(),
                                 Value = x.DataValue.ToString()
                             }).ToList();
            #endregion

            #region alarm.com dropdowns

            SetupAlarm AlarmModel = _Util.Facade.AlarmFacade.GetSetupalarmByCustomerId(model.CustomerId);
            if (AlarmModel == null || model.CustomerId == new Guid())
            {
                AlarmModel = new SetupAlarm()
                {
                    CustomerId = model.CustomerId,
                    CompanyId = CurrentLoggedInUser.CompanyId.Value,
                    Phone = model.PrimaryPhone,
                    InsCity = model.City,
                    InsState = model.State,
                    InsZip = model.ZipCode,
                    EmailAddress = model.EmailAddress,
                };
                if (!string.IsNullOrWhiteSpace(model.CustomerNo))
                {
                    string[] str = model.CustomerNo.Split(new[] { "FLV12" }, StringSplitOptions.None);
                    if (str.Count() == 2)
                    {
                        AlarmModel.CentralStationAccountNo = str[1];
                    }
                }
            }
            ViewBag.SetupAlarm = AlarmModel;


            ViewBag.CentralStationForwardingOption = _Util.Facade.LookupFacade.GetLookupByKey("CentralStationForwardingOption").OrderBy(x => x.DataValue).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString(),
                              Selected = (AlarmModel.CentrastationForwardingOption == x.DataValue)
                          }).ToList();
            ViewBag.PanelTypeEnum = _Util.Facade.LookupFacade.GetLookupByKey("PanelTypeEnum").OrderBy(x => x.DataValue).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString(),
                              Selected = (AlarmModel.PanelType == x.DataValue)
                          }).ToList();
            ViewBag.PanelVersionEnum = _Util.Facade.LookupFacade.GetLookupByKey("PanelVersionEnum").OrderBy(x => x.DataValue).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString(),
                              Selected = (AlarmModel.PanelVersion == x.DataValue)
                          }).ToList();
            ViewBag.PropertyTypeEnum = _Util.Facade.LookupFacade.GetLookupByKey("PropertyTypeEnum").OrderBy(x => x.DisplayText != "Property Type").ThenBy(x =>x.DisplayText).Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString(),
                             Selected = (AlarmModel.PropertyType == x.DataValue)
                         }).ToList();
            ViewBag.CultureEnum = _Util.Facade.LookupFacade.GetLookupByKey("CultureEnum").OrderBy(x => x.DataValue).Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString(),
                             Selected = (AlarmModel.Culture == x.DataValue)
                         }).ToList();
            ViewBag.StateList = _Util.Facade.LookupFacade.GetLookupByKey("StateList").OrderBy(x => x.DisplayText != "Please Select One").ThenBy(x => x.DisplayText).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString(),
                               Selected = (AlarmModel.InsState == x.DataValue)
                           }).ToList();
            #endregion
            model.Ticket = tickets;
            List<SelectListItem> SourceLead = new List<SelectListItem>();
            SourceLead.AddRange(_Util.Facade.LookupFacade.GetLookupByKeyWithParent("LeadSource").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList());
            ViewBag.SourceLead = SourceLead;
            return PartialView("_AddCustomerDraft", model);
        }

        public ActionResult CheckToken(CustomerDraft customer, CustomerSystemInfoDraft systemInfo, CustomerApiSetting apiAlarm, CustomerApiSetting apiMoni, CustomerApiSetting apiCentral, PaymentInfo PaymentInfo, string nexttab, string Token)
        {
            bool result = false;
            string message = "";
            if (PaymentInfo.AccountName == "ProfilePackage")
            {
                PaymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoById(PaymentInfo.Id);
            }


            if (customer.Passcode == Token)
            {
                //Session["TokenPassed"] = "Yes";
                //Session.Timeout = 1;
                result = true;
                return AddCustomerDraft(customer, systemInfo, apiAlarm, apiMoni, apiCentral, PaymentInfo, nexttab);
            }
            else
            {
                result = false;
                message = "Verbal password does not match. ";
                //Session.Remove("token");
                //Session.Remove("TokenPassed");
                return Json(new { result = result, message = message });
            }




        }


        //public ActionResult CheckRoleAndSaveEmergencyContact(EmergencyContactDraft _EmergencyContact)
        //{
        //    bool result = false;
        //    string message = "";
        //    bool timeout = true;
        //    var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
        //    string UserRole = currentLoggedIn.UserRole;
        //    //Check |Token Have in session 
        //    //If Yes - Save \\//
        //    string token = "";
        //    //If no Token generate send customer and say please add OTP 

        //    if (Session["token"] == null && Session["TokenPassed"] == null)
        //    {

        //        result = false;
        //        timeout = true;
        //        message = "An OTP sent to your email.Please put your OTP here";
        //        token = GenarateToken(8);
        //        Session["token"] = token;
        //        Session["TokenPassed"] = "No";
        //        Session.Timeout = 1;
        //        OTPEmail email = new OTPEmail()
        //        {
        //            ToEmail = currentLoggedIn.EmailAddress,
        //            Name = currentLoggedIn.FirstName + currentLoggedIn.LastName,
        //            OTP = token,
        //            CompanyId = currentLoggedIn.CompanyId.Value

        //        };
        //        try
        //        {
        //            _Util.Facade.MailFacade.SendOTPEmail(email);
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        return Json(new { result = result, message = message, timeout = timeout });
        //    }
        //    else if (Session["TokenPassed"].ToString() == "Yes")
        //    {
        //        result = true;
        //        return AddEmergencyContactDraft(_EmergencyContact);
        //    }
        //    else
        //    {

        //        timeout = false;
        //        result = false;
        //        message = "Session timed out.Try again.";
        //        Session.Remove("token");
        //        Session.Remove("TokenPassed");
        //        return Json(new { result = result, message = message, timeout = timeout });
        //    }

        //}
        public ActionResult CheckTokenAndSaveEmergencyContact(EmergencyContactDraft _EmergencyContact)
        {
            bool result = false;
            string message = "";
            Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(_EmergencyContact.CustomerId);
            if (customer.Passcode == _EmergencyContact.Token)
            {

                result = true;
                return AddEmergencyContactDraft(_EmergencyContact);
            }
            else
            {
                result = false;
                message = "Verbal password does not match. ";

                return Json(new { result = result, message = message });
            }

        }

        public ActionResult ReferedCustomer(Guid customerId)
        {
            ViewBag.CustomerId = customerId;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult SendMailToFriend(CustomerRefer referInfo)
        {
            bool result = false;
            string message = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(referInfo.CustomerId);

            try
            {
                int id = _Util.Facade.CustomerDraftFacade.InsertCustomerRefer(referInfo);
                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(id
                                   + "#"
                                   + currentLoggedIn.CompanyId.Value
                                   + "#"
                                   + referInfo.CustomerId);
                string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Refer/", encryptedurl);
                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, referInfo.CustomerId);
                ViewBag.url = AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code;
                ViewBag.InvoiceId = id;
                SendFriendEmail email = new SendFriendEmail()
                {

                    Name = referInfo.Name,
                    Messsage = referInfo.Message,
                    CompanyId = currentLoggedIn.CompanyId.Value,
                    ToEmail = referInfo.Email,
                    SenderName = customer.FirstName + " " + customer.LastName,
                    ShortLink = ViewBag.url
                };
                try
                {
                    _Util.Facade.MailFacade.SendFriendEmail(email);
                    message = "An email has sent to your friend.";
                    result = true;
                }
                catch (Exception ex)
                {
                    message = "Mail has not sent to your friend.";
                }

            }
            catch (Exception ex)
            {
                result = false;
                message = "Mail has not sent to your friend.";
            }

            return Json(new { result = result, message = message });
        }




        [Authorize]
        [HttpPost]
        public JsonResult AddEmergencyContactDraft(EmergencyContactDraft _EmergencyContact)
        {
            var result = false;
            var message = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            if (currentLoggedIn != null)
            {
                if (_EmergencyContact.Id > 0)
                {
                    var OldEmergencyContactCopy = _Util.Facade.EmergencyContactFacade.GetEmergencyContactByCustomerIdAndCompanyIdAndId(_EmergencyContact.CustomerId, currentLoggedIn.CompanyId.Value, _EmergencyContact.Id);
                    var OldEmergencyContactDraftCopy = _Util.Facade.EmergencyContactFacade.GetEmergencyContactDraftByCustomerIdAndCompanyIdAndId(_EmergencyContact.CustomerId, currentLoggedIn.CompanyId.Value, _EmergencyContact.Id);
                    if (OldEmergencyContactDraftCopy != null)
                    {
                        OldEmergencyContactCopy.FirstName = _EmergencyContact.FirstName;
                        OldEmergencyContactCopy.LastName = _EmergencyContact.LastName;
                        OldEmergencyContactCopy.CrossSteet = _EmergencyContact.CrossSteet;
                        OldEmergencyContactCopy.RelationShip = _EmergencyContact.RelationShip;
                        OldEmergencyContactCopy.Phone = _EmergencyContact.Phone;
                        OldEmergencyContactCopy.HasKey = _EmergencyContact.HasKey;
                        OldEmergencyContactCopy.PhoneType = _EmergencyContact.PhoneType;

                        result = _Util.Facade.EmergencyContactFacade.UpdateEmergencyContact(OldEmergencyContactCopy) > 0;
                        message = "Emergancy Contact Saved successfully.Wait for admin approveal";
                    }
                    else if (OldEmergencyContactCopy != null && OldEmergencyContactDraftCopy == null)
                    {
                        OldEmergencyContactDraftCopy.Id = OldEmergencyContactCopy.Id;
                        OldEmergencyContactDraftCopy.CompanyId = OldEmergencyContactCopy.CompanyId;
                        OldEmergencyContactDraftCopy.FirstName = _EmergencyContact.FirstName;
                        OldEmergencyContactDraftCopy.LastName = _EmergencyContact.LastName;
                        OldEmergencyContactDraftCopy.CrossSteet = _EmergencyContact.CrossSteet;
                        OldEmergencyContactDraftCopy.RelationShip = _EmergencyContact.RelationShip;
                        OldEmergencyContactDraftCopy.Phone = _EmergencyContact.Phone;
                        OldEmergencyContactDraftCopy.HasKey = _EmergencyContact.HasKey;
                        OldEmergencyContactDraftCopy.PhoneType = _EmergencyContact.PhoneType;
                        result = _Util.Facade.EmergencyContactFacade.InsertEmergencyContactDraft(OldEmergencyContactDraftCopy) > 0;
                        message = "Emergancy Contact Saved successfully.Wait for admin approval";
                    }
                }
                else
                {
                    //var EmergencyContactCount = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(_EmergencyContact.CustomerId, currentLoggedIn.CompanyId.Value);
                    //if (EmergencyContactCount.Count < 1)
                    //{
                    //    var LeadInfo = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(_EmergencyContact.CustomerId);
                    //    if (LeadInfo != null)
                    //    {
                    //        var LeadIntId = LeadInfo.Id;
                    //        var LeadobjName = _Util.Facade.CustomerFacade.GetLeadNameByLeadId(LeadIntId).LeadName;

                    //    }
                    //}
                    _EmergencyContact.CompanyId = currentLoggedIn.CompanyId.Value;
                    result = _Util.Facade.EmergencyContactFacade.InsertEmergencyContactDraft(_EmergencyContact) > 0;
                    message = "Emergancy Contact Saved successfully";
                }
            }

            Notification notification = new Notification()
            {
                CompanyId = currentLoggedIn.CompanyId.Value,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                NotificationId = Guid.NewGuid(),
                Type = LabelHelper.NotificationType.Employee,
                Who = currentLoggedIn.UserId,
                What = string.Format(@"{0} Updated his Emergency contact Info ", currentLoggedIn.FirstName + " " + currentLoggedIn.LastName),
                NotificationUrl = AppConfig.DomainSitePath + "/CustomerPublic/ShowCustomerChange?CustomerId=" + currentLoggedIn.UserId

            };
            _Util.Facade.NotificationFacade.InsertNotification(notification);
            List<Employee> emp = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Admin, new Guid());
            foreach (var item in emp)
            {
                NotificationUser nu = new NotificationUser()
                {
                    NotificationId = notification.NotificationId,
                    IsRead = false,
                    NotificationPerson = item.UserId,
                };
                _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
            }
            return Json(new { result = result, message = message });
        }

        //public ActionResult CheckRoleAndGenerateToken()
        //{
        //    bool result = false;
        //    string message = "";
        //    bool timeout = true;
        //    var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
        //    string UserRole = currentLoggedIn.UserRole;
        //    if (UserRole == "Customer")
        //    {
        //        if (Session["token"] == null && Session["TokenPassed"] == null)
        //        {

        //            result = false;

        //            message = "Please put your varbal password here.";
        //            token = GenarateToken(8);
        //            Session["token"] = token;
        //            Session["TokenPassed"] = "No";
        //            Session.Timeout = 1;
        //            OTPEmail email = new OTPEmail()
        //            {
        //                ToEmail = customer.EmailAddress,
        //                Name = customer.FirstName + customer.LastName,
        //                OTP = token,
        //                CompanyId = currentLoggedIn.CompanyId.Value

        //            };
        //            try
        //            {
        //                _Util.Facade.MailFacade.SendOTPEmail(email);
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //            return Json(new { result = result, message = message, timeout = timeout });
        //        }
        //        else if (Session["TokenPassed"].ToString() == "Yes")
        //        {
        //            result = true;

        //            return AddCustomerDraft(customer, systemInfo, apiAlarm, apiMoni, apiCentral, PaymentInfo, nexttab);
        //        }
        //        else
        //        {

        //            timeout = false;
        //            result = false;
        //            message = "Session timed out.Try again.";
        //            Session.Remove("token");
        //            Session.Remove("TokenPassed");
        //            return Json(new { result = result, message = message, timeout = timeout });
        //        }
        //    }

        //    return Json(new { result = result, message = message, timeout = timeout });

        //}

        public string GenarateToken(int length)
        {
            string password = "";
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            string Alphabet = "0123456789";
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = Alphabet[rand.Next(Alphabet.Length)];
            }
            password = new string(chars);
            return password;
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddCustomerDraft(CustomerDraft customer, CustomerSystemInfoDraft systemInfo, CustomerApiSetting apiAlarm, CustomerApiSetting apiMoni, CustomerApiSetting apiCentral, PaymentInfo PaymentInfo, string nexttab)
        {
            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerCreate))
            {
                return Json(new
                {
                    status = false,
                    message = "Access denied."
                });
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            customer.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            customer.LastUpdatedByUid = currentLoggedIn.UserId;
            customer.LastUpdatedBy = User.Identity.Name;
            bool result = true;
            string role = currentLoggedIn.UserRole;
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(false);
            }
            try
            {
                #region Edit Customer 
                if (customer.Id > 0)
                {
                    #region Customer Update
                    Customer getCustomer = _Util.Facade.CustomerFacade.GetById(customer.Id);
                    CustomerDraft cust = _Util.Facade.CustomerDraftFacade.GetCustomerDraftByCustomerId(currentLoggedIn.UserId);
                    if (cust == null)
                    {
                        #region Insert New Customer

                        //if (!string.IsNullOrWhiteSpace(customer.CustomerNo))
                        //{
                        //    Customer cusnoCheck = _Util.Facade.CustomerFacade.GetCustomerByCustomerNo(customer.CustomerNo);
                        //    if (cusnoCheck != null)
                        //    {
                        //        return Json(new
                        //        {
                        //            status = false,
                        //            message = "Customer No. already used by another customer."
                        //        });
                        //    }
                        //}
                        customer.CustomerId = currentLoggedIn.UserId;
                        customer.Id = getCustomer.Id;
                        customer.Address = MakeAddress(customer.Street, customer.City, customer.State, customer.ZipCode, "");
                        customer.Address2 = MakeAddress(customer.StreetPrevious, customer.CityPrevious, customer.StatePrevious, customer.ZipCodePrevious, "");

                        var customerIsAlarm = customer.IsAlarmCom;
                        customer.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        customer.LastUpdatedBy = User.Identity.Name;
                        customer.CreatedByUid = currentLoggedIn.UserId;
                        customer.LastUpdatedByUid = currentLoggedIn.UserId;
                        customer.JoinDate = DateTime.Now.UTCCurrentTime();
                        customer.IsTechCallPassed = false;
                        customer.IsDirect = true;
                        customer.IsActive = true;
                        customer.CreatedDate = DateTime.Now;
                        if (customer.SalesDate.HasValue)
                        {
                            customer.SalesDate = customer.SalesDate.Value.ClientToUTCTime();
                        }
                        if (customer.InstallDate.HasValue)
                        {
                            customer.InstallDate = customer.InstallDate.Value.ClientToUTCTime();
                        }
                        if (customer.CutInDate.HasValue)
                        {
                            customer.CutInDate = customer.CutInDate.Value.ClientToUTCTime();
                        }
                        if (customer.FundingDate.HasValue)
                        {
                            customer.FundingDate = customer.FundingDate.Value.ClientToUTCTime();
                        }
                        if (customer.FirstBilling.HasValue)
                        {
                            customer.FirstBilling = customer.FirstBilling.Value.SetZeroHour();
                        }
                        if (apiAlarm.Url != null && apiAlarm.UserName != null && apiAlarm.Password != null)
                        {
                            customer.IsAlarmCom = true;
                        }
                        else
                        {
                            customer.IsAlarmCom = false;
                        }
                        customer.ChildOf = Guid.Empty;
                        if (string.IsNullOrEmpty(customer.MonthlyMonitoringFee) && customer.PaymentMethod != "-1")
                        {
                            return Json(new
                            {
                                status = false,

                                message = "Your monitoring fee have not set yet."
                            });
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(customer.MonthlyMonitoringFee))
                            {
                                var MonitoringFee = Convert.ToDouble(customer.MonthlyMonitoringFee) + customer.PaymentIncress;
                                customer.MonthlyMonitoringFee = MonitoringFee.ToString();
                                customer.BillAmount = customer.BillAmount + customer.PaymentIncress;
                            }

                            int CustomerIntID = _Util.Facade.CustomerDraftFacade.InsertCustomerDraftAndReturnId(customer);
                        }


                        if (systemInfo.Id > 0)
                        {
                            CustomerSystemInfo cussys = _Util.Facade.CustomerSystemInfoFacade.GetCustomerSystemInfoById(systemInfo.Id);
                            if (cussys != null)
                            {
                                CustomerSystemInfoDraft cussysDraft = _Util.Facade.CustomerSystemInfoFacade.GetCustomerSystemInfoDraftByCustomerIdAndCompanyId(cussys.CustomerId, cussys.CompanyId);
                                if (cussysDraft != null)
                                {
                                    systemInfo.CompanyId = currentLoggedIn.CompanyId.Value;
                                    systemInfo.CustomerId = cussysDraft.CustomerId;
                                    systemInfo.Id = cussysDraft.Id;
                                    _Util.Facade.CustomerSystemInfoFacade.UpdateSystemInfoDraft(systemInfo);
                                }
                                else
                                {
                                    CustomerSystemInfoDraft objsystemInfo = new CustomerSystemInfoDraft()
                                    {
                                        CustomerId = customer.CustomerId,
                                        CompanyId = currentLoggedIn.CompanyId.Value,
                                        PanelType = systemInfo.PanelType,
                                        InstallType = systemInfo.InstallType,
                                        CellularBackup = systemInfo.CellularBackup,
                                        Zone1 = systemInfo.Zone1,
                                        Zone2 = systemInfo.Zone2,
                                        Zone3 = systemInfo.Zone3,
                                        Zone4 = systemInfo.Zone4,
                                        Zone5 = systemInfo.Zone5,
                                        Zone6 = systemInfo.Zone6,
                                        Zone7 = systemInfo.Zone7,
                                        Zone8 = systemInfo.Zone8,
                                        Zone9 = systemInfo.Zone9
                                    };
                                    _Util.Facade.CustomerSystemInfoFacade.InsertSystemInfoDraft(objsystemInfo);
                                }
                            }
                            else
                            {
                                CustomerSystemInfoDraft objsystemInfo = new CustomerSystemInfoDraft()
                                {
                                    CustomerId = customer.CustomerId,
                                    CompanyId = currentLoggedIn.CompanyId.Value,
                                    PanelType = systemInfo.PanelType,
                                    InstallType = systemInfo.InstallType,
                                    CellularBackup = systemInfo.CellularBackup,
                                    Zone1 = systemInfo.Zone1,
                                    Zone2 = systemInfo.Zone2,
                                    Zone3 = systemInfo.Zone3,
                                    Zone4 = systemInfo.Zone4,
                                    Zone5 = systemInfo.Zone5,
                                    Zone6 = systemInfo.Zone6,
                                    Zone7 = systemInfo.Zone7,
                                    Zone8 = systemInfo.Zone8,
                                    Zone9 = systemInfo.Zone9
                                };
                                _Util.Facade.CustomerSystemInfoFacade.InsertSystemInfoDraft(objsystemInfo);
                            }


                        }
                        else
                        {
                            CustomerSystemInfoDraft objsystemInfo = new CustomerSystemInfoDraft()
                            {
                                CustomerId = customer.CustomerId,
                                CompanyId = currentLoggedIn.CompanyId.Value,
                                PanelType = systemInfo.PanelType,
                                InstallType = systemInfo.InstallType,
                                CellularBackup = systemInfo.CellularBackup,
                                Zone1 = systemInfo.Zone1,
                                Zone2 = systemInfo.Zone2,
                                Zone3 = systemInfo.Zone3,
                                Zone4 = systemInfo.Zone4,
                                Zone5 = systemInfo.Zone5,
                                Zone6 = systemInfo.Zone6,
                                Zone7 = systemInfo.Zone7,
                                Zone8 = systemInfo.Zone8,
                                Zone9 = systemInfo.Zone9
                            };
                            _Util.Facade.CustomerSystemInfoFacade.InsertSystemInfoDraft(objsystemInfo);
                        }
                        bool PaymentInfoIsNull = true;
                        if (!string.IsNullOrWhiteSpace(PaymentInfo.BankAccountType) && !string.IsNullOrWhiteSpace(PaymentInfo.RoutingNo) && !string.IsNullOrWhiteSpace(PaymentInfo.AcountNo))
                        {
                            //ACH checking
                            PaymentInfoIsNull = false;
                        }
                        else if (!string.IsNullOrWhiteSpace(PaymentInfo.CardNumber) && !string.IsNullOrWhiteSpace(PaymentInfo.CardSecurityCode) && !string.IsNullOrWhiteSpace(PaymentInfo.CardExpireDate))
                        {
                            //CreditCard checking
                            PaymentInfoIsNull = false;
                        }
                        #region Customer Payment Info part
                        if (!PaymentInfoIsNull)
                        {
                            if (PaymentInfo.Id > 0)
                            {
                                //if payment info changes what will happen to arb?

                                if (customer.PaymentMethod == LabelHelper.PaymentMethod.CreditCard && PaymentInfo.CardNumber.IndexOf("******") > -1 && PaymentInfo.Id > 0)
                                {
                                    PaymentInfo tmpPaymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfo_Card_ByCompanyIdAndCustomerId(currentLoggedIn.CompanyId.Value, customer.CustomerId);
                                    PaymentInfo.CardNumber = tmpPaymentInfo.CardNumber;
                                }

                                PaymentInfo.CompanyId = currentLoggedIn.CompanyId.Value;
                                PaymentInfo.IsCash = false;
                                _Util.Facade.PaymentInfoFacade.UpdatePaymentInfo(PaymentInfo);
                            }
                            else
                            {
                                PaymentInfo.CompanyId = currentLoggedIn.CompanyId.Value;
                                PaymentInfo objInfoPay = new PaymentInfo()
                                {
                                    CompanyId = PaymentInfo.CompanyId,
                                    AccountName = PaymentInfo.AccountName,
                                    BankAccountType = PaymentInfo.BankAccountType,
                                    RoutingNo = PaymentInfo.RoutingNo,
                                    AcountNo = PaymentInfo.AcountNo,
                                    CardNumber = PaymentInfo.CardNumber,
                                    CardExpireDate = PaymentInfo.CardExpireDate,
                                    CardSecurityCode = PaymentInfo.CardSecurityCode,
                                    EcheckType = PaymentInfo.EcheckType,
                                    BankName = PaymentInfo.BankName

                                };
                                _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(objInfoPay);
                                PaymentInfoCustomerDraft pic = new PaymentInfoCustomerDraft()
                                {
                                    CompanyId = currentLoggedIn.CompanyId.Value,
                                    CustomerId = customer.CustomerId,
                                    PaymentInfoId = objInfoPay.Id,
                                    Payfor = "MMR"
                                };
                                pic.Id = (int)_Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomerDraft(pic);
                            }
                            #endregion Customer Payment Info part
                        }
                        #endregion


                    }
                    else
                    {
                        customer.CustomerId = cust.CustomerId;

                        if (!string.IsNullOrWhiteSpace(customer.CustomerNo) && cust.CustomerNo != customer.CustomerNo)
                        {
                            CustomerDraft cusnoCheck = _Util.Facade.CustomerDraftFacade.GetCustomerDraftByCustomerNo(customer.CustomerNo);
                            if (cusnoCheck != null && cusnoCheck.CustomerId != cust.CustomerId)
                            {
                                return Json(new
                                {
                                    status = false,
                                    customerid = cust.Id,
                                    message = "Customer No. already used by another customer."
                                });
                            }
                            else
                            {
                                CustomerSystemNo sysno = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerIdAndSysNo(cust.Id, cust.CustomerNo);
                                if (sysno != null)
                                {
                                    sysno.IsReserved = false;
                                    sysno.IsUsed = false;
                                    _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(sysno);
                                }
                            }
                        }
                        if (customer.FirstBilling.HasValue)
                        {
                            customer.FirstBilling = customer.FirstBilling.Value.SetZeroHour();
                        }
                        customer.ActivationFee = cust.ActivationFee;
                        customer.CustomerId = cust.CustomerId;
                        customer.JoinDate = cust.JoinDate;
                        customer.IsActive = cust.IsActive;
                        customer.Address = MakeAddress(customer.Street, customer.City, customer.State, customer.ZipCode, customer.Country);
                        customer.Address2 = MakeAddress(customer.StreetPrevious, customer.CityPrevious, customer.StatePrevious, customer.ZipCodePrevious, customer.CountryPrevious);
                        customer.AuthorizeRefId = cust.AuthorizeRefId;
                        customer.AuthorizeCusPaymentProfileId = cust.AuthorizeCusPaymentProfileId;
                        customer.AuthorizeCusProfileId = cust.AuthorizeCusProfileId;
                        customer.AlarmRefId = cust.AlarmRefId;
                        customer.TransunionRefId = cust.TransunionRefId;
                        customer.MonitronicsRefId = cust.MonitronicsRefId;
                        customer.CmsRefId = cust.CmsRefId;
                        customer.Latlng = cust.Latlng;
                        customer.ReminderDate = cust.ReminderDate;
                        customer.QA1 = cust.QA1;
                        customer.QA1Date = cust.QA1Date;
                        customer.QA2 = cust.QA2;
                        customer.QA2Date = cust.QA2Date;
                        customer.DateofBirth = cust.DateofBirth;
                        customer.LastGeneratedInvoice = cust.LastGeneratedInvoice;
                        customer.Singature = cust.Singature;
                        // customer.AuthorizeDescription = cust.AuthorizeDescription; 
                        customer.CreatedByUid = cust.CreatedByUid;
                        customer.CreatedDate = cust.CreatedDate;
                        #region edit customer number 

                        var OldCustomerNoObj = _Util.Facade.CustomerSystemNoFacade.GetCustomerSystemNoObjectByCustomerIdAndCompanyId(customer.Id, currentLoggedIn.CompanyId.Value);
                        if (OldCustomerNoObj != null)
                        {
                            if (OldCustomerNoObj.CustomerNo != customer.CustomerNo)
                            {
                                if (customer.CustomerNo == "")
                                {
                                    OldCustomerNoObj.CustomerId = 0;
                                    OldCustomerNoObj.IsReserved = false;
                                    OldCustomerNoObj.IsUsed = false;
                                    var oldObjRemovedResult = _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(OldCustomerNoObj);
                                }

                                else
                                {
                                    var NewCustomerNumberObj = _Util.Facade.CustomerSystemNoFacade.GetCustomerSystemNoObjectByNumberAndCompanyId(customer.CustomerNo, currentLoggedIn.CompanyId.Value);
                                    if (NewCustomerNumberObj != null)
                                    {
                                        NewCustomerNumberObj.IsReserved = true;
                                        NewCustomerNumberObj.IsUsed = true;
                                        NewCustomerNumberObj.ReserveDate = DateTime.Now.UTCCurrentTime();
                                        NewCustomerNumberObj.UsedDate = DateTime.Now.UTCCurrentTime();
                                        NewCustomerNumberObj.CustomerId = customer.Id;
                                        var NewObjRemovedResult = _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(NewCustomerNumberObj);

                                        if (NewObjRemovedResult == true)
                                        {
                                            OldCustomerNoObj.CustomerId = 0;
                                            OldCustomerNoObj.IsReserved = false;
                                            OldCustomerNoObj.IsUsed = false;
                                            var oldObjRemovedResult = _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(OldCustomerNoObj);

                                            if (oldObjRemovedResult == true)
                                            {
                                                customer.CustomerNo = NewCustomerNumberObj.CustomerNo;
                                            }
                                            else
                                            {
                                                customer.CustomerNo = OldCustomerNoObj.CustomerNo;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        customer.CustomerNo = OldCustomerNoObj.CustomerNo;
                                    }
                                }
                            }
                            else
                            {
                                customer.CustomerNo = OldCustomerNoObj.CustomerNo;
                            }
                        }
                        else
                        {
                            var assignNumber = _Util.Facade.CustomerSystemNoFacade.GetCustomerSystemNoObjectByNumberAndCompanyId(customer.CustomerNo, currentLoggedIn.CompanyId.Value);
                            if (assignNumber != null)
                            {
                                assignNumber.IsReserved = true;
                                assignNumber.IsUsed = true;
                                assignNumber.ReserveDate = DateTime.Now.UTCCurrentTime();
                                assignNumber.UsedDate = DateTime.Now.UTCCurrentTime();
                                assignNumber.CustomerId = customer.Id;
                                var NewObjRemovedResult = _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(assignNumber);
                            }
                            else
                            {
                                customer.CustomerNo = "";
                            }
                        }

                        #endregion

                        if (apiAlarm.Url != null && apiAlarm.UserName != null && apiAlarm.Password != null)
                        {
                            customer.IsAlarmCom = true;
                        }
                        else
                        {
                            customer.IsAlarmCom = false;
                        }
                        if (cust.IsDirect.HasValue && !cust.IsDirect.Value)
                        {
                            customer.IsDirect = false;
                        }
                        //if (cust.IsDirect.HasValue && cust.IsDirect.Value)
                        else
                        {
                            customer.IsDirect = true;
                        }
                        if (customer.SalesDate.HasValue)
                        {
                            customer.SalesDate = customer.SalesDate.Value.ClientToUTCTime();
                        }
                        if (customer.InstallDate.HasValue)
                        {
                            customer.InstallDate = customer.InstallDate.Value.ClientToUTCTime();
                        }
                        if (customer.CutInDate.HasValue)
                        {
                            customer.CutInDate = customer.CutInDate.Value.ClientToUTCTime();
                        }
                        if (customer.FundingDate.HasValue)
                        {
                            customer.FundingDate = customer.FundingDate.Value.ClientToUTCTime();
                        }

                        if (string.IsNullOrEmpty(customer.MonthlyMonitoringFee) && customer.PaymentMethod != "-1")
                        {
                            return Json(new
                            {
                                status = false,
                                customerid = cust.Id,
                                message = "Your monitoring fee have not set yet."
                            });
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(customer.MonthlyMonitoringFee))
                            {
                                var MonitoringFee = Convert.ToDouble(customer.MonthlyMonitoringFee) + customer.PaymentIncress;
                                customer.MonthlyMonitoringFee = MonitoringFee.ToString();
                                customer.BillAmount = customer.BillAmount + customer.PaymentIncress;
                            }
                            _Util.Facade.CustomerDraftFacade.UpdateCustomerDraft(customer);
                        }


                        #endregion



                        if (systemInfo.Id > 0)
                        {
                            CustomerSystemInfo cussys = _Util.Facade.CustomerSystemInfoFacade.GetCustomerSystemInfoById(systemInfo.Id);
                            if (cussys != null)
                            {
                                CustomerSystemInfoDraft cussysDraft = _Util.Facade.CustomerSystemInfoFacade.GetCustomerSystemInfoDraftByCustomerIdAndCompanyId(cussys.CustomerId, cussys.CompanyId);
                                if (cussysDraft != null)
                                {
                                    systemInfo.CompanyId = currentLoggedIn.CompanyId.Value;
                                    systemInfo.CustomerId = cussysDraft.CustomerId;
                                    systemInfo.Id = cussysDraft.Id;
                                    _Util.Facade.CustomerSystemInfoFacade.UpdateSystemInfoDraft(systemInfo);
                                }
                                else
                                {
                                    CustomerSystemInfoDraft objsystemInfo = new CustomerSystemInfoDraft()
                                    {
                                        CustomerId = customer.CustomerId,
                                        CompanyId = currentLoggedIn.CompanyId.Value,
                                        PanelType = systemInfo.PanelType,
                                        InstallType = systemInfo.InstallType,
                                        CellularBackup = systemInfo.CellularBackup,
                                        Zone1 = systemInfo.Zone1,
                                        Zone2 = systemInfo.Zone2,
                                        Zone3 = systemInfo.Zone3,
                                        Zone4 = systemInfo.Zone4,
                                        Zone5 = systemInfo.Zone5,
                                        Zone6 = systemInfo.Zone6,
                                        Zone7 = systemInfo.Zone7,
                                        Zone8 = systemInfo.Zone8,
                                        Zone9 = systemInfo.Zone9
                                    };
                                    _Util.Facade.CustomerSystemInfoFacade.InsertSystemInfoDraft(objsystemInfo);
                                }
                            }
                            else
                            {
                                CustomerSystemInfoDraft objsystemInfo = new CustomerSystemInfoDraft()
                                {
                                    CustomerId = customer.CustomerId,
                                    CompanyId = currentLoggedIn.CompanyId.Value,
                                    PanelType = systemInfo.PanelType,
                                    InstallType = systemInfo.InstallType,
                                    CellularBackup = systemInfo.CellularBackup,
                                    Zone1 = systemInfo.Zone1,
                                    Zone2 = systemInfo.Zone2,
                                    Zone3 = systemInfo.Zone3,
                                    Zone4 = systemInfo.Zone4,
                                    Zone5 = systemInfo.Zone5,
                                    Zone6 = systemInfo.Zone6,
                                    Zone7 = systemInfo.Zone7,
                                    Zone8 = systemInfo.Zone8,
                                    Zone9 = systemInfo.Zone9
                                };
                                _Util.Facade.CustomerSystemInfoFacade.InsertSystemInfoDraft(objsystemInfo);
                            }

                        }
                        else
                        {
                            CustomerSystemInfoDraft objsystemInfo = new CustomerSystemInfoDraft()
                            {
                                CustomerId = customer.CustomerId,
                                CompanyId = currentLoggedIn.CompanyId.Value,
                                PanelType = systemInfo.PanelType,
                                InstallType = systemInfo.InstallType,
                                CellularBackup = systemInfo.CellularBackup,
                                Zone1 = systemInfo.Zone1,
                                Zone2 = systemInfo.Zone2,
                                Zone3 = systemInfo.Zone3,
                                Zone4 = systemInfo.Zone4,
                                Zone5 = systemInfo.Zone5,
                                Zone6 = systemInfo.Zone6,
                                Zone7 = systemInfo.Zone7,
                                Zone8 = systemInfo.Zone8,
                                Zone9 = systemInfo.Zone9
                            };
                            _Util.Facade.CustomerSystemInfoFacade.InsertSystemInfoDraft(objsystemInfo);
                        }
                        bool PaymentInfoIsNull = true;
                        if (!string.IsNullOrWhiteSpace(PaymentInfo.BankAccountType) && !string.IsNullOrWhiteSpace(PaymentInfo.RoutingNo) && !string.IsNullOrWhiteSpace(PaymentInfo.AcountNo))
                        {
                            //ACH checking
                            PaymentInfoIsNull = false;
                        }
                        else if (!string.IsNullOrWhiteSpace(PaymentInfo.CardNumber) && !string.IsNullOrWhiteSpace(PaymentInfo.CardSecurityCode) && !string.IsNullOrWhiteSpace(PaymentInfo.CardExpireDate))
                        {
                            //CreditCard checking
                            PaymentInfoIsNull = false;
                        }
                        #region Customer Payment Info part
                        if (!PaymentInfoIsNull)
                        {
                            if (PaymentInfo.Id > 0)
                            {
                                //if payment info changes what will happen to arb?

                                if (customer.PaymentMethod == LabelHelper.PaymentMethod.CreditCard && PaymentInfo.CardNumber.IndexOf("******") > -1 && PaymentInfo.Id > 0)
                                {
                                    PaymentInfo tmpPaymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfo_Card_ByCompanyIdAndCustomerId(currentLoggedIn.CompanyId.Value, customer.CustomerId);
                                    PaymentInfo.CardNumber = tmpPaymentInfo.CardNumber;
                                }

                                PaymentInfo.CompanyId = currentLoggedIn.CompanyId.Value;
                                PaymentInfo.IsCash = false;
                                _Util.Facade.PaymentInfoFacade.UpdatePaymentInfo(PaymentInfo);

                            }
                            else
                            {
                                PaymentInfo.CompanyId = currentLoggedIn.CompanyId.Value;
                                PaymentInfo objInfoPay = new PaymentInfo()
                                {
                                    CompanyId = PaymentInfo.CompanyId,
                                    AccountName = PaymentInfo.AccountName,
                                    BankAccountType = PaymentInfo.BankAccountType,
                                    RoutingNo = PaymentInfo.RoutingNo,
                                    AcountNo = PaymentInfo.AcountNo,
                                    CardNumber = PaymentInfo.CardNumber,
                                    CardExpireDate = PaymentInfo.CardExpireDate,
                                    CardSecurityCode = PaymentInfo.CardSecurityCode,
                                    EcheckType = PaymentInfo.EcheckType,
                                    BankName = PaymentInfo.BankName

                                };
                                _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(objInfoPay);
                                PaymentInfoCustomerDraft pic = new PaymentInfoCustomerDraft()
                                {
                                    CompanyId = currentLoggedIn.CompanyId.Value,
                                    CustomerId = customer.CustomerId,
                                    PaymentInfoId = objInfoPay.Id,
                                    Payfor = "MMR"
                                };
                                pic.Id = (int)_Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomerDraft(pic);
                            }
                            #endregion Customer Payment Info part
                        }
                    }
                    Notification notification1 = new Notification()
                    {
                        CompanyId = currentLoggedIn.CompanyId.Value,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        NotificationId = Guid.NewGuid(),
                        Type = LabelHelper.NotificationType.Employee,
                        Who = currentLoggedIn.UserId,
                        What = string.Format(@"{0} Updated his profile ", currentLoggedIn.FirstName + " " + currentLoggedIn.LastName),
                        NotificationUrl = AppConfig.DomainSitePath + "/CustomerPublic/ShowCustomerChange?CustomerId=" + currentLoggedIn.UserId
                    };
                    _Util.Facade.NotificationFacade.InsertNotification(notification1);
                    List<Employee> emp1 = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Admin, new Guid());
                    foreach (var item in emp1)
                    {
                        NotificationUser nu = new NotificationUser()
                        {
                            NotificationId = notification1.NotificationId,
                            IsRead = false,
                            NotificationPerson = item.UserId,
                        };
                        _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                    }
                    return Json(new
                    {
                        status = result,
                        customerid = customer.Id,
                        tab = nexttab,
                        PaymentInfoId = PaymentInfo.Id,
                        PaymentMethod = customer.PaymentMethod,
                        AuthId = customer.AuthorizeRefId,
                        SyncRequired = customer.IsRequiredCsvSync
                        //HasMessage = HasAuthorizeMessage,
                        //AuthMessage = AuthorizeMessage
                    });

                }

                #endregion


            }
            catch (Exception ex)
            {
                result = false;
            }
            return Json(new
            {
                status = false,
                result = true
            });

        }

        public ActionResult ShowCustomerChange(Guid CustomerId)
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ViewBag.CustomerId = CustomerId;
            Customer customer = _Util.Facade.CustomerFacade.GetAllCustomerByCustomerId(CustomerId);
            CustomerDraft customerDraft = _Util.Facade.CustomerDraftFacade.GetCustomerDraftByCustomerId(CustomerId);
            CustomerSystemInfo cusSysinfo = _Util.Facade.CustomerSystemInfoFacade.GetCustomerSystemInfoByCustomerIdAndCompanyId(CustomerId, currentLoggedIn.CompanyId.Value);
            CustomerSystemInfoDraft cusSysinfoDraft = _Util.Facade.CustomerSystemInfoFacade.GetCustomerSystemInfoDraftByCustomerIdAndCompanyId(CustomerId, currentLoggedIn.CompanyId.Value);
            EmergencyContact em = _Util.Facade.EmergencyContactFacade.GetEmergencyContactByCustomerIdAndCompanyId(CustomerId, currentLoggedIn.CompanyId.Value);
            EmergencyContactDraft emDraft = _Util.Facade.EmergencyContactFacade.GetEmergencyContactDraftByCustomerIdAndCompanyId(CustomerId, currentLoggedIn.CompanyId.Value);
            if (cusSysinfo == null)
            {
                cusSysinfo = new CustomerSystemInfo();
            }
            if (cusSysinfoDraft == null)
            {
                cusSysinfoDraft = new CustomerSystemInfoDraft();
            }
            if (em == null)
            {
                em = new EmergencyContact();
            }
            if (emDraft == null)
            {
                ViewBag.NewContact = "Null";
                emDraft = new EmergencyContactDraft();
            }
            else
            {
                ViewBag.NewContact = "";
            }
            CustomerChangeField customerChange = new CustomerChangeField();
            customerChange.Customer = customer;
            customerChange.CustomerDraft = customerDraft;
            customerChange.customerSysinfo = cusSysinfo;
            customerChange.customerSysinfoDraft = cusSysinfoDraft;
            customerChange.emContact = em;
            customerChange.emContactDraft = emDraft;
            return View(customerChange);
        }
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
        public ActionResult ShowCustomerAnnouncement()
        {
            List<Announcement> ancList = _Util.Facade.CustomerFacade.GetAllAnnouncement();
            List<Announcement> activeList = new List<Announcement>();
            ViewBag.Announcement = "";
            foreach (var item in ancList)
            {
                if ((item.StartTime < DateTime.Now && item.EndTime > DateTime.Now) || (item.StartTime.Date == DateTime.Now.Date || item.EndTime.Date == DateTime.Now.Date))
                {
                    activeList.Add(item);


                }
            }
            return View(activeList);
        }
        public ActionResult ShowReferedFriend()
        {
            List<CustomerRefer> referList = _Util.Facade.CustomerDraftFacade.GetAllReferedFriend();
            ViewBag.FriendCount = referList.Count;
            return View(referList);
        }
        private void CustomerListInit()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            List<SelectListItem> LeadSource = new List<SelectListItem>();
            LeadSource.AddRange(_Util.Facade.LookupFacade.GetLookupByKeyWithParent("LeadSource").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.LeadSource = LeadSource;
            ViewBag.ECheckType = _Util.Facade.LookupFacade.GetLookupByKey("ECheckType").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
            ViewBag.BankAccountType = _Util.Facade.LookupFacade.GetLookupByKey("BankAccountType").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
            ViewBag.PaymentMethod = _Util.Facade.LookupFacade.GetLookupByKey("PaymentMethod").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();

            ViewBag.CustomerTypeList = _Util.Facade.LookupFacade.GetLookupByKey("CustomerType").Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();

            ViewBag.YesNoList = _Util.Facade.LookupFacade.GetLookupByKey("YesNo").Select(x =>
                                new SelectListItem()
                                {
                                    Text = x.DisplayText.ToString(),
                                    Value = x.DataValue
                                }).ToList();

            ViewBag.TimeToCall = _Util.Facade.LookupFacade.GetLookupByKey("TimeToCall").Select(x =>
                              new SelectListItem()
                              {
                                  Text = x.DisplayText.ToString(),
                                  Value = x.DataValue.ToString()
                              }).ToList();

            ViewBag.CreditScore = _Util.Facade.LookupFacade.GetLookupByKey("CreditScore").Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.DisplayText.ToString(),
                                 Value = x.DataValue.ToString()
                             }).ToList();

            if (CurrentUser.UserTags.ToLower() == "admin")
            {
                ViewBag.ContractTerm = _Util.Facade.LookupFacade.GetLookupByKey("ContractTerm").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            }
            else
            {
                ViewBag.ContractTerm = _Util.Facade.LookupFacade.GetLookupByKey("SalesContractTerm").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            }
            ViewBag.StateList = _Util.Facade.LookupFacade.GetLookupByKey("StateList").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
            ViewBag.PaymentMethod = _Util.Facade.LookupFacade.GetLookupByKey("PaymentMethod").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            ViewBag.BillCycle = _Util.Facade.LookupFacade.GetLookupByKey("BillCycle").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            ViewBag.EmployeeName = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value).Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                                      Value = x.UserId.ToString()
                                  }).ToList();
            List<SelectListItem> SalesPersonList = new List<SelectListItem>();
            SalesPersonList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = ""
            });
            List<Employee> EmployeeDropdown = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentUser.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid());
            if (EmployeeDropdown != null && EmployeeDropdown.Count > 0)
            {
                SalesPersonList.AddRange(EmployeeDropdown.OrderBy(x => x.FirstName).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.FirstName + " " + x.LastName,
                               Value = x.UserId.ToString()
                           }).ToList());
            }

            ViewBag.SalesPersonList = SalesPersonList;


            List<SelectListItem> InstallerList = new List<SelectListItem>();
            InstallerList.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = ""
            });

            List<Employee> EmployeeDropdown2 = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Installer, new Guid());
            if (EmployeeDropdown2 != null && EmployeeDropdown2.Count > 0)
            {

                InstallerList.AddRange(EmployeeDropdown2.OrderBy(x => x.FirstName).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.FirstName + " " + x.LastName,
                               Value = x.UserId.ToString()
                           }).ToList());
                ViewBag.InstallerList = InstallerList;


                List<SelectListItem> FundingCompany = new List<SelectListItem>();
                FundingCompany.Add(new SelectListItem() { Text = "Please Select One", Value = "-1" });
                List<FundingCompany> FundingCompanyDropDown = _Util.Facade.FundFacade.GetAllFundingCompany();
                if (FundingCompanyDropDown != null && FundingCompanyDropDown.Count > 0)
                {
                    FundingCompany.AddRange(FundingCompanyDropDown.OrderBy(x => x.Name).Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.Name.ToString(),
                                 Value = x.Value.ToString()
                             }).ToList());
                }

                ViewBag.FundingCompany = FundingCompany;

                ViewBag.BillYesNo = _Util.Facade.LookupFacade.GetLookupByKey("BillYesNo").Select(x =>
                                 new SelectListItem()
                                 {
                                     Text = x.DisplayText.ToString(),
                                     Value = x.DataValue.ToString()
                                 }).ToList();
            }
        }
    }
}