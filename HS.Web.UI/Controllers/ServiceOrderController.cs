using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using Rotativa.Options;
using HS.Web.UI.Helper;
using System.Configuration;
using System.Globalization;
using HS.Framework;
using HS.Framework.Utils;

namespace HS.Web.UI.Controllers
{
    public class ServiceOrderController : BaseController
    {
        // GET: ServiceOrder
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
        public ActionResult ServiceOrderPartial(Guid? customerid)
        {

            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerServiceOrderList ))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            bool res = _Util.Facade.CustomerFacade.CustomerIsInCompanySalesPartial(customerid.Value, currentLoggedIn.CompanyId.Value);
            if (!res)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid.Value);
            Guid? employeeId = null;

            List<CustomerAppointment> CustomerAppoint = _Util.Facade.CustomerAppoinmentFacade.GetAllServiceAppoinmentByEmployeeId(tmpCustomer.CustomerId, currentLoggedIn.CompanyId.Value, employeeId);
            var count = CustomerAppoint.Count();

            return PartialView("_ServiceOrderPartial", CustomerAppoint);
        }
        [Authorize]
        public ActionResult AddServiceOrder(int? id, int customerid, int? Date, int? Month, int? Year)
        {
            CustomerAppointment model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (Date.HasValue && Month.HasValue && Year.HasValue)
            {
                //ViewBag.SelectedDate = Date.ToString() + "-" + Month.ToString() + "-" + Year.ToString();
                ViewBag.SelectedDate = Month.ToString() + "-" + Date.ToString() + "-" + Year.ToString();
            }
            if (id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.CustomerAppoinmentFacade.GetAppointmentById(id.Value);
            }
            else
            {
                model = new CustomerAppointment();
            }
            string NewGuid = new Guid().ToString();
            ViewBag.CustomerId = NewGuid;
            var CustomerDetails = _Util.Facade.CustomerFacade.GetCustomersById(customerid);
            if (CustomerDetails != null)
            {
                if (CustomerDetails.Installer != NewGuid && CustomerDetails.Installer != "")
                {
                    model.EmployeeId = Guid.Parse(CustomerDetails.Installer);
                }
                ViewBag.CustomerId = CustomerDetails.CustomerId.ToString();
            }

            //ViewBag.CustomerId = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid).CustomerId.ToString();

            ViewBag.ServiceList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value,LabelHelper.UserTags.ServicePerson, new Guid()).Select(x =>
                 new SelectListItem()
                 {
                     Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                     Value = x.UserId.ToString()
                 }).ToList();
            ViewBag.Technician = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid()).Select(x =>
                  new SelectListItem()
                  {
                      Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                      Value = x.UserId.ToString()
                  }).ToList();
            ViewBag.AppointmentTime = _Util.Facade.LookupFacade.GetLookupByKey("Arrival").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();

            var CustomerObj = _Util.Facade.CustomerFacade.GetById(customerid);
            if (CustomerObj != null)
            {
                string CustomerName = "";
                if (CustomerObj.MiddleName != null)
                {
                    CustomerName = CustomerObj.FirstName + " " + CustomerObj.MiddleName + " " + CustomerObj.LastName;
                }
                else
                {
                    CustomerName = CustomerObj.FirstName + CustomerObj.LastName;
                }
                ViewBag.CustomerName = CustomerName;
            }

            return PartialView("AddServiceOrder", model);
        }
        public ActionResult ServiceOrderCalendar()
        {
            return PartialView("_ServiceOrderCalendar");
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddServiceOrder(CustomerAppointment ca, CustomerAppointmentTechnician cat)
        {
            bool result = false;
            string message1 = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ca.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value;
            ca.AppointmentType = AppoinmentType.ServiceOrder;
            ca.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            ca.LastUpdatedBy = User.Identity.Name;
            if (ca.Id > 0)
            {
                CustomerAppointment tempCa = _Util.Facade.CustomerAppoinmentFacade.GetById(ca.Id);
                if (tempCa.CompanyId != ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value)
                {
                    return Json(new { result = false, message = "Invalid Parameter" });
                }
                ca.CreatedBy = tempCa.CreatedBy;
                ca.AppointmentId = tempCa.AppointmentId;
                result = _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinment(ca);
                if (result == true)
                {
                    Guid cusID = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentidByAppoinmentId(ca.AppointmentId).CustomerId;
                    var LSchedule = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(cusID);
                    if (LSchedule != null)
                    {
                        var getSchedule = _Util.Facade.ScheduleFacade.GetServiceScheduleByCustomerid(LSchedule.Id);
                        if (getSchedule.Id > 0)
                        {
                            DateTime ScheduleStartDateandTime;
                            DateTime ScheduleEndDateandTime;
                            DateTime Servicedate = Convert.ToDateTime(ca.AppointmentDate);
                            DateTime ServiceStartTime = Convert.ToDateTime(ca.AppointmentStartTime);
                            DateTime ServiceEndTime = Convert.ToDateTime(ca.AppointmentEndTime);
                            ScheduleStartDateandTime = Servicedate.Date.Add(ServiceStartTime.TimeOfDay);
                            ScheduleEndDateandTime = Servicedate.Date.Add(ServiceEndTime.TimeOfDay);
                            var objAppointmentCustomerId = _Util.Facade.CustomerAppoinmentFacade.GetCustomerIdByCustomerAppointmentCustomerId(cusID).Id;
                            var objEmployeeName = _Util.Facade.CustomerAppoinmentFacade.GteEmployeeNameByCustomerAppointmentEmployeeId(ca.EmployeeId).EMPName;
                            Schedule ServiceOrderSchedule = new Schedule()
                            {
                                Id = getSchedule.Id,
                                CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value,
                                Type = getSchedule.Type,
                                StartDate = ScheduleStartDateandTime,
                                EndDate = ScheduleEndDateandTime,
                                IsCompleted = false,
                                Title = getSchedule.Type + " required for " + objEmployeeName,
                                LeadId = Convert.ToInt32(objAppointmentCustomerId),
                                IsFullDay = true,
                                Identifier = ca.EmployeeId.ToString()
                            };
                            _Util.Facade.ScheduleFacade.UpdateSchedule(ServiceOrderSchedule);
                        }
                    }
                }
            }
            else
            {
                var userobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(ca.EmployeeId);
                if (userobj != null)
                {
                    var applist = _Util.Facade.ScheduleFacade.GetAppointmentScheduleByAppointmentStartAndEndDatetimeAndEmployeeid(ca.AppointmentDate.ToString(), ca.AppointmentStartTime, ca.EmployeeId, ca.AppointmentEndTime);
                    var applist1 = _Util.Facade.ScheduleFacade.GetAppointmentScheduleByAppointmentDatetimeAndEmployeeid(ca.AppointmentDate.ToString(), ca.AppointmentStartTime, ca.EmployeeId, ca.AppointmentEndTime);
                    if (applist.Count > 1)
                    {
                        message1 = "This schedule already exist";
                    }
                    else
                    {
                        if(applist.Count == 1)
                        {
                            foreach(var item in applist)
                            {
                                if(ca.AppointmentId == item.AppointmentId)
                                {
                                    ca.CreatedBy = User.Identity.Name;
                                    ca.AppointmentId = Guid.NewGuid();
                                    ca.Status = false;
                                    if (ca.AppointmentDate.HasValue)
                                    {
                                        ca.AppointmentDate = ca.AppointmentDate.Value.SetZeroHour();
                                    }
                                    string Idstring = ca.AppointmentId.ToString();
                                    result = _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(ca) > 0;
                                }
                                else
                                {
                                    message1 = "This schedule already exist";
                                }
                            }
                        }
                        else
                        {
                            if(applist1.Count > 0)
                            {
                                message1 = "This schedule already exist";
                            }
                            else
                            {
                                ca.CreatedBy = User.Identity.Name;
                                ca.AppointmentId = Guid.NewGuid();
                                ca.Status = false;
                                if (ca.AppointmentDate.HasValue)
                                {
                                    ca.AppointmentDate = ca.AppointmentDate.Value.SetZeroHour();
                                }
                                string Idstring = ca.AppointmentId.ToString();
                                result = _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(ca) > 0;
                            }
                        }
                    }
                }

                if (result == true)
                {
                    DateTime ScheduleStartDateandTime;
                    DateTime ScheduleEndDateandTime;
                    DateTime Servicedate = ca.AppointmentDate.Value;
                    DateTime ServiceStartTime = Convert.ToDateTime(ca.AppointmentStartTime);
                    DateTime ServiceEndTime = Convert.ToDateTime(ca.AppointmentEndTime);
                    ScheduleStartDateandTime = Servicedate.Date.Add(ServiceStartTime.TimeOfDay);
                    ScheduleEndDateandTime = Servicedate.Date.Add(ServiceEndTime.TimeOfDay);
                    var objAppointmentCustomerId = _Util.Facade.CustomerAppoinmentFacade.GetCustomerIdByCustomerAppointmentCustomerId(ca.CustomerId).Id;
                    var objEmployeeName = _Util.Facade.CustomerAppoinmentFacade.GteEmployeeNameByCustomerAppointmentEmployeeId(ca.EmployeeId).EMPName;
                    Schedule ServiceOrderSchedule = new Schedule()
                    {
                        CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value,
                        Type = ca.AppointmentType,
                        StartDate = ScheduleStartDateandTime,
                        EndDate = ScheduleEndDateandTime,
                        IsCompleted = false,
                        Title = ca.AppointmentType + " required for " + objEmployeeName,
                        LeadId = Convert.ToInt32(objAppointmentCustomerId),
                        IsFullDay = true,
                        Identifier = ca.EmployeeId.ToString()
                    };
                    _Util.Facade.ScheduleFacade.InsertSchedule(ServiceOrderSchedule);
                    if(cat.ListHelperTech.Count > 0)
                    {
                        foreach(var item in cat.ListHelperTech)
                        {
                            CustomerAppointmentTechnician objtech = new CustomerAppointmentTechnician()
                            {
                                EmployeeId = new Guid(item),
                                CustomerAppointmentId = ca.Id
                            };
                            _Util.Facade.CustomerAppoinmentFacade.InsertAppointmentTechnician(objtech);
                        }
                    }
                }
                string EmployeeName = "";
                var EmployeeDetails = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(ca.EmployeeId);
                if (EmployeeDetails != null)
                {
                    EmployeeName = EmployeeDetails.FirstName + " " + EmployeeDetails.LastName;
                }
                var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                if(empobj != null)
                {
                    var servicesnap = ca.LastUpdatedDate.UTCToClientTime();
                    CustomerSnapshot objSnapShotHistory = new CustomerSnapshot
                    {
                        CompanyId = ca.CompanyId,
                        CustomerId = ca.CustomerId,
                        Description = string.Format("<a onclick=OpenTopToBottomModal('{3}/ServiceOrder/TopToBottomModalServiceOrder?AppointmentId={0}&CustomerId={1}') style='cursor: pointer;'>", ca.AppointmentId, ca.CustomerId, AppConfig.DomainSitePath) + "<b>" + "Service order#" + ca.Id + "</b>" + "</a>",
                        Logdate = DateTime.Now.UTCCurrentTime(),
                        Updatedby = empobj.FirstName + " " + empobj.LastName,
                        Type = "ServiceOrderCreated"
                    };
                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objSnapShotHistory);
                }
                
            }

            return Json(new { result = result, message = "Invalid Parameter", CustomerId = ca.CustomerId, AppointmentId = ca.AppointmentId, message1 = message1 });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteServiceAppointment(int? id)
        {
            if (id.HasValue)
            {
                var service = _Util.Facade.CustomerAppoinmentFacade.DeleteServiceAppointment(id.Value);
            }
            return Json(true);
        }

        [Authorize]
        public PartialViewResult TopToBottomModalServiceOrder(Guid AppointmentId, Guid CustomerId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            AddCustomerAppointment model = new AddCustomerAppointment();
            //check valid user or not
            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (AppointmentId != null && CustomerId != null)
            {
                //check CustomerAppointmentEquipments exist or not
                var IsAppointmentEquipmentExist = _Util.Facade.CustomerAppoinmentFacade.IsAppointmentEquipmentExistCheck(AppointmentId);
       
                if (IsAppointmentEquipmentExist.Count > 0)
                {
                    model.CustomerAppointmentEquipmentList = IsAppointmentEquipmentExist;

                    model.CustomerAppointment = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentinfoByAppointmentIdandCustomerIdandCompanyId(AppointmentId, CustomerId, currentLoggedIn.CompanyId.Value);
                    var customerObjectCustomerId = model.CustomerAppointment.CustomerId;
                    var customerObject = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerObjectCustomerId);
                    if (customerObject != null)
                    {
                        model.ServiceOrderCustomerName = customerObject.FirstName + " " + customerObject.LastName;
                        model.ServiceOrderCustomerPhone = customerObject.PrimaryPhone;
                        model.ServiceOrderCustomerEmail = customerObject.EmailAddress;
                        model.ServiceOrderCustomerAddress = customerObject.Address;
                        model.StreetName = customerObject.Street;
                        model.CityName = customerObject.City;
                        model.StateName = customerObject.State;
                        model.ZipName = customerObject.ZipCode;
                    }
                    foreach (var items in model.CustomerAppointmentEquipmentList)
                    {
                        Guid EquipmentId = items.EquipmentId;
                        String EquipmentName = _Util.Facade.EquipmentFacade.GetEquipmentServiceNameByEquipmentId(EquipmentId);
                        if (EquipmentName != null)
                        {
                            items.EquipmentServiceName = EquipmentName;
                        }
                        else
                        {
                            items.EquipmentServiceName = "";
                        }
                    }
                    model.ListCustomerAppointmentTechnician = _Util.Facade.CustomerAppoinmentFacade.GetAllAppointmentTechnicianByAppointmentId(model.CustomerAppointment.Id);
                }

                else
                {
                    model.CustomerAppointment = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentinfoByAppointmentIdandCustomerIdandCompanyId(AppointmentId, CustomerId, currentLoggedIn.CompanyId.Value);
                    var customerObjectCustomerId = model.CustomerAppointment.CustomerId;
                    var customerObject = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerObjectCustomerId);
                    if (customerObject != null)
                    {
                        model.ServiceOrderCustomerName = customerObject.FirstName + " " + customerObject.LastName;
                        model.ServiceOrderCustomerPhone = customerObject.PrimaryPhone;
                        model.ServiceOrderCustomerEmail = customerObject.EmailAddress;
                        model.ServiceOrderCustomerAddress = customerObject.Address;
                        model.StreetName = customerObject.Street;
                        model.CityName = customerObject.City;
                        model.StateName = customerObject.State;
                        model.ZipName = customerObject.ZipCode;
                    }
                    var salesPersonObject = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.CustomerAppointment.EmployeeId);
                    if (salesPersonObject != null)
                    {
                        model.ServiceOrderEmployeeName = salesPersonObject.FirstName + " " + salesPersonObject.LastName;
                    }
                    model.ListCustomerAppointmentTechnician = _Util.Facade.CustomerAppoinmentFacade.GetAllAppointmentTechnicianByAppointmentId(model.CustomerAppointment.Id);
                }

                ViewBag.ServiceProviderEmployeeList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.ServicePerson, new Guid()).Select(x =>
                   new SelectListItem()
                   {
                       Text = x.FirstName.ToString(),
                       Value = x.UserId.ToString()
                   }).ToList();
                ViewBag.AppointmentTime = _Util.Facade.LookupFacade.GetLookupByKey("Arrival").Select(x =>
                                new SelectListItem()
                                {
                                    Text = x.DisplayText.ToString(),
                                    Value = x.DataValue.ToString()
                                }).ToList();

                List<SelectListItem> TaxListItem = new List<SelectListItem>();
                var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(CustomerId, currentLoggedIn.CompanyId.Value);
                if (GetCityTaxList.Count > 0)
                {
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
                    if (model != null)
                    {
                        tempCustomerId = model.CustomerAppointment.CustomerId;
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
                TaxListItem.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("NonTaxValue").Select(x => new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList());

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

                ViewBag.Technician = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid()).Select(x =>
                  new SelectListItem()
                  {
                      Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                      Value = x.UserId.ToString()
                  }).ToList();
            }
            return PartialView("_TopToBottomModalServiceOrder", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddCustomerAppointmentDetail(AddCustomerAppointment AddCustomerAppointment, Guid AppointmentIdForAddProduct, Guid ServiceOrderEmployeeId, DateTime ServiceOrderDate, string ServiceOrderAppointmentStartTime, string ServiceOrderAppointmentEndTime, string ServiceOrderNote, string ServiceOrderTaxPercent, string ServiceOrderTaxTotal, string ServiceOrderTotalAmount, string ServiceOrderTotalAmountTax, string ServiceOrderTaxType, bool IsComplete, List<string> ListHelperTech)
        {
            //AddCustomerAppointment model;
            bool result = false;
            bool MakeComplete = false;
            string message1 = "";
            Guid cusID = new Guid();
            int appid = 0;
            bool res1 = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn != null)
            {
                var DeletePreviousEquipmentList = _Util.Facade.CustomerAppoinmentFacade.IsAppointmentEquipmentExistCheck(AppointmentIdForAddProduct);
                if (DeletePreviousEquipmentList != null)
                {
                    result = _Util.Facade.CustomerAppoinmentFacade.DeletePreviousCustomerDetailsEquiptmentByEquipmentId(DeletePreviousEquipmentList);
                }

                if (AppointmentIdForAddProduct != null)
                {
                    #region Update
                    CustomerAppointment OldCustomerAppointmentObject = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentObjectByAppointmentId(AppointmentIdForAddProduct);
                    if (OldCustomerAppointmentObject != null)
                    {
                        var userobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(ServiceOrderEmployeeId);
                        if (userobj != null)
                        {
                            var applist = _Util.Facade.ScheduleFacade.GetAppointmentScheduleByAppointmentStartAndEndDatetimeAndEmployeeid(ServiceOrderDate.ToString(), ServiceOrderAppointmentStartTime, ServiceOrderEmployeeId, ServiceOrderAppointmentEndTime);
                            var applist1 = _Util.Facade.ScheduleFacade.GetAppointmentScheduleByAppointmentDatetimeAndEmployeeid(ServiceOrderDate.ToString(), ServiceOrderAppointmentStartTime, ServiceOrderEmployeeId, ServiceOrderAppointmentEndTime);
                            if (applist.Count > 1)
                            {
                                message1 = "This schedule already exist";
                            }
                            else
                            {
                                if (applist.Count == 1)
                                {
                                    foreach (var item in applist)
                                    {
                                        if (AppointmentIdForAddProduct == item.AppointmentId)
                                        {
                                            OldCustomerAppointmentObject.AppointmentDate = ServiceOrderDate.SetZeroHour();
                                            if (string.IsNullOrWhiteSpace(ServiceOrderAppointmentStartTime)
                                                || string.IsNullOrWhiteSpace(ServiceOrderAppointmentStartTime))
                                            {
                                                OldCustomerAppointmentObject.IsAllDay = true;
                                            }
                                            else
                                            {
                                                OldCustomerAppointmentObject.AppointmentEndTime = ServiceOrderAppointmentEndTime;
                                                OldCustomerAppointmentObject.AppointmentStartTime = ServiceOrderAppointmentStartTime;

                                            }
                                            OldCustomerAppointmentObject.EmployeeId = ServiceOrderEmployeeId;
                                            OldCustomerAppointmentObject.Notes = ServiceOrderNote;
                                            OldCustomerAppointmentObject.TaxPercent = Convert.ToDouble(ServiceOrderTaxPercent);
                                            OldCustomerAppointmentObject.TaxTotal = Convert.ToDouble(ServiceOrderTaxTotal);
                                            OldCustomerAppointmentObject.TotalAmount = Convert.ToDouble(ServiceOrderTotalAmount);
                                            OldCustomerAppointmentObject.TotalAmountTax = Convert.ToDouble(ServiceOrderTotalAmountTax);
                                            OldCustomerAppointmentObject.TaxType = ServiceOrderTaxType;
                                            result = _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppointmentInformation(OldCustomerAppointmentObject);
                                            appid = OldCustomerAppointmentObject.Id;
                                        }
                                        else
                                        {
                                            message1 = "This schedule already exist";
                                        }
                                    }
                                }
                                else
                                {
                                    if (applist1.Count > 0)
                                    {
                                        message1 = "This schedule already exist";
                                    }
                                    else
                                    {
                                        OldCustomerAppointmentObject.AppointmentDate = ServiceOrderDate.SetZeroHour();
                                        if (string.IsNullOrWhiteSpace(ServiceOrderAppointmentStartTime)
                                            || string.IsNullOrWhiteSpace(ServiceOrderAppointmentStartTime))
                                        {
                                            OldCustomerAppointmentObject.IsAllDay = true;
                                        }
                                        else
                                        {
                                            OldCustomerAppointmentObject.AppointmentEndTime = ServiceOrderAppointmentEndTime;
                                            OldCustomerAppointmentObject.AppointmentStartTime = ServiceOrderAppointmentStartTime;

                                        }
                                        OldCustomerAppointmentObject.EmployeeId = ServiceOrderEmployeeId;
                                        OldCustomerAppointmentObject.Notes = ServiceOrderNote;
                                        OldCustomerAppointmentObject.TaxPercent = Convert.ToDouble(ServiceOrderTaxPercent);
                                        OldCustomerAppointmentObject.TaxTotal = Convert.ToDouble(ServiceOrderTaxTotal);
                                        OldCustomerAppointmentObject.TotalAmount = Convert.ToDouble(ServiceOrderTotalAmount);
                                        OldCustomerAppointmentObject.TotalAmountTax = Convert.ToDouble(ServiceOrderTotalAmountTax);
                                        OldCustomerAppointmentObject.TaxType = ServiceOrderTaxType;
                                        result = _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppointmentInformation(OldCustomerAppointmentObject);
                                        appid = OldCustomerAppointmentObject.Id;
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }

                cusID = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentidByAppoinmentId(AppointmentIdForAddProduct).CustomerId;

                #region No need schedule now
                //var LSchedule = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(cusID);
                //if (LSchedule != null)
                //{
                //    var getSchedule = _Util.Facade.ScheduleFacade.GetServiceScheduleByCustomerid(LSchedule.Id);
                //    if (getSchedule.Id > 0)
                //    {
                //        DateTime ScheduleStartDateandTime;
                //        DateTime ScheduleEndDateandTime;
                //        DateTime Servicedate = Convert.ToDateTime(ServiceOrderDate);
                //        DateTime ServiceStartTime = Convert.ToDateTime(ServiceOrderAppointmentStartTime);
                //        DateTime ServiceEndTime = Convert.ToDateTime(ServiceOrderAppointmentEndTime);
                //        ScheduleStartDateandTime = Servicedate.Date.Add(ServiceStartTime.TimeOfDay);
                //        ScheduleEndDateandTime = Servicedate.Date.Add(ServiceEndTime.TimeOfDay);
                //        var objAppointmentCustomerId = _Util.Facade.CustomerAppoinmentFacade.GetCustomerIdByCustomerAppointmentCustomerId(cusID).Id;
                //        var objEmployeeName = _Util.Facade.CustomerAppoinmentFacade.GteEmployeeNameByCustomerAppointmentEmployeeId(ServiceOrderEmployeeId).EMPName;
                //        Schedule ServiceOrderSchedule = new Schedule()
                //        {
                //            Id = getSchedule.Id,
                //            CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value,
                //            Type = getSchedule.Type,
                //            StartDate = ScheduleStartDateandTime,
                //            EndDate = ScheduleEndDateandTime,
                //            IsCompleted = false,
                //            Title = getSchedule.Type + " required for " + objEmployeeName,
                //            LeadId = Convert.ToInt32(objAppointmentCustomerId),
                //            IsFullDay = true,
                //            Identifier = ServiceOrderEmployeeId.ToString()
                //        };
                //        result = _Util.Facade.ScheduleFacade.UpdateSchedule(ServiceOrderSchedule);
                //    }
                //}
                #endregion

                List<CustomerAppointmentEquipment> objCustomerAppointmentEquipment = new List<CustomerAppointmentEquipment>();
                
                
                if(AddCustomerAppointment.CustomerAppointmentEquipmentList != null)
                {
                    objCustomerAppointmentEquipment = AddCustomerAppointment.CustomerAppointmentEquipmentList;
                    if (objCustomerAppointmentEquipment.Count > 0)
                    {
                        foreach (var items in AddCustomerAppointment.CustomerAppointmentEquipmentList)
                        {
                            items.AppointmentId = AppointmentIdForAddProduct;
                            items.CreatedBy = User.Identity.Name;
                            items.CreatedDate = DateTime.Now.UTCCurrentTime();
                            items.Id = _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppointmentEquipmentDetail(items);
                        }
                        //result = _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppointmentEquipmentDetails(objCustomerAppointmentEquipment);
                    }
                }
                if (IsComplete == true)
                {
                    MakeServiceOrderComplete(appid);
                    MakeComplete = true;
                }
                #region HelperTech Not used anymore
                //_Util.Facade.CustomerAppoinmentFacade.DeleteAppointmentAllTech(appid);
                //if (ListHelperTech.Count > 0)
                //{
                //    foreach(var item1 in ListHelperTech)
                //    {
                //        CustomerAppointmentTechnician objapptech = new CustomerAppointmentTechnician()
                //        {
                //            EmployeeId = new Guid(item1),
                //            CustomerAppointmentId = appid
                //        };
                //        _Util.Facade.CustomerAppoinmentFacade.InsertAppointmentTechnician(objapptech);
                //    }
                //}
                #endregion

                
                string Message = "Service order saved successfully.";
                if (MakeComplete)
                {
                    Message = "Service order successfully marked as complete.";
                }
                return Json(new { result = result, cusID = cusID, MakeComplete = MakeComplete,message = Message, message1 = message1 });
            }
            else
            {
                return Json(new { result = result, cusid = cusID, message1 = message1 });
            }
        }

        public ActionResult GetCustomerAppointmentPdfView(int Id)
        {
            AddCustomerAppointment model = new AddCustomerAppointment();
            var CustomerAppointmentObj = _Util.Facade.CustomerAppoinmentFacade.GetAppointmentById(Id);
            model.CustomerAppointment = CustomerAppointmentObj;
            return View("GetCustomerAppointmentPdfView", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveServiceOrderPdf(int AppointmentId, List<ServiceOrderPdfDetail> ServiceOrderPdfDetail, string TotalServiceOrderPrice, string ServiceOrderTaxAmount, string ServiceOrderSubTotalAmount)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            AddCustomerAppointment modelServiceOrderPdf = new AddCustomerAppointment();

            var CustomerAppointment = _Util.Facade.CustomerAppoinmentFacade.GetAppointmentById(AppointmentId);
            if (CustomerAppointment != null)
            {
                modelServiceOrderPdf.CustomerAppointment = CustomerAppointment;
            }

            if (CustomerAppointment.CustomerId != null)
            {
                var customerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerAppointment.CustomerId);
                if (customerDetails != null)
                {
                    string CustomerAddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(currentLoggedIn.CompanyId.Value);
                    modelServiceOrderPdf.ServiceOrderCustomerAddress = AddressHelper.MakeCustomerAddress(customerDetails, AddressHelper.ShippingAddress, CustomerAddressTemplate);
                }
                var CustomerSystemDetails = _Util.Facade.CustomerFacade.GetZoneInfoByCustomerId(CustomerAppointment.CustomerId);
                if(CustomerSystemDetails != null)
                {
                    modelServiceOrderPdf.ServiceZone1 = CustomerSystemDetails.Zone1;
                    modelServiceOrderPdf.ServiceZone2 = CustomerSystemDetails.Zone2;
                    modelServiceOrderPdf.ServiceZone3 = CustomerSystemDetails.Zone3;
                    modelServiceOrderPdf.ServiceZone4 = CustomerSystemDetails.Zone4;
                    modelServiceOrderPdf.ServiceZone5 = CustomerSystemDetails.Zone5;
                    modelServiceOrderPdf.ServiceZone6 = CustomerSystemDetails.Zone6;
                    modelServiceOrderPdf.ServiceZone7 = CustomerSystemDetails.Zone7;
                    modelServiceOrderPdf.ServiceZone8 = CustomerSystemDetails.Zone8;
                    modelServiceOrderPdf.ServiceZone9 = CustomerSystemDetails.Zone9;
                }
            }
            if (CustomerAppointment.EmployeeId != null)
            {
                var EmployeeName = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CustomerAppointment.EmployeeId);
                if (EmployeeName != null)
                {
                    modelServiceOrderPdf.ServiceOrderEmployeeName = EmployeeName.FirstName + " " + EmployeeName.LastName;

                }
            }
            if (CustomerAppointment.AppointmentId != null)
            {
                Guid GuidAppointmentId = CustomerAppointment.AppointmentId;
                
                if (ServiceOrderPdfDetail != null)
                {
                    if (ServiceOrderPdfDetail.Count > 0)
                    {
                        modelServiceOrderPdf.ServiceOrderPdfDetail = ServiceOrderPdfDetail;
                    }
                }
                modelServiceOrderPdf.ServiceNote = CustomerAppointment.Notes;
                modelServiceOrderPdf.PriceServiceOrderTotal = TotalServiceOrderPrice;
                if (ServiceOrderTaxAmount != HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency())
                {
                    modelServiceOrderPdf.ServiceOrderTaxAmount = ServiceOrderTaxAmount;
                }
                
                modelServiceOrderPdf.ServiceOrderSubTotalAmount = ServiceOrderSubTotalAmount;
            }

            Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(modelServiceOrderPdf.CustomerAppointment.CompanyId);
            string CompanyAddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(currentLoggedIn.CompanyId.Value);
            modelServiceOrderPdf.ServiceOrderCompanyAddress = AddressHelper.MakeCompanyAddress(tempCom, CompanyAddressTemplate);
            string tempcompanyBranch = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(currentLoggedIn.CompanyId.Value);
            modelServiceOrderPdf.CompanyLogo = tempcompanyBranch;
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("ServiceOrderPDF", modelServiceOrderPdf)
            {
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.ServiceOrderFiles"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            filename = string.Format(filename, comname);
            filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + rand.Next().ToString() + "_" + rand.Next().ToString() + "_ServiceOrder.pdf";
            string Serverfilename = FileHelper.GetFileFullPath(filename);
            FileHelper.SaveFile(applicationPDFData, Serverfilename);

            return Json(new { result = true, message = "Service Order Successfully Saved", filePath = AppConfig.DomainSitePath + filename });

        }

        private void MakeServiceOrderComplete(int Id)
        {
            bool result = false;
            bool invoiceresult = false;
            string EmpMail = "";
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            if(empobj != null)
            {
                EmpMail = empobj.UserName;
            }
            CustomerAppointment CusAppointmentObj = _Util.Facade.CustomerAppoinmentFacade.GetAppointmentById(Id);
            if (CusAppointmentObj != null)
            {
                if (CusAppointmentObj.CompanyId == CurrentUser.CompanyId)
                {
                    CusAppointmentObj.Status = true;
                    CusAppointmentObj.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    CusAppointmentObj.LastUpdatedBy = User.Identity.Name;
                    result = _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinment(CusAppointmentObj);
                }
                Guid cusID = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentidByAppoinmentId(CusAppointmentObj.AppointmentId).CustomerId;
                var LSchedule = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(cusID);
                if (LSchedule != null)
                {
                    var getSchedule = _Util.Facade.ScheduleFacade.GetServiceScheduleByCustomerid(LSchedule.Id);
                    if (getSchedule.Id > 0)
                    {
                        DateTime ScheduleStartDateandTime;
                        DateTime ScheduleEndDateandTime;
                        DateTime Servicedate = CusAppointmentObj.AppointmentDate.Value;
                        DateTime ServiceStartTime = Convert.ToDateTime(CusAppointmentObj.AppointmentStartTime);
                        DateTime ServiceEndTime = Convert.ToDateTime(CusAppointmentObj.AppointmentEndTime);
                        ScheduleStartDateandTime = Servicedate.Date.Add(ServiceStartTime.TimeOfDay);
                        ScheduleEndDateandTime = Servicedate.Date.Add(ServiceEndTime.TimeOfDay);
                        var objAppointmentCustomerId = _Util.Facade.CustomerAppoinmentFacade.GetCustomerIdByCustomerAppointmentCustomerId(CusAppointmentObj.CustomerId).Id;
                        var objEmployeeName = _Util.Facade.CustomerAppoinmentFacade.GteEmployeeNameByCustomerAppointmentEmployeeId(CusAppointmentObj.EmployeeId).EMPName;
                        Schedule ServiceOrderSchedule = new Schedule()
                        {
                            Id = getSchedule.Id,
                            CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value,
                            Type = CusAppointmentObj.AppointmentType,
                            StartDate = ScheduleStartDateandTime,
                            EndDate = ScheduleEndDateandTime,
                            IsCompleted = true,
                            Title = CusAppointmentObj.AppointmentType + " required for " + objEmployeeName,
                            LeadId = Convert.ToInt32(objAppointmentCustomerId),
                            IsFullDay = true,
                            Identifier = CusAppointmentObj.EmployeeId.ToString()
                        };
                        _Util.Facade.ScheduleFacade.UpdateSchedule(ServiceOrderSchedule);
                    }
                }
                double ttotalamount = 0.0;
                string DetailInvoiceId = ""; 
                Invoice objServiceOrderInvoice = new Invoice();
                var customerobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CusAppointmentObj.CustomerId);
                 
                 
                var objAppointmentEquipment = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipmentListByAppointmentId(CusAppointmentObj.AppointmentId);
                var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
                if (objAppointmentEquipment.Count > 0)
                {
                    objServiceOrderInvoice.InvoiceId = Id.GenerateInvoiceNo();
                    objServiceOrderInvoice.CustomerId = CusAppointmentObj.CustomerId;
                    objServiceOrderInvoice.CompanyId = CurrentUser.CompanyId.Value;
                    objServiceOrderInvoice.Amount = CusAppointmentObj.TotalAmount.Value;
                    objServiceOrderInvoice.TotalAmount = CusAppointmentObj.TotalAmountTax;
                    objServiceOrderInvoice.Status = "Open";
                    objServiceOrderInvoice.InvoiceDate = DateTime.Now.UTCCurrentTime();
                    objServiceOrderInvoice.CreatedDate = DateTime.Now.UTCCurrentTime();
                    objServiceOrderInvoice.CreatedBy = EmpMail;
                    objServiceOrderInvoice.IsEstimate = false;
                    objServiceOrderInvoice.DueDate = DateTime.Now.UTCCurrentTime();
                    objServiceOrderInvoice.ShippingDate = DateTime.Now.UTCCurrentTime();
                    objServiceOrderInvoice.BalanceDue = CusAppointmentObj.TotalAmountTax;
                    objServiceOrderInvoice.BillingAddress = AddressHelper.MakeCustomerAddress(customerobj, AddressHelper.BillingAddress, AddressTemplate);
                    objServiceOrderInvoice.ShippingAddress = AddressHelper.MakeCustomerAddress(customerobj, AddressHelper.ShippingAddress, AddressTemplate);
                    objServiceOrderInvoice.Tax = CusAppointmentObj.TaxTotal;
                    objServiceOrderInvoice.CreatedByUid = CurrentUser.UserId;
                    objServiceOrderInvoice.LastUpdatedByUid = CurrentUser.UserId;
                    objServiceOrderInvoice.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    objServiceOrderInvoice.Description = "Auto Generated from Service Order #" + CusAppointmentObj.Id + " (Completion Date: " + DateTime.Now.UTCCurrentTime().ToString("MM/dd/yyyy") + ")";
                    if (CusAppointmentObj.TaxTotal != 0)
                    {
                        objServiceOrderInvoice.TaxType = "Sales-Tax";
                    }
                    else
                    {
                        objServiceOrderInvoice.TaxType = "Non-Tax";
                    }
                    objServiceOrderInvoice.InvoiceFor = "Service Order";
                    invoiceresult = _Util.Facade.InvoiceFacade.InsertInvoice(objServiceOrderInvoice) > 0;
                    if (invoiceresult == true)
                    {
                        if(objServiceOrderInvoice.Id > 0)
                        {
                            objServiceOrderInvoice.InvoiceId =  objServiceOrderInvoice.Id.GenerateInvoiceNo();
                            DetailInvoiceId = objServiceOrderInvoice.InvoiceId;
                            invoiceresult = _Util.Facade.InvoiceFacade.UpdateInvoice(objServiceOrderInvoice);
                        }
                    }
                    foreach(var item1 in objAppointmentEquipment)
                    {
                        InvoiceDetail objInvoiceDetail = new InvoiceDetail()
                        {
                            InvoiceId = objServiceOrderInvoice.InvoiceId,
                            InventoryId = new Guid("00000000-0000-0000-0000-000000000000"),
                            EquipmentId = item1.EquipmentId,
                            CompanyId = CurrentUser.CompanyId.Value,
                            Quantity = item1.Quantity,
                            UnitPrice = item1.UnitPrice,
                            TotalPrice = item1.TotalPrice,
                            EquipDetail = "Auto Generated from Service Order #" + CusAppointmentObj.Id + " (Completion Date: " + DateTime.Now.UTCCurrentTime().ToString("MM/dd/yyyy") + ")",
                            //EquipmentDescription = "Auto Generated from Service Order #" + CusAppointmentObj.Id + " (Completion Date: " + DateTime.Now.UTCCurrentTime().ToString("MM/dd/yyyy") + ")"
                        };
                        invoiceresult = _Util.Facade.InvoiceFacade.InsertInvoiceDetails(objInvoiceDetail) > 0;
                    }
                }
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult ServiceOrderCompleteMail(string ServiceCusName, string ServiceCusMail)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            EmailServicekOrderComplete EmailServicekOrderComplete = new EmailServicekOrderComplete();
            EmailServicekOrderComplete.Name = ServiceCusName;
            EmailServicekOrderComplete.ToEmail = ServiceCusMail;
            _Util.Facade.MailFacade.EmailToCustomerServiceOrderComplete(EmailServicekOrderComplete, CurrentLoggedInUser.CompanyId.Value);
            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult ServiceOrderInformationSendMail(Guid CustomerId, Guid employeeid, Guid appointmentid)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ServiceOrderInformationSendEMail ServiceOrderInformationSendEMail = new ServiceOrderInformationSendEMail();
            string UserMail = "";
            string employeeName = "";
            var EmployeeDetail = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(employeeid);
            if (EmployeeDetail != null)
            {
                UserMail = EmployeeDetail.UserName;
                employeeName = EmployeeDetail.FirstName + " " + EmployeeDetail.LastName;
            }
            string ServiceOrderTable = "";
            string ServiceOrderTableHeader = "";
            string TableValues = "";
            var equipmentlist = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(appointmentid);
            if (equipmentlist != null)
            {
                ServiceOrderTableHeader = @"<table style='width:100%; margin-top:10px'>
                                            <tr>
                                                <td>ServiceName</td>
                                                <td>ServiceQuantity</td>
                                                <td>ServiceUnitPrice</td>
                                                <td>ServiceTotalprice</td>
                                            </tr>
                                            {0}
                                        </table>";
                foreach (var item in equipmentlist)
                {
                    TableValues += @"<tr>
                                        <td>" + item.EquipmentServiceName + @"</td>
                                        <td>" + item.Quantity + @"</td>
                                        <td>" + item.UnitPrice + @"</td>
                                        <td>" + item.TotalPrice + @"</td>
                                    </tr>";
                }
                ServiceOrderTable = string.Format(ServiceOrderTableHeader, TableValues);
            }
            string customername = "";
            var CustomerNameObj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (CustomerNameObj != null)
            {
                if (CustomerNameObj.MiddleName != "")
                {
                    customername = CustomerNameObj.FirstName + " " + CustomerNameObj.MiddleName + " " + CustomerNameObj.LastName;
                }
                else
                {
                    customername = CustomerNameObj.FirstName + " " + CustomerNameObj.LastName;
                }

            }

            ServiceOrderInformationSendEMail.Name = employeeName;
            ServiceOrderInformationSendEMail.ToEmail = UserMail;
            ServiceOrderInformationSendEMail.EmailBody = customername;
            ServiceOrderInformationSendEMail.ServiceList = ServiceOrderTable;
            _Util.Facade.MailFacade.EmailToEmployeeServiceOrderSendMail(ServiceOrderInformationSendEMail, CurrentLoggedInUser.CompanyId.Value);
            var objcust = _Util.Facade.CustomerAppoinmentFacade.GetServiceCustomerIdByAppointmentIdAndAppointmentType(appointmentid);
            if (objcust != null)
            {
                string empName = "";
                var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
                if (empobj != null)
                {
                    empName = empobj.FirstName + " " + empobj.LastName;
                }
                CustomerSnapshot objSendMailInfo = new CustomerSnapshot()
                {
                    CustomerId = objcust.CustomerId,
                    CompanyId = CurrentLoggedInUser.CompanyId.Value,
                    Description = "Service Order email sent by " + "<b>" + empName + "</b>",
                    Logdate = DateTime.Now.UTCCurrentTime(),
                    Updatedby = CurrentLoggedInUser.Identity.Name,
                    Type = "CustomerMailHistory"
                };
                _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objSendMailInfo);
            }
            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult serviceOrderCreateMail(Guid AppointmentId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (CurrentLoggedInUser != null)
            {

                var ServiceOrderDetails = _Util.Facade.CustomerAppoinmentFacade.GetServiceOrderMailDetailsByAppointmentIdAndCompanyId(AppointmentId, CurrentLoggedInUser.CompanyId.Value);
                if (ServiceOrderDetails != null)
                {
                    EmailCreateServiceOrder EmailCreateServiceOrder = new EmailCreateServiceOrder();
                    EmailCreateServiceOrder.EmployeeName = ServiceOrderDetails.EmployeeName;
                    EmailCreateServiceOrder.CustomerName = ServiceOrderDetails.CustomerFirstName + " " + ServiceOrderDetails.CustomerLastName;
                    if (ServiceOrderDetails.CustomerMiddleName != "")
                    {
                        EmailCreateServiceOrder.CustomerName = ServiceOrderDetails.CustomerFirstName + " " + ServiceOrderDetails.CustomerMiddleName + " " + ServiceOrderDetails.CustomerLastName;
                    }
                    EmailCreateServiceOrder.AppointmentDate = ServiceOrderDetails.AppointmentDate;
                    EmailCreateServiceOrder.AppointmentStartTime = ServiceOrderDetails.AppointmentStartTime;
                    EmailCreateServiceOrder.AppointmentEndTime = ServiceOrderDetails.AppointmentEndTime;
                    EmailCreateServiceOrder.Notes = ServiceOrderDetails.Notes;
                    EmailCreateServiceOrder.CreatedBy = ServiceOrderDetails.CreatedBy;
                    EmailCreateServiceOrder.CreatedDate = ServiceOrderDetails.CreatedDate;
                    EmailCreateServiceOrder.ToEmail = ServiceOrderDetails.EmployeeEmail;

                    //_Util.Facade.MailFacade.EmailToEmployeeCreateServiceOrder(EmailCreateServiceOrder, CurrentLoggedInUser.CompanyId.Value);
                    //var custid = _Util.Facade.CustomerAppoinmentFacade.GetServiceCustomerIdByAppointmentIdAndAppointmentType(AppointmentId);
                    //if (custid != null)
                    //{
                    //    CustomerSnapshot objCreateServiceOrderMail = new CustomerSnapshot()
                    //    {
                    //        CustomerId = custid.CustomerId,
                    //        CompanyId = CurrentLoggedInUser.CompanyId.Value,
                    //        Logdate = DateTime.Now.UTCCurrentTime(),
                    //        Description = "Service order create send mail to " + EmailCreateServiceOrder.EmployeeName,
                    //        Updatedby = CurrentLoggedInUser.Identity.Name,
                    //        Type = "CustomerMailHistory"
                    //    };
                    //    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objCreateServiceOrderMail);
                    //}
                }
            }
            return Json(result);
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
    }
}