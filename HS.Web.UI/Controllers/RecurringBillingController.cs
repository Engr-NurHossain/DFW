using ClosedXML.Excel;
using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.Helper;
using Newtonsoft.Json;
using NLog;
using OS.AWS.S3.Services;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Mvc;
using Excel = HS.Web.UI.Helper.ExcelFormatHelper;
using PermissionChecker = HS.Web.UI.Helper.PermissionHelper;
using PermissionList = HS.Framework.UserPermissions;

namespace HS.Web.UI.Controllers
{
    public class RecurringBillingController : BaseController
    {
        public RecurringBillingController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        public ActionResult RecurringBillingPartial(int? CustomerId)
        {
            ViewBag.CustomerId = CustomerId;
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


            string PtoFilter = "-1";

            ViewBag.PTOFilter = _Util.Facade.LookupFacade.GetLookupByKey("PTOFilterOptions").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString(),
                   Selected = x.DataValue == PtoFilter
               }).ToList();

            ViewBag.FirstDayOfWeek = FirstDayOfWeek;

            return PartialView("RecurringBillingPartial");
        }
        [Authorize]
        [HttpGet]
        public ActionResult AddRecurringBilling(int? id, int? CustomerId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ReurringBillingScheduleModel model = new ReurringBillingScheduleModel();
            var SalesTaxPercent = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("Sales Tax");
            if (SalesTaxPercent != null)
            {
                model.SalesTax = SalesTaxPercent.Value.ToFloat();
            }
            if ((id.HasValue && id.Value > 0))
            {
                if (id.HasValue && id > 0)
                {
                    model.Schedule = _Util.Facade.CustomerFacade.GetByRecurringBillingID(id.Value);
                }
                if (model.Schedule == null || model.Schedule.CompanyId != currentLoggedIn.CompanyId.Value)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                else
                {
                    model.Schedule.StartDate = model.Schedule.StartDate;
                    if (model.Schedule.EndDate != null)
                    {
                        DateTime End = (DateTime)model.Schedule.EndDate;
                        model.Schedule.EndDate = End;
                    }
                }

                model.Customer = _Util.Facade.CustomerFacade.GetAllCustomerByCustomerId(model.Schedule.CustomerId);
                if (model.Customer == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model.ScheduleItems = _Util.Facade.CustomerFacade.GetRecurringBillingScheduleItemsByScheduleId(model.Schedule.ScheduleId);

                model.Schedule.ShowLineItem = false;
                model.Schedule.ShowDayInAdvance = false;
                model.Schedule.ShowBillDate = false;
                model.Schedule.ShowAddNewLineItemButton = true;
                string DaysInAdvance = _Util.Facade.GlobalSettingsFacade.GetExcelFormat("DayInAdvanceShowEnable", currentLoggedIn.CompanyId.Value);
                if (DaysInAdvance.ToLower() == "true")
                {
                    model.Schedule.ShowDayInAdvance = true;
                }
                string BillDateSettings = _Util.Facade.GlobalSettingsFacade.GetExcelFormat("BillDateShowEnable", currentLoggedIn.CompanyId.Value);
                if (BillDateSettings.ToLower() == "true")
                {
                    model.Schedule.ShowBillDate = true;
                }
                string LineItems = _Util.Facade.GlobalSettingsFacade.GetExcelFormat("LineItemShowEnable", currentLoggedIn.CompanyId.Value);
                if (LineItems.ToLower() == "true")
                {
                    model.Schedule.ShowLineItem = true;
                }
                if (model.Schedule.PaymentCollectionDate != null && model.Schedule.PaymentCollectionDate.HasValue)
                {
                    model.BillDay = Convert.ToInt32(model.Schedule.PaymentCollectionDate.Value.ToString("dd"));
                }
            }
            else
            {
                if (CustomerId.HasValue && CustomerId > 0)
                {
                    model.Customer = _Util.Facade.CustomerFacade.GetById(CustomerId.Value);
                }

                if (model.Customer == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }

                model.Schedule = new RecurringBillingSchedule()
                {
                    ScheduleId = Guid.NewGuid(),
                    CustomerId = model.Customer.CustomerId,
                    CustomerIntId = model.Customer.Id,
                    CompanyId = currentLoggedIn.CompanyId.Value,
                    //TemplateName = "Recurring Billing",
                    EmailAddress = model.Customer.EmailAddress,
                    CCEmail = "",
                    BCCEmail = "",
                    Status = "Init",
                    CollectOnline = false,
                    CustomerPaymentProfileId = 0,
                    BillCycle = "",
                    DayInAdvance = 0,
                    Interval = 0,
                    BillAmount = 0,
                    TaxAmount = 0,
                    TaxPercentage = model.SalesTax,
                    TotalBillAmount = 0,
                    MessageOnInvoice = "",
                    StartDate = DateTime.UtcNow.UTCToClientTime().AddDays(1),
                    //PaymentCollectionDate = DateTime.Now,
                    //PreviousDate = DateTime.Now.UTCCurrentTime(),
                    NextDate = DateTime.UtcNow.UTCToClientTime(),
                    Id = 0

                };
                model.Schedule.IncludeOpenInvoices = false;
                model.Schedule.OthersUnpaidBill = false;
                //model.Schedule.AutomaticallySendEmail = false;
                model.Schedule.IsEInvoice = false;
                model.Schedule.IsEReceipt = false;
                model.Schedule.ShowLineItem = false;
                model.Schedule.ShowDayInAdvance = false;
                model.Schedule.ShowAddNewLineItemButton = false;
                model.Schedule.ShowBillDate = false;
                List<GlobalSetting> RecurringAllGlobalSettings = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsForRecurringByCompanyId(currentLoggedIn.CompanyId.Value);
                if (RecurringAllGlobalSettings != null)
                {
                    foreach (var settings in RecurringAllGlobalSettings)
                    {
                        bool settingValue = false;
                        bool ParseResult = bool.TryParse(settings.Value, out settingValue);
                        if (ParseResult)
                        {
                            if (settings.SearchKey == "RecurringUnpaidBillIncludeEnable")
                            {
                                model.Schedule.IncludeOpenInvoices = settingValue;
                            }
                            else if (settings.SearchKey == "OthersUnpaidBillIncludeEnable")
                            {
                                model.Schedule.OthersUnpaidBill = settingValue;
                            }
                            //else if (settings.SearchKey == "RecurringBillingEmailSendEnable")
                            //{
                            //    model.Schedule.AutomaticallySendEmail = settingValue;
                            //}
                            else if (settings.SearchKey == "EInvoiceEnable")
                            {
                                model.Schedule.IsEInvoice = settingValue;
                            }
                            else if (settings.SearchKey == "EReceiptEnable")
                            {
                                model.Schedule.IsEReceipt = settingValue;
                            }
                            else if (settings.SearchKey == "DayInAdvanceShowEnable")
                            {
                                model.Schedule.ShowDayInAdvance = settingValue;
                            }
                            else if (settings.SearchKey == "BillDateShowEnable")
                            {
                                model.Schedule.ShowBillDate = settingValue;
                            }
                            else if (settings.SearchKey == "LineItemShowEnable")
                            {
                                model.Schedule.ShowLineItem = settingValue;
                            }
                            else if (settings.SearchKey == "RMRAddNewLineItemButtonShowEnable")
                            {
                                model.Schedule.ShowAddNewLineItemButton = settingValue;
                            }
                        }
                    }
                }
                var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(currentLoggedIn.CompanyId.Value);
                model.Schedule.BillingAddress = AddressHelper.MakeCustomerAddress(model.Customer, "BillingAddress", AddressTemplate);
                //model.Schedule.Id = _Util.Facade.CustomerFacade.InsertRecurringBillingSchedule(model.Schedule);
                model.ScheduleItems = new List<RecurringBillingScheduleItems>();
                //model.Schedule.ScheduleId = model.Schedule.ScheduleId.GenerateScheduleNo();
                //_Util.Facade.CustomerFacade.UpdateRecurringBillingSchedule(model.Schedule);
            }
            #region Dropdown
            ViewBag.Cycle = _Util.Facade.LookupFacade.GetLookupByKey("BillCycle").Where(x => x.IsActive == true && x.DataOrder > -1).Select(x =>
           new SelectListItem()
           {
               Text = x.DisplayText.ToString(),
               Value = x.DataValue.ToString()
           }).ToList();
            List<SelectListItem> ProfilePackage = new List<SelectListItem>();
            //ProfilePackage.Add(new SelectListItem()
            //{
            //    Text = "Please Select",
            //    Value = "-1"
            //});
            ProfilePackage.Add(new SelectListItem()
            {
                Text = "Invoice",
                Value = "Invoice"
            });
            List<PaymentProfileCustomer> PaymentInfoList = _Util.Facade.CustomerFacade.GetAllPaymentProfileByCustomerId(model.Customer.CustomerId, currentLoggedIn.CompanyId.Value);
            PaymentInfoList = PaymentInfoList.Where(x => x.Type != "Invoice").ToList();
            ProfilePackage.AddRange(PaymentInfoList.Select(x =>
             new SelectListItem()
             {
                 Text = x.Type.ToString(),
                 Value = x.PaymentInfoId.ToString()
             }).ToList());

            ViewBag.PaymentMethod = ProfilePackage;//.Where(x => x.Text != "OnFile").OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();

            //if(model.Schedule.PaymentMethod == "Invoice")
            //{
            //    model.Schedule.CustomerPaymentProfileId = 0;
            //}
            ViewBag.Status = _Util.Facade.LookupFacade.GetLookupByKey("RecurringBillingStatus").Where(x => x.IsActive == true && x.DataOrder > -1).Select(x =>
           new SelectListItem()
           {
               Text = x.DisplayText.ToString(),
               Value = x.DataValue.ToString()
           }).ToList();

            #region View for TaxList
            Customer tempCustomer = model.Customer;

            if (tempCustomer != null)
            {
                List<SelectListItem> TaxListItem = new List<SelectListItem>();
                var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(tempCustomer.CustomerId, currentLoggedIn.CompanyId.Value);
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
                    if (tempCustomer != null)
                    {
                        tempCustomerId = tempCustomer.CustomerId;
                    }

                    var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(currentLoggedIn.CompanyId.Value, tempCustomerId);
                    if (GetSalesTax != null)
                    {
                        TaxListItem.Add(new SelectListItem()
                        {
                            Text = GetSalesTax.SearchKey.ToString(),
                            Value = GetSalesTax.Value.ToString()
                        });
                    }
                }
                //TaxListItem.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("NonTaxValue").Select(x => new SelectListItem()
                //{
                //    Text = x.DisplayText.ToString(),
                //    Value = x.DataValue.ToString()
                //}).ToList());
                //var GetOutOfStateTax = _Util.Facade.GlobalSettingsFacade.GetOutOfStateTax(currentLoggedIn.CompanyId.Value);
                //if (GetOutOfStateTax != null)
                //{
                //    TaxListItem.Add(new SelectListItem()
                //    {
                //        Text = GetOutOfStateTax.SearchKey.ToString(),
                //        Value = GetOutOfStateTax.Value.ToString()
                //    });
                //}
                //var GetNonProfitTax = _Util.Facade.GlobalSettingsFacade.GetNonProfitTax(currentLoggedIn.CompanyId.Value);
                //if (GetNonProfitTax != null)
                //{
                //    TaxListItem.Add(new SelectListItem()
                //    {
                //        Text = GetNonProfitTax.SearchKey.ToString(),
                //        Value = GetNonProfitTax.Value.ToString()
                //    });
                //}
                TaxListItem.Add(new SelectListItem()
                {
                    Text = "Custom",
                    Value = "Custom"
                });
                ViewBag.TaxListItem = TaxListItem;
                #endregion
            }
            ViewBag.Today = DateTime.Now.ToString("yyyy/MM/dd");
            #endregion
            if (model.Schedule.CustomerPaymentProfileId > 0)
            {
                model.Schedule.PaymentMethod = model.Schedule.CustomerPaymentProfileId.ToString();
            }
            var rmrbillingmethod = _Util.Facade.LookupFacade.GetLookupByKey("RMRBillingMethod")
                 .Where(x => x.DataValue.ToString() != "-1")
                 .Select(x => new
                 {
                     Text = x.DisplayText.ToString(),
                     Value = x.DataValue.ToString()
                 })
                 .ToList();

            ViewBag.RMRBillingMethod = rmrbillingmethod;
            //var Customer = _Util.Facade.CustomerFacade.GetCustomerById(model.Customer.CustomerId);
            string customerName = model.Customer.FirstName + " " + model.Customer.LastName;
            if (!string.IsNullOrWhiteSpace(model.Customer.DBA))
            {
                customerName = model.Customer.DBA;
            }
            else if (!string.IsNullOrWhiteSpace(model.Customer.BusinessName))
            {
                customerName = model.Customer.BusinessName;
            }
            if (model.Schedule.PaymentCollectionDate.HasValue)
            {
                int DayNumber = Convert.ToInt32(model.Schedule.PaymentCollectionDate.Value.ToString("dd"));
                if (DayNumber < 29 && DayNumber > 0) { model.BillDay = DayNumber; }
                else { model.BillDay = 28; }
            }
            ViewBag.BillingDay = _Util.Facade.LookupFacade.GetDropdownsByKey("BillingDay");
            ViewBag.today = DateTime.UtcNow.UTCToClientTime().SetZeroHour();
            model.Schedule.CustomerName = customerName;
            model.Schedule.CustomerIntId = model.Customer.Id;
            var checkinvoiceitem = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(model.Schedule.LastRMRInvoiceRefId);
            ViewBag.CheckInvoice = checkinvoiceitem;


            return PartialView("AddRecurringBilling", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddRecurringBilling(ReurringBillingScheduleModel Model)
        {

            bool updateflag = true;
            string Message = "";
            string TempateName = "Recurring Billing";
            if (!string.IsNullOrWhiteSpace(Model.Schedule.TemplateName) && !string.IsNullOrWhiteSpace(Model.Schedule.TemplateName))
            {
                TempateName = Model.Schedule.TemplateName;
            }
            #region Validations            
            if (Model == null || Model.Schedule == null)
            {
                return Json(new { result = false, message = "Billing template not found." });
            }
            if (Model.Schedule.StartDate != null && Model.Schedule.PaymentCollectionDate != null && Model.Schedule.StartDate > Model.Schedule.PaymentCollectionDate)
            {
                return Json(new { result = false, message = "The payment collection date must be greater than or equal to the start date." });
            }
            if (Model.Schedule.StartDate != null && Model.Schedule.EndDate != null && Model.Schedule.StartDate >= Model.Schedule.EndDate)
            {
                return Json(new { result = false, message = "The end date must be greater than the Start date." });
            }
            if (Model.Schedule.EndDate != null && Model.Schedule.PaymentCollectionDate != null && Model.Schedule.EndDate < Model.Schedule.PaymentCollectionDate)
            {
                return Json(new { result = false, message = "The end date must be greater than or equal to the payment collection date." });
            }
            if (Model.Schedule.PaymentMethod.ToLower() != "invoice" && Model.Schedule.CustomerPaymentProfileId != null && Model.Schedule.CustomerPaymentProfileId < 1)
            {
                return Json(new { result = false, message = "Please add payment method." });
            }
            Customer _customer = _Util.Facade.CustomerFacade.GetAllCustomerByCustomerId(Model.Schedule.CustomerId);
            if (_customer == null)
            {
                return Json(new { result = false, message = "Customer not found." });
            }
            #endregion
            if (Model.Schedule.CustomerPaymentProfileId > 0)
            {
                Model.Schedule.PaymentMethod = _Util.Facade.PaymentInfoFacade.GetPaymentProfileCustomerByPaymentInfoId((int)Model.Schedule.CustomerPaymentProfileId);
            }
            RecurringBillingSchedule StoreData = _Util.Facade.CustomerFacade.GetByRecurringBillingScheduleID(Model.Schedule.ScheduleId);
            RecurringBillingSchedule tempSchedule = _Util.Facade.CustomerFacade.GetByRecurringBillingScheduleID(Model.Schedule.ScheduleId);
            //tempSchedule = StoreData;
            if (tempSchedule == null)
            {
                updateflag = false;
                tempSchedule = new RecurringBillingSchedule();
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //int Interval = Model.Schedule.Interval;
            DateTime? StartDate = Model.Schedule.StartDate;
            if (StartDate != null)
            {
                StartDate = Convert.ToDateTime(StartDate.Value.ToString("yyyy/MM/dd"));
            }
            DateTime CurrentDate = DateTime.UtcNow;
            DateTime? EndDate = null;
            if (!string.IsNullOrWhiteSpace(Model.Schedule.EndDate.ToString()))
            {
                EndDate = Model.Schedule.EndDate.Value;
                EndDate = Convert.ToDateTime(EndDate.Value.ToString("yyyy/MM/dd"));
            }

            int DayInAdvance = 0;
            if (Model.Schedule.DayInAdvance != null && Model.Schedule.DayInAdvance > 0)
            {
                DayInAdvance = Model.Schedule.DayInAdvance.Value;
            }
            if (Model.Schedule.PaymentMethod == "0")
            {
                Model.Schedule.CollectOnline = false;
            }

            #region Update Schedule
            tempSchedule.TemplateName = !string.IsNullOrWhiteSpace(TempateName) ? TempateName : "";
            tempSchedule.DayInAdvance = DayInAdvance;
            tempSchedule.EmailAddress = !string.IsNullOrWhiteSpace(Model.Schedule.EmailAddress) ? Model.Schedule.EmailAddress : "";
            tempSchedule.BCCEmail = !string.IsNullOrWhiteSpace(Model.Schedule.BCCEmail) ? Model.Schedule.BCCEmail : "";
            tempSchedule.CCEmail = !string.IsNullOrWhiteSpace(Model.Schedule.CCEmail) ? Model.Schedule.CCEmail : "";
            tempSchedule.AutomaticallySendEmail = Model.Schedule.AutomaticallySendEmail;
            tempSchedule.IncludeOpenInvoices = Model.Schedule.IncludeOpenInvoices;
            tempSchedule.OthersUnpaidBill = Model.Schedule.OthersUnpaidBill;
            tempSchedule.CollectOnline = Model.Schedule.CollectOnline;
            tempSchedule.CustomerPaymentProfileId = Model.Schedule.CustomerPaymentProfileId;
            tempSchedule.PaymentMethod = !string.IsNullOrWhiteSpace(Model.Schedule.PaymentMethod) ? Model.Schedule.PaymentMethod : "";
            tempSchedule.BillCycle = !string.IsNullOrWhiteSpace(Model.Schedule.BillCycle) ? Model.Schedule.BillCycle : "";
            tempSchedule.IsEInvoice = Model.Schedule.IsEInvoice;
            tempSchedule.IsEReceipt = Model.Schedule.IsEReceipt;
            tempSchedule.IsReplacement = Model.Schedule.IsReplacement;
            tempSchedule.IsFCReplacement = Model.Schedule.IsFCReplacement;
            tempSchedule.IsPOO = Model.Schedule.IsPOO;
            tempSchedule.IsTransfer = Model.Schedule.IsTransfer;
            //tempSchedule.Interval = Model.Schedule.Interval;
            tempSchedule.StartDate = StartDate;
            tempSchedule.EndDate = EndDate;
            tempSchedule.BillingAddress = !string.IsNullOrWhiteSpace(Model.Schedule.BillingAddress) ? Model.Schedule.BillingAddress : "";
            //tempSchedule.Status = Model.Schedule.Status;
            tempSchedule.BillAmount = Model.Schedule.BillAmount;
            tempSchedule.TaxPercentage = Model.Schedule.TaxPercentage;
            tempSchedule.TaxAmount = Model.Schedule.TaxAmount;
            tempSchedule.TotalBillAmount = Model.Schedule.TotalBillAmount;
            tempSchedule.MessageOnInvoice = !string.IsNullOrWhiteSpace(Model.Schedule.MessageOnInvoice) ? Model.Schedule.MessageOnInvoice : "";
            tempSchedule.LastUpdatedBy = CurrentUser.UserId;
            tempSchedule.LastUpdatedDate = CurrentDate;


            if (updateflag)
            {
                if (tempSchedule.PreviousDate == null)
                {
                    if (Model.Schedule.DayInAdvance != null && Model.Schedule.DayInAdvance > 0)
                    {
                        tempSchedule.NextDate = Model.Schedule.PaymentCollectionDate;
                    }
                    else
                    {
                        tempSchedule.NextDate = StartDate;
                    }
                    DateTime? newDate = new DateTime();
                    if (Model.Schedule.BillCycle.ToLower() == "monthly")
                    {
                        newDate = Convert.ToDateTime(Model.Schedule.StartDate).AddDays(DayInAdvance).AddMonths(1);
                    }
                    if (Model.Schedule.BillCycle.ToLower() == "quarterly")
                    {
                        newDate = Convert.ToDateTime(Model.Schedule.StartDate).AddDays(DayInAdvance).AddMonths(3);
                    }
                    if (Model.Schedule.BillCycle.ToLower() == "semi-annually")
                    {
                        newDate = Convert.ToDateTime(Model.Schedule.StartDate).AddDays(DayInAdvance).AddMonths(6);
                    }
                    if (Model.Schedule.BillCycle.ToLower() == "annually")
                    {
                        newDate = Convert.ToDateTime(Model.Schedule.StartDate).AddDays(DayInAdvance).AddMonths(12);
                    }
                    tempSchedule.PaymentCollectionDate = Model.Schedule.PaymentCollectionDate;
                    tempSchedule.Status = !string.IsNullOrWhiteSpace(Model.Schedule.Status) ? Model.Schedule.Status : "";
                }
                else
                {
                    DateTime Today = DateTime.Now.SetZeroHour();
                    DateTime NextPayment = Today;
                    DateTime NextInvoice = Today;
                    if (Model.Schedule.PaymentCollectionDate != null && Model.Schedule.StartDate != null && Model.Schedule.PaymentCollectionDate > Model.Schedule.StartDate)
                    {
                        NextInvoice = Model.Schedule.StartDate.Value;
                        NextPayment = Model.Schedule.PaymentCollectionDate.Value;
                    }
                    else
                    {
                        return Json(new { result = false, message = "Please input start date and payment collection date." });
                    }
                    string niDate = NextInvoice.ToString("dd/MM/yyyy");
                    string niDay = niDate.Split('/')[0];
                    string tDate = Today.ToString("dd/MM/yyyy");
                    string tMonth = tDate.Split('/')[1];
                    string tYear = tDate.Split('/')[2];
                    NextInvoice = Convert.ToDateTime(tYear + "/" + tMonth + "/" + niDay);
                    if (Model.Schedule.DayInAdvance != null && Model.Schedule.DayInAdvance > 0)
                    {
                        tempSchedule.NextDate = NextPayment.SetZeroHour();
                    }
                    else
                    {
                        tempSchedule.NextDate = NextInvoice.SetZeroHour();
                    }
                    DateTime? newDate = new DateTime();
                    if (Model.Schedule.BillCycle.ToLower() == "monthly")
                    {
                        newDate = Convert.ToDateTime(tempSchedule.PreviousDate).AddMonths(1);
                        Convert.ToDateTime(newDate).AddDays(DayInAdvance);
                    }
                    if (Model.Schedule.BillCycle.ToLower() == "quarterly")
                    {
                        newDate = Convert.ToDateTime(tempSchedule.PreviousDate).AddMonths(3);
                    }
                    if (Model.Schedule.BillCycle.ToLower() == "semi-annually")
                    {
                        newDate = Convert.ToDateTime(tempSchedule.PreviousDate).AddMonths(6);
                    }
                    if (Model.Schedule.BillCycle.ToLower() == "annually")
                    {
                        newDate = Convert.ToDateTime(tempSchedule.PreviousDate).AddMonths(12);
                    }
                    tempSchedule.PaymentCollectionDate = Model.Schedule.PaymentCollectionDate;
                    tempSchedule.Status = !string.IsNullOrWhiteSpace(Model.Schedule.Status) ? Model.Schedule.Status : "";
                }
            }
            else
            {
                if (Model.Schedule.DayInAdvance.HasValue && Model.Schedule.DayInAdvance.Value > 0)
                {
                    tempSchedule.NextDate = Model.Schedule.PaymentCollectionDate;
                }
                else
                {
                    tempSchedule.NextDate = StartDate;
                }
                tempSchedule.PaymentCollectionDate = Model.Schedule.PaymentCollectionDate;
                tempSchedule.Status = !string.IsNullOrWhiteSpace(Model.Schedule.Status) ? Model.Schedule.Status : "";
            }

            List<RecurringBillingSchedule> list = _Util.Facade.CustomerFacade.GetByRecurringBillingListByCustomerId(Model.Schedule.CustomerId);
            bool customerUpdate = true;
            int ListCount = 0;
            if (list != null && list.Count > 0)
            {
                list.OrderBy(x => x.Id).ToList();
                if (list[0].ScheduleId == tempSchedule.ScheduleId)
                {
                    customerUpdate = true;
                }
                else
                {
                    customerUpdate = false;
                }
                ListCount = list.Count + 1;
            }

            if (updateflag)
            {
                _Util.Facade.CustomerFacade.UpdateRecurringBillingSchedule(tempSchedule);
                Message = "Billing template successfully updated.";
                #region User Log
                string LogMsg = "";
                if (StoreData != null)
                {
                    if (StoreData.TemplateName != tempSchedule.TemplateName) { LogMsg += " template name change " + StoreData.TemplateName.ToLower() + " to " + tempSchedule.TemplateName.ToLower() + ","; }
                    if (StoreData.DayInAdvance != tempSchedule.DayInAdvance) { LogMsg += " day in advance change " + StoreData.DayInAdvance.ToString().ToLower() + " to " + tempSchedule.DayInAdvance.ToString().ToLower() + ","; }
                    if (StoreData.EmailAddress != tempSchedule.EmailAddress) { LogMsg += " email address change " + StoreData.EmailAddress.ToLower() + " to " + tempSchedule.EmailAddress.ToLower() + ","; }
                    if (StoreData.CCEmail != tempSchedule.CCEmail) { LogMsg += " cc email address change " + StoreData.CCEmail.ToLower() + " to " + tempSchedule.CCEmail.ToLower() + ","; }
                    if (StoreData.BCCEmail != tempSchedule.BCCEmail) { LogMsg += " bcc email address change " + StoreData.BCCEmail.ToLower() + " to " + tempSchedule.BCCEmail.ToLower() + ","; }
                    if (StoreData.IsEReceipt != tempSchedule.IsEReceipt) { LogMsg += " e-receipt change " + StoreData.IsEReceipt.ToString().ToLower() + " to " + tempSchedule.IsEReceipt.ToString().ToLower() + ","; }
                    if (StoreData.IsEInvoice != tempSchedule.IsEInvoice) { LogMsg += " e-invoice change " + StoreData.IsEInvoice.ToString().ToLower() + " to " + tempSchedule.IsEInvoice.ToString().ToLower() + ","; }
                    if (StoreData.IsReplacement != tempSchedule.IsReplacement) { LogMsg += " Replacement change " + StoreData.IsReplacement.ToString().ToLower() + " to " + tempSchedule.IsReplacement.ToString().ToLower() + ","; }
                    if (StoreData.IsFCReplacement != tempSchedule.IsFCReplacement) { LogMsg += " FC Replacement change " + StoreData.IsFCReplacement.ToString().ToLower() + " to " + tempSchedule.IsFCReplacement.ToString().ToLower() + ","; }
                    if (StoreData.IsTransfer != tempSchedule.IsTransfer) { LogMsg += " Not For Transfer change " + StoreData.IsTransfer.ToString().ToLower() + " to " + tempSchedule.IsTransfer.ToString().ToLower() + ","; }
                    if (StoreData.IsPOO != tempSchedule.IsPOO) { LogMsg += " NO POO change " + StoreData.IsPOO.ToString().ToLower() + " to " + tempSchedule.IsPOO.ToString().ToLower() + ","; }
                    if (StoreData.IncludeOpenInvoices != tempSchedule.IncludeOpenInvoices) { LogMsg += " include rmr unpaid bills change " + StoreData.IncludeOpenInvoices.ToString().ToLower() + " to " + tempSchedule.IncludeOpenInvoices.ToString().ToLower() + ","; }
                    if (StoreData.OthersUnpaidBill != tempSchedule.OthersUnpaidBill) { LogMsg += " include other unpaid bills change " + StoreData.OthersUnpaidBill.ToString().ToLower() + " to " + tempSchedule.OthersUnpaidBill.ToString().ToLower() + ","; }
                    if (StoreData.PaymentMethod != tempSchedule.PaymentMethod) { LogMsg += " payment method change " + StoreData.PaymentMethod.ToLower() + " to " + tempSchedule.PaymentMethod.ToLower() + ","; }
                    if (StoreData.BillCycle != tempSchedule.BillCycle) { LogMsg += " bill cycle change " + StoreData.BillCycle.ToLower() + " to " + tempSchedule.BillCycle.ToLower() + ","; }
                    if (StoreData.StartDate != tempSchedule.StartDate) { LogMsg += " start date change " + StoreData.StartDate.ToString() + " to " + tempSchedule.StartDate.ToString() + ","; }
                    if (StoreData.EndDate != tempSchedule.EndDate) { LogMsg += " end date change " + StoreData.EndDate.ToString() + " to " + tempSchedule.EndDate.ToString() + ","; }
                    if (StoreData.PaymentCollectionDate != tempSchedule.PaymentCollectionDate) { LogMsg += " bill day change " + StoreData.PaymentCollectionDate.ToString() + " to " + tempSchedule.PaymentCollectionDate.ToString() + ","; }
                    if (StoreData.BillingAddress != tempSchedule.BillingAddress) { LogMsg += " billing address change " + StoreData.BillingAddress.ToLower() + " to " + tempSchedule.BillingAddress.ToLower() + ","; }
                    if (StoreData.Status != tempSchedule.Status) { LogMsg += " billing status change " + StoreData.Status.ToLower() + " to " + tempSchedule.Status.ToLower() + ","; }
                    if (StoreData.MessageOnInvoice != tempSchedule.MessageOnInvoice) { LogMsg += " recurring billing description change " + StoreData.MessageOnInvoice.ToLower() + " to " + tempSchedule.MessageOnInvoice.ToLower() + ","; }
                    //if (StoreData.Status != tempSchedule.Status) { LogMsg += " billing status change " + StoreData.Status.ToLower() + " to " + tempSchedule.Status.ToLower() + ","; }
                    var ItemsList = _Util.Facade.CustomerFacade.GetRecurringBillingScheduleItemsByScheduleId(StoreData.ScheduleId);
                    if (ItemsList != null && ItemsList.Count > 0)
                    {
                        foreach (var item in Model.ScheduleItems)
                        {
                            string LogFlag = "newlog";
                            string strLog = "";
                            foreach (var x in ItemsList)
                            {
                                if (item.ProductName == x.ProductName && Math.Round(item.Amount, 2) == Math.Round(x.Amount, 2))
                                {
                                    LogFlag = "";
                                }
                                else if (item.ProductName == x.ProductName && Math.Round(item.Amount, 2) != Math.Round(x.Amount, 2))
                                {
                                    LogFlag = "log";
                                    strLog = "amount change " + x.Amount.ToString("N2") + " to " + item.Amount.ToString("N2");
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(LogFlag) && !string.IsNullOrWhiteSpace(item.ProductName))
                            {
                                if (LogFlag == "newlog")
                                {
                                    LogMsg += " add new item " + item.ProductName.ToLower() + ",";
                                }
                                else
                                {
                                    LogMsg += " " + item.ProductName.ToLower() + " " + strLog + ",";
                                }
                            }
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(LogMsg))
                {
                    base.AddUserActivityForCustomer(tempSchedule.TemplateName + " template " + LogMsg + " updated by " + CurrentUser.GetFullName(), "AddRecurringBilling,RecurringBilling", _customer.CustomerId, _customer.Id, null, true);
                }
                #endregion
            }
            else
            {
                if (ListCount > 0 && TempateName == "Recurring Billing")
                {
                    tempSchedule.TemplateName = TempateName + " " + ListCount;
                }
                tempSchedule.PreviousDate = null;
                tempSchedule.CompanyId = CurrentUser.CompanyId.Value;
                tempSchedule.ScheduleId = Model.Schedule.ScheduleId;
                tempSchedule.CustomerId = Model.Schedule.CustomerId;
                tempSchedule.CreatedDate = CurrentDate;
                tempSchedule.CreatedBy = CurrentUser.UserId;
                _Util.Facade.CustomerFacade.InsertRecurringBillingSchedule(tempSchedule);
                Message = "Billing template successfully created.";
                base.AddUserActivityForCustomer(tempSchedule.TemplateName + " template successfully created by " + CurrentUser.GetFullName(), "AddRecurringBilling,RecurringBilling", _customer.CustomerId, _customer.Id, null, true);
            }
            #endregion

            #region Update Schedule Items
            _Util.Facade.CustomerFacade.DeleteRecurringBillingScheduleItemsByScheduleId(Model.Schedule.ScheduleId);
            foreach (RecurringBillingScheduleItems item in Model.ScheduleItems)
            {
                if (item.ProductName != null)
                {
                    RecurringBillingScheduleItems ScheduleItems = new RecurringBillingScheduleItems()
                    {
                        ScheduleId = Model.Schedule.ScheduleId,
                        ProductName = item.ProductName,
                        Description = item.Description,
                        Qty = item.Qty,
                        EffectiveDate = item.EffectiveDate,
                        CycleStartDate = item.CycleStartDate,
                        Rate = item.Rate,
                        Amount = item.Amount,
                        IsTaxable = item.IsTaxable,
                        AddedDate = CurrentDate,
                        AddedBy = CurrentUser.UserId
                    };

                    _Util.Facade.CustomerFacade.InsertRecurringBillingScheduleItems(ScheduleItems);
                }

            }
            #endregion

            #region Customer Info Update
            if (customerUpdate)
            {

                _customer.PaymentMethod = Model.Schedule.PaymentMethod;
                _customer.BillCycle = Model.Schedule.BillCycle;
                if (tempSchedule.PreviousDate == null)
                {
                    _customer.FirstBilling = Model.Schedule.StartDate;
                    _customer.InstallDate = Model.Schedule.StartDate;
                }
                _customer.MonthlyMonitoringFee = Model.Schedule.BillAmount.ToString("N2");
                if (Model.Schedule.TaxPercentage > 0) { _customer.BillTax = true; }
                else { _customer.BillTax = false; }

                if (Model.Schedule.IsEInvoice != null && (bool)Model.Schedule.IsEInvoice == true && Model.Schedule.IsEReceipt != null && (bool)Model.Schedule.IsEReceipt == true) { _customer.IsReceivePaymentMail = true; }
                else { _customer.IsReceivePaymentMail = false; }

                _customer.BillAmount = Model.Schedule.TotalBillAmount;
                _customer.LastUpdatedBy = CurrentUser.FirstName + " " + CurrentUser.LastName;
                _customer.LastUpdatedDate = CurrentDate;
                _customer.LastUpdatedByUid = CurrentUser.UserId;
                if (Model.Schedule.PaymentCollectionDate != null && Model.Schedule.PaymentCollectionDate.HasValue)
                {
                    _customer.BillDay = Convert.ToInt32(Model.Schedule.PaymentCollectionDate.Value.ToString("dd"));
                }

                _Util.Facade.CustomerFacade.UpdateCustomer(_customer);
            }
            #endregion

            return Json(new { result = true, message = Message });
        }
        [Authorize]
        public ActionResult RecurringBillingListPartial(int? CustomerId, string SearchText, string Order)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<RecurringBillingSchedule> model = new List<RecurringBillingSchedule>();

            if (CustomerId.HasValue)
            {
                var CustomerInfo = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);
                if (CustomerInfo != null)
                {
                    model = _Util.Facade.CustomerFacade.GetScheduleByCustomerIdAndCompanyId(CustomerInfo.CustomerId, CurrentUser.CompanyId.Value, SearchText, Order);
                }
            }
            return View(model);
        }
        [Authorize]
        public JsonResult CloneRecurringBillingSchedule(ReurringBillingScheduleModel Model)
        {

            #region Validations
            if (Model == null || Model.Schedule == null)
            {
                return Json(new { result = false, message = "Billing not found." });
            }

            RecurringBillingSchedule tempSchedule = _Util.Facade.CustomerFacade.GetByRecurringBillingScheduleID(Model.Schedule.ScheduleId);
            if (tempSchedule == null)
            {
                return Json(new { result = false, message = "Billing not found." });
            }
            #endregion
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            bool clone = _Util.Facade.CustomerFacade.CloneRecurringBilling(Model.Schedule.ScheduleId, CurrentUser.UserId);
            return Json(new { result = true, message = "Billing Clone successfully." });
        }
        [Authorize]
        public JsonResult DeleteRecurringBillingSchedule(ReurringBillingScheduleModel Model)
        {

            #region Validations
            if (Model == null || Model.Schedule == null)
            {
                return Json(new { result = false, message = "Billing not found." });
            }

            RecurringBillingSchedule tempSchedule = _Util.Facade.CustomerFacade.GetByRecurringBillingScheduleID(Model.Schedule.ScheduleId);
            if (tempSchedule == null)
            {
                return Json(new { result = false, message = "Billing not found." });
            }
            #endregion
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            bool Shedule = _Util.Facade.CustomerFacade.DeleteRecurringBillingScheduleByScheduleId(Model.Schedule.ScheduleId);
            bool SheduleItem = _Util.Facade.CustomerFacade.DeleteRecurringBillingScheduleItemsByScheduleId(Model.Schedule.ScheduleId);
            return Json(new { result = true, message = "Billing Delete successfully." });
        }

        [Authorize]
        public PartialViewResult RecurringTempSettingsPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            RecurringBillingTempSettings model = new RecurringBillingTempSettings()
            {
                DaysInAdvanceSetting = false,
                eInvoiceSettings = false,
                eReceiptSettings = false,
                //PaperlessBillsSettings = false,
                RMRUnpaidBillsSettings = false,
                OtherUnpaidBillsSettings = false,
                LineItemsSettings = false,
                AddNewLineItemsButtonSettings = false,
                BillDay = false
            };
            if (CurrentUser != null)
            {
                List<GlobalSetting> RecurringAllGlobalSettings = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsForRecurringByCompanyId(CurrentUser.CompanyId.Value);
                if (RecurringAllGlobalSettings != null && RecurringAllGlobalSettings.Count > 0)
                {
                    foreach (var settings in RecurringAllGlobalSettings)
                    {
                        bool settingValue = false;
                        bool ParseResult = bool.TryParse(settings.Value, out settingValue);
                        if (ParseResult)
                        {
                            //if (settings.SearchKey == "RecurringUnpaidBillIncludeEnable")
                            //{
                            //    model.RMRUnpaidBillsSettings = settingValue;
                            //}
                            //else if (settings.SearchKey == "OthersUnpaidBillIncludeEnable")
                            //{
                            //    model.OtherUnpaidBillsSettings = settingValue;
                            //}
                            //else if (settings.SearchKey == "RecurringBillingEmailSendEnable")
                            //{
                            //    model.PaperlessBillsSettings = settingValue;
                            //}
                            if (settings.SearchKey == "EInvoiceEnable")
                            {
                                model.eInvoiceSettings = Convert.ToBoolean(settings.Value);
                            }
                            else if (settings.SearchKey == "EReceiptEnable")
                            {
                                model.eReceiptSettings = settingValue;
                            }
                            else if (settings.SearchKey == "DayInAdvanceShowEnable")
                            {
                                model.DaysInAdvanceSetting = settingValue;
                            }
                            else if (settings.SearchKey == "LineItemShowEnable")
                            {
                                model.LineItemsSettings = settingValue;
                            }
                            else if (settings.SearchKey == "RMRAddNewLineItemButtonShowEnable")
                            {
                                model.AddNewLineItemsButtonSettings = settingValue;
                            }
                            else if (settings.SearchKey == "BillDateShowEnable")
                            {
                                model.BillDay = settingValue;
                            }

                        }
                    }
                }
            }
            return PartialView("RecurringTempSettingsPartial", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult ChangeRecurringTempSetting(bool Value, string Datakey)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return Json(result);
            }
            if (CurrentUser != null)
            {
                var GlobalSettingObject = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingByCompanyIdAndKey(Datakey, CurrentUser.CompanyId.Value);
                if (GlobalSettingObject != null)
                {
                    GlobalSettingObject.Value = Value.ToString().ToLower();
                    result = _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(GlobalSettingObject);
                }
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult PaymentDromdownLoading(Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return Json(new { result = false, message = "Current user not found." });
            }
            List<SelectListItem> ProfilePackage = new List<SelectListItem>();
            ProfilePackage.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            ProfilePackage.Add(new SelectListItem()
            {
                Text = "Invoice",
                Value = "Invoice"
            });
            List<PaymentProfileCustomer> PaymentInfoList = _Util.Facade.CustomerFacade.GetAllPaymentProfileByCustomerId(CustomerId, CurrentUser.CompanyId.Value);
            PaymentInfoList = PaymentInfoList.Where(x => x.Type != "Invoice").ToList();
            ProfilePackage.AddRange(PaymentInfoList.Select(y =>
             new SelectListItem()
             {
                 Text = y.Type.ToString(),
                 Value = y.PaymentInfoId.ToString()
             }).ToList());

            //ProfilePackage.AddRange(_Util.Facade.CustomerFacade.GetAllPaymentProfileByType(CustomerId, CurrentUser.CompanyId.Value, "PaymentMethodPackage", true).Select(x =>
            // new SelectListItem()
            // {
            //     Text = x.Type.ToString(),
            //     Value = x.PaymentInfoId.ToString()
            // }).ToList());
            //var MethodList = ProfilePackage.Where(x => x.Text != "OnFile").OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();

            return Json(new { result = true, PaymentMethodList = ProfilePackage });
        }
        [Authorize]
        [HttpPost]
        public JsonResult MenualRecurringBillingInvoiceGenerate(string StrStartDate, string StrEndDate, string PaymentFilter)
        {
            DateTime currenttime = DateTime.UtcNow.UTCToClientTime();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Guid CompanyId = CurrentUser.CompanyId.Value;
            bool Result = false;
            //List<Customer> CustomerList = _Util.Facade.CustomerFacade.GetCustomerListForRecurringBillByCompanyId(CompanyId);
            //if (CustomerList != null && CustomerList.Count > 0)
            //{
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            DateTime.TryParse(StrStartDate, out StartDate);
            DateTime.TryParse(StrEndDate, out EndDate);
            if (StartDate != new DateTime()) { StartDate = StartDate.SetZeroHour(); }
            if (EndDate == new DateTime()) { EndDate = currenttime.SetMaxHour(); }
            else { EndDate = EndDate.SetMaxHour(); }
            //var CustList = CustomerList.OrderBy(x => x.Id).ToList();
            //foreach (var customer in CustList)
            //{

            List<RecurringBillingSchedule> RecurringBillingList = _Util.Facade.CustomerFacade.GetRecurringBillingListByFilter(CompanyId, PaymentFilter);
            if (RecurringBillingList != null && RecurringBillingList.Count > 0)
            {
                RecurringBillingList = RecurringBillingList.Where(x => x.NextDate == null || (x.NextDate >= StartDate && x.NextDate <= EndDate)).OrderBy(x => x.Id).ToList();
            }
            if (RecurringBillingList != null && RecurringBillingList.Count > 0)
            {
                foreach (var recurring in RecurringBillingList)
                {
                    if (!recurring.StartDate.HasValue && !recurring.PaymentCollectionDate.HasValue) { continue; }
                    DateTime InvoiceStartDate = DateTime.UtcNow.UTCToClientTime().SetZeroHour();
                    DateTime InvoiceEndDate = DateTime.UtcNow.UTCToClientTime().SetZeroHour();
                    #region Bill cycle 
                    if (recurring.BillCycle.ToLower() == "weekly") { InvoiceStartDate = InvoiceStartDate.AddDays(-7); }
                    else if (recurring.BillCycle.ToLower() == "bi-weekly") { InvoiceStartDate = InvoiceStartDate.AddDays(-14); }
                    else if (recurring.BillCycle.ToLower() == "semi-monthly") { InvoiceStartDate = InvoiceStartDate.AddDays(-15); }
                    else if (recurring.BillCycle.ToLower() == "monthly") { InvoiceStartDate = InvoiceStartDate.AddMonths(-1); }
                    else if (recurring.BillCycle.ToLower() == "bi-monthly") { InvoiceStartDate = InvoiceStartDate.AddMonths(-2); }
                    else if (recurring.BillCycle.ToLower() == "quarterly") { InvoiceStartDate = InvoiceStartDate.AddMonths(-3); }
                    else if (recurring.BillCycle.ToLower() == "semi-annually") { InvoiceStartDate = InvoiceStartDate.AddMonths(-6); }
                    else if (recurring.BillCycle.ToLower() == "annually") { InvoiceStartDate = InvoiceStartDate.AddYears(-1); }
                    else { InvoiceStartDate = InvoiceStartDate.AddMonths(-1); }
                    #endregion
                    if (!string.IsNullOrWhiteSpace(recurring.LastRMRInvoiceRefId))
                    {
                        Invoice _inv = _Util.Facade.InvoiceFacade.GetByInvoiceId(recurring.LastRMRInvoiceRefId);
                        if (_inv != null)
                        {
                            DateTime CreateDate = _inv.CreatedDate.UTCToClientTime().SetZeroHour();
                            if (CreateDate <= InvoiceEndDate && CreateDate >= InvoiceStartDate)
                            {
                                continue;
                            }
                        }
                    }
                    else if (recurring.PreviousDate.HasValue)
                    {
                        DateTime RecurringDate = recurring.PreviousDate.Value.UTCToClientTime().SetZeroHour();
                        if (RecurringDate <= InvoiceEndDate && RecurringDate >= InvoiceStartDate)
                        {
                            continue;
                        }
                    }
                    // Add Invoice Create Function
                    Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(recurring.CustomerId);
                    if (customer != null)
                    {
                        string InvoiceId = GenerateInvoiceForRecurringBilling(customer, recurring);
                        string OnlyInvoiceCreaate = "Attempt to generate rmr invoice failed by " + CurrentUser.GetFullName();
                        if (!string.IsNullOrWhiteSpace(InvoiceId))
                        {
                            Result = true;
                            OnlyInvoiceCreaate = "RMR invoice no: " + InvoiceId + " has been created by " + CurrentUser.GetFullName();
                        }

                        if (recurring.IsEInvoice.HasValue && recurring.IsEInvoice.Value && !string.IsNullOrWhiteSpace(InvoiceId))
                        {
                            Result = EmailSendForRecurringBilling(InvoiceId, true);
                            if (Result)
                            {
                                OnlyInvoiceCreaate = "RMR invoice no: " + InvoiceId + " has been created and sent email notification by " + CurrentUser.GetFullName();
                            }
                        }
                        base.AddUserActivityForCustomer(OnlyInvoiceCreaate, "MenualRecurringBillingInvoiceGenerate,RecurringBilling", customer.CustomerId, customer.Id, InvoiceId, true);
                    }
                }

            }
            //    }
            //}
            return Json(new { result = Result, message = "Invoice create successfully." });
        }
        [Authorize]
        private string GenerateInvoiceForRecurringBilling(Customer customer, RecurringBillingSchedule recurring)
        {
            DateTime CreatedDate = DateTime.UtcNow;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string UserName = CurrentUser.GetFullName();
            Guid CreatedByUid = CurrentUser.UserId;
            string result = "";
            try
            {

                DateTime DueDate = CreatedDate.AddDays(-1).AddMonths(1); //DateTime.Today.AddDays(-1).AddMonths(1);

                #region InvoiceInsert
                string TaxType = "Non-Tax";
                if (recurring.TaxPercentage > 0)
                {
                    TaxType = "Sales Tax";
                }
                Invoice inv = new Invoice()
                {
                    CompanyId = recurring.CompanyId,
                    Tax = recurring.TaxAmount,
                    TotalAmount = recurring.TotalBillAmount,
                    BalanceDue = recurring.TotalBillAmount,
                    Amount = recurring.BillAmount,
                    DiscountAmount = 0,
                    Status = "Init",
                    DueDate = DueDate,//DateTime.Today.AddDays(-1).AddMonths(1),
                    CreatedBy = UserName,
                    CreatedDate = CreatedDate,
                    InvoiceDate = CreatedDate,
                    CreatedByUid = CreatedByUid,
                    LastUpdatedDate = CreatedDate,
                    LastUpdatedByUid = CreatedByUid,
                    LateAmount = 0,
                    IsBill = false,
                    IsEstimate = false,
                    CustomerName = customer.FirstName + " " + customer.LastName,
                    CustomerId = customer.CustomerId,
                    BillingCycle = recurring.BillCycle,
                    BillingAddress = recurring.BillingAddress,
                    ShippingAddress = recurring.BillingAddress,
                    TaxType = TaxType,
                    Description = recurring.MessageOnInvoice,
                    IsARBInvoice = true,
                    InvoiceEmailAddress = !string.IsNullOrWhiteSpace(recurring.EmailAddress) ? recurring.EmailAddress : customer.EmailAddress,
                    TaxPercentage = recurring.TaxPercentage,
                    PaymentType = recurring.PaymentMethod
                };
                inv.Id = _Util.Facade.InvoiceFacade.InsertInvoice(inv);
                inv.InvoiceId = inv.Id.GenerateInvoiceNo();
                if (!string.IsNullOrWhiteSpace(recurring.PaymentMethod))
                {
                    var firstTwoChars = recurring.PaymentMethod.Length <= 3 ? recurring.PaymentMethod : recurring.PaymentMethod.Substring(0, 3);
                    if (firstTwoChars.ToLower() == "cc_") { inv.InvoiceFor = LabelHelper.InvoiceFor.CreditCard; }
                    else if (firstTwoChars.ToLower() == "ach") { inv.InvoiceFor = LabelHelper.InvoiceFor.ACH; }
                    else { inv.InvoiceFor = LabelHelper.InvoiceFor.Invoice; }
                }
                else
                {
                    inv.InvoiceFor = LabelHelper.InvoiceFor.Invoice;
                }
                if (recurring.Status.ToLower() == "freetrial")
                {
                    inv.InvoiceFor = LabelHelper.InvoiceFor.CreditCard;
                }
                #endregion

                #region InvoiceItems
                var ItemsList = _Util.Facade.CustomerFacade.GetRecurringBillingScheduleItemsByScheduleId(recurring.ScheduleId);
                List<InvoiceDetail> invoiceDetailList = new List<InvoiceDetail>();
                if (ItemsList != null && ItemsList.Count > 0)
                {
                    ItemsList = ItemsList.OrderBy(x => x.Id).ToList();
                    foreach (var item in ItemsList)
                    {
                        InvoiceDetail invDet = new InvoiceDetail()
                        {
                            CompanyId = recurring.CompanyId,
                            CreatedBy = UserName,
                            CreatedDate = CreatedDate,
                            InvoiceId = inv.InvoiceId,
                            TotalPrice = item.Amount,
                            UnitPrice = item.Rate,
                            Quantity = item.Qty,
                            EquipName = item.ProductName,
                            EquipDetail = item.Description,
                            Taxable = item.IsTaxable
                        };
                        invoiceDetailList.Add(invDet);
                    }
                }
                #region UnpaidRecurringInvoiceItems
                if (recurring.IncludeOpenInvoices)
                {
                    var InvoiceItemsList = _Util.Facade.InvoiceFacade.GetManuallyUnpaidRecurringBillingInvoiceDetailsListByCustomerId(customer.CustomerId);
                    if (InvoiceItemsList != null && InvoiceItemsList.Count > 0)
                    {
                        InvoiceItemsList = InvoiceItemsList.OrderBy(x => x.Id).ToList();

                        foreach (var details in InvoiceItemsList)
                        {
                            var Discription = "(" + details.InvoiceId + ") " + details.EquipDetail;
                            details.EquipDetail = Discription;
                            details.Id = 0;
                            details.InvoiceId = inv.InvoiceId;
                            details.CreatedBy = UserName;
                            details.CreatedDate = CreatedDate;
                            invoiceDetailList.Add(details);
                        }
                    }
                }
                #endregion
                #region UnpaidOthersInvoiceItems
                if (recurring.OthersUnpaidBill != null && (bool)recurring.OthersUnpaidBill)
                {
                    var InvoiceItemsList = _Util.Facade.InvoiceFacade.GetManuallyUnpaidOthersInvoiceDetailsListByCustomerId(customer.CustomerId);
                    if (InvoiceItemsList != null && InvoiceItemsList.Count > 0)
                    {
                        InvoiceItemsList = InvoiceItemsList.OrderBy(x => x.Id).ToList();
                        foreach (var details in InvoiceItemsList)
                        {
                            var Discription = "(" + details.InvoiceId + ") " + details.EquipDetail;
                            details.EquipDetail = Discription;
                            details.Id = 0;
                            details.InvoiceId = inv.InvoiceId;
                            details.CreatedBy = UserName;
                            details.CreatedDate = CreatedDate;
                            invoiceDetailList.Add(details);
                        }
                    }
                }
                #endregion
                if (invoiceDetailList.Count > 0)
                {
                    foreach (var items in invoiceDetailList)
                    {
                        _Util.Facade.InvoiceFacade.InsertInvoiceDetails(items);
                    }
                }
                #endregion
                #region Update Customer      
                customer.LastGeneratedInvoice = CreatedDate;
                customer.LastUpdatedBy = UserName;
                _Util.Facade.CustomerFacade.UpdateCustomer(customer);
                #endregion

                double TotalDueAmount = 0;
                double SubTotalAmount = 0;
                double taxAmount = 0;
                double BillAmount = 0;
                #region HideInvoice
                if (recurring.IncludeOpenInvoices)
                {
                    var InvoiceList = _Util.Facade.InvoiceFacade.GetManuallyUnpaidRecurringBillingInvoiceListByCustomerId(customer.CustomerId, CurrentUser.CompanyId.Value);
                    if (InvoiceList != null && InvoiceList.Count > 0)
                    {
                        InvoiceList = InvoiceList.OrderBy(x => x.Id).ToList();
                        foreach (var invoice in InvoiceList)
                        {
                            if (invoice.Tax != null && invoice.TotalAmount != null)
                            {
                                SubTotalAmount += (double)invoice.TotalAmount;
                                taxAmount += (double)invoice.Tax;
                                BillAmount += invoice.Amount;
                                if (invoice.BalanceDue != null)
                                {
                                    TotalDueAmount += (double)invoice.BalanceDue;
                                }
                            }
                            var Discription = "(Added in invoice number : " + inv.InvoiceId + ") " + invoice.Description;
                            invoice.Status = LabelHelper.InvoiceStatus.RolledOver;
                            invoice.Description = Discription;
                            invoice.LastUpdatedDate = CreatedDate;
                            invoice.LastUpdatedByUid = CreatedByUid;
                            _Util.Facade.InvoiceFacade.UpdateInvoice(invoice);
                        }
                    }
                }
                if (recurring.OthersUnpaidBill.HasValue && recurring.OthersUnpaidBill.Value)
                {
                    var InvoiceList = _Util.Facade.InvoiceFacade.GetManuallyUnpaidOthersInvoiceListByCustomerId(customer.CustomerId, CurrentUser.CompanyId.Value);
                    if (InvoiceList != null && InvoiceList.Count > 0)
                    {
                        InvoiceList = InvoiceList.OrderBy(x => x.Id).ToList();
                        foreach (var invoice in InvoiceList)
                        {
                            if (invoice.Tax != null && invoice.TotalAmount != null)
                            {
                                SubTotalAmount += (double)invoice.TotalAmount;
                                taxAmount += (double)invoice.Tax;
                                BillAmount += invoice.Amount;
                                if (invoice.BalanceDue != null)
                                {
                                    TotalDueAmount += (double)invoice.BalanceDue;
                                }
                            }
                            var Discription = "(Added in invoice number : " + inv.InvoiceId + ") " + invoice.Description;
                            invoice.Status = LabelHelper.InvoiceStatus.RolledOver;
                            invoice.Description = Discription;
                            invoice.LastUpdatedDate = CreatedDate;
                            invoice.LastUpdatedByUid = CreatedByUid;
                            _Util.Facade.InvoiceFacade.UpdateInvoice(invoice);
                        }
                    }
                }
                #endregion

                #region Customer Credit
                if (recurring.Status.ToLower() == "freetrial")
                {
                    string CustomerCreditNote = string.Format(@"Invoice# <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{0}')"">{1}</a>", inv.Id, inv.InvoiceId);
                    CustomerCredit cc = new CustomerCredit()
                    {
                        Amount = Math.Round(inv.BalanceDue.Value, 2),
                        CompanyId = inv.CompanyId,
                        CreatedBy = CreatedByUid,
                        CreatedDate = CreatedDate,
                        CustomerId = inv.CustomerId,
                        TransactionId = 0,
                        Type = LabelHelper.CustomerCreditType.Credit,
                        IsRMRCredit = true,
                        Note = CustomerCreditNote
                    };
                    _Util.Facade.TransactionFacade.InsertCustomerCredit(cc);
                }
                #endregion


                inv.TotalAmount += SubTotalAmount;
                inv.BalanceDue += TotalDueAmount;
                inv.Tax += taxAmount;
                inv.Amount += BillAmount;
                //inv.LateAmount += BillAmount;
                if (SubTotalAmount == TotalDueAmount)
                {
                    inv.Status = LabelHelper.InvoiceStatus.Open;
                }
                else
                {
                    inv.Status = LabelHelper.InvoiceStatus.Partial;
                }
                bool r = _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
                if (r) { result = inv.InvoiceId; }
                #region Insert CustomerSnapshot
                CustomerSnapshot objCustomerSnapshot = new CustomerSnapshot
                {
                    CustomerId = customer.CustomerId,
                    CompanyId = CurrentUser.CompanyId.Value,
                    Description = "Invoice " + string.Format("<a onclick=OpenTopToBottomModal('{2}/Invoice/AddInvoice?id={0}&CustomerId={1}') style='cursor: pointer;'>", inv.Id, customer.Id, AppConfig.DomainSitePath) + "<b>" + inv.InvoiceId + "</b>" + "</a>",
                    Logdate = DateTime.Now.UTCCurrentTime(),
                    Updatedby = UserName,
                    Type = "InvoiceCreated"
                };
                _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objCustomerSnapshot);

                #endregion

                #region Next Date Change
                #region Bill cycle 
                DateTime NextDate = CreatedDate.UTCToClientTime().SetZeroHour();
                if (recurring.BillCycle.ToLower() == "weekly") { NextDate = CreatedDate.AddDays(7); }
                else if (recurring.BillCycle.ToLower() == "bi-weekly") { NextDate = CreatedDate.AddDays(14); }
                else if (recurring.BillCycle.ToLower() == "semi-monthly") { NextDate = CreatedDate.AddDays(15); }
                else if (recurring.BillCycle.ToLower() == "monthly") { NextDate = CreatedDate.AddMonths(1); }
                else if (recurring.BillCycle.ToLower() == "bi-monthly") { NextDate = CreatedDate.AddMonths(2); }
                else if (recurring.BillCycle.ToLower() == "quarterly") { NextDate = CreatedDate.AddMonths(3); }
                else if (recurring.BillCycle.ToLower() == "semi-annually") { NextDate = CreatedDate.AddMonths(6); }
                else if (recurring.BillCycle.ToLower() == "annually") { NextDate = CreatedDate.AddYears(1); }
                else { NextDate = CreatedDate.AddMonths(1); }
                #endregion

                recurring.NextDate = NextDate;
                recurring.PreviousDate = CreatedDate;
                recurring.LastUpdatedDate = CreatedDate;
                recurring.LastUpdatedBy = CreatedByUid;
                recurring.LastRMRInvoiceRefId = inv.InvoiceId;
                _Util.Facade.CustomerFacade.UpdateRecurringBillingSchedule(recurring);

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return result;
            }
        }
        [Authorize]
        public bool EmailSendForRecurringBilling(string InvoiceId, bool IsCCAdd = false)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            Company companyInfo = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceByInvoiceId(InvoiceId);
            List<InvoiceDetail> invoiceDetails = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(InvoiceId);
            Customer customerInfo = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(inv.CustomerId);
            if (companyInfo == null || inv == null || invoiceDetails == null || invoiceDetails.Count < 1 || customerInfo == null)
            {
                return result;
            }
            RecurringBillingSchedule recurring = new RecurringBillingSchedule();
            if (IsCCAdd)
            {
                recurring = _Util.Facade.CustomerFacade.GetRecurringBillingScheduleByInvoiceID(InvoiceId);
                if (recurring == null) { recurring = new RecurringBillingSchedule(); }
            }

            CreateInvoice Model = new CreateInvoice();
            List<CreateInvoice> CreateInvoList = new List<CreateInvoice>();
            Model.Invoice = inv;
            Model.InvoiceDetailList = invoiceDetails;
            CreateInvoice processedModel = GetInvoiceModelById(Model.Invoice, Model.InvoiceDetailList, companyInfo, customerInfo);
            if (processedModel == null) { return result; }
            #region PFD Create
            companyInfo.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            string settingskey = @" 'InvoiceSettingsShipping', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
            List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CurrentUser.CompanyId.Value);
            GlobalSetting PaymentStubs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "InvPreviewPaymentStubs");
            if (PaymentStubs != null)
            {
                ViewBag.PaymentStubs = PaymentStubs.Value;
            }
            else
            {
                ViewBag.PaymentStubs = "";
            }
            processedModel.InvoiceSetting = new InvoiceSetting();
            if (printsetting != null && printsetting.Count > 0)
            {
                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            processedModel.InvoiceSetting.DepositSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsDiscount")
                        {
                            processedModel.InvoiceSetting.DiscountSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsShipping")
                        {
                            processedModel.InvoiceSetting.ShippingSetting = true;
                        }
                    }
                }
            }
            processedModel.InvoiceNotes = _Util.Facade.InvoiceNoteFacade.GetAllInvoiceNoteByInvoiceIdAndCompanyId(Model.Invoice.Id, CurrentUser.CompanyId.Value);
            InvoicePaymentDate PayDate = _Util.Facade.TransactionFacade.GetLatestPaymentDateByInvoiceId(processedModel.Invoice.Id);
            if (PayDate != null)
            {
                processedModel.Invoice.TransacationDate = PayDate.PaymentDate;
            }
            CreateInvoList.Add(processedModel);
            ViewBag.CompanyId = companyInfo.CompanyId.ToString();
            if (PermissionChecker.IsPermitted(PermissionList.InvoicePermissions.InvoiceDetailsLineItemDiscountAmountShow)) { ViewBag.DiscountShow = true; }
            else { ViewBag.DiscountShow = false; }
            string pdfname = Model.Invoice.InvoiceId;
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Invoice/InvoicePdf.cshtml", CreateInvoList)
            {
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 }
            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            string filename = ConfigurationManager.AppSettings["File.InvoiceFiles"];
            var comname = companyInfo.CompanyName.ReplaceSpecialChar();
            filename = string.Format(filename, comname);
            filename += DateTime.UtcNow.Year.ToString() + "/" + DateTime.UtcNow.Month.ToString() + "/" + pdfname + ".pdf";
            string Serverfilename = FileHelper.GetFileFullPath(filename);
            FileHelper.SaveFile(applicationPDFData, Serverfilename);
            filename = AppConfig.DomainSitePath + "/" + filename;

            #endregion

            #region Send Email
            string SalesPhone = "", SalesGuy = "", CustomerName = "", emailtemplateBody = "", url = "";
            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            if (objemp != null)
            {
                SalesGuy = objemp.FirstName + " " + objemp.LastName;
                if (!string.IsNullOrWhiteSpace(objemp.Phone))
                {
                    SalesPhone = objemp.Phone;
                }
            }
            if (string.IsNullOrWhiteSpace(SalesPhone)) { SalesPhone = companyInfo.Phone; }
            if (string.IsNullOrWhiteSpace(SalesGuy)) { SalesGuy = User.Identity.Name; }
            CustomerName = customerInfo.FirstName + " " + customerInfo.LastName;
            EmailTemplate EmailTemplate = new EmailTemplate();
            if (Model.Invoice.InvoiceFor != "ACH" && Model.Invoice.InvoiceFor != "Credit Card")
            {
                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(Model.Invoice.Id
                                    + "#"
                                    + CurrentUser.CompanyId.Value
                                    + "#"
                                    + Model.Invoice.CustomerId);
                string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Invoice/", encryptedurl);
                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, Model.Invoice.CustomerId);
                url = AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code;
                EmailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("InvoicePredefineEmailTemplate");
            }
            else
            {
                EmailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("InvoicePredefineWithoutLinkEmailTemplate");
            }
            Hashtable datatemplate = new Hashtable();
            datatemplate.Add("CustomerName", CustomerName);
            datatemplate.Add("ExpirationDate", Model.Invoice.DueDate);
            datatemplate.Add("SalesPhone Number", SalesPhone);
            datatemplate.Add("CompanyName", companyInfo.CompanyName);
            datatemplate.Add("SalesGuy", SalesGuy);
            datatemplate.Add("url", url);
            emailtemplateBody = HS.Web.UI.Helper.LabelHelper.ParserHelper(EmailTemplate.BodyContent, datatemplate);

            InvoiceCreatedEmail email = new InvoiceCreatedEmail()
            {
                CompanyName = companyInfo.CompanyName,
                CustomerName = CustomerName,
                BalanceDue = Model.Invoice.TotalAmount != null ? LabelHelper.CurrentTransMakeCurrency.MakeCurrency() + Model.Invoice.TotalAmount.Value.ToString("0,0.00") : "0.00",
                DueDate = Model.Invoice.DueDate.HasValue ? Model.Invoice.DueDate.Value.ToString("MM/dd/yy") : "",
                InvoiceId = Model.Invoice.InvoiceId,
                ToEmail = !string.IsNullOrWhiteSpace(recurring.EmailAddress) ? recurring.EmailAddress : customerInfo.EmailAddress,
                EmailBody = emailtemplateBody,
                ccEmail = recurring.CCEmail,
                BccEmail = recurring.BCCEmail,
                Subject = string.Format("New Invoice From {0}:{1}", companyInfo.CompanyName, Model.Invoice.InvoiceId),
                FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : companyInfo.EmailAdress,
                FromName = SalesGuy,
                InvoicePdf = new Attachment(
                      FileHelper.GetFileFullPath(filename),
                     MediaTypeNames.Application.Octet)
            };
            result = _Util.Facade.MailFacade.SendInvoiceCreatedEmail(email, CurrentUser.CompanyId.Value);
            email.InvoicePdf.Dispose();
            if (result)
            {
                #region Email Log History
                CustomerSnapshot objEstimateEmailLog = new CustomerSnapshot()
                {
                    CustomerId = Model.Invoice.CustomerId,
                    CompanyId = CurrentUser.CompanyId.Value,
                    Description = "Invoice:" + "  " + inv.InvoiceId + " " + "email sent by " + "<b>" + CurrentUser.GetFullName().ToLower() + "</b>",
                    Logdate = DateTime.Now.UTCCurrentTime(),
                    Updatedby = CurrentUser.Identity.Name,
                    Type = "CustomerMailHistory"
                };
                _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEstimateEmailLog);
                CustomerAgreement objagree = new CustomerAgreement()
                {
                    CustomerId = customerInfo.CustomerId,
                    CompanyId = CurrentUser.CompanyId.Value,
                    InvoiceId = inv.InvoiceId,
                    Type = LabelHelper.EstimateStatus.SentToCustomer,
                    AddedDate = DateTime.Now.UTCCurrentTime()
                };
                _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objagree);
                #endregion
            }

            #endregion
            return result;
        }

        public CreateInvoice GetInvoiceModelById(Invoice Invoice, List<InvoiceDetail> InvoiceDetialList, Company tempCom, Customer tempCUstomer)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateInvoice Model = new CreateInvoice();
            Model.Invoice = Invoice;
            Model.InvoiceDetailList = InvoiceDetialList;

            Model.Invoice.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;

            Model.Invoice.IsEstimate = false;
            if (!string.IsNullOrWhiteSpace(Model.Invoice.EstimateTerm) && Model.Invoice.EstimateTerm != "-1")
                Model.Invoice.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(Model.Invoice.EstimateTerm);
            #region Discount Calculation 
            if (!string.IsNullOrWhiteSpace(Model.Invoice.DiscountType))
            {
                if (Model.Invoice.DiscountType == "amount")
                {
                    if (Invoice.Discountpercent != null)
                    {
                        Model.Discount = Invoice.Discountpercent.Value;
                    }
                }
                else
                {
                    if (Invoice.Discountpercent != null)
                    {
                        Model.Discount = ((Invoice.Discountpercent / 100) * Model.SubTotal).Value;
                    }
                }
            }
            #endregion

            //customer name is customer business name here 
            if (tempCUstomer.BusinessName != "")
            {
                Model.CustomerName = tempCUstomer.BusinessName;
            }
            else
            {
                Model.CustomerName = Model.Invoice.CustomerName;
            }
            Model.CusBussinessName = tempCUstomer.BusinessName;

            Model.SubTotal = 0;
            foreach (var item in Model.InvoiceDetailList)
            {
                item.CreatedBy = User.Identity.Name;
                item.CreatedDate = DateTime.UtcNow;
                item.CompanyId = CurrentUser.CompanyId.Value;
                Model.SubTotal += (item.TotalPrice.HasValue) ? item.TotalPrice.Value : 0;
            }
            if (string.IsNullOrWhiteSpace(Model.Invoice.InvoiceMessage))
            {
                Model.Invoice.InvoiceMessage = _Util.Facade.GlobalSettingsFacade.GetInvoiceMessageByCompanyId(CurrentUser.CompanyId.Value);
            }
            Model.CompanyAddress = tempCom.Address;
            Model.CompanyStreet = tempCom.Street;
            string ComCity = "";
            string ComState = "";
            if (!string.IsNullOrWhiteSpace(tempCom.City))
            {
                ComCity = tempCom.City.UppercaseFirst() + ", ";
            }
            if (!string.IsNullOrWhiteSpace(tempCom.State))
            {
                ComState = tempCom.State + " ";
            }
            Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
            Model.CompanyCity = tempCom.City.UppercaseFirst();
            Model.CompanyState = tempCom.State;
            Model.CompanyZip = tempCom.ZipCode;
            Model.CompanyPhone = tempCom.Phone;
            Model.CompanyEmail = tempCom.EmailAdress;
            Model.CompanyName = tempCom.CompanyName;
            Model.PhoneNum = tempCom.Phone;
            Model.CompanyWebsite = tempCom.Website;
            Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
            Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
            Model.CustomerState = tempCUstomer.State;
            Model.CustomerZipCode = tempCUstomer.ZipCode;
            Model.CustomerNo = tempCUstomer.CustomerNo;
            Model.CustomerStreet = tempCUstomer.Street;
            Model.Invoice.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);

            Model.ShowInvoiceShippingAddress = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(CurrentUser.CompanyId.Value).ToLower() == "true" ? true : false;
            GlobalSetting invSta = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "InvoiceStaticBox");
            if (invSta != null)
            {
                Model.ShowInvoiceStaticBox = invSta.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowInvoiceStaticBox = false;
            }
            GlobalSetting invCode3Sta = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ShowCode3InvoiceStaticBox");
            if (invCode3Sta != null)
            {
                Model.ShowCode3InvoiceStaticBox = invCode3Sta.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowCode3InvoiceStaticBox = false;
            }
            GlobalSetting invPayAddress = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ShowPaymentAddressForSendInvoice");
            if (invPayAddress != null)
            {
                Model.ShowPaymentAddressForSendInvoice = invPayAddress.Value.ToLower() == "true" ? true : false;
                if (Model.ShowPaymentAddressForSendInvoice)
                {
                    Model.PaymentAddress = !string.IsNullOrWhiteSpace(invPayAddress.OptionalValue) ? invPayAddress.OptionalValue : (!string.IsNullOrWhiteSpace(Model.CompanyStreet) ? Model.CompanyStreet + ", " : "") + Model.CompanyCity + " " + Model.CompanyState + " " + Model.CompanyZip;
                }
            }
            else
            {
                Model.ShowPaymentAddressForSendInvoice = false;
            }
            if (string.IsNullOrWhiteSpace(tempCom.CompanyLogo))
            {
                tempCom.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            }
            Model.CompanyLogo = tempCom.CompanyLogo;
            if (!string.IsNullOrWhiteSpace(Model.Invoice.DiscountType))
            {
                if (Model.Invoice.DiscountType != "amount")
                {
                    if (Model.Invoice.Discountpercent.HasValue && Model.Invoice.Discountpercent.Value > 0)
                    {
                        Model.Discount = ((Model.Invoice.Discountpercent / 100) * Model.SubTotal).Value;
                    }
                }
            }
            if (Model.Invoice.BalanceDue > 0)
            {
                Model.AmountInWord = InvoiceController.NumberToWords(Model.Invoice.BalanceDue.Value); //NumberToWords(Model.Invoice.TotalAmount.Value);
            }
            return Model;
        }

        public void UpdateRecurringBillingInformationByCustomerModification(Guid customerId, Guid TicketId, Guid RMRUserId, Guid RMRCompanyId, string InvoiceNo, string ControllerName)
        {
            double MonthlyAmount = 0;
            Guid ScheduleId = Guid.Empty;
            bool Activity = true;
            bool IsInsertPermission = true;
            Invoice _Inv = new Invoice();
            string LogMsg = "Failed to create recurring billing template from " + ControllerName.ToLower();
            if (customerId != Guid.Empty)
            {
                Customer _customer = _Util.Facade.CustomerFacade.GetCustomerById(customerId);
                if (_customer != null)
                {
                    if (ControllerName == "Ticket" || ControllerName == "Customer") { IsInsertPermission = false; }
                    if (!string.IsNullOrWhiteSpace(InvoiceNo))
                    {
                        _Inv = _Util.Facade.InvoiceFacade.GetInvoiceByInvoiceId(InvoiceNo);
                        if (_Inv == null) { _Inv = new Invoice(); }
                    }
                    if (RMRUserId == Guid.Empty)
                    {
                        RMRUserId = new Guid("22222222-2222-2222-2222-222222222222");
                    }
                    if (RMRCompanyId == Guid.Empty)
                    {
                        Company company = _Util.Facade.CustomerFacade.GetCompanyByCustomerId(_customer.CustomerId);
                        if (company != null) { RMRCompanyId = company.CompanyId; }
                    }
                    RecurringBillingSchedule model = new RecurringBillingSchedule();
                    var IsRMRCreditAdd = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(RMRCompanyId, "IsCapturePaymentAmountAddToRMRCredit");
                    List<RecurringBillingSchedule> ScheduleList = _Util.Facade.CustomerFacade.GetAllRecurringBillingListByCustomerIdAndCompanyId(customerId, RMRCompanyId);

                    if (ScheduleList != null && ScheduleList.Count > 0) { Activity = false; }
                    bool parseresult = false;
                    if (_Inv != null && _Inv.InstallationType == "Service")
                    {
                        parseresult = true;
                    }
                    else
                    {
                        parseresult = double.TryParse(_customer.MonthlyMonitoringFee, out MonthlyAmount);
                    }
                    #region insert recurring template info
                    if (parseresult && Activity && TicketId != Guid.Empty && IsInsertPermission)
                    {
                        DateTime Currentdate = DateTime.UtcNow;
                        model.CustomerId = _customer.CustomerId;
                        model.EmailAddress = _customer.EmailAddress;
                        model.ScheduleId = Guid.NewGuid();
                        model.CreatedDate = Currentdate;
                        model.LastUpdatedBy = RMRUserId;
                        model.LastUpdatedDate = Currentdate;
                        model.CreatedBy = RMRUserId;
                        model.TemplateName = "Recurring Billing";
                        model.DayInAdvance = 0;
                        model.Interval = 0;
                        model.MessageOnInvoice = _customer.BillNotes;
                        model.Status = "Active";
                        model.PreviousDate = null;
                        model.CompanyId = RMRCompanyId;

                        if (!string.IsNullOrWhiteSpace(_customer.BillCycle) && _customer.BillCycle != "-1") { model.BillCycle = _customer.BillCycle; }
                        else { model.BillCycle = "Monthly"; }
                        if (_customer.InstallDate.HasValue && model.StartDate == null)
                        {
                            if (IsRMRCreditAdd != null && IsRMRCreditAdd.Value.ToLower() == "true" && _Inv != null && !string.IsNullOrWhiteSpace(_Inv.InvoiceId))
                            {
                                RMRCreditApplied(_Inv, _customer, RMRUserId, RMRCompanyId);
                            }
                            model.StartDate = _customer.InstallDate.Value.UTCToClientTime().SetZeroHour();
                            model.PreviousDate = _customer.InstallDate.Value;
                            DateTime InvoiceStartDate = new DateTime();
                            if (model.BillCycle.ToLower() == "weekly") { InvoiceStartDate = _customer.InstallDate.Value.AddDays(7); }
                            else if (model.BillCycle.ToLower() == "bi-weekly") { InvoiceStartDate = _customer.InstallDate.Value.AddDays(14); }
                            else if (model.BillCycle.ToLower() == "semi-monthly") { InvoiceStartDate = _customer.InstallDate.Value.AddDays(15); }
                            else if (model.BillCycle.ToLower() == "monthly") { InvoiceStartDate = _customer.InstallDate.Value.AddMonths(1); }
                            else if (model.BillCycle.ToLower() == "bi-monthly") { InvoiceStartDate = _customer.InstallDate.Value.AddMonths(2); }
                            else if (model.BillCycle.ToLower() == "quarterly") { InvoiceStartDate = _customer.InstallDate.Value.AddMonths(3); }
                            else if (model.BillCycle.ToLower() == "semi-annually") { InvoiceStartDate = _customer.InstallDate.Value.AddMonths(6); }
                            else if (model.BillCycle.ToLower() == "annually") { InvoiceStartDate = _customer.InstallDate.Value.AddYears(1); }
                            else { InvoiceStartDate = _customer.InstallDate.Value.AddMonths(1); }
                            int BillDay = 1;
                            if (int.TryParse(_customer.BillDay.ToString(), out BillDay) && BillDay < 29 && BillDay > 0)
                            {
                                string MonthYear = InvoiceStartDate.ToString("yyyy-MM");
                                string StrDay = "";
                                if (BillDay < 10) { StrDay = "0" + BillDay; }
                                else { StrDay = BillDay.ToString(); }
                                DateTime BillDate = Convert.ToDateTime(MonthYear + "-" + StrDay);
                                model.NextDate = BillDate;
                                model.PaymentCollectionDate = BillDate;
                            }
                            else
                            {
                                model.NextDate = InvoiceStartDate.SetZeroHour();
                                model.PaymentCollectionDate = InvoiceStartDate.SetZeroHour();
                            }
                        }
                        else
                        {
                            if (model.PreviousDate == null)
                            {
                                model.StartDate = _customer.InstallDate;
                                if (DateTime.TryParse(_customer.InstallDate.ToString(), out Currentdate))
                                {
                                    model.NextDate = Currentdate.SetZeroHour();
                                    model.PaymentCollectionDate = Currentdate.SetZeroHour();
                                }
                                else
                                {
                                    model.NextDate = null;
                                    model.PaymentCollectionDate = null;
                                }
                            }
                        }

                        if (model.StartDate != null)
                        {
                            int Team = 1;
                            if (int.TryParse(_customer.ContractTeam, out Team))
                            {
                                if (Team > 0) { model.EndDate = Currentdate.AddMonths(12 * Team); }
                                else { model.EndDate = null; }
                            }
                        }
                        var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(RMRCompanyId);
                        model.BillingAddress = AddressHelper.MakeCustomerAddress(_customer, "BillingAddress", AddressTemplate);
                        double TaxPercentage = 0;
                        if (_customer.BillTax != null)
                        {
                            if (_customer.BillTax.HasValue)
                            {
                                var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(RMRCompanyId, _customer.CustomerId);
                                if (GetSalesTax != null)
                                {
                                    if (double.TryParse(GetSalesTax.Value, out TaxPercentage)) { model.TaxPercentage = TaxPercentage; }
                                    else { model.TaxPercentage = TaxPercentage; }
                                }
                            }
                        }
                        if (MonthlyAmount > 0)
                        {
                            model.BillAmount = MonthlyAmount;
                            model.TaxAmount = MonthlyAmount * TaxPercentage / 100;
                            model.TotalBillAmount = _customer.BillAmount != null ? (double)_customer.BillAmount : 0;
                        }
                        else
                        {
                            model.BillAmount = _Inv.Amount;
                            model.TaxAmount = _Inv.Tax != null ? _Inv.Tax.Value : 0;
                            model.TotalBillAmount = _Inv.TotalAmount != null ? _Inv.TotalAmount.Value : 0;
                        }
                        RecurringBillingPaymentInfoModel PaymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentProfileCustomerForRMRByCustomerId(_customer.CustomerId, RMRCompanyId);
                        if (PaymentInfo != null && !string.IsNullOrWhiteSpace(PaymentInfo.PaymentMethod) && PaymentInfo.PaymentMethod.Length > 2)
                        {
                            string strpaymentValue = PaymentInfo.PaymentMethod.Substring(0, 3);
                            if (strpaymentValue.ToLower() == "cc_" || strpaymentValue.ToLower() == "ach")
                            {
                                model.CustomerPaymentProfileId = PaymentInfo.PaymentId;
                                model.PaymentMethod = PaymentInfo.PaymentMethod;
                                model.CollectOnline = true;
                            }
                            else
                            {
                                model.CustomerPaymentProfileId = 0;
                                model.PaymentMethod = "Invoice";
                                model.CollectOnline = false;
                            }
                        }
                        else
                        {
                            model.CustomerPaymentProfileId = 0;
                            model.PaymentMethod = "Invoice";
                            model.CollectOnline = false;
                        }

                        // from global settings by company
                        List<GlobalSetting> RecurringAllGlobalSettings = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsForRecurringByCompanyId(RMRCompanyId);
                        if (RecurringAllGlobalSettings != null && RecurringAllGlobalSettings.Count > 0)
                        {
                            foreach (var settings in RecurringAllGlobalSettings)
                            {
                                if (!string.IsNullOrWhiteSpace(settings.Value))
                                {
                                    if (settings.SearchKey == "RecurringUnpaidBillIncludeEnable")
                                    {
                                        model.IncludeOpenInvoices = Convert.ToBoolean(settings.Value);
                                    }
                                    else if (settings.SearchKey == "OthersUnpaidBillIncludeEnable")
                                    {
                                        model.OthersUnpaidBill = Convert.ToBoolean(settings.Value);
                                    }
                                    else if (settings.SearchKey == "RecurringBillingEmailSendEnable")
                                    {
                                        model.AutomaticallySendEmail = Convert.ToBoolean(settings.Value);
                                    }
                                    else if (settings.SearchKey == "EInvoiceEnable")
                                    {
                                        model.IsEInvoice = Convert.ToBoolean(settings.Value);
                                    }
                                    else if (settings.SearchKey == "EReceiptEnable")
                                    {
                                        model.IsEReceipt = Convert.ToBoolean(settings.Value);
                                    }
                                }
                            }
                        }
                        if (_customer.IsReceivePaymentMail.HasValue && _customer.IsReceivePaymentMail.Value)
                        {
                            model.IsEInvoice = true;
                            model.IsEReceipt = true;
                        }
                        bool result = false;
                        List<CustomerAppointmentEquipment> AppoinmentEqpList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(TicketId);
                        if (AppoinmentEqpList != null && AppoinmentEqpList.Count() > 0)
                        {
                            var ServiceList = AppoinmentEqpList.Where(x => x.EquipmentClassId == 2).ToList();
                            if (ServiceList != null && ServiceList.Count > 0)
                            {
                                bool taxable = true;
                                if (model.BillAmount == model.TotalBillAmount) { taxable = false; }
                                foreach (var service in ServiceList.Where(x => x.IsARBEnabled == true))
                                {
                                    RecurringBillingScheduleItems items = new RecurringBillingScheduleItems();
                                    items.ScheduleId = model.ScheduleId;
                                    items.AddedDate = DateTime.UtcNow;
                                    items.AddedBy = RMRUserId;
                                    items.ProductName = service.EquipName;
                                    items.Description = service.EquipDetail;
                                    items.Qty = service.Quantity;
                                    items.Rate = service.UnitPrice;
                                    items.Amount = service.TotalPrice;
                                    // find out taxable
                                    items.IsTaxable = taxable;
                                    _Util.Facade.CustomerFacade.InsertRecurringBillingScheduleItems(items);
                                }
                                result = true;
                            }
                        }
                        if (result)
                        {
                            if (_Inv != null && !string.IsNullOrWhiteSpace(_Inv.InvoiceId)) { model.LastRMRInvoiceRefId = _Inv.InvoiceId; }
                            _Util.Facade.CustomerFacade.InsertRecurringBillingSchedule(model);
                        }
                        LogMsg = "Recurring billing template updated successfully from " + ControllerName.ToLower();
                    }
                    #endregion
                    #region Update recurring template info
                    else
                    {
                        if (parseresult && !Activity && !IsInsertPermission)
                        {
                            model = ScheduleList[0];
                            DateTime Currentdate = DateTime.UtcNow;
                            model.LastUpdatedBy = RMRUserId;
                            model.LastUpdatedDate = Currentdate;
                            if (_customer.IsReceivePaymentMail.HasValue && _customer.IsReceivePaymentMail.Value)
                            {
                                model.IsEInvoice = true;
                                model.IsEReceipt = true;
                            }
                            else
                            {
                                model.IsEInvoice = false;
                                model.IsEReceipt = false;
                            }
                            if (!string.IsNullOrWhiteSpace(_customer.BillCycle) && _customer.BillCycle != "-1") { model.BillCycle = _customer.BillCycle; }
                            else { model.BillCycle = "Monthly"; }
                            if (DateTime.TryParse(_customer.InstallDate.ToString(), out Currentdate) && model.StartDate == null)
                            {
                                if (string.IsNullOrWhiteSpace(_Inv.InvoiceId) && !string.IsNullOrWhiteSpace(model.LastRMRInvoiceRefId))
                                {
                                    _Inv = _Util.Facade.InvoiceFacade.GetInvoiceByInvoiceId(model.LastRMRInvoiceRefId);
                                    if (_Inv == null) { _Inv = new Invoice(); }
                                }
                                if (IsRMRCreditAdd != null && IsRMRCreditAdd.Value.ToLower() == "true" && _Inv != null && !string.IsNullOrWhiteSpace(_Inv.InvoiceId))
                                {
                                    RMRCreditApplied(_Inv, _customer, RMRUserId, RMRCompanyId);
                                }

                                model.StartDate = Currentdate;
                                model.PreviousDate = Currentdate;
                                DateTime InvoiceStartDate = new DateTime();
                                if (model.BillCycle.ToLower() == "weekly") { InvoiceStartDate = Currentdate.AddDays(7); }
                                else if (model.BillCycle.ToLower() == "bi-weekly") { InvoiceStartDate = Currentdate.AddDays(14); }
                                else if (model.BillCycle.ToLower() == "semi-monthly") { InvoiceStartDate = Currentdate.AddDays(15); }
                                else if (model.BillCycle.ToLower() == "monthly") { InvoiceStartDate = Currentdate.AddMonths(1); }
                                else if (model.BillCycle.ToLower() == "bi-monthly") { InvoiceStartDate = Currentdate.AddMonths(2); }
                                else if (model.BillCycle.ToLower() == "quarterly") { InvoiceStartDate = Currentdate.AddMonths(3); }
                                else if (model.BillCycle.ToLower() == "semi-annually") { InvoiceStartDate = Currentdate.AddMonths(6); }
                                else if (model.BillCycle.ToLower() == "annually") { InvoiceStartDate = Currentdate.AddYears(1); }
                                int BillDay = 1;
                                if (int.TryParse(_customer.BillDay.ToString(), out BillDay) && BillDay < 29 && BillDay > 0)
                                {
                                    string MonthYear = InvoiceStartDate.ToString("yyyy-MM");
                                    string StrDay = "";
                                    if (BillDay < 10) { StrDay = "0" + BillDay; }
                                    else { StrDay = BillDay.ToString(); }
                                    DateTime BillDate = Convert.ToDateTime(MonthYear + "-" + StrDay);
                                    model.NextDate = BillDate;
                                    model.PaymentCollectionDate = BillDate;
                                }
                                else
                                {
                                    model.NextDate = InvoiceStartDate.SetZeroHour();
                                    model.PaymentCollectionDate = InvoiceStartDate.SetZeroHour();
                                }
                            }
                            else
                            {
                                if (model.PreviousDate == null)
                                {
                                    model.StartDate = _customer.InstallDate;
                                    if (DateTime.TryParse(_customer.InstallDate.ToString(), out Currentdate))
                                    {
                                        model.NextDate = Currentdate.SetZeroHour();
                                        model.PaymentCollectionDate = Currentdate.SetZeroHour();
                                    }
                                    else
                                    {
                                        model.NextDate = null;
                                        model.PaymentCollectionDate = null;
                                    }
                                }
                            }

                            if (model.StartDate != null)
                            {
                                int Team = 1;
                                if (int.TryParse(_customer.ContractTeam, out Team))
                                {
                                    if (Team > 0) { model.EndDate = Currentdate.AddMonths(12 * Team); }
                                    else { model.EndDate = null; }
                                }
                            }

                            double TaxPercentage = 0;
                            if (_customer.BillTax.HasValue && _customer.BillTax.Value)
                            {
                                var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(RMRCompanyId, _customer.CustomerId);
                                if (GetSalesTax != null)
                                {
                                    if (double.TryParse(GetSalesTax.Value, out TaxPercentage)) { model.TaxPercentage = TaxPercentage; }
                                    else { model.TaxPercentage = TaxPercentage; }
                                }
                            }
                            model.BillAmount = MonthlyAmount;
                            model.TaxAmount = MonthlyAmount * TaxPercentage / 100;
                            model.TotalBillAmount = _customer.BillAmount != null ? (double)_customer.BillAmount : 0;

                            bool isInvoice = false;
                            if (string.IsNullOrWhiteSpace(_customer.PaymentMethod))
                            {
                                RecurringBillingPaymentInfoModel PaymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentProfileCustomerForRMRByCustomerId(_customer.CustomerId, RMRCompanyId);

                                if (PaymentInfo != null && !string.IsNullOrWhiteSpace(PaymentInfo.PaymentMethod) && PaymentInfo.PaymentMethod.Length > 2)
                                {
                                    string strpaymentValue = PaymentInfo.PaymentMethod.Substring(0, 3);
                                    if (strpaymentValue.ToLower() == "cc_" || strpaymentValue.ToLower() == "ach")
                                    {
                                        model.CustomerPaymentProfileId = PaymentInfo.PaymentId;
                                        model.PaymentMethod = PaymentInfo.PaymentMethod;
                                        model.CollectOnline = true;
                                    }
                                    else
                                    {
                                        isInvoice = true;
                                    }
                                }
                                else
                                {
                                    isInvoice = true;
                                }
                            }
                            else if (!string.IsNullOrWhiteSpace(_customer.PaymentMethod))
                            {
                                int PaymentInfoId = 0;
                                if (int.TryParse(_customer.PaymentMethod, out PaymentInfoId) && PaymentInfoId > 0)
                                {
                                    var PPC = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentProfileByPaymentInfoId(PaymentInfoId);
                                    if (PPC != null && PPC.Type.Length > 2)
                                    {
                                        string strpaymentValue = PPC.Type.Substring(0, 3);
                                        if (strpaymentValue.ToLower() == "cc_" || strpaymentValue.ToLower() == "ach")
                                        {
                                            _customer.PaymentMethod = PPC.Type;
                                        }
                                        else
                                        {
                                            PaymentInfoId = 0;
                                        }
                                    }
                                    else
                                    {
                                        PaymentInfoId = 0;
                                    }
                                }
                                else if (_customer.PaymentMethod.Length > 2)
                                {
                                    string strpaymentValue = _customer.PaymentMethod.Substring(0, 3);
                                    if (strpaymentValue.ToLower() == "cc_" || strpaymentValue.ToLower() == "ach")
                                    {
                                        PaymentInfoId = _Util.Facade.PaymentInfoFacade.GetPaymentInfoIdByPaymentProfileCustomerType(_customer.PaymentMethod);
                                    }
                                }
                                if (PaymentInfoId > 0)
                                {
                                    model.CustomerPaymentProfileId = PaymentInfoId;
                                    model.PaymentMethod = _customer.PaymentMethod;
                                    model.CollectOnline = true;
                                }
                                else
                                {
                                    isInvoice = true;
                                }
                            }
                            else
                            {
                                isInvoice = true;
                            }
                            if (isInvoice)
                            {
                                model.CustomerPaymentProfileId = 0;
                                model.PaymentMethod = "Invoice";
                                model.CollectOnline = false;
                            }
                            if (_Inv != null && !string.IsNullOrWhiteSpace(_Inv.InvoiceId)) { model.LastRMRInvoiceRefId = _Inv.InvoiceId; }
                            _Util.Facade.CustomerFacade.UpdateRecurringBillingSchedule(model);
                        }
                        if (TicketId != Guid.Empty)
                        {
                            // Insert Recurring Items
                            List<CustomerAppointmentEquipment> AppoinmentEqpList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(TicketId);
                            if (AppoinmentEqpList != null && AppoinmentEqpList.Count() > 0)
                            {
                                var ServiceList = AppoinmentEqpList.Where(x => x.EquipmentClassId == 2).ToList();
                                if (ServiceList != null && ServiceList.Count > 0)
                                {
                                    // Delete Recurring Items
                                    _Util.Facade.CustomerFacade.DeleteRecurringBillingScheduleItemsByScheduleId(model.ScheduleId);

                                    bool taxable = true;
                                    if (model.BillAmount == model.TotalBillAmount) { taxable = false; }
                                    foreach (var service in ServiceList)
                                    {
                                        RecurringBillingScheduleItems items = new RecurringBillingScheduleItems();
                                        items.ScheduleId = model.ScheduleId;
                                        items.AddedDate = DateTime.UtcNow;
                                        items.AddedBy = RMRUserId;
                                        items.ProductName = service.EquipName;
                                        items.Description = service.EquipDetail;
                                        items.Qty = service.Quantity;
                                        items.Rate = service.UnitPrice;
                                        items.Amount = service.TotalPrice;
                                        // find out taxable
                                        items.IsTaxable = taxable;
                                        _Util.Facade.CustomerFacade.InsertRecurringBillingScheduleItems(items);
                                    }
                                }
                            }
                        }
                        LogMsg = "Recurring billing template created successfully from " + ControllerName.ToLower();
                    }
                    #endregion
                    #region Customer Update
                    _customer.PaymentMethod = model.PaymentMethod;
                    if (model.StartDate.HasValue)
                    {
                        int BillDay = Convert.ToInt32(model.StartDate.Value.ToString("dd"));
                        if (BillDay > 28) { BillDay = 28; }
                        _customer.BillDay = BillDay;
                    }
                    _customer.FirstBilling = model.StartDate;
                    _Util.Facade.CustomerFacade.UpdateCustomer(_customer);
                    #endregion
                    #region Invoice Update
                    if (model != null && _Inv != null && model.PaymentMethod != null)
                    {
                        if (model.PaymentMethod.Length > 2)
                        {
                            string paymentType = model.PaymentMethod.Substring(0, 3);
                            if (paymentType.ToLower() == "cc_") { _Inv.InvoiceFor = LabelHelper.InvoiceFor.CreditCard; }
                            else if (paymentType.ToLower() == "ach") { _Inv.InvoiceFor = LabelHelper.InvoiceFor.ACH; }
                        }
                        else
                        {
                            _Inv.InvoiceFor = LabelHelper.InvoiceFor.Invoice;
                        }
                        _Util.Facade.InvoiceFacade.UpdateInvoice(_Inv);
                    }
                    #endregion
                    #region User Log
                    if (_Inv == null) { _Inv = new Invoice(); }
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(RMRUserId);
                    string UserName = "";
                    if (empobj != null)
                    {
                        UserName = empobj.FirstName + " " + empobj.LastName;
                    }
                    else
                    {
                        UserName = "Customer";
                    }
                    UserActivityCustomer uac = new UserActivityCustomer()
                    {
                        ActivityId = Guid.NewGuid(),
                        CustomerId = _customer.CustomerId,
                        RefId = _Inv.InvoiceId
                    };
                    _Util.Facade.UserActivityCustomerFacade.InsertUserActivityCustomer(uac);
                    UserActivity ua = new UserActivity()
                    {
                        ActivityId = uac.ActivityId,
                        PageUrl = "",
                        ReferrerUrl = "",
                        Action = "UpdateRecurringBillingInformationByCustomerModification,RecurringBilling",
                        StatsDate = DateTime.UtcNow,
                        UserId = RMRUserId,
                        UserName = UserName,
                        ActionDisplyText = LogMsg,
                        IsARB = true,
                        UserAgent = AppConfig.GetUserAgent != null ? AppConfig.GetUserAgent : "",
                        UserIp = AppConfig.GetIP != null ? AppConfig.GetIP : ""
                    };
                    _Util.Facade.UserActivityFacade.InsertUserActivity(ua);

                    #endregion
                }
            }
        }
        [Authorize]
        public JsonResult SendRecurringInvoiceEmail(List<string> ArrayItemInvoiceId)
        {
            bool Result = false;
            if (ArrayItemInvoiceId != null && ArrayItemInvoiceId.Count > 0)
            {
                foreach (var item in ArrayItemInvoiceId)
                {
                    Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceByInvoiceId(item);
                    if (inv != null && inv.Id > 0 && !string.IsNullOrWhiteSpace(inv.InvoiceId))
                    {
                        Result = EmailSendForRecurringBilling(inv.InvoiceId);
                    }
                }
            }
            return Json(Result);
        }
        [Authorize]
        [HttpPost]
        public JsonResult RecurringBillingCollectPayment(List<int> scheduleList)
        {
            bool Result = true;
            string Message = "Please try again later.";
            if (scheduleList != null && scheduleList.Count > 0)
            {
                string ids = string.Format("'{0}'", string.Join("','", scheduleList.Select(i => i.ToString())));
                List<RecurringBillingSchedule> recurrings = _Util.Facade.CustomerFacade.GetRecurringBillingListByTempIds(ids);

                if (recurrings != null && recurrings.Count > 0)
                {
                    DateTime todateTime = DateTime.UtcNow.UTCToClientTime().SetZeroHour().AddDays(5);
                    foreach (RecurringBillingSchedule schedule in recurrings)
                    {

                        if (schedule != null && schedule.Id > 0 && (schedule.Status == "Active" || schedule.Status == "FreeTrial") && schedule.StartDate.HasValue && schedule.StartDate.Value != new DateTime())
                        {

                            if (schedule.NextDate.HasValue)
                            {
                                DateTime Enddate = schedule.NextDate.Value.SetZeroHour();
                                if (Enddate <= todateTime)
                                {
                                    MakeInvoice(schedule);
                                }
                            }
                            else
                            {
                                MakeInvoice(schedule);
                            }

                        }

                    }
                    Message = "Invoice generated successfully.";
                }
            }

            return Json(new { result = Result, message = Message });
        }
        [Authorize]
        public ActionResult RecurringBillingInvoiceList(int? CustomerId, string SearchText, string Order, DateTime? Start, DateTime? End)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (Start.HasValue == false || End.HasValue == false)
            {
                Start = new DateTime();
                End = new DateTime();
            }
            List<RMRInvoice> model = new List<RMRInvoice>();

            if (CustomerId.HasValue)
            {
                var CustomerInfo = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);
                if (CustomerInfo != null)
                {
                    model = _Util.Facade.CustomerFacade.GetRMRInvoiceByCustomerIdAndCompanyId(CustomerInfo.CustomerId, CurrentUser.CompanyId.Value, SearchText, Order, Start, End);
                }
            }
            return View(model);
        }
        [Authorize]
        public ActionResult RecurringBillingHistoryList(int? CustomerId, string SearchText, string Order, DateTime? Start, DateTime? End)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (Start.HasValue == false || End.HasValue == false)
            {
                Start = new DateTime();
                End = new DateTime();
            }
            List<RMRHistory> model = new List<RMRHistory>();

            if (CustomerId.HasValue)
            {
                var CustomerInfo = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);
                if (CustomerInfo != null)
                {
                    model = _Util.Facade.CustomerFacade.GetRMRHistoryByCustomerIdAndCompanyId(CustomerInfo.CustomerId, CurrentUser.CompanyId.Value, SearchText, Order, Start, End);
                }
            }
            return View(model);
        }
        [Authorize]
        public ActionResult RecurringBillingLogList(int? CustomerId, string SearchText, string TabStatus, string Start, string End, string order, string logstartdate, string logenddate, int pageno, int pagesize)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Customer CustomerModel = new Customer();
            CustomerModel = _Util.Facade.CustomerFacade.GetCustomerById((int)CustomerId);
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            List<string> invstatus = new List<string>();


            if (StartDate == new DateTime() && !string.IsNullOrWhiteSpace(Start))
            {
                StartDate = Convert.ToDateTime(Start).SetZeroHour();
            }
            if (EndDate == new DateTime() && !string.IsNullOrWhiteSpace(End))
            {
                EndDate = Convert.ToDateTime(End).SetMaxHour();

            }
            UserActivityCustomerModel Model = _Util.Facade.CustomerFacade.GetAllUserActivityForRMRCustomerListByCustomerId(pageno, pagesize, StartDate, EndDate, SearchText, CustomerModel.CustomerId, order, logstartdate, logenddate);
            ViewBag.CustomerId = CustomerId;
            ViewBag.PageNumber = pageno;
            ViewBag.OutOfNumber = 0;
            ViewBag.orderval = order;
            ViewBag.Logstartdate = logstartdate;
            ViewBag.Logenddate = logenddate;
            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "undefined")
            {
                ViewBag.searchtxt = SearchText;
            }
            else
            {
                ViewBag.searchtxt = "";
            }
            if (ViewBag.order == null)
            {
                ViewBag.order = 0;
            }

            if (Model.ListUserActivity != null && Model.ListUserActivity.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.InvoiceReportCountModel.TotalCount;
            }


            if ((int)ViewBag.PageNumber * pagesize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * pagesize;
            }
            ViewBag.Totalpagesize = (int)ViewBag.PageNumber * pagesize;
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / pagesize);

            List<SelectListItem> statusinv = new List<SelectListItem>();
            statusinv.Add(new SelectListItem()
            {
                Text = "Paid",
                Value = "Paid"
            });
            statusinv.Add(new SelectListItem()
            {
                Text = "Open",
                Value = "Open"
            });
            statusinv.Add(new SelectListItem()
            {
                Text = "Partial",
                Value = "Partial"
            });
            ViewBag.statusinv = statusinv;
            return View("RecurringBillingLogList", Model);
        }

        public void RMRCreditApplied(Invoice _Inv, Customer _customer, Guid RMRUserId, Guid RMRCompanyId)
        {
            #region Customer Credit Charged
            string InvCurrentStatus = _Inv.Status;
            double CustomerCreditAmount = _Util.Facade.TransactionFacade.GetCustomerCreditAmountByCustomerIdWithBoolValue(_customer.CustomerId, true, false);
            double Usedamount = 0;
            if (CustomerCreditAmount > 0)
            {
                CustomerCreditAmount = Math.Round(CustomerCreditAmount, 2);
                #region Invoice Update
                if (_Inv.BalanceDue.HasValue && _Inv.BalanceDue.Value > 0)
                {
                    if (CustomerCreditAmount >= _Inv.BalanceDue.Value)
                    {
                        Usedamount = _Inv.BalanceDue.Value;
                    }
                    else
                    {
                        Usedamount = CustomerCreditAmount;
                    }
                    _Inv.BalanceDue = Math.Round(_Inv.BalanceDue.HasValue ? _Inv.BalanceDue.Value : 0, 2) - Math.Round(Usedamount, 2);

                    if (_Inv.BalanceDue == 0)
                    {
                        _Inv.Status = LabelHelper.InvoiceStatus.Paid;
                    }
                    else
                    {
                        _Inv.Status = LabelHelper.InvoiceStatus.Partial;
                    }
                    _Inv.PaymentType = LabelHelper.PaymentMethod.CustomerCredit;
                    _Inv.LastUpdatedDate = DateTime.UtcNow;
                    _Util.Facade.InvoiceFacade.UpdateInvoice(_Inv);
                }
                #endregion
                #region User Payment Log
                if (InvCurrentStatus != _Inv.Status)
                {
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(RMRUserId);
                    string UserName = "";
                    if (empobj != null)
                    {
                        UserName = empobj.FirstName + " " + empobj.LastName;
                    }
                    else
                    {
                        UserName = "Customer";
                    }

                    #region Add Customer Credit Amount
                    string CustomerCreditNote = string.Format(@"Invoice# <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{0}')"">{1}</a>", _Inv.Id, _Inv.InvoiceId);
                    CustomerCredit CustomerCreditAmountDebit = new CustomerCredit()
                    {
                        Amount = Math.Round(Usedamount * -1, 2),
                        CompanyId = RMRCompanyId,
                        CreatedBy = RMRUserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = _customer.CustomerId,
                        TransactionId = 0,
                        Type = LabelHelper.CustomerCreditType.Debit,
                        IsRefund = false,
                        IsRMRCredit = true,
                        Note = CustomerCreditNote
                    };
                    _Util.Facade.TransactionFacade.InsertCustomerCredit(CustomerCreditAmountDebit);
                    #endregion
                    CustomerSnapshot objInvoicePayment = new CustomerSnapshot()
                    {
                        CustomerId = _customer.CustomerId,
                        CompanyId = RMRCompanyId,
                        Description = "Invoice " + string.Format("<a onclick=OpenTopToBottomModal('{3}/Invoice/AddInvoice?id={0}&CustomerId={1}') style='cursor: pointer;'>", _Inv.Id, _customer.Id, _customer.CustomerId, AppConfig.DomainSitePath) + "<b>" + _Inv.InvoiceId + "</b>" + "</a>" + " created for " + "<b>" + _customer.FirstName + " " + _customer.LastName + "</b>",
                        Logdate = DateTime.Now.UTCCurrentTime(),
                        Type = "InvoicePaymentHistory",
                        Updatedby = UserName
                    };
                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objInvoicePayment);

                    #region User Log
                    UserActivityCustomer uac = new UserActivityCustomer()
                    {
                        ActivityId = Guid.NewGuid(),
                        CustomerId = _customer.CustomerId,
                        RefId = _Inv.InvoiceId
                    };
                    _Util.Facade.UserActivityCustomerFacade.InsertUserActivityCustomer(uac);
                    UserActivity ua = new UserActivity()
                    {
                        ActivityId = uac.ActivityId,
                        PageUrl = "",
                        ReferrerUrl = "",
                        Action = "UpdateRecurringBillingInformationByCustomerModification,RecurringBilling",
                        StatsDate = DateTime.UtcNow,
                        UserId = RMRUserId,
                        UserName = UserName,
                        ActionDisplyText = "RMR customer credit applied amount $" + Usedamount.ToString("N2") + " for " + _Inv.InvoiceId + " by " + UserName,
                        IsARB = true,
                        UserAgent = AppConfig.GetUserAgent != null ? AppConfig.GetUserAgent : "",
                        UserIp = AppConfig.GetIP != null ? AppConfig.GetIP : ""
                    };
                    _Util.Facade.UserActivityFacade.InsertUserActivity(ua);

                    #endregion

                }
                #endregion
            }

            #endregion
        }
        public ActionResult RMRSystemSetting()
        {
            return View("rmrsystemsettings");
        }
        [Authorize]
        public JsonResult GetOnlyRMRServiceListByKey(string key, string ExistEquipment = "")
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetEquipmentSearchMaxLoad(CurrentUser.CompanyId.Value);

                List<EquipmentSearchModel> EqList = _Util.Facade.InvoiceFacade.GetOnlyRMRServiceListBySearchKeyAndCompanyId(key, CurrentUser.CompanyId.Value, ItemsLoadCount, ExistEquipment);
                if (EqList.Count > 0)
                    result = JsonConvert.SerializeObject(EqList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult RMRReports()
        {
            return View();
        }
        public ActionResult RecurringRMRReports()
        {
            //if (!base.IsPermitted(UserPermissions.MenuPermissions.LeftMenuSalesReports))
            //{
            //    return View("~/Views/Shared/_AccessDenied.cshtml");
            //}
            ViewBag.StartTab = "RecurringReportRMRTab";
            //if (base.IsPermitted(UserPermissions.ReportsPermissions.SalesReportSalesTab))
            //{
            //    ViewBag.StartTab = "SalesReportSalesTab";
            //}

            return View();
        }

        [Authorize]
        public ActionResult LoadRecurringReportPartial(bool? GetReport, string Start, string End, int pageno, int pagesize, string searchtxt, string invostatus, string Order, int? BillDay, string Interval, string BillingStatus, string BillingMethod)
        {
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();

            ViewBag.statusinv = 1;
            ViewBag.BillingDay = _Util.Facade.LookupFacade.GetDropdownsByKey("BillingDay");
            ViewBag.Status = _Util.Facade.LookupFacade.GetLookupByKey("RecurringBillingStatus").Where(x => x.IsActive == true && x.DataOrder > -1).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            ViewBag.PayrollBillingMethod = _Util.Facade.LookupFacade.GetLookupByKey("PayrollCustomerBillingMethod").OrderBy(x => x.DataValue != "-1").ThenBy(x => x.DisplayText).ToList().Select(x =>
              new SelectListItem()
              {
                  Text = x.DisplayText.ToString(),
                  Value = x.DataValue.ToString()
              }).ToList();
            List<SelectListItem> invoicestatuslist = new List<SelectListItem>();
            invoicestatuslist.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("InvoiceStatusForSales").Select(x => new SelectListItem()
            {
                Text = x.DataValue.ToString() == "-1" ? "Invoice Status" : x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.InvoiceStatus = invoicestatuslist;
            ViewBag.Cycle = _Util.Facade.LookupFacade.GetLookupByKey("BillCycle").Where(x => x.IsActive == true && x.DataOrder > -1).Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();

            if (GetReport.HasValue && GetReport == true)
            {
                if (!string.IsNullOrWhiteSpace(Order) && Order != "undefined")
                {
                    Order = Order;
                }
                DataTable dt;
                if (!string.IsNullOrWhiteSpace(Start) && Start != "undefined" && !string.IsNullOrWhiteSpace(End) && End != "undefined")
                {
                    StartDate = Convert.ToDateTime(Start).SetZeroHour().ClientToUTCTime();
                    EndDate = Convert.ToDateTime(End).SetMaxHour().ClientToUTCTime();
                    dt = _Util.Facade.CustomerFacade.GetReurringBillingScheduleListExportReport(searchtxt, BillDay, Interval, BillingMethod, BillingStatus, Order);
                }
                else
                {
                    if (StartDate != new DateTime() && EndDate != new DateTime())
                    {
                        dt = _Util.Facade.CustomerFacade.GetReurringBillingScheduleListExportReport(searchtxt, BillDay, Interval, BillingMethod, BillingStatus, Order);
                    }
                    else
                    {
                        dt = _Util.Facade.CustomerFacade.GetReurringBillingScheduleListExportReport(searchtxt, BillDay, Interval, BillingMethod, BillingStatus, Order);
                    }
                }

                dt.Columns.Remove("Id");
                dt.Columns.Remove("UnpaidCount");
                dt.Columns.Remove("rmrmid");
                int[] colarray = { 5 };
                int[] rowarray = { dt.Rows.Count + 2 };
                return MakeExcelFromDataTable(dt, "Reccurring Billing RMR Report", rowarray, colarray);
            }
            RecurringBillingScheduleReportModel Model = new RecurringBillingScheduleReportModel();
            Model = _Util.Facade.CustomerFacade.GetRecurringBilliingScheduleForReport(searchtxt, BillDay, Interval, BillingMethod, BillingStatus, pageno, pagesize, Order);
            ViewBag.PageNumber = pageno;
            ViewBag.OutOfNumber = 0;
            //ViewBag.order = null;
            //if (ViewBag.order == null)
            //{
            //    ViewBag.order = "";
            //}
            //else
            //{
            //    ViewBag.order = Order;
            //}
            if (!string.IsNullOrWhiteSpace(Order) && Order != "undefined")
            {
                ViewBag.order = Order;
            }
            if (Model != null && Model.ScheduleList != null && Model.ScheduleList.Count() > 0 && Order == "ascending/billday")
            {
                Model.ScheduleList = Model.ScheduleList.OrderBy(x => x.BillDay).ToList();
            }
            else if (Model != null && Model.ScheduleList != null && Model.ScheduleList.Count() > 0 && Order == "descending/billday")
            {
                Model.ScheduleList = Model.ScheduleList.OrderByDescending(x => x.BillDay).ToList();
            }
            if (Model.TemplateCount > 0)
            {
                ViewBag.OutOfNumber = Model.TemplateCount;
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
            ViewBag.searchtext = searchtxt;
            return View("_LoadRecurringReportPartial", Model);
        }


        public ActionResult LoadRmrMarginPartial()
        {
            return View();
        }
        public ActionResult LoadRmrMarginList(DateTime? Start, DateTime? End, string searchtext, int pageno, string order, bool? GetReport)
        {
            ViewBag.StartDate = Start;
            ViewBag.EndDate = End;
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            int Psize = _Util.Facade.GlobalSettingsFacade.GetLeadPageLength(CurrentUser.CompanyId.Value);
            if (Start != null && Start != new DateTime() && End != null && End != new DateTime())
            {
                StartDate = Start.Value.SetZeroHour();
                EndDate = End.Value.SetMaxHour();
            }

            if (GetReport.HasValue && GetReport.Value == true)
            {
                //DataTable dta = _Util.Facade.EmployeeFacade.GetAllTicketReworkDownload(StartDate, EndDate, searchtext, order, data.Start.ToString("yyyy/MM/dd hh:mm:ss.fff"), data.End.ToString("yyyy/MM/dd hh:mm:ss.fff"), data.Hours, data.WeekEnd);
                //int[] colarray = { };
                //int[] rowarray = { dta.Rows.Count + 2 };
                //int[] percentage = { 6 };
                //return MakeExcelFromDataTable2(dta, "Rework Report", rowarray, colarray, percentage);
            }
            MarginReportCustom model = new MarginReportCustom();

            model = _Util.Facade.CustomerFacade.GetAllMarginReport(StartDate, EndDate, searchtext, pageno, Psize, order);


            ViewBag.PageNumber = pageno;
            ViewBag.Order = order;
            ViewBag.OutOfNumber = 0;
            if (model.CustomerModel.Count > 0)
            {
                ViewBag.OutOfNumber = model.TotalCount;
            }

            if ((int)ViewBag.PageNumber * Psize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * Psize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / Psize);
            return View(model);
        }

        public ActionResult LoadRmrAuditPartial(CustomerFilter filter)
        {
            string outputstring = "";

            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerList))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ViewBag.BillingDay = _Util.Facade.LookupFacade.GetDropdownsByKey("BillingDay");
            ViewBag.Cycle = _Util.Facade.LookupFacade.GetLookupByKey("BillCycle").Where(x => x.IsActive == true && x.DataOrder > -1).Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();
            List<SelectListItem> ownerlist = new List<SelectListItem>();
            ownerlist.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            ownerlist.AddRange(_Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("OwnerShip", null, null).OrderBy(x => x.DisplayText != "Ownership").ThenBy(x => x.DisplayText).Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList());
            ViewBag.OwnerShipList = ownerlist;
            filter.EmployeeRole = currentLoggedIn.UserTags;
            filter.UserRole = currentLoggedIn.UserRole;
            filter.EmployeeId = currentLoggedIn.UserId.ToString();
            filter.CompanyId = currentLoggedIn.CompanyId.Value;
            filter.Partners = _Util.Facade.EmployeeFacade.GetEmployeeByPartnerId(currentLoggedIn.UserId);
            filter.isPermit = IsPermitted(UserPermissions.CustomerPermissions.ShowAllCustomerList);

            CustomerListWithCountModel customerfilterlist = new CustomerListWithCountModel();

            ViewBag.CustomerUiSetting = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettings().Where(x => x.Tag == "CustomerUiSettings").ToList();
            if (filter.PageNo == 0)
            {
                filter.PageNo = 1;
            }
            filter.PageSize = 50;


            GlobalSetting settingOrd = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "CustomerListOrder");
            if (settingOrd != null)
            {
                filter.SettingOrderBy = settingOrd.Value.ToString();
            }
            else
            {
                filter.SettingOrderBy = "Id desc";
            }


            customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilterAuditReccuring(filter);

            Session["GetCustomerFilter"] = filter;

            #region Paging Ready
            if (customerfilterlist.CustomerList.Count() == 0)
            {
                filter.PageNo = 1;
            }
            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = 0;

            if (customerfilterlist.CustomerList.Count() > 0)
            {
                ViewBag.OutOfNumber = customerfilterlist.TotalCustomerCount.Counter;
            }

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }
            #endregion

            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);
            return PartialView("_LoadRmrAuditPartial", customerfilterlist);
        }
        public ActionResult RecurringBillingInvoiceListPartial(string TemplateName)
        {
            ViewBag.TemplateName = "Recurring Template";
            return View();
        }

        private FileContentResult MakeExcelFromDataTable(DataTable dtResult, string ReportFor, int[] rowIndex, int[] coloumnIndex)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                if (dtResult != null)
                {

                    var worksheet = wb.Worksheets.Add(dtResult);
                    var format = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("CurrentCurrencyExcelFormat");
                    if (coloumnIndex != null && format != null && rowIndex != null)
                    {
                        foreach (int itemcol in coloumnIndex)
                        {
                            for (int i = 1; i < rowIndex[0]; i++)
                            {
                                worksheet.Cell(i, itemcol).Style.NumberFormat.Format = format.Value;

                            }
                        }
                    }
                    MemoryStream memorystreem = new MemoryStream();
                    wb.SaveAs(memorystreem);
                    var fName = string.Format("{0}-{1}.xlsx", ReportFor, DateTime.Now.UTCCurrentTime().ToString("MM/dd/yyyy"));
                    byte[] fileContents = memorystreem.ToArray();
                    var userAgent = HttpContext.Request.UserAgent.ToLower();
                    if (userAgent.Contains("iphone;") || userAgent.Contains("ipad;"))
                    {
                        return File(fileContents, Excel.Format("ExcelFormat"), fName);
                    }
                    else
                    {
                        return File(fileContents, Excel.Format("ExcelFormat"), fName);
                    }
                }
                else
                {
                    byte[] fileContents = new byte[1];
                    return File(fileContents, Excel.Format("ExcelFormat"), "empty.xlsx");
                }
            }
        }

        private bool MakeInvoice(RecurringBillingSchedule recurring)
        {
            bool result = false;
            try
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                DateTime today = DateTime.UtcNow.UTCToClientTime().SetZeroHour();
                Customer customer = _Util.Facade.CustomerFacade.GetCustomerById(recurring.CustomerId);
                List<Invoice> invList = new List<Invoice>();

                if (customer != null && customer.Id > 0)
                {
                    DateTime stDate = today.SetZeroHour();
                    List<DateTime> daylist = new List<DateTime>();
                    bool isnew = true;
                    if (recurring.PreviousDate.HasValue && recurring.PreviousDate.Value != new DateTime())
                    {
                        stDate = recurring.PreviousDate.Value.SetZeroHour();
                        isnew = false;
                    }
                    else
                    {
                        stDate = recurring.StartDate.Value.SetZeroHour();
                    }
                    DateTime newDate = stDate;
                    DateTime advanceinvoiceDate = today.AddDays(5);

                    #region Interval
                    do
                    {
                        if (recurring.BillCycle == LabelHelper.BillCycle.Annual)
                        {
                            newDate = stDate.AddYears(1);
                        }
                        else if (recurring.BillCycle == LabelHelper.BillCycle.Quarterly)
                        {
                            newDate = stDate.AddMonths(3);
                        }
                        else if (recurring.BillCycle == LabelHelper.BillCycle.SemiAnnually)
                        {
                            newDate = stDate.AddMonths(6);
                        }
                        else
                        {
                            newDate = stDate.AddMonths(1);
                        }
                        if (isnew)
                        {
                            daylist.Add(stDate);
                            daylist.Add(newDate);
                            isnew = false;
                        }
                        else
                        {
                            daylist.Add(newDate);
                        }
                        stDate = newDate;
                    }
                    while (newDate <= advanceinvoiceDate);

                    daylist.RemoveAt(daylist.Count - 1);
                    #endregion

                    string TaxType = "Non-Tax";
                    DateTime CreatedDate = DateTime.UtcNow;
                    if (recurring.TaxPercentage > 0)
                    {
                        TaxType = "Sales Tax";
                    }

                    if (daylist.Count() > 0)
                    {
                        List<RecurringBillingScheduleItems> RecurringItemsList = _Util.Facade.CustomerFacade.GetRecurringBillingScheduleItemsByScheduleId(recurring.ScheduleId);
                        Company com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(recurring.CompanyId);
                        var EmailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("PaidInvoicePredefineEmailTemplate");
                        foreach (var day in daylist)
                        {
                            advanceinvoiceDate = day.AddDays(5);
                            List<RecurringBillingScheduleItems> ItemsList = RecurringItemsList.Where(x => x.CycleStartDate.HasValue && x.CycleStartDate.Value <= advanceinvoiceDate).ToList();
                            if (ItemsList != null && ItemsList.Count() > 0)
                            {
                                #region InvoiceInsert      
                                double itemAmount = ItemsList.Sum(x => x.Amount);
                                double itemtaxAmount = ItemsList.Where(x => x.IsTaxable).Sum(x => x.Amount);
                                double itemtax = (itemtaxAmount != 0 ? (itemtaxAmount > 0 ? ((itemtaxAmount * recurring.TaxPercentage) / 100) : ((itemtaxAmount * recurring.TaxPercentage * -1) / 100)) : 0.00);
                                Invoice inv = new Invoice()
                                {
                                    CompanyId = recurring.CompanyId,
                                    Tax = itemtax.DoubleRound(2),
                                    TotalAmount = (itemAmount + itemtax).DoubleRound(2),
                                    BalanceDue = (itemAmount + itemtax).DoubleRound(2),
                                    Amount = itemAmount.DoubleRound(2),
                                    DiscountAmount = 0,
                                    Status = LabelHelper.InvoiceStatus.Open,
                                    DueDate = day,
                                    CreatedBy = CurrentUser.GetFullName(),
                                    CreatedDate = CreatedDate,
                                    InvoiceDate = day,
                                    CreatedByUid = CurrentUser.UserId,
                                    LastUpdatedDate = CreatedDate,
                                    LastUpdatedByUid = CurrentUser.UserId,
                                    LateAmount = 0,
                                    IsBill = false,
                                    IsEstimate = false,
                                    CustomerName = customer.FirstName + " " + customer.LastName,
                                    CustomerId = customer.CustomerId,
                                    BillingCycle = recurring.BillCycle,
                                    BillingAddress = recurring.BillingAddress,
                                    ShippingAddress = recurring.BillingAddress,
                                    TaxType = TaxType,
                                    Description = $"{recurring.MessageOnInvoice} {{ Payment Period up to {day.ToString("M/d/yy")} }}",
                                    IsARBInvoice = true,
                                    InvoiceEmailAddress = !string.IsNullOrWhiteSpace(recurring.EmailAddress) ? recurring.EmailAddress : customer.EmailAddress,
                                    TaxPercentage = recurring.TaxPercentage,
                                    PaymentType = recurring.PaymentMethod,
                                    BookingId = recurring.Id.ToString()
                                };
                                inv.Id = _Util.Facade.InvoiceFacade.InsertInvoice(inv);
                                inv.InvoiceId = inv.Id.GenerateInvoiceNo();
                                if (!string.IsNullOrWhiteSpace(recurring.PaymentMethod))
                                {
                                    var firstTwoChars = recurring.PaymentMethod.Length <= 3 ? recurring.PaymentMethod : recurring.PaymentMethod.Substring(0, 3);
                                    if (firstTwoChars.ToLower() == "cc_") { inv.InvoiceFor = LabelHelper.InvoiceFor.CreditCard; }
                                    else if (firstTwoChars.ToLower() == "ach") { inv.InvoiceFor = LabelHelper.InvoiceFor.ACH; }
                                    else { inv.InvoiceFor = LabelHelper.InvoiceFor.Invoice; }
                                }
                                else
                                {
                                    inv.Description = $"{recurring.MessageOnInvoice} {{ Payment Period up to {day.AddDays(14).ToString("M/d/yy")} }}";
                                    inv.InvoiceFor = LabelHelper.InvoiceFor.Invoice;
                                }
                                if (inv.InvoiceFor != LabelHelper.InvoiceFor.CreditCard && inv.InvoiceFor != LabelHelper.InvoiceFor.ACH)
                                {
                                    inv.DueDate = day.AddDays(14);
                                }
                                _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
                                invList.Add(inv);

                                #endregion

                                #region InvoiceItems

                                List<InvoiceDetail> invoiceDetailList = new List<InvoiceDetail>();
                                if (ItemsList != null && ItemsList.Count > 0)
                                {
                                    ItemsList = ItemsList.OrderBy(x => x.Id).ToList();
                                    foreach (var item in ItemsList)
                                    {
                                        InvoiceDetail invDet = new InvoiceDetail()
                                        {
                                            CompanyId = recurring.CompanyId,
                                            CreatedBy = CurrentUser.GetFullName(),
                                            CreatedDate = CreatedDate,
                                            InvoiceId = inv.InvoiceId,
                                            TotalPrice = item.Amount,
                                            UnitPrice = item.Rate,
                                            Quantity = item.Qty,
                                            EquipName = item.ProductName,
                                            EquipDetail = item.Description,
                                            Taxable = item.IsTaxable
                                        };
                                        invoiceDetailList.Add(invDet);
                                        _Util.Facade.InvoiceFacade.InsertInvoiceDetails(invDet);
                                    }
                                }
                                #endregion

                                #region Send Email
                                if (recurring.IsEInvoice.HasValue && recurring.IsEInvoice.Value)
                                {
                                    #region SendEmail
                                    string fullurl;
                                    var Temp_FileName1 = "";
                                    List<CreateInvoice> createinvoicelist = new List<CreateInvoice>();
                                    createinvoicelist.Add(new CreateInvoice() { Invoice = inv, InvoiceDetailList = invoiceDetailList });
                                    string filename = SaveInvoiceToPdf(createinvoicelist);
                                    try
                                    {
                                        string FromEmail = customer.EmailAddress;
                                        string EmailDescription = "";
                                        string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(inv.Id
                                            + "#"
                                            + inv.CompanyId
                                            + "#"
                                            + customer.CustomerId);
                                        fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Invoice/", encryptedurl);
                                        ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, customer.CustomerId);
                                        var FileName = filename;
                                        FileName = FileName.Split('/').Last();
                                        var Filepath = filename;
                                        string Full_Path = S3Domain + Filepath.TrimStart('/');
                                        WebClient webClient = new WebClient();
                                        byte[] FileDataInBytes = webClient.DownloadData(Full_Path);
                                        File(FileDataInBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName).ToString();
                                        decimal _fileSize = (decimal)FileDataInBytes.Length / 1024;
                                        _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);
                                        Temp_FileName1 = Server.MapPath("~/EmailFileCache/" + FileName);
                                        if (!System.IO.File.Exists(Temp_FileName1))
                                        {
                                            System.IO.File.WriteAllBytes(Temp_FileName1, FileDataInBytes);
                                        }
                                        else
                                        {
                                            System.IO.File.WriteAllBytes(Temp_FileName1, FileDataInBytes);
                                        }

                                        if (string.IsNullOrWhiteSpace(EmailTemplate.BodyContent))
                                        {
                                            string emailtemplateBody = string.Format("<p><span style='font-weight:600;'>Dear {0}</span>,<br /><br />Please find the attachment of your invoice for proof of payment.<br /><br />We appreciate the opportunity to give you an invoice on our products and services. If you have any questions, please call at {1}. <br /><br />Thank you for your business,<br />{2}<br />{3}</span></p>", customer.FirstName + " " + customer.LastName, com.Phone, CurrentUser.GetFullName(), com.CompanyName);
                                            EmailTemplate.BodyContent = emailtemplateBody;
                                        }

                                        EmailTemplate.BodyContent = EmailTemplate.BodyContent.Replace("##url##", ShortUrl.Code);
                                        InvoiceCreatedEmail email = new InvoiceCreatedEmail()
                                        {
                                            CompanyName = com.CompanyName,
                                            CustomerName = customer.FirstName + " " + customer.LastName,
                                            BalanceDue = inv.TotalAmount.HasValue ? "$" + inv.TotalAmount.Value.ToString("0,0.00") : HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency(),
                                            DueDate = inv.DueDate.HasValue ? inv.DueDate.Value.ToString("MM/dd/yy") : "",
                                            InvoiceId = inv.InvoiceId,
                                            ToEmail = customer.EmailAddress.Trim(),
                                            EmailBody = EmailDescription,
                                            Subject = EmailTemplate.Subject,
                                            CustomerId = customer.CustomerId.ToString(),
                                            EmployeeId = CurrentUser.UserId.ToString(),
                                            FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : "info@rmrcloud.com",
                                            FromName = CurrentUser.GetFullName(),
                                            InvoicePdf = new Attachment(
                                            Temp_FileName1,
                                            MediaTypeNames.Application.Octet)
                                        };
                                        bool responseEmailSent = _Util.Facade.MailFacade.SendInvoiceCreatedEmail(email, CurrentUser.CompanyId.Value);
                                        if (email.InvoicePdf != null)
                                        {
                                            email.InvoicePdf.Dispose();
                                        }

                                        if (responseEmailSent)
                                        {
                                            #region Log for Invoice Send
                                            base.AddUserActivityForCustomer("RMR Invoice is sent #Ref:" + inv.InvoiceId, LabelHelper.ActivityAction.SendInvoice, inv.CustomerId, customer.Id, inv.InvoiceId, true);
                                            #endregion
                                            CustomerSnapshot objEstimateEmailLog = new CustomerSnapshot()
                                            {
                                                CustomerId = customer.CustomerId,
                                                CompanyId = CurrentUser.CompanyId.Value,
                                                Description = "RMR Invoice:" + "  " + inv.InvoiceId + " " + "email sent by " + "<b>" + CurrentUser.GetFullName() + "</b>",
                                                Logdate = DateTime.UtcNow,
                                                Updatedby = CurrentUser.Identity.Name,
                                                Type = "CustomerMailHistory"
                                            };
                                            _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEstimateEmailLog);

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        result = false;
                                    }

                                    #endregion
                                }
                                #endregion
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                    Invoice LastInv = invList.LastOrDefault();
                    #region Update Customer      
                    customer.LastGeneratedInvoice = CreatedDate;
                    customer.LastUpdatedBy = CurrentUser.GetFullName();
                    _Util.Facade.CustomerFacade.UpdateCustomer(customer);
                    #endregion                    
                    #region Insert CustomerSnapshot
                    CustomerSnapshot objCustomerSnapshot = new CustomerSnapshot
                    {
                        CustomerId = customer.CustomerId,
                        CompanyId = recurring.CompanyId,
                        Description = "Invoice " + string.Format("<a onclick=OpenTopToBottomModal('{2}/Invoice/AddInvoice?id={0}&CustomerId={1}') style='cursor: pointer;'>", LastInv.Id, customer.Id, AppConfig.DomainSitePath) + "<b>" + LastInv.InvoiceId + "</b>" + "</a>",
                        Logdate = CreatedDate,
                        Updatedby = CurrentUser.GetFullName(),
                        Type = "InvoiceCreated"
                    };
                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objCustomerSnapshot);
                    #region Log for create new Invoice
                    base.AddUserActivityForCustomer("New RMR Invoice is created #Ref:" + LastInv.InvoiceId, LabelHelper.ActivityAction.AddInvoice, LastInv.CustomerId, customer.Id, LastInv.InvoiceId, true);
                    #endregion

                    #endregion

                    #region Bill cycle 
                    DateTime NextDate = LastInv.InvoiceDate.Value;
                    recurring.PreviousDate = NextDate;
                    if (recurring.BillCycle.ToLower() == "monthly") { NextDate = NextDate.AddMonths(1); }
                    else if (recurring.BillCycle.ToLower() == "quarterly") { NextDate = NextDate.AddMonths(3); }
                    else if (recurring.BillCycle.ToLower() == "semi-annually") { NextDate = NextDate.AddMonths(6); }
                    else if (recurring.BillCycle.ToLower() == "annually") { NextDate = NextDate.AddYears(1); }
                    else { NextDate = NextDate.AddMonths(1); }
                    #endregion
                    recurring.NextDate = NextDate;
                    recurring.PaymentCollectionDate = NextDate;
                    recurring.LastUpdatedDate = CreatedDate;
                    recurring.LastUpdatedBy = CurrentUser.UserId;
                    recurring.LastRMRInvoiceRefId = LastInv.InvoiceId;
                    _Util.Facade.CustomerFacade.UpdateRecurringBillingSchedule(recurring);


                    //#region Customer Credit
                    //if (recurring.Status.ToLower() == "freetrial")
                    //{
                    //    string CustomerCreditNote = string.Format(@"Invoice# <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{0}')"">{1}</a>", inv.Id, inv.InvoiceId);
                    //    CustomerCredit cc = new CustomerCredit()
                    //    {
                    //        Amount = Math.Round(inv.BalanceDue.Value, 2),
                    //        CompanyId = inv.CompanyId,
                    //        CreatedBy = CreatedByUid,
                    //        CreatedDate = CreatedDate,
                    //        CustomerId = inv.CustomerId,
                    //        TransactionId = 0,
                    //        Type = LabelHelper.CustomerCreditType.Credit,
                    //        IsRMRCredit = true,
                    //        Note = CustomerCreditNote
                    //    };
                    //    _Util.Facade.TransactionFacade.InsertCustomerCredit(cc);
                    //}
                    //#endregion
                    //#region Credit Card Payment Collection
                    //else if (inv.InvoiceFor == LabelHelper.InvoiceFor.CreditCard)
                    //{

                    //}
                    //#endregion
                    //#region ACH Payment Collection
                    //else if (inv.InvoiceFor == LabelHelper.InvoiceFor.ACH)
                    //{

                    //}
                    //#endregion
                    //inv.TotalAmount += SubTotalAmount;
                    //inv.BalanceDue += TotalDueAmount;
                    //if (SubTotalAmount == TotalDueAmount)
                    //{
                    //    inv.Status = LabelHelper.InvoiceStatus.Open;
                    //}
                    //else
                    //{
                    //    inv.Status = LabelHelper.InvoiceStatus.Partial;
                    //}
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        private string SaveInvoiceToPdf(List<CreateInvoice> InvoiceList)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            tempCom.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            List<CreateInvoice> CreateInvoList = new List<CreateInvoice>();
            string pdfname = "";

            string settingskey = @" 'InvoiceSettingsShipping', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
            List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CurrentUser.CompanyId.Value);
            GlobalSetting PaymentStubs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "InvPreviewPaymentStubs");

            if (PaymentStubs != null)
            {
                ViewBag.PaymentStubs = PaymentStubs.Value;
            }
            else
            {
                ViewBag.PaymentStubs = "";
            }

            if (InvoiceList.Count() > 0)
            {
                foreach (var Model in InvoiceList)
                {
                    if (Model.InvoiceDetailList == null && Model.InvoiceDetailList.Count() == 0)
                    {
                        continue;
                    }
                    Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Invoice.CustomerId);
                    if (tempCUstomer == null)
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(Model.Invoice.InvoiceId) && string.IsNullOrWhiteSpace(Model.Invoice.InvoiceId))
                    {
                        continue;
                    }
                    Invoice InvoiceInfo = Model.Invoice;
                    List<InvoiceDetail> InvoiceDetailListInfo = Model.InvoiceDetailList;
                    if (PermissionChecker.IsPermitted(PermissionList.InvoicePermissions.InvoiceDetailsLineItemDiscountAmountShow))
                    {
                        InvoiceInfo = _Util.Facade.InvoiceFacade.GetInvoiceByInvoiceId(Model.Invoice.InvoiceId);
                        if (InvoiceInfo == null) { InvoiceInfo = Model.Invoice; }
                        InvoiceDetailListInfo = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(Model.Invoice.InvoiceId);
                        if (InvoiceDetailListInfo == null && InvoiceDetailListInfo.Count == 0) { InvoiceDetailListInfo = Model.InvoiceDetailList; }
                    }
                    CreateInvoice processedModel = GetInvoiceModelById(InvoiceInfo, InvoiceDetailListInfo, tempCom, tempCUstomer);
                    processedModel.InvoiceSetting = new InvoiceSetting();
                    foreach (var print in printsetting)
                    {
                        if (print.Value.ToLower() == "true")
                        {
                            if (print.SearchKey == "InvoiceSettingsDeposit")
                            {
                                processedModel.InvoiceSetting.DepositSetting = true;
                            }
                            if (print.SearchKey == "InvoiceSettingsDiscount")
                            {
                                processedModel.InvoiceSetting.DiscountSetting = true;
                            }
                            if (print.SearchKey == "InvoiceSettingsShipping")
                            {
                                processedModel.InvoiceSetting.ShippingSetting = true;
                            }
                        }
                    }
                    processedModel.InvoiceNotes = _Util.Facade.InvoiceNoteFacade.GetAllInvoiceNoteByInvoiceIdAndCompanyId(processedModel.Invoice.Id, CurrentUser.CompanyId.Value);
                    foreach (var item in Model.InvoiceDetailList)
                    {
                        item.CreatedBy = User.Identity.Name;
                        item.CreatedDate = DateTime.Now.UTCCurrentTime();
                        item.CompanyId = CurrentUser.CompanyId.Value;
                        Model.SubTotal = Model.SubTotal + (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
                        item.EquipmentFile = _Util.Facade.EquipmentFileFacade.GetEquipmentFilesByEquipmentId(item.EquipmentId).Where(x => x.FileType == LabelHelper.EquipmentFileType.ProfilePicture).FirstOrDefault();
                        if (item.EquipmentFile == null)
                        {
                            item.EquipmentFile = new EquipmentFile();
                        }
                    }
                    InvoicePaymentDate PayDate = _Util.Facade.TransactionFacade.GetLatestPaymentDateByInvoiceId(processedModel.Invoice.Id);
                    if (PayDate != null)
                    {
                        processedModel.Invoice.TransacationDate = PayDate.PaymentDate;
                    }
                    CreateInvoList.Add(processedModel);
                }

                if (InvoiceList.Count() == 1)
                {
                    pdfname = InvoiceList[0].Invoice.InvoiceId;
                }
                else
                {
                    Random rand = new Random();
                    pdfname = "InvoiceList_" + rand.Next().ToString();
                }

            }
            ViewBag.CompanyId = tempCom.CompanyId.ToString();
            if (PermissionChecker.IsPermitted(PermissionList.InvoicePermissions.InvoiceDetailsLineItemDiscountAmountShow)) { ViewBag.DiscountShow = true; }
            else { ViewBag.DiscountShow = false; }
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Invoice/InvoicePdf.cshtml", CreateInvoList)
            {
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            #region File Save on AWS S3

            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            var tempFolderName = ConfigurationManager.AppSettings["File.InvoiceFiles"];

            var pdfTempFold = string.Format(tempFolderName, comname);
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName = tempFolderName.TrimEnd('/');
            tempFolderName += "/" + DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString();
            string FileName = pdfname + ".pdf";
            string FilePath = tempFolderName;
            string FileKey = string.Format($"{FilePath}/{FileName}");
            var task = Task.Run(async () =>
            {
                AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                await AWSobject.UploadFile(FileKey, applicationPDFData);
                await AWSobject.MakePublic(FileName, FilePath);
            });
            task.Wait();
            string returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
            returnurl = returnurl + FileKey;
            ViewBag.ReturnUrl = returnurl;
            ViewBag.FileName = FileName;
            ViewBag.FileKey = FileKey;
            decimal _fileSize = (decimal)applicationPDFData.Length / 1024;
            _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);
            #endregion
            if (CreateInvoList.Count() == 1)
            {
                List<InvoiceSessionModel> ModelList = new List<InvoiceSessionModel>();
                string SelectedInvoiceId = CreateInvoList.Select(x => x.Invoice.InvoiceId).FirstOrDefault();
                ModelList.Add(new InvoiceSessionModel
                {
                    FileName = FileKey,
                    InvoiceId = SelectedInvoiceId
                });
            }
            return returnurl;
        }
    }
}