using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
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
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Localize = HS.Web.UI.Helper.LanguageHelper;
using PermissionChecker = HS.Web.UI.Helper.PermissionHelper;
using PermissionList = HS.Framework.UserPermissions;

namespace HS.Web.UI.Controllers
{
    public class InvoiceController : BaseController
    {
        public InvoiceController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Invoice
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// InvoiceType => 'Paid' / 'Unpaid'
        /// </summary>
        public ActionResult InvoicePartial(Guid CustomerId, string InvoiceType)
        {
            ViewBag.InvoiceType = InvoiceType;
            ViewBag.CustomerId = CustomerId;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
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

            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            string PtoFilter = "-1";



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
            #region Common filter
            ViewBag.PTOFilterOptions = _Util.Facade.LookupFacade.GetLookupByKey("PTOFilterOptions").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString(),
                   Selected = x.DataValue == PtoFilter
               }).ToList();
            #endregion
            ViewBag.FirstDayOfWeek = FirstDayOfWeek;
            #endregion

            string UserRole = currentLoggedIn.UserRole;
            if (UserRole == "Customer")
            {
                ViewBag.Role = "Customer";
            }
            else
            {
                ViewBag.Role = "";
            }
            if (InvoiceType == "Paid" && base.IsPermitted(UserPermissions.CustomerPermissions.PaidInvoiceTab))
            {
                ViewBag.StartTab = "Paid";
            }
            else if (base.IsPermitted(UserPermissions.CustomerPermissions.UnpaidInvoiceTab))
            {
                ViewBag.StartTab = "Unpaid";
            }
            else if (base.IsPermitted(UserPermissions.CustomerPermissions.RolledOverInvoiceTab))
            {
                ViewBag.StartTab = "Rolled Over";
            }
            else if (base.IsPermitted(UserPermissions.CustomerPermissions.PaidInvoiceTab))
            {
                ViewBag.StartTab = "Paid";
            }
            var RolledOverPermission = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("ShowRolledOverTab");
            if (RolledOverPermission != null)
            {
                ViewBag.IsRolledOverPermission = RolledOverPermission.Value;
            }
            #region count
            if (CustomerId != new Guid())
            {
                CustomerDetailsTabCount customerDetailsTabCount = _Util.Facade.InvoiceFacade.GetCustomerDetailsTabCountsByCustomerId(CustomerId, CurrentUser.CompanyId.Value);
                ViewBag.OpenInvoiceCount = customerDetailsTabCount.OpenInvoiceCount;
                ViewBag.PaidInvoiceCount = customerDetailsTabCount.PaidInvoiceCount;
                ViewBag.RolledOverInvoiceCount = customerDetailsTabCount.RolledOverInvoiceCount;
            }


            #endregion
            return PartialView("_InvoicePartial");

        }
        
        public ActionResult FundingListPartial(Guid CustomerId, string InvoiceType)
        {
            ViewBag.InvoiceType = InvoiceType;
            if (InvoiceType == "Paid" && base.IsPermitted(UserPermissions.CustomerPermissions.PaidInvoiceTab))
            {
                ViewBag.StartTab = "Paid";
            }
            else if (base.IsPermitted(UserPermissions.CustomerPermissions.UnpaidInvoiceTab))
            {
                ViewBag.StartTab = "Unpaid";
            }
            else if (base.IsPermitted(UserPermissions.CustomerPermissions.RolledOverInvoiceTab))
            {
                ViewBag.StartTab = "Rolled Over";
            }
            else if (base.IsPermitted(UserPermissions.CustomerPermissions.PaidInvoiceTab))
            {
                ViewBag.StartTab = "Paid";
            }
            return PartialView("_InvoicePartial");

        }
        public PartialViewResult InvoiceListPartial(Guid CustomerId, string InvoiceType, CustomerFilter filter)
        {
            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerInvoiceList)
                && !base.IsPermitted(UserPermissions.CustomerDashBoard.CustomerInvoiceList))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            ViewBag.InvoiceType = InvoiceType;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            //bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId, currentLoggedIn.CompanyId.Value);
            //if (!res)
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}


            int PageLimit = _Util.Facade.GlobalSettingsFacade.GetCustomerInvoicePageLimit(currentLoggedIn.CompanyId.Value);
            if (filter.PageNo == 0)
            {
                filter.PageNo = 1;
            }

            if (filter.PageSize < PageLimit)
            {
                filter.PageSize = PageLimit;
            }
            //Customer tmpCustomer = _Util.Facade.CustomerFacade.GetById(CustomerId);
            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            List<Invoice> InvoiceList = new List<Invoice>();
            bool isDeclinedAdded = true;
            GlobalSetting isDeclined = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "IsDeclinedInvoiceAddedInUnpaidAmount");
            if (isDeclined != null)
            {
                isDeclinedAdded = !string.IsNullOrWhiteSpace(isDeclined.Value) && isDeclined.Value.ToLower() == "false" ? false : true;
            }
            InvoiceList = _Util.Facade.InvoiceFacade.GetAllInvoiceByCompanyIdAndCustomerId(currentLoggedIn.CompanyId.Value, tmpCustomer.CustomerId, InvoiceType, filter, isDeclinedAdded);
            //System.Web.HttpRuntime.Cache["GetAllInvoiceId"] = InvoiceList;
            var TotalInvoiceCount = _Util.Facade.InvoiceFacade.GetAllInvoice1ByCompanyIdAndCustomerId(currentLoggedIn.CompanyId.Value, tmpCustomer.CustomerId, InvoiceType, isDeclinedAdded);
            if (InvoiceList.Count() == 0)
            {
                filter.PageNo = 1;
                //customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilter(filter);
            }

            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = 0;


            if (InvoiceList.Count() > 0)
            {
                ViewBag.OutOfNumber = TotalInvoiceCount.Count();
            }

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }


            string UserRole = currentLoggedIn.UserRole;
            if (UserRole == "Customer")
            {
                ViewBag.Role = "Customer";
            }
            else
            {
                ViewBag.Role = "";
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);
            ViewBag.invoicetype = InvoiceType;
            string isDeclinedAddedCheck = "true";
            GlobalSetting isDeclinedCheck = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "IsDeclinedInvoiceAddedInUnpaidAmount");
            if (isDeclinedCheck != null)
            {
                isDeclinedAddedCheck = !string.IsNullOrWhiteSpace(isDeclinedCheck.Value) && isDeclinedCheck.Value.ToLower() == "false" ? "false" : "true";
            }
            ViewBag.IsDeclined = isDeclinedAddedCheck.ToLower();
            //List<InvoiceDetail> invoiceDetailsList = _Util.Facade.InvoiceFacade.GetAllInvoiceDetailsByCompanyId(currentLoggedIn.CompanyId.Value);
            return PartialView("_InvoiceListPartial", InvoiceList);

        }
        //no need to use
        //public PartialViewResult FilterInvoiceListPartial(int CustomerId, string InvoiceType, CustomerFilter filter)
        //{
        //    if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerInvoiceList))
        //    {
        //        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
        //    }
        //    var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
        //    bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId, currentLoggedIn.CompanyId.Value);
        //    if (!res)
        //    {
        //        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
        //    }
        //    int PageLimit = _Util.Facade.GlobalSettingsFacade.GetCustomerInvoicePageLimit(currentLoggedIn.CompanyId.Value);
        //    if (filter.PageNo == 0)
        //    {
        //        filter.PageNo = 1;
        //    }
        //    ViewBag.order = filter.order;
        //    ViewBag.InvoiceType = InvoiceType;
        //    if (filter.PageSize < PageLimit)
        //    {
        //        filter.PageSize = PageLimit;
        //    }
        //    Customer tmpCustomer = _Util.Facade.CustomerFacade.GetById(CustomerId);
        //    List<Invoice> InvoiceList = new List<Invoice>();
        //    InvoiceList = _Util.Facade.InvoiceFacade.GetAllInvoiceByCompanyIdAndCustomerId(currentLoggedIn.CompanyId.Value, tmpCustomer.CustomerId, InvoiceType, filter);
        //    var TotalInvoiceCount = _Util.Facade.InvoiceFacade.GetAllInvoice1ByCompanyIdAndCustomerId(currentLoggedIn.CompanyId.Value, tmpCustomer.CustomerId, InvoiceType);
        //    if (InvoiceList.Count() == 0)
        //    {
        //        filter.PageNo = 1;
        //        //customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilter(filter);
        //    }

        //    ViewBag.PageNumber = filter.PageNo;
        //    ViewBag.OutOfNumber = 0;
        //    ViewBag.order = filter.order;

        //    if (InvoiceList.Count() > 0)
        //    {
        //        ViewBag.OutOfNumber = TotalInvoiceCount.Count();
        //    }

        //    if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
        //    {
        //        ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
        //    }
        //    else
        //    {
        //        ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
        //    }
        //    ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);
        //    //List<InvoiceDetail> invoiceDetailsList = _Util.Facade.InvoiceFacade.GetAllInvoiceDetailsByCompanyId(currentLoggedIn.CompanyId.Value);
        //    return PartialView("FilterInvoiceListPartial", InvoiceList);

        //}
        [Authorize]
        public PartialViewResult AddInvoice(int? id, string InvoiceId, int? CustomerId, Guid? CustomerGuid)
        {
            CreateInvoice model = new CreateInvoice();
            Customer tempCustomer = new Customer();
            try
            {
                var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
                model.Invoice.CompanyId = currentLoggedIn.CompanyId.Value;
                //string dateTime = "";
                DateTime dateTime = new DateTime();


                #region Retrive Or Create Invoice
                if (id.HasValue && id > 0)
                {
                    model.Invoice = _Util.Facade.InvoiceFacade.GetById(id.Value);
                }
                else if (!string.IsNullOrWhiteSpace(InvoiceId))
                {
                    model.Invoice = _Util.Facade.InvoiceFacade.GetByInvoiceId(InvoiceId);
                }
                else
                {
                    model.Invoice.IsEstimate = false;
                    model.Invoice.IsBill = false;
                    model.Invoice.CreatedBy = User.Identity.Name;
                    model.Invoice.CreatedByUid = currentLoggedIn.UserId;
                    model.Invoice.LastUpdatedByUid = currentLoggedIn.UserId;
                    model.Invoice.InvoiceFor = "Invoice";
                    model.Invoice.Id = _Util.Facade.InvoiceFacade.InsertInvoice(model.Invoice);
                    model.Invoice.InvoiceId = model.Invoice.Id.GenerateInvoiceNo();
                    //_Util.Facade.InvoiceFacade.UpdateInvoice(model.Invoice);
                }

                if (model.Invoice == null || model.Invoice.CompanyId != currentLoggedIn.CompanyId.Value)
                {
                    InvoiceId = string.IsNullOrEmpty(InvoiceId) ? id.Value.ToString() : InvoiceId;
                    ViewBag.ContentIdText = $"Invoice Id #{InvoiceId}";
                    return PartialView("~/Views/Shared/_NotFoundGeneric.cshtml");
                }
                else
                {
                    model.PaymentHistoryList = _Util.Facade.InvoiceFacade.GetPaymentHistoryByInvoiceId(model.Invoice.Id);
                }

                //InvoiceId = InvoiceId ?? id.Value.ToString();
                InvoiceId = InvoiceId ?? model.Invoice.InvoiceId;
                CustomerId = CustomerId ?? model.Invoice.CustomerIntId;
                CustomerGuid = CustomerGuid ?? model.Invoice.CustomerId;

                if (model.Invoice.Status != "Init")
                {
                    
                    model.InvoiceDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetialsListByInvoiceId(model.Invoice.InvoiceId);
                    model.InvoiceNotes = _Util.Facade.InvoiceNoteFacade.GetAllInvoiceNoteByInvoiceIdAndCompanyId(model.Invoice.Id, currentLoggedIn.CompanyId.Value);
                    List<TransactionHistory> Trhs = _Util.Facade.TransactionFacade.GetAllTransactionHistoryByInvoiceId(model.Invoice.Id);
                    if (Trhs.Count() > 0)
                    {
                        model.Invoice.AmountReceived = Trhs.Sum(x => x.Amout);
                    }
                    InvoicePaymentDate PayDate = _Util.Facade.TransactionFacade.GetLatestPaymentDateByInvoiceId(model.Invoice.Id);

                    if (PayDate != null && PayDate.PaymentDate != null && PayDate.PaymentDate != new DateTime())
                    {
                        dateTime = PayDate.PaymentDate;//PayDate.PaymentDate.ToString("M/d/yy");
                    }
                }
                model.JobNo = model.Invoice.JobNo;
                model.Invoice.LastUpdatedByUid = currentLoggedIn.UserId;
                #endregion

                #region retrive customer info

                if (CustomerId.HasValue && CustomerId > 0)
                {
                    tempCustomer = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);
                }
                else 
                {
                    tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerGuid.Value);
                }

                if (tempCustomer == null)
                {
                    ViewBag.ContentIdText = $"Customer Id #{CustomerId.Value}";
                    return PartialView("~/Views/Shared/_NotFoundGeneric.cshtml");
                }

                model.Invoice.CustomerId = tempCustomer.CustomerId;
                model.Invoice.CustomerName = tempCustomer.DisplayName;
                if (!string.IsNullOrWhiteSpace(tempCustomer.BusinessName))
                {
                    model.CusBussinessName = tempCustomer.BusinessName;
                }
                //model.Invoice.InvoiceEmailAddress = tempCustomer.EmailAddress;
                model.EmailAddress = tempCustomer.EmailAddress;
                if (string.IsNullOrWhiteSpace(model.Invoice.InvoiceEmailAddress))
                {
                    model.Invoice.InvoiceEmailAddress = tempCustomer.EmailAddress;
                }
                var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(currentLoggedIn.CompanyId.Value);

                if (tempCustomer.Type == "Commercial")
                {
                    model.CusType = tempCustomer.Type;
                }
                model.Invoice.BillingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "BillingAddress", AddressTemplate);
                model.Invoice.ShippingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "ShippingAddress", AddressTemplate);
                //model.Invoice.CreatedByUid = currentLoggedIn.UserId;

                //bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(tempCustomer.CustomerId, currentLoggedIn.CompanyId.Value);
                //if (!res)
                //{
                //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                //}
                #endregion

                if (!string.IsNullOrWhiteSpace(model.Invoice.EstimateTerm) && model.Invoice.EstimateTerm != "-1" && model.Invoice.EstimateTerm == "50UponAcceptance50UponCompletion")
                    model.Invoice.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(model.Invoice.EstimateTerm);

                #region ViewBags
                //Response.Cache.SetExpires(DateTime.Now);
                
                //ViewBag.CustomerList = _Util.Facade.CustomerFacade
                //.GetAllCustomersByCompanyId(currentLoggedIn.CompanyId.Value);
                ViewBag.ShowShipping = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(currentLoggedIn.CompanyId.Value);
                ViewBag.PaymentDate = dateTime;

                HttpContext.Cache[RMRCacheKey.EquipmentServiceList] = HttpContext.Cache[RMRCacheKey.EquipmentServiceList] ?? _Util.Facade.EquipmentFacade
                    .GetAllEquipmentServiceByCompanyId(currentLoggedIn.CompanyId.Value).Select(x =>
                      new SelectListItem()
                      {
                          Text = x.Name.ToString() + " " + x.SKU.ToString(),
                          Value = x.EquipmentId.ToString()
                      }).ToList();
                ViewBag.EquipmentServiceList = HttpContext.Cache[RMRCacheKey.EquipmentServiceList];

                HttpContext.Cache[RMRCacheKey.DiscountMethod]= HttpContext.Cache[RMRCacheKey.DiscountMethod]?? _Util.Facade.LookupFacade.GetLookupByKey("DiscountMethod").Select(x =>
                               new SelectListItem()
                               {
                                   Text = x.DisplayText.ToString(),
                                   Value = x.DataValue.ToString()
                               }).ToList();

                ViewBag.DiscountMethod = HttpContext.Cache[RMRCacheKey.DiscountMethod];

                //HttpContext.Cache[RMRCacheKey.TermList] = HttpContext.Cache[RMRCacheKey.TermList] ?? _Util.Facade.LookupFacade.GetLookupByKey("InvoiceTerms").Select(x =>
                // new SelectListItem()
                // {
                //     Text = x.DisplayText.ToString(),
                //     Value = x.DataValue.ToString()
                // }).ToList();
                List<SelectListItem> TermList = TermListCache; // (List<SelectListItem>)HttpContext.Cache[RMRCacheKey.TermList];



                // condition for rug tracker
                if (model.Invoice.Terms == "Custom" /*&& model.Invoice.CompanyId.ToString() == "46FE75B2-B321-4645-90AF-D6ACC64B9AF6"*/)
                {
                    TermList.Add(new SelectListItem()
                    {
                        Text = "Custom",
                        Value = "Custom"
                    });
                }
                ViewBag.TermList = TermList;

                HttpContext.Cache[RMRCacheKey.InvoiceMessage] = HttpContext.Cache[RMRCacheKey.InvoiceMessage] ?? _Util.Facade.GlobalSettingsFacade.GetInvoiceMessageByCompanyId(currentLoggedIn.CompanyId.Value);
                ViewBag.InvoiceMessage = HttpContext.Cache[RMRCacheKey.InvoiceMessage];

                Dictionary<string, string> InvoiceForList = new Dictionary<string, string>();
                //System.Web.HttpCookie myCookie = new System.Web.HttpCookie("__InvoiceForList");

                InvoiceForList= (Dictionary<string, string>)HttpContext.Cache[RMRCacheKey.InvoiceForList] ?? _Util.Facade.LookupFacade.GetLookupByKey("InvoiceForList").ToDictionary(x => x.DisplayText, y => y.DataValue);

                #region Cookie Helper
                //if (Response.Cookies.AllKeys.Contains("__InvoiceForList"))
                //{
                //    myCookie = Response.Cookies["__InvoiceForList"];
                //    foreach (var ckey in myCookie.Values.AllKeys)
                //    {
                //        InvoiceForList.Add(ckey, myCookie.Values.Get(ckey));
                //    }
                //}
                //else
                //{
                //    InvoiceForList = _Util.Facade.LookupFacade.GetLookupByKey("InvoiceForList").ToDictionary(x => x.DisplayText, y => y.DataValue);
                //    foreach (KeyValuePair<string, string> val in InvoiceForList)
                //    {
                //        myCookie.Values.Add(val.Key,val.Value);
                //    }
                //    myCookie.Expires = DateTime.Now.AddDays(1);
                //    //myCookie.Domain = "localhost";
                //    //myCookie.HttpOnly=true;
                //    Response.Cookies.Add(myCookie);
                //    Session["__InvoiceForList"] = InvoiceForList;
                //}
                #endregion Cookie Helper

                ViewBag.InvoiceForList = InvoiceForList.Select(x =>
                               new SelectListItem()
                               {
                                   Text = x.Key,
                                   Value = x.Value
                               }).ToList();

                //ViewBag.InvoiceForList = _Util.Facade.LookupFacade.GetLookupByKey("InvoiceForList").Select(x =>
                //               new SelectListItem()
                //               {
                //                   Text = x.DisplayText.ToString(),
                //                   Value = x.DataValue.ToString()
                //               }).ToList();

                

                List<SelectListItem> TicketList = new List<SelectListItem>();
                
                TicketList.Add(new SelectListItem()
                {
                    Text = "Select One",
                    Value = "-1"
                });
                List<Ticket> ExistTicketList = null;  _Util.Facade.TicketFacade.GetAllTicketByCustomerId(tempCustomer.CustomerId);
                if (ExistTicketList != null)
                {
                    TicketList.AddRange(ExistTicketList.Select(x => new SelectListItem()
                    {
                        Text = x.Id.ToString(),
                        Value = x.TicketId.ToString()
                    }).ToList());
                }
                ViewBag.TicketList = TicketList;
                #endregion

                #region View for TaxList
                List<SelectListItem> TaxListItem = new List<SelectListItem>();
                //var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(tempCustomer.CustomerId, currentLoggedIn.CompanyId.Value);
                //if (GetCityTaxList.Count > 0)
                //{
                //    foreach (var item in GetCityTaxList)
                //    {
                //        TaxListItem.Add(new SelectListItem()
                //        {
                //            Text = " [" + item.City.ToString() + "-" + item.State.ToString() + "]",
                //            Value = item.Rate.ToString()
                //        });
                //    }
                //}
                //else
                //{
                //    var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(currentLoggedIn.CompanyId.Value, tempCustomer.CustomerId);
                //    if (GetSalesTax != null)
                //    {
                //        TaxListItem.Add(new SelectListItem()
                //        {
                //            Text = GetSalesTax.SearchKey.ToString(),
                //            Value = GetSalesTax.Value.ToString()
                //        });
                //    }
                //}

                var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(currentLoggedIn.CompanyId.Value, tempCustomer.CustomerId);
                if (GetSalesTax != null)
                {
                    TaxListItem.Add(new SelectListItem()
                    {
                        Text = GetSalesTax.SearchKey.ToString(),
                        Value = GetSalesTax.Value.ToString()
                    });
                }

                TaxListItem.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("NonTaxValue").Select(x => new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList());
                //var GetOutOfStateTax = _Util.Facade.GlobalSettingsFacade.GetOutOfStateTax(currentLoggedIn.CompanyId.Value);
                //if (GetOutOfStateTax != null)
                //{
                //    TaxListItem.Add(new SelectListItem()
                //    {
                //        Text = GetOutOfStateTax.SearchKey.ToString(),
                //        Value = GetOutOfStateTax.Value.ToString(),
                //        Selected = (model.Invoice.Tax.HasValue && model.Invoice.Tax.Value == 0)
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
                ViewBag.TaxListItem = TaxListItem;
                #endregion

                #region Invoice Settings

                string settingskey = @" 'InvoiceSettingsShipping', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, currentLoggedIn.CompanyId.Value);
                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            model.InvoiceSetting.DepositSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsDiscount")
                        {
                            model.InvoiceSetting.DiscountSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsShipping")
                        {
                            model.InvoiceSetting.ShippingSetting = true;
                        }
                    }
                }
                #endregion
                var shippingStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceShippingSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
                var DiscountStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceDiscountSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
                var DipositStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceDepositSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
                if (shippingStutas != null)
                {
                    ViewBag.ShowShipping = shippingStutas.IsActive.Value;
                }
                if (DiscountStutas != null)
                {
                    ViewBag.DiscountValue = DiscountStutas.IsActive.Value;
                }
                if (DipositStutas != null)
                {
                    ViewBag.DipositValue = DipositStutas.IsActive.Value;
                }

                //ViewBag.StopWatch = watch.ElapsedMilliseconds.ToString();
                //watch.Stop();
                ViewBag.InvoiceStatus = _Util.Facade.LookupFacade.GetLookupByKey("InvoiceStatus").Select(x =>
                               new SelectListItem()
                               {
                                   Text = x.DisplayText.ToString(),
                                   Value = x.DataValue.ToString()
                               }).ToList();

                //Session[SessionKeys.InvoicePdfSession] = null; 

                #region Payment Methods added
                if (currentLoggedIn.UserTags == "Customer")
                {
                    List<SelectListItem> PaymentMethods = new List<SelectListItem>();
                    PaymentMethods.Add(new SelectListItem()
                    {
                        Text = "Please Select One",
                        Value = "-1"
                    });
                    PaymentMethods.Add(new SelectListItem()
                    {
                        Text = "ACH",
                        Value = "ACH"
                    });
                    PaymentMethods.Add(new SelectListItem()
                    {
                        Text = "Credit Card",
                        Value = "Credit Card"
                    });
                    if (!string.IsNullOrWhiteSpace(tempCustomer.AuthorizeRefId))
                    {
                        PaymentMethods.Add(new SelectListItem()
                        {
                            Text = string.Format("Customer Payment Profile"),
                            Value = "CustomerProfile"
                        });
                    }

                    if (base.IsPermitted(UserPermissions.CustomerPermissions.ReceivePaymentCreditMemo))
                    {
                        PaymentMethods.Add(new SelectListItem()
                        {
                            Text = string.Format("Credit Memo"),
                            Value = "Credit Memo"
                        });
                    }
                    //if (Model.GeneralCreditAmount > 0)
                    //{
                    //    PaymentMethods.Add(new SelectListItem()
                    //    {
                    //        Text = string.Format("General Credit [{0}{1}]", LabelHelper.CurrentTransMakeCurrency.MakeCurrency(CurrentUser.CompanyId.Value), string.Format("{0:0,0.00}", Model.GeneralCreditAmount)),
                    //        Value = "General Customer Credit"
                    //    });
                    //}
                    //if (Model.RMRCreditAmount > 0)
                    //{
                    //    PaymentMethods.Add(new SelectListItem()
                    //    {
                    //        Text = string.Format("RMR Credit [{0}{1}]", LabelHelper.CurrentTransMakeCurrency.MakeCurrency(CurrentUser.CompanyId.Value), string.Format("{0:0,0.00}", Model.RMRCreditAmount)),
                    //        Value = "RMRCustomerCredit"
                    //    });
                    //}
                    List<PaymentProfileCustomer> PaymentProfileList = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentProfileCustomerByCustomerId(model.Invoice.CustomerId);
                    if (PaymentProfileList != null && PaymentProfileList.Where(x => x.Type != "Invoice" && x.Type != "Financed" && x.Type != "Cash" && x.Type.IndexOf("CHK_") == -1).Count() > 0)
                    {
                        PaymentProfileList = PaymentProfileList.Where(x => x.Type != "Invoice" && x.Type != "Financed" && x.Type != "Cash" && x.Type.IndexOf("CHK_") == -1).ToList();
                        foreach (var item in PaymentProfileList)
                        {
                            PaymentMethods.Add(new SelectListItem()
                            {
                                Text = item.Type,
                                Value = "PP_" + item.Id
                            });
                        }
                    }
                    ViewBag.PaymentMethods = PaymentMethods;
                }
                else
                {
                    List<SelectListItem> Paymentmethods = _Util.Facade.LookupFacade.GetLookupByKey("PaymentMethodInvoice").Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.DisplayText.ToString(),
                                 Value = x.DataValue.ToString()
                             }).ToList();

                    if (!string.IsNullOrWhiteSpace(tempCustomer.AuthorizeRefId))
                    {
                        Paymentmethods.Add(new SelectListItem()
                        {
                            Text = string.Format("Customer Payment Profile"),
                            Value = "CustomerProfile"
                        });
                    }
                    if (base.IsPermitted(UserPermissions.CustomerPermissions.ReceivePaymentCreditMemo))
                    {
                        Paymentmethods.Add(new SelectListItem()
                        {
                            Text = string.Format("Credit Memo"),
                            Value = "Credit Memo"
                        });
                    }
                    if (base.IsPermitted(UserPermissions.CustomerPermissions.PaymentMethodOthers))
                    {
                        Paymentmethods.Add(new SelectListItem()
                        {
                            Text = string.Format("Others"),
                            Value = "Others"
                        });
                    }
                    //if (Model.GeneralCreditAmount > 0)
                    //{
                    //    Paymentmethods.Add(new SelectListItem()
                    //    {
                    //        Text = string.Format("General Credit [{0}{1}]", LabelHelper.CurrentTransMakeCurrency.MakeCurrency(CurrentUser.CompanyId.Value), string.Format("{0:0,0.00}", Model.GeneralCreditAmount)),
                    //        Value = "General Customer Credit"
                    //    });
                    //}

                    //if (Model.RMRCreditAmount > 0)
                    //{
                    //    Paymentmethods.Add(new SelectListItem()
                    //    {
                    //        Text = string.Format("RMR Credit [{0}{1}]", LabelHelper.CurrentTransMakeCurrency.MakeCurrency(CurrentUser.CompanyId.Value), string.Format("{0:0,0.00}", Model.RMRCreditAmount)),
                    //        Value = "RMRCustomerCredit"
                    //    });
                    //}
                    List<PaymentProfileCustomer> PaymentProfileList = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentProfileCustomerByCustomerId(model.Invoice.CustomerId);
                    if (PaymentProfileList != null && PaymentProfileList.Where(x => x.Type != "Invoice" && x.Type != "Financed" && x.Type != "Cash" && x.Type.IndexOf("CHK_") == -1).Count() > 0)
                    {
                        PaymentProfileList = PaymentProfileList.Where(x => x.Type != "Invoice" && x.Type != "Financed" && x.Type != "Cash" && x.Type.IndexOf("CHK_") == -1).ToList();
                        foreach (var item in PaymentProfileList)
                        {
                            Paymentmethods.Add(new SelectListItem()
                            {
                                Text = item.Type,
                                Value = "PP_" + item.Id
                            });
                        }

                    }
                    ViewBag.PaymentMethods = Paymentmethods;
                }
                #endregion Payment Methods added

                if (string.IsNullOrEmpty(model.Invoice.PaymentMethod))
                {
                    model.Invoice.PaymentMethod = "-1";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                //TODO: need to pass more details to the user in case of exception and close modal.
            }
            return PartialView("_AddInvoice", model);
        }

        [Authorize]
        public PartialViewResult AddInvoice_O1(int? id, string InvoiceId, int? CustomerId, Guid? CustomerGuid)
        {
            //if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerInvoiceAdd))
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}

            //Stopwatch watch = new Stopwatch();
            //watch.Start();

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            CreateInvoice model = new CreateInvoice();
            Customer tempCustomer = new Customer();
            string dateTime = "";
            #region Retrive Or Create Invoice
            if ((id.HasValue && id.Value > 0) || !string.IsNullOrWhiteSpace(InvoiceId))
            {
                model = new CreateInvoice();
                if (id.HasValue && id > 0)
                {
                    model.Invoice = _Util.Facade.InvoiceFacade.GetById(id.Value);
                }
                else
                {
                    model.Invoice = _Util.Facade.InvoiceFacade.GetByInvoiceId(InvoiceId);
                }

                if (model.Invoice == null || model.Invoice.CompanyId != currentLoggedIn.CompanyId.Value)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                else
                {
                    model.PaymentHistoryList = _Util.Facade.InvoiceFacade.GetPaymentHistoryByInvoiceId(model.Invoice.Id);
                }
                tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(model.Invoice.CustomerId);
                if (tempCustomer == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                //bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(tempCustomer.CustomerId, currentLoggedIn.CompanyId.Value);
                //if (!res)
                //{
                //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                //}
                model.Invoice.CustomerName = tempCustomer.DisplayName;
                if (!string.IsNullOrWhiteSpace(tempCustomer.BusinessName))
                {
                    model.CusBussinessName = tempCustomer.BusinessName;
                }
                model.InvoiceDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetialsListByInvoiceId(model.Invoice.InvoiceId);

                if (string.IsNullOrWhiteSpace(model.Invoice.InvoiceEmailAddress))
                {
                    model.Invoice.InvoiceEmailAddress = tempCustomer.EmailAddress;
                }
                model.JobNo = model.Invoice.JobNo;
                model.InvoiceNotes = _Util.Facade.InvoiceNoteFacade.GetAllInvoiceNoteByInvoiceIdAndCompanyId(model.Invoice.Id, currentLoggedIn.CompanyId.Value);
                List<TransactionHistory> Trhs = _Util.Facade.TransactionFacade.GetAllTransactionHistoryByInvoiceId(model.Invoice.Id);
                if (Trhs.Count() > 0)
                {
                    model.Invoice.AmountReceived = Trhs.Sum(x => x.Amout);
                }
                InvoicePaymentDate PayDate = _Util.Facade.TransactionFacade.GetLatestPaymentDateByInvoiceId(model.Invoice.Id);

                if (PayDate != null && PayDate.PaymentDate != null && PayDate.PaymentDate != new DateTime())
                {
                    dateTime = PayDate.PaymentDate.ToString("M/d/yy");
                }
                ViewBag.ShowShipping = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(currentLoggedIn.CompanyId.Value);
                if (!string.IsNullOrWhiteSpace(model.Invoice.EstimateTerm) && model.Invoice.EstimateTerm != "-1" && model.Invoice.EstimateTerm == "50UponAcceptance50UponCompletion")
                    model.Invoice.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(model.Invoice.EstimateTerm);
            }
            else
            {
                #region retrive customer info
                if (CustomerId.HasValue)
                {
                    tempCustomer = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);
                }
                else if (CustomerGuid.HasValue)
                {
                    tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerGuid.Value);
                }
                if (tempCustomer == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                //bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(tempCustomer.CustomerId, currentLoggedIn.CompanyId.Value);
                //if (!res)
                //{
                //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                //}
                #endregion

                model = new CreateInvoice();
                model.EmailAddress = tempCustomer.EmailAddress;
                var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(currentLoggedIn.CompanyId.Value);

                model.Invoice = new Invoice()
                {
                    CustomerId = tempCustomer.CustomerId,
                    CustomerName = tempCustomer.DisplayName,
                    IsEstimate = false,
                    IsBill = false,
                    InvoiceEmailAddress = tempCustomer.EmailAddress,
                    CompanyId = currentLoggedIn.CompanyId.Value,
                    Amount = 0,
                    BalanceDue = 0,
                    Balance = 0,
                    Deposit = 0,
                    Tax = 0,
                    LateFee = 0,
                    LateAmount = 0,
                    Status = "Init",
                    CreatedDate = DateTime.Now,
                    CreatedBy = User.Identity.Name,
                    InvoiceFor = "Others",
                    InvoiceDate = DateTime.Now,
                    DueDate = DateTime.Now,
                    CreatedByUid = currentLoggedIn.UserId,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedByUid = currentLoggedIn.UserId,
                    PaymentType = "-1"
                };
                if (!string.IsNullOrWhiteSpace(tempCustomer.BusinessName))
                {
                    model.CusBussinessName = tempCustomer.BusinessName;
                }
                if (tempCustomer.Type == "Commercial")
                {
                    model.CusType = tempCustomer.Type;
                }
                model.Invoice.BillingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "BillingAddress", AddressTemplate);
                model.Invoice.ShippingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "ShippingAddress", AddressTemplate);
                //model.Invoice.CreatedByUid = currentLoggedIn.UserId;
                model.Invoice.LastUpdatedByUid = currentLoggedIn.UserId;
                model.Invoice.Id = _Util.Facade.InvoiceFacade.InsertInvoice(model.Invoice);
                model.Invoice.InvoiceId = model.Invoice.Id.GenerateInvoiceNo();
                model.Invoice.InvoiceFor = "Invoice";
                _Util.Facade.InvoiceFacade.UpdateInvoice(model.Invoice);

                //model.Invoice.InvoiceDate = model.Invoice.InvoiceDate.Value.UTCToClientTime();
                //model.Invoice.DueDate = model.Invoice.DueDate.Value.UTCToClientTime();

                model.InvoiceDetailList = new List<InvoiceDetail>();
            }
            #endregion

            #region ViewBags
            //ViewBag.CustomerList = _Util.Facade.CustomerFacade
            //.GetAllCustomersByCompanyId(currentLoggedIn.CompanyId.Value);
            ViewBag.PaymentDate = dateTime;
            ViewBag.EquipmentServiceList = _Util.Facade.EquipmentFacade
                .GetAllEquipmentServiceByCompanyId(currentLoggedIn.CompanyId.Value).Select(x =>
                  new SelectListItem()
                  {
                      Text = x.Name.ToString() + " " + x.SKU.ToString(),
                      Value = x.EquipmentId.ToString()
                  }).ToList();

            ViewBag.DiscountMethod = _Util.Facade.LookupFacade.GetLookupByKey("DiscountMethod").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();

            List<SelectListItem> TermList = _Util.Facade.LookupFacade.GetLookupByKey("InvoiceTerms").Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();

            // condition for rug tracker
            if (model.Invoice.Terms == "Custom" /*&& model.Invoice.CompanyId.ToString() == "46FE75B2-B321-4645-90AF-D6ACC64B9AF6"*/)
            {
                TermList.Add(new SelectListItem()
                {
                    Text = "Custom",
                    Value = "Custom"
                });
            }
            ViewBag.TermList = TermList;

            ViewBag.InvoiceMessage = _Util.Facade.GlobalSettingsFacade.GetInvoiceMessageByCompanyId(currentLoggedIn.CompanyId.Value);

            ViewBag.InvoiceForList = _Util.Facade.LookupFacade.GetLookupByKey("InvoiceForList").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();

            List<SelectListItem> TicketList = new List<SelectListItem>();
            TicketList.Add(new SelectListItem()
            {
                Text = "Select One",
                Value = "-1"
            });
            var ExistTicketList = _Util.Facade.TicketFacade.GetAllTicketByCustomerId(tempCustomer.CustomerId);
            if (ExistTicketList != null)
            {
                TicketList.AddRange(ExistTicketList.Select(x => new SelectListItem()
                {
                    Text = x.Id.ToString(),
                    Value = x.TicketId.ToString()
                }).ToList());
            }
            ViewBag.TicketList = TicketList;
            #endregion

            #region View for TaxList

            //var CustomerInfo = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId);

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

            TaxListItem.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("NonTaxValue").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            var GetOutOfStateTax = _Util.Facade.GlobalSettingsFacade.GetOutOfStateTax(currentLoggedIn.CompanyId.Value);
            if (GetOutOfStateTax != null)
            {
                TaxListItem.Add(new SelectListItem()
                {
                    Text = GetOutOfStateTax.SearchKey.ToString(),
                    Value = GetOutOfStateTax.Value.ToString(),
                    Selected = (model.Invoice.Tax.HasValue && model.Invoice.Tax.Value == 0)
                });
            }
            var GetNonProfitTax = _Util.Facade.GlobalSettingsFacade.GetNonProfitTax(currentLoggedIn.CompanyId.Value);
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

            #region Invoice Settings

            string settingskey = @" 'InvoiceSettingsShipping', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
            List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, currentLoggedIn.CompanyId.Value);
            model.InvoiceSetting = new InvoiceSetting();
            foreach (var print in printsetting)
            {
                if (print.Value.ToLower() == "true")
                {
                    if (print.SearchKey == "InvoiceSettingsDeposit")
                    {
                        model.InvoiceSetting.DepositSetting = true;
                    }
                    if (print.SearchKey == "InvoiceSettingsDiscount")
                    {
                        model.InvoiceSetting.DiscountSetting = true;
                    }
                    if (print.SearchKey == "InvoiceSettingsShipping")
                    {
                        model.InvoiceSetting.ShippingSetting = true;
                    }
                }
            }
            #endregion
            var shippingStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceShippingSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            var DiscountStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceDiscountSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            var DipositStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceDepositSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            if (shippingStutas != null)
            {
                ViewBag.ShowShipping = shippingStutas.IsActive.Value;
            }
            if (DiscountStutas != null)
            {
                ViewBag.DiscountValue = DiscountStutas.IsActive.Value;
            }
            if (DipositStutas != null)
            {
                ViewBag.DipositValue = DipositStutas.IsActive.Value;
            }

            //ViewBag.StopWatch = watch.ElapsedMilliseconds.ToString();
            //watch.Stop();
            ViewBag.InvoiceStatus = _Util.Facade.LookupFacade.GetLookupByKey("InvoiceStatus").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();

            //Session[SessionKeys.InvoicePdfSession] = null; 

            #region Payment Methods added
            if (currentLoggedIn.UserTags == "Customer")
            {
                List<SelectListItem> PaymentMethods = new List<SelectListItem>();
                PaymentMethods.Add(new SelectListItem()
                {
                    Text = "Please Select One",
                    Value = "-1"
                });
                PaymentMethods.Add(new SelectListItem()
                {
                    Text = "ACH",
                    Value = "ACH"
                });
                PaymentMethods.Add(new SelectListItem()
                {
                    Text = "Credit Card",
                    Value = "Credit Card"
                });
                if (!string.IsNullOrWhiteSpace(tempCustomer.AuthorizeRefId))
                {
                    PaymentMethods.Add(new SelectListItem()
                    {
                        Text = string.Format("Customer Payment Profile"),
                        Value = "CustomerProfile"
                    });
                }

                if (base.IsPermitted(UserPermissions.CustomerPermissions.ReceivePaymentCreditMemo))
                {
                    PaymentMethods.Add(new SelectListItem()
                    {
                        Text = string.Format("Credit Memo"),
                        Value = "Credit Memo"
                    });
                }
                //if (Model.GeneralCreditAmount > 0)
                //{
                //    PaymentMethods.Add(new SelectListItem()
                //    {
                //        Text = string.Format("General Credit [{0}{1}]", LabelHelper.CurrentTransMakeCurrency.MakeCurrency(CurrentUser.CompanyId.Value), string.Format("{0:0,0.00}", Model.GeneralCreditAmount)),
                //        Value = "General Customer Credit"
                //    });
                //}
                //if (Model.RMRCreditAmount > 0)
                //{
                //    PaymentMethods.Add(new SelectListItem()
                //    {
                //        Text = string.Format("RMR Credit [{0}{1}]", LabelHelper.CurrentTransMakeCurrency.MakeCurrency(CurrentUser.CompanyId.Value), string.Format("{0:0,0.00}", Model.RMRCreditAmount)),
                //        Value = "RMRCustomerCredit"
                //    });
                //}
                List<PaymentProfileCustomer> PaymentProfileList = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentProfileCustomerByCustomerId(model.Invoice.CustomerId);
                if (PaymentProfileList != null && PaymentProfileList.Where(x => x.Type != "Invoice" && x.Type != "Financed" && x.Type != "Cash" && x.Type.IndexOf("CHK_") == -1).Count() > 0)
                {
                    PaymentProfileList = PaymentProfileList.Where(x => x.Type != "Invoice" && x.Type != "Financed" && x.Type != "Cash" && x.Type.IndexOf("CHK_") == -1).ToList();
                    foreach (var item in PaymentProfileList)
                    {
                        PaymentMethods.Add(new SelectListItem()
                        {
                            Text = item.Type,
                            Value = "PP_" + item.Id
                        });
                    }

                }

                ViewBag.PaymentMethods = PaymentMethods;
            }
            else
            {
                List<SelectListItem> Paymentmethods = _Util.Facade.LookupFacade.GetLookupByKey("PaymentMethodInvoice").Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString()
                         }).ToList();

                if (!string.IsNullOrWhiteSpace(tempCustomer.AuthorizeRefId))
                {
                    Paymentmethods.Add(new SelectListItem()
                    {
                        Text = string.Format("Customer Payment Profile"),
                        Value = "CustomerProfile"
                    });
                }
                if (base.IsPermitted(UserPermissions.CustomerPermissions.ReceivePaymentCreditMemo))
                {
                    Paymentmethods.Add(new SelectListItem()
                    {
                        Text = string.Format("Credit Memo"),
                        Value = "Credit Memo"
                    });
                }
                if (base.IsPermitted(UserPermissions.CustomerPermissions.PaymentMethodOthers))
                {
                    Paymentmethods.Add(new SelectListItem()
                    {
                        Text = string.Format("Others"),
                        Value = "Others"
                    });
                }
                //if (Model.GeneralCreditAmount > 0)
                //{
                //    Paymentmethods.Add(new SelectListItem()
                //    {
                //        Text = string.Format("General Credit [{0}{1}]", LabelHelper.CurrentTransMakeCurrency.MakeCurrency(CurrentUser.CompanyId.Value), string.Format("{0:0,0.00}", Model.GeneralCreditAmount)),
                //        Value = "General Customer Credit"
                //    });
                //}
                //if (Model.RMRCreditAmount > 0)
                //{
                //    Paymentmethods.Add(new SelectListItem()
                //    {
                //        Text = string.Format("RMR Credit [{0}{1}]", LabelHelper.CurrentTransMakeCurrency.MakeCurrency(CurrentUser.CompanyId.Value), string.Format("{0:0,0.00}", Model.RMRCreditAmount)),
                //        Value = "RMRCustomerCredit"
                //    });
                //}
                List<PaymentProfileCustomer> PaymentProfileList = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentProfileCustomerByCustomerId(model.Invoice.CustomerId);
                if (PaymentProfileList != null && PaymentProfileList.Where(x => x.Type != "Invoice" && x.Type != "Financed" && x.Type != "Cash" && x.Type.IndexOf("CHK_") == -1).Count() > 0)
                {
                    PaymentProfileList = PaymentProfileList.Where(x => x.Type != "Invoice" && x.Type != "Financed" && x.Type != "Cash" && x.Type.IndexOf("CHK_") == -1).ToList();
                    foreach (var item in PaymentProfileList)
                    {
                        Paymentmethods.Add(new SelectListItem()
                        {
                            Text = item.Type,
                            Value = "PP_" + item.Id
                        });
                    }

                }
                ViewBag.PaymentMethods = Paymentmethods;
            }
            #endregion Payment Methods added

            if (string.IsNullOrEmpty(model.Invoice.PaymentMethod))
            {
                model.Invoice.PaymentMethod = "-1";
            }

            return PartialView("_AddInvoice", model);
        }


        [Authorize]
        public PartialViewResult AddInvoiceCustomer(int? id, string InvoiceId, int? CustomerId, Guid? CustomerGuid)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            CreateInvoice model = new CreateInvoice();
            Customer tempCustomer = new Customer();
            #region Retrive Or Create Invoice
            if ((id.HasValue && id.Value > 0) || !string.IsNullOrWhiteSpace(InvoiceId))
            {
                model = new CreateInvoice();
                if (id.HasValue && id > 0)
                {
                    model.Invoice = _Util.Facade.InvoiceFacade.GetById(id.Value);
                }
                else
                {
                    model.Invoice = _Util.Facade.InvoiceFacade.GetByInvoiceId(InvoiceId);
                }

                if (model.Invoice == null || model.Invoice.CompanyId != currentLoggedIn.CompanyId.Value)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.Invoice.CustomerId);
                if (tempCustomer == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(tempCustomer.CustomerId, currentLoggedIn.CompanyId.Value);
                if (!res)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model.Invoice.CustomerName = tempCustomer.FirstName + " " + tempCustomer.LastName;
                if (!string.IsNullOrWhiteSpace(tempCustomer.BusinessName))
                {
                    model.CusBussinessName = tempCustomer.BusinessName;
                }
                model.InvoiceDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetialsListByInvoiceId(model.Invoice.InvoiceId);
                model.EmailAddress = tempCustomer.EmailAddress;
                model.InvoiceNotes = _Util.Facade.InvoiceNoteFacade.GetAllInvoiceNoteByInvoiceIdAndCompanyId(model.Invoice.Id, currentLoggedIn.CompanyId.Value);
                List<TransactionHistory> Trhs = _Util.Facade.TransactionFacade.GetAllTransactionHistoryByInvoiceId(model.Invoice.Id);
                if (Trhs.Count() > 0)
                {
                    model.Invoice.AmountReceived = Trhs.Sum(x => x.Amout);
                }
                ViewBag.ShowShipping = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(currentLoggedIn.CompanyId.Value);
                if (!string.IsNullOrWhiteSpace(model.Invoice.EstimateTerm) && model.Invoice.EstimateTerm != "-1" && model.Invoice.EstimateTerm == "50UponAcceptance50UponCompletion")
                    model.Invoice.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(model.Invoice.EstimateTerm);
            }
            else
            {
                #region retrive customer info
                if (CustomerId.HasValue)
                {
                    tempCustomer = _Util.Facade.CustomerFacade.GetCustomerById(CustomerId.Value);
                }
                else if (CustomerGuid.HasValue)
                {
                    tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerGuid.Value);
                }
                if (tempCustomer == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(tempCustomer.CustomerId, currentLoggedIn.CompanyId.Value);
                if (!res)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                #endregion

                model = new CreateInvoice();
                model.EmailAddress = tempCustomer.EmailAddress;
                var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(currentLoggedIn.CompanyId.Value);

                model.Invoice = new Invoice()
                {
                    CustomerId = tempCustomer.CustomerId,
                    CustomerName = tempCustomer.FirstName + " " + tempCustomer.LastName,
                    IsEstimate = false,
                    IsBill = false,
                    CompanyId = currentLoggedIn.CompanyId.Value,
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
                    InvoiceFor = "Others",
                    InvoiceDate = DateTime.Now.UTCCurrentTime(),
                    DueDate = DateTime.Now.UTCCurrentTime(),
                    CreatedByUid = currentLoggedIn.UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedByUid = currentLoggedIn.UserId
                };
                if (!string.IsNullOrWhiteSpace(tempCustomer.BusinessName))
                {
                    model.CusBussinessName = tempCustomer.BusinessName;
                }
                if (tempCustomer.Type == "Commercial")
                {
                    model.CusType = tempCustomer.Type;
                }
                model.Invoice.BillingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "BillingAddress", AddressTemplate);
                model.Invoice.ShippingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "ShippingAddress", AddressTemplate);
                //model.Invoice.CreatedByUid = currentLoggedIn.UserId;
                model.Invoice.LastUpdatedByUid = currentLoggedIn.UserId;
                model.Invoice.Id = _Util.Facade.InvoiceFacade.InsertInvoice(model.Invoice);
                model.Invoice.InvoiceId = model.Invoice.Id.GenerateInvoiceNo();
                model.Invoice.InvoiceFor = "Invoice";
                _Util.Facade.InvoiceFacade.UpdateInvoice(model.Invoice);

                //model.Invoice.InvoiceDate = model.Invoice.InvoiceDate.Value.UTCToClientTime();
                //model.Invoice.DueDate = model.Invoice.DueDate.Value.UTCToClientTime();

                model.InvoiceDetailList = new List<InvoiceDetail>();
            }
            #endregion

            #region ViewBags
            //ViewBag.CustomerList = _Util.Facade.CustomerFacade
            //.GetAllCustomersByCompanyId(currentLoggedIn.CompanyId.Value);

            ViewBag.EquipmentServiceList = _Util.Facade.EquipmentFacade
                .GetAllEquipmentServiceByCompanyId(currentLoggedIn.CompanyId.Value).Select(x =>
                  new SelectListItem()
                  {
                      Text = x.Name.ToString() + " " + x.SKU.ToString(),
                      Value = x.EquipmentId.ToString()
                  }).ToList();

            ViewBag.DiscountMethod = _Util.Facade.LookupFacade.GetLookupByKey("DiscountMethod").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();

            ViewBag.Term = _Util.Facade.LookupFacade.GetLookupByKey("InvoiceTerms").OrderBy(x => x.DataValue != "0").ThenBy(x => x.DisplayText).Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();

            ViewBag.InvoiceMessage = _Util.Facade.GlobalSettingsFacade.GetInvoiceMessageByCompanyId(currentLoggedIn.CompanyId.Value);
            #endregion

            #region View for TaxList

            //var CustomerInfo = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId);

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

            TaxListItem.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("NonTaxValue").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            var GetOutOfStateTax = _Util.Facade.GlobalSettingsFacade.GetOutOfStateTax(currentLoggedIn.CompanyId.Value);
            if (GetOutOfStateTax != null)
            {
                TaxListItem.Add(new SelectListItem()
                {
                    Text = GetOutOfStateTax.SearchKey.ToString(),
                    Value = GetOutOfStateTax.Value.ToString(),
                    Selected = (model.Invoice.Tax.HasValue && model.Invoice.Tax.Value == 0)
                });
            }
            var GetNonProfitTax = _Util.Facade.GlobalSettingsFacade.GetNonProfitTax(currentLoggedIn.CompanyId.Value);
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

            #region Invoice Settings

            string settingskey = @" 'InvoiceSettingsShipping', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
            List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, currentLoggedIn.CompanyId.Value);
            model.InvoiceSetting = new InvoiceSetting();
            foreach (var print in printsetting)
            {
                if (print.Value.ToLower() == "true")
                {
                    if (print.SearchKey == "InvoiceSettingsDeposit")
                    {
                        model.InvoiceSetting.DepositSetting = true;
                    }
                    if (print.SearchKey == "InvoiceSettingsDiscount")
                    {
                        model.InvoiceSetting.DiscountSetting = true;
                    }
                    if (print.SearchKey == "InvoiceSettingsShipping")
                    {
                        model.InvoiceSetting.ShippingSetting = true;
                    }
                }
            }
            #endregion
            var shippingStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceShippingSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            var DiscountStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceDiscountSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            var DipositStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceDepositSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            if (shippingStutas != null)
            {
                ViewBag.ShowShipping = shippingStutas.IsActive.Value;
            }
            if (DiscountStutas != null)
            {
                ViewBag.DiscountValue = DiscountStutas.IsActive.Value;
            }
            if (DipositStutas != null)
            {
                ViewBag.DipositValue = DipositStutas.IsActive.Value;
            }

            //ViewBag.StopWatch = watch.ElapsedMilliseconds.ToString();
            //watch.Stop();
            ViewBag.InvoiceStatus = _Util.Facade.LookupFacade.GetLookupByKey("InvoiceStatus").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();

            //Session[SessionKeys.InvoicePdfSession] = null; 

            return PartialView("_AddInvoiceCustomer", model);
        }
        [Authorize]
        public JsonResult InvoiceTicketSave(int InvoiceId, Guid TicketId)
        {
            bool result = false;
            if (InvoiceId > 0 && TicketId != Guid.Empty)
            {
                var invoiceDetails = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                if (invoiceDetails != null)
                {
                    invoiceDetails.TicketId = TicketId;
                }
                result = _Util.Facade.InvoiceFacade.UpdateInvoice(invoiceDetails);
            }
            return Json(new { result = result });
        }
        [Authorize]
        public JsonResult InvoiceStatusSave(int InvoiceId, string InvoiceStatus)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(InvoiceStatus))
            {
                var invoiceDetails = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                if (invoiceDetails != null)
                {
                    invoiceDetails.Status = InvoiceStatus;
                }
                result = _Util.Facade.InvoiceFacade.UpdateInvoice(invoiceDetails);
            }
            return Json(new { result = result });
        }
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public JsonResult AddInvoice_O1(CreateInvoice Model, bool SendEmail, bool CreatePdf, int? id, string ccEmail)
        {
            WebClient webClient;
            byte[] FileDataInBytes;

            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerInvoiceAdd))
            {
                return Json(new { result = false, message = "Permission denied." });
            }

            bool EmailSent = false;
            bool NewTransaction = false;
            string statuslogmsg = "";
            string newterms = "";
            string oldterms = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (Model.Invoice == null)
            {
                if (id.HasValue)
                {
                    Model.Invoice = _Util.Facade.InvoiceFacade.GetInvoiceById(id.Value);
                    Model.InvoiceDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(Model.Invoice.InvoiceId);
                }
            }
            if (Model.Invoice != null && Model.InvoiceDetailList.Count > 0)
            {
                #region if Model.Invoice != null
                Invoice tempInvo = _Util.Facade.InvoiceFacade.GetByInvoiceId(Model.Invoice.InvoiceId);
                List<InvoiceDetail> tempInvodetail = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(Model.Invoice.InvoiceId.ToString());

                if (tempInvo == null || tempInvo.CompanyId != CurrentUser.CompanyId.Value)
                {
                    return Json(new { result = false });
                }
                if (Model.InvoiceDetailList == null)
                {
                    return Json(new { result = false, message = "Customer Equipment Found" });
                }
                Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Invoice.CustomerId);
                if (tempCUstomer == null)
                {
                    return Json(new { result = false, message = "Customer Not Found" });
                }
                if (Model.Invoice.InvoiceDate.HasValue && Model.Invoice.DueDate.HasValue)
                {
                    if (Model.Invoice.DueDate.Value < Model.Invoice.InvoiceDate.Value)
                    {
                        return Json(new { result = false, message = "Due date should be greater than or equal to invoice date." });
                    }
                }

                Model.Invoice.Id = tempInvo.Id;
                Model.Invoice.IsEstimate = false;
                Model.Invoice.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;
                Model.CustomerName = Model.Invoice.CustomerName;
                Model.Invoice.CreatedBy = tempInvo.CreatedBy;
                Model.Invoice.CreatedDate = tempInvo.CreatedDate;
                Model.Invoice.LastUpdatedDate = tempInvo.LastUpdatedDate;
                Model.Invoice.CreatedByUid = tempInvo.CreatedByUid;
                Model.Invoice.BillingCycle = tempInvo.BillingCycle;
                Model.Invoice.LastUpdatedByUid = tempInvo.LastUpdatedByUid;
                if (string.IsNullOrWhiteSpace(ccEmail))
                {
                    Model.Invoice.InvoiceCcEmailAddress = tempInvo.InvoiceCcEmailAddress;
                }
                Model.Invoice.ccEmail = ccEmail;

                Model.JobNo = tempInvo.JobNo;

                //Model.Invoice.Terms = tempInvo.Terms;
                //_Util.Facade.UserActivityFacade.AddElmah("before: " + Model.Invoice.InvoiceDate.ToString());
                //DateTime myDateTime = DateTime.SpecifyKind(Model.Invoice.InvoiceDate.Value, DateTimeKind.Utc);
                //_Util.Facade.UserActivityFacade.AddElmah("After: " + myDateTime.ToString());
                //_Util.Facade.UserActivityFacade.AddElmah("ToUniversalTime : " + Model.Invoice.InvoiceDate.Value.ToUniversalTime().ToString());
                /*if (Model.Invoice.InvoiceDate.HasValue)
                {
                    _Util.Facade.UserActivityFacade.AddElmah("before: " + Model.Invoice.InvoiceDate.ToString());
                    Model.Invoice.InvoiceDate = Model.Invoice.InvoiceDate.Value.ToUniversalTime();
                    _Util.Facade.UserActivityFacade.AddElmah("After: " + Model.Invoice.InvoiceDate.ToString());
                }
                if (Model.Invoice.DueDate.HasValue)
                {
                    _Util.Facade.UserActivityFacade.AddElmah("before: " + Model.Invoice.DueDate.ToString());
                    Model.Invoice.DueDate = Model.Invoice.DueDate.Value.ToUniversalTime();
                    _Util.Facade.UserActivityFacade.AddElmah("After: " + Model.Invoice.DueDate.ToString());
                }*/
                if (Model.Invoice.InvoiceDate.HasValue)
                {
                    Model.Invoice.InvoiceDate = Model.Invoice.InvoiceDate.Value.SetZeroHour();
                }
                if (Model.Invoice.DueDate.HasValue)
                {
                    Model.Invoice.DueDate = Model.Invoice.DueDate.Value.SetMaxHour();
                }
                if (Model.Invoice.ShippingDate.HasValue)
                {
                    Model.Invoice.ShippingDate = Model.Invoice.ShippingDate.Value.SetMaxHour();
                }

                //Model.Invoice.CustomerId = tempInvo.CustomerId;
                Model.Invoice.CompanyId = CurrentUser.CompanyId.Value;
                Model.Invoice.Amount = Math.Round(Model.Invoice.Amount, 2);
                Model.Invoice.TotalAmount = Math.Round(Model.Invoice.TotalAmount.HasValue ? Model.Invoice.TotalAmount.Value : 0, 2);
                Model.Invoice.BalanceDue = Math.Round(Model.Invoice.BalanceDue.HasValue ? Model.Invoice.BalanceDue.Value : 0, 2);
                Model.Invoice.DiscountType = tempInvo.DiscountType;

                if (tempInvo.DiscountType=="Percent")
                {
                    Model.Invoice.DiscountAmount=tempInvo.DiscountAmount;
                    Model.Invoice.Discountpercent = tempInvo.Discountpercent;
                }
                else if (tempInvo.DiscountType == "Amount")
                {
                    Model.Invoice.DiscountAmount = tempInvo.DiscountAmount;
                    Model.Invoice.Discountpercent = 0.00;
                }

                //Model.Invoice.Status = "Open";
                if (Model.Invoice.BalanceDue == 0)
                {
                    Model.Invoice.Status = "Paid";
                }
                else if (Model.Invoice.BalanceDue < Model.Invoice.TotalAmount)
                {
                    Model.Invoice.Status = LabelHelper.InvoiceStatus.Partial;
                }
                else
                {
                    Model.Invoice.Status = LabelHelper.InvoiceStatus.Open;
                }
                Model.Invoice.Tax = Math.Round(Model.Invoice.Tax.HasValue ? Model.Invoice.Tax.Value : 0, 2);
                logger.WithProperty("tags", "invoices,taxation")
                    .WithProperty("params", JsonConvert.SerializeObject(Model.Invoice))
                    .Trace($"Invoice data for #{Model.Invoice.Id} by user {CurrentUser.GetFullName()}");
                Model.Invoice.Deposit = Math.Round(Model.Invoice.Deposit.HasValue ? Model.Invoice.Deposit.Value : 0, 2);
                Model.Invoice.ShippingCost = Math.Round(Model.Invoice.ShippingCost.HasValue ? Model.Invoice.ShippingCost.Value : 0, 2);
                foreach (var item in Model.InvoiceDetailList)
                {
                    item.CreatedBy = User.Identity.Name;
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    item.CompanyId = CurrentUser.CompanyId.Value;
                    if (item.TotalPrice.HasValue)
                    {
                        Model.SubTotal = Model.SubTotal + item.TotalPrice.Value;

                    }
                    else
                    {
                        Model.SubTotal = Model.SubTotal;
                    }
                    item.EquipmentFile = _Util.Facade.EquipmentFileFacade.GetEquipmentFilesByEquipmentId(item.EquipmentId).FirstOrDefault();
                    if (item.EquipmentFile == null)
                    {
                        item.EquipmentFile = new EquipmentFile();
                    }
                }
                if (tempInvo.Discountpercent != null)
                {
                    Model.Discount = ((tempInvo.Discountpercent * Model.SubTotal) / 100).Value;
                }

                if (Model.Invoice.Deposit.HasValue && Model.Invoice.Deposit > 0)
                {
                    if (Model.Invoice.BalanceDue == Model.Invoice.Deposit || Model.Invoice.BalanceDue == 0)
                    {
                        Model.Invoice.Status = "Paid";
                        // Model.Transaction.Status = "Paid";
                    }
                    else
                    {
                        Model.Invoice.Status = "Partial";
                        // Model.Transaction.Status = "Partial"; 
                    }
                }

                Model.Invoice.RefType = tempInvo.RefType;
                Model.Invoice.BalanceDue = Math.Round(Model.Invoice.BalanceDue.HasValue ? Model.Invoice.BalanceDue.Value : 0, 2);
                Model.Invoice.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                Model.Invoice.LastUpdatedByUid = CurrentUser.UserId;

                if (string.IsNullOrWhiteSpace(Model.Invoice.CreatedBy) || Model.Invoice.CreatedByUid == new Guid())
                {
                    Model.Invoice.CreatedBy = tempInvo.CreatedBy;
                    Model.Invoice.CreatedByUid = tempInvo.CreatedByUid;
                }
                if (!string.IsNullOrWhiteSpace(tempInvo.EstimateTerm) && tempInvo.EstimateTerm != "-1" && tempInvo.EstimateTerm == "50UponAcceptance50UponCompletion")
                    Model.Invoice.EstimateTerm = tempInvo.EstimateTerm;
                _Util.Facade.InvoiceFacade.UpdateInvoice(Model.Invoice);
                bool ckInvoiceAlreadyIn = false;
                List<InvoiceDetail> CKInvoiceDetailList = new List<InvoiceDetail>();
                CKInvoiceDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(Model.Invoice.InvoiceId);
                if (CKInvoiceDetailList != null && CKInvoiceDetailList.Count > 0)
                {
                    ckInvoiceAlreadyIn = true;
                }
                #region log 
                if (ckInvoiceAlreadyIn && !SendEmail)
                {
                    if (Model.Invoice.Description == null)
                    {
                        Model.Invoice.Description = "";
                    }
                    if (Model.Invoice.Memo == null)
                    {
                        Model.Invoice.Memo = "";
                    }
                    if (Model.Invoice.Message == null)
                    {
                        Model.Invoice.Message = "";
                    }
                    if (Model.Invoice.InvoiceEmailAddress == null)
                    {
                        Model.Invoice.InvoiceEmailAddress = "";
                    }
                    if (Model.Invoice.BillingAddress == null)
                    {
                        Model.Invoice.BillingAddress = "";
                    }
                    if (!string.IsNullOrEmpty(Model.Invoice.Terms) && Model.Invoice.Terms != "-1")
                    {
                        newterms = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyandDataValueFromLLookup("InvoiceTerms", Model.Invoice.Terms);
                    }
                    if (!string.IsNullOrEmpty(tempInvo.Terms) && tempInvo.Terms != "-1")
                    {
                        oldterms = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyandDataValueFromLLookup("InvoiceTerms", tempInvo.Terms);
                    }
                    if (tempInvo.Status != Model.Invoice.Status)
                    {
                        statuslogmsg += "Status Changed from " + tempInvo.Status + " To " + Model.Invoice.Status + "</br>";
                    }

                    if (tempInvo.InvoiceEmailAddress != Model.Invoice.InvoiceEmailAddress)
                    {
                        statuslogmsg += "Email Changed from " + tempInvo.InvoiceEmailAddress + " To " + Model.Invoice.InvoiceEmailAddress + "</br>";
                    }
                    if (tempInvo.BillingAddress != Model.Invoice.BillingAddress)
                    {
                        statuslogmsg += "Billing Address Changed from " + tempInvo.BillingAddress + " To " + Model.Invoice.BillingAddress + "</br>";
                    }
                    if (oldterms != newterms)
                    {
                        statuslogmsg += "Terms Changed from " + oldterms + " To " + newterms + "</br>";
                    }
                    if (tempInvo.InvoiceDate != null && Model.Invoice.InvoiceDate != null)
                    {
                        if (tempInvo.InvoiceDate != Model.Invoice.InvoiceDate)
                        {
                            statuslogmsg += "Invoice Date Changed from " + tempInvo.InvoiceDate.Value.ToString("dd/MM/yyyy") + " To " + Model.Invoice.InvoiceDate.Value.ToString("dd/MM/yyyy") + "</br>";
                        }
                    }
                    if (tempInvo.DueDate != null && Model.Invoice.DueDate != null)
                    {
                        if (tempInvo.DueDate != Model.Invoice.DueDate)
                        {
                            statuslogmsg += "Due Date Changed from " + tempInvo.DueDate.Value.ToString("dd/MM/yyyy") + " To " + Model.Invoice.DueDate.Value.ToString("dd/MM/yyyy") + "</br>";
                        }
                    }

                    if (tempInvodetail != null && Model.InvoiceDetailList != null)
                    {
                        if (tempInvodetail.Count > Model.InvoiceDetailList.Count)
                        {
                            statuslogmsg += "With removing line item </br>";
                        }
                        if (tempInvodetail.Count < Model.InvoiceDetailList.Count)
                        {
                            statuslogmsg += "With adding line item </br>";
                        }
                    }

                    if (tempInvo.Description != Model.Invoice.Description)
                    {
                        statuslogmsg += "Invoice Description Changed from " + tempInvo.Description + " To " + Model.Invoice.Description + "</br>";
                    }
                    if (tempInvo.Memo != Model.Invoice.Memo)
                    {
                        statuslogmsg += "Invoice Memo Changed from " + tempInvo.Memo + " To " + Model.Invoice.Memo + "</br>";
                    }
                    if (tempInvo.Message != Model.Invoice.Message)
                    {
                        statuslogmsg += "Invoice Message Changed from " + tempInvo.Message + " To " + Model.Invoice.Message + "</br>";
                    }
                    base.AddUserActivityForCustomer("Invoice is updated #Ref:" + Model.Invoice.InvoiceId + "</br>" + statuslogmsg, LabelHelper.ActivityAction.UpdateInvoice, Model.Invoice.CustomerId, null, Model.Invoice.InvoiceId);
                }
                else if (!ckInvoiceAlreadyIn)
                {
                    base.AddUserActivityForCustomer("Invoice is added #Ref:" + Model.Invoice.InvoiceId, LabelHelper.ActivityAction.AddInvoice, Model.Invoice.CustomerId, null, Model.Invoice.InvoiceId);
                }
                #endregion
                _Util.Facade.InvoiceFacade.DeleteAllInvoiceDetailsByInvoiceId(Model.Invoice.InvoiceId);

                var CustomerUpdatedBy = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Invoice.CustomerId).LastUpdatedBy;
                var objInvoiceSnapshot = _Util.Facade.CustomerSnapshotFacade.GetCustomerSnapshotDetail(Model.Invoice.InvoiceId.ToString());
                if (objInvoiceSnapshot.Count == 0)
                {
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                    if (empobj != null)
                    {
                        CustomerSnapshot objCustomerSnapshot = new CustomerSnapshot
                        {
                            CustomerId = Model.Invoice.CustomerId,
                            CompanyId = CurrentUser.CompanyId.Value,
                            Description = "Invoice " + string.Format("<a onclick=OpenTopToBottomModal('{2}/Invoice/AddInvoice?id={0}&CustomerId={1}') style='cursor: pointer;'>", Model.Invoice.Id, tempCUstomer.Id, AppConfig.DomainSitePath) + "<b>" + Model.Invoice.InvoiceId + "</b>" + "</a>",
                            Logdate = DateTime.Now.UTCCurrentTime(),
                            Updatedby = empobj.FirstName + " " + empobj.LastName,
                            Type = "InvoiceCreated"
                        };
                        _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objCustomerSnapshot);
                    }
                }
                foreach (var item in Model.InvoiceDetailList)
                {
                    item.CreatedBy = User.Identity.Name;
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    item.CompanyId = CurrentUser.CompanyId.Value;
                    item.TotalPrice = Math.Round(item.TotalPrice.HasValue ? item.TotalPrice.Value : 0, 2);
                    item.UnitPrice = Math.Round(item.UnitPrice.HasValue ? item.UnitPrice.Value : 0, 2);
                    item.DiscountAmount = Math.Round(item.DiscountAmount.HasValue ? item.DiscountAmount.Value : 0, 2);
                    item.EquipmentFile = _Util.Facade.EquipmentFileFacade.GetEquipmentFilesByEquipmentId(item.EquipmentId).FirstOrDefault();
                    if (item.EquipmentFile == null)
                    {
                        item.EquipmentFile = new EquipmentFile();
                    }
                    _Util.Facade.InvoiceFacade.InsertInvoiceDetails(item);
                }


                if (tempInvo.DiscountType == "Percent")
                {
                    Model.Invoice.DiscountAmount = tempInvo.DiscountAmount;
                    Model.Invoice.Discountpercent = tempInvo.Discountpercent;
                }
                else if (tempInvo.DiscountType == "Amount")
                {
                    Model.Invoice.DiscountAmount = tempInvo.DiscountAmount;
                    Model.Invoice.Discountpercent = 0.00;
                }

                #endregion

                if (CreatePdf)
                {
                    List<CreateInvoice> createinvoicelist = new List<CreateInvoice>();
                    createinvoicelist.Add(Model);
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
                    GlobalSetting invComAddress = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ShowInvoiceCompanyAddress");
                    if (invComAddress != null)
                    {
                        Model.ShowInvoiceCompanyAddress = invComAddress.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        Model.ShowInvoiceCompanyAddress = false;
                    }
                    GlobalSetting invPayAddress = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ShowPaymentAddressForSendInvoice");
                    if (invPayAddress != null)
                    {
                        Model.ShowPaymentAddressForSendInvoice = invPayAddress.Value.ToLower() == "true" ? true : false;
                        if (Model.ShowPaymentAddressForSendInvoice)
                        {
                            Company _temCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
                            Model.PaymentAddress = !string.IsNullOrWhiteSpace(invPayAddress.OptionalValue) ? invPayAddress.OptionalValue : _temCom != null ? (!string.IsNullOrWhiteSpace(_temCom.Street) ? _temCom.Street + ", " : "") + _temCom.City + " " + _temCom.State + " " + _temCom.ZipCode : "";
                        }
                    }
                    else
                    {
                        Model.ShowPaymentAddressForSendInvoice = false;
                    }


                    return Json(new { result = true, emailSent = EmailSent, message = "Invoice Successfully Saved" });
                }
                else if (SendEmail)
                {
                    #region SendEmail


                    string filename = "";
                    if (id.HasValue && id > 0)
                    {
                        #region Get the file
                        if (Session[SessionKeys.InvoicePdfSession] != null)
                        {
                            List<InvoiceSessionModel> ModelList = (List<InvoiceSessionModel>)Session[SessionKeys.InvoicePdfSession];
                            filename = ModelList.Where(x => x.InvoiceId == Model.Invoice.InvoiceId).Select(x => x.FileName).FirstOrDefault();

                        }
                        if (string.IsNullOrWhiteSpace(filename))
                        {
                            List<CreateInvoice> createinvoicelist = new List<CreateInvoice>();
                            createinvoicelist.Add(Model);
                            filename = SaveInvoiceToPdf(createinvoicelist, new List<int>());
                        }
                        #endregion

                        var FileName = filename;
                        FileName = FileName.Split('/').Last();
                        var Filepath = filename;

                        string Full_Path = S3Domain + Filepath.TrimStart('/');


                        /// Mayur :: File Download to temp folder :start

                        webClient = new WebClient();
                        FileDataInBytes = webClient.DownloadData(Full_Path);

                        File(FileDataInBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName).ToString();

                        decimal _fileSize = (decimal)FileDataInBytes.Length / 1024;
                        _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);


                        var Temp_FileName = Server.MapPath("~/EmailFileCache/inv_" + FileName);

                        if (!System.IO.File.Exists(Temp_FileName))
                        {
                            System.IO.File.WriteAllBytes(Temp_FileName, FileDataInBytes);
                        }
                        else
                        {
                            System.IO.File.WriteAllBytes(Temp_FileName, FileDataInBytes);
                        }



                        //var cusobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Invoice.CustomerId);
                        var comobj = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
                        InvoiceCreatedEmail email = new InvoiceCreatedEmail()
                        {
                            CompanyName = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName,
                            CustomerName = Model.Invoice.CustomerName,
                            BalanceDue = Model.Invoice.TotalAmount != null ? LabelHelper.CurrentTransMakeCurrency.MakeCurrency() + Model.Invoice.TotalAmount.Value.ToString("0,0.00") : "0.00",
                            DueDate = Model.Invoice.DueDate.HasValue ? Model.Invoice.DueDate.Value.ToString("MM/dd/yy") : "",
                            InvoiceId = Model.Invoice.InvoiceId,
                            ToEmail = Model.EmailAddress,
                            EmailBody = Model.EmailDescription,
                            ccEmail = Model.Invoice.ccEmail,
                            Subject = Model.EmailSubject,
                            FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : "info@rmrcloud.com",
                            FromName = string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName),
                            InvoicePdf = new Attachment(
                                  Temp_FileName,
                                 MediaTypeNames.Application.Octet)
                        };
                        EmailSent = _Util.Facade.MailFacade.SendInvoiceCreatedEmail(email, CurrentUser.CompanyId.Value);
                        email.InvoicePdf.Dispose();
                        tempCUstomer.PreferedEmail = true;
                        _Util.Facade.CustomerFacade.UpdateCustomer(tempCUstomer);

                        if (EmailSent)
                        {
                            #region Log for Invoice Send
                            base.AddUserActivityForCustomer("Invoice is sent #Ref:" + Model.Invoice.InvoiceId, LabelHelper.ActivityAction.SendInvoice, Model.Invoice.CustomerId, null, Model.Invoice.InvoiceId);
                            #endregion
                            #region Customer File
                            string InvoiceNo = Model.Invoice.Id.GenerateInvoiceNo();
                            bool res = false;
                            var objCustomerFile = _Util.Facade.CustomerFileFacade.GetCustomerFileByDescriptionAndCustomerId(InvoiceNo, tempCUstomer.CustomerId);
                            {
                                if (objCustomerFile != null)
                                {
                                    res = _Util.Facade.CustomerFileFacade.DeleteEstimateFile(objCustomerFile.Id);
                                    if (res == true)
                                    {
                                        CustomerFile cuf = new CustomerFile()
                                        {
                                            CompanyId = CurrentUser.CompanyId.Value,
                                            FileId = Guid.NewGuid(),
                                            FileSize= (double)_fileSize,
                                            CustomerId = tempCUstomer.CustomerId,
                                            FileDescription = tempCUstomer.Id + "_" + InvoiceNo + ".pdf",
                                            FileFullName = InvoiceNo + ".pdf",
                                            Filename = "/" + filename,
                                            IsActive = true,
                                            Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                            CreatedBy = CurrentUser.UserId,
                                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                                            UpdatedBy = CurrentUser.UserId,
                                            UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                            WMStatus = LabelHelper.WatermarkStatus.Pending,
                                            AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                                        };
                                        _Util.Facade.CustomerFileFacade.InsertCustomerFile(cuf);
                                    }
                                }
                                else
                                {
                                    CustomerFile cuf = new CustomerFile()
                                    {
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        FileId = Guid.NewGuid(),
                                        FileSize = (double)_fileSize,
                                        CustomerId = tempCUstomer.CustomerId,
                                        FileDescription = tempCUstomer.Id + "_" + InvoiceNo + ".pdf",
                                        FileFullName = InvoiceNo + ".pdf",
                                        Filename = "/" + filename,
                                        IsActive = true,
                                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                        CreatedBy = CurrentUser.UserId,
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        UpdatedBy = CurrentUser.UserId,
                                        UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                        WMStatus = LabelHelper.WatermarkStatus.Pending,
                                        AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                                    };
                                    _Util.Facade.CustomerFileFacade.InsertCustomerFile(cuf);
                                }
                            }
                            #endregion

                            string empName = "";
                            var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(tempInvo.CreatedByUid);
                            if (empobj != null)
                            {
                                empName = empobj.FirstName + " " + empobj.LastName;
                            }
                            CustomerSnapshot objEstimateEmailLog = new CustomerSnapshot()
                            {
                                CustomerId = Model.Invoice.CustomerId,
                                CompanyId = CurrentUser.CompanyId.Value,
                                Description = "Invoice:" + "  " + Model.Invoice.InvoiceId + " " + "email sent by " + "<b>" + empName + "</b>",
                                Logdate = DateTime.Now.UTCCurrentTime(),
                                Updatedby = CurrentUser.Identity.Name,
                                Type = "CustomerMailHistory"
                            };
                            _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEstimateEmailLog);
                            CustomerAgreement objagree = new CustomerAgreement()
                            {
                                CustomerId = Model.Invoice.CustomerId,
                                CompanyId = CurrentUser.CompanyId.Value,
                                InvoiceId = Model.Invoice.InvoiceId,
                                Type = LabelHelper.EstimateStatus.SentToCustomer,
                                AddedDate = DateTime.Now.UTCCurrentTime()
                            };
                            _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objagree);


                        }
                    }
                    else
                    {

                        if (Session[SessionKeys.InvoicePdfSession] != null)
                        {
                            List<InvoiceSessionModel> ModelList = (List<InvoiceSessionModel>)Session[SessionKeys.InvoicePdfSession];
                            filename = ModelList.Where(x => x.InvoiceId == Model.Invoice.InvoiceId).Select(x => x.FileName).FirstOrDefault();

                        }
                        if (string.IsNullOrWhiteSpace(filename))
                        {
                            //Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
                            //Model.CompanyAddress = MakeAddress(tempCom.Street, tempCom.City, tempCom.State, tempCom.ZipCode, "");
                            //Model.CompanyEmail = tempCom.EmailAdress;
                            //Model.CompanyName = tempCom.CompanyName;
                            List<CreateInvoice> createinvoicelist = new List<CreateInvoice>();
                            createinvoicelist.Add(Model);
                            filename = SaveInvoiceToPdf(createinvoicelist, new List<int>());
                        }
                        try
                        {
                            //Model.EmailAddress
                            string FromEmail = CurrentUser.EmailAddress;

                            if (Model.EmailDescription.IndexOf("##url##") > -1)
                            {
                                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(Model.Invoice.Id
                                    + "#"
                                    + CurrentUser.CompanyId.Value
                                    + "#"
                                    + tempCUstomer.CustomerId);
                                string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Invoice/", encryptedurl);
                                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, tempCUstomer.CustomerId);
                                Model.EmailDescription = Model.EmailDescription.Replace("##url##", ShortUrl.Code);
                            }

                            InvoiceCreatedEmail email = new InvoiceCreatedEmail()
                            {
                                CompanyName = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName,
                                CustomerName = Model.Invoice.CustomerName,
                                BalanceDue = Model.Invoice.TotalAmount != null ? "$" + Model.Invoice.TotalAmount.Value.ToString("0,0.00") : HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency(),
                                DueDate = Model.Invoice.DueDate.HasValue ? Model.Invoice.DueDate.Value.ToString("MM/dd/yy") : "",
                                InvoiceId = Model.Invoice.InvoiceId,
                                ToEmail = Model.EmailAddress.Trim(),
                                EmailBody = Model.EmailDescription,
                                Subject = Model.EmailSubject,
                                CustomerId = Model.Invoice.CustomerId.ToString(),
                                EmployeeId = CurrentUser.UserId.ToString(),
                                FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : "info@rmrcloud.com",
                                FromName = CurrentUser.GetFullName(),
                                ccEmail = Model.Invoice.ccEmail,
                                InvoicePdf = new Attachment(
                                  FileHelper.GetFileFullPath(filename),
                                 MediaTypeNames.Application.Octet)
                            };
                            EmailSent = _Util.Facade.MailFacade.SendInvoiceCreatedEmail(email, CurrentUser.CompanyId.Value);
                            if (email.InvoicePdf != null)
                            {
                                email.InvoicePdf.Dispose();
                            }


                            if (EmailSent)
                            {
                                #region Log for Invoice Send
                                base.AddUserActivityForCustomer("Invoice is sent #Ref:" + Model.Invoice.InvoiceId, LabelHelper.ActivityAction.SendInvoice, Model.Invoice.CustomerId, null, Model.Invoice.InvoiceId);
                                #endregion
                                #region Customer File
                                string InvoiceNo = Model.Invoice.Id.GenerateInvoiceNo();

                                bool res = false;
                                var objCustomerFile = _Util.Facade.CustomerFileFacade.GetCustomerFileByDescriptionAndCustomerId(InvoiceNo, tempCUstomer.CustomerId);
                                {
                                    if (objCustomerFile != null)
                                    {
                                        res = _Util.Facade.CustomerFileFacade.DeleteEstimateFile(objCustomerFile.Id);
                                        if (res == true)
                                        {
                                            CustomerFile cuf = new CustomerFile()
                                            {
                                                CompanyId = CurrentUser.CompanyId.Value,
                                                FileId = Guid.NewGuid(),
                                                CustomerId = tempCUstomer.CustomerId,
                                                FileDescription = tempCUstomer.Id + "_" + InvoiceNo,
                                                FileFullName = InvoiceNo + ".pdf",
                                                Filename = filename,
                                                IsActive = true,
                                                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                                CreatedBy = CurrentUser.UserId,
                                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                                UpdatedBy = CurrentUser.UserId,
                                                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                                WMStatus = LabelHelper.WatermarkStatus.Pending,
                                                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                                            };
                                            _Util.Facade.CustomerFileFacade.InsertCustomerFile(cuf);
                                        }
                                    }
                                    else
                                    {
                                        CustomerFile cuf = new CustomerFile()
                                        {
                                            CompanyId = CurrentUser.CompanyId.Value,
                                            FileId = Guid.NewGuid(),
                                            CustomerId = tempCUstomer.CustomerId,
                                            FileDescription = tempCUstomer.Id + "_" + InvoiceNo,
                                            FileFullName = InvoiceNo + ".pdf",
                                            Filename = "/" + filename,
                                            IsActive = true,
                                            Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                            CreatedBy = CurrentUser.UserId,
                                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                                            UpdatedBy = CurrentUser.UserId,
                                            UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                            WMStatus = LabelHelper.WatermarkStatus.Pending,
                                            AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                                        };
                                        _Util.Facade.CustomerFileFacade.InsertCustomerFile(cuf);
                                    }
                                }
                                #endregion
                                string empName = "";
                                var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(tempInvo.CreatedByUid);
                                if (empobj != null)
                                {
                                    empName = empobj.FirstName + " " + empobj.LastName;
                                }
                                CustomerSnapshot objEstimateEmailLog = new CustomerSnapshot()
                                {
                                    CustomerId = Model.Invoice.CustomerId,
                                    CompanyId = CurrentUser.CompanyId.Value,

                                    Description = "Invoice:" + "  " + Model.Invoice.InvoiceId + " " + "email sent by " + "<b>" + empName + "</b>",
                                    Logdate = DateTime.Now.UTCCurrentTime(),
                                    Updatedby = CurrentUser.Identity.Name,
                                    Type = "CustomerMailHistory"
                                };
                                _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEstimateEmailLog);
                                CustomerAgreement objagree = new CustomerAgreement()
                                {
                                    CustomerId = Model.Invoice.CustomerId,
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    InvoiceId = Model.Invoice.InvoiceId,
                                    Type = LabelHelper.EstimateStatus.SentToCustomer,
                                    AddedDate = DateTime.Now.UTCCurrentTime()
                                };
                                _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objagree);

                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex);
                        }
                    }
                    string errorMessage = "Failed send email";
                    if (EmailSent == false)
                    {
                        if (string.IsNullOrWhiteSpace(Model.EmailAddress))
                        {
                            errorMessage = "Eamil cannot be send without email address";
                        }
                        return Json(new { result = true, message = errorMessage, EmailSent = EmailSent });
                    }
                    //if (EmailSent)
                    //{
                    //    string serverFile = Server.MapPath(filename);
                    //    if (System.IO.File.Exists(serverFile))
                    //    {
                    //        System.IO.File.Delete(serverFile);
                    //    }
                    //}
                    #endregion
                }

            }
            return Json(new { result = true, message = string.Concat("Invoice Saved Successfully and Emailed to ", Model.EmailAddress), EmailSent = EmailSent });
        }

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public JsonResult AddInvoice(CreateInvoice Model, bool SendEmail, bool CreatePdf, int? id, string ccEmail)
        {
            WebClient webClient;
            byte[] FileDataInBytes;
            Invoice oldInvoice=null; // = new Invoice();
            List<InvoiceDetail> tempInvodetail=null;
            Customer invCustomer=null;
            bool responseResult = false;
            string responseMessage = string.Empty;
            bool responseEmailSent = false;
            int _invoiceid=0;

            try
            {

                if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerInvoiceAdd))
                {
                    return Json(new { result = false, message = "Permission denied." });
                }

                
                bool NewTransaction = false;
                string statuslogmsg = "";
                string newterms = "";
                string oldterms = "";
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                if (Model.Invoice.Id > 0)
                {
                    _invoiceid = Model.Invoice.Id;
                }
                else if (id != null)
                {
                    _invoiceid = id.Value;
                }


                if (_invoiceid > 0)
                {
                    //  oldInvoice = _Util.Facade.InvoiceFacade.GetInvoiceById(Model.Invoice.Id);
                    //  oldInvoice.InvoiceListDetail = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(Model.Invoice.InvoiceId);

                    //  invCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Invoice.CustomerId);

                    oldInvoice = _Util.Facade.InvoiceFacade.GetInvoiceById(_invoiceid);

                    oldInvoice.InvoiceListDetail = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(_invoiceid.ToString());



                    if (Model.Invoice.CustomerId == null || Model.Invoice.CustomerId.ToString() == "00000000-0000-0000-0000-000000000000")
                    {

                        invCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(oldInvoice.CustomerId);
                    }
                    else
                    {
                        invCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Invoice.CustomerId);

                    }


                    //tempInvodetail = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(Model.Invoice.InvoiceId.ToString());
                    if (oldInvoice == null || oldInvoice.CompanyId != CurrentUser.CompanyId.Value)
                    {
                        return Json(new { result = false });
                    }
                    if (Model.InvoiceDetailList == null)
                    {
                        return Json(new { result = false, message = "Customer Equipment Found" });
                    }

                    if (oldInvoice.Status == "Paid")
                    {
                        Model.Invoice.Id = oldInvoice.Id;
                        Model.Invoice.IsEstimate = false;
                        Model.Invoice.CustomerName = invCustomer.Title + " " + invCustomer.FirstName + " " + invCustomer.LastName;
                        Model.CustomerName = Model.Invoice.CustomerName;
                        Model.Invoice.CreatedBy = oldInvoice.CreatedBy;
                        Model.Invoice.CreatedDate = oldInvoice.CreatedDate;
                        Model.Invoice.LastUpdatedDate = oldInvoice.LastUpdatedDate;
                        Model.Invoice.CreatedByUid = oldInvoice.CreatedByUid;
                        Model.Invoice.BillingCycle = oldInvoice.BillingCycle;
                        Model.Invoice.LastUpdatedByUid = oldInvoice.LastUpdatedByUid;
                        Model.JobNo = oldInvoice.JobNo;

                        if (string.IsNullOrWhiteSpace(ccEmail))
                        {
                            Model.Invoice.InvoiceCcEmailAddress = oldInvoice.InvoiceCcEmailAddress;
                            Model.Invoice.ccEmail = ccEmail;
                        }


                        Model.Invoice.InvoiceDate = oldInvoice.InvoiceDate.Value.SetZeroHour();
                        Model.Invoice.DueDate = oldInvoice.DueDate.Value.SetMaxHour();
                        Model.Invoice.ShippingDate = oldInvoice.ShippingDate?.SetMaxHour();

                        Model.Invoice.CompanyId = CurrentUser.CompanyId.Value;
                        Model.Invoice.Amount = Math.Round(oldInvoice.Amount, 2);
                        Model.Invoice.TotalAmount = Math.Round(oldInvoice.TotalAmount.HasValue ? oldInvoice.TotalAmount.Value : 0, 2);
                        Model.Invoice.BalanceDue = Math.Round(oldInvoice.BalanceDue.HasValue ? oldInvoice.BalanceDue.Value : 0, 2);
                        Model.Invoice.DiscountType = oldInvoice.DiscountType;

                        //TODO: Check this part of the discount not clear
                        if (oldInvoice.DiscountType == "Percent")
                        {
                            Model.Invoice.DiscountAmount = oldInvoice.DiscountAmount;
                            Model.Invoice.Discountpercent = oldInvoice.Discountpercent;
                        }
                        else if (oldInvoice.DiscountType == "amount")
                        {
                            Model.Invoice.DiscountAmount = oldInvoice.DiscountAmount;
                            Model.Invoice.Discountpercent = 0.00;
                        }

                        //Model.Invoice.Status = "Open";
                        if (Model.Invoice.BalanceDue == 0)
                        {
                            Model.Invoice.Status = LabelHelper.InvoiceStatus.Paid;
                        }
                        else if (Model.Invoice.BalanceDue < Model.Invoice.TotalAmount)
                        {
                            Model.Invoice.Status = LabelHelper.InvoiceStatus.Partial;
                        }
                        else if (Model.Invoice.BalanceDue < Model.Invoice.TotalAmount)
                        {
                            Model.Invoice.Status = LabelHelper.InvoiceStatus.Excess;
                        }
                        else if (Model.Invoice.BalanceDue == Model.Invoice.TotalAmount)
                        {
                            Model.Invoice.Status = LabelHelper.InvoiceStatus.Open;
                        }
                        else
                        {
                            Model.Invoice.Status = LabelHelper.InvoiceStatus.Verify;
                        }
                    }
                    else
                    {

                        Model.Invoice.Id = oldInvoice.Id;
                        Model.Invoice.IsEstimate = false;
                        Model.Invoice.CustomerName = invCustomer.Title + " " + invCustomer.FirstName + " " + invCustomer.LastName;
                        Model.CustomerName = Model.Invoice.CustomerName;
                        Model.Invoice.CreatedBy = oldInvoice.CreatedBy;
                        Model.Invoice.CreatedDate = oldInvoice.CreatedDate;
                        Model.Invoice.LastUpdatedDate = oldInvoice.LastUpdatedDate;
                        Model.Invoice.CreatedByUid = oldInvoice.CreatedByUid;
                        Model.Invoice.BillingCycle = oldInvoice.BillingCycle;
                        Model.Invoice.LastUpdatedByUid = oldInvoice.LastUpdatedByUid;
                        Model.JobNo = oldInvoice.JobNo;
                        if (string.IsNullOrWhiteSpace(ccEmail))
                        {
                            Model.Invoice.InvoiceCcEmailAddress = oldInvoice.InvoiceCcEmailAddress;
                            Model.Invoice.ccEmail = ccEmail;
                        }


                        Model.Invoice.InvoiceDate = Model.Invoice.InvoiceDate.Value.SetZeroHour();
                        Model.Invoice.DueDate = Model.Invoice.DueDate.Value.SetMaxHour();
                        Model.Invoice.ShippingDate = Model.Invoice.ShippingDate?.SetMaxHour();

                        Model.Invoice.CompanyId = CurrentUser.CompanyId.Value;
                        Model.Invoice.Amount = Math.Round(Model.Invoice.Amount, 2);
                        Model.Invoice.TotalAmount = Math.Round(Model.Invoice.TotalAmount.HasValue ? Model.Invoice.TotalAmount.Value : 0, 2);
                        Model.Invoice.BalanceDue = Math.Round(Model.Invoice.BalanceDue.HasValue ? Model.Invoice.BalanceDue.Value : 0, 2);
                        Model.Invoice.DiscountType = oldInvoice.DiscountType;

                        //TODO: Check this part of the discount not clear
                        if (oldInvoice.DiscountType == "Percent")
                        {
                            Model.Invoice.DiscountAmount = oldInvoice.DiscountAmount;
                            Model.Invoice.Discountpercent = oldInvoice.Discountpercent;
                        }
                        else if (oldInvoice.DiscountType == "Amount")
                        {
                            Model.Invoice.DiscountAmount = oldInvoice.DiscountAmount;
                            Model.Invoice.Discountpercent = 0.00;
                        }

                        //Model.Invoice.Status = "Open";
                        if (Model.Invoice.BalanceDue == 0)
                        {
                            Model.Invoice.Status = LabelHelper.InvoiceStatus.Paid;
                        }
                        else if (Model.Invoice.BalanceDue < Model.Invoice.TotalAmount)
                        {
                            Model.Invoice.Status = LabelHelper.InvoiceStatus.Partial;
                        }
                        else if (Model.Invoice.BalanceDue < Model.Invoice.TotalAmount)
                        {
                            Model.Invoice.Status = LabelHelper.InvoiceStatus.Excess;
                        }
                        else if (Model.Invoice.BalanceDue == Model.Invoice.TotalAmount)
                        {
                            Model.Invoice.Status = LabelHelper.InvoiceStatus.Open;
                        }
                        else
                        {
                            Model.Invoice.Status = LabelHelper.InvoiceStatus.Verify;
                        }
                    }
                }

                if (invCustomer == null)
                {
                 
                    return Json(new { result = false, message = "Customer Not Found" });

                }

                if (Model.Invoice.InvoiceDate.HasValue && Model.Invoice.DueDate.HasValue)
                {
                    if (Model.Invoice.DueDate.Value < Model.Invoice.InvoiceDate.Value)
                    {
                        return Json(new { result = false, message = "Due date should be greater than or equal to invoice date." });
                    }
                }

                if (Model.Invoice != null && Model.InvoiceDetailList.Count > 0)
                    {
                        #region if Model.Invoice != null
                        //= _Util.Facade.InvoiceFacade.GetByInvoiceId(Model.Invoice.InvoiceId);
                        //Model.Invoice.CustomerId = tempInvo.CustomerId;

                        Model.Invoice.Tax = Math.Round(Model.Invoice.Tax.HasValue ? Model.Invoice.Tax.Value : 0, 2);
                        if (Model.Invoice.Tax == 0) responseMessage += $" | Tax amount {oldInvoice.Tax} - {Model.Invoice.Tax}";
                        logger.WithProperty("tags", "invoices,taxation")
                            .WithProperty("params", JsonConvert.SerializeObject(Model.Invoice))
                            .Trace($"Tax before update {oldInvoice.Tax} for #{Model.Invoice.Id} user {CurrentUser.GetFullName()}");
                        Model.Invoice.Deposit = Math.Round(Model.Invoice.Deposit.HasValue ? Model.Invoice.Deposit.Value : 0, 2);
                        Model.Invoice.ShippingCost = Math.Round(Model.Invoice.ShippingCost.HasValue ? Model.Invoice.ShippingCost.Value : 0, 2);

                        Model.SubTotal = Model.InvoiceDetailList.Sum(x => x.TotalPrice.Value);

                        foreach (var item in Model.InvoiceDetailList)
                        {
                            //if Invoice is already created then no need to update the CreatedBy & Date part
                            //item.CreatedBy = User.Identity.Name;
                            //item.CreatedDate = DateTime.Now.UTCCurrentTime();
                            //item.CompanyId = CurrentUser.CompanyId.Value;

                            //if (item.TotalPrice.HasValue)
                            //{
                            //    Model.SubTotal = Model.SubTotal + item.TotalPrice.Value;

                            //}
                            //else
                            //{
                            //    Model.SubTotal = Model.SubTotal;
                            //}

                            //Commented below as the call is repeated below
                            //item.EquipmentFile = _Util.Facade.EquipmentFileFacade.GetEquipmentFilesByEquipmentId(item.EquipmentId).FirstOrDefault();
                            //if (item.EquipmentFile == null)
                            //{
                            //    item.EquipmentFile = new EquipmentFile();
                            //}
                        }
                        if (oldInvoice.Discountpercent != null)
                        {
                            Model.Discount = ((oldInvoice.Discountpercent * Model.SubTotal) / 100).Value;
                        }

                        if (Model.Invoice.Deposit.HasValue && Model.Invoice.Deposit > 0)
                        {
                            if (Model.Invoice.BalanceDue == Model.Invoice.Deposit || Model.Invoice.BalanceDue == 0)
                            {
                                Model.Invoice.Status = "Paid";
                                // Model.Transaction.Status = "Paid";
                            }
                            else
                            {
                                Model.Invoice.Status = "Partial";
                                // Model.Transaction.Status = "Partial"; 
                            }
                        }

                        Model.Invoice.RefType = oldInvoice.RefType;
                        Model.Invoice.BalanceDue = Math.Round(Model.Invoice.BalanceDue.HasValue ? Model.Invoice.BalanceDue.Value : 0, 2);
                        Model.Invoice.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        Model.Invoice.LastUpdatedByUid = CurrentUser.UserId;

                        if (string.IsNullOrWhiteSpace(Model.Invoice.CreatedBy) || Model.Invoice.CreatedByUid == new Guid())
                        {
                            Model.Invoice.CreatedBy = oldInvoice.CreatedBy;
                            Model.Invoice.CreatedByUid = oldInvoice.CreatedByUid;
                        }
                        if (!string.IsNullOrWhiteSpace(oldInvoice.EstimateTerm) && oldInvoice.EstimateTerm != "-1" && oldInvoice.EstimateTerm == "50UponAcceptance50UponCompletion")
                            Model.Invoice.EstimateTerm = oldInvoice.EstimateTerm;
                        _Util.Facade.InvoiceFacade.UpdateInvoice(Model.Invoice);
                        bool ckInvoiceAlreadyIn = false;
                        //List<InvoiceDetail> CKInvoiceDetailList = new List<InvoiceDetail>();
                        //CKInvoiceDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(Model.Invoice.InvoiceId);
                        //if (CKInvoiceDetailList != null && CKInvoiceDetailList.Count > 0)
                        if (Model.InvoiceDetailList != null && oldInvoice.InvoiceListDetail.Count > 0)
                        {
                            ckInvoiceAlreadyIn = true;
                        }
                        #region log 
                        if (ckInvoiceAlreadyIn && !SendEmail)
                        {
                            //    if (Model.Invoice.Description == null)
                            //    {
                            //        Model.Invoice.Description = "";
                            //    }
                            //    if (Model.Invoice.Memo == null)
                            //    {
                            //        Model.Invoice.Memo = "";
                            //    }
                            //    if (Model.Invoice.Message == null)
                            //    {
                            //        Model.Invoice.Message = "";
                            //    }
                            //    if (Model.Invoice.InvoiceEmailAddress == null)
                            //    {
                            //        Model.Invoice.InvoiceEmailAddress = "";
                            //    }
                            //    if (Model.Invoice.BillingAddress == null)
                            //    {
                            //        Model.Invoice.BillingAddress = "";
                            //    }
                            if (!string.IsNullOrEmpty(Model.Invoice.Terms) && Model.Invoice.Terms != "-1")
                            {
                                newterms = TermListCache.Where(x => x.Value == Model.Invoice.Terms).FirstOrDefault().Text;
                                //_Util.Facade.LookupFacade.GetDisplayTextByDataKeyandDataValueFromLLookup("InvoiceTerms", Model.Invoice.Terms);
                            }
                            if (!string.IsNullOrEmpty(oldInvoice.Terms) && oldInvoice.Terms != "-1")
                            {
                                oldterms = TermListCache.Where(x => x.Value == oldInvoice.Terms).FirstOrDefault().Text;
                                //oldterms = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyandDataValueFromLLookup("InvoiceTerms", oldInvoice.Terms);
                            }
                            if (oldInvoice.Status != Model.Invoice.Status)
                            {
                                statuslogmsg += "Status Changed from " + oldInvoice.Status + " To " + Model.Invoice.Status + "</br>";
                            }

                            if (oldInvoice.InvoiceEmailAddress != Model.Invoice.InvoiceEmailAddress)
                            {
                                statuslogmsg += "Email Changed from " + oldInvoice.InvoiceEmailAddress + " To " + Model.Invoice.InvoiceEmailAddress + "</br>";
                            }
                            if (oldInvoice.BillingAddress != Model.Invoice.BillingAddress)
                            {
                                statuslogmsg += "Billing Address Changed from " + oldInvoice.BillingAddress + " To " + Model.Invoice.BillingAddress + "</br>";
                            }
                            if (oldterms != newterms)
                            {
                                statuslogmsg += "Terms Changed from " + oldterms + " To " + newterms + "</br>";
                            }
                            if (oldInvoice.InvoiceDate != null && Model.Invoice.InvoiceDate != null)
                            {
                                if (oldInvoice.InvoiceDate != Model.Invoice.InvoiceDate)
                                {
                                    statuslogmsg += "Invoice Date Changed from " + oldInvoice.InvoiceDate.Value.ToString("dd/MM/yyyy") + " To " + Model.Invoice.InvoiceDate.Value.ToString("dd/MM/yyyy") + "</br>";
                                }
                            }
                            if (oldInvoice.DueDate != null && Model.Invoice.DueDate != null)
                            {
                                if (oldInvoice.DueDate != Model.Invoice.DueDate)
                                {
                                    statuslogmsg += "Due Date Changed from " + oldInvoice.DueDate.Value.ToString("dd/MM/yyyy") + " To " + Model.Invoice.DueDate.Value.ToString("dd/MM/yyyy") + "</br>";
                                }
                            }

                            if (tempInvodetail != null && Model.InvoiceDetailList != null)
                            {
                                if (tempInvodetail.Count > Model.InvoiceDetailList.Count)
                                {
                                    statuslogmsg += "With removing line item </br>";
                                }
                                if (tempInvodetail.Count < Model.InvoiceDetailList.Count)
                                {
                                    statuslogmsg += "With adding line item </br>";
                                }
                            }

                            if (oldInvoice.Description != Model.Invoice.Description)
                            {
                                statuslogmsg += "Invoice Description Changed from " + oldInvoice.Description + " To " + Model.Invoice.Description + "</br>";
                            }
                            if (oldInvoice.Memo != Model.Invoice.Memo)
                            {
                                statuslogmsg += "Invoice Memo Changed from " + oldInvoice.Memo + " To " + Model.Invoice.Memo + "</br>";
                            }
                            if (oldInvoice.Message != Model.Invoice.Message)
                            {
                                statuslogmsg += "Invoice Message Changed from " + oldInvoice.Message + " To " + Model.Invoice.Message + "</br>";
                            }
                            base.AddUserActivityForCustomer("Invoice is updated #Ref:" + Model.Invoice.InvoiceId + "</br>" + statuslogmsg, LabelHelper.ActivityAction.UpdateInvoice, Model.Invoice.CustomerId, null, Model.Invoice.InvoiceId);
                        }
                        else if (!ckInvoiceAlreadyIn)
                        {
                            base.AddUserActivityForCustomer("Invoice is added #Ref:" + Model.Invoice.InvoiceId, LabelHelper.ActivityAction.AddInvoice, Model.Invoice.CustomerId, null, Model.Invoice.InvoiceId);
                        }
                        #endregion
                        _Util.Facade.InvoiceFacade.DeleteAllInvoiceDetailsByInvoiceId(Model.Invoice.InvoiceId);

                        //Commented as not used elsewhere
                        //var CustomerUpdatedBy = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Invoice.CustomerId).LastUpdatedBy;

                        //var objInvoiceSnapshot = _Util.Facade.CustomerSnapshotFacade.GetCustomerSnapshotDetail(Model.Invoice.InvoiceId.ToString());
                        //if (objInvoiceSnapshot.Count == 0)
                        //{
                        //    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                        //    if (empobj != null)
                        //    {
                        //        CustomerSnapshot objCustomerSnapshot = new CustomerSnapshot
                        //        {
                        //            CustomerId = Model.Invoice.CustomerId,
                        //            CompanyId = CurrentUser.CompanyId.Value,
                        //            Description = "Invoice " + string.Format("<a onclick=OpenTopToBottomModal('{2}/Invoice/AddInvoice?id={0}&CustomerId={1}') style='cursor: pointer;'>", Model.Invoice.Id, invCustomer.Id, AppConfig.DomainSitePath) + "<b>" + Model.Invoice.InvoiceId + "</b>" + "</a>",
                        //            Logdate = DateTime.Now.UTCCurrentTime(),
                        //          Updatedby = empobj.FirstName + " " + empobj.LastName,
                        //            Type = "InvoiceCreated"
                        //        };
                        //        _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objCustomerSnapshot);
                        //    }
                        //}

                        CustomerSnapshot objCustomerSnapshot = new CustomerSnapshot
                        {
                            CustomerId = Model.Invoice.CustomerId,
                            CompanyId = CurrentUser.CompanyId.Value,
                            Description = "Invoice " + string.Format("<a onclick=OpenTopToBottomModal('{2}/Invoice/AddInvoice?id={0}&CustomerId={1}') style='cursor: pointer;'>", Model.Invoice.Id, invCustomer.Id, AppConfig.DomainSitePath) + "<b>" + Model.Invoice.InvoiceId + "</b>" + "</a>",
                            Logdate = DateTime.Now,
                            Updatedby = $"{CurrentUser.FirstName} {CurrentUser.LastName}",
                            Type = "InvoiceCreated"
                        };
                        _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objCustomerSnapshot);

                        foreach (var item in Model.InvoiceDetailList)
                        {
                            item.CreatedBy = User.Identity.Name;
                            item.CreatedDate = DateTime.Now.UTCCurrentTime();
                            item.CompanyId = CurrentUser.CompanyId.Value;
                            item.TotalPrice = Math.Round(item.TotalPrice.HasValue ? item.TotalPrice.Value : 0, 2);
                            item.UnitPrice = Math.Round(item.UnitPrice.HasValue ? item.UnitPrice.Value : 0, 2);
                            item.DiscountAmount = Math.Round(item.DiscountAmount.HasValue ? item.DiscountAmount.Value : 0, 2);

                            //Commented as it has no utility
                            //item.EquipmentFile = _Util.Facade.EquipmentFileFacade.GetEquipmentFilesByEquipmentId(item.EquipmentId).FirstOrDefault();
                            //if (item.EquipmentFile == null)
                            //{
                            //    item.EquipmentFile = new EquipmentFile();
                            //}
                            item.EquipmentFile = new EquipmentFile();

                            _Util.Facade.InvoiceFacade.InsertInvoiceDetails(item);
                        }


                        if (oldInvoice.DiscountType == "Percent")
                        {
                            Model.Invoice.DiscountAmount = oldInvoice.DiscountAmount;
                            Model.Invoice.Discountpercent = oldInvoice.Discountpercent;
                        }
                        else if (oldInvoice.DiscountType == "Amount")
                        {
                            Model.Invoice.DiscountAmount = oldInvoice.DiscountAmount;
                            Model.Invoice.Discountpercent = 0.00;
                        }

                        #endregion

                        if (CreatePdf)
                        {
                            List<CreateInvoice> createinvoicelist = new List<CreateInvoice>();
                            createinvoicelist.Add(Model);
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
                            GlobalSetting invComAddress = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ShowInvoiceCompanyAddress");
                            if (invComAddress != null)
                            {
                                Model.ShowInvoiceCompanyAddress = invComAddress.Value.ToLower() == "true" ? true : false;
                            }
                            else
                            {
                                Model.ShowInvoiceCompanyAddress = false;
                            }
                            GlobalSetting invPayAddress = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ShowPaymentAddressForSendInvoice");
                            if (invPayAddress != null)
                            {
                                Model.ShowPaymentAddressForSendInvoice = invPayAddress.Value.ToLower() == "true" ? true : false;
                                if (Model.ShowPaymentAddressForSendInvoice)
                                {
                                    Company _temCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
                                    Model.PaymentAddress = !string.IsNullOrWhiteSpace(invPayAddress.OptionalValue) ? invPayAddress.OptionalValue : _temCom != null ? (!string.IsNullOrWhiteSpace(_temCom.Street) ? _temCom.Street + ", " : "") + _temCom.City + " " + _temCom.State + " " + _temCom.ZipCode : "";
                                }
                            }
                            else
                            {
                                Model.ShowPaymentAddressForSendInvoice = false;
                            }


                            return Json(new { result = true, emailSent = responseEmailSent, message = "Invoice Successfully Saved" });
                        }

                        else if (SendEmail)
                        {
                            #region SendEmail
                            string fullurl;
                            var Temp_FileName1 = "";

                            string filename = "";
                            if (Model.Invoice.Id > 0)
                            {
                                #region Get the file
                                if (Session[SessionKeys.InvoicePdfSession] != null)
                                {
                                    List<InvoiceSessionModel> ModelList = (List<InvoiceSessionModel>)Session[SessionKeys.InvoicePdfSession];
                                    filename = ModelList.Where(x => x.InvoiceId == Model.Invoice.InvoiceId).Select(x => x.FileName).FirstOrDefault();

                                }
                                if (string.IsNullOrWhiteSpace(filename))
                                {
                                    List<CreateInvoice> createinvoicelist = new List<CreateInvoice>();
                                    createinvoicelist.Add(Model);
                                    filename = SaveInvoiceToPdf(createinvoicelist, new List<int>());
                                }
                                #endregion

                                var FileName = filename;
                                FileName = FileName.Split('/').Last();
                                var Filepath = filename;

                                string Full_Path = S3Domain + Filepath.TrimStart('/');


                                /// Mayur :: File Download to temp folder :start

                                webClient = new WebClient();
                                FileDataInBytes = webClient.DownloadData(Full_Path);

                                File(FileDataInBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName).ToString();

                                decimal _fileSize = (decimal)FileDataInBytes.Length / 1024;
                                _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);


                                var Temp_FileName = Server.MapPath("~/EmailFileCache/inv_" + FileName);

                                if (!System.IO.File.Exists(Temp_FileName))
                                {
                                    System.IO.File.WriteAllBytes(Temp_FileName, FileDataInBytes);
                                }
                                else
                                {
                                    System.IO.File.WriteAllBytes(Temp_FileName, FileDataInBytes);
                                }



                                //var cusobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Invoice.CustomerId);
                                var comobj = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
                                InvoiceCreatedEmail email = new InvoiceCreatedEmail()
                                {
                                    CompanyName = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName,
                                    CustomerName = Model.Invoice.CustomerName,
                                    BalanceDue = Model.Invoice.TotalAmount != null ? LabelHelper.CurrentTransMakeCurrency.MakeCurrency() + Model.Invoice.TotalAmount.Value.ToString("0,0.00") : "0.00",
                                    DueDate = Model.Invoice.DueDate.HasValue ? Model.Invoice.DueDate.Value.ToString("MM/dd/yy") : "",
                                    InvoiceId = Model.Invoice.InvoiceId,
                                    ToEmail = Model.EmailAddress,
                                    EmailBody = Model.EmailDescription,
                                    ccEmail = Model.Invoice.ccEmail,
                                    Subject = Model.EmailSubject,
                                    FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : "info@rmrcloud.com",
                                    FromName = string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName),
                                    InvoicePdf = new Attachment(
                                          Temp_FileName,
                                         MediaTypeNames.Application.Octet)
                                };
                                responseEmailSent = _Util.Facade.MailFacade.SendInvoiceCreatedEmail(email, CurrentUser.CompanyId.Value);
                                email.InvoicePdf.Dispose();
                                invCustomer.PreferedEmail = true;
                                _Util.Facade.CustomerFacade.UpdateCustomer(invCustomer);

                                if (responseEmailSent)
                                {
                                    #region Log for Invoice Send
                                    base.AddUserActivityForCustomer("Invoice is sent #Ref:" + Model.Invoice.InvoiceId, LabelHelper.ActivityAction.SendInvoice, Model.Invoice.CustomerId, null, Model.Invoice.InvoiceId);
                                    #endregion
                                    #region Customer File
                                    string InvoiceNo = Model.Invoice.Id.GenerateInvoiceNo();

                                    bool res = false;
                                    var objCustomerFile = _Util.Facade.CustomerFileFacade.GetCustomerFileByDescriptionAndCustomerId(InvoiceNo, invCustomer.CustomerId);
                                    {
                                        if (objCustomerFile != null)
                                        {
                                            res = _Util.Facade.CustomerFileFacade.DeleteEstimateFile(objCustomerFile.Id);
                                            if (res == true)
                                            {
                                                CustomerFile cuf = new CustomerFile()
                                                {
                                                    CompanyId = CurrentUser.CompanyId.Value,
                                                    FileId = Guid.NewGuid(),
                                                    FileSize = (double)_fileSize,
                                                    CustomerId = invCustomer.CustomerId,
                                                    FileDescription = invCustomer.Id + "_" + InvoiceNo + ".pdf",
                                                    FileFullName = InvoiceNo + ".pdf",
                                                    Filename = "/" + filename,
                                                    IsActive = true,
                                                    Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                                    CreatedBy = CurrentUser.UserId,
                                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                                    UpdatedBy = CurrentUser.UserId,
                                                    UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                                    WMStatus = LabelHelper.WatermarkStatus.Pending,
                                                    AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                                                };
                                                _Util.Facade.CustomerFileFacade.InsertCustomerFile(cuf);
                                            }
                                        }
                                        else
                                        {
                                            CustomerFile cuf = new CustomerFile()
                                            {
                                                CompanyId = CurrentUser.CompanyId.Value,
                                                FileId = Guid.NewGuid(),
                                                FileSize = (double)_fileSize,
                                                CustomerId = invCustomer.CustomerId,
                                                FileDescription = invCustomer.Id + "_" + InvoiceNo + ".pdf",
                                                FileFullName = InvoiceNo + ".pdf",
                                                Filename = "/" + filename,
                                                IsActive = true,
                                                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                                CreatedBy = CurrentUser.UserId,
                                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                                UpdatedBy = CurrentUser.UserId,
                                                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                                WMStatus = LabelHelper.WatermarkStatus.Pending,
                                                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                                            };
                                            _Util.Facade.CustomerFileFacade.InsertCustomerFile(cuf);
                                        }
                                    }
                                    #endregion
                                    string empName = "";
                                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(oldInvoice.CreatedByUid);
                                    if (empobj != null)
                                    {
                                        empName = empobj.FirstName + " " + empobj.LastName;
                                    }
                                    CustomerSnapshot objEstimateEmailLog = new CustomerSnapshot()
                                    {
                                        CustomerId = Model.Invoice.CustomerId,
                                        CompanyId = CurrentUser.CompanyId.Value,

                                        Description = "Invoice:" + "  " + Model.Invoice.InvoiceId + " " + "email sent by " + "<b>" + empName + "</b>",
                                        Logdate = DateTime.Now.UTCCurrentTime(),
                                        Updatedby = CurrentUser.Identity.Name,
                                        Type = "CustomerMailHistory"
                                    };
                                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEstimateEmailLog);
                                    CustomerAgreement objagree = new CustomerAgreement()
                                    {
                                        CustomerId = Model.Invoice.CustomerId,
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        InvoiceId = Model.Invoice.InvoiceId,
                                        Type = LabelHelper.EstimateStatus.SentToCustomer,
                                        AddedDate = DateTime.Now.UTCCurrentTime()
                                    };
                                    _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objagree);

                                }
                            }
                            else
                            {

                                if (Session[SessionKeys.InvoicePdfSession] != null)
                                {
                                    List<InvoiceSessionModel> ModelList = (List<InvoiceSessionModel>)Session[SessionKeys.InvoicePdfSession];
                                    filename = ModelList.Where(x => x.InvoiceId == Model.Invoice.InvoiceId).Select(x => x.FileName).FirstOrDefault();


                                }
                                if (string.IsNullOrWhiteSpace(filename))
                                {
                                    //Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
                                    //Model.CompanyAddress = MakeAddress(tempCom.Street, tempCom.City, tempCom.State, tempCom.ZipCode, "");
                                    //Model.CompanyEmail = tempCom.EmailAdress;
                                    //Model.CompanyName = tempCom.CompanyName;
                                    List<CreateInvoice> createinvoicelist = new List<CreateInvoice>();
                                    createinvoicelist.Add(Model);
                                    filename = SaveInvoiceToPdf(createinvoicelist, new List<int>());
                                }
                                try
                                {
                                    //Model.EmailAddress
                                    string FromEmail = Model.EmailAddress;

                                    if (Model.EmailDescription.IndexOf("##url##") > -1)
                                    {
                                        string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(Model.Invoice.Id
                                            + "#"
                                            + CurrentUser.CompanyId.Value
                                            + "#"
                                            + invCustomer.CustomerId);
                                        fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Invoice/", encryptedurl);
                                        ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, invCustomer.CustomerId);
                                        Model.EmailDescription = Model.EmailDescription.Replace("##url##", ShortUrl.Code);


                                        /// Mayur :: File Download to temp folder :start

                                        webClient = new WebClient();
                                        FileDataInBytes = webClient.DownloadData(fullurl);

                                        File(FileDataInBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename).ToString();

                                        decimal _fileSize = (decimal)FileDataInBytes.Length / 1024;
                                        _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);


                                        Temp_FileName1 = Server.MapPath("~/EmailFileCache/inv_" + filename);

                                        if (!System.IO.File.Exists(Temp_FileName1))
                                        {
                                            System.IO.File.WriteAllBytes(Temp_FileName1, FileDataInBytes);
                                        }
                                        else
                                        {
                                            System.IO.File.WriteAllBytes(Temp_FileName1, FileDataInBytes);
                                        }
                                        ////
                                    }

                                    InvoiceCreatedEmail email = new InvoiceCreatedEmail()
                                    {
                                        CompanyName = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName,
                                        CustomerName = Model.Invoice.CustomerName,
                                        BalanceDue = Model.Invoice.TotalAmount != null ? "$" + Model.Invoice.TotalAmount.Value.ToString("0,0.00") : HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency(),
                                        DueDate = Model.Invoice.DueDate.HasValue ? Model.Invoice.DueDate.Value.ToString("MM/dd/yy") : "",
                                        InvoiceId = Model.Invoice.InvoiceId,
                                        ToEmail = Model.EmailAddress.Trim(),
                                        EmailBody = Model.EmailDescription,
                                        Subject = Model.EmailSubject,
                                        CustomerId = Model.Invoice.CustomerId.ToString(),
                                        EmployeeId = CurrentUser.UserId.ToString(),
                                        FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : "info@rmrcloud.com",
                                        FromName = CurrentUser.GetFullName(),
                                        ccEmail = Model.Invoice.ccEmail,
                                        InvoicePdf = new Attachment(
                                        Temp_FileName1,
                                        MediaTypeNames.Application.Octet)
                                    };
                                    responseEmailSent = _Util.Facade.MailFacade.SendInvoiceCreatedEmail(email, CurrentUser.CompanyId.Value);
                                    if (email.InvoicePdf != null)
                                    {
                                        email.InvoicePdf.Dispose();
                                    }


                                    if (responseEmailSent)
                                    {
                                        #region Log for Invoice Send
                                        base.AddUserActivityForCustomer("Invoice is sent #Ref:" + Model.Invoice.InvoiceId, LabelHelper.ActivityAction.SendInvoice, Model.Invoice.CustomerId, null, Model.Invoice.InvoiceId);
                                        #endregion
                                        #region Customer File
                                        string InvoiceNo = Model.Invoice.Id.GenerateInvoiceNo();

                                        bool res = false;
                                        var objCustomerFile = _Util.Facade.CustomerFileFacade.GetCustomerFileByDescriptionAndCustomerId(InvoiceNo, invCustomer.CustomerId);
                                        {
                                            if (objCustomerFile != null)
                                            {
                                                res = _Util.Facade.CustomerFileFacade.DeleteEstimateFile(objCustomerFile.Id);
                                                if (res == true)
                                                {
                                                    CustomerFile cuf = new CustomerFile()
                                                    {
                                                        CompanyId = CurrentUser.CompanyId.Value,
                                                        FileId = Guid.NewGuid(),
                                                        CustomerId = invCustomer.CustomerId,
                                                        FileDescription = invCustomer.Id + "_" + InvoiceNo,
                                                        FileFullName = InvoiceNo + ".pdf",
                                                        Filename = filename,
                                                        IsActive = true,
                                                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                                        CreatedBy = CurrentUser.UserId,
                                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                                        UpdatedBy = CurrentUser.UserId,
                                                        UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                                        WMStatus = LabelHelper.WatermarkStatus.Pending,
                                                        AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                                                    };
                                                    _Util.Facade.CustomerFileFacade.InsertCustomerFile(cuf);
                                                }
                                            }
                                            else
                                            {
                                                CustomerFile cuf = new CustomerFile()
                                                {
                                                    CompanyId = CurrentUser.CompanyId.Value,
                                                    FileId = Guid.NewGuid(),
                                                    CustomerId = invCustomer.CustomerId,
                                                    FileDescription = invCustomer.Id + "_" + InvoiceNo,
                                                    FileFullName = InvoiceNo + ".pdf",
                                                    Filename = "/" + filename,
                                                    IsActive = true,
                                                    Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                                    CreatedBy = CurrentUser.UserId,
                                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                                    UpdatedBy = CurrentUser.UserId,
                                                    UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                                    WMStatus = LabelHelper.WatermarkStatus.Pending,
                                                    AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                                                };
                                                _Util.Facade.CustomerFileFacade.InsertCustomerFile(cuf);
                                            }
                                        }
                                        #endregion
                                        string empName = "";
                                        var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(oldInvoice.CreatedByUid);
                                        if (empobj != null)
                                        {
                                            empName = empobj.FirstName + " " + empobj.LastName;
                                        }
                                        CustomerSnapshot objEstimateEmailLog = new CustomerSnapshot()
                                        {
                                            CustomerId = Model.Invoice.CustomerId,
                                            CompanyId = CurrentUser.CompanyId.Value,

                                            Description = "Invoice:" + "  " + Model.Invoice.InvoiceId + " " + "email sent by " + "<b>" + empName + "</b>",
                                            Logdate = DateTime.Now.UTCCurrentTime(),
                                            Updatedby = CurrentUser.Identity.Name,
                                            Type = "CustomerMailHistory"
                                        };
                                        _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEstimateEmailLog);
                                        CustomerAgreement objagree = new CustomerAgreement()
                                        {
                                            CustomerId = Model.Invoice.CustomerId,
                                            CompanyId = CurrentUser.CompanyId.Value,
                                            InvoiceId = Model.Invoice.InvoiceId,
                                            Type = LabelHelper.EstimateStatus.SentToCustomer,
                                            AddedDate = DateTime.Now.UTCCurrentTime()
                                        };
                                        _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objagree);

                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex);
                                }
                            }
                            string errorMessage = "Failed send email";
                            if (responseEmailSent == false)
                            {
                                if (string.IsNullOrWhiteSpace(Model.EmailAddress))
                                {
                                    errorMessage = "Eamil cannot be send without email address";
                                }
                                return Json(new { result = true, message = errorMessage, EmailSent = responseEmailSent });
                            }
                            //if (EmailSent)
                            //{
                            //    string serverFile = Server.MapPath(filename);
                            //    if (System.IO.File.Exists(serverFile))
                            //    {
                            //        System.IO.File.Delete(serverFile);
                            //    }
                            //}
                            #endregion
                        }

                    }


                else // for email sending from list 
                    {
                    if (SendEmail)
                    {
                            #region SendEmail
                            string fullurl;
                            var Temp_FileName1 = "";

                            string filename = "";
                           

                                if (Session[SessionKeys.InvoicePdfSession] != null)
                                {
                                    List<InvoiceSessionModel> ModelList = (List<InvoiceSessionModel>)Session[SessionKeys.InvoicePdfSession];
                                    filename = ModelList.Where(x => x.InvoiceId == oldInvoice.InvoiceId).Select(x => x.FileName).FirstOrDefault(); // for email sending from list 


                        }
                                if (string.IsNullOrWhiteSpace(filename))
                                {
                                    //Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
                                    //Model.CompanyAddress = MakeAddress(tempCom.Street, tempCom.City, tempCom.State, tempCom.ZipCode, "");
                                    //Model.CompanyEmail = tempCom.EmailAdress;
                                    //Model.CompanyName = tempCom.CompanyName;
                                    List<CreateInvoice> createinvoicelist = new List<CreateInvoice>();
                                    createinvoicelist.Add(Model);
                                    filename = SaveInvoiceToPdf(createinvoicelist, new List<int>());
                                }
                                try
                                {


                                        //Model.EmailAddress
                                        string FromEmail = Model.EmailAddress;

                                        string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(Model.Invoice.Id
                                            + "#"
                                            + CurrentUser.CompanyId.Value
                                            + "#"
                                            + invCustomer.CustomerId);
                                        fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Invoice/", encryptedurl);
                                        ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, invCustomer.CustomerId);
                                        Model.EmailDescription = Model.EmailDescription.Replace("##url##", ShortUrl.Code);

                                        var FileName = filename;
                                        FileName = FileName.Split('/').Last();
                                        var Filepath = filename;

                                         string Full_Path = S3Domain + Filepath.TrimStart('/');


                                        /// Mayur :: File Download to temp folder :start

                                        webClient = new WebClient();

                                        FileDataInBytes = webClient.DownloadData(Full_Path);

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
                                        ////
                                   
                                    InvoiceCreatedEmail email = new InvoiceCreatedEmail()
                                    {
                                        CompanyName = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName,
                                        CustomerName = Model.Invoice.CustomerName,
                                        BalanceDue = Model.Invoice.TotalAmount != null ? "$" + Model.Invoice.TotalAmount.Value.ToString("0,0.00") : HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency(),
                                        DueDate = Model.Invoice.DueDate.HasValue ? Model.Invoice.DueDate.Value.ToString("MM/dd/yy") : "",
                                        InvoiceId = Model.Invoice.InvoiceId,
                                        ToEmail = Model.EmailAddress.Trim(),
                                        EmailBody = Model.EmailDescription,
                                        Subject = Model.EmailSubject,
                                        CustomerId = Model.Invoice.CustomerId.ToString(),
                                        EmployeeId = CurrentUser.UserId.ToString(),
                                        FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : "info@rmrcloud.com",
                                        FromName = CurrentUser.GetFullName(),
                                        ccEmail = Model.Invoice.ccEmail,
                                        InvoicePdf = new Attachment(
                                        Temp_FileName1,
                                        MediaTypeNames.Application.Octet)
                                    };
                                    responseEmailSent = _Util.Facade.MailFacade.SendInvoiceCreatedEmail(email, CurrentUser.CompanyId.Value);
                                    if (email.InvoicePdf != null)
                                    {
                                        email.InvoicePdf.Dispose();
                                    }


                                    if (responseEmailSent)
                                    {
                                        #region Log for Invoice Send
                                        base.AddUserActivityForCustomer("Invoice is sent #Ref:" + Model.Invoice.InvoiceId, LabelHelper.ActivityAction.SendInvoice, Model.Invoice.CustomerId, null, Model.Invoice.InvoiceId);
                                        #endregion
                                   
                                        string empName = "";
                                        var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(oldInvoice.CreatedByUid);
                                        if (empobj != null)
                                        {
                                            empName = empobj.FirstName + " " + empobj.LastName;
                                        }

                                        CustomerSnapshot objEstimateEmailLog = new CustomerSnapshot()
                                        {
                                            CustomerId = Model.Invoice.CustomerId,
                                            CompanyId = CurrentUser.CompanyId.Value,

                                            Description = "Invoice:" + "  " + Model.Invoice.InvoiceId + " " + "email sent by " + "<b>" + empName + "</b>",
                                            Logdate = DateTime.Now.UTCCurrentTime(),
                                            Updatedby = CurrentUser.Identity.Name,
                                            Type = "CustomerMailHistory"
                                        };
                                        _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEstimateEmailLog);
                                     

                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex);
                                }
                            
                            string errorMessage = "Failed send email";

                            if (responseEmailSent == false)
                            {
                                if (string.IsNullOrWhiteSpace(Model.EmailAddress))
                                {
                                    errorMessage = "Eamil cannot be send without email address";
                                }
                                return Json(new { result = true, message = errorMessage, EmailSent = responseEmailSent });
                            }
                           
                            #endregion
                     }

                    
                }
                responseResult = true;
                responseMessage += $"Invoice Saved Successfully and Emailed to {Model.EmailAddress}";
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                responseResult = false;
                responseMessage += $"An error occured: {ex.Message} | {ex.StackTrace}";
            }

            return Json(new { result = responseResult, message = responseMessage, EmailSent = responseEmailSent });
        }


        [HttpPost]
        [Authorize]
        public JsonResult CloneDeclinedInvoice(int InvoiceId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
            string TempStatus = "";

            if (inv != null)
            {
                TempStatus = inv.Status;

            }
            if (inv == null)
            {
                return Json(new { result = false, message = "Invoie not found." });
            }
            else if (inv.Status != LabelHelper.InvoiceStatus.Declined)
            {
                return Json(new { result = false, message = "Not a declined Invoice." });
            }
            string DecliendInvoiceId = inv.InvoiceId;

            List<InvoiceDetail> invDetList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(inv.InvoiceId);

            inv.Status = LabelHelper.InvoiceStatus.Open;
            inv.CreatedBy = User.Identity.Name;
            inv.CreatedByUid = CurrentUser.UserId;
            inv.CreatedDate = DateTime.Now.UTCCurrentTime();

            inv.Id = _Util.Facade.InvoiceFacade.InsertInvoice(inv);
            inv.InvoiceId = inv.Id.GenerateInvoiceNo();
            _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
            if (inv != null && TempStatus != inv.Status)
            {
                #region log
                int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                bool newBool = inv.IsARBInvoice ?? false;


                base.AddUserActivityForCustomer("Invoice Status Changed from " + TempStatus + " To " + inv.Status + "#InvoiceId: " + inv.InvoiceId, lineNumber + "," + actionName + "/" + controllerName, inv.CustomerId, null, null, newBool);
                #endregion
            }

            foreach (var item in invDetList)
            {
                item.CreatedBy = User.Identity.Name;
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.InvoiceId = inv.InvoiceId;
                _Util.Facade.InvoiceFacade.InsertInvoiceDetails(item);
            }
            InvoiceNote InvoNote = new InvoiceNote()
            {
                AddedBy = CurrentUser.UserId,
                AddedByText = User.Identity.Name,
                AddedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CurrentUser.CompanyId.Value,
                InvoiceId = inv.Id,
                Note = string.Format("Copied from Invoice {0}", DecliendInvoiceId),
            };
            _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(InvoNote);
            InvoNote = new InvoiceNote()
            {
                AddedBy = CurrentUser.UserId,
                AddedByText = User.Identity.Name,
                AddedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CurrentUser.CompanyId.Value,
                InvoiceId = InvoiceId,
                Note = string.Format("A copy ({0}) of this invoice has been created.", inv.InvoiceId),
            };
            _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(InvoNote);
            return Json(new { result = true, message = string.Format("Invoice coppied successfully. {0} has been created.", inv.InvoiceId) });
        }

        [HttpPost]
        [Authorize]
        public JsonResult CloneCustomerInvoice(int InvoiceId, Guid? CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);

            string TempStatus = "";

            if (inv != null)
            {
                TempStatus = inv.Status;

            }
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId.Value);
            var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
            if (cus == null)
            {
                return Json(new { result = false, message = "Customer not found." });
            }
            if (inv == null)
            {
                return Json(new { result = false, message = "Invoie not found." });
            }
            //else if (inv.Status != LabelHelper.InvoiceStatus.Declined)
            //{
            //    return Json(new { result = false, message = "Not a declined Invoice." });
            //}
            string DecliendInvoiceId = inv.InvoiceId;

            List<InvoiceDetail> invDetList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(inv.InvoiceId);

            inv.InvoiceEmailAddress = cus.EmailAddress;
            inv.CustomerId = cus.CustomerId;
            inv.BillingAddress = AddressHelper.MakeCustomerAddress(cus, "BillingAddress", AddressTemplate);
            inv.ShippingAddress = AddressHelper.MakeCustomerAddress(cus, "BillingAddress", AddressTemplate);
            inv.Status = LabelHelper.InvoiceStatus.Open;
            inv.CreatedBy = User.Identity.Name;
            inv.CreatedByUid = CurrentUser.UserId;
            inv.CreatedDate = DateTime.Now.UTCCurrentTime();

            inv.Id = _Util.Facade.InvoiceFacade.InsertInvoice(inv);
            inv.InvoiceId = inv.Id.GenerateInvoiceNo();
            _Util.Facade.InvoiceFacade.UpdateInvoice(inv);

            if (inv != null && TempStatus != inv.Status)
            {
                #region log
                int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                bool newBool = inv.IsARBInvoice ?? false;


                base.AddUserActivityForCustomer("Invoice Status Changed from " + TempStatus + " To " + inv.Status + "#InvoiceId: " + inv.InvoiceId, lineNumber + "," + actionName + "/" + controllerName, inv.CustomerId, null, null, newBool);
                #endregion
            }
            foreach (var item in invDetList)
            {
                item.CreatedBy = User.Identity.Name;
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.InvoiceId = inv.InvoiceId;
                _Util.Facade.InvoiceFacade.InsertInvoiceDetails(item);
            }
            InvoiceNote InvoNote = new InvoiceNote()
            {
                AddedBy = CurrentUser.UserId,
                AddedByText = User.Identity.Name,
                AddedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CurrentUser.CompanyId.Value,
                InvoiceId = inv.Id,
                Note = string.Format("Copied from Invoice {0}", DecliendInvoiceId),
            };
            _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(InvoNote);
            InvoNote = new InvoiceNote()
            {
                AddedBy = CurrentUser.UserId,
                AddedByText = User.Identity.Name,
                AddedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CurrentUser.CompanyId.Value,
                InvoiceId = InvoiceId,
                Note = string.Format("A copy ({0}) of this invoice has been created.", inv.InvoiceId),
            };
            _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(InvoNote);
            return Json(new { result = true, message = string.Format("Invoice coppied successfully. {0} has been created.", inv.InvoiceId) });
        }

        [HttpPost]
        [Authorize]
        public JsonResult CancelAndRecreateInvoice(int InvoiceId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
            string TempStatus = "";

            if (inv != null)
            {
                TempStatus = inv.Status;

            }
            if (inv == null)
            {
                return Json(new { result = false, message = "Invoie not found." });
            }
            else if (inv.Status != LabelHelper.InvoiceStatus.Paid)
            {
                return Json(new { result = false, message = "Not a paid Invoice." });
            }
            string DecliendInvoiceId = inv.InvoiceId;

            List<InvoiceDetail> invDetList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(inv.InvoiceId);

            inv.Status = LabelHelper.InvoiceStatus.Cancelled;
            _Util.Facade.InvoiceFacade.UpdateInvoice(inv);

            inv.Status = LabelHelper.InvoiceStatus.Open;
            inv.BalanceDue = inv.TotalAmount;
            inv.CreatedBy = User.Identity.Name;
            inv.CreatedByUid = CurrentUser.UserId;
            inv.CreatedDate = DateTime.Now.UTCCurrentTime();

            inv.Id = _Util.Facade.InvoiceFacade.InsertInvoice(inv);
            inv.InvoiceId = inv.Id.GenerateInvoiceNo();
            _Util.Facade.InvoiceFacade.UpdateInvoice(inv);

            if (inv != null && TempStatus != inv.Status)
            {
                #region log
                int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                bool newBool = inv.IsARBInvoice ?? false;
                base.AddUserActivityForCustomer("Invoice Status Changed from " + TempStatus + " To " + inv.Status + "#InvoiceId: " + inv.InvoiceId, lineNumber + "," + actionName + "/" + controllerName, inv.CustomerId, null, null, newBool);
                #endregion
            }
            foreach (var item in invDetList)
            {
                item.CreatedBy = User.Identity.Name;
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.InvoiceId = inv.InvoiceId;
                _Util.Facade.InvoiceFacade.InsertInvoiceDetails(item);
            }
            InvoiceNote InvoNote = new InvoiceNote()
            {
                AddedBy = CurrentUser.UserId,
                AddedByText = User.Identity.Name,
                AddedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CurrentUser.CompanyId.Value,
                InvoiceId = inv.Id,
                Note = string.Format("Copied from Invoice {0}", DecliendInvoiceId),
            };
            _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(InvoNote);
            InvoNote = new InvoiceNote()
            {
                AddedBy = CurrentUser.UserId,
                AddedByText = User.Identity.Name,
                AddedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CurrentUser.CompanyId.Value,
                InvoiceId = InvoiceId,
                Note = string.Format("A copy ({0}) of this invoice has been created.", inv.InvoiceId),
            };
            _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(InvoNote);
            #region Delete Transaction Part
            List<TransactionHistory> TRHS = _Util.Facade.TransactionFacade.GetTransactionHistoryByInvoiceId(InvoiceId);
            foreach (var item in TRHS)
            {
                Transaction TRS = _Util.Facade.TransactionFacade.GetTransactionById(item.TransactionId);

                TRS.Amount = TRS.Amount - item.Amout;

                if (Math.Round(TRS.Amount, 2) == 0)
                {
                    _Util.Facade.TransactionFacade.DeleteTransaction(Convert.ToInt32(TRS.Id));
                }
                else
                {
                    _Util.Facade.TransactionFacade.UpdateTransaction(TRS);
                }
                CustomerCredit cc = _Util.Facade.CustomerFacade.GetCustomerCreditByTransactionId(item.TransactionId);
                if (cc != null)
                {
                    _Util.Facade.CustomerFacade.DeleteCustomerCreditById(cc.Id);
                }
                _Util.Facade.TransactionFacade.DeleteTransactionHistoryById(item.Id);
            }
            #endregion

            return Json(new { result = true, message = string.Format("Invoice coppied successfully. {0} has been created.", inv.InvoiceId) });
        }

        [HttpPost]
        [Authorize]
        public JsonResult CloneInvoice(int InvoiceId, string Cancelled)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
            string TempStatus = "";

            if (inv != null)
            {
                TempStatus = inv.Status;

            }
            #region validations
            if (inv == null)
            {
                return Json(new { result = false, message = "Invoice/Estimate not found." });
            }
            else if (inv.IsEstimate && inv.Status == LabelHelper.EstimateStatus.Init)
            {
                return Json(new { result = false, message = "Estimate not found." });
            }
            else if (inv.IsEstimate && inv.Status == LabelHelper.InvoiceStatus.Init)
            {
                return Json(new { result = false, message = "Invoice not found." });
            }
            else if (inv.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false, message = "Access denied." });
            }
            #endregion

            if (Cancelled == "true")
            {
                if (inv.IsEstimate)
                {
                    inv.Status = LabelHelper.EstimateStatus.CancelEstimate;
                }
                else
                {
                    inv.Status = LabelHelper.InvoiceStatus.Cancelled;
                }
                _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
                if (inv != null && TempStatus != inv.Status)
                {
                    #region log
                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    bool newBool = inv.IsARBInvoice ?? false;
                    base.AddUserActivityForCustomer("Invoice Status Changed from " + TempStatus + " To " + inv.Status + "#InvoiceId: " + inv.InvoiceId, lineNumber + "," + actionName + "/" + controllerName, inv.CustomerId, null, null, newBool);
                    #endregion
                }
            }
            else if (Cancelled == "ReCreate")
            {
                if (inv.IsEstimate)
                {
                    inv.Status = LabelHelper.EstimateStatus.CancelEstimate;
                }
                else
                {
                    inv.Status = LabelHelper.InvoiceStatus.Cancelled;
                }
                _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
                if (inv != null && TempStatus != inv.Status)
                {
                    #region log
                    int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                    bool newBool = inv.IsARBInvoice ?? false;
                    base.AddUserActivityForCustomer("Invoice Status Changed from " + TempStatus + " To " + inv.Status + "#InvoiceId: " + inv.InvoiceId, lineNumber + "," + actionName + "/" + controllerName, inv.CustomerId, null, null, newBool);
                    #endregion
                }
                List<TransactionHistory> TRHSList = _Util.Facade.TransactionFacade.GetTransactionHistoryByInvoiceId(inv.Id);

            }

            inv.Id = 0;
            inv.Id = _Util.Facade.InvoiceFacade.InsertInvoice(inv);

            List<InvoiceDetail> InvDet = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(inv.InvoiceId);
            List<InvoiceNote> InvNotes = _Util.Facade.InvoiceNoteFacade.GetAllInvoiceNoteByInvoiceIdAndCompanyId(InvoiceId, CurrentUser.CompanyId.Value);
            #region Update New Invoice
            string InvoiceName = "";
            if (inv.IsEstimate)
            {
                InvoiceName = "estimate";
                inv.InvoiceId = inv.Id.GenerateEstimateNo();
                inv.Status = LabelHelper.EstimateStatus.Created;
            }
            else
            {
                InvoiceName = "invoice";
                inv.InvoiceId = inv.Id.GenerateInvoiceNo();
                inv.BalanceDue = inv.TotalAmount;
                inv.Status = LabelHelper.InvoiceStatus.Open;
            }

            inv.CreatedBy = User.Identity.Name;
            inv.CreatedByUid = CurrentUser.UserId;
            inv.CreatedDate = DateTime.Now.UTCCurrentTime();
            _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
            #endregion
            #region Insert Invoice Details
            foreach (var item in InvDet)
            {
                item.InvoiceId = inv.InvoiceId;
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.CreatedBy = User.Identity.Name;
                item.Id = 0;
                _Util.Facade.InvoiceFacade.InsertInvoiceDetails(item);
            }
            #endregion
            #region Insert Invoice Note
            foreach (var item in InvNotes)
            {
                item.Id = 0;
                item.InvoiceId = inv.Id;
                _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(item);
            }
            #endregion 

            return Json(new { result = true, message = string.Format("Estimate cloned successfully with new {1} id {0}.", inv.InvoiceId, InvoiceName) });
        }

        [HttpPost]
        [Authorize]
        public JsonResult CloneCustomerEstimate(int InvoiceId, Guid? CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId.Value);
            var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
            #region validations
            if (cus == null)
            {
                return Json(new { result = false, message = "Customer not found." });
            }
            if (inv == null)
            {
                return Json(new { result = false, message = "Invoice/Estimate not found." });
            }
            //else if (inv.IsEstimate && inv.Status == LabelHelper.EstimateStatus.Init)
            //{
            //    return Json(new { result = false, message = "Estimate not found." });
            //}
            //else if (inv.IsEstimate && inv.Status == LabelHelper.InvoiceStatus.Init)
            //{
            //    return Json(new { result = false, message = "Invoice not found." });
            //}
            else if (inv.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false, message = "Access denied." });
            }
            #endregion

            //if (Cancelled == "true")
            //{
            //    if (inv.IsEstimate)
            //    {
            //        inv.Status = LabelHelper.EstimateStatus.CancelEstimate;
            //    }
            //    else
            //    {
            //        inv.Status = LabelHelper.InvoiceStatus.Cancelled;
            //    }
            //    _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
            //}
            //else if (Cancelled == "ReCreate")
            //{
            //    if (inv.IsEstimate)
            //    {
            //        inv.Status = LabelHelper.EstimateStatus.CancelEstimate;
            //    }
            //    else
            //    {
            //        inv.Status = LabelHelper.InvoiceStatus.Cancelled;
            //    }
            //    _Util.Facade.InvoiceFacade.UpdateInvoice(inv);

            //    List<TransactionHistory> TRHSList = _Util.Facade.TransactionFacade.GetTransactionHistoryByInvoiceId(inv.Id);

            //}

            inv.Id = 0;
            inv.Id = _Util.Facade.InvoiceFacade.InsertInvoice(inv);

            List<InvoiceDetail> InvDet = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(inv.InvoiceId);
            List<InvoiceNote> InvNotes = _Util.Facade.InvoiceNoteFacade.GetAllInvoiceNoteByInvoiceIdAndCompanyId(InvoiceId, CurrentUser.CompanyId.Value);
            #region Update New Invoice
            string InvoiceName = "";
            //if (inv.IsEstimate)
            //{
            InvoiceName = "estimate";
            inv.InvoiceId = inv.Id.GenerateEstimateNo();
            inv.Status = LabelHelper.EstimateStatus.Created;
            //}
            //else
            //{
            //    InvoiceName = "invoice";
            //    inv.InvoiceId = inv.Id.GenerateInvoiceNo();
            //    inv.BalanceDue = inv.TotalAmount;
            //    inv.Status = LabelHelper.InvoiceStatus.Open;
            //}
            inv.CustomerId = cus.CustomerId;
            inv.InvoiceEmailAddress = cus.EmailAddress;
            inv.BillingAddress = AddressHelper.MakeCustomerAddress(cus, "BillingAddress", AddressTemplate);
            inv.ShippingAddress = AddressHelper.MakeCustomerAddress(cus, "BillingAddress", AddressTemplate);
            inv.CreatedBy = User.Identity.Name;
            inv.CreatedByUid = CurrentUser.UserId;
            inv.CreatedDate = DateTime.Now.UTCCurrentTime();
            _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
            #endregion
            #region Insert Invoice Details
            foreach (var item in InvDet)
            {
                item.InvoiceId = inv.InvoiceId;
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.CreatedBy = User.Identity.Name;
                item.Id = 0;
                _Util.Facade.InvoiceFacade.InsertInvoiceDetails(item);
            }
            #endregion
            #region Insert Invoice Note
            foreach (var item in InvNotes)
            {
                item.Id = 0;
                item.InvoiceId = inv.Id;
                _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(item);
            }
            #endregion 

            return Json(new { result = true, message = string.Format("Estimate cloned successfully with new {1} id {0}.", inv.InvoiceId, InvoiceName) });
        }


        public ActionResult InvoicePdf(CreateInvoice model)
        {
            return View(model);
        }

        public ActionResult PaymentPdf(int InvoiceId)
        {
            CreateInvoice model = new CreateInvoice();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            model.Invoice = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
            Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.Invoice.CustomerId);
            model.InvoiceDetailList = model.InvoiceDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetialsListByInvoiceId(model.Invoice.InvoiceId);
            model.EmailAddress = tempCustomer.EmailAddress;
            model.InvoiceNotes = _Util.Facade.InvoiceNoteFacade.GetAllInvoiceNoteByInvoiceIdAndCompanyId(model.Invoice.Id, CurrentUser.CompanyId.Value);
            Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(model.Invoice.CompanyId);
            model.CompanyAddress = MakeAddress(tempCom.Street, tempCom.City, tempCom.State, tempCom.ZipCode, "");
            model.CompanyEmail = tempCom.EmailAdress;
            model.CompanyName = tempCom.CompanyName;
            model.CustomerName = tempCustomer.FirstName + " " + tempCustomer.LastName;
            if (model.Invoice.Discountpercent != null)
            {
                model.Discount = ((model.Invoice.Discountpercent * model.SubTotal) / 100).Value;
            }
            return new Rotativa.ViewAsPdf(model);
        }


        public JsonResult AutomatedInvoice()
        {
            //new ManageScheduleTasks().GenerateInvoice();

            return Json(DateTime.Now.UTCCurrentTime().ToString("MMM"), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetEquipmentListByKey(string key, string ExistEquipment = "")
        {
            ExistEquipment = "";//as per alam vais request

            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetEquipmentSearchMaxLoad(CurrentUser.CompanyId.Value);

                List<EquipmentSearchModel> EqList = _Util.Facade.InvoiceFacade.GetEqupmentListBySearchKeyAndCompanyId(key, CurrentUser.CompanyId.Value, ItemsLoadCount, ExistEquipment);
                if (EqList.Count > 0)
                    result = JsonConvert.SerializeObject(EqList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetEquipmentListByKeyTechnicianId(string key, Guid technicianId, string ExistEquipment)
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetEquipmentSearchMaxLoad(CurrentUser.CompanyId.Value);

                //List<EquipmentSearchModel> EqList = _Util.Facade.InvoiceFacade.GetEqupmentListBySearchKeyAndCompanyIdTechnicianId(key, CurrentUser.CompanyId.Value, ItemsLoadCount, technicianId, ExistEquipment);
                List<EquipmentSearchModel> EqList = new List<EquipmentSearchModel>();

                if (technicianId.ToString()=="22222222-2222-2222-2222-222222222222" ||
                    technicianId.ToString() == "22222222-2222-2222-2222-222222222223" ||
                    technicianId.ToString() == "22222222-2222-2222-2222-222222222224" ||
                    technicianId.ToString() == "22222222-2222-2222-2222-222222222225" ||
                    technicianId.ToString() == "22222222-2222-2222-2222-222222222226" ||
                    technicianId.ToString() == "22222222-2222-2222-2222-222222222231" ||
                    technicianId.ToString() == "22222222-2222-2222-2222-222222222232"
                    )
                {
                    EqList = _Util.Facade.InventoryFacade.GetEqupmentListBySearchKeyAndCompanyIdTechnicianId(key, CurrentUser.CompanyId.Value, ItemsLoadCount, technicianId, ExistEquipment);
                }
                else
                {
                    EqList = _Util.Facade.InvoiceFacade.GetEqupmentListBySearchKeyAndCompanyIdTechnicianId(key, CurrentUser.CompanyId.Value, ItemsLoadCount, technicianId, ExistEquipment);
                }
                
                if (EqList.Count > 0)
                    result = JsonConvert.SerializeObject(EqList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetOnlyEquipmentListByKey(string key, string ExistEquipment = "")
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetEquipmentSearchMaxLoad(CurrentUser.CompanyId.Value);

                List<EquipmentSearchModel> EqList = _Util.Facade.InvoiceFacade.GetOnlyEqupmentListBySearchKeyAndCompanyId(key, CurrentUser.CompanyId.Value, ItemsLoadCount, ExistEquipment);
                if (EqList.Count > 0)
                    result = JsonConvert.SerializeObject(EqList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetEquipmentListByTypeAndKey(string key, string ExistEquipment = "", string EqpTypeId = "")
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetEquipmentSearchMaxLoad(CurrentUser.CompanyId.Value);

                List<EquipmentSearchModel> EqList = _Util.Facade.InvoiceFacade.GetEqupmentListByTypeAndSearchKeyAndCompanyId(key, CurrentUser.CompanyId.Value, ItemsLoadCount, ExistEquipment, EqpTypeId);
                if (EqList.Count > 0)
                    result = JsonConvert.SerializeObject(EqList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetOnlyEquipmentListByTypeAndKey(string key, string ExistEquipment = "", string EqpTypeId = "")
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetEquipmentSearchMaxLoad(CurrentUser.CompanyId.Value);

                List<EquipmentSearchModel> EqList = _Util.Facade.InvoiceFacade.GetOnlyEqupmentListByTypeAndSearchKeyAndCompanyId(key, CurrentUser.CompanyId.Value, ItemsLoadCount, ExistEquipment, EqpTypeId);
                if (EqList.Count > 0)
                    result = JsonConvert.SerializeObject(EqList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetOnlyServiceListByKey(string key, string ExistEquipment = "", string ServiceEquipment = "")
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetEquipmentSearchMaxLoad(CurrentUser.CompanyId.Value);

                List<EquipmentSearchModel> EqList = _Util.Facade.InvoiceFacade.GetOnlyServiceListBySearchKeyAndCompanyId(key, CurrentUser.CompanyId.Value, ItemsLoadCount, ExistEquipment, ServiceEquipment);
                if (EqList.Count > 0)
                    result = JsonConvert.SerializeObject(EqList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public JsonResult GetOnlyServiceListByTypeAndKey(string key, string ExistEquipment = "", string EqpTypeId = "")
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetEquipmentSearchMaxLoad(CurrentUser.CompanyId.Value);

                List<EquipmentSearchModel> EqList = _Util.Facade.InvoiceFacade.GetOnlyServiceListByTypeAndSearchKeyAndCompanyId(key, CurrentUser.CompanyId.Value, ItemsLoadCount, ExistEquipment, EqpTypeId);
                if (EqList.Count > 0)
                    result = JsonConvert.SerializeObject(EqList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public PartialViewResult ShowInvoiceNotes(int InvoiceId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);

            if (inv == null || inv.CompanyId != CurrentUser.CompanyId.Value)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            List<InvoiceNote> Model = _Util.Facade.InvoiceNoteFacade.GetAllInvoiceNoteByInvoiceIdAndCompanyId(InvoiceId, CurrentUser.CompanyId.Value);
            if (inv.IsEstimate)
            {
                ViewBag.TitleText = Localize.T("Estimate Notes For") + " : " + inv.InvoiceId;
            }
            else
            {
                ViewBag.TitleText = Localize.T("Invoice Notes For") + " : " + inv.InvoiceId;
            }


            return PartialView("_ShowInvoiceNotes", Model);
        }

        [Authorize]
        public JsonResult GetCustomerListByKey(string key)
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetCustomerSearchMaxLoad(CurrentUser.CompanyId.Value);

                List<CustomerSearchModel> CusList = _Util.Facade.CustomerFacade.GetCustomerListBySearchKeyAndCompanyId(key, CurrentUser.CompanyId.Value, ItemsLoadCount);
                if (CusList.Count > 0)
                    result = JsonConvert.SerializeObject(CusList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public JsonResult GetLeadListByKey(string key)
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetCustomerSearchMaxLoad(CurrentUser.CompanyId.Value);

                List<CustomerSearchModel> CusList = _Util.Facade.CustomerFacade.GetLeadListBySearchKeyAndCompanyId(key, CurrentUser.CompanyId.Value, ItemsLoadCount);
                if (CusList.Count > 0)
                    result = JsonConvert.SerializeObject(CusList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetInvoice(int Id)
        {
            return View();
        }

        public ActionResult GetInvoicePartial(int Id)
        {
            ViewBag.InvoiceId = Id;
            //Invoice model = new Invoice();
            //model = _Util.Facade.InvoiceFacade.GetInvoiceById(Id);
            //model.InvoiceListDetail = _Util.Facade.InvoiceFacade.GetInvoiceDetialsListByInvoiceId(model.InvoiceId);
            return View();
        }

        public ActionResult GetInvoiceListPartial(string Ids)
        {
            ViewBag.InvoiceId = Ids;
            //Invoice model = new Invoice();
            //model = _Util.Facade.InvoiceFacade.GetInvoiceById(Id);
            //model.InvoiceListDetail = _Util.Facade.InvoiceFacade.GetInvoiceDetialsListByInvoiceId(model.InvoiceId);
            return View();
        }
        public ActionResult PrintInvoiceList(string idlist)
        {
            string filefullpath = "";



            List<int> idList = new List<int>();
            if (!string.IsNullOrWhiteSpace(idlist))
            {
                var sidlist = idlist.Split(',');
                if (sidlist.Length > 0)
                {

                    foreach (var item in sidlist)
                    {
                        int val;
                        if (Int32.TryParse(item, out val))
                        {
                            idList.Add(val);
                        }
                        //idList.Add(Convert.ToInt32(item));
                    }

                }
            }
            //if (idList.Any()) //prevent IndexOutOfRangeException for empty list
            //{
            //    idList.RemoveAt(idList.Count - 1);
            //}
            try
            {
                if (idList.Count > 0)
                {
                    var invpdf = SaveInvoiceToPdf(null, idList);
                    filefullpath = Server.MapPath(invpdf);
                    if (!System.IO.File.Exists(filefullpath))
                    {
                        return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                    }
                }
                string filename = System.IO.Path.GetFileName(filefullpath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(filefullpath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_FileNotFound.cshtml");
            }
        }
        public ActionResult PrintInvoiceListNew(string idlist)
        {
            string filefullpath = "";
            idlist = idlist.Substring(0, idlist.Length - 12);


            List<int> idList = new List<int>();
            if (!string.IsNullOrWhiteSpace(idlist))
            {
                var sidlist = idlist.Split(',');
                if (sidlist.Length > 0)
                {
                    foreach (var item in sidlist)
                    {
                        idList.Add(Convert.ToInt32(item));
                    }
                }
            }
            try
            {
                if (idList.Count > 0)
                {
                    var invpdf = SaveInvoiceToPdf(null, idList);
                    filefullpath = Server.MapPath(invpdf);
                    if (!System.IO.File.Exists(filefullpath))
                    {
                        return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                    }
                }
                string filename = System.IO.Path.GetFileName(filefullpath);
                byte[] fileBytes = System.IO.File.ReadAllBytes(filefullpath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_FileNotFound.cshtml");
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteInvoice(int Id, CreateInvoice Model, Guid? CustomerId, string InvoiceId)
        {
            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerInvoiceDelete))
            {
                return Json(new { result = false, message = "Permission denied." });
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            Invoice inv = _Util.Facade.InvoiceFacade.GetById(Id);
            if (inv == null || inv.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false, message = "Permission denied." });
            }
            List<TransactionHistory> Trhs = _Util.Facade.TransactionFacade.GetAllTransactionHistoryByInvoiceId(inv.Id);
            if (Trhs.Count > 0)
            {
                return Json(new { result = false, message = "This invoice already has one or more transaction(s). You can't delete this invoice." });
            }
            base.AddUserActivityForCustomer("Invoice is deleted #Ref:" + InvoiceId, LabelHelper.ActivityAction.Delete, CustomerId, null, InvoiceId);


            _Util.Facade.InvoiceFacade.DeleteAllInvoiceDetailsByInvoiceId(inv.InvoiceId);

            _Util.Facade.InvoiceFacade.DeleteInvoiceById(Id);

            return Json(new { result = true, message = "Invoice deleted successfully." });
        }


        [Authorize]
        [HttpPost]
        public JsonResult SaveInvoicePdf(CreateInvoice Model, int? InvoiceId)
        {
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
            //{
            //    file.WriteLine("Function started");
            //}
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            string Filename = "";
            if (InvoiceId.HasValue && InvoiceId > 0)
            {
                List<int> InvoiceIdList = new List<int>();
                InvoiceIdList.Add(InvoiceId.Value);
                //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
                //{
                //    file.WriteLine("SaveInvoiceToPdf going to be started");
                //}
                Filename = SaveInvoiceToPdf(new List<CreateInvoice>(), InvoiceIdList);
            }
            else if (Model != null)
            {
                Model.Invoice.CompanyId = CurrentUser.CompanyId.Value;
                List<CreateInvoice> CreateInvoiceList = new List<CreateInvoice>();
                if (Model.Invoice.DueDate.HasValue)
                {
                    //DateTime DueDate = Model.Invoice.DueDate.Value;
                    //DueDate = new DateTime(DueDate.Year, DueDate.Month, DueDate.Day, 23, 59, 59);
                    //Model.Invoice.DueDate = DueDate;//.ClientToUTCTime();
                    Model.Invoice.DueDate = Model.Invoice.DueDate.Value.SetMaxHour();
                }
                if (Model.Invoice.InvoiceDate.HasValue)
                {
                    Model.Invoice.InvoiceDate = Model.Invoice.InvoiceDate.Value.SetZeroHour();

                }
                if (Model.Invoice.ShippingDate.HasValue)
                {
                    //DateTime ShippingDate = Model.Invoice.ShippingDate.Value;
                    //ShippingDate = new DateTime(ShippingDate.Year, ShippingDate.Month, ShippingDate.Day, 23, 59, 59);
                    //Model.Invoice.ShippingDate = ShippingDate;//.ClientToUTCTime();
                    Model.Invoice.ShippingDate = Model.Invoice.ShippingDate.Value.SetMaxHour();

                }
                CreateInvoiceList.Add(Model);

                Filename = SaveInvoiceToPdf(CreateInvoiceList, new List<int>());
            }
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
            //{
            //    file.WriteLine("returning file path");
            //}
            return Json(new { result = true, message = "Invoice Successfully Saved", filePath = Filename });

        }

        [Authorize]
        public JsonResult SaveInvoicePdfList(List<int> InvoiceIdList)
        {
            if (InvoiceIdList.Count() == 0)
            {
                return Json(new { result = false, message = "No invoice selected." }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                string filename = SaveInvoiceToPdf(new List<CreateInvoice>(), InvoiceIdList);
                if (string.IsNullOrWhiteSpace(filename))
                {
                    return Json(new { result = false, message = "No valid invoice selected." });
                }

                return Json(new { result = true, message = "Invoice file generated succsssfully.", filePath = filename });
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "System error occured. Please contact systmen admin." });
            }

        }

        [Authorize]
        public ActionResult DownLoadAllInvoicePdfList(string InvIdList)
        {

            if (InvIdList.Substring(InvIdList.Length - 1) == ",")
            {
                InvIdList = InvIdList.Remove(InvIdList.Length - 1, 1);
            }
            List<int> InvoiceIdList = new List<int>(Array.ConvertAll(InvIdList.Split(','), int.Parse));
            if (InvoiceIdList.Count() == 0)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            try
            {
                string filename = SaveInvoiceToPdf(new List<CreateInvoice>(), InvoiceIdList);
                if (string.IsNullOrWhiteSpace(filename))
                {
                    return Json(new { result = false, message = "No valid invoice selected." });
                }
                string fullfilename = Server.MapPath(filename);
                byte[] fileBytes = System.IO.File.ReadAllBytes(fullfilename);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);


                //return Json(new { result = true, message = "Invoice file generated succsssfully.", filePath = filename });
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
        }


        [Authorize]
        public ActionResult DownloadInvoicePdfList(AllInvoicesFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<Invoice> InvoiceIdList = new List<Invoice>();
            filter.CompanyId = CurrentUser.CompanyId.Value;
            InvoiceIdList = _Util.Facade.InvoiceFacade.GetAllAutogeneratedUnpaidInvoiceByCompanyIdAndInvoiceFor(filter)/*.Select(x=>x.Id).ToList()*/;

            if (InvoiceIdList.Count() == 0)
            {

                return Json(new { result = false, message = "No invoice selected." }, JsonRequestBehavior.AllowGet);
            }
            List<CreateInvoice> InvoiceList = new List<CreateInvoice>();

            Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            string tempcompanyBranch = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            string ComCity = "";
            string ComState = "";
            if (!string.IsNullOrWhiteSpace(tempCom.City))
            {
                ComCity = tempCom.City + ", ";
            }
            if (!string.IsNullOrWhiteSpace(tempCom.State))
            {
                ComState = tempCom.State + " ";
            }
            string CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
            string CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
            string settingskey = @" 'InvoiceSettingsShipping', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
            List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CurrentUser.CompanyId.Value);

            foreach (var invitem in InvoiceIdList)
            {
                //CreateInvoice Model = new CreateInvoice();
                //Model.Invoice = invitem;//_Util.Facade.InvoiceFacade.GetInvoiceById(invitem);
                List<InvoiceDetail> InvoiceDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(invitem.InvoiceId);
                if (invitem == null || invitem.CompanyId != CurrentUser.CompanyId.Value)
                {
                    continue;
                    //return Json(new { result = false });
                }
                if (InvoiceDetailList == null)
                {
                    continue;
                    //return Json(new { result = false, message = "Customer Equipment Found" });
                }
                Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(invitem.CustomerId);

                #region No Need
                /*
                Invoice tempInvo = invitem;//_Util.Facade.InvoiceFacade.GetByInvoiceId(Model.Invoice.InvoiceId);
                
                if (tempCUstomer == null)
                {
                    continue;
                    //return Json(new { result = false, message = "Customer Not Found" });
                }

                //Model.Invoice.Id = invitem.Id;
                Model.Invoice.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;
                if (tempCUstomer.BusinessName != "")
                {
                    Model.CustomerName = tempCUstomer.BusinessName;
                }
                else
                {
                    Model.CustomerName = Model.Invoice.CustomerName;
                }
                //Model.Invoice.IsEstimate = true;
                //Model.Invoice.CreatedBy = tempInvo.CreatedBy;
                //Model.Invoice.CreatedDate = tempInvo.CreatedDate;
                //Model.Invoice.CustomerId = tempInvo.CustomerId;
                //Model.Invoice.CompanyId = tempInvo.CompanyId;
                //Model.Invoice.Status = tempInvo.Status;
                //Model.Invoice.Status = "";

                //if (!Model.Invoice.ShippingCost.HasValue)
                //{
                //    Model.Invoice.ShippingCost = tempInvo.ShippingCost;
                //}
                //Model.Invoice.ShippingAddress = tempInvo.ShippingAddress;
                Model.CusBussinessName = tempCUstomer.BusinessName;
                //Customer tempCus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Invoice.CustomerId);
                //Model.CustomerAddress = tempCus.Address;
                if (!Model.Invoice.BalanceDue.HasValue)
                {
                    Model.Invoice.BalanceDue = tempInvo.BalanceDue;
                }
                Model.SubTotal = 0;
                foreach (var item in Model.InvoiceDetailList)
                {
                    item.CreatedBy = User.Identity.Name;
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    item.CompanyId = CurrentUser.CompanyId.Value;
                    Model.SubTotal = Model.SubTotal + item.TotalPrice.Value;
                }
                if (!string.IsNullOrWhiteSpace(Model.Invoice.DiscountType))
                {
                    if (Model.Invoice.DiscountType == "amount")
                    {
                        if (tempInvo.Discountpercent != null)
                        {
                            Model.Discount = tempInvo.Discountpercent.Value;
                        }
                    }
                    else
                    {
                        if (tempInvo.Discountpercent != null)
                        {
                            Model.Discount = ((tempInvo.Discountpercent / 100) * Model.SubTotal).Value;
                        }
                    }
                }
                Model.CompanyAddress = tempCom.Address;
                Model.CompanyStreet = tempCom.Street;
                Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
                Model.CompanyEmail = tempCom.EmailAdress;
                Model.CompanyName = tempCom.CompanyName;
                Model.PhoneNum = tempCom.Phone;
                Model.CompanyLogo = tempcompanyBranch;
                Model.Invoice.CompanyInfo = CompanyInfo;
                Model.CustomerInfo = CustomerInfo;
                */
                #endregion

                CreateInvoice Model = GetInvoiceModelById(invitem, InvoiceDetailList, tempCom, tempCUstomer);
                Model.InvoiceSetting = new InvoiceSetting();

                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            Model.InvoiceSetting.DepositSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsDiscount")
                        {
                            Model.InvoiceSetting.DiscountSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsShipping")
                        {
                            Model.InvoiceSetting.ShippingSetting = true;
                        }
                    }
                }
                InvoicePaymentDate PayDate = _Util.Facade.TransactionFacade.GetLatestPaymentDateByInvoiceId(Model.Invoice.Id);
                if (PayDate != null)
                {
                    Model.Invoice.TransacationDate = PayDate.PaymentDate;
                }
                InvoiceList.Add(Model);
            }
            if (InvoiceList.Count() == 0)
            {
                return Json(new { result = false, message = "No valid invoice selected." });
            }
            ViewBag.CompanyId = tempCom.CompanyId.ToString();
            try
            {
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Invoice/InvoicePdf.cshtml", InvoiceList)
                {
                    PageSize = Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },
                };
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                return File(applicationPDFData, System.Net.Mime.MediaTypeNames.Application.Octet, "InvoiceList.pdf");
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = "System error occured. Please contact systmen admin." }, JsonRequestBehavior.AllowGet);
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

        public PartialViewResult UnpaidInvoicePartial(Guid? Customerid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<Invoice> invoiceId = _Util.Facade.InvoiceFacade.GetInvoiceIdByCustomerIdAndCompanyId(Customerid.Value, CurrentUser.CompanyId.Value);
            return PartialView("_UnpaidInvoicePartial", invoiceId);
        }

        [Authorize]
        [HttpPost]
        public JsonResult MakeInvoicePaid(int id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser != null)
            {
                if (id > 0)
                {
                    var InvoiceObject = _Util.Facade.InvoiceFacade.MakeInvoicePaidByInvoiceId(id);
                    InvoiceObject.Status = "Paid";
                    var result = _Util.Facade.InvoiceFacade.UpdateInvoice(InvoiceObject);
                    return Json(result);
                }
                else
                {
                    return Json(false);
                }
            }

            else
            {
                return Json(false);
            }
        }
        [Authorize]
        [HttpPost]
        public JsonResult EstimeApproveById(int Id,string Status)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            int cusid = 0;
            string EstimateId;
            Invoice inv = _Util.Facade.InvoiceFacade.GetById(Id);

            var JsonResult = false;
            if (inv == null)
            {
                return Json(new { result = JsonResult, message = "Estimate Not found" });
            }
            if (inv.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = JsonResult, message = "Permission denied." });
            }

            inv.Status = LabelHelper.EstimateStatus.Completed;
            _Util.Facade.InvoiceFacade.UpdateInvoice(inv);


            inv.IsEstimate = false;
            inv.Status = LabelHelper.InvoiceStatus.Open;
            inv.InvoiceFor = LabelHelper.InvoiceFor.Invoice;
            EstimateId = inv.InvoiceId;
            inv.CreatedBy = User.Identity.Name;
            inv.CreatedByUid = CurrentUser.UserId;
            inv.Id = _Util.Facade.InvoiceFacade.InsertInvoice(inv);
            inv.InvoiceId = inv.Id.GenerateInvoiceNo();
            var invomessage = _Util.Facade.GlobalSettingsFacade.GetInvoiceMessageByCompanyId(CurrentUser.CompanyId.Value);
            if (!string.IsNullOrWhiteSpace(invomessage))
            {
                inv.Message = invomessage;
            }
            JsonResult = _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
            if (JsonResult)
            {
                base.AddUserActivityForCustomer("Estimate is Converted To Invoice #InvoiceId:" + inv.InvoiceId, LabelHelper.ActivityAction.UpdateEstimate, inv.CustomerId, null, inv.InvoiceId);

            }

            List<InvoiceDetail> invDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(EstimateId);
            if (invDetailList != null)
            {
                foreach (var item in invDetailList)
                {
                    var InvoiceDetailId = item.Id;
                    item.InvoiceId = inv.InvoiceId;
                    item.Id = _Util.Facade.InvoiceFacade.InsertInvoiceDetails(item);
                }
            }
            InvoiceNote invnote = new InvoiceNote()
            {
                AddedBy = CurrentUser.UserId,
                InvoiceId = inv.Id,
                AddedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CurrentUser.CompanyId.Value,
                Note = "This invoice has been created from Estimate " + EstimateId
            };
            _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(invnote);

            invnote.InvoiceId = Id;
            invnote.Note = string.Format("Invoice {0} has been created from this estimnate", inv.InvoiceId);
            _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(invnote);

            var cusobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(inv.CustomerId);
            base.AddUserActivityForCustomer("Estimate is Approved #Ref:" + EstimateId, "ApproveEstimate,Invoice/ConvertEstimateToInvoice", cusobj.CustomerId, null, EstimateId);

            if (cusobj != null)
            {
                cusid = cusobj.Id;
            }
            return Json(new { result = JsonResult, cusid = cusid, message = string.Format("Invoice {0} has been created.", inv.InvoiceId) });
        }

        [Authorize]//obaydullah2
        [HttpPost]
        public JsonResult ConvertEstimateToInvoice(int Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            int cusid = 0;
            string EstimateId;
            Invoice inv = _Util.Facade.InvoiceFacade.GetById(Id);

            var JsonResult = false;
            if (inv == null)
            {
                return Json(new { result = JsonResult, message = "Estimate Not found" });
            }
            if (inv.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = JsonResult, message = "Permission denied." });
            }

            inv.Status = LabelHelper.EstimateStatus.Completed;
            _Util.Facade.InvoiceFacade.UpdateInvoice(inv);


            inv.IsEstimate = false;
            inv.Status = LabelHelper.InvoiceStatus.Open;
            inv.InvoiceFor = LabelHelper.InvoiceFor.Invoice;
            EstimateId = inv.InvoiceId;
            inv.CreatedBy = User.Identity.Name;
            inv.CreatedByUid = CurrentUser.UserId;
            inv.Id = _Util.Facade.InvoiceFacade.InsertInvoice(inv);
            inv.InvoiceId = inv.Id.GenerateInvoiceNo();
            var invomessage = _Util.Facade.GlobalSettingsFacade.GetInvoiceMessageByCompanyId(CurrentUser.CompanyId.Value);
            if (!string.IsNullOrWhiteSpace(invomessage))
            {
                inv.Message = invomessage;
            }
            JsonResult = _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
            if (JsonResult)
            {
                base.AddUserActivityForCustomer("Estimate is Converted To Invoice #InvoiceId:" + inv.InvoiceId, LabelHelper.ActivityAction.UpdateEstimate, inv.CustomerId, null, inv.InvoiceId);

            }

            List<InvoiceDetail> invDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(EstimateId);
            if (invDetailList != null)
            {
                foreach (var item in invDetailList)
                {
                    var InvoiceDetailId = item.Id;
                    item.InvoiceId = inv.InvoiceId;
                    item.Id = _Util.Facade.InvoiceFacade.InsertInvoiceDetails(item);
                }
            }
            InvoiceNote invnote = new InvoiceNote()
            {
                AddedBy = CurrentUser.UserId,
                InvoiceId = inv.Id,
                AddedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CurrentUser.CompanyId.Value,
                Note = "This invoice has been created from Estimate " + EstimateId
            };
            _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(invnote);

            invnote.InvoiceId = Id;
            invnote.Note = string.Format("Invoice {0} has been created from this estimnate", inv.InvoiceId);
            _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(invnote);

            var cusobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(inv.CustomerId);
            base.AddUserActivityForCustomer("Estimate is Approved #Ref:" + EstimateId, "ApproveEstimate,Invoice/ConvertEstimateToInvoice", cusobj.CustomerId, null, EstimateId);

            if (cusobj != null)
            {
                cusid = cusobj.Id;
            }
            return Json(new { result = JsonResult, cusid = cusid, message = string.Format("Invoice {0} has been created.", inv.InvoiceId) });
        }

        [Authorize]
        public PartialViewResult InvoiceSettingsPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            InvoiceSetting model = new InvoiceSetting()
            {
                DiscountSetting = false,
                DepositSetting = false,
                ShippingSetting = false
            };
            if (CurrentUser != null)
            {
                var ShippingSetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceShippingSettingValueByCompanyId(CurrentUser.CompanyId.Value);
                if (ShippingSetting != null)
                {

                    model.ShippingSetting = ShippingSetting.IsActive.Value;
                }
                var Discountsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceDiscountSettingValueByCompanyId(CurrentUser.CompanyId.Value);
                if (Discountsetting != null)
                {
                    model.DiscountSetting = Discountsetting.IsActive.Value;
                }
                var DepositSetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceDepositSettingValueByCompanyId(CurrentUser.CompanyId.Value);
                if (DepositSetting != null)
                {
                    model.DepositSetting = DepositSetting.IsActive.Value;
                }
            }
            return PartialView("InvoiceSettingsPartial", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult ChangeInvoiceSetting(bool Value, string Datakey)
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
                    GlobalSettingObject.IsActive = Value;
                    GlobalSettingObject.Value = Value.ToString().ToLower();
                    result = _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(GlobalSettingObject);
                }
            }
            return Json(result);
        }

        public PartialViewResult SendEmailInvoice(int Id, string EmailAddress)
        {
            var S3Domain="";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateCustomerInvoice model = new CreateCustomerInvoice();
            model.Invoice = _Util.Facade.InvoiceFacade.GetInvoiceById(Id);
            //var invoiceId = Id.GenerateInvoiceNo(); 
            model.InvoiceId = model.Invoice.InvoiceId;
            model.CustomerId = model.Invoice.CustomerId;
            var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.Invoice.CustomerId);
            var objcom = _Util.Facade.CompanyFacade.GetAllCompanyByCompanyId(CurrentUser.CompanyId.Value).FirstOrDefault();
            if (objcus != null)
            {
                model.CustomerName = objcus.FirstName + " " + objcus.LastName;
                model.CustomerEmailAddress = string.IsNullOrWhiteSpace(EmailAddress) ? objcus.EmailAddress : EmailAddress;

                if (!string.IsNullOrWhiteSpace(objcus.CellNo) && objcus.CellNo.Length > 6)
                {
                    model.CustomerContactNumber = objcus.CellNo;
                }
                else if (!string.IsNullOrWhiteSpace(objcus.PrimaryPhone) && objcus.PrimaryPhone.Length > 6)
                {
                    model.CustomerContactNumber = objcus.PrimaryPhone;
                }
                else if (!string.IsNullOrWhiteSpace(objcus.SecondaryPhone) && objcus.SecondaryPhone.Length > 6)
                {
                    model.CustomerContactNumber = objcus.SecondaryPhone;
                }
            }
            if (objcom != null)
            {
                model.CompanyName = objcom.CompanyName;
                model.CompanyEmail = objcom.EmailAdress;
            }
            model.ShortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/");

            //if (Session[SessionKeys.InvoicePdfSession] != null)
            //{
            //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();
            //    S3Domain = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);

            //    List<InvoiceSessionModel> ModelList = (List<InvoiceSessionModel>)Session[SessionKeys.InvoicePdfSession];
            //    // ViewBag.pdfLocation = S3Domain + "/" + ModelList.Where(x => x.InvoiceId == model.Invoice.InvoiceId).Select(x => x.FileName).FirstOrDefault();
            //    ViewBag.pdfLocation = S3Domain + ModelList.Where(x => x.InvoiceId == model.Invoice.InvoiceId).Select(x => x.FileName).FirstOrDefault();
            //}
            //else
            //{
                List<int> InvoiceIdList = new List<int>();
                InvoiceIdList.Add(Id);

                ViewBag.pdfLocation = SaveInvoiceToPdf(new List<CreateInvoice>(), InvoiceIdList);


            //}

            model.SMSBody = string.Concat("New Invoice from", " ", model.CompanyName, ": ", model.Invoice.InvoiceId, Environment.NewLine
                , Environment.NewLine, model.ShortUrl, "##url##");
            model.EmailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("InvoicePredefineEmailTemplate");
            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            if (objemp != null)
            {
                ViewBag.SalesGuy = objemp.FirstName + " " + objemp.LastName;
                if (!string.IsNullOrWhiteSpace(objemp.Phone))
                {
                    ViewBag.SalesPhone = objemp.Phone;
                }
                else
                {
                    ViewBag.SalesPhone = objcom.Phone;
                }
            }
            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(model.Invoice.Id
                                        + "#"
                                        + CurrentUser.CompanyId.Value
                                        + "#"
                                        + model.CustomerId);
            string fullurl = string.Concat(S3Domain, "/Customer-Invoice/", encryptedurl);
            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, model.CustomerId);
            ViewBag.url = AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code;
            return PartialView("SendEmailInvoice", model);
        }
        //Duplicate method; same as previous one..
        public ActionResult SendEmailInvoicePartial(int id, string EmailAddress)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateCustomerInvoice model = new CreateCustomerInvoice();
            model.Invoice = _Util.Facade.InvoiceFacade.GetInvoiceById(id);
            //var invoiceId = Id.GenerateInvoiceNo(); 
            model.InvoiceId = model.Invoice.InvoiceId;
            model.CustomerId = model.Invoice.CustomerId;
            var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.Invoice.CustomerId);
            var objcom = _Util.Facade.CompanyFacade.GetAllCompanyByCompanyId(CurrentUser.CompanyId.Value).FirstOrDefault();
            if (objcus != null)
            {
                model.CustomerName = objcus.FirstName + " " + objcus.LastName;
                model.CustomerEmailAddress = string.IsNullOrWhiteSpace(EmailAddress) ? objcus.EmailAddress : EmailAddress;
            }
            if (objcom != null)
            {
                model.CompanyName = objcom.CompanyName;
                model.CompanyEmail = objcom.EmailAdress;
            }
            model.ShortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/");
            if (Session[SessionKeys.InvoicePdfSession] != null)
            {

                //AWSS3ObjectService AWSobject = new AWSS3ObjectService();
                var S3Domain = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);

                List<InvoiceSessionModel> ModelList = (List<InvoiceSessionModel>)Session[SessionKeys.InvoicePdfSession];
                ViewBag.pdfLocation = S3Domain + "/" + ModelList.Where(x => x.InvoiceId == model.Invoice.InvoiceId).Select(x => x.FileName).FirstOrDefault();
            }
            model.SMSBody = string.Concat("New Invoice from", " ", model.CompanyName, ": ", model.Invoice.InvoiceId, Environment.NewLine
                , Environment.NewLine, model.ShortUrl, "##url##");

            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            GlobalSetting invPhnGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "IsEstimateAndInvoicePhoneNumberOfUser");
            if (invPhnGlobal != null)
            {
                if (invPhnGlobal.Value.ToLower() == "true")
                {
                    if (objemp != null)
                    {
                        ViewBag.SalesGuy = objemp.FirstName + " " + objemp.LastName;
                        if (!string.IsNullOrWhiteSpace(objemp.Phone))
                        {
                            ViewBag.SalesPhone = objemp.Phone;
                        }
                        else
                        {
                            ViewBag.SalesPhone = objcom.Phone;
                        }
                    }
                }
                else
                {
                    ViewBag.SalesPhone = objcom.Phone;
                }

            }
            else
            {
                ViewBag.SalesPhone = objcom.Phone;
            }

            if (!string.IsNullOrWhiteSpace(model.Invoice.Status) && model.Invoice.Status != LabelHelper.InvoiceStatus.Paid)
            {
                model.EmailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("InvoicePredefineEmailTemplate");
            }
            else
            {
                model.EmailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("PaidInvoicePredefineEmailTemplate");
                if (string.IsNullOrWhiteSpace(model.EmailTemplate.BodyContent))
                {
                    string emailtemplateBody = string.Format("<p><span style='font-weight:600;'>Dear {0}</span>,<br /><br />Please find the attachment of your invoice for proof of payment.<br /><br />We appreciate the opportunity to give you an invoice on our products and services. If you have any questions, please call at {1}. <br /><br />Thank you for your business,<br />{2}<br />{3}</span></p>", model.CustomerName, ViewBag.SalesPhone, ViewBag.SalesGuy, model.CompanyName);
                    model.EmailTemplate.BodyContent = emailtemplateBody;
                }


            }

            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(model.Invoice.Id
                                        + "#"
                                        + CurrentUser.CompanyId.Value
                                        + "#"
                                        + model.CustomerId);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Invoice/", encryptedurl);
            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, model.CustomerId);
            ViewBag.url = AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code;
            ViewBag.InvoiceId = id;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddInvoiceNote(InvoiceNote InvNote)
        {
            InvNote.AddedBy = ((HS.Web.UI.Helper.CustomPrincipal)User).UserId;
            Employee tmpEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(InvNote.AddedBy);


            InvNote.AddedDate = DateTime.Now.UTCCurrentTime();
            InvNote.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value;
            int Id = _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(InvNote);
            string AddedBy = ((HS.Web.UI.Helper.CustomPrincipal)User).GetFullName();
            string AddedDate = DateTime.Now.UTCCurrentTime().UTCToClientTime().ToString("MM/dd/yy hh:mm");
            if (tmpEmp == null)
            {
                Customer tmpCus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(InvNote.AddedBy);
                return Json(new { result = true, Id = Id, AddedDate = AddedDate, Note = InvNote.Note, AddedBy = tmpCus.FirstName + " " + tmpCus.LastName, message = "Invoice note inserted successfully." });
            }
            else
            {
                return Json(new { result = true, Id = Id, AddedDate = AddedDate, Note = InvNote.Note, AddedBy = tmpEmp.FirstName + " " + tmpEmp.LastName, message = "Invoice note inserted successfully." });
            }

        }
        [Authorize]
        [HttpPost]
        public JsonResult ConvertInvoiceStatus(string Invoval, string PaymentMethod, int Invoice_int_Id)
        {
            bool result = false;
            var objInvoInfo = _Util.Facade.InvoiceFacade.GetInvoiceById(Invoice_int_Id);
            if (objInvoInfo != null)
            {
                if (Invoval == "Cancel")
                {
                    objInvoInfo.Status = Invoval;
                    objInvoInfo.PaymentType = PaymentMethod;
                    result = _Util.Facade.InvoiceFacade.UpdateInvoice(objInvoInfo);
                }
                else
                {
                    objInvoInfo.Status = Invoval;
                    objInvoInfo.PaymentType = PaymentMethod;
                    result = _Util.Facade.InvoiceFacade.UpdateInvoice(objInvoInfo);
                }
            }
            return Json(result);
        }

        private string SaveInvoiceToPdf(List<CreateInvoice> InvoiceList, List<int> InvoiceIdList)
        {
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
            //{
            //   file.WriteLine("Inside SaveInvoiceToPdf");
            //}
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

            if (InvoiceIdList != null && InvoiceIdList.Count() > 0)
            {

                InvoiceList = new List<CreateInvoice>();

                foreach (int InvoiceId in InvoiceIdList)
                {
                    CreateInvoice Model = new CreateInvoice();
                    Model.InvoiceSetting = new InvoiceSetting();
                    foreach (var print in printsetting)
                    {
                        if (print.Value.ToLower() == "true")
                        {
                            if (print.SearchKey == "InvoiceSettingsDeposit")
                            {
                                Model.InvoiceSetting.DepositSetting = true;
                            }
                            if (print.SearchKey == "InvoiceSettingsDiscount")
                            {
                                Model.InvoiceSetting.DiscountSetting = true;
                            }
                            if (print.SearchKey == "InvoiceSettingsShipping")
                            {
                                Model.InvoiceSetting.ShippingSetting = true;
                            }
                        }
                    }

                    Model.Invoice = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                    if (Model.Invoice.TaxType == "")
                    {
                        Model.Invoice.TaxType = "Sales Tax";
                    }
                    if (Model.Invoice.DiscountType == "")
                    {
                        Model.Invoice.DiscountType = "percent";
                    }

                    Model.InvoiceDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(Model.Invoice.InvoiceId);
                    if (Model.Invoice == null || Model.Invoice.CompanyId != CurrentUser.CompanyId.Value)
                    {
                        continue;
                    }
                    if (Model.InvoiceDetailList == null || Model.InvoiceDetailList.Count() == 0)
                    {
                        //return Json(new { result = false, message = "Customer Equipment Found" });
                        continue;
                    }
                    Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Invoice.CustomerId);
                    if (tempCUstomer == null)
                    {
                        //return Json(new { result = false, message = "Customer Not Found" });
                        return null;
                    }

                    CreateInvoice ci = GetInvoiceModelById(Model.Invoice, Model.InvoiceDetailList, tempCom, tempCUstomer);

                    ci.InvoiceNotes = _Util.Facade.InvoiceNoteFacade.GetAllInvoiceNoteByInvoiceIdAndCompanyId(InvoiceId, CurrentUser.CompanyId.Value);
                    //Model.Invoice.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
                    if (ci != null)
                    {
                        ci.InvoiceSetting = Model.InvoiceSetting;
                        InvoicePaymentDate PayDate = _Util.Facade.TransactionFacade.GetLatestPaymentDateByInvoiceId(ci.Invoice.Id);
                        if (PayDate != null)
                        {
                            ci.Invoice.TransacationDate = PayDate.PaymentDate;
                        }
                        CreateInvoList.Add(ci);
                    }
                }
                if (InvoiceIdList.Count() == 1)
                {
                    pdfname = InvoiceIdList[0].GenerateInvoiceNo();
                }
                else
                {
                    Random rand = new Random();
                    pdfname = "InvoiceList_" + rand.Next().ToString();
                }
            }
            else if (InvoiceList.Count() > 0)
            {
                //string CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
                foreach (var Model in InvoiceList)
                {
                    if (Model.InvoiceDetailList == null && Model.InvoiceDetailList.Count() == 0)
                    {
                        //return Json(new { result = false, message = "Customer Equipment Found" });
                        continue;
                    }
                    Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Invoice.CustomerId);
                    if (tempCUstomer == null)
                    {
                        //return Json(new { result = false, message = "Customer Not Found" });
                        continue;
                    }
                    //Model.Invoice.CompanyInfo = CompanyInfo;
                    if (string.IsNullOrEmpty(Model.Invoice.InvoiceId) && string.IsNullOrWhiteSpace(Model.Invoice.InvoiceId))
                    {
                        //return Json(new { result = false, message = "Customer Not Found" });
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
                    //Invoice invoice = _Util.Facade.InvoiceFacade.GetInvoiceByInvoiceId(Model.Invoice.InvoiceId);
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

            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
            //{
            //    file.WriteLine("Creating Pdf");
            //}
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Invoice/InvoicePdf.cshtml", CreateInvoList)
            {
                //FileName = "TestView.pdf",
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };




            //byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            //string filename = ConfigurationManager.AppSettings["File.InvoiceFiles"];
            //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            //filename = string.Format(filename, comname);
            //filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + pdfname + ".pdf";
            //string Serverfilename = FileHelper.GetFileFullPath(filename);
          

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


            ///// Tocheck same file already exist it yes increment its count

            //AWSS3ObjectService AWSobject1 = new AWSS3ObjectService();
            //var S3Domain = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);


            //var status = AWSobject1.CheckFileExists(FileKey);
            //int count = 1;

            //while (status)
            //{
            //    string Temp_fileName = Path.GetFileNameWithoutExtension(FileKey); ;
            //    string newFileName = $"{Temp_fileName}_{count}";
            //    FileName = Path.Combine(newFileName + ".pdf");

            //    FileKey = string.Format($"{FilePath}/{FileName}");
            //    break;
            //}

            /////

            var task = Task.Run(async () =>
            {
                AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                await AWSobject.UploadFile(FileKey, applicationPDFData);
                await AWSobject.MakePublic(FileName, FilePath);
            });

            task.Wait();

            //Thread thread = new Thread(async () =>
            //{

            //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

            //    await AWSobject.UploadFile(FileKey, applicationPDFData);
            //    await AWSobject.MakePublic(FileName, FilePath);

            //});
            //thread.Start();
            //Thread.Sleep(5000);

            string returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
            returnurl = returnurl + FileKey;


            ViewBag.ReturnUrl = returnurl;
            ViewBag.FileName = FileName;
            ViewBag.FileKey = FileKey;


            decimal _fileSize = (decimal)applicationPDFData.Length / 1024;
            _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);


            #endregion



            if (CreateInvoList.Count() == 1 || InvoiceIdList.Count() == 1)
            {
                List<InvoiceSessionModel> ModelList = new List<InvoiceSessionModel>();
                string SelectedInvoiceId = CreateInvoList.Select(x => x.Invoice.InvoiceId).FirstOrDefault();

                if (Session[SessionKeys.InvoicePdfSession] != null)
                {
                    ModelList = (List<InvoiceSessionModel>)Session[SessionKeys.InvoicePdfSession];
                    ModelList.RemoveAll(x => x.InvoiceId == SelectedInvoiceId);

                    ModelList.Add(new InvoiceSessionModel
                    {
                        FileName = FileKey,
                        InvoiceId = SelectedInvoiceId
                    });
                    Session[SessionKeys.InvoicePdfSession] = ModelList;

                }
                else
                {
                    ModelList.Add(new InvoiceSessionModel
                    {
                        FileName = FileKey,
                        InvoiceId = SelectedInvoiceId
                    });
                    Session[SessionKeys.InvoicePdfSession] = ModelList;
                }
            }
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
            //{
            //    file.WriteLine("returning path");
            //}
            return returnurl;
        }

        private CreateInvoice GetInvoiceModelById(Invoice Invoice, List<InvoiceDetail> InvoiceDetialList, Company tempCom, Customer tempCUstomer)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateInvoice Model = new CreateInvoice();
            Model.Invoice = Invoice;
            Model.InvoiceDetailList = InvoiceDetialList;

            Model.Invoice.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;

            Model.Invoice.IsEstimate = false;
            //Model.Invoice.InvoiceDate = Invoice.InvoiceDate.HasValue ? Invoice.InvoiceDate.Value : Model.Invoice.InvoiceDate.Value.ClientToUTCTime();
            if (!string.IsNullOrWhiteSpace(Model.Invoice.EstimateTerm) && Model.Invoice.EstimateTerm != "-1")
                Model.Invoice.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(Model.Invoice.EstimateTerm);
            //Model.Invoice.DueDate = Invoice.DueDate.HasValue ? Invoice.DueDate.Value : Model.Invoice.DueDate.Value.ClientToUTCTime();
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

            #region making Name of Address Bold
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.BillingAddress))
            //{
            //    var split = Model.Invoice.BillingAddress.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.Invoice.BillingAddress = NewAddress;
            //    }
            //}
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.ShippingAddress))
            //{
            //    var split = Model.Invoice.ShippingAddress.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.Invoice.ShippingAddress = NewAddress;
            //    }
            //}
            //if (!string.IsNullOrWhiteSpace(Model.InvoiceShipping))
            //{
            //    var split = Model.InvoiceShipping.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.InvoiceShipping = NewAddress;
            //    }
            //}
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
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
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
            //ViewBag.ShippingValue = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(tempCom.CompanyId);

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
            GlobalSetting invComAddress = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ShowInvoiceCompanyAddress");
            if (invComAddress != null)
            {
                Model.ShowInvoiceCompanyAddress = invComAddress.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowInvoiceCompanyAddress = false;
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
                Model.AmountInWord = NumberToWords(Model.Invoice.BalanceDue.Value); //NumberToWords(Model.Invoice.TotalAmount.Value);
            }
            return Model;
        }

        [HttpPost]
        public JsonResult GetCustomerAddressByCustomerId(Guid? CustomerId)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string BillingAddressVal = "";
            string ShippingAddressVal = "";
            if (CustomerId.HasValue)
            {
                var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId.Value);
                if (tempCustomer != null)
                {
                    BillingAddressVal = AddressHelper.MakeCustomerAddress(tempCustomer, AddressHelper.BillingAddress, AddressTemplate);
                    ShippingAddressVal = AddressHelper.MakeCustomerAddress(tempCustomer, AddressHelper.ShippingAddress, AddressTemplate);
                    result = true;
                }
            }
            return Json(new { result = result, BillingAddressVal = BillingAddressVal, ShippingAddressVal = ShippingAddressVal });
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
        #region NumberToWord
        public static string NumberToWords(double Amount)
        {

            string centval = (Amount - (int)Amount).ToString("#.##");
            double valcent = 0;
            if (!string.IsNullOrWhiteSpace(centval))
            {
                valcent = Convert.ToDouble(centval);
            }
            int cent = (int)(valcent * 100);
            int number = (int)Amount;

            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " Billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
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
                        words += "-" + unitsMap[number % 10];
                }
            }
            if (cent > 0)
            {
                words += " " + cent + "/100";
            }
            return words;
        }
        #endregion
        public JsonResult SendInvoiceInEmail(List<string> ArrayItemInvoiceId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var CompanyName = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName;
            bool result = false;
            if (ArrayItemInvoiceId != null)
            {
                List<CreateInvoice> createinvoicelist = new List<CreateInvoice>();

                foreach (var item in ArrayItemInvoiceId)
                {
                    Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceByInvoiceId(item);

                    if (inv != null)
                    {
                        Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(inv.CustomerId);

                        if (cus != null)
                        {
                            string filename = "";
                            CreateInvoice Model = new CreateInvoice();
                            Model.Invoice = inv;

                            Model.InvoiceDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(inv.InvoiceId);
                            //Model.EmailDescription = inv.Description;
                            createinvoicelist.Add(Model);
                            filename = SaveInvoiceToPdf(createinvoicelist, new List<int>());


                            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(Model.Invoice.Id
                                + "#"
                                + CurrentUser.CompanyId.Value
                                + "#"
                                + cus.CustomerId);
                            string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Invoice/", encryptedurl);
                            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, cus.CustomerId);
                            //Model.EmailDescription = Model.EmailDescription.Replace("##url##", ShortUrl.Code);


                            SendEmailToSelectedCustomer SendEmail = new SendEmailToSelectedCustomer()
                            {
                                CompanyName = CompanyName,
                                CustomerName = inv.CustomerName,
                                BalanceDue = inv.TotalAmount != null ? LabelHelper.CurrentTransMakeCurrency.MakeCurrency() + inv.TotalAmount.Value.ToString("0,0.00") : "0.00",
                                DueDate = inv.DueDate.HasValue ? inv.DueDate.Value.ToString("MM/dd/yy") : "",
                                InvoiceId = inv.InvoiceId,
                                ToEmail = cus.EmailAddress,
                                EmailBody = ShortUrl.Url,
                                ccEmail = inv.ccEmail,
                                FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : "info@rmrcloud.com",
                                FromName = string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName),
                                //Subject = "Invoice " + inv.InvoiceId + " from PiiSTech -RMR Cloud",
                                InvoicePdf = new Attachment(
                                                                 FileHelper.GetFileFullPath(filename),
                                                                 MediaTypeNames.Application.Octet)
                            };
                            result = _Util.Facade.MailFacade.SendEmailToSelectedCustomer(SendEmail, CurrentUser.CompanyId.Value);
                            SendEmail.InvoicePdf.Dispose();
                        }
                    }
                }
            }
            return Json(result);
        }

        #region For RMR Statement Section 

        [Authorize]
        [HttpPost]
        public JsonResult SendInvoiceStatementByEmail(InvoiceStatementEmailSendModel EmailModel)
        {
            #region SendEmail

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            #region Validation Check
            if (CurrentUser == null) { return Json(new { result = false, message = "Unauthorized access" }); }
            Company _com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            if (_com == null) { return Json(new { result = false, message = "Invalid company" }); }
            string filename = "";
            string ReturnMessage = "Failed send email";
            if (string.IsNullOrWhiteSpace(EmailModel.ToEmail)) { return Json(new { result = false, message = "Eamil cannot be send without email address" }); }
            if (string.IsNullOrWhiteSpace(EmailModel.EmailDescription)) { return Json(new { result = false, message = "Official email body cannot be empty" }); }
            if (string.IsNullOrWhiteSpace(EmailModel.EmailSubject)) { return Json(new { result = false, message = "Official email subject cannot be empty" }); }
            int InvoiceIntId = 0, CustomerIntId = 0;
            string BodyHtml = string.Format(System.Uri.UnescapeDataString(EmailModel.EmailDescription));
            int.TryParse(EmailModel.Id, out InvoiceIntId);
            int.TryParse(EmailModel.CustomerId, out CustomerIntId);
            if (InvoiceIntId < 1 && CustomerIntId < 1 && string.IsNullOrWhiteSpace(EmailModel.StatementFor)) { return Json(new { result = false, message = "Reload your browser and try again later" }); }
            Customer _customer = _Util.Facade.CustomerFacade.GetById(CustomerIntId);
            if (_customer == null) { return Json(new { result = false, message = "Invalid customer" }); }
            #endregion
            string InvoiceId = InvoiceIntId.GenerateInvoiceNo();
            #region Get the file
            if (Session[SessionKeys.InvoiceStatementPdfSession] != null)
            {
                List<InvoiceSessionModel> ModelList = (List<InvoiceSessionModel>)Session[SessionKeys.InvoiceStatementPdfSession];
                filename = ModelList.Where(x => x.InvoiceId == InvoiceId).Select(x => x.FileName).FirstOrDefault();

            }
            if (string.IsNullOrWhiteSpace(filename))
            {
                List<int> CustomerIntIdList = new List<int>();
                CustomerIntIdList.Add(CustomerIntId);
                filename = SaveInvoiceStatementToPdf(CustomerIntIdList, EmailModel.StatementFor);
            }
            #endregion

            string FromEmailAddress = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : _com.EmailAdress;
            string UserName = string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName);
            if (string.IsNullOrWhiteSpace(UserName)) { UserName = _com.CompanyName; }

            RequisitionCreatedEmail email = new RequisitionCreatedEmail()
            {
                CompanyName = _com.CompanyName,
                ToEmail = EmailModel.ToEmail,
                EmailBody = BodyHtml,
                ccEmail = EmailModel.CCEmail,
                Subject = EmailModel.EmailSubject,
                FromEmail = FromEmailAddress,
                FromName = UserName,
                RequisitionPdf = new Attachment(
                      FileHelper.GetFileFullPath(filename),
                     MediaTypeNames.Application.Octet)
            };
            bool EmailSentResult = _Util.Facade.MailFacade.SendRequisitionCreatedEmail(email, CurrentUser.CompanyId.Value);
            email.RequisitionPdf.Dispose();
            _customer.PreferedEmail = true;
            _Util.Facade.CustomerFacade.UpdateCustomer(_customer);

            if (EmailSentResult)
            {
                string empName = "";
                var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(_customer.CreatedByUid);
                if (empobj != null)
                {
                    empName = empobj.FirstName + " " + empobj.LastName;
                }
                CustomerSnapshot objEstimateEmailLog = new CustomerSnapshot()
                {
                    CustomerId = _customer.CustomerId,
                    CompanyId = CurrentUser.CompanyId.Value,
                    Description = "Invoice statement for " + InvoiceId + " email sent by " + "<b>" + empName + "</b>",
                    Logdate = DateTime.Now.UTCCurrentTime(),
                    Updatedby = CurrentUser.Identity.Name,
                    Type = "CustomerMailHistory"
                };
                _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEstimateEmailLog);
                CustomerAgreement objagree = new CustomerAgreement()
                {
                    CustomerId = _customer.CustomerId,
                    CompanyId = CurrentUser.CompanyId.Value,
                    InvoiceId = InvoiceId,
                    Type = LabelHelper.EstimateStatus.SentToCustomer,
                    AddedDate = DateTime.Now.UTCCurrentTime()
                };
                _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objagree);

                LeadCorrespondence objCor = new LeadCorrespondence()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    CustomerId = _customer.CustomerId,
                    TemplateKey = "InvoiceStatementEmail",
                    Type = LabelHelper.CorrespondenceMessageTyp.Email,
                    ToEmail = EmailModel.ToEmail,
                    Subject = EmailModel.EmailSubject,
                    BodyContent = BodyHtml,
                    SentDate = DateTime.Now.UTCCurrentTime(),
                    IsSystemAutoSent = true,
                    SentBy = CurrentUser.UserId
                };
                _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objCor);
                ReturnMessage = string.Concat("Invoice statement successfully sent to ", EmailModel.ToEmail);
            }
            #endregion
            return Json(new { result = EmailSentResult, message = ReturnMessage, EmailSent = EmailSentResult });
        }
        [Authorize]
        public ActionResult CustomerUnpaidInvoiceStatement(int? CustomerId, string StatementType, bool? PaymentLink)
        {
            #region Validation Check
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null) { return View("~/Views/Shared/_AccessDenied.cshtml"); }
            if (CustomerId == null || CustomerId.Value < 1) { return View("~/Views/Shared/_AccessDenied.cshtml"); }
            Customer _customer = _Util.Facade.CustomerFacade.GetById(CustomerId.Value);
            if (_customer == null) { return View("~/Views/Shared/_AccessDenied.cshtml"); }
            Invoice _inv = _Util.Facade.InvoiceFacade.GetInvoiceByCustomerIdandIsARB(_customer.CustomerId, StatementType);
            if (_inv == null || _inv.Id < 1) { return View("~/Views/Shared/_AccessDenied.cshtml"); }
            #endregion

            string CustomerName = "", SalesGuy = "", ContactPhone = "", CompanyName = "", EmailBody = "", Linkurl = "";
            if (!string.IsNullOrEmpty(_customer.DBA) && !string.IsNullOrWhiteSpace(_customer.DBA)) { CustomerName = _customer.DBA; }
            else if (!string.IsNullOrEmpty(_customer.BusinessName) && !string.IsNullOrWhiteSpace(_customer.BusinessName)) { CustomerName = _customer.BusinessName; }
            else { CustomerName = _customer.FirstName + " " + _customer.LastName; }

            var objcom = _Util.Facade.CompanyFacade.GetAllCompanyByCompanyId(CurrentUser.CompanyId.Value).FirstOrDefault();
            if (objcom != null)
            {
                CompanyName = objcom.CompanyName;
            }
            else
            {
                CompanyName = CurrentUser.CompanyName;
            }
            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            if (objemp != null)
            {
                SalesGuy = objemp.FirstName + " " + objemp.LastName;
                if (!string.IsNullOrWhiteSpace(objemp.Phone))
                {
                    ContactPhone = objemp.Phone;
                }
                else
                {
                    ContactPhone = objcom.Phone;
                }
            }
            #region Email Template Section
            EmailTemplate _emailTemplate = new EmailTemplate();
            Hashtable datatemplate = new Hashtable();
            string Month = _inv.InvoiceDate.HasValue ? _inv.InvoiceDate.Value.ToString("MMMM, yyyy") : DateTime.UtcNow.UTCToClientTime().ToString("MMMM, yyyy");

            if (PaymentLink != null && PaymentLink.Value)
            {
                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(_inv.Id
                                           + "#"
                                           + CurrentUser.CompanyId.Value
                                           + "#"
                                           + _customer.CustomerId
                                           + "#"
                                           + StatementType);
                string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Invoice-Statement/", encryptedurl);
                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, _customer.CustomerId);
                Linkurl = AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code;
                _emailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("InvoiceStatementEmailSendWithLinkFromInvoiceTab");
            }
            else
            {
                _emailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("InvoiceStatementEmailSendFromInvoiceTab");
            }
            datatemplate.Add("CustomerName", CustomerName);
            datatemplate.Add("ExpirationDate", _inv.DueDate);
            datatemplate.Add("SalesPhone", ContactPhone);
            datatemplate.Add("CompanyName", CompanyName);
            datatemplate.Add("SalesGuy", SalesGuy);
            datatemplate.Add("Month", Month.ToLower());
            datatemplate.Add("url", Linkurl);
            datatemplate.Add("SubjectMonth", Month);
            datatemplate.Add("InvoiceNumber", _inv.InvoiceId);
            EmailBody = LabelHelper.ParserHelper(_emailTemplate.BodyContent, datatemplate);
            string EmailSubject = LabelHelper.ParserHelper(_emailTemplate.Subject, datatemplate);
            #endregion
            #region View bag Section
            ViewBag.InvoiceId = _inv.Id;
            ViewBag.CustomerEmailAddress = _customer.EmailAddress;
            ViewBag.EmailSubject = EmailSubject;
            ViewBag.EmailBodyTemplate = EmailBody;
            ViewBag.customerIntId = _customer.Id;
            ViewBag.StatementFor = StatementType;
            string SMSUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/");
            ViewBag.SMSBody = string.Concat("New Invoice Statement ", Month, " from ", CompanyName, ".", Environment.NewLine, Environment.NewLine, SMSUrl, "##url##");
            if (Session[SessionKeys.InvoiceStatementPdfSession] != null)
            {
                List<InvoiceSessionModel> ModelList = (List<InvoiceSessionModel>)Session[SessionKeys.InvoiceStatementPdfSession];
                ViewBag.pdfLocation = AppConfig.DomainSitePath + "/" + ModelList.Where(x => x.InvoiceId == _inv.InvoiceId).Select(x => x.FileName).FirstOrDefault();
            }
            #endregion
            return View();
        }
        [Authorize]
        [HttpPost]
        public JsonResult CreateCustomerUnpaidInvoiceStatement(List<int> CustomerIntIdList, string StatementFor)
        {
            if (CustomerIntIdList == null || (CustomerIntIdList != null && CustomerIntIdList.Count() == 0))
            {
                return Json(new { result = false, message = "No customer selected." }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                string Filepath = SaveInvoiceStatementToPdf(CustomerIntIdList, StatementFor);
                if (string.IsNullOrWhiteSpace(Filepath))
                {
                    return Json(new { result = false, message = "Selected customer have no unpaid invoice." });
                }

                return Json(new { result = true, message = "Statement file generated succsssfully.", filePath = Filepath });
            }
            catch (Exception)
            {
                return Json(new { result = false, message = "System error occured. Please contact systmen admin." });
            }
        }
        [Authorize]
        private string SaveInvoiceStatementToPdf_v2(List<int> CustomerIntIdList, string StatementFor)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            //tempCom.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);

            string pdfname = "";
            if (tempCom == null) { return ""; }
            #region Company Info Viewbag section
            ViewBag.CompanyId = tempCom.CompanyId;
            ViewBag.CompanyName = tempCom.CompanyName;
            ViewBag.CompanyAddress = tempCom.Street;
            ViewBag.CompanyCity = tempCom.City + ", " + tempCom.State + " " + tempCom.ZipCode;
            ViewBag.CompanyEmail = tempCom.EmailAdress;
            ViewBag.CompanyPhone = tempCom.Phone;
            ViewBag.CompanyWebsite = tempCom.Website;
            #endregion
            List<GeneratePdfInvoiceStatementModelList> GetAllDateList = _Util.Facade.InvoiceFacade.GetAllForInvoiceStatementDataByCustomerIntIdListWithType(CustomerIntIdList, StatementFor);
            if (GetAllDateList == null || GetAllDateList.Count < 1)
            {
                return "";
            }
            if (CustomerIntIdList != null && CustomerIntIdList.Count() == 1)
            {
                pdfname = "InvoiceStatement_" + GetAllDateList.Select(x => x.InvoiceStatement.InvoiceId).FirstOrDefault();
            }
            else
            {
                Random rand = new Random();
                pdfname = "InvoiceStatementList_" + rand.Next().ToString();
            }
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Invoice/InvoiceStatementPdf.cshtml", GetAllDateList)
            {
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 }
            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            string filename = ConfigurationManager.AppSettings["File.InvoiceFiles"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            filename = string.Format(filename, comname);
            filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + pdfname + ".pdf";
            string Serverfilename = FileHelper.GetFileFullPath(filename);
            FileHelper.SaveFile(applicationPDFData, Serverfilename);
            if (GetAllDateList.Count() == 1 || CustomerIntIdList.Count() == 1)
            {
                List<InvoiceSessionModel> ModelList = new List<InvoiceSessionModel>();
                string SelectedInvoiceId = GetAllDateList.Select(x => x.InvoiceStatement.InvoiceId).FirstOrDefault();

                if (Session[SessionKeys.InvoiceStatementPdfSession] != null)
                {
                    ModelList = (List<InvoiceSessionModel>)Session[SessionKeys.InvoiceStatementPdfSession];
                    ModelList.RemoveAll(x => x.InvoiceId == SelectedInvoiceId);

                    ModelList.Add(new InvoiceSessionModel
                    {
                        FileName = filename,
                        InvoiceId = SelectedInvoiceId
                    });
                    Session[SessionKeys.InvoiceStatementPdfSession] = ModelList;

                }
                else
                {
                    ModelList.Add(new InvoiceSessionModel
                    {
                        FileName = filename,
                        InvoiceId = SelectedInvoiceId
                    });
                    Session[SessionKeys.InvoiceStatementPdfSession] = ModelList;
                }
            }
            return AppConfig.DomainSitePath + "/" + filename;
        }

        private string SaveInvoiceStatementToPdf(List<int> CustomerIntIdList, string StatementFor)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            //tempCom.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);

            string pdfname = "";
            if (tempCom == null) { return ""; }
            #region Company Info Viewbag section
            ViewBag.CompanyId = tempCom.CompanyId;
            ViewBag.CompanyName = tempCom.CompanyName;
            ViewBag.CompanyAddress = tempCom.Street;
            ViewBag.CompanyCity = tempCom.City + ", " + tempCom.State + " " + tempCom.ZipCode;
            ViewBag.CompanyEmail = tempCom.EmailAdress;
            ViewBag.CompanyPhone = tempCom.Phone;
            ViewBag.CompanyWebsite = tempCom.Website;
            #endregion
            List<GeneratePdfInvoiceStatementModelList> GetAllDateList = _Util.Facade.InvoiceFacade.GetAllForInvoiceStatementDataByCustomerIntIdListWithType(CustomerIntIdList, StatementFor);
            if (GetAllDateList == null || GetAllDateList.Count < 1)
            {
                return "";
            }
            if (CustomerIntIdList != null && CustomerIntIdList.Count() == 1)
            {
                pdfname = "InvoiceStatement_" + GetAllDateList.Select(x => x.InvoiceStatement.InvoiceId).FirstOrDefault();
            }
            else
            {
                Random rand = new Random();
                pdfname = "InvoiceStatementList_" + rand.Next().ToString();
            }
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Invoice/InvoiceStatementPdf.cshtml", GetAllDateList)
            {
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 }
            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);



            //string filename = ConfigurationManager.AppSettings["File.InvoiceFiles"];
            //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            //filename = string.Format(filename, comname);
            //filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + pdfname + ".pdf";
            //string Serverfilename = FileHelper.GetFileFullPath(filename);
            //FileHelper.SaveFile(applicationPDFData, Serverfilename);

           
            //// "mayur" AWS S3 Changes //// Start

            #region File Save on AWS S3

            string returnurl = "";
          
            string filename = ConfigurationManager.AppSettings["File.InvoiceFiles"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            string FilePath = string.Format(filename, comname);
            FilePath += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/";

            String FileName = pdfname + ".pdf";
           


            string FileKey = string.Format($"{FilePath}/{FileName}");


            ///// Tocheck same file already exist it yes increment its count
            ///
            //AWSS3ObjectService AWSobject1 = new AWSS3ObjectService();
            //var S3Domain = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);


            //var status = AWSobject1.CheckFileExists(FileKey);
            //int count = 1;

            //while (status)
            //{
            //    string Temp_fileName = Path.GetFileNameWithoutExtension(FileKey); ;
            //    string newFileName = $"{Temp_fileName}({count})";
            //    FileName = Path.Combine(newFileName + ".pdf");

            //    FileKey = string.Format($"{FilePath}/{FileName}");
            //    break;
            //}

            /////

            var task = Task.Run(async () => {
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
            //Thread.Sleep(5000);

            /// "mayur" used thread for async s3 methods : End



            returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
            returnurl = returnurl + FileKey;


            ViewBag.ReturnUrl = returnurl;
            ViewBag.FileName = FileName;
            ViewBag.FileKey = FileKey;

            #endregion

            //// "mayur" AWS S3 Changes //// End



            if (GetAllDateList.Count() == 1 || CustomerIntIdList.Count() == 1)
            {
                List<InvoiceSessionModel> ModelList = new List<InvoiceSessionModel>();
                string SelectedInvoiceId = GetAllDateList.Select(x => x.InvoiceStatement.InvoiceId).FirstOrDefault();

                if (Session[SessionKeys.InvoiceStatementPdfSession] != null)
                {
                    ModelList = (List<InvoiceSessionModel>)Session[SessionKeys.InvoiceStatementPdfSession];
                    ModelList.RemoveAll(x => x.InvoiceId == SelectedInvoiceId);

                    ModelList.Add(new InvoiceSessionModel
                    {
                        FileName = filename,
                        InvoiceId = SelectedInvoiceId
                    });
                    Session[SessionKeys.InvoiceStatementPdfSession] = ModelList;

                }
                else
                {
                    ModelList.Add(new InvoiceSessionModel
                    {
                        FileName = filename,
                        InvoiceId = SelectedInvoiceId
                    });
                    Session[SessionKeys.InvoiceStatementPdfSession] = ModelList;
                }
            }
            //return AppConfig.DomainSitePath + "/" + filename;
            return ViewBag.FileKey;
        }


        [Authorize]
        [HttpPost]
        public JsonResult SendGroupRMRInvoiceStatementByEmail(List<int> CustomerIntIdList, string StatementType, bool PaymentLink = false)
        {

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool EmailSentResult = false;
            string CustomerName = "", SalesGuy = "", ContactPhone = "", CompanyName = "", EmailBody = "", Linkurl = "", filename = "", EmailSubject = "";
            string ReturnMessage = "Group invoice statement email sent failed";
            if (CustomerIntIdList == null || CustomerIntIdList.Count < 1)
            {
                return Json(new { result = EmailSentResult, message = "Please select at least one customer", EmailSent = EmailSentResult });
            }
            var objcom = _Util.Facade.CompanyFacade.GetAllCompanyByCompanyId(CurrentUser.CompanyId.Value).FirstOrDefault();
            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            foreach (int cusId in CustomerIntIdList)
            {
                Customer _customer = _Util.Facade.CustomerFacade.GetById(cusId);
                Invoice _inv = _Util.Facade.InvoiceFacade.GetInvoiceByCustomerIdandIsARB(_customer.CustomerId, StatementType);

                if (_customer == null || _inv == null || objcom == null || objemp == null)
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(_customer.DBA) && !string.IsNullOrWhiteSpace(_customer.DBA)) { CustomerName = _customer.DBA; }
                else if (!string.IsNullOrEmpty(_customer.BusinessName) && !string.IsNullOrWhiteSpace(_customer.BusinessName)) { CustomerName = _customer.BusinessName; }
                else { CustomerName = _customer.FirstName + " " + _customer.LastName; }

                if (objcom != null)
                {
                    CompanyName = objcom.CompanyName;
                }
                else
                {
                    CompanyName = CurrentUser.CompanyName;
                }

                if (objemp != null)
                {
                    SalesGuy = objemp.FirstName + " " + objemp.LastName;
                    if (!string.IsNullOrWhiteSpace(objemp.Phone))
                    {
                        ContactPhone = objemp.Phone;
                    }
                    else
                    {
                        ContactPhone = objcom.Phone;
                    }
                }
                #region Email Template Section
                EmailTemplate _emailTemplate = new EmailTemplate();
                Hashtable datatemplate = new Hashtable();
                string Month = _inv.InvoiceDate.HasValue ? _inv.InvoiceDate.Value.ToString("MMMM, yyyy") : DateTime.UtcNow.UTCToClientTime().ToString("MMMM, yyyy");

                if (PaymentLink)
                {
                    string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(_inv.Id
                                               + "#"
                                               + CurrentUser.CompanyId.Value
                                               + "#"
                                               + _customer.CustomerId
                                               + "#"
                                               + StatementType);
                    string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Invoice-Statement/", encryptedurl);
                    ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, _customer.CustomerId);
                    Linkurl = AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code;
                    _emailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("InvoiceStatementEmailSendWithLinkFromInvoiceTab");
                }
                else
                {
                    _emailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("InvoiceStatementEmailSendFromInvoiceTab");
                }
                if (_emailTemplate == null) { continue; }
                datatemplate.Add("CustomerName", CustomerName);
                datatemplate.Add("ExpirationDate", _inv.DueDate);
                datatemplate.Add("SalesPhone", ContactPhone);
                datatemplate.Add("CompanyName", CompanyName);
                datatemplate.Add("SalesGuy", SalesGuy);
                datatemplate.Add("Month", Month.ToLower());
                datatemplate.Add("url", Linkurl);
                datatemplate.Add("SubjectMonth", Month);
                datatemplate.Add("InvoiceNumber", _inv.InvoiceId);
                EmailBody = LabelHelper.ParserHelper(_emailTemplate.BodyContent, datatemplate);
                EmailSubject = LabelHelper.ParserHelper(_emailTemplate.Subject, datatemplate);
                #endregion

                #region Get the file
                if (Session[SessionKeys.InvoiceStatementPdfSession] != null)
                {
                    List<InvoiceSessionModel> ModelList = (List<InvoiceSessionModel>)Session[SessionKeys.InvoiceStatementPdfSession];
                    filename = ModelList.Where(x => x.InvoiceId == _inv.InvoiceId).Select(x => x.FileName).FirstOrDefault();

                }
                if (string.IsNullOrWhiteSpace(filename))
                {
                    List<int> CusIntIdList = new List<int>();
                    CusIntIdList.Add(cusId);
                    filename = SaveInvoiceStatementToPdf(CusIntIdList, StatementType);
                }
                #endregion

                string FromEmailAddress = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : objcom.EmailAdress;
                string UserName = string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName);
                if (string.IsNullOrWhiteSpace(UserName)) { UserName = objcom.CompanyName; }

                RequisitionCreatedEmail email = new RequisitionCreatedEmail()
                {
                    CompanyName = CompanyName,
                    ToEmail = "ataul.piistech@gmail.com"/*_customer.EmailAddress*/,
                    EmailBody = EmailBody,
                    Subject = EmailSubject,
                    FromEmail = FromEmailAddress,
                    FromName = UserName,
                    RequisitionPdf = new Attachment(
                          FileHelper.GetFileFullPath(filename),
                         MediaTypeNames.Application.Octet)
                };
                EmailSentResult = _Util.Facade.MailFacade.SendRequisitionCreatedEmail(email, CurrentUser.CompanyId.Value);
                email.RequisitionPdf.Dispose();
                _customer.PreferedEmail = true;
                _Util.Facade.CustomerFacade.UpdateCustomer(_customer);

                #region Record of Email Send
                if (EmailSentResult)
                {
                    string empName = "";
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(_customer.CreatedByUid);
                    if (empobj != null)
                    {
                        empName = empobj.FirstName + " " + empobj.LastName;
                    }
                    CustomerSnapshot objEstimateEmailLog = new CustomerSnapshot()
                    {
                        CustomerId = _customer.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        Description = "Invoice statement for " + _inv.InvoiceId + " email sent by " + "<b>" + empName + "</b>",
                        Logdate = DateTime.Now.UTCCurrentTime(),
                        Updatedby = CurrentUser.Identity.Name,
                        Type = "CustomerMailHistory"
                    };
                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEstimateEmailLog);
                    CustomerAgreement objagree = new CustomerAgreement()
                    {
                        CustomerId = _customer.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        InvoiceId = _inv.InvoiceId,
                        Type = LabelHelper.EstimateStatus.SentToCustomer,
                        AddedDate = DateTime.Now.UTCCurrentTime()
                    };
                    _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objagree);

                    LeadCorrespondence objCor = new LeadCorrespondence()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = _customer.CustomerId,
                        TemplateKey = "InvoiceStatementEmail",
                        Type = LabelHelper.CorrespondenceMessageTyp.Email,
                        ToEmail = _customer.EmailAddress,
                        Subject = EmailSubject,
                        BodyContent = EmailBody,
                        SentDate = DateTime.Now.UTCCurrentTime(),
                        IsSystemAutoSent = true,
                        SentBy = CurrentUser.UserId
                    };
                    _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objCor);
                    ReturnMessage = string.Concat("Group invoice statement email  successfully sent.");
                }
                #endregion

            }
            return Json(new { result = EmailSentResult, message = ReturnMessage, EmailSent = EmailSentResult });
        }
        public ActionResult PrintGroupInvoiceStatementAsPDF(string StrCustomerIdlist, string StatementFor)
        {
            WebClient webClient;
            byte[] fileBytes1;
            string FileFullPath = "";
            string Full_Path="";
            string FileName = "";
            try
            {
                List<int> CustomerIdlist = new List<int>();
                if (!string.IsNullOrEmpty(StrCustomerIdlist) && !string.IsNullOrWhiteSpace(StrCustomerIdlist))
                {
                    string[] ArrCustomerId = StrCustomerIdlist.Split(',');
                    if (ArrCustomerId.Length > 0)
                    {
                        foreach (string item in ArrCustomerId)
                        {
                            CustomerIdlist.Add(Convert.ToInt32(item));
                        }
                    }
                }
                else
                {
                    return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                }

                if (CustomerIdlist != null && CustomerIdlist.Count > 0)
                {
                    string ReturnFileName = SaveInvoiceStatementToPdf(CustomerIdlist, StatementFor);

                    AWSS3ObjectService AWSobject = new AWSS3ObjectService();
                    var S3Domain = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);

                    var Filepath = ReturnFileName;

                    Full_Path = S3Domain + Filepath.TrimStart('/');

                    var File_status = AWSobject.CheckFileExists(Full_Path);

                    if (!File_status)
                    {
                        return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                    }
                }
                else
                {
                    return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                }


                

                webClient = new WebClient();
                fileBytes1 = webClient.DownloadData(Full_Path);

                FileName = Path.GetFileName(Full_Path);

                return File(fileBytes1, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_FileNotFound.cshtml");
            }
        }
        #endregion

        public bool MonitorTaxCalculation(string InvoiceId)
        {
            bool Status = false;
            var userLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            logger.WithProperty("tags", "invoice,tax,calculation")
            .WithProperty("params", InvoiceId)
            .Trace($"Tax Calculation by {userLoggedIn.GetFullName()} for {InvoiceId}.");
            Status = true;
            return Status;
        }
    }
}