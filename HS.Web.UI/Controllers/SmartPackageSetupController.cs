using HS.Entities;
using HS.Framework;
using HS.Web.UI.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class SmartPackageSetupController : BaseController
    {
        // GET: SmartPackageSetup
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }

        public ActionResult SecondaryContactListForCreditCheck(Guid CustomerId,string For)
        {
            List<CustomerAdditionalContact> contactList = new List<CustomerAdditionalContact>();
            contactList = _Util.Facade.AdditionalContactFacade.GetAllAdditionalContactByCustomerId(CustomerId);
            ViewBag.For = For;
            return View(contactList);
        }
        public ActionResult AddSecondaryCreditCheckContact(int? Id,string ContactFor)
        {
            CustomerAdditionalContact contact = new CustomerAdditionalContact();
            if(Id.HasValue && Id > 0)
            {
                contact = _Util.Facade.AdditionalContactFacade.GetById(Id.Value);
            }
            ViewBag.ContactFor = ContactFor;
            return View(contact);
        }
        public ActionResult GetAllOtherCustomerContact(Guid CustomerId,bool? IsSoftCheck,string Bureau,string For)
        {
            List<CustomerAdditionalContact> contactList = new List<CustomerAdditionalContact>();
            contactList = _Util.Facade.AdditionalContactFacade.GetAllAdditionalContactByCustomerId(CustomerId);
            ViewBag.IsSoftCheck = IsSoftCheck;
            ViewBag.Bureau = Bureau;
            ViewBag.For = For;
            return View(contactList);
        }
        [HttpPost]
        public JsonResult AddSecondaryCreditCheckContact(CustomerAdditionalContact creditContact)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            string message = "Contact not saved";

            if (creditContact.Id > 0)
            {
                CustomerAdditionalContact oldContact = _Util.Facade.AdditionalContactFacade.GetById(creditContact.Id);
                if(oldContact != null)
                {
                    oldContact.FirstName = creditContact.FirstName;
                    oldContact.LastName = creditContact.LastName;
                    oldContact.CorpCity = creditContact.CorpCity;
                    oldContact.CorpState = creditContact.CorpState;
                    oldContact.CorpZipCode = creditContact.CorpZipCode;
                    oldContact.SSN = creditContact.SSN;
                    oldContact.CorpAddress = creditContact.CorpAddress;
                    oldContact.Email = creditContact.Email;
                    oldContact.Phone = creditContact.Phone;
                    oldContact.DOB = creditContact.DOB;
                    oldContact.AlternateContact = true;
                    _Util.Facade.AdditionalContactFacade.UpdateAdditionalContact(oldContact);
                    result = true;
                    message = "Contact Updated Successfully.";
                }
            }
            else
            {
                creditContact.CustomerId = creditContact.CustomerId;
         
         
                creditContact.AlternateContact = true;
                _Util.Facade.AdditionalContactFacade.InsertAdditionalContact(creditContact);

                result = true;
                message = "Contact saved succefully.";
            }
            return Json(new { result = result, message = message });
        }
        [Authorize]
        public ActionResult SmartPackageSettingsPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView("SmartPackageSettingsPartial");
        }



        [Authorize]
        public ActionResult LoadCompanySmartPackageSettingsPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView("LoadCompanySmartPackageSettingsPartial");
        }

        [Authorize]
        public ActionResult CompanyPackageListPartial(SmartPackageFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            filter.CompanyId = CurrentUser.CompanyId.Value;
            if (filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }
            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PackageListPageSize");
            if (glob != null)
            {
                filter.UnitPerPage = Convert.ToInt32(glob.Value);
            }
            else
            {
                filter.UnitPerPage = 20;
            }
            if (filter.PageNumber == null || filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }

            PackageModel packageModel = new PackageModel();
            packageModel = _Util.Facade.SmartPackageFacade.GetAllSmartPackageByFilter(filter);
            ViewBag.OutOfNumber = packageModel.TotalCount.TotalCount;

            if ((int)ViewBag.OutOfNumber == 0)
            {
                ViewBag.Message = "No Content Available !";
            }
            if (ViewBag.OutOfNumber == 0)
            {
                filter.PageNumber = 1;
            }
            ViewBag.PageNumber = filter.PageNumber;

            if ((int)ViewBag.PageNumber * filter.UnitPerPage > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.UnitPerPage;
            }

            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.UnitPerPage.Value);


            return PartialView("_CompanyPackageListPartial", packageModel.PackageList);
        }

        [Authorize]
        public ActionResult AddCompanyPackagePartial(int? Id, string From)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SmartPackage model = new SmartPackage();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #region ViewBags
            ViewBag.From = "";
            if (!string.IsNullOrEmpty(From))
            {
                ViewBag.From = From;
            }
            List<SelectListItem> SmartSystemTypeList = new List<SelectListItem>();
            SmartSystemTypeList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            SmartSystemTypeList.AddRange(ViewBag.PackageList = _Util.Facade.SmartPackageFacade
              .GetAllSmartSystemType(CurrentUser.CompanyId.Value).Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.Name.ToString(),
                                 Value = x.Id.ToString()
                             }).ToList());
            ViewBag.SmartSystemTypeList = SmartSystemTypeList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            List<SelectListItem> PackageType = new List<SelectListItem>();
            PackageType.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("PackageType").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.PackageType = PackageType;
            #region Manufacturers
            List<SelectListItem> ManufacturerList = new List<SelectListItem>();
            ManufacturerList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            ManufacturerList.AddRange(_Util.Facade.ManufacturerFacade.GetAllManufacturerByCompanyId(CurrentUser.CompanyId.Value).OrderBy(x => x.Name).Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.ManufacturerId.ToString()
                }).ToList());
            ViewBag.ManufacturerList = ManufacturerList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion

            List<SelectListItem> SmartInstallTypeList = new List<SelectListItem>();
            SmartInstallTypeList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            ViewBag.SmartInstallTypeList = SmartInstallTypeList;
            List<SelectListItem> usertype = new List<SelectListItem>();
            usertype.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("CustomerType").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.typeuser = usertype;
            #endregion
            if (Id.HasValue && Id > 0)
            {
                model = _Util.Facade.SmartPackageFacade.GetPackageByIdAndCompanyId(Id.Value, CurrentUser.CompanyId.Value);
                SmartPackageSystemInstallTypeMap SPSIT = _Util.Facade.SmartPackageFacade.GetSmartPackageSystemInstallTypeMapByPackageId(model.PackageId);
                if (SPSIT != null)
                {
                    SmartInstallTypeList.AddRange(_Util.Facade.SmartPackageFacade.LoadInstallType(SPSIT.SmartSystemTypeId, CurrentUser.CompanyId.Value).OrderBy(x => x.Name != "Please Select").ThenBy(x => x.Name).Select(x =>
                              new SelectListItem()
                              {
                                  Text = x.Name.ToString(),
                                  Value = x.Id.ToString()
                              }).ToList());
                    model.SmartSystemTypeId = SPSIT.SmartSystemTypeId;
                    model.SmartInstallTypeId = SPSIT.SmartInstallTypeId;
                }
                ViewBag.SmartInstallTypeList = SmartInstallTypeList;
                model.MMRRange = _Util.Facade.PackageFacade.GetMMrRangeByPackageId(model.PackageId);
            }
            List<CustomerNoPrefix> customerNoPrefix = _Util.Facade.CustomerSystemNoFacade.GetAllNumberPrefixByCompanyId(CurrentUser.CompanyId.Value);
            List<SelectListItem> customerNoPrefixList = new List<SelectListItem>();
            customerNoPrefixList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            customerNoPrefixList.AddRange(customerNoPrefix.Select(x =>
                         new SelectListItem()
                         {
                             Text = x.Name.ToString(),
                             Value = x.Name.ToString()
                         }).ToList());
            ViewBag.customerNoPrefixList = customerNoPrefixList;
            return PartialView("_AddCompanyPackagePartial", model);
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddCompanyPackage(SmartPackage _Package, MMRRange _MMRRange)
        {
            var result = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            _Package.CompanyId = CurrentUser.CompanyId.Value;
            if (CurrentUser == null)
            {
                return Json(result);
            }
            if (_Package.Id > 0)
            {
                SmartPackage OldPackage = _Util.Facade.SmartPackageFacade.GetPackageByIdAndCompanyId(_Package.Id, _Package.CompanyId);
                SmartPackageSystemInstallTypeMap OldPackageMap = _Util.Facade.SmartPackageFacade.GetSmartPackageSystemInstallTypeMapByPackageId(OldPackage.PackageId);
                if (OldPackage != null)
                {
                    OldPackage.PackageName = _Package.PackageName;
                    OldPackage.EquipmentMaxLimit = _Package.EquipmentMaxLimit;
                    OldPackage.ActivationFee = _Package.ActivationFee;
                    OldPackage.NonConforming = _Package.NonConforming;
                    OldPackage.MinCredit = _Package.MinCredit;
                    OldPackage.MaxCredit = _Package.MaxCredit;
                    OldPackage.ManufacturerId = _Package.ManufacturerId;
                    OldPackage.PackageCode = _Package.PackageCode;
                    OldPackage.UserType = _Package.UserType;
                    OldPackage.ConformingFee = _Package.ConformingFee;
                    OldPackage.PackageType = _Package.PackageType;
                    OldPackage.IsDelete = false;
                    OldPackage.CustomerNumber = _Package.CustomerNumber;
                    OldPackage.LastUpdatedBy = CurrentUser.UserId;
                    OldPackage.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    result = _Util.Facade.SmartPackageFacade.UpdatePackage(OldPackage);
                    if (OldPackageMap != null)
                    {
                        OldPackageMap.SmartInstallTypeId = _Package.SmartInstallTypeId;
                        OldPackageMap.SmartSystemTypeId = _Package.SmartSystemTypeId;
                        result = _Util.Facade.SmartPackageFacade.UpdateSmartPackageSystemInstallTypeMap(OldPackageMap);
                    }
                    else
                    {
                        SmartPackageSystemInstallTypeMap model = new SmartPackageSystemInstallTypeMap();
                        model.PackageId = OldPackage.PackageId;
                        model.SmartInstallTypeId = _Package.SmartInstallTypeId;
                        model.SmartSystemTypeId = _Package.SmartSystemTypeId;
                        result = _Util.Facade.SmartPackageFacade.InsertSmartPackageSystemInstallTypeMap(model);
                    }
                }
            }
            else
            {
                _Package.PackageId = Guid.NewGuid();
                _Package.LastUpdatedBy = CurrentUser.UserId;
                _Package.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                _Package.IsActive = true;
                _Package.IsPromo = true;
                _Package.StartDate = DateTime.Now;
                _Package.EndDate = DateTime.Now.AddMonths(12);
                _Package.TotalRMR = 0;
                _Package.IsDelete = false;
                result = _Util.Facade.SmartPackageFacade.InsertPackage(_Package);
                SmartPackageSystemInstallTypeMap model = new SmartPackageSystemInstallTypeMap();
                model.PackageId = _Package.PackageId;
                model.SmartInstallTypeId = _Package.SmartInstallTypeId;
                model.SmartSystemTypeId = _Package.SmartSystemTypeId;

                result = _Util.Facade.SmartPackageFacade.InsertSmartPackageSystemInstallTypeMap(model);
            }
            if (result == true)
            {
                var delMMrange = _Util.Facade.PackageFacade.DeleteMMrRangeByPackageId(CurrentUser.CompanyId.Value, _Package.PackageId);
                if (delMMrange == true)
                {
                    MMRRange objMMRRange = new MMRRange()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        PackageId = _Package.PackageId,
                        MinMMR = _MMRRange.MinMMR,
                        MaxMMR = _MMRRange.MaxMMR
                    };
                    _Util.Facade.PackageFacade.InsertMMrRange(objMMRRange);
                }
                else
                {
                    MMRRange objMMRRange = new MMRRange()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        PackageId = _Package.PackageId,
                        MinMMR = _MMRRange.MinMMR,
                        MaxMMR = _MMRRange.MaxMMR
                    };
                    _Util.Facade.PackageFacade.InsertMMrRange(objMMRRange);
                }
            }
            return Json(new { result = result, PackageId = _Package.PackageId });
        }

        public JsonResult ClonePackage(int id)
        {
            var result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            #region Insert Smart Package
            SmartPackage OldPackage = _Util.Facade.SmartPackageFacade.GetPackageById(id);
            SmartPackage NewPackage = new SmartPackage();

            SmartPackageSystemInstallTypeMap OldPackageMap = _Util.Facade.SmartPackageFacade.GetSmartPackageSystemInstallTypeMapByPackageId(OldPackage.PackageId);
            if (OldPackage != null)
            {
                ExistSmartPackage PackageExist = _Util.Facade.SmartPackageFacade.GetAllDuplicateSmartPackagesCountByPackageName(OldPackage.PackageName);
                NewPackage.PackageId = Guid.NewGuid();
                if (PackageExist.ExistCount > 0)
                {
                    NewPackage.PackageName = OldPackage.PackageName + "(Clone " + PackageExist.ExistCount + ")";
                }
                else
                {
                    NewPackage.PackageName = OldPackage.PackageName;
                }

                NewPackage.CompanyId = OldPackage.CompanyId;
                NewPackage.EquipmentMaxLimit = OldPackage.EquipmentMaxLimit;
                NewPackage.ActivationFee = OldPackage.ActivationFee;
                NewPackage.NonConforming = OldPackage.NonConforming;
                NewPackage.MinCredit = OldPackage.MinCredit;
                NewPackage.MaxCredit = OldPackage.MaxCredit;
                NewPackage.ManufacturerId = OldPackage.ManufacturerId;
                NewPackage.PackageCode = OldPackage.PackageCode;
                NewPackage.UserType = OldPackage.UserType;
                NewPackage.InstallType = OldPackage.InstallType;
                NewPackage.PackageCode = "";
                NewPackage.PackageType = OldPackage.PackageType;
                NewPackage.SystemType = OldPackage.SystemType;
                NewPackage.ConformingFee = OldPackage.ConformingFee;
                NewPackage.IsActive = OldPackage.IsActive;
                NewPackage.IsPromo = OldPackage.IsPromo;
                NewPackage.ConformingFee = OldPackage.ConformingFee;
                NewPackage.NonConforming = OldPackage.NonConforming;
                NewPackage.UserType = OldPackage.UserType;
                NewPackage.StartDate = OldPackage.StartDate;
                NewPackage.LastUpdatedBy = CurrentUser.UserId;
                NewPackage.IsDelete = true;
                NewPackage.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                NewPackage.ManufacturerId = OldPackage.ManufacturerId;
                NewPackage.CustomerNumber = OldPackage.CustomerNumber;

                result = _Util.Facade.SmartPackageFacade.InsertPackage(NewPackage);
                SmartPackageSystemInstallTypeMap oldInstallMap = _Util.Facade.SmartPackageFacade.GetSmartPackageSystemInstallTypeMapByPackageId(OldPackage.PackageId);
                SmartPackageSystemInstallTypeMap newInstallMap = new SmartPackageSystemInstallTypeMap()
                {
                    PackageId = NewPackage.PackageId,
                    SmartInstallTypeId = oldInstallMap.SmartInstallTypeId,
                    SmartSystemTypeId = oldInstallMap.SmartSystemTypeId

                };
                result = _Util.Facade.SmartPackageFacade.InsertSmartPackageSystemInstallTypeMap(newInstallMap);
            }
            #endregion

            if (result == true)
            {
                #region Insert MMRRange
                var delMMrange = _Util.Facade.PackageFacade.DeleteMMrRangeByPackageId(CurrentUser.CompanyId.Value, NewPackage.PackageId);
                MMRRange _MMRRange = _Util.Facade.PackageFacade.GetMMrRangeByPackageId(OldPackage.PackageId);
                if (delMMrange == true)
                {
                    MMRRange objMMRRange = new MMRRange()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        PackageId = NewPackage.PackageId,
                        MinMMR = _MMRRange.MinMMR,
                        MaxMMR = _MMRRange.MaxMMR
                    };
                    _Util.Facade.PackageFacade.InsertMMrRange(objMMRRange);
                }
                else
                {
                    MMRRange objMMRRange = new MMRRange()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        PackageId = NewPackage.PackageId,
                        MinMMR = _MMRRange.MinMMR,
                        MaxMMR = _MMRRange.MaxMMR
                    };
                    _Util.Facade.PackageFacade.InsertMMrRange(objMMRRange);
                }

                #endregion
                #region Insert Smart Package EquipmentService
                List<SmartPackageEquipmentService> oldServiceList = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentServiceByPackageId(OldPackage.PackageId);
                SmartPackageEquipmentService newPackageEqp = new SmartPackageEquipmentService();

                if (oldServiceList.Count > 0)
                {
                    foreach (var item in oldServiceList)
                    {
                        newPackageEqp.PackageId = NewPackage.PackageId;
                        newPackageEqp.CompanyId = item.CompanyId;
                        newPackageEqp.EquipmentId = item.EquipmentId;
                        newPackageEqp.IsFree = item.IsFree;
                        newPackageEqp.IsSelected = item.IsSelected;
                        newPackageEqp.EptNo = item.EptNo;
                        newPackageEqp.Type = item.Type;
                        newPackageEqp.Price = item.Price;
                        newPackageEqp.Status = item.Status;
                        newPackageEqp.LastUpdatedBy = item.LastUpdatedBy;
                        newPackageEqp.LastUpdatedDate = item.LastUpdatedDate;
                        newPackageEqp.SmartPackageEquipmentServiceId = item.SmartPackageEquipmentServiceId;
                        _Util.Facade.SmartPackageFacade.InsertSmartPackageEquipmentService(newPackageEqp);
                    }
                }
                #endregion
            }
            return Json(new { result = result, PackageId = NewPackage.Id });
        }

        [HttpPost]
        [Authorize]
        public JsonResult SmartPackageStatusChange(int Id)
        {
            var result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (CurrentUser == null)
            {
                return Json(result);
            }
            SmartPackage smartPackage = _Util.Facade.SmartPackageFacade.GetAllSmartPackageByCompanyId(CurrentUser.CompanyId.Value).Where(x => x.Id == Id).FirstOrDefault();
            var temp = smartPackage.IsActive;
            if (smartPackage.IsActive == true)
            {
                smartPackage.IsActive = false;
            }
            else
            {
                smartPackage.IsActive = true;
            }
            smartPackage.LastUpdatedDate = DateTime.Now;
            smartPackage.LastUpdatedBy = CurrentUser.UserId;
            result = _Util.Facade.SmartPackageFacade.UpdatePackage(smartPackage);
            if (result)
            {
                result = Convert.ToBoolean(smartPackage.IsActive);
            }
            else
            {
                result = Convert.ToBoolean(temp);
            }
            return Json(result);
        }


        [HttpPost]
        [Authorize]
        public JsonResult SmartServiceStatusChange(int Id)
        {
            var result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (CurrentUser == null)
            {
                return Json(result);
            }
            SmartPackageEquipmentService smartPackageEqp = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService(CurrentUser.CompanyId.Value, "Service").Where(x => x.Id == Id).FirstOrDefault();
            var temp = smartPackageEqp.Status;
            if (smartPackageEqp.Status == true)
            {
                smartPackageEqp.Status = false;
            }
            else
            {
                smartPackageEqp.Status = true;
            }
            smartPackageEqp.Type = "Service";
            smartPackageEqp.LastUpdatedBy = CurrentUser.UserId;
            smartPackageEqp.LastUpdatedDate = DateTime.Now;
            result = _Util.Facade.SmartPackageFacade.UpdateSmartPackageEquipmentService(smartPackageEqp);
            if (result)
            {
                result = Convert.ToBoolean(smartPackageEqp.Status);
            }
            else
            {
                result = Convert.ToBoolean(temp);
            }
            return Json(result);
        }
        [Authorize]
        public ActionResult PackageSettingsList(Guid id)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ViewBag.PackageId = id.ToString();

            SmartPackage smartPackage = _Util.Facade.SmartPackageFacade.GetAllSmartPackageByCompanyId(CurrentLoggedInUser.CompanyId.Value).Where(x => x.PackageId == id).FirstOrDefault();
            ViewBag.PackageName = smartPackage.PackageName;
            ViewBag.LeadType = smartPackage.UserType;
            ViewBag.packIncludeEquipment = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService(CurrentLoggedInUser.CompanyId.Value, "Include").Where(x => x.PackageId == id).ToList();
            ViewBag.packDevice = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService(CurrentLoggedInUser.CompanyId.Value, "Device").Where(x => x.PackageId == id).ToList();
            ViewBag.packOptional = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService(CurrentLoggedInUser.CompanyId.Value, "Optional").Where(x => x.PackageId == id).ToList();
            ViewBag.packServices = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService(CurrentLoggedInUser.CompanyId.Value, "Service").Where(x => x.PackageId == id).ToList();
            var Type = LabelHelper.CommissionType.PackageRMR;
            ViewBag.SalesCommision = _Util.Facade.SmartPackageFacade.GetAllPackageComissionByLeadTypeandPackageType(Type, smartPackage.UserType, smartPackage.PackageType);
            return PartialView();
        }

        [Authorize]
        public ActionResult SmartServiceView(Guid id)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var equipmentService = _Util.Facade.EquipmentFacade.GetAllEquipmentServiceByCompanyId(CurrentLoggedInUser.CompanyId.Value).Where(x => x.EquipmentId == id).FirstOrDefault();
            ViewBag.PackageId = id.ToString();
            ViewBag.PackageName = equipmentService.Name;
            ViewBag.SalesCommision = _Util.Facade.SmartPackageFacade.GetAllSalesCommisionBypackageId(id, CurrentLoggedInUser.CompanyId.Value);
            return PartialView();
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteSmartPackage(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                var smartPackage = _Util.Facade.SmartPackageFacade.GetSmartPackageById(id.Value);
                result = _Util.Facade.SmartPackageFacade.DeletePackage(id.Value);
                if (result)
                {
                    _Util.Facade.SmartPackageFacade.DeleteSmartPackageEquipmentServiceByPackageId(smartPackage.PackageId);
                }
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult LoadcompanySmartPackageInchudedSettings()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView("LoadcompanySmartPackageInchudedSettings");
        }

        [Authorize]
        public ActionResult CompanyPackageIncludeListPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SmartPackageEquipmentService> model = new List<SmartPackageEquipmentService>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService(CurrentUser.CompanyId.Value, "Include");
            }
            return PartialView("_CompanyPackageIncludeListPartial", model);
        }

        [Authorize]
        public ActionResult AddCompanyPackageIncludePartial(int? Id, string packageid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SmartPackageEquipmentService model = new SmartPackageEquipmentService();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (string.IsNullOrEmpty(packageid))
            {
                ViewBag.Check = 1;
            }
            #region PackageList in viewbag
            List<SelectListItem> PackageList = new List<SelectListItem>();
            PackageList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            PackageList.AddRange(ViewBag.PackageList = _Util.Facade.SmartPackageFacade
              .GetAllSmartPackageByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.PackageName.ToString(),
                                 Value = x.PackageId.ToString()
                             }).ToList());
            ViewBag.PackageList = PackageList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion
            #region EquipmentList In viewbag
            List<SelectListItem> EquipmentList = new List<SelectListItem>();
            EquipmentList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            if (Id.HasValue && Id > 0)
            {
                EquipmentList.AddRange(_Util.Facade.EquipmentFacade
             .GetAllEquipmentsForOptByCompanyId(CurrentUser.CompanyId.Value, Id.Value).Select(x =>
                            new SelectListItem()
                            {
                                Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]",
                                Value = x.EquipmentId.ToString()
                            }).ToList());
            }
            //EquipmentList.AddRange(ViewBag.EquipmentList = _Util.Facade.EquipmentFacade
            //  .GetAllEquipmentsByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
            //                 new SelectListItem()
            //                 {
            //                     Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]",
            //                     Value = x.EquipmentId.ToString()
            //                 }).ToList());
            Equipment equiments = null;
            if (Id.HasValue && Id > 0)
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService(CurrentUser.CompanyId.Value, "Include").Where(x => x.Id == Id).FirstOrDefault();
                equiments = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(model.EquipmentId, CurrentUser.CompanyId.Value);
                var equipClass = _Util.Facade.EquipmentFacade.GetAllEquipmentClassByCompanyId(CurrentUser.CompanyId.Value);
                if (equipClass.Count > 0)
                {
                    equiments.EquipmentClass = equipClass.Where(x => x.Id == equiments.EquipmentClassId).FirstOrDefault().Name;
                }
                else
                {
                    equiments.EquipmentClass = "";
                }
            }
            if (equiments != null)
            {
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
            }
            ViewBag.EquipmentList = EquipmentList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion

            return PartialView("_AddCompanyPackageIncludePartial", model);
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddCompanyPackageInclude(SmartPackageEquipmentService SmartPackageEquipmentService)
        {
            var result = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SmartPackageEquipmentService.CompanyId = CurrentUser.CompanyId.Value;
            SmartPackageEquipmentService.IsFree = true;
            if (CurrentUser == null)
            {
                return Json(result);
            }
            //SmartPackageEquipmentService _SmartPackageEquipmentService = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService()(SmartPackageEquipmentService.PackageId, CurrentUser.CompanyId.Value, _PackageInclude.EquipmentId);
            //if (_SmartPackageEquipmentService != null)
            //{
            //    return Json(result);
            //}
            if (SmartPackageEquipmentService.Id > 0)
            {
                var oldSmartPackageEquipmentService = _Util.Facade.SmartPackageFacade.GetSmartPackageEquipmentServiceById(SmartPackageEquipmentService.Id);
                if (oldSmartPackageEquipmentService != null)
                {
                    oldSmartPackageEquipmentService.PackageId = SmartPackageEquipmentService.PackageId;
                    oldSmartPackageEquipmentService.EquipmentId = SmartPackageEquipmentService.EquipmentId;
                    oldSmartPackageEquipmentService.EptNo = SmartPackageEquipmentService.EptNo;
                    oldSmartPackageEquipmentService.Status = false;
                    oldSmartPackageEquipmentService.LastUpdatedDate = DateTime.Now;
                    oldSmartPackageEquipmentService.LastUpdatedBy = CurrentUser.UserId;
                    result = _Util.Facade.SmartPackageFacade.UpdateSmartPackageEquipmentService(oldSmartPackageEquipmentService);
                }
            }
            else
            {
                SmartPackageEquipmentService.Type = "Include";
                SmartPackageEquipmentService.Status = false;
                SmartPackageEquipmentService.LastUpdatedDate = DateTime.Now;
                SmartPackageEquipmentService.LastUpdatedBy = CurrentUser.UserId;
                result = _Util.Facade.SmartPackageFacade.InsertSmartPackageEquipmentService(SmartPackageEquipmentService);
            }
            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddCompanySalesCommission(SalesComission SalesComission)
        {
            var result = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SalesComission.CompanyId = CurrentUser.CompanyId.Value;
            if (CurrentUser == null)
            {
                return Json(result);
            }

            if (SalesComission.Id > 0)
            {
                var oldSalesComission = _Util.Facade.SmartPackageFacade.GetAllSalesCommisionBypackageId(SalesComission.PackageServiceId, CurrentUser.CompanyId.Value).Where(x => x.Id == SalesComission.Id).FirstOrDefault();
                if (oldSalesComission != null)
                {
                    oldSalesComission.PackageServiceId = SalesComission.PackageServiceId;
                    oldSalesComission.SalesLocation = SalesComission.SalesLocation;
                    oldSalesComission.LeadType = SalesComission.LeadType;
                    if (SalesComission.SalesLocation == "Tech Install Pay")
                    {
                        oldSalesComission.LeadType = "";
                    }
                    oldSalesComission.AmoutParcent = SalesComission.AmoutParcent;
                    result = _Util.Facade.SmartPackageFacade.UpdateSalesCommission(oldSalesComission);
                }
            }
            else
            {
                SalesComission.CompanyId = CurrentUser.CompanyId.Value;
                if (SalesComission.SalesLocation == "Tech Install Pay")
                {
                    SalesComission.LeadType = "";
                }
                result = _Util.Facade.SmartPackageFacade.InsertSalesCommission(SalesComission);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeletePackageInclude(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.PackageFacade.DeletePackageInclude(id.Value);
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult LoadCompanySmartPackageDeviceSettings()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView();
        }

        [Authorize]
        public ActionResult AddCompanyPackageDevicePartial(int? Id, string packageid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SmartPackageEquipmentService model = new SmartPackageEquipmentService();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (string.IsNullOrEmpty(packageid))
            {
                ViewBag.Check = 1;
            }
            #region PackageList in viewbag
            List<SelectListItem> PackageList = new List<SelectListItem>();
            PackageList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            PackageList.AddRange(ViewBag.PackageList = _Util.Facade.SmartPackageFacade
                      .GetAllSmartPackageByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
                                     new SelectListItem()
                                     {
                                         Text = x.PackageName.ToString(),
                                         Value = x.PackageId.ToString()
                                     }).ToList());
            ViewBag.PackageList = PackageList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion
            #region EquipmentList In viewbag
            List<SelectListItem> EquipmentList = new List<SelectListItem>();
            EquipmentList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            if (Id.HasValue && Id > 0)
            {
                EquipmentList.AddRange(_Util.Facade.EquipmentFacade
          .GetAllEquipmentsForOptByCompanyId(CurrentUser.CompanyId.Value, Id.Value).Select(x =>
                         new SelectListItem()
                         {
                             Text = x.ManufacturerName.ToString() + " (" + x.Name.ToString() + ")",
                             Value = x.EquipmentId.ToString()
                         }).ToList());
            }
            //EquipmentList.AddRange(ViewBag.EquipmentList = _Util.Facade.EquipmentFacade
            //  .GetAllEquipmentsByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
            //                 new SelectListItem()
            //                 {
            //                     Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]",
            //                     Value = x.EquipmentId.ToString()
            //                 }).ToList());
            Equipment equiments = null;
            if (Id.HasValue && Id > 0)
            {
                //model = _Util.Facade.PackageFacade.GetPackageDeviceByIdAndCompanyId(Id.Value, CurrentUser.CompanyId.Value);
                model = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService(CurrentUser.CompanyId.Value, "Device").Where(x => x.Id == Id).FirstOrDefault();
                equiments = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(model.EquipmentId, CurrentUser.CompanyId.Value);
                var equipClass = _Util.Facade.EquipmentFacade.GetAllEquipmentClassByCompanyId(CurrentUser.CompanyId.Value);
                if (equipClass.Count > 0)
                {
                    equiments.EquipmentClass = equipClass.Where(x => x.Id == equiments.EquipmentClassId).FirstOrDefault().Name;
                }
                else
                {
                    equiments.EquipmentClass = "";
                }
            }
            if (equiments != null)
            {
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
            }
            ViewBag.EquipmentList = EquipmentList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion

            return PartialView("_AddCompanyPackageDevicePartial", model);
        }


        [Authorize]
        public ActionResult AddCompanySalesCommissionPartial(int? Id, string packageid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SalesComission model = new SalesComission();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #region PackageList in viewbag
            List<SelectListItem> PackageList = new List<SelectListItem>();
            ViewBag.PackId = packageid;
            PackageList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            PackageList.AddRange(ViewBag.PackageList = _Util.Facade.SmartPackageFacade
                      .GetAllSmartPackageByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
                                     new SelectListItem()
                                     {
                                         Text = x.PackageName.ToString(),
                                         Value = x.PackageId.ToString()
                                     }).ToList());
            ViewBag.PackageList = PackageList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion
            #region SalesLocation In viewbag
            List<SelectListItem> SalesLocationList = new List<SelectListItem>();
            SalesLocationList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            //if (packageid.HasValue && packageid > 0)
            //{
            SalesLocationList.AddRange(_Util.Facade.LookupFacade
      .GetLookupByKey("SalesLocation").Select(x =>
                     new SelectListItem()
                     {
                         Text = x.DisplayText,
                         Value = x.DataValue
                     }).ToList());
            ViewBag.SalesLocationList = SalesLocationList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion
            if (Id.HasValue && Id > 0)
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSalesCommisionBypackageId(new Guid(packageid), CurrentUser.CompanyId.Value).Where(x => x.Id == Id).FirstOrDefault();
            }
            return PartialView(model);
        }

        public ActionResult AddCompanyServicesSalesCommissionPartial(int? Id, string packageid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SalesComission model = new SalesComission();
            model.PackageServiceId = new Guid(packageid);
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #region PackageList in viewbag
            List<SelectListItem> PackageList = new List<SelectListItem>();
            ViewBag.PackId = packageid;
            ViewBag.EqpId = Id.Value;
            PackageList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            PackageList.AddRange(ViewBag.PackageList = _Util.Facade.SmartPackageFacade
                      .GetAllSmartPackageByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
                                     new SelectListItem()
                                     {
                                         Text = x.PackageName.ToString(),
                                         Value = x.PackageId.ToString()
                                     }).ToList());
            ViewBag.PackageList = PackageList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion
            #region SalesLocation In viewbag
            List<SelectListItem> SalesLocationList = new List<SelectListItem>();
            SalesLocationList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            //if (packageid.HasValue && packageid > 0)
            //{
            SalesLocationList.AddRange(_Util.Facade.LookupFacade
      .GetLookupByKey("SalesLocation").Select(x =>
                     new SelectListItem()
                     {
                         Text = x.DisplayText,
                         Value = x.DataValue
                     }).ToList());
            ViewBag.SalesLocationList = SalesLocationList;
            #endregion
            if (Id.HasValue && Id > 0)
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSalesCommisionBypackageId(new Guid(packageid), CurrentUser.CompanyId.Value).Where(x => x.Id == Id).FirstOrDefault();
            }
            return PartialView(model);
        }
        [HttpPost]
        [Authorize]
        public JsonResult AddCompanyPackageDevice(SmartPackageEquipmentService SmartPackageEquipmentService)
        {
            var result = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SmartPackageEquipmentService.CompanyId = CurrentUser.CompanyId.Value;
            SmartPackageEquipmentService.IsFree = true;
            if (CurrentUser == null)
            {
                return Json(result);
            }
            //SmartPackageEquipmentService _SmartPackageEquipmentService = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService()(SmartPackageEquipmentService.PackageId, CurrentUser.CompanyId.Value, _PackageInclude.EquipmentId);
            //if (_SmartPackageEquipmentService != null)
            //{
            //    return Json(result);
            //}
            if (SmartPackageEquipmentService.Id > 0)
            {
                var oldSmartPackageEquipmentService = _Util.Facade.SmartPackageFacade.GetSmartPackageEquipmentServiceById(SmartPackageEquipmentService.Id);
                if (oldSmartPackageEquipmentService != null)
                {
                    oldSmartPackageEquipmentService.PackageId = SmartPackageEquipmentService.PackageId;
                    oldSmartPackageEquipmentService.EquipmentId = SmartPackageEquipmentService.EquipmentId;
                    oldSmartPackageEquipmentService.EptNo = SmartPackageEquipmentService.EptNo;
                    oldSmartPackageEquipmentService.Status = false;
                    oldSmartPackageEquipmentService.LastUpdatedDate = DateTime.Now;
                    oldSmartPackageEquipmentService.LastUpdatedBy = CurrentUser.UserId;
                    result = _Util.Facade.SmartPackageFacade.UpdateSmartPackageEquipmentService(oldSmartPackageEquipmentService);
                }
            }
            else
            {
                SmartPackageEquipmentService.Type = "Device";
                SmartPackageEquipmentService.Status = false;
                SmartPackageEquipmentService.LastUpdatedDate = DateTime.Now;
                SmartPackageEquipmentService.LastUpdatedBy = CurrentUser.UserId;
                result = _Util.Facade.SmartPackageFacade.InsertSmartPackageEquipmentService(SmartPackageEquipmentService);
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult CompanyPackageDeviceListPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SmartPackageEquipmentService> model = new List<SmartPackageEquipmentService>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService(CurrentUser.CompanyId.Value, "Device");
            }
            return PartialView("_CompanyPackageDeviceListPartial", model);
        }


        [Authorize]
        [HttpPost]
        public JsonResult DeleteSystemType(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.SmartPackageFacade.DeleteSystemType(id.Value);
                if (result)
                {
                    _Util.Facade.SmartPackageFacade.DeleteSmartPackageSystemInstallTypeMapBySystemId(id.Value);
                }
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteInstallType(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.SmartPackageFacade.DeleteInstallType(id.Value);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteSystemInstallType(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.SmartPackageFacade.DeleteSystemInstallType(id.Value);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeletePackageDevice(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.PackageFacade.DeletePackageDevice(id.Value);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteSmartPackageEquipmentService(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.SmartPackageFacade.DeleteSmartPackageEquipmentService(id.Value);
            }
            return Json(result);
        }


        [Authorize]
        public ActionResult LoadCompanySmartPackageServicesSettings()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView();
        }

        [Authorize]
        public ActionResult AddCompanyPackageServicesPartial(int? Id, string packageid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SmartPackageEquipmentService model = new SmartPackageEquipmentService();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (string.IsNullOrEmpty(packageid))
            {
                ViewBag.Check = 1;
            }
            #region PackageList in viewbag
            List<SelectListItem> PackageList = new List<SelectListItem>();
            PackageList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            PackageList.AddRange(ViewBag.PackageList = _Util.Facade.SmartPackageFacade
            .GetAllSmartPackageByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.PackageName.ToString(),
                               Value = x.PackageId.ToString()
                           }).ToList());
            ViewBag.PackageList = PackageList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion
            #region EquipmentList In viewbag
            List<SelectListItem> EquipmentList = new List<SelectListItem>();
            EquipmentList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            if (Id.HasValue && Id > 0)
            {
                EquipmentList.AddRange(_Util.Facade.EquipmentFacade
           .GetAllEquipmentsForOptByCompanyId(CurrentUser.CompanyId.Value, Id.Value).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]",
                              Value = x.EquipmentId.ToString()
                          }).ToList());
            }
            Equipment equiments = null;
            if (Id.HasValue && Id > 0)
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService(CurrentUser.CompanyId.Value, "Service").Where(x => x.Id == Id.Value).FirstOrDefault();
                model.ServiceEquipments = _Util.Facade.SmartPackageFacade.GetSmartPackageEquipmentServiceEquipmentBySmartPackageEquipmentServiceId(model.SmartPackageEquipmentServiceId);

                equiments = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(model.EquipmentId, CurrentUser.CompanyId.Value);
                var equipClass = _Util.Facade.EquipmentFacade.GetAllEquipmentClassByCompanyId(CurrentUser.CompanyId.Value);
                if (equipClass.Count > 0)
                {
                    equiments.EquipmentClass = equipClass.Where(x => x.Id == equiments.EquipmentClassId).FirstOrDefault().Name;
                }
                else
                {
                    equiments.EquipmentClass = "";
                }
            }
            if (equiments != null)
            {
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
            }
            ViewBag.EquipmentList = EquipmentList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion

            return PartialView(model);
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddCompanyPackageServices(SmartPackageEquipmentService SmartPackageEquipmentService)
        {
            var result = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SmartPackageEquipmentService.CompanyId = CurrentUser.CompanyId.Value;
            SmartPackageEquipmentService.IsFree = true;
            if (CurrentUser == null)
            {
                return Json(result);
            }
            //SmartPackageEquipmentService _SmartPackageEquipmentService = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService()(SmartPackageEquipmentService.PackageId, CurrentUser.CompanyId.Value, _PackageInclude.EquipmentId);
            //if (_SmartPackageEquipmentService != null)
            //{
            //    return Json(result);
            //}
            if (SmartPackageEquipmentService.Id > 0)
            {
                var oldSmartPackageEquipmentService = _Util.Facade.SmartPackageFacade.GetSmartPackageEquipmentServiceById(SmartPackageEquipmentService.Id);
                if (oldSmartPackageEquipmentService != null)
                {
                    oldSmartPackageEquipmentService.PackageId = SmartPackageEquipmentService.PackageId;
                    oldSmartPackageEquipmentService.EquipmentId = SmartPackageEquipmentService.EquipmentId;
                    oldSmartPackageEquipmentService.Price = SmartPackageEquipmentService.Price;
                    oldSmartPackageEquipmentService.OriginalPrice = SmartPackageEquipmentService.OriginalPrice;
                    oldSmartPackageEquipmentService.LastUpdatedBy = CurrentUser.UserId;
                    oldSmartPackageEquipmentService.LastUpdatedDate = DateTime.Now;
                    oldSmartPackageEquipmentService.Status = true;
                    oldSmartPackageEquipmentService.EptNo = SmartPackageEquipmentService.EptNo;

                    oldSmartPackageEquipmentService.ServiceEquipments = SmartPackageEquipmentService.ServiceEquipments;
                    result = _Util.Facade.SmartPackageFacade.UpdateSmartPackageEquipmentService(oldSmartPackageEquipmentService);

                    SmartPackageEquipmentService = oldSmartPackageEquipmentService;
                }
            }
            else
            {
                SmartPackageEquipmentService.Type = "Service";
                SmartPackageEquipmentService.LastUpdatedBy = CurrentUser.UserId;
                SmartPackageEquipmentService.LastUpdatedDate = DateTime.Now;
                SmartPackageEquipmentService.Status = true;
                SmartPackageEquipmentService.SmartPackageEquipmentServiceId = Guid.NewGuid();

                result = _Util.Facade.SmartPackageFacade.InsertSmartPackageEquipmentService(SmartPackageEquipmentService);
            }
            _Util.Facade.SmartPackageFacade.DeleteSmartPackageEquipmentServiceBySmartPackageEquipmentServiceId(SmartPackageEquipmentService.SmartPackageEquipmentServiceId);

            if (SmartPackageEquipmentService.ServiceEquipments != null && SmartPackageEquipmentService.ServiceEquipments.Count() > 0)
            {
                foreach (var item in SmartPackageEquipmentService.ServiceEquipments)
                {
                    if (item.EquipmentId != Guid.Empty && item.Quantity > 0)
                    {
                        item.SmartPackageEquipmentServiceId = SmartPackageEquipmentService.SmartPackageEquipmentServiceId;
                        _Util.Facade.SmartPackageFacade.InsertSmartPackageEquipmentServiceEquipment(item);
                    }
                }
            }
            return Json(result);
        }


        [Authorize]
        public ActionResult CompanyPackageServicesListPartial(int pageno, int pagesize, string status)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            PackageEquipmentServiceModel model = new PackageEquipmentServiceModel();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentServiceListByTypeAndCompanyId(CurrentUser.CompanyId.Value, "Service", pageno, pagesize, status);
                if (pageno == 0)
                {
                    pageno = 1;
                }
                if (model.ListSmartPackageEquipmentService.Count() == 0)
                {
                    pageno = 1;
                }

                ViewBag.PageNumber = pageno;
                ViewBag.OutOfNumber = 0;


                if (model.ListSmartPackageEquipmentService.Count() > 0)
                {
                    ViewBag.OutOfNumber = model.PackageEquipmentServiceTotalCountModel.TotalCount;
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
            }
            ViewBag.status = status;
            return PartialView(model);
        }

        public JsonResult LoadEquipmentAndService(int id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SelectListItem> equipmentList = new List<SelectListItem>();
            //equipmentList.Add(new SelectListItem()
            //{
            //    Text = "Please Select",
            //    Value = "-1"
            //});
            equipmentList.AddRange(_Util.Facade.EquipmentFacade
              .GetAllEquipmentsForOptByCompanyId(CurrentUser.CompanyId.Value, id).Where(x => x.Name != "").Select(x =>
                               new SelectListItem()
                               {
                                   Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]",
                                   Value = x.EquipmentId.ToString()
                               }).ToList());
            return Json(equipmentList);
        }

        [Authorize]
        public ActionResult LoadCompanySmartPackageOptionalSettings()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView();
        }

        [Authorize]
        public ActionResult AddCompanyPackageOptionalPartial(int? Id, string packageid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SmartPackageEquipmentService model = new SmartPackageEquipmentService();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (string.IsNullOrEmpty(packageid))
            {
                ViewBag.Check = 1;
            }
            #region PackageList in viewbag
            List<SelectListItem> PackageList = new List<SelectListItem>();
            PackageList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            PackageList.AddRange(ViewBag.PackageList = _Util.Facade.SmartPackageFacade
            .GetAllSmartPackageByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.PackageName.ToString(),
                               Value = x.PackageId.ToString()
                           }).ToList());
            ViewBag.PackageList = PackageList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion
            #region EquipmentList In viewbag
            List<SelectListItem> EquipmentList = new List<SelectListItem>();
            EquipmentList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            //EquipmentList.AddRange(ViewBag.EquipmentList = _Util.Facade.EquipmentFacade
            //  .GetAllEquipmentsForOptByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
            //                 new SelectListItem()
            //                 {
            //                     Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]",
            //                     Value = x.EquipmentId.ToString()
            //                 }).ToList());
            if (Id.HasValue && Id > 0)
            {
                EquipmentList.AddRange(_Util.Facade.EquipmentFacade
           .GetAllEquipmentsForOptByCompanyId(CurrentUser.CompanyId.Value, Id.Value).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]",
                              Value = x.EquipmentId.ToString()
                          }).ToList());
            }
            Equipment equiments = null;
            if (Id.HasValue && Id > 0)
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService(CurrentUser.CompanyId.Value, "Optional").Where(x => x.Id == Id.Value).FirstOrDefault();
                equiments = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(model.EquipmentId, CurrentUser.CompanyId.Value);
                var equipClass = _Util.Facade.EquipmentFacade.GetAllEquipmentClassByCompanyId(CurrentUser.CompanyId.Value);
                if (equipClass.Count > 0)
                {
                    equiments.EquipmentClass = equipClass.Where(x => x.Id == equiments.EquipmentClassId).FirstOrDefault().Name;
                }
                else
                {
                    equiments.EquipmentClass = "";
                }
            }
            if (equiments != null)
            {
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
            }
            ViewBag.EquipmentList = EquipmentList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion

            return PartialView("_AddCompanyPackageOptionalPartial", model);
        }

        public ActionResult LoadEquipmentAndServiceSearch(Guid id, int equipmentClassId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string query = Request.QueryString["q[term]"] != null ? Request.QueryString["q[term]"].ToString() : "";
            var equipmentList = _Util.Facade.EquipmentFacade
              .GetAllEquipmentsForOptByCompanyId(CurrentUser.CompanyId.Value, id, query, equipmentClassId);
            List<Dictionary<string, object>>
             lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in equipmentList.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in equipmentList.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }

            return Json(lstRows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadTechEquipmentAndServiceSearch(Guid id, int equipmentClassId, Guid techid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string query = Request.QueryString["q[term]"] != null ? Request.QueryString["q[term]"].ToString() : "";
            var equipmentList = _Util.Facade.EquipmentFacade
              .GetAllTechEquipmentsForOptByCompanyId(CurrentUser.CompanyId.Value, id, query, equipmentClassId, techid);
            List<Dictionary<string, object>>
             lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in equipmentList.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in equipmentList.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }

            return Json(lstRows, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddCompanyPackageOptional(SmartPackageEquipmentService SmartPackageEquipmentService)
        {
            var result = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SmartPackageEquipmentService.CompanyId = CurrentUser.CompanyId.Value;
            SmartPackageEquipmentService.IsFree = false;
            if (CurrentUser == null)
            {
                return Json(result);
            }
            //SmartPackageEquipmentService _SmartPackageEquipmentService = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService()(SmartPackageEquipmentService.PackageId, CurrentUser.CompanyId.Value, _PackageInclude.EquipmentId);
            //if (_SmartPackageEquipmentService != null)
            //{
            //    return Json(result);
            //}
            if (SmartPackageEquipmentService.Id > 0)
            {
                var oldSmartPackageEquipmentService = _Util.Facade.SmartPackageFacade.GetSmartPackageEquipmentServiceById(SmartPackageEquipmentService.Id);
                if (oldSmartPackageEquipmentService != null)
                {
                    oldSmartPackageEquipmentService.PackageId = SmartPackageEquipmentService.PackageId;
                    oldSmartPackageEquipmentService.EquipmentId = SmartPackageEquipmentService.EquipmentId;
                    oldSmartPackageEquipmentService.EptNo = SmartPackageEquipmentService.EptNo;
                    oldSmartPackageEquipmentService.Status = false;
                    oldSmartPackageEquipmentService.LastUpdatedDate = DateTime.Now;
                    oldSmartPackageEquipmentService.LastUpdatedBy = CurrentUser.UserId;
                    result = _Util.Facade.SmartPackageFacade.UpdateSmartPackageEquipmentService(oldSmartPackageEquipmentService);
                }
            }
            else
            {
                SmartPackageEquipmentService.Type = "Optional";
                SmartPackageEquipmentService.Status = false;
                SmartPackageEquipmentService.LastUpdatedDate = DateTime.Now;
                SmartPackageEquipmentService.LastUpdatedBy = CurrentUser.UserId;
                result = _Util.Facade.SmartPackageFacade.InsertSmartPackageEquipmentService(SmartPackageEquipmentService);
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult CompanyPackageOptionalListPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SmartPackageEquipmentService> model = new List<SmartPackageEquipmentService>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSmartPackageEquipmentService(CurrentUser.CompanyId.Value, "Optional");
            }
            return PartialView("_CompanyPackageOptionalListPartial", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeletePackageOptional(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.PackageFacade.DeletePackageOptional(id.Value);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteSalesCommission(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.SmartPackageFacade.DeleteSalesCommission(id.Value);
            }
            return Json(result);
        }
        [Authorize]

        public ActionResult LoadcompanySmartSystemTypeSettings()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView();
        }
        public ActionResult LoadcompanySmartPackageComission()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView();
        }
        public ActionResult AddCompanyPackageSmartSystemPartial(int? Id, int? packageid)
        {
            if (Id != null && Id > 0)
            {
                SmartSystemType smartSystemType = _Util.Facade.SmartPackageFacade.GetSmartSystemTypeById(Id.Value);
                return PartialView(smartSystemType);
            }
            else
            {
                SmartSystemType model = new SmartSystemType();
                return PartialView(model);
            }
        }

        [Authorize]
        public ActionResult CompanyPackageSmartSystemListPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SmartSystemType> model = new List<SmartSystemType>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSmartSystemType(CurrentUser.CompanyId.Value);
            }
            return PartialView(model);
        }
        [Authorize]
        public ActionResult CompanyPackageComission()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<PackageCommission> model = new List<PackageCommission>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }


            model = _Util.Facade.SmartPackageFacade.GetAllPackageComission();

            return PartialView(model);
        }


        public JsonResult AddCompanySmartSystemType(SmartSystemType SmartSystemType)
        {
            var result = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SmartSystemType.CompanyId = CurrentUser.CompanyId.Value;
            if (CurrentUser == null)
            {
                return Json(result);
            }
            if (SmartSystemType.Id > 0)
            {
                var OldPackage = _Util.Facade.SmartPackageFacade.GetSmartSystemTypeById(SmartSystemType.Id);
                if (OldPackage != null)
                {
                    OldPackage.Name = SmartSystemType.Name;
                    OldPackage.CompanyId = SmartSystemType.CompanyId;
                    result = _Util.Facade.SmartPackageFacade.UpdateSystemType(OldPackage);
                }
            }
            else
            {
                result = _Util.Facade.SmartPackageFacade.InsertSystemType(SmartSystemType);
            }
            return Json(result);
        }
        [Authorize]
        public ActionResult AddCompanyPackageComission(int? Id, int? packageid)
        {
            #region Viewbags
            List<SelectListItem> PackageType = new List<SelectListItem>();
            PackageType.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("PackageType").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            List<SelectListItem> CommissionType = new List<SelectListItem>();
            CommissionType.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("CommissionType").OrderBy(x => x.DisplayText != "Please Select").ThenBy(x => x.DisplayText).Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            List<SelectListItem> LeadType = new List<SelectListItem>();
            LeadType.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("CustomerType").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            List<SelectListItem> Type = new List<SelectListItem>();
            Type.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("Type").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.PackageType = PackageType;
            ViewBag.LeadType = LeadType;
            ViewBag.CommissionType = CommissionType;
            ViewBag.Type = Type;
            #endregion
            if (Id != null && Id > 0)
            {
                PackageCommission packageCom = _Util.Facade.SmartPackageFacade.GetPackageCommissionById(Id.Value);
                return PartialView(packageCom);
            }
            else
            {
                PackageCommission model = new PackageCommission();
                return PartialView(model);
            }
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddCompanyPackageComission(PackageCommission packageCommission)
        {
            var result = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (CurrentUser == null)
            {
                return Json(result);
            }
            if (packageCommission.Id > 0)
            {
                var OldPackageCom = _Util.Facade.SmartPackageFacade.GetPackageCommissionById(packageCommission.Id);
                if (OldPackageCom != null)
                {
                    OldPackageCom.Type = packageCommission.Type;
                    OldPackageCom.LeadType = packageCommission.LeadType;

                    OldPackageCom.PackageType = packageCommission.PackageType;
                    OldPackageCom.Commission = packageCommission.Commission;
                    OldPackageCom.CommissionType = packageCommission.CommissionType;

                    OldPackageCom.LastUpdatedBy = CurrentUser.UserId;
                    OldPackageCom.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    OldPackageCom.CreatedBy = OldPackageCom.CreatedBy;
                    OldPackageCom.CreatedDate = OldPackageCom.CreatedDate;
                    OldPackageCom.CommissionType = packageCommission.CommissionType;
                    result = _Util.Facade.SmartPackageFacade.UpdatePackageCommission(OldPackageCom);
                }
            }
            else
            {

                packageCommission.PackageCommissionId = Guid.NewGuid();
                packageCommission.CreatedBy = CurrentUser.UserId;
                packageCommission.LastUpdatedBy = CurrentUser.UserId;
                packageCommission.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                packageCommission.CreatedDate = DateTime.Now.UTCCurrentTime();


                result = _Util.Facade.SmartPackageFacade.InsertPackageCommission(packageCommission);
            }

            return Json(result);
        }
        public ActionResult LoadCompanySmartInstallTypeSettings()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView();
        }
        [Authorize]
        public ActionResult CompanyPackageInstallTypeListPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SmartInstallType> model = new List<SmartInstallType>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSmartInstallType(CurrentUser.CompanyId.Value);
            }
            return PartialView(model);
        }

        public ActionResult AddCompanyPackageSmartInstallTypePartial(int? Id, int? packageid)
        {
            if (Id != null && Id > 0)
            {
                SmartInstallType SmartInstallType = _Util.Facade.SmartPackageFacade.GetSmartInstallTypeByID(Id.Value);
                return PartialView(SmartInstallType);
            }
            else
            {
                SmartInstallType model = new SmartInstallType();
                return PartialView(model);
            }
        }

        [Authorize]
        public JsonResult AddCompanySmartInstallType(SmartInstallType SmartInstallType)
        {
            var result = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SmartInstallType.CompanyId = CurrentUser.CompanyId.Value;
            if (CurrentUser == null)
            {
                return Json(result);
            }
            if (SmartInstallType.Id > 0)
            {
                var OldPackage = _Util.Facade.SmartPackageFacade.GetSmartInstallTypeByID(SmartInstallType.Id);
                if (OldPackage != null)
                {
                    OldPackage.Name = SmartInstallType.Name;
                    OldPackage.CompanyId = SmartInstallType.CompanyId;
                    result = _Util.Facade.SmartPackageFacade.UpdateInstallType(OldPackage);
                }
            }
            else
            {
                result = _Util.Facade.SmartPackageFacade.InsertInstallType(SmartInstallType);
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult LoadMapSystemInstallTypeSettings()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView();
        }

        [Authorize]
        public ActionResult AddCompanyPackageMapSmartSystemPartial(int? Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var CompanyId = CurrentUser.CompanyId.Value;
            if (Id != null && Id > 0)
            {
                SmartSystemInstallType SmartSystemInstallType = _Util.Facade.SmartPackageFacade.GetAllSmartSystemInstallTypeByCompanyId(CompanyId).Where(x => x.SystemId == Id.Value).FirstOrDefault();
                return PartialView(SmartSystemInstallType);
            }
            else
            {
                List<SelectListItem> SmartSystemTypeList = new List<SelectListItem>();
                SmartSystemTypeList.Add(new SelectListItem()
                {
                    Text = "Please Select",
                    Value = "-1"
                });
                SmartSystemTypeList.AddRange(ViewBag.PackageList = _Util.Facade.SmartPackageFacade
           .GetAllSmartSystemType(CurrentUser.CompanyId.Value).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.Name.ToString(),
                              Value = x.Id.ToString()
                          }).ToList());
                ViewBag.SmartSystemTypeList = SmartSystemTypeList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();

                List<SelectListItem> SmartInstallTypeList = new List<SelectListItem>();
                SmartInstallTypeList.Add(new SelectListItem()
                {
                    Text = "Please Select",
                    Value = "-1"
                });
                SmartInstallTypeList.AddRange(ViewBag.PackageList = _Util.Facade.SmartPackageFacade
          .GetAllSmartInstallType(CurrentUser.CompanyId.Value).Select(x =>
                         new SelectListItem()
                         {
                             Text = x.Name.ToString(),
                             Value = x.Id.ToString()
                         }).ToList());
                ViewBag.SmartInstallTypeList = SmartInstallTypeList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
                SmartSystemInstallType model = new SmartSystemInstallType();
                return PartialView(model);
            }
        }

        [Authorize]
        public ActionResult AddCompanyPackageMapSmartSystem(SmartSystemInstallType smartSystemInstallType)
        {
            var result = false;
            bool found = false;
            string message = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            smartSystemInstallType.CompanyId = CurrentUser.CompanyId.Value;
            List<SmartSystemInstallType> smartTypeList = _Util.Facade.SmartPackageFacade.GetAllSmartSystemInstallTypeByCompanyId(CurrentUser.CompanyId.Value);
            if (CurrentUser == null)
            {
                return Json(result);
            }
            else
            {
                SmartSystemInstallType smrttype = smartTypeList.Where(x => x.InstallTypeId == smartSystemInstallType.InstallTypeId && x.SystemId == smartSystemInstallType.SystemId).FirstOrDefault();

                if (smrttype == null)
                {
                    result = _Util.Facade.SmartPackageFacade.InsertSystemInstallType(smartSystemInstallType);
                    message = "System type and install type mapped successfully.";
                    result = true;
                }
                else
                {
                    message = "This system type and install type map have already added.";
                    result = false;
                }

            }
            return Json(new { result = result, message = message });
        }
        [Authorize]
        public ActionResult CompanyPackageMapSmartSystemInstallListPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SmartSystemInstallType> model = new List<SmartSystemInstallType>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSmartSystemInstallTypeByCompanyId(CurrentUser.CompanyId.Value);
            }
            return PartialView(model);
        }

        #region type manufacturer
        [Authorize]
        public ActionResult LoadMapTypeManufacturer()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView();
        }
        [Authorize]
        public ActionResult LoadMapTypeManufacturerPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SystemTypeManufacturerMap> model = new List<SystemTypeManufacturerMap>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSystemTypeManufacturerMap();
            }
            return PartialView(model);
        }
        [Authorize]
        public ActionResult AddMapTypeManufacturer(int? Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var CompanyId = CurrentUser.CompanyId.Value;
            SystemTypeManufacturerMap model = new SystemTypeManufacturerMap();
            if (Id != null && Id > 0)
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSystemTypeManufacturerMap().Where(x => x.SystemId == Id.Value).FirstOrDefault();
            }
            #region viewbag
            List<SelectListItem> SmartSystemTypeList = new List<SelectListItem>();
            SmartSystemTypeList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            SmartSystemTypeList.AddRange(ViewBag.PackageList = _Util.Facade.SmartPackageFacade
       .GetAllSmartSystemType(CurrentUser.CompanyId.Value).Select(x =>
                      new SelectListItem()
                      {
                          Text = x.Name.ToString(),
                          Value = x.Id.ToString()
                      }).ToList());
            ViewBag.SmartSystemTypeList = SmartSystemTypeList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();

            List<SelectListItem> ManufacturerList = new List<SelectListItem>();
            ManufacturerList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            ManufacturerList.AddRange(_Util.Facade.ManufacturerFacade.GetAllManufacturerByCompanyIdBasePackage(CurrentUser.CompanyId.Value).Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.ManufacturerId.ToString()
                }).ToList());
            ViewBag.ManufacturerList = ManufacturerList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion
            return PartialView(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddMapTypeManufacturer(SystemTypeManufacturerMap systemTypeManufacturerMap)
        {
            var result = false;
            string message = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SystemTypeManufacturerMap> smartTypeList = _Util.Facade.SmartPackageFacade.GetAllSystemTypeManufacturerMap();
            if (CurrentUser == null)
            {
                return Json(result);
            }
            else
            {
                SystemTypeManufacturerMap smrttype = smartTypeList.Where(x => x.ManufacturerId == systemTypeManufacturerMap.ManufacturerId && x.SystemId == systemTypeManufacturerMap.SystemId).FirstOrDefault();

                if (smrttype == null)
                {
                    result = _Util.Facade.SmartPackageFacade.InsertSystemTypeManufacturerMap(systemTypeManufacturerMap);
                    message = "System type and manufacturer mapped successfully.";
                    result = true;
                }
                else
                {
                    message = "This system type and manufacturer map have already added.";
                    result = false;
                }

            }
            return Json(new { result = result, message = message });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteSystemTypeManufacturerMap(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.SmartPackageFacade.DeleteSystemTypeManufacturerMap(id.Value);
            }
            return Json(result);
        }
        #endregion
        #region type service
        [Authorize]
        public ActionResult LoadMapTypeService()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView();
        }
        [Authorize]
        public ActionResult LoadMapTypeServicePartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SystemTypeServiceMap> model = new List<SystemTypeServiceMap>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSystemTypeServiceMap();
            }
            return PartialView(model);
        }
        [Authorize]
        public ActionResult AddMapTypeService(int? Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var CompanyId = CurrentUser.CompanyId.Value;
            SystemTypeServiceMap model = new SystemTypeServiceMap();
            if (Id != null && Id > 0)
            {
                model = _Util.Facade.SmartPackageFacade.GetAllSystemTypeServiceMap().Where(x => x.SystemTypeId == Id.Value).FirstOrDefault();
            }
            #region viewbag
            List<SelectListItem> SmartSystemTypeList = new List<SelectListItem>();
            SmartSystemTypeList.AddRange(ViewBag.PackageList = _Util.Facade.SmartPackageFacade
       .GetAllSmartSystemType(CurrentUser.CompanyId.Value).Select(x =>
                      new SelectListItem()
                      {
                          Text = x.Name.ToString(),
                          Value = x.Id.ToString()
                      }).ToList());
            ViewBag.SmartSystemTypeList = SmartSystemTypeList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();

            List<SelectListItem> PackageList = new List<SelectListItem>();
            PackageList.AddRange(ViewBag.PackageList = _Util.Facade.SmartPackageFacade
              .GetAllSmartPackageByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
                             new SelectListItem()
                             {
                                 Text = (!string.IsNullOrEmpty(x.PackageCode) ? x.PackageCode + "->" : "") + x.PackageName.ToString(),
                                 Value = x.PackageId.ToString()
                             }).ToList());
            ViewBag.PackageList = PackageList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();

            List<SelectListItem> ServiceList = new List<SelectListItem>();
            ServiceList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            ServiceList.AddRange(_Util.Facade.EquipmentFacade.GetAllService().Select(x =>
                new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.EquipmentId.ToString()
                }).ToList());
            ViewBag.ServiceList = ServiceList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion
            return PartialView(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddMapTypeService(SystemTypeServiceMap systemTypeServiceMap)
        {
            var result = false;
            string message = "";
            bool isMapped = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SystemTypeServiceMap> smartTypeList = _Util.Facade.SmartPackageFacade.GetAllSystemTypeServiceMap();
            if (CurrentUser == null)
            {
                return Json(result);
            }
            else
            {
                foreach (var packageId in systemTypeServiceMap.PackageIdList)
                {
                    foreach (var equipmentId in systemTypeServiceMap.EquipmentIdList)
                    {
                        SystemTypeServiceMap smrttype = smartTypeList.Where(x => x.EquipmentId == equipmentId && x.SystemTypeId == systemTypeServiceMap.SystemTypeId && x.PackageId == packageId).FirstOrDefault();
                        if (smrttype == null)
                        {
                            systemTypeServiceMap.PackageId = packageId;
                            systemTypeServiceMap.EquipmentId = equipmentId;
                            result = _Util.Facade.SmartPackageFacade.InsertSystemTypeServiceMap(systemTypeServiceMap);
                            message = "Mapped successfully.";
                            result = true;
                            isMapped = true;
                        }
                    }
                }
            }
            if(isMapped == false)
            {
                result = true;
                message = "Already Mapped.";
            }
            return Json(new { result = result, message = message });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteSystemTypeServiceMap(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.SmartPackageFacade.DeleteSystemTypeServiceMap(id.Value);
            }
            return Json(result);
        }
        #endregion
        public JsonResult LoadInstallType(int id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SelectListItem> InstallTypeList = new List<SelectListItem>();
            InstallTypeList.AddRange(_Util.Facade.SmartPackageFacade
              .LoadInstallType(id, CurrentUser.CompanyId.Value).Where(x => x.Name != "").Select(x =>
                               new SelectListItem()
                               {
                                   Text = x.Name.ToString(),
                                   Value = x.Id.ToString()
                               }).ToList());
            return Json(InstallTypeList);
        }

    }
}