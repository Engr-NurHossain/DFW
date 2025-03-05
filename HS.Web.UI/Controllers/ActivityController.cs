using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using HS.Entities;
using ClosedXML.Excel;
using Excel = HS.Web.UI.Helper.ExcelFormatHelper;
using System.Data;
using System.IO;
using HS.Web.UI.Helper;
using HS.Framework;
using NsExcel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Rotativa.Options;
using NLog;

namespace HS.Web.UI.Controllers
{
    public class ActivityController : BaseController
    {
        public ActivityController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        // GET: Activity
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
        public ActionResult Activities()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.ActivityTypeList = _Util.Facade.LookupFacade.GetLookupByKey("ActivityType").Select(x =>
                    new SelectListItem()
                    {
                        Text = x.DisplayText.ToString(),
                        Value = x.DataValue.ToString()
                    }).ToList();
            ViewBag.ActivityStatusList = _Util.Facade.LookupFacade.GetLookupByKey("ActivityStatus").Select(x =>
                      new SelectListItem()
                      {
                          Text = x.DisplayText.ToString(),
                          Value = x.DataValue.ToString()
                      }).ToList();
            List<SelectListItem> EmployeeList = new List<SelectListItem>();
            EmployeeList.Add(new SelectListItem()
            {
                Text = "Select Assign To",
                Value = Guid.Empty.ToString()
            });

            List<Employee> employeeforDropDown = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);
            if(employeeforDropDown != null && employeeforDropDown.Count > 0)
            {
                EmployeeList.AddRange(employeeforDropDown.OrderBy(x => x.FirstName).Select(x =>
                     new SelectListItem()
                     {
                         Text = x.FirstName + " " + x.LastName,
                         Value = x.UserId.ToString()
                     }).ToList());
            }

            
            ViewBag.EmployeeList = EmployeeList;
            return View();
        }
        [Authorize]
        public ActionResult LoadActivityList(ActivityFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }
            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ActivityListPageSize");
            if (glob != null)
            {
                filter.UnitPerPage = Convert.ToInt32(glob.Value);
            }
            else
            {
                filter.UnitPerPage = 30;
            }
            if (filter.PageNumber == null || filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }
            ActivityModel Activity = new ActivityModel();
            ViewBag.FromCustomer = null;
            if (!CurrentUser.UserTags.Contains("admin"))
            {
                //List<string> ActivityIdList = new List<string>();
                //List<Activity> ActivityList = new List<Activity>();
                //ActivityList = _Util.Facade.ActivityFacade.GetAllActivitybyAssignTo(CurrentUser.UserId);
                //if (ActivityList.Count > 0)
                //{
                //    foreach (var item in ActivityList)
                //    {
                //        ActivityIdList.Add(item.ActivityId.ToString());
                //    }
                //}
                //filter.ActivityList = ActivityIdList;
                filter.AssignToId = CurrentUser.UserId;
                Activity = _Util.Facade.ActivityFacade.GetActivities(filter);
                ViewBag.OutOfNumber = Activity.TotalCount.TotalCount;
            }
            else
            {
                Activity = _Util.Facade.ActivityFacade.GetActivities(filter);
                ViewBag.OutOfNumber = Activity.TotalCount.TotalCount;
            }
            if ((int)ViewBag.OutOfNumber == 0)
            {
                ViewBag.Message = "No Content Available !";
            }


            if (ViewBag.OutOfNumber == 0)
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
            ViewBag.searchtext = filter.SearchText;


            return View(Activity.ActivityList);
        }
        [Authorize]
        public ActionResult ActivityDetails(int id, string tab, string noteid, string timeval, string IsComplete)
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
            Activity model = _Util.Facade.ActivityFacade.GetActivityInfoById(id);
            if (model != null)
            {

                return PartialView("_ActivityDetails", model);
            }
            else
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

        }
        [Authorize]
        public ActionResult AddActivity(int? Id, Guid? CustomerId, string activityTab)
        {
            bool res = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!string.IsNullOrWhiteSpace(activityTab))
            {
                res = Convert.ToBoolean(activityTab);
            }
            Activity Model = new Activity() {
                ActivityType = "-1",
                Status = "-1",
                ActivityId = Guid.NewGuid(),
                AssignedTo = CurrentUser.UserId,
                ActivityTab = res
            };
            ViewBag.CustomerId = null;
            Customer RefAccount = new Customer();
            if (CustomerId != null && CustomerId != Guid.Empty)
            {
                ViewBag.CustomerId = CustomerId;
                RefAccount = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId.Value);
            }
            if (Id.HasValue && Id > 0)
            {
                Model = _Util.Facade.ActivityFacade.GetActivityById(Id.Value);
            }
            #region Viewbags
            ViewBag.ActivityTypeList = _Util.Facade.LookupFacade.GetLookupByKey("ActivityType").OrderBy(x => x.DisplayText != "Select One").ThenBy(x =>x.DisplayText).Select(x =>
                      new SelectListItem()
                      {
                          Text = x.DisplayText.ToString(),
                          Value = x.DataValue.ToString()
                      }).ToList();
            ViewBag.ActivityStatusList = _Util.Facade.LookupFacade.GetLookupByKey("ActivityStatus").Select(x =>
                      new SelectListItem()
                      {
                          Text = x.DisplayText.ToString(),
                          Value = x.DataValue.ToString()
                      }).ToList();
            List<SelectListItem> EmployeeList = new List<SelectListItem>();
            EmployeeList.Add(new SelectListItem() {
                Text ="Select One",
                Value = Guid.Empty.ToString()
            });


            List<Employee> employeeforDropDown = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);
            if (employeeforDropDown != null && employeeforDropDown.Count > 0)
            {
                EmployeeList.AddRange(employeeforDropDown.OrderBy(x => x.FirstName != "Select One").ThenBy(x => x.FirstName).Select(x =>
                     new SelectListItem()
                     {
                         Text = x.FirstName + " " + x.LastName,
                         Value = x.UserId.ToString()
                     }).ToList());
            }
            ViewBag.EmployeeList = EmployeeList;

            List<SelectListItem> notifyby = new List<SelectListItem>();
            notifyby.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("NotifyBy").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.notifybylist = notifyby;

            List<SelectListItem> AssociatedWithList = new List<SelectListItem>();
            AssociatedWithList.Add(new SelectListItem()
            {
                Text = "Select One",
                Value = "-1"
            });
            AssociatedWithList.Add(new SelectListItem()
            {
                Text = LabelHelper.ActivityAssociateType.Opportunity,
                Value = LabelHelper.ActivityAssociateType.Opportunity
            });
            AssociatedWithList.Add(new SelectListItem()
            {
                Text = LabelHelper.ActivityAssociateType.Lead,
                Value = LabelHelper.ActivityAssociateType.Lead
            });
            AssociatedWithList.Add(new SelectListItem()
            {
                Text = LabelHelper.ActivityAssociateType.Account,
                Value = LabelHelper.ActivityAssociateType.Account
            });
            AssociatedWithList.Add(new SelectListItem()
            {
                Text = LabelHelper.ActivityAssociateType.Contact,
                Value = LabelHelper.ActivityAssociateType.Contact
            });
            if (AssociatedWithList != null && AssociatedWithList.Count > 0)
            {
                AssociatedWithList = AssociatedWithList.OrderBy(x => x.Text != "Select One").ThenBy(x => x.Text).ToList();
            }

            ViewBag.AssociatedWithList = AssociatedWithList;
            
            #region Customer List
            List<SelectListItem> CustomerSelectList = new List<SelectListItem>();
            List<SelectListItem> LeadSelectList = new List<SelectListItem>();
            List<SelectListItem> OpportunitySelectList = new List<SelectListItem>();
            List<SelectListItem> ContactSelectList = new List<SelectListItem>();
            if (Model.AssociatedWith != Guid.Empty && (Model.AssociatedType == "Account" || Model.AssociatedType == "Lead" || Model.AssociatedType == "Opportunity"))
            {
                Customer RefCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.AssociatedWith);
                Opportunity RefOpportunity = _Util.Facade.OpportunityFacade.GetOpportunityByOpportunityId(Model.AssociatedWith);
                if (RefCustomer != null)
                {
                    CustomerSelectList.Add(new SelectListItem
                    {
                        Text = (RefCustomer.DBA) + (string.IsNullOrWhiteSpace(RefCustomer.BusinessName) ? "" : " (" + RefCustomer.BusinessName + ")"),
                        Value = RefCustomer.CustomerId.ToString()
                    });
                    LeadSelectList.Add(new SelectListItem
                    {
                        Text = (RefCustomer.DBA) + (string.IsNullOrWhiteSpace(RefCustomer.BusinessName) ? "" : " (" + RefCustomer.BusinessName + ")"),
                        Value = RefCustomer.CustomerId.ToString()
                    });
                }
                else if(RefOpportunity != null)
                {
                    OpportunitySelectList.Add(new SelectListItem
                    {
                        Text = RefOpportunity.OpportunityName,
                        Value = RefOpportunity.OpportunityId.ToString()
                    });
                }
                else
                {
                  
                    if (Model.AssociatedWith == new Guid())
                    {
                        ContactSelectList.Add(new SelectListItem
                        {
                            Text = "Contact",
                            Value = "00000000-0000-0000-0000-000000000000"
                        });
                     
                        if (RefAccount != null)
                        {
                            CustomerSelectList.Add(new SelectListItem
                            {
                                Text = (RefAccount.DBA) + (string.IsNullOrWhiteSpace(RefAccount.BusinessName) ? "" : " (" + RefAccount.BusinessName + ")"),
                                Value = RefAccount.CustomerId.ToString(),
                                Selected = true
                            });
                        }
                        else
                        {
                            CustomerSelectList.Add(new SelectListItem
                            {
                                Text = "Account",
                                Value = "00000000-0000-0000-0000-000000000000"
                            });
                        }
                      
                      
                        LeadSelectList.Add(new SelectListItem
                        {
                            Text = "Lead",
                            Value = "00000000-0000-0000-0000-000000000000"
                        });
                        OpportunitySelectList.Add(new SelectListItem
                        {
                            Text = "Opportunity",
                            Value = "00000000-0000-0000-0000-000000000000"
                        });
                    }
                }
            }
            else
            { 
                if (Model.AssociatedWith == new Guid())
                {
                 
                    if (RefAccount != null)
                    {
                        CustomerSelectList.Add(new SelectListItem
                        {
                            Text = (RefAccount.DBA) + (string.IsNullOrWhiteSpace(RefAccount.BusinessName) ? "" : " (" + RefAccount.BusinessName + ")"),
                            Value = RefAccount.CustomerId.ToString(),
                            Selected = true
                        });
                    }
                    else
                    {
                        CustomerSelectList.Add(new SelectListItem
                        {
                            Text = "Account",
                            Value = "00000000-0000-0000-0000-000000000000"
                        });
                    }
                    LeadSelectList.Add(new SelectListItem
                    {
                        Text = "Lead",
                        Value = "00000000-0000-0000-0000-000000000000"
                    });
                    OpportunitySelectList.Add(new SelectListItem
                    {
                        Text = "Opportunity",
                        Value = "00000000-0000-0000-0000-000000000000"
                    });
                } 
            }
            ViewBag.ContactList = ContactSelectList;
            ViewBag.CustomerList = CustomerSelectList;
            ViewBag.LeadList = LeadSelectList;
            ViewBag.OpportunityList = OpportunitySelectList;
            #endregion




            #endregion
            ViewBag.origin = _Util.Facade.LookupFacade.GetLookupByKey("ActivityOrigin").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            });

            ViewBag.ActivityDepartment = _Util.Facade.LookupFacade.GetLookupByKey("ActivityDepartment").Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList();

            return View(Model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddActivity(Activity Activity)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //if(Activity.ActivityType != "-1")
            //{
                if (Activity.Id > 0)
                {
                    Activity Old = _Util.Facade.ActivityFacade.GetActivityById(Activity.Id);

                    Activity.CreatedBy = Old.CreatedBy;
                    Activity.CreatedDate = Old.CreatedDate;
                if(Activity.DueDate.HasValue)
                {
                    Activity.DueDate = Activity.DueDate.Value.ClientToUTCTime();
                } 

                    _Util.Facade.ActivityFacade.UpdateActivity(Activity);
                    return Json(new { result = true, message = "Activity updated successfully.", FromCustomer = Activity.FromCustomer, ActivityTab = Activity.ActivityTab, customerid = Activity.AssociatedWith });
                }
                else
                {
                    Activity.CreatedBy = CurrentUser.UserId;
                    Activity.CreatedDate = DateTime.Now.UTCCurrentTime();
                if (Activity.DueDate.HasValue)
                {
                    Activity.DueDate = Activity.DueDate.Value.ClientToUTCTime();
                }
                _Util.Facade.ActivityFacade.InsertActivity(Activity);
                    return Json(new { result = true, message = "Activity added successfully.", FromCustomer = Activity.FromCustomer, ActivityTab = Activity.ActivityTab, customerid = Activity.AssociatedWith });
                }
            //}
            //else
            //{
            //    return Json(new { result = false, message = "Please select an activity type." });
            //}
           

            
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteActivity(int Id, string ActivityTab)
        {
            bool result = false;
            bool res = false;
            if (!string.IsNullOrWhiteSpace(ActivityTab))
            {
                res = Convert.ToBoolean(ActivityTab);
            }
            if (Id > 0)
            {
                long id = _Util.Facade.ActivityFacade.DeleteActivity(Id);
                if (id > 0)
                {
                    result = true;

                }
                return Json(new { result = true, message = "Activity Deleted successfully.", res = res });
            }
            else
            {
                return Json(new { result = false, message = "Internal error. Please report to system admin." });
            }

        }
        [Authorize]
        public ActionResult GetActivityFilterList(ActivityFilter filter)
        {
            
            List<Activity> ActivityList = new List<Activity>();
            ActivityList = _Util.Facade.ActivityFacade.GetFilteredActivities(filter);
  
            //Invoice model = new Invoice();
            //model = _Util.Facade.InvoiceFacade.GetInvoiceById(Id);
            //model.InvoiceListDetail = _Util.Facade.InvoiceFacade.GetInvoiceDetialsListByInvoiceId(model.InvoiceId);
           
            return new ViewAsPdf(ActivityList);

        }

        [Authorize]
        public ActionResult ActivityListPartial(Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CustomerId != new Guid())
            {
                ViewBag.ActivityTypeList = _Util.Facade.LookupFacade.GetLookupByKey("ActivityType").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList();
                ViewBag.ActivityStatusList = _Util.Facade.LookupFacade.GetLookupByKey("ActivityStatus").Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
                List<SelectListItem> EmployeeList = new List<SelectListItem>();
                EmployeeList.Add(new SelectListItem()
                {
                    Text = "Select One",
                    Value = Guid.Empty.ToString()
                });
                List<Employee> EmployeeListDropDown = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);
                if(EmployeeListDropDown != null && EmployeeListDropDown.Count > 0)
                {
                    EmployeeList.AddRange(EmployeeListDropDown.OrderBy(x => x.FirstName).Select(x =>
                         new SelectListItem()
                         {
                             Text = x.FirstName + " " + x.LastName,
                             Value = x.UserId.ToString()
                         }).ToList());
                }
                
                ViewBag.EmployeeList = EmployeeList;
                ViewBag.customerid = CustomerId;
                ViewBag.activityTab = true;
            }
            return View("_ActivityListPartial");
        }

        [Authorize]
        public ActionResult LoadActivityListPartial(ActivityFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }
            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ActivityListPageSize");
            if (glob != null)
            {
                filter.UnitPerPage = Convert.ToInt32(glob.Value);
            }
            else
            {
                filter.UnitPerPage = 30;
            }
            if (filter.PageNumber == null || filter.PageNumber == 0)
            {
                filter.PageNumber = 1;
            }
            ActivityModel Activity = new ActivityModel();
            ViewBag.FromCustomer = null;

            Activity = _Util.Facade.ActivityFacade.GetActivities(filter);
            ViewBag.OutOfNumber = Activity.TotalCount.TotalCount;



            if ((int)ViewBag.OutOfNumber == 0)
            {
                ViewBag.Message = "No Content Available !";
            }


            if (ViewBag.OutOfNumber == 0)
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



            return View(Activity.ActivityList);
        }

        [Authorize]
        public ActionResult AddActivityCustomer(int? Id, Guid customerid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Activity Model = new Activity()
            {
                ActivityType = "-1",
                Status = "-1",
                ActivityId = Guid.NewGuid(),
                AssignedTo = CurrentUser.UserId
            };

            if (Id.HasValue && Id > 0)
            {
                Model = _Util.Facade.ActivityFacade.GetActivityById(Id.Value);
            }
            #region Viewbags
            ViewBag.ActivityTypeList = _Util.Facade.LookupFacade.GetLookupByKey("ActivityType").Select(x =>
                      new SelectListItem()
                      {
                          Text = x.DisplayText.ToString(),
                          Value = x.DataValue.ToString()
                      }).ToList();
            ViewBag.ActivityStatusList = _Util.Facade.LookupFacade.GetLookupByKey("ActivityStatus").Select(x =>
                      new SelectListItem()
                      {
                          Text = x.DisplayText.ToString(),
                          Value = x.DataValue.ToString()
                      }).ToList();
            List<SelectListItem> EmployeeList = new List<SelectListItem>();
            EmployeeList.Add(new SelectListItem()
            {
                Text = "Select One",
                Value = Guid.Empty.ToString()
            });
            List<Employee> EmployeeListDropDown = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);
            if (EmployeeListDropDown != null && EmployeeListDropDown.Count > 0)
            {
                EmployeeList.AddRange(EmployeeListDropDown.OrderBy(x => x.FirstName != "Select One").ThenBy(x => x.FirstName).Select(x =>
                                         new SelectListItem()
                                         {
                                             Text = x.FirstName + " " + x.LastName,
                                             Value = x.UserId.ToString()
                                         }).ToList());

            }

            ViewBag.EmployeeList = EmployeeList;

            if(customerid != new Guid())
            {
                Model.AssociatedWith = customerid;
                Model.AssociatedType = "Account";
            }
            List<SelectListItem> AssociatedWithList = new List<SelectListItem>();
            AssociatedWithList.Add(new SelectListItem()
            {
                Text = "Select One",
                Value = "-1"
            });
            AssociatedWithList.Add(new SelectListItem()
            {
                Text = "Opportunity",
                Value = "Opportunity"
            });
            AssociatedWithList.Add(new SelectListItem()
            {
                Text = "Lead",
                Value = "Lead"
            });
            AssociatedWithList.Add(new SelectListItem()
            {
                Text = "Account",
                Value = "Account"
            });
            AssociatedWithList = AssociatedWithList.OrderBy(x => x.Text != "Select One").ThenBy(x => x.Text).ToList();
            ViewBag.AssociatedWithList = AssociatedWithList;


            #region Customer List
            List<SelectListItem> CustomerSelectList = new List<SelectListItem>();
            List<SelectListItem> LeadSelectList = new List<SelectListItem>();
            List<SelectListItem> OpportunitySelectList = new List<SelectListItem>();
            if (Model.AssociatedWith != Guid.Empty && (Model.AssociatedType == "Account" || Model.AssociatedType == "Lead" || Model.AssociatedType == "Opportunity"))
            {
                Customer RefCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.AssociatedWith);
                Opportunity RefOpportunity = _Util.Facade.OpportunityFacade.GetOpportunityByOpportunityId(Model.AssociatedWith);
                if (RefCustomer != null)
                {
                    CustomerSelectList.Add(new SelectListItem
                    {
                        Text = (RefCustomer.DBA) + (string.IsNullOrWhiteSpace(RefCustomer.BusinessName) ? "" : " (" + RefCustomer.BusinessName + ")"),
                        Value = RefCustomer.CustomerId.ToString()
                    });
                    LeadSelectList.Add(new SelectListItem
                    {
                        Text = (RefCustomer.DBA) + (string.IsNullOrWhiteSpace(RefCustomer.BusinessName) ? "" : " (" + RefCustomer.BusinessName + ")"),
                        Value = RefCustomer.CustomerId.ToString()
                    });
                }
                else if (RefOpportunity != null)
                {
                    OpportunitySelectList.Add(new SelectListItem
                    {
                        Text = RefOpportunity.OpportunityName,
                        Value = RefOpportunity.OpportunityId.ToString()
                    });
                }
                else
                {
                    if (Model.AssociatedWith == new Guid())
                    {
                        CustomerSelectList.Add(new SelectListItem
                        {
                            Text = "Account",
                            Value = "00000000-0000-0000-0000-000000000000"
                        });
                        LeadSelectList.Add(new SelectListItem
                        {
                            Text = "Lead",
                            Value = "00000000-0000-0000-0000-000000000000"
                        });
                        OpportunitySelectList.Add(new SelectListItem
                        {
                            Text = "Opportunity",
                            Value = "00000000-0000-0000-0000-000000000000"
                        });
                    }
                }
            }
            else
            {
                if (Model.AssociatedWith == new Guid())
                {
                    CustomerSelectList.Add(new SelectListItem
                    {
                        Text = "Account",
                        Value = "00000000-0000-0000-0000-000000000000"
                    });
                    LeadSelectList.Add(new SelectListItem
                    {
                        Text = "Lead",
                        Value = "00000000-0000-0000-0000-000000000000"
                    });
                    OpportunitySelectList.Add(new SelectListItem
                    {
                        Text = "Opportunity",
                        Value = "00000000-0000-0000-0000-000000000000"
                    });
                }
            }
            ViewBag.CustomerList = CustomerSelectList;
            ViewBag.LeadList = LeadSelectList;
            ViewBag.OpportunityList = OpportunitySelectList;
            #endregion




            #endregion


            return View(Model);
        }

        public ActionResult ShowNoteEmail(int Id)
        {
            Activity act = new Activity();
            if(Id > 0)
            {
                act = _Util.Facade.ActivityFacade.GetActivityById(Id);
               
            }
            return View(act);
        }
    
    }
}