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
using Plivo;

namespace HS.Web.UI.Controllers
{
    public class OrderController : BaseController
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

        public ActionResult Order(string firstdate, string lastdate)
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.LeftMenuOrders))
            {
                return View("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (base.IsPermitted(UserPermissions.OrderPermissions.OrdersTab))
            {
                ViewBag.StartTab = "OrdersTab";
            }
            else if (base.IsPermitted(UserPermissions.OrderPermissions.ReservationsTab))
            {
                ViewBag.StartTab = "ReservationsTab";
            }
            else if (base.IsPermitted(UserPermissions.OrderPermissions.CateringsTab))
            {
                ViewBag.StartTab = "CateringsTab";
            }
            
            else
            {
                return View("~/Views/Shared/_AccessDenied.cshtml");
            }
            ViewBag.firstdate = firstdate;
            ViewBag.lastdate = lastdate;
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            //ViewBag.ispayroll = _Util.Facade.EmployeeFacade.GetEmployeeByUsername(CurrentUser.Identity.Name).IsPayroll;
            return View();
        }
        public PartialViewResult OrderReports()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (currentLoggedIn != null)
            {
                return PartialView("_Order");
            }
            else
            {
                return PartialView();
            }
        }
        public PartialViewResult OrderPartial(int? PageNo, int? PageSize, string SearchText, string order, string startdate, string enddate, Guid? customerid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var objglobalsetting = _Util.Facade.GlobalSettingsFacade.GetOrderListPageLimit(CurrentUser.CompanyId.Value);
            if (objglobalsetting > 0)
            {
                PageSize = objglobalsetting;
            }
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

            //MenuListModel Model = _Util.Facade.MenuFacade.GetMenuList(PageNo.Value, PageSize.Value, SearchText, order);
            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            //ViewBag.order = order;




            //return PartialView("_MenuPartial", Model);
            //  MenuListModel Model = _Util.Facade.MenuFacade.GetMenuList(PageNo.Value, PageSize.Value, SearchText, order);

            ViewBag.order = order;
            List<ResturantOrder> listorder = _Util.Facade.CustomerFacade.GetAllResturantOrderList(CurrentUser.CompanyId.Value, PageNo.Value, PageSize.Value, SearchText, order, startdate, enddate, customerid.Value, false, "", "");
            var objtotalcount = _Util.Facade.MenuFacade.GetAllOrdersByCompanyId(CurrentUser.CompanyId.Value, startdate, enddate, customerid.Value, false, "", "");
            ViewBag.webloc = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(CurrentUser.CompanyId.Value);
            if (listorder.Count() > 0)
            {
                ViewBag.OutOfNumber = objtotalcount.Count;
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
            return PartialView(listorder);
        }

        public PartialViewResult LoadOrdersPartial(int? PageNo, int? PageSize, string SearchText, string order, string startdate, string enddate, bool? filter, string ordertype, string orderstatus)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var objglobalsetting = _Util.Facade.GlobalSettingsFacade.GetOrderListPageLimit(CurrentUser.CompanyId.Value);
            if(objglobalsetting > 0)
            {
                PageSize = objglobalsetting;
            }
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

            //MenuListModel Model = _Util.Facade.MenuFacade.GetMenuList(PageNo.Value, PageSize.Value, SearchText, order);
            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            //ViewBag.order = order;




            //return PartialView("_MenuPartial", Model);
            //  MenuListModel Model = _Util.Facade.MenuFacade.GetMenuList(PageNo.Value, PageSize.Value, SearchText, order);

            ViewBag.order = order;
            List<ResturantOrder> listorder = _Util.Facade.CustomerFacade.GetAllResturantOrderList(CurrentUser.CompanyId.Value,PageNo.Value, PageSize.Value, SearchText, order, startdate, enddate, new Guid(), (filter.HasValue ? filter.Value : false), ordertype, orderstatus);
            var objtotalcount = _Util.Facade.MenuFacade.GetAllOrdersByCompanyId(CurrentUser.CompanyId.Value, startdate, enddate, new Guid(), (filter.HasValue ? filter.Value : false), ordertype, orderstatus);
            ViewBag.webloc = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(CurrentUser.CompanyId.Value);
            ViewBag.OrderSummery = _Util.Facade.MenuFacade.GetRestaurantOrderSummeryByCompanyId(CurrentUser.CompanyId.Value, startdate, enddate, "", "");
            if (listorder.Count() > 0)
            {
                ViewBag.OutOfNumber = objtotalcount.Count;
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
            ViewBag.filter = filter.HasValue ? filter.Value : false;
            return PartialView("_OrderPartial", listorder);
        }

        public PartialViewResult LoadReservationsPartial(int? PageNo, int? PageSize, string SearchText, string order)
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
            return PartialView("_Order");
        }

        public PartialViewResult LoadCateringsPartial(int? PageNo, int? PageSize, string SearchText, string order)
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
            return PartialView("_Order");
        }

        public ActionResult OrderDetailPartial(int? id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ResturantOrderCustomModel model = new ResturantOrderCustomModel();
            if(id.HasValue && id.Value > 0)
            {
                var objorder = _Util.Facade.CustomerFacade.GetResturantOrderById(id.Value);
                if(objorder != null)
                {
                    model.ListOrderDetail = _Util.Facade.CustomerFacade.GetAllResturantOrderDetailByOrderId(objorder.OrderId);
                    if (!string.IsNullOrWhiteSpace(objorder.CardProfile))
                    {
                        var objpayprofile = _Util.Facade.PaymentInfoFacade.GetPaymentProfileCustomerByType(objorder.CardProfile);
                        if (objpayprofile != null)
                        {
                            var objpayinfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoById(objpayprofile.PaymentInfoId);
                            if (objpayinfo != null)
                            {
                                objorder.CardNumber = objpayinfo.CardNumber;
                            }
                        }
                    }
                }
                ViewBag.orderid = id.Value;
                model.ResturantOrder = objorder;
                model.Customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.ResturantOrder.CustomerId);
                model.WebsiteLocation = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(CurrentUser.CompanyId.Value);
                model.RestaurantCoupons = _Util.Facade.MenuFacade.GetCouponsByCompanyIdandCode(CurrentUser.CompanyId.Value, model.ResturantOrder.DiscountCode);
                if(model.ResturantOrder != null)
                {
                    if(model.ResturantOrder.OrderType.ToLower() == "pickup")
                    {
                        List<SelectListItem> orderstatus = new List<SelectListItem>();
                        orderstatus.AddRange(_Util.Facade.LookupFacade.GetLookUpByKey("RestaurantOrderPickupStatus").Select(x => new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList());
                        ViewBag.OrderStatus = orderstatus;
                    }
                    else
                    {
                        List<SelectListItem> orderstatus = new List<SelectListItem>();
                        orderstatus.AddRange(_Util.Facade.LookupFacade.GetLookUpByKey("RestaurantOrderDeliveryStatus").Select(x => new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList());
                        ViewBag.OrderStatus = orderstatus;
                    }
                    List<SelectListItem> timelist = new List<SelectListItem>();
                    var lktimelist = _Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", null, null);
                    timelist.AddRange(lktimelist.Select(x => new SelectListItem()
                    {
                        Text = x.DisplayText.ToString(),
                        Value = x.DataValue.ToString(),
                        Selected = (x.DataValue == (!string.IsNullOrWhiteSpace(model.ResturantOrder.AcceptTime) ? model.ResturantOrder.AcceptTime : model.ResturantOrder.PickUpTime))
                    }).ToList());
                    timelist.RemoveAt(0);
                    ViewBag.timelist = timelist;
                }
                else
                {
                    List<SelectListItem> timelist = new List<SelectListItem>();
                    timelist.AddRange(_Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", null, null).Select(x => new SelectListItem()
                    {
                        Text = x.DisplayText.ToString(),
                        Value = x.DataValue.ToString()
                    }).ToList());
                    timelist.RemoveAt(0);
                    ViewBag.timelist = timelist;
                }
                if (model.ResturantOrder != null)
                {
                    var objlookup = _Util.Facade.LookupFacade.GetLookupByKeyAndValueAndCompanyId("IeateryPaymentOption", model.ResturantOrder.PaymentMethod, model.ResturantOrder.CompanyId);
                    if (objlookup != null)
                    {
                        model.ResturantOrder.PaymentMethod = objlookup.DisplayText;
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult SubmitResturantCustomerOrder(int? orderid, string orderstatus, string reason, string accepttime, List<string> stockItem, List<string> HasstockItem)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            var subtotal = 0.0;
            var total = 0.0;
            var taxtotal = 0.0;
            var discount = 0.0;
            if(orderid.HasValue && orderid.Value > 0)
            {
                WebsiteLocation WebsiteLocation = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(CurrentUser.CompanyId.Value);
                ResturantOrder order = _Util.Facade.CustomerFacade.GetResturantOrderById(orderid.Value);
                if(order != null)
                {
                    var objcoupon = _Util.Facade.MenuFacade.GetCouponsByCompanyIdandCode(CurrentUser.CompanyId.Value, order.DiscountCode);
                    if (!string.IsNullOrWhiteSpace(orderstatus))
                    {
                        order.Status = orderstatus;
                    }
                    if (!string.IsNullOrWhiteSpace(reason))
                    {
                        order.RejectedReason = reason;
                        order.RejectedDate = DateTime.Now.UTCCurrentTime();
                    }
                    if (!string.IsNullOrWhiteSpace(accepttime))
                    {
                        order.AcceptTime = accepttime;
                        order.AcceptDate = Convert.ToDateTime((order.OrderDate.HasValue ? order.OrderDate.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy")) + " " + accepttime).ClientToUTCTime();
                    }
                    order.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    order.LastUpdatedBy = "Admin";
                    result = _Util.Facade.CustomerFacade.UpdateResturantOrder(order);
                    if (result)
                    {
                        if (HasstockItem != null && HasstockItem.Count > 1)
                        {
                            foreach (var item in HasstockItem)
                            {
                                var objorderdetail = _Util.Facade.MenuFacade.GetOrderDetailById(Convert.ToInt32(item));
                                if (objorderdetail != null)
                                {
                                    objorderdetail.IsStock = true;
                                    _Util.Facade.MenuFacade.UpdateOrderDetail(objorderdetail);
                                    subtotal = subtotal + objorderdetail.ItemPrice;
                                }
                            }
                            if (objcoupon != null && subtotal >= Convert.ToDouble(objcoupon.MinimumOrder))
                            {
                                if (objcoupon.DiscountType.ToLower() == "dollar")
                                {
                                    subtotal = subtotal - Convert.ToDouble(objcoupon.Discount);
                                }
                                else
                                {
                                    var disval = (subtotal * Convert.ToDouble(objcoupon.Discount)) / 100;
                                    subtotal = subtotal - disval;
                                }
                            }
                            if ((order.TaxAmount.HasValue ? order.TaxAmount.Value : 0) > 0)
                            {
                                var taxrate = ((order.TaxAmount / ((order.Amount - (order.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0)) - order.TaxAmount)) * 100);
                                taxtotal = Convert.ToDouble((subtotal * taxrate) / 100);
                                total = subtotal + taxtotal + (order.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0);
                            }
                            else
                            {
                                total = subtotal + (order.TaxAmount.HasValue ? order.TaxAmount.Value : 0) + (order.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0);
                            }
                            order.Amount = total;
                            order.TaxAmount = taxtotal;
                            _Util.Facade.MenuFacade.UpdateOrder(order);
                        }
                        else if(HasstockItem != null && HasstockItem.Count == 1)
                        {
                            foreach (var item in HasstockItem)
                            {
                                var objorderdetail = _Util.Facade.MenuFacade.GetOrderDetailById(Convert.ToInt32(item));
                                if (objorderdetail != null)
                                {
                                    objorderdetail.IsStock = true;
                                    _Util.Facade.MenuFacade.UpdateOrderDetail(objorderdetail);
                                    subtotal = subtotal + objorderdetail.ItemPrice;
                                }
                            }
                            if (objcoupon != null && subtotal >= Convert.ToDouble(objcoupon.MinimumOrder))
                            {
                                if (objcoupon.DiscountType.ToLower() == "dollar")
                                {
                                    subtotal = subtotal - Convert.ToDouble(objcoupon.Discount);
                                }
                                else
                                {
                                    var disval = (subtotal * Convert.ToDouble(objcoupon.Discount)) / 100;
                                    subtotal = subtotal - disval;
                                }
                            }
                            if ((order.TaxAmount.HasValue ? order.TaxAmount.Value : 0) > 0)
                            {
                                var taxrate = ((order.TaxAmount / ((order.Amount - (order.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0)) - order.TaxAmount)) * 100);
                                taxtotal = Convert.ToDouble((subtotal * taxrate) / 100);
                                total = subtotal + taxtotal + (order.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0);
                            }
                            else
                            {
                                total = subtotal + (order.TaxAmount.HasValue ? order.TaxAmount.Value : 0) + (order.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0);
                            }
                            order.Amount = total;
                            order.TaxAmount = taxtotal;
                            _Util.Facade.MenuFacade.UpdateOrder(order);
                        }
                        else
                        {
                            order.Status = "Rejected";
                            order.RejectedDate = DateTime.Now.UTCCurrentTime();
                            order.RejectedReason = "Restaurant not accepted your order";
                            _Util.Facade.MenuFacade.UpdateOrder(order);
                        }
                        if (stockItem != null && stockItem.Count > 0)
                        {
                            foreach(var item in stockItem)
                            {
                                var objorderdetail = _Util.Facade.MenuFacade.GetOrderDetailById(Convert.ToInt32(item));
                                if(objorderdetail != null)
                                {
                                    objorderdetail.IsStock = false;
                                    _Util.Facade.MenuFacade.UpdateOrderDetail(objorderdetail);
                                }
                            }
                        }
                    }
                    if (result && (order.Status.ToLower() == "accepted" || order.Status.ToLower() == "rejected" || order.Status.ToLower() == "readypick" || order.Status.ToLower() == "pickedup" || order.Status.ToLower() == "readydeliver" || order.Status.ToLower() == "delivered"))
                    {
                        SendNotificationtoCustomerByMethod(order.CustomerId, order.Status, order.RestaurantLocation, order.Amount, order.Id);
                    }
                }
            }
            return Json(result);
        }

        public bool SendNotificationtoCustomerByMethod(Guid? customerid, string status, string location, double amount, int orderid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            WebsiteLocation WebsiteLocation = new WebsiteLocation();
            string message = "";
            string subject = "";
            List<string> ReceiverNumberList = new List<string>();
            string receivernumber = "";
            bool result = false;
            var sploca = "";
            var urlslug = "";
            if (!string.IsNullOrWhiteSpace(location))
            {
                var sploc = location.Split('~');
                if(sploc.Length == 4)
                {
                    sploca = sploc[1] + ", " + sploc[2];
                    urlslug = sploc[3];
                }
                else
                {
                    sploca = location;
                }
            }
            if (CurrentUser.CompanyId.Value != new Guid())
            {
                WebsiteLocation = _Util.Facade.MenuFacade.GetWebsiteLocationBySlug(urlslug);
                if (!string.IsNullOrWhiteSpace(sploca))
                {
                    if (status == "Accepted")
                    {
                        message = "Your order accepted from " + WebsiteLocation.Name + " in " + sploca;
                        subject = "Your Order at " + WebsiteLocation.Name + " Has Been Accepted";
                    }
                    else if (status == "Rejected")
                    {
                        message = "Your order rejected from " + WebsiteLocation.Name + " in " + sploca;
                        subject = "Your Order at " + WebsiteLocation.Name + " Has Been Rejected";
                    }
                    else if (status.ToLower() == "readypick")
                    {
                        message = "Your order Ready for Pickup from " + WebsiteLocation.Name + " in " + sploca;
                        subject = "Your Order at " + WebsiteLocation.Name + " Has Been Ready to pick";
                    }
                    else if (status.ToLower() == "pickedup")
                    {
                        message = "Your order Pickedup from " + WebsiteLocation.Name + " in " + sploca;
                        subject = "Your Order at " + WebsiteLocation.Name + " Has Been Picked up";
                    }
                    else if (status.ToLower() == "readydeliver")
                    {
                        message = "Your order Ready for Delivery from " + WebsiteLocation.Name + " in " + sploca;
                        subject = "Your Order at " + WebsiteLocation.Name + " Has Been Ready to deliver";
                    }
                    else if (status.ToLower() == "delivered")
                    {
                        message = "Your order Delivered from " + WebsiteLocation.Name + " in " + sploca;
                        subject = "Your Order at " + WebsiteLocation.Name + " Has Been Delivered";
                    }
                }
                else
                {
                    if (status == "Accepted")
                    {
                        message = "Your order accepted from " + WebsiteLocation.Name + " in " + WebsiteLocation.City + ", " + WebsiteLocation.State;
                        subject = "Your Order at " + WebsiteLocation.Name + " Has Been Accepted";
                    }
                    else if (status == "Rejected")
                    {
                        message = "Your order rejected from " + WebsiteLocation.Name + " in " + WebsiteLocation.City + ", " + WebsiteLocation.State;
                        subject = "Your Order at " + WebsiteLocation.Name + " Has Been Rejected";
                    }
                    else if (status.ToLower() == "readypick")
                    {
                        message = "Your order Ready for Pickup from " + WebsiteLocation.Name + " in " + sploca;
                        subject = "Your Order at " + WebsiteLocation.Name + " Has Been Ready to pick";
                    }
                    else if (status.ToLower() == "pickedup")
                    {
                        message = "Your order Pickedup from " + WebsiteLocation.Name + " in " + sploca;
                        subject = "Your Order at " + WebsiteLocation.Name + " Has Been Picked up";
                    }
                    else if (status.ToLower() == "readydeliver")
                    {
                        message = "Your order Ready for Delivery from " + WebsiteLocation.Name + " in " + sploca;
                        subject = "Your Order at " + WebsiteLocation.Name + " Has Been Ready to deliver";
                    }
                    else if (status.ToLower() == "delivered")
                    {
                        message = "Your order Delivered from " + WebsiteLocation.Name + " in " + sploca;
                        subject = "Your Order at " + WebsiteLocation.Name + " Has Been Delivered";
                    }
                }
            }
            else
            {
                return result;
            }
            if (customerid.HasValue && customerid.Value != new Guid())
            {
                Customer model = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid.Value);
                if (model != null)
                {
                    if (model.PreferedSms == true)
                    {
                        if (!string.IsNullOrWhiteSpace(model.CellNo))
                        {
                            receivernumber = model.CellNo.Replace("-", "");
                            ReceiverNumberList.Add(receivernumber);
                            result = _Util.Facade.SMSFacade.SendSMSPrivate(CurrentUser.CompanyId.Value, CurrentUser.UserId, "", message, ReceiverNumberList, false, WebsiteLocation.Name);
                        }
                    }
                    if (model.PreferedEmail == true)
                    {
                        string resturantlogo = "";
                        var objcom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
                        var ResturantSystemSetting = _Util.Facade.MenuFacade.GetResturantSystemSettingByCompanyId(CurrentUser.CompanyId.Value);
                        if (ResturantSystemSetting != null && !string.IsNullOrWhiteSpace(ResturantSystemSetting.Logo))
                        {
                            resturantlogo = ResturantSystemSetting.Logo;
                        }
                        else
                        {
                            resturantlogo = objcom.CompanyLogo;
                        }
                        EmailToOrderPlace EmailToOrderPlace = new EmailToOrderPlace();
                        EmailToOrderPlace.ToEmail = model.EmailAddress;
                        EmailToOrderPlace.Body = message;
                        EmailToOrderPlace.Name = model.FirstName + " " + model.LastName;
                        EmailToOrderPlace.Subject = subject;
                        EmailToOrderPlace.OrderNo = "#" + orderid.ToString();
                        EmailToOrderPlace.TotalAmount = HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency(null) + HS.Web.UI.Helper.LabelHelper.FormatAmount(amount);
                        EmailToOrderPlace.RestaurantPhone = !string.IsNullOrWhiteSpace(WebsiteLocation.TrackingPhonePhone) ? WebsiteLocation.TrackingPhonePhone : WebsiteLocation.StorePhone;
                        EmailToOrderPlace.RestaurantLogo = "https://app.ieatery.com" + resturantlogo;
                        result = _Util.Facade.MailFacade.EmailToPlaceOrder(EmailToOrderPlace, CurrentUser.CompanyId.Value);
                    }
                }
            }
            return result;
        }

        [HttpPost]
        public JsonResult IeateryIsviewChange(int? id)
        {
            bool result = false;
            if(id.HasValue && id.Value > 0)
            {
                var objorder = _Util.Facade.MenuFacade.GetOrderById(id.Value);
                if(objorder != null)
                {
                    objorder.IsViewed = true;
                    result = _Util.Facade.MenuFacade.UpdateOrder(objorder);
                }
            }
            return Json(result);
        }

        public ActionResult GetDeliveryRouteByOrderId(int? id)
        {
            ResturantOrder model = new ResturantOrder();
            if(id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.MenuFacade.GetOrderById(id.Value);
            }
            return View(model);
        }

        public ActionResult GetRestaurantMapUrl(string loc)
        {
            string formatstr = "";
            string gmapurl = "https://www.google.com/maps?q=";
            var sploc = loc.Split("~");
            if(sploc.Length > 0)
            {
                var objwebloc = _Util.Facade.MenuFacade.GetWebsiteLocationBySlug(sploc[3]);
                if(objwebloc != null)
                {
                    formatstr = gmapurl + objwebloc.Name.Replace(" ", "+") + "+" + objwebloc.Address.Replace(" ", "+") + "+" + objwebloc.City + "+" + objwebloc.State + "+" + objwebloc.Zipcode;
                }
                else
                {
                    formatstr = gmapurl;
                }
            }
            return Redirect(formatstr);
        }

        public ActionResult ReviewPartial()
        {
            return View();
        }

        public ActionResult ReviewListPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<ResturantReview> model = _Util.Facade.MenuFacade.GetAllReviewListByCompanyId(CurrentUser.CompanyId.Value);
            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateResturantReview(int? id, bool checkrev)
        {
            bool result = false;
            if(id.HasValue && id.Value > 0)
            {
                var objreview = _Util.Facade.MenuFacade.GetReviewById(id.Value);
                if(objreview != null)
                {
                    objreview.IsActive = checkrev;
                    result = _Util.Facade.MenuFacade.UpdateReview(objreview);
                }
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult SaveReviewReply(int? id, string reply)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            if (id.HasValue && id.Value > 0)
            {
                var objreview = _Util.Facade.MenuFacade.GetReviewById(id.Value);
                if (objreview != null)
                {
                    objreview.Reply = reply;
                    objreview.ReplyBy = CurrentUser.UserId;
                    result = _Util.Facade.MenuFacade.UpdateReview(objreview);
                }
            }
            return Json(result);
        }

        public ActionResult LoadPhoneOrdersTracking(int pageno, int pagesize, string mindate, string maxdate)
        {
            List<TrackingNumberRecorded> model = new List<TrackingNumberRecorded>();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            model = _Util.Facade.MenuFacade.GetAllTrackingNumberRecorded(CurrentUser.CompanyId.Value, mindate, maxdate);
            return View(model);
        }

        public ActionResult EditPhoneOrdersTracking(int? id)
        {
            TrackingNumberRecorded model;
            if(id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.MenuFacade.GetTrackingNumberRecordedById(id.Value);
                model.WebsiteLocation = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(model.CompanyId);
            }
            else
            {
                model = new TrackingNumberRecorded();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveTrackStatus(int? id, string status)
        {
            bool result = false;
            if(id.HasValue && id.Value > 0)
            {
                var objtracknum = _Util.Facade.MenuFacade.GetTrackingNumberRecordedById(id.Value);
                if(objtracknum != null)
                {
                    objtracknum.Status = status;
                    result = _Util.Facade.MenuFacade.UpdateTrackingNumberRecorded(objtracknum);
                }
            }
            return Json(result);
        }

        public ActionResult RewardPartial()
        {
            return View();
        }

        public ActionResult LoadRewardPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            RestaurantRewards model = _Util.Facade.MenuFacade.GetRestaurantRewards(CurrentUser.CompanyId.Value);
            if (model == null)
                model = new RestaurantRewards();
            return View(model);
        }

        public ActionResult LoadCouponPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<RestaurantCoupons> model = _Util.Facade.MenuFacade.GetAllRestaurantCoupons(CurrentUser.CompanyId.Value);
            return View(model);
        }

        public JsonResult SaveRewards(RestaurantRewards rr)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            if(rr.Id > 0)
            {
                var objreward = _Util.Facade.MenuFacade.GetRestaurantRewards(CurrentUser.CompanyId.Value);
                if(objreward != null)
                {
                    objreward.ReedemValue = rr.ReedemValue;
                    objreward.Status = rr.Status;
                    objreward.LastUpdatedBy = CurrentUser.UserId;
                    objreward.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    result = _Util.Facade.MenuFacade.UpdateRestaurantRewards(objreward);
                }
            }
            else
            {
                rr.CompanyId = CurrentUser.CompanyId.Value;
                rr.CreatedBy = CurrentUser.UserId;
                rr.LastUpdatedBy = CurrentUser.UserId;
                rr.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                rr.CreatedDate = DateTime.Now.UTCCurrentTime();
                result = _Util.Facade.MenuFacade.InsertRestaurantRewards(rr) > 0;
            }
            return Json(result);
        }

        public ActionResult AddCoupon(int? id)
        {
            RestaurantCoupons model;
            if(id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.MenuFacade.GetCouponById(id.Value);
            }
            else
            {
                model = new RestaurantCoupons();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult SaveCoupons(RestaurantCoupons rc)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            
            if(rc.Id > 0)
            {
                var objcoupons = _Util.Facade.MenuFacade.GetCouponById(rc.Id);
                if(objcoupons != null)
                {
                    objcoupons.LastUpdatedBy = CurrentUser.UserId;
                    objcoupons.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    objcoupons.CouponCode = rc.CouponCode;
                    objcoupons.StartDate = rc.StartDate;
                    objcoupons.EndDate = rc.EndDate;
                    objcoupons.Discount = rc.Discount;
                    objcoupons.MinimumOrder = rc.MinimumOrder;
                    objcoupons.ReedemRequired = rc.ReedemRequired;
                    objcoupons.DiscountType = rc.DiscountType;
                    objcoupons.Status = rc.Status;
                    result = _Util.Facade.MenuFacade.UpdateCoupons(objcoupons);
                }
            }
            else
            {
                var objcodevalid = _Util.Facade.MenuFacade.GetCouponsByCompanyIdandCode(CurrentUser.CompanyId.Value, rc.CouponCode);
                if (objcodevalid != null)
                {
                    return Json(result);
                }
                rc.CompanyId = CurrentUser.CompanyId.Value;
                rc.CreatedBy = CurrentUser.UserId;
                rc.CreatedDate = DateTime.Now.UTCCurrentTime();
                rc.LastUpdatedBy = CurrentUser.UserId;
                rc.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                result = _Util.Facade.MenuFacade.InsertCoupons(rc) > 0;
            }
            return Json(result);
        }
    }
}
