using HS.Entities;
using HS.Entities.Custom;
using HS.Framework;
using HS.Web.UI.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using static HS.Framework.Utils.SMSTemplateKey;

namespace HS.Web.UI.Controllers
{
    public class NotesController : BaseController
    {
        // GET: Notes
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
        public ActionResult NotesPartial(Guid customerid, string pageno, string pagesize,DateTime? StartDate,DateTime? EndDate,string SearchText)
        {
            List<CustomerNote> customerNote = new List<CustomerNote>();
            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerNotesTab ))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if(!string.IsNullOrWhiteSpace(pageno) && pageno != "undefined" && !string.IsNullOrWhiteSpace(pagesize) && pagesize != "undefined")
            {
                customerNote = _Util.Facade.NotesFacade.GetAllCustomerNotesByCustomerId(customerid, currentLoggedIn.CompanyId.Value, Convert.ToInt32(pageno), Convert.ToInt32(pagesize),StartDate,EndDate,SearchText);
            }
            else
            {
                pageno = "1";
                customerNote = _Util.Facade.NotesFacade.GetAllCustomerNotesByCustomerId(customerid, currentLoggedIn.CompanyId.Value, Convert.ToInt32(pageno), Convert.ToInt32(pagesize),StartDate,EndDate,SearchText);
            }

            ViewBag.StartDate = "";
            ViewBag.EndDate = "";
            ViewBag.SearchText = "";
            if (StartDate != null && StartDate != new DateTime())
            {
                ViewBag.StartDate = StartDate.Value.ToString("MM/dd/yy");
            }
            if (EndDate != null && EndDate != new DateTime())
            {
                ViewBag.EndDate = EndDate.Value.ToString("MM/dd/yy");
            }
            if(!string.IsNullOrEmpty(SearchText))
            {
                ViewBag.SearchText = SearchText;
            }

            ViewBag.PageNumber = Convert.ToInt32(pageno);
            ViewBag.OutOfNumber = 0;
            int PageSize = Convert.ToInt32(pagesize);

            if (customerNote.Count() > 0)
            {
                foreach (var item in customerNote)
                {
                    ViewBag.OutOfNumber = item.TotalNoteCount;
                    break;
                }
            }

            if (ViewBag.PageNumber * PageSize > ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / PageSize);
            //ViewBag.CustomerId = customerid;
            ViewBag.CustomerFollowUpPartialCustomerId = customerid;
            return PartialView("_NotesPartial", customerNote);
        }
        [Authorize]
        public ActionResult AddNotes(int? id, int customerid,string from)
        {
            CustomerNote model = new CustomerNote();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (id > 0)
            {
                //[Shariful-17-9-19]
                model = _Util.Facade.NotesFacade.GetAllNotesByCustomerNoteId(id.Value);
                var AssignNoteObj = _Util.Facade.NoteAssignFacade.GetAssignedNotesByNoteId(model.Id);
                if (AssignNoteObj != null)
                {
                    model.EmployeeID = AssignNoteObj.EmployeeId;
                }
                //[~Shariful-17-9-19]
                //model = _Util.Facade.NotesFacade.GetAllNotesByCustomerNoteId(id.Value);
                model.AssignEmpList = _Util.Facade.NoteAssignFacade.GetAllAssignCustomerNoteListByNoteId(id.Value);
            }
            else
            {
                //model = new CustomerNote();
                model.IsEmail = false;
                model.IsText = false;
                model.AssignEmpList = new List<AssignEmployeeCustomerNote>();
            }

            ViewBag.From = "";
            if(!string.IsNullOrEmpty(from))
            {
                ViewBag.From = from;
            }

            ViewBag.CustomerId = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid).CustomerId.ToString();
            var cusID = new Guid(ViewBag.CustomerId);
            //ViewBag.Notes = _Util.Facade.NotesFacade.GetAllCustomerNoteByCompanyId(currentLoggedIn.CompanyId.Value).Select(x =>
            //                    new SelectListItem()
            //                    {
            //                        Text = x.Notes.ToString(),
            //                        Value = x.Id.ToString()
            //                    }).ToList();
            //ViewBag.ReminderDate = _Util.Facade.NotesFacade.GetAllCustomerNoteByCompanyId(currentLoggedIn.CompanyId.Value).Select(x =>
            //                    new SelectListItem()
            //                    {
            //                        Text = x.ReminderDate.ToString(),
            //                        Value = x.Id.ToString()
            //                    }).ToList();

            List<SelectListItem> EmployeeList = new List<SelectListItem>();
            EmployeeList.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                                      Value = x.UserId.ToString(),
                                      Selected = (model.AssignEmpList.Where(y => y.AssignedEmpId == x.UserId).Count() > 0)
                                  }).ToList());
            ViewBag.EmployeeName = EmployeeList;
            var personalObject = _Util.Facade.CustomerFacade.GetAllResposiblePersonByCompanyIdandCustomerId(cusID);
            foreach (var item in personalObject)
            {
                //var obj1 = item.ResponsiblePerson1;
                //var obj2 = item.ResponsiblePerson2;
                //var obj3 = item.ResponsiblePerson3;
                //var obj4 = item.ResponsiblePerson4;
                //var email1 = item.ResponsiblePersonEmail1;
                //var email2 = item.ResponsiblePersonEmail2;
                //var email3 = item.ResponsiblePersonEmail3;
                //var email4 = item.ResponsiblePersonEmail4;
                //List<SelectListItem> items = new List<SelectListItem>();

                //if (obj1 != "" && email1 != "")
                //{
                //    items.Add(new SelectListItem { Text = obj1, Value = email1 });
                //}
                //if (obj2 != "" && email2 != "")
                //{
                //    items.Add(new SelectListItem { Text = obj2, Value = email2 });
                //}
                //if (obj3 != "" && email3 != "")
                //{
                //    items.Add(new SelectListItem { Text = obj3, Value = email3 });
                //}
                //if (obj4 != "" && email4 != "")
                //{
                //    items.Add(new SelectListItem { Text = obj4, Value = email4 });
                //}
                //if (items.Count > 0)
                //{
                //    ViewBag.person = items;
                //}

                //model.PersonCount = items.Count;

            }
            List<SelectListItem> SalesPersonList = new List<SelectListItem>();
            if (currentLoggedIn.UserRole == "Admin" || currentLoggedIn.UserRole == "SysAdmin")
            {
                var objcurrentlog = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                //SalesPersonList.Add (new SelectListItem()
                //{
                //    Text = "Please Select One",
                //    Value = ""
                //});

                //SalesPersonList.AddRange(_Util.Facade.EmployeeFacade.GetALLEmployeeByCompanyIdAndIsRecruted(currentLoggedIn.CompanyId.Value).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Select(x =>
                //      new SelectListItem()
                //      {
                //          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                //          Value = x.UserId.ToString(),
                //          Selected = (model.AssignEmpList.Where(y => y.AssignedEmpId == x.UserId).Count() > 0)
                //      }).ToList());
                //ViewBag.SalesPersonList = SalesPersonList.OrderBy(x => x.Text).ToList();
                if (string.IsNullOrWhiteSpace(model.EmpId))
                {
                    model.EmpId = objcurrentlog.UserId.ToString();
                }
            }
            //else
            //{
            //    var objEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
            //    if (objEmp != null)
            //    {
            //        SalesPersonList.Add(new SelectListItem()
            //        {
            //            Text = objEmp.FirstName.ToString() + " " + objEmp.LastName.ToString(),
            //            Value = currentLoggedIn.UserId.ToString()
            //        });
            //        ViewBag.SalesPersonList = SalesPersonList;
            //    }
            //}
            List<Employee> empLsit = _Util.Facade.EmployeeFacade.GetCurrentEmployeeListByCompanyId(currentLoggedIn.CompanyId.Value);
            SalesPersonList.Add(new SelectListItem()
                {
                    Text = "Please Select One",
                    Value = "-1"
                });
            if (empLsit!=null && empLsit.Count()>0)
            {
                SalesPersonList.AddRange(empLsit.OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Select(x =>
                      new SelectListItem()
                      {
                          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                          Value = x.UserId.ToString(),
                          Selected = (model.AssignEmpList.Where(y => y.AssignedEmpId == x.UserId).Count() > 0)
                      }).ToList());
            }
            List<TeamSetting> teamList = _Util.Facade.UserLoginFacade.GetAllTeam();
            if (teamList == null && teamList.Count() == 0)
            {
                teamList = new List<TeamSetting>();
            }
            ViewBag.TeamList = teamList;
            ViewBag.SalesPersonList = SalesPersonList.OrderBy(x => x.Value.ToString() != "-1").ThenBy(x => x.Text).ToList();
            ViewBag.NoteTypeList = _Util.Facade.LookupFacade.GetLookupByKey("NoteType").OrderBy(x => x.DataValue.ToString() != "-1").ThenBy(x => x.DisplayText).ToList();
            return PartialView("AddNotes", model);
        }
        [HttpPost]
        public JsonResult ChangeReminderStatus(int id, string isActive)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            

            CustomerNote cusFile = _Util.Facade.NotesFacade.GetById(id);

            if (cusFile != null)
            {
                if (cusFile.IsShedule == false)
                {
                cusFile.IsShedule= true;
            }
            else
            {
                cusFile.IsShedule = false;

            }

                //if (cusFile.IsShedule == false)
                //{
                //    cusFile.IsShedule = true;
                //}
                //else
                //{
                //    cusFile.IsShedule = false;
                //}
                _Util.Facade.NotesFacade.UpdateNotes(cusFile);
                if (cusFile.IsShedule.Value)
                {
                    base.AddUserActivityForCustomer($"Task #{id} schedule is Active. </br> ", LabelHelper.ActivityAction.UpdateRemineder, cusFile.CustomerId, null, null);                    
                }
                else
                {
                    base.AddUserActivityForCustomer($"Task #{id} schedule is In-active. </br> ", LabelHelper.ActivityAction.UpdateRemineder, cusFile.CustomerId, null, null);
                }
                result = true;
            }

            return Json(new { result = result });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddNotes(CustomerNote cn, NoteAssign noteassign)
        {
            bool result = false;

            var currentlogin = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            cn.CompanyId = currentlogin.CompanyId.Value;
            cn.IsFollowUp = false;
            EmailToEmployeeFromCustomerNote MailtoEmployee = new EmailToEmployeeFromCustomerNote();
            ReminderSms _remindersms = new ReminderSms();
            
            if (cn.IsFollowUp != true)
            {
                if (cn.Id > 0)
                {
                    string notemsg = "";
                    CustomerNote cnote = _Util.Facade.NotesFacade.GetById(cn.Id);
                    
                    if (cnote.CompanyId != currentlogin.CompanyId.Value)
                    {
                        return Json(new { result = false,message="Invalid company selected" });
                    }
                    cn.CreatedDate = cnote.CreatedDate;
                    cn.CreatedBy = cnote.CreatedBy;
                    cn.CreatedByUid = cnote.CreatedByUid;
                    if(cnote.NoteType != cn.NoteType)
                    {
                        notemsg += "NoteType is updated from " + cnote.NoteType + " to " + cn.NoteType+"</br>";
                    }
                    if (cnote.Notes != cn.Notes)
                    {
                        notemsg += "Notes is updated from " + cnote.Notes + " to " + cn.Notes + "</br>";
                    }
                    if (cnote.IsEmail != cn.IsEmail)
                    {
                        notemsg += "Email Note is updated from " + cnote.IsEmail + " to " + cn.IsEmail + "</br>";
                    }
                    if (cnote.IsText != cn.IsText)
                    {
                        notemsg += "Text Note is updated from " + cnote.IsText + " to " + cn.IsText + "</br>";
                    }
                    if (cnote.IsPin != cn.IsPin)
                    {
                        notemsg += "Pin To Top is updated from " + cnote.IsPin + " to " + cn.IsPin + "</br>";
                    }
                    if (cnote.IsOverview != cn.IsOverview)
                    {
                        notemsg += "Show in Overview is updated from " + cnote.IsOverview + " to " + cn.IsOverview + "</br>";
                    }
                    result = _Util.Facade.NotesFacade.UpdateNotes(cn);
                    
                        base.AddUserActivityForCustomer("Note is Updated #NoteId:" + cn.Id + "</br>" + notemsg, LabelHelper.ActivityAction.UpdateNote, cn.CustomerId, null, null);

                   
                    //else
                    //{
                    //    base.AddUserActivityForCustomer("Note is Updated #NoteId:" + cn.Id + "</br>" + "Notefor is Updated", LabelHelper.ActivityAction.UpdateNote, cn.CustomerId, null, null);

                    //}
                    var delitem = _Util.Facade.NoteAssignFacade.DeleteAllAssignNoteByNoteId(cn.Id);
                }
                else
                {
                    cn.CreatedDate = DateTime.Now.UTCCurrentTime();
                    cn.CreatedBy = User.Identity.Name;
                    cn.CreatedByUid = currentlogin.UserId;

                    result = _Util.Facade.NotesFacade.InsertCustomerNote(cn) > 0;
                    if (base.IsPermitted(UserPermissions.CustomerPermissions.CustomerNoteSendToGlobalMail))
                    {
                        if(result)
                        {
                            Customer objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(cn.CustomerId);
                            GlobalSetting gloObj = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("EmailForSendNote",currentlogin.CompanyId.Value);
                            MailtoEmployee.CustomerName = objcus.FirstName + " " + objcus.LastName;
                            MailtoEmployee.Notes = cn.NoteType + ":" + cn.Notes;
                            MailtoEmployee.ToEmail = gloObj.Value;
                            _Util.Facade.MailFacade.EmailToEmployeeFromCustomerNote(MailtoEmployee, currentlogin.CompanyId.Value);
                        }
                    }
                        base.AddUserActivityForCustomer("New Note is Added #Id:"+cn.Id+" </br> NoteType:"+cn.NoteType+"</br> Notes:"+cn.Notes , LabelHelper.ActivityAction.AddNote, cn.CustomerId, null,null);
                    CustomerSnapshot objEmailNoteToResponsiblePerson = new CustomerSnapshot()
                        {
                            CustomerId = cn.CustomerId,
                            CompanyId = currentlogin.CompanyId.Value,
                            Description = "Email sent successfully to selected employee",
                            Logdate = DateTime.Now.UTCCurrentTime(),
                            Updatedby = currentlogin.Identity.Name,
                            Type = "CustomerMailHistory"
                        };
                        _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEmailNoteToResponsiblePerson);
                }

                if (result == true)
                {
                    if (noteassign.EmployeeIdVal != null && noteassign.EmployeeIdVal.Length > 0)
                    {
                        foreach (var item1 in noteassign.EmployeeIdVal)
                        {
                            NoteAssign objassign = new NoteAssign()
                            {
                                NoteId = cn.Id,
                                EmployeeId = new Guid(item1)
                            };
                            
                            _Util.Facade.NoteAssignFacade.InsertNoteAssign(objassign);

                            Customer objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(cn.CustomerId);
                           
                            //EmailToEmployeeFromCustomerNote MailtoEmployee = new EmailToEmployeeFromCustomerNote();
                            MailtoEmployee.CustomerName = objcus.FirstName + " " + objcus.LastName;
                            if(cn.NoteType != "-1")
                            {
                                MailtoEmployee.Notes = cn.NoteType + ":" + cn.Notes;
                            }
                            else
                            {
                                MailtoEmployee.Notes = cn.Notes;
                            }

                            if (objassign != null && objassign.EmployeeId != Guid.Empty)
                            {
                                Employee empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(objassign.EmployeeId);
                                Customer objcustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(objassign.EmployeeId);
                                Employee _createdByName = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(cn.CreatedByUid);
                                string cusName = objcus.FirstName + " " + objcus.LastName;
                                _remindersms.CusId = objcus.Id;
                                _remindersms.CustomerName = cusName;
                                var NoteTypeVal = _Util.Facade.LookupFacade.GetLookupByKeyAndValueAndCompanyId("NoteType", cn.NoteType, currentlogin.CompanyId.Value);
                                _remindersms.NoteType = NoteTypeVal != null && !string.IsNullOrWhiteSpace(NoteTypeVal.DataValue) && NoteTypeVal.DataValue != "-1" ? NoteTypeVal.DisplayText : "";
                                _remindersms.Message = cn.Notes;
                                if (cn.ReminderEndDate != null && cn.ReminderEndDate != new DateTime())
                                {
                                    _remindersms.AttnBy = cn.ReminderEndDate.Value.UTCToClientTime().ToString("M/dd/yy");
                                }
                                _remindersms.CreatedBy = _createdByName!= null? _createdByName.FirstName + " " + _createdByName.LastName : "";
                                _remindersms.CreatedDate = cn.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                                _remindersms.CompanyName = currentlogin.CompanyName;
                                if (empobj != null)
                                {
                                    if (cn.IsEmail == true && empobj.Email.IsValidEmailAddress())
                                    {
                                        MailtoEmployee.ToEmail = empobj.Email;
                                        MailtoEmployee.EmailReciver = empobj.FirstName + " " + empobj.LastName;
                                        _Util.Facade.MailFacade.EmailToEmployeeFromCustomerNote(MailtoEmployee, currentlogin.CompanyId.Value);
                                    }
                                    if (cn.IsText == true && !string.IsNullOrWhiteSpace(empobj.Phone))
                                    {
                                        List<string> receiverlist = new List<string>();
                                        receiverlist.Add(empobj.Phone);
                                        _Util.Facade.SMSFacade.ReminderFollowupSMS(cn.CompanyId,currentlogin.UserId, _remindersms, receiverlist);
                                    }
                                }
                                else if (objcustomer != null)
                                {
                                    if (cn.IsEmail == true && objcustomer.EmailAddress.IsValidEmailAddress())
                                    {
                                        MailtoEmployee.ToEmail = objcustomer.EmailAddress;
                                        MailtoEmployee.EmailReciver = objcustomer.FirstName + " " + objcustomer.LastName;
                                        _Util.Facade.MailFacade.EmailToEmployeeFromCustomerNote(MailtoEmployee, currentlogin.CompanyId.Value);
                                    }
                                    if (cn.IsText == true && !string.IsNullOrWhiteSpace(objcustomer.PrimaryPhone))
                                    {
                                        List<string> receiverlist = new List<string>();
                                        receiverlist.Add(objcustomer.PrimaryPhone);
                                        _Util.Facade.SMSFacade.ReminderFollowupSMS(cn.CompanyId,currentlogin.UserId, _remindersms, receiverlist);
                                    }
                                }
                            }
                        }
                    }
                }

                return Json(result);
            }
            else
            {
                return Json(false);
            }
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteCustomerNote(int? id ,Guid? CustomerId)
        {
            if (id.HasValue)
            {
                var customernote = _Util.Facade.NotesFacade.DeleteCustomerNote(id.Value);
                base.AddUserActivityForCustomer("Note is deleted #Id:"+ id, LabelHelper.ActivityAction.Delete, CustomerId, null, null);

            }
            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AssignEmployeeSendEmail(List<string> employee, int id)
        {
            bool result = false;
            var currentlogin = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var objnote = _Util.Facade.NotesFacade.GetNotesById(id);
            var objnoteass = _Util.Facade.NoteAssignFacade.GetAllAssignCustomerNoteListByNoteId(id);
            if(objnote != null)
            {
                var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(objnote.CustomerId);
                if(objcus != null)
                {
                    if(objnoteass != null)
                    {
                        foreach(var item1 in objnoteass)
                        {
                            var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item1.AssignedEmpId);
                            if(empobj != null)
                            {
                                EmailToEmployeeFromCustomerNote MailtoEmployee = new EmailToEmployeeFromCustomerNote();
                                MailtoEmployee.CustomerName = objcus.FirstName + " " + objcus.LastName;
                                MailtoEmployee.Notes = objnote.Notes;
                                MailtoEmployee.ToEmail = empobj.UserName;
                                MailtoEmployee.EmailReciver = empobj.FirstName + " " + empobj.LastName;
                                result = _Util.Facade.MailFacade.EmailToEmployeeFromCustomerNote(MailtoEmployee, currentlogin.CompanyId.Value);
                            }
                        }
                    }
                }
                
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AssignEmployeeFollowSendEmail(List<string> employee, int id)
        {
            bool result = false;
            var currentlogin = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var objnote = _Util.Facade.NotesFacade.GetNotesById(id);
            var objnoteass = _Util.Facade.NoteAssignFacade.GetAllAssignCustomerNoteListByNoteId(id);
            if (objnote != null)
            {
                var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(objnote.CustomerId);
                Employee _createdByName = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(objnote.CreatedByUid);
                if (objcus != null)
                {
                    if (objnoteass != null)
                    {
                        foreach (var item1 in objnoteass)
                        {
                            var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item1.AssignedEmpId);
                            if (empobj != null)
                            {
                                EmailToEmployeeFromFollowUpNote MailtoEmployee = new EmailToEmployeeFromFollowUpNote();
                                MailtoEmployee.CustomerName = objcus.FirstName + " " + objcus.LastName;
                                MailtoEmployee.EmailBody = objnote.Notes;
                                MailtoEmployee.ToEmail = empobj.Email;
                                MailtoEmployee.AssignPersonName = empobj.FirstName + " " + empobj.LastName;
                                if (IsPermitted(UserPermissions.CustomerPermissions.ShowReminderCompletedDateOnAddReminder) && objnote.ReminderEndDate != null && objnote.ReminderEndDate != new DateTime())
                                {
                                    MailtoEmployee.AttnBy = objnote.ReminderEndDate.Value.UTCToClientTime().ToString("M/dd/yy");
                                }
                                MailtoEmployee.CreatedOn = objnote.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                                if (objcus != null)
                                {
                                    MailtoEmployee.CustomerIntId = objcus.Id;
                                }
                                if (_createdByName != null)
                                {
                                    MailtoEmployee.CreatedByName = _createdByName.FirstName + " " + _createdByName.LastName;
                                }
                                result = _Util.Facade.MailFacade.EmailToEmployeeFromFollowUpNotes(MailtoEmployee, currentlogin.CompanyId.Value);
                            }
                        }
                    }
                }

            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AssignEmployeeSendText(int id)
        {
            bool result = false;
            ArrayList phnString = new ArrayList();
            var currentlogin = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var objnote = _Util.Facade.NotesFacade.GetNotesById(id);
            var objnoteass = _Util.Facade.NoteAssignFacade.GetAllAssignCustomerNoteListByNoteId(id);
            if (objnote != null)
            {
                var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(objnote.CustomerId);
                if (objcus != null)
                {
                    if (objnoteass != null)
                    {
                        foreach (var item1 in objnoteass)
                        {
                            var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(item1.AssignedEmpId);
                            if (empobj != null)
                            {
                                if(empobj.Phone != null)
                                {
                                    phnString.Add("tel:" + empobj.Phone);
                                }
                            }
                        }
                        result = true;
                    }
                }
            }
            return Json(new { result = result, phnString = phnString });
        }

        [Authorize]
        public ActionResult RecruitUserNote(int id)
        {
            List<EmployeeNote> ListEmployeeNote = new List<EmployeeNote>();
            var currentlogin = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ViewBag.empid = id;
            var employeeobj = _Util.Facade.EmployeeFacade.GetEmployeeById(id);
            if(employeeobj != null)
            {
                ListEmployeeNote = _Util.Facade.EmployeeFacade.GetAllEmployeeNoteByCompanyIdAndEmployeeId(currentlogin.CompanyId.Value, employeeobj.UserId);
            }
            return View("_RecruitUserNote", ListEmployeeNote);
        }

        [Authorize]
        public ActionResult AddRecruitUserNote(int? id, int empid)
        {
            var currentlogin = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            EmployeeNote model;
            if (id > 0)
            {
                model = _Util.Facade.EmployeeFacade.GetEmployeeNoteById(id.Value);
            }
            else
            {
                model = new EmployeeNote();
                model.IsEmail = false;
                model.IsText = false;
            }
            return View("_AddRecruitUserNote", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddRecruitUserNote(EmployeeNote en)
        {
            bool result = false;
            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeById(en.EmpId);
            int empid = 0;
            var currentlogin = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if(en.Id > 0)
            {
                en.CompanyId = currentlogin.CompanyId.Value;
                en.EmployeeId = objemp.UserId;
                en.CreatedDate = DateTime.Now.UTCCurrentTime();
                empid = objemp.Id;
                en.IsActive = true;
                en.CreatedBy = objemp.FirstName + " " + objemp.LastName;
                result = _Util.Facade.EmployeeFacade.UpdateEmployeeNote(en);
            }
            else
            {
                if(objemp != null)
                {
                    EmployeeNote objempnote = new EmployeeNote()
                    {
                        EmployeeId = objemp.UserId,
                        CompanyId = currentlogin.CompanyId.Value,
                        Notes = en.Notes,
                        ReminderDate = en.ReminderDate,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        IsEmail = en.IsEmail,
                        IsText = en.IsText,
                        IsShedule = false,
                        IsFollowUp = false,
                        IsActive = true,
                        CreatedBy = objemp.FirstName + " " + objemp.LastName,
                        IsClose = false
                    };
                    result = _Util.Facade.EmployeeFacade.InsertEmployeeNote(objempnote) > 0;
                    empid = objemp.Id;
                }
            }
            return Json(new { result = result, empid = empid });
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteEmployeeNote(int Id, int empid)
        {
            bool result = false;
            _Util.Facade.EmployeeFacade.DeleteEmployeeNote(Id);
            result = true;
            return Json(new { result = result, empid = empid });
        }

        [Authorize]
        public ActionResult RecruitUserRemainder(int id)
        {
            List<EmployeeNote> ListEmployeeRemainder = new List<EmployeeNote>();
            var currentlogin = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ViewBag.empid = id;
            var employeeobj = _Util.Facade.EmployeeFacade.GetEmployeeById(id);
            if (employeeobj != null)
            {
                ListEmployeeRemainder = _Util.Facade.EmployeeFacade.GetAllEmployeeRemainderByCompanyIdAndEmployeeId(currentlogin.CompanyId.Value, employeeobj.UserId);
            }
            return View("_RecruitUserRemainder", ListEmployeeRemainder);
        }

        [Authorize]
        public ActionResult AddRecruitUserRemainder(int? id, int empid)
        {
            var currentlogin = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            EmployeeNote model;
            if (id > 0)
            {
                model = _Util.Facade.EmployeeFacade.GetEmployeeNoteById(id.Value);
            }
            else
            {
                model = new EmployeeNote();
            }
            return View("_AddRecruitUserRemainder", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddRecruitUserRemainder(EmployeeNote en)
        {
            bool result = false;
            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeById(en.EmpId);
            int empid = 0;
            var currentlogin = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (en.Id > 0)
            {
                en.CompanyId = currentlogin.CompanyId.Value;
                en.EmployeeId = objemp.UserId;
                en.CreatedDate = DateTime.Now.UTCCurrentTime();
                empid = objemp.Id;
                en.IsActive = true;
                en.CreatedBy = objemp.FirstName + " " + objemp.LastName;
                en.IsEmail = false;
                en.IsText = false;
                en.IsShedule = false;
                en.IsFollowUp = true;
                result = _Util.Facade.EmployeeFacade.UpdateEmployeeNote(en);
            }
            else
            {
                if (objemp != null)
                {
                    EmployeeNote objempnote = new EmployeeNote()
                    {
                        EmployeeId = objemp.UserId,
                        CompanyId = currentlogin.CompanyId.Value,
                        Notes = en.Notes,
                        ReminderDate = en.ReminderDate,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        IsEmail = false,
                        IsText = false,
                        IsShedule = false,
                        IsFollowUp = true,
                        IsActive = true,
                        CreatedBy = objemp.FirstName + " " + objemp.LastName,
                        IsClose = false
                    };
                    result = _Util.Facade.EmployeeFacade.InsertEmployeeNote(objempnote) > 0;
                    empid = objemp.Id;
                }
            }
            return Json(new { result = result, empid = empid });
        }

        [Authorize]
        public ActionResult RecruitUserSchedule(int id)
        {
            List<EmployeeNote> ListEmployeeRemainder = new List<EmployeeNote>();
            var currentlogin = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ViewBag.empid = id;
            var employeeobj = _Util.Facade.EmployeeFacade.GetEmployeeById(id);
            if (employeeobj != null)
            {
                ListEmployeeRemainder = _Util.Facade.EmployeeFacade.GetAllEmployeeScheduleByCompanyIdAndEmployeeId(currentlogin.CompanyId.Value, employeeobj.UserId);
            }
            return View("_RecruitUserSchedule", ListEmployeeRemainder);
        }

        [Authorize]
        public ActionResult AddRecruitUserSchedule(int? id, int empid)
        {
            var currentlogin = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            EmployeeNote model;
            if (id > 0)
            {
                model = _Util.Facade.EmployeeFacade.GetEmployeeNoteById(id.Value);
            }
            else
            {
                model = new EmployeeNote();
            }
            return View("_AddRecruitUserSchedule", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddRecruitUserSchedule(EmployeeNote en)
        {
            bool result = false;
            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeById(en.EmpId);
            int empid = 0;
            var currentlogin = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (en.Id > 0)
            {
                en.CompanyId = currentlogin.CompanyId.Value;
                en.EmployeeId = objemp.UserId;
                en.CreatedDate = DateTime.Now.UTCCurrentTime();
                empid = objemp.Id;
                en.IsActive = true;
                en.CreatedBy = objemp.FirstName + " " + objemp.LastName;
                en.IsEmail = false;
                en.IsText = false;
                en.IsShedule = true;
                en.IsFollowUp = false;
                result = _Util.Facade.EmployeeFacade.UpdateEmployeeNote(en);
            }
            else
            {
                if (objemp != null)
                {
                    EmployeeNote objempnote = new EmployeeNote()
                    {
                        EmployeeId = objemp.UserId,
                        CompanyId = currentlogin.CompanyId.Value,
                        Notes = en.Notes,
                        ReminderDate = en.ReminderDate,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        IsEmail = false,
                        IsText = false,
                        IsShedule = true,
                        IsFollowUp = false,
                        IsActive = true,
                        CreatedBy = objemp.FirstName + " " + objemp.LastName,
                        IsClose = false
                    };
                    result = _Util.Facade.EmployeeFacade.InsertEmployeeNote(objempnote) > 0;
                    empid = objemp.Id;
                }
            }
            return Json(new { result = result, empid = empid });
        }  
    }
}