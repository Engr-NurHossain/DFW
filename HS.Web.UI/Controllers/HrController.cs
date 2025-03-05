using AuthorizeNet;
using HS.Entities;
using HS.Facade;
using HS.Framework;
using HS.Web.UI.Helper;
using NLog;
using Org.BouncyCastle.Asn1.Pkcs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class HrController : BaseController
    {
        public HrController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Hr
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult HrHome(Guid UserId)
        {
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(UserId);
            return View(emp);
        }
        [Authorize]
        public ActionResult GetEmployeePtoHoursLog(Guid UserId,string Paytype)
        {
            List<EmployeePTOHourLog> employeePTOList = _Util.Facade.EmployeeFacade.GetAllEmployeePTOHourLogbyUserId(UserId, Paytype);
            ViewBag.EmpPaytype = Paytype;
            return View(employeePTOList);
        }
        #region HrOwn
        [Authorize]
        public PartialViewResult HrHumanRes(Guid userId)
        {

            ViewBag.SelectBasePayData = _Util.Facade.LookupFacade.GetLookupByKey("BasePayData").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            ViewBag.SelectEmployeeTypeData = _Util.Facade.LookupFacade.GetLookupByKey("EmployeeTypeData").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            ViewBag.SelectEmployeePayTypeData = _Util.Facade.LookupFacade.GetLookupByKey("EmployeePayTypeData").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString(), 
                            }).ToList();
            ViewBag.SelectDepartmentData = _Util.Facade.LookupFacade.GetLookupByKey("Department").OrderBy(x => x.DisplayText != "Department").ThenBy(x => x.DisplayText).Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();


            Employee employee = new Employee();
            if (userId != Guid.Empty)
            {
                employee = _Util.Facade.HrFacade.GetEmployeeHumanResByUserId(userId).FirstOrDefault();
                if (employee == null)
                {
                    employee = new Employee();
                }
            }
            List<SelectListItem> SalesCommisssion = new List<SelectListItem>();
            SalesCommisssion.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("CommissionType").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList());
            ViewBag.SalesCommisssion = SalesCommisssion.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            List<SelectListItem> session = new List<SelectListItem>();
            session.Add(new SelectListItem() { Text = "2018", Value = "2018" });
            ViewBag.Session = session;
            List<SelectListItem> TermSheetList = new List<SelectListItem>();
            TermSheetList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            TermSheetList.AddRange(_Util.Facade.PayrollFacade.GetPayrollTermSheetList().OrderBy(x => x.Name).Select(x =>
                new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.TermSheetId.ToString(),
                }).ToList());
            ViewBag.TermSheetList = TermSheetList;
             
            EmployeeAccrualPtoAndApprovePtohourModel model = _Util.Facade.EmployeeFacade.GetEmployeeAccrualPtoAndApprovePtohour(employee.UserId);
            if(model != null)
            { 
                employee.PTOUnassigned = model.TotalPto.TotalPtoHour; 
            }

            return PartialView("_HrHumanRes", employee);
        }
        public double? GetPtoAccrualRate(int Days, string PayType)
        {
            PayType = "Salary";
            double? Rate = 0.0;                 
            EmployeePtoAccrualRate PtoAccrualRate = _Util.Facade.EmployeeFacade.GetEmployeePtoAccrualRate(Days, PayType);

            if (PtoAccrualRate != null)
            {
                if (Days >= PtoAccrualRate.MinimumDay && Days <= PtoAccrualRate.MaximumDay)
                {
                    Rate = PtoAccrualRate.AccrualRate;
                }
            }
            return Rate;
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveHrHumanRes(EmployeePartial employee)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var message = "Employee not save successfully";
            var result = false;
            if (employee != null && employee.UserId != Guid.Empty)
            {
                var employeeModel = _Util.Facade.HrFacade.GetEmployeeHumanResByUserId(employee.UserId).FirstOrDefault();
                if (employeeModel != null)
                {
                    employeeModel.DOB = employee.DOB;
                    employeeModel.HireDate = employee.HireDate;
                    employeeModel.AnniversaryDate = employee.AnniversaryDate;
                    employeeModel.BasePay = employee.BasePay;
                    employeeModel.PayType = employee.PayType;
                    employeeModel.HourlyRate = employee.HourlyRate;
                    employeeModel.EmpType = employee.EmpType;
                    employeeModel.Department = employee.Department;
                    employeeModel.PtoHour = employee.PtoHour;
                    employeeModel.PtoRate = employee.PtoRate;
                    employeeModel.PtoRemain = employee.PtoRemain;
                    employeeModel.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    employeeModel.SalesCommissionStructure = employee.SalesCommissionStructure;
                    employeeModel.Session = employee.Session;
                    employeeModel.UserXComission = employee.UserXComission;
                    employeeModel.IsSalesMatrixUserX = employee.IsSalesMatrixUserX;
                    employeeModel.TermSheetId = employee.TermSheetId;
                    result = _Util.Facade.HrFacade.UpdateEmployeeHumanRes(employeeModel);
                    message = "Employee save successfully";
                }
            }
            return Json(new { result = result, message = message });
        }
        #endregion
        


        #region Insurance
        [Authorize]
        public PartialViewResult HrInsurance(Guid userId)
        {
            ViewBag.SelectInsurance = _Util.Facade.LookupFacade.GetLookupByKey("YesNo").Where(m => m.DataValue != "-1").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            ViewBag.SelectInsurenceRentType = _Util.Facade.LookupFacade.GetLookupByKey("InsurenceRentType").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList();
            ViewBag.InsurancePlans = _Util.Facade.LookupFacade.GetLookupByKey("InsurancePlans").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString(),
                            Disabled = Convert.ToBoolean(x.AlterDisplayText) == true ? Convert.ToBoolean(x.AlterDisplayText) : false
                        }).ToList();
            ViewBag.MedicleInsurancePlans = _Util.Facade.LookupFacade.GetLookupByKey("MedicalInsurance").Select(x =>
                      new SelectListItem()
                      {
                          Text = x.DisplayText.ToString(),
                          Value = x.DataValue.ToString()
                      }).ToList();
            ViewBag.Dental = _Util.Facade.LookupFacade.GetLookupByKey("DentalInsurance").Select(x =>
                   new SelectListItem()
                   {
                       Text = x.DisplayText.ToString(),
                       Value = x.DataValue.ToString()
                   }).ToList();
            ViewBag.Vision = _Util.Facade.LookupFacade.GetLookupByKey("VisionInsurance").Select(x =>
                   new SelectListItem()
                   {
                       Text = x.DisplayText.ToString(),
                       Value = x.DataValue.ToString()
                   }).ToList();

            EmployeeInsurance insurence = new EmployeeInsurance();
            if (userId != Guid.Empty)
            {
                List<EmployeeInsurance> insurenceList = _Util.Facade.HrFacade.GetAllEmployeeInsurance().Where(x => x.UserId == userId).ToList();
                string type = "";
                string subTypeMadical = "";
                string subTypeDental = "";
                string subtypeVision = "";
                foreach (var item in insurenceList)
                {
                    type += item.Type + ',';

                    if (item.Type == "Medical")
                    {
                        ViewBag.MedicalRate = item.InsuranceRate;
                        ViewBag.MedicalInsuranceType = item.RateType;
                        subTypeMadical = item.Subtype;
                    }
                    else if (item.Type == "Dental")
                    {

                        ViewBag.DentalRate = item.InsuranceRate;
                        ViewBag.DentalType = item.RateType;
                        subTypeDental = item.Subtype;
                    }
                    else if (item.Type == "Vision")
                    {

                        ViewBag.VisionRate = item.InsuranceRate;
                        ViewBag.VisionType = item.RateType;
                        subtypeVision = item.Subtype;
                    }
                    else if (item.Type == "VoluntaryLife")
                    {

                        ViewBag.VoluntaryLifeRate = item.InsuranceRate;
                        ViewBag.VoluntaryLifeType = item.RateType;
                    }
                    else if (item.Type == "ShortTermDisability")
                    {

                        ViewBag.ShortTermDisabilityRate = item.InsuranceRate;
                        ViewBag.ShortTermDisabilityType = item.RateType;
                    }
                    else if (item.Type == "LongTermDisability")
                    {

                        ViewBag.LongTermDisabilityRate = item.InsuranceRate;
                        ViewBag.LongTermDisabilityType = item.RateType;
                    }
                    else if (item.Type == "HealthiestYou")
                    {

                        ViewBag.HealthiestYouRate = item.InsuranceRate;
                        ViewBag.HealthiestYouType = item.RateType;
                    }
                }
                ViewBag.Type = type;
                ViewBag.SubTypeMadical = subTypeMadical;
                ViewBag.SubTypeDental = subTypeDental;
                ViewBag.subtypeVision = subtypeVision;
                insurence = _Util.Facade.HrFacade.GetEmployeeInsuranceByUserId(userId).FirstOrDefault();
                if (insurence == null)
                {
                    insurence = new EmployeeInsurance();
                }
            }
            return PartialView("_HrInsurance", insurence);
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveHrInsurance(List<EmployeeInsurance> insurence, Guid EUserId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var message = "Insurance not save successfully";
            var result = false;
            if (insurence != null && insurence.Count > 0)
            {
                var insurenceModel = _Util.Facade.HrFacade.GetAllEmployeeInsurance().Where(x => x.UserId == insurence[0].UserId).ToList();
                foreach (var item in insurence)
                {
                    if (insurenceModel.Count > 0)
                    {
                        foreach (var insurenceItem in insurenceModel)
                        {
                            _Util.Facade.HrFacade.DeleteInsurence(insurenceItem.Id);
                        }
                        if (item.Type == "Medical")
                        {
                            item.Subtype = item.SubTypeMedicle;
                            ViewBag.MedicalRate = item.InsuranceRate;
                            ViewBag.MedicalInsuranceType = item.RateType;
                        }
                        else if (item.Type == "Dental")
                        {
                            item.Subtype = item.SubTypeDental;
                            ViewBag.DentalRate = item.InsuranceRate;
                            ViewBag.DentalType = item.RateType;
                        }
                        else if (item.Type == "Vision")
                        {
                            item.Subtype = item.SubTypeVision;
                            ViewBag.VisionRate = item.InsuranceRate;
                            ViewBag.VisionType = item.RateType;
                        }


                        item.CreatedByUid = item.LastUpdatedByUid = CurrentUser.UserId;
                        item.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        var id = _Util.Facade.HrFacade.InsertEmployeeInsurance(item);
                        result = true;
                        message = "Insurance save successfully";
                    }

                    else
                    {
                        if (item.Type == "Medical")
                        {
                            item.Subtype = item.SubTypeMedicle;
                        }
                        else if (item.Type == "Dental")
                        {
                            item.Subtype = item.SubTypeDental;
                        }
                        else if (item.Type == "Dental")
                        {
                            item.Subtype = item.SubTypeVision;
                        }
                        item.CreatedByUid = item.LastUpdatedByUid = CurrentUser.UserId;
                        item.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        var id = _Util.Facade.HrFacade.InsertEmployeeInsurance(item);
                        if (id > 0)
                        {
                            result = true;
                            message = "Insurance save successfully";
                        }
                    }
                }
            }
            else if (EUserId != null && EUserId != new Guid())
            {

                var insurenceModel = _Util.Facade.HrFacade.GetAllEmployeeInsurance().Where(x => x.UserId == EUserId).ToList();
                foreach (var insurenceItem in insurenceModel)
                {
                    _Util.Facade.HrFacade.DeleteInsurence(insurenceItem.Id);
                    result = true;
                    message = "Insurance save successfully";
                }
            }
            return Json(new { result = result, message = message });
        }



        #endregion

        #region Evaluation
        [Authorize]
        public PartialViewResult HrEvaluation(Guid userId)
        {
            ViewBag.SelectEvaluationType = _Util.Facade.LookupFacade.GetLookupByKey("EvaluationType").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList();

            EmployeeEvaluation evaluation = new EmployeeEvaluation();
            if (userId != Guid.Empty)
            {
                evaluation = _Util.Facade.HrFacade.GetEmployeeEvaluationByUserId(userId).FirstOrDefault();
                if (evaluation == null)
                {
                    evaluation = new EmployeeEvaluation();
                }
            }
            return PartialView("_HrEvaluation", evaluation);
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveHrEvaluation(EmployeeEvaluation evaluation)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var message = "Evaluation not save successfully";
            var result = false;
            if (evaluation != null && evaluation.UserId != Guid.Empty)
            {
                var evaluationModel = _Util.Facade.HrFacade.GetEmployeeEvaluationByUserId(evaluation.UserId).FirstOrDefault();
                if (evaluationModel != null)
                {
                    //evaluationModel.EvaluationDate = evaluation.EvaluationDate;
                    evaluationModel.NextEvaluationDate = evaluation.NextEvaluationDate;
                    evaluationModel.LastEvaluationDate = evaluation.LastEvaluationDate;

                    evaluationModel.EvaluationReminderDate = evaluation.EvaluationReminderDate;
                    evaluationModel.EvaluationType = evaluation.EvaluationType;
                    evaluationModel.LastUpdatedByUid = CurrentUser.UserId;
                    evaluationModel.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    result = _Util.Facade.HrFacade.UpdateEmployeeEvaluation(evaluationModel);
                    if (result)
                        message = "Saved successfully";
                }
                else
                {
                    evaluation.CreatedByUid = evaluation.LastUpdatedByUid = CurrentUser.UserId;
                    evaluation.CreatedDate = evaluation.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    var id = _Util.Facade.HrFacade.InsertEmployeeEvaluation(evaluation);
                    if (id > 0)
                    {
                        result = true;
                        message = "Evaluation save successfully";
                    }
                }
            }
            return Json(new { result = result, message = message });
        }
        #endregion

        #region Ocurance
        [Authorize]
        public PartialViewResult HrOccurance(Guid userId)
        {
            List<EmployeeOccurences> occurence = new List<EmployeeOccurences>();
            if (userId != Guid.Empty)
            {
                occurence = _Util.Facade.HrFacade.GetEmployeeOccuranceByUserId(userId);
            }
            double occuranceTotal = 0;
            foreach (var item in occurence)
            {
                occuranceTotal += Convert.ToDouble(item.Amount);
            }
            ViewBag.occuranceTotal = occuranceTotal;
            return PartialView("_HrOccurance", occurence);
        }
        [Authorize]
        public PartialViewResult AddHrOccurance(int? Id)
        {
            EmployeeOccurences empOcc = new EmployeeOccurences();
            if (Id.HasValue && Id > 0)
            {
                empOcc = _Util.Facade.HrFacade.GetEmployeeOccuranceById(Id.Value);
            }
            else
            {
                empOcc.OccurenceDate = DateTime.Now;
            }
            return PartialView("_AddHrOccurance", empOcc);
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveHrOccurance(EmployeeOccurences occurence)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var message = "Occurance not save successfully";
            var result = false;
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(occurence.UserId);
            if (emp != null)
            {
                UserLogin ul = _Util.Facade.UserLoginFacade.GetUserLoginByUserId(emp.UserId);
                if (occurence != null && occurence.UserId != Guid.Empty)
                {
                    if (occurence.Id > 0)
                    {
                        var occuranceExist = _Util.Facade.HrFacade.GetEmployeeOccuranceById(occurence.Id);
                        if (occuranceExist != null)
                        {
                            occuranceExist.UserId = occurence.UserId;
                            occuranceExist.OccurenceDate = occurence.OccurenceDate;
                            occuranceExist.Amount = occurence.Amount;
                            occuranceExist.Notes = occurence.Notes;
                            occuranceExist.LastUpdatedByUid = CurrentUser.UserId;
                            occuranceExist.LastUpdatedDate = DateTime.Now;
                            result = _Util.Facade.HrFacade.UpdateEmployeeOccurance(occuranceExist);

                            message = "Occurance save successfully";
                        }
                    }
                    else
                    {
                        occurence.CreatedByUid = occurence.LastUpdatedByUid = CurrentUser.UserId;
                        occurence.CreatedDate = occurence.LastUpdatedDate = DateTime.Now;
                        var id = _Util.Facade.HrFacade.InsertEmployeeOccurance(occurence);
                        if (id > 0)
                        {
                            result = true;
                            message = "Occurance added successfully";

                            #region Notification To accused user
                            Notification notification = new Notification()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                CreatedDate = DateTime.Now,
                                NotificationId = Guid.NewGuid(),
                                Type = LabelHelper.NotificationType.Employee,
                                Who = CurrentUser.UserId,
                                What = string.Format("{0} added an occurance against you of ${1}.", "{0}", occurence.Amount)
                            };
                            _Util.Facade.NotificationFacade.InsertNotification(notification);

                            NotificationUser nu = new NotificationUser()
                            {
                                NotificationId = notification.NotificationId,
                                IsRead = false,
                                NotificationPerson = emp.UserId,
                            };
                            _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                            #endregion

                            #region Notification to System Admin
                            List<UserPermission> UPList = _Util.Facade.PermissionFacade.GetAllSysAdminUserPermissions();
                            if (UPList.Count() > 0)
                            {
                                notification = new Notification()
                                {
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    CreatedDate = DateTime.Now,
                                    NotificationId = Guid.NewGuid(),
                                    Type = LabelHelper.NotificationType.Employee,
                                    Who = CurrentUser.UserId,
                                    What = string.Format("{0} added an occurance against {2} {3} of ${1}.", "{0}", occurence.Amount, emp.FirstName, emp.LastName),
                                    NotificationUrl = string.Format("/UserInformation/?Id={0}", ul.Id)
                                };
                                _Util.Facade.NotificationFacade.InsertNotification(notification);
                                foreach (var item in UPList)
                                {
                                    nu = new NotificationUser()
                                    {
                                        NotificationId = notification.NotificationId,
                                        IsRead = false,
                                        NotificationPerson = item.UserId,
                                    };
                                    _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                                }
                            }
                            #endregion

                        }
                    }
                }
            }


            return Json(new { result = result, message = message });
        }


        [Authorize]
        [HttpPost]
        public JsonResult DeleteOccurence(int Id)
        {
            bool result = false;
            string Message = "";

            if (Id > 0)
            {
                result = _Util.Facade.HrFacade.DeleteOccurence(Id);

                Message = "Occurence deleted successfully.";

            }
            else
            {
                result = false;
                Message = "Internal Error. Please report to system admin.";
            }
            return Json(new { result = result, message = Message });
        }
        #endregion

        #region Writeups
        [Authorize]
        public ActionResult AddHrWriteUps(int? Id)
        {

            //List<SelectListItem> CatagoryList = new List<SelectListItem>();
            List<SelectListItem> SuppervisorList = new List<SelectListItem>();
            SuppervisorList = _Util.Facade.EmployeeFacade.GetAllSupervisors().OrderBy(x => x.FirstName).Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                }).ToList();

            //CatagoryList.Add(new SelectListItem() {
            //  Text = "Select One",
            //  Value = "-1"
            //});

            ViewBag.CatagoryList = _Util.Facade.LookupFacade.GetLookupByKey("WriteUpCatagoryType").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString()
                         }).ToList();


            //ViewBag.CatagoryList = CatagoryList;
            ViewBag.SupervisorList = SuppervisorList;
            EmployeeWriteUp writeUp = new EmployeeWriteUp();
            if (Id.HasValue && Id > 0)
            {
                writeUp = _Util.Facade.HrFacade.GetEmployeeWriteUpById(Id.Value);
            }
            return View(writeUp);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteWriteUp(int Id)
        {
            bool result = false;
            string Message = "";

            if (Id > 0)
            {
                _Util.Facade.HrFacade.DeleteEmpWriteUp(Id);

                Message = "Write up deleted successfully.";
                result = true;
            }
            else
            {
                result = false;
                Message = "Internal Error. Please report to system admin.";
            }
            return Json(new { result = result, message = Message });
        }
        public ActionResult HrWriteUps(Guid userId)
        {
            List<EmployeeWriteUp> writeUp = new List<EmployeeWriteUp>();
            if (userId != Guid.Empty)
            {
                writeUp = _Util.Facade.HrFacade.GetEmployeeWriteUpByUserId(userId);
            }
            List<Lookup> LookupList = _Util.Facade.LookupFacade.GetLookupByKey("WriteUpCatagoryType");
            if (writeUp != null && writeUp.Count() > 0)
            {
                foreach (var item in writeUp)
                {
                    if (LookupList.Where(x => x.DataValue == item.Category) != null && LookupList.Where(x => x.DataValue == item.Category).Count() > 0)
                    {
                        item.Category = LookupList.Where(x => x.DataValue == item.Category).FirstOrDefault().DisplayText;
                    }
                    if (item.Category == "Select One")
                    {
                        item.Category = "";
                    }
                }
            }
            return View(writeUp);
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveHrWriteup(EmployeeWriteUp writeup)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var message = "Writeup not save successfully";
            var result = false;
            var serverFile = Server.MapPath(writeup.FilePath);

            try
            {
                if (writeup.Id > 0)
                {
                    EmployeeWriteUp empWriteup = _Util.Facade.HrFacade.GetEmployeeWriteUpById(writeup.Id);


                    empWriteup.Description = writeup.Description;
                    empWriteup.Category = writeup.Category;
                    empWriteup.Supervisor = writeup.Supervisor;
                    empWriteup.WriteupDate = writeup.WriteupDate;
                    empWriteup.FilePath = writeup.FilePath;
                    empWriteup.FileName = writeup.FileName;

                    _Util.Facade.HrFacade.UpdateEmpWriteUp(empWriteup);
                    result = true;
                    message = "Write up updated successfully.";

                }
                else
                {
                    writeup.WriteupId = Guid.NewGuid();
                    writeup.CreatedBy = CurrentUser.UserId;
                    writeup.CreatedDate = DateTime.Now;
                    //empWriteup.File = writeup.File;
                    //empWriteup.FileDescription = writeup.FileDescription;
                    _Util.Facade.HrFacade.InsertEmpWriteUp(writeup);
                    result = true;
                    message = "Write up added successfully.";
                }
            }
            catch (Exception ex)
            {
                result = false;
                message = "Internal Error";
            }

            return Json(new { result = result, message = message });
        }
        #endregion

        #region emergencyContact
        [Authorize]
        public PartialViewResult HrEmergencyContact()
        {
            return PartialView("_HrEmergencyContact");
        }

        [Authorize]
        public ActionResult ComputerPanel(Guid userId)
        {
            ViewBag.userId = userId;
            EmployeeComputer Computer = _Util.Facade.EmployeeComputerFacade.GetComputerByUserId(userId);

            if (Computer == null)
            {
                Computer = new EmployeeComputer();
            }
            return View(Computer);
        }
        [HttpPost]
        public JsonResult SaveComputer(EmployeeComputer Computer)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string message = "Internal Error";
            bool result = false;
            try
            {
                EmployeeComputer computer = _Util.Facade.EmployeeComputerFacade.GetComputerById(Computer.Id);
                if (Computer.Id > 0)
                {
                    Computer.CompanyId = computer.CompanyId;
                    Computer.CreatedBy = computer.UserId;
                    Computer.LastUpdatedBy = CurrentUser.UserId;
                    Computer.LastUpdatedBy = CurrentUser.UserId;
                    Computer.LastUpdatedDate = DateTime.Now;
                    _Util.Facade.EmployeeComputerFacade.UpdateComputer(Computer);

                }
                else
                {
                    Computer.CompanyId = (Guid)CurrentUser.CompanyId;
                    Computer.UserId = Computer.UserId;
                    Computer.CreatedBy = CurrentUser.UserId;
                    Computer.LastUpdatedBy = CurrentUser.UserId;
                    Computer.LastUpdatedDate = DateTime.Now;
                    Computer.Id = (int)_Util.Facade.EmployeeComputerFacade.InsertComputer(Computer);
                }
                result = true;
                message = "Saved Successfully.";
            }
            catch (Exception ex)
            {

            }
            // return RedirectToAction("Dashboard");
            return Json(new { result = result, message });

        }
        #endregion

        [Authorize]
        public ActionResult VehiclePanel(Guid userId)
        {
            ViewBag.userId = userId;
            EmployeeVehicle vehicle = new EmployeeVehicle();

            List<SelectListItem> vehicleList = new List<SelectListItem>();
            vehicleList.Add(new SelectListItem
            {
                Text = "Select One",
                Value = Guid.Empty.ToString()
            });
            vehicleList.AddRange(_Util.Facade.VehicleFacade.GetAllVehicle().Select(x =>
            new SelectListItem()
            {
                Text = !string.IsNullOrEmpty(x.Model) ? x.VIN.ToString() + "[" + x.Model + ']' : x.VIN.ToString(),
                Value = x.VehicleId.ToString()
            }).ToList());
            if (userId != new Guid())
            {
                vehicle = _Util.Facade.VehicleFacade.GetEmployeeVehicleByUserId(userId);

                if (vehicle == null)
                {
                    vehicle = new EmployeeVehicle();
                }
            }
            ViewBag.VehicleList = vehicleList;

            return View(vehicle);
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveWeeklyEmployeeOperationTimeList(EmployeeWeeklyOperations EmpOperations)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var Result = false;
            string Message = "";
            try
            {
                Guid Uid = Guid.Empty;
                DateTime dt;
                bool successfullyParsed = DateTime.TryParse(EmpOperations.CurrentDate, out dt);
                if (string.IsNullOrEmpty(EmpOperations.CurrentDate) || !successfullyParsed)
                {
                    dt = DateTime.UtcNow.UTCToClientTime();
                }
                dt = dt.SetZeroHour();
                if (Guid.TryParse(EmpOperations.UserId, out Uid) && Uid != Guid.Empty)
                {
                    string dayName = dt.ToString("dddd");
                    string startingtime = _Util.Facade.GlobalSettingsFacade.GetAvailabilityStartTime(CurrentUser.CompanyId.Value);
                    string endingtime = _Util.Facade.GlobalSettingsFacade.GetAvailabilityEndTime(CurrentUser.CompanyId.Value);
                    List<EmployeeOperations> modellist = _Util.Facade.EmployeeFacade.GetAllEmployeeOperationByIdandFromDate(Uid, dt);
                    if (modellist != null && modellist.Count > 0)
                    {
                        for (int i = 0; i < 366; i++)
                        {
                            EmployeeOperations UpdateModel = modellist.Where(x => x.SelectedDate == dt).FirstOrDefault();

                            if (UpdateModel != null && UpdateModel.Id > 0)
                            {
                                // Update
                                string startTime = "";
                                string endTime = "";
                                UpdateModel.LastUpdatedBy = CurrentUser.UserId;
                                UpdateModel.UpdatedDate = DateTime.UtcNow;
                                switch (dayName)
                                {
                                    case "Monday":
                                        startTime = EmpOperations.MonStart;
                                        endTime = EmpOperations.MonEnd;
                                        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                        {
                                            UpdateModel.OperationStartTime = string.Empty;
                                            UpdateModel.OperationEndTime = string.Empty;
                                            UpdateModel.IsDayOff = true;
                                        }
                                       else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                        {
                                            dt = dt.AddDays(1);
                                            dayName = dt.ToString("dddd");
                                            continue;
                                        }
                                        else
                                        {
                                            UpdateModel.OperationStartTime = startTime;
                                            UpdateModel.OperationEndTime = endTime;
                                            UpdateModel.IsDayOff = false;
                                        }
                                        Result = _Util.Facade.EmployeeFacade.UpdateEmployeeOperation(UpdateModel) > 0;
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    case "Tuesday":
                                        startTime = EmpOperations.TueStart;
                                        endTime = EmpOperations.TueEnd;
                                        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                        {
                                            UpdateModel.OperationStartTime = string.Empty;
                                            UpdateModel.OperationEndTime = string.Empty;
                                            UpdateModel.IsDayOff = true;
                                        }
                                        else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                        {
                                            dt = dt.AddDays(1);
                                            dayName = dt.ToString("dddd");
                                            continue;
                                        }
                                        else
                                        {
                                            UpdateModel.OperationStartTime = startTime;
                                            UpdateModel.OperationEndTime = endTime;
                                            UpdateModel.IsDayOff = false;
                                        }
                                        Result = _Util.Facade.EmployeeFacade.UpdateEmployeeOperation(UpdateModel) > 0;
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    case "Wednesday":
                                        startTime = EmpOperations.WedStart;
                                        endTime = EmpOperations.WedEnd;
                                        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                        {
                                            UpdateModel.OperationStartTime = string.Empty;
                                            UpdateModel.OperationEndTime = string.Empty;
                                            UpdateModel.IsDayOff = true;
                                        }
                                        else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                        {
                                            dt = dt.AddDays(1);
                                            dayName = dt.ToString("dddd");
                                            continue;
                                        }
                                        else
                                        {
                                            UpdateModel.OperationStartTime = startTime;
                                            UpdateModel.OperationEndTime = endTime;
                                            UpdateModel.IsDayOff = false;
                                        }
                                        Result = _Util.Facade.EmployeeFacade.UpdateEmployeeOperation(UpdateModel) > 0;
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    case "Thursday":
                                        startTime = EmpOperations.ThuStart;
                                        endTime = EmpOperations.ThuEnd;
                                        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                        {
                                            UpdateModel.OperationStartTime = string.Empty;
                                            UpdateModel.OperationEndTime = string.Empty;
                                            UpdateModel.IsDayOff = true;
                                        }
                                        else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                        {
                                            dt = dt.AddDays(1);
                                            dayName = dt.ToString("dddd");
                                            continue;
                                        }
                                        else
                                        {
                                            UpdateModel.OperationStartTime = startTime;
                                            UpdateModel.OperationEndTime = endTime;
                                            UpdateModel.IsDayOff = false;
                                        }
                                        Result = _Util.Facade.EmployeeFacade.UpdateEmployeeOperation(UpdateModel) > 0;
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    case "Friday":
                                        startTime = EmpOperations.FriStart;
                                        endTime = EmpOperations.FriEnd;
                                        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                        {
                                            UpdateModel.OperationStartTime = string.Empty;
                                            UpdateModel.OperationEndTime = string.Empty;
                                            UpdateModel.IsDayOff = true;
                                        }
                                        else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                        {
                                            dt = dt.AddDays(1);
                                            dayName = dt.ToString("dddd");
                                            continue;
                                        }
                                        else
                                        {
                                            UpdateModel.OperationStartTime = startTime;
                                            UpdateModel.OperationEndTime = endTime;
                                            UpdateModel.IsDayOff = false;
                                        }
                                        Result = _Util.Facade.EmployeeFacade.UpdateEmployeeOperation(UpdateModel) > 0;
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    case "Saturday":
                                        startTime = EmpOperations.SatStart;
                                        endTime = EmpOperations.SatEnd;
                                        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                        {
                                            UpdateModel.OperationStartTime = string.Empty;
                                            UpdateModel.OperationEndTime = string.Empty;
                                            UpdateModel.IsDayOff = true;
                                        }
                                        else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                        {
                                            dt = dt.AddDays(1);
                                            dayName = dt.ToString("dddd");
                                            continue;
                                        }
                                        else
                                        {
                                            UpdateModel.OperationStartTime = startTime;
                                            UpdateModel.OperationEndTime = endTime;
                                            UpdateModel.IsDayOff = false;
                                        }
                                        Result = _Util.Facade.EmployeeFacade.UpdateEmployeeOperation(UpdateModel) > 0;
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    case "Sunday":
                                        startTime = EmpOperations.SunStart;
                                        endTime = EmpOperations.SunEnd;
                                        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                        {
                                            UpdateModel.OperationStartTime = string.Empty;
                                            UpdateModel.OperationEndTime = string.Empty;
                                            UpdateModel.IsDayOff = true;
                                        }
                                        else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                        {
                                            dt = dt.AddDays(1);
                                            dayName = dt.ToString("dddd");
                                            continue;
                                        }
                                        else
                                        {
                                            UpdateModel.OperationStartTime = startTime;
                                            UpdateModel.OperationEndTime = endTime;
                                            UpdateModel.IsDayOff = false;
                                        }
                                        Result = _Util.Facade.EmployeeFacade.UpdateEmployeeOperation(UpdateModel) > 0;
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    default:
                                        continue;
                                }
                            }
                            else
                            {
                                UpdateModel = new EmployeeOperations();
                                string startTime = "";
                                string endTime = "";
                                UpdateModel.EmployeeId = Uid;
                                UpdateModel.CompanyId = CurrentUser.CompanyId.HasValue ? CurrentUser.CompanyId.Value : Guid.Empty;
                                UpdateModel.LastUpdatedBy = CurrentUser.UserId;
                                UpdateModel.UpdatedDate = DateTime.UtcNow;
                                UpdateModel.DayName = dayName;
                                UpdateModel.SelectedDate = dt;
                                switch (dayName)
                                {
                                    case "Monday":
                                        startTime = EmpOperations.MonStart;
                                        endTime = EmpOperations.MonEnd;
                                        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                        {
                                            UpdateModel.OperationStartTime = string.Empty;
                                            UpdateModel.OperationEndTime = string.Empty;
                                            UpdateModel.IsDayOff = true;
                                        }
                                        else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                        {
                                            dt = dt.AddDays(1);
                                            dayName = dt.ToString("dddd");
                                            continue;
                                        }
                                        else
                                        {
                                            UpdateModel.OperationStartTime = startTime;
                                            UpdateModel.OperationEndTime = endTime;
                                            UpdateModel.IsDayOff = false;
                                        }
                                        Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(UpdateModel) > 0;
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    case "Tuesday":
                                        startTime = EmpOperations.TueStart;
                                        endTime = EmpOperations.TueEnd;
                                        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                        {
                                            UpdateModel.OperationStartTime = string.Empty;
                                            UpdateModel.OperationEndTime = string.Empty;
                                            UpdateModel.IsDayOff = true;
                                        }
                                        else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                        {
                                            dt = dt.AddDays(1);
                                            dayName = dt.ToString("dddd");
                                            continue;
                                        }
                                        else
                                        {
                                            UpdateModel.OperationStartTime = startTime;
                                            UpdateModel.OperationEndTime = endTime;
                                            UpdateModel.IsDayOff = false;
                                        }
                                        Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(UpdateModel) > 0;
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    case "Wednesday":
                                        startTime = EmpOperations.WedStart;
                                        endTime = EmpOperations.WedEnd;
                                        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                        {
                                            UpdateModel.OperationStartTime = string.Empty;
                                            UpdateModel.OperationEndTime = string.Empty;
                                            UpdateModel.IsDayOff = true;
                                        }
                                        else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                        {
                                            dt = dt.AddDays(1);
                                            dayName = dt.ToString("dddd");
                                            continue;
                                        }
                                        else
                                        {
                                            UpdateModel.OperationStartTime = startTime;
                                            UpdateModel.OperationEndTime = endTime;
                                            UpdateModel.IsDayOff = false;
                                        }
                                        Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(UpdateModel) > 0;
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    case "Thursday":
                                        startTime = EmpOperations.ThuStart;
                                        endTime = EmpOperations.ThuEnd;
                                        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                        {
                                            UpdateModel.OperationStartTime = string.Empty;
                                            UpdateModel.OperationEndTime = string.Empty;
                                            UpdateModel.IsDayOff = true;
                                        }
                                        else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                        {
                                            dt = dt.AddDays(1);
                                            dayName = dt.ToString("dddd");
                                            continue;
                                        }
                                        else
                                        {
                                            UpdateModel.OperationStartTime = startTime;
                                            UpdateModel.OperationEndTime = endTime;
                                            UpdateModel.IsDayOff = false;
                                        }
                                        Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(UpdateModel) > 0;
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    case "Friday":
                                        startTime = EmpOperations.FriStart;
                                        endTime = EmpOperations.FriEnd;
                                        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                        {
                                            UpdateModel.OperationStartTime = string.Empty;
                                            UpdateModel.OperationEndTime = string.Empty;
                                            UpdateModel.IsDayOff = true;
                                        }
                                        else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                        {
                                            dt = dt.AddDays(1);
                                            dayName = dt.ToString("dddd");
                                            continue;
                                        }
                                        else
                                        {
                                            UpdateModel.OperationStartTime = startTime;
                                            UpdateModel.OperationEndTime = endTime;
                                            UpdateModel.IsDayOff = false;
                                        }
                                        Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(UpdateModel) > 0;
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    case "Saturday":
                                        startTime = EmpOperations.SatStart;
                                        endTime = EmpOperations.SatEnd;
                                        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                        {
                                            UpdateModel.OperationStartTime = string.Empty;
                                            UpdateModel.OperationEndTime = string.Empty;
                                            UpdateModel.IsDayOff = true;
                                        }
                                        else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                        {
                                            dt = dt.AddDays(1);
                                            dayName = dt.ToString("dddd");
                                            continue;
                                        }
                                        else
                                        {
                                            UpdateModel.OperationStartTime = startTime;
                                            UpdateModel.OperationEndTime = endTime;
                                            UpdateModel.IsDayOff = false;
                                        }
                                        Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(UpdateModel) > 0;
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    case "Sunday":
                                        startTime = EmpOperations.SunStart;
                                        endTime = EmpOperations.SunEnd;
                                        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                        {
                                            UpdateModel.OperationStartTime = string.Empty;
                                            UpdateModel.OperationEndTime = string.Empty;
                                            UpdateModel.IsDayOff = true;
                                        }
                                        else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                        {
                                            dt = dt.AddDays(1);
                                            dayName = dt.ToString("dddd");
                                            continue;
                                        }
                                        else
                                        {
                                            UpdateModel.OperationStartTime = startTime;
                                            UpdateModel.OperationEndTime = endTime;
                                            UpdateModel.IsDayOff = false;
                                        }
                                        Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(UpdateModel) > 0;
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    default:
                                        continue;
                                }
                            }
                        }
                        Message = "Saved successfully.";
                    }
                    else
                    {
                        // Insert

                        for (int i = 0; i < 366; i++)
                        {
                            EmployeeOperations InsertModel = new EmployeeOperations();
                            string startTime = "";
                            string endTime = "";
                            InsertModel.EmployeeId = Uid;
                            InsertModel.CompanyId = CurrentUser.CompanyId.HasValue ? CurrentUser.CompanyId.Value : Guid.Empty;
                            InsertModel.LastUpdatedBy = CurrentUser.UserId;
                            InsertModel.UpdatedDate = DateTime.UtcNow;
                            InsertModel.DayName = dayName;
                            InsertModel.SelectedDate = dt;
                            switch (dayName)
                            {
                                case "Monday":
                                    startTime = EmpOperations.MonStart;
                                    endTime = EmpOperations.MonEnd;                                    
                                    if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                    {
                                        InsertModel.OperationStartTime = string.Empty;
                                        InsertModel.OperationEndTime = string.Empty;
                                        InsertModel.IsDayOff = true;
                                    }
                                    else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                    {
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    }
                                    else
                                    {
                                        InsertModel.OperationStartTime = startTime;
                                        InsertModel.OperationEndTime = endTime;
                                        InsertModel.IsDayOff = false;
                                    }                                    
                                    Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(InsertModel) > 0;
                                    dt = dt.AddDays(1);
                                    dayName = dt.ToString("dddd");
                                    continue;
                                case "Tuesday":
                                    startTime = EmpOperations.TueStart;
                                    endTime = EmpOperations.TueEnd;
                                    if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                    {
                                        InsertModel.OperationStartTime = string.Empty;
                                        InsertModel.OperationEndTime = string.Empty;
                                        InsertModel.IsDayOff = true;
                                    }
                                    else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                    {
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    }
                                    else
                                    {
                                        InsertModel.OperationStartTime = startTime;
                                        InsertModel.OperationEndTime = endTime;
                                        InsertModel.IsDayOff = false;
                                    }
                                    Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(InsertModel) > 0;
                                    dt = dt.AddDays(1);
                                    dayName = dt.ToString("dddd");
                                    continue;
                                case "Wednesday":
                                    startTime = EmpOperations.WedStart;
                                    endTime = EmpOperations.WedEnd;
                                    if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                    {
                                        InsertModel.OperationStartTime = string.Empty;
                                        InsertModel.OperationEndTime = string.Empty;
                                        InsertModel.IsDayOff = true;
                                    }
                                    else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                    {
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    }
                                    else
                                    {
                                        InsertModel.OperationStartTime = startTime;
                                        InsertModel.OperationEndTime = endTime;
                                        InsertModel.IsDayOff = false;
                                    }
                                    Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(InsertModel) > 0;
                                    dt = dt.AddDays(1);
                                    dayName = dt.ToString("dddd");
                                    continue;
                                case "Thursday":
                                    startTime = EmpOperations.ThuStart;
                                    endTime = EmpOperations.ThuEnd;
                                    if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                    {
                                        InsertModel.OperationStartTime = string.Empty;
                                        InsertModel.OperationEndTime = string.Empty;
                                        InsertModel.IsDayOff = true;
                                    }
                                    else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                    {
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    }
                                    else
                                    {
                                        InsertModel.OperationStartTime = startTime;
                                        InsertModel.OperationEndTime = endTime;
                                        InsertModel.IsDayOff = false;
                                    }
                                    Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(InsertModel) > 0;
                                    dt = dt.AddDays(1);
                                    dayName = dt.ToString("dddd");
                                    continue;
                                case "Friday":
                                    startTime = EmpOperations.FriStart;
                                    endTime = EmpOperations.FriEnd;
                                    if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                    {
                                        InsertModel.OperationStartTime = string.Empty;
                                        InsertModel.OperationEndTime = string.Empty;
                                        InsertModel.IsDayOff = true;
                                    }
                                    else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                    {
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    }
                                    else
                                    {
                                        InsertModel.OperationStartTime = startTime;
                                        InsertModel.OperationEndTime = endTime;
                                        InsertModel.IsDayOff = false;
                                    }
                                    Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(InsertModel) > 0;
                                    dt = dt.AddDays(1);
                                    dayName = dt.ToString("dddd");
                                    continue;
                                case "Saturday":
                                    startTime = EmpOperations.SatStart;
                                    endTime = EmpOperations.SatEnd;
                                    if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                    {
                                        InsertModel.OperationStartTime = string.Empty;
                                        InsertModel.OperationEndTime = string.Empty;
                                        InsertModel.IsDayOff = true;
                                    }
                                    else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                    {
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    }
                                    else
                                    {
                                        InsertModel.OperationStartTime = startTime;
                                        InsertModel.OperationEndTime = endTime;
                                        InsertModel.IsDayOff = false;
                                    }
                                    Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(InsertModel) > 0;
                                    dt = dt.AddDays(1);
                                    dayName = dt.ToString("dddd");
                                    continue;
                                case "Sunday":
                                    startTime = EmpOperations.SunStart;
                                    endTime = EmpOperations.SunEnd;
                                    if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
                                    {
                                        InsertModel.OperationStartTime = string.Empty;
                                        InsertModel.OperationEndTime = string.Empty;
                                        InsertModel.IsDayOff = true;
                                    }
                                    else if (startTime == "WeekEnd" || endTime == "WeekEnd")
                                    {
                                        dt = dt.AddDays(1);
                                        dayName = dt.ToString("dddd");
                                        continue;
                                    }
                                    else
                                    {
                                        InsertModel.OperationStartTime = startTime;
                                        InsertModel.OperationEndTime = endTime;
                                        InsertModel.IsDayOff = false;
                                    }
                                    Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(InsertModel) > 0;
                                    dt = dt.AddDays(1);
                                    dayName = dt.ToString("dddd");
                                    continue;
                                default:
                                    continue;

                            }
                        }
                        Message = "Saved successfully.";
                    }

                }
                return Json(new { result = Result, message = Message });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = "Internal error." });
            }
        }
        [Authorize]
        public ActionResult WeeklyAvailibilityPanel(Guid userId, string strdate)
        {
            EmployeeWeeklyOperations model = new EmployeeWeeklyOperations();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            DateTime dt;
            bool successfullyParsed = DateTime.TryParse(strdate, out dt);
            if (string.IsNullOrEmpty(strdate) || !successfullyParsed)
            {
                dt = DateTime.UtcNow.UTCToClientTime();
            }
            dt = dt.SetZeroHour();
            string currentDay = dt.ToString("dddd");
            string startingtime = _Util.Facade.GlobalSettingsFacade.GetAvailabilityStartTime(currentLoggedIn.CompanyId.Value);
            string endingtime = _Util.Facade.GlobalSettingsFacade.GetAvailabilityEndTime(currentLoggedIn.CompanyId.Value);
            string FirstDayOfWeek = _Util.Facade.GlobalSettingsFacade.GetOnlyStrValueFromGlobalSettingByKey("FirstDayOfWeek");
            string HolidayCount = _Util.Facade.GlobalSettingsFacade.GetOnlyStrValueFromGlobalSettingByKey("CustomCalendarColumnHourDuration");

            DateTime startDate = dt;
            DateTime EndDate = dt;
            if (FirstDayOfWeek.ToLower() == "saturday")
            {
                if (currentDay == "Sunday")
                {
                    startDate = dt.AddDays(-1);
                    EndDate = dt.AddDays(5);
                }
                else if (currentDay == "Monday")
                {
                    startDate = dt.AddDays(-2);
                    EndDate = dt.AddDays(4);
                }
                else if (currentDay == "Tuesday")
                {
                    startDate = dt.AddDays(-3);
                    EndDate = dt.AddDays(3);
                }
                else if (currentDay == "Wednesday")
                {
                    startDate = dt.AddDays(-4);
                    EndDate = dt.AddDays(2);
                }
                else if (currentDay == "Thursday")
                {
                    startDate = dt.AddDays(-5);
                    EndDate = dt.AddDays(1);
                }
                else if (currentDay == "Friday")
                {
                    startDate = dt.AddDays(-6);
                }
                else
                {
                    EndDate = dt.AddDays(6);
                }
            }
            else if (FirstDayOfWeek.ToLower() == "sunday")
            {
                if (currentDay == "Monday")
                {
                    startDate = dt.AddDays(-1);
                    EndDate = dt.AddDays(5);
                }
                else if (currentDay == "Tuesday")
                {
                    startDate = dt.AddDays(-2);
                    EndDate = dt.AddDays(4);
                }
                else if (currentDay == "Wednesday")
                {
                    startDate = dt.AddDays(-3);
                    EndDate = dt.AddDays(3);
                }
                else if (currentDay == "Thursday")
                {
                    startDate = dt.AddDays(-4);
                    EndDate = dt.AddDays(2);
                }
                else if (currentDay == "Friday")
                {
                    startDate = dt.AddDays(-5);
                    EndDate = dt.AddDays(1);
                }
                else if (currentDay == "Saturday")
                {
                    startDate = dt.AddDays(-6);
                }
                else
                {
                    EndDate = dt.AddDays(6);
                }
            }
            else if (FirstDayOfWeek.ToLower() == "monday")
            {
                if (currentDay == "Tuesday")
                {
                    startDate = dt.AddDays(-1);
                    EndDate = dt.AddDays(5);
                }
                else if (currentDay == "Wednesday")
                {
                    startDate = dt.AddDays(-2);
                    EndDate = dt.AddDays(4);
                }
                else if (currentDay == "Thursday")
                {
                    startDate = dt.AddDays(-3);
                    EndDate = dt.AddDays(3);
                }
                else if (currentDay == "Friday")
                {
                    startDate = dt.AddDays(-4);
                    EndDate = dt.AddDays(2);
                }
                else if (currentDay == "Saturday")
                {
                    startDate = dt.AddDays(-5);
                    EndDate = dt.AddDays(1);
                }
                else if (currentDay == "Sunday")
                {
                    startDate = dt.AddDays(-6);
                }
                else
                {
                    EndDate = dt.AddDays(6);
                }
            }
            List<EmployeeOperations> operations = _Util.Facade.EmployeeFacade.GetAllEmployeeOperationById(userId, startDate, EndDate);
            if (operations == null) { operations = new List<EmployeeOperations>(); }

            DateTime CurrentstartDate = startDate;
            for (int i = 1; i < 8; i++)
            {

                string startTime = startingtime.Contains(":") ? startingtime : "09:00";
                string endTime = endingtime.Contains(":") ? endingtime : "17:00";
                string DayName = CurrentstartDate.ToString("dddd");
                var ExistDate = operations.Where(x => x.SelectedDate == CurrentstartDate).FirstOrDefault();
                if (ExistDate != null)
                {
                    if (ExistDate.IsDayOff.HasValue && ExistDate.IsDayOff.Value)
                    {
                        startTime = string.Empty;
                        endTime = string.Empty;
                    }
                    else if (!string.IsNullOrWhiteSpace(ExistDate.OperationStartTime) && ExistDate.OperationStartTime.Contains(":") && !string.IsNullOrWhiteSpace(ExistDate.OperationEndTime) && ExistDate.OperationEndTime.Contains(":"))
                    {
                        startTime = ExistDate.OperationStartTime;
                        endTime = ExistDate.OperationEndTime;
                    }
                }

                #region Weekly Holiday Calculation
                if (HolidayCount == "1" && i == 7)
                {
                    startTime = "WeekEnd";
                    endTime = "WeekEnd";
                }
                else if (HolidayCount == "2" && (i == 7 || i == 6))
                {
                    startTime = "WeekEnd";
                    endTime = "WeekEnd";
                }
                else if (HolidayCount == "3" && (i == 7 || i == 6 || i == 5))
                {
                    startTime = "WeekEnd";
                    endTime = "WeekEnd";
                }
                else if (HolidayCount == "4" && (i == 7 || i == 6 || i == 5 || i == 4))
                {
                    startTime = "WeekEnd";
                    endTime = "WeekEnd";
                }
                else if (HolidayCount == "5" && (i == 7 || i == 6 || i == 5 || i == 4 || i == 3))
                {
                    startTime = "WeekEnd";
                    endTime = "WeekEnd";
                }
                else if (HolidayCount == "6" && (i == 7 || i == 6 || i == 5 || i == 4 || i == 2 || i == 3))
                {
                    startTime = "WeekEnd";
                    endTime = "WeekEnd";
                }
                #endregion

                switch (DayName)
                {
                    case "Monday":
                        model.MonStart = startTime;
                        model.MonEnd = endTime;
                        CurrentstartDate = CurrentstartDate.AddDays(1);
                        continue;
                    case "Tuesday":
                        model.TueStart = startTime;
                        model.TueEnd = endTime;
                        CurrentstartDate = CurrentstartDate.AddDays(1);
                        continue;
                    case "Wednesday":
                        model.WedStart = startTime;
                        model.WedEnd = endTime;
                        CurrentstartDate = CurrentstartDate.AddDays(1);
                        continue;
                    case "Thursday":
                        model.ThuStart = startTime;
                        model.ThuEnd = endTime;
                        CurrentstartDate = CurrentstartDate.AddDays(1);
                        continue;
                    case "Friday":
                        model.FriStart = startTime;
                        model.FriEnd = endTime;
                        CurrentstartDate = CurrentstartDate.AddDays(1);
                        continue;
                    case "Saturday":
                        model.SatStart = startTime;
                        model.SatEnd = endTime;
                        CurrentstartDate = CurrentstartDate.AddDays(1);
                        continue;
                    case "Sunday":
                        model.SunStart = startTime;
                        model.SunEnd = endTime;
                        CurrentstartDate = CurrentstartDate.AddDays(1);
                        continue;
                    default:
                        continue;
                }
            }
            model.UserId = userId.ToString();
            model.CurrentDate = startDate.ToString("MM/dd/yyyy");
            List<SelectListItem> DefaultTimeList = new List<SelectListItem>();
            DefaultTimeList.Add(new SelectListItem
            {
                Text = "Day Off",
                Value = string.Empty
            });
            DefaultTimeList.Add(new SelectListItem
            {
                Text = "WeekEnd",
                Value = "WeekEnd"
            });
            var TimeList = _Util.Facade.LookupFacade.GetAllLookupByKey("Arrival").OrderBy(x => x.DataOrder).Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText,
                 Value = x.DataValue
             }).ToList();
            DefaultTimeList.AddRange(TimeList.Where(x => x.Value.Contains(":00")).ToList());
            ViewBag.TimeList = DefaultTimeList;

            return View(model);
        }
        public ActionResult EditEmployeeAvailibility(string GetDate, Guid userId)
        {

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            DateTime dt;
            bool successfullyParsed = DateTime.TryParse(GetDate, out dt);
            if (string.IsNullOrEmpty(GetDate) || !successfullyParsed)
            {
                dt = DateTime.UtcNow.UTCToClientTime();
            }
            EmployeeOperations operations = _Util.Facade.EmployeeFacade.GetOnlyEmployeeOperationById(userId, dt.SetZeroHour());
            var timelist = _Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", null, null).Select(x =>
                    new SelectListItem()
                    {
                        Text = x.DisplayText.ToString(),
                        Value = x.DataValue.ToString()
                    }).ToList();
            List<SelectListItem> SchedularTime = new List<SelectListItem>();
            SchedularTime.Add(new SelectListItem() { Text = "Please Select One", Value = "-1" });
            SchedularTime.AddRange(timelist.Where(x => x.Value.Contains(":00")).Select(x => x).ToList()); // Only get 00 min.s
            ViewBag.SchedularTime = SchedularTime;
            if (operations == null)
            {
                operations = new EmployeeOperations();
                string startingtime = _Util.Facade.GlobalSettingsFacade.GetAvailabilityStartTime(currentLoggedIn.CompanyId.Value);
                string endingtime = _Util.Facade.GlobalSettingsFacade.GetAvailabilityEndTime(currentLoggedIn.CompanyId.Value);
                DateTime startdateconversion = DateTime.Parse(string.Format("{0} {1}:00.000", dt.ToString("yyyy/MM/dd"), startingtime.Contains(':') ? startingtime : "09:00"));
                DateTime enddateconversion = DateTime.Parse(string.Format("{0} {1}:00.000", dt.ToString("yyyy/MM/dd"), endingtime.Contains(':') ? endingtime : "17:00"));
                operations.EmployeeId = userId;
                operations.SelectedDate = dt.SetZeroHour();
                operations.OperationStartTime = startdateconversion.ToString("HH:mm");
                operations.OperationEndTime = enddateconversion.ToString("HH:mm");
                operations.LastUpdatedBy = currentLoggedIn.UserId;
                operations.UpdatedDate = DateTime.UtcNow;
                operations.Notes = "";
                operations.DayName = dt.ToString("dddd");
                operations.CompanyId = currentLoggedIn.CompanyId.HasValue ? currentLoggedIn.CompanyId.Value : Guid.Empty;
            }
            return View(operations);
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveEmployeeOperationTime(EmployeeOperations EmpOperations)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var Result = false;
            string Message = "";
            try
            {
                Guid Uid = Guid.Empty;
                if (EmpOperations != null && EmpOperations.EmployeeId != Guid.Empty && EmpOperations.SelectedDate.HasValue && EmpOperations.SelectedDate != new DateTime())
                {
                    EmployeeOperations model = _Util.Facade.EmployeeFacade.GetOnlyEmployeeOperationById(EmpOperations.EmployeeId, EmpOperations.SelectedDate.Value.SetZeroHour());
                    if (model != null && model.Id > 0)
                    {
                        // Update
                        model.IsDayOff = EmpOperations.IsDayOff;
                        if (string.IsNullOrWhiteSpace(EmpOperations.OperationStartTime) || string.IsNullOrWhiteSpace(EmpOperations.OperationEndTime))
                        {
                            model.OperationStartTime = string.Empty;
                            model.OperationEndTime = string.Empty;
                            model.IsDayOff = true;
                        }
                        else
                        {
                            model.OperationStartTime = EmpOperations.OperationStartTime;
                            model.OperationEndTime = EmpOperations.OperationEndTime;
                        }
                        model.LastUpdatedBy = CurrentUser.UserId;
                        model.UpdatedDate = DateTime.UtcNow;
                        model.Notes = EmpOperations.Notes;
                        model.SelectedDate = EmpOperations.SelectedDate;
                        model.DayName = EmpOperations.SelectedDate.Value.ToString("dddd");
                        Result = _Util.Facade.EmployeeFacade.UpdateEmployeeOperation(model) > 0;
                        Message = "Updated successfully.";
                    }
                    else
                    {
                        // Insert
                        model = new EmployeeOperations();
                        model.IsDayOff = EmpOperations.IsDayOff;
                        if (string.IsNullOrWhiteSpace(EmpOperations.OperationStartTime) || string.IsNullOrWhiteSpace(EmpOperations.OperationEndTime))
                        {
                            model.OperationStartTime = string.Empty;
                            model.OperationEndTime = string.Empty;
                            model.IsDayOff = true;
                        }
                        else
                        {
                            model.OperationStartTime = EmpOperations.OperationStartTime;
                            model.OperationEndTime = EmpOperations.OperationEndTime;
                        }
                        model.LastUpdatedBy = CurrentUser.UserId;
                        model.UpdatedDate = DateTime.UtcNow;
                        model.Notes = EmpOperations.Notes;
                        model.SelectedDate = EmpOperations.SelectedDate;
                        model.DayName = EmpOperations.SelectedDate.Value.ToString("dddd");
                        model.EmployeeId = EmpOperations.EmployeeId;
                        model.CompanyId = CurrentUser.CompanyId.HasValue ? CurrentUser.CompanyId.Value : Guid.Empty;
                        Result = _Util.Facade.EmployeeFacade.InsertEmployeeOperation(model) > 0;

                    }
                    Message = "Saved successfully.";
                }
                return Json(new { result = Result, message = Message });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = "Internal error." });
            }
        }
        public static string GetFormattedStringFromDays(int numberOfDays)
        {
            int years = numberOfDays / 365;
            int months = (numberOfDays % 365) / 30;
            int days = (numberOfDays % 365) % 30;

            string yearsDisplay = years > 0 ? years + (years == 1 ? " year, " : " years, ") : "";
            string monthsDisplay = months > 0 ? months + (months == 1 ? " month, " : " months, ") : "";
            string daysDisplay = days > 0 ? days + (days == 1 ? " day" : " days") : "";

            // Remove the trailing comma and space if present
            string result = yearsDisplay + monthsDisplay + daysDisplay;
            return result.EndsWith(", ") ? result.Substring(0, result.Length - 2) : result;
        }

        [Authorize]
        public ActionResult EmployeePtoAccrualRateSetting()
        { 
            List<EmployeePtoAccrualRate> model = new List<EmployeePtoAccrualRate>();
             model = _Util.Facade.EmployeeFacade.GetAllEmployeePtoAccrualRate();
             
            
            if(model == null)
            {
                 model = new List<EmployeePtoAccrualRate>();
            }  
            return View(model);
        }
        [Authorize]
        public ActionResult EmployeePtoAccrualRate(int Id)
        {
            EmployeePtoAccrualRate model = new EmployeePtoAccrualRate();
            if (Id> 0)
            {
                 model = _Util.Facade.EmployeeFacade.GetEmployeePtoAccrualRateById(Id);
            }

            ViewBag.EmployeeAccrualPayType = _Util.Facade.LookupFacade.GetLookupByKey("EmployeePayTypeData").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString(),
                                //Selected = x.DataValue.ToString() == model.PayType
                            }).ToList();
             
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveEmployeePtoAccrualRate(EmployeePtoAccrualRate employeePtoAccrual)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var Result = false;
            EmployeePtoAccrualRate model = new EmployeePtoAccrualRate();
            string Message = "";
            try
            {
                Guid Uid = Guid.Empty;
                if (employeePtoAccrual != null)
                {
                     if(employeePtoAccrual.Id>0)
                    {
                        model = _Util.Facade.EmployeeFacade.GetEmployeePtoAccrualRateById(employeePtoAccrual.Id);
                        if (model != null)
                        {
                            model.PayType = employeePtoAccrual.PayType;
                            model.MinimumDay = employeePtoAccrual.MinimumDay;
                            model.MaximumDay = employeePtoAccrual.MaximumDay;
                            model.AccrualRate = employeePtoAccrual.AccrualRate;
                            model.PtoHours = employeePtoAccrual.PtoHours;
                            model.CreatedBy = CurrentUser.UserId;
                            model.CreatedDate = DateTime.UtcNow;
                            model.LastUpdatedDate = DateTime.UtcNow;
                            model.LastUpdatedBy = CurrentUser.UserId;
                            Result = _Util.Facade.EmployeeFacade.UpdateEmployeePtoAccrualRate(model) > 0;
                        }
                    }
                    else
                    {
                        model.PayType = employeePtoAccrual.PayType;
                        model.MinimumDay = employeePtoAccrual.MinimumDay;
                        model.MaximumDay = employeePtoAccrual.MaximumDay;
                        model.AccrualRate = employeePtoAccrual.AccrualRate;
                        model.PtoHours = employeePtoAccrual.PtoHours;
                        model.CompanyId = CurrentUser.CompanyId.Value;
                        model.CreatedBy = CurrentUser.UserId;
                        model.CreatedDate = DateTime.UtcNow;
                        model.LastUpdatedDate = DateTime.UtcNow;
                        model.LastUpdatedBy = CurrentUser.UserId;
                        Result = _Util.Facade.EmployeeFacade.InsertEmployeePtoAccrualRate(model) > 0;
                    } 
                    Message = "Saved successfully.";
                }
                return Json(new { result = true, message = Message });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = "Internal error." });
            }
        }
        public JsonResult GetHours(Guid? UserId, DateTime? SelectedDate)
        {
            bool result = false;
            double total = 0.00;
            if (UserId.HasValue && UserId.Value != Guid.Empty && SelectedDate.HasValue && SelectedDate.Value != new DateTime())
            {
                DateTime StartDate = new DateTime(SelectedDate.Value.Year, SelectedDate.Value.Month, 1).SetZeroHour();
                DateTime EndDate = StartDate.AddMonths(1).AddDays(-1).SetMaxHour();
                HolidayReturnModel data = new ReportsController().HolidayCalculation(StartDate, EndDate);
                result = true;
                total = _Util.Facade.EmployeeFacade.GetTotalPromiseHours(data.Start, data.End, data.Hours, data.WeekEnd, UserId.Value);

            }
            return Json(new { result = result, total = total });
        }
        [Authorize]
        public ActionResult AvailibilityPanel(Guid userId, string date, bool isProfile = false)
        {

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            DateTime dt;
            bool successfullyParsed = DateTime.TryParse(date, out dt);
            if (string.IsNullOrEmpty(date) || !successfullyParsed)
            {
                dt = DateTime.UtcNow.UTCToClientTime();
            }
            var dateend = new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
            int daycount = Convert.ToInt32(dateend.ToString("dd"));
            List<CalListModelList> callist = new List<CalListModelList>();
            List<CalListModel> tlist = new List<CalListModel>();
            ViewBag.UserId = userId;
            ViewBag.isProfile = isProfile;
            ViewBag.CurrentDate = dt.ToString("yyyy/MM/dd");
            ViewBag.MonthName = dt.ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("en-US"));
            List<EmployeeOperations> operations = _Util.Facade.EmployeeFacade.GetAllEmployeeOperationById(userId, dateend);
            List<CompanyHoliday> comHoliday = _Util.Facade.EmployeeFacade.GetCompanyHolidayList(new DateTime(dt.Year, dt.Month, 01), dateend);
            if (comHoliday == null) { comHoliday = new List<CompanyHoliday>(); }
            string startingtime = _Util.Facade.GlobalSettingsFacade.GetAvailabilityStartTime(currentLoggedIn.CompanyId.Value);
            string endingtime = _Util.Facade.GlobalSettingsFacade.GetAvailabilityEndTime(currentLoggedIn.CompanyId.Value);
            string FirstDayOfWeek = _Util.Facade.GlobalSettingsFacade.GetOnlyStrValueFromGlobalSettingByKey("FirstDayOfWeek");
            string HolidayCount = _Util.Facade.GlobalSettingsFacade.GetOnlyStrValueFromGlobalSettingByKey("CustomCalendarColumnHourDuration");
            int j = 1;
            for (int i = 1; i <= daycount; i++)
            {
                var startday = new DateTime(dt.Year, dt.Month, i);
                string day = startday.ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
                string todate = startday.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture("en-US"));
                CalListModel adddate = new CalListModel();
                var govholiday = comHoliday.Where(x => x.Holiday == startday).FirstOrDefault();
                #region Weekly Holiday Calculation
                string WeekEndDay = "";
                if (FirstDayOfWeek.ToLower() == "saturday")
                {

                    if (HolidayCount == "0") { WeekEndDay = ""; }
                    else if (HolidayCount == "1") { WeekEndDay = "friday"; }
                    else if (HolidayCount == "2") { WeekEndDay = "friday,thursday"; }
                    else if (HolidayCount == "3") { WeekEndDay = "friday,thursday,wednesday"; }
                    else if (HolidayCount == "4") { WeekEndDay = "friday,thursday,wednesday,tuesday"; }
                    else if (HolidayCount == "5") { WeekEndDay = "friday,thursday,wednesday,tuesday,monday"; }
                    else if (HolidayCount == "6") { WeekEndDay = "friday,thursday,wednesday,tuesday,monday,sunday"; }
                }
                else if (FirstDayOfWeek.ToLower() == "sunday")
                {
                    if (HolidayCount == "0") { WeekEndDay = ""; }
                    else if (HolidayCount == "1") { WeekEndDay = "saturday"; }
                    else if (HolidayCount == "2") { WeekEndDay = "saturday,friday"; }
                    else if (HolidayCount == "3") { WeekEndDay = "saturday,friday,thursday"; }
                    else if (HolidayCount == "4") { WeekEndDay = "saturday,friday,thursday,wednesday"; }
                    else if (HolidayCount == "5") { WeekEndDay = "saturday,friday,thursday,wednesday,tuesday"; }
                    else if (HolidayCount == "6") { WeekEndDay = "saturday,friday,thursday,wednesday,tuesday,monday"; }
                }
                else if (FirstDayOfWeek.ToLower() == "monday")
                {
                    if (HolidayCount == "0") { WeekEndDay = ""; }
                    else if (HolidayCount == "1") { WeekEndDay = "sunday"; }
                    else if (HolidayCount == "2") { WeekEndDay = "sunday,saturday"; }
                    else if (HolidayCount == "3") { WeekEndDay = "sunday,saturday,friday"; }
                    else if (HolidayCount == "4") { WeekEndDay = "sunday,saturday,friday,thursday"; }
                    else if (HolidayCount == "5") { WeekEndDay = "sunday,saturday,friday,thursday,wednesday"; }
                    else if (HolidayCount == "6") { WeekEndDay = "sunday,saturday,friday,thursday,wednesday,tuesday"; }
                }
                #endregion
                if (govholiday != null)
                {
                    adddate.WorkingTime = "Company Holiday";
                }
                else if (WeekEndDay.Contains(day.ToLower()))
                {
                    adddate.WorkingTime = "Weekend Holiday";
                }
                else
                {
                    var customdate = operations.Where(x => x.SelectedDate == startday).FirstOrDefault();
                    if (customdate != null)
                    {
                        if (customdate.IsDayOff.HasValue && customdate.IsDayOff.Value)
                        {
                            adddate.WorkingTime = "Day Off";
                        }
                        else
                        {
                            DateTime startdateconversion = DateTime.Parse(string.Format("{0} {1}:00.000", startday.ToString("yyyy/MM/dd"), customdate.OperationStartTime.Contains(':') ? customdate.OperationStartTime : startingtime));
                            DateTime enddateconversion = DateTime.Parse(string.Format("{0} {1}:00.000", startday.ToString("yyyy/MM/dd"), customdate.OperationEndTime.Contains(':') ? customdate.OperationEndTime : endingtime));
                            adddate.WorkingTime = string.Format("{0}<br />to<br />{1}", startdateconversion.ToString("hh:mm tt"), enddateconversion.ToString("hh:mm tt"));
                        }
                    }
                    else if (startingtime.Contains(':') && endingtime.Contains(':'))
                    {
                        DateTime dateconversion = DateTime.Parse(string.Format("{0} {1}:00.000", startday.ToString("yyyy/MM/dd"), startingtime));
                        DateTime enddateconversion = DateTime.Parse(string.Format("{0} {1}:00.000", startday.ToString("yyyy/MM/dd"), endingtime));
                        adddate.WorkingTime = string.Format("{0}<br />to<br />{1}", dateconversion.ToString("hh:mm tt"), enddateconversion.ToString("hh:mm tt"));
                    }
                    else
                    {
                        DateTime dateconversion = DateTime.Parse(string.Format("{0} 09:00:00.000", startday.ToString("yyyy/MM/dd")));
                        DateTime enddateconversion = DateTime.Parse(string.Format("{0} 17:00:00.000", startday.ToString("yyyy/MM/dd")));
                        adddate.WorkingTime = string.Format("{0}<br />to<br />{1}", dateconversion.ToString("hh:mm tt"), enddateconversion.ToString("hh:mm tt"));
                    }
                }

                if (day == "Monday") { adddate.SL = (int)DaysMonday.Monday; }
                else if (day == "Tuesday") { adddate.SL = (int)DaysMonday.Tuesday; }
                else if (day == "Wednesday") { adddate.SL = (int)DaysMonday.Wednesday; }
                else if (day == "Thursday") { adddate.SL = (int)DaysMonday.Thursday; }
                else if (day == "Friday") { adddate.SL = (int)DaysMonday.Friday; }
                else if (day == "Saturday") { adddate.SL = (int)DaysMonday.Saturday; }
                else { adddate.SL = (int)DaysMonday.Sunday; }

                adddate.DateName = todate;
                adddate.TimeName = day;
                adddate.DayNumber = i;
                tlist.Add(adddate);
                if (adddate.SL == 7 || i == daycount)
                {
                    int DCount = callist.Count;
                    int tcount = tlist.Count;
                    var mn = startday.ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("en-US"));
                    if (DCount == 0)
                    {
                        var previousend = new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month)).AddMonths(-1);

                        for (int d = 7; d > tcount; d--)
                        {
                            CalListModel add = new CalListModel();
                            string preday = previousend.ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
                            string predate = previousend.ToString("dd-MMM", CultureInfo.CreateSpecificCulture("en-US"));
                            add.DateName = predate;
                            add.TimeName = preday;
                            add.SL = (d - tcount);
                            tlist.Add(add);
                            previousend = new DateTime(previousend.Year, previousend.Month, previousend.Day).AddDays(-1);
                        }
                    }
                    if (DCount == 4 && tcount < 7 && i == daycount)
                    {
                        var nextmonth = new DateTime(dt.Year, dt.Month, 1).AddMonths(1);
                        for (int d = 1; d <= 7 - tcount; d++)
                        {
                            CalListModel add = new CalListModel();
                            string preday = nextmonth.ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
                            string predate = nextmonth.ToString("dd-MMM", CultureInfo.CreateSpecificCulture("en-US"));
                            add.DateName = predate;
                            add.TimeName = preday;
                            add.SL = (d + tcount);
                            tlist.Add(add);
                            nextmonth = new DateTime(nextmonth.Year, nextmonth.Month, nextmonth.Day).AddDays(1);
                        }
                    }
                    else if (DCount == 5 && i == daycount)
                    {
                        var nextmonth = new DateTime(dt.Year, dt.Month, 1).AddMonths(1);
                        for (int d = 1; d <= 7 - tcount; d++)
                        {
                            CalListModel add = new CalListModel();
                            string preday = nextmonth.ToString("dddd", CultureInfo.CreateSpecificCulture("en-US"));
                            string predate = nextmonth.ToString("dd-MMM", CultureInfo.CreateSpecificCulture("en-US"));
                            add.DateName = predate;
                            add.TimeName = preday;
                            add.SL = (d + tcount);
                            tlist.Add(add);
                            nextmonth = new DateTime(nextmonth.Year, nextmonth.Month, nextmonth.Day).AddDays(1);
                        }
                    }
                    var asc = tlist.OrderBy(x => x.SL).ToList();
                    var weekinfo = new CalListModelList() { MonthName = mn, WeekCount = j, WeekList = asc };
                    callist.Add(weekinfo);
                    j++;
                    tlist = new List<CalListModel>();
                }
            }
            return View(callist);
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveEmployeeVehicle(EmployeeVehicle EmpVehicle)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var result = false;
            string message = "";
            try
            {
                if (EmpVehicle.VehicleId == Guid.Empty)
                {
                    var objvehicle = _Util.Facade.VehicleFacade.GetEmployeeVehicleByUserId(EmpVehicle.EmployeeId);
                    if (objvehicle != null)
                    {
                        _Util.Facade.VehicleFacade.DeleteEmployeeVehicleByVehicleId(objvehicle.VehicleId);
                    }
                    return Json(new { result = true, message = "Saved successfully." });

                }
                EmployeeVehicle TemEmV = new EmployeeVehicle();
                string EmployeeNames = "";
                List<EmployeeVehicle> TemEmVList = _Util.Facade.VehicleFacade.GetAllEmployeeVehicleByVehicleId(EmpVehicle.VehicleId);
                if (EmpVehicle.IsAssign == false)
                {
                    if (TemEmVList.Count > 0)
                    {
                        foreach (var item in TemEmVList)
                        {
                            EmployeeNames += item.EmployeeName + ",";
                        }
                        EmployeeNames = (EmployeeNames).Remove(EmployeeNames.Length - 1);
                        EmpVehicle.IsAssign = true;
                        return Json(new { result = false, message = "This vehicle is assigned to " + EmployeeNames + " are you sure you want to re-assign to this user? ", isAssign = EmpVehicle.IsAssign });
                    }
                }
                foreach (var item in TemEmVList)
                {
                    _Util.Facade.VehicleFacade.DeleteAllEmployeeVehicleByVehicleId(item.VehicleId);
                }
                TemEmV = _Util.Facade.VehicleFacade.GetEmployeeVehicleByUserId(EmpVehicle.EmployeeId);
                if (TemEmV == null)
                {
                    TemEmV = new EmployeeVehicle()
                    {
                        AddedBy = CurrentUser.UserId,
                        AddedDate = DateTime.Now.UTCCurrentTime(),
                        CompanyId = CurrentUser.CompanyId.Value,
                        EmployeeId = EmpVehicle.EmployeeId,
                        VehicleId = EmpVehicle.VehicleId,

                    };

                    _Util.Facade.VehicleFacade.InsertEmployeeVehicle(TemEmV);
                    result = true;
                    message = "Saved successfully";

                }
                else
                {
                    TemEmV.VehicleId = EmpVehicle.VehicleId;
                    TemEmV.AddedBy = CurrentUser.UserId;
                    TemEmV.AddedDate = DateTime.Now.UTCCurrentTime();
                    _Util.Facade.VehicleFacade.UpdateEmployeeVehicle(TemEmV);
                    result = true;
                    message = "Saved successfully";
                }


                return Json(new { result = result, message = message, isAssign = EmpVehicle.IsAssign });
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return Json(new { result = false, message = "Internal error." });
            }
        }

    }
}