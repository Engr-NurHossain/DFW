using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HS.Framework;
using HS.Entities;
using HS.Web.UI.Helper;
using System.Configuration;
using Rotativa;
using System.Text.RegularExpressions;
using Rotativa.Options;
using HS.Framework.Utils;
using Newtonsoft.Json;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using Excel = HS.Web.UI.Helper.ExcelFormatHelper;
using System.Collections;
using OS.AWS.S3.Services;
using OS.AWS.S3;
using System.Threading;
using EO.Internal;
using System.Threading.Tasks;

namespace HS.Web.UI.Controllers
{
    public class BookingController : BaseController
    {
        // GET: Booking
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
        [Authorize]
        public JsonResult GetRugShapeListByKey(string key)
        {
            string result = "[]";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<Lookup> shape = _Util.Facade.BookingFacade.GetRugShapeListBySearchKey(key);
            if (shape.Count > 0)
                result = JsonConvert.SerializeObject(shape);

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        [Authorize]
        public JsonResult GetPackageList()
        {
            string result = "[]";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var package = _Util.Facade.BookingFacade.GetPackageAndInclude();

            result = JsonConvert.SerializeObject(package);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }
        //LeadBooking Partial Action 
        public ActionResult LeadBookingPartial(int? CustomerId)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<Booking> model = new List<Booking>();

            if (CustomerId.HasValue)
            {
                //bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId.Value, CurrentUser.CompanyId.Value);
                //if (!res)
                //{
                //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                //}

                Customer customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);
                if (customerInfo != null)
                {
                    model = _Util.Facade.BookingFacade.GetAllBookingListByCustomerIdAndCompanyId(customerInfo.CustomerId, CurrentUser.CompanyId.Value);
                    ViewBag.CustomerId = customerInfo.Id;
                }
            }

            return PartialView("_LeadBookingPartial", model);
        }

        //public ActionResult LeadLogPartial(int? CustomerId)
        //{
        //    var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
        //    if (CurrentUser == null)
        //    {
        //        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
        //    }
        //    List<UserActivity> model = new List<UserActivity>();

        //    if (CustomerId.HasValue)
        //    {
        //        bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId.Value, CurrentUser.CompanyId.Value);
        //        if (!res)
        //        {
        //            return PartialView("~/Views/Shared/_AccessDenied.cshtml");
        //        }

        //        Customer customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);
        //        if (customerInfo != null)
        //        {
        //            model = _Util.Facade.BookingFacade.GetAllUserActivityCustomerListByCustomerId(customerInfo.CustomerId);
        //            ViewBag.CustomerId = customerInfo.Id;
        //        }
        //    }

        //    return PartialView("_LeadBookingPartial", model);
        //}

        
        public ActionResult LoadLogPartial(bool? GetReport, string Start, string End, int pageno, int pagesize, string searchtxt, int CustomerId, string order,string logstartdate,string logenddate)
        {
            Customer CustomerModel = new Customer();
            CustomerModel = _Util.Facade.CustomerFacade.GetCustomerById(CustomerId);
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<string> invstatus = new List<string>();
            
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
            if (GetReport.HasValue && GetReport == true)
            {
                DataTable dt;
                if (!string.IsNullOrWhiteSpace(Start) && Start != "undefined" && !string.IsNullOrWhiteSpace(End) && End != "undefined")
                {
                    StartDate = Convert.ToDateTime(Start).SetZeroHour().ClientToUTCTime();
                    EndDate = Convert.ToDateTime(End).SetMaxHour().ClientToUTCTime();
                    dt = _Util.Facade.BookingFacade.GetAllUserActivityCustomerListByCustomerIdExport(CustomerModel.CustomerId, StartDate, EndDate, searchtxt);
                }
                else
                {
                    if (StartDate != new DateTime() && EndDate != new DateTime())
                    {
                        dt = _Util.Facade.BookingFacade.GetAllUserActivityCustomerListByCustomerIdExport(CustomerModel.CustomerId, StartDate, EndDate, searchtxt);
                    }
                    else
                    {
                        dt = _Util.Facade.BookingFacade.GetAllUserActivityCustomerListByCustomerIdExport(CustomerModel.CustomerId, null, null, searchtxt);
                    }

                }
                return MakeExcelFromDataTable(dt, "LogReport");
            }
             
            if(pageno == 0 || pageno < 0)
            {
                pageno = 1;
            }
             
            
            UserActivityCustomerModel Model = _Util.Facade.BookingFacade.GetAllUserActivityCustomerListByCustomerId(pageno, pagesize, StartDate, EndDate, searchtxt, CustomerModel.CustomerId, order,logstartdate,logenddate);
            ViewBag.CustomerId = CustomerId;
            ViewBag.PageNumber = pageno;
            ViewBag.OutOfNumber = 0;
            ViewBag.orderval = order;
            ViewBag.order = order;
            ViewBag.Logstartdate = logstartdate;
            ViewBag.Logenddate = logenddate;
            if (!string.IsNullOrWhiteSpace(searchtxt) && searchtxt != "undefined")
            {
                ViewBag.searchtxt = searchtxt;
            }
            else
            {
                ViewBag.searchtxt = "";
            }
            if (ViewBag.order == null)
            {
                ViewBag.order = "";
            }
            else
            {
                ViewBag.order = order;
            }
             
            if (Model.ListUserActivity != null && Model.ListUserActivity.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.InvoiceReportCountModel.TotalCount;
            }


            if ((int)ViewBag.PageNumber * pagesize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * pagesize;
            }
            ViewBag.Totalpagesize = (int)ViewBag.PageNumber * pagesize;
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / pagesize);

            List<SelectListItem> statusinv = new List<SelectListItem>();
            statusinv.Add(new SelectListItem()
            {
                Text = "Paid",
                Value = "Paid"
            });
            statusinv.Add(new SelectListItem()
            {
                Text = "Open",
                Value = "Open"
            });
            statusinv.Add(new SelectListItem()
            {
                Text = "Partial",
                Value = "Partial"
            });
            ViewBag.statusinv = statusinv;
            return View("_LoadLogPartial", Model);
        }

        #region MakeExcel
        private FileContentResult MakeExcelFromDataTable(DataTable dtResult, string ReportFor)
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

                    return File(fileContents, Excel.Format("ExcelFormat"), fName);

                }
                else
                {
                    byte[] fileContents = new byte[1];
                    return File(fileContents, Excel.Format("ExcelFormat"), "empty.xlsx");
                }
            }
        }

        private FileContentResult MakeExcelFromDataSet(DataSet dsResult, string ReportFor)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                if (dsResult != null)
                {
                    int cus = 0;
                    //wb.Worksheets.Add(dsResult.Tables[1]);
                    //wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //wb.Style.Font.Bold = true;
                    for (int k = 0; k < dsResult.Tables.Count; k++)
                    {
                        DataTable dt = dsResult.Tables[k];
                        IXLWorksheet Sheet = wb.Worksheets.Add(dt.Columns[cus].ColumnName);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                Sheet.Cell((i + 2), (j + 1)).Value = dt.Rows[i][j].ToString();
                            }
                        }
                        cus = cus + 1;
                    }

                    MemoryStream memorystreem = new MemoryStream();
                    wb.SaveAs(memorystreem);
                    var fName = string.Format("{0}-{1}.xlsx", ReportFor, DateTime.Now.UTCCurrentTime().ToString("MM/dd/yyyy"));

                    byte[] fileContents = memorystreem.ToArray();

                    return File(fileContents, Excel.Format("ExcelFormat"), fName);

                }
                else
                {
                    byte[] fileContents = new byte[1];
                    return File(fileContents, Excel.Format("ExcelFormat"), "empty.xlsx");
                }
            }

        }

        #endregion



        //Add Lead Booking
        [Authorize]
        public ActionResult AddLeadBooking(int? id,string BookingId, int CustomerId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            CreateBooking model;
            Customer tempCustomer = new Customer();



            if (id.HasValue && id.Value > 0)
            {
                model = new CreateBooking();
                model.Booking = _Util.Facade.BookingFacade.GetById(id.Value);
                if (model.Booking == null || model.Booking.CompanyId != currentLoggedIn.CompanyId.Value)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.Booking.CustomerId);
                if(tempCustomer == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model.BookingExtraItem = _Util.Facade.BookingFacade.GetBookingExtraItemListByBookingId(id.Value); 
                model.Booking.CustomerName = tempCustomer.FirstName + " " + tempCustomer.LastName;
               

                model.BookingDetailsList = _Util.Facade.BookingFacade.GetBookingDetialsListByBookingId(model.Booking.BookingId);
                model.EmailAddress = model.Booking.EmailAddress;

                //ViewBag.ShippingAddress = model.Booking.ShippingAddress;
                ViewBag.Value = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(currentLoggedIn.CompanyId.Value);
                if (string.IsNullOrWhiteSpace(model.EmailAddress))
                {
                    model.EmailAddress = tempCustomer.EmailAddress;
                    model.Booking.EmailAddress = tempCustomer.EmailAddress;
                }
                var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(currentLoggedIn.CompanyId.Value);
                CustomerAddress tempCustomerAddressBilling = new CustomerAddress();
                tempCustomerAddressBilling = _Util.Facade.CustomerFacade.GetCustomerAddressByCustomerIdRefIdAddressType(tempCustomer.CustomerId, model.Booking.BookingId, "BillingAddress");
                if (tempCustomerAddressBilling != null)
                {
                    tempCustomer.Street = tempCustomerAddressBilling.Street;
                    tempCustomer.City = tempCustomerAddressBilling.City;
                    tempCustomer.State = tempCustomerAddressBilling.State;
                    tempCustomer.ZipCode = tempCustomerAddressBilling.ZipCode;
                    tempCustomer.County = tempCustomerAddressBilling.County;
                    tempCustomer.FirstName = tempCustomerAddressBilling.FirstName;
                    tempCustomer.LastName = tempCustomerAddressBilling.LastName;
                    tempCustomer.BusinessName = tempCustomerAddressBilling.BusinessName;
                    model.Booking.BillingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "BillingAddress", AddressTemplate);
                }
                CustomerAddress tempCustomerAddressPickUp = new CustomerAddress();
                tempCustomerAddressPickUp = _Util.Facade.CustomerFacade.GetCustomerAddressByCustomerIdRefIdAddressType(tempCustomer.CustomerId, model.Booking.BookingId, "PickUpLocation");
                if (tempCustomerAddressPickUp != null)
                {
                    tempCustomer.Street = tempCustomerAddressPickUp.Street;
                    tempCustomer.City = tempCustomerAddressPickUp.City;
                    tempCustomer.State = tempCustomerAddressPickUp.State;
                    tempCustomer.ZipCode = tempCustomerAddressPickUp.ZipCode;
                    tempCustomer.County = tempCustomerAddressPickUp.County;
                    tempCustomer.FirstName = tempCustomerAddressPickUp.FirstName;
                    tempCustomer.LastName = tempCustomerAddressPickUp.LastName;
                    tempCustomer.BusinessName = tempCustomerAddressPickUp.BusinessName;
                    model.Booking.PickUpLocation = AddressHelper.MakeCustomerAddress(tempCustomer, "PickUpLocation", AddressTemplate);
                }
                CustomerAddress tempCustomerAddressDropOff = new CustomerAddress();
                tempCustomerAddressDropOff = _Util.Facade.CustomerFacade.GetCustomerAddressByCustomerIdRefIdAddressType(tempCustomer.CustomerId, model.Booking.BookingId, "DropOffLocation");
                if (tempCustomerAddressDropOff != null)
                {
                    tempCustomer.Street = tempCustomerAddressDropOff.Street;
                    tempCustomer.City = tempCustomerAddressDropOff.City;
                    tempCustomer.State = tempCustomerAddressDropOff.State;
                    tempCustomer.ZipCode = tempCustomerAddressDropOff.ZipCode;
                    tempCustomer.County = tempCustomerAddressDropOff.County;
                    tempCustomer.FirstName = tempCustomerAddressDropOff.FirstName;
                    tempCustomer.LastName = tempCustomerAddressDropOff.LastName;
                    tempCustomer.BusinessName = tempCustomerAddressDropOff.BusinessName;
                    model.Booking.DropOffLocation = AddressHelper.MakeCustomerAddress(tempCustomer, "DropOffLocation", AddressTemplate);
                }



                //model.Booking.PickUpLocation = model.Booking.BillingAddress;
                //model.Booking.DropOffLocation = model.Booking.BillingAddress;


            }
            else
            {
                tempCustomer = _Util.Facade.CustomerFacade.GetCustomerById(CustomerId);
                if(tempCustomer == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }

                model = new CreateBooking();
                model.BookingExtraItem = new List<BookingExtraItem>();
                model.EmailAddress = tempCustomer.EmailAddress;
                model.Booking = new Booking()
                {
                    CustomerId = tempCustomer.CustomerId,
                    CustomerName = tempCustomer.FirstName + " " + tempCustomer.LastName,
                    CompanyId = currentLoggedIn.CompanyId.Value,
                    Amount = 0,
                    Status = "Init",
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CreatedBy = currentLoggedIn.UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedBy = currentLoggedIn.UserId,
                    EmailAddress = tempCustomer.EmailAddress
                };
                tempCustomer.CustomerNo = "";
                ViewBag.Value = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(currentLoggedIn.CompanyId.Value);
                var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(currentLoggedIn.CompanyId.Value);
                model.Booking.BillingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "BillingAddress", AddressTemplate);  
                model.Booking.PickUpLocation = model.Booking.BillingAddress;
                model.Booking.DropOffLocation = model.Booking.BillingAddress;

                CustomerAddress tempCustomerAddressBilling = new CustomerAddress();
                //tempCustomerAddressBilling = _Util.Facade.CustomerFacade.GetCustomerAddressByCustomerIdRefIdAddressType(tempCustomer.CustomerId, model.Booking.BookingId, "BillingAddress");
                //if(tempCustomerAddressBilling == null)
                //{
                //    tempCustomerAddressBilling = new CustomerAddress();
                //}
                tempCustomerAddressBilling.CustomerId = model.Booking.CustomerId;
                tempCustomerAddressBilling.RefId = model.Booking.BookingId;
                tempCustomerAddressBilling.AddressType = "BillingAddress";
                tempCustomerAddressBilling.Street = tempCustomer.Street;
                tempCustomerAddressBilling.City = tempCustomer.City;
                tempCustomerAddressBilling.State = tempCustomer.State;
                tempCustomerAddressBilling.ZipCode = tempCustomer.ZipCode;
                tempCustomerAddressBilling.County = tempCustomer.County;
                tempCustomerAddressBilling.FirstName = tempCustomer.FirstName;
                tempCustomerAddressBilling.LastName = tempCustomer.LastName;
                tempCustomerAddressBilling.BusinessName = tempCustomer.BusinessName;
                //if (tempCustomerAddressBilling != null && (!string.IsNullOrWhiteSpace(tempCustomerAddressBilling.Street) || !string.IsNullOrWhiteSpace(tempCustomerAddressBilling.City) || !string.IsNullOrWhiteSpace(tempCustomerAddressBilling.State) || !string.IsNullOrWhiteSpace(tempCustomerAddressBilling.ZipCode) || !string.IsNullOrWhiteSpace(tempCustomerAddressBilling.County)) )
                //{

                //    tempCustomer.Street = tempCustomerAddressBilling.Street;
                //    tempCustomer.City = tempCustomerAddressBilling.City;
                //    tempCustomer.State = tempCustomerAddressBilling.State;
                //    tempCustomer.ZipCode = tempCustomerAddressBilling.ZipCode;
                //    tempCustomer.County = tempCustomerAddressBilling.County;
                //    tempCustomer.FirstName = tempCustomerAddressBilling.FirstName;
                //    tempCustomer.LastName = tempCustomerAddressBilling.LastName;
                //    tempCustomer.BusinessName = tempCustomerAddressBilling.BusinessName;
                //    model.Booking.BillingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "BillingAddress", AddressTemplate);
                    

                //}
                CustomerAddress tempCustomerAddressPickUp = new CustomerAddress();
                //tempCustomerAddressPickUp = _Util.Facade.CustomerFacade.GetCustomerAddressByCustomerIdRefIdAddressType(tempCustomer.CustomerId, model.Booking.BookingId, "PickUpLocation");
                //if (tempCustomerAddressPickUp == null)
                //{
                //    tempCustomerAddressPickUp = new CustomerAddress();
                //}
                tempCustomerAddressPickUp.CustomerId = model.Booking.CustomerId;
                tempCustomerAddressPickUp.RefId = model.Booking.BookingId;
                tempCustomerAddressPickUp.AddressType = "PickUpLocation";
                tempCustomerAddressPickUp.Street = tempCustomer.Street;
                tempCustomerAddressPickUp.City = tempCustomer.City;
                tempCustomerAddressPickUp.State = tempCustomer.State;
                tempCustomerAddressPickUp.ZipCode = tempCustomer.ZipCode;
                tempCustomerAddressPickUp.County = tempCustomer.County;
                tempCustomerAddressPickUp.FirstName = tempCustomer.FirstName;
                tempCustomerAddressPickUp.LastName = tempCustomer.LastName;
                tempCustomerAddressPickUp.BusinessName = tempCustomer.BusinessName;
                //if (tempCustomerAddressPickUp != null && (!string.IsNullOrWhiteSpace(tempCustomerAddressPickUp.Street) || !string.IsNullOrWhiteSpace(tempCustomerAddressPickUp.City) || !string.IsNullOrWhiteSpace(tempCustomerAddressPickUp.State) || !string.IsNullOrWhiteSpace(tempCustomerAddressPickUp.ZipCode) || !string.IsNullOrWhiteSpace(tempCustomerAddressPickUp.County)))
                //{
                //    tempCustomer.Street = tempCustomerAddressPickUp.Street;
                //    tempCustomer.City = tempCustomerAddressPickUp.City;
                //    tempCustomer.State = tempCustomerAddressPickUp.State;
                //    tempCustomer.ZipCode = tempCustomerAddressPickUp.ZipCode;
                //    tempCustomer.County = tempCustomerAddressPickUp.County;
                //    tempCustomer.FirstName = tempCustomerAddressPickUp.FirstName;
                //    tempCustomer.LastName = tempCustomerAddressPickUp.LastName;
                //    tempCustomer.BusinessName = tempCustomerAddressPickUp.BusinessName;
                //    model.Booking.PickUpLocation = AddressHelper.MakeCustomerAddress(tempCustomer, "BillingAddress", AddressTemplate);

                //}
                CustomerAddress tempCustomerAddressDropOff = new CustomerAddress();
                //tempCustomerAddressDropOff = _Util.Facade.CustomerFacade.GetCustomerAddressByCustomerIdRefIdAddressType(tempCustomer.CustomerId, model.Booking.BookingId, "DropOffLocation");
                //if (tempCustomerAddressDropOff == null)
                //{
                //    tempCustomerAddressDropOff = new CustomerAddress();
                //}
                tempCustomerAddressDropOff.CustomerId = model.Booking.CustomerId;
                tempCustomerAddressDropOff.RefId = model.Booking.BookingId;
                tempCustomerAddressDropOff.AddressType = "DropOffLocation";
                tempCustomerAddressDropOff.Street = tempCustomer.Street;
                tempCustomerAddressDropOff.City = tempCustomer.City;
                tempCustomerAddressDropOff.State = tempCustomer.State;
                tempCustomerAddressDropOff.ZipCode = tempCustomer.ZipCode;
                tempCustomerAddressDropOff.County = tempCustomer.County;
                tempCustomerAddressDropOff.FirstName = tempCustomer.FirstName;
                tempCustomerAddressDropOff.LastName = tempCustomer.LastName;
                tempCustomerAddressDropOff.BusinessName = tempCustomer.BusinessName;
                //if (tempCustomerAddressDropOff != null && (!string.IsNullOrWhiteSpace(tempCustomerAddressDropOff.Street) || !string.IsNullOrWhiteSpace(tempCustomerAddressDropOff.City) || !string.IsNullOrWhiteSpace(tempCustomerAddressDropOff.State) || !string.IsNullOrWhiteSpace(tempCustomerAddressDropOff.ZipCode) || !string.IsNullOrWhiteSpace(tempCustomerAddressDropOff.County)))
                //{
                //    tempCustomer.Street = tempCustomerAddressDropOff.Street;
                //    tempCustomer.City = tempCustomerAddressDropOff.City;
                //    tempCustomer.State = tempCustomerAddressDropOff.State;
                //    tempCustomer.ZipCode = tempCustomerAddressDropOff.ZipCode;
                //    tempCustomer.County = tempCustomerAddressDropOff.County;
                //    tempCustomer.FirstName = tempCustomerAddressDropOff.FirstName;
                //    tempCustomer.LastName = tempCustomerAddressDropOff.LastName;
                //    tempCustomer.BusinessName = tempCustomerAddressDropOff.BusinessName;
                //    model.Booking.DropOffLocation = AddressHelper.MakeCustomerAddress(tempCustomer, "BillingAddress", AddressTemplate);

                //}



                //model.Booking.ShippingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "ShippingAddress", AddressTemplate);

                model.Booking.Id = _Util.Facade.BookingFacade.InsertBooking(model.Booking);
               
                
               
                
                model.Booking.BookingId = model.Booking.Id.GenerateBookingNo();
                _Util.Facade.BookingFacade.UpdateBooking(model.Booking);
                if (tempCustomerAddressBilling != null)
                {
                    tempCustomerAddressBilling.RefId = model.Booking.BookingId;
                    model.Booking.BillingAddressID = _Util.Facade.CustomerFacade.InsertCustomerAddress(tempCustomerAddressBilling);

                }
                if (tempCustomerAddressPickUp != null)
                {
                    tempCustomerAddressPickUp.RefId = model.Booking.BookingId;
                    model.Booking.PickupLocationID = _Util.Facade.CustomerFacade.InsertCustomerAddress(tempCustomerAddressPickUp);
                }
                if (tempCustomerAddressDropOff != null)
                {
                    tempCustomerAddressDropOff.RefId = model.Booking.BookingId;
                    model.Booking.DropoffLocationID = _Util.Facade.CustomerFacade.InsertCustomerAddress(tempCustomerAddressDropOff);
                }

                model.Booking.PickUpDate = DateTime.UtcNow.AddDays(1).UTCToClientTime();
                if (model.Booking.PickUpDate.Value.DayOfWeek == 0)
                {
                    model.Booking.PickUpDate = model.Booking.PickUpDate.Value.AddDays(1);
                }
                model.Booking.DropOffDate = model.Booking.PickUpDate.Value.AddDays(5).UTCToClientTime();
                if (model.Booking.DropOffDate.Value.DayOfWeek == 0)
                {
                    model.Booking.DropOffDate = model.Booking.DropOffDate.Value.AddDays(1);
                }

                model.BookingDetailsList = new List<BookingDetails>();
            }

            #region View for TaxList
            
            List<SelectListItem> TaxListItem = new List<SelectListItem>();
            var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(tempCustomer.CustomerId, currentLoggedIn.CompanyId.Value);
            if (GetCityTaxList.Count > 0)
            {
                GetCityTaxList = GetCityTaxList.OrderBy(x => x.City).ToList();
                foreach (var item in GetCityTaxList)
                {
                    TaxListItem.Add(new SelectListItem()
                    {
                        Text = " [" + item.City.ToString() + "-" + item.State.ToString() + "]",
                        Value = item.Rate.ToString()
                    });
                }
            }
            else
            {
                Guid tempCustomerId = new Guid();
                if (tempCustomer != null)
                {
                    tempCustomerId = tempCustomer.CustomerId;
                }
                var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(currentLoggedIn.CompanyId.Value, tempCustomerId);
                if (GetSalesTax != null)
                {
                    TaxListItem.Add(new SelectListItem()
                    {
                        Text = GetSalesTax.SearchKey.ToString(),
                        Value = GetSalesTax.Value.ToString()
                    });
                }
            }
            TaxListItem.Add(new SelectListItem()
            {
                Text = "Non-Tax",
                Value = "0"
            });

            var GetOutOfStateTax = _Util.Facade.GlobalSettingsFacade.GetOutOfStateTax(currentLoggedIn.CompanyId.Value);
            if (GetOutOfStateTax != null)
            {
                TaxListItem.Add(new SelectListItem()
                {
                    Text = GetOutOfStateTax.SearchKey.ToString(),
                    Value = GetOutOfStateTax.Value.ToString()
                });
            }

            var GetNonProfitTax = _Util.Facade.GlobalSettingsFacade.GetNonProfitTax(currentLoggedIn.CompanyId.Value);
            if (GetNonProfitTax != null)
            {
                TaxListItem.Add(new SelectListItem()
                {
                    Text = GetNonProfitTax.SearchKey.ToString(),
                    Value = GetNonProfitTax.Value.ToString()
                });
            }
            ViewBag.TaxListItem = TaxListItem;
            #endregion

            #region Viewbag
            ViewBag.CustomerList = _Util.Facade.CustomerFacade
                .GetAllCustomersByCompanyId(currentLoggedIn.CompanyId.Value);

            ViewBag.EquipmentServiceList = _Util.Facade.EquipmentFacade
                .GetAllEquipmentServiceByCompanyId(currentLoggedIn.CompanyId.Value).Select(x =>
                  new SelectListItem()
                  {
                      Text = x.Name.ToString() + " " + x.SKU.ToString(),
                      Value = x.EquipmentId.ToString()
                  }).ToList();

            ViewBag.DiscountMethod = _Util.Facade.LookupFacade.GetLookupByKey("DiscountMethod").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString()
               }).ToList();

            ViewBag.Term = _Util.Facade.LookupFacade.GetLookupByKey("EstimateTerms").Select(x =>
              new SelectListItem()
              {
                  Text = x.DisplayText.ToString(),
                  Value = x.DataValue.ToString()
              }).ToList();
            ViewBag.EstimatePaymentTerms = _Util.Facade.LookupFacade.GetLookupByKey("EstimatePaymentTerms").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString()
               }).ToList();
            #endregion

            //model.InvoiceSetting = new InvoiceSetting();

            //var ShippingSetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceShippingSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            //if (ShippingSetting != null)
            //{
            //    model.InvoiceSetting.ShippingSetting = ShippingSetting.IsActive.Value;
            //}
            //var Discountsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceDiscountSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            //if (Discountsetting != null)
            //{
            //    model.InvoiceSetting.DiscountSetting = Discountsetting.IsActive.Value;
            //}
            //var DepositSetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceDepositSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            //if (DepositSetting != null)
            //{
            //    model.InvoiceSetting.DepositSetting = DepositSetting.IsActive.Value;
            //}

            //ViewBag.BookingMessage = _Util.Facade.GlobalSettingsFacade.GetEstimateByCompanyId(currentLoggedIn.CompanyId.Value);




            //Booking tempBooking = _Util.Facade.BookingFacade.GetByBookingId(model.Booking.BookingId);

            //model.EmailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("BookingPredefineEmailTemplate");

            //string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(model.Booking.CustomerId
            //                    + "#"
            //                    + currentLoggedIn.CompanyId.Value
            //                    + "#"
            //                    + model.Booking.Id);
            //string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Booking/", encryptedurl);

            //var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
            //var objcom = _Util.Facade.CompanyFacade.GetAllCompanyByCompanyId(currentLoggedIn.CompanyId.Value).FirstOrDefault();
            //ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, model.Booking.CustomerId);
            //ViewBag.url = AppConfig.SiteDomain + "/shrt/" + ShortUrl.Code;
            //if (objemp != null)
            //{
            //    ViewBag.SalesGuy = objemp.FirstName + " " + objemp.LastName;
            //    if (!string.IsNullOrWhiteSpace(objemp.Phone))
            //    {
            //        ViewBag.SalesPhone = objemp.Phone;
            //    }
            //    else
            //    {
            //        ViewBag.SalesPhone = objcom.Phone;
            //    }
            //}
           

            //ViewBag.CustomerName = tempCustomer.FirstName + " " + tempCustomer.LastName;
            //ViewBag.CompanyName = tempCustomer.CompanyName;






            return PartialView("AddLeadBooking", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddLeadBooking_v2(CreateBooking Model, bool SendEmail, bool CreatePdf)
        {
            bool EmailSent = false;
            string PdfLocation = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Booking tempBooking = _Util.Facade.BookingFacade.GetByBookingId(Model.Booking.BookingId);
             
            if (tempBooking == null || tempBooking.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false });
            }
            //if (Model.BookingDetailsList == null)
            //{
            //    return Json(new { result = false, message = "Customer Booking Found" });
            //}
            Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Booking.CustomerId);
            if (tempCUstomer == null)
            {
                return Json(new { result = false, message = "Customer Not Found" });
            }  
            Model.Booking.Id = tempBooking.Id;
            Model.Booking.CustomerName = tempCUstomer.FirstName + " " + tempCUstomer.LastName;
            Model.CustomerName = Model.Booking.CustomerName;
            Model.Booking.CreatedBy = tempBooking.CreatedBy;
            Model.Booking.CreatedDate = tempBooking.CreatedDate;
            Model.Booking.CompanyId = tempBooking.CompanyId;
            Model.Booking.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            Model.Booking.LastUpdatedBy = CurrentUser.UserId;

            Model.Booking.Message = Model.Booking.BookingMessage;
            if (string.IsNullOrWhiteSpace(Model.Booking.Status))
                Model.Booking.Status = LabelHelper.BookingStatus.Created;
            _Util.Facade.BookingFacade.UpdateBooking(Model.Booking);

            #region booking Extra Item 
            _Util.Facade.BookingFacade.DeleteAllBookingExtraItemByBookingId(Model.Booking.Id);            
            if(Model.BookingExtraItem != null && Model.BookingExtraItem.Count > 0)
            {
                foreach(var item in Model.BookingExtraItem)
                {

                    item.CreatedBy = CurrentUser.GetFullName();
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    item.BookingId = Model.Booking.Id; 
                    _Util.Facade.BookingFacade.InsertBookingExtraItem(item);
                }
            }

            #endregion


            #region CustomerSnapshot
            var objBookingSnapshot = _Util.Facade.CustomerSnapshotFacade.GetCustomerSnapshotDetail(Model.Booking.BookingId.ToString());
            if (objBookingSnapshot.Count == 0)
            {
                var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                if (empobj != null)
                {
                    var updatedate = Model.Booking.LastUpdatedDate.UTCToClientTime();
                    CustomerSnapshot BookingLogObj = new CustomerSnapshot()
                    {
                        CustomerId = tempCUstomer.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        Description = "Booking "
                        + string.Format
                        ("<a onclick=OpenTopToBottomModal('/Booking/AddLeadBooking?id={0}&CustomerId={1}') style='cursor: pointer;'>",
                        Model.Booking.Id, tempCUstomer.Id, tempCUstomer.CustomerId)
                        + "<b>" + Model.Booking.BookingId + "</b>" + "</a>",

                        Logdate = DateTime.Now.UTCCurrentTime(),
                        Updatedby = empobj.FirstName + " " + empobj.LastName,
                        Type = "BookingCreateHistory"
                    };
                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(BookingLogObj);
                }
            }
            #endregion

            #region Booking details
            Model.SubTotal = 0;
            _Util.Facade.BookingFacade.DeleteAllBookingDetailsByBookingId(Model.Booking.BookingId);
           if(Model.BookingDetailsList != null)
            {
                foreach (var item in Model.BookingDetailsList)
                {
                    item.AddedBy = CurrentUser.UserId;
                    item.AddedDate = DateTime.Now.UTCCurrentTime();
                    item.CompanyId = CurrentUser.CompanyId.Value;
                    _Util.Facade.BookingFacade.InsertBookingDetails(item);
                }
                foreach (var item in Model.BookingDetailsList)
                {
                    item.AddedBy = CurrentUser.UserId;
                    item.AddedDate = DateTime.Now.UTCCurrentTime();
                    item.CompanyId = CurrentUser.CompanyId.Value;
                    Model.SubTotal = Model.SubTotal + (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
                }
            }
         
            #endregion

           
            

            //Send and SMS region 
            if (CreatePdf)
            {
                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Booking.CompanyId);
                Model.CompanyAddress = tempCom.Address;
                Model.CompanyStreet = tempCom.Street;
                string ComCity = "";
                string ComState = "";
                if (!string.IsNullOrWhiteSpace(tempCom.City))
                {
                    ComCity = tempCom.City.UppercaseFirst() + ", ";
                }
                if (!string.IsNullOrWhiteSpace(tempCom.State))
                {
                    ComState = tempCom.State + " ";
                }
                Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
                Model.CompanyEmail = tempCom.EmailAdress;
                Model.CompanyName = tempCom.CompanyName;
                Model.PhoneNum = tempCom.Phone;
                Model.CompanyWebsite = tempCom.Website;
                Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
                Model.CompanyCity = tempCom.City;
                Model.CompanyState = tempCom.State;
                Model.CompanyZip = tempCom.ZipCode;
                Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
                Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
                Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
                Model.CustomerState = tempCUstomer.State;
                Model.CustomerZipCode = tempCUstomer.ZipCode;
                Model.CustomerNo = tempCUstomer.CustomerNo;
                Model.CustomerStreet = tempCUstomer.Street;
                
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("BookingPdf", Model)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);

                #region File Save
                string filename = ConfigurationManager.AppSettings["File.BookingFiles"];
                string comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
                filename = string.Format(filename, comname);
                filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + Model.Booking.BookingId + ".pdf";
                string Serverfilename = FileHelper.GetFileFullPath(filename);
                //Session[SessionKeys. BookingPdfSession] = filename;
                FileHelper.SaveFile(applicationPDFData, Serverfilename);
                #endregion



                #region Save CustomerFile
                _Util.Facade.BookingFacade.SaveBookingPdfFile(filename, Model.Booking.BookingId, tempCUstomer.CustomerId, CurrentUser.CompanyId.Value);
                #endregion
                return Json(new { result = true, emailSent = EmailSent, message = "Booking Successfully Saved" });
            }
            else if (SendEmail)
            {
                var tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Booking.CompanyId);
                PdfLocation = SendBookingEmail(Model, tempCUstomer, tempCom).FileLocation;
            }

            return Json(new { result = true, message = string.Concat("Booking Saved Successfully and Email to ", Model.EmailAddress), filePath = PdfLocation, EmailSent = EmailSent });
        }

        public JsonResult AddLeadBooking(CreateBooking Model, bool SendEmail, bool CreatePdf)
        {
            bool EmailSent = false;
            string PdfLocation = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Booking tempBooking = _Util.Facade.BookingFacade.GetByBookingId(Model.Booking.BookingId);

            if (tempBooking == null || tempBooking.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false });
            }
            //if (Model.BookingDetailsList == null)
            //{
            //    return Json(new { result = false, message = "Customer Booking Found" });
            //}
            Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Booking.CustomerId);
            if (tempCUstomer == null)
            {
                return Json(new { result = false, message = "Customer Not Found" });
            }
            Model.Booking.Id = tempBooking.Id;
            Model.Booking.CustomerName = tempCUstomer.FirstName + " " + tempCUstomer.LastName;
            Model.CustomerName = Model.Booking.CustomerName;
            Model.Booking.CreatedBy = tempBooking.CreatedBy;
            Model.Booking.CreatedDate = tempBooking.CreatedDate;
            Model.Booking.CompanyId = tempBooking.CompanyId;
            Model.Booking.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            Model.Booking.LastUpdatedBy = CurrentUser.UserId;

            Model.Booking.Message = Model.Booking.BookingMessage;
            if (string.IsNullOrWhiteSpace(Model.Booking.Status))
                Model.Booking.Status = LabelHelper.BookingStatus.Created;
            _Util.Facade.BookingFacade.UpdateBooking(Model.Booking);

            #region booking Extra Item 
            _Util.Facade.BookingFacade.DeleteAllBookingExtraItemByBookingId(Model.Booking.Id);
            if (Model.BookingExtraItem != null && Model.BookingExtraItem.Count > 0)
            {
                foreach (var item in Model.BookingExtraItem)
                {

                    item.CreatedBy = CurrentUser.GetFullName();
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    item.BookingId = Model.Booking.Id;
                    _Util.Facade.BookingFacade.InsertBookingExtraItem(item);
                }
            }

            #endregion


            #region CustomerSnapshot
            var objBookingSnapshot = _Util.Facade.CustomerSnapshotFacade.GetCustomerSnapshotDetail(Model.Booking.BookingId.ToString());
            if (objBookingSnapshot.Count == 0)
            {
                var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                if (empobj != null)
                {
                    var updatedate = Model.Booking.LastUpdatedDate.UTCToClientTime();
                    CustomerSnapshot BookingLogObj = new CustomerSnapshot()
                    {
                        CustomerId = tempCUstomer.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        Description = "Booking "
                        + string.Format
                        ("<a onclick=OpenTopToBottomModal('/Booking/AddLeadBooking?id={0}&CustomerId={1}') style='cursor: pointer;'>",
                        Model.Booking.Id, tempCUstomer.Id, tempCUstomer.CustomerId)
                        + "<b>" + Model.Booking.BookingId + "</b>" + "</a>",

                        Logdate = DateTime.Now.UTCCurrentTime(),
                        Updatedby = empobj.FirstName + " " + empobj.LastName,
                        Type = "BookingCreateHistory"
                    };
                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(BookingLogObj);
                }
            }
            #endregion

            #region Booking details
            Model.SubTotal = 0;
            _Util.Facade.BookingFacade.DeleteAllBookingDetailsByBookingId(Model.Booking.BookingId);
            if (Model.BookingDetailsList != null)
            {
                foreach (var item in Model.BookingDetailsList)
                {
                    item.AddedBy = CurrentUser.UserId;
                    item.AddedDate = DateTime.Now.UTCCurrentTime();
                    item.CompanyId = CurrentUser.CompanyId.Value;
                    _Util.Facade.BookingFacade.InsertBookingDetails(item);
                }
                foreach (var item in Model.BookingDetailsList)
                {
                    item.AddedBy = CurrentUser.UserId;
                    item.AddedDate = DateTime.Now.UTCCurrentTime();
                    item.CompanyId = CurrentUser.CompanyId.Value;
                    Model.SubTotal = Model.SubTotal + (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
                }
            }

            #endregion




            //Send and SMS region 
            if (CreatePdf)
            {
                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Booking.CompanyId);
                Model.CompanyAddress = tempCom.Address;
                Model.CompanyStreet = tempCom.Street;
                string ComCity = "";
                string ComState = "";
                if (!string.IsNullOrWhiteSpace(tempCom.City))
                {
                    ComCity = tempCom.City.UppercaseFirst() + ", ";
                }
                if (!string.IsNullOrWhiteSpace(tempCom.State))
                {
                    ComState = tempCom.State + " ";
                }
                Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
                Model.CompanyEmail = tempCom.EmailAdress;
                Model.CompanyName = tempCom.CompanyName;
                Model.PhoneNum = tempCom.Phone;
                Model.CompanyWebsite = tempCom.Website;
                Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
                Model.CompanyCity = tempCom.City;
                Model.CompanyState = tempCom.State;
                Model.CompanyZip = tempCom.ZipCode;
                Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
                Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
                Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
                Model.CustomerState = tempCUstomer.State;
                Model.CustomerZipCode = tempCUstomer.ZipCode;
                Model.CustomerNo = tempCUstomer.CustomerNo;
                Model.CustomerStreet = tempCUstomer.Street;

                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("BookingPdf", Model)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);

                #region File Save
                //string filename = ConfigurationManager.AppSettings["File.BookingFiles"];
                //string comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
                //filename = string.Format(filename, comname);
                //filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + Model.Booking.BookingId + ".pdf";
                //string Serverfilename = FileHelper.GetFileFullPath(filename);
                ////Session[SessionKeys. BookingPdfSession] = filename;
                //FileHelper.SaveFile(applicationPDFData, Serverfilename);
                #endregion

                //// "mayur" AWS S3 Changes //// Start

                #region File Save on AWS S3

                Random rand = new Random();
                string filename = ConfigurationManager.AppSettings["File.BookingFiles"];
                filename = filename.TrimEnd('/');

                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();

                string FilePath = string.Format(filename, comname);
                FilePath += "/" + DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString();

                string FileName = Model.Booking.BookingId + ".pdf";

                string FileKey = string.Format($"{FilePath}/{FileName}");

                var returnurl = "";

                var task = Task.Run(async () => {
                    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                    await AWSobject.UploadFile(FileKey, applicationPDFData);
                    await AWSobject.MakePublic(FileName, FilePath);
                });

                task.Wait();

                /// "mayur" used thread for async s3 methods : start

                //Thread thread = new Thread(async () => {

                //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                //    await AWSobject.UploadFile(FileKey, applicationPDFData);
                //    await AWSobject.MakePublic(FileName, FilePath);

                //});
                //thread.Start();

                /// "mayur" used thread for async s3 methods : End



                returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
                returnurl = returnurl + FileKey;


                ViewBag.ReturnUrl = returnurl;
                ViewBag.FileName = FileName;
                ViewBag.Filekey = FileKey;

                #endregion

                //// "mayur" AWS S3 Changes //// End



                #region Save CustomerFile
                _Util.Facade.BookingFacade.SaveBookingPdfFile(ViewBag.Filekey, Model.Booking.BookingId, tempCUstomer.CustomerId, CurrentUser.CompanyId.Value);
                #endregion


                return Json(new { result = true, emailSent = EmailSent, message = "Booking Successfully Saved" });
            }
            else if (SendEmail)
            {
                var tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Booking.CompanyId);
                PdfLocation = SendBookingEmail(Model, tempCUstomer, tempCom).FileLocation;
            }

            return Json(new { result = true, message = string.Concat("Booking Saved Successfully and Email to ", Model.EmailAddress), filePath = PdfLocation, EmailSent = EmailSent });
        }

        [Authorize]
        public ActionResult OpenEditBillingAddressModel(int? id, string Address, Guid CustomerId, string BookingId,string from)
        {

            CustomerAddress ModelCustomerAddress = new CustomerAddress();
            ModelCustomerAddress = _Util.Facade.CustomerFacade.GetCustomerAddressByCustomerIdRefIdAddressType(CustomerId, BookingId, Address);

            ViewBag.From = from;
            Booking model = new Booking();
            if (!string.IsNullOrWhiteSpace(Address))
            {
                ViewBag.Address = Address;
            }
            Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            
            if (ModelCustomerAddress != null && ModelCustomerAddress.Id > 0)
            {
                ViewBag.CustomerAddressId = ModelCustomerAddress.Id;
                customer.FirstName = ModelCustomerAddress.FirstName;
                customer.LastName = ModelCustomerAddress.LastName;
                customer.BusinessName = ModelCustomerAddress.BusinessName;
                customer.Street = ModelCustomerAddress.Street;
                customer.City = ModelCustomerAddress.City;
                customer.State = ModelCustomerAddress.State;
                customer.ZipCode = ModelCustomerAddress.ZipCode;
            }
            
            else if (string.IsNullOrWhiteSpace(customer.Address))
            {
                if (!string.IsNullOrWhiteSpace(customer.Address2))
                {
                    if (!string.IsNullOrWhiteSpace(customer.StreetPrevious))
                    {
                        customer.Street = customer.StreetPrevious;
                    }
                    if (!string.IsNullOrWhiteSpace(customer.CityPrevious))
                    {
                        customer.City = customer.CityPrevious;
                    }
                    if (!string.IsNullOrWhiteSpace(customer.StatePrevious))
                    {
                        customer.State = customer.StatePrevious;
                    }
                    if (!string.IsNullOrWhiteSpace(customer.ZipCodePrevious))
                    {
                        customer.ZipCode = customer.ZipCodePrevious;
                    }
                }
            }

            return View("OpenEditBillingAddressModel", customer);
        }



        //Method For Get Lead List By Key 
        [Authorize]
        public JsonResult GetLeadListByKey(string key)
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetCustomerSearchMaxLoad(CurrentUser.CompanyId.Value);

                List<CustomerSearchModel> CusList = _Util.Facade.CustomerFacade.GetLeadListBySearchKeyAndCompanyId(key, CurrentUser.CompanyId.Value, ItemsLoadCount);
                if (CusList.Count > 0)
                    result = JsonConvert.SerializeObject(CusList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }

        //Get Customer Address By Customer ID
        [HttpPost]
        public JsonResult GetCustomerAddressByCustomerId(Guid? CustomerId)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string BillingAddressVal = "";
            string ShippingAddressVal = "";
            if (CustomerId.HasValue)
            {
                var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId.Value);
                if (tempCustomer != null)
                {
                    BillingAddressVal = AddressHelper.MakeCustomerAddress(tempCustomer, AddressHelper.BillingAddress, AddressTemplate);
                    ShippingAddressVal = AddressHelper.MakeCustomerAddress(tempCustomer, AddressHelper.ShippingAddress, AddressTemplate);
                    result = true;
                }
            }
            return Json(new { result = result, BillingAddressVal = BillingAddressVal, ShippingAddressVal = ShippingAddressVal });
        }


        [Authorize]
        [HttpPost]
        public JsonResult SaveBookingPdf_V2(CreateBooking Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Booking tempBook = _Util.Facade.BookingFacade.GetByBookingId(Model.Booking.BookingId);
            if (tempBook == null || tempBook.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false });
            }
            //if (Model.BookingDetailsList == null)
            //{
            //    return Json(new { result = false, message = "Customer Equipment Found" });
            //}
            Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Booking.CustomerId);
            if (tempCUstomer == null)
            {
                return Json(new { result = false, message = "Customer Not Found" });
            }

            Model.Booking.Id = tempBook.Id;
            Model.Booking.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;
            if (tempCUstomer.BusinessName != "")
            {
                Model.CustomerName = tempCUstomer.BusinessName;
            }
            else
            {
                Model.CustomerName = Model.Booking.CustomerName;
            }
            //Model.CustomerName = Model.Booking.CustomerName;
            Model.Booking.CreatedBy = tempBook.CreatedBy;
            //Model.Booking.CreatedDate = tempBook.CreatedDate;
            Model.Booking.CustomerId = tempBook.CustomerId;
            Model.Booking.CompanyId = tempBook.CompanyId;
            Model.Booking.Status = "";
            Model.Booking.CreatedDate = tempBook.CreatedDate;

            Model.CusBussinessName = tempCUstomer.BusinessName;

            Model.SubTotal = 0;
            if(Model.BookingDetailsList != null)
            {
                foreach (var item in Model.BookingDetailsList)
                {
                    item.AddedBy = CurrentUser.UserId;
                    item.AddedDate = DateTime.Now.UTCCurrentTime();
                    item.CompanyId = CurrentUser.CompanyId.Value;
                    Model.SubTotal = Model.SubTotal + (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
                }
            }
         

            //if (!string.IsNullOrWhiteSpace(Model.Booking.DiscountType))
            //{
            //    if (Model.Booking.DiscountType == "amount")
            //    {
            //        if (Model.Booking.Discountpercent != null)
            //        {
            //            Model.Discount = Model.Invoice.Discountpercent.Value;
            //        }
            //    }
            //    else
            //    {
            //        if (Model.Invoice.Discountpercent != null)
            //        {
            //            Model.Discount = ((Model.Invoice.Discountpercent / 100) * Model.SubTotal).Value;
            //        }
            //    }
            //}

            Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Booking.CompanyId);
            Model.CompanyAddress = tempCom.Address;
            Model.CompanyStreet = tempCom.Street;
            string ComCity = "";
            string ComState = "";
            if (!string.IsNullOrWhiteSpace(tempCom.City))
            {
                ComCity = tempCom.City.UppercaseFirst() + ", ";
            }
            if (!string.IsNullOrWhiteSpace(tempCom.State))
            {
                ComState = tempCom.State + " ";
            }
            Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
            Model.CompanyEmail = tempCom.EmailAdress;
            Model.CompanyName = tempCom.CompanyName;
            Model.PhoneNum = tempCom.Phone;
            Model.CompanyWebsite = tempCom.Website;
            Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
            Model.CompanyCity = tempCom.City;
            Model.CompanyState = tempCom.State;
            Model.CompanyZip = tempCom.ZipCode;
            Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
            Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
            Model.CustomerState = tempCUstomer.State;
            Model.CustomerZipCode = tempCUstomer.ZipCode;
            Model.CustomerNo = tempCUstomer.CustomerNo;
            Model.CustomerStreet = tempCUstomer.Street;
             
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("BookingPdf", Model)
            {
                //FileName = "TestView.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            //
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.BookingFiles"];
            string comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            filename = string.Format(filename, comname);
            filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + Model.Booking.BookingId + ".pdf";
            string Serverfilename = FileHelper.GetFileFullPath(filename);
            FileHelper.SaveFile(applicationPDFData, Serverfilename);
            //Session[SessionKeys. BookingPdfSession] = filename;

            return Json(new { result = true, message = "Booking Successfully Saved", filePath = filename });

        }

        public JsonResult SaveBookingPdf(CreateBooking Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Booking tempBook = _Util.Facade.BookingFacade.GetByBookingId(Model.Booking.BookingId);
            if (tempBook == null || tempBook.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false });
            }
            //if (Model.BookingDetailsList == null)
            //{
            //    return Json(new { result = false, message = "Customer Equipment Found" });
            //}
            Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Booking.CustomerId);
            if (tempCUstomer == null)
            {
                return Json(new { result = false, message = "Customer Not Found" });
            }

            Model.Booking.Id = tempBook.Id;
            Model.Booking.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;
            if (tempCUstomer.BusinessName != "")
            {
                Model.CustomerName = tempCUstomer.BusinessName;
            }
            else
            {
                Model.CustomerName = Model.Booking.CustomerName;
            }
            //Model.CustomerName = Model.Booking.CustomerName;
            Model.Booking.CreatedBy = tempBook.CreatedBy;
            //Model.Booking.CreatedDate = tempBook.CreatedDate;
            Model.Booking.CustomerId = tempBook.CustomerId;
            Model.Booking.CompanyId = tempBook.CompanyId;
            Model.Booking.Status = "";
            Model.Booking.CreatedDate = tempBook.CreatedDate;

            Model.CusBussinessName = tempCUstomer.BusinessName;

            Model.SubTotal = 0;
            if (Model.BookingDetailsList != null)
            {
                foreach (var item in Model.BookingDetailsList)
                {
                    item.AddedBy = CurrentUser.UserId;
                    item.AddedDate = DateTime.Now.UTCCurrentTime();
                    item.CompanyId = CurrentUser.CompanyId.Value;
                    Model.SubTotal = Model.SubTotal + (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
                }
            }


            //if (!string.IsNullOrWhiteSpace(Model.Booking.DiscountType))
            //{
            //    if (Model.Booking.DiscountType == "amount")
            //    {
            //        if (Model.Booking.Discountpercent != null)
            //        {
            //            Model.Discount = Model.Invoice.Discountpercent.Value;
            //        }
            //    }
            //    else
            //    {
            //        if (Model.Invoice.Discountpercent != null)
            //        {
            //            Model.Discount = ((Model.Invoice.Discountpercent / 100) * Model.SubTotal).Value;
            //        }
            //    }
            //}

            Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Booking.CompanyId);
            Model.CompanyAddress = tempCom.Address;
            Model.CompanyStreet = tempCom.Street;
            string ComCity = "";
            string ComState = "";
            if (!string.IsNullOrWhiteSpace(tempCom.City))
            {
                ComCity = tempCom.City.UppercaseFirst() + ", ";
            }
            if (!string.IsNullOrWhiteSpace(tempCom.State))
            {
                ComState = tempCom.State + " ";
            }
            Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
            Model.CompanyEmail = tempCom.EmailAdress;
            Model.CompanyName = tempCom.CompanyName;
            Model.PhoneNum = tempCom.Phone;
            Model.CompanyWebsite = tempCom.Website;
            Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
            Model.CompanyCity = tempCom.City;
            Model.CompanyState = tempCom.State;
            Model.CompanyZip = tempCom.ZipCode;
            Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
            Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
            Model.CustomerState = tempCUstomer.State;
            Model.CustomerZipCode = tempCUstomer.ZipCode;
            Model.CustomerNo = tempCUstomer.CustomerNo;
            Model.CustomerStreet = tempCUstomer.Street;

            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("BookingPdf", Model)
            {
                //FileName = "TestView.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            //
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);

            #region File Save
            //Random rand = new Random();
            //string filename = ConfigurationManager.AppSettings["File.BookingFiles"];
            //string comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            //filename = string.Format(filename, comname);
            //filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + Model.Booking.BookingId + ".pdf";
            //string Serverfilename = FileHelper.GetFileFullPath(filename);
            //FileHelper.SaveFile(applicationPDFData, Serverfilename);
            ////Session[SessionKeys. BookingPdfSession] = filename;
            #endregion


            //// "mayur" AWS S3 Changes //// Start

            #region File Save on AWS S3

            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.S3.LeadToBooking"];

            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();

            string FilePath = string.Format(filename, comname);
            FilePath += "/" + DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString();
            string FileName =  Model.Booking.BookingId + ".pdf";

            string FileKey = string.Format($"{FilePath}/{FileName}");

            var returnurl = "";

            var task = Task.Run(async () => {
                AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                await AWSobject.UploadFile(FileKey, applicationPDFData);
                await AWSobject.MakePublic(FileName, FilePath);
            });

            task.Wait();

            /// "mayur" used thread for async s3 methods : start

            //Thread thread = new Thread(async () => {

            //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

            //    await AWSobject.UploadFile(FileKey, applicationPDFData);
            //    await AWSobject.MakePublic(FileName, FilePath);

            //});
            //thread.Start();

            /// "mayur" used thread for async s3 methods : End



            returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
            returnurl = returnurl + FileKey;


            ViewBag.ReturnUrl = returnurl;
            ViewBag.FileName = FileName;
            ViewBag.FileKey = FileKey;

            #endregion

            //// "mayur" AWS S3 Changes //// End


            return Json(new { result = true, message = "Booking Successfully Saved", filePath = FileKey });

        }

        public ActionResult GetBooking(int Id)
        {
            return View();
        }

        //Send Email Booking (Generate Send Email Url Method )
        [Authorize]
        public PartialViewResult SendEmailBooking(int Id, string EmailAddress)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateCustomerBooking model = new CreateCustomerBooking();
            if (Id == 0)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            Guid companyid = CurrentUser.CompanyId.Value;
            model.Booking = _Util.Facade.BookingFacade.GetBookingById(Id);

            if (model.Booking.CompanyId != companyid)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (model.Booking == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            model.BookingId = model.Booking.BookingId;
            Customer objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.Booking.CustomerId);

            if (objcus != null)
            {
                model.CustomerName = objcus.FirstName + " " + objcus.LastName;
                model.CustomerEmailAddress = string.IsNullOrWhiteSpace(EmailAddress) ? objcus.EmailAddress : EmailAddress;
                model.CustomerContactNumber = string.IsNullOrWhiteSpace(objcus.PrimaryPhone) ? objcus.SecondaryPhone : objcus.PrimaryPhone;
                model.CustomerId = objcus.CustomerId;
            }
            else
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var objcom = _Util.Facade.CompanyFacade.GetAllCompanyByCompanyId(CurrentUser.CompanyId.Value).FirstOrDefault();
            if (objcom != null)
            {
                model.CompanyName = objcom.CompanyName;
                model.CompanyEmail = objcom.EmailAdress;
                model.CompanyPhone = objcom.Phone;
            }
            else
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            model.ShortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/");
            //if (Session[SessionKeys. BookingPdfSession] != null)
            //{
            //    ViewBag.pdfLocation = Session[SessionKeys. BookingPdfSession].ToString();
            //}

            model.EmailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("BookingPredefineEmailTemplate");

            model.SMSBody = string.Concat("New Booking from", " ", model.CompanyName, ": ", model.Booking.BookingId, Environment.NewLine
                , Environment.NewLine, model.ShortUrl, "##url##");
            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            if (objemp != null)
            {
                ViewBag.SalesGuy = objemp.FirstName + " " + objemp.LastName;
                if (!string.IsNullOrWhiteSpace(objemp.Phone))
                {
                    ViewBag.SalesPhone = objemp.Phone;
                }
                else
                {
                    ViewBag.SalesPhone = objcom.Phone;
                }
            }
            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(model.CustomerId
                                + "#"
                                + CurrentUser.CompanyId.Value
                                + "#"
                                + model.Booking.Id);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Booking/", encryptedurl);
            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, model.CustomerId);
            ViewBag.url = AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code;

            return PartialView("SendEmailBooking", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult ResendBookingEmail(int BookingId, string EmailAddress)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateBooking Model = new CreateBooking();
            Customer tempCUstomer;
            Company tempCom;
            
            Model.Booking = _Util.Facade.BookingFacade.GetBookingById(BookingId);

            if (Model.Booking == null || Model.Booking.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false, message = "Access denied." });
            }
            tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Booking.CustomerId);
            if (tempCUstomer == null)
            {
                return Json(new { result = false, message = "Access denied." });
            }
            tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Booking.CompanyId);
            if (tempCom == null)
            {
                return Json(new { result = false, message = "Access denied." });
            }
            Model.Booking.Message = Model.Booking.BookingMessage;
            Model.Booking.CustomerName =  tempCUstomer.FirstName + " " + tempCUstomer.LastName;
            Model.CustomerName = Model.Booking.CustomerName;
            Model.BookingDetailsList = _Util.Facade.BookingFacade.GetBookingDetialsListByBookingId(Model.Booking.BookingId);
            Model.EmailAddress = EmailAddress;
            //Model.EmailDescription = EmailDescription;
            

            #region Booking BOdy
            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            if (objemp != null)
            {
                ViewBag.SalesGuy = objemp.FirstName + " " + objemp.LastName;
                if (!string.IsNullOrWhiteSpace(objemp.Phone))
                {
                    ViewBag.SalesPhone = objemp.Phone;
                }
                else
                {
                    ViewBag.SalesPhone = tempCom.Phone;
                }
            }

            Model.EmailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("BookingPredefineEmailTemplate");

            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(Model.Booking.CustomerId
                                + "#"
                                + CurrentUser.CompanyId.Value
                                + "#"
                                + Model.Booking.Id);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Booking/", encryptedurl); 
            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl,  Model.Booking.CustomerId);
            ViewBag.url = AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code;


            Hashtable datatemplate = new Hashtable();
            datatemplate.Add("CustomerName", Model.Booking.CustomerName);
            datatemplate.Add("SalesPhone Number", ViewBag.SalesPhone);
            datatemplate.Add("CompanyName", tempCom.CompanyName);
            datatemplate.Add("SalesGuy", ViewBag.SalesGuy);
            datatemplate.Add("url", ViewBag.url);
            string emailtemplate = HS.Web.UI.Helper.LabelHelper.ParserHelper(Model.EmailTemplate.BodyContent, datatemplate);

            string EmaiSubject = "New Booking from " + tempCom.CompanyName + " : " + Model.Booking.BookingId;

            #endregion

            Model.EmailDescription = emailtemplate;
            Model.EmailSubject = EmaiSubject;

            var response = SendBookingEmail(Model, tempCUstomer, tempCom);

            if (response.IsSent)
            {
                return Json(new { result = true, message = string.Format("Email send successfully to {0}.", EmailAddress) });
            }
            else
            {
                return Json(new { result = false, message = "Sending email was unsuccessful." });
            }

        }

        //Email will send to Model.EmailAddress
        private BookingEmailSentResponse SendBookingEmail_v2(CreateBooking Model, Customer tempCUstomer, Company tempCom)
        {
            var response = new BookingEmailSentResponse
            {
                FileLocation = "",
                IsSent = false
            };
            tempCUstomer.PreferedEmail = true;
            _Util.Facade.CustomerFacade.UpdateCustomer(tempCUstomer);
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            response.IsSent = false;

            #region Booking PDF Ready
            string ComCity = "";
            string ComState = "";
            if (!string.IsNullOrWhiteSpace(tempCom.City))
            {
                ComCity = tempCom.City.UppercaseFirst() + ", ";
            }
            if (!string.IsNullOrWhiteSpace(tempCom.State))
            {
                ComState = tempCom.State + " ";
            }

            Model.CompanyAddress = MakeAddress(tempCom.Street, tempCom.City, tempCom.State, tempCom.ZipCode, "");
            Model.CompanyEmail = tempCom.EmailAdress;
            Model.CompanyName = tempCom.CompanyName;
            Model.CompanyWebsite = tempCom.Website;
            Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            Model.CompanyStreet = tempCom.Street;
            Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
            Model.PhoneNum = tempCom.Phone;
            Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
            Model.CompanyCity = tempCom.City;
            Model.CompanyState = tempCom.State;
            Model.CompanyZip = tempCom.ZipCode;

            Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
            Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
            Model.CustomerState = tempCUstomer.State;
            Model.CustomerZipCode = tempCUstomer.ZipCode;
            Model.CustomerNo = tempCUstomer.CustomerNo;
            Model.CustomerStreet = tempCUstomer.Street;
             

            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("BookingPdf", Model)
            {
                //FileName = "TestView.pdf",
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },
            };

            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);

            #region File Save
            string filename = ConfigurationManager.AppSettings["File.BookingFiles"];
            string comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            filename = string.Format(filename, comname);
            filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + Model.Booking.BookingId + ".pdf";
            string Serverfilename = FileHelper.GetFileFullPath(filename);
            //Session[SessionKeys. BookingPdfSession] = filename;
            FileHelper.SaveFile(applicationPDFData, Serverfilename);
            #endregion


            #region Save CustomerFile
            _Util.Facade.BookingFacade.SaveBookingPdfFile(filename, Model.Booking.BookingId, tempCUstomer.CustomerId, CurrentUser.CompanyId.Value);
            #endregion

            #endregion

            ////Get Email Content 
            //var mailContent = _Util.Facade.BookingFacade.GetEmailSubAndDescriptionByBookingId(Model.Booking.BookingId);

            try
            {
                List<string> EmailAddressesList = Model.EmailAddress.Split(',').ToList<string>();
                foreach (var item in EmailAddressesList)
                {
                    if (item.IsValidEmailAddress())
                    {
                        BookingCreatedEmail email = new BookingCreatedEmail()
                        {
                            CompanyName = tempCom.CompanyName,
                            CustomerName = Model.Booking.CustomerName,
                            BalanceAmount = Model.Booking.TotalAmount != null ? "$" + Model.Booking.TotalAmount.Value.ToString("0,0.00") : "$0.00",
                            BookingId = Model.Booking.BookingId,
                            ToEmail = item.Trim(),
                            //EmailBody = Model.EmailDescription,
                            //Subject = Model.EmailSubject,
                            EmailBody = Model.EmailDescription,
                            Subject = Model.EmailSubject,
                            CustomerId = Model.Booking.CustomerId.ToString(),
                            EmployeeId = CurrentUser.UserId.ToString(),
                            FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : "info@rmrcloud.com",
                            FromName = CurrentUser.GetFullName(),
                            CCEmail = Model.CCMail
                            //BookingPdf = new Attachment(
                            //  FileHelper.GetFileFullPath(filename),
                            // MediaTypeNames.Application.Octet)
                        };
                        #region Comment
                        //if (email.EmailBody.IndexOf("##url##") > -1)
                        //{
                        //    string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(tempCUstomer.CustomerId
                        //        + "#"
                        //        + CurrentUser.CompanyId.Value
                        //        + "#"
                        //        + Model.Invoice.Id);
                        //    string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Estimate/", encryptedurl);
                        //    ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, tempCUstomer.CustomerId);
                        //    email.EmailBody = email.EmailBody.Replace("##url##", ShortUrl.Code);
                        //}
                        #endregion
                        response.IsSent = _Util.Facade.MailFacade.SendBookingCreatedEmail(email, CurrentUser.CompanyId.Value);
                        //email.BookingPdf.Dispose();

                        if (response.IsSent)
                        {
                            CustomerSnapshot BookingEmailLogObj = new CustomerSnapshot()
                            {
                                CustomerId = Model.Booking.CustomerId,
                                CompanyId = CurrentUser.CompanyId.Value,
                                Description = "Booking:" + "  " + Model.Booking.BookingId + " " + "email sent by ",
                                Logdate = DateTime.Now.UTCCurrentTime(),
                                Updatedby = CurrentUser.Identity.Name,
                                Type = "CustomerMailHistory"
                            };
                            _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(BookingEmailLogObj);

                        }
                        //EmailSent = _Util.Facade.MailFacade.SendBookingCreatedEmail(email, CurrentUser.CompanyId.Value);
                        //email.BookingPdf.Dispose();
                    }

                }
                if (response.IsSent)
                {

                    CustomerAgreement objagree = new CustomerAgreement()
                    {
                        CustomerId = Model.Booking.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        InvoiceId = Model.Booking.BookingId,
                        Type = LabelHelper.EstimateStatus.SentToCustomer,
                        AddedDate = DateTime.Now.UTCCurrentTime()
                    };
                    _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objagree);

                    if (Model.Booking.Status == LabelHelper.BookingStatus.SentToCustomer)
                    {
                        Model.Booking.Status = LabelHelper.BookingStatus.ResendToCustomer;
                    }
                    else
                    {
                        Model.Booking.Status = LabelHelper.BookingStatus.SentToCustomer;
                    }
                    _Util.Facade.BookingFacade.UpdateBooking(Model.Booking);
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        private BookingEmailSentResponse SendBookingEmail(CreateBooking Model, Customer tempCUstomer, Company tempCom)
        {
            var response = new BookingEmailSentResponse
            {
                FileLocation = "",
                IsSent = false
            };
            tempCUstomer.PreferedEmail = true;
            _Util.Facade.CustomerFacade.UpdateCustomer(tempCUstomer);
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            response.IsSent = false;

            #region Booking PDF Ready
            string ComCity = "";
            string ComState = "";
            if (!string.IsNullOrWhiteSpace(tempCom.City))
            {
                ComCity = tempCom.City.UppercaseFirst() + ", ";
            }
            if (!string.IsNullOrWhiteSpace(tempCom.State))
            {
                ComState = tempCom.State + " ";
            }

            Model.CompanyAddress = MakeAddress(tempCom.Street, tempCom.City, tempCom.State, tempCom.ZipCode, "");
            Model.CompanyEmail = tempCom.EmailAdress;
            Model.CompanyName = tempCom.CompanyName;
            Model.CompanyWebsite = tempCom.Website;
            Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            Model.CompanyStreet = tempCom.Street;
            Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
            Model.PhoneNum = tempCom.Phone;
            Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
            Model.CompanyCity = tempCom.City;
            Model.CompanyState = tempCom.State;
            Model.CompanyZip = tempCom.ZipCode;

            Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
            Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
            Model.CustomerState = tempCUstomer.State;
            Model.CustomerZipCode = tempCUstomer.ZipCode;
            Model.CustomerNo = tempCUstomer.CustomerNo;
            Model.CustomerStreet = tempCUstomer.Street;


            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("BookingPdf", Model)
            {
                //FileName = "TestView.pdf",
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },
            };

            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);

            #region File Save
            //string filename = ConfigurationManager.AppSettings["File.BookingFiles"];
            //string comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            //filename = string.Format(filename, comname);
            //filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + Model.Booking.BookingId + ".pdf";
            //string Serverfilename = FileHelper.GetFileFullPath(filename);
            ////Session[SessionKeys. BookingPdfSession] = filename;
            //FileHelper.SaveFile(applicationPDFData, Serverfilename);
            #endregion


            //// "mayur" AWS S3 Changes //// Start

            #region File Save on AWS S3

            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.BookingFiles"];
            filename = filename.TrimEnd('/');

            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();

            string FilePath = string.Format(filename, comname);
            FilePath += "/" + DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString();
            string FileName = Model.Booking.BookingId + ".pdf";

            string FileKey = string.Format($"{FilePath}/{FileName}");

            var returnurl = "";

            var task = Task.Run(async () => {
                AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                await AWSobject.UploadFile(FileKey, applicationPDFData);
                await AWSobject.MakePublic(FileName, FilePath);
            });

            task.Wait();

            /// "mayur" used thread for async s3 methods : start

            //Thread thread = new Thread(async () => {

            //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

            //    await AWSobject.UploadFile(FileKey, applicationPDFData);
            //    await AWSobject.MakePublic(FileName, FilePath);

            //});
            //thread.Start();

            /// "mayur" used thread for async s3 methods : End



            returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
            returnurl = returnurl + FileKey;

            ViewBag.ReturnUrl = returnurl;
            ViewBag.FileName = FileName;
            ViewBag.FileKey = FileKey;

            #endregion

            //// "mayur" AWS S3 Changes //// End

            #region Save CustomerFile
            _Util.Facade.BookingFacade.SaveBookingPdfFile(FileKey, Model.Booking.BookingId, tempCUstomer.CustomerId, CurrentUser.CompanyId.Value);
            #endregion

            #endregion

            ////Get Email Content 
            //var mailContent = _Util.Facade.BookingFacade.GetEmailSubAndDescriptionByBookingId(Model.Booking.BookingId);

            try
            {
                List<string> EmailAddressesList = Model.EmailAddress.Split(',').ToList<string>();
                foreach (var item in EmailAddressesList)
                {
                    if (item.IsValidEmailAddress())
                    {
                        BookingCreatedEmail email = new BookingCreatedEmail()
                        {
                            CompanyName = tempCom.CompanyName,
                            CustomerName = Model.Booking.CustomerName,
                            BalanceAmount = Model.Booking.TotalAmount != null ? "$" + Model.Booking.TotalAmount.Value.ToString("0,0.00") : "$0.00",
                            BookingId = Model.Booking.BookingId,
                            ToEmail = item.Trim(),
                            //EmailBody = Model.EmailDescription,
                            //Subject = Model.EmailSubject,
                            EmailBody = Model.EmailDescription,
                            Subject = Model.EmailSubject,
                            CustomerId = Model.Booking.CustomerId.ToString(),
                            EmployeeId = CurrentUser.UserId.ToString(),
                            FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : "info@rmrcloud.com",
                            FromName = CurrentUser.GetFullName(),
                            CCEmail = Model.CCMail
                            //BookingPdf = new Attachment(
                            //  FileHelper.GetFileFullPath(filename),
                            // MediaTypeNames.Application.Octet)
                        };
                        #region Comment
                        //if (email.EmailBody.IndexOf("##url##") > -1)
                        //{
                        //    string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(tempCUstomer.CustomerId
                        //        + "#"
                        //        + CurrentUser.CompanyId.Value
                        //        + "#"
                        //        + Model.Invoice.Id);
                        //    string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Estimate/", encryptedurl);
                        //    ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, tempCUstomer.CustomerId);
                        //    email.EmailBody = email.EmailBody.Replace("##url##", ShortUrl.Code);
                        //}
                        #endregion
                        response.IsSent = _Util.Facade.MailFacade.SendBookingCreatedEmail(email, CurrentUser.CompanyId.Value);
                        //email.BookingPdf.Dispose();

                        if (response.IsSent)
                        {
                            CustomerSnapshot BookingEmailLogObj = new CustomerSnapshot()
                            {
                                CustomerId = Model.Booking.CustomerId,
                                CompanyId = CurrentUser.CompanyId.Value,
                                Description = "Booking:" + "  " + Model.Booking.BookingId + " " + "email sent by ",
                                Logdate = DateTime.Now.UTCCurrentTime(),
                                Updatedby = CurrentUser.Identity.Name,
                                Type = "CustomerMailHistory"
                            };
                            _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(BookingEmailLogObj);

                        }
                        //EmailSent = _Util.Facade.MailFacade.SendBookingCreatedEmail(email, CurrentUser.CompanyId.Value);
                        //email.BookingPdf.Dispose();
                    }

                }
                if (response.IsSent)
                {

                    CustomerAgreement objagree = new CustomerAgreement()
                    {
                        CustomerId = Model.Booking.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        InvoiceId = Model.Booking.BookingId,
                        Type = LabelHelper.EstimateStatus.SentToCustomer,
                        AddedDate = DateTime.Now.UTCCurrentTime()
                    };
                    _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objagree);

                    if (Model.Booking.Status == LabelHelper.BookingStatus.SentToCustomer)
                    {
                        Model.Booking.Status = LabelHelper.BookingStatus.ResendToCustomer;
                    }
                    else
                    {
                        Model.Booking.Status = LabelHelper.BookingStatus.SentToCustomer;
                    }
                    _Util.Facade.BookingFacade.UpdateBooking(Model.Booking);
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }

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

        //Clone Booking Code 
        [HttpPost]
        [Authorize]
        public JsonResult CloneBooking(int BookingId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Booking bk = _Util.Facade.BookingFacade.GetBookingById(BookingId);

            #region validations
            if (bk == null)
            {
                return Json(new { result = false, message = "Booking not found." });
            }
            else if (bk.Status == LabelHelper.BookingStatus.Init)
            {
                return Json(new { result = false, message = "Booking not found." });
            }
            else if (bk.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false, message = "Access denied." });
            }
            #endregion

            bk.Id = 0;
            bk.Id = _Util.Facade.BookingFacade.InsertBooking(bk);
            List<BookingDetails> BkDetails = _Util.Facade.BookingFacade.GetBookingDetailsByBookingId(bk.BookingId);

            //Update Booking 
            #region Update New Booking
            bk.BookingId = bk.Id.GenerateBookingNo();
            bk.Status = LabelHelper.BookingStatus.Created;
            bk.CreatedBy = CurrentUser.UserId;
            bk.CreatedDate = DateTime.Now.UTCCurrentTime();

            _Util.Facade.BookingFacade.UpdateBooking(bk);
            #endregion

            #region Insert Booking Details
            foreach (var item in BkDetails)
            {
                item.BookingId = bk.BookingId;
                item.AddedDate = DateTime.Now.UTCCurrentTime();
                item.AddedBy = CurrentUser.UserId;
                item.Id = 0;
                _Util.Facade.BookingFacade.InsertBookingDetails(item);
            }
            #endregion

            return Json(new { result = true, message = string.Format("Booking Duplicated Successfully With A New Booking Id {0}.", bk.BookingId) });
        }


        #region ApproveBooking
        [HttpPost]
        [Authorize]
        public JsonResult ApproveBooking(int BookingId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Booking bk = _Util.Facade.BookingFacade.GetBookingById(BookingId);

            #region validations
            if (bk == null)
            {
                return Json(new { result = false, message = "Booking not found." });
            }
            else if (bk.Status == LabelHelper.BookingStatus.Init)
            {
                return Json(new { result = false, message = "Booking not found." });
            }
            else if (bk.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false, message = "Access denied." });
            }
            #endregion

            #region Update Booking
            bk.Status = LabelHelper.BookingStatus.Approved;
            bk.LastUpdatedBy = CurrentUser.UserId;
            bk.LastUpdatedDate = DateTime.Now.UTCCurrentTime();

            _Util.Facade.BookingFacade.UpdateBooking(bk);
            #endregion

            #region Update Lead to customer

            CustomerCompany cc = _Util.Facade.CustomerFacade.GetCustomerCompanyByCompanyIdAndCustomerId(bk.CompanyId, bk.CustomerId);
            Customer Cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(cc.CustomerId);
            if (cc == null || Cus==null )
            {
                return Json(new { result = false, message = "Customer not found." });
            }
            else if (cc.IsLead == true)
            {
                cc.IsLead = false;
                cc.ConvertionDate = DateTime.Now.UTCCurrentTime();
                cc.ConvertionType = LabelHelper.LeadConvertionType.System;
                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cc);

                Cus.JoinDate = DateTime.Now.UTCCurrentTime();
                _Util.Facade.CustomerFacade.UpdateCustomer(Cus);
            }
            #endregion

            _Util.Facade.TicketFacade.CreateTicketsForApprovingBooking(
                LabelHelper.TicketStatus.Created,
                bk,
                //Cus.CustomerId,
                Cus.Id,
                //CurrentUser.CompanyId.Value,
                CurrentUser.UserId,
                User.Identity.Name,
                //bk.BookingId,
                //bk.PickUpDate.HasValue ? bk.PickUpDate.Value : DateTime.Now.UTCCurrentTime().AddDays(1),
                //bk.PickUpLocation,
                //bk.DropOffLocation,
                LabelHelper.TicketType.PickUp,
                LabelHelper.TicketType.Service,
                LabelHelper.TicketType.DropOff,
                LabelHelper.UserTags.Admin,
                LabelHelper.UserTags.HRManager,
                LabelHelper.NotificationType.Employee
                );

            #region Create Tickets Previous
            /*
            DateTime NextWorkingDayPickUp = DateTime.Now.UTCCurrentTime().UTCToClientTime().AddDays(1);
            DateTime NextWorkingDayService = DateTime.Now.UTCCurrentTime().UTCToClientTime().AddDays(2);
            DateTime NextWorkingDayDropOff = DateTime.Now.UTCCurrentTime().UTCToClientTime().AddDays(6);
            //Sun=0,Mon=1,Tue=2,Wed=3,Thu=4,Fri=5,Sat=6
            if (NextWorkingDayPickUp.DayOfWeek == 0)
            {
                NextWorkingDayPickUp = NextWorkingDayPickUp.AddDays(1);
            }
            if (NextWorkingDayService.DayOfWeek == 0)
            {
                NextWorkingDayService = NextWorkingDayService.AddDays(1);
            }
            if (NextWorkingDayDropOff.DayOfWeek == 0)
            {
                NextWorkingDayDropOff = NextWorkingDayDropOff.AddDays(1);
            }

            #region Create Ticket 
            {
                #region Ticket For Pickup
                Ticket TicketForPickup = new Ticket()
                {
                    TicketId = Guid.NewGuid(),
                    CompanyId = CurrentUser.CompanyId.Value,
                    CustomerId = bk.CustomerId,
                    TicketType = LabelHelper.TicketType.PickUp,
                    Message = "Rug pickup.",
                    CreatedBy = CurrentUser.UserId,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CompletionDate = NextWorkingDayPickUp,
                    Status = LabelHelper.TicketStatus.Created,
                    LastUpdatedBy = CurrentUser.UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    HasInvoice = false,
                    HasSurvey = false,
                    IsClosed = false,
                    IsAgreementTicket = false,
                };
                _Util.Facade.TicketFacade.InsertTicket(TicketForPickup);
                TicketUser TUPickup = new TicketUser()
                {
                    TiketId = TicketForPickup.TicketId,
                    UserId = new Guid("22222222-2222-2222-2222-222222222222"),
                    IsPrimary = true,
                    AddedDate = DateTime.Now.UTCCurrentTime(),
                    AddedBy = CurrentUser.UserId,
                    NotificationOnly = false,
                };
                _Util.Facade.TicketFacade.InsertTicketUser(TUPickup);
                CustomerAppointment CAForPickup = new CustomerAppointment()
                {
                    AppointmentId = TicketForPickup.TicketId,
                    CompanyId = TicketForPickup.CompanyId,
                    CustomerId = TicketForPickup.CustomerId,
                    EmployeeId = new Guid("22222222-2222-2222-2222-222222222222"),
                    AppointmentType = TicketForPickup.TicketType,
                    AppointmentStartTime = "08:00",
                    AppointmentEndTime = "10:00",
                    IsAllDay = false,
                    Notes = TicketForPickup.Message,
                    Status = false,
                    AppointmentDate = NextWorkingDayPickUp.SetZeroHour(),
                    CreatedBy = User.Identity.Name,
                    LastUpdatedBy = User.Identity.Name,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime()
                };
                _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(CAForPickup);

                #endregion
            }
            {
                #region Ticket For Service
                Ticket TicketForService = new Ticket()
                {
                    TicketId = Guid.NewGuid(),
                    CompanyId = CurrentUser.CompanyId.Value,
                    CustomerId = bk.CustomerId,
                    TicketType = LabelHelper.TicketType.Service,
                    Message = "Rug Servicing.",
                    CreatedBy = CurrentUser.UserId,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CompletionDate = NextWorkingDayService,//bk.PickUpDate.HasValue ? bk.PickUpDate.Value : DateTime.Now.UTCCurrentTime(),
                    Status = LabelHelper.TicketStatus.Created,
                    LastUpdatedBy = CurrentUser.UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    HasInvoice = false,
                    HasSurvey = false,
                    IsClosed = false,
                    IsAgreementTicket = false,
                };
                _Util.Facade.TicketFacade.InsertTicket(TicketForService);
                TicketUser TUService = new TicketUser()
                {
                    TiketId = TicketForService.TicketId,
                    UserId = new Guid("22222222-2222-2222-2222-222222222222"),
                    IsPrimary = true,
                    AddedDate = DateTime.Now.UTCCurrentTime(),
                    AddedBy = CurrentUser.UserId,
                    NotificationOnly = false,
                };
                _Util.Facade.TicketFacade.InsertTicketUser(TUService);
                CustomerAppointment CAForService = new CustomerAppointment()
                {
                    AppointmentId = TicketForService.TicketId,
                    CompanyId = TicketForService.CompanyId,
                    CustomerId = TicketForService.CustomerId,
                    EmployeeId = new Guid("22222222-2222-2222-2222-222222222222"),
                    AppointmentType = TicketForService.TicketType,
                    AppointmentStartTime = "10:00",
                    AppointmentEndTime = "12:00",
                    IsAllDay = false,
                    Notes = TicketForService.Message,
                    Status = false,
                    AppointmentDate = NextWorkingDayService.SetZeroHour(),
                    CreatedBy = User.Identity.Name,
                    LastUpdatedBy = User.Identity.Name,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime()
                };
                _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(CAForService);
                #endregion
            }
            {
                #region Ticket For DropOff
                Ticket TicketForDropOff = new Ticket()
                {
                    TicketId = Guid.NewGuid(),
                    CompanyId = CurrentUser.CompanyId.Value,
                    CustomerId = bk.CustomerId,
                    TicketType = LabelHelper.TicketType.DropOff,
                    Message = "Rug Drop off.",
                    CreatedBy = CurrentUser.UserId,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CompletionDate = NextWorkingDayDropOff,//bk.PickUpDate.HasValue ? bk.PickUpDate.Value : DateTime.Now.UTCCurrentTime(),
                    Status = LabelHelper.TicketStatus.Created,
                    LastUpdatedBy = CurrentUser.UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    HasInvoice = false,
                    HasSurvey = false,
                    IsClosed = false,
                    IsAgreementTicket = false,
                };
                _Util.Facade.TicketFacade.InsertTicket(TicketForDropOff);
                TicketUser TUDropOff = new TicketUser()
                {
                    TiketId = TicketForDropOff.TicketId,
                    UserId = new Guid("22222222-2222-2222-2222-222222222222"),
                    IsPrimary = true,
                    AddedDate = DateTime.Now.UTCCurrentTime(),
                    AddedBy = CurrentUser.UserId,
                    NotificationOnly = false,
                };
                _Util.Facade.TicketFacade.InsertTicketUser(TUDropOff);
                CustomerAppointment CAForDropOff = new CustomerAppointment()
                {
                    AppointmentId = TicketForDropOff.TicketId,
                    CompanyId = TicketForDropOff.CompanyId,
                    CustomerId = TicketForDropOff.CustomerId,
                    EmployeeId = new Guid("22222222-2222-2222-2222-222222222222"),
                    AppointmentType = TicketForDropOff.TicketType,
                    AppointmentStartTime = "12:00",
                    AppointmentEndTime = "14:00",
                    IsAllDay = false,
                    Notes = TicketForDropOff.Message,
                    Status = false,
                    CreatedBy = User.Identity.Name,
                    AppointmentDate = NextWorkingDayDropOff.SetZeroHour(),
                    LastUpdatedBy = User.Identity.Name,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime()
                };
                _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(CAForDropOff);

                #endregion
            }
            
            #endregion*/
            #endregion



            return Json(new { result = true, CustomerId = Cus.Id, message ="Booking has been approved successfully." });
        }

        #endregion

        //Delete Booking By ID 
        [Authorize]
        [HttpPost]
        public JsonResult DeleteBooking(int Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Booking booked = _Util.Facade.BookingFacade.GetById(Id);

            if (booked == null || booked.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false, message = "Permission denied." });
            }

            #region Booking Transaction 
            //If Any Transaction Occured 
            #endregion

            _Util.Facade.BookingFacade.DeleteAllBookingDetailsByBookingId(booked.BookingId);
            _Util.Facade.BookingFacade.DeleteBookingById(Id);

            return Json(new { result = true, message = "Booking deleted successfully." });
        }

        //Find Paragraph Tag
        private string GetFirstParagraphFromEmailBodyContent(string file)
        {
            Match check = Regex.Match(file, @"<p>\s*(.+?)\s*</p>");
            if (check.Success)
            {
                return check.Groups[1].Value;
            }
            else
            {
                return null;
            }
        }



        [Authorize]
        [HttpPost]
        public JsonResult DeclineLeadBookingStatus(int Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Booking booked = _Util.Facade.BookingFacade.GetById(Id);

            if (booked == null || booked.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false, message = "Permission denied." });
            }

            #region Booking Transaction 
            //If Any Transaction Occured 
            #endregion
            
            Booking bking = _Util.Facade.BookingFacade.GetBookingById(Id);
            if(bking != null)
            {
                bking.Status = LabelHelper.BookingStatus.Cancelled;
                bking.LastUpdatedBy = CurrentUser.UserId;
                bking.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                _Util.Facade.BookingFacade.UpdateBooking(bking);

                List<Ticket> ticketlist = _Util.Facade.TicketFacade.GetAllTicketByBookingId(bking.BookingId);
                if(ticketlist != null && ticketlist.Count() > 0)
                {
                    foreach(var item in ticketlist)
                    {
                        item.Status = LabelHelper.TicketStatus.Lost;
                        item.LastUpdatedBy = CurrentUser.UserId;
                        item.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        _Util.Facade.TicketFacade.UpdateTicket(item);
                    }
                }
                List<Invoice> Invoicelist = _Util.Facade.InvoiceFacade.GetInvoicebyBookingId(bking.BookingId);
                if (Invoicelist != null && Invoicelist.Count() > 0)
                {
                    foreach (var item in Invoicelist)
                    {
                        string TempStatus = item.Status;

                        item.Status = LabelHelper.InvoiceStatus.Cancelled;
                        item.LastUpdatedByUid = CurrentUser.UserId;
                        item.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        _Util.Facade.InvoiceFacade.UpdateInvoice(item);
                        #region log
                        int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
                        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                        bool newBool = item.IsARBInvoice ?? false;

                        
                        base.AddUserActivityForCustomer("Invoice Status Changed from " + TempStatus + " To "+item.Status +"#InvoiceId: "+item.InvoiceId,lineNumber+","+ actionName +"/"+ controllerName, item.CustomerId, null, null, newBool);
                        #endregion
                        if (TempStatus == LabelHelper.InvoiceStatus.Paid || item.BalanceDue == 0 )
                        {
                            TransactionHistory transactionHistory = _Util.Facade.TransactionFacade.GetTransactionHistoryByInvoiceId(item.Id).FirstOrDefault();
                            if(transactionHistory != null)
                            {
                                Transaction transaction = _Util.Facade.TransactionFacade.GetTransactionById(transactionHistory.TransactionId);
                                if(transaction!= null)
                                {
                                    string CreditNote = string.Format(@"Invoice# <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{0}')"">{1}</a>", item.Id, item.InvoiceId);
                                    CustomerCredit customerCredit = new CustomerCredit()
                                    {
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        CustomerId = transaction.CustomerId,
                                        TransactionId = transaction.Id,
                                        CreatedBy = CurrentUser.UserId,
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        Amount = Math.Round(transaction.Amount, 2),
                                        Type = LabelHelper.CustomerCreditType.Credit,
                                        IsRefund = false,
                                        Note = CreditNote
                                    };
                                    bool done = _Util.Facade.TransactionFacade.InsertCustomerCredit(customerCredit)>0;
                                    if (done)
                                    {
                                        _Util.Facade.TransactionFacade.DeleteTransactionAndHistoryByTranId(transaction.Id);
                                    }
                                }
                            } 
                        }
                    }
                }
            }

            //_Util.Facade.BookingFacade.DeleteAllBookingDetailsByBookingId(booked.BookingId);
            //_Util.Facade.BookingFacade.DeleteBookingById(Id);

            return Json(new { result = true, message = "Booking Cancelled successfully." });
        }


    }
}