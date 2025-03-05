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

namespace HS.Web.UI.Controllers
{
    public class MenuManagementController : BaseController
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

        public PartialViewResult LoadMenusPartial(int? PageNo, int? PageSize, string SearchText, string order)
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

            MenuListModel Model = _Util.Facade.MenuFacade.GetMenuList(CurrentUser.CompanyId.Value, PageNo.Value, PageSize.Value, SearchText, order);
            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = order;
            TempData["data"] = SearchText;


            if (Model.Menus.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCount;
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
            ViewBag.ItemCount = _Util.Facade.MenuFacade.GetAllMenuItemByCompanyId(CurrentUser.CompanyId.Value).Count;
            Model.WebsiteLocation = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(CurrentUser.CompanyId.Value);
            return PartialView("_MenuPartial", Model);
        //    return PartialView( Model);
        }

        public ActionResult MenuManagement()
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.LeftMenuMenuManagement))
            {
                return View("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (true)
            {
                ViewBag.StartTab = "AllItemsTab";
            }
            else if (base.IsPermitted(UserPermissions.MenuManagementPermissions.MenusTab))
            {
                ViewBag.StartTab = "MenusTab";
            }
            else if (base.IsPermitted(UserPermissions.MenuManagementPermissions.CategoriesTab))
            {
                ViewBag.StartTab = "CategoriesTab";
            }
            else if (base.IsPermitted(UserPermissions.MenuManagementPermissions.ToppingsTab))
            {
                ViewBag.StartTab = "ToppingsTab";
            }
            else if (base.IsPermitted(UserPermissions.MenuManagementPermissions.SpecialsTab))
            {
                ViewBag.StartTab = "SpecialsTab";
            }
            else if (base.IsPermitted(UserPermissions.MenuManagementPermissions.ArchivedItemsTab))
            {
                ViewBag.StartTab = "ArchivedItemsTab";
            }
            else
            {
                return View("~/Views/Shared/_AccessDenied.cshtml");
            }


            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ViewBag.AllItemCount = _Util.Facade.MenuFacade.GetAllMenuItemByCompanyId(CurrentUser.CompanyId.Value).Count;
            ViewBag.MenuCount = _Util.Facade.MenuFacade.GetAllMenuByCompanyId(CurrentUser.CompanyId.Value).Count;
            ViewBag.CategoryCount = _Util.Facade.MenuFacade.GetAllCategory(CurrentUser.CompanyId.Value).Count;
            ViewBag.ToppingCount = _Util.Facade.MenuFacade.GetAllToppingCategoryByCompanyId(CurrentUser.CompanyId.Value).Count;
            return View();
        }
        public PartialViewResult LoadCategoriesPartial(int? PageNo, int? PageSize, string SearchText, string order)
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
            ViewBag.WebLoc = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(CurrentUser.CompanyId.Value);
            CategoryListModel Model = _Util.Facade.CategoryFacade.GetCategoryList(CurrentUser.CompanyId.Value, PageNo.Value, PageSize.Value, SearchText, order);
            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = order;
            TempData["data"] = SearchText;
            if (Model.Categoriess.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCount;
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

            return PartialView("_CategoryPartial", Model);
        }
        public PartialViewResult LoadToppingsPartial(int? PageNo, int? PageSize, string SearchText, string order)
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

            ToppingListModel Model = _Util.Facade.ToppingFacade.GetToppingList(PageNo.Value, PageSize.Value, SearchText, order, CurrentUser.CompanyId.Value);
            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = order;
            TempData["data"] = SearchText;
            if (Model.ListToppingCategory.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCount;
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

            return PartialView("_ToppingPartial", Model);
        }
        public PartialViewResult LoadSpecialsPartial(int? PageNo, int? PageSize, string SearchText, string order)
        {
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //if (!PageNo.HasValue || PageNo.Value < 1)
            //{
            //    PageNo = 1;
            //}
            //if (SearchText == "undefined" || SearchText == null)
            //{
            //    SearchText = "";
            //}
            //if (!PageSize.HasValue || PageSize.Value < 1)
            //{
            //    PageSize = 50;
            //}

            //MenuListModel Model = _Util.Facade.MenuFacade.GetMenuList(PageNo.Value, PageSize.Value, SearchText, order);
            //ViewBag.PageNumber = PageNo;
            //ViewBag.OutOfNumber = 0;
            //ViewBag.order = order;

            //if (Model.Menus.Count() > 0)
            //{
            //    ViewBag.OutOfNumber = Model.TotalCount;
            //}

            //if ((int)ViewBag.PageNumber * PageSize > (int)ViewBag.OutOfNumber)
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            //}
            //else
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.PageNumber * PageSize;
            //}
            //ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / PageSize.Value);

            //return PartialView("_MenuPartial", Model);
            return PartialView("_MenuManagementPartial");
        }
        public PartialViewResult LoadArchivedItemsPartial(int? PageNo, int? PageSize, string SearchText, string order)
        {
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //if (!PageNo.HasValue || PageNo.Value < 1)
            //{
            //    PageNo = 1;
            //}
            //if (SearchText == "undefined" || SearchText == null)
            //{
            //    SearchText = "";
            //}
            //if (!PageSize.HasValue || PageSize.Value < 1)
            //{
            //    PageSize = 50;
            //}

            //MenuListModel Model = _Util.Facade.MenuFacade.GetMenuList(PageNo.Value, PageSize.Value, SearchText, order);
            //ViewBag.PageNumber = PageNo;
            //ViewBag.OutOfNumber = 0;
            //ViewBag.order = order;

            //if (Model.Menus.Count() > 0)
            //{
            //    ViewBag.OutOfNumber = Model.TotalCount;
            //}

            //if ((int)ViewBag.PageNumber * PageSize > (int)ViewBag.OutOfNumber)
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            //}
            //else
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.PageNumber * PageSize;
            //}
            //ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / PageSize.Value);

            //return PartialView("_MenuPartial", Model);
            return PartialView("_MenuManagementPartial");
        }
        public PartialViewResult ComingPartial()
        {
            return PartialView("_MenuManagementPartial");
        }
        [Authorize]
        public PartialViewResult AddMenu(int? Id, bool? editmenu)
        {
            RestMenu Model = new RestMenu();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            
            if (Id > 0)
            {
                Model = _Util.Facade.MenuFacade.GetMenuById(Id.Value);

                //Model.MenuItems = _Util.Facade.MenuFacade.GetAllMenuItemByMenuId(Model.Id);

                //Model.Categories = _Util.Facade.MenuFacade.GetAllCategoryByMenuId(Model.Id);
                if (!string.IsNullOrWhiteSpace(Model.DaysAvailable))
                {
                    Model.DaysAvailable = Model.DaysAvailable.Replace(", ", ",");
                    var spadays = Model.DaysAvailable.Split(',');
                    Model.AvailableDays = spadays.ToList();
                }
                Model.Category = _Util.Facade.MenuFacade.GetCategoryByCompanyIdAndMenuId(currentLoggedIn.CompanyId.Value, Model.Id);
            }
            else
            {
                Model.Category = new RestCategory();
            }
            Model.WebsiteLocation = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(currentLoggedIn.CompanyId.Value);
            #region ViewBag
            ViewBag.selectMenuStatus = "1";
            if (Model.Status != null)
            {
                ViewBag.selectMenuStatus = Model.Status == true ? "1" : "0";
            }

            ViewBag.MenuStatus = _Util.Facade.LookupFacade.GetLookupByKey("MenuStatus").Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString()
                         }).ToList();
            var objwebloc = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(currentLoggedIn.CompanyId.Value);
            List<Lookup> allDays = new List<Lookup>();
            List<string> selectedDayList = new List<string>();
            List<Lookup> selectDropdown = new List<Lookup>();
            if (objwebloc != null && !string.IsNullOrWhiteSpace(objwebloc.DaysAvailable))
            {
                var spdaysavail = objwebloc.DaysAvailable.Replace(" ", "").Split(',');
                if(spdaysavail.Length > 0)
                {
                    foreach(var item in spdaysavail)
                    {
                        allDays.Add(_Util.Facade.LookupFacade.GetLookupByKey("DaysAvailable").Where(x => x.DataValue == item).FirstOrDefault());
                    }
                }
                selectDropdown = allDays;
            }
            else
            {
                allDays = _Util.Facade.LookupFacade.GetLookupByKey("DaysAvailable");
                selectDropdown = allDays;
            }
            if (!string.IsNullOrWhiteSpace(Model.DaysAvailable) && Model.DaysAvailable != null)
            {
                selectedDayList = Model.DaysAvailable.Split(',').ToList();
                selectDropdown = allDays.Where(x => selectedDayList.Contains(x.DataValue)).ToList();
            }

            ViewBag.DaysAvailable = allDays.Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString(),
                             Selected = selectDropdown.Count(y => y.DataValue == x.DataValue) > 0
                         }).ToList();
            var mintime = _Util.Facade.GlobalSettingsFacade.GetMenuTimeAvailableStartTime(currentLoggedIn.CompanyId.Value);
            var maxtime = _Util.Facade.GlobalSettingsFacade.GetMenuTimeAvailableEndTime(currentLoggedIn.CompanyId.Value);
            string selectTimeFrom = mintime;
            string selectTimeTo = maxtime;
            if (!string.IsNullOrWhiteSpace(Model.TimeAvailable) && Model.TimeAvailable != null)
            {
                List<string> timeList = Model.TimeAvailable.Split(new[] { " to " }, StringSplitOptions.None).ToList();
                List<string> fromtimeList = timeList[0].Split(' ').ToList();
                List<string> totimeList = timeList[1].Split(' ').ToList();
                if (fromtimeList.Count > 2)
                {
                    selectTimeFrom = (Convert.ToDateTime(fromtimeList[1])).ToString("HH:mm");
                }
                else
                {
                    selectTimeFrom = (Convert.ToDateTime(fromtimeList[0])).ToString("HH:mm");
                }
                if (totimeList.Count > 2)
                {
                    selectTimeTo = (Convert.ToDateTime(totimeList[1])).ToString("HH:mm");
                }
                else
                {
                    selectTimeTo = (Convert.ToDateTime(totimeList[0])).ToString("HH:mm");
                }
            }
            List<SelectListItem> timeAvailable = new List<SelectListItem>();
            timeAvailable.AddRange(_Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", mintime, maxtime).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.timeAvailable = timeAvailable;
            ViewBag.selectTimeFrom = selectTimeFrom;
            ViewBag.selectTimeTo = selectTimeTo;
            ViewBag.storetimefrom = mintime;
            ViewBag.storetimeto = maxtime;
            #endregion
            List<string> dayavailable = new List<string>();
            foreach (var item in (List<SelectListItem>)ViewBag.DaysAvailable)
            {
                dayavailable.Add(item.Value);
            }
            ViewBag.dayavailable = dayavailable;
            if(editmenu.HasValue && editmenu.Value == true)
            {
                return PartialView("AddMenuAllItem", Model);
            }
            else
            {
                return PartialView("_AddMenu", Model);
            }
        }
        [Authorize]
        public PartialViewResult AddMenuItem(int? menuId, int? miId, bool? edititem)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            RestMenuItem Model = new RestMenuItem();
            //if (Model.TaxPercentage > 0)
            //{
            //    Model.IsTax = true;
            //}
            //else
            //{
            //    Model.IsTax = false;
            //}
            //if (menuId > 0)
            //ViewBag.AddMenuId = menuId.Value;
            if (miId > 0)
            {
                Model = _Util.Facade.MenuFacade.GetMenuItemById(miId.Value);
                var objmenu = _Util.Facade.MenuFacade.GetMenuById(menuId.Value);
                Model.Toppings = _Util.Facade.MenuFacade.GetAllToppingByMenuId(objmenu.MenuId, Model.ItemId, currentLoggedIn.CompanyId.Value);
                Model.Categories = _Util.Facade.MenuFacade.GetAllCategoryByMenuId(objmenu.MenuId, Model.ItemId, currentLoggedIn.CompanyId.Value);
                if (!string.IsNullOrWhiteSpace(Model.DaysAvailable))
                {
                    Model.DaysAvailable = Model.DaysAvailable.Replace(", ", ",");
                    var spadays = Model.DaysAvailable.Split(',');
                    Model.AvailableDays = spadays.ToList();
                }
                if (menuId.HasValue && menuId.Value > 0 && miId.HasValue && miId.Value > 0)
                {
                    Model.category = _Util.Facade.MenuFacade.GetCategoryByCompanyIdAndMenuIdAndMenuItemId(currentLoggedIn.CompanyId.Value, menuId.Value, Model.ItemId);
                }
                else
                {
                    Model.category = new Category();
                }
                if(Model.MaxQty == null || Model.MaxQty == 0)
                {
                    Model.MaxQty = _Util.Facade.GlobalSettingsFacade.GetMaxOrderingQuantityByCompanyId(currentLoggedIn.CompanyId.Value);
                }
                Model.ListAdditionalContent = _Util.Facade.MenuFacade.GetAllAdditionalContentByItemId(Model.ItemId);
            }
            else
            {
                var objlistmenu = _Util.Facade.MenuFacade.GetAllMenuByCompanyId(currentLoggedIn.CompanyId.Value).OrderBy(x => x.Id);
                ViewBag.FirstMenuSelected = objlistmenu != null && objlistmenu.Count() > 0 ? objlistmenu.FirstOrDefault().Id : 0;
                Model.MaxQty = _Util.Facade.GlobalSettingsFacade.GetMaxOrderingQuantityByCompanyId(currentLoggedIn.CompanyId.Value);
            }
            Model.WebsiteLocation = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(currentLoggedIn.CompanyId.Value);
            if (menuId.HasValue && menuId.Value > 0)
            {
                Model.Menus = _Util.Facade.MenuFacade.GetMenuById(menuId.Value);
            }
            #region ViewBag
            ViewBag.MenuLevel = _Util.Facade.LookupFacade.GetLookupByKey("MenuItemLevel").Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.DisplayText.ToString(),
                                 Value = x.DataValue.ToString()
                             }).ToList();
            ViewBag.selectMenuItemStatus = "1";
            if (Model.Status != null)
            {
                ViewBag.selectMenuItemStatus = Model.Status == true ? "1" : "0";
            }

            ViewBag.MenuItemStatus = _Util.Facade.LookupFacade.GetLookupByKey("MenuStatus").Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString()
                         }).ToList();
            List<Lookup> allDays = new List<Lookup>();
            List<Lookup> selectDropdown = new List<Lookup>();
            
            List<string> selectedDayList = new List<string>();
            
            if(Model.WebsiteLocation != null && !string.IsNullOrWhiteSpace(Model.WebsiteLocation.DaysAvailable))
            {
                var spdaysavail = Model.WebsiteLocation.DaysAvailable.Replace(" ", "").Split(',');
                if (spdaysavail.Length > 0)
                {
                    foreach (var item in spdaysavail)
                    {
                        allDays.Add(_Util.Facade.LookupFacade.GetLookupByKey("DaysAvailable").Where(x => x.DataValue == item).FirstOrDefault());
                    }
                }
                selectDropdown = allDays;
            }
            else
            {
                allDays = _Util.Facade.LookupFacade.GetLookupByKey("DaysAvailable");
                selectDropdown = allDays;
            }
            if (!string.IsNullOrWhiteSpace(Model.DaysAvailable) && Model.DaysAvailable != null)
            {
                selectedDayList = Model.DaysAvailable.Split(',').ToList();
                selectDropdown = allDays.Where(x => selectedDayList.Contains(x.DataValue)).ToList();
            }

            ViewBag.DaysAvailable = allDays.Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString(),
                             Selected = selectDropdown.Count(y => y.DataValue == x.DataValue) > 0
                         }).ToList();
            var mintime = _Util.Facade.GlobalSettingsFacade.GetMenuTimeAvailableStartTime(currentLoggedIn.CompanyId.Value);
            var maxtime = _Util.Facade.GlobalSettingsFacade.GetMenuTimeAvailableEndTime(currentLoggedIn.CompanyId.Value);
            string selectTimeFrom = mintime;
            string selectTimeTo = maxtime;
            if (!string.IsNullOrWhiteSpace(Model.TimeAvailable) && Model.TimeAvailable != null)
            {
                List<string> timeList = Model.TimeAvailable.Split(new[] { " to " }, StringSplitOptions.None).ToList();
                List<string> fromtimeList = timeList[0].Split(' ').ToList();
                List<string> totimeList = timeList[1].Split(' ').ToList();
                if (fromtimeList.Count > 2)
                {
                    selectTimeFrom = (Convert.ToDateTime(fromtimeList[1])).ToString("HH:mm");
                }
                else
                {
                    selectTimeFrom = (Convert.ToDateTime(fromtimeList[0])).ToString("HH:mm");
                }
                if (totimeList.Count > 2)
                {
                    selectTimeTo = (Convert.ToDateTime(totimeList[1])).ToString("HH:mm");
                }
                else
                {
                    selectTimeTo = (Convert.ToDateTime(totimeList[0])).ToString("HH:mm");
                }
            }
            List<SelectListItem> timeAvailable = new List<SelectListItem>();
            timeAvailable.AddRange(_Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", mintime, maxtime).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.timeAvailable = timeAvailable;
            ViewBag.selectTimeFromForItem = selectTimeFrom;
            ViewBag.selectTimeToForItem = selectTimeTo;
            ViewBag.storetimefrom = mintime;
            ViewBag.storetimeto = maxtime;
            //List<SelectListItem> ItemList = new List<SelectListItem>();
            //ItemList.Add(new SelectListItem()
            //{
            //    Text = "Please Select",
            //    Value = "-1"
            //});
            //ItemList.AddRange(_Util.Facade.MenuFacade
            //  .GetAllMenuItem().OrderBy(x => x.ItemName).Select(x =>
            //                   new SelectListItem()
            //                   {
            //                       Text = x.ItemName.ToString(),
            //                       Value = x.Id.ToString()
            //                   }).ToList());

            //ViewBag.MenuItemList = ItemList;

            List<SelectListItem> CategoryList = new List<SelectListItem>();
            //CategoryList.Add(new SelectListItem()
            //{
            //    Text = "Please Select",
            //    Value = "-1"
            //});
            if (Model.Categories == null)
            {
                Model.Categories = new List<RestCategory>();
            }
            CategoryList.AddRange(_Util.Facade.MenuFacade
              .GetAllCategory(currentLoggedIn.CompanyId.Value).OrderBy(x => x.CategoryName).Select(x =>
                               new SelectListItem()
                               {
                                   Text = x.CategoryName.ToString(),
                                   Value = x.Id.ToString(),
                                   Selected = Model.Categories.Count(y => y.Id == x.Id) > 0
                               }).ToList());

            if (CategoryList.Count() == 0)
            {
                CategoryList.Add(new SelectListItem()
                {
                    Text = "Please create a category",
                    Value = "-1"
                });
            }
            ViewBag.CategoryList = CategoryList;

            List<SelectListItem> ToppingList = new List<SelectListItem>();
            if (Model.Toppings == null)
            {
                Model.Toppings = new List<RestToppingCategory>();
            }
            ToppingList.AddRange(_Util.Facade.ToppingFacade
              .GetAllTopping(currentLoggedIn.CompanyId.Value).OrderBy(x => x.ToppingCategory).Select(x =>
                               new SelectListItem()
                               {
                                   Text = x.ToppingCategory.ToString(),
                                   Value = x.Id.ToString(),
                                   Selected = Model.Toppings.Count(y => y.Id == x.Id) > 0
                               }).ToList());

            if (ToppingList.Count() == 0)
            {
                ToppingList.Add(new SelectListItem()
                {
                    Text = "Please create a topping",
                    Value = "-1"
                });
            }

            //ViewBag.CategoryList = CategoryList;
            //List<SelectListItem> ToppingList = new List<SelectListItem>();
            //ToppingList.Add(new SelectListItem()
            //{
            //    Text = "Please select",
            //    Value = "-1"
            //});

            //ToppingList.AddRange(_Util.Facade.ToppingFacade
            //  .GetAllTopping(currentLoggedIn.CompanyId.Value).OrderBy(x => x.ToppingCategory).Select(x =>
            //                   new SelectListItem()
            //                   {
            //                       Text = x.ToppingCategory.ToString(),
            //                       Value = x.Id.ToString()
            //                   }).ToList());
            //ToppingList.Add(new SelectListItem()
            //{
            //    Text = "Create new topping",
            //    Value = "-2"
            //});
            ViewBag.ToppingList = ToppingList;
            #endregion
            List<string> dayavailable = new List<string>();
            foreach (var item in (List<SelectListItem>)ViewBag.DaysAvailable)
            {
                dayavailable.Add(item.Value);
            }
            ViewBag.dayavailable = dayavailable;
            ViewBag.edititem = edititem.HasValue ? edititem.Value : false;
            ViewBag.MenuId = menuId.HasValue ? menuId.Value : 0;
            
            List<SelectListItem> menulist = new List<SelectListItem>();
            menulist.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            menulist.AddRange(_Util.Facade.MenuFacade.GetAllMenuByCompanyId(currentLoggedIn.CompanyId.Value).Select(x => new SelectListItem()
            {
                Text = x.MenuName,
                Value = x.Id.ToString(),
            }).ToList());
            ViewBag.menulist = menulist;
            List<SelectListItem> IsSpecialInstruction = new List<SelectListItem>();
            IsSpecialInstruction.AddRange(_Util.Facade.LookupFacade.GetLookUpByKey("ContractYesNo").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            IsSpecialInstruction.RemoveAt(0);
            ViewBag.IsSpecialInstruction = IsSpecialInstruction;
            return PartialView("AddMenuItem", Model);
        }
        [Authorize]
        public PartialViewResult MenuItemListPartial(int? Id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<RestMenuItem> model = new List<RestMenuItem>();
            if (currentLoggedIn != null)
            {
                if (!Id.HasValue)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                else
                {
                    var objmenu = _Util.Facade.MenuFacade.GetMenuById(Id.Value);
                    if(objmenu != null)
                    {
                        model = _Util.Facade.MenuFacade.GetAllMenuItemByMenuId(objmenu.MenuId).ToList();
                    }
                    else
                    {
                        model = new List<RestMenuItem>();
                    }
                }
                ViewBag.WebLoc = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(currentLoggedIn.CompanyId.Value);
            }
            #region Viewbag
            //GlobalSetting HasKeyValue = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "On emergency contact do you want has key?");
            //if (HasKeyValue != null)
            //{
            //    ViewBag.HasKeyValue = HasKeyValue.Value;
            //}

            #endregion
            return PartialView("MenuItemListPartial", model);
        }

        [Authorize]
        public PartialViewResult AddCategory(int? Id)
        {
            RestCategory Model = new RestCategory();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (Id > 0)
            {
                Model = _Util.Facade.CategoryFacade.GetCategoryById(Id.Value);
                if (!string.IsNullOrWhiteSpace(Model.DaysAvailable))
                {
                    Model.DaysAvailable = Model.DaysAvailable.Replace(", ", ",");
                    var spadays = Model.DaysAvailable.Split(',');
                    Model.AvailableDays = spadays.ToList();
                }
            }
            Model.WebsiteLocation = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(currentLoggedIn.CompanyId.Value);
            #region ViewBag
            ViewBag.CategoryStatus = _Util.Facade.LookupFacade.GetLookupByKey("CategoryStatus").Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString()
                         }).ToList();
            ViewBag.selectCategoryStatus = "1";
            if (Model.Status != null)
            {
                ViewBag.selectCategoryStatus = Model.Status == true ? "1" : "0";
            }
            var objwebloc = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(currentLoggedIn.CompanyId.Value);
            List<Lookup> allDays = new List<Lookup>();
            List<Lookup> selectDropdown = new List<Lookup>();

            List<string> selectedDayList = new List<string>();
            
            if(objwebloc != null && !string.IsNullOrWhiteSpace(objwebloc.DaysAvailable))
            {
                var spdaysavail = Model.WebsiteLocation.DaysAvailable.Replace(" ", "").Split(',');
                if (spdaysavail.Length > 0)
                {
                    foreach (var item in spdaysavail)
                    {
                        allDays.Add(_Util.Facade.LookupFacade.GetLookupByKey("DaysAvailable").Where(x => x.DataValue == item).FirstOrDefault());
                    }
                }
                selectDropdown = allDays;
            }
            else
            {
                allDays = _Util.Facade.LookupFacade.GetLookupByKey("DaysAvailable");
                selectDropdown = allDays;
            }
            if (!string.IsNullOrWhiteSpace(Model.DaysAvailable) && Model.DaysAvailable != null)
            {
                selectedDayList = Model.DaysAvailable.Split(',').ToList();
                selectDropdown = allDays.Where(x => selectedDayList.Contains(x.DataValue)).ToList();
            }

            ViewBag.DaysAvailable = allDays.Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString(),
                             Selected = selectDropdown.Count(y => y.DataValue == x.DataValue) > 0
                         }).ToList();

            var mintime = _Util.Facade.GlobalSettingsFacade.GetMenuTimeAvailableStartTime(currentLoggedIn.CompanyId.Value);
            var maxtime = _Util.Facade.GlobalSettingsFacade.GetMenuTimeAvailableEndTime(currentLoggedIn.CompanyId.Value);
            string selectTimeFrom = mintime;
            string selectTimeTo = maxtime;
            if (!string.IsNullOrWhiteSpace(Model.TimeAvailable) && Model.TimeAvailable != null)
            {
                List<string> timeList = Model.TimeAvailable.Split(new[] { " to " }, StringSplitOptions.None).ToList();
                List<string> fromtimeList = timeList[0].Split(' ').ToList();
                List<string> totimeList = timeList[1].Split(' ').ToList();
                if(fromtimeList.Count > 2)
                {
                    selectTimeFrom = (Convert.ToDateTime(fromtimeList[1])).ToString("HH:mm");
                }
                else
                {
                    selectTimeFrom = (Convert.ToDateTime(fromtimeList[0])).ToString("HH:mm");
                }
                if (totimeList.Count > 2)
                {
                    selectTimeTo = (Convert.ToDateTime(totimeList[1])).ToString("HH:mm");
                }
                else
                {
                    selectTimeTo = (Convert.ToDateTime(totimeList[0])).ToString("HH:mm");
                }
            }
            List<SelectListItem> timeAvailable = new List<SelectListItem>();
            timeAvailable.AddRange(_Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", mintime, maxtime).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.timeAvailable = timeAvailable;
            ViewBag.selectTimeFrom = selectTimeFrom;
            ViewBag.selectTimeTo = selectTimeTo;
            ViewBag.storetimefrom = mintime;
            ViewBag.storetimeto = maxtime;
            #endregion
            List<string> dayavailable = new List<string>();
            foreach (var item in (List<SelectListItem>)ViewBag.DaysAvailable)
            {
                dayavailable.Add(item.Value);
            }
            ViewBag.dayavailable = dayavailable;
            return PartialView("_AddCategory", Model);
        }
        [Authorize]
        public PartialViewResult AddTopping(int? ToppingCategoryId)
        {
            ToppingListModel Model = new ToppingListModel();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (ToppingCategoryId > 0)
            {
                Model.ToppingCategoryModel = _Util.Facade.ToppingFacade.GetToppingCategoryById(ToppingCategoryId.Value);
                if (Model.ToppingCategoryModel != null)
                {
                    Model.Toppings = _Util.Facade.ToppingFacade.GetToppingListByCategoryId(Model.ToppingCategoryModel.ToppingCategoryId);
                }
            }
            return PartialView("_AddTopping", Model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddCategory(RestCategory category)
        {
            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(result);
            }
            string time = "";
            time = category.TimeFrom.ToString() != "" ? LabelHelper.TimeFormatUsingDateTime.For12Hours(DateTime.Today.Add(category.TimeFrom)) + " to " : " to ";
            time = category.TimeTo.ToString() != "" ? time + LabelHelper.TimeFormatUsingDateTime.For12Hours(DateTime.Today.Add(category.TimeTo)) : "";
            category.TimeAvailable = time;
            category.LastUpdatedBy = currentLoggedIn.UserId;
            category.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            if (!string.IsNullOrWhiteSpace(category.DaysAvailable))
            {
                category.DaysAvailable = category.DaysAvailable.Replace(",", ", ");
            }
            if (category.Id > 0)
            {
                #region update category
                RestCategory c = _Util.Facade.CategoryFacade.GetCategoryById(category.Id);
                category.CreatedBy = c.CreatedBy;
                category.CreatedDate = c.CreatedDate;
                category.CompanyId = c.CompanyId;
                category.CategoryId = c.CategoryId;
                category.OrderBy = c.OrderBy;
                result = _Util.Facade.CategoryFacade.UpdateCategory(category);
                #endregion
            }
            else
            {
                category.CreatedDate = DateTime.Now.UTCCurrentTime();
                category.CreatedBy = currentLoggedIn.UserId;
                category.CompanyId = currentLoggedIn.CompanyId.Value;
                category.CategoryId = Guid.NewGuid();
                category.OrderBy = 0;
                result = _Util.Facade.CategoryFacade.InsertCategory(category) > 0;

            }
            #region Menu Item Category
            var objmic = _Util.Facade.MenuFacade.GetAllMenuItemCategoryByCategoryId(category.CategoryId);
            if(objmic != null && objmic.Count > 0)
            {
                foreach(var item in objmic)
                {
                    var objmenu = _Util.Facade.MenuFacade.GetMenuByMenuId(item.MenuId);
                    var objmenuitem = _Util.Facade.MenuFacade.GetMenuItemByItemId(item.ItemId);
                    var objmicurl = _Util.Facade.MenuFacade.GetMICUrlByMenuIdAndItemId(objmenu.Id, (objmenuitem != null ? objmenuitem.Id : 0));
                    if(objmicurl != null)
                    {
                        objmicurl.MenuCategoryURL = objmenu.WebsiteURL + "/" + category.UrlSlug;
                        objmicurl.ItemCategoryURL = objmenu.WebsiteURL + "/" + category.UrlSlug + "/" + objmenuitem.UrlSlug;
                        _Util.Facade.MenuFacade.UpdateRestMenuItemCategoryURL(objmicurl);
                    }
                }
            }
            #endregion
            return Json(new { result = result, id = category.Id });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddMenu(RestMenu menu)
        {
            var result = false;
            int menuId;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(result);
            }
            string time = "";
            time = menu.TimeFrom.ToString() != "" ? LabelHelper.TimeFormatUsingDateTime.For12Hours(DateTime.Today.Add(menu.TimeFrom)) + " to " : " to ";
            time = menu.TimeTo.ToString() != "" ? time + LabelHelper.TimeFormatUsingDateTime.For12Hours(DateTime.Today.Add(menu.TimeTo)) : "";
            menu.TimeAvailable = time;
            menu.LastUpdatedBy = currentLoggedIn.UserId;
            menu.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            if (!string.IsNullOrWhiteSpace(menu.DaysAvailable))
            {
                menu.DaysAvailable = menu.DaysAvailable.Replace(",", ", ");
            }
            if (menu.Id > 0)
            {
                #region update menu
                menuId = menu.Id;
                RestMenu m = _Util.Facade.MenuFacade.GetMenuById(menu.Id);
                menu.CreatedBy = m.CreatedBy;
                menu.CreatedDate = m.CreatedDate;
                menu.CompanyId = m.CompanyId;
                menu.MenuId = m.MenuId;
                result = _Util.Facade.MenuFacade.UpdateMenu(menu);
                #endregion
            }
            else
            {
                #region insert menu
                menu.CreatedDate = DateTime.Now.UTCCurrentTime();
                menu.CreatedBy = currentLoggedIn.UserId;
                menu.CompanyId = currentLoggedIn.CompanyId.Value;
                menu.MenuId = Guid.NewGuid();
                menuId = _Util.Facade.MenuFacade.InsertMenu(menu);
                result = menuId > 0;
                #endregion
            }
            return Json(new { result = result, menuId = menuId });
        }
        [HttpPost]
        public JsonResult AddMenuItem(RestMenuItem _MenuItem, List<RestMenuItemAdditionalContent> AdditionalContent)
        {
            var result = false;
            int menuItemId = 0;
            var message = "";
            //if(_MenuItem.TaxPercentage > 0)
            //{
            //    _MenuItem.IsTax = true;
            //}
            //else
            //{
            //    _MenuItem.IsTax = false;
            //}
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            if (!string.IsNullOrWhiteSpace(_MenuItem.DaysAvailable))
            {
                _MenuItem.DaysAvailable = _MenuItem.DaysAvailable.Replace(",", ", ");
            }
            if (currentLoggedIn != null)
            {
                string time = "";
                time = _MenuItem.TimeFrom.ToString() != "" ? DateTime.Today.Add(_MenuItem.TimeFrom).ToString("hh:mm tt") + " to " : " to ";
                time = _MenuItem.TimeTo.ToString() != "" ? time + DateTime.Today.Add(_MenuItem.TimeTo).ToString("hh:mm tt") : "";
                _MenuItem.TimeAvailable = time;
                _MenuItem.LastUpdatedBy = currentLoggedIn.UserId;
                _MenuItem.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                if (_MenuItem.Id > 0)
                {
                    #region update menu item
                    menuItemId = _MenuItem.Id;
                    RestMenuItem mi = _Util.Facade.MenuFacade.GetMenuItemById(_MenuItem.Id);
                    _MenuItem.CompanyId = mi.CompanyId;
                    _MenuItem.ItemId = mi.ItemId;
                    _MenuItem.OrderBy = mi.OrderBy;
                    _MenuItem.CreatedBy = mi.CreatedBy;
                    _MenuItem.CreatedDate = mi.CreatedDate;
                    result = _Util.Facade.MenuFacade.UpdateMenuItem(_MenuItem);
                    message = "Menu item saved successfully";
                    #endregion
                }
                else
                {
                    #region insert menu item
                    _MenuItem.CompanyId = currentLoggedIn.CompanyId.Value;
                    _MenuItem.CreatedBy = currentLoggedIn.UserId;
                    _MenuItem.CreatedDate = DateTime.Now.UTCCurrentTime();
                    _MenuItem.ItemId = Guid.NewGuid();
                    _MenuItem.OrderBy = 0;
                    menuItemId = _Util.Facade.MenuFacade.InsertMenuItem(_MenuItem);
                    result = menuItemId > 0;
                    message = "Menu item saved successfully";
                    #endregion
                }

                #region delete category detail & toppings
                var objmenu = _Util.Facade.MenuFacade.GetMenuById(_MenuItem.PrevMenuId);
                if(objmenu != null)
                {
                    _Util.Facade.CategoryFacade.DeleteAllMenuItemCategoryByMenuIdAndItemId(objmenu.MenuId, _MenuItem.ItemId, objmenu.Id, _MenuItem.Id);
                }
                

                #endregion

                #region insert category detail & toppings
                var _objmenu = _Util.Facade.MenuFacade.GetMenuById(_MenuItem.MenuId);
                if(_objmenu != null)
                {
                    foreach (var item in _MenuItem.CategoryNameList)
                    {
                        var objcategory = _Util.Facade.CategoryFacade.GetCategoryById(Convert.ToInt32(item));
                        if (objcategory != null)
                        {
                            RestMenuItemCategory RestMenuItemCategory = new RestMenuItemCategory()
                            {
                                MenuId = _objmenu.MenuId,
                                ItemId = _MenuItem.ItemId,
                                CategoryId = objcategory.CategoryId,
                                CompanyId = currentLoggedIn.CompanyId.Value,
                                CreatedBy = currentLoggedIn.UserId,
                                CreatedDate = DateTime.Now.UTCCurrentTime()
                            };
                            _Util.Facade.MenuFacade.InsertMenuItemDetail(RestMenuItemCategory);
                            _Util.Facade.MenuFacade.DeleteAllMenuDetailByMenuIdAndCategoryId(objmenu != null ? objmenu.MenuId : new Guid(), objcategory.CategoryId);
                            RestMenuCategory RestMenuCategory = new RestMenuCategory()
                            {
                                CompanyId = currentLoggedIn.CompanyId.Value,
                                MenuId = _objmenu.MenuId,
                                CategoryId = objcategory.CategoryId,
                                CreatedBy = currentLoggedIn.UserId,
                                CreatedDate = DateTime.Now.UTCCurrentTime()
                            };
                            _Util.Facade.MenuFacade.InsertMenuItemCategory(RestMenuCategory);
                        }
                    }
                    foreach (var item in _MenuItem.ToppingNameList)
                    {
                        var objtopcat = _Util.Facade.ToppingFacade.GetToppingCategoryById(Convert.ToInt32(item));
                        if (objtopcat != null)
                        {
                            RestMenuItemCategoryTopping RestMenuItemCategoryTopping = new RestMenuItemCategoryTopping()
                            {
                                CompanyId = currentLoggedIn.CompanyId.Value,
                                MenuId = _objmenu.MenuId,
                                ToppingCategoryId = objtopcat.ToppingCategoryId,
                                ItemId = _MenuItem.ItemId,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CreatedBy = currentLoggedIn.UserId
                            };
                            _Util.Facade.MenuFacade.InsertMenuDetail(RestMenuItemCategoryTopping);
                        }
                    }
                }
                
                #endregion
            }
            if (result)
            {
                var menuobj = _Util.Facade.MenuFacade.GetMenuById(_MenuItem.MenuId);
                if(menuobj != null)
                {
                    if(_MenuItem.CategoryNameList.Count > 0)
                    {
                        foreach(var item in _MenuItem.CategoryNameList)
                        {
                            var categoryobj = _Util.Facade.CategoryFacade.GetCategoryById(Convert.ToInt32(item));
                            if(categoryobj != null)
                            {
                                RestMenuItemCategoryURL MenuItemCategoryURL = new RestMenuItemCategoryURL()
                                {
                                    MenuId = _MenuItem.MenuId,
                                    MenuItemId = menuItemId,
                                    MenuCategoryURL = menuobj.WebsiteURL + "/" + categoryobj.UrlSlug,
                                    ItemCategoryURL = menuobj.WebsiteURL + "/" + categoryobj.UrlSlug + "/" + _MenuItem.UrlSlug,
                                    CreatedBy = currentLoggedIn.UserId,
                                    CreatedDate = DateTime.Now.UTCCurrentTime()
                                };
                                _Util.Facade.MenuFacade.InsertMenuItemCategoryURL(MenuItemCategoryURL);
                            }
                        }
                    }
                }
                if(AdditionalContent != null && AdditionalContent.Count > 0)
                {
                    _Util.Facade.MenuFacade.DeleteAllAdditionalContentByItemId(_MenuItem.ItemId);
                    foreach (var item in AdditionalContent)
                    {
                        RestMenuItemAdditionalContent RestMenuItemAdditionalContent = new RestMenuItemAdditionalContent()
                        {
                            ItemId = _MenuItem.ItemId,
                            Name = item.Name,
                            ImageLoc = item.ImageLoc,
                            CreatedBy = currentLoggedIn.UserId,
                            CreatedDate = DateTime.Now.UTCCurrentTime()
                        };
                        _Util.Facade.MenuFacade.InsertAdditionalContent(RestMenuItemAdditionalContent);
                    }
                }
            }
            return Json(new { result = result, message = message, menuid = _MenuItem.MenuId, id = _MenuItem.Id });
        }
        [HttpPost]
        public JsonResult DeleteMenu(int Id)
        {
            var objmenu = _Util.Facade.MenuFacade.GetMenuById(Id);
            var res = _Util.Facade.MenuFacade.DeleteMenuById(Id);
            if (res)
            {
                List<RestMenuItem> mid = new List<RestMenuItem>();
                
                mid = _Util.Facade.MenuFacade.GetAllMenuItemByMenuId(objmenu.MenuId);
                foreach(var item in mid)
                {
                    _Util.Facade.MenuFacade.DeleteMenuItemById(item.Id);
                    //_Util.Facade.CategoryFacade.DeleteAllCategoryDetailsByMenuId(item.MenuId, item);
                    _Util.Facade.MenuFacade.DeleteAllMenuDetailByMenuId(objmenu.MenuId);
                    _Util.Facade.MenuFacade.DeleteAllMenuItemDetailByMenuItemId(item.ItemId);
                }
                return Json(new { result = res, message = "Menu deleted successfully." });
            }
            else
            {
                return Json(new { result = res, message = "An error occured." });
            }

        }
        [HttpPost]
        public JsonResult DeleteMenuItem(int Id)
        {
            var itemobj = _Util.Facade.MenuFacade.GetMenuItemById(Id);
            var res = _Util.Facade.MenuFacade.DeleteMenuItemById(Id);
            if (res)
            {
                _Util.Facade.MenuFacade.DeleteMenuItemAndRelation(itemobj.ItemId);
                return Json(new { result = res, message = "Menu item deleted successfully." });
            }
            else
            {
                return Json(new { result = res, message = "An error occured." });
            }

        }
        [HttpPost]
        public JsonResult DeleteCategory(int Id)
        {
            var objcategory = _Util.Facade.MenuFacade.GetCategoryById(Id);
            if(objcategory != null)
            {
                var objmenucat = _Util.Facade.MenuFacade.GetMenuItemCatgoryByCategoryId(objcategory.CategoryId);
                if(objmenucat == null)
                {
                    var res = _Util.Facade.CategoryFacade.DeleteCategoryById(Id);
                    if (res)
                    {
                        return Json(new { result = res, message = "Category deleted successfully." });
                    }
                    else
                    {
                        return Json(new { result = res, message = "An error occured." });
                    }
                }
                else
                {
                    return Json(new { result = false, message = "An error occured." });
                }
            }
            else
            {
                return Json(new { result = false, message = "An error occured." });
            }
        }
        [HttpPost]
        public JsonResult DeleteTopping(int Id)
        {
            var objtoppingcat = _Util.Facade.ToppingFacade.GetToppingCategoryById(Id);
            if(objtoppingcat != null)
            {
                var objitemcattop = _Util.Facade.MenuFacade.GetMenuItemCatgoryToppingByCategoryId(objtoppingcat.ToppingCategoryId);
                if(objitemcattop == null)
                {
                    var res = _Util.Facade.ToppingFacade.DeleteToppingCategoryById(Id);
                    if (res)
                    {
                        _Util.Facade.ToppingFacade.DeleteAllToppingsByToppingCategoryId(objtoppingcat.ToppingCategoryId);
                        return Json(new { result = res, message = "Topping deleted successfully." });
                    }
                    else
                    {
                        return Json(new { result = res, message = "An error occured." });
                    }
                }
                else
                {
                    return Json(new { result = false, message = "An error occured." });
                }
            }
            else
            {
                return Json(new { result = false, message = "An error occured." });
            }
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddTopping(ToppingListModel toppingCategory)
        {

            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(result);
            }
            int tCategoryId;
            Guid topCategoryId = new Guid();
            //ToppingCategory.Toppings.LastUpdatedBy = currentLoggedIn.UserId;
            //ToppingCategory.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            if (toppingCategory.ToppingCategoryModel.Id > 0)
            {
                #region update topping category
                RestToppingCategory tc = _Util.Facade.ToppingFacade.GetToppingCategoryById(toppingCategory.ToppingCategoryModel.Id);
                toppingCategory.ToppingCategoryModel.CompanyId = tc.CompanyId;
                toppingCategory.ToppingCategoryModel.ToppingCategoryId = tc.ToppingCategoryId;
                result = _Util.Facade.ToppingFacade.UpdateToppingCategory(toppingCategory.ToppingCategoryModel);
                tCategoryId = toppingCategory.ToppingCategoryModel.Id;
                topCategoryId = toppingCategory.ToppingCategoryModel.ToppingCategoryId;
                #endregion

                #region delete topping
                _Util.Facade.ToppingFacade.DeleteAllToppingsByToppingCategoryId(toppingCategory.ToppingCategoryModel.ToppingCategoryId);
                #endregion
            }
            else
            {
                toppingCategory.ToppingCategoryModel.CompanyId = currentLoggedIn.CompanyId.Value;
                toppingCategory.ToppingCategoryModel.ToppingCategoryId = Guid.NewGuid();
                topCategoryId = toppingCategory.ToppingCategoryModel.ToppingCategoryId;
                tCategoryId = _Util.Facade.ToppingFacade.InsertToppingCategory(toppingCategory.ToppingCategoryModel);
                result = tCategoryId > 0;
            }

            #region insert topping
            if (toppingCategory.Toppings != null && toppingCategory.Toppings.Count()>0 )
            {
                foreach (var item in toppingCategory.Toppings)
                {
                    RestTopping t = new RestTopping();
                    if (item.Price == null)
                    {
                        t.Price = 0;
                    }
                    else
                    {
                        t.Price = item.Price;
                    }
                    if(!string.IsNullOrWhiteSpace(item.ToppingName) && item.ToppingName!="")
                    {
                        t.ToppingName = item.ToppingName;
                    }
                    else
                    {
                        t.ToppingName = toppingCategory.ToppingCategoryModel.ToppingCategory;
                    }
                    t.ToppingCategoryId = topCategoryId;
                    t.Description = item.Description;
                    t.IsAvailable = item.IsAvailable;
                    t.LastUpdatedBy = currentLoggedIn.UserId;
                    t.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    t.CreatedBy = currentLoggedIn.UserId;
                    t.CreatedDate = DateTime.Now.UTCCurrentTime();
                    t.CompanyId = currentLoggedIn.CompanyId.Value;
                    t.ToppingId = Guid.NewGuid();
                    t.IsDefault = item.IsDefault;
                    _Util.Facade.ToppingFacade.InsertTopping(t);
                }
            }
            #endregion
            return Json(new { result = result, id = toppingCategory.ToppingCategoryModel.Id });
        }
        [Authorize]
        public JsonResult GetMenuItemListByKey(string key, string ExistItem = "")
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetMenuItemSearchMaxLoad(CurrentUser.CompanyId.Value);

                List<RestMenuItem> miList = _Util.Facade.MenuFacade.GetMenuItemListBySearchKey(key, ItemsLoadCount, ExistItem);
                if (miList.Count > 0)
                    result = JsonConvert.SerializeObject(miList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetToppingByToppingCategoryId(int tcid)
        {
            string result = "[]";
            string categoryName = "";
            if (tcid > 0)
            {
                var objtopcategory = _Util.Facade.ToppingFacade.GetToppingCategoryById(tcid);
                List<RestTopping> topping = _Util.Facade.ToppingFacade.GetToppingListByCategoryId(objtopcategory.ToppingCategoryId);
                categoryName = topping.FirstOrDefault().ToppingCategoryName;
                if (topping.Count > 0)
                    result = JsonConvert.SerializeObject(topping);
            }

            return Json(new { result = result, categoryName = categoryName }, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadMenuPhoto()
        {
            bool isUploaded = false;
            int width = 0;
            int height = 0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBaseMenu = Request.Files["CustomerFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.MenuPhoto"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBaseMenu.FileName;
            string tempFolderPath = "";
            if (httpPostedFileBaseMenu != null && httpPostedFileBaseMenu.ContentLength != 0)
            {

                tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBaseMenu.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }
            
            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            return Json(new { isUploaded = isUploaded, FullFilePath = AppConfig.DomainSitePath + filePath }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadSiteConfigurationTemplate()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBaseMenu = Request.Files["CustomerFile"];
            string NameFile = "";
            string tempFolderName = ConfigurationManager.AppSettings["File.SiteConfigTemplate"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBaseMenu.FileName;

            if (httpPostedFileBaseMenu != null && httpPostedFileBaseMenu.ContentLength != 0)
            {
                NameFile = httpPostedFileBaseMenu.FileName;
                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBaseMenu.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            return Json(new { isUploaded = isUploaded, FullFilePath = AppConfig.DomainSitePath + filePath, NameFile = NameFile }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadMenuItemPhoto()
        {
            bool isUploaded = false;
            int width = 0;
            int height = 0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBaseMenu = Request.Files["CustomerFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.MenuItemPhoto"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBaseMenu.FileName;
            string tempFolderPath = "";
            if (httpPostedFileBaseMenu != null && httpPostedFileBaseMenu.ContentLength != 0)
            {

                tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBaseMenu.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }
            
            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            return Json(new { isUploaded = isUploaded, FullFilePath = AppConfig.DomainSitePath + filePath }, "text/html");
        }
        [Authorize]
        [HttpPost]
        public ActionResult UploadCategoryPhoto()
        {
            bool isUploaded = false;
            int width = 0;
            int height = 0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBaseCategory = Request.Files["CustomerFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.CategoryPhoto"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBaseCategory.FileName;
            string tempFolderPath = "";
            if (httpPostedFileBaseCategory != null && httpPostedFileBaseCategory.ContentLength != 0)
            {

                tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBaseCategory.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }
            
            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            return Json(new { isUploaded = isUploaded, FullFilePath = AppConfig.DomainSitePath + filePath }, "text/html");
        }

        [Authorize]
        public ActionResult AllMenuItemListPartial(int? PageNo, int? PageSize, string SearchText, string order)
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

            MenuListModel Model = _Util.Facade.MenuFacade.GetAllMenuItemList(CurrentUser.CompanyId.Value, PageNo.Value, PageSize.Value, SearchText, order);
            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = order;
            TempData["data"] = SearchText;


            if (Model.Menus.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCount;
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

            return PartialView(Model);
        }

        public ActionResult AllMenuItemDetail(int? id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            AllMenuItemModel model = new AllMenuItemModel();
            if (id.HasValue && id.Value > 0)
            {
                model.MenuList = _Util.Facade.MenuFacade.GetMenuListByMenuIdAndCompanyId(CurrentUser.CompanyId.Value, id.Value);
                model.MenuItemList = _Util.Facade.MenuFacade.GetMenuItemListByMenuIdAndCompanyId(CurrentUser.CompanyId.Value, id.Value);
                model.CategoryList = _Util.Facade.MenuFacade.GetCategoryListByMenuIdAndCompanyId(CurrentUser.CompanyId.Value, id.Value);
                model.ToppingCategoryList = _Util.Facade.MenuFacade.GetToppingCategoryListByMenuIdAndCompanyId(CurrentUser.CompanyId.Value, id.Value);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult DeleteAllMenuItemByMenuId(int? MenuId)
        {
            string message = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            if(MenuId.HasValue && MenuId.Value > 0)
            {
                var objmenu = _Util.Facade.MenuFacade.GetMenuByIdAndCompanyId(MenuId.Value, CurrentUser.CompanyId.Value);
                if(objmenu != null)
                {
                    var itemcount = _Util.Facade.MenuFacade.GetAllMenuItemCategoryByMenuId(objmenu.MenuId);
                    if(itemcount != null && itemcount.Count > 0)
                    {
                        message = "Menu should not be deleted";
                    }
                    else
                    {
                        result = _Util.Facade.MenuFacade.DeleteMenuById(MenuId.Value);
                        _Util.Facade.MenuFacade.DeleteMenuRelation(objmenu.MenuId);
                        message = "Menu deleted successfully";
                    }
                }
            }
            return Json(new { result = result, message = message });
        }

        [HttpPost]
        public JsonResult DeleteCategoryById(int? MenuId)
        {
            var objcategory = _Util.Facade.MenuFacade.GetCategoryById(MenuId.Value);
            if (objcategory != null)
            {
                var objmenucat = _Util.Facade.MenuFacade.GetMenuItemCatgoryByCategoryId(objcategory.CategoryId);
                if (objmenucat == null)
                {
                    var res = _Util.Facade.CategoryFacade.DeleteCategoryById(MenuId.Value);
                    if (res)
                    {
                        return Json(new { result = res, message = "Category deleted successfully." });
                    }
                    else
                    {
                        return Json(new { result = res, message = "An error occured." });
                    }
                }
                else
                {
                    return Json(new { result = false, message = "An error occured." });
                }
            }
            else
            {
                return Json(new { result = false, message = "An error occured." });
            }
        }

        [HttpPost]
        public JsonResult DeleteToppingById(int? MenuId)
        {
            var objtoppingcat = _Util.Facade.ToppingFacade.GetToppingCategoryById(MenuId.Value);
            if (objtoppingcat != null)
            {
                var objitemcattop = _Util.Facade.MenuFacade.GetMenuItemCatgoryToppingByCategoryId(objtoppingcat.ToppingCategoryId);
                if (objitemcattop == null)
                {
                    var res = _Util.Facade.ToppingFacade.DeleteToppingCategoryById(MenuId.Value);
                    if (res)
                    {
                        _Util.Facade.ToppingFacade.DeleteAllToppingsByToppingCategoryId(objtoppingcat.ToppingCategoryId);
                        return Json(new { result = res, message = "Topping deleted successfully." });
                    }
                    else
                    {
                        return Json(new { result = res, message = "An error occured." });
                    }
                }
                else
                {
                    return Json(new { result = false, message = "An error occured." });
                }
            }
            else
            {
                return Json(new { result = false, message = "An error occured." });
            }
        }

        public ActionResult CategoryItemListPartial(int? categoryid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<RestMenuItem> MenuItem = new List<RestMenuItem>();
            if(categoryid.HasValue && categoryid.Value > 0)
            {
                MenuItem = _Util.Facade.CategoryFacade.GetCategoryItemListPartial(CurrentUser.CompanyId.Value, categoryid.Value);
            }
            ViewBag.WebLoc = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(CurrentUser.CompanyId.Value);
            return View(MenuItem);
        }

        public ActionResult ToppingListPartial(int? toppingcateoryid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<RestMenuItem> Topping = new List<RestMenuItem>();
            if(toppingcateoryid.HasValue && toppingcateoryid.Value > 0)
            {
                var objtopcategory = _Util.Facade.ToppingFacade.GetToppingCategoryById(toppingcateoryid.Value);
                Topping = _Util.Facade.ToppingFacade.GetToppingListPartial(CurrentUser.CompanyId.Value, objtopcategory.ToppingCategoryId);
            }
            ViewBag.WebLoc = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(CurrentUser.CompanyId.Value);
            return View(Topping);
        }

        public ActionResult LoadMenuItemsPartial(int? PageNo, int? PageSize, string SearchText, string order)
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

            MenuItemListModel Model = _Util.Facade.CategoryFacade.GetMenuItemListByCompanyId(CurrentUser.CompanyId.Value, PageNo.Value, PageSize.Value, SearchText, order);
            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = order;

            if (Model.MenuItems.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCount;
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
            return View(Model);
        }

        public ActionResult AllItemsTabPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ItemReportsModel ItemReportsModel = new ItemReportsModel();
            List<SelectListItem> menulist = new List<SelectListItem>();
            menulist.AddRange(_Util.Facade.MenuFacade.GetAllMenuByCompanyId(CurrentUser.CompanyId.Value).Select(x => new SelectListItem()
            {
                Text = x.MenuName,
                Value = x.Id.ToString()
            }).ToList());
            ViewBag.menulist = menulist;
            List<SelectListItem> categorylist = new List<SelectListItem>();
            categorylist.AddRange(_Util.Facade.MenuFacade.GetAllCategory(CurrentUser.CompanyId.Value).Select(x => new SelectListItem()
            {
                Text = x.CategoryName,
                Value = x.Id.ToString()
            }).ToList());
            ViewBag.categorylist = categorylist;
            ItemReportsModel = _Util.Facade.MenuFacade.GetItemsReportByCompanyId(CurrentUser.CompanyId.Value);
            List<string> listmenuid = new List<string>();
            List<string> listcategoryid = new List<string>();
            
            
            if (Session["MenuList"] != null)
            {
                if (!string.IsNullOrWhiteSpace(Session["MenuList"].ToString()) && Session["MenuList"].ToString() != "null")
                {
                    string[] splituser = Session["MenuList"].ToString().Split(',');
                    if (splituser.Length > 0)
                    {
                        foreach (var item in splituser)
                        {
                            listmenuid.Add(item);
                        }
                    }
                }
                if(listmenuid.Count > 0)
                {
                    ViewBag.listmenu = listmenuid;
                }
            }
            if (Session["TextSearch"] != null)
            {
                ViewBag.TextSearch = Session["TextSearch"].ToString();
            }
            if (Session["categoryList"] != null)
            {
                if (!string.IsNullOrWhiteSpace(Session["categoryList"].ToString()) && Session["categoryList"].ToString() != "null")
                {
                    string[] splituser = Session["categoryList"].ToString().Split(',');
                    if (splituser.Length > 0)
                    {
                        foreach (var item in splituser)
                        {
                            listcategoryid.Add(item);
                        }
                    }
                }
                if(listcategoryid.Count > 0)
                {
                    ViewBag.listcategory = listcategoryid;
                }
                
            }
            return View(ItemReportsModel);
        }

        public ActionResult AllItemsTabListPartial(string MenuList, string TextSearch, string categoryList, bool? IsStatus)
        {
            Session["MenuList"] = MenuList;
            Session["TextSearch"] = TextSearch;
            Session["categoryList"] = categoryList;
            List<RestMenuItem> model = new List<RestMenuItem>();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<string> listmenuid = new List<string>();
            List<string> listcategoryid = new List<string>();
            if (!string.IsNullOrWhiteSpace(MenuList) && MenuList != "null")
            {
                string[] splituser = MenuList.Split(',');
                if (splituser.Length > 0)
                {
                    foreach(var item in splituser)
                    {
                        var objmenu = _Util.Facade.MenuFacade.GetMenuById(Convert.ToInt32(item));
                        listmenuid.Add(objmenu.MenuId.ToString());
                    }
                    MenuList = string.Format("'{0}'", string.Join("','", listmenuid));
                }
            }
            if (!string.IsNullOrWhiteSpace(categoryList) && categoryList != "null")
            {
                string[] splituser = categoryList.Split(',');
                if (splituser.Length > 0)
                {
                    foreach (var item in splituser)
                    {
                        var objcategory = _Util.Facade.MenuFacade.GetCategoryById(Convert.ToInt32(item));
                        listcategoryid.Add(objcategory.CategoryId.ToString());
                    }
                    categoryList = string.Format("'{0}'", string.Join("','", listcategoryid));
                }
            }
            ViewBag.WebLoc = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(CurrentUser.CompanyId.Value);
            model = _Util.Facade.MenuFacade.GetAllMenuItemByCompanyId(CurrentUser.CompanyId.Value, MenuList, TextSearch, categoryList, (IsStatus.HasValue ? IsStatus.Value : false));
            
            return View(model);
        }

        [HttpPost]
        public JsonResult GetURLSlug(string key)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(key))
            {
                key = key.Trim(' ');
                key = key.Replace("'", "").Replace(",", "").Replace(":", "");
                key = Regex.Replace(key, @"[^0-9a-zA-Z]+", "-").ToLower();
                result = true;
            }
            return Json(new { result = result, str = key });
        }

        [HttpPost]
        public JsonResult UpdateIeateryCategory(List<IeateryCategoryCustomModel> categoryArr)
        {
            bool result = false;
            if(categoryArr != null && categoryArr.Count > 0)
            {
                foreach(var item in categoryArr)
                {
                    var objcategory = _Util.Facade.CategoryFacade.GetCategoryById(item.id);
                    if(objcategory != null)
                    {
                        objcategory.OrderBy = item.orderid;
                        result = _Util.Facade.CategoryFacade.UpdateCategory(objcategory);
                    }
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateIeateryMenuItem(List<IeateryCategoryCustomModel> categoryArr)
        {
            bool result = false;
            if (categoryArr != null && categoryArr.Count > 0)
            {
                foreach (var item in categoryArr)
                {
                    var objmenuitem = _Util.Facade.MenuFacade.GetMenuItemById(item.id);
                    if (objmenuitem != null)
                    {
                        objmenuitem.OrderBy = item.orderid;
                        result = _Util.Facade.MenuFacade.UpdateMenuItem(objmenuitem);
                    }
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetItemUrlSlugPermission(int? id, string urlslug)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            if (id.HasValue && id.Value > 0 && !string.IsNullOrWhiteSpace(urlslug))
            {
                var objitem = _Util.Facade.MenuFacade.GetMenuItemById(id.Value);
                if (objitem != null && objitem.UrlSlug == urlslug)
                {
                    var itemlist = _Util.Facade.MenuFacade.GetMenuItemByUrlSlug(urlslug, CurrentUser.CompanyId.Value);
                    if (itemlist.Count < 2)
                    {
                        result = true;
                    }
                }
                else
                {
                    var itemlist = _Util.Facade.MenuFacade.GetMenuItemByUrlSlug(urlslug, CurrentUser.CompanyId.Value);
                    if (itemlist.Count == 0)
                    {
                        result = true;
                    }
                }
            }
            else
            {
                var itemlist = _Util.Facade.MenuFacade.GetMenuItemByUrlSlug(urlslug, CurrentUser.CompanyId.Value);
                if (itemlist.Count == 0)
                {
                    result = true;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetMenuUrlSlugPermission(int? id, string urlslug)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            if (id.HasValue && id.Value > 0 && !string.IsNullOrWhiteSpace(urlslug))
            {
                var objitem = _Util.Facade.MenuFacade.GetMenuById(id.Value);
                if (objitem != null && objitem.UrlSlug == urlslug)
                {
                    var itemlist = _Util.Facade.MenuFacade.GetMenuByUrlSlug(urlslug, CurrentUser.CompanyId.Value);
                    if (itemlist.Count < 2)
                    {
                        result = true;
                    }
                }
                else
                {
                    var itemlist = _Util.Facade.MenuFacade.GetMenuByUrlSlug(urlslug, CurrentUser.CompanyId.Value);
                    if (itemlist.Count == 0)
                    {
                        result = true;
                    }
                }
            }
            else
            {
                var itemlist = _Util.Facade.MenuFacade.GetMenuByUrlSlug(urlslug, CurrentUser.CompanyId.Value);
                if (itemlist.Count == 0)
                {
                    result = true;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetCategoryUrlSlugPermission(int? id, string urlslug)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            if (id.HasValue && id.Value > 0 && !string.IsNullOrWhiteSpace(urlslug))
            {
                var objitem = _Util.Facade.MenuFacade.GetCategoryById(id.Value);
                if (objitem != null && objitem.UrlSlug == urlslug)
                {
                    var itemlist = _Util.Facade.MenuFacade.GetCategoryByUrlSlug(urlslug, CurrentUser.CompanyId.Value);
                    if (itemlist.Count < 2)
                    {
                        result = true;
                    }
                }
                else
                {
                    var itemlist = _Util.Facade.MenuFacade.GetCategoryByUrlSlug(urlslug, CurrentUser.CompanyId.Value);
                    if (itemlist.Count == 0)
                    {
                        result = true;
                    }
                }
            }
            else
            {
                var itemlist = _Util.Facade.MenuFacade.GetCategoryByUrlSlug(urlslug, CurrentUser.CompanyId.Value);
                if (itemlist.Count == 0)
                {
                    result = true;
                }
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult UploadMenuItemAdditionalPhoto()
        {
            string NameFile = "";
            bool isUploaded = false;
            int width = 0;
            int height = 0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBaseMenu = Request.Files["CustomerFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.MenuItemPhoto"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBaseMenu.FileName;
            string tempFolderPath = "";
            if (httpPostedFileBaseMenu != null && httpPostedFileBaseMenu.ContentLength != 0)
            {
                NameFile = httpPostedFileBaseMenu.FileName;
                tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBaseMenu.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            return Json(new { isUploaded = isUploaded, filePath = filePath, NameFile = NameFile, FullFilePath = FullFilePath }, "text/html");
        }

        public ActionResult CopyMenuPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            WebsiteLocation model = new WebsiteLocation();
            List<SelectListItem> reslist = new List<SelectListItem>();
            reslist.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            reslist.AddRange(_Util.Facade.MenuFacade.GetAllWebLocWithNoItems().OrderBy(x => x.Name).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.CompanyId.ToString()
            }).ToList());
            ViewBag.ToppingCount = _Util.Facade.MenuFacade.GetAllToppingCategoryByCompanyId(CurrentUser.CompanyId.Value).Count;
            ViewBag.Restaurant = reslist;
            return View(model);
        }

        [HttpPost]
        public JsonResult ImportMenuByRestaurant(Guid comid, bool iscategory, bool isitem, bool istopping)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            if(comid != new Guid())
            {
                result = _Util.Facade.MenuFacade.CloneRestaurantMenuByCompanyId(comid, CurrentUser.CompanyId.Value, CurrentUser.UserId);
                if (iscategory)
                {
                    result = _Util.Facade.MenuFacade.CloneRestaurantCategoryByCompanyId(comid, CurrentUser.CompanyId.Value, CurrentUser.UserId);
                }
                if (istopping)
                {
                    result = _Util.Facade.MenuFacade.CloneRestaurantToppingByCompanyId(comid, CurrentUser.CompanyId.Value, CurrentUser.UserId);
                }
                if (isitem)
                {
                    result = _Util.Facade.MenuFacade.CloneRestaurantMenuItemByCompanyId(comid, CurrentUser.CompanyId.Value, CurrentUser.UserId);
                }
            }
            return Json(result);
        }

        public JsonResult DeleteAllItems()
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            result = _Util.Facade.MenuFacade.DeleteAllItemsByCompanyId(CurrentUser.CompanyId.Value);
            return Json(result);
        }
    }
}
