using HS.Entities;
using HS.Framework;
using HS.Web.UI.Helper;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using HS.Framework.Utils;
using Localize = HS.Web.UI.Helper.LanguageHelper;
using System.Collections;
using System.Globalization;
using HS.Facade;
using System.IO;
using System.Threading.Tasks;
using OS.AWS.S3.Services;

namespace HS.Web.UI.Controllers
{
    public class EstimateController : BaseController
    {
        // GET: Estimate
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
            return View();

        }
        public static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
        public ActionResult EstimateLeftSetting()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            InvoiceSetting model = new InvoiceSetting();
            GlobalSetting _showServiceSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ShowEstimateServiceSetting");
            if (_showServiceSetting != null)
            {
                model.ShowEstimateServiceSetting = _showServiceSetting.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                model.ShowEstimateServiceSetting = false;
            }
            if (CurrentUser != null)
            {
                string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit','VendorPrice','EstimateTaxSetting'";
                List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CurrentUser.CompanyId.Value);

                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            model.DepositSetting = true;
                        }
                        if (print.SearchKey == "EstimateServiceSetting")
                        {
                            model.ServiceSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsDiscount")
                        {
                            model.DiscountSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsShipping")
                        {
                            model.ShippingSetting = true;
                        }
                        if (print.SearchKey == "VendorPrice")
                        {
                            model.VendorPriceSetting = true;
                        }
                        if (print.SearchKey == "EstimateTaxSetting")
                        {
                            model.ShowEstimateTaxSetting = true;
                        }
                    }
                }
            }
            return PartialView("EstimateLeftSetting", model);
        }
        

        [Authorize]
        [HttpPost]
        public JsonResult ChangeEstimateSetting(bool Value, string Datakey)
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
        public ActionResult EstimatePartial(int? CustomerId, EstimateFilter filter)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
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
            #region count
            if (CustomerId != null)
            {
                Customer customer = _Util.Facade.CustomerFacade.GetCustomerById((int)CustomerId);
                CustomerDetailsTabCount customerDetailsTabCount = _Util.Facade.InvoiceFacade.GetCustomerDetailsTabCountsByCustomerId(customer.CustomerId, CurrentUser.CompanyId.Value);
                ViewBag.OpenEstimateCount = customerDetailsTabCount.OpenEstimateCount;
                ViewBag.CompletedEstimateCount = customerDetailsTabCount.CompletedEstimateCount;
            }


            #endregion
            #region Week Filter Dropdown
            ////Copany start date will be retrive from global settings
            //DateTime CompanyStartDate = new DateTime(2018, 7, 1);
            //int Week = GetIso8601WeekOfYear(CompanyStartDate);

            //int CurrentWeek = GetIso8601WeekOfYear(DateTime.Now);
            //CompanyStartDate = FirstDateOfWeek(CompanyStartDate.Year, Week, ci, DateOffset);

            //DateTime StartDate = new DateTime();
            //DateTime EndDate = new DateTime();
            //string PtoFilter = "-1";

            ////#region CookieJobs
            ////bool fromCookie = false;
            ////string newCookie = "";
            ////if (Request.Cookies[CookieKeys.PtoFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.PtoFilter].Value))
            ////{
            ////    newCookie = Request.Cookies[CookieKeys.PtoFilter].Value;
            ////    newCookie = Server.UrlDecode(newCookie);
            ////    var CookieVals = newCookie.Split(',');
            ////    if (CookieVals.Length == 4)
            ////    {
            ////        StartDate = CookieVals[0].ToDateTime();
            ////        EndDate = CookieVals[1].ToDateTime();
            ////        string SelectedWeek = CookieVals[2];
            ////        if (SelectedWeek.Split('/').Length == 2)
            ////        {
            ////            int.TryParse(SelectedWeek.Split('/')[1], out CurrentWeek);
            ////        }
            ////        PtoFilter = CookieVals[3];

            ////        fromCookie = true;
            ////    }
            ////}
            ////#endregion

            //while (CompanyStartDate < DateTime.Now)
            //{
            //    string suffix = "th";
            //    if (CompanyStartDate.Day == 1 || CompanyStartDate.Day % 20 == 1 || CompanyStartDate.Day % 30 == 1)
            //    {
            //        suffix = "st";
            //    }
            //    else if (CompanyStartDate.Day == 2 || CompanyStartDate.Day % 20 == 2)
            //    {
            //        suffix = "nd";
            //    }
            //    else if (CompanyStartDate.Day == 3 || CompanyStartDate.Day % 20 == 3)
            //    {
            //        suffix = "rd";
            //    }


            //    CompanyStartDate = CompanyStartDate.AddDays(7);
            //    Week = GetIso8601WeekOfYear(CompanyStartDate);
            //}

            //if (StartDate == new DateTime() && EndDate == new DateTime())
            //{
            //    ViewBag.EndDate = CompanyStartDate;
            //    ViewBag.StartDate = CompanyStartDate.AddDays(-7);
            //}
            //else
            //{
            //    ViewBag.EndDate = EndDate;
            //    ViewBag.StartDate = StartDate;
            //}
            #region Common filter
            ViewBag.EndDate = new DateTime();
            ViewBag.StartDate = new DateTime();
            string PtoFilter = "-1";
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
            return PartialView("_EstimatePartial");
        }
       
        public ActionResult EstimateListPartial(int? CustomerId, EstimateFilter filter)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<Invoice> model = new List<Invoice>();

            if (CustomerId.HasValue)
            {
                //bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId.Value, CurrentUser.CompanyId.Value);
                //if (!res)
                //{
                //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                //}

                var customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);
                if (customerInfo != null)
                {
                    model = _Util.Facade.InvoiceFacade.GetAllEstimateListByCustomerIdAndCompanyId(customerInfo.CustomerId, CurrentUser.CompanyId.Value, filter);
                    if (model != null && model.Count > 0)
                    {
                        foreach (var item in model)
                        {
                            if (item.CreatedByUid == new Guid())
                            {
                                var objest = _Util.Facade.InvoiceFacade.GetById(item.Id);
                                if (objest != null)
                                {
                                    objest.CreatedByUid = CurrentUser.UserId;
                                    _Util.Facade.InvoiceFacade.UpdateInvoice(objest);
                                }
                            }
                        }
                    }
                }
            }
            return View(model);
        }
    
        [Authorize]
        public ActionResult AddEstimate(int? id, int? CustomerId, string InvoiceId)
        {
            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            CreateInvoice model;
            List<InvoiceDetail> _tempInvDeltailList = new List<InvoiceDetail>();
            Customer tempCustomer = new Customer();
            if (CustomerId.HasValue && CustomerId > 0)
            {
                //bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId.Value, currentLoggedIn.CompanyId.Value);
                //if (!res)
                //{
                //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                //}
                tempCustomer = _Util.Facade.CustomerFacade.GetById(CustomerId.Value);
            }
            var VendorPriceStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceVendorPriceSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            if (VendorPriceStutas != null)
            {
                ViewBag.VendorPriceValue = VendorPriceStutas.IsActive.Value;
            }
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
                if (!CustomerId.HasValue || CustomerId == 0)
                {
                    tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.Invoice.CustomerId);
                }

                model.Invoice.CustomerName = tempCustomer.FirstName + " " + tempCustomer.LastName;
                if (model.Invoice == null || model.Invoice.CompanyId != currentLoggedIn.CompanyId.Value)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                _tempInvDeltailList = _Util.Facade.InvoiceFacade.GetInvoiceDetialsListByInvoiceId(model.Invoice.InvoiceId);
                //model.EmailAddress = tempCustomer.EmailAddress;
                if (string.IsNullOrWhiteSpace(model.Invoice.InvoiceEmailAddress))
                {
                    model.Invoice.InvoiceEmailAddress = tempCustomer.EmailAddress;
                }
                if (!string.IsNullOrWhiteSpace(model.Invoice.InvoiceEmailAddress)&& model.Invoice.InvoiceEmailAddress!=null)
                {
                    model.EmailAddress = model.Invoice.InvoiceEmailAddress;
                }
                ViewBag.ShippingAddress = model.Invoice.ShippingAddress;
                model.InvoiceNotes = _Util.Facade.InvoiceNoteFacade.GetAllInvoiceNoteByInvoiceIdAndCompanyId(model.Invoice.Id, currentLoggedIn.CompanyId.Value);
                var shippingStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceShippingSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
                if (shippingStutas != null)
                {
                    ViewBag.value = shippingStutas.IsActive.Value;
                }
                if (model.Invoice.LastUpdatedByUid.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.Invoice.LastUpdatedByUid);
                    if (empobj != null)
                    {
                        ViewBag.CreatedUser = HS.Web.UI.Helper.LabelHelper.EstimateStatus.CancelEstimate + empobj.FirstName + " " + empobj.LastName;
                    }
                }
            }
            else
            {
                model = new CreateInvoice();
                model.EmailAddress = tempCustomer.EmailAddress;
                model.Invoice = new Invoice()
                {
                    CustomerId = tempCustomer.CustomerId,
                    IsEstimate = true,
                    InvoiceEmailAddress = tempCustomer.EmailAddress,
                    CustomerName = tempCustomer.FirstName + " " + tempCustomer.LastName,
                    CompanyId = currentLoggedIn.CompanyId.Value,
                    Amount = 0,
                    Status = "Init",
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    InvoiceDate = DateTime.Now.UTCCurrentTime(),
                    CreatedBy = User.Identity.Name,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedByUid = currentLoggedIn.UserId,
                    CreatedByUid = currentLoggedIn.UserId,

                };
                var shippingStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceShippingSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
                var DiscountStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceDiscountSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
                var DipositStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceDepositSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);

                if (shippingStutas != null)
                {
                    ViewBag.value = shippingStutas.IsActive.Value;
                }
                if (DiscountStutas != null)
                {
                    ViewBag.DiscountValue = DiscountStutas.IsActive.Value;
                }
                if (DipositStutas != null)
                {
                    ViewBag.DipositValue = DipositStutas.IsActive.Value;
                }

                var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(currentLoggedIn.CompanyId.Value);
                model.Invoice.BillingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "BillingAddress", AddressTemplate);
                model.Invoice.ShippingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "ShippingAddress", AddressTemplate);
                if (!string.IsNullOrWhiteSpace(tempCustomer.BusinessName))
                {
                    model.CusBussinessName = tempCustomer.BusinessName;
                }
                if (tempCustomer.Type == "Commercial")
                {
                    model.CusType = tempCustomer.Type;
                }
                model.Invoice.Id = _Util.Facade.InvoiceFacade.InsertInvoice(model.Invoice);
                model.Invoice.InvoiceId = model.Invoice.Id.GenerateEstimateNo();
                _Util.Facade.InvoiceFacade.UpdateInvoice(model.Invoice);

                model.Invoice.InvoiceDate = DateTime.UtcNow.UTCToClientTime();
                model.Invoice.DueDate = DateTime.UtcNow.UTCToClientTime();
                //model.Invoice.ShippingDate = DateTime.UtcNow.UTCToClientTime();


                model.InvoiceNotes = _Util.Facade.InvoiceNoteFacade.GetAllInvoiceNoteByInvoiceIdAndCompanyId(model.Invoice.Id, currentLoggedIn.CompanyId.Value);

                List<Equipment> _tempEqpDeltailList = new List<Equipment>();
                _tempEqpDeltailList = _Util.Facade.EquipmentFacade.GetIncludeEstimateEquipment();

              
                if(_tempEqpDeltailList.Count > 0)
                {
                    _tempInvDeltailList = (from u in _tempEqpDeltailList
                                           select new InvoiceDetail
                                           {
                                               EquipName = u.Name,
                                               Quantity = u.Quantity,
                                               UnitPrice = u.Retail,
                                               TotalPrice = u.Total,
                                               EquipmentId = u.EquipmentId,
                                               EquipmentClassId = u.EquipmentClassId,
                                               EquipDetail = u.Description
                                           }).ToList();
                }
                    
                                
                    
                                     
                   
             
            }


            #region View for TaxList

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
                    Value = GetOutOfStateTax.Value.ToString()
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
             

            ViewBag.DiscountMethod = _Util.Facade.LookupFacade.GetLookupByKey("DiscountMethod").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString()
               }).ToList();
            //ViewBag.Term = _Util.Facade.LookupFacade.GetLookupByKey("EstimateTerms").Select(x =>
            //  new SelectListItem()
            //  {
            //      Text = x.DisplayText.ToString(),
            //      Value = x.DataValue.ToString()
            //  }).ToList();
            ViewBag.Term = _Util.Facade.LookupFacade.GetDropdownsByKey("EstimateTerms");

            ViewBag.EstimatePaymentTerms = _Util.Facade.LookupFacade.GetDropdownsByKey("EstimatePaymentTerms");


            //_Util.Facade.LookupFacade.GetLookupByKey("EstimatePaymentTerms").Select(x =>
            // new SelectListItem()
            // {
            //     Text = x.DisplayText.ToString(),
            //     Value = x.DataValue.ToString()
            // }).ToList();

            ViewBag.EstimateContractTerm = _Util.Facade.LookupFacade.GetLookupByKey("EstimateContractTerm").Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();
            ViewBag.EstimateMonitoringDescription = _Util.Facade.LookupFacade.GetLookupByKey("EstimateMonitoringDescription").Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();
            ViewBag.EstimateUpfrontMonth = _Util.Facade.LookupFacade.GetLookupByKey("EstimateUpfrontMonth").Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();
            ViewBag.FinanceCompanyList = _Util.Facade.LookupFacade.GetLookupByKey("FinanceCompany").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                      new SelectListItem()
                      {
                          Text = x.DisplayText.ToString(),
                          Value = x.DataValue.ToString()
                      }).ToList();
            model.InvoiceSetting = new InvoiceSetting();
            var ShippingSetting = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(currentLoggedIn.CompanyId.Value);
            if (ShippingSetting != null)
            {
                model.InvoiceSetting.ShippingSetting = ShippingSetting.ToLower() == "true" ? true : false;
            }
            var TaxSetting = _Util.Facade.GlobalSettingsFacade.GetEstimateTaxSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            if (TaxSetting != null)
            {
                model.InvoiceSetting.ShowEstimateTaxSetting = TaxSetting.IsActive.Value;
            }
            var Discountsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceDiscountSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            if (Discountsetting != null)
            {
                model.InvoiceSetting.DiscountSetting = Discountsetting.IsActive.Value;
            }
            var Servicesetting = _Util.Facade.GlobalSettingsFacade.GetEstimateServiceSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            if (Servicesetting != null)
            {
                model.InvoiceSetting.ServiceSetting = Servicesetting.IsActive.Value;
            }

            #region Service box model create by service setting
            if (model.InvoiceSetting.ServiceSetting == true)
            {
                if (_tempInvDeltailList != null && _tempInvDeltailList.Count > 0)
                {
                    model.InvoiceDetailList = _tempInvDeltailList.Where(x => x.EquipmentClassId == 1).ToList();
                    model.InvoiceDetailServiceList = _tempInvDeltailList.Where(x => x.EquipmentClassId == 2).ToList();
                }
                else
                {
                    model.InvoiceDetailList = new List<InvoiceDetail>();
                    model.InvoiceDetailServiceList = new List<InvoiceDetail>();
                }
            }
            else
            {
                if (_tempInvDeltailList != null && _tempInvDeltailList.Count > 0)
                {
                    model.InvoiceDetailList = _tempInvDeltailList;
                }
                else
                {
                    model.InvoiceDetailList = new List<InvoiceDetail>();
                }
                model.InvoiceDetailServiceList = new List<InvoiceDetail>();
            }
            #endregion
            var DepositSetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceDepositSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            if (DepositSetting != null)
            {
                model.InvoiceSetting.DepositSetting = DepositSetting.IsActive.Value;
            }

            ViewBag.EstimateMessage = _Util.Facade.GlobalSettingsFacade.GetEstimateByCompanyId(currentLoggedIn.CompanyId.Value);
            //ViewBag.Watchtimmer = watch.ElapsedMilliseconds.ToString();
            //watch.Stop();
            GlobalSetting monitoringVal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "EstimateMonitoringAmount");
            GlobalSetting contractTerm = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "EstimateContractTerm");
            GlobalSetting monitoringDes = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "EstimateMonitoringDescription");
            if (monitoringVal != null)
            {
                model.ShowEstimateMonitoringAmount = monitoringVal.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                model.ShowEstimateMonitoringAmount = false;
            }
            if (contractTerm != null)
            {
                model.ShowEstimateContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                model.ShowEstimateContractTerm = false;
            }
            if (monitoringDes != null)
            {
                model.ShowEstimateMonitoringDescription = monitoringDes.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                model.ShowEstimateMonitoringDescription = false;
            }
            GlobalSetting estimateOldBtn = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "EstimateOldButton");
            if (estimateOldBtn != null)
            {
                model.ShowEstimateOldButton = estimateOldBtn.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                model.ShowEstimateOldButton = false;
            }
            //GlobalSetting estimateDefaultLineItem = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "ShowEstimateDefaultLineItem");
            //if (estimateDefaultLineItem != null)
            //{
            //    model.ShowEstimateDefaultLineItem = estimateDefaultLineItem.Value.ToLower() == "true" ? true : false;
            //}
            //else
            //{
            //    model.ShowEstimateDefaultLineItem = false;
            //}
            #region show estimate finance company
            bool IsFinance = false;
            GlobalSetting GlobalSet = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("ShowEstimateFinanceCompany", currentLoggedIn.CompanyId.Value);
            if (GlobalSet != null && !string.IsNullOrWhiteSpace(GlobalSet.Value))
            {
                if (GlobalSet.Value == "true")
                {
                    IsFinance = true;
                }
            }
            #endregion
            #region equipment category
            ViewBag.EquipmentTypeList = GetEquipmentTypeList();

            #endregion
            ViewBag.ShowFinanceCompany = IsFinance;
            return PartialView("AddEstimate", model);
        }
        private List<SelectListItem> GetEquipmentTypeList()
        {
            var currentLoggedIn = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SelectListItem> EquipentTypeList = new List<SelectListItem>();
            EquipentTypeList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            EquipentTypeList.AddRange(ViewBag.EquipmentTypeList = _Util.Facade.EquipmentTypeFacade.GetAllEquipmentCategoryWithSubCategoryByCompanyId(currentLoggedIn.CompanyId.Value).OrderBy(x => x.Text).ToList());
            return EquipentTypeList;
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddEstimate(CreateInvoice Model, bool SendEmail, bool CreatePdf, string ccEmail, bool? ApproveEstimate, List<int> attachedments)
        {
            bool EmailSent = false;
            string PdfLocation = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string statuslogmsg = "";

            #region Update Invoice Table
            Invoice tempInvo = _Util.Facade.InvoiceFacade.GetByInvoiceId(Model.Invoice.InvoiceId);

            #region Data Validations 
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
            CustomerExtended tempCusExtd = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(tempCUstomer.CustomerId);

            if (Model.Invoice.InvoiceDate.HasValue && Model.Invoice.DueDate.HasValue)
            {
                if (Model.Invoice.DueDate.Value < Model.Invoice.InvoiceDate.Value)
                {
                    return Json(new { result = false, message = "Due date should be greater than or equal to invoice date." });
                }
            }
            #endregion

            Model.Invoice.Message = Model.Invoice.InvoiceMessage;
            Model.Invoice.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;
            Model.CustomerName = Model.Invoice.CustomerName;
            Model.Invoice.InvoiceEmailAddress = Model.EmailAddress;
            Model.Invoice.Id = tempInvo.Id;
            Model.Invoice.IsEstimate = true;
            Model.Invoice.CreatedBy = tempInvo.CreatedBy;
            Model.Invoice.CreatedByUid = tempInvo.CreatedByUid;
            //Model.Invoice.InvoiceDate = Model.Invoice.CreatedDate.ClientToUTCTime();
            Model.Invoice.CreatedDate = tempInvo.CreatedDate;
            //Model.Invoice.DueDate = Model.Invoice.DueDate.HasValue? Model.Invoice.DueDate.Value.ClientToUTCTime(): new DateTime();
            //Model.Invoice.CustomerId = tempInvo.CustomerId;
            Model.Invoice.CompanyId = tempInvo.CompanyId;
            Model.Invoice.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            Model.Invoice.LastUpdatedByUid = CurrentUser.UserId;
            if (string.IsNullOrWhiteSpace(ccEmail))
            {
                Model.Invoice.InvoiceCcEmailAddress = tempInvo.InvoiceCcEmailAddress;
            }
            Model.Invoice.ccEmail = ccEmail;
            if (tempInvo.Status == LabelHelper.EstimateStatus.Init
                || string.IsNullOrWhiteSpace(tempInvo.Status))
            {
                Model.Invoice.Status = LabelHelper.EstimateStatus.Created;
            }
            else
            {
                Model.Invoice.Status = tempInvo.Status;
            }

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
            if (string.IsNullOrWhiteSpace(Model.Invoice.CreatedBy)
                || Model.Invoice.CreatedByUid == new Guid())
            {
                Model.Invoice.CreatedBy = User.Identity.Name;
                Model.Invoice.CreatedByUid = CurrentUser.UserId;
            }
            Model.Invoice.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            #region If Approved Convert Estimate to invoice
            if (ApproveEstimate.HasValue
                && ApproveEstimate.Value
                && Model.Invoice.EstimateTerm != LabelHelper.EstimatePaymentTerms.DueUponCompletion)
            {
                Model.Invoice.IsEstimate = false;
                Model.Invoice.InvoiceFor = LabelHelper.InvoiceFor.Invoice;
                Model.Invoice.Status = LabelHelper.InvoiceStatus.Open;
                Model.Invoice.InvoiceId = Model.Invoice.Id.GenerateInvoiceNo();
            }
            #endregion 

            _Util.Facade.InvoiceFacade.UpdateInvoice(Model.Invoice);


            #endregion Update Invoice Table

            #region Update Invoice Details
            bool ckInvoiceAlreadyIn = false;
            List<InvoiceDetail> CKInvoiceDetailList = new List<InvoiceDetail>();
            CKInvoiceDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(Model.Invoice.InvoiceId);
            if (CKInvoiceDetailList != null && CKInvoiceDetailList.Count > 0)
            {
                ckInvoiceAlreadyIn = true;
            }
            _Util.Facade.InvoiceFacade.DeleteAllInvoiceDetailsByInvoiceId(Model.Invoice.InvoiceId);
            Model.SubTotal = 0;
            List<InvoiceDetail> InvoiceDetailList = Model.InvoiceDetailList.OrderBy(x => x.Order).ToList();
            foreach (var item in InvoiceDetailList)
            {
                item.CreatedBy = User.Identity.Name;
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.CompanyId = CurrentUser.CompanyId.Value;
                item.InvoiceId = Model.Invoice.InvoiceId;
                item.Id = _Util.Facade.InvoiceFacade.InsertInvoiceDetails(item);

                item.EquipmentFile = _Util.Facade.EquipmentFileFacade.GetEquipmentFilesByEquipmentId(item.EquipmentId).FirstOrDefault();
                if (item.EquipmentFile == null)
                {
                    item.EquipmentFile = new EquipmentFile();
                }
                Model.SubTotal = Model.SubTotal + item.TotalPrice.Value;
            }
            if (ckInvoiceAlreadyIn)
            {
                if (tempInvo.Status != Model.Invoice.Status)
                {
                    statuslogmsg = "Status Changed from " + tempInvo.Status + " To " + Model.Invoice.Status;
                }
                base.AddUserActivityForCustomer("Estimate is updated #RefId:"+ Model.Invoice.InvoiceId +"</br>"+ statuslogmsg, LabelHelper.ActivityAction.UpdateEstimate, Model.Invoice.CustomerId, null, Model.Invoice.InvoiceId);
            }
            else
            {
                base.AddUserActivityForCustomer("Estimate is added #RefId:" + Model.Invoice.InvoiceId, LabelHelper.ActivityAction.AddEstimate, Model.Invoice.CustomerId, null, Model.Invoice.InvoiceId);
            }
            if (tempInvo.Discountpercent != null)
            {
                Model.Discount = ((tempInvo.Discountpercent * Model.SubTotal) / 100).Value;
            }
            #endregion Update Invoice Details

            #region Insert Into Customer Snapshot Table

            var objEstimateSnapshot = _Util.Facade.CustomerSnapshotFacade.GetCustomerSnapshotDetail(Model.Invoice.InvoiceId.ToString());
            if (objEstimateSnapshot.Count == 0)
            {
                var updatedate = Model.Invoice.LastUpdatedDate.UTCToClientTime();
                CustomerSnapshot objEstimateLog = new CustomerSnapshot()
                {
                    CustomerId = tempCUstomer.CustomerId,
                    CompanyId = CurrentUser.CompanyId.Value,
                    Description = "Estimate " + string.Format("<a onclick=OpenTopToBottomModal('/Estimate/AddEstimate?id={0}&CustomerId={1}') style='cursor: pointer;'>", Model.Invoice.Id, tempCUstomer.Id, tempCUstomer.CustomerId) + "<b>" + Model.Invoice.InvoiceId + "</b>" + "</a>",
                    Logdate = DateTime.Now.UTCCurrentTime(),
                    Updatedby = CurrentUser.FirstName + " " + CurrentUser.LastName,
                    Type = "EstimateCreateHistory"
                };
                _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEstimateLog);
            }

            #endregion

            if (CreatePdf)
            {
                #region Create Pdf
                Model.Invoice.EstimateTerm = "";//don't know the reason behind this line -inan
                if (!string.IsNullOrWhiteSpace(tempInvo.EstimateTerm) && tempInvo.EstimateTerm != "-1")
                    Model.Invoice.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(tempInvo.EstimateTerm);

                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
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
                if (IsPermitted(UserPermissions.CustomerPermissions.ShowSalesPersonEmail))
                {
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(tempInvo.CreatedByUid);
                    if (empobj != null)
                    {
                        Model.CompanyEmail = empobj.Email;
                    }
                }
                else
                {
                    Model.CompanyEmail = tempCom.EmailAdress;
                }
                Model.CompanyName = tempCom.CompanyName;
                Model.PhoneNum = tempCom.Phone;
                Model.CompanyWebsite = tempCom.Website;
                Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
                Model.CompanyCity = tempCom.City;
                Model.CompanyState = tempCom.State;
                Model.CompanyZip = tempCom.ZipCode;
                Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
                Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
                Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
                Model.CustomerState = tempCUstomer.State;
                Model.CustomerZipCode = tempCUstomer.ZipCode;
                Model.CustomerNo = tempCUstomer.CustomerNo;
                Model.CustomerSSN = tempCUstomer.SSN;
                Model.CustomerDOB = tempCUstomer.DateofBirth.HasValue ? tempCUstomer.DateofBirth.Value : new DateTime();
                Model.CustomerDrivingLicense = tempCusExtd!=null ? tempCusExtd.DrivingLicense:"";
                Model.CustomerStreet = tempCUstomer.Street;
                
                //ViewBag.CompanyId = tempCom.CompanyId.ToString();
                //ViewBag.ShippingValue = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(tempCom.CompanyId);
                Model.ShowInvoiceShippingAddress = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(CurrentUser.CompanyId.Value).ToLower() == "true" ? true : false;
                Model.Invoice.CompanyId = tempCom.CompanyId;
                Model.InvoiceSetting = new InvoiceSetting();
                string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CurrentUser.CompanyId.Value);
                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            Model.InvoiceSetting.DepositSetting = true;
                        }
                        if (print.SearchKey == "EstimateServiceSetting")
                        {
                            Model.InvoiceSetting.ServiceSetting = true;
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
                GlobalSetting monitoringVal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringAmount");
                GlobalSetting contractTerm = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateContractTerm");
                GlobalSetting onitWater = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowONITWaterLLCForEstimate");
                GlobalSetting monitoringDes = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringDescription");
                GlobalSetting thompsonEstimate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowThompsonEstimateText");
                GlobalSetting gutterEqpImg = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowGutterEstimateEquipmentImage");
                if (gutterEqpImg != null)
                {
                    Model.ShowGutterEquipmentImage = gutterEqpImg.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowGutterEquipmentImage = false;
                }
                if (thompsonEstimate != null)
                {
                    Model.ShowThompsonEstimateText = thompsonEstimate.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowThompsonEstimateText = false;
                }
                if (monitoringDes != null)
                {
                    Model.ShowEstimateMonitoringDescription = monitoringDes.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateMonitoringDescription = false;
                }
                if (monitoringVal!=null)
                {
                    Model.ShowEstimateMonitoringAmount = monitoringVal.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateMonitoringAmount = false;
                }
                if (contractTerm != null)
                {
                    Model.ShowEstimateContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateContractTerm = false;
                }
                if (onitWater != null)
                {
                    Model.ShowOnitWaterTreatment = onitWater.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowOnitWaterTreatment = false;
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
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("EstimatePdf", Model)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                Random rand = new Random();
                string filename = ConfigurationManager.AppSettings["File.EstimateFiles"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
                filename = string.Format(filename, comname);
                filename += rand.Next().ToString()+ "___" + DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + Model.Invoice.InvoiceId + ".pdf";
                string Serverfilename = FileHelper.GetFileFullPath(filename);
                
                Session[SessionKeys.EstimatePdfSession] = filename;
                FileHelper.SaveFile(applicationPDFData, Serverfilename);
                #region Insert into CustomerFile
                //_Util.Facade.CustomerFileFacade.SaveEstimatePdfFile(filename, Model.Invoice.InvoiceId, tempCUstomer.CustomerId,CurrentUser.CompanyId.Value);
                #endregion

                #endregion Create Pdf

                return Json(new { result = true, emailSent = EmailSent, message = "Invoice Successfully Saved" });
            }
            else if (SendEmail)
            {
                #region SendEmail
                var tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
                PdfLocation = SendEstimateEmail(Model, tempCUstomer, tempCom, attachedments).FileLocation;
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
            else if (ApproveEstimate.HasValue && ApproveEstimate.Value)
            {

                #region Instructions
                /*
                 * **
                 * **
                 * Callers
                 * I.  This part will call only if the administrator approves an estimate.
                 * II. This can be called from customer as well as lead section.
                 * **
                 * Tasks
                 * 1.Convert Lead to customer
                 * 2.Create Estimate_approved.pdf
                 * 3.Convert Estimate to Invoice
                 * 4.Send notification Email/SMS to Customer.
                 * 5.Send Email To sales person
                 * **
                 * Additional Notes
                 * a.there will be no entree on CustomerAgreement Table
                 * **
                 */
                #endregion

                #region 1.Convert Lead to customer
                CustomerCompany cc = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerIdAndCompanyId(tempCUstomer.CustomerId, CurrentUser.CompanyId.Value);
                cc.IsLead = false;
                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cc);
                tempCUstomer.JoinDate = DateTime.Now.UTCCurrentTime();
                tempCUstomer.IsActive = true;
                _Util.Facade.CustomerFacade.UpdateCustomer(tempCUstomer);
                #endregion

                #region 2.Create Estimate_approved.pdf

                if (!string.IsNullOrWhiteSpace(tempInvo.EstimateTerm) && tempInvo.EstimateTerm != "-1")
                    Model.Invoice.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(tempInvo.EstimateTerm);

                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
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
                if (IsPermitted(UserPermissions.CustomerPermissions.ShowSalesPersonEmail))
                {
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(tempInvo.CreatedByUid);
                    if (empobj != null)
                    {
                        Model.CompanyEmail = empobj.Email;
                    }
                }
                else
                {
                    Model.CompanyEmail = tempCom.EmailAdress;
                }
                Model.CompanyName = tempCom.CompanyName;
                Model.PhoneNum = tempCom.Phone;
                Model.CompanyWebsite = tempCom.Website;
                Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
                Model.CompanyCity = tempCom.City;
                Model.CompanyState = tempCom.State;
                Model.CompanyZip = tempCom.ZipCode;
                Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
                Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
                Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
                Model.CustomerState = tempCUstomer.State;
                Model.CustomerZipCode = tempCUstomer.ZipCode;
                Model.CustomerNo = tempCUstomer.CustomerNo;
                Model.CustomerSSN = tempCUstomer.SSN;
                Model.CustomerDOB = tempCUstomer.DateofBirth.HasValue ? tempCUstomer.DateofBirth.Value : new DateTime();
                Model.CustomerDrivingLicense = tempCusExtd != null ? tempCusExtd.DrivingLicense : "";
                Model.CustomerStreet = tempCUstomer.Street;
                Model.ShowInvoiceShippingAddress = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(CurrentUser.CompanyId.Value).ToLower() == "true" ? true : false;
                Model.Invoice.CompanyId = tempCom.CompanyId;
                Model.EstimateImage = _Util.Facade.InvoiceFacade.GetAllEstimateImageByInvoiceId(Model.Invoice.InvoiceId);
                Model.InvoiceSetting = new InvoiceSetting();
                string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CurrentUser.CompanyId.Value);
                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            Model.InvoiceSetting.DepositSetting = true;
                        }
                        if (print.SearchKey == "EstimateServiceSetting")
                        {
                            Model.InvoiceSetting.ServiceSetting = true;
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
                GlobalSetting monitoringVal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringAmount");
                GlobalSetting contractTerm = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateContractTerm");
                GlobalSetting onitWater = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowONITWaterLLCForEstimate");
                GlobalSetting monitoringDes = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringDescription");
                GlobalSetting thompsonEstimate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowThompsonEstimateText");
                GlobalSetting gutterEqpImg = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowGutterEstimateEquipmentImage");
                if (gutterEqpImg != null)
                {
                    Model.ShowGutterEquipmentImage = gutterEqpImg.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowGutterEquipmentImage = false;
                }
                if (thompsonEstimate != null)
                {
                    Model.ShowThompsonEstimateText = thompsonEstimate.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowThompsonEstimateText = false;
                }
                if (monitoringDes != null)
                {
                    Model.ShowEstimateMonitoringDescription = monitoringDes.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateMonitoringDescription = false;
                }
                if (monitoringVal != null)
                {
                    Model.ShowEstimateMonitoringAmount = monitoringVal.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateMonitoringAmount = false;
                }
                if (contractTerm != null)
                {
                    Model.ShowEstimateContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateContractTerm = false;
                }
                if (onitWater != null)
                {
                    Model.ShowOnitWaterTreatment = onitWater.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowOnitWaterTreatment = false;
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
                ViewAsPdf EstimateActionPdf = new Rotativa.ViewAsPdf("~/Views/Estimate/EstimatePdf.cshtml", Model)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] EstimatePdfData = EstimateActionPdf.BuildPdf(ControllerContext);

                #region Save Estimate.pdf to file System  
                string estimateno = Model.Invoice.Id.GenerateEstimateNo();
                string filename = ConfigurationManager.AppSettings["File.EstimateFiles"];
                string comname = tempCom.CompanyName.ReplaceSpecialChar();
                filename = string.Format(filename, comname);
                filename += DateTime.Now.UTCCurrentTime().Year.ToString()
                    + "/" + DateTime.Now.UTCCurrentTime().Month.ToString()
                    + "/" + estimateno + "_Approved.pdf";
                string Serverfilename = FileHelper.GetFileFullPath(filename);
                FileHelper.SaveFile(EstimatePdfData, Serverfilename);
                #endregion

                #region Save CustomerFile
                _Util.Facade.CustomerFileFacade.SaveEstimatePdfFile(AppConfig.DomainSitePath + filename, estimateno, tempCUstomer.CustomerId, CurrentUser.CompanyId.Value, false, true);
                #endregion

                #endregion

                #region 3.Convert Estimate To Invoice
                /**
                 * This part is done on updating estimate
                 */
                #endregion

                #region Send notification Email to customer

                #endregion

                return Json(new { result = true, message = "Estimate converted to invoice successfully.", converted = true });
            }

            return Json(new { result = true, message = string.Concat("Estimate Saved Successfully and Email to ", Model.EmailAddress), filePath = PdfLocation, EmailSent = EmailSent });
        }


        [Authorize]
        [HttpPost]
        public JsonResult DeleteEstimate(int Id, CreateInvoice Model, Guid? CustomerId, string InvoiceId)
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
                return Json(new { result = false, message = "This Estimate already has one or more transaction(s). You can't delete this invoice." });
            }
            base.AddUserActivityForCustomer("Estimate is deleted #Ref:" + InvoiceId, LabelHelper.ActivityAction.Delete, CustomerId, null, InvoiceId);


            _Util.Facade.InvoiceFacade.DeleteAllInvoiceDetailsByInvoiceId(inv.InvoiceId);

            _Util.Facade.InvoiceFacade.DeleteInvoiceById(Id);

            return Json(new { result = true, message = "Estimate deleted successfully." });
        }


        [Authorize]
        [HttpPost]
        public JsonResult ApproveEstimate()
        {
            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult ResendEstimateEmail(int InvoiceId, string EmailAddress)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateInvoice Model = new CreateInvoice();
            Customer tempCUstomer;
            Company tempCom;
            Model.Invoice = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
            if(!string.IsNullOrWhiteSpace(EmailAddress))
            {
                Model.Invoice.InvoiceEmailAddress = EmailAddress;
                _Util.Facade.InvoiceFacade.UpdateInvoice(Model.Invoice);
            }
            if (Model.Invoice == null || Model.Invoice.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false, message = "Access denied." });
            }
            tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Invoice.CustomerId);
            if (tempCUstomer == null)
            {
                return Json(new { result = false, message = "Access denied." });
            }
            tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
            if (tempCom == null)
            {
                return Json(new { result = false, message = "Access denied." });
            }
            Model.Invoice.Message = Model.Invoice.InvoiceMessage;
            Model.Invoice.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;
            Model.CustomerName = Model.Invoice.CustomerName;
            Model.InvoiceDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetialsListByInvoiceId(Model.Invoice.InvoiceId);
            Model.EmailAddress = EmailAddress;

            List<int> temAttId = new List<int>();
            var response = SendEstimateEmail(Model, tempCUstomer, tempCom, temAttId);
            if (response.IsSent)
            {
                return Json(new { result = true, message = string.Format("Email send successfully to {0}.", EmailAddress) });
            }
            else
            {
                return Json(new { result = false, message = "Sending email was unsuccessful." });
            }

        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveEstimatePdf(CreateInvoice Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Invoice tempInvo = _Util.Facade.InvoiceFacade.GetByInvoiceId(Model.Invoice.InvoiceId);
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
            CustomerExtended tempCusExtd = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(tempCUstomer.CustomerId);

            Model.Invoice.Id = tempInvo.Id;
            Model.Invoice.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;
            if (tempCUstomer.BusinessName != "")
            {
                Model.CustomerName = tempCUstomer.BusinessName;
            }
            else
            {
                Model.CustomerName = Model.Invoice.CustomerName;
            }
            //Model.CustomerName = Model.Invoice.CustomerName;
            Model.Invoice.IsEstimate = true;
            Model.Invoice.CreatedBy = tempInvo.CreatedBy;
            //Model.Invoice.CreatedDate = tempInvo.CreatedDate;
            Model.Invoice.CustomerId = tempInvo.CustomerId;
            Model.Invoice.CompanyId = tempInvo.CompanyId;
            Model.Invoice.Status = "";
            Model.Invoice.CreatedDate = tempInvo.CreatedDate;
            Model.Invoice.InstallDate = tempInvo.InstallDate;
            Model.Invoice.EstimateTerm = "";
            Model.Invoice.MonitoringAmount = tempInvo.MonitoringAmount;
            Model.Invoice.ContractTerm = tempInvo.ContractTerm;
            if (!string.IsNullOrWhiteSpace(tempInvo.EstimateTerm) && tempInvo.EstimateTerm != "-1")
                Model.Invoice.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(tempInvo.EstimateTerm);
            Model.Invoice.FinanceCompany = tempInvo.FinanceCompany;
            Model.CusBussinessName = tempCUstomer.BusinessName;
            //if (!Model.Invoice.BalanceDue.HasValue)
            //{
            //    Model.Invoice.BalanceDue = tempInvo.BalanceDue;
            //}
            Model.SubTotal = 0;
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
            Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
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
            if (IsPermitted(UserPermissions.CustomerPermissions.ShowSalesPersonEmail))
            {
                var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(tempInvo.CreatedByUid);
                if (empobj != null)
                {
                    Model.CompanyEmail = empobj.Email;
                }
            }
            else
            {
                Model.CompanyEmail = tempCom.EmailAdress;
            }
            Model.CompanyName = tempCom.CompanyName;
            Model.PhoneNum = tempCom.Phone;
            Model.CompanyWebsite = tempCom.Website;
            Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
            Model.CompanyCity = tempCom.City;
            Model.CompanyState = tempCom.State;
            Model.CompanyZip = tempCom.ZipCode;
            Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
            Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
            Model.CustomerState = tempCUstomer.State;
            Model.CustomerZipCode = tempCUstomer.ZipCode;
            Model.CustomerNo = tempCUstomer.CustomerNo;
            Model.CustomerSSN = tempCUstomer.SSN;
            Model.CustomerDOB = tempCUstomer.DateofBirth.HasValue ? tempCUstomer.DateofBirth.Value : new DateTime();
            Model.CustomerDrivingLicense = tempCusExtd != null ? tempCusExtd.DrivingLicense : "";
            Model.CustomerStreet = tempCUstomer.Street;
            //ViewBag.CompanyId = tempCom.CompanyId.ToString();
            Model.EstimateImage = _Util.Facade.InvoiceFacade.GetAllEstimateImageByInvoiceId(Model.Invoice.InvoiceId);
            Model.Invoice.CompanyId = tempCom.CompanyId;
            Model.InvoiceSetting = new InvoiceSetting();
            string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
            List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CurrentUser.CompanyId.Value);
            foreach (var print in printsetting)
            {
                if (print.Value.ToLower() == "true")
                {
                    if (print.SearchKey == "InvoiceSettingsDeposit")
                    {
                        Model.InvoiceSetting.DepositSetting = true;
                    }
                    if (print.SearchKey == "EstimateServiceSetting")
                    {
                        Model.InvoiceSetting.ServiceSetting = true;
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
            #region show estimate finance company
            bool IsFinance = false;
            GlobalSetting GlobalSet = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("ShowEstimateFinanceCompany", CurrentUser.CompanyId.Value);
            if (GlobalSet != null && !string.IsNullOrWhiteSpace(GlobalSet.Value))
            {
                if (GlobalSet.Value == "true")
                {
                    IsFinance = true;
                }
            }
            #endregion
            ViewBag.ShowFinanceCompany = IsFinance;
            //ViewBag.ShippingValue = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(tempCom.CompanyId);
            Model.ShowInvoiceShippingAddress = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(CurrentUser.CompanyId.Value).ToLower() == "true" ? true : false;
            GlobalSetting monitoringVal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringAmount");
            GlobalSetting contractTerm = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateContractTerm");
            GlobalSetting onitWater = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowONITWaterLLCForEstimate");
            GlobalSetting monitoringDes = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringDescription");
            GlobalSetting thompsonEstimate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowThompsonEstimateText");
            GlobalSetting gutterEqpImg = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowGutterEstimateEquipmentImage");
            if (gutterEqpImg != null)
            {
                Model.ShowGutterEquipmentImage = gutterEqpImg.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowGutterEquipmentImage = false;
            }
            if (thompsonEstimate != null)
            {
                Model.ShowThompsonEstimateText = thompsonEstimate.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowThompsonEstimateText = false;
            }
            if (monitoringDes != null)
            {
                Model.ShowEstimateMonitoringDescription = monitoringDes.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowEstimateMonitoringDescription = false;
            }
            if (monitoringVal != null)
            {
                Model.ShowEstimateMonitoringAmount = monitoringVal.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowEstimateMonitoringAmount = false;
            }
            if (contractTerm != null)
            {
                Model.ShowEstimateContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowEstimateContractTerm = false;
            }
            if (onitWater != null)
            {
                Model.ShowOnitWaterTreatment = onitWater.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowOnitWaterTreatment = false;
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


            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("EstimatePdf", Model)
            {
                //FileName = "TestView.pdf",
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };


            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.EstimateFiles"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            filename = string.Format(filename, comname);
            filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString();
            //string Serverfilename = FileHelper.GetFileFullPath(filename);
            //FileHelper.SaveFile(applicationPDFData, Serverfilename);
            //Session[SessionKeys.EstimatePdfSession] = filename;

            #region AWS S3 Invoice Save
            string FileName = Model.Invoice.InvoiceId + ".pdf";
            string FilePath = filename;
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

            #endregion AWS S3 Invoice Save

            Session[SessionKeys.EstimatePdfSession] = FileKey;

            //return Json(new { result = true, message = "Estimate Successfully Saved", filePath = AppConfig.DomainSitePath + filename });
            return Json(new { result = true, message = "Estimate Successfully Saved", filePath = returnurl });

        }


        public ActionResult GetEstimate(int Id)
        {
            return View();
        }
        [Authorize]
        public PartialViewResult SendEmailEstimate(int Id, string EmailAddress)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateCustomerInvoice model = new CreateCustomerInvoice();
            if (Id == 0)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            Guid companyid = CurrentUser.CompanyId.Value;
            model.Invoice = _Util.Facade.InvoiceFacade.GetInvoiceById(Id);
            if (model.Invoice.CompanyId != companyid)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (model.Invoice == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            model.InvoiceId = model.Invoice.InvoiceId;
            var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.Invoice.CustomerId);
            if (objcus != null)
            {
                model.CustomerName = objcus.FirstName + " " + objcus.LastName;
                model.CustomerEmailAddress = string.IsNullOrWhiteSpace(EmailAddress) ? objcus.EmailAddress : EmailAddress;
                model.CustomerContactNumber = string.IsNullOrWhiteSpace(objcus.CellNo) ? objcus.PrimaryPhone : objcus.CellNo;
                model.CustomerId = objcus.CustomerId;
            }
            else
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var objcom = _Util.Facade.CompanyFacade.GetAllCompanyByCompanyId(CurrentUser.CompanyId.Value).FirstOrDefault();
            if (objcom != null)
            {
                model.CompanyName = objcom.CompanyName;
                model.CompanyEmail = objcom.EmailAdress;
                model.CompanyPhone = objcom.Phone;
            }
            else
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            model.ShortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/");
            if (Session[SessionKeys.EstimatePdfSession] != null)
            {
                ViewBag.pdfLocation = AppConfig.DomainSitePath + "/" + Session[SessionKeys.EstimatePdfSession].ToString();
            }
            model.EmailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("EstimatePredefineEmailTemplate");
            model.SMSBody = string.Concat("New Estimate from", " ", model.CompanyName, ": ", model.Invoice.InvoiceId, Environment.NewLine
                , Environment.NewLine, model.ShortUrl, "##url##");
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

            if (IsPermitted(UserPermissions.CustomerPermissions.SendAttachmentsWithEstimate))
            {
                #region Attachedments
                List<SelectListItem> fileList = new List<SelectListItem>();
                fileList.AddRange(_Util.Facade.CustomerFileFacade.GetAllFilesByCustomerIdAndCompanyId(model.CustomerId, CurrentUser.CompanyId.Value, null).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.FileDescription.ToString(),
                              Value = x.Id.ToString()
                          }).ToList());
                ViewBag.CustomerFileList = fileList.OrderBy(x => x.Text).ToList();
                #endregion
            }

            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(model.CustomerId
                                + "#"
                                + CurrentUser.CompanyId.Value
                                + "#"
                                + model.Invoice.Id);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Estimate/", encryptedurl);
            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, model.CustomerId);
            ViewBag.url = AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code;
            return PartialView("SendEmailEstimate", model);
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

        public JsonResult ConvertEstimateToWorkOrServiceOrder(Guid customerid, string InstallDate, string detailId)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var empid = "00000000-0000-0000-0000-000000000000";
            var WorkAppointID = "00000000-0000-0000-0000-000000000000";
            var ServiceAppointID = "00000000-0000-0000-0000-000000000000";
            var objAppointmentWorkOrder = _Util.Facade.CustomerAppoinmentFacade.GetCustomerWorkOrderbyCustomerIdAndCompanyId(customerid, CurrentUser.CompanyId.Value);
            if (objAppointmentWorkOrder == null)
            {
                CustomerAppointment WorkCustomerAppointment = new CustomerAppointment()
                {
                    AppointmentId = Guid.NewGuid(),
                    CompanyId = CurrentUser.CompanyId.Value,
                    CustomerId = customerid,
                    EmployeeId = new Guid(empid),
                    AppointmentType = AppoinmentType.WorkOrder,
                    AppointmentDate = Convert.ToDateTime(InstallDate),
                    AppointmentStartTime = "",
                    AppointmentEndTime = "",
                    IsAllDay = false,
                    Notes = "",
                    Status = false,
                    CreatedBy = CurrentUser.Identity.Name,
                    LastUpdatedBy = CurrentUser.Identity.Name,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime()
                };
                result = _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(WorkCustomerAppointment) > 0;
                WorkAppointID = WorkCustomerAppointment.AppointmentId.ToString();
                if (result == true)
                {
                    var objEstimateDetail = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(detailId);
                    if (objEstimateDetail.Count > 0)
                    {
                        foreach (var item in objEstimateDetail)
                        {
                            CustomerAppointmentEquipment objOrderEquipment = new CustomerAppointmentEquipment()
                            {
                                AppointmentId = WorkCustomerAppointment.AppointmentId,
                                EquipmentId = item.EquipmentId,
                                Quantity = item.Quantity.Value,
                                UnitPrice = item.UnitPrice.Value,
                                TotalPrice = item.TotalPrice.Value,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CreatedBy = CurrentUser.Identity.Name
                            };
                            _Util.Facade.CustomerAppoinmentFacade.InsertAppointmentEquipment(objOrderEquipment);
                        }
                    }
                }
            }
            else
            {
                CustomerAppointment ServiceCustomerAppointment = new CustomerAppointment()
                {
                    AppointmentId = Guid.NewGuid(),
                    CompanyId = CurrentUser.CompanyId.Value,
                    CustomerId = customerid,
                    EmployeeId = new Guid(empid),
                    AppointmentType = AppoinmentType.ServiceOrder,
                    AppointmentDate = Convert.ToDateTime(InstallDate),
                    AppointmentStartTime = "",
                    AppointmentEndTime = "",
                    IsAllDay = false,
                    Notes = "",
                    Status = false,
                    CreatedBy = CurrentUser.Identity.Name,
                    LastUpdatedBy = CurrentUser.Identity.Name,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime()
                };
                result = _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(ServiceCustomerAppointment) > 0;
                ServiceAppointID = ServiceCustomerAppointment.AppointmentId.ToString();
                if (result == true)
                {
                    var objEstimateDetail = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(detailId);
                    if (objEstimateDetail.Count > 0)
                    {
                        foreach (var item in objEstimateDetail)
                        {
                            CustomerAppointmentEquipment objOrderEquipment = new CustomerAppointmentEquipment()
                            {
                                AppointmentId = ServiceCustomerAppointment.AppointmentId,
                                EquipmentId = item.EquipmentId,
                                Quantity = item.Quantity.Value,
                                UnitPrice = item.UnitPrice.Value,
                                TotalPrice = item.TotalPrice.Value,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CreatedBy = CurrentUser.Identity.Name
                            };
                            _Util.Facade.CustomerAppoinmentFacade.InsertAppointmentEquipment(objOrderEquipment);
                        }
                    }
                }
            }
            if (result == true)
            {
                result = _Util.Facade.InvoiceFacade.DeleteEstimateConvertToOrder(detailId);
            }
            int idcus = 0;
            var cusobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid);
            if (cusobj != null)
            {
                idcus = cusobj.Id;
            }
            return Json(new { result = result, cusid = customerid, ServiceAppointID = ServiceAppointID, WorkAppointID = WorkAppointID, idcus = idcus });
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddEstimateNote(InvoiceNote InvNote)
        {
            InvNote.AddedBy = ((HS.Web.UI.Helper.CustomPrincipal)User).UserId;
            Employee tmpEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(InvNote.AddedBy);

            InvNote.AddedDate = DateTime.Now.UTCCurrentTime();
            InvNote.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value;
            int Id = _Util.Facade.InvoiceNoteFacade.InsertInvoiceNote(InvNote);
            string AddedBy = ((HS.Web.UI.Helper.CustomPrincipal)User).GetFullName();
            string AddedDate = DateTime.Now.UTCCurrentTime().ToString("MM/dd/yyyy hh:mm:ss");
            return Json(new { result = true, Id = Id, AddedDate = AddedDate, Note = InvNote.Note, AddedBy = tmpEmp.FirstName + " " + tmpEmp.LastName, message = "Invoice note inserted successfully." });
        }
        //Email will send to Model.EmailAddress
        private InvoiceEmailSentResponse SendEstimateEmail(CreateInvoice Model, Customer tempCUstomer, Company tempCom, List<int> attachedments)
        {
            var response = new InvoiceEmailSentResponse
            {
                FileLocation = "",
                IsSent = false
            };
            tempCUstomer.PreferedEmail = true;
            _Util.Facade.CustomerFacade.UpdateCustomer(tempCUstomer);
            CustomerExtended tempCusExtd = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(tempCUstomer.CustomerId);
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string filename = "";
            response.IsSent = false;

            #region Estimate PDF Ready
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

            Model.CompanyAddress = MakeAddress(tempCom.Street, tempCom.City, tempCom.State, tempCom.ZipCode, "");
            if (IsPermitted(UserPermissions.CustomerPermissions.ShowSalesPersonEmail))
            {
                var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(Model.Invoice.CreatedByUid);
                if (empobj != null)
                {
                    Model.CompanyEmail = empobj.Email;
                }
            }
            else
            {
                Model.CompanyEmail = tempCom.EmailAdress;
            }
            Model.CompanyName = tempCom.CompanyName;
            Model.CompanyWebsite = tempCom.Website;
            Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            Model.CompanyStreet = tempCom.Street;
            Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
            Model.PhoneNum = tempCom.Phone;
            Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
            Model.CompanyCity = tempCom.City;
            Model.CompanyState = tempCom.State;
            Model.CompanyZip = tempCom.ZipCode;
            Model.EstimateImage = _Util.Facade.InvoiceFacade.GetAllEstimateImageByInvoiceId(Model.Invoice.InvoiceId);
            Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
            Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
            Model.CustomerState = tempCUstomer.State;
            Model.CustomerZipCode = tempCUstomer.ZipCode;
            Model.CustomerNo = tempCUstomer.CustomerNo;
            Model.CustomerSSN = tempCUstomer.SSN;
            Model.CustomerDOB = tempCUstomer.DateofBirth.HasValue ? tempCUstomer.DateofBirth.Value : new DateTime();
            Model.CustomerDrivingLicense = tempCusExtd != null ? tempCusExtd.DrivingLicense : "";
            Model.CustomerStreet = tempCUstomer.Street;
            //ViewBag.ShippingValue = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(tempCom.CompanyId);
            Model.ShowInvoiceShippingAddress = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(CurrentUser.CompanyId.Value).ToLower() == "true" ? true : false;
            //ViewBag.CompanyId = tempCom.CompanyId.ToString();
            Model.Invoice.CompanyId = tempCom.CompanyId;
            Model.InvoiceSetting = new InvoiceSetting();
            string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
            List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CurrentUser.CompanyId.Value);
            foreach (var print in printsetting)
            {
                if (print.Value.ToLower() == "true")
                {
                    if (print.SearchKey == "InvoiceSettingsDeposit")
                    {
                        Model.InvoiceSetting.DepositSetting = true;
                    }
                    if (print.SearchKey == "EstimateServiceSetting")
                    {
                        Model.InvoiceSetting.ServiceSetting = true;
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
            GlobalSetting monitoringVal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringAmount");
            GlobalSetting contractTerm = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateContractTerm");
            GlobalSetting onitWater = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowONITWaterLLCForEstimate");
            GlobalSetting monitoringDes = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringDescription");
            GlobalSetting thompsonEstimate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowThompsonEstimateText");
            GlobalSetting gutterEqpImg = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowGutterEstimateEquipmentImage");
            if (gutterEqpImg != null)
            {
                Model.ShowGutterEquipmentImage = gutterEqpImg.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowGutterEquipmentImage = false;
            }
            if (thompsonEstimate != null)
            {
                Model.ShowThompsonEstimateText = thompsonEstimate.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowThompsonEstimateText = false;
            }
            if (monitoringDes != null)
            {
                Model.ShowEstimateMonitoringDescription = monitoringDes.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowEstimateMonitoringDescription = false;
            }
            if (monitoringVal != null)
            {
                Model.ShowEstimateMonitoringAmount = monitoringVal.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowEstimateMonitoringAmount = false;
            }
            if (contractTerm != null)
            {
                Model.ShowEstimateContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowEstimateContractTerm = false;
            }
            if (onitWater != null)
            {
                Model.ShowOnitWaterTreatment = onitWater.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowOnitWaterTreatment = false;
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
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("EstimatePdf", Model)
            {
                //FileName = "TestView.pdf",
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };

            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Random rand = new Random();
            filename = ConfigurationManager.AppSettings["File.EstimateFiles"];
            string comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            filename = string.Format(filename, comname);
            filename += rand.Next().ToString() + "___" + DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + Model.Invoice.InvoiceId + ".pdf";
            string ServerFileAddress = filename;
            Session[SessionKeys.EstimatePdfSession] = filename;
            filename = FileHelper.GetFileFullPath(filename);
            response.FileLocation = filename;
            FileHelper.SaveFile(applicationPDFData, filename);
            #region Insert Into Customer file
            _Util.Facade.CustomerFileFacade.SaveEstimatePdfFile(AppConfig.DomainSitePath + ServerFileAddress, Model.Invoice.InvoiceId, tempCUstomer.CustomerId, CurrentUser.CompanyId.Value);
            #endregion
            #endregion

            try
            {
                EmailTemplate EmailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey(EmailTemplateKey.PredefinedTemplates.EstimatePredefineEmailTemplate);

                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(tempCUstomer.CustomerId
                                + "#"
                                + CurrentUser.CompanyId.Value
                                + "#"
                                + Model.Invoice.Id);
                string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Estimate/", encryptedurl);
                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, tempCUstomer.CustomerId);
                string SalesGuy = CurrentUser.GetFullName();
                string SalesPhone = string.IsNullOrWhiteSpace(CurrentUser.PhoneNumber) ? tempCom.Phone : CurrentUser.PhoneNumber;
                //CurrentUser.phon
                Hashtable datatemplate = new Hashtable();
                datatemplate.Add("CustomerName", Model.CustomerName);
                datatemplate.Add("ExpirationDate", Model.Invoice.DueDate);
                datatemplate.Add("SalesPhone Number", SalesPhone);
                datatemplate.Add("CompanyName", Model.CompanyName);
                datatemplate.Add("SalesGuy", SalesGuy);
                datatemplate.Add("url", string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code));
                string emailtemplate = HS.Web.UI.Helper.LabelHelper.ParserHelper(EmailTemplate.BodyContent, datatemplate);

                

                string Currency = HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency();
                InvoiceCreatedEmail email = new InvoiceCreatedEmail()
                {
                    CompanyName = tempCom.CompanyName,
                    CustomerName = Model.Invoice.CustomerName,
                    BalanceDue = Model.Invoice.TotalAmount != null ? Currency + Model.Invoice.TotalAmount.Value.ToString("0,0.00") : Currency + "0.00",
                    DueDate = Model.Invoice.DueDate.HasValue ? Model.Invoice.DueDate.Value.ToString("MM/dd/yy") : "",
                    InvoiceId = Model.Invoice.InvoiceId,
                    ToEmail = Model.EmailAddress.Trim(),
                    EmailBody = emailtemplate,/*Model.EmailDescription,*/
                    //Subject = Model.EmailSubject,
                    CustomerId = Model.Invoice.CustomerId.ToString(),
                    EmployeeId = CurrentUser.UserId.ToString(),
                    FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : "info@rmrcloud.com",
                    FromName = CurrentUser.GetFullName(),

                    ccEmail = Model.Invoice.InvoiceCcEmailAddress,
                    InvoicePdf = new Attachment(
                      FileHelper.GetFileFullPath(filename),
                     MediaTypeNames.Application.Octet)
                };
                #region Get CustomerFile by Id
                //List<Attachment> attList = new List<Attachment>();
                try
                {
                    if (attachedments != null && attachedments.Count() > 0)
                    {
                        List<CustomerFile> fileList = _Util.Facade.CustomerFileFacade.GetCustomerFileListById(attachedments);
                        if (fileList != null && fileList.Count() > 0)
                        {
                            email.attachedmentList = new List<Attachment>();
                            foreach (var item in fileList)
                            {
                                string FileName = FileHelper.GetFileFullPath(item.Filename);
                                email.attachedmentList.Add(new Attachment(FileName, MediaTypeNames.Application.Octet));
                            }
                        }
                    }
                }
                catch(Exception ex)
                {

                }
                
                #endregion
                #region Comment
                //if (email.EmailBody.IndexOf("##url##") > -1)
                //{
                //    string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(tempCUstomer.CustomerId
                //        + "#"
                //        + CurrentUser.CompanyId.Value
                //        + "#"
                //        + Model.Invoice.Id);
                //    string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Estimate/", encryptedurl);
                //    ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, tempCUstomer.CustomerId);
                //    email.EmailBody = email.EmailBody.Replace("##url##", ShortUrl.Code);
                //}
                #endregion

                response.IsSent = _Util.Facade.MailFacade.SendEstimateCreatedEmail(email, CurrentUser.CompanyId.Value);
                //email.InvoicePdf.Dispose();
                if (response.IsSent)
                {
                    string empName = "";
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(Model.Invoice.CreatedByUid);
                    if (empobj != null)
                    {
                        empName = empobj.FirstName + " " + empobj.LastName;
                    }
                    CustomerSnapshot objEstimateEmailLog = new CustomerSnapshot()
                    {
                        CustomerId = Model.Invoice.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        Description = "Estimate:" + "  " + Model.Invoice.InvoiceId + " " + "email sent by " + "<b>" + empName + "</b>",
                        Logdate = DateTime.Now.UTCCurrentTime(),
                        Updatedby = CurrentUser.Identity.Name,
                        Type = "CustomerMailHistory"
                    };
                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEstimateEmailLog);

                }
                //EmailSent = _Util.Facade.MailFacade.SendEstimateCreatedEmail(email, CurrentUser.CompanyId.Value);
                //email.InvoicePdf.Dispose();  
                if (response.IsSent)
                {
                    if (Model.Invoice.Status == LabelHelper.EstimateStatus.SentToCustomer)
                    {
                        Model.Invoice.Status = LabelHelper.EstimateStatus.ResendToCustomer;
                    }
                    else
                    {
                        Model.Invoice.Status = LabelHelper.EstimateStatus.SentToCustomer;
                    }
                    _Util.Facade.InvoiceFacade.UpdateInvoice(Model.Invoice);
                }
            }
            catch (Exception ex)
            {

            }
            return response;
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

        public ActionResult GetPdf(string code)
        {
            CreateInvoice CreateInvoice = new CreateInvoice();
            if (!string.IsNullOrWhiteSpace(code))
            {
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                if (Decryptval.Length != 2)
                    return View("~/Views/Shared/_APIAccessDenied.cshtml");
                string invid = Decryptval[0];
                string username = Decryptval[1];
                #region Create Pdf
                HSApiFacade HSApiFacade = new HSApiFacade();
                HSMainApiFacade HSMainApiFacade = new HSMainApiFacade();
                var logcompany = HSMainApiFacade.GetCompanyConnectionByUserName(username);
                if (logcompany == null)
                    return View("~/Views/Shared/_APIAccessDenied.cshtml");
                HSApiFacade = new HSApiFacade(logcompany.ConnectionString);

                Invoice EstimateDetails = HSApiFacade.GetInvoiceByInvoiceId(invid);
                CreateInvoice.ConStr = logcompany.ConnectionString;
                CreateInvoice.CurrencyType = "API";
                if (EstimateDetails != null)
                {
                    ViewBag.UserName = username;
                    CreateInvoice.InvoiceDetailList = HSApiFacade.GetInvoiceDetialsListByInvoiceId(EstimateDetails.InvoiceId);
                    Company company = HSApiFacade.GetCompanyByCompanyId(EstimateDetails.CompanyId);
                    CreateInvoice.Company = company;
                    Customer customer = HSApiFacade.GetCustomerByCustomerId(EstimateDetails.CustomerId);
                    CustomerExtended tempCusExtd = new CustomerExtended();
                    if (customer != null)
                    {
                       tempCusExtd = HSApiFacade.GetCustomerExtendedByCustomerId(customer.CustomerId);
                    }
                    CreateInvoice.Customer = customer;
                    CreateInvoice.CustomerInspection = HSApiFacade.GetCustomerInspectionValueByCustomerIdAndCompanyId(customer.CustomerId, company.CompanyId);
                    CreateInvoice.Invoice = EstimateDetails;
                    CreateInvoice.Invoice.EstimateTerm = "";
                    if (!string.IsNullOrWhiteSpace(EstimateDetails.EstimateTerm) && EstimateDetails.EstimateTerm != "-1")
                        CreateInvoice.Invoice.EstimateTerm = HSApiFacade.GetDisplayTextByDataValueFromLLookup(EstimateDetails.EstimateTerm, EstimateDetails.CompanyId);
                    CreateInvoice.CompanyAddress = company.Address;
                    CreateInvoice.CompanyStreet = company.Street;
                    string ComCity = "";
                    string ComState = "";
                    if (!string.IsNullOrWhiteSpace(company.City))
                    {
                        ComCity = company.City.UppercaseFirst() + ", ";
                    }
                    if (!string.IsNullOrWhiteSpace(company.State))
                    {
                        ComState = company.State + " ";
                    }
                    CreateInvoice.EstimateImage = HSApiFacade.GetAllEstimateImageByInvoiceId(EstimateDetails.InvoiceId);
                    CreateInvoice.companyStreetInfo = ComCity + ComState + company.ZipCode;
                    CreateInvoice.CompanyEmail = company.EmailAdress;
                    CreateInvoice.CompanyName = company.CompanyName;
                    CreateInvoice.PhoneNum = company.Phone;
                    CreateInvoice.CompanyWebsite = company.Website;
                    CreateInvoice.CompanyInfo = HSApiFacade.GetCompanyAddressFormat(EstimateDetails.CompanyId);
                    CreateInvoice.CompanyCity = company.City;
                    CreateInvoice.CompanyState = company.State;
                    CreateInvoice.CompanyZip = company.ZipCode;
                    CreateInvoice.CompanyLogo = HSApiFacade.GetCompanyLogoForPDFByCompanyId(EstimateDetails.CompanyId);
                    CreateInvoice.CustomerInfo = HSApiFacade.GetCustomerAddressFormat(EstimateDetails.CompanyId);
                    CreateInvoice.CustomerCity = customer.City.UppercaseFirst();
                    CreateInvoice.CustomerState = customer.State;
                    CreateInvoice.CustomerZipCode = customer.ZipCode;
                    CreateInvoice.CustomerNo = customer.CustomerNo;
                    CreateInvoice.CustomerSSN = customer.SSN;
                    CreateInvoice.CustomerDOB = customer.DateofBirth.HasValue ? customer.DateofBirth.Value : new DateTime();
                    CreateInvoice.CustomerDrivingLicense = tempCusExtd != null ? tempCusExtd.DrivingLicense : "";
                    CreateInvoice.CustomerStreet = customer.Street;
                    CreateInvoice.ShowInvoiceShippingAddress = HSApiFacade.GetShippingSettingCompanyId(EstimateDetails.CompanyId).ToLower() == "true" ? true : false;
                    CreateInvoice.Invoice.CompanyId = company.CompanyId;
                    CreateInvoice.InvoiceSetting = new InvoiceSetting();
                    string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                    List<GlobalSetting> printsetting = HSApiFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, EstimateDetails.CompanyId);
                    foreach (var print in printsetting)
                    {
                        if (print.Value.ToLower() == "true")
                        {
                            if (print.SearchKey == "InvoiceSettingsDeposit")
                            {
                                CreateInvoice.InvoiceSetting.DepositSetting = true;
                            }
                            if (print.SearchKey == "EstimateServiceSetting")
                            {
                                CreateInvoice.InvoiceSetting.ServiceSetting = true;
                            }
                            if (print.SearchKey == "InvoiceSettingsDiscount")
                            {
                                CreateInvoice.InvoiceSetting.DiscountSetting = true;
                            }
                            if (print.SearchKey == "InvoiceSettingsShipping")
                            {
                                CreateInvoice.InvoiceSetting.ShippingSetting = true;
                            }
                        }
                    }
                    CreateInvoice.SubTotal = 0;
                    List<InvoiceDetail> InvoDetailList = CreateInvoice.InvoiceDetailList.OrderBy(x => x.Order).ToList();
                    foreach (var item in InvoDetailList)
                    {
                        CreateInvoice.SubTotal = CreateInvoice.SubTotal + item.TotalPrice.Value;
                    }

                    if (EstimateDetails.Discountpercent != null)
                    {
                        CreateInvoice.Discount = ((EstimateDetails.Discountpercent * CreateInvoice.SubTotal) / 100).Value;
                    }
                    GlobalSetting monitoringVal = HSApiFacade.GetGlobalSettingsByKey(CreateInvoice.Invoice.CompanyId, "EstimateMonitoringAmount");
                    GlobalSetting contractTerm = HSApiFacade.GetGlobalSettingsByKey(CreateInvoice.Invoice.CompanyId, "EstimateContractTerm");
                    GlobalSetting onitWater = HSApiFacade.GetGlobalSettingsByKey(CreateInvoice.Invoice.CompanyId, "ShowONITWaterLLCForEstimate");
                    GlobalSetting monitoringDes = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CreateInvoice.Invoice.CompanyId, "EstimateMonitoringDescription");
                    GlobalSetting thompsonEstimate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CreateInvoice.Invoice.CompanyId, "ShowThompsonEstimateText");
                    GlobalSetting gutterEqpImg = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CreateInvoice.Invoice.CompanyId, "ShowGutterEstimateEquipmentImage");
                    if (gutterEqpImg != null)
                    {
                        CreateInvoice.ShowGutterEquipmentImage = gutterEqpImg.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        CreateInvoice.ShowGutterEquipmentImage = false;
                    }
                    if (thompsonEstimate != null)
                    {
                        CreateInvoice.ShowThompsonEstimateText = thompsonEstimate.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        CreateInvoice.ShowThompsonEstimateText = false;
                    }
                    if (monitoringDes != null)
                    {
                        CreateInvoice.ShowEstimateMonitoringDescription = monitoringDes.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        CreateInvoice.ShowEstimateMonitoringDescription = false;
                    }
                    if (monitoringVal != null)
                    {
                        CreateInvoice.ShowEstimateMonitoringAmount = monitoringVal.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        CreateInvoice.ShowEstimateMonitoringAmount = false;
                    }
                    if (contractTerm != null)
                    {
                        CreateInvoice.ShowEstimateContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        CreateInvoice.ShowEstimateContractTerm = false;
                    }
                    if (onitWater != null)
                    {
                        CreateInvoice.ShowOnitWaterTreatment = onitWater.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        CreateInvoice.ShowOnitWaterTreatment = false;
                    }
                    return new Rotativa.ViewAsPdf("EstimatePdf", CreateInvoice)
                    {
                        PageSize = Size.A4,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 },

                    };
                }
                #endregion Create Pdf
                else
                {
                    return View("~/Views/Shared/_APIAccessDenied.cshtml");
                }
            }
            else
            {
                return View("~/Views/Shared/_APIAccessDenied.cshtml");
            }
        }

        public bool SendEmailEstimatePdfAPI(string code)
        {
            CreateInvoice CreateInvoice = new CreateInvoice();
            if (!string.IsNullOrWhiteSpace(code))
            {
                Guid userid = new Guid();
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split(new[] { "##" }, StringSplitOptions.None);
                if (Decryptval.Length != 6)
                    return false;
                string invid = Decryptval[0];
                string username = Decryptval[1];
                string toemail = Decryptval[2];
                string ccmail = Decryptval[3];
                Guid.TryParse(Decryptval[4], out userid);
                string[] imageurl = Decryptval[5].Split(",");
                string body = "";
                string subject = "";

                List<string> ImageUrlList = new List<string>();
                if(imageurl != null && imageurl.Length > 0)
                {
                    foreach (var item in imageurl)
                    {
                        ImageUrlList.Add(item);
                    }
                }
                
                #region Create Pdf
                HSApiFacade HSApiFacade = new HSApiFacade();
                HSMainApiFacade HSMainApiFacade = new HSMainApiFacade();
                var logcompany = HSMainApiFacade.GetCompanyConnectionByUserName(username);
                if (logcompany == null)
                    return false;
                HSApiFacade = new HSApiFacade(logcompany.ConnectionString);

                Invoice EstimateDetails = HSApiFacade.GetInvoiceByInvoiceId(invid);
                CreateInvoice.ConStr = logcompany.ConnectionString;
                CreateInvoice.CurrencyType = "API";
                if (EstimateDetails != null)
                {
                    ViewBag.UserName = username;
                    Employee emp = HSApiFacade.GetEmployeeByUserId(userid);
                    CreateInvoice.InvoiceDetailList = HSApiFacade.GetInvoiceDetialsListByInvoiceId(EstimateDetails.InvoiceId);
                    Company company = HSApiFacade.GetCompanyByCompanyId(EstimateDetails.CompanyId);
                    CreateInvoice.Company = company;
                    Customer customer = HSApiFacade.GetCustomerByCustomerId(EstimateDetails.CustomerId);
                    CustomerExtended tempCusExtd = new CustomerExtended();
                    if (customer!=null)
                    {
                       tempCusExtd = HSApiFacade.GetCustomerExtendedByCustomerId(customer.CustomerId);
                    }
                    
                    CreateInvoice.Customer = customer;
                    CreateInvoice.CustomerInspection = HSApiFacade.GetCustomerInspectionValueByCustomerIdAndCompanyId(customer.CustomerId, company.CompanyId);
                    CreateInvoice.Invoice = EstimateDetails;
                    CreateInvoice.Invoice.EstimateTerm = "";
                    if (!string.IsNullOrWhiteSpace(EstimateDetails.EstimateTerm) && EstimateDetails.EstimateTerm != "-1")
                        CreateInvoice.Invoice.EstimateTerm = HSApiFacade.GetDisplayTextByDataValueFromLLookup(EstimateDetails.EstimateTerm, EstimateDetails.CompanyId);
                    CreateInvoice.CompanyAddress = company.Address;
                    CreateInvoice.CompanyStreet = company.Street;
                    string ComCity = "";
                    string ComState = "";
                    if (!string.IsNullOrWhiteSpace(company.City))
                    {
                        ComCity = company.City.UppercaseFirst() + ", ";
                    }
                    if (!string.IsNullOrWhiteSpace(company.State))
                    {
                        ComState = company.State + " ";
                    }
                    CreateInvoice.EstimateImage = HSApiFacade.GetAllEstimateImageByInvoiceId(EstimateDetails.InvoiceId);
                    CreateInvoice.companyStreetInfo = ComCity + ComState + company.ZipCode;
                    CreateInvoice.CompanyEmail = company.EmailAdress;
                    CreateInvoice.CompanyName = company.CompanyName;
                    CreateInvoice.PhoneNum = company.Phone;
                    CreateInvoice.CompanyWebsite = company.Website;
                    CreateInvoice.CompanyInfo = HSApiFacade.GetCompanyAddressFormat(EstimateDetails.CompanyId);
                    CreateInvoice.CompanyCity = company.City;
                    CreateInvoice.CompanyState = company.State;
                    CreateInvoice.CompanyZip = company.ZipCode;
                    CreateInvoice.CompanyLogo = HSApiFacade.GetCompanyLogoForPDFByCompanyId(EstimateDetails.CompanyId);
                    CreateInvoice.CustomerInfo = HSApiFacade.GetCustomerAddressFormat(EstimateDetails.CompanyId);
                    CreateInvoice.CustomerCity = customer.City.UppercaseFirst();
                    CreateInvoice.CustomerState = customer.State;
                    CreateInvoice.CustomerZipCode = customer.ZipCode;
                    CreateInvoice.CustomerNo = customer.CustomerNo;
                    CreateInvoice.CustomerSSN = customer.SSN;
                    CreateInvoice.CustomerDOB = customer.DateofBirth.HasValue ? customer.DateofBirth.Value : new DateTime();
                    CreateInvoice.CustomerDrivingLicense = tempCusExtd != null ? tempCusExtd.DrivingLicense : "";
                    CreateInvoice.CustomerStreet = customer.Street;
                    CreateInvoice.ShowInvoiceShippingAddress = HSApiFacade.GetShippingSettingCompanyId(EstimateDetails.CompanyId).ToLower() == "true" ? true : false;
                    CreateInvoice.Invoice.CompanyId = company.CompanyId;
                    CreateInvoice.InvoiceSetting = new InvoiceSetting();
                    string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                    List<GlobalSetting> printsetting = HSApiFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, EstimateDetails.CompanyId);
                    foreach (var print in printsetting)
                    {
                        if (print.Value.ToLower() == "true")
                        {
                            if (print.SearchKey == "InvoiceSettingsDeposit")
                            {
                                CreateInvoice.InvoiceSetting.DepositSetting = true;
                            }
                            if (print.SearchKey == "EstimateServiceSetting")
                            {
                                CreateInvoice.InvoiceSetting.ServiceSetting = true;
                            }
                            if (print.SearchKey == "InvoiceSettingsDiscount")
                            {
                                CreateInvoice.InvoiceSetting.DiscountSetting = true;
                            }
                            if (print.SearchKey == "InvoiceSettingsShipping")
                            {
                                CreateInvoice.InvoiceSetting.ShippingSetting = true;
                            }
                        }
                    }
                    CreateInvoice.SubTotal = 0;
                    List<InvoiceDetail> InvoDetailList = CreateInvoice.InvoiceDetailList.OrderBy(x => x.Order).ToList();
                    foreach (var item in InvoDetailList)
                    {
                        CreateInvoice.SubTotal = CreateInvoice.SubTotal + item.TotalPrice.Value;
                    }

                    if (EstimateDetails.Discountpercent != null)
                    {
                        CreateInvoice.Discount = ((EstimateDetails.Discountpercent * CreateInvoice.SubTotal) / 100).Value;
                    }
                    GlobalSetting monitoringVal = HSApiFacade.GetGlobalSettingsByKey(CreateInvoice.Invoice.CompanyId, "EstimateMonitoringAmount");
                    GlobalSetting contractTerm = HSApiFacade.GetGlobalSettingsByKey(CreateInvoice.Invoice.CompanyId, "EstimateContractTerm");
                    GlobalSetting onitWater = HSApiFacade.GetGlobalSettingsByKey(CreateInvoice.Invoice.CompanyId, "ShowONITWaterLLCForEstimate");
                    GlobalSetting monitoringDes = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CreateInvoice.Invoice.CompanyId, "EstimateMonitoringDescription");
                    GlobalSetting thompsonEstimate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CreateInvoice.Invoice.CompanyId, "ShowThompsonEstimateText");
                    GlobalSetting gutterEqpImg = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CreateInvoice.Invoice.CompanyId, "ShowGutterEstimateEquipmentImage");
                    if (gutterEqpImg != null)
                    {
                        CreateInvoice.ShowGutterEquipmentImage = gutterEqpImg.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        CreateInvoice.ShowGutterEquipmentImage = false;
                    }
                    if (thompsonEstimate != null)
                    {
                        CreateInvoice.ShowThompsonEstimateText = thompsonEstimate.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        CreateInvoice.ShowThompsonEstimateText = false;
                    }
                    if (monitoringDes != null)
                    {
                        CreateInvoice.ShowEstimateMonitoringDescription = monitoringDes.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        CreateInvoice.ShowEstimateMonitoringDescription = false;
                    }
                    if (monitoringVal != null)
                    {
                        CreateInvoice.ShowEstimateMonitoringAmount = monitoringVal.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        CreateInvoice.ShowEstimateMonitoringAmount = false;
                    }
                    if (contractTerm != null)
                    {
                        CreateInvoice.ShowEstimateContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        CreateInvoice.ShowEstimateContractTerm = false;
                    }
                    if (onitWater != null)
                    {
                        CreateInvoice.ShowOnitWaterTreatment = onitWater.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        CreateInvoice.ShowOnitWaterTreatment = false;
                    }
                    GlobalSetting invCode3Sta = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CreateInvoice.Invoice.CompanyId, "ShowCode3InvoiceStaticBox");
                    if (invCode3Sta != null)
                    {
                        CreateInvoice.ShowCode3InvoiceStaticBox = invCode3Sta.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        CreateInvoice.ShowCode3InvoiceStaticBox = false;
                    }
                    ViewAsPdf actionpdf = new Rotativa.ViewAsPdf("EstimatePdf", CreateInvoice)
                    {
                        PageSize = Size.A4,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 },

                    };
                    byte[] applicationPDFData = actionpdf.BuildPdf(ControllerContext); // View Not Found
                    Random rand = new Random();

                    string filename = ConfigurationManager.AppSettings["File.EstimateFiles"];
                    var comname = HSApiFacade.GetCompanyByCompanyId(EstimateDetails.CompanyId).CompanyName.ReplaceSpecialChar();
                    filename = string.Format(filename, comname);
                    filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + EstimateDetails.InvoiceId + ".pdf";
                    string Serverfilename = FileHelper.GetFileFullPath(filename);
                    FileHelper.SaveFile(applicationPDFData, filename);
                    var predefinedemailtempobj = HSApiFacade.GetEmailTemplateByTemplateKeyAndCompanyId(company.CompanyId, "EstimatePredefineEmailTemplate");
                    if (predefinedemailtempobj != null)
                    {
                        subject = "New Estimate from " + company.CompanyName + " " + EstimateDetails.InvoiceId;
                        body = "<p><span data-sheets-value=\"{&quot;1&quot;:2,&quot;2&quot;:&quot;Dear " + customer.FirstName + " " + customer.LastName + ",\\n\\nWe appreciate the opportunity to give you an estimate on our products and services. Please click on the link below to review and approve the estimate. \\n\\n" + AppConfig.SiteDomain + "/" + filename + " \\n\\nThis estimate is valid until " + (EstimateDetails.DueDate.HasValue ? EstimateDetails.DueDate.Value.ToString("MM/dd/yy") : "") + ". If you have any questions, please call me directly at " + emp.Phone + ". \\n\\nThank you for your business,\\n" + emp.FirstName + " " + emp.LastName + "\\n" + company.CompanyName + "&quot;}\" data-sheets-userformat=\"{&quot;2&quot;:4480,&quot;10&quot;:2,&quot;11&quot;:4,&quot;15&quot;:&quot;arial,sans,sans-serif&quot;}\"><span style=\"font-weight:600;\">Dear " + customer.FirstName + " " + customer.LastName + "</span>,<br /><br />We appreciate the opportunity to give you an estimate on our products and services. Please click on the link below to review and approve the estimate. <br /><br /><a style=\"line-height:2.6666666666666665rem;background-color:#2ca01c;color:#333;border:1px solid #d6d6d6;-webkit-transition:background-color .2s ease-in;transition:background-color .2s ease-in;font-size:1rem;font-family:'Open Sans',Helvetica,Arial,sans-serif;font-weight:400;display:block;height:2.8rem;-webkit-box-sizing:border-box;-moz-box-sizing: border-box;-o-box-sizing:border-box;-ms-box-sizing:border-box;box-sizing:border-box;-webkit-border-radius:4px;-moz-border-radius:4px;border-radius:4px;-moz-background-clip:padding;-webkit-background-clip:padding-box;background-clip:padding-box;text-align:center;vertical-align:middle;margin:0 auto 11px;padding:0 15px;white-space:nowrap;cursor:pointer;-webkit-user-select:none;-moz-user-select:none; -ms-user-select: none; -o-user-select: none; width: 100%; max-width: 165px; text-decoration:none; color:white; font-weight:600;\" href=\"" + AppConfig.SiteDomain + "/" + filename + "\">View Estimate</a><br /><br />This estimate is valid until " + EstimateDetails.DueDate + ". If you have any questions, please call me directly at " + emp.Phone + ". <br /><br />Thank you for your business,<br />" + emp.FirstName + " " + emp.LastName + "<br />" + company.CompanyName + "</span></p>  ";
                    }
                    InvoiceCreatedEmail email = new InvoiceCreatedEmail()
                    {
                        CompanyName = company.CompanyName,
                        CustomerName = customer.FirstName + " " + customer.LastName,
                        BalanceDue = EstimateDetails.TotalAmount != null ? LabelHelper.CurrentTransMakeCurrency.MakeCurrencyAPI(EstimateDetails.CompanyId, CreateInvoice.ConStr) + EstimateDetails.TotalAmount.Value.ToString("0,0.00") : LabelHelper.CurrentTransMakeCurrency.MakeCurrencyAPI(EstimateDetails.CompanyId, CreateInvoice.ConStr) + "0.00",
                        DueDate = EstimateDetails.DueDate.HasValue ? EstimateDetails.DueDate.Value.ToString("MM/dd/yy") : "",
                        InvoiceId = EstimateDetails.InvoiceId,
                        ToEmail = toemail,
                        EmailBody = body,/*Model.EmailDescription,*/
                        Subject = subject,
                        CustomerId = customer.CustomerId.ToString(),
                        EmployeeId = emp.UserId.ToString(),
                        FromEmail = emp.Email,
                        FromName = emp.FirstName + " " + emp.LastName,
                        ccEmail = ccmail,
                        ListImageUrl = ImageUrlList,
                        InvoicePdf = new Attachment(
                        Serverfilename,
                        MediaTypeNames.Application.Octet)
                    };
                    HSApiFacade.SendEstimateCreatedEmail(email, EstimateDetails.CompanyId);
                    return true;
                }
                #endregion Create Pdf
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}