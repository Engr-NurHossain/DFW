using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.Helper;
using Newtonsoft.Json;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class ExpenseController : BaseController
    {
        // GET: Expense
        [Authorize]
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }
        #region no need
        [Authorize]
        public PartialViewResult ExpensePartial(string tab)
        {
            if (!string.IsNullOrWhiteSpace(tab))
            {
                ViewBag.tab = tab;
            }
            return PartialView("_Expense");
        }
         
        [Authorize]
        public PartialViewResult BillingExpensePartial()
        {
            return PartialView("BillingExpensePartial");
        }

        [Authorize]
        public PartialViewResult NewBillingPartial()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<SelectListItem> SupplierList = new List<SelectListItem>();
            SupplierList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            List<Supplier> SupplierDropDown = _Util.Facade.SupplierFacade.GetAllSupplierByCompanyId(currentLoggedIn.CompanyId.Value);
            ViewBag.EquipmentTypeList = SupplierDropDown;
            if(SupplierDropDown != null && SupplierDropDown.Count >0)
            {
                SupplierList.AddRange(SupplierDropDown.OrderBy(x => x.Name).Select(x => new SelectListItem()
                {
                    Text = x.Name.ToString(),
                    Value = x.Id.ToString()
                }).ToList());
            }
            
            ViewBag.SupplierListForBillPartial = SupplierList;

            return PartialView("_NewBillingPartial");
        }

        [Authorize]
        public PartialViewResult NewExpensePartial()
        {
            return PartialView("_NewExpensePartial");
        }

        [Authorize]
        public JsonResult GetAccountTypeListByKey(string key)
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                List<AccountType> AccountList = _Util.Facade.AccountTypeFacade.GetAccountTypeBySearchKey(key);

                if (AccountList.Count > 0)
                    result = JsonConvert.SerializeObject(AccountList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetEquipmentTypeListByKey(string key)
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                //List<AccountType> AccountList = _Util.Facade.AccountTypeFacade.GetAccountTypeBySearchKey(key);
                List<EquipmentType> AccountList = _Util.Facade.EquipmentTypeFacade.GetEquipmentTypeListByKey(key);
                if (AccountList.Count > 0)
                    result = JsonConvert.SerializeObject(AccountList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public PartialViewResult VendorBillingPartial()
        {

            if (!base.IsPermitted(UserPermissions.ExpensePermissions.ExpenseBillingTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            ExpenseSummary Model = _Util.Facade.ExpenseFacade.GetExpenseSummary(CurrentUser.CompanyId.Value);
            ViewBag.BillTypeList = _Util.Facade.LookupFacade.GetLookupByKey("BillType").Select(x =>
                    new SelectListItem()
                    {
                        Text = x.DisplayText.ToString(),
                        Value = x.DataValue.ToString()
                    }).ToList();
            // var EmployeeId = currentLoggedIn.EmployeeId.Value;

            return PartialView("VendorBillingPartial", Model);
        }

        [Authorize]
        public PartialViewResult ShowExpensePartial()
        {

            if (!base.IsPermitted(UserPermissions.ExpensePermissions.ExpenseBillingTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var ExpneseTypeList = _Util.Facade.LookupFacade.GetLookupByKey("ExpenseCategory").OrderBy(x => x.DisplayText).ToList();
            ExpneseTypeList.RemoveAll(x => x.DataValue == "-1");
          
            var  ExpenseCategory = ExpneseTypeList.Select(x =>
              new SelectListItem()
              {
                  Text = x.DisplayText.ToString(),
                  Value = x.DataValue.ToString(),
              
              }).ToList();
            ExpenseCategory.Insert(0, new SelectListItem()
            {
                Text = "Expense Category",
                Value ="-1"
            });
            ViewBag.ExpenseCategory = ExpenseCategory;
            List<SelectListItem> Emplist = new List<SelectListItem>();


            Emplist = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value).OrderBy(x =>x.FirstName).ToList().Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                }).ToList();

            Emplist.Insert(0, new SelectListItem()
            {
                Text = "Expensed By",
                Value = Guid.Empty.ToString()
            });
            ViewBag.EmployeeList = Emplist;

            return PartialView("ShowExpensePartial");
        }
        #endregion
        public ActionResult ExpenseListPartial(VendorBillFilter filter)
        {

            if (!base.IsPermitted(UserPermissions.ExpensePermissions.ExpenseBillingTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
         
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
          
            if (filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }
            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "CustomerListPageSize");
            if (glob != null)
            {
                filter.UnitPerPage = Convert.ToInt32(glob.Value);
            }
            else
            {
                filter.UnitPerPage = 10;
            }
            if (filter.PageNumber == null || filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }

            TransactionExpenseModel expenseList = new TransactionExpenseModel();
            expenseList = _Util.Facade.TransactionFacade.GetAllExpenseByComanyId(currentLoggedIn.CompanyId.Value, filter);

            ViewBag.OutOfNumber = expenseList.TransactionExpenseCount.TotalCount;


            if ((int)ViewBag.OutOfNumber == 0)
            {
                ViewBag.Message = "No Content Available !";
            }


            if (@ViewBag.OutOfNumber == 0)
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
            ViewBag.CustomerId = Guid.Empty;

            
            return View(expenseList.TransactionExpenseList);
        }
        ////Bill List
        //[Authorize]
        //public PartialViewResult VendorBillList()
        //{
        //    if (!base.IsPermitted(UserPermissions.ExpensePermissions.ExpenseBillingTab))
        //    {
        //        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
        //    }

        //    var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

        //    List<ShowBillModel> vendorBillListModel = new List<ShowBillModel>();

        //    //var EmployeeId = currentLoggedIn.EmployeeId.Value;
        //    var CompanyId = currentLoggedIn.CompanyId.Value;
        //    vendorBillListModel = _Util.Facade.BillFacade.GetAllVendorBillViewByComanyId(CompanyId);

        //    List<int> CheckPaymentIdList = vendorBillListModel.Where(x => x.PaymentMethod == "Check" && x.Type == "Payment").Select(x => x.PaymentId).ToList();

        //    if (CheckPaymentIdList.Count() > 0)
        //    {
        //        ViewBag.CheckList = _Util.Facade.BillFacade.GetCheckListByPaymentIdListAndCompanyId(currentLoggedIn.CompanyId.Value,CheckPaymentIdList);

        //    }

        //    return PartialView("VendorBillList", vendorBillListModel);
        //}

        public ActionResult VendorBillList(VendorBillFilter filter)
        {
            if (!base.IsPermitted(UserPermissions.ExpensePermissions.ExpenseBillingTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }
            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "CustomerListPageSize");
            if (glob != null)
            {
                filter.UnitPerPage = Convert.ToInt32(glob.Value);
            }
            else
            {
                filter.UnitPerPage = 10;
            }
            if (filter.PageNumber == null || filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }

            //List<ShowBillModel> vendorBillListModel = new List<ShowBillModel>();
            ShowBillModel vendorBillListModel = new ShowBillModel();
            var CompanyId = currentLoggedIn.CompanyId.Value;

            vendorBillListModel = _Util.Facade.BillFacade.GetAllVendorBillViewByComanyId(CompanyId,filter);
            
            ViewBag.OutOfNumber = vendorBillListModel.ShowBillModelList.Count;
          

            if ((int)ViewBag.OutOfNumber == 0)
            {
                ViewBag.Message = "No Content Available !";
            }


            if (@ViewBag.OutOfNumber == 0)
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



            return PartialView("VendorBillList", vendorBillListModel);
        }

        public ActionResult LoadBillFiles(string BillNo)
        {
            List<BillFile> billFile = new List<BillFile>();
             billFile = _Util.Facade.BillFacade.GetAllBillFileByBillNo(BillNo);
            return View(billFile);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteBillFile(int Id)
        {
            bool result = false;
            if (Id > 0)
            {
                long id = _Util.Facade.BillFacade.DeleteBillFile(Id);
                if (id > 0)
                {
                    result = true;

                }
                return Json(new { result = true, message = "File deleted successfully."});
            }
            else
            {
                return Json(new { result = false, message = "Internal error. Please report to system admin." });
            }

        }
        //Add Bill
        [Authorize]
        public PartialViewResult AddVendorBill(int? id, int? SupplierId,string BillID)
        {

            if (SupplierId.HasValue)
            {
                if (!base.IsPermitted(UserPermissions.ExpensePermissions.VendorsDetailCreateABill))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.ExpensePermissions.ExpneseBillingAddBill))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            CreateVendorBill model;
             
            #region Load Old Bill

            if ((id.HasValue && id.Value > 0) || !string.IsNullOrWhiteSpace(BillID))
            {
                model = new CreateVendorBill();
                if (id.HasValue && id.Value > 0)
                {
                    model.Bill = _Util.Facade.BillFacade.GetBillById(id.Value);
                }
                else
                {
                    model.Bill = _Util.Facade.BillFacade.GetBillByBillNo(BillID);
                }

                if (model.Bill == null || model.Bill.CompanyId != currentLoggedIn.CompanyId.Value)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                if(model.Bill.SupplierId.HasValue && model.Bill.SupplierId.Value > 0)
                {
                    Supplier Sup = _Util.Facade.SupplierFacade.GetSupplierById(model.Bill.SupplierId.Value);
                    if(Sup != null)
                    {
                        model.Bill.SupplierName = Sup.CompanyName;
                    }
                }
                model.BillDetailList = _Util.Facade.BillFacade.GetBillDetialsListByBillId(model.Bill.Id); 
            }
            #endregion

            #region Insert and Load Bill
            else
            {
                model = new CreateVendorBill(); 
                model.Bill = new Bill()
                {
                    PaymentDueDate = DateTime.Now.UTCCurrentTime(),
                    PaymentDate = DateTime.Now.UTCCurrentTime(),
                    CompanyId = currentLoggedIn.CompanyId.Value,
                    Amount = 0,
                    PaymentStatus = "Init",
                    UpdatedDate = DateTime.Now.UTCCurrentTime(), 
                    UpdatedBy = User.Identity.Name,
                    
                };
                
                model.Bill.Id = _Util.Facade.BillFacade.InsertBill(model.Bill);
                model.Bill.BillNo =  model.Bill.Id.GenerateBillNO();
                _Util.Facade.BillFacade.UpdateBill(model.Bill);
                
                model.Bill.SupplierId = SupplierId.HasValue ? SupplierId: 0;
               

                model.BillDetailList = new List<BillDetail>();
            }

            #endregion

            #region ViewBags
            if( model.Bill.SupplierId.HasValue && model.Bill.SupplierId > 0 && string.IsNullOrWhiteSpace(model.Bill.SupplierName))
            {
                Supplier supplier = _Util.Facade.SupplierFacade.GetVendorNameByCompanyIdAndSupplierId(currentLoggedIn.CompanyId.Value, model.Bill.SupplierId);
                model.Bill.SupplierName = supplier.CompanyName;
                if(string.IsNullOrWhiteSpace( model.Bill.SupplierAddress))
                {
                    model.Bill.SupplierAddress = supplier.Street + Environment.NewLine + supplier.City + " " + supplier.State + ", " + supplier.Zipcode;

                }

            }
            
            ViewBag.PaymentMethods = _Util.Facade.LookupFacade.GetLookupByKey("PaymentMethod").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
            ViewBag.Term = _Util.Facade.LookupFacade.GetLookupByKey("InvoiceTerms").Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString(),
                 Selected = x.DataValue.ToString()== "30" ? true : false
             }).ToList();
            #endregion

            List<SelectListItem> BillForList = new List<SelectListItem>();
            BillForList.Add(new SelectListItem() {
                Text ="Employee",
                Value = "Employee",
            });
            BillForList.Add(new SelectListItem()
            {
                Text = "Vendor",
                Value = "Vendor",
                Selected = true
            });
            BillForList.Add(new SelectListItem()
            {
                Text = "In-House",
                Value = "InHouse",
            });
            ViewBag.BillForList = BillForList;

            if (currentLoggedIn.UserTags.ToLower().IndexOf(LabelHelper.UserTags.Admin.ToLower()) > -1)
            {
                List<SelectListItem> EmployeeList = new List<SelectListItem>();
                List<Employee> EmployeeDropDown = _Util.Facade.EmployeeFacade.GetAllEmployee(currentLoggedIn.CompanyId.Value).ToList();

                if(EmployeeDropDown != null && EmployeeDropDown.Count > 0)
                {
                    EmployeeList = EmployeeDropDown.OrderBy( x => x.FirstName).Select(x =>
                    new SelectListItem()
                    {
                        Text = x.FirstName + " " + x.LastName,
                        Value = x.UserId.ToString()
                    }).ToList();
                }

                
                EmployeeList.Insert(0, new SelectListItem() {
                    Text ="Select One",
                    Value = Guid.Empty.ToString()
                });
                ViewBag.EmployeeList = EmployeeList;
            }
            else
            {
                List<SelectListItem> EmployeeList = new List<SelectListItem>();
                EmployeeList.Add(new SelectListItem()
                {
                    Text = currentLoggedIn.GetFullName(),
                    Value = currentLoggedIn.UserId.ToString()
                });
                ViewBag.EmployeeList = EmployeeList;
            }
            
            return PartialView("AddVendorBill",model);
        }
        //Add Bill
        [Authorize]
        [HttpPost]
        public JsonResult AddVendorBill(CreateVendorBill Model)
        {
            if (!base.IsPermitted(UserPermissions.ExpensePermissions.ExpneseBillingAddBill))
            {
                return Json(new
                {
                    result = false,
                    message = "Access denied."
                });
            }
            
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            Model.Bill.CompanyId = CurrentUser.CompanyId.Value;
            Model.Bill.UpdatedBy = User.Identity.Name;
            Model.Bill.UpdatedDate = DateTime.Now.UTCCurrentTime();
            Model.Bill.PaymentStatus = "Open";

            if (Model.BillDetailList.Count() == 0)
            {
                return Json(new
                {
                    result = false, 
                    message = "No Bill Detail."
                });
            }

            if (Model.Bill.Id > 0)
            {
                _Util.Facade.BillFacade.UpdateBill(Model.Bill);
                result = _Util.Facade.BillFacade.DeleteAllBillDetailsByBillId(Model.Bill.Id);
                if(result == true)
                {
                    //_Util.Facade.BillFacade.InsertBillDetailList(Model.BillDetailList);
                    foreach(var item in Model.BillDetailList)
                    {
                        BillDetail objBillDetail = new BillDetail()
                        {
                            CustomerBillId = item.CustomerBillId,
                            EquipmentId = item.EquipmentId,
                            AccoutTypeId = item.AccoutTypeId,
                            Dscription = item.Dscription,
                            Quantity = item.Quantity,
                            Rate = item.Rate,
                            Amount = item.Amount,
                            ItemName = item.EquipmentName
                        };
                        _Util.Facade.BillFacade.InsertBillDetail(objBillDetail);
                    }
                }

             
            }
            else
            {
                Model.Bill.Id = _Util.Facade.BillFacade.InsertBill(Model.Bill);
            }
    
            return Json(new {
                result = true,
                Id = Model.Bill.Id,
                message = "Bill saved successfully."
            });
        }

        public JsonResult AddVendorBilFile(string File,string BillNo, string FileDes)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            #region Attach File
            bool result = false;
            if (!string.IsNullOrEmpty(File))
            {

                var serverFile = Server.MapPath(File);

                if (!System.IO.File.Exists(serverFile))
                {
                    return Json(new { result = true, message = "File not exsists" });
                }
                //Customer tmpCustomer = _Util.Facade.CustomerFacade.GetById(CustomerId);
                var OnlyFileName = File.Split('/');
                float fileSize = OnlyFileName[6].Length;
                var delimeter = new string[] { "-___" };
                var fullFileName = OnlyFileName[6].Split(delimeter, StringSplitOptions.RemoveEmptyEntries);

                BillFile cf = new BillFile()
                {
                  
                    BillNo = BillNo,
                    CompanyId = CurrentUser.CompanyId.Value,
                    Filename = File,
                    Uploadeddate = DateTime.Now.UTCCurrentTime(),
                    FileFullName = fullFileName[fullFileName.Length - 1],
                    IsActive = true,
                    FileSize = fileSize,
                    FileDescription = FileDes
                };
                result = _Util.Facade.BillFacade.InsertBillFile(cf) > 0;
            }

            return Json(new { result = result });
            #endregion
        }

        //MakePayment
        [Authorize]
        public PartialViewResult MakePayment( int? PaymentId , int? BillId, int? SupplierId, Guid? empid)
        {

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            BillPayment tmpPayment =null; 
            MakePayment Model = new Entities.MakePayment();  
            Model.PaymentDate = DateTime.Now.UTCCurrentTime();
            Model.EmailAddress = "";

            if (SupplierId.HasValue && SupplierId>0) {
                Supplier sup = _Util.Facade.SupplierFacade.GetSupplierById(SupplierId.Value);
                ViewBag.SupplierId = SupplierId.Value;
                ViewBag.SupplierName = sup.Name;
            }

            Model.Transactions = _Util.Facade.BillFacade.GetAllReceivePaymentList(CurrentUser.CompanyId.Value, PaymentId, SupplierId, empid);

            if (Model.Transactions == null)
            {
                Model.Transactions = new List<OutStandingTransactions>();
            }

            List<int> BillIdList = new List<int>();
            if (PaymentId.HasValue)
            {
                List<BillPaymentHistory> BillHis = _Util.Facade.BillFacade.GetAllBillHistoryByBillId(PaymentId.Value);
                BillIdList = BillHis.Select(x => x.InvoiceId).ToList();
                Model.AmountDue = BillHis.Sum(x => x.Amount);
                ViewBag.PaymentId = PaymentId.Value;
                tmpPayment = _Util.Facade.BillFacade.GetBillPaymentByIdAndCompanyId(PaymentId.Value,CurrentUser.CompanyId.Value);
            }
            else
            {
                Model.AmountDue = 0;
                ViewBag.PaymentId = 0;
            }
             

            #region ViewBags
              
            if (BillId.HasValue)
            {
                BillIdList.Add(BillId.Value);
                ViewBag.BillId = BillId.Value;
            }
            else
            {
                ViewBag.BillId = 0;
            }

            if(BillIdList.Count() > 0)
            {
                ViewBag.BillIdList = BillIdList;
            }
            if(tmpPayment!=null && tmpPayment.PaymentInfoId > 0)
            {
                Model.PaymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoById(tmpPayment.PaymentInfoId.Value);
                Model.PaymentMethod = tmpPayment.PaymentMethod;
            }
            if(Model.PaymentInfo == null)
            {
                Model.PaymentInfo = new PaymentInfo();
            }
            ViewBag.ECheckType = _Util.Facade.LookupFacade.GetLookupByKey("ECheckType").Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString()
                         }).ToList();
            ViewBag.BankAccountType = _Util.Facade.LookupFacade.GetLookupByKey("BankAccountType").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
            //ViewBag.VendorList = _Util.Facade.SupplierFacade.GetAllSupplierByCompanyId(CurrentUser.CompanyId.Value);

            ViewBag.PaymentMethods = _Util.Facade.LookupFacade.GetLookupByKey("PaymentMethod").Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();

            #endregion

            return PartialView("_MakePayment",Model);
        }


        [Authorize]
        [HttpPost]
        public ActionResult UploadBillFile()
        {

            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
          

            HttpPostedFileBase httpPostedFileBase = Request.Files["BillFiles"];

            string tempFolderName = ConfigurationManager.AppSettings["File.BillFiles"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            Random rand = new Random();
            tempFolderName += "/" + rand.Next().ToString() + "/" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

        
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
            string FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
            string filedes = httpPostedFileBase.FileName;
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath, FullFilePath = FullFilePath, filedes = filedes }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public JsonResult MakePayment(MakePaymentModel Model, bool SendEmail, bool CreatePdf)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            var TotalAmount = Model.Transactions.Count() == 0 ? 0 : Model.Transactions.Sum(x => x.Payment);


            BillPayment bp = new BillPayment()
            {
                Status = "Closed",
                Type = "Payment",
                TransacationDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CurrentUser.CompanyId.Value,
                CustomerId = new Guid(), 
                Amount = TotalAmount,
                PaymentMethod = Model.PaymentMethod,
                AddedBy = User.Identity.Name,
                AddedDate = DateTime.Now.UTCCurrentTime(),
                ReferenceNo =Model.RefNo,
                

            };
            bp.Id = _Util.Facade.BillFacade.InsertBillPayment(bp);

            if (Model.PaymentInfo != null)
            {
                Model.PaymentInfo.CompanyId = CurrentUser.CompanyId.Value;
                Model.PaymentInfo.Id = (int)_Util.Facade.PaymentInfoFacade.InsertPaymentInfo(Model.PaymentInfo);
                bp.PaymentInfoId = Model.PaymentInfo.Id;
                _Util.Facade.BillFacade.UpdateBillPayment(bp);
            }

            List<BillPaymentHistory> bphistory = new List<BillPaymentHistory>();

            foreach (var item in Model.Transactions)
            {
                Bill Bill = _Util.Facade.BillFacade.GetBillById(item.Id);

                //if(inv.BalanceDue>0 && inv.BalanceDue <= item.Payment)
                //{
                bphistory.Add(new BillPaymentHistory()
                {
                    Amount = item.Payment,
                    InvoiceId = item.Id,
                    BillPaymentId = bp.Id,
                    Balance = Bill.PaymentDue.HasValue ? Bill.PaymentDue.Value : 0,
                     
                });
                Bill.PaymentDue = Bill.PaymentDue - item.Payment;
                if (Bill.PaymentDue == 0)
                {
                    Bill.PaymentStatus = "Paid";
                }
                else
                {
                    Bill.PaymentStatus = "Partial";
                }
                if(Model.PaymentMethod != "")
                {
                    Bill.PaymentMethod = Model.PaymentMethod;
                }
                _Util.Facade.BillFacade.UpdateBill(Bill);
            }
            if (bphistory.Count() > 0)
            {
                _Util.Facade.BillFacade.InsertBillPaymentHistoryList(bphistory);
            }

            return Json(new { result = true, message="Make payemnt done" });
        }

        [Authorize]
        public ActionResult ExpensePreview()
        { 
            if (Session["PaymentPdf_"] != null)
            {
                ViewBag.pdfLocation = AppConfig.DomainSitePath + "/" +  Session["PaymentPdf_"].ToString();
            }else
            {
                ViewBag.pdfLocation = "";
            }
            return View();
        }

        [Authorize]
        public JsonResult CreatePaymentPreview(MakePaymentModel Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string PdfFilePath = "";
            Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            Model.CompanyAddress = MakeAddress(tempCom.State, tempCom.City, tempCom.State, tempCom.ZipCode, "");
            Model.CompanyEmail = tempCom.EmailAdress;
            Model.CompanyName = tempCom.CompanyName;
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("PaymentPdf", Model)
            {
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.Payments"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            filename = string.Format(filename, comname);
            filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + rand.Next().ToString() + "_" + rand.Next().ToString() + "_Payment.pdf";
            Session["PaymentPdf_"] = filename;
            PdfFilePath = filename;
            string Serverfilename = FileHelper.GetFileFullPath(filename);
            FileHelper.SaveFile(applicationPDFData, Serverfilename);

            return Json(new { result = true, message = "Make payemnt done", filepath = AppConfig.DomainSitePath + PdfFilePath }); 
        }

        [Authorize]
        public JsonResult CreateBillPreview(CreateVendorBill Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string PdfFilePath = "";
            Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            Model.CompanyAddress = MakeAddress(tempCom.State, tempCom.City, tempCom.State, tempCom.ZipCode, "");
            Model.CompanyEmail = tempCom.EmailAdress;
            Model.CompanyName = tempCom.CompanyName;
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("BillPdf", Model)
            {
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.Bills"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            filename = string.Format(filename, comname);
            filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + rand.Next().ToString() + "_" + rand.Next().ToString() + "_Bill.pdf";
            Session["PaymentPdf_"] = filename;
            PdfFilePath = filename;
            string Serverfilename = FileHelper.GetFileFullPath(filename);
            FileHelper.SaveFile(applicationPDFData, Serverfilename);

            return Json(new { result = true, message = "Make payemnt done", filepath = AppConfig.DomainSitePath + PdfFilePath });
        }


        public ActionResult Getcheck(double Amount,string Name,string Memo)
        {
            string centval = (Amount - (int)Amount).ToString("#.##");
            double valcent = 0;
            if (!string.IsNullOrWhiteSpace(centval))
            {
                valcent = Convert.ToDouble(centval);
            }
            int cent = (int)(valcent * 100);
            //int cent = (int)((Amount - (int)Amount) * 100);

            CheckModel cm = new CheckModel()
            {
                Amount = string.Format("{0:0,0.00}",Amount) ,
                AmountInWord = NumberToWords((int)Amount),
                CheckDate = DateTime.Now.UTCCurrentTime(),
                Name = Name,
                Memo = Memo

            };
            if (cent > 0)
            {
                cm.AmountInWord += " " + cent + "/100";
            }

            //return new Rotativa.ActionAsPdf("GetSamples");

            return new ViewAsPdf(cm)
            {
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 0, Right = 0 },

            };
            return View(cm);
        }


        public ActionResult CheckPreview(float Amount, string Name,string Memo)
        {
            ViewBag.Amount = Amount;
            ViewBag.Name = Name;
            ViewBag.Memo = Memo;
            
            return View();
        }

        public ActionResult BillAccountsPayable(DateTime? Start, DateTime? End,string Order)
        {
            if (!base.IsPermitted(UserPermissions.ExpensePermissions.ExpenseAccountsPayableTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (Start == null && End == null)
            {
                DateTime StartDate = new DateTime();
                DateTime EndDate = new DateTime();
                Start = StartDate;
                End = EndDate;
            }

            List<ShowBillModel> sbm = new List<ShowBillModel>();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            sbm = _Util.Facade.BillFacade.GetVendorBillPayableList(CurrentUser.CompanyId.Value, Start, End, Order);
            return View(sbm);
        }
        #region Private

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
         
        #region NumberToWord
        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
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

            return words;
        }
        #endregion

        #endregion

       
        [Authorize]
        public JsonResult GetVendorListByKey(string key)
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetVendorSearchMaxLoad(CurrentUser.CompanyId.Value);

                List<Supplier> SupplierList = _Util.Facade.SupplierFacade.GetAllSupplierByCompanyIdAndSearchKey(CurrentUser.CompanyId.Value,key, ItemsLoadCount);
                if (SupplierList.Count > 0)
                    result = JsonConvert.SerializeObject(SupplierList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult BillPaymentPartial(DateTime? Start, DateTime? End,string order)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            //ViewBag.StartDate = Start;
            //ViewBag.EndDate = End;
            //DateTime StartDate = new DateTime();
            //DateTime EndDate = new DateTime();
            //string newCookie = "";
            //if (Request.Cookies[CookieKeys.DateViewFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.DateViewFilter].Value))
            //{
            //    newCookie = Request.Cookies[CookieKeys.DateViewFilter].Value;
            //    newCookie = Server.UrlDecode(newCookie);
            //    var CookieVals = newCookie.Split(',');

            //    if (CookieVals.Length == 3)
            //    {
            //        StartDate = CookieVals[0].ToDateTime();
            //        EndDate = CookieVals[1].ToDateTime();
            //    }
            //}
            if (Start == null && End == null)
            {
                DateTime StartDate = new DateTime();
                DateTime EndDate = new DateTime();
                Start = StartDate;
                End = EndDate;
            }
            //List<BillPaymentHistory> model = new List<BillPaymentHistory>();
            BillPaymentHistory model = new BillPaymentHistory();
            model = _Util.Facade.BillFacade.GetVendorbillPaymentList(Start, End,order);
            return View(model);
        }
    }
}