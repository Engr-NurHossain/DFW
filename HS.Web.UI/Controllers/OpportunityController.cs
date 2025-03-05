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
    public class OpportunityController : BaseController
    {
        // GET: Opportunity
        [Authorize]
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
        public ActionResult Opportunities(string opportunityAdd, string ADDid)
        {
            if (!string.IsNullOrWhiteSpace(opportunityAdd) && opportunityAdd == "opportunityAdd")
            {
                ViewBag.opportunityAdd = true;
                ViewBag.ADDid = ADDid;
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.OpportunityType = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityType").Select(x =>
                     new SelectListItem()
                     {
                         Text = x.DisplayText.ToString(),
                         Value = x.DataValue.ToString()
                     }).ToList();
            ViewBag.OpportunityStatus = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityStatus").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityProbability = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityProbability").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityDealReason = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityDealReason").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityYesNo = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityYesNo").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityDeliveryDays = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityDeliveryDays").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityCampaignSource = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityCampaignSource").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();

            List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);
            if (EmpList != null && EmpList.Count > 0)
            {
                ViewBag.EmployeeList = EmpList.OrderBy(x => x.FirstName).Select(x =>
               new SelectListItem()
               {
                   Text = x.FirstName + " " + x.LastName,
                   Value = x.UserId.ToString()
               }).ToList();

            }

            ViewBag.EmployeeList.Insert(0, new SelectListItem()
            {
                Text = "Select Account Owner",
                Value = Guid.Empty.ToString()
            });

            return View();
        }

        [Authorize]
        public PartialViewResult OpportunityDetailTab(int id, string OpportunityName, string Tablink, string noteid, string timeval, string IsComplete)
        {
            string OpportunityGuid = "";
            //if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerDetails))
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}

            List<OpportunityTabModel> Model = new List<OpportunityTabModel>();
            if (id > 0)
            {
                if (string.IsNullOrWhiteSpace(OpportunityName))
                {
                    Opportunity tmOpp = _Util.Facade.OpportunityFacade.GetOpportunityById(id);
                    if (tmOpp != null)
                    {
                        if (!string.IsNullOrWhiteSpace(tmOpp.OpportunityName))
                        {
                            OpportunityName = tmOpp.OpportunityName;
                        }
                        else
                        {
                            OpportunityName = tmOpp.Id.ToString();
                        }
                        OpportunityGuid = tmOpp.OpportunityId.ToString();

                    }

                }
                if (Request.Cookies[CookieKeys.Opportunity] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.Opportunity].Value))
                {

                    //HttpUtility.UrlDecode(Request.Cookies["__favProp"].Value);

                    string cookie = HttpUtility.UrlDecode(Request.Cookies[CookieKeys.Opportunity].Value); //Request.Cookies[CookieKeys.Customer].Value;
                    string[] OppList = cookie.Split('|');
                    foreach (var item in OppList)
                    {
                        if (string.IsNullOrWhiteSpace(item))
                        {
                            continue;
                        }
                        string[] itm = item.Split(',');
                        if (itm.Count() > 2)
                        {
                            Model.Add(new OpportunityTabModel()
                            {
                                OpportunityId = Convert.ToInt32(itm[0]),
                                OpportunityName = itm[1],
                                OpportunityGuid = itm[2]
                            });
                        }
                    }
                    if (Model.Count() > 0)
                    {

                        if (Model.Where(x => x.OpportunityId == id).Count() > 0)
                        {
                            var index = Model.FindIndex(x => x.OpportunityId == id);
                            var item = Model[index];
                            Model[index] = Model[0];
                            Model[0] = item;
                        }
                        else
                        {
                            Model.Insert(0, new OpportunityTabModel()
                            {
                                OpportunityId = id,
                                OpportunityName = OpportunityName,
                                OpportunityGuid = OpportunityGuid
                            });
                        }
                    }
                }
                else
                {
                    Model.Add(new OpportunityTabModel()
                    {
                        OpportunityId = id,
                        OpportunityName = OpportunityName,
                        OpportunityGuid = OpportunityGuid
                    });
                }
                string newCookie = "";
                for (int i = 0; i < Model.Count() && i < 4; i++)
                {
                    newCookie += string.Format("{0},{1},{2}|", Model[i].OpportunityId, Model[i].OpportunityName, Model[i].OpportunityGuid);
                }
                HttpCookie myCookie = new HttpCookie(CookieKeys.Opportunity);
                myCookie.Value = newCookie;// HttpUtility.UrlEncode(cookie);
                myCookie.Expires = DateTime.Now.UTCCurrentTime().AddDays(2d);
                Response.Cookies.Add(myCookie);
            }
            ViewBag.tablink = Tablink;
            ViewBag.noteid = Convert.ToInt32(noteid);
            ViewBag.time = timeval;
            ViewBag.complete = IsComplete;
            return PartialView("_OpportunityDetailTab", Model.GetRange(0, (Model.Count() <= 4 ? Model.Count() : 4)));
        }
        [Authorize]
        public ActionResult OpportunityDetails(int id, string tab, string noteid, string timeval, string IsComplete)
        {
            if (id == 0)
            {
                return null;
            }
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //bool result = _Util.Facade.CustomerFacade.CustomerIsInCompany(id, CurrentUser.CompanyId.Value);
            //if (!result)
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}
            Opportunity model = _Util.Facade.OpportunityFacade.GetOpportunyDetailById(id);
            if (model != null)
            {

                return PartialView("_OpportunityDetails", model);
            }
            else
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveOpportunityImportFile(string File)
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
                        int Probability;
                        Opportunity opportunity = new Opportunity();
                        Guid OpportunityId = new Guid();
                        Guid ContactOwner = new Guid();
                        Guid CustomerId = new Guid();
                        Guid AccountOwner = new Guid();
                        Guid CreatedBy = new Guid();
                        Guid AccessGivenTo = new Guid();

                        for (int j = 1; j <= colCount; j++)
                        {
                            if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                            {
                                try
                                {
                                    var value = xlRange.Cell(i, j).Value.ToString();
                                    var header = xlRange.Cell(1, j).Value.ToString();
                                    if (header == "OpportunityId")
                                    {

                                        Guid.TryParse(value, out OpportunityId);
                                    }
                                    if (header == "CustomerId")
                                    {

                                        Guid.TryParse(value, out CustomerId);
                                    }
                                    if (header == "AccountOwner")
                                    {

                                        Guid.TryParse(value, out AccountOwner);
                                    }
                                    if (header == "AccessGivenTo")
                                    {

                                        Guid.TryParse(value, out AccessGivenTo);
                                    }
                                    if (header == "ContactOwner")
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
                                    else if (header == "Probability")
                                    {

                                        Int32.TryParse(value, out Probability);
                                    }
                                    else if (opportunity.GetType().GetProperty(header) != null && header == "CreatedDate")
                                    {
                                        opportunity.GetType().GetProperty(header).SetValue(opportunity, Convert.ToDateTime(value));
                                    }
                                    if (opportunity.GetType().GetProperty(header) != null)
                                    {
                                        if (header == "CreatedDate" || header == "CloseDate" || header == "LastUpdatedDate")
                                        {

                                            opportunity.GetType().GetProperty(header).SetValue(opportunity, Convert.ToDateTime(value));
                                        }
                                        else
                                        {
                                            opportunity.GetType().GetProperty(header).SetValue(opportunity, value);
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                        opportunity.OpportunityId = OpportunityId;
                        Opportunity isExistOpportunity = new Opportunity();
                        if (opportunity.OpportunityId != new Guid())
                        {
                            isExistOpportunity = _Util.Facade.OpportunityFacade.GetOpportunityByOpportunityId(opportunity.OpportunityId);
                            if (isExistOpportunity != null)
                            {
                                opportunity.Id = isExistOpportunity.Id;
                                isExistOpportunity.OpportunityId = opportunity.OpportunityId;
                                isExistOpportunity.CustomerId = opportunity.CustomerId;
                                isExistOpportunity.OpportunityName = opportunity.OpportunityName;
                                isExistOpportunity.Type = opportunity.Type;
                                isExistOpportunity.LeadSource = opportunity.LeadSource;
                                isExistOpportunity.Revenue = opportunity.Revenue;
                                isExistOpportunity.ProjectedGP = opportunity.ProjectedGP;
                                isExistOpportunity.Points = opportunity.Points;
                                isExistOpportunity.TotalProjectedGP = opportunity.TotalProjectedGP;
                                isExistOpportunity.CloseDate = opportunity.CloseDate;
                                isExistOpportunity.Status = opportunity.Status;
                                isExistOpportunity.Probability = opportunity.Probability;
                                isExistOpportunity.DealReason = opportunity.DealReason;
                                isExistOpportunity.DeliveryDays = opportunity.DeliveryDays;
                                isExistOpportunity.Competitors = opportunity.Competitors;
                                isExistOpportunity.DealReason = opportunity.DealReason;
                                isExistOpportunity.AccountOwner = opportunity.AccountOwner;
                                isExistOpportunity.CreatedBy = opportunity.CreatedBy;
                                isExistOpportunity.CreatedDate = opportunity.CreatedDate;
                                isExistOpportunity.LastUpdatedDate = DateTime.Now.UTCCurrentTime();

                                result = _Util.Facade.OpportunityFacade.UpdateOpportunity(isExistOpportunity);
                            }
                            else
                            {

                                opportunity.CreatedDate = DateTime.Now.UTCCurrentTime();
                                opportunity.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                opportunity.CreatedBy = CurrentUser.UserId;

                                _Util.Facade.OpportunityFacade.InsertOpportunity(opportunity);
                            }
                        }
                        else
                        {
                            opportunity.CreatedDate = DateTime.Now.UTCCurrentTime();
                            opportunity.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            opportunity.CreatedBy = CurrentUser.UserId;

                            _Util.Facade.OpportunityFacade.InsertOpportunity(opportunity);

                            _Util.Facade.OpportunityFacade.InsertOpportunity(opportunity);

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
        public ActionResult LoadOpportunityList(OpportunityFilter filter)
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
            OpportunityModel opportunities = new OpportunityModel();
            if (!CurrentUser.UserTags.Contains("admin"))
            {
                List<string> OpportunityIdList = new List<string>();
                List<Opportunity> OpportunityList = new List<Opportunity>();
                //OpportunityList = _Util.Facade.OpportunityFacade.GetAllOpportunitybyAccountOwner(CurrentUser.UserId);
                //if (OpportunityList.Count > 0)
                //{
                //    foreach (var item in OpportunityList)
                //    {
                //        OpportunityIdList.Add(item.OpportunityId.ToString());
                //    }
                //}
                //filter.OpportunityList = OpportunityIdList;
                filter.AccountOwnerId = CurrentUser.UserId;
                opportunities = _Util.Facade.OpportunityFacade.GetOpportunities(filter);
                ViewBag.OutOfNumber = opportunities.TotalCount.TotalCount;
            }
            else
            {
                opportunities = _Util.Facade.OpportunityFacade.GetOpportunities(filter);

                ViewBag.OutOfNumber = opportunities.TotalCount.TotalCount;
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



            return View(opportunities.OpportunityList);
        }
        [Authorize]
        public ActionResult AddOpportunity(int? id, Guid? CustomerId, string opportunityTab)
        {
            bool res = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!string.IsNullOrWhiteSpace(opportunityTab))
            {
                res = Convert.ToBoolean(opportunityTab); ;
            }
            Opportunity model = new Opportunity();
            if (id.HasValue && id > 0)
            {
                model = _Util.Facade.OpportunityFacade.GetOpportunityById(id.Value);

                #region ViewBag
                List<SelectListItem> AccessAssignedPersons = new List<SelectListItem>();
                AccessAssignedPersons.Add(new SelectListItem()
                {
                    Text = "Access Assign To",
                    Value = "-1"
                });
                AccessAssignedPersons.AddRange(_Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value).Select(x => new SelectListItem()
                {
                    Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                    Value = x.UserId.ToString()
                }).ToList());
                ViewBag.AccessAssignedPersons = AccessAssignedPersons.OrderBy(x => x.Text != "Access Assign To").ThenBy(x => x.Text).ToList();

                #endregion
            }
            else
            {
                model.AccountOwner = CurrentLoggedInUser.UserId;
                model.opportunityTab = res;
            }
            #region viewbag 


            List<SelectListItem> Emplist = new List<SelectListItem>();


            Emplist = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value).Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                }).ToList();

            Emplist.Insert(0, new SelectListItem()
            {
                Text = "Select One",
                Value = Guid.Empty.ToString()
            });
            ViewBag.EmployeeList = Emplist.OrderBy(x => x.Text != "Select One").ThenBy(x => x.Text).ToList();


            ViewBag.SourceLead = _Util.Facade.LookupFacade.GetLookupByKeyWithParent("LeadSource").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();

            ViewBag.OpportunityType = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityType").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityStatus = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityStatus").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityProbability = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityProbability").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityDealReason = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityDealReason").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityYesNo = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityYesNo").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityDeliveryDays = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityDeliveryDays").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityCampaignSource = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityCampaignSource").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.VehicleCondition = _Util.Facade.LookupFacade.GetLookupByKey("VehicleCondition").Select(x =>
                  new SelectListItem()
                  {
                      Text = x.DisplayText.ToString(),
                      Value = x.DataValue.ToString()
                  }).ToList();
            List<SelectListItem> CustomerSelectList = new List<SelectListItem>();
            if (CustomerId.HasValue && CustomerId != Guid.Empty)
            {
                Customer RefCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId.Value);
                if (RefCustomer != null)
                {
                    CustomerSelectList.Add(new SelectListItem
                    {
                        Text = string.IsNullOrWhiteSpace(RefCustomer.BusinessName) ? (RefCustomer.FirstName + " " + RefCustomer.LastName) : RefCustomer.BusinessName,
                        Value = RefCustomer.CustomerId.ToString()
                    });
                }
                ViewBag.FromCustomer = CustomerId;

            }
            else
            {
                ViewBag.FromCustomer = null;
                if (model.CustomerId == new Guid())
                {
                    CustomerSelectList.Add(new SelectListItem
                    {
                        Text = HS.Web.UI.Helper.LanguageHelper.T("Customer"),
                        Value = "00000000-0000-0000-0000-000000000000"
                    });
                }
                else
                {
                    Customer RefCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.CustomerId);
                    if (RefCustomer != null)
                    {
                        CustomerSelectList.Add(new SelectListItem
                        {
                            Text = string.IsNullOrWhiteSpace(RefCustomer.BusinessName) ? (RefCustomer.FirstName + " " + RefCustomer.LastName) : RefCustomer.BusinessName,
                            Value = RefCustomer.CustomerId.ToString()
                        });
                    }
                }
            }

            ViewBag.CustomerList = CustomerSelectList;
            #endregion
            return View("_AddOpportunity", model);
        }


        [Authorize]
        [HttpPost]
        public JsonResult DeleteOpportunity(int Id, string opportunityTab)
        {
            bool result = false;
            bool res = false;
            if (!string.IsNullOrWhiteSpace(opportunityTab))
            {
                res = Convert.ToBoolean(opportunityTab);
            }
            if (Id > 0)
            {
                long id = _Util.Facade.OpportunityFacade.DeleteOpportunity(Id);
                if (id > 0)
                {
                    result = true;

                }
                return Json(new { result = true, message = "Opportunity Deleted successfully.", res = res });
            }
            else
            {
                return Json(new { result = false, message = "Internal error. Please report to system admin." });
            }

        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveOpportunity(Opportunity opportunity)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var result = false;
            string OpportunityId = "";
            if (opportunity != null)
            {
                if (opportunity.Id > 0)
                {
                    var OpportunityDetails = _Util.Facade.OpportunityFacade.GetOpportunityById(opportunity.Id);
                    if (OpportunityDetails.CloseDate != opportunity.CloseDate)
                    {
                        OpportunityDetails.CloseDateSetDate = DateTime.Now.UTCCurrentTime();
                    }
                    OpportunityDetails.CustomerId = opportunity.CustomerId;
                    OpportunityDetails.OpportunityName = opportunity.OpportunityName;
                    OpportunityDetails.Type = opportunity.Type;
                    OpportunityDetails.LeadSource = opportunity.LeadSource;
                    OpportunityDetails.Revenue = opportunity.Revenue;
                    OpportunityDetails.ProjectedGP = opportunity.ProjectedGP;
                    OpportunityDetails.Points = opportunity.Points;
                    OpportunityDetails.TotalProjectedGP = opportunity.TotalProjectedGP;
                    OpportunityDetails.CloseDate = opportunity.CloseDate;
                    OpportunityDetails.Status = opportunity.Status;
                    OpportunityDetails.Probability = opportunity.Probability;
                    OpportunityDetails.DealReason = opportunity.DealReason;
                    OpportunityDetails.IsForecast = opportunity.IsForecast;
                    OpportunityDetails.DeliveryDays = opportunity.DeliveryDays;
                    OpportunityDetails.Competitors = opportunity.Competitors;
                    OpportunityDetails.CampaignSource = opportunity.CampaignSource;
                    OpportunityDetails.AccountOwner = opportunity.AccountOwner;
                    OpportunityDetails.AccessGivenTo = opportunity.AccessGivenTo;
                    OpportunityDetails.LastUpdatedDate = DateTime.Now;
                    OpportunityId = OpportunityDetails.Id.ToString();
                    OpportunityDetails.VehicleCondition = opportunity.VehicleCondition;
                    result = _Util.Facade.OpportunityFacade.UpdateOpportunity(OpportunityDetails);
                    base.AddUserActivityForCustomer("Opportunity Details is updated", LabelHelper.ActivityAction.UpdateOpportunity, OpportunityDetails.CustomerId, null,null);

                }
                else
                {
                    DateTime? CloseDateSetDate = null;
                    if (opportunity.CloseDate != null && opportunity.CloseDate != new DateTime())
                    {
                        CloseDateSetDate = DateTime.Now.UTCCurrentTime();
                    }
                    Opportunity model = new Opportunity()
                    {
                        OpportunityId = Guid.NewGuid(),
                        CustomerId = opportunity.CustomerId,
                        OpportunityName = opportunity.OpportunityName,
                        Type = opportunity.Type,
                        LeadSource = opportunity.LeadSource,
                        Revenue = opportunity.Revenue,
                        ProjectedGP = opportunity.ProjectedGP,
                        Points = opportunity.Points,
                        TotalProjectedGP = opportunity.TotalProjectedGP,
                        CloseDate = opportunity.CloseDate,
                        CloseDateSetDate = CloseDateSetDate,
                        Status = opportunity.Status,
                        Probability = opportunity.Probability,
                        DealReason = opportunity.DealReason,
                        IsForecast = opportunity.IsForecast,
                        DeliveryDays = opportunity.DeliveryDays,
                        Competitors = opportunity.Competitors,
                        CampaignSource = opportunity.CampaignSource,
                        AccountOwner = opportunity.AccountOwner,
                        CreatedBy = CurrentLoggedInUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        VehicleCondition = opportunity.VehicleCondition,
                        AccessGivenTo = opportunity.AccessGivenTo
                    };

                    result = _Util.Facade.OpportunityFacade.InsertOpportunity(model);
                    base.AddUserActivityForCustomer("Opportunity Details is added", LabelHelper.ActivityAction.AddOpportunity, model.CustomerId, null,null);

                    OpportunityId = model.Id.ToString();
                }
            }
            return Json(new { result = result, fromCustomer = opportunity.FromCustomer, OpportunityTab = opportunity.opportunityTab, OpportunityId = OpportunityId });
        }

        [Authorize]
        public ActionResult GetOpportunityFilterList(OpportunityFilter filter)
        {

            List<Opportunity> opportunityList = new List<Opportunity>();
            opportunityList = _Util.Facade.OpportunityFacade.GetFilteredOpportunities(filter);


            return new ViewAsPdf(opportunityList);

        }

        [Authorize]
        public ActionResult OpportunityListPartial(Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CustomerId != new Guid())
            {
                ViewBag.OpportunityType = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityType").Select(x =>
                     new SelectListItem()
                     {
                         Text = x.DisplayText.ToString(),
                         Value = x.DataValue.ToString()
                     }).ToList();
                ViewBag.OpportunityStatus = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityStatus").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
                ViewBag.OpportunityProbability = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityProbability").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
                ViewBag.OpportunityDealReason = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityDealReason").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
                ViewBag.OpportunityYesNo = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityYesNo").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
                ViewBag.OpportunityDeliveryDays = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityDeliveryDays").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
                ViewBag.OpportunityCampaignSource = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityCampaignSource").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
                List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);
                ViewBag.EmployeeList = EmpList.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.FirstName + " " + x.LastName,
                        Value = x.UserId.ToString()
                    }).ToList();
                ViewBag.customerid = CustomerId;
                ViewBag.opoortunityTab = true;
            }
            return View();
        }

        public ActionResult LoadCustomerOpportunityList(OpportunityFilter filter)
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
            OpportunityModel opportunities = _Util.Facade.OpportunityFacade.GetOpportunities(filter);

            ViewBag.OutOfNumber = opportunities.TotalCount.TotalCount;
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



            return View(opportunities.OpportunityList);
        }

        public ActionResult AddCustomerOpportunity(int? id, Guid? CustomerId)
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            Opportunity model = new Opportunity();
            if (id.HasValue && id > 0)
            {
                model = _Util.Facade.OpportunityFacade.GetOpportunityById(id.Value);
            }


            #region viewbag 
            ViewBag.SourceLead = _Util.Facade.LookupFacade.GetLookupByKeyWithParent("LeadSource").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();

            ViewBag.OpportunityType = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityType").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityStatus = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityStatus").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityProbability = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityProbability").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityDealReason = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityDealReason").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityYesNo = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityYesNo").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityDeliveryDays = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityDeliveryDays").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            ViewBag.OpportunityCampaignSource = _Util.Facade.LookupFacade.GetLookupByKey("OpportunityCampaignSource").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();

            List<SelectListItem> CustomerSelectList = new List<SelectListItem>();
            if (CustomerId.HasValue && CustomerId != Guid.Empty)
            {
                Customer RefCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId.Value);
                if (RefCustomer != null)
                {
                    CustomerSelectList.Add(new SelectListItem
                    {
                        Text = string.IsNullOrWhiteSpace(RefCustomer.BusinessName) ? (RefCustomer.FirstName + " " + RefCustomer.LastName) : RefCustomer.BusinessName,
                        Value = RefCustomer.CustomerId.ToString()
                    });
                }
                ViewBag.FromCustomer = CustomerId;

            }
            else
            {
                ViewBag.FromCustomer = null;
                if (model.CustomerId == new Guid())
                {
                    CustomerSelectList.Add(new SelectListItem
                    {
                        Text = HS.Web.UI.Helper.LanguageHelper.T("Customer"),
                        Value = "00000000-0000-0000-0000-000000000000"
                    });
                }
                else
                {
                    Customer RefCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.CustomerId);
                    if (RefCustomer != null)
                    {
                        CustomerSelectList.Add(new SelectListItem
                        {
                            Text = string.IsNullOrWhiteSpace(RefCustomer.BusinessName) ? (RefCustomer.FirstName + " " + RefCustomer.LastName) : RefCustomer.BusinessName,
                            Value = RefCustomer.CustomerId.ToString()
                        });
                    }
                }
            }

            ViewBag.CustomerList = CustomerSelectList;

            List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentLoggedInUser.CompanyId.Value);
            ViewBag.EmployeeList = EmpList.Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString()
                }).ToList();
            #endregion
            return View(model);
        }
    }
}