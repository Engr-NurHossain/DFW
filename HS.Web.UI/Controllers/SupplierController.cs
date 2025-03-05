using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.Helper;
using Newtonsoft.Json;
using Rotativa;
using Rotativa.Options;
using ClosedXML.Excel;
using Excel = HS.Web.UI.Helper.ExcelFormatHelper;
using NsExcel = Microsoft.Office.Interop.Excel;
using System.Globalization;
using System.Reflection;
using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HS.Web.UI.Controllers
{
    public class SupplierController : BaseController
    {
        // GET: Supplier
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

        public PartialViewResult SupplierPertial()
        {
            if (!base.IsPermitted(UserPermissions.ExpensePermissions.ExpenseVendorsTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView("_Supplier");
             
        }

        public PartialViewResult SupplierListAndBillPartial()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            else
            {
                return PartialView("_SupplierListAndBillPartial");
            }
        }

        public ActionResult SupplierListPartial(bool? GetReport)
        {
            if (!base.IsPermitted(UserPermissions.ExpensePermissions.ExpenseVendorsTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (GetReport.HasValue && GetReport == true)
            {
                DataTable dt;
                    dt = _Util.Facade.SupplierFacade.GetAllSupplierListByCompanyIdForExport(currentLoggedIn.CompanyId.Value);
                
                return MakeExcelFromDataTableForSupplier(dt, "Vendor V2");
            }
            List<Supplier> suppliers = _Util.Facade.SupplierFacade.GetAllSupplierListByCompanyId(currentLoggedIn.CompanyId.Value);
            VendorBillAmountPanel billPanelAmount = _Util.Facade.BillFacade.GetVendorBillAmountPanelByCompanyId(currentLoggedIn.CompanyId.Value);
            ViewBag.VendorBillOverDue = billPanelAmount.VendorBillOverDue;
            ViewBag.VendorBillOpen = billPanelAmount.VendorBillOpen;
            ViewBag.VendorBillPaid = billPanelAmount.VendorBillPaid;
            return PartialView("_SupplierList", suppliers);
        }

        private FileContentResult MakeExcelFromDataTableForSupplier(DataTable dtResult, string ReportFor)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                if (dtResult != null)
                {
                    wb.Worksheets.Add(dtResult);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    MemoryStream memorystreem = new MemoryStream();
                    wb.SaveAs(memorystreem);
                    var fName = string.Format("{0}-{1}.xlsx", ReportFor, DateTime.Now.UTCCurrentTime().ToString("MM/dd/yyyy"));

                    byte[] fileContents = memorystreem.ToArray();
                    var userAgent = HttpContext.Request.UserAgent.ToLower();
                    if (userAgent.Contains("iphone;") || userAgent.Contains("ipad;"))
                    {
                        //var newExFile= File(fileContents, Excel.Format("ExcelFormat"), fName);
                        //var excelApplicatiopn = new NsExcel.Application();
                        //excelApplicatiopn.Visible = true;
                        //NsExcel.Workbooks books = excelApplicatiopn.Workbooks;
                        //NsExcel.Workbook sheet = books.Open();
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

        public PartialViewResult SupperBillListPartial()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            else
            {
                List<Supplier> suppliers = _Util.Facade.SupplierFacade.GetAllSupplier();
                return PartialView("_SupplierBillList", suppliers);
            }
        }

        public PartialViewResult AddSupplier(int? id)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Supplier supplier;
            if (id.HasValue)
            {
                supplier = _Util.Facade.SupplierFacade.GetSupplierById(id.Value);
            }
            else
            {
                supplier = new Supplier();
            }
            ViewBag.StateList = _Util.Facade.LookupFacade.GetLookupByKey("StateList").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
            ViewBag.USACityist = _Util.Facade.LookupFacade.GetLookupByKey("USACity").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
            List<SelectListItem> SalesPersonList = new List<SelectListItem>();
            if (CurrentLoggedInUser.UserRole == "Admin" || CurrentLoggedInUser.UserRole == "SysAdmin")
            {
                var objcurrentlog = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
                SalesPersonList.Add(new SelectListItem()
                {
                    Text = "Please Select One",
                    Value = ""
                });
                SalesPersonList.AddRange(_Util.Facade.EmployeeFacade.GetALLEmployeeByCompanyIdAndIsRecruted(CurrentLoggedInUser.CompanyId.Value).Select(x =>
                      new SelectListItem()
                      {
                          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                          Value = x.UserId.ToString()
                      }).ToList());
                ViewBag.SalesPersonList = SalesPersonList.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
            }
            else
            {
                var objEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
                if (objEmp != null)
                {
                    SalesPersonList.Add(new SelectListItem()
                    {
                        Text = objEmp.FirstName.ToString() + " " + objEmp.LastName.ToString(),
                        Value = CurrentLoggedInUser.UserId.ToString()
                    });
                    ViewBag.SalesPersonList = SalesPersonList;
                }
            }
            return PartialView("_AddSupplier", supplier);
        }
     
        public PartialViewResult SupplierDetails(int? id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Supplier supplier;
            if (id.HasValue)
            {
                supplier = _Util.Facade.SupplierFacade.GetSupplierById(id.Value);
                supplier.SupplierPaymentList = _Util.Facade.BillFacade.GetVendorbillPaymentListBySupplierId(id.Value);
                VendorBillAmountPanel billPanelAmount = _Util.Facade.BillFacade.GetVendorDetailBillAmountPanelByCompanyId(currentLoggedIn.CompanyId.Value, id.Value);
                ViewBag.VendorBillOverDue = billPanelAmount.VendorBillOverDue;
                ViewBag.VendorBillOpen = billPanelAmount.VendorBillOpen;
                ViewBag.VendorBillPaid = billPanelAmount.VendorBillPaid;
            }
            else
            {
                supplier = new Supplier();
            }
            return PartialView("SupplierDetails", supplier);
        }

        public ActionResult SupplierBillListPartial(int? id, string order, int PageNo,string SearchText)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            Supplier model = new Supplier();
            //if (id.HasValue && id.Value > 0)
            //{
            //    model.SupplierBillList = _Util.Facade.BillFacade.GetVendorBillDeatailByVendorId(id.Value, order);
            //}
            if (PageNo == 0)
            {
                PageNo = 1;
            }
            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "SupplierBillListPageSize");
            int PageSize;
            if (glob != null)
            {
                PageSize = Convert.ToInt32(glob.Value);
            }
            else
            {
                PageSize = 10;
            }
            if (!string.IsNullOrEmpty(SearchText))
            {
                string str = SearchText;
                str = str.Replace(" ", "");
                SearchText = str;
            }
            if (id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.BillFacade.GetVendorBillDeatailByVendorId(id.Value, order,PageNo,PageSize,SearchText);
            }
            if (model.SupplierBillList.Count() == 0)
            {
                PageNo = 1;
            }

            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;


            if (model.SupplierBillList.Count() > 0)
            {
                ViewBag.OutOfNumber = model.TotalCount.CountTotal;
            }

            if ((int)ViewBag.PageNumber * PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / PageSize);
            return View(model);
        }

        public void VendorContact(Supplier supplier)
        {
            Contact contact = new Contact();
            if (!string.IsNullOrWhiteSpace(supplier.EmailAddress) || !string.IsNullOrWhiteSpace(supplier.Phone) )
            {
                bool existEmail = _Util.Facade.ContactFacade.ExistEmailorCellNo(supplier.EmailAddress, supplier.Phone, null);
                UserContact uc = _Util.Facade.ContactFacade.GetUserContactsByCustomerId(supplier.SupplierId);
            
                if (existEmail == false)
                {

                    contact.FirstName = supplier.CompanyName;
                  
                    contact.ContactId = Guid.NewGuid();


                    contact.Mobile = supplier.Phone;
                    contact.CreatedDate = DateTime.Now;
                    contact.Email = supplier.EmailAddress;
                 

                    _Util.Facade.ContactFacade.InsertContacts(contact);
                    if (uc == null)
                    {
                        UserContact userContact = new UserContact();
                        userContact.UserType = LabelHelper.UserType.Vendor;
                        userContact.UserId = supplier.SupplierId;
                        userContact.ContactId = contact.ContactId;
                        _Util.Facade.ContactFacade.InsertUserContacts(userContact);
                    }


                }
            }


        }
        [Authorize]
        [HttpPost]
        public JsonResult AddSupplier(Supplier supplier)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool Result = false;
            string Message = "";
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return Json(false);
            }
            if (string.IsNullOrWhiteSpace(supplier.CompanyName))
            {
                return Json(new { result = false, message = "Supplier name required." });
            }
            if (supplier.Id > 0 && supplier.SupplierId != new Guid())
            {
                supplier.CompanyId = currentLoggedIn.CompanyId.Value;
                //Supplier tempSup = _Util.Facade.SupplierFacade.GetSupplierById(supplier.Id);
                Result = _Util.Facade.SupplierFacade.UpdateSupplier(supplier);
                Message = "Vendor updated successfully.";
            }
            else
            {
                supplier.CompanyId = currentLoggedIn.CompanyId.Value;
                supplier.SupplierId = Guid.NewGuid();

                Supplier tempSupplier = _Util.Facade.SupplierFacade.GetSupplierByCompanyName(supplier.CompanyName);
                if (tempSupplier != null)
                {
                    return Json(new { result = false, message = "Supplier already exists." });
                }

                Result = _Util.Facade.SupplierFacade.InsertSupplier(supplier)>0;
                Message = "Vendor added successfully.";
                #region Vendor Contact
                var PopulateContact = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "AutomaticPopulateContactData");
                if (PopulateContact != null && PopulateContact.Value.ToLower() == "true")
                {
                    VendorContact(supplier);
                }
                #endregion
            }
            if (!Result)
            {
                Message = "Internal error. Please try again or contact system admin.";
            }

            return Json(new { result = Result,message=Message, SupplierId = supplier.SupplierId, SupplierName = supplier.CompanyName});
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteSupplier(int? id)
        {
            if (id.HasValue)
            {
                var productCategory = _Util.Facade.SupplierFacade.DeleteSupplier(id.Value);
            }
            return Json(true);
        }

        [Authorize]
        public ActionResult AddSupplierBill(int? Id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            NewBillingOfSuppliers model = new NewBillingOfSuppliers();
            if (Id.HasValue)
            {
                
            }
            else
            {
                SupplierBill NewSupplierBillObj = new SupplierBill();

                NewSupplierBillObj.BillNo = "";
                NewSupplierBillObj.CompanyId = currentLoggedIn.CompanyId.Value;
                NewSupplierBillObj.SuplierId = -1;
                NewSupplierBillObj.Amount = 0;
                NewSupplierBillObj.PaymentMethod = "";
                NewSupplierBillObj.PaymentStatus = "Init";
                NewSupplierBillObj.PaymentDate = DateTime.Now.UTCCurrentTime();
                NewSupplierBillObj.PaymentDueDate = DateTime.Now.UTCCurrentTime();
                NewSupplierBillObj.UpdatedDate = DateTime.Now.UTCCurrentTime();
                NewSupplierBillObj.UpdatedBy = User.Identity.Name;

                int SupplierBillId = _Util.Facade.SupplierFacade.InsertInitSupplierBill(NewSupplierBillObj);

                NewSupplierBillObj.BillNo = "SB-00" +SupplierBillId;

                _Util.Facade.SupplierFacade.UpdateSupplierBill(NewSupplierBillObj);
                model.SupplierBillObject = NewSupplierBillObj; 
            }

            List<SelectListItem> SupplierList = new List<SelectListItem>();
            SupplierList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            SupplierList.AddRange(ViewBag.EquipmentTypeList = _Util.Facade.SupplierFacade
                .GetAllSupplierByCompanyId(currentLoggedIn.CompanyId.Value).Select(x => new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList());
            ViewBag.SupplierListForBillPartial = SupplierList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();

            return PartialView("_AddSupplierBill",model);
        }

        [Authorize]
        public JsonResult GetCityStateZipListByKey(string key)
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetLeadCityStateSearchMaxLoad(CurrentUser.CompanyId.Value);

                List<CityZipCode> LCityState = _Util.Facade.CityZipcodeFacade.GetLeadCityStateListBySearchKey(key, ItemsLoadCount);
                if (LCityState.Count > 0)
                    result = JsonConvert.SerializeObject(LCityState);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public ActionResult AddSupplierDocument(int id)
        {
            ViewBag.SupplierId = id;
            return View("AddSupplierDocument");
        }
        public JsonResult GetSupplierDetailsBySupplierId(Guid supplierId)
        {
            var supplierDetails = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(supplierId);
            return Json(new { supplierDetails = supplierDetails });
        }
        [Authorize]
        public ActionResult SupplierDocument(int id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SupplierFile> model = new List<SupplierFile>();
            var objsupplier = _Util.Facade.SupplierFacade.GetSupplierById(id);
            if(objsupplier != null)
            {
                model = _Util.Facade.SupplierFacade.GetAllSupplierFileBySupplierIdAndCompanyId(CurrentUser.CompanyId.Value, objsupplier.SupplierId);
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult UploadSupplierDocument(int? id)
        {
            bool isUploaded = false;
            string filePath = "";
            string FullFilePath = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (id.HasValue)
            {
                var objsupplier = _Util.Facade.SupplierFacade.GetSupplierById(id.Value);
                HttpPostedFileBase httpPostedFileBase = Request.Files["SupplierFile"];

                string tempFolderName = ConfigurationManager.AppSettings["File.SupplierFiles"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                tempFolderName = string.Format(tempFolderName, comname);
                tempFolderName += "/" + objsupplier.SupplierId.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

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
                filePath = string.Concat("/", tempFolderName, "/", FileName);
                FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + AppConfig.DomainSitePath + filePath;
            }
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveSupplierFile(string File, int supplierid, string Description)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            
            if (!System.IO.File.Exists(serverFile))
            {
                return Json(new { result = true, message = "File not exsists" });
            }
            Supplier supplier = _Util.Facade.SupplierFacade.GetSupplierById(supplierid);
            var OnlyFileName = File.Split('/');
            float fileSize = OnlyFileName[6].Length;
            var delimeter = new string[] { "-___" };
            var fullFileName = OnlyFileName[6].Split(delimeter, StringSplitOptions.RemoveEmptyEntries);

            SupplierFile sf = new SupplierFile()
            {
                CompanyId = CurrentUser.CompanyId.Value,
                SupplierId = supplier.SupplierId,
                FileDescription = Description,
                Filename = File,
                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                FileFullName = fullFileName[1],
                IsActive = true,
                FileSize = fileSize
            };
            bool result = _Util.Facade.SupplierFacade.InsertSupplierFile(sf) > 0;
            return Json(new { result = result });
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveExpenseVendorImportFile(string File)
        {
            string subPath = "~/LeadImportReports"; // your code goes here

            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));

            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
            //{
            //    file.WriteLine("Started at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            //}
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    //{
                    //    file.WriteLine("Excel file read at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    //}
                    //Excel.Application xlApp = new Excel.Application();
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    //{
                    //    file.WriteLine("Excel application create at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    //}
                    var workbook = new ClosedXML.Excel.XLWorkbook(ExcelFile.FullName);
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    //{
                    //    file.WriteLine("Workbooks.Open at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    //}
                    ClosedXML.Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    //{
                    //    file.WriteLine("select sheet 1 at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    //}
                    var xlRange = workSheet.RangeUsed();

                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    //{
                    //    file.WriteLine("calculation started at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    //}
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();
                    var LookUpList = _Util.Facade.LookupFacade.GetAllLookup();
                    List<Supplier> supplierList = _Util.Facade.SupplierFacade.GetAllSupplier();
                    List<Employee> employees = _Util.Facade.EmployeeFacade.GetAllEmployee();
                    for (int i = 2; i <= rowCount; i++)
                    {
                        var result = false;
                        Supplier supplier = new Supplier();
                        Guid SupplierId = new Guid();
                        try
                        {
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        //if (header == "Id")
                                        //{
                                        //    int Id = 0;
                                        //    int.TryParse(value, out Id);
                                        //    if (Id > 0)
                                        //    {
                                        //        supplier.Id = Id;
                                        //    }
                                        //    continue;
                                        //}

                                        //else if (header == "SupplierId")
                                        //{
                                        //    if (Guid.TryParse(value, out SupplierId) && SupplierId != new Guid())
                                        //    {
                                        //        supplier.SupplierId = SupplierId;
                                        //    }
                                        //    else
                                        //    {
                                        //        supplier.SupplierId = Guid.NewGuid();
                                        //    }
                                        //    continue;

                                        //}
                                        if (header == "Companyname")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                supplier.CompanyName = value;
                                            }
                                            else
                                            {
                                                supplier.CompanyName = "";
                                            }
                                            continue;
                                        }
                                        else if (header == "EmailAddress")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                supplier.EmailAddress = value;
                                            }
                                            else
                                            {
                                                supplier.EmailAddress = "";
                                            }
                                            continue;
                                        }
                                        else if (header == "SalesRepName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                supplier.SalesRepName = value;
                                            }
                                            else
                                            {
                                                supplier.SalesRepName = "";
                                            }
                                            continue;
                                        }
                                        else if (header == "Street")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                supplier.Street = value;
                                            }
                                            else
                                            {
                                                supplier.Street = "";
                                            }
                                            continue;
                                        }
                                        else if (header == "Zipcode")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                supplier.Zipcode = value;
                                            }
                                            else
                                            {
                                                supplier.Zipcode = "";
                                            }
                                            continue;
                                        }
                                        else if (header == "City")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                supplier.City = value;
                                            }
                                            else
                                            {
                                                supplier.City = "";
                                            }
                                            continue;
                                        }
                                        else if (header == "State")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                supplier.State = value;
                                            }
                                            else
                                            {
                                                supplier.State = "";
                                            }

                                            continue;
                                        }
                                        else if (header == "Phone")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                supplier.Phone = value;
                                            }
                                            else
                                            {
                                                supplier.Phone = "";
                                            }
                                            continue;
                                        }
                                        else if (header == "TaxId")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                supplier.TaxId = value;
                                            }
                                            else
                                            {
                                                supplier.TaxId = "";
                                            }
                                            continue;
                                        }

                                        else if (header == "ContactPersonName")
                                        {
                                            Guid ContactPerson = new Guid();
                                            int ContactPersonInt = 0;

                                            if (value == "Unknown")
                                            {
                                                supplier.ContactPerson = new Guid("00000000-0000-0000-0000-000000000000");
                                            }

                                            if (string.IsNullOrWhiteSpace(value))
                                            {
                                                supplier.ContactPerson = new Guid("00000000-0000-0000-0000-000000000000");
                                            }
                                            else if (Guid.TryParse(value, out ContactPerson) && ContactPerson != new Guid())
                                            {
                                                supplier.ContactPerson = ContactPerson;
                                            }
                                            else if (int.TryParse(value, out ContactPersonInt) && ContactPersonInt > 0)
                                            {
                                                UserLogin ul = _Util.Facade.UserLoginFacade.GetUserLoginById(ContactPersonInt);
                                                if (ul != null)
                                                {
                                                    supplier.ContactPerson = ul.UserId;
                                                }
                                            }
                                            else
                                            {
                                                Employee employee = employees.Where(x => x.FirstName + " " + x.LastName == value).FirstOrDefault();
                                                if (employee != null)
                                                {
                                                    supplier.ContactPerson = employee.UserId;
                                                }
                                            }
                                            continue;

                                        }

                                        //else if (header == "CellularBackup")
                                        //{
                                        //    bool CellularBackup = false;
                                        //    if (bool.TryParse(value, out CellularBackup))
                                        //    {
                                        //        customer.CellularBackup = CellularBackup;
                                        //    }
                                        //    else
                                        //    {
                                        //        customer.CellularBackup = null;
                                        //    }
                                        //    continue;

                                        //}

                                        #region Header fix
                                        //else if (header == "First Name")
                                        //{
                                        //    header = "FirstName";
                                        //}
                                        //else if (header == "Last Name")
                                        //{
                                        //    header = "LastName";
                                        //}
                                        //else if (header == "Lead Type")
                                        //{
                                        //    header = "InstallType";
                                        //    value = LookUpList.Where(m => m.DisplayText == value && m.DataKey == "LeadInstallType").Select(m => m.DataValue).FirstOrDefault();
                                        //}
                                        
                                        
                                        #endregion

                                        if (supplier.GetType().GetProperty(header) != null)
                                        {
                                            supplier.GetType().GetProperty(header).SetValue(supplier, value);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {

                        }

                        supplier.SupplierId = SupplierId;
                        supplier.CompanyId = CurrentUser.CompanyId.Value;
                        supplier.IsActive = true;
                        Supplier isExistSupplier = new Supplier();
                        bool SupplierInserted = true;
                        if (!string.IsNullOrWhiteSpace(supplier.CompanyName))
                        {
                            isExistSupplier = supplierList.Where(x => x.CompanyName == supplier.CompanyName).FirstOrDefault();
                        }
                        if (isExistSupplier != null)
                        {
                            SupplierInserted = false;

                            supplier.Id = isExistSupplier.Id;
                            supplier.SupplierId = isExistSupplier.SupplierId;

                            result = _Util.Facade.SupplierFacade.UpdateSupplier(supplier);
                            
                        }
                        else
                        {
                            supplier.SupplierId = Guid.NewGuid();

                            supplier.Id = (int)_Util.Facade.SupplierFacade.InsertSupplier(supplier);
                            supplierList.Add(supplier);
                        }
                        result = supplier.Id > 0;
                    }
                    object misValue = System.Reflection.Missing.Value;
                    //xlWorkbook.Close(true, misValue, misValue);
                    //xlApp.Quit();

                    //Marshal.ReleaseComObject(xlWorksheet);
                    //Marshal.ReleaseComObject(xlWorkbook);
                    //Marshal.ReleaseComObject(xlApp);
                    System.IO.DirectoryInfo dir = new DirectoryInfo(serverFile);
                    if (ExcelFile != null)
                    {
                        ExcelFile.Delete();
                    }
                }
            }
            else
            {
                return Json(new { result = true, message = "File not exsists" });
            }
            return Json(new { result = true });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteSupplierFile(int Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SupplierFile tmpCF = _Util.Facade.SupplierFacade.GetFileNameBySupplierId(Id);
            var serverFile = Server.MapPath(tmpCF.Filename);
            if (System.IO.File.Exists(serverFile))
            {
                System.IO.File.Delete(serverFile);
            }
            _Util.Facade.SupplierFacade.DeleteSupplierById(Id);
            var objsup = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(tmpCF.SupplierId);
            return Json(new { result = true, UploadSupplierId = objsup.Id });
        }
    }
}