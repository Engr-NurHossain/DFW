using HS.CSM;
using HS.CSM.Models;
using HS.Entities;
using HS.Entities.Custom;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.Helper;
using Newtonsoft.Json;
using NLog;
using OS.AWS.S3.Services;
using RestSharp;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Excel = ClosedXML.Excel;
using PermissionList = HS.Framework.UserPermissions;
//using System.Runtime.InteropServices.ComTypes;

namespace HS.Web.UI.Controllers
{
    public class LeadsController : BaseController
    {
        public string AWSS3Url { get; set; }

        public LeadsController()
        {
            AWSS3Url = string.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
            logger = LogManager.GetCurrentClassLogger();
        }

        [Authorize]
        // GET: Leads
        public ActionResult Index(int? id, int? LeadId)
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            if (id.HasValue)
            {
                ViewBag.id = id.Value;
            }
            if (LeadId.HasValue)
            {
                ViewBag.LeadId = LeadId.Value;
            }
            return View();
        }
        public ActionResult LeadImportFromCMS()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<GlobalSetting> Settings = _Util.Facade.GlobalSettingsFacade.GetAllJupiterSettingsByCompanyId(CurrentUser.CompanyId.Value);

            return View(Settings);
        }
        [Authorize]
        [HttpPost]
        public JsonResult LeadImportFromCMS(GetLeadsRequest request)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            int CusCount = 0;
            int LeadCount = 0;
            GetLeadsResponseList response = JupiterLeadMigration.GetLeads(request);
            if (response != null && response.GetLeadsResponse.Count > 0)
            {
                //Lookup Lookupstatus = _Util.Facade.LookupFacade.GetLookupByKey("LeadStatus").Where(x => x.DisplayText == "New").FirstOrDefault();
                string LookupstatusS = "New";
                //if (Lookupstatus != null)
                //{
                //    LookupstatusS = Lookupstatus.DisplayText;
                //}
                //Lookup LookupLeadSource = _Util.Facade.LookupFacade.GetLookupByKey("LeadSource").Where(x => x.DisplayText == "Jupiter").FirstOrDefault();
                string LookupLeadSourceS = "Jupiter";
                //if (LookupLeadSource != null)
                //{
                //    LookupLeadSourceS = LookupLeadSource.DisplayText;
                //}
                //Lookup Lookupstatus = _Util.Facade.LookupFacade.GetLookupByKey("LeadStatus").Where(x => x.DisplayText == "New").FirstOrDefault();
                //Lookup LookupLeadSource = _Util.Facade.LookupFacade.GetLookupByKey("LeadSource").Where(x => x.DisplayText == "Jupiter").FirstOrDefault();
                foreach (var item in response.GetLeadsResponse)
                {
                    #region If Customer already exists
                    Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCentralStationRefId(item.id);
                    if (cus != null)
                    {
                        if (item.notes != null && item.notes.Count > 0)
                        {
                            InsertCustomerNotes(item.notes, cus.CustomerId);
                        }
                        continue;
                    }
                    #endregion

                    #region If CustomerContactTrack Exists
                    CustomerContactTrack CustomerContactTrack = _Util.Facade.CustomerFacade.GetCustomerContactTrackByPlatformId(item.id);
                    if (CustomerContactTrack != null)
                    {
                        cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerContactTrack.CustomerId);
                        if (cus != null && item.notes != null && item.notes.Count > 0)
                        {
                            InsertCustomerNotes(item.notes, cus.CustomerId);
                        }
                        continue;
                    }
                    #endregion

                    if (item.email == null)
                    {
                        item.email = "";
                    }
                    cus = _Util.Facade.CustomerFacade.GetCustomerByPhoneNoOrEmail(item.phone, item.email.Replace("'", "''"));

                    if (cus == null)
                    {

                        if (!string.IsNullOrWhiteSpace(item.phone) && item.phone.Length == 12)
                        {
                            string TempphoneNo = item.phone.Substring(2, item.phone.Length - 2);

                            long PhoneNumber = 0;
                            if (long.TryParse(TempphoneNo, out PhoneNumber))
                            {
                                item.phone = String.Format("{0:###-###-####}", PhoneNumber);
                            }
                        }

                        cus = new Customer()
                        {
                            CentralStationRefId = item.id.ToString(),
                            CmsRefId = request.siteId.ToString(),
                            CustomerId = Guid.NewGuid(),
                            FirstName = item.firstName,
                            LastName = item.lastName,
                            CreatedByUid = CurrentUser.UserId,
                            CreatedDate = item.timestamp,
                            PrimaryPhone = item.phone,
                            EmailAddress = item.email,
                            City = item.city,
                            State = item.state,
                            ZipCode = item.zip,
                            Street = item.street,
                            Note = item.comment,
                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            LastUpdatedBy = User.Identity.Name,
                            LastUpdatedByUid = CurrentUser.UserId,
                            ReferringCustomer = Guid.Empty,
                            ChildOf = Guid.Empty,
                            AccessGivenTo = Guid.Empty,
                            IsActive = true,
                            JoinDate = item.timestamp,
                            Status = LookupstatusS,
                            //LeadSource = LookupLeadSourceS,
                            LeadSource = item.campaign,
                            //SoldBy2 = new Guid(),
                            //SoldBy3 = new Guid()
                        };

                        bool soldStatus = true;
                        if (item.soldStatus != null && item.soldStatus == 0)
                        {
                            soldStatus = true;
                        }
                        else if (item.soldStatus != null)
                        {
                            soldStatus = false;
                        }
                        CustomerCompany cc = new CustomerCompany()
                        {
                            CustomerId = cus.CustomerId,
                            CompanyId = CurrentUser.CompanyId.Value,
                            IsLead = soldStatus,
                            IsActive = true
                        };

                        CustomerMigration Cm = new CustomerMigration()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            CreatedBy = CurrentUser.UserId,
                            CustomerId = cus.CustomerId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            Platform = LabelHelper.CustomerMigrationPlatforms.Jupiter,
                            RefenrenceId = item.id,
                            Note = "ServiceType: " + item.ServiceType,

                        };
                        _Util.Facade.CustomerFacade.InsertCustomerMigration(Cm);

                        _Util.Facade.CustomerFacade.InsertCustomer(cus);
                        _Util.Facade.CustomerFacade.InsertCustomerCompany(cc);

                        CustomerContactTrack model = new CustomerContactTrack()
                        {
                            CustomerId = cus.CustomerId,
                            CustomerPlatform = LookupLeadSourceS,
                            Note = item.recordingUrl,
                            CreatedDate = item.timestamp,
                            PlatformId = item.id,
                        };
                        _Util.Facade.CustomerFacade.InsertCustomerContactTrack(model);
                        if (soldStatus == true)
                        {
                            LeadCount++;
                        }
                        else
                        {
                            CusCount++;
                        }

                    }
                    else
                    {
                        CustomerContactTrack model = new CustomerContactTrack()
                        {
                            CustomerId = cus.CustomerId,
                            CustomerPlatform = LookupLeadSourceS,
                            Note = item.recordingUrl,
                            CreatedDate = item.timestamp,
                            PlatformId = item.id,
                        };
                        _Util.Facade.CustomerFacade.InsertCustomerContactTrack(model);
                    }
                    InsertCustomerNotes(item.notes, cus.CustomerId);

                    #region Schedule
                    DateTime appointmentDate = new DateTime();

                    if (item.isAppointmentSet.HasValue && item.isAppointmentSet > 0
                        && DateTime.TryParse(item.appointmentDate, out appointmentDate)
                        && appointmentDate != new DateTime())
                    {
                        #region Insert Ticket
                        Guid CustomerId = new Guid();
                        if (cus != null)
                        {
                            CustomerId = cus.CustomerId;
                        }
                        //else if (cus != null)
                        //{
                        //    CustomerId = cus.CustomerId;
                        //}


                        Ticket Ticket = new Ticket()
                        {
                            TicketId = Guid.NewGuid(),
                            CompanyId = CurrentUser.CompanyId.Value,
                            CustomerId = CustomerId,
                            TicketType = LabelHelper.TicketType.Inspection,
                            Subject = LabelHelper.TicketType.Inspection,
                            Message = item.comment,
                            CreatedBy = new Guid("22222222-2222-2222-2222-222222222222"),
                            CreatedDate = item.timestamp,
                            CompletionDate = appointmentDate,
                            Status = LabelHelper.TicketStatus.Created,
                            Priority = "",
                            LastUpdatedBy = new Guid("22222222-2222-2222-2222-222222222222"),
                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            HasInvoice = false,
                            HasSurvey = false,
                            IsClosed = false,
                            IsAgreementTicket = false,
                            IsDispatch = false,
                        };

                        _Util.Facade.TicketFacade.InsertTicket(Ticket);

                        TicketUser TicketUser = new TicketUser()
                        {
                            TiketId = Ticket.TicketId,
                            UserId = new Guid("22222222-2222-2222-2222-222222222222"),
                            IsPrimary = true,
                            AddedBy = new Guid("22222222-2222-2222-2222-222222222222"),
                            NotificationOnly = false,
                            AddedDate = DateTime.Now.UTCCurrentTime(),
                        };
                        _Util.Facade.TicketFacade.InsertTicketUser(TicketUser);

                        CustomerAppointment CustomerAppointment = new CustomerAppointment()
                        {
                            AppointmentId = Ticket.TicketId,
                            CompanyId = CurrentUser.CompanyId.Value,
                            CustomerId = Ticket.CustomerId,
                            EmployeeId = new Guid("22222222-2222-2222-2222-222222222222"),
                            AppointmentType = Ticket.TicketType,
                            AppointmentDate = appointmentDate,
                            AppointmentStartTime = "12:00",
                            AppointmentEndTime = "14:00",
                            IsAllDay = false,
                            Notes = item.comment,
                            Status = false,
                            CreatedBy = "System User",
                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            LastUpdatedBy = User.Identity.Name
                        };
                        _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(CustomerAppointment);

                        #endregion
                    }
                    #endregion


                }
            }
            return Json(new { result = true, message = string.Format("{0} leads and {1} customers imported successfully.", LeadCount, CusCount) });
        }
        public JsonResult SetAsFavorite(int Id, bool IsFavourite)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if (CurrentLoggedInUser != null)
            {
                KnowledgebaseFavouriteUser favlead = _Util.Facade.CustomerFacade.GetFavoriteArticleByUserId(CurrentLoggedInUser.UserId, Id);
                if (favlead != null)
                {
                    favlead.IsFavourite = IsFavourite;
                    result = _Util.Facade.CustomerFacade.UpdateFavoriteUserArticle(favlead);
                }
                else
                {
                    KnowledgebaseFavouriteUser newfav = new KnowledgebaseFavouriteUser()
                    {
                        UserId = CurrentLoggedInUser.UserId,
                        KnowledgebaseId = Id,
                        IsFavourite = IsFavourite,
                        CreatedBy = CurrentLoggedInUser.UserId,
                        CreatedDate = DateTime.UtcNow,
                        LastUpdatedDate = DateTime.UtcNow,
                        LastUpdatedBy = CurrentLoggedInUser.UserId
                    };
                    _Util.Facade.CustomerFacade.InsertKnowledgebaseFavouriteUser(newfav);
                }
            }
            return Json(result);
        }
        private void InsertCustomerNotes(List<CSMNote> notes, Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (notes != null && notes.Count > 0)
            {
                foreach (CSMNote NoteItem in notes)
                {
                    CustomerNote customerNote = _Util.Facade.NotesFacade.GetCustomerNoteByThirdPartyId(NoteItem.noteID);
                    if (customerNote == null)
                    {
                        Guid UserId = new Guid("22222222-2222-2222-2222-222222222222");

                        #region get And Update Employee
                        Employee emp = null;
                        if (!string.IsNullOrWhiteSpace(NoteItem.userEmail))
                        {
                            emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmailAddress(NoteItem.userEmail);
                        }
                        if (emp == null) //if there is no employee with this email address add new employee
                        {
                            emp = _Util.Facade.EmployeeFacade.GetemployeeByFirstNameAndLastNameOrCSID(NoteItem.userName, NoteItem.userId);
                            if (emp == null)
                            {
                                #region Split FirstName And last name from full name
                                string FirstName = "";
                                string LastName = "";
                                if (!string.IsNullOrWhiteSpace(NoteItem.userName))
                                {
                                    var str = NoteItem.userName.Replace("  ", " ").Split(' ');
                                    if (str.Count() > 0)
                                    {
                                        var i = 1;
                                        FirstName = str[0];
                                        for (; i < str.Count() - 1; i++)
                                        {
                                            FirstName += " " + str[i];
                                        }
                                        if (str.Count() > 1)
                                        {
                                            LastName = str[i];
                                        }
                                        else
                                        {
                                            LastName = "";
                                        }

                                    }
                                    else
                                    {
                                        FirstName = NoteItem.userName;
                                        LastName = "";
                                    }
                                }
                                #endregion

                                #region Insert new employee with email address
                                Employee newEmp = new Employee()
                                {
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    UserId = Guid.NewGuid(),
                                    FirstName = FirstName,
                                    LastName = LastName,
                                    Email = NoteItem.userEmail,
                                    UserName = NoteItem.userEmail,
                                    IsActive = false,
                                    IsCalendar = false,
                                    IsDeleted = false,
                                    IsCurrentEmployee = false,
                                    IsPayroll = false,
                                    IsSalesMatrixUserX = false,
                                    IsSupervisor = false,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    CSId = NoteItem.userId,
                                    Recruited = true,
                                    LastUpdatedBy = User.Identity.Name,
                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime()
                                };
                                UserLogin ul = new UserLogin()
                                {
                                    UserName = NoteItem.userEmail,
                                    UserId = newEmp.UserId,
                                    EmailAddress = newEmp.Email,
                                    IsActive = false,
                                    IsDeleted = false,
                                    IsSupervisor = false,
                                    LastUpdatedBy = "System User",
                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                    FirstName = FirstName,
                                    LastName = LastName,
                                    CompanyId = CurrentUser.CompanyId.Value
                                };
                                UserCompany uc = new UserCompany()
                                {
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    UserId = newEmp.UserId,
                                    IsDefault = true,
                                };

                                UserOrganization userOrganization = new UserOrganization()
                                {
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    UserName = newEmp.UserName,
                                    IsActive = true
                                };
                                _Util.Facade.EmployeeFacade.InsertEmployee(newEmp);
                                _Util.Facade.UserLoginFacade.InsertUserLogin(ul);
                                _Util.Facade.UserCompanyFacade.InsertUserCompany(uc);
                                _Util.Facade.UserOrganizationFacade.InsertUserOrganization(userOrganization);
                                #endregion
                            }
                        }
                        else if (emp != null)
                        {
                            UserId = emp.UserId;
                            if (emp.CSId == null)
                            {
                                emp.CSId = NoteItem.userId;
                                _Util.Facade.EmployeeFacade.UpdateEmployee(emp);
                            }
                        }
                        #endregion

                        //Guid CustomerId = new Guid();
                        //if (cus != null)
                        //{
                        //    CustomerId = CustomerId;
                        //}

                        #region Customer Note Insert
                        CustomerNote Note = new CustomerNote()
                        {
                            Notes = NoteItem.comment,
                            ReminderDate = null,
                            ReminderEndDate = null,
                            CustomerId = CustomerId,
                            CompanyId = CurrentUser.CompanyId.Value,
                            CreatedDate = NoteItem.dateTime,
                            IsEmail = false,
                            IsText = false,
                            IsShedule = false,
                            IsFollowUp = false,
                            IsActive = true,
                            CreatedBy = NoteItem.userName,
                            IsClose = false,
                            IsAllDay = false,
                            CreatedByUid = UserId,
                            IsPin = false,
                            NoteType = "",
                            ThirdPartyId = NoteItem.noteID
                        };
                        _Util.Facade.NotesFacade.InsertCustomerNote(Note);
                        #endregion
                    }
                }
            }

        }
        public ActionResult SetupIndex(int? id)
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
        public ActionResult LeadSetupIndex(int? id)
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

        public ActionResult LeadAdditionalInfo(Guid CustomerId)
        {
            List<CustomerAdditionalContact> additionalInfo = _Util.Facade.AdditionalContactFacade.GetAllAdditionalContactByCustomerId(CustomerId);
            return View(additionalInfo);
        }
        public ActionResult LeadSystemInfo(Guid CustomerId)
        {
            ViewBag.CustomerId = CustomerId;
            CustomerExtended _extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(CustomerId);
            ViewBag.WarrantyList = _Util.Facade.LookupFacade.GetDropdownsByKey("Warranty");
            ViewBag.KeypadList = _Util.Facade.LookupFacade.GetDropdownsByKey("Keypad");
            ViewBag.FrontEndList = _Util.Facade.LookupFacade.GetDropdownsByKey("Front-End");
            return View(_extended);
        }
        public JsonResult UpdateCustomerExtendedSystemInfo(Guid CustomerId, string Warranty, string Keypad, string FrontEnd)
        {
            bool result = false;
            CustomerExtended _extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(CustomerId);
            if (_extended != null)
            {
                if (!string.IsNullOrWhiteSpace(Warranty))
                {
                    _extended.Warranty = Warranty;
                    _Util.Facade.CustomerFacade.UpdateCustomerExtended(_extended);
                }
                else if (!string.IsNullOrWhiteSpace(Keypad))
                {
                    _extended.Keypad = Keypad;
                    _Util.Facade.CustomerFacade.UpdateCustomerExtended(_extended);
                }
                else if (!string.IsNullOrWhiteSpace(FrontEnd))
                {
                    _extended.FrontEnd = FrontEnd;
                    _Util.Facade.CustomerFacade.UpdateCustomerExtended(_extended);
                }
                result = true;
            }
            return Json(new { result = result, model = _extended });
        }
        public ActionResult LeadCustomerContactTrackList(Guid CustomerId)
        {
            List<CustomerContactTrack> CustomerContactTrackList = new List<CustomerContactTrack>();

            #region No need
            //Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            //CustomerContactTrackList.Add(new CustomerContactTrack
            //{
            //    CustomerId= customer.CustomerId,
            //    CustomerPlatform = customer.LeadSource,
            //    Note=customer.BillNotes,
            //    CreatedDate=customer.JoinDate,
            //    PlatformId= customer.CentralStationRefId.ToInt()

            //});
            #endregion

            CustomerContactTrackList.AddRange(_Util.Facade.CustomerFacade.GetCustomerContactTrackByCustomerId(CustomerId));
            return View(CustomerContactTrackList);
        }



        public PartialViewResult LeadsPartial(string firstdate, string lastdate)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ViewBag.firstdate = firstdate;
            ViewBag.lastdate = lastdate;
            ViewBag.CustomFormGeneration = _Util.Facade.GlobalSettingsFacade.GetCustomFormGeneration(CurrentLoggedInUser.CompanyId.Value);
            return PartialView("_Leads");
        }
        public PartialViewResult LeadsLitePartial(string firstdate, string lastdate)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ViewBag.firstdate = firstdate;
            ViewBag.lastdate = lastdate;
            ViewBag.CustomFormGeneration = _Util.Facade.GlobalSettingsFacade.GetCustomFormGeneration(CurrentLoggedInUser.CompanyId.Value);
            return PartialView("_LeadsLite");
        }
        public ActionResult GetLeadsForPopUp(int? LeadId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (LeadId.HasValue)
            {
                ViewBag.LeadId = LeadId;
            }
            ViewBag.AgreementDocumentHeight = _Util.Facade.GlobalSettingsFacade.GetAgreementDocumentHeightByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            ViewBag.StringHeight = ViewBag.AgreementDocumentHeight + "px";
            return View();
        }
        public PartialViewResult LeadsListPartial(string firstdate, string lastdate)
        {
            //if (HasNoPermission(CurrentPermission.Lead.List))
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}

            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            #region Not in use
            //List<SelectListItem> LeadTypeList = new List<SelectListItem>();
            //if (CurrentLoggedInUser.UserRole == "Admin" || CurrentLoggedInUser.UserRole == "SysAdmin")
            //{
            //    LeadTypeList.Add(new SelectListItem()
            //    {
            //        Text = "Please Select",
            //        Value = "-1"
            //    });
            //    LeadTypeList.AddRange(_Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value).OrderBy(x => x.FirstName != "Select One").ThenBy(x => x.FirstName).Select(x => new SelectListItem()
            //    {
            //        Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
            //        Value = x.UserId.ToString()
            //    }).ToList());
            //    ViewBag.LeadUserList = LeadTypeList;
            //}
            //else
            //{
            //    var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
            //    if (objemp != null)
            //    {
            //        LeadTypeList.Add(new SelectListItem()
            //        {
            //            Text = "Please Select One",
            //            Value = "-1"
            //        });
            //        LeadTypeList.Add(new SelectListItem()
            //        {
            //            Text = objemp.FirstName.ToString() + " " + objemp.LastName.ToString(),
            //            Value = objemp.UserId.ToString()
            //        });
            //    }
            //    ViewBag.LeadUserList = LeadTypeList;
            //}
            #endregion

            ViewBag.dailyfirstdate = firstdate;
            ViewBag.dailylastdate = lastdate;
            List<SelectListItem> LeadStatus = new List<SelectListItem>();
            LeadStatus.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            LeadStatus.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("LeadStatus").OrderBy(x => x.DataValue.ToString() != "-1").ThenBy(x => x.DisplayText).Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.LeadStatus = LeadStatus;
            List<SelectListItem> BranchList = new List<SelectListItem>();
            BranchList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });

            List<CompanyBranch> CompanyBranchDropDown = _Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            if (CompanyBranchDropDown != null && CompanyBranchDropDown.Count > 0)
            {
                BranchList.AddRange(CompanyBranchDropDown.OrderBy(x => x.Id.ToString() != "-1").ThenBy(x => x.Name).Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList());
            }

            ViewBag.BranchList = BranchList;
            LeadTabCountModel model = new LeadTabCountModel();
            if (base.IsPermitted(PermissionList.LeadPermissions.LeadGridSummary))
            {

                model = _Util.Facade.CustomerFacade.GetAllLeadTabCountByCompanyId(CurrentLoggedInUser.CompanyId.Value,
                    base.IsPermitted(PermissionList.LeadPermissions.LeadBookingCountTab),
                    base.IsPermitted(PermissionList.LeadPermissions.LeadEstimatesCountTab));
            }


            ViewBag.NewLeadsList = "false";
            if (base.IsPermitted(PermissionList.LeadPermissions.NewLeadsFilter))
            {
                List<Customer> newLeadList = _Util.Facade.CustomerFacade.GetAllLeadByCompanyIdByCustomerStatus(CurrentLoggedInUser.CompanyId.Value);
                if (newLeadList != null && newLeadList.Count > 0)
                {
                    ViewBag.NewLeadsList = "true";
                    ViewBag.NewLeadCount = newLeadList.Count;
                }
            }
            return PartialView("_LeadsList", model);
        }
        [Authorize]
        public PartialViewResult AddLeads(int? id)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            Customer model;
            if (id.HasValue && id > 0)
            {
                model = _Util.Facade.CustomerFacade.GetCustomersById(id.Value);
                ViewBag.ReferringCustomer = model.ReferringCustomer;
            }
            else
            {
                model = new Customer();
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();

            ViewBag.CustomerTypeList = _Util.Facade.LookupFacade.GetLookupByKey("CustomerType").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();

            ViewBag.YesNoList = _Util.Facade.LookupFacade.GetLookupByKey("YesNo").Select(x =>
                                new SelectListItem()
                                {
                                    Text = x.DisplayText.ToString(),
                                    Value = x.DataValue.ToString()
                                }).ToList();

            ViewBag.TimeToCall = _Util.Facade.LookupFacade.GetLookupByKey("TimeToCall").OrderBy(x => x.DisplayText).Select(x =>
                              new SelectListItem()
                              {
                                  Text = x.DisplayText.ToString(),
                                  Value = x.DataValue.ToString()
                              }).ToList();

            ViewBag.CreditScore = _Util.Facade.LookupFacade.GetLookupByKey("CreditScore").Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.DisplayText.ToString(),
                                 Value = x.DataValue.ToString()
                             }).ToList();

            if (CurrentLoggedInUser.UserTags.ToLower() == "admin")
            {
                ViewBag.ContractTerm = _Util.Facade.LookupFacade.GetLookupByKey("ContractTerm").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            }
            else
            {
                ViewBag.ContractTerm = _Util.Facade.LookupFacade.GetLookupByKey("SalesContractTerm").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            }


            List<SelectListItem> SalesPersons = new List<SelectListItem>();
            SalesPersons.Add(new SelectListItem()
            {
                Text = "Sales Persons",
                Value = "-1"
            });
            List<Employee> EmployeeDropDown = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentLoggedInUser.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid(), LabelHelper.UserTags.Partner).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).ToList();
            if (EmployeeDropDown != null && EmployeeDropDown.Count > 0)
            {
                SalesPersons.AddRange(EmployeeDropDown.OrderBy(x => x.UserId.ToString() != "-1").ThenBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Select(x => new SelectListItem()
                {
                    Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                    Value = x.UserId.ToString()
                }).ToList());
            }

            ViewBag.SalesPersonList = SalesPersons.ToList();



            ViewBag.InstallerList = _Util.Facade.EmployeeFacade.GetAllInstallerEmployeeByCompnayId(CurrentUser.CompanyId.Value).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.FirstName + " " + x.LastName,
                               Value = x.UserId.ToString()
                           }).ToList();
            List<SelectListItem> SourceLead = new List<SelectListItem>();
            SourceLead.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            SourceLead.AddRange(_Util.Facade.LookupFacade.GetLookupByKeyWithoutOrder("LeadSource").OrderBy(x => x.DataValue.ToString() != "-1").ThenBy(x => x.DisplayText).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList());
            ViewBag.SourceLead = SourceLead;
            ViewBag.StateList = _Util.Facade.LookupFacade.GetLookupByKey("StateList").OrderBy(x => x.DisplayText != "Lead Source").ThenBy(x => x.DisplayText).Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList();
            return PartialView("AddLeads", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveCustomerLeadImportFile(string File, string isCustomer)
        {
            string subPath = "~/LeadImportReports"; // your code goes here

            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));

            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
            //{
            //    file.WriteLine("Started at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            //}
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    //{
                    //    file.WriteLine("Excel file read at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    //}
                    //Excel.Application xlApp = new Excel.Application();
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    //{
                    //    file.WriteLine("Excel application create at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    //}
                    var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    //{
                    //    file.WriteLine("Workbooks.Open at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    //}
                    Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    //{
                    //    file.WriteLine("select sheet 1 at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    //}
                    var xlRange = workSheet.RangeUsed();

                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    //{
                    //    file.WriteLine("calculation started at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    //}
                    DateTime CreatedDate = DateTime.Now.UTCCurrentTime();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();
                    var LookUpList = _Util.Facade.LookupFacade.GetAllLookup();
                    Guid CustomerGuidId = new Guid();
                    List<Employee> employees = _Util.Facade.EmployeeFacade.GetAllEmployee();
                    for (int i = 2; i <= rowCount; i++)
                    {
                        var result = false;
                        Customer customer = new Customer();
                        Guid CustomerId = new Guid();
                        Customer ExistCustomer = new Customer();
                        if (customer.Id > 0)
                        {
                            ExistCustomer = _Util.Facade.CustomerFacade.GetCustomerById(customer.Id);
                            if (ExistCustomer != null)
                            {
                                CustomerGuidId = ExistCustomer.CustomerId;
                            }
                            else
                            {
                                CustomerGuidId = Guid.NewGuid();
                            }
                        }
                        else
                        {
                            CustomerGuidId = Guid.NewGuid();
                        }
                        try
                        {
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        }
                                        if (header == "Id")
                                        {
                                            int Id = 0;
                                            int.TryParse(value, out Id);
                                            if (Id > 0)
                                            {
                                                customer.Id = Id;
                                            }
                                            continue;
                                        }

                                        #region GUID
                                        else if (header == "CustomerId")
                                        {
                                            if (Guid.TryParse(value, out CustomerId) && CustomerId != new Guid())
                                            {
                                                customer.CustomerId = CustomerId;
                                            }
                                            else
                                            {
                                                customer.CustomerId = Guid.NewGuid();
                                            }
                                            continue;

                                        }
                                        else if (header == "CreatedByUid")
                                        {
                                            Guid CreatedByUid = new Guid();
                                            if (Guid.TryParse(value, out CreatedByUid))
                                            {
                                                customer.CreatedByUid = CreatedByUid;
                                            }
                                            else
                                            {
                                                customer.CreatedByUid = CurrentUser.UserId;
                                            }
                                            continue;
                                        }
                                        else if (header == "LastUpdatedByUid")
                                        {
                                            Guid LastUpdatedByUid = new Guid();
                                            if (Guid.TryParse(value, out LastUpdatedByUid))
                                            {
                                                customer.LastUpdatedByUid = LastUpdatedByUid;
                                            }
                                            else
                                            {
                                                customer.LastUpdatedByUid = CurrentUser.UserId;
                                            }
                                            continue;
                                        }
                                        else if (header == "ReferringCustomer")
                                        {
                                            Guid ReferringCustomer = new Guid();
                                            int ReferringCustomerInt = 0;
                                            if (Guid.TryParse(value, out ReferringCustomer))
                                            {
                                                customer.ReferringCustomer = ReferringCustomer;
                                            }
                                            else if (int.TryParse(value, out ReferringCustomerInt) && ReferringCustomerInt > 0)
                                            {
                                                Customer cus = _Util.Facade.CustomerFacade.GetCustomerById(ReferringCustomerInt);
                                                if (cus != null)
                                                {
                                                    customer.ReferringCustomer = customer.ReferringCustomer;
                                                }
                                                else
                                                {
                                                    customer.ReferringCustomer = new Guid();
                                                }
                                            }
                                            else
                                            {
                                                customer.ReferringCustomer = new Guid();
                                            }
                                            continue;
                                        }
                                        else if (header == "ChildOf")
                                        {
                                            Guid ChildOf = new Guid();
                                            int ChildOfInt = 0;
                                            if (Guid.TryParse(value, out ChildOf))
                                            {
                                                customer.ChildOf = ChildOf;
                                            }
                                            else if (int.TryParse(value, out ChildOfInt) && ChildOfInt > 0)
                                            {
                                                Customer cus = _Util.Facade.CustomerFacade.GetCustomerById(ChildOfInt);
                                                if (cus != null)
                                                {
                                                    customer.ChildOf = customer.CustomerId;
                                                }
                                                else
                                                {
                                                    customer.ChildOf = new Guid();
                                                }
                                            }
                                            else
                                            {
                                                customer.ChildOf = new Guid();
                                            }
                                            continue;
                                        }
                                        else if (header == "AccessGivenTo")
                                        {
                                            Guid AccessGivenTo = new Guid();
                                            if (Guid.TryParse(value, out AccessGivenTo))
                                            {
                                                customer.AccessGivenTo = AccessGivenTo;
                                            }
                                            else
                                            {
                                                customer.AccessGivenTo = new Guid();
                                            }
                                            continue;
                                        }
                                        else if (header == "DuplicateCustomer")
                                        {
                                            Guid DuplicateCustomer = new Guid();
                                            int DuplicateCustomerInt = 0;
                                            if (Guid.TryParse(value, out DuplicateCustomer))
                                            {
                                                customer.DuplicateCustomer = DuplicateCustomer;
                                            }
                                            else if (int.TryParse(value, out DuplicateCustomerInt) && DuplicateCustomerInt > 0)
                                            {
                                                Customer cus = _Util.Facade.CustomerFacade.GetCustomerById(DuplicateCustomerInt);
                                                if (cus != null)
                                                {
                                                    customer.DuplicateCustomer = customer.CustomerId;
                                                }
                                                else
                                                {
                                                    customer.DuplicateCustomer = new Guid();
                                                }
                                            }
                                            else
                                            {
                                                customer.DuplicateCustomer = new Guid();
                                            }
                                            continue;
                                        }
                                        else if (header == "Soldby" || header == "Sales Person")
                                        {
                                            Guid Soldby = new Guid();
                                            int SoldbyInt = 0;

                                            if (value == "Unknown")
                                            {
                                                continue;
                                            }

                                            if (string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Soldby = "";
                                            }
                                            else if (Guid.TryParse(value, out Soldby) && Soldby != new Guid())
                                            {
                                                customer.Soldby = Soldby.ToString();
                                            }
                                            else if (int.TryParse(value, out SoldbyInt) && SoldbyInt > 0)
                                            {
                                                UserLogin ul = _Util.Facade.UserLoginFacade.GetUserLoginById(SoldbyInt);
                                                if (ul != null)
                                                {
                                                    customer.Soldby = ul.UserId.ToString();
                                                    customer.Soldby1 = ul.UserId;
                                                }
                                            }
                                            else
                                            {
                                                Employee employee = employees.Where(x => x.FirstName + " " + x.LastName == value).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).FirstOrDefault();
                                                if (employee == null)
                                                {
                                                    employee = employees.Where(x => x.UserName == value).FirstOrDefault();

                                                }
                                                if (employee != null)
                                                {
                                                    customer.Soldby = employee.UserId.ToString();
                                                    customer.Soldby1 = employee.UserId;
                                                }
                                            }

                                            continue;
                                        }
                                        else if (header == "SoldBy2")
                                        {
                                            Guid SoldBy2 = new Guid();
                                            int SoldbyInt = 0;
                                            if (Guid.TryParse(value, out SoldBy2))
                                            {
                                                customer.SoldBy2 = SoldBy2;
                                            }
                                            else if (int.TryParse(value, out SoldbyInt) && SoldbyInt > 0)
                                            {
                                                UserLogin ul = _Util.Facade.UserLoginFacade.GetUserLoginById(SoldbyInt);
                                                if (ul != null)
                                                {
                                                    customer.SoldBy2 = ul.UserId;
                                                }
                                                else
                                                {
                                                    customer.SoldBy2 = new Guid();
                                                }
                                            }
                                            else
                                            {
                                                Employee employee = employees.Where(x => x.FirstName + " " + x.LastName == value).OrderBy(x => x.FirstName).FirstOrDefault();
                                                if (employee != null)
                                                {
                                                    customer.SoldBy2 = employee.UserId;
                                                }
                                            }
                                            continue;
                                        }
                                        else if (header == "SoldBy3")
                                        {
                                            Guid SoldBy3 = new Guid();
                                            int SoldbyInt = 0;
                                            if (Guid.TryParse(value, out SoldBy3))
                                            {
                                                customer.SoldBy3 = SoldBy3;
                                            }
                                            else if (int.TryParse(value, out SoldbyInt) && SoldbyInt > 0)
                                            {
                                                UserLogin ul = _Util.Facade.UserLoginFacade.GetUserLoginById(SoldbyInt);
                                                if (ul != null)
                                                {
                                                    customer.SoldBy3 = ul.UserId;
                                                }
                                                else
                                                {
                                                    customer.SoldBy3 = new Guid();
                                                }
                                            }
                                            else
                                            {
                                                Employee employee = employees.Where(x => x.FirstName + " " + x.LastName == value).OrderBy(x => x.FirstName).FirstOrDefault();
                                                if (employee != null)
                                                {
                                                    customer.SoldBy2 = employee.UserId;
                                                }
                                            }
                                            continue;
                                        }

                                        #endregion

                                        #region Bool
                                        else if (header == "IsAlarmCom")
                                        {
                                            bool IsAlarm = false;
                                            if (bool.TryParse(value, out IsAlarm))
                                            {
                                                customer.IsAlarmCom = IsAlarm;
                                            }
                                            else
                                            {
                                                customer.IsAlarmCom = null;
                                            }
                                            continue;

                                        }
                                        else if (header == "CreditScoreValue")
                                        {
                                            int CreditScoreValue = 0;
                                            if (int.TryParse(value, out CreditScoreValue))
                                            {
                                                customer.CreditScoreValue = CreditScoreValue;
                                            }
                                            continue;

                                        }
                                        else if (header == "CellularBackup")
                                        {
                                            bool CellularBackup = false;
                                            if (bool.TryParse(value, out CellularBackup))
                                            {
                                                customer.CellularBackup = CellularBackup;
                                            }
                                            else
                                            {
                                                customer.CellularBackup = null;
                                            }
                                            continue;

                                        }
                                        else if (header == "CustomerFunded")
                                        {
                                            bool CustomerFunded = false;
                                            if (bool.TryParse(value, out CustomerFunded))
                                            {
                                                customer.CustomerFunded = CustomerFunded;
                                            }
                                            else
                                            {
                                                customer.CustomerFunded = null;
                                            }
                                            continue;

                                        }
                                        else if (header == "Maintenance")
                                        {
                                            bool Maintenance = false;
                                            if (bool.TryParse(value, out Maintenance))
                                            {
                                                customer.Maintenance = Maintenance;
                                            }
                                            else
                                            {
                                                customer.Maintenance = null;
                                            }
                                            continue;

                                        }
                                        else if (header == "BillTax")
                                        {
                                            bool BillTax = false;
                                            if (bool.TryParse(value, out BillTax))
                                            {
                                                customer.BillTax = BillTax;
                                            }
                                            else
                                            {
                                                customer.BillTax = null;
                                            }
                                            continue;

                                        }
                                        else if (header == "IsTechCallPassed")
                                        {
                                            bool IsTechCallPassed = false;
                                            if (bool.TryParse(value, out IsTechCallPassed))
                                            {
                                                customer.IsTechCallPassed = IsTechCallPassed;
                                            }
                                            else
                                            {
                                                customer.IsTechCallPassed = null;
                                            }
                                            continue;

                                        }
                                        else if (header == "IsDirect")
                                        {
                                            bool IsDirect = false;
                                            if (bool.TryParse(value, out IsDirect))
                                            {
                                                customer.IsDirect = IsDirect;
                                            }
                                            else
                                            {
                                                customer.IsDirect = null;
                                            }
                                            continue;

                                        }
                                        else if (header == "IsRequiredCsvSync")
                                        {
                                            bool IsRequiredCsvSync = false;
                                            if (bool.TryParse(value, out IsRequiredCsvSync))
                                            {
                                                customer.IsRequiredCsvSync = IsRequiredCsvSync;
                                            }
                                            else
                                            {
                                                customer.IsRequiredCsvSync = null;
                                            }
                                            continue;

                                        }
                                        else if (header == "IsActive")
                                        {
                                            bool IsActive = false;
                                            if (bool.TryParse(value, out IsActive))
                                            {
                                                customer.IsActive = IsActive;
                                            }
                                            else
                                            {
                                                customer.IsActive = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "PreferedEmail")
                                        {
                                            bool PreferedEmail = false;
                                            if (bool.TryParse(value, out PreferedEmail))
                                            {
                                                customer.PreferedEmail = PreferedEmail;
                                            }
                                            else
                                            {
                                                customer.PreferedEmail = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "PreferedSms")
                                        {
                                            bool PreferedSms = false;
                                            if (bool.TryParse(value, out PreferedSms))
                                            {
                                                customer.PreferedSms = PreferedSms;
                                            }
                                            else
                                            {
                                                customer.PreferedSms = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "IsAgreement")
                                        {
                                            bool IsAgreement = false;
                                            if (bool.TryParse(value, out IsAgreement))
                                            {
                                                customer.IsAgreement = IsAgreement;
                                            }
                                            else
                                            {
                                                customer.IsAgreement = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "IsFireAccount")
                                        {
                                            bool IsFireAccount = false;
                                            if (bool.TryParse(value, out IsFireAccount))
                                            {
                                                customer.IsFireAccount = IsFireAccount;
                                            }
                                            else
                                            {
                                                customer.IsFireAccount = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "EmailVerified")
                                        {
                                            bool EmailVerified = false;
                                            if (bool.TryParse(value, out EmailVerified))
                                            {
                                                customer.EmailVerified = EmailVerified;
                                            }
                                            else
                                            {
                                                customer.EmailVerified = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "HomeVerified")
                                        {
                                            bool HomeVerified = false;
                                            if (bool.TryParse(value, out HomeVerified))
                                            {
                                                customer.HomeVerified = HomeVerified;
                                            }
                                            else
                                            {
                                                customer.HomeVerified = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "HomeVerified")
                                        {
                                            bool HomeVerified = false;
                                            if (bool.TryParse(value, out HomeVerified))
                                            {
                                                customer.HomeVerified = HomeVerified;
                                            }
                                            else
                                            {
                                                customer.HomeVerified = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "IsPrimaryPhoneVerified")
                                        {
                                            bool IsPrimaryPhoneVerified = false;
                                            if (bool.TryParse(value, out IsPrimaryPhoneVerified))
                                            {
                                                customer.IsPrimaryPhoneVerified = IsPrimaryPhoneVerified;
                                            }
                                            else
                                            {
                                                customer.IsPrimaryPhoneVerified = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "IsSecondaryPhoneVerified")
                                        {
                                            bool IsSecondaryPhoneVerified = false;
                                            if (bool.TryParse(value, out IsSecondaryPhoneVerified))
                                            {
                                                customer.IsPrimaryPhoneVerified = IsSecondaryPhoneVerified;
                                            }
                                            else
                                            {
                                                customer.IsSecondaryPhoneVerified = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "IsCellNoVerified")
                                        {
                                            bool IsCellNoVerified = false;
                                            if (bool.TryParse(value, out IsCellNoVerified))
                                            {
                                                customer.IsCellNoVerified = IsCellNoVerified;
                                            }
                                            else
                                            {
                                                customer.IsCellNoVerified = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "IsReceivePaymentMail")
                                        {
                                            bool IsReceivePaymentMail = false;
                                            if (bool.TryParse(value, out IsReceivePaymentMail))
                                            {
                                                customer.IsReceivePaymentMail = IsReceivePaymentMail;
                                            }
                                            else
                                            {
                                                customer.IsReceivePaymentMail = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "PreferedCall")
                                        {
                                            bool PreferedCall = false;
                                            if (bool.TryParse(value, out PreferedCall))
                                            {
                                                customer.PreferedCall = PreferedCall;
                                            }
                                            else
                                            {
                                                customer.PreferedCall = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "IsAgreementSend")
                                        {
                                            bool IsAgreementSend = false;
                                            if (bool.TryParse(value, out IsAgreementSend))
                                            {
                                                customer.IsAgreementSend = IsAgreementSend;
                                            }
                                            else
                                            {
                                                customer.IsAgreementSend = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "IsACHDiscount")
                                        {
                                            bool IsACHDiscount = false;
                                            if (bool.TryParse(value, out IsACHDiscount))
                                            {
                                                customer.IsACHDiscount = IsACHDiscount;
                                            }
                                            else
                                            {
                                                customer.IsACHDiscount = null;
                                            }
                                            continue;
                                        }
                                        #endregion

                                        #region Dates
                                        else if (header == "DateofBirth" || header == "Date of Birth")
                                        {
                                            DateTime DOB = new DateTime();
                                            if (DateTime.TryParse(value, out DOB))
                                            {
                                                customer.DateofBirth = DOB;
                                            }
                                            else
                                            {
                                                customer.DateofBirth = null;
                                            }
                                            continue;

                                        }
                                        else if (header == "Est Close Date" || header == "EstCloseDate")
                                        {
                                            header = "EstCloseDate";
                                            DateTime EstCloseDate = new DateTime();
                                            if (DateTime.TryParse(value, out EstCloseDate))
                                            {
                                                customer.EstCloseDate = EstCloseDate;
                                            }
                                            else
                                            {
                                                customer.EstCloseDate = null;
                                            }
                                        }
                                        else if (header == "MovingDate")
                                        {
                                            DateTime MovingDate = new DateTime();
                                            if (DateTime.TryParse(value, out MovingDate))
                                            {
                                                customer.MovingDate = MovingDate;
                                            }
                                            else
                                            {
                                                customer.MovingDate = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "SalesDate")
                                        {
                                            DateTime SalesDate = new DateTime();
                                            if (DateTime.TryParse(value, out SalesDate))
                                            {
                                                customer.SalesDate = SalesDate;
                                            }
                                            else
                                            {
                                                customer.SalesDate = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "InstallDate")
                                        {
                                            DateTime InstallDate = new DateTime();
                                            if (DateTime.TryParse(value, out InstallDate))
                                            {
                                                customer.InstallDate = InstallDate;
                                            }
                                            else
                                            {
                                                customer.InstallDate = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "CutInDate")
                                        {
                                            DateTime CutInDate = new DateTime();
                                            if (DateTime.TryParse(value, out CutInDate))
                                            {
                                                customer.CutInDate = CutInDate;
                                            }
                                            else
                                            {
                                                customer.CutInDate = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "FundingDate")
                                        {
                                            DateTime FundingDate = new DateTime();
                                            if (DateTime.TryParse(value, out FundingDate))
                                            {
                                                customer.FundingDate = FundingDate;
                                            }
                                            else
                                            {
                                                customer.FundingDate = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "JoinDate")
                                        {
                                            DateTime JoinDate = new DateTime();
                                            if (DateTime.TryParse(value, out JoinDate))
                                            {
                                                customer.JoinDate = JoinDate;
                                            }
                                            else
                                            {
                                                customer.JoinDate = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "ReminderDate")
                                        {
                                            DateTime ReminderDate = new DateTime();
                                            if (DateTime.TryParse(value, out ReminderDate))
                                            {
                                                customer.ReminderDate = ReminderDate;
                                            }
                                            else
                                            {
                                                customer.ReminderDate = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "QA1Date")
                                        {
                                            DateTime QA1Date = new DateTime();
                                            if (DateTime.TryParse(value, out QA1Date))
                                            {
                                                customer.QA1Date = QA1Date;
                                            }
                                            else
                                            {
                                                customer.QA1Date = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "QA2Date")
                                        {
                                            DateTime QA2Date = new DateTime();
                                            if (DateTime.TryParse(value, out QA2Date))
                                            {
                                                customer.QA2Date = QA2Date;
                                            }
                                            else
                                            {
                                                customer.QA2Date = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "QA2Date")
                                        {
                                            DateTime QA2Date = new DateTime();
                                            if (DateTime.TryParse(value, out QA2Date))
                                            {
                                                customer.QA2Date = QA2Date;
                                            }
                                            else
                                            {
                                                customer.QA2Date = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "Project Walk Date" || header == "ProjectWalkDate")
                                        {
                                            DateTime ProjectWalkDate = new DateTime();
                                            if (DateTime.TryParse(value, out ProjectWalkDate))
                                            {
                                                customer.ProjectWalkDate = ProjectWalkDate;
                                            }
                                            else
                                            {
                                                customer.ProjectWalkDate = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "ServiceDate")
                                        {
                                            DateTime ServiceDate = new DateTime();
                                            if (DateTime.TryParse(value, out ServiceDate))
                                            {
                                                customer.ServiceDate = ServiceDate;
                                            }
                                            else
                                            {
                                                customer.ServiceDate = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "FirstBilling")
                                        {
                                            DateTime FirstBilling = new DateTime();
                                            if (DateTime.TryParse(value, out FirstBilling))
                                            {
                                                customer.FirstBilling = FirstBilling;
                                            }
                                            else
                                            {
                                                customer.FirstBilling = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "LastGeneratedInvoice")
                                        {
                                            DateTime LastGeneratedInvoice = new DateTime();
                                            if (DateTime.TryParse(value, out LastGeneratedInvoice))
                                            {
                                                customer.LastGeneratedInvoice = LastGeneratedInvoice;
                                            }
                                            else
                                            {
                                                customer.LastGeneratedInvoice = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "EstCloseDate")
                                        {
                                            DateTime EstCloseDate = new DateTime();
                                            if (DateTime.TryParse(value, out EstCloseDate))
                                            {
                                                customer.EstCloseDate = EstCloseDate;
                                            }
                                            else
                                            {
                                                customer.EstCloseDate = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "FollowUpDate")
                                        {
                                            DateTime FollowUpDate = new DateTime();
                                            if (DateTime.TryParse(value, out FollowUpDate))
                                            {
                                                customer.FollowUpDate = FollowUpDate;
                                            }
                                            else
                                            {
                                                customer.FollowUpDate = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "OriginalContactDate")
                                        {
                                            DateTime OriginalContactDate = new DateTime();
                                            if (DateTime.TryParse(value, out OriginalContactDate))
                                            {
                                                customer.OriginalContactDate = OriginalContactDate;
                                            }
                                            else
                                            {
                                                customer.OriginalContactDate = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "DoNotCall")
                                        {
                                            DateTime DoNotCall = new DateTime();
                                            if (DateTime.TryParse(value, out DoNotCall))
                                            {
                                                customer.DoNotCall = DoNotCall;
                                            }
                                            else
                                            {
                                                customer.DoNotCall = null;
                                            }
                                            continue;
                                        }

                                        #endregion

                                        #region Doubles
                                        else if (header == "BillAmount")
                                        {
                                            Double BillAmount = new Double();
                                            if (Double.TryParse(value, out BillAmount))
                                            {
                                                customer.BillAmount = BillAmount;
                                            }
                                            else
                                            {
                                                customer.BillAmount = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "BillOutStanding")
                                        {
                                            Double BillOutStanding = new Double();
                                            if (Double.TryParse(value, out BillOutStanding))
                                            {
                                                customer.BillOutStanding = BillOutStanding;
                                            }
                                            else
                                            {
                                                customer.BillOutStanding = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "ActivationFee")
                                        {
                                            Double ActivationFee = new Double();
                                            if (Double.TryParse(value, out ActivationFee))
                                            {
                                                customer.ActivationFee = ActivationFee;
                                            }
                                            else
                                            {
                                                customer.ActivationFee = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "PurchasePrice")
                                        {
                                            Double PurchasePrice = new Double();
                                            if (Double.TryParse(value, out PurchasePrice))
                                            {
                                                customer.PurchasePrice = PurchasePrice;
                                            }
                                            else
                                            {
                                                customer.PurchasePrice = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "Budget")
                                        {
                                            Double Budget = new Double();
                                            if (Double.TryParse(value, out Budget))
                                            {
                                                customer.Budget = Budget;
                                            }
                                            else
                                            {
                                                customer.Budget = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "AnnualRevenue")
                                        {
                                            Double AnnualRevenue = new Double();
                                            if (Double.TryParse(value, out AnnualRevenue))
                                            {
                                                customer.AnnualRevenue = AnnualRevenue;
                                            }
                                            else
                                            {
                                                customer.AnnualRevenue = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "FinancedAmount")
                                        {
                                            Double FinancedAmount = new Double();
                                            if (Double.TryParse(value, out FinancedAmount))
                                            {
                                                customer.FinancedAmount = FinancedAmount;
                                            }
                                            else
                                            {
                                                customer.FinancedAmount = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "SoldAmount")
                                        {
                                            Double SoldAmount = new Double();
                                            if (Double.TryParse(value, out SoldAmount))
                                            {
                                                customer.SoldAmount = SoldAmount;
                                            }
                                            else
                                            {
                                                customer.SoldAmount = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "Levels")
                                        {
                                            Double Levels = new Double();
                                            if (Double.TryParse(value, out Levels))
                                            {
                                                customer.Levels = Levels;
                                            }
                                            else
                                            {
                                                customer.Levels = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "FinancedTerm")
                                        {
                                            Double FinancedTerm = new Double();
                                            if (Double.TryParse(value, out FinancedTerm))
                                            {
                                                customer.FinancedTerm = FinancedTerm;
                                            }
                                            else
                                            {
                                                customer.FinancedTerm = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "BuyoutAmountByADS")
                                        {
                                            Double BuyoutAmountByADS = new Double();
                                            if (Double.TryParse(value, out BuyoutAmountByADS))
                                            {
                                                customer.BuyoutAmountByADS = BuyoutAmountByADS;
                                            }
                                            else
                                            {
                                                customer.BuyoutAmountByADS = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "BuyoutAmountBySalesRep")
                                        {
                                            Double BuyoutAmountBySalesRep = new Double();
                                            if (Double.TryParse(value, out BuyoutAmountBySalesRep))
                                            {
                                                customer.BuyoutAmountBySalesRep = BuyoutAmountBySalesRep;
                                            }
                                            else
                                            {
                                                customer.BuyoutAmountBySalesRep = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "Lat")
                                        {
                                            Double Lat = new Double();
                                            if (Double.TryParse(value, out Lat))
                                            {
                                                customer.Latlng = Lat.ToString();
                                            }
                                            else
                                            {
                                                customer.Latlng = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "Lng")
                                        {
                                            Double Lng = new Double();
                                            if (Double.TryParse(value, out Lng))
                                            {
                                                customer.Latlng = customer.Latlng.ToString() + ',' + Lng.ToString();
                                            }
                                            else
                                            {
                                                customer.Latlng = null;
                                            }
                                            continue;
                                        }
                                        #endregion

                                        #region int
                                        else if (header == "BillDay")
                                        {
                                            int BillDay = new int();
                                            if (int.TryParse(value, out BillDay))
                                            {
                                                customer.BillDay = BillDay;
                                            }
                                            else
                                            {
                                                customer.BillDay = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "CreditScoreValue")
                                        {
                                            int CreditScoreValue = 0;
                                            if (int.TryParse(value, out CreditScoreValue))
                                            {
                                                customer.CreditScoreValue = CreditScoreValue;
                                            }
                                            continue;
                                        }
                                        else if (header == "BranchId")
                                        {
                                            int BranchId = new int();
                                            if (int.TryParse(value, out BranchId))
                                            {
                                                customer.BranchId = BranchId;
                                            }
                                            else
                                            {
                                                customer.BranchId = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "Passengers")
                                        {
                                            int Passengers = new int();
                                            if (int.TryParse(value, out Passengers))
                                            {
                                                customer.Passengers = Passengers;
                                            }
                                            else
                                            {
                                                customer.Passengers = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "SmartSetUpStep")
                                        {
                                            int SmartSetUpStep = new int();
                                            if (int.TryParse(value, out SmartSetUpStep))
                                            {
                                                customer.SmartSetUpStep = SmartSetUpStep;
                                            }
                                            else
                                            {
                                                customer.SmartSetUpStep = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "RenewalTerm")
                                        {
                                            int RenewalTerm = new int();
                                            if (int.TryParse(value, out RenewalTerm))
                                            {
                                                customer.RenewalTerm = RenewalTerm;
                                            }
                                            else
                                            {
                                                customer.RenewalTerm = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "TransferCustomerId")
                                        {
                                            int TransferCustomerId = new int();
                                            if (int.TryParse(value, out TransferCustomerId))
                                            {
                                                customer.TransferCustomerId = TransferCustomerId;
                                            }
                                            else
                                            {
                                                customer.TransferCustomerId = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "GeeseCount")
                                        {
                                            int GeeseCount = new int();
                                            if (int.TryParse(value, out GeeseCount))
                                            {
                                                if (CustomerId != null && CustomerId != new Guid())
                                                {
                                                    CustomerExtended Cus = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(CustomerId);
                                                    if (Cus != null)
                                                    {
                                                        Cus.GeeseCount = GeeseCount;
                                                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(Cus);
                                                    }
                                                    else
                                                    {
                                                        CustomerExtended Extended = new CustomerExtended()
                                                        {
                                                            CustomerId = CustomerGuidId,
                                                            GeeseCount = GeeseCount
                                                        };
                                                        _Util.Facade.CustomerFacade.InsertCustomerExtended(Extended);
                                                    }
                                                }
                                                else
                                                {
                                                    CustomerExtended Extended = new CustomerExtended()
                                                    {
                                                        CustomerId = CustomerGuidId,
                                                        GeeseCount = GeeseCount
                                                    };
                                                    _Util.Facade.CustomerFacade.InsertCustomerExtended(Extended);
                                                }

                                            }
                                            else
                                            {
                                                GeeseCount = 0;
                                            }
                                            continue;


                                        }
                                        #endregion

                                        #region Header fix
                                        else if (header == "First Name")
                                        {
                                            header = "FirstName";
                                        }
                                        else if (header == "Last Name")
                                        {
                                            header = "LastName";
                                        }
                                        else if (header == "Property Type")
                                        {
                                            header = "Type";
                                        }
                                        else if (header == "Lead Type")
                                        {
                                            header = "InstallType";
                                            value = LookUpList.Where(m => m.DisplayText == value && m.DataKey == "LeadInstallType").OrderBy(m => m.DisplayText != "select One").ThenBy(m => m.DisplayText).Select(m => m.DataValue).FirstOrDefault();
                                        }
                                        else if (header == "Existing Panel")
                                        {
                                            header = "EsistingPanel";
                                        }
                                        else if (header == "Business Name")
                                        {
                                            header = "BusinessName";
                                        }
                                        else if (header == "Street Type")
                                        {
                                            header = "StreetType";
                                            value = LookUpList.Where(m => m.DisplayText == value && m.DataKey == "LeadStreetType").Select(m => m.DataValue).FirstOrDefault();
                                        }
                                        else if (header == "Apt/Suite")
                                        {
                                            header = "Appartment";
                                        }
                                        else if (header == "Doing Business As (dba)")
                                        {
                                            header = "DBA";
                                        }
                                        else if (header == "Cross Street")
                                        {
                                            header = "CrossStreet";
                                        }
                                        else if (header == "Zip" || header == "Postal Code")
                                        {
                                            header = "ZipCode";
                                        }
                                        else if (header == "Site Phone" || header == "Phone Number" || header == "PrimaryPhone")
                                        {
                                            header = "PrimaryPhone";
                                            value = value.PhoneNumberFormat();
                                        }
                                        //else if (header == "Branch")
                                        //{
                                        //    header = "BranchId";
                                        //    if (value != null)
                                        //    {
                                        //        value = Convert.ToInt32(value);
                                        //    }
                                        //}
                                        else if (header == "Cell Phone")
                                        {
                                            header = "SecondaryPhone";
                                        }
                                        else if (header == "Email")
                                        {
                                            header = "EmailAddress";
                                        }
                                        else if (header == "Phone Type")
                                        {
                                            header = "PhoneType";
                                            value = LookUpList.Where(m => m.DisplayText == value && m.DataKey == "PhoneType").OrderBy(m => m.DataValue.ToString() != "-1").ThenBy(m => m.DataValue).Select(m => m.DataValue).FirstOrDefault();
                                        }
                                        //else if (header == "Sales Person" || header == "Soldby")
                                        //{
                                        //    header = "Soldby"; 
                                        //}
                                        else if (header == "Verbal Password")
                                        {
                                            header = "Passcode";
                                        }
                                        else if (header == "Status")
                                        {
                                            header = "Status";
                                            value = LookUpList.Where(m => m.DisplayText == value && m.DataKey == "LeadStatus").OrderBy(m => m.DataValue.ToString() != "-1").ThenBy(m => m.DisplayText).Select(m => m.DataValue).FirstOrDefault();
                                        }
                                        else if (header == "Lead Source" || header == "LeadSource")
                                        {
                                            header = "LeadSource";
                                            if (string.IsNullOrWhiteSpace(value))
                                            {
                                                value = "-1";
                                            }
                                            else
                                            {
                                                value = LookUpList.Where(m => m.DataKey == "LeadSource" && (m.DisplayText == value || m.DataValue == value)).OrderBy(m => m.DataValue.ToString() != "-1").ThenBy(m => m.DisplayText)
                                                    .Select(m => m.DataValue).FirstOrDefault();
                                            }
                                            if (string.IsNullOrWhiteSpace(value))
                                            {
                                                value = "-1";
                                            }
                                        }
                                        #endregion

                                        if (customer.GetType().GetProperty(header) != null)
                                        {
                                            #region Geese Relief
                                            if (/*header == "RouteName"*/ false)
                                            {
                                                GeeseRoute Route = _Util.Facade.CustomerFacade.GetGeeseRouteByName(value);
                                                if (Route != null)
                                                {
                                                    if (CustomerId != null && CustomerId != new Guid())
                                                    {
                                                        CustomerRoute ExistingRoute = _Util.Facade.CustomerFacade.GetCustomerRouteByCustomerId(CustomerId);
                                                        if (ExistingRoute != null)
                                                        {
                                                            ExistingRoute.Name = Route.Name;
                                                            ExistingRoute.RouteId = Route.RouteId;
                                                            _Util.Facade.CustomerFacade.UpdateCustomerRoute(ExistingRoute);
                                                        }
                                                        else
                                                        {
                                                            CustomerRoute CRoute = new CustomerRoute()
                                                            {
                                                                CustomerId = CustomerGuidId,
                                                                RouteId = Route.RouteId,
                                                                Name = Route.Name,
                                                                CreatedBy = CurrentUser.UserId,
                                                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                                                UserId = CurrentUser.UserId
                                                            };
                                                            _Util.Facade.CustomerFacade.InsertCustomerRoute(CRoute);
                                                            continue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        CustomerRoute CRoute = new CustomerRoute()
                                                        {
                                                            CustomerId = CustomerGuidId,
                                                            RouteId = Route.RouteId,
                                                            Name = Route.Name,
                                                            CreatedBy = CurrentUser.UserId,
                                                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                                                            UserId = CurrentUser.UserId
                                                        };
                                                        _Util.Facade.CustomerFacade.InsertCustomerRoute(CRoute);
                                                        continue;
                                                    }
                                                }
                                            }
                                            #endregion
                                            else
                                            {
                                                customer.GetType().GetProperty(header).SetValue(customer, value);
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            logger.Error(e);
                        }

                        //customer.CustomerId = CustomerId;
                        customer.CustomerId = CustomerGuidId;
                        Customer isExistCustomer = new Customer();
                        bool CustomerInserted = true;
                        if (customer.Id > 0)
                        {
                            isExistCustomer = _Util.Facade.CustomerFacade.GetCustomerById(customer.Id);
                        }
                        if (customer.CustomerId != new Guid() || isExistCustomer != null)
                        {
                            if (isExistCustomer == null)
                            {
                                isExistCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customer.CustomerId);
                            }
                            if (isExistCustomer != null && isExistCustomer.CustomerId != Guid.Empty && isExistCustomer.Id > 0)
                            {
                                CustomerInserted = false;

                                customer.Id = isExistCustomer.Id;
                                customer.CustomerId = isExistCustomer.CustomerId;
                                isExistCustomer.FirstName = customer.FirstName;
                                isExistCustomer.LastName = customer.LastName;
                                isExistCustomer.BusinessName = customer.BusinessName;
                                isExistCustomer.Street = customer.Street;
                                isExistCustomer.EsistingPanel = customer.EsistingPanel;
                                isExistCustomer.StreetType = customer.StreetType;
                                isExistCustomer.Address = customer.Address;
                                isExistCustomer.CrossStreet = customer.CrossStreet;
                                isExistCustomer.DBA = customer.DBA;
                                isExistCustomer.Appartment = customer.Appartment;
                                isExistCustomer.State = customer.State;
                                isExistCustomer.ZipCode = customer.ZipCode;
                                isExistCustomer.City = customer.City;
                                isExistCustomer.Country = customer.Country;
                                isExistCustomer.SecondaryPhone = customer.SecondaryPhone;
                                isExistCustomer.PrimaryPhone = customer.PrimaryPhone;
                                isExistCustomer.EmailAddress = customer.EmailAddress;
                                isExistCustomer.Soldby = customer.Soldby;
                                isExistCustomer.PhoneType = customer.PhoneType;
                                isExistCustomer.ProjectWalkDate = customer.ProjectWalkDate;
                                isExistCustomer.EstCloseDate = customer.EstCloseDate;
                                isExistCustomer.Status = customer.Status;
                                isExistCustomer.Note = customer.Note;
                                isExistCustomer.LeadSource = customer.LeadSource;
                                isExistCustomer.SSN = customer.SSN;
                                isExistCustomer.Type = customer.Type;
                                isExistCustomer.MovingDate = customer.MovingDate;
                                isExistCustomer.AdditionalCustomerNo = customer.AdditionalCustomerNo;
                                isExistCustomer.LastUpdatedBy = User.Identity.Name;
                                isExistCustomer.LastUpdatedDate = CreatedDate;

                                result = _Util.Facade.CustomerFacade.UpdateCustomer(isExistCustomer);

                                //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\CustomerImportReport.txt"), true))
                                //{
                                //    file.WriteLine("CustomerExists: {0}", isExistCustomer.Id);
                                //}

                            }
                            else
                            {
                                if (CustomerGuidId != null && CustomerGuidId != new Guid())
                                {
                                    CustomerGuidId = customer.CustomerId;
                                }
                                else
                                {
                                    CustomerGuidId = Guid.NewGuid();
                                }
                                customer.Address = MakeAddress(customer.Street, customer.City, customer.State, customer.ZipCode, customer.Country);
                                customer.JoinDate = CreatedDate;
                                customer.IsTechCallPassed = false;
                                customer.LastUpdatedDate = CreatedDate;
                                customer.LastUpdatedBy = User.Identity.Name;
                                customer.IsDirect = false;
                                customer.IsActive = true;
                                customer.CreatedDate = CreatedDate;
                                customer.CreatedByUid = CurrentUser.UserId;
                                customer.EmailVerified = false;
                                customer.CustomerId = CustomerGuidId; //CustomerId;
                                //customer.SoldBy2 = new Guid();
                                //customer.SoldBy3 = new Guid();
                                customer.Id = (int)_Util.Facade.CustomerFacade.InsertCustomer(customer);
                            }
                        }
                        else
                        {
                            customer.CustomerId = Guid.NewGuid();
                            customer.Address = MakeAddress(customer.Street, customer.City, customer.State, customer.ZipCode, customer.Country);
                            if (customer.JoinDate == null)
                            {
                                customer.JoinDate = CreatedDate;
                            }
                            customer.IsTechCallPassed = false;
                            customer.LastUpdatedDate = CreatedDate;
                            customer.LastUpdatedBy = User.Identity.Name;
                            customer.IsDirect = false;
                            customer.IsActive = true;
                            customer.CreatedDate = CreatedDate;
                            customer.CreatedByUid = CurrentUser.UserId;
                            customer.EmailVerified = false;
                            //customer.SoldBy2 = new Guid();
                            //customer.SoldBy3 = new Guid();
                            customer.Id = (int)_Util.Facade.CustomerFacade.InsertCustomer(customer);
                        }
                        result = customer.Id > 0;
                        if (result == true && CustomerInserted)
                        {
                            #region Add CustomerCompany
                            CustomerCompany cc = new CustomerCompany();
                            if (isCustomer == "True")
                            {
                                cc = new CustomerCompany()
                                {
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    CustomerId = customer.CustomerId,
                                    IsLead = false,
                                    IsActive = true
                                };
                            }
                            else
                            {
                                cc = new CustomerCompany()
                                {
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    CustomerId = customer.CustomerId,
                                    IsLead = true,
                                    //IsActive = true
                                };
                            }
                            result = _Util.Facade.CustomerFacade.InsertCustomerCompany(cc) > 0;
                            #endregion

                            #region Add CustomerSnapShot
                            CustomerSnapshot LeadSnapShot = new CustomerSnapshot
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                CustomerId = customer.CustomerId,
                                Description = "Created at : " + customer.JoinDate,
                                Type = "CustomerCreatedHistory",
                                Updatedby = User.Identity.Name,
                                Logdate = CreatedDate
                            };
                            result = _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(LeadSnapShot);
                            #endregion

                            #region Add SystemInfo
                            if (result == true)
                            {
                                CustomerSystemInfo objsysinfo = new CustomerSystemInfo()
                                {
                                    CustomerId = customer.CustomerId,
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    InstallType = customer.InstallType
                                };
                                _Util.Facade.CustomerSystemInfoFacade.InsertSystemInfo(objsysinfo);
                            }
                            #endregion
                        }
                    }
                    object misValue = System.Reflection.Missing.Value;
                    //xlWorkbook.Close(true, misValue, misValue);
                    //xlApp.Quit();

                    //Marshal.ReleaseComObject(xlWorksheet);
                    //Marshal.ReleaseComObject(xlWorkbook);
                    //Marshal.ReleaseComObject(xlApp);
                    System.IO.DirectoryInfo dir = new DirectoryInfo(serverFile);
                    if (ExcelFile != null)
                    {
                        ExcelFile.Delete();
                    }
                }
            }
            else
            {
                return Json(new { result = true, message = "File not exsists" });
            }
            return Json(new { result = true, isCustomer = isCustomer });
        }
        public void CustomerContact(Customer customer)
        {

            if (!string.IsNullOrWhiteSpace(customer.EmailAddress) || !string.IsNullOrWhiteSpace(customer.SecondaryPhone) || !string.IsNullOrWhiteSpace(customer.CellNo))
            {
                bool existEmail = _Util.Facade.ContactFacade.ExistEmailorCellNo(customer.EmailAddress, customer.PrimaryPhone, customer.SecondaryPhone);
                //UserContact uc = _Util.Facade.ContactFacade.GetUserContactsByCustomerId(customer.CustomerId);
                Guid Soldby = Guid.Empty;
                Guid.TryParse(customer.Soldby, out Soldby);
                if (existEmail == false)
                {
                    Contact contact = new Contact()
                    {
                        ContactOwner = Soldby,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        ContactId = Guid.NewGuid(),
                        CreatedBy = customer.CreatedByUid,
                        CreatedDate = customer.CreatedDate,
                        Mobile = customer.PrimaryPhone,
                        Work = customer.SecondaryPhone,
                        Email = customer.EmailAddress,
                        Notes = customer.Note,
                    };
                    _Util.Facade.ContactFacade.InsertContacts(contact);

                    UserContact userContact = new UserContact()
                    {
                        UserType = LabelHelper.UserType.Customer,
                        UserId = customer.CustomerId,
                        ContactId = contact.ContactId,
                    };
                    _Util.Facade.ContactFacade.InsertUserContacts(userContact);


                }
            }


        }
        [Authorize]
        [HttpPost]
        public JsonResult AddLeads(Customer customer, CustomerSystemInfo csi, CustomerExtended customerExtended)
        {

            customer.CustomerExtended = customerExtended;
            bool soldbyChange = false;
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var result = false;
            var resultcusextended = false;
            Customer Cusmodel = _Util.Facade.CustomerFacade.GetCustomersByIdAndSoldBy(customer.Id, CurrentUser.UserId, "", "");

            if (CurrentUser == null)
            {
                return Json(result);
            }
            if (!CurrentUser.CompanyId.HasValue)
            {
                return Json(result);
            }
            try
            {
                customer.Soldby1 = Guid.Parse(customer.Soldby);
            }
            catch (Exception e)
            {
                customer.Soldby1 = Guid.Empty;
            }
            List<Employee> employee = _Util.Facade.EmployeeFacade.GetEmployeeByuserIdandCompanyId(CurrentUser.CompanyId.Value, CurrentUser.UserId);

            if (customer.Id > 0)
            {
                #region Update Existing Lead

                Customer c = _Util.Facade.CustomerFacade.GetById(customer.Id);
                #region Check if customer save to lead
                CustomerCompany cc2 = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerIdAndCompanyId(c.CustomerId, CurrentUser.CompanyId.Value);
                if (cc2 != null && cc2.IsLead == false)
                {
                    result = true;
                    return Json(new { result = result, LeadId = c.Id });
                }
                #endregion
                if (c != null && customer != null)
                {
                    if (c.Soldby1 != customer.Soldby1)
                    {
                        soldbyChange = true;
                    }
                }
                if (c.EmailAddress != customer.EmailAddress)
                {
                    c.EmailVerified = false;
                }
                if (c.PrimaryPhone != customer.PrimaryPhone)
                {
                    if (customer.IsPrimaryPhoneVerified == true)
                    {
                        c.IsPrimaryPhoneVerified = true;
                    }
                    else
                    {
                        c.IsPrimaryPhoneVerified = false;
                    }
                    if (customer.PrimaryPhone != null)
                    {
                        customer.PrimaryPhone = customer.PrimaryPhone.TrimPhoneNum();
                    }
                }
                else
                {
                    if (c.IsPrimaryPhoneVerified != true)
                    {
                        c.IsPrimaryPhoneVerified = customer.IsPrimaryPhoneVerified;
                    }
                    if (customer.PrimaryPhone != null)
                    {
                        customer.PrimaryPhone = customer.PrimaryPhone.TrimPhoneNum();
                    }
                }
                if (c.SecondaryPhone != customer.SecondaryPhone)
                {
                    if (customer.IsSecondaryPhoneVerified == true)
                    {
                        c.IsSecondaryPhoneVerified = true;
                    }
                    else
                    {
                        c.IsSecondaryPhoneVerified = false;
                    }
                    if (customer.SecondaryPhone != null)
                    {
                        customer.SecondaryPhone = customer.SecondaryPhone.TrimPhoneNum();
                    }
                }
                else
                {
                    if (c.IsSecondaryPhoneVerified != true)
                    {
                        c.IsSecondaryPhoneVerified = customer.IsSecondaryPhoneVerified;
                    }
                    if (customer.SecondaryPhone != null)
                    {
                        customer.SecondaryPhone = customer.SecondaryPhone.TrimPhoneNum();
                    }
                }
                if (c.CellNo != customer.CellNo)
                {
                    if (customer.IsCellNoVerified == true)
                    {
                        c.IsCellNoVerified = true;
                    }
                    else
                    {
                        c.IsCellNoVerified = false;
                    }
                    if (customer.CellNo != null)
                    {
                        customer.CellNo = customer.CellNo.TrimPhoneNum();
                    }
                }
                else
                {
                    if (c.IsCellNoVerified != true)
                    {
                        c.IsCellNoVerified = customer.IsCellNoVerified;
                    }
                    if (customer.CellNo != null)
                    {
                        customer.CellNo = customer.CellNo.TrimPhoneNum();
                    }
                }
                if (!string.IsNullOrEmpty(customer.HomeOwner))
                {
                    c.HomeOwner = customer.HomeOwner;
                }
                customer.CustomerId = c.CustomerId;

                #region Add CustomerNote if Note not matches
                if (/*!string.IsNullOrWhiteSpace(customer.Note) &&*/ !string.IsNullOrWhiteSpace(customer.Note) && customer.Note != c.Note)
                {
                    CustomerNote objnote = _Util.Facade.CustomerFacade.GetCustomerNoteByCustomerIdAndIsPrimary(customer.CustomerId);
                    if (objnote != null)
                    {
                        objnote.Notes = customer.Note;
                        objnote.CustomerId = customer.CustomerId;
                        objnote.CompanyId = CurrentUser.CompanyId.Value;
                        objnote.IsPrimaryNote = true;
                        //objnote.CreatedDate = DateTime.Now.UTCCurrentTime();
                        //objnote.CreatedBy = User.Identity.Name;
                        //objnote.CreatedByUid = CurrentUser.UserId;

                        _Util.Facade.NotesFacade.UpdateNotes(objnote);
                    }
                    else
                    {
                        objnote = new CustomerNote()
                        {
                            Notes = customer.Note,
                            CustomerId = customer.CustomerId,
                            CompanyId = CurrentUser.CompanyId.Value,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            CreatedBy = User.Identity.Name,
                            IsPrimaryNote = true,
                            CreatedByUid = CurrentUser.UserId
                        };
                        _Util.Facade.NotesFacade.InsertCustomerNote(objnote);
                    }

                }
                #endregion


                #region Updates from view
                //New Update...
                c.FirstName = customer.FirstName;
                c.LastName = customer.LastName;
                c.LeadSource = customer.LeadSource;
                c.InstalledStatus = customer.InstalledStatus;
                c.AcquiredFrom = customer.AcquiredFrom;
                c.FollowUpDate = customer.FollowUpDate;
                c.BuyoutAmountByADS = customer.BuyoutAmountByADS;
                c.BuyoutAmountBySalesRep = customer.BuyoutAmountBySalesRep;
                c.FinancedTerm = customer.FinancedTerm;
                c.FinancedAmount = customer.FinancedAmount;
                c.EsistingPanel = customer.EsistingPanel;
                c.BusinessName = customer.BusinessName;
                if (!string.IsNullOrEmpty(customer.Street))
                {
                    c.Street = customer.Street.Replace("'", "`");
                }

                c.StreetType = customer.StreetType;
                c.Appartment = customer.Appartment;
                c.ZipCode = customer.ZipCode;
                c.City = customer.City;
                c.State = customer.State;
                c.PersonSales = customer.PersonSales;
                c.EmailAddress = customer.EmailAddress;
                c.Address2 = customer.Address2;
                c.Address2 = !string.IsNullOrEmpty(c.Address2) ? c.Address2.Replace("'", "`") : "";
                c.PhoneType = customer.PhoneType;
                c.Carrier = customer.Carrier;
                c.Status = customer.Status;
                c.ReferringCustomer = customer.ReferringCustomer;
                c.Note = customer.Note;
                c.Address = MakeAddress(customer.Street, customer.City, customer.State, customer.ZipCode, customer.Country);
                c.Address = !string.IsNullOrEmpty(c.Address) ? c.Address.Replace("'", "`") : "";
                c.LastUpdatedBy = User.Identity.Name;
                c.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                c.DBA = customer.DBA;
                c.PhoneType = customer.PhoneType;
                c.PrimaryPhone = customer.PrimaryPhone;
                c.SecondaryPhone = customer.SecondaryPhone;
                c.CellNo = customer.CellNo;
                //c.Soldby = customer.Soldby;
                c.County = customer.County;
                c.LeadSource = customer.LeadSource;
                c.BranchId = customer.BranchId;
                c.EstCloseDate = customer.EstCloseDate;
                c.ProjectWalkDate = customer.ProjectWalkDate;
                c.CrossStreet = customer.CrossStreet;
                c.Market = customer.Market;
                c.Passengers = customer.Passengers;
                c.Budget = customer.Budget;
                c.Type = customer.Type;
                if (!string.IsNullOrEmpty(customer.LeadSourceType))
                {
                    c.LeadSourceType = customer.LeadSourceType;
                }
                c.SSN = customer.SSN;
                c.AccessGivenTo = customer.AccessGivenTo;
                c.PreferredContactMethod = customer.PreferredContactMethod;
                c.PreferedCall = customer.PreferedCall;
                c.PreferedSms = customer.PreferedSms;
                c.PreferedEmail = customer.PreferedEmail;
                c.DoNotCall = customer.DoNotCall;
                c.SalesLocation = customer.SalesLocation;
                c.BestTimeToCall = customer.BestTimeToCall;
                c.CSProvider = customer.CSProvider;
                c.CustomerAccountType = customer.CustomerAccountType;
                c.DateofBirth = customer.DateofBirth;
                c.SecondCustomerNo = customer.SecondCustomerNo;
                c.Soldby1 = customer.Soldby1;
                c.SoldBy2 = customer.SoldBy2;
                c.SoldBy3 = customer.SoldBy3;
                c.Passcode = customer.Passcode;
                c.Levels = customer.Levels;
                c.SoldAmount = customer.SoldAmount;
                c.InspectionCompany = customer.InspectionCompany;
                c.TaxExemption = customer.TaxExemption;
                c.AppoinmentSet = customer.AppoinmentSet;
                c.AgreementEmail = customer.EmailAddress;
                c.AgreementPhoneNo = customer.CellNo;
                c.ContractValue = customer.ContractValue;
                c.MapscoNo = customer.MapscoNo;
                c.MoveCustomerId = customer.MoveCustomerId;
                c.Website = customer.Website;
                c.CreditScore = customer.CreditScore;
                c.CreditScoreValue = customer.CreditScoreValue;
                c.Ownership = customer.Ownership;
                c.Carrier = customer.Carrier;
                c.CustomerNo = customer.CustomerNo;
                c.AccountNo = customer.AccountNo;
                c.BusinessAccountType = customer.BusinessAccountType;
                if (customer.CreditScoreValue.HasValue)
                {
                    CreditScoreGrade grade = _Util.Facade.CustomerFacade.GetCreditScoreGradeByScoreRange(customer.CreditScoreValue.Value);
                    if (grade != null)
                    {
                        c.CreditScore = grade.ID.ToString();
                    }
                }
                //---------------
                //customer.Address = MakeAddress(customer.Street, customer.City, customer.State, customer.ZipCode, customer.Country);
                //customer.LastUpdatedBy = User.Identity.Name;
                //customer.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                //customer.CustomerId = c.CustomerId;
                //customer.IsTechCallPassed = false;
                //customer.IsActive = true;
                //customer.ActivationFeePaymentMethod = c.ActivationFeePaymentMethod;
                //customer.CreatedDate = c.CreatedDate;
                //customer.JoinDate = c.JoinDate;
                //customer.CreatedByUid = c.CreatedByUid;
                //customer.EmailVerified = c.EmailVerified;
                #endregion

                #region Notification 
                string CustomerName = c.FirstName + " " + c.LastName;
                if (!string.IsNullOrWhiteSpace(c.BusinessName))
                {
                    CustomerName = c.BusinessName;
                }
                if (soldbyChange)
                {
                    Notification notification = new Notification()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        NotificationId = Guid.NewGuid(),
                        Type = LabelHelper.NotificationType.Employee,
                        Who = CurrentUser.UserId,
                        What = string.Format(@"{0} (<a class=""cus-anchor"" href=""/UserInformation/?id={2}"">{2}</a>) assigned a lead {1} to you", "{0}", CustomerName, employee.FirstOrDefault().UserLoginId),
                        NotificationUrl = "/Lead/Leadsdetail/?id=" + c.Id
                    };
                    _Util.Facade.NotificationFacade.InsertNotification(notification);

                    #region set user to notification
                    NotificationUser nu = new NotificationUser()
                    {
                        NotificationId = notification.NotificationId,
                        IsRead = false,
                        NotificationPerson = c.Soldby1,
                    };
                    _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                    #endregion
                    #region Email Lead Creation
                    if (customer != null && !string.IsNullOrWhiteSpace(customer.Soldby) && customer.Soldby != "00000000-0000-0000-0000-000000000000" && customer.Soldby != CurrentUser.UserId.ToString())
                    {
                        Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(new Guid(customer.Soldby));
                        Employee cEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                        GlobalSetting leadCreationMail = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "SendEmailLeadCreation");
                        if (leadCreationMail != null && leadCreationMail.Value.ToLower() == "true")
                        {
                            LeadCreation lc = new LeadCreation
                            {
                                CustomerNum = emp.FirstName + " " + emp.LastName,
                                ToEmail = emp.Email,
                                CustomerId = emp.UserId.ToString(),
                                EmployeeId = CurrentUser.CompanyId.ToString(),
                                BodyContent = string.Format(@"{0} (<a class=""cus-anchor"" href=""/UserInformation/?id={2}"">{2}</a>) assigned a lead {1} to you", cEmp.FirstName + " " + cEmp.LastName, CustomerName, employee.FirstOrDefault().UserLoginId)
                            };
                            _Util.Facade.MailFacade.SendMailLeadCreation(lc, CurrentUser.CompanyId.Value);
                        }
                    }
                    #endregion
                }
                #endregion

                //if (!string.IsNullOrWhiteSpace(c.BusinessName))
                //{
                //    c.Type = "Commercial";
                //}
                //else
                //{
                //    c.Type = "Residential";
                //}
                if (customer.CustomerAccountTypeList != null && customer.CustomerAccountTypeList.Count > 0)
                {
                    c.CustomerAccountType = string.Join(", ", customer.CustomerAccountTypeList);
                }
                c.Soldby = customer.Soldby;

                result = _Util.Facade.CustomerFacade.UpdateCustomer(c);

                #region Update Leads CustomerCompany
                CustomerCompany cc = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerIdAndCompanyId(customer.CustomerId, CurrentUser.CompanyId.Value);
                if (cc == null)
                {
                    cc = new CustomerCompany()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = customer.CustomerId,
                        IsLead = true,
                        IsActive = true
                    };
                    _Util.Facade.CustomerFacade.InsertCustomerCompany(cc);
                }
                else
                {
                    cc.IsLead = true;
                    _Util.Facade.CustomerFacade.UpdateCustomerCompany(cc);
                }
                #endregion

                #region CustomerSystemInfo Insert/Update
                if (csi.Id > 0)
                {
                    CustomerSystemInfo objcsi = _Util.Facade.CustomerSystemInfoFacade.GetSystemInfoById(csi.Id);
                    if (objcsi != null)
                    {
                        objcsi.CustomerId = c.CustomerId;
                        objcsi.CompanyId = CurrentUser.CompanyId.Value;
                        objcsi.InstallType = csi.InstallType;
                        _Util.Facade.CustomerSystemInfoFacade.UpdateSystemInfo(objcsi);
                    }
                }
                else
                {
                    CustomerSystemInfo objsysinfo = new CustomerSystemInfo()
                    {
                        CustomerId = c.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        InstallType = csi.InstallType
                    };
                    _Util.Facade.CustomerSystemInfoFacade.InsertSystemInfo(objsysinfo);
                }
                #endregion

                customer = c;

                Logmessage(customer, Cusmodel);


                #endregion
            }
            else
            {
                #region Add New Lead
                #region New Lead Add
                customer.CustomerId = Guid.NewGuid();
                customer.Address = MakeAddress(customer.Street, customer.City, customer.State, customer.ZipCode, customer.Country);
                customer.Address = !string.IsNullOrEmpty(customer.Address) ? customer.Address.Replace("'", "`") : "";
                customer.JoinDate = DateTime.Now;
                customer.IsTechCallPassed = false;
                customer.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                customer.LastUpdatedBy = User.Identity.Name;
                customer.IsDirect = false;
                customer.IsActive = true;
                customer.CreatedDate = DateTime.Now.UTCCurrentTime();
                customer.CreatedByUid = CurrentUser.UserId;
                customer.AgreementEmail = customer.EmailAddress;
                customer.AgreementPhoneNo = customer.CellNo;
                List<Lookup> OwnerShipList = _Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("OwnerShip", null, null).ToList();
                if (OwnerShipList != null && OwnerShipList.Count() > 0)
                {
                    foreach (var item in OwnerShipList)
                    {

                        if (CurrentUser.CompanyName.ToLower().IndexOf(item.DisplayText.ToLower()) > -1)
                        {
                            customer.Ownership = item.DisplayText;
                            break;
                        }
                    }
                }

                if (customer.CreditScoreValue.HasValue)
                {
                    CreditScoreGrade grade = _Util.Facade.CustomerFacade.GetCreditScoreGradeByScoreRange(customer.CreditScoreValue.Value);
                    if (grade != null)
                    {
                        customer.CreditScore = grade.ID.ToString();
                    }
                }
                //if (!string.IsNullOrWhiteSpace(customer.BusinessName))
                //{
                //    customer.Type = "Commercial";
                //}
                //else
                //{
                //    customer.Type = "Residential";

                //if(customer.SoldBy2 == null)
                //{
                //    customer.SoldBy2 = new Guid();
                //}
                //if (customer.SoldBy3 == null)
                //{
                //    customer.SoldBy3 = new Guid();
                //}
                if (customer.CustomerAccountTypeList != null && customer.CustomerAccountTypeList.Count > 0)
                {
                    customer.CustomerAccountType = string.Join(", ", customer.CustomerAccountTypeList);
                }

                customer.Id = (int)_Util.Facade.CustomerFacade.InsertCustomer(customer);

                base.AddUserActivityForCustomer("New Lead Created #Ref: Lead#" + customer.Id, LabelHelper.ActivityAction.AddCustomer, customer.CustomerId, customer.Id, customer.Id.ToString());

                result = customer.Id > 0;
                #endregion
                #region Add CustomerNote
                if (!string.IsNullOrWhiteSpace(customer.Note))
                {
                    CustomerNote objnote = new CustomerNote()
                    {
                        Notes = customer.Note,
                        CustomerId = customer.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        CreatedBy = User.Identity.Name,
                        IsPrimaryNote = true,
                        CreatedByUid = CurrentUser.UserId
                    };
                    _Util.Facade.NotesFacade.InsertCustomerNote(objnote);
                }
                #endregion

                #region Notification TO SoldBy
                Guid SoldByVal = new Guid();
                string CustomerName = customer.FirstName + " " + customer.LastName;
                if (!string.IsNullOrWhiteSpace(customer.BusinessName))
                {
                    CustomerName = customer.BusinessName;
                }
                if (Guid.TryParse(customer.Soldby, out SoldByVal) && SoldByVal != new Guid())
                {
                    Notification notification = new Notification()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        NotificationId = Guid.NewGuid(),
                        Type = LabelHelper.NotificationType.Employee,
                        Who = CurrentUser.UserId,
                        //What = string.Format(@"{0} assigned a lead <a class=""cus-anchor"" href=""/Lead/Leadsdetail/?id={1}"">{2}</a> to you", "{0}", customer.Id, CustomerName),
                        What = string.Format(@"{0} (<a class=""cus-anchor"" href=""/UserInformation/?id={3}"">{3}</a>) assigned a lead {2} to you", "{0}", customer.Id, CustomerName, employee.FirstOrDefault().UserLoginId),
                        NotificationUrl = "/Lead/Leadsdetail/?id=" + customer.Id
                    };
                    _Util.Facade.NotificationFacade.InsertNotification(notification);

                    #region set user to notification
                    NotificationUser nu = new NotificationUser()
                    {
                        NotificationId = notification.NotificationId,
                        IsRead = false,
                        NotificationPerson = SoldByVal,
                    };
                    _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                    #endregion
                    #region Email Lead Creation
                    if (customer.Soldby != CurrentUser.UserId.ToString())
                    {
                        Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(new Guid(customer.Soldby));
                        Employee cEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                        GlobalSetting leadCreationMail = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "SendEmailLeadCreation");
                        if (leadCreationMail != null && leadCreationMail.Value.ToLower() == "true")
                        {
                            LeadCreation lc = new LeadCreation
                            {
                                CustomerNum = emp.FirstName + " " + emp.LastName,
                                ToEmail = emp.Email,
                                CustomerId = emp.UserId.ToString(),
                                EmployeeId = CurrentUser.CompanyId.ToString(),
                                BodyContent = string.Format(@"{0} assigned a lead {1} to you", cEmp.FirstName + " " + cEmp.LastName, CustomerName)
                            };
                            _Util.Facade.MailFacade.SendMailLeadCreation(lc, CurrentUser.CompanyId.Value);
                        }
                    }
                    #endregion
                }
                #endregion


                if (result != false)
                {
                    CustomerCompany cc = new CustomerCompany()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = customer.CustomerId,
                        IsLead = true,
                        IsActive = true
                    };
                    result = _Util.Facade.CustomerFacade.InsertCustomerCompany(cc) > 0;




                    #region Add CustomerSnapShot
                    //var CustomerUpdatedby = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customer.CustomerId).LastUpdatedBy;
                    CustomerSnapshot LeadSnapShot = new CustomerSnapshot
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = customer.CustomerId,
                        Description = "Created at : " + customer.JoinDate,
                        Type = "CustomerCreatedHistory",
                        Updatedby = User.Identity.Name,
                        Logdate = DateTime.Now.UTCCurrentTime()
                    };
                    result = _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(LeadSnapShot);
                    #endregion

                    #region Send Email (Not functional)
                    //SetupLeadCustormer SetupLeadCustormer = new SetupLeadCustormer();
                    //SetupLeadCustormer.CustomerName = customer.FirstName + " " + customer.LastName;
                    //if (customer.MiddleName != null)
                    //{
                    //    SetupLeadCustormer.CustomerName = customer.FirstName + " " + customer.MiddleName + " " + customer.LastName;
                    //}
                    //SetupLeadCustormer.ToEmail = customer.EmailAddress;
                    //_Util.Facade.MailFacade.LeadCreateSuccessEmail(SetupLeadCustormer, currentLoggedIn.CompanyId.Value);
                    #endregion

                    #region Add SystemInfo
                    if (result == true)
                    {
                        CustomerSystemInfo objsysinfo = new CustomerSystemInfo()
                        {
                            CustomerId = customer.CustomerId,
                            CompanyId = CurrentUser.CompanyId.Value,
                            InstallType = csi.InstallType
                        };
                        _Util.Facade.CustomerSystemInfoFacade.InsertSystemInfo(objsysinfo);
                    }
                    #endregion

                }
                #endregion
            }


            #region Customer Extended Insert / Update

            #region Check if anything coming from UI
            customer.CustomerExtended = customerExtended;
            if (customer.CustomerExtended == null)
            {
                customer.CustomerExtended = new CustomerExtended();
            }
            #endregion

            CustomerExtended TempCustomerExtended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(customer.CustomerId);
            if (TempCustomerExtended == null)
            {
                CustomerExtended customerExt = new CustomerExtended()
                {
                    CustomerId = customer.CustomerId,
                    FinanceCompany = customerExtended.FinanceCompany,
                    SalesPerson4 = customerExtended.SalesPerson4,
                    IsFinanced = customerExtended.IsFinanced,
                    RWST1 = customerExtended.RWST1,
                    RWST2 = customerExtended.RWST2,
                    RWST3 = customerExtended.RWST3,
                    RWST4 = customerExtended.RWST4,
                    RWST5 = customerExtended.RWST5,
                    RWST6 = customerExtended.RWST6,
                    RWST7 = customerExtended.RWST7,
                    RWST8 = customerExtended.RWST8,
                    RWST9 = customerExtended.RWST9,
                    RWST10 = customerExtended.RWST10,
                    RWST11 = customerExtended.RWST11,
                    RWST12 = customerExtended.RWST12,
                    RWST13 = customerExtended.RWST13,
                    RWST14 = customerExtended.RWST14,
                    RWST15 = customerExtended.RWST15,
                    RepsAssignedDate = customerExtended.RepsAssignedDate,
                    DrivingLicense = customerExtended.DrivingLicense,
                    MonthlyFinanceRate = customerExtended.MonthlyFinanceRate,
                    GrossFundedAmount = customerExtended.GrossFundedAmount,
                    NetFundedAmount = customerExtended.NetFundedAmount,
                    DiscountFundedAmount = customerExtended.DiscountFundedAmount,
                    DiscountFundedPercentage = customerExtended.DiscountFundedPercentage,
                    CustomerPaymentAmount = customerExtended.CustomerPaymentAmount,
                    FinanceRepCommissionRate = customerExtended.FinanceRepCommissionRate,
                    LoanNumber = customerExtended.LoanNumber,
                    CreditAppNumber = customerExtended.CreditAppNumber,
                    Term = customerExtended.Term,
                    APR = customerExtended.APR,
                    MaxCreditLimit = customerExtended.MaxCreditLimit,
                    ApprovalDate = customerExtended.ApprovalDate,
                    Batch = customerExtended.Batch,
                    MonthlyBatch = customerExtended.MonthlyBatch,
                    FinanceRep = customerExtended.FinanceRep,
                    AppoinmentSetBy = customerExtended.AppoinmentSetBy,
                    LeadVersion = customerExtended.LeadVersion,
                    IsSignAgrSendToCus = true,
                    CreatedDay = DateTime.Today,
                    IsTestAccount = false,
                    DealerFee = customerExtended.DealerFee
                };
                _Util.Facade.CustomerFacade.InsertCustomerExtended(customerExt);
            }
            else
            {
                TempCustomerExtended.FinanceCompany = customerExtended.FinanceCompany;
                TempCustomerExtended.SalesPerson4 = customerExtended.SalesPerson4;
                TempCustomerExtended.IsFinanced = customerExtended.IsFinanced;
                TempCustomerExtended.RWST1 = customerExtended.RWST1;
                TempCustomerExtended.RWST2 = customerExtended.RWST2;
                TempCustomerExtended.RWST3 = customerExtended.RWST3;
                TempCustomerExtended.RWST4 = customerExtended.RWST4;
                TempCustomerExtended.RWST5 = customerExtended.RWST5;
                TempCustomerExtended.RWST6 = customerExtended.RWST6;
                TempCustomerExtended.RWST7 = customerExtended.RWST7;
                TempCustomerExtended.RWST8 = customerExtended.RWST8;
                TempCustomerExtended.RWST9 = customerExtended.RWST9;
                TempCustomerExtended.RWST10 = customerExtended.RWST10;
                TempCustomerExtended.RWST11 = customerExtended.RWST11;
                TempCustomerExtended.RWST12 = customerExtended.RWST12;
                TempCustomerExtended.RWST13 = customerExtended.RWST13;
                TempCustomerExtended.RWST14 = customerExtended.RWST14;
                TempCustomerExtended.RWST15 = customerExtended.RWST15;
                TempCustomerExtended.RepsAssignedDate = customerExtended.RepsAssignedDate;
                TempCustomerExtended.DrivingLicense = customerExtended.DrivingLicense;
                TempCustomerExtended.MonthlyFinanceRate = customerExtended.MonthlyFinanceRate;
                TempCustomerExtended.GrossFundedAmount = customerExtended.GrossFundedAmount;
                TempCustomerExtended.NetFundedAmount = customerExtended.NetFundedAmount;
                TempCustomerExtended.DiscountFundedAmount = customerExtended.DiscountFundedAmount;
                TempCustomerExtended.DiscountFundedPercentage = customerExtended.DiscountFundedPercentage;
                TempCustomerExtended.CustomerPaymentAmount = customerExtended.CustomerPaymentAmount;
                TempCustomerExtended.FinanceRepCommissionRate = customerExtended.FinanceRepCommissionRate;
                TempCustomerExtended.LoanNumber = customerExtended.LoanNumber;
                TempCustomerExtended.CreditAppNumber = customerExtended.CreditAppNumber;
                TempCustomerExtended.Term = customerExtended.Term;
                TempCustomerExtended.APR = customerExtended.APR;
                TempCustomerExtended.MaxCreditLimit = customerExtended.MaxCreditLimit;
                TempCustomerExtended.ApprovalDate = customerExtended.ApprovalDate;
                TempCustomerExtended.Batch = customerExtended.Batch;
                TempCustomerExtended.MonthlyBatch = customerExtended.MonthlyBatch;
                TempCustomerExtended.FinanceRep = customerExtended.FinanceRep;
                TempCustomerExtended.AppoinmentSetBy = customerExtended.AppoinmentSetBy;
                TempCustomerExtended.LeadVersion = customerExtended.LeadVersion;
                TempCustomerExtended.DealerFee = customerExtended.DealerFee;

                resultcusextended = _Util.Facade.CustomerFacade.UpdateCustomerExtended(TempCustomerExtended);


            }

            #endregion

            #region Customer Extended
            if (customer.CustomerExtended == null)
            {
                customer.CustomerExtended = new CustomerExtended();
            }

            CustomerExtended _extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(customer.CustomerId);
            if (_extended != null)
            {
                _extended.CustomerId = customer.CustomerId;
                _extended.Pets = customer.CustomerExtended.Pets;
                _extended.PetsType = customer.CustomerExtended.PetsType;
                _extended.Repair = customer.CustomerExtended.Repair;
                _extended.RepairType = customer.CustomerExtended.RepairType;
                _extended.BirthDateCoupon = customer.CustomerExtended.BirthDateCoupon;
                _extended.VipClubMember = customer.CustomerExtended.VipClubMember;
                _extended.SecondaryFirstName = customer.CustomerExtended.SecondaryFirstName;
                _extended.SecondarySSN = customer.CustomerExtended.SecondarySSN;
                _extended.DrivingLicense = customer.CustomerExtended.DrivingLicense;
                _extended.SecondaryEmail = customer.CustomerExtended.SecondaryEmail;
                _extended.SecondaryLastName = customer.CustomerExtended.SecondaryLastName;
                _extended.SecondaryBirthDate = customer.CustomerExtended.SecondaryBirthDate;
                _extended.LeadVersion = customer.CustomerExtended.LeadVersion;

                _Util.Facade.CustomerFacade.UpdateCustomerExtended(_extended);

            }
            else
            {
                _extended = new CustomerExtended()
                {
                    CustomerId = customer.CustomerId,

                    Pets = customer.CustomerExtended.Pets,
                    PetsType = customer.CustomerExtended.PetsType,
                    Repair = customer.CustomerExtended.Repair,
                    RepairType = customer.CustomerExtended.RepairType,
                    BirthDateCoupon = customer.CustomerExtended.BirthDateCoupon,
                    VipClubMember = customer.CustomerExtended.VipClubMember,
                    SecondaryFirstName = customer.CustomerExtended.SecondaryFirstName,
                    SecondarySSN = customer.CustomerExtended.SecondarySSN,
                    DrivingLicense = customer.CustomerExtended.DrivingLicense,
                    SecondaryEmail = customer.CustomerExtended.SecondaryEmail,
                    SecondaryLastName = customer.CustomerExtended.SecondaryLastName,
                    SecondaryBirthDate = customer.CustomerExtended.SecondaryBirthDate,
                    LeadVersion = customer.CustomerExtended.LeadVersion,
                    IsTestAccount = false
                };
                _Util.Facade.CustomerFacade.InsertCustomerExtended(_extended);
            }
            #endregion


            #region Generate Contact
            var PopulateContact = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "AutomaticPopulateContactData");
            if (PopulateContact != null && PopulateContact.Value.ToLower() == "true")
            {
                CustomerContact(customer);
            }

            #endregion
            ViewBag.LeadId = customer.Id;

            return Json(new { result = result, LeadId = ViewBag.LeadId });
        }
        public void Logmessage(Customer customer, Customer Cusmodel)
        {
            #region lead update log
            string leadsourcevalforlog = "";
            string customerstatusvalforlog = "";
            string refcusvalforlognew = "";
            string refcusvalforlogprev = "";
            string childcusvalforlognew = "";
            string childcusvalforlogprev = "";
            string soldbyvalforlognew = "";
            string soldbyvalforlogprev = "";
            string apptsetbyforlognew = "";
            string apptsetbyforlogprev = "";
            string financerepforlognew = "";
            string financerepforlogprev = "";
            string prevcontacttermvalforlog = "";
            string newcontacttermvalforlog = "";
            string prevbranchnameforlog = "";
            string newbranchnameforlog = "";
            string prevsaleslocationforlog = "";
            string newsaleslocationforlog = "";
            string prevcreditgradeforlog = "";
            string newcreditgradeforlog = "";
            string guidstring = "00000000-0000-0000-0000-000000000000";
            CustomerExtended CusExmodel = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(customer.CustomerId);

            if (Cusmodel != null)
            {
                if (Cusmodel.DBA == "")
                {
                    Cusmodel.DBA = null;
                }
                if (customer.DBA == "")
                {
                    customer.DBA = null;
                }

                if (customer.Passcode == "")
                {
                    customer.Passcode = null;
                }
                if (Cusmodel.Passcode == "")
                {
                    Cusmodel.Passcode = null;
                }
                if (Cusmodel.CSProvider == "")
                {
                    Cusmodel.CSProvider = null;
                }
                if (customer.CSProvider == "")
                {
                    Cusmodel.CSProvider = null;
                }
                if (Cusmodel.TaxExemption == "")
                {
                    Cusmodel.TaxExemption = null;
                }
                if (customer.TaxExemption == "")
                {
                    customer.TaxExemption = null;
                }
                if (Cusmodel.SalesLocation == "")
                {
                    Cusmodel.SalesLocation = null;
                }
                if (customer.SalesLocation == "")
                {
                    customer.SalesLocation = null;
                }
                if (Cusmodel.LeadSourceType == "")
                {
                    Cusmodel.LeadSourceType = null;
                }
                if (customer.LeadSourceType == "")
                {
                    customer.LeadSourceType = null;
                }
                if (customer.CustomerExtended != null && customer.CustomerExtended.FinanceCompany == "")
                {
                    customer.CustomerExtended.FinanceCompany = null;
                }
                if (CusExmodel != null && CusExmodel.FinanceCompany == "")
                {
                    CusExmodel.FinanceCompany = null;
                }
                if (customer.CustomerExtended != null && customer.CustomerExtended.Batch == "")
                {
                    customer.CustomerExtended.Batch = null;
                }
                if (CusExmodel != null && CusExmodel.Batch == "")
                {
                    CusExmodel.Batch = null;
                }
                if (Cusmodel.ZipCode == "")
                {
                    Cusmodel.ZipCode = null;
                }
                if (Cusmodel.City == "")
                {
                    Cusmodel.City = null;
                }
                if (Cusmodel.State == "")
                {
                    Cusmodel.State = null;
                }
                if (Cusmodel.Street == "")
                {
                    Cusmodel.Street = null;
                }
                if (Cusmodel.County == "")
                {
                    Cusmodel.County = null;
                }
                if (Cusmodel.Appartment == "")
                {
                    Cusmodel.Appartment = null;
                }
                if (Cusmodel.StreetPrevious == "")
                {
                    Cusmodel.StreetPrevious = null;
                }
                if (Cusmodel.CityPrevious == "")
                {
                    Cusmodel.CityPrevious = null;
                }
                if (Cusmodel.StatePrevious == "")
                {
                    Cusmodel.StatePrevious = null;
                }
                if (Cusmodel.ZipCodePrevious == "")
                {
                    Cusmodel.ZipCodePrevious = null;
                }
                if (customer.ZipCode == "")
                {
                    customer.ZipCode = null;
                }
                if (customer.City == "")
                {
                    customer.City = null;
                }
                if (customer.State == "")
                {
                    customer.State = null;
                }
                if (customer.Street == "")
                {
                    customer.Street = null;
                }
                if (customer.County == "")
                {
                    customer.County = null;
                }
                if (customer.Appartment == "")
                {
                    customer.Appartment = null;
                }
                if (customer.StreetPrevious == "")
                {
                    customer.StreetPrevious = null;
                }
                if (customer.CityPrevious == "")
                {
                    customer.CityPrevious = null;
                }
                if (customer.StatePrevious == "")
                {
                    customer.StatePrevious = null;
                }
                if (customer.ZipCodePrevious == "")
                {
                    customer.ZipCodePrevious = null;
                }
                if (Cusmodel.BillNotes == "")
                {
                    Cusmodel.BillNotes = null;
                }
                if (customer.BillNotes == "")
                {
                    customer.BillNotes = null;
                }
                if (Cusmodel.EmailAddress == "")
                {
                    Cusmodel.EmailAddress = null;
                }
                if (customer.EmailAddress == "")
                {
                    customer.EmailAddress = null;
                }
                if (Cusmodel.BusinessName == "")
                {
                    Cusmodel.BusinessName = null;
                }
                if (customer.BusinessName == "")
                {
                    customer.BusinessName = null;
                }
                if (Cusmodel.CellNo == "")
                {
                    Cusmodel.CellNo = null;
                }
                if (customer.CellNo == "")
                {
                    customer.CellNo = null;
                }
                if (Cusmodel.PrimaryPhone == "")
                {
                    Cusmodel.PrimaryPhone = null;
                }
                if (Cusmodel.AdditionalCustomerNo == "")
                {
                    Cusmodel.AdditionalCustomerNo = null;
                }
                if (customer.AdditionalCustomerNo == "")
                {
                    customer.AdditionalCustomerNo = null;
                }
                if (CusExmodel != null && string.IsNullOrWhiteSpace(CusExmodel.LoanNumber))
                {
                    CusExmodel.LoanNumber = null;
                }

                if (customer.CustomerExtended != null && customer.CustomerExtended.LoanNumber == "")
                {
                    customer.CustomerExtended.LoanNumber = null;
                }
                if (Cusmodel.Note == "")
                {
                    Cusmodel.Note = null;
                }
                if (customer.Note == "")
                {
                    customer.Note = null;
                }
                if (customer.SecondCustomerNo == "")
                {
                    customer.SecondCustomerNo = null;
                }
                if (Cusmodel.SecondCustomerNo == "")
                {
                    Cusmodel.SecondCustomerNo = null;
                }
                if (customer.FirstBilling == new DateTime())
                {
                    customer.FirstBilling = null;
                }
                if (Cusmodel.FirstBilling == new DateTime())
                {
                    Cusmodel.FirstBilling = null;
                }
                if (CusExmodel != null && CusExmodel.APR == "")
                {
                    CusExmodel.APR = null;
                }
                if (customer.CustomerExtended != null && customer.CustomerExtended.APR == "")
                {
                    customer.CustomerExtended.APR = null;
                }
                if (Cusmodel.SSN == "")
                {
                    Cusmodel.SSN = null;
                }
                if (customer.SSN == "")
                {
                    customer.SSN = null;
                }
                if (customer.PaymentMethod == "-1")
                {
                    customer.PaymentMethod = "";
                }
                if (Cusmodel.PaymentMethod == "-1")
                {
                    Cusmodel.PaymentMethod = "";
                }
                if (!string.IsNullOrEmpty(customer.LeadSource) && customer.LeadSource != "-1")
                {
                    leadsourcevalforlog = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(customer.LeadSource);
                }
                if (!string.IsNullOrEmpty(customer.CustomerStatus) && customer.CustomerStatus != "-1")
                {
                    customerstatusvalforlog = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyandDataValueFromLLookup("CustomerStatus1", customer.CustomerStatus);
                }
                if (!string.IsNullOrEmpty(customer.ContractTeam) && customer.ContractTeam != "-1")
                {
                    newcontacttermvalforlog = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyandDataValueFromLLookup("ContractTerm", customer.ContractTeam);
                }
                if (!string.IsNullOrEmpty(Cusmodel.ContractTeam) && Cusmodel.ContractTeam != "-1")
                {
                    prevcontacttermvalforlog = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyandDataValueFromLLookup("ContractTerm", Cusmodel.ContractTeam);
                }
                if (!string.IsNullOrEmpty(customer.SalesLocation) && customer.SalesLocation != "-1")
                {
                    newsaleslocationforlog = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyandDataValueFromLLookup("SalesLocation", customer.SalesLocation);
                }
                if (!string.IsNullOrEmpty(Cusmodel.SalesLocation) && Cusmodel.SalesLocation != "-1")
                {
                    prevsaleslocationforlog = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyandDataValueFromLLookup("SalesLocation", Cusmodel.SalesLocation);
                }
                if (customer.ReferringCustomer != new Guid())
                {
                    Customer refcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customer.ReferringCustomer);
                    refcusvalforlognew = refcus.FirstName + " " + refcus.LastName;
                }
                if (Cusmodel.ReferringCustomer != new Guid())
                {
                    Customer refcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Cusmodel.ReferringCustomer);
                    refcusvalforlogprev = refcus.FirstName + " " + refcus.LastName;
                }
                if (customer.ChildOf != new Guid())
                {
                    Customer chcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customer.ChildOf);
                    childcusvalforlognew = chcus.FirstName + " " + chcus.LastName;
                }
                if (Cusmodel.ChildOf != new Guid())
                {
                    Customer chcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Cusmodel.ChildOf);
                    childcusvalforlogprev = chcus.FirstName + " " + chcus.LastName;
                }
                if (!string.IsNullOrEmpty(customer.Soldby) && customer.Soldby != guidstring)
                {
                    Employee soldbyval = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(new Guid(customer.Soldby));
                    soldbyvalforlognew = soldbyval.FirstName + " " + soldbyval.LastName;
                }
                if (!string.IsNullOrEmpty(Cusmodel.Soldby) && customer.Soldby != guidstring && Cusmodel.Soldby != "00000000-0000-0000-0000-000000000000")
                {
                    Employee soldbyval = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(new Guid(Cusmodel.Soldby));
                    soldbyvalforlogprev = soldbyval.FirstName + " " + soldbyval.LastName;
                }
                if (customer.CustomerExtended != null && customer.CustomerExtended.AppoinmentSetBy != new Guid())
                {
                    Employee apptsetby = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(customer.CustomerExtended.AppoinmentSetBy);
                    apptsetbyforlognew = apptsetby.FirstName + " " + apptsetby.LastName;
                }
                if (CusExmodel != null && CusExmodel.AppoinmentSetBy != new Guid())
                {
                    Employee apptsetby = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(CusExmodel.AppoinmentSetBy);
                    apptsetbyforlogprev = apptsetby.FirstName + " " + apptsetby.LastName;
                }
                if (customer.CustomerExtended != null && customer.CustomerExtended.FinanceRep != new Guid())
                {
                    Employee financerep = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(customer.CustomerExtended.FinanceRep);
                    financerepforlognew = financerep.FirstName + " " + financerep.LastName;
                }
                if (CusExmodel != null && CusExmodel.FinanceRep != new Guid())
                {
                    Employee financerep = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(CusExmodel.FinanceRep);
                    financerepforlogprev = financerep.FirstName + " " + financerep.LastName;
                }
                if (Cusmodel.BranchId != null && Cusmodel.BranchId != -1)
                {
                    CompanyBranch companyBranch = _Util.Facade.CompanyBranchFacade.GetCompanyBranchById((int)Cusmodel.BranchId);
                    prevbranchnameforlog = companyBranch.Name;
                }
                if (customer.BranchId != null && customer.BranchId != -1)
                {
                    CompanyBranch companyBranch = _Util.Facade.CompanyBranchFacade.GetCompanyBranchById((int)customer.BranchId);
                    newbranchnameforlog = companyBranch.Name;
                }
                if (!string.IsNullOrEmpty(Cusmodel.CreditScore) && Cusmodel.CreditScore != "-1")
                {
                    CreditScoreGrade creditScoreGrade = _Util.Facade.CustomerFacade.GetCreditScoreGradeById(Int32.Parse(Cusmodel.CreditScore));
                    prevcreditgradeforlog = creditScoreGrade.Grade;
                }
                if (!string.IsNullOrEmpty(customer.CreditScore) && customer.CreditScore != "-1")
                {
                    CreditScoreGrade creditScoreGrade = _Util.Facade.CustomerFacade.GetCreditScoreGradeById(Int32.Parse(customer.CreditScore));
                    newcreditgradeforlog = creditScoreGrade.Grade;
                }
            }
            string message = "";

            if (customer.EmailAddress != Cusmodel.EmailAddress)
            {
                message += " Email Updated from " + Cusmodel.EmailAddress + " to " + customer.EmailAddress + "<br>";
            }
            if (customer.CustomerStatus != Cusmodel.CustomerStatus)
            {
                message += " Status Updated from " + Cusmodel.CustomerStatusVal + " to " + customerstatusvalforlog + "<br>";
            }
            if (customer.FirstName != Cusmodel.FirstName)
            {
                message += " FirstName Updated from " + Cusmodel.FirstName + " to " + customer.FirstName + "<br>";
            }
            if (customer.LastName != Cusmodel.LastName)
            {
                message += " LastName Updated from " + Cusmodel.LastName + " to " + customer.LastName + "<br>";
            }
            if (customer.BusinessName != Cusmodel.BusinessName)
            {
                message += " BusinessName Updated from " + Cusmodel.BusinessName + " to " + customer.BusinessName + "<br>";
            }
            if (customer.DBA != Cusmodel.DBA)
            {
                message += " DBA Updated from " + Cusmodel.DBA + " to " + customer.DBA + "<br>";
            }
            if (customer.Passcode != Cusmodel.Passcode)
            {
                message += " Verbal Password Updated from " + Cusmodel.Passcode + " to " + customer.Passcode + "<br>";
            }
            if (customer.LeadSource != Cusmodel.LeadSource)
            {
                message += " Lead Source Updated from " + Cusmodel.LeadSourceVal + " to " + leadsourcevalforlog + "<br>";
            }
            if (customer.ChildOf != Cusmodel.ChildOf)
            {
                message += " Child of Updated from " + childcusvalforlogprev + " to " + childcusvalforlognew + "<br>";
            }
            if (customer.ReferringCustomer != Cusmodel.ReferringCustomer)
            {
                message += " Reffering Customer Updated from " + refcusvalforlogprev + " to " + refcusvalforlognew + "<br>";
            }
            if (customer.Type != Cusmodel.Type)
            {
                if (customer.Type == "-1")
                {
                    customer.Type = "";
                }
                if (Cusmodel.Type == "-1")
                {
                    Cusmodel.Type = "";
                }
                message += " Type Updated from " + Cusmodel.Type + " to " + customer.Type + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.GrossFundedAmount != CusExmodel.GrossFundedAmount)
            {
                message += " Gross Funded Amount Updated from " + CusExmodel.GrossFundedAmount + "$ to " + customer.CustomerExtended.GrossFundedAmount + "$" + "<br>";
            }
            if (customer.CellNo != Cusmodel.CellNo)
            {
                message += " Contact Phone Updated from " + Cusmodel.CellNo + " to " + customer.CellNo + "<br>";
            }
            if (customer.PrimaryPhone != Cusmodel.PrimaryPhone)
            {
                message += " Land line Updated from " + Cusmodel.PrimaryPhone + " to " + customer.PrimaryPhone + "<br>";
            }
            if (customer.PreferredContactMethod != Cusmodel.PreferredContactMethod)
            {
                if (customer.PreferredContactMethod == "-1")
                {
                    customer.PreferredContactMethod = "";
                }
                if (Cusmodel.PreferredContactMethod == "-1")
                {
                    Cusmodel.PreferredContactMethod = "";
                }
                message += " Preferred Contact Method Updated from " + Cusmodel.PreferredContactMethod + " to " + customer.PreferredContactMethod + "<br>";
            }
            if (customer.SSN != Cusmodel.SSN)
            {
                message += " SSN Updated from " + Cusmodel.SSN + " to " + customer.SSN + "<br>";
            }
            if (customer.DateofBirth != Cusmodel.DateofBirth)
            {
                string prevdateofbirth = "";
                string nextdateofbirth = "";
                if (Cusmodel.DateofBirth != null && Cusmodel.DateofBirth != new DateTime())
                {
                    prevdateofbirth = Cusmodel.DateofBirth.Value.ToString("MM/dd/yyyy");
                }
                if (customer.DateofBirth != null && customer.DateofBirth != new DateTime())
                {
                    nextdateofbirth = customer.DateofBirth.Value.ToString("MM/dd/yyyy");
                }
                message += " Date of Birth Updated from " + prevdateofbirth + " to " + nextdateofbirth + "<br>";
            }
            if (customer.SecondCustomerNo != Cusmodel.SecondCustomerNo)
            {
                message += " Second Customer No Updated from " + Cusmodel.SecondCustomerNo + " to " + customer.SecondCustomerNo + "<br>";
            }
            if (customer.AdditionalCustomerNo != Cusmodel.AdditionalCustomerNo)
            {
                message += " Additional Ac No Updated from " + Cusmodel.AdditionalCustomerNo + " to " + customer.AdditionalCustomerNo + "<br>";
            }
            if (customer.CSProvider != Cusmodel.CSProvider)
            {
                if (customer.CSProvider == "-1")
                {
                    customer.CSProvider = "";
                }
                if (Cusmodel.CSProvider == "-1")
                {
                    Cusmodel.CSProvider = "";
                }
                message += " CS Provider Updated from " + Cusmodel.CSProvider + " to " + customer.CSProvider + "<br>";
            }
            if (customer.BranchId != Cusmodel.BranchId)
            {
                message += " Branch Updated from " + prevbranchnameforlog + " to " + newbranchnameforlog + "<br>";
            }
            if (customer.Ownership != Cusmodel.Ownership)
            {
                if (customer.Ownership == "-1")
                {
                    customer.Ownership = "";
                }
                if (Cusmodel.Ownership == "-1")
                {
                    Cusmodel.Ownership = "";
                }
                message += " Ownership Updated from " + Cusmodel.Ownership + " to " + customer.Ownership + "<br>";
            }
            if (customer.TaxExemption != Cusmodel.TaxExemption)
            {
                message += " Tax Exemption Updated from " + Cusmodel.TaxExemptionVal + " to " + customer.TaxExemption + "<br>";
            }
            if (customer.Soldby != Cusmodel.Soldby)
            {
                message += " Sales Person Updated from " + soldbyvalforlogprev + " to " + soldbyvalforlognew + "<br>";
            }
            if (customer.SalesLocation != Cusmodel.SalesLocation)
            {

                if (customer.SalesLocation == "-1")
                {
                    customer.SalesLocation = "";
                }
                if (Cusmodel.SalesLocation == "-1")
                {
                    Cusmodel.SalesLocation = "";
                }
                message += " Sales Location Updated from " + prevsaleslocationforlog + " to " + newsaleslocationforlog + "<br>";
            }
            if (customer.Status != Cusmodel.Status)
            {

                if (customer.Status == "-1")
                {
                    customer.Status = "";
                }
                if (Cusmodel.Status == "-1")
                {
                    Cusmodel.Status = "";
                }
                message += " Lead Status Updated from " + Cusmodel.Status + " to " + customer.Status + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.AppoinmentSetBy != CusExmodel.AppoinmentSetBy)
            {
                message += " Appoinment Set By Updated from " + apptsetbyforlogprev + " to " + apptsetbyforlognew + "<br>";
            }

            if (customer.FinancedAmount != Cusmodel.FinancedAmount)
            {
                message += " Financed Amount Updated from " + Cusmodel.FinancedAmount + "$ to " + customer.FinancedAmount + "$" + "<br>";
            }
            if (customer.LeadSourceType != Cusmodel.LeadSourceType)
            {
                if (customer.LeadSourceType == "-1")
                {
                    customer.LeadSourceType = "";
                }
                if (Cusmodel.LeadSourceType == "-1")
                {
                    Cusmodel.LeadSourceType = "";
                }
                message += " Lead Source Type Updated from " + Cusmodel.LeadSourceType + " to " + customer.LeadSourceType + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.FinanceCompany != CusExmodel.FinanceCompany)
            {
                if (customer.CustomerExtended.FinanceCompany == "-1")
                {
                    customer.CustomerExtended.FinanceCompany = "";
                }
                if (CusExmodel.FinanceCompany == "-1")
                {
                    CusExmodel.FinanceCompany = "";
                }
                message += " Finance Company Updated from " + CusExmodel.FinanceCompany + " to " + customer.CustomerExtended.FinanceCompany + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.IsFinanced != CusExmodel.IsFinanced)
            {
                message += " Is Financed Updated from " + CusExmodel.IsFinanced + " to " + customer.CustomerExtended.IsFinanced + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.CustomerPaymentAmount != CusExmodel.CustomerPaymentAmount)
            {
                message += " Customer Payment Amount Updated from " + CusExmodel.CustomerPaymentAmount + "$ to " + customer.CustomerExtended.CustomerPaymentAmount + "$" + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.FinanceRepCommissionRate != CusExmodel.FinanceRepCommissionRate)
            {
                message += " Finance Commission Rate Updated from " + CusExmodel.FinanceRepCommissionRate + " to " + customer.CustomerExtended.FinanceRepCommissionRate + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.LoanNumber != CusExmodel.LoanNumber)
            {
                message += " Loan Number Updated from " + CusExmodel.LoanNumber + " to " + customer.CustomerExtended.LoanNumber + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.Term != CusExmodel.Term)
            {
                if (customer.CustomerExtended.Term == "-1")
                {
                    customer.CustomerExtended.Term = "";
                }
                if (CusExmodel.Term == "-1")
                {
                    CusExmodel.Term = "";
                }
                message += " Term Updated from " + CusExmodel.Term + " to " + customer.CustomerExtended.Term + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.APR != CusExmodel.APR)
            {
                message += " APR Updated from " + CusExmodel.APR + " to " + customer.CustomerExtended.APR + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.MaxCreditLimit != CusExmodel.MaxCreditLimit)
            {
                message += " Max Credit Limit Updated from " + CusExmodel.MaxCreditLimit + " to " + customer.CustomerExtended.MaxCreditLimit + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.ApprovalDate != CusExmodel.ApprovalDate)
            {
                string prevApprovalDate = "";
                string nextApprovalDate = "";
                if (CusExmodel.ApprovalDate != null && CusExmodel.ApprovalDate != new DateTime())
                {
                    prevApprovalDate = CusExmodel.ApprovalDate.Value.ToString("MM/dd/yyyy");
                }
                if (customer.CustomerExtended.ApprovalDate != null && customer.CustomerExtended.ApprovalDate != new DateTime())
                {
                    nextApprovalDate = customer.CustomerExtended.ApprovalDate.Value.ToString("MM/dd/yyyy");
                }
                message += " Approval Date Updated from " + prevApprovalDate + " to " + nextApprovalDate + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.FinanceRep != CusExmodel.FinanceRep)
            {
                message += " Finance Rep Updated from " + financerepforlogprev + " to " + financerepforlognew + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.DiscountFundedAmount != CusExmodel.DiscountFundedAmount)
            {
                message += " Discount Funded Amount Updated from " + CusExmodel.DiscountFundedAmount + "$ to " + customer.CustomerExtended.DiscountFundedAmount + "$" + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.DiscountFundedPercentage != CusExmodel.DiscountFundedPercentage)
            {
                message += " Discount Funded Percentage Updated from " + CusExmodel.DiscountFundedPercentage + " to " + customer.CustomerExtended.DiscountFundedPercentage + "<br>";
            }
            //if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.GrossFundedAmount != CusExmodel.GrossFundedAmount)
            //{
            //    message += " Gross Funded Amount Updated from " + CusExmodel.GrossFundedAmount + " to " + customer.CustomerExtended.GrossFundedAmount + "<br>";
            //}
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.NetFundedAmount != CusExmodel.NetFundedAmount)
            {
                message += " Net Funded Amount Updated from " + CusExmodel.NetFundedAmount + "$ to " + customer.CustomerExtended.NetFundedAmount + "$" + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.Batch != CusExmodel.Batch)
            {
                message += " Batch Updated from " + CusExmodel.Batch + " to " + customer.CustomerExtended.Batch + "<br>";
            }
            if (customer.Note != Cusmodel.Note)
            {
                message += " Note Updated from " + Cusmodel.Note + " to " + customer.Note + "<br>";
            }
            if (customer.ZipCode != Cusmodel.ZipCode)
            {
                message += " ZipCode Updated from " + Cusmodel.ZipCode + " to " + customer.ZipCode + "<br>";
            }
            if (customer.City != Cusmodel.City)
            {
                message += " City Updated from " + Cusmodel.City + " to " + customer.City + "<br>";
            }
            if (customer.State != Cusmodel.State)
            {
                message += " State Updated from " + Cusmodel.State + " to " + customer.State + "<br>";
            }
            if (customer.Street != Cusmodel.Street)
            {
                message += " Street Updated from " + Cusmodel.Street + " to " + customer.Street + "<br>";
            }
            if (customer.County != Cusmodel.County)
            {
                message += " County Updated from " + Cusmodel.County + " to " + customer.County + "<br>";
            }
            if (customer.Appartment != Cusmodel.Appartment)
            {
                message += " Apt/Suit Updated from " + Cusmodel.Appartment + " to " + customer.Appartment + "<br>";
            }
            if (customer.CityPrevious != Cusmodel.CityPrevious)
            {
                message += " City(Billing Address) Updated from " + Cusmodel.CityPrevious + " to " + customer.CityPrevious + "<br>";
            }
            if (customer.StatePrevious != Cusmodel.StatePrevious)
            {
                message += " State(Billing Address) Updated from " + Cusmodel.StatePrevious + " to " + customer.StatePrevious + "<br>";
            }
            if (customer.StreetPrevious != Cusmodel.StreetPrevious)
            {
                message += " Street(Billing Address) Updated from " + Cusmodel.StreetPrevious + " to " + customer.StreetPrevious + "<br>";
            }
            if (customer.SalesDate != Cusmodel.SalesDate)
            {
                string prevSalesDate = "";
                string nextSalesDate = "";
                if (Cusmodel.SalesDate != null && Cusmodel.SalesDate != new DateTime())
                {
                    prevSalesDate = Cusmodel.SalesDate.Value.ToString("MM/dd/yyyy");
                }
                if (customer.SalesDate != null && customer.SalesDate != new DateTime())
                {
                    nextSalesDate = customer.SalesDate.Value.ToString("MM/dd/yyyy");
                }
                message += " Sales Date Updated from " + prevSalesDate + " to " + nextSalesDate + "<br>";
            }
            if (customer.InstallDate != Cusmodel.InstallDate)
            {
                string prevInstallDate = "";
                string nextInstallDate = "";
                if (Cusmodel.InstallDate != null && Cusmodel.InstallDate != new DateTime())
                {
                    prevInstallDate = Cusmodel.InstallDate.Value.ToString("MM/dd/yyyy");
                }
                if (customer.InstallDate != null && customer.InstallDate != new DateTime())
                {
                    nextInstallDate = customer.InstallDate.Value.ToString("MM/dd/yyyy");
                }
                message += " Install Date Updated from " + prevInstallDate + " to " + nextInstallDate + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.ContractStartDate != CusExmodel.ContractStartDate)
            {
                string prevContractStartDate = "";
                string nextContractStartDate = "";
                if (CusExmodel.ContractStartDate != null && CusExmodel.ContractStartDate != new DateTime())
                {
                    prevContractStartDate = CusExmodel.ContractStartDate.Value.ToString("MM/dd/yyyy");
                }
                if (customer.CustomerExtended.ContractStartDate != null && customer.CustomerExtended.ContractStartDate != new DateTime())
                {
                    nextContractStartDate = customer.CustomerExtended.ContractStartDate.Value.ToString("MM/dd/yyyy");
                }
                message += " Contract Start Date Updated from " + prevContractStartDate + " to " + nextContractStartDate + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.OriginalContactDate != Cusmodel.OriginalContactDate)
            {
                string prevOriginalContactDate = "";
                string nextOriginalContactDate = "";
                if (Cusmodel.OriginalContactDate != null && Cusmodel.OriginalContactDate != new DateTime())
                {
                    prevOriginalContactDate = Cusmodel.OriginalContactDate.Value.ToString("MM/dd/yyyy");
                }
                if (customer.OriginalContactDate != null && customer.OriginalContactDate != new DateTime())
                {
                    nextOriginalContactDate = customer.OriginalContactDate.Value.ToString("MM/dd/yyyy");
                }
                message += " Original Contact Date Updated from " + prevOriginalContactDate + " to " + nextOriginalContactDate + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.CustomerSince != CusExmodel.CustomerSince)
            {
                string prevCustomerSince = "";
                string nextCustomerSince = "";
                if (CusExmodel.CustomerSince != null && CusExmodel.CustomerSince != new DateTime())
                {
                    prevCustomerSince = CusExmodel.CustomerSince.Value.ToString("MM/dd/yyyy");
                }
                if (customer.CustomerExtended.CustomerSince != null && customer.CustomerExtended.CustomerSince != new DateTime())
                {
                    nextCustomerSince = customer.CustomerExtended.CustomerSince.Value.ToString("MM/dd/yyyy");
                }
                message += " Customer Since Updated from " + prevCustomerSince + " to " + nextCustomerSince + "<br>";
            }
            if (CusExmodel != null && customer.CustomerExtended != null && customer.CustomerExtended.ResignDate != CusExmodel.ResignDate)
            {
                string prevResignDate = "";
                string nextResignDate = "";
                if (CusExmodel.ResignDate != null && CusExmodel.ResignDate != new DateTime())
                {
                    prevResignDate = CusExmodel.ResignDate.Value.ToString("MM/dd/yyyy");
                }
                if (customer.CustomerExtended.ResignDate != null && customer.CustomerExtended.ResignDate != new DateTime())
                {
                    nextResignDate = customer.CustomerExtended.ResignDate.Value.ToString("MM/dd/yyyy");
                }
                message += " Resign Date Updated from " + prevResignDate + " to " + nextResignDate + "<br>";
            }
            if (customer.RenewalTerm != Cusmodel.RenewalTerm)
            {
                message += " Renewal Term Updated from " + Cusmodel.RenewalTerm + " to " + customer.RenewalTerm + "<br>";
            }
            //if (customer.FundingDate != Cusmodel.FundingDate)
            //{
            //    message += " Payment Date Updated from " + customer.FundingDate + " to " + Cusmodel.FundingDate + "<br>";
            //}
            if (customer.IsAgreement != Cusmodel.IsAgreement)
            {
                message += " Sign Agreement Updated from " + Cusmodel.IsAgreement + " to " + customer.IsAgreement + "<br>";
            }
            if (customer.IsFireAccount != Cusmodel.IsFireAccount)
            {
                message += " Fire Account Updated from " + Cusmodel.IsFireAccount + " to " + customer.IsFireAccount + "<br>";
            }
            if (customer.CreditScoreValue != Cusmodel.CreditScoreValue)
            {
                message += " Credit Score Updated from " + Cusmodel.CreditScoreValue + " to " + customer.CreditScoreValue + "<br>";
            }
            if (customer.CreditScore != Cusmodel.CreditScore)
            {
                message += " Credit Grade Updated from " + prevcreditgradeforlog + " to " + newcreditgradeforlog + "<br>";
            }
            if (customer.ContractTeam != Cusmodel.ContractTeam)
            {
                if (customer.ContractTeam == "-1")
                {
                    customer.ContractTeam = "";
                }
                if (Cusmodel.ContractTeam == "-1")
                {
                    Cusmodel.ContractTeam = "";
                }
                message += " Contract Term Updated from " + prevcontacttermvalforlog + " to " + newcontacttermvalforlog + "<br>";
            }
            //if (customer.CustomerFunded != Cusmodel.CustomerFunded)
            //{
            //    message += " Customer Funded Updated from " + Cusmodel.CustomerFunded + " to " + customer.CustomerFunded + "<br>";
            //}
            if (customer.BillAmount != Cusmodel.BillAmount)
            {
                message += " Bill Amount Updated from " + Cusmodel.BillAmount + "$ to " + customer.BillAmount + "$" + "<br>";
            }
            if (customer.BillCycle != Cusmodel.BillCycle)
            {
                if (customer.BillCycle == "-1")
                {
                    customer.BillCycle = "";
                }
                if (Cusmodel.BillCycle == "-1")
                {
                    Cusmodel.BillCycle = "";
                }
                message += " Bill Cycle Updated from " + Cusmodel.BillCycle + " to " + customer.BillCycle + "<br>";
            }
            if (customer.BillOutStanding != Cusmodel.BillOutStanding)
            {
                message += " OutStanding Balance Updated from " + Cusmodel.BillOutStanding + " to " + customer.BillOutStanding + "<br>";
            }
            if (customer.MonthlyMonitoringFee != Cusmodel.MonthlyMonitoringFee)
            {
                message += " Monitoring Fee Updated from " + Cusmodel.MonthlyMonitoringFee + "$ to " + customer.MonthlyMonitoringFee + "$" + "<br>";
            }
            if (customer.FirstBilling != Cusmodel.FirstBilling)
            {
                message += " Start Date Updated from " + Cusmodel.FirstBilling + " to " + customer.FirstBilling + "<br>";
            }
            if (customer.PaymentMethod != Cusmodel.PaymentMethod)
            {
                if (customer.PaymentMethod == "-1")
                {
                    customer.PaymentMethod = "";
                }
                if (Cusmodel.PaymentMethod == "-1")
                {
                    Cusmodel.PaymentMethod = "";
                }
                message += " Payment Profile Updated from " + Cusmodel.PaymentMethod + " to " + customer.PaymentMethod + "<br>";
            }
            if (customer.IsReceivePaymentMail != Cusmodel.IsReceivePaymentMail)
            {
                message += " Send Email Updated from " + Cusmodel.IsReceivePaymentMail + " to " + customer.IsReceivePaymentMail + "<br>";
            }
            if (customer.BillNotes != Cusmodel.BillNotes)
            {
                message += " Bill Notes Updated from " + Cusmodel.BillNotes + " to " + customer.BillNotes + "<br>";
            }
            string updatecusid = customer.Id.ToString();
            base.AddUserActivityForCustomer("Lead Information Updated #Ref: Lead#" + customer.Id + "</br>" + message, LabelHelper.ActivityAction.UpdateCustomer, customer.CustomerId, customer.Id, updatecusid);

            #endregion
        }
        [Authorize]
        public PartialViewResult DeleteLeads(int? id)
        {
            if (id.HasValue)
            {
                var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomersById(id.Value);
                CustomerCompany cc = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerIdAndCompanyId(tempCustomer.CustomerId, CurrentLoggedInUser.CompanyId.Value);
                if (cc == null)
                {
                    return null;
                }
                _Util.Facade.CustomerFacade.DeleteCustomerAndCustomerCompanyByIdAndCompanyId(cc.CustomerId, CurrentLoggedInUser.CompanyId.Value);
                //var iasd = _Util.Facade.CustomerFacade.DeleteCustomer(id.Value);

            }
            List<Customer> customers = _Util.Facade.CustomerFacade.GetAllCustomers();
            return PartialView("_LeadsList", customers);
        }

        [Authorize]
        [HttpPost]
        public JsonResult MarkAllNewLeadAsRead()
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Guid CompanyId = new Guid();
            var result = false;
            if (CurrentUser == null)
            {
                return Json(result);
            }
            if (!CurrentUser.CompanyId.HasValue)
            {
                return Json(result);
            }
            else
            {
                CompanyId = (CurrentUser.CompanyId.HasValue) ? CurrentUser.CompanyId.Value : CompanyId;
            }
            // List<Customer> CustomerList = _Util.Facade.CustomerFacade.GetAllCustomersByCompanyId(CompanyId).Where(x => x.Status == "New").ToList();

            List<Customer> CustomerList = _Util.Facade.CustomerFacade.GetAllLeadByCompanyIdByCustomerStatus(CompanyId).ToList();


            if (CustomerList != null && CustomerList.Count > 0)
            {
                foreach (var item in CustomerList)
                {
                    if (item.Status == "New")
                    {
                        item.Status = "";
                        _Util.Facade.CustomerFacade.UpdateCustomer(item);
                    }
                }
            }


            return Json(new { result = true });
        }



        [Authorize]
        public ActionResult LeadsDetails(int id, string Tablink, string noteid, string timeval, string IsComplete)
        {

            string LeadDetailCity = "";
            string LeadDetailState = "";
            string StreetType = "";
            string LAppartment = "";
            //if (HasNoPermission(CurrentPermission.Lead.ViewDetails))
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            Customer model = new Customer();
            if (id > 0)
            {

                List<SelectListItem> SalesPersons = new List<SelectListItem>();
                SalesPersons.Add(new SelectListItem()
                {
                    Text = "Sales Persons",
                    Value = "-1"
                });

                List<Employee> EmployeeDropDown = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentLoggedInUser.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid(), LabelHelper.UserTags.Partner).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).ToList();
                if (EmployeeDropDown != null && EmployeeDropDown.Count > 0)
                {
                    SalesPersons.AddRange(EmployeeDropDown.OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Select(x => new SelectListItem()
                    {
                        Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                        Value = x.UserId.ToString()
                    }).ToList());
                }

                ViewBag.SalesPersonList = SalesPersons.OrderBy(x => x.Text).ToList();

            }
            var SmartLeadGlobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "LeadSmartSetUp");
            if (SmartLeadGlobal != null && SmartLeadGlobal.Value.ToLower() == "true")
            {
                ViewBag.LeadPackageDetail = _Util.Facade.PackageFacade.GetAllSmartLeadPackageDetailByLeadIdandCompanyId(CurrentLoggedInUser.CompanyId.Value, id);
            }
            else
            {
                ViewBag.LeadPackageDetail = _Util.Facade.PackageFacade.GetAllLeadPackageDetailByLeadIdandCompanyId(CurrentLoggedInUser.CompanyId.Value, id);
            }
            ViewBag.LeadEmergencyDetail = _Util.Facade.EmergencyContactFacade.GetAllLeadEmergencyDetailByLeadIdandCompanyId(CurrentLoggedInUser.CompanyId.Value, id);

            ViewBag.LeadEquipmentDetail = _Util.Facade.CustomerAppoinmentFacade.GetAllLeadEquipmentDetailByLeadIdandCompanyId(CurrentLoggedInUser.CompanyId.Value, id);

            model = _Util.Facade.CustomerFacade.GetCustomersByIdAndSoldBy(id, CurrentLoggedInUser.UserId, CurrentLoggedInUser.UserTags, CurrentLoggedInUser.UserRole);
            if (model != null)
            {
                if (model.Status == "New")
                {
                    model.Status = "";
                    _Util.Facade.CustomerFacade.UpdateCustomer(model);

                }
                #region ReferringCustomer
                var objrefer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.ReferringCustomer);
                if (objrefer != null)
                {
                    string refname = "";
                    if (!string.IsNullOrWhiteSpace(objrefer.BusinessName))
                    {
                        refname = objrefer.BusinessName;
                    }
                    else
                    {
                        refname = objrefer.FirstName + " " + objrefer.LastName;
                    }
                    model.RefCustomer = refname;
                    ViewBag.RefId = objrefer.Id;
                }
                else
                {
                    ViewBag.RefId = "";
                }
                #endregion
                #region DuplicateCustomer
                if (model.DuplicateCustomer != Guid.Empty)
                {
                    Customer DuplicateCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.DuplicateCustomer);
                    if (DuplicateCustomer != null)
                    {
                        string refname = "";
                        if (!string.IsNullOrWhiteSpace(DuplicateCustomer.BusinessName))
                        {
                            refname = DuplicateCustomer.BusinessName;
                        }
                        else
                        {
                            refname = DuplicateCustomer.FirstName + " " + DuplicateCustomer.LastName;
                        }
                        model.DuplicateCustomerName = refname;
                        model.DuplicateCustomerId = DuplicateCustomer.Id;
                    }
                }

                #endregion
                #region Child Of
                var objchild = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.ChildOf);
                if (objchild != null)
                {
                    string refname = "";
                    if (!string.IsNullOrWhiteSpace(objchild.BusinessName))
                    {
                        refname = objchild.BusinessName;
                    }
                    else
                    {
                        refname = objchild.FirstName + " " + objchild.LastName;
                    }
                    model.chCustomer = refname;
                }
                #endregion
                //ViewBag.CustomerNoteList = _Util.Facade.CustomerFacade.GetAllCustomerNoteByCustomerCompany(CurrentLoggedInUser.CompanyId.Value);
                model.QAList = _Util.Facade.QaAnswerFacade.GetQa1QuestionaireByCompanyIdandCustomerId(CurrentLoggedInUser.CompanyId.Value, model.CustomerId);
                model.QAList1 = _Util.Facade.QaAnswerFacade.GetQa2QuestionaireByCompanyIdandCustomerId(CurrentLoggedInUser.CompanyId.Value, model.CustomerId);
                if (model.City != "")
                {
                    LeadDetailCity = model.City + ", ";
                }
                if (model.State != "")
                {
                    LeadDetailState = model.State + " ";
                }
                if (model.StreetType != "-1")
                {
                    StreetType = model.StreetType + " ";
                }
                if (!string.IsNullOrWhiteSpace(model.Appartment))
                {
                    LAppartment = "#" + model.Appartment;
                }

                ViewBag.LeadDetailAddress = AddressHelper.MakeAddress(model);
                ViewBag.LeadDetailAddress1 = LeadDetailCity + LeadDetailState + model.ZipCode;
                model.CustomerSpouse = _Util.Facade.CustomerFacade.GetSpouseByCustomerIdAndComapnyId(model.CustomerId, CurrentLoggedInUser.CompanyId.Value);
                model.LeadDetailTabCountModel = _Util.Facade.CustomerFacade.GetLeadDetailTabCount(CurrentLoggedInUser.CompanyId.Value, model.CustomerId);
                ViewBag.CustomerExistingItem = _Util.Facade.CustomerFacade.GetAllExistingItemByCustomerId(model.CustomerId);
                ViewBag.tab = Tablink;
                ViewBag.noteid = Convert.ToInt32(noteid);
                ViewBag.time = timeval;
                ViewBag.complete = IsComplete;
                ViewBag.creditreport = _Util.Facade.GlobalSettingsFacade.GetLeadCreditReportCheck(CurrentLoggedInUser.CompanyId.Value);
                ViewBag.CustomFormGeneration = _Util.Facade.GlobalSettingsFacade.GetCustomFormGeneration(CurrentLoggedInUser.CompanyId.Value);
                //if(model.LeadSource != null)
                //{
                //    model.SmartLeadSource = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(model.LeadSource);
                //}
                if (model.BranchId != null && model.BranchId != -1)
                {
                    CompanyBranch CB = _Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(CurrentLoggedInUser.CompanyId.Value).OrderBy(x => x.Id.ToString() != "-1").ThenBy(x => x.Name).Where(x => x.Id == model.BranchId).FirstOrDefault();
                    if (CB != null)
                    {
                        ViewBag.Branch = CB.Name;
                    }

                }
                if (!string.IsNullOrWhiteSpace(model.BusinessAccountType) && model.BusinessAccountType != "-1")
                {
                    Lookup Val = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyAndDataValue("BussinessAccountType", model.BusinessAccountType);
                    model.BusinessAccountTypeVal = Val.DisplayText;
                }
                Guid SoldBy = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(model.Soldby) && Guid.TryParse(model.Soldby, out SoldBy))
                {
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(SoldBy);
                    if (empobj != null)
                    {
                        if (!IsPermitted(UserPermissions.MyCompanyPermissions.ShowAllSalesPerson) && SoldBy != CurrentLoggedInUser.UserId)
                        {
                            model.PersonSales = "**********";
                        }
                        else
                        {
                            model.PersonSales = empobj.FirstName + " " + empobj.LastName;
                        }
                    }
                }

                model.CustomerSystemInfo = _Util.Facade.CustomerSystemInfoFacade.GetAllCustomerSystemInfoDetailsByCustomerId(model.CustomerId);
                if (model.CreatedByUid != new Guid())
                {
                    var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.CreatedByUid);
                    if (objemp != null)
                    {
                        model.CreatedByVal = objemp.FirstName + " " + objemp.LastName;
                    }
                }
                //if (model.BranchId > 0)
                //{
                //    model.CompanyBranch = _Util.Facade.CompanyBranchFacade.GetCompanyBranchById(model.BranchId.Value);
                //}
                ViewBag.CustomerUiSetting = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsByCompanyId(CurrentLoggedInUser.CompanyId.Value).Where(x => x.Tag == "CustomerUiSettings").ToList();
                ViewBag.CustomerSetting = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsByCompanyId(CurrentLoggedInUser.CompanyId.Value).Where(x => x.Tag == "CustomerSetting").ToList();
                List<GridSetting> LeadUiSetting = new List<GridSetting>();
                LeadUiSetting = _Util.Facade.GridSettingsFacade.GetAllByKey("LeadGrid", CurrentLoggedInUser.CompanyId.Value);
                if (LeadUiSetting.Count > 0)
                {
                    LeadUiSetting = LeadUiSetting.Where(x => x.FormActive == true).ToList();
                }
                ViewBag.LeadUiSetting = LeadUiSetting;
                ViewBag.LeadDetailBlock = _Util.Facade.GridSettingsFacade.GetAllByKey("LeadDetailBlock", CurrentLoggedInUser.CompanyId.Value);
                ViewBag.CustomerDetailTabList = _Util.Facade.GridSettingsFacade.GetAllByKey("CustomerDetailTab", CurrentLoggedInUser.CompanyId.Value);

                CustomerCompany cc = _Util.Facade.CustomerFacade.GetCustomerCompanyByCompanyIdAndCustomerId(CurrentLoggedInUser.CompanyId.Value, model.CustomerId);
                GlobalSetting LeadTrackingTab = _Util.Facade.GlobalSettingsFacade.GetLeadTrackingTabByCompanyId(CurrentLoggedInUser.CompanyId.Value);

                if (LeadTrackingTab != null && LeadTrackingTab.IsActive.Value)
                {
                    ViewBag.LeadTrackingTab = LeadTrackingTab.Value;
                }
                else
                {
                    ViewBag.LeadTrackingTab = false;
                }
                //ViewBag.DefaultLeadSiteType = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsByCompanyId(CurrentLoggedInUser.CompanyId.Value).Where(x => x.SearchKey == "DefaultLeadSiteType").FirstOrDefault().Value;
                //var objsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "DefaultLeadSiteType");
                //if (objsetting != null)
                //{
                //    ViewBag.LeadSiteType = Convert.ToBoolean(objsetting.Value);
                //}
                //else
                //{
                //    ViewBag.LeadSiteType = false;
                //}

                GlobalSetting CheckEmailAddress = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "CheckEmailWhenConvertingToCustomer");

                if (CheckEmailAddress != null && CheckEmailAddress.IsActive.Value)
                {
                    ViewBag.CheckEmailAddress = CheckEmailAddress.Value;
                }
                else
                {
                    ViewBag.CheckEmailAddress = false;
                }
                if (model.TransferCustomerId != null && model.TransferCustomerId.HasValue && model.TransferCustomerId.Value > 0)
                {
                    var objtransfercus = _Util.Facade.CustomerFacade.GetCustomerById(model.TransferCustomerId.Value);
                    if (objtransfercus != null)
                    {
                        model.TransferCustomerName = objtransfercus.FirstName + " " + objtransfercus.LastName;
                    }
                }
                var refcusobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.MoveCustomerId);
                if (refcusobj != null)
                {
                    model.MovedCustomerName = !string.IsNullOrWhiteSpace(refcusobj.DBA) ? refcusobj.DBA : !string.IsNullOrWhiteSpace(refcusobj.BusinessName) ? refcusobj.BusinessName : refcusobj.FirstName + " " + refcusobj.LastName;
                    model.MovedCustomerId = refcusobj.Id;
                }
                else
                {
                    model.MovedCustomerId = 0;
                }
                if (!string.IsNullOrEmpty(model.CustomerAccountType))
                {
                    var spacctype = model.CustomerAccountType.Replace(", ", ",").Split(',');
                    List<string> acctype = new List<string>();
                    if (spacctype.Length > 0)
                    {
                        foreach (var item in spacctype)
                        {
                            var lookupobj = _Util.Facade.LookupFacade.GetLookupByKeyAndValue("CustomerAccountType", item);
                            if (lookupobj != null)
                            {
                                acctype.Add(lookupobj.DisplayText);
                            }
                        }
                    }
                    model.CustomerAccountType = string.Join(", ", acctype);
                }


                #region Customer Extended Data
                if (model != null)
                {
                    CustomerExtended objextended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(model.CustomerId);
                    if (objextended != null)
                    {
                        model.CustomerExtended = new CustomerExtended();
                        model.CustomerExtended.ContractStartDate = objextended.ContractStartDate.HasValue ? objextended.ContractStartDate.Value : new DateTime();
                        model.CustomerExtended.RemainingContractTerm = objextended.RemainingContractTerm;
                        model.CustomerExtended.Pets = objextended.Pets;
                        model.CustomerExtended.PetsType = objextended.PetsType;
                        model.CustomerExtended.RepairType = objextended.RepairType;
                        model.CustomerExtended.Repair = objextended.Repair;
                        model.CustomerExtended.BirthDateCoupon = objextended.BirthDateCoupon;
                        model.CustomerExtended.VipClubMember = objextended.VipClubMember;

                        model.CustomerExtended.SecondarySSN = objextended.SecondarySSN;
                        model.CustomerExtended.SecondaryFirstName = objextended.SecondaryFirstName;
                        model.CustomerExtended.SecondaryLastName = objextended.SecondaryLastName;
                        model.CustomerExtended.SecondaryEmail = objextended.SecondaryEmail;
                        model.CustomerExtended.SecondaryBirthDate = objextended.SecondaryBirthDate;

                        model.CustomerExtended.MonthlyFinanceRate = objextended.MonthlyFinanceRate;
                        model.CustomerExtended.GrossFundedAmount = objextended.GrossFundedAmount;
                        model.CustomerExtended.NetFundedAmount = objextended.NetFundedAmount;
                        model.CustomerExtended.DiscountFundedAmount = objextended.DiscountFundedAmount;
                        model.CustomerExtended.DiscountFundedPercentage = objextended.DiscountFundedPercentage;
                        model.CustomerExtended.CustomerPaymentAmount = objextended.CustomerPaymentAmount;
                        model.CustomerExtended.FinanceRepCommissionRate = objextended.FinanceRepCommissionRate;
                        model.CustomerExtended.LoanNumber = objextended.LoanNumber;
                        model.CustomerExtended.CreditAppNumber = objextended.CreditAppNumber;
                        model.CustomerExtended.Term = objextended.Term;
                        model.CustomerExtended.APR = objextended.APR;
                        model.CustomerExtended.MaxCreditLimit = objextended.MaxCreditLimit;
                        model.CustomerExtended.ApprovalDate = objextended.ApprovalDate;
                        model.CustomerExtended.Batch = objextended.Batch;
                        model.CustomerExtended.MonthlyBatch = objextended.MonthlyBatch;
                        model.CustomerExtended.FinanceRep = objextended.FinanceRep;
                        model.CustomerExtended.AppoinmentSetBy = objextended.AppoinmentSetBy;
                        model.CustomerExtended.LeadVersion = objextended.LeadVersion;
                        model.CustomerExtended.IsFinanced = objextended.IsFinanced;
                        model.CustomerExtended.IsPcAppStatus = objextended.IsPcAppStatus;
                        model.CustomerExtended.IsPcApplicationId = objextended.IsPcApplicationId;
                        model.CustomerExtended.PowerPayAppStatus = objextended.PowerPayAppStatus;
                        model.CustomerExtended.PowerPayAppId = objextended.PowerPayAppId;
                        model.CustomerExtended.UnlinkCustomer = objextended.UnlinkCustomer;
                        model.CustomerExtended.DrivingLicense = objextended.DrivingLicense;
                        model.CustomerExtended.IsTestAccount = objextended.IsTestAccount;
                        model.CustomerExtended.DealerFee = objextended.DealerFee;
                        //Emergency fix needed, need to change
                        if (model.CustomerExtended.FinanceRep != Guid.Empty)
                        {
                            var RepEmployeeDetails = _Util.Facade.EmployeeFacade.GetEmployeeNameByEmployeeId(model.CustomerExtended.FinanceRep);
                            if (RepEmployeeDetails != null)
                            {
                                model.FinanceRepValue = RepEmployeeDetails.FirstName + " " + RepEmployeeDetails.LastName;
                            }
                        }
                        if (model.CustomerExtended.AppoinmentSetBy != Guid.Empty)
                        {
                            var AppSetEmployeeDetails = _Util.Facade.EmployeeFacade.GetEmployeeNameByEmployeeId(model.CustomerExtended.AppoinmentSetBy);
                            if (AppSetEmployeeDetails != null)
                            {
                                model.AppoinmentSetByValue = AppSetEmployeeDetails.FirstName + " " + AppSetEmployeeDetails.LastName;
                            }
                        }
                    }
                    else
                    {
                        objextended = new CustomerExtended();
                    }
                }
                #endregion

                #region Insert Customer View
                bool result = false;

                CustomerView cv = new CustomerView();
                cv.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                cv.CustomerId = model.CustomerId;
                cv.LastVistited = DateTime.Now.UTCCurrentTime();
                cv.LastVisitedBy = User.Identity.Name;
                cv.LastVisitedByUId = CurrentLoggedInUser.UserId;
                cv.IsLead = true;
                result = _Util.Facade.CustomerViewFacade.InsertViewList(cv) > 0;


                #endregion

                #region Customer Migration 
                model.CustomerMigration = _Util.Facade.CustomerFacade.GetCustomerMigrationByCustomerId(model.CustomerId);
                #endregion

                if (cc != null && cc.IsLead == true)
                {
                    ViewBag.isContactFormList = _Util.Facade.CustomerFacade.IsCustomerInCustomerContactByCustomerId(model.CustomerId);
                    if (model.CustomerExtended == null)
                    {
                        model.CustomerExtended = new CustomerExtended();
                    }

                    string GoogleMapAPIKey = _Util.Facade.GlobalSettingsFacade.GetGoogleMapAPIKeyByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                    ViewBag.GoogleMapAPIKey = GoogleMapAPIKey;

                    return PartialView("_LeadDetails", model);
                }
                else if (cc != null && cc.IsLead == false)
                {
                    ViewBag.Location = "/Customer/Customerdetail/?id=" + model.Id;
                    return View("~/Views/Shared/_RedirectPage.cshtml");
                }
                else
                {
                    return View("~/Views/Shared/_NotFound.cshtml");
                }
            }
            else
            {
                return View("_AccessDenied");

            }

        }
        public ActionResult HistoryLogPartial(Guid CustomerId, int PageNo, int PageSize)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            CustomerDetailTimestamp model = _Util.Facade.CustomerViewFacade.GetCustomerTimestampByCustomerId(CustomerId, PageNo, PageSize);

            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = null;
            if (ViewBag.order == null)
            {
                ViewBag.order = 0;
            }
            if (model.Count.TotalCustomer > 0)
            {
                ViewBag.OutOfNumber = model.Count.TotalCustomer;
            }

            if ((int)ViewBag.PageNumber * PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / PageSize);

            return View(model.CustomerDetailTimestampList);
        }
        [Authorize]
        public ActionResult CreditScoreList(Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            #region CustomerCreditCheckList
            List<CustomerCreditCheck> creditList = new List<CustomerCreditCheck>();
            creditList = _Util.Facade.CustomerFacade.GetAllCustomerCreditCheckByCustomerId(CustomerId);

            #endregion

            return View(creditList);
        }
        [Authorize]
        public PartialViewResult CorrespondenceList(Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<LeadCorrespondence> CorrespondenceList = _Util.Facade.LeadCorrespondenceFacade.GetAllMailCorrespondenceByCompanyIdAndCustomerId(CurrentUser.CompanyId.Value, CustomerId);

            var cusobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (cusobj != null)
            {
                ViewBag.LeadId = cusobj.Id;
            }

            return PartialView("_CorrespondenceList", CorrespondenceList);
        }

        [Authorize]
        public PartialViewResult MailToSalesPerson(int? id, int? Cid)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Customer model = new Customer();
            if (id.HasValue)
            {
                model = _Util.Facade.CustomerFacade.GetCustomerById(id.Value);
                List<SelectListItem> SalesPerson = new List<SelectListItem>();
                if (IsPermitted(UserPermissions.MyCompanyPermissions.ShowAllSalesPerson))
                {
                    var objcurrentlog = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                    SalesPerson.Add(new SelectListItem()
                    {
                        Text = "Please Select One",
                        Value = ""
                    });
                    if (objcurrentlog != null)
                    {
                        SalesPerson.Add(new SelectListItem()
                        {
                            Text = objcurrentlog.FirstName + " " + objcurrentlog.LastName,
                            Value = objcurrentlog.UserId.ToString()
                        });
                    }
                    List<Employee> EmployeeDropDown = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid(), LabelHelper.UserTags.Partner);

                    if (EmployeeDropDown != null && EmployeeDropDown.Count > 0)
                    {
                        SalesPerson.AddRange(EmployeeDropDown.OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                              Value = x.UserId.ToString()
                          }).ToList());
                    }

                    ViewBag.SalesPersonList = SalesPerson.OrderBy(x => x.Text).ToList();
                    if (string.IsNullOrWhiteSpace(model.Soldby))
                    {
                        model.Soldby = objcurrentlog.UserId.ToString();
                    }
                }
                else
                {
                    var objEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                    if (objEmp != null)
                    {
                        SalesPerson.Add(new SelectListItem()
                        {
                            Text = objEmp.FirstName.ToString() + " " + objEmp.LastName.ToString(),
                            Value = currentLoggedIn.UserId.ToString()
                        });
                        ViewBag.SalesPersonList = SalesPerson.OrderBy(x => x.Text).ToList();
                    }
                }
                if (Cid.HasValue)
                {
                    model.LeadCorrespondence = _Util.Facade.LeadCorrespondenceFacade.GetCorrespondenceById(Cid.Value);
                }
            }
            return PartialView("_MailToSalesPerson", model);
        }


        [Authorize]
        [HttpPost]
        public JsonResult MailToSalesperson(Guid customerId, string EmailAddress, string CustomerName, string SupplierId, String MailBody, string subject, string ccMail)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            EmailToSalesPersonFromLeads EmailToSalesPersonFromLeads = new EmailToSalesPersonFromLeads();
            bool result = false;
            bool emailres = false;
            DateTime DatetimeNow = DateTime.Now.UTCCurrentTime();
            DateTime DatetimeUtc = DateTime.Now.UTCCurrentTime();
            List<string> AssignName = new List<string>();
            string message = "";
            if (CurrentLoggedInUser == null)
            {
                return Json(false);
            }
            if (CurrentLoggedInUser != null)
            {
                if (IsValidEmail(EmailAddress))
                {
                    if (!string.IsNullOrWhiteSpace(ccMail))
                    {
                        AssignName = ccMail.Split(';').ToList();
                        if (AssignName.Count > 0)
                        {
                            foreach (var item in AssignName)
                            {
                                emailres = IsValidEmail(item);
                            }
                        }
                        if (emailres)
                        {
                            EmailTemplate emailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("EmailToSalesPersonFromLead");
                            if (emailTemplate != null)
                            {
                                emailTemplate.CcEmail = ccMail;
                                _Util.Facade.MailFacade.UpdateEmailTemplate(emailTemplate);
                            }
                        }
                        else
                        {
                            message = "CC field once or more email address not valid";
                        }
                    }
                    else
                    {
                        ccMail = null;
                        EmailTemplate emailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("EmailToSalesPersonFromLead");
                        if (emailTemplate != null)
                        {
                            _Util.Facade.MailFacade.UpdateEmailTemplate(emailTemplate);
                        }
                    }
                    string cusname = "";
                    var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerId);
                    if (objcus != null)
                    {
                        cusname = objcus.FirstName + " " + objcus.LastName;
                    }
                    EmailToSalesPersonFromLeads.MailPersonName = cusname;
                    EmailToSalesPersonFromLeads.SendMailPersonName = CurrentLoggedInUser.CompanyName;
                    EmailToSalesPersonFromLeads.EmailBody = MailBody;
                    EmailToSalesPersonFromLeads.ToEmail = EmailAddress;
                    EmailToSalesPersonFromLeads.Subject = string.IsNullOrWhiteSpace(subject) ? string.Concat("Message From - ", CurrentLoggedInUser.GetFullName()) : subject;
                    EmailToSalesPersonFromLeads.SentBy = CurrentLoggedInUser.GetFullName();
                    result = _Util.Facade.MailFacade.EmailToSalesPersonFromLeads(EmailToSalesPersonFromLeads, CurrentLoggedInUser.CompanyId.Value);
                    if (result == true)
                    {
                        message = "Email sent successfully";
                        CustomerSnapshot ObjCustomerSnapShot = new CustomerSnapshot
                        {
                            CustomerId = customerId,
                            CompanyId = CurrentLoggedInUser.CompanyId.Value,
                            Description = "Email To Sales Person",
                            Logdate = DateTime.Now.UTCCurrentTime(),
                            Updatedby = CurrentLoggedInUser.FirstName + " " + CurrentLoggedInUser.LastName
                        };
                        _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(ObjCustomerSnapShot);

                        LeadCorrespondence objCorrespondence = new LeadCorrespondence()
                        {
                            CompanyId = CurrentLoggedInUser.CompanyId.Value,
                            CustomerId = customerId,
                            Type = LabelHelper.CorrespondenceMessageTyp.Email,
                            TemplateKey = "EmailToSalesPersonFromLeads",
                            ToEmail = EmailAddress,
                            CcEmail = ccMail,
                            Subject = subject,
                            BodyContent = MailBody,
                            SentDate = DateTime.Now.UTCCurrentTime(),
                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            SentBy = CurrentLoggedInUser.UserId
                        };
                        _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objCorrespondence);
                    }
                    else
                    {
                        message = "Email sending failed.";
                    }
                }
                else
                {
                    message = "Invalid email address.";
                }
            }
            if (result)
            {
                base.AddUserActivityForCustomer("Mali is sent", LabelHelper.ActivityAction.MailSend, customerId, null, null);
            }
            return Json(new { result = result, message = LanguageHelper.T(message) });
        }

        [Authorize]
        [HttpPost]
        public JsonResult MailToLeadByCurrentLogInuser(Customer Customer, string CustomerName, string MailBody, string subject)
        {

            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (CurrentLoggedInUser == null)
            {
                return Json("Error Occured");
            }

            else
            {
                var LeadEmailAddress = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Customer.CustomerId);
                if (LeadEmailAddress.EmailAddress != null && LeadEmailAddress.EmailAddress != "")
                {
                    EmailToLeadFromCurrentLoggedinUser EmailToLeadFromCurrentLoggedinUser = new EmailToLeadFromCurrentLoggedinUser();
                    EmailToLeadFromCurrentLoggedinUser.Name = CustomerName;
                    EmailToLeadFromCurrentLoggedinUser.EmailBody = MailBody;
                    EmailToLeadFromCurrentLoggedinUser.Subject = subject;
                    EmailToLeadFromCurrentLoggedinUser.ToEmail = LeadEmailAddress.EmailAddress;
                    var result = _Util.Facade.MailFacade.EmailToLeadFromCurrentLoggedinUser(EmailToLeadFromCurrentLoggedinUser, CurrentLoggedInUser.CompanyId.Value);
                }
                else
                {
                    return Json("Error Occoured");
                }
            }
            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult MailConvertLeadToCustomer(Customer Customer, string CustomerName, string CustomerId, string CustomerMail, Guid gCompanyId)
        {

            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            EmailConvertLeadToCustomer EmailConvertLeadToCustomer = new EmailConvertLeadToCustomer();
            EmailConvertLeadToCustomer.CustomerName = CustomerName;
            EmailConvertLeadToCustomer.EmailBody = CustomerName;
            EmailConvertLeadToCustomer.ToEmail = CustomerMail;
            //EmailConvertLeadToCustomer.Logo = _Util.Facade.GlobalSettingsFacade.GetEmailLogoByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            //EmailConvertLeadToCustomer.Facebook = _Util.Facade.GlobalSettingsFacade.GetFacebookUrlByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            //EmailConvertLeadToCustomer.Youtube = _Util.Facade.GlobalSettingsFacade.GetYoutubeUrlByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            var Result = _Util.Facade.MailFacade.EmailConvertLeadToCustomer(EmailConvertLeadToCustomer, CurrentLoggedInUser.CompanyId.Value);

            return Json(Result);

        }

        [Authorize]
        public ActionResult FilterCustomerListPartial(CustomerFilter filter)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (currentLoggedIn.UserId != new Guid())
            {
                filter.EmployeeRole = currentLoggedIn.UserTags;
                filter.UserRole = currentLoggedIn.UserRole;
                filter.EmployeeId = currentLoggedIn.UserId.ToString();
            }
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                filter.SearchText = System.Web.HttpUtility.UrlDecode(filter.SearchText);
            }
            //GlobalSetting LeadSmartSetup = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "LeadSmartSetup");
            //ViewBag.LeadSmartSetup = LeadSmartSetup.Value.ToLower();
            CustomerListWithCountModel customerfilterlist = new CustomerListWithCountModel();
            List<GridSetting> LeadGridSettings = new List<GridSetting>();
            LeadGridSettings = _Util.Facade.GridSettingsFacade.GetByKey("LeadGrid", currentLoggedIn.CompanyId.Value);
            if (LeadGridSettings.Count > 0)
            {
                LeadGridSettings = LeadGridSettings.OrderBy(x => x.OrderBy).Where(x => x.GridActive == true).ToList();
            }
            ViewBag.LeadGridSettings = LeadGridSettings;
            List<GridSetting> LeadGroupGridSettings = new List<GridSetting>();
            LeadGroupGridSettings = _Util.Facade.GridSettingsFacade.GetByKey("LeadGridGroup", currentLoggedIn.CompanyId.Value);
            if (LeadGridSettings.Count > 0)
            {
                LeadGroupGridSettings = LeadGroupGridSettings.Where(x => x.GridActive == true).ToList();
            }
            ViewBag.LeadGroupGridSettings = LeadGroupGridSettings;
            filter.CompanyId = currentLoggedIn.CompanyId.Value;
            filter.isPermit = IsPermitted(UserPermissions.CustomerPermissions.ShowAllLeadList);
            if (filter.PageNo == 0)
            {
                filter.PageNo = 1;
            }

            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "CustomerListPageSize");
            if (glob != null)
            {
                filter.PageSize = Convert.ToInt32(glob.Value);
            }
            else
            {
                filter.PageSize = 10;
            }
            //if (!string.IsNullOrEmpty(filter.SearchText))
            //{
            //    string str = filter.SearchText;
            //    str = str.Replace(" ", "");
            //    filter.SearchText = str;
            //}



            customerfilterlist = _Util.Facade.CustomerFacade.GetLeadsByFilter(filter);

            Session["GetLeadIdFilter"] = filter;

            if (customerfilterlist.CustomerList.Count() == 0)
            {
                filter.PageNo = 1;
                //customerfilterlist = _Util.Facade.CustomerFacade.GetCustomerByFilter(filter);
            }

            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = 0;


            if (customerfilterlist.CustomerList.Count() > 0)
            {
                ViewBag.OutOfNumber = customerfilterlist.TotalCustomerCount.Counter;
            }

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }
            List<GridSetting> LocationGroup = LeadGridSettings.Where(x => x.ColumnGroup == "Location" && (x.SelectedColumn.ToLower() == "street" || x.SelectedColumn.ToLower() == "city" || x.SelectedColumn.ToLower() == "state" || x.SelectedColumn.ToLower() == "zipcode" || x.SelectedColumn.ToLower() == "streetprevious" || x.SelectedColumn.ToLower() == "cityprevious" || x.SelectedColumn.ToLower() == "stateprevious" || x.SelectedColumn.ToLower() == "zipcodeprevious" || x.SelectedColumn.ToLower() == "streettype" || x.SelectedColumn.ToLower() == "appartment")).ToList();
            ViewBag.LocationGroup = LocationGroup;
            // ViewBag.CustomerUiSetting = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettingsByCompanyId(currentLoggedIn.CompanyId.Value).Where(x => x.Tag == "CustomerUiSettings").ToList();
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);
            return PartialView("_FilterLeadListPartial", customerfilterlist);
        }
        public ActionResult LeadNotesPartial(Guid? customerid)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<CustomerNote> NoteCustomer = _Util.Facade.NotesFacade.GetLeadNotesByCustomerIdandCompanyId(customerid.Value, CurrentLoggedInUser.CompanyId.Value);
            foreach (var item in NoteCustomer)
            {
                var SalesPersonId = item.EmployeeID;
                var SalesPersonObj = _Util.Facade.EmployeeFacade.GetSalesPersonByEmployeeId(SalesPersonId);
                if (SalesPersonObj != null)
                {
                    item.AssignName = SalesPersonObj.FirstName + " " + SalesPersonObj.LastName;
                }
            }
            return PartialView("_LeadNotesPartial", NoteCustomer);
        }
        [Authorize]
        public ActionResult AddLeadNotes(int? id, string From, Guid? CustomerId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            CustomerNote model = new CustomerNote();
            if (id.HasValue)
            {
                model = _Util.Facade.NotesFacade.GetAllNotesByCustomerNoteId(id.Value);
                var AssignNoteObj = _Util.Facade.NoteAssignFacade.GetAssignedNotesByNoteId(model.Id);
                if (AssignNoteObj != null)
                {
                    model.EmployeeID = AssignNoteObj.EmployeeId;
                }
                model.AssignEmpList = _Util.Facade.NoteAssignFacade.GetAllAssignCustomerNoteListByNoteId(id.Value);
            }
            else
            {
                //model.CustomerId = CustomerId;
                model.IsEmail = false;
                model.IsText = false;
                //model = new CustomerNote();
            }

            //ViewBag.SalesPersonList = _Util.Facade.EmployeeFacade.GetAllSalesPersonEmployeeByCompnayId(CurrentLoggedInUser.CompanyId.Value).Select(x =>
            //              new SelectListItem()
            //              {
            //                  Text = x.FirstName + " " + x.LastName,
            //                  Value = x.UserId.ToString()
            //              }).ToList();

            ViewBag.From = "";
            if (!string.IsNullOrEmpty(From))
            {
                ViewBag.From = From;
            }

            //List<SelectListItem> SalesPersons = new List<SelectListItem>();
            //SalesPersons.Add(new SelectListItem()
            //{
            //    Text = "Sales Persons",
            //    Value = "-1"
            //});
            //List<Employee> EmployeeDropDown = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentLoggedInUser.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid());
            //if(EmployeeDropDown != null && EmployeeDropDown.Count > 0)
            //{
            //    SalesPersons.AddRange(EmployeeDropDown.OrderBy(x => x.FirstName).Select(x => new SelectListItem()
            //    {
            //        Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
            //        Value = x.UserId.ToString()
            //    }).ToList());
            //}

            List<SelectListItem> SalesPersonList = new List<SelectListItem>();
            if (IsPermitted(UserPermissions.MyCompanyPermissions.ShowAllSalesPerson))
            {
                var objcurrentlog = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
                //SalesPersonList.Add(new SelectListItem()
                //{
                //    Text = "Please Select One",
                //    Value = ""
                //});

                //SalesPersonList.AddRange(_Util.Facade.EmployeeFacade.GetALLEmployeeByCompanyIdAndIsRecruted(CurrentLoggedInUser.CompanyId.Value).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Select(x =>
                //      new SelectListItem()
                //      {
                //          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                //          Value = x.UserId.ToString()
                //      }).ToList());
                //ViewBag.SalesPersonList = SalesPersonList.OrderBy(x => x.Text).ToList();
                if (string.IsNullOrWhiteSpace(model.EmpId))
                {
                    model.EmpId = objcurrentlog.UserId.ToString();
                }
            }
            //else
            //{
            //    var objEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
            //    if (objEmp != null)
            //    {
            //        SalesPersonList.Add(new SelectListItem()
            //        {
            //            Text = objEmp.FirstName.ToString() + " " + objEmp.LastName.ToString(),
            //            Value = CurrentLoggedInUser.UserId.ToString()
            //        });
            //        ViewBag.SalesPersonList = SalesPersonList.OrderBy(x => x.Text).ToList();
            //    }
            //}
            List<Employee> empLsit = _Util.Facade.EmployeeFacade.GetCurrentEmployeeListByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            SalesPersonList.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            if (empLsit != null && empLsit.Count() > 0)
            {
                SalesPersonList.AddRange(empLsit.OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Select(x =>
                      new SelectListItem()
                      {
                          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                          Value = x.UserId.ToString()
                      }).ToList());
            }
            ViewBag.CustomerId = CustomerId;
            ViewBag.SalesPersonList = SalesPersonList.OrderBy(x => x.Value.ToString() != "-1").ThenBy(x => x.Text).ToList();
            //ViewBag.SalesPersonList = SalesPersons;
            ViewBag.NoteTypeList = _Util.Facade.LookupFacade.GetLookupByKey("NoteType").OrderBy(x => x.DataValue.ToString() != "-1").ThenBy(x => x.DisplayText).ToList();
            return PartialView("AddLeadNotes", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddLeadNotes(CustomerNote CustomerNote)
        {
            var result = false;
            CustomerNote customernote = new CustomerNote();
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            customernote.CompanyId = CurrentLoggedInUser.CompanyId.Value;
            customernote.CustomerId = CustomerNote.CustomerId;
            customernote.CreatedDate = DateTime.Now.UTCCurrentTime();
            customernote.ReminderDate = DateTime.Now.UTCCurrentTime();
            customernote.IsPin = CustomerNote.IsPin;
            customernote.IsEmail = CustomerNote.IsEmail;
            customernote.IsText = CustomerNote.IsText;
            customernote.IsOverview = CustomerNote.IsOverview;

            var currentuser = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
            if (CustomerNote.Id > 0)
            {
                string notemsg = "";

                if (CustomerNote.Notes != "")
                {

                    customernote = _Util.Facade.NotesFacade.GetById(CustomerNote.Id);
                    //customernote.Notes = CustomerNote.Notes;
                    // customernote.NoteType = CustomerNote.NoteType;
                    CustomerNote.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                    CustomerNote.ReminderDate = DateTime.Now.UTCCurrentTime();

                    CustomerNote.CreatedDate = customernote.CreatedDate;
                    CustomerNote.CreatedBy = customernote.CreatedBy;
                    CustomerNote.CreatedByUid = customernote.CreatedByUid;
                    if (currentuser != null)
                    {
                        customernote.CreatedBy = currentuser.FirstName + " " + currentuser.LastName;
                    }
                    //customernote.IsOverview = CustomerNote.IsOverview;
                    if (customernote.NoteType != CustomerNote.NoteType)
                    {
                        notemsg += "NoteType is updated from " + customernote.NoteType + " to " + CustomerNote.NoteType + "</br>";
                    }
                    if (customernote.Notes != CustomerNote.Notes)
                    {
                        notemsg += "Notes is updated from " + customernote.Notes + " to " + CustomerNote.Notes + "</br>";
                    }
                    if (customernote.IsEmail != CustomerNote.IsEmail)
                    {
                        notemsg += "Email Note is updated from " + customernote.IsEmail + " to " + CustomerNote.IsEmail + "</br>";
                    }
                    if (customernote.IsText != CustomerNote.IsText)
                    {
                        notemsg += "Text Note is updated from " + customernote.IsText + " to " + CustomerNote.IsText + "</br>";
                    }
                    if (customernote.IsPin != CustomerNote.IsPin)
                    {
                        notemsg += "Pin To Top is updated from " + customernote.IsPin + " to " + CustomerNote.IsPin + "</br>";
                    }
                    if (customernote.IsOverview != CustomerNote.IsOverview)
                    {
                        notemsg += "Show in Overview is updated from " + customernote.IsOverview + " to " + CustomerNote.IsOverview + "</br>";
                    }
                    result = _Util.Facade.NotesFacade.UpdateNotes(CustomerNote);
                    base.AddUserActivityForCustomer("Note is Updated #NoteId:" + customernote.Id + "</br>" + notemsg, LabelHelper.ActivityAction.UpdateNote, customernote.CustomerId, null, null);

                    _Util.Facade.NoteAssignFacade.DeleteAllAssignNoteByNoteId(customernote.Id);

                }

            }
            else
            {
                if (CustomerNote.Notes != "")
                {
                    customernote.Notes = CustomerNote.Notes;
                    customernote.NoteType = CustomerNote.NoteType;
                    customernote.IsShedule = false;
                    //if (currentuser != null)
                    //{
                    //    customernote.CreatedBy = currentuser.FirstName + " " + currentuser.LastName;
                    //}
                    customernote.CreatedBy = User.Identity.Name;
                    customernote.CreatedByUid = currentuser.UserId;
                    result = _Util.Facade.NotesFacade.InsertCustomerNote(customernote) > 0;
                    base.AddUserActivityForCustomer("New Note is Added #Id:" + customernote.Id + " </br> NoteType:" + customernote.NoteType + "</br> Notes:" + customernote.Notes, LabelHelper.ActivityAction.AddNote, customernote.CustomerId, null, null);

                    CustomerSnapshot objCustomerSnapShot = new CustomerSnapshot
                    {
                        CompanyId = customernote.CompanyId,
                        CustomerId = customernote.CustomerId,
                        Description = "LeadNote : " + customernote.Notes,
                        Logdate = DateTime.Now.UTCCurrentTime(),
                        Updatedby = CurrentLoggedInUser.FirstName + " " + CurrentLoggedInUser.LastName
                    };

                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objCustomerSnapShot);
                }

            }

            if (result == true)
            {
                if (CustomerNote.EmpIdList != null && CustomerNote.EmpIdList.Length > 0)
                {
                    foreach (var item1 in CustomerNote.EmpIdList)
                    {
                        NoteAssign objassign = new NoteAssign()
                        {
                            NoteId = customernote.Id,
                            EmployeeId = new Guid(item1)
                        };
                        _Util.Facade.NoteAssignFacade.InsertNoteAssign(objassign);

                        Customer objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customernote.CustomerId);
                        Employee _createdByName = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(customernote.CreatedByUid);
                        EmailToEmployeeFromFollowUpNote EmailToEmployeeFromFollowUpNote = new EmailToEmployeeFromFollowUpNote();
                        ReminderSms _remindersms = new ReminderSms();
                        string cusName = objcus.FirstName + " " + objcus.LastName;
                        _remindersms.CusId = objcus.Id;
                        _remindersms.CustomerName = cusName;
                        var NoteTypeVal = _Util.Facade.LookupFacade.GetLookupByKeyAndValueAndCompanyId("NoteType", customernote.NoteType, CurrentLoggedInUser.CompanyId.Value);
                        _remindersms.NoteType = NoteTypeVal != null && !string.IsNullOrWhiteSpace(NoteTypeVal.DataValue) && NoteTypeVal.DataValue != "-1" ? NoteTypeVal.DisplayText : "";
                        _remindersms.Message = customernote.Notes;
                        if (customernote.ReminderEndDate != null && customernote.ReminderEndDate != new DateTime())
                        {
                            _remindersms.AttnBy = customernote.ReminderEndDate.Value.UTCToClientTime().ToString("M/dd/yy");
                        }
                        _remindersms.CreatedBy = _createdByName != null ? _createdByName.FirstName + " " + _createdByName.LastName : "";
                        _remindersms.CreatedDate = customernote.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        _remindersms.CompanyName = CurrentLoggedInUser.CompanyName;
                        EmailToEmployeeFromFollowUpNote.CustomerName = cusName;
                        EmailToEmployeeFromFollowUpNote.EmailBody = customernote.NoteType + ":" + customernote.Notes;

                        if (objassign != null && objassign.EmployeeId != Guid.Empty)
                        {
                            Employee empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(objassign.EmployeeId);
                            Customer objcustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(objassign.EmployeeId);

                            if (empobj != null)
                            {
                                if (customernote.IsEmail == true && empobj.Email.IsValidEmailAddress())
                                {
                                    EmailToEmployeeFromFollowUpNote.ToEmail = empobj.Email;
                                    EmailToEmployeeFromFollowUpNote.AssignPersonName = empobj.FirstName + " " + empobj.LastName;
                                    if (IsPermitted(UserPermissions.CustomerPermissions.ShowReminderCompletedDateOnAddReminder) && customernote.ReminderEndDate != null && customernote.ReminderEndDate != new DateTime())
                                    {
                                        EmailToEmployeeFromFollowUpNote.AttnBy = customernote.ReminderEndDate.Value.UTCToClientTime().ToString("M/dd/yy");
                                    }
                                    EmailToEmployeeFromFollowUpNote.CreatedOn = customernote.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                                    if (objcus != null)
                                    {
                                        EmailToEmployeeFromFollowUpNote.CustomerIntId = objcus.Id;
                                    }
                                    if (_createdByName != null)
                                    {
                                        EmailToEmployeeFromFollowUpNote.CreatedByName = _createdByName.FirstName + " " + _createdByName.LastName;
                                    }
                                    _Util.Facade.MailFacade.EmailToEmployeeFromFollowUpNotes(EmailToEmployeeFromFollowUpNote, customernote.CompanyId);
                                }
                                if (CustomerNote.IsText == true && !string.IsNullOrWhiteSpace(empobj.Phone))
                                {
                                    List<string> receiverlist = new List<string>();
                                    receiverlist.Add(empobj.Phone);
                                    //Work start from here ---Create Sms template for note and task from lead
                                    _Util.Facade.SMSFacade.ReminderFollowupSMS(customernote.CompanyId, currentuser.UserId, _remindersms, receiverlist);
                                }
                            }
                            else if (objcustomer != null)
                            {
                                if (customernote.IsEmail == true && objcustomer.EmailAddress.IsValidEmailAddress())
                                {
                                    EmailToEmployeeFromFollowUpNote.ToEmail = objcustomer.EmailAddress;
                                    EmailToEmployeeFromFollowUpNote.AssignPersonName = objcustomer.FirstName + " " + objcustomer.LastName;
                                    if (IsPermitted(UserPermissions.CustomerPermissions.ShowReminderCompletedDateOnAddReminder) && customernote.ReminderEndDate != null && customernote.ReminderEndDate != new DateTime())
                                    {
                                        EmailToEmployeeFromFollowUpNote.AttnBy = customernote.ReminderEndDate.Value.UTCToClientTime().ToString("M/dd/yy");
                                    }
                                    EmailToEmployeeFromFollowUpNote.CreatedOn = customernote.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                                    if (objcus != null)
                                    {
                                        EmailToEmployeeFromFollowUpNote.CustomerIntId = objcus.Id;
                                    }
                                    if (_createdByName != null)
                                    {
                                        EmailToEmployeeFromFollowUpNote.CreatedByName = _createdByName.FirstName + " " + _createdByName.LastName;
                                    }
                                    _Util.Facade.MailFacade.EmailToEmployeeFromFollowUpNotes(EmailToEmployeeFromFollowUpNote, customernote.CompanyId);
                                }
                                if (customernote.IsText == true && !string.IsNullOrWhiteSpace(objcustomer.PrimaryPhone))
                                {
                                    List<string> receiverlist = new List<string>();
                                    receiverlist.Add(objcustomer.PrimaryPhone);
                                    _Util.Facade.SMSFacade.ReminderFollowupSMS(customernote.CompanyId, currentuser.UserId, _remindersms, receiverlist);
                                }
                            }
                        }
                    }
                }
            }
            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteLeadNote(int? id, Guid? CustomerId)
        {
            if (id.HasValue)
            {
                var dellead = _Util.Facade.NotesFacade.DeleteCustomerNote(id.Value);
                base.AddUserActivityForCustomer("Note is deleted #Id:" + id, LabelHelper.ActivityAction.Delete, CustomerId, null, null);

            }
            return Json(true);
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteLeadReminder(int? id, Guid? CustomerId)
        {
            if (id.HasValue)
            {
                var dellead = _Util.Facade.NotesFacade.DeleteCustomerNote(id.Value);
                base.AddUserActivityForCustomer("Reminder is deleted #Id:" + id, LabelHelper.ActivityAction.Delete, CustomerId, null, null);

            }
            return Json(true);
        }
        [Authorize]
        public ActionResult LoadLeadFollowUpTabPartial(Guid CustomerId, string pageno, string pagesize, DateTime? StartDate, DateTime? EndDate, string SearchText)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<CustomerNote> model = new List<CustomerNote>();

            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            else
            {
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
                if (!string.IsNullOrEmpty(SearchText))
                {
                    ViewBag.SearchText = SearchText;
                }

                Customer CustomerInfo = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                if (!string.IsNullOrWhiteSpace(pageno) && pageno != "undefined" && !string.IsNullOrWhiteSpace(pagesize) && pagesize != "undefined")
                {
                    model = _Util.Facade.NotesFacade.GetAllCustomerNotesByCustomerId(CustomerId, CurrentLoggedInUser.CompanyId.Value, Convert.ToInt32(pageno), Convert.ToInt32(pagesize), StartDate, EndDate, SearchText);
                }
                else
                {
                    pageno = "1";
                    model = _Util.Facade.NotesFacade.GetAllCustomerNotesByCustomerId(CustomerId, CurrentLoggedInUser.CompanyId.Value, Convert.ToInt32(pageno), Convert.ToInt32(pagesize), StartDate, EndDate, SearchText);
                }

                ViewBag.PageNumber = Convert.ToInt32(pageno);
                ViewBag.OutOfNumber = 0;
                int PageSize = Convert.ToInt32(pagesize);

                if (model.Count() > 0)
                {
                    foreach (var item in model)
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
                ViewBag.LeadFollowUpPartialCustomerId = CustomerId;
                return PartialView("_LeadFollwUpTabPartial", model);
            }
        }

        public ActionResult LeadNoteBoxes(Guid CustomerId, string SoldBy)
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            else if (CustomerId == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            else
            {
                ViewBag.SoldBy = SoldBy;
                List<CustomerNote> customerNote = _Util.Facade.NotesFacade.GetAllCustomerNoteByCustomerId(CustomerId, CurrentLoggedInUser.CompanyId.Value);
                return PartialView("_LeadNoteBoxes", customerNote);
            }
        }

        [Authorize]
        public ActionResult LeadSalesPersonBox(Guid SoldBy)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Employee emp = new Employee();
            if (SoldBy != null && SoldBy != new Guid())
            {
                emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(SoldBy);
                if (emp == null)
                {
                    emp = new Employee();
                }
            }
            return View(emp);
        }

        [Authorize]
        public ActionResult AddNewReminderNote(int? id, Guid CustomerId, string Timeval, string IsComplete)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            CustomerNote model = new CustomerNote();

            if (id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.NotesFacade.GetNotesById(id.Value);
                if (model.ReminderDate.HasValue)
                {
                    model.RemainderTime = model.ReminderDate.Value.UTCToClientTime().ToString("HH:mm");
                    model.ReminderDate = model.ReminderDate.Value.UTCToClientTime();
                }
                if (model.ReminderEndDate.HasValue)
                {
                    model.ReminderEndDate = model.ReminderEndDate.Value.UTCToClientTime();
                }
                model.CreatedDate = model.CreatedDate.UTCToClientTime();
                //model.EmpId = _Util.Facade.NoteAssignFacade.GetAssignEmployeeIdByNotesId(model.Id);
                model.AssignEmpList = _Util.Facade.NoteAssignFacade.GetAllAssignCustomerNoteListByNoteId(id.Value);
            }
            else
            {
                model.CustomerId = CustomerId;
                model.IsEmail = false;
                model.IsText = false;
                model.ReminderDate = DateTime.UtcNow.UTCToClientTime().AddDays(1);
                model.ReminderEndDate = DateTime.UtcNow.UTCToClientTime().AddDays(1);
                model.CreatedDate = DateTime.UtcNow.UTCToClientTime();
            }
            var mintime = _Util.Facade.GlobalSettingsFacade.GetReminderMinTime(CurrentLoggedInUser.CompanyId.Value);
            var maxtime = _Util.Facade.GlobalSettingsFacade.GetReminderMaxTime(CurrentLoggedInUser.CompanyId.Value);
            List<SelectListItem> RemainderTime = new List<SelectListItem>();
            RemainderTime.AddRange(_Util.Facade.LookupFacade.GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange("Arrival", mintime, maxtime).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.RemainderTime = RemainderTime;
            List<SelectListItem> SalesPersonList = new List<SelectListItem>();
            SalesPersonList.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = ""
            });
            if (IsPermitted(UserPermissions.MyCompanyPermissions.ShowAllSalesPerson))
            {
                var objcurrentlog = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
                //SalesPersonList.Add(new SelectListItem()
                //{
                //    Text = "Please Select One",
                //    Value = ""
                //});
                //if (CustomerId != new Guid())
                //{
                //    var cusobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                //    if (cusobj != null)
                //    {
                //        SalesPersonList.Add(new SelectListItem()
                //        {
                //            Text = cusobj.FirstName.ToString() + " " + cusobj.LastName.ToString(),
                //            Value = cusobj.CustomerId.ToString()
                //        });
                //    }
                //}
                //SalesPersonList.AddRange(_Util.Facade.EmployeeFacade.GetALLEmployeeByCompanyIdAndIsRecruted(CurrentLoggedInUser.CompanyId.Value).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Select(x =>
                //      new SelectListItem()
                //      {
                //          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                //          Value = x.UserId.ToString()
                //      }).ToList());
                //ViewBag.SalesPersonList = SalesPersonList.OrderBy(x => x.Text).ToList();
                if (string.IsNullOrWhiteSpace(model.EmpId))
                {
                    model.EmpId = objcurrentlog.UserId.ToString();
                }
            }
            //else
            //{
            //    var objEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
            //    if (objEmp != null)
            //    {
            //        SalesPersonList.Add(new SelectListItem()
            //        {
            //            Text = objEmp.FirstName.ToString() + " " + objEmp.LastName.ToString(),
            //            Value = CurrentLoggedInUser.UserId.ToString()
            //        });
            //        ViewBag.SalesPersonList = SalesPersonList.OrderBy(x => x.Text).ToList();
            //    }
            //}
            List<Employee> empLsit = _Util.Facade.EmployeeFacade.GetCurrentEmployeeListByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            if (empLsit != null && empLsit.Count() > 0)
            {
                SalesPersonList.AddRange(empLsit.OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Select(x =>
                      new SelectListItem()
                      {
                          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                          Value = x.UserId.ToString()
                      }).ToList());
            }
            List<TeamSetting> teamList = _Util.Facade.UserLoginFacade.GetAllTeam();
            if (teamList == null && teamList.Count() == 0)
            {
                teamList = new List<TeamSetting>();
            }
            ViewBag.TeamList = teamList;
            ViewBag.SalesPersonList = SalesPersonList.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
            if (IsComplete == "true")
            {
                ViewBag.complete = IsComplete;
            }
            return PartialView("_AddFollowUpReminder", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddNewReminderNote(CustomerNote CustomerNote)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            string EmailReminderCache = RMRCacheKey.EmailReminder;
            //string CreatedBy = "";
            string message1 = "";
            var UpdateResult = false;
            var result = false;
            CustomerNote CustomerNoteObj = new CustomerNote();
            var count = _Util.Facade.NotesFacade.GetCountCustomerNoteByCompanyId(CurrentLoggedInUser.CompanyId.Value, CustomerNote.ReminderDate.Value.ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss"), CustomerNote.ReminderDate.Value.AddHours(2).ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss"));
            if (CurrentLoggedInUser != null)
            {
                if (CustomerNote.Id > 0)
                {
                    string remindermsg = "";

                    //if (count.ReminderCount > 1)
                    //{
                    //    message1 = "This ticket schedule event would not be able to overlap";
                    //}
                    //else
                    //{

                    //}
                    CustomerNote Obj = _Util.Facade.NotesFacade.GetNotesById(CustomerNote.Id);

                    var newreminderdate = "";
                    if (CustomerNote != null && CustomerNote.reminderdatetimeforlog != null)
                    {
                        newreminderdate = CustomerNote.reminderdatetimeforlog.Value.ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                    }


                    string prevdate = Obj.ReminderDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime? prevdateforlog = Obj.ReminderDate.Value.AddHours(6);
                    if (CustomerNote.Notes != Obj.Notes)
                    {
                        remindermsg += "Reminder Note is updated from " + Obj.Notes + " to " + CustomerNote.Notes + "</br>";
                    }
                    if (newreminderdate != "" && newreminderdate != prevdate)
                    {
                        remindermsg += "Reminder Date and Time is updated from " + prevdateforlog + " to " + CustomerNote.reminderdatetimeforlog + "</br>";
                    }

                    if (CustomerNote.IsEmail != Obj.IsEmail)
                    {
                        remindermsg += "Email Reminder is updated from " + Obj.IsEmail + " to " + CustomerNote.IsEmail + "</br>";
                    }
                    if (CustomerNote.IsText != Obj.IsText)
                    {
                        remindermsg += "Text Reminder is updated from " + Obj.IsText + " to " + CustomerNote.IsText + "</br>";
                    }

                    Obj.Notes = CustomerNote.Notes;
                    if (!string.IsNullOrWhiteSpace(CustomerNote.RemainderTime) && CustomerNote.RemainderTime != "-1")
                    {
                        var redate = CustomerNote.ReminderDate.Value.Date.ToString("MM/dd/yyyy");
                        var Fredate = redate + " " + CustomerNote.RemainderTime;
                        CustomerNote.ReminderDate = Convert.ToDateTime(Fredate);
                        Obj.ReminderDate = CustomerNote.ReminderDate.Value.ClientToUTCTime();
                    }
                    else
                    {
                        Obj.ReminderDate = CustomerNote.ReminderDate.Value.ClientToUTCTime();
                    }
                    if (IsPermitted(UserPermissions.CustomerPermissions.ShowReminderCompletedDateOnAddReminder))
                    {
                        var redate = CustomerNote.ReminderEndDate.Value.Date.ToString("MM/dd/yyyy");
                        var Fredate = redate + " " + CustomerNote.RemainderTime;
                        CustomerNote.ReminderEndDate = Convert.ToDateTime(Fredate);
                        Obj.ReminderEndDate = CustomerNote.ReminderEndDate.Value.AddHours(2).ClientToUTCTime();
                    }
                    else
                    {
                        Obj.ReminderEndDate = CustomerNote.ReminderDate.Value.AddHours(2).ClientToUTCTime();
                    }
                    //Obj.ReminderEndDate = CustomerNote.ReminderDate.Value.AddHours(2).ClientToUTCTime();
                    Obj.IsEmail = CustomerNote.IsEmail;
                    Obj.TeamSettingId = CustomerNote.TeamSettingId;
                    Obj.IsText = CustomerNote.IsText;
                    //Obj.CreatedBy = User.Identity.Name; why this on update!?
                    Obj.IsClose = CustomerNote.IsClose;
                    if (CustomerNote.IsClose != null && CustomerNote.IsClose == true)
                    {
                        Obj.IsShedule = false;
                    }
                    UpdateResult = _Util.Facade.NotesFacade.UpdateNotes(Obj);
                    base.AddUserActivityForCustomer("Reminder is Updated #ReminderId:" + CustomerNote.Id + "</br>" + remindermsg, LabelHelper.ActivityAction.UpdateRemineder, CustomerNote.CustomerId, null, null);

                    if (UpdateResult == true)
                    {
                        if (CustomerNote.IsClose == false || CustomerNote.IsInstantNotification == true)
                        {
                            _Util.Facade.NoteAssignFacade.DeleteAllAssignNoteByNoteId(Obj.Id);
                            if (CustomerNote.EmpIdList != null && CustomerNote.EmpIdList.Length > 0)
                            {
                                foreach (var item1 in CustomerNote.EmpIdList)
                                {
                                    Guid EmpArrayId = new Guid(item1);
                                    NoteAssign objassign = new NoteAssign()
                                    {
                                        NoteId = Obj.Id,
                                        EmployeeId = EmpArrayId
                                    };
                                    _Util.Facade.NoteAssignFacade.InsertNoteAssign(objassign);
                                    if (CustomerNote.IsInstantNotification == true)
                                    {
                                        var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerNote.CustomerId);
                                        Employee _createdByName = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CustomerNoteObj.CreatedByUid);
                                        if (objcus != null)
                                        {
                                            CustomerNote.cusName = objcus.FirstName + ' ' + objcus.LastName;
                                        }
                                        string recname = "";
                                        var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmpArrayId);
                                        if (objemp != null)
                                        {
                                            CustomerNote.empName = objemp.Email;
                                            recname = objemp.FirstName + ' ' + objemp.LastName;
                                            CustomerNote.AssignName = recname;
                                        }
                                        if (EmpArrayId != null && CustomerNote.IsInstantNotification == true)
                                        {
                                            if (CustomerNote.IsEmail == true && objemp.Email.IsValidEmailAddress())
                                            {
                                                EmailToEmployeeFromFollowUpNote EmailToEmployeeFromFollowUpNote = new EmailToEmployeeFromFollowUpNote();
                                                EmailToEmployeeFromFollowUpNote.CustomerName = CustomerNote.cusName;
                                                EmailToEmployeeFromFollowUpNote.EmailBody = CustomerNote.Notes;
                                                EmailToEmployeeFromFollowUpNote.ToEmail = CustomerNote.empName;
                                                EmailToEmployeeFromFollowUpNote.AssignPersonName = CustomerNote.AssignName;
                                                if (IsPermitted(UserPermissions.CustomerPermissions.ShowReminderCompletedDateOnAddReminder) && CustomerNoteObj.ReminderEndDate != null && CustomerNoteObj.ReminderEndDate != new DateTime())
                                                {
                                                    EmailToEmployeeFromFollowUpNote.AttnBy = CustomerNoteObj.ReminderEndDate.Value.UTCToClientTime().ToString("M/dd/yy");
                                                }
                                                EmailToEmployeeFromFollowUpNote.CreatedOn = CustomerNoteObj.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                                                if (objcus != null)
                                                {
                                                    EmailToEmployeeFromFollowUpNote.CustomerIntId = objcus.Id;
                                                }
                                                if (_createdByName != null)
                                                {
                                                    EmailToEmployeeFromFollowUpNote.CreatedByName = _createdByName.FirstName + " " + _createdByName.LastName;
                                                }
                                                _Util.Facade.MailFacade.EmailToEmployeeFromFollowUpNotes(EmailToEmployeeFromFollowUpNote, CurrentLoggedInUser.CompanyId.Value);
                                            }
                                            if (CustomerNote.IsText == true && !string.IsNullOrWhiteSpace(objemp.Phone))
                                            {
                                                ReminderSms _remindersms = new ReminderSms();
                                                string cusName = objcus.FirstName + " " + objcus.LastName;
                                                _remindersms.CusId = objcus.Id;
                                                _remindersms.CustomerName = cusName;
                                                _remindersms.Message = CustomerNote.Notes;
                                                if (IsPermitted(UserPermissions.CustomerPermissions.ShowReminderCompletedDateOnAddReminder) && CustomerNoteObj.ReminderEndDate != null && CustomerNoteObj.ReminderEndDate != new DateTime())
                                                {
                                                    _remindersms.AttnBy = CustomerNoteObj.ReminderEndDate.Value.UTCToClientTime().ToString("M/dd/yy");
                                                }
                                                _remindersms.CreatedBy = _createdByName != null ? _createdByName.FirstName + " " + _createdByName.LastName : "";
                                                _remindersms.CreatedDate = CustomerNoteObj.CreatedDate.UTCToServerTime().ToString("M/dd/yy");
                                                _remindersms.CompanyName = CurrentLoggedInUser.CompanyName;
                                                List<string> receiverlist = new List<string>();
                                                receiverlist.Add(objemp.Phone);
                                                _Util.Facade.SMSFacade.ReminderFollowupSMS(CurrentLoggedInUser.CompanyId.Value, Guid.Empty, _remindersms, receiverlist);
                                            }
                                        }
                                    }
                                }
                            }
                            if ((List<CustomerNote>)System.Web.HttpRuntime.Cache[EmailReminderCache] != null)
                            {
                                List<CustomerNote> ListCacheNotes = new List<CustomerNote>();
                                string Constr = _Util.Facade.UserOrganizationFacade.GetConnectionStringByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                                var tempcache = _Util.Facade.NotesFacade.GetAllCustomerNoteByCompanyIdAndIsSchedule(CurrentLoggedInUser.CompanyId.Value, DateTime.Now.ToString(), Constr);
                                foreach (var itm in tempcache)
                                {
                                    itm.ConStr = Constr;
                                }
                                ListCacheNotes.AddRange(tempcache);
                                if (ListCacheNotes != null)
                                {
                                    System.Web.HttpRuntime.Cache.Remove(EmailReminderCache);
                                    System.Web.HttpRuntime.Cache.Insert(EmailReminderCache, ListCacheNotes);
                                }
                            }
                        }
                    }
                    return Json(new { UpdateResult = UpdateResult, message1 = message1 });
                }
                else
                {
                    //if (count.ReminderCount > 0)
                    //{
                    //    message1 = "This ticket schedule event would not be able to overlap";
                    //}
                    //else
                    //{

                    //}
                    CustomerNoteObj.Notes = CustomerNote.Notes;
                    if (!string.IsNullOrWhiteSpace(CustomerNote.RemainderTime) && CustomerNote.RemainderTime != "-1")
                    {
                        var redate = CustomerNote.ReminderDate.Value.Date.ToString("MM/dd/yyyy");
                        var Fredate = redate + " " + CustomerNote.RemainderTime;
                        CustomerNote.ReminderDate = Convert.ToDateTime(Fredate);
                        CustomerNoteObj.ReminderDate = CustomerNote.ReminderDate.Value.ClientToUTCTime();
                    }
                    else
                    {
                        CustomerNoteObj.ReminderDate = CustomerNote.ReminderDate.Value.ClientToUTCTime();
                    }
                    if (IsPermitted(UserPermissions.CustomerPermissions.ShowReminderCompletedDateOnAddReminder))
                    {
                        var redate = CustomerNote.ReminderEndDate.Value.Date.ToString("MM/dd/yyyy");
                        var Fredate = redate + " " + CustomerNote.RemainderTime;
                        CustomerNote.ReminderEndDate = Convert.ToDateTime(Fredate);
                        CustomerNoteObj.ReminderEndDate = CustomerNote.ReminderEndDate.Value.AddHours(2).ClientToUTCTime();
                    }
                    else
                    {
                        CustomerNoteObj.ReminderEndDate = CustomerNote.ReminderDate.Value.AddHours(2).ClientToUTCTime();
                    }
                    //CustomerNoteObj.ReminderEndDate = CustomerNote.ReminderDate.Value.AddHours(2).ClientToUTCTime();
                    CustomerNoteObj.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                    CustomerNoteObj.CustomerId = CustomerNote.CustomerId;
                    CustomerNoteObj.CreatedDate = DateTime.Now.UTCCurrentTime();
                    CustomerNoteObj.IsEmail = CustomerNote.IsEmail;
                    CustomerNoteObj.TeamSettingId = CustomerNote.TeamSettingId;
                    CustomerNoteObj.IsText = CustomerNote.IsText;
                    CustomerNoteObj.IsShedule = true;
                    CustomerNoteObj.CreatedBy = User.Identity.Name;
                    CustomerNoteObj.CreatedByUid = CurrentLoggedInUser.UserId;
                    result = _Util.Facade.NotesFacade.InsertCustomerNote(CustomerNoteObj) > 0;
                    base.AddUserActivityForCustomer("New Reminder is Added #Reminder Id:" + CustomerNoteObj.Id + " </br> Reminder Note:" + CustomerNote.Notes + "</br> Reminder Date and Time:" + CustomerNote.ReminderDate, LabelHelper.ActivityAction.AddRemineder, CustomerNote.CustomerId, null, null);

                    if (result == true)
                    {
                        if (CustomerNote.EmpIdList != null && CustomerNote.EmpIdList.Length > 0)
                        {
                            foreach (var item1 in CustomerNote.EmpIdList)
                            {
                                Guid EmpArrayId = new Guid(item1);
                                NoteAssign objassign = new NoteAssign()
                                {
                                    NoteId = CustomerNoteObj.Id,
                                    EmployeeId = EmpArrayId
                                };
                                _Util.Facade.NoteAssignFacade.InsertNoteAssign(objassign);
                                var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerNote.CustomerId);
                                Employee _createdByName = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CustomerNoteObj.CreatedByUid);
                                if (objcus != null)
                                {
                                    CustomerNote.cusName = objcus.FirstName + ' ' + objcus.LastName;
                                }
                                string recname = "";
                                var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmpArrayId);
                                if (objemp != null)
                                {
                                    CustomerNote.empName = objemp.Email;
                                    recname = objemp.FirstName + ' ' + objemp.LastName;
                                    CustomerNote.AssignName = recname;
                                }
                                if (EmpArrayId != null && CustomerNote.IsInstantNotification == true)
                                {
                                    if (CustomerNote.IsEmail == true && objemp.Email.IsValidEmailAddress())
                                    {
                                        EmailToEmployeeFromFollowUpNote EmailToEmployeeFromFollowUpNote = new EmailToEmployeeFromFollowUpNote();
                                        EmailToEmployeeFromFollowUpNote.CustomerName = CustomerNote.cusName;
                                        EmailToEmployeeFromFollowUpNote.EmailBody = CustomerNote.Notes;
                                        EmailToEmployeeFromFollowUpNote.ToEmail = CustomerNote.empName;
                                        EmailToEmployeeFromFollowUpNote.AssignPersonName = CustomerNote.AssignName;
                                        if (IsPermitted(UserPermissions.CustomerPermissions.ShowReminderCompletedDateOnAddReminder) && CustomerNoteObj.ReminderEndDate != null && CustomerNoteObj.ReminderEndDate != new DateTime())
                                        {
                                            EmailToEmployeeFromFollowUpNote.AttnBy = CustomerNoteObj.ReminderEndDate.Value.UTCToClientTime().ToString("M/dd/yy");
                                        }
                                        EmailToEmployeeFromFollowUpNote.CreatedOn = CustomerNoteObj.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                                        if (objcus != null)
                                        {
                                            EmailToEmployeeFromFollowUpNote.CustomerIntId = objcus.Id;
                                        }
                                        if (_createdByName != null)
                                        {
                                            EmailToEmployeeFromFollowUpNote.CreatedByName = _createdByName.FirstName + " " + _createdByName.LastName;
                                        }
                                        _Util.Facade.MailFacade.EmailToEmployeeFromFollowUpNotes(EmailToEmployeeFromFollowUpNote, CurrentLoggedInUser.CompanyId.Value);
                                    }
                                    if (CustomerNote.IsText == true && !string.IsNullOrWhiteSpace(objemp.Phone))
                                    {
                                        ReminderSms _remindersms = new ReminderSms();
                                        string cusName = objcus.FirstName + " " + objcus.LastName;
                                        _remindersms.CusId = objcus.Id;
                                        _remindersms.CustomerName = cusName;
                                        _remindersms.Message = CustomerNote.Notes;
                                        if (IsPermitted(UserPermissions.CustomerPermissions.ShowReminderCompletedDateOnAddReminder) && CustomerNoteObj.ReminderEndDate != null && CustomerNoteObj.ReminderEndDate != new DateTime())
                                        {
                                            _remindersms.AttnBy = CustomerNoteObj.ReminderEndDate.Value.UTCToClientTime().ToString("M/dd/yy");
                                        }
                                        _remindersms.CreatedBy = _createdByName != null ? _createdByName.FirstName + " " + _createdByName.LastName : "";
                                        _remindersms.CreatedDate = CustomerNoteObj.CreatedDate.UTCToServerTime().ToString("M/dd/yy");
                                        _remindersms.CompanyName = CurrentLoggedInUser.CompanyName;
                                        List<string> receiverlist = new List<string>();
                                        receiverlist.Add(objemp.Phone);
                                        _Util.Facade.SMSFacade.ReminderFollowupSMS(CurrentLoggedInUser.CompanyId.Value, Guid.Empty, _remindersms, receiverlist);
                                    }
                                }
                            }
                        }
                        System.Web.HttpRuntime.Cache.Remove(EmailReminderCache);
                    }

                    CustomerSnapshot ObjCustomerSnapShot = new CustomerSnapshot
                    {
                        CustomerId = CustomerNoteObj.CustomerId,
                        CompanyId = CustomerNoteObj.CompanyId,
                        Description = "LeadFollowUp : " + CustomerNoteObj.Notes,
                        Logdate = DateTime.Now.UTCCurrentTime(),
                        Updatedby = CurrentLoggedInUser.FirstName + " " + CurrentLoggedInUser.LastName
                    };
                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(ObjCustomerSnapShot);
                    return Json(new { result = result, message1 = message1 });
                }
            }

            else
            {
                return Json(false);
            }
        }

        [Authorize]
        public ActionResult LeadJoiningHistoryPartial(Guid? CustomerId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CustomerId == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            CustomerSnapshot model;
            model = _Util.Facade.CustomerSnapshotFacade.GetLeadJoinDateByCustomerIdCompanyIdandCreatedDateKey(CustomerId.Value, CurrentLoggedInUser.CompanyId.Value);
            return PartialView("_LeadJoiningHistoryPartial", model);
        }

        [Authorize]
        public ActionResult LeadNotesHistoryPartial(Guid? CustomerId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CustomerId == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<CustomerSnapshot> model;
            model = _Util.Facade.CustomerSnapshotFacade.GetLeadNoteByCustomerIdCompanyIdandLeadNoteKey(CustomerId.Value, CurrentLoggedInUser.CompanyId.Value);
            return PartialView("_LeadNotesHistoryPartial", model);
        }

        [Authorize]
        public ActionResult LeadFollowUpHistoryPartial(Guid? CustomerId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CustomerId == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<CustomerSnapshot> model;
            model = _Util.Facade.CustomerSnapshotFacade.GetLeadFollowUpsByCustomerIdCompanyIdandLeadFollowUpKey(CustomerId.Value, CurrentLoggedInUser.CompanyId.Value);
            return PartialView("_LeadFollowUpHistoryPartial", model);
        }

        [Authorize]
        public ActionResult LeadEmailHistoryPartial(Guid? CustomerId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CustomerId == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<CustomerSnapshot> model;
            model = _Util.Facade.CustomerSnapshotFacade.GetLeadMailHistoryByCustomerIdCompanyIdandEmailUpKey(CustomerId.Value, CurrentLoggedInUser.CompanyId.Value);
            return PartialView("_LeadEmailHistoryPartial", model);
        }

        [Authorize]
        public PartialViewResult LeadGridSettings()
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<GridSetting> LeadGridSettings = _Util.Facade.GridSettingsFacade.GetAllByKey("LeadGrid", CurrentLoggedInUser.CompanyId.Value);
            return PartialView("_LeadGridSettings", LeadGridSettings);
        }
        [Authorize]
        public JsonResult UpdateLeadGridSettings(List<GridSetting> LeadGridSettings)
        {
            var result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (LeadGridSettings.Count > 0)
            {
                foreach (var item in LeadGridSettings)
                {
                    item.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                    result = _Util.Facade.GridSettingsFacade.UpdateGridSettings(item);
                }
            }
            return Json(result);
        }
        public ActionResult LeadAddressMap(Guid? LeadId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!LeadId.HasValue)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            else if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            else
            {
                Customer model = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(LeadId.Value);
                if (model == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                else
                {
                    string GoogleMapAPIKey = _Util.Facade.GlobalSettingsFacade.GetGoogleMapAPIKeyByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                    ViewBag.GoogleMapAPIKey = GoogleMapAPIKey;

                    return PartialView("_LeadAddressMapPopUp", model);
                }

            }
        }

        public ActionResult LeadVerifyAddressMap(int? LeadId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            string GoogleMapAPIKey = _Util.Facade.GlobalSettingsFacade.GetGoogleMapAPIKeyByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            ViewBag.GoogleMapAPIKey = GoogleMapAPIKey;
            Customer model = new Customer();
            if (CurrentLoggedInUser != null)
            {
                if (LeadId.HasValue)
                {
                    var CustomerInfoObj = _Util.Facade.CustomerFacade.GetCustomersById(LeadId.Value);
                    if (CustomerInfoObj != null)
                    {
                        model = CustomerInfoObj;
                    }
                }

            }
            return PartialView("LeadVerifyAddressMap", model);
        }
        [Authorize]
        public ActionResult LeadEmailTextTemplatePartial(int? id)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            EmailTextTemplate model;
            if (id.HasValue)
            {
                model = _Util.Facade.EmailTextTemplateFacade.GetById(id.Value);
            }
            else
            {
                model = new EmailTextTemplate();
            }
            return PartialView("LeadEmailTextTemplatePartial", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult LeadEmailTextTemplatePartial(EmailTextTemplate et)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (et.Id > 0)
            {
                et.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                et.Type = TextTemplateType.Lead;
                _Util.Facade.EmailTextTemplateFacade.UpdateTextTemplate(et);
            }
            else
            {
                et.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                et.Type = TextTemplateType.Lead;
                _Util.Facade.EmailTextTemplateFacade.InsertTextTemplate(et);
            }
            return Json(true);
        }

        [Authorize]
        public ActionResult ShowEmailTextTemplateListPartial()
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<EmailTextTemplate> model = new List<EmailTextTemplate>();
            model = _Util.Facade.EmailTextTemplateFacade.GetAllLeadEmailTextTemplateByCompanyIdAndLeadKey(CurrentLoggedInUser.CompanyId.Value);
            return PartialView("_ShowEmailTextTemplateListPartial", model);
        }

        public ActionResult VerifyLeadPartial(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.LeadIdFromVerifyPartial = id.Value;
            }
            //else
            //{
            //    ViewBag.LeadIdFromVerifyPartial = 0;
            //}
            return PartialView("VerifyLeadPartial");
        }
        [Authorize]
        public ActionResult LeadVerificationPartial(int id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            string Lcity = "";
            string Lstate = "";
            string LStreetType = "";
            string LAppartment = "";
            Customer model;
            string IsCarrier = ConfigurationManager.AppSettings["IsCarrier"];
            ViewBag.Carrier = IsCarrier;


            List<GridSetting> LeadUiSetting = new List<GridSetting>();
            LeadUiSetting = _Util.Facade.GridSettingsFacade.GetAllByKey("LeadGrid", currentLoggedIn.CompanyId.Value);
            if (LeadUiSetting.Count > 0)
            {
                LeadUiSetting = LeadUiSetting.OrderBy(x => x.OrderBy).Where(x => x.FormActive == true).ToList();
            }
            ViewBag.LeadUiSetting = LeadUiSetting;

            if (id > 0)
            {
                model = _Util.Facade.CustomerFacade.GetCustomersById(id);
                if (model == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }

                if (model != null && model.Status == "New")
                {
                    model.Status = "";
                    _Util.Facade.CustomerFacade.UpdateCustomer(model);

                }

                model.CustomerExtended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(model.CustomerId);

                if (model.CustomerExtended != null && model.CustomerExtended.LeadVersion == "V2")
                {
                    return RedirectToAction("LeadInfoPartial", new { id = id });

                }
                if (model.CustomerExtended == null)
                {
                    model.CustomerExtended = new CustomerExtended();
                }

                //Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.ReferringCustomer);

                if (model.City != "")
                {
                    Lcity = model.City + ", ";
                }
                if (model.State != "")
                {
                    Lstate = model.State + " ";
                }
                if (model.StreetType != "-1")
                {
                    LStreetType = model.StreetType + " ";
                }
                if (!string.IsNullOrWhiteSpace(model.Appartment))
                {
                    LAppartment = "#" + model.Appartment;
                }
                #region CreditScore
                ViewBag.CreditGrade = "";
                if (model.CreditScoreValue != null && model.CreditScoreValue > 0)
                {
                    CreditScoreGrade creditscoreGrade = _Util.Facade.CustomerFacade.GetCreditScoreGradeByScoreRange(model.CreditScoreValue.Value);
                    if (creditscoreGrade != null)
                    {
                        ViewBag.CreditGrade = creditscoreGrade.ID;
                    }

                }

                #endregion
                #region HasDifferentCreditContact
                bool HasDiffrentCreditContact = _Util.Facade.AdditionalContactFacade.HasUsedSecondaryCreditCheck(model.CustomerId);
                ViewBag.HasDiffrentCreditContact = HasDiffrentCreditContact;
                #endregion
                ViewBag.AddressLead = model.Street + " " + LStreetType + LAppartment + " " + Lcity.UppercaseFirst() + Lstate + model.ZipCode;
                model.CustomerSystemInfo = _Util.Facade.CustomerSystemInfoFacade.GetCustomerSystemInfoByCustomerIdAndCompanyId(model.CustomerId, currentLoggedIn.CompanyId.Value);
                if (model.CustomerSystemInfo != null)
                {
                    ViewBag.Install = model.CustomerSystemInfo.InstallType;
                }


                if (model.TransferCustomerId != null && model.TransferCustomerId.HasValue && model.TransferCustomerId.Value > 0)
                {
                    var objtransfercus = _Util.Facade.CustomerFacade.GetCustomerById(model.TransferCustomerId.Value);
                    if (objtransfercus != null)
                    {
                        model.TransferCustomerName = objtransfercus.FirstName + " " + objtransfercus.LastName;
                    }
                }
                if (!string.IsNullOrEmpty(model.CustomerAccountType))
                {
                    var spaccounttype = model.CustomerAccountType.Replace(", ", ",").Split(',');
                    List<string> accounttypelist = new List<string>();
                    if (spaccounttype.Length > 0)
                    {
                        foreach (var item in spaccounttype)
                        {
                            accounttypelist.Add(item);
                        }
                    }
                    model.CustomerAccountTypeList = accounttypelist;
                }

            }
            else
            {
                model = new Customer();
                model.LeadSource = "-1";

            }

            var objsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "CustomerTypeRequired");
            if (objsetting != null)
            {
                ViewBag.CustomerTypeRequired = Convert.ToBoolean(objsetting.Value);
            }
            else
            {
                ViewBag.CustomerTypeRequired = false;
            }

            List<GridSetting> getleadsdropdown = _Util.Facade.GridSettingsFacade.GetAllLeadsFilterSettingByKeyAndCompanyIdAndIsActive(currentLoggedIn.CompanyId.Value);



            foreach (var item in getleadsdropdown)
            {
                string columnname = item.SelectedColumn.ToLower();

                switch (columnname)
                {

                    case "esistingpanel":
                        #region esistingpanel
                        ViewBag.PanelType = _Util.Facade.PanelTypeFacade.GetAllPanelType().Select(x =>
                         new SelectListItem()
                         {
                             Text = x.Name.ToString(),
                             Value = x.Id.ToString(),

                         }).ToList();
                        ViewBag.LeadInstallType = _Util.Facade.LookupFacade.GetDropdownsByKey("LeadInstallType");

                        break;
                    #endregion

                    case "streettype":
                        ViewBag.LeadStreetType = _Util.Facade.LookupFacade.GetDropdownsByKey("LeadStreetType");
                        break;

                    case "branchid":
                        #region branch ID
                        UserBranch ub = _Util.Facade.UserCompanyFacade.GetUserBranchByUserId(currentLoggedIn.UserId);
                        List<SelectListItem> BranchList = new List<SelectListItem>();
                        BranchList.Add(new SelectListItem()
                        {
                            Text = "Please Select",
                            Value = "-1"
                        });
                        if (ub != null && id == 0)
                        {
                            BranchList.AddRange(_Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(currentLoggedIn.CompanyId.Value).OrderBy(x => x.Id.ToString() != "-1").ThenBy(x => x.Name).Select(x => new SelectListItem()
                            {
                                Text = x.Name,
                                Value = x.Id.ToString(),
                                Selected = x.Id == ub.BranchId,
                            }).ToList());
                            ViewBag.BranchList = BranchList.ToList();
                        }
                        else
                        {
                            BranchList.AddRange(_Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(currentLoggedIn.CompanyId.Value).OrderBy(x => x.Id.ToString() != "-1").ThenBy(x => x.Name).Select(x => new SelectListItem()
                            {
                                Text = x.Name,
                                Value = x.Id.ToString(),
                                Selected = x.IsMainBranch == true,
                            }).ToList());
                            ViewBag.BranchList = BranchList.ToList();
                        }
                        break;
                    #endregion

                    case "besttimetocall":
                        ViewBag.BestTimeToCallList = _Util.Facade.LookupFacade.GetDropdownsByKey("BestTimeToCall");
                        break;

                    case "csprovider":
                        ViewBag.CSProviderList = _Util.Facade.LookupFacade.GetDropdownsByKey("CSProvider");
                        break;

                    case "preferredcontactmethod":
                        ViewBag.PreferredContactMethod = _Util.Facade.LookupFacade.GetDropdownsByKey("PreferredContactMethod");
                        break;

                    case "saleslocation":
                        #region saleslocation
                        List<SelectListItem> SalesCommisssion = new List<SelectListItem>();
                        SalesCommisssion.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("CommissionType").Select(x =>
                                    new SelectListItem()
                                    {
                                        Text = x.DisplayText.ToString(),
                                        Value = x.DataValue.ToString()
                                    }).ToList());
                        ViewBag.SalesCommisssion = SalesCommisssion.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
                        #endregion
                        break;

                    case "accessgivento":
                        #region accessgivento
                        List<SelectListItem> AccessAssignedPersons = new List<SelectListItem>();
                        AccessAssignedPersons.Add(new SelectListItem()
                        {
                            Text = "Access Assign To",
                            Value = "-1"
                        });
                        List<Employee> EmployeeDropDown = _Util.Facade.EmployeeFacade.GetAllEmployee(currentLoggedIn.CompanyId.Value);
                        if (EmployeeDropDown != null && EmployeeDropDown.Count > 0)
                        {
                            AccessAssignedPersons.AddRange(EmployeeDropDown.OrderBy(x => x.FirstName).Select(x => new SelectListItem()
                            {
                                Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                                Value = x.UserId.ToString()
                            }).ToList());
                        }

                        ViewBag.AccessAssignedPersons = AccessAssignedPersons;
                        #endregion
                        break;

                    case "phonetype":
                        ViewBag.PhoneTypeList = _Util.Facade.LookupFacade.GetDropdownsByKey("PhoneType");
                        break;

                    case "leadsourcetype":
                        ViewBag.LeadSourceTypeList = _Util.Facade.LookupFacade.GetDropdownsByKey("LeadSourceType");
                        break;

                    case "carrier":
                        ViewBag.LeadCarrierList = _Util.Facade.LookupFacade.GetDropdownsByKey("Carrier");
                        break;

                    case "soldby":
                        #region soldby
                        var objEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                        List<SelectListItem> SalesPerson = new List<SelectListItem>();
                        if (IsPermitted(UserPermissions.MyCompanyPermissions.ShowAllSalesPerson))
                        {
                            Employee objcurrentlog = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                            SalesPerson.Add(new SelectListItem()
                            {
                                Text = "Please Select One",
                                Value = ""
                            });
                            List<Employee> Employeelist = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Where(x => x.UserId != currentLoggedIn.UserId).ToList();
                            if (!string.IsNullOrEmpty(model.Soldby))
                            {
                                Guid SoldBy = Guid.Empty;
                                Guid.TryParse(model.Soldby, out SoldBy);
                                var SoldByEmployeeDetails = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(SoldBy);
                                if (SoldByEmployeeDetails != null && SoldByEmployeeDetails.IsActive == false)
                                {
                                    Employeelist.Add(SoldByEmployeeDetails);
                                    SalesPerson.Add(new SelectListItem()
                                    {
                                        Text = SoldByEmployeeDetails.FirstName + " " + SoldByEmployeeDetails.LastName,
                                        Value = SoldByEmployeeDetails.UserId.ToString(),
                                        Selected = true
                                    });
                                }
                            }
                            if (objcurrentlog != null)
                            {
                                Employeelist.Add(objcurrentlog);
                                SalesPerson.Add(new SelectListItem()
                                {
                                    Text = objcurrentlog.FirstName + " " + objcurrentlog.LastName,
                                    Value = objcurrentlog.UserId.ToString(),
                                    Selected = true
                                });
                            }

                            SalesPerson.AddRange(Employeelist.Select(x =>
                                      new SelectListItem()
                                      {
                                          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                                          Value = x.UserId.ToString()
                                      }).ToList());
                            if (model.CustomerExtended != null && model.CustomerExtended.AppoinmentSetBy != null && model.CustomerExtended.AppoinmentSetBy != new Guid("00000000-0000-0000-0000-000000000000"))
                            {
                                Employee _appoinmentSetbyEmployee = Employeelist.Where(x => x.UserId == model.CustomerExtended.AppoinmentSetBy).FirstOrDefault();
                                Employeelist.Remove(_appoinmentSetbyEmployee);
                            }
                            ViewBag.EmployeeList = Employeelist;

                            ViewBag.SalesPerson = SalesPerson.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
                        }
                        else
                        {
                            List<Employee> Employeelist = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Where(x => x.UserId == currentLoggedIn.UserId).ToList();
                            if (objEmp != null)
                            {
                                SalesPerson.Add(new SelectListItem()
                                {
                                    Text = objEmp.FirstName.ToString() + " " + objEmp.LastName.ToString(),
                                    Value = objEmp.UserId.ToString()
                                });
                                ViewBag.SalesPerson = SalesPerson.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
                            }
                            if (model.CustomerExtended != null && model.CustomerExtended.AppoinmentSetBy != null && model.CustomerExtended.AppoinmentSetBy != new Guid("00000000-0000-0000-0000-000000000000"))
                            {
                                Employee _appoinmentSetbyEmployee = Employeelist.Where(x => x.UserId == model.CustomerExtended.AppoinmentSetBy).FirstOrDefault();
                                Employeelist.Remove(_appoinmentSetbyEmployee);
                            }
                            if (!string.IsNullOrEmpty(model.Soldby))
                            {
                                Guid SoldBy = Guid.Empty;
                                Guid.TryParse(model.Soldby, out SoldBy);
                                var SoldByEmployeeDetails = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(SoldBy);
                                if (SoldByEmployeeDetails != null && SoldBy != currentLoggedIn.UserId)
                                {
                                    SoldByEmployeeDetails.FirstName = "**********";
                                    SoldByEmployeeDetails.LastName = "";
                                    Employeelist.Add(SoldByEmployeeDetails);
                                    SalesPerson.Add(new SelectListItem()
                                    {
                                        Text = "**********",
                                        Value = SoldByEmployeeDetails.UserId.ToString(),
                                        Selected = true
                                    });
                                }
                            }
                            ViewBag.EmployeeList = Employeelist;
                        }
                        if (string.IsNullOrWhiteSpace(model.Soldby) && model.SalesLocation == "" || model.SalesLocation == "-1" || model.SalesLocation == null)
                        {
                            model.SalesLocation = objEmp.SalesCommissionStructure;
                        }
                        if (!string.IsNullOrWhiteSpace(model.Soldby))
                        {
                            ViewBag.SelectSoldby = model.Soldby;
                        }
                        else
                        {
                            ViewBag.SelectSoldby = objEmp.UserId.ToString();
                        }
                        break;
                    #endregion

                    case "soldby2":
                        #region soldby2
                        var objEmp2 = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                        List<SelectListItem> SalesPerson2 = new List<SelectListItem>();

                        if (IsPermitted(UserPermissions.MyCompanyPermissions.ShowAllSalesPerson))
                        {
                            Employee objcurrentlog2 = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                            SalesPerson2.Add(new SelectListItem()
                            {
                                Text = "Please Select One",
                                Value = ""
                            });
                            List<Employee> Employeelist2 = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Where(x => x.UserId != currentLoggedIn.UserId).ToList();
                            if (model.SoldBy2 != Guid.Empty)
                            {
                                var SoldByEmployeeDetails2 = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.SoldBy2);
                                if (SoldByEmployeeDetails2 != null && SoldByEmployeeDetails2.IsActive == false)
                                {
                                    SalesPerson2.Add(new SelectListItem()
                                    {
                                        Text = SoldByEmployeeDetails2.FirstName + " " + SoldByEmployeeDetails2.LastName,
                                        Value = SoldByEmployeeDetails2.UserId.ToString(),
                                        Selected = true
                                    });
                                }
                            }
                            if (objcurrentlog2 != null)
                            {
                                SalesPerson2.Add(new SelectListItem()
                                {
                                    Text = objcurrentlog2.FirstName + " " + objcurrentlog2.LastName,
                                    Value = objcurrentlog2.UserId.ToString(),
                                    Selected = true
                                });
                            }

                            SalesPerson2.AddRange(Employeelist2.Select(x =>
                                      new SelectListItem()
                                      {
                                          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                                          Value = x.UserId.ToString()
                                      }).ToList());
                            ViewBag.SalesPerson2 = SalesPerson2.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
                        }
                        else
                        {
                            if (objEmp2 != null)
                            {
                                SalesPerson2.Add(new SelectListItem()
                                {
                                    Text = objEmp2.FirstName.ToString() + " " + objEmp2.LastName.ToString(),
                                    Value = objEmp2.UserId.ToString()
                                });
                            }
                            ViewBag.SalesPerson2 = SalesPerson2.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
                        }
                        break;
                    #endregion

                    case "soldby3":
                        #region soldby3
                        var objEmp3 = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                        List<SelectListItem> SalesPerson3 = new List<SelectListItem>();

                        if (IsPermitted(UserPermissions.MyCompanyPermissions.ShowAllSalesPerson))
                        {
                            Employee objcurrentlog3 = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                            SalesPerson3.Add(new SelectListItem()
                            {
                                Text = "Please Select One",
                                Value = ""
                            });
                            List<Employee> Employeelist3 = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Where(x => x.UserId != currentLoggedIn.UserId).ToList();
                            if (model.SoldBy3 != Guid.Empty)
                            {
                                var SoldByEmployeeDetails3 = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.SoldBy3);
                                if (SoldByEmployeeDetails3 != null && SoldByEmployeeDetails3.IsActive == false)
                                {
                                    SalesPerson3.Add(new SelectListItem()
                                    {
                                        Text = SoldByEmployeeDetails3.FirstName + " " + SoldByEmployeeDetails3.LastName,
                                        Value = SoldByEmployeeDetails3.UserId.ToString(),
                                        Selected = true
                                    });
                                }
                            }
                            if (objcurrentlog3 != null)
                            {
                                SalesPerson3.Add(new SelectListItem()
                                {
                                    Text = objcurrentlog3.FirstName + " " + objcurrentlog3.LastName,
                                    Value = objcurrentlog3.UserId.ToString(),
                                    Selected = true
                                });
                            }

                            SalesPerson3.AddRange(Employeelist3.Select(x =>
                                      new SelectListItem()
                                      {
                                          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                                          Value = x.UserId.ToString()
                                      }).ToList());
                            ViewBag.SalesPerson3 = SalesPerson3.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
                        }
                        else
                        {
                            if (objEmp3 != null)
                            {
                                SalesPerson3.Add(new SelectListItem()
                                {
                                    Text = objEmp3.FirstName.ToString() + " " + objEmp3.LastName.ToString(),
                                    Value = objEmp3.UserId.ToString()
                                });
                            }
                            ViewBag.SalesPerson3 = SalesPerson3.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
                        }
                        break;
                    #endregion

                    case "salesperson4":
                        #region soldby4
                        var objEmp4 = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                        List<SelectListItem> SalesPerson4 = new List<SelectListItem>();

                        if (IsPermitted(UserPermissions.MyCompanyPermissions.ShowAllSalesPerson))
                        {
                            Employee objcurrentlog4 = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                            SalesPerson4.Add(new SelectListItem()
                            {
                                Text = "Please Select One",
                                Value = ""
                            });
                            List<Employee> Employeelist4 = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Where(x => x.UserId != currentLoggedIn.UserId).ToList();
                            if (model.CustomerExtended.SalesPerson4 != Guid.Empty)
                            {
                                var SoldByEmployeeDetails4 = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(model.CustomerExtended.SalesPerson4);
                                if (SoldByEmployeeDetails4 != null && SoldByEmployeeDetails4.IsActive == false)
                                {
                                    SalesPerson4.Add(new SelectListItem()
                                    {
                                        Text = SoldByEmployeeDetails4.FirstName + " " + SoldByEmployeeDetails4.LastName,
                                        Value = SoldByEmployeeDetails4.UserId.ToString(),
                                        Selected = true
                                    });
                                }
                            }
                            if (objcurrentlog4 != null)
                            {
                                SalesPerson4.Add(new SelectListItem()
                                {
                                    Text = objcurrentlog4.FirstName + " " + objcurrentlog4.LastName,
                                    Value = objcurrentlog4.UserId.ToString(),
                                    Selected = true
                                });
                            }

                            SalesPerson4.AddRange(Employeelist4.Select(x =>
                                      new SelectListItem()
                                      {
                                          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                                          Value = x.UserId.ToString()
                                      }).ToList());
                            ViewBag.SalesPerson4 = SalesPerson4.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
                        }
                        else
                        {
                            if (objEmp4 != null)
                            {
                                SalesPerson4.Add(new SelectListItem()
                                {
                                    Text = objEmp4.FirstName.ToString() + " " + objEmp4.LastName.ToString(),
                                    Value = objEmp4.UserId.ToString()
                                });
                            }
                            ViewBag.SalesPerson4 = SalesPerson4.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
                        }
                        break;
                    #endregion

                    case "rwst01":
                        #region rwst01
                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                        break;
                    #endregion

                    case "rwst02":
                        #region rwst02
                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                        break;
                    #endregion

                    case "rwst03":
                        #region rwst03
                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                        break;
                    #endregion

                    case "rwst04":
                        #region rwst04
                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
                        break;
                    #endregion

                    case "rwst05":
                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                        break;

                    case "rwst06":
                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                        break;

                    case "rwst07":

                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                        break;


                    case "rwst08":

                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                        break;

                    case "rwst09":
                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                        break;

                    case "rwst10":
                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                        break;

                    case "rwst11":

                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                        break;

                    case "rwst12":

                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                        break;

                    case "rwst13":

                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                        break;

                    case "rwst14":

                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                        break;

                    case "rwst15":
                        #region rwst15
                        ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                        break;
                    #endregion
                    case "financerep":
                        #region financerep
                        //var objEmp3Fin = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                        List<Employee> EmployeelistFin = _Util.Facade.EmployeeFacade.GetAllEmployeeByCompanyId(currentLoggedIn.CompanyId.Value).ToList();

                        EmployeelistFin.Insert(0, new Employee()
                        {
                            FirstName = "Please Select",
                            LastName = "One",
                            UserId = Guid.Empty
                        });
                        ViewBag.EmployeeListFin = EmployeelistFin.OrderBy(x => x.FirstName + " " + x.LastName != "Please Select One").ThenBy(x => x.FirstName + " " + x.LastName).ToList();

                        if (model.CustomerExtended != null && model.CustomerExtended.FinanceRep != null && model.CustomerExtended.FinanceRep != new Guid("00000000-0000-0000-0000-000000000000"))
                        {
                            ViewBag.SelectSoldbyFin = model.CustomerExtended.FinanceRep;
                        }
                        else
                        {
                            //ViewBag.SelectSoldbyFin = objEmp3Fin.UserId;

                        }
                        if (model.CustomerExtended != null && model.CustomerExtended.AppoinmentSetBy != null && model.CustomerExtended.AppoinmentSetBy != new Guid("00000000-0000-0000-0000-000000000000"))
                        {
                            ViewBag.SelectAppoinmentBy = model.CustomerExtended.AppoinmentSetBy;
                        }
                        //else
                        //{
                        //    ViewBag.SelectSoldbyFin = objEmp3Fin.UserId;

                        //}
                        break;
                    #endregion 

                    case "isfinanced":
                        ViewBag.IsFinancedList = _Util.Facade.LookupFacade.GetDropdownsByKey("IsFinanced");
                        break;

                    case "financecompany":
                        #region financecompany
                        ViewBag.FinanceCompanyList = _Util.Facade.LookupFacade.GetLookupByKey("FinanceCompany").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
                        break;
                    #endregion

                    case "pets":
                        ViewBag.IsPets = _Util.Facade.LookupFacade.GetDropdownsByKey("Pets");
                        break;
                    case "petstype":
                        ViewBag.TypeOfPets = _Util.Facade.LookupFacade.GetDropdownsByKey("TypeOfPets");
                        break;

                    case "repair":
                        ViewBag.IsRepair = _Util.Facade.LookupFacade.GetDropdownsByKey("Repair");
                        break;

                    case "vipclubmember":
                        ViewBag.IsVipClubMember = _Util.Facade.LookupFacade.GetDropdownsByKey("VipClubMember");
                        break;

                    case "lead status":
                        ViewBag.LeadStatus = _Util.Facade.LookupFacade.GetDropdownsByKey("LeadStatus");
                        break;

                    case "leadsource":
                        #region leadsource
                        List<SelectListItem> leadSourceList = new List<SelectListItem>();
                        if (currentLoggedIn.UserRole == "Sales Rep - Outside")
                        {
                            leadSourceList.Add(new SelectListItem()
                            {
                                Text = "Outside Sales - Rep",
                                Value = "Outside Sales - Rep"
                            });


                        }
                        else
                        {


                            //List<Lookup> leadSourceLookup = _Util.Facade.LookupFacade.GetLookupByKeyWithParent("LeadSource");
                            leadSourceList.AddRange(_Util.Facade.LookupFacade.GetLookupByKeyWithParent("LeadSource").OrderBy(x => x.DataValue != "-1").ThenBy(x => x.DataOrder).Select(x => new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString(),
                                Selected = model.LeadSource == x.DataValue
                            }).ToList());
                            if (leadSourceList.Where(x => x.Selected).Count() == 0)
                            {
                                leadSourceList.Where(x => x.Value == "-1").FirstOrDefault(x => x.Selected = true);
                                model.LeadSource = "-1";
                            }
                        }
                        /*
                        leadSourceList = _Util.Facade.LookupFacade.GetDropdownsByKey("LeadSource");
                        if(model!=null && !string.IsNullOrWhiteSpace(model.LeadSource))
                        {
                            Lookup _lookup = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyAndDataValue("LeadSource", model.LeadSource);
                            if(_lookup!=null && _lookup.IsActive==false)
                            {
                                leadSourceList.Add(new SelectListItem()
                                {
                                    Text = _lookup.DisplayText.ToString(),
                                    Value = _lookup.DataValue.ToString(),
                                    Selected = true
                                });
                            }
                        }*/



                        ViewBag.LeadSource = leadSourceList;


                        break;
                    #endregion

                    case "installedstatus":
                        ViewBag.InstalledStatus = _Util.Facade.LookupFacade.GetDropdownsByKey("InstalledStatus");
                        break;

                    case "duplicatecustomer":
                        #region duplicatecustomer
                        List<SelectListItem> DuplicateCustomerList = new List<SelectListItem>();
                        if (model.DuplicateCustomer == Guid.Empty)
                        {
                            DuplicateCustomerList.Add(new SelectListItem
                            {
                                Text = LanguageHelper.T("Customer"),
                                Value = "00000000-0000-0000-0000-000000000000"
                            });
                        }
                        else
                        {
                            Customer DuplicateCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.DuplicateCustomer);
                            if (DuplicateCustomer != null)
                            {
                                DuplicateCustomerList.Add(new SelectListItem
                                {
                                    Text = string.IsNullOrWhiteSpace(DuplicateCustomer.BusinessName) ? (DuplicateCustomer.FirstName + " " + DuplicateCustomer.LastName) : DuplicateCustomer.BusinessName,
                                    Value = DuplicateCustomer.CustomerId.ToString()
                                });
                            }
                        }
                        ViewBag.DuplicateCustomerList = DuplicateCustomerList;

                        break;
                    #endregion

                    case "referringcustomer":
                        #region referringcustomer
                        List<SelectListItem> CustomerList = new List<SelectListItem>();
                        if (model.ReferringCustomer == new Guid())
                        {
                            CustomerList.Add(new SelectListItem
                            {
                                Text = LanguageHelper.T("Customer"),
                                Value = "00000000-0000-0000-0000-000000000000"
                            });
                        }
                        else
                        {
                            Customer RefCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.ReferringCustomer);
                            //Contact RefContact = _Util.Facade.ContactFacade.GetContactbyContactId(model.ReferringCustomer);
                            if (RefCustomer != null)
                            {
                                CustomerList.Add(new SelectListItem
                                {
                                    Text = string.IsNullOrWhiteSpace(RefCustomer.BusinessName) ? (RefCustomer.FirstName + " " + RefCustomer.LastName) : RefCustomer.BusinessName,
                                    Value = RefCustomer.CustomerId.ToString()
                                });
                            }
                        }
                        ViewBag.CustomerList = CustomerList;
                        break;
                    #endregion

                    case "market":
                        #region market
                        ViewBag.LeadMarket = _Util.Facade.LookupFacade.GetDropdownsByKey("LeadMarket");
                        break;
                    #endregion
                    case "type":
                        ViewBag.CustomerTypeList = _Util.Facade.LookupFacade.GetDropdownsByKey("CustomerType");
                        break;

                    case "creditscore":
                        #region creditscore
                        List<SelectListItem> creditGradeList = new List<SelectListItem>();
                        creditGradeList.Add(new SelectListItem()
                        {
                            Text = "Select One",
                            Value = "-1"
                        }); ;
                        creditGradeList.AddRange(_Util.Facade.CustomerFacade.GetAllCreditScoreGrade().Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.Grade.ToString() + " (" + x.MinScore + " - " + x.MaxScore + ")",
                                      Value = x.ID.ToString()
                                  }).ToList());
                        ViewBag.CreditScore = creditGradeList;
                        #endregion
                        break;

                    case "ownership":
                        ViewBag.OwnerShipList = _Util.Facade.LookupFacade.GetDropdownsByKey("OwnerShip");
                        break;

                    case "customeraccounttype":
                        ViewBag.CustomerAccountTypeLookup = _Util.Facade.LookupFacade.GetDropdownsByKey("CustomerAccountType");
                        break;

                    case "contactedperviously":
                        ViewBag.ContractedPrevious = _Util.Facade.LookupFacade.GetDropdownsByKey("ContractYesNo");
                        break;

                    case "tax exemption":
                        ViewBag.TaxExemptionList = _Util.Facade.LookupFacade.GetDropdownsByKey("TaxExemption");
                        break;

                    case "billtax":
                        ViewBag.BillYesNo = _Util.Facade.LookupFacade.GetDropdownsByKey("CustomerTaxYesNo");
                        break;

                    case "appoinment set":
                        ViewBag.AppoinmentSetList = _Util.Facade.LookupFacade.GetDropdownsByKey("AppoinmentSet");
                        break;

                    case "term":
                        ViewBag.TermList = _Util.Facade.LookupFacade.GetDropdownsByKey("TermList");
                        break;
                }
            }



            ViewBag.VerifyID = id;

            //Customer model;
            //string IsCarrier = ConfigurationManager.AppSettings["IsCarrier"];
            //ViewBag.Carrier = IsCarrier;
            //List<GridSetting> LeadUiSetting = new List<GridSetting>();
            //LeadUiSetting = _Util.Facade.GridSettingsFacade.GetAllByKey("LeadGrid", currentLoggedIn.CompanyId.Value);
            //if (LeadUiSetting.Count > 0)
            //{
            //    LeadUiSetting = LeadUiSetting.OrderBy(x => x.OrderBy).Where(x => x.FormActive == true).ToList();
            //}
            //ViewBag.LeadUiSetting = LeadUiSetting;

            //ViewBag.OwnerShipList = _Util.Facade.LookupFacade.GetDropdownsByKey("OwnerShip");
            //if (id > 0)
            //{
            //    model = _Util.Facade.CustomerFacade.GetCustomersById(id);
            //    if (model == null)
            //    {
            //        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //    }

            //    if (model != null && model.Status == "New")
            //    {
            //        model.Status = "";
            //        _Util.Facade.CustomerFacade.UpdateCustomer(model);

            //    }

            //    model.CustomerExtended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(model.CustomerId);
            //    if (model.CustomerExtended == null)
            //    {
            //        model.CustomerExtended = new CustomerExtended();
            //    }

            //    //Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.ReferringCustomer);

            //    if (model.City != "")
            //    {
            //        Lcity = model.City + ", ";
            //    }
            //    if (model.State != "")
            //    {
            //        Lstate = model.State + " ";
            //    }
            //    if (model.StreetType != "-1")
            //    {
            //        LStreetType = model.StreetType + " ";
            //    }
            //    if (!string.IsNullOrWhiteSpace(model.Appartment))
            //    {
            //        LAppartment = "#" + model.Appartment;
            //    }


            //    #region CreditScore
            //    ViewBag.CreditGrade = "";
            //    if (model.CreditScoreValue != null && model.CreditScoreValue > 0)
            //    {
            //        CreditScoreGrade creditscoreGrade = _Util.Facade.CustomerFacade.GetCreditScoreGradeByScoreRange(model.CreditScoreValue.Value);
            //        if (creditscoreGrade != null)
            //        {
            //            ViewBag.CreditGrade = creditscoreGrade.ID;
            //        }

            //    }

            //    #endregion

            //    ViewBag.AddressLead = model.Street + " " + LStreetType + LAppartment + " " + Lcity.UppercaseFirst() + Lstate + model.ZipCode;
            //    model.CustomerSystemInfo = _Util.Facade.CustomerSystemInfoFacade.GetCustomerSystemInfoByCustomerIdAndCompanyId(model.CustomerId, currentLoggedIn.CompanyId.Value);
            //    if (model.CustomerSystemInfo != null)
            //    {
            //        ViewBag.Install = model.CustomerSystemInfo.InstallType;
            //    }


            //    if (model.TransferCustomerId != null && model.TransferCustomerId.HasValue && model.TransferCustomerId.Value > 0)
            //    {
            //        var objtransfercus = _Util.Facade.CustomerFacade.GetCustomerById(model.TransferCustomerId.Value);
            //        if (objtransfercus != null)
            //        {
            //            model.TransferCustomerName = objtransfercus.FirstName + " " + objtransfercus.LastName;
            //        }
            //    }
            //    var spaccounttype = model.CustomerAccountType.Replace(", ", ",").Split(',');
            //    List<string> accounttypelist = new List<string>();
            //    if (spaccounttype.Length > 0)
            //    {
            //        foreach (var item in spaccounttype)
            //        {
            //            accounttypelist.Add(item);
            //        }
            //    }
            //    model.CustomerAccountTypeList = accounttypelist;
            //}
            //else
            //{
            //    model = new Customer();

            //}







            ViewBag.CityList = _Util.Facade.LookupFacade.GetDropdownsByKey("USACitiesList");

            ViewBag.StateList = _Util.Facade.LookupFacade.GetDropdownsByKey("StateList");
























            // ViewBag.TypeOfPets = _Util.Facade.LookupFacade.GetDropdownsByKey("TypeOfPets");

            //  ViewBag.IsRepair = _Util.Facade.LookupFacade.GetDropdownsByKey("Repair");

            ViewBag.TypeOfRepair = _Util.Facade.LookupFacade.GetDropdownsByKey("TypeOfRepair");

            //var objsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "CustomerTypeRequired");
            //if (objsetting != null)
            //{
            //    ViewBag.CustomerTypeRequired = Convert.ToBoolean(objsetting.Value);
            //}
            //else
            //{
            //    ViewBag.CustomerTypeRequired = false;
            //}

            //var objEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
            //List<SelectListItem> SalesPerson = new List<SelectListItem>();

            //if (IsPermitted(UserPermissions.MyCompanyPermissions.ShowAllSalesPerson))
            //{


            //    Employee objcurrentlog = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
            //    SalesPerson.Add(new SelectListItem()
            //    {
            //        Text = "Please Select One",
            //        Value = ""
            //    });
            //    List<Employee> Employeelist = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Where(x => x.UserId != currentLoggedIn.UserId).ToList();
            //    if (!string.IsNullOrEmpty(model.Soldby))
            //    {
            //        Guid SoldBy = Guid.Empty;
            //        Guid.TryParse(model.Soldby, out SoldBy);
            //        var SoldByEmployeeDetails = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(SoldBy);
            //        if (SoldByEmployeeDetails != null && SoldByEmployeeDetails.IsActive == false)
            //        {
            //            Employeelist.Add(SoldByEmployeeDetails);
            //            SalesPerson.Add(new SelectListItem()
            //            {
            //                Text = SoldByEmployeeDetails.FirstName + " " + objcurrentlog.LastName,
            //                Value = SoldByEmployeeDetails.UserId.ToString(),
            //                Selected = true
            //            });
            //        }
            //    }
            //    if (objcurrentlog != null)
            //    {
            //        Employeelist.Add(objcurrentlog);
            //        SalesPerson.Add(new SelectListItem()
            //        {
            //            Text = objcurrentlog.FirstName + " " + objcurrentlog.LastName,
            //            Value = objcurrentlog.UserId.ToString(),
            //            Selected = true
            //        });
            //    }

            //    SalesPerson.AddRange(Employeelist.Select(x =>
            //              new SelectListItem()
            //              {
            //                  Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
            //                  Value = x.UserId.ToString()
            //              }).ToList());

            //    ViewBag.EmployeeList = Employeelist;

            //    ViewBag.SalesPerson = SalesPerson.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
            //}
            //else
            //{
            //    List<Employee> Employeelist = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Where(x => x.UserId == currentLoggedIn.UserId).ToList();
            //    if (objEmp != null)
            //    {
            //        SalesPerson.Add(new SelectListItem()
            //        {
            //            Text = objEmp.FirstName.ToString() + " " + objEmp.LastName.ToString(),
            //            Value = objEmp.UserId.ToString()
            //        });
            //        ViewBag.SalesPerson = SalesPerson.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
            //    }
            //    ViewBag.EmployeeList = Employeelist;
            //}







            //if (string.IsNullOrWhiteSpace(model.Soldby) && model.SalesLocation == "" || model.SalesLocation == "-1" || model.SalesLocation == null)
            //{
            //    model.SalesLocation = objEmp.SalesCommissionStructure;
            //}
            //if (!string.IsNullOrWhiteSpace(model.Soldby))
            //{
            //    ViewBag.SelectSoldby = model.Soldby;
            //}
            //else
            //{
            //    ViewBag.SelectSoldby = objEmp.UserId.ToString();
            //}
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
            ViewBag.EmpSalesLocation = emp.SalesCommissionStructure;

            var objsettings = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "LeadSourceRequired");
            if (objsettings != null)
            {
                ViewBag.LeadSourceRequired = Convert.ToBoolean(objsettings.Value);
            }
            else
            {
                ViewBag.LeadSourceRequired = false;
            }
            var objsettingsBusinessName = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "BusinessNameRequiredSettingDFW");
            if (objsettingsBusinessName != null)
            {
                ViewBag.BusinessNameRequired = Convert.ToBoolean(objsettingsBusinessName.Value);
            }
            else
            {
                ViewBag.BusinessNameRequired = false;
            }
            #region Credit Check Info
            CustomerCreditCheck creditCheck = new CustomerCreditCheck();
            creditCheck = _Util.Facade.CustomerFacade.GetAllCustomerCreditCheckByCustomerId(model.CustomerId).FirstOrDefault();
            if (creditCheck != null)
            {
                model.CreditCheck = creditCheck;
                ViewBag.HasCreditCheck = "true";
            }
            else
            {
                model.CreditCheck = null;
                ViewBag.HasCreditCheck = "false";
            }
            var BrinksCreditCheck = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "BrinksCreditCheck");
            if (BrinksCreditCheck != null)
            {
                ViewBag.BrinksCreditCheck = BrinksCreditCheck.Value;
            }

            GlobalSetting GlobSet = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("PassingCreditScore", currentLoggedIn.CompanyId.Value);
            if (GlobSet != null)
            {
                ViewBag.CreditScoreValue = GlobSet.Value;
            }
            else
            {
                ViewBag.CreditScoreValue = 0;
            }
            #endregion
            var refcusobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.MoveCustomerId);
            if (refcusobj != null)
            {
                model.MovedCustomerName = !string.IsNullOrWhiteSpace(refcusobj.DBA) ? refcusobj.DBA : !string.IsNullOrWhiteSpace(refcusobj.BusinessName) ? refcusobj.BusinessName : refcusobj.FirstName + " " + refcusobj.LastName;
                model.MovedCustomerId = refcusobj.Id;
            }
            else
            {
                model.MovedCustomerId = 0;
            }

            ViewBag.BussinessAccountType = _Util.Facade.LookupFacade.GetDropdownsByKey("BussinessAccountType");


            return View("LeadVerificationPartial", model);
        }

        public ActionResult LeadInfoPartial(int id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            string Lcity = "";
            string Lstate = "";
            string LStreetType = "";
            string LAppartment = "";

            ViewBag.VerifyID = id;
            Customer model;
            string IsCarrier = ConfigurationManager.AppSettings["IsCarrier"];
            ViewBag.Carrier = IsCarrier;
            List<GridSetting> LeadUiSetting = new List<GridSetting>();
            LeadUiSetting = _Util.Facade.GridSettingsFacade.GetAllByKey("LeadGrid", currentLoggedIn.CompanyId.Value);
            if (LeadUiSetting.Count > 0)
            {
                LeadUiSetting = LeadUiSetting.OrderBy(x => x.OrderBy).Where(x => x.FormActive == true).ToList();
            }
            ViewBag.LeadUiSetting = LeadUiSetting;

            ViewBag.OwnerShipList = _Util.Facade.LookupFacade.GetDropdownsByKey("OwnerShip");
            if (id > 0)
            {
                model = _Util.Facade.CustomerFacade.GetCustomersById(id);
                if (model == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }

                if (model != null && model.Status == "New")
                {
                    model.Status = "";
                    _Util.Facade.CustomerFacade.UpdateCustomer(model);

                }

                model.CustomerExtended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(model.CustomerId);
                if (model.CustomerExtended == null)
                {
                    model.CustomerExtended = new CustomerExtended();
                }

                //Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.ReferringCustomer);

                if (model.City != "")
                {
                    Lcity = model.City + ", ";
                }
                if (model.State != "")
                {
                    Lstate = model.State + " ";
                }
                if (model.StreetType != "-1")
                {
                    LStreetType = model.StreetType + " ";
                }
                if (!string.IsNullOrWhiteSpace(model.Appartment))
                {
                    LAppartment = "#" + model.Appartment;
                }


                #region CreditScore
                ViewBag.CreditGrade = "";
                if (model.CreditScoreValue != null && model.CreditScoreValue > 0)
                {
                    CreditScoreGrade creditscoreGrade = _Util.Facade.CustomerFacade.GetCreditScoreGradeByScoreRange(model.CreditScoreValue.Value);
                    if (creditscoreGrade != null)
                    {
                        ViewBag.CreditGrade = creditscoreGrade.ID;
                    }

                }

                #endregion
                #region HasDifferentCreditContact
                bool HasDiffrentCreditContact = _Util.Facade.AdditionalContactFacade.HasUsedSecondaryCreditCheck(model.CustomerId);
                ViewBag.HasDiffrentCreditContact = HasDiffrentCreditContact;
                #endregion
                ViewBag.AddressLead = model.Street + " " + LStreetType + LAppartment + " " + Lcity.UppercaseFirst() + Lstate + model.ZipCode;
                model.CustomerSystemInfo = _Util.Facade.CustomerSystemInfoFacade.GetCustomerSystemInfoByCustomerIdAndCompanyId(model.CustomerId, currentLoggedIn.CompanyId.Value);
                if (model.CustomerSystemInfo != null)
                {
                    ViewBag.Install = model.CustomerSystemInfo.InstallType;
                }

                List<SelectListItem> AccessAssignedPersons = new List<SelectListItem>();
                AccessAssignedPersons.Add(new SelectListItem()
                {
                    Text = "Access Assign To",
                    Value = "-1"
                });
                List<Employee> EmployeeDropDown = _Util.Facade.EmployeeFacade.GetAllEmployee(currentLoggedIn.CompanyId.Value);
                if (EmployeeDropDown != null && EmployeeDropDown.Count > 0)
                {
                    AccessAssignedPersons.AddRange(EmployeeDropDown.OrderBy(x => x.FirstName).Select(x => new SelectListItem()
                    {
                        Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                        Value = x.UserId.ToString()
                    }).ToList());
                }

                ViewBag.AccessAssignedPersons = AccessAssignedPersons;
                if (model.TransferCustomerId != null && model.TransferCustomerId.HasValue && model.TransferCustomerId.Value > 0)
                {
                    var objtransfercus = _Util.Facade.CustomerFacade.GetCustomerById(model.TransferCustomerId.Value);
                    if (objtransfercus != null)
                    {
                        model.TransferCustomerName = objtransfercus.FirstName + " " + objtransfercus.LastName;
                    }
                }
                var spaccounttype = model.CustomerAccountType.Replace(", ", ",").Split(',');
                List<string> accounttypelist = new List<string>();
                if (spaccounttype.Length > 0)
                {
                    foreach (var item in spaccounttype)
                    {
                        accounttypelist.Add(item);
                    }
                }
                model.CustomerAccountTypeList = accounttypelist;
            }
            else
            {
                model = new Customer();

            }
            var BrinksCreditCheck = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "BrinksCreditCheck");
            if (BrinksCreditCheck != null)
            {
                ViewBag.BrinksCreditCheck = BrinksCreditCheck.Value;
            }

            #region Duplicate Customer
            List<SelectListItem> DuplicateCustomerList = new List<SelectListItem>();
            if (model.DuplicateCustomer == Guid.Empty)
            {
                DuplicateCustomerList.Add(new SelectListItem
                {
                    Text = LanguageHelper.T("Customer"),
                    Value = "00000000-0000-0000-0000-000000000000"
                });
            }
            else
            {
                Customer DuplicateCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.DuplicateCustomer);
                if (DuplicateCustomer != null)
                {
                    DuplicateCustomerList.Add(new SelectListItem
                    {
                        Text = string.IsNullOrWhiteSpace(DuplicateCustomer.BusinessName) ? (DuplicateCustomer.FirstName + " " + DuplicateCustomer.LastName) : DuplicateCustomer.BusinessName,
                        Value = DuplicateCustomer.CustomerId.ToString()
                    });
                }
            }
            ViewBag.DuplicateCustomerList = DuplicateCustomerList;
            #endregion

            #region ReferringCustomer
            List<SelectListItem> CustomerList = new List<SelectListItem>();
            if (model.ReferringCustomer == new Guid())
            {
                CustomerList.Add(new SelectListItem
                {
                    Text = LanguageHelper.T("Customer"),
                    Value = "00000000-0000-0000-0000-000000000000"
                });
            }
            else
            {
                Customer RefCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.ReferringCustomer);
                //Contact RefContact = _Util.Facade.ContactFacade.GetContactbyContactId(model.ReferringCustomer);
                if (RefCustomer != null)
                {
                    CustomerList.Add(new SelectListItem
                    {
                        Text = string.IsNullOrWhiteSpace(RefCustomer.BusinessName) ? (RefCustomer.FirstName + " " + RefCustomer.LastName) : RefCustomer.BusinessName,
                        Value = RefCustomer.CustomerId.ToString()
                    });
                }
            }
            ViewBag.CustomerList = CustomerList;
            #endregion

            ViewBag.PanelType = _Util.Facade.PanelTypeFacade.GetAllPanelType().Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Name.ToString(),
                        Value = x.Id.ToString(),

                    }).ToList();

            List<SelectListItem> creditGradeList = new List<SelectListItem>();
            creditGradeList.Add(new SelectListItem()
            {
                Text = "Select One",
                Value = "-1"
            }); ;

            creditGradeList.AddRange(_Util.Facade.CustomerFacade.GetAllCreditScoreGrade().Select(x =>
                      new SelectListItem()
                      {
                          Text = x.Grade.ToString() + " (" + x.MinScore + " - " + x.MaxScore + ")",
                          Value = x.ID.ToString()
                      }).ToList());
            ViewBag.CreditScore = creditGradeList;
            ViewBag.CityList = _Util.Facade.LookupFacade.GetDropdownsByKey("USACitiesList");

            ViewBag.StateList = _Util.Facade.LookupFacade.GetDropdownsByKey("StateList");

            ViewBag.CustomerTypeList = _Util.Facade.LookupFacade.GetDropdownsByKey("CustomerType");

            ViewBag.LeadStreetType = _Util.Facade.LookupFacade.GetDropdownsByKey("LeadStreetType");

            ViewBag.LeadInstallType = _Util.Facade.LookupFacade.GetDropdownsByKey("LeadInstallType");

            ViewBag.BestTimeToCallList = _Util.Facade.LookupFacade.GetDropdownsByKey("BestTimeToCall");

            ViewBag.TaxExemptionList = _Util.Facade.LookupFacade.GetDropdownsByKey("TaxExemption");

            ViewBag.BillYesNo = _Util.Facade.LookupFacade.GetDropdownsByKey("CustomerTaxYesNo");

            ViewBag.AppoinmentSetList = _Util.Facade.LookupFacade.GetDropdownsByKey("AppoinmentSet");

            ViewBag.PreferredContactMethod = _Util.Facade.LookupFacade.GetDropdownsByKey("PreferredContactMethod");

            ViewBag.LeadStatus = _Util.Facade.LookupFacade.GetDropdownsByKey("LeadStatus");

            ViewBag.LeadMarket = _Util.Facade.LookupFacade.GetDropdownsByKey("LeadMarket");


            if (currentLoggedIn.UserRole == "Sales Rep - Outside")
            {
                List<SelectListItem> leadSourceList = new List<SelectListItem>();
                leadSourceList.Add(new SelectListItem()
                {
                    Text = "Outside Sales - Rep",
                    Value = "Outside Sales - Rep"
                });

                ViewBag.LeadSource = leadSourceList;

            }
            else
            {
                ViewBag.LeadSource = _Util.Facade.LookupFacade.GetDropdownsByKey("LeadSource");

            }

            ViewBag.InstalledStatus = _Util.Facade.LookupFacade.GetDropdownsByKey("InstalledStatus");

            ViewBag.PhoneTypeList = _Util.Facade.LookupFacade.GetDropdownsByKey("PhoneType");

            ViewBag.LeadSourceTypeList = _Util.Facade.LookupFacade.GetDropdownsByKey("LeadSourceType");

            ViewBag.LeadCarrierList = _Util.Facade.LookupFacade.GetDropdownsByKey("Carrier");

            ViewBag.CustomerAccountTypeLookup = _Util.Facade.LookupFacade.GetDropdownsByKey("CustomerAccountType");

            ViewBag.ContractedPrevious = _Util.Facade.LookupFacade.GetDropdownsByKey("ContractYesNo");

            ViewBag.CSProviderList = _Util.Facade.LookupFacade.GetDropdownsByKey("CSProvider");

            ViewBag.IsFinancedList = _Util.Facade.LookupFacade.GetDropdownsByKey("IsFinanced");

            ViewBag.FinanceCompanyList = _Util.Facade.LookupFacade.GetLookupByKey("FinanceCompany").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                                      new SelectListItem()
                                      {
                                          Text = x.DisplayText.ToString(),
                                          Value = x.DataValue.ToString()
                                      }).ToList();
            ViewBag.RWSTList = _Util.Facade.LookupFacade.GetLookupByKey("RWST").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();

            ViewBag.IsPets = _Util.Facade.LookupFacade.GetDropdownsByKey("Pets");

            ViewBag.TypeOfPets = _Util.Facade.LookupFacade.GetDropdownsByKey("TypeOfPets");

            ViewBag.IsRepair = _Util.Facade.LookupFacade.GetDropdownsByKey("Repair");

            ViewBag.IsVipClubMember = _Util.Facade.LookupFacade.GetDropdownsByKey("VipClubMember");
            ViewBag.TypeOfRepair = _Util.Facade.LookupFacade.GetDropdownsByKey("TypeOfRepair");

            var objsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "CustomerTypeRequired");
            if (objsetting != null)
            {
                ViewBag.CustomerTypeRequired = Convert.ToBoolean(objsetting.Value);
            }
            else
            {
                ViewBag.CustomerTypeRequired = false;
            }

            var objEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
            List<SelectListItem> SalesPerson = new List<SelectListItem>();

            if (IsPermitted(UserPermissions.MyCompanyPermissions.ShowAllSalesPerson))
            {
                Employee objcurrentlog = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                SalesPerson.Add(new SelectListItem()
                {
                    Text = "Please Select One",
                    Value = ""
                });

                List<Employee> Employeelist = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Where(x => x.UserId != currentLoggedIn.UserId).ToList();
                if (objcurrentlog != null)
                {
                    Employeelist.Add(objcurrentlog);
                    SalesPerson.Add(new SelectListItem()
                    {
                        Text = objcurrentlog.FirstName + " " + objcurrentlog.LastName,
                        Value = objcurrentlog.UserId.ToString(),
                        Selected = true
                    });
                }

                SalesPerson.AddRange(Employeelist.Select(x =>
                          new SelectListItem()
                          {
                              Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                              Value = x.UserId.ToString()
                          }).ToList());

                ViewBag.EmployeeList = Employeelist;

                ViewBag.SalesPerson = SalesPerson.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
            }
            else
            {
                List<Employee> Employeelist = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Where(x => x.UserId == currentLoggedIn.UserId).ToList();
                if (objEmp != null)
                {
                    SalesPerson.Add(new SelectListItem()
                    {
                        Text = objEmp.FirstName.ToString() + " " + objEmp.LastName.ToString(),
                        Value = objEmp.UserId.ToString()
                    });
                    ViewBag.SalesPerson = SalesPerson.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
                }
                ViewBag.EmployeeList = Employeelist;
            }

            UserBranch ub = _Util.Facade.UserCompanyFacade.GetUserBranchByUserId(currentLoggedIn.UserId);
            List<SelectListItem> BranchList = new List<SelectListItem>();
            BranchList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            if (ub != null && id == 0)
            {
                BranchList.AddRange(_Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(currentLoggedIn.CompanyId.Value).OrderBy(x => x.Id.ToString() != "-1").ThenBy(x => x.Name).Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.Id == ub.BranchId,
                }).ToList());
                ViewBag.BranchList = BranchList.ToList();
            }
            else
            {
                BranchList.AddRange(_Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(currentLoggedIn.CompanyId.Value).OrderBy(x => x.Id.ToString() != "-1").ThenBy(x => x.Name).Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                    Selected = x.IsMainBranch == true,
                }).ToList());
                ViewBag.BranchList = BranchList.ToList();
            }



            //ViewBag.SalesLocation = _Util.Facade.LookupFacade.GetLookupByKey("CommissionType").Select(x =>
            //        new SelectListItem()
            //        {
            //            Text = x.DisplayText.ToString(),
            //            Value = x.DataValue.ToString()
            //        }).ToList();
            List<SelectListItem> SalesCommisssion = new List<SelectListItem>();
            SalesCommisssion.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("CommissionType").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList());
            ViewBag.SalesCommisssion = SalesCommisssion.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            if (model.SalesLocation == "" || model.SalesLocation == "-1" || model.SalesLocation == null)
            {
                model.SalesLocation = objEmp.SalesCommissionStructure;
            }
            if (!string.IsNullOrWhiteSpace(model.Soldby))
            {
                ViewBag.SelectSoldby = model.Soldby;
            }
            else
            {
                ViewBag.SelectSoldby = objEmp.UserId.ToString();
            }
            if (model.CustomerExtended != null && model.CustomerExtended.FinanceRep != null && model.CustomerExtended.FinanceRep != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                ViewBag.SelectSoldbyFin = model.CustomerExtended.FinanceRep;
            }
            else
            {
                ViewBag.SelectSoldbyFin = objEmp.UserId;

            }
            if (model.CustomerExtended != null && model.CustomerExtended.AppoinmentSetBy != null && model.CustomerExtended.AppoinmentSetBy != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                ViewBag.SelectAppoinmentBy = model.CustomerExtended.AppoinmentSetBy;
            }
            //else
            //{
            //    ViewBag.SelectSoldbyFin = objEmp.UserId;

            //}
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
            ViewBag.EmpSalesLocation = emp.SalesCommissionStructure;

            var objsettings = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "LeadSourceRequired");
            if (objsettings != null)
            {
                ViewBag.LeadSourceRequired = Convert.ToBoolean(objsettings.Value);
            }
            else
            {
                ViewBag.LeadSourceRequired = false;
            }
            #region Credit Check Info
            CustomerCreditCheck creditCheck = new CustomerCreditCheck();
            creditCheck = _Util.Facade.CustomerFacade.GetAllCustomerCreditCheckByCustomerId(model.CustomerId).FirstOrDefault();
            if (creditCheck != null)
            {
                model.CreditCheck = creditCheck;
                ViewBag.HasCreditCheck = "true";
            }
            else
            {
                model.CreditCheck = null;
                ViewBag.HasCreditCheck = "false";
            }


            GlobalSetting GlobSet = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("PassingCreditScore", currentLoggedIn.CompanyId.Value);
            if (GlobSet != null)
            {
                ViewBag.CreditScoreValue = GlobSet.Value;
            }
            else
            {
                ViewBag.CreditScoreValue = 0;
            }
            #endregion
            var refcusobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.MoveCustomerId);
            if (refcusobj != null)
            {
                model.MovedCustomerName = !string.IsNullOrWhiteSpace(refcusobj.DBA) ? refcusobj.DBA : !string.IsNullOrWhiteSpace(refcusobj.BusinessName) ? refcusobj.BusinessName : refcusobj.FirstName + " " + refcusobj.LastName;
                model.MovedCustomerId = refcusobj.Id;
            }
            else
            {
                model.MovedCustomerId = 0;
            }

            return View("LeadInfoPartial", model);


        }
        //public ActionResult FillState(string ParentDataKey)
        //{
        //    var stateList = _Util.Facade.LookupFacade.GetLookupByKey("USACitiesList").Where(m => m.ParentDataKey==ParentDataKey).ToList();
        //    return Json(stateList, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult AddLeadAssignedToUser()
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            List<SelectListItem> AccessAssignedPersons = new List<SelectListItem>();
            AccessAssignedPersons.Add(new SelectListItem()
            {
                Text = "Select One",
                Value = new Guid().ToString()
            });
            List<Employee> EmployeeDropDown = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value);
            if (EmployeeDropDown != null && EmployeeDropDown.Count > 0)
            {
                AccessAssignedPersons.AddRange(EmployeeDropDown.OrderBy(x => x.FirstName).Select(x => new SelectListItem()
                {
                    Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                    Value = x.UserId.ToString()
                }).ToList());
            }

            ViewBag.AccessAssignedPersons = AccessAssignedPersons;


            return View(EmployeeDropDown);
        }

        [Authorize]
        public ActionResult AddVerifyInfo(int? Id)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            Customer model;
            ViewBag.GoogleMapAPIKey = _Util.Facade.GlobalSettingsFacade.GetGoogleMapAPIKeyByCompanyId(CurrentLoggedInUser.CompanyId.Value);

            if (Id.HasValue && Id.Value > 0)
            {
                model = _Util.Facade.CustomerFacade.GetCustomersById(Id.Value);
                model.CustomerSystemInfo = _Util.Facade.CustomerSystemInfoFacade.GetCustomerSystemInfoByCustomerIdAndCompanyId(model.CustomerId, CurrentLoggedInUser.CompanyId.Value);
                if (model.CustomerSystemInfo == null)
                {
                    model.CustomerSystemInfo = new CustomerSystemInfo();
                }
                model.CustomerSpouse = _Util.Facade.CustomerFacade.GetSpouseByCustomerIdAndComapnyId(model.CustomerId, CurrentLoggedInUser.CompanyId.Value);
                if (model.CustomerSpouse == null)
                {
                    model.CustomerSpouse = new CustomerSpouse();
                }
            }
            else
            {
                model = new Customer();
                model.CustomerSystemInfo = new CustomerSystemInfo();
                model.CustomerSpouse = new CustomerSpouse();
            }
            var cityListForDropdown = _Util.Facade.LookupFacade.GetLookupByKey("USACitiesList").ToList();
            ViewBag.cityListForDropdown = cityListForDropdown;

            ViewBag.CityList = _Util.Facade.LookupFacade.GetLookupByKey("USACitiesList").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString(),

                        }).ToList();

            ViewBag.StateList = _Util.Facade.LookupFacade.GetLookupByKey("StateList").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList();
            ViewBag.CustomerTypeList = _Util.Facade.LookupFacade.GetLookupByKey("CustomerType").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
            ViewBag.LeadStreetType = _Util.Facade.LookupFacade.GetLookupByKey("LeadStreetType").OrderBy(x => x.DataValue).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
            ViewBag.LeadInstallType = _Util.Facade.LookupFacade.GetLookupByKey("LeadInstallType").Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
            List<SelectListItem> SalesPerson = new List<SelectListItem>();
            if (IsPermitted(UserPermissions.MyCompanyPermissions.ShowAllSalesPerson))
            {
                var objcurrentlog = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
                SalesPerson.Add(new SelectListItem()
                {
                    Text = "Please Select One",
                    Value = ""
                });
                if (objcurrentlog != null)
                {
                    SalesPerson.Add(new SelectListItem()
                    {
                        Text = objcurrentlog.FirstName + " " + objcurrentlog.LastName,
                        Value = objcurrentlog.UserId.ToString()
                    });
                }
                SalesPerson.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentLoggedInUser.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).OrderBy(x => x.FirstName).Select(x =>
                      new SelectListItem()
                      {
                          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                          Value = x.UserId.ToString()
                      }).ToList());
                SalesPerson = SalesPerson.OrderBy(x => x.Text != "Select One").ThenBy(x => x.Text).ToList();
                ViewBag.SalesPerson = SalesPerson;
                if (string.IsNullOrWhiteSpace(model.Soldby))
                {
                    model.Soldby = objcurrentlog.UserId.ToString();
                }
            }
            else
            {
                var objEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
                if (objEmp != null)
                {
                    SalesPerson.Add(new SelectListItem()
                    {
                        Text = objEmp.FirstName.ToString() + " " + objEmp.LastName.ToString(),
                        Value = CurrentLoggedInUser.UserId.ToString()
                    });
                    ViewBag.SalesPerson = SalesPerson;
                }
            }
            List<SelectListItem> LeadStatus = new List<SelectListItem>();
            LeadStatus.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("LeadStatus").Where(x => x.DataValue != "New").OrderBy(x => x.DataValue.ToString() != "-1").ThenBy(x => x.DisplayText).Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.LeadStatus = LeadStatus;
            ViewBag.geo = _Util.Facade.GlobalSettingsFacade.GetGeoLocation(CurrentLoggedInUser.CompanyId.Value);
            return PartialView("AddVerifyInfo", model);
        }

        [Authorize]
        public ActionResult LeadSetupPartial(int? id, string setup)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            string LeadCity = "";
            string LeadState = "";
            string TypeStreet = "";
            string Lappartment = "";
            if (id.HasValue)
            {
                ViewBag.LeadTitle = "";
                Customer CustomerInfo = _Util.Facade.CustomerFacade.GetCustomersById(id.Value);
                if (CustomerInfo != null)
                {
                    if (CustomerInfo.Type == "Commercial")
                    {
                        ViewBag.LeadTitle = CustomerInfo.BusinessName;
                    }
                    else
                    {
                        var FormattedName = CustomerInfo.FirstName + " " + CustomerInfo.LastName;
                        if (CustomerInfo.MiddleName != "")
                        {
                            FormattedName = CustomerInfo.FirstName + " " + CustomerInfo.MiddleName + " " + CustomerInfo.LastName;
                        }
                        ViewBag.LeadTitle = FormattedName;
                    }
                    if (CustomerInfo.City != "")
                    {
                        LeadCity = CustomerInfo.City + ", ";
                    }
                    if (CustomerInfo.State != "")
                    {
                        LeadState = CustomerInfo.State + " ";
                    }
                    if (CustomerInfo.StreetType != "-1")
                    {
                        TypeStreet = CustomerInfo.StreetType + " ";
                    }
                    if (!string.IsNullOrWhiteSpace(CustomerInfo.Appartment))
                    {
                        Lappartment = "#" + CustomerInfo.Appartment;
                    }
                    ViewBag.LeadAddress = CustomerInfo.Street + " " + TypeStreet + Lappartment + " " + UppercaseFirst(LeadCity) + LeadState + CustomerInfo.ZipCode;
                    ViewBag.LeadGuid = CustomerInfo.CustomerId;
                }
                ViewBag.LeadSetupId = id.Value;
                ViewBag.setupClick = setup;
                ViewBag.LeadPackageDetail = _Util.Facade.PackageFacade.GetAllLeadPackageDetailByLeadIdandCompanyId(CurrentLoggedInUser.CompanyId.Value, id.Value);
                ViewBag.LeadEmergencyDetail = _Util.Facade.EmergencyContactFacade.GetAllLeadEmergencyDetailByLeadIdandCompanyId(CurrentLoggedInUser.CompanyId.Value, id.Value);
                ViewBag.LeadEquipmentDetail = _Util.Facade.CustomerAppoinmentFacade.GetAllLeadEquipmentDetailByLeadIdandCompanyId(CurrentLoggedInUser.CompanyId.Value, id.Value);
                if (((List<LeadPackageDetail>)ViewBag.LeadPackageDetail).Count > 0 && ((List<LeadEquipmentDetail>)ViewBag.LeadEquipmentDetail).Count > 0 && ((List<LeadEmergencyDetail>)ViewBag.LeadEmergencyDetail).Count > 0 && (!string.IsNullOrWhiteSpace(CustomerInfo.ContractTeam) && !string.IsNullOrWhiteSpace(CustomerInfo.MonthlyMonitoringFee)))
                {
                    ViewBag.FourthSetup = "FourthSetup";
                }
                else if (((List<LeadPackageDetail>)ViewBag.LeadPackageDetail).Count > 0 && ((List<LeadEquipmentDetail>)ViewBag.LeadEquipmentDetail).Count > 0 && (!string.IsNullOrWhiteSpace(CustomerInfo.ContractTeam) && !string.IsNullOrWhiteSpace(CustomerInfo.MonthlyMonitoringFee)))
                {
                    ViewBag.ThirdSetup = "ThirdSetup";
                }
                else if (((List<LeadPackageDetail>)ViewBag.LeadPackageDetail).Count > 0 && ((List<LeadEquipmentDetail>)ViewBag.LeadEquipmentDetail).Count > 0)
                {
                    ViewBag.SecondSetup = "SecondSetup";
                }
                else
                {
                    ViewBag.FirstSetup = "FirstSetup";
                }
            }

            return PartialView("LeadSetupPartial");
        }


        public ActionResult PackageSettingsList(int id)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ViewBag.PackageId = id.ToString();
            ViewBag.PackageName = _Util.Facade.PackageFacade.GetPackageByIdAndCompanyId(id, new Guid(CurrentLoggedInUser.CompanyId.ToString())).Name;
            ViewBag.packIncludeEquipment = _Util.Facade.PackageFacade.GetAllPackageIncludeEquipmentListByPackageIdAndCompanyId(id, new Guid(CurrentLoggedInUser.CompanyId.ToString())).OrderBy(x => x.OrderBy).ToList();
            ViewBag.packDevice = _Util.Facade.PackageFacade.GetAllPackageDeviceProducts(new Guid(CurrentLoggedInUser.CompanyId.ToString())).Where(x => x.PackageId == id).ToList();
            ViewBag.packOptional = _Util.Facade.PackageFacade.GetAllPackageOptionalProducts(new Guid(CurrentLoggedInUser.CompanyId.ToString())).Where(x => x.PackageId == id).ToList();
            return PartialView();
        }
        public ActionResult PackagePartial(int? id)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            CustomInitialLeadPackageModel model = new CustomInitialLeadPackageModel();
            if (id.HasValue)
            {
                ViewBag.CustomerIDForPackageInstall = id.Value;
                var LeadGuidId = _Util.Facade.CustomerFacade.GetCustomersById(id.Value).CustomerId;
                model.CustomerSystemInfo = _Util.Facade.CustomerSystemInfoFacade.GetAllCustomerSystemInfoDetailsByCustomerId(LeadGuidId);
                if (id.Value > 0)
                {
                    if (LeadGuidId != new Guid())
                    {
                        var PackageSystemTypeId = _Util.Facade.PackageFacade.GetPackageSystemCustomerByCustomerIdandCompanyId(LeadGuidId, CurrentLoggedInUser.CompanyId.Value);

                        if (PackageSystemTypeId != null)
                        {
                            model.PackageSystemType = PackageSystemTypeId.PackageSystemId;
                        }
                    }
                    if (LeadGuidId != new Guid())
                    {
                        var PackageCustomerId = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerIdandCompanyId(LeadGuidId, CurrentLoggedInUser.CompanyId.Value);
                        if (PackageCustomerId != null)
                        {
                            var PackageDetails = _Util.Facade.PackageFacade.GetPackageByPackageIdAndCompanyId(PackageCustomerId.PackageId, CurrentLoggedInUser.CompanyId.Value);
                            if (PackageDetails != null)
                            {
                                model.PackageType = PackageDetails.Id;
                            }
                        }
                    }
                    if (LeadGuidId != new Guid())
                    {
                        var AppointmentId = _Util.Facade.CustomerAppoinmentFacade.GetCustomerLastAppointmentIdByCustomerIdAndCompanyId(LeadGuidId, CurrentLoggedInUser.CompanyId.Value);
                        if (AppointmentId != new Guid())
                        {
                            var CustomerAppointmentDetail = _Util.Facade.CustomerAppoinmentDetailFacade.GetCustomerAppointmentDetailByAppointmentId(AppointmentId);
                            if (CustomerAppointmentDetail != null)
                            {
                                model.InstallType = CustomerAppointmentDetail.InstallType;
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.CustomerIDForPackageInstall = 0;
                }
            }
            //List<SelectListItem> TypeInstall = new List<SelectListItem>();
            //TypeInstall.Add(new SelectListItem{ Text = "New Install", Value = "NewInstall" });
            //TypeInstall.Add(new SelectListItem { Text = "Takeover", Value = "Takeover" });
            //ViewBag.Install = TypeInstall;
            ViewBag.Install = _Util.Facade.LookupFacade.GetLookupByKey("LeadInstallType").Select(x =>
                              new SelectListItem()
                              {
                                  Text = x.DisplayText.ToString(),
                                  Value = x.DataValue.ToString()
                              }).ToList();
            List<SelectListItem> PackageSystemList = new List<SelectListItem>();
            PackageSystemList.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            PackageSystemList.AddRange(_Util.Facade.PanelTypeFacade.GetAllPanelTypeByCompanyId(CurrentLoggedInUser.CompanyId.Value).OrderBy(s => s.Name).Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.Name.ToString(),
                                 Value = x.Id.ToString()
                             }).ToList());
            ViewBag.PackageSystemList = PackageSystemList;
            List<SelectListItem> PackageList = new List<SelectListItem>();
            PackageList.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            PackageList.AddRange(_Util.Facade.PackageFacade.GetAllPackageListByCompanyId(CurrentLoggedInUser.CompanyId.Value).OrderBy(s => s.Name).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.Name.ToString(),
                              Value = x.Id.ToString()
                          }).ToList());
            ViewBag.PackageList = PackageList;
            return PartialView("PackagePartial", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteCustomerAppointmentEquipment(int id, int PDCId, Guid AppoinmentId, Guid EquipmentId)
        {

            CustomerAppointmentEquipment eq = _Util.Facade.CustomerAppoinmentFacade.GetAppoinmentEquipmentByIdAppoinmentIdAndEquipmentId(id, AppoinmentId, EquipmentId);
            if (eq == null)
            {
                return Json(new { result = false, message = "Not found." });
            }
            _Util.Facade.PackageFacade.DeletePackageDetailCustomerById(PDCId);
            _Util.Facade.CustomerAppoinmentFacade.DeleteCustomerAppoinmentEquipment(eq.Id);

            return Json(new { result = true, message = "Deleted successfully." });
        }

        [Authorize]
        public ActionResult EquipmentPartial(int? LeadId, Guid? Appointmentid)
        {

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<PackageInclude> packagelist = new List<PackageInclude>();

            AddLeadEquipment model = new AddLeadEquipment();
            if (LeadId.HasValue)
            {
                var CustomerInfo = _Util.Facade.CustomerFacade.GetCustomersById(LeadId.Value);
                ViewBag.EquipmentPartialLeadId = LeadId;
                if (CustomerInfo != null)
                {
                    Appointmentid = _Util.Facade.CustomerAppoinmentFacade.GetCustomerLastAppointmentIdByCustomerIdAndCompanyId(CustomerInfo.CustomerId, currentLoggedIn.CompanyId.Value);
                }

                packagelist = _Util.Facade.PackageFacade.GetPackageIncludeListByCustomerId(CustomerInfo.CustomerId);
                List<PackageDetailCustomer> packageDetailCustomer = _Util.Facade.PackageFacade.GetAllPackageDetailCustomerByCustomerIdAndCompanyId(CustomerInfo.CustomerId, currentLoggedIn.CompanyId.Value);
                model.packageDetailCustomer = packageDetailCustomer;
            }
            else
            {
                ViewBag.EquipmentPartialLeadId = 0;
            }
            if (Appointmentid != new Guid())
            {
                List<CustomerAppointmentEquipment> LeadEquipmentExist = _Util.Facade.CustomerFacade.IsLeadAppointmentEquipmentExistCheck(Appointmentid.Value);
                if (LeadEquipmentExist != null)
                {

                    if (packagelist != null && packagelist.Count() > 0)
                    {
                        int i = 0;
                        foreach (var item in LeadEquipmentExist)
                        {
                            if (i < model.packageDetailCustomer.Count())
                            {
                                if (packagelist.Where(x => x.EquipmentId == item.EquipmentId).Count() > 0 && model.packageDetailCustomer[i].IsIncluded == true)
                                {
                                    item.IsDeletable = false;
                                }
                                else
                                {
                                    item.IsDeletable = true;
                                }
                            }
                            else
                            {
                                item.IsDeletable = true;
                            }
                            if (model.packageDetailCustomer.Count > i)
                                item.PDCId = model.packageDetailCustomer[i].Id;
                            i++;
                        }
                    }
                    model.PackageEquipmentsList = LeadEquipmentExist;
                }
            }

            return PartialView("EquipmentPartial", model);
        }

        public ActionResult ContactTermtPartial(int? id)
        {
            int max = 0;
            int min = 0;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var objcus = _Util.Facade.CustomerFacade.GetById(id.Value);
            if (objcus != null)
            {
                var packid = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerIdandCompanyId(objcus.CustomerId, currentLoggedIn.CompanyId.Value);
                if (packid != null)
                {
                    var objmmrRange = _Util.Facade.PackageFacade.GetMMrRangeByPackageId(packid.PackageId);
                    if (objmmrRange != null)
                    {
                        max = Convert.ToInt32(objmmrRange.MaxMMR);
                        min = Convert.ToInt32(objmmrRange.MinMMR);
                    }
                }
            }
            LeadServiceSetupCustomModel model = new LeadServiceSetupCustomModel();
            model.CustomerModel = new Customer();
            model.PaymentInfo = new PaymentInfo();
            //model.ActivationFeePaymentInfoModel = new PaymentInfo();
            //Customer CUSmodel = new Customer();
            if (id.HasValue)
            {
                //CUSmodel = _Util.Facade.CustomerFacade.GetLeadIdByCompanyIdAndCustomerId(currentLoggedIn.CompanyId.Value, id.Value);
                model.CustomerModel = _Util.Facade.CustomerFacade.GetCustomersById(id.Value);
                if (model.CustomerModel.CustomerId != new Guid())
                {

                    model.PaymentInfoList = _Util.Facade.PaymentInfoFacade.GetPaymentInfoListByCompanyIdAndCustomerId(currentLoggedIn.CompanyId.Value, model.CustomerModel.CustomerId);
                    if (model.PaymentInfoList == null)
                    {
                        model.PaymentInfoList = new List<PaymentInfo>();
                    }
                    //var ModelMMRIsExist = _Util.Facade.PaymentInfoFacade.GetLeadPaymentInfoByCustomerIdAndCompanyId(model.CustomerModel.CustomerId, currentLoggedIn.CompanyId.Value, "MMR");
                    //if (ModelMMRIsExist != null)
                    //{
                    //    model.MMRPaymentInfoModel = ModelMMRIsExist;
                    //}

                    //var ModelAFIsExist = _Util.Facade.PaymentInfoFacade.GetLeadPaymentInfoByCustomerIdAndCompanyId(model.CustomerModel.CustomerId, currentLoggedIn.CompanyId.Value, "AF");
                    //if (ModelAFIsExist != null)
                    //{
                    //    model.ActivationFeePaymentInfoModel = ModelAFIsExist;
                    //}
                }
            }
            var objLead = _Util.Facade.CustomerFacade.GetLeadIdByCompanyIdAndCustomerId(currentLoggedIn.CompanyId.Value, id.Value);
            //ViewBag.LeadContractId = id;
            if (objLead != null)
            {
                ViewBag.leadcontractid = objLead.Id;
                ViewBag.leadcontractcustomerid = objLead.CustomerId;
                ViewBag.leadcontractfirstname = objLead.FirstName;
                ViewBag.leadcontractlastname = objLead.LastName;
                ViewBag.leadcontractstreet = objLead.Street;
                ViewBag.leadcontractZipCode = objLead.ZipCode;
            }
            #region viewbag contract terms
            if (currentLoggedIn.UserTags.ToLower() == "admin")
            {
                ViewBag.ContractTerm = _Util.Facade.LookupFacade.GetLookupByKey("ContractTerm").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            }
            else
            {
                ViewBag.ContractTerm = _Util.Facade.LookupFacade.GetLookupByKey("SalesContractTerm").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            }
            #endregion
            #region viewbag Monitoring List
            List<SelectListItem> MonitoringList = new List<SelectListItem>();
            MonitoringList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            MonitoringList.AddRange(_Util.Facade.CustomerFacade.GetLeadMMRValueListByCompanyId(currentLoggedIn.CompanyId.Value, max, min).Select(x =>
                            new SelectListItem()
                            {
                                Text = x.Name.ToString(),
                                Value = x.Value.ToString()
                            }).ToList());
            MonitoringList = MonitoringList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            ViewBag.MonthlyMonitoringFee = MonitoringList;
            #endregion
            #region viewbag Activation Fee List
            List<SelectListItem> ActivationFeeList = new List<SelectListItem>();
            ActivationFeeList.Add(new SelectListItem()
            {
                Text = "Please Select One",
                Value = "-1"
            });
            ActivationFeeList.AddRange(_Util.Facade.CustomerFacade.GetAllActivationFeeValueByCompanyId(currentLoggedIn.CompanyId.Value).Select(x =>
                            new SelectListItem()
                            {
                                Text = x.Name.ToString(),
                                Value = x.Fee.ToString()
                            }).ToList());
            ActivationFeeList = ActivationFeeList.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
            ViewBag.ActivationFee = ActivationFeeList;
            #endregion
            #region viewbag MMR Paying

            List<SelectListItem> MMRPaying = new List<SelectListItem>();
            MMRPaying.Add(new SelectListItem() { Value = "-1", Text = "Select One" });
            MMRPaying.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("PaymentMethod").Where(x => x.DataValue == "ACH" || x.DataValue == "Credit Card").Select(x =>
                              new SelectListItem()
                              {
                                  Text = x.DisplayText.ToString(),
                                  Value = x.DataValue.ToString()
                              }).ToList());
            MMRPaying = MMRPaying.OrderBy(x => x.Text != "Select One").ThenBy(x => x.Text).ToList();
            ViewBag.MMRPaying = MMRPaying;
            #endregion
            #region viewbag activation fee payment method list
            ViewBag.PaymentMethodActivationFee = _Util.Facade.LookupFacade.GetLookupByKey("PaymentMethodActivationFee").Select(x =>
                              new SelectListItem()
                              {
                                  Text = x.DisplayText.ToString(),
                                  Value = x.DataValue.ToString()
                              }).ToList();
            #endregion
            #region Billing Day
            ViewBag.BillingDay = _Util.Facade.LookupFacade.GetLookupByKey("BillingDay").Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.DisplayText.ToString(),
                                 Value = x.DataValue.ToString()
                             }).ToList();
            #endregion
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
            return PartialView("ContactTermtPartial", model);
        }
        public ActionResult PaymentPartial(int? PaymentLeadId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            PaymentInfo model = new PaymentInfo();
            if (PaymentLeadId.HasValue)
            {
                var objPaymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoByCompanyIdandLeadId(currentLoggedIn.CompanyId.Value, PaymentLeadId.Value);
                var objPaymentInfo1 = _Util.Facade.PaymentInfoFacade.GetPaymentInfo1ByCompanyIdandLeadId(currentLoggedIn.CompanyId.Value, PaymentLeadId.Value);
                var objPaymentInfoEFT = _Util.Facade.PaymentInfoFacade.GetPaymentInfoEFTByCompanyIdandLeadId(currentLoggedIn.CompanyId.Value, PaymentLeadId.Value);
                var objPaymentInfoCredit = _Util.Facade.PaymentInfoFacade.GetPaymentInfoCreditTByCompanyIdandLeadId(currentLoggedIn.CompanyId.Value, PaymentLeadId.Value);
                var objPaymentInfoCheck = _Util.Facade.PaymentInfoFacade.GetPaymentInfoCheckByCompanyIdAndLeadId(currentLoggedIn.CompanyId.Value, PaymentLeadId.Value);
                var objPaymentInfoCash = _Util.Facade.PaymentInfoFacade.GetPaymentInfoCashByCompanyIdAndLeadId(currentLoggedIn.CompanyId.Value, PaymentLeadId.Value);

                if (objPaymentInfo != null && objPaymentInfo.BillMethod == "ACH")
                {
                    model = objPaymentInfo;
                }
                else if (objPaymentInfo1 != null && objPaymentInfo1.BillMethod == "Debit Card")
                {
                    model = objPaymentInfo1;
                }
                else if (objPaymentInfoEFT != null && objPaymentInfoEFT.BillMethod == "EFT")
                {
                    model = objPaymentInfoEFT;
                }
                else if (objPaymentInfoCredit != null && objPaymentInfoCredit.BillMethod == "Credit Card")
                {
                    model = objPaymentInfoCredit;
                }
                else if (objPaymentInfoCheck != null && objPaymentInfoCheck.BillMethod == "Check")
                {
                    model = objPaymentInfoCheck;
                }
                else if (objPaymentInfoCash != null && objPaymentInfoCash.BillMethod == "Cash")
                {
                    model = objPaymentInfoCash;
                }
            }
            ViewBag.BillingMethod = _Util.Facade.LookupFacade.GetLookupByKey("PaymentMethod").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            model.PaymentCustomerId = _Util.Facade.CustomerFacade.GetLeadIdByCompanyIdAndCustomerId(currentLoggedIn.CompanyId.Value, PaymentLeadId.Value).CustomerId;
            return PartialView("PaymentPartial", model);
        }

        public ActionResult EmergencyContactPartial(int? id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            EmergencyContact model = new EmergencyContact();
            if (currentLoggedIn != null)
            {
                if (id.HasValue)
                {
                    var LeadGuidId = _Util.Facade.CustomerFacade.GetCustomersById(id.Value).CustomerId;
                    if (LeadGuidId != new Guid())
                    {
                        var ExistingEmergencyInfo = _Util.Facade.EmergencyContactFacade.GetEmergencyContactByCustomerIdAndCompanyId(LeadGuidId, currentLoggedIn.CompanyId.Value);
                        if (ExistingEmergencyInfo != null)
                        {
                            model = ExistingEmergencyInfo;
                        }
                    }
                }

            }

            ViewBag.LeadCustomerID = _Util.Facade.CustomerFacade.GetLeadIdByCompanyIdAndCustomerId(currentLoggedIn.CompanyId.Value, id.Value).CustomerId;
            model.LeadId = id ?? default(int);
            return PartialView("EmergencyContactPartial", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddLeadSetup(EmergencyContact ec, PaymentInfo pi4, LeadServiceSetupCustomModel CUS, AddLeadPackage ModelAddleadPackage, PaymentInfo pi, PaymentInfo pi2, PaymentInfo pi1, AddLeadAddedEquipments AddedEquipmentsList, PaymentInfo PaymentCash, PaymentInfo PaymentCheck)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            //bool result1 = false;
            bool result = false;
            var LeadAppointmentId = new Guid();

            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            #region Insert and Update Package settings and Equipment Settings

            if (ModelAddleadPackage.LeadId > 0 || AddedEquipmentsList.EquipmentLeadId > 0)
            {
                #region Equipment Setup
                if (AddedEquipmentsList.AddedEquipmetList != null)
                {
                    if (AddedEquipmentsList.EquipmentLeadId > 0)
                    {
                        var LeadDetails = _Util.Facade.CustomerFacade.GetCustomersById(AddedEquipmentsList.EquipmentLeadId);
                        if (LeadDetails != null)
                        {
                            var LeadOldAppointmentId = _Util.Facade.CustomerAppoinmentFacade.GetCustomerLastAppointmentIdByCustomerIdAndCompanyId(LeadDetails.CustomerId, currentLoggedIn.CompanyId.Value);
                            if (LeadOldAppointmentId != new Guid())
                            {
                                if (AddedEquipmentsList.AddedEquipmetList.Count > 0)
                                {
                                    foreach (var item in AddedEquipmentsList.AddedEquipmetList)
                                    {
                                        if (item.EquipmentId != new Guid())
                                        {
                                            CustomerAppointmentEquipment _CustomerAppointmentEquipment = new CustomerAppointmentEquipment()
                                            {
                                                AppointmentId = LeadOldAppointmentId,
                                                EquipmentId = item.EquipmentId,
                                                Quantity = item.Quantity,
                                                UnitPrice = item.UnitPrice,
                                                TotalPrice = item.TotalPrice,
                                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                                CreatedBy = User.Identity.Name
                                            };
                                            result = _Util.Facade.CustomerAppoinmentDetailFacade.InsertCustomerAppointmentEquipment(_CustomerAppointmentEquipment) > 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return Json(new { result = result });
                }

                #endregion

                #region PackageSetup
                if (ModelAddleadPackage.EquipmentList != null)
                {
                    var LeadInfo = _Util.Facade.CustomerFacade.GetCustomersById(ModelAddleadPackage.LeadId);

                    if (LeadInfo != null)
                    {
                        var CheckLeadWorkOrder = _Util.Facade.CustomerAppoinmentFacade.GetCustomerLastAppointmentIdByCustomerIdAndCompanyId(LeadInfo.CustomerId, currentLoggedIn.CompanyId.Value);
                        if (CheckLeadWorkOrder != null)
                        {
                            var PackageDetails = new Package();
                            if (ModelAddleadPackage.PackageIdInt > 0)
                            {
                                PackageDetails = _Util.Facade.PackageFacade.GetPackageByIdAndCompanyId(ModelAddleadPackage.PackageIdInt, currentLoggedIn.CompanyId.Value);
                            }
                            #region Edit Package
                            if (CheckLeadWorkOrder != new Guid())
                            {
                                //update PackageSystemCustomer 
                                var OldPackageSystemCustomerobj = _Util.Facade.PackageFacade.GetPackageSystemCustomerByCustomerIdandCompanyId(LeadInfo.CustomerId, currentLoggedIn.CompanyId.Value);
                                if (OldPackageSystemCustomerobj != null)
                                {
                                    if (ModelAddleadPackage.SystemTypeId > 0)
                                    {
                                        OldPackageSystemCustomerobj.PackageSystemId = ModelAddleadPackage.SystemTypeId;
                                        _Util.Facade.PackageFacade.UpdatePackageSystemCustomer(OldPackageSystemCustomerobj);
                                    }
                                }

                                //update PackageCustomer 
                                var OldPackageCustomerObj = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerIdandCompanyId(LeadInfo.CustomerId, currentLoggedIn.CompanyId.Value);
                                if (OldPackageCustomerObj != null)
                                {
                                    //ChangePackage
                                    //if (ModelAddleadPackage.PackageId > 0)
                                    //{
                                    OldPackageCustomerObj.PackageId = PackageDetails.PackageId;
                                    _Util.Facade.PackageFacade.UpdatePackageCustomer(OldPackageCustomerObj);
                                    //}
                                }

                                //Update CustomerAppointmentDetail
                                var OldCustomerAppointmentObj = _Util.Facade.CustomerAppoinmentDetailFacade.GetCustomerAppointmentDetailByAppointmentId(CheckLeadWorkOrder);
                                if (OldCustomerAppointmentObj != null)
                                {
                                    if (ModelAddleadPackage.InstallType != "-1" && ModelAddleadPackage.InstallType != null)
                                    {
                                        OldCustomerAppointmentObj.InstallType = ModelAddleadPackage.InstallType;
                                        _Util.Facade.CustomerAppoinmentDetailFacade.UpdateCustomerAppoinmentDetail(OldCustomerAppointmentObj);
                                    }
                                }
                                //Delete old workorder products and insert new (update CustomerAppointmentProducts)
                                var OldCustomerAppointmentProducts = _Util.Facade.CustomerAppoinmentFacade.GetAllCustomerAppointmentEquipmentListByAppointmentId(CheckLeadWorkOrder);
                                if (OldCustomerAppointmentProducts.Count > 0)
                                {
                                    //deleteProducts
                                    var deleteSuccess = _Util.Facade.CustomerAppoinmentFacade.DeleteAllCustomerAppointmentEquipmentByAppointmentId(CheckLeadWorkOrder);

                                }
                                /*Add new equipment to Customer appointment  */
                                LeadAppointmentId = CheckLeadWorkOrder;
                                foreach (var item in ModelAddleadPackage.EquipmentList)
                                {
                                    if (item.SelectedEquipmentId != new Guid())
                                    {
                                        var ProductInfo = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(item.SelectedEquipmentId, currentLoggedIn.CompanyId.Value);
                                        var tempCAE = _Util.Facade.CustomerAppoinmentFacade.GetAllAppointmentEquipmentByAppointmentIdandEquipmentId(ProductInfo.EquipmentId, LeadAppointmentId);
                                        if (tempCAE.Count == 0)
                                        {
                                            if (ProductInfo != null)
                                            {
                                                if (item.SelectedEquipmentIsFree == true)
                                                {
                                                    ProductInfo.Retail = 0.0;
                                                }
                                                CustomerAppointmentEquipment DBCustomerAppointmentEquipment = new CustomerAppointmentEquipment()
                                                {
                                                    AppointmentId = LeadAppointmentId,
                                                    EquipmentId = ProductInfo.EquipmentId,
                                                    Quantity = item.NumOfEquipments,
                                                    UnitPrice = ProductInfo.Retail.Value,
                                                    TotalPrice = ProductInfo.Retail.Value,
                                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                                    CreatedBy = User.Identity.Name,
                                                };

                                                result = _Util.Facade.CustomerAppoinmentDetailFacade.InsertCustomerAppointmentEquipment(DBCustomerAppointmentEquipment) > 0;
                                            }
                                        }
                                    }
                                }
                                /*End Add new equipment to Customer appointment  */

                                var OldPackageDetailCustomerProducts = _Util.Facade.PackageFacade.GetAllPackageDetailCustomerByCustomerIdAndCompanyId(LeadInfo.CustomerId, currentLoggedIn.CompanyId.Value);
                                if (OldPackageDetailCustomerProducts.Count > 0)
                                {
                                    var deleteSuccessPackageDetailCustomer = _Util.Facade.PackageFacade.DeleteAllPackageDetailCustomerByCustomerIdAndComapnyId(LeadInfo.CustomerId, currentLoggedIn.CompanyId.Value);
                                    if (deleteSuccessPackageDetailCustomer == true)
                                    {
                                        if (ModelAddleadPackage.PackageCustomerEquipmentsList.Count > 0)
                                        {
                                            foreach (var item in ModelAddleadPackage.PackageCustomerEquipmentsList)
                                            {
                                                //check valid equipmentItem
                                                if (item.Type != "" && item.PackageEqpId > 0)
                                                {
                                                    PackageDetailCustomer DBPackageDetailCustomer = new PackageDetailCustomer()
                                                    {
                                                        CompanyId = currentLoggedIn.CompanyId.Value,
                                                        CustomerId = LeadInfo.CustomerId,
                                                        IsIncluded = item.IsIncluded,
                                                        PackageEqpId = item.PackageEqpId,
                                                        Type = item.Type
                                                    };
                                                    _Util.Facade.PackageFacade.InsertPackageDetailCustomer(DBPackageDetailCustomer);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region insert Package
                            else
                            {
                                //insert PackageSystemCustomer
                                if (ModelAddleadPackage.SystemTypeId > 0)
                                {
                                    PackageSystemCustomer ObjPackageSystemCustomer = new PackageSystemCustomer()
                                    {
                                        CompanyId = currentLoggedIn.CompanyId.Value,
                                        CustomerId = LeadInfo.CustomerId,
                                        PackageSystemId = ModelAddleadPackage.SystemTypeId
                                    };

                                    result = _Util.Facade.PackageFacade.InsertPackageSystemCustomer(ObjPackageSystemCustomer) > 0;
                                }
                                //ChangePackage
                                //insert PackageCustomer
                                //if (ModelAddleadPackage.PackageId > 0)
                                //{
                                PackageCustomer ObjPackageCustomer = new PackageCustomer()
                                {
                                    CompanyId = currentLoggedIn.CompanyId.Value,
                                    CustomerId = LeadInfo.CustomerId,
                                    PackageId = PackageDetails.PackageId,
                                };
                                _Util.Facade.PackageFacade.InsertPackageCustomer(ObjPackageCustomer);
                                //}

                                //insert lead work order
                                var tempInstaller = new Guid();
                                if (LeadInfo.Installer != "" && LeadInfo.Installer != new Guid().ToString())
                                {
                                    tempInstaller = Guid.Parse(LeadInfo.Installer);
                                }
                                CustomerAppointment LeadCustomerAppointmentObj = new CustomerAppointment()
                                {
                                    CustomerId = LeadInfo.CustomerId,
                                    AppointmentId = Guid.NewGuid(),
                                    CompanyId = currentLoggedIn.CompanyId.Value,
                                    EmployeeId = tempInstaller,
                                    AppointmentType = "WorkOrder",
                                    IsAllDay = false,
                                    Notes = "New lead workorder from setup package",
                                    CreatedBy = User.Identity.Name,
                                    LastUpdatedBy = User.Identity.Name,
                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime()
                                };
                                result = _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(LeadCustomerAppointmentObj) > 0;


                                if (result != false)
                                {
                                    //insert CustomerAppointmentDetail
                                    if (ModelAddleadPackage.InstallType != "-1")
                                    {
                                        CustomerAppointmentDetail DBCustomerAppointmentDetail = new CustomerAppointmentDetail()
                                        {
                                            AppointmentId = LeadCustomerAppointmentObj.AppointmentId,
                                            CollectedAmount = 0.0,
                                            InstallType = ModelAddleadPackage.InstallType,
                                        };
                                        result = _Util.Facade.CustomerAppoinmentDetailFacade.InsertCustomerAppoinmentDetail(DBCustomerAppointmentDetail) > 0;
                                    }

                                    //Insert workorder equipments
                                    if (ModelAddleadPackage.EquipmentList.Count > 0)
                                    {
                                        LeadAppointmentId = LeadCustomerAppointmentObj.AppointmentId;
                                        foreach (var item in ModelAddleadPackage.EquipmentList)
                                        {
                                            if (item.SelectedEquipmentId != new Guid())
                                            {
                                                var ProductInfo = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(item.SelectedEquipmentId, currentLoggedIn.CompanyId.Value);
                                                if (ProductInfo != null)
                                                {
                                                    CustomerAppointmentEquipment tempACE = _Util.Facade.CustomerAppoinmentFacade.GetAllAppointmentEquipmentByAppointmentIdandEquipmentId(ProductInfo.EquipmentId, LeadAppointmentId).FirstOrDefault();
                                                    if (tempACE == null)
                                                    {
                                                        if (item.SelectedEquipmentIsFree == true)
                                                        {
                                                            ProductInfo.Retail = 0.0;
                                                        }
                                                        CustomerAppointmentEquipment DBCustomerAppointmentEquipment = new CustomerAppointmentEquipment()
                                                        {
                                                            AppointmentId = LeadAppointmentId,
                                                            EquipmentId = ProductInfo.EquipmentId,
                                                            Quantity = item.NumOfEquipments,
                                                            UnitPrice = ProductInfo.Retail.Value,
                                                            TotalPrice = ProductInfo.Retail.Value,
                                                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                                                            CreatedBy = User.Identity.Name,
                                                        };

                                                        result = _Util.Facade.CustomerAppoinmentDetailFacade.InsertCustomerAppointmentEquipment(DBCustomerAppointmentEquipment) > 0;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //insert PackageDetailCustomer

                                    if (ModelAddleadPackage.PackageCustomerEquipmentsList.Count > 0)
                                    {
                                        foreach (var item in ModelAddleadPackage.PackageCustomerEquipmentsList)
                                        {
                                            //check valid equipmentItem
                                            if (item.Type != "" && item.PackageEqpId > 0)
                                            {
                                                PackageDetailCustomer DBPackageDetailCustomer = new PackageDetailCustomer()
                                                {
                                                    CompanyId = currentLoggedIn.CompanyId.Value,
                                                    CustomerId = LeadInfo.CustomerId,
                                                    IsIncluded = item.IsIncluded,
                                                    PackageEqpId = item.PackageEqpId,
                                                    Type = item.Type
                                                };
                                                _Util.Facade.PackageFacade.InsertPackageDetailCustomer(DBPackageDetailCustomer);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    return Json(new { result = result, CustomerAppointmetId = LeadAppointmentId });
                }
                #endregion
            }
            #endregion

            #region Emergency Contact Setup
            if (ec.CustomerId != null && ec.CustomerId != new Guid())
            {
                var LeadInfo = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(ec.CustomerId);
                if (LeadInfo != null)
                {
                    var LeadIntId = LeadInfo.Id;
                    return Json(new { result = true, LeadIDValue = LeadIntId });
                }
            }
            #endregion

            #region Payment Method Setup

            #region Payment Method Type ACH

            if (pi4.BillMethod == "ACH")
            {
                if (pi4.AccountName != null || pi4.AcountNo != null || pi4.BankAccountType != null || pi4.CardExpireDate != null || pi4.CardNumber != null || pi4.CardSecurityCode != null || pi4.CardType != null || pi4.RoutingNo != null)
                {
                    if (pi4.Id > 0)
                    {
                        pi4.CompanyId = currentLoggedIn.CompanyId.Value;
                        pi4.BillMethod = MethodBilling.ACH;
                        _Util.Facade.PaymentInfoFacade.UpdatePaymentInfo(pi4);
                        PaymentInfoCustomer objpayment = new PaymentInfoCustomer()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            CustomerId = pi4.PaymentCustomerId,
                            PaymentInfoId = pi4.Id
                        };
                        _Util.Facade.PaymentInfoCustomerFacade.UpdatePaymentInfoCustomer(objpayment);
                        var objRelationCustomerandPaymentinfo = _Util.Facade.CustomerFacade.GetLeadByPaymentinfoID(pi4.Id);
                        if (objRelationCustomerandPaymentinfo != null)
                        {
                            objRelationCustomerandPaymentinfo.PaymentMethod = pi4.BillMethod;
                            objRelationCustomerandPaymentinfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            objRelationCustomerandPaymentinfo.LastUpdatedBy = User.Identity.Name;

                            _Util.Facade.CustomerFacade.UpdateCustomer(objRelationCustomerandPaymentinfo);
                        }
                        return Json(new { result = true, LeadMethodBill = objRelationCustomerandPaymentinfo.PaymentMethod });
                    }
                    else
                    {
                        pi4.CompanyId = currentLoggedIn.CompanyId.Value;
                        pi4.BillMethod = MethodBilling.ACH;
                        _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(pi4);
                        PaymentInfoCustomer objpayment = new PaymentInfoCustomer()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            CustomerId = pi4.PaymentCustomerId,
                            PaymentInfoId = pi4.Id
                        };
                        _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(objpayment);
                        var objRelationCustomerandPaymentinfo = _Util.Facade.CustomerFacade.GetLeadByPaymentinfoID(pi4.Id);
                        if (objRelationCustomerandPaymentinfo != null)
                        {
                            objRelationCustomerandPaymentinfo.PaymentMethod = pi4.BillMethod;
                            objRelationCustomerandPaymentinfo.LastUpdatedBy = User.Identity.Name;
                            objRelationCustomerandPaymentinfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();

                            _Util.Facade.CustomerFacade.UpdateCustomer(objRelationCustomerandPaymentinfo);
                        }

                        return Json(new { result = true, LeadMethodBill = objRelationCustomerandPaymentinfo.PaymentMethod });
                    }
                }
            }

            #endregion

            #region Payment Method Type EFT
            if (pi.BillMethod == "EFT")
            {
                if (pi.AccountName != null || pi.AcountNo != null || pi.BankAccountType != null || pi.CardExpireDate != null || pi.CardNumber != null || pi.CardSecurityCode != null || pi.CardType != null || pi.RoutingNo != null)
                {
                    if (pi.Id > 0)
                    {
                        pi.CompanyId = currentLoggedIn.CompanyId.Value;
                        pi.BillMethod = MethodBilling.EFT;
                        _Util.Facade.PaymentInfoFacade.UpdatePaymentInfo(pi);
                        PaymentInfoCustomer objpayment = new PaymentInfoCustomer()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            CustomerId = pi.PaymentCustomerId,
                            PaymentInfoId = pi.Id
                        };
                        _Util.Facade.PaymentInfoCustomerFacade.UpdatePaymentInfoCustomer(objpayment);
                        var objRelationCustomerandPaymentinfo = _Util.Facade.CustomerFacade.GetLeadByPaymentinfoID(pi.Id);
                        if (objRelationCustomerandPaymentinfo != null)
                        {
                            objRelationCustomerandPaymentinfo.PaymentMethod = pi.BillMethod;
                            objRelationCustomerandPaymentinfo.LastUpdatedBy = User.Identity.Name;
                            objRelationCustomerandPaymentinfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            _Util.Facade.CustomerFacade.UpdateCustomer(objRelationCustomerandPaymentinfo);
                        }

                        return Json(new { result = true, LeadMethodBill = objRelationCustomerandPaymentinfo.PaymentMethod });
                    }
                    else
                    {
                        pi.CompanyId = currentLoggedIn.CompanyId.Value;
                        pi.BillMethod = MethodBilling.EFT;
                        _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(pi);
                        PaymentInfoCustomer objpayment = new PaymentInfoCustomer()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            CustomerId = pi.PaymentCustomerId,
                            PaymentInfoId = pi.Id
                        };
                        _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(objpayment);
                        var objRelationCustomerandPaymentinfo = _Util.Facade.CustomerFacade.GetLeadByPaymentinfoID(pi.Id);
                        if (objRelationCustomerandPaymentinfo != null)
                        {
                            objRelationCustomerandPaymentinfo.PaymentMethod = pi.BillMethod;
                            objRelationCustomerandPaymentinfo.LastUpdatedBy = User.Identity.Name;
                            objRelationCustomerandPaymentinfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            _Util.Facade.CustomerFacade.UpdateCustomer(objRelationCustomerandPaymentinfo);
                        }

                        return Json(new { result = true, LeadMethodBill = objRelationCustomerandPaymentinfo.PaymentMethod });
                    }
                }
            }
            #endregion

            #region Payment Method Type Credit Card
            if (pi2.BillMethod == "Credit Card")
            {
                if (pi2.AccountName != null || pi2.AcountNo != null || pi2.BankAccountType != null || pi2.CardExpireDate != null || pi2.CardNumber != null || pi2.CardSecurityCode != null || pi2.CardType != null || pi2.RoutingNo != null)
                {
                    if (pi2.Id > 0)
                    {
                        pi2.CompanyId = currentLoggedIn.CompanyId.Value;

                        pi2.BillMethod = MethodBilling.CreditCard;
                        _Util.Facade.PaymentInfoFacade.UpdatePaymentInfo(pi2);
                        PaymentInfoCustomer objpayment = new PaymentInfoCustomer()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            CustomerId = pi2.PaymentCustomerId,
                            PaymentInfoId = pi2.Id
                        };
                        _Util.Facade.PaymentInfoCustomerFacade.UpdatePaymentInfoCustomer(objpayment);
                        var objRelationCustomerandPaymentinfo = _Util.Facade.CustomerFacade.GetLeadByPaymentinfoID(pi2.Id);
                        if (objRelationCustomerandPaymentinfo != null)
                        {
                            objRelationCustomerandPaymentinfo.PaymentMethod = pi2.BillMethod;
                            objRelationCustomerandPaymentinfo.LastUpdatedBy = User.Identity.Name;
                            objRelationCustomerandPaymentinfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            _Util.Facade.CustomerFacade.UpdateCustomer(objRelationCustomerandPaymentinfo);
                        }

                        return Json(new { result = true, LeadMethodBill = objRelationCustomerandPaymentinfo.PaymentMethod });
                    }
                    else
                    {
                        pi2.CompanyId = currentLoggedIn.CompanyId.Value;
                        pi2.BillMethod = MethodBilling.CreditCard;
                        _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(pi2);
                        PaymentInfoCustomer objpayment = new PaymentInfoCustomer()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            CustomerId = pi2.PaymentCustomerId,
                            PaymentInfoId = pi2.Id
                        };
                        _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(objpayment);
                        var objRelationCustomerandPaymentinfo = _Util.Facade.CustomerFacade.GetLeadByPaymentinfoID(pi2.Id);
                        if (objRelationCustomerandPaymentinfo != null)
                        {
                            objRelationCustomerandPaymentinfo.PaymentMethod = pi2.BillMethod;
                            objRelationCustomerandPaymentinfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            objRelationCustomerandPaymentinfo.LastUpdatedBy = User.Identity.Name;
                            _Util.Facade.CustomerFacade.UpdateCustomer(objRelationCustomerandPaymentinfo);
                        }

                        return Json(new { result = true, LeadMethodBill = objRelationCustomerandPaymentinfo.PaymentMethod });
                    }
                }
            }
            #endregion

            #region Payment Method Type Debit Card

            if (pi1.BillMethod == "Debit Card")
            {
                if (pi1.AccountName != null || pi1.AcountNo != null || pi1.BankAccountType != null || pi1.CardExpireDate != null || pi1.CardSecurityCode != null || pi1.CardType != null || pi1.RoutingNo != null)
                {
                    if (pi1.Id > 0)
                    {
                        pi1.CompanyId = currentLoggedIn.CompanyId.Value;
                        pi1.BillMethod = MethodBilling.DebitCard;
                        _Util.Facade.PaymentInfoFacade.UpdatePaymentInfo(pi1);
                        PaymentInfoCustomer objpayment = new PaymentInfoCustomer()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            CustomerId = pi1.PaymentCustomerId,
                            PaymentInfoId = pi1.Id
                        };
                        _Util.Facade.PaymentInfoCustomerFacade.UpdatePaymentInfoCustomer(objpayment);
                        var objRelationCustomerandPaymentinfo = _Util.Facade.CustomerFacade.GetLeadByPaymentinfoID(pi1.Id);
                        if (objRelationCustomerandPaymentinfo != null)
                        {
                            objRelationCustomerandPaymentinfo.PaymentMethod = pi1.BillMethod;
                            objRelationCustomerandPaymentinfo.LastUpdatedBy = User.Identity.Name;
                            objRelationCustomerandPaymentinfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            _Util.Facade.CustomerFacade.UpdateCustomer(objRelationCustomerandPaymentinfo);
                        }

                        return Json(new { result = true, LeadMethodBill = objRelationCustomerandPaymentinfo.PaymentMethod });
                    }
                    else
                    {
                        pi1.CompanyId = currentLoggedIn.CompanyId.Value;
                        pi1.BillMethod = MethodBilling.DebitCard;
                        _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(pi1); //insert into paymentInfo
                        PaymentInfoCustomer objpayment = new PaymentInfoCustomer()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            CustomerId = pi1.PaymentCustomerId,
                            PaymentInfoId = pi1.Id
                        };
                        _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(objpayment); //insert into PaymentInfoCustomer
                        var objRelationCustomerandPaymentinfo = _Util.Facade.CustomerFacade.GetLeadByPaymentinfoID(pi1.Id);
                        if (objRelationCustomerandPaymentinfo != null)
                        {
                            objRelationCustomerandPaymentinfo.PaymentMethod = pi1.BillMethod;
                            objRelationCustomerandPaymentinfo.LastUpdatedBy = User.Identity.Name;
                            objRelationCustomerandPaymentinfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            _Util.Facade.CustomerFacade.UpdateCustomer(objRelationCustomerandPaymentinfo);
                        }

                        return Json(new { result = true, LeadMethodBill = objRelationCustomerandPaymentinfo.PaymentMethod });
                    }
                }
            }

            #endregion

            #region Payment Method Type Cash
            if (PaymentCash.BillMethod != null)
            {
                if (PaymentCash.BillMethod.ToLower() == "cash")
                {
                    if (PaymentCash.PaymentCustomerId != new Guid())
                    {
                        #region Edit cash payment method
                        if (PaymentCash.Id > 0)
                        {
                            var PaymentInfoDb = _Util.Facade.PaymentInfoFacade.GetPaymentInfoByIdAndCompanyId(PaymentCash.Id, currentLoggedIn.CompanyId.Value);
                            if (PaymentInfoDb != null)
                            {
                                PaymentInfoDb.AccountName = "";
                                PaymentInfoDb.BankAccountType = "";
                                PaymentInfoDb.RoutingNo = "";
                                PaymentInfoDb.AcountNo = "";
                                PaymentInfoDb.CardType = "";
                                PaymentInfoDb.CardNumber = "";
                                PaymentInfoDb.CardExpireDate = "";
                                PaymentInfoDb.CardSecurityCode = "";
                                PaymentInfoDb.CheckNo = "";
                                PaymentInfoDb.IsCash = true;

                                result = _Util.Facade.PaymentInfoFacade.UpdatePaymentInfo(PaymentInfoDb);

                                if (result)
                                {
                                    var CustomerDbvalue = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(PaymentCash.PaymentCustomerId);
                                    if (CustomerDbvalue != null)
                                    {
                                        CustomerDbvalue.PaymentMethod = PaymentCash.BillMethod;
                                        CustomerDbvalue.LastUpdatedBy = User.Identity.Name;
                                        CustomerDbvalue.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                        CustomerDbvalue.IsActive = true;
                                        result = _Util.Facade.CustomerFacade.UpdateCustomer(CustomerDbvalue);
                                    }
                                }
                            }
                        }
                        #endregion
                        #region Insert cash payment method
                        else
                        {
                            PaymentCash.CompanyId = currentLoggedIn.CompanyId.Value;
                            PaymentCash.IsCash = true;
                            result = _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(PaymentCash) > 0;
                            if (result)
                            {
                                PaymentInfoCustomer DBPaymentInfoCustomer = new PaymentInfoCustomer()
                                {
                                    CompanyId = currentLoggedIn.CompanyId.Value,
                                    CustomerId = PaymentCash.PaymentCustomerId,
                                    PaymentInfoId = PaymentCash.Id
                                };
                                result = _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(DBPaymentInfoCustomer) > 0;

                                var CustomerInfo = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(PaymentCash.PaymentCustomerId);
                                if (CustomerInfo != null)
                                {
                                    CustomerInfo.PaymentMethod = PaymentCash.BillMethod;
                                    CustomerInfo.LastUpdatedBy = User.Identity.Name;
                                    CustomerInfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                    CustomerInfo.IsActive = true;
                                    result = _Util.Facade.CustomerFacade.UpdateCustomer(CustomerInfo);
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            #endregion

            #region Payment Method Type Check
            if (PaymentCheck.BillMethod != null)
            {
                if (PaymentCheck.BillMethod.ToLower() == "check")
                {
                    if (PaymentCheck.CheckNo != "" && PaymentCheck.PaymentCustomerId != new Guid())
                    {
                        #region Edit Check Payment Method
                        if (PaymentCheck.Id > 0)
                        {
                            var PaymentInfoDBData = _Util.Facade.PaymentInfoFacade.GetPaymentInfoByIdAndCompanyId(PaymentCheck.Id, currentLoggedIn.CompanyId.Value);
                            if (PaymentInfoDBData != null)
                            {

                                PaymentInfoDBData.AccountName = "";
                                PaymentInfoDBData.BankAccountType = "";
                                PaymentInfoDBData.RoutingNo = "";
                                PaymentInfoDBData.AcountNo = "";
                                PaymentInfoDBData.CardType = "";
                                PaymentInfoDBData.CardNumber = "";
                                PaymentInfoDBData.CardExpireDate = "";
                                PaymentInfoDBData.CardSecurityCode = "";
                                PaymentInfoDBData.CheckNo = PaymentCheck.CheckNo;
                                PaymentInfoDBData.IsCash = false;
                                result = _Util.Facade.PaymentInfoFacade.UpdatePaymentInfo(PaymentInfoDBData);
                                if (result)
                                {
                                    var CustomerInfo = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(PaymentCheck.PaymentCustomerId);
                                    if (CustomerInfo != null)
                                    {
                                        CustomerInfo.PaymentMethod = PaymentCheck.BillMethod;
                                        CustomerInfo.LastUpdatedBy = User.Identity.Name;
                                        CustomerInfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                        result = _Util.Facade.CustomerFacade.UpdateCustomer(CustomerInfo);
                                    }
                                }
                            }
                        }
                        #endregion
                        #region Insert Check Payment Method
                        else
                        {
                            PaymentCheck.CompanyId = currentLoggedIn.CompanyId.Value;
                            result = _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(PaymentCheck) > 0;
                            if (result)
                            {
                                PaymentInfoCustomer DBPaymentInfoCustomer = new PaymentInfoCustomer()
                                {
                                    CompanyId = currentLoggedIn.CompanyId.Value,
                                    CustomerId = PaymentCheck.PaymentCustomerId,
                                    PaymentInfoId = PaymentCheck.Id
                                };
                                result = _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(DBPaymentInfoCustomer) > 0;

                                var CustomerInfo = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(PaymentCheck.PaymentCustomerId);
                                if (CustomerInfo != null)
                                {
                                    CustomerInfo.PaymentMethod = PaymentCheck.BillMethod;
                                    CustomerInfo.LastUpdatedBy = User.Identity.Name;
                                    CustomerInfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                    result = _Util.Facade.CustomerFacade.UpdateCustomer(CustomerInfo);
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            #endregion

            #endregion

            #region Service Setups
            if (CUS.CustomerModel != null)
            {
                #region update customer informations
                if (CUS.CustomerModel.ContractTeam != null)// && CUS.CustomerModel.PaymentMethod != null)
                {
                    var LeadID = CUS.CustomerModel.Id;
                    if (LeadID > 0)
                    {
                        var LeadInfo = _Util.Facade.CustomerFacade.GetCustomersById(LeadID);
                        if (LeadInfo != null)
                        {
                            LeadInfo.ContractTeam = CUS.CustomerModel.ContractTeam;
                            //LeadInfo.PaymentMethod = CUS.CustomerModel.PaymentMethod;
                            LeadInfo.Passcode = CUS.CustomerModel.Passcode;
                            LeadInfo.BillDay = CUS.CustomerModel.BillDay;
                            LeadInfo.MonthlyMonitoringFee = CUS.CustomerModel.MonthlyMonitoringFee;
                            LeadInfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            LeadInfo.LastUpdatedBy = User.Identity.Name;
                            LeadInfo.FirstBilling = CUS.CustomerModel.FirstBilling;
                            LeadInfo.ActivationFee = CUS.CustomerModel.ActivationFee;
                            //LeadInfo.ActivationFeePaymentMethod = CUS.CustomerModel.ActivationFeePaymentMethod;
                            result = _Util.Facade.CustomerFacade.UpdateCustomer(LeadInfo);
                        }
                    }
                }
                #endregion

                //#region insert MMR payment method 
                //if (CUS.MMRPaymentInfoModel != null)
                //{
                //    if (CUS.MMRPaymentInfoModel.Id > 0)
                //    {
                //        return Json(result);
                //    }
                //    else
                //    {
                //        var MMRPaymentMethod = CUS.MMRPaymentInfoModel.BillMethod;

                //        #region Insert ACH
                //        if (MMRPaymentMethod == "ACH")
                //        {
                //            PaymentInfo DbACHPaymentInfo = new PaymentInfo
                //            {
                //                CompanyId = currentLoggedIn.CompanyId.Value,
                //                AccountName = CUS.MMRPaymentInfoModel.AccountName,
                //                BankAccountType = CUS.MMRPaymentInfoModel.BankAccountType,
                //                RoutingNo = CUS.MMRPaymentInfoModel.RoutingNo,
                //                AcountNo = CUS.MMRPaymentInfoModel.AccountName,
                //                CardType = "",
                //                CardNumber = "",
                //                CardExpireDate = "",
                //                CardSecurityCode = "",
                //                CheckNo = "",
                //                IsCash = false
                //            };

                //            bool IsACHPaymentInfoInsert = _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(DbACHPaymentInfo) > 0;
                //            if (IsACHPaymentInfoInsert)
                //            {
                //                PaymentInfoCustomer DbPaymentInfoCustomer = new PaymentInfoCustomer()
                //                {
                //                    CompanyId = currentLoggedIn.CompanyId.Value,
                //                    CustomerId = CUS.MMRPaymentInfoModel.PaymentCustomerId,
                //                    PaymentInfoId = DbACHPaymentInfo.Id,
                //                    Type = "MMR"
                //                };
                //                result = _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(DbPaymentInfoCustomer) > 0;
                //            }
                //        }
                //        #endregion

                //        #region Insert Check
                //        if (MMRPaymentMethod == "Check")
                //        {
                //            PaymentInfo DbCheckPaymentInfo = new PaymentInfo
                //            {
                //                CompanyId = currentLoggedIn.CompanyId.Value,
                //                AccountName = "",
                //                BankAccountType = "",
                //                RoutingNo = "",
                //                AcountNo = "",
                //                CardType = "",
                //                CardNumber = "",
                //                CardExpireDate = "",
                //                CardSecurityCode = "",
                //                CheckNo = CUS.MMRPaymentInfoModel.CheckNo,
                //                IsCash = false
                //            };

                //            bool IsCheckPaymentInfoInsert = _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(DbCheckPaymentInfo) > 0;
                //            if (IsCheckPaymentInfoInsert)
                //            {
                //                PaymentInfoCustomer DbPaymentInfoCustomer = new PaymentInfoCustomer()
                //                {
                //                    CompanyId = currentLoggedIn.CompanyId.Value,
                //                    CustomerId = CUS.MMRPaymentInfoModel.PaymentCustomerId,
                //                    PaymentInfoId = DbCheckPaymentInfo.Id,
                //                    Type = "MMR"
                //                };
                //                result = _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(DbPaymentInfoCustomer) > 0;
                //            }
                //        }
                //        #endregion

                //        #region Insert Credit Card
                //        if (MMRPaymentMethod == "Credit Card")
                //        {
                //            PaymentInfo DbCreditCardPaymentInfo = new PaymentInfo
                //            {
                //                CompanyId = currentLoggedIn.CompanyId.Value,
                //                AccountName = CUS.MMRPaymentInfoModel.AccountName,
                //                BankAccountType = "",
                //                RoutingNo = "",
                //                AcountNo = "",
                //                CardType = CUS.MMRPaymentInfoModel.CardType,
                //                CardNumber = CUS.MMRPaymentInfoModel.CardNumber,
                //                CardExpireDate = CUS.MMRPaymentInfoModel.CardExpireDate,
                //                CardSecurityCode = CUS.MMRPaymentInfoModel.CardSecurityCode,
                //                CheckNo = "",
                //                IsCash = false
                //            };

                //            bool IsCreditCardPaymentInfoInsert = _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(DbCreditCardPaymentInfo) > 0;
                //            if (IsCreditCardPaymentInfoInsert)
                //            {
                //                PaymentInfoCustomer DbPaymentInfoCustomer = new PaymentInfoCustomer()
                //                {
                //                    CompanyId = currentLoggedIn.CompanyId.Value,
                //                    CustomerId = CUS.MMRPaymentInfoModel.PaymentCustomerId,
                //                    PaymentInfoId = DbCreditCardPaymentInfo.Id,
                //                    Type = "MMR"
                //                };
                //                result = _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(DbPaymentInfoCustomer) > 0;
                //            }
                //        }
                //        #endregion

                //        #region Insert Debit Card
                //        if (MMRPaymentMethod == "Debit Card")
                //        {
                //            PaymentInfo DbDebitCardPaymentInfo = new PaymentInfo
                //            {
                //                CompanyId = currentLoggedIn.CompanyId.Value,
                //                AccountName = CUS.MMRPaymentInfoModel.AccountName,
                //                BankAccountType = "",
                //                RoutingNo = "",
                //                AcountNo = "",
                //                CardType = CUS.MMRPaymentInfoModel.CardType,
                //                CardNumber = CUS.MMRPaymentInfoModel.CardNumber,
                //                CardExpireDate = CUS.MMRPaymentInfoModel.CardExpireDate,
                //                CardSecurityCode = CUS.MMRPaymentInfoModel.CardSecurityCode,
                //                CheckNo = "",
                //                IsCash = false
                //            };

                //            bool IsDebitCardPaymentInfoInsert = _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(DbDebitCardPaymentInfo) > 0;
                //            if (IsDebitCardPaymentInfoInsert)
                //            {
                //                PaymentInfoCustomer DbPaymentInfoCustomer = new PaymentInfoCustomer()
                //                {
                //                    CompanyId = currentLoggedIn.CompanyId.Value,
                //                    CustomerId = CUS.MMRPaymentInfoModel.PaymentCustomerId,
                //                    PaymentInfoId = DbDebitCardPaymentInfo.Id,
                //                    Type = "MMR"
                //                };
                //                result = _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(DbPaymentInfoCustomer) > 0;
                //            }
                //        }
                //        #endregion

                //    }
                //}
                //#endregion

                //#region Insert Activation Fee Payment Method 
                //if (CUS.ActivationFeePaymentInfoModel != null)
                //{
                //    if (CUS.ActivationFeePaymentInfoModel.Id > 0)
                //    {
                //        return Json(result);
                //    }
                //    else
                //    {
                //        var AFPaymentMethod = CUS.ActivationFeePaymentInfoModel.BillMethod;

                //        #region Insert ACH
                //        if (AFPaymentMethod == "ACH")
                //        {
                //            PaymentInfo DbACHPaymentInfo = new PaymentInfo
                //            {
                //                CompanyId = currentLoggedIn.CompanyId.Value,
                //                AccountName = CUS.ActivationFeePaymentInfoModel.AccountName,
                //                BankAccountType = CUS.ActivationFeePaymentInfoModel.BankAccountType,
                //                RoutingNo = CUS.ActivationFeePaymentInfoModel.RoutingNo,
                //                AcountNo = CUS.ActivationFeePaymentInfoModel.AccountName,
                //                CardType = "",
                //                CardNumber = "",
                //                CardExpireDate = "",
                //                CardSecurityCode = "",
                //                CheckNo = "",
                //                IsCash = false
                //            };

                //            bool IsACHPaymentInfoInsert = _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(DbACHPaymentInfo) > 0;
                //            if (IsACHPaymentInfoInsert)
                //            {
                //                PaymentInfoCustomer DbPaymentInfoCustomer = new PaymentInfoCustomer()
                //                {
                //                    CompanyId = currentLoggedIn.CompanyId.Value,
                //                    CustomerId = CUS.ActivationFeePaymentInfoModel.PaymentCustomerId,
                //                    PaymentInfoId = DbACHPaymentInfo.Id,
                //                    Type = "AF"
                //                };
                //                result = _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(DbPaymentInfoCustomer) > 0;
                //            }
                //        }
                //        #endregion

                //        #region Insert Check
                //        if (AFPaymentMethod == "Check")
                //        {
                //            PaymentInfo DbCheckPaymentInfo = new PaymentInfo
                //            {
                //                CompanyId = currentLoggedIn.CompanyId.Value,
                //                AccountName = "",
                //                BankAccountType = "",
                //                RoutingNo = "",
                //                AcountNo = "",
                //                CardType = "",
                //                CardNumber = "",
                //                CardExpireDate = "",
                //                CardSecurityCode = "",
                //                CheckNo = CUS.ActivationFeePaymentInfoModel.CheckNo,
                //                IsCash = false
                //            };

                //            bool IsCheckPaymentInfoInsert = _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(DbCheckPaymentInfo) > 0;
                //            if (IsCheckPaymentInfoInsert)
                //            {
                //                PaymentInfoCustomer DbPaymentInfoCustomer = new PaymentInfoCustomer()
                //                {
                //                    CompanyId = currentLoggedIn.CompanyId.Value,
                //                    CustomerId = CUS.ActivationFeePaymentInfoModel.PaymentCustomerId,
                //                    PaymentInfoId = DbCheckPaymentInfo.Id,
                //                    Type = "AF"
                //                };
                //                result = _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(DbPaymentInfoCustomer) > 0;
                //            }
                //        }
                //        #endregion

                //        #region Insert Credit Card
                //        if (AFPaymentMethod == "Credit Card")
                //        {
                //            PaymentInfo DbCreditCardPaymentInfo = new PaymentInfo
                //            {
                //                CompanyId = currentLoggedIn.CompanyId.Value,
                //                AccountName = CUS.ActivationFeePaymentInfoModel.AccountName,
                //                BankAccountType = "",
                //                RoutingNo = "",
                //                AcountNo = "",
                //                CardType = CUS.ActivationFeePaymentInfoModel.CardType,
                //                CardNumber = CUS.ActivationFeePaymentInfoModel.CardNumber,
                //                CardExpireDate = CUS.ActivationFeePaymentInfoModel.CardExpireDate,
                //                CardSecurityCode = CUS.ActivationFeePaymentInfoModel.CardSecurityCode,
                //                CheckNo = "",
                //                IsCash = false
                //            };

                //            bool IsCreditCardPaymentInfoInsert = _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(DbCreditCardPaymentInfo) > 0;
                //            if (IsCreditCardPaymentInfoInsert)
                //            {
                //                PaymentInfoCustomer DbPaymentInfoCustomer = new PaymentInfoCustomer()
                //                {
                //                    CompanyId = currentLoggedIn.CompanyId.Value,
                //                    CustomerId = CUS.ActivationFeePaymentInfoModel.PaymentCustomerId,
                //                    PaymentInfoId = DbCreditCardPaymentInfo.Id,
                //                    Type = "AF"
                //                };
                //                result = _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(DbPaymentInfoCustomer) > 0;
                //            }
                //        }
                //        #endregion

                //        #region Insert Debit Card
                //        if (AFPaymentMethod == "Debit Card")
                //        {
                //            PaymentInfo DbDebitCardPaymentInfo = new PaymentInfo
                //            {
                //                CompanyId = currentLoggedIn.CompanyId.Value,
                //                AccountName = CUS.ActivationFeePaymentInfoModel.AccountName,
                //                BankAccountType = "",
                //                RoutingNo = "",
                //                AcountNo = "",
                //                CardType = CUS.ActivationFeePaymentInfoModel.CardType,
                //                CardNumber = CUS.ActivationFeePaymentInfoModel.CardNumber,
                //                CardExpireDate = CUS.ActivationFeePaymentInfoModel.CardExpireDate,
                //                CardSecurityCode = CUS.ActivationFeePaymentInfoModel.CardSecurityCode,
                //                CheckNo = "",
                //                IsCash = false
                //            };

                //            bool IsDebitCardPaymentInfoInsert = _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(DbDebitCardPaymentInfo) > 0;
                //            if (IsDebitCardPaymentInfoInsert)
                //            {
                //                PaymentInfoCustomer DbPaymentInfoCustomer = new PaymentInfoCustomer()
                //                {
                //                    CompanyId = currentLoggedIn.CompanyId.Value,
                //                    CustomerId = CUS.ActivationFeePaymentInfoModel.PaymentCustomerId,
                //                    PaymentInfoId = DbDebitCardPaymentInfo.Id,
                //                    Type = "AF"
                //                };
                //                result = _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(DbPaymentInfoCustomer) > 0;
                //            }
                //        }
                //        #endregion

                //    }
                //}
                //#endregion

                return Json(result);
            }

            #endregion
            else
            {
                return Json(false);
            }

        }

        [Authorize]
        [HttpPost]
        public JsonResult AddPaymentProfileWithInfo(PaymentInfo PaymentInfo, Guid customerid, string paymentmethod)
        {
            bool result = false;
            string methodtype = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            PaymentInfo.CompanyId = currentLoggedIn.CompanyId.Value;
            if (!string.IsNullOrWhiteSpace(paymentmethod) && paymentmethod == "ACH")
            {
                methodtype = paymentmethod + "_" + PaymentInfo.AccountName + "_" + PaymentInfo.AcountNo.Substring(Math.Max(0, PaymentInfo.AcountNo.Length - 2));
            }
            else
            {
                methodtype = "CC" + "_" + PaymentInfo.AccountName + "_" + PaymentInfo.CardNumber.Substring(Math.Max(0, PaymentInfo.CardNumber.Length - 4));
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (PaymentInfo.Id > 0)
            {

            }
            else
            {
                PaymentInfo.Id = 0;
                result = (int)_Util.Facade.PaymentInfoFacade.InsertPaymentInfo(PaymentInfo) > 0;
            }
            if (result)
            {
                PaymentProfileCustomer PaymentProfileCustomer = new PaymentProfileCustomer()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    CustomerId = customerid,
                    PaymentInfoId = PaymentInfo.Id,
                    Type = methodtype
                };
                _Util.Facade.CustomerFacade.InsertPaymentProfileCustomer(PaymentProfileCustomer);
            }
            return Json(result);
        }


        [Authorize]
        [HttpPost]
        public JsonResult SavePaymentInfoCustomer(Guid CustomerId, int PaymentInfoId, string Type, string Payfor, int? ForMonths)
        {
            bool IsACH = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (CustomerId == Guid.Empty)
            {
                return Json(new { result = false, message = "Customer not found." });
            }
            if (PaymentInfoId < 1)
            {
                return Json(new { result = false, message = "Payment method not found." });
            }

            PaymentInfoCustomer PIC = _Util.Facade.PaymentInfoCustomerFacade.GetByCustomerIdAndPayfor(CustomerId, Payfor);
            if (PaymentInfoId == 9999999)
            {
                if (PIC != null)
                {
                    PIC.ForMonths = ForMonths;
                    _Util.Facade.PaymentInfoCustomerFacade.UpdatePaymentInfoCustomer(PIC);
                }
            }
            else
            {
                if (PIC != null && PIC.IsPaid.HasValue && PIC.IsPaid.Value)
                {
                    return Json(new { result = false, message = "Customer has already paid for {0}.", Payfor });
                }
                if (Payfor.ToLower() != "service")
                {
                    ForMonths = 0;
                }
                if (PIC == null)
                {
                    PIC = new PaymentInfoCustomer()
                    {
                        CustomerId = CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        Payfor = Payfor,
                        PaymentInfoId = PaymentInfoId,
                        Type = Type,
                        ForMonths = ForMonths
                    };
                    _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(PIC);
                }
                else
                {
                    PIC.PaymentInfoId = PaymentInfoId;
                    PIC.ForMonths = ForMonths;
                    _Util.Facade.PaymentInfoCustomerFacade.UpdatePaymentInfoCustomer(PIC);
                }
                if (PaymentInfoId > 0)
                {
                    var objpayprofile = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentProfileByPaymentInfoId(PaymentInfoId);
                    if (objpayprofile != null && objpayprofile.Type.ToLower().IndexOf("ach") > -1)
                    {
                        IsACH = true;
                        Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                        if (customer != null)
                        {
                            customer.IsACHDiscount = true;
                            _Util.Facade.CustomerFacade.UpdateCustomer(customer);
                        }
                    }
                    else
                    {
                        Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                        if (customer != null)
                        {
                            customer.IsACHDiscount = false;
                            _Util.Facade.CustomerFacade.UpdateCustomer(customer);
                        }
                    }
                }
            }
            return Json(new { result = true, message = "Saved successfully.", IsACH = IsACH });
        }


        [Authorize]
        [HttpPost]
        public JsonResult AddLeadPaymentInfo(string AddLeadPaymentInfo, string paymentfor, string PaymentMethod, Guid CustomerId, string FileName)
        {
            bool PaymentForMMr = false;
            bool PaymentForFirstMonth = false;
            bool PaymetForActivationFee = false;
            bool PaymentForEquipment = false;
            bool PaymentForService = false;
            if (!string.IsNullOrWhiteSpace(paymentfor))
            {
                if (paymentfor == "FirstMonth")
                {
                    PaymentForFirstMonth = true;
                }
                else if (paymentfor == "ActivationFee")
                {
                    PaymetForActivationFee = true;
                }
                else if (paymentfor == "Equipment")
                {
                    PaymentForEquipment = true;
                }
                else if (paymentfor == "Service")
                {
                    PaymentForService = true;
                }
            }
            if (PaymentForMMr == false && PaymentForFirstMonth == false && PaymentForEquipment == false && PaymentForService == false && PaymetForActivationFee == false)
            {
                return Json(new { result = false, message = "You have to choose at least 1 payment reason." });
            }
            PaymentInfo PaymentInfo = new PaymentInfo();
            PaymentProfileCustomer PaymentProfileCustomer = new PaymentProfileCustomer();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Customer Customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);

            if (Customer == null)
            {
                return Json(new { result = false, message = "Lead not found." });
            }
            if (!string.IsNullOrWhiteSpace(AddLeadPaymentInfo))
            {
                PaymentInfo = _Util.Facade.PaymentInfoFacade.GetPaymentInfoById(Convert.ToInt32(AddLeadPaymentInfo));
                if (PaymentInfo != null)
                {
                    PaymentInfo.FileName = FileName;
                    _Util.Facade.PaymentInfoFacade.UpdatePaymentInfo(PaymentInfo);
                }

                PaymentProfileCustomer = _Util.Facade.CustomerFacade.GetProfileByPaymentInfoId(Convert.ToInt32(AddLeadPaymentInfo));
                if (PaymentProfileCustomer != null)
                {
                    var splitprofile = PaymentProfileCustomer.Type.Split('_');
                    if (splitprofile.Length > 0)
                    {
                        if (splitprofile[0] == "CC")
                        {
                            PaymentMethod = "Credit Card";
                        }
                        else
                        {
                            PaymentMethod = "ACH";
                        }
                    }
                }
                List<PaymentInfo> PaymentInfoList = _Util.Facade.PaymentInfoFacade.GetPaymentInfoListByCompanyIdAndCustomerId(CurrentUser.CompanyId.Value, Customer.CustomerId);
                if (PaymentForMMr)
                {
                    if (PaymentInfoList.Where(x => x.PayFor == "MMR").Count() > 0)
                    {
                        return Json(new { result = false, message = "Payment method already exists for MMR. Please delete previous one to add another." });
                    }


                    PaymentInfoCustomer pic = new PaymentInfoCustomer()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = Customer.CustomerId,
                        //Payfor = PaymentInfo.PayFor,
                        Type = PaymentMethod + " (" + PaymentProfileCustomer.Type + ")",
                        PaymentInfoId = Convert.ToInt32(AddLeadPaymentInfo),

                    };
                    pic.Payfor = "MMR";

                    Customer.PaymentMethod = PaymentMethod;
                    pic.Id = (int)_Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(pic);
                    _Util.Facade.CustomerFacade.UpdateCustomer(Customer);
                }
                if (PaymentForFirstMonth)
                {
                    if (PaymentInfoList.Where(x => x.PayFor == "First Month").Count() > 0)
                    {
                        return Json(new { result = false, message = "Payment method already exists for first month payment. Please delete previous one to add another." });
                    }


                    PaymentInfoCustomer pic = new PaymentInfoCustomer()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = Customer.CustomerId,
                        //Payfor = PaymentInfo.PayFor,
                        Type = PaymentMethod + " (" + PaymentProfileCustomer.Type + ")",
                        PaymentInfoId = Convert.ToInt32(AddLeadPaymentInfo),

                    };
                    pic.Payfor = "First Month";

                    pic.Id = (int)_Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(pic);
                }
                if (PaymetForActivationFee)
                {
                    if (PaymentInfoList.Where(x => x.PayFor == "Activation Fee").Count() > 0)
                    {
                        return Json(new { result = false, message = "Payment method already exists for activation fee payment. Please delete previous one to add another." });
                    }


                    PaymentInfoCustomer pic = new PaymentInfoCustomer()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = Customer.CustomerId,
                        //Payfor = PaymentInfo.PayFor,
                        Type = PaymentMethod + " (" + PaymentProfileCustomer.Type + ")",
                        PaymentInfoId = Convert.ToInt32(AddLeadPaymentInfo),

                    };
                    pic.Payfor = "Activation Fee";

                    Customer.ActivationFeePaymentMethod = PaymentMethod;
                    pic.Id = (int)_Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(pic);
                    _Util.Facade.CustomerFacade.UpdateCustomer(Customer);
                }
                if (PaymentForEquipment)
                {
                    if (PaymentInfoList.Where(x => x.PayFor == "Equipment").Count() > 0)
                    {
                        return Json(new { result = false, message = "Payment method already exists for equipment. Please delete previous one to add another." });
                    }



                    PaymentInfoCustomer pic = new PaymentInfoCustomer()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = Customer.CustomerId,
                        //Payfor = PaymentInfo.PayFor,
                        Type = PaymentMethod + " (" + PaymentProfileCustomer.Type + ")",
                        PaymentInfoId = Convert.ToInt32(AddLeadPaymentInfo),

                    };
                    pic.Payfor = "Equipment";

                    pic.Id = (int)_Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(pic);
                }
                if (PaymentForService)
                {
                    if (PaymentInfoList.Where(x => x.PayFor == "Service").Count() > 0)
                    {
                        return Json(new { result = false, message = "Payment method already exists for service. Please delete previous one to add another." });
                    }
                    PaymentInfoCustomer pic = new PaymentInfoCustomer()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = Customer.CustomerId,
                        //Payfor = PaymentInfo.PayFor,
                        Type = PaymentMethod + " (" + PaymentProfileCustomer.Type + ")",
                        PaymentInfoId = Convert.ToInt32(AddLeadPaymentInfo),

                    };
                    pic.Payfor = "Equipment";

                    pic.Id = (int)_Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(pic);
                }
            }
            return Json(new { result = true, message = "Payment info successfully added." });
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteLeadPaymentInfo(Guid CustomerId, int PaymentInfoId, int paymentinfoCusId)
        {
            if (CustomerId != new Guid() && PaymentInfoId > 0)
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                _Util.Facade.PaymentInfoCustomerFacade.DeleteByCustomerIdCompanyIdAndPaymentInfoId(CustomerId, CurrentUser.CompanyId.Value, PaymentInfoId, paymentinfoCusId);
                //_Util.Facade.PaymentInfoFacade.DeletePaymentInfoByIdAndCompanyId(PaymentInfoId, CurrentUser.CompanyId.Value);
                return Json(new { result = true, message = "Payment information deleted successfully." });
            }
            return Json(new { result = false, message = "And error occured. Please contact system admin." });
        }

        //[Authorize]
        //public PartialViewResult CustomerFiles(int id)
        //{
        //    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
        //    bool result = _Util.Facade.CustomerFacade.CustomerIsInCompany(id, CurrentUser.CompanyId.Value);
        //    if (!result)
        //    {
        //        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
        //    }
        //    Customer Customer = _Util.Facade.CustomerFacade.GetById(id);
        //    if (Customer == null)
        //    {
        //        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
        //    }
        //    List<CustomerFile> files = _Util.Facade.CustomerFileFacade.GetAllFilesByCustomerIdAndCompanyId(Customer.CustomerId, CurrentUser.CompanyId.Value);

        //    ViewBag.CustomerId = id;

        //    return PartialView("_CustomerFiles", files);
        //}

        [Authorize]
        public PartialViewResult LeadFilesAndDocument(int id, string soldby)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            //bool result = _Util.Facade.CustomerFacade.CustomerIsInCompany(id, CurrentUser.CompanyId.Value);
            //if (!result)
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //} 
            ViewBag.CustomerId = id;
            ViewBag.SoldBy = soldby;
            return PartialView("_LeadFilesAndDocument");
        }
        [Authorize]
        public ActionResult DocumentCenterPartial(int? id, string SoldBy, string SearchText)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            string LeadDetailCity = "";
            string LeadDetailState = "";
            string streettype = "";
            string LAppartment = "";
            List<CustomerFile> files = new List<CustomerFile>();
            if (id.HasValue)
            {
                Customer Customer = _Util.Facade.CustomerFacade.GetById(id.Value);
                if (Customer.City != "")
                {
                    LeadDetailCity = Customer.City + ", ";
                }
                if (Customer.State != "")
                {
                    LeadDetailState = Customer.State + " ";
                }
                if (Customer.StreetType != "-1")
                {
                    streettype = Customer.StreetType + " ";
                }
                if (!string.IsNullOrWhiteSpace(Customer.Appartment))
                {
                    LAppartment = "#" + Customer.Appartment;
                }
                ViewBag.LeadDetailAddress = AddressHelper.MakeAddress(Customer);
                if (Customer.Type == "Commercial")
                {
                    ViewBag.Name = Customer.BusinessName;
                }
                else
                {
                    ViewBag.Name = Customer.FirstName + " " + Customer.LastName;
                }
                files = _Util.Facade.CustomerFileFacade.GetAllFilesByCustomerIdAndCompanyId(Customer.CustomerId, CurrentUser.CompanyId.Value, SearchText);
                ViewBag.CustomerId = id;
                ViewBag.CustomerGuidId = Customer.CustomerId;
            }
            ViewBag.SoldBy = SoldBy;
            return PartialView("DocumentCenterPartial", files);
        }

        [Authorize]
        public ActionResult LeadTrackingPartial(string leadId)
        {

            ViewBag.leadId = leadId;
            return PartialView("LeadTrackingPartial");
        }



        [Authorize]
        public ActionResult LoadLeadTrackingPartial(LeadActivityListFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<LeadActivityViewModel> LeadActivityViewModel = new List<LeadActivityViewModel>();

            if (string.IsNullOrWhiteSpace(filter.leadId))
            {
                filter.leadId = new Guid().ToString();
            }

            if (filter.startDate == null)
            {
                filter.startDate = new DateTime(0001, 01, 01);
            }
            if (filter.endDate == new DateTime(0001, 01, 01))
            {
                filter.endDate = DateTime.Now;
            }
            else
            {
                TimeSpan ts = new TimeSpan(23, 59, 59);
                filter.endDate = filter.endDate + ts;
            }


            var client = new RestSharp.RestClient(AppConfig.SiteAPIDomain + "1.0/GetLeadActivity");
            var request = new RestSharp.RestRequest(RestSharp.Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("EndDate", filter.endDate.ToString());
            request.AddHeader("StartDate", filter.startDate.ToString());
            request.AddHeader("LeadId", filter.leadId);
            RestSharp.IRestResponse response = client.Execute(request);

            List<LeadActivity> LeadActivity2 = new List<LeadActivity>();

            try
            {
                LeadActivity2 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LeadActivity>>(response.Content);
            }
            catch
            {
                return PartialView("LoadLeadTrackingPartial", LeadActivityViewModel);
            }


            LeadActivity2 = LeadActivity2.OrderByDescending(x => x.StatsDate).ToList();

            if (LeadActivity2.Count > 0)
            {
                LeadActivityViewModel.Add(new LeadActivityViewModel());
                LeadActivityViewModel.LastOrDefault().LeadActivityList = new List<LeadActivity>();
                LeadActivityViewModel.LastOrDefault().LeadActivityList.Add(LeadActivity2[0]);

                for (int i = 1; i < LeadActivity2.Count; i++)
                {

                    if (LeadActivity2[i].StatsDate.Date == LeadActivity2[i - 1].StatsDate.Date)
                    {
                        LeadActivityViewModel.LastOrDefault().LeadActivityList.Add(LeadActivity2[i]);

                    }
                    else
                    {
                        LeadActivityViewModel.Add(new LeadActivityViewModel());
                        LeadActivityViewModel.LastOrDefault().LeadActivityList = new List<LeadActivity>();
                        LeadActivityViewModel.LastOrDefault().LeadActivityList.Add(LeadActivity2[i]);
                    }

                }

            }

            string[] browser_list = new string[] { "Edge", "opr", "Opera", "UCBrowser", "Outlook", "Chrome", "Firefox", "Safari", "Netscape", "MSIE", "rv:11.0" };


            List<Lookup> UserAgentLookUp = new List<Lookup>();
            UserAgentLookUp = _Util.Facade.LookupFacade.GetLookupByKey("UserAgent");


            TimeSpan t = new TimeSpan();



            foreach (var item in LeadActivityViewModel)
            {
                foreach (var item2 in item.LeadActivityList)
                {


                    t = TimeSpan.FromMilliseconds(item2.PassedTime);
                    item2.PassedTimeInMin = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                            t.Hours,
                                            t.Minutes,
                                            t.Seconds);
                    item2.StatsDateInPMAM = item2.StatsDate.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);

                    item2.PassedTime = item2.PassedTime - (int)t.Milliseconds;
                    //item2.PassedTime = ((int)t.Hours * 3600000) + ((int)t.Minutes * 60000) + ((int)t.Seconds * 1000);
                    item.TotalTimeInMilliseconds = item.TotalTimeInMilliseconds + item2.PassedTime;

                    foreach (var browser in UserAgentLookUp)
                    {
                        if (item2.UserAgent.ToLower().Contains(browser.DataValue.ToLower()))
                        {
                            item2.UserAgent = browser.DisplayText;
                            break;
                        }
                    }



                    //foreach (var browser in browser_list)
                    //{
                    //    if (item2.UserAgent.ToLower().Contains(browser.ToLower()))
                    //    {
                    //        item2.UserAgent = browser;
                    //        break;
                    //    }
                    //}

                    //if (item2.UserAgent.ToLower() == "opr")
                    //{
                    //    item2.UserAgent = "Opera";
                    //}
                    //else if (item2.UserAgent.ToLower() == "UCBrowser")
                    //{
                    //    item2.UserAgent = "UC Browser";
                    //}

                    //else if (item2.UserAgent.ToLower() == "msie" || item2.UserAgent.ToLower() == "rv:11.0")
                    //{
                    //    item2.UserAgent = "Internet Explorer";
                    //}


                }

                t = TimeSpan.FromMilliseconds(item.TotalTimeInMilliseconds);
                item.TotalTime = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                        t.Hours,
                                        t.Minutes,
                                        t.Seconds);


            }


            return PartialView("LoadLeadTrackingPartial", LeadActivityViewModel);
        }



        public ActionResult LoadLeadPackageEquipments(int? PackageId, int LeadId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            LeadPackageModelEquipmentList model = new LeadPackageModelEquipmentList();
            if (currentLoggedIn != null)
            {
                if (PackageId.HasValue && LeadId > 0)
                {
                    var LeadCustomerId = new Guid();
                    var LeadInformations = _Util.Facade.CustomerFacade.GetCustomersById(LeadId);
                    if (LeadInformations != null)
                    {
                        LeadCustomerId = LeadInformations.CustomerId;
                    }
                    var PackageDeviceList = _Util.Facade.PackageFacade.GetAllPackageDeviceEquipmentListByPackageIdAndCompanyIdAndCustomerId(PackageId.Value, currentLoggedIn.CompanyId.Value, LeadCustomerId);
                    var PackageIncludeList = _Util.Facade.PackageFacade.GetAllPackageIncludeEquipmentListByPackageIdAndCompanyId(PackageId.Value, currentLoggedIn.CompanyId.Value);
                    var PackageOptionalList = _Util.Facade.PackageFacade.GetAllPackageOptionalEquipmentListByPackageIdAndCompanyIdAndCustomerId(PackageId.Value, currentLoggedIn.CompanyId.Value, LeadCustomerId);

                    //var newGuidproduct = "00000000-0000-0000-0000-000000000000";
                    model.PackageDeviceEquipmentList = new List<LeadPackageEuipment>();
                    model.PackageIncludeEquipmentList = new List<LeadPackageEuipment>();
                    model.PackageOptionalEquipmentList = new List<LeadPackageEuipment>();
                    foreach (var item in PackageDeviceList)
                    {
                        if (item.PackageEquipmentid != Guid.Empty)
                        {
                            item.EquipmentIsFreeFlag = true;
                            model.PackageDeviceEquipmentList.Add(item);
                        }
                    }
                    foreach (var item in PackageIncludeList)
                    {
                        if (item.PackageEquipmentid != Guid.Empty)
                        {
                            item.EquipmentIsFreeFlag = true;
                            model.PackageIncludeEquipmentList.Add(item);
                        }
                    }

                    foreach (var item in PackageOptionalList)
                    {
                        if (item.PackageEquipmentid != Guid.Empty)
                        {
                            item.EquipmentIsFreeFlag = false;
                            model.PackageOptionalEquipmentList.Add(item);
                        }
                    }
                    var PackageDetails = _Util.Facade.PackageFacade.GetPackageByIdAndCompanyId(PackageId.Value, currentLoggedIn.CompanyId.Value);
                    //ChangePackage
                    if (PackageDetails != null)
                    {
                        model.PackageMMRRange = _Util.Facade.PackageFacade.GetMMrRangeByPackageId(PackageDetails.PackageId);
                    }
                    if (model.PackageMMRRange == null)
                    {
                        model.PackageMMRRange = new MMRRange() { };
                    }
                }
            }
            model.PackageMaxDeviceEquipmentLimit = _Util.Facade.PackageFacade.GetPackageOptionEqpMaxLimitByPackageIdAndCompanyId(PackageId.Value, currentLoggedIn.CompanyId.Value);
            return PartialView("_LoadLeadPackageEquipments", model);
        }

        public ActionResult LeadVerifyAndSetupDetailInformationPartial()
        {
            return View("LeadVerifyAndSetupDetailInformationPartial");
        }

        [Authorize]
        public ActionResult QA1QuestionariePartial(int? id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ListQuestionAnswer model = new ListQuestionAnswer();
            if (id.HasValue)
            {
                model.ListQa1Question = _Util.Facade.QAQuestionFacade.GetQa1QuestionByCompanyId(currentLoggedIn.CompanyId.Value);
            }

            ViewBag.LeadCustomerIdForQuestionaire = _Util.Facade.CustomerFacade.GetCustomersById(id.Value).CustomerId;
            ViewBag.LeadIDForQuestionarie = id.Value;
            model.ListQa1Answer = _Util.Facade.QaAnswerFacade.GetQa1QuestionaireByCompanyIdandCustomerId(currentLoggedIn.CompanyId.Value, ViewBag.LeadCustomerIdForQuestionaire);
            return View("QA1QuestionariePartial", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddNewQuestionaire(string LeadCustomerId, List<ListOfQaAnswers> QaAnswersList, int LeadId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (QaAnswersList != null)
            {
                var CheckQAExist = _Util.Facade.QaAnswerFacade.GetQa1QuestionaireByCompanyIdandCustomerId(currentLoggedIn.CompanyId.Value, new Guid(LeadCustomerId));
                if (CheckQAExist != null)
                {
                    var Answerobj = _Util.Facade.QaAnswerFacade.DeleteQa1AnswerByCustomerIdAndComapnyId(new Guid(LeadCustomerId), currentLoggedIn.CompanyId.Value);
                }
                foreach (var Item in QaAnswersList)
                {
                    var Boolval = Convert.ToBoolean(Item.SelectedAnswer);

                    QaAnswer _QaAnswer = new QaAnswer()
                    {
                        CompanyId = currentLoggedIn.CompanyId.Value,
                        CustomerId = new Guid(LeadCustomerId),
                        QuestionId = Item.SelectedQuesid,
                        Answer = Item.SelectedAnswer
                    };
                    _Util.Facade.QaAnswerFacade.InsertQaAnswer(_QaAnswer);

                }
                //var LeadobjName = _Util.Facade.CustomerFacade.GetLeadNameByLeadId(LeadId).LeadName;
                //var ScheduleObj = _Util.Facade.ScheduleFacade.GetByQA1Id(LeadId);
                //if (ScheduleObj != null)
                //{
                //    var ScheduleId = ScheduleObj.Id;
                //    Schedule modelSchedule = new Schedule();
                //    modelSchedule.Id = ScheduleId;
                //    if (modelSchedule.Id > 0)
                //    {
                //        modelSchedule.CompanyId = currentLoggedIn.CompanyId.Value;
                //        modelSchedule.Type = ScheduleType.QA1;
                //        modelSchedule.StartDate = (DateTime.Now.UTCCurrentTime()).AddDays(1);
                //        modelSchedule.EndDate = (DateTime.Now.UTCCurrentTime()).AddDays(2);
                //        modelSchedule.Title = ScheduleTitle.QA1Required + " " + LeadobjName;
                //        modelSchedule.LeadId = LeadId;
                //        modelSchedule.IsCompleted = true;
                //        _Util.Facade.ScheduleFacade.UpdateSchedule(modelSchedule);
                //    }
                //}
            }

            return Json(new { result = true, LeadQuestionaireId = LeadId });
        }

        public ActionResult QA2QuestionariePartial(int? id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ListQuestionAnswer model = new ListQuestionAnswer();
            if (id.HasValue)
            {
                model.ListQa2Question = _Util.Facade.QAQuestionFacade.GetQa2QuestionByCompanyId(currentLoggedIn.CompanyId.Value);
            }
            ViewBag.LeadCustomerIdForQuestionaire = _Util.Facade.CustomerFacade.GetCustomersById(id.Value).CustomerId;
            ViewBag.LeadIDForQuestionarie = id.Value;
            model.ListQa2Answer = _Util.Facade.QaAnswerFacade.GetQa2QuestionaireByCompanyIdandCustomerId(currentLoggedIn.CompanyId.Value, ViewBag.LeadCustomerIdForQuestionaire);
            return View("QA2QuestionariePartial", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddNewQuestionaire2(string LeadCustomerId, List<ListOfQaAnswers> QaAnswersList, int LeadId1)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (QaAnswersList != null)
            {
                var CheckQAExist = _Util.Facade.QaAnswerFacade.GetQa2QuestionaireByCompanyIdandCustomerId(currentLoggedIn.CompanyId.Value, new Guid(LeadCustomerId));
                if (CheckQAExist != null)
                {
                    var Answerobj = _Util.Facade.QaAnswerFacade.DeleteQa2AnswerByCustomerIdAndComapnyId(new Guid(LeadCustomerId), currentLoggedIn.CompanyId.Value);
                }
                foreach (var Item in QaAnswersList)
                {
                    var Boolval = Convert.ToBoolean(Item.SelectedAnswer);

                    QaAnswer _QaAnswer = new QaAnswer()
                    {
                        CompanyId = currentLoggedIn.CompanyId.Value,
                        CustomerId = new Guid(LeadCustomerId),
                        QuestionId = Item.SelectedQuesid,
                        Answer = Item.SelectedAnswer
                    };
                    _Util.Facade.QaAnswerFacade.InsertQaAnswer(_QaAnswer);
                }
                var LeadobjName = _Util.Facade.CustomerFacade.GetLeadNameByLeadId(LeadId1).LeadName;
                var ScheduleId = _Util.Facade.ScheduleFacade.GetByQA2Id(LeadId1).Id;
                Schedule modelSchedule = new Schedule();
                modelSchedule.Id = ScheduleId;
                if (modelSchedule.Id > 0)
                {
                    modelSchedule.CompanyId = currentLoggedIn.CompanyId.Value;
                    modelSchedule.Type = ScheduleType.QA2;
                    modelSchedule.StartDate = (DateTime.Now.UTCCurrentTime()).AddDays(1);
                    modelSchedule.EndDate = (DateTime.Now.UTCCurrentTime()).AddDays(2);
                    modelSchedule.Title = ScheduleTitle.QA2Required + " " + LeadobjName;
                    modelSchedule.LeadId = LeadId1;
                    modelSchedule.IsCompleted = true;
                    _Util.Facade.ScheduleFacade.UpdateSchedule(modelSchedule);
                }
            }

            return Json(new { result = true, LeadQuestionaireId = LeadId1 });
        }
        public ActionResult LeadTechCallCalendar(string parent)
        {
            if (!string.IsNullOrWhiteSpace(parent))
            {
                parent = "." + parent;
                ViewBag.Parent = parent;
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<CustomerAppointment> appointment = _Util.Facade.CustomerAppoinmentFacade.GetAllWorkOrderByCompanyId(currentLoggedIn.CompanyId.Value);
            return PartialView("LeadTechCallCalendar", appointment);
        }

        public ActionResult LeadTechCallPartial()
        {
            return View("LeadTechCallPartial");
        }

        [Authorize]
        public ActionResult LeadTechScheduleCalendar()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<LeadTechScheduleCalendar> Calmodel = new List<LeadTechScheduleCalendar>();
            Calmodel = _Util.Facade.TechScheduleFacade.GetAllLeadTechScheduleByCompanyid(currentLoggedIn.CompanyId.Value);
            return View("LeadTechScheduleCalendar", Calmodel);
        }

        [Authorize]
        [HttpPost]
        public JsonResult LeadTechScheduleCalendarDisplay()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var objLeadSchedule = _Util.Facade.TechScheduleFacade.GetAllLeadTechScheduleByCompanyid(currentLoggedIn.CompanyId.Value);
            return Json(new { result = objLeadSchedule });
        }

        [Authorize]
        public ActionResult AddLeadTechSchedule(int? id, int? Leadid)
        {
            TechSchedule model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (id.HasValue)
            {
                model = _Util.Facade.TechScheduleFacade.GetTechScheduleById(id.Value);
            }
            else
            {
                model = new TechSchedule();
            }
            ViewBag.leadCustomerid = _Util.Facade.CustomerFacade.GetLeadIdByCompanyIdAndCustomerId(currentLoggedIn.CompanyId.Value, Leadid.Value).CustomerId;
            //ViewBag.FirstName = _Util.Facade.EmployeeFacade.GetAllInstallersByCompanyId(currentLoggedIn.CompanyId.Value).Select(x =>
            //                    new SelectListItem()
            //                    {
            //                        Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
            //                        Value = x.UserId.ToString()
            //                    }).ToList();

            List<SelectListItem> Installers = new List<SelectListItem>();
            Installers.Add(new SelectListItem()
            {
                Text = "Instellers",
                Value = "-1"
            });
            Installers.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Installer, new Guid()).Select(x => new SelectListItem()
            {
                Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                Value = x.UserId.ToString()
            }).ToList());
            ViewBag.FirstName = Installers;

            ViewBag.Arrival = _Util.Facade.LookupFacade.GetLookupByKey("Arrival").Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.DisplayText.ToString(),
                                      Value = x.DataValue.ToString()
                                  }).ToList();
            ViewBag.EstimatedArrival = _Util.Facade.LookupFacade.GetLookupByKey("EstimatedArrival").Select(x =>
                                     new SelectListItem()
                                     {
                                         Text = x.DisplayText.ToString(),
                                         Value = x.DataValue.ToString()
                                     }).ToList();
            return View("AddLeadTechSchedule", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddLeadTechSchedule(TechSchedule ts)
        {
            bool result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            ts.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)(User)).CompanyId.Value;
            if (ts.Id > 0)
            {
                result = _Util.Facade.TechScheduleFacade.UpdateTechSchedule(ts);
            }
            else
            {
                result = _Util.Facade.TechScheduleFacade.InsertTechSchedule(ts) > 0;
            }
            return Json(result);
        }

        [Authorize]
        public PartialViewResult EmergencyContactListPartial(Guid? LeadId, string IsSummary)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<EmergencyContact> model = new List<EmergencyContact>();
            if (currentLoggedIn != null)
            {
                if (!LeadId.HasValue)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                else
                {
                    if (LeadId.Value != new Guid())
                    {
                        model = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(LeadId.Value, currentLoggedIn.CompanyId.Value);
                    }
                }
                ViewBag.EmrLeadId = LeadId;
                // ViewBag.IsSummary = IsSummary;
                // ViewBag.IsSummary = true;
            }
            ViewBag.HasKey = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "On emergency contact do you want has key?").Value;
            return PartialView("EmergencyContactListPartial", model);
        }

        [Authorize]
        public PartialViewResult AddEmergencyContact(Guid? LeadId, int? EmgId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (/*!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerSetupEmergencyTab) ||*/ !LeadId.HasValue)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            EmergencyContact model = new EmergencyContact();

            ViewBag.AddEmgContactCustomerId = new Guid();

            if (LeadId.Value != new Guid() && EmgId.Value > 0)
            {
                ViewBag.AddEmgContactCustomerId = LeadId.Value;
                model = _Util.Facade.EmergencyContactFacade.GetEmergencyContactByCustomerIdAndCompanyIdAndId(LeadId.Value, currentLoggedIn.CompanyId.Value, EmgId.Value);
            }
            if (LeadId.Value != new Guid())
            {
                ViewBag.AddEmgContactCustomerId = LeadId.Value;
            }
            ViewBag.PhoneTypeList = _Util.Facade.LookupFacade.GetLookupByKey("PhoneType").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            ViewBag.Relationship = _Util.Facade.LookupFacade.GetLookupByKey("Relationship").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            ViewBag.HasKey = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "On emergency contact do you want has key?").Value;

            return PartialView("AddEmergencyContact", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddEmergencyContact(EmergencyContact _EmergencyContact)
        {
            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(new { result = result, message = "Access denied." });
            }

            if (EmergencyContactExists(_EmergencyContact.CustomerId, currentLoggedIn.CompanyId.Value, _EmergencyContact.Phone, _EmergencyContact.Id))
            {
                return Json(new { result = result, message = "Contact number already exists." });
            }
            if (_EmergencyContact.Phone != null)
            {
                _EmergencyContact.Phone = _EmergencyContact.Phone.TrimPhoneNum();
            }
            if (_EmergencyContact.Id > 0)
            {
                var OldEmergencyContactCopy = _Util.Facade.EmergencyContactFacade.GetEmergencyContactByCustomerIdAndCompanyIdAndId(_EmergencyContact.CustomerId, currentLoggedIn.CompanyId.Value, _EmergencyContact.Id);
                if (OldEmergencyContactCopy != null)
                {
                    OldEmergencyContactCopy.FirstName = _EmergencyContact.FirstName;
                    OldEmergencyContactCopy.LastName = _EmergencyContact.LastName;
                    OldEmergencyContactCopy.CrossSteet = _EmergencyContact.CrossSteet;
                    OldEmergencyContactCopy.RelationShip = _EmergencyContact.RelationShip;
                    OldEmergencyContactCopy.Phone = _EmergencyContact.Phone;
                    OldEmergencyContactCopy.PhoneType = _EmergencyContact.PhoneType;
                    OldEmergencyContactCopy.HasKey = _EmergencyContact.HasKey;

                    result = _Util.Facade.EmergencyContactFacade.UpdateEmergencyContact(OldEmergencyContactCopy) > 0;
                }
            }
            else
            {
                var EmergencyContactCount = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(_EmergencyContact.CustomerId, currentLoggedIn.CompanyId.Value);
                if (EmergencyContactCount.Count < 1)
                {
                    var LeadInfo = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(_EmergencyContact.CustomerId);
                    if (LeadInfo != null)
                    {
                        var LeadIntId = LeadInfo.Id;


                        var LeadobjName = _Util.Facade.CustomerFacade.GetLeadNameByLeadId(LeadIntId).LeadName;
                        Schedule objScheduleQA1 = new Schedule()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            StartDate = (DateTime.Now.UTCCurrentTime()).AddDays(1),
                            EndDate = (DateTime.Now.UTCCurrentTime()).AddDays(2),
                            Type = ScheduleType.QA1,
                            Title = ScheduleTitle.QA1Required + " " + LeadobjName,
                            IsCompleted = false,
                            LeadId = LeadIntId
                        };
                        _Util.Facade.ScheduleFacade.InsertSchedule(objScheduleQA1);
                        Schedule objScheduleQA2 = new Schedule()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            StartDate = (DateTime.Now.UTCCurrentTime()).AddDays(1),
                            EndDate = (DateTime.Now.UTCCurrentTime()).AddDays(2),
                            Type = ScheduleType.QA2,
                            Title = ScheduleTitle.QA2Required + " " + LeadobjName,
                            IsCompleted = false,
                            LeadId = LeadIntId
                        };
                        _Util.Facade.ScheduleFacade.InsertSchedule(objScheduleQA2);
                        Schedule objScheduleInstaller = new Schedule()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            StartDate = (DateTime.Now.UTCCurrentTime()).AddDays(1),
                            EndDate = (DateTime.Now.UTCCurrentTime()).AddDays(2),
                            Type = ScheduleType.Installer,
                            Title = ScheduleTitle.InstallerRequired + " " + LeadobjName,
                            IsCompleted = false,
                            LeadId = LeadIntId
                        };
                        _Util.Facade.ScheduleFacade.InsertSchedule(objScheduleInstaller);
                    }
                }
                _EmergencyContact.CompanyId = currentLoggedIn.CompanyId.Value;
                result = _Util.Facade.EmergencyContactFacade.InsertEmergencyContact(_EmergencyContact) > 0;

            }
            return Json(new { result = result, message = "Successful." });
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

        //[Authorize]
        //[HttpPost]
        //public JsonResult CustomerSetUpEmail(int LeadCustomerId)
        //{
        //    var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
        //    bool result = false;
        //    if (CurrentLoggedInUser == null)
        //    {
        //        return Json(result);
        //    }
        //    if (CurrentLoggedInUser != null)
        //    {
        //        var leadDetails = _Util.Facade.CustomerFacade.GetCustomerByLeadId(LeadCustomerId);
        //        if (leadDetails != null)
        //        {
        //            SetupLeadCustormer SetupLeadCustormer = new SetupLeadCustormer();
        //            SetupLeadCustormer.CustomerName = leadDetails.FirstName + " " + leadDetails.LastName;
        //            if (leadDetails.MiddleName != null)
        //            {
        //                SetupLeadCustormer.CustomerName = leadDetails.FirstName + " " + leadDetails.MiddleName + " " + leadDetails.LastName;
        //            }
        //            SetupLeadCustormer.ToEmail = leadDetails.EmailAddress;
        //            _Util.Facade.MailFacade.EmailToSuccessFullCustomerSetUp(SetupLeadCustormer, CurrentLoggedInUser.CompanyId.Value);
        //        }
        //    }
        //    return Json(result);
        //}

        public JsonResult EmergencyContactExists(Guid CustomerId, string PhoneNumber, int Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (EmergencyContactExists(CustomerId, CurrentUser.CompanyId.Value, PhoneNumber, Id))
            {
                return Json(new { result = true, message = "Contact number already exsists." });
            }
            else
            {
                return Json(new { result = false, message = "" });
            }

        }

        [Authorize]
        [HttpPost]
        public JsonResult LeadtoCustomerConvertQAEmail(int LeadtoCustomerid)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (CurrentLoggedInUser != null)
            {
                List<Employee> EmployeeDetails = _Util.Facade.EmployeeFacade.GetAllQAEmployee(CurrentLoggedInUser.CompanyId.Value);
                var leadDetails = _Util.Facade.CustomerFacade.GetCustomerByLeadId(LeadtoCustomerid);
                if (EmployeeDetails.Count > 0)
                {
                    LeadtoCustomer leadtocus = new LeadtoCustomer();

                    leadtocus.CustomerName = leadDetails.FirstName + " " + leadDetails.LastName;
                    if (leadDetails.MiddleName != null)
                    {
                        leadtocus.CustomerName = leadDetails.FirstName + " " + leadDetails.MiddleName + " " + leadDetails.LastName;
                    }
                    leadtocus.CustomerAddress = leadDetails.Address;

                    foreach (var item in EmployeeDetails)
                    {

                        leadtocus.EmployeeName = item.FirstName + " " + item.LastName;
                        leadtocus.ToEmail = item.UserName;
                        _Util.Facade.MailFacade.EmailToQALeadtoCusConverFor(leadtocus, CurrentLoggedInUser.CompanyId.Value);
                    }
                }
            }
            return Json(result);
        }

        [Authorize]
        public PartialViewResult LeadRequestTechnician(int? LeadId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            Customer model = new Customer();
            if (LeadId.HasValue)
            {
                model = _Util.Facade.CustomerFacade.GetCustomersById(LeadId.Value);
            }

            List<SelectListItem> InstallerList = new List<SelectListItem>();
            InstallerList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });

            InstallerList.AddRange(ViewBag.LeadUserList = _Util.Facade.EmployeeFacade
              .GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Installer, new Guid()).Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.FirstName + " " + x.LastName,
                                 Value = x.UserId.ToString()
                             }).ToList());
            ViewBag.InstallerList = InstallerList;
            InstallerList = InstallerList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            return PartialView("_LeadRequestTechnician", model);
        }

        [Authorize]
        public JsonResult AddLeadRequestTechnician(int? Id, Guid Installer)
        {
            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }

            if (Id.HasValue && Id.Value > 0)
            {
                var DbCustomer = _Util.Facade.CustomerFacade.GetCustomersById(Id.Value);
                if (DbCustomer != null)
                {
                    DbCustomer.Installer = Installer.ToString();
                    result = _Util.Facade.CustomerFacade.UpdateCustomer(DbCustomer);
                }

            }
            return Json(new { result = result, TechInstaller = Installer });
        }

        public PartialViewResult AddLeadDocument(int? id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            ViewBag.CustomerId = id.Value;
            ViewBag.FileTag = _Util.Facade.LookupFacade.GetLookupByKey("FileTag").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();
            return PartialView("_AddLeadDocument");
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveLeadFile(string File, int CustomerId, string Description, double _fileSize, string Tag)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = File;

            bool status = new AWSS3ObjectService().CheckFileExists(serverFile);

            //if (!System.IO.File.Exists(serverFile))   
            //{
            //    return Json(new { result = true, message = "File not Exist" });
            //}

            if (!status == true)
            {
                return Json(new { result = true, message = "File not Exist" });

            }

            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetById(CustomerId);


            //// ""Mayur" Calculate File Size : start : already in kb
            #region Calculate file size

            double fileSize = _fileSize;

            #endregion


            CustomerFile cf = new CustomerFile()
            {
                CompanyId = CurrentUser.CompanyId.Value,
                FileId = Guid.NewGuid(),
                CustomerId = tmpCustomer.CustomerId,
                FileDescription = Description,
                Filename = "/" + File,
                Tag = Tag,
                FileFullName = Description,
                IsActive = true,
                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                CreatedBy = CurrentUser.UserId,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                UpdatedBy = CurrentUser.UserId,
                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                WMStatus = LabelHelper.WatermarkStatus.Pending,
                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
            };
            bool result = _Util.Facade.CustomerFileFacade.InsertCustomerFile(cf) > 0;
            return Json(new { result = result });
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> UploadLeadFile(int CustomerId)
        {
            string FilePath = "";
            string FileKey = "";
            bool isUploaded = false;
            string tempFolderName = "";
            string FileName = "";
            var returnurl = "";
            byte[] data;
            decimal _fileSize = 1.00m;


            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

            // old save 
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

            //string tempFolderName = ConfigurationManager.AppSettings
            //
            //;
            //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            //tempFolderName = string.Format(tempFolderName, comname);
            //tempFolderName += "/" + CustomerId.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            //Random rand = new Random();
            //string FileName = rand.Next().ToString();
            //FileName += "-___" + httpPostedFileBase.FileName;

            //if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            //{

            //    string tempFolderPath = Server.MapPath("~/" + tempFolderName);

            //    if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
            //    {
            //        try
            //        {
            //            httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
            //            isUploaded = true;
            //        }
            //        catch (Exception) {  /*TODO: You must process this exception.*/}
            //    }
            //}

            //old save


            //// "mayur" AWS S3 Changes //// Start

            #region File Save on AWS S3
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = ConfigurationManager.AppSettings["File.CustomerFiles"];
            tempFolderName = tempFolderName.TrimEnd('/');
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += "/" + CustomerId.ToString() + "/" + DateTime.Now.UTCCurrentTime().Year + "-" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBase.FileName;
            FileName = Regex.Replace(FileName, @"[^a-zA-Z0-9\._]", "_");
            // "mayur" //// Converting file to memory stream ////////// Start

            using (Stream inputStream = httpPostedFileBase.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }
            // "mayur" //// Converting file to memory stream ////////// End

            FilePath = tempFolderName;
            FileKey = string.Format($"{FilePath}/{FileName}");


            AWSS3ObjectService AWSobject = new AWSS3ObjectService();

            await AWSobject.UploadFile(FileKey, data);
            await AWSobject.MakePublic(FileName, FilePath);


            returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
            returnurl = returnurl + FileKey;



            isUploaded = true;

            ViewBag.ReturnUrl = returnurl;
            ViewBag.FileName = FileName;
            ViewBag.FileKey = FileKey;

            #endregion

            //// "mayur" AWS S3 Changes //// End

            _fileSize = (decimal)data.Length / 1024;
            _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);


            FilePath = FilePath + "/" + FileName;
            string FullFilePath = returnurl;
            return Json(new { isUploaded = isUploaded, filePath = FilePath, FullFilePath = returnurl, fileSize = _fileSize, FullPath = FileKey }, "text/html", JsonRequestBehavior.AllowGet);
        }

        public ActionResult InstallationAgreement(int? LeadId)
        {
            DateTime FixDate = DateTime.Now.UTCCurrentTime();
            InstallationAgreementModel Model = new InstallationAgreementModel();
            Model.EmergencyContactList = new List<EmergencyContact>();
            Model.ListAgreementAnswer = new List<AgreementAnswer>();
            Model.EquipmentList = new List<Equipment>();
            Model.CustomerAgreement = new List<CustomerAgreement>();
            Model.SingleCustomerAgreement = new CustomerAgreement();
            var taxtotal = 0.0;
            Guid CompanyId = new Guid();
            if (User.Identity.IsAuthenticated)
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CompanyId = CurrentUser.CompanyId.Value;
            }
            else
            {
                CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(LeadId.Value);
                CompanyId = custommerCompany.CompanyId;
            }
            Customer Cus = new Customer();
            Company Com = new Company();
            if (LeadId.HasValue)
            {
                //if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(LeadId.Value, CompanyId))
                //{
                //    return null;
                //}

                Cus = _Util.Facade.CustomerFacade.GetCustomerById(LeadId.Value);
                Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);

                string ContractTerm = "";
                if (!string.IsNullOrWhiteSpace(Cus.ContractTeam) && Cus.ContractTeam != "-1")
                {
                    ContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))).ToString();
                }
                var objCusAgree = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByComIdAndCusIsAndLoadAgreement(Com.CompanyId, Cus.CustomerId);
                if (objCusAgree == null)
                {
                    CustomerAgreement objCustomerAgreement = new CustomerAgreement()
                    {
                        CompanyId = Com.CompanyId,
                        CustomerId = Cus.CustomerId,
                        IP = AppConfig.GetIP,
                        UserAgent = AppConfig.GetUserAgent,
                        Type = LabelHelper.CustomerAgreementLog.LoadAgreement,
                        AddedDate = DateTime.UtcNow
                    };
                    _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objCustomerAgreement);
                }
                var EquipmentTotalPrice = 0.0;
                var AgreementSubtotal = 0.0;
                var AgreementTotal = 0.0;
                var AgreementTax = 0.0;
                var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(Cus.CustomerId, Com.CompanyId);
                if (GetCityTaxList.Count > 0)
                {
                    foreach (var item1 in GetCityTaxList)
                    {
                        AgreementTax = item1.Rate;
                    }
                }
                else
                {
                    Guid CustomerId = new Guid();
                    if (Cus != null)
                    {
                        CustomerId = Cus.CustomerId;
                    }
                    var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(Com.CompanyId, CustomerId);
                    if (GetSalesTax != null)
                    {
                        AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                    }
                }
                var CustomEquipmentList = _Util.Facade.EquipmentFacade.GetEquipmentListByCustomerIdAndCompanyId(LeadId.Value, CompanyId);
                if (CustomEquipmentList.Count > 0)
                {
                    foreach (var item in CustomEquipmentList)
                    {
                        EquipmentTotalPrice += item.TotalPrice;
                    }
                    if (Cus.ActivationFee.HasValue)
                    {
                        AgreementSubtotal = Cus.ActivationFee.Value + EquipmentTotalPrice;
                    }
                    else
                    {
                        AgreementSubtotal = EquipmentTotalPrice;
                    }
                }
                if (AgreementTax != 0.0)
                {
                    taxtotal = (AgreementSubtotal / 100) * AgreementTax;
                    Model.TaxTotal = taxtotal;
                    AgreementTotal = AgreementSubtotal + taxtotal;
                }
                else
                {
                    Model.TaxTotal = 0.0;
                    AgreementTotal = AgreementSubtotal;
                }

                #region LeadSource
                string LeadSource = "";
                Lookup leadsource = _Util.Facade.LookupFacade.GetLookupByKeyAndValueAndCompanyId("LeadSource", Cus.LeadSource, Com.CompanyId);
                LeadSource = leadsource != null && !string.IsNullOrWhiteSpace(leadsource.DisplayText) && leadsource.DataValue != "-1" ? leadsource.DisplayText : "";
                #endregion

                Model = new InstallationAgreementModel()
                {
                    CSIDNumber = Cus.Id,
                    LeadSource = LeadSource,
                    BillingAddress = Cus.Street + " " + Cus.City + ", " + Cus.State + " " + Cus.ZipCode,
                    OwnerAddress = Cus.Street + " " + Cus.City + ", " + Cus.State + " " + Cus.ZipCode,
                    OwnerEmail = Cus.EmailAddress,
                    OwnerPhone = Cus.PrimaryPhone,
                    OwnerName = Cus.FirstName + " " + Cus.LastName,
                    EmergencyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId),
                    CompanyName = Com.CompanyName,
                    CompanySate = string.Format("{0}, {1} {2}", Com.City, Com.State, Com.ZipCode),
                    CompanyStreet = Com.Street,
                    CompanyWebsite = Com.Website,
                    SubscribedMonths = ContractTerm,
                    Password = Cus.Passcode,
                    DateOfTransaction = FixDate.UTCToClientTime(),
                    CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CompanyId),
                    EquipmentList = CustomEquipmentList.ToList(),
                    ActivationFee = Cus.ActivationFee.HasValue ? Cus.ActivationFee.Value : 0,
                    BusinessName = Cus.BusinessName,
                    MonthlyMonitoringFee = Cus.MonthlyMonitoringFee,
                    EffectiveDate = FixDate.UTCToClientTime(),
                    CustomerSignature = Cus.Singature,
                    CustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByCompanyIdAndCustomerId1(CompanyId, Cus.CustomerId),
                    Subtotal = AgreementSubtotal,
                    Tax = AgreementTax,
                    TaxTotal = taxtotal,
                    Total = AgreementTotal,
                    EContractId = Cus.Id,
                    ListAgreementAnswer = _Util.Facade.AgreementFacade.GetAllAgreementAnswerByCustomerId(Cus.CustomerId),
                    SalesRepresentative = !string.IsNullOrEmpty(Cus.Soldby) ? _Util.Facade.EmployeeFacade.GetEmployeeNumByEmployeeId(new Guid(Cus.Soldby)).ToString() : "",
                    TotalPayments = (!string.IsNullOrWhiteSpace(Cus.MonthlyMonitoringFee) ? Convert.ToDouble(Cus.MonthlyMonitoringFee) : 1) * Convert.ToDouble(ContractTerm),
                    SingleCustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetIpAndUserAgentByCustomerIdAndCompanyId(CompanyId, Cus.CustomerId)
                };
            }
            else
            {
                Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CompanyId);
            }

            //  return View(Model);
            ViewBag.CompanyId = Com.CompanyId.ToString();
            Model.CompanyId = Com.CompanyId.ToString();
            Model.CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(Com.CompanyId);
            string body = _Util.Facade.AgreementFacade.MakeAgreementPdf(Model);
            ViewBag.Body = body;
            return new ViewAsPdf()
            {
                // FileName = flightPlan.ListingItemDetailsModel.FlightDetails + ".pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                //PageMargins = new Margins(10, 0, 0, 0),
                PageMargins = new Margins(10, 2, 10, 3)
            };
        }

        [Authorize]
        [HttpPost]
        public JsonResult LeadConvertedToCustomerPDFMail_V2(int? leadid)
        {
            DateTime FixDate = DateTime.Now.UTCCurrentTime();
            //int idlead = Convert.ToInt32(Lid);
            InstallationAgreementModel Model = new InstallationAgreementModel();
            Model.EmergencyContactList = new List<EmergencyContact>();
            Model.ListAgreementAnswer = new List<AgreementAnswer>();
            Model.EquipmentList = new List<Equipment>();
            Model.CustomerAgreement = new List<CustomerAgreement>();
            Model.SingleCustomerAgreement = new CustomerAgreement();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool AgreementResult = false;
            //var ActivationfeeValue = 0.0;
            //var IsActivationFee = _Util.Facade.ActivationFeeFacade.GetActivationFeeByCompanyId(CurrentUser.CompanyId.Value);
            //if (IsActivationFee != null)
            //{
            //    ActivationfeeValue = IsActivationFee.Fee;
            //}
            Customer Cus = new Customer();
            Company Com = new Company();
            if (leadid.HasValue)
            {
                //if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(leadid.Value, CurrentUser.CompanyId.Value))
                //{
                //    return null;
                //}

                Cus = _Util.Facade.CustomerFacade.GetCustomerById(leadid.Value);
                Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);

                string ContractTerm = "";
                if (!string.IsNullOrWhiteSpace(Cus.ContractTeam) && Cus.ContractTeam != "-1")
                {
                    ContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))).ToString();
                }
                var EquipmentTotalPrice = 0.0;
                var AgreementSubtotal = 0.0;
                var AgreementTotal = 0.0;
                var AgreementTax = 0.0;
                var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(Cus.CustomerId, Com.CompanyId);
                if (GetCityTaxList.Count > 0)
                {
                    foreach (var item1 in GetCityTaxList)
                    {
                        AgreementTax = item1.Rate;
                    }
                }
                else
                {
                    Guid CustomerId = new Guid();
                    if (Cus != null)
                    {
                        CustomerId = Cus.CustomerId;
                    }
                    var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(Com.CompanyId, CustomerId);
                    if (GetSalesTax != null)
                    {
                        AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                    }
                }
                var CustomEquipmentList = _Util.Facade.EquipmentFacade.GetEquipmentListByCustomerIdAndCompanyId(leadid.Value, Com.CompanyId);
                if (CustomEquipmentList.Count > 0)
                {
                    foreach (var item in CustomEquipmentList)
                    {
                        EquipmentTotalPrice += item.TotalPrice;
                    }
                    if (Cus.ActivationFee.HasValue)
                    {
                        AgreementSubtotal = Cus.ActivationFee.Value + EquipmentTotalPrice;
                    }
                    else
                    {
                        AgreementSubtotal = EquipmentTotalPrice;
                    }
                }
                if (AgreementTax != 0.0)
                {
                    var taxtotal = (AgreementSubtotal / 100) * AgreementTax;
                    AgreementTotal = AgreementSubtotal + taxtotal;
                }
                else
                {
                    AgreementTotal = AgreementSubtotal;
                }

                #region LeadSource
                string LeadSource = "";
                Lookup leadsource = _Util.Facade.LookupFacade.GetLookupByKeyAndValueAndCompanyId("LeadSource", Cus.LeadSource, Com.CompanyId);
                LeadSource = leadsource != null && !string.IsNullOrWhiteSpace(leadsource.DisplayText) && leadsource.DataValue != "-1" ? leadsource.DisplayText : "";
                #endregion

                Model = new InstallationAgreementModel()
                {
                    CSIDNumber = Cus.Id,
                    LeadSource = LeadSource,
                    BillingAddress = Cus.Street + " " + Cus.City + ", " + Cus.State + " " + Cus.ZipCode,
                    OwnerAddress = Cus.Street + " " + Cus.City + ", " + Cus.State + " " + Cus.ZipCode,
                    OwnerEmail = Cus.EmailAddress,
                    OwnerPhone = Cus.PrimaryPhone,
                    OwnerName = Cus.FirstName + " " + Cus.LastName,
                    EmergencyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(Cus.CustomerId, CurrentUser.CompanyId.Value),
                    CompanyName = Com.CompanyName,
                    CompanySate = string.Format("{0}, {1} {2}", Com.City, Com.State, Com.ZipCode),
                    CompanyStreet = Com.Street,
                    CompanyWebsite = Com.Website,
                    SubscribedMonths = ContractTerm,
                    Password = Cus.Passcode,
                    DateOfTransaction = FixDate.UTCToClientTime(),
                    //CompanyLogo = _Util.Facade.GlobalSettingsFacade.GetCompanyColoredLogoByCompanyId(CurrentUser.CompanyId.Value),
                    CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value),
                    EquipmentList = _Util.Facade.EquipmentFacade.GetEquipmentListByCustomerIdAndCompanyId(leadid.Value, CurrentUser.CompanyId.Value),
                    ActivationFee = Cus.ActivationFee.HasValue ? Cus.ActivationFee.Value : 0,
                    BusinessName = Cus.BusinessName,
                    MonthlyMonitoringFee = Cus.MonthlyMonitoringFee,
                    EffectiveDate = FixDate.UTCToClientTime(),
                    CustomerSignature = Cus.Singature,
                    Subtotal = AgreementSubtotal,
                    Tax = AgreementTax,
                    Total = AgreementTotal,
                    EContractId = Cus.Id,
                    ListAgreementAnswer = _Util.Facade.AgreementFacade.GetAllAgreementAnswerByCustomerId(Cus.CustomerId),
                    SalesRepresentative = !string.IsNullOrEmpty(Cus.Soldby) ? _Util.Facade.EmployeeFacade.GetEmployeeNumByEmployeeId(new Guid(Cus.Soldby)).ToString() : "",
                    TotalPayments = (!string.IsNullOrWhiteSpace(Cus.MonthlyMonitoringFee) ? Convert.ToDouble(Cus.MonthlyMonitoringFee) : 1) * Convert.ToDouble(ContractTerm),
                    SingleCustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetIpAndUserAgentByCustomerIdAndCompanyId(CurrentUser.CompanyId.Value, Cus.CustomerId),
                    CustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByCompanyIdAndCustomerId1(CurrentUser.CompanyId.Value, Cus.CustomerId)
                };
                //if (Model.EquipmentList != null && Model.EquipmentList.Count() > 0)
                //{
                //    Model.Subtotal = Model.EquipmentList.Sum(x => x.TotalPrice) + Model.ActivationFee;
                //    var taxp = Convert.ToDouble(_Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value).Value);
                //    Model.Tax = (Model.Subtotal * taxp) / 100;
                //    Model.Total = Model.Subtotal + Model.Tax;

                //}
                //else
                //{
                //    Model.EquipmentList = new List<Equipment>();
                //}

                if (Model.EmergencyContactList == null)
                {
                    Model.EmergencyContactList = new List<EmergencyContact>();
                }
            }
            else
            {
                Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            }

            //  return View(Model);
            ViewBag.CompanyId = Com.CompanyId.ToString();
            Model.CompanyId = Com.CompanyId.ToString();
            Model.CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(Com.CompanyId);
            string body = _Util.Facade.AgreementFacade.MakeAgreementPdf(Model);
            ViewBag.Body = body;
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("InstallationAgreement")
            {
                //FileName = "TestView.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.LeadToCustomerAgreement"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            var pdftempFolderName = string.Format(filename, comname) + rand.Next().ToString() + leadid + "AgreementMail.pdf";
            string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
            FileHelper.SaveFile(applicationPDFData, Serverfilename);
            //var cusinfo = _Util.Facade.CustomerFacade.GetById(leadid.Value);
            bool result = false;
            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(leadid + "#" + Cus.EmailAddress + "#" + CurrentUser.CompanyId.Value.ToString());
            //string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Agreement/", encryptedurl);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/Public/LeadsAgreementDocument/?code=", encryptedurl);

            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, Cus.CustomerId);
            string shortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code);

            if (Cus != null)
            {
                if (Cus.EmailAddress != "" && Cus.EmailAddress != null)
                {
                    result = true;
                    LeadsAggrement la = new LeadsAggrement
                    {
                        CustomerNum = Cus.FirstName + " " + Cus.LastName,
                        ToEmail = Cus.EmailAddress,
                        LeadsAggrementpdf = new Attachment(Serverfilename, MediaTypeNames.Application.Octet),
                        BodyLink = shortUrl,
                        CustomerId = Cus.CustomerId.ToString(),
                        EmployeeId = CurrentUser.UserId.ToString()
                    };
                    AgreementResult = _Util.Facade.MailFacade.EmailOnlyLeadsAggrement(la, CurrentUser.CompanyId.Value, null);
                }
            }
            string message = "Agreement sent to " + Cus.EmailAddress;
            return Json(new { result = result, message = message });
        }

        public JsonResult LeadConvertedToCustomerPDFMail(int? leadid)
        {

            //WebClient webClient;
            //byte[] fileBytes1;
            //string Temp_FileName;

            DateTime FixDate = DateTime.Now.UTCCurrentTime();
            //int idlead = Convert.ToInt32(Lid);
            InstallationAgreementModel Model = new InstallationAgreementModel();
            Model.EmergencyContactList = new List<EmergencyContact>();
            Model.ListAgreementAnswer = new List<AgreementAnswer>();
            Model.EquipmentList = new List<Equipment>();
            Model.CustomerAgreement = new List<CustomerAgreement>();
            Model.SingleCustomerAgreement = new CustomerAgreement();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool AgreementResult = false;
            //var ActivationfeeValue = 0.0;
            //var IsActivationFee = _Util.Facade.ActivationFeeFacade.GetActivationFeeByCompanyId(CurrentUser.CompanyId.Value);
            //if (IsActivationFee != null)
            //{
            //    ActivationfeeValue = IsActivationFee.Fee;
            //}
            Customer Cus = new Customer();
            Company Com = new Company();
            if (leadid.HasValue)
            {
                //if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(leadid.Value, CurrentUser.CompanyId.Value))
                //{
                //    return null;
                //}

                Cus = _Util.Facade.CustomerFacade.GetCustomerById(leadid.Value);
                Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);

                string ContractTerm = "";
                if (!string.IsNullOrWhiteSpace(Cus.ContractTeam) && Cus.ContractTeam != "-1")
                {
                    ContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))).ToString();
                }
                var EquipmentTotalPrice = 0.0;
                var AgreementSubtotal = 0.0;
                var AgreementTotal = 0.0;
                var AgreementTax = 0.0;
                var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(Cus.CustomerId, Com.CompanyId);
                if (GetCityTaxList.Count > 0)
                {
                    foreach (var item1 in GetCityTaxList)
                    {
                        AgreementTax = item1.Rate;
                    }
                }
                else
                {
                    Guid CustomerId = new Guid();
                    if (Cus != null)
                    {
                        CustomerId = Cus.CustomerId;
                    }
                    var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(Com.CompanyId, CustomerId);
                    if (GetSalesTax != null)
                    {
                        AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                    }
                }
                var CustomEquipmentList = _Util.Facade.EquipmentFacade.GetEquipmentListByCustomerIdAndCompanyId(leadid.Value, Com.CompanyId);
                if (CustomEquipmentList.Count > 0)
                {
                    foreach (var item in CustomEquipmentList)
                    {
                        EquipmentTotalPrice += item.TotalPrice;
                    }
                    if (Cus.ActivationFee.HasValue)
                    {
                        AgreementSubtotal = Cus.ActivationFee.Value + EquipmentTotalPrice;
                    }
                    else
                    {
                        AgreementSubtotal = EquipmentTotalPrice;
                    }
                }
                if (AgreementTax != 0.0)
                {
                    var taxtotal = (AgreementSubtotal / 100) * AgreementTax;
                    AgreementTotal = AgreementSubtotal + taxtotal;
                }
                else
                {
                    AgreementTotal = AgreementSubtotal;
                }

                #region LeadSource
                string LeadSource = "";
                Lookup leadsource = _Util.Facade.LookupFacade.GetLookupByKeyAndValueAndCompanyId("LeadSource", Cus.LeadSource, Com.CompanyId);
                LeadSource = leadsource != null && !string.IsNullOrWhiteSpace(leadsource.DisplayText) && leadsource.DataValue != "-1" ? leadsource.DisplayText : "";
                #endregion

                Model = new InstallationAgreementModel()
                {
                    CSIDNumber = Cus.Id,
                    LeadSource = LeadSource,
                    BillingAddress = Cus.Street + " " + Cus.City + ", " + Cus.State + " " + Cus.ZipCode,
                    OwnerAddress = Cus.Street + " " + Cus.City + ", " + Cus.State + " " + Cus.ZipCode,
                    OwnerEmail = Cus.EmailAddress,
                    OwnerPhone = Cus.PrimaryPhone,
                    OwnerName = Cus.FirstName + " " + Cus.LastName,
                    EmergencyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(Cus.CustomerId, CurrentUser.CompanyId.Value),
                    CompanyName = Com.CompanyName,
                    CompanySate = string.Format("{0}, {1} {2}", Com.City, Com.State, Com.ZipCode),
                    CompanyStreet = Com.Street,
                    CompanyWebsite = Com.Website,
                    SubscribedMonths = ContractTerm,
                    Password = Cus.Passcode,
                    DateOfTransaction = FixDate.UTCToClientTime(),
                    //CompanyLogo = _Util.Facade.GlobalSettingsFacade.GetCompanyColoredLogoByCompanyId(CurrentUser.CompanyId.Value),
                    CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value),
                    EquipmentList = _Util.Facade.EquipmentFacade.GetEquipmentListByCustomerIdAndCompanyId(leadid.Value, CurrentUser.CompanyId.Value),
                    ActivationFee = Cus.ActivationFee.HasValue ? Cus.ActivationFee.Value : 0,
                    BusinessName = Cus.BusinessName,
                    MonthlyMonitoringFee = Cus.MonthlyMonitoringFee,
                    EffectiveDate = FixDate.UTCToClientTime(),
                    CustomerSignature = Cus.Singature,
                    Subtotal = AgreementSubtotal,
                    Tax = AgreementTax,
                    Total = AgreementTotal,
                    EContractId = Cus.Id,
                    ListAgreementAnswer = _Util.Facade.AgreementFacade.GetAllAgreementAnswerByCustomerId(Cus.CustomerId),
                    SalesRepresentative = !string.IsNullOrEmpty(Cus.Soldby) ? _Util.Facade.EmployeeFacade.GetEmployeeNumByEmployeeId(new Guid(Cus.Soldby)).ToString() : "",
                    TotalPayments = (!string.IsNullOrWhiteSpace(Cus.MonthlyMonitoringFee) ? Convert.ToDouble(Cus.MonthlyMonitoringFee) : 1) * Convert.ToDouble(ContractTerm),
                    SingleCustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetIpAndUserAgentByCustomerIdAndCompanyId(CurrentUser.CompanyId.Value, Cus.CustomerId),
                    CustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByCompanyIdAndCustomerId1(CurrentUser.CompanyId.Value, Cus.CustomerId)
                };
                //if (Model.EquipmentList != null && Model.EquipmentList.Count() > 0)
                //{
                //    Model.Subtotal = Model.EquipmentList.Sum(x => x.TotalPrice) + Model.ActivationFee;
                //    var taxp = Convert.ToDouble(_Util.Facade.GlobalSettingsFacade.GetSalesTax(CurrentUser.CompanyId.Value).Value);
                //    Model.Tax = (Model.Subtotal * taxp) / 100;
                //    Model.Total = Model.Subtotal + Model.Tax;

                //}
                //else
                //{
                //    Model.EquipmentList = new List<Equipment>();
                //}

                if (Model.EmergencyContactList == null)
                {
                    Model.EmergencyContactList = new List<EmergencyContact>();
                }
            }
            else
            {
                Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            }

            //  return View(Model);
            ViewBag.CompanyId = Com.CompanyId.ToString();
            Model.CompanyId = Com.CompanyId.ToString();
            Model.CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(Com.CompanyId);
            string body = _Util.Facade.AgreementFacade.MakeAgreementPdf(Model);
            ViewBag.Body = body;
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("InstallationAgreement")
            {
                //FileName = "TestView.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            #region Save File
            //Random rand = new Random();
            //string filename = ConfigurationManager.AppSettings["File.LeadToCustomerAgreement"];
            //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            //var pdftempFolderName = string.Format(filename, comname) + rand.Next().ToString() + leadid + "AgreementMail.pdf";
            //string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
            //FileHelper.SaveFile(applicationPDFData, Serverfilename);
            #endregion


            //// "mayur" AWS S3 Changes //// Start

            #region File Save on AWS S3

            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.LeadToCustomerAgreement"];
            filename = filename.TrimEnd('/');

            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();

            string FilePath = string.Format(filename, comname);
            string FileName = rand.Next().ToString() + leadid + "AgreementMail.pdf";

            string FileKey = string.Format($"{FilePath}/{FileName}");

            var returnurl = string.Empty;

            var task = Task.Run(async () =>
            {
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



            //returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
            //returnurl = returnurl + FileKey;
            returnurl += FileKey;


            ViewBag.ReturnUrl = returnurl;
            ViewBag.FileName = FileName;
            ViewBag.FileKey = FileKey;

            #endregion

            //// "mayur" AWS S3 Changes //// End

            //var cusinfo = _Util.Facade.CustomerFacade.GetById(leadid.Value);


            bool result = false;
            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(leadid + "#" + Cus.EmailAddress + "#" + CurrentUser.CompanyId.Value.ToString());
            //string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Agreement/", encryptedurl);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/Public/LeadsAgreementDocument/?code=", encryptedurl);

            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, Cus.CustomerId);
            string shortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code);


            /// Mayur :: File Download to temp folder :start

            //webClient = new WebClient();
            //fileBytes1 = webClient.DownloadData(returnurl);

            //File(applicationPDFData, System.Net.Mime.MediaTypeNames.Application.Octet, FileName).ToString();



            //Temp_FileName = Server.MapPath("~/EmailFileCache/tmp_" + FileName);

            //if (!System.IO.File.Exists(Temp_FileName))
            //{
            //    System.IO.File.WriteAllBytes(Temp_FileName, applicationPDFData);
            //}
            //else
            //{
            //    System.IO.File.WriteAllBytes(Temp_FileName, applicationPDFData);
            //}

            /// Mayur :: File Download to temp folder :End

            if (Cus != null)
            {
                if (Cus.EmailAddress != "" && Cus.EmailAddress != null)
                {
                    result = true;
                    LeadsAggrement la = new LeadsAggrement
                    {
                        CustomerNum = Cus.FirstName + " " + Cus.LastName,
                        ToEmail = Cus.EmailAddress,
                        //LeadsAggrementpdf = new Attachment(Temp_FileName, MediaTypeNames.Application.Octet),
                        LeadsAggrementpdf = new Attachment(new MemoryStream(applicationPDFData), MediaTypeNames.Application.Octet),
                        BodyLink = shortUrl,
                        CustomerId = Cus.CustomerId.ToString(),
                        EmployeeId = CurrentUser.UserId.ToString()
                    };
                    AgreementResult = _Util.Facade.MailFacade.EmailOnlyLeadsAggrement(la, CurrentUser.CompanyId.Value, null);
                }
            }
            string message = "Agreement sent to " + Cus.EmailAddress;
            return Json(new { result = result, message = message });
        }


        [HttpPost]
        public JsonResult IAgreeSetup(int? Id)
        {
            if (!Id.HasValue)
                return Json(false);
            Customer _Customer = new Customer();

            Guid CompanyId = new Guid();
            if (User.Identity.IsAuthenticated)
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CompanyId = CurrentUser.CompanyId.Value;
            }
            else
            {
                CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(Id.Value);
                CompanyId = custommerCompany.CompanyId;
            }
            Company _Company = new Company { CompanyId = CompanyId };

            if (Id.Value > 0)
            {
                _Customer = _Util.Facade.CustomerFacade.GetCustomersById(Id.Value);

            }
            bool result = IAgreeConvertLeadToCustomer(_Customer, _Company);
            result = result && IAgreeLeadtoCustomerConvertQAEmail(_Customer, _Company);
            result = result && IAgreeAllScheduleCalendar(_Customer, _Company);
            result = result && IAgreeCustomerFileUpload(_Customer, _Company);
            return Json(result);
        }
        [Authorize]
        public ActionResult PackageSettingsPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView("PackageSettingsPartial");
        }

        [Authorize]
        public ActionResult LoadCompanyPackageSettingsPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView("LoadCompanyPackageSettingsPartial");
        }
        [Authorize]
        [HttpPost]
        public JsonResult UpdatePackageServiceList(List<PackageInclude> pkgServiceList)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (pkgServiceList.Count() == 0)
            {
                return Json(new { result = false, message = "No data selected." });
            }
            foreach (var item in pkgServiceList)
            {
                //also can take all lookup list by data key
                //then update individually.

                PackageInclude pkg;
                if (item.Id > 0)
                {
                    pkg = _Util.Facade.PackageFacade.GetPackageIncludeByIdAndCompanyId(item.Id, CurrentUser.CompanyId.Value);
                    if (pkg != null)
                    {
                        pkg.OrderBy = item.OrderBy;

                        _Util.Facade.PackageFacade.UpdatePackageInclude(pkg);
                    }
                }


            }
            return Json(new { result = true, message = "Package service updated successfully." });

        }

        [Authorize]
        public ActionResult CompanyPackageListPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<Package> model = new List<Package>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.PackageFacade.GetAllPackageByCompanyId(CurrentUser.CompanyId.Value);
            }

            return PartialView("_CompanyPackageListPartial", model);
        }

        [Authorize]
        public ActionResult AddCompanyPackagePartial(int? Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Package model = new Package();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (Id.HasValue && Id > 0)
            {
                model = _Util.Facade.PackageFacade.GetPackageByIdAndCompanyId(Id.Value, CurrentUser.CompanyId.Value);
                ////ChangePackage
                //model.MMRRange = _Util.Facade.PackageFacade.GetMMrRangeByPackageId(model.PackageId);
            }
            return PartialView("_AddCompanyPackagePartial", model);
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddCompanyPackage(Package _Package)
        {
            var result = false;
            var OldPackage = new Package();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            _Package.CompanyId = CurrentUser.CompanyId.Value;
            if (CurrentUser == null)
            {
                return Json(result);
            }
            if (_Package.Id > 0)
            {
                OldPackage = _Util.Facade.PackageFacade.GetPackageByIdAndCompanyId(_Package.Id, _Package.CompanyId);
                if (OldPackage != null)
                {
                    OldPackage.Name = _Package.Name;
                    OldPackage.Rate = _Package.Rate;
                    result = _Util.Facade.PackageFacade.UpdatePackage(OldPackage);
                }
            }
            else
            {
                _Package.PackageId = Guid.NewGuid();
                result = _Util.Facade.PackageFacade.InsertPackage(_Package) > 0;
            }
            //if (result == true)
            //{
            //    //ChangePackage
            //    var delMMrange = false;
            //    if (OldPackage.PackageId != Guid.Empty)
            //    {
            //        delMMrange = _Util.Facade.PackageFacade.DeleteMMrRangeByPackageId(CurrentUser.CompanyId.Value, OldPackage.PackageId);
            //    }
            //    if (delMMrange == true)
            //    {
            //        MMRRange objMMRRange = new MMRRange()
            //        {
            //            CompanyId = CurrentUser.CompanyId.Value,
            //            //ChangePackage
            //            PackageId = OldPackage.PackageId,
            //            MinMMR = _MMRRange.MinMMR,
            //            MaxMMR = _MMRRange.MaxMMR
            //        };
            //        _Util.Facade.PackageFacade.InsertMMrRange(objMMRRange);
            //    }
            //    else
            //    {
            //        MMRRange objMMRRange = new MMRRange()
            //        {
            //            CompanyId = CurrentUser.CompanyId.Value,
            //            //ChangePackage
            //            PackageId = _Package.PackageId,
            //            MinMMR = _MMRRange.MinMMR,
            //            MaxMMR = _MMRRange.MaxMMR
            //        };
            //        _Util.Facade.PackageFacade.InsertMMrRange(objMMRRange);
            //    }
            //}
            return Json(result);
        }


        [Authorize]
        [HttpPost]
        public JsonResult DeletePackage(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.PackageFacade.DeletePackage(id.Value);
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult LoadcompanyPackageInchudedSettings()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView("LoadcompanyPackageInchudedSettings");
        }

        [Authorize]
        public ActionResult CompanyPackageIncludeListPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<PackageInclude> model = new List<PackageInclude>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.PackageFacade.GetAllPackageIncludeProducts(CurrentUser.CompanyId.Value);
            }
            return PartialView("_CompanyPackageIncludeListPartial", model);
        }

        [Authorize]
        public ActionResult AddCompanyPackageIncludePartial(int? Id, int? packageid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            PackageInclude model = new PackageInclude();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #region PackageList in viewbag
            List<SelectListItem> PackageList = new List<SelectListItem>();
            PackageList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            PackageList.AddRange(ViewBag.PackageList = _Util.Facade.PackageFacade
              .GetAllPackageByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.Name.ToString(),
                                 Value = x.Id.ToString()
                             }).ToList());
            PackageList = PackageList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            ViewBag.PackageList = PackageList;
            #endregion
            #region EquipmentList In viewbag
            List<SelectListItem> EquipmentList = new List<SelectListItem>();
            EquipmentList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            if (packageid.HasValue && packageid > 0)
            {
                EquipmentList.AddRange(_Util.Facade.EquipmentFacade
             .GetAllEquipmentsForOptByCompanyId(CurrentUser.CompanyId.Value, packageid.Value).Select(x =>
                            new SelectListItem()
                            {
                                Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]",
                                Value = x.EquipmentId.ToString()
                            }).ToList());
            }
            //EquipmentList.AddRange(ViewBag.EquipmentList = _Util.Facade.EquipmentFacade
            //  .GetAllEquipmentsByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
            //                 new SelectListItem()
            //                 {
            //                     Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]",
            //                     Value = x.EquipmentId.ToString()
            //                 }).ToList());
            Equipment equiments = null;
            if (Id.HasValue && Id > 0)
            {
                model = _Util.Facade.PackageFacade.GetPackageIncludeByIdAndCompanyId(Id.Value, CurrentUser.CompanyId.Value);
                equiments = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(model.EquipmentId, CurrentUser.CompanyId.Value);
                var equipClass = _Util.Facade.EquipmentFacade.GetAllEquipmentClassByCompanyId(CurrentUser.CompanyId.Value);
                if (equipClass.Count > 0)
                {
                    equiments.EquipmentClass = equipClass.Where(x => x.Id == equiments.EquipmentClassId).FirstOrDefault().Name;
                }
                else
                {
                    equiments.EquipmentClass = "";
                }
            }
            if (equiments != null)
            {
                var manufacturer = _Util.Facade.ManufacturerFacade.GetById(equiments.ManufacturerId);
                if (manufacturer != null)
                {
                    EquipmentList.Add(new SelectListItem()
                    {
                        Text = manufacturer.Name.ToString() + " ( " + equiments.Name.ToString() + " )",
                        Value = equiments.EquipmentId.ToString()
                    });
                }
                else
                {
                    EquipmentList.Add(new SelectListItem()
                    {
                        Text = "( " + equiments.Name.ToString() + " )",
                        Value = equiments.EquipmentId.ToString()
                    });
                }
            }
            EquipmentList = EquipmentList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            ViewBag.EquipmentList = EquipmentList;
            #endregion

            return PartialView("_AddCompanyPackageIncludePartial", model);
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddCompanyPackageInclude(PackageInclude _PackageInclude)
        {
            var result = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            _PackageInclude.CompanyId = CurrentUser.CompanyId.Value;
            _PackageInclude.IsFree = true;
            if (CurrentUser == null)
            {
                return Json(result);
            }
            PackageInclude packageInclude = _Util.Facade.PackageFacade.GetPackageIncludeByPackageIdAndCompanyIdAndEquipmentId(_PackageInclude.PackageId, CurrentUser.CompanyId.Value, _PackageInclude.EquipmentId);
            if (packageInclude != null)
            {
                return Json(result);
            }
            if (_PackageInclude.Id > 0)
            {
                var OldPackage = _Util.Facade.PackageFacade.GetPackageIncludeByIdAndCompanyId(_PackageInclude.Id, _PackageInclude.CompanyId);
                if (OldPackage != null)
                {
                    OldPackage.PackageId = _PackageInclude.PackageId;
                    OldPackage.EquipmentId = _PackageInclude.EquipmentId;
                    OldPackage.EptNo = _PackageInclude.EptNo;
                    result = _Util.Facade.PackageFacade.UpdatePackageInclude(OldPackage);
                }
            }
            else
            {
                result = _Util.Facade.PackageFacade.InsertPackageInclude(_PackageInclude) > 0;
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeletePackageInclude(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.PackageFacade.DeletePackageInclude(id.Value);
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult LoadCompanyPackageDeviceSettings()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView("LoadCompanyPackageDeviceSettings");
        }

        [Authorize]
        public ActionResult AddCompanyPackageDevicePartial(int? Id, int? packageid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            PackageDevice model = new PackageDevice();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #region PackageList in viewbag
            List<SelectListItem> PackageList = new List<SelectListItem>();
            PackageList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            PackageList.AddRange(ViewBag.PackageList = _Util.Facade.PackageFacade
              .GetAllPackageByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.Name.ToString(),
                                 Value = x.Id.ToString()
                             }).ToList());
            PackageList = PackageList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            ViewBag.PackageList = PackageList;
            #endregion
            #region EquipmentList In viewbag
            List<SelectListItem> EquipmentList = new List<SelectListItem>();
            EquipmentList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            if (packageid.HasValue && packageid > 0)
            {
                EquipmentList.AddRange(_Util.Facade.EquipmentFacade
          .GetAllEquipmentsForOptByCompanyId(CurrentUser.CompanyId.Value, packageid.Value).Select(x =>
                         new SelectListItem()
                         {
                             Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]",
                             Value = x.EquipmentId.ToString()
                         }).ToList());
            }
            //EquipmentList.AddRange(ViewBag.EquipmentList = _Util.Facade.EquipmentFacade
            //  .GetAllEquipmentsByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
            //                 new SelectListItem()
            //                 {
            //                     Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]",
            //                     Value = x.EquipmentId.ToString()
            //                 }).ToList());
            Equipment equiments = null;
            if (Id.HasValue && Id > 0)
            {
                model = _Util.Facade.PackageFacade.GetPackageDeviceByIdAndCompanyId(Id.Value, CurrentUser.CompanyId.Value);
                equiments = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(model.EquipmentId, CurrentUser.CompanyId.Value);
                var equipClass = _Util.Facade.EquipmentFacade.GetAllEquipmentClassByCompanyId(CurrentUser.CompanyId.Value);
                if (equipClass.Count > 0)
                {
                    equiments.EquipmentClass = equipClass.Where(x => x.Id == equiments.EquipmentClassId).FirstOrDefault().Name;
                }
                else
                {
                    equiments.EquipmentClass = "";
                }
            }
            if (equiments != null)
            {
                var manufacturer = _Util.Facade.ManufacturerFacade.GetById(equiments.ManufacturerId);
                if (manufacturer != null)
                {
                    EquipmentList.Add(new SelectListItem()
                    {
                        Text = manufacturer.Name.ToString() + " ( " + equiments.Name.ToString() + " )",
                        Value = equiments.EquipmentId.ToString()
                    });
                }
                else
                {
                    EquipmentList.Add(new SelectListItem()
                    {
                        Text = "( " + equiments.Name.ToString() + " )",
                        Value = equiments.EquipmentId.ToString()
                    });
                }
            }
            EquipmentList = EquipmentList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            ViewBag.EquipmentList = EquipmentList;
            #endregion

            return PartialView("_AddCompanyPackageDevicePartial", model);
        }

        [HttpPost]
        [Authorize]
        public JsonResult AddCompanyPackageDevice(PackageDevice _PackageDevice)
        {
            var result = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            _PackageDevice.CompanyId = CurrentUser.CompanyId.Value;
            _PackageDevice.IsFree = true;
            if (CurrentUser == null)
            {
                return Json(result);
            }
            PackageDevice packageDevice = _Util.Facade.PackageFacade.GetPackageDeviceByPackageIdAndCompanyIdAndEquipmentId(_PackageDevice.PackageId, CurrentUser.CompanyId.Value, _PackageDevice.EquipmentId);
            if (packageDevice != null)
            {
                return Json(result);
            }
            if (_PackageDevice.Id > 0)
            {
                var OldPackage = _Util.Facade.PackageFacade.GetPackageDeviceByIdAndCompanyId(_PackageDevice.Id, _PackageDevice.CompanyId);
                if (OldPackage != null)
                {
                    OldPackage.PackageId = _PackageDevice.PackageId;
                    OldPackage.EquipmentId = _PackageDevice.EquipmentId;
                    OldPackage.EptNo = _PackageDevice.EptNo;
                    result = _Util.Facade.PackageFacade.UpdatePackageDevice(OldPackage);
                }
            }
            else
            {
                result = _Util.Facade.PackageFacade.InsertPackageDevice(_PackageDevice) > 0;
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult CompanyPackageDeviceListPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<PackageDevice> model = new List<PackageDevice>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.PackageFacade.GetAllPackageDeviceProducts(CurrentUser.CompanyId.Value);
            }
            return PartialView("_CompanyPackageDeviceListPartial", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeletePackageDevice(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.PackageFacade.DeletePackageDevice(id.Value);
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult LoadCompanyPackageOptionalSettings()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView("LoadCompanyPackageOptionalSettings");
        }

        [Authorize]
        public ActionResult AddCompanyPackageOptionalPartial(int? Id, int? packageid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            PackageOptional model = new PackageOptional();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            #region PackageList in viewbag
            List<SelectListItem> PackageList = new List<SelectListItem>();
            PackageList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            PackageList.AddRange(ViewBag.PackageList = _Util.Facade.PackageFacade
              .GetAllPackageByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.Name.ToString(),
                                 Value = x.Id.ToString()
                             }).ToList());
            PackageList = PackageList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            ViewBag.PackageList = PackageList;
            #endregion
            #region EquipmentList In viewbag
            List<SelectListItem> EquipmentList = new List<SelectListItem>();
            EquipmentList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            //EquipmentList.AddRange(ViewBag.EquipmentList = _Util.Facade.EquipmentFacade
            //  .GetAllEquipmentsForOptByCompanyId(CurrentUser.CompanyId.Value).Select(x =>
            //                 new SelectListItem()
            //                 {
            //                     Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]",
            //                     Value = x.EquipmentId.ToString()
            //                 }).ToList());
            if (packageid.HasValue && packageid > 0)
            {
                EquipmentList.AddRange(_Util.Facade.EquipmentFacade
           .GetAllEquipmentsForOptByCompanyId(CurrentUser.CompanyId.Value, packageid.Value).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]",
                              Value = x.EquipmentId.ToString()
                          }).ToList());
            }
            Equipment equiments = null;
            if (Id.HasValue && Id > 0)
            {
                model = _Util.Facade.PackageFacade.GetPackageOptionalByIdAndCompanyId(Id.Value, CurrentUser.CompanyId.Value);
                equiments = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(model.EquipmentId, CurrentUser.CompanyId.Value);
                var equipClass = _Util.Facade.EquipmentFacade.GetAllEquipmentClassByCompanyId(CurrentUser.CompanyId.Value);
                if (equipClass.Count > 0)
                {
                    equiments.EquipmentClass = equipClass.Where(x => x.Id == equiments.EquipmentClassId).FirstOrDefault().Name;
                }
                else
                {
                    equiments.EquipmentClass = "";
                }
            }
            if (equiments != null)
            {
                var manufacturer = _Util.Facade.ManufacturerFacade.GetById(equiments.ManufacturerId);
                if (manufacturer != null)
                {
                    EquipmentList.Add(new SelectListItem()
                    {
                        Text = manufacturer.Name.ToString() + " ( " + equiments.Name.ToString() + " )",
                        Value = equiments.EquipmentId.ToString()
                    });
                }
                else
                {
                    EquipmentList.Add(new SelectListItem()
                    {
                        Text = "( " + equiments.Name.ToString() + " )",
                        Value = equiments.EquipmentId.ToString()
                    });
                }
            }

            ViewBag.EquipmentList = EquipmentList.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            #endregion

            return PartialView("_AddCompanyPackageOptionalPartial", model);
        }

        public JsonResult LoadEquipmentAndService(int id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SelectListItem> equipmentList = new List<SelectListItem>();
            //equipmentList.Add(new SelectListItem()
            //{
            //    Text = "Please Select",
            //    Value = "-1"
            //});
            equipmentList.AddRange(_Util.Facade.EquipmentFacade
              .GetAllEquipmentsForOptByCompanyId(CurrentUser.CompanyId.Value, id).Where(x => x.Name != "").Select(x =>
                               new SelectListItem()
                               {
                                   Text = x.Name.ToString() + " [ " + x.EquipmentClass.ToString() + " ]",
                                   Value = x.EquipmentId.ToString()
                               }).ToList());
            return Json(equipmentList);
        }
        public ActionResult LoadEquipmentAndServiceSearch(int id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string query = Request.QueryString["q[term]"] != null ? Request.QueryString["q[term]"].ToString() : "";
            var equipmentList = _Util.Facade.EquipmentFacade
              .GetAllEquipmentsForOptByCompanyId(CurrentUser.CompanyId.Value, id, query);
            List<Dictionary<string, object>>
             lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in equipmentList.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in equipmentList.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }

            return Json(lstRows, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        public JsonResult AddCompanyPackageOptional(PackageOptional _PackageOptional)
        {
            var result = false;

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            _PackageOptional.CompanyId = CurrentUser.CompanyId.Value;
            _PackageOptional.IsFree = false;
            if (CurrentUser == null)
            {
                return Json(result);
            }
            PackageOptional packageOptional = _Util.Facade.PackageFacade.GetPackageOptionalByPackageIdAndCompanyIdAndEquipmentId(_PackageOptional.PackageId, CurrentUser.CompanyId.Value, _PackageOptional.EquipmentId);
            if (packageOptional != null)
            {
                return Json(result);
            }
            if (_PackageOptional.Id > 0)
            {
                var OldPackage = _Util.Facade.PackageFacade.GetPackageOptionalByIdAndCompanyId(_PackageOptional.Id, _PackageOptional.CompanyId);
                if (OldPackage != null)
                {
                    OldPackage.PackageId = _PackageOptional.PackageId;
                    OldPackage.EquipmentId = _PackageOptional.EquipmentId;
                    result = _Util.Facade.PackageFacade.UpdatePackageOptional(OldPackage);
                }
            }
            else
            {
                result = _Util.Facade.PackageFacade.InsertPackageOptional(_PackageOptional) > 0;
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult CompanyPackageOptionalListPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<PackageOptional> model = new List<PackageOptional>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.PackageFacade.GetAllPackageOptionalProducts(CurrentUser.CompanyId.Value);
            }
            return PartialView("_CompanyPackageOptionalListPartial", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeletePackageOptional(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.PackageFacade.DeletePackageOptional(id.Value);
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult LoadCustomerSignatureImage(string data, string LeadConvertId, string templateid, bool? firstpage, bool? recreate, bool? isinvoice, string invoiceid)
        {
            Guid CompanyId = new Guid();
            if (User.Identity.IsAuthenticated)
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CompanyId = CurrentUser.CompanyId.Value;
            }
            else
            {
                CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(int.Parse(LeadConvertId));
                CompanyId = custommerCompany.CompanyId;
            }

            bool uploadImage = false;
            string filePath = "";
            var leadID = LeadConvertId;
            string[] datasplit = data.Split(',');
            byte[] bytes = Convert.FromBase64String(datasplit[1]);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                string tempFolder = ConfigurationManager.AppSettings["File.CustomerSignatureFile"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                var FtempFolderName = string.Format(tempFolder, comname) + leadID + "Signature";
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
            int leadid = Convert.ToInt32(leadID);
            //bool rsult = _Util.Facade.CustomerFacade.CustomerIsInCompany(leadid, CompanyId);
            //if (!rsult)
            //{

            //    if (System.IO.File.Exists(serverFile))
            //    {
            //        System.IO.File.Delete(serverFile);
            //    }
            //    return Json(new { result = true, message = "invalid user" });
            //}
            if (!System.IO.File.Exists(serverFile))
            {
                return Json(new { result = true, message = "File not exsists" });
            }
            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetById(leadid);
            if (firstpage.HasValue && firstpage.Value == true)
            {
                CustomerSignature cs = new CustomerSignature()
                {
                    CustomerId = tmpCustomer.CustomerId,
                    ReferenceIdGuid = Guid.Empty,
                    ReferenceIdnvarchar = templateid,
                    Type = "First Page",
                    Signature = filePath,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CreatedBy = tmpCustomer.CustomerId
                };
                _Util.Facade.CustomerSignatureFacade.InsertCustomerSignature(cs);
            }
            else if (isinvoice.HasValue && isinvoice.Value == true)
            {
                CustomerSignature cs = new CustomerSignature()
                {
                    CustomerId = tmpCustomer.CustomerId,
                    ReferenceIdGuid = Guid.Empty,
                    ReferenceIdnvarchar = templateid,
                    Type = "Estimate",
                    Signature = filePath,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CreatedBy = tmpCustomer.CustomerId
                };
                _Util.Facade.CustomerSignatureFacade.InsertCustomerSignature(cs);
            }
            else if (recreate.HasValue && recreate.Value == true)
            {
                CustomerSignature cs = new CustomerSignature()
                {
                    CustomerId = tmpCustomer.CustomerId,
                    ReferenceIdGuid = Guid.Empty,
                    ReferenceIdnvarchar = templateid,
                    Type = "Recreate",
                    Signature = filePath,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CreatedBy = tmpCustomer.CustomerId
                };
                _Util.Facade.CustomerSignatureFacade.InsertCustomerSignature(cs);
            }
            else if (tmpCustomer != null && !string.IsNullOrWhiteSpace(templateid) && templateid != "0")
            {
                CustomerSignature cs = new CustomerSignature()
                {
                    CustomerId = tmpCustomer.CustomerId,
                    ReferenceIdGuid = Guid.Empty,
                    ReferenceIdnvarchar = templateid,
                    Type = "Agreement File",
                    Signature = filePath,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CreatedBy = tmpCustomer.CustomerId
                };
                _Util.Facade.CustomerSignatureFacade.InsertCustomerSignature(cs);
            }
            else if (tmpCustomer != null)
            {
                tmpCustomer.Singature = filePath;
                tmpCustomer.CustomerSignatureDate = DateTime.Now.UTCCurrentTime();
                _Util.Facade.CustomerFacade.UpdateCustomer(tmpCustomer);
            }
            else
            {
                uploadImage = false;
                return Json(new { uploadImage = uploadImage, message = "Invalid customer." });
            }

            if (uploadImage == true)
            {
                var objCusAgree = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByComIdAndCusIsAndSignAgreement(CompanyId, tmpCustomer.CustomerId);
                if (objCusAgree != null)
                {
                    if (objCusAgree.Id > 0)
                    {
                        objCusAgree.CompanyId = CompanyId;
                        objCusAgree.CustomerId = tmpCustomer.CustomerId;
                        objCusAgree.AddedDate = DateTime.UtcNow;
                        _Util.Facade.CustomerAgreementFacade.UpdateCustomerAgreement(objCusAgree);
                    }
                }
                if (objCusAgree == null)
                {
                    CustomerAgreement objCustomerAgreement = new CustomerAgreement()
                    {
                        CompanyId = CompanyId,
                        CustomerId = tmpCustomer.CustomerId,
                        IP = AppConfig.GetIP,
                        UserAgent = AppConfig.GetUserAgent,
                        Type = LabelHelper.CustomerAgreementLog.SignAgreement,
                        AddedDate = DateTime.UtcNow
                    };
                    _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objCustomerAgreement);
                }
            }
            return Json(new { uploadImage = uploadImage, UploadFilePath = AppConfig.DomainSitePath + filePath, LeadID = leadID }, "text/html");
        }

        [HttpPost]
        public JsonResult LoadCancelCustomerSignatureImage(string data, string LeadConvertId)
        {
            Guid CompanyId = new Guid();
            if (User.Identity.IsAuthenticated)
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CompanyId = CurrentUser.CompanyId.Value;
            }
            else
            {
                CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(int.Parse(LeadConvertId));
                CompanyId = custommerCompany.CompanyId;
            }

            bool uploadImage = false;
            string filePath = "";
            var leadID = LeadConvertId;
            string[] datasplit = data.Split(',');
            byte[] bytes = Convert.FromBase64String(datasplit[1]);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                string tempFolder = ConfigurationManager.AppSettings["File.CustomerSignatureFile"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                var FtempFolderName = string.Format(tempFolder, comname) + leadID + "Signature";
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
            int leadid = Convert.ToInt32(leadID);
            //bool rsult = _Util.Facade.CustomerFacade.CustomerIsInCompany(leadid, CompanyId);
            //if (!rsult)
            //{

            //    if (System.IO.File.Exists(serverFile))
            //    {
            //        System.IO.File.Delete(serverFile);
            //    }
            //    return Json(new { result = true, message = "invalid user" });
            //}
            if (!System.IO.File.Exists(serverFile))
            {
                return Json(new { result = true, message = "File not exsists" });
            }
            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetById(leadid);
            if (tmpCustomer != null)
            {
                tmpCustomer.CancellationSignature = filePath;
            }
            bool rsult = _Util.Facade.CustomerFacade.UpdateCustomer(tmpCustomer);

            return Json(new { uploadImage = uploadImage, UploadFilePath = AppConfig.DomainSitePath + filePath, LeadID = leadID }, "text/html");
        }
        #region Private Func

        private bool IAgreeConvertLeadToCustomer(Customer _Customer, Company company)
        {
            var result = false;


            var LeadConvertResult = _Util.Facade.CustomerFacade.ConvertLeadToCustomer(_Customer.CustomerId, company.CompanyId, LabelHelper.LeadConvertionType.System);

            if (LeadConvertResult != false)
            {
                result = LeadConvertResult;
                var CustomerdB = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(_Customer.CustomerId);
                if (CustomerdB != null)
                {
                    CustomerdB.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    CustomerdB.LastUpdatedBy = User.Identity.Name;
                    CustomerdB.IsTechCallPassed = false;
                    CustomerdB.IsDirect = false;
                    CustomerdB.IsActive = true;
                    //CustomerdB.CreatedDate = DateTime.Now.UTCCurrentTime();
                    CustomerdB.IsAgreement = true;
                    CustomerdB.JoinDate = DateTime.Now.UTCCurrentTime();
                    result = _Util.Facade.CustomerFacade.UpdateCustomer(CustomerdB);
                }
            }

            CustomerSnapshot ObjCutomerSnapshot = new CustomerSnapshot
            {
                CustomerId = _Customer.CustomerId,
                CompanyId = company.CompanyId,
                Description = "ConvertLeadToCustomer",
                Logdate = DateTime.Now.UTCCurrentTime(),
                Updatedby = User.Identity.Name
            };

            _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(ObjCutomerSnapshot);
            return result;
        }
        public bool IAgreeLeadtoCustomerConvertQAEmail(Customer leadDetails, Company _Company)
        {

            bool result = true;

            List<Employee> EmployeeDetails = _Util.Facade.EmployeeFacade.GetAllQAEmployee(_Company.CompanyId);
            //var leadDetails = _Util.Facade.CustomerFacade.GetCustomerByLeadId(_Customer.Id);
            if (EmployeeDetails.Count > 0)
            {
                LeadtoCustomer leadtocus = new LeadtoCustomer();

                leadtocus.CustomerName = leadDetails.FirstName + " " + leadDetails.LastName;
                if (leadDetails.MiddleName != null)
                {
                    leadtocus.CustomerName = leadDetails.FirstName + " " + leadDetails.MiddleName + " " + leadDetails.LastName;
                }
                leadtocus.CustomerAddress = leadDetails.Address;

                foreach (var item in EmployeeDetails)
                {

                    leadtocus.EmployeeName = item.FirstName + " " + item.LastName;
                    leadtocus.ToEmail = item.UserName;
                    _Util.Facade.MailFacade.EmailToQALeadtoCusConverFor(leadtocus, _Company.CompanyId);
                }
            }

            return result;
        }
        private bool IAgreeCustomerFileUpload_V2(Customer Cus, Company Com)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            DateTime FixDate = DateTime.Now.UTCCurrentTime();
            int? Id;
            Id = Cus.Id;
            //int idlead = Convert.ToInt32(Lid);
            InstallationAgreementModel Model = new InstallationAgreementModel();
            Model.EmergencyContactList = new List<EmergencyContact>();
            Model.ListAgreementAnswer = new List<AgreementAnswer>();
            Model.EquipmentList = new List<Equipment>();
            Model.CustomerAgreement = new List<CustomerAgreement>();
            Model.SingleCustomerAgreement = new CustomerAgreement();
            string ContractTerm = "";
            if (!string.IsNullOrWhiteSpace(Cus.ContractTeam) && Cus.ContractTeam != "-1")
            {
                ContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))).ToString();
            }
            var EquipmentTotalPrice = 0.0;
            var AgreementSubtotal = 0.0;
            var AgreementTotal = 0.0;
            var AgreementTax = 0.0;
            var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(Cus.CustomerId, Com.CompanyId);
            if (GetCityTaxList.Count > 0)
            {
                foreach (var item1 in GetCityTaxList)
                {
                    AgreementTax = item1.Rate;
                }
            }
            else
            {
                Guid CustomerId = new Guid();
                if (Cus != null)
                {
                    CustomerId = Cus.CustomerId;

                }
                var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(Com.CompanyId, CustomerId);
                if (GetSalesTax != null)
                {
                    AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                }
            }
            var CustomEquipmentList = _Util.Facade.EquipmentFacade.GetEquipmentListByCustomerIdAndCompanyId(Id.Value, Com.CompanyId);
            if (CustomEquipmentList.Count > 0)
            {
                foreach (var item in CustomEquipmentList)
                {
                    EquipmentTotalPrice += item.TotalPrice;
                }
                if (Cus.ActivationFee.HasValue)
                {
                    AgreementSubtotal = Cus.ActivationFee.Value + EquipmentTotalPrice;
                }
                else
                {
                    AgreementSubtotal = EquipmentTotalPrice;
                }
            }
            if (AgreementTax != 0.0)
            {
                var taxtotal = (AgreementSubtotal / 100) * AgreementTax;
                AgreementTotal = AgreementSubtotal + taxtotal;
            }
            else
            {
                AgreementTotal = AgreementSubtotal;
            }
            var objCusAgree = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByComIdAndCusIsAndSubmitAgreement(Com.CompanyId, Cus.CustomerId);
            if (objCusAgree == null)
            {
                CustomerAgreement objCustomerAgreement = new CustomerAgreement()
                {
                    CompanyId = Com.CompanyId,
                    CustomerId = Cus.CustomerId,
                    IP = AppConfig.GetIP,
                    UserAgent = AppConfig.GetUserAgent,
                    Type = LabelHelper.CustomerAgreementLog.SubmitAgreement,
                    AddedDate = DateTime.UtcNow
                };
                _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objCustomerAgreement);
            }
            GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Com.CompanyId, "CompanySignature");

            #region LeadSource
            string LeadSource = "";
            Lookup leadsource = _Util.Facade.LookupFacade.GetLookupByKeyAndValueAndCompanyId("LeadSource", Cus.LeadSource, Com.CompanyId);
            LeadSource = leadsource != null && !string.IsNullOrWhiteSpace(leadsource.DisplayText) && leadsource.DataValue != "-1" ? leadsource.DisplayText : "";
            #endregion

            Model = new InstallationAgreementModel()
            {
                CSIDNumber = Cus.Id,
                LeadSource = LeadSource,
                BillingAddress = Cus.Street + " " + Cus.City + ", " + Cus.State + " " + Cus.ZipCode,
                OwnerAddress = Cus.Street + " " + Cus.City + ", " + Cus.State + " " + Cus.ZipCode,
                OwnerEmail = Cus.EmailAddress,
                OwnerPhone = Cus.PrimaryPhone,
                OwnerName = Cus.FirstName + " " + Cus.LastName,
                EmergencyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId),
                CompanyName = Com.CompanyName,
                CompanySate = string.Format("{0}, {1} {2}", Com.City, Com.State, Com.ZipCode),
                CompanyStreet = Com.Street,
                CompanyWebsite = Com.Website,
                CompanyPhone = Com.Phone,
                SubscribedMonths = ContractTerm,
                Password = Cus.Passcode,
                DateOfTransaction = FixDate.UTCToClientTime(),
                //CompanyLogo = _Util.Facade.GlobalSettingsFacade.GetCompanyColoredLogoByCompanyId(CurrentUser.CompanyId.Value),
                CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(Com.CompanyId),
                EquipmentList = _Util.Facade.EquipmentFacade.GetEquipmentListByCustomerIdAndCompanyId(Id.Value, Com.CompanyId),
                ActivationFee = Cus.ActivationFee.HasValue ? Cus.ActivationFee.Value : 0,
                BusinessName = Cus.BusinessName,
                MonthlyMonitoringFee = Cus.MonthlyMonitoringFee,
                EffectiveDate = FixDate.UTCToClientTime(),
                CustomerSignature = Cus.Singature,
                Subtotal = AgreementSubtotal,
                Tax = AgreementTax,
                Total = AgreementTotal,
                EContractId = Cus.Id,
                ListAgreementAnswer = _Util.Facade.AgreementFacade.GetAllAgreementAnswerByCustomerId(Cus.CustomerId),
                SalesRepresentative = !string.IsNullOrEmpty(Cus.Soldby) ? _Util.Facade.EmployeeFacade.GetEmployeeNumByEmployeeId(new Guid(Cus.Soldby)).ToString() : "",
                TotalPayments = (!string.IsNullOrWhiteSpace(Cus.MonthlyMonitoringFee) ? Convert.ToDouble(Cus.MonthlyMonitoringFee) : 1) * Convert.ToDouble(ContractTerm),
                SingleCustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetIpAndUserAgentByCustomerIdAndCompanyId(Com.CompanyId, Cus.CustomerId),
                CustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByCompanyIdAndCustomerId1(Com.CompanyId, Cus.CustomerId)
            };
            if (!string.IsNullOrWhiteSpace(Cus.Singature))
            {
                if (Cus.CustomerSignatureDate != null && Cus.CustomerSignatureDate != new DateTime())
                {
                    Model.CustomerSignatureStringDate = Cus.CustomerSignatureDate.Value.UTCToClientTime().ToString("M/dd/yy");
                }

                if (glbs != null && !string.IsNullOrWhiteSpace(glbs.Value))
                {
                    Model.CompanySignature = glbs.Value;
                    if (Cus.CustomerSignatureDate != null && Cus.CustomerSignatureDate != new DateTime())
                    {
                        Model.CompanySignatureDate = Cus.CustomerSignatureDate.Value.UTCToClientTime().ToString("M/dd/yy");
                    }
                }

            }
            if (Model.EquipmentList != null && Model.EquipmentList.Count() > 0)
            {
                Model.Subtotal = Model.EquipmentList.Sum(x => x.TotalPrice) + Model.ActivationFee;
                Guid CustomerId = new Guid();
                if (Cus != null)
                {
                    CustomerId = Cus.CustomerId;
                }
                var taxp = Convert.ToDouble(_Util.Facade.GlobalSettingsFacade.GetSalesTax(Com.CompanyId, CustomerId));
                Model.Tax = (Model.Subtotal * taxp) / 100;
                Model.Total = Model.Subtotal + Model.Tax;

            }
            else
            {
                Model.EquipmentList = new List<Equipment>();
            }

            if (Model.EmergencyContactList == null)
            {
                Model.EmergencyContactList = new List<EmergencyContact>();
            }


            ViewBag.CompanyId = Com.CompanyId.ToString();
            Model.CompanyId = Com.CompanyId.ToString();
            Model.CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(Com.CompanyId);
            string body = _Util.Facade.AgreementFacade.MakeAgreementPdf(Model);
            ViewBag.Body = body;
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("InstallationAgreement", Model)
            {
                //FileName = "TestView.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.LeadToCustomerAgreement"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Com.CompanyId).CompanyName.ReplaceSpecialChar();
            var pdftempFolderName = string.Format(filename, comname) + Id + "AgreementMail.pdf";
            string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
            FileHelper.SaveFile(applicationPDFData, Serverfilename);
            var cusinfo = _Util.Facade.CustomerFacade.GetById(Id.Value);
            string[] comNameSplit = comname.Split('/');
            SetupLeadCustormer slc = new SetupLeadCustormer
            {
                CustomerName = cusinfo.FirstName + " " + cusinfo.LastName,
                EmailBody = cusinfo.FirstName + " " + cusinfo.LastName,
                ToEmail = cusinfo.EmailAddress,
                CompanyName = Com.CompanyName,
                CustomerId = cusinfo.CustomerId.ToString(),
                EmployeeId = Com.CompanyId.ToString(),
                PdfAggrement = new Attachment(Serverfilename, MediaTypeNames.Application.Octet)
            };
            _Util.Facade.MailFacade.EmailToLeadSignAgreement(slc, Com.CompanyId);
            CustomerFile cfs = new CustomerFile()
            {
                FileDescription = Id + "_Agreement_Signed.pdf",
                FileId = Guid.NewGuid(),
                Filename = AppConfig.DomainSitePath + "/" + pdftempFolderName,
                FileFullName = Id + "Agreement_Signed.pdf",
                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                CustomerId = cusinfo.CustomerId,
                CompanyId = Com.CompanyId,
                IsActive = true,
                CreatedBy = CurrentUser.UserId,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                UpdatedBy = CurrentUser.UserId,
                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                WMStatus = LabelHelper.WatermarkStatus.Pending,
                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
            };
            _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);

            return true;
        }

        private bool IAgreeCustomerFileUpload(Customer Cus, Company Com)
        {
            WebClient webClient;
            byte[] fileBytes1;
            string Temp_FileName;


            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            DateTime FixDate = DateTime.Now.UTCCurrentTime();
            int? Id;
            Id = Cus.Id;
            //int idlead = Convert.ToInt32(Lid);
            InstallationAgreementModel Model = new InstallationAgreementModel();
            Model.EmergencyContactList = new List<EmergencyContact>();
            Model.ListAgreementAnswer = new List<AgreementAnswer>();
            Model.EquipmentList = new List<Equipment>();
            Model.CustomerAgreement = new List<CustomerAgreement>();
            Model.SingleCustomerAgreement = new CustomerAgreement();
            string ContractTerm = "";
            if (!string.IsNullOrWhiteSpace(Cus.ContractTeam) && Cus.ContractTeam != "-1")
            {
                ContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))).ToString();
            }
            var EquipmentTotalPrice = 0.0;
            var AgreementSubtotal = 0.0;
            var AgreementTotal = 0.0;
            var AgreementTax = 0.0;
            var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(Cus.CustomerId, Com.CompanyId);
            if (GetCityTaxList.Count > 0)
            {
                foreach (var item1 in GetCityTaxList)
                {
                    AgreementTax = item1.Rate;
                }
            }
            else
            {
                Guid CustomerId = new Guid();
                if (Cus != null)
                {
                    CustomerId = Cus.CustomerId;

                }
                var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(Com.CompanyId, CustomerId);
                if (GetSalesTax != null)
                {
                    AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                }
            }
            var CustomEquipmentList = _Util.Facade.EquipmentFacade.GetEquipmentListByCustomerIdAndCompanyId(Id.Value, Com.CompanyId);
            if (CustomEquipmentList.Count > 0)
            {
                foreach (var item in CustomEquipmentList)
                {
                    EquipmentTotalPrice += item.TotalPrice;
                }
                if (Cus.ActivationFee.HasValue)
                {
                    AgreementSubtotal = Cus.ActivationFee.Value + EquipmentTotalPrice;
                }
                else
                {
                    AgreementSubtotal = EquipmentTotalPrice;
                }
            }
            if (AgreementTax != 0.0)
            {
                var taxtotal = (AgreementSubtotal / 100) * AgreementTax;
                AgreementTotal = AgreementSubtotal + taxtotal;
            }
            else
            {
                AgreementTotal = AgreementSubtotal;
            }
            var objCusAgree = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByComIdAndCusIsAndSubmitAgreement(Com.CompanyId, Cus.CustomerId);
            if (objCusAgree == null)
            {
                CustomerAgreement objCustomerAgreement = new CustomerAgreement()
                {
                    CompanyId = Com.CompanyId,
                    CustomerId = Cus.CustomerId,
                    IP = AppConfig.GetIP,
                    UserAgent = AppConfig.GetUserAgent,
                    Type = LabelHelper.CustomerAgreementLog.SubmitAgreement,
                    AddedDate = DateTime.UtcNow
                };
                _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objCustomerAgreement);
            }
            GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Com.CompanyId, "CompanySignature");

            #region LeadSource
            string LeadSource = "";
            Lookup leadsource = _Util.Facade.LookupFacade.GetLookupByKeyAndValueAndCompanyId("LeadSource", Cus.LeadSource, Com.CompanyId);
            LeadSource = leadsource != null && !string.IsNullOrWhiteSpace(leadsource.DisplayText) && leadsource.DataValue != "-1" ? leadsource.DisplayText : "";
            #endregion

            Model = new InstallationAgreementModel()
            {
                CSIDNumber = Cus.Id,
                LeadSource = LeadSource,
                BillingAddress = Cus.Street + " " + Cus.City + ", " + Cus.State + " " + Cus.ZipCode,
                OwnerAddress = Cus.Street + " " + Cus.City + ", " + Cus.State + " " + Cus.ZipCode,
                OwnerEmail = Cus.EmailAddress,
                OwnerPhone = Cus.PrimaryPhone,
                OwnerName = Cus.FirstName + " " + Cus.LastName,
                EmergencyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId),
                CompanyName = Com.CompanyName,
                CompanySate = string.Format("{0}, {1} {2}", Com.City, Com.State, Com.ZipCode),
                CompanyStreet = Com.Street,
                CompanyWebsite = Com.Website,
                CompanyPhone = Com.Phone,
                SubscribedMonths = ContractTerm,
                Password = Cus.Passcode,
                DateOfTransaction = FixDate.UTCToClientTime(),
                //CompanyLogo = _Util.Facade.GlobalSettingsFacade.GetCompanyColoredLogoByCompanyId(CurrentUser.CompanyId.Value),
                CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(Com.CompanyId),
                EquipmentList = _Util.Facade.EquipmentFacade.GetEquipmentListByCustomerIdAndCompanyId(Id.Value, Com.CompanyId),
                ActivationFee = Cus.ActivationFee.HasValue ? Cus.ActivationFee.Value : 0,
                BusinessName = Cus.BusinessName,
                MonthlyMonitoringFee = Cus.MonthlyMonitoringFee,
                EffectiveDate = FixDate.UTCToClientTime(),
                CustomerSignature = Cus.Singature,
                Subtotal = AgreementSubtotal,
                Tax = AgreementTax,
                Total = AgreementTotal,
                EContractId = Cus.Id,
                ListAgreementAnswer = _Util.Facade.AgreementFacade.GetAllAgreementAnswerByCustomerId(Cus.CustomerId),
                SalesRepresentative = !string.IsNullOrEmpty(Cus.Soldby) ? _Util.Facade.EmployeeFacade.GetEmployeeNumByEmployeeId(new Guid(Cus.Soldby)).ToString() : "",
                TotalPayments = (!string.IsNullOrWhiteSpace(Cus.MonthlyMonitoringFee) ? Convert.ToDouble(Cus.MonthlyMonitoringFee) : 1) * Convert.ToDouble(ContractTerm),
                SingleCustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetIpAndUserAgentByCustomerIdAndCompanyId(Com.CompanyId, Cus.CustomerId),
                CustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByCompanyIdAndCustomerId1(Com.CompanyId, Cus.CustomerId)
            };
            if (!string.IsNullOrWhiteSpace(Cus.Singature))
            {
                if (Cus.CustomerSignatureDate != null && Cus.CustomerSignatureDate != new DateTime())
                {
                    Model.CustomerSignatureStringDate = Cus.CustomerSignatureDate.Value.UTCToClientTime().ToString("M/dd/yy");
                }

                if (glbs != null && !string.IsNullOrWhiteSpace(glbs.Value))
                {
                    Model.CompanySignature = glbs.Value;
                    if (Cus.CustomerSignatureDate != null && Cus.CustomerSignatureDate != new DateTime())
                    {
                        Model.CompanySignatureDate = Cus.CustomerSignatureDate.Value.UTCToClientTime().ToString("M/dd/yy");
                    }
                }

            }
            if (Model.EquipmentList != null && Model.EquipmentList.Count() > 0)
            {
                Model.Subtotal = Model.EquipmentList.Sum(x => x.TotalPrice) + Model.ActivationFee;
                Guid CustomerId = new Guid();
                if (Cus != null)
                {
                    CustomerId = Cus.CustomerId;
                }
                var taxp = Convert.ToDouble(_Util.Facade.GlobalSettingsFacade.GetSalesTax(Com.CompanyId, CustomerId));
                Model.Tax = (Model.Subtotal * taxp) / 100;
                Model.Total = Model.Subtotal + Model.Tax;

            }
            else
            {
                Model.EquipmentList = new List<Equipment>();
            }

            if (Model.EmergencyContactList == null)
            {
                Model.EmergencyContactList = new List<EmergencyContact>();
            }


            ViewBag.CompanyId = Com.CompanyId.ToString();
            Model.CompanyId = Com.CompanyId.ToString();
            Model.CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(Com.CompanyId);
            string body = _Util.Facade.AgreementFacade.MakeAgreementPdf(Model);
            ViewBag.Body = body;
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("InstallationAgreement", Model)
            {
                //FileName = "TestView.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);

            #region File Save
            //Random rand = new Random();
            //string filename = ConfigurationManager.AppSettings["File.LeadToCustomerAgreement"];
            //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Com.CompanyId).CompanyName.ReplaceSpecialChar();
            //var pdftempFolderName = string.Format(filename, comname) + Id + "AgreementMail.pdf";
            //string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
            //FileHelper.SaveFile(applicationPDFData, Serverfilename);
            #endregion

            //// "mayur" AWS S3 Changes //// Start

            #region File Save on AWS S3

            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.LeadToCustomerAgreement"];
            filename = filename.TrimEnd('/');

            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Com.CompanyId).CompanyName.ReplaceSpecialChar();

            string FilePath = string.Format(filename, comname);
            string FileName = rand.Next().ToString() + Id + "AgreementMail.pdf";

            string FileKey = string.Format($"{FilePath}/{FileName}");

            var returnurl = "";

            var task = Task.Run(async () =>
            {
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

            decimal _fileSize = (decimal)applicationPDFData.Length / 1024;
            _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);

            #endregion

            //// "mayur" AWS S3 Changes //// End

            /// Mayur :: File Download to temp folder :start

            webClient = new WebClient();
            fileBytes1 = webClient.DownloadData(returnurl);

            File(fileBytes1, System.Net.Mime.MediaTypeNames.Application.Octet, FileName).ToString();


            Temp_FileName = Server.MapPath("~/EmailFileCache/tmp_" + FileName);

            if (!System.IO.File.Exists(Temp_FileName))
            {
                System.IO.File.WriteAllBytes(Temp_FileName, fileBytes1);
            }
            else
            {
                System.IO.File.WriteAllBytes(Temp_FileName, fileBytes1);
            }

            /// Mayur :: File Download to temp folder :End


            var cusinfo = _Util.Facade.CustomerFacade.GetById(Id.Value);
            string[] comNameSplit = comname.Split('/');
            SetupLeadCustormer slc = new SetupLeadCustormer
            {
                CustomerName = cusinfo.FirstName + " " + cusinfo.LastName,
                EmailBody = cusinfo.FirstName + " " + cusinfo.LastName,
                ToEmail = cusinfo.EmailAddress,
                CompanyName = Com.CompanyName,
                CustomerId = cusinfo.CustomerId.ToString(),
                EmployeeId = Com.CompanyId.ToString(),
                PdfAggrement = new Attachment(Temp_FileName, MediaTypeNames.Application.Octet)
            };
            _Util.Facade.MailFacade.EmailToLeadSignAgreement(slc, Com.CompanyId);


            //// ""Mayur" Calculate File Size : start
            #region Calculate file size

            _fileSize = applicationPDFData.Length / 1024;
            _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);

            #endregion
            //// ""Mayur" Calculate File Size : End


            CustomerFile cfs = new CustomerFile()
            {
                FileDescription = Id + "_Agreement_Signed.pdf",
                FileId = Guid.NewGuid(),
                FileSize = (double)_fileSize,
                Filename = "/" + FileKey,
                FileFullName = Id + "Agreement_Signed.pdf",
                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                CustomerId = cusinfo.CustomerId,
                CompanyId = Com.CompanyId,
                IsActive = true,
                CreatedBy = CurrentUser.UserId,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                UpdatedBy = CurrentUser.UserId,
                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                WMStatus = LabelHelper.WatermarkStatus.Pending,
                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
            };
            _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);

            return true;
        }
        private bool IAgreeAllScheduleCalendar(Customer LSchedule, Company _Company)
        {
            ScheduleCalendarList model = new ScheduleCalendarList();
            Schedule objScheduleCalendar = new Schedule();

            //int leadidvalue = Convert.ToInt32(LeadScheduleId);
            //var LSchedule = _Util.Facade.CustomerFacade.GetCustomersById(leadidvalue);
            if (LSchedule != null)
            {
                if (!string.IsNullOrWhiteSpace(LSchedule.Installer))
                {
                    Guid ScheduleTechid = new Guid(LSchedule.Installer);
                    var ScheduleCustomerappointment = _Util.Facade.CustomerAppoinmentFacade.GetAllAppointmentByCustomerId(LSchedule.CustomerId);
                    if (ScheduleCustomerappointment != null)
                    {
                        objScheduleCalendar.CompanyId = _Company.CompanyId;
                        objScheduleCalendar.Type = ScheduleCustomerappointment.AppointmentType;
                        objScheduleCalendar.Title = ScheduleCustomerappointment.Notes;
                        objScheduleCalendar.IsCompleted = false;
                        objScheduleCalendar.LeadId = LSchedule.Id;
                        objScheduleCalendar.IsFullDay = true;
                        objScheduleCalendar.Identifier = ScheduleTechid.ToString();
                        objScheduleCalendar.StartDate = Convert.ToDateTime("01/01/0001");
                        objScheduleCalendar.EndDate = Convert.ToDateTime("01/01/0001");
                        _Util.Facade.ScheduleFacade.InsertSchedule(objScheduleCalendar);
                    }
                }
                if (LSchedule.CustomerId != null)
                {
                    var LeadobjName = LSchedule.FirstName + " " + LSchedule.LastName;
                    Schedule objQa1Schedule = new Schedule()
                    {
                        CompanyId = _Company.CompanyId,
                        Type = ScheduleType.QA1,
                        StartDate = (DateTime.Now.UTCCurrentTime()).AddDays(1),
                        EndDate = (DateTime.Now.UTCCurrentTime()).AddDays(2),
                        Title = ScheduleTitle.QA1Required + " " + LeadobjName,
                        IsCompleted = false,
                        LeadId = LSchedule.Id,
                        IsFullDay = true,
                        Identifier = "00000000-0000-0000-0000-000000000000"
                    };
                    _Util.Facade.ScheduleFacade.InsertSchedule(objQa1Schedule);

                    Schedule objQa2Schedule = new Schedule()
                    {
                        CompanyId = _Company.CompanyId,
                        Type = ScheduleType.QA2,
                        StartDate = (DateTime.Now.UTCCurrentTime()).AddDays(1),
                        EndDate = (DateTime.Now.UTCCurrentTime()).AddDays(2),
                        Title = ScheduleTitle.QA2Required + " " + LeadobjName,
                        IsCompleted = false,
                        LeadId = LSchedule.Id,
                        IsFullDay = true,
                        Identifier = "00000000-0000-0000-0000-000000000000"
                    };
                    _Util.Facade.ScheduleFacade.InsertSchedule(objQa2Schedule);
                }
                if (!string.IsNullOrWhiteSpace(LSchedule.Installer))
                {
                    var ScheduleCustomerappointment = _Util.Facade.CustomerAppoinmentFacade.GetAllAppointmentByCustomerId(LSchedule.CustomerId);
                    if (ScheduleCustomerappointment != null)
                    {
                        objScheduleCalendar.CompanyId = _Company.CompanyId;
                        objScheduleCalendar.Type = ScheduleCustomerappointment.AppointmentType;
                        objScheduleCalendar.Title = ScheduleCustomerappointment.Notes;
                        objScheduleCalendar.IsCompleted = false;
                        objScheduleCalendar.LeadId = LSchedule.Id;
                        objScheduleCalendar.IsFullDay = true;
                        objScheduleCalendar.Identifier = "00000000-0000-0000-0000-000000000000";
                        objScheduleCalendar.StartDate = Convert.ToDateTime("01/01/0001");
                        objScheduleCalendar.EndDate = Convert.ToDateTime("01/01/0001");
                        _Util.Facade.ScheduleFacade.InsertSchedule(objScheduleCalendar);
                    }
                }
            }

            //if (currentLoggedIn.UserRole != "SysAdmin" && currentLoggedIn.UserRole != "Stuff")
            //{
            //    model.ListTechnicianSchedule = _Util.Facade.ScheduleFacade.GetPermissionScheduleListByCompanyId(_Company.CompanyId, currentLoggedIn.UserRole);
            //}
            //else
            //{
            //    model.ListTechnicianSchedule = _Util.Facade.ScheduleFacade.GetScheduleListByCompanyId(_Company.CompanyId);
            //}
            return true;
        }
        #endregion


        public JsonResult GetCityStateZipListByKey(string key)
        {
            string appname = "RMRCloud";
            string result = "[]";
            List<CityZipCode> resultarr = new List<CityZipCode>();
            string URL = "http://zipcodelookup.rmrcloud.com/1.0/GetCityZipCodeLookupList";
            string urlParameters = "?key=" + key + "&appname=" + appname;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Add("key", key);
            client.DefaultRequestHeaders.Add("appname", appname);
            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var dataObjects = response.Content.ReadAsStringAsync().Result;
                resultarr = JsonConvert.DeserializeObject<List<CityZipCode>>(dataObjects);
            }
            if (resultarr.Count > 0)
            {
                result = JsonConvert.SerializeObject(resultarr);
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult UpdateSalesPersonListForAddLead(Guid? modelSelectSalesPerson, Guid removeSalesPerson)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            string result = "[]";
            var objEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
            List<Employee> Employeelist = new List<Employee>();
            if (IsPermitted(UserPermissions.MyCompanyPermissions.ShowAllSalesPerson))
            {
                Employee objcurrentlog = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);

                Employeelist = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Where(x => x.UserId != currentLoggedIn.UserId).ToList();

                if (modelSelectSalesPerson != null && modelSelectSalesPerson != new Guid())
                {
                    var SoldByEmployeeDetails = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(modelSelectSalesPerson.Value);
                    if (SoldByEmployeeDetails != null && SoldByEmployeeDetails.IsActive == false)
                    {
                        Employeelist.Add(SoldByEmployeeDetails);

                    }
                }

                if (objcurrentlog != null)
                {
                    Employeelist.Add(objcurrentlog);

                }

                Employee _appoinmentSetbyEmployee = Employeelist.Where(x => x.UserId == removeSalesPerson).FirstOrDefault();
                Employeelist.Remove(_appoinmentSetbyEmployee);
            }
            else
            {
                Employeelist = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Where(x => x.UserId == currentLoggedIn.UserId).ToList();

                Employee _appoinmentSetbyEmployee = Employeelist.Where(x => x.UserId == removeSalesPerson).FirstOrDefault();
                Employeelist.Remove(_appoinmentSetbyEmployee);
            }

            if (Employeelist.Count > 0)
            {
                result = JsonConvert.SerializeObject(Employeelist.OrderBy(x => x.FirstName + " " + x.LastName));
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }
        public static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        private bool EmergencyContactExists(Guid CustomerId, Guid CompanyId, string ContactNumber, int Id)
        {
            return _Util.Facade.CustomerFacade.EmergencyContactExists(CustomerId, CompanyId, ContactNumber, Id);
        }

        public ActionResult LeadEstimatePartial(int? CustomerId, EstimateFilter filter)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
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

            //#region CookieJobs
            //bool fromCookie = false;
            //string newCookie = "";
            //if (Request.Cookies[CookieKeys.PtoFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.PtoFilter].Value))
            //{
            //    newCookie = Request.Cookies[CookieKeys.PtoFilter].Value;
            //    newCookie = Server.UrlDecode(newCookie);
            //    var CookieVals = newCookie.Split(',');
            //    if (CookieVals.Length == 4)
            //    {
            //        StartDate = CookieVals[0].ToDateTime();
            //        EndDate = CookieVals[1].ToDateTime();
            //        string SelectedWeek = CookieVals[2];
            //        if (SelectedWeek.Split('/').Length == 2)
            //        {
            //            int.TryParse(SelectedWeek.Split('/')[1], out CurrentWeek);
            //        }
            //        PtoFilter = CookieVals[3];

            //        fromCookie = true;
            //    }
            //}
            //#endregion

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
            #region Common filter
            ViewBag.PTOFilterOptions = _Util.Facade.LookupFacade.GetLookupByKey("PTOFilterOptions").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString(),
                   Selected = x.DataValue == PtoFilter
               }).ToList();
            #endregion
            ViewBag.FirstDayOfWeek = FirstDayOfWeek;
            #endregion
            List<Invoice> model = new List<Invoice>();
            if (CustomerId.HasValue)
            {
                //bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId.Value, CurrentUser.CompanyId.Value);
                //if (!res)
                //{
                //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                //}

                var customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);
                if (customerInfo != null)
                {
                    model = _Util.Facade.InvoiceFacade.GetAllEstimateListByCustomerIdAndCompanyId(customerInfo.CustomerId, CurrentUser.CompanyId.Value, filter);
                    if (model != null && model.Count > 0)
                    {
                        foreach (var item in model)
                        {
                            if (item.CreatedByUid == new Guid("00000000-0000-0000-0000-000000000000"))
                            {
                                var objest = _Util.Facade.InvoiceFacade.GetById(item.Id);
                                if (objest != null)
                                {
                                    objest.CreatedByUid = CurrentUser.UserId;
                                    _Util.Facade.InvoiceFacade.UpdateInvoice(objest);
                                }
                            }
                        }
                    }
                }
            }
            return PartialView("_LeadEstimatePartial", model);
        }

        [Authorize]
        public ActionResult AddLeadEstimate(int? id, int CustomerId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            CreateInvoice model;
            Customer tempCustomer = new Customer();
            if (CustomerId > 0)
            {
                //bool res = _Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId, currentLoggedIn.CompanyId.Value);
                //if (!res)
                //{
                //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                //}
                tempCustomer = _Util.Facade.CustomerFacade.GetById(CustomerId);
            }
            var VendorPriceStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceVendorPriceSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            if (VendorPriceStutas != null)
            {
                ViewBag.VendorPriceValue = VendorPriceStutas.IsActive.Value;
            }
            if (id.HasValue && id.Value > 0)
            {
                model = new CreateInvoice();
                model.Invoice = _Util.Facade.InvoiceFacade.GetById(id.Value);
                model.Invoice.CustomerName = tempCustomer.FirstName + " " + tempCustomer.LastName;
                if (model.Invoice == null || model.Invoice.CompanyId != currentLoggedIn.CompanyId.Value)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model.InvoiceDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetialsListByInvoiceId(model.Invoice.InvoiceId);
                model.EmailAddress = tempCustomer.EmailAddress;
                if (!string.IsNullOrWhiteSpace(model.Invoice.InvoiceEmailAddress) && model.Invoice.InvoiceEmailAddress != null)
                {
                    model.EmailAddress = model.Invoice.InvoiceEmailAddress;
                }
                ViewBag.ShippingAddress = model.Invoice.ShippingAddress;
                model.InvoiceNotes = _Util.Facade.InvoiceNoteFacade.GetAllInvoiceNoteByInvoiceIdAndCompanyId(model.Invoice.Id, currentLoggedIn.CompanyId.Value);
                ViewBag.Value = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(currentLoggedIn.CompanyId.Value);

            }
            else
            {
                model = new CreateInvoice();
                model.EmailAddress = tempCustomer.EmailAddress;
                model.Invoice = new Invoice()
                {
                    CustomerId = tempCustomer.CustomerId,
                    IsEstimate = true,
                    InvoiceEmailAddress = tempCustomer.EmailAddress,
                    CustomerName = tempCustomer.FirstName + " " + tempCustomer.LastName,
                    CompanyId = currentLoggedIn.CompanyId.Value,
                    Amount = 0,
                    Status = "Init",
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CreatedBy = User.Identity.Name,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    CreatedByUid = currentLoggedIn.UserId,
                    LastUpdatedByUid = currentLoggedIn.UserId,


                };

                var shippingStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceShippingSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
                var DiscountStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceDiscountSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
                var DipositStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceDepositSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
                if (shippingStutas != null)
                {
                    ViewBag.Value = shippingStutas.IsActive.Value;
                }
                if (DiscountStutas != null)
                {
                    ViewBag.DiscountValue = DiscountStutas.IsActive.Value;
                }
                if (DipositStutas != null)
                {
                    ViewBag.DipositValue = DipositStutas.IsActive.Value;
                }



                // ViewBag.Value = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(currentLoggedIn.CompanyId.Value);
                var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(currentLoggedIn.CompanyId.Value);
                model.Invoice.BillingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "BillingAddress", AddressTemplate);
                model.Invoice.ShippingAddress = AddressHelper.MakeCustomerAddress(tempCustomer, "ShippingAddress", AddressTemplate);
                model.CusBussinessName = tempCustomer.BusinessName;
                model.Invoice.Id = _Util.Facade.InvoiceFacade.InsertInvoice(model.Invoice);
                model.Invoice.InvoiceId = model.Invoice.Id.GenerateEstimateNo();
                _Util.Facade.InvoiceFacade.UpdateInvoice(model.Invoice);

                model.Invoice.InvoiceDate = DateTime.UtcNow.UTCToClientTime();
                model.Invoice.DueDate = DateTime.UtcNow.UTCToClientTime();
                //model.Invoice.ShippingDate = DateTime.UtcNow.UTCToClientTime();

                model.InvoiceNotes = _Util.Facade.InvoiceNoteFacade.GetAllInvoiceNoteByInvoiceIdAndCompanyId(model.Invoice.Id, currentLoggedIn.CompanyId.Value);

                model.InvoiceDetailList = new List<InvoiceDetail>();
                List<Equipment> _tempEqpDeltailList = new List<Equipment>();
                _tempEqpDeltailList = _Util.Facade.EquipmentFacade.GetIncludeEstimateEquipment();


                if (_tempEqpDeltailList.Count > 0)
                {
                    model.InvoiceDetailList = (from u in _tempEqpDeltailList
                                               select new InvoiceDetail
                                               {
                                                   EquipName = u.Name,
                                                   Quantity = u.Quantity,
                                                   UnitPrice = u.Retail,
                                                   TotalPrice = u.Total,
                                                   EquipmentId = u.EquipmentId,
                                                   EquipmentClassId = u.EquipmentClassId,
                                                   EquipDetail = u.Description
                                               }).ToList();
                }
            }


            #region View for TaxList

            var CustomerInfo = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId);

            List<SelectListItem> TaxListItem = new List<SelectListItem>();
            var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(CustomerInfo.CustomerId, currentLoggedIn.CompanyId.Value);
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
            #endregion

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
            ViewBag.FinanceCompanyList = _Util.Facade.LookupFacade.GetLookupByKey("FinanceCompany").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();

            model.InvoiceSetting = new InvoiceSetting();



            string ShippingSetting = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(currentLoggedIn.CompanyId.Value);
            if (!string.IsNullOrWhiteSpace(ShippingSetting))
            {
                model.InvoiceSetting.ShippingSetting = ShippingSetting.ToLower() == "true" ? true : false;
            }
            var TaxSetting = _Util.Facade.GlobalSettingsFacade.GetEstimateTaxSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            if (TaxSetting != null)
            {
                model.InvoiceSetting.ShowEstimateTaxSetting = TaxSetting.IsActive.Value;
            }
            var Discountsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceDiscountSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            if (Discountsetting != null)
            {
                model.InvoiceSetting.DiscountSetting = Discountsetting.IsActive.Value;
            }
            var DepositSetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceDepositSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            if (DepositSetting != null)
            {
                model.InvoiceSetting.DepositSetting = DepositSetting.IsActive.Value;
            }

            ViewBag.EstimateMessage = _Util.Facade.GlobalSettingsFacade.GetEstimateByCompanyId(currentLoggedIn.CompanyId.Value);
            ViewBag.EstimatePaymentTerms = _Util.Facade.LookupFacade.GetDropdownsByKey("EstimatePaymentTerms");

            //    _Util.Facade.LookupFacade.GetLookupByKey("EstimatePaymentTerms").Select(x =>
            //new SelectListItem()
            //{
            //    Text = x.DisplayText.ToString(),
            //    Value = x.DataValue.ToString()
            //}).ToList();
            ViewBag.EstimateContractTerm = _Util.Facade.LookupFacade.GetLookupByKey("EstimateContractTerm").Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();
            ViewBag.EstimateUpfrontMonth = _Util.Facade.LookupFacade.GetLookupByKey("EstimateUpfrontMonth").Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();
            ViewBag.EstimateMonitoringDescription = _Util.Facade.LookupFacade.GetLookupByKey("EstimateMonitoringDescription").Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();
            GlobalSetting monitoringVal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "EstimateMonitoringAmount");
            GlobalSetting contractTerm = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "EstimateContractTerm");
            GlobalSetting monitoringDes = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "EstimateMonitoringDescription");
            if (monitoringVal != null)
            {
                model.ShowEstimateMonitoringAmount = monitoringVal.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                model.ShowEstimateMonitoringAmount = false;
            }
            if (contractTerm != null)
            {
                model.ShowEstimateContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                model.ShowEstimateContractTerm = false;
            }
            if (monitoringDes != null)
            {
                model.ShowEstimateMonitoringDescription = monitoringDes.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                model.ShowEstimateMonitoringDescription = false;
            }
            GlobalSetting estimateOldBtn = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "EstimateOldButton");
            if (monitoringVal != null)
            {
                model.ShowEstimateOldButton = estimateOldBtn.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                model.ShowEstimateOldButton = false;
            }
            #region show estimate finance company
            bool IsFinance = false;
            GlobalSetting GlobalSet = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("ShowEstimateFinanceCompany", currentLoggedIn.CompanyId.Value);
            if (GlobalSet != null && !string.IsNullOrWhiteSpace(GlobalSet.Value))
            {
                if (GlobalSet.Value == "true")
                {
                    IsFinance = true;
                }
            }
            #endregion
            #region equipment category
            ViewBag.EquipmentTypeList = GetEquipmentTypeList();

            #endregion
            ViewBag.ShowFinanceCompany = IsFinance;
            return PartialView("AddLeadEstimate", model);
        }
        private List<SelectListItem> GetEquipmentTypeList()
        {
            var currentLoggedIn = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SelectListItem> EquipentTypeList = new List<SelectListItem>();
            EquipentTypeList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            EquipentTypeList.AddRange(ViewBag.EquipmentTypeList = _Util.Facade.EquipmentTypeFacade.GetAllEquipmentCategoryWithSubCategoryByCompanyId(currentLoggedIn.CompanyId.Value).OrderBy(x => x.Text).ToList());
            return EquipentTypeList;
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddLeadEstimate(CreateInvoice Model, bool SendEmail, bool CreatePdf, string ccEmail, bool? ApproveEstimate, List<int> attachedments)
        {
            /***
             * This code is same as Estimate/AddEstimte
             * I next time any change happens, I think i am going to remove this whole block for good.
             * Need some tests before removing, thats all.
             */

            bool EmailSent = false;
            string PdfLocation = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            #region Update Invocie Table 
            Invoice tempInvo = _Util.Facade.InvoiceFacade.GetInvoiceByInvoiceId(Model.Invoice.InvoiceId);

            #region Data validations
            if (tempInvo == null || tempInvo.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false });
            }
            if (Model.InvoiceDetailList == null)
            {
                return Json(new { result = false, message = "Customer Equipment Found" });
            }
            Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Invoice.CustomerId);
            if (tempCUstomer == null)
            {
                return Json(new { result = false, message = "Customer Not Found" });
            }
            CustomerExtended tempCusExtd = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(tempCUstomer.CustomerId);
            if (Model.Invoice.InvoiceDate.HasValue && Model.Invoice.DueDate.HasValue)
            {
                if (Model.Invoice.DueDate.Value < Model.Invoice.InvoiceDate.Value)
                {
                    return Json(new { result = false, message = "Due date should be greater than or equal to invoice date." });
                }
            }
            #endregion Data validations

            Model.Invoice.Message = Model.Invoice.InvoiceMessage;
            Model.Invoice.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;
            Model.CustomerName = Model.Invoice.CustomerName;
            Model.Invoice.InvoiceEmailAddress = Model.EmailAddress;
            Model.Invoice.Id = tempInvo.Id;
            Model.Invoice.IsEstimate = true;
            Model.Invoice.CreatedBy = tempInvo.CreatedBy;
            Model.Invoice.CreatedByUid = tempInvo.CreatedByUid;
            Model.Invoice.CreatedDate = tempInvo.CreatedDate;
            Model.Invoice.CompanyId = tempInvo.CompanyId;
            Model.Invoice.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            Model.Invoice.LastUpdatedByUid = CurrentUser.UserId;
            if (string.IsNullOrWhiteSpace(ccEmail))
            {
                Model.Invoice.InvoiceCcEmailAddress = tempInvo.InvoiceCcEmailAddress;
            }
            Model.Invoice.ccEmail = ccEmail;
            if (tempInvo.Status == LabelHelper.EstimateStatus.Init
                 || string.IsNullOrWhiteSpace(tempInvo.Status))
            {
                Model.Invoice.Status = LabelHelper.EstimateStatus.Created;
            }
            else
            {
                Model.Invoice.Status = tempInvo.Status;
            }

            if (Model.Invoice.InvoiceDate.HasValue)
            {
                Model.Invoice.InvoiceDate = Model.Invoice.InvoiceDate.Value.SetZeroHour();
            }
            if (Model.Invoice.DueDate.HasValue)
            {
                Model.Invoice.DueDate = Model.Invoice.DueDate.Value.SetMaxHour();
            }
            if (Model.Invoice.ShippingDate.HasValue)
            {
                Model.Invoice.ShippingDate = Model.Invoice.ShippingDate.Value.SetMaxHour();
            }

            if (string.IsNullOrWhiteSpace(Model.Invoice.CreatedBy)
               || Model.Invoice.CreatedByUid == new Guid())
            {
                Model.Invoice.CreatedBy = User.Identity.Name;
                Model.Invoice.CreatedByUid = CurrentUser.UserId;
            }
            Model.Invoice.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            #region If Approved Convert Estimate to invoice
            if (ApproveEstimate.HasValue
                && ApproveEstimate.Value
                && Model.Invoice.EstimateTerm != LabelHelper.EstimatePaymentTerms.DueUponCompletion)
            {
                Model.Invoice.IsEstimate = false;
                Model.Invoice.InvoiceFor = LabelHelper.InvoiceFor.Invoice;
                Model.Invoice.Status = LabelHelper.InvoiceStatus.Open;
                Model.Invoice.InvoiceId = Model.Invoice.Id.GenerateInvoiceNo();
            }
            #endregion

            _Util.Facade.InvoiceFacade.UpdateInvoice(Model.Invoice);

            #endregion

            #region Update Invoice details
            _Util.Facade.InvoiceFacade.DeleteAllInvoiceDetailsByInvoiceId(Model.Invoice.InvoiceId);
            Model.SubTotal = 0;
            var InvoiceDetailList = Model.InvoiceDetailList.OrderBy(x => x.Order);
            foreach (var item in InvoiceDetailList)
            {
                item.CreatedBy = User.Identity.Name;
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.CompanyId = CurrentUser.CompanyId.Value;
                item.InvoiceId = Model.Invoice.InvoiceId;
                item.Id = _Util.Facade.InvoiceFacade.InsertInvoiceDetails(item);
                item.EquipmentFile = _Util.Facade.EquipmentFileFacade.GetEquipmentFilesByEquipmentId(item.EquipmentId).FirstOrDefault();
                if (item.EquipmentFile == null)
                {
                    item.EquipmentFile = new EquipmentFile();
                }
                Model.SubTotal = Model.SubTotal + item.TotalPrice.Value;
            }
            if (tempInvo.Discountpercent != null)
            {
                Model.Discount = ((tempInvo.Discountpercent * Model.SubTotal) / 100).Value;
            }
            #endregion Update Invoice details

            #region Insert Into Customer Snapshot Table
            var objEstimateSnapshot = _Util.Facade.CustomerSnapshotFacade.GetCustomerSnapshotDetail(Model.Invoice.InvoiceId.ToString());
            if (objEstimateSnapshot.Count == 0)
            {
                var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                if (empobj != null)
                {
                    var updatedate = Model.Invoice.LastUpdatedDate.UTCToClientTime();
                    CustomerSnapshot objEstimateLog = new CustomerSnapshot()
                    {
                        CustomerId = tempCUstomer.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        Description = "Estimate " + string.Format("<a onclick=OpenTopToBottomModal('/Estimate/AddEstimate?id={0}&CustomerId={1}') style='cursor: pointer;'>", Model.Invoice.Id, tempCUstomer.Id, tempCUstomer.CustomerId) + "<b>" + Model.Invoice.InvoiceId + "</b>" + "</a>",
                        Logdate = DateTime.Now.UTCCurrentTime(),
                        Updatedby = empobj.FirstName + " " + empobj.LastName,
                        Type = "EstimateCreateHistory"
                    };
                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEstimateLog);
                }
            }
            #endregion Insert Into Customer Snapshot Table


            if (CreatePdf)
            {
                #region Create Pdf
                Model.Invoice.EstimateTerm = "";
                if (!string.IsNullOrWhiteSpace(tempInvo.EstimateTerm) && tempInvo.EstimateTerm != "-1")
                    Model.Invoice.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(tempInvo.EstimateTerm);
                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
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
                if (IsPermitted(UserPermissions.CustomerPermissions.ShowSalesPersonEmail))
                {
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(tempInvo.CreatedByUid);
                    if (empobj != null)
                    {
                        Model.CompanyEmail = empobj.Email;
                    }
                }
                else
                {
                    Model.CompanyEmail = tempCom.EmailAdress;
                }
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
                Model.CustomerSSN = tempCUstomer.SSN;
                Model.CustomerDOB = tempCUstomer.DateofBirth.HasValue ? tempCUstomer.DateofBirth.Value : new DateTime();
                Model.CustomerDrivingLicense = tempCusExtd != null ? tempCusExtd.DrivingLicense : "";
                Model.CustomerStreet = tempCUstomer.Street;
                //ViewBag.ShippingValue = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(tempCom.CompanyId);
                Model.ShowInvoiceShippingAddress = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(CurrentUser.CompanyId.Value).ToLower() == "true" ? true : false;
                Model.InvoiceSetting = new InvoiceSetting();
                string settingskey = @" 'InvoiceSettingsShipping', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CurrentUser.CompanyId.Value);
                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            Model.InvoiceSetting.DepositSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsDiscount")
                        {
                            Model.InvoiceSetting.DiscountSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsShipping")
                        {
                            Model.InvoiceSetting.ShippingSetting = true;
                        }
                    }
                }
                GlobalSetting monitoringVal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringAmount");
                GlobalSetting contractTerm = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateContractTerm");
                GlobalSetting onitWater = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowONITWaterLLCForEstimate");
                GlobalSetting monitoringDes = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringDescription");
                GlobalSetting thompsonEstimate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowThompsonEstimateText");
                if (thompsonEstimate != null)
                {
                    Model.ShowThompsonEstimateText = thompsonEstimate.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowThompsonEstimateText = false;
                }
                if (monitoringDes != null)
                {
                    Model.ShowEstimateMonitoringDescription = monitoringDes.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateMonitoringDescription = false;
                }
                if (monitoringVal != null)
                {
                    Model.ShowEstimateMonitoringAmount = monitoringVal.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateMonitoringAmount = false;
                }
                if (contractTerm != null)
                {
                    Model.ShowEstimateContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateContractTerm = false;
                }
                if (onitWater != null)
                {
                    Model.ShowOnitWaterTreatment = onitWater.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowOnitWaterTreatment = false;
                }
                GlobalSetting invCode3Sta = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowCode3InvoiceStaticBox");
                if (invCode3Sta != null)
                {
                    Model.ShowCode3InvoiceStaticBox = invCode3Sta.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowCode3InvoiceStaticBox = false;
                }
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("EstimatePdf", Model)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                Random rand = new Random();
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                string filename = ConfigurationManager.AppSettings["File.EstimateFiles"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
                filename = string.Format(filename, comname);
                filename += rand.Next().ToString() + "___" + DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + Model.Invoice.InvoiceId + ".pdf";
                string Serverfilename = FileHelper.GetFileFullPath(filename);
                //var oldServerFile = Server.MapPath(filename);
                //if (System.IO.File.Exists(oldServerFile))
                //{
                //    System.IO.File.Delete(oldServerFile);
                //}
                Session[SessionKeys.EstimatePdfSession] = filename;
                FileHelper.SaveFile(applicationPDFData, Serverfilename);
                #region Save CustomerFile
                //_Util.Facade.CustomerFileFacade.SaveEstimatePdfFile(filename, Model.Invoice.InvoiceId, tempCUstomer.CustomerId, CurrentUser.CompanyId.Value);
                #endregion

                #endregion

                return Json(new { result = true, emailSent = EmailSent, message = "Invoice Successfully Saved" });
            }
            else if (SendEmail)
            {
                var tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
                PdfLocation = SendEstimateEmail(Model, tempCUstomer, tempCom, attachedments).FileLocation;
                #region Comment
                //tempCUstomer.PreferedEmail = true;
                //_Util.Facade.CustomerFacade.UpdateCustomer(tempCUstomer);
                //string filename = "";
                //if (Session[SessionKeys.EstimatePdfSession] == null)
                //{
                //    Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
                //    Model.CompanyAddress = MakeAddress(tempCom.Street, tempCom.City, tempCom.State, tempCom.ZipCode, "");
                //    Model.CompanyEmail = tempCom.EmailAdress;
                //    Model.CompanyName = tempCom.CompanyName;
                //    Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);

                //    ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("EstimatePdf", Model)
                //    {
                //        //FileName = "TestView.pdf",
                //        PageSize = Rotativa.Options.Size.A4,
                //        PageOrientation = Rotativa.Options.Orientation.Portrait,
                //        PageMargins = { Left = 1, Right = 1 },

                //    };

                //    byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                //    Random rand = new Random();
                //    filename = ConfigurationManager.AppSettings["File.EstimateFiles"];
                //    filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + rand.Next().ToString() + "_" + rand.Next().ToString() + "_Estimate.pdf";
                //    Session[SessionKeys.EstimatePdfSession] = filename;
                //    filename = FileHelper.GetFileFullPath(filename);
                //    string FileLoc = filename;
                //    PdfLocation = filename;
                //    FileHelper.SaveFile(applicationPDFData, filename);
                //    #region Save CustomerFile
                //    _Util.Facade.CustomerFileFacade.SaveEstimatePdfFile(filename, Model.Invoice.InvoiceId, tempCUstomer.CustomerId, CurrentUser.CompanyId.Value);
                //    #endregion
                //}
                //else
                //{
                //    filename = Session[SessionKeys.EstimatePdfSession].ToString();
                //}
                //try
                //{
                //    List<string> EmailAddressesList = Model.EmailAddress.Split(',').ToList<string>();
                //    foreach (var item in EmailAddressesList)
                //    {
                //        InvoiceCreatedEmail email = new InvoiceCreatedEmail()
                //        {
                //            CompanyName = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName,
                //            CustomerName = Model.Invoice.CustomerName,
                //            BalanceDue = Model.Invoice.TotalAmount != null ? "$" + Model.Invoice.TotalAmount.Value.ToString("0,0.00") : "@HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency",
                //            DueDate = Model.Invoice.DueDate.HasValue ? Model.Invoice.DueDate.Value.ToString("MM/dd/yy") : "",
                //            InvoiceId = Model.Invoice.InvoiceId,
                //            ToEmail = item.Trim(),
                //            FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : "info@rmrcloud.com",
                //            FromName = CurrentUser.GetFullName(),
                //            EmailBody = Model.EmailDescription,
                //            Subject = Model.EmailSubject,
                //            CustomerId = Model.Invoice.CustomerId.ToString(),
                //            EmployeeId = CurrentUser.UserId.ToString(),
                //            InvoicePdf = new Attachment(
                //              FileHelper.GetFileFullPath(filename),
                //             MediaTypeNames.Application.Octet)
                //        };
                //        EmailSent = _Util.Facade.MailFacade.SendEstimateCreatedEmail(email, CurrentUser.CompanyId.Value);
                //        email.InvoicePdf.Dispose();
                //        if (EmailSent)
                //        {
                //            CustomerSnapshot objEstimateEmailLog = new CustomerSnapshot()
                //            {
                //                CustomerId = Model.Invoice.CustomerId,
                //                CompanyId = CurrentUser.CompanyId.Value,
                //                Description = "Estimate:" + "  " + Model.Invoice.InvoiceId + " " + "email sent by ",
                //                Logdate = DateTime.Now.UTCCurrentTime(),
                //                Updatedby = CurrentUser.Identity.Name,
                //                Type = "CustomerMailHistory"
                //            };
                //            _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEstimateEmailLog);
                //            Model.Invoice.Status = LabelHelper.EstimateStatus.SentToCustomer;
                //            _Util.Facade.InvoiceFacade.UpdateInvoice(Model.Invoice);
                //        }
                //        //else
                //        //{
                //        //    Model.Invoice.Status = LabelHelper.EstimateStatus.SentToCustomer;
                //        //    _Util.Facade.InvoiceFacade.UpdateInvoice(Model.Invoice);
                //        //}
                //    }
                //}
                //catch (Exception ex)
                //{

                //}
                #endregion
            }
            else if (ApproveEstimate.HasValue && ApproveEstimate.Value)
            {

                #region Instructions
                /*
                 * **
                 * **
                 * Callers
                 * I.  This part will call only if the administrator approves an estimate.
                 * II. This can be called from customer as well as lead section.
                 * **
                 * Tasks
                 * 1.Convert Lead to customer
                 * 2.Create Estimate_approved.pdf
                 * 3.Convert Estimate to Invoice
                 * 4.Send notification Email/SMS to Customer.
                 * 5.Send Email To sales person
                 * **
                 * Additional Notes
                 * a.there will be no entree on CustomerAgreement Table
                 * **
                 */
                #endregion

                #region 1.Convert Lead to customer
                CustomerCompany cc = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerIdAndCompanyId(tempCUstomer.CustomerId, CurrentUser.CompanyId.Value);
                cc.IsLead = false;
                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cc);
                tempCUstomer.JoinDate = DateTime.Now.UTCCurrentTime();
                tempCUstomer.IsActive = true;
                _Util.Facade.CustomerFacade.UpdateCustomer(tempCUstomer);
                #endregion

                #region 2.Create Estimate_approved.pdf

                if (!string.IsNullOrWhiteSpace(Model.Invoice.EstimateTerm) && Model.Invoice.EstimateTerm != "-1")
                    Model.Invoice.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(Model.Invoice.EstimateTerm);
                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
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
                if (IsPermitted(UserPermissions.CustomerPermissions.ShowSalesPersonEmail))
                {
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(tempInvo.CreatedByUid);
                    if (empobj != null)
                    {
                        Model.CompanyEmail = empobj.Email;
                    }
                }
                else
                {
                    Model.CompanyEmail = tempCom.EmailAdress;
                }
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
                Model.ShowInvoiceShippingAddress = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(CurrentUser.CompanyId.Value).ToLower() == "true" ? true : false;
                Model.Invoice.CompanyId = tempCom.CompanyId;

                Model.InvoiceSetting = new InvoiceSetting();
                string settingskey = @" 'InvoiceSettingsShipping', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CurrentUser.CompanyId.Value);
                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            Model.InvoiceSetting.DepositSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsDiscount")
                        {
                            Model.InvoiceSetting.DiscountSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsShipping")
                        {
                            Model.InvoiceSetting.ShippingSetting = true;
                        }
                    }
                }
                GlobalSetting invCode3Sta = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowCode3InvoiceStaticBox");
                if (invCode3Sta != null)
                {
                    Model.ShowCode3InvoiceStaticBox = invCode3Sta.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowCode3InvoiceStaticBox = false;
                }
                ViewAsPdf EstimateActionPdf = new Rotativa.ViewAsPdf("~/Views/Estimate/EstimatePdf.cshtml", Model)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] EstimatePdfData = EstimateActionPdf.BuildPdf(ControllerContext);

                #region Save Estimate.pdf to file System  
                string estimateno = Model.Invoice.Id.GenerateEstimateNo();
                string filename = ConfigurationManager.AppSettings["File.EstimateFiles"];
                string comname = tempCom.CompanyName.ReplaceSpecialChar();
                filename = string.Format(filename, comname);
                filename += DateTime.Now.UTCCurrentTime().Year.ToString()
                    + "/" + DateTime.Now.UTCCurrentTime().Month.ToString()
                    + "/" + estimateno + "_Approved.pdf";
                string Serverfilename = FileHelper.GetFileFullPath(filename);
                FileHelper.SaveFile(EstimatePdfData, Serverfilename);
                #endregion

                #region Save CustomerFile
                _Util.Facade.CustomerFileFacade.SaveEstimatePdfFile(AppConfig.DomainSitePath + filename, estimateno, tempCUstomer.CustomerId, CurrentUser.CompanyId.Value, false, true);
                #endregion

                #endregion

                #region 3.Convert Estimate To Invoice
                /**
                 * This part is done on updating estimate
                 */
                #endregion

                #region Send notification Email to customer

                #endregion

                return Json(new { result = true, message = "Estimate converted to invoice successfully.", converted = true });
            }
            return Json(new { result = true, message = string.Concat("Estimate Saved Successfully and Email to ", Model.EmailAddress), filePath = PdfLocation, EmailSent = EmailSent });
        }

        [Authorize]
        [HttpPost]
        public JsonResult FinalCustomerSetupData(int setupid)
        {
            bool result = false;
            List<string> message = new List<string>();
            FinalCustomerSetupData Model = new FinalCustomerSetupData();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var objCustomerdata = _Util.Facade.CustomerFacade.GetCustomerById(setupid);
            if (objCustomerdata != null)
            {
                Model = _Util.Facade.CustomerFacade.GetAllFinalCustomerSetupDataByCompanyIdAndCustomerId(CurrentUser.CompanyId.Value, objCustomerdata.CustomerId);
            }
            if (string.IsNullOrWhiteSpace(objCustomerdata.FirstName) || string.IsNullOrWhiteSpace(objCustomerdata.LastName))
            {
                message.Add("For signing agreement first name and last name required");
            }
            if (Model.ListPackageCustomer.Count == 0)
            {
                message.Add("You have not selected any package");
            }
            if (Model.ListCustomerAppointmentEquipment.Count == 0)
            {
                message.Add("You have not selected any Equipment");
            }
            if (Model.ListPaymentInfoCustomer.Count == 0)
            {
                message.Add("You have not selected any payment method in service tab");
            }
            if (objCustomerdata.ActivationFee.ToString() == "-1")
            {
                message.Add("You have not selected any activation fee amount");
            }
            if (string.IsNullOrWhiteSpace(objCustomerdata.Passcode))
            {
                message.Add("You have not selected any verbal password");
            }
            if (Model.ListPackageCustomer.Count != 0 && Model.ListCustomerAppointmentEquipment.Count != 0 && Model.ListPaymentInfoCustomer.Count != 0 && objCustomerdata.ActivationFee.ToString() != "-1" && !string.IsNullOrWhiteSpace(objCustomerdata.Passcode) && !string.IsNullOrWhiteSpace(objCustomerdata.FirstName) && !string.IsNullOrWhiteSpace(objCustomerdata.LastName))
            {
                if (Model.ListPaymentInfoCustomer.Count == 4)
                {
                    result = true;
                }
                if (Model.ListPaymentInfoCustomer.Count < 4)
                {
                    message.Add("You have not selected Payment option");
                }
            }
            return Json(new { result = result, message = message });
        }

        [Authorize]
        public ActionResult SMSToSalesPerson(int? id, int? Cid)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Customer model = new Customer();
            if (id.HasValue)
            {
                model = _Util.Facade.CustomerFacade.GetCustomerById(id.Value);
                List<SelectListItem> SalesPerson = new List<SelectListItem>();
                if (IsPermitted(UserPermissions.MyCompanyPermissions.ShowAllSalesPerson))
                {
                    var objcurrentlog = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                    SalesPerson.Add(new SelectListItem()
                    {
                        Text = "Please Select One",
                        Value = ""
                    });
                    if (objcurrentlog != null)
                    {
                        SalesPerson.Add(new SelectListItem()
                        {
                            Text = objcurrentlog.FirstName + " " + objcurrentlog.LastName,
                            Value = objcurrentlog.UserId.ToString()
                        });
                    }
                    SalesPerson.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid(), LabelHelper.UserTags.Partner).OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                              Value = x.UserId.ToString()
                          }).ToList());
                    ViewBag.SalesPersonList = SalesPerson.OrderBy(x => x.Text).ToList();
                    if (string.IsNullOrWhiteSpace(model.Soldby))
                    {
                        model.Soldby = objcurrentlog.UserId.ToString();
                    }
                }
                else
                {
                    var objEmp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                    if (objEmp != null)
                    {
                        SalesPerson.Add(new SelectListItem()
                        {
                            Text = objEmp.FirstName.ToString() + " " + objEmp.LastName.ToString(),
                            Value = currentLoggedIn.UserId.ToString()
                        });
                        ViewBag.SalesPersonList = SalesPerson.OrderBy(x => x.Text).ToList(); ;
                    }
                }
                if (Cid.HasValue)
                {
                    model.LeadCorrespondence = _Util.Facade.LeadCorrespondenceFacade.GetCorrespondenceById(Cid.Value);
                }
            }
            return PartialView("_SMSToSalesPerson", model);
        }

        #region CheckValidMail
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        public JsonResult DeclineLeadEstimateStatus(int? id)
        {
            bool result = false;
            Employee empobj = new Employee();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (id.HasValue)
            {
                var objinv = _Util.Facade.InvoiceFacade.GetInvoiceById(id.Value);
                if (objinv != null)
                {
                    var Cusobj = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(objinv.CustomerId);
                    if (!string.IsNullOrWhiteSpace(Cusobj.Soldby))
                    {
                        empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(new Guid(Cusobj.Soldby));
                    }
                    else
                    {
                        empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(currentLoggedIn.UserId);
                    }
                    objinv.Status = LabelHelper.EstimateStatus.CancelEstimate;
                    result = _Util.Facade.InvoiceFacade.UpdateInvoice(objinv);
                }
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult ChangeFollowUpReminderTime()
        {
            List<Lookup> model = _Util.Facade.LookupFacade.GetLookupByKey("Arrival").ToList();
            return View("ChangeFollowUpRemainderTime", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveArrivalTime(int? lid, bool checkval)
        {
            bool result = false;
            if (lid.HasValue)
            {
                var objlookup = _Util.Facade.LookupFacade.GetLookUpById(lid.Value);
                if (objlookup != null)
                {
                    if (checkval == true)
                    {
                        objlookup.IsActive = checkval;
                        _Util.Facade.LookupFacade.UpdateLookUp(objlookup);
                    }
                    else
                    {
                        objlookup.IsActive = checkval;
                        _Util.Facade.LookupFacade.UpdateLookUp(objlookup);
                    }
                }
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult AddLeadCreditCheckPartial(int? id)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Customer model;
            if (id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.CustomerFacade.GetCustomersById(id.Value);
                model.CustomerSpouse = _Util.Facade.CustomerFacade.GetSpouseByCustomerIdAndComapnyId(model.CustomerId, CurrentLoggedInUser.CompanyId.Value);
                if (model.CustomerSpouse == null)
                {
                    model.CustomerSpouse = new CustomerSpouse();
                }
            }
            else
            {
                model = new Customer();
                model.CustomerSpouse = new CustomerSpouse();
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveLeadCreditCheck(Customer cus, CustomerSpouse spouse)
        {
            bool result = false;
            bool delres = false;
            bool cusres = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Customer tempcus;
            if (cus.Id > 0)
            {
                tempcus = _Util.Facade.CustomerFacade.GetCustomerById(cus.Id);
                if (tempcus != null)
                {
                    if (string.IsNullOrWhiteSpace(cus.StreetPrevious) && string.IsNullOrWhiteSpace(cus.CityPrevious) && string.IsNullOrWhiteSpace(cus.StatePrevious) && string.IsNullOrWhiteSpace(cus.ZipCodePrevious))
                    {
                        tempcus.DateofBirth = cus.DateofBirth;
                        tempcus.SSN = cus.SSN;
                        tempcus.StreetPrevious = cus.StreetPrevious;
                        tempcus.CityPrevious = cus.CityPrevious;
                        tempcus.StatePrevious = cus.StatePrevious;
                        tempcus.ZipCodePrevious = cus.ZipCodePrevious;
                        cusres = _Util.Facade.CustomerFacade.UpdateCustomer(tempcus);
                    }
                    else
                    {
                        tempcus.DateofBirth = cus.DateofBirth;
                        tempcus.SSN = cus.SSN;
                        tempcus.StreetPrevious = cus.StreetPrevious;
                        tempcus.CityPrevious = cus.CityPrevious;
                        tempcus.StatePrevious = cus.StatePrevious;
                        tempcus.ZipCodePrevious = cus.ZipCodePrevious;
                        result = _Util.Facade.CustomerFacade.UpdateCustomer(tempcus);
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(spouse.FirstName) && !string.IsNullOrWhiteSpace(spouse.LastName) && spouse.CheckSpouse == true)
            {
                var objspouse = _Util.Facade.CustomerFacade.GetSpouseByCustomerIdAndComapnyId(spouse.CustomerId, CurrentLoggedInUser.CompanyId.Value);
                if (objspouse != null)
                {
                    objspouse.FirstName = spouse.FirstName;
                    objspouse.LastName = spouse.LastName;
                    objspouse.DateofBirth = spouse.DateofBirth;
                    objspouse.SSN = spouse.SSN;
                    result = _Util.Facade.CustomerFacade.UpdateCustomerSpouse(objspouse);
                }
                else
                {
                    CustomerSpouse objspo = new CustomerSpouse()
                    {
                        CompanyId = CurrentLoggedInUser.CompanyId.Value,
                        CustomerId = spouse.CustomerId,
                        FirstName = spouse.FirstName,
                        LastName = spouse.LastName,
                        DateofBirth = spouse.DateofBirth,
                        SSN = spouse.SSN,
                        AddedDate = DateTime.Now,
                        CreditCheckDate = DateTime.Now
                    };
                    result = _Util.Facade.CustomerFacade.InsertCustomerSpouse(objspo) > 0;
                }
            }
            else
            {
                var objspo = _Util.Facade.CustomerFacade.GetSpouseByCustomerIdAndComapnyId(spouse.CustomerId, CurrentLoggedInUser.CompanyId.Value);
                if (objspo != null)
                {
                    _Util.Facade.CustomerFacade.DeleteSpouseById(objspo.Id);
                }
                delres = true;
            }
            return Json(new { result = result, delres = delres, cusres = cusres });
        }

        //Email will send to Model.EmailAddress
        private InvoiceEmailSentResponse SendEstimateEmail(CreateInvoice Model, Customer tempCUstomer, Company tempCom, List<int> attachedments)
        {
            var response = new InvoiceEmailSentResponse
            {
                FileLocation = "",
                IsSent = false
            };
            tempCUstomer.PreferedEmail = true;
            _Util.Facade.CustomerFacade.UpdateCustomer(tempCUstomer);
            CustomerExtended tempCusExtd = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(tempCUstomer.CustomerId);
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string filename = "";
            response.IsSent = false;
            if (Session[SessionKeys.EstimatePdfSession] == null)
            {
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
                if (IsPermitted(UserPermissions.CustomerPermissions.ShowSalesPersonEmail))
                {
                    var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(Model.Invoice.CreatedByUid);
                    if (empobj != null)
                    {
                        Model.CompanyEmail = empobj.Email;
                    }
                }
                else
                {
                    Model.CompanyEmail = tempCom.EmailAdress;
                }
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
                Model.CustomerSSN = tempCUstomer.SSN;
                Model.CustomerDOB = tempCUstomer.DateofBirth.HasValue ? tempCUstomer.DateofBirth.Value : new DateTime();
                Model.CustomerDrivingLicense = tempCusExtd != null ? tempCusExtd.DrivingLicense : "";
                Model.CustomerStreet = tempCUstomer.Street;
                //ViewBag.ShippingValue = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(tempCom.CompanyId);
                Model.ShowInvoiceShippingAddress = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(CurrentUser.CompanyId.Value).ToLower() == "true" ? true : false;
                Model.InvoiceSetting = new InvoiceSetting();
                string settingskey = @" 'InvoiceSettingsShipping', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CurrentUser.CompanyId.Value);
                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            Model.InvoiceSetting.DepositSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsDiscount")
                        {
                            Model.InvoiceSetting.DiscountSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsShipping")
                        {
                            Model.InvoiceSetting.ShippingSetting = true;
                        }
                    }
                }
                GlobalSetting monitoringVal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringAmount");
                GlobalSetting contractTerm = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateContractTerm");
                GlobalSetting onitWater = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowONITWaterLLCForEstimate");
                GlobalSetting monitoringDes = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringDescription");
                GlobalSetting thompsonEstimate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowThompsonEstimateText");
                if (thompsonEstimate != null)
                {
                    Model.ShowThompsonEstimateText = thompsonEstimate.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowThompsonEstimateText = false;
                }
                if (monitoringDes != null)
                {
                    Model.ShowEstimateMonitoringDescription = monitoringDes.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateMonitoringDescription = false;
                }
                if (monitoringVal != null)
                {
                    Model.ShowEstimateMonitoringAmount = monitoringVal.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateMonitoringAmount = false;
                }
                if (contractTerm != null)
                {
                    Model.ShowEstimateContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateContractTerm = false;
                }
                if (onitWater != null)
                {
                    Model.ShowOnitWaterTreatment = onitWater.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowOnitWaterTreatment = false;
                }
                GlobalSetting invCode3Sta = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowCode3InvoiceStaticBox");
                if (invCode3Sta != null)
                {
                    Model.ShowCode3InvoiceStaticBox = invCode3Sta.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowCode3InvoiceStaticBox = false;
                }
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("EstimatePdf", Model)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };

                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                Random rand = new Random();
                filename = ConfigurationManager.AppSettings["File.EstimateFiles"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
                filename = string.Format(filename, comname);
                filename += rand.Next().ToString() + "___" + DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + Model.Invoice.InvoiceId + ".pdf";
                Session[SessionKeys.EstimatePdfSession] = filename;
                filename = FileHelper.GetFileFullPath(filename);
                response.FileLocation = filename;
                FileHelper.SaveFile(applicationPDFData, filename);
                #region Insert Into Customer file
                _Util.Facade.CustomerFileFacade.SaveEstimatePdfFile(AppConfig.DomainSitePath + filename, Model.Invoice.InvoiceId, tempCUstomer.CustomerId, CurrentUser.CompanyId.Value);
                #endregion
            }
            else
            {
                filename = Session[SessionKeys.EstimatePdfSession].ToString();
            }
            try
            {
                List<string> EmailAddressesList = Model.EmailAddress.Split(',').ToList<string>();
                foreach (var item in EmailAddressesList)
                {
                    if (item.IsValidEmailAddress())
                    {
                        InvoiceCreatedEmail email = new InvoiceCreatedEmail()
                        {
                            CompanyName = tempCom.CompanyName,
                            CustomerName = Model.Invoice.CustomerName,
                            BalanceDue = Model.Invoice.TotalAmount != null ? "$" + Model.Invoice.TotalAmount.Value.ToString("0,0.00") : @HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency(),
                            DueDate = Model.Invoice.DueDate.HasValue ? Model.Invoice.DueDate.Value.ToString("MM/dd/yy") : "",
                            InvoiceId = Model.Invoice.InvoiceId,
                            ToEmail = item.Trim(),
                            EmailBody = Model.EmailDescription,
                            Subject = Model.EmailSubject,
                            CustomerId = Model.Invoice.CustomerId.ToString(),
                            EmployeeId = CurrentUser.UserId.ToString(),
                            FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : "info@rmrcloud.com",
                            FromName = CurrentUser.GetFullName(),
                            ccEmail = Model.Invoice.InvoiceCcEmailAddress,
                            //InvoicePdf = new Attachment(
                            //  FileHelper.GetFileFullPath(filename),
                            // MediaTypeNames.Application.Octet)
                        };
                        #region Get CustomerFile by Id
                        //List<Attachment> attList = new List<Attachment>();
                        try
                        {
                            if (attachedments != null && attachedments.Count() > 0)
                            {
                                List<CustomerFile> fileList = _Util.Facade.CustomerFileFacade.GetCustomerFileListById(attachedments);
                                if (fileList != null && fileList.Count() > 0)
                                {
                                    email.attachedmentList = new List<Attachment>();
                                    foreach (var item2 in fileList)
                                    {
                                        string FileName = FileHelper.GetFileFullPath(item2.Filename);
                                        email.attachedmentList.Add(new Attachment(FileName, MediaTypeNames.Application.Octet));
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        #endregion
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
                        response.IsSent = _Util.Facade.MailFacade.SendEstimateCreatedEmail(email, CurrentUser.CompanyId.Value);
                        //email.InvoicePdf.Dispose();
                        if (response.IsSent)
                        {
                            string empName = "";
                            var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(Model.Invoice.CreatedByUid);
                            if (empobj != null)
                            {
                                empName = empobj.FirstName + " " + empobj.LastName;
                            }
                            CustomerSnapshot objEstimateEmailLog = new CustomerSnapshot()
                            {
                                CustomerId = Model.Invoice.CustomerId,
                                CompanyId = CurrentUser.CompanyId.Value,
                                Description = "Estimate:" + "  " + Model.Invoice.InvoiceId + " " + "email sent by " + "<b>" + empName + "</b>",
                                Logdate = DateTime.Now.UTCCurrentTime(),
                                Updatedby = CurrentUser.Identity.Name,
                                Type = "CustomerMailHistory"
                            };
                            _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objEstimateEmailLog);

                        }
                        //EmailSent = _Util.Facade.MailFacade.SendEstimateCreatedEmail(email, CurrentUser.CompanyId.Value);
                        //email.InvoicePdf.Dispose();
                    }

                }
                if (response.IsSent)
                {
                    if (Model.Invoice.Status == LabelHelper.EstimateStatus.SentToCustomer)
                    {
                        Model.Invoice.Status = LabelHelper.EstimateStatus.ResendToCustomer;
                    }
                    else
                    {
                        Model.Invoice.Status = LabelHelper.EstimateStatus.SentToCustomer;
                    }
                    _Util.Facade.InvoiceFacade.UpdateInvoice(Model.Invoice);
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        [HttpPost]
        public JsonResult DeleteLead(int id)
        {
            bool res = false;
            if (id > 0)
            {
                res = _Util.Facade.CustomerFacade.DeleteCustomer(id);
            }
            return Json(res);
        }
        [HttpPost]
        public JsonResult ChangeToRequiredFieldTrue(string fieldname)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<GridSetting> gridSettings = _Util.Facade.GridSettingsFacade.GetAllLeadsFilterSettingByKeyAndCompanyIdAndIsActiveandKey(CurrentUser.CompanyId.Value, fieldname);

            if (gridSettings != null)
            {
                if (gridSettings.FirstOrDefault().IsLeadRequired == false)
                {
                    gridSettings.FirstOrDefault().IsLeadRequired = true;
                }


                UpdateLeadGridSettings(gridSettings);
                result = true;
            }

            return Json(new { result = result });
        }
        public JsonResult ChangeToRequiredFieldFalse(string fieldname)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<GridSetting> gridSettings = _Util.Facade.GridSettingsFacade.GetAllLeadsFilterSettingByKeyAndCompanyIdAndIsActiveandKey(CurrentUser.CompanyId.Value, fieldname);

            if (gridSettings != null)
            {
                if (gridSettings.FirstOrDefault().IsLeadRequired == true)
                {
                    gridSettings.FirstOrDefault().IsLeadRequired = false;
                }


                UpdateLeadGridSettings(gridSettings);
                result = true;
            }

            return Json(new { result = result });
        }
        public ActionResult GetLeadsForIpad(int? LeadId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (LeadId.HasValue)
            {
                ViewBag.LeadId = LeadId;
            }
            ViewBag.AgreementDocumentHeight = _Util.Facade.GlobalSettingsFacade.GetAgreementDocumentHeightByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            ViewBag.StringHeight = ViewBag.AgreementDocumentHeight + "px";
            return View();
        }
        [Authorize]
        [HttpPost]
        public JsonResult GetCustomer(int LeadLoadId)
        {
            Customer Customer = _Util.Facade.CustomerFacade.GetCustomersById(LeadLoadId);
            return Json(new { result = Customer, message = "customer" }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult LeadFormGenerationPartial(int? LeadId)
        {
            string result = "[]";
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            LeadFormGeneration model = new LeadFormGeneration();
            ViewBag.GoogleMapAPIKey = _Util.Facade.GlobalSettingsFacade.GetGoogleMapAPIKeyByCompanyId(CurrentLoggedInUser.CompanyId.Value);
            model.ListFormGenerator = _Util.Facade.FormGeneratorFacade.GetAllFormGeneratorByFormNameAndCompanyId(CurrentLoggedInUser.CompanyId.Value, "LeadDetail");
            if (LeadId.HasValue && LeadId.Value > 0)
            {

                ViewBag.LeadId = LeadId.Value;
                model.Customer = _Util.Facade.CustomerFacade.GetCustomersById(LeadId.Value);
                model.Customer.CustomerSystemInfo = _Util.Facade.CustomerSystemInfoFacade.GetCustomerSystemInfoByCustomerIdAndCompanyId(model.Customer.CustomerId, CurrentLoggedInUser.CompanyId.Value);
                if (model.Customer.CustomerSystemInfo == null)
                {
                    model.Customer.CustomerSystemInfo = new CustomerSystemInfo();
                }
                model.Customer.CustomerSpouse = _Util.Facade.CustomerFacade.GetSpouseByCustomerIdAndComapnyId(model.Customer.CustomerId, CurrentLoggedInUser.CompanyId.Value);
                if (model.Customer.CustomerSpouse == null)
                {
                    model.Customer.CustomerSpouse = new CustomerSpouse();
                }
                //result = JsonConvert.SerializeObject(model.Customer); 
                //ViewBag.CustomerModel = Json(new { Customer = model.Customer },JsonRequestBehavior.AllowGet);
                //model.Customer = (Customer)JsonConvert.DeserializeObject<Customer>(result);
            }
            else
            {
                model.Customer = new Customer();
                model.Customer.CustomerSystemInfo = new CustomerSystemInfo();
                model.Customer.CustomerSpouse = new CustomerSpouse();
                if (CurrentLoggedInUser.UserRole == "Admin" || CurrentLoggedInUser.UserRole == "SysAdmin")
                {
                    var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentLoggedInUser.UserId);
                    if (objemp != null)
                    {
                        foreach (var item in model.ListFormGenerator)
                        {
                            if (item.FieldName == "Soldby")
                            {
                                ViewBag.soldby = objemp.UserId;
                            }
                        }
                    }
                }
            }
            ViewBag.geo = _Util.Facade.GlobalSettingsFacade.GetGeoLocation(CurrentLoggedInUser.CompanyId.Value);
            return View(model);
        }

        [Authorize]
        public PartialViewResult LeadFormGeneratorSetting(int leadid)
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<FormGenerator> GridSettings = _Util.Facade.FormGeneratorFacade.GetAllFormGeneratorByFormName(CurrentLoggedInUser.CompanyId.Value, "LeadDetail");
            ViewBag.id = leadid;
            return PartialView(GridSettings);
        }

        [Authorize]
        public JsonResult UpdateLeadFormSettings(List<FormGenerator> settings)
        {
            var result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (settings.Count > 0)
            {
                foreach (var item in settings)
                {
                    item.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                    result = _Util.Facade.FormGeneratorFacade.UpdateFormGenerator(item);
                }
            }
            return Json(result);
        }

        [Authorize]
        public ActionResult LoadPaymentForPartial(Guid? customerid)
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (customerid.HasValue && customerid.Value != new Guid())
            {
                List<SelectListItem> profile = new List<SelectListItem>();
                profile.Add(new SelectListItem()
                {
                    Text = "Please Select",
                    Value = ""
                });
                profile.AddRange(_Util.Facade.CustomerFacade.GetAllPaymentProfileByCustomerId(customerid.Value, CurrentLoggedInUser.CompanyId.Value).Select(x =>
                new SelectListItem()
                {
                    Text = x.Type.ToString(),
                    Value = x.PaymentInfoId.ToString()
                }).ToList());
                ViewBag.profile = profile.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList(); ;
                List<SelectListItem> paymentfor = new List<SelectListItem>();
                paymentfor.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("PaymentFor").Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList());
                ViewBag.paymentfor = paymentfor;
            }
            return View();
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

        public string GetCreditScoreBody(CustomerCreditScore CreditScore)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string result = "false";
            IRestResponse response = _Util.Facade.CustomerFacade.GetCreditReportResponse(CreditScore, CurrentUser.CompanyId.Value);
            string responseContent = response.Content;
            return responseContent;
        }
        [Authorize]
        public ActionResult ViewCreditScore(Guid CustomerId)
        {
            Customer cus = new Customer();
            CustomerCreditScore CreditScore = new CustomerCreditScore();
            if (CustomerId != new Guid())
            {
                cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);

                CreditScore.FirstName = cus.FirstName;
                CreditScore.LastName = cus.LastName;
                CreditScore.CITY = cus.City;
                CreditScore.SSN = cus.SSN;
                CreditScore.ADDRESS = cus.Address;
                CreditScore.STATE = cus.State;
                CreditScore.ZIP = cus.ZipCode;
            }
            ViewBag.ContentBody = GetCreditScoreBody(CreditScore);

            return new ViewAsPdf();

        }
        public JsonResult LeadDetailsTabCount(Guid? customerid)
        {
            bool result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            LeadDetailTabCountModel model = new LeadDetailTabCountModel();
            if (customerid.HasValue && customerid.Value != new Guid())
            {
                model = _Util.Facade.CustomerFacade.GetLeadDetailTabCount(CurrentLoggedInUser.CompanyId.Value, customerid.Value);
                result = true;
            }
            return Json(new { result = result, model = model });
        }


        #region
        /*public ActionResult LoadACHPaymentInfo(Guid? customerid)
        {
            List<PaymentInfo> model = new List<PaymentInfo>();
            if(customerid.HasValue && customerid.Value != new Guid())
            {
                var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid.Value);
                if(objcus != null)
                {
                    var objpayprofile = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentProfileCustomerByCustomerIdAndProfileType(customerid.Value, "ACH");
                    if(objpayprofile != null && objpayprofile.Count > 0)
                    {
                        foreach(var item in objpayprofile)
                        {
                            var objpayment = _Util.Facade.PaymentInfoFacade.GetPaymentInfoById(item.PaymentInfoId);
                            if(objpayment != null)
                            {
                                var objlookupBankAccountType = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyAndDataValue("BankAccountType", objpayment.BankAccountType);
                                if(objlookupBankAccountType != null)
                                {
                                    objpayment.BankAccountType = objlookupBankAccountType.DisplayText;
                                }
                                var objlookupeCheckType = _Util.Facade.LookupFacade.GetDisplayTextByDataKeyAndDataValue("ECheckType", objpayment.EcheckType);
                                if (objlookupeCheckType != null)
                                {
                                    objpayment.EcheckType = objlookupeCheckType.DisplayText;
                                }
                            }
                            model.Add(objpayment);
                        }
                    }
                }
                ViewBag.customerid = customerid.Value;
            }
            return View(model);
        }*/
        #endregion
        #region
        //public ActionResult LoadCCPaymentInfo(Guid? customerid)
        //{
        //    List<PaymentInfo> model = new List<PaymentInfo>();
        //    if (customerid.HasValue && customerid.Value != new Guid())
        //    {
        //        var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid.Value);
        //        if (objcus != null)
        //        {
        //            var objpayprofile = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentProfileCustomerByCustomerIdAndProfileType(customerid.Value, "CC");
        //            if (objpayprofile != null && objpayprofile.Count > 0)
        //            {
        //                foreach (var item in objpayprofile)
        //                {
        //                    model.Add(_Util.Facade.PaymentInfoFacade.GetPaymentInfoById(item.PaymentInfoId));
        //                }
        //            }
        //        }
        //        ViewBag.customerid = customerid.Value;
        //    }
        //    return View(model);
        //}
        #endregion

        [Authorize]
        public ActionResult EditBillingAddressOnly(Customer Customer, int? CustomerAddressId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;

            string BillingAddressVal = "";
            string AddressForTicket = "";
            string ShippingAddressVal = "";
            if (Customer != null)
            {
                Customer model = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Customer.Id);
                if (model != null)
                {
                    model.FirstName = !string.IsNullOrWhiteSpace(Customer.FirstName) ? Customer.FirstName.ToString() : model.FirstName;
                    model.LastName = Customer.LastName;
                    model.BusinessName = Customer.BusinessName;
                    model.State = Customer.State;
                    model.ZipCode = Customer.ZipCode;
                    model.Street = Customer.Street;
                    model.City = Customer.City;
                    model.CustomerNo = Customer.CustomerNo;

                    var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);

                    if (model != null)
                    {
                        BillingAddressVal = AddressHelper.MakeCustomerAddress(model, AddressHelper.BillingAddress, AddressTemplate);
                        AddressForTicket = AddressHelper.MakeAddress(model);
                        result = true;
                        if (result)
                        {
                            if (CustomerAddressId.HasValue && CustomerAddressId > 0)
                            {
                                CustomerAddress CustomerAddresModel = _Util.Facade.CustomerFacade.GetCustomerAddesssById(CustomerAddressId.Value);
                                CustomerAddresModel.FirstName = !string.IsNullOrWhiteSpace(Customer.FirstName) ? Customer.FirstName.ToString() : model.FirstName;
                                CustomerAddresModel.LastName = Customer.LastName;
                                CustomerAddresModel.BusinessName = Customer.BusinessName;
                                CustomerAddresModel.State = Customer.State;
                                CustomerAddresModel.ZipCode = Customer.ZipCode;
                                CustomerAddresModel.Street = Customer.Street;
                                CustomerAddresModel.City = Customer.City;
                                _Util.Facade.CustomerFacade.UpdateCustomerAddress(CustomerAddresModel);
                            }
                        }
                    }
                }
                return Json(new { result = result, BillingAddressVal = BillingAddressVal, ShippingAddressVal = ShippingAddressVal, AddressForTicket = AddressForTicket });
            }
            return Json(new { result = result, BillingAddressVal = BillingAddressVal, ShippingAddressVal = ShippingAddressVal, AddressForTicket = AddressForTicket });
        }

        [HttpPost]
        public JsonResult ChangeAccessGivenToByUser(Guid userid, List<int> ArrayItemLead)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;

            if (CurrentUser == null)
            {
                return Json(result);
            }
            if (!CurrentUser.CompanyId.HasValue)
            {
                return Json(result);
            }

            if (userid != new Guid() && ArrayItemLead != null && ArrayItemLead.Count > 0)
            {
                foreach (var item in ArrayItemLead)
                {
                    Customer cus = _Util.Facade.CustomerFacade.GetCustomerById(item);

                    if (cus != null)
                    {
                        cus.Soldby = userid.ToString();
                        result = _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                    }
                    else
                    {
                        result = false;
                    }
                }
            }

            return Json(result);
        }


    }
}