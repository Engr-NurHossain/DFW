using HS.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using HS.Framework;
using System.Web.Mvc;
using MyExcel = ClosedXML.Excel;
using NLog;

namespace HS.Web.UI.Controllers
{
    public class CityTaxController : BaseController
    {
        public CityTaxController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        // GET: CityTax
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

        public ActionResult CityTaxPartial()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<CityTax> CityTax = _Util.Facade.CityTaxFacade.GetAllCityTaxByCompanyId(currentLoggedIn.CompanyId.Value);
            return PartialView("CityTaxPartial", CityTax);
        }

        [Authorize]
        public ActionResult AddCityTax(int? id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            CityTax model;
            if (id.HasValue)
            {
                model = _Util.Facade.CityTaxFacade.GetCityTaxById(id.Value);
            }
            else
            {
                model = new CityTax();
            }
            ViewBag.StateList = _Util.Facade.LookupFacade.GetLookupByKey("StateList").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
            return PartialView("AddCityTax", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddCityTax(CityTax ct)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ct.CompanyId = currentLoggedIn.CompanyId.Value;
            ct.IsActive = true;
            if (ct.Id > 0)
            {
                _Util.Facade.CityTaxFacade.UpdateCityTax(ct);
            }
            else
            {
                _Util.Facade.CityTaxFacade.InsertCityTax(ct);
            }
            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteCityTax(int? id)
        {
            if (id.HasValue)
            {
                var Citytaxval = _Util.Facade.CityTaxFacade.DeleteCityTax(id.Value);
            }

            return Json(true);
        }
        public ActionResult RestrictedZipCode()
        {
            return PartialView("_RestrictedZipCode");
        }
        public ActionResult LoadRestrictedZipCode(int pageno, int pagesize, string searchtext)
        {


            RestrictedZipCode restrictedZipCodes = _Util.Facade.CityTaxFacade.GetRestrictedZipCode(pageno, pagesize, searchtext);
            ViewBag.PageNumber = pageno;
            ViewBag.OutOfNumber = 0;
            TempData["data"] = searchtext;
          
            if (restrictedZipCodes.TotalCount > 0)
            {
                ViewBag.OutOfNumber = restrictedZipCodes.TotalCount;
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
            return PartialView("_LoadRestrictedZipCode", restrictedZipCodes);
        }

        [Authorize]
        public ActionResult AddRestricterdZipCodeFile()
        {
            return View();
        }

        [Authorize]
        public JsonResult RestricterdZipCodeImport(string File)
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
                    RestrictedZipCode reszipcode = new RestrictedZipCode();
                    List<RestrictedZipCode> reszipcodeList = _Util.Facade.CityTaxFacade.GetAllRestrictedZipCode();
                    //List<Supplier> supplierList = _Util.Facade.SupplierFacade.GetAllSupplierByCompanyId(CurrentUser.CompanyId.Value);
                    //List<EquipmentVendor> EquipmentVendorList = _Util.Facade.EquipmentFacade.GetAllEquipmentVendor();
                    for (int i = 2; i <= rowCount; i++)
                    {
                        try
                        {
                            //var companycost = 0.0;
                            //Guid vendorid = new Guid();
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
                                        else if (header.ToLower() == "zipcode" || header.ToLower()=="zip code" || header.ToLower()=="restrictedzipcode" ||header.ToLower()== "restricted zip code")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                RestrictedZipCode _resZip = reszipcodeList.Where(x => x.Zipcode == value).FirstOrDefault();
                                                if(_resZip!=null)
                                                {
                                                    reszipcode.Zipcode = "";
                                                    break;
                                                }
                                                else
                                                {
                                                    reszipcode.Zipcode = value;
                                                }
                                                
                                            }
                                            else
                                            {
                                                reszipcode.Zipcode = "";
                                                break;
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
                            if (reszipcode != null && reszipcode.Zipcode!="")
                            {
                                #region Restricted zipcode Insert
                                RestrictedZipCode res = new RestrictedZipCode()
                                {
                                    Zipcode = reszipcode.Zipcode,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    CreatedBy = CurrentUser.FirstName + " " + CurrentUser.LastName
                                };
                                _Util.Facade.CityTaxFacade.InsertRestrictedZipCode(res);
                                #endregion
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

        [Authorize]
        [HttpPost]
        public JsonResult DeleteRestrictedZipCode(int Id)
        {
            var res = _Util.Facade.CityTaxFacade.DeleteRestrictedZipCode(Id);
            if (res > 0)
            {
                return Json(new { result = res, message = "Restricted ZipCode deleted successfully." });
            }
            else
            {
                return Json(new { result = res, message = "An error occured." });
            }

        }
        public ActionResult AddRestrictedZipCode()
        {


            return View("_AddRestrictedZipCode");
        }
        public JsonResult SaveRestrictedZipCode(string zipcode)
        {
            var result = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;


            if (CurrentUser == null || String.IsNullOrWhiteSpace(zipcode))
            {
                return Json(result);
            }

            RestrictedZipCode restrictedZipCodes = _Util.Facade.CityTaxFacade.GetRestrictedZipcodelistbyzipcode(zipcode);

            if (restrictedZipCodes.TotalCount > 0)
            {
                return Json(result);

            }
            RestrictedZipCode restricted = new RestrictedZipCode();
            restricted.Zipcode = zipcode;
            restricted.CreatedBy = CurrentUser.FirstName + " " + CurrentUser.LastName;
            restricted.CreatedDate = DateTime.Now;



                try
                {
                    _Util.Facade.CityTaxFacade.InsertRestrictedZipCode(restricted);
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                    logger.Error(ex);
                }
            
            return Json(result);
        }

        public JsonResult CheckZipCode(string Zipcode)
        {
            var result = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;


            if (CurrentUser == null)
            {
                return Json(result);
            }

            RestrictedZipCode restrictedZipCodes = _Util.Facade.CityTaxFacade.GetRestrictedZipcodelistbyzipcode(Zipcode);

            if (restrictedZipCodes.TotalCount > 0)
            {
                result = true;

            }
            else
            {
                result = false;
            }


         

            return Json(result);
        }
    }
}