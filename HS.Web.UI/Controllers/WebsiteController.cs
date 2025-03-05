using HS.Entities;
using Rotativa;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
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
using Forte.Entities;
using Forte;
using System.Globalization;
using HS.Kickbox.Models;
using System.Net.Http;
using HS.Entities.Custom;
using System.Xml.Linq;
using System.Text;
using System.Diagnostics;
using HS.Web.UI.Models;
using Plivo;
using Plivo.Exception;
using HS.Facade;

namespace HS.Web.UI.Controllers
{
    public class WebsiteController : BaseController
    {
        [Authorize]
        // GET: Customer
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

        public PartialViewResult LoadSiteConfigurationPartial(int? PageNo, int? PageSize, string SearchText, string order)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!PageNo.HasValue || PageNo.Value < 1)
            {
                PageNo = 1;
            }
            if (SearchText == "undefined" || SearchText == null)
            {
                SearchText = "";
            }
            if (!PageSize.HasValue || PageSize.Value < 1)
            {
                PageSize = 50;
            }

            List<WebsiteConfiguration> Model = _Util.Facade.MenuFacade.GetAllWebsiteConfig(CurrentUser.CompanyId.Value, PageNo.Value, PageSize.Value, SearchText, order);
            var objCount = _Util.Facade.MenuFacade.GetAllSiteConfig();
            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = order;

            if (objCount.Count() > 0)
            {
                ViewBag.OutOfNumber = objCount.Count();
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
            return PartialView("_SiteConfigurationPartial", Model);
        }

        public ActionResult Website()
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.QuickMenuTitleSetting))
            {
                return View("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            return View();
        }
        public PartialViewResult LoadLocationsPartial(int? PageNo, int? PageSize, string SearchText, string order)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!PageNo.HasValue || PageNo.Value < 1)
            {
                PageNo = 1;
            }
            if (SearchText == "undefined" || SearchText == null)
            {
                SearchText = "";
            }
            if (!PageSize.HasValue || PageSize.Value < 1)
            {
                PageSize = 50;
            }

            List<WebsiteLocation> Model = _Util.Facade.MenuFacade.GetAllWebsiteLocation(CurrentUser.CompanyId.Value, PageNo.Value, PageSize.Value, SearchText, order);
            var objCount = _Util.Facade.MenuFacade.GetAllSiteLoc(CurrentUser.CompanyId.Value, SearchText);
            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = order;
            ViewBag.searchtext = SearchText;
            TempData["data"] = SearchText;
            if (objCount.Count() > 0)
            {
                ViewBag.OutOfNumber = objCount.Count();
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
            return PartialView("_LocationsPartial", Model);
        }
        public PartialViewResult LoadContentPartial(int? PageNo, int? PageSize, string SearchText, string order)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!PageNo.HasValue || PageNo.Value < 1)
            {
                PageNo = 1;
            }
            if (SearchText == "undefined" || SearchText == null)
            {
                SearchText = "";
            }
            if (!PageSize.HasValue || PageSize.Value < 1)
            {
                PageSize = 50;
            }

            List<Seo> Model = _Util.Facade.MenuFacade.GetAllSiteContent(CurrentUser.CompanyId.Value, PageNo.Value, PageSize.Value, SearchText, order);
            var objCount = _Util.Facade.MenuFacade.GetAllSeo();
            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = order;
            TempData["data"] = SearchText;

            if (objCount.Count() > 0)
            {
                ViewBag.OutOfNumber = objCount.Count();
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
            return PartialView("_ContentPartial", Model);
        }

        [Authorize]
        public ActionResult AddSiteConfiguration(int? id)
        {
            WebsiteConfiguration model;
            if (id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.MenuFacade.GetWebsiteConfigurationById(id.Value);
            }
            else
            {
                model = new WebsiteConfiguration();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult AddSiteConfiguration(WebsiteConfiguration web)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            web.CompanyId = CurrentUser.CompanyId.Value;
            web.IsEmail = true;
            if (web.Id > 0)
            {
                var objsiteconfig = _Util.Facade.MenuFacade.GetWebsiteConfigurationById(web.Id);
                objsiteconfig.SiteName = web.SiteName;
                objsiteconfig.DomainName = web.DomainName;
                objsiteconfig.Phone = web.Phone;
                objsiteconfig.ThemeLoc = web.ThemeLoc;
                result = _Util.Facade.MenuFacade.UpdateWebsiteConfiguration(objsiteconfig);
            }
            else
            {
                web.CreatedBy = CurrentUser.UserId;
                web.CreatedDate = DateTime.Now.UTCCurrentTime();
                result = _Util.Facade.MenuFacade.InsertWebsiteConfiguration(web) > 0;
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult AddSiteLocation(int? id)
        {
            WebsiteLocation model;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.MenuFacade.GetSiteLocationById(id.Value);
                if (!string.IsNullOrWhiteSpace(model.DaysAvailable))
                {
                    model.DaysAvailable = model.DaysAvailable.Replace(", ", ",");
                    var spadays = model.DaysAvailable.Split(',');
                    model.AvailableDays = spadays.ToList();
                }
                List<string> selectedcartopt = new List<string>();
                if (!string.IsNullOrWhiteSpace(model.CartOption) && model.CartOption != "null")
                {
                    var spcart = model.CartOption.Split(',');
                    if(spcart.Length > 0)
                    {
                        foreach(var item in spcart)
                        {
                            selectedcartopt.Add(item);
                        }
                    }
                }
                ViewBag.selectedcartopt = selectedcartopt;
                List<string> selectedcuisine = new List<string>();
                if (!string.IsNullOrWhiteSpace(model.CuisineType) && model.CuisineType != "null")
                {
                    var spcuisine = model.CuisineType.Split(',');
                    if (spcuisine.Length > 0)
                    {
                        foreach (var item in spcuisine)
                        {
                            selectedcuisine.Add(item);
                        }
                    }
                }
                ViewBag.selectedcuisine = selectedcuisine;
                List<string> selectedpayment = new List<string>();
                if (!string.IsNullOrWhiteSpace(model.PaymentOption) && model.PaymentOption != "null")
                {
                    var sppayment = model.PaymentOption.Split(',');
                    if (sppayment.Length > 0)
                    {
                        foreach (var item in sppayment)
                        {
                            selectedpayment.Add(item);
                        }
                    }
                }
                ViewBag.selectedpayment = selectedpayment;
                if (model.PreparationTime == null || model.PreparationTime == 0)
                {
                    model.PreparationTime = _Util.Facade.GlobalSettingsFacade.GetMinimumPrepTimeByCompanyId(CurrentUser.CompanyId.Value);
                }
                if (model.MinimumDeliveryTime == null || model.MinimumDeliveryTime == 0)
                {
                    model.MinimumDeliveryTime = _Util.Facade.GlobalSettingsFacade.GetMinimumPrepTimeByCompanyId(CurrentUser.CompanyId.Value);
                }
                if (string.IsNullOrWhiteSpace(model.ExpireTime))
                {
                    model.ExpireTime = _Util.Facade.GlobalSettingsFacade.GetExpireTimeByCompanyId(CurrentUser.CompanyId.Value);
                }
                List<string> listneigh = _Util.Facade.MenuFacade.GetAllNeighbarhoodByLocationId(model.Id).Select(x => x.NeighborhoodName).ToList();
                model.Neighbarhood = string.Join(",", listneigh);
            }
            else
            {
                model = new WebsiteLocation();
                model.PreparationTime = _Util.Facade.GlobalSettingsFacade.GetMinimumPrepTimeByCompanyId(CurrentUser.CompanyId.Value);
                model.MinimumDeliveryTime = _Util.Facade.GlobalSettingsFacade.GetMinimumPrepTimeByCompanyId(CurrentUser.CompanyId.Value);
                model.ExpireTime = _Util.Facade.GlobalSettingsFacade.GetExpireTimeByCompanyId(CurrentUser.CompanyId.Value);
                var objsystemsetting = _Util.Facade.MenuFacade.GetResturantSystemSettingByCompanyId(CurrentUser.CompanyId.Value);
                if(objsystemsetting != null)
                {
                    model.TaxPercentage = objsystemsetting.TaxRate;
                }
                model.PaidOption = "Paid0";
            }
            List<SelectListItem> primarycontact = new List<SelectListItem>();
            primarycontact.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            primarycontact.AddRange(_Util.Facade.MenuFacade.GetAllSiteContact(CurrentUser.CompanyId.Value, "").Select(x => new SelectListItem()
            {
                Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                Value = x.ContactId.ToString()
            }).ToList());
            ViewBag.primarycontact = primarycontact;
            var mintime = _Util.Facade.GlobalSettingsFacade.GetMenuTimeAvailableStartTime(CurrentUser.CompanyId.Value);
            var maxtime = _Util.Facade.GlobalSettingsFacade.GetMenuTimeAvailableEndTime(CurrentUser.CompanyId.Value);
            List<SelectListItem> timeAvailable = new List<SelectListItem>();
            timeAvailable.AddRange(_Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", mintime, maxtime).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.timeAvailable = timeAvailable;
            List<Lookup> allDays = _Util.Facade.LookupFacade.GetLookupByKey("DaysAvailable");
            List<string> selectedDayList = new List<string>();
            List<Lookup> selectDropdown = allDays;
            if (!string.IsNullOrWhiteSpace(model.DaysAvailable) && model.DaysAvailable != null)
            {
                selectedDayList = model.DaysAvailable.Split(',').ToList();
                selectDropdown = allDays.Where(x => selectedDayList.Contains(x.DataValue)).ToList();
            }

            ViewBag.DaysAvailable = allDays.Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString(),
                             Selected = selectDropdown.Count(y => y.DataValue == x.DataValue) > 0
                         }).ToList();
            List<string> dayavailable = new List<string>();
            foreach (var item in (List<SelectListItem>)ViewBag.DaysAvailable)
            {
                dayavailable.Add(item.Value);
            }
            ViewBag.dayavailable = dayavailable;
            List<SelectListItem> cartoption = new List<SelectListItem>();
            cartoption.Add(new SelectListItem() {
                Text = "Pickup",
                Value = "pickup"
            });
            if(!string.IsNullOrWhiteSpace(model.PaidOption) && model.PaidOption.ToLower() != "unpaid" && model.PaidOption.ToLower() != "paid0")
            {
                cartoption.Add(new SelectListItem()
                {
                    Text = "Delivery",
                    Value = "delivery"
                });
            }
            ViewBag.CartOption = cartoption;
            ViewBag.CuisineType = _Util.Facade.LookupFacade.GetLookupByKey("CuisinesType").OrderBy(x => x.DisplayText).Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            List<SelectListItem> paidoption = new List<SelectListItem>();
            paidoption.AddRange(_Util.Facade.LookupFacade.GetLookUpByKey("RestaurantPaidStatus").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.paidoption = paidoption;
            List<SelectListItem> paymentopt = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(model.PaidOption) && model.PaidOption.ToLower() != "unpaid" && model.PaidOption.ToLower() != "paid0")
            {
                paymentopt.AddRange(_Util.Facade.LookupFacade.GetLookUpByKey("IeateryPaymentOption").Select(x => new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList());
            }
            else
            {
                paymentopt.AddRange(_Util.Facade.LookupFacade.GetLookUpByKey("IeateryPaymentOption").Where(x => x.DataValue != "CC").Select(x => new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList());
            }
            ViewBag.paymentopt = paymentopt;
            List<string> neighname = new List<string>();
            var objresneigh = _Util.Facade.MenuFacade.GetAllNeighbarhoodByKey(null);
            if(objresneigh != null && objresneigh.Count > 0)
            {
                foreach(var item in objresneigh)
                {
                    neighname.Add(item.NeighborhoodName);
                }
            }
            ViewBag.neighname = neighname;
            return View(model);
        }

        [HttpPost]
        public JsonResult AddSiteLocation(WebsiteLocation wl)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string GoogleMapAPIKey = _Util.Facade.GlobalSettingsFacade.GetGoogleMapAPIKeyByCompanyId(CurrentUser.CompanyId.Value);
            
            bool result = false;
            bool isnew = false;
            Guid companyid = new Guid();
            if(wl.NewLocation == true)
            {
                companyid = Guid.NewGuid();
            }
            else
            {
                companyid = CurrentUser.CompanyId.Value;
            }
            if (!string.IsNullOrWhiteSpace(wl.DaysAvailable))
            {
                wl.DaysAvailable = wl.DaysAvailable.Replace(",", ", ");
            }
            if(wl.TaxPercentage > 0)
            {
                wl.IsTax = true;
            }
            else
            {
                wl.IsTax = false;
            }
            if(!string.IsNullOrWhiteSpace(wl.DomainName) && wl.DomainName.IndexOf("http") < 0)
            {
                wl.DomainName = "http://" + wl.DomainName;
            }
            if (wl.Id > 0)
            {
                var objwebloc = _Util.Facade.MenuFacade.GetSiteLocationById(wl.Id);
                if ((string.IsNullOrWhiteSpace(objwebloc.OperationStartTime) || objwebloc.OperationStartTime == "-1") && (string.IsNullOrWhiteSpace(objwebloc.OperationEndTime) || objwebloc.OperationEndTime == "-1"))
                {
                    isnew = true;
                }
                objwebloc.Name = wl.Name;
                objwebloc.Address = wl.Address;
                objwebloc.Address2 = wl.Address2;
                objwebloc.City = wl.City;
                objwebloc.State = wl.State;
                objwebloc.Zipcode = wl.Zipcode;
                objwebloc.PrimaryContact = wl.PrimaryContact;
                objwebloc.DomainName = wl.DomainName;
                objwebloc.StorePhone = wl.StorePhone;
                objwebloc.TrackingPhonePhone = wl.TrackingPhonePhone;
                objwebloc.HoursofOperation = wl.HoursofOperation;
                objwebloc.HoursofOperationOption = wl.HoursofOperationOption;
                objwebloc.OperationStartTime = wl.OperationStartTime;
                objwebloc.OperationEndTime = wl.OperationEndTime;
                objwebloc.DaysAvailableOption = wl.DaysAvailableOption;
                objwebloc.DaysAvailable = wl.DaysAvailable;
                objwebloc.UrlSlug = wl.UrlSlug;
                objwebloc.WebsiteURL = wl.WebsiteURL;
                objwebloc.MetaTitle = wl.MetaTitle;
                objwebloc.MetaDescription = wl.MetaDescription;
                objwebloc.CartOption = wl.CartOption;
                objwebloc.ImageLoc = wl.ImageLoc;
                objwebloc.IsTax = wl.IsTax;
                objwebloc.TaxPercentage = wl.TaxPercentage;
                objwebloc.CuisineType = wl.CuisineType;
                objwebloc.PreparationTime = wl.PreparationTime;
                objwebloc.IsInstruction = wl.IsInstruction;
                objwebloc.PaidOption = wl.PaidOption;
                objwebloc.PaymentOption = wl.PaymentOption;
                objwebloc.FacebookFollowURL = wl.FacebookFollowURL;
                objwebloc.TwitterFollowURL = wl.TwitterFollowURL;
                objwebloc.InstagramFollowURL = wl.InstagramFollowURL;
                objwebloc.YoutubeFollowURL = wl.YoutubeFollowURL;
                objwebloc.CoverImageLoc = wl.CoverImageLoc;
                objwebloc.ExpireTime = wl.ExpireTime;
                objwebloc.DeliveryRadius = wl.DeliveryRadius;
                objwebloc.DeliveryFee = wl.DeliveryFee;
                objwebloc.LastUpdatedBy = CurrentUser.UserId;
                objwebloc.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                objwebloc.SearchEngineIndex = wl.SearchEngineIndex;
                objwebloc.MinimumDeliveryTime = wl.MinimumDeliveryTime;
                objwebloc.MinimumOrderValue = wl.MinimumOrderValue;
                result = _Util.Facade.MenuFacade.UpdateWebsiteLocation(objwebloc);
                companyid = objwebloc.CompanyId;
            }
            else
            {
                wl.CreatedBy = CurrentUser.UserId;
                wl.CreatedDate = DateTime.Now;
                wl.CompanyId = companyid;
                wl.LastUpdatedBy = CurrentUser.UserId;
                wl.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                if (wl.NewLocation == true)
                {
                    wl.ReferCompanyId = CurrentUser.CompanyId.Value;
                    wl.IsDefault = false;
                }
                else
                {
                    wl.ReferCompanyId = new Guid();
                    wl.IsDefault = true;
                }
                if (string.IsNullOrWhiteSpace(wl.PaidOption))
                {
                    wl.PaidOption = "Paid0";
                }
                result = _Util.Facade.MenuFacade.InsertWebsiteLocation(wl) > 0;
                isnew = true;
            }
            if (result)
            {
                #region Latitude and Longitude By Location
                string address = wl.Address.ReplaceSpecialChar().Replace(" ", "+") + "," + wl.City.Replace(" ", "+") + "," + wl.State.Replace(" ", "+") + "," + wl.Zipcode;
                getGoogleMapInfo(address, wl.CompanyId,GoogleMapAPIKey);
                #endregion
                _Util.Facade.MenuFacade.DeleteResturantNeighbarhoodByLocationId(wl.Id);
                if (wl.ListNeighborhood != null && wl.ListNeighborhood.Count > 0)
                {
                    foreach (var item in wl.ListNeighborhood)
                    {
                        ResturantNeighborhood neigh = new ResturantNeighborhood()
                        {
                            SiteLocationId = wl.Id,
                            NeighborhoodName = item,
                            NeighborhoodURL = GetURLSlug(item),
                            CreatedBy = CurrentUser.UserId,
                            CreatedDate = DateTime.Now.UTCCurrentTime()
                        };
                        _Util.Facade.MenuFacade.InsertResturantNeighbarhood(neigh);
                    }
                }
                #region Location Operation
                if (isnew)
                {
                    string fvalue = "";
                    List<Lookup> daysavail = new List<Lookup>();
                    wl.DaysAvailable = wl.DaysAvailable.Replace(", ", ",");
                    var spdays = wl.DaysAvailable.Split(",");
                    if (spdays.Length > 0)
                    {
                        foreach (var day in spdays)
                        {
                            if (day != "null")
                            {
                                var objweblocopt = _Util.Facade.MenuFacade.GetWebsiteLocationOperationBySiteIdAndDayAndCompanyId(wl.Id, day, companyid);
                                if (objweblocopt == null)
                                {
                                    WebsiteLocationOperation WebsiteLocationOperation = new WebsiteLocationOperation()
                                    {
                                        SiteLocationId = wl.Id,
                                        HoursofOperation = day,
                                        OperationStartTime = wl.OperationStartTime,
                                        OperationEndTime = wl.OperationEndTime,
                                        CreatedBy = CurrentUser.UserId,
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        CompanyId = companyid
                                    };
                                    _Util.Facade.MenuFacade.InsertSiteLocationOperation(WebsiteLocationOperation);
                                }
                            }
                        }
                        fvalue = string.Format("'{0}'", string.Join("','", spdays));
                        daysavail = _Util.Facade.LookupFacade.GetAllLookupNotAvailableByDataKeyAndDataValue("DaysAvailable", fvalue, companyid);
                    }
                    if (daysavail != null && daysavail.Count > 0)
                    {
                        foreach (var dl in daysavail)
                        {
                            _Util.Facade.MenuFacade.DeleteAllWebLocOptByCompanyIdAndSiteLocId(companyid, wl.Id, dl.DataValue);
                        }
                    }
                }
                #endregion
            }
            return Json(new { result = result, id = wl.Id, closetype = wl.CloseType });
        }

        [Authorize]
        public ActionResult SeoContent(int? id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Seo model;
            if (id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.MenuFacade.GetSeoById(id.Value);
            }
            else
            {
                model = new Seo();
            }
            List<SelectListItem> folderoption = new List<SelectListItem>();
            folderoption.Add(new SelectListItem()
            {
                Text = "Select Folder",
                Value = "-1"
            });
            folderoption.AddRange(_Util.Facade.MenuFacade.GetAllSeoByCompanyIdAndFolderOPtion(CurrentUser.CompanyId.Value).Select(x => new SelectListItem()
            {
                Text = x.Name.ToString(),
                Value = x.Name.ToString()
            }).ToList());
            ViewBag.folderoption = folderoption;
            return View(model);
        }

        [HttpPost]
        public JsonResult SeoContent(Seo seo)
        {
            bool result = false;
            string message = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var checkpageurl = _Util.Facade.MenuFacade.GetSeoByPageURL(CurrentUser.CompanyId.Value, seo.PageUrl);
            if(checkpageurl != null)
            {
                if(seo.Id > 0)
                {
                    var objseodata = _Util.Facade.MenuFacade.GetSeoById(seo.Id);
                    if(objseodata != null && objseodata.PageUrl != seo.PageUrl)
                    {
                        message = "PageUrl should be unique";
                        return Json(new { result = result, id = seo.Id, message = message });
                    }
                }
                else
                {
                    message = "PageUrl should be unique";
                    return Json(new { result = result, id = seo.Id, message = message });
                }
            }
            seo.CompanyId = CurrentUser.CompanyId.Value;
            seo.IsActive = true;
            if (seo.Id > 0)
            {
                var objseo = _Util.Facade.MenuFacade.GetSeoById(seo.Id);
                objseo.Name = seo.Name;
                objseo.PageUrl = seo.PageUrl;
                objseo.MetaTitle = seo.MetaTitle;
                objseo.MetaDescription = seo.MetaDescription;
                objseo.MetaKeywords = seo.MetaKeywords;
                objseo.OgTitle = seo.OgTitle;
                objseo.OgDescription = seo.OgDescription;
                objseo.IsFolder = seo.IsFolder;
                objseo.FolderOption = seo.FolderOption;
                objseo.IsNav = seo.IsNav;
                objseo.PublishOption = seo.PublishOption;
                result = _Util.Facade.MenuFacade.UpdateSiteContent(objseo);
            }
            else
            {
                result = _Util.Facade.MenuFacade.InsertSiteContent(seo) > 0;
            }
            return Json(new { result = result, id = seo.Id, message = message });
        }

        [HttpPost]
        public JsonResult DeleteSiteContent(int? id)
        {
            bool result = false;
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.MenuFacade.DeleteSiteContent(id.Value);
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteSiteLocation(int? id)
        {
            bool result = false;
            string urlslug = "";
            if (id.HasValue && id.Value > 0)
            {
                var objwebloc = _Util.Facade.MenuFacade.GetSiteLocationById(id.Value);
                if(objwebloc != null)
                {
                    urlslug = objwebloc.UrlSlug;
                }
                result = _Util.Facade.MenuFacade.DeleteSiteLocation(id.Value);
            }
            
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteSiteConfiguration(int? id)
        {
            bool result = false;
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.MenuFacade.DeleteSiteConfiguration(id.Value);
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult SiteContact(int? PageNo, int? PageSize, string SearchText, string order)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!PageNo.HasValue || PageNo.Value < 1)
            {
                PageNo = 1;
            }
            if (SearchText == "undefined" || SearchText == null)
            {
                SearchText = "";
            }
            if (!PageSize.HasValue || PageSize.Value < 1)
            {
                PageSize = 50;
            }

            List<SiteContact> Model = _Util.Facade.MenuFacade.GetAllSiteContactList(CurrentUser.CompanyId.Value, PageNo.Value, PageSize.Value, SearchText, order);
            var objCount = _Util.Facade.MenuFacade.GetAllSiteContact(CurrentUser.CompanyId.Value, SearchText);
            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = order;
            TempData["data"] = SearchText;

            if (objCount.Count() > 0)
            {
                ViewBag.OutOfNumber = objCount.Count();
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
            return View("SiteContact",Model);
        }

        [Authorize]
        public ActionResult AddSiteContact(int? id)
        {
            SiteContact model;
            if (id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.MenuFacade.GetSiteContactById(id.Value);
            }
            else
            {
                model = new SiteContact();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult AddSiteContact(SiteContact contact)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (contact.Id > 0)
            {
                var objsitecontact = _Util.Facade.MenuFacade.GetSiteContactById(contact.Id);
                objsitecontact.FirstName = contact.FirstName;
                objsitecontact.LastName = contact.LastName;
                objsitecontact.CellPhone = contact.CellPhone;
                objsitecontact.WorkPhone = contact.WorkPhone;
                objsitecontact.Email = contact.Email;
                objsitecontact.IsEmail = contact.IsEmail;
                objsitecontact.IsText = contact.IsText;
                objsitecontact.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                objsitecontact.LastUpdatedBy = CurrentUser.UserId;
                result = _Util.Facade.MenuFacade.UpdateSiteContact(objsitecontact);
            }
            else
            {
                contact.ContactId = Guid.NewGuid();
                contact.CreatedBy = CurrentUser.UserId;
                contact.CreatedDate = DateTime.Now.UTCCurrentTime();
                contact.LastUpdatedBy = CurrentUser.UserId;
                contact.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                contact.CompanyId = CurrentUser.CompanyId.Value;
                result = _Util.Facade.MenuFacade.InsertSiteContact(contact) > 0;
            }
            return Json(new { result = result, id = contact.Id });
        }

        [HttpPost]
        public JsonResult DeleteSiteContact(int? id)
        {
            bool result = false;
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.MenuFacade.DeleteSiteContact(id.Value);
            }
            return Json(result);
        }

        public ActionResult LoadHoursOperation(string daylist, int locid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SelectListItem> daysavailable = new List<SelectListItem>();
            List<SelectListItem> timeAvailable = new List<SelectListItem>();
            List<WebsiteLocationOperation> ListWebsiteLocationOperation = new List<WebsiteLocationOperation>();
            if (!string.IsNullOrWhiteSpace(daylist) && daylist != "null")
            {
                ListWebsiteLocationOperation = _Util.Facade.MenuFacade.GetAllWebsiteLocationOperationByLocationIdAndCompanyId(CurrentUser.CompanyId.Value, locid, "");
                var spday = daylist.Split(',');
                if (spday.Length > 0)
                {
                    foreach (var item in spday)
                    {
                        if (ListWebsiteLocationOperation.Where(x => x.HoursofOperation == item).FirstOrDefault() == null)
                        {
                            daysavailable.Add(new SelectListItem()
                            {
                                Text = item.ToString(),
                                Value = item.ToString()
                            });
                        }
                    }
                }
                timeAvailable.AddRange(_Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", null, null).Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList());
            }
            ViewBag.daysavailable = daysavailable;
            ViewBag.timeAvailable = timeAvailable;
            return View();
        }

        [HttpPost]
        public JsonResult SaveHoursOperation(WebsiteLocationOperation WebsiteLocationOperation)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (WebsiteLocationOperation.Id > 0)
            {
                if (WebsiteLocationOperation.IsAdditional.HasValue && WebsiteLocationOperation.IsAdditional.Value == true)
                {
                    var objsitelocopt = _Util.Facade.MenuFacade.GetWebsiteLocationOperationById(WebsiteLocationOperation.Id);
                    if(objsitelocopt != null && objsitelocopt.IsAdditional == true)
                    {
                        _Util.Facade.MenuFacade.DeleteWebsiteLocationOperation(WebsiteLocationOperation.Id);
                    }
                    WebsiteLocationOperation.CreatedBy = CurrentUser.UserId;
                    WebsiteLocationOperation.CreatedDate = DateTime.Now.UTCCurrentTime();
                    result = _Util.Facade.MenuFacade.InsertSiteLocationOperation(WebsiteLocationOperation) > 0;
                }
                else
                {
                    var objsitelocopt = _Util.Facade.MenuFacade.GetWebsiteLocationOperationById(WebsiteLocationOperation.Id);
                    objsitelocopt.HoursofOperation = WebsiteLocationOperation.HoursofOperation;
                    objsitelocopt.OperationStartTime = WebsiteLocationOperation.OperationStartTime;
                    objsitelocopt.OperationEndTime = WebsiteLocationOperation.OperationEndTime;
                    objsitelocopt.StoreOperationStartTime = WebsiteLocationOperation.StoreOperationStartTime;
                    objsitelocopt.StoreOperationEndTime = WebsiteLocationOperation.StoreOperationEndTime;
                    objsitelocopt.IsAdditional = WebsiteLocationOperation.IsAdditional;
                    result = _Util.Facade.MenuFacade.UpdateSiteLocationOperation(objsitelocopt);
                }
            }
            else
            {
                WebsiteLocationOperation.CreatedBy = CurrentUser.UserId;
                WebsiteLocationOperation.CreatedDate = DateTime.Now.UTCCurrentTime();
                result = _Util.Facade.MenuFacade.InsertSiteLocationOperation(WebsiteLocationOperation) > 0;
            }
            return Json(new { result = result, locid = WebsiteLocationOperation.SiteLocationId });
        }

        public ActionResult LoadHoursOfOPt(int? locid, string starttime, string endtime)
        {
            List<WebsiteLocationOperation> model = new List<WebsiteLocationOperation>();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (locid.HasValue && locid.Value > 0)
            {
                model = _Util.Facade.MenuFacade.GetAllWebsiteLocationOperationByLocationIdAndCompanyId(CurrentUser.CompanyId.Value, locid.Value, "store");
            }
            ViewBag.StartTime = starttime;
            ViewBag.EndTime = endtime;
            return View(model);
        }

        public ActionResult LoadOnlineOrderHoursOfOPt(int? locid, string starttime, string endtime)
        {
            List<WebsiteLocationOperation> model = new List<WebsiteLocationOperation>();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (locid.HasValue && locid.Value > 0)
            {
                model = _Util.Facade.MenuFacade.GetAllWebsiteLocationOperationByLocationIdAndCompanyId(CurrentUser.CompanyId.Value, locid.Value, "");
            }
            ViewBag.StartTime = starttime;
            ViewBag.EndTime = endtime;
            return View(model);
        }

        public ActionResult AddHoursofOpt(int? id, string starttime, string endtime, string type, string day, int? locid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            WebsiteLocationOperation model;
            if(id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.MenuFacade.GetWebsiteLocationOperationById(id.Value);
            }
            else
            {
                model = new WebsiteLocationOperation();
                model.HoursofOperation = day;
                model.SiteLocationId = (locid.HasValue ? locid.Value : 0);
            }
            if(locid.HasValue && locid.Value > 0)
            {
                var objwebloc = _Util.Facade.MenuFacade.GetWebLocById(locid.Value);
                if(objwebloc != null)
                {
                    model.CompanyId = objwebloc.CompanyId;
                }
            }
            var mintime = _Util.Facade.GlobalSettingsFacade.GetMenuTimeAvailableStartTime(CurrentUser.CompanyId.Value);
            var maxtime = _Util.Facade.GlobalSettingsFacade.GetMenuTimeAvailableEndTime(CurrentUser.CompanyId.Value);
            List<SelectListItem> timeAvailable = new List<SelectListItem>();
            timeAvailable.AddRange(_Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", null, null).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.timeAvailable = timeAvailable;
            ViewBag.type = !string.IsNullOrWhiteSpace(type) ? type : "";
            return View(model);
        }

        [HttpPost]
        public JsonResult DeleteHoursOpt(int? id)
        {
            bool result = false;
            int locid = 0;
            if(id.HasValue && id.Value > 0)
            {
                var objweblocopt = _Util.Facade.MenuFacade.GetWebsiteLocationOperationById(id.Value);
                if(objweblocopt != null)
                {
                    if(objweblocopt.IsAdditional.HasValue && objweblocopt.IsAdditional == true)
                    {
                        result = _Util.Facade.MenuFacade.DeleteWebsiteLocationOperation(id.Value);
                        locid = objweblocopt.SiteLocationId;
                    }
                    else
                    {
                        result = _Util.Facade.MenuFacade.DeleteAllWebsiteLocationOperationByHoursOpt(objweblocopt.HoursofOperation, objweblocopt.SiteLocationId);
                        locid = objweblocopt.SiteLocationId;
                    }
                }
            }
            return Json(new { result = result, locid = locid });
        }

        [HttpPost]
        public JsonResult LocationAddPermission(string state, string city, string address, string zip, string urlslug)
        {
            string websiteurl = "/" + state.ToLower() + "/" + city.ToLower() + "/" + urlslug.ToLower();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            if (!string.IsNullOrWhiteSpace(state))
            {
                var objwebloc = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyIdAndStateAndCity(state, city, address, zip, urlslug);
                if(objwebloc.Count == 0)
                {
                    var objurl = _Util.Facade.MenuFacade.GetWebsiteLocationByUrlSlug(websiteurl);
                    if(objurl.Count == 0)
                    {
                        result = true;
                    }
                }
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult UploadWeblocPhoto(string imgurl)
        {
            bool isUploaded = false;
            int width = 0;
            int height = 0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string[] datasplit = imgurl.Split(',');
            byte[] bytes = Convert.FromBase64String(datasplit[1]);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
                string tempFolderName = ConfigurationManager.AppSettings["File.CategoryPhoto"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___image_preview.png";
            string tempFolderPath = "";

            tempFolderPath = Server.MapPath("~/" + tempFolderName);

            if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
            {
                try
                {
                    image.Save(Path.Combine(tempFolderPath, FileName));
                }
                catch (Exception) {  /*TODO: You must process this exception.*/}
            }
            var imgpath = Image.FromFile(Path.Combine(tempFolderPath, FileName));
            if(imgpath != null)
            {
                width = imgpath.Width;
                height = imgpath.Height;
            }
            isUploaded = true;
            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            return Json(new { isUploaded = isUploaded, FullFilePath = AppConfig.DomainSitePath + filePath });
        }

        public ActionResult AddSocialMediaContent(int? id, Guid comid)
        {
            SocialMediaContent model = new SocialMediaContent();
            model.CompanyId = comid;
            if(id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.MenuFacade.GetMediaContentById(id.Value);
            }
            List<SelectListItem> socialmediatype = new List<SelectListItem>();
            socialmediatype.AddRange(_Util.Facade.LookupFacade.GetLookUpByKey("SocialMediaType").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.socialmediatype = socialmediatype;
            return View(model);
        }

        public ActionResult LoadSocialMediaContent(Guid? comid)
        {
            List<SocialMediaContent> model = new List<SocialMediaContent>();
            if(comid.HasValue && comid.Value != new Guid())
            {
                model = _Util.Facade.MenuFacade.GetAllMediaContentByCompanyId(comid.Value);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveSocialMedia(SocialMediaContent smc)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            smc.CreatedBy = CurrentUser.UserId;
            smc.LastUpdatedBy = CurrentUser.UserId;
            smc.CreatedDate = DateTime.Now.UTCCurrentTime();
            smc.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            if (smc.Id > 0)
            {
                result = _Util.Facade.MenuFacade.UpdateSocialMedia(smc);
            }
            else
            {
                var objsmc = _Util.Facade.MenuFacade.GetMediaContentByCompanyIdAndName(smc.CompanyId, smc.Name);
                if(objsmc != null)
                {
                    objsmc.FollowUpLink = smc.FollowUpLink;
                    objsmc.ShareLink = smc.ShareLink;
                    result = _Util.Facade.MenuFacade.UpdateSocialMedia(objsmc);
                }
                else
                {
                    result = _Util.Facade.MenuFacade.InsertSocialMedia(smc) > 0;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteSocialMedia(int? id, Guid comid)
        {
            bool result = false;
            if(id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.MenuFacade.DeleteSocialMedia(id.Value);
            }
            return Json(new { result = result, comid = comid });
        }

        [HttpPost]
        public JsonResult GetLocationUrlSlugPermission(int? id, string urlslug, string state, string city)
        {
            bool result = false;
            var websiteurl = "/" + state.ToLower() + "/" + city.ToLower() + "/" + urlslug.ToLower();
            if (id.HasValue && id.Value > 0 && !string.IsNullOrWhiteSpace(urlslug))
            {
                var objwebloc = _Util.Facade.MenuFacade.GetWebLocById(id.Value);
                if(objwebloc != null && objwebloc.WebsiteURL == websiteurl)
                {
                    var loclist = _Util.Facade.MenuFacade.GetWebsiteLocationByUrlSlug(websiteurl);
                    if (loclist.Count < 2)
                    {
                        result = true;
                    }
                }
                else
                {
                    var loclist = _Util.Facade.MenuFacade.GetWebsiteLocationByUrlSlug(websiteurl);
                    if(loclist.Count == 0)
                    {
                        result = true;
                    }
                }
            }
            return Json(result);
        }

        public ActionResult LoadSystemSettings()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ResturantSystemSetting model = _Util.Facade.MenuFacade.GetAllResturantSystemSettingByCompanyId(CurrentUser.CompanyId.Value);
            return View("LoadSystemSetting", model);
        }

        public ActionResult AddSystemSetting(int? id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ResturantSystemSetting model = new ResturantSystemSetting();
            if(id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.MenuFacade.GetResturantSystemSettingById(id.Value);
            }
            List<SelectListItem> primarycontact = new List<SelectListItem>();
            primarycontact.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            primarycontact.AddRange(_Util.Facade.MenuFacade.GetAllSiteContact(CurrentUser.CompanyId.Value, "").Select(x => new SelectListItem()
            {
                Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                Value = x.ContactId.ToString()
            }).ToList());
            ViewBag.primarycontact = primarycontact;
            return View(model);
        }

        [HttpPost]
        public JsonResult AddSystemSettings(ResturantSystemSetting rss)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            var objrss = _Util.Facade.MenuFacade.GetResturantSystemSettingById(rss.Id);
            if(objrss != null)
            {
                objrss.Restaurant = rss.Restaurant;
                objrss.Logo = rss.Logo;
                objrss.TaxRate = rss.TaxRate;
                objrss.PrimaryContact = rss.PrimaryContact;
                objrss.AuthApiLoginKey = rss.AuthApiLoginKey;
                objrss.AuthApiTransactionKey = rss.AuthApiTransactionKey;
                objrss.MinimumOrderValue = rss.MinimumOrderValue;
                objrss.AutoConfirmOrder = rss.AutoConfirmOrder;
                if (objrss.CreatedBy == new Guid())
                {
                    objrss.CreatedBy = CurrentUser.UserId;
                }
                result = _Util.Facade.MenuFacade.UpdateResturantSystemSetting(objrss);
            }
            return Json(new { result = result, id = rss.Id });
        }

        public string GetURLSlug(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                key = key.Trim(' ');
                key = key.Replace("'", "").Replace(",", "").Replace(":", "");
                key = Regex.Replace(key, @"[^0-9a-zA-Z]+", "-").ToLower();
            }
            return key;
        }

        public JsonResult NeighbarhoodTagitSuggestion(string key)
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                List<ResturantNeighborhood> rnlist = _Util.Facade.MenuFacade.GetAllNeighbarhoodByKey(key);
                if (rnlist.Count > 0)
                {
                    result = JsonConvert.SerializeObject(rnlist);
                }
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public RootObject getGoogleMapInfo(string address, Guid comid,string GoogleMapAPIKey)
        {
            var root = new RootObject();

            var url =
                string.Format(
                    "https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}", address ,GoogleMapAPIKey);
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
            if (root.status.ToLower() == "ok" && root.results != null && root.results.Count > 0)
            {
                _Util.Facade.MenuFacade.DeleteLocationMapInfo(comid);
                foreach (var item in root.results)
                {
                    WebsiteLocationMapInfo WebsiteLocationMapInfo = new WebsiteLocationMapInfo()
                    {
                        CompanyId = comid,
                        Latitude = item.geometry.location.lat.ToString(),
                        Longitude = item.geometry.location.lng.ToString(),
                        CreatedBy = new Guid(),
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedBy = new Guid(),
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    };
                    _Util.Facade.MenuFacade.InsertLoctionMapInfo(WebsiteLocationMapInfo);
                }
            }
            return root;
        }

        public ActionResult LoadTrackingSetting()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<TrackingNumberSetting> model = _Util.Facade.MenuFacade.GetAllTrackingNumbersByComapnyId(CurrentUser.CompanyId.Value);
            return View(model);
        }

        public ActionResult AddTrackingNumber(int? id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            TrackingNumberSetting model;
            if(id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.MenuFacade.GetTrackingNumberSettingById(id.Value);
            }
            else
            {
                model = new TrackingNumberSetting();
            }
            List<SelectListItem> domainlist = new List<SelectListItem>();
            domainlist.Add(new SelectListItem()
            {
                Text = "Select Location",
                Value = "-1"
            });
            domainlist.AddRange(_Util.Facade.MenuFacade.GetAllDifferentLocationsByCompanyId(CurrentUser.CompanyId.Value).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.CompanyId.ToString()
            }).ToList());
            ViewBag.domainlist = domainlist;
            return View(model);
        }

        [HttpPost]
        public JsonResult AddTrackingNumber(TrackingNumberSetting tns)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Random rand = new Random();
            bool result = false;
            List<string> numberlist = new List<string>();
            try
            {
                if (tns.Id > 0)
                {
                    var objtracking = _Util.Facade.MenuFacade.GetTrackingNumberSettingById(tns.Id);
                    tns.CreatedBy = objtracking.CreatedBy;
                    tns.CreatedDate = objtracking.CreatedDate;
                    tns.LastUpdatedBy = CurrentUser.UserId;
                    tns.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    result = _Util.Facade.MenuFacade.UpdateTrackingNumberSetting(tns);
                }
                else
                {
                    var tracknumber = "1" + tns.TrackingNumber;
                    var pauthid = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PlivoAuthId");
                    var pauthtoken = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PlivoAuthToken");
                    var objtracknumber = _Util.Facade.MenuFacade.GetTrackingNumberSettingByCompanyId(tns.CompanyId);
                    var api = new PlivoApi("MAN2Y3YZU5ZDMWZJEYOD", "OTVlNjg5YmMzMjNlMDc1MjYzMjg0OGQ1MTA0ZDZh");
                    var response = api.PhoneNumber.Buy(number: tracknumber.Replace("-", ""), appId: "75635309776253134");
                    tns.CreatedBy = CurrentUser.UserId;
                    tns.CreatedDate = DateTime.Now.UTCCurrentTime();
                    tns.LastUpdatedBy = CurrentUser.UserId;
                    tns.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    tns.TrackingId = Guid.NewGuid();
                    result = _Util.Facade.MenuFacade.InsertTrackingNumberSetting(tns) > 0;
                }
            }
            catch (PlivoRestException e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteTrackNumber(int? id)
        {
            if(id.HasValue && id.Value > 0)
            {
                _Util.Facade.MenuFacade.DeleteTrackingNumber(id.Value);
            }
            return Json(true);
        }

        [HttpGet]
        public JsonResult GetTrackingNumberSearchResult(string key, Guid comid, string authid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Random rand = new Random();
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var pauthid = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PlivoAuthId");
                var pauthtoken = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PlivoAuthToken");
                
                var api = new PlivoApi("MAN2Y3YZU5ZDMWZJEYOD", "OTVlNjg5YmMzMjNlMDc1MjYzMjg0OGQ1MTA0ZDZh");
                var response = api.PhoneNumber.List(countryIso: "US", pattern: key.Replace("-", ""));
                if (response.Objects.Count > 0)
                    result = JsonConvert.SerializeObject(response.Objects);
                return Json(new { result = result, authid = "", authtoken = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = result, authid = "", authtoken = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetSiteLocationTrackingNumberSearchResult(string key, Guid comid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var response = _Util.Facade.MenuFacade.GetAllSearchTrackingNumberSettingsByKeyAndCompanyId(comid, key);
                if (response.Count > 0)
                    result = JsonConvert.SerializeObject(response);
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerCallRecordPartial(int? id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            TrackingNumberSetting model = new TrackingNumberSetting();
            if(id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.MenuFacade.GetTrackingNumberSettingById(id.Value);
            }
            List<SelectListItem> customerlist = new List<SelectListItem>();
            customerlist.Add(new SelectListItem()
            {
                Text = "All",
                Value = "All"
            });
            customerlist.AddRange(_Util.Facade.MenuFacade.GetAllCustomersByCompanyId(CurrentUser.CompanyId.Value).Select(x => new SelectListItem()
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.CustomerId.ToString()
            }).ToList());
            ViewBag.customerlist = customerlist;
            return View(model);
        }

        public JsonResult GetPlivoNumberDetails()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var pauthid = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PlivoAuthId");
            var pauthtoken = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PlivoAuthToken");

            var api = new PlivoApi("MAN2Y3YZU5ZDMWZJEYOD", "OTVlNjg5YmMzMjNlMDc1MjYzMjg0OGQ1MTA0ZDZh");
            var response = api.Number.Get(number: "15183024606");
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult forward(string From, string To, string CallUUID)
        {
            Plivo.XML.Response resp = new Plivo.XML.Response();
            Plivo.XML.GetDigits get_digits = new
                Plivo.XML.GetDigits("",
                   new Dictionary<string, string>()
            {
                {"action", "https://www.foo.com/record/action/"},
                        {"method", "POST"},
            });
            resp.Add(get_digits);

            get_digits.AddSpeak("This call may be recorded for quality and training purposes",
                new Dictionary<string, string>() { });
            var output = resp.ToString();
            Console.WriteLine(output);
            return RedirectToAction("forwardindex", new { From = From, To = To, CallUUID = CallUUID });
        }

        public ActionResult forwardindex(string From, string To, string CallUUID)
        {
            string output = "";
            string from_number = "12035023245";
            string to_number = "18172428806";
            string calluuid = "";
            string queryfrom_number = "";
            string queryto_number = "";
            try
            {
                string constr = ConfigurationManager.AppSettings["CallForwardingConStr"];
                HSApiFacade HSApiFacade = new HSApiFacade(constr);
                if (!string.IsNullOrWhiteSpace(From))
                {
                    queryfrom_number = From;
                }
                else
                {
                    queryfrom_number = Request.QueryString["From"];
                }
                if (!string.IsNullOrWhiteSpace(To))
                {
                    queryto_number = To;
                }
                else
                {
                    queryto_number = Request.QueryString["To"];
                }
                if (!string.IsNullOrWhiteSpace(CallUUID))
                {
                    calluuid = CallUUID;
                }
                else
                {
                    calluuid = Request.QueryString["CallUUID"];
                }
                
                if (queryfrom_number != null && !string.IsNullOrWhiteSpace(queryfrom_number))
                {
                    from_number = "+" + queryfrom_number;
                }
                else
                {
                    from_number = "+" + from_number;
                }
                if (queryto_number != null && !string.IsNullOrWhiteSpace(queryto_number))
                {
                    to_number = "+" + queryto_number;
                }
                else
                {
                    to_number = "+" + to_number;
                }
                Plivo.XML.Response resp = new Plivo.XML.Response();
                Plivo.XML.Dial dial = new Plivo.XML.Dial(new
                    Dictionary<string, string>(){
                {"callerId",from_number.Replace("+", "")}
            });
                if (!string.IsNullOrWhiteSpace(to_number))
                {
                    var objtrack = HSApiFacade.GetTrackingDetailsByTrackingPhone(to_number.Replace("+1", ""));
                    if (objtrack != null)
                    {
                        dial.AddNumber(objtrack.ForwardingNumber.Replace("-", "").Length == 10 ? "1" + objtrack.ForwardingNumber.Replace("-", "") : objtrack.ForwardingNumber.Replace("-", ""),
                        new Dictionary<string, string>() { });
                        resp.Add(dial);
                        RecordCall(calluuid);
                    }
                }

                output = resp.ToString();
                Console.WriteLine(output);
                
                System.IO.File.WriteAllText(Server.MapPath("~/PlivoCallerLog.txt"), "Success Log- " + from_number + " " + to_number);
            }
            catch(Exception e)
            {
                System.IO.File.WriteAllText(Server.MapPath("~/PlivoCallerLog.txt"), "Error Log- " + e.Message);
            }
            

            return this.Content(output, "text/xml");
        }

        public static void RecordCall(string calluuid)
        {
            var api = new PlivoApi("MAN2Y3YZU5ZDMWZJEYOD", "OTVlNjg5YmMzMjNlMDc1MjYzMjg0OGQ1MTA0ZDZh");
            try
            {
                var response = api.Call.StartRecording(
                    callUuid: calluuid,
                    timeLimit : 3600
                );
                Console.WriteLine(response);
            }
            catch (PlivoRestException e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        [HttpPost]
        public JsonResult GetStorePhoneNumberByTracking(Guid comid)
        {
            bool result = false;
            WebsiteLocation model = new WebsiteLocation();
            if(comid != new Guid())
            {
                model = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(comid);
                result = true;
            }
            return Json(new { result = result, storephn = model.StorePhone });
        }
    }
}
