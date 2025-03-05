using System;
using HS.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using HS.Entities;
using HS.Web.UI.Helper;
using System.Configuration;
using Rotativa.Options;
using System.IO;
using System.Drawing;
using HS.Framework.Utils;

namespace HS.Web.UI.Controllers
{
    public class RecruitController : BaseController
    {
        // GET: Recruit
        public ActionResult Index(bool? IsPdf)
        {
            IsPdf = false;
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            if (IsPdf.HasValue)
            {
                ViewBag.IsPdf = IsPdf.Value;
            }
            return View();
        }

        [Authorize]
        public ActionResult RecruitTab(string UserStatus)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if((CurrentUser.UserRole == LabelHelper.UserTypes.Admin 
                || CurrentUser.UserRole == LabelHelper.UserTypes.SysAdmin) && UserStatus == null)
            {
                List<UserMgmtList> sysmodel = new List<UserMgmtList>(); 
                sysmodel = _Util.Facade.UserLoginFacade.GetAllRecruitUserListByCompanyId(CurrentUser.CompanyId.Value, UserStatus);
                return View("_RecruitTabAdmin", sysmodel);

            }
            
            List<RecruitmentFormEmployee> model = new List<RecruitmentFormEmployee>();
            if (CurrentUser.UserId != new Guid())
            {
                model = _Util.Facade.RecruitFacade.GetAllEmployeeRecruitmentFormsByEmpId(CurrentUser.UserId);
            }
            //foreach(var item in model)
            //{
            //    var form = _Util.Facade.RecruitFacade.GetAllRecruitmentForms();
            //    item.FormName = form.Select(x => x.Name).FirstOrDefault();
            //}

            return View("_RecruitTab",model);
        }
        [Authorize]
        public ActionResult FilterRecruitTabUser(string UserStatus)
        {
            List<UserMgmtList> sysmodel = new List<UserMgmtList>();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if ((CurrentUser.UserRole == LabelHelper.UserTypes.Admin
                || CurrentUser.UserRole == LabelHelper.UserTypes.SysAdmin) && !string.IsNullOrWhiteSpace(UserStatus))
            {
                
                sysmodel = _Util.Facade.UserLoginFacade.GetAllRecruitUserListByCompanyId(CurrentUser.CompanyId.Value, UserStatus);
            }
            return View("_FilterRecruitTabAdmin", sysmodel);
        }
        [Authorize]
        public ActionResult W9Form(bool? IsPdf,Guid? EmpId)
        {
            if (!IsPdf.HasValue)
            {
                IsPdf = false;
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Employee emp = new Employee();
            List<RecruitmentFormEmployee> formlist = new List<RecruitmentFormEmployee>();
            if (EmpId.HasValue)
            {
                formlist = _Util.Facade.RecruitFacade.GetAllEmployeeRecruitmentFormsByEmpId(EmpId.Value);
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmpId.Value);
            }
            else if (CurrentUser.UserId != new Guid())
            {
                formlist = _Util.Facade.RecruitFacade.GetAllEmployeeRecruitmentFormsByEmpId(CurrentUser.UserId);
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            }
            else
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (formlist.Where(x => x.FormName == LabelHelper.RecruitmentForms.W9Form).Count() == 0)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            RecruitmentW9Form W9Form = new RecruitmentW9Form();
            RecruitmentFormEmployee rfe = formlist.Where(x => x.FormName == LabelHelper.RecruitmentForms.W9Form).FirstOrDefault();
            if (rfe.IsSubmitted && !IsPdf.Value)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (rfe.FormId == new Guid())
            {
                W9Form = new RecruitmentW9Form()
                {
                    BusinessName = "",
                    Name = emp.FirstName + " "+emp.LastName ,
                    CCorporation = false,
                    SCorporation = false,
                    Individual = false,
                    Partnership = false,
                    Trust = false,
                    LimitedLiability = false,
                    AccountNumber = "",
                    Address = "",
                    City = emp.City,
                    EIN = "",
                    ExemptPayeeCode = "",
                    FATCAReportingCode = "",
                    FormId = Guid.NewGuid(),
                    SSN = emp.SSN,
                    Other = "",
                    State = emp.State,
                    Zipcode = emp.ZipCode,
                    TaxClassification = ""
                };
                W9Form.Id = _Util.Facade.RecruitFacade.InsertW9Form(W9Form);
                if (W9Form.Id > 0)
                {
                    rfe.FormId = W9Form.FormId;
                    _Util.Facade.RecruitFacade.UpdateRecruitmentFormEmployee(rfe);
                }
            }
            else
            {
                W9Form = _Util.Facade.RecruitFacade.GetW9FormByFormId(rfe.FormId);
            }

            if (IsPdf.HasValue && IsPdf.Value)
            {
                W9Form.IsPdf = true;
                return new ViewAsPdf(W9Form);
            }
            else
            {
                W9Form.IsPdf = false;
                return View(W9Form);

            }
        }
        [Authorize]
        [HttpPost]
        public JsonResult W9Form(RecruitmentW9Form Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Employee emp = new Employee();
            if (CurrentUser.UserId != new Guid() && Model.FormId != new Guid())
            {
                RecruitmentW9Form Form = _Util.Facade.RecruitFacade.GetW9FormByFormId(Model.FormId);
                if (Form == null)
                {
                    return Json(new { result = false, message = "Access denied, Form not found." });
                }
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                if (emp == null)
                {
                    return Json(new { result = false, message = "Access denied, you do not have any w9 form assigned." });
                }
                RecruitmentFormEmployee rfe = _Util.Facade.RecruitFacade.GetRecruitmentFormEmployeeByFormIdAndEmpId(Model.FormId, CurrentUser.UserId);
                if (rfe.IsSubmitted)
                {
                    return Json(new { result = false, message = "Access denied, you already have submited this form. If you need any change please contact system admin." });
                }


                Model.Id = Form.Id;
                if (!_Util.Facade.RecruitFacade.UpdateW9Form(Model))
                {
                    return Json(new { result = false, message = "Internal error. Please contact system admin." });
                }

                rfe.FillDate = DateTime.Now.UTCCurrentTime();
                rfe.IsFillUp = true;
                _Util.Facade.RecruitFacade.UpdateRecruitmentFormEmployee(rfe);



                emp.ZipCode = Model.Zipcode;
                emp.City = Model.City;
                emp.State = Model.State;
            }
            else
            {
                return Json(new { result = false, message = "Access denied, you do not have any w9 form assigned." });
            }

            return Json(new {result= true,message="W9 From saved successfully." });
        }
        
        [Authorize]
        [HttpPost]
        public JsonResult SubmitForm(string FormName)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Employee emp = new Employee();
            List<RecruitmentFormEmployee> formlist = new List<RecruitmentFormEmployee>();
            if (CurrentUser.UserId != new Guid())
            {
                formlist = _Util.Facade.RecruitFacade.GetAllEmployeeRecruitmentFormsByEmpId(CurrentUser.UserId);
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            }
            else
            {
                return Json(new { result = false,message="Access denied. please Contact" }) ;
            }
            RecruitmentFormEmployee form = formlist.Where(x=>x.FormName == FormName).FirstOrDefault();

            if (form == null)
            {
                return Json(new { result = false, message = "Form not found." });
            }
            form.IsSubmitted = true;
            form.SubmitDate = DateTime.Now.UTCCurrentTime();
            bool result = _Util.Facade.RecruitFacade.UpdateRecruitmentFormEmployee(form);

            var fileList = _Util.Facade.HrDocFacade.GetAllUserFilesByCompanyId(CurrentUser.CompanyId.Value, User.Identity.Name);
            if (fileList.Where(x => x.FileDescription == FormName).Count() < 1)
            {

                byte[] applicationPDFData = new byte[] { };

                if (FormName== LabelHelper.RecruitmentForms.W4Form)
                {
                    RecruitmentW4Form W4Form = _Util.Facade.RecruitFacade.GetW4FormByFormId(form.FormId);
                    if (W4Form != null)
                    {
                        W4Form.Ipadd = AppConfig.GetIP;
                        W4Form.UserAg = AppConfig.GetUserAgent;
                        W4Form.Tstamp = DateTime.Now.UTCCurrentTime().ToString("MM/dd/yyyy") + " at " + DateTime.Now.UTCCurrentTime().ToString("hh:mm tt");
                    }
                    W4Form.IsPdf = true;
                    if (!W4Form.SingatureDate.HasValue)
                    {
                        W4Form.SingatureDate = DateTime.Now.UTCCurrentTime();
                    }
                    ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("W4Form", W4Form)
                    {
                        //FileName = "TestView.pdf",
                        PageSize = Rotativa.Options.Size.A4,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 },
                    };
                    applicationPDFData = actionPDF.BuildPdf(ControllerContext);

                }
                else if (FormName == LabelHelper.RecruitmentForms.W9Form)
                {
                    var W9Form = _Util.Facade.RecruitFacade.GetW9FormByFormId(form.FormId);
                    W9Form.IsPdf = true;
                    ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("W9Form", W9Form)
                    {
                        //FileName = "TestView.pdf",
                        PageSize = Rotativa.Options.Size.A4,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 },

                    };
                    applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                }
                else if (FormName == LabelHelper.RecruitmentForms.I9Form)
                {
                    Recruitmenti9Form I9 = _Util.Facade.RecruitFacade.GetI9FormByFormId(form.FormId);
                    if(I9 != null)
                    {
                        I9.IsPdf = true;
                        I9.Ipadd = AppConfig.GetIP;
                        I9.UserAg = AppConfig.GetUserAgent;
                        I9.Tstamp = DateTime.Now.UTCCurrentTime().ToString("MM/dd/yyyy") + " at " + DateTime.Now.UTCCurrentTime().ToString("hh:mm tt");
                    }
                    ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("I9Form", I9)
                    {
                        //FileName = "TestView.pdf",
                        PageSize = Rotativa.Options.Size.A4,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 },

                    };
                    applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                }
                else if(FormName == LabelHelper.RecruitmentForms.DrivingLicense 
                    || FormName == LabelHelper.RecruitmentForms.StateLicenseTX)
                {
                    var Doc = _Util.Facade.RecruitFacade.GetDocFormByFormId(form.FormId);
                    HrDoc hrdoc = new HrDoc()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        FileDescription = FormName,
                        Filename = Doc.FileLocation,
                        UserName = User.Identity.Name,
                        Uploadeddate = DateTime.Now.UTCCurrentTime()
                    };
                    _Util.Facade.HrDocFacade.InsertHrDoc(hrdoc);

                }
                #region file saving part
                if (applicationPDFData.Length > 100)
                {
                    Random rand = new Random();
                    string tempFolderName = ConfigurationManager.AppSettings["File.UserFile"];
                    var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                    tempFolderName = string.Format(tempFolderName, comname);
                    tempFolderName = string.Concat("/", tempFolderName, "/" , CurrentUser.UserId.ToString(), "/" , DateTime.Now.UTCCurrentTime().Month , "-", DateTime.Now.UTCCurrentTime().Day, "/");
                    //filename
                    string filename = string.Concat(rand.Next().ToString(), FormName, ".pdf");
                    //File with location
                    string FullFileName = string.Concat(tempFolderName, filename);
                     
                    string Serverfilename = FileHelper.GetFileFullPath(FullFileName);
                    FileHelper.SaveFile(applicationPDFData, Serverfilename);
                    
                    HrDoc doc = new HrDoc()
                    {
                        Filename = AppConfig.DomainSitePath + FullFileName,
                        CompanyId = CurrentUser.CompanyId.Value,
                        FileDescription = FormName,
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        UserName = User.Identity.Name,
                    };
                    _Util.Facade.HrDocFacade.InsertHrDoc(doc);
                }
                #endregion
            }


            if (result)
            {
                return Json(new { result = true, message = "Your form submitted successfully." });
            }
            

            return Json(new { result = false, message = "Access denied. please Contact" });
        }
        
        [Authorize]
        public ActionResult W4Form(bool? IsPdf, Guid? EmpId)
        {
            if (!IsPdf.HasValue)
            {
                IsPdf = false;
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Employee emp = new Employee();
            List<RecruitmentFormEmployee> formlist = new List<RecruitmentFormEmployee>();
            if (EmpId.HasValue)
            {
                formlist = _Util.Facade.RecruitFacade.GetAllEmployeeRecruitmentFormsByEmpId(EmpId.Value);
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmpId.Value);
            }
            else if (CurrentUser.UserId != new Guid())
            {
                formlist = _Util.Facade.RecruitFacade.GetAllEmployeeRecruitmentFormsByEmpId(CurrentUser.UserId);
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            }
            else
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (formlist.Where(x => x.FormName == LabelHelper.RecruitmentForms.W4Form).Count() == 0)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            RecruitmentW4Form W4Form = new RecruitmentW4Form();
            RecruitmentFormEmployee rfe = formlist.Where(x => x.FormName == LabelHelper.RecruitmentForms.W4Form).FirstOrDefault();
            if (rfe.IsSubmitted && !IsPdf.Value)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }


            if (rfe.FormId == new Guid())
            {
                W4Form = new RecruitmentW4Form()
                {
                    
                    Address = "",
                    City = emp.City, 
                    FormId = Guid.NewGuid(),
                    Id = 0,
                    SSN = emp.SSN, 
                    State = emp.State,
                    Zipcode = emp.ZipCode,
                    AdditionalAmount6 =0.0,
                    AdjustWorkSheet1 ="",
                    AdjustWorkSheet10 = "",
                    AdjustWorkSheet2="",
                    AdjustWorkSheet3="",
                    AdjustWorkSheet4="",
                    AdjustWorkSheet5="",
                    AdjustWorkSheet6="",
                    AdjustWorkSheet7="",
                    AdjustWorkSheet8="",
                    AdjustWorkSheet9="",
                    AllowanceWorksheetA="",
                    AllowanceWorksheetB="",
                    AllowanceWorksheetC="",
                    AllowanceWorksheetD="",
                    AllowanceWorksheetE="",
                    AllowanceWorksheetF="",
                    AllowanceWorksheetG="",
                    AllowanceWorksheetH="",
                    EmployerEIN="",
                    FirstName = emp.FirstName,
                    LastName = emp.FirstName,
                    JobWroksheet1 ="",
                    JobWroksheet2="",
                    JobWroksheet3="",
                    JobWroksheet4="",
                    JobWroksheet5="",
                    JobWroksheet6="",
                    JobWroksheet7="",
                    JobWroksheet8="",
                    JobWroksheet9="",
                    EmployernameAndAddress="",
                    NoTaxLiability7="",
                    Signature="",
                    MiddleInitial="",
                    OfficeCode="",
                    SingatureDate= DateTime.Now.UTCCurrentTime(),
                    TotalAllowance5=0,
                    ReplaceSSNCard4=false,
                    Single=false,
                    MarriadButSeparated=false,
                    Married=false,
                     
                };
                W4Form.Id = _Util.Facade.RecruitFacade.InsertW4Form(W4Form);
                if (W4Form.Id > 0)
                {
                    rfe.FormId = W4Form.FormId;
                    _Util.Facade.RecruitFacade.UpdateRecruitmentFormEmployee(rfe);
                }
            }
            else
            {
                W4Form = _Util.Facade.RecruitFacade.GetW4FormByFormId(rfe.FormId);
            }

            if (W4Form != null && !W4Form.SingatureDate.HasValue)
            {
                W4Form.SingatureDate = new DateTime();
            }
            if (IsPdf.HasValue && IsPdf.Value)
            {
                W4Form.IsPdf = true;
                return new ViewAsPdf(W4Form);
            }
            else
            {
                W4Form.IsPdf = false;
                return View(W4Form);

            }
        }
        [Authorize]
        [HttpPost]
        public JsonResult W4Form(RecruitmentW4Form Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Employee emp = new Employee();
            List<RecruitmentFormEmployee> formlist = new List<RecruitmentFormEmployee>();
            if (CurrentUser.UserId != new Guid() && Model.FormId != new Guid())
            {
                RecruitmentW4Form Form = _Util.Facade.RecruitFacade.GetW4FormByFormId(Model.FormId);
                if (Form == null)
                {
                    return Json(new { result = false, message = "Access denied, Form not found." });
                }
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                if (emp == null)
                {
                    return Json(new { result = false, message = "Access denied, you do not have any w4 form assigned." });
                }

                RecruitmentFormEmployee rfe = _Util.Facade.RecruitFacade.GetRecruitmentFormEmployeeByFormIdAndEmpId(Model.FormId, CurrentUser.UserId);
                if (rfe.IsSubmitted)
                {
                    return Json(new { result = false, message = "Access denied, you already have submited this form. If you need any change please contact system admin." });
                }

                Model.Id = Form.Id;
                if (!_Util.Facade.RecruitFacade.UpdateW4Form(Model))
                {
                    return Json(new { result = false, message = "Internal error. Please contact system admin." });
                }
                
                rfe.FillDate = DateTime.Now.UTCCurrentTime();
                rfe.IsFillUp = true;
                _Util.Facade.RecruitFacade.UpdateRecruitmentFormEmployee(rfe);
                

                emp.ZipCode = Model.Zipcode;
                emp.City = Model.City;
                emp.State = Model.State;
            }
            else
            {
                return Json(new { result = false, message = "Access denied, you do not have any w4 form assigned." });
            }

            return Json(new { result = true, message = "W4 From saved successfully." });
        }

        [Authorize]
        public ActionResult I9Form(bool? IsPdf, Guid? EmpId)
        {
            if (!IsPdf.HasValue)
            {
                IsPdf = false;
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Employee emp = new Employee();
            List<RecruitmentFormEmployee> formlist = new List<RecruitmentFormEmployee>();
            if (EmpId.HasValue)
            {
                formlist = _Util.Facade.RecruitFacade.GetAllEmployeeRecruitmentFormsByEmpId(EmpId.Value);
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmpId.Value);
            }
            else if (CurrentUser.UserId != new Guid())
            {
                formlist = _Util.Facade.RecruitFacade.GetAllEmployeeRecruitmentFormsByEmpId(CurrentUser.UserId);
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            }
            else
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (formlist.Where(x => x.FormName == LabelHelper.RecruitmentForms.I9Form).Count() == 0)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            Recruitmenti9Form I9Form = new Recruitmenti9Form();
            RecruitmentFormEmployee rfe = formlist.Where(x => x.FormName == LabelHelper.RecruitmentForms.I9Form).FirstOrDefault();
            if (rfe.IsSubmitted && !IsPdf.Value)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (rfe.FormId == new Guid())
            {
                I9Form = new Recruitmenti9Form()
                {
                    Id = 0,
                    FormId = Guid.NewGuid(),
                    FirstName =emp.FirstName,
                    LastName =emp.LastName,
                    MiddleInitial ="", 
                    MaidenName = "",
                    DOB = new DateTime(),
                    SSN = emp.SSN,
                    Address="",
                    Apartment = "", 
                    City = emp.City, 
                    State = emp.State, 
                    ZipCode = emp.ZipCode, 
                    USCitizen=false, 
                    NoncitizenNational = false, 
                    LawfulPermanentResident = false, 
                    AlienAuthorizedToWork = false, 
                    UntilExp = false,
                    Signature = "",
                    SignatureDate = new DateTime(),
                    TransSignature = "",
                    TransPrintName = "",
                    TransAddress = "",
                    TransSignaturedate = new DateTime(), 
                    DocTitleListA = "",
                    DoctTitleListB = "",
                    DoctTitleListC = "",
                    IssuingAuthorityListA = "",
                    IssuingAuthorityListB = "",
                    IssuingAuthorityListC = "",
                    Doc1ListA = "",
                    Doc1ListB = "",
                    Doc1ListC = "",
                    Exp1ListA = new DateTime(),
                    Exp1ListB = new DateTime(),
                    Exp1ListC = new DateTime(),
                    Doc2ListA = "",
                    Doc2ListB = "",
                    Doc2ListC = "",
                    Exp2ListA = new DateTime(),
                    Exp2ListB = new DateTime(),
                    Exp2ListC = new DateTime(),
                    BeganEmploymentOn = new DateTime(),
                    AuthRepresentativeSignature ="", 
                    AuthRepresentativeName = "",
                    AuthRepresentativeTitle = "",
                    AuthRepSignatureDate = new DateTime(),
                    OrgNameAndAddress = "",
                    NewName = "",
                    DateOfRehire = new DateTime(),
                    PrevDocTitle = "",
                    PrevDocNo = "",
                    PrevDocExp = new DateTime(),
                    AuthRepSignature2 = "",
                    AuthRepSignatureDate2 = new DateTime(),
                };
                I9Form.Id = _Util.Facade.RecruitFacade.InsertI9Form(I9Form);
                if (I9Form.Id > 0)
                {
                    rfe.FormId = I9Form.FormId;
                    _Util.Facade.RecruitFacade.UpdateRecruitmentFormEmployee(rfe);
                }
            }
            else
            {
                I9Form = _Util.Facade.RecruitFacade.GetI9FormByFormId(rfe.FormId);

                if (!I9Form.DOB.HasValue)
                    I9Form.DOB = new DateTime();
                if(!I9Form.SignatureDate.HasValue)
                    I9Form.SignatureDate = new DateTime();
                if (!I9Form.TransSignaturedate.HasValue)
                    I9Form.TransSignaturedate = new DateTime();
                if (!I9Form.Exp1ListA.HasValue)
                    I9Form.Exp1ListA = new DateTime();
                if (!I9Form.Exp1ListB.HasValue)
                    I9Form.Exp1ListB = new DateTime();
                if (!I9Form.Exp1ListC.HasValue)
                    I9Form.Exp1ListC= new DateTime();
                if (!I9Form.Exp2ListA.HasValue)
                    I9Form.Exp2ListA = new DateTime();
                if (!I9Form.Exp2ListB.HasValue)
                    I9Form.Exp2ListB = new DateTime();
                if (!I9Form.Exp2ListC.HasValue)
                    I9Form.Exp2ListC = new DateTime();
                if (!I9Form.AuthRepSignatureDate.HasValue)
                    I9Form.AuthRepSignatureDate = new DateTime();
                if (!I9Form.DateOfRehire.HasValue)
                    I9Form.DateOfRehire = new DateTime();
                if (!I9Form.PrevDocExp.HasValue)
                    I9Form.PrevDocExp = new DateTime();
                if (!I9Form.AuthRepSignatureDate2.HasValue)
                    I9Form.AuthRepSignatureDate2 = new DateTime();
                if(!I9Form.BeganEmploymentOn.HasValue)
                    I9Form.BeganEmploymentOn = new DateTime();
                if (string.IsNullOrWhiteSpace(I9Form.Signature))
                    I9Form.Signature = "";
            }

             
            if (IsPdf.HasValue && IsPdf.Value)
            {
                I9Form.IsPdf = true;
                return new ViewAsPdf(I9Form);
            }
            else
            {
                I9Form.IsPdf = false;
                return View(I9Form);

            }
        }

        [HttpPost]
        public JsonResult I9Form(Recruitmenti9Form Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Employee emp = new Employee();
            List<RecruitmentFormEmployee> formlist = new List<RecruitmentFormEmployee>();
            if (CurrentUser.UserId != new Guid() && Model.FormId != new Guid())
            {
                Recruitmenti9Form Form = _Util.Facade.RecruitFacade.GetI9FormByFormId(Model.FormId);
                if (Form == null)
                {
                    return Json(new { result = false, message = "Access denied, Form not found." });
                }
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                if (emp == null)
                {
                    return Json(new { result = false, message = "Access denied, you do not have any w4 form assigned." });
                }

                RecruitmentFormEmployee rfe = _Util.Facade.RecruitFacade.GetRecruitmentFormEmployeeByFormIdAndEmpId(Model.FormId, CurrentUser.UserId);
                if (rfe.IsSubmitted)
                {
                    return Json(new { result = false, message = "Access denied, you already have submited this form. If you need any change please contact system admin." });
                }
                Model.Id = Form.Id;
                if (!_Util.Facade.RecruitFacade.UpdateI9Form(Model))
                {
                    return Json(new { result = false, message = "Internal error. Please contact system admin." });
                }

                rfe.FillDate = DateTime.Now.UTCCurrentTime();
                rfe.IsFillUp = true;
                _Util.Facade.RecruitFacade.UpdateRecruitmentFormEmployee(rfe);

            }
            else
            {
                return Json(new { result = false, message = "Access denied, you do not have any w4 form assigned." });
            }

            return Json(new { result = true, message = "W4 From saved successfully." });
        }

        [HttpPost]
        public JsonResult LoadRecruitW4SignatureUpload(string data, int formid)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Company objCompany = new Company();
            Guid CompanyId = new Guid();
            if (User.Identity.IsAuthenticated)
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CompanyId = CurrentUser.CompanyId.Value;
                objCompany = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
            }
            else
            {
                objCompany = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value);
            }
            bool uploadImage = false;
            string filePath = "";
            string[] datasplit = data.Split(',');
            byte[] bytes = Convert.FromBase64String(datasplit[1]);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                string tempFolder = ConfigurationManager.AppSettings["File.CustomerSignatureFile"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                var FtempFolderName = string.Format(tempFolder, comname) + formid + "Signature";
                Random rand = new Random();
                string FileName = rand.Next().ToString();
                FileName += "-___" + "Signature.png";
                string tempFolderPath = Server.MapPath("~/" + FtempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        image.Save(Path.Combine(tempFolderPath, FileName));
                        uploadImage = true;
                    }
                    catch (Exception)
                    {

                    }
                }
                filePath = string.Concat("/", FtempFolderName, "/", FileName);
            }
            var serverFile = Server.MapPath(filePath);
            if (!System.IO.File.Exists(serverFile))
            {
                return Json(new { result = true, message = "File not exsists" });
            }
            var objW4Form = _Util.Facade.RecruitFacade.GetW4FormById(formid);
            if (objW4Form != null)
            {
                objW4Form.Signature = AppConfig.DomainSitePath + filePath;
                _Util.Facade.RecruitFacade.UpdateW4Form(objW4Form);
            }
            return Json(new { uploadImage = uploadImage, UploadFilePath = AppConfig.DomainSitePath + filePath, formid = formid }, "text/html");
        }

        [Authorize]
        public ActionResult UploadDocuments(bool? IsPdf, Guid? EmpId,bool? IsDrivingLicnese,bool? IsStateLicTx)
        {
            RecruitmentDocForm DocForm = new RecruitmentDocForm();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (IsDrivingLicnese.HasValue && IsDrivingLicnese.Value)
            {
                DocForm.FormFor = LabelHelper.RecruitmentForms.DrivingLicense;
            }
            else if(IsStateLicTx.HasValue && IsStateLicTx.Value)
            {
                DocForm.FormFor = LabelHelper.RecruitmentForms.StateLicenseTX;
            }
            else
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (!IsPdf.HasValue)
            {
                IsPdf = false;
            }
            Employee emp = new Employee();
            List<RecruitmentFormEmployee> formlist = new List<RecruitmentFormEmployee>();
            if (EmpId.HasValue)
            {
                formlist = _Util.Facade.RecruitFacade.GetAllEmployeeRecruitmentFormsByEmpId(EmpId.Value);
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmpId.Value);
            }
            else if (CurrentUser.UserId != new Guid())
            {
                formlist = _Util.Facade.RecruitFacade.GetAllEmployeeRecruitmentFormsByEmpId(CurrentUser.UserId);
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            }
            else
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (formlist.Where(x => x.FormName == DocForm.FormFor).Count() == 0)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            
            RecruitmentFormEmployee rfe = formlist.Where(x => x.FormName == DocForm.FormFor).FirstOrDefault();
            if (rfe.IsSubmitted && !IsPdf.Value)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (rfe.FormId == new Guid())
            {

                DocForm.Name = DocForm.FormFor;
                DocForm.FormId = Guid.NewGuid();
                DocForm.Id = 0;
                DocForm.FileLocation = "";
                DocForm.Id = _Util.Facade.RecruitFacade.InsertDocForm(DocForm);
                if (DocForm.Id > 0)
                {
                    rfe.FormId = DocForm.FormId;
                    _Util.Facade.RecruitFacade.UpdateRecruitmentFormEmployee(rfe);
                }
            }
            else
            {
                DocForm = _Util.Facade.RecruitFacade.GetDocFormByFormId(rfe.FormId);

                if (IsPdf.HasValue && IsPdf.Value)
                {
                    HrDoc Hrd = _Util.Facade.HrDocFacade.GetHrDocByUsernameDescriptionAndCompanyId(User.Identity.Name, DocForm.FormFor, CurrentUser.CompanyId.Value);
                    if (DocForm != null && DocForm.Id > 0 && !string.IsNullOrWhiteSpace(DocForm.FileLocation))
                    {
                        byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~" + DocForm.FileLocation));
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, DocForm.FileLocation);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return View(DocForm); 
        } 

        [Authorize]
        [HttpPost]
        public JsonResult UploadDocuments(RecruitmentDocForm Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Employee emp = new Employee();
            if(Model.Name!= LabelHelper.RecruitmentForms.StateLicenseTX && Model.Name != LabelHelper.RecruitmentForms.DrivingLicense)
            {
                return Json(new { result = false,message="Form type not found, please try again later." });
            }
            if (CurrentUser.UserId != new Guid() && Model.FormId != new Guid())
            {
                RecruitmentDocForm Form = _Util.Facade.RecruitFacade.GetDocFormByFormId(Model.FormId);
                if (Form == null)
                {
                    return Json(new { result = false, message = "Access denied, Form not found." });
                }
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                if (emp == null)
                {
                    return Json(new { result = false, message = "Access denied, you do not have any w9 form assigned." });
                }
                RecruitmentFormEmployee rfe = _Util.Facade.RecruitFacade.GetRecruitmentFormEmployeeByFormIdAndEmpId(Model.FormId, CurrentUser.UserId);
                if (rfe.IsSubmitted)
                {
                    return Json(new { result = false, message = "Access denied, you already have submited this form. If you need any change please contact system admin." });
                }


                Model.Id = Form.Id;
                if (!_Util.Facade.RecruitFacade.UpdateDocForm(Model))
                {
                    return Json(new { result = false, message = "Internal error. Please contact system admin." });
                }

                rfe.FillDate = DateTime.Now.UTCCurrentTime();
                rfe.IsFillUp = true;
                _Util.Facade.RecruitFacade.UpdateRecruitmentFormEmployee(rfe);
            }
            else
            {
                return Json(new { result = false, message = "Access denied." });
            }
            return Json(new { result = true, message = "Saved successfully." });
        }
         
        [Authorize]
        public JsonResult SubmitLicense(string FormName)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Employee emp = new Employee();
            List<RecruitmentFormEmployee> formlist = new List<RecruitmentFormEmployee>();
            if (CurrentUser.UserId != new Guid())
            {
                formlist = _Util.Facade.RecruitFacade.GetAllEmployeeRecruitmentFormsByEmpId(CurrentUser.UserId);
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            }
            else
            {
                return Json(new { result = false, message = "Access denied. please Contact" });
            }
            RecruitmentFormEmployee form = formlist.Where(x => x.FormName == FormName).FirstOrDefault();

            if (form == null)
            {
                return Json(new { result = false, message = "Form not found." });
            }
            form.IsSubmitted = true;
            form.SubmitDate = DateTime.Now.UTCCurrentTime();
            bool result = _Util.Facade.RecruitFacade.UpdateRecruitmentFormEmployee(form);
            return Json(true);
        }

        [HttpPost]
        public JsonResult LoadRecruitI9SignatureUpload(string data, int formid)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Company objCompany = new Company();
            Guid CompanyId = new Guid();
            if (User.Identity.IsAuthenticated)
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CompanyId = CurrentUser.CompanyId.Value;
                objCompany = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
            }
            else
            {
                objCompany = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentLoggedInUser.CompanyId.Value);
            }

            bool uploadImage = false;
            string filePath = "";
            string[] datasplit = data.Split(',');
            byte[] bytes = Convert.FromBase64String(datasplit[1]);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                string tempFolder = ConfigurationManager.AppSettings["File.CustomerSignatureFile"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                var FtempFolderName = string.Format(tempFolder, comname) + formid + "Signature";
                Random rand = new Random();
                string FileName = rand.Next().ToString();
                FileName += "-___" + "Signature.png";
                string tempFolderPath = Server.MapPath("~/" + FtempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        image.Save(Path.Combine(tempFolderPath, FileName));
                        uploadImage = true;
                    }
                    catch (Exception)
                    {

                    }
                }
                filePath = string.Concat("/", FtempFolderName, "/", FileName);
            }
            var serverFile = Server.MapPath(filePath);
            if (!System.IO.File.Exists(serverFile))
            {
                return Json(new { result = true, message = "File not exsists" });
            }
            var objI9Form = _Util.Facade.RecruitFacade.GetI9FormById(formid);
            if (objI9Form != null)
            {
                objI9Form.Signature = AppConfig.DomainSitePath + filePath;
                _Util.Facade.RecruitFacade.UpdateI9Form(objI9Form);
            }
            return Json(new { uploadImage = uploadImage, UploadFilePath = AppConfig.DomainSitePath + filePath, formid = formid }, "text/html");
        }
    }
}