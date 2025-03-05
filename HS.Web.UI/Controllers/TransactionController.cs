using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HS.Entities;
using HS.Framework;
using HS.Payments.RecurringBilling;
using HS.Payments.TransactionReporting;
using System.Net;
using System.IO;
using System.Text;
using AuthorizeNet.Api.Contracts.V1;
using HS.Web.UI.Helper;
using HS.Payments.PaymentTransactions;
using System.Collections;
using Rotativa;
using HS.Facade;
using Excel = ClosedXML.Excel;
using System.Runtime.InteropServices;
using Rotativa.Options;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Globalization;
using HS.Framework.Utils;
using Newtonsoft.Json;
using NLog;

namespace HS.Web.UI.Controllers
{
    public class TransactionController : BaseController
    {
        public TransactionController()
        {
            logger= LogManager.GetCurrentClassLogger();
        }
        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TransactionPartial(int CustomerId)
        {
            var cusobj = _Util.Facade.CustomerFacade.GetCustomerById(CustomerId);
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CustomerCompany cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerGuidId(cusobj.CustomerId);
            bool isDeclinedAdded = true;
            if (cuscom != null)
            {
                GlobalSetting isDeclined = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(cuscom.CompanyId, "IsDeclinedInvoiceAddedInUnpaidAmount");
                if (isDeclined != null)
                {
                    isDeclinedAdded = !string.IsNullOrWhiteSpace(isDeclined.Value) && isDeclined.Value.ToLower() == "false" ? false : true;
                }
            }

            EstimateStatusDetail EstimateStatusDetail = _Util.Facade.CustomerFacade.GetAllEstimateStatusDetailByCustomerId(cusobj.CustomerId, isDeclinedAdded);
            ViewBag.DueAmountDetail = EstimateStatusDetail.DueAmountDetail;
            ViewBag.EstimateAmountDetail = EstimateStatusDetail.EstimateAmountDetail;
            ViewBag.PaidAmountDetail = EstimateStatusDetail.PaidAmountDetail;
            ViewBag.UnpaidInvoice = EstimateStatusDetail.UnpaidAmount;
            ViewBag.CustomerCredit = EstimateStatusDetail.CustomerCredit;
            var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value, cusobj.CustomerId);

            double Tax = 0.0;
            if (GetSalesTax != null)
            {
                Tax = Convert.ToDouble(GetSalesTax.Value);
            }
            if (EstimateStatusDetail.PaidAmountDetail > 0)
            {
                if (cusobj.TaxExemption == "No" || cusobj.TaxExemption == "")
                {
                    ViewBag.PaidAmountDetailWithOutTax = EstimateStatusDetail.PaidAmountDetail - ((EstimateStatusDetail.PaidAmountDetail * Tax) / (100 + Tax));
                }
                else
                {
                    ViewBag.PaidAmountDetailWithOutTax = EstimateStatusDetail.PaidAmountDetail;
                }

            }
            else
            {
                ViewBag.PaidAmountDetailWithOutTax = EstimateStatusDetail.PaidAmountDetail;
            }
            return View();
        }
        [Authorize]
        public ActionResult PrintTransaction(int TransactionId, string ToEmailAddress)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            TransactionHistory objtrans = _Util.Facade.TransactionFacade.GetTransactionHistoryById(TransactionId);
            if (objtrans != null)
            {
                ViewBag.TransactionId = objtrans.Id;
                ViewBag.companyname = CurrentUser.CompanyName;
            }
            ViewBag.ToEmailAddress = ToEmailAddress;
            return View();
        }



        public ActionResult TransactionPdf(int TransactionId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            TransactionPdfModel pdfmodel = _Util.Facade.TransactionFacade.GetTransactionPdfDataByTransactionId(TransactionId);
            pdfmodel.CompanyId = CurrentUser.CompanyId.Value;
            pdfmodel.CompanyName = CurrentUser.CompanyName;
            pdfmodel.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            pdfmodel.CompanyAddressFormat = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
            pdfmodel.AmountInWord = NumberToWords(pdfmodel.PaymentAmount);
            Company Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);

            Hashtable datatemplate = new Hashtable();
            #region CompanyAddress Info
            datatemplate.Add("ComapnyName", Company.CompanyName);
            datatemplate.Add("Address", Company.Address);
            datatemplate.Add("Street", Company.Street);
            datatemplate.Add("City", Company.City);
            datatemplate.Add("State", Company.State);
            datatemplate.Add("Zip", Company.ZipCode);
            datatemplate.Add("CompanyPhone", Company.Phone);
            datatemplate.Add("EmailAddress", Company.EmailAdress);
            datatemplate.Add("WebAddress", Company.Website);
            #endregion
            ViewBag.TransactionId = TransactionId;
            pdfmodel.CompanyAddressFormat = HS.Web.UI.Helper.LabelHelper.ParserHelper(pdfmodel.CompanyAddressFormat, datatemplate);
            return new ViewAsPdf("TransactionPdf", pdfmodel);
        }

        public ActionResult ShowAllTransactions(int CustomerId, string SearchText, CustomerFilter filter)
        {

            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerFundingList))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
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

            #region Week Filter Dropdown
            //Copany start date will be retrive from global settings
            DateTime CompanyStartDate = new DateTime(2018, 7, 1);
            int Week = GetIso8601WeekOfYear(CompanyStartDate);

            int CurrentWeek = GetIso8601WeekOfYear(DateTime.Now);
            CompanyStartDate = FirstDateOfWeek(CompanyStartDate.Year, Week, ci, DateOffset);

            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            string PtoFilter = "-1";



            while (CompanyStartDate < DateTime.Now)
            {
                string suffix = "th";
                if (CompanyStartDate.Day == 1 || CompanyStartDate.Day % 20 == 1 || CompanyStartDate.Day % 30 == 1)
                {
                    suffix = "st";
                }
                else if (CompanyStartDate.Day == 2 || CompanyStartDate.Day % 20 == 2)
                {
                    suffix = "nd";
                }
                else if (CompanyStartDate.Day == 3 || CompanyStartDate.Day % 20 == 3)
                {
                    suffix = "rd";
                }
                #region Common filter
                ViewBag.PTOFilterOptions = _Util.Facade.LookupFacade.GetLookupByKey("PTOFilterOptions").Select(x =>
                   new SelectListItem()
                   {
                       Text = x.DisplayText.ToString(),
                       Value = x.DataValue.ToString(),
                       Selected = x.DataValue == PtoFilter
                   }).ToList();
                #endregion
                //WeekList.Add(new SelectListItem()
                //{
                //    Text = string.Format(CompanyStartDate.ToString("dd{0} MMMM yy"), suffix),
                //    Value = CompanyStartDate.Year + "/" + Week,
                //    Selected = Week == CurrentWeek
                //});
                CompanyStartDate = CompanyStartDate.AddDays(7);
                Week = GetIso8601WeekOfYear(CompanyStartDate);
            }

            if (StartDate == new DateTime() && EndDate == new DateTime())
            {
                ViewBag.EndDate = CompanyStartDate;
                ViewBag.StartDate = CompanyStartDate.AddDays(-7);
            }
            else
            {
                ViewBag.EndDate = EndDate;
                ViewBag.StartDate = StartDate;
            }

            ViewBag.FirstDayOfWeek = FirstDayOfWeek;
            #endregion
            #region count

            Customer customer = _Util.Facade.CustomerFacade.GetCustomerById(CustomerId);
            CustomerDetailsTabCount customerDetailsTabCount = _Util.Facade.InvoiceFacade.GetCustomerDetailsTabCountsByCustomerId(customer.CustomerId, CurrentUser.CompanyId.Value);
            ViewBag.FundingCount = customerDetailsTabCount.FundingCount;
            ViewBag.ExpenseCount = customerDetailsTabCount.ExpenseCount;



            #endregion
            return View("_ShowAllTransactions");
        }
        [Authorize]
        public ActionResult FundingListPartial(int CustomerId, string SearchText, int PageNo, CustomerFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId, CurrentUser.CompanyId.Value);
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (!res)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            int PageLimit = _Util.Facade.GlobalSettingsFacade.GetCustomerFundingPageLimit(CurrentUser.CompanyId.Value);
            if (PageNo > 0)
            {
                filter.PageNo = PageNo;
            }

            if (filter.PageNo == 0)
            {
                filter.PageNo = 1;
            }

            if (filter.PageSize < PageLimit)
            {
                filter.PageSize = PageLimit;
            }
            List<Transaction> CustomerTransactions = _Util.Facade.TransactionFacade.GetAllTransactionsByCustomerIdAndCompanyIdAndFilter(CustomerId, CurrentUser.CompanyId.Value, SearchText, CurrentUser.UserId, filter);
            var TotalFundCount = _Util.Facade.TransactionFacade.GetAllTransactions1ByCustomerIdAndCompanyIdAndFilter(CustomerId, CurrentUser.CompanyId.Value, SearchText, CurrentUser.UserId);
            if (CustomerTransactions.Count() == 0)
            {
                filter.PageNo = 1;
                //customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilter(filter);
            }

            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = filter.order;

            if (CustomerTransactions.Count() > 0)
            {
                ViewBag.OutOfNumber = TotalFundCount.Count();
            }

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }
            ViewBag.CustomerGuidId = cus.CustomerId;
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);
            ViewBag.CustomerEmailAddress = cus != null && !string.IsNullOrWhiteSpace(cus.EmailAddress) ? cus.EmailAddress : "";
            return View(CustomerTransactions);
        }

        public ActionResult ExpenseListPartial(int CustomerId, string SearchText, int PageNo, CustomerFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId, CurrentUser.CompanyId.Value);
            if (!res)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            int PageLimit = _Util.Facade.GlobalSettingsFacade.GetCustomerFundingPageLimit(CurrentUser.CompanyId.Value);
            if (PageNo > 0)
            {
                filter.PageNo = PageNo;
            }
            if (filter.PageNo == 0)
            {
                filter.PageNo = 1;
            }

            if (filter.PageSize < PageLimit)
            {
                filter.PageSize = PageLimit;
            }
            List<TransactionExpense> CustomerTransactions = _Util.Facade.TransactionFacade.GetAllExpenseByCustomerIdAndCompanyIdAndFilter(CustomerId, CurrentUser.CompanyId.Value, SearchText, CurrentUser.UserId, filter);
            var TotalFundCount = _Util.Facade.TransactionFacade.GetAllExpenseByCustomerIdAndCompanyIdAndFilter(CustomerId, CurrentUser.CompanyId.Value, SearchText, CurrentUser.UserId);
            if (CustomerTransactions.Count() == 0)
            {
                filter.PageNo = 1;
                //customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilter(filter);
            }

            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = filter.order;

            if (CustomerTransactions.Count() > 0)
            {
                ViewBag.OutOfNumber = TotalFundCount.Count();
            }

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);
            return View(CustomerTransactions);
        }
        [Authorize]
        public PartialViewResult AddExpense(int? Id, Guid CustomerId, string From)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            TransactionExpense Expense = new TransactionExpense()
            {
                ExpenseDate = DateTime.Now.UTCCurrentTime(),
                Amount = 0,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CurrentUser.CompanyId.Value,
                UserId = CurrentUser.UserId,
            };

            if (Id.HasValue && Id.Value > 0)
            {
                Expense = _Util.Facade.TransactionFacade.GetTransactionExpenseById(Id.Value);
            }
            Expense.CustomerId = CustomerId;


            #region ViewBags 
            List<SelectListItem> StatusList = new List<SelectListItem>();
            StatusList.Add(new SelectListItem()
            {
                Text = "Select One",
                Value = "-1",
            });
            StatusList.Add(new SelectListItem()
            {
                Text = "Paid",
                Value = "Paid",
            });
            StatusList.Add(new SelectListItem()
            {
                Text = "Unpaid",
                Value = "Unpaid",

            });
            ViewBag.ExpenseStatusList = StatusList;
            ViewBag.ExpenseCategory = _Util.Facade.LookupFacade.GetLookupByKey("ExpenseCategory").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString()
               }).ToList();
            List<SelectListItem> Emplist = new List<SelectListItem>();


            Emplist = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value).OrderBy(x => x.FirstName != "Select One").ThenBy(x => x.FirstName).Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                }).ToList();


            ViewBag.EmployeeList = Emplist;
            List<SelectListItem> PaymentMethodList = new List<SelectListItem>();
            PaymentMethodList.Add(new SelectListItem()
            {
                Text = "Select One",
                Value = "-1",
            });
            PaymentMethodList.Add(new SelectListItem()
            {
                Text = "ACH",
                Value = "ACH",
            });
            PaymentMethodList.Add(new SelectListItem()
            {
                Text = "Credit Card",
                Value = "CreditCard",

            });
            PaymentMethodList.Add(new SelectListItem()
            {
                Text = "Check",
                Value = "Check",

            });
            PaymentMethodList.Add(new SelectListItem()
            {
                Text = "Cash",
                Value = "Cash",

            });
            ViewBag.ExpensePaymentMethodList = PaymentMethodList;
            ViewBag.From = "";
            if (!string.IsNullOrEmpty(From))
            {
                ViewBag.From = From;
            }

            #endregion


            return PartialView(Expense);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddExpense(TransactionExpense Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            string Message = "";
            if (Model.Id > 0)
            {
                TransactionExpense tempExp = _Util.Facade.TransactionFacade.GetTransactionExpenseById(Model.Id);
                if (tempExp == null)
                {
                    return Json(new { result = false, message = "Expense not found" });
                }

                Model.CompanyId = tempExp.CompanyId;
                Model.CreatedBy = tempExp.CreatedBy;
                Model.CreatedDate = tempExp.CreatedDate;
                Model.Type = tempExp.Type;
                _Util.Facade.TransactionFacade.UpdateTransactionExpense(Model);
                Message = "Expense updated successfully.";
                base.AddUserActivityForCustomer("Expense #ExpenseId:" + tempExp.Id + " is Updated", LabelHelper.ActivityAction.AddAdditionalContact, Model.CustomerId, null, null);

            }
            else
            {
                Model.CompanyId = CurrentUser.CompanyId.Value;
                Model.CreatedBy = CurrentUser.UserId;
                Model.CreatedDate = DateTime.Now.UTCCurrentTime();
                Model.Type = LabelHelper.TransactionExpenseType.Manual;
                _Util.Facade.TransactionFacade.InsertTransactionExpense(Model);
              
                base.AddUserActivityForCustomer("New Expense is added #ExpenseId: "+Model.Id +"</br> Expense Date: "+Model.ExpenseDate.ToString("MM/dd/yyyy"), LabelHelper.ActivityAction.AddAdditionalContact, Model.CustomerId, null, null);

                Message = "Expense added successfully.";
            }
            return Json(new { result = true, message = Message });
        }


        public ActionResult ShowAllTransactionsFilter(int CustomerId, string SearchText, CustomerFilter filter)
        {

            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerFundingList))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId, CurrentUser.CompanyId.Value);
            if (!res)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            int PageLimit = _Util.Facade.GlobalSettingsFacade.GetCustomerFundingPageLimit(CurrentUser.CompanyId.Value);
            if (filter.PageNo == 0)
            {
                filter.PageNo = 1;
            }

            if (filter.PageSize < PageLimit)
            {
                filter.PageSize = PageLimit;
            }
            List<Transaction> CustomerTransactions = _Util.Facade.TransactionFacade.GetAllTransactionsByCustomerIdAndCompanyIdAndFilter(CustomerId, CurrentUser.CompanyId.Value, SearchText, CurrentUser.UserId, filter);
            var TotalFundCount = _Util.Facade.TransactionFacade.GetAllTransactions1ByCustomerIdAndCompanyIdAndFilter(CustomerId, CurrentUser.CompanyId.Value, SearchText, CurrentUser.UserId);
            if (CustomerTransactions.Count() == 0)
            {
                filter.PageNo = 1;
                //customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilter(filter);
            }

            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = 0;


            if (CustomerTransactions.Count() > 0)
            {
                ViewBag.OutOfNumber = TotalFundCount.Count();
            }

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);
            return View("_ShowAllTransactionsFilter", CustomerTransactions);
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteTransaction(int Id, Guid? CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            TransactionHistory trh = _Util.Facade.TransactionFacade.GetTransactionHistoryById(Id);
            if (trh != null)
            {
                Transaction tr = _Util.Facade.TransactionFacade.GetTransactionById(trh.TransactionId);
                tr.Amount -= trh.Amout;
                if (tr.Amount == 0)
                {
                    _Util.Facade.TransactionFacade.DeleteTransaction(tr.Id);
                }
                else
                {
                    _Util.Facade.TransactionFacade.UpdateTransaction(tr);
                }
                CustomerCredit cc = _Util.Facade.CustomerFacade.GetCustomerCreditByTransactionId(trh.TransactionId);
                if (cc != null)
                {
                    _Util.Facade.CustomerFacade.DeleteCustomerCreditById(cc.Id);
                }
                Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(trh.InvoiceId);
                string TempStatus = "";

                if (inv != null)
                {
                    TempStatus = inv.Status;

                }
                if (inv != null)
                {
                    inv.BalanceDue += trh.Amout;

                    if (inv.BalanceDue == inv.TotalAmount)
                    {
                        inv.Status = LabelHelper.InvoiceStatus.Open;
                    }
                    else if (inv.BalanceDue < inv.TotalAmount)
                    {
                        inv.Status = LabelHelper.InvoiceStatus.Partial;
                    }

                    _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
                    if ( TempStatus != inv.Status)
                    {
                        #region log
                        int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                        bool newBool = inv.IsARBInvoice ?? false;


                        base.AddUserActivityForCustomer("Invoice Status Changed from " + TempStatus + " To " + inv.Status + "#InvoiceId: " + inv.InvoiceId, lineNumber + "," + actionName + "/" + controllerName, inv.CustomerId, null, null, newBool);
                        #endregion
                    }
                }
                _Util.Facade.TransactionFacade.DeleteTransactionHistoryById(trh.Id);
                if(inv.IsARBInvoice.HasValue && inv.IsARBInvoice.Value)
                {
                    base.AddUserActivityForCustomer("Payment for " + inv.InvoiceId + " Is Deleted", LabelHelper.ActivityAction.Delete, CustomerId, null, inv.InvoiceId, true);
                }
                else
                {
                    base.AddUserActivityForCustomer("Payment for " + inv.InvoiceId + " Is Deleted", LabelHelper.ActivityAction.Delete, CustomerId, null, null);
                }    
                return Json(new { result = true, message = "Transaction deleted successfully." });

            }
            else
            {
                return Json(new { result = false, message = "Transaction not found." });
            }


            //Transaction tr = _Util.Facade.TransactionFacade.GetTransactionById(Id);
            //if (tr != null)
            //{
            //    List<TransactionHistory> trhs = _Util.Facade.TransactionFacade.GetAllTransactionHistoryByTransacetionId(tr.Id);
            //    List<string> NewInvocieIdList = new List<string>();
            //    foreach(int InvoiceId in trhs.GroupBy(x => x.InvoiceId).Select(x => x.Key))
            //    {
            //        Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
            //        if (inv != null)
            //        {
            //            List<InvoiceDetail> invdet = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(inv.InvoiceId);
            //            inv.Status = LabelHelper.InvoiceStatus.Cancelled;
            //            _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
            //            inv.BalanceDue = inv.TotalAmount;
            //            inv.Status = LabelHelper.InvoiceStatus.Open;
            //            inv.Id = _Util.Facade.InvoiceFacade.InsertInvoice(inv);
            //            inv.InvoiceId = inv.Id.GenerateInvoiceNo();
            //            NewInvocieIdList.Add(inv.InvoiceId);
            //            inv.CreatedDate = DateTime.Now.UTCCurrentTime();
            //            inv.CreatedBy = User.Identity.Name;
            //            inv.CreatedByUid = CurrentUser.UserId;
            //            _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
            //            foreach (var item in invdet)
            //            {
            //                item.InvoiceId = inv.InvoiceId;
            //                item.CreatedBy = User.Identity.Name;
            //                item.CreatedDate = DateTime.Now.UTCCurrentTime();
            //                _Util.Facade.InvoiceFacade.InsertInvoiceDetails(item);
            //            }
            //        }
            //    }
            //    _Util.Facade.TransactionFacade.DeleteTransactionAndHistoryByTranId(tr.Id);
            //    return Json(new { result=true,message=string.Format("Transaction deleted successfully.New Invoice Id(s): {0}", string.Join(", ",NewInvocieIdList)) }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(new { result = false, message = "Transaction not found." });
            //}

        }

        public ActionResult ShowAllPaymentTransactions(int CustomerId, string SearchText)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId, CurrentUser.CompanyId.Value);
            if (!res)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            List<Transaction> CustomerTransactions = _Util.Facade.TransactionFacade.GetAllTransactions1ByCustomerIdAndCompanyIdAndFilter(CustomerId, CurrentUser.CompanyId.Value, SearchText, CurrentUser.UserId);
            return View("_ShowAllTransactions", CustomerTransactions);
        }

        [Authorize]
        public ActionResult ReceivePayment(int? CustomerId, Guid? CustomerGuid, int? InvoiceId, int? TransactionId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ReceivePaymentModel Model = new ReceivePaymentModel();
            Customer tempCustomer = new Customer();

            if (CustomerId.HasValue && CustomerId > 0)
            {
                bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId.Value, CurrentUser.CompanyId.Value);
                if (!res)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                tempCustomer = _Util.Facade.CustomerFacade.GetCustomerById(CustomerId.Value);
            }
            else if (CustomerGuid.HasValue && CustomerGuid != new Guid())
            {
                //bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerGuid.Value, CurrentUser.CompanyId.Value);
                //if (!res)
                //{
                //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                //}
                tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerGuid.Value);
            }
            else if (InvoiceId.HasValue && InvoiceId > 0)
            {
                Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId.Value);
                //GlobalSettingsFacade globsetfacade = new GlobalSettingsFacade();
                //GlobalSetting paymentGetway = globsetfacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PaymentGetway");
                //ViewBag.PaymentGetway = paymentGetway.Value;
                //ViewBag.CardType = _Util.Facade.LookupFacade.GetLookupByKey("CardType").Select(x =>
                //   new SelectListItem()
                //   {
                //       Text = x.DisplayText.ToString(),
                //       Value = x.DataValue.ToString()
                //   }).ToList();
                if (inv == null || inv.CompanyId != CurrentUser.CompanyId.Value)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(inv.CustomerId, CurrentUser.CompanyId.Value);
                if (!res)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(inv.CustomerId);

            }
            else if (!TransactionId.HasValue || TransactionId.Value == 0)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<int> InvoiceIdList = new List<int>();

            if (TransactionId.HasValue)
            {

                TransactionHistory transactionHistory = _Util.Facade.TransactionFacade.GetTransactionHistoryById(TransactionId.Value);
                if (transactionHistory != null)
                {
                    TransactionId = transactionHistory.TransactionId;

                    Transaction tempTransaction = _Util.Facade.TransactionFacade.GetTransactionByIdAndCompanyId(transactionHistory.TransactionId, CurrentUser.CompanyId.Value);


                    if (tempTransaction == null || tempTransaction.CompanyId != CurrentUser.CompanyId.Value)
                    {
                        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                    }
                    if (tempCustomer == null)
                    {
                        tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(tempTransaction.CustomerId);
                    }
                    List<TransactionHistory> trHis = _Util.Facade.TransactionFacade.GetAllTransactionHistoryByTransacetionId(transactionHistory.TransactionId);
                    InvoiceIdList = trHis.Select(x => x.InvoiceId).ToList();
                    Model.AmoutReceived = trHis.Sum(x => x.Amout);
                    Model.PaymentMethod = tempTransaction.PaymentMethod;
                    if (tempTransaction.PaymentProfileId != null && tempTransaction.PaymentProfileId > 0)
                    {
                        Model.PaymentMethod = "PP_" + tempTransaction.PaymentProfileId;
                    }

                    Model.CardInfo = new CardInfo()
                    {
                        CheckNo = tempTransaction.CheckNo
                    };
                    Model.RefNo = tempTransaction.ReferenceNo;
                    ViewBag.TransactionId = transactionHistory.TransactionId;
                }
            }
            else
            {
                Model.CardInfo = new CardInfo();
                Model.AmoutReceived = 0;
                ViewBag.TransactionId = 0;
            }

            Model.CustomerId = tempCustomer.Id;
            if (tempCustomer.Type == "Commercial" && !string.IsNullOrWhiteSpace(tempCustomer.BusinessName))
            {
                Model.CustomerName = tempCustomer.BusinessName;
            }
            else
            {
                Model.CustomerName = tempCustomer.FirstName + " " + tempCustomer.LastName;
            }
            Model.CustomerNo = tempCustomer.CustomerNo;
            Model.CustomerGId = tempCustomer.CustomerId;
            Model.PaymentDate = DateTime.Now.UTCCurrentTime();
            Model.EmailAddress = tempCustomer.EmailAddress;

            List<OutStandingTransactions> TransactionsData = _Util.Facade.TransactionFacade.GetReceivePaymentListByCustomerId(tempCustomer.Id, CurrentUser.CompanyId.Value, TransactionId);
            if (TransactionsData != null && TransactionsData.Count > 0)
            {
                Model.Transactions = TransactionsData.OrderBy(x => x.Id).ToList();
            }
            else
            {
                Model.Transactions = new List<OutStandingTransactions>();
            }
            // Model.CreditAmount = _Util.Facade.TransactionFacade.GetCustomerCreditAmountByCustomerId(tempCustomer.CustomerId);
            Model.GeneralCreditAmount = _Util.Facade.TransactionFacade.GetCustomerCreditAmountByCustomerIdWithBoolValue(tempCustomer.CustomerId, false, true);
            Model.RMRCreditAmount = _Util.Facade.TransactionFacade.GetCustomerCreditAmountByCustomerIdWithBoolValue(tempCustomer.CustomerId, true, false);

            #region ViewBags

            if (InvoiceId.HasValue)
            {
                InvoiceIdList.Add(InvoiceId.Value);
                ViewBag.InvoiceId = InvoiceId.Value;
            }
            else
            {
                ViewBag.InvoiceId = 0;
            }

            if (InvoiceIdList.Count() > 0)
            {
                ViewBag.InvoiceIdList = InvoiceIdList;
            }
            if (CurrentUser.UserTags == "Customer")
            {
                List<SelectListItem> PaymentMethods = new List<SelectListItem>();
                PaymentMethods.Add(new SelectListItem()
                {
                    Text = "Please Select One",
                    Value = "-1"
                });
                PaymentMethods.Add(new SelectListItem()
                {
                    Text = "ACH",
                    Value = "ACH"
                });
                PaymentMethods.Add(new SelectListItem()
                {
                    Text = "Credit Card",
                    Value = "Credit Card"
                });
                if (!string.IsNullOrWhiteSpace(tempCustomer.AuthorizeRefId))
                {
                    PaymentMethods.Add(new SelectListItem()
                    {
                        Text = string.Format("Customer Payment Profile"),
                        Value = "CustomerProfile"
                    });
                }

                if (base.IsPermitted(UserPermissions.CustomerPermissions.ReceivePaymentCreditMemo))
                {
                    PaymentMethods.Add(new SelectListItem()
                    {
                        Text = string.Format("Credit Memo"),
                        Value = "Credit Memo"
                    });
                }
                if (Model.GeneralCreditAmount > 0)
                {
                    PaymentMethods.Add(new SelectListItem()
                    {
                        Text = string.Format("General Credit [{0}{1}]", LabelHelper.CurrentTransMakeCurrency.MakeCurrency(CurrentUser.CompanyId.Value), string.Format("{0:0,0.00}", Model.GeneralCreditAmount)),
                        Value = "General Customer Credit"
                    });
                }
                if (Model.RMRCreditAmount > 0)
                {
                    PaymentMethods.Add(new SelectListItem()
                    {
                        Text = string.Format("RMR Credit [{0}{1}]", LabelHelper.CurrentTransMakeCurrency.MakeCurrency(CurrentUser.CompanyId.Value), string.Format("{0:0,0.00}", Model.RMRCreditAmount)),
                        Value = "RMRCustomerCredit"
                    });
                }
                List<PaymentProfileCustomer> PaymentProfileList = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentProfileCustomerByCustomerId(Model.CustomerGId);
                if (PaymentProfileList != null && PaymentProfileList.Where(x => x.Type != "Invoice" && x.Type != "Financed" && x.Type != "Cash" && x.Type.IndexOf("CHK_") == -1).Count() > 0)
                {
                    PaymentProfileList = PaymentProfileList.Where(x => x.Type != "Invoice" && x.Type != "Financed" && x.Type != "Cash" && x.Type.IndexOf("CHK_") == -1).ToList();
                    foreach (var item in PaymentProfileList)
                    {
                        PaymentMethods.Add(new SelectListItem()
                        {
                            Text = item.Type,
                            Value = "PP_" + item.Id
                        });
                    }

                }

                ViewBag.PaymentMethods = PaymentMethods;
            }
            else
            {
                List<SelectListItem> Paymentmethods = _Util.Facade.LookupFacade.GetLookupByKey("PaymentMethodInvoice").Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString()
                         }).ToList();

                if (!string.IsNullOrWhiteSpace(tempCustomer.AuthorizeRefId))
                {
                    Paymentmethods.Add(new SelectListItem()
                    {
                        Text = string.Format("Customer Payment Profile"),
                        Value = "CustomerProfile"
                    });
                }
                if (base.IsPermitted(UserPermissions.CustomerPermissions.ReceivePaymentCreditMemo))
                {
                    Paymentmethods.Add(new SelectListItem()
                    {
                        Text = string.Format("Credit Memo"),
                        Value = "Credit Memo"
                    });
                }
                if (base.IsPermitted(UserPermissions.CustomerPermissions.PaymentMethodOthers))
                {
                    Paymentmethods.Add(new SelectListItem()
                    {
                        Text = string.Format("Others"),
                        Value = "Others"
                    });
                }
                if (Model.GeneralCreditAmount > 0)
                {
                    Paymentmethods.Add(new SelectListItem()
                    {
                        Text = string.Format("General Credit [{0}{1}]", LabelHelper.CurrentTransMakeCurrency.MakeCurrency(CurrentUser.CompanyId.Value), string.Format("{0:0,0.00}", Model.GeneralCreditAmount)),
                        Value = "General Customer Credit"
                    });
                }
                if (Model.RMRCreditAmount > 0)
                {
                    Paymentmethods.Add(new SelectListItem()
                    {
                        Text = string.Format("RMR Credit [{0}{1}]", LabelHelper.CurrentTransMakeCurrency.MakeCurrency(CurrentUser.CompanyId.Value), string.Format("{0:0,0.00}", Model.RMRCreditAmount)),
                        Value = "RMRCustomerCredit"
                    });
                }
                List<PaymentProfileCustomer> PaymentProfileList = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentProfileCustomerByCustomerId(Model.CustomerGId);
                if (PaymentProfileList != null && PaymentProfileList.Where(x => x.Type != "Invoice" && x.Type != "Financed" && x.Type != "Cash" && x.Type.IndexOf("CHK_") == -1).Count() > 0)
                {
                    PaymentProfileList = PaymentProfileList.Where(x => x.Type != "Invoice" && x.Type != "Financed" && x.Type != "Cash" && x.Type.IndexOf("CHK_") == -1).ToList();
                    foreach (var item in PaymentProfileList)
                    {
                        Paymentmethods.Add(new SelectListItem()
                        {
                            Text = item.Type,
                            Value = "PP_" + item.Id
                        });
                    }

                }
                ViewBag.PaymentMethods = Paymentmethods;
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
            ViewBag.CustomerId = CustomerId;

            #endregion

            return View("_ReceivePayment", Model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult ReceivePayment(ReceivePaymentModel Model)
        {
            string methodtype = "";
            long PaymentInfoId = 0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Model.CompanyId = CurrentUser.CompanyId.Value;
            Model.CompanyName = CurrentUser.CompanyName;
            string TransactionId = "";
            bool TransactionSuccess = true;
            bool ChargeCustomerCreditAcount = false;
            string InvoiceList = "";
            Model.Description = string.Format("Received by {0}", CurrentUser.GetFullName());
            ///*double AmountPaid = Model.Transactions.Count() == 0 ? 0 : Model.Transactions.Sum(x => x.Payment);*/

            double AmountPaid = Math.Round(Model.AmoutReceived, 2);
            //AmountPaid = Math.Round(AmountPaid, 2);
            #region Customer Credit Check
            double InvoiceAmount = Model.Transactions.Count() == 0 ? 0 : Math.Round(Model.Transactions.Sum(x => x.Payment),2);
            //InvoiceAmount = Math.Round(InvoiceAmount, 2);
            double GeneralCreditBalance = Math.Round(Model.GeneralCreditAmount, 2);
            if (GeneralCreditBalance > 0 && AmountPaid > GeneralCreditBalance)
            {
                double CreditBalance = Math.Round(Model.GeneralCreditAmount, 2);
                if (Model.Transactions != null && Model.Transactions.Count > 1)
                {
                    Model.Transactions = Model.Transactions.OrderBy(x => x.Id).ToList();
                    foreach (var item in Model.Transactions)
                    {
                        if (item.Payment <= CreditBalance)
                        {
                            CreditBalance -= Math.Round(item.Payment, 2);
                        }
                        else
                        {
                            item.Payment = Math.Round(CreditBalance, 2);
                            CreditBalance = 0;
                        }
                    }
                    Model.GeneralCreditAmount = Math.Round(CreditBalance, 2);
                }
                else if (Model.Transactions != null && Model.Transactions.Count == 1)
                {
                    Model.Transactions[0].Payment = Math.Round(CreditBalance, 2);
                    Model.GeneralCreditAmount = 0;
                }
                AmountPaid = GeneralCreditBalance;
                InvoiceAmount = GeneralCreditBalance;
            }
            else
            {
                Model.GeneralCreditAmount = 0;
            }
            double CreditAmount = AmountPaid - InvoiceAmount;
            #endregion

            Customer CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.CustomerId);
            #region Insert Into Transaction Queue
            string Starttime = DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss");
            string Endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            List<TransactionQueue> transqueList = new List<TransactionQueue>();
            transqueList = _Util.Facade.TransactionFacade.GetTransactionQueueCustomerId(CustomerDetails.CustomerId, Starttime, Endtime, AmountPaid);
            if (transqueList.Count > 0)
            {
                return Json(new { result = false, transactionSuccess = false, message = "Duplicate transection requested with same amount. Please try after 1 minute." });
            }
            else
            {
                TransactionQueue transque = new TransactionQueue();
                transque.CustomerId = CustomerDetails.CustomerId;
                transque.Amount = AmountPaid;
                transque.InvoiceId = Model.InvoiceList;
                transque.CreatedBy = CurrentUser.UserId;
                transque.CreatedDate = DateTime.Now;
                _Util.Facade.TransactionFacade.InsertTransactionQueue(transque);
            }

            #endregion

            if (Model.PaymentMethod == "General Customer Credit" && Math.Round(CreditAmount, 2) != Math.Round(Model.GeneralCreditAmount, 2))
            {
                return Json(new { result = false, transactionSuccess = false, message = "Credit amount missmatch." });
            }
            if (Model.PaymentMethod == "RMRCustomerCredit" && Math.Round(CreditAmount, 2) != Math.Round(Model.RMRCreditAmount, 2))
            {
                return Json(new { result = false, transactionSuccess = false, message = "Credit amount missmatch." });
            }
            if (AmountPaid == 0)
            {
                return Json(new { result = true, transactionSuccess = false, message = "Amount can't be zero." });
            }
            if (CurrentUser.UserTags == "Customer")
            {
                if (Model.PaymentMethod != "ACH"
                    && Model.PaymentMethod != "Credit Card"
                    && Model.PaymentMethod != "Debit Card"
                    && Model.PaymentMethod != "General Customer Credit"
                    && Model.PaymentMethod != "RMRCustomerCredit"
                    && Model.PaymentMethod != "CustomerProfile")
                {
                    return Json(new { result = true, transactionSuccess = false, message = "Invalid payment method." });
                }
            }

            int PaymentProfileId = 0;
            #region  If paid from Payment profile

            if (Model.PaymentMethod.IndexOf("PP_") > -1)
            {
                String[] PPId = Model.PaymentMethod.Split('_');
                
                if (PPId.Length == 2 && int.TryParse(PPId[1], out PaymentProfileId) && PaymentProfileId > 0)
                {

                    PaymentProfileCustomer PPC = _Util.Facade.PaymentInfoCustomerFacade.PaymentProfileCustomerById(PaymentProfileId);
                    if (PPC == null || PPC.CustomerId != CustomerDetails.CustomerId)
                    {
                        logger.WithProperty("tags", "invoice,payment,failure").WithProperty("params", JsonConvert.SerializeObject(Model))
                        .Trace($"Payment by {CurrentUser.GetFullName()} failed, Invalid payment method. Error Code: E-P001");
                        return Json(new { result = true, transactionSuccess = false, message = "Invalid payment method. Error Code: E-P001" });
                    }
                    PaymentInfo Pinfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoById(PPC.PaymentInfoId);
                    if (Pinfo == null)
                    {
                        logger.WithProperty("tags", "invoice,payment,failure").WithProperty("params", JsonConvert.SerializeObject(Model))
                        .Trace($"Payment by {CurrentUser.GetFullName()} failed, Invalid payment method. Error Code: E-P002");
                        return Json(new { result = true, transactionSuccess = false, message = "Invalid payment method. Error Code: E-P002" });
                    }

                    if (!string.IsNullOrWhiteSpace(Pinfo.AccountName)
                        && !string.IsNullOrWhiteSpace(Pinfo.BankAccountType)
                        && !string.IsNullOrWhiteSpace(Pinfo.RoutingNo)
                        && !string.IsNullOrWhiteSpace(Pinfo.AcountNo))
                    {
                        Model.ACHInfo = new ACHInfo();
                        Model.ACHInfo.AccountName = Pinfo.AccountName;
                        Model.ACHInfo.AccountType = Pinfo.BankAccountType;
                        Model.ACHInfo.RoutingNo = Pinfo.RoutingNo;
                        Model.ACHInfo.AccountNo = Pinfo.AcountNo;
                        Model.ACHInfo.ECheckType = Pinfo.EcheckType;
                        Model.ACHInfo.BankName = Pinfo.BankName;

                        Model.ACHInfo.Company = CustomerDetails.BusinessName;
                        Model.ACHInfo.Phone = CustomerDetails.PrimaryPhone;
                        Model.ACHInfo.BillingAddress = CustomerDetails.Street;
                        Model.ACHInfo.City = CustomerDetails.City;
                        Model.ACHInfo.State = CustomerDetails.State;
                        Model.ACHInfo.Zipcode = CustomerDetails.ZipCode;

                        Model.ACHInfo.PaymentProfileId = PaymentProfileId;

                        Model.PaymentMethod = LabelHelper.PaymentMethod.ACH;
                    }
                    else if (!string.IsNullOrWhiteSpace(Pinfo.AccountName)
                        && !string.IsNullOrWhiteSpace(Pinfo.CardNumber)
                        && !string.IsNullOrWhiteSpace(Pinfo.CardExpireDate)
                        && !string.IsNullOrWhiteSpace(Pinfo.CardSecurityCode))
                    {
                        Model.CardInfo = new CardInfo();
                        Model.CardInfo.CardNumber = Pinfo.CardNumber;
                        Model.CardInfo.ExpiredDate = Pinfo.CardExpireDate;
                        Model.CardInfo.SecurityCode = Pinfo.CardSecurityCode;
                        Model.CardInfo.NameOnCard = Pinfo.AccountName;
                        Model.CardInfo.CardType = Pinfo.CardType;

                        Model.CardInfo.Company = CustomerDetails.BusinessName;
                        Model.CardInfo.Phone = CustomerDetails.PrimaryPhone;
                        Model.CardInfo.BillingAddress = CustomerDetails.Street;
                        Model.CardInfo.City = CustomerDetails.City;
                        Model.CardInfo.State = CustomerDetails.State;
                        Model.CardInfo.Zipcode = CustomerDetails.ZipCode;

                        Model.CardInfo.PaymentProfileId = PaymentProfileId;

                        Model.PaymentMethod = LabelHelper.PaymentMethod.CreditCard;
                    }
                    else
                    {
                        logger.WithProperty("tags", "invoice,payment,failure").WithProperty("params", JsonConvert.SerializeObject(Model))
                        .Trace($"Payment by {CurrentUser.GetFullName()} failed, Invalid payment method. Error Code: E-P002");
                        return Json(new { result = true, transactionSuccess = false, message = "Invalid payment method. Error Code: E-P002" });
                    }

                }
                else
                {
                    logger.WithProperty("tags", "invoice,payment,failure").WithProperty("params", JsonConvert.SerializeObject(Model))
                        .Trace($"Payment by {CurrentUser.GetFullName()} failed, Invalid payment method. Error Code: E-P003");
                    return Json(new { result = true, transactionSuccess = false, message = "Invalid payment method. Error Code: E-P003" });
                }
            }
            
            #endregion

            #region Set InvoiceNo
            List<Invoice> ListInvoice = new List<Invoice>();
            foreach (var item in Model.Transactions)
            {
                Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(item.Id);
                if (inv.Status == "Paid")
                {
                    logger.WithProperty("tags", "invoice,payment,failure").WithProperty("params", JsonConvert.SerializeObject(Model))
                        .Trace($"Payment by {CurrentUser.GetFullName()} failed, Transaction unsuccessful. Some of invoice already paid, please refresh your browser.");
                    return Json(new { result = false, transactionSuccess = TransactionSuccess, message = "Transaction unsuccessful. Some of invoice already paid, please refresh your browser." });

                }
                ListInvoice.Add(inv);
                string balanceduestr = string.Format("{0:#.00}", inv.BalanceDue);
                string Paymentstr = string.Format("{0:#.00}", inv.BalanceDue);
                double BalaceDue = 0.0;
                double Payment = 0.0;
                double.TryParse(balanceduestr, out BalaceDue);
                double.TryParse(Paymentstr, out Payment);
                inv.BalanceDue = BalaceDue - Payment;
                if (inv.BalanceDue < 0)
                {
                    logger.WithProperty("tags", "invoice,payment,failure").WithProperty("params", JsonConvert.SerializeObject(Model))
                        .Trace($"Payment by {CurrentUser.GetFullName()} failed, Transaction unsuccessful. Balance can't be less than 0.");
                    return Json(new { result = false, transactionSuccess = TransactionSuccess, message = "Transaction unsuccessful. Balance can't be less than 0." });
                }
                string NewInvStr = InvoiceList + item.Id.GenerateInvoiceNo();
                if (NewInvStr.Length <= 20)
                {
                    InvoiceList += item.Id.GenerateInvoiceNo();
                }
            }
            Model.InvoiceList = InvoiceList;
            #endregion

            #region Payment Method Ready
            string paymentmethod = Model.PaymentMethod.Trim().ToLower().Replace(" ", "");
            bool onlinetransaction = (paymentmethod == "creditcard" || paymentmethod == "debitcard" || paymentmethod == "ach" || paymentmethod == "customerprofile") ? true : false;

            if (AmountPaid > 0 && (paymentmethod == "creditcard"
                || paymentmethod == "debitcard"))
            {
                if (Model.CardInfo == null)
                {
                    logger.WithProperty("tags", "invoice,payment,failure").WithProperty("params", JsonConvert.SerializeObject(Model))
                        .Trace($"Payment by {CurrentUser.GetFullName()} failed, Card info required.");
                    return Json(new { result = false, transactionSuccess = TransactionSuccess, message = "Card info required." });
                }
                Model.CardInfo.Amount = AmountPaid;
                Model.CardInfo.FirstName = CustomerDetails.FirstName;
                Model.CardInfo.Lastname = CustomerDetails.LastName;
                Model.CardInfo.CustomerId = CustomerDetails.Id.ToString();
                Model.CardInfo.EmailAddress = CustomerDetails.EmailAddress;
                Model.CardInfo.InvoiceNo = InvoiceList;

                Model.CardInfo.Company = CustomerDetails.BusinessName;
                Model.CardInfo.Phone = CustomerDetails.PrimaryPhone;
                Model.CardInfo.BillingAddress = CustomerDetails.Street;
                Model.CardInfo.City = CustomerDetails.City;
                Model.CardInfo.State = CustomerDetails.State;
                Model.CardInfo.Zipcode = CustomerDetails.ZipCode;

                if (!string.IsNullOrWhiteSpace(Model.CardInfo.Description))
                {
                    Model.Description = Model.CardInfo.Description;
                }
                Model.CardInfo.Description = string.Format("Received by {0}", CurrentUser.GetFullName());
                string cardno = DESEncryptionDecryption.DecryptCipherTextToPlainText(Model.CardInfo.CardNumber);


                PaymentProfileCustomer paymentProfileCustomer = _Util.Facade.ReceivePaymentFacade.GetPaymentProfileCustomerByPaymentInfo(Model);

                methodtype = "CC" + "_" + Model.CardInfo.NameOnCard + "_" + cardno.Substring(Math.Max(0, cardno.Length - 4));

                if (paymentProfileCustomer != null)
                {
                    if (ListInvoice != null && ListInvoice.Count > 0)
                    {
                        foreach (var item in ListInvoice)
                        {
                            var objinv = _Util.Facade.InvoiceFacade.GetInvoiceById(item.Id);
                            if (objinv != null)
                            {
                                objinv.PaymentType = paymentProfileCustomer.Type;
                                _Util.Facade.InvoiceFacade.UpdateInvoice(objinv);
                            }
                        }
                    }

                    PaymentProfileId = paymentProfileCustomer.Id;
                }

                #region Insert PaymentInfo and PaymentProfile
                //var objpayprofile = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentProfileByTypeAndCustomerId(methodtype, CustomerDetails.CustomerId);
                //if (objpayprofile == null)
                //{
                //    PaymentInfo pInfo = new PaymentInfo()
                //    {
                //        CompanyId = CurrentUser.CompanyId.Value,
                //        AccountName = Model.CardInfo.NameOnCard,
                //        CardType = Model.CardInfo.CardType,
                //        CardNumber = Model.CardInfo.CardNumber,
                //        CardExpireDate = Model.CardInfo.ExpiredDate,
                //        CardSecurityCode = Model.CardInfo.SecurityCode
                //    };
                //    PaymentInfoId = _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(pInfo);

                //    PaymentProfileCustomer PaymentProfileCustomer = new PaymentProfileCustomer()
                //    {
                //        CompanyId = CurrentUser.CompanyId.Value,
                //        CustomerId = CustomerDetails.CustomerId,
                //        PaymentInfoId = pInfo.Id,
                //        Type = methodtype
                //    };
                //    _Util.Facade.CustomerFacade.InsertPaymentProfileCustomer(PaymentProfileCustomer);
                //}
                #endregion

            }
            else if (AmountPaid > 0 && paymentmethod == "ach")
            {
                if (Model.ACHInfo == null)
                {
                    return Json(new { result = false, transactionSuccess = TransactionSuccess, message = "ACH info required." });
                }
                Model.ACHInfo.Amount = AmountPaid;
                Model.ACHInfo.FirstName = CustomerDetails.FirstName;
                Model.ACHInfo.Lastname = CustomerDetails.LastName;
                Model.ACHInfo.CustomerId = CustomerDetails.Id.ToString();
                Model.ACHInfo.EmailAddress = CustomerDetails.EmailAddress;
                Model.ACHInfo.InvoiceNo = InvoiceList;

                Model.ACHInfo.Company = CustomerDetails.BusinessName;
                Model.ACHInfo.Phone = CustomerDetails.PrimaryPhone;
                Model.ACHInfo.BillingAddress = CustomerDetails.Street;
                Model.ACHInfo.City = CustomerDetails.City;
                Model.ACHInfo.State = CustomerDetails.State;
                Model.ACHInfo.Zipcode = CustomerDetails.ZipCode;

                if (!string.IsNullOrWhiteSpace(Model.ACHInfo.Description))
                {
                    Model.Description = Model.ACHInfo.Description;
                }
                Model.ACHInfo.Description = string.Format("Received by {0}", CurrentUser.GetFullName());
                //methodtype = "ACH" + "_" + Model.ACHInfo.AccountName + "_" + Model.ACHInfo.AccountNo.Substring(Math.Max(0, Model.ACHInfo.AccountNo.Length - 2));

                PaymentProfileCustomer paymentProfileCustomer = _Util.Facade.ReceivePaymentFacade.GetPaymentProfileCustomerByPaymentInfo(Model);
                
                if (paymentProfileCustomer != null)
                {
                    if (ListInvoice != null && ListInvoice.Count > 0)
                    {
                        foreach (var item in ListInvoice)
                        {
                            var objinv = _Util.Facade.InvoiceFacade.GetInvoiceById(item.Id);
                            if (objinv != null)
                            {
                                objinv.PaymentType = paymentProfileCustomer.Type;
                                _Util.Facade.InvoiceFacade.UpdateInvoice(objinv);
                            }
                        }
                    }

                    PaymentProfileId = paymentProfileCustomer.Id;
                }

                #region  Insert Payment Info and payment profile

                //var objpayprofile = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentProfileByTypeAndCustomerId(methodtype, CustomerDetails.CustomerId);
                //if (objpayprofile == null)
                //{
                //    PaymentInfo pInfo = new PaymentInfo()
                //    {
                //        CompanyId = CurrentUser.CompanyId.Value,
                //        AccountName = Model.ACHInfo.AccountName,
                //        BankAccountType = Model.ACHInfo.AccountType,
                //        RoutingNo = Model.ACHInfo.RoutingNo,
                //        AcountNo = Model.ACHInfo.AccountNo,
                //        EcheckType = Model.ACHInfo.ECheckType,
                //        BankName = Model.ACHInfo.BankName
                //    };
                //    PaymentInfoId = _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(pInfo);
                //    PaymentProfileCustomer PaymentProfileCustomer = new PaymentProfileCustomer()
                //    {
                //        CompanyId = CurrentUser.CompanyId.Value,
                //        CustomerId = CustomerDetails.CustomerId,
                //        PaymentInfoId = pInfo.Id,
                //        Type = methodtype
                //    };
                //    _Util.Facade.CustomerFacade.InsertPaymentProfileCustomer(PaymentProfileCustomer);
                //}
                #endregion
            }
            else if (Model.PaymentMethod == "General Customer Credit")
            {
                // double CustomerCreditAmount = _Util.Facade.TransactionFacade.GetCustomerCreditAmountByCustomerId(CustomerDetails.CustomerId);
                double CustomerCreditAmount = Math.Round(_Util.Facade.TransactionFacade.GetCustomerCreditAmountByCustomerIdWithBoolValue(CustomerDetails.CustomerId, false, true), 2);
                if (CustomerCreditAmount < Math.Round(AmountPaid, 2) )
                {
                    return Json(new { result = false, transactionSuccess = false, message = "Insufficient funds." });
                }
                else
                {
                    ChargeCustomerCreditAcount = true;
                }
            }
            else if (Model.PaymentMethod == "RMRCustomerCredit")
            {
                double CustomerCreditAmount = Math.Round(_Util.Facade.TransactionFacade.GetCustomerCreditAmountByCustomerIdWithBoolValue(CustomerDetails.CustomerId, true, false), 2);
                if (CustomerCreditAmount < Math.Round(AmountPaid, 2))
                {
                    return Json(new { result = false, transactionSuccess = false, message = "Insufficient funds." });
                }
                else
                {
                    ChargeCustomerCreditAcount = true;
                  
                }
            }
            else if (Model.PaymentMethod == "CustomerProfile")
            {
                if (string.IsNullOrWhiteSpace(CustomerDetails.AuthorizeRefId))
                {
                    return Json(new { result = false, transactionSuccess = false, message = "Authorize.net subscription id required." });
                }
                Model.AuthorizeSubscriptionId = CustomerDetails.AuthorizeRefId;
                Model.CustomerProfileId = CustomerDetails.AuthorizeCusProfileId;
                Model.CustomerPaymentProfileId = CustomerDetails.AuthorizeCusPaymentProfileId;
            }
            else if (Model.PaymentMethod == "Others" && Model.OthersInfo != null)
            {
                Model.PaymentMethod = Model.OthersInfo.PaymentMethodOthers;
                TransactionId = Model.OthersInfo.ConfirmationNumber;
            }
            #endregion

            #region Receiving The payment
            ReceivePaymentResponse response = new ReceivePaymentResponse();
            if (onlinetransaction)
            {
                response = _Util.Facade.ReceivePaymentFacade.ReceivePayment(Model);
                if (!response.TransactionSuccess)
                    return Json(new { result = false, transactionSuccess = TransactionSuccess, message = response.Message });
                else
                {
                    TransactionId = response.TransactionId;
                }
            }
            #endregion
            
            #region Insert Transaction
            if (Model.Transactions.Count() > 0)
            {
                #region Transaction Count gt 0

                List<int> invoiceIdList = Model.Transactions.Select(x => x.Id).ToList();
                string reference = "";
                foreach (var item in invoiceIdList)
                {
                    reference += item.GenerateInvoiceNo() + " ";
                }
                if (Model.CardInfo == null)
                {
                    Model.CardInfo = new CardInfo();
                }

                Transaction tr = new Transaction()
                {
                    Status = "Closed",
                    Type = "Payment",
                    TransacationDate = DateTime.Now.UTCCurrentTime(),
                    CustomerId = Model.CustomerGId,
                    CompanyId = CurrentUser.CompanyId.Value,
                    Amount = Math.Round(AmountPaid, 2),
                    CardTransactionId = TransactionId,
                    PaymentMethod = Model.PaymentMethod,
                    ReferenceNo = string.IsNullOrWhiteSpace(TransactionId) ? reference : TransactionId,
                    CheckNo = Model.CardInfo.CheckNo,
                    AddedBy = CurrentUser.UserTags == "Customer" ? "Customer" : User.Identity.Name,
                    AddedDate = DateTime.Now.UTCCurrentTime(),
                    CreatedBy = CurrentUser.UserId,
                    PaymentInfoId = Convert.ToInt32(PaymentInfoId),
                    PaymentProfileId = PaymentProfileId,
                };
                tr.Id = _Util.Facade.TransactionFacade.InsertTransaction(tr);

                string PaymentLogMessage = "Payment received for " + reference + " by " + Model.PaymentMethod.ToLower();
                base.AddUserActivityForCustomer(PaymentLogMessage, LabelHelper.ActivityAction.PaymentReceived, Model.CustomerGId, null, null);
                List<TransactionHistory> trhistory = new List<TransactionHistory>();
                string CreditNote = "";
                foreach (var item in Model.Transactions)
                {
                    Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(item.Id);
                    if (string.IsNullOrWhiteSpace(CreditNote))
                    {
                        CreditNote = string.Format(@"Invoice# <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{0}')"">{1}</a>", item.Id, item.InvoiceId);
                    }
                    else
                    {
                        CreditNote = CreditNote + ",<br />" + string.Format(@"<a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{0}')"">{1}</a>", item.Id, item.InvoiceId);
                    }
                    trhistory.Add(new TransactionHistory()
                    {
                        Amout = Math.Round(item.Payment, 2),
                        InvoiceId = item.Id,
                        TransactionId = tr.Id,
                        Balance = Math.Round(inv.BalanceDue.HasValue ? inv.BalanceDue.Value : 0, 2),
                        ReceivedBy = CurrentUser.UserTags == "Customer" ? new Guid("11111111-1111-1111-1111-111111111111") : CurrentUser.UserId,
                        InvoiceTotal = inv.TotalAmount.HasValue ? inv.TotalAmount.Value : 0,
                        InvoiceBalanceDue = inv.BalanceDue.HasValue ? inv.BalanceDue.Value : 0
                    });

                    // old rounding method

                    //  double DueBalance = Math.Round(inv.BalanceDue.HasValue ? inv.BalanceDue.Value : 0, 2);
                    //  inv.BalanceDue = Math.Round(DueBalance - Math.Round(item.Payment, 2), 2);

                    //



                    // new rounding method : mayur

                    double DueBalance = Math.Round(inv.BalanceDue.HasValue ? inv.BalanceDue.Value : 0, 2);

                    double roundedPayment = Math.Round(item.Payment, 2, MidpointRounding.AwayFromZero);

                    inv.BalanceDue = Math.Round(DueBalance - roundedPayment, 2); 

                    //


                    //TRACE:Log added to check the BalanceDue value changes

                    if (inv.BalanceDue.Value>0)
                    {
                        logger.WithProperty("tags", "invoice,payment,balance")
                        .WithProperty("params", string.Format($"Amt {DueBalance} | Paid {item.Payment} | Due {inv.BalanceDue} | Bal {inv.Balance}"))
                        .Trace($"Payment for Inv {item.Id} - {inv.Id} {inv.InvoiceId}");
                    }
                    if (item.Payment > 0)
                    {
                        if (inv.BalanceDue == 0)
                        {
                            inv.Status = LabelHelper.InvoiceStatus.Paid;
                        }
                        else
                        {
                            inv.Status = LabelHelper.InvoiceStatus.Partial;
                        }
                    }
                    inv.PaymentType = Model.PaymentMethod;
                    _Util.Facade.InvoiceFacade.UpdateInvoice(inv);

                    CustomerSnapshot objInvoicePayment = new CustomerSnapshot()
                    {
                        CustomerId = CustomerDetails.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        Description = "Invoice " + string.Format("<a onclick=OpenTopToBottomModal('{3}/Invoice/AddInvoice?id={0}&CustomerId={1}') style='cursor: pointer;'>", inv.Id, CustomerDetails.Id, CustomerDetails.CustomerId, AppConfig.DomainSitePath) + "<b>" + inv.InvoiceId + "</b>" + "</a>" + " created for " + "<b>" + CustomerDetails.FirstName + " " + CustomerDetails.LastName + "</b>",
                        Logdate = DateTime.Now.UTCCurrentTime(),
                        Updatedby = CurrentUser.FirstName + " " + CurrentUser.LastName,
                        Type = "InvoicePaymentHistory"
                    };
                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objInvoicePayment);
                }
                #region Customer Credit Amount
                if (ChargeCustomerCreditAcount)
                {
                    List<CustomerCredit> cusCreditList = _Util.Facade.CustomerFacade.GetCustomerCreditByTransactionIdAndCustomerId(0, Model.CustomerGId).OrderBy(x => x.Id).ToList();
                    if(cusCreditList != null && cusCreditList.Count > 0)
                    {
                        foreach(var item in cusCreditList)
                        {
                            if( AmountPaid != 0)
                            {
                                var DabitAmount = 0.0;
                                var AvailableAmount = 0.0;
                                if(item.DebitRefId != null && item.DebitRefId.HasValue)
                                {
                                    CustomerCredit cusDabit = _Util.Facade.CustomerFacade.GetCustomerCreditById(item.DebitRefId.Value);
                                    AvailableAmount = item.Amount + cusDabit.Amount;
                                    if(AvailableAmount > 0)
                                    {
                                        if (AvailableAmount >= AmountPaid)
                                        {
                                            DabitAmount = AmountPaid;
                                        }
                                        else
                                        {
                                            DabitAmount = AvailableAmount;
                                        }
                                        cusDabit.Amount = cusDabit.Amount + Math.Round(DabitAmount * -1, 2);
                                        AmountPaid = AmountPaid - DabitAmount;
                                        if (Model.PaymentMethod == "RMRCustomerCredit") { cusDabit.IsRMRCredit = true; }
                                        _Util.Facade.CustomerFacade.UpdateCustomerCredit(cusDabit);
                                    }
                                }
                                else
                                {
                                    if (item.Amount >= AmountPaid)
                                    {
                                        DabitAmount = AmountPaid;
                                    }
                                    else
                                    {
                                        DabitAmount = item.Amount;
                                    }
                                    CustomerCredit CustomerCreditDebit = new CustomerCredit()
                                    {
                                        Amount = Math.Round(DabitAmount * -1, 2),
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        CreatedBy = CurrentUser.UserId,
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        CustomerId = CustomerDetails.CustomerId,
                                        TransactionId = tr.Id,
                                        Type = LabelHelper.CustomerCreditType.Debit,
                                        Note = CreditNote
                                    };
                                    AmountPaid = AmountPaid - DabitAmount;
                                    if (Model.PaymentMethod == "RMRCustomerCredit") { CustomerCreditDebit.IsRMRCredit = true; }
                                    int debitRefId = _Util.Facade.TransactionFacade.InsertCustomerCredit(CustomerCreditDebit);
                                    item.DebitRefId = debitRefId;
                                    _Util.Facade.TransactionFacade.UpdateCustomerCredit(item);
                                }
                            }
                         
                        }
                    }

                   
                }

                if (CreditAmount > 0)
                {
                    CustomerCredit CusCredit = new CustomerCredit()
                    {
                        Amount = Math.Round(CreditAmount, 2),
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedBy = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = CustomerDetails.CustomerId,
                        TransactionId = tr.Id,
                        Type = LabelHelper.CustomerCreditType.Credit,
                        Note = CreditNote
                    };
                    if (Model.PaymentMethod == "RMRCustomerCredit") { CusCredit.IsRMRCredit = true; }
                    _Util.Facade.TransactionFacade.InsertCustomerCredit(CusCredit);
                }
                #endregion

                if (trhistory.Count() > 0)
                {
                    _Util.Facade.TransactionFacade.InsertTransactionHistoryList(trhistory);
                    if (CustomerDetails != null)
                    {
                        SetupLeadCustormer SetupLeadCustormer = new SetupLeadCustormer();
                        SetupLeadCustormer.CompanyName = CurrentUser.CompanyName;
                        SetupLeadCustormer.CustomerName = CustomerDetails.FirstName + " " + CustomerDetails.LastName;
                        if (!string.IsNullOrWhiteSpace(CustomerDetails.BusinessName))
                        {
                            SetupLeadCustormer.CustomerName = CustomerDetails.BusinessName;
                        }
                        SetupLeadCustormer.CustomerNo = CustomerDetails.CustomerNo;
                        SetupLeadCustormer.ToEmail = CustomerDetails.EmailAddress;
                        SetupLeadCustormer.CustomerId = CustomerDetails.CustomerId.ToString();
                        SetupLeadCustormer.EmployeeId = CurrentUser.UserId.ToString();
                        if (response != null && !string.IsNullOrWhiteSpace(response.TransactionId))
                        {
                            SetupLeadCustormer.TransactionId = response.TransactionId;
                        }
                        else if (Model.PaymentMethod == LabelHelper.PaymentMethod.Check)
                        {
                            SetupLeadCustormer.TransactionId = Model.CardInfo.CheckNo;
                        }

                        Guid SalesPersonId = new Guid();
                        if (Guid.TryParse(CustomerDetails.Soldby, out SalesPersonId) && SalesPersonId != new Guid())
                        {
                            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(SalesPersonId);
                            if (emp != null)
                            {
                                SetupLeadCustormer.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);
                                if (emp.Email.IsValidEmailAddress())
                                {
                                    SetupLeadCustormer.ToSalesPersonsEmail = emp.Email;
                                }
                                else if (emp.UserName.IsValidEmailAddress())
                                {
                                    SetupLeadCustormer.ToSalesPersonsEmail = emp.UserName;
                                }
                            }
                        }
                        //SetupLeadCustormer.EmailBody = emailbody;
                        if (CustomerDetails.IsReceivePaymentMail == true)
                        {
                            foreach (var item in trhistory)
                            {
                                #region Send Email for each transaction
                                SetupLeadCustormer.InvoiceId = item.InvoiceId.GenerateInvoiceNo();
                                SetupLeadCustormer.PaymentMethod = Model.PaymentMethod;
                                SetupLeadCustormer.AmountPaid = item.Amout;
                                SetupLeadCustormer.TotalAmount = item.InvoiceTotal;
                                SetupLeadCustormer.BalanceDue = item.InvoiceBalanceDue - item.Amout;
                                SetupLeadCustormer.Description = Model.Description;

                                _Util.Facade.MailFacade.EmailToSuccessTransaction(SetupLeadCustormer, CurrentUser.CompanyId.Value);
                                #endregion
                            }
                        }

                    }

                }
                #endregion
            }
            #endregion

            CustomerSalesDateUpdate(CustomerDetails);

            return Json(new { result = true, transactionSuccess = TransactionSuccess, message = "Payment received successfully." });
        }
        [Authorize]
        public ActionResult GetTransaction(int Id)
        {
            return View();
        }
        [Authorize]
        public PartialViewResult SendEmailTransaction(int Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            CreateCustomerInvoice model = new CreateCustomerInvoice();

            Invoice TempInvoice = _Util.Facade.InvoiceFacade.GetInvoiceById(Id);
            Guid cusidval = TempInvoice.CustomerId;
            Guid companyid = TempInvoice.CompanyId;
            model.InvoiceId = TempInvoice.InvoiceId;
            var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(cusidval);
            var objcom = _Util.Facade.CompanyFacade.GetAllCompanyByCompanyId(CurrentUser.CompanyId.Value).FirstOrDefault();
            if (objcus != null)
            {
                model.CustomerName = objcus.FirstName + " " + objcus.LastName;
                model.CustomerEmailAddress = objcus.EmailAddress;
            }
            if (objcom != null)
            {
                model.CompanyName = objcom.CompanyName;
                model.CompanyEmail = objcom.EmailAdress;
            }
            return PartialView("SendEmailTransaction", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult TransactionSendEmail(int CustomerId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if (CurrentLoggedInUser == null)
            {
                return Json(new { result = result, message = "error" });
            }
            if (CurrentLoggedInUser != null)
            {
                var leadDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                if (leadDetails != null)
                {
                    SetupLeadCustormer SetupLeadCustormer = new SetupLeadCustormer();
                    SetupLeadCustormer.CompanyName = "";
                    var CompanyObj = _Util.Facade.CustomerFacade.GetCompanyByCustomerId(leadDetails.CustomerId);
                    if (CompanyObj != null)
                    {
                        SetupLeadCustormer.CompanyName = CompanyObj.CompanyName;
                    }
                    SetupLeadCustormer.CustomerName = leadDetails.FirstName + " " + leadDetails.LastName;
                    if (!string.IsNullOrWhiteSpace(leadDetails.BusinessName))
                    {
                        SetupLeadCustormer.CustomerName = leadDetails.BusinessName;
                    }
                    SetupLeadCustormer.CustomerNo = leadDetails.CustomerNo;
                    SetupLeadCustormer.ToEmail = leadDetails.EmailAddress;
                    SetupLeadCustormer.CustomerId = leadDetails.CustomerId.ToString();
                    SetupLeadCustormer.EmployeeId = CurrentLoggedInUser.UserId.ToString();
                    Guid SalesPersonId = new Guid();
                    if (Guid.TryParse(leadDetails.Soldby, out SalesPersonId) && SalesPersonId != new Guid())
                    {
                        Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(SalesPersonId);
                        if (emp != null)
                        {
                            SetupLeadCustormer.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);
                            if (emp.Email.IsValidEmailAddress())
                            {
                                SetupLeadCustormer.ToSalesPersonsEmail = emp.Email;
                            }
                            else if (emp.UserName.IsValidEmailAddress())
                            {
                                SetupLeadCustormer.ToSalesPersonsEmail = emp.UserName;
                            }
                        }
                    }
                    _Util.Facade.MailFacade.EmailToSuccessTransaction(SetupLeadCustormer, CurrentLoggedInUser.CompanyId.Value);
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeclineInvoicesFromFile(string FileName)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            #region read excell file
            string filepath = Server.MapPath("~" + FileName);
            var workbook = new Excel.XLWorkbook(filepath);
            //xlApp = xlApp.Worksheets(Server.MapPath("~"+FileName)/*ExcelFile[0].FullName*/);
            //xlApp.Worksheet = xlWorkbook.Sheets[1];
            Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
            var xlRange = workSheet.RangeUsed();

            int rowCount = xlRange.Rows().Count();
            int colCount = xlRange.Cells().Count();
            List<DeclinedTransactions> TransactionList = new List<DeclinedTransactions>();
            for (int i = 2; i <= rowCount; i++)
            {
                DeclinedTransactions dt = new DeclinedTransactions();
                dt.CompanyId = CurrentUser.CompanyId.Value;
                dt.InvoiceId = "-";
                dt.CustomerId = Guid.Empty;
                for (int j = 1; j <= colCount; j++)
                {
                    #region GetValues Into Object
                    var value = xlRange.Cell(i, j).Value.ToString();
                    var header = xlRange.Cell(1, j).Value.ToString();
                    if (!string.IsNullOrWhiteSpace(value)
                        && !string.IsNullOrWhiteSpace(header))
                    {
                        if (header == "Original Trans ID")
                        {
                            dt.TransactionId = value;
                        }
                        else if (header == "Invoice Number")
                        {
                            dt.InvoiceId = value;
                        }
                        else if (header == "Reason")
                        {
                            dt.Reason = value;
                        }
                        else if (header == "Returned Date")
                        {
                            //dt.ReturnedDate = DateTime.Now.UTCCurrentTime();
                            DateTime ReturnedDate = DateTime.Now.UTCCurrentTime();
                            DateTime.TryParse(value, out ReturnedDate);
                            dt.ReturnedDate = ReturnedDate;
                        }
                        else if (header == "Returned Amount")
                        {
                            double ReturnedAmount = 0;
                            double.TryParse(value, out ReturnedAmount);
                            dt.ReturnAmount = ReturnedAmount;
                        }
                        else if (header == "Submit Amount")
                        {
                            double ReturnedAmount = 0;
                            double.TryParse(value, out ReturnedAmount);
                            dt.SubmitAmount = ReturnedAmount;
                        }
                        else if (header == "Original Settlement Batch")
                        {
                            DateTime SettlementBatch = DateTime.Now.UTCCurrentTime();
                            DateTime.TryParse(value, out SettlementBatch);
                            dt.SettlementBatch = SettlementBatch;
                        }

                    }
                    #endregion
                }
                if (string.IsNullOrWhiteSpace(dt.TransactionId))
                {
                    continue;
                }
                TransactionList.Add(dt);
            }

            object misValue = System.Reflection.Missing.Value;
            //xlWorkbook.Close(true, misValue, misValue);
            //xlApp.Quit();

            //Marshal.ReleaseComObject(workSheet);
            //Marshal.ReleaseComObject(workbook);
            //Marshal.ReleaseComObject(xlApp);
            if (TransactionList.Count() == 0)
            {
                return Json(new { result = false, message = "No records found." });
            }
            #endregion

            #region Search if the transaction is already in database

            List<string> CommonTransactions = _Util.Facade.DeclinedTransactionsFacade.GetExistingTransactionsByTransactionIdList(TransactionList.Select(x => x.TransactionId).ToList());

            if (CommonTransactions != null && CommonTransactions.Count() > 0)
            {
                foreach (var item in CommonTransactions)
                {
                    var itemToRemove = TransactionList.SingleOrDefault(r => r.TransactionId == item);
                    if (itemToRemove != null)
                        TransactionList.Remove(itemToRemove);
                }
            }
            #endregion

            if (TransactionList.Count() == 0)
            {
                return Json(new { result = false, message = "No new records found." });
            }

            #region pore
            foreach (var item in TransactionList)
            {
                item.InvoiceId = "";
                item.Comment = "";
                List<Invoice> invlist = _Util.Facade.InvoiceFacade.GetInvoiceByTransactionId(item.TransactionId);
                if (invlist != null && invlist.Count() > 0)
                {
                    foreach (var inv in invlist)
                    {
                        item.CustomerId = inv.CustomerId;
                        item.InvoiceId += inv.InvoiceId + " ";
                        if (inv.Status == LabelHelper.InvoiceStatus.Declined)
                        {
                            item.Comment += "Invoice " + inv.InvoiceId + " is already declined.";
                        }
                        if (inv.InvoiceFor == LabelHelper.InvoiceFor.SystemGenerated
                            || inv.InvoiceFor == LabelHelper.InvoiceFor.CreditCard
                            || inv.InvoiceFor == LabelHelper.InvoiceFor.ACH)
                         {
                            string TempStatus = "";

                            if (inv != null)
                            {
                                TempStatus = inv.Status;

                            }
                            List<InvoiceDetail> DetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(item.InvoiceId);
                            inv.Status = LabelHelper.InvoiceStatus.Declined;
                            _Util.Facade.InvoiceFacade.UpdateInvoice(inv);

                            if (inv != null && TempStatus != inv.Status)
                            {
                                #region log
                                int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                                bool newBool = inv.IsARBInvoice ?? false;


                                base.AddUserActivityForCustomer("Invoice Status Changed from " + TempStatus + " To " + inv.Status + "#InvoiceId: " + inv.InvoiceId, lineNumber + "," + actionName + "/" + controllerName, inv.CustomerId, null, null, newBool);
                                #endregion
                            }

                            inv.Status = LabelHelper.InvoiceStatus.Open;
                            inv.CreatedDate = DateTime.Now.UTCCurrentTime();
                            inv.CreatedBy = User.Identity.Name;
                            inv.CreatedByUid = CurrentUser.UserId;
                            inv.BalanceDue = inv.TotalAmount;
                            inv.Id = _Util.Facade.InvoiceFacade.InsertInvoice(inv);

                            //inv.Description = string.Format("{0}. Invoice created for payment declination of {1}", CopyInvoice.Description, item.InvoiceId);
                            if (inv.Description.IndexOf("Monthly Monitoring Fee") > -1)
                            {
                                string[] split = inv.Description.Split(new[] { "Monthly Monitoring Fee" }, StringSplitOptions.None);
                                if (split.Length > 1)
                                {
                                    string text = split[1].TrimEnd('.');
                                    inv.Description = string.Format("Duplicate Invoice {0}. Returned {1}", text, inv.InvoiceId);
                                }
                                else
                                {
                                    inv.Description = string.Format("{0} Returned {1}", inv.Description, inv.InvoiceId);
                                }
                            }
                            else
                            {
                                inv.Description = string.Format("{0} Returned {1}", inv.Description, inv.InvoiceId);
                            }
                            inv.InvoiceId = inv.Id.GenerateInvoiceNo();
                            item.Comment = string.Format("Successful. {0} has been created.", inv.InvoiceId);

                            _Util.Facade.InvoiceFacade.UpdateInvoice(inv);

                            foreach (var detailItem in DetailList)
                            {
                                detailItem.InvoiceId = inv.InvoiceId;
                                detailItem.CreatedBy = User.Identity.Name;
                                detailItem.CreatedDate = DateTime.Now.UTCCurrentTime();
                                _Util.Facade.InvoiceFacade.InsertInvoiceDetails(detailItem);
                            }

                            #region Delete Transaction and Transaction details
                            _Util.Facade.TransactionFacade.DeleteTransactionAndHistoryByCardTranId(item.TransactionId, item.CustomerId);

                            #endregion
                        }
                        else
                        {
                            item.Comment += string.Format("Failed. Invoice {0} is not system generated. ", inv.InvoiceId);
                        }
                    }
                }
                else
                {
                    item.InvoiceId = "-";
                    item.Comment = string.Format("Failed. Invoice not found for transaction {0}.", item.TransactionId);
                }

                _Util.Facade.DeclinedTransactionsFacade.InsertDeclinedTransaction(item);
            }

            #region previous logics
            //List<Invoice> InvoiceList = _Util.Facade.TransactionFacade.GetInvoiceListByCardTransactionIdList(TransactionIdList);
            //foreach (var item in InvoiceList)
            //{
            //    if (item.Status != LabelHelper.InvoiceStatus.Declined)
            //    {
            //        List<InvoiceDetail> DetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(item.InvoiceId);

            //        Invoice CopyInvoice = (Invoice)item.Clone();
            //        CopyInvoice.Status = LabelHelper.InvoiceStatus.Open;
            //        CopyInvoice.CreatedDate = DateTime.Now.UTCCurrentTime();
            //        CopyInvoice.CreatedBy = User.Identity.Name;
            //        CopyInvoice.CreatedByUid = CurrentUser.UserId;
            //        CopyInvoice.Id = _Util.Facade.InvoiceFacade.InsertInvoice(CopyInvoice);
            //        CopyInvoice.Description = string.Format("{0}. Invoice created for payment declination of {1}", CopyInvoice.Description, item.InvoiceId);
            //        CopyInvoice.InvoiceId = CopyInvoice.Id.GenerateInvoiceNo();

            //        _Util.Facade.InvoiceFacade.UpdateInvoice(CopyInvoice);

            //        foreach (var detailItem in DetailList)
            //        {
            //            detailItem.InvoiceId = CopyInvoice.InvoiceId;
            //            detailItem.CreatedBy = User.Identity.Name;
            //            detailItem.CreatedDate = DateTime.Now.UTCCurrentTime();
            //            _Util.Facade.InvoiceFacade.InsertInvoiceDetails(detailItem);
            //        }

            //    }
            //}
            #endregion

            #endregion

            return Json(new { result = true, message = "Transactions updated successfully." });
        }


        #region NumberToWord
        private static string NumberToWords(double Amount)
        {

            string centval = (Amount - (int)Amount).ToString("#.##");
            double valcent = 0;
            if (!string.IsNullOrWhiteSpace(centval))
            {
                valcent = Convert.ToDouble(centval);
            }
            int cent = (int)(valcent * 100);
            int number = (int)Amount;

            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " Billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
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
            if (cent > 0)
            {
                words += " " + cent + "/100";
            }
            return words;
        }
        #endregion
        //Used for test perpouse
        //[HttpGet]
        //public JsonResult CreateSubscriptions()
        //{
        //    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
        //    string TransactionKey = _Util.Facade.GlobalSettingsFacade.GetAuthTransactionKeyByCompanyId(CurrentUser.CompanyId.Value);
        //    string APILoginId = _Util.Facade.GlobalSettingsFacade.GetAuthAPILoginIdByCompanyId(CurrentUser.CompanyId.Value);
        //    //short day = 30;
        //    ARBSubscription Model = new ARBSubscription()
        //    {
        //        FirstName = "Kaizar Tariq",
        //        LastName = "Inan",

        //        CardNumber = "4929065231937858",
        //        CardPassword = "123",
        //        ExpirationDate = "1220",

        //        Address = "H#44, R#01", 
        //        City = "Dhaka",
        //        State = "Dhaka",
        //        Zip = "1230",

        //        CompanyName ="PiisTech",
        //        Description="Test Description",
        //        Country = "Bangladesh",

        //        Invoice="Inv-1044",
        //        SubscriptionStartDate = DateTime.Now.UTCCurrentTime().AddDays(1),
        //        SubscritptionAmount = 100.10m,
        //        TotalOccurrences= 12,
        //        TrialAmount=2,
        //        TrialOccurrences=1,
        //        SubscriptionInterval = 30,

        //    };

        //    var result= CreateSubscription.Run(APILoginId, TransactionKey, Model);

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public JsonResult TransactionList()
        //{
        //    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
        //    string TransactionKey = _Util.Facade.GlobalSettingsFacade.GetAuthTransactionKeyByCompanyId(CurrentUser.CompanyId.Value);
        //    string APILoginId = _Util.Facade.GlobalSettingsFacade.GetAuthAPILoginIdByCompanyId(CurrentUser.CompanyId.Value);

        //    var result = GetTransactionList.Run(APILoginId, TransactionKey, "7693939");

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public JsonResult GetSubscriptionStatuss()
        //{
        //    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
        //    string TransactionKey = _Util.Facade.GlobalSettingsFacade.GetAuthTransactionKeyByCompanyId(CurrentUser.CompanyId.Value);
        //    string APILoginId = _Util.Facade.GlobalSettingsFacade.GetAuthAPILoginIdByCompanyId(CurrentUser.CompanyId.Value);

        //    var result = GetSubscriptionStatus.Run(APILoginId, TransactionKey, "4889136");

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public JsonResult GetSubscriptionList()
        //{
        //    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
        //    string TransactionKey = _Util.Facade.GlobalSettingsFacade.GetAuthTransactionKeyByCompanyId(CurrentUser.CompanyId.Value);
        //    string APILoginId = _Util.Facade.GlobalSettingsFacade.GetAuthAPILoginIdByCompanyId(CurrentUser.CompanyId.Value);
        //    var result = GetListOfSubscriptions.Run(APILoginId, TransactionKey);

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //private ActionResult Auth()
        //{
        //    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

        //    String post_url = "https://test.authorize.net/gateway/transact.dll";

        //    Dictionary<string, string> post_values = new Dictionary<string, string>();
        //    //the API Login ID and Transaction Key must be replaced with valid values

        //    string TransactionKey = _Util.Facade.GlobalSettingsFacade.GetAuthTransactionKeyByCompanyId(CurrentUser.CompanyId.Value);
        //    string APILoginId = _Util.Facade.GlobalSettingsFacade.GetAuthAPILoginIdByCompanyId(CurrentUser.CompanyId.Value);

        //    post_values.Add("x_login", APILoginId);
        //    post_values.Add("x_tran_key", TransactionKey);


        //    post_values.Add("x_delim_data", "TRUE");
        //    post_values.Add("x_delim_char", "|");
        //    post_values.Add("x_relay_response", "FALSE");

        //    post_values.Add("x_type", "AUTH_CAPTURE");
        //    post_values.Add("x_method", "CC");

        //    //Credit Card Number
        //    post_values.Add("x_card_num", "349224707744724");

        //    //Expiration Date Card Number
        //    post_values.Add("x_exp_date", "1218");

        //    //Order Amount
        //    post_values.Add("x_amount", "19.99");

        //    //post_values.Add("x_description", "Sample Transaction");

        //    post_values.Add("x_first_name", "John");
        //    post_values.Add("x_last_name", "Doe");
        //    //post_values.Add("x_address", "1234 Street");
        //    //post_values.Add("x_state", "WA");
        //    //post_values.Add("x_zip", "98004");

        //    // Additional fields can be added here as outlined in the AIM integration
        //    // guide at: http://developer.authorize.net

        //    // This section takes the input fields and converts them to the proper format
        //    // for an http post.  For example: "x_login=username&x_tran_key=a1B2c3D4"
        //    String post_string = "";

        //    foreach (KeyValuePair<string, string> post_value in post_values)
        //    {
        //        post_string += post_value.Key + "=" +
        //        HttpUtility.UrlEncode(post_value.Value) + "&";
        //    }
        //    post_string = post_string.TrimEnd('&');

        //    // The following section provides an example of how to add line item details to
        //    // the post string.  Because line items may consist of multiple values with the
        //    // same key/name, they cannot be simply added into the above array.
        //    //
        //    // This section is commented out by default.

        //    // create an HttpWebRequest object to communicate with Authorize.net
        //    HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(post_url);
        //    objRequest.Method = "POST";
        //    objRequest.ContentLength = post_string.Length;
        //    objRequest.ContentType = "application/x-www-form-urlencoded";

        //    // post data is sent as a stream
        //    StreamWriter myWriter = null;
        //    myWriter = new StreamWriter(objRequest.GetRequestStream());
        //    myWriter.Write(post_string);
        //    myWriter.Close();

        //    // returned values are returned as a stream, then read into a string
        //    String post_response;
        //    HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
        //    using (StreamReader responseStream = new StreamReader(objResponse.GetResponseStream()))
        //    {
        //        post_response = responseStream.ReadToEnd();
        //        responseStream.Close();
        //    }

        //    // the response string is broken into an array
        //    // The split character specified here must match the delimiting character specified above
        //    Array response_array = post_response.Split('|');

        //    // the results are output to the screen in the form of an html numbered list.
        //    var result = "<OL> \n";
        //    foreach (string value in response_array)
        //    {
        //        result = result + "<LI>" + value + "&nbsp;</LI> \n";
        //    }
        //    result = result + "</OL> \n";
        //    // individual elements of the array could be accessed to read certain response
        //    // fields.  For example, response_array[0] would return the Response Code,
        //    // response_array[2] would return the Response Reason Code.
        //    // for a list of response fields, please review the AIM Implementation Guide


        //    return View();
        //}

        //[HttpGet]
        //public JsonResult Unsattled()
        //{
        //    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
        //    string TransactionKey = _Util.Facade.GlobalSettingsFacade.GetAuthTransactionKeyByCompanyId(CurrentUser.CompanyId.Value);
        //    string APILoginId = _Util.Facade.GlobalSettingsFacade.GetAuthAPILoginIdByCompanyId(CurrentUser.CompanyId.Value);
        //    getUnsettledTransactionListResponse unsattled = GetUnsettledTransactionList.Run(APILoginId,TransactionKey);
        //    if(unsattled.transactions.Count() > 0)
        //    {

        //    }
        //    return Json(true,JsonRequestBehavior.AllowGet);
        //}


        /* public JsonResult Alarm_Info()
         {
             try
             {
                 com.alarm.alarmadmin.CustomerManagement ws = new com.alarm.alarmadmin.CustomerManagement();
                 com.alarm.alarmadmin.Authentication auth = new com.alarm.alarmadmin.Authentication();
                 auth.User = "RABSecurityWebservices";
                 auth.Password = "MaishaM@2";
                 ws.AuthenticationValue = auth;

                 com.alarm.alarmadmin.CreateCustomerInput customer = new com.alarm.alarmadmin.CreateCustomerInput() {
                     DesiredLoginName= "Test Customer",
                     CustomerAccountEmail="kaizar.tariq@gmail.com"
                 };
                 customer.CustomerAccountEmail = "kaizar.tariq@gmail.com";
                 customer.CustomerAccountPhone = "571-279-0064";

                 customer.CustomerAccountAddress = new com.alarm.alarmadmin.AddressWithName()
                 {
                     City = "NY",
                     CompanyName = "PiisTech",
                     CountryId = com.alarm.alarmadmin.CountryEnum.USA,
                     FirstName = "Test2",
                     LastName = "Last name2",

                 };
                 customer.InstallationAddress = new com.alarm.alarmadmin.Address()
                 {
                     City = "New York",
                     CountryId = com.alarm.alarmadmin.CountryEnum.USA,
                     State = "New York",
                     Street1 = "1",
                     Street2= "2",
                     SubCity = "3",
                     Zip = "10006"
                 };

                 com.alarm.alarmadmin.CreateCustomerOutput CustomerOutPut = ws.CreateCustomer(customer);

                 /*StringBuilder sb = new StringBuilder();
                 foreach (com.alarm.alarmadmin1.InvoiceLineItem li in invoice.LineItems)
                 {
                     if (li.Customer != null && //has customer info
                     li.LineItemType == com.alarm.alarmadmin1.InvoiceLineItemTypeEnum.Service && //is for service (not activation)
                     li.ChargeDate.Day == 1 ) //is for a package (not add-on)
                     {
                         sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\r\n"
                         , li.LineItemType.ToString()
                         , li.ChargeAmount
                         , li.ChargeDate, li.ChargeDesc
                         , li.Customer.CustomerId, li.Customer.DealerCustomerId
                         , li.Customer.CentralStationPhoneNumber
                         , li.Customer.CentralStationAccountNumber, li.Customer.FirstName
                         , li.Customer.LastName
                         , li.Customer.PackageId, li.Customer.PackageDesc);
                     }
                 }*/

        /*}
        catch (Exception ex)
        {

        }

        return Json("true", JsonRequestBehavior.AllowGet);
    }*/

        [HttpPost]
        public JsonResult SendEmailFunding(int? transactionid, string subject, string body, string ccEmail, string toEmail)
        {
            bool result = false;
            string message = "";
            if (transactionid.HasValue && transactionid.Value > 0)
            {
                var objTransactionHistory = _Util.Facade.TransactionFacade.GetTransactionHistoryById(transactionid.Value);
                if (objTransactionHistory != null)
                {
                    var objtrans = _Util.Facade.TransactionFacade.GetTransactionById(objTransactionHistory.TransactionId);
                    if (objtrans != null)
                    {
                        var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(objtrans.CustomerId);
                        if (objcus != null)
                        {
                            if (!string.IsNullOrWhiteSpace(toEmail))
                            {
                                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                                string pdfname = "Funding";
                                TransactionPdfModel pdfmodel = _Util.Facade.TransactionFacade.GetTransactionPdfDataByTransactionId(transactionid.Value);
                                pdfmodel.CompanyId = CurrentUser.CompanyId.Value;
                                pdfmodel.CompanyName = CurrentUser.CompanyName;
                                pdfmodel.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
                                pdfmodel.CompanyAddressFormat = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
                                pdfmodel.AmountInWord = NumberToWords(pdfmodel.PaymentAmount);
                                Company Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);

                                Hashtable datatemplate = new Hashtable();
                                #region CompanyAddress Info
                                datatemplate.Add("ComapnyName", Company.CompanyName);
                                datatemplate.Add("Address", Company.Address);
                                datatemplate.Add("Street", Company.Street);
                                datatemplate.Add("City", Company.City);
                                datatemplate.Add("State", Company.State);
                                datatemplate.Add("Zip", Company.ZipCode);
                                datatemplate.Add("CompanyPhone", Company.Phone);
                                datatemplate.Add("EmailAddress", Company.EmailAdress);
                                datatemplate.Add("WebAddress", Company.Website);
                                #endregion
                                pdfmodel.CompanyAddressFormat = HS.Web.UI.Helper.LabelHelper.ParserHelper(pdfmodel.CompanyAddressFormat, datatemplate);
                                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Transaction/TransactionPdf.cshtml", pdfmodel)
                                {
                                    //FileName = "TestView.pdf",
                                    PageSize = Size.A4,
                                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                                    PageMargins = { Left = 1, Right = 1 },

                                };
                                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                                string filename = ConfigurationManager.AppSettings["File.InvoiceFiles"];
                                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                                filename = string.Format(filename, comname);
                                Random random = new Random();
                                filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + random.Next() + pdfname + ".pdf";
                                string Serverfilename = FileHelper.GetFileFullPath(filename);
                                FileHelper.SaveFile(applicationPDFData, Serverfilename);
                                FundingSendEmail email = new FundingSendEmail()
                                {
                                    CompanyName = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName,
                                    CustomerName = pdfmodel.CustomerName,
                                    ToEmail = toEmail,
                                    ccEmail = ccEmail,
                                    FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : "info@rmrcloud.com",
                                    FromName = string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName),
                                    Subject = subject,
                                    EmailBody = body,
                                    FundingPdf = new Attachment(
                                      FileHelper.GetFileFullPath(filename),
                                     MediaTypeNames.Application.Octet)
                                };
                                result = _Util.Facade.MailFacade.SendFundingEmail(email, CurrentUser.CompanyId.Value);
                                if (result)
                                {
                                    LeadCorrespondence objCor = new LeadCorrespondence()
                                    {
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        CustomerId = objcus.CustomerId,
                                        TemplateKey = "SendFundingEmail",
                                        Type = LabelHelper.CorrespondenceMessageTyp.Email,
                                        ToEmail = toEmail,
                                        Subject = subject,
                                        BodyContent = body,
                                        SentDate = DateTime.Now.UTCCurrentTime(),
                                        IsSystemAutoSent = true,
                                        SentBy = CurrentUser.UserId
                                    };
                                    _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objCor);
                                    message = "Successfully send email to " + toEmail;
                                }
                            }
                            else
                            {
                                message = "Customer email address not found";
                            }
                        }
                    }
                }

            }
            return Json(new { result = result, message = message });
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

        private bool CustomerSalesDateUpdate(Customer customer)
        {
            if (customer != null)
            {
                var objticket = _Util.Facade.TicketFacade.GetTicketByCustomerIdAndIsAgreementTicket(customer.CustomerId);
                var objinvoice = _Util.Facade.InvoiceFacade.GetInvoiceByCustomerIdAndInstallationType(customer.CustomerId, "Service");
                if (objinvoice != null && objticket != null && objinvoice.Status == LabelHelper.InvoiceStatus.Paid && customer.IsAgreementSend.HasValue && customer.IsAgreementSend.Value)
                {
                    var objticketuser = _Util.Facade.TicketFacade.GetTicketUserByTicketIdAndIsPrimary(new Guid(), objticket.TicketId);
                    if (objticketuser != null && objticketuser.UserId.ToString() != "22222222-2222-2222-2222-222222222222")
                    {
                        if (!customer.SalesDate.HasValue)
                        {
                            customer.SalesDate = DateTime.Now.UTCCurrentTime();
                            _Util.Facade.CustomerFacade.UpdateCustomer(customer);
                        }
                    }
                }
            }
            return true;
        }

        [Authorize]
        [HttpPost]
        public JsonResult CollectPaymentRMRInvoicesFromFile(string FileName)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!CurrentUser.CompanyId.HasValue)
            {
                return Json(new { result = false, message = "Unauthorized! access is denied." });
            }

            #region Excel File Data Read
            string StrFileName = FileName.Split('/').Last();
            string BatchNo = StrFileName.Split('-')[2];
            if (string.IsNullOrWhiteSpace(BatchNo) || string.IsNullOrEmpty(BatchNo) || BatchNo.Length != 15)
            {
                BatchNo = DateTime.UtcNow.UTCToClientTime().ToString("yyMMddHHmmss").GenerateInvoiceBatchNo();
            }
            #region read excell file
            string filepath = Server.MapPath("~" + FileName);
            var workbook = new Excel.XLWorkbook(filepath);
            Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
            var xlRange = workSheet.RangeUsed();

            int rowCount = xlRange.Rows().Count();
            int colCount = xlRange.Cells().Count();
            List<RMRCollectPaymentInfoFromFileModel> RMRList = new List<RMRCollectPaymentInfoFromFileModel>();
            for (int i = 2; i <= rowCount; i++)
            {
                RMRCollectPaymentInfoFromFileModel rmr = new RMRCollectPaymentInfoFromFileModel();
                rmr.CompanyId = CurrentUser.CompanyId.Value;
                for (int j = 1; j <= colCount; j++)
                {
                    #region GetValues Into Object
                    var value = xlRange.Cell(i, j).Value.ToString();
                    var header = xlRange.Cell(1, j).Value.ToString();
                    if (!string.IsNullOrWhiteSpace(value)
                        && !string.IsNullOrWhiteSpace(header))
                    {
                        if (header == "Customer Id")
                        {
                            int CustomerId = 0;
                            int.TryParse(value, out CustomerId);
                            rmr.CustomerId = CustomerId;
                        }
                        else if (header == "Customer Name")
                        {
                            rmr.CustomerName = value;
                        }
                        else if (header == "Invoice Number")
                        {
                            rmr.InvoiceNumber = value;
                        }
                        else if (header == "Total Due Amount")
                        {
                            double DueAmount = 0;
                            double.TryParse(value, out DueAmount);
                            rmr.TotalDueAmount = DueAmount;
                        }
                        else if (header == "Check No")
                        {
                            rmr.ChequeNumber = value;
                        }
                        else if (header == "Collected Amount")
                        {
                            double CollectedAmount = 0;
                            double.TryParse(value, out CollectedAmount);
                            rmr.CollectedAmount = CollectedAmount;
                        }
                        else if (header == "Invoice For")
                        {
                            rmr.InvoiceType = value;
                        }
                        else if (header == "Payment Method")
                        {
                            rmr.PaymentPethod = value;
                        }
                        else if (header == "Company Name")
                        {
                            rmr.CompanyName = value;
                        }

                    }
                    #endregion
                }
                if (string.IsNullOrEmpty(rmr.InvoiceNumber) && string.IsNullOrWhiteSpace(rmr.InvoiceNumber) && rmr.CustomerId < 1 && rmr.CollectedAmount <= 0 && CurrentUser.CompanyName.ToLower() != rmr.CompanyName.ToLower())
                {
                    continue;
                }
                RMRList.Add(rmr);
            }

            if (RMRList == null || RMRList.Count() < 1)
            {
                return Json(new { result = false, message = "No records found." });
            }
            #endregion

            #endregion

            #region Collect Payments Section
            bool ReturnResult = false;
            string ReturnMessage = "Your payment is not successfully charged.";
            string Description = string.Format("Received by {0}", CurrentUser.GetFullName());
            foreach (var paymentInfo in RMRList)
            {
                string StrIdList = "";
                int InvCount = 0;
                if (!(paymentInfo.CollectedAmount > 0 && paymentInfo.TotalDueAmount > 0)) { continue; }
                if (string.IsNullOrWhiteSpace(paymentInfo.InvoiceNumber) && paymentInfo.CustomerId < 1 && paymentInfo.CollectedAmount <= 0 && CurrentUser.CompanyName.ToLower() != paymentInfo.CompanyName.ToLower()) { continue; }
                Customer _customer = _Util.Facade.CustomerFacade.GetById(paymentInfo.CustomerId);
                CustomerCompany cusCom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(_customer.Id);
                if (_customer == null && cusCom == null) { continue; }
                List<Invoice> InvoiceList = new List<Invoice>();
                if (paymentInfo.InvoiceType.ToLower() == "rmr" || paymentInfo.InvoiceType.ToLower() == "others" || paymentInfo.InvoiceType.ToLower() == "all")
                {
                    string[] arrId = paymentInfo.InvoiceNumber.Split(',');

                    if (arrId != null && arrId.Length > 0)
                    {
                        InvCount = arrId.Length;
                        for (int i = 0; i < arrId.Length; i++)
                        {
                            if (i == 0) { StrIdList = string.Format("'" + arrId[i] + "'"); }
                            else { StrIdList = string.Format(StrIdList + ",'" + arrId[i] + "'"); }
                        }
                    }
                    else { continue; }
                    var InvList = _Util.Facade.InvoiceFacade.GetUnpaidInvoiceFromExcelFileByInvoiceId(StrIdList, _customer.CustomerId, CurrentUser.CompanyId.Value);
                    if (InvList != null && InvList.Count > 0) { InvoiceList = InvList.OrderBy(x => x.Id).ToList(); }
                    else { continue; }
                }
                else { continue; }
                if (InvoiceList != null && InvoiceList.Count < 1) { continue; }
                List<string> StrInvoiceIdList = new List<string>();
                List<int> IntInvoiceIdList = new List<int>();
                string TransactionReference = paymentInfo.InvoiceNumber;
                if (paymentInfo.InvoiceNumber.Length > 20)
                {
                    TransactionReference = "Total " + InvCount + " Invoices - " + BatchNo;
                }

                #region Insert Into Transaction Queue
                string Starttime = DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss");
                string Endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                double AmountPaid = Math.Round(paymentInfo.CollectedAmount, 2);
                List<TransactionQueue> transqueList = new List<TransactionQueue>();
                transqueList = _Util.Facade.TransactionFacade.GetTransactionQueueCustomerId(_customer.CustomerId, Starttime, Endtime, AmountPaid);
                if (transqueList.Count > 0)
                {
                    continue;
                }
                else
                {
                    TransactionQueue transque = new TransactionQueue()
                    {
                        CustomerId = _customer.CustomerId,
                        Amount = AmountPaid,
                        InvoiceId = TransactionReference,
                        CreatedBy = CurrentUser.UserId,
                        CreatedDate = DateTime.Now
                    };
                    _Util.Facade.TransactionFacade.InsertTransactionQueue(transque);
                }
                #endregion

                #region Transaction Section
                Transaction tr = new Transaction()
                {
                    Status = "Closed",
                    Type = "Payment",
                    TransacationDate = DateTime.Now.UTCCurrentTime(),
                    CustomerId = _customer.CustomerId,
                    CompanyId = CurrentUser.CompanyId.Value,
                    Amount = Math.Round(AmountPaid, 2),
                    ReferenceNo = TransactionReference,
                    CheckNo = paymentInfo.ChequeNumber,
                    AddedBy = User.Identity.Name,
                    AddedDate = DateTime.Now.UTCCurrentTime(),
                    CreatedBy = CurrentUser.UserId
                };
                if (!string.IsNullOrWhiteSpace(paymentInfo.ChequeNumber) && !string.IsNullOrEmpty(paymentInfo.ChequeNumber))
                {
                    tr.PaymentMethod = LabelHelper.PaymentMethod.Check;
                }
                else
                {
                    tr.PaymentMethod = LabelHelper.PaymentMethod.Cash;
                }
                tr.Id = _Util.Facade.TransactionFacade.InsertTransaction(tr);

                #endregion
                #region transaction History Section
                List<TransactionHistory> trhistory = new List<TransactionHistory>();
                InvoiceList = InvoiceList.OrderBy(x => x.Id).ToList();
                double TotalCollectedAmount = Math.Round(AmountPaid, 2);
                string StrInvoiceId = "";
                string CreditNote = "";
                foreach (var item in InvoiceList)
                {
                    string TempStatus = item.Status;
                    if (string.IsNullOrWhiteSpace(CreditNote))
                    {
                        CreditNote = string.Format(@"Invoice# <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{0}')"">{1}</a>", item.Id, item.InvoiceId);
                    }
                    else
                    {
                        CreditNote = CreditNote + ",<br />" + string.Format(@"<a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{0}')"">{1}</a>", item.Id, item.InvoiceId);
                    }

                    double PerPayment = 0;
                    if (item.BalanceDue.HasValue && item.BalanceDue.Value > 0 && TotalCollectedAmount > 0)
                    {
                        if (TotalCollectedAmount > item.BalanceDue.Value)
                        {
                            PerPayment = Math.Round(item.BalanceDue.Value, 2);
                            TotalCollectedAmount -= PerPayment;
                        }
                        else
                        {
                            PerPayment = Math.Round(TotalCollectedAmount, 2);
                            TotalCollectedAmount = 0;
                        }
                    }
                    else { continue; }

                    trhistory.Add(new TransactionHistory()
                    {
                        Amout = Math.Round(PerPayment, 2),
                        InvoiceId = item.Id,
                        TransactionId = tr.Id,
                        Balance = Math.Round(item.BalanceDue.HasValue ? item.BalanceDue.Value : 0, 2),
                        ReceivedBy = CurrentUser.UserId,
                        InvoiceTotal = item.TotalAmount.HasValue ? item.TotalAmount.Value : 0,
                        InvoiceBalanceDue = item.BalanceDue.HasValue ? item.BalanceDue.Value : 0
                    });
                    #region Update Invocie
                    item.BalanceDue = Math.Round(item.BalanceDue.HasValue ? item.BalanceDue.Value : 0, 2) - Math.Round(PerPayment, 2);
                    item.Status = item.BalanceDue == 0 ? LabelHelper.InvoiceStatus.Paid : LabelHelper.InvoiceStatus.Partial;
                    item.PaymentType = tr.PaymentMethod;
                    item.BatchNumber = BatchNo;
                    _Util.Facade.InvoiceFacade.UpdateInvoice(item);
                    #region User Log
                    if (item != null && TempStatus != item.Status)
                    {
                        string LogMsg = "Invoice status changed from " + TempStatus.ToLower() + " to " + item.Status.ToLower() + " #InvoiceId: " + item.InvoiceId + " collected amount $" + PerPayment.ToString("N2") + " by " + tr.PaymentMethod.ToLower() + " uploading excel file.";
                        bool IsARB = item.IsARBInvoice.HasValue ? item.IsARBInvoice.Value : false;
                        base.AddUserActivityForCustomer(LogMsg, "CollectPaymentRMRInvoicesFromFile,Transaction", _customer.CustomerId, _customer.Id, item.InvoiceId, IsARB);
                    }
                    #endregion
                    #endregion

                    #region Insert CustomerSnapshot
                    CustomerSnapshot objInvoicePayment = new CustomerSnapshot()
                    {
                        CustomerId = _customer.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        Description = "Invoice " + string.Format("<a onclick=OpenTopToBottomModal('{2}/Invoice/AddInvoice?id={0}&CustomerId={1}') style='cursor: pointer;'>", item.Id, _customer.Id, _customer.CustomerId, AppConfig.DomainSitePath) + "<b>" + item.InvoiceId + "</b>" + "</a>" + " created for " + "<b>" + _customer.FirstName + " " + _customer.LastName + "</b>",
                        Logdate = DateTime.Now.UTCCurrentTime(),
                        Updatedby = User.Identity.Name,
                        Type = "InvoicePaymentHistory"
                    };
                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objInvoicePayment);
                    #endregion
                    #region Notification
                    #region Insert notification
                    Notification notification = new Notification();
                    if (cusCom.IsLead == false)
                    {
                        notification = new Notification()
                        {
                            CompanyId = cusCom.CompanyId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            NotificationId = Guid.NewGuid(),
                            Type = LabelHelper.NotificationType.Customer,
                            Who = _customer.CustomerId,
                            What = string.Format(@"A customer <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just make payment to an invoice 
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a>", "{0}"
                                  , _customer.Id
                                  , item.Id
                                  , item.InvoiceId, AppConfig.DomainSitePath),
                            NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + _customer.Id + "#InvoiceTab"

                        };
                    }
                    else
                    {
                        notification = new Notification()
                        {
                            CompanyId = cusCom.CompanyId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            NotificationId = Guid.NewGuid(),
                            Type = LabelHelper.NotificationType.Customer,
                            Who = _customer.CustomerId,
                            What = string.Format(@"A lead <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just make payment to an invoice 
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a>", "{0}"
                                 , _customer.Id
                                 , item.Id
                                 , item.InvoiceId, AppConfig.DomainSitePath),
                            NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + _customer.Id + "#InvoiceTab"
                        };
                    }
                    _Util.Facade.NotificationFacade.InsertNotification(notification);

                    #endregion
                    #region set user to notification
                    NotificationUser nu = new NotificationUser()
                    {
                        NotificationId = notification.NotificationId,
                        IsRead = false,
                        NotificationPerson = CurrentUser.UserId,
                    };
                    _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                    #endregion
                    #endregion
                    StrInvoiceId = item.InvoiceId;
                }
                if (trhistory.Count() > 0)
                {
                    _Util.Facade.TransactionFacade.InsertTransactionHistoryList(trhistory);
                }
                #endregion

                #region Add Customer Credit Amount
                double CreditAmount = paymentInfo.CollectedAmount - paymentInfo.TotalDueAmount;
                if (CreditAmount > 0)
                {
                    CustomerCredit CusCredit = new CustomerCredit()
                    {
                        Amount = Math.Round(CreditAmount, 2),
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedBy = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = _customer.CustomerId,
                        TransactionId = tr.Id,
                        Type = LabelHelper.CustomerCreditType.Credit,
                        Note = CreditNote
                    };
                    if (paymentInfo.InvoiceType.ToLower() == "rmr") { CusCredit.IsRMRCredit = true; }
                    else { CusCredit.IsRMRCredit = false; }
                    _Util.Facade.TransactionFacade.InsertCustomerCredit(CusCredit);
                    string CreditLogMessage = "Add customer credit amount : $" + CreditAmount.ToString("N2") + " received from batch no: " + BatchNo + " payment method: " + tr.PaymentMethod.ToLower() + " By " + CurrentUser.GetFullName();
                    string UARef = null;
                    if (InvoiceList.Count == 1) { UARef = InvoiceList[0].InvoiceId; }
                    bool IsValue = true;
                    if (paymentInfo.InvoiceType.ToLower() == "others") { IsValue = false; }
                    base.AddUserActivityForCustomer(CreditLogMessage, "CollectPaymentRMRInvoicesFromFile,Transaction", _customer.CustomerId, _customer.Id, UARef, IsValue);
                }
                #endregion              

                ReturnResult = true;
                ReturnMessage = "Payment received successfully.";
                if (_customer.IsReceivePaymentMail.HasValue && _customer.IsReceivePaymentMail.Value && paymentInfo.CollectedAmount > 0)
                {
                    #region Send Payment Recipt Email 

                    SetupLeadCustormer SetupLeadCustormer = new SetupLeadCustormer();

                    SetupLeadCustormer.CompanyName = CurrentUser.CompanyName;

                    SetupLeadCustormer.CustomerName = _customer.FirstName + " " + _customer.LastName;
                    if (!string.IsNullOrWhiteSpace(_customer.BusinessName))
                    {
                        SetupLeadCustormer.CustomerName = _customer.BusinessName;
                    }
                    SetupLeadCustormer.CustomerNo = _customer.CustomerNo;
                    SetupLeadCustormer.ToEmail = _customer.EmailAddress;
                    SetupLeadCustormer.InvoiceId = StrInvoiceId;
                    SetupLeadCustormer.PaymentMethod = tr.PaymentMethod;
                    SetupLeadCustormer.AmountPaid = Math.Round(AmountPaid, 2);
                    SetupLeadCustormer.TotalAmount = Math.Round(paymentInfo.TotalDueAmount, 2);
                    double TotalBalanceDueModel = paymentInfo.TotalDueAmount - paymentInfo.CollectedAmount;
                    if (TotalBalanceDueModel > 0)
                    {
                        SetupLeadCustormer.BalanceDue = Math.Round(TotalBalanceDueModel, 2);
                    }
                    else
                    {
                        SetupLeadCustormer.BalanceDue = Math.Round(0.00, 2);
                    }
                    SetupLeadCustormer.TransactionId = "";
                    if (tr.PaymentMethod == LabelHelper.PaymentMethod.Check)
                    {
                        SetupLeadCustormer.TransactionId = paymentInfo.ChequeNumber;
                    }
                    SetupLeadCustormer.CustomerId = _customer.CustomerId.ToString();
                    Guid SalesPersonId = new Guid();
                    if (Guid.TryParse(_customer.Soldby, out SalesPersonId) && SalesPersonId != new Guid())
                    {
                        Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(SalesPersonId);
                        if (emp != null)
                        {
                            SetupLeadCustormer.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);
                            if (emp.Email.IsValidEmailAddress())
                            {
                                SetupLeadCustormer.ToSalesPersonsEmail = emp.Email;
                            }
                            else if (emp.UserName.IsValidEmailAddress())
                            {
                                SetupLeadCustormer.ToSalesPersonsEmail = emp.UserName;
                            }
                        }
                    }
                    bool IsEmail = _Util.Facade.MailFacade.EmailToSuccessTransaction(SetupLeadCustormer, CurrentUser.CompanyId.Value);
                    #region Email Send User Log
                    string LogMessage = "";
                    if (IsEmail) { LogMessage = "payment received confirmation email sent successfully"; }
                    else { LogMessage = "Payment receipt send failed"; }
                    LogMessage = LogMessage + " and collected amount $" + AmountPaid.ToString("N2") + " from batch no: " + BatchNo + " payment method: " + tr.PaymentMethod.ToLower() + " By " + CurrentUser.GetFullName() + " uploading excel file";
                    string EmailRef = null;
                    if (InvoiceList.Count == 1) { EmailRef = InvoiceList[0].InvoiceId; }
                    bool IsFlag = true;
                    if (paymentInfo.InvoiceType.ToLower() == "others") { IsFlag = false; }
                    base.AddUserActivityForCustomer(LogMessage, "CollectPaymentRMRInvoicesFromFile,Transaction", _customer.CustomerId, _customer.Id, EmailRef, IsFlag);
                    #endregion
                    #endregion
                }
            }
            #endregion
            return Json(new { result = ReturnResult, message = ReturnMessage });
        }

        [Authorize]
        [HttpPost]
        public JsonResult RMRCollectPaymentByCCandACH(List<string> InvoiceIdList, string PaymentType)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!CurrentUser.CompanyId.HasValue)
            {
                return Json(new { result = false, message = "Unauthorized! access is denied." });
            }
            bool ReturnResult = false;
            string ReturnMessage = "Transaction unsuccessful.";
            string BatchNo = DateTime.UtcNow.UTCToClientTime().ToString("yyMMddHHmmss").GenerateInvoiceBatchNo();
            #region Collect Payments Section
            if (InvoiceIdList != null && InvoiceIdList.Count > 0 && !string.IsNullOrWhiteSpace(PaymentType))
            {
                string Description = string.Format("Received by {0}", CurrentUser.GetFullName());
                foreach (string InvId in InvoiceIdList)
                {
                    var _inv = _Util.Facade.InvoiceFacade.GetByInvoiceId(InvId);
                    if (_inv == null || _inv.CompanyId != CurrentUser.CompanyId.Value) { continue; }
                    Customer _Cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(_inv.CustomerId);
                    double AmountPaid = 0;
                    int PaymentInfoId = 0;
                    ReceivePaymentModel Model = new ReceivePaymentModel();
                    List<Invoice> InvoiceList = new List<Invoice>();
                    RecurringBillingSchedule recurrig = new RecurringBillingSchedule();
                    if (_Cus != null && !string.IsNullOrWhiteSpace(_Cus.PaymentMethod))
                    {
                        recurrig = _Util.Facade.CustomerFacade.GetRecurringBillingScheduleByInvoiceID(_inv.InvoiceId);

                        string StrPaymentType = "";
                        #region Checking Valid Payment Type Credit Card or ACH
                        if (recurrig == null)
                        {
                            if (int.TryParse(_Cus.PaymentMethod, out PaymentInfoId) && PaymentInfoId > 0)
                            {
                                var PPC = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentProfileByPaymentInfoId(PaymentInfoId);
                                if (PPC != null && PPC.Type.Length > 2)
                                {
                                    string strpaymentValue = PPC.Type.Substring(0, 3);
                                    if (strpaymentValue.ToLower() == "cc_" || strpaymentValue.ToLower() == "ach")
                                    {
                                        StrPaymentType = PPC.Type;
                                    }
                                    else
                                    {
                                        PaymentInfoId = 0;
                                    }
                                }
                            }
                            else if (_Cus.PaymentMethod == LabelHelper.InvoiceFor.CreditCard || _Cus.PaymentMethod == LabelHelper.InvoiceFor.ACH || _Cus.PaymentMethod == LabelHelper.InvoiceFor.DebitCard)
                            {
                                var CustomerPaymentData = _Util.Facade.PaymentInfoCustomerFacade.GetByCustomerIdAndPayfor(_Cus.CustomerId, "MMR");
                                if (CustomerPaymentData != null)
                                {
                                    var PPC = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentProfileByPaymentInfoId(CustomerPaymentData.PaymentInfoId);
                                    if (PPC != null && PPC.Type.Length > 2)
                                    {
                                        string strpaymentValue = PPC.Type.Substring(0, 3);
                                        if (strpaymentValue.ToLower() == "cc_" || strpaymentValue.ToLower() == "ach")
                                        {
                                            StrPaymentType = PPC.Type;
                                            PaymentInfoId = CustomerPaymentData.PaymentInfoId;
                                        }
                                        else
                                        {
                                            PaymentInfoId = 0;
                                        }
                                    }
                                }
                            }
                            else if (_Cus.PaymentMethod.Length > 2)
                            {
                                string strpaymentValue = _Cus.PaymentMethod.Substring(0, 3);
                                if (strpaymentValue.ToLower() == "cc_" || strpaymentValue.ToLower() == "ach")
                                {
                                    StrPaymentType = _Cus.PaymentMethod;
                                    PaymentInfoId = _Util.Facade.PaymentInfoFacade.GetPaymentInfoIdByPaymentProfileCustomerType(_Cus.PaymentMethod);
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(recurrig.PaymentMethod) && recurrig.PaymentMethod.Length > 2)
                            {
                                string strpaymentValue = recurrig.PaymentMethod.Substring(0, 3);
                                if (strpaymentValue.ToLower() == "cc_" || strpaymentValue.ToLower() == "ach")
                                {
                                    StrPaymentType = recurrig.PaymentMethod;
                                    PaymentInfoId = recurrig.CustomerPaymentProfileId.HasValue ? recurrig.CustomerPaymentProfileId.Value : 0;
                                }
                            }
                        }
                        #endregion
                        #region Customer Credit Apply First
                        double CustomerCreditAmount = _Util.Facade.TransactionFacade.GetCustomerCreditAmountByCustomerIdWithBoolValue(_Cus.CustomerId, true, false);
                        if (CustomerCreditAmount > 0 && _inv.IsARBInvoice.HasValue && _inv.IsARBInvoice.Value)
                        {
                            InvoiceList = _Util.Facade.InvoiceFacade.GetManuallyUnpaidRecurringBillingInvoiceListByCustomerId(_Cus.CustomerId, CurrentUser.CompanyId.Value);
                            if (InvoiceList != null && InvoiceList.Count > 0)
                            {
                                InvoiceList = InvoiceList.OrderBy(x => x.Id).ToList();
                                var controller = DependencyResolver.Current.GetService<RecurringBillingController>();
                                foreach (var InvItem in InvoiceList)
                                {
                                    controller.RMRCreditApplied(InvItem, _Cus, CurrentUser.UserId, CurrentUser.CompanyId.Value);
                                }
                            }
                        }
                        #endregion
                        else if (PaymentInfoId < 1)
                        {
                            _inv.InvoiceFor = LabelHelper.InvoiceFor.Invoice;
                            _Util.Facade.InvoiceFacade.UpdateInvoice(_inv);
                            continue;
                        }
                        if (_inv.IsARBInvoice.HasValue && _inv.IsARBInvoice.Value)
                        {
                            InvoiceList = _Util.Facade.InvoiceFacade.GetManuallyUnpaidRecurringBillingInvoiceListByCustomerId(_inv.CustomerId, CurrentUser.CompanyId.Value);
                        }
                        if (InvoiceList != null && InvoiceList.Count > 0 && PaymentInfoId > 0)
                        {

                            Model.CompanyId = CurrentUser.CompanyId.Value;
                            Model.CustomerId = _Cus.Id;
                            Model.CustomerGId = _Cus.CustomerId;

                            foreach (var item in InvoiceList)
                            {
                                AmountPaid += item.BalanceDue.HasValue ? item.BalanceDue.Value : 0;
                            }

                            PaymentInfo Pinfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoById(PaymentInfoId);
                            if (Pinfo == null)
                            {
                                continue;
                            }

                            if (!string.IsNullOrWhiteSpace(Pinfo.AccountName)
                                && !string.IsNullOrWhiteSpace(Pinfo.BankAccountType)
                                && !string.IsNullOrWhiteSpace(Pinfo.RoutingNo)
                                && !string.IsNullOrWhiteSpace(Pinfo.AcountNo))
                            {
                                Model.ACHInfo = new ACHInfo();
                                Model.ACHInfo.AccountName = Pinfo.AccountName;
                                Model.ACHInfo.AccountType = Pinfo.BankAccountType;
                                Model.ACHInfo.RoutingNo = Pinfo.RoutingNo;
                                Model.ACHInfo.AccountNo = Pinfo.AcountNo;
                                Model.ACHInfo.ECheckType = Pinfo.EcheckType;
                                Model.ACHInfo.BankName = Pinfo.BankName;

                                Model.ACHInfo.Company = _Cus.BusinessName;
                                Model.ACHInfo.Phone = _Cus.PrimaryPhone;
                                Model.ACHInfo.BillingAddress = _Cus.Street;
                                Model.ACHInfo.City = _Cus.City;
                                Model.ACHInfo.State = _Cus.State;
                                Model.ACHInfo.Zipcode = _Cus.ZipCode;

                                Model.PaymentMethod = LabelHelper.PaymentMethod.ACH;
                            }
                            else if (!string.IsNullOrWhiteSpace(Pinfo.AccountName)
                                && !string.IsNullOrWhiteSpace(Pinfo.CardNumber)
                                && !string.IsNullOrWhiteSpace(Pinfo.CardExpireDate)
                                && !string.IsNullOrWhiteSpace(Pinfo.CardSecurityCode))
                            {
                                Model.CardInfo = new CardInfo();
                                Model.CardInfo.CardNumber = Pinfo.CardNumber;
                                Model.CardInfo.ExpiredDate = Pinfo.CardExpireDate;
                                Model.CardInfo.SecurityCode = Pinfo.CardSecurityCode;
                                Model.CardInfo.NameOnCard = Pinfo.AccountName;
                                Model.CardInfo.CardType = Pinfo.CardType;

                                Model.CardInfo.Company = _Cus.BusinessName;
                                Model.CardInfo.Phone = _Cus.PrimaryPhone;
                                Model.CardInfo.BillingAddress = _Cus.Street;
                                Model.CardInfo.City = _Cus.City;
                                Model.CardInfo.State = _Cus.State;
                                Model.CardInfo.Zipcode = _Cus.ZipCode;

                                Model.PaymentMethod = LabelHelper.PaymentMethod.CreditCard;
                            }
                            else
                            {
                                continue;
                            }

                            if (AmountPaid > 0 && Model.CardInfo != null)
                            {
                                Model.CardInfo.Amount = Math.Round(AmountPaid, 2);
                                Model.CardInfo.FirstName = _Cus.FirstName;
                                Model.CardInfo.Lastname = _Cus.LastName;
                                Model.CardInfo.CustomerId = _Cus.Id.ToString();
                                Model.CardInfo.EmailAddress = _Cus.EmailAddress;
                                Model.CardInfo.InvoiceNo = _inv.InvoiceId;
                                Model.CardInfo.Company = _Cus.BusinessName;
                                Model.CardInfo.Phone = _Cus.PrimaryPhone;
                                Model.CardInfo.BillingAddress = _Cus.Street;
                                Model.CardInfo.City = _Cus.City;
                                Model.CardInfo.State = _Cus.State;
                                Model.CardInfo.Zipcode = _Cus.ZipCode;
                                Model.CardInfo.Description = string.Format("Received by {0}", CurrentUser.GetFullName());
                                if (!string.IsNullOrWhiteSpace(Model.CardInfo.Description))
                                {
                                    Model.Description = Model.CardInfo.Description;
                                }

                            }
                            else if (AmountPaid > 0 && Model.ACHInfo != null)
                            {
                                Model.ACHInfo.Amount = Math.Round(AmountPaid, 2);
                                Model.ACHInfo.FirstName = _Cus.FirstName;
                                Model.ACHInfo.Lastname = _Cus.LastName;
                                Model.ACHInfo.CustomerId = _Cus.Id.ToString();
                                Model.ACHInfo.EmailAddress = _Cus.EmailAddress;
                                Model.ACHInfo.InvoiceNo = _inv.InvoiceId;
                                Model.ACHInfo.Company = _Cus.BusinessName;
                                Model.ACHInfo.Phone = _Cus.PrimaryPhone;
                                Model.ACHInfo.BillingAddress = _Cus.Street;
                                Model.ACHInfo.City = _Cus.City;
                                Model.ACHInfo.State = _Cus.State;
                                Model.ACHInfo.Zipcode = _Cus.ZipCode;
                                Model.ACHInfo.Description = string.Format("Received by {0}", CurrentUser.GetFullName());
                                if (!string.IsNullOrWhiteSpace(Model.ACHInfo.Description))
                                {
                                    Model.Description = Model.ACHInfo.Description;
                                }
                            }
                            else { continue; }
                        }
                        else { continue; }
                    }
                    else { continue; }

                    if (InvoiceList == null || InvoiceList.Count < 1) { continue; }
                    #region Insert Into Transaction Queue
                    string Starttime = DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    string Endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    List<TransactionQueue> transqueList = new List<TransactionQueue>();
                    transqueList = _Util.Facade.TransactionFacade.GetTransactionQueueCustomerId(_Cus.CustomerId, Starttime, Endtime, Math.Round(AmountPaid, 2));
                    if (transqueList.Count > 0)
                    {
                        continue;
                    }
                    #region Capture Payment By Api
                    string TransactionReference = "";
                    ReceivePaymentResponse response = new ReceivePaymentResponse();
                    response = _Util.Facade.ReceivePaymentFacade.ReceivePayment(Model);
                    if (!response.TransactionSuccess)
                        continue;
                    else
                    {
                        TransactionReference = response.TransactionId;
                    }
                    #endregion
                    TransactionQueue transque = new TransactionQueue()
                    {
                        CustomerId = _Cus.CustomerId,
                        Amount = Math.Round(AmountPaid, 2),
                        InvoiceId = TransactionReference,
                        CreatedBy = CurrentUser.UserId,
                        CreatedDate = DateTime.Now
                    };
                    _Util.Facade.TransactionFacade.InsertTransactionQueue(transque);
                    #endregion

                    #region Transaction Section
                    Transaction tr = new Transaction()
                    {
                        Status = "Closed",
                        Type = "Payment",
                        TransacationDate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = _Cus.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        Amount = Math.Round(AmountPaid, 2),
                        ReferenceNo = TransactionReference,
                        CardTransactionId = TransactionReference,
                        AddedBy = User.Identity.Name,
                        AddedDate = DateTime.Now.UTCCurrentTime(),
                        CreatedBy = CurrentUser.UserId,
                        PaymentMethod = Model.PaymentMethod,
                        PaymentInfoId = PaymentInfoId,
                        Note = Model.Description
                    };
                    tr.Id = _Util.Facade.TransactionFacade.InsertTransaction(tr);

                    #endregion
                    #region transaction History Section
                    List<TransactionHistory> trhistory = new List<TransactionHistory>();
                    InvoiceList = InvoiceList.OrderBy(x => x.Id).ToList();
                    double TotalCollectedAmount = Math.Round(AmountPaid, 2);
                    foreach (var item in InvoiceList)
                    {
                        string TempStatus = item.Status;
                        double PerPayment = 0;
                        if (item.BalanceDue.HasValue && item.BalanceDue.Value > 0 && TotalCollectedAmount > 0)
                        {
                            if (TotalCollectedAmount > item.BalanceDue.Value)
                            {
                                PerPayment = Math.Round(item.BalanceDue.Value, 2);
                                TotalCollectedAmount -= PerPayment;
                            }
                            else
                            {
                                PerPayment = Math.Round(TotalCollectedAmount, 2);
                                TotalCollectedAmount = 0;
                            }
                        }
                        else { continue; }
                        #region Transaction History Log
                        trhistory.Add(new TransactionHistory()
                        {
                            Amout = Math.Round(PerPayment, 2),
                            InvoiceId = item.Id,
                            TransactionId = tr.Id,
                            Balance = Math.Round(item.BalanceDue.HasValue ? item.BalanceDue.Value : 0, 2),
                            ReceivedBy = CurrentUser.UserId,
                            InvoiceTotal = item.TotalAmount.HasValue ? item.TotalAmount.Value : 0,
                            InvoiceBalanceDue = item.BalanceDue.HasValue ? item.BalanceDue.Value : 0
                        });
                        #region Update Invocie
                        item.BalanceDue = Math.Round(item.BalanceDue.HasValue ? item.BalanceDue.Value : 0, 2) - Math.Round(PerPayment, 2);
                        item.Status = item.BalanceDue == 0 ? LabelHelper.InvoiceStatus.Paid : LabelHelper.InvoiceStatus.Partial;
                        item.PaymentType = tr.PaymentMethod;
                        item.BatchNumber = BatchNo;
                        _Util.Facade.InvoiceFacade.UpdateInvoice(item);
                        #region User Log
                        if (item != null && TempStatus != item.Status)
                        {
                            string LogMsg = "Invoice status changed from " + TempStatus.ToLower() + " to " + item.Status.ToLower() + " #InvoiceId: " + item.InvoiceId + " collected amount $" + PerPayment.ToString("N2") + " using " + tr.PaymentMethod.ToLower() + " by " + CurrentUser.GetFullName() + ".";
                            base.AddUserActivityForCustomer(LogMsg, "RMRCollectPaymentByCCandACH,Transaction", _Cus.CustomerId, _Cus.Id, item.InvoiceId, true);
                        }
                        #endregion
                        #endregion

                        #region Insert CustomerSnapshot
                        CustomerSnapshot objInvoicePayment = new CustomerSnapshot()
                        {
                            CustomerId = _Cus.CustomerId,
                            CompanyId = CurrentUser.CompanyId.Value,
                            Description = "Invoice " + string.Format("<a onclick=OpenTopToBottomModal('{2}/Invoice/AddInvoice?id={0}&CustomerId={1}') style='cursor: pointer;'>", item.Id, _Cus.Id, _Cus.CustomerId, AppConfig.DomainSitePath) + "<b>" + item.InvoiceId + "</b>" + "</a>" + " created for " + "<b>" + _Cus.FirstName + " " + _Cus.LastName + "</b>",
                            Logdate = DateTime.Now.UTCCurrentTime(),
                            Updatedby = User.Identity.Name,
                            Type = "InvoicePaymentHistory"
                        };
                        _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objInvoicePayment);
                        #endregion
                        #region Notification
                        #region Insert notification
                        Notification notification = new Notification()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            NotificationId = Guid.NewGuid(),
                            Type = LabelHelper.NotificationType.Customer,
                            Who = _Cus.CustomerId,
                            What = string.Format(@"A lead <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just make payment to an invoice 
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a>", "{0}"
                                     , _Cus.Id
                                     , item.Id
                                     , item.InvoiceId, AppConfig.DomainSitePath),
                            NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + _Cus.Id + "#InvoiceTab"
                        };
                        _Util.Facade.NotificationFacade.InsertNotification(notification);

                        #endregion
                        #region set user to notification
                        NotificationUser nu = new NotificationUser()
                        {
                            NotificationId = notification.NotificationId,
                            IsRead = false,
                            NotificationPerson = CurrentUser.UserId,
                        };
                        _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                        #endregion
                        #endregion
                    }
                    if (trhistory.Count() > 0)
                    {
                        _Util.Facade.TransactionFacade.InsertTransactionHistoryList(trhistory);
                    }
                    #endregion
                    #endregion
                    ReturnResult = true;
                    ReturnMessage = "Payment received successfully.";
                    if ((recurrig != null && recurrig.IsEReceipt.HasValue && recurrig.IsEReceipt.Value && AmountPaid > 0) || (_Cus.IsReceivePaymentMail.HasValue && _Cus.IsReceivePaymentMail.Value && AmountPaid > 0))
                    {
                        #region Send Payment Recipt Email 

                        SetupLeadCustormer SetupLeadCustormer = new SetupLeadCustormer();
                        SetupLeadCustormer.CompanyName = CurrentUser.CompanyName;
                        SetupLeadCustormer.CustomerName = _Cus.FirstName + " " + _Cus.LastName;
                        if (!string.IsNullOrWhiteSpace(_Cus.BusinessName))
                        {
                            SetupLeadCustormer.CustomerName = _Cus.BusinessName;
                        }
                        SetupLeadCustormer.CustomerNo = _Cus.CustomerNo;
                        string StrToEmail = "";
                        if (recurrig != null && recurrig.EmailAddress.IsValidEmailAddress())
                        {
                            StrToEmail = recurrig.EmailAddress;
                        }
                        else
                        {
                            StrToEmail = _Cus.EmailAddress;
                        }
                        SetupLeadCustormer.ToEmail = StrToEmail;
                        SetupLeadCustormer.InvoiceId = _inv.InvoiceId;
                        SetupLeadCustormer.PaymentMethod = tr.PaymentMethod;
                        SetupLeadCustormer.AmountPaid = Math.Round(AmountPaid, 2);
                        SetupLeadCustormer.TotalAmount = Math.Round(AmountPaid, 2);
                        SetupLeadCustormer.BalanceDue = Math.Round(0.00, 2);
                        SetupLeadCustormer.TransactionId = TransactionReference;
                        SetupLeadCustormer.CustomerId = _Cus.CustomerId.ToString();
                        Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                        if (emp != null)
                        {
                            SetupLeadCustormer.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);
                            if (emp.Email.IsValidEmailAddress())
                            {
                                SetupLeadCustormer.ToSalesPersonsEmail = emp.Email;
                            }
                            else if (emp.UserName.IsValidEmailAddress())
                            {
                                SetupLeadCustormer.ToSalesPersonsEmail = emp.UserName;
                            }
                        }
                        bool IsEmail = _Util.Facade.MailFacade.EmailToSuccessTransaction(SetupLeadCustormer, CurrentUser.CompanyId.Value);
                        #region Email Send User Log
                        string LogMessage = "";
                        if (IsEmail) { LogMessage = "Payment received confirmation email sent successfully"; }
                        else { LogMessage = "Payment receipt send failed"; }
                        LogMessage = LogMessage + " and collected amount $" + AmountPaid.ToString("N2") + " from batch no: " + BatchNo + " payment method: " + tr.PaymentMethod.ToLower() + " by " + CurrentUser.GetFullName() + ".";
                        base.AddUserActivityForCustomer(LogMessage, "RMRCollectPaymentByCCandACH,Transaction", _Cus.CustomerId, _Cus.Id, _inv.InvoiceId, true);
                        #endregion
                        #endregion
                    }
                }
            }
            #endregion
            return Json(new { result = ReturnResult, message = ReturnMessage });

        }
    }
}