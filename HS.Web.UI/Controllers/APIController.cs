using HS.Alarm.AlarmCustomer;
using HS.Alarm.CustomerManager;
using HS.Alarm.DealerManager;
using HS.Bounce.com.brinkshome.senti;
using HS.CommonFunding.CommonFundingApi;
using HS.Econtract.eContractApi;
using HS.Entities;
using HS.Entities.Custom;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;




//using HS.EContract.eContractApi;

namespace HS.Web.UI.Controllers
{
    public class APIController : BaseController
    {
        public APIController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        private class KickBoxResponse
        {
            public KickBoxResponse(string json)
            {
                json = @"{""json"":" + json + "}";
                JObject jObject = JObject.Parse(json);
                JToken jUser = jObject["json"];
                result = (string)jUser["result"];
                reason = (string)jUser["reason"];
                role = (string)jUser["role"];
                free = (string)jUser["free"];
                disposable = (string)jUser["disposable"];
                accept_all = (string)jUser["accept_all"];
                did_you_mean = (string)jUser["did_you_mean"];
                sendex = (string)jUser["sendex"];
                email = (string)jUser["email"];
                user = (string)jUser["user"];
                domain = (string)jUser["domain"];
                message = (string)jUser["message"];

            }
            public string result { get; set; }
            public string reason { get; set; }
            public string role { get; set; }
            public string free { get; set; }
            public string disposable { get; set; }
            public string accept_all { get; set; }
            public string did_you_mean { get; set; }
            public string sendex { get; set; }
            public string email { get; set; }
            public string user { get; set; }
            public string domain { get; set; }
            public string message { set; get; }
        }


        [Authorize]
        public ActionResult CustomerApiTabs(Guid CustomerId)
        {
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            ViewBag.CustomerId = cus.CustomerId;
            ViewBag.AlarmCusId = cus.AlarmRefId;
            return PartialView();
        }

        public string GetCSNumber(Guid CustomerId, string Platform)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string CSNumber = "";
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
            #region Get CSNumber
            if (!string.IsNullOrEmpty(cus.CustomerNo))
            {
                CSNumber = cus.CustomerNo;
            }
            else
            {
                PackageCustomer packageCustomer = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerId(cus.CustomerId);
                if (packageCustomer != null)
                {
                    SmartPackage package = _Util.Facade.SmartPackageFacade.GetPackageByPackageIdAndCompanyId(packageCustomer.PackageId, CurrentUser.CompanyId.Value);
                    if (package != null)
                    {
                        CustomerSystemNo custNumber = _Util.Facade.CustomerSystemNoFacade.GetAllOpenCustomerSystemNoByCompanyIdandPlatformPrifix(CurrentUser.CompanyId.Value, package.CustomerNumber).FirstOrDefault();
                        if (custNumber != null)
                        {
                            CSNumber = custNumber.CustomerNo;
                        }
                    }
                }
                if (string.IsNullOrEmpty(CSNumber))
                {

                    CustomerNoPrefix cusPrifix = _Util.Facade.CustomerSystemNoFacade.GetAllNumberPrefixByCentralstationName(CurrentUser.CompanyId.Value, Platform);

                    List<CustomerSystemNo> customerSystemNoList = new List<CustomerSystemNo>();
                    if (cusPrifix != null)
                    {
                        customerSystemNoList = _Util.Facade.CustomerSystemNoFacade.GetAllOpenCustomerSystemNoByCompanyIdandPlatformPrifix(CurrentUser.CompanyId.Value, cusPrifix.Name);

                        if (customerSystemNoList != null && customerSystemNoList.Count > 0)
                        {
                            CustomerSystemNo cusSysNo = customerSystemNoList.FirstOrDefault();
                            CSNumber = cusSysNo.CustomerNo;
                        }
                    }
                }
            }

            #endregion
            return CSNumber;
        }

        #region Brinks
        [Authorize]
        public ActionResult Brinks(Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<BrinksTestCategory> cetegoryList = new List<BrinksTestCategory>();
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (!string.IsNullOrEmpty(cus.BrinksRefId))
            {
                bool InProduction = false;
                #region IsInProduction
                var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
                if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
                {
                    InProduction = true;
                }
                else
                {
                    InProduction = false;
                }
                #endregion
                //HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI(InProduction);
                HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();

                string Entity = "Testcats";

                string UserId = "";
                string Password = "";

                string DelearId = "";

                #region Credentials
                var DealerIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksDelearId");
                if (DealerIdGlobal != null)
                {
                    DelearId = DealerIdGlobal.Value;
                }
                var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
                if (UserIdGlobal != null)
                {
                    UserId = UserIdGlobal.Value;
                }
                var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
                if (PasswordGlobal != null)
                {
                    Password = PasswordGlobal.Value;
                }
                string CsNumber = cus.BrinksRefId;
                var result = wsi.GetData(Entity, UserId, Password, CsNumber, null);

                if (result != null)
                {
                    var dataTable = result.Tables[0];
                    cetegoryList = (from DataRow dr in dataTable.Rows
                                    select new BrinksTestCategory()
                                    {
                                        testcat_id = dr["testcat_id"].ToString(),
                                        descr = dr["descr"].ToString(),
                                        default_hours = dr["default_hours"].ToString(),
                                    }).ToList();
                    System.Web.HttpRuntime.Cache["GetTestingCategory"] = cetegoryList;
                }


                List<SelectListItem> TestCategory = new List<SelectListItem>();
                TestCategory.Add(new SelectListItem()
                {
                    Text = "Select Category",
                    Value = "-1"
                });
                TestCategory.AddRange(cetegoryList.Select(x => new SelectListItem()
                {
                    Text = x.descr,
                    Value = x.testcat_id
                }).ToList());
                ViewBag.TestCategoryList = TestCategory;
                #endregion
            }


            return View(cus);
        }

        public ActionResult EditGlobalSettingsBrinksSearchkeyandValue()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            GlobalSetting globalSetting = new GlobalSetting();
            BrinksCredentialsSetting brinksCredentials = new BrinksCredentialsSetting();

            globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
            brinksCredentials.BrinksUserId = globalSetting.Value;

            globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
            brinksCredentials.BrinksPassword = globalSetting.Value;

            globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksDelearId");
            brinksCredentials.BrinksDelearId = globalSetting.Value;

            globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            brinksCredentials.BrinksInProduction = bool.Parse(globalSetting.Value);

            globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksCreditCheck");
            brinksCredentials.BrinksCreditCheck = bool.Parse(globalSetting.Value);

            return PartialView("EditGlobalSettingsBrinksSearchkeyandValue", brinksCredentials);
        }

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public JsonResult SaveGlobalSettingsBrinksSearchkeyandValue(BrinksCredentialsSetting brinksCredentials)
        {
            bool result = false;
            string message = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            GlobalSetting globalSetting = new GlobalSetting();
            try
            {
                globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
                if (!String.IsNullOrEmpty(brinksCredentials.BrinksUserId))
                {
                    globalSetting.Value = brinksCredentials.BrinksUserId;
                    _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(globalSetting);
                }

                globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
                if (!String.IsNullOrEmpty(brinksCredentials.BrinksPassword))
                {
                    globalSetting.Value = brinksCredentials.BrinksPassword;
                    _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(globalSetting);
                }

                globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksDelearId");
                if (!String.IsNullOrEmpty(brinksCredentials.BrinksDelearId))
                {
                    globalSetting.Value = brinksCredentials.BrinksDelearId;
                    _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(globalSetting);
                }
                globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
                if (brinksCredentials.BrinksInProduction == false || brinksCredentials.BrinksInProduction == true)
                {
                    globalSetting.Value = brinksCredentials.BrinksInProduction.ToString();
                    _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(globalSetting);
                }

                globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksCreditCheck");
                if (brinksCredentials.BrinksCreditCheck == false || brinksCredentials.BrinksCreditCheck == true)
                {
                    globalSetting.Value = brinksCredentials.BrinksCreditCheck.ToString();
                    _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(globalSetting);
                }
                message = "updated successfully";
                result = true;
            }
            catch (Exception ex)
            {
                message = "update failed";
                logger.Error(message, ex);
            }
            return Json(new { result = result, message = message });
        }

        [Authorize]
        public JsonResult PlaceAccountOnTest(Guid CustomerGuidId, string TestCategory, string TestHour, string TestSec)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string message = "";
            string BrinksRefId = "";
            bool result = false;
            bool InProduction = false;
            #region IsInProduction
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerGuidId);
            if (cus != null)
            {
                BrinksRefId = cus.BrinksRefId;
            }
            #endregion
            if (!string.IsNullOrEmpty(BrinksRefId))
            {
                #region Place test
                HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();
                HS.Brinks.net.monitronics.senti.ApplicationIDHeader applicationIDHeader = new HS.Brinks.net.monitronics.senti.ApplicationIDHeader();
                applicationIDHeader.appID = "WSI";
                wsi.ApplicationIDHeaderValue = applicationIDHeader;

                string Entity = "OnTest";
                string UserId = "";
                string Password = "";
                #region Credentials

                var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
                if (UserIdGlobal != null)
                {
                    UserId = UserIdGlobal.Value;
                }
                var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
                if (PasswordGlobal != null)
                {
                    Password = PasswordGlobal.Value;
                }
                List<BrinksTestCategory> CategorylList = (List<BrinksTestCategory>)System.Web.HttpRuntime.Cache["GetTestingCategory"];
                BrinksTestCategory category = new BrinksTestCategory();
                if (TestHour == "-1")
                {
                    if (!string.IsNullOrEmpty(TestCategory))
                    {
                        TestHour = "0";
                        category = CategorylList.Where(x => x.testcat_id == TestCategory).FirstOrDefault();
                        if (category != null)
                        {
                            TestHour = category.default_hours;
                        }
                    }
                    else
                    {
                        TestHour = "0";
                    }

                }
                if (TestSec == "-1")
                {
                    if (!string.IsNullOrEmpty(TestCategory))
                    {

                    }
                    TestSec = "0";
                }
                if (TestCategory == "-1")
                {
                    TestCategory = "";
                }
                #endregion
                var TestXML = string.Format(@"<?xml version='1.0'?> <OnTests xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>  <OnTest onoff_flag='On' testcat_id='{2}' test_hours='{0}' test_minutes='{1}' /> </OnTests>", TestHour, TestSec, TestCategory);
                var TestData = wsi.Immediate(Entity, UserId, Password, BrinksRefId, TestXML);
                if (TestData != null)
                {
                    message = BrinksErrorMsg(TestData);
                    if (message == "")
                    {
                        result = true;
                        message = "Account placed on test successfully.";

                    }
                    else
                    {
                        result = false;

                    }
                }

                #endregion
            }
            return Json(new { result = result, message = message });
        }

        [Authorize]
        public JsonResult PlaceAccountOutOfService(Guid CustomerGuidId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string message = "";
            string BrinksRefId = "";
            bool result = false;
            bool InProduction = false;
            #region IsInProduction
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerGuidId);
            if (cus != null)
            {
                BrinksRefId = cus.BrinksRefId;
            }
            #endregion
            if (!string.IsNullOrEmpty(BrinksRefId))
            {
                #region Place test
                HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();
                HS.Brinks.net.monitronics.senti.ApplicationIDHeader applicationIDHeader = new HS.Brinks.net.monitronics.senti.ApplicationIDHeader();
                applicationIDHeader.appID = "WSI";
                wsi.ApplicationIDHeaderValue = applicationIDHeader;

                string Entity = "OutOfService";
                string UserId = "";
                string Password = "";
                #region Credentials

                var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
                if (UserIdGlobal != null)
                {
                    UserId = UserIdGlobal.Value;
                }
                var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
                if (PasswordGlobal != null)
                {
                    Password = PasswordGlobal.Value;
                }
                List<BrinksTestCategory> CategorylList = (List<BrinksTestCategory>)System.Web.HttpRuntime.Cache["GetTestingCategory"];
                BrinksTestCategory category = new BrinksTestCategory();

                #endregion
                var TestXML = @"<?xml version='1.0'?> <OutOfServices xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>  <OutOfService status_flag='O' ooscat_id='DLRREQ' /> </OutOfServices>";
                var TestData = wsi.Immediate(Entity, UserId, Password, BrinksRefId, TestXML);
                if (TestData != null)
                {
                    message = BrinksErrorMsg(TestData);
                    if (message == "")
                    {
                        result = true;
                        message = "Account placed on Out Of Service.";

                    }
                    else
                    {
                        result = false;

                    }
                }

                #endregion
            }
            return Json(new { result = result, message = message });
        }
        [Authorize]
        public JsonResult SyncBrinksCustomer(string BrinksRefId, Guid CustomerGuidId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string message = "Successfully Sync";
            bool result = false;
            bool InProduction = false;
            #region IsInProduction
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            #endregion
            var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerGuidId);
            Customer ExistBrinksCustomer = _Util.Facade.CustomerFacade.IsCustomerBrinksExistCheck(BrinksRefId).FirstOrDefault();
            if (ExistBrinksCustomer == null)
            {
                if (CustomerDetails != null)
                {
                    HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();
                    string Entity = "Sitesystems";
                    string UserId = "";
                    string Password = "";
                    #region Credentials

                    var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
                    if (UserIdGlobal != null)
                    {
                        UserId = UserIdGlobal.Value;
                    }
                    var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
                    if (PasswordGlobal != null)
                    {
                        Password = PasswordGlobal.Value;
                    }
                    SetupBrinks setUpBrink = new SetupBrinks();
                    #endregion
                    var SiteInfo = wsi.GetData(Entity, UserId, Password, BrinksRefId, null);
                    if (SiteInfo != null)
                    {
                        try
                        {
                            var dataTable1 = SiteInfo.Tables[0];
                            setUpBrink = (from DataRow dr in dataTable1.Rows
                                          select new SetupBrinks()
                                          {
                                              site_name = dr["site_name"].ToString(),
                                          }).FirstOrDefault();


                            result = true;
                            message = "Account placed on test successfully.";

                            CustomerDetails.BrinksRefId = BrinksRefId;
                            result = _Util.Facade.CustomerFacade.UpdateCustomer(CustomerDetails);
                            #region Update Customer System No
                            CustomerSystemNo cusno = new CustomerSystemNo();
                            cusno = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerNo(CustomerDetails.BrinksRefId);
                            if (cusno != null)
                            {
                                cusno.IsUsed = true;
                                cusno.CustomerId = CustomerDetails.Id;
                                cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                                _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(cusno);
                            }
                            #endregion
                            #region Udate Zones
                            Entity = "Zones";
                            var ZoneData = wsi.GetData(Entity, UserId, Password, BrinksRefId, null);
                            List<ZoneObject> zoneList = new List<ZoneObject>();
                            if (ZoneData != null)
                            {
                                var dataTable = ZoneData.Tables[0];
                                zoneList = (from DataRow dr in dataTable.Rows
                                            select new ZoneObject()
                                            {
                                                cs_no = dr["cs_no"].ToString(),
                                                zone_id = dr["zone_id"].ToString(),
                                                zonestate_id = dr["zonestate_id"].ToString(),
                                                event_id = dr["event_id"].ToString(),
                                                equiploc_id = dr["equiploc_id"].ToString(),
                                                equiptype_id = dr["equiptype_id"].ToString(),
                                                zone_comment = dr["zone_comment"].ToString()

                                            }).ToList();
                                List<CustomerSecurityZones> cusZoneList = new List<CustomerSecurityZones>();
                                cusZoneList = _Util.Facade.CustomerFacade.GetAllCustomerSecurityZoneByCustomerId(CustomerGuidId, "'Brinks'");
                                if (cusZoneList != null && cusZoneList.Count > 0)
                                {
                                    foreach (var item in cusZoneList)
                                    {
                                        _Util.Facade.CustomerFacade.DeleteCustomerSecurityZone(item.ID);
                                    }
                                }
                                CustomerSecurityZones securityZone = new CustomerSecurityZones();
                                if (zoneList != null && zoneList.Count > 0)
                                {
                                    foreach (var item in zoneList)
                                    {
                                        securityZone = new CustomerSecurityZones()
                                        {
                                            ZoneNumber = item.zone_id,
                                            EventCode = item.event_id,
                                            EquipmentType = item.equiptype_id,
                                            Location = item.equiploc_id,
                                            Platform = "Brinks",
                                            CustomerId = CustomerGuidId
                                        };
                                        _Util.Facade.CustomerFacade.InsertCustomerSecurityZone(securityZone);
                                    }

                                }


                            }

                            #endregion

                        }
                        catch (Exception ex)
                        {
                            result = false;
                            message = "Account not found";
                            logger.Error(message, ex );
                        }


                    }
                    else
                    {
                        result = false;
                    }



                }
            }
            else
            {
                result = false;
                message = "This Brinks id already synced with " + ExistBrinksCustomer.FirstName + " " + ExistBrinksCustomer.LastName + "(Id:" + ExistBrinksCustomer.Id + ")";
            }

            return Json(new { result = result, message = message });
        }

        public ActionResult AddTwoWayTest()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool InProduction = false;
            #region IsInProduction
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            #endregion
            HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();

            string Entity = "Twoways";

            string UserId = "";
            string Password = "";

            string DelearId = "";

            #region Credentials
            var DealerIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksDelearId");
            if (DealerIdGlobal != null)
            {
                DelearId = DealerIdGlobal.Value;
            }
            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
            if (UserIdGlobal != null)
            {
                UserId = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion
            List<BrinksDeviceList> DeviceList = new List<BrinksDeviceList>();
            var result = wsi.GetData(Entity, UserId, Password, null, null);
            if (result != null)
            {
                var dataTable = result.Tables[0];
                DeviceList = (from DataRow dr in dataTable.Rows
                              select new BrinksDeviceList()
                              {
                                  twoway_device_id = dr["twoway_device_id"].ToString(),
                                  descr = dr["descr"].ToString(),
                              }).ToList();

            }
            List<SelectListItem> TestDevice = new List<SelectListItem>();
            TestDevice.Add(new SelectListItem()
            {
                Text = "Select Device",
                Value = "-1"
            });
            TestDevice.AddRange(DeviceList.Select(x => new SelectListItem()
            {
                Text = x.descr,
                Value = x.twoway_device_id
            }).ToList());
            ViewBag.TestDeviceList = TestDevice;

            return View();
        }
        [HttpPost]
        public JsonResult AddTwoWayTest(string CSNumber, Guid CustomerId, string DeviceId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool InProduction = false;
            #region IsInProduction
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            #endregion
            HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();
            HS.Brinks.net.monitronics.senti.ApplicationIDHeader applicationIDHeader = new HS.Brinks.net.monitronics.senti.ApplicationIDHeader();
            applicationIDHeader.appID = "WSI";
            wsi.ApplicationIDHeaderValue = applicationIDHeader;

            string Entity = "Twoway";
            string message = "";
            bool result = false;
            string UserId = "";
            string Password = "";

            string DelearId = "";

            #region Credentials
            var DealerIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksDelearId");
            if (DealerIdGlobal != null)
            {
                DelearId = DealerIdGlobal.Value;
            }
            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
            if (UserIdGlobal != null)
            {
                UserId = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            string TestXML = string.Format(@"<?xml version='1.0'?> <Twoways xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'> <Twoway twoway_device_id='{0}' /> </Twoways>", DeviceId);
            #endregion
            var TestData = wsi.Immediate(Entity, UserId, Password, CSNumber, TestXML);
            if (TestData != null)
            {
                message = BrinksErrorMsg(TestData);
                if (message == "")
                {
                    result = true;
                    message = "Account is placed for two-way test.";
                    Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
                    if (cus != null)
                    {
                        cus.BrinksRefId = CSNumber;
                        result = _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                        #region Update Customer System No
                        CustomerSystemNo cusno = new CustomerSystemNo();
                        cusno = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerNo(cus.BrinksRefId);
                        if (cusno != null)
                        {
                            cusno.IsUsed = true;
                            cusno.CustomerId = cus.Id;
                            cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                            _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(cusno);
                        }
                        #endregion
                    }

                }
                else
                {
                    result = false;

                }
            }
            return Json(new { result = result, message = message });
        }
        [Authorize]
        public ActionResult BrinksCustomerDetails(string BrinksRefId, Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SetupBrinks setUpBrink = new SetupBrinks();
            bool InProduction = false;
            #region IsInProduction
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            #endregion
            HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();

            string Entity = "Sitesystems";
            //string UserId = "ADSTest_WSI";
            //string Password = "Brinks!12345";

            string UserId = "";
            string Password = "";

            string DelearId = "";

            #region Credentials
            var DealerIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksDelearId");
            if (DealerIdGlobal != null)
            {
                DelearId = DealerIdGlobal.Value;
            }
            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
            if (UserIdGlobal != null)
            {
                UserId = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion

            string CsNumber = BrinksRefId;
            var result = wsi.GetData(Entity, UserId, Password, CsNumber, null);

            if (result != null)
            {
                var dataTable = result.Tables[0];
                setUpBrink = (from DataRow dr in dataTable.Rows
                              select new SetupBrinks()
                              {
                                  site_name = dr["site_name"].ToString(),
                                  sitetype_id = dr["sitetype_id"].ToString(),
                                  sitestat_id = dr["sitestat_id"].ToString(),
                                  site_addr1 = dr["site_addr1"].ToString(),
                                  site_addr2 = dr["site_addr2"].ToString(),
                                  city_name = dr["city_name"].ToString(),
                                  county_name = dr["county_name"].ToString(),
                                  state_id = dr["state_id"].ToString(),
                                  zip_code = dr["zip_code"].ToString(),
                                  phone1 = dr["phone1"].ToString(),
                                  timezone_descr = dr["timezone_descr"].ToString(),
                                  servco_no = dr["servco_no"].ToString(),
                                  install_servco_no = dr["install_servco_no"].ToString(),
                                  cspart_no = dr["cspart_no"].ToString(),
                                  subdivision = dr["subdivision"].ToString(),
                                  cross_street = dr["cross_street"].ToString(),
                                  codeword1 = dr["codeword1"].ToString(),
                                  codeword2 = dr["codeword2"].ToString(),
                                  orig_install_date = dr["orig_install_date"] != DBNull.Value ? Convert.ToDateTime(dr["orig_install_date"]) : new DateTime(),
                                  lang_id = dr["lang_id"].ToString(),
                                  cs_no = dr["cs_no"].ToString(),
                                  systype_id = dr["systype_id"].ToString(),
                                  sec_systype_id = dr["sec_systype_id"].ToString(),
                                  panel_phone = dr["panel_phone"].ToString(),
                                  panel_location = dr["panel_location"].ToString(),
                                  receiver_phone = dr["receiver_phone"].ToString(),
                                  ati_hours = dr["ati_hours"].ToString(),
                                  ati_minutes = dr["ati_minutes"].ToString(),
                                  panel_code = dr["panel_code"].ToString(),
                                  twoway_device_id = dr["twoway_device_id"].ToString(),
                                  alkup_cs_no = dr["alkup_cs_no"].ToString(),
                                  blkup_cs_no = dr["blkup_cs_no"].ToString(),
                                  ontest_flag = dr["ontest_flag"].ToString(),
                                  ontest_expire_date = dr["ontest_expire_date"].ToString(),
                                  oos_flag = dr["oos_flag"].ToString(),
                                  install_date = dr["install_date"] != DBNull.Value ? Convert.ToDateTime(dr["install_date"]) : new DateTime(),
                                  monitor_type = dr["monitor_type"].ToString()
                              }).ToList().FirstOrDefault();

                #region ZoneList
                Entity = "Zones";

                var ZoneData = wsi.GetData(Entity, UserId, Password, CsNumber, null);
                List<ZoneObject> zoneList = new List<ZoneObject>();
                if (ZoneData != null)
                {
                    var zonedataTable = ZoneData.Tables[0];
                    zoneList = (from DataRow dr in zonedataTable.Rows
                                select new ZoneObject()
                                {
                                    cs_no = dr["cs_no"].ToString(),
                                    zone_id = dr["zone_id"].ToString(),
                                    zonestate_id = dr["zonestate_id"].ToString(),
                                    event_id = dr["event_id"].ToString(),
                                    equiploc_id = dr["equiploc_id"].ToString(),
                                    equiptype_id = dr["equiptype_id"].ToString(),
                                    zone_comment = dr["zone_comment"].ToString()
                                }).ToList();
                    setUpBrink.ZoneObjectList = zoneList;
                }
                #endregion

                #region Contact List
                Entity = "Contacts";

                var ContactData = wsi.GetData(Entity, UserId, Password, CsNumber, null);
                List<BrinksContactObject> contactList = new List<BrinksContactObject>();
                if (ContactData != null)
                {
                    var contactdataTable = ContactData.Tables[0];
                    contactList = (from DataRow dr in contactdataTable.Rows
                                   select new BrinksContactObject()
                                   {
                                       first_name = dr["first_name"].ToString(),
                                       last_name = dr["last_name"].ToString(),
                                       phone1 = dr["phone1"].ToString(),
                                       contact_no = dr["contact_no"].ToString(),
                                       ctaclink_no = dr["ctaclink_no"].ToString(),
                                       relation_id = dr["relation_id"].ToString(),
                                       has_key_flag = dr["has_key_flag"].ToString(),

                                   }).ToList();
                    List<EmergencyContact> thirdPartyContactList = new List<EmergencyContact>();
                    thirdPartyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerId(CustomerId);
                    EmergencyContact emgContact = new EmergencyContact();
                    foreach (var item in thirdPartyContactList)
                    {
                        _Util.Facade.EmergencyContactFacade.DeleteEmergencyContactById(item.Id);
                    }
                    if (contactList != null && contactList.Count > 0)
                    {
                        foreach (var item in contactList)
                        {
                            emgContact = new EmergencyContact()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                FirstName = item.first_name,
                                LastName = item.last_name,
                                RelationShip = item.relation_id,
                                HasKey = item.has_key_flag,
                                Phone = item.phone1,
                                ContactNo = item.contact_no,
                                CustomerId = CustomerId,
                                Platform = "Brinks"
                            };
                            _Util.Facade.EmergencyContactFacade.InsertEmergencyContact(emgContact);
                        }
                    }
                    setUpBrink.contactList = contactList;
                }
                #endregion

                #region Get CommonFunding Data
                CustomerExtended extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(CustomerId);
                ViewBag.FundingData = false;

                if (extended != null)
                {
                    ViewBag.FundingData = extended.FundingResult;
                    ViewBag.FundingStatus = extended.BrinksFundingStatus;
                }

                #endregion

            }
            return View(setUpBrink);
        }

        public JsonResult GetEquipmentType(string EventCode)
        {
            string result = "[]";
            List<ZonesEquipmentTypeEventMapModel> eqpTypeMap = new List<ZonesEquipmentTypeEventMapModel>();
            eqpTypeMap = _Util.Facade.CustomerFacade.GetEquipmentTypeEventMapByEventCode(EventCode);
            if (eqpTypeMap.Count > 0)
                result = JsonConvert.SerializeObject(eqpTypeMap);
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveBrinksSignedInfo(BrinksSignedInfo signedInfo)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            BrinksSignedInfo oldSingenInfo = _Util.Facade.CustomerFacade.GetBrinksSignedInfoByCustomerId(signedInfo.CustomerId);
            bool result = false;
            if (oldSingenInfo != null)
            {
                oldSingenInfo.IsSigned = signedInfo.IsSigned;
                oldSingenInfo.HasBillingInfo = signedInfo.HasBillingInfo;
                oldSingenInfo.HasBusinessPicture = signedInfo.HasBusinessPicture;
                oldSingenInfo.IsCreditCheck = signedInfo.IsCreditCheck;
                oldSingenInfo.CreatedBy = CurrentUser.UserId;
                oldSingenInfo.CreatedDate = DateTime.Now.UTCCurrentTime();

                _Util.Facade.CustomerFacade.UpdateBrinksSignedInfo(oldSingenInfo);
                result = true;

            }
            else
            {
                signedInfo.CreatedBy = CurrentUser.UserId;
                signedInfo.CreatedDate = DateTime.Now.UTCCurrentTime();
                _Util.Facade.CustomerFacade.InsertBrinksSingendInfo(signedInfo);
                result = true;
            }

            if (result == true)
            {
                string message = "";
                if (signedInfo.IsSigned == true)
                {
                    message += "Contract";
                }
                if (signedInfo.HasBillingInfo == true)
                {
                    message += ",Billing Info";
                }
                if (signedInfo.HasBusinessPicture == true)
                {
                    message += ",Business Picture";
                }
                message = message.Replace(" ", ",");
                base.AddUserActivityForCustomer(message + "is checked for brinks customer.", LabelHelper.ActivityAction.MailSend, signedInfo.CustomerId, null, null);
            }
            return Json(new { result = result });
        }

        [Authorize]

        public ActionResult InsertZoneLocation()
        {
            string Entity = "Equiplocs";
            string UserId = "";
            string Password = "";
            bool InProduction = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            #region IsInProduction
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            #endregion
            #region Credentials

            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
            if (UserIdGlobal != null)
            {
                UserId = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion
            HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();
            var result = wsi.GetData(Entity, UserId, Password, null, null);
            List<BrinksEquipmentLocation> zonesLocationList = new List<BrinksEquipmentLocation>();
            if (result != null)
            {
                var dataTable = result.Tables[0];
                zonesLocationList = (from DataRow dr in dataTable.Rows
                                     select new BrinksEquipmentLocation()
                                     {
                                         descr = dr["descr"].ToString(),
                                         equiploc_id = dr["equiploc_id"].ToString()

                                     }).ToList();

                ZonesEquipmentLocation xoneLocation = new ZonesEquipmentLocation();
                if (zonesLocationList != null && zonesLocationList.Count > 0)
                {
                    foreach (var item in zonesLocationList)
                    {
                        xoneLocation = new ZonesEquipmentLocation()
                        {
                            EquipmentLocation = item.descr,
                            EquipmentLocationId = item.equiploc_id,
                            Platform = "Brinks"

                        };
                        _Util.Facade.CustomerFacade.InsertZoneEquipmentLocation(xoneLocation);
                    }
                }
            }
            return null;
        }

        [Authorize]
        public ActionResult InsertEquipmentType()
        {
            string Entity = "Equiptypes";
            string UserId = "";
            string Password = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool InProduction = false;

            #region IsInProduction
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            #endregion

            #region Credentials

            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
            if (UserIdGlobal != null)
            {
                UserId = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion
            HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();
            var result = wsi.GetData(Entity, UserId, Password, null, null);
            List<BrinksEquipmentType> zonesEqpTypeList = new List<BrinksEquipmentType>();
            if (result != null)
            {
                var dataTable = result.Tables[0];
                zonesEqpTypeList = (from DataRow dr in dataTable.Rows
                                    select new BrinksEquipmentType()
                                    {
                                        descr = dr["descr"].ToString(),
                                        equiptype_id = dr["equiptype_id"].ToString()

                                    }).ToList();

                ZonesEquipmentType xoneType = new ZonesEquipmentType();
                if (zonesEqpTypeList != null && zonesEqpTypeList.Count > 0)
                {
                    foreach (var item in zonesEqpTypeList)
                    {
                        xoneType = new ZonesEquipmentType()
                        {
                            EqpmentTypeId = item.equiptype_id,
                            EquipmentType = item.descr,
                            Platform = "Brinks"

                        };
                        _Util.Facade.CustomerFacade.InsertZoneEquipmentType(xoneType);
                    }
                }
            }
            return null;
        }


        [Authorize]
        public ActionResult InsertEquipmentTypeEventMap()
        {
            string Entity = "Equip_event_xref";
            string UserId = "";
            string Password = "";
            string DelearId = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool InProduction = false;

            #region IsInProduction
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            #endregion
            #region Credentials
            var DealerIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksDelearId");
            if (DealerIdGlobal != null)
            {
                DelearId = DealerIdGlobal.Value;
            }
            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
            if (UserIdGlobal != null)
            {
                UserId = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion
            HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();
            var result = wsi.GetData(Entity, UserId, Password, null, null);
            List<BrinksEquipmentTypeEventMap> zonesEqpTypeEventMapList = new List<BrinksEquipmentTypeEventMap>();
            if (result != null)
            {
                var dataTable = result.Tables[0];
                zonesEqpTypeEventMapList = (from DataRow dr in dataTable.Rows
                                            select new BrinksEquipmentTypeEventMap()
                                            {
                                                equiptype_id = dr["equiptype_id"].ToString(),
                                                event_id = dr["event_id"].ToString()

                                            }).ToList();

                ZonesEquipmentTypeEventMap xoneTypeMap = new ZonesEquipmentTypeEventMap();
                if (zonesEqpTypeEventMapList != null && zonesEqpTypeEventMapList.Count > 0)
                {
                    foreach (var item in zonesEqpTypeEventMapList)
                    {
                        xoneTypeMap = new ZonesEquipmentTypeEventMap()
                        {
                            EquipmentTypeId = item.equiptype_id,
                            EventId = item.event_id,
                        };
                        _Util.Facade.CustomerFacade.InsertZoneEquipmentTypeEventMap(xoneTypeMap);
                    }
                }
            }
            return null;
        }
        [Authorize]
        public ActionResult AddBrinksAccount(Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool InProduction = false;

            #region IsInProduction
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            #endregion
            HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();
            SetupBrinks model = new SetupBrinks();
            ViewBag.CsNumber = "";
            string Entity = "";
            //string UserId = "ADSTest_WSI";
            //string Password = "Brinks!12345";

            string UserId = "";
            string Password = "";
            string CsNumber = "";
            string DelearId = "";
            #region Credentials
            var DealerIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksDelearId");
            if (DealerIdGlobal != null)
            {
                DelearId = DealerIdGlobal.Value;
            }
            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
            if (UserIdGlobal != null)
            {
                UserId = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion

            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
            var emContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerId(CustomerId);
            CustomerSecurityZones securityZones = new CustomerSecurityZones();
            List<CustomerSecurityZones> securityZoneList = new List<CustomerSecurityZones>();
            securityZoneList = _Util.Facade.CustomerFacade.GetAllCustomerSecurityZoneByCustomerId(CustomerId, "'Brinks'");
            List<Alarm.AlarmCustomer.PanelDevice> divicelist = new List<HS.Alarm.AlarmCustomer.PanelDevice>();
            divicelist = GetAlarmSystemInfoList(CustomerId);

            if (cus != null)
            {
                #region Edit Site
                if (!string.IsNullOrEmpty(cus.BrinksRefId))
                {
                    ViewBag.CsNumber = cus.BrinksRefId;
                    Entity = "Sitesystems";

                    CsNumber = cus.BrinksRefId;
                    var result = wsi.GetData(Entity, UserId, Password, CsNumber, null);
                    if (result != null)
                    {
                        var dataTable = result.Tables[0];
                        model = (from DataRow dr in dataTable.Rows
                                 select new SetupBrinks()
                                 {
                                     site_name = dr["site_name"].ToString(),
                                     sitetype_id = dr["sitetype_id"].ToString(),
                                     sitestat_id = dr["sitestat_id"].ToString(),
                                     site_addr1 = dr["site_addr1"].ToString(),
                                     site_addr2 = dr["site_addr2"].ToString(),
                                     city_name = dr["city_name"].ToString(),
                                     county_name = dr["county_name"].ToString(),
                                     state_id = dr["state_id"].ToString(),
                                     zip_code = dr["zip_code"].ToString(),
                                     phone1 = dr["phone1"].ToString(),
                                     timezone_descr = dr["timezone_descr"].ToString(),
                                     servco_no = dr["servco_no"].ToString(),
                                     install_servco_no = dr["install_servco_no"].ToString(),
                                     cspart_no = dr["cspart_no"].ToString(),
                                     subdivision = dr["subdivision"].ToString(),
                                     cross_street = dr["cross_street"].ToString(),
                                     codeword1 = dr["codeword1"].ToString(),
                                     codeword2 = dr["codeword2"].ToString(),
                                     orig_install_date = dr["orig_install_date"] != DBNull.Value ? Convert.ToDateTime(dr["orig_install_date"]) : new DateTime(),
                                     lang_id = dr["lang_id"].ToString(),
                                     cs_no = dr["cs_no"].ToString(),
                                     systype_id = dr["systype_id"].ToString(),
                                     sec_systype_id = dr["sec_systype_id"].ToString(),
                                     panel_phone = dr["panel_phone"].ToString(),
                                     panel_location = dr["panel_location"].ToString(),
                                     receiver_phone = dr["receiver_phone"].ToString(),
                                     ati_hours = dr["ati_hours"].ToString(),
                                     ati_minutes = dr["ati_minutes"].ToString(),
                                     panel_code = dr["panel_code"].ToString(),

                                     twoway_device_id = dr["twoway_device_id"].ToString(),
                                     alkup_cs_no = dr["alkup_cs_no"].ToString(),
                                     blkup_cs_no = dr["blkup_cs_no"].ToString(),
                                     ontest_flag = dr["ontest_flag"].ToString(),
                                     ontest_expire_date = dr["ontest_expire_date"].ToString(),
                                     oos_flag = dr["oos_flag"].ToString(),
                                     install_date = dr["install_date"] != DBNull.Value ? Convert.ToDateTime(dr["install_date"]) : DateTime.Now,
                                     monitor_type = dr["monitor_type"].ToString()
                                 }).ToList().FirstOrDefault();
                    }
                    model.site_name = model.site_name.Replace("|", " ");
                    model.site_name = model.site_name.Substring(1);
                    model.site_name = model.site_name.Replace("  ", " ");
                    #region Edit Contact List
                    Entity = "Contacts";
                    CsNumber = cus.BrinksRefId;

                    var ContactData = wsi.GetData(Entity, UserId, Password, CsNumber, null);
                    List<BrinksContactObject> contactList = new List<BrinksContactObject>();
                    if (ContactData != null)
                    {
                        var contactdataTable = ContactData.Tables[0];
                        contactList = (from DataRow dr in contactdataTable.Rows
                                       select new BrinksContactObject()
                                       {
                                           first_name = dr["first_name"].ToString(),
                                           last_name = dr["last_name"].ToString(),
                                           phone1 = dr["phone1"].ToString(),
                                           contact_no = dr["contact_no"].ToString(),
                                           ctaclink_no = dr["ctaclink_no"].ToString(),
                                           has_key_flag = dr["has_key_flag"].ToString(),

                                       }).ToList();
                        model.contactList = contactList;
                    }
                    #endregion

                    #region CustomerSecurityZone

                    if (divicelist != null && divicelist.Count > 0)
                    {

                        foreach (var item in divicelist)
                        {
                            if (securityZoneList.Find(x => x.ZoneNumber == item.DeviceId.ToString()) == null)
                            {
                                securityZones = new CustomerSecurityZones()
                                {
                                    CustomerId = CustomerId,
                                    ZoneNumber = item.DeviceId.ToString(),
                                    Location = item.WebSiteDeviceName,
                                    Platform = "Brinks"
                                };
                                _Util.Facade.CustomerFacade.InsertCustomerSecurityZone(securityZones);
                            }
                        }

                    }

                    #endregion
                    #endregion
                }
                else
                {
                    Ticket tic = _Util.Facade.TicketFacade.GetAllInstallationTicketByCustomerId(cus.CustomerId).FirstOrDefault();
                    #region Insert Site
                    model.cs_no = GetCSNumber(cus.CustomerId, "Brinks");
                    model.site_name = cus.FirstName + " " + cus.LastName;
                    model.site_addr1 = cus.Street;
                    model.city_name = cus.City;
                    model.state_id = cus.State;
                    model.zip_code = cus.ZipCode;
                    model.cross_street = cus.CrossStreet;
                    model.county_name = cus.County;
                    model.panel_phone = cus.CellNo;
                    model.phone1 = cus.PrimaryPhone;
                    model.servco_no = "809240005";
                    model.receiver_phone = "8443432470";
                    model.codeword1 = cus.Passcode;
                    if (tic != null)
                    {
                        model.install_date = tic.CompletionDate != null && tic.CompletionDate != new DateTime() ? tic.CompletionDate : DateTime.Now;
                    }
                    else
                    {
                        model.install_date = DateTime.Now;
                    }

                    #endregion
                    #region CustomerSecurityZone

                    if (divicelist != null && divicelist.Count > 0)
                    {
                        if (securityZoneList != null && securityZoneList.Count > 0)
                        {

                            foreach (var item in divicelist)
                            {
                                if (securityZoneList.Find(x => x.ZoneNumber == item.DeviceId.ToString()) == null)
                                {
                                    securityZones = new CustomerSecurityZones()
                                    {
                                        CustomerId = CustomerId,
                                        ZoneNumber = item.DeviceId.ToString(),
                                        Location = item.WebSiteDeviceName,
                                        Platform = "Brinks"
                                    };
                                    _Util.Facade.CustomerFacade.InsertCustomerSecurityZone(securityZones);
                                }
                            }

                        }
                        else
                        {
                            foreach (var item in divicelist)
                            {
                                securityZones = new CustomerSecurityZones()
                                {
                                    CustomerId = CustomerId,
                                    ZoneNumber = item.DeviceId.ToString(),
                                    ZoneComment = item.WebSiteDeviceName,
                                    Platform = "Brinks"
                                };
                                _Util.Facade.CustomerFacade.InsertCustomerSecurityZone(securityZones);
                            }
                        }

                    }

                    #endregion
                }

            }

            ViewBag.BrinksEventCode = _Util.Facade.LookupFacade.GetLookupByKey("UccEventCode").Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();

            ViewBag.BrinksEventCode = _Util.Facade.LookupFacade.GetLookupByKey("BrinksEventCode").Select(x =>
              new SelectListItem()
              {
                  Text = x.DisplayText.ToString(),
                  Value = x.DataValue.ToString()
              }).ToList();

            ViewBag.BrinksEquipLocation = _Util.Facade.CustomerFacade.GetAllZoneEquipmentLocation().Select(x =>
              new SelectListItem()
              {
                  Text = x.EquipmentLocation.ToString(),
                  Value = x.EquipmentLocationId.ToString()
              }).ToList();

            ViewBag.Relationship = _Util.Facade.LookupFacade.GetLookupByKey("Relationship").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();

            List<SelectListItem> EqpTypeList = new List<SelectListItem>();
            EqpTypeList.Add(new SelectListItem()
            {
                Text = "Select Type",
                Value = "-1"
            });

            ViewBag.BrinksEquipType = EqpTypeList;

            ViewBag.AgencyTypeList = _Util.Facade.LookupFacade.GetLookupByKey("BrinksAgencyType").Select(x =>
              new SelectListItem()
              {
                  Text = x.DisplayText.ToString(),
                  Value = x.DataValue.ToString()
              }).ToList();
            ViewBag.PremType = _Util.Facade.LookupFacade.GetLookupByKey("BrinksPeermTypes").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            BrinksSignedInfo oldSingenInfo = _Util.Facade.CustomerFacade.GetBrinksSignedInfoByCustomerId(CustomerId);
            if (oldSingenInfo != null)
            {
                model.IsSigned = oldSingenInfo.IsSigned;
                model.HasBillingInfo = oldSingenInfo.HasBillingInfo;
                model.HasBusinessPicture = oldSingenInfo.HasBusinessPicture;
                model.IsCreditCheck = oldSingenInfo.IsCreditCheck;
            }
            #region permission for medatory checkbox
            if (IsPermitted(UserPermissions.CustomerPermissions.MendatoryCheckBoxForBrinks))
            {
                ViewBag.MendatoryCheckBox = "true";
            }
            #endregion

            return View(model);
        }


        public JsonResult CheckBrinksCreditScore(Guid CustomerId, int? ContactId)
        {

            bool result = false;
            string message = "";
            int matchcode = 0;
            SearchInfo searchModel = new SearchInfo();
            bool InProduction = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            #region IsInProduction
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            #endregion
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);

            if (cus != null)
            {
                CustomerAdditionalContact secondaryCredit = new CustomerAdditionalContact();
                if (ContactId.HasValue && ContactId > 0)
                {
                    secondaryCredit = _Util.Facade.AdditionalContactFacade.GetById(ContactId.Value);
                    if (secondaryCredit != null)
                    {
                        searchModel.FirstName = secondaryCredit.FirstName;
                        searchModel.LastName = secondaryCredit.LastName;
                        searchModel.State = secondaryCredit.CorpState;
                        searchModel.Zip = secondaryCredit.CorpZipCode;
                        searchModel.City = secondaryCredit.CorpCity;
                        searchModel.Address1 = secondaryCredit.CorpAddress;
                        searchModel.SSN = secondaryCredit.SSN;

                        if (secondaryCredit.DOB.HasValue)
                        {
                            searchModel.DoB = secondaryCredit.DOB.Value.ToString();
                        }

                    }
                }
                else
                {
                    searchModel.Address1 = cus.Address;
                    searchModel.City = cus.City;
                    searchModel.State = cus.State;
                    searchModel.Zip = cus.ZipCode;
                    if (!string.IsNullOrEmpty(cus.CellNo))
                    {
                        searchModel.Phone1 = cus.CellNo.Replace("-", "");
                    }

                    searchModel.FirstName = cus.FirstName;
                    searchModel.LastName = cus.LastName;

                    searchModel.SSN = cus.SSN;
                    if (cus.DateofBirth.HasValue)
                    {
                        searchModel.DoB = cus.DateofBirth.Value.ToString();
                    }
                }
                searchModel.ProcessName = "CreditCheck";
                searchModel.DealerName = "ADS";
                searchModel.DealerNumber = "809240005";
                searchModel.ApplicationName = "RMRCloud";


            }
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HS.Bounce.com.brinkshome.senti.Bouncer bounce = new Bounce.com.brinkshome.senti.Bouncer();
                MatchResult matchresult = bounce.Match(searchModel);
                if (matchresult != null)
                {
                    matchcode = matchresult.MatchCode;
                    if (matchresult.MatchCode == 0)
                    {
                        message = "Brinks have no credit score record for this customer. Do you want to generate?";
                    }
                    else if (matchresult.MatchCode == 1)
                    {
                        message = "Brinks already have credit score record for this customer. Do you want to regenerate?";
                    }
                    #region Insert on customer extended
                    CustomerExtended extended = new CustomerExtended();

                    extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                    if (extended != null)
                    {
                        extended.BounceMatchId = matchresult.MatchID;
                        extended.BounceStatus = matchresult.Status;
                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                    }
                    else
                    {
                        extended.CustomerId = cus.CustomerId;
                        extended.BounceMatchId = matchresult.MatchID;
                        extended.BounceStatus = matchresult.Status;
                        _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
                    }
                    #endregion

                }
                result = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }


            return Json(new { result = result, message = message, matchcode = matchcode });
        }
        public JsonResult AddBrinksAgency(CustomerThirdPartyAgency thirdPartyAgency)
        {

            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            try
            {

                _Util.Facade.CustomerFacade.InsertCustomerAgency(thirdPartyAgency);
                result = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, JsonConvert.SerializeObject(thirdPartyAgency));
            }


            return Json(new { result = result });
        }


        public JsonResult AddBrinksEmgContact(ThirdPartyEmergencyContact thirdPartyContact)
        {

            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            try
            {
                if (thirdPartyContact.Id > 0)
                {
                    ThirdPartyEmergencyContact contact = new ThirdPartyEmergencyContact();
                    contact = _Util.Facade.CustomerFacade.GetCustomerThirdPartyEmergencyContact(thirdPartyContact.Id);
                    if (contact != null)
                    {
                        contact.FirstName = thirdPartyContact.FirstName;
                        contact.LastName = thirdPartyContact.LastName;
                        contact.Phone = thirdPartyContact.Phone;
                        _Util.Facade.CustomerFacade.UpdateThirdpartyCustomerEmgContact(contact);
                        result = true;
                    }

                }
                else
                {
                    _Util.Facade.CustomerFacade.InsertThirdpartyCustomerEmgContact(thirdPartyContact);
                    result = true;
                }


            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }


            return Json(new { result = result });
        }

        public string BrinksErrorMsg(DataSet data)
        {
            string message = "";
            List<BrinksConfirmationModel> cunfirmModel = new List<BrinksConfirmationModel>();
            if (data != null)
            {
                var dataTable = data.Tables[0];
                cunfirmModel = (from DataRow dr in dataTable.Rows
                                select new BrinksConfirmationModel()
                                {
                                    table_name = dr["table_name"].ToString(),
                                    entry_id = dr["entry_id"].ToString(),
                                    site_no = dr["site_no"].ToString(),
                                    err_no = dr["err_no"].ToString(),
                                    msg_type = dr["msg_type"].ToString(),
                                    err_text = dr["err_text"].ToString(),
                                    err_date = dr["err_date"] != DBNull.Value ? Convert.ToDateTime(dr["err_date"]) : new DateTime(),
                                    cs_no = dr["cs_no"].ToString(),

                                }).ToList();
                int count = 1;
                if (cunfirmModel != null && cunfirmModel.Count > 0)
                {
                    foreach (var item in cunfirmModel)
                    {
                        if (item.err_no != "107130")
                        {
                            message += count + ". " + item.err_text + "</br>";
                            count++;
                        }

                    }
                }
            }
            return message;
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddBrinksAccount(SetupBrinks BrinksModel, UccEmergencyContactModel BrinksemgContact)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var result = false;
            string message = "";
            bool InProduction = false;
            #region Cs Id check
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(BrinksModel.CustomerId);
            CustomerSystemNo sysNo = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerNo(BrinksModel.cs_no);
            if (sysNo != null && sysNo.IsUsed == true)
            {
                BrinksModel.cs_no = GetCSNumber(cus.CustomerId, "Brinks");

            }
            #endregion

            #region Prepare Data

            HS.Brinks.net.monitronics.senti.ApplicationIDHeader applicationIDHeader = new HS.Brinks.net.monitronics.senti.ApplicationIDHeader();
            applicationIDHeader.appID = "WSI";
            #region IsInProduction
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            #endregion


            HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();
            wsi.ApplicationIDHeaderValue = applicationIDHeader;
            string UserId = "";
            string Password = "";
            string DelearId = "";



            #region Credentials
            var DealerIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksDelearId");
            if (DealerIdGlobal != null)
            {
                DelearId = DealerIdGlobal.Value;
            }
            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
            if (UserIdGlobal != null)
            {
                UserId = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }

            #endregion
            string Entity = "";

            BrinksModel.cspart_no = "350";
            BrinksModel.install_servco_no = BrinksModel.servco_no;
            BrinksModel.sitestat_id = "C";
            BrinksModel.sitetype_id = "RBFM";
            BrinksModel.subdivision = "HALL";
            BrinksModel.systype_id = "A2C007";
            BrinksModel.sec_systype_id = "20S001";
            BrinksModel.cross_street = "N/A";
            string installdate = "";
            string city = "";
            string state = "";
            string country = "";
            string ssn = "";
            if (!string.IsNullOrEmpty(cus.SSN))
            {
                ssn = cus.SSN.Replace("-", "");
            }
            else
            {
                result = false;
                message = "SSN Number required.";
                return Json(new { result = result, message = message });
            }
            if (!string.IsNullOrEmpty(BrinksModel.city_name))
            {
                city = BrinksModel.city_name.ToUpper();
            }
            if (!string.IsNullOrEmpty(BrinksModel.state_id))
            {
                state = BrinksModel.state_id.ToUpper();
            }
            if (!string.IsNullOrEmpty(BrinksModel.county_name))
            {
                country = BrinksModel.county_name.ToUpper();
            }
            if (!string.IsNullOrEmpty(cus.BrinksRefId))
            {
                installdate = BrinksModel.install_date.ToString("yyyy-MM-dd");
            }
            else
            {
                installdate = BrinksModel.install_date.ToString("MM/dd/yyyy");
            }
            string CsNumber = BrinksModel.cs_no;
            string PanelPhone = "";
            if (!string.IsNullOrEmpty(BrinksModel.panel_phone))
            {
                PanelPhone = BrinksModel.panel_phone.Replace("-", "");
            }
            string Phone1 = "";
            if (!string.IsNullOrEmpty(BrinksModel.phone1))
            {
                Phone1 = BrinksModel.phone1.Replace("-", "");
            }
            string ReceiverPhone = "";
            if (!string.IsNullOrEmpty(BrinksModel.receiver_phone))
            {
                ReceiverPhone = BrinksModel.receiver_phone.Replace("-", "");
            }
            #endregion

            #region Prepare Siteinfo Xml
            string SiteInfo = string.Format(@"<SiteSystem city_name='{0}' codeword1='{1}'
                                            county_name='{2}' cross_street='{3}' cspart_no='{4}'
                                            ext1='999' ext2='999' install_servco_no='{5}' lang_id='ENG'
                                            map_coord='12345' map_page='8' phone1='{21}'
                                            servco_no='{6}' site_addr1='{7}' 
                                            site_name='{8}' sitestat_id='{9}'
                                            sitetype_id='{10}' state_id='{11}' subdivision='{12}'
                                            systype_id='{13}' sec_systype_id = '{14}'
                                            receiver_phone='{15}' panel_phone='{16}'
                                            panel_location='{17}' install_date='{18}'
                                            panel_code='{19}' zip_code='{20}'/>",
                                            city,//0
                                            BrinksModel.codeword1,//1
                                            country,//2
                                            BrinksModel.cross_street,//3
                                            BrinksModel.cspart_no,//4
                                            BrinksModel.install_servco_no,//5
                                            BrinksModel.servco_no,//6
                                            BrinksModel.site_addr1,//7
                                            BrinksModel.site_name,//8
                                            BrinksModel.sitestat_id,//9
                                            BrinksModel.sitetype_id,//10
                                            state,//11
                                            BrinksModel.subdivision,//12
                                            BrinksModel.systype_id,//13
                                            BrinksModel.sec_systype_id,//14
                                            ReceiverPhone,//15
                                            PanelPhone,//16
                                            BrinksModel.panel_location,//17
                                            installdate,//18
                                            BrinksModel.panel_code,//19
                                            BrinksModel.zip_code,//20
                                            Phone1);//21
            #endregion

            #region Prepare Emergancy Contact Info Xml
            string ContactInfo = "";
            List<EmergencyContact> thirdPartyContactList = new List<EmergencyContact>();

            thirdPartyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerId(cus.CustomerId);
            if (thirdPartyContactList != null && thirdPartyContactList.Count > 0)
            {
                string HasKey = "";
                string Phone = "";
                foreach (var item in thirdPartyContactList)
                {
                    if (item.HasKey == "True")
                    {
                        HasKey = "Y";
                    }
                    else
                    {
                        HasKey = "N";
                    }
                    if (!string.IsNullOrEmpty(item.Phone))
                    {
                        Phone = item.Phone.Replace("-", "");
                    }
                    if (!string.IsNullOrEmpty(item.FirstName))
                    {
                        ContactInfo += string.Format(@"<Contact last_name='{0}' first_name='{1}' ctactype_id='MON' relation_id='{3}'
                                                auth_id='FULL' contract_signer_flag='Y' has_key_flag='{4}' phone1='{2}'
                                                phonetype_id1='WK' contltype_no='5000' />", item.LastName, item.FirstName, Phone, item.RelationShip, HasKey);
                    }
                }
            }
            else
            {
                result = false;
                message = "No emergency contact added.";
                return Json(new { result = result, message = message });
            }
            #endregion

            #region Prepare Zones Xml
            var Zonelist = "";
            List<CustomerSecurityZones> securityZonesList = new List<CustomerSecurityZones>();
            securityZonesList = _Util.Facade.CustomerFacade.GetAllCustomerSecurityZoneByCustomerId(BrinksModel.CustomerId, "'Brinks'");
            if (securityZonesList != null && securityZonesList.Count > 0)
            {
                foreach (var item in securityZonesList)
                {
                    Zonelist += string.Format(@"<Zone zone_id='{0}' zonestate_id='A' event_id='{1}'
                                                equiploc_id='{2}' equiptype_id='{3}' zone_comment='{4}' />", item.ZoneNumber, item.EventCode, item.Location, item.EquipmentType, item.ZoneComment);
                }
            }
            else
            {

                result = false;
                message = "No zone added.";
                return Json(new { result = result, message = message });
            }

            #endregion

            #region Prepare Agency Xml
            var Agencylist = "";
            List<CustomerThirdPartyAgency> securityAgencyList = new List<CustomerThirdPartyAgency>();
            securityAgencyList = _Util.Facade.CustomerFacade.GetAllCustomerThirdPartyAgencyByCustomerId(BrinksModel.CustomerId, "'Brinks'");
            string AgencyPhone = "";
            if (securityAgencyList != null && securityAgencyList.Count > 0)
            {

                foreach (var item in securityAgencyList)
                {
                    if (!string.IsNullOrEmpty(item.Phone))
                    {
                        AgencyPhone = item.Phone.Replace("-", "");
                    }
                    Agencylist += string.Format(@"<SiteAgencyPermit agencytype_id='{0}' phone1='{1}' permit_no='' permtype_id='{2}' agency_no='{3}' />", item.Agencytype, AgencyPhone, item.PermType, item.AgencyNo);
                }
            }
            else
            {
                result = false;
                message = "No agency added.";
                return Json(new { result = result, message = message });
            }

            #endregion

            try
            {
                System.Data.DataSet data;
                if (cus != null)
                {
                    if (!string.IsNullOrEmpty(cus.BrinksRefId))
                    {
                        #region Update Site System
                        string rawxml = "";
                        string XML = "";
                        rawxml = @"<Account xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
                                <SiteSystems>
                                    {0}
                                </SiteSystems>

                            </Account>";
                        XML = string.Format(rawxml, SiteInfo);
                        data = wsi.Update(UserId, Password, "C", CsNumber, XML);
                        if (BrinksErrorMsg(data) != "")
                        {
                            message = "Site System: </br>" + BrinksErrorMsg(data);

                        }
                        else
                        {
                            result = true;
                            message = "Site systemp updated successfully.</br>";
                        }
                        #endregion
                        #region Udate Zones
                        Entity = "Zones";
                        string ZonesUpdate = "";
                        string ZonesAdd = "";
                        string ZonesDelete = "";
                        var ZoneData = wsi.GetData(Entity, UserId, Password, CsNumber, null);
                        List<ZoneObject> zoneList = new List<ZoneObject>();
                        if (ZoneData != null)
                        {
                            var dataTable = ZoneData.Tables[0];
                            zoneList = (from DataRow dr in dataTable.Rows
                                        select new ZoneObject()
                                        {
                                            cs_no = dr["cs_no"].ToString(),
                                            zone_id = dr["zone_id"].ToString(),
                                            zonestate_id = dr["zonestate_id"].ToString(),
                                            event_id = dr["event_id"].ToString(),
                                            equiploc_id = dr["equiploc_id"].ToString(),
                                            equiptype_id = dr["equiptype_id"].ToString(),
                                            zone_comment = dr["zone_comment"].ToString()
                                        }).ToList();
                            if (securityZonesList != null && securityZonesList.Count > 0)
                            {
                                foreach (var item in securityZonesList)
                                {
                                    if (zoneList != null && zoneList.Count > 0)
                                    {
                                        if (zoneList.Find(x => x.zone_id.Replace(" ", "") == item.ZoneNumber.ToString()) != null)
                                        {
                                            ZonesUpdate += string.Format(@"<Zone zone_id='{0}' zonestate_id='A' event_id='{1}'
                                                equiploc_id='{2}' equiptype_id='{3}'  zone_comment='{4}'/>", item.ZoneNumber, item.EventCode, item.Location, item.EquipmentType, item.ZoneComment);
                                        }
                                        else
                                        {
                                            ZonesAdd += string.Format(@"<Zone zone_id='{0}' zonestate_id='A' event_id='{1}'
                                                equiploc_id='{2}' equiptype_id='{3}'  zone_comment='{4}'/>", item.ZoneNumber, item.EventCode, item.Location, item.EquipmentType, item.ZoneComment);
                                        }
                                    }

                                }

                            }
                            if (zoneList != null && zoneList.Count > 0)
                            {
                                foreach (var item in zoneList)
                                {
                                    if (securityZonesList.Find(x => x.ZoneNumber.ToString() == item.zone_id.Replace(" ", "")) == null)
                                    {
                                        ZonesDelete += string.Format(@"<Zone zone_id='{0}' zonestate_id='A' event_id='{1}'
                                                equiploc_id='{2}' equiptype_id='{3}' zone_comment='{4}'/>", item.zone_id, item.event_id, item.equiploc_id, item.equiptype_id, item.zone_comment);
                                    }
                                }
                            }
                            rawxml = @"<Account xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
                                 <Zones>
                                    {0}
                                </Zones>

                            </Account>";
                            try
                            {
                                if (ZonesUpdate != "")
                                {
                                    XML = string.Format(rawxml, ZonesUpdate);
                                    data = wsi.Update(UserId, Password, "C", CsNumber, XML);
                                    message += "</br>" + BrinksErrorMsg(data);
                                }
                                if (ZonesAdd != "")
                                {
                                    XML = string.Format(rawxml, ZonesAdd);
                                    data = wsi.Update(UserId, Password, "A", CsNumber, XML);
                                    message += "</br>" + BrinksErrorMsg(data);
                                }
                                if (ZonesDelete != "")
                                {
                                    XML = string.Format(rawxml, ZonesDelete);
                                    data = wsi.Update(UserId, Password, "D", CsNumber, XML);
                                    message += "</br>" + BrinksErrorMsg(data);
                                }
                            }
                            catch (Exception ex)
                            {
                                message += "</br>Zone not updated.";
                                logger.Error(message, ex);
                            }


                        }

                        #endregion
                        #region Update Emergency Contact
                        Entity = "Contacts";
                        string ContactUpdate = "";
                        string ContactAdd = "";
                        string ContactDelete = "";
                        var ContactData = wsi.GetData(Entity, UserId, Password, CsNumber, null);
                        List<BrinksContactObject> contactList = new List<BrinksContactObject>();
                        if (ContactData != null)
                        {
                            var dataTable = ContactData.Tables[0];
                            contactList = (from DataRow dr in dataTable.Rows
                                           select new BrinksContactObject()
                                           {
                                               first_name = dr["first_name"].ToString(),
                                               last_name = dr["last_name"].ToString(),
                                               phone1 = dr["phone1"].ToString(),
                                               contact_no = dr["contact_no"].ToString(),
                                               ctaclink_no = dr["ctaclink_no"].ToString(),

                                           }).ToList();
                            if (thirdPartyContactList != null && thirdPartyContactList.Count > 0)
                            {
                                string HasKey = "";
                                string Phone = "";
                                foreach (var item in thirdPartyContactList)
                                {
                                    if (item.HasKey == "True")
                                    {
                                        HasKey = "Y";
                                    }
                                    else
                                    {
                                        HasKey = "N";
                                    }
                                    if (!string.IsNullOrEmpty(item.Phone))
                                    {
                                        Phone = item.Phone.Replace("-", "");
                                    }
                                    if (contactList != null && contactList.Count > 0)
                                    {
                                        if (contactList.Find(x => x.contact_no == item.ContactNo) != null)
                                        {
                                            ContactUpdate += string.Format(@"<Contact last_name='{0}' first_name='{1}' ctactype_id='MON' relation_id='{4}'
                                                auth_id='FULL' contract_signer_flag='Y' has_key_flag='{5}' phone1='{2}'
                                                phonetype_id1='WK' contltype_no='5000' contact_no='{3}'/>", item.LastName, item.FirstName, Phone, item.ContactNo, item.RelationShip, HasKey);
                                        }
                                        else
                                        {
                                            ContactAdd += string.Format(@"<Contact last_name='{0}' first_name='{1}' ctactype_id='MON' relation_id='{3}'
                                                auth_id='FULL' contract_signer_flag='Y' has_key_flag='{4}' phone1='{2}'
                                                phonetype_id1='WK' contltype_no='5000' />", item.LastName, item.FirstName, Phone, item.RelationShip, HasKey);
                                        }
                                    }

                                }

                            }
                            if (contactList != null && contactList.Count > 0)
                            {
                                foreach (var item in contactList)
                                {
                                    if (thirdPartyContactList.Find(x => x.ContactNo.ToString() == item.contact_no) == null)
                                    {
                                        ContactDelete += string.Format(@"<Contact contact_no='{0}'/>", item.contact_no);
                                    }
                                }
                            }
                            rawxml = @"<Account xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
                                 <Contacts>
                                    {0}
                                </Contacts>
                                </Account>";
                            try
                            {
                                if (ContactDelete != "")
                                {
                                    XML = string.Format(rawxml, ContactDelete);
                                    data = wsi.Update(UserId, Password, "D", CsNumber, XML);
                                    message += "</br>" + BrinksErrorMsg(data);
                                }
                                if (ContactUpdate != "")
                                {
                                    XML = string.Format(rawxml, ContactUpdate);
                                    data = wsi.Update(UserId, Password, "C", CsNumber, XML);
                                    message += "</br>" + BrinksErrorMsg(data);
                                }
                                if (ContactAdd != "")
                                {
                                    XML = string.Format(rawxml, ContactAdd);
                                    data = wsi.Update(UserId, Password, "A", CsNumber, XML);
                                    message += "</br>" + BrinksErrorMsg(data);
                                }
                            }
                            catch (Exception ex)
                            {
                                message += "</br>Contact not updated";
                                logger.Error(message, ex);
                            }

                        }

                        #endregion

                        #region update Into ThirdpartyCustomer table
                        ThirdPartyCustomer tcustomer = new ThirdPartyCustomer();

                        tcustomer = _Util.Facade.CustomerFacade.GetThirdPartyCustomerByCustomerId(BrinksModel.CustomerId);
                        if (tcustomer != null)
                        {
                            tcustomer.Id = tcustomer.Id;
                            tcustomer.City = BrinksModel.city_name;
                            tcustomer.State = BrinksModel.state_id;
                            tcustomer.ZipCode = BrinksModel.zip_code;
                            tcustomer.SiteName = BrinksModel.site_name;
                            tcustomer.ReceiverPhone = BrinksModel.receiver_phone;
                            tcustomer.PanelPhone = BrinksModel.panel_phone;
                            tcustomer.PanelLocation = BrinksModel.panel_location;
                            tcustomer.InstallDate = BrinksModel.install_date;
                            tcustomer.CustomerId = BrinksModel.CustomerId;
                            tcustomer.CrossStreet = BrinksModel.cross_street;
                            tcustomer.CountryName = BrinksModel.county_name;
                            tcustomer.CodeWord = BrinksModel.codeword1;
                            tcustomer.CustomerNumber = Convert.ToInt32(BrinksModel.cs_no);
                            tcustomer.DealerNumber = BrinksModel.servco_no;
                            tcustomer.SiteAddress = BrinksModel.site_addr1;
                            tcustomer.eContact = cus.EcontractId;
                            _Util.Facade.CustomerFacade.UpdateThirdPartyCustomer(tcustomer);
                        }

                        #endregion
                    }
                    else
                    {
                        #region Insert Account Online
                        string rawxml = @"<Account xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
                                <SiteSystems>
                                    {0}
                                </SiteSystems>
                                <Zones>
                                    {1}
                                </Zones>
                                <SiteAgencyPermits>
                                    {3}
                                </SiteAgencyPermits>
                                <Contacts>
                                    {2}
                                </Contacts>
                                <SiteOptions>
                                <SiteOption option_id='CMPUR' option_value='PUR'/>
                                <SiteOption option_id='CONTRLEN' option_value='36'/>
                                </SiteOptions>
                                <SiteSystemOptions>
                                <SiteSystemOption option_id='CELLMAC' option_value='123456789012345'/>
                                <SiteSystemOption option_id='CELLPROV' option_value='ALMCOM'/>
                                <SiteSystemOption option_id='ALMCOMINTSVC' option_value='RBI'/>
                                <SiteSystemOption option_id='ALMCOMWEATHER' option_value=''/>
                                <SiteSystemOption option_id='DSL-VOIP' option_value='VOIP'/>
                                <SiteSystemOption option_id='INST CODE' option_value='4444'/>
                                <SiteSystemOption option_id='PRIVACY' option_value='N'/>
                                <SiteSystemOption option_id='SIGFMT' option_value='CID'/>
                                <SiteSystemOption option_id='TRANSFORMER' option_value='closet'/>
                                </SiteSystemOptions>
                            </Account>";


                        string XML = string.Format(rawxml, SiteInfo, Zonelist, ContactInfo, Agencylist);
                        string FiCO = cus.CreditScore;
                        string RequestedDate = DateTime.Now.ToString("yyyy-MM-dd");
                        string DealerName = CurrentUser.CompanyName;
                        SmartSetupSummary Model = new SmartSetupSummary();
                        Model.CustomerPackageServiceList = _Util.Facade.CustomerFacade.IsLeadAppointmentServiceExistCheckCustomerPackageEqp(cus.CustomerId, CurrentUser.CompanyId.Value);

                        double? ServiceTotal = 0.0;

                        if (Model.CustomerPackageServiceList != null)
                        {
                            ServiceTotal = Model.CustomerPackageServiceList.Sum(x => x.Total);
                        }

                        if (string.IsNullOrEmpty(FiCO))
                        {
                            FiCO = "0";
                        }
                        CustomerCreditCheck creditCheck = new CustomerCreditCheck();
                        creditCheck = _Util.Facade.CustomerFacade.GetAllCustomerCreditCheckByCustomerId(cus.CustomerId).OrderByDescending(x => x.Id).FirstOrDefault();
                        int BureauID = 0;
                        if (creditCheck != null)
                        {
                            if (creditCheck.CreditBureau == "TU")
                            {
                                BureauID = 1;
                            }
                            else if (creditCheck.CreditBureau == "EFX")
                            {
                                BureauID = 5;
                            }
                        }
                        string creditRequestXml = @"<creditRequestXml>
                                                        <CreditRequest xmlns='http://schemas.datacontract.org/2004/07/CreditReporting.DataController' xmlns:i='http://www.w3.org/2001/XMLSchema-instance'>
                                                            <CS>{0}</CS>
                                                            <SSN>{1}</SSN>
                                                            <FirstName>{2}</FirstName>
                                                            <LastName>{3}</LastName>
                                                            <StreetNumber>{4}</StreetNumber>
                                                            <StreetName>{5}</StreetName>
                                                            <City>{6}</City>
                                                            <State>{7}</State>
                                                            <Zip>{8}</Zip>
                                                            <DealerId>{9}</DealerId>
                                                            <UserId>{10}</UserId>
                                                            <FICO>{11}</FICO>
                                                            <RequestDate>{12}</RequestDate>
                                                            <TransactionID>123456</TransactionID>
                                                            <Token>abc123def456</Token>
                                                            <BureauID>{13}</BureauID>
                                                        </CreditRequest>
                                                    </creditRequestXml>";
                        string purchasedInfoXML = @"<purchaseInfoXml>
                                                        <PurchaseInfo xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                                                            <CS>{0}</CS>
                                                            <RMR>{1}</RMR>
                                                            <DealerId>{2}</DealerId>
                                                            <UserId>{3}</UserId>
                                                            <LastUpdated>{4}</LastUpdated>
                                                            <Source>{5}</Source>
                                                        </PurchaseInfo>
                                                    </purchaseInfoXml>";
                        creditRequestXml = string.Format(creditRequestXml, CsNumber, ssn, cus.FirstName, cus.LastName, "0", cus.Street, cus.City, cus.State, cus.ZipCode, DelearId, UserId, FiCO, RequestedDate, BureauID);
                        purchasedInfoXML = string.Format(purchasedInfoXML, CsNumber, ServiceTotal, DelearId, UserId, RequestedDate, DealerName);
                        data = wsi.AccountOnline(UserId, Password, CsNumber, XML, creditRequestXml, purchasedInfoXML);
                        message = BrinksErrorMsg(data);

                        if (message.Contains("Online confirmation"))
                        {
                            cus.BrinksRefId = BrinksModel.cs_no;

                            _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            result = true;
                            #region Update Customer System No
                            CustomerSystemNo cusno = new CustomerSystemNo();
                            cusno = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerNo(cus.BrinksRefId);
                            if (cusno != null)
                            {
                                cusno.IsUsed = true;
                                cusno.CustomerId = cus.Id;
                                cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                                _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(cusno);
                            }
                            #endregion

                            #region Update Cs Agreement
                            if (cus.Ownership == "Brinks")
                            {
                                CustomerExtended extended = new CustomerExtended();

                                extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                                if (extended != null)
                                {
                                    extended.BrinksFundingStatus = "Not-Submitted";
                                    extended.CSAgreement = "PUR";
                                    _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                                }
                                else
                                {
                                    extended.BrinksFundingStatus = "Not-Submitted";
                                    extended.CustomerId = cus.CustomerId;
                                    extended.CSAgreement = "PUR";
                                    _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
                                }
                            }

                            #endregion
                            #region Insert Into ThirdpartyCustomer table
                            ThirdPartyCustomer tcustomer = new ThirdPartyCustomer();
                            tcustomer.AccountOnlineDate = DateTime.Now.UTCCurrentTime();
                            tcustomer.City = BrinksModel.city_name;
                            tcustomer.State = BrinksModel.state_id;
                            tcustomer.ZipCode = BrinksModel.zip_code;
                            tcustomer.SiteName = BrinksModel.site_name;
                            tcustomer.ReceiverPhone = BrinksModel.receiver_phone;
                            tcustomer.PanelPhone = BrinksModel.panel_phone;
                            tcustomer.PanelLocation = BrinksModel.panel_location;
                            tcustomer.InstallDate = BrinksModel.install_date;
                            tcustomer.CustomerId = BrinksModel.CustomerId;
                            tcustomer.CrossStreet = BrinksModel.cross_street;
                            tcustomer.CountryName = BrinksModel.county_name;
                            tcustomer.CodeWord = BrinksModel.codeword1;
                            tcustomer.CustomerNumber = Convert.ToInt32(BrinksModel.cs_no);
                            tcustomer.DealerNumber = BrinksModel.servco_no;
                            tcustomer.SiteAddress = BrinksModel.site_addr1;
                            tcustomer.eContact = cus.EcontractId;
                            tcustomer.CreatedBy = CurrentUser.UserId;
                            tcustomer.Platform = "Brinks";
                            _Util.Facade.CustomerFacade.InsertThirdPartyCustomer(tcustomer);
                            #endregion

                            #region Completed for ticket
                            try
                            {
                                Ticket _tick = _Util.Facade.TicketFacade.GetInstallationTicketByCustomerId(cus.CustomerId);
                                if (_tick != null)
                                {
                                    CalculatePayrollBrinks(_tick);
                                }
                                List<CustomerAppointmentEquipment> TicketItemList = new List<CustomerAppointmentEquipment>();
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
                                if (_tick != null && _tick.Status != LabelHelper.TicketStatus.Completed)
                                {
                                    Customer CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(_tick.CustomerId);

                                    if (CustomerDetails != null)
                                    {
                                        if (_tick.CompletedDate != null)
                                        {
                                            CustomerDetails.InstallDate = _tick.CompletedDate.Value;
                                        }

                                    }

                                    var objticketlist = _Util.Facade.TicketFacade.GetTicketListByCustomerIdAndCompanyIdAndNotCompleted(_tick.CustomerId, CurrentUser.CompanyId.Value);
                                    if (objticketlist != null && objticketlist.Count > 0)
                                    {
                                        foreach (var item in objticketlist)
                                        {
                                            var objappeqplist = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipmentByAppointmentIdAndIsBilling(item.TicketId);
                                            if (objappeqplist != null && objappeqplist.Count > 0)
                                            {
                                                foreach (var app in objappeqplist)
                                                {
                                                    app.IsBilling = false;
                                                    _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(app);
                                                }
                                            }
                                        }
                                    }
                                    var totalbillamount = 0.0;
                                    var objeqpappoint = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentEquipmentListByAppointmentId(_tick.TicketId);
                                    if (objeqpappoint != null && objeqpappoint.Count > 0)
                                    {
                                        foreach (var item in objeqpappoint)
                                        {
                                            if (item.IsService == true && item.IsDefaultService == false)
                                            {
                                                item.IsBilling = true;
                                                _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(item);
                                                totalbillamount += item.TotalPrice;
                                            }
                                        }
                                    }
                                    //var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(_tick.CustomerId);
                                    if (CustomerDetails != null)
                                    {
                                        var totalbillamountTax = 0.0;
                                        GlobalSetting GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, _tick.CustomerId);
                                        if (GetSalesTax != null)
                                        {
                                            totalbillamountTax = Math.Round((totalbillamount * Convert.ToDouble(GetSalesTax.Value)) / 100, 2);
                                        }
                                        CustomerDetails.MonthlyMonitoringFee = totalbillamount.ToString("#.##");
                                        CustomerDetails.BillAmount = defaultBillTaxVal ? totalbillamount + totalbillamountTax : totalbillamount;
                                        CustomerDetails.TotalTax = defaultBillTaxVal ? totalbillamountTax : 0;
                                        CustomerDetails.BillTax = defaultBillTaxVal;
                                        //_Util.Facade.CustomerFacade.UpdateCustomer(CustomerDetails);
                                    }

                                    TicketItemList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipmentByTicketId(CurrentUser.CompanyId.Value, _tick.TicketId);
                                    if (TicketItemList != null && TicketItemList.Count() > 0)
                                    {
                                        foreach (var item in TicketItemList)
                                        {
                                            int totalQty = 0;
                                            int techAddQty = 0;
                                            int techReleaseQty = 0;
                                            bool eqpRelease = false;
                                            CustomerAppointmentEquipment CusAptEqpListDetails = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentEquipmentByAppoinmentIdAndEquipmentIdAndId(_tick.TicketId, item.EquipmentId, item.Id);
                                            TicketUser tikuser = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndPrimary(_tick.TicketId);
                                            List<InventoryTech> objinvtech = _Util.Facade.InventoryFacade.GetAllInventoryTechByTechnicianIdAndEquipmentIdAndType(tikuser.UserId, item.EquipmentId);
                                            if (objinvtech != null && objinvtech.Count > 0)
                                            {
                                                foreach (var invtech in objinvtech)
                                                {
                                                    if (invtech.Type == "Add")
                                                    {
                                                        techAddQty += invtech.Quantity;
                                                    }
                                                    if (invtech.Type == "Release")
                                                    {
                                                        techReleaseQty += invtech.Quantity;
                                                    }
                                                }
                                                totalQty = techAddQty - techReleaseQty;
                                            }

                                            if (totalQty > 0 && totalQty >= item.Quantity)
                                            {
                                                item.QuantityLeftEquipment = item.Quantity;
                                            }
                                            else if (totalQty > 0 && totalQty < item.Quantity)
                                            {
                                                item.QuantityLeftEquipment = totalQty;
                                            }
                                            if (CusAptEqpListDetails != null && CusAptEqpListDetails.IsEquipmentRelease == false && totalQty > 0)
                                            {
                                                CusAptEqpListDetails.IsEquipmentRelease = true;
                                                CustomerDetails.Installer = CusAptEqpListDetails.TechnicianId.ToString();
                                                CusAptEqpListDetails.QuantityLeftEquipment = item.QuantityLeftEquipment;
                                                _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(CusAptEqpListDetails);
                                                var delres = _Util.Facade.InventoryFacade.DeleteInventoryTechByCustomerAppointmentEquipmentIdAndType(item.Id);
                                                InventoryTech invtech = new InventoryTech()
                                                {
                                                    CompanyId = CurrentUser.CompanyId.Value,
                                                    TechnicianId = item.TechnicianId,
                                                    EquipmentId = item.EquipmentId,
                                                    Type = LabelHelper.InventoryType.Release,
                                                    Quantity = item.QuantityLeftEquipment.HasValue ? item.QuantityLeftEquipment.Value : 0,
                                                    LastUpdatedBy = CurrentUser.UserId,
                                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                                    Description = "Release from technician by ticket",
                                                    CustomerAppointmentEquipmentId = item.Id
                                                };
                                                _Util.Facade.InventoryFacade.InsertInventoryTech(invtech);

                                            }

                                        }
                                    }
                                    _tick.LastUpdatedBy = CurrentUser.UserId;
                                    _tick.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                    _tick.CompletedDate = DateTime.Now.UTCCurrentTime();
                                    _tick.Status = LabelHelper.TicketStatus.Completed;
                                    _Util.Facade.TicketFacade.UpdateTicket(_tick);
                                    _Util.Facade.CustomerFacade.UpdateCustomer(CustomerDetails);
                                }

                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex);
                            }

                            #endregion
                        }
                        #endregion
                    }
                }


            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }
            #region Check Funding Result
            if (!string.IsNullOrEmpty(cus.BrinksRefId) && !string.IsNullOrEmpty(cus.EcontractId))
            {
                CustomerExtended extended = new CustomerExtended();
                bool FundingResult = GetCommonFundingResult(cus.CustomerId);
                extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                if (extended != null)
                {
                    extended.FundingResult = FundingResult;
                    _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                }
                else
                {
                    extended.CustomerId = cus.CustomerId;
                    extended.FundingResult = FundingResult;
                    _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
                }
            }

            #endregion

            return Json(new { result = result, message = message });
        }

        public ActionResult SendEcontractWithSurvey(Guid CustomerId, string from)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.CustomerId = CustomerId;
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
            if (cus != null)
            {
                ViewBag.CusType = cus.Type;
            }
            ViewBag.from = from;
            ViewBag.CompanyName = CurrentUser.CompanyName;
            return View();
        }

        public ActionResult SendISPCEcontract(Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.CustomerId = CustomerId;
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
            CustomerIsPcCreditApplication creditApp = new CustomerIsPcCreditApplication();
            creditApp = _Util.Facade.CustomerFacade.GetCustomerIsPcAppByCustomerId(CustomerId);
            if (cus != null)
            {
                ViewBag.CusType = cus.Type;
            }
            ViewBag.BillingMethod = _Util.Facade.LookupFacade.GetLookupByKey("IspcBillingMethodOptions").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.DayToBill = _Util.Facade.LookupFacade.GetLookupByKey("IspcDayToBillOptions").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.SigningApplicant = _Util.Facade.LookupFacade.GetLookupByKey("IspcSigningApplicantOption").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            ViewBag.CompanyName = CurrentUser.CompanyName;
            return View(creditApp);
        }
        public JsonResult SendEcontract(Guid CustomerId, string PaymentDate, string InstallStartDate, string InstallFinishDate, SurveyQuestion Questions, PersonalGuarantee personalGuarantee, string from, int PromotionMonth, int PrepaidMonth, double? ActivitionFee, int? ContactId, string ContractType)
        {
            bool result = false;
            string Message = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            string userName = "econtractapi";
            string password = "qh0`%Q$:51";

            string Login = "";
            string Password = "";
            var endPointAddress = new EndpointAddress("https://senti.monitronics.net/eContractAPIUAT");
            #region Credentials

            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
            if (UserIdGlobal != null)
            {
                Login = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                endPointAddress = new EndpointAddress("https://mimasweb.monitronics.net/eContractAPI");
            }
            else
            {
                endPointAddress = new EndpointAddress("https://senti.monitronics.net/eContractAPIUAT");
            }


            #endregion
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var basicHttpBinding = new BasicHttpBinding(
                              BasicHttpSecurityMode.TransportWithMessageCredential);
            basicHttpBinding.Security.Message.ClientCredentialType =
                                                 BasicHttpMessageCredentialType.UserName;
            try
            {
                using (HS.Econtract.eContractApi.VirtualInterfaceClient client = new HS.Econtract.eContractApi.VirtualInterfaceClient(basicHttpBinding, endPointAddress))
                {

                    client.ClientCredentials.UserName.UserName = userName;
                    client.ClientCredentials.UserName.Password = password;
                    AuthenticationResult2 authresult = client.AuthenticateUser2(Login, Password);
                    if (authresult != null)
                    {

                        Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
                        #region Emergency Contact Ready
                        List<EmergencyContact> emgContact = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerId(CustomerId);
                        List<ContactItem> contactItemList = new List<ContactItem>();
                        ContactItem contact = new ContactItem();
                        int order = 1;

                        if (emgContact != null && emgContact.Count > 0)
                        {
                            foreach (var item in emgContact)
                            {
                                contact = new ContactItem();
                                contact.Name = item.FirstName + " " + item.LastName;
                                contact.Phone = item.Phone;
                                contact.UserNumber = order.ToString();
                                if (cus.PhoneType == "HO")
                                {
                                    contact.PhoneType = PhoneTypeEnum.Home;
                                }
                                else if (cus.PhoneType == "MB")
                                {
                                    contact.PhoneType = PhoneTypeEnum.Cell;
                                }
                                else if (cus.PhoneType == "BS")
                                {
                                    contact.PhoneType = PhoneTypeEnum.Work;
                                }
                                contactItemList.Add(contact);

                            }
                        }

                        #endregion
                        List<EquipmentItem> eqpItemList = new List<EquipmentItem>();
                        EquipmentItem eqpItem = new EquipmentItem();
                        double? ServiceTotal = 0.0;
                        double? EquipmentSubTotal = 0.0;
                        double ServiceTax = 0.0;
                        double PackageEquipTaxAmount = 0.0;
                        double PackageEquipTax = 0.0;
                        double? MonthlyPayment = 0.0;
                        double? OnedayPayment = 0.0;

                        PaymentItem payMonthly = new PaymentItem();
                        PaymentItem payInitial = new PaymentItem();
                        CustomerAdditionalContact secondaryCredit = new CustomerAdditionalContact();
                        if (from == "lead")
                        {
                            #region customer package
                            SmartSetupSummary Model = new SmartSetupSummary();
                            Model.Customer = cus;
                            Model.PackageCustomer = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerId(CustomerId);
                            Model.CustomerPackageServiceList = _Util.Facade.CustomerFacade.IsLeadAppointmentServiceExistCheckCustomerPackageEqp(CustomerId, CurrentUser.CompanyId.Value);
                            Model.CustomerPackageEqpList = _Util.Facade.CustomerFacade.IsLeadAppointmentEquipmentExistCheckCustomerPackageEqp(CustomerId, CurrentUser.CompanyId.Value);


                            #endregion
                            #region Tax

                            var TaxActivationNoncorming = 0.0;

                            var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, CustomerId);

                            ViewBag.SaleTax = 0.00;
                            ViewBag.Tax = 0.00;
                            if (GetSalesTax != null)
                            {
                                ViewBag.SaleTax = Math.Round(Convert.ToDouble(GetSalesTax.Value), 2);

                                double ServiceTaxAmount = (Convert.ToDouble(Model.CustomerPackageServiceList.Sum(x => x.Total)) * Convert.ToDouble(GetSalesTax.Value)) / 100;
                                ServiceTax = Math.Round(ServiceTaxAmount, 2);

                                PackageEquipTaxAmount = (Convert.ToDouble(Model.CustomerPackageEqpList.Sum(x => x.Total)) * Convert.ToDouble(GetSalesTax.Value)) / 100;
                                PackageEquipTax = Math.Round(PackageEquipTaxAmount, 2);


                                var SumActivationNon = Model.PackageCustomer.ActivationFee + Model.PackageCustomer.NonConformingFee;
                                TaxActivationNoncorming = (Convert.ToDouble(SumActivationNon) * Convert.ToDouble(GetSalesTax.Value)) / 100;

                            }
                            #endregion

                            #region Equipment List Ready


                            if (Model.CustomerPackageEqpList != null)
                            {
                                foreach (var item in Model.CustomerPackageEqpList)
                                {
                                    eqpItem = new EquipmentItem();
                                    eqpItem.Name = item.EquipmentServiceName;

                                    //eqpItem.Points = item.Point;

                                    if (item.Total != null)
                                    {
                                        eqpItem.Total = item.Total.Value;
                                    }
                                    if (item.Quantity != null)
                                    {
                                        eqpItem.Quantity = item.Quantity.Value;
                                    }

                                    eqpItem.Price = eqpItem.Price;

                                    eqpItemList.Add(eqpItem);

                                }
                            }

                            #endregion
                            #region Service Equipment List Ready

                            //if (Model.CustomerPackageServiceList != null)
                            //{
                            //    foreach (var item in Model.CustomerPackageServiceList)
                            //    {
                            //        eqpItem = new EquipmentItem();
                            //        eqpItem.Name = item.EquipmentServiceName;

                            //        if (item.Total != null)
                            //        {
                            //            eqpItem.Total = item.Total.Value;
                            //        }
                            //        eqpItem.Price = eqpItem.Price;

                            //        eqpItemList.Add(eqpItem);

                            //    }
                            //}

                            #endregion
                            #region Payment Info Customer Related
                            List<PaymentInfoCustomer> PICList = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentInfoCustomerByCustomerId(Model.Customer.CustomerId);

                            if (PICList != null && PICList.Count() > 0)
                            {
                                PaymentInfoCustomer PIC = PICList.Where(x => x.Payfor == "Equipment").FirstOrDefault();

                                PIC = PICList.Where(x => x.Payfor == "MMR").FirstOrDefault();
                                if (PIC != null)
                                {
                                    PaymentInfo payInfoMonthly = _Util.Facade.PaymentInfoFacade.GetPaymentInfoById(PIC.PaymentInfoId);


                                    if (payMonthly != null)
                                    {
                                        payMonthly.BankAccountNumber = payInfoMonthly.AcountNo;
                                        payMonthly.BankRoutingNumber = payInfoMonthly.RoutingNo;
                                        int expireMonth = 0;
                                        int expireYear = 0;

                                        if (!string.IsNullOrEmpty(payInfoMonthly.CardExpireDate))
                                        {
                                            expireMonth = payInfoMonthly.CardExpireDate.Split("/")[0].ToInt();
                                            var expireYearval = "";
                                            if (payInfoMonthly.CardExpireDate.Split("/")[1].Length < 3)
                                            {
                                                expireYearval = "20" + payInfoMonthly.CardExpireDate.Split("/")[1];
                                                expireYear = expireYearval.ToInt();
                                            }
                                            else
                                            {
                                                expireYear = payInfoMonthly.CardExpireDate.Split("/")[1].ToInt();
                                            }
                                        }
                                        payMonthly.CreditCardExpireMonth = expireMonth;
                                        payMonthly.CreditCardExpireYear = expireYear;
                                        payMonthly.CreditCardNumber = payInfoMonthly.CardNumber;
                                        if (payInfoMonthly.CardType == LabelHelper.CardType.Visa)
                                        {
                                            payMonthly.CreditCardType = CreditCardTypeEnum.Visa;
                                            payMonthly.PaymentType = PaymentTypeEnum.CreditCard;
                                        }
                                        else if (payInfoMonthly.CardType == LabelHelper.CardType.MasterCard)
                                        {
                                            payMonthly.CreditCardType = CreditCardTypeEnum.MasterCard;
                                            payMonthly.PaymentType = PaymentTypeEnum.CreditCard;
                                        }
                                        else if (payInfoMonthly.CardType == LabelHelper.CardType.AmericanExpress)
                                        {
                                            payMonthly.CreditCardType = CreditCardTypeEnum.AmericanExpress;
                                            payMonthly.PaymentType = PaymentTypeEnum.CreditCard;
                                        }
                                        else if (payInfoMonthly.CardType == LabelHelper.CardType.Discover)
                                        {
                                            payMonthly.CreditCardType = CreditCardTypeEnum.Discover;
                                            payMonthly.PaymentType = PaymentTypeEnum.CreditCard;
                                        }


                                    }
                                }
                                PIC.ForMonths = PrepaidMonth;
                                _Util.Facade.PaymentInfoCustomerFacade.UpdatePaymentInfoCustomer(PIC);
                                PIC = PICList.Where(x => x.Payfor == "Activation Fee").FirstOrDefault();
                                if (PIC != null)
                                {
                                    PaymentInfo payInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoById(PIC.PaymentInfoId);


                                    if (payInfo != null)
                                    {
                                        payInitial.BankAccountNumber = payInfo.AcountNo;
                                        payInitial.BankRoutingNumber = payInfo.RoutingNo;
                                        if (payInitial.BankAccountNumber != "")
                                        {
                                            payInitial.PaymentType = PaymentTypeEnum.BankAccount;
                                        }
                                        int expireMonth = 0;
                                        int expireYear = 0;
                                        if (!string.IsNullOrEmpty(payInfo.CardExpireDate))
                                        {
                                            expireMonth = payInfo.CardExpireDate.Split("/")[0].ToInt();
                                            var expireYearval = "";
                                            if (payInfo.CardExpireDate.Split("/")[1].Length < 3)
                                            {
                                                expireYearval = "20" + payInfo.CardExpireDate.Split("/")[1];
                                                expireYear = expireYearval.ToInt();
                                            }
                                            else
                                            {
                                                expireYear = payInfo.CardExpireDate.Split("/")[1].ToInt();
                                            }

                                        }
                                        payInitial.CreditCardExpireMonth = expireMonth;
                                        payInitial.CreditCardExpireYear = expireYear;
                                        payInitial.CreditCardNumber = payInfo.CardNumber;
                                        if (payInfo.CardType == LabelHelper.CardType.Visa)
                                        {
                                            payInitial.CreditCardType = CreditCardTypeEnum.Visa;
                                            payInitial.PaymentType = PaymentTypeEnum.CreditCard;
                                        }
                                        else if (payInfo.CardType == LabelHelper.CardType.MasterCard)
                                        {
                                            payInitial.CreditCardType = CreditCardTypeEnum.MasterCard;
                                            payInitial.PaymentType = PaymentTypeEnum.CreditCard;
                                        }
                                        else if (payInfo.CardType == LabelHelper.CardType.AmericanExpress)
                                        {
                                            payInitial.CreditCardType = CreditCardTypeEnum.AmericanExpress;
                                            payInitial.PaymentType = PaymentTypeEnum.CreditCard;
                                        }
                                        else if (payInfo.CardType == LabelHelper.CardType.Discover)
                                        {
                                            payInitial.CreditCardType = CreditCardTypeEnum.Discover;
                                            payInitial.PaymentType = PaymentTypeEnum.CreditCard;
                                        }
                                        if (payInfo.PaymentMethod == LabelHelper.PaymentMethod.Invoice)
                                        {
                                            payInitial.PaymentType = PaymentTypeEnum.Invoice;
                                        }

                                    }
                                }
                                PaymentInfoCustomer PICService = PICList.Where(x => x.Payfor == "Service").FirstOrDefault();
                                PICService.ForMonths = PrepaidMonth;
                                _Util.Facade.PaymentInfoCustomerFacade.UpdatePaymentInfoCustomer(PIC);
                            }


                            #endregion


                            if (Model.CustomerPackageServiceList != null)
                            {
                                ServiceTotal = Model.CustomerPackageServiceList.Sum(x => x.Total);
                            }
                            if (Model.CustomerPackageEqpList != null)
                            {
                                EquipmentSubTotal = Model.CustomerPackageEqpList.Sum(x => x.Total);
                            }

                            MonthlyPayment = ServiceTotal;
                            OnedayPayment = Model.PackageCustomer.ActivationFee + Model.PackageCustomer.NonConformingFee + TaxActivationNoncorming;
                        }
                        else
                        {
                            #region Payment Info Ready
                            List<PaymentInfo> model = new List<PaymentInfo>();
                            PaymentInfo payInfoInitial = new PaymentInfo();
                            PaymentInfo payInfoPartial = new PaymentInfo();
                            var objpayprofile = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentProfileCustomerByCustomerId(cus.CustomerId);
                            if (objpayprofile != null && objpayprofile.Count > 0)
                            {
                                foreach (var item in objpayprofile)
                                {

                                    model.Add(_Util.Facade.PaymentInfoFacade.GetPaymentInfoById(item.PaymentInfoId));
                                }
                            }
                            if (model != null && model.Count > 0)
                            {
                                payInfoInitial = model.Where(x => x.IsForBrinks == true && x.IsInitialPayment == true).FirstOrDefault();
                                payInfoPartial = model.Where(x => x.IsForBrinks == true && x.IsPartialPayment == true).FirstOrDefault();
                            }
                            if (payInfoInitial != null)
                            {
                                payInitial.BankAccountNumber = payInfoInitial.AcountNo;
                                payInitial.BankRoutingNumber = payInfoInitial.RoutingNo;
                                if (payInitial.BankAccountNumber != "")
                                {
                                    payInitial.PaymentType = PaymentTypeEnum.BankAccount;
                                }
                                int expireMonth = 0;
                                int expireYear = 0;
                                if (!string.IsNullOrEmpty(payInfoInitial.CardExpireDate))
                                {
                                    expireMonth = payInfoInitial.CardExpireDate.Split("/")[0].ToInt();
                                    var expireYearval = "";
                                    if (payInfoInitial.CardExpireDate.Split("/")[1].Length < 3)
                                    {
                                        expireYearval = "20" + payInfoInitial.CardExpireDate.Split("/")[1];
                                        expireYear = expireYearval.ToInt();
                                    }
                                    else
                                    {
                                        expireYear = payInfoInitial.CardExpireDate.Split("/")[1].ToInt();
                                    }

                                }
                                payInitial.CreditCardExpireMonth = expireMonth;
                                payInitial.CreditCardExpireYear = expireYear;
                                payInitial.CreditCardNumber = payInfoInitial.CardNumber;
                                if (payInfoInitial.CardType == LabelHelper.CardType.Visa)
                                {
                                    payInitial.CreditCardType = CreditCardTypeEnum.Visa;
                                    payInitial.PaymentType = PaymentTypeEnum.CreditCard;
                                }
                                else if (payInfoInitial.CardType == LabelHelper.CardType.MasterCard)
                                {
                                    payInitial.CreditCardType = CreditCardTypeEnum.MasterCard;
                                    payInitial.PaymentType = PaymentTypeEnum.CreditCard;
                                }
                                else if (payInfoInitial.CardType == LabelHelper.CardType.AmericanExpress)
                                {
                                    payInitial.CreditCardType = CreditCardTypeEnum.AmericanExpress;
                                    payInitial.PaymentType = PaymentTypeEnum.CreditCard;
                                }
                                else if (payInfoInitial.CardType == LabelHelper.CardType.Discover)
                                {
                                    payInitial.CreditCardType = CreditCardTypeEnum.Discover;
                                    payInitial.PaymentType = PaymentTypeEnum.CreditCard;
                                }
                                if (payInfoInitial.PaymentMethod == LabelHelper.PaymentMethod.Invoice)
                                {
                                    payInitial.PaymentType = PaymentTypeEnum.Invoice;
                                }

                            }

                            if (payInfoPartial != null)
                            {
                                payMonthly.BankAccountNumber = payInfoPartial.AcountNo;
                                payMonthly.BankRoutingNumber = payInfoPartial.RoutingNo;
                                if (payMonthly.BankAccountNumber != "")
                                {
                                    payMonthly.PaymentType = PaymentTypeEnum.BankAccount;
                                }
                                int expireMonth = 0;
                                int expireYear = 0;
                                if (!string.IsNullOrEmpty(payInfoPartial.CardExpireDate))
                                {
                                    expireMonth = payInfoPartial.CardExpireDate.Split("/")[0].ToInt();
                                    var expireYearval = "";
                                    if (payInfoPartial.CardExpireDate.Split("/")[1].Length < 3)
                                    {
                                        expireYearval = "20" + payInfoPartial.CardExpireDate.Split("/")[1];
                                        expireYear = expireYearval.ToInt();
                                    }
                                    else
                                    {
                                        expireYear = payInfoPartial.CardExpireDate.Split("/")[1].ToInt();
                                    }

                                }
                                payMonthly.CreditCardExpireMonth = expireMonth;
                                payMonthly.CreditCardExpireYear = expireYear;
                                payMonthly.CreditCardNumber = payInfoPartial.CardNumber;
                                if (payInfoPartial.CardType == LabelHelper.CardType.Visa)
                                {
                                    payMonthly.CreditCardType = CreditCardTypeEnum.Visa;
                                    payMonthly.PaymentType = PaymentTypeEnum.CreditCard;
                                }
                                else if (payInfoPartial.CardType == LabelHelper.CardType.MasterCard)
                                {
                                    payMonthly.CreditCardType = CreditCardTypeEnum.MasterCard;
                                    payMonthly.PaymentType = PaymentTypeEnum.CreditCard;
                                }
                                else if (payInfoPartial.CardType == LabelHelper.CardType.AmericanExpress)
                                {
                                    payMonthly.CreditCardType = CreditCardTypeEnum.AmericanExpress;
                                    payMonthly.PaymentType = PaymentTypeEnum.CreditCard;
                                }
                                else if (payInfoPartial.CardType == LabelHelper.CardType.Discover)
                                {
                                    payMonthly.CreditCardType = CreditCardTypeEnum.Discover;
                                    payMonthly.PaymentType = PaymentTypeEnum.CreditCard;
                                }
                                if (payInfoPartial.PaymentMethod == LabelHelper.PaymentMethod.Invoice)
                                {
                                    payMonthly.PaymentType = PaymentTypeEnum.Invoice;
                                }

                            }
                            #endregion
                            #region Equipment and Service Ready
                            List<CustomerAppointmentEquipment> appEqpList = new List<CustomerAppointmentEquipment>();
                            List<CustomerAppointmentEquipment> appServiceList = new List<CustomerAppointmentEquipment>();
                            appEqpList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByCustomerId(CustomerId).Where(x => x.IsService == false).ToList();
                            appServiceList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByCustomerId(CustomerId).Where(x => x.IsService == true).ToList();
                            if (appEqpList != null && appEqpList.Count > 0)
                            {
                                Equipment eqp = new Equipment();
                                foreach (var item in appEqpList)
                                {

                                    // eqp = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(item.EquipmentId);
                                    Ticket tic = _Util.Facade.TicketFacade.GetTicketByTicketId(item.AppointmentId);
                                    if (tic != null && tic.TicketType == "Installation")
                                    {
                                        eqpItem = new EquipmentItem();
                                        eqpItem.Name = item.EquipName;

                                        //eqpItem.Points = item.Point;
                                        eqpItem.Total = item.TotalPrice;
                                        eqpItem.Quantity = item.Quantity;
                                        eqpItem.Price = item.UnitPrice;

                                        eqpItemList.Add(eqpItem);
                                    }


                                }



                            }
                            foreach (var item in appServiceList)
                            {
                                Ticket tic = _Util.Facade.TicketFacade.GetTicketByTicketId(item.AppointmentId);
                                if (tic != null && tic.TicketType == "Installation")
                                {
                                    ServiceTotal += item.TotalPrice;

                                }

                            }
                            MonthlyPayment = ServiceTotal;
                            if (appEqpList != null)
                            {
                                EquipmentSubTotal = appEqpList.Sum(x => x.TotalPrice);
                            }
                            if (ActivitionFee.HasValue)
                            {
                                OnedayPayment = Math.Round(ActivitionFee.Value, 2);
                            }
                            #endregion
                            #region Tax

                            var TaxActivationNoncorming = 0.0;

                            var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, CustomerId);

                            ViewBag.SaleTax = 0.00;
                            ViewBag.Tax = 0.00;
                            if (GetSalesTax != null)
                            {
                                ViewBag.SaleTax = Math.Round(Convert.ToDouble(GetSalesTax.Value), 2);

                                double ServiceTaxAmount = (Convert.ToDouble(appServiceList.Sum(x => x.TotalPrice)) * Convert.ToDouble(GetSalesTax.Value)) / 100;
                                ServiceTax = Math.Round(ServiceTaxAmount, 2);

                                PackageEquipTaxAmount = (Convert.ToDouble(appEqpList.Sum(x => x.TotalPrice)) * Convert.ToDouble(GetSalesTax.Value)) / 100;
                                PackageEquipTax = Math.Round(PackageEquipTaxAmount, 2);

                            }
                            #endregion
                        }


                        #region Attachment in Base64 Formate
                        List<CustomerFile> cusFileList = _Util.Facade.CustomerFileFacade.GetAllTagFilesByCustomerIdAndCompanyId(CustomerId, CurrentUser.CompanyId.Value);
                        List<string> attachmentList = new List<string>();
                        //if(cusFileList != null && cusFileList.Count > 0)
                        //{
                        //    foreach(var item in cusFileList)
                        //    {
                        //        string path = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/") + item.Filename;
                        //        byte[] imageArray = System.IO.File.ReadAllBytes(path);
                        //        string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                        //        string attachmentGuid = client.UploadAttachment(imageArray, item.Tag);
                        //        attachmentList.Add(attachmentGuid);
                        //    }

                        //}


                        #endregion

                        DateTime PaymentEffectiveDate = new DateTime();
                        DateTime.TryParse(PaymentDate, out PaymentEffectiveDate);

                        DateTime InstallationStartDate = new DateTime();
                        DateTime.TryParse(InstallStartDate, out InstallationStartDate);

                        DateTime InstallationFinishDate = new DateTime();
                        DateTime.TryParse(InstallFinishDate, out InstallationFinishDate);

                        #region Gether ContractDocument Info


                        ContractDocument2 document = new ContractDocument2();
                        if (cus != null)
                        {
                            document.BillStartDate = PaymentEffectiveDate;
                            document.BillingAddress1 = cus.Address;
                            document.BillingCity = cus.City;
                            document.BillingCounty = cus.County != "" ? cus.County : "N/A";
                            if (!string.IsNullOrEmpty(cus.State))
                            {
                                StateProvinceEnum state = (StateProvinceEnum)Enum.Parse(typeof(StateProvinceEnum), cus.State);
                                document.BillingState = state;
                                document.PremiseState = state;
                            }

                            document.BillingZip = cus.ZipCode;
                            document.CompanyName = cus.BusinessName;
                            document.CompanyType = CompanyTypes.LLC;

                            document.ContactList = contactItemList.Select(x => x as ContactItem).ToArray();
                            document.CountryOfSale = HS.Econtract.eContractApi.CountryEnum.US;
                            if (cus.Type == "Residential")
                            {
                                document.CustomerType = HS.Econtract.eContractApi. CustomerTypeEnum.Residential;
                                document.PaymentExtendedServiceOption = Questions.IsExtendedService;
                            }
                            else if (cus.Type == "Commercial")
                            {
                                document.CustomerType = HS.Econtract.eContractApi. CustomerTypeEnum.Commercial;
                                //if (attachmentList != null && attachmentList.Count > 0)
                                //{
                                //    document.AttachmentGUIDListing = attachmentList.Select(x => x as string).ToArray();

                                //}

                                document.PersonalGuaranteeRequired = personalGuarantee.IsPersonalGuarantee;
                                if (personalGuarantee.IsPersonalGuarantee == true)
                                {
                                    document.PGTitle = personalGuarantee.Title;
                                    document.PGHomeAdddress1 = personalGuarantee.Address1;
                                    document.PGHomeAdddress2 = personalGuarantee.Address2;
                                    document.PGHomeCity = personalGuarantee.City;
                                    if (!string.IsNullOrEmpty(personalGuarantee.State))
                                    {
                                        StateProvinceEnum PGstate = (StateProvinceEnum)Enum.Parse(typeof(StateProvinceEnum), personalGuarantee.State);
                                        document.PGHomeState = PGstate;
                                    }

                                    document.PGHomeZip = personalGuarantee.ZipCode;
                                }
                            }
                            document.DealerPassword = Password;
                            document.DealerUsername = Login;
                            if (cus.RenewalTerm != null)
                            {
                                if (cus.RenewalTerm > 28)
                                {
                                    document.DraftDay = 28;
                                }
                                else
                                {
                                    document.DraftDay = cus.RenewalTerm.Value;
                                }

                            }
                            document.EquipmentAlarmNetwork = AlarmNetworkEnum.AlarmDotcom;
                            document.EquipmentAlarmNetworkIncluded = true;
                            document.EquipmentList = eqpItemList.Select(x => x as EquipmentItem).ToArray();
                            document.EquipmentSubtotalAmount = EquipmentSubTotal != null ? Math.Round(EquipmentSubTotal.Value, 2) : 0.0;
                            document.EquipmentTaxAmount = Math.Round(PackageEquipTaxAmount, 2);
                            document.EquipmentTotalAmount = Math.Round((document.EquipmentSubtotalAmount + document.EquipmentTaxAmount), 2);
                            document.InstallationFinish = InstallationStartDate;
                            document.InstallationStart = InstallationFinishDate;
                            document.InstallationWorkDescription = "Nothing";
                            document.InsurancePersonalInjuryAmount = 0;
                            document.InsurancePropertyDamageAmount = 0;
                            document.Language = ContractLanguageEnum.English;

                            document.PaymentCount = 36;
                            document.PaymentEffectiveDate = PaymentEffectiveDate;

                            document.PaymentInitial = payInitial;
                            document.PaymentMonthly = payMonthly;


                            document.PaymentMonthlyMonitoringRate = MonthlyPayment != null ? Math.Round(MonthlyPayment.Value, 2) : 0.0;
                            document.PaymentOneTimeActivationFee = OnedayPayment != null ? Math.Round(OnedayPayment.Value) : 0.0;
                            document.PremiseAddress1 = cus.Address;
                            document.PremiseCity = cus.City;
                            document.PremiseCounty = cus.County != "" ? cus.County : "N/A";
                            document.MonthsPaidUpFront = PrepaidMonth;
                            document.PromotionPeriod = PromotionMonth;
                            document.PremiseZip = cus.ZipCode;

                            document.PrimaryFirstName = cus.FirstName;
                            document.PrimaryLastName = cus.LastName;
                            document.PrimaryPassword = cus.Passcode;
                            document.PrimaryPhone = cus.PrimaryPhone != "" ? cus.PrimaryPhone : cus.CellNo;
                            document.PrimaryTaxIDNumber = cus.SSN;

                            document.PrimaryBirthDate = cus.DateofBirth != null ? cus.DateofBirth.Value.ToString("MM/dd/yyyy") : "";
                            document.PrimaryEmail = cus.EmailAddress;

                            #region Second Signer Value

                            if (ContractType == "S" || ContractType == "B")
                            {
                                if (ContactId.HasValue)
                                {
                                    secondaryCredit = _Util.Facade.AdditionalContactFacade.GetById(ContactId.Value);
                                    if (secondaryCredit != null)
                                    {
                                        document.SecondaryFirstName = secondaryCredit.FirstName;
                                        document.SecondaryLastName = secondaryCredit.LastName;
                                        document.SecondaryEmail = secondaryCredit.Email;
                                        document.SecondaryPhone = secondaryCredit.Phone;
                                        if (secondaryCredit.DOB.HasValue)
                                        {
                                            document.SecondaryBirthDate = secondaryCredit.DOB != null ? secondaryCredit.DOB.Value.ToString("MM/dd/yyyy") : "";
                                        }

                                        if (!string.IsNullOrEmpty(secondaryCredit.SSN))
                                        {
                                            document.SecondaryTaxIDNumber = secondaryCredit.SSN.Trim();
                                        }

                                    }
                                }

                            }

                            #endregion


                            document.PaymentEffectiveDate = PaymentEffectiveDate;
                            if (cus.DateofBirth != null)
                            {
                                document.PrimaryBirthDate = cus.DateofBirth.Value.ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                document.PrimaryBirthDate = new DateTime().ToString("MM/dd/yyyy");
                            }


                            document.SurveyCancellingService = Questions.SurveyCancellingService;
                            document.SurveyConfirmContractLength = Questions.SurveyConfirmContractLength;
                            document.SurveyFamiliarizationPeriod = Questions.SurveyFamiliarizationPeriod;
                            document.SurveyHomeowner = Questions.SurveyHomeowner;
                            document.SurveyNewConstruction = Questions.SurveyNewConstruction;
                            document.SurveyUnderContract = Questions.SurveyUnderContract;

                        }
                        #endregion
                        //CreateContractResult contractResult1 = client.CreateContract2(document, SigningType.Embedded, SigningType.Embedded);
                        CreateContractResult contractResult = new CreateContractResult();
                        if (ContractType == "P")
                        {
                            contractResult = client.CreateContract2(document, SigningType.Remote, SigningType.None);
                        }
                        else if (ContractType == "S")
                        {
                            contractResult = client.CreateContract2(document, SigningType.None, SigningType.Remote);
                        }
                        else
                        {
                            contractResult = client.CreateContract2(document, SigningType.Remote, SigningType.Remote);
                        }

                        if (contractResult.EnvelopeID != "")
                        {
                            result = true;
                            Message = "Econtract send successfully.";
                            cus.EcontractEnvlobeId = contractResult.EnvelopeID;
                            cus.EcontractId = contractResult.ResultData;
                            _Util.Facade.CustomerFacade.UpdateCustomer(cus);

                            CustomerExtended cusExted = new CustomerExtended();
                            cusExted = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                            if (cusExted == null)
                            {
                                cusExted = new CustomerExtended();
                                cusExted.CustomerId = cus.CustomerId;
                                cusExted.ContractSentBy = CurrentUser.UserId;
                                _Util.Facade.CustomerFacade.InsertCustomerExtended(cusExted);
                            }
                            else
                            {
                                cusExted.ContractSentBy = CurrentUser.UserId;
                                _Util.Facade.CustomerFacade.UpdateCustomerExtended(cusExted);
                            }
                            ThirdPartyCustomer tcustomer = new ThirdPartyCustomer();
                            tcustomer = _Util.Facade.CustomerFacade.GetThirdPartyCustomerByCustomerId(cus.CustomerId);
                            if (tcustomer != null)
                            {
                                tcustomer.eContact = cus.EcontractId;
                                _Util.Facade.CustomerFacade.UpdateThirdPartyCustomer(tcustomer);
                            }

                            #region Update SecondaryContact
                            List<CustomerAdditionalContact> contactList = new List<CustomerAdditionalContact>();
                            if (ContactId.HasValue && secondaryCredit != null)
                            {
                                contactList = _Util.Facade.AdditionalContactFacade.GetAllAdditionalContactByCustomerId(cus.CustomerId);
                                if (contactList != null)
                                {
                                    foreach (var item in contactList)
                                    {
                                        item.IsSigningUsed = false;
                                        _Util.Facade.AdditionalContactFacade.UpdateAdditionalContact(item);
                                    }
                                }
                                secondaryCredit.IsSigningUsed = true;
                                _Util.Facade.AdditionalContactFacade.UpdateAdditionalContact(secondaryCredit);
                            }
                            #endregion

                        }
                        else
                        {
                            int count = 1;
                            foreach (var item in contractResult.FaultFields)
                            {
                                Message += count + "." + item.Key + ", " + item.Value + "</br>";
                            }
                            if (Message == "")
                            {
                                Message = contractResult.ResultData;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                result = false;
                Message = "Internal Error!";
            }
            return Json(new { result = result, Message = Message });
        }

        public ActionResult GetBrinksEventHistory(string CSNumber)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            SetupBrinks setUpBrink = new SetupBrinks();
            bool InProduction = false;
            #region IsInProduction
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            #endregion
            HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();

            string Entity = "Eventhistories";
            //string UserId = "ADSTest_WSI";
            //string Password = "Brinks!12345";

            string UserId = "";
            string Password = "";

            string DelearId = "";

            #region Credentials

            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
            if (UserIdGlobal != null)
            {
                UserId = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion
            var eventHistories = wsi.GetData(Entity, UserId, Password, CSNumber, null);
            List<BrinksEventHistory> eventHistoryList = new List<BrinksEventHistory>();
            if (eventHistories != null)
            {
                var HistoryTable = eventHistories.Tables[0];
                eventHistoryList = (from DataRow dr in HistoryTable.Rows
                                    select new BrinksEventHistory()
                                    {
                                        cs_no = dr["cs_no"].ToString(),
                                        zone_id = dr["zone_id"].ToString(),
                                        event_date = dr["event_date"] != DBNull.Value ? Convert.ToDateTime(dr["event_date"]) : new DateTime(),
                                        computed = dr["computed"].ToString(),
                                        event_id = dr["event_id"].ToString(),

                                    }).ToList();

            }

            return View(eventHistoryList);

        }


        public bool GetCommonFundingResult(Guid CustomerId)
        {
            bool result = false;
            string message = "";
            Customer Cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
            if (Cus != null)
            {
                string userName = "FundingDealer";
                string password = "J3d1Kn!gh8";

                string Login = "";
                string Password = "";
                var endPointAddress = new EndpointAddress("https://senti.monitronics.net/CommonFundingUAT");
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                #region Credentials

                var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
                if (UserIdGlobal != null)
                {
                    Login = UserIdGlobal.Value;
                }
                var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
                if (PasswordGlobal != null)
                {
                    Password = PasswordGlobal.Value;
                }
                #endregion

                var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
                if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
                {
                    endPointAddress = new EndpointAddress("https://mimasweb.monitronics.net/CommonFunding");
                }
                else
                {
                    endPointAddress = new EndpointAddress("https://senti.monitronics.net/CommonFundingUAT");
                }
                Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var basicHttpBinding = new BasicHttpBinding(
                                    BasicHttpSecurityMode.TransportWithMessageCredential);
                basicHttpBinding.Security.Message.ClientCredentialType =
                                                        BasicHttpMessageCredentialType.UserName;
                basicHttpBinding.MaxReceivedMessageSize = Int32.MaxValue;
                basicHttpBinding.MaxBufferSize = Int32.MaxValue;
                int EcontractId = 0;
                Int32.TryParse(cus.EcontractId, out EcontractId);
                try
                {
                    using (HS.CommonFunding.CommonFundingApi.VirtualInterfaceClient client = new HS.CommonFunding.CommonFundingApi.VirtualInterfaceClient(basicHttpBinding, endPointAddress))
                    {

                        client.ClientCredentials.UserName.UserName = userName;
                        client.ClientCredentials.UserName.Password = password;
                        CommonFundingDataResults commonFundingDataResults = client.UpsertCommonFundingDataFromThirdPartyCRM(2, Login, Password, cus.BrinksRefId, EcontractId);
                        if (commonFundingDataResults.Success == true)
                        {
                            result = true;
                            message = "Brinks Home Security Funding system updated successfully.";
                        }
                        else
                        {
                            message = commonFundingDataResults.FailureCode;
                            result = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    result = false;
                    message = "Internal Error";
                }

            }
            return result;
        }
        public ActionResult GetEcontract(string EcontractId, Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            string userName = "econtractapi";
            string password = "qh0`%Q$:51";
            var endPointAddress = new EndpointAddress("https://senti.monitronics.net/eContractAPIUAT");
            string Login = "";
            string Password = "";
            #region Credentials

            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
            if (UserIdGlobal != null)
            {
                Login = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                endPointAddress = new EndpointAddress("https://mimasweb.monitronics.net/eContractAPI");
            }
            else
            {
                endPointAddress = new EndpointAddress("https://senti.monitronics.net/eContractAPIUAT");
            }


            var basicHttpBinding = new BasicHttpBinding(
                                BasicHttpSecurityMode.TransportWithMessageCredential);
            basicHttpBinding.Security.Message.ClientCredentialType =
                                                    BasicHttpMessageCredentialType.UserName;
            basicHttpBinding.MaxReceivedMessageSize = Int32.MaxValue;
            basicHttpBinding.MaxBufferSize = Int32.MaxValue;
            try
            {
                using (HS.Econtract.eContractApi.VirtualInterfaceClient client = new HS.Econtract.eContractApi.VirtualInterfaceClient(basicHttpBinding, endPointAddress))
                {

                    client.ClientCredentials.UserName.UserName = userName;
                    client.ClientCredentials.UserName.Password = password;
                    ContractEnvelope contractPdf = client.GetContract(EcontractId);
                    if (contractPdf != null)
                    {
                        string fileName = "Econtract.pdf";

                        var pdfasBytes = contractPdf.PDFBytes;
                        if (pdfasBytes != null)
                        {
                            var FileNo = 1;
                            var ExistEcontractPdfCount = _Util.Facade.CustomerFileFacade.GetCustomerEcontractFileByCustomerId(cus.CustomerId);
                            if (ExistEcontractPdfCount != null)
                            {
                                FileNo = ExistEcontractPdfCount.Count + 1;
                            }
                            Random rand = new Random();
                            string filename = ConfigurationManager.AppSettings["File.LeadToCustomerAgreement"];
                            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                            var pdftempFolderName = string.Format(filename, comname) + cus.Id + "Econtract.pdf";
                            if (FileNo > 1)
                            {
                                pdftempFolderName = string.Format(filename, comname) + cus.Id + string.Format("Econtract_{0}.pdf", FileNo);
                            }
                            string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
                            FileHelper.SaveFile(pdfasBytes, Serverfilename);
                            var FileNameDes = "Econtract.pdf";
                            if (FileNo > 1)
                            {
                                FileNameDes = string.Format("Econtract_{0}.pdf", FileNo);
                            }
                            CustomerFile cfs = new CustomerFile()
                            {
                                FileDescription = cus.Id + "_" + FileNameDes,
                                FileId = Guid.NewGuid(),
                                Filename = AppConfig.DomainSitePath + "/" + pdftempFolderName,
                                FileFullName = cus.Id + FileNameDes,
                                Uploadeddate = DateTime.Now,
                                CustomerId = cus.CustomerId,
                                CompanyId = CurrentUser.CompanyId.Value,
                                IsActive = true,
                                CreatedBy = CurrentUser.UserId,
                                CreatedDate = DateTime.Now,
                                UpdatedBy = CurrentUser.UserId,
                                UpdatedDate = DateTime.Now,
                                WMStatus = LabelHelper.WatermarkStatus.Pending
                            };
                            int CustomerFileId = (int)_Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);

                        }

                        return File(pdfasBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                    }
                    else
                    {
                        return null;
                    }


                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }

        }
        public JsonResult GetThirdpartyAgencies(string ZipCode, Guid CustomerId)
        {

            bool result = false;
            string message = "";
            bool InProduction = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            #region IsInProduction
            var BrinksInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksInProduction");
            if (BrinksInProductionGlobal != null && BrinksInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            #endregion
            if (!string.IsNullOrEmpty(ZipCode))
            {
                List<CustomerThirdPartyAgency> agenciyBrinks = new List<CustomerThirdPartyAgency>();
                agenciyBrinks = _Util.Facade.CustomerFacade.GetAllCustomerThirdPartyAgencyByCustomerId(CustomerId, "'Brinks'");
                if (agenciyBrinks != null)
                {
                    foreach (var agency in agenciyBrinks)
                    {
                        _Util.Facade.CustomerFacade.DeleteCustomerThirdPartyAgency(agency.Id);
                    }
                }
                HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();

                string Entity = "Agencies";
                string UserId = "";
                string Password = "";
                #region Credentials

                var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksUserId");
                if (UserIdGlobal != null)
                {
                    UserId = UserIdGlobal.Value;
                }
                var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksPassword");
                if (PasswordGlobal != null)
                {
                    Password = PasswordGlobal.Value;
                }
                #endregion


                string XML = string.Format(@"<?xml version='1.0'?>
                        <GetAgencies xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>
                        <GetAgency zip_code='{0}'/>
                        </GetAgencies>", ZipCode);
                XML = XML.Replace("'", "\"");
                try
                {
                    var AgenciList = wsi.GetData(Entity, UserId, Password, null, XML);

                    if (AgenciList != null)
                    {
                        var dataTable = AgenciList.Tables[0];
                        agenciyBrinks = (from DataRow dr in dataTable.Rows
                                         select new CustomerThirdPartyAgency()
                                         {
                                             AgencyNo = dr["agency_no"].ToString(),
                                             Agencytype = dr["agencytype_id"].ToString(),
                                             AgencyName = dr["agency_name"].ToString(),
                                             City = dr["city_name"].ToString(),
                                             State = dr["state_id"].ToString(),
                                             Phone = dr["phone1"].ToString(),
                                             CustomerId = CustomerId,
                                             Platform = "Brinks"
                                         }).ToList();

                        foreach (var item in agenciyBrinks)
                        {
                            _Util.Facade.CustomerFacade.InsertCustomerAgency(item);
                        }
                        message = "Zip Code required for get agencies.";
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(message, ex);
                    message = "Internal Error!";
                }


                result = true;
            }
            else
            {
                result = false;
            }

            return Json(new { result = result });
        }
        public JsonResult DeleteThirdpartyContact(int ContactId)
        {

            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            try
            {

                _Util.Facade.EmergencyContactFacade.DeleteEmergencyContactById(ContactId);
                result = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }


            return Json(new { result = result });
        }

        public JsonResult DeleteThirdpartyAgency(int AgencyId)
        {

            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            try
            {

                _Util.Facade.CustomerFacade.DeleteCustomerThirdPartyAgency(AgencyId);
                result = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }


            return Json(new { result = result });
        }

        public JsonResult GetThirdpartyContact(int ContactId)
        {

            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            EmergencyContact model = new EmergencyContact();
            try
            {
                model = _Util.Facade.EmergencyContactFacade.GetEmergencyContactById(ContactId);
                result = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }


            return Json(new { result = model });
        }

        public JsonResult GetCustomerSecurityZone(int ZoneId)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CustomerSecurityZones model = new CustomerSecurityZones();
            try
            {
                model = _Util.Facade.CustomerFacade.GetCustomerSecurityZoneById(ZoneId);
                result = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return Json(new { result = model });
        }

        public ActionResult CustomerEmgContactlist(Guid CustomerId, string Platform)
        {
            List<EmergencyContact> thirdPartyContactList = new List<EmergencyContact>();
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);

            if (cus != null)
            {
                thirdPartyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerIdandPlatform(CustomerId, Platform);
                if (thirdPartyContactList != null && thirdPartyContactList.Count > 0)
                {
                    foreach (var item in thirdPartyContactList)
                    {
                        if (item.HasKey == "Y")
                        {
                            item.HasKey = "True";
                        }
                        else if (item.HasKey == "N")
                        {
                            item.HasKey = "False";
                        }
                        _Util.Facade.EmergencyContactFacade.UpdateEmergencyContact(item);
                    }
                }

            }

            return View(thirdPartyContactList);
        }
        #endregion

        #region UCC
        [Authorize]
        public ActionResult UCC(Guid CustomerId)
        {
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            return View(cus);
        }
        [Authorize]
        public ActionResult UCCCustomerDetails(string UCCRefId, Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ResultUcc model = new ResultUcc();
            ViewBag.UCCRefId = UCCRefId;
            #region Get Credential From GlobalSetting
            var UserName = "";
            var Password = "";
            var SiteNumber = "";
            var UccUserName = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccUserName");
            if (UccUserName != null)
            {
                UserName = UccUserName.Value;
            }
            var UccPassword = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccPassword");
            if (UccPassword != null)
            {
                Password = UccPassword.Value;
            }
            Customer Cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
            #region Check Production or not
            bool UccInProduction = false;
            string UccUrl = "";
            GlobalSetting globset2 = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("UccInProduction", CurrentUser.CompanyId.Value);
            if (globset2 != null && globset2.Value.ToLower() == "true")
            {
                UccInProduction = true;
            }
            if (UccInProduction == true)
            {
                UccUrl = "https://dealer2.teamucc.com/SiteGroupGateway/OpenAPI/";
            }
            else
            {
                UccUrl = "https://dealer.teamucc.com/SiteGroupGatewayDev/OpenAPI/";
            }
            #endregion

            #endregion
            ViewBag.UccTestDuration = _Util.Facade.LookupFacade.GetLookupByKey("UccTestDuration").Select(x =>
              new SelectListItem()
              {
                  Text = x.DisplayText.ToString(),
                  Value = x.DataValue.ToString()
              }).ToList();
            var client = new RestClient(UccUrl + "method/GetSite");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "5f1e7e08-821d-d34f-ef2e-2a2a0fa04fe9");
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("application/json", "{\n\t\"UserName\":\"" + UserName + "\",\n\t\"Password\":\"" + Password + "\",\n\t\"TransmitterCode\":\"" + UCCRefId + "\"\n\t\n}", ParameterType.RequestBody);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            try
            {
                IRestResponse response = client.Execute(request);
                model = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultUcc>(response.Content);
                try
                {
                    if (model.Result != null && Cus != null)
                    {
                        #region Syncing Zones
                        List<CustomerSecurityZones> securityZonesList = new List<CustomerSecurityZones>();
                        securityZonesList = _Util.Facade.CustomerFacade.GetAllCustomerSecurityZoneByCustomerId(Cus.CustomerId, "'UCC'");
                        if (securityZonesList != null && securityZonesList.Count > 0)
                        {
                            foreach (var item in securityZonesList)
                            {
                                _Util.Facade.CustomerFacade.DeleteCustomerSecurityZone(item.ID);
                            }
                        }
                        if (model.Result.Devices != null && model.Result.Devices.Count > 0)
                        {

                            foreach (var item in model.Result.Devices)
                            {
                                if (item.Points != null && item.Points.Count > 0)
                                {
                                    foreach (var zones in item.Points)
                                    {
                                        CustomerSecurityZones securityZones = new CustomerSecurityZones();
                                        securityZones.CustomerId = Cus.CustomerId;
                                        securityZones.ZoneNumber = zones.Point;
                                        securityZones.Platform = "UCC";
                                        securityZones.Location = zones.Description;
                                        securityZones.EventCode = zones.EventCode;
                                        securityZones.CreatedBy = CurrentUser.UserId;
                                        securityZones.CreatedDate = DateTime.Now.UTCCurrentTime();
                                        _Util.Facade.CustomerFacade.InsertCustomerSecurityZone(securityZones);
                                    }

                                }
                            }
                        }
                        #endregion

                        #region Syncing Agencies
                        List<CustomerThirdPartyAgency> agencyList = new List<CustomerThirdPartyAgency>();
                        agencyList = _Util.Facade.CustomerFacade.GetAllCustomerThirdPartyAgencyByCustomerId(Cus.CustomerId, "'UCC'");
                        if (agencyList != null && agencyList.Count > 0)
                        {
                            foreach (var item in agencyList)
                            {
                                _Util.Facade.CustomerFacade.DeleteCustomerThirdPartyAgency(item.Id);
                            }
                        }
                        if (model.Result.SiteAgencies != null && model.Result.SiteAgencies.Count > 0)
                        {
                            CustomerThirdPartyAgency agency = new CustomerThirdPartyAgency();
                            foreach (var item in model.Result.SiteAgencies)
                            {
                                agency = new CustomerThirdPartyAgency();
                                agency.AgencyName = item.AgencyName;
                                agency.AgencyNo = item.AgencyNum > 0 ? item.AgencyNum.ToString() : "";
                                agency.Agencytype = item.AgencyType;
                                agency.CustomerId = Cus.CustomerId;
                                agency.Phone = item.AgencyPhone;
                                agency.PermitNo = item.Permit != null ? item.Permit.ToString() : "";
                                agency.Platform = "UCC";

                                _Util.Facade.CustomerFacade.InsertCustomerAgency(agency);
                            }
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }


            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ViewBag.WorningMessage = "Server Down! Please try again.";
            }
            return View(model);
        }
        public ActionResult EditGlobalSettingsUCCSearchkeyandValue()
        {
            UccCredentialsSettings uccCredentialsSettings = new UccCredentialsSettings();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));


            GlobalSetting globalsetting = new GlobalSetting();

            globalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "UccUserName");
            uccCredentialsSettings.UccUserName = globalsetting.Value;

            globalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "UccPassword");
            uccCredentialsSettings.UccPassword = globalsetting.Value;

            globalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "UccSiteGroupNumber");
            uccCredentialsSettings.UccSiteGroupNumber = globalsetting.Value;



            globalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "UccInProduction");
            uccCredentialsSettings.UccInProduction = bool.Parse(globalsetting.Value);


            return PartialView("EditGlobalSettingsUCCSearchkeyandValue", uccCredentialsSettings);

        }

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public JsonResult SaveGlobalSettingsUCCSearchkeyandValue(UccCredentialsSettings uccCredentialsSettings)
        {
            bool result = false;
            string message = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            GlobalSetting globalSetting = new GlobalSetting();
            try
            {
                globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccUserName");
                if (!String.IsNullOrEmpty(uccCredentialsSettings.UccUserName))
                {
                    globalSetting.Value = uccCredentialsSettings.UccUserName;
                    _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(globalSetting);
                }

                globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccPassword");
                if (!String.IsNullOrEmpty(uccCredentialsSettings.UccPassword))
                {
                    globalSetting.Value = uccCredentialsSettings.UccPassword;
                    _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(globalSetting);
                }

                globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccSiteGroupNumber");
                if (!String.IsNullOrEmpty(uccCredentialsSettings.UccSiteGroupNumber))
                {
                    globalSetting.Value = uccCredentialsSettings.UccSiteGroupNumber;
                    _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(globalSetting);
                }


                globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccInProduction");
                if (uccCredentialsSettings.UccInProduction == false || uccCredentialsSettings.UccInProduction == true)
                {
                    globalSetting.Value = uccCredentialsSettings.UccInProduction.ToString();
                    _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(globalSetting);
                }
                message = "updated successfully";
                result = true;
            }
            catch (Exception e)
            {
                logger.Error(e);
                message = "update failed";
            }
            return Json(new { result = result, message = message });
        }
        public ActionResult UCCCustomerHistory(string UCCRefId, DateTime startdate, DateTime enddate)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            UccCustomerHistory model = new UccCustomerHistory();
            ViewBag.UCCRefId = UCCRefId;
            #region Get Credential From GlobalSetting
            var UserName = "";
            var Password = "";
            var SiteNumber = "";
            var UccUserName = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccUserName");
            if (UccUserName != null)
            {
                UserName = UccUserName.Value;
            }
            var UccPassword = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccPassword");
            if (UccPassword != null)
            {
                Password = UccPassword.Value;
            }

            #region Check Production or not
            bool UccInProduction = false;
            string UccUrl = "";
            GlobalSetting globset2 = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("UccInProduction", CurrentUser.CompanyId.Value);
            if (globset2 != null && globset2.Value.ToLower() == "true")
            {
                UccInProduction = true;
            }
            if (UccInProduction == true)
            {
                UccUrl = "https://dealer2.teamucc.com/SiteGroupGateway/OpenAPI/";
            }
            else
            {
                UccUrl = "https://dealer.teamucc.com/SiteGroupGatewayDev/OpenAPI/";
            }
            #endregion

            #endregion

            var client = new RestClient(UccUrl + "method/GetHistory");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "5f1e7e08-821d-d34f-ef2e-2a2a0fa04fe9");
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("application/json", "{\r\n  \"UserName\":\"" + UserName + "\",\r\n  \"Password\":\"" + Password + "\",\r\n  \"TransmitterCode\":\"" + UCCRefId + "\",\r\n  \"StartDate\":\"" + startdate.SetZeroHour().ToString("s") + "\",\r\n  \"EndDate\":\"" + enddate.SetMaxHour().ToString("s") + "\"\r\n\r\n}", ParameterType.RequestBody);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            try
            {
                IRestResponse response = client.Execute(request);
                model = Newtonsoft.Json.JsonConvert.DeserializeObject<UccCustomerHistory>(response.Content);
            }
            catch (Exception ex)
            {
                ViewBag.WorningMessage = "Server Down! Please try again.";
                logger.Error(ex);
            }
            return View(model.Result);
        }
        [Authorize]
        public JsonResult SyncUCCCustomer(string UCCRefId, Guid CustomerGuidId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            ResultUcc model = new ResultUcc();
            string message = "Successfully Sync";
            bool result = false;
            var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerGuidId);
            #region CheckExistingSite
            Customer ExistUccCustomer = _Util.Facade.CustomerFacade.IsCustomerUccExistCheck(UCCRefId).FirstOrDefault();
            #endregion
            if (ExistUccCustomer == null)
            {
                #region Get Credential From GlobalSetting
                var UserName = "";
                var Password = "";
                var SiteNumber = "";
                var UccUserName = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccUserName");
                if (UccUserName != null)
                {
                    UserName = UccUserName.Value;
                }
                var UccPassword = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccPassword");
                if (UccPassword != null)
                {
                    Password = UccPassword.Value;
                }

                #region Check Production or not
                bool UccInProduction = false;
                string UccUrl = "";
                GlobalSetting globset2 = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("UccInProduction", CurrentUser.CompanyId.Value);
                if (globset2 != null && globset2.Value.ToLower() == "true")
                {
                    UccInProduction = true;
                }
                if (UccInProduction == true)
                {
                    UccUrl = "https://dealer2.teamucc.com/SiteGroupGateway/OpenAPI/";
                }
                else
                {
                    UccUrl = "https://dealer.teamucc.com/SiteGroupGatewayDev/OpenAPI/";
                }
                #endregion

                #endregion

                if (CustomerDetails != null)
                {
                    var client = new RestClient(UccUrl + "method/GetSite");
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("postman-token", "5f1e7e08-821d-d34f-ef2e-2a2a0fa04fe9");
                    request.AddHeader("cache-control", "no-cache");
                    request.AddParameter("application/json", "{\n\t\"UserName\":\"" + UserName + "\",\n\t\"Password\":\"" + Password + "\",\n\t\"TransmitterCode\":\"" + UCCRefId + "\"\n\t\n}", ParameterType.RequestBody);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    try
                    {
                        IRestResponse response = client.Execute(request);
                        message = response.Content;
                        model = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultUcc>(response.Content);
                        if (model.Result != null)
                        {
                            CustomerDetails.UCCRefId = UCCRefId;
                            CustomerDetails.CustomerNo = UCCRefId;
                            result = _Util.Facade.CustomerFacade.UpdateCustomer(CustomerDetails);
                            #region Update Customer System No
                            CustomerSystemNo cusno = new CustomerSystemNo();
                            cusno = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerNo(CustomerDetails.UCCRefId);
                            if (cusno != null)
                            {
                                cusno.IsUsed = true;
                                cusno.CustomerId = CustomerDetails.Id;
                                cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                                _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(cusno);
                            }
                            #endregion
                            if (CustomerDetails != null && CustomerGuidId != new Guid())
                            {
                                base.AddUserActivityForCustomer("Customer" + "#" + CustomerDetails.Id + " synced successfully to UCC by" + (CurrentUser.FirstName + " " + CurrentUser.LastName), LabelHelper.ActivityAction.AddAlarm_com, CustomerGuidId, null, null);

                            }

                        }
                        else
                        {
                            message = "No Record Found.";
                            result = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        //message = "Internal Error!";
                        message = message ?? "Internal Error!";
                        logger.Error($"Response:{message} \nError:{ex}");
                    }
                }
            }
            else
            {
                result = false;
                message = "This Ucc id already synced with " + ExistUccCustomer.FirstName + " " + ExistUccCustomer.LastName + "(Id:" + ExistUccCustomer.Id + ")";
            }


            return Json(new { result = result, message = message });
        }
        [Authorize]
        public ActionResult AddUCCCustomer(Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            UccResultList uccModel = new UccResultList();
            List<CodewordsObject> codeWordList = new List<CodewordsObject>();
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            List<Alarm.AlarmCustomer.PanelDevice> divicelist = new List<HS.Alarm.AlarmCustomer.PanelDevice>();
            divicelist = GetAlarmSystemInfoList(CustomerId);
            CustomerSecurityZones securityZones = new CustomerSecurityZones();
            List<CustomerSecurityZones> securityZoneList = new List<CustomerSecurityZones>();
            securityZoneList = _Util.Facade.CustomerFacade.GetAllCustomerSecurityZoneByCustomerId(CustomerId, "'UCC'");
            var emContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerId(CustomerId);
            ViewBag.ZoneList = null;
            ViewBag.AgencyList = null;
            #region Get Credential From GlobalSetting
            var UserName = "";
            var Password = "";
            var SiteNumber = "";
            var UccUserName = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccUserName");
            if (UccUserName != null)
            {
                UserName = UccUserName.Value;
            }
            var UccPassword = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccPassword");
            if (UccPassword != null)
            {
                Password = UccPassword.Value;
            }
            #region Check Production or not
            bool UccInProduction = false;
            string UccUrl = "";
            GlobalSetting globset2 = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("UccInProduction", CurrentUser.CompanyId.Value);
            if (globset2 != null && globset2.Value.ToLower() == "true")
            {
                UccInProduction = true;
            }
            if (UccInProduction == true)
            {
                UccUrl = "https://dealer2.teamucc.com/SiteGroupGateway/OpenAPI/";
            }
            else
            {
                UccUrl = "https://dealer.teamucc.com/SiteGroupGatewayDev/OpenAPI/";
            }
            #endregion
            #endregion

            #region Edit Ucc Customer
            if (!string.IsNullOrEmpty(cus.UCCRefId))
            {
                var client = new RestClient(UccUrl + "method/GetSite");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "5f1e7e08-821d-d34f-ef2e-2a2a0fa04fe9");
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter("application/json", "{\n\t\"UserName\":\"" + UserName + "\",\n\t\"Password\":\"" + Password + "\",\n\t\"TransmitterCode\":\"" + cus.UCCRefId + "\"\n\t\n}", ParameterType.RequestBody);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                try
                {
                    ResultUcc model = new ResultUcc();
                    IRestResponse response = client.Execute(request);
                    model = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultUcc>(response.Content);
                    uccModel.SiteName = model.Result.SiteName;
                    uccModel.SiteAddress = model.Result.SiteAddress;
                    uccModel.SiteAddr2 = model.Result.SiteAddr2;
                    uccModel.CrossStreet = model.Result.CrossStreet;
                    uccModel.City = model.Result.City;
                    uccModel.State = model.Result.State;
                    uccModel.ZipCode = model.Result.ZipCode;
                    uccModel.County = model.Result.County;
                    uccModel.SiteType = model.Result.SiteType;
                    uccModel.TransmitterCode = cus.UCCRefId;
                    uccModel.Contacts = model.Result.Contacts;
                    uccModel.Devices = model.Result.Devices;
                    uccModel.SiteAgencies = model.Result.SiteAgencies;

                    List<string> typedispatch = new List<string>();
                    List<string> typedevice = new List<string>();
                    if (model.Result.DispatchTypes != null)
                    {
                        var dis = model.Result.DispatchTypes;
                        if (dis.Count > 0)
                        {
                            foreach (var item in dis)
                            {
                                typedispatch.Add(item.DispatchType);
                            }
                        }
                        ViewBag.typedispatch = typedispatch;
                    }
                    if (model.Result.Devices != null)
                    {

                        var device = model.Result.Devices;
                        if (device.Count > 0)
                        {
                            foreach (var item in device)
                            {
                                ViewBag.ZoneList = item.Points;
                                typedevice.Add(item.DeviceType);
                            }
                            uccModel.PanelPhone = device[0].PanelPhone;
                            uccModel.ReceiverPhone = device[0].ReceiverPhone;
                        }
                        ViewBag.typedevice = typedevice;
                    }
                    if (model.Result.SiteAgencies != null && model.Result.SiteAgencies.Count > 0)
                    {
                        ViewBag.AgencyList = model.Result.SiteAgencies;
                    }
                    if (model.Result.Codewords != null)
                    {
                        uccModel.Codewords = model.Result.Codewords;
                    }

                    #region CustomerSecurityZone

                    if (divicelist != null && divicelist.Count > 0)
                    {

                        foreach (var item in divicelist)
                        {
                            if (securityZoneList.Find(x => x.ZoneNumber == item.DeviceId.ToString()) == null)
                            {
                                securityZones = new CustomerSecurityZones()
                                {
                                    CustomerId = CustomerId,
                                    ZoneNumber = item.DeviceId.ToString(),
                                    Location = item.WebSiteDeviceName,
                                    Platform = "UCC"
                                };
                                _Util.Facade.CustomerFacade.InsertCustomerSecurityZone(securityZones);
                            }
                        }

                    }

                    #endregion

                }
                catch (Exception ex)
                {
                    ViewBag.WorningMessage = "Server Down! Please try again.";
                    logger.Error(ex);
                }
            }
            #endregion
            #region Insert Ucc Customer
            else
            {
                string DisplayName = "";
                if (!string.IsNullOrEmpty(cus.BusinessName))
                {
                    DisplayName = cus.BusinessName;
                }
                else
                {
                    if (!string.IsNullOrEmpty(cus.FirstName))
                    {
                        DisplayName = cus.FirstName + ' ' + cus.LastName;
                    }

                }
                #region ModelAlways filldup from customer
                uccModel.SiteName = DisplayName;
                uccModel.SiteAddress = cus.Street;
                uccModel.SiteAddr2 = cus.Address2;
                uccModel.CrossStreet = cus.Street;
                uccModel.City = cus.City;
                uccModel.State = cus.State;
                uccModel.ZipCode = cus.ZipCode;
                uccModel.County = cus.County;
                uccModel.SiteType = cus.Type;
                uccModel.TransmitterCode = GetCSNumber(cus.CustomerId, "UCC");
                uccModel.PanelPhone = cus.PrimaryPhone != "" ? cus.PrimaryPhone : cus.CellNo;
                codeWordList.Add(new CodewordsObject()
                {
                    Codeword = cus.Passcode
                });
                uccModel.Codewords = codeWordList;
                List<EmergencyContact> emergencyContactList = new List<EmergencyContact>();
                emergencyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerId(cus.CustomerId);
                List<EmergencyContactUCC> Contacts = new List<EmergencyContactUCC>();

                List<ContactObject> contactsList = new List<ContactObject>();


                if (emergencyContactList != null && emergencyContactList.Count > 0)
                {
                    foreach (var item in emergencyContactList)
                    {
                        ContactObject emgContact = new ContactObject();
                        emgContact.FirstName = item.FirstName;
                        emgContact.LastName = item.LastName;

                        Phone phone = new Phone();
                        if (item.Phone != null)
                        {
                            phone.PhoneNumber = item.Phone;
                            //phone.PhoneType = UccemgContact.PhoneType1;
                            List<Phone> phoneNoList = new List<Phone>();
                            phoneNoList.Add(phone);
                            emgContact.Phones = phoneNoList;
                        }
                        contactsList.Add(emgContact);
                    }
                }
                uccModel.Contacts = contactsList;
                #region CustomerSecurityZone

                if (divicelist != null && divicelist.Count > 0)
                {
                    foreach (var item in securityZoneList)
                    {
                        _Util.Facade.CustomerFacade.DeleteCustomerSecurityZone(item.ID);
                    }
                    foreach (var item in divicelist)
                    {
                        securityZones = new CustomerSecurityZones()
                        {
                            CustomerId = CustomerId,
                            ZoneNumber = item.DeviceId.ToString(),
                            Location = item.WebSiteDeviceName,
                            Platform = "UCC"
                        };
                        _Util.Facade.CustomerFacade.InsertCustomerSecurityZone(securityZones);
                    }
                }

                #endregion


                #endregion


            }

            #region DropDown ViewBags
            ViewBag.CustomerId = cus.CustomerId;

            ViewBag.UccDispatchType = _Util.Facade.LookupFacade.GetLookupByKey("UccDispatchType").OrderBy(x => x.DataValue).Select(x =>
              new SelectListItem()
              {
                  Text = x.DisplayText.ToString(),
                  Value = x.DataValue.ToString()
              }).ToList();

            ViewBag.UccDeviceType = _Util.Facade.LookupFacade.GetLookupByKey("UccDeviceType").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();

            ViewBag.PhoneType = _Util.Facade.LookupFacade.GetLookupByKey("PhoneType").Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();


            ViewBag.UccZoneNumber = _Util.Facade.LookupFacade.GetLookupByKey("UccZoneNumber").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.UccLocation = _Util.Facade.LookupFacade.GetLookupByKey("UccLocation").Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();

            ViewBag.UccEventCode = _Util.Facade.LookupFacade.GetLookupByKey("UccEventCode").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();

            ViewBag.agencyType = _Util.Facade.LookupFacade.GetLookupByKey("UccAgencyType").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
              new SelectListItem()
              {
                  Text = x.DisplayText.ToString(),
                  Value = x.DataValue.ToString()
              }).ToList();

            List<Lookup> ReceiverLookup = _Util.Facade.LookupFacade.GetLookupByKey("CentralStationRecieverNumber").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).ToList();
            if (!string.IsNullOrEmpty(uccModel.TransmitterCode))
            {
                var Prefix = uccModel.TransmitterCode.Substring(0, 4);
                Lookup PreferedReceiver = ReceiverLookup.Where(x => x.DataValue != "-1" && x.DataValue.Substring(x.DataValue.Length - 4) == Prefix).FirstOrDefault();
                if (PreferedReceiver != null)
                {
                    ViewBag.PreferedReceiver = PreferedReceiver.DataValue;
                }
            }


            ViewBag.CentralStationRecieverNumber = ReceiverLookup.Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString(),

             }).ToList();



            #endregion

            #endregion

            return PartialView("_AddUCCCustomer", uccModel);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddUCCCustomer(UccResultList model, UccEmergencyContactModel UccemgContact, UccAgencyModel UccAgencies, Guid CustomerId, string TransmitterCode, List<SiteAgencies> UccAgencyList)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var result = false;
            string message = "Something wrong";
            #region Check Cs number used or not

            #region Get Credential From GlobalSetting
            var UserName = "";
            var Password = "";
            int SiteNumber = 0;
            var UccUserName = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccUserName");
            if (UccUserName != null)
            {
                UserName = UccUserName.Value;
            }
            var UccPassword = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccPassword");
            if (UccPassword != null)
            {
                Password = UccPassword.Value;
            }
            var UccSiteGroupNumber = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccSiteGroupNumber");
            if (UccSiteGroupNumber != null)
            {
                int.TryParse(UccSiteGroupNumber.Value, out SiteNumber);
            }

            #region Check Production or not
            bool UccInProduction = false;
            string UccUrl = "";
            GlobalSetting globset2 = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("UccInProduction", CurrentUser.CompanyId.Value);
            if (globset2 != null && globset2.Value.ToLower() == "true")
            {
                UccInProduction = true;
            }
            if (UccInProduction == true)
            {
                UccUrl = "https://dealer2.teamucc.com/SiteGroupGateway/OpenAPI/";
            }
            else
            {
                UccUrl = "https://dealer.teamucc.com/SiteGroupGatewayDev/OpenAPI/";
            }
            #endregion

            #endregion

            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            CustomerSystemNo sysNo = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerNo(model.TransmitterCode);
            if (sysNo != null && sysNo.IsUsed == true)
            {
                model.TransmitterCode = GetCSNumber(cus.CustomerId, "UCC");
                TransmitterCode = model.TransmitterCode;
            }
            #endregion
            List<PhoneObject> phoneList = new List<PhoneObject>();
            string PhoneType = "";
            if (cus.PhoneType != "-1")
            {
                PhoneType = cus.PhoneType;
            }
            else
            {
                PhoneType = null;
            }
            string SiteType = "";
            if (cus.Type != "-1" && cus.Type != null)
            {
                if (cus.Type == "Residential")
                {
                    SiteType = "R";
                }
                else
                {
                    SiteType = "C";
                }
            }
            phoneList.Add(new PhoneObject()
            {
                PhoneNumber = cus.PrimaryPhone,
                Extension = null,
                //AutoNotifyFlag = true
            });
            List<CodewordsObject> codeWordList = new List<CodewordsObject>();
            codeWordList.Add(new CodewordsObject()
            {
                Codeword = model.CodeWord
            });
            #region Add Site Agencies
            if (UccAgencyList == null)
            {
                UccAgencyList = new List<SiteAgencies>();
            }
            if (UccAgencies.AgencyNumber1 != null && UccAgencies.AgencyNumber1 != 0)
            {
                UccAgencyList.Add(new SiteAgencies()
                {
                    AgencyNum = UccAgencies.AgencyNumber1,
                    //AgencyType = UccAgencies.AgencyType1,
                    //Permit = null,
                    //AgencyName = UccAgencies.AgencyName1,
                    //AgencyPhone = UccAgencies.AgencyPhone1,
                });
            }

            if (UccAgencies.AgencyNumber2 != null && UccAgencies.AgencyNumber2 != 0)
            {
                UccAgencyList.Add(new SiteAgencies()
                {
                    AgencyNum = UccAgencies.AgencyNumber2,
                    //AgencyType = UccAgencies.AgencyType2,
                    //Permit = null,
                    //AgencyName = UccAgencies.AgencyName2,
                    //AgencyPhone = UccAgencies.AgencyPhone2,
                });
            }

            if (UccAgencies.AgencyNumber3 != null && UccAgencies.AgencyNumber3 != 0)
            {
                UccAgencyList.Add(new SiteAgencies()
                {
                    AgencyNum = UccAgencies.AgencyNumber3,
                    //AgencyType = UccAgencies.AgencyType3,
                    //Permit = null,
                    //AgencyName = UccAgencies.AgencyName3,
                    //AgencyPhone = UccAgencies.AgencyPhone3,
                });
            }

            if (UccAgencies.AgencyNumber4 != null && UccAgencies.AgencyNumber4 != 0)
            {
                UccAgencyList.Add(new SiteAgencies()
                {
                    AgencyNum = UccAgencies.AgencyNumber4,
                    //AgencyType = UccAgencies.AgencyType4,
                    //Permit = null,
                    //AgencyName = UccAgencies.AgencyName4,
                    //AgencyPhone = UccAgencies.AgencyPhone4,
                });
            }

            if (UccAgencies.AgencyNumber5 != null && UccAgencies.AgencyNumber5 != 0)
            {
                UccAgencyList.Add(new SiteAgencies()
                {
                    AgencyNum = UccAgencies.AgencyNumber5,
                    //AgencyType = UccAgencies.AgencyType5,
                    //Permit = null,
                    //AgencyName = UccAgencies.AgencyName5,
                    //AgencyPhone = UccAgencies.AgencyPhone5,
                });
            }
            #endregion



            #region Insert Site Contact
            List<EmergencyContact> Contacts = new List<EmergencyContact>();

            List<ContactObject> contactsList = new List<ContactObject>();


            if (!string.IsNullOrEmpty(UccemgContact.FirstName1))
            {
                ContactObject emgContact = new ContactObject();
                emgContact.FirstName = UccemgContact.FirstName1;
                emgContact.LastName = UccemgContact.LastName1;

                Phone phone = new Phone();
                if (UccemgContact.Phone1 != null)
                {
                    phone.PhoneNumber = UccemgContact.Phone1;
                    //phone.PhoneType = UccemgContact.PhoneType1;
                    List<Phone> phoneNoList = new List<Phone>();
                    phoneNoList.Add(phone);
                    emgContact.Phones = phoneNoList;
                }
                contactsList.Add(emgContact);
                Contacts.Add(new EmergencyContact()
                {
                    FirstName = UccemgContact.FirstName1,
                    LastName = UccemgContact.LastName1,
                    Phone = UccemgContact.Phone1,
                    PhoneType = UccemgContact.PhoneType1,
                    CustomerId = cus.CustomerId,
                    CompanyId = CurrentUser.CompanyId.Value
                });
            }
            if (!string.IsNullOrEmpty(UccemgContact.FirstName2))
            {
                ContactObject emgContact = new ContactObject();
                emgContact.FirstName = UccemgContact.FirstName2;
                emgContact.LastName = UccemgContact.LastName2;

                Phone phone = new Phone();
                if (UccemgContact.Phone2 != null)
                {
                    phone.PhoneNumber = UccemgContact.Phone2;
                    //phone.PhoneType = UccemgContact.PhoneType2;
                    List<Phone> phoneNoList = new List<Phone>();
                    phoneNoList.Add(phone);
                    emgContact.Phones = phoneNoList;

                }
                contactsList.Add(emgContact);
                Contacts.Add(new EmergencyContact()
                {
                    FirstName = UccemgContact.FirstName2,
                    LastName = UccemgContact.LastName2,
                    Phone = UccemgContact.Phone2,
                    PhoneType = UccemgContact.PhoneType2,
                    CustomerId = cus.CustomerId,
                    CompanyId = CurrentUser.CompanyId.Value
                });
            }
            if (!string.IsNullOrEmpty(UccemgContact.FirstName3))
            {
                ContactObject emgContact = new ContactObject();
                emgContact.FirstName = UccemgContact.FirstName3;
                emgContact.LastName = UccemgContact.LastName3;

                Phone phone = new Phone();
                if (UccemgContact.Phone3 != null)
                {
                    phone.PhoneNumber = UccemgContact.Phone3;
                    //phone.PhoneType = UccemgContact.PhoneType3;
                    List<Phone> phoneNoList = new List<Phone>();
                    phoneNoList.Add(phone);
                    emgContact.Phones = phoneNoList;
                }
                contactsList.Add(emgContact);
                Contacts.Add(new EmergencyContact()
                {
                    FirstName = UccemgContact.FirstName3,
                    LastName = UccemgContact.LastName3,
                    Phone = UccemgContact.Phone3,
                    PhoneType = UccemgContact.PhoneType3,
                    CustomerId = cus.CustomerId,
                    CompanyId = CurrentUser.CompanyId.Value
                });
            }

            if (!string.IsNullOrEmpty(UccemgContact.FirstName4))
            {
                ContactObject emgContact = new ContactObject();
                emgContact.FirstName = UccemgContact.FirstName4;
                emgContact.LastName = UccemgContact.LastName4;

                Phone phone = new Phone();
                if (UccemgContact.Phone4 != null)
                {
                    phone.PhoneNumber = UccemgContact.Phone4;
                    //phone.PhoneType = UccemgContact.PhoneType4;
                    List<Phone> phoneNoList = new List<Phone>();
                    phoneNoList.Add(phone);
                    emgContact.Phones = phoneNoList;
                }
                contactsList.Add(emgContact);
                Contacts.Add(new EmergencyContact()
                {
                    FirstName = UccemgContact.FirstName4,
                    LastName = UccemgContact.LastName4,
                    Phone = UccemgContact.Phone4,
                    PhoneType = UccemgContact.PhoneType4,
                    CustomerId = cus.CustomerId,
                    CompanyId = CurrentUser.CompanyId.Value
                });
            }


            #endregion

            List<Device> deviceList = new List<Device>();
            List<CustomerSecurityZones> securityZonesList = new List<CustomerSecurityZones>();
            securityZonesList = _Util.Facade.CustomerFacade.GetAllCustomerSecurityZoneByCustomerId(cus.CustomerId, "'UCC'");
            List<UccZone> uccZoneList = new List<UccZone>();
            if (model.DeviceTypeList != null)
            {
                foreach (var item in model.DeviceTypeList)
                {
                    if (securityZonesList != null && securityZonesList.Count > 0)
                    {
                        foreach (var zone in securityZonesList)
                        {
                            if (zone.ZoneNumber != null && zone.EventCode != "-1" && zone.Location != "")
                            {
                                uccZoneList.Add(new UccZone()
                                {
                                    Point = zone.ZoneNumber.ToString(),
                                    EventCode = zone.EventCode,
                                    Description = zone.Location,

                                });

                            }
                        }

                    }



                    deviceList.Add(new Device()
                    {
                        DeviceType = item,
                        TransmitterCode = TransmitterCode,
                        Points = uccZoneList,
                        InServiceFlag = true,
                        PanelPhone = model.PanelPhone,
                        ReceiverPhone = model.ReceiverPhone
                        //OOSStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
                    });
                }
            }
            else
            {
                deviceList.Add(new Device()
                {
                    DeviceType = "",
                    TransmitterCode = TransmitterCode,
                    Points = uccZoneList,
                    InServiceFlag = true,
                    PanelPhone = model.PanelPhone,
                    ReceiverPhone = model.ReceiverPhone
                });
            }
            List<DispatchTypesObject> dispatchTypesList = new List<DispatchTypesObject>();
            if (model.DispatchTypesList != null)
            {
                foreach (var item in model.DispatchTypesList)
                {
                    dispatchTypesList.Add(new DispatchTypesObject()
                    {
                        DispatchType = item
                    });
                }
            }


            List<SiteGroup> siteGroupList = new List<SiteGroup>();
            siteGroupList.Add(new SiteGroup()
            {
                SiteGroupNum = SiteNumber,
                SiteGroupTypeOverride = null
            });

            var dataModel = new CreateUCCCustomer
            {
                UserName = UserName,
                Password = Password,
                Site = new UccResultList
                {
                    Phones = phoneList,
                    Codewords = codeWordList,
                    SiteInstructions = null,
                    SiteAgencies = UccAgencyList,
                    Contacts = contactsList,
                    Devices = deviceList,
                    DispatchTypes = dispatchTypesList,

                    SiteAddress = model.SiteAddress,
                    SiteName = model.SiteName,
                    SiteAddr2 = null,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    County = model.County,

                    Latitude = null,
                    Longitude = null,
                    TimeZoneNum = 12,
                    CrossStreet = model.CrossStreet,
                    SiteType = SiteType,
                    Region = model.State,
                    Info = "",
                    Directions = null,
                    Pets = null,
                    Map = null,
                    MapPage = null,
                    MapCoordinates = null,
                    SiteID1 = null,
                    SiteID2 = null,
                    Subdivision = null,
                    ULCode = null,
                    SiteLanguage = null,
                    LockBoxCode = null,
                    LockBoxLocation = null,
                    BillingID = null,
                    SiteGroups = siteGroupList,
                    UDF = null,
                    SiteNum = 0
                }
            };
            try
            {
                string data = JsonConvert.SerializeObject(dataModel);
                var client = new RestClient(UccUrl + "method/ImportSite");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "5f1e7e08-821d-d34f-ef2e-2a2a0fa04fe9");
                request.AddHeader("cache-control", "no-cache");
                request.AddParameter("application/json", data, ParameterType.RequestBody);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                IRestResponse response = client.Execute(request);
                if (response.StatusDescription == "OK")
                {
                    UccErrorModel errors = new UccErrorModel();
                    errors = Newtonsoft.Json.JsonConvert.DeserializeObject<UccErrorModel>(response.Content);
                    if (errors.ErrorMessage == null || errors.ErrorNum == "145" || errors.ErrorMessage.Contains("Required Data is Missing") || (!string.IsNullOrEmpty(errors.ErrorMessage) && errors.ErrorMessage.ToLower().Contains("unknown error url")))
                    {
                        if (!string.IsNullOrWhiteSpace(cus.UCCRefId))
                        {
                            #region update Into ThirdpartyCustomer table
                            ThirdPartyCustomer tcustomer = new ThirdPartyCustomer();

                            tcustomer = _Util.Facade.CustomerFacade.GetThirdPartyCustomerByCustomerId(cus.CustomerId);
                            if (tcustomer != null)
                            {
                                tcustomer.Id = tcustomer.Id;
                                tcustomer.City = model.City != null ? model.City.ToString() : "";
                                tcustomer.State = model.State != null ? model.State.ToString() : "";
                                tcustomer.ZipCode = model.ZipCode != null ? model.ZipCode.ToString() : "";
                                tcustomer.SiteName = model.SiteName != null ? model.SiteName.ToString() : "";
                                tcustomer.ReceiverPhone = model.ReceiverPhone != null ? model.ReceiverPhone.ToString() : "";
                                tcustomer.PanelPhone = model.PanelPhone != null ? model.PanelPhone.ToString() : "";
                                tcustomer.PanelLocation = model.Devices != null && model.Devices.Count() > 0 && model.Devices[0].PanelLocation != null ? model.Devices[0].PanelLocation.ToString() : "";
                                tcustomer.InstallDate = cus.InstallDate.HasValue ? cus.InstallDate.Value : DateTime.UtcNow;
                                tcustomer.CustomerId = cus.CustomerId;
                                tcustomer.CrossStreet = model.CrossStreet != null ? model.CrossStreet.ToString() : "";
                                tcustomer.CountryName = model.County != null ? model.County.ToString() : "";
                                tcustomer.CodeWord = model.CodeWord != null ? model.CodeWord.ToString() : "";
                                tcustomer.CustomerNumber = Convert.ToInt32(model.TransmitterCode);
                                //tcustomer.DealerNumber = model.Devices[0].de;
                                tcustomer.SiteAddress = model.SiteAddress != null ? model.SiteAddress.ToString() : "";
                                tcustomer.eContact = cus.EcontractId;
                                _Util.Facade.CustomerFacade.UpdateThirdPartyCustomer(tcustomer);
                            }
                            else
                            {
                                #region Insert Into ThirdpartyCustomer table
                                tcustomer = new ThirdPartyCustomer();
                                tcustomer.AccountOnlineDate = DateTime.Now.UTCCurrentTime();
                                tcustomer.City = model.City != null ? model.City.ToString() : "";
                                tcustomer.State = model.State != null ? model.State.ToString() : "";
                                tcustomer.ZipCode = model.ZipCode != null ? model.ZipCode.ToString() : "";
                                tcustomer.SiteName = model.SiteName != null ? model.SiteName.ToString() : "";
                                tcustomer.ReceiverPhone = model.ReceiverPhone != null ? model.ReceiverPhone.ToString() : "";
                                tcustomer.PanelPhone = model.PanelPhone != null ? model.PanelPhone.ToString() : "";
                                tcustomer.PanelLocation = model.Devices != null && model.Devices.Count() > 0 && model.Devices[0].PanelLocation != null ? model.Devices[0].PanelLocation.ToString() : "";
                                tcustomer.InstallDate = cus.InstallDate.HasValue ? cus.InstallDate.Value : DateTime.UtcNow;
                                tcustomer.CustomerId = cus.CustomerId;
                                tcustomer.CrossStreet = model.CrossStreet != null ? model.CrossStreet.ToString() : "";
                                tcustomer.CountryName = model.County != null ? model.County.ToString() : "";
                                tcustomer.CodeWord = model.CodeWord != null ? model.CodeWord.ToString() : "";
                                tcustomer.CustomerNumber = Convert.ToInt32(model.TransmitterCode);
                                //tcustomer.DealerNumber = BrinksModel.servco_no;
                                tcustomer.SiteAddress = model.SiteAddress != null ? model.SiteAddress.ToString() : "";
                                tcustomer.eContact = cus.EcontractId;
                                tcustomer.CreatedBy = CurrentUser.UserId;
                                tcustomer.Platform = "UCC";
                                _Util.Facade.CustomerFacade.InsertThirdPartyCustomer(tcustomer);
                                #endregion
                            }

                            #endregion
                        }
                        else
                        {
                            #region Insert Into ThirdpartyCustomer table
                            ThirdPartyCustomer tcustomer = new ThirdPartyCustomer();
                            tcustomer.AccountOnlineDate = DateTime.Now.UTCCurrentTime();
                            tcustomer.City = model.City != null ? model.City.ToString() : "";
                            tcustomer.State = model.State != null ? model.State.ToString() : "";
                            tcustomer.ZipCode = model.ZipCode != null ? model.ZipCode.ToString() : "";
                            tcustomer.SiteName = model.SiteName != null ? model.SiteName.ToString() : "";
                            tcustomer.ReceiverPhone = model.ReceiverPhone != null ? model.ReceiverPhone.ToString() : "";
                            tcustomer.PanelPhone = model.PanelPhone != null ? model.PanelPhone.ToString() : "";
                            tcustomer.PanelLocation = model.Devices != null && model.Devices.Count() > 0 && model.Devices[0].PanelLocation != null ? model.Devices[0].PanelLocation.ToString() : "";
                            tcustomer.InstallDate = cus.InstallDate.HasValue ? cus.InstallDate.Value : DateTime.UtcNow;
                            tcustomer.CustomerId = cus.CustomerId;
                            tcustomer.CrossStreet = model.CrossStreet != null ? model.CrossStreet.ToString() : "";
                            tcustomer.CountryName = model.County != null ? model.County.ToString() : "";
                            tcustomer.CodeWord = model.CodeWord != null ? model.CodeWord.ToString() : "";
                            tcustomer.CustomerNumber = Convert.ToInt32(model.TransmitterCode);
                            //tcustomer.DealerNumber = BrinksModel.servco_no;
                            tcustomer.SiteAddress = model.SiteAddress != null ? model.SiteAddress.ToString() : "";
                            tcustomer.eContact = cus.EcontractId;
                            tcustomer.CreatedBy = CurrentUser.UserId;
                            tcustomer.Platform = "UCC";
                            _Util.Facade.CustomerFacade.InsertThirdPartyCustomer(tcustomer);
                            #endregion
                        }

                        cus.UCCRefId = TransmitterCode;
                        cus.CustomerNo = TransmitterCode;
                        _Util.Facade.CustomerFacade.UpdateCustomer(cus);

                        result = true;
                        message = "Successfully Created";
                        cus.Passcode = model.CodeWord;
                        _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                        List<EmergencyContact> EmContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerId(cus.CustomerId);
                        if (EmContactList != null)
                        {
                            foreach (var item in EmContactList)
                            {
                                _Util.Facade.EmergencyContactFacade.DeleteEmergencyContactById(item.Id);
                            }
                        }
                        if (Contacts != null)
                        {
                            foreach (var item in Contacts)
                            {
                                _Util.Facade.EmergencyContactFacade.InsertEmergencyContact(item);
                            }
                        }
                        CustomerSystemNo cusno = new CustomerSystemNo();
                        cusno = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerNo(cus.UCCRefId);
                        if (cusno != null)
                        {
                            cusno.IsUsed = true;
                            cusno.CustomerId = cus.Id;
                            cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                            _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(cusno);
                        }

                        #region Completed for ticket
                        try
                        {
                            #region Check do tickt completed or not 
                            bool isticketcompletedfunction = false;
                            GlobalSetting ticketcompletedForUCC = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("TicketCompletedForUCC");

                            if (ticketcompletedForUCC != null && ticketcompletedForUCC.Value.ToLower() == "true")
                            {
                                isticketcompletedfunction = true;
                            }
                            else
                            {
                                isticketcompletedfunction = false;
                            }
                            #endregion

                            if (isticketcompletedfunction)
                            {
                                Ticket _tick = _Util.Facade.TicketFacade.GetInstallationTicketByCustomerId(cus.CustomerId);
                                if (_tick != null)
                                {
                                    CalculatePayrollBrinks(_tick);
                                }
                                List<CustomerAppointmentEquipment> TicketItemList = new List<CustomerAppointmentEquipment>();
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
                                if (_tick != null && _tick.Status != LabelHelper.TicketStatus.Completed)
                                {
                                    Customer CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(_tick.CustomerId);

                                    if (CustomerDetails != null)
                                    {
                                        if (_tick.CompletedDate != null)
                                        {
                                            CustomerDetails.InstallDate = _tick.CompletedDate.Value;
                                        }

                                    }

                                    var objticketlist = _Util.Facade.TicketFacade.GetTicketListByCustomerIdAndCompanyIdAndNotCompleted(_tick.CustomerId, CurrentUser.CompanyId.Value);
                                    if (objticketlist != null && objticketlist.Count > 0)
                                    {
                                        foreach (var item in objticketlist)
                                        {
                                            var objappeqplist = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipmentByAppointmentIdAndIsBilling(item.TicketId);
                                            if (objappeqplist != null && objappeqplist.Count > 0)
                                            {
                                                foreach (var app in objappeqplist)
                                                {
                                                    app.IsBilling = false;
                                                    _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(app);
                                                }
                                            }
                                        }
                                    }
                                    var totalbillamount = 0.0;
                                    var objeqpappoint = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentEquipmentListByAppointmentId(_tick.TicketId);
                                    if (objeqpappoint != null && objeqpappoint.Count > 0)
                                    {
                                        foreach (var item in objeqpappoint)
                                        {
                                            if (item.IsService == true && item.IsDefaultService == false)
                                            {
                                                item.IsBilling = true;
                                                _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(item);
                                                totalbillamount += item.TotalPrice;
                                            }
                                        }
                                    }
                                    //var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(_tick.CustomerId);
                                    if (CustomerDetails != null)
                                    {
                                        var totalbillamountTax = 0.0;
                                        GlobalSetting GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, _tick.CustomerId);
                                        if (GetSalesTax != null)
                                        {
                                            totalbillamountTax = Math.Round((totalbillamount * Convert.ToDouble(GetSalesTax.Value)) / 100, 2);
                                        }
                                        CustomerDetails.MonthlyMonitoringFee = totalbillamount.ToString("#.##");
                                        CustomerDetails.BillAmount = defaultBillTaxVal ? totalbillamount + totalbillamountTax : totalbillamount;
                                        CustomerDetails.TotalTax = defaultBillTaxVal ? totalbillamountTax : 0;
                                        CustomerDetails.BillTax = defaultBillTaxVal;
                                        //_Util.Facade.CustomerFacade.UpdateCustomer(CustomerDetails);
                                    }

                                    TicketItemList = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipmentByTicketId(CurrentUser.CompanyId.Value, _tick.TicketId);
                                    if (TicketItemList != null && TicketItemList.Count() > 0)
                                    {
                                        foreach (var item in TicketItemList)
                                        {
                                            int totalQty = 0;
                                            int techAddQty = 0;
                                            int techReleaseQty = 0;
                                            bool eqpRelease = false;
                                            CustomerAppointmentEquipment CusAptEqpListDetails = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentEquipmentByAppoinmentIdAndEquipmentIdAndId(_tick.TicketId, item.EquipmentId, item.Id);
                                            TicketUser tikuser = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndPrimary(_tick.TicketId);
                                            List<InventoryTech> objinvtech = _Util.Facade.InventoryFacade.GetAllInventoryTechByTechnicianIdAndEquipmentIdAndType(tikuser.UserId, item.EquipmentId);
                                            if (objinvtech != null && objinvtech.Count > 0)
                                            {
                                                foreach (var invtech in objinvtech)
                                                {
                                                    if (invtech.Type == "Add")
                                                    {
                                                        techAddQty += invtech.Quantity;
                                                    }
                                                    if (invtech.Type == "Release")
                                                    {
                                                        techReleaseQty += invtech.Quantity;
                                                    }
                                                }
                                                totalQty = techAddQty - techReleaseQty;
                                            }

                                            if (totalQty > 0 && totalQty >= item.Quantity)
                                            {
                                                item.QuantityLeftEquipment = item.Quantity;
                                            }
                                            else if (totalQty > 0 && totalQty < item.Quantity)
                                            {
                                                item.QuantityLeftEquipment = totalQty;
                                            }
                                            if (CusAptEqpListDetails != null && CusAptEqpListDetails.IsEquipmentRelease == false && totalQty > 0)
                                            {
                                                CusAptEqpListDetails.IsEquipmentRelease = true;
                                                CustomerDetails.Installer = CusAptEqpListDetails.TechnicianId.ToString();
                                                CusAptEqpListDetails.QuantityLeftEquipment = item.QuantityLeftEquipment;
                                                _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinmentEquipment(CusAptEqpListDetails);
                                                var delres = _Util.Facade.InventoryFacade.DeleteInventoryTechByCustomerAppointmentEquipmentIdAndType(item.Id);
                                                InventoryTech invtech = new InventoryTech()
                                                {
                                                    CompanyId = CurrentUser.CompanyId.Value,
                                                    TechnicianId = item.TechnicianId,
                                                    EquipmentId = item.EquipmentId,
                                                    Type = LabelHelper.InventoryType.Release,
                                                    Quantity = item.QuantityLeftEquipment.HasValue ? item.QuantityLeftEquipment.Value : 0,
                                                    LastUpdatedBy = CurrentUser.UserId,
                                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                                    Description = "Release from technician by ticket",
                                                    CustomerAppointmentEquipmentId = item.Id
                                                };
                                                _Util.Facade.InventoryFacade.InsertInventoryTech(invtech);

                                            }

                                        }
                                    }
                                    _tick.LastUpdatedBy = CurrentUser.UserId;
                                    _tick.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                    _tick.CompletedDate = DateTime.Now.UTCCurrentTime();
                                    _tick.Status = LabelHelper.TicketStatus.Completed;
                                    _Util.Facade.TicketFacade.UpdateTicket(_tick);
                                    _Util.Facade.CustomerFacade.UpdateCustomer(CustomerDetails);
                                }
                            }
                            else
                            {
                                Ticket _tick = _Util.Facade.TicketFacade.GetInstallationTicketByCustomerId(cus.CustomerId);
                                if (_tick != null)
                                {
                                    CalculatePayrollBrinks(_tick);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex);
                        }
                        #endregion
                        if (cus != null && CustomerId != new Guid())
                        {
                            base.AddUserActivityForCustomer("Customer #" + cus.Id + "Update to UCC by " + (CurrentUser.FirstName + " " + CurrentUser.LastName), LabelHelper.ActivityAction.Updateucc, CustomerId, null, null);

                        }

                    }
                    else
                    {
                        result = false;
                        message = errors.ErrorMessage;
                    }

                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Json(new { result = result, message = message });
        }
        public ActionResult CustomerZoneList(Guid CustomerId, string Platform)
        {
            List<CustomerSecurityZones> securityZoneList = new List<CustomerSecurityZones>();
            securityZoneList = _Util.Facade.CustomerFacade.GetAllCustomerSecurityZoneByCustomerId(CustomerId, Platform);
            return View(securityZoneList);
        }
        public ActionResult CustomerAgencylist(Guid CustomerId, string Platform)
        {
            ViewBag.Platform = Platform;
            List<CustomerThirdPartyAgency> thirdPartyAgency = new List<CustomerThirdPartyAgency>();
            thirdPartyAgency = _Util.Facade.CustomerFacade.GetAllCustomerThirdPartyAgencyByCustomerId(CustomerId, Platform);
            return View(thirdPartyAgency);
        }



        //[Authorize]
        //public JsonResult PushUccSiteContact(UccEmergencyContactModel emgModel)
        //{
        //    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
        //    string message = "Successfully Updated.";
        //    bool result = false;
        //    UccSiteContact siteContact = new UccSiteContact();
        //    siteContact.UserName = "DFWDev1";
        //    siteContact.Password = "WfD0791$df";
        //    siteContact.TransmitterCode = emgModel.TransmitterCode;
        //    List<EmergencyContactUCC> Contacts = new List<EmergencyContactUCC>();



        //    if (!string.IsNullOrEmpty(emgModel.FirstName1))
        //    {
        //        EmergencyContactUCC emgContact = new EmergencyContactUCC();
        //        emgContact.FirstName = emgModel.FirstName1;
        //        emgContact.LastName = emgModel.LastName1;

        //        Phone phone = new Phone();
        //        if (emgModel.Phone1 != null)
        //        {
        //            phone.PhoneNumber = emgModel.Phone1;
        //            phone.PhoneType = emgModel.PhoneType1;
        //            List<Phone> phoneList = new List<Phone>();
        //            phoneList.Add(phone);
        //            emgContact.Phones = phoneList;
        //        }
        //        Contacts.Add(emgContact);
        //    }
        //    if (!string.IsNullOrEmpty(emgModel.FirstName2))
        //    {
        //        EmergencyContactUCC emgContact = new EmergencyContactUCC();
        //        emgContact.FirstName = emgModel.FirstName2;
        //        emgContact.LastName = emgModel.LastName2;
        //        Phone phone = new Phone();
        //        if (emgModel.Phone2 != null)
        //        {
        //            phone.PhoneNumber = emgModel.Phone2;
        //            phone.PhoneType = emgModel.PhoneType2;
        //            List<Phone> phoneList = new List<Phone>();
        //            phoneList.Add(phone);
        //            emgContact.Phones = phoneList;
        //        }
        //        Contacts.Add(emgContact);
        //    }
        //    if (!string.IsNullOrEmpty(emgModel.FirstName3))
        //    {
        //        EmergencyContactUCC emgContact = new EmergencyContactUCC();
        //        emgContact.FirstName = emgModel.FirstName3;
        //        emgContact.LastName = emgModel.LastName3;
        //        Phone phone = new Phone();
        //        if (emgModel.Phone3 != null)
        //        {
        //            phone.PhoneNumber = emgModel.Phone3;
        //            phone.PhoneType = emgModel.PhoneType3;
        //            List<Phone> phoneList = new List<Phone>();
        //            phoneList.Add(phone);
        //            emgContact.Phones = phoneList;
        //        }
        //        Contacts.Add(emgContact);
        //    }

        //    if (!string.IsNullOrEmpty(emgModel.FirstName4))
        //    {
        //        EmergencyContactUCC emgContact = new EmergencyContactUCC();
        //        emgContact.FirstName = emgModel.FirstName4;
        //        emgContact.LastName = emgModel.LastName4;
        //        Phone phone = new Phone();
        //        if (emgModel.Phone4 != null)
        //        {
        //            phone.PhoneNumber = emgModel.Phone4;
        //            phone.PhoneType = emgModel.PhoneType4;
        //            List<Phone> phoneList = new List<Phone>();
        //            phoneList.Add(phone);
        //            emgContact.Phones = phoneList;
        //        }
        //        Contacts.Add(emgContact);
        //    }

        //    SiteContacts siteContactList = new SiteContacts()
        //    {
        //        Contacts = Contacts
        //    };
        //    siteContact.SiteContacts = siteContactList;

        //    string data = JsonConvert.SerializeObject(siteContact);

        //    var client = new RestClient("https://dealer.teamucc.com/SiteGroupGatewayDev/OpenAPI/method/UpdateSiteContacts");
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("postman-token", "fb376d3b-96c1-eab0-a87f-f42c5155f8f0");
        //    request.AddHeader("cache-control", "no-cache");
        //    request.AddParameter("application/json", data, ParameterType.RequestBody);
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        //    try
        //    {
        //        IRestResponse response = client.Execute(request);
        //        if (response.StatusDescription == "OK")
        //        {

        //            result = true;
        //            message = "Successfully Updated";
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Json(new { result = result, message = message });

        //}
        public ActionResult UpdateUccSiteContact(string UCCRefId)
        {
            ViewBag.PhoneType = _Util.Facade.LookupFacade.GetLookupByKey("PhoneType").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString()
               }).ToList();
            ViewBag.TransmitterCode = UCCRefId;
            return View();
        }

        [Authorize]
        public JsonResult PlaceOnTest(UccTestModel model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string message = "";
            bool result = false;
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.CustomerGuidId);

            #region Get Credential From GlobalSetting
            var UserName = "";
            var Password = "";
            var SiteNumber = "";
            var UccUserName = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccUserName");
            if (UccUserName != null)
            {
                UserName = UccUserName.Value;
            }
            var UccPassword = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "UccPassword");
            if (UccPassword != null)
            {
                Password = UccPassword.Value;
            }

            #region Check Production or not
            bool UccInProduction = false;
            string UccUrl = "";
            GlobalSetting globset2 = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("UccInProduction", CurrentUser.CompanyId.Value);
            if (globset2 != null && globset2.Value.ToLower() == "true")
            {
                UccInProduction = true;
            }
            if (UccInProduction == true)
            {
                UccUrl = "https://dealer2.teamucc.com/SiteGroupGateway/OpenAPI/";
            }
            else
            {
                UccUrl = "https://dealer.teamucc.com/SiteGroupGatewayDev/OpenAPI/";
            }
            #endregion
            #endregion

            model.UserName = UserName;
            model.Password = Password;
            model.Comment = null;
            model.Minutes = 0;
            model.TestCategory = "0" + model.Hours;
            model.StartDate = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'");
            string data = JsonConvert.SerializeObject(model);
            var client = new RestClient(UccUrl + "method/OnTest");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "fb376d3b-96c1-eab0-a87f-f42c5155f8f0");
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("application/json", data, ParameterType.RequestBody);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            try
            {
                IRestResponse response = client.Execute(request);
                if (response.StatusDescription == "OK")
                {
                    UccErrorModel errors = new UccErrorModel();
                    errors = Newtonsoft.Json.JsonConvert.DeserializeObject<UccErrorModel>(response.Content);
                    if (errors.ErrorMessage == null)
                    {
                        result = true;
                        message = "Account is Placed on test";
                        if (cus != null && cus.CustomerId != new Guid())
                        {
                            base.AddUserActivityForCustomer("UCC" + "#" + model.TransmitterCode + "is placed on test by " + (CurrentUser.FirstName + " " + CurrentUser.LastName), LabelHelper.ActivityAction.AddAlarm_com, model.CustomerGuidId, null, null);

                        }

                    }
                    else
                    {
                        result = false;
                        message = errors.ErrorMessage;
                    }


                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return Json(new { result = result, message = message });

        }

        public JsonResult DeleteUccZone(int ZoneId)
        {

            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            try
            {

                _Util.Facade.CustomerFacade.DeleteCustomerSecurityZone(ZoneId);
                result = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }


            return Json(new { result = result });
        }

        #endregion

        #region Alarm
        [Authorize]
        public ActionResult Alarm(Guid CustomerId)
        {
            SetupAlarm AlarmModel = _Util.Facade.AlarmFacade.GetSetupalarmByCustomerId(CustomerId);
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (!string.IsNullOrEmpty(cus.AlarmRefId))
            {
                AlarmModel = new SetupAlarm();
                AlarmModel.CustomerStatus = LabelHelper.AlarmCustomerStatus.Created;
            }
            else if (cus.AlarmRefId == "" || cus.AlarmRefId == null)
            {
                AlarmModel = new SetupAlarm();
                AlarmModel.CustomerStatus = LabelHelper.AlarmCustomerStatus.Init;
            }
            ViewBag.AlarmRefId = cus.AlarmRefId;
            return PartialView("_Alarm", AlarmModel);
        }
        public ActionResult EditGlobalSettingsAlarmSearchkeyandValue()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));


            GlobalSetting globalsetting = new GlobalSetting();
            AlamcredentialsSetting alarmcredentials = new AlamcredentialsSetting();
            globalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "AlarmUsername");
            alarmcredentials.AlarmUsername = globalsetting.Value;

            globalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "AlarmPassword");
            alarmcredentials.AlarmPassword = globalsetting.Value;

            globalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "BrinksAlarmUserName");
            alarmcredentials.BrinksAlarmUsername = globalsetting.Value;

            globalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "BrinksAlarmUserPassword");
            alarmcredentials.BrinksAlarmPassword = globalsetting.Value;

            globalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "HasMultipleAlarmUser");
            alarmcredentials.Hasmultiplevalue = bool.Parse(globalsetting.Value);

            ViewBag.Hasmultiplevalue = alarmcredentials.Hasmultiplevalue;
            return PartialView("EditGlobalSettingsAlarmSearchkeyandValue", alarmcredentials);

        }

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public JsonResult EditSettings(AlamcredentialsSetting alamcredentialsSetting)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            string message = "";
            bool result = false;

            GlobalSetting globalsetting = new GlobalSetting();
            try
            {
                globalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "AlarmUsername");

                if (!String.IsNullOrEmpty(alamcredentialsSetting.AlarmUsername))
                {
                    globalsetting.Value = alamcredentialsSetting.AlarmUsername;
                    _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(globalsetting);
                }

                globalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "AlarmPassword");


                if (!String.IsNullOrEmpty(alamcredentialsSetting.AlarmPassword))
                {
                    globalsetting.Value = alamcredentialsSetting.AlarmPassword;
                    _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(globalsetting);
                }


                globalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "BrinksAlarmUserName");

                if (!String.IsNullOrEmpty(alamcredentialsSetting.BrinksAlarmUsername))
                {
                    globalsetting.Value = alamcredentialsSetting.BrinksAlarmUsername;
                    _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(globalsetting);
                }

                globalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "BrinksAlarmUserPassword");
                if (!String.IsNullOrEmpty(alamcredentialsSetting.BrinksAlarmPassword))
                {
                    globalsetting.Value = alamcredentialsSetting.BrinksAlarmPassword;
                    _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(globalsetting);
                }

                globalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "HasMultipleAlarmUser");
                if (alamcredentialsSetting.Hasmultiplevalue == false || alamcredentialsSetting.Hasmultiplevalue == true)
                {
                    globalsetting.Value = alamcredentialsSetting.Hasmultiplevalue.ToString();
                    _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(globalsetting);
                }
                result = true;
                message = "updated successfully";
            }


            catch (Exception e)
            {
                logger.Error(e);
                message = "update failed";

            }


            return Json(new { result = result, message = message });




        }
        [Authorize]
        public JsonResult SyncAlarmCustomer(int CustomerID, Guid CustomerGuidId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.CustomerId = CustomerID;
            string message = "";
            bool result = false;

            GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
            GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerGuidId);

            HasMultipleAlarmUser = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "HasMultipleAlarmUser");
            if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
            {
                if (cus != null && cus.Ownership == "Brinks")
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserName");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserPassword");
                }
                else
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
                }
            }
            else
            {
                AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
            }


            Customer ExistAlarmIdCustomer = _Util.Facade.CustomerFacade.IsCustomerAlarmIdExistCheck(CustomerID).FirstOrDefault();
            CustomerExtended extended = new CustomerExtended();

            extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
            if (ExistAlarmIdCustomer == null)
            {
                Alarm.AlarmCustomer.CustomerManagement response = new Alarm.AlarmCustomer.CustomerManagement()
                {
                    AuthenticationValue = new HS.Alarm.AlarmCustomer.Authentication()
                    {
                        User = AlarmUserGlobalSetting.Value,
                        Password = AlarmPassGlobalSetting.Value
                    }
                };
                Alarm.AlarmCustomer.CustomerInfo cusresponse = new HS.Alarm.AlarmCustomer.CustomerInfo();
                Alarm.AlarmCustomer.CustomerBestPracticesOutput practiceResponse = new Alarm.AlarmCustomer.CustomerBestPracticesOutput();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                try
                {
                    cusresponse = response.GetCustomerInfo(CustomerID);
                    result = true;
                    cus.AlarmRefId = CustomerID.ToString();
                    _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                    message = "Customer synced successfully";
                    base.AddUserActivityForCustomer("Customer" + "#" + CustomerID + " synced successfully to Alarm by" + (CurrentUser.FirstName + " " + CurrentUser.LastName), LabelHelper.ActivityAction.AddAlarm_com, CustomerGuidId, null, null);
                    #region Update Customer System No

                    CustomerSystemNo cusno = new CustomerSystemNo();
                    if (cusresponse.CentralStationInfo != null && !string.IsNullOrEmpty(cusresponse.CentralStationInfo.AccountNumber))
                    {
                        cusno = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerNo(cusresponse.CentralStationInfo.AccountNumber);
                    }

                    if (cusno != null)
                    {
                        cusno.IsUsed = true;
                        cusno.IsReserved = true;
                        cusno.CustomerId = cus.Id;
                        cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                        _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(cusno);
                    }
                    #endregion
                    #region Update customer Packages
                    try
                    {
                        int AutomationCount = 0;
                        int videoCount = 0;
                        int skybellCount = 0;
                        int Svrcount = 0;
                        AlarmCustomerSelectedAddon addon = new AlarmCustomerSelectedAddon();

                        List<AlarmCustomerSelectedAddon> OldAddon = new List<AlarmCustomerSelectedAddon>();
                        OldAddon = _Util.Facade.CustomerFacade.GetAllCutomerAlarmAddonsByCustomerId(cus.CustomerId);
                        if (OldAddon != null && OldAddon.Count > 0)
                        {
                            foreach (var item in OldAddon)
                            {
                                _Util.Facade.CustomerFacade.DeleteCutomerAlarmAddons(item.Id);
                            }
                        }
                        if (extended != null)
                        {
                            extended.AlarmBasicPackage = "";
                            _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                        }
                        if (cusresponse.ServicePlanInfo.Addons != null && cusresponse.ServicePlanInfo.Addons.Count() > 0)
                        {
                            foreach (var item in cusresponse.ServicePlanInfo.Addons)
                            {
                                AlarmAddOnns addons = _Util.Facade.AlarmFacade.GeAddOnnsByName(item.ToString());
                                if (addons.Value == 36 || addons.Value == 41 || addons.Value == 2 || addons.Value == 5 || addons.Value == 23 || addons.Value == 21 || addons.Value == 19 || addons.Value == 20 || addons.Value == 52)
                                {
                                    AutomationCount++;
                                }
                                else if (addons.Value == 76 || addons.Value == 70)
                                {
                                    skybellCount++;
                                }
                                else if (addons.Value == 12 || addons.Value == 13)
                                {
                                    videoCount++;
                                }
                                else if (addons.Value == 43)
                                {
                                    Svrcount++;
                                }

                            }

                            if (AutomationCount == 1)
                            {
                                addon = new AlarmCustomerSelectedAddon();

                                addon.CustomerId = cus.CustomerId;
                                addon.AddonType = "Automation1";

                                _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                            }
                            if (AutomationCount == 2)
                            {

                                addon = new AlarmCustomerSelectedAddon();

                                addon.CustomerId = cus.CustomerId;
                                addon.AddonType = "Automation2";

                                _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                            }
                            if (AutomationCount == 3)
                            {
                                addon = new AlarmCustomerSelectedAddon();

                                addon.CustomerId = cus.CustomerId;
                                addon.AddonType = "Automation3";

                                _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                            }
                            if (AutomationCount > 3)
                            {
                                addon = new AlarmCustomerSelectedAddon();

                                addon.CustomerId = cus.CustomerId;
                                addon.AddonType = "Automation4";
                                _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                            }
                            if (Svrcount > 0)
                            {
                                addon = new AlarmCustomerSelectedAddon();

                                addon.CustomerId = cus.CustomerId;
                                addon.AddonType = "SVR";
                                _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                            }
                            if (skybellCount > 0 && videoCount > 0)
                            {
                                addon = new AlarmCustomerSelectedAddon();

                                addon.CustomerId = cus.CustomerId;
                                addon.AddonType = "ProVideo";
                                _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                            }
                            else
                            {
                                if (skybellCount > 0)
                                {
                                    addon = new AlarmCustomerSelectedAddon();

                                    addon.CustomerId = cus.CustomerId;
                                    addon.AddonType = "SkyBell";
                                    _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                                }
                                else if (videoCount > 0)
                                {

                                    addon = new AlarmCustomerSelectedAddon();

                                    addon.CustomerId = cus.CustomerId;
                                    addon.AddonType = "ProVideo";
                                    _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                                }
                            }

                        }

                        if (cusresponse.ServicePlanInfo.PackageId > 0)
                        {
                            AlarmCustomerSelectedAddon basicaddon = new AlarmCustomerSelectedAddon();
                            if (cusresponse.ServicePlanInfo.PackageId == 208 || cusresponse.ServicePlanInfo.PackageId == 209 || cusresponse.ServicePlanInfo.PackageId == 193)
                            {

                                if (extended != null)
                                {
                                    extended.AlarmBasicPackage = "Interactive";
                                    _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                                }
                                else
                                {
                                    extended.CustomerId = cus.CustomerId;
                                    extended.AlarmBasicPackage = "Interactive";
                                    _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
                                }
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }
                    #endregion
                    #region Delete previous Security Zones
                    try
                    {

                        List<Alarm.AlarmCustomer.PanelDevice> divicelist = new List<HS.Alarm.AlarmCustomer.PanelDevice>();
                        divicelist = GetAlarmSystemInfoList(CustomerGuidId);
                        List<CustomerSecurityZones> securityZoneList = _Util.Facade.CustomerFacade.GetAllCustomerSecurityZoneByCustomerId(CustomerGuidId, "'Brinks'");
                        if (securityZoneList != null && securityZoneList.Count > 0)
                        {
                            foreach (var item in securityZoneList)
                            {
                                _Util.Facade.CustomerFacade.DeleteCustomerSecurityZone(item.ID);
                            }
                        }
                        CustomerSecurityZones securityZones = new CustomerSecurityZones();
                        if (divicelist != null && divicelist.Count > 0)
                        {
                            foreach (var item in divicelist)
                            {
                                securityZones = new CustomerSecurityZones()
                                {
                                    CustomerId = CustomerGuidId,
                                    ZoneNumber = item.DeviceId.ToString(),
                                    ZoneComment = item.WebSiteDeviceName,
                                    Platform = "Brinks"
                                };
                                _Util.Facade.CustomerFacade.InsertCustomerSecurityZone(securityZones);
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    result = false;
                    if (ex.InnerException != null)
                    {
                        message = ex.InnerException.Message;
                    }
                    else
                    {
                        message = "Customer not found";
                    }


                }
            }
            else
            {
                result = false;
                message = "This alarm id already synced with " + ExistAlarmIdCustomer.FirstName + " " + ExistAlarmIdCustomer.LastName + "(Id:" + ExistAlarmIdCustomer.Id + ")";
            }

            return Json(new { result = result, message = message });
        }
        [Authorize]
        public JsonResult RemoveUnassociateCus(Guid CustomerGuidId, string Platform)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            string message = "";
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerGuidId);
            CustomerExtended cusEx = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(CustomerGuidId);
            try
            {
                if (Platform == "Alarm")
                {

                    cus.AlarmRefId = "";
                    cus.CustomerNo = "";
                    List<AlarmCustomerSelectedAddon> OldAddon = new List<AlarmCustomerSelectedAddon>();
                    OldAddon = _Util.Facade.CustomerFacade.GetAllCutomerAlarmAddonsByCustomerId(cus.CustomerId);
                    if (OldAddon != null && OldAddon.Count > 0)
                    {
                        foreach (var item in OldAddon)
                        {
                            _Util.Facade.CustomerFacade.DeleteCutomerAlarmAddons(item.Id);
                        }
                    }
                    if (cusEx != null)
                    {
                        cusEx.AlarmBasicPackage = "";
                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(cusEx);
                    }
                    SetupAlarm alarm = _Util.Facade.AlarmFacade.GetSetupalarmByCustomerId(CustomerGuidId);
                    if (alarm != null)
                    {
                        _Util.Facade.AlarmFacade.DeleteSetupAlarm(alarm.Id);
                    }
                }
                else if (Platform == "Brinks")
                {
                    cus.BrinksRefId = "";
                    cus.CustomerNo = "";
                    cus.AccountNo = "";
                }
                else if (Platform == "UCC")
                {
                    cus.UCCRefId = "";
                    cus.CustomerNo = "";
                    cus.AccountNo = "";
                }
                else if (Platform == "NMC")
                {
                    cusEx.NMCRefId = "";
                    cus.CustomerNo = "";
                    cus.AccountNo = "";
                    _Util.Facade.CustomerFacade.UpdateCustomerExtended(cusEx);
                }
                else if (Platform == "agmonitoring")
                {
                    cusEx.AvantgradRefId = "";
                    cus.CustomerNo = "";
                    cus.AccountNo = "";
                    _Util.Facade.CustomerFacade.UpdateCustomerExtended(cusEx);
                }
                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                message = "Unassociate Customer removed successfully";
                if (cus != null && CustomerGuidId != new Guid())
                {
                    base.AddUserActivityForCustomer("Unassociate Customer" + "#" + cus.Id + " removed by" + (CurrentUser.FirstName + " " + CurrentUser.LastName), LabelHelper.ActivityAction.Terminate, CustomerGuidId, null, null);

                }

                result = true;

            }
            catch (Exception ex)
            {
                result = false;
                message = "Customer not removed.";

            }
            return Json(new { result = result, message = message });
        }

        public List<Alarm.AlarmCustomer.PanelDevice> GetAlarmSystemInfoList(Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            List<Alarm.AlarmCustomer.PanelDevice> divicelist = new List<HS.Alarm.AlarmCustomer.PanelDevice>();
            if (cus != null && !string.IsNullOrEmpty(cus.AlarmRefId))
            {
                GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
                GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
                GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();

                HasMultipleAlarmUser = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "HasMultipleAlarmUser");
                if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
                {
                    if (cus != null && cus.Ownership == "Brinks")
                    {
                        AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserName");
                        AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserPassword");
                    }
                    else
                    {
                        AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                        AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
                    }
                }
                else
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
                }


                Alarm.AlarmCustomer.CustomerManagement response = new Alarm.AlarmCustomer.CustomerManagement()
                {
                    AuthenticationValue = new HS.Alarm.AlarmCustomer.Authentication()
                    {
                        User = AlarmUserGlobalSetting.Value,
                        Password = AlarmPassGlobalSetting.Value
                    }
                };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                try
                {
                    Alarm.AlarmCustomer.PanelDevice[] result = response.GetFullEquipmentList(Convert.ToInt32(cus.AlarmRefId));

                    if (result != null && result.ToList().Count > 0)
                    {
                        divicelist = result.ToList();

                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    return null;
                }
            }
            return divicelist;
        }
        [Authorize]
        public ActionResult AlarmCustomerDetails(int CustomerID, Guid CustomerLoadGuid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.CustomerId = CustomerID;
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerLoadGuid);
            GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
            GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();

            HasMultipleAlarmUser = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "HasMultipleAlarmUser");
            if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
            {
                if (cus != null && cus.Ownership == "Brinks")
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserName");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserPassword");
                }
                else
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
                }
            }
            else
            {
                AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
            }

            Alarm.AlarmCustomer.CustomerManagement response = new Alarm.AlarmCustomer.CustomerManagement()
            {
                AuthenticationValue = new HS.Alarm.AlarmCustomer.Authentication()
                {
                    User = AlarmUserGlobalSetting.Value,
                    Password = AlarmPassGlobalSetting.Value
                }
            };
            Alarm.AlarmCustomer.CustomerInfo result = new HS.Alarm.AlarmCustomer.CustomerInfo();
            Alarm.AlarmCustomer.CustomerBestPracticesOutput practiceResponse = new Alarm.AlarmCustomer.CustomerBestPracticesOutput();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                result = response.GetCustomerInfo(CustomerID);
                if (result != null)
                {
                    ViewBag.HasCustomer = "true";
                    if (result.CentralStationInfo != null)
                    {
                        if (result.IsTerminated != true)
                        {
                            cus.CustomerNo = result.CentralStationInfo.AccountNumber;
                            _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                        }

                    }

                }
                ViewBag.PracticeCount = 0;
                practiceResponse = response.GetCustomerBestPractices(CustomerID);
                if (practiceResponse.Success == true)
                {
                    ViewBag.PracticeCount = practiceResponse.CheckedCount;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ViewBag.HasCustomer = "false";
            }

            return View(result);
        }

        [Authorize]
        public ActionResult AlarmEquipmentPartial(int CustomerId)
        {
            ViewBag.CustomerId = CustomerId;
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return PartialView("_AlarmEquipmentPartial");
        }
        [Authorize]
        public ActionResult AlarmEquipmentList(int CustomerID)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            Customer cus = _Util.Facade.CustomerFacade.GetDirectCustomerByAlarmRefId(CustomerID.ToString());
            GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
            GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();

            HasMultipleAlarmUser = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "HasMultipleAlarmUser");
            if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
            {
                if (cus != null && cus.Ownership == "Brinks")
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserName");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserPassword");
                }
                else
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
                }
            }
            else
            {
                AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
            }

            Alarm.AlarmCustomer.CustomerManagement response = new Alarm.AlarmCustomer.CustomerManagement()
            {
                AuthenticationValue = new HS.Alarm.AlarmCustomer.Authentication()
                {
                    User = AlarmUserGlobalSetting.Value,
                    Password = AlarmPassGlobalSetting.Value
                }
            };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            List<Alarm.AlarmCustomer.PanelDevice> divicelist = new List<HS.Alarm.AlarmCustomer.PanelDevice>();
            try
            {
                Alarm.AlarmCustomer.PanelDevice[] result = response.GetFullEquipmentList(CustomerID);
                divicelist = result.ToList();
            }
            catch (Exception ex)
            {

            }

            return View(divicelist);
        }

        [Authorize]
        public ActionResult CustomerSystemInfoList(Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            List<Alarm.AlarmCustomer.PanelDevice> divicelist = new List<HS.Alarm.AlarmCustomer.PanelDevice>();

            divicelist = GetAlarmSystemInfoList(CustomerId);
            if (divicelist == null)
            {
                divicelist = new List<PanelDevice>();
            }
            return View(divicelist);
        }
        public ActionResult PackageInfo(int PackageId)
        {

            HS.Alarm.AlarmDealer.GetPackageIdsOutput packageInfo = (HS.Alarm.AlarmDealer.GetPackageIdsOutput)System.Web.HttpRuntime.Cache["GetPackageInfo"];

            return Json(new { result = packageInfo.Packages.Where(x => x.PackageId == PackageId) });
        }
        public ActionResult VideoPackageInfo(int PackageId)
        {

            HS.Alarm.AlarmDealer.GetPackageIdsOutput packageInfo = (HS.Alarm.AlarmDealer.GetPackageIdsOutput)System.Web.HttpRuntime.Cache["GetVideoPackageInfo"];

            return Json(new { result = packageInfo.Packages.Where(x => x.PackageId == PackageId) });
        }
        public ActionResult AutomationPackageInfo(int PackageId)
        {

            HS.Alarm.AlarmDealer.GetPackageIdsOutput packageInfo = (HS.Alarm.AlarmDealer.GetPackageIdsOutput)System.Web.HttpRuntime.Cache["GetEnergyAutomationPackageInfo"];

            return Json(new { result = packageInfo.Packages.Where(x => x.PackageId == PackageId) });
        }
        public ActionResult GetFeatures(int[] featureList, int?[] adonitem)
        {
            List<string> features = new List<string>();
            List<string> addonfeatures = new List<string>();
            foreach (var item in featureList)
            {
                features.Add(Enum.GetName(typeof(HS.Alarm.AlarmDealer.AddOnFeatureEnum), item));

            }

            if (adonitem != null)
            {
                foreach (var item in adonitem)
                {
                    addonfeatures.Add(Enum.GetName(typeof(HS.Alarm.AlarmDealer.AddOnFeatureEnum), item));
                }

            }
            return Json(new { result = features, addonfeatures = addonfeatures });
        }
        public ActionResult GetAllAddonsList()
        {
            List<AddOns> addonList = new List<AddOns>();
            var myEnumMemberCount = Enum.GetNames(typeof(HS.Alarm.AlarmDealer.AddOnFeatureEnum)).Length;
            HS.Alarm.AlarmDealer.AddOnFeatureEnum myEnum = (HS.Alarm.AlarmDealer.AddOnFeatureEnum)2;

            //HS.Alarm.AlarmDealer.AddOnFeatureEnum myEnum = (HS.Alarm.AlarmCustomer.AddOnFeatureEnum)Enum.Parse(typeof(HS.Alarm.AlarmCustomer.AddOnFeatureEnum), "VoiceNotificationsForAlarms");
            AlarmAddOnns addons = new AlarmAddOnns();
            List<AlarmAddOnns> addonsList = new List<AlarmAddOnns>();
            addonsList = _Util.Facade.AlarmFacade.GetAllAddOnns();
            if (addonsList.Count == 0)
            {
                for (int i = 0; i < myEnumMemberCount; i++)
                {
                    addons = new AlarmAddOnns
                    {
                        Name = Enum.GetName(typeof(HS.Alarm.AlarmDealer.AddOnFeatureEnum), i),
                        Value = i
                    };
                    _Util.Facade.AlarmFacade.InsertAlarmAddOnns(addons);
                }
                addonsList = _Util.Facade.AlarmFacade.GetAllAddOnns();
            }


            return View(addonsList.OrderBy(x => x.Name).ToList());

        }

        public JsonResult FreeAllUnassignedCsNumber(Guid CustomerId)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
            try
            {
                if (string.IsNullOrEmpty(cus.CustomerNo))
                {
                    List<CustomerSystemNo> cusNoLists = _Util.Facade.CustomerSystemNoFacade.GetAllReservedCustomerSystemNoByCustomerId(cus.Id);
                    foreach (var item in cusNoLists)
                    {
                        item.IsReserved = false;
                        item.CustomerId = 0;
                        _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(item);
                    }

                }
                result = true;
            }
            catch (Exception ex)
            {

            }

            return Json(new { result = result });
        }
        public JsonResult GetCsNumberForAlarm(Guid CustomerId, string Platform)
        {
            string CsNumber = "";
            string Prifix = "";
            string receiverNumber = "-1";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
            if (!string.IsNullOrEmpty(Platform) && Platform != "-1")
            {
                CsNumber = GetCSNumber(CustomerId, Platform);
                if (!string.IsNullOrEmpty(CsNumber))
                {

                    List<CustomerSystemNo> cusNoLists = _Util.Facade.CustomerSystemNoFacade.GetAllReservedCustomerSystemNoByCustomerId(cus.Id);
                    foreach (var item in cusNoLists)
                    {
                        item.IsReserved = false;
                        item.CustomerId = 0;
                        _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(item);
                    }
                    CustomerSystemNo cusno = new CustomerSystemNo();
                    cusno = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerNo(CsNumber);
                    if (cusno != null)
                    {
                        cusno.IsReserved = true;
                        cusno.ReserveDate = DateTime.Now;
                        cusno.CustomerId = cus.Id;
                        _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(cusno);
                    }

                    Prifix = CsNumber.Substring(0, CsNumber.Length - 4);

                    CustomerNoPrefix cusPrifix = _Util.Facade.CustomerSystemNoFacade.GetNumberPrefixByCompanyIdAndPrifix(CurrentUser.CompanyId.Value, Prifix).FirstOrDefault();
                    Lookup numberLookup = new Lookup();

                    numberLookup = _Util.Facade.LookupFacade.GetCsReceiverNumberLookupByKeyAndPrifix("CentralStationRecieverNumber", cusPrifix.Name).FirstOrDefault();

                    if (numberLookup != null)
                    {

                        receiverNumber = numberLookup.DataValue;
                    }
                }


            }


            return Json(new { CsNumber = CsNumber, ReceiverNumber = receiverNumber });
        }
        [Authorize]
        public ActionResult AddAlarmCustomer(Guid CustomerId, string Actions)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
            CustomerExtended cusExt = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(CustomerId);
            ViewBag.AbleToCreateAccount = false;

            if (cus.IsContractSigned == true || (cusExt != null && cusExt.ContractSentBy != null && cusExt.ContractSentBy != Guid.Empty))
            {
                ViewBag.AbleToCreateAccount = true;
            }
            GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
            GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();

            HasMultipleAlarmUser = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "HasMultipleAlarmUser");
            if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
            {
                if (cus != null && cus.Ownership == "Brinks")
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserName");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserPassword");
                }
                else
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
                }
            }
            else
            {
                AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
            }
            AlarmDealerManager ad = new AlarmDealerManager();
            HS.Alarm.AlarmDealer.GetPackageIdsOutput Basic = ad.GetBasicPackages(AlarmUserGlobalSetting.Value, AlarmPassGlobalSetting.Value);
            HS.Alarm.AlarmDealer.GetPackageIdsOutput Video = ad.GetVideoPackages(AlarmUserGlobalSetting.Value, AlarmPassGlobalSetting.Value);
            HS.Alarm.AlarmDealer.GetPackageIdsOutput Automation = ad.GetEnergyAutomationPackages(AlarmUserGlobalSetting.Value, AlarmPassGlobalSetting.Value);
            List<HS.Alarm.AlarmDealer.PackageInfo> PackageDataList = new List<HS.Alarm.AlarmDealer.PackageInfo>();
            PackageDataList.Add(new HS.Alarm.AlarmDealer.PackageInfo()
            {
                PackageDescription = "Select Feature",
                PackageId = -1
            });
            PackageDataList.AddRange(Basic.Packages);

            List<HS.Alarm.AlarmDealer.PackageInfo> VideoPackageDataList = new List<HS.Alarm.AlarmDealer.PackageInfo>();
            VideoPackageDataList.Add(new HS.Alarm.AlarmDealer.PackageInfo()
            {
                PackageDescription = "Select Feature",
                PackageId = -1
            });


            List<HS.Alarm.AlarmDealer.PackageInfo> AutomationPackageDataList = new List<HS.Alarm.AlarmDealer.PackageInfo>();
            AutomationPackageDataList.Add(new HS.Alarm.AlarmDealer.PackageInfo()
            {
                PackageDescription = "Select Feature",
                PackageId = -1
            });


            VideoPackageDataList.AddRange(Video.Packages);
            AutomationPackageDataList.AddRange(Automation.Packages);
            ViewBag.PhoneNo = "";
            EmergencyContact emContact = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerId(CustomerId).FirstOrDefault();
            if (emContact != null)
            {
                ViewBag.PhoneNo = emContact.Phone;
            }


            //DD.Packages.ToList().Add(new HS.Alarm.AlarmDealer.PackageInfo()
            //{
            //    PackageDescription = "Select Feature",
            //    PackageId = -1
            //});
            ViewBag.SelectPackageData = PackageDataList.OrderBy(x => x.PackageDescription != "Select Feature").ThenBy(x => x.PackageDescription).ToList().Select(x =>
                          new SelectListItem()
                          {
                              Text = x.PackageDescription.ToString(),
                              Value = x.PackageId.ToString()
                          }).ToList();

            ViewBag.VideoPackageDataList = VideoPackageDataList.OrderBy(x => x.PackageDescription != "Select Feature").ThenBy(x => x.PackageDescription).ToList().Select(x =>
                 new SelectListItem()
                 {
                     Text = x.PackageDescription.ToString(),
                     Value = x.PackageId.ToString()
                 }).ToList();

            ViewBag.AutomationPackageDataList = AutomationPackageDataList.OrderBy(x => x.PackageDescription != "Select Feature").ThenBy(x => x.PackageDescription).ToList().Select(x =>
                 new SelectListItem()
                 {
                     Text = x.PackageDescription.ToString(),
                     Value = x.PackageId.ToString()
                 }).ToList();





            System.Web.HttpRuntime.Cache["GetPackageInfo"] = Basic;
            System.Web.HttpRuntime.Cache["GetVideoPackageInfo"] = Video;
            System.Web.HttpRuntime.Cache["GetEnergyAutomationPackageInfo"] = Automation;
            //propertyList = (List<HotPropertyForHome>)System.Web.HttpRuntime.Cache["GetHotPropertyListForHomeMy"];




            SetupAlarm AlarmModel = _Util.Facade.AlarmFacade.GetSetupalarmByCustomerId(CustomerId);
            if (AlarmModel == null)
            {
                AlarmModel = new SetupAlarm();
                AlarmModel.CustomerStatus = LabelHelper.AlarmCustomerStatus.Init;
                AlarmModel.EmailAddress = cus.EmailAddress;
                AlarmModel.CentralStationAccountNo = cus.AccountNo;

                AlarmModel.InsStreet = cus.Street;
                AlarmModel.InsCity = cus.City;
                AlarmModel.InsState = cus.State;
                AlarmModel.InsZip = cus.ZipCode;
                AlarmModel.Phone = cus.CellNo;
                AlarmModel.CompanyName = CurrentUser.CompanyName;
                AlarmModel.LoginName = cus.EmailAddress;
                AlarmModel.CentralStationAccountNo = cus.CustomerNo;
                AlarmModel.CustomerType = cus.Type;
            }
            #region ModelAlways filldup from customer
            AlarmModel.Action = Actions;
            AlarmModel.FirstName = cus.FirstName;
            AlarmModel.LastName = cus.LastName;

            AlarmModel.Street = cus.Street;
            AlarmModel.City = cus.City;
            AlarmModel.State = cus.State;
            AlarmModel.Zip = cus.ZipCode;
            AlarmModel.AlarmRefId = cus.AlarmRefId;
            AlarmModel.CompanyName = CurrentUser.CompanyName;
            AlarmModel.Phone = cus.PrimaryPhone != "" ? cus.PrimaryPhone : cus.CellNo;
            #endregion
            AlarmModel.CustomerId = CustomerId;
            #region DropDown ViewBags
            ViewBag.CentralStationForwardingOption = _Util.Facade.LookupFacade.GetLookupByKey("CentralStationForwardingOption").OrderBy(x => x.DataValue).Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString(),
                            Selected = (AlarmModel.CentrastationForwardingOption == x.DataValue)
                        }).ToList();

            ViewBag.CentralStationRecieverNumber = _Util.Facade.LookupFacade.GetLookupByKey("CentralStationRecieverNumber").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
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
            ViewBag.PropertyTypeEnum = _Util.Facade.LookupFacade.GetLookupByKey("PropertyTypeEnum").OrderBy(x => x.DisplayText != "Property Type").ThenBy(x => x.DisplayText).Select(x =>
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
            ViewBag.StateList = _Util.Facade.LookupFacade.GetLookupByKey("StateList").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString(),
                               Selected = (AlarmModel.InsState == x.DataValue)
                           }).ToList();
            ViewBag.ExpectedNetwork = _Util.Facade.LookupFacade.GetLookupByKey("ExpectedNetwork").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString(),
                               Selected = (AlarmModel.InsState == x.DataValue)
                           }).ToList();
            ViewBag.AlarmPackages = _Util.Facade.LookupFacade.GetLookupByKey("AlarmPackages").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString(),
                               Selected = (AlarmModel.InsState == x.DataValue)
                           }).ToList();
            ViewBag.CentralStationName = _Util.Facade.LookupFacade.GetLookupByKey("CentralStationName").Select(x =>
                    new SelectListItem()
                    {
                        Text = x.DisplayText.ToString(),
                        Value = x.DataValue.ToString()
                    }).ToList();

            if (IsPermitted(UserPermissions.CustomerPermissions.CSNameSelected))
            {
                ViewBag.OwnerShip = cus.Ownership;
            }

            #endregion
            #region permission for CreateAccount
            if (IsPermitted(UserPermissions.CustomerPermissions.CreateAccountIfContractSigned))
            {
                ViewBag.CreateAccountIfContractSigned = "true";
            }
            AlarmModel.IsContractSigned = cus.IsContractSigned;
            #endregion
            return PartialView("_AddAlarmCustomer", AlarmModel);
        }
        [Authorize]
        [HttpPost]
        public JsonResult IntegrateToAlarm(SetupAlarm Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (Model.CustomerId == new Guid())
            {
                return Json(new { result = false, message = "Customer Id not found." });
            }
            AlarmCustomerManager cc = new AlarmCustomerManager();
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(Model.CustomerId);
            GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
            GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmSalesRepGlobalSetting = new GlobalSetting();
            HasMultipleAlarmUser = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "HasMultipleAlarmUser");
            if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
            {
                if (cus != null && cus.Ownership == "Brinks")
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserName");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserPassword");
                    AlarmSalesRepGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmSalesRepName");
                }
                else
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
                    AlarmSalesRepGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmSalesRepName");
                }
            }
            else
            {
                AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
                AlarmSalesRepGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmSalesRepName");
            }

            if (AlarmUserGlobalSetting == null || AlarmPassGlobalSetting == null || string.IsNullOrWhiteSpace(AlarmUserGlobalSetting.Value) || string.IsNullOrWhiteSpace(AlarmPassGlobalSetting.Value))
            {
                return Json(new { result = false, message = "alarm.com credentials are not configured." });
            }
            Model.AuthUser = AlarmUserGlobalSetting.Value;
            Model.AuthPass = AlarmPassGlobalSetting.Value;
            Model.CompanyId = CurrentUser.CompanyId.Value;
            if (AlarmSalesRepGlobalSetting != null)
            {
                Model.SalesRepName = AlarmSalesRepGlobalSetting.Value;
            }

            #region Check Cs Number
            CustomerSystemNo sysNo = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerNo(Model.CentralStationAccountNo);
            if (sysNo != null && sysNo.IsUsed == true)
            {
                Model.CentralStationAccountNo = GetCSNumber(cus.CustomerId, Model.CentralStationName);

            }
            #endregion
            //Model.AuthUser = "RABSecurityWebservices";
            //Model.AuthPass = "MaishaM@2";
            Alarm.AlarmCustomer.CreateCustomerOutput response = new HS.Alarm.AlarmCustomer.CreateCustomerOutput();

            if (Model.Action == LabelHelper.AlarmCustomerActions.CreateCustomer)
            {
                response = cc.CreateCustomer(Model);
                #region Update customer Packages
                CustomerExtended extended = new CustomerExtended();

                extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                if (response.Success)
                {
                    int AutomationCount = 0;
                    int videoCount = 0;
                    int skybellCount = 0;
                    int Svrcount = 0;
                    AlarmCustomerSelectedAddon addon = new AlarmCustomerSelectedAddon();
                    List<AlarmCustomerSelectedAddon> addons = new List<AlarmCustomerSelectedAddon>();
                    List<AlarmCustomerSelectedAddon> OldAddon = new List<AlarmCustomerSelectedAddon>();
                    OldAddon = _Util.Facade.CustomerFacade.GetAllCutomerAlarmAddonsByCustomerId(Model.CustomerId);
                    if (OldAddon != null && OldAddon.Count > 0)
                    {
                        foreach (var item in OldAddon)
                        {
                            _Util.Facade.CustomerFacade.DeleteCutomerAlarmAddons(item.Id);
                        }
                    }
                    if (extended != null)
                    {
                        extended.AlarmBasicPackage = "";
                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                    }
                    if (Model.adonitem != null && Model.adonitem.Count() > 0)
                    {
                        foreach (var item in Model.adonitem)
                        {
                            if (item == 36 || item == 41 || item == 2 || item == 5 || item == 23 || item == 21 || item == 19 || item == 20 || item == 52)
                            {
                                AutomationCount++;
                            }
                            else if (item == 76 || item == 70)
                            {
                                skybellCount++;
                            }
                            else if (item == 12 || item == 13)
                            {
                                videoCount++;
                            }
                            else if (item == 43)
                            {
                                Svrcount++;
                            }

                        }

                        if (AutomationCount == 1)
                        {
                            addon = new AlarmCustomerSelectedAddon();

                            addon.CustomerId = Model.CustomerId;
                            addon.AddonType = "Automation1";

                            _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                        }
                        if (AutomationCount == 2)
                        {

                            addon = new AlarmCustomerSelectedAddon();

                            addon.CustomerId = Model.CustomerId;
                            addon.AddonType = "Automation2";

                            _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                        }
                        if (AutomationCount == 3)
                        {
                            addon = new AlarmCustomerSelectedAddon();

                            addon.CustomerId = Model.CustomerId;
                            addon.AddonType = "Automation3";

                            _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                        }
                        if (AutomationCount > 3)
                        {
                            addon = new AlarmCustomerSelectedAddon();

                            addon.CustomerId = Model.CustomerId;
                            addon.AddonType = "Automation4";
                            _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                        }
                        if (Svrcount > 0)
                        {
                            addon = new AlarmCustomerSelectedAddon();

                            addon.CustomerId = Model.CustomerId;
                            addon.AddonType = "SVR";
                            _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                        }
                        if (skybellCount > 0 && videoCount > 0)
                        {
                            addon = new AlarmCustomerSelectedAddon();

                            addon.CustomerId = Model.CustomerId;
                            addon.AddonType = "ProVideo";
                            _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                        }
                        else
                        {
                            if (skybellCount > 0)
                            {
                                addon = new AlarmCustomerSelectedAddon();

                                addon.CustomerId = Model.CustomerId;
                                addon.AddonType = "SkyBell";
                                _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                            }
                            else if (videoCount > 0)
                            {

                                addon = new AlarmCustomerSelectedAddon();

                                addon.CustomerId = Model.CustomerId;
                                addon.AddonType = "ProVideo";
                                _Util.Facade.CustomerFacade.InsertCutomerAlarmAddons(addon);
                            }
                        }

                    }

                    if (Model.PackageId.HasValue)
                    {
                        AlarmCustomerSelectedAddon basicaddon = new AlarmCustomerSelectedAddon();
                        if (Model.PackageId == 208 || Model.PackageId == 209 || Model.PackageId == 193)
                        {

                            if (extended != null)
                            {
                                extended.AlarmBasicPackage = "Interactive";
                                _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                            }
                            else
                            {
                                extended.CustomerId = cus.CustomerId;
                                extended.AlarmBasicPackage = "Interactive";
                                _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
                            }
                        }
                    }

                    #region Update Customer System No
                    CustomerSystemNo cusno = new CustomerSystemNo();
                    cusno = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerNo(cus.CustomerNo);
                    if (cusno != null)
                    {
                        cusno.IsUsed = true;
                        cusno.CustomerId = cus.Id;
                        cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                        _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(cusno);
                    }
                    #endregion
                }
                #endregion

            }
            else if (Model.Action == LabelHelper.AlarmCustomerActions.CreateCommiment)
            {
                response = cc.CreateCommitment(Model);
            }

            if (response.Success)
            {
                if (Model.Action == LabelHelper.AlarmCustomerActions.CreateCommiment)
                {
                    Model.CustomerStatus = LabelHelper.AlarmCustomerStatus.Commited;
                }
                else if (Model.Action == LabelHelper.AlarmCustomerActions.CreateCustomer)
                {
                    Model.CustomerStatus = LabelHelper.AlarmCustomerStatus.Created;
                }
                else
                {
                    Model.CustomerStatus = LabelHelper.AlarmCustomerStatus.Init;
                }

                SetupAlarm temp = _Util.Facade.AlarmFacade.GetSetupalarmByCustomerId(Model.CustomerId);
                if (temp == null)
                {
                    Model.CreatedDate = DateTime.Now.ClientToUTCTime();
                    Model.CreatedBy = CurrentUser.UserId;
                    Model.LastUpdatedBy = CurrentUser.UserId;
                    Model.LastUpdatedDate = DateTime.Now.ClientToUTCTime();
                    Model.Id = _Util.Facade.AlarmFacade.InsertSetupAlarm(Model);
                    if (cus != null && Model.CustomerId != new Guid())
                    {
                        base.AddUserActivityForCustomer("Customer #" + cus.Id + " Added to alarm.com by " + (CurrentUser.FirstName + " " + CurrentUser.LastName), LabelHelper.ActivityAction.AddAlarm_com, Model.CustomerId, null, null);

                    }
                }
                else
                {

                    temp.LastUpdatedBy = CurrentUser.UserId;
                    temp.LastUpdatedDate = DateTime.Now.ClientToUTCTime();

                    _Util.Facade.AlarmFacade.UpdateSetupAlarm(temp);
                    if (cus != null && Model.CustomerId != new Guid())
                    {
                        base.AddUserActivityForCustomer("Customer #" + cus.Id + "Alarm is updated by " + (CurrentUser.FirstName + " " + CurrentUser.LastName), LabelHelper.ActivityAction.UpdateAlarm_com, Model.CustomerId, null, null);

                    }
                }

                Customer TempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.CustomerId);
                TempCustomer.AlarmRefId = response.CustomerId.ToString();
                _Util.Facade.CustomerFacade.UpdateCustomer(TempCustomer);

                string susccessMessage = string.Format("Customer added to alarm.com successfully. CustomerId: {0}", response.CustomerId);
                Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.CustomerId);
                if (customer != null)
                {

                    customer.AlarmRefId = response.CustomerId.ToString();
                    _Util.Facade.CustomerFacade.UpdateCustomer(customer);
                }
                #region Delete previous Security Zones
                List<Alarm.AlarmCustomer.PanelDevice> divicelist = new List<HS.Alarm.AlarmCustomer.PanelDevice>();
                divicelist = GetAlarmSystemInfoList(cus.CustomerId);
                List<CustomerSecurityZones> securityZoneList = _Util.Facade.CustomerFacade.GetAllCustomerSecurityZoneByCustomerId(cus.CustomerId, "'Brinks'");
                if (securityZoneList != null && securityZoneList.Count > 0)
                {
                    foreach (var item in securityZoneList)
                    {
                        _Util.Facade.CustomerFacade.DeleteCustomerSecurityZone(item.ID);
                    }
                }
                CustomerSecurityZones securityZones = new CustomerSecurityZones();
                foreach (var item in divicelist)
                {
                    securityZones = new CustomerSecurityZones()
                    {
                        CustomerId = cus.CustomerId,
                        ZoneNumber = item.DeviceId.ToString(),
                        ZoneComment = item.WebSiteDeviceName,
                        Platform = "Brinks"
                    };
                    _Util.Facade.CustomerFacade.InsertCustomerSecurityZone(securityZones);
                }
                #endregion
                return Json(new { result = response.Success, message = susccessMessage, CustomerId = response.CustomerId });
            }
            else
            {
                Model.CustomerStatus = LabelHelper.AlarmCustomerStatus.Init;
                return Json(new { result = response.Success, message = response.ErrorMessage });
            }
        }
        [Authorize]
        public JsonResult GetBestPracticeToAlarm()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            var AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
            var AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
            Alarm.AlarmCustomer.CustomerManagement response = new Alarm.AlarmCustomer.CustomerManagement()
            {
                AuthenticationValue = new HS.Alarm.AlarmCustomer.Authentication()
                {
                    User = AlarmUserGlobalSetting.Value,
                    Password = AlarmPassGlobalSetting.Value
                }
            };

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //Authentication authentication = new Authentication();
            //authentication.User = "evan@centralstationmarketing.com";
            //authentication.Password = "Dalworth123a";
            Alarm.AlarmCustomer.CustomerInfo result1 = response.GetCustomerInfo(7518350);
            Alarm.AlarmCustomer.CustomerBestPracticesOutput result = response.GetCustomerBestPractices(7518350);
            int[] result11 = response.GetCustomerList(true);
            return Json(new { result = result, message = result });
        }
        public JsonResult GetBestPracticeDataByCusId(int CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string result = "false";
            Customer cus = _Util.Facade.CustomerFacade.GetDirectCustomerByAlarmRefId(CustomerId.ToString());
            GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
            GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();

            HasMultipleAlarmUser = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "HasMultipleAlarmUser");
            if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
            {
                if (cus != null && cus.Ownership == "Brinks")
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserName");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserPassword");
                }
                else
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
                }
            }
            else
            {
                AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
            }
            Alarm.AlarmCustomer.CustomerManagement response = new Alarm.AlarmCustomer.CustomerManagement()
            {
                AuthenticationValue = new HS.Alarm.AlarmCustomer.Authentication()
                {
                    User = AlarmUserGlobalSetting.Value,
                    Password = AlarmPassGlobalSetting.Value
                }
            };
            Alarm.AlarmCustomer.CustomerBestPracticesOutput practiceResponse = new Alarm.AlarmCustomer.CustomerBestPracticesOutput();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                practiceResponse = response.GetCustomerBestPractices(CustomerId);
                if (practiceResponse.Success == true)
                {
                    result = "true";
                }

            }
            catch (Exception ex)
            {

            }
            return Json(new { result = result, EmailVerified = practiceResponse.EmailVerified, MobileContact = practiceResponse.MobileContact, ArmingRemainder = practiceResponse.ArmingReminder, GeoDevice = practiceResponse.GeoDevice, AdvanceDevice = practiceResponse.AdvancedDevice, RuleOrSchedule = practiceResponse.RuleOrSchedule });
        }

        public ActionResult CustomerTermination(int CustomerId, Guid CustomerLoadGuid)
        {

            CustomerInfo cusInfo = GetAlarmCustomerInfoByAlarmId(CustomerId);
            if (cusInfo == null)
            {
                cusInfo = new CustomerInfo();
            }
            ViewBag.CustomerGuid = CustomerLoadGuid;
            ViewBag.TrerminationReason = _Util.Facade.LookupFacade.GetLookupByKey("TerminationReason").OrderBy(x => x.DataValue).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString(),
            }).ToList();

            return View(cusInfo);
        }

        public CustomerInfo GetAlarmCustomerInfoByAlarmId(int alarmId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            Customer cus = _Util.Facade.CustomerFacade.GetDirectCustomerByAlarmRefId(alarmId.ToString());
            GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
            GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();

            HasMultipleAlarmUser = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "HasMultipleAlarmUser");
            if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
            {
                if (cus != null && cus.Ownership == "Brinks")
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserName");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserPassword");
                }
                else
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
                }
            }
            else
            {
                AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
            }
            Alarm.AlarmCustomer.CustomerManagement response = new Alarm.AlarmCustomer.CustomerManagement()
            {
                AuthenticationValue = new HS.Alarm.AlarmCustomer.Authentication()
                {
                    User = AlarmUserGlobalSetting.Value,
                    Password = AlarmPassGlobalSetting.Value
                }
            };
            Alarm.AlarmCustomer.CustomerInfo result = new HS.Alarm.AlarmCustomer.CustomerInfo();
            Alarm.AlarmCustomer.CustomerBestPracticesOutput practiceResponse = new Alarm.AlarmCustomer.CustomerBestPracticesOutput();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                result = response.GetCustomerInfo(alarmId);
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public JsonResult TerminateCustomer(int CustomerId, string Reason, Guid CustomerLoadGuid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            string message = "";
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerLoadGuid);
            GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
            GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();

            HasMultipleAlarmUser = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "HasMultipleAlarmUser");
            if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
            {
                if (cus != null && cus.Ownership == "Brinks")
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserName");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserPassword");
                }
                else
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
                }
            }
            else
            {
                AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
            }
            Alarm.AlarmCustomer.CustomerManagement response = new Alarm.AlarmCustomer.CustomerManagement()
            {
                AuthenticationValue = new HS.Alarm.AlarmCustomer.Authentication()
                {
                    User = AlarmUserGlobalSetting.Value,
                    Password = AlarmPassGlobalSetting.Value
                }
            };

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                result = response.TerminateCustomer(CustomerId, CustomerTerminateReasonEnum.NotUsingService);
                message = "Customer terminated successfully";
                base.AddUserActivityForCustomer("Customer" + "#" + CustomerId + "is terminated by" + (CurrentUser.FirstName + " " + CurrentUser.LastName), LabelHelper.ActivityAction.Terminate, CustomerLoadGuid, null, null);

                #region Insert into AlarmCustomerTermination
                CustomerInfo cusinfo = GetAlarmCustomerInfoByAlarmId(CustomerId);
                AlarmCustomerTermination cusTerm = new AlarmCustomerTermination()
                {
                    CustomerId = CustomerLoadGuid,
                    AlarmId = CustomerId,
                    TerminationReason = Reason,
                    TerminationDate = cusinfo.TermDate.Value,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CreatedBy = CurrentUser.UserId

                };
                _Util.Facade.CustomerFacade.InsertCustomerTermination(cusTerm);
                #endregion

                #region Reservation of Cs Number
                GlobalSetting RemoveCustomerNoGlobal = new GlobalSetting();


                RemoveCustomerNoGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "RemoveCustomerNoForTermination");
                if (RemoveCustomerNoGlobal != null && RemoveCustomerNoGlobal.Value.ToLower() == "true")
                {
                    CustomerSystemNo sysNo = _Util.Facade.CustomerSystemNoFacade.GetCustomerSystemNoObjectByNumberAndCompanyId(cus.CustomerNo, CurrentUser.CompanyId.Value);
                    if (sysNo != null)
                    {
                        sysNo.IsUsed = true;
                        sysNo.IsReserved = true;
                        _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(sysNo);
                    }
                    cus.CustomerNo = "";
                    _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                }

                #endregion

            }
            catch (Exception ex)
            {
                message = "Customer not terminated";
            }
            return Json(new { result = result, message = message });
        }

        public ActionResult TerminationHistoryLog(Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            List<AlarmCustomerTermination> logList = new List<AlarmCustomerTermination>();
            logList = _Util.Facade.CustomerFacade.GetAlarmTerminationHistoryByCustomerId(CustomerId);
            return View(logList);
        }
        public JsonResult RunSystemCheck(int CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string result = "false";
            string message = "";
            Customer cus = _Util.Facade.CustomerFacade.GetDirectCustomerByAlarmRefId(CustomerId.ToString());
            GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
            GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();

            HasMultipleAlarmUser = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "HasMultipleAlarmUser");
            if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
            {
                if (cus != null && cus.Ownership == "Brinks")
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserName");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserPassword");
                }
                else
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
                }
            }
            else
            {
                AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
            }

            Alarm.AlarmCustomer.CustomerManagement response = new Alarm.AlarmCustomer.CustomerManagement()
            {
                AuthenticationValue = new HS.Alarm.AlarmCustomer.Authentication()
                {
                    User = AlarmUserGlobalSetting.Value,
                    Password = AlarmPassGlobalSetting.Value
                }
            };

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                response.RunSystemCheck(CustomerId, SystemCheckTestCategoryEnum.Video);
                response.RunSystemCheck(CustomerId, SystemCheckTestCategoryEnum.ImageSensor);
                response.RunSystemCheck(CustomerId, SystemCheckTestCategoryEnum.Panel);
                response.RunSystemCheck(CustomerId, SystemCheckTestCategoryEnum.Zwave);
                response.RunSystemCheck(CustomerId, SystemCheckTestCategoryEnum.Engagement);
                response.RunSystemCheck(CustomerId, SystemCheckTestCategoryEnum.Communications);

                response.RunSystemCheck(CustomerId, SystemCheckTestCategoryEnum.AccessControl);
                response.RunSystemCheck(CustomerId, SystemCheckTestCategoryEnum.Sensors);
                result = "true";
                message = "System is runing system check";
            }
            catch (Exception ex)
            {

                message = "Running system check faild for some category";
            }

            return Json(new { result = result, message = message });
        }
        public ActionResult DoSystemRunTestByCusId(int CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            string result = "false";
            Customer cus = _Util.Facade.CustomerFacade.GetDirectCustomerByAlarmRefId(CustomerId.ToString());
            GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
            GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();

            HasMultipleAlarmUser = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "HasMultipleAlarmUser");
            if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
            {
                if (cus != null && cus.Ownership == "Brinks")
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserName");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "BrinksAlarmUserPassword");
                }
                else
                {
                    AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                    AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
                }
            }
            else
            {
                AlarmUserGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmUsername");
                AlarmPassGlobalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AlarmPassword");
            }
            Alarm.AlarmCustomer.CustomerManagement response = new Alarm.AlarmCustomer.CustomerManagement()
            {
                AuthenticationValue = new HS.Alarm.AlarmCustomer.Authentication()
                {
                    User = AlarmUserGlobalSetting.Value,
                    Password = AlarmPassGlobalSetting.Value
                }
            };
            Alarm.AlarmCustomer.GetSystemCheckResultsOutput checkResponse = new Alarm.AlarmCustomer.GetSystemCheckResultsOutput();
            GetSystemCheckResultsInput input = new GetSystemCheckResultsInput();
            input.CustomerId = CustomerId;
            List<SystemCheckCategoryOutput> systemCheckList = new List<SystemCheckCategoryOutput>();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ViewBag.ErrorMassege = "";
            try
            {
                checkResponse = response.GetSystemCheckResults(input);
                if (checkResponse.Success == true)
                {
                    SystemCheckCategoryOutput[] CheckCategoryList = checkResponse.SystemCheckResults.TestCategories;
                    systemCheckList = CheckCategoryList.ToList();
                    if (cus != null && cus.CustomerId != new Guid())
                    {
                        base.AddUserActivityForCustomer("Customer" + "#" + CustomerId + "is placed on test by " + (CurrentUser.FirstName + " " + CurrentUser.LastName), LabelHelper.ActivityAction.AddAlarm_com, cus.CustomerId, null, null);

                    }

                }
                else
                {
                    ViewBag.ErrorMassege = checkResponse.ErrorMessage;
                }

            }
            catch (Exception ex)
            {

            }

            return View(systemCheckList);
        }
        [Authorize]
        public JsonResult ValidateEmailAddress(string EmailAddress)
        {
            try
            {
                string Key = "test_a35eeaa09b8e4894b24ed712650dd6570cd0c98ea9d957ac64a61250497ce67b";
                string requestUrl = string.Format("https://api.kickbox.com/v2/verify?email={0}&apikey={1}", EmailAddress, Key);

                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(requestUrl);
                myReq.ContentType = "application/json";


                // here's how to set response content type:
                Response.ContentType = "application/json"; // that's all

                var response = (HttpWebResponse)myReq.GetResponse();
                /*
                 * PossibleStatusCodes
                 * 200 Success
                 * 400 Bad Request
                 * 403 Forbidden
                 * 429 Rate Limit Exceeded
                 * 500 Internal Server Error
                 */
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string text;
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        text = sr.ReadToEnd();
                    }
                    /*
                     Possible Results
                     --Deliverable
                     --Undeliverable
                     --Risky
                     --Unknown
                     */

                    KickBoxResponse kbResponse = new KickBoxResponse(text);


                    return Json(new { kbResponse = kbResponse, json = text }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(false);
            }
            return Json(new { result = true, message = "Validation successful." }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Calculate Payroll
        public void CalculatePayrollBrinks(Ticket ticket)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string Currency = HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency();
            if (ticket != null && ticket.TicketId != Guid.Empty && ticket.CustomerId != Guid.Empty)
            {
                var payrollBrinksDetail = _Util.Facade.PayrollFacade.GetPayrollBrinksByTicketId(ticket.TicketId);
                var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(ticket.CustomerId);
                if (CustomerDetails != null)
                {
                    var SalesPayCalculation = _Util.Facade.TicketFacade.GetSalesPayCalculationByTicketId(ticket.TicketId);
                    if (SalesPayCalculation != null)
                    {
                        var MonthlyMonitoringFee = 0.0;
                        double.TryParse(CustomerDetails.MonthlyMonitoringFee, out MonthlyMonitoringFee);
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
                                FundingStatus = "Pending"
                            };
                            _Util.Facade.PayrollFacade.InsertPayrollBrinks(model);
                        }
                    }
                }
            }
        }
        #endregion

        #region MACConnect
        public JsonResult SyncNMCCustomer(string NmcRefId, Guid CustomerGuidId)
        {
            #region Credentials
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var UserName = "";
            var Password = "";
            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCUserId");
            if (UserIdGlobal != null)
            {
                UserName = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion

            bool result = false;
            string message = "";
            Customer Cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerGuidId);
            var NMCInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCInProduction");
            bool InProduction = false;
            if (NMCInProductionGlobal != null && NMCInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            //HS.MACConnect.com.alarmaccount.www.NMCLinkPortal nmc = new MACConnect.com.alarmaccount.www.NMCLinkPortal(InProduction);
            HS.MACConnect.com.alarmaccount.www.NMCLinkPortal nmc = new MACConnect.com.alarmaccount.www.NMCLinkPortal();
            string RequestType = "G";
            string CsNumber = NmcRefId;
            string XMLData = @"<?xml version='1.0' encoding='utf-8'?><NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'><GetAccountInfo><GetAccountInfo_Request><data_element>GetAccountInfo</data_element></GetAccountInfo_Request></GetAccountInfo></NMCNexusDocument>";
            NMCNexusDocument SiteInfo = new NMCNexusDocument();
            try
            {
                var response = nmc.ProcessData(RequestType, UserName, Password, CsNumber, XMLData);
                SiteInfo = Deserialize<NMCNexusDocument>(response);
                if (!string.IsNullOrEmpty(SiteInfo.GetAccountInfo.GetAccountInfo_Response.Cs_no))
                {
                    result = true;
                    message = "Account synced successfully";
                    #region Insert on customer extended
                    Cus.CustomerNo = NmcRefId;
                    _Util.Facade.CustomerFacade.UpdateCustomer(Cus);
                    CustomerExtended extended = new CustomerExtended();

                    extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(Cus.CustomerId);
                    if (extended != null)
                    {
                        extended.NMCRefId = NmcRefId;

                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                    }
                    else
                    {
                        extended = new CustomerExtended();
                        extended.CustomerId = Cus.CustomerId;
                        extended.NMCRefId = NmcRefId;
                        _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);

                    }
                    #endregion
                    #region Update Customer System No
                    CustomerSystemNo cusno = new CustomerSystemNo();
                    cusno = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerNo(Cus.BrinksRefId);
                    if (cusno != null)
                    {
                        cusno.IsUsed = true;
                        cusno.CustomerId = Cus.Id;
                        cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                        _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(cusno);
                    }
                    #endregion
                }
                else
                {
                    result = false;
                    message = "Account does not exist";
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                result = false;
                message = "Account does not exist";
            }
            return Json(new { result = result, message = message });
        }
        public ActionResult NMCConnect(Guid CustomerId)
        {
            CustomerExtended cus = new CustomerExtended();
            cus = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(CustomerId);
            ViewBag.NmcRefId = "";
            if (cus != null)
            {
                ViewBag.NmcRefId = cus.NMCRefId;

            }

            return View(cus);
        }

        public T Deserialize<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }
        public ActionResult NMCCustomerDetails(string NMCRefId, Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ResultUcc model = new ResultUcc();
            ViewBag.NMCRefId = NMCRefId;

            #region Credentials

            var UserName = "";
            var Password = "";
            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCUserId");
            if (UserIdGlobal != null)
            {
                UserName = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion

            Customer Cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
            var NMCInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCInProduction");
            bool InProduction = false;
            if (NMCInProductionGlobal != null && NMCInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            HS.MACConnect.com.alarmaccount.www.NMCLinkPortal nmc = new MACConnect.com.alarmaccount.www.NMCLinkPortal();
            string RequestType = "G";
            string CsNumber = NMCRefId;
            string XMLData = @"<?xml version='1.0' encoding='utf-8'?><NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'><GetAccountInfo><GetAccountInfo_Request><data_element>GetAccountInfo</data_element></GetAccountInfo_Request></GetAccountInfo></NMCNexusDocument>";
            string ContactXMLData = @"<?xml version='1.0' encoding='utf-8'?>
                                    <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                                    <GetAccountContacts>
		                                    <GetAccountContacts_Request>
			                                    <data_element>GetAccountContacts</data_element>
		                                    </GetAccountContacts_Request>
	                                    </GetAccountContacts>
                                    </NMCNexusDocument>";
            string ZoneXMLData = @"<?xml version='1.0' encoding='utf-8'?>
                                                <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                                                <GetAccountZones>
		                                                <GetAccountZones_Request>
			                                                <data_element>GetAccountZones</data_element>
		                                                </GetAccountZones_Request>
	                                                </GetAccountZones>
                                                </NMCNexusDocument>";
            string AgencyXMLData = @"<?xml version='1.0' encoding='utf-8'?>
                                    <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                                    <GetAccountAgency>
		                                    <GetAccountAgency_Request>
			                                    <data_element>GetAccountAgency</data_element>
		                                    </GetAccountAgency_Request>
	                                    </GetAccountAgency>
                                    </NMCNexusDocument>";
            NMCNexusDocument SiteInfo = new NMCNexusDocument();

            NMCOverview Model = new NMCOverview();
            HS.Entities.Custom.NMCContactResponse.NMCNexusDocument contactInfo = new NMCContactResponse.NMCNexusDocument();
            HS.Entities.Custom.NMCAccountZoneResponse.NMCNexusDocument zoneInfo = new NMCAccountZoneResponse.NMCNexusDocument();
            HS.Entities.Custom.NMCAccAgencyResponse.NMCNexusDocument agencyInfo = new NMCAccAgencyResponse.NMCNexusDocument();
            try
            {
                var result = nmc.ProcessData(RequestType, UserName, Password, CsNumber, XMLData);

                SiteInfo = Deserialize<NMCNexusDocument>(result);

                Model.SiteInfo = SiteInfo.GetAccountInfo.GetAccountInfo_Response;

                #region Geting Contacts
                var contactresult = nmc.ProcessData(RequestType, UserName, Password, CsNumber, ContactXMLData);
                contactInfo = Deserialize<NMCContactResponse.NMCNexusDocument>(contactresult);
                Model.ContactList = contactInfo.GetAccountContacts;
                List<EmergencyContact> thirdPartyContactList = new List<EmergencyContact>();

                thirdPartyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerId(CustomerId);
                foreach (var contact in Model.ContactList.GetAccountContacts_Response)
                {
                    EmergencyContact itemContact = thirdPartyContactList.Where(x => x.FirstName == contact.First_name && x.LastName == contact.Last_name).FirstOrDefault();
                    if (itemContact != null)
                    {
                        itemContact.Platform = "NMC";
                        itemContact.ContactNo = contact.Contact_no;
                        _Util.Facade.EmergencyContactFacade.UpdateEmergencyContact(itemContact);
                    }
                    else
                    {
                        itemContact = new EmergencyContact();
                        itemContact.CustomerId = Cus.CustomerId;
                        itemContact.CompanyId = CurrentUser.CompanyId.Value;
                        itemContact.FirstName = contact.First_name;
                        itemContact.LastName = contact.Last_name;
                        itemContact.Phone = contact.Phone1;
                        itemContact.Email = contact.Email_address;
                        itemContact.ContactNo = contact.Contact_no;
                        itemContact.Platform = "NMC";
                        _Util.Facade.EmergencyContactFacade.InsertEmergencyContact(itemContact);
                    }
                }
                #endregion
                #region Geting Zones
                var ZoneResult = nmc.ProcessData(RequestType, UserName, Password, CsNumber, ZoneXMLData);
                zoneInfo = Deserialize<NMCAccountZoneResponse.NMCNexusDocument>(ZoneResult);
                Model.ZoneList = zoneInfo.GetAccountZones;
                List<CustomerSecurityZones> securityZoneList = new List<CustomerSecurityZones>();

                securityZoneList = _Util.Facade.CustomerFacade.GetAllCustomerSecurityZoneByCustomerId(Cus.CustomerId, "'NMC'");
                if (Model.ZoneList.GetAccountZones_Response.Count > 0 && Model.ZoneList.GetAccountZones_Response[0].Event_id != null)
                {
                    foreach (var zone in Model.ZoneList.GetAccountZones_Response)
                    {
                        CustomerSecurityZones secZone = securityZoneList.Where(x => x.ZoneNumber == zone.Zone_id.Replace(" ", "")).FirstOrDefault();
                        if (secZone == null)
                        {
                            secZone = new CustomerSecurityZones();
                            secZone.CustomerId = Cus.CustomerId;
                            secZone.ZoneNumber = zone.Zone_id.Replace(" ", "");
                            secZone.ZoneComment = zone.Comment;
                            secZone.Location = zone.Equiploc_id;
                            secZone.EquipmentType = zone.Equiptype_id;
                            secZone.Platform = "NMC";
                            secZone.EventCode = zone.Event_id;
                            _Util.Facade.CustomerFacade.InsertCustomerSecurityZone(secZone);
                        }



                    }
                }

                #endregion
                #region Geting Agecies
                var AgencyResult = nmc.ProcessData(RequestType, UserName, Password, CsNumber, AgencyXMLData);
                agencyInfo = Deserialize<NMCAccAgencyResponse.NMCNexusDocument>(AgencyResult);
                Model.AccAgencyList = agencyInfo.GetAccountAgency;
                List<CustomerThirdPartyAgency> agencyList = new List<CustomerThirdPartyAgency>();

                agencyList = _Util.Facade.CustomerFacade.GetAllCustomerThirdPartyAgencyByCustomerId(Cus.CustomerId, "'NMC'");
                if (Model.AccAgencyList.GetAccountAgency_Response.Count > 0 && Model.AccAgencyList.GetAccountAgency_Response[0].Agency_no != null)
                {
                    foreach (var itemAgency in Model.AccAgencyList.GetAccountAgency_Response)
                    {
                        CustomerThirdPartyAgency agency = agencyList.Where(x => x.AgencyNo == itemAgency.Agency_no.Replace(" ", "")).FirstOrDefault();
                        if (agency == null)
                        {
                            agency = new CustomerThirdPartyAgency();
                            agency.CustomerId = Cus.CustomerId;
                            agency.AgencyNo = itemAgency.Agency_no.Replace(" ", "");
                            agency.Agencytype = itemAgency.Agencytype_id.Replace(" ", "");
                            agency.AgencyName = itemAgency.Agency_name;
                            //agency.EffectiveDate = itemAgency.Effective_date;
                            agency.Phone = itemAgency.Phone1;
                            agency.PermitNo = itemAgency.Permit_no;
                            agency.PermType = itemAgency.Permtype_id;

                            agency.Platform = "NMC";

                            _Util.Facade.CustomerFacade.InsertCustomerAgency(agency);
                        }



                    }
                }

                #endregion
            }
            catch (Exception ex)
            {

            }

            return View(Model);

        }

        public ActionResult AddNmcCustomer(Guid CustomerId)
        {
            NMCNexusDocument accInfo = new NMCNexusDocument();
            CustomerExtended cusExt = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(CustomerId);
            Customer Cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
            if (cusExt != null && !string.IsNullOrEmpty(cusExt.NMCRefId))
            {
                #region Credentials
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                var UserName = "";
                var Password = "";
                var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCUserId");
                if (UserIdGlobal != null)
                {
                    UserName = UserIdGlobal.Value;
                }
                var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCPassword");
                if (PasswordGlobal != null)
                {
                    Password = PasswordGlobal.Value;
                }
                #endregion
                var NMCInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCInProduction");
                bool InProduction = false;
                if (NMCInProductionGlobal != null && NMCInProductionGlobal.Value.ToLower() == "true")
                {
                    InProduction = true;
                }
                else
                {
                    InProduction = false;
                }
                HS.MACConnect.com.alarmaccount.www.NMCLinkPortal nmc = new MACConnect.com.alarmaccount.www.NMCLinkPortal();
                string RequestType = "G";
                string CsNumber = cusExt.NMCRefId;
                string XMLData = @"<?xml version='1.0' encoding='utf-8'?><NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'><GetAccountInfo><GetAccountInfo_Request><data_element>GetAccountInfo</data_element></GetAccountInfo_Request></GetAccountInfo></NMCNexusDocument>";

                try
                {
                    var accresult = nmc.ProcessData(RequestType, UserName, Password, CsNumber, XMLData);

                    accInfo = Deserialize<NMCNexusDocument>(accresult);

                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                accInfo = new NMCNexusDocument();
                accInfo.GetAccountInfo = new GetAccountInfo();
                accInfo.GetAccountInfo.GetAccountInfo_Response = new GetAccountInfo_Response();
                accInfo.GetAccountInfo.GetAccountInfo_Response.Site_name = !string.IsNullOrEmpty(Cus.BusinessName) ? Cus.BusinessName : Cus.FirstName + ' ' + Cus.LastName;
                accInfo.GetAccountInfo.GetAccountInfo_Response.zip_code = Cus.ZipCode;
                accInfo.GetAccountInfo.GetAccountInfo_Response.City_name = Cus.City;
                accInfo.GetAccountInfo.GetAccountInfo_Response.State_id = Cus.State;
                accInfo.GetAccountInfo.GetAccountInfo_Response.Country_name = Cus.Country;
                accInfo.GetAccountInfo.GetAccountInfo_Response.phone1 = Cus.PrimaryPhone != null ? Cus.PrimaryPhone : Cus.CellNo;
                accInfo.GetAccountInfo.GetAccountInfo_Response.Site_addr1 = Cus.Street;
            }
            accInfo.GetAccountInfo.GetAccountInfo_Response.sitetype_id = Cus.Type;
            ViewBag.Relationship = _Util.Facade.LookupFacade.GetLookupByKey("Relationship").Select(x =>
                      new SelectListItem()
                      {
                          Text = x.DisplayText.ToString(),
                          Value = x.DataValue.ToString()
                      }).ToList();
            ViewBag.EquipTypeList = _Util.Facade.LookupFacade.GetLookupByKey("NMCEqpType").Select(x =>
                   new SelectListItem()
                   {
                       Text = x.DisplayText.ToString(),
                       Value = x.DataValue.ToString()
                   }).ToList();
            ViewBag.NMCEqpLocationList = _Util.Facade.LookupFacade.GetLookupByKey("NMCEqpLocation").Select(x =>
                 new SelectListItem()
                 {
                     Text = x.DisplayText.ToString(),
                     Value = x.DataValue.ToString()
                 }).ToList();
            ViewBag.CustomerType = _Util.Facade.LookupFacade.GetLookupByKey("CustomerType").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString()
               }).ToList();

            ViewBag.Relationship = _Util.Facade.LookupFacade.GetLookupByKey("NMCRelationship").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            List<SelectListItem> eventList = new List<SelectListItem>();
            eventList.Add(new SelectListItem()
            {
                Text = "Select Event Code",
                Value = "-1"
            });
            eventList.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("NMCEventCode").Select(x => new SelectListItem()
            {
                Text = x.DisplayText,
                Value = x.DataValue
            }).ToList());

            ViewBag.EventCodeList = eventList;
            return View(accInfo);
        }
        [HttpPost]
        public JsonResult AddNmcCustomer(SetUpNmc accInfo)
        {
            bool result = false;
            string message = "";
            #region Credentials
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var UserName = "";
            var Password = "";
            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCUserId");
            if (UserIdGlobal != null)
            {
                UserName = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion

            Customer Cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(accInfo.CustomerId);
            CustomerExtended extended = new CustomerExtended();

            extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(Cus.CustomerId);
            var NMCInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCInProduction");
            bool InProduction = false;
            if (NMCInProductionGlobal != null && NMCInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            HS.MACConnect.com.alarmaccount.www.NMCLinkPortal nmc = new MACConnect.com.alarmaccount.www.NMCLinkPortal();
            string RequestType = "";
            if (!string.IsNullOrEmpty(extended.NMCRefId))
            {
                RequestType = "C";
            }
            else
            {
                RequestType = "A";
            }

            string CsNumber = accInfo.CsNo;
            if (accInfo.Sitetype_id == "Residential")
            {
                accInfo.Sitetype_id = "R";
                accInfo.Systype_id = "1007AR";
            }
            else
            {
                accInfo.Sitetype_id = "C";
                accInfo.Systype_id = "1007AC";
            }
            accInfo.Sitestat_id = "A";
            accInfo.country_name = "USA";
            string XMLData = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
                            <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                            <SiteSystem>
		                            <SiteSystem_Request>
                                        <data_element>SiteSystem</data_element>
                                        <site_name>{0}</site_name>
                                        <site_addr1>{1}</site_addr1>
                                        <zip_code>{2}</zip_code>
                                        <city_name>{3}</city_name>
                                        <state_id>{4}</state_id>
                                        <country_name>{5}</country_name>
                                        <cspart_no>3</cspart_no>
                                        <install_servco_no>9982</install_servco_no>
                                        <servco_no>9982</servco_no>
                                     
                                        <phone1>{6}</phone1>
                                        <receiver_phone>{7}</receiver_phone>
                                        <panel_location>{8}</panel_location>
                                        <panel_phone>{9}</panel_phone>
                                        <sitetype_id>{10}</sitetype_id>
                                        <systype_id>{11}</systype_id>
                                        <sitestat_id>{12}</sitestat_id>
		                            </SiteSystem_Request>
	                            </SiteSystem>
                            </NMCNexusDocument>", accInfo.Site_name,
                            accInfo.Site_addr1,
                            accInfo.Zip_code,
                            accInfo.City_name,
                            accInfo.State_id,
                            accInfo.country_name,
                            accInfo.phone1,
                            accInfo.receiver_phone,
                            accInfo.panel_location,
                            accInfo.panel_phone,
                            accInfo.Sitetype_id,
                            accInfo.Systype_id,
                            accInfo.Sitestat_id);
            HS.Entities.Custom.NmcCreateResponse.NMCNexusDocument SiteInfo = new HS.Entities.Custom.NmcCreateResponse.NMCNexusDocument();
            try
            {
                var response = nmc.ProcessData(RequestType, UserName, Password, CsNumber, XMLData);
                SiteInfo = Deserialize<HS.Entities.Custom.NmcCreateResponse.NMCNexusDocument>(response);
                if (SiteInfo != null && SiteInfo.SiteSystem != null && SiteInfo.SiteSystem.SiteSystem_Response != null)
                {
                    if (SiteInfo.SiteSystem.SiteSystem_Response.Err_msg == "ok")
                    {
                        result = true;
                        if (RequestType == "A")
                        {
                            message = "Account is created successfully";
                        }
                        else
                        {
                            message = "Account is Updated successfully";
                        }

                        #region Insert on customer extended
                        Cus.CustomerNo = accInfo.CsNo;
                        _Util.Facade.CustomerFacade.UpdateCustomer(Cus);

                        if (extended != null)
                        {
                            extended.NMCRefId = accInfo.CsNo;

                            _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                        }
                        else
                        {
                            extended.CustomerId = Cus.CustomerId;
                            extended.NMCRefId = accInfo.CsNo;
                            _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
                        }
                        #endregion
                        #region Update Customer System No
                        CustomerSystemNo cusno = new CustomerSystemNo();
                        cusno = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerNo(extended.NMCRefId);
                        if (cusno != null)
                        {
                            cusno.IsUsed = true;
                            cusno.CustomerId = Cus.Id;
                            cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                            _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(cusno);
                        }
                        #endregion
                    }
                    else
                    {
                        result = false;
                        message = SiteInfo.SiteSystem.SiteSystem_Response.addl_err_info;
                        return Json(new { result = result, message = message });
                    }

                }
                else
                {
                    result = false;
                    message = "Accout is already created with this cs number";
                    return Json(new { result = result, message = message });
                }

            }
            catch (Exception ex)
            {
                result = false;
                message = "Internal Error!";
                return Json(new { result = result, message = message });
            }
            #region Add Contact
            try
            {
                List<EmergencyContact> thirdPartyContactList = new List<EmergencyContact>();
                NMCContactResponse.NMCNexusDocument contactInfo = new NMCContactResponse.NMCNexusDocument();
                thirdPartyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerId(accInfo.CustomerId);
                if (thirdPartyContactList != null && thirdPartyContactList.Count > 0)
                {
                    foreach (var item in thirdPartyContactList)
                    {
                        string ContactXML = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
                        <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                        <Contact>
		                        <Contact_Request>
			                        <data_element>Contact</data_element>
			                        <last_name>{0}</last_name>
			                        <first_name>{1}</first_name>
			                        <phone1>{2}</phone1>
			                        <phonetype_id1>H</phonetype_id1>
			                        <has_key_flag>Y</has_key_flag>
			                        <email_address>{4}</email_address>
                                    <relation_id>{5}</relation_id>
			                        <contact_no>{3}</contact_no>
		                        </Contact_Request>
	                        </Contact>
                        </NMCNexusDocument>", item.LastName, item.FirstName, item.Phone, !string.IsNullOrEmpty(item.ContactNo) ? item.ContactNo : "0", item.Email, item.RelationShip);
                        string ContactXMLData = @"<?xml version='1.0' encoding='utf-8'?>
                                    <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                                    <GetAccountContacts>
		                                    <GetAccountContacts_Request>
			                                    <data_element>GetAccountContacts</data_element>
		                                    </GetAccountContacts_Request>
	                                    </GetAccountContacts>
                                    </NMCNexusDocument>";
                        var contactresult = nmc.ProcessData("G", UserName, Password, CsNumber, ContactXMLData);
                        contactInfo = Deserialize<NMCContactResponse.NMCNexusDocument>(contactresult);
                        if (!string.IsNullOrEmpty(item.ContactNo) && item.Platform == "NMC")
                        {

                            if (contactInfo.GetAccountContacts != null && contactInfo.GetAccountContacts.GetAccountContacts_Response != null && contactInfo.GetAccountContacts.GetAccountContacts_Response.Count > 0)
                            {
                                if (contactInfo.GetAccountContacts.GetAccountContacts_Response.Where(x => x.Contact_no == item.ContactNo).FirstOrDefault() != null)
                                {
                                    RequestType = "C";
                                }

                            }

                        }
                        else
                        {
                            RequestType = "A";
                        }

                        var response = nmc.ProcessData(RequestType, UserName, Password, CsNumber, ContactXML);
                        //SiteInfo = Deserialize<HS.Entities.Custom.NmcCreateResponse.NMCNexusDocument>(response);
                    }

                }
                if (contactInfo.GetAccountContacts != null && contactInfo.GetAccountContacts.GetAccountContacts_Response != null && contactInfo.GetAccountContacts.GetAccountContacts_Response.Count > 0)
                {
                    DateTime PreviousDate = new DateTime();
                    PreviousDate = DateTime.Now.AddDays(-1);
                    foreach (var item in contactInfo.GetAccountContacts.GetAccountContacts_Response)
                    {
                        if (thirdPartyContactList.Where(x => x.FirstName == item.First_name && x.LastName == item.Last_name).FirstOrDefault() == null)
                        {
                            RequestType = "C";
                            string ContactXML = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
                            <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
                                <Contact>
                                    <Contact_Request>
                                        <data_element>Contact</data_element>
                                        <end_date>{0}</end_date>
                                        <last_name>{1}</last_name>
                                        <contact_no>{2}</contact_no>
                                    </Contact_Request>
                                </Contact>
                            </NMCNexusDocument>", PreviousDate.ToString("s"), item.Last_name, item.Contact_no);
                            var response = nmc.ProcessData(RequestType, UserName, Password, CsNumber, ContactXML);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message += "</br> Contact is not updated.";
            }
            #endregion

            #region Add Zones
            try
            {
                List<CustomerSecurityZones> cusZoneList = new List<CustomerSecurityZones>();
                cusZoneList = _Util.Facade.CustomerFacade.GetAllCustomerSecurityZoneByCustomerId(Cus.CustomerId, "'NMC'");
                NMCAccountZoneResponse.NMCNexusDocument zoneInfo = new NMCAccountZoneResponse.NMCNexusDocument();
                string ZoneXMLData = @"<?xml version='1.0' encoding='utf-8'?>
                                                <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                                                <GetAccountZones>
		                                                <GetAccountZones_Request>
			                                                <data_element>GetAccountZones</data_element>
		                                                </GetAccountZones_Request>
	                                                </GetAccountZones>
                                                </NMCNexusDocument>";
                var zoneresult = nmc.ProcessData("G", UserName, Password, CsNumber, ZoneXMLData);
                zoneInfo = Deserialize<NMCAccountZoneResponse.NMCNexusDocument>(zoneresult);
                if (cusZoneList != null && cusZoneList.Count > 0)
                {
                    foreach (var item in cusZoneList)
                    {
                        string ZoneXML = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
                        <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                        <ZoneRule>
		                        <ZoneRule_Request>
			                        <data_element>ZoneRule</data_element>
			                        <zone_id>{0}</zone_id>
			                        <zonestate_id>A</zonestate_id>
			                        <event_id>{1}</event_id>
			                        <equiploc_id>{2}</equiploc_id>
			                        <equiptype_id>{3}</equiptype_id>
			                        <zone_comment>{4}</zone_comment>
		                        </ZoneRule_Request>
	                        </ZoneRule>
                        </NMCNexusDocument>", item.ZoneNumber, item.EventCode, item.Location, item.EquipmentType, item.ZoneComment);


                        if (zoneInfo.GetAccountZones != null && zoneInfo.GetAccountZones.GetAccountZones_Response != null && zoneInfo.GetAccountZones.GetAccountZones_Response.Count > 0 && zoneInfo.GetAccountZones.GetAccountZones_Response[0].Zone_id != null)
                        {
                            if (zoneInfo.GetAccountZones.GetAccountZones_Response.Where(x => x.Zone_id.Replace(" ", "") == item.ZoneNumber).FirstOrDefault() != null)
                            {
                                RequestType = "C";
                            }

                        }
                        else
                        {
                            RequestType = "A";
                        }

                        var response = nmc.ProcessData(RequestType, UserName, Password, CsNumber, ZoneXML);
                        //SiteInfo = Deserialize<HS.Entities.Custom.NmcCreateResponse.NMCNexusDocument>(response);
                    }

                }
                if (zoneInfo.GetAccountZones != null && zoneInfo.GetAccountZones.GetAccountZones_Response != null && zoneInfo.GetAccountZones.GetAccountZones_Response.Count > 0 && zoneInfo.GetAccountZones.GetAccountZones_Response[0].Zone_id != null)
                {
                    foreach (var item in zoneInfo.GetAccountZones.GetAccountZones_Response)
                    {
                        if (cusZoneList.Find(x => x.ZoneNumber.ToString() == item.Zone_id.Replace(" ", "")) == null)
                        {
                            RequestType = "D";
                            string ContactXML = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
                        <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                        <ZoneRule>
		                        <ZoneRule_Request>
			                        <data_element>ZoneRule</data_element>
			                        <zone_id>{0}</zone_id>
		                        </ZoneRule_Request>
	                        </ZoneRule>
                        </NMCNexusDocument>", item.Zone_id);
                            var response = nmc.ProcessData(RequestType, UserName, Password, CsNumber, ContactXML);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message += "</br> Zone is not updated.";
            }
            #endregion

            #region Add Agencies
            try
            {
                List<CustomerThirdPartyAgency> cusAgencies = new List<CustomerThirdPartyAgency>();
                cusAgencies = _Util.Facade.CustomerFacade.GetAllCustomerThirdPartyAgencyByCustomerId(Cus.CustomerId, "'NMC'");
                NMCAccAgencyResponse.NMCNexusDocument agencyInfo = new NMCAccAgencyResponse.NMCNexusDocument();
                string AgencyXMLData = @"<?xml version='1.0' encoding='utf-8'?>
                                    <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                                    <GetAccountAgency>
		                                    <GetAccountAgency_Request>
			                                    <data_element>GetAccountAgency</data_element>
		                                    </GetAccountAgency_Request>
	                                    </GetAccountAgency>
                                    </NMCNexusDocument>";
                var agecnyresult = nmc.ProcessData("G", UserName, Password, CsNumber, AgencyXMLData);
                agencyInfo = Deserialize<NMCAccAgencyResponse.NMCNexusDocument>(agecnyresult);
                if (cusAgencies != null && cusAgencies.Count > 0)
                {
                    foreach (var item in cusAgencies)
                    {
                        string cusAgencyXML = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
                        <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                        <AgencyPermit>
		                        <AgencyPermit_Request>
			                        <data_element>AgencyPermit</data_element>
			                        <agency_no>{0}</agency_no>
			                        <agencytype_id>{1}</agencytype_id>
			                        <agency_name>{2}</agency_name>
			                        <phone1>{3}</phone1>
			                        <permit_no>1234</permit_no>
			                        <permtype_id>1</permtype_id>
			                        <effective_date>2008-01-01T08:00:00.0000000-08:00</effective_date>
			                        <expire_date>2008-01-01T08:00:00.0000000-08:00</expire_date>
	
		                        </AgencyPermit_Request>
	                        </AgencyPermit>
                        </NMCNexusDocument>", item.AgencyNo, item.Agencytype, item.AgencyName, item.Phone);


                        if (agencyInfo.GetAccountAgency != null && agencyInfo.GetAccountAgency.GetAccountAgency_Response != null && agencyInfo.GetAccountAgency.GetAccountAgency_Response.Count > 0 && agencyInfo.GetAccountAgency.GetAccountAgency_Response[0].Agency_no != null)
                        {
                            if (agencyInfo.GetAccountAgency.GetAccountAgency_Response.Where(x => x.Agency_no.Replace(" ", "") == item.AgencyNo).FirstOrDefault() != null)
                            {
                                RequestType = "C";
                            }

                        }
                        else
                        {
                            RequestType = "A";
                        }

                        var response = nmc.ProcessData(RequestType, UserName, Password, CsNumber, cusAgencyXML);
                        //SiteInfo = Deserialize<HS.Entities.Custom.NmcCreateResponse.NMCNexusDocument>(response);
                    }

                }
                if (agencyInfo.GetAccountAgency != null && agencyInfo.GetAccountAgency.GetAccountAgency_Response != null && agencyInfo.GetAccountAgency.GetAccountAgency_Response.Count > 0 && agencyInfo.GetAccountAgency.GetAccountAgency_Response[0].Agency_no != null)
                {
                    foreach (var item in agencyInfo.GetAccountAgency.GetAccountAgency_Response)
                    {
                        if (cusAgencies.Find(x => x.AgencyNo.ToString() == item.Agency_no.Replace(" ", "")) == null)
                        {
                            RequestType = "D";
                            string AgencyDeleteXML = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
                        <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                        <AgencyPermit>
		                        <AgencyPermit_Request>

			                        <data_element>AgencyPermit</data_element>
			                        <agency_no>{0}</agency_no>
			                        <agencytype_id>{1}</agencytype_id>
			
		                        </AgencyPermit_Request>
	                        </AgencyPermit>
                        </NMCNexusDocument>", item.Agency_no, item.Agencytype_id.Replace(" ", ""));
                            var response = nmc.ProcessData(RequestType, UserName, Password, CsNumber, AgencyDeleteXML);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message += "</br> Agency is not updated.";
            }
            #endregion
            return Json(new { result = result, message = message });
        }

        public void GetNmcEventCode()
        {
            #region Credentials
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var UserName = "";
            var Password = "";
            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCUserId");
            if (UserIdGlobal != null)
            {
                UserName = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion
            var RequestType = "G";
            var CsNumber = "RMRCL1234";
            var XmlData = @"<?xml version='1.0' encoding='utf-8'?>
                                <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                                <GetEventCode>
		                                <GetEventCode_Request>
			                                <data_element>GetEventCode</data_element>
		                                </GetEventCode_Request>
	                                </GetEventCode>
                                </NMCNexusDocument>";
            var NMCInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCInProduction");
            bool InProduction = false;
            if (NMCInProductionGlobal != null && NMCInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            HS.MACConnect.com.alarmaccount.www.NMCLinkPortal nmc = new MACConnect.com.alarmaccount.www.NMCLinkPortal();
            NMCEventCodeResponse.NMCNexusDocument response = new NMCEventCodeResponse.NMCNexusDocument();
            Lookup lk = new Lookup();
            try
            {
                var result = nmc.ProcessData(RequestType, UserName, Password, CsNumber, XmlData);

                response = Deserialize<NMCEventCodeResponse.NMCNexusDocument>(result);
                if (response.GetEventCode != null && response.GetEventCode.GetEventCode_Response != null && response.GetEventCode.GetEventCode_Response.Count > 0)
                {
                    foreach (var item in response.GetEventCode.GetEventCode_Response)
                    {
                        lk = new Lookup()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            DataKey = "NMCEventCode",
                            DisplayText = item.Descr,
                            DataValue = item.Event_id,
                            DataOrder = Convert.ToInt32(item.Priority)
                        };
                        _Util.Facade.LookupFacade.InsertLookup(lk);
                    }

                }
            }
            catch (Exception ex)
            {

            }


        }
        public void GetNmcAgencies()
        {
            #region Credentials
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var UserName = "";
            var Password = "";
            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCUserId");
            if (UserIdGlobal != null)
            {
                UserName = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion
            var RequestType = "G";
            var CsNumber = "RMRCL1234";
            var XmlData = @"<?xml version='1.0' encoding='utf-8'?>
                            <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                            <GetDispatchAgencies>
		                            <GetDispatchAgencies_Request>
			                            <data_element>GetDispatchAgencies</data_element>
			                            <change_date>2008-01-01T08:00:00.0000000-08:00</change_date>
		                            </GetDispatchAgencies_Request>
                                </GetDispatchAgencies>
                            </NMCNexusDocument>";
            var NMCInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCInProduction");
            bool InProduction = false;
            if (NMCInProductionGlobal != null && NMCInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            HS.MACConnect.com.alarmaccount.www.NMCLinkPortal nmc = new MACConnect.com.alarmaccount.www.NMCLinkPortal();
            NMCAgencyResponse.NMCNexusDocument response = new NMCAgencyResponse.NMCNexusDocument();
            ThirdPartyAgencies agencies = new ThirdPartyAgencies();
            try
            {
                var result = nmc.ProcessData(RequestType, UserName, Password, CsNumber, XmlData);

                response = Deserialize<NMCAgencyResponse.NMCNexusDocument>(result);
                if (response.GetDispatchAgencies != null && response.GetDispatchAgencies.GetDispatchAgencies_Response != null && response.GetDispatchAgencies.GetDispatchAgencies_Response.Count > 0)
                {
                    foreach (var item in response.GetDispatchAgencies.GetDispatchAgencies_Response)
                    {
                        DateTime ChangeDate = new DateTime();
                        DateTime.TryParse(item.Change_date, out ChangeDate);
                        agencies = new ThirdPartyAgencies()
                        {
                            AgencyName = item.Agency_name,
                            AgencyNo = item.Agency_no,
                            AgencyType = item.Agencytype_id.Replace(" ", ""),
                            City = item.City_name,
                            State = item.State_id,
                            Zipcode = item.zip_code,
                            Phone1 = item.Phone1,
                            Phone2 = item.Phone2,
                            ChangeDate = ChangeDate,
                            Platform = "NMC"

                        };
                        _Util.Facade.CustomerFacade.InsertThirdPartyAgencies(agencies);
                    }


                }
            }
            catch (Exception ex)
            {

            }


        }
        public JsonResult PlaceAccountOnTestNmc(NmcTestModel testmodel)
        {
            #region Credentials
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var UserName = "";
            var Password = "";
            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCUserId");
            if (UserIdGlobal != null)
            {
                UserName = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion
            var CsNumber = testmodel.CsNo;
            var RequestType = "G";
            bool result = false;
            string message = "";

            var XmlData = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
                            <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                            <Test>
		                            <Test_Request>
			                            <data_element>Test</data_element>
			                            <onoff_flag>on</onoff_flag>
			                            <testcat_id>1</testcat_id>
			                            <test_hours>{0}</test_hours>
			                            <test_minutes>{1}</test_minutes>
		                            </Test_Request>
	                            </Test>
                            </NMCNexusDocument>", testmodel.TestHour, testmodel.TestMinute);
            var NMCInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCInProduction");
            bool InProduction = false;
            if (NMCInProductionGlobal != null && NMCInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            HS.MACConnect.com.alarmaccount.www.NMCLinkPortal nmc = new MACConnect.com.alarmaccount.www.NMCLinkPortal();
            NMCTestAccountResponse.NMCNexusDocument response = new NMCTestAccountResponse.NMCNexusDocument();
            try
            {
                var testResult = nmc.ProcessData(RequestType, UserName, Password, CsNumber, XmlData);

                response = Deserialize<NMCTestAccountResponse.NMCNexusDocument>(testResult);
                if (response.Test.Test_Response.Err_msg == "ok")
                {
                    result = true;
                    message = "This account is placed on test";
                }
                else
                {
                    result = false;
                    message = response.Test.Test_Response.Err_msg;
                }
            }
            catch (Exception ex)
            {
                result = false;
                message = "Internal Error";
            }
            return Json(new { result = result, message = message });
        }
        public JsonResult PlaceNmcAccountOutOfService(string CsNo)
        {
            #region Credentials
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var UserName = "";
            var Password = "";
            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCUserId");
            if (UserIdGlobal != null)
            {
                UserName = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion
            var CsNumber = CsNo;
            var RequestType = "G";
            bool result = false;
            string message = "";

            var XmlData = string.Format(@"<?xml version='1.0' encoding='utf-8'?>
            <NMCNexusDocument xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	            <OOS>
		            <OOS_Request>
			            <data_element>OOS</data_element>
			            <status_flag>O</status_flag>
			            <ooscat_id>I</ooscat_id>
			            <oos_start_date>{0}</oos_start_date>
		            </OOS_Request>
	            </OOS>
            </NMCNexusDocument>", DateTime.Now.ToString("s"));
            var NMCInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCInProduction");
            bool InProduction = false;
            if (NMCInProductionGlobal != null && NMCInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            HS.MACConnect.com.alarmaccount.www.NMCLinkPortal nmc = new MACConnect.com.alarmaccount.www.NMCLinkPortal();
            NMCOOSResponse.NMCNexusDocument response = new NMCOOSResponse.NMCNexusDocument();
            try
            {
                var testResult = nmc.ProcessData(RequestType, UserName, Password, CsNumber, XmlData);

                response = Deserialize<NMCOOSResponse.NMCNexusDocument>(testResult);
                if (response.OOS.OOS_Response.Err_msg == "ok")
                {
                    result = true;
                    message = "This account is placed on out of service";
                }
                else
                {
                    result = false;
                    message = response.OOS.OOS_Response.Err_msg;
                }
            }
            catch (Exception ex)
            {
                result = false;
                message = "Internal Error";
            }
            return Json(new { result = result, message = message });
        }
        public ActionResult GetAccountSignalHistory(string CsNumber, DateTime StartDate, DateTime EndDate)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            #region Credentials

            var UserName = "";
            var Password = "";
            var UserIdGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCUserId");
            if (UserIdGlobal != null)
            {
                UserName = UserIdGlobal.Value;
            }
            var PasswordGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCPassword");
            if (PasswordGlobal != null)
            {
                Password = PasswordGlobal.Value;
            }
            #endregion

            var RequestType = "G";
            bool result = false;
            string message = "";

            var XmlData = string.Format(@"<?xml version='1.0' encoding='utf-8'?><NMCNexusDocument xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='http://www.nmccentral.com/webservices/nmcmapi'>
	                                        <SignalHistory>
	                                        <SignalHistory_Request>
			                                        <data_element>SignalHistory</data_element>
			                                        <start_date>{0}</start_date>
			                                        <end_date>{1}</end_date>
	                                        </SignalHistory_Request>
	                                        </SignalHistory>
                                        </NMCNexusDocument>", StartDate.ToString("s"), EndDate.ToString("s"));
            var NMCInProductionGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "NMCInProduction");
            bool InProduction = false;
            if (NMCInProductionGlobal != null && NMCInProductionGlobal.Value.ToLower() == "true")
            {
                InProduction = true;
            }
            else
            {
                InProduction = false;
            }
            HS.MACConnect.com.alarmaccount.www.NMCLinkPortal nmc = new MACConnect.com.alarmaccount.www.NMCLinkPortal();
            NMCSignalHIstoryResponse.NMCNexusDocument response = new NMCSignalHIstoryResponse.NMCNexusDocument();
            try
            {
                var testResult = nmc.ProcessData(RequestType, UserName, Password, CsNumber, XmlData);

                response = Deserialize<NMCSignalHIstoryResponse.NMCNexusDocument>(testResult);

            }
            catch (Exception ex)
            {
                result = false;
                message = "Internal Error";
            }
            return View(response.SignalHistory);
        }

        public JsonResult GetNmcThirdpartyAgencies(string ZipCode, Guid CustomerId)
        {

            bool result = false;
            string message = "";

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (!string.IsNullOrEmpty(ZipCode))
            {
                List<CustomerThirdPartyAgency> agenciyNmc = new List<CustomerThirdPartyAgency>();
                agenciyNmc = _Util.Facade.CustomerFacade.GetAllCustomerThirdPartyAgencyByCustomerId(CustomerId, "'NMC'");
                if (agenciyNmc != null)
                {
                    foreach (var agency in agenciyNmc)
                    {
                        _Util.Facade.CustomerFacade.DeleteCustomerThirdPartyAgency(agency.Id);
                    }
                }


                try
                {
                    var AgenciList = _Util.Facade.CustomerFacade.GetThirdpartyAgenciesByZipcode(ZipCode);

                    if (AgenciList != null && AgenciList.Count > 0)
                    {
                        foreach (var item in AgenciList)
                        {
                            CustomerThirdPartyAgency agency = new CustomerThirdPartyAgency();
                            agency.CustomerId = CustomerId;
                            agency.AgencyName = item.AgencyName;
                            agency.AgencyNo = item.AgencyNo;
                            agency.Agencytype = item.AgencyType;
                            agency.Phone = item.Phone1;
                            agency.Platform = "NMC";


                            _Util.Facade.CustomerFacade.InsertCustomerAgency(agency);
                            result = true;
                        }

                    }
                    else
                    {
                        result = false;
                        message = "No Agency found";
                    }
                }
                catch (Exception ex)
                {
                    message = "Internal Error!";
                }


            }
            else
            {
                result = false;
            }

            return Json(new { result = result, message = message });
        }
        #endregion

        #region Avantgrad

        [Authorize]
        public JsonResult SyncAvantgradCustomer(string AvantgradRefId, Guid CustomerGuidId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string message = "Successfully Sync";
            bool result = false;


            string UserId = "RMRCloudAPI";
            string Password = "TXLxSZZ2hOlQe";
            CustomerExtended cusExt = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(CustomerGuidId);
            try
            {
                HS.Avantguard.com.agmonitoring.devportal.SiteGroupGateway gateway = new Avantguard.com.agmonitoring.devportal.SiteGroupGateway();
                HS.Avantguard.com.agmonitoring.devportal.DataResultOfSite siteInfo = gateway.GetSite(UserId, Password, AvantgradRefId);
                if (siteInfo != null && siteInfo.Result1 != null)
                {

                    result = true;
                    cusExt.AvantgradRefId = AvantgradRefId;
                    if (cusExt != null)
                    {
                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(cusExt);
                    }
                    else
                    {
                        cusExt.CustomerId = CustomerGuidId;
                        _Util.Facade.CustomerFacade.InsertCustomerExtended(cusExt);
                    }

                }
                else
                {
                    result = false;
                    message = "Site not found";
                }

            }
            catch (Exception ex)
            {
                result = false;
                message = "Internal Error!";
            }



            return Json(new { result = result, message = message });
        }

        public ActionResult Avantgrad(Guid CustomerId)
        {
            CustomerExtended cus = new CustomerExtended();
            cus = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(CustomerId);
            ViewBag.AvantgradRefId = "";
            if (cus != null)
            {
                ViewBag.AvantgradRefId = cus.AvantgradRefId;

            }

            return View();
        }

        public ActionResult AvantgradCustomerDetails(string AvantgradRefId, Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string UserId = "RMRCloudAPI";
            string Password = "TXLxSZZ2hOlQe";
            ViewBag.AgRefId = AvantgradRefId;
            HS.Avantguard.com.agmonitoring.devportal.DataResultOfSite siteInfo = new Avantguard.com.agmonitoring.devportal.DataResultOfSite();
            HS.Avantguard.com.agmonitoring.devportal.DataResultOfSitePlus siteAdditionalInfo = new Avantguard.com.agmonitoring.devportal.DataResultOfSitePlus();
            try
            {
                HS.Avantguard.com.agmonitoring.devportal.SiteGroupGateway gateway = new Avantguard.com.agmonitoring.devportal.SiteGroupGateway();
                siteInfo = new Avantguard.com.agmonitoring.devportal.DataResultOfSite();
                siteInfo = gateway.GetSite(UserId, Password, AvantgradRefId);
                siteAdditionalInfo = gateway.GetSiteAdditionalInfo(UserId, Password, AvantgradRefId);
                if (siteAdditionalInfo != null && siteAdditionalInfo.Result1 != null)
                {
                    if (siteAdditionalInfo.Result1.Devices != null && siteAdditionalInfo.Result1.Devices.Count() > 0)
                    {
                        ViewBag.OOSCategory = siteAdditionalInfo.Result1.Devices[0].OOSCatDescription;
                    }
                }
                #region Geting Contacts
                if (siteInfo.Result1.Contacts != null && siteInfo.Result1.Contacts.Count() > 0)
                {
                    List<EmergencyContact> thirdPartyContactList = new List<EmergencyContact>();

                    thirdPartyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerId(CustomerId);
                    foreach (var contact in siteInfo.Result1.Contacts)
                    {
                        EmergencyContact itemContact = thirdPartyContactList.Where(x => x.FirstName == contact.FirstName && x.LastName == contact.LastName).FirstOrDefault();
                        if (itemContact != null)
                        {
                            itemContact.Platform = "AG";

                            _Util.Facade.EmergencyContactFacade.UpdateEmergencyContact(itemContact);
                        }
                        else
                        {

                            itemContact = new EmergencyContact();
                            itemContact.CustomerId = CustomerId;
                            itemContact.CompanyId = CurrentUser.CompanyId.Value;
                            itemContact.FirstName = contact.FirstName;
                            itemContact.LastName = contact.LastName;
                            if (contact.Phones != null && contact.Phones.Count() > 0)
                            {
                                itemContact.Phone = contact.Phones[0].PhoneNumber;
                            }
                            if (contact.EmailAddresses != null && contact.EmailAddresses.Count() > 0)
                            {
                                itemContact.Email = contact.EmailAddresses[0].EmailAddress1;
                            }

                            itemContact.Platform = "AG";
                            _Util.Facade.EmergencyContactFacade.InsertEmergencyContact(itemContact);
                        }
                    }
                }

                #endregion

                #region Geting Agecies

                List<CustomerThirdPartyAgency> agencyList = new List<CustomerThirdPartyAgency>();

                agencyList = _Util.Facade.CustomerFacade.GetAllCustomerThirdPartyAgencyByCustomerId(CustomerId, "'AG'");
                if (siteInfo.Result1.SiteAgencies != null && siteInfo.Result1.SiteAgencies.Count() > 0)
                {
                    foreach (var itemAgency in siteInfo.Result1.SiteAgencies)
                    {
                        CustomerThirdPartyAgency agency = agencyList.Where(x => x.AgencyNo == itemAgency.AgencyNum.ToString()).FirstOrDefault();
                        if (agency == null)
                        {
                            agency = new CustomerThirdPartyAgency();
                            agency.CustomerId = CustomerId;
                            agency.AgencyNo = itemAgency.AgencyNum.ToString();
                            agency.Agencytype = itemAgency.AgencyType;
                            agency.AgencyName = itemAgency.AgencyName;
                            //agency.EffectiveDate = itemAgency.Effective_date;
                            agency.Phone = itemAgency.AgencyPhone;

                            agency.PermType = itemAgency.Permit;

                            agency.Platform = "AG";

                            _Util.Facade.CustomerFacade.InsertCustomerAgency(agency);
                        }



                    }
                }

                #endregion

                ViewBag.AGTestCategoryList = _Util.Facade.LookupFacade.GetLookupByKey("AgTestCategory").OrderBy(x => x.DataValue).Select(x =>
                  new SelectListItem()
                  {
                      Text = x.DisplayText.ToString(),
                      Value = x.DataValue.ToString()
                  }).ToList();

            }
            catch (Exception ex)
            {

            }
            return View(siteInfo);
        }

        [HttpGet]
        public ActionResult AddAvantgradCustomer(Guid CustomerId)
        {
            HS.Avantguard.com.agmonitoring.devportal.DataResultOfSite siteInfo = new Avantguard.com.agmonitoring.devportal.DataResultOfSite();
            CustomerExtended cusExt = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(CustomerId);
            Customer Cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
            if (cusExt != null && !string.IsNullOrEmpty(cusExt.AvantgradRefId))
            {
                try
                {
                    string UserId = "RMRCloudAPI";
                    string Password = "TXLxSZZ2hOlQe";
                    HS.Avantguard.com.agmonitoring.devportal.SiteGroupGateway gateway = new Avantguard.com.agmonitoring.devportal.SiteGroupGateway();
                    siteInfo = new Avantguard.com.agmonitoring.devportal.DataResultOfSite();
                    siteInfo = gateway.GetSite(UserId, Password, cusExt.AvantgradRefId);
                    if (!string.IsNullOrEmpty(siteInfo.Result1.SiteType))
                    {
                        if (siteInfo.Result1.SiteType == "R")
                        {
                            siteInfo.Result1.SiteType = "Residential";

                        }
                        else
                        {
                            siteInfo.Result1.SiteType = "Commercial";
                        }
                    }
                    else
                    {
                        siteInfo.Result1.SiteType = "-1";
                    }
                    List<string> typedispatch = new List<string>();
                    List<string> typedevice = new List<string>();
                    if (siteInfo.Result1.DispatchTypes != null)
                    {
                        var dis = siteInfo.Result1.DispatchTypes;
                        if (siteInfo.Result1.DispatchTypes.Count() > 0)
                        {
                            foreach (var item in dis)
                            {
                                typedispatch.Add(item.DispatchType1);
                            }
                        }
                        ViewBag.typedispatch = typedispatch;
                    }

                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                siteInfo.Result1 = new Avantguard.com.agmonitoring.devportal.Site();
                siteInfo.Result1.SiteName = Cus.BusinessName != "" ? Cus.BusinessName : Cus.FirstName + " " + Cus.LastName;
                siteInfo.Result1.City = Cus.City;
                siteInfo.Result1.State = Cus.State;
                siteInfo.Result1.ZipCode = Cus.ZipCode;
                siteInfo.Result1.SiteAddr2 = Cus.Street;
                List<HS.Avantguard.com.agmonitoring.devportal.Codeword> CodewordArray = new List<HS.Avantguard.com.agmonitoring.devportal.Codeword>();
                CodewordArray.Add(new HS.Avantguard.com.agmonitoring.devportal.Codeword()
                {
                    Codeword1 = Cus.Passcode

                });
                List<HS.Avantguard.com.agmonitoring.devportal.Phone> PhoneArray = new List<HS.Avantguard.com.agmonitoring.devportal.Phone>();
                PhoneArray.Add(new HS.Avantguard.com.agmonitoring.devportal.Phone()
                {
                    PhoneNumber = Cus.PrimaryPhone
                });
                siteInfo.Result1.Phones = PhoneArray.ToArray();
                siteInfo.Result1.Codewords = CodewordArray.ToArray();
                siteInfo.Result1.SiteType = Cus.CustomerType;
            }
            ViewBag.CustomerType = _Util.Facade.LookupFacade.GetLookupByKey("CustomerType").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.AGDispatchType = _Util.Facade.LookupFacade.GetLookupByKey("AGDispatchType").OrderBy(x => x.DataValue).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.UccDeviceType = _Util.Facade.LookupFacade.GetLookupByKey("UccDeviceType").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            ViewBag.AGAgencyTypeList = _Util.Facade.LookupFacade.GetLookupByKey("AGAgencyType").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            ViewBag.Relationship = _Util.Facade.LookupFacade.GetLookupByKey("Relationship").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();

            ViewBag.SignalStatusList = _Util.Facade.LookupFacade.GetLookupByKey("AgSignalStatus").Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();
            ViewBag.EventCodeList = _Util.Facade.LookupFacade.GetLookupByKey("AgEventCode").Select(x =>
              new SelectListItem()
              {
                  Text = x.DisplayText.ToString(),
                  Value = x.DataValue.ToString()
              }).ToList();
            return View(siteInfo);
        }
        [HttpPost]
        public JsonResult AddAvantgradCustomer(HS.Avantguard.com.agmonitoring.devportal.Site model, Guid CustomerId, string TransmitterCode)
        {
            bool result = false;
            string message = "";


            Customer Cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(CustomerId);
            CustomerExtended extended = new CustomerExtended();

            extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(Cus.CustomerId);

            string UserId = "RMRCloudAPI";
            string Password = "TXLxSZZ2hOlQe";
            HS.Avantguard.com.agmonitoring.devportal.SiteGroupGateway gateway = new Avantguard.com.agmonitoring.devportal.SiteGroupGateway();



            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);



            List<HS.Avantguard.com.agmonitoring.devportal.Phone> phoneList = new List<HS.Avantguard.com.agmonitoring.devportal.Phone>();
            phoneList.Add(new HS.Avantguard.com.agmonitoring.devportal.Phone()
            {
                PhoneNumber = model.Phones[0].PhoneNumber
            });


            List<HS.Avantguard.com.agmonitoring.devportal.Codeword> cdList = new List<HS.Avantguard.com.agmonitoring.devportal.Codeword>();
            cdList.Add(new HS.Avantguard.com.agmonitoring.devportal.Codeword()
            {
                Codeword1 = model.Codewords[0].Codeword1
            });


            #region Add Zones
            List<CustomerSecurityZones> securityList = new List<CustomerSecurityZones>();
            List<HS.Avantguard.com.agmonitoring.devportal.Point> agZoneList = new List<Avantguard.com.agmonitoring.devportal.Point>();

            securityList = _Util.Facade.CustomerFacade.GetAllCustomerSecurityZoneByCustomerId(CustomerId, "'AG'");
            if (securityList != null && securityList.Count() > 0)
            {

                foreach (var item in securityList)
                {
                    HS.Avantguard.com.agmonitoring.devportal.Point point = new Avantguard.com.agmonitoring.devportal.Point();


                    point.Point1 = item.ZoneNumber;
                    point.EventCode = item.EventCode;
                    point.SignalStatus = item.EquipmentType;
                    point.Description = item.ZoneComment;

                    agZoneList.Add(point);
                }

            }
            #endregion
            List<HS.Avantguard.com.agmonitoring.devportal.Device> deviceList = new List<HS.Avantguard.com.agmonitoring.devportal.Device>();
            deviceList.Add(new HS.Avantguard.com.agmonitoring.devportal.Device()
            {
                //TransmitterCode = model.TransmitterCode,
                TransmitterCode = model.Devices[0].TransmitterCode,
                DeviceType = "2W1",
                //ReceiverPhone = model.ReceiverPhone,
                ReceiverPhone = model.Devices[0].ReceiverPhone,
                //PanelLocation = model.PanelLocation,
                PanelLocation = model.Devices[0].PanelLocation,
                //PanelPhone = model.PanelPhone,
                PanelPhone = model.Devices[0].PanelPhone,
                Points = agZoneList.ToArray()
            });

            List<HS.Avantguard.com.agmonitoring.devportal.DispatchType> dispatchTypesList = new List<HS.Avantguard.com.agmonitoring.devportal.DispatchType>();
            //if (model.DispatchTypesList != null)
            if (model.DispatchTypes != null)
            {
                foreach (var item in model.DispatchTypes)
                {
                    dispatchTypesList.Add(new HS.Avantguard.com.agmonitoring.devportal.DispatchType()
                    {
                        DispatchType1 = item.DispatchType1
                    });
                }
            }

            string SiteType = "";
            if (cus.Type != "-1" && cus.Type != null)
            {
                if (cus.Type == "Residential")
                {
                    SiteType = "R";
                }
                else
                {
                    SiteType = "C";
                }
            }
            HS.Avantguard.com.agmonitoring.devportal.DataResultOfSite siteInfo = new Avantguard.com.agmonitoring.devportal.DataResultOfSite();

            siteInfo = gateway.GetSite(UserId, Password, model.Devices[0].TransmitterCode);
            if (siteInfo.ErrorNum == 204)
            {
                siteInfo.Result1 = new Avantguard.com.agmonitoring.devportal.Site();
            }
            siteInfo.Result1.Phones = phoneList.ToArray();
            siteInfo.Result1.Codewords = cdList.ToArray();
            siteInfo.Result1.Devices = deviceList.ToArray();
            siteInfo.Result1.DispatchTypes = dispatchTypesList.ToArray();
            siteInfo.Result1.SiteType = SiteType;
            siteInfo.Result1.City = model.City;
            siteInfo.Result1.State = model.State;
            siteInfo.Result1.ZipCode = model.ZipCode;
            siteInfo.Result1.SiteAddress = model.SiteAddress;
            siteInfo.Result1.SiteName = model.SiteName;
            #region Add Contact

            List<EmergencyContact> thirdPartyContactList = new List<EmergencyContact>();
            List<HS.Avantguard.com.agmonitoring.devportal.Contact> agContactList = new List<Avantguard.com.agmonitoring.devportal.Contact>();
            thirdPartyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerIdandPlatform(cus.CustomerId, "AG");

            if (thirdPartyContactList != null && thirdPartyContactList.Count() > 0)
            {

                foreach (var item in thirdPartyContactList)
                {
                    HS.Avantguard.com.agmonitoring.devportal.Contact contact = new Avantguard.com.agmonitoring.devportal.Contact();
                    List<HS.Avantguard.com.agmonitoring.devportal.EmailAddress> emailArray = new List<HS.Avantguard.com.agmonitoring.devportal.EmailAddress>();
                    emailArray.Add(new HS.Avantguard.com.agmonitoring.devportal.EmailAddress()
                    {
                        EmailAddress1 = item.Email,
                        AutoNotifyFlag = false

                    });
                    List<HS.Avantguard.com.agmonitoring.devportal.Phone> PhoneArray = new List<HS.Avantguard.com.agmonitoring.devportal.Phone>();
                    PhoneArray.Add(new HS.Avantguard.com.agmonitoring.devportal.Phone()
                    {
                        PhoneNumber = item.Phone
                    });
                    contact.EmailAddresses = emailArray.ToArray();
                    contact.FirstName = item.FirstName;
                    contact.LastName = item.LastName;
                    contact.Phones = PhoneArray.ToArray();
                    agContactList.Add(contact);
                }
                siteInfo.Result1.Contacts = agContactList.ToArray();
            }
            #endregion

            #region Add Agencies
            List<CustomerThirdPartyAgency> thirdPartyAgencyList = new List<CustomerThirdPartyAgency>();
            List<HS.Avantguard.com.agmonitoring.devportal.SiteAgency> agAgencyList = new List<Avantguard.com.agmonitoring.devportal.SiteAgency>();

            thirdPartyAgencyList = _Util.Facade.CustomerFacade.GetAllCustomerThirdPartyAgencyByCustomerId(CustomerId, "'AG'");
            if (thirdPartyAgencyList != null && thirdPartyAgencyList.Count() > 0)
            {

                foreach (var item in thirdPartyAgencyList)
                {
                    HS.Avantguard.com.agmonitoring.devportal.SiteAgency agency = new Avantguard.com.agmonitoring.devportal.SiteAgency();
                    List<HS.Avantguard.com.agmonitoring.devportal.EmailAddress> emailArray = new List<HS.Avantguard.com.agmonitoring.devportal.EmailAddress>();
                    //emailArray.Add(new HS.Avantguard.com.agmonitoring.devportal.EmailAddress()
                    //{
                    //    EmailAddress1 = item.Email,
                    //    AutoNotifyFlag = false

                    //});
                    //List<HS.Avantguard.com.agmonitoring.devportal.Phone> PhoneArray = new List<HS.Avantguard.com.agmonitoring.devportal.Phone>();
                    //PhoneArray.Add(new HS.Avantguard.com.agmonitoring.devportal.Phone()
                    //{
                    //    PhoneNumber = item.Phone
                    //});
                    agency.AgencyName = item.AgencyName;
                    agency.AgencyType = item.Agencytype;
                    agency.Permit = item.PermType;
                    agency.AgencyNum = Convert.ToInt32(item.AgencyNo);
                    agency.AgencyPhone = item.Phone;
                    agAgencyList.Add(agency);
                }
                siteInfo.Result1.Contacts = agContactList.ToArray();
                siteInfo.Result1.SiteAgencies = agAgencyList.ToArray();
            }
            #endregion


            try
            {
                HS.Avantguard.com.agmonitoring.devportal.Result response = gateway.ImportSite(UserId, Password, siteInfo.Result1, null);
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    result = true;
                    message = "Account updated successfully.";
                    #region Insert on customer extended
                    Cus.CustomerNo = model.Devices[0].TransmitterCode;
                    _Util.Facade.CustomerFacade.UpdateCustomer(Cus);

                    if (extended != null)
                    {
                        extended.AvantgradRefId = model.Devices[0].TransmitterCode;

                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                    }
                    else
                    {
                        extended.CustomerId = cus.CustomerId;
                        extended.AvantgradRefId = model.Devices[0].TransmitterCode;
                        _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
                    }
                    #endregion
                    #region Update Customer System No
                    CustomerSystemNo cusno = new CustomerSystemNo();
                    cusno = _Util.Facade.CustomerSystemNoFacade.GetCusSysNoByCustomerNo(extended.AvantgradRefId);
                    if (cusno != null)
                    {
                        cusno.IsUsed = true;
                        cusno.CustomerId = Cus.Id;
                        cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                        _Util.Facade.CustomerSystemNoFacade.UpdateCustomerSystemNo(cusno);
                    }
                    #endregion
                }
                else
                {
                    result = false;
                    message = response.ErrorMessage;
                }

            }
            catch (Exception ex)
            {
                result = false;
                message = "Internal Error!";
                return Json(new { result = result, message = message });
            }

            return Json(new { result = result, message = message });
        }

        public ActionResult AGTerminationDetails(string TransmitterCode)
        {
            ViewBag.AgOOSCategoryList = _Util.Facade.LookupFacade.GetLookupByKey("AgOOSCategory").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
              new SelectListItem()
              {
                  Text = x.DisplayText.ToString(),
                  Value = x.DataValue.ToString()
              }).ToList();
            return View();
        }

        public JsonResult TerminateAgAccount(string TransmitterCode, string Category, DateTime OOSDate, string Comment)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string message = "Successfully Sync";
            bool result = false;


            string UserId = "RMRCloudAPI";
            string Password = "TXLxSZZ2hOlQe";

            try
            {
                HS.Avantguard.com.agmonitoring.devportal.SiteGroupGateway gateway = new Avantguard.com.agmonitoring.devportal.SiteGroupGateway();
                HS.Avantguard.com.agmonitoring.devportal.Result resultInfo = gateway.OutService(UserId, Password, TransmitterCode, Category, OOSDate.Date, Comment);
                if (resultInfo != null && resultInfo.ErrorNum == 0)
                {

                    result = true;
                    message = "Terminated successfully";

                }
                else
                {
                    result = false;
                    message = resultInfo.ErrorMessage;
                }

            }
            catch (Exception ex)
            {
                result = false;
                message = "Internal Error!";
            }



            return Json(new { result = result, message = message });
        }

        public ActionResult GetAGAccountHistory(string TransmitterCode, DateTime StartDate, DateTime EndDate)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            string message = "Successfully Sync";
            bool result = false;


            string UserId = "RMRCloudAPI";
            string Password = "TXLxSZZ2hOlQe";
            List<HS.Avantguard.com.agmonitoring.devportal.History> model = new List<Avantguard.com.agmonitoring.devportal.History>();
            try
            {
                HS.Avantguard.com.agmonitoring.devportal.SiteGroupGateway gateway = new Avantguard.com.agmonitoring.devportal.SiteGroupGateway();
                HS.Avantguard.com.agmonitoring.devportal.DataResultOfListOfHistory historyList = new Avantguard.com.agmonitoring.devportal.DataResultOfListOfHistory();
                historyList = gateway.GetHistory(UserId, Password, TransmitterCode, StartDate, EndDate, StartDate.UTCCurrentTime(), EndDate.UTCCurrentTime());

                if (historyList != null && historyList.Result1.Count() > 1)
                {
                    model = historyList.Result1.ToList();
                }

            }
            catch (Exception ex)
            {
                result = false;
                message = "Internal Error!";
            }



            return View(model);
        }

        public JsonResult PlaceAgAccountOnTest(string TransmitterCode, string Category, int hour, int minute)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string message = "Account is placed on test.";
            bool result = false;


            string UserId = "RMRCloudAPI";
            string Password = "TXLxSZZ2hOlQe";

            try
            {
                HS.Avantguard.com.agmonitoring.devportal.SiteGroupGateway gateway = new Avantguard.com.agmonitoring.devportal.SiteGroupGateway();
                HS.Avantguard.com.agmonitoring.devportal.Result testResult = gateway.OnTest(UserId, Password, TransmitterCode, Category, DateTime.Now, hour, minute, null);
                if (testResult != null && testResult.ErrorNum == 0)
                {

                    result = true;
                    message = "Account is placed on test";

                }
                else
                {
                    result = false;
                    message = testResult.ErrorMessage;
                }

            }
            catch (Exception ex)
            {
                result = false;
                message = "Internal Error!";
            }



            return Json(new { result = result, message = message });
        }


        #endregion


        #region ISPC
        public ActionResult ISPC(Guid CustomerId)
        {
            CustomerExtended cusEx = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(CustomerId);
            ViewBag.AppId = "";
            if (!String.IsNullOrEmpty(cusEx.IsPcApplicationId))
            {
                ViewBag.AppId = cusEx.IsPcApplicationId;
            }
            return View();
        }
        public ActionResult IsPcDetailInfo(Guid CustomerId)
        {
            CustomerExtended cusExt = new CustomerExtended();
            cusExt = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(CustomerId);
            if (cusExt != null)
            {
                ViewBag.AppId = cusExt.IsPcApplicationId;
                ViewBag.AppStatus = cusExt.IsPcAppStatus;
            }

            return View();
        }
        #endregion


        #region SmartVault
        public async Task<ActionResult> GetRequestedNonce()
        {
            var baseAddress = new Uri("https://rest.smartvault.com/");
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                using (var content = new StringContent("{  \"client_id\": \"Rezwanul10\"}", System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("auto/auth/nonce/1", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return null;
        }
//        public async Task<ActionResult> GetClientToken()
//        {

//            var baseAddress = new Uri("https://rest.smartvault.com/");

//            SmartVaultTokenGenerator tg = new SmartVaultTokenGenerator();
//            string token = tg.Generate(@"-----BEGIN RSA PRIVATE KEY-----
//MIIEpAIBAAKCAQEA7wMGN5Egrn+vCGI5hqN9mGGtcf6SiY7P9zqB7Q+ZnGC9d2Zg
//16hJZixfpmyo7O9kdwaslH4A4myOAzuH87kUaoAVBrYrocYF1EIwWB64oi4TCEtQ
//3o0W2x5pZ1k6so1xHvW203NZWlvkdFoaixi5XOi3ZObh4UHxDJvT7pBTX3gqR1fZ
//lg+P5KWNnIUxfglIy1YNir9qEic87BHxxixdeQ9EhV4mel2wvdMzFKD+3IeYi2SQ
//vrK4Oh1yRBtWnvn1dxGiwy0UuxiKNcbwbyoIrY0CyKGekcaALgMdOYJovYAzCa9y
//6Yb+JgI5/JSFeTXGUSle49cLijISOhs3K5uBDQIDAQABAoIBAQDBmYSGrAJ59ij0
//NIP+QB2yuaQBi0BtNgXUb0rgl9tIZL/zoDTfWowhu926c5edXAfXtctC+JGE1oNt
//sOHdlFQqNBdT+Wl8P2dnWDnKlw/dJk76T+9wZX5W/NpKAWffS1yOxF9UBcIhw/9Y
//E8geVWeID2TF0ZKzoHM9UFepjLRLxfwxy1PhlKF5+nmn0A170z8Gsc3oc+r8YF9P
//Ao9/SlMn5H3utNcHfjKwyrs24tAjTAuWsgO6f+6+j6cCVMVvaWPbnlfy6/23OtXO
//75LcUeC96Q4TiRUZKrQQdAgLiD5oJJr7SFU9YWcdLgg3PaYnwbyxBGj4Y7Lvv+l6
//hclt4NDBAoGBAP4LHC0GkTeiQMb7HdKcfgHB9s1WjUPqRfF6PR+o9le/iu8S0UeO
//95DZ4br1s2JqsU8iJ5jl58AACzFLJVLXEinvTOhpdOEmwNipTzAJljSxs+EyqGWX
//yGbACUdQ5xGAxHhn3Yb2AqjhrzJp70tNTiDSgjFex5eISf0QunJAknI9AoGBAPDa
//RuINN5+JgEBN8Cry1++Z2Uto+AbWXAwHDGC4OZCRIW0Tl6juOA4F9DfOOqUXPxHR
//r053HZxhfPZa0IHAjVMR+WafKcWkEhw0Bamz7J+ljl+iLaskYgY4k5A56/BRwlhH
//WCvl+2Wn1cE4t4lDO0ouyHZjVW4i6Ei7GgYnL0cRAoGAZrAg/IZKDt30Jzp8bJS7
//ToST+Z6BzbEWAq0xpemLYh7XzxmhJLbClXetCgClOSP9dgpNTT2pdu/NlmhUtu6e
//tfq38L4n85bRnwKZMsa9Q6GNH3t1nFNFO1tmpMAsFuQhBradUh+BJbjMM6mkg6DE
//8vmEjOZN8Y+ysKuhYet+BtUCgYEAh9o2/S6DX2OJibvsae1MS4O2A0kUDZc/mDEp
//lCyVm6ug9QuGEe08hPxqwkd64L/5/S0O+u1JMHn0qGiVD3ryvBZ5XJ8OSsK+zFWz
//lAM+xtX1NpAdvljpSaUD5ugk4wra2jxzyV8RrEc81J4POEdJ3BADVnO1LkhWSR/w
//D+RuUGECgYAcZu3SjCb74v3tWNZkDsPR1ynDYOCE3qONf95/tivmrBhhcdnyFavN
//58uWuBf7FIr+nml8GRhk+eza+bIcSRjE+R6oHPvxKTZe8HmNiW1XHpkWkShaken5
//pWQZlsPF1uzyBYIsYFS1aYgDUBnfrqB703BwKHA53FpJ7RIwm92+vQ==
//-----END RSA PRIVATE KEY-----", "Rezwanul10", "d3NU3fD5NEeq2mGlVuGD9A");
//            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
//            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
//            {

//                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

//                using (var content = new StringContent("{  \"token\": \"" + token + "\"}", System.Text.Encoding.Default, "application/json"))
//                {
//                    using (var response = await httpClient.PostAsync("auto/auth/ctoken/1", content))
//                    {
//                        string responseData = await response.Content.ReadAsStringAsync();
//                    }
//                }
//            }
//            return null;
//        }
        public async Task<ActionResult> GetDeligationToken()
        {
            var baseAddress = new Uri("https://rest.smartvault.com/");
            string token = @"Q0xJMDAAAAAAAAABUYCGflRfXt7RYQ3vYa/aCHHeVcuzPW4Xlzq2ZR6ABn7EtQ==";
            string toEncode = "Rezwanul10:" + token;
            byte[] bytes = Encoding.GetEncoding(28591).GetBytes(toEncode);
            string toReturn = System.Convert.ToBase64String(bytes);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Basic " + toReturn);

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                using (var content = new StringContent("{  \"user_email\": \"rezwan.piistech@gmail.com\"}", System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("auto/auth/dtoken/1", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                    }
                }
            }
                return null;
        }
        public async Task<ActionResult> UploadFile()
        {
            var baseAddress = new Uri("https://rest.smartvault.com/");
            string token = @"REVMMDAAAAAAAAASdQBBienHwTJxWg9DEzdUxj83daTkJk4OgQ1tQtzZLXTj9dwbcmNuagHRhGiICUCGDqBiNYNZc7nIJcHhNRby+JJe";
            string toEncode = "rezwan.piistech@gmail.com:" + token;
            byte[] bytes = Encoding.GetEncoding(28591).GetBytes(toEncode);
            string toReturn = System.Convert.ToBase64String(bytes);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
         
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Basic "+toReturn);

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                using (var content = new StringContent("{  \"create_file\": [    {      \"name\": \"hello.txt\"    }  ]}", System.Text.Encoding.Default, "application/json"))
                {
                    using (  var response = await httpClient.PutAsync("nodes/pth/{Test}", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return null;
        }  
        public async Task<ActionResult> RetriveFolder()
        {
            var baseAddress = new Uri("https://rest.smartvault.com/");
            string token = @"REVMMDAAAAAAAAASdQBBienHwTJxWg9DEzdUxj83daTkJk4OgQ1tQtzZLXTj9dwbcmNuagHRhGiICUCGDqBiNYNZc7nIJcHhNRby+JJe";
            string toEncode = "rezwan.piistech@gmail.com:" + token;
            byte[] bytes = Encoding.GetEncoding(28591).GetBytes(toEncode);
            string toReturn = System.Convert.ToBase64String(bytes);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


            
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Basic dGVzdHVzZXJAc21hcnR2YXVsdC5jb206UTB4Sk1EQUFBQUFBQUFBQlVZRE9MOE82N3oyQjdvVmJLcytWMngybmZHTXgzR2FzY2pNUEp4Y0dGeHZPeWc9PQ==");

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                using (var response = await httpClient.GetAsync("nodes/pth/{path}{?children,acl,eprop,search,page,per_page,sort,direction,record_id}"))
                {

                    string responseData = await response.Content.ReadAsStringAsync();
                }
            }
            return null;
        }


        #endregion
    }
}
