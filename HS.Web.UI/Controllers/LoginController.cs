using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.App_Start;
using HS.Web.UI.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace HS.Web.UI.Controllers
{
    public class LoginController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            //if (Session["Version"] == null)
            //    Session["Version"] = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("ATDeployedVersion");
            return View();
        }
        public ActionResult PasswordReset(int userId)
        {
            string verifysalt = "";
            string cryptmessage = DESEncryptionDecryption.DecryptCipherTextToPlainText(verifysalt);
            string[] values = cryptmessage.Split(new[] { "__" }, StringSplitOptions.None);
            string EmailAddress = ""; 
            DateTime LastUpdatedDate;
            //LastUpdatedDate = Convert.ToDateTime(values[0]);
            //if (values.Length == 4)
            //{
            //    EmailAddress = values[1];
            //    DateTime check = Convert.ToDateTime(values[2]);
            //    if (check > DateTime.Today.AddDays(1))
            //    {
            //        return RedirectToAction("Index", "Login");
            //    }
            //    LastUpdatedDate = Convert.ToDateTime(values[3]);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Login");
            //}
            //Session[SessionKeys.CompanyConnectionString] = null;
            //CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByUsername(EmailAddress);

            //if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            //{
            //    return RedirectToAction("Index", "Login");
            //}
            //else
            //{
            //    Guid CompanyId = CC.CompanyId;
            //    string ConnectionString = CC.ConnectionString;
            //    if (!string.IsNullOrWhiteSpace(ConnectionString))
            //    {
            //        ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);

            //        Session[SessionKeys.CompanyConnectionString] = ConnectionString;
            //    }
            //    else
            //    {
            //        return RedirectToAction("Index", "Login");
            //    }
            //}
            UserLogin uLogin = _Util.Facade.UserLoginFacade.GetById(userId);
            if (uLogin != null)
            {
                DateTime dt = new DateTime(uLogin.LastUpdatedDate.Value.Year
                    , uLogin.LastUpdatedDate.Value.Month
                    , uLogin.LastUpdatedDate.Value.Day
                    , uLogin.LastUpdatedDate.Value.Hour
                    , uLogin.LastUpdatedDate.Value.Minute
                    , uLogin.LastUpdatedDate.Value.Second);

                //if (dt != LastUpdatedDate)
                //{
                //    return RedirectToAction("Index", "Login");
                //}
                ViewBag.EncryptedUsersname = DESEncryptionDecryption.EncryptPlainTextToCipherText(uLogin.UserName + "__" + DateTime.Now.UTCCurrentTime().ToString());

            }
 
            if (uLogin != null)
            {
                ResetLoginUser up = new ResetLoginUser();
                //up.UserLogin.Id = uLogin.Id;
                up.UserLogin = uLogin;
                up.Employee = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(uLogin.UserId);

                return View(up);
            }
            return RedirectToAction("Index", "Login");
        }
        public ActionResult AccountVerification(int userId,string verifysalt)
        {
            if(userId>0&& !string.IsNullOrWhiteSpace(verifysalt))
            {
                Guid comid = new Guid();
                string cryptmessage = DESEncryptionDecryption.DecryptCipherTextToPlainText(verifysalt);
                string [] values = cryptmessage.Split(new[] { "__" }, StringSplitOptions.None);
                string EmailAddress="";
                if (values.Length == 4)
                {
                    EmailAddress = values[1];
                    Guid.TryParse(values[3].ToString(), out comid);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
                Session[SessionKeys.CompanyConnectionString] = null;
                
                
                CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByUsernameAndCompanyId(EmailAddress, comid);

                if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    string ConnectionString = CC.ConnectionString;
                    if (!string.IsNullOrWhiteSpace(ConnectionString))
                    {
                        ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);

                        Session[SessionKeys.CompanyConnectionString] = ConnectionString;
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
                UserLogin uLogin = _Util.Facade.UserLoginFacade.GetById(userId);
                if (uLogin != null)
                {
                    UserProfile up = new UserProfile();
                    up.UserLogin = uLogin;
                    up.Employee = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(uLogin.UserId);

                    return View(up);
                }
            }
            return RedirectToAction("Index", "Login");
        }
        //40001
        [HttpPost]
        public JsonResult AccountVerification(int Id,string LastName, string FirstName,string UserName,string Password)
        {
            UserLogin ul;
            if (Id < 1 || string.IsNullOrWhiteSpace(UserName))
            {
                return Json(new { result = false ,message= "Error UL0001 Please Contact System admin" });
            }
            ul = _Util.Facade.UserLoginFacade.GetById(Id);
            if (ul == null||ul.UserName!=UserName)
            {
                return Json(new { result = false, message = "Error UL002 Please Contact System admin" });
            }

            ul.EmailAddress = UserName;
            ul.Password = MD5Encryption.GetMD5HashData(Password);
            //ul.Id = Id;
            ul.IsDeleted = false;
            ul.IsActive = true;
            ul.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            ul.LastUpdatedBy = ul.UserName;
            bool result = _Util.Facade.UserLoginFacade.UpdateUserLogin(ul);
            if (!result)
            {
                return Json(new { result = false, message = "Error UL003 Please Contact System admin" });
            }
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(ul.UserId);
            if (emp != null)
            {
                emp.FirstName = FirstName;
                emp.LastName = LastName;
                emp.LastUpdatedBy = UserName;
                emp.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                emp.IsActive = true;
                emp.IsDeleted = false;
                result = _Util.Facade.EmployeeFacade.UpdateEmployee(emp);
            }
            else
            {
                emp = new Employee()
                {
                    UserId = ul.UserId,
                    FirstName = FirstName,
                    LastName = LastName,
                    LastUpdatedBy = UserName,
                    IsActive = true,
                    IsDeleted = false,
                    UserName = UserName,
                    Email = UserName,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    CompanyId = ul.CompanyId
                };
                result = _Util.Facade.EmployeeFacade.InsertEmployee(emp)>0;
            }


            UserOrganization userOrganization = _Util.Facade.UserOrganizationFacade.GetOrganizationByUserName(ul.UserName, ul.CompanyId);
            if (userOrganization == null)
            {
                userOrganization = new UserOrganization()
                {
                    CompanyId = ul.CompanyId,
                    UserName = ul.UserName,
                    IsActive = false
                };

                List<UserOrganization> userOrganizations = _Util.Facade.UserOrganizationFacade.GetAllUserOrganizationsByUsername(ul.UserName);
                if (userOrganizations == null || userOrganizations.Count == 0)
                {
                    userOrganization.IsActive = true;
                }
                result = _Util.Facade.UserOrganizationFacade.InsertUserOrganization(userOrganization) > 0;
            }

            #region Change Password in all companies
            List<Organization> orgList = _Util.Facade.UserOrganizationFacade.GetAllOrganizationsByUsername(UserName);
            string prevConStr = Session[SessionKeys.CompanyConnectionString].ToString();
            foreach (var item in orgList)
            {
                try
                {
                    Session[SessionKeys.CompanyConnectionString] = item.ConnectionString;
                    UserLogin Tempul = _Util.Facade.UserLoginFacade.GetUserByUsername(UserName, true);
                    if (Tempul != null)
                    {
                        if (!string.IsNullOrWhiteSpace(Tempul.Password) && Tempul.Password != ul.Password)
                        {
                            Tempul.Password = ul.Password;
                            _Util.Facade.UserLoginFacade.UpdateUserLogin(Tempul);
                        }

                    }
                }
                catch (Exception)
                {
                    Session[SessionKeys.CompanyConnectionString] = prevConStr;
                }
            }
            Session[SessionKeys.CompanyConnectionString] = prevConStr;
            #endregion

            if (!result)
            {
                return Json(new { result = false, message = "Error UL004 Please Contact System admin" });
            }

            FormsAuthentication.SetAuthCookie(ul.UserName, true); 
            return Json(new { result =true, message="User Verification Successful." });
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ForgotPassword(string EmailAddress)
        {

            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByUsername(EmailAddress);
            Session[SessionKeys.CompanyConnectionString] = null;
            if (CC == null)
            {
                return Json(new { result = false, message = "User not found" });
            }
            Guid CompanyId = CC.CompanyId;
            string ConnectionString = CC.ConnectionString;
            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString); 
                Session[SessionKeys.CompanyConnectionString] = ConnectionString;
            }else
            {
                return Json(new { result = false, message = "Internal error. Please try after sometime." });
            } 
            UserLogin ul = _Util.Facade.UserLoginFacade.GetUserByUsername(EmailAddress);
            if (ul == null)
            {
                return Json(new { result = false, message = "User not found" });
            }
            else if(ul != null && ul.IsActive == false)
            {
                return Json(new { result = false, message = "This user is inactive, please contact with company admin." });
            }
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(ul.UserId);
            if (emp == null)
            {
                return Json(new { result = false, message = "User not found" });
            }
            ul.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            _Util.Facade.UserLoginFacade.UpdateUserLogin(ul);

            string cryptmessage = DESEncryptionDecryption.EncryptPlainTextToCipherText(ul.Id + "__" + ul.UserName + "__" + DateTime.Today.AddDays(1)+"__"+ul.LastUpdatedDate);

            //UtilHelper.GetCryptMessage(ulId + email + ul2.LastUpdatedDate.ToString());

            ResetPasswordEmail verifyEmail = new ResetPasswordEmail();
            verifyEmail.Name = string.Format("{0} {1}", emp.FirstName, emp.LastName);
            string SiteURL = ConfigurationManager.AppSettings["SiteURL"];
            verifyEmail.EmailVerificationLink = AppConfig.SiteDomain + string.Format("/resetpass/{1}/{0}", ul.Id, cryptmessage);
            verifyEmail.ToEmail = ul.EmailAddress;
            var Result = _Util.Facade.MailFacade.SendEmailResetPassword(verifyEmail, CC.CompanyId);
            
            return Json(new {result=true,message= "Password reset email sent successfully! The email sent contains a password reset link which will expire after " +  DateTime.Now.UTCCurrentTime().AddDays(1).ToString("MMM, dd yyyy") + "." });
        }

        [HttpPost]
        public JsonResult ValidateCurrentPassword(int UserId, string currentpassword)
        {
            var Password = MD5Encryption.GetMD5HashData(currentpassword);
            UserLogin uLogin = _Util.Facade.UserLoginFacade.GetById(UserId);
            if (uLogin != null && uLogin.Password == Password)
            {
                return Json(new { isValid = true});
            }
            return Json(new { isValid = false, message = "Password don't match! " });
        }

        public ActionResult ResetPass(int userId, string verifysalt)
        {
            if (userId > 0 && !string.IsNullOrWhiteSpace(verifysalt))
            { 
                string cryptmessage = DESEncryptionDecryption.DecryptCipherTextToPlainText(verifysalt);
                string[] values = cryptmessage.Split(new[] { "__" }, StringSplitOptions.None);
                string EmailAddress = "";
                DateTime LastUpdatedDate;
                if (values.Length == 4)
                {
                    EmailAddress = values[1]; 
                    DateTime check = Convert.ToDateTime(values[2]);
                    if (check > DateTime.Today.AddDays(1)) {
                        return RedirectToAction("Index", "Login");
                    }
                    LastUpdatedDate = Convert.ToDateTime(values[3]);
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
                Session[SessionKeys.CompanyConnectionString] = null;
                CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByUsername(EmailAddress);

                if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    Guid CompanyId = CC.CompanyId;
                    string ConnectionString = CC.ConnectionString;
                    if (!string.IsNullOrWhiteSpace(ConnectionString))
                    {
                        ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);

                        Session[SessionKeys.CompanyConnectionString] = ConnectionString;
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
                UserLogin uLogin = _Util.Facade.UserLoginFacade.GetById(userId);
                if (uLogin != null)
                {
                    DateTime dt = new DateTime(uLogin.LastUpdatedDate.Value.Year
                        , uLogin.LastUpdatedDate.Value.Month
                        , uLogin.LastUpdatedDate.Value.Day
                        , uLogin.LastUpdatedDate.Value.Hour
                        , uLogin.LastUpdatedDate.Value.Minute
                        , uLogin.LastUpdatedDate.Value.Second);

                    if (dt != LastUpdatedDate)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    //else
                    //{
                    //    uLogin.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    //    _Util.Facade.UserLoginFacade.UpdateUserLogin(uLogin);
                    //}
                    ViewBag.EncryptedUsername = DESEncryptionDecryption.EncryptPlainTextToCipherText(uLogin.UserName+"__"+DateTime.Now.UTCCurrentTime().ToString());

                    UserProfile up = new UserProfile()
                    {
                        UserLogin =new UserLogin()
                        {
                            UserName = uLogin.UserName,
                            Password="",
                        }
                    };

                    return View(up);
                }
            }
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public JsonResult LoginUserResetPassword(string ResetPasswordToken, string password)
        {

            string cryptmessage = DESEncryptionDecryption.DecryptCipherTextToPlainText(ResetPasswordToken);
            string[] values = cryptmessage.Split(new[] { "__" }, StringSplitOptions.None);
            string Username = "";
            if (values.Length == 2)
            {
                Username = values[0];
                List<Organization> orgList = _Util.Facade.UserOrganizationFacade.GetAllOrganizationsByUsername(Username);
                string prevConStr = Session[SessionKeys.CompanyConnectionString].ToString();
                foreach (var item in orgList)
                {
                    try
                    {
                        Session[SessionKeys.CompanyConnectionString] = item.ConnectionString;
                        UserLogin ul = _Util.Facade.UserLoginFacade.GetUserByUsernameAndCompanyId(Username, item.CompanyId);
                        Employee empdata = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(ul.UserId);
                        if (ul == null)
                        {
                            return Json(new
                            {
                                result = false,
                                message = "User not found."
                            });
                        }
                        if (ul != null)
                        {
                            ul.Password = MD5Encryption.GetMD5HashData(password);
                            ul.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            ul.IsActive = true;
                            ul.PasswordResetDate = DateTime.Now.AddDays((int)empdata.PasswordUpdateDays);
                            _Util.Facade.UserLoginFacade.UpdateUserLogin(ul);
                        }
                    }
                    catch (Exception)
                    {
                        Session[SessionKeys.CompanyConnectionString] = prevConStr;
                    }
                }
                return Json(new
                {
                    result = true,
                    message = "Password reset successful."
                });
            }
            return Json(new
            {
                result = false,
                message = "An error occured."
            });
        }

        [HttpPost]
        public JsonResult ResetPass(string ResetPasswordToken, string password)
        {

            string cryptmessage = DESEncryptionDecryption.DecryptCipherTextToPlainText(ResetPasswordToken);
            string[] values = cryptmessage.Split(new[] { "__" }, StringSplitOptions.None);
            string Username = "";
            if (values.Length == 2)
            {
                Username = values[0];
                List<Organization> orgList = _Util.Facade.UserOrganizationFacade.GetAllOrganizationsByUsername(Username);
                string prevConStr = Session[SessionKeys.CompanyConnectionString].ToString();
                foreach (var item in orgList)
                {
                    try
                    {
                        Session[SessionKeys.CompanyConnectionString] = item.ConnectionString;
                        UserLogin ul = _Util.Facade.UserLoginFacade.GetUserByUsernameAndCompanyId(Username, item.CompanyId);
                        if (ul == null)
                        {
                            return Json(new
                            {
                                result = false,
                                message = "User not found."
                            });
                        }
                        if (ul != null)
                        {
                            ul.Password = MD5Encryption.GetMD5HashData(password);
                            ul.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            ul.IsActive = true;
                            _Util.Facade.UserLoginFacade.UpdateUserLogin(ul);
                        }
                    }
                    catch (Exception)
                    {
                        Session[SessionKeys.CompanyConnectionString] = prevConStr;
                    }
                }
                return Json(new
                {
                    result = true,
                    message = "Password reset successful."
                });
            }
            return Json(new {
                result =false,
                message ="An error occured."
            } );
        }

        [HttpPost]
        public JsonResult LoginAjax(string UserName, string Password, bool Remember, string Currentdate, string Currenttime, string Currentzone, string currenttimezone, string companyid)
        {
            Session[SessionKeys.CompanyConnectionString] = null;
            Guid CompanyId = new Guid();
            if (Session["PrefferedCompanyId"] != null)
            {
                Guid PrefferedCompanyId = (Guid)Session["PrefferedCompanyId"];
                _Util.Facade.UserOrganizationFacade.SetDefaultUserCompany(UserName, PrefferedCompanyId);
                Session["PrefferedCompanyId"] = null;
            }
            CompanyConneciton CC = new CompanyConneciton();
            if (!string.IsNullOrWhiteSpace(companyid))
            {
                CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByUsernameAndCompanyId(UserName, new Guid(companyid));
            }
            else
            {
                CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByUsername(UserName);
            }
            //List<UserOrganization> UO = _Util.Facade.UserOrganizationFacade.UserOrganizationByUsername(UserName);
            if (CC==null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                return Json(new { data= "none" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                CompanyId = CC.CompanyId;
                string ConnectionString = CC.ConnectionString;
                if (!string.IsNullOrWhiteSpace(ConnectionString))
                {
                    //ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);
                    Session[SessionKeys.CompanyConnectionString] = ConnectionString;
                }
                else
                {
                    return Json(new { data = "none" }, JsonRequestBehavior.AllowGet);
                }
            }

            Employee empdata = _Util.Facade.EmployeeFacade.GetEmployeeByUserName(UserName);
            UserLogin uLogin = _Util.Facade.UserLoginFacade.GetUserLoginByUserId(empdata.UserId);
            if (empdata.PasswordUpdateDays > 0)
            {
                if (uLogin.PasswordResetDate.HasValue)
                {
                    DateTime currentDate = DateTime.Now;
                    DateTime resetDate = uLogin.PasswordResetDate.Value.Date;
                    if (resetDate < currentDate.Date)
                    {
                        return Json(new { msg = "reset", updatedays = empdata.PasswordUpdateDays, UserId = uLogin.Id }, JsonRequestBehavior.AllowGet);
                    }
                }
            } 
            Password = MD5Encryption.GetMD5HashData(Password);
            UserLogin obUser = _Util.Facade.UserLoginFacade.GetUserType(UserName, Password, CC.MasterPassword ,Remember, CC.CompanyId);
            
            if (obUser !=null && (obUser.UserType != "none" ))
            {
                obUser.DefaultCompanyId = CC.CompanyId;
                Session[SessionKeys.CompanyId] = CC.CompanyId;
                FormsAuthentication.SetAuthCookie(obUser.UserName, Remember); 
                CustomPrincipal UserPrincipal = new CustomPrincipal(obUser, User.Identity);
                HttpContext.User = UserPrincipal;
                _Util.Facade.PermissionFacade.GetAllUserPermissions(obUser.UserId, CompanyId);
                UserActivity UserActivity = new UserActivity()
                {
                    Action = "LogIn",
                    ActionDisplyText = "LogInSuccess",
                    UserIp = AppConfig.GetIP,
                    UserAgent = AppConfig.GetUserAgent,
                    UserName = obUser.UserName,
                    StatsDate = DateTime.Now.UTCCurrentTime(),
                    UserId = obUser.UserId
                };
                _Util.Facade.UserActivityFacade.InsertUserActivity(UserActivity);
                if (Request.Url.Host.ToLower().IndexOf("app.ieatery.com") > -1)
                {
                    var GlobalSettings = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CC.CompanyId, "IsDefultDate");
                    if (GlobalSettings != null)
                    {
                        GlobalSettings.Value = "true";
                        _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(GlobalSettings);
                    }
                }
            }
            if (obUser.UserType == "none")
            {
                Session[SessionKeys.CompanyConnectionString] = null;
            }
            //SetCompanyOnSession();
            //HttpCookie CurrentUserTimeZone = HttpContext.Request.Cookies.Get(CookieKeys.CurrentUser);

            HttpCookie CurrentUserTimeZone = new HttpCookie(CookieKeys.CurrentUser);
            CurrentUserTimeZone[CookieKeys.CurrentUserTimeZone] = Currentzone;
            CurrentUserTimeZone[CookieKeys.CurrentUserDate] = Currentdate;
            CurrentUserTimeZone[CookieKeys.CurrentUserTime] = Currenttime;
            CurrentUserTimeZone[CookieKeys.CurrentUserTimeZoneName] = currenttimezone;
            //CurrentUserTimeZone.Value = CurrentUserTimeZone;
            CurrentUserTimeZone.Expires = DateTime.Now.UTCCurrentTime().AddDays(2d);
            Response.Cookies.Add(CurrentUserTimeZone);
            return Json(new { data = obUser.UserType, UserId = obUser.UserId }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            base.AddPageLoadUserActivity("Logout " + LabelHelper.ActivityAction.Success);
            SessionHelper hs = new SessionHelper();
            hs.ClearCurrentSession();
           
            return Redirect("/login");
        }

        //public JsonResult Encryption(string Message , string passsword)
        //{
        //    string EncryptedMessage = DESEncryptionDecryption.EncryptPlainTextToCipherText(Message);

        //    return Json(new { message = EncryptedMessage },JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult Decryption(string Message, string passsword)
        //{
        //    Message = "XV+9FrQFLfoHJhls9jt5FMeQ+OK9jpJD78RNMMSXjKRYYnKgzrN2YQaptwEbYxFwgF73LaVGqFLQ2zjVZeTDHNnc4adLGB9sH5vJnFl4t74GEmVRzXOcH40wxh/IaCQM+CKeFpSDdrE=";
        //    string EncryptedMessage =  DESEncryptionDecryption.DecryptCipherTextToPlainText(Message);

        //    return Json(new { message = EncryptedMessage }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult CreateRestaurantGreetingsPartial(string code)
        {
            LoginCustomModel model = new LoginCustomModel();
            if (!string.IsNullOrWhiteSpace(code))
            {
                var spcode = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('~');
                model.UserName = spcode[0];
                model.Password = spcode[1];
            }
            return View(model);
        }
    }
}