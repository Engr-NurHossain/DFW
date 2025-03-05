using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.Helper;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class WorkOrderController : BaseController
    {
        // GET: WorkOrder
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
        public ActionResult WorkOrderPartial(Guid? customerid)
        {

            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerWorkOrderList ))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            } 
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<CustomerAppointment> CustomerAppoint = new List<CustomerAppointment>();
            if (customerid.HasValue)
            {
                bool res = _Util.Facade.CustomerFacade.CustomerIsInCompanySalesPartial(customerid.Value, currentLoggedIn.CompanyId.Value);
                if (!res)
                {
                    return null;
                }
                Customer tmpCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid.Value);

                Guid employeeId = currentLoggedIn.UserId;
                //bool isSales = true; 

                //List<CustomerAppointment> CustomerAppoint = _Util.Facade.CustomerAppoinmentFacade.GetAllWorkOrderAppoinmentByEmployeeId(tmpCustomer.CustomerId, currentLoggedIn.CompanyId.Value, employeeId);
                CustomerAppoint = _Util.Facade.CustomerAppoinmentFacade.GetAllWorkOrderAppoinmentByCustomerIdAndCompanyId(tmpCustomer.CustomerId, currentLoggedIn.CompanyId.Value);
            }
            var count = CustomerAppoint.Count();
            if (count > 0)
            {
                return PartialView("_WorkOrderPartial", CustomerAppoint);
            }
            else
            {
                return PartialView("_WorkOrderCalendar", CustomerAppoint);
            }
        }
        public ActionResult TopToBottomWorkOrder(Guid AppointmentId, Guid CustomerId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            AddCustomerAppointmentWorkOrder model = new AddCustomerAppointmentWorkOrder();

            if (!currentLoggedIn.CompanyId.HasValue)
            {
                return PartialView(false);
            }

            if (AppointmentId != null && CustomerId != null)
            {
                var IsAppointmentEquipmentExist = _Util.Facade.CustomerAppoinmentFacade.IsAppointmentEquipmentExistCheck(AppointmentId);

                if (IsAppointmentEquipmentExist.Count > 0)
                {
                    model.CustomerAppointmentEquipmentList = IsAppointmentEquipmentExist;
                    model.CustomerAppointment = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentinfoByAppointmentIdandCustomerIdandCompanyId(AppointmentId, CustomerId, currentLoggedIn.CompanyId.Value);
                    var customerObjectCustomerId = model.CustomerAppointment.CustomerId;
                    var customerObject = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerObjectCustomerId);
                    if (customerObject != null)
                    {
                        model.WorkOrderCustomerName = customerObject.FirstName + " " + customerObject.LastName;
                        model.WorkOrderCustomerPhone = customerObject.PrimaryPhone;
                        model.WorkOrderCustomerEmail = customerObject.EmailAddress;
                        model.WorkOrderCustomerAddress = customerObject.Address;
                        model.WorkOrderCustomerCity = customerObject.City;
                        model.WorkOrderCustomerState = customerObject.State;
                        model.WorkOrderCustomerSteet = customerObject.Street;
                        model.WorkOrderCustomerZipCode = customerObject.ZipCode;

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
                        model.WorkOrderCustomerName = customerObject.FirstName + " " + customerObject.LastName;
                        model.WorkOrderCustomerPhone = customerObject.PrimaryPhone;
                        model.WorkOrderCustomerEmail = customerObject.EmailAddress;
                        model.WorkOrderCustomerAddress = customerObject.Address;
                        model.WorkOrderCustomerCity = customerObject.City;
                        model.WorkOrderCustomerState = customerObject.State;
                        model.WorkOrderCustomerSteet = customerObject.Street;
                        model.WorkOrderCustomerZipCode = customerObject.ZipCode;
                    }
                    var employeObjectCustomerId = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.CustomerAppointment.EmployeeId);
                    //var employeObject = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(employeObjectCustomerId);
                    if (employeObjectCustomerId != null)
                    {
                        model.WorkOrderEmployeeName = employeObjectCustomerId.FirstName + " " + employeObjectCustomerId.LastName;
                    }
                    model.ListCustomerAppointmentTechnician = _Util.Facade.CustomerAppoinmentFacade.GetAllAppointmentTechnicianByAppointmentId(model.CustomerAppointment.Id);
                }

                //ViewBag.WorkInstallerList = _Util.Facade.EmployeeFacade.GetAllWorkPersonEmployee(currentLoggedIn.CompanyId.Value).Select(x =>
                //  new SelectListItem()
                //  {
                //      Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                //      Value = x.EmployeeId.ToString()
                //  }).ToList();
                ViewBag.WorkInstallerList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Installer, new Guid()).Select(x =>
                  new SelectListItem()
                  {
                      Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                      Value = x.UserId.ToString()
                  }).ToList();



                ViewBag.AppointmentTime = _Util.Facade.LookupFacade.GetLookupByKey("WOAppointmentTime").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            }
            List<SelectListItem> EmployeeList = new List<SelectListItem>();
            EmployeeList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "00000000-0000-0000-0000-000000000000"
            });
            EmployeeList.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.QA, new Guid()).Select(x =>
                      new SelectListItem()
                      {
                          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                          Value = x.UserId.ToString()
                      }).ToList());
            ViewBag.QAList = EmployeeList.OrderBy(x => x.Text != "Please Select").ThenBy( x => x.Text).ToList();

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
                if (model.CustomerAppointment != null)
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
            ViewBag.AppointmentTime = _Util.Facade.LookupFacade.GetLookupByKey("Arrival").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            ViewBag.Technician = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid()).Select(x =>
                  new SelectListItem()
                  {
                      Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                      Value = x.UserId.ToString()
                  }).ToList();
            return PartialView("_TopToBottomWorkOrder", model);
        }
        [Authorize]
        public ActionResult AddWorkOrder(int? id, int customerid, int? Date, int? Month, int? Year)
        {
            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerWorkOrderAdd))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            CustomerAppointment model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (Date.HasValue && Month.HasValue && Year.HasValue)
            {
                //ViewBag.SelectedDate= Date.ToString() + "-" + Month.ToString() + "-" + Year.ToString();
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
            //ViewBag.WorkList = _Util.Facade.EmployeeFacade.GetAllWorkPersonEmployee(currentLoggedIn.CompanyId.Value).Select(x =>
            //      new SelectListItem()
            //      {
            //          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
            //          Value = x.EmployeeId.ToString()
            //      }).ToList();

            ViewBag.WorkList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Installer, new Guid()).Select(x =>
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
            Guid cusid = Guid.Parse(ViewBag.CustomerId);
            ViewBag.CusName = _Util.Facade.CustomerAppoinmentFacade.GetCustomerNameByCompanyIdandCustomerId(currentLoggedIn.CompanyId.Value, cusid);
            return PartialView("_AddWorkOrder", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddWorkOrder(CustomerAppointment ca, CustomerAppointmentTechnician cat)
        {
            bool result = false;
            string message1 = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            //ca.EmployeeId = ((HS.Web.UI.Helper.CustomPrincipal)(User)).EmployeeId.Value;
            ca.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value;
            ca.AppointmentType = AppoinmentType.WorkOrder;
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


                //result = _Util.Facade.CustomerSnapshotFacade.UpdateSnapshot(Snapshotobj);
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
                        if (applist.Count == 1)
                        {
                            foreach (var item in applist)
                            {
                                if (ca.AppointmentId == item.AppointmentId)
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
                            if (applist1.Count > 0)
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
                    DateTime Workedate = ca.AppointmentDate.Value;
                    DateTime WorkStartTime = Convert.ToDateTime(ca.AppointmentStartTime);
                    DateTime WorkEndTime = Convert.ToDateTime(ca.AppointmentEndTime);
                    ScheduleStartDateandTime = Workedate.Date.Add(WorkStartTime.TimeOfDay);
                    ScheduleEndDateandTime = Workedate.Date.Add(WorkEndTime.TimeOfDay);
                    var objAppointmentCustomerId = _Util.Facade.CustomerAppoinmentFacade.GetCustomerIdByCustomerAppointmentCustomerId(ca.CustomerId).Id;
                    var objEmployeeName = _Util.Facade.CustomerAppoinmentFacade.GteEmployeeNameByCustomerAppointmentEmployeeId(ca.EmployeeId).EMPName;
                    Schedule WorkOrderSchedule = new Schedule()
                    {
                        CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value,
                        Type = ca.AppointmentType,
                        StartDate = Convert.ToDateTime(ScheduleStartDateandTime),
                        EndDate = Convert.ToDateTime(ScheduleEndDateandTime),
                        IsCompleted = false,
                        Title = ca.AppointmentType + " required for " + objEmployeeName,
                        LeadId = Convert.ToInt32(objAppointmentCustomerId),
                        IsFullDay = true,
                        Identifier = ca.EmployeeId.ToString()
                    };
                    _Util.Facade.ScheduleFacade.InsertSchedule(WorkOrderSchedule);
                    if (cat.ListHelperTech.Count > 0)
                    {
                        foreach (var item in cat.ListHelperTech)
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
                var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                if(objemp != null)
                {
                    CustomerSnapshot ObjCusWorkOrderHistorySnapshot = new CustomerSnapshot
                    {
                        CompanyId = ca.CompanyId,
                        CustomerId = ca.CustomerId,
                        Description =   string.Format("<a onclick=OpenTopToBottomModal('{2}/WorkOrder/TopToBottomWorkOrder?AppointmentId={0}&CustomerId={1}') style='cursor: pointer;'>", ca.AppointmentId, ca.CustomerId, AppConfig.DomainSitePath) + "<b>" + "Workorder#" + ca.Id + "</b>" + "</a>",
                        Type = "WorkOrderCreated",
                        Logdate = DateTime.Now.UTCCurrentTime(),
                        Updatedby = objemp.FirstName + " " + objemp.LastName
                    };
                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(ObjCusWorkOrderHistorySnapshot);
                }
            }

            return Json(new { result = result, CustomerId = ca.CustomerId, AppointmentId = ca.AppointmentId, message = "Invalid Parameter", message1 = message1 });
        }
        [Authorize]
        [HttpPost]
        public JsonResult WorkOrderCreateMail(Guid AppointmentId)
        {

            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (CurrentLoggedInUser != null)
            {

                var WorkOrderDetails = _Util.Facade.CustomerAppoinmentFacade.GetServiceOrderMailDetailsByAppointmentIdAndCompanyId(AppointmentId, CurrentLoggedInUser.CompanyId.Value);
                if (WorkOrderDetails != null)
                {
                    EmailCreateWorkOrder EmailCreateWorkOrder = new EmailCreateWorkOrder();

                    EmailCreateWorkOrder.EmployeeName = WorkOrderDetails.EmployeeName;
                    EmailCreateWorkOrder.CustomerName = WorkOrderDetails.CustomerFirstName + " " + WorkOrderDetails.CustomerLastName;
                    if (WorkOrderDetails.CustomerMiddleName != "")
                    {
                        EmailCreateWorkOrder.CustomerName = WorkOrderDetails.CustomerFirstName + " " + WorkOrderDetails.CustomerMiddleName + " " + WorkOrderDetails.CustomerLastName;
                    }
                    EmailCreateWorkOrder.AppointmentDate = WorkOrderDetails.AppointmentDate;
                    EmailCreateWorkOrder.AppointmentStartTime = WorkOrderDetails.AppointmentStartTime;
                    EmailCreateWorkOrder.AppointmentEndTime = WorkOrderDetails.AppointmentEndTime;
                    EmailCreateWorkOrder.Notes = WorkOrderDetails.Notes;
                    EmailCreateWorkOrder.CreatedBy = WorkOrderDetails.CreatedBy;
                    EmailCreateWorkOrder.CreatedDate = WorkOrderDetails.CreatedDate;
                    EmailCreateWorkOrder.ToEmail = WorkOrderDetails.EmployeeEmail;

                    //_Util.Facade.MailFacade.EmailToEmployeeCreateWorkOrder(EmailCreateWorkOrder, CurrentLoggedInUser.CompanyId.Value);
                    //var custid = _Util.Facade.CustomerAppoinmentFacade.GetCustomerIdByAppointmentIdAndAppointmentType(AppointmentId);
                    //if(custid != null)
                    //{
                    //    CustomerSnapshot objCreateWorkOrderMail = new CustomerSnapshot()
                    //    {
                    //        CustomerId = custid.CustomerId,
                    //        CompanyId = CurrentLoggedInUser.CompanyId.Value,
                    //        Logdate = DateTime.Now.UTCCurrentTime(),
                    //        Description = "Workorder create send mail to " + EmailCreateWorkOrder.EmployeeName,
                    //        Updatedby = CurrentLoggedInUser.Identity.Name,
                    //        Type = "CustomerMailHistory"
                    //    };
                    //    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objCreateWorkOrderMail);
                    //}

                }
            }
            return Json(result);
        }
        public ActionResult WorkOrderCalendar()
        {
            return PartialView("_CustomerAppoinments");
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddCustomerAppointmentDetailWorkOrder(AddCustomerAppointmentWorkOrder AddCustomerAppointmentWorkOrder, Guid AppointmentIdForAddProduct, Guid WorkOrderEmployeeId, DateTime WorkOrderDate, string WorkOrderStartTime, string WorkOrderEndTime, string WorkOrderNote, string InstallTypeWorkOrder, string ServiceOrderTaxPercent, string ServiceOrderTaxTotal, string ServiceOrderTotalAmount, string ServiceOrderTotalAmountTax, string WorkOrderTaxType, List<string> ListHelperTech)
        {
            //AddCustomerAppointment model;
            var result = false;
            bool res1 = false;
            string message1 = "";
            Guid cusID = new Guid();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            int appid = 0;
            if (currentLoggedIn == null)
            {
                return Json(result);
            }

            if (currentLoggedIn != null)
            {
                var DeletePreviousEquipmentList = _Util.Facade.CustomerAppoinmentFacade.IsAppointmentEquipmentExistCheck(AppointmentIdForAddProduct);
                if (DeletePreviousEquipmentList != null)
                {
                    result = _Util.Facade.CustomerAppoinmentFacade.DeletePreviousCustomerDetailsEquiptmentByEquipmentId(DeletePreviousEquipmentList);
                }

                if (AppointmentIdForAddProduct != new Guid())
                {
                    var WorkOrderCustomerAppointmentObject = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentObjectByAppointmentId(AppointmentIdForAddProduct);
                    if (WorkOrderCustomerAppointmentObject != null)
                    {
                        var userobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(WorkOrderEmployeeId);
                        if (userobj != null)
                        {
                            var applist = _Util.Facade.ScheduleFacade.GetAppointmentScheduleByAppointmentStartAndEndDatetimeAndEmployeeid(WorkOrderDate.ToString(), WorkOrderStartTime, WorkOrderEmployeeId, WorkOrderEndTime);
                            var applist1 = _Util.Facade.ScheduleFacade.GetAppointmentScheduleByAppointmentDatetimeAndEmployeeid(WorkOrderDate.ToString(), WorkOrderStartTime, WorkOrderEmployeeId, WorkOrderEndTime);
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
                                            WorkOrderCustomerAppointmentObject.AppointmentDate = WorkOrderDate.SetZeroHour();
                                            WorkOrderCustomerAppointmentObject.AppointmentEndTime = WorkOrderEndTime;
                                            WorkOrderCustomerAppointmentObject.AppointmentStartTime = WorkOrderStartTime;
                                            WorkOrderCustomerAppointmentObject.EmployeeId = WorkOrderEmployeeId;
                                            WorkOrderCustomerAppointmentObject.Notes = WorkOrderNote;
                                            WorkOrderCustomerAppointmentObject.TaxPercent = Convert.ToDouble(ServiceOrderTaxPercent);
                                            WorkOrderCustomerAppointmentObject.TaxType = WorkOrderTaxType;
                                            WorkOrderCustomerAppointmentObject.TaxTotal = Convert.ToDouble(ServiceOrderTaxTotal);
                                            WorkOrderCustomerAppointmentObject.TotalAmount = Convert.ToDouble(ServiceOrderTotalAmount);
                                            WorkOrderCustomerAppointmentObject.TotalAmountTax = Convert.ToDouble(ServiceOrderTotalAmountTax);

                                            result = _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppointmentInformation(WorkOrderCustomerAppointmentObject);
                                            appid = WorkOrderCustomerAppointmentObject.Id;
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
                                        WorkOrderCustomerAppointmentObject.AppointmentDate = WorkOrderDate.SetZeroHour();
                                        WorkOrderCustomerAppointmentObject.AppointmentEndTime = WorkOrderEndTime;
                                        WorkOrderCustomerAppointmentObject.AppointmentStartTime = WorkOrderStartTime;
                                        WorkOrderCustomerAppointmentObject.EmployeeId = WorkOrderEmployeeId;
                                        WorkOrderCustomerAppointmentObject.Notes = WorkOrderNote;
                                        WorkOrderCustomerAppointmentObject.TaxPercent = Convert.ToDouble(ServiceOrderTaxPercent);
                                        WorkOrderCustomerAppointmentObject.TaxType = WorkOrderTaxType;
                                        WorkOrderCustomerAppointmentObject.TaxTotal = Convert.ToDouble(ServiceOrderTaxTotal);
                                        WorkOrderCustomerAppointmentObject.TotalAmount = Convert.ToDouble(ServiceOrderTotalAmount);
                                        WorkOrderCustomerAppointmentObject.TotalAmountTax = Convert.ToDouble(ServiceOrderTotalAmountTax);

                                        result = _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppointmentInformation(WorkOrderCustomerAppointmentObject);
                                        appid = WorkOrderCustomerAppointmentObject.Id;
                                    }
                                }
                            }
                        }
                    }
                }

                List<CustomerAppointmentEquipment> objCustomerAppointmentEquipment = new List<CustomerAppointmentEquipment>();

                foreach (var items in AddCustomerAppointmentWorkOrder.CustomerAppointmentEquipmentList)
                {
                    items.AppointmentId = AppointmentIdForAddProduct;
                    items.CreatedBy = User.Identity.Name;
                    items.CreatedDate = DateTime.Now.UTCCurrentTime();
                }
                objCustomerAppointmentEquipment = AddCustomerAppointmentWorkOrder.CustomerAppointmentEquipmentList;
                if (objCustomerAppointmentEquipment.Count > 0)
                {
                    result = _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppointmentEquipmentDetails(objCustomerAppointmentEquipment);
                }
                 
                cusID = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentidByAppoinmentId(AppointmentIdForAddProduct).CustomerId;

                #region No need Schedule anymore
                //var LSchedule = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(cusID);

                //if (LSchedule != null)
                //{
                //    var getSchedule = _Util.Facade.ScheduleFacade.GetScheduleByCustomerid(LSchedule.Id);
                //    DateTime Workedate = WorkOrderDate;
                //    DateTime WorkStartTime = Convert.ToDateTime(WorkOrderStartTime);
                //    DateTime WorkEndTime = Convert.ToDateTime(WorkOrderEndTime);
                //    var ScheduleStartDateandTime1 = Workedate.Date.Add(WorkStartTime.TimeOfDay);
                //    var ScheduleEndDateandTime1 = Workedate.Date.Add(WorkEndTime.TimeOfDay);
                //    var objAppointmentCustomerId = _Util.Facade.CustomerAppoinmentFacade.GetCustomerIdByCustomerAppointmentCustomerId(cusID).Id;
                //    var objEmployeeName = _Util.Facade.CustomerAppoinmentFacade.GteEmployeeNameByCustomerAppointmentEmployeeId(WorkOrderEmployeeId).EMPName;
                //    if (getSchedule != null)
                //    {
                //        if (getSchedule.Id > 0)
                //        {
                //            Schedule objWorkOrderSchedule = new Schedule()
                //            {
                //                Id = getSchedule.Id,
                //                CompanyId = getSchedule.CompanyId,
                //                Type = getSchedule.Type,
                //                StartDate = ScheduleStartDateandTime1,
                //                EndDate = ScheduleEndDateandTime1,
                //                Title = getSchedule.Type + " Required for " + objEmployeeName,
                //                IsCompleted = false,
                //                LeadId = Convert.ToInt32(objAppointmentCustomerId),
                //                IsFullDay = true,
                //                Identifier = WorkOrderEmployeeId.ToString()
                //            };
                //            result = _Util.Facade.ScheduleFacade.UpdateSchedule(objWorkOrderSchedule);
                //        }
                //    }

                //}
                #endregion

                #region Helper Tech 
                _Util.Facade.CustomerAppoinmentFacade.DeleteAppointmentAllTech(appid);
                if (ListHelperTech.Count > 0)
                {
                    foreach (var item1 in ListHelperTech)
                    {
                        CustomerAppointmentTechnician objapptech = new CustomerAppointmentTechnician()
                        {
                            EmployeeId = new Guid(item1),
                            CustomerAppointmentId = appid
                        };
                        _Util.Facade.CustomerAppoinmentFacade.InsertAppointmentTechnician(objapptech);
                    } 
                }
                #endregion

                //else
                //{
                //    var objtech = _Util.Facade.CustomerAppoinmentFacade.GetAllAppointmentTechnicianByAppointmentId(appid);
                //    if (objtech.Count > 0)
                //    {
                //        foreach (var item in objtech)
                //        {
                //            _Util.Facade.CustomerAppoinmentFacade.DeleteAppointmentTech(item.Id);
                //            res1 = true;
                //        }
                //    }
                //}
                return Json(new { result = result, cusid = cusID, message = "Workorder saved successfully.", message1 = message1 });
                
            }
            else
            {
                return Json(new { result = result, cusid = cusID });
            }
        }
        [Authorize]
        public ActionResult GetWorkOrder(int id)
        {
            AddCustomerAppointment model = new AddCustomerAppointment();
            var CustomerAppointmentObj = _Util.Facade.CustomerAppoinmentFacade.GetAppointmentById(id);
            model.CustomerAppointment = CustomerAppointmentObj;
            return View("GetWorkOrder", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveWorkOrderPdf(int AppointmentId, List<WorkOrderPdfDetail> WorkOrderPdfDetail, string TotalWorkOrderPrice, string WorkOrderTaxPercent, string WorkOrderTaxAmount, string WorkOrderSubTotalAmount)
        {
            AddCustomerAppointmentWorkOrder modelWorkOrderPdf = new AddCustomerAppointmentWorkOrder();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            var CustomerAppointment = _Util.Facade.CustomerAppoinmentFacade.GetAppointmentById(AppointmentId);
            if (CustomerAppointment != null)
            {
                modelWorkOrderPdf.CustomerAppointment = CustomerAppointment;
            }

            var CustomerAppointmentDetail = _Util.Facade.CustomerAppoinmentDetailFacade.GetAppointmentDetailInstallTypeById(AppointmentId);
            if (CustomerAppointmentDetail != null)
            {
                modelWorkOrderPdf.CustomerAppointmentDetail = CustomerAppointmentDetail;
            }

            if (CustomerAppointment.AppointmentId != null)
            {
                Guid GuidAppointmentId = CustomerAppointment.AppointmentId;
                List<CustomerAppointmentEquipment> CustomerAppointmentEquipment = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipmentListByAppointmentId(GuidAppointmentId);
                if (CustomerAppointmentEquipment != null)
                {
                    modelWorkOrderPdf.CustomerAppointmentEquipmentList = CustomerAppointmentEquipment;
                }

                Guid DetailAppointmentId = CustomerAppointment.AppointmentId;
                var CustomerAppointmentDetailInstall = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentDetailListByAppointmentId(DetailAppointmentId);
                if (CustomerAppointmentDetailInstall != null)
                {
                    modelWorkOrderPdf.CustomerAppointmentDetailList = CustomerAppointmentDetailInstall;

                }
                modelWorkOrderPdf.CustomerAppointment = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentDetailinfoByAppointmentId(CustomerAppointment.AppointmentId);

                var ObjInstallType = _Util.Facade.CustomerAppoinmentDetailFacade.GetInstallTypeNameByAppointmentID(modelWorkOrderPdf.CustomerAppointment.AppointmentId);
                if (ObjInstallType != null)
                {
                    modelWorkOrderPdf.WorkOrderInstallType = ObjInstallType.InstallType;
                }

                modelWorkOrderPdf.CustomerAppointment = _Util.Facade.CustomerAppoinmentFacade.GetCustomerAppointmentinfoByAppointmentIdandCustomerIdandCompanyId(CustomerAppointment.AppointmentId, CustomerAppointment.CustomerId, currentLoggedIn.CompanyId.Value);
                var customerObjectCustomerId = modelWorkOrderPdf.CustomerAppointment.CustomerId;
                var customerObject = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerObjectCustomerId);
                if (customerObject != null)
                {
                    string CustomerAddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(currentLoggedIn.CompanyId.Value);
                    modelWorkOrderPdf.WorkOrderCustomerAddress = Helper.AddressHelper.MakeCustomerAddress(customerObject, AddressHelper.ShippingAddress, CustomerAddressTemplate);
                }
                var employeObjectCustomerId = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(modelWorkOrderPdf.CustomerAppointment.EmployeeId);
                //var employeObject = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(employeObjectCustomerId);
                if (employeObjectCustomerId != null)
                {
                    modelWorkOrderPdf.WorkOrderEmployeeName = employeObjectCustomerId.FirstName + " " + employeObjectCustomerId.LastName;
                }
                if (WorkOrderPdfDetail != null)
                {
                    if (WorkOrderPdfDetail.Count > 0)
                    {
                        modelWorkOrderPdf.WorkOrderPdfDetail = WorkOrderPdfDetail;
                    }
                }

                modelWorkOrderPdf.PriceWorkOrderTotal = TotalWorkOrderPrice;
                if (WorkOrderTaxAmount != HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency())
                {
                    modelWorkOrderPdf.WorkOrderTaxAmount = WorkOrderTaxAmount;
                }
                if (WorkOrderTaxPercent != "0%")
                {
                    modelWorkOrderPdf.WorkOrderTaxPercent = WorkOrderTaxPercent;
                }
                modelWorkOrderPdf.WorkOrderSubTotalAmount = WorkOrderSubTotalAmount;
                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(modelWorkOrderPdf.CustomerAppointment.CompanyId);
                string CompanyAddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(currentLoggedIn.CompanyId.Value);
                modelWorkOrderPdf.WorkOrderCompanyAddress = Helper.AddressHelper.MakeCompanyAddress(tempCom, CompanyAddressTemplate);
                string tempcompanyBranch = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(currentLoggedIn.CompanyId.Value);
                modelWorkOrderPdf.CompanyLogo = tempcompanyBranch;
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("WorkOrderPDF", modelWorkOrderPdf)
                {
                    PageSize = Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                Random rand = new Random();
                string filename = ConfigurationManager.AppSettings["File.WorkOrderFiles"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
                filename = string.Format(filename, comname);
                filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + rand.Next().ToString() + "_" + rand.Next().ToString() + "_WorkOrder.pdf";
                string Serverfilename = FileHelper.GetFileFullPath(filename);
                FileHelper.SaveFile(applicationPDFData, Serverfilename);

                return Json(new { result = true, message = "Work Order Successfully Saved", filePath = AppConfig.DomainSitePath + filename });
            }

            else
            {
                return Json(false);
            }

        }
        [Authorize]
        [HttpPost]
        public JsonResult MakeWorkOrderComplete(int Id)
        {
            bool result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            CustomerAppointment CusAppointmentObj = _Util.Facade.CustomerAppoinmentFacade.GetAppointmentById(Id);
            if (CusAppointmentObj != null)
            {
                if (CusAppointmentObj.CompanyId == currentLoggedIn.CompanyId)
                {
                    CusAppointmentObj.Status = true;
                    CusAppointmentObj.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    CusAppointmentObj.LastUpdatedBy = currentLoggedIn.UserId.ToString();
                    result = _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinment(CusAppointmentObj);
                }
                Guid cusID = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentidByAppoinmentId(CusAppointmentObj.AppointmentId).CustomerId;
                var LSchedule = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(cusID);
                if (LSchedule != null)
                {
                    var getSchedule = _Util.Facade.ScheduleFacade.GetScheduleByCustomerid(LSchedule.Id);
                    if (getSchedule != null)
                    {
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

                }
            }
            return Json(new { result = result, message = "Done" });
        }

        [Authorize]
        [HttpPost]
        public JsonResult WorkOrderCompleteMail(string CusName, string CusMail, Guid CustomerEquipId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            EmailWorkOrderComplete EmailWorkOrderComplete = new EmailWorkOrderComplete();
            EmailWorkOrderComplete.Name = CusName;
            EmailWorkOrderComplete.ToEmail = CusMail;
            if (CustomerEquipId != null)
            {
                var equiplist = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(CustomerEquipId);
                foreach (var item in equiplist)
                {
                    EmailWorkOrderComplete.WorkOrderProductName = item.EquipmentServiceName;
                    EmailWorkOrderComplete.WorkOrderProductQuantity = item.Quantity;
                    EmailWorkOrderComplete.WorkOrderProductUnitPrice = item.UnitPrice;
                    EmailWorkOrderComplete.WorkOrderTotalPrice = item.TotalPrice;
                }
            }
            _Util.Facade.MailFacade.EmailToCustomerWorkOrderComplete(EmailWorkOrderComplete, CurrentLoggedInUser.CompanyId.Value);
            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult WorkOrderInformationSendMail(string customername, Guid employeeid, Guid appointmentid)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            WorkOrderInformationSendEMail WorkOrderInformationSendEMail = new WorkOrderInformationSendEMail();
            string UserMail = "";
            string employeeName = "";
            var EmployeeDetail = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(employeeid);
            if (EmployeeDetail != null)
            {
                UserMail = EmployeeDetail.UserName;
                employeeName = EmployeeDetail.FirstName + " " + EmployeeDetail.LastName;
            }
            var objAppointmentPayment = _Util.Facade.CustomerAppoinmentDetailFacade.GetCustomerAppointmentDetailByAppointmentId(appointmentid);
            if(objAppointmentPayment != null)
            {
                WorkOrderInformationSendEMail.WorkInstallType = objAppointmentPayment.InstallType;
                WorkOrderInformationSendEMail.WorkAmount = objAppointmentPayment.CollectedAmount.Value;
            }
            string WorkOrderTable = "";
            string WorkOrderTableHeader = "";
            string TableValues = "";
            var equipmentlist = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipListByAppointmentId(appointmentid);
            if (equipmentlist != null)
            {

                WorkOrderTableHeader = @"<table style='width:100%; margin-top:10px'>
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
                WorkOrderTable = string.Format(WorkOrderTableHeader, TableValues);
            }

            WorkOrderInformationSendEMail.Name = employeeName;
            WorkOrderInformationSendEMail.ToEmail = UserMail;
            WorkOrderInformationSendEMail.EmailBody = customername;
            WorkOrderInformationSendEMail.ServiceListTable = WorkOrderTable;
            result = _Util.Facade.MailFacade.EmailToEmployeeWorkOrderSendMail(WorkOrderInformationSendEMail, CurrentLoggedInUser.CompanyId.Value);
            var objcust = _Util.Facade.CustomerAppoinmentFacade.GetCustomerIdByAppointmentIdAndAppointmentType(appointmentid);
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
                    Description = "Work Order email sent by " + "<b>" + empName + "</b>",
                    Logdate = DateTime.Now.UTCCurrentTime(),
                    Updatedby = CurrentLoggedInUser.Identity.Name,
                    Type = "CustomerMailHistory"
                };
                _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objSendMailInfo);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]

        public JsonResult CompleteTaskAfterInstallation(Guid customerId, string Zone1, string Zone2, string Zone3, string Zone4, string Zone5, string Zone6, string Zone7, string Zone8, string Zone9, DateTime Installationdate, Guid QA1, DateTime QA1picker, Guid QA2, DateTime QA2picker, Guid WorkOrderEmployeeId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            bool result = false;

            if (CurrentLoggedInUser != null)
            {
                if (customerId != null)
                {
                    var CustomerSystemInfoOldObj = _Util.Facade.CustomerSystemInfoFacade.GetCustomerSystemInfoByCustomerIdAndCompanyId(customerId, CurrentLoggedInUser.CompanyId.Value);
                    if (CustomerSystemInfoOldObj != null)
                    {
                        CustomerSystemInfoOldObj.Zone1 = Zone1;
                        CustomerSystemInfoOldObj.Zone2 = Zone2;
                        CustomerSystemInfoOldObj.Zone3 = Zone3;
                        CustomerSystemInfoOldObj.Zone4 = Zone4;
                        CustomerSystemInfoOldObj.Zone5 = Zone5;
                        CustomerSystemInfoOldObj.Zone6 = Zone6;
                        CustomerSystemInfoOldObj.Zone7 = Zone7;
                        CustomerSystemInfoOldObj.Zone8 = Zone8;
                        CustomerSystemInfoOldObj.Zone9 = Zone9;

                        result = _Util.Facade.CustomerSystemInfoFacade.UpdateSystemInfo(CustomerSystemInfoOldObj);
                    }
                    else
                    {
                        CustomerSystemInfo DBCustomerSystemInfo = new CustomerSystemInfo();

                        DBCustomerSystemInfo.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                        DBCustomerSystemInfo.CustomerId = customerId;
                        DBCustomerSystemInfo.Zone1 = Zone1;
                        DBCustomerSystemInfo.Zone2 = Zone2;
                        DBCustomerSystemInfo.Zone3 = Zone3;
                        DBCustomerSystemInfo.Zone4 = Zone4;
                        DBCustomerSystemInfo.Zone5 = Zone5;
                        DBCustomerSystemInfo.Zone6 = Zone6;
                        DBCustomerSystemInfo.Zone7 = Zone7;
                        DBCustomerSystemInfo.Zone8 = Zone8;
                        DBCustomerSystemInfo.Zone9 = Zone9;

                        result = _Util.Facade.CustomerSystemInfoFacade.InsertSystemInfo(DBCustomerSystemInfo) > 0;
                    }

                    var CustomerObj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerId);
                    if (CustomerObj != null)
                    {

                        var CusAppointmentObj = _Util.Facade.CustomerAppoinmentFacade.GetCustomerWorkOrderbyCustomerIdAndCompanyId(customerId, CurrentLoggedInUser.CompanyId.Value);

                        if (CusAppointmentObj != null)
                        {
                            CustomerObj.CutInDate = Installationdate;
                            CustomerObj.InstallDate = DateTime.Now.UTCCurrentTime();
                            CustomerObj.Installer = CusAppointmentObj.EmployeeId.ToString();
                            CustomerObj.QA1 = QA1.ToString();
                            CustomerObj.QA1Date = QA1picker;
                            CustomerObj.QA2 = QA2.ToString();
                            CustomerObj.QA2Date = QA2picker;
                            CustomerObj.Installer = WorkOrderEmployeeId.ToString();
                            result = _Util.Facade.CustomerFacade.UpdateCustomer(CustomerObj);
                        }
                    }
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