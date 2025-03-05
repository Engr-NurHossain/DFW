using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HS.Entities.AppPermission;

namespace HS.Web.UI.Controllers
{
    public class SetupController : BaseController
    {
        // GET: Setup
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

        public ActionResult MMRS()
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.MMRFeeTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<MMR> mmrs = _Util.Facade.MmrFacade.GetAllMMRByCompanyId(currentLoggedIn.CompanyId.Value);
            foreach (var item in mmrs)
            {
                float n;
                if (float.TryParse(item.Name, out n))
                {
                    //item.Name = string.Format("{0:#,###0}", Convert.ToDouble(item.Name));
                    item.Name = string.Format("{0:0,0.00}", Convert.ToDouble(item.Name));
                }
            }
            return PartialView(mmrs);
        }

        [Authorize]
        public PartialViewResult AddMmr(int? Id)
        {
            MMR model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (Id.HasValue)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.MMRFeeEdit))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }

                model = _Util.Facade.MmrFacade.GetAllMMRById(Id.Value);
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.MMRFeeAdd))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }

                model = new MMR();
            }

            return PartialView("_AddMmr", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteMMR(int? id)
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.MMRFeeDelete))
            {
                return Json(false);
            }

            if (id.HasValue)
            {
                var ProductMMR = _Util.Facade.MmrFacade.DeleteMMR(id.Value);
            }

            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddMmrSetup(MMR mr)
        {
            bool result = false;
            mr.IsActivve = true;
            mr.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)(User)).CompanyId.Value;
            //string value = mr.Value.ToString();
            if (mr.Id > 0)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.MMRFeeEdit))
                {
                    return Json(false);
                }
                result = _Util.Facade.MmrFacade.UpdateMMR(mr);
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.MMRFeeAdd))
                {
                    return Json(false);
                }
                result = _Util.Facade.MmrFacade.InsertMMR(mr) > 0;
            }
            //if (!string.IsNullOrWhiteSpace(value))
            //{
            //    if (value.IndexOf("$") == -1)
            //    {
            //        mr.Value = convert "$" + value;
            //    }
            //    else
            //    {
            //        mr.Name = name;
            //    }

            //}

            return Json(result);
        }

        [Authorize]
        public PartialViewResult SettingsPartial()
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.GlobalSettingsTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            List<GlobalSetting> Settings = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsByCompanyId(CurrentUser.CompanyId.Value);

            return PartialView("_Settings", Settings);
        }

        [Authorize]
        public PartialViewResult EditSettings(int? id)
        {

            GlobalSetting model;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            model = new GlobalSetting();
            if (id.HasValue && id > 0)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.GlobalSettingsEdit))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsDetailsById(id.Value);
            }
            List<SelectListItem> defaultView = new List<SelectListItem>();
            defaultView.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("DefaultView").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.DefaultView = defaultView;

            List<SelectListItem> AutoClockOut = new List<SelectListItem>();
            AutoClockOut.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("AutoClockOut").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.AutoClockOut = AutoClockOut;

            ViewBag.SchedularTime = _Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", null, null).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.CustomerListOrderBy = _Util.Facade.LookupFacade.GetLookupByKey("CustomerListOrder").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.FirstDayOfWeek = _Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("FirstDayOfWeek", null, null).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
                
            }).ToList();

            ViewBag.SalesMatrixWeek = _Util.Facade.LookupFacade.GetLookupByKey("SalesMatrixEffectiveFilterWeek").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()

            }).ToList();

            ViewBag.PaymentMethodList = _Util.Facade.LookupFacade.GetLookupByKey("PaymentGetway").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()

            }).ToList();

            ViewBag.IeateryPaymentOptionList = _Util.Facade.LookupFacade.GetLookupByKey("IeateryPaymentOption").Select(x =>
          new SelectListItem()
          {
              Text = x.DisplayText.ToString(),
              Value = x.DataValue.ToString(),
            //  Selected = x.DataValue.ToString() != null 
          }).ToList();

            ViewBag.DefaultTicketType = _Util.Facade.LookupFacade.GetLookupByKey("TicketType").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.ShowTicketPricing = _Util.Facade.LookupFacade.GetLookupByKey("ShowTicketPricing").Select(x =>
           new SelectListItem()
           {
               Text = x.DisplayText.ToString(),
               Value = x.DataValue.ToString()
           }).ToList();

            var mintime = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarMinTimeRange(CurrentUser.CompanyId.Value).Split(':');
            if (mintime.Length > 1)
            {
                ViewBag.mintime = mintime[0] + mintime[1];
            }
            var maxtime = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarMaxTimeRange(CurrentUser.CompanyId.Value).Split(':');
            if (maxtime.Length > 1)
            {
                ViewBag.maxtime = maxtime[0] + maxtime[1];
            }
            ViewBag.CompanyIdDefaultList = _Util.Facade.CompanyFacade.GetAllCompany().Select(x =>
            new SelectListItem()
            {
                Text = x.CompanyName.ToString(),
                Value = x.CompanyId.ToString()
            }).ToList();
            GlobalSetting CompanyIdDetails = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("CompanyIdDefault");
            var CompanyId = CompanyIdDetails != null ? new Guid(CompanyIdDetails.Value) : Guid.Empty;
            ViewBag.EmployeeIdDefaultList = _Util.Facade.EmployeeFacade.GetAllEmployee(CompanyId).Select(x =>
           new SelectListItem()
           {
               Text = x.FirstName + " " + x.LastName,
               Value = x.UserId.ToString()
           }).ToList();
            ViewBag.PermissionGroupList = _Util.Facade.PermissionFacade.GetAllPermissionGroupListByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
           new SelectListItem()
           {
               Text = x.Name,
               Value = x.Id.ToString()
           }).ToList();
            ViewBag.AssignTaskTeamList = _Util.Facade.UserLoginFacade.GetAllTeam().Select(x =>
           new SelectListItem()
           {
               Text = x.Name,
               Value = x.Id.ToString()
           }).ToList();
            ViewBag.PayrollFilterWeek = _Util.Facade.LookupFacade.GetLookupByKey("PayrollFilterWeek").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.DateRangeList = _Util.Facade.LookupFacade.GetLookupByKey("PTOFilterOptions").Where(x =>x.DataValue != "Custom").ToList().Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString(),

            }).ToList();
            ViewBag.PayrollFilterWeek = _Util.Facade.LookupFacade.GetLookupByKey("BillFor").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            List<SelectListItem> ordertype = new List<SelectListItem>();
            ordertype.Add(new SelectListItem()
            {
                Text = "Pickup/Delivery",
                Value = "pickupdelivery"
            });
            ordertype.Add(new SelectListItem()
            {
                Text = "Pickup",
                Value = "pickup"
            });
            ordertype.Add(new SelectListItem()
            {
                Text = "Delivery",
                Value = "delivery"
            });
            ViewBag.ordertype = ordertype;

            List<SelectListItem> CurrencyFormat = new List<SelectListItem>();
            CurrencyFormat.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("CurrencyFormat").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.CurrencyFormat = CurrencyFormat;

            if (model.SearchKey == "TicketAssignPermissionGroup")
            {
                model.mulselectId = new List<int>();
                if (!string.IsNullOrWhiteSpace(model.Value) && model.Value.ToLower()!="null")
                {
                    string[] SList = model.Value.Split(',');
                    if (SList != null)
                    {
                        foreach (var item in SList)
                        {
                            model.mulselectId.Add(Convert.ToInt32(item));
                        }
                    }

                }
            }
            return PartialView("_EditSettings", model);
        }

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public JsonResult EditSettings(GlobalSetting globalSetting)
        {
            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            globalSetting.CompanyId = currentLoggedIn.CompanyId.Value;
            globalSetting.IsActive = true;
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(result);
            }
            if (globalSetting.Id > 0)
            {
                var OldGlobalSettingModel = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsDetailsById(globalSetting.Id);
                if (OldGlobalSettingModel != null)
                {
                    if(OldGlobalSettingModel.InputType != "Password")
                    {
                        OldGlobalSettingModel.Value = globalSetting.Value;
                        OldGlobalSettingModel.OptionalValue = globalSetting.OptionalValue;
                        result = _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(OldGlobalSettingModel);
                    }
                    else
                    {
                        if (globalSetting.Value != null && globalSetting.Value != "")
                        {
                            OldGlobalSettingModel.Value = globalSetting.Value;
                            OldGlobalSettingModel.OptionalValue = globalSetting.OptionalValue;
                            result = _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(OldGlobalSettingModel);
                        }
                    }
                }
                ClearCache(currentLoggedIn, OldGlobalSettingModel);
                if(globalSetting.SearchKey == "SetDateFilterRange")
                {
                    if (Request.Cookies["_DateViewFilter"] != null)
                    {

                        HttpCookie myCookie = new HttpCookie("_DateViewFilter");
                      
                        myCookie.Expires = DateTime.Now;
                        Response.Cookies.Add(myCookie);

                    }
                }
             
            }
            return Json(result);
        }

        [Authorize]
        public PartialViewResult GridSettings()
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<GridSetting> GridSettings = _Util.Facade.GridSettingsFacade.GetAllByKey("CustomerGrid", CurrentLoggedInUser.CompanyId.Value);
            return PartialView("_GridSettings", GridSettings);
        }

        [Authorize]
        public JsonResult UpdateGridSettings(List<GridSetting> settings)
        {
            var result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (settings.Count > 0)
            {
                foreach (var item in settings)
                {
                    item.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                    result = _Util.Facade.GridSettingsFacade.UpdateGridSettings(item);
                }
            }
            return Json(result);
        }

        public ActionResult AlarmDotComSettingPartial()
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.AlarmComTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<ThirdPartySetting> ThirdPartySetting = _Util.Facade.ThirdPartySettingFacade.GetAllAlarmSettingByCompanyId(currentLoggedIn.CompanyId.Value);
            return PartialView("AlarmDotComSettingPartial", ThirdPartySetting);
        }

        [Authorize]
        public ActionResult AddAlarmDotComSetting(int? id)
        {
            if (id.HasValue && id > 0)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.AlarmComEdit))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.AlarmComAdd))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
            }

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            ThirdPartySetting model;
            if (id.HasValue)
            {
                model = _Util.Facade.ThirdPartySettingFacade.GetThirdPartyById(id.Value);
            }
            else
            {
                model = new ThirdPartySetting();
            }
            return View("AddAlarmDotComSetting", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddAlarmDotComSetting(ThirdPartySetting tps)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            tps.CompanyId = currentLoggedIn.CompanyId.Value;
            tps.IsActive = true;
            if (tps.Id > 0)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.AlarmComEdit))
                {
                    return Json(false);
                }

                tps.Type = ThirdPartyType.AlarmCom;

                _Util.Facade.ThirdPartySettingFacade.UpdateThirdPartySetting(tps);
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.AlarmComAdd))
                {
                    return Json(false);
                }

                tps.Type = ThirdPartyType.AlarmCom;

                _Util.Facade.ThirdPartySettingFacade.InsertThirdPartySetting(tps);
            }
            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteAlarmSetting(int? id)
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.AlarmComDelete))
            {
                return Json(false);
            }

            if (id.HasValue)
            {
                var Alarmval = _Util.Facade.ThirdPartySettingFacade.DeleteThirdPartySetting(id.Value);
            }

            return Json(true);
        }
        [Authorize]
        public ActionResult SetupDetails()
        {
            return PartialView("_SetupDetails");
        }
        #region UI Set Up
        [Authorize]
        public ActionResult UISetUpDetails()
        {
            return PartialView("_UISetUpDetails");
        }
        #endregion
        [Authorize]
        public ActionResult AuthorizeDotNetSettingPartial()
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.AuthorizeNetTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<ThirdPartySetting> ThirdPartySetting = _Util.Facade.ThirdPartySettingFacade.GetAllAuthorizeSettingByCompanyId(currentLoggedIn.CompanyId.Value);
            return View("AuthorizeDotNetSettingPartial", ThirdPartySetting);
        }

        [Authorize]
        public ActionResult AddAuthorizeDotNetSetting(int? id)
        {

            if (id.HasValue && id > 0)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.AuthorizeNetEdit))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.AuthorizeNetAdd))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ThirdPartySetting model;
            if (id.HasValue)
            {
                model = _Util.Facade.ThirdPartySettingFacade.GetThirdPartyById(id.Value);
            }
            else
            {
                model = new ThirdPartySetting();
            }
            return View("AddAuthorizeDotNetSetting", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddAuthorizeDotNetSetting(ThirdPartySetting tps)
        {

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            tps.CompanyId = currentLoggedIn.CompanyId.Value;
            tps.IsActive = true;
            if (tps.Id > 0)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.AuthorizeNetEdit))
                {
                    return Json(false);
                }

                tps.Type = ThirdPartyType.AuthorizeNet;

                _Util.Facade.ThirdPartySettingFacade.UpdateThirdPartySetting(tps);
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.AuthorizeNetAdd))
                {
                    return Json(false);
                }
                tps.Type = ThirdPartyType.AuthorizeNet;

                _Util.Facade.ThirdPartySettingFacade.InsertThirdPartySetting(tps);
            }
            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteAuthorizeSetting(int? id)
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.AuthorizeNetDelete))
            {
                return Json(false);
            }

            if (id.HasValue)
            {
                var Authorizeval = _Util.Facade.ThirdPartySettingFacade.DeleteThirdPartySetting(id.Value);
            }

            return Json(true);
        }


        [Authorize]
        public PartialViewResult LookupSetup()
        {
            List<string> Model = _Util.Facade.LookupFacade.GetDataKeyList();

            return PartialView("_LookupSetup", Model);
        } 

        [Authorize]
        public PartialViewResult LookupItems(string Key)
        {
            List<Lookup> Model = _Util.Facade.LookupFacade.GetLookupByKey(Key, true,true);
            ViewBag.DataKey = Key;

            return PartialView("_LookupItems", Model);
        }
        [Authorize]
        public PartialViewResult LookupItemsWithParent(string Key)
        {
            List<Lookup> Model = _Util.Facade.LookupFacade.GetLookupByKey(Key, true,true);
            ViewBag.DataKey = Key;

            var LookUpParent = Model.Where(m => m.ParentDataKey == "Parent").ToList();
            List<SelectListItem> ParentList = new List<SelectListItem>();
            ParentList.Add(new SelectListItem()
            {
                Text = "Please Select Parent",
                Value = "-1"
            });
            ParentList.AddRange(LookUpParent.Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.ParentList = ParentList;
            var MaxDataOrder = 0;
            var MaxDataOrderDetails = Model.OrderByDescending(m => m.DataOrder).FirstOrDefault();
            if(MaxDataOrderDetails!=null && MaxDataOrderDetails.DataOrder.HasValue)
            {
                MaxDataOrder = MaxDataOrderDetails.DataOrder.Value;
            }
            ViewBag.MaxDataOrder = MaxDataOrder;
            return PartialView("_LookupItemsWithParent", Model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddLookup(string Datakey, string DataValue, string DisplayText, int DataOrder, bool IsActive)
        {
            Lookup lookup = _Util.Facade.LookupFacade.GetLookupByKeyAndValue(Datakey, DataValue);
            if (lookup != null)
            {
                return Json(new { reuslt = false, message = "Data Value already exists for Data Key: " + Datakey });
            }

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Lookup lk = new Lookup()
            {
                DataKey = Datakey,
                DisplayText = DisplayText,
                IsActive = IsActive,
                DataOrder = DataOrder,
                DataValue = DataValue,
                CompanyId = currentLoggedIn.CompanyId.Value
            };
            lk.Id = _Util.Facade.LookupFacade.InsertLookup(lk);

            return Json(new { result= true, message="Inserted successfully.",data= lk });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddLookupWithParent(string Datakey, string DataValue, string DisplayText, int DataOrder, bool IsActive,string ParentDataKey)
        {

            Lookup lookup = _Util.Facade.LookupFacade.GetLookupByKeyAndValue(Datakey,DataValue);
            if(lookup != null)
            {
                return Json(new { reuslt=false, message = "Data Value already exists for Data Key: " + Datakey });
            }

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Lookup lk = new Lookup()
            {
                DataKey = Datakey,
                DisplayText = DisplayText,
                IsActive = IsActive,
                DataOrder = DataOrder,
                DataValue = DataValue,
                ParentDataKey= ParentDataKey,
                CompanyId = currentLoggedIn.CompanyId.Value
            };
            lk.Id = _Util.Facade.LookupFacade.InsertLookup(lk);
            return Json(new { result = true, message = "Inserted successfully.", data= lk });
            //return Json(lk);
        }

        [Authorize]
        [HttpPost]
        public JsonResult UpdateLookupList(List<Lookup> LkList)
        {
            if (LkList.Count() == 0)
            {
                return Json(new { result = false, message = "No data selected." });
            }
            foreach (var item in LkList)
            {
                //also can take all lookup list by data key
                //then update individually.

                Lookup lk;
                if (item.Id > 0)
                {
                    lk = _Util.Facade.LookupFacade.GetLookupByKeyAndId(item.DataKey, item.Id);
                }
                else
                {
                    lk = _Util.Facade.LookupFacade.GetLookupByKeyAndValue(item.DataKey, item.DataValue);
                }
                if (lk != null)
                {
                    lk.DataOrder = item.DataOrder;
                    lk.DisplayText = item.DisplayText;
                    lk.IsActive = item.IsActive;
                    lk.IsDefaultItem = item.IsDefaultItem;
                    lk.AlterDisplayText1 = item.AlterDisplayText1;
                    _Util.Facade.LookupFacade.UpdateLookUp(lk);
                }
            }
            return Json(new { result = true, message = "Lookup Updated successfully." });

        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteLookup(int LookupId, string Datakey)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (_Util.Facade.LookupFacade.DeleteLookupByIdAndDataKey(LookupId, Datakey, CurrentUser.CompanyId.Value))
            {
                return Json(new { result = true, message = "Deleted successfully." });
            }
            else
            {
                return Json(new { result = false, message = "Internal error. Please report to system admin." });
            }

        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteLookupWithParent(int LookupId, string Datakey)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (_Util.Facade.LookupFacade.DeleteLookupByIdAndDataKeyWithChild(LookupId, Datakey, CurrentUser.CompanyId.Value))
            {
                return Json(new { result = true, message = "Deleted successfully." });
            }
            else
            {
                return Json(new { result = false, message = "Internal error. Please report to system admin." });
            }

        }

        [Authorize]
        [HttpPost]
        public JsonResult UpdateLookup(Lookup lk)
        {
            Lookup Temp = _Util.Facade.LookupFacade.GetLookupByKeyAndId(lk.DataKey, lk.Id);
            if (Temp == null)
            {
                return Json(new { result = false, message = "Not found, please contact system admin." });

            }

            Temp.DisplayText = lk.DisplayText;
            Temp.IsActive = lk.IsActive;
            Temp.IsDefaultItem = lk.IsDefaultItem;
            Temp.DataOrder = lk.DataOrder;
            Temp.AlterDisplayText = lk.AlterDisplayText;
            Temp.AlterDisplayText1 = lk.AlterDisplayText1;
            _Util.Facade.LookupFacade.UpdateLookUp(Temp);

            return Json(new { result = true, message = "Updated successfully." });
        }

        [Authorize]
        public ActionResult CompanyDocumentPartial()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<CompanyFile> model = _Util.Facade.CompanyFileFacade.GetAllCompanyFile(currentLoggedIn.CompanyId.Value);
            return View(model);
        }

        [Authorize]
        public ActionResult CustomerUiSettingsPartial()
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.GlobalSettingsTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            List<GridSetting> Settings = _Util.Facade.GridSettingsFacade.GetAllByKey("CustomerGrid", CurrentUser.CompanyId.Value);
            List<SelectListItem> GroupList = new List<SelectListItem>();
            GroupList.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<GridSetting> GroupGridSettings = _Util.Facade.GridSettingsFacade.GetAllByKey("CustomerGridGroup", CurrentUser.CompanyId.Value);
            if (GroupGridSettings.Count > 0)
            {
                GroupGridSettings = GroupGridSettings.OrderBy(x => x.OrderBy).ToList();
            }
            //Settings.AddRange(GroupGridSettings);
            //.Where(x => x.GridActive == true)
            ViewBag.GroupGridSettings = GroupGridSettings;
            GroupList.AddRange(GroupGridSettings.Select(x => new SelectListItem()
            {
                Text = x.SelectedColumn.ToString(),
                Value = x.SelectedColumn.ToString()
            }).ToList());
            ViewBag.GroupList = GroupList;
            return PartialView(Settings);
        }

        [Authorize]
        public ActionResult LeadUiSettingsPartial()
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.GlobalSettingsTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            List<GridSetting> Settings = _Util.Facade.GridSettingsFacade.GetAllByKey("LeadGrid", CurrentUser.CompanyId.Value);
            List<SelectListItem> GroupList = new List<SelectListItem>();
            GroupList.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<GridSetting> GroupGridSettings = _Util.Facade.GridSettingsFacade.GetAllByKey("LeadGridGroup", CurrentUser.CompanyId.Value);
            if (GroupGridSettings.Count > 0)
            {
                GroupGridSettings = GroupGridSettings.OrderBy(x => x.OrderBy).ToList();
            }
            //Settings.AddRange(GroupGridSettings);
            //.Where(x => x.GridActive == true)
            ViewBag.GroupGridSettings = GroupGridSettings;
            GroupList.AddRange(GroupGridSettings.Select(x => new SelectListItem()
            {
                Text = x.SelectedColumn.ToString(),
                Value = x.SelectedColumn.ToString()
            }).ToList());
            ViewBag.GroupList = GroupList;
            return PartialView(Settings);
        }

        [Authorize]
        public ActionResult InventoryUiSettingsPartial()
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.GlobalSettingsTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            List<GlobalSetting> Settings = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsByCompanyId(CurrentUser.CompanyId.Value).Where(x => x.Tag == "InventoryUiSettings").ToList();

            return PartialView(Settings);
        }

        [Authorize]
        public ActionResult EquipmentUiSettingsPartial()
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.GlobalSettingsTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //List<GlobalSetting> Settings = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsByCompanyId(CurrentUser.CompanyId.Value).Where(x => x.Tag == "EquipmentUiSettings").OrderBy(it => it.OrderBy).ToList();
            List<GridSetting> EquipmentGridSettings = _Util.Facade.GridSettingsFacade.GetAllByKey("EquipmentGrid", CurrentUser.CompanyId.Value).OrderBy(x=>x.OrderBy).ToList();
            return PartialView(EquipmentGridSettings);
        }


        [Authorize]
        public PartialViewResult EditCustomerUiSettings(int? id)
        {

            GlobalSetting model;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            model = new GlobalSetting();
            if (id.HasValue && id > 0)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.GlobalSettingsEdit))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsDetailsById(id.Value);
            }
            List<SelectListItem> defaultView = new List<SelectListItem>();
            defaultView.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("DefaultView").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.DefaultView = defaultView;
            ViewBag.SchedularTime = _Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", null, null).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.CustomerListOrderBy = _Util.Facade.LookupFacade.GetLookupByKey("CustomerListOrder").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.FirstDayOfWeek = _Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("FirstDayOfWeek", null, null).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.PaymentMethodList = _Util.Facade.LookupFacade.GetLookupByKey("PaymentGetway").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            var mintime = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarMinTimeRange(CurrentUser.CompanyId.Value).Split(':');
            if (mintime.Length > 1)
            {
                ViewBag.mintime = mintime[0] + mintime[1];
            }
            var maxtime = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarMaxTimeRange(CurrentUser.CompanyId.Value).Split(':');
            if (maxtime.Length > 1)
            {
                ViewBag.maxtime = maxtime[0] + maxtime[1];
            }
            return PartialView("_EditCustomerUiSettings", model);
        }

        [Authorize]
        public PartialViewResult EditInventoryUiSettings(int? id)
        {

            GlobalSetting model;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            model = new GlobalSetting();
            if (id.HasValue && id > 0)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.GlobalSettingsEdit))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsDetailsById(id.Value);
            }
            List<SelectListItem> defaultView = new List<SelectListItem>();
            defaultView.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("DefaultView").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.DefaultView = defaultView;
            ViewBag.SchedularTime = _Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", null, null).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.FirstDayOfWeek = _Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("FirstDayOfWeek", null, null).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.PaymentMethodList = _Util.Facade.LookupFacade.GetLookupByKey("PaymentGetway").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            var mintime = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarMinTimeRange(CurrentUser.CompanyId.Value).Split(':');
            if (mintime.Length > 1)
            {
                ViewBag.mintime = mintime[0] + mintime[1];
            }
            var maxtime = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarMaxTimeRange(CurrentUser.CompanyId.Value).Split(':');
            if (maxtime.Length > 1)
            {
                ViewBag.maxtime = maxtime[0] + maxtime[1];
            }
            return PartialView("_EditInventoryUiSettings", model);
        }

        [Authorize]
        public PartialViewResult EditEquipmentUiSettings(int? id)
        {

            GlobalSetting model;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            model = new GlobalSetting();
            if (id.HasValue && id > 0)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.GlobalSettingsEdit))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsDetailsById(id.Value);
            }
            List<SelectListItem> defaultView = new List<SelectListItem>();
            defaultView.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("DefaultView").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.DefaultView = defaultView;
            ViewBag.SchedularTime = _Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", null, null).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList(); 
            ViewBag.FirstDayOfWeek = _Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("FirstDayOfWeek", null, null).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.PaymentMethodList = _Util.Facade.LookupFacade.GetLookupByKey("PaymentGetway").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            var mintime = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarMinTimeRange(CurrentUser.CompanyId.Value).Split(':');
            if (mintime.Length > 1)
            {
                ViewBag.mintime = mintime[0] + mintime[1];
            }
            var maxtime = _Util.Facade.GlobalSettingsFacade.GetScheduleCalendarMaxTimeRange(CurrentUser.CompanyId.Value).Split(':');
            if (maxtime.Length > 1)
            {
                ViewBag.maxtime = maxtime[0] + maxtime[1];
            }
            return PartialView("_EditEquipmentUiSettings", model);
        }


        [Authorize]
        [HttpPost, ValidateInput(false)]
        public JsonResult EditCustomerUiSettings(GlobalSetting globalSetting)
        {
            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            globalSetting.CompanyId = currentLoggedIn.CompanyId.Value;
            globalSetting.IsActive = true;
            if (globalSetting.Id > 0)
            {
                GlobalSetting OldGlobalSettingModel = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsDetailsById(globalSetting.Id);
                if (OldGlobalSettingModel != null)
                {
                    OldGlobalSettingModel.Value = globalSetting.Value;
                    result = _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(OldGlobalSettingModel);
                    globalSetting.Tag = OldGlobalSettingModel.Tag;
                }
                ClearCache(currentLoggedIn, OldGlobalSettingModel);
            }
            return Json(new { result = result, tag = globalSetting.Tag });
        }


        [Authorize]
        [HttpPost, ValidateInput(false)]
        public JsonResult EditInventoryUiSettings(GlobalSetting globalSetting)
        {
            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            globalSetting.CompanyId = currentLoggedIn.CompanyId.Value;
            globalSetting.IsActive = true;
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(result);
            }
            if (globalSetting.Id > 0)
            {
                var OldGlobalSettingModel = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsDetailsById(globalSetting.Id);
                if (OldGlobalSettingModel != null)
                {
                    OldGlobalSettingModel.Value = globalSetting.Value;
                    result = _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(OldGlobalSettingModel);
                }
                ClearCache(currentLoggedIn, OldGlobalSettingModel);
            }
            return Json(result);
        }


        [Authorize]
        public JsonResult UpdateEquipmentUiSettings(List<GridSetting> EquipmentUISettings)
        {
            var result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (EquipmentUISettings.Count > 0)
            {
                foreach (var item in EquipmentUISettings)
                {
                    item.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                    result = _Util.Facade.GridSettingsFacade.UpdateGridSettings(item);
                    //result = _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(item);
                }
            }
            return Json(result);
        }

        [Authorize]
        public JsonResult UpdateLeadUiSettings(List<GridSetting> LeadUISettings)
        {
            var result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (LeadUISettings.Count > 0)
            {
                foreach (var item in LeadUISettings)
                {
                    item.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                    result = _Util.Facade.GridSettingsFacade.UpdateGridSettings(item);
                }
            }
            return Json(result);
        }

        [Authorize]
        public JsonResult UpdateCustomerUiSettings(List<GridSetting> CustomerUISettings)
        {
            var result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (CustomerUISettings.Count > 0)
            {
                foreach (var item in CustomerUISettings)
                {
                    item.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                    result = _Util.Facade.GridSettingsFacade.UpdateGridSettings(item);
                }
            }
            return Json(result);
        }

        [Authorize]
        public JsonResult UpdateCustomerTabUiSettings(List<GridSetting> CustomerUISettings)
        {
            var result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (CustomerUISettings.Count > 0)
            {
                foreach (var item in CustomerUISettings)
                {
                    item.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                    result = _Util.Facade.GridSettingsFacade.UpdateGridSettings(item);
                }
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public JsonResult EditEquipmentUiSettings(GlobalSetting globalSetting)
        {
            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            globalSetting.CompanyId = currentLoggedIn.CompanyId.Value;
            globalSetting.IsActive = true;
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(result);
            }
            if (globalSetting.Id > 0)
            {
                var OldGlobalSettingModel = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsDetailsById(globalSetting.Id);
                if (OldGlobalSettingModel != null)
                {
                    OldGlobalSettingModel.Value = globalSetting.Value;
                    result = _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(OldGlobalSettingModel);
                }
                ClearCache(currentLoggedIn, OldGlobalSettingModel);
            }
            return Json(result);
        }


        [Authorize]
        public ActionResult AddCompanyFile()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadCompanyFile()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            HttpPostedFileBase httpPostedFileBase = Request.Files["CompanyFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.CompanyFiles"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += "/" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + AppConfig.DomainSitePath + filePath;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveCompanyFile(string File, string Description)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);

            if (!System.IO.File.Exists(serverFile))
            {
                return Json(new { result = true, message = "File not exsists" });
            }
            var OnlyFileName = File.Split('/');
            float fileSize = OnlyFileName[5].Length;
            var delimeter = new string[] { "-___" };
            var fullFileName = OnlyFileName[5].Split(delimeter, StringSplitOptions.RemoveEmptyEntries);

            CompanyFile cf = new CompanyFile()
            {
                CompanyId = CurrentUser.CompanyId.Value,
                FileDescription = Description,
                Filename = File,
                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                FileFullName = fullFileName[1],
                IsActive = true,
                FileSize = fileSize
            };
            bool result = _Util.Facade.CompanyFileFacade.InsertCompanyFile(cf) > 0;
            return Json(new { result = result });
        }

        public ActionResult CompanyDocumentDownload()
        {

            if (Request.QueryString.Count == 0)
            {
                return RedirectPermanent("/");
            }
            else
            {
                string Idstr = DESEncryptionDecryption.DecryptCipherTextToPlainText((HttpUtility.UrlDecode(Request.QueryString[0].ToString())));

                CompanyFile filename = _Util.Facade.CompanyFileFacade.GetById(Int32.Parse(Idstr));
                // string Filenum = Encryption.Encrypt(filename.Id.ToString(),true);
                try
                {
                    string fullName = Server.MapPath("~" + filename.Filename);
                    if (!System.IO.File.Exists(fullName))
                    {
                        return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                    }

                    byte[] fileBytes = System.IO.File.ReadAllBytes(fullName);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename.FileFullName);
                }
                catch (Exception)
                {
                    return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                }

            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteCompanyFile(int Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CompanyFile tmpCF = _Util.Facade.CompanyFileFacade.GetById(Id);

            var serverFile = Server.MapPath(tmpCF.Filename);
            if (System.IO.File.Exists(serverFile))
            {
                System.IO.File.Delete(serverFile);
            }
            _Util.Facade.CompanyFileFacade.DeleteCompanyFile(Id);

            return Json(new { result = true });
        }
        [Authorize]
        public PartialViewResult AddLocalizeResource()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            LocalizeResource model = new Entities.LocalizeResource()
            {
                CompanyId = CurrentUser.CompanyId.Value,
                LanguageId = -1,
                ResourceName = "",
                ResourceValue = ""
            };

            ViewBag.LanguageList = _Util.Facade.LocalizeFacade.GetAllLanguageByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
             new SelectListItem()
             {
                 Text = x.Name.ToString(),
                 Value = x.Id.ToString()
             }).ToList();

            return PartialView("_AddLocalizeResource", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddLocalizeResource(LocalizeResource Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Model.CompanyId = CurrentUser.CompanyId.Value;
            if (string.IsNullOrWhiteSpace(Model.ResourceName))
            {
                return Json(new { result = false, message = "Resource name can't be empty." });
            }
            else if (string.IsNullOrWhiteSpace(Model.ResourceValue))
            {
                return Json(new { result = false, message = "Resource value can't be empty." });
            }
            else if (Model.LanguageId < 1)
            {
                return Json(new { result = false, message = "Please select specific language." });
            }
            Model.Id = _Util.Facade.LocalizeFacade.InsertLocalizeResource(Model);

            #region Reload System Cache
            string LanguagePack = RMRCacheKey.LanguagePack + CurrentUser.CompanyId.Value.GetHashCode().ToString();

            System.Web.HttpRuntime.Cache.Remove(LanguagePack);
            List<LocalizeResource> resources = new List<LocalizeResource>();
            resources = _Util.Facade.LocalizeFacade.GetAllLocalizeResourceByCompanyId(CurrentUser.CompanyId.Value);
            System.Web.HttpRuntime.Cache[LanguagePack] = resources;
            #endregion

            return Json(new { result = true, message = "Inserted successfully." });

        }

        [Authorize]
        public ActionResult LocalizeResource()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<Language> LangList = _Util.Facade.LocalizeFacade.GetAllLanguageByCompanyId(CurrentUser.CompanyId.Value);

            return View(LangList);
        }
        [Authorize]
        public ActionResult ShowResources(LocalizeFilterModel filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            filter.CompanyId = CurrentUser.CompanyId.Value;
            filter.PageSize = 50;

            if (filter.PageNo == 0)
            {
                filter.PageNo = 1;
            }

            //List<LocalizeResource> resourceList = _Util.Facade.LocalizeFacade.GetAllLocalizeResourceByCompanyIdAndLanguageId(CurrentUser.CompanyId.Value, filter.LangId);
            LocalizeResourceViewModel Model = _Util.Facade.LocalizeFacade.GetAllLocalizeResourceByFilter(filter);


            if (Model.LocalizeResource.Count() == 0)
            {
                filter.PageNo = 1;
                //customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilter(filter);
            }

            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = Model.TotalCount;

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);



            return View(Model);

        }
        [Authorize]
        [HttpPost]
        public JsonResult UpdateLocalizeResource(int Id, string ResourceName, string ResourceValue)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            LocalizeResource lr = _Util.Facade.LocalizeFacade.GetLocalizeResourceById(Id);
            lr.ResourceValue = ResourceValue;
            _Util.Facade.LocalizeFacade.UpdateLocalizeResource(lr);
            string LanguagePack = RMRCacheKey.LanguagePack + CurrentUser.CompanyId.Value.GetHashCode().ToString();
            System.Web.HttpRuntime.Cache.Remove(LanguagePack);
            List<LocalizeResource> resources = new List<LocalizeResource>();
            resources = _Util.Facade.LocalizeFacade.GetAllLocalizeResourceByCompanyId(CurrentUser.CompanyId.Value);
            System.Web.HttpRuntime.Cache[LanguagePack] = resources;

            return Json(new { result = true, message = "Updated successfully." });
        }

        [HttpPost]
        public JsonResult DeleteLocalizeResource(int? Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool res = false;
            if(Id.HasValue && Id.Value > 0)
            {
                LocalizeResource objlr = _Util.Facade.LocalizeFacade.GetLocalizeResourceById(Id.Value);
                if(objlr != null)
                {
                    res = _Util.Facade.LocalizeFacade.DeleteLocalizeResource(Id.Value);
                    string LanguagePack = RMRCacheKey.LanguagePack + CurrentUser.CompanyId.Value.GetHashCode().ToString();
                    System.Web.HttpRuntime.Cache.Remove(LanguagePack);
                    List<LocalizeResource> resources = new List<LocalizeResource>();
                    resources = _Util.Facade.LocalizeFacade.GetAllLocalizeResourceByCompanyId(CurrentUser.CompanyId.Value);
                    System.Web.HttpRuntime.Cache[LanguagePack] = resources;
                }
            }
            return Json(res);
        }

        #region Private Method
        private static void ClearCache(CustomPrincipal currentLoggedIn, GlobalSetting OldGlobalSettingModel)
        {
            if (HttpRuntime.Cache[OldGlobalSettingModel.SearchKey + currentLoggedIn.CompanyId.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(OldGlobalSettingModel.SearchKey + currentLoggedIn.CompanyId.ToString());
            }

            if (OldGlobalSettingModel.SearchKey == "InvoiceMessage" && HttpRuntime.Cache[RMRCacheKey.InvoiceMessageGlobal + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.InvoiceMessageGlobal + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "EstimateMessage" && HttpRuntime.Cache[RMRCacheKey.EstimateMessageGlobal + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.EstimateMessageGlobal + currentLoggedIn.CompanyId.Value.ToString());
            }

            else if (OldGlobalSettingModel.SearchKey == "AuthAPILoginId" && HttpRuntime.Cache[RMRCacheKey.AuthAPILoginId + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.AuthAPILoginId + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "AuthTransactionKey" && HttpRuntime.Cache[RMRCacheKey.AuthTransactionKey + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.AuthTransactionKey + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "AuthAPILoginIdACH" && HttpRuntime.Cache[RMRCacheKey.AuthAPILoginIdACH + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.AuthAPILoginIdACH + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "AuthTransactionKeyACH" && HttpRuntime.Cache[RMRCacheKey.AuthTransactionKeyACH + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.AuthTransactionKeyACH + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ForteAPILoginId" && HttpRuntime.Cache[RMRCacheKey.ForteAPILoginId + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ForteAPILoginId + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ForteTransactionKey" && HttpRuntime.Cache[RMRCacheKey.ForteTransactionKey + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ForteTransactionKey + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ForteOrganizationId" && HttpRuntime.Cache[RMRCacheKey.ForteOrganizationId + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ForteOrganizationId + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ForteLocationId" && HttpRuntime.Cache[RMRCacheKey.ForteLocationId + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ForteLocationId + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ForteAuthAccountId" && HttpRuntime.Cache[RMRCacheKey.ForteAuthAccountId + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ForteAuthAccountId + currentLoggedIn.CompanyId.Value.ToString());
            }

            else if (OldGlobalSettingModel.SearchKey == "InventoryPagingLimit" && HttpRuntime.Cache[RMRCacheKey.InventoryPagingLimit + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.InventoryPagingLimit + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "InvoiceSettingsShipping" && HttpRuntime.Cache[RMRCacheKey.InvoiceSettingsShipping + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.InvoiceSettingsShipping + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "CurrentCurrency" && HttpRuntime.Cache[RMRCacheKey.CurrentCurrency + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.CurrentCurrency + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "LeadCreditReportCheck" && HttpRuntime.Cache[RMRCacheKey.LeadCreditReportCheck + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.LeadCreditReportCheck + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "CustomFormGeneration" && HttpRuntime.Cache[RMRCacheKey.CustomFormGeneration + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.CustomFormGeneration + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ScheduleCalendarDefaultView" && HttpRuntime.Cache[RMRCacheKey.ScheduleCalendarDefaultView + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ScheduleCalendarDefaultView + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "GeoLocation" && HttpRuntime.Cache[RMRCacheKey.GeoLocation + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.GeoLocation + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "CustomerInvoicePageLimit" && HttpRuntime.Cache[RMRCacheKey.CustomerInvoicePageLimit + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.CustomerInvoicePageLimit + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "CustomerFundingPageLimit" && HttpRuntime.Cache[RMRCacheKey.CustomerFundingPageLimit + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.CustomerFundingPageLimit + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "CustomerSystemNoPagingLimit" && HttpRuntime.Cache[RMRCacheKey.CustomerSystemNoPagingLimit + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.CustomerSystemNoPagingLimit + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "CustomerAddressPdfFormat" && HttpRuntime.Cache[RMRCacheKey.CustomerAddressPdfFormat + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.CustomerAddressPdfFormat + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "CompanyAddressPdfFormat" && HttpRuntime.Cache[RMRCacheKey.CompanyAddressPdfFormat + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.CompanyAddressPdfFormat + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ScheduleCalendarResourceLimit" && HttpRuntime.Cache[RMRCacheKey.ScheduleCalendarResourceLimit + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ScheduleCalendarResourceLimit + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "TicketDefaultTimeDuration" && HttpRuntime.Cache[RMRCacheKey.TicketDefaultTimeDuration + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.TicketDefaultTimeDuration + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ScheduleCalendarMinTimeRange" && HttpRuntime.Cache[RMRCacheKey.ScheduleCalendarMinTimeRange + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ScheduleCalendarMinTimeRange + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ScheduleCalendarMaxTimeRange" && HttpRuntime.Cache[RMRCacheKey.ScheduleCalendarMaxTimeRange + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ScheduleCalendarMaxTimeRange + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ReminderMinTime" && HttpRuntime.Cache[RMRCacheKey.ReminderMinTime + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ReminderMinTime + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ReminderMaxTime" && HttpRuntime.Cache[RMRCacheKey.ReminderMaxTime + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ReminderMaxTime + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "MenuTimeAvailableStartTime" && HttpRuntime.Cache[RMRCacheKey.MenuTimeAvailableStartTime + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.MenuTimeAvailableStartTime + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "MenuTimeAvailableEndTime" && HttpRuntime.Cache[RMRCacheKey.MenuTimeAvailableEndTime + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.MenuTimeAvailableEndTime + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "CustomerListOrder" && HttpRuntime.Cache[RMRCacheKey.CustomerListOrder + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.CustomerListOrder + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "EquipmentSearchMaxLoad" && HttpRuntime.Cache[RMRCacheKey.EquipmentSearchMaxLoad + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.EquipmentSearchMaxLoad + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "PayrollFilterWeek" && HttpRuntime.Cache[RMRCacheKey.PayrollFilterWeek + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.PayrollFilterWeek + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "MapZoomLevel" && HttpRuntime.Cache[RMRCacheKey.MapZoomLevel + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.MapZoomLevel + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "NotificationManager" && HttpRuntime.Cache[RMRCacheKey.NotificationManager + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.NotificationManager + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "LeadPageLength" && HttpRuntime.Cache[RMRCacheKey.LeadPageLength + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.LeadPageLength + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "EmergencyContactRequired" && HttpRuntime.Cache[RMRCacheKey.EmergencyContactRequired + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.EmergencyContactRequired + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "EcheckTypeDefault" && HttpRuntime.Cache[RMRCacheKey.EcheckTypeDefault + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.EcheckTypeDefault + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "TicketAdditionalMemberOnlyTechnician" && HttpRuntime.Cache[RMRCacheKey.TicketAdditionalMemberOnlyTechnician + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.TicketAdditionalMemberOnlyTechnician + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ShoppingCartHeading" && HttpRuntime.Cache[RMRCacheKey.ShoppingCartHeading + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ShoppingCartHeading + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "PaymentOptionChoosing" && HttpRuntime.Cache[RMRCacheKey.PaymentOptionChoosing + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.PaymentOptionChoosing + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "iEateryOrderType" && HttpRuntime.Cache[RMRCacheKey.iEateryOrderType + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.iEateryOrderType + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "MaxOrderingQuantity" && HttpRuntime.Cache[RMRCacheKey.MaxOrderingQuantity + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.MaxOrderingQuantity + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "MinimumPrepTime" && HttpRuntime.Cache[RMRCacheKey.MinimumPrepTime + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.MinimumPrepTime + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "EstimatorOverheadRate" && HttpRuntime.Cache[RMRCacheKey.EstimatorOverheadRate + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.EstimatorOverheadRate + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "EstimatorProfitRate" && HttpRuntime.Cache[RMRCacheKey.EstimatorProfitRate + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.EstimatorProfitRate + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "ExpireTime" && HttpRuntime.Cache[RMRCacheKey.ExpireTime + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.ExpireTime + currentLoggedIn.CompanyId.Value.ToString());
            }
            else if (OldGlobalSettingModel.SearchKey == "OrderListPageLimit" && HttpRuntime.Cache[RMRCacheKey.OrderListPageLimit + currentLoggedIn.CompanyId.Value.ToString()] != null)
            {
                HttpRuntime.Cache.Remove(RMRCacheKey.OrderListPageLimit + currentLoggedIn.CompanyId.Value.ToString());
            }
        }

        #endregion
        public ActionResult SmartUiSetup()
        {
            return View();
        }

        [Authorize]
        public ActionResult CustomersmartUiSettingsPartial()
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.GlobalSettingsTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<CustomerLeadGridModel> model = _Util.Facade.GridSettingsFacade.GetCustomerLeadGridSetting(CurrentUser.CompanyId.Value);
            List<SelectListItem> CustomerGroupList = new List<SelectListItem>();
            CustomerGroupList.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<GridSetting> GroupGridSettings = _Util.Facade.GridSettingsFacade.GetAllByKey("CustomerGridGroup", CurrentUser.CompanyId.Value);
            if (GroupGridSettings.Count > 0)
            {
                GroupGridSettings = GroupGridSettings.OrderBy(x => x.OrderBy).ToList();
            }
            //Settings.AddRange(GroupGridSettings);
            //.Where(x => x.GridActive == true)
            ViewBag.GroupGridSettings = GroupGridSettings;
            CustomerGroupList.AddRange(GroupGridSettings.Select(x => new SelectListItem()
            {
                Text = x.SelectedColumn.ToString(),
                Value = x.SelectedColumn.ToString()
            }).ToList());
            ViewBag.CustomerGroupList = CustomerGroupList.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
            List<SelectListItem> LeadGroupList = new List<SelectListItem>();
            LeadGroupList.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            List<GridSetting> LeadGroupGridSettings = _Util.Facade.GridSettingsFacade.GetAllByKey("LeadGridGroup", CurrentUser.CompanyId.Value);
            if (LeadGroupGridSettings.Count > 0)
            {
                LeadGroupGridSettings = LeadGroupGridSettings.OrderBy(x => x.OrderBy).ToList();
            }
            //Settings.AddRange(GroupGridSettings);
            //.Where(x => x.GridActive == true)
            ViewBag.LeadGroupGridSettings = LeadGroupGridSettings;
            LeadGroupList.AddRange(LeadGroupGridSettings.Select(x => new SelectListItem()
            {
                Text = x.SelectedColumn.ToString(),
                Value = x.SelectedColumn.ToString()
            }).ToList());
            ViewBag.LeadGroupList = LeadGroupList.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList(); 
            return PartialView(model);
        }

        public ActionResult TabUiSettingsPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<GridSetting> CustomerTabList = _Util.Facade.GridSettingsFacade.GetAllByKey("CustomerTab", CurrentUser.CompanyId.Value);
            ViewBag.CustomerDetailTabList = _Util.Facade.GridSettingsFacade.GetAllByKey("CustomerDetailTab", CurrentUser.CompanyId.Value);
            ViewBag.DetailBlock = _Util.Facade.GridSettingsFacade.GetAllByKey("CustomerDetailBlock", CurrentUser.CompanyId.Value);
            ViewBag.LeadDetailBlock = _Util.Facade.GridSettingsFacade.GetAllByKey("LeadDetailBlock", CurrentUser.CompanyId.Value);
            return View(CustomerTabList);
        }
        public PartialViewResult IndividualSettingsPartial(string For)
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.GlobalSettingsTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            List<GlobalSetting> Settings = _Util.Facade.GlobalSettingsFacade.GetAllIndividualGlobalSettingsByCompanyId(CurrentUser.CompanyId.Value, For);

            return PartialView("_Settings", Settings);
        }
    }
}