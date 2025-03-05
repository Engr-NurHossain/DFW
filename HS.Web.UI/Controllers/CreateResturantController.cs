using HS.Entities;
using HS.Facade;
using HS.Framework;
using HS.Web.UI.App_Start;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HS.Web.UI.Controllers
{
    public class CreateResturantController : BaseController
    {
        // GET: CreateResturant
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateResturantPartial()
        {
            return View("CreateResturant");
        }

        [HttpPost]
        public JsonResult CreateResturantIeatery(CreateResturantModel model)
        {
            bool result = false;
            Guid CompanyId = Guid.NewGuid();
            Guid UserId = Guid.NewGuid();
            Guid NewUserId = Guid.NewGuid();
            Guid EvansUserId = Guid.NewGuid();
            Guid AllensUserId = Guid.NewGuid();
            #region Master DB Create Resturant
            Organization organization = new Organization()
            {
                CompanyId = CompanyId,
                CompanyName = model.ResturantName,
                UserName = model.Email,
                EmailAdress = model.Email,
                Phone = model.Phone,
                Street = model.Street,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                Website = model.DomainName,
                ConnectionString = ConfigurationManager.AppSettings["LiveConnectionString"],
                MasterPassword = ConfigurationManager.AppSettings["MasterPassword"]
            };
            organization.Id = _Util.Facade.UserOrganizationFacade.InsertOrganization(organization);
            UserOrganization UserOrganization = new UserOrganization()
            {
                CompanyId = CompanyId,
                UserName = "administrator",
                IsActive = false
            };
            _Util.Facade.UserOrganizationFacade.InsertUserOrganization(UserOrganization);
            #endregion
            #region Live DB Create Resturant
            CompanyFacade CompanyFacade = new CompanyFacade(organization.ConnectionString);
            EmployeeFacade EmployeeFacade = new EmployeeFacade(organization.ConnectionString);
            CompanyBranchFacade CompanyBranchFacade = new CompanyBranchFacade(organization.ConnectionString);
            UserCompanyFacade UserCompanyFacade = new UserCompanyFacade(organization.ConnectionString);
            UserLoginFacade UserLoginFacade = new UserLoginFacade(organization.ConnectionString);
            PermissionFacade PermissionFacade = new PermissionFacade(organization.ConnectionString);
            MenuFacade MenuFacade = new MenuFacade(organization.ConnectionString);
            LocalizeFacade LocalizeFacade = new LocalizeFacade(organization.ConnectionString);
            GlobalSettingsFacade GlobalSettingsFacade = new GlobalSettingsFacade(organization.ConnectionString);
            GridSettingsFacade GridSettingsFacade = new GridSettingsFacade(organization.ConnectionString);
            LookupFacade LookupFacade = new LookupFacade(organization.ConnectionString);
            CustomerFacade CustomerFacade = new CustomerFacade(organization.ConnectionString);
            UserOrganizationFacade UserOrganizationFacade = new UserOrganizationFacade();
            MailFacade MailFacade = new MailFacade(organization.ConnectionString);

            Company Company = new Company()
            {
                CompanyId = CompanyId,
                CompanyName = model.ResturantName,
                UserName = model.Email,
                EmailAdress = model.Email,
                Phone = model.Phone,
                Street = model.Street,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                Website = model.DomainName,
                CompanyLogo = "/IeateryDefaultLogo/ieateryapp-logo.png"
            };
            CompanyFacade.InsertCompany(Company);
            Employee Employee = new Employee()
            {
                UserId = UserId,
                UserName = "administrator",
                Title = "Mr.",
                FirstName = "Administrator",
                LastName = "Sir",
                Email = "administrator",
                Street = model.Street,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                Phone = model.Phone,
                IsActive = true,
                IsDeleted = false,
                Recruited = true,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                LastUpdatedBy = model.Email,
                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CompanyId
            };
            EmployeeFacade.InsertEmployee(Employee);
            Employee empuser = new Employee()
            {
                UserId = NewUserId,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Street = model.Street,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                Phone = model.Phone,
                IsActive = true,
                IsDeleted = false,
                Recruited = true,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                LastUpdatedBy = model.Email,
                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CompanyId
            };
            EmployeeFacade.InsertEmployee(empuser);
            Employee evanempuser = new Employee()
            {
                UserId = EvansUserId,
                UserName = "evan@centralstationmarketing.com",
                FirstName = "Evan",
                LastName = "Islam",
                Email = "evan@centralstationmarketing.com",
                IsActive = true,
                IsDeleted = false,
                Recruited = true,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                LastUpdatedBy = model.Email,
                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CompanyId
            };
            EmployeeFacade.InsertEmployee(evanempuser);
            Employee allenempuser = new Employee()
            {
                UserId = AllensUserId,
                UserName = "allen@alifsecurity.com",
                FirstName = "Allen",
                LastName = "Alam",
                Email = "allen@alifsecurity.com",
                IsActive = true,
                IsDeleted = false,
                Recruited = true,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                LastUpdatedBy = model.Email,
                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CompanyId
            };
            EmployeeFacade.InsertEmployee(allenempuser);
            CompanyBranch CompanyBranch = new CompanyBranch()
            {
                CompanyId = CompanyId,
                Name = model.DomainName,
                Street = model.Street,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                TimeZone = "-6",
                Tax = 8.25,
                IsMainBranch = true,
                Logo = "/IeateryDefaultLogo/ieateryapp-logo.png",
                ColorLogo = "/IeateryDefaultLogo/ieateryapp-logo.png",
                EmailLogo = "/IeateryDefaultLogo/ieateryapp-logo.png"
            };
            CompanyBranchFacade.InsertCompanyBranch(CompanyBranch);
            UserBranch UserBranch = new UserBranch()
            {
                CompanyId = CompanyId,
                UserId = UserId,
                BranchId = CompanyBranch.Id
            };
            CompanyBranchFacade.InsertUserBranch(UserBranch);
            UserBranch UserBranchobj = new UserBranch()
            {
                CompanyId = CompanyId,
                UserId = NewUserId,
                BranchId = CompanyBranch.Id
            };
            CompanyBranchFacade.InsertUserBranch(UserBranchobj);
            UserBranch evanUserBranchobj = new UserBranch()
            {
                CompanyId = CompanyId,
                UserId = EvansUserId,
                BranchId = CompanyBranch.Id
            };
            CompanyBranchFacade.InsertUserBranch(evanUserBranchobj);
            UserBranch allenUserBranchobj = new UserBranch()
            {
                CompanyId = CompanyId,
                UserId = AllensUserId,
                BranchId = CompanyBranch.Id
            };
            CompanyBranchFacade.InsertUserBranch(allenUserBranchobj);
            UserCompany UserCompany = new UserCompany()
            {
                CompanyId = CompanyId,
                UserId = UserId,
                IsDefault = true
            };
            UserCompanyFacade.InsertUserCompany(UserCompany);
            UserCompany UserCompanyobj = new UserCompany()
            {
                CompanyId = CompanyId,
                UserId = NewUserId,
                IsDefault = true
            };
            UserCompanyFacade.InsertUserCompany(UserCompanyobj);
            UserCompany evanUserCompanyobj = new UserCompany()
            {
                CompanyId = CompanyId,
                UserId = EvansUserId,
                IsDefault = true
            };
            UserCompanyFacade.InsertUserCompany(evanUserCompanyobj);
            UserCompany allenUserCompanyobj = new UserCompany()
            {
                CompanyId = CompanyId,
                UserId = AllensUserId,
                IsDefault = true
            };
            UserCompanyFacade.InsertUserCompany(allenUserCompanyobj);
            UserLogin UserLogin = new UserLogin()
            {
                UserId = UserId,
                UserName = "administrator",
                Password = MD5Encryption.GetMD5HashData("@AadOyMpQx5!"),
                EmailAddress = "administrator",
                IsActive = true,
                IsDeleted = false,
                LastUpdatedBy = model.Email,
                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CompanyId
            };
            UserLoginFacade.InsertUserLogin(UserLogin);
            UserLogin UserLoginobj = new UserLogin()
            {
                UserId = NewUserId,
                UserName = model.Email,
                Password = MD5Encryption.GetMD5HashData(model.Password),
                EmailAddress = model.Email,
                IsActive = true,
                IsDeleted = false,
                LastUpdatedBy = model.Email,
                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CompanyId
            };
            UserLoginFacade.InsertUserLogin(UserLoginobj);
            UserLogin evanUserLoginobj = new UserLogin()
            {
                UserId = EvansUserId,
                UserName = "evan@centralstationmarketing.com",
                Password = MD5Encryption.GetMD5HashData("123456"),
                EmailAddress = "evan@centralstationmarketing.com",
                IsActive = true,
                IsDeleted = false,
                LastUpdatedBy = model.Email,
                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CompanyId
            };
            UserLoginFacade.InsertUserLogin(evanUserLoginobj);
            UserLogin allenUserLoginobj = new UserLogin()
            {
                UserId = AllensUserId,
                UserName = "allen@alifsecurity.com",
                Password = MD5Encryption.GetMD5HashData("123456"),
                EmailAddress = "allen@alifsecurity.com",
                IsActive = true,
                IsDeleted = false,
                LastUpdatedBy = model.Email,
                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                CompanyId = CompanyId
            };
            UserLoginFacade.InsertUserLogin(allenUserLoginobj);
            PermissionGroup PermissionGroup = new PermissionGroup()
            {
                CompanyId = CompanyId,
                Name = "SysAdmin",
                Tag = "admin"
            };
            PermissionFacade.InsertPermissionGroup(PermissionGroup);
            UserPermission UserPermission = new UserPermission()
            {
                CompanyId = CompanyId,
                UserId = UserId,
                PermissionGroupId = PermissionGroup.Id
            };
            PermissionFacade.InsertUserPermission(UserPermission);
            UserPermission UserPermissionobj = new UserPermission()
            {
                CompanyId = CompanyId,
                UserId = NewUserId,
                PermissionGroupId = PermissionGroup.Id
            };
            PermissionFacade.InsertUserPermission(UserPermissionobj);
            UserPermission evanUserPermissionobj = new UserPermission()
            {
                CompanyId = CompanyId,
                UserId = EvansUserId,
                PermissionGroupId = PermissionGroup.Id
            };
            PermissionFacade.InsertUserPermission(evanUserPermissionobj);
            UserPermission allenUserPermissionobj = new UserPermission()
            {
                CompanyId = CompanyId,
                UserId = AllensUserId,
                PermissionGroupId = PermissionGroup.Id
            };
            PermissionFacade.InsertUserPermission(allenUserPermissionobj);
            var objuserorg = UserOrganizationFacade.GetAllUserOrganizationsByUsername(model.Email);
            if(objuserorg != null && objuserorg.Count > 0)
            {
                foreach(var item in objuserorg)
                {
                    item.IsActive = false;
                    UserOrganizationFacade.UpdateUserOrganiZation(item);
                }
            }
            UserOrganization UserOrganizationobj = new UserOrganization()
            {
                CompanyId = CompanyId,
                UserName = model.Email,
                IsActive = true
            };
            UserOrganizationobj.Id = UserOrganizationFacade.InsertUserOrganizationObj(UserOrganizationobj);
            var objevanuserorg = UserOrganizationFacade.GetAllUserOrganizationsByUsername("evan@centralstationmarketing.com");
            if (objevanuserorg != null && objevanuserorg.Count > 0)
            {
                foreach (var item in objevanuserorg)
                {
                    item.IsActive = false;
                    UserOrganizationFacade.UpdateUserOrganiZation(item);
                }
            }
            UserOrganization evanUserOrganizationobj = new UserOrganization()
            {
                CompanyId = CompanyId,
                UserName = "evan@centralstationmarketing.com",
                IsActive = true
            };
            evanUserOrganizationobj.Id = UserOrganizationFacade.InsertUserOrganizationObj(evanUserOrganizationobj);
            var objallenuserorg = UserOrganizationFacade.GetAllUserOrganizationsByUsername("allen@alifsecurity.com");
            if (objallenuserorg != null && objallenuserorg.Count > 0)
            {
                foreach (var item in objallenuserorg)
                {
                    item.IsActive = false;
                    UserOrganizationFacade.UpdateUserOrganiZation(item);
                }
            }
            UserOrganization allenUserOrganizationobj = new UserOrganization()
            {
                CompanyId = CompanyId,
                UserName = "allen@alifsecurity.com",
                IsActive = true
            };
            allenUserOrganizationobj.Id = UserOrganizationFacade.InsertUserOrganizationObj(allenUserOrganizationobj);
            WebsiteLocation WebsiteLocation = new WebsiteLocation()
            {
                CompanyId = CompanyId,
                Name = model.ResturantName,
                Address = model.Street,
                City = model.City,
                State = model.State,
                Zipcode = model.ZipCode,
                CreatedDate = DateTime.Now,
                CreatedBy = UserId,
                PrimaryContact = "-1",
                DomainName = model.DomainName,
                StorePhone = model.Phone,
                UrlSlug = model.UrlSlug,
                WebsiteURL = model.WebsiteUrl,
                MetaTitle = model.MetaTitle,
                MetaDescription = model.MetaDescription
            };
            MenuFacade.InsertWebsiteLocation(WebsiteLocation);
            
            Language Language = new Language()
            {
                CompanyId = CompanyId,
                Name = "English",
                LanguageCulture = "en-US",
                Rtl = false,
                Published = true,
                DisplayOrder = 1
            };
            LocalizeFacade.InsertLanguage(Language);
            var oldcompany = CompanyFacade.GetAllCompany().FirstOrDefault();
            var objglobalsetting = GlobalSettingsFacade.GetAllGlobalSettingByCompanyId(oldcompany.CompanyId);
            if(objglobalsetting != null && objglobalsetting.Count > 0)
            {
                foreach(var item in objglobalsetting)
                {
                    item.CompanyId = CompanyId;
                    GlobalSettingsFacade.InsertGlobalSetting(item);
                }
            }
            var objgridsetting = GridSettingsFacade.GetAllGridSettingByCompanyId(oldcompany.CompanyId);
            if (objgridsetting != null && objgridsetting.Count > 0)
            {
                foreach (var item in objgridsetting)
                {
                    item.CompanyId = CompanyId;
                    GridSettingsFacade.InsertGridSetting(item);
                }
            }
            var objlookup = LookupFacade.GetAllLookupByCompanyId(oldcompany.CompanyId);
            if (objlookup != null && objlookup.Count > 0)
            {
                foreach (var item in objlookup)
                {
                    item.CompanyId = CompanyId;
                    LookupFacade.InsertLookup(item);
                }
            }
            var objlocalizeresource = LocalizeFacade.GetAllLocalizeResourceByCompanyId(oldcompany.CompanyId);
            if (objlocalizeresource != null && objlocalizeresource.Count > 0)
            {
                foreach (var item in objlocalizeresource)
                {
                    item.CompanyId = CompanyId;
                    LocalizeFacade.InsertLocalizeResource(item);
                }
            }
            var objpermissiongroupmap = PermissionFacade.GetAllPermissionGroupMapByCompanyId(oldcompany.CompanyId);
            if(objpermissiongroupmap != null && objpermissiongroupmap.Count > 0)
            {
                foreach(var item in objpermissiongroupmap)
                {
                    item.PermissionGroupId = PermissionGroup.Id;
                    item.CompanyId = CompanyId;
                    PermissionFacade.InsertPermissionGroupMap(item);
                }
            }
            var objemailtemp = MailFacade.GetAllTemplateByCompanyId(oldcompany.CompanyId);
            if(objemailtemp != null && objemailtemp.Count > 0)
            {
                foreach(var item in objemailtemp)
                {
                    item.CompanyId = CompanyId;
                    MailFacade.InsertEmailTemplate(item);
                }
            }
            #endregion
            FormsAuthentication.SignOut();
            SessionHelper hs = new SessionHelper();
            hs.ClearCurrentSession();
            result = true;
            if (result)
            {
                #region Create Restaurant Email
                SendEmailToCreateRestaurant SendEmailToCreateRestaurant = new SendEmailToCreateRestaurant();
                SendEmailToCreateRestaurant.ToEmail = model.Email;
                SendEmailToCreateRestaurant.Body = "Please login to your admin panel and update website information such as store hours, marketing offers and order settings.<br><br>Also, add your online ordering menu to accept orders immediately.";
                SendEmailToCreateRestaurant.Name = model.FirstName + " " + model.LastName;
                SendEmailToCreateRestaurant.Subject = model.ResturantName + " Profile Created";
                SendEmailToCreateRestaurant.CopyrightYear = DateTime.Now.Year.ToString();
                MailFacade.EmailToCreateRestaurant(SendEmailToCreateRestaurant, CompanyId);
                #endregion
                if (model.SentMail)
                {
                    #region Create Restaurant Assign Email
                    SendEmailToCreateRestaurant evanSendEmailToCreateRestaurant = new SendEmailToCreateRestaurant();
                    evanSendEmailToCreateRestaurant.ToEmail = "evan@centralstationmarketing.com";
                    evanSendEmailToCreateRestaurant.Body = "Please login to your admin panel and update website information such as store hours, marketing offers and order settings.<br><br>Also, add your online ordering menu to accept orders immediately.";
                    evanSendEmailToCreateRestaurant.Name = "Evan Islam";
                    evanSendEmailToCreateRestaurant.Subject = model.ResturantName + " Profile Created";
                    evanSendEmailToCreateRestaurant.CopyrightYear = DateTime.Now.Year.ToString();
                    MailFacade.EmailToCreateRestaurant(evanSendEmailToCreateRestaurant, CompanyId);

                    SendEmailToCreateRestaurant allenSendEmailToCreateRestaurant = new SendEmailToCreateRestaurant();
                    allenSendEmailToCreateRestaurant.ToEmail = "allen@alifsecurity.com";
                    allenSendEmailToCreateRestaurant.Body = "Please login to your admin panel and update website information such as store hours, marketing offers and order settings.<br><br>Also, add your online ordering menu to accept orders immediately.";
                    allenSendEmailToCreateRestaurant.Name = "Allen Alam";
                    allenSendEmailToCreateRestaurant.Subject = model.ResturantName + " Profile Created";
                    allenSendEmailToCreateRestaurant.CopyrightYear = DateTime.Now.Year.ToString();
                    MailFacade.EmailToCreateRestaurant(allenSendEmailToCreateRestaurant, CompanyId);
                    #endregion
                }
                var sourceHomepath = HS.Web.UI.Helper.FileHelper.GetFileFullPath("/iEatery.com/Theme/CopyTheme/Home");
                var sourceSharedpath = HS.Web.UI.Helper.FileHelper.GetFileFullPath("/iEatery.com/Theme/CopyTheme/Shared");
                var sourceWebConfigpath = HS.Web.UI.Helper.FileHelper.GetFileFullPath("/iEatery.com/Theme/CopyTheme");
                var filepath = HS.Web.UI.Helper.FileHelper.GetFileFullPath("/iEatery.com/Theme");
                Directory.CreateDirectory(filepath + "/" + WebsiteLocation.UrlSlug);
                Directory.CreateDirectory(filepath + "/" + WebsiteLocation.UrlSlug + "/Views");
                Directory.CreateDirectory(filepath + "/" + WebsiteLocation.UrlSlug + "/Views/Home");
                Directory.CreateDirectory(filepath + "/" + WebsiteLocation.UrlSlug + "/Views/Shared");
                foreach(var config in Directory.GetFiles(sourceWebConfigpath))
                {
                    System.IO.File.Copy(config, config.Replace(sourceWebConfigpath, filepath + "/" + WebsiteLocation.UrlSlug + "/Views"), true);
                }
                foreach (var home in Directory.GetFiles(sourceHomepath))
                {
                    System.IO.File.Copy(home, home.Replace(sourceHomepath, filepath + "/" + WebsiteLocation.UrlSlug + "/Views/Home"), true);
                }
                foreach (var share in Directory.GetFiles(sourceSharedpath))
                {
                    System.IO.File.Copy(share, share.Replace(sourceSharedpath, filepath + "/" + WebsiteLocation.UrlSlug + "/Views/Shared"), true);
                }
            }
            return Json(new { result = result, companyid = CompanyId, username = model.Email, password = model.Password });
        }

        [HttpPost]
        public JsonResult DeleteIeateryCompany(Guid? companyid, Guid userid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            if(companyid.HasValue && companyid.Value != new Guid())
            {
                var objwebloc = _Util.Facade.MenuFacade.GetWebsiteLocationByCompanyId(companyid.Value);
                if(objwebloc != null)
                {
                    var filepath = Server.MapPath("/iEatery.com/Theme/" + objwebloc.UrlSlug);
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                }
                result = _Util.Facade.UserOrganizationFacade.DeleteResturantMasterDB(companyid.Value);
                result = _Util.Facade.MenuFacade.DeleteResturantLiveDB(companyid.Value);
            }
            if (result)
            {
                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUsername(CurrentUser.Identity.Name);
                if(emp != null)
                {
                    var objuserpermission = _Util.Facade.UserOrganizationFacade.GetUserOrganizationByUserName(emp.UserName);
                    if (objuserpermission != null)
                    {
                        objuserpermission.IsActive = true;
                        _Util.Facade.UserOrganizationFacade.UpdateUserOrganiZation(objuserpermission);
                    }
                }
            }
            FormsAuthentication.SignOut();
            SessionHelper hs = new SessionHelper();
            hs.ClearCurrentSession();
            return Json(result);
        }

        public ActionResult ShowIeateryTermsAndConditions()
        {
            return View();
        }

        public ActionResult ShowIeateryPrivacyPolicy()
        {
            return View();
        }
    }
}