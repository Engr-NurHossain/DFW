using System;
using HS.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using HS.Web.UI.Helper;
using HS.Framework;
using Newtonsoft.Json;
using HS.Framework.Utils;
using MyExcel = ClosedXML.Excel;
using ClosedXML.Excel;
using ExcelCl = HS.Web.UI.Helper.ExcelFormatHelper;
using HS.Web.UI.Business.Inventory;
using HS.Entities.Result;
using NLog;
using DocumentFormat.OpenXml.Wordprocessing;

namespace HS.Web.UI.Controllers
{
    public class InventoryController : BaseController
    {
        public InventoryController()
        {
            //LogManager.ThrowExceptions = true;
            //LogManager.ThrowConfigExceptions = true;
            logger = LogManager.GetCurrentClassLogger();
        }
        // GET: Inventory
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

        public ActionResult ProductClassPartial()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<EquipmentClass> equipment = _Util.Facade.InventoryFacade.GetAllEquipmentClassByCompanyId(currentLoggedIn.CompanyId.Value);
            return PartialView(equipment);
        }
        [Authorize]
        public ActionResult AddProductClass(int? id)
        {
            EquipmentClass model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (id.HasValue)
            {
                model = _Util.Facade.InventoryFacade.GetEquipmentClassById(id.Value);
            }
            else
            {
                model = new EquipmentClass();
            }
            ViewBag.EquipmentClassList = _Util.Facade.InventoryFacade
                .GetAllEquipmentClassByCompanyId(currentLoggedIn.CompanyId.Value).Select(x =>
                               new SelectListItem()
                               {
                                   Text = x.Name.ToString(),
                                   Value = x.Id.ToString()
                               }).ToList();
            return PartialView("AddProductClass", model);
        }

        public PartialViewResult ErrorPage()
        {
            return PartialView("~/Views/Shared/_AccessDenied.cshtml");
        }

        [Authorize]
        public JsonResult GetEquipmentOptionsByKey(string Key, string Type)
        {
            string result = "[]";
            if (string.IsNullOrWhiteSpace(Type) ||
                (Type != LabelHelper.ServiceOptionsType.Location && Type != LabelHelper.ServiceOptionsType.Type
                && Type != LabelHelper.ServiceOptionsType.Model && Type != LabelHelper.ServiceOptionsType.Finish
                && Type != LabelHelper.ServiceOptionsType.Capacity)
                )
            {
                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }
            List<EquipmentOptions> Options = _Util.Facade.EquipmentFacade.GetEquipmentOptionsByKeyAndType(Key, Type);

            if (Options.Count > 0)
                result = JsonConvert.SerializeObject(Options);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddProductClass(EquipmentClass eq)
        {
            bool result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            eq.IsActive = true;
            eq.CompanyId = currentLoggedIn.CompanyId.Value;
            if (eq.Id > 0)
            {

                result = _Util.Facade.InventoryFacade.UpdateEquipmentClass(eq);
            }
            else
            {
                result = _Util.Facade.InventoryFacade.InsertEquipmentClass(eq) > 0;

            }
            return Json(result);
        }
        [Authorize]
        public ActionResult AddEquepment(int? Id, Guid? EquipmentId, bool? OpenFromModal, string Flag)
        {
            ViewBag.OpenFromModal = OpenFromModal;
            Equipment equipment = new Equipment();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            #region Point Values
            GlobalSetting globalSetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "EquipmentPointValue");
            if (globalSetting != null)
            {
                ViewBag.PointValue = globalSetting.Value;
            }

            GlobalSetting GlobSetRefPoint = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "EquipmentRefPointValue");
            if (GlobSetRefPoint != null)
            {
                ViewBag.RefPointValue = GlobSetRefPoint.Value;
            }
            #endregion

            ViewBag.EquipmentTypeList = GetEquipmentTypeList();
            ViewBag.ManufacturerList = GetManufacturerList();
            ViewBag.SupplierList = GetSupplierList();

            if (Id.HasValue && Id > 0)
            {
                equipment = _Util.Facade.EquipmentFacade.GetEquipmentById(Id.Value);
            }
            else if (EquipmentId.HasValue && EquipmentId != Guid.Empty)
            {
                equipment = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(EquipmentId.Value);
            }
            else
            {
                equipment.EquipmentId = Guid.NewGuid();

                #region Overhead rate and profit rate
                GlobalSetting GlobOverheadRate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("EstimatorOverheadRate");
                GlobalSetting GlobProfitRate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("EstimatorProfitRate");
                double OverheadRate = 0;
                double ProfitRate = 0;
                if (GlobOverheadRate != null)
                {
                    double.TryParse(GlobOverheadRate.Value, out OverheadRate);
                }
                if (GlobProfitRate != null)
                {
                    double.TryParse(GlobProfitRate.Value, out ProfitRate);
                }
                equipment.OverheadRate = OverheadRate;
                equipment.ProfitRate = ProfitRate;
                #endregion

            }
            //else
            //{
            //    equipment.EquipmentId = Guid.NewGuid();
            //    //equipment.EquipmentClassId = 1;  
            //    equipment.CreatedDate = DateTime.Now.UTCCurrentTime();
            //    equipment.CompanyId = currentLoggedIn.CompanyId.Value;
            //    equipment.LastUpdatedBy = User.Identity.Name;
            //    equipment.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            //    equipment.IsActive = false;
            //    _Util.Facade.EquipmentFacade.InsertEquipment(equipment);
            //}
            List<GridSetting> EquipmentUiSetting = new List<GridSetting>();
            EquipmentUiSetting = _Util.Facade.GridSettingsFacade.GetAllByKey("EquipmentGrid", currentLoggedIn.CompanyId.Value);
            if (EquipmentUiSetting.Count > 0)
            {
                EquipmentUiSetting = EquipmentUiSetting.OrderBy(x => x.OrderBy).Where(x => x.FormActive == true).ToList();
            }
            if (equipment.IsTaxable == null)
            {
                equipment.IsTaxable = true;
            }
            var settingsManufacDetails = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "InventoryManufacturerDetailsSettings");
            if (settingsManufacDetails != null)
            {
                bool showManuDetails = false;
                bool.TryParse(settingsManufacDetails.Value, out showManuDetails);

                ViewBag.ShowManufactureDetails = showManuDetails; //Convert.ToBoolean(settingsManufacDetails.Value);
            }
            else
            {
                ViewBag.ShowManufactureDetails = false;
            }
            ViewBag.EquipmentUiSetting = EquipmentUiSetting;
            ViewBag.Flag = Flag;
            ViewBag.EquipmentUnitList = _Util.Facade.LookupFacade.GetDropdownsByKey("EquipmentUnit");
            return View(equipment);
        }

        [Authorize]
        public ActionResult AddService(int? Id)
        {
            Equipment equipment = new Equipment();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            //equipment.EquipmentId = Guid.NewGuid();
            //equipment.EquipmentClassId = 1;  
            //equipment.CreatedDate = DateTime.Now.UTCCurrentTime();
            //equipment.CompanyId = currentLoggedIn.CompanyId.Value;
            //equipment.LastUpdatedBy = User.Identity.Name;
            //equipment.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            //equipment.IsActive = false;
            //_Util.Facade.EquipmentFacade.InsertEquipment(equipment);

            ViewBag.EquipmentTypeList = GetEquipmentTypeList();
            ViewBag.ManufacturerList = GetManufacturerList();
            ViewBag.SupplierList = GetSupplierList();

            List<SelectListItem> TagList = new List<SelectListItem>();
            TagList.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("TagList").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList());
            ViewBag.TagList = TagList;

            if (Id > 0)
            {
                equipment = _Util.Facade.EquipmentFacade.GetEquipmentById(Id.Value);
                if (equipment != null)
                {
                    equipment.ServiceEquipments = _Util.Facade.EquipmentFacade.GetEquipmentServiceListByServiceId(equipment.EquipmentId);

                }
            }
            if (equipment.IsWarrenty == null)
            {
                equipment.IsWarrenty = false;
            }
            if (equipment.IsTaxable == null)
            {
                equipment.IsTaxable = true;
            }
            return PartialView("_AddService", equipment);
        }
        [Authorize]
        public ActionResult AddEquipmentManufacturer(int? eqManuId)
        {
            ViewBag.ManufacturerList = null;
            EquipmentManufacturer em = new EquipmentManufacturer();
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            List<Manufacturer> Manufacturerdropdown = _Util.Facade.ManufacturerFacade.GetAllManufacturerName();
            if (Manufacturerdropdown != null && Manufacturerdropdown.Count > 0)
            {
                ListItems.AddRange(Manufacturerdropdown.OrderBy(x => x.ManufacturerId.ToString() != "-1").ThenBy(x => x.Name).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.Name.ToString(),
                               Value = x.ManufacturerId.ToString()
                           }).ToList());
            }

            //ViewBag.SupplierList = ListItems;
            ViewBag.ManufacturerList = ListItems;
            if (eqManuId != null && eqManuId > 0)
            {
                EquipmentManufacturer tempEquipManu = _Util.Facade.EquipmentFacade.GetEquipmentManufacturerById(eqManuId.Value);
                if (tempEquipManu != null)
                {
                    em = tempEquipManu;
                }
            }
            return View(em);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddEquipmentManufacturer(EquipmentManufacturer eqpManufacturer)
        {
            bool result = false;
            double SupplierCost = 0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (eqpManufacturer.EquipmentId == Guid.Empty)
            {
                return Json(new { result = result, message = "Equipment not found." });
            }
            if (eqpManufacturer.ManufacturerId == Guid.Empty)
            {
                return Json(new { result = result, message = "Select a vendor first." });
            }

            Equipment equipment = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(eqpManufacturer.EquipmentId);
            List<EquipmentManufacturer> equipmentmanucount = _Util.Facade.EquipmentFacade.GetEquipmentManufacturerByEquipmentId(eqpManufacturer.EquipmentId);
            if (equipmentmanucount == null || equipmentmanucount.Where(x => x.IsPrimary).Count() == 0)
            {
                eqpManufacturer.IsPrimary = true;
            }
            //if (equipment == null)
            //{
            //    return Json(new { result = result, message = "Equipment not found." });
            //}

            if (eqpManufacturer.IsPrimary)
            {

                SupplierCost = eqpManufacturer.Cost;
                _Util.Facade.EquipmentFacade.UpdateEquipmentManufacturerSetIsPrimaryFalse(eqpManufacturer.EquipmentId);
                if (equipment != null)
                {
                    equipment.SupplierCost = eqpManufacturer.Cost;
                    equipment.Cost = eqpManufacturer.Cost;
                    _Util.Facade.EquipmentFacade.UpdateEquipment(equipment);
                }
            }
            else
            {
                EquipmentManufacturer primary = equipmentmanucount.Where(x => x.IsPrimary).FirstOrDefault();
                if (primary != null)
                {
                    SupplierCost = primary.Cost;
                }
            }

            if (eqpManufacturer.Id > 0)
            {
                EquipmentManufacturer tempEquipVendor = _Util.Facade.EquipmentFacade.GetEquipmentManufacturerById(eqpManufacturer.Id);
                if (tempEquipVendor == null)
                {
                    eqpManufacturer.AddedBy = CurrentUser.UserId;
                    eqpManufacturer.AddedDate = DateTime.Now.UTCCurrentTime();
                    result = _Util.Facade.EquipmentFacade.InsertEquipmentManufacturer(eqpManufacturer) > 0;
                }
                else
                {
                    eqpManufacturer.AddedBy = tempEquipVendor.AddedBy;
                    eqpManufacturer.AddedDate = tempEquipVendor.AddedDate;

                    result = _Util.Facade.EquipmentFacade.UpdateEquipmentManufacturer(eqpManufacturer);
                }

            }
            else
            {
                eqpManufacturer.AddedBy = CurrentUser.UserId;
                eqpManufacturer.AddedDate = DateTime.Now.UTCCurrentTime();

                result = _Util.Facade.EquipmentFacade.InsertEquipmentManufacturer(eqpManufacturer) > 0;

            }
            return Json(new { result = result, message = "", SupplierCost = SupplierCost });
        }
        public ActionResult ManufacturerList(Guid EquipmentId)
        {
            List<EquipmentManufacturer> evList = new List<EquipmentManufacturer>();
            evList = _Util.Facade.EquipmentFacade.GetAllEquipmentManufacturerByEquipmentId(EquipmentId);

            return View(evList);
        }
        [Authorize]
        public ActionResult AddEquipmentVendor(int? eqVendorId)
        {
            ViewBag.SupplierList = null;
            EquipmentVendor ev = new EquipmentVendor();
            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            List<Supplier> SupplierDropDown = _Util.Facade.SupplierFacade.GetAllSupplierName();
            if (SupplierDropDown != null && SupplierDropDown.Count > 0)
            {
                ListItems.AddRange(SupplierDropDown.OrderBy(x => x.SupplierId.ToString() != "-1").ThenBy(x => x.CompanyName).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.CompanyName.ToString(),
                               Value = x.SupplierId.ToString()
                           }).ToList());
            }

            ViewBag.SupplierList = ListItems;
            if (eqVendorId != null && eqVendorId > 0)
            {
                EquipmentVendor tempEquipVendor = _Util.Facade.EquipmentFacade.GetEquipmentVendorById(eqVendorId.Value);
                if (tempEquipVendor != null)
                {
                    ev = tempEquipVendor;
                }
            }
            return View(ev);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddEquipmentVendor(EquipmentVendor eqpVendor)
        {
            bool result = false;
            double SupplierCost = 0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (eqpVendor.EquipmentId == Guid.Empty)
            {
                return Json(new { result = result, message = "Equipment not found." });
            }
            if (eqpVendor.SupplierId == Guid.Empty)
            {
                return Json(new { result = result, message = "Select a vendor first." });
            }

            Equipment equipment = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(eqpVendor.EquipmentId);
            List<EquipmentVendor> equipmetntVendorCount = _Util.Facade.EquipmentFacade.GetEquipmentVendorByEquipmentId(eqpVendor.EquipmentId);
            if (equipmetntVendorCount == null || equipmetntVendorCount.Where(x => x.IsPrimary).Count() == 0)
            {
                eqpVendor.IsPrimary = true;
            }
            //if (equipment == null)
            //{
            //    return Json(new { result = result, message = "Equipment not found." });
            //}

            if (eqpVendor.IsPrimary)
            {

                SupplierCost = eqpVendor.Cost;
                _Util.Facade.EquipmentFacade.UpdateEquipmentVendorSetIsPrimaryFalse(eqpVendor.EquipmentId);
                if (equipment != null)
                {
                    equipment.SupplierCost = eqpVendor.Cost;
                    equipment.Cost = eqpVendor.Cost;
                    _Util.Facade.EquipmentFacade.UpdateEquipment(equipment);
                }
            }
            else
            {
                EquipmentVendor primary = equipmetntVendorCount.Where(x => x.IsPrimary).FirstOrDefault();
                if (primary != null)
                {
                    SupplierCost = primary.Cost;
                }
            }

            if (eqpVendor.Id > 0)
            {
                EquipmentVendor tempEquipVendor = _Util.Facade.EquipmentFacade.GetEquipmentVendorById(eqpVendor.Id);
                if (tempEquipVendor == null)
                {
                    eqpVendor.AddedBy = CurrentUser.UserId;
                    eqpVendor.AddedDate = DateTime.Now.UTCCurrentTime();
                    result = _Util.Facade.EquipmentFacade.InsertEquipmentVendor(eqpVendor) > 0;
                }
                else
                {
                    eqpVendor.AddedBy = tempEquipVendor.AddedBy;
                    eqpVendor.AddedDate = tempEquipVendor.AddedDate;

                    result = _Util.Facade.EquipmentFacade.UpdateEquipmentVendor(eqpVendor);
                }

            }
            else
            {
                eqpVendor.AddedBy = CurrentUser.UserId;
                eqpVendor.AddedDate = DateTime.Now.UTCCurrentTime();

                result = _Util.Facade.EquipmentFacade.InsertEquipmentVendor(eqpVendor) > 0;

            }
            return Json(new { result = result, message = "", SupplierCost = SupplierCost });
        }

        public ActionResult VendorList(Guid EquipmentId)
        {
            List<EquipmentVendor> evList = new List<EquipmentVendor>();
            evList = _Util.Facade.EquipmentFacade.GetAllEquipmentVendorByEquipmentId(EquipmentId);

            return View(evList);
        }

        [Authorize]
        public ActionResult DeleteEquipmentManufacturer(int? Id)
        {
            bool result = false;
            try
            {
                if (Id.HasValue && Id.Value > 0)
                {
                    EquipmentManufacturer ev = _Util.Facade.EquipmentFacade.GetEquipmentManufacturerById(Id.Value);
                    _Util.Facade.EquipmentFacade.DeleteEquipmentManufacturerById(Id.Value);
                    Equipment eq = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(ev.EquipmentId);

                    Double NewSupplierCost = 0;

                    if (ev != null && ev.IsPrimary)
                    {
                        List<EquipmentManufacturer> evlist = _Util.Facade.EquipmentFacade.GetEquipmentManufacturerByEquipmentId(ev.EquipmentId);
                        if (evlist != null && evlist.Count() > 0)
                        {
                            EquipmentManufacturer primary = evlist.FirstOrDefault();
                            primary.IsPrimary = true;
                            _Util.Facade.EquipmentFacade.UpdateEquipmentManufacturer(primary);
                            NewSupplierCost = primary.Cost;
                        }
                    }

                    if (eq != null)
                    {
                        eq.SupplierCost = NewSupplierCost;
                        _Util.Facade.EquipmentFacade.UpdateEquipment(eq);
                    }
                    result = true;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return Json(new { result = result });
        }

        [Authorize]
        public ActionResult DeleteEquipmentVendor(int? Id)
        {
            bool result = false;
            try
            {
                if (Id.HasValue && Id.Value > 0)
                {
                    EquipmentVendor ev = _Util.Facade.EquipmentFacade.GetEquipmentVendorById(Id.Value);
                    _Util.Facade.EquipmentFacade.DeleteEquipmentVendorById(Id.Value);
                    Equipment eq = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(ev.EquipmentId);

                    Double NewSupplierCost = 0;

                    if (ev != null && ev.IsPrimary)
                    {
                        List<EquipmentVendor> evlist = _Util.Facade.EquipmentFacade.GetEquipmentVendorByEquipmentId(ev.EquipmentId);
                        if (evlist != null && evlist.Count() > 0)
                        {
                            EquipmentVendor primary = evlist.FirstOrDefault();
                            primary.IsPrimary = true;
                            _Util.Facade.EquipmentFacade.UpdateEquipmentVendor(primary);
                            NewSupplierCost = primary.Cost;
                        }
                    }

                    if (eq != null)
                    {
                        eq.SupplierCost = NewSupplierCost;
                        _Util.Facade.EquipmentFacade.UpdateEquipment(eq);
                    }
                    result = true;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return Json(new { result = result });
        }
        [Authorize]
        public PartialViewResult EquipmentsListPartial()
        {
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.InventoryWareHouseTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //List<Equipment> equipments = _Util.Facade.InventoryFacade.GetAllEquipmentsByCompanyId(currentLoggedIn.CompanyId.Value);

            ViewBag.EquipmentTypeList = GetEquipmentTypeList();

            //List<SelectListItem> EquipentTypeList = new List<SelectListItem>();
            //EquipentTypeList.Add(new SelectListItem()
            //{
            //    Text = "Please Select",
            //    Value = "-1"
            //});
            //EquipentTypeList.AddRange(ViewBag.EquipmentTypeList = _Util.Facade.EquipmentTypeFacade
            //  .GetAllProductCategoryByCompanyId(currentLoggedIn.CompanyId.Value).Select(x =>
            //                 new SelectListItem()
            //                 {
            //                     Text = x.Name.ToString(),
            //                     Value = x.Id.ToString()
            //                 }).ToList());
            //ViewBag.EquipmentTypeList = EquipentTypeList;

            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });

            List<EquipmentClass> EquipmentClassDropDown = _Util.Facade.EquipmentFacade.GetAllEquipmentClassByCompanyId(CurrentUser.CompanyId.Value);
            if (EquipmentClassDropDown != null && EquipmentClassDropDown.Count > 0)
            {
                ListItems.AddRange(EquipmentClassDropDown.OrderBy(x => x.Name).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.Name.ToString(),
                               Value = x.Id.ToString()
                           }).ToList());
            }

            ViewBag.EquipmentClassTypeList = ListItems;

            ViewBag.EquipmentActiveStatus = _Util.Facade.LookupFacade.GetLookupByKey("EquipmentActiveStatus").Where(x => x.DataValue != "-1").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();

            ViewBag.StockStatus = _Util.Facade.LookupFacade.GetLookupByKey("StockStatus").Select(x =>
                      new SelectListItem()
                      {
                          Text = x.DisplayText.ToString(),
                          Value = x.DataValue.ToString()
                      }).ToList();
            List<string> activestatus = new List<string>();
            activestatus.Add("1");
            ViewBag.listactivestatus = activestatus;
            return PartialView("_EuipmentList");
        }

        [HttpPost]
        public JsonResult ChangeEquipmentStatus(int id, string isActive)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Equipment equip = _Util.Facade.InventoryFacade.GetEquipmentId(id);
            if (equip != null)
            {
                if (isActive == "false")
                {
                    equip.IsActive = false;
                }
                else
                {
                    equip.IsActive = true;
                }
                _Util.Facade.InventoryFacade.UpdateEquipmentByEquipmentObject(equip);
                result = true;
            }

            return Json(new { result = result });
        }
        [Authorize]
        public PartialViewResult FilterEquipmentsListPartial(FilterEquipment _FilterEquipment)
        {
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.InventoryWareHouseTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            int PageLimit = 0;
            _FilterEquipment.EmployeeRole = "";
            List<string> activestatus = new List<string>();
            if (!string.IsNullOrWhiteSpace(_FilterEquipment.ActiveInactiveStatus))
            {
                string[] splituser = _FilterEquipment.ActiveInactiveStatus.Split(',');
                if (splituser.Length > 0)
                {
                    _FilterEquipment.ActiveInactiveStatus = string.Format("{0}", string.Join(",", splituser));
                    foreach (var item in splituser)
                    {
                        activestatus.Add(item);
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(currentLoggedIn.UserRole) && currentLoggedIn.UserId != new Guid())
            {
                var objEmpRole = _Util.Facade.EmployeeFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(currentLoggedIn.UserId, currentLoggedIn.CompanyId.Value);
                _FilterEquipment.EmployeeRole = objEmpRole.Name;
            }
            else if (!string.IsNullOrWhiteSpace(currentLoggedIn.UserRole))
            {
                _FilterEquipment.EmployeeRole = currentLoggedIn.UserRole;
            }
            PageLimit = _Util.Facade.GlobalSettingsFacade.GetInventoryPagingLimit(currentLoggedIn.CompanyId.Value);
            EquipmentListWithCountModel equipmentfilterlist = new EquipmentListWithCountModel();
            ViewBag.InventoryGridSettings = _Util.Facade.GridSettingsFacade.GetByKey("InventoryGrid", currentLoggedIn.CompanyId.Value);
            _FilterEquipment.CompanyId = currentLoggedIn.CompanyId.Value;

            if (_FilterEquipment.PageNo == 0)
            {
                _FilterEquipment.PageNo = 1;
            }

            if (_FilterEquipment.PageSize < PageLimit)
            {
                _FilterEquipment.PageSize = PageLimit;
            }
            _FilterEquipment.EquipmentClass = 1;
            equipmentfilterlist = _Util.Facade.InventoryFacade.GetEquipmentByFilter(_FilterEquipment);

            if (equipmentfilterlist.EquipmentList.Count() == 0)
            {
                _FilterEquipment.PageNo = 1;
                //customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilter(filter);
            }

            ViewBag.PageNumber = _FilterEquipment.PageNo;
            ViewBag.OutOfNumber = 0;


            if (equipmentfilterlist.EquipmentList.Count() > 0)
            {
                ViewBag.OutOfNumber = equipmentfilterlist.TotalEquipmentCount.Counter;
            }

            if ((int)ViewBag.PageNumber * _FilterEquipment.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * _FilterEquipment.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / _FilterEquipment.PageSize);
            ViewBag.order = _FilterEquipment.order;

            ViewBag.listactivestatus = activestatus;
            return PartialView("_FilteredEquipmentList", equipmentfilterlist);
        }
        [Authorize]
        public PartialViewResult ServiceListPartial()
        {
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.InventoryWareHouseTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            ViewBag.EquipmentTypeList = GetEquipmentTypeList();

            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });

            List<EquipmentClass> EquipmentClassDropDown = _Util.Facade.EquipmentFacade.GetAllEquipmentClassByCompanyId(CurrentUser.CompanyId.Value);
            if (EquipmentClassDropDown != null && EquipmentClassDropDown.Count > 0)
            {
                ListItems.AddRange(EquipmentClassDropDown.OrderBy(x => x.Name).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.Name.ToString(),
                               Value = x.Id.ToString()
                           }).ToList());
            }



            ViewBag.EquipmentClassTypeList = ListItems;

            ViewBag.EquipmentActiveStatus = _Util.Facade.LookupFacade.GetLookupByKey("EquipmentActiveStatus").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();

            ViewBag.StockStatus = _Util.Facade.LookupFacade.GetLookupByKey("StockStatus").Select(x =>
                      new SelectListItem()
                      {
                          Text = x.DisplayText.ToString(),
                          Value = x.DataValue.ToString()
                      }).ToList();


            return PartialView("_ServiceList");
        }

        [Authorize]
        public PartialViewResult FilterServiceListPartial(FilterEquipment _FilterEquipment)
        {
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.InventoryWareHouseTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            int PageLimit = 0;
            _FilterEquipment.EmployeeRole = "";
            if (string.IsNullOrWhiteSpace(currentLoggedIn.UserRole) && currentLoggedIn.UserId != new Guid())
            {
                var objEmpRole = _Util.Facade.EmployeeFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(currentLoggedIn.UserId, currentLoggedIn.CompanyId.Value);
                _FilterEquipment.EmployeeRole = objEmpRole.Name;
            }
            else if (!string.IsNullOrWhiteSpace(currentLoggedIn.UserRole))
            {
                _FilterEquipment.EmployeeRole = currentLoggedIn.UserRole;
            }
            PageLimit = _Util.Facade.GlobalSettingsFacade.GetInventoryPagingLimit(currentLoggedIn.CompanyId.Value);
            EquipmentListWithCountModel equipmentfilterlist = new EquipmentListWithCountModel();
            ViewBag.InventoryGridSettings = _Util.Facade.GridSettingsFacade.GetByKey("ServiceGrid", currentLoggedIn.CompanyId.Value);
            _FilterEquipment.CompanyId = currentLoggedIn.CompanyId.Value;

            if (_FilterEquipment.PageNo == 0)
            {
                _FilterEquipment.PageNo = 1;
            }

            if (_FilterEquipment.PageSize < PageLimit)
            {
                _FilterEquipment.PageSize = PageLimit;
            }
            _FilterEquipment.EquipmentClass = 2;
            equipmentfilterlist = _Util.Facade.InventoryFacade.GetEquipmentByFilter(_FilterEquipment);

            if (equipmentfilterlist.EquipmentList.Count() == 0)
            {
                _FilterEquipment.PageNo = 1;
                //customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilter(filter);
            }

            ViewBag.PageNumber = _FilterEquipment.PageNo;
            ViewBag.OutOfNumber = 0;


            if (equipmentfilterlist.EquipmentList.Count() > 0)
            {
                ViewBag.OutOfNumber = equipmentfilterlist.TotalEquipmentCount.Counter;
            }

            if ((int)ViewBag.PageNumber * _FilterEquipment.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * _FilterEquipment.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / _FilterEquipment.PageSize);
            ViewBag.order = _FilterEquipment.order;
            return PartialView("_FilteredServiceList", equipmentfilterlist);
        }

        public JsonResult ServiceStatusChange(int Id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            Equipment eqp = _Util.Facade.EquipmentFacade.GetEquipmentById(Id);
            if (eqp.IsActive == true)
            {
                eqp.IsActive = false;
            }
            else
            {
                eqp.IsActive = true;
            }
            eqp.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            eqp.LastUpdatedBy = currentLoggedIn.UserId.ToString();
            result = _Util.Facade.EquipmentFacade.UpdateEquipment(eqp);
            return Json(result);
        }

        [Authorize]
        public PartialViewResult ProductListPartial(FilterEquipment _FilterEquipment)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            int PageLimit = 0;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            string newCookie = "";
            if (Request.Cookies[CookieKeys.DateViewFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.DateViewFilter].Value))
            {
                newCookie = Request.Cookies[CookieKeys.DateViewFilter].Value;
                newCookie = Server.UrlDecode(newCookie);
                var CookieVals = newCookie.Split(',');

                if (CookieVals.Length == 3)
                {
                    StartDate = CookieVals[0].ToDateTime();
                    EndDate = CookieVals[1].ToDateTime();
                }
            }
            _FilterEquipment.ActiveStatus = 1;
            _FilterEquipment.EmployeeRole = "";
            if (string.IsNullOrWhiteSpace(CurrentUser.UserRole) && CurrentUser.UserId != new Guid())
            {
                var objRole = _Util.Facade.EmployeeFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(CurrentUser.UserId, CurrentUser.CompanyId.Value);
                _FilterEquipment.EmployeeRole = objRole.Name;
            }
            else if (!string.IsNullOrWhiteSpace(CurrentUser.UserRole))
            {
                _FilterEquipment.EmployeeRole = CurrentUser.UserRole;
            }

            EquipmentListWithCountModel equipmentfilterlist = new EquipmentListWithCountModel();
            PageLimit = _Util.Facade.GlobalSettingsFacade.GetInventoryPagingLimit(CurrentUser.CompanyId.Value);
            _FilterEquipment.CompanyId = CurrentUser.CompanyId.Value;

            if (_FilterEquipment.PageNo == 0)
            {
                _FilterEquipment.PageNo = 1;
            }

            if (_FilterEquipment.PageSize < PageLimit)
            {
                _FilterEquipment.PageSize = PageLimit;
            }

            // equipmentfilterlist = _Util.Facade.InventoryFacade.GetEquipmentByFilter(_FilterEquipment);

            equipmentfilterlist = _Util.Facade.InventoryFacade.GetProductListByFilter(_FilterEquipment, StartDate, EndDate);

            if (equipmentfilterlist.EquipmentList.Count() == 0)
            {
                _FilterEquipment.PageNo = 1;
                //customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilter(filter);
            }

            ViewBag.PageNumber = _FilterEquipment.PageNo;
            ViewBag.OutOfNumber = 0;


            if (equipmentfilterlist.EquipmentList.Count() > 0)
            {
                ViewBag.OutOfNumber = equipmentfilterlist.TotalEquipmentCount.Counter;
            }

            if ((int)ViewBag.PageNumber * _FilterEquipment.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * _FilterEquipment.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / _FilterEquipment.PageSize);
            ViewBag.EquipmentTypeList = GetEquipmentTypeList();

            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            List<EquipmentClass> EquipmentClassDropDown = _Util.Facade.EquipmentFacade.GetAllEquipmentClassByCompanyId(CurrentUser.CompanyId.Value);

            if (EquipmentClassDropDown != null && EquipmentClassDropDown.Count > 0)
            {
                ListItems.AddRange(EquipmentClassDropDown.OrderBy(x => x.Name).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.Name.ToString(),
                               Value = x.Id.ToString()
                           }).ToList());
            }

            ViewBag.EquipmentClassTypeList = ListItems;

            ViewBag.EquipmentActiveStatus = _Util.Facade.LookupFacade.GetLookupByKey("EquipmentActiveStatus").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();

            ViewBag.StockStatus = _Util.Facade.LookupFacade.GetLookupByKey("StockStatus").Select(x =>
                      new SelectListItem()
                      {
                          Text = x.DisplayText.ToString(),
                          Value = x.DataValue.ToString()
                      }).ToList();
            return PartialView("_ProductListPartial", equipmentfilterlist);
        }

        [Authorize]
        public PartialViewResult FilterProductList(FilterEquipment _FilterEquipment)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            int PageLimit = 0;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            string newCookie = "";
            if (Request.Cookies[CookieKeys.DateViewFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.DateViewFilter].Value))
            {
                newCookie = Request.Cookies[CookieKeys.DateViewFilter].Value;
                newCookie = Server.UrlDecode(newCookie);
                var CookieVals = newCookie.Split(',');

                if (CookieVals.Length == 3)
                {
                    StartDate = CookieVals[0].ToDateTime();
                    EndDate = CookieVals[1].ToDateTime();
                }
            }
            _FilterEquipment.ActiveStatus = 1;
            _FilterEquipment.EmployeeRole = "";
            if (string.IsNullOrWhiteSpace(CurrentUser.UserRole) && CurrentUser.UserId != new Guid())
            {
                var objRole = _Util.Facade.EmployeeFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(CurrentUser.UserId, CurrentUser.CompanyId.Value);
                _FilterEquipment.EmployeeRole = objRole.Name;
            }
            else if (!string.IsNullOrWhiteSpace(CurrentUser.UserRole))
            {
                _FilterEquipment.EmployeeRole = CurrentUser.UserRole;
            }

            EquipmentListWithCountModel equipmentfilterlist = new EquipmentListWithCountModel();
            PageLimit = _Util.Facade.GlobalSettingsFacade.GetInventoryPagingLimit(CurrentUser.CompanyId.Value);
            _FilterEquipment.CompanyId = CurrentUser.CompanyId.Value;

            if (_FilterEquipment.PageNo == 0)
            {
                _FilterEquipment.PageNo = 1;
            }

            if (_FilterEquipment.PageSize < PageLimit)
            {
                _FilterEquipment.PageSize = PageLimit;
            }

            // equipmentfilterlist = _Util.Facade.InventoryFacade.GetEquipmentByFilter(_FilterEquipment);

            equipmentfilterlist = _Util.Facade.InventoryFacade.GetProductListByFilter(_FilterEquipment, StartDate, EndDate);

            if (equipmentfilterlist.EquipmentList.Count() == 0)
            {
                _FilterEquipment.PageNo = 1;
                //customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilter(filter);
            }

            ViewBag.PageNumber = _FilterEquipment.PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = _FilterEquipment.order;

            if (equipmentfilterlist.EquipmentList.Count() > 0)
            {
                ViewBag.OutOfNumber = equipmentfilterlist.TotalEquipmentCount.Counter;
            }

            if ((int)ViewBag.PageNumber * _FilterEquipment.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * _FilterEquipment.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / _FilterEquipment.PageSize);
            ViewBag.InventoryGridSettings = _Util.Facade.GridSettingsFacade.GetByKey("ProductServiceGrid", CurrentUser.CompanyId.Value);
            return PartialView("FilterProductList", equipmentfilterlist);
        }
        [Authorize]
        public PartialViewResult StockStatusPartialView()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            StockStatus stockCount = _Util.Facade.InventoryFacade.GetStockStatusByCompanyId(currentLoggedIn.CompanyId.Value);
            return PartialView("_StockStatusPartialView", stockCount);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteProductClass(int? id)
        {
            var result = false;
            if (id.HasValue)
            {
                result = _Util.Facade.InventoryFacade.DeleteEquipmentClass(id.Value);
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult AddInventoryEquipment(int? id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            Equipment model = new Equipment();
            if (id.HasValue)
            {
                var modelObject = _Util.Facade.EquipmentFacade.GetEquipmentByIdAndCompanyId(id.Value, currentLoggedIn.CompanyId.Value);
                if (modelObject != null)
                {
                    var companyId = modelObject.CompanyId;
                    var equipmentId = modelObject.EquipmentId;
                    Inventory InventoryObject = _Util.Facade.InventoryFacade.GetInventoryEquipmentQuantityAmountByEquipmentIdAndCompanyId(equipmentId, companyId);
                    if (InventoryObject != null)
                    {
                        if (InventoryObject.Quantity > 0)
                        {
                            modelObject.QtyOnHand = InventoryObject.Quantity;
                        }
                    }
                    model = modelObject;
                }
            }
            else
            {
                model.EquipmentId = Guid.NewGuid();
            }


            ViewBag.EquipmentTypeList = GetEquipmentTypeList();
            ViewBag.ManufacturerList = GetManufacturerList();
            ViewBag.SupplierList = GetSupplierList();

            return PartialView("_AddEquipment", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddInventoryEquipment(Equipment equipment)
        {

            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(result);
            }

            if (equipment.Id > 0)
            {
                #region update inventory equipment
                equipment.LastUpdatedBy = User.Identity.Name;
                equipment.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                equipment.IsActive = true;

                result = _Util.Facade.EquipmentFacade.UpdateEquipment(equipment);

                #region Inventory part
                //if (result && equipment.EquipmentClassId == 1)
                //{
                //    Inventory Inventory = new Inventory()
                //    {
                //        InventoryId = Guid.NewGuid(),
                //        EquipmentId = equipment.EquipmentId,
                //        CompanyId = equipment.CompanyId,
                //        Quantity = equipment.QtyOnHand,
                //        Type = equipment.EquipmentTypeId.ToString(),
                //        SupplierCost = equipment.SupplierCost,
                //        Cost = equipment.Cost,
                //        CreatedDate = DateTime.Now.UTCCurrentTime(),
                //        CreatedBy = User.Identity.Name,
                //        Retail = 0

                //    };

                //    result = _Util.Facade.InventoryFacade.InsertInventory(Inventory) > 0;
                //}
                //Inventory objInventory = new Inventory();
                #endregion

                if (result && equipment.EquipmentClassId == 1)
                {
                    #region Inventory
                    //objInventory = _Util.Facade.InventoryFacade.GetInventoryByEquipmentIdAndCompanyId(equipment.EquipmentId, currentLoggedIn.CompanyId.Value);
                    //if (objInventory != null)
                    //{
                    //    objInventory.Cost = equipment.Cost;
                    //    objInventory.SupplierCost = equipment.SupplierCost;
                    //    objInventory.Quantity = equipment.QtyOnHand;
                    //    result = _Util.Facade.InventoryFacade.UpdateInventoryByInventoryObject(objInventory);
                    //}
                    #endregion
                }
                else
                {
                    _Util.Facade.InventoryFacade.DeleteInventoryByEquipmentId(equipment.EquipmentId);
                }
                #endregion
            }
            else
            {
                if (equipment.EquipmentId == Guid.Empty)
                {
                    equipment.EquipmentId = Guid.NewGuid();
                }
                //equipment.EquipmentClassId = 1;
                equipment.CreatedDate = DateTime.Now.UTCCurrentTime();
                equipment.CompanyId = currentLoggedIn.CompanyId.Value;
                equipment.LastUpdatedBy = User.Identity.Name;
                equipment.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                equipment.IsActive = true;
                result = _Util.Facade.EquipmentFacade.InsertEquipment(equipment) > 0;

            }
            _Util.Facade.EquipmentFacade.DeleteServiceEquipmentsByServiceId(equipment.EquipmentId);
            if (equipment.ServiceEquipments != null && equipment.ServiceEquipments.Count > 0)
            {
                foreach (var item in equipment.ServiceEquipments)
                {

                    item.CreatedBy = currentLoggedIn.UserId;
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    item.ServiceId = equipment.EquipmentId;
                    item.CompanyId = currentLoggedIn.CompanyId.Value;
                    _Util.Facade.EquipmentFacade.InsertServiceEquipment(item);
                }
            }
            return Json(result);
        }

        [Authorize]
        public JsonResult DeleteInventory(Guid? id, int? deleteId)
        {
            if (id.HasValue && deleteId.HasValue)
            {
                var IsDeleted = _Util.Facade.InventoryFacade.DeleteEquipment(id.Value, deleteId.Value);
                if (IsDeleted == true)
                {
                    return Json(true, "Successfully Deleted");
                }
                else
                {
                    return Json(false, "Already exist in inventory");
                }
            }
            return Json(false, "Bad Request");
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddService(Equipment equipment)
        {
            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(result);
            }
            if (equipment.Id > 0)
            {
                equipment.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                equipment.LastUpdatedBy = User.Identity.Name;
                result = _Util.Facade.EquipmentFacade.UpdateEquipment(equipment);
            }
            else
            {
                equipment.EquipmentId = Guid.NewGuid();
                equipment.EquipmentClassId = 3;
                equipment.CreatedDate = DateTime.Now.UTCCurrentTime();
                equipment.CompanyId = currentLoggedIn.CompanyId.Value;
                equipment.IsActive = true;
                equipment.LastUpdatedBy = User.Identity.Name;
                equipment.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                result = _Util.Facade.EquipmentFacade.InsertEquipment(equipment) > 0;
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult AddNonInventory(int? id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            Equipment model;
            if (id.HasValue)
            {
                model = _Util.Facade.EquipmentFacade.GetEquipmentById(id.Value);
            }
            else
            {
                model = new Equipment();
            }

            ViewBag.EquipmentTypeList = GetEquipmentTypeList();
            ViewBag.ManufacturerList = GetManufacturerList();
            ViewBag.SupplierList = GetSupplierList();

            return PartialView("_AddNonInventory", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddNonInventory(Equipment equipment)
        {
            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(result);
            }
            if (equipment.Id > 0)
            {
                equipment.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                equipment.LastUpdatedBy = User.Identity.Name;
                result = _Util.Facade.EquipmentFacade.UpdateEquipment(equipment);
            }
            else
            {
                equipment.EquipmentId = Guid.NewGuid();
                equipment.EquipmentClassId = 2;
                equipment.CreatedDate = DateTime.Now.UTCCurrentTime();
                equipment.CompanyId = currentLoggedIn.CompanyId.Value;
                equipment.IsActive = true;
                equipment.LastUpdatedBy = User.Identity.Name;
                equipment.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                result = _Util.Facade.EquipmentFacade.InsertEquipment(equipment) > 0;
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult AddBundleServiceEquipment(int? id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(false);
            }
            Equipment model;
            if (id.HasValue)
            {
                model = new Equipment();
            }
            else
            {
                model = new Equipment();
            }
            return PartialView("_AddBundle", model);
        }

        [Authorize]
        public JsonResult MakeEquipmentServiceInactive(int? equipmentId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var result = false;
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(result);
            }
            if (equipmentId.HasValue)
            {
                result = _Util.Facade.EquipmentFacade.ConvertActiveEquipmentService(equipmentId.Value);
            }
            return Json(result);
        }

        [Authorize]
        public JsonResult MakeEquipmentServiceActive(int? equipmentId)
        {
            bool result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(result);
            }
            if (equipmentId.HasValue)
            {
                result = _Util.Facade.EquipmentFacade.ConvertInactiveEquipmentService(equipmentId.Value);
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult AdjustProductServiceQuantity(Guid? equipmentId, Guid? companyId)
        {
            Inventory model;
            model = new Inventory();

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(false);
            }

            if (equipmentId.HasValue && companyId.HasValue)
            {
                model = _Util.Facade.InventoryFacade.GetInventoryProductByProductIdAndCompanyId(equipmentId.Value, companyId.Value);
            }
            return PartialView("_AdjustProductServiceQuantity", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AdjustProductServiceQuantity(Inventory equipment)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(false);
            }
            if (equipment.Id > 0)
            {
                _Util.Facade.InventoryFacade.UpdateInventoryByInventoryObject(equipment);
            }

            return Json(true);
        }

        [Authorize]
        public ActionResult AddEquipmentServiceBundleView(int? Id, Guid? EquipmentId)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Equipment Model = new Equipment();

            if (Id.HasValue && Id.Value > 0)
            {
                Model = _Util.Facade.EquipmentFacade.GetEquipmentById(Id.Value);
                if (Model.CompanyId != CurrentUser.CompanyId.Value)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
            }
            else if (EquipmentId.HasValue)
            {
                Model = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(EquipmentId.Value, CurrentUser.CompanyId.Value);
            }


            if (Model == null)
            {
                Model = new Equipment();
            }
            #region ViewBags
            ViewBag.EquipmentTypeList = GetEquipmentTypeList();
            ViewBag.ManufacturerList = GetManufacturerList();
            ViewBag.SupplierList = GetSupplierList();
            ViewBag.GlobalSettings = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsByCompanyId(CurrentUser.CompanyId.Value).Where(x => x.Tag == "EquipmentUiSettings").OrderBy(it => it.OrderBy).ToList();

            #endregion

            return PartialView("_AddEquipmentServiceBundle", Model);
        }

        [Authorize]
        public ActionResult AdjustStartingValue(Guid? equipmentId, Guid? companyId)
        {
            CustomInventoryEquipmentModel model;
            model = new CustomInventoryEquipmentModel();

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(false);
            }

            if (equipmentId.HasValue && companyId.HasValue)
            {
                model = _Util.Facade.InventoryFacade.GetCustomInventoryEquipmentByEquipmentIdAndCompanyId(equipmentId.Value, companyId.Value);
            }
            return PartialView("_AdjustStartingValue", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AdjustStartingValue(CustomInventoryEquipmentModel customInventoryEquipmentModel)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(false);
            }
            else
            {
                if (customInventoryEquipmentModel.EquipmentAsOfDate != null && customInventoryEquipmentModel.EquipmentCost > 0)
                {
                    Equipment modelEquipment = new Equipment();
                    modelEquipment = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(customInventoryEquipmentModel.EquipmentId, currentLoggedIn.CompanyId.Value);
                    if (modelEquipment != null)
                    {
                        modelEquipment.AsOfDate = customInventoryEquipmentModel.EquipmentAsOfDate;
                        modelEquipment.Cost = customInventoryEquipmentModel.InventoryCost;
                        modelEquipment.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        modelEquipment.LastUpdatedBy = User.Identity.Name;
                        _Util.Facade.EquipmentFacade.UpdateEquipment(modelEquipment);
                    }
                    else
                    {

                    }
                }

                if (customInventoryEquipmentModel.InventoryQuantity > 0 && customInventoryEquipmentModel.InventoryCost > 0)
                {
                    Inventory modelInventory = new Inventory();
                    modelInventory = _Util.Facade.InventoryFacade.GetInventoryByEquipmentIdAndCompanyId(customInventoryEquipmentModel.EquipmentId, currentLoggedIn.CompanyId.Value);
                    if (modelInventory != null)
                    {
                        modelInventory.Quantity = customInventoryEquipmentModel.InventoryQuantity;
                        modelInventory.Cost = customInventoryEquipmentModel.InventoryCost;
                        _Util.Facade.InventoryFacade.UpdateInventoryByInventoryObject(modelInventory);
                    }
                    else
                    {

                    }

                }
            }
            return Json(true);
        }

        [Authorize]
        public ActionResult DuplicateEquipment(Guid? EquipmentId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Equipment modelObject = new Equipment();
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(false);
            }
            else
            {
                modelObject = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(EquipmentId.Value, currentLoggedIn.CompanyId.Value);
                if (modelObject != null)
                {
                    modelObject.Name = modelObject.Name + " - Copy";
                    modelObject.Id = 0;
                    var equipmentId = modelObject.EquipmentId;
                    var companyId = modelObject.CompanyId;
                    var InventoryObject = _Util.Facade.InventoryFacade.GetInventoryEquipmentQuantityAmountByEquipmentIdAndCompanyId(equipmentId, companyId);
                    if (InventoryObject != null)
                    {
                        modelObject.QtyOnHand = InventoryObject.Quantity;
                    }
                }
            }

            ViewBag.EquipmentTypeList = GetEquipmentTypeList();
            ViewBag.ManufacturerList = GetManufacturerList();
            ViewBag.SupplierList = GetSupplierList();

            return PartialView("_AddEquipment", modelObject);
        }

        [Authorize]
        public ActionResult DuplicateService(Guid? ServiceId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Equipment modelObject = new Equipment();
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(false);
            }
            else
            {
                modelObject = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(ServiceId.Value, currentLoggedIn.CompanyId.Value);
                if (modelObject != null)
                {
                    modelObject.Name = modelObject.Name + " - Copy";
                    modelObject.Id = 0;
                }
            }
            if (modelObject.IsWarrenty == null)
            {
                modelObject.IsWarrenty = false;
            }
            ViewBag.EquipmentTypeList = GetEquipmentTypeList();
            ViewBag.ManufacturerList = GetManufacturerList();
            ViewBag.SupplierList = GetSupplierList();
            return PartialView("_AddService", modelObject);
        }

        [Authorize]
        public ActionResult DuplicateNonInventory(Guid? ServiceId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Equipment modelObject = new Equipment();
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(false);
            }
            else
            {
                modelObject = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(ServiceId.Value, currentLoggedIn.CompanyId.Value);
                if (modelObject != null)
                {
                    modelObject.Name = modelObject.Name + " - Copy";
                    modelObject.Id = 0;
                }
            }

            ViewBag.EquipmentTypeList = GetEquipmentTypeList();
            ViewBag.ManufacturerList = GetManufacturerList();
            ViewBag.SupplierList = GetSupplierList();

            return PartialView("_AddNonInventory", modelObject);
        }

        [Authorize]
        public ActionResult UploadExcel()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            HttpPostedFileBase httpPostedFileBase = Request.Files["InventoryFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.InventoryFiles"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += "/";

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += httpPostedFileBase.FileName;

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
                return RedirectToAction("ExcelDataFile");
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath }, "excel");
        }

        [Authorize]
        public ActionResult ExcelDataFile()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn != null)
            {
                string tempFolderNameExcel = ConfigurationManager.AppSettings["File.InventoryFiles"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(currentLoggedIn.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                tempFolderNameExcel = string.Format(tempFolderNameExcel, comname);
                string folderDirectoryExcel = Server.MapPath("~/" + tempFolderNameExcel);
                try
                {
                    if (Directory.Exists(folderDirectoryExcel))
                    {
                        DirectoryInfo di = new DirectoryInfo(folderDirectoryExcel);
                        FileInfo[] ExcelFile = di.GetFiles("*.xlsx");

                        if (ExcelFile.Length > 0)
                        {
                            Excel.Application xlApp = new Excel.Application();
                            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(ExcelFile[0].FullName);
                            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                            Excel.Range xlRange = xlWorksheet.UsedRange;

                            int rowCount = xlRange.Rows.Count;
                            int colCount = xlRange.Columns.Count;

                            for (int i = 2; i <= rowCount; i++)
                            {
                                Equipment equipment = new Equipment();
                                for (int j = 1; j <= colCount; j++)
                                {
                                    if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                                    {
                                        var header = xlRange.Cells[1, j].Value2.ToString();
                                        var value = xlRange.Cells[i, j].Value2.ToString();

                                        if (header == "EquipmentId")
                                        {
                                            if (!string.IsNullOrEmpty(value))
                                                value = Guid.NewGuid().ToString();
                                        }
                                        else if (header == "CompanyId")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                                value = currentLoggedIn.CompanyId.Value;
                                        }
                                        if (header == "ManufacturerId")
                                        {
                                            value = Convert.ToInt32(value);
                                        }
                                        else if (header == "SupplierId")
                                        {
                                            value = Convert.ToInt32(value);
                                        }
                                        else if (header == "EquipmentTypeId")
                                        {
                                            value = Convert.ToInt32(value);
                                        }
                                        else if (header == "EquipmentClassId")
                                        {
                                            value = Convert.ToInt32(value);
                                        }
                                        else if (header == "Point")
                                        {
                                            if (value.ToLower() == "null")
                                            {
                                                value = 0.0;
                                            }
                                        }
                                        else if (header == "SupplierCost")
                                        {
                                            if (value.ToLower() == "null")
                                            {
                                                value = 0.0;
                                            }

                                        }
                                        else if (header == "Cost")
                                        {
                                            if (value.ToLower() == "null")
                                            {
                                                value = 0.0;
                                            }
                                            else
                                            {
                                                float f = float.Parse(value);
                                                value = Convert.ToDouble(value);
                                            }

                                        }
                                        else if (header == "Retail")
                                        {
                                            if (value.ToLower() == "null")
                                            {
                                                value = 0.0;
                                            }
                                            else
                                            {
                                                value = Convert.ToDouble(value);
                                            }

                                        }
                                        else if (header == "EqOrder")
                                        {
                                            if (value.ToLower() == "null")
                                            {
                                                value = 0;
                                            }
                                        }

                                        else if (header == "reorderpoint")
                                        {
                                            if (value.ToLower() == "null")
                                            {
                                                value = 0;
                                            }
                                            else
                                            {
                                                value = Convert.ToInt32(value);
                                            }

                                        }
                                        else if (header == "AsOfDate")
                                        {
                                            if (value.ToLower() == "null")
                                            {
                                                value = null;
                                            }
                                            else
                                            {
                                                value = Convert.ToDateTime(value);
                                            }
                                        }
                                        else if (header == "IsActive")
                                        {
                                            bool active = bool.Parse(value);
                                            value = active;
                                        }
                                        else if (header == "CreatedDate")
                                        {
                                            if (value.ToLower() == "null")
                                            {
                                                value = DateTime.Now.UTCCurrentTime();
                                            }

                                        }
                                        else if (header == "LastUpdatedDate")
                                        {
                                            if (value.ToLower() == "null")
                                            {
                                                value = DateTime.Now.UTCCurrentTime();
                                            }

                                        }
                                        if (equipment.GetType().GetProperty(header) != null)
                                        {
                                            equipment.GetType().GetProperty(header).SetValue(equipment, value);
                                        }
                                    }
                                    else
                                    {
                                        var header = xlRange.Cells[1, j].Value2.ToString();
                                        if (header == "CompanyId")
                                        {
                                            var comId = currentLoggedIn.CompanyId.Value;
                                            if (equipment.GetType().GetProperty(header) != null)
                                            {
                                                equipment.GetType().GetProperty(header).SetValue(equipment, comId);
                                            }
                                        }
                                        else if (header == "EquipmentId")
                                        {

                                            var valueGuid = Guid.NewGuid();
                                            if (equipment.GetType().GetProperty(header) != null)
                                            {
                                                equipment.GetType().GetProperty(header).SetValue(equipment, valueGuid);
                                            }
                                        }
                                        else if (header == "CreatedDate")
                                        {
                                            var create = DateTime.Now.UTCCurrentTime();
                                            if (equipment.GetType().GetProperty(header) != null)
                                            {
                                                equipment.GetType().GetProperty(header).SetValue(equipment, create);
                                            }
                                        }
                                        else if (header == "LastUpdatedDate")
                                        {
                                            var last = DateTime.Now.UTCCurrentTime();
                                            if (equipment.GetType().GetProperty(header) != null)
                                            {
                                                equipment.GetType().GetProperty(header).SetValue(equipment, last);
                                            }
                                        }
                                        else if (header == "ManufacturerId")
                                        {
                                            var manuid = 0;
                                            if (equipment.GetType().GetProperty(header) != null)
                                            {
                                                equipment.GetType().GetProperty(header).SetValue(equipment, manuid);
                                            }
                                        }
                                        else if (header == "SupplierId")
                                        {
                                            var suppid = 0;
                                            if (equipment.GetType().GetProperty(header) != null)
                                            {
                                                equipment.GetType().GetProperty(header).SetValue(equipment, suppid);
                                            }
                                        }
                                        else if (header == "Point")
                                        {
                                            var point = 0.0;
                                            if (equipment.GetType().GetProperty(header) != null)
                                            {
                                                equipment.GetType().GetProperty(header).SetValue(equipment, point);
                                            }
                                        }
                                    }
                                }
                                equipment.LastUpdatedBy = User.Identity.Name;
                                equipment.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                _Util.Facade.EquipmentFacade.InsertEquipment(equipment);

                            }

                            object misValue = System.Reflection.Missing.Value;
                            xlWorkbook.Close(true, misValue, misValue);
                            xlApp.Quit();

                            Marshal.ReleaseComObject(xlWorksheet);
                            Marshal.ReleaseComObject(xlWorkbook);
                            Marshal.ReleaseComObject(xlApp);

                        }

                    }
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }

            }

            return RedirectToAction("Index");
        }

        private List<SelectListItem> GetManufacturerList()
        {
            var currentLoggedIn = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SelectListItem> ManufacturerList = new List<SelectListItem>();
            ManufacturerList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            List<Manufacturer> ManufacturerDropDown = _Util.Facade.ManufacturerFacade.GetAllManufacturerByCompanyId(currentLoggedIn.CompanyId.Value);
            ViewBag.ManufacturerList = ManufacturerDropDown;
            if (ManufacturerDropDown != null && ManufacturerDropDown.Count > 0)
            {
                ManufacturerList.AddRange(ManufacturerDropDown.OrderBy(x => x.Name).Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.Name.ToString(),
                                 Value = x.Id.ToString()
                             }).ToList());
            }

            return ManufacturerList;
        }

        private List<SelectListItem> GetSupplierList()
        {
            var currentLoggedIn = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SelectListItem> SupplierList = new List<SelectListItem>();
            SupplierList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            List<Supplier> SupplierDropDown = _Util.Facade.SupplierFacade.GetAllSupplierByCompanyId(currentLoggedIn.CompanyId.Value);
            ViewBag.SupplierList = SupplierDropDown;
            if (SupplierDropDown != null && SupplierDropDown.Count > 0)
            {
                SupplierList.AddRange(SupplierDropDown.OrderBy(x => x.Id.ToString() != "-1").ThenBy(x => x.CompanyName).Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.CompanyName.ToString(),
                                 Value = x.Id.ToString()
                             }).ToList());
            }


            return SupplierList;
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
        public PartialViewResult InventoryGridSettings()
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<GridSetting> InventoryGridSettings = _Util.Facade.GridSettingsFacade.GetAllByKey("InventoryGrid", CurrentLoggedInUser.CompanyId.Value);
            return PartialView("_InventoryGridSettings", InventoryGridSettings);
        }

        [Authorize]
        public JsonResult UpdateInventoryGridSettings(List<GridSetting> InventoryGridSettings)
        {
            var result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (InventoryGridSettings.Count > 0)
            {
                foreach (var item in InventoryGridSettings)
                {
                    item.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                    result = _Util.Facade.GridSettingsFacade.UpdateGridSettings(item);
                }
            }
            return Json(result);
        }

        [Authorize]
        public PartialViewResult ProductServiceGridSettings()
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<GridSetting> ProductServiceGridSettings = _Util.Facade.GridSettingsFacade.GetAllByKey("ProductServiceGrid", CurrentLoggedInUser.CompanyId.Value);
            return PartialView("_ProductServiceGridSettings", ProductServiceGridSettings);
        }

        [Authorize]
        public JsonResult UpdateProductServiceGridSettings(List<GridSetting> ProductServiceGridSettings)
        {
            var result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (ProductServiceGridSettings.Count > 0)
            {
                foreach (var item in ProductServiceGridSettings)
                {
                    item.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                    result = _Util.Facade.GridSettingsFacade.UpdateGridSettings(item);
                }
            }
            return Json(result);
        }


        [Authorize]
        public ActionResult BranchInventoryPartial()
        {

            if (!base.IsPermitted(UserPermissions.InventoryPermissions.InventoryBranchTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView("_BranchInventoryPartial");
        }
        [Authorize]
        public ActionResult BranchListInventoryPartial()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<CompanyBranch> BranchList = new List<CompanyBranch>();
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            BranchList = _Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(currentLoggedIn.CompanyId.Value);
            return PartialView("_BranchListInventoryPartial", BranchList);
        }
        [Authorize]
        public PartialViewResult BranchEquipmentsListPartial()
        {
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.InventoryTechTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.EquipmentTypeList = GetEquipmentTypeList();

            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            List<EquipmentClass> EquipmentClassDropDown = _Util.Facade.EquipmentFacade.GetAllEquipmentClassByCompanyId(CurrentUser.CompanyId.Value);

            if (EquipmentClassDropDown != null && EquipmentClassDropDown.Count > 0)
            {
                ListItems.AddRange(EquipmentClassDropDown.OrderBy(x => x.Name).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.Name.ToString(),
                               Value = x.Id.ToString()
                           }).ToList());
            }

            ViewBag.EquipmentClassTypeList = ListItems;

            ViewBag.EquipmentActiveStatus = _Util.Facade.LookupFacade.GetLookupByKey("EquipmentActiveStatus").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();

            ViewBag.StockStatus = _Util.Facade.LookupFacade.GetLookupByKey("StockStatus").Select(x =>
                      new SelectListItem()
                      {
                          Text = x.DisplayText.ToString(),
                          Value = x.DataValue.ToString()
                      }).ToList();


            return PartialView("_BranchEuipmentList");
        }
        [Authorize]
        public PartialViewResult BranchFilterEquipmentsListPartial(FilterEquipment _FilterEquipment)
        {
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.InventoryTechTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (_FilterEquipment.BranchId == 0)
            {
                var BranchInfo = _Util.Facade.EmployeeFacade.GetUserBranchByCompanyIdAndUserId(currentLoggedIn.CompanyId.Value, currentLoggedIn.UserId);
                if (BranchInfo != null)
                {
                    _FilterEquipment.BranchId = BranchInfo.BranchId;
                }
            }
            int PageLimit = 0;
            _FilterEquipment.EmployeeRole = "";
            if (string.IsNullOrWhiteSpace(currentLoggedIn.UserRole) && currentLoggedIn.UserId != new Guid())
            {
                var objEmpRole = _Util.Facade.EmployeeFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(currentLoggedIn.UserId, currentLoggedIn.CompanyId.Value);
                _FilterEquipment.EmployeeRole = objEmpRole.Name;
            }
            else if (!string.IsNullOrWhiteSpace(currentLoggedIn.UserRole))
            {
                _FilterEquipment.EmployeeRole = currentLoggedIn.UserRole;
            }
            PageLimit = _Util.Facade.GlobalSettingsFacade.GetInventoryPagingLimit(currentLoggedIn.CompanyId.Value);
            EquipmentListWithCountModel equipmentfilterlist = new EquipmentListWithCountModel();
            ViewBag.InventoryGridSettings = _Util.Facade.GridSettingsFacade.GetByKey("InventoryGrid", currentLoggedIn.CompanyId.Value);
            _FilterEquipment.CompanyId = currentLoggedIn.CompanyId.Value;

            if (_FilterEquipment.PageNo == 0)
            {
                _FilterEquipment.PageNo = 1;
            }

            if (_FilterEquipment.PageSize < PageLimit)
            {
                _FilterEquipment.PageSize = PageLimit;
            }

            equipmentfilterlist = _Util.Facade.InventoryFacade.GetEquipmentByFilterBranch(_FilterEquipment);

            if (equipmentfilterlist.EquipmentList.Count() == 0)
            {
                _FilterEquipment.PageNo = 1;
                //customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilter(filter);
            }

            ViewBag.PageNumber = _FilterEquipment.PageNo;
            ViewBag.OutOfNumber = 0;


            if (equipmentfilterlist.EquipmentList.Count() > 0)
            {
                ViewBag.OutOfNumber = equipmentfilterlist.TotalEquipmentCount.Counter;
            }

            if ((int)ViewBag.PageNumber * _FilterEquipment.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * _FilterEquipment.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / _FilterEquipment.PageSize);
            return PartialView("_BranchFilteredEquipmentList", equipmentfilterlist);
        }
        [Authorize]
        public ActionResult BranchdetailListWithInventoryPartial(Guid? EmployeeId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            TechnicianInventoryCustomModel model = new TechnicianInventoryCustomModel();
            model.EmployeeModel = new Employee();
            model.TechnicianInventoryListModel = new List<TechnicianInventory>();
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (EmployeeId.HasValue && EmployeeId.Value != new Guid())
            {
                var TechnicianIsInCompany = _Util.Facade.EmployeeFacade.CheckTechnicianIsInCompany(EmployeeId.Value, currentLoggedIn.CompanyId.Value);
                if (!TechnicianIsInCompany)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model.EmployeeModel = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmployeeId.Value);
                model.TechnicianInventoryListModel = _Util.Facade.InventoryFacade.GetAllTechnicianInventoryByEmployeeIdAndCompanyId(EmployeeId.Value, currentLoggedIn.CompanyId.Value);
            }
            return PartialView("_BranchdetailListWithInventoryPartial", model);
        }

        [Authorize]
        public ActionResult TechnicianInventoryPartial()
        {

            if (!base.IsPermitted(UserPermissions.InventoryPermissions.InventoryTechTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView("_TechnicianInventoryPartial");
        }
        [Authorize]
        public ActionResult TechnicianListInventoryPartial(string searchtext)

        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<Employee> TechnicianList = new List<Employee>();
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (base.IsPermitted(UserPermissions.InventoryPermissions.TechAllInventory))
            {
                if (currentLoggedIn.UserRole.ToLower().IndexOf("technician") != 0 && currentLoggedIn.UserRole.ToLower().IndexOf("installation") != 0)
                {
                    TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTagAndSearch(currentLoggedIn.CompanyId.Value, searchtext, LabelHelper.UserTags.Technicians, new Guid());
                    ViewBag.TechnicianList = TechnicianList.Select(x =>
                                               new SelectListItem()
                                               {
                                                   Text = x.FirstName + " " + x.LastName,
                                                   Value = x.UserId.ToString(),
                                                   Selected = true
                                               }).ToList();
                }
                else
                {
                    TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTagAndSearch(currentLoggedIn.CompanyId.Value, searchtext, LabelHelper.UserTags.Technicians, new Guid());
                    ViewBag.TechnicianList = TechnicianList.Select(x =>
                                               new SelectListItem()
                                               {
                                                   Text = x.FirstName + " " + x.LastName,
                                                   Value = x.UserId.ToString(),
                                                   Selected = true
                                               }).ToList();
                }
            }
            else if (currentLoggedIn.UserRole == LabelHelper.UserTypes.Technician || currentLoggedIn.UserRole == LabelHelper.UserTypes.Installation)
            {
                TechnicianList = _Util.Facade.EmployeeFacade.GetSingleTechnicianList(currentLoggedIn.UserId);
            }
            return PartialView("_TechnicianListInventoryPartial", TechnicianList);
        }

        [Authorize]
        public PartialViewResult TechEquipmentsListPartial()
        {
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.InventoryTechTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.EquipmentTypeList = GetEquipmentTypeList();

            List<SelectListItem> ListItems = new List<SelectListItem>();
            ListItems.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            ListItems.AddRange(_Util.Facade.EquipmentFacade
                .GetAllEquipmentClassByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.Name.ToString(),
                               Value = x.Id.ToString()
                           }).ToList());
            ViewBag.EquipmentClassTypeList = ListItems;

            ViewBag.EquipmentActiveStatus = _Util.Facade.LookupFacade.GetLookupByKey("EquipmentActiveStatus").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();

            ViewBag.StockStatus = _Util.Facade.LookupFacade.GetLookupByKey("StockStatus").Select(x =>
                      new SelectListItem()
                      {
                          Text = x.DisplayText.ToString(),
                          Value = x.DataValue.ToString()
                      }).ToList();


            return PartialView("_TechEuipmentList");
        }
        [Authorize]
        public ActionResult TechFilterEquipmentsListPartial(FilterEquipment _FilterEquipment)
        {
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.InventoryTechTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (_FilterEquipment.UserId == Guid.Empty)
            {
                _FilterEquipment.UserId = currentLoggedIn.UserId;
            }
            int PageLimit = 0;
            _FilterEquipment.EmployeeRole = "";
            if (string.IsNullOrWhiteSpace(currentLoggedIn.UserRole) && currentLoggedIn.UserId != new Guid())
            {
                var objEmpRole = _Util.Facade.EmployeeFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(currentLoggedIn.UserId, currentLoggedIn.CompanyId.Value);
                _FilterEquipment.EmployeeRole = objEmpRole.Name;
            }
            else if (!string.IsNullOrWhiteSpace(currentLoggedIn.UserRole))
            {
                _FilterEquipment.EmployeeRole = currentLoggedIn.UserRole;
            }
            PageLimit = _Util.Facade.GlobalSettingsFacade.GetInventoryPagingLimit(currentLoggedIn.CompanyId.Value);
            EquipmentListWithCountModel equipmentfilterlist = new EquipmentListWithCountModel();
            ViewBag.InventoryGridSettings = _Util.Facade.GridSettingsFacade.GetByKey("InventoryGrid", currentLoggedIn.CompanyId.Value);
            _FilterEquipment.CompanyId = currentLoggedIn.CompanyId.Value;

            if (_FilterEquipment.PageNo == 0)
            {
                _FilterEquipment.PageNo = 1;
            }

            if (_FilterEquipment.PageSize < PageLimit)
            {
                _FilterEquipment.PageSize = PageLimit;
            }
            if (_FilterEquipment.GetReport.HasValue && _FilterEquipment.GetReport.Value == true)
            {
                DataTable dt = _Util.Facade.InventoryFacade.GetEquipmentByFilterTechDownload(_FilterEquipment);
                dt.Columns["cost"].ColumnName = "vendor cost";
                bool hasPermission = PermissionHelper.IsPermitted(UserPermissions.InventoryPermissions.TechInvShowHideVendorCost);


                if (!hasPermission)
                {
                    // Find and remove the "cost" column from the DataTable
                    if (dt.Columns.Contains("vendor cost"))
                    {
                        dt.Columns.Remove("vendor cost");
                    }
                }
                int[] colarray = { 6 };
                int[] rowarray = { dt.Rows.Count + 2 };
                return MakeExcelFromDataTable(dt, "Tech Inventory", rowarray, colarray);
            }
            equipmentfilterlist = _Util.Facade.InventoryFacade.GetEquipmentByFilterTech(_FilterEquipment);

            if (equipmentfilterlist.EquipmentList.Count() == 0)
            {
                _FilterEquipment.PageNo = 1;
                //customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilter(filter);
            }

            ViewBag.PageNumber = _FilterEquipment.PageNo;
            ViewBag.OutOfNumber = 0;


            if (equipmentfilterlist.EquipmentList.Count() > 0)
            {
                ViewBag.OutOfNumber = equipmentfilterlist.TotalEquipmentCount.Counter;
            }

            if ((int)ViewBag.PageNumber * _FilterEquipment.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * _FilterEquipment.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / _FilterEquipment.PageSize);
            return PartialView("_TechFilteredEquipmentList", equipmentfilterlist);
        }

        [Authorize]
        public ActionResult TechniciandetailListWithInventoryPartial(Guid? EmployeeId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            TechnicianInventoryCustomModel model = new TechnicianInventoryCustomModel();
            model.EmployeeModel = new Employee();
            model.TechnicianInventoryListModel = new List<TechnicianInventory>();
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (EmployeeId.HasValue && EmployeeId.Value != new Guid())
            {
                var TechnicianIsInCompany = _Util.Facade.EmployeeFacade.CheckTechnicianIsInCompany(EmployeeId.Value, currentLoggedIn.CompanyId.Value);
                if (!TechnicianIsInCompany)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model.EmployeeModel = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmployeeId.Value);
                model.TechnicianInventoryListModel = _Util.Facade.InventoryFacade.GetAllTechnicianInventoryByEmployeeIdAndCompanyId(EmployeeId.Value, currentLoggedIn.CompanyId.Value);
            }
            return PartialView("_TechniciandetailListWithInventoryPartial", model);
        }

        [Authorize]
        public ActionResult AddNewTechnicianInventoryProduct(int? Id, Guid TechnicianId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            TechnicianInventory model = new TechnicianInventory();
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (Id.HasValue)
            {
                if (Id.Value > 0 && TechnicianId != new Guid())
                {
                    model = _Util.Facade.InventoryFacade.GetTechnicianInventoryByIdAndCompanyId(Id.Value, currentLoggedIn.CompanyId.Value, TechnicianId);
                }
            }
            if (TechnicianId != new Guid())
            {
                ViewBag.TechnicianIdForAddNewEquipment = TechnicianId;
            }
            if (TechnicianId == new Guid())
            {
                ViewBag.TechnicianIdForAddNewEquipment = new Guid();
            }
            #region EquipmentList In viewbag
            List<SelectListItem> EquipmentList = new List<SelectListItem>();
            EquipmentList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            List<Equipment> EquipmentDropDown = _Util.Facade.EquipmentFacade.GetAllEquipmentsByCompanyId(currentLoggedIn.CompanyId.Value);
            ViewBag.EquipmentList = EquipmentDropDown;
            if (EquipmentDropDown != null && EquipmentDropDown.Count > 0)
            {
                EquipmentList.AddRange(EquipmentDropDown.OrderBy(x => x.Name).Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]" + " [ " + x.Retail.ToString() + " ]",
                                 Value = x.EquipmentId.ToString()
                             }).ToList());
            }


            ViewBag.EquipmentList = EquipmentList;
            #endregion

            return PartialView("_AddNewTechnicianInventoryProduct", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddNewTechnicianInventoryProduct(TechnicianInventory _TechnicianInventory)
        {
            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            _TechnicianInventory.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            _TechnicianInventory.LastUpdatedBy = User.Identity.Name;

            if (currentLoggedIn == null)
            {
                return Json(result);
            }

            if (_TechnicianInventory.Id > 0)
            {
                var OldModel = _Util.Facade.InventoryFacade.GetTechnicianInventoryByIdAndCompanyId(_TechnicianInventory.Id, currentLoggedIn.CompanyId.Value, _TechnicianInventory.TechnicianId);
                if (OldModel != null)
                {
                    if (OldModel.EquipmentId == _TechnicianInventory.EquipmentId)
                    {
                        var InventoryItem = _Util.Facade.InventoryFacade.GetInventoryByEquipmentIdAndCompanyId(_TechnicianInventory.EquipmentId, currentLoggedIn.CompanyId.Value);
                        if (InventoryItem != null)
                        {
                            var ChangedQuantity = 0;
                            if (OldModel.Quantity < _TechnicianInventory.Quantity)
                            {
                                ChangedQuantity = _TechnicianInventory.Quantity - OldModel.Quantity;
                                InventoryItem.Quantity -= ChangedQuantity;
                                _Util.Facade.InventoryFacade.UpdateInventoryByInventoryObject(InventoryItem);
                            }
                            if (OldModel.Quantity > _TechnicianInventory.Quantity)
                            {
                                ChangedQuantity = OldModel.Quantity - _TechnicianInventory.Quantity;
                                InventoryItem.Quantity += ChangedQuantity;
                                _Util.Facade.InventoryFacade.UpdateInventoryByInventoryObject(InventoryItem);
                            }
                        }
                    }
                    if (OldModel.EquipmentId != _TechnicianInventory.EquipmentId)
                    {
                        var OldInventoryItem = _Util.Facade.InventoryFacade.GetInventoryByEquipmentIdAndCompanyId(OldModel.EquipmentId, currentLoggedIn.CompanyId.Value);
                        if (OldInventoryItem != null)
                        {
                            OldInventoryItem.Quantity += OldModel.Quantity;
                            _Util.Facade.InventoryFacade.UpdateInventoryByInventoryObject(OldInventoryItem);
                        }
                        var NewInventoryItem = _Util.Facade.InventoryFacade.GetInventoryByEquipmentIdAndCompanyId(_TechnicianInventory.EquipmentId, currentLoggedIn.CompanyId.Value);
                        if (NewInventoryItem != null)
                        {
                            NewInventoryItem.Quantity -= _TechnicianInventory.Quantity;
                            _Util.Facade.InventoryFacade.UpdateInventoryByInventoryObject(NewInventoryItem);
                        }
                    }

                    OldModel.EquipmentId = _TechnicianInventory.EquipmentId;
                    OldModel.Quantity = _TechnicianInventory.Quantity;
                    OldModel.LastUpdatedBy = _TechnicianInventory.LastUpdatedBy;
                    OldModel.LastUpdatedDate = _TechnicianInventory.LastUpdatedDate;
                    result = _Util.Facade.InventoryFacade.UpdateTechnicianInventoryEquipment(OldModel);
                }
            }
            else
            {
                var InventoryItem = _Util.Facade.InventoryFacade.GetInventoryByEquipmentIdAndCompanyId(_TechnicianInventory.EquipmentId, currentLoggedIn.CompanyId.Value);
                if (InventoryItem != null)
                {
                    InventoryItem.Quantity -= _TechnicianInventory.Quantity;
                    _Util.Facade.InventoryFacade.UpdateInventoryByInventoryObject(InventoryItem);

                    TechnicianInventory ObjInsertDB = new TechnicianInventory()
                    {
                        CompanyId = currentLoggedIn.CompanyId.Value,
                        TechnicianId = _TechnicianInventory.TechnicianId,
                        EquipmentId = _TechnicianInventory.EquipmentId,
                        Quantity = _TechnicianInventory.Quantity,
                        LastUpdatedBy = _TechnicianInventory.LastUpdatedBy,
                        LastUpdatedDate = _TechnicianInventory.LastUpdatedDate
                    };
                    result = _Util.Facade.InventoryFacade.InsertTechnicianInventoryEquipment(ObjInsertDB) > 0;
                }
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteProduct(int? id)
        {
            var result = false;
            string message = "";
            if (id.HasValue)
            {
                var equipobj = _Util.Facade.EquipmentFacade.GetEquipmentById(id.Value);
                if (equipobj != null)
                {
                    var invoicedetailobj = _Util.Facade.InvoiceFacade.GetAllInvoiceDetailEquipmentByEquipmentId(equipobj.EquipmentId);
                    var AppointmentEquipmentobj = _Util.Facade.CustomerAppoinmentFacade.GetAllAppointmentEquipmentByEquipmentId(equipobj.EquipmentId);
                    if (invoicedetailobj.Count > 0 || AppointmentEquipmentobj.Count > 0)
                    {
                        message = "This euipment or product already used";
                    }
                    else
                    {
                        _Util.Facade.EquipmentFacade.DeleteProduct(id.Value);
                        result = true;
                    }
                }
            }
            return Json(new { result = result, message = message });
        }

        [Authorize]
        public ActionResult EquipmentDetailPartial(int Id, string showall)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateEquipment Model = new CreateEquipment();
            Model.Equipment = _Util.Facade.EquipmentFacade.GetEquipmentById(Id);
            if (Model.Equipment == null)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
            Model.InventoryHistoryList = _Util.Facade.InventoryFacade.GetTop50InventoryListByEquipmentIdAndCompanyId(Model.Equipment.EquipmentId, CurrentUser.CompanyId.Value);

            if (showall == "yes")
            {
                Model.InventoryHistoryList = _Util.Facade.InventoryFacade.GetInventoryListByEquipmentIdAndCompanyId(Model.Equipment.EquipmentId, CurrentUser.CompanyId.Value);

            }
            ViewBag.showall = showall;
            Model.ServiceEquipmentList = _Util.Facade.EquipmentFacade.GetEquipmentServiceListByServiceId(Model.Equipment.EquipmentId);
            Model.ListInventoryTech = _Util.Facade.EquipmentFacade.GetInventoryTechByEquipmentId(Model.Equipment.EquipmentId);
            #region equipment grid UI settings
            List<GridSetting> EquipmentUiSetting = new List<GridSetting>();
            EquipmentUiSetting = _Util.Facade.GridSettingsFacade.GetAllByKey("EquipmentGrid", CurrentUser.CompanyId.Value);
            if (EquipmentUiSetting.Count > 0)
            {
                EquipmentUiSetting = EquipmentUiSetting.OrderBy(x => x.OrderBy).Where(x => x.FormActive == true).ToList();
            }

            ViewBag.EquipmentUiSetting = EquipmentUiSetting;
            #endregion
            return View("_EquipmentDetailPage", Model);
        }

        [Authorize]
        public ActionResult TechEquipmentDetailPartial(Guid Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateEquipment Model = new CreateEquipment();
            Model.ListInventoryTech = _Util.Facade.EquipmentFacade.GetInventoryTechByEquipmentId(Id);
            return View(Model);
        }

        [Authorize]
        public ActionResult LocEquipmentDetailPartial(Guid Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateEquipment Model = new CreateEquipment();
            Model.ListInventoryTech = _Util.Facade.EquipmentFacade.GetInventoryLocByEquipmentId(Id);
            return View(Model);
        }

        [Authorize]
        public ActionResult OpenServiceOptions(Guid EquipmentId)
        {
            if (EquipmentId == Guid.Empty)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
            ServiceDetailInfoView Model = _Util.Facade.InventoryFacade.GetServiceDetailInfoViewByServiceId(EquipmentId);
            if (Model == null)
            {
                Model = new ServiceDetailInfoView();
                Model.ServiceId = EquipmentId;
            }


            return View(Model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteServiceOption(Guid ServiceInfoId)
        {
            ServiceDetailInfo SDI = _Util.Facade.InventoryFacade.GetServiceDetailInfoByServiceInfoId(ServiceInfoId);
            if (SDI == null)
            {
                return Json(new { result = false, message = "Not found." });
            }
            _Util.Facade.InventoryFacade.DeleteServiceDetailInfoById(SDI.Id);
            _Util.Facade.InventoryFacade.DeleteServiceMapByServiceInfoId(ServiceInfoId);


            return Json(new { result = true, message = "Service option deleted successfully." });
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteServiceMap(int Id, Guid EquipmentId)
        {
            ServiceMap SM = _Util.Facade.InventoryFacade.GetserviceMapById(Id);
            if (SM == null || SM.ServiceId != EquipmentId)
            {
                return Json(new { result = false, message = "Combination not found." });
            }
            _Util.Facade.InventoryFacade.DeleteServiceMapById(Id);

            return Json(new { result = true, message = "Combination deleted successfully." });
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveServiceOptionsView(ServiceDetailInfoView Model)
        {
            if (Model.ServiceId == Guid.Empty)
            {
                return Json(new { result = false, message = "No service is selected." });
            }

            var temp = _Util.Facade.InventoryFacade.GetServiceDetailInfoViewByServiceId(Model.ServiceId);
            if (temp != null && temp.Id > 0)
            {
                if (temp.ShowCapacity != Model.ShowCapacity || temp.ShowFinish != Model.ShowFinish
                    || temp.ShowLocation != Model.ShowLocation || temp.ShowManufacturer != Model.ShowManufacturer || temp.ShowModel != Model.ShowModel
                    || temp.ShowType != Model.ShowType)
                {
                    _Util.Facade.InventoryFacade.UpdateServiceDetailInfoView(Model);

                    _Util.Facade.InventoryFacade.DeleteServiceMapByServiceId(Model.ServiceId);
                }
            }
            else
            {
                _Util.Facade.InventoryFacade.InsertServiceDetailInfoView(Model);
            }
            return Json(new { result = true, message = "Saved successfully." });
        }


        [Authorize]
        public ActionResult ShowCombinationList(Guid EquipmentId)
        {
            ViewBag.ServiceId = EquipmentId;

            ServiceDetailInfoView ServiceView = _Util.Facade.InventoryFacade.GetServiceDetailInfoViewByServiceId(EquipmentId);
            ViewBag.ServiceDetailInfoView = ServiceView;
            List<ServiceMap> Model = _Util.Facade.InventoryFacade.GetServiceModelListByServiceId(EquipmentId);
            return View(Model);
        }


        [Authorize]
        public ActionResult MatchCombinations(Guid EquipmentId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ServiceDetailInfoView ShowView = _Util.Facade.InventoryFacade.GetServiceDetailInfoViewByServiceId(EquipmentId);
            ViewBag.ShowView = ShowView;
            ServiceMap Model = new ServiceMap();
            Model.ServiceId = EquipmentId;
            if (ShowView.ShowCapacity)
            {
                List<SelectListItem> CapList = _Util.Facade.InventoryFacade.GetServiceInfoByServiceIdAndType(EquipmentId, "Capacity").Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Name.ToString(),
                        Value = x.ServiceInfoId.ToString()
                    }).ToList();
                CapList.Insert(0, new SelectListItem()
                {
                    Text = "Select Capacity",
                    Value = "00000000-0000-0000-0000-000000000000",
                    Selected = true
                });

                ViewBag.ShowCapacityList = CapList;

            }
            if (ShowView.ShowFinish)
            {
                List<SelectListItem> CapList = _Util.Facade.InventoryFacade.GetServiceInfoByServiceIdAndType(EquipmentId, "Finish").Select(x =>
                     new SelectListItem()
                     {
                         Text = x.Name.ToString(),
                         Value = x.ServiceInfoId.ToString()
                     }).ToList();
                CapList.Insert(0, new SelectListItem()
                {
                    Text = "Select Finish",
                    Value = "00000000-0000-0000-0000-000000000000",
                    Selected = true

                });

                ViewBag.ShowFinishList = CapList;
            }
            if (ShowView.ShowLocation)
            {
                List<SelectListItem> CapList = _Util.Facade.InventoryFacade.GetServiceInfoByServiceIdAndType(EquipmentId, "Location").Select(x =>
                     new SelectListItem()
                     {
                         Text = x.Name.ToString(),
                         Value = x.ServiceInfoId.ToString()
                     }).ToList();
                CapList.Insert(0, new SelectListItem()
                {
                    Text = "Select Location",
                    Value = "00000000-0000-0000-0000-000000000000",
                    Selected = true

                });

                ViewBag.ShowLocationList = CapList;
            }
            if (ShowView.ShowManufacturer)
            {
                List<SelectListItem> CapList = _Util.Facade.ManufacturerFacade.GetAllManufacturerByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Name.ToString(),
                        Value = x.ManufacturerId.ToString()
                    }).ToList();
                CapList.Insert(0, new SelectListItem()
                {
                    Text = "Select Manufacturer",
                    Value = "00000000-0000-0000-0000-000000000000",
                    Selected = true

                });

                ViewBag.ShowManufacturerList = CapList;
            }
            if (ShowView.ShowModel)
            {
                List<SelectListItem> CapList = _Util.Facade.InventoryFacade.GetServiceInfoByServiceIdAndType(EquipmentId, "Model").Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Name.ToString(),
                        Value = x.ServiceInfoId.ToString()
                    }).ToList();
                CapList.Insert(0, new SelectListItem()
                {
                    Text = "Select Model",
                    Value = "00000000-0000-0000-0000-000000000000",
                    Selected = true

                });

                ViewBag.ShowModelList = CapList;
            }
            if (ShowView.ShowType)
            {
                List<SelectListItem> CapList = _Util.Facade.InventoryFacade.GetServiceInfoByServiceIdAndType(EquipmentId, "Type").Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Name.ToString(),
                        Value = x.ServiceInfoId.ToString()
                    }).ToList();
                CapList.Insert(0, new SelectListItem()
                {
                    Text = "Select Type",
                    Value = "00000000-0000-0000-0000-000000000000",
                    Selected = true

                });

                ViewBag.ShowTypeList = CapList;
            }

            return View(Model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddCombination(ServiceMap Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ServiceDetailInfoView ShowView = _Util.Facade.InventoryFacade.GetServiceDetailInfoViewByServiceId(Model.ServiceId);
            #region Validations
            if (ShowView == null || (ShowView.ShowManufacturer == false && ShowView.ShowCapacity == false
                && ShowView.ShowFinish == false && ShowView.ShowLocation == false && ShowView.ShowModel == false && ShowView.ShowType == false))
            {
                return Json(new { result = false, message = "Please select options to view first." });
            }

            if (ShowView.ShowCapacity && Model.CapacityId == Guid.Empty)
            {
                return Json(new { result = false, message = "Capacity Required." });
            }
            else if (ShowView.ShowFinish && Model.FinishId == Guid.Empty)
            {
                return Json(new { result = false, message = "Finish required." });
            }
            else if (ShowView.ShowLocation && Model.LocationId == Guid.Empty)
            {
                return Json(new { result = false, message = "Location Required." });
            }
            else if (ShowView.ShowManufacturer && Model.ManufacturerId == Guid.Empty)
            {
                return Json(new { result = false, message = "Manufacturer Required." });
            }
            else if (ShowView.ShowModel && Model.ModelId == Guid.Empty)
            {
                return Json(new { result = false, message = "Model required." });
            }
            else if (ShowView.ShowType && Model.TypeId == Guid.Empty)
            {
                return Json(new { result = false, message = "Type required." });
            }
            #endregion

            Model.CompanyId = CurrentUser.CompanyId.Value;

            ServiceMap TempMap = _Util.Facade.InventoryFacade.GetServiceMapByServiceMap(Model);
            if (TempMap != null)
            {
                return Json(new { result = false, message = "Combination already exists." });
            }
            _Util.Facade.InventoryFacade.InsertServiceMap(Model);
            return Json(new { result = true, message = "Saved successfully." });
        }

        [Authorize]
        public ActionResult EquipmentFilesPartial(Guid EquipmentId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<EquipmentFile> EquipmentFiles = _Util.Facade.EquipmentFileFacade.GetEquipmentFilesByEquipmentId(EquipmentId);

            return View("_EquipmentFilesPartial", EquipmentFiles);
        }
        [Authorize]
        public ActionResult EquipmentAssign(int selectedTransferId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var Equipment = _Util.Facade.EquipmentFacade.GetEquipmentById(selectedTransferId);
            var EquipmentDetails = _Util.Facade.InventoryFacade.GetCustomInventoryWarehouseEquipmentByEquipmentIdAndCompanyId(Equipment.EquipmentId, CurrentUser.CompanyId.Value).FirstOrDefault();
            var TechnicianList = new List<Employee>();
            var TransferLocations = _Util.Facade.EmployeeFacade.GetTransferLocations(CurrentUser.CompanyId.Value);

            var defaultOption = new SelectListItem
            {
                Text = "Please select",
                Value = "-1",
                Selected = true
            };

            var transferLocationItems = TransferLocations.Select(location => new SelectListItem
            {
                Text = location.UserName,
                Value = location.UserId.ToString(),

            }).ToList();

            if (CurrentUser.UserTags.ToLower().IndexOf("technician") != 0)
            {
                TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid())
                    .OrderBy(x => x.FirstName + " " + x.LastName).ToList();

                var technicianItems = TechnicianList.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString()
                }).ToList();

                ViewBag.TechnicianList = new List<SelectListItem> { defaultOption }
                    .Concat(transferLocationItems)
                    .Concat(technicianItems)
                    .ToList();
            }
            else
            {
                TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, CurrentUser.UserId)
                    .OrderBy(x => x.FirstName + " " + x.LastName).ToList();

                var technicianItems = TechnicianList.Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                    Selected = true
                }).ToList();

                ViewBag.TechnicianList = new List<SelectListItem> { defaultOption }
                    .Concat(transferLocationItems)
                    .Concat(technicianItems)
                    .ToList();
            }

            return View("_EquipmentAssign", EquipmentDetails);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EquipmentAssignSave(InventoryTech inventory)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var result = true;
            TechTransferRequest model = new TechTransferRequest();

            model.Items = new List<AssignedInventoryTechReceived>();
            model.CreatedBy = new Guid();
            //model.CreatedDate = DateTime.Now.ClientToUTCTime();
            model.Items.Add(new AssignedInventoryTechReceived() { EquipmentId = inventory.EquipmentId, TechnicianId = new Guid("22222222-2222-2222-2222-222222222222"), ReceivedBy = inventory.TechnicianId, Quantity = inventory.Quantity, CreatedBy = CurrentUser.UserId, CreatedDate = DateTime.Now.ClientToUTCTime(), ReqSrc = "[WHTT-Approve]" });
            model.CreatedBy = CurrentUser.UserId;
            result = _Util.Facade.InventoryFacade.InsertTechTransfer(model, CurrentUser.UserId) > 0;

            //new EquipmentTransfer().TransferWHToTech(inventory.TechnicianId, inventory.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, "[Inv-WH-Trf]",inventory.Quantity, inventory.PurchaseOrderId);

            return Json(result);
        }
        [Authorize]
        public ActionResult ServiceOptionList(Guid EquipmentId)
        {
            if (EquipmentId == Guid.Empty)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
            ViewBag.ServiceId = EquipmentId.ToString();

            ServiceDetailInfoView Infoview = _Util.Facade.InventoryFacade.GetServiceDetailInfoViewByServiceId(EquipmentId);
            ViewBag.ServiceDetailInfoView = Infoview;

            List<ServiceDetailInfo> Model = _Util.Facade.InventoryFacade.GetAllServiceDetailinfoByServiceId(EquipmentId);
            return View(Model);
        }
        [Authorize]
        public ActionResult AddServiceOption(Guid EquipmentId)
        {
            ServiceDetailInfoView ServiceOptionsView = _Util.Facade.InventoryFacade.GetServiceDetailInfoViewByServiceId(EquipmentId);
            List<SelectListItem> ServiceOtionList = new List<SelectListItem>();

            ServiceOtionList.Add(new SelectListItem
            {
                Text = "Select One",
                Value = "-1"
            });
            if (ServiceOptionsView != null)
            {
                if (ServiceOptionsView.ShowLocation)
                {
                    ServiceOtionList.Add(new SelectListItem
                    {
                        Text = "Location",
                        Value = "Location"
                    });

                }
                if (ServiceOptionsView.ShowType)
                {
                    ServiceOtionList.Add(new SelectListItem
                    {
                        Text = "Type",
                        Value = "Type"
                    });
                }
                if (ServiceOptionsView.ShowModel)
                {
                    ServiceOtionList.Add(new SelectListItem
                    {
                        Text = "Model",
                        Value = "Model"
                    });
                }
                if (ServiceOptionsView.ShowFinish)
                {
                    ServiceOtionList.Add(new SelectListItem
                    {
                        Text = "Finish",
                        Value = "Finish"
                    });
                }
                if (ServiceOptionsView.ShowCapacity)
                {
                    ServiceOtionList.Add(new SelectListItem
                    {
                        Text = "Capacity",
                        Value = "Capacity"
                    });
                }


            }
            ServiceOtionList = ServiceOtionList.OrderBy(x => x.Text != "Select One").ThenBy(x => x.Text).ToList();
            ViewBag.ServiceOptionList = ServiceOtionList;

            //ViewBag.ServiceOptionList = _Util.Facade.LookupFacade.GetLookupByKey("ServiceOption").Select(x =>
            //          new SelectListItem()
            //          {
            //              Text = x.DisplayText.ToString(),
            //              Value = x.DataValue.ToString()
            //          }).ToList();
            ViewBag.ServiceInfoId = EquipmentId;
            return View();
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveServiceOption(ServiceDetailInfo serviceOption)
        {
            var result = false;
            var message = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            try
            {
                List<ServiceDetailInfo> serviceInfo = _Util.Facade.InventoryFacade.GetAllServiceDetailinfoByServiceId(serviceOption.ServiceInfoId);
                foreach (var item in serviceInfo)
                {
                    if (item.Name == serviceOption.Name)
                    {
                        message = "Name already exist.";
                        result = false;
                        return Json(new { result = result, message = message });
                    }
                }
                serviceOption.ServiceInfoId = Guid.NewGuid();

                _Util.Facade.InventoryFacade.InsertServiceDetailInfo(serviceOption);
                message = "Service option saved successfully.";
                result = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                message = "Service option not saved";
                result = true;
            }
            return Json(new { result = result, message = message });
        }

        #region Add Manual Inventory
        [Authorize]
        [HttpGet]
        public ActionResult AddManualInventory(Guid EquipmentId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            InventoryWarehouse Model = new InventoryWarehouse()
            {
                CompanyId = CurrentUser.CompanyId.Value,
                EquipmentId = EquipmentId,
                Type = "-1"

            };
            List<SelectListItem> InventoryTypList = new List<SelectListItem>();
            InventoryTypList.Add(new SelectListItem
            {
                Text = "Select One",
                Value = "-1"
            });
            InventoryTypList.Add(new SelectListItem
            {
                Text = LabelHelper.InventoryType.Add,
                Value = LabelHelper.InventoryType.Add,
            });
            InventoryTypList.Add(new SelectListItem
            {
                Text = LabelHelper.InventoryType.Release,
                Value = LabelHelper.InventoryType.Release,
            });
            ViewBag.InventoryType = InventoryTypList;

            return View(Model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddManualInventory(InventoryWarehouse Model)
        {
            Model.LocationId = new Guid("22222222-2222-2222-2222-222222222222");
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Model.CompanyId = CurrentUser.CompanyId.Value;
            Model.PurchaseOrderId = "";
            Model.LastUpdatedBy = CurrentUser.UserId;
            Model.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            Model.Description = (Model.Type == LabelHelper.InventoryType.Add) ?
                LabelHelper.InventoryDescription.AddedToWarehouseManually : LabelHelper.InventoryDescription.RemovedFromWarehouseManually;

            _Util.Facade.InventoryFacade.InsertInventoryWareHouse(Model);
            return Json(new { result = true, message = "Inventory updated successfully." });

        }
        #endregion

        #region MassRestock
        [Authorize]
        public ActionResult AddMassRestock()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var TechnicianList = new List<Employee>();
            if (currentLoggedIn.UserTags.ToLower().IndexOf("technician") != 0)
            {
                TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid());
                ViewBag.TechnicianList = TechnicianList.Select(x =>
                                           new SelectListItem()
                                           {
                                               Text = x.FirstName + " " + x.LastName,
                                               Value = x.UserId.ToString(),
                                               Selected = true
                                           }).ToList();
            }
            else
            {
                TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Technicians, currentLoggedIn.UserId);
                ViewBag.TechnicianList = TechnicianList.Select(x =>
                                           new SelectListItem()
                                           {
                                               Text = x.FirstName + " " + x.LastName,
                                               Value = x.UserId.ToString(),
                                               Selected = true
                                           }).ToList();
            }
            return PartialView("_AddMassRestock", TechnicianList);
        }
        [Authorize]
        public ActionResult AddMassRestockPartial(MassRestockFilter massFilter)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            massFilter.CompanyId = currentLoggedIn.CompanyId.Value;
            //List<MassRestock> model = new List<MassRestock>();
            MassRestockModel model = new MassRestockModel();
            model = _Util.Facade.EquipmentFacade.GetEquipmentListByCompanyIdTechnicianId(massFilter);
            ViewBag.TechnicianId = massFilter.TechnicianId;
            ViewBag.Id = massFilter.Id;
            //ViewBag.order = massFilter.Order;
            var TechnicianList = new List<Employee>();
            if (currentLoggedIn.UserTags.ToLower().IndexOf("technician") != 0)
            {
                TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid());
                ViewBag.TechnicianList = TechnicianList.Select(x =>
                                           new SelectListItem()
                                           {
                                               Text = x.FirstName + " " + x.LastName,
                                               Value = x.UserId.ToString(),
                                               Selected = true
                                           }).ToList();
            }
            else
            {
                TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Technicians, currentLoggedIn.UserId);
                ViewBag.TechnicianList = TechnicianList.Select(x =>
                                           new SelectListItem()
                                           {
                                               Text = x.FirstName + " " + x.LastName,
                                               Value = x.UserId.ToString(),
                                               Selected = true
                                           }).ToList();
            }
            if (TechnicianList.Count > 0)
            {
                var ItemToRemove = TechnicianList.Where(m => m.UserId == massFilter.TechnicianId).FirstOrDefault();
                TechnicianList.Remove(ItemToRemove);
            }
            ViewBag.CloneTechnicianList = TechnicianList.Select(x =>
                                            new SelectListItem()
                                            {
                                                Text = x.FirstName + " " + x.LastName,
                                                Value = x.UserId.ToString()
                                            }).ToList();
            return PartialView("_AddMassRestockPartial", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveMassRestock(List<MassRestock> MassRestockList, Guid TechnicianId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var result = false;
            bool massrestockexists = true;
            int correctmassrestoc = 0;
            List<PurchaseOrderDetail> purchaseOrderDetail = new List<PurchaseOrderDetail>();
            if (MassRestockList != null)
            {
                foreach (var massrestock in MassRestockList)
                {
                    if (massrestock.New > massrestock.Quantity)
                    {
                        var EquipmentDetails = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(massrestock.EquipmentId);
                        var DOQuantity = massrestock.New - massrestock.Quantity;
                        massrestock.New = massrestock.Quantity;
                        purchaseOrderDetail.Add(new PurchaseOrderDetail()
                        {
                            EquipmentId = massrestock.EquipmentId,
                            EquipName = EquipmentDetails.Name,
                            EquipDetail = EquipmentDetails.Description,
                            BundleId = 0,
                            Quantity = DOQuantity,
                            UnitPrice = EquipmentDetails.Retail,
                            TotalPrice = DOQuantity * EquipmentDetails.Retail,
                            CreatedDate = DateTime.Now,
                            CreatedBy = CurrentUser.UserId
                        });
                    }
                    if (massrestock.New > 0)
                    {
                        correctmassrestoc += 1;
                        //InventoryWarehouse invWare = new InventoryWarehouse()
                        //{
                        //    CompanyId = CurrentUser.CompanyId.Value,
                        //    EquipmentId = massrestock.EquipmentId,
                        //    Type = LabelHelper.InventoryType.Release,
                        //    Quantity = massrestock.New,
                        //    LastUpdatedBy = CurrentUser.UserId,
                        //    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        //    Description = "Send to technician from warehouse by massrestock"
                        //};
                        //result = _Util.Facade.InventoryFacade.InsertInventoryWareHouse(invWare) > 0;

                        //InventoryTech invTech = new InventoryTech()
                        //{
                        //    CompanyId = CurrentUser.CompanyId.Value,
                        //    EquipmentId = massrestock.EquipmentId,
                        //    Type = LabelHelper.InventoryType.Add,
                        //    Quantity = massrestock.New,
                        //    TechnicianId = massrestock.TechnicianId,
                        //    LastUpdatedBy = CurrentUser.UserId,
                        //    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        //    Description = "Receive by technician from warehouse by massrestock"
                        //};
                        //result = _Util.Facade.InventoryFacade.InsertInventoryTech(invTech) > 0;

                        TechTransferRequest model = new TechTransferRequest();
                        model.Items = new List<AssignedInventoryTechReceived>();
                        model.Items.Add(new AssignedInventoryTechReceived() { EquipmentId = massrestock.EquipmentId, TechnicianId = new Guid("22222222-2222-2222-2222-222222222222"), ReceivedBy = massrestock.TechnicianId, Quantity = massrestock.New, CreatedBy = CurrentUser.UserId, CreatedDate = DateTime.Now.UTCCurrentTime(), ReqSrc = "[MSRSTK-WHTT]" });
                        model.CreatedBy = CurrentUser.UserId;
                        result = _Util.Facade.InventoryFacade.InsertTechTransfer(model, CurrentUser.UserId) > 0;

                        //new EquipmentTransfer().TransferWHToTech(massrestock.TechnicianId, massrestock.EquipmentId, CurrentUser.CompanyId.Value,
                        //    CurrentUser.UserId, "[MSRSTK-WHTT]", massrestock.New);
                        //result = true;
                    }
                }
                if (correctmassrestoc == 0)
                {
                    massrestockexists = false;
                }

                #region Demand Order create
                var BranchIdInner = 0;
                var BranchInfo = _Util.Facade.EmployeeFacade.GetUserBranchByCompanyIdAndUserId(CurrentUser.CompanyId.Value, CurrentUser.UserId);
                if (BranchInfo != null)
                {
                    BranchIdInner = BranchInfo.BranchId;
                }
                var PurchaseOrderTech = new PurchaseOrderTech()
                {
                    IsBulkPO = true,
                    CompanyId = CurrentUser.CompanyId.Value,
                    Status = LabelHelper.PurchaseOrderStatus.Created,
                    TechnicianId = TechnicianId,
                    CreatedByUid = CurrentUser.UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedByUid = CurrentUser.UserId,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                };
                PurchaseOrderTech.Id = _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderTech(PurchaseOrderTech);
                PurchaseOrderTech.DemandOrderId = PurchaseOrderTech.Id.GenerateDONoTech();
                _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderTech(PurchaseOrderTech);
                var PurchaseOrderBranch = new PurchaseOrderBranch()
                {
                    IsBulkPO = true,
                    CompanyId = CurrentUser.CompanyId.Value,
                    Status = LabelHelper.PurchaseOrderStatus.Created,
                    IsReceived = false,
                    BranchId = BranchIdInner,
                    CreatedByUid = CurrentUser.UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedByUid = CurrentUser.UserId,
                    CreatedDate = DateTime.Now.UTCCurrentTime()
                };
                PurchaseOrderBranch.Id = _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderBranch(PurchaseOrderBranch);
                PurchaseOrderBranch.DemandOrderId = PurchaseOrderBranch.Id.GenerateDONoBranch();
                PurchaseOrderBranch.TechDemandOrderId = PurchaseOrderTech.DemandOrderId;
                _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderBranch(PurchaseOrderBranch);

                _Util.Facade.PurchaseOrderFacade.DeleteAllPurchaseOrderDetailByPurchaseOrderId(PurchaseOrderTech.DemandOrderId);
                foreach (var item in purchaseOrderDetail)
                {
                    item.BulkStatus = false;
                    item.PurchaseOrderId = PurchaseOrderTech.DemandOrderId;
                    _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderDetail(item);
                }
                _Util.Facade.PurchaseOrderFacade.DeleteAllPurchaseOrderDetailByPurchaseOrderId(PurchaseOrderBranch.DemandOrderId);
                foreach (var item in purchaseOrderDetail)
                {
                    item.BulkStatus = false;
                    item.PurchaseOrderId = PurchaseOrderBranch.DemandOrderId;
                    _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderDetail(item);
                }
                #endregion
            }
            else
            {
                return Json(new { result = false, message = "Insufficient equipment" });
            }

            if (massrestockexists)
            {
                return Json(new { result = true, message = "Transfer request added successfully" });
            }
            else
            {
                return Json(new { result = false, message = "Insufficient quantity or equipment" });
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult CheckMassRestock(List<MassRestock> MassRestockList, Guid TechnicianId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<PurchaseOrderDetail> purchaseOrderDetail = new List<PurchaseOrderDetail>();
            List<string> MassRestockName = new List<string>();

            if (MassRestockList != null)
            {
                foreach (var massrestock in MassRestockList)
                {
                    var EquipmentDetails = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(massrestock.EquipmentId);

                    long result = _Util.Facade.PurchaseOrderFacade.CheckMassRestockOrderDetail(massrestock.EquipmentId, TechnicianId);
                    if (result > 0)
                    {
                        MassRestockName.Add(EquipmentDetails.Name);
                    }
                }
            }

            if (MassRestockName.Count > 0)
            {
                return Json(new { result = true, MassRestockName = string.Join(",", MassRestockName) });
            }
            else
            {
                return Json(new { result = false });
            }

        }


        [Authorize]
        [HttpPost]
        public ActionResult SaveMassRestockClone(List<MassRestock> MassRestockList, List<Guid> TechnicianIdList)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var result = false;
            if (MassRestockList != null && TechnicianIdList != null)
            {
                foreach (var TechnicianId in TechnicianIdList)
                {
                    List<PurchaseOrderDetail> purchaseOrderDetail = new List<PurchaseOrderDetail>();

                    foreach (var massrestock in MassRestockList)
                    {
                        if (massrestock.New > massrestock.Quantity)
                        {
                            var EquipmentDetails = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(massrestock.EquipmentId);
                            var DOQuantity = massrestock.New - massrestock.Quantity;
                            massrestock.New = massrestock.Quantity;
                            purchaseOrderDetail.Add(new PurchaseOrderDetail()
                            {
                                EquipmentId = massrestock.EquipmentId,
                                EquipName = EquipmentDetails.Name,
                                EquipDetail = EquipmentDetails.Description,
                                BundleId = 0,
                                Quantity = DOQuantity,
                                UnitPrice = EquipmentDetails.Retail,
                                TotalPrice = DOQuantity * EquipmentDetails.Retail,
                                CreatedDate = DateTime.Now,
                                CreatedBy = CurrentUser.UserId
                            });
                        }
                        if (massrestock.New > 0)
                        {
                            //InventoryWarehouse invWare = new InventoryWarehouse()
                            //{
                            //    CompanyId = CurrentUser.CompanyId.Value,
                            //    EquipmentId = massrestock.EquipmentId,
                            //    Type = LabelHelper.InventoryType.Release,
                            //    Quantity = massrestock.New,
                            //    LastUpdatedBy = CurrentUser.UserId,
                            //    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            //    Description = "Send to technician from warehouse by massrestock"
                            //};
                            //result = _Util.Facade.InventoryFacade.InsertInventoryWareHouse(invWare) > 0;

                            //InventoryTech invTech = new InventoryTech()
                            //{
                            //    CompanyId = CurrentUser.CompanyId.Value,
                            //    EquipmentId = massrestock.EquipmentId,
                            //    Type = LabelHelper.InventoryType.Add,
                            //    Quantity = massrestock.New,
                            //    TechnicianId = TechnicianId,
                            //    LastUpdatedBy = CurrentUser.UserId,
                            //    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            //    Description = "Receive by technician from warehouse by massrestock"
                            //};
                            //result = _Util.Facade.InventoryFacade.InsertInventoryTech(invTech) > 0;

                            new EquipmentTransfer().TransferWHToTech(massrestock.TechnicianId, massrestock.EquipmentId, CurrentUser.CompanyId.Value,
                            CurrentUser.UserId, new Guid("22222222-2222-2222-2222-222222222222"), "[MSRSTK-WHTTC]", massrestock.New);
                            result = true;
                        }
                    }
                    #region Demand Order create
                    var BranchIdInner = 0;
                    var BranchInfo = _Util.Facade.EmployeeFacade.GetUserBranchByCompanyIdAndUserId(CurrentUser.CompanyId.Value, CurrentUser.UserId);
                    if (BranchInfo != null)
                    {
                        BranchIdInner = BranchInfo.BranchId;
                    }
                    var PurchaseOrderTech = new PurchaseOrderTech()
                    {
                        IsBulkPO = true,
                        CompanyId = CurrentUser.CompanyId.Value,
                        Status = LabelHelper.PurchaseOrderStatus.Created,
                        TechnicianId = TechnicianId,
                        CreatedByUid = CurrentUser.UserId,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedByUid = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                    };
                    PurchaseOrderTech.Id = _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderTech(PurchaseOrderTech);
                    PurchaseOrderTech.DemandOrderId = PurchaseOrderTech.Id.GenerateDONoTech();
                    _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderTech(PurchaseOrderTech);
                    var PurchaseOrderBranch = new PurchaseOrderBranch()
                    {
                        IsBulkPO = true,
                        CompanyId = CurrentUser.CompanyId.Value,
                        Status = LabelHelper.PurchaseOrderStatus.Created,
                        IsReceived = false,
                        BranchId = BranchIdInner,
                        CreatedByUid = CurrentUser.UserId,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedByUid = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime()
                    };
                    PurchaseOrderBranch.Id = _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderBranch(PurchaseOrderBranch);
                    PurchaseOrderBranch.DemandOrderId = PurchaseOrderBranch.Id.GenerateDONoBranch();
                    PurchaseOrderBranch.TechDemandOrderId = PurchaseOrderTech.DemandOrderId;
                    _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderBranch(PurchaseOrderBranch);

                    _Util.Facade.PurchaseOrderFacade.DeleteAllPurchaseOrderDetailByPurchaseOrderId(PurchaseOrderTech.DemandOrderId);
                    foreach (var item in purchaseOrderDetail)
                    {
                        item.BulkStatus = false;
                        item.PurchaseOrderId = PurchaseOrderTech.DemandOrderId;
                        _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderDetail(item);
                    }
                    _Util.Facade.PurchaseOrderFacade.DeleteAllPurchaseOrderDetailByPurchaseOrderId(PurchaseOrderBranch.DemandOrderId);
                    foreach (var item in purchaseOrderDetail)
                    {
                        item.BulkStatus = false;
                        item.PurchaseOrderId = PurchaseOrderBranch.DemandOrderId;
                        _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderDetail(item);
                    }
                    #endregion
                }
            }
            else
            {
                return Json(new { result = false });
            }
            return Json(new { result = true });
        }
        #endregion

        #region MassPO
        [Authorize]
        public ActionResult AddMassPO(string order)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<MassPO> massPoList = new List<MassPO>();
            massPoList = _Util.Facade.EquipmentFacade.GetMassPOEquipmentListByCompanyId(CurrentUser.CompanyId.Value);
            ViewBag.order = order;
            return PartialView("_AddMassPO", massPoList);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SaveMassPO(List<MassPO> MassPOList)
        {
            if (MassPOList == null)
            {
                return Json(new { result = false });
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Company Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            Hashtable datatemplate = new Hashtable();
            datatemplate.Add("ComapnyName", Company.CompanyName);
            datatemplate.Add("Address", Company.Address);
            datatemplate.Add("Street", Company.Street);
            datatemplate.Add("City", Company.City);
            datatemplate.Add("State", Company.State);
            datatemplate.Add("Zip", Company.ZipCode);
            datatemplate.Add("CompanyPhone", Company.Phone);
            datatemplate.Add("EmailAddress", Company.EmailAdress);
            datatemplate.Add("WebAddress", Company.Website);
            string CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
            CompanyInfo = HS.Web.UI.Helper.LabelHelper.ParserHelper(CompanyInfo, datatemplate);
            string poPreText = "PO";
            GlobalSetting _poPretxt = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PrefixForPurchaseOrderId");
            if (_poPretxt != null && !string.IsNullOrWhiteSpace(_poPretxt.Value) && _poPretxt.Value != "")
            {
                poPreText = _poPretxt.Value;
            }
            var POList = MassPOList.GroupBy(x => new { x.SupplierId })
                     .Select(grp => grp.ToList())
                     .ToList();
            foreach (var POMain in POList)
            {
                bool PurchaseOrderCreate = false;
                var PurchaseOrderId = "";
                foreach (var PO in POMain)
                {
                    var EquipmentVendorDetails = _Util.Facade.EquipmentFacade.GetEquipmentVendorById(PO.EquipmentId, PO.SupplierId);
                    if (EquipmentVendorDetails != null)
                    {
                        if (!PurchaseOrderCreate)
                        {
                            var PurchaseOrderWarehouse = new PurchaseOrderWarehouse()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                POFor = new Guid("22222222-2222-2222-2222-222222222222"),
                                Status = LabelHelper.PurchaseOrderStatus.Created,
                                IsReceived = false,
                                CreatedByUid = CurrentUser.UserId,
                                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                LastUpdatedByUid = CurrentUser.UserId,
                                Amount = 0.0,
                                Balance = 0.0,
                                BalanceDue = 0.0,
                                BillingAddress = "",
                                TotalAmount = 0.0,
                                Tax = 0.0,
                                Deposit = 0.0,
                                ShippingCost = 0.0,
                                ShippingAddress = CompanyInfo,
                                TaxType = "",
                                Message = "",
                                ShippingVia = "",
                                Description = "",
                                TrackingNo = "",
                                RecieveByUid = new Guid(),
                                ShippingDate = DateTime.Now.AddDays(1),
                                OrderDate = DateTime.Now.UTCCurrentTime(),
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                SuplierId = PO.SupplierId,
                                IsBulkPO = true,
                                BranchDemandOrderId = PO.DemandOrderId
                            };
                            PurchaseOrderWarehouse.Id = _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderWarehouse(PurchaseOrderWarehouse);
                            PurchaseOrderId = PurchaseOrderWarehouse.PurchaseOrderId = PurchaseOrderWarehouse.Id.GeneratePONo(poPreText);
                            PurchaseOrderCreate = _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderWarehouse(PurchaseOrderWarehouse);

                            var BranchPurchaseOrderDetails = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderBranchByPOId(PO.DemandOrderId);
                            BranchPurchaseOrderDetails.Status = LabelHelper.DemandOrderStatus.POCreated;
                            _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderBranch(BranchPurchaseOrderDetails);
                        }

                        var BranchPODetails = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailByPurchaseOrderId(PO.DemandOrderId, PO.EquipmentId);
                        BranchPODetails.BulkStatus = true;
                        _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderDetail(BranchPODetails);

                        var EquipmentDetails = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(PO.EquipmentId);
                        var PurchaseOrderDeails = new PurchaseOrderDetail()
                        {
                            PurchaseOrderId = PurchaseOrderId,
                            EquipmentId = PO.EquipmentId,
                            EquipName = EquipmentDetails.Name,
                            EquipDetail = EquipmentDetails.Description,
                            BundleId = 0,
                            Quantity = PO.Quantity,
                            UnitPrice = EquipmentVendorDetails.Cost,
                            TotalPrice = PO.Quantity * EquipmentVendorDetails.Cost,
                            CreatedDate = DateTime.Now,
                            CreatedBy = CurrentUser.UserId
                        };
                        _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderDetail(PurchaseOrderDeails);
                    }
                }
            }
            return Json(new { result = true });
        }
        #endregion

        #region TechReorderPoint
        [Authorize]
        public ActionResult AddTechReorderPoint()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var TechnicianList = new List<Employee>();
            if (currentLoggedIn.UserTags.ToLower().IndexOf("technician") != 0)
            {
                TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid());
                ViewBag.TechnicianList = TechnicianList.Select(x =>
                                           new SelectListItem()
                                           {
                                               Text = x.FirstName + " " + x.LastName,
                                               Value = x.UserId.ToString(),
                                               Selected = true
                                           }).ToList();
            }
            else
            {
                TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Technicians, currentLoggedIn.UserId);
                ViewBag.TechnicianList = TechnicianList.Select(x =>
                                           new SelectListItem()
                                           {
                                               Text = x.FirstName + " " + x.LastName,
                                               Value = x.UserId.ToString(),
                                               Selected = true
                                           }).ToList();
            }
            return PartialView("_AddTechReorderPoint", TechnicianList);
        }
        [Authorize]
        public ActionResult AddTechReorderPointPartial(MassRestockFilter massFilter)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            massFilter.CompanyId = currentLoggedIn.CompanyId.Value;
            List<MassRestock> model = new List<MassRestock>();
            model = _Util.Facade.EquipmentFacade.GetEquipmentListByCompanyIdTechnicianIdReorderPoint(massFilter);
            ViewBag.TechnicianId = massFilter.TechnicianId;
            ViewBag.Id = massFilter.Id;
            var TechnicianList = new List<Employee>();
            if (currentLoggedIn.UserTags.ToLower().IndexOf("technician") != 0)
            {
                TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid());
                ViewBag.TechnicianList = TechnicianList.Select(x =>
                                           new SelectListItem()
                                           {
                                               Text = x.FirstName + " " + x.LastName,
                                               Value = x.UserId.ToString(),
                                               Selected = true
                                           }).ToList();
            }
            else
            {
                TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Technicians, currentLoggedIn.UserId);
                ViewBag.TechnicianList = TechnicianList.Select(x =>
                                           new SelectListItem()
                                           {
                                               Text = x.FirstName + " " + x.LastName,
                                               Value = x.UserId.ToString(),
                                               Selected = true
                                           }).ToList();
            }
            if (TechnicianList.Count > 0)
            {
                var ItemToRemove = TechnicianList.Where(m => m.UserId == massFilter.TechnicianId).FirstOrDefault();
                TechnicianList.Remove(ItemToRemove);
            }
            ViewBag.CloneTechnicianList = TechnicianList.Select(x =>
                                            new SelectListItem()
                                            {
                                                Text = x.FirstName + " " + x.LastName,
                                                Value = x.UserId.ToString()
                                            }).ToList();
            return PartialView("_AddTechReorderPointPartial", model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult SaveTechReorderPoint(List<MassRestock> MassRestockList, Guid TechnicianId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var result = false;
            if (MassRestockList != null && TechnicianId != null)
            {
                _Util.Facade.InventoryFacade.DeleteEquipmentTechnicianReorderPoint(TechnicianId);
                foreach (var massrestock in MassRestockList)
                {
                    EquipmentTechnicianReorderPoint eqpReorderPoint = new EquipmentTechnicianReorderPoint();
                    eqpReorderPoint.CompanyId = CurrentUser.CompanyId.Value;
                    eqpReorderPoint.EquipmentId = massrestock.EquipmentId;
                    eqpReorderPoint.TechnicianId = massrestock.TechnicianId;
                    eqpReorderPoint.ReorderPoint = massrestock.ReorderPoint;
                    result = _Util.Facade.InventoryFacade.InsertEquipmentTechnicianReorderPoint(eqpReorderPoint);
                }
            }
            return Json(new { result = result });
        }
        public ActionResult SaveTechReorderPointClone(List<MassRestock> MassRestockList, List<Guid> TechnicianIdList)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var result = false;
            if (MassRestockList != null && TechnicianIdList != null)
            {
                foreach (var TechnicianId in TechnicianIdList)
                {
                    _Util.Facade.InventoryFacade.DeleteEquipmentTechnicianReorderPoint(TechnicianId);
                    foreach (var massrestock in MassRestockList)
                    {
                        EquipmentTechnicianReorderPoint eqpReorderPoint = new EquipmentTechnicianReorderPoint();
                        eqpReorderPoint.CompanyId = CurrentUser.CompanyId.Value;
                        eqpReorderPoint.EquipmentId = massrestock.EquipmentId;
                        eqpReorderPoint.TechnicianId = TechnicianId;
                        eqpReorderPoint.ReorderPoint = massrestock.ReorderPoint;
                        result = _Util.Facade.InventoryFacade.InsertEquipmentTechnicianReorderPoint(eqpReorderPoint);
                    }
                }
            }
            return Json(new { result = result });
        }
        #endregion

        #region TechBadInventory
        [Authorize]
        public ActionResult TechBadInventoryPartial(string tab)
        {
            ViewBag.tab = tab;
            return PartialView();
        }

        [Authorize]
        public ActionResult TechBadInventoryListPartial(BadInventoryFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            #region Inits
            if (filters == null)
            {
                filters = new BadInventoryFilter();
            }

            if (filters.PageNo == 0)
            {
                filters.PageNo = 1;
            }
            filters.CompanyId = CurrentUser.CompanyId.Value;
            filters.PageSize = Convert.ToInt32(_Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("BadInventoryPageLimit", CurrentUser.CompanyId.Value).Value);
            BIListModel Model = new BIListModel();
            #endregion
            if (CurrentUser.UserRole.ToLower().IndexOf("technician") != 0 && CurrentUser.UserRole.ToLower().IndexOf("installation") != 0)
            {
                Model = _Util.Facade.PurchaseOrderFacade.GetBadInventoryListByFilters(filters, new Guid());
            }
            else
            {
                Model = _Util.Facade.PurchaseOrderFacade.GetBadInventoryListByFilters(filters, CurrentUser.UserId);
            }

            ViewBag.PageNumber = filters.PageNo;
            ViewBag.OutOfNumber = 0;

            if (Model.EquipmentReturnList.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCount;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);
            ViewBag.order = filters.order;
            ViewBag.tab = filters.tab;
            return View(Model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult TechAddBadInventory(int? Id)
        {
            var Currentuser = (CustomPrincipal)(User);
            List<SelectListItem> EquipmentList = new List<SelectListItem>();
            EquipmentList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            ViewBag.EquipmentList = EquipmentList;
            List<SelectListItem> CustomerList = new List<SelectListItem>();
            CustomerList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            ViewBag.CustomerList = CustomerList;
            var TechnicianList = new List<Employee>();
            if (Currentuser.UserTags.ToLower().IndexOf("technician") != 0)
            {
                TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(Currentuser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid());
                ViewBag.TechnicianList = TechnicianList.Select(x =>
                                           new SelectListItem()
                                           {
                                               Text = x.FirstName + " " + x.LastName,
                                               Value = x.UserId.ToString(),
                                               Selected = true
                                           }).ToList();
            }
            else
            {
                TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(Currentuser.CompanyId.Value, LabelHelper.UserTags.Technicians, Currentuser.UserId);
                ViewBag.TechnicianList = TechnicianList.Select(x =>
                                           new SelectListItem()
                                           {
                                               Text = x.FirstName + " " + x.LastName,
                                               Value = x.UserId.ToString(),
                                               Selected = true
                                           }).ToList();
            }

            var equipmentReturn = new EquipmentReturn();
            if (Id > 0)
            {
                equipmentReturn = _Util.Facade.PurchaseOrderFacade.GetEquipmentReturnById(Id.Value);
                var equiments = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(equipmentReturn.EquipmentId, Currentuser.CompanyId.Value);
                var equipClass = _Util.Facade.EquipmentFacade.GetAllEquipmentClassByCompanyId(Currentuser.CompanyId.Value);
                var manufacaturer = _Util.Facade.ManufacturerFacade.GetById(equiments.ManufacturerId);
                if (manufacaturer != null)
                {
                    EquipmentList.Add(new SelectListItem()
                    {
                        Text = manufacaturer.Name.ToString() + "(" + equiments.Name.ToString() + ")",
                        Value = equiments.EquipmentId.ToString()
                    });
                }
                else
                {
                    manufacaturer = new Manufacturer();
                    EquipmentList.Add(new SelectListItem()
                    {
                        Text = "(" + equiments.Name.ToString() + ")",
                        Value = equiments.EquipmentId.ToString()
                    });
                }
                var Customers = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(equipmentReturn.CustomerId);
                CustomerList.Add(new SelectListItem()
                {
                    Text = Customers.FirstName + " " + Customers.LastName,
                    Value = Customers.CustomerId.ToString()
                });
            }
            ViewBag.EquipmentList = EquipmentList;
            List<SelectListItem> badstatus = new List<SelectListItem>();
            badstatus.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("BadInventoryStatus").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.badStatus = badstatus;
            return View(equipmentReturn);
        }

        [Authorize]
        [HttpPost]
        public ActionResult TechAddBadInventory(EquipmentReturn equipmentReturn)
        {
            var CurrentUser = (CustomPrincipal)(User);
            string message = "";
            bool result = false;

            if (equipmentReturn.Id > 0)
            {
                EquipmentReturn oldEquipmentReturn = _Util.Facade.PurchaseOrderFacade.GetEquipmentReturnById(equipmentReturn.Id);
                oldEquipmentReturn.InvoiceNo = equipmentReturn.InvoiceNo;
                oldEquipmentReturn.LastUpdatedBy = CurrentUser.UserId;
                oldEquipmentReturn.LastUpdatedDate = DateTime.Now;
                oldEquipmentReturn.EquipmentId = equipmentReturn.EquipmentId;
                oldEquipmentReturn.TechnicianId = equipmentReturn.TechnicianId;
                oldEquipmentReturn.CustomerId = equipmentReturn.CustomerId;
                oldEquipmentReturn.Quantity = equipmentReturn.Quantity;
                oldEquipmentReturn.PurchaseDate = equipmentReturn.PurchaseDate;
                oldEquipmentReturn.Description = equipmentReturn.Description;
                if ((DateTime.Now - Convert.ToDateTime(equipmentReturn.PurchaseDate)).TotalDays > 0)
                {
                    oldEquipmentReturn.WanrantyAvailable = true;
                }
                else
                {
                    oldEquipmentReturn.WanrantyAvailable = false;
                }
                result = _Util.Facade.PurchaseOrderFacade.UpdateEquipmentReturn(oldEquipmentReturn);
                if (result)
                {
                    // EquipmentReturnNote
                    EquipmentReturnNote equipmentReturnNote = _Util.Facade.PurchaseOrderFacade.GetByReturnId(oldEquipmentReturn.ReturnId);
                    equipmentReturnNote.ReturnId = oldEquipmentReturn.ReturnId;
                    equipmentReturnNote.Description = equipmentReturn.Description;
                    equipmentReturnNote.LastUpdatedBy = CurrentUser.UserId;
                    equipmentReturnNote.LastUpdatedDate = DateTime.Now;
                    _Util.Facade.PurchaseOrderFacade.UpdateEquipmentReturnNote(equipmentReturnNote);

                    // EquipmentReturnVendor
                    EquipmentReturnVendor equipmentReturnVendor = _Util.Facade.PurchaseOrderFacade.GetVendorByReturnId(oldEquipmentReturn.ReturnId);
                    equipmentReturnVendor.ReturnId = oldEquipmentReturn.ReturnId;
                    equipmentReturnVendor.Description = equipmentReturn.Description;
                    equipmentReturnVendor.Status = equipmentReturn.Status;
                    equipmentReturnNote.LastUpdatedBy = CurrentUser.UserId;
                    equipmentReturnNote.LastUpdatedDate = DateTime.Now;
                    _Util.Facade.PurchaseOrderFacade.UpdateEquipmentReturnVendor(equipmentReturnVendor);

                    message = "Updated Successfully";
                    return Json(new { result = result, message = message });
                }
                message = "Update Fail!!!";
                return Json(new { result = result, message = message });
            }
            equipmentReturn.Status = LabelHelper.BadInventoryStatus.Created;
            var TotalDays = (DateTime.Now - Convert.ToDateTime(equipmentReturn.PurchaseDate)).TotalDays;
            if (TotalDays >= 0 && TotalDays <= 365)
            {
                equipmentReturn.WanrantyAvailable = true;
            }
            else
            {
                equipmentReturn.WanrantyAvailable = false;
            }
            equipmentReturn.LastUpdatedBy = CurrentUser.UserId;
            equipmentReturn.LastUpdatedDate = DateTime.Now;
            equipmentReturn.ReturnId = Guid.NewGuid();
            equipmentReturn.CompanyId = CurrentUser.CompanyId.Value;
            result = _Util.Facade.PurchaseOrderFacade.InsertEquipmentReturn(equipmentReturn);
            if (result)
            {
                // EquipmentReturnNote
                EquipmentReturnNote equipmentReturnNote = new EquipmentReturnNote();
                equipmentReturnNote.ReturnId = equipmentReturn.ReturnId;
                equipmentReturnNote.Description = equipmentReturn.Description;
                equipmentReturnNote.LastUpdatedBy = CurrentUser.UserId;
                equipmentReturnNote.LastUpdatedDate = DateTime.Now;
                equipmentReturnNote.CompanyId = CurrentUser.CompanyId.Value;
                _Util.Facade.PurchaseOrderFacade.InsertEquipmentReturnNote(equipmentReturnNote);

                // EquipmentReturnVendor
                EquipmentReturnVendor equipmentReturnVendor = new EquipmentReturnVendor();
                equipmentReturnVendor.ReturnId = equipmentReturn.ReturnId;
                equipmentReturnVendor.Description = equipmentReturn.Description;
                equipmentReturnVendor.Status = equipmentReturn.Status;
                equipmentReturnVendor.LastUpdatedBy = CurrentUser.UserId;
                equipmentReturnVendor.LastUpdatedDate = DateTime.Now;
                equipmentReturnVendor.CompanyId = CurrentUser.CompanyId.Value;
                _Util.Facade.PurchaseOrderFacade.InsertEquipmentReturnVendor(equipmentReturnVendor);

                message = "Saved Successfully";
                return Json(new { result = result, message = message });
            }
            message = "Something Wrong";
            return Json(new { result = result, message = message });
        }

        [Authorize]
        [HttpPost]
        public JsonResult TechDeleteEquipmentReturn(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.PurchaseOrderFacade.DeleteEquipmentReturn(id.Value);
            }
            return Json(result);
        }
        [Authorize]
        [HttpPost]
        public JsonResult TechSendToBadInventory(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                var EquipmentReturnDetails = _Util.Facade.PurchaseOrderFacade.GetEquipmentReturnById(id.Value);
                if (EquipmentReturnDetails != null)
                {
                    EquipmentReturnDetails.Status = LabelHelper.BadInventoryStatus.Send;
                    result = _Util.Facade.PurchaseOrderFacade.UpdateEquipmentReturn(EquipmentReturnDetails);
                }
            }
            return Json(result);
        }
        #endregion

        [Authorize]
        public JsonResult GetVendorListByKeyAndEquipmentId(string key, Guid EquipmentId)
        {
            string result = "[]";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetVendorSearchMaxLoad(CurrentUser.CompanyId.Value);

            List<SupplierCustom> SupplierList = _Util.Facade.SupplierFacade.GetAllSupplierByCompanyIdAndSearchKeyAndEquipmentId(CurrentUser.CompanyId.Value, key, ItemsLoadCount, EquipmentId);
            if (SupplierList.Count > 0)
                result = JsonConvert.SerializeObject(SupplierList);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult TechAcceptBadInventory(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                var objbadinv = _Util.Facade.InventoryFacade.GetBadInventoryById(id.Value);
                if (objbadinv != null)
                {
                    objbadinv.Status = "Accepted";
                    _Util.Facade.EquipmentFacade.UpdateEquipmentReturn(objbadinv);
                }
            }
            return Json(true);
        }

        public ActionResult TransferFromTechnician(TransferFromTechnicianModel model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SelectListItem> TechnicianList = new List<SelectListItem>();
            TechnicianList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = new Guid().ToString()
            });
            //TechnicianList.Add(new SelectListItem()
            //{
            //    Text = "Warehouse",
            //    Value = "22222222-2222-2222-2222-222222222222"
            //});
            var TransferLocations = _Util.Facade.EmployeeFacade.GetTransferLocations(CurrentUser.CompanyId.Value);

            foreach (var location in TransferLocations)
            {
                TechnicianList.Add(new SelectListItem()
                {
                    Text = location.UserName,
                    Value = location.UserId.ToString(),
                    Selected = location.UserId.ToString() == "22222222-2222-2222-2222-222222222222",
                });
            }
            TechnicianList.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTagAndTechnician(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, model.Technicianid).OrderBy(x => x.FirstName).Select(x =>
            new SelectListItem()
            {
                Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                Value = x.UserId.ToString()
            }).ToList());
            ViewBag.TechnicianList = TechnicianList;

            return PartialView("_TransferFromTechnician", model);
        }

        public JsonResult SaveTransferFromTechnician(TransferFromTechnicianModel model)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<Guid> EquipmentidlistGuid = new List<Guid>();

            if (!string.IsNullOrEmpty(model.Equipmentidlist))
            {
                string[] splituser = model.Equipmentidlist.Split(',');
                if (splituser.Length > 0)
                {
                    foreach (var item in splituser)
                    {
                        EquipmentidlistGuid.Add(Guid.Parse(item));
                    }
                }
            }

            TechTransferRequest trf_request = new TechTransferRequest
            {
                CreatedDate = DateTime.Now,
                Items = new List<AssignedInventoryTechReceived>()
            };
            List<Guid> warehouseGuids = new List<Guid>
            {
                new Guid("22222222-2222-2222-2222-222222222222"),
                new Guid("22222222-2222-2222-2222-222222222223"),
                new Guid("22222222-2222-2222-2222-222222222224"),
                new Guid("22222222-2222-2222-2222-222222222225"),
                new Guid("22222222-2222-2222-2222-222222222226"),
                new Guid("22222222-2222-2222-2222-222222222231"),
                new Guid("22222222-2222-2222-2222-222222222232")
            };

            foreach (var item in EquipmentidlistGuid)
            {
                var EquipmetInventoryQuantity = _Util.Facade.InventoryFacade.InventoryTechAvailableCount(model.FromTechnicianid, item);
                AssignedInventoryTechReceived receivedModel = new AssignedInventoryTechReceived
                {
                    EquipmentId = item,
                    TechnicianId = model.FromTechnicianid,
                    Quantity = EquipmetInventoryQuantity,
                    ReceivedBy = model.Technicianid,
                    CreatedDate = DateTime.Now,
                    CreatedBy = CurrentUser.UserId,
                    ReqSrc = "[TI-AIT]"
                };



                trf_request.Items.Add(receivedModel);

                //if (warehouseGuids.Contains(model.Technicianid))
                //{
                //    new EquipmentTransfer().TransferTechToWH(model.FromTechnicianid, item, CurrentUser.CompanyId.Value, CurrentUser.UserId, model.Technicianid, "[TI-AIT]", EquipmetInventoryQuantity);
                //}
                //else
                //{
                //    new EquipmentTransfer().TransferTechToTech(model.FromTechnicianid, item, CurrentUser.CompanyId.Value, CurrentUser.UserId, model.Technicianid, "[TI-AIT]", EquipmetInventoryQuantity);
                //}

            }

            result = _Util.Facade.InventoryFacade.InsertTechTransfer(trf_request, CurrentUser.UserId) > 0;

            return Json(new { result = result, message = result ? "Saved successfully" : "Save failed" });
        }


        public ActionResult AssignInventoryTechReceive(Guid? eqpid, Guid? techid, int Qty, bool ISLocation)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            // int QtyCount = 0;
            //int AddQty = 0;
            //int ReleaseQty = 0;
            AssignedInventoryTechReceived model = new AssignedInventoryTechReceived();
            if (eqpid.HasValue && eqpid.Value != new Guid() && techid.HasValue && techid.Value != new Guid())
            {
                //model.ListInventoryTech = _Util.Facade.InventoryFacade.GetAllInventoryTechByTechnicianIdAndEquipmentIdAndType(techid.Value, eqpid.Value);
                //if (model.ListInventoryTech != null && model.ListInventoryTech.Count > 0)
                //{
                //    foreach (var item in model.ListInventoryTech)
                //    {
                //        if (item.Type == "Add")
                //        {
                //            AddQty = AddQty + item.Quantity;
                //        }
                //        if (item.Type == "Release")
                //        {
                //            ReleaseQty = ReleaseQty + item.Quantity;
                //        }
                //    }
                //}
                //TempData["IsLocation"] = ISLocation;
                ViewBag.IsLocation = ISLocation;
                ViewBag.QtyCount = Qty;
                List<SelectListItem> TechnicianList = new List<SelectListItem>();
                TechnicianList.Add(new SelectListItem()
                {
                    Text = "Please Select",
                    Value = new Guid().ToString()
                });
                //TechnicianList.Add(new SelectListItem()
                //{
                //    Text = "Warehouse",
                //    Value = "22222222-2222-2222-2222-222222222222"
                //});

                var TransferLocations = _Util.Facade.EmployeeFacade.GetTransferLocations(CurrentUser.CompanyId.Value);

                foreach (var location in TransferLocations)
                {
                    TechnicianList.Add(new SelectListItem()
                    {
                        Text = location.UserName,
                        Value = location.UserId.ToString(),
                        Selected = location.UserId.ToString() == "22222222-2222-2222-2222-222222222222",
                    });
                }
                TechnicianList.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTagAndTechnician(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, techid.Value).OrderBy(x => x.FirstName).Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                    Value = x.UserId.ToString()
                }).ToList());
                ViewBag.TechnicianList = TechnicianList;
                ViewBag.Equipmentid = eqpid.Value;
                ViewBag.technicianid = techid.Value;
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveTechInventoryReceive(AssignedInventoryTechReceived model, bool? request, int TotalQty)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            TechTransferRequest trf_request = new TechTransferRequest();
            model.ReqSrc = "[Inv-Tech-Trf]";
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = CurrentUser.UserId;

            trf_request.CreatedDate = DateTime.Now;
            trf_request.Items = new List<AssignedInventoryTechReceived>();
            trf_request.Items.Add(model);
            result = _Util.Facade.InventoryFacade.InsertTechTransfer(trf_request, CurrentUser.UserId) > 0;

            #region Old Code
            //if (model.EquipmentId != new Guid() && model.TechnicianId != new Guid())
            //{
            //    if (model.TechnicianId == new Guid("22222222-2222-2222-2222-222222222222"))
            //    {
            //        InventoryTech ReleaseInventoryTech = new InventoryTech()
            //        {
            //            CompanyId = CurrentUser.CompanyId.Value,
            //            TechnicianId = model.ReceivedBy.Value,
            //            EquipmentId = model.EquipmentId,
            //            Type = "Release",
            //            Quantity = model.Quantity,
            //            LastUpdatedBy = CurrentUser.UserId,
            //            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
            //            Description = "Release from technician to warehouse"
            //        };
            //        result = _Util.Facade.InventoryFacade.InsertInventoryTech(ReleaseInventoryTech) > 0;
            //        InventoryWarehouse AddInventoryWarehouse = new InventoryWarehouse()
            //        {
            //            CompanyId = CurrentUser.CompanyId.Value,
            //            EquipmentId = model.EquipmentId,
            //            Type = "Add",
            //            Quantity = model.Quantity,
            //            LastUpdatedBy = CurrentUser.UserId,
            //            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
            //            Description = "Receive by warehouse from technician"
            //        };
            //        result = _Util.Facade.InventoryFacade.InsertInventoryWareHouse(AddInventoryWarehouse) > 0;
            //    }
            //    else
            //    {
            //        var objtechreceive = _Util.Facade.InventoryFacade.GetTechReceiveByEquipmentIdAndTechnicianIdAndReceived(model.EquipmentId, model.TechnicianId);
            //        if (request.Value == false)
            //        {
            //            if (objtechreceive != null)
            //            {
            //                return Json(new { result = result, message = "This technician has got a receive request with quantity " + objtechreceive.Quantity + " but not received yet, Do you want to sent another request?" });
            //            }
            //            else
            //            {
            //                model.CreatedBy = CurrentUser.UserId;
            //                model.CreatedDate = DateTime.Now.UTCCurrentTime();
            //                model.ReceivedDate = DateTime.Now.UTCCurrentTime();
            //                result = _Util.Facade.InventoryFacade.InsertTechReceive(model) > 0;
            //                if (result)
            //                {
            //                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.TechnicianId);
            //                    if (empobj != null)
            //                    {
            //                        Notification notification = new Notification()
            //                        {
            //                            CompanyId = CurrentUser.CompanyId.Value,
            //                            NotificationId = Guid.NewGuid(),
            //                            Who = model.TechnicianId,
            //                            What = string.Format("{0} assign equipments for receiving", empobj.FirstName + " " + empobj.LastName),
            //                            CreatedDate = DateTime.Now.UTCCurrentTime(),
            //                            Type = "Technician",
            //                            NotificationUrl = "/Inventory#TechReceiveTab"
            //                        };
            //                        _Util.Facade.NotificationFacade.InsertNotification(notification);
            //                        NotificationUser NotificationUser = new NotificationUser()
            //                        {
            //                            NotificationId = notification.NotificationId,
            //                            NotificationPerson = notification.Who,
            //                            IsRead = false
            //                        };
            //                        _Util.Facade.NotificationFacade.InsertNotificationUser(NotificationUser);
            //                        var techobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.ReceivedBy.Value);
            //                        if (techobj != null)
            //                        {
            //                            InventoryTechReceiveNotificationEmail InventoryTechReceiveNotificationEmail = new InventoryTechReceiveNotificationEmail();
            //                            InventoryTechReceiveNotificationEmail.Name = empobj.FirstName + " " + empobj.LastName;
            //                            InventoryTechReceiveNotificationEmail.ToEmail = empobj.Email;
            //                            InventoryTechReceiveNotificationEmail.Body = string.Format("{0} have assigned equipments for receiving", techobj.FirstName + " " + techobj.LastName);
            //                            _Util.Facade.MailFacade.SendEmailAssignInventoryTechReceive(InventoryTechReceiveNotificationEmail, CurrentUser.CompanyId.Value);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            objtechreceive.Quantity = objtechreceive.Quantity + model.Quantity;
            //            if (objtechreceive.Quantity > TotalQty)
            //            {
            //                return Json(new { result = result, message = "Total Quantity limit exceed", request = request.Value });
            //            }
            //            else
            //            {
            //                objtechreceive.ReceivedDate = DateTime.Now.UTCCurrentTime();
            //                objtechreceive.IsApprove = model.IsApprove;
            //                result = _Util.Facade.InventoryFacade.UpdateTechReceive(objtechreceive);
            //                if (result)
            //                {
            //                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(objtechreceive.TechnicianId);
            //                    if (empobj != null)
            //                    {
            //                        Notification notification = new Notification()
            //                        {
            //                            CompanyId = CurrentUser.CompanyId.Value,
            //                            NotificationId = Guid.NewGuid(),
            //                            Who = model.TechnicianId,
            //                            What = string.Format("{0} assign equipments for receiving", empobj.FirstName + " " + empobj.LastName),
            //                            CreatedDate = DateTime.Now.UTCCurrentTime(),
            //                            Type = "Technician",
            //                            NotificationUrl = "/Inventory#TechReceiveTab"
            //                        };
            //                        _Util.Facade.NotificationFacade.InsertNotification(notification);
            //                        NotificationUser NotificationUser = new NotificationUser()
            //                        {
            //                            NotificationId = notification.NotificationId,
            //                            NotificationPerson = notification.Who,
            //                            IsRead = false
            //                        };
            //                        _Util.Facade.NotificationFacade.InsertNotificationUser(NotificationUser);
            //                        var techobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.ReceivedBy.Value);
            //                        if (techobj != null)
            //                        {
            //                            InventoryTechReceiveNotificationEmail InventoryTechReceiveNotificationEmail = new InventoryTechReceiveNotificationEmail();
            //                            InventoryTechReceiveNotificationEmail.Name = empobj.FirstName + " " + empobj.LastName;
            //                            InventoryTechReceiveNotificationEmail.ToEmail = empobj.Email;
            //                            InventoryTechReceiveNotificationEmail.Body = string.Format("{0} have assigned equipments for receiving", techobj.FirstName + " " + techobj.LastName);
            //                            _Util.Facade.MailFacade.SendEmailAssignInventoryTechReceive(InventoryTechReceiveNotificationEmail, CurrentUser.CompanyId.Value);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion Old Code

            return Json(new { result = result, message = "Saved successfully" });
        }

        public JsonResult SaveTechInventoryReceive_Old(AssignedInventoryTechReceived model, bool? request, int TotalQty)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (model.EquipmentId != new Guid() && model.TechnicianId != new Guid())
            {
                if (model.TechnicianId == new Guid("22222222-2222-2222-2222-222222222222"))
                {
                    InventoryTech ReleaseInventoryTech = new InventoryTech()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        TechnicianId = model.ReceivedBy.Value,
                        EquipmentId = model.EquipmentId,
                        Type = "Release",
                        Quantity = model.Quantity,
                        LastUpdatedBy = CurrentUser.UserId,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        Description = "Release from technician to warehouse"
                    };
                    result = _Util.Facade.InventoryFacade.InsertInventoryTech(ReleaseInventoryTech) > 0;
                    InventoryWarehouse AddInventoryWarehouse = new InventoryWarehouse()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        EquipmentId = model.EquipmentId,
                        Type = "Add",
                        Quantity = model.Quantity,
                        LastUpdatedBy = CurrentUser.UserId,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        Description = "Receive by warehouse from technician"
                    };
                    result = _Util.Facade.InventoryFacade.InsertInventoryWareHouse(AddInventoryWarehouse) > 0;
                }
                else
                {
                    var objtechreceive = _Util.Facade.InventoryFacade.GetTechReceiveByEquipmentIdAndTechnicianIdAndReceived(model.EquipmentId, model.TechnicianId);
                    if (request.Value == false)
                    {
                        if (objtechreceive != null)
                        {
                            return Json(new { result = result, message = "This technician has got a receive request with quantity " + objtechreceive.Quantity + " but not received yet, Do you want to sent another request?" });
                        }
                        else
                        {
                            model.CreatedBy = CurrentUser.UserId;
                            model.CreatedDate = DateTime.Now.UTCCurrentTime();
                            model.ReceivedDate = DateTime.Now.UTCCurrentTime();
                            result = _Util.Facade.InventoryFacade.InsertTechReceive(model) > 0;
                            if (result)
                            {
                                var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.TechnicianId);
                                if (empobj != null)
                                {
                                    Notification notification = new Notification()
                                    {
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        NotificationId = Guid.NewGuid(),
                                        Who = model.TechnicianId,
                                        What = string.Format("{0} assign equipments for receiving", empobj.FirstName + " " + empobj.LastName),
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        Type = "Technician",
                                        NotificationUrl = "/Inventory#TechReceiveTab"
                                    };
                                    _Util.Facade.NotificationFacade.InsertNotification(notification);
                                    NotificationUser NotificationUser = new NotificationUser()
                                    {
                                        NotificationId = notification.NotificationId,
                                        NotificationPerson = notification.Who,
                                        IsRead = false
                                    };
                                    _Util.Facade.NotificationFacade.InsertNotificationUser(NotificationUser);
                                    var techobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.ReceivedBy.Value);
                                    if (techobj != null)
                                    {
                                        InventoryTechReceiveNotificationEmail InventoryTechReceiveNotificationEmail = new InventoryTechReceiveNotificationEmail();
                                        InventoryTechReceiveNotificationEmail.Name = empobj.FirstName + " " + empobj.LastName;
                                        InventoryTechReceiveNotificationEmail.ToEmail = empobj.Email;
                                        InventoryTechReceiveNotificationEmail.Body = string.Format("{0} have assigned equipments for receiving", techobj.FirstName + " " + techobj.LastName);
                                        _Util.Facade.MailFacade.SendEmailAssignInventoryTechReceive(InventoryTechReceiveNotificationEmail, CurrentUser.CompanyId.Value);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        objtechreceive.Quantity = objtechreceive.Quantity + model.Quantity;
                        if (objtechreceive.Quantity > TotalQty)
                        {
                            return Json(new { result = result, message = "Total Quantity limit exceed", request = request.Value });
                        }
                        else
                        {
                            objtechreceive.ReceivedDate = DateTime.Now.UTCCurrentTime();
                            objtechreceive.IsApprove = model.IsApprove;
                            result = _Util.Facade.InventoryFacade.UpdateTechReceive(objtechreceive);
                            if (result)
                            {
                                var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(objtechreceive.TechnicianId);
                                if (empobj != null)
                                {
                                    Notification notification = new Notification()
                                    {
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        NotificationId = Guid.NewGuid(),
                                        Who = model.TechnicianId,
                                        What = string.Format("{0} assign equipments for receiving", empobj.FirstName + " " + empobj.LastName),
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        Type = "Technician",
                                        NotificationUrl = "/Inventory#TechReceiveTab"
                                    };
                                    _Util.Facade.NotificationFacade.InsertNotification(notification);
                                    NotificationUser NotificationUser = new NotificationUser()
                                    {
                                        NotificationId = notification.NotificationId,
                                        NotificationPerson = notification.Who,
                                        IsRead = false
                                    };
                                    _Util.Facade.NotificationFacade.InsertNotificationUser(NotificationUser);
                                    var techobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.ReceivedBy.Value);
                                    if (techobj != null)
                                    {
                                        InventoryTechReceiveNotificationEmail InventoryTechReceiveNotificationEmail = new InventoryTechReceiveNotificationEmail();
                                        InventoryTechReceiveNotificationEmail.Name = empobj.FirstName + " " + empobj.LastName;
                                        InventoryTechReceiveNotificationEmail.ToEmail = empobj.Email;
                                        InventoryTechReceiveNotificationEmail.Body = string.Format("{0} have assigned equipments for receiving", techobj.FirstName + " " + techobj.LastName);
                                        _Util.Facade.MailFacade.SendEmailAssignInventoryTechReceive(InventoryTechReceiveNotificationEmail, CurrentUser.CompanyId.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Json(new { result = result, message = "Saved successfully" });
        }

        [HttpPost]
        public JsonResult SaveTechInventoryApprove(AssignedInventoryTechReceived model)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (model.EquipmentId != new Guid())
            {
                model.ReceivedBy = CurrentUser.UserId;
                var objtechreceive = _Util.Facade.InventoryFacade.GetTechReceiveByEquipmentIdAndTechnicianIdAndReceivedAndApprove(model.EquipmentId, model.TechnicianId);
                if (objtechreceive != null)
                {
                    _Util.Facade.InventoryFacade.DeleteInventoryTechReceive(objtechreceive.Id);
                }
                model.CreatedBy = CurrentUser.UserId;
                model.CreatedDate = DateTime.Now.UTCCurrentTime();
                model.ReceivedDate = DateTime.Now.UTCCurrentTime();
                result = _Util.Facade.InventoryFacade.InsertTechReceive(model) > 0;
                if (result)
                {
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.TechnicianId);
                    var techobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.ReceivedBy.Value);
                    if (empobj != null)
                    {
                        Notification notification = new Notification()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            NotificationId = Guid.NewGuid(),
                            Who = model.TechnicianId,
                            What = string.Format("{0} sent request for approving", techobj.FirstName + " " + techobj.LastName),
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            Type = "Technician",
                            NotificationUrl = "/Inventory#TechReceiveTab"
                        };
                        _Util.Facade.NotificationFacade.InsertNotification(notification);
                        NotificationUser NotificationUser = new NotificationUser()
                        {
                            NotificationId = notification.NotificationId,
                            NotificationPerson = notification.Who,
                            IsRead = false
                        };
                        _Util.Facade.NotificationFacade.InsertNotificationUser(NotificationUser);

                        if (techobj != null)
                        {
                            InventoryTechReceiveNotificationEmail InventoryTechReceiveNotificationEmail = new InventoryTechReceiveNotificationEmail();
                            InventoryTechReceiveNotificationEmail.Name = empobj.FirstName + " " + empobj.LastName;
                            InventoryTechReceiveNotificationEmail.ToEmail = empobj.Email;
                            InventoryTechReceiveNotificationEmail.Body = string.Format("{0} sent request for approving", techobj.FirstName + " " + techobj.LastName);
                            _Util.Facade.MailFacade.SendEmailAssignInventoryTechReceive(InventoryTechReceiveNotificationEmail, CurrentUser.CompanyId.Value);
                        }
                    }
                }
            }
            return Json(result);
        }

        public ActionResult InventoryTechReceivePartial(string searchtext)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<Employee> model = new List<Employee>();
            if (CurrentUser.UserTags.ToLower().IndexOf("technician") > -1)
            {
                model = _Util.Facade.EmployeeFacade.GetTechtransferEmployeeByCompanyIdAndTag(CurrentUser.CompanyId.Value, searchtext, LabelHelper.UserTags.Technicians, CurrentUser.UserId);

                // model = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, CurrentUser.UserId);
            }
            else
            {
                model = _Util.Facade.EmployeeFacade.GetTechtransferEmployeeByCompanyIdAndTag(CurrentUser.CompanyId.Value, searchtext, LabelHelper.UserTags.Technicians, CurrentUser.UserId);

                // model = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid());
            }
            return View(model);
        }



        public ActionResult TechReceiveListPartial(TechReceiveFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            #region Permission Check
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.PurchaseOrderTechTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #endregion

            #region Inits
            if (filters == null)
            {
                filters = new TechReceiveFilter();
            }

            if (filters.PageNo == 0)
            {
                filters.PageNo = 1;
            }
            if (filters.EmployeeId == Guid.Empty)
            {
                filters.EmployeeId = CurrentUser.UserId;
            }
            filters.PageSize = _Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);
            TechReceiveListModel Model = new TechReceiveListModel();
            filters.CompanyId = CurrentUser.CompanyId.Value;
            #endregion

            Model = _Util.Facade.InventoryFacade.GetAllTechReceiveByTechnicianId(filters);
            ViewBag.PageNumber = filters.PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.PageNumberApprove = filters.PageNo;
            ViewBag.OutOfNumberApprove = 0;

            if (Model.ListAssignedInventoryTechReceived.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCountTrf;
            }
            if (Model.ListAssignedInventoryTechApprove.Count() > 0)
            {
                ViewBag.OutOfNumberApprove = Model.TotalCountRcv;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);

            if ((int)ViewBag.PageNumberApprove * filters.PageSize > (int)ViewBag.OutOfNumberApprove)
            {
                ViewBag.CurrentNumberApprove = (int)ViewBag.OutOfNumberApprove;
            }
            else
            {
                ViewBag.CurrentNumberApprove = (int)ViewBag.PageNumberApprove * filters.PageSize;
            }
            ViewBag.PageCountApprove = Math.Ceiling((double)ViewBag.OutOfNumberApprove / filters.PageSize);
            ViewBag.order = filters.order;
            ViewBag.EmployeeId = filters.EmployeeId;
            ViewBag.SearchText = filters.Searchtext;
            return View(Model);
        }

        [HttpPost]
        public JsonResult InventoryTechReceiveConfirm(Guid? eqpid, Guid? techid, int qty)
        {
            bool result = false;
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            AssignedInventoryTechReceived objassigntech = new AssignedInventoryTechReceived();
            int TotalQty = 0;
            int AddQty = 0;
            int ReleaseQty = 0;
            if (eqpid.HasValue && eqpid.Value != new Guid() && techid.HasValue && techid.Value != new Guid())
            {
                objassigntech = _Util.Facade.InventoryFacade.GetTechReceiveByEquipmentIdAndTechnicianIdAndReceivedAndIsApprove(eqpid.Value, techid.Value, qty);
                if (objassigntech != null)
                {
                    var objinvtech = _Util.Facade.InventoryFacade.GetAllInventoryTechByTechnicianIdAndEquipmentIdAndType(objassigntech.ReceivedBy.Value, eqpid.Value);
                    if (objinvtech != null && objinvtech.Count > 0)
                    {
                        foreach (var item in objinvtech)
                        {
                            if (item.Type == "Add")
                            {
                                AddQty = AddQty + item.Quantity;
                            }
                            if (item.Type == "Release")
                            {
                                ReleaseQty = ReleaseQty + item.Quantity;
                            }
                        }
                    }
                    TotalQty = AddQty - ReleaseQty;
                    if (qty > TotalQty)
                    {
                        return Json(new { result = result, message = "Total quantity limit exceed" });
                    }
                    else
                    {
                        objassigntech.ReceivedDate = DateTime.Now.UTCCurrentTime();
                        objassigntech.IsReceived = true;
                        objassigntech.ClosedBy = CurrentUser.UserId;
                        result = _Util.Facade.InventoryFacade.UpdateTechReceive(objassigntech);
                        if (result)
                        {
                            InventoryTech ReleaseInventoryTech = new InventoryTech()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                TechnicianId = objassigntech.ReceivedBy.Value,
                                EquipmentId = eqpid.Value,
                                Type = "Release",
                                Quantity = qty,
                                LastUpdatedBy = CurrentUser.UserId,
                                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                Description = "Receive by technician from technician"
                            };
                            _Util.Facade.InventoryFacade.InsertInventoryTech(ReleaseInventoryTech);
                            InventoryTech AddInventoryTech = new InventoryTech()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                TechnicianId = objassigntech.TechnicianId,
                                EquipmentId = eqpid.Value,
                                Type = "Add",
                                Quantity = qty,
                                LastUpdatedBy = CurrentUser.UserId,
                                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                Description = "Receive by technician from technician"
                            };
                            _Util.Facade.InventoryFacade.InsertInventoryTech(AddInventoryTech);
                        }
                    }
                }
            }
            return Json(new { result = result, message = "Received successfully" });
        }



        public ActionResult AssignInventoryTechApprove(Guid? eqpid, Guid? techid, int qty, int tqty)
        {
            AssignedInventoryTechReceived model = new AssignedInventoryTechReceived();
            if (eqpid.HasValue && eqpid.Value != new Guid() && techid.HasValue && techid.Value != new Guid())
            {
                model = _Util.Facade.InventoryFacade.GetTechReceiveByEquipmentIdAndTechnicianIdAndReceivedAndApprove(eqpid.Value, techid.Value);
            }
            ViewBag.qty = qty;
            ViewBag.tqty = tqty;
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveTechInventoryApproveConfirm(AssignedInventoryTechReceived model)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if (model.Id > 0)
            {
                var objassigntech = _Util.Facade.InventoryFacade.GetTechReceiveById(model.Id);
                if (objassigntech != null)
                {
                    objassigntech.TechnicianId = model.TechnicianId;
                    objassigntech.ReceivedBy = model.ReceivedBy;
                    objassigntech.IsApprove = model.IsApprove;
                    objassigntech.Quantity = model.Quantity;
                    result = _Util.Facade.InventoryFacade.UpdateTechReceive(objassigntech);
                }
                if (result)
                {
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.TechnicianId);
                    if (empobj != null)
                    {
                        Notification notification = new Notification()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            NotificationId = Guid.NewGuid(),
                            Who = model.TechnicianId,
                            What = string.Format("{0} assign equipments for receiving", empobj.FirstName + " " + empobj.LastName),
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            Type = "Technician",
                            NotificationUrl = "/Inventory#TechReceiveTab"
                        };
                        _Util.Facade.NotificationFacade.InsertNotification(notification);
                        NotificationUser NotificationUser = new NotificationUser()
                        {
                            NotificationId = notification.NotificationId,
                            NotificationPerson = notification.Who,
                            IsRead = false
                        };
                        _Util.Facade.NotificationFacade.InsertNotificationUser(NotificationUser);
                        var techobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.ReceivedBy.Value);
                        if (techobj != null)
                        {
                            InventoryTechReceiveNotificationEmail InventoryTechReceiveNotificationEmail = new InventoryTechReceiveNotificationEmail();
                            InventoryTechReceiveNotificationEmail.Name = empobj.FirstName + " " + empobj.LastName;
                            InventoryTechReceiveNotificationEmail.ToEmail = empobj.Email;
                            InventoryTechReceiveNotificationEmail.Body = string.Format("{0} have assigned equipments for receiving", techobj.FirstName + " " + techobj.LastName);
                            _Util.Facade.MailFacade.SendEmailAssignInventoryTechReceive(InventoryTechReceiveNotificationEmail, CurrentUser.CompanyId.Value);
                        }
                    }
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult InventoryTechReceiveDecline(int? id)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if (id.HasValue && id.Value > 0)
            {
                var objInventoryTechReceive = _Util.Facade.InventoryFacade.GetTechReceiveById(id.Value);
                if (objInventoryTechReceive != null)
                {
                    objInventoryTechReceive.ReceivedDate = DateTime.Now.UTCCurrentTime();
                    objInventoryTechReceive.IsDecline = true;
                    objInventoryTechReceive.ClosedBy = CurrentUser.UserId;
                    result = _Util.Facade.InventoryFacade.UpdateTechReceive(objInventoryTechReceive);
                }
            }
            return Json(new { result = result, message = "Transfer declined" });
        }

        public JsonResult EquipmentsImport(string File)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string serverFile = Server.MapPath(File);
            #region Import
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new MyExcel.XLWorkbook(ExcelFile.FullName);
                    MyExcel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();
                    Equipment Equipment = new Equipment();
                    Equipment tempEquipment = new Equipment();

                    List<Manufacturer> manufacturerList = _Util.Facade.ManufacturerFacade.GetAllManufacturerByCompanyId(CurrentUser.CompanyId.Value);
                    List<Supplier> supplierList = _Util.Facade.SupplierFacade.GetAllSupplierByCompanyId(CurrentUser.CompanyId.Value);
                    //List<EquipmentVendor> EquipmentVendorList = _Util.Facade.EquipmentFacade.GetAllEquipmentVendor();
                    for (int i = 2; i <= rowCount; i++)
                    {
                        try
                        {
                            var companycost = 0.0;
                            Guid vendorid = new Guid();
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        }
                                        else if (header.ToLower() == "product name" || header.ToLower() == "productname")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                Equipment.EquipmentId = Guid.NewGuid();
                                                Equipment.Name = value;
                                            }
                                            else
                                            {
                                                Equipment.EquipmentId = new Guid();
                                                break;
                                            }
                                            continue;
                                        }
                                        else if (header.ToLower() == "manufacturer")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                Manufacturer Manufacturer = manufacturerList.Where(x => x.Name == value).FirstOrDefault();

                                                if (Manufacturer != null)
                                                {
                                                    Equipment.ManufacturerId = Manufacturer.Id;
                                                }
                                                else
                                                {
                                                    Manufacturer Manu = new Manufacturer()
                                                    {
                                                        ManufacturerId = Guid.NewGuid(),
                                                        Name = value,
                                                        CompanyId = CurrentUser.CompanyId.Value,
                                                        IsActive = true
                                                    };
                                                    Manu.Id = _Util.Facade.ManufacturerFacade.InsertManufacturer(Manu);
                                                    Equipment.ManufacturerId = Manu.Id;
                                                    manufacturerList.Add(Manu);
                                                }
                                            }
                                            continue;
                                        }
                                        else if (header.ToLower() == "sku")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                Equipment.SKU = value;

                                                tempEquipment = _Util.Facade.EquipmentFacade.GetEquipmentBySKU(value);
                                            }
                                            continue;
                                        }
                                        else if (header.ToLower() == "description")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                Equipment.Description = value;
                                            }
                                            continue;
                                        }
                                        else if (header.ToLower() == "retail price")
                                        {
                                            double RetailPrice = 0;
                                            double.TryParse(value, out RetailPrice);
                                            Equipment.Retail = RetailPrice;
                                            continue;
                                        }
                                        else if (header.ToLower() == "company cost")
                                        {
                                            double.TryParse(value, out companycost);
                                            continue;
                                        }
                                        else if (header.ToLower() == "cost/supplier cost" || header.ToLower() == "supplier cost" || header.ToLower() == "cost")
                                        {
                                            double cost = 0;
                                            double.TryParse(value, out cost);
                                            Equipment.SupplierCost = cost;
                                            continue;
                                        }
                                        else if (header.ToLower() == "rep cost" || header.ToLower() == "repcost")
                                        {
                                            double cost = 0;
                                            double.TryParse(value, out cost);
                                            Equipment.RepCost = cost;
                                            continue;
                                        }
                                        else if (header.ToLower() == "points")
                                        {
                                            double cost = 0;
                                            double.TryParse(value, out cost);
                                            Equipment.Point = cost;
                                            continue;
                                        }
                                        else if (header.ToLower() == "vendor")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                Supplier Supplier = supplierList.Where(x => x.CompanyName == value).FirstOrDefault();
                                                if (Supplier != null)
                                                {
                                                    Equipment.SupplierId = Supplier.Id;
                                                    vendorid = Supplier.SupplierId;
                                                }
                                                else
                                                {
                                                    Supplier sup = new Supplier()
                                                    {
                                                        SupplierId = Guid.NewGuid(),
                                                        CompanyName = value,
                                                        CompanyId = CurrentUser.CompanyId.Value,
                                                        IsActive = true
                                                    };
                                                    sup.Id = _Util.Facade.SupplierFacade.InsertSupplier(sup);
                                                    Equipment.SupplierId = sup.Id;
                                                    vendorid = sup.SupplierId;
                                                    supplierList.Add(sup);
                                                }
                                            }
                                            continue;
                                        }
                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                            if (Equipment != null && Equipment.EquipmentId != new Guid())
                            {
                                Guid EquipmentId = Equipment.EquipmentId;

                                if (tempEquipment == null || tempEquipment == new Equipment() || string.IsNullOrWhiteSpace(tempEquipment.Name))
                                {
                                    #region Equipment Insert
                                    Equipment eqp = new Equipment()
                                    {
                                        EquipmentId = Equipment.EquipmentId,
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        EquipmentTypeId = -1,
                                        EquipmentClassId = 1,
                                        ManufacturerId = Equipment.ManufacturerId,
                                        SupplierId = Equipment.SupplierId,
                                        SKU = Equipment.SKU,
                                        SupplierCost = Equipment.SupplierCost,
                                        RepCost = Equipment.RepCost,
                                        Point = Equipment.Point,
                                        Retail = Equipment.Retail,
                                        IsActive = true,
                                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                        LastUpdatedBy = CurrentUser.Identity.Name,
                                        Name = Equipment.Name
                                    };
                                    _Util.Facade.EquipmentFacade.InsertEquipment(eqp);
                                    #endregion
                                }
                                else
                                {
                                    EquipmentId = tempEquipment.EquipmentId;

                                    #region Equipment Update
                                    tempEquipment.Name = Equipment.Name;
                                    tempEquipment.ManufacturerId = Equipment.ManufacturerId;
                                    tempEquipment.SupplierId = Equipment.SupplierId;
                                    tempEquipment.SupplierCost = Equipment.SupplierCost;
                                    tempEquipment.RepCost = Equipment.RepCost;
                                    tempEquipment.Point = Equipment.Point;
                                    tempEquipment.Retail = Equipment.Retail;
                                    _Util.Facade.EquipmentFacade.UpdateEquipment(tempEquipment);
                                    #endregion
                                }

                                if (vendorid != null && vendorid != new Guid())
                                {
                                    //_Util.Facade.EquipmentFacade.UpdateEquipmentVendorSetIsPrimaryFalse(EquipmentId);

                                    _Util.Facade.EquipmentFacade.DeleteEquipmentVendorByEquipmentId(EquipmentId);
                                    EquipmentVendor EquipmentVendor = new EquipmentVendor()
                                    {
                                        SupplierId = vendorid,
                                        EquipmentId = EquipmentId,
                                        Cost = companycost,
                                        IsPrimary = true,
                                        AddedBy = CurrentUser.UserId,
                                        AddedDate = DateTime.Now.UTCCurrentTime()
                                    };
                                    _Util.Facade.EquipmentFacade.InsertEquipmentVendor(EquipmentVendor);
                                }
                                // EquipmentVendorList.Add(EquipmentVendor);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            #endregion
            return Json(1);
        }

        #region Equipment Imort for Richmond
        public JsonResult EquipmentImportForRichmond(string File)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string serverFile = Server.MapPath(File);
            #region Import
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new MyExcel.XLWorkbook(ExcelFile.FullName);
                    MyExcel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();
                    Equipment Equipment = new Equipment();
                    //List<Manufacturer> manufacturerList = _Util.Facade.ManufacturerFacade.GetAllManufacturerByCompanyId(CurrentUser.CompanyId.Value);
                    List<Lookup> unitList = _Util.Facade.LookupFacade.GetLookupByKey("EquipmentUnit");
                    List<Supplier> supplierList = _Util.Facade.SupplierFacade.GetAllSupplierByCompanyId(CurrentUser.CompanyId.Value);
                    List<EquipmentType> eqpTypeList = _Util.Facade.EquipmentTypeFacade.GetAllProductCategoryByCompanyId(CurrentUser.CompanyId.Value);
                    //List<EquipmentVendor> EquipmentVendorList = _Util.Facade.EquipmentFacade.GetAllEquipmentVendor();
                    for (int i = 2; i <= rowCount; i++)
                    {
                        try
                        {
                            var companycost = 0.0;
                            Guid vendorid = new Guid();
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        }
                                        else if (header.ToLower() == "item")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                Equipment.EquipmentId = Guid.NewGuid();
                                                Equipment.Name = value;
                                            }
                                            else
                                            {
                                                Equipment.EquipmentId = new Guid();
                                                break;
                                            }
                                            continue;
                                        }
                                        else if (header.ToLower() == "unit")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                //Manufacturer Manufacturer = manufacturerList.Where(x => x.Name == value).FirstOrDefault();
                                                Lookup lookup = unitList.Where(x => x.DisplayText.ToLower() == value.ToLower()).FirstOrDefault();
                                                if (lookup != null)
                                                {
                                                    Equipment.Unit = lookup.DataValue;
                                                }
                                                else
                                                {
                                                    Lookup _unit = new Lookup()
                                                    {
                                                        DataKey = "EquipmentUnit",
                                                        DisplayText = value,
                                                        DataValue = value,
                                                        DataOrder = unitList.Count(),
                                                        CompanyId = CurrentUser.CompanyId.Value,
                                                        IsActive = true
                                                    };
                                                    _Util.Facade.LookupFacade.InsertLookup(_unit);
                                                    Equipment.Unit = _unit.DataValue;
                                                    unitList.Add(_unit);
                                                }
                                            }
                                            continue;
                                        }
                                        else if (header.ToLower() == "default variation/ sku" || header.ToLower() == "sku" || header.ToLower() == "default variation")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                Equipment.SKU = value;
                                            }
                                            continue;
                                        }
                                        else if (header.ToLower() == "notes")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                Equipment.Comments = value;
                                            }
                                            continue;
                                        }
                                        else if (header.ToLower() == "classid")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value) && value.ToLower() == "materials")
                                            {
                                                Equipment.EquipmentClassId = 1;
                                            }
                                            else
                                            {
                                                Equipment.EquipmentClassId = 2;
                                            }
                                            continue;
                                        }
                                        else if (header.ToLower() == "cost/supplier cost" || header.ToLower() == "supplier cost" || header.ToLower() == "cost")
                                        {
                                            double cost = 0;
                                            double.TryParse(value, out cost);
                                            Equipment.SupplierCost = cost;
                                            continue;
                                        }

                                        else if (header.ToLower() == "supplier")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                Supplier Supplier = supplierList.Where(x => x.CompanyName.ToLower() == value.ToLower()).FirstOrDefault();
                                                if (Supplier != null)
                                                {
                                                    Equipment.SupplierId = Supplier.Id;
                                                    vendorid = Supplier.SupplierId;
                                                }
                                                else
                                                {
                                                    Supplier sup = new Supplier()
                                                    {
                                                        SupplierId = Guid.NewGuid(),
                                                        CompanyName = value,
                                                        CompanyId = CurrentUser.CompanyId.Value,
                                                        IsActive = true
                                                    };
                                                    sup.Id = _Util.Facade.SupplierFacade.InsertSupplier(sup);
                                                    Equipment.SupplierId = sup.Id;
                                                    vendorid = sup.SupplierId;
                                                    supplierList.Add(sup);
                                                }
                                            }
                                            continue;
                                        }
                                        else if (header.ToLower() == "category/equipment type" || header.ToLower() == "category" || header.ToLower() == "equipment type")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                EquipmentType eqpType = eqpTypeList.Where(x => x.Name.ToLower() == value.ToLower()).FirstOrDefault();
                                                if (eqpType != null)
                                                {
                                                    Equipment.EquipmentTypeId = eqpType.Id;
                                                }
                                                else
                                                {
                                                    EquipmentType _eqpType = new EquipmentType()
                                                    {
                                                        Name = value,
                                                        CompanyId = CurrentUser.CompanyId.Value,
                                                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                                        LastUpdatedBy = CurrentUser.Identity.Name,
                                                        IsActive = true
                                                    };
                                                    _eqpType.Id = Convert.ToInt32(_Util.Facade.EquipmentTypeFacade.InsertEquipmentType(_eqpType));
                                                    Equipment.EquipmentTypeId = _eqpType.Id;
                                                    eqpTypeList.Add(_eqpType);
                                                }
                                            }
                                            continue;
                                        }
                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                            if (Equipment != null && Equipment.EquipmentId != new Guid())
                            {
                                #region Equipment Insert
                                Equipment eqp = new Equipment()
                                {
                                    EquipmentId = Equipment.EquipmentId,
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    EquipmentTypeId = Equipment.EquipmentTypeId,
                                    EquipmentClassId = Equipment.EquipmentClassId,
                                    Unit = Equipment.Unit,
                                    Comments = Equipment.Comments,
                                    ManufacturerId = 0,
                                    SupplierId = Equipment.SupplierId,
                                    SKU = Equipment.SKU,
                                    SupplierCost = Equipment.SupplierCost,
                                    IsActive = true,
                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                    LastUpdatedBy = CurrentUser.Identity.Name,
                                    Name = Equipment.Name
                                };
                                _Util.Facade.EquipmentFacade.InsertEquipment(eqp);
                                #endregion
                                if (vendorid != null && vendorid != new Guid())
                                {
                                    EquipmentVendor EquipmentVendor = new EquipmentVendor()
                                    {
                                        SupplierId = vendorid,
                                        EquipmentId = Equipment.EquipmentId,
                                        Cost = companycost,
                                        IsPrimary = true,
                                        AddedBy = CurrentUser.UserId,
                                        AddedDate = DateTime.Now.UTCCurrentTime()
                                    };
                                    _Util.Facade.EquipmentFacade.InsertEquipmentVendor(EquipmentVendor);
                                }

                                // EquipmentVendorList.Add(EquipmentVendor);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            #endregion
            return Json(1);
        }
        #endregion

        #region BT Team Rio

        [Authorize]
        public PartialViewResult EquipmentHistoryPartial(DetailedHistoryFilter filters)
        {
            List<SelectListItem> EstimatorIdList = new List<SelectListItem>();
            List<string> selectedTechsList = new List<string>();
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            ViewBag.LoggedUserName = CurrentUser.FirstName + " " + CurrentUser.LastName;
            ViewBag.LoggedUserId = CurrentUser.UserId;

            if (filters == null)
            {
                filters = new DetailedHistoryFilter();
            }

            if (filters.EquipmentIds != null)
            {
                filters.EquipmentIds = filters.EquipmentIds[0].Split(',');
            }

            if (filters.EmployeeIds != null)
            {
                filters.EmployeeIds = filters.EmployeeIds[0].Split(',');
            }

            EstimatorIdList.Add(new SelectListItem()
            {
                Text = "Select Estimate Id",
                Value = "-1"
            });

            string PerGrpAssgnTicketId = "";
            GlobalSetting PerGrpAssgnTicket = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "TicketAssignPermissionGroup");
            if (PerGrpAssgnTicket != null && !string.IsNullOrWhiteSpace(PerGrpAssgnTicket.Value))
            {
                PerGrpAssgnTicketId = PerGrpAssgnTicket.Value;
            }

            //var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);
            //TechnicianList = TechnicianList.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            var TechnicianList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);

            var emplst = new List<SelectListItem>();

            if (TechnicianList != null)
            {
                if (base.IsPermitted(UserPermissions.InventoryPermissions.TechDropDownEditable))
                {
                    emplst.Add(new SelectListItem()
                    {
                        Text = "Warehouse",
                        Value = "22222222-2222-2222-2222-222222222222",
                        Selected = false,
                    });
                }
                foreach (var tech in TechnicianList)
                {
                    emplst.Add(new SelectListItem()
                    {
                        Text = tech.FirstName + " " + tech.LastName,
                        Value = tech.UserId.ToString(),
                        Selected = tech.UserId == CurrentUser.UserId ? true : false,
                    });
                    if (tech.UserId == CurrentUser.UserId)
                    {
                        selectedTechsList.Add(tech.UserId.ToString());
                    }

                }
            }

            ViewBag.TechList = emplst; // TechList;

            List<string> selectsts = new List<string>();

            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectsts.Add(item);
                    }
                }
            }
            ViewBag.selectedTechsList = selectedTechsList;
            ViewBag.selectsts = selectsts;
            ViewBag.EstimatorId = filters.EstimatorId;
            return PartialView();

        }

        public ActionResult EquipmentHistoryListPartial(DetailedHistoryFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (filters.EquipmentIds[0] != "undefined")
            {
                filters.EquipmentIds = filters.EquipmentIds[0].Split(',');
            }

            if (filters.EmployeeIds[0] != "undefined")
            {
                filters.EmployeeIds = filters.EmployeeIds[0].Split(',');
            }
            if (filters.ManufacturerIds[0] != "undefined")
            {
                filters.ManufacturerIds = filters.ManufacturerIds[0].Split(',');
            }

            List<string> selectSts = new List<string>();
            List<string> selectedEquipmentsList = new List<string>();
            List<string> selectedManufacturersList = new List<string>();
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectSts.Add(item);
                    }
                }
            }

            #region Inits
            if (filters == null)
            {
                filters = new DetailedHistoryFilter();
            }
            //if (filters.EquipmentIds == null)
            //{
            //    filters.EquipmentIds = new int[] { 0 };
            //}

            if (filters.PageNo == 0)
            {
                filters.PageNo = 1;
            }
            filters.CompanyId = CurrentUser.CompanyId.Value;
            //filters.EmployeeIds = filters.EmployeeIds;

            DetailedHistoryVM Model = new DetailedHistoryVM();
            #endregion

            if (filters.GetReport)
            {
                DataTable dt = _Util.Facade.InventoryFacade.GetDetailedHistoryListByFiltersR(filters).Tables[0];
                //dt.Columns.Remove("Total RMR");
                int[] colarray = { 2 };
                int[] rowarray = { dt.Rows.Count + 2 };
                return MakeExcelFromDataTable(dt, "EquipmentHistory", rowarray, colarray);
            }

            Model = _Util.Facade.InventoryFacade.GetDetailedHistoryListByFilters(filters);


            ViewBag.PageNumber = filters.PageNo;
            ViewBag.OutOfNumber = 0;
            filters.PageSize = Model.ItemsCount; //_Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);

            if (Model.ItemsCount > 0)
            {
                ViewBag.OutOfNumber = Model.ItemsCount;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);
            ViewBag.order = filters.order;
            string poPreText = "PO";
            GlobalSetting _poPretxt = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PrefixForPurchaseOrderId");
            if (_poPretxt != null && !string.IsNullOrWhiteSpace(_poPretxt.Value) && _poPretxt.Value != "")
            {
                poPreText = _poPretxt.Value;
            }


            bool TechnicianColumnPO = false;
            GlobalSetting tehcnicianColumn = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PurchaseOrderTechnicianColumn");
            if (tehcnicianColumn != null && !string.IsNullOrWhiteSpace(tehcnicianColumn.Value))
            {
                TechnicianColumnPO = Convert.ToBoolean(tehcnicianColumn.Value);
            }
            ViewBag.TechnicianColumnPO = TechnicianColumnPO;

            var EquipmentsList = new List<SelectListItem>();
            var ManufacturersList = new List<SelectListItem>();

            foreach (var item in Model.EquipmentsList)
            {
                var listItem = new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value,
                    Selected = true
                };


                if (filters.EquipmentIds.Count() > 0 && filters.EquipmentIds[0] != "undefined")
                {
                    if (!filters.EquipmentIds.Contains(item.Value))
                    {
                        listItem.Selected = false;
                    }
                    else
                    {
                        selectedEquipmentsList.Add(item.Value);
                    }
                }
                else
                {
                    selectedEquipmentsList.Add(item.Value);
                }

                EquipmentsList.Add(listItem);

            }

            foreach (var item in Model.ManufacturersList)
            {
                var listItem = new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value,
                    Selected = true
                };

                if (filters.ManufacturerIds.Count() > 0 && filters.ManufacturerIds[0] != "undefined")
                {
                    if (!filters.ManufacturerIds.Contains(item.Value))
                    {
                        listItem.Selected = false;
                    }
                    else
                    {
                        selectedManufacturersList.Add(item.Value);
                    }
                }
                else
                {
                    selectedManufacturersList.Add(item.Value);
                }

                ManufacturersList.Add(listItem);
            }

            ViewBag.EqList = EquipmentsList;
            ViewBag.MfgList = ManufacturersList;
            @ViewBag.selectedEquipmentsList = selectedEquipmentsList;
            @ViewBag.selectedManufacturersList = selectedManufacturersList;
            return View(Model);
        }

        #endregion BT Team Rio

        public JsonResult EquipmentImportForRichmondV2(string File)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string serverFile = Server.MapPath(File);
            #region Import
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new MyExcel.XLWorkbook(ExcelFile.FullName);
                    MyExcel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();
                    Equipment Equipment = new Equipment();
                    Manufacturer Manufacturer = null;
                    List<Manufacturer> manufacturerList = _Util.Facade.ManufacturerFacade.GetAllManufacturerByCompanyId(CurrentUser.CompanyId.Value);
                    List<Lookup> unitList = _Util.Facade.LookupFacade.GetLookupByKey("EquipmentUnit");
                    //List<Supplier> supplierList = _Util.Facade.SupplierFacade.GetAllSupplierByCompanyId(CurrentUser.CompanyId.Value);
                    List<EquipmentType> eqpTypeList = _Util.Facade.EquipmentTypeFacade.GetAllProductCategoryByCompanyId(CurrentUser.CompanyId.Value);
                    //List<EquipmentVendor> EquipmentVendorList = _Util.Facade.EquipmentFacade.GetAllEquipmentVendor();
                    for (int i = 2; i <= rowCount; i++)
                    {
                        try
                        {
                            var companycost = 0.0;
                            Guid ManufacturerId = new Guid();
                            string SKU = "";
                            string Variation = "";
                            bool IsEqpExists = false;
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        }
                                        else if (header.ToLower() == "name")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {

                                                Equipment ExistingEqp = _Util.Facade.InventoryFacade.GetEquipmentByName(value);
                                                if (ExistingEqp != null)
                                                {
                                                    Equipment = ExistingEqp;
                                                    IsEqpExists = true;
                                                }
                                                else
                                                {
                                                    Equipment.EquipmentId = Guid.NewGuid();
                                                    Equipment.Name = value;
                                                }
                                            }
                                            else
                                            {
                                                Equipment.EquipmentId = new Guid();
                                                break;
                                            }
                                            continue;
                                        }
                                        else if (header.ToLower() == "unit")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                //Manufacturer Manufacturer = manufacturerList.Where(x => x.Name == value).FirstOrDefault();
                                                Lookup lookup = unitList.Where(x => x.DisplayText.ToLower() == value.ToLower()).FirstOrDefault();
                                                if (lookup != null)
                                                {
                                                    Equipment.Unit = lookup.DataValue;
                                                }
                                                else
                                                {
                                                    Lookup _unit = new Lookup()
                                                    {
                                                        DataKey = "EquipmentUnit",
                                                        DisplayText = value,
                                                        DataValue = value,
                                                        DataOrder = unitList.Count(),
                                                        CompanyId = CurrentUser.CompanyId.Value,
                                                        IsActive = true
                                                    };
                                                    _Util.Facade.LookupFacade.InsertLookup(_unit);
                                                    Equipment.Unit = _unit.DataValue;
                                                    unitList.Add(_unit);
                                                }
                                            }
                                            continue;
                                        }

                                        else if (header.ToLower() == "sku")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                SKU = value;
                                            }
                                            continue;
                                        }
                                        else if (header.ToLower() == "variation")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                Variation = value;
                                            }
                                            continue;
                                        }
                                        else if (header.ToLower() == "cost")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                double Cost = 0;
                                                double.TryParse(value, out Cost);
                                                Equipment.RepCost = Cost;
                                            }
                                            continue;
                                        }
                                        else if (header.ToLower() == "manufacturer")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                Manufacturer = manufacturerList.Where(x => x.Name.Trim().ToLower() == value.Trim().ToLower()).FirstOrDefault();
                                                if (Manufacturer != null)
                                                {
                                                    //Equipment.SupplierId = Manufacturer.Id;
                                                    ManufacturerId = Manufacturer.ManufacturerId;
                                                }
                                                else
                                                {
                                                    Manufacturer = new Manufacturer()
                                                    {
                                                        ManufacturerId = Guid.NewGuid(),
                                                        Name = value.Trim(),
                                                        CompanyId = CurrentUser.CompanyId.Value,
                                                        IsActive = true
                                                    };
                                                    Manufacturer.Id = _Util.Facade.ManufacturerFacade.InsertManufacturer(Manufacturer);
                                                    //Equipment.SupplierId = Manufacturer.Id;
                                                    ManufacturerId = Manufacturer.ManufacturerId;
                                                    manufacturerList.Add(Manufacturer);
                                                }
                                            }
                                            continue;
                                        }
                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                            if (Equipment != null && Equipment.EquipmentId != new Guid())
                            {
                                #region Equipment Insert
                                if (!IsEqpExists)
                                {
                                    Equipment eqp = new Equipment()
                                    {
                                        EquipmentId = Equipment.EquipmentId,
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        EquipmentTypeId = 1152, //Equipment.EquipmentTypeId,//Material
                                        EquipmentClassId = 1,
                                        Unit = Equipment.Unit,
                                        Comments = Equipment.Comments,
                                        ManufacturerId = 0,
                                        SupplierId = Equipment.SupplierId,
                                        SKU = Equipment.SKU,
                                        SupplierCost = Equipment.SupplierCost,
                                        RepCost = Equipment.RepCost,
                                        IsActive = true,
                                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                        LastUpdatedBy = CurrentUser.Identity.Name,
                                        Name = Equipment.Name,
                                    };
                                    _Util.Facade.EquipmentFacade.InsertEquipment(eqp);
                                }


                                #endregion
                                if (ManufacturerId != null && ManufacturerId != new Guid())
                                {
                                    EquipmentManufacturer EquipmentManufacturer = new EquipmentManufacturer()
                                    {
                                        ManufacturerId = ManufacturerId,
                                        EquipmentId = Equipment.EquipmentId,
                                        Cost = companycost,
                                        IsPrimary = true,
                                        AddedBy = CurrentUser.UserId,
                                        AddedDate = DateTime.Now.UTCCurrentTime(),
                                        SKU = SKU,
                                        Variation = Variation

                                    };
                                    _Util.Facade.EquipmentFacade.InsertEquipmentManufacturer(EquipmentManufacturer);
                                }

                                // EquipmentVendorList.Add(EquipmentVendor);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            #endregion
            return Json(1);
        }

        public ActionResult FavouriteEquipmentPartial(Guid? EquipmentId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            EquipmentsFavourite model = new EquipmentsFavourite();
            List<Employee> emplist = _Util.Facade.EmployeeFacade.GetAllEmployeeByCompanyId(CurrentUser.CompanyId.Value, null);
            if (EquipmentId.HasValue && EquipmentId.Value != new Guid())
            {
                var objeqpfavlist = _Util.Facade.InventoryFacade.GetAllEquipmentsFavouriteByEquipmentId(EquipmentId.Value, CurrentUser.CompanyId.Value);
                if (objeqpfavlist != null && objeqpfavlist.Count > 0)
                {
                    model = objeqpfavlist.FirstOrDefault();
                    ViewBag.FavouriteList = emplist.Select(x => new SelectListItem()
                    {
                        Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                        Value = x.UserId.ToString(),
                        Selected = objeqpfavlist.Count(y => y.UserId == x.UserId) > 0
                    }).ToList();
                }
                else
                {
                    ViewBag.FavouriteList = emplist.Select(x => new SelectListItem()
                    {
                        Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                        Value = x.UserId.ToString()
                    }).ToList();
                }
                model.EquipmentId = EquipmentId.Value;
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveFavouriteEquipments(EquipmentsFavourite ef)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            if (ef.UserList != null && ef.UserList.Count > 0)
            {
                foreach (var item in ef.UserList)
                {
                    ef.UserId = new Guid(item);
                    ef.CompanyId = CurrentUser.CompanyId.Value;
                    ef.EquipmentId = ef.EquipmentId;
                    ef.IsFavourite = true;
                    ef.CreatedBy = CurrentUser.UserId;
                    ef.CreatedDate = DateTime.Now.UTCCurrentTime();
                    result = _Util.Facade.InventoryFacade.InsertEquipmentsFavourite(ef) > 0;
                }
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult ProductHistory(string tab)
        {
            ViewBag.tab = tab;
            return PartialView();
        }
        [Authorize]
        public ActionResult ProductHistoryPartial(int pageNo, int pagesize, string SearchText, Guid UserId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            InventoryHistoryModel Model = new InventoryHistoryModel();
            Model = _Util.Facade.InventoryFacade.GetProductHistory(pageNo, pagesize, SearchText, currentLoggedIn.CompanyId.Value, UserId);

            ViewBag.PageNumber = pageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = null;
            if (ViewBag.order == null)
            {
                ViewBag.order = 0;
            }
            if (Model.TotalProductCount.TotalProduct > 0)
            {
                ViewBag.OutOfNumber = Model.TotalProductCount.TotalProduct;
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
            return View(Model);
        }
        #region MakeExcel
        private FileContentResult MakeExcelFromDataTable(DataTable dtResult, string ReportFor, int[] rowIndex, int[] col_Format_Number)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                if (dtResult != null)
                {

                    var worksheet = wb.Worksheets.Add(dtResult);
                    if (ReportFor == "BrinksReport")
                    {
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;
                        worksheet.AutoFilter.Enabled = false;

                        worksheet.Column(1).Style.Font.SetFontName("Times New Roman");
                        worksheet.Column(2).Style.Font.SetFontName("Times New Roman");
                        worksheet.Column(3).Style.Font.SetFontName("Times New Roman");
                        worksheet.Column(4).Style.Font.SetFontName("Times New Roman");
                        worksheet.Column(5).Style.Font.SetFontName("Times New Roman");
                        worksheet.Column(6).Style.Font.SetFontName("Times New Roman");
                        worksheet.Column(7).Style.Font.SetFontName("Times New Roman");
                        worksheet.Column(1).Style.Font.SetFontSize(12);
                        worksheet.Column(2).Style.Font.SetFontSize(12);
                        worksheet.Column(3).Style.Font.SetFontSize(12);
                        worksheet.Column(4).Style.Font.SetFontSize(12);
                        worksheet.Column(5).Style.Font.SetFontSize(12);
                        worksheet.Column(6).Style.Font.SetFontSize(12);
                        worksheet.Column(7).Style.Font.SetFontSize(12);

                        worksheet.Column(1).Width = 13.71;
                        worksheet.Column(2).Width = 13.71;
                        worksheet.Column(3).Width = 13.71;
                        worksheet.Column(4).Width = 13.71;
                        worksheet.Column(5).Width = 13.71;
                        worksheet.Column(6).Width = 13.71;
                        worksheet.Column(7).Width = 13.71;



                        worksheet.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        worksheet.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        worksheet.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        worksheet.Ranges("A1:G1").Style.Font.Bold = true;
                        worksheet.Cells("A1:G1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cells("A1:D1").Style.Font.SetFontSize(11);
                        worksheet.Cells("A1:D1").Style.Font.SetFontName("Calibri");
                        worksheet.Cells("A1:D1").Style.Font.SetBold();
                        worksheet.Cells("A1:G1").Style.Font.SetFontColor(XLColor.CoolBlack);
                        worksheet.Cells("A1:D1").Style.Fill.BackgroundColor = XLColor.Yellow;
                        worksheet.Cells("E1:G1").Style.Fill.BackgroundColor = XLColor.AshGrey;


                    }
                    var format = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("CurrentCurrencyExcelFormat");
                    if (col_Format_Number != null && format != null && rowIndex != null)
                    {
                        foreach (int itemcol in col_Format_Number)
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
                        return File(fileContents, ExcelCl.Format("ExcelFormat"), fName);
                    }
                    else
                    {
                        return File(fileContents, ExcelCl.Format("ExcelFormat"), fName);
                    }
                }
                else
                {
                    byte[] fileContents = new byte[1];
                    return File(fileContents, ExcelCl.Format("ExcelFormat"), "empty.xlsx");
                }
            }
        }

        #endregion

        #region Digiture Changes

        [Authorize]
        public PartialViewResult DetailedHistoryPartial(DetailedHistoryFilter filters)
        {
            List<SelectListItem> EstimatorIdList = new List<SelectListItem>();
            List<string> selectedTechsList = new List<string>();
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            ViewBag.LoggedUserName = CurrentUser.FirstName + " " + CurrentUser.LastName;
            ViewBag.LoggedUserId = CurrentUser.UserId;

            if (filters == null)
            {
                filters = new DetailedHistoryFilter();
            }

            if (filters.EquipmentIds != null)
            {
                filters.EquipmentIds = filters.EquipmentIds[0].Split(',');
            }

            if (filters.EmployeeIds != null)
            {
                filters.EmployeeIds = filters.EmployeeIds[0].Split(',');
            }

            EstimatorIdList.Add(new SelectListItem()
            {
                Text = "Select Estimate Id",
                Value = "-1"
            });

            string PerGrpAssgnTicketId = "";
            GlobalSetting PerGrpAssgnTicket = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "TicketAssignPermissionGroup");
            if (PerGrpAssgnTicket != null && !string.IsNullOrWhiteSpace(PerGrpAssgnTicket.Value))
            {
                PerGrpAssgnTicketId = PerGrpAssgnTicket.Value;
            }

            //var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);
            //TechnicianList = TechnicianList.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            var TechnicianList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);

            TechnicianList = TechnicianList.OrderBy(tech => tech.FirstName).ThenBy(tech => tech.LastName).ToList();

            var emplst = new List<SelectListItem>();

            if (TechnicianList != null)
            {
                //if (base.IsPermitted(UserPermissions.InventoryPermissions.TechDropDownEditable))
                //{
                //    emplst.Add(new SelectListItem()
                //    {
                //        Text = "Warehouse",
                //        Value = "22222222-2222-2222-2222-222222222222",
                //        Selected = false,
                //    });
                //}
                foreach (var tech in TechnicianList)
                {
                    emplst.Add(new SelectListItem()
                    {
                        Text = tech.FirstName + " " + tech.LastName,
                        Value = tech.UserId.ToString(),
                        Selected = tech.UserId == CurrentUser.UserId ? true : false,
                    });
                    if (tech.UserId == CurrentUser.UserId)
                    {
                        selectedTechsList.Add(tech.UserId.ToString());
                    }

                }
            }

            ViewBag.TechList = emplst; // TechList;

            List<string> selectsts = new List<string>();

            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectsts.Add(item);
                    }
                }
            }
            ViewBag.selectedTechsList = selectedTechsList;
            ViewBag.selectsts = selectsts;
            ViewBag.EstimatorId = filters.EstimatorId;
            return PartialView("_DetailedHistoryPartial");

        }

        public ActionResult DetailedHistoryList2Partial(DetailedHistoryFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (filters.EquipmentIds[0] != "undefined")
            {
                filters.EquipmentIds = filters.EquipmentIds[0].Split(',');
            }

            if (filters.EmployeeIds[0] != "undefined")
            {
                filters.EmployeeIds = filters.EmployeeIds[0].Split(',');
            }
            if (filters.ManufacturerIds[0] != "undefined")
            {
                filters.ManufacturerIds = filters.ManufacturerIds[0].Split(',');
            }

            if (filters.Start == "01/01/0001" || filters.Start == null)
            {
                filters.Start = null;
            }
            if (filters.End == "01/01/0001" || filters.End == null)
            {
                var Today = DateTime.Today;
                filters.End = DateTime.Today.ToString("MM/dd/yyyy");
            }


            List<string> selectSts = new List<string>();
            List<string> selectedEquipmentsList = new List<string>();
            List<string> selectedManufacturersList = new List<string>();
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectSts.Add(item);
                    }
                }
            }

            #region Inits
            if (filters == null)
            {
                filters = new DetailedHistoryFilter();
            }
            //if (filters.EquipmentIds == null)
            //{
            //    filters.EquipmentIds = new int[] { 0 };
            //}

            if (filters.PageNo == 0)
            {
                filters.PageNo = 1;
            }
            filters.CompanyId = CurrentUser.CompanyId.Value;
            //filters.EmployeeIds = filters.EmployeeIds;

            DetailedHistoryVM Model = new DetailedHistoryVM();
            #endregion

            if (filters.GetReport)
            {
                DataTable dt = _Util.Facade.InventoryFacade.GetDetailedHistoryListByFiltersR(filters).Tables[0];
                //dt.Columns.Remove("Total RMR");
                int[] colarray = { 2 };
                int[] rowarray = { dt.Rows.Count + 2 };
                return MakeExcelFromDataTable(dt, "EquipmentHistory", rowarray, colarray);
            }

            Model = _Util.Facade.InventoryFacade.GetDetailedHistoryListByFilters(filters);


            ViewBag.PageNumber = filters.PageNo;
            ViewBag.OutOfNumber = 0;
            filters.PageSize = Model.ItemsCount; //_Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);

            if (Model.ItemsCount > 0)
            {
                ViewBag.OutOfNumber = Model.ItemsCount;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);
            ViewBag.order = filters.order;
            string poPreText = "PO";
            GlobalSetting _poPretxt = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PrefixForPurchaseOrderId");
            if (_poPretxt != null && !string.IsNullOrWhiteSpace(_poPretxt.Value) && _poPretxt.Value != "")
            {
                poPreText = _poPretxt.Value;
            }


            bool TechnicianColumnPO = false;
            GlobalSetting tehcnicianColumn = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PurchaseOrderTechnicianColumn");
            if (tehcnicianColumn != null && !string.IsNullOrWhiteSpace(tehcnicianColumn.Value))
            {
                TechnicianColumnPO = Convert.ToBoolean(tehcnicianColumn.Value);
            }
            ViewBag.TechnicianColumnPO = TechnicianColumnPO;

            var EquipmentsList = new List<SelectListItem>();
            var ManufacturersList = new List<SelectListItem>();

            foreach (var item in Model.EquipmentsList)
            {
                var listItem = new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value,
                    Selected = true
                };


                if (filters.EquipmentIds.Count() > 0 && filters.EquipmentIds[0] != "undefined")
                {
                    if (!filters.EquipmentIds.Contains(item.Value))
                    {
                        listItem.Selected = false;
                    }
                    else
                    {
                        selectedEquipmentsList.Add(item.Value);
                    }
                }
                else
                {
                    selectedEquipmentsList.Add(item.Value);
                }

                EquipmentsList.Add(listItem);

            }

            foreach (var item in Model.ManufacturersList)
            {
                var listItem = new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value,
                    Selected = true
                };

                if (filters.ManufacturerIds.Count() > 0 && filters.ManufacturerIds[0] != "undefined")
                {
                    if (!filters.ManufacturerIds.Contains(item.Value))
                    {
                        listItem.Selected = false;
                    }
                    else
                    {
                        selectedManufacturersList.Add(item.Value);
                    }
                }
                else
                {
                    selectedManufacturersList.Add(item.Value);
                }

                ManufacturersList.Add(listItem);
            }

            ViewBag.EqList = EquipmentsList;
            ViewBag.MfgList = ManufacturersList;
            @ViewBag.selectedEquipmentsList = selectedEquipmentsList;
            @ViewBag.selectedManufacturersList = selectedManufacturersList;
            return View("_DetailedHistoryList2Partial", Model);
        }
        public ActionResult DetailedHistoryListPartial(DetailedHistoryFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            //#region Permission Check
            //if (!base.IsPermitted(UserPermissions.InventoryPermissions.PurchaseOrderWareHouseTab))
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}
            //#endregion

            if (filters.EquipmentIds[0] != "undefined")
            {
                filters.EquipmentIds = filters.EquipmentIds[0].Split(',');
            }

            if (filters.EmployeeIds[0] != "undefined")
            {
                filters.EmployeeIds = filters.EmployeeIds[0].Split(',');
            }

            List<string> selectSts = new List<string>();
            List<string> selectedEquipmentsList = new List<string>();
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectSts.Add(item);
                    }
                }
            }

            #region Inits
            if (filters == null)
            {
                filters = new DetailedHistoryFilter();
            }
            //if (filters.EquipmentIds == null)
            //{
            //    filters.EquipmentIds = new int[] { 0 };
            //}

            if (filters.PageNo == 0)
            {
                filters.PageNo = 1;
            }
            filters.CompanyId = CurrentUser.CompanyId.Value;
            //filters.EmployeeIds = filters.EmployeeIds;
            filters.PageSize = _Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);
            DetailedHistoryVM Model = new DetailedHistoryVM();
            #endregion
            //filters.IsAllTechPO = true;
            //if (!base.IsPermitted(UserPermissions.InventoryPermissions.TechAllInventory))
            //{
            //    filters.IsAllTechPO = false;
            //}
            //Model = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderListByFilters(filters);
            Model = _Util.Facade.InventoryFacade.GetDetailedHistoryListByFilters(filters);
            ViewBag.PageNumber = filters.PageNo;
            ViewBag.OutOfNumber = 0;

            if (Model.ItemsCount > 0)
            {
                ViewBag.OutOfNumber = Model.ItemsCount;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);
            ViewBag.order = filters.order;
            string poPreText = "PO";
            GlobalSetting _poPretxt = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PrefixForPurchaseOrderId");
            if (_poPretxt != null && !string.IsNullOrWhiteSpace(_poPretxt.Value) && _poPretxt.Value != "")
            {
                poPreText = _poPretxt.Value;
            }


            bool TechnicianColumnPO = false;
            GlobalSetting tehcnicianColumn = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PurchaseOrderTechnicianColumn");
            if (tehcnicianColumn != null && !string.IsNullOrWhiteSpace(tehcnicianColumn.Value))
            {
                TechnicianColumnPO = Convert.ToBoolean(tehcnicianColumn.Value);
            }
            ViewBag.TechnicianColumnPO = TechnicianColumnPO;

            var EquipmentsList = new List<SelectListItem>();

            foreach (var equip in Model.Items)
            {
                var listItem = new SelectListItem()
                {
                    Text = equip.EquipmentName,
                    Value = equip.EquipmentId.ToString(),
                    Selected = true
                };

                if (filters.EquipmentIds.Count() > 0 && filters.EquipmentIds[0] != "undefined")
                {
                    if (!filters.EquipmentIds.Contains(equip.EquipmentId.ToString()))
                    {
                        listItem.Selected = false;
                    }
                    else
                    {
                        selectedEquipmentsList.Add(equip.EquipmentId.ToString());
                    }
                }
                else
                {
                    selectedEquipmentsList.Add(equip.EquipmentId.ToString());
                }


                EquipmentsList.Add(listItem);
            }

            ViewBag.EqList = EquipmentsList;
            @ViewBag.selectedEquipmentsList = selectedEquipmentsList;
            return View("_DetailedHistoryListPartial", Model);
        }

        public ActionResult InventoryDataPagePartial(DetailedHistoryFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (filters.EquipmentIds[0] != "undefined")
            {
                filters.EquipmentIds = filters.EquipmentIds[0].Split(',');
            }

            if (filters.EmployeeIds[0] != "undefined")
            {
                filters.EmployeeIds = filters.EmployeeIds[0].Split(',');
            }
            if (filters.ManufacturerIds[0] != "undefined")
            {
                filters.ManufacturerIds = filters.ManufacturerIds[0].Split(',');
            }

            List<string> selectSts = new List<string>();
            List<string> selectedEquipmentsList = new List<string>();
            List<string> selectedManufacturersList = new List<string>();
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectSts.Add(item);
                    }
                }
            }

            #region Inits
            if (filters == null)
            {
                filters = new DetailedHistoryFilter();
            }
            //if (filters.EquipmentIds == null)
            //{
            //    filters.EquipmentIds = new int[] { 0 };
            //}

            if (filters.PageNo == 0)
            {
                filters.PageNo = 1;
            }
            filters.CompanyId = CurrentUser.CompanyId.Value;
            //filters.EmployeeIds = filters.EmployeeIds;

            DetailedHistoryVM Model = new DetailedHistoryVM();
            #endregion

            if (filters.GetReport)
            {
                DataTable dt = _Util.Facade.InventoryFacade.GetDetailedHistoryListByFiltersR(filters).Tables[0];
                //dt.Columns.Remove("Total RMR");
                int[] colarray = { 2, 4, 5, 6, 7 };
                int[] rowarray = { dt.Rows.Count + 2 };
                return MakeExcelFromDataTable(dt, "EquipmentHistory", rowarray, colarray);
            }

            Model = _Util.Facade.InventoryFacade.GetDetailedHistoryListByFilters(filters);


            ViewBag.PageNumber = filters.PageNo;
            ViewBag.OutOfNumber = 0;
            filters.PageSize = Model.ItemsCount; //_Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);

            if (Model.ItemsCount > 0)
            {
                ViewBag.OutOfNumber = Model.ItemsCount;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);
            ViewBag.order = filters.order;
            string poPreText = "PO";
            GlobalSetting _poPretxt = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PrefixForPurchaseOrderId");
            if (_poPretxt != null && !string.IsNullOrWhiteSpace(_poPretxt.Value) && _poPretxt.Value != "")
            {
                poPreText = _poPretxt.Value;
            }


            bool TechnicianColumnPO = false;
            GlobalSetting tehcnicianColumn = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PurchaseOrderTechnicianColumn");
            if (tehcnicianColumn != null && !string.IsNullOrWhiteSpace(tehcnicianColumn.Value))
            {
                TechnicianColumnPO = Convert.ToBoolean(tehcnicianColumn.Value);
            }
            ViewBag.TechnicianColumnPO = TechnicianColumnPO;

            var EquipmentsList = new List<SelectListItem>();
            var ManufacturersList = new List<SelectListItem>();

            foreach (var item in Model.EquipmentsList)
            {
                var listItem = new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value,
                    Selected = true
                };


                if (filters.EquipmentIds.Count() > 0 && filters.EquipmentIds[0] != "undefined")
                {
                    if (!filters.EquipmentIds.Contains(item.Value))
                    {
                        listItem.Selected = false;
                    }
                    else
                    {
                        selectedEquipmentsList.Add(item.Value);
                    }
                }
                else
                {
                    selectedEquipmentsList.Add(item.Value);
                }

                EquipmentsList.Add(listItem);

            }

            foreach (var item in Model.ManufacturersList)
            {
                var listItem = new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value,
                    Selected = true
                };

                if (filters.ManufacturerIds.Count() > 0 && filters.ManufacturerIds[0] != "undefined")
                {
                    if (!filters.ManufacturerIds.Contains(item.Value))
                    {
                        listItem.Selected = false;
                    }
                    else
                    {
                        selectedManufacturersList.Add(item.Value);
                    }
                }
                else
                {
                    selectedManufacturersList.Add(item.Value);
                }

                ManufacturersList.Add(listItem);
            }

            ViewBag.EqList = EquipmentsList;
            ViewBag.MfgList = ManufacturersList;
            @ViewBag.selectedEquipmentsList = selectedEquipmentsList;
            @ViewBag.selectedManufacturersList = selectedManufacturersList;
            return View("_DetailedHistoryList2Partial", Model);
        }

        [Authorize]
        public PartialViewResult InventoryDataPage(DetailedHistoryFilter filters)
        {
            List<SelectListItem> EstimatorIdList = new List<SelectListItem>();
            List<string> selectedTechsList = new List<string>();
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            //logger.WithProperty("tags", "equipment,history")
            //.WithProperty("params", JsonConvert.SerializeObject(filters))
            //.Debug("Equipment History by {employee} for {period}", CurrentUser.GetFullName(), filters.Start +" to "+ filters.End);

            //logger.WithProperty("property1", JsonConvert.SerializeObject(filters)).WithProperty("property2","testing data").Trace("Equipment History by {employee} for {period}", CurrentUser.GetFullName(), filters.Start + " to " + filters.End);
            ViewBag.LoggedUserName = CurrentUser.FirstName + " " + CurrentUser.LastName;
            ViewBag.LoggedUserId = CurrentUser.UserId;

            if (filters == null)
            {
                filters = new DetailedHistoryFilter();
            }

            if (filters.EquipmentIds != null)
            {
                filters.EquipmentIds = filters.EquipmentIds[0].Split(',');
            }

            if (filters.EmployeeIds != null)
            {
                filters.EmployeeIds = filters.EmployeeIds[0].Split(',');
            }

            EstimatorIdList.Add(new SelectListItem()
            {
                Text = "Select Estimate Id",
                Value = "-1"
            });

            string PerGrpAssgnTicketId = "";
            GlobalSetting PerGrpAssgnTicket = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "TicketAssignPermissionGroup");
            if (PerGrpAssgnTicket != null && !string.IsNullOrWhiteSpace(PerGrpAssgnTicket.Value))
            {
                PerGrpAssgnTicketId = PerGrpAssgnTicket.Value;
            }

            //var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);
            //TechnicianList = TechnicianList.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            var TechnicianList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);

            var emplst = new List<SelectListItem>();

            if (TechnicianList != null)
            {
                if (base.IsPermitted(UserPermissions.InventoryPermissions.TechDropDownEditable))
                {
                    emplst.Add(new SelectListItem()
                    {
                        Text = "Warehouse",
                        Value = "22222222-2222-2222-2222-222222222222",
                        Selected = false,
                    });
                }
                foreach (var tech in TechnicianList)
                {
                    emplst.Add(new SelectListItem()
                    {
                        Text = tech.FirstName + " " + tech.LastName,
                        Value = tech.UserId.ToString(),
                        Selected = tech.UserId == CurrentUser.UserId ? true : false,
                    });
                    if (tech.UserId == CurrentUser.UserId)
                    {
                        selectedTechsList.Add(tech.UserId.ToString());
                    }

                }
            }

            ViewBag.TechList = emplst; // TechList;

            List<string> selectsts = new List<string>();

            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectsts.Add(item);
                    }
                }
            }
            ViewBag.selectedTechsList = selectedTechsList;
            ViewBag.selectsts = selectsts;
            ViewBag.EstimatorId = filters.EstimatorId;
            logger.Info("Information started {0}", DateTime.Now);
            return PartialView("_DetailedHistoryPartial");

        }


        [Authorize]
        public ActionResult TechTransferPartial(PurchaseOrderFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<SelectListItem> EstimatorIdList = new List<SelectListItem>();

            EstimatorIdList.Add(new SelectListItem()
            {
                Text = "Select Estimate Id",
                Value = "-1"
            });
            EstimatorIdList.AddRange(_Util.Facade.PurchaseOrderFacade.GetEstimatorIdListOfPurchaseOrder().Select(x =>
                                       new SelectListItem()
                                       {
                                           Text = x.Id,
                                           Value = x.Id
                                       }).ToList());
            ViewBag.EstimatorIdList = EstimatorIdList;

            List<string> selectsts = new List<string>();
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectsts.Add(item);
                    }
                }
            }
            string PerGrpAssgnTicketId = "";
            var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);

            TechnicianList = TechnicianList.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            //List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);
            var emplst = new List<SelectListItem>();
            if (TechnicianList != null)
            {
                emplst = TechnicianList.Where(y => y.UserId != CurrentUser.UserId).Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                    Selected = true,
                }).ToList();
                if (emplst.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault() != null)
                {
                    emplst.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";
                }
            }
            ViewBag.selectsts = selectsts;
            ViewBag.EstimatorId = filters.EstimatorId;
            ViewBag.EqList = emplst;
            return PartialView();
        }

        [Authorize]
        public ActionResult TechTransferListPartial(TechTransferFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<string> selectedTechsTrfList = new List<string>();
            List<string> selectedTechsRcvList = new List<string>();

            ViewBag.LoggedUserName = CurrentUser.FirstName + " " + CurrentUser.LastName;
            ViewBag.LoggedUserId = CurrentUser.UserId;

            #region Inits
            if (filters == null)
            {
                filters = new TechTransferFilter();
            }

            if (filters.PageNoTrf == 0)
            {
                filters.PageNoTrf = 1;
            }

            if (filters.PageNoRcv == 0)
            {
                filters.PageNoRcv = 1;
            }

            filters.EmployeeId = CurrentUser.UserId;

            if (filters.TTEmployeeIds != null)
            {
                filters.TTEmployeeIds = filters.TTEmployeeIds[0].Split(',');
            }

            if (filters.RTEmployeeIds != null)
            {
                filters.RTEmployeeIds = filters.RTEmployeeIds[0].Split(',');
            }

            filters.PageSize = _Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);
            TechReceiveListModel Model = new TechReceiveListModel();
            filters.CompanyId = CurrentUser.CompanyId.Value;
            #endregion

            string PerGrpAssgnTicketId = "";
            GlobalSetting PerGrpAssgnTicket = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "TicketAssignPermissionGroup");
            if (PerGrpAssgnTicket != null && !string.IsNullOrWhiteSpace(PerGrpAssgnTicket.Value))
            {
                PerGrpAssgnTicketId = PerGrpAssgnTicket.Value;
            }

            //var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);

            if (filters.GetReport)
            {
                DataSet ds = _Util.Facade.InventoryFacade.GetTechTransferListByFiltersR(filters);
                //dt.Columns.Remove("Total RMR");
                int[] colarray = null; // { 2, 4, 5, 6, 7 };
                int[] rowarray = null; // { dt.Rows.Count + 2 };
                return MakeExcelFromDataSet(ds, "TechTransfer", rowarray, colarray);
            }

            Model = _Util.Facade.InventoryFacade.GetTechTransferListByFilters(filters);

            var TechTrfList = new List<SelectListItem>();
            var TechRcvList = new List<SelectListItem>();

            foreach (var item in Model.ListAssignedInventoryTechApprove.Where(x => x.ReceivedBy == Guid.Parse("22222222-2222-2222-2222-222222222222")))
            {
                item.ReceivedByName = "Warehouse";
            }

            //foreach (var item in Model.ListAssignedInventoryTechApprove.Where(x => x.TechnicianId == Guid.Parse("22222222-2222-2222-2222-222222222222")))
            //{
            //    item.TransferByName = "Warehouse";
            //}

            //foreach (var item in Model.ListAssignedInventoryTechReceived.Where(x => x.ReceivedBy == Guid.Parse("22222222-2222-2222-2222-222222222222")))
            //{
            //    item.ReceivedByName = "Warehouse";
            //}

            foreach (var item in Model.ListAssignedInventoryTechReceived.Where(x => x.TechnicianId == Guid.Parse("22222222-2222-2222-2222-222222222222")))
            {
                item.ReceivedByName = "Warehouse";
            }

            //if (.FirstOrDefault()!=null)
            //    Model.ListAssignedInventoryTechApprove.Where(x => x.ReceivedBy == Guid.Parse("22222222-2222-2222-2222-222222222222")).FirstOrDefault().ReceivedByName = "Warehouse";
            //if(Model.ListAssignedInventoryTechReceived.Where(x => x.ReceivedBy == Guid.Parse("22222222-2222-2222-2222-222222222222")).FirstOrDefault()!=null)
            //    Model.ListAssignedInventoryTechReceived.Where(x => x.ReceivedBy == Guid.Parse("22222222-2222-2222-2222-222222222222")).FirstOrDefault().ReceivedByName = "Warehouse";

            if (Model.TechTrfFromList != null)
            {
                //if (base.IsPermitted(UserPermissions.InventoryPermissions.TechDropDownEditable))
                //{
                //    TechTrfList.Add(new SelectListItem()
                //    {
                //        Text = "Warehouse",
                //        Value = "22222222-2222-2222-2222-222222222222",
                //        Selected = true,
                //    });

                //    TechRcvList.Add(new SelectListItem()
                //    {
                //        Text = "Warehouse",
                //        Value = "22222222-2222-2222-2222-222222222222",
                //        Selected = true,
                //    });
                //}


                if (Model.TechTrfFromList.Any(f => f.Value == "22222222-2222-2222-2222-222222222222"))
                {
                    Model.TechTrfFromList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";
                }


                foreach (var tech in Model.TechTrfFromList)
                {
                    tech.Selected = true;
                    if (filters.TTEmployeeIds[0] != "undefined")
                    {
                        if (!filters.TTEmployeeIds.Contains(tech.Value))
                        {
                            tech.Selected = false;
                        }
                    }

                    TechTrfList.Add(new SelectListItem()
                    {
                        Text = tech.Text,
                        Value = tech.Value,
                        Selected = tech.Selected,
                    });
                    if (tech.Selected) selectedTechsTrfList.Add(tech.Value);
                }
                //TechTrfList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";

                foreach (var tech in Model.TechRcvFromList)
                {
                    tech.Selected = true;
                    if (filters.RTEmployeeIds[0] != "undefined")
                    {
                        if (!filters.RTEmployeeIds.Contains(tech.Value))
                        {
                            tech.Selected = false;
                        }
                    }

                    TechRcvList.Add(new SelectListItem()
                    {
                        Text = tech.Text,
                        Value = tech.Value,
                        Selected = true,
                    });
                    if (tech.Selected) selectedTechsRcvList.Add(tech.Value);
                }
                //TechRcvList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";



                //foreach (var tech in Model.ListAssignedInventoryTechReceived.OrderBy(x => x.ReceivedByName))
                //{
                //    if (TechTrfList.Where(x => x.Value == tech.TechnicianId.ToString()).Count() == 0)
                //    {
                //        TechRcvList.Add(new SelectListItem()
                //        {
                //            Text = tech.ReceivedByName,
                //            Value = tech.TechnicianId.ToString(),
                //            Selected = tech.TechnicianId == CurrentUser.UserId ? true : false,
                //        });
                //        if (tech.ReceivedBy == CurrentUser.UserId)
                //        {
                //            selectedTechsRcvList.Add(tech.TechnicianId.ToString());
                //        }
                //    }
                //}
            }


            ViewBag.PageNumber = filters.PageNoRcv;
            ViewBag.OutOfNumber = 0;
            ViewBag.PageNumberApprove = filters.PageNoTrf;
            ViewBag.OutOfNumberApprove = 0;

            if (Model.TotalCountTrf > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCountRcv;
            }
            if (Model.TotalCountRcv > 0)
            {
                ViewBag.OutOfNumberApprove = Model.TotalCountTrf;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);

            if ((int)ViewBag.PageNumberApprove * filters.PageSize > (int)ViewBag.OutOfNumberApprove)
            {
                ViewBag.CurrentNumberApprove = (int)ViewBag.OutOfNumberApprove;
            }
            else
            {
                ViewBag.CurrentNumberApprove = (int)ViewBag.PageNumberApprove * filters.PageSize;
            }
            ViewBag.PageCountApprove = Math.Ceiling((double)ViewBag.OutOfNumberApprove / filters.PageSize);
            ViewBag.order = filters.order;
            ViewBag.EmployeeId = filters.EmployeeId;
            ViewBag.SearchText = filters.Searchtext;

            ViewBag.TechTrfList = TechTrfList; // TechList;
            ViewBag.TechRcvList = TechRcvList; // TechList;
            ViewBag.selectedTechsTrfList = selectedTechsTrfList;
            ViewBag.selectedTechsRcvList = selectedTechsRcvList;
            ViewBag.AllowApprove = false;
            if (base.IsPermitted(UserPermissions.InventoryPermissions.TechDropDownEditable) || base.IsPermitted(UserPermissions.InventoryPermissions.TechTransferApprove) || (filters.RTEmployeeIds[0] == "undefined"))
            {
                ViewBag.AllowApprove = true;
            }
            return View(Model);
        }

        [Authorize]
        public ActionResult DetailedHistory(DetailedHistoryFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            //#region Permission Check
            //if (!base.IsPermitted(UserPermissions.InventoryPermissions.EquipHistoryTab))
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}
            //#endregion

            if (filters.Start == null)
            {
                filters.Start = "01/01/1901";
            }
            if (filters.End == null)
            {
                filters.End = DateTime.UtcNow.ToString("MM-dd-yyyy");
            }

            if (filters.EquipmentIds != null)
            {
                filters.EquipmentIds = filters.EquipmentIds[0].Split(',');
            }

            if (filters.EmployeeIds != null)
            {
                filters.EmployeeIds = filters.EmployeeIds[0].Split(',');
            }

            List<string> selectSts = new List<string>();
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectSts.Add(item);
                    }
                }
            }

            #region Inits
            if (filters == null)
            {
                filters = new DetailedHistoryFilter();
            }

            if (filters.PageNo == 0)
            {
                filters.PageNo = 1;
            }
            filters.CompanyId = CurrentUser.CompanyId.Value;
            //filters.EmployeeIds = filters.EmployeeIds;
            filters.PageSize = _Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);
            DetailedEquipmentVM Model = new DetailedEquipmentVM();
            #endregion

            Model = _Util.Facade.InventoryFacade.GetDetailedEquipmentListByFilters(filters);
            ViewBag.PageNumberTickets = filters.PageNo;
            ViewBag.OutOfNumberTickets = 0;
            ViewBag.PageNumberTransfers = filters.PageNoTransfers;
            ViewBag.OutOfNumberTransfers = 0;

            if (Model.TicketsCount > 0)
            {
                ViewBag.OutOfNumberTickets = Model.TicketsCount;
            }

            if (Model.TransfersCount > 0)
            {
                ViewBag.OutOfNumberTransfers = Model.TransfersCount;
            }

            if ((int)ViewBag.PageNumberTickets * filters.PageSize > (int)ViewBag.OutOfNumberTickets)
            {
                ViewBag.CurrentNumberTickets = (int)ViewBag.OutOfNumberTickets;
            }
            else
            {
                ViewBag.CurrentNumberTickets = (int)ViewBag.PageNumberTickets * filters.PageSize;
            }

            if ((int)ViewBag.PageNumberTransfers * filters.PageSize > (int)ViewBag.OutOfNumberTransfers)
            {
                ViewBag.CurrentNumberTransfers = (int)ViewBag.OutOfNumberTransfers;
            }
            else
            {
                ViewBag.CurrentNumberTransfers = (int)ViewBag.PageNumberTransfers * filters.PageSize;
            }

            ViewBag.PageCountTickets = Math.Ceiling((double)ViewBag.OutOfNumberTickets / filters.PageSize);
            ViewBag.PageCountTransfers = Math.Ceiling((double)ViewBag.OutOfNumberTransfers / filters.PageSize);
            ViewBag.order = filters.order;
            string poPreText = "PO";
            GlobalSetting _poPretxt = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PrefixForPurchaseOrderId");
            if (_poPretxt != null && !string.IsNullOrWhiteSpace(_poPretxt.Value) && _poPretxt.Value != "")
            {
                poPreText = _poPretxt.Value;
            }

            return View("_DetailedHistory", Model);
        }

        [Authorize]
        public ActionResult AddTechTransfer(int? Id, string PurchaseOrderId, bool? Receive, string OpenTab, Guid? EmployeeId, int? BranchId)
        {

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var BranchList = _Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(CurrentUser.CompanyId.Value);
            TechTransferRequest model = new TechTransferRequest();
            model.ItemList = new List<TechTransferRequestItem>();
            model.Supplier = new Supplier();
            model.Supplier.SupplierId = new Guid();
            model.PurchaseOrderDetail = new List<PurchaseOrderDetail>();
            model.PurchaseOrderWarehouse = new PurchaseOrderWarehouse();
            model.PurchaseOrderWarehouse.OrderDate = DateTime.Now;

            string PerGrpAssgnTicketId = "";
            GlobalSetting PerGrpAssgnTicket = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "TicketAssignPermissionGroup");
            if (PerGrpAssgnTicket != null && !string.IsNullOrWhiteSpace(PerGrpAssgnTicket.Value))
            {
                PerGrpAssgnTicketId = PerGrpAssgnTicket.Value;
            }

            //var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);
            //TechnicianList = TechnicianList.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            var TechnicianList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);

            var emplst = new List<SelectListItem>();
            if (TechnicianList != null)
            {

                emplst = TechnicianList.Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                    Selected = x.UserId == CurrentUser.UserId ? true : false,
                }).ToList();

                if (base.IsPermitted(UserPermissions.InventoryPermissions.TechDropDownEditable))
                {
                    var TransferLocations = _Util.Facade.EmployeeFacade.GetTransferLocations(CurrentUser.CompanyId.Value);

                    foreach (var location in TransferLocations)
                    {
                        emplst.Add(new SelectListItem()
                        {
                            Text = location.UserName,
                            Value = location.UserId.ToString(),
                            Selected = location.UserId.ToString() == "22222222-2222-2222-2222-222222222222" ? true : false,
                        });
                    }

                    //emplst.Add(new SelectListItem()
                    //{
                    //    Text = "Commercial lost bucket",
                    //    Value = "22222222-2222-2222-2222-222222222223"
                    //});

                    //emplst.Add(new SelectListItem()
                    //{
                    //    Text = "Residential lost bucket",
                    //    Value = "22222222-2222-2222-2222-222222222224"
                    //});

                    //emplst.Add(new SelectListItem()
                    //{
                    //    Text = "Warehouse lost bucket",
                    //    Value = "22222222-2222-2222-2222-222222222225"
                    //});

                    //emplst.Add(new SelectListItem()
                    //{
                    //    Text = "RMA getting equipment back bucket",
                    //    Value = "22222222-2222-2222-2222-222222222226"
                    //});

                    //emplst.Add(new SelectListItem()
                    //{
                    //    Text = "RMA getting a refund",
                    //    Value = "22222222-2222-2222-2222-222222222231"
                    //});

                    //emplst.Add(new SelectListItem()
                    //{
                    //    Text = "Everything else bucket",
                    //    Value = "22222222-2222-2222-2222-222222222232"
                    //});
                }
            }

            ViewBag.OpenTab = OpenTab;
            ViewBag.EmployeeList = emplst.OrderBy(x => x.Text).ThenBy(x => x.Text).ToList();
            return View(model);
        }

        //Transfer
        [HttpPost]
        public JsonResult TechTransferRequest(TechTransferRequest model)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            model.CreatedBy = CurrentUser.UserId;
            foreach (var item in model.Items)
            {
                item.ReqSrc = "[TT-Approve]";
                if (item.TechnicianId.ToString().Equals("22222222-2222-2222-2222-222222222222")) item.ReqSrc = "[WHTT-Approve]";
                if (item.ReceivedBy.ToString().Equals("22222222-2222-2222-2222-222222222222")) item.ReqSrc = "[TTWH-Approve]";


                if (item.TechnicianId.ToString().Equals("22222222-2222-2222-2222-222222222223")) item.ReqSrc = "[SLTT-Approve]";
                if (item.ReceivedBy.ToString().Equals("22222222-2222-2222-2222-222222222223")) item.ReqSrc = "[TTSL-Approve]";


                if (item.TechnicianId.ToString().Equals("22222222-2222-2222-2222-222222222224")) item.ReqSrc = "[SLTT-Approve]";
                if (item.ReceivedBy.ToString().Equals("22222222-2222-2222-2222-222222222224")) item.ReqSrc = "[TTSL-Approve]";



                if (item.TechnicianId.ToString().Equals("22222222-2222-2222-2222-222222222225")) item.ReqSrc = "[SLTT-Approve]";
                if (item.ReceivedBy.ToString().Equals("22222222-2222-2222-2222-222222222225")) item.ReqSrc = "[TTSL-Approve]";

                if (item.TechnicianId.ToString().Equals("22222222-2222-2222-2222-222222222226")) item.ReqSrc = "[SLTT-Approve]";
                if (item.ReceivedBy.ToString().Equals("22222222-2222-2222-2222-222222222226")) item.ReqSrc = "[TTSL-Approve]";

                if (item.TechnicianId.ToString().Equals("22222222-2222-2222-2222-222222222231")) item.ReqSrc = "[SLTT-Approve]";
                if (item.ReceivedBy.ToString().Equals("22222222-2222-2222-2222-222222222231")) item.ReqSrc = "[TTSL-Approve]";


                if (item.TechnicianId.ToString().Equals("22222222-2222-2222-2222-222222222232")) item.ReqSrc = "[SLTT-Approve]";
                if (item.ReceivedBy.ToString().Equals("22222222-2222-2222-2222-222222222232")) item.ReqSrc = "[TTSL-Approve]";

                if (item.TechnicianId.ToString().StartsWith("22222222-2222-2222-2222-") && item.ReceivedBy.ToString().StartsWith("22222222-2222-2222-2222-"))
                {
                    item.ReqSrc = "[WHWH-Approve]";
                }
            }
            result = _Util.Facade.InventoryFacade.InsertTechTransfer(model, CurrentUser.UserId) > 0;
            return Json(new { result = result, message = "Transfer Request added successfully" });
        }


        [HttpPost]
        public JsonResult TechReceiveConfirm(int id, Guid eqpid, Guid techid, int qty, string reqSrc)
        {
            bool result = false;
            string userMessage = "Received successfully";
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            AssignedInventoryTechReceived objassigntech = new AssignedInventoryTechReceived();
            objassigntech.TechnicianId = techid;
            objassigntech.EquipmentId = eqpid;
            objassigntech.Id = id;
            //TODO: Check if still there is still sufficient balance on hand

            if (eqpid != new Guid() && techid != new Guid())
            {
                //var objinvtech = _Util.Facade.InventoryFacade.GetAllInventoryTechByTechnicianIdAndEquipmentIdAndType(objassigntech.ReceivedBy.Value, eqpid.Value);
                objassigntech = _Util.Facade.InventoryFacade.GetAllAssignedInventory(objassigntech);
                //if (objinvtech != null && objinvtech.Id > 0)
                //{
                //    foreach (var item in objinvtech)
                //    {
                //        if (item.Type == "Add")
                //        {
                //            AddQty = AddQty + item.Quantity;
                //        }
                //        if (item.Type == "Release")
                //        {
                //            ReleaseQty = ReleaseQty + item.Quantity;
                //        }
                //    }
                //}
                //TotalQty = AddQty - ReleaseQty;
                //if (qty > TotalQty)
                //{
                //    return Json(new { result = result, message = "Total quantity limit exceed" });
                //}
                //else
                //{
                TransferResult transferResult = new TransferResult();



                //if (objassigntech.TechnicianId == new Guid("22222222-2222-2222-2222-222222222222"))
                //{
                //    transferResult = new EquipmentTransfer().TransferWHToTech((Guid)objassigntech.ReceivedBy, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, objassigntech.TechnicianId, objassigntech.ReqSrc, objassigntech.Quantity); //"[TT-Approve]"
                //}
                //else if (objassigntech.ReceivedBy.Value == new Guid("22222222-2222-2222-2222-222222222222"))
                //{
                //    transferResult = new EquipmentTransfer().TransferTechToWH(objassigntech.TechnicianId, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, (Guid)objassigntech.ReceivedBy, objassigntech.ReqSrc, objassigntech.Quantity); //"[TT-Approve]"
                //}
                //if (objassigntech.TechnicianId.IsVirtualLocation())
                //{
                //    transferResult = new EquipmentTransfer().TransferWHToTech((Guid)objassigntech.ReceivedBy, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, objassigntech.TechnicianId, objassigntech.ReqSrc, objassigntech.Quantity); //"[TT-Approve]"
                //}
                //else if (objassigntech.ReceivedBy.Value.IsVirtualLocation())
                //{
                //    transferResult = new EquipmentTransfer().TransferTechToWH(objassigntech.TechnicianId, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, (Guid)objassigntech.ReceivedBy, objassigntech.ReqSrc, objassigntech.Quantity); //"[TT-Approve]"
                //}

                //else
                //{
                //    transferResult = new EquipmentTransfer().TransferTechToTech(objassigntech.TechnicianId, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, objassigntech.ReceivedBy.Value, objassigntech.ReqSrc, objassigntech.Quantity); //"[TT-Approve]"
                //}
                if (objassigntech.ReceivedBy.Value.IsVirtualLocation() && objassigntech.TechnicianId.IsVirtualLocation())
                {
                    transferResult = new EquipmentTransfer().TransferWHToWH(objassigntech.TechnicianId, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, (Guid)objassigntech.ReceivedBy, objassigntech.ReqSrc, objassigntech.Quantity); //"[TT-Approve]"
                }
                else if (objassigntech.TechnicianId.IsVirtualLocation() && (objassigntech.ReqSrc != "[PURORD-WHTT]"))
                {
                    transferResult = new EquipmentTransfer().TransferWHToTech((Guid)objassigntech.ReceivedBy, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, objassigntech.TechnicianId, objassigntech.ReqSrc, objassigntech.Quantity); //"[TT-Approve]"
                }
                else if (objassigntech.ReceivedBy.Value.IsVirtualLocation())
                {
                    transferResult = new EquipmentTransfer().TransferTechToWH(objassigntech.TechnicianId, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, (Guid)objassigntech.ReceivedBy, objassigntech.ReqSrc, objassigntech.Quantity); //"[TT-Approve]"
                }
                else
                {
                    transferResult = new EquipmentTransfer().TransferTechToTech(objassigntech.TechnicianId, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, objassigntech.ReceivedBy.Value, objassigntech.ReqSrc, objassigntech.Quantity); //"[TT-Approve]"
                }

                objassigntech.IsApprove = transferResult.IsReleased;
                if (transferResult.IsReleased) userMessage = "Transfer Released";
                objassigntech.IsReceived = transferResult.IsAdded;
                if (transferResult.IsAdded) userMessage = "Transfer Added";

                //InventoryTech ReleaseInventoryTech = new InventoryTech()
                //    {
                //        CompanyId = CurrentUser.CompanyId.Value,
                //        TechnicianId = objassigntech.TechnicianId,
                //        EquipmentId = eqpid,
                //        Type = "Release",
                //        Quantity = qty,
                //        LastUpdatedBy = CurrentUser.UserId,
                //        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                //        Description = "Transfer to technician  [TT]"
                //    };
                //    objassigntech.IsApprove = _Util.Facade.InventoryFacade.InsertInventoryTech(ReleaseInventoryTech)>0;
                //    if ((bool)objassigntech.IsApprove)
                //    {
                //        userMessage = "Transfer Approved only";
                //        InventoryTech AddInventoryTech = new InventoryTech()
                //        {
                //            CompanyId = CurrentUser.CompanyId.Value,
                //            TechnicianId = objassigntech.ReceivedBy.Value,
                //            EquipmentId = eqpid,
                //            Type = "Add",
                //            Quantity = qty,
                //            LastUpdatedBy = CurrentUser.UserId,
                //            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                //            Description = "Receive from technician [TT]"
                //        };
                //        objassigntech.IsReceived = _Util.Facade.InventoryFacade.InsertInventoryTech(AddInventoryTech) > 0;
                //    }

                if ((bool)objassigntech.IsApprove && objassigntech.IsReceived)
                {
                    objassigntech.ReceivedDate = DateTime.Now.UTCCurrentTime();
                    objassigntech.ClosedBy = CurrentUser.UserId;
                    result = _Util.Facade.InventoryFacade.UpdateTechReceive_DG(objassigntech);
                    userMessage = "Transfer Approved";
                }
            }

            return Json(new { result = result, message = userMessage });
        }

        //private FileContentResult MakeExcelFromList(DataTable dtResult, string ReportFor, int[] rowIndex, int[] coloumnIndex)
        //{
        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        if (dtResult != null)
        //        {

        //            var worksheet = wb.Worksheets.Add(dtResult);
        //            if (ReportFor == "BrinksReport")
        //            {
        //                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //                wb.Style.Font.Bold = true;
        //                worksheet.AutoFilter.Enabled = false;

        //                worksheet.Column(1).Style.Font.SetFontName("Times New Roman");
        //                worksheet.Column(2).Style.Font.SetFontName("Times New Roman");
        //                worksheet.Column(3).Style.Font.SetFontName("Times New Roman");
        //                worksheet.Column(4).Style.Font.SetFontName("Times New Roman");
        //                worksheet.Column(5).Style.Font.SetFontName("Times New Roman");
        //                worksheet.Column(6).Style.Font.SetFontName("Times New Roman");
        //                worksheet.Column(7).Style.Font.SetFontName("Times New Roman");
        //                worksheet.Column(1).Style.Font.SetFontSize(12);
        //                worksheet.Column(2).Style.Font.SetFontSize(12);
        //                worksheet.Column(3).Style.Font.SetFontSize(12);
        //                worksheet.Column(4).Style.Font.SetFontSize(12);
        //                worksheet.Column(5).Style.Font.SetFontSize(12);
        //                worksheet.Column(6).Style.Font.SetFontSize(12);
        //                worksheet.Column(7).Style.Font.SetFontSize(12);

        //                worksheet.Column(1).Width = 13.71;
        //                worksheet.Column(2).Width = 13.71;
        //                worksheet.Column(3).Width = 13.71;
        //                worksheet.Column(4).Width = 13.71;
        //                worksheet.Column(5).Width = 13.71;
        //                worksheet.Column(6).Width = 13.71;
        //                worksheet.Column(7).Width = 13.71;



        //                worksheet.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //                worksheet.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //                worksheet.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
        //                worksheet.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        //                worksheet.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //                worksheet.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //                worksheet.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        //                worksheet.Ranges("A1:G1").Style.Font.Bold = true;
        //                worksheet.Cells("A1:G1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //                worksheet.Cells("A1:D1").Style.Font.SetFontSize(11);
        //                worksheet.Cells("A1:D1").Style.Font.SetFontName("Calibri");
        //                worksheet.Cells("A1:D1").Style.Font.SetBold();
        //                worksheet.Cells("A1:G1").Style.Font.SetFontColor(XLColor.CoolBlack);
        //                worksheet.Cells("A1:D1").Style.Fill.BackgroundColor = XLColor.Yellow;
        //                worksheet.Cells("E1:G1").Style.Fill.BackgroundColor = XLColor.AshGrey;


        //            }
        //            var format = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("CurrentCurrencyExcelFormat");
        //            if (coloumnIndex != null && format != null && rowIndex != null)
        //            {
        //                foreach (int itemcol in coloumnIndex)
        //                {
        //                    for (int i = 1; i < rowIndex[0]; i++)
        //                    {
        //                        worksheet.Cell(i, itemcol).Style.NumberFormat.Format = format.Value;

        //                    }
        //                }
        //            }
        //            MemoryStream memorystreem = new MemoryStream();
        //            wb.SaveAs(memorystreem);
        //            var fName = string.Format("{0}-{1}.xlsx", ReportFor, DateTime.Now.UTCCurrentTime().ToString("MM/dd/yyyy"));

        //            byte[] fileContents = memorystreem.ToArray();
        //            var userAgent = HttpContext.Request.UserAgent.ToLower();
        //            if (userAgent.Contains("iphone;") || userAgent.Contains("ipad;"))
        //            {
        //                return File(fileContents, ExcelCl.Format("ExcelFormat"), fName);
        //            }
        //            else
        //            {
        //                return File(fileContents, ExcelCl.Format("ExcelFormat"), fName);
        //            }
        //        }
        //        else
        //        {
        //            byte[] fileContents = new byte[1];
        //            return File(fileContents, ExcelCl.Format("ExcelFormat"), "empty.xlsx");
        //        }
        //    }
        //}

        private FileContentResult MakeExcelFromDataSet(DataSet dsResult, string ReportFor, int[] rowIndex1, int[] col_Format_Number)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                if (dsResult != null)
                {
                    for (int i = 0; i < dsResult.Tables.Count; i++)
                    {
                        var worksheet = wb.Worksheets.Add(dsResult.Tables[i]);
                        int transferDateColIndex = 7;
                        int acceptedDateColIndex = 8;
                    
                        //int[] rowarray = { dsResult.Tables[i].Rows.Count + 2 };
                        int[] rowIndex = { dsResult.Tables[i].Rows.Count + 2 };
                        if (ReportFor == "BrinksReport")
                        {
                            wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            wb.Style.Font.Bold = true;
                            worksheet.AutoFilter.Enabled = false;

                            worksheet.Column(1).Style.Font.SetFontName("Times New Roman");
                            worksheet.Column(2).Style.Font.SetFontName("Times New Roman");
                            worksheet.Column(3).Style.Font.SetFontName("Times New Roman");
                            worksheet.Column(4).Style.Font.SetFontName("Times New Roman");
                            worksheet.Column(5).Style.Font.SetFontName("Times New Roman");
                            worksheet.Column(6).Style.Font.SetFontName("Times New Roman");
                            worksheet.Column(7).Style.Font.SetFontName("Times New Roman");
                            worksheet.Column(1).Style.Font.SetFontSize(12);
                            worksheet.Column(2).Style.Font.SetFontSize(12);
                            worksheet.Column(3).Style.Font.SetFontSize(12);
                            worksheet.Column(4).Style.Font.SetFontSize(12);
                            worksheet.Column(5).Style.Font.SetFontSize(12);
                            worksheet.Column(6).Style.Font.SetFontSize(12);
                            worksheet.Column(7).Style.Font.SetFontSize(12);

                            worksheet.Column(1).Width = 13.71;
                            worksheet.Column(2).Width = 13.71;
                            worksheet.Column(3).Width = 13.71;
                            worksheet.Column(4).Width = 13.71;
                            worksheet.Column(5).Width = 13.71;
                            worksheet.Column(6).Width = 13.71;
                            worksheet.Column(7).Width = 13.71;



                            worksheet.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            worksheet.Ranges("A1:G1").Style.Font.Bold = true;
                            worksheet.Cells("A1:G1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            worksheet.Cells("A1:D1").Style.Font.SetFontSize(11);
                            worksheet.Cells("A1:D1").Style.Font.SetFontName("Calibri");
                            worksheet.Cells("A1:D1").Style.Font.SetBold();
                            worksheet.Cells("A1:G1").Style.Font.SetFontColor(XLColor.CoolBlack);
                            worksheet.Cells("A1:D1").Style.Fill.BackgroundColor = XLColor.Yellow;
                            worksheet.Cells("E1:G1").Style.Fill.BackgroundColor = XLColor.AshGrey;


                        }

                        var format = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("CurrentCurrencyExcelFormat");
                        if (col_Format_Number != null && format != null && rowIndex != null)
                        {
                            foreach (int itemcol in col_Format_Number)
                            {
                                for (int j = 1; j < rowIndex[0]; j++)
                                {
                                    worksheet.Cell(j, itemcol).Style.NumberFormat.Format = format.Value;

                                }
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
                        return File(fileContents, ExcelCl.Format("ExcelFormat"), fName);
                    }
                    else
                    {
                        return File(fileContents, ExcelCl.Format("ExcelFormat"), fName);
                    }
                }
                else
                {
                    byte[] fileContents = new byte[1];
                    return File(fileContents, ExcelCl.Format("ExcelFormat"), "empty.xlsx");
                }
            }
        }

        [Authorize]
        public ActionResult TechTransferLogPartial(PurchaseOrderFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<SelectListItem> EstimatorIdList = new List<SelectListItem>();

            //#region Permission Check
            //if (!base.IsPermitted(404))
            //{
            //    return PartialView("~/Views/Shared/_UnderDevelopment.cshtml");
            //}
            //#endregion

            EstimatorIdList.Add(new SelectListItem()
            {
                Text = "Select Estimate Id",
                Value = "-1"
            });
            EstimatorIdList.AddRange(_Util.Facade.PurchaseOrderFacade.GetEstimatorIdListOfPurchaseOrder().Select(x =>
                                       new SelectListItem()
                                       {
                                           Text = x.Id,
                                           Value = x.Id
                                       }).ToList());
            ViewBag.EstimatorIdList = EstimatorIdList;

            List<string> selectsts = new List<string>();
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectsts.Add(item);
                    }
                }
            }
            string PerGrpAssgnTicketId = "";
            var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);

            TechnicianList = TechnicianList.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            //List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);
            var emplst = new List<SelectListItem>();
            if (TechnicianList != null)
            {
                emplst = TechnicianList.Where(y => y.UserId != CurrentUser.UserId).Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                    Selected = true,
                }).ToList();
                if (emplst.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault() != null)
                {
                    emplst.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";
                }
            }
            ViewBag.selectsts = selectsts;
            ViewBag.EstimatorId = filters.EstimatorId;
            ViewBag.EqList = emplst;
            return PartialView();
        }
         
        [Authorize]
        [HttpPost]
        public ActionResult TechTransferLogListPartialPost(TechTransferFilter filters)
        {

            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            if (filters.Searchtext == "undefined")
            {
                filters.Searchtext = null;
            }
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<string> selectedTechsTrfFromList = new List<string>();
            List<string> selectedTechsTrfToList = new List<string>();
            List<string> selectedTechsRcvFromList = new List<string>();
            List<string> selectedTechsRcvToList = new List<string>();

            ViewBag.LoggedUserName = CurrentUser.FirstName + " " + CurrentUser.LastName;
            ViewBag.LoggedUserId = CurrentUser.UserId;

            #region Inits
            if (filters == null)
            {
                filters = new TechTransferFilter();
            }

            if (filters.PageNoTrf == 0)
            {
                filters.PageNoTrf = 1;
            }

            if (filters.PageNoRcv == 0)
            {
                filters.PageNoRcv = 1;
            }

            filters.EmployeeId = CurrentUser.UserId;

            //if (filters.TFEmployeeIds != null)
            //{
            //    filters.TFEmployeeIds = filters.TFEmployeeIds[0].Split(',');
            //}

            //if (filters.TTEmployeeIds != null)
            //{
            //    filters.TTEmployeeIds = filters.TTEmployeeIds[0].Split(',');
            //}

            //if (filters.RFEmployeeIds != null)
            //{
            //    filters.RFEmployeeIds = filters.RFEmployeeIds[0].Split(',');
            //}

            //if (filters.RTEmployeeIds != null)
            //{
            //    filters.RTEmployeeIds = filters.RTEmployeeIds[0].Split(',');
            //}

            filters.PageSize = _Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);
            TechReceiveListModel Model = new TechReceiveListModel();
            filters.CompanyId = CurrentUser.CompanyId.Value;
            #endregion

            string PerGrpAssgnTicketId = "";
            GlobalSetting PerGrpAssgnTicket = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "TicketAssignPermissionGroup");
            if (PerGrpAssgnTicket != null && !string.IsNullOrWhiteSpace(PerGrpAssgnTicket.Value))
            {
                PerGrpAssgnTicketId = PerGrpAssgnTicket.Value;
            }

            //var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);
            
            
            if (filters.GetReport)
            {
                if (!string.IsNullOrWhiteSpace(filters.Start) && filters.Start != "undefined" && filters.Start != "01/01/0001" && !string.IsNullOrWhiteSpace(filters.End) && filters.End != "undefined" && filters.End != "01/01/0001")
                {
                    StartDate = Convert.ToDateTime(filters.Start).SetZeroHour();
                    EndDate = Convert.ToDateTime(filters.End).SetMaxHour();
                    DataSet ds = _Util.Facade.InventoryFacade.GetTechTransferLogListByFiltersRDate(filters, StartDate, EndDate);
                    //dt.Columns.Remove("Total RMR");
                    int[] colarray = null; // { 2, 4, 5, 6, 7 };
                    int[] rowarray = null; // { dt.Rows.Count + 2 };
                    ds.Tables.Remove("Default1");
                    ds.Tables.Remove("Default2");
                    ds.Tables.Remove("Default3");
                    ds.Tables.Remove("Default4");
                    ds.Tables.Remove("Default5");
                    ds.Tables.Remove("Default6");
                    ds.Tables.Remove("Default7");
                    ds.Tables[0].Columns.RemoveAt(10);
                    ds.Tables[0].Columns.RemoveAt(10);
                    return MakeExcelFromDataSet(ds, "Tech Receive", rowarray, colarray);

                }
                else
                {
                    DataSet ds = _Util.Facade.InventoryFacade.GetTechTransferLogListByFiltersRDate(filters, null, null);
                    //dt.Columns.Remove("Total RMR");
                    int[] colarray = null; // { 2, 4, 5, 6, 7 };
                    int[] rowarray = null; // { dt.Rows.Count + 2 };
                    ds.Tables.Remove("Default1");
                    ds.Tables.Remove("Default2");
                    ds.Tables.Remove("Default3");
                    ds.Tables.Remove("Default4");
                    ds.Tables.Remove("Default5");
                    ds.Tables.Remove("Default6");
                    ds.Tables.Remove("Default7");
                    ds.Tables[0].Columns.RemoveAt(10);
                    ds.Tables[0].Columns.RemoveAt(10);
                    return MakeExcelFromDataSet(ds, "Tech Receive", rowarray, colarray);

                }

            }

            Model = _Util.Facade.InventoryFacade.GetTechTransferLogListByFilters(filters);

            var TechTrfFromList = new List<SelectListItem>();
            var TechTrfToList = new List<SelectListItem>();
            var TechRcvFromList = new List<SelectListItem>();
            var TechRcvToList = new List<SelectListItem>();

            //foreach (var item in Model.ListAssignedInventoryTechApprove.Where(x => x.ReceivedBy == Guid.Parse("22222222-2222-2222-2222-222222222222")))
            //{
            //    item.ReceivedByName = "Warehouse";
            //}

            //foreach (var item in Model.ListAssignedInventoryTechApprove.Where(x => x.TechnicianId == Guid.Parse("22222222-2222-2222-2222-222222222222")))
            //{
            //    item.TransferByName = "Warehouse";
            //}

            //foreach (var item in Model.ListAssignedInventoryTechReceived.Where(x => x.ReceivedBy == Guid.Parse("22222222-2222-2222-2222-222222222222")))
            //{
            //    item.ReceivedByName = "Warehouse";
            //}

            //foreach (var item in Model.ListAssignedInventoryTechReceived.Where(x => x.TechnicianId == Guid.Parse("22222222-2222-2222-2222-222222222222")))
            //{
            //    item.TransferByName = "Warehouse";
            //}




            //if (Model.ListAssignedInventoryTechApprove.Where(x => x.ReceivedBy == Guid.Parse("22222222-2222-2222-2222-222222222222")).FirstOrDefault()==null)
            //    Model.ListAssignedInventoryTechApprove.Where(x => x.ReceivedBy == Guid.Parse("22222222-2222-2222-2222-222222222222")).FirstOrDefault().ReceivedByName = "Warehouse";
            //if(Model.ListAssignedInventoryTechReceived.Where(x => x.ReceivedBy == Guid.Parse("22222222-2222-2222-2222-222222222222")).FirstOrDefault()==null)
            //    Model.ListAssignedInventoryTechReceived.Where(x => x.ReceivedBy == Guid.Parse("22222222-2222-2222-2222-222222222222")).FirstOrDefault().ReceivedByName = "Warehouse";

            if (Model.TechTrfFromList != null)
            {
                //if (base.IsPermitted(UserPermissions.InventoryPermissions.TechDropDownEditable))
                //{
                //    TechTrfList.Add(new SelectListItem()
                //    {
                //        Text = "Warehouse",
                //        Value = "22222222-2222-2222-2222-222222222222",
                //        Selected = true,
                //    });

                //    TechRcvList.Add(new SelectListItem()
                //    {
                //        Text = "Warehouse",
                //        Value = "22222222-2222-2222-2222-222222222222",
                //        Selected = true,
                //    });
                //}
                Model.TechTrfFromList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";
                Model.TechTrfToList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";
                Model.TechRcvFromList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";
                Model.TechRcvToList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";

                foreach (var tech in Model.TechTrfFromList)
                {
                    tech.Selected = true;
                    //if (filters.TFEmployeeIds[0] != "undefined")
                    if (filters.TFEmployeeIds != null)
                    {
                        if (!filters.TFEmployeeIds.Contains(tech.Value))
                        {
                            tech.Selected = false;
                        }
                    }

                    TechTrfFromList.Add(new SelectListItem()
                    {
                        Text = tech.Text,
                        Value = tech.Value,
                        Selected = tech.Selected,
                    });
                    if (tech.Selected) selectedTechsTrfFromList.Add(tech.Value);
                }
                //TechTrfFromList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";

                foreach (var tech in Model.TechTrfToList)
                {
                    tech.Selected = true;
                    //if (filters.TTEmployeeIds[0] != "undefined")
                    if (filters.TTEmployeeIds != null)
                    {
                        if (!filters.TTEmployeeIds.Contains(tech.Value))
                        {
                            tech.Selected = false;
                        }
                    }

                    TechTrfToList.Add(new SelectListItem()
                    {
                        Text = tech.Text,
                        Value = tech.Value,
                        Selected = true,
                    });
                    if (tech.Selected) selectedTechsTrfToList.Add(tech.Value);
                }
                //TechTrfToList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";

                foreach (var tech in Model.TechRcvFromList)
                {
                    tech.Selected = true;
                    //if (filters.RFEmployeeIds[0] != "undefined")
                    if (filters.RFEmployeeIds != null)
                    {
                        if (!filters.RFEmployeeIds.Contains(tech.Value))
                        {
                            tech.Selected = false;
                        }
                    }

                    TechRcvFromList.Add(new SelectListItem()
                    {
                        Text = tech.Text,
                        Value = tech.Value,
                        Selected = true,
                    });
                    if (tech.Selected) selectedTechsRcvFromList.Add(tech.Value);
                }
                //TechRcvFromList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";

                foreach (var tech in Model.TechRcvToList)
                {
                    tech.Selected = true;
                    //if (filters.RTEmployeeIds[0] != "undefined")
                    if (filters.RTEmployeeIds != null)
                    {
                        if (!filters.RTEmployeeIds.Contains(tech.Value))
                        {
                            tech.Selected = false;
                        }
                    }

                    TechRcvToList.Add(new SelectListItem()
                    {
                        Text = tech.Text,
                        Value = tech.Value,
                        Selected = true,
                    });
                    if (tech.Selected) selectedTechsRcvToList.Add(tech.Value);
                }
                //TechRcvToList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";

                //foreach (var tech in Model.ListAssignedInventoryTechReceived.OrderBy(x => x.ReceivedByName))
                //{
                //    if (TechTrfList.Where(x => x.Value == tech.TechnicianId.ToString()).Count() == 0)
                //    {
                //        TechRcvList.Add(new SelectListItem()
                //        {
                //            Text = tech.ReceivedByName,
                //            Value = tech.TechnicianId.ToString(),
                //            Selected = tech.TechnicianId == CurrentUser.UserId ? true : false,
                //        });
                //        if (tech.ReceivedBy == CurrentUser.UserId)
                //        {
                //            selectedTechsRcvList.Add(tech.TechnicianId.ToString());
                //        }
                //    }
                //}
            }

            ViewBag.PageNumber = filters.PageNoTrf;
            ViewBag.OutOfNumber = 0;
            ViewBag.PageNumberApprove = filters.PageNoTrf;
            ViewBag.OutOfNumberApprove = 0;

            if (Model.TotalCountTrf > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCountRcv;
            }
            if (Model.TotalCountRcv > 0)
            {
                ViewBag.OutOfNumberApprove = Model.TotalCountTrf;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);

            if ((int)ViewBag.PageNumberApprove * filters.PageSize > (int)ViewBag.OutOfNumberApprove)
            {
                ViewBag.CurrentNumberApprove = (int)ViewBag.OutOfNumberApprove;
            }
            else
            {
                ViewBag.CurrentNumberApprove = (int)ViewBag.PageNumberApprove * filters.PageSize;
            }
            ViewBag.PageCountApprove = Math.Ceiling((double)ViewBag.OutOfNumberApprove / filters.PageSize);
            ViewBag.order = filters.order ?? "null";
            ViewBag.EmployeeId = filters.EmployeeId;
            ViewBag.SearchText = filters.Searchtext;

            ViewBag.TechTrfFromList = TechTrfFromList; // TechList;
            ViewBag.TechTrfToList = TechTrfToList; // TechList;
            ViewBag.TechRcvFromList = TechRcvFromList; // TechList;
            ViewBag.TechRcvToList = TechRcvToList; // TechList;
            ViewBag.selectedTechsTrfFromList = selectedTechsTrfFromList;
            ViewBag.selectedTechsTrfToList = selectedTechsTrfToList;
            ViewBag.selectedTechsRcvFromList = selectedTechsRcvFromList;
            ViewBag.selectedTechsRcvToList = selectedTechsRcvToList;
            ViewBag.AllowApprove = false;
            if (base.IsPermitted(UserPermissions.InventoryPermissions.TechDropDownEditable) || base.IsPermitted(UserPermissions.InventoryPermissions.TechTransferApprove) || (filters.RTEmployeeIds[0] == "undefined"))
            {
                ViewBag.AllowApprove = true;
            }

            return View("TechTransferLogListPartial", Model);
        }
        [Authorize]
        public ActionResult InventoryTransfferLogTab(PurchaseOrderFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<SelectListItem> EstimatorIdList = new List<SelectListItem>();


            EstimatorIdList.Add(new SelectListItem()
            {
                Text = "Select Estimate Id",
                Value = "-1"
            });
            EstimatorIdList.AddRange(_Util.Facade.PurchaseOrderFacade.GetEstimatorIdListOfPurchaseOrder().Select(x =>
                                       new SelectListItem()
                                       {
                                           Text = x.Id,
                                           Value = x.Id
                                       }).ToList());
            ViewBag.EstimatorIdList = EstimatorIdList;

            List<string> selectsts = new List<string>();
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectsts.Add(item);
                    }
                }
            }
            string PerGrpAssgnTicketId = "";
            var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);

            TechnicianList = TechnicianList.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            //List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);
            var emplst = new List<SelectListItem>();
            if (TechnicianList != null)
            {
                emplst = TechnicianList.Where(y => y.UserId != CurrentUser.UserId).Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                    Selected = true,
                }).ToList();
                if (emplst.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault() != null)
                {
                    emplst.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";
                }
            }
            ViewBag.selectsts = selectsts;
            ViewBag.EstimatorId = filters.EstimatorId;
            ViewBag.EqList = emplst;
            return PartialView(); 
        }
        public ActionResult InventoryTransferListPartial(TechTransferFilter filters)
        {
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            if (filters.Searchtext == "undefined")
            {
                filters.Searchtext = null;
            }
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<string> selectedTechsTrfFromList = new List<string>();
            List<string> selectedTechsTrfToList = new List<string>();
            List<string> selectedTechsRcvFromList = new List<string>();
            List<string> selectedTechsRcvToList = new List<string>();

            ViewBag.LoggedUserName = CurrentUser.FirstName + " " + CurrentUser.LastName;
            ViewBag.LoggedUserId = CurrentUser.UserId;

            #region Inits
            if (filters == null)
            {
                filters = new TechTransferFilter();
            }

            if (filters.PageNoTrf == 0)
            {
                filters.PageNoTrf = 1;
            }

            if (filters.PageNoRcv == 0)
            {
                filters.PageNoRcv = 1;
            } 
            filters.EmployeeId = CurrentUser.UserId; 
            filters.PageSize = _Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);
            TechReceiveListModel Model = new TechReceiveListModel();
            filters.CompanyId = CurrentUser.CompanyId.Value;
            #endregion

            string PerGrpAssgnTicketId = "";
            GlobalSetting PerGrpAssgnTicket = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "TicketAssignPermissionGroup");
            if (PerGrpAssgnTicket != null && !string.IsNullOrWhiteSpace(PerGrpAssgnTicket.Value))
            {
                PerGrpAssgnTicketId = PerGrpAssgnTicket.Value;
            }

            //var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);

            if (filters.GetReport)
            {
                DataSet ds = _Util.Facade.InventoryFacade.GetTechTransferLogListByFiltersR(filters);
                //dt.Columns.Remove("Total RMR");
                int[] colarray = null; // { 2, 4, 5, 6, 7 };
                int[] rowarray = null; // { dt.Rows.Count + 2 };
                ds.Tables.Remove("Default1");
                ds.Tables.Remove("Default2");
                ds.Tables.Remove("Default3");
                ds.Tables.Remove("Default4");
                ds.Tables.Remove("Default5");
                ds.Tables.Remove("Default6");
                ds.Tables.Remove("Default7");
                ds.Tables[0].Columns.RemoveAt(9);
                ds.Tables[0].Columns.RemoveAt(9);
                return MakeExcelFromDataSet(ds, "TechTransferLog", rowarray, colarray);
            }

            if (!string.IsNullOrWhiteSpace(filters.Start) && filters.Start != "undefined" && filters.Start != "01/01/0001" && !string.IsNullOrWhiteSpace(filters.End) && filters.End != "undefined" && filters.End != "01/01/0001")

            {
                StartDate = Convert.ToDateTime(filters.Start).SetZeroHour();
                EndDate = Convert.ToDateTime(filters.End).SetMaxHour();

                Model = _Util.Facade.InventoryFacade.GetTechTransferLogListByFiltersDate(filters, StartDate, EndDate);
            }
            else
            {
                Model = _Util.Facade.InventoryFacade.GetTechTransferLogListByFiltersDate(filters, null, null);
            }

            var TechTrfFromList = new List<SelectListItem>();
            var TechTrfToList = new List<SelectListItem>();
            var TechRcvFromList = new List<SelectListItem>();
            var TechRcvToList = new List<SelectListItem>();

            
            if (Model.TechTrfFromList != null)
            { 
                Model.TechTrfFromList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";
                Model.TechTrfToList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";
                Model.TechRcvFromList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";
                Model.TechRcvToList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";

                foreach (var tech in Model.TechTrfFromList)
                {
                    tech.Selected = true;
                    //if (filters.TFEmployeeIds[0] != "undefined")
                    if (filters.TFEmployeeIds != null)
                    {
                        if (!filters.TFEmployeeIds.Contains(tech.Value))
                        {
                            tech.Selected = false;
                        }
                    }

                    TechTrfFromList.Add(new SelectListItem()
                    {
                        Text = tech.Text,
                        Value = tech.Value,
                        Selected = tech.Selected,
                    });
                    if (tech.Selected) selectedTechsTrfFromList.Add(tech.Value);
                }
                //TechTrfFromList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";

                foreach (var tech in Model.TechTrfToList)
                {
                    tech.Selected = true;
                    //if (filters.TTEmployeeIds[0] != "undefined")
                    if (filters.TTEmployeeIds != null)
                    {
                        if (!filters.TTEmployeeIds.Contains(tech.Value))
                        {
                            tech.Selected = false;
                        }
                    }

                    TechTrfToList.Add(new SelectListItem()
                    {
                        Text = tech.Text,
                        Value = tech.Value,
                        Selected = true,
                    });
                    if (tech.Selected) selectedTechsTrfToList.Add(tech.Value);
                }
                //TechTrfToList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";

                foreach (var tech in Model.TechRcvFromList)
                {
                    tech.Selected = true;
                    //if (filters.RFEmployeeIds[0] != "undefined")
                    if (filters.RFEmployeeIds != null)
                    {
                        if (!filters.RFEmployeeIds.Contains(tech.Value))
                        {
                            tech.Selected = false;
                        }
                    }

                    TechRcvFromList.Add(new SelectListItem()
                    {
                        Text = tech.Text,
                        Value = tech.Value,
                        Selected = true,
                    });
                    if (tech.Selected) selectedTechsRcvFromList.Add(tech.Value);
                }
                //TechRcvFromList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";

                foreach (var tech in Model.TechRcvToList)
                {
                    tech.Selected = true;
                    //if (filters.RTEmployeeIds[0] != "undefined")
                    if (filters.RTEmployeeIds != null)
                    {
                        if (!filters.RTEmployeeIds.Contains(tech.Value))
                        {
                            tech.Selected = false;
                        }
                    }

                    TechRcvToList.Add(new SelectListItem()
                    {
                        Text = tech.Text,
                        Value = tech.Value,
                        Selected = true,
                    });
                    if (tech.Selected) selectedTechsRcvToList.Add(tech.Value);
                }
                //TechRcvToList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";

                //foreach (var tech in Model.ListAssignedInventoryTechReceived.OrderBy(x => x.ReceivedByName))
                //{
                //    if (TechTrfList.Where(x => x.Value == tech.TechnicianId.ToString()).Count() == 0)
                //    {
                //        TechRcvList.Add(new SelectListItem()
                //        {
                //            Text = tech.ReceivedByName,
                //            Value = tech.TechnicianId.ToString(),
                //            Selected = tech.TechnicianId == CurrentUser.UserId ? true : false,
                //        });
                //        if (tech.ReceivedBy == CurrentUser.UserId)
                //        {
                //            selectedTechsRcvList.Add(tech.TechnicianId.ToString());
                //        }
                //    }
                //}
            }

            ViewBag.PageNumber = filters.PageNoTrf;
            ViewBag.OutOfNumber = 0;
            ViewBag.PageNumberApprove = filters.PageNoTrf;
            ViewBag.OutOfNumberApprove = 0;

            if (Model.TotalCountTrf > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCountRcv;
            }
            if (Model.TotalCountRcv > 0)
            {
                ViewBag.OutOfNumberApprove = Model.TotalCountTrf;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);

            if ((int)ViewBag.PageNumberApprove * filters.PageSize > (int)ViewBag.OutOfNumberApprove)
            {
                ViewBag.CurrentNumberApprove = (int)ViewBag.OutOfNumberApprove;
            }
            else
            {
                ViewBag.CurrentNumberApprove = (int)ViewBag.PageNumberApprove * filters.PageSize;
            }
            ViewBag.PageCountApprove = Math.Ceiling((double)ViewBag.OutOfNumberApprove / filters.PageSize);
            ViewBag.order = filters.order ?? "null";
            ViewBag.EmployeeId = filters.EmployeeId;
            ViewBag.SearchText = filters.Searchtext;

            ViewBag.TechTrfFromList = TechTrfFromList; // TechList;
            ViewBag.TechTrfToList = TechTrfToList; // TechList;
            ViewBag.TechRcvFromList = TechRcvFromList; // TechList;
            ViewBag.TechRcvToList = TechRcvToList; // TechList;
            ViewBag.selectedTechsTrfFromList = selectedTechsTrfFromList;
            ViewBag.selectedTechsTrfToList = selectedTechsTrfToList;
            ViewBag.selectedTechsRcvFromList = selectedTechsRcvFromList;
            ViewBag.selectedTechsRcvToList = selectedTechsRcvToList;
            ViewBag.AllowApprove = false;
            if (base.IsPermitted(UserPermissions.InventoryPermissions.TechDropDownEditable) || base.IsPermitted(UserPermissions.InventoryPermissions.TechTransferApprove) || (filters.RTEmployeeIds[0] == "undefined"))
            {
                ViewBag.AllowApprove = true;
            }

            return View(Model);
        }
        public ActionResult InventoryReceiveListPartial(TechTransferFilter filters)
        {
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            if (filters.Searchtext == "undefined")
            {
                filters.Searchtext = null;
            }
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<string> selectedTechsTrfFromList = new List<string>();
            List<string> selectedTechsTrfToList = new List<string>();
            List<string> selectedTechsRcvFromList = new List<string>();
            List<string> selectedTechsRcvToList = new List<string>();

            ViewBag.LoggedUserName = CurrentUser.FirstName + " " + CurrentUser.LastName;
            ViewBag.LoggedUserId = CurrentUser.UserId;

            #region Inits
            if (filters == null)
            {
                filters = new TechTransferFilter();
            }

            if (filters.PageNoTrf == 0)
            {
                filters.PageNoTrf = 1;
            }

            if (filters.PageNoRcv == 0)
            {
                filters.PageNoRcv = 1;
            }

            filters.EmployeeId = CurrentUser.UserId;

             

            filters.PageSize = _Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);
            TechReceiveListModel Model = new TechReceiveListModel();
            filters.CompanyId = CurrentUser.CompanyId.Value;
            #endregion

            string PerGrpAssgnTicketId = "";
            GlobalSetting PerGrpAssgnTicket = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "TicketAssignPermissionGroup");
            if (PerGrpAssgnTicket != null && !string.IsNullOrWhiteSpace(PerGrpAssgnTicket.Value))
            {
                PerGrpAssgnTicketId = PerGrpAssgnTicket.Value;
            }

            //var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);

            if (filters.GetReport)
            {
                DataSet ds = _Util.Facade.InventoryFacade.GetTechTransferLogListByFiltersR(filters);
                //dt.Columns.Remove("Total RMR");
                int[] colarray = null; // { 2, 4, 5, 6, 7 };
                int[] rowarray = null; // { dt.Rows.Count + 2 };
                ds.Tables.Remove("Default1");
                ds.Tables.Remove("Default2");
                ds.Tables.Remove("Default3");
                ds.Tables.Remove("Default4");
                ds.Tables.Remove("Default5");
                ds.Tables.Remove("Default6");
                ds.Tables.Remove("Default7");
                ds.Tables[0].Columns.RemoveAt(9);
                ds.Tables[0].Columns.RemoveAt(9);
                return MakeExcelFromDataSet(ds, "TechTransferLog", rowarray, colarray);
            }
            if (!string.IsNullOrWhiteSpace(filters.Start) &&  filters.Start != "undefined" && filters.Start != "01/01/0001" && !string.IsNullOrWhiteSpace(filters.End) &&  filters.End != "undefined" && filters.End != "01/01/0001")
            {
                StartDate = Convert.ToDateTime(filters.Start).SetZeroHour();
                EndDate = Convert.ToDateTime(filters.End).SetMaxHour();

                Model = _Util.Facade.InventoryFacade.GetTechTransferLogListByFiltersDate(filters, StartDate, EndDate);
            }
            else
            {
                Model = _Util.Facade.InventoryFacade.GetTechTransferLogListByFiltersDate(filters, null, null);
            }
          //  Model = _Util.Facade.InventoryFacade.GetTechTransferLogListByFilters(filters);
            var TechTrfFromList = new List<SelectListItem>();
            var TechTrfToList = new List<SelectListItem>();
            var TechRcvFromList = new List<SelectListItem>();
            var TechRcvToList = new List<SelectListItem>();


            if (Model.TechTrfFromList != null)
            {
                Model.TechTrfFromList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";
                Model.TechTrfToList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";
                Model.TechRcvFromList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";
                Model.TechRcvToList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";

                foreach (var tech in Model.TechTrfFromList)
                {
                    tech.Selected = true;
                    //if (filters.TFEmployeeIds[0] != "undefined")
                    if (filters.TFEmployeeIds != null)
                    {
                        if (!filters.TFEmployeeIds.Contains(tech.Value))
                        {
                            tech.Selected = false;
                        }
                    }

                    TechTrfFromList.Add(new SelectListItem()
                    {
                        Text = tech.Text,
                        Value = tech.Value,
                        Selected = tech.Selected,
                    });
                    if (tech.Selected) selectedTechsTrfFromList.Add(tech.Value);
                }
                //TechTrfFromList.Where(x => x.Value == "22222222-2222-2222-2222-222222222222").FirstOrDefault().Text = "Warehouse";

                foreach (var tech in Model.TechTrfToList)
                {
                    tech.Selected = true;
                    //if (filters.TTEmployeeIds[0] != "undefined")
                    if (filters.TTEmployeeIds != null)
                    {
                        if (!filters.TTEmployeeIds.Contains(tech.Value))
                        {
                            tech.Selected = false;
                        }
                    }

                    TechTrfToList.Add(new SelectListItem()
                    {
                        Text = tech.Text,
                        Value = tech.Value,
                        Selected = true,
                    });
                    if (tech.Selected) selectedTechsTrfToList.Add(tech.Value);
                }
               

                foreach (var tech in Model.TechRcvFromList)
                {
                    tech.Selected = true; 
                    if (filters.RFEmployeeIds != null)
                    {
                        if (!filters.RFEmployeeIds.Contains(tech.Value))
                        {
                            tech.Selected = false;
                        }
                    }

                    TechRcvFromList.Add(new SelectListItem()
                    {
                        Text = tech.Text,
                        Value = tech.Value,
                        Selected = true,
                    });
                    if (tech.Selected) selectedTechsRcvFromList.Add(tech.Value);
                } 

                foreach (var tech in Model.TechRcvToList)
                {
                    tech.Selected = true;
                    //if (filters.RTEmployeeIds[0] != "undefined")
                    if (filters.RTEmployeeIds != null)
                    {
                        if (!filters.RTEmployeeIds.Contains(tech.Value))
                        {
                            tech.Selected = false;
                        }
                    }

                    TechRcvToList.Add(new SelectListItem()
                    {
                        Text = tech.Text,
                        Value = tech.Value,
                        Selected = true,
                    });
                    if (tech.Selected) selectedTechsRcvToList.Add(tech.Value);
                } 
            }

            ViewBag.PageNumber = filters.PageNoTrf;
            ViewBag.OutOfNumber = 0;
            ViewBag.PageNumberApprove = filters.PageNoTrf;
            ViewBag.OutOfNumberApprove = 0;

            if (Model.TotalCountTrf > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCountRcv;
            }
            if (Model.TotalCountRcv > 0)
            {
                ViewBag.OutOfNumberApprove = Model.TotalCountTrf;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);

            if ((int)ViewBag.PageNumberApprove * filters.PageSize > (int)ViewBag.OutOfNumberApprove)
            {
                ViewBag.CurrentNumberApprove = (int)ViewBag.OutOfNumberApprove;
            }
            else
            {
                ViewBag.CurrentNumberApprove = (int)ViewBag.PageNumberApprove * filters.PageSize;
            }
            ViewBag.PageCountApprove = Math.Ceiling((double)ViewBag.OutOfNumberApprove / filters.PageSize);
            ViewBag.order = filters.order ?? "null";
            ViewBag.EmployeeId = filters.EmployeeId;
            ViewBag.SearchText = filters.Searchtext;

            ViewBag.TechTrfFromList = TechTrfFromList; // TechList;
            ViewBag.TechTrfToList = TechTrfToList; // TechList;
            ViewBag.TechRcvFromList = TechRcvFromList; // TechList;
            ViewBag.TechRcvToList = TechRcvToList; // TechList;
            ViewBag.selectedTechsTrfFromList = selectedTechsTrfFromList;
            ViewBag.selectedTechsTrfToList = selectedTechsTrfToList;
            ViewBag.selectedTechsRcvFromList = selectedTechsRcvFromList;
            ViewBag.selectedTechsRcvToList = selectedTechsRcvToList;
            ViewBag.AllowApprove = false;
            if (base.IsPermitted(UserPermissions.InventoryPermissions.TechDropDownEditable) || base.IsPermitted(UserPermissions.InventoryPermissions.TechTransferApprove) || (filters.RTEmployeeIds[0] == "undefined"))
            {
                ViewBag.AllowApprove = true;
            }

            return View(Model);
        }
        public ActionResult InventoryReportPartial()
        {
            //if (!base.IsPermitted(UserPermissions.ReportsPermissions.LeadsReport))
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}
            List<string> eqcategory = new List<string>();
            List<string> manulist = new List<string>();

            List<string> listprimaryVendor = new List<string>();
            List<string> listproductType = new List<string>();



            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            string newCookie = "";







            List<SelectListItem> eqpCategory = new List<SelectListItem>();
            //eqpCategory.Add(new SelectListItem()
            //{
            //    Text = "Select One",
            //    Value = "-1"
            //});
            eqpCategory.AddRange(_Util.Facade.EquipmentFacade.GetAllEquipmentType().OrderBy(x => x.Id.ToString() != "-1").ThenBy(x => x.Name.ToString()).Select(x => new SelectListItem()
            {
                Text = x.Name.ToString(),
                Value = x.Id.ToString()
            }).ToList());
            ViewBag.eqpCategory = eqpCategory;
            List<SelectListItem> manufacturer = new List<SelectListItem>();
            //manufacturer.Add(new SelectListItem()
            //{
            //    Text = "Select One",
            //    Value = "-1"
            //});
            manufacturer.AddRange(_Util.Facade.EquipmentFacade.GetAllManufacturer().OrderBy(x => x.ManufacturerId.ToString() != "-1").ThenBy(x => x.Name.ToString()).Select(x => new SelectListItem()
            {
                Text = x.Name.ToString(),
                Value = x.ManufacturerId.ToString()
            }).ToList());

            List<SelectListItem> primaryVendor = new List<SelectListItem>();
            primaryVendor.AddRange(_Util.Facade.SupplierFacade.GetAllSupplier().OrderBy(x => x.CompanyName.ToString() != "-1").ThenBy(x => x.CompanyName.ToString()).Select(x => new SelectListItem()
            {
                Text = x.CompanyName.ToString(),
                Value = x.CompanyName.ToString()
            }).ToList());
            List<SelectListItem> productType = new List<SelectListItem>();
            productType.Add(new SelectListItem()
            {
                Text = "Equipment",
                Value = "1"
            });
            productType.Add(new SelectListItem()
            {
                Text = "Service",
                Value = "2"
            });
            //productType.AddRange(_Util.Facade.EquipmentFacade.GetAllManufacturer().Select(x => new SelectListItem()
            //{
            //    Text = x.Name.ToString(),
            //    Value = x.ManufacturerId.ToString()
            //}).ToList());

            ViewBag.productType = productType;
            ViewBag.primaryVendor = primaryVendor;
            ViewBag.manufacturer = manufacturer;
            ViewBag.listcategory = eqcategory;
            ViewBag.listmanu = manulist;


            ViewBag.listprimaryVendor = listprimaryVendor;
            ViewBag.listproductType = listproductType;


            return View();
        }

        [Authorize]
        public ActionResult InventoryReportPartialList(string Start, string End, bool? GetReport, string category, string manufact, int pageno, int pagesize, string SearchText, string ProductTypeID, string primaryVendorID, string order)
        {
            //if (!base.IsPermitted(UserPermissions.ReportsPermissions.LeadsReport))
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}
            List<string> eqcategory = new List<string>();
            List<string> manulist = new List<string>();

            List<string> listprimaryVendor = new List<string>();
            List<string> listproductType = new List<string>();



            if (!string.IsNullOrWhiteSpace(category))
            {
                string[] splituser = category.Split(',');
                if (splituser.Length > 0)
                {
                    category = string.Format("{0}", string.Join(",", splituser));
                    foreach (var item in splituser)
                    {
                        eqcategory.Add(item);
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(manufact))
            {
                string[] splituser = manufact.Split(',');
                if (splituser.Length > 0)
                {
                    manufact = string.Format("'{0}'", string.Join("','", splituser.Select(i => i.Replace("'", "''"))));
                    foreach (var item in splituser)
                    {
                        manulist.Add(item);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(ProductTypeID))
            {
                string[] splituser = ProductTypeID.Split(',');
                if (splituser.Length > 0)
                {
                    ProductTypeID = string.Format("'{0}'", string.Join("','", splituser.Select(i => i.Replace("'", "''"))));
                    foreach (var item in splituser)
                    {
                        listproductType.Add(item);
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(primaryVendorID))
            {
                string[] splituser = primaryVendorID.Split(',');
                if (splituser.Length > 0)
                {
                    primaryVendorID = string.Format("'{0}'", string.Join("','", splituser.Select(i => i.Replace("'", "''"))));
                    foreach (var item in splituser)
                    {
                        listprimaryVendor.Add(item);
                    }
                }
            }



            ViewBag.StartDate = Start;
            ViewBag.EndDate = End;
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            string newCookie = "";
            if (Request.Cookies[CookieKeys.DateViewFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.DateViewFilter].Value))
            {
                newCookie = Request.Cookies[CookieKeys.DateViewFilter].Value;
                newCookie = Server.UrlDecode(newCookie);
                var CookieVals = newCookie.Split(',');

                if (CookieVals.Length == 3)
                {
                    StartDate = CookieVals[0].ToDateTime().SetZeroHour();
                    EndDate = CookieVals[1].ToDateTime().SetMaxHour();
                }
            }

            if (GetReport.HasValue && GetReport == true)
            {
                DataTable dt;
                if (!string.IsNullOrWhiteSpace(Start) && !string.IsNullOrWhiteSpace(End))
                {
                    StartDate = Convert.ToDateTime(Start).SetZeroHour().ClientToUTCTime();
                    EndDate = Convert.ToDateTime(End).SetMaxHour().ClientToUTCTime();
                    dt = _Util.Facade.InventoryFacade.GetGetEquipmentReportByCompanyId(CurrentUser.CompanyId.Value, StartDate, EndDate, category, manufact, SearchText, ProductTypeID, primaryVendorID);
                }
                else
                {
                    if (StartDate != new DateTime() && EndDate != new DateTime())
                    {
                        dt = _Util.Facade.InventoryFacade.GetGetEquipmentReportByCompanyId(CurrentUser.CompanyId.Value, StartDate, EndDate, category, manufact, SearchText, ProductTypeID, primaryVendorID);
                    }
                    else
                    {
                        dt = _Util.Facade.InventoryFacade.GetGetEquipmentReportByCompanyId(CurrentUser.CompanyId.Value, new DateTime(), new DateTime(), category, manufact, SearchText, ProductTypeID, primaryVendorID);
                    }

                }
                int[] colarray = { 6 };
                int[] rowarray = { dt.Rows.Count + 2 };

                return MakeExcelFromDataTable(dt, "WarehouseInventoryReport", rowarray, colarray);
            }
            EquipmentListWithCountModel Model = new EquipmentListWithCountModel();
            if (!string.IsNullOrWhiteSpace(Start) && !string.IsNullOrWhiteSpace(End))
            {
                StartDate = Convert.ToDateTime(Start).SetZeroHour().ClientToUTCTime();
                EndDate = Convert.ToDateTime(End).SetMaxHour().ClientToUTCTime();

                Model = _Util.Facade.InventoryFacade.GetEquipmentListByCompanyId(CurrentUser.CompanyId.Value, StartDate, EndDate, category, manufact, pageno, pagesize, SearchText, ProductTypeID, primaryVendorID, order);
            }
            else
            {
                if (StartDate != new DateTime() && EndDate != new DateTime())
                {
                    Model = _Util.Facade.InventoryFacade.GetEquipmentListByCompanyId(CurrentUser.CompanyId.Value, StartDate, EndDate, category, manufact, pageno, pagesize, SearchText, ProductTypeID, primaryVendorID, order);
                }
                else
                {
                    Model = _Util.Facade.InventoryFacade.GetEquipmentListByCompanyId(CurrentUser.CompanyId.Value, new DateTime(), new DateTime(), category, manufact, pageno, pagesize, SearchText, ProductTypeID, primaryVendorID, order);
                }
            }
            ViewBag.TotalLeads = Model.TotalEquipmentCount.Counter;

            if (!string.IsNullOrWhiteSpace(Start))
            {
                ViewBag.Start = Start.Replace('_', '/');
            }
            if (!string.IsNullOrWhiteSpace(End))
            {
                ViewBag.End = End.Replace('_', '/');
            }
            List<SelectListItem> eqpCategory = new List<SelectListItem>();
            //eqpCategory.Add(new SelectListItem()
            //{
            //    Text = "Select One",
            //    Value = "-1"
            //});
            eqpCategory.AddRange(_Util.Facade.EquipmentFacade.GetAllEquipmentType().Select(x => new SelectListItem()
            {
                Text = x.Name.ToString(),
                Value = x.Id.ToString()
            }).ToList());
            ViewBag.eqpCategory = eqpCategory;
            List<SelectListItem> manufacturer = new List<SelectListItem>();
            //manufacturer.Add(new SelectListItem()
            //{
            //    Text = "Select One",
            //    Value = "-1"
            //});
            manufacturer.AddRange(_Util.Facade.EquipmentFacade.GetAllManufacturer().Select(x => new SelectListItem()
            {
                Text = x.Name.ToString(),
                Value = x.ManufacturerId.ToString()
            }).ToList());

            List<SelectListItem> primaryVendor = new List<SelectListItem>();
            primaryVendor.AddRange(_Util.Facade.SupplierFacade.GetAllSupplier().Select(x => new SelectListItem()
            {
                Text = x.CompanyName.ToString(),
                Value = x.CompanyName.ToString()
            }).ToList());
            List<SelectListItem> productType = new List<SelectListItem>();
            productType.Add(new SelectListItem()
            {
                Text = "Equipment",
                Value = "1"
            });
            productType.Add(new SelectListItem()
            {
                Text = "Service",
                Value = "2"
            });
            //productType.AddRange(_Util.Facade.EquipmentFacade.GetAllManufacturer().Select(x => new SelectListItem()
            //{
            //    Text = x.Name.ToString(),
            //    Value = x.ManufacturerId.ToString()
            //}).ToList());

            ViewBag.productType = productType;
            ViewBag.primaryVendor = primaryVendor;
            ViewBag.manufacturer = manufacturer;
            ViewBag.listcategory = eqcategory;
            ViewBag.listmanu = manulist;


            ViewBag.listprimaryVendor = listprimaryVendor;
            ViewBag.listproductType = listproductType;
            ViewBag.searchtext = SearchText;




            if (Model.EquipmentList.Count() == 0)
            {
                pageno = 1;
                //customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilter(filter);
            }

            ViewBag.PageNumber = pageno;
            ViewBag.OutOfNumber = 0;


            if (Model.EquipmentList.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.TotalEquipmentCount.Counter;
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
            return View(Model);
        }


        [Authorize]
        public PartialViewResult WH_HistoryPartial(DetailedHistoryFilter filters)
        {
            List<SelectListItem> EstimatorIdList = new List<SelectListItem>();
            List<string> selectedTechsList = new List<string>();
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            ViewBag.LoggedUserName = CurrentUser.FirstName + " " + CurrentUser.LastName;
            ViewBag.LoggedUserId = CurrentUser.UserId;

            if (filters == null)
            {
                filters = new DetailedHistoryFilter();
            }

            if (filters.EquipmentIds != null)
            {
                filters.EquipmentIds = filters.EquipmentIds[0].Split(',');
            }

            if (filters.EmployeeIds != null)
            {
                filters.EmployeeIds = filters.EmployeeIds[0].Split(',');
            }

            EstimatorIdList.Add(new SelectListItem()
            {
                Text = "Select Estimate Id",
                Value = "-1"
            });

            //string PerGrpAssgnTicketId = "";
            //GlobalSetting PerGrpAssgnTicket = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "TicketAssignPermissionGroup");
            //if (PerGrpAssgnTicket != null && !string.IsNullOrWhiteSpace(PerGrpAssgnTicket.Value))
            //{
            //    PerGrpAssgnTicketId = PerGrpAssgnTicket.Value;
            //}

            //var TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndPerGrpId(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid(), PerGrpAssgnTicketId);
            //TechnicianList = TechnicianList.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            //var TechnicianList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);

            //var emplst = new List<SelectListItem>();

            //if (TechnicianList != null)
            //{
            //    if (base.IsPermitted(UserPermissions.InventoryPermissions.TechDropDownEditable))
            //    {
            //        emplst.Add(new SelectListItem()
            //        {
            //            Text = "Warehouse",
            //            Value = "22222222-2222-2222-2222-222222222222",
            //            Selected = false,
            //        });
            //    }
            //    foreach (var tech in TechnicianList)
            //    {
            //        emplst.Add(new SelectListItem()
            //        {
            //            Text = tech.FirstName + " " + tech.LastName,
            //            Value = tech.UserId.ToString(),
            //            Selected = tech.UserId == CurrentUser.UserId ? true : false,
            //        });
            //        if (tech.UserId == CurrentUser.UserId)
            //        {
            //            selectedTechsList.Add(tech.UserId.ToString());
            //        }

            //    }
            //}

            //ViewBag.TechList = emplst; // TechList;

            //List<string> selectsts = new List<string>();

            //if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            //{
            //    string[] splituser = filters.selectsts.Split(',');
            //    if (splituser.Length > 0)
            //    {
            //        filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
            //        foreach (var item in splituser)
            //        {
            //            selectsts.Add(item);
            //        }
            //    }
            //}
            //ViewBag.selectedTechsList = selectedTechsList;
            //ViewBag.selectsts = selectsts;
            //ViewBag.EstimatorId = filters.EstimatorId;

            var TransferLocations = _Util.Facade.EmployeeFacade.GetTransferLocations(CurrentUser.CompanyId.Value);

            var emplst = new List<SelectListItem>();
            if (TransferLocations != null)
            {

                foreach (var location in TransferLocations)
                {

                    emplst.Add(new SelectListItem()
                    {
                        Text = location.UserName,
                        Value = location.UserId.ToString(),
                        Selected = location.UserId.ToString() == "22222222-2222-2222-2222-222222222222",
                    });

                    if (location.UserId == CurrentUser.UserId)
                    {
                        selectedTechsList.Add(location.UserId.ToString());
                    }

                }
            }

            ViewBag.TechList = emplst;
            ViewBag.DefaultSelectedTech = "22222222-2222-2222-2222-222222222222";

            List<string> selectsts = new List<string>();

            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectsts.Add(item);
                    }
                }
            }
            ViewBag.selectedTechsList = selectedTechsList;
            ViewBag.selectsts = selectsts;
            ViewBag.EstimatorId = filters.EstimatorId;
            return PartialView("_WH_HistoryPartial");

        }

        public ActionResult WH_HistoryListPartial(DetailedHistoryFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (filters.EquipmentIds[0] != "undefined")
            {
                filters.EquipmentIds = filters.EquipmentIds[0].Split(',');
            }

            if (filters.EmployeeIds[0] != "undefined")
            {
                filters.EmployeeIds = filters.EmployeeIds[0].Split(',');
            }

            //filters.EmployeeIds = new string[] { "22222222-2222-2222-2222-222222222222" };

            if (filters.ManufacturerIds[0] != "undefined")
            {
                filters.ManufacturerIds = filters.ManufacturerIds[0].Split(',');
            }
            if (filters.Start == "01/01/0001" || filters.Start == null)
            {
                filters.Start = null;
            }
            if (filters.End == "01/01/0001" || filters.End == null)
            {
                var Today = DateTime.Today;
                filters.End = DateTime.Today.ToString("MM/dd/yyyy");
            }


            List<string> selectSts = new List<string>();
            List<string> selectedEquipmentsList = new List<string>();
            List<string> selectedManufacturersList = new List<string>();
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectSts.Add(item);
                    }
                }
            }

            #region Inits
            if (filters == null)
            {
                filters = new DetailedHistoryFilter();
            }
            //if (filters.EquipmentIds == null)
            //{
            //    filters.EquipmentIds = new int[] { 0 };
            //}

            if (filters.PageNo == 0)
            {
                filters.PageNo = 1;
            }
            filters.CompanyId = CurrentUser.CompanyId.Value;
            //filters.EmployeeIds = filters.EmployeeIds;

            DetailedHistoryVM Model = new DetailedHistoryVM();
            #endregion

            if (filters.GetReport)
            {
                DataTable dt = _Util.Facade.InventoryFacade.GetWHHistoryListByFiltersR(filters).Tables[0];
                //dt.Columns.Remove("Total RMR");
                int[] colarray = { 2 };
                int[] rowarray = { dt.Rows.Count + 2 };
                return MakeExcelFromDataTable(dt, "WarehouseHistory", rowarray, colarray);
            }

            Model = _Util.Facade.InventoryFacade.GetWHHistoryListByFilters(filters);


            ViewBag.PageNumber = filters.PageNo;
            ViewBag.OutOfNumber = 0;
            filters.PageSize = Model.ItemsCount; //_Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);

            if (Model.ItemsCount > 0)
            {
                ViewBag.OutOfNumber = Model.ItemsCount;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);
            ViewBag.order = filters.order;
            string poPreText = "PO";
            GlobalSetting _poPretxt = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PrefixForPurchaseOrderId");
            if (_poPretxt != null && !string.IsNullOrWhiteSpace(_poPretxt.Value) && _poPretxt.Value != "")
            {
                poPreText = _poPretxt.Value;
            }


            bool TechnicianColumnPO = false;
            GlobalSetting tehcnicianColumn = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PurchaseOrderTechnicianColumn");
            if (tehcnicianColumn != null && !string.IsNullOrWhiteSpace(tehcnicianColumn.Value))
            {
                TechnicianColumnPO = Convert.ToBoolean(tehcnicianColumn.Value);
            }
            ViewBag.TechnicianColumnPO = TechnicianColumnPO;

            var EquipmentsList = new List<SelectListItem>();
            var ManufacturersList = new List<SelectListItem>();

            foreach (var item in Model.EquipmentsList)
            {
                var listItem = new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value,
                    Selected = true
                };


                if (filters.EquipmentIds.Count() > 0 && filters.EquipmentIds[0] != "undefined")
                {
                    if (!filters.EquipmentIds.Contains(item.Value))
                    {
                        listItem.Selected = false;
                    }
                    else
                    {
                        selectedEquipmentsList.Add(item.Value);
                    }
                }
                else
                {
                    selectedEquipmentsList.Add(item.Value);
                }

                EquipmentsList.Add(listItem);

            }

            foreach (var item in Model.ManufacturersList)
            {
                var listItem = new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Value,
                    Selected = true
                };

                if (filters.ManufacturerIds.Count() > 0 && filters.ManufacturerIds[0] != "undefined")
                {
                    if (!filters.ManufacturerIds.Contains(item.Value))
                    {
                        listItem.Selected = false;
                    }
                    else
                    {
                        selectedManufacturersList.Add(item.Value);
                    }
                }
                else
                {
                    selectedManufacturersList.Add(item.Value);
                }

                ManufacturersList.Add(listItem);
            }

            ViewBag.EqList = EquipmentsList;
            ViewBag.MfgList = ManufacturersList;
            @ViewBag.selectedEquipmentsList = selectedEquipmentsList;
            @ViewBag.selectedManufacturersList = selectedManufacturersList;
            return View("_WH_HistoryListPartial", Model);
        }

        [Authorize]
        public ActionResult WH_HistoryPopup(DetailedHistoryFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            #region Permission Check
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.PurchaseOrderWareHouseTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #endregion
            if (filters.Start == null)
            {
                filters.Start = "01/01/1901";
            }
            if (filters.End == null)
            {
                filters.End = DateTime.UtcNow.ToString("MM-dd-yyyy");
            }

            if (filters.EquipmentIds != null)
            {
                filters.EquipmentIds = filters.EquipmentIds[0].Split(',');
            }

            if (filters.EmployeeIds != null)
            {
                filters.EmployeeIds = filters.EmployeeIds[0].Split(',');
            }

            List<string> selectSts = new List<string>();
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectSts.Add(item);
                    }
                }
            }

            #region Inits
            if (filters == null)
            {
                filters = new DetailedHistoryFilter();
            }

            if (filters.PageNo == 0)
            {
                filters.PageNo = 1;
            }
            filters.CompanyId = CurrentUser.CompanyId.Value;
            //filters.EmployeeIds = filters.EmployeeIds;
            filters.PageSize = _Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);
            DetailedEquipmentVM Model = new DetailedEquipmentVM();
            #endregion

            Model = _Util.Facade.InventoryFacade.GetWHHistoryDetailedListByFilters(filters);
            ViewBag.PageNumberTickets = filters.PageNo;
            ViewBag.OutOfNumberTickets = 0;
            ViewBag.PageNumberTransfers = filters.PageNoTransfers;
            ViewBag.OutOfNumberTransfers = 0;

            if (Model.TicketsCount > 0)
            {
                ViewBag.OutOfNumberTickets = Model.TicketsCount;
            }

            if (Model.TransfersCount > 0)
            {
                ViewBag.OutOfNumberTransfers = Model.TransfersCount;
            }

            if ((int)ViewBag.PageNumberTickets * filters.PageSize > (int)ViewBag.OutOfNumberTickets)
            {
                ViewBag.CurrentNumberTickets = (int)ViewBag.OutOfNumberTickets;
            }
            else
            {
                ViewBag.CurrentNumberTickets = (int)ViewBag.PageNumberTickets * filters.PageSize;
            }

            if ((int)ViewBag.PageNumberTransfers * filters.PageSize > (int)ViewBag.OutOfNumberTransfers)
            {
                ViewBag.CurrentNumberTransfers = (int)ViewBag.OutOfNumberTransfers;
            }
            else
            {
                ViewBag.CurrentNumberTransfers = (int)ViewBag.PageNumberTransfers * filters.PageSize;
            }

            ViewBag.PageCountTickets = Math.Ceiling((double)ViewBag.OutOfNumberTickets / filters.PageSize);
            ViewBag.PageCountTransfers = Math.Ceiling((double)ViewBag.OutOfNumberTransfers / filters.PageSize);
            ViewBag.order = filters.order;
            string poPreText = "PO";
            GlobalSetting _poPretxt = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PrefixForPurchaseOrderId");
            if (_poPretxt != null && !string.IsNullOrWhiteSpace(_poPretxt.Value) && _poPretxt.Value != "")
            {
                poPreText = _poPretxt.Value;
            }

            return View("_WH_HistoryPopup", Model);
        }


        [HttpPost]
        public JsonResult BulkTechReceiveConfirm(List<BulkTechReceiveRequest> bulkTechReceiveConfirmRequest)
        {
            bool result = false;
            string userMessage = "Received successfully";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)(User);

            if (bulkTechReceiveConfirmRequest != null)
            {
                for (int i = 0; i < bulkTechReceiveConfirmRequest.Count; i++)
                {
                    AssignedInventoryTechReceived objassigntech = new AssignedInventoryTechReceived();
                    objassigntech.TechnicianId = bulkTechReceiveConfirmRequest[i].techid;
                    objassigntech.EquipmentId = bulkTechReceiveConfirmRequest[i].eqpid;
                    objassigntech.Id = bulkTechReceiveConfirmRequest[i].id;
                    objassigntech.ReqSrc = bulkTechReceiveConfirmRequest[i].reqSrc;

                    //TODO: Check if still there is still sufficient balance on hand
                    if (bulkTechReceiveConfirmRequest[i].eqpid != new Guid() && bulkTechReceiveConfirmRequest[i].techid != new Guid())
                    {
                        objassigntech = _Util.Facade.InventoryFacade.GetAllAssignedInventory(objassigntech);
                        if (objassigntech.IsApprove == true || objassigntech.IsDecline == true)
                        {
                            continue;
                        }
                        TransferResult transferResult = new TransferResult();
                        if (objassigntech.ReqSrc == "[MSRSTK-WHTT]")
                        {
                            transferResult = new EquipmentTransfer().TransferWHToTech((Guid)objassigntech.ReceivedBy, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, objassigntech.TechnicianId, "[MSRSTK-WHTT]", objassigntech.Quantity);
                        }
                        //else if (objassigntech.TechnicianId == new Guid("22222222-2222-2222-2222-222222222222"))
                        //{
                        //    transferResult = new EquipmentTransfer().TransferWHToTech((Guid)objassigntech.ReceivedBy, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, objassigntech.TechnicianId, "[TT-Approve]", objassigntech.Quantity);
                        //}
                        //else if (objassigntech.ReceivedBy.Value == new Guid("22222222-2222-2222-2222-222222222222"))
                        //{
                        //    transferResult = new EquipmentTransfer().TransferTechToWH(objassigntech.TechnicianId, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, (Guid)objassigntech.ReceivedBy, "[TT-Approve]", objassigntech.Quantity);
                        //}
                        //else if
                        //{
                        //    //transferResult = new EquipmentTransfer().TransferTechToTech(objassigntech.TechnicianId, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, objassigntech.ReceivedBy.Value, "[TT-Approve]", objassigntech.Quantity);
                        //    transferResult = new EquipmentTransfer().TransferTechToTech(objassigntech.TechnicianId, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, objassigntech.ReceivedBy.Value, objassigntech.ReqSrc, objassigntech.Quantity);
                        //}

                        else if (objassigntech.ReceivedBy.Value.IsVirtualLocation() && objassigntech.TechnicianId.IsVirtualLocation())
                        {
                            transferResult = new EquipmentTransfer().TransferWHToWH(objassigntech.TechnicianId, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, (Guid)objassigntech.ReceivedBy, objassigntech.ReqSrc, objassigntech.Quantity); //"[TT-Approve]"
                        }
                        else if (objassigntech.TechnicianId.IsVirtualLocation() && (objassigntech.ReqSrc != "[PURORD-WHTT]"))
                        {
                            transferResult = new EquipmentTransfer().TransferWHToTech((Guid)objassigntech.ReceivedBy, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, objassigntech.TechnicianId, objassigntech.ReqSrc, objassigntech.Quantity); //"[TT-Approve]"
                        }
                        else if (objassigntech.ReceivedBy.Value.IsVirtualLocation())
                        {
                            transferResult = new EquipmentTransfer().TransferTechToWH(objassigntech.TechnicianId, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, (Guid)objassigntech.ReceivedBy, objassigntech.ReqSrc, objassigntech.Quantity); //"[TT-Approve]"
                        }
                        else
                        {
                            transferResult = new EquipmentTransfer().TransferTechToTech(objassigntech.TechnicianId, objassigntech.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, objassigntech.ReceivedBy.Value, objassigntech.ReqSrc, objassigntech.Quantity); //"[TT-Approve]"
                        }

                        objassigntech.IsApprove = transferResult.IsReleased;
                        if (transferResult.IsReleased) userMessage = "Transfer Released";
                        objassigntech.IsReceived = transferResult.IsAdded;
                        if (transferResult.IsAdded) userMessage = "Transfer Added";

                        if ((bool)objassigntech.IsApprove && objassigntech.IsReceived)
                        {
                            objassigntech.ReceivedDate = DateTime.Now;

                            objassigntech.ClosedBy = CurrentUser.UserId;
                            result = _Util.Facade.InventoryFacade.UpdateTechReceive_DG(objassigntech);
                            userMessage = "Transfer Approved";

                            logger.WithProperty("tags", $"Trasfer Equipment Approved datetime {DateTime.Now} and {objassigntech.Id}")
                          .WithProperty("params", JsonConvert.SerializeObject(objassigntech))
                          .Trace($"Equipment History Approved by {CurrentUser.UserId}.");
                        }
                    }
                }
            }
            return Json(new { result = result, message = userMessage });
        }


        [HttpPost]
        public JsonResult BulkInventoryTechReceiveDecline(List<BulkTechReceiveRequest> bulkTechReceiveDeclineRequest)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)(User);

            bool result = false;
            if (bulkTechReceiveDeclineRequest != null)
            {
                for (int i = 0; i < bulkTechReceiveDeclineRequest.Count; i++)
                {
                    if (bulkTechReceiveDeclineRequest[i].id > 0)
                    {
                        var objInventoryTechReceive = _Util.Facade.InventoryFacade.GetTechReceiveById(bulkTechReceiveDeclineRequest[i].id);
                        if (objInventoryTechReceive != null)
                        {
                            if (objInventoryTechReceive.IsApprove == true || objInventoryTechReceive.IsDecline == true)
                            {
                                continue;
                            }

                            objInventoryTechReceive.ReceivedDate = DateTime.Now;

                            objInventoryTechReceive.IsDecline = true;
                            objInventoryTechReceive.ClosedBy = CurrentUser.UserId;

                            logger.WithProperty("tags", $"Trasfer Equipment Declined datetime {DateTime.Now} and {objInventoryTechReceive.Id}")
                                                     .WithProperty("params", JsonConvert.SerializeObject(objInventoryTechReceive))
                                                     .Trace($"Equipment History Declined by {CurrentUser.UserId}.");

                            result = _Util.Facade.InventoryFacade.UpdateTechReceive(objInventoryTechReceive);
                        }
                    }
                }
            }
            return Json(new { result = result, message = "Transfer declined" });
        }
        #endregion Digiture Changes
    }
}