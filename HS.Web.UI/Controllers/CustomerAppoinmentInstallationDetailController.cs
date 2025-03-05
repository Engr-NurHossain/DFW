using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class CustomerAppoinmentInstallationDetailController : BaseController
    {
        // GET: CustomerAppoinmentInstallationDetail
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.id = id.Value;
            }
            return View();
        }
        public ActionResult CustomerAppoinmentInstallType(Guid? appoinmentID)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<CustomerAppointment> appoinmentDetail = _Util.Facade.CustomerAppoinmentDetailFacade.GetAllCustomerAppoinmentDetailByAppoinmentId(appoinmentID.Value);
            ViewBag.CustomerAppoinmentInstallTypeAppointMentId = appoinmentID;
            return PartialView("_CustomerAppoinmentInstallType", appoinmentDetail);
        }

        [Authorize]
        public ActionResult AddDetailInstallType(string AppoinmentId)
        {
            //CustomerAppointmentDetail model;
            CustomerAppointmentDetailWorkOrder Detailmodel = new CustomerAppointmentDetailWorkOrder();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Guid Appoinmentid = new Guid(AppoinmentId);
            ViewBag.AppointmentId = Appoinmentid;

            var IsAppointmentIdDetailExist = _Util.Facade.CustomerAppoinmentDetailFacade.IsAppointmentIdDetail(Appoinmentid);

            if (IsAppointmentIdDetailExist.Count > 0)
            {
                Detailmodel.CustomerAppointmentDetailList = IsAppointmentIdDetailExist;
                Detailmodel.CustomerAppointmentDetail = _Util.Facade.CustomerAppoinmentDetailFacade.GetCustomerAppointmentDetailByAppointmentId(Appoinmentid);
                var customerObjectAppointmentId = Detailmodel.CustomerAppointmentDetail.AppointmentId;
                var CustomerObjectDetail = _Util.Facade.CustomerAppoinmentDetailFacade.GetAppoinmentidByAppoinmentId(customerObjectAppointmentId);
                if (CustomerObjectDetail != null)
                {
                    Detailmodel.WorkOrderInstallType = CustomerObjectDetail.InstallType;
                    Detailmodel.WorkOrderCollectedAmount = CustomerObjectDetail.CollectedAmount.ToString();
                }
            }

            else
            {

                Detailmodel.CustomerAppointment = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentDetailByAppointmentId(Appoinmentid);
                var customerObjectAppointmentId = Detailmodel.CustomerAppointment.AppointmentId;
                var CustomerObjectDetail = _Util.Facade.CustomerAppoinmentDetailFacade.GetAppoinmentidByAppoinmentId(customerObjectAppointmentId);
                if (CustomerObjectDetail != null)
                {
                    Detailmodel.WorkOrderInstallType = CustomerObjectDetail.InstallType;
                    Detailmodel.WorkOrderCollectedAmount = CustomerObjectDetail.CollectedAmount.ToString();
                }
            }


            ViewBag.InstallType = _Util.Facade.LookupFacade.GetLookupByKey("InstallType").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();

            return PartialView("AddDetailInstallType", Detailmodel);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddDetailInstallType(string AppointmentId,string InstallType,string CollectedAmount)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Guid AppointId = new Guid("00000000-0000-0000-0000-000000000000");
            double ColAmount = 0.0;
            if (!string.IsNullOrWhiteSpace(AppointmentId) && !string.IsNullOrWhiteSpace(CollectedAmount))
            {
                AppointId = new Guid(AppointmentId);
                ColAmount = Convert.ToDouble(CollectedAmount);
            }
            
            if (currentLoggedIn != null)
            {
                var DeletePreviousDetailList = _Util.Facade.CustomerAppoinmentDetailFacade.IsAppointmentDetailExistCheck(AppointId);
                if (DeletePreviousDetailList != null)
                {
                    _Util.Facade.CustomerAppoinmentDetailFacade.DeletePreviousCustomerDetailsByEquipmentId(DeletePreviousDetailList);
                }
                CustomerAppointmentDetail ObjCus = new CustomerAppointmentDetail();
                ObjCus.AppointmentId = AppointId;
                ObjCus.InstallType = InstallType;
                ObjCus.CollectedAmount = ColAmount;

                _Util.Facade.CustomerAppoinmentDetailFacade.InsertCustomerAppointmentDetail(ObjCus);
                if (!string.IsNullOrWhiteSpace(AppointmentId))
                {
                    var ObjCusId = _Util.Facade.CustomerAppoinmentFacade.GetCustomerIdByAppointmentIdAndAppointmentType(AppointId);
                    if(ObjCusId != null)
                    {
                        var objWorkAppointment = _Util.Facade.PaymentRevenueFacade.GetWorkOrderPaymentRevenueByWorkId(ObjCusId.Id);
                        if (objWorkAppointment == null)
                        {
                            PaymentRevenue objPaymentRevenue = new PaymentRevenue()
                            {
                                CompanyId = currentLoggedIn.CompanyId.Value,
                                CustomerId = ObjCusId.CustomerId,
                                Type = "Work Order",
                                Amount = ColAmount,
                                Status = "Work Order Revenue",
                                TransacationDate = DateTime.Now.UTCCurrentTime(),
                                AddedBy = currentLoggedIn.Identity.Name,
                                AddedDate = DateTime.Now.UTCCurrentTime(),
                                Desccription = "Work Order Installation Amount by" + ObjCusId.WorkOrderCustomerName,
                                WorkOrder = ObjCusId.Id
                            };
                            _Util.Facade.PaymentRevenueFacade.InsertWorkOrderPaymentRevenue(objPaymentRevenue);
                        }
                        if(objWorkAppointment != null)
                        {
                            if(objWorkAppointment.Id > 0)
                            {
                                objWorkAppointment.Amount = ColAmount;
                                _Util.Facade.PaymentRevenueFacade.UpdateWorkOrderPaymentRevenue(objWorkAppointment);
                            }
                            
                        }
                    }
                }
            }
            ////bool result = false;
            //var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            //if(currentLoggedIn != null)
            //{
            //    var DeletePreviousDetailList = _Util.Facade.CustomerAppoinmentDetailFacade.IsAppointmentDetailExistCheck(ad.AppointmentId);
            //    if (DeletePreviousDetailList != null)
            //    {
            //        _Util.Facade.CustomerAppoinmentDetailFacade.DeletePreviousCustomerDetailsByEquipmentId(DeletePreviousDetailList);
            //    }
            //    List<CustomerAppointmentDetail> tempCa = new List<CustomerAppointmentDetail>();
            //    foreach (var items in CustomerAppointmentDetailWorkOrder.CustomerAppointmentDetailList)
            //    {
            //        items.AppointmentId = ad.AppointmentId;
            //        items.InstallType = ad.InstallType;
            //        items.CollectedAmount = ad.CollectedAmount;
            //    }
            //    tempCa = CustomerAppointmentDetailWorkOrder.CustomerAppointmentDetailList;
            //    if (tempCa.Count > 0)
            //    {
            //        _Util.Facade.CustomerAppoinmentDetailFacade.InsertCustomerAppointmentDetail(tempCa);
            //    }
            //    return Json(true);
            //}
            //else
            //{
            //    return Json(false);
            //}
            return Json(false);
        }
    }
}