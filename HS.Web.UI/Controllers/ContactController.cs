using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HS.Framework;
using HS.Web.UI.Helper;
using System.Configuration;
using System.IO;
using Rotativa;
using Rotativa.Options;
using System.Drawing;
using System.Net.Mime;
using System.Net.Mail;
using Newtonsoft.Json;
using System.Globalization;
using HS.Framework.Utils;
using HS.SMS;
using System.Drawing.Imaging;
using System.Collections;
using System.Data;
using Excel = ClosedXML.Excel;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Net;


namespace HS.Web.UI.Controllers
{
    public class ContactController : BaseController
    {
        // GET: Contact
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
        public ActionResult Contacts()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SelectListItem> taglist = new List<SelectListItem>();
            taglist.Add(new SelectListItem()
            {
                Text = "Select Tag",
                Value = ""
            });

            List<RMRTag> RMRTagDropDown = _Util.Facade.CredentialSettingFacade.GetAllTag();
            if(RMRTagDropDown != null && RMRTagDropDown.Count > 0)
            {
                taglist.AddRange(RMRTagDropDown.OrderBy(x => x.TagName).Select(x =>
                new SelectListItem()
                {
                    Text = x.TagName.ToString(),
                    Value = x.TagIdentifier.ToString()
                }).ToList());
            }
            
            ViewBag.taglist = taglist;
            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ContactTag");
            if (glob != null)
            {
                ViewBag.CotactTag = glob.Value;
            }
            else
            {
                ViewBag.CotactTag = "";
            }
            return View();
        }

        [Authorize]
        public PartialViewResult ContactDetailTab(int id, string ContactName, string Tablink, string noteid, string timeval, string IsComplete)
        {
            string ContactGuid = "";
            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerDetails))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            List<ContactTabModel> Model = new List<ContactTabModel>();
            if (id > 0)
            {
                if (string.IsNullOrWhiteSpace(ContactName))
                {
                    Contact tmCus = _Util.Facade.ContactFacade.GetContactById(id);
                    if (tmCus != null)
                    {
                        if (!string.IsNullOrWhiteSpace(tmCus.FirstName+" "+tmCus.LastName))
                        {
                            ContactName = tmCus.FirstName + " " + tmCus.LastName;
                        }
                        else
                        {
                            ContactName = tmCus.Email;
                        }
                        ContactGuid = tmCus.ContactId.ToString();

                    }

                }
                if (Request.Cookies[CookieKeys.Contact] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.Contact].Value))
                {

                    //HttpUtility.UrlDecode(Request.Cookies["__favProp"].Value);

                    string cookie = HttpUtility.UrlDecode(Request.Cookies[CookieKeys.Contact].Value); //Request.Cookies[CookieKeys.Customer].Value;
                    string[] CusList = cookie.Split('|');
                    foreach (var item in CusList)
                    {
                        if (string.IsNullOrWhiteSpace(item))
                        {
                            continue;
                        }
                        string[] itm = item.Split(',');
                        if (itm.Count() > 2)
                        {
                            Model.Add(new ContactTabModel()
                            {
                                ContactId = Convert.ToInt32(itm[0]),
                                ContactName = itm[1],
                                ContactGuid = itm[2]
                            });
                        }
                    }
                    if (Model.Count() > 0)
                    {

                        if (Model.Where(x => x.ContactId == id).Count() > 0)
                        {
                            var index = Model.FindIndex(x => x.ContactId == id);
                            var item = Model[index];
                            Model[index] = Model[0];
                            Model[0] = item;
                        }
                        else
                        {
                            Model.Insert(0, new ContactTabModel()
                            {
                                ContactId = id,
                                ContactName = ContactName,
                                ContactGuid = ContactGuid
                            });
                        }
                    }
                }
                else
                {
                    Model.Add(new ContactTabModel()
                    {
                        ContactId = id,
                        ContactName = ContactName,
                        ContactGuid = ContactGuid
                    });
                }
                string newCookie = "";
                for (int i = 0; i < Model.Count() && i < 4; i++)
                {
                    newCookie += string.Format("{0},{1},{2}|", Model[i].ContactId, Model[i].ContactName, Model[i].ContactGuid);
                }
                HttpCookie myCookie = new HttpCookie(CookieKeys.Contact);
                myCookie.Value = newCookie;// HttpUtility.UrlEncode(cookie);
                myCookie.Expires = DateTime.Now.UTCCurrentTime().AddDays(2d);
                Response.Cookies.Add(myCookie);
            }
            ViewBag.tablink = Tablink;
            ViewBag.noteid = Convert.ToInt32(noteid);
            ViewBag.time = timeval;
            ViewBag.complete = IsComplete;
            return PartialView("_ContactDetailTab", Model.GetRange(0, (Model.Count() <= 4 ? Model.Count() : 4)));
        }

        [Authorize]
        public ActionResult ContactDetails(int id, string tab, string noteid, string timeval, string IsComplete)
        {
            if (id == 0)
            {
                return null;
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //bool result = _Util.Facade.CustomerFacade.CustomerIsInCompany(id, CurrentUser.CompanyId.Value);
            //if (!result)
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}
            Contact model = _Util.Facade.ContactFacade.GetContactInfoById(id);
            if (model != null)
            {
              
                return PartialView("_ContactDetails", model);
            }
            else
            {
                return View("_AccessDenied");
            }

        }

        [Authorize]
        public PartialViewResult CorrespondenceList(Guid ContactId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<LeadCorrespondence> CorrespondenceList = _Util.Facade.LeadCorrespondenceFacade.GetAllMailCorrespondenceByCompanyIdAndCustomerId(CurrentUser.CompanyId.Value, ContactId);
            
            var cusobj = _Util.Facade.ContactFacade.GetContactbyContactId(ContactId);
            if (cusobj != null)
            {
                ViewBag.ContactId = cusobj.Id;
            }

            return PartialView("_CorrespondenceList", CorrespondenceList);
        }

        [Authorize]
        public PartialViewResult MailToSalesPerson(int? id, int? Cid, string tab)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Contact model = new Contact();
            if (id.HasValue)
            {
                model = _Util.Facade.ContactFacade.GetContactById(id.Value);
              
               
                List<SelectListItem> SalesPerson = new List<SelectListItem>();
                if (currentLoggedIn.UserRole == "Admin" || currentLoggedIn.UserRole == "SysAdmin")
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
                    SalesPerson.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                              Value = x.UserId.ToString()
                          }).ToList());
                    SalesPerson = SalesPerson.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
                    ViewBag.SalesPersonList = SalesPerson;
                    if (string.IsNullOrWhiteSpace(model.ContactOwner.ToString()))
                    {
                        model.ContactOwner = objcurrentlog.UserId;
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
                        ViewBag.SalesPersonList = SalesPerson;
                    }
                }
                if (Cid.HasValue)
                {
                    model.LeadCorrespondence = _Util.Facade.LeadCorrespondenceFacade.GetCorrespondenceById(Cid.Value);
                }
            }
            ViewBag.tab = tab;
            return PartialView("_MailToContactPerson", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveContactImportFile(string File)
        {
            string subPath = "~/LeadImportReports"; // your code goes here

            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
            {
                file.WriteLine("Started at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    {
                        file.WriteLine("Excel file read at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    }
                    //Excel.Application xlApp = new Excel.Application();
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    {
                        file.WriteLine("Excel application create at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    }
                    var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    {
                        file.WriteLine("Workbooks.Open at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    }
                    Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    {
                        file.WriteLine("select sheet 1 at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    }
                    var xlRange = workSheet.RangeUsed();

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                    {
                        file.WriteLine("calculation started at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    }
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();
                    var LookUpList = _Util.Facade.LookupFacade.GetAllLookup();
                    for (int i = 2; i <= rowCount; i++)
                    {
                        var result = false;
                        int Id;
                        Contact contact = new Contact();
                        Guid ContactId = new Guid();
                        Guid ContactOwner = new Guid();
                        Guid CreatedBy = new Guid();
                        DateTime CreatedDate = new DateTime();
                        for (int j = 1; j <= colCount; j++)
                        {
                            if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                            {
                                try
                                {
                                    var value = xlRange.Cell(i, j).Value.ToString();
                                    var header = xlRange.Cell(1, j).Value.ToString();
                                    if (header == "ContactId")
                                    {

                                        Guid.TryParse(value, out ContactId);
                                    }
                                    else if (header == "ContactOwner")
                                    {

                                        Guid.TryParse(value, out ContactOwner);
                                    }
                                    else if (header == "CreatedBy")
                                    {

                                        Guid.TryParse(value, out CreatedBy);
                                    }
                                  
                                    else if (header == "Id")
                                    {

                                        Int32.TryParse(value, out Id);
                                    }
                                    else if (contact.GetType().GetProperty(header) != null && header == "CreatedDate")
                                    {
                                        contact.GetType().GetProperty(header).SetValue(contact, Convert.ToDateTime(value));
                                    }
                                    if (contact.GetType().GetProperty(header) != null)
                                    {
                                        if (header == "CreatedDate")
                                        {
                                            contact.GetType().GetProperty(header).SetValue(contact, Convert.ToDateTime(value));
                                        }
                                        else
                                        {
                                            contact.GetType().GetProperty(header).SetValue(contact, value);
                                        }
                                       
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                        contact.ContactId = ContactId;
                        Contact isExistContact = new Contact();
                        if (contact.ContactId != new Guid())
                        {
                            isExistContact = _Util.Facade.ContactFacade.GetContactbyContactId(contact.ContactId);
                            if (isExistContact != null)
                            {
                                contact.Id = isExistContact.Id;
                                isExistContact.FirstName = contact.FirstName;
                                isExistContact.LastName = contact.LastName;
                                isExistContact.Suffix = contact.Suffix;
                                isExistContact.Title = contact.Title;
                                isExistContact.Work = contact.Work;
                                isExistContact.Ext = contact.Ext;
                                isExistContact.Mobile = contact.Mobile;
                                isExistContact.Email = contact.Email;
                                isExistContact.Role = contact.Role;
                                isExistContact.Facebook = contact.Facebook;
                                isExistContact.Twitter = contact.Twitter;
                                isExistContact.Instagram = contact.Instagram;
                                isExistContact.LinkedIN = contact.LinkedIN;
                                isExistContact.ContactOwner = contact.ContactOwner;
                                isExistContact.Notes = contact.Notes;
                                isExistContact.CreatedBy = contact.CreatedBy;
                                isExistContact.CreatedDate = contact.CreatedDate;
                                isExistContact.ContactType = contact.ContactType;
                              
                                result = _Util.Facade.ContactFacade.UpdateContacts(isExistContact);
                            }
                            else
                            {
                               
                                contact.CreatedDate = DateTime.Now.UTCCurrentTime();
                        
                                contact.CreatedBy = CurrentUser.UserId;

                                _Util.Facade.ContactFacade.InsertContacts(contact);
                            }
                        }
                        else
                        {
                            contact.ContactId = Guid.NewGuid();
                            contact.CreatedDate = DateTime.Now.UTCCurrentTime();

                            contact.CreatedBy = CurrentUser.UserId;

                            _Util.Facade.ContactFacade.InsertContacts(contact);
                          
                        }

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                        {
                            file.WriteLine("Contact Save Started at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                        }

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\LeadImportReports\report.txt"), true))
                        {
                            file.WriteLine("Contact Saved at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
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
            return Json(new { result = true });
        }
        [Authorize]
        public ActionResult SMSToContactPerson(int? id, int? Cid)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Contact model = new Contact();
            if (id.HasValue)
            {
                model = _Util.Facade.ContactFacade.GetContactById(id.Value);
                List<SelectListItem> SalesPerson = new List<SelectListItem>();
                if (currentLoggedIn.UserTags.ToLower().IndexOf("admin") > -1)
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
                    SalesPerson.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                              Value = x.UserId.ToString()
                          }).ToList());

                    SalesPerson = SalesPerson.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
                    ViewBag.SalesPersonList = SalesPerson;
                    if (string.IsNullOrWhiteSpace(model.ContactOwner.ToString()))
                    {
                        model.ContactOwner = objcurrentlog.UserId;
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
                        ViewBag.SalesPersonList = SalesPerson;
                    }
                }
                if (Cid.HasValue)
                {
                    model.LeadCorrespondence = _Util.Facade.LeadCorrespondenceFacade.GetCorrespondenceById(Cid.Value);
                }
            }
            return PartialView("_SMSToContactPerson", model);
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
                    string contactname = "";
                    var objcus = _Util.Facade.ContactFacade.GetContactbyContactId(customerId);
                    if (objcus != null)
                    {
                        contactname = objcus.FirstName + " " + objcus.LastName;
                    }
                    EmailToSalesPersonFromLeads.MailPersonName = contactname;
                    EmailToSalesPersonFromLeads.SendMailPersonName = CurrentLoggedInUser.CompanyName;
                    EmailToSalesPersonFromLeads.EmailBody = MailBody;
                    EmailToSalesPersonFromLeads.ToEmail = objcus.Email;
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
                        if(customerId != new Guid())
                        {
                            List<UserContact> ucontact = _Util.Facade.ContactFacade.GetAllUserContactListByContactId(customerId);
                            LeadCorrespondence objCorrespondence = new LeadCorrespondence();
                            if (ucontact.Count > 0)
                            {
                                foreach (var item in ucontact)
                                {
                                    objCorrespondence = new LeadCorrespondence()
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
                                        SentBy = CurrentLoggedInUser.UserId,
                                        AssociatedType = LabelHelper.ActivityAssociateType.Contact
                                    };
                                    _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objCorrespondence);
                                }
                            }
                            else
                            {
                                objCorrespondence = new LeadCorrespondence()
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
                                    SentBy = CurrentLoggedInUser.UserId,
                                    AssociatedType = LabelHelper.ActivityAssociateType.Contact
                                };
                                _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objCorrespondence);
                            }
                        }
                      
                      
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
            return Json(new { result = result, message = LanguageHelper.T(message) });
        }
        public ActionResult LoadContactList(ContactFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }
            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "CustomerListPageSize");
            if (glob != null)
            {
                filter.UnitPerPage = Convert.ToInt32(glob.Value);
            }
            else
            {
                filter.UnitPerPage = 10;
            }
            if (filter.PageNumber == null || filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }
            ContactModel Contact = new ContactModel();
            
            if (filter.FromCustomer == null || filter.FromCustomer == Guid.Empty)
            {
                ViewBag.FromCustomer = null;
                if(!CurrentUser.UserTags.Contains("admin"))
                {
                    List<string> ContactIdList = new List<string>();
                   
                    filter.ContactOwnerId = CurrentUser.UserId;
                    Contact = _Util.Facade.ContactFacade.GetContacts(filter);
           
                    ViewBag.OutOfNumber = Contact.TotalCount.TotalCount;
                }
                else
                {
                    Contact = _Util.Facade.ContactFacade.GetContacts(filter);
                 
                    ViewBag.OutOfNumber = Contact.TotalCount.TotalCount;
                }
            
            }
            else
            {
                ViewBag.FromCustomer = filter.FromCustomer;
                List<string> ContactIdList = new List<string>();

                List<UserContact> userContact = _Util.Facade.ContactFacade.GetAllUserContactsByCustomerId(filter.FromCustomer);
                if(userContact.Count > 0)
                {
                    foreach (var item in userContact)
                    {
                        ContactIdList.Add(item.ContactId.ToString());
                    }
                }
                filter.ContactsList = ContactIdList;
                if(filter.ContactsList.Count > 0)
                {
                    Contact = _Util.Facade.ContactFacade.GetContacts(filter);
              
                    ViewBag.OutOfNumber = Contact.TotalCount.TotalCount;
                }
                else
                {
                    Contact = new ContactModel();
                    Contact.ContactList = null;
                    ViewBag.CurrentNumber = 0;
                    ViewBag.OutOfNumber = 0;
                }
          
               
           
            }
        
            if ((int)ViewBag.OutOfNumber == 0)
            {
                ViewBag.Message = "No Content Available !";
            }


            if (@ViewBag.OutOfNumber == 0)
            {
                filter.PageNumber = 1;
            }
            ViewBag.PageNumber = filter.PageNumber;

            if ((int)ViewBag.PageNumber * filter.UnitPerPage > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.UnitPerPage;
            }

            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.UnitPerPage.Value);

            ViewBag.SoldBy = filter.soldby;

            return View(Contact.ContactList);
        }
        [Authorize]
        public ActionResult AddContact(int? id,Guid? CustomerId, string contactTab)
        {
            bool res = false;
            if (!string.IsNullOrWhiteSpace(contactTab))
            {
                res = Convert.ToBoolean(contactTab);
            }
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ViewBag.CustomerId = null;
            if(CustomerId != null && CustomerId != Guid.Empty)
            {
                ViewBag.CustomerId = CustomerId;
            }

            #region viewbag
            List<SelectListItem> CustomerSelectList = new List<SelectListItem>();
            CustomerSelectList.Add(new SelectListItem
            {
                Text = HS.Web.UI.Helper.LanguageHelper.T("Customer"),
                Value = "00000000-0000-0000-0000-000000000000"
            });
            ViewBag.CustomerList = CustomerSelectList;

            List<SelectListItem> LeadSelectList = new List<SelectListItem>();
            LeadSelectList.Add(new SelectListItem
            {
                Text = LanguageHelper.T("Lead"),
                Value = "00000000-0000-0000-0000-000000000000"
            });
            ViewBag.LeadList = LeadSelectList;

            List<SelectListItem> OpportunitySelectList = new List<SelectListItem>();
            OpportunitySelectList.Add(new SelectListItem
            {
                Text = LanguageHelper.T("Opportunity"),
                Value = "00000000-0000-0000-0000-000000000000"
            });
            ViewBag.OpportunityList = OpportunitySelectList;

            ViewBag.ContactRole = _Util.Facade.LookupFacade.GetLookupByKey("ContactRole").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();

            List<SelectListItem> SalesPersons = new List<SelectListItem>();
            SalesPersons.Add(new SelectListItem()
            {
                Text = "Sales Persons",
                Value = "-1"
            });

            List<Employee> EmployeeDropDown = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentLoggedInUser.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid());
            if(EmployeeDropDown != null && EmployeeDropDown.Count > 0)
            {
                SalesPersons.AddRange(EmployeeDropDown.OrderBy(x => x.FirstName).Select(x => new SelectListItem()
                {
                    Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                    Value = x.UserId.ToString()
                }).ToList());
            }
            
            ViewBag.SalesPersonsList = SalesPersons;
            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "ContactTag");
            if (glob != null)
            {
                ViewBag.ContactTag = glob.Value;
            }
            else
            {
                ViewBag.ContactTag = "";
            }
          
            #endregion

            Contact model = new Contact() {
                ContactTab = res
            };
            if (id.HasValue && id > 0)
            {
                model = _Util.Facade.ContactFacade.GetContactById(id.Value);
                List<SelectListItem> taglist = new List<SelectListItem>();
                taglist.AddRange(_Util.Facade.CredentialSettingFacade.GetAllTagListByContactId(model.ContactId).Select(x => new SelectListItem()
                {
                    Text = x.TagName.ToString(),
                    Value = x.TagIdentifier.ToString(),
                    Selected = true
                }).ToList());
                ViewBag.taglist = taglist;
                List<UserContact> UserContactList = _Util.Facade.ContactFacade.GetAllUserContactListByContactId(model.ContactId);
                ViewBag.UserContactList = UserContactList;
                

                if (UserContactList.Count >0)
                {
                    string CustomerIdList = "";
                    string OpportunityIdList = "";
                    foreach (var item in UserContactList)
                    {
                        CustomerIdList += string.Format("'{0}',", item.UserId);
                        if(item.UserType == LabelHelper.UserType.Opportunity)
                        {
                            OpportunityIdList += string.Format("'{0}',", item.UserId);
                        }
                      
                    }

                    if(CustomerIdList != "")
                    {
                        CustomerIdList = CustomerIdList.TrimEnd(',');
                        var CustomerList = _Util.Facade.CustomerFacade.GetCustomerListByCustomerIdList(CustomerIdList);
                        foreach (var item in CustomerList)
                        {
                            if (item.IsLead == true)
                            {
                                LeadSelectList.Add(new SelectListItem
                                {
                                    Text = item.customerName,
                                    Value = item.CustomerId.ToString(),
                                    
                                    Selected = true
                                });
                                ViewBag.LeadList = LeadSelectList;
                            }
                            else
                            {
                                CustomerSelectList.Add(new SelectListItem
                                {
                                    Text = item.customerName,
                                    Value = item.CustomerId.ToString(),
                                    Selected = true
                                });
                                ViewBag.CustomerList = CustomerSelectList;
                            }
                        }
                    }
                   
                    if(OpportunityIdList != "")
                    {
                        OpportunityIdList = OpportunityIdList.TrimEnd(',');
                        var OpportunityList = _Util.Facade.OpportunityFacade.GetAllOpportunityByOpportunityIdList(OpportunityIdList);
                        foreach (var item in OpportunityList)
                        {
                            OpportunitySelectList.Add(new SelectListItem
                            {
                                Text = item.OpportunityName,
                                Value = item.OpportunityId.ToString(),
                                Selected = true
                            });
                        }
                    }
                }
            }
            else
            {
                List<SelectListItem> taglist = new List<SelectListItem>();
                taglist.Add(new SelectListItem()
                {
                    Text = "",
                    Value = ""
                });
                ViewBag.taglist = taglist;
            }
            //List<SelectListItem> taglist = new List<SelectListItem>();
            //taglist = _Util.Facade.CredentialSettingFacade.GetAllTag().Select(x => new SelectListItem()
            //{
            //    Text = x.TagIdentifier.ToString(),
            //    Value = x.TagName.ToString()
            //}).ToList();
            //ViewBag.taglist = taglist;
            model.IsWorkNoVerified = true;
            model.IsMobileVerified = true;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddContact(Contact contact, List<string> ListTag)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var result = false;
            string ContactId = "";
            if (contact != null)
            {
                if (contact.Id > 0)
                {
                    var ContactDetails = _Util.Facade.ContactFacade.GetContactById(contact.Id);
                    ContactDetails.FirstName = contact.FirstName;
                    ContactDetails.LastName = contact.LastName;
                    ContactDetails.Suffix = contact.Suffix;
                    ContactDetails.Title = contact.Title;
                    ContactDetails.Work = contact.Work;
                    ContactDetails.Ext = contact.Ext;
                    ContactDetails.Mobile = contact.Mobile;
                    ContactDetails.Email = contact.Email;
                    ContactDetails.Role = contact.Role;
                    ContactDetails.Facebook = contact.Facebook;
                    ContactDetails.Twitter = contact.Twitter;
                    ContactDetails.Instagram = contact.Instagram;
                    ContactDetails.LinkedIN = contact.LinkedIN;
                    ContactDetails.Notes = contact.Notes;
                    ContactDetails.ContactOwner = contact.ContactOwner;

                    result = _Util.Facade.ContactFacade.UpdateContacts(ContactDetails);
                    ContactId = contact.Id.ToString();
                }
                else
                {
                    Contact model = new Contact()
                    {
                        ContactId = Guid.NewGuid(),
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        Suffix = contact.Suffix,
                        Title = contact.Title,
                        Work = contact.Work,
                        Ext = contact.Ext,
                        Mobile = contact.Mobile,
                        Email = contact.Email,
                        Role = contact.Role,
                        Facebook = contact.Facebook,
                        Twitter = contact.Twitter,
                        Instagram = contact.Instagram,
                        LinkedIN = contact.LinkedIN,
                        Notes = contact.Notes,
                        ContactOwner = contact.ContactOwner,
                        CreatedBy = CurrentLoggedInUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime()
                    };
                    result = _Util.Facade.ContactFacade.InsertContacts(model);
                    contact.ContactId = model.ContactId;
                    ContactId = model.Id.ToString();
                }
                if(result)
                {
                    _Util.Facade.ContactFacade.DeleteContactsByContactId(contact.ContactId);
                    if (contact.CustomerId!=null)
                    {
                        foreach(var item in contact.CustomerId)
                        {
                            UserContact user = new UserContact()
                            {
                                UserId=item,
                                ContactId=contact.ContactId,
                                UserType=LabelHelper.UserType.Customer
                            };
                            _Util.Facade.ContactFacade.InsertUserContacts(user);
                        }
                    }
                    if (contact.LeadId != null)
                    {
                        foreach (var item in contact.LeadId)
                        {
                            UserContact user = new UserContact()
                            {
                                UserId = item,
                                ContactId = contact.ContactId,
                                UserType = LabelHelper.UserType.Lead
                            };
                            _Util.Facade.ContactFacade.InsertUserContacts(user);
                        }
                    }
                    if (contact.OpportunityId != null)
                    {
                        foreach (var item in contact.OpportunityId)
                        {
                            UserContact user = new UserContact()
                            {
                                UserId = item,
                                ContactId = contact.ContactId,
                                UserType = LabelHelper.UserType.Opportunity
                            };
                            _Util.Facade.ContactFacade.InsertUserContacts(user);
                        }
                    }
                }
            }
            if(ListTag != null && ListTag.Count > 0)
            {
                var objtagmap = _Util.Facade.CredentialSettingFacade.GetAllTagMapByContactid(contact.ContactId);
                if(objtagmap != null && objtagmap.Count > 0)
                {
                    foreach(var map in objtagmap)
                    {
                        _Util.Facade.CredentialSettingFacade.DeleteTagMapById(map.Id);
                    }
                }
                foreach(var item in ListTag)
                {
                    RMRTagMap tagmap = new RMRTagMap()
                    {
                        TagId = new Guid(item),
                        ContactId = contact.ContactId,
                        CreatedBy = CurrentLoggedInUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedBy = CurrentLoggedInUser.UserId,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime()
                    };
                    _Util.Facade.CredentialSettingFacade.InsertTagMap(tagmap);
                }
            }
            return Json( new { result = result, FromCustomer = contact.FromCustomer, ContactTab = contact.ContactTab, ContactId = ContactId } );
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteContact(int Id, string ContactTab)
        {
            bool result = false;
            bool res = false;
            if (!string.IsNullOrWhiteSpace(ContactTab))
            {
                res = Convert.ToBoolean(ContactTab);
            }
            if (Id>0)
            {
                long id = _Util.Facade.ContactFacade.DeleteContact(Id);
                if(id > 0)
                {
                    result = true;
                   
                }
                return Json(new { result = true, message = "Contact Deleted successfully." ,ContactTab = res });
            }   
            else
            {
                return Json(new { result = false, message = "Internal error. Please report to system admin." });
            }

        }
        [Authorize]
        public ActionResult GetContactFilterList(ContactFilter filter)
        {
          
            List<Contact> ContactList = new List<Contact>();
         
            ContactList = _Util.Facade.ContactFacade.GetFilteredContacts(filter);
            return new ViewAsPdf(ContactList);

        }

        public ActionResult GetContactList()
        {
            List<string> tag = new List<string>();
            string query = Request.QueryString["q[term]"] != null ? Request.QueryString["q[term]"].ToString() : "";
            string GuidList = Request.QueryString["data[]"] != null ? Request.QueryString["data[]"].ToString() : "";
            if (!string.IsNullOrWhiteSpace(GuidList))
            {
                string[] sguid = GuidList.Split(',');
                if(sguid.Length > 0)
                {
                    foreach(var item in sguid)
                    {
                        tag.Add(item);
                    }
                }
                GuidList = string.Format("'{0}'", string.Join("','", tag.Select(i => i.Replace("'", "''"))));
            }
            var contactlist = _Util.Facade.CredentialSettingFacade.GetAllTagListByQuery(query, GuidList);
            List<Dictionary<string, object>>
             lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = null;

            foreach (DataRow dr in contactlist.Rows)
            {
                dictRow = new Dictionary<string, object>();
                foreach (DataColumn col in contactlist.Columns)
                {
                    dictRow.Add(col.ColumnName, dr[col]);
                }
                lstRows.Add(dictRow);
            }

            return Json(lstRows, JsonRequestBehavior.AllowGet);
        }
    }
}