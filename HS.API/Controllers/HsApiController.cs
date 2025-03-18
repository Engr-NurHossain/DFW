using HS.API.Models;
using HS.Entities;
using HS.Facade;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.Controllers;
using HS.Web.UI.Helper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Routing;
using System.Text.RegularExpressions;
using Plivo;
using Plivo.Exception;
using System.Web.Script.Serialization;
using System.Text;
using Rotativa;
using System.Xml;
using HS.Entities.Bases;
using Microsoft.Ajax.Utilities;
using Plivo.XML;
using System.Web.Helpers;
using Plivo.Resource.Identity;
using System.Threading.Tasks;
using Lookup = HS.Entities.Lookup;
using System.Globalization;
using OS.AWS.S3.Services;
using System.Drawing.Printing;
using DinkToPdf;
using DinkToPdf.Contracts;
using ColorMode = DinkToPdf.ColorMode;
using PaperKind = DinkToPdf.PaperKind;
using ObjectSettings = DinkToPdf.ObjectSettings;
using HS.Entities.Custom;
using HS.Entities.List;
using RestSharp;
using static HS.Entities.API.ThirdParty;
using Plivo.Resource.Node;
using Amazon.Runtime.Internal.Util;
using Antlr.Runtime.Misc;
using HS.Alarm.DealerManager;
using RestSharp.Extensions;
using HS.Alarm.CustomerManager;
using HS.Alarm.AlarmCustomer;
using Microsoft.ApplicationInsights.Extensibility.Implementation;



namespace HS.API.Controllers
{
    [RoutePrefix("api")]
    public class HsApiController : BaseAPIController
    {
        #region Initialize
        public static string ComId = "";

        private static readonly SynchronizedConverter Converter = new SynchronizedConverter(new PdfTools());

        public bool IsEstimate { get; private set; }
        public object CustomerId { get; private set; }
        public int CustomerName { get; private set; }
        public object CompanyId { get; private set; }
        public object EstimateDetails { get; private set; }
        public object LastUpdatedByUid { get; private set; }
        public object CreatedByUid { get; private set; }
        public DateTime LastUpdatedDate { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime InvoiceDate { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public string Status { get; private set; }
        public int Amount { get; private set; }
        //private void APIInitialize()
        //{
        //    if (!string.IsNullOrWhiteSpace(ConnStr))
        //    {
        //        if (!string.IsNullOrWhiteSpace(ComId))
        //        {
        //            CompanyConneciton CC = new CompanyConneciton();
        //            var identity = (ClaimsIdentity)User.Identity;
        //            if(!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
        //            {
        //                CC = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(ComId));
        //            }
        //            else
        //            {
        //                CC = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(UserNAME, new Guid(ComId));
        //            }
        //            ComId = CC.CompanyId.ToString();
        //            HSapiFacade = new HSApiFacade(CC.ConnectionString);
        //        }
        //        else
        //        {
        //            var identity = (ClaimsIdentity)User.Identity;
        //            CompanyConneciton CC = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
        //            ComId = CC.CompanyId.ToString();
        //            HSapiFacade = new HSApiFacade(CC.ConnectionString);
        //        }
        //    }
        //    else
        //    {
        //        var identity = (ClaimsIdentity)User.Identity;
        //        string connectionstring = identity.Claims.Where(c => c.Type == "connectionstring").Select(c => c.Value).SingleOrDefault();
        //        CompanyConneciton CC = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
        //        ComId = CC.CompanyId.ToString();
        //        HSapiFacade = new HSApiFacade(CC.ConnectionString);
        //    }
        //}
        private void ForgotPasswordAPIInitialize(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                CompanyConneciton CC = HSMainApiFacade.GetCompanyConnectionByUserName(username);
                ComId = CC.CompanyId.ToString();
                HSapiFacade = new HSApiFacade(CC.ConnectionString);
            }
        }

        #endregion


        #region Customer

        [Authorize]
        [Route("SaveCustomer")]
        public HttpResponseMessage CreateCustomer()
        {
            APIInitialize();

            #region Input Params
            int Id = 0;
            string FirstName = "";
            string LastName = "";
            string BusinessName = "";
            string DBA = "";
            string SSN = "";


            string PrimaryPhone = "";
            string SecondaryPhone = "";
            string CellNo = "";
            string Email = "";

            string Street = "";
            string City = "";
            string State = "";
            string ZipCode = "";
            string County = "";

            string Type = "";
            string Notes = "";
            string LeadSource = "";//LeadSource
            string CSAccountNumber = "";
            string AbortCode = "";
            string AlternateId = "";
            string CSProvider = "";
            string Ownership = "";//Ownership
            string SalesLocation = "";//SalesLocation
            string LeadStatus = ""; //LeadStatus  //// Somehow it's not working,need to check
            string CustomerStatus = ""; //CustomerStatus
            string Apartment = "";
            string AccountNo = "";

            DateTime DOB = new DateTime();
            DateTime MovingDate = new DateTime();
            DateTime FollowUpDate = new DateTime();

            Guid SoldBy = Guid.Empty;
            string username = "";

            bool IsLead = true;
            Guid CustomerId = Guid.Empty;
            #endregion

            var re = Request;
            var headers = re.Headers;
            bool result = false;

            #region Retrive Data From Headers
            if (headers.Contains("IsLead"))
            {
                string IsLeadValue = headers.GetValues("IsLead").First();

                if (!bool.TryParse(IsLeadValue, out IsLead))
                {
                    if (!string.IsNullOrWhiteSpace(IsLeadValue) && (IsLeadValue.ToLower().Trim() == "false" || IsLeadValue.ToLower().Trim() == "0"))
                    {
                        IsLead = false;
                    }
                }
            }
            if (headers.Contains("SoldBy"))
            {
                username = headers.GetValues("SoldBy").First();
                int UserLoginId = 0;
                if (int.TryParse(username, out UserLoginId) && UserLoginId > 0)
                {
                    UserLogin ul = HSapiFacade.GetUserLoginById(UserLoginId);
                    if (ul != null)
                    {
                        SoldBy = ul.UserId;
                    }
                }
                else if (SoldBy == Guid.Empty && !string.IsNullOrWhiteSpace(username))
                {
                    UserLogin ul = HSapiFacade.GetUserLoginByUsername(username);
                    if (ul != null)
                    {
                        SoldBy = ul.UserId;
                    }
                    else if (username.IsValidEmailAddress())
                    {
                        Employee employee = HSapiFacade.GetEmployeeByEmailAddress(username);
                        if (employee != null)
                        {
                            SoldBy = employee.UserId;
                        }
                    }
                }
            }
            if (headers.Contains("FirstName"))
            {
                FirstName = headers.GetValues("FirstName").First();
            }
            if (headers.Contains("LastName"))
            {
                LastName = headers.GetValues("LastName").First();
            }
            if (headers.Contains("BusinessName"))
            {
                BusinessName = headers.GetValues("BusinessName").First();
            }
            if (headers.Contains("DBA"))
            {
                DBA = headers.GetValues("DBA").First();
            }
            if (headers.Contains("SSN"))
            {
                SSN = headers.GetValues("SSN").First();
            }

            if (headers.Contains("Type"))
            {
                Type = headers.GetValues("Type").First();
            }
            if (headers.Contains("PrimaryPhone"))
            {
                PrimaryPhone = headers.GetValues("PrimaryPhone").First();
                if (!string.IsNullOrWhiteSpace(PrimaryPhone))
                {
                    PrimaryPhone = PrimaryPhone.PhoneNumberFormat();
                }
            }
            if (headers.Contains("SecondaryPhone"))
            {
                SecondaryPhone = headers.GetValues("SecondaryPhone").First();
                if (!string.IsNullOrWhiteSpace(SecondaryPhone))
                {
                    SecondaryPhone = SecondaryPhone.PhoneNumberFormat();
                }
            }
            if (headers.Contains("CellNo"))
            {
                CellNo = headers.GetValues("CellNo").First();
                if (!string.IsNullOrWhiteSpace(CellNo))
                {
                    CellNo = CellNo.PhoneNumberFormat();
                }
            }
            if (headers.Contains("Email"))
            {
                Email = headers.GetValues("Email").First();
            }
            if (headers.Contains("Street"))
            {
                Street = headers.GetValues("Street").First();
            }
            if (headers.Contains("City"))
            {
                City = headers.GetValues("City").First();
            }
            if (headers.Contains("State"))
            {
                State = headers.GetValues("State").First();
            }
            if (headers.Contains("ZipCode"))
            {
                ZipCode = headers.GetValues("ZipCode").First();
            }
            if (headers.Contains("County"))
            {
                County = headers.GetValues("County").First();
            }
            if (headers.Contains("CSAccountNumber"))
            {
                CSAccountNumber = headers.GetValues("CSAccountNumber").First();
            }
            if (headers.Contains("AbortCode"))
            {
                AbortCode = headers.GetValues("AbortCode").First();
            }
            if (headers.Contains("AlternateId"))
            {
                AlternateId = headers.GetValues("AlternateId").First();
            }
            if (headers.Contains("CSProvider"))
            {
                CSProvider = headers.GetValues("CSProvider").First();
            }
            if (headers.Contains("Ownership"))
            {
                Ownership = headers.GetValues("Ownership").First();
            }
            if (headers.Contains("SalesLocation"))
            {
                SalesLocation = headers.GetValues("SalesLocation").First();
            }
            if (headers.Contains("LeadStatus"))
            {
                LeadStatus = headers.GetValues("LeadStatus").First();
            }
            if (headers.Contains("CustomerStatus"))
            {
                CustomerStatus = headers.GetValues("CustomerStatus").First();
            }

            if (headers.Contains("CustomerId"))
            {
                string CustomerIdSt = headers.GetValues("CustomerId").First();
                Guid.TryParse(CustomerIdSt, out CustomerId);
            }
            if (headers.Contains("Id"))
            {
                string CustomerIdSt = headers.GetValues("Id").First();
                Int32.TryParse(CustomerIdSt, out Id);
            }
            if (headers.Contains("Notes"))
            {
                Notes = headers.GetValues("Notes").First();
            }
            if (headers.Contains("LeadSource"))
            {
                LeadSource = headers.GetValues("LeadSource").First();
            }
            if (headers.Contains("Apartment"))
            {
                Apartment = headers.GetValues("Apartment").First();
            }
            if (string.IsNullOrWhiteSpace(LeadSource))
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "'LeadSource' Is Required. Goto /GetLookupbyKey with key='LeadSource' for LeadSource list."));
            }
            if (headers.Contains("DOB"))
            {
                string DOBStr = headers.GetValues("DOB").First();
                DOB = DOBStr.ToDateTime();
            }
            if (headers.Contains("MovingDate"))
            {
                string MovingDateStr = headers.GetValues("MovingDate").First();
                MovingDate = MovingDateStr.ToDateTime();
            }
            if (headers.Contains("FollowUpDate"))
            {
                string FollowUpDateStr = headers.GetValues("FollowUpDate").First();
                FollowUpDate = FollowUpDateStr.ToDateTime();
            }
            if (headers.Contains("AccountNo"))
            {
                AccountNo = headers.GetValues("AccountNo").First();
            }
            #endregion 
            string message = "";
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string Username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));

                    #region LOG

                    try
                    {
                        #region Create Directory if not exists
                        string subPath = "~/APILOG"; // your code goes here 
                        bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
                        if (!exists)
                            System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
                        #endregion

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\APILOG\SaveCustomer.txt"), true))
                        {
                            string LOGFile = @"IP ADDRESS: " + base.GetClientIp(Request) + " Username: " + identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault() + " Date: " + DateTime.Now.ToString("MM/dd/yy hh:mm:ss tt") + " Data: ";
                            LOGFile += " Id = " + Id + " FirstName  = " + FirstName + " LastName = " + LastName + " BusinessName = " + BusinessName + " Type = " + Type + " PrimaryPhone =" + PrimaryPhone;
                            LOGFile += " SecondaryPhone = " + SecondaryPhone + " CellNo = " + CellNo + " Email = " + Email + " Street = " + Street;
                            LOGFile += " City = " + City + " State = " + State + " ZipCode = " + ZipCode + " Notes = " + Notes + " LeadSource=" + LeadSource;
                            LOGFile += " DBA = " + DBA + " SSN = " + SSN; LOGFile += " County = " + County;
                            LOGFile += " CSProvider = " + CSProvider + " CustomerNo = " + CSAccountNumber;
                            LOGFile += " SecondCustomerNo =" + AlternateId + " Ownership =" + Ownership;
                            LOGFile += " SalesLocation =" + SalesLocation + " CustomerStatus =" + CustomerStatus;
                            LOGFile += " Status =" + LeadStatus + " Passcode =" + AbortCode;
                            LOGFile += " DateofBirth = " + DOB + " FollowUpDate =" + FollowUpDate;
                            LOGFile += " MovingDate =" + MovingDate + " Soldby =" + SoldBy.ToString() + " Soldby Username: " + username;

                            file.WriteLine(LOGFile);
                            file.Close();
                        }
                    }
                    catch (Exception)
                    {

                    }
                    #endregion

                    if (string.IsNullOrWhiteSpace(LeadSource))
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "'LeadSource' Is Required. Goto /GetLookupbyKey with key='LeadSource' for LeadSource list."));
                    }
                    if (string.IsNullOrWhiteSpace(PrimaryPhone) && string.IsNullOrWhiteSpace(SecondaryPhone) && string.IsNullOrWhiteSpace(CellNo) && string.IsNullOrWhiteSpace(Email))
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "At least one contact method is required."));
                    }

                    var CustomerDetails = new Customer();

                    Employee employee = HSapiFacade.GetEmployeeByUsername(Username);
                    if (employee == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                    }

                    #region SoldBy
                    if (SoldBy == Guid.Empty)
                    {
                        SoldBy = employee.UserId;
                    }
                    #endregion

                    if (Id > 0)
                    {
                        CustomerDetails = HSapiFacade.GetCustomerById(Id);
                    }
                    if (Id == 0 || CustomerDetails == null)
                    {
                        CustomerDetails = HSapiFacade.GetCustomerByPhoneNumberAndEmail(PrimaryPhone, SecondaryPhone, CellNo, Email);
                        if (CustomerDetails != null)
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Customer with same contact number and email address is already exists."));
                        }

                        #region InsertCustomer
                        CustomerDetails = new Customer()
                        {
                            CustomerId = Guid.NewGuid(),
                            FirstName = FirstName,
                            LastName = LastName,
                            BusinessName = BusinessName,
                            Type = Type,
                            PrimaryPhone = PrimaryPhone,
                            SecondaryPhone = SecondaryPhone,
                            CellNo = CellNo,
                            EmailAddress = Email,
                            Street = Street,
                            City = City,
                            State = State,
                            ZipCode = ZipCode,
                            CreatedByUid = employee.UserId,
                            LastUpdatedBy = employee.UserName,
                            LastUpdatedByUid = employee.UserId,
                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            JoinDate = DateTime.Now.UTCCurrentTime(),
                            IsActive = true,
                            PhoneType = "",
                            Carrier = "",
                            ReferringCustomer = Guid.Empty,
                            ChildOf = Guid.Empty,
                            Note = Notes,
                            LeadSource = LeadSource,

                            DBA = DBA,
                            SSN = SSN,
                            County = County,
                            CSProvider = CSProvider,
                            CustomerNo = CSAccountNumber,
                            SecondCustomerNo = AlternateId,
                            //AdditionalCustomerNo = AlternateId,
                            Ownership = Ownership,
                            SalesLocation = SalesLocation,
                            CustomerStatus = CustomerStatus,
                            Status = LeadStatus,
                            Passcode = AbortCode,
                            DateofBirth = DOB,
                            FollowUpDate = FollowUpDate,
                            MovingDate = MovingDate,
                            Soldby = SoldBy.ToString(),
                            Soldby1 = SoldBy,
                            Appartment = Apartment,
                            AccountNo = AccountNo
                        };
                        Id = HSapiFacade.InsertCustomers(CustomerDetails);
                        CustomerId = CustomerDetails.CustomerId;
                        #endregion

                        CustomerCompany cusCompany = new CustomerCompany()
                        {
                            CustomerId = CustomerDetails.CustomerId,
                            CompanyId = usercontext.CompanyId,
                            IsLead = IsLead,
                            IsActive = true
                        };
                        result = HSapiFacade.InsertCustomerCompany(cusCompany);

                        CustomerSnapshot ObjCustomerSnapShot = new CustomerSnapshot
                        {
                            CustomerId = CustomerDetails.CustomerId,
                            CompanyId = usercontext.CompanyId,
                            Description = Notes,
                            Logdate = DateTime.Now.UTCCurrentTime(),
                            Updatedby = employee.UserName,
                            Type = Type
                        };
                        result = HSapiFacade.InsertSnapshot(ObjCustomerSnapShot);
                        message = "Customer added successfully.";
                        if (IsLead)
                        {
                            message = "Lead added successfully.";
                        }

                    }
                    else
                    {
                        #region Update Customer
                        CustomerDetails.FirstName = FirstName;
                        CustomerDetails.LastName = LastName;
                        CustomerDetails.BusinessName = BusinessName;
                        CustomerDetails.Type = Type;
                        CustomerDetails.PrimaryPhone = PrimaryPhone;
                        CustomerDetails.SecondaryPhone = SecondaryPhone;
                        CustomerDetails.CellNo = CellNo;
                        CustomerDetails.EmailAddress = Email;
                        CustomerDetails.Street = Street;
                        CustomerDetails.City = City;
                        CustomerDetails.State = State;
                        CustomerDetails.ZipCode = ZipCode;
                        CustomerDetails.Note = Notes;
                        CustomerDetails.DBA = DBA;
                        CustomerDetails.SSN = SSN;
                        CustomerDetails.County = County;
                        CustomerDetails.CSProvider = CSProvider;
                        CustomerDetails.CustomerNo = CSAccountNumber;
                        CustomerDetails.SecondCustomerNo = AlternateId;
                        CustomerDetails.Ownership = Ownership;
                        CustomerDetails.SalesLocation = SalesLocation;
                        CustomerDetails.CustomerStatus = CustomerStatus;
                        CustomerDetails.Status = LeadStatus;
                        CustomerDetails.Passcode = AbortCode;
                        CustomerDetails.DateofBirth = DOB;
                        CustomerDetails.FollowUpDate = FollowUpDate;
                        CustomerDetails.MovingDate = MovingDate;
                        CustomerDetails.Soldby = SoldBy.ToString();
                        CustomerDetails.AccountNo = AccountNo;
                        if (!string.IsNullOrWhiteSpace(LeadSource))
                        {
                            CustomerDetails.LeadSource = LeadSource;
                        }

                        result = HSapiFacade.UpdateCustomer(CustomerDetails);

                        //var cus = HSapiFacade.GetCustomerByCustomerId(CustomerId);
                        CustomerSnapshot ObjCustomerSnapShot = new CustomerSnapshot
                        {
                            CustomerId = CustomerDetails.CustomerId,
                            CompanyId = usercontext.CompanyId,
                            Description = Notes,
                            Logdate = DateTime.Now.UTCCurrentTime(),
                            Updatedby = employee.UserName,
                            Type = Type
                        };
                        result = HSapiFacade.InsertSnapshot(ObjCustomerSnapShot);

                        CustomerCompany customerCompany = HSapiFacade.GetCustomerCompanyByCustomerId(CustomerDetails.CustomerId, usercontext.CompanyId);
                        if (customerCompany != null)
                        {
                            customerCompany.IsLead = IsLead;
                            if (IsLead)
                                customerCompany.ConvertionDate = DateTime.Now.UTCCurrentTime();
                            HSapiFacade.UpdateCustomerCompany(customerCompany);
                        }
                        else
                        {
                            CustomerCompany cusCompany = new CustomerCompany()
                            {
                                CustomerId = CustomerDetails.CustomerId,
                                CompanyId = usercontext.CompanyId,
                                IsLead = IsLead,
                                IsActive = true
                            };
                            result = HSapiFacade.InsertCustomerCompany(cusCompany);
                        }

                        message += "Customer updated successfully.";
                        if (IsLead)
                        {
                            message += "Lead updated successfully.";
                        }
                        #endregion
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new BaseAPIResponse(true, message, new { customerId = Id, customerGuid = CustomerDetails.CustomerId }));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Token Expired.", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, ex.Message, null));
            }
        }
        [Authorize]
        [Route("GetCustomerByIdWithEstimateList")]
        public HttpResponseMessage GetCustomerId()
        {
            APIInitialize();
            bool Inspection = false;
            int PageNo = 1;
            int PageSize = 10;
            int CustomerIntId = 0;

            var re = Request;
            var headers = re.Headers;

            InvoiceFilter filter = new InvoiceFilter();
            if (headers.Contains("PageNo"))
            {
                int.TryParse(headers.GetValues("PageNo").First(), out PageNo);
                filter.PageNo = PageNo;
            }
            if (headers.Contains("PageSize"))
            {
                int.TryParse(headers.GetValues("PageSize").First(), out PageSize);
                filter.PageSize = PageSize;
            }
            if (headers.Contains("CustomerIntId"))
            {
                int.TryParse(headers.GetValues("CustomerIntId").First(), out CustomerIntId);
                filter.CustomerIntId = CustomerIntId;
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    Customer customer = HSapiFacade.GetCustomerById(CustomerIntId);
                    if (customer == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Customer Not Found.");

                    var CustomerDetails = HSapiFacade.GetCustomerById(filter.CustomerIntId);
                    var EstimateList = HSapiFacade.GetEstimateListByFilter(filter);
                    var CustomerInspection = HSapiFacade.GetCustomerInspectionByCustomerId(CustomerDetails.CustomerId);
                    foreach (var item in EstimateList.EstimateList)
                    {
                        string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(item.InvoiceId + "#" + identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                        item.PDFUrl = ConfigurationManager.AppSettings["SiteURL"] + "/Estimate/GetPdf/?code=" + encryptedurl;
                    }

                    if (CustomerInspection.Count > 0)
                    {
                        Inspection = true;
                        return Request.CreateResponse(HttpStatusCode.OK, new { CustomerDetails, EstimateList, Inspection });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { CustomerDetails, EstimateList, Inspection });
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Authorize]
        [Route("customer")]
        [HttpGet]
        public HttpResponseMessage Customer()
        {
            APIInitialize();
            Guid cusId = Guid.Empty;

            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("customerId"))
            {
                Guid.TryParse(headers.GetValues("customerId").First(), out cusId);
            }

            try
            {
                var Userinfo = base.GetUserInfo();
                if (Userinfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<string>.Error("Token Expired."));
                }

                CompanyConneciton usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(Userinfo.Username);
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<string>.Error("Authorization Denied."));

                Customer customer = HSapiFacade.GetCustomerById(cusId);
                if (customer == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<string>.Error("Customer Not Found."));

                #region Partner Data
                PermissionGroup objRoleEmployee = HSapiFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(Userinfo.Userid, usercontext.CompanyId);

                if (objRoleEmployee == null)
                {
                    var objemp = HSapiFacade.GetEmployeeByUserName(Userinfo.Username);
                    if (objemp == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    objRoleEmployee = HSapiFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(objemp.UserId, usercontext.CompanyId);
                }

                if (objRoleEmployee.Tag.ToLower().IndexOf("partner") > -1)
                {
                    bool SendResult = false;
                    List<Partner> Partners = HSapiFacade.GetEmployeeByPartnerId(Userinfo.Userid);
                    if (Partners.Count > 0)
                    {
                        foreach (var item in Partners)
                        {
                            if (customer.CreatedByUid == item.UserId || customer.QA1 == item.UserId.ToString() ||
                                customer.Soldby == item.UserId.ToString() || customer.QA2 == item.UserId.ToString())
                            {
                                //If any partner related to this customer only then that partner can see this customer data.
                                SendResult = true;
                                break;
                            }
                        }
                        if (!SendResult)
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Access denied."));
                        }
                    }
                }
                #endregion

                var response = new CustomerResponse
                {
                    id = customer.Id,
                    customerGuid = customer.CustomerId,
                    name = $"{customer.FirstName} {customer.LastName}",
                    business = customer.BusinessName,
                    phone = customer.CellNo,
                    email = customer.EmailAddress,
                    street = customer.Street,
                    city = customer.City,
                    state = customer.State,
                    zip = customer.ZipCode,
                    type = customer.Type,
                    county = customer.County,
                    verbalPassword = customer.Passcode,
                    profilePicture = customer.ProfileImage
                };
                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<CustomerResponse>.Success(response));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ApiResponse<string>.Error(ex.Message));
            }
        }


        [Authorize]
        [Route("GetCustomerByCustomerId")]
        public HttpResponseMessage GetCustomerCustomerId()
        {
            APIInitialize();
            //bool Inspection = false;
            int CustomerIntId = 0;

            var re = Request;
            var headers = re.Headers;

            InvoiceFilter filter = new InvoiceFilter();

            if (headers.Contains("CustomerIntId"))
            {
                int.TryParse(headers.GetValues("CustomerIntId").First(), out CustomerIntId);
                filter.CustomerIntId = CustomerIntId;
            }
            try
            {
                var Userinfo = base.GetUserInfo();
                if (Userinfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Token Expired.", null));
                }

                CompanyConneciton usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(Userinfo.Username);
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");

                var CustomerDetails = HSapiFacade.GetCustomerById(filter.CustomerIntId);
                if (CustomerDetails == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Customer Not Found."));

                //var CustomerInspection = HSapiFacade.GetCustomerInspectionByCustomerId(CustomerDetails.CustomerId);
                //if (CustomerInspection.Count > 0)
                //{
                //    Inspection = true;
                //}
                #region Partner Data
                //Partners can't see others data.
                PermissionGroup objRoleEmployee = HSapiFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(Userinfo.Userid, usercontext.CompanyId);
                //objRoleEmployee.Tag;
                if (objRoleEmployee == null)
                {
                    var objemp = HSapiFacade.GetEmployeeByUserName(Userinfo.Username);
                    if (objemp == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    objRoleEmployee = HSapiFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(objemp.UserId, usercontext.CompanyId);
                }
                if (objRoleEmployee.Tag.ToLower().IndexOf("partner") > -1)
                {
                    bool SendResult = false;
                    List<Partner> Partners = HSapiFacade.GetEmployeeByPartnerId(Userinfo.Userid);
                    if (Partners.Count > 0)
                    {
                        foreach (var item in Partners)
                        {
                            if (CustomerDetails.CreatedByUid == item.UserId || CustomerDetails.QA1 == item.UserId.ToString() ||
                                CustomerDetails.Soldby == item.UserId.ToString() || CustomerDetails.QA2 == item.UserId.ToString())
                            {
                                //If any partner related to this customer only then that partner can see this customer data.
                                SendResult = true;
                                break;
                            }
                        }
                        if (!SendResult)
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Access denied."));
                        }
                    }
                }
                #endregion
                return Request.CreateResponse(HttpStatusCode.OK, new BaseAPIResponse(true, "Successful.", CustomerDetails));

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, ex.Message));
            }
        }

        [Route("create-service")]
        [HttpPost]
        public HttpResponseMessage SaveTicketBookingServiceItems()
        {
            Guid ticketid = new Guid();
            Guid userid = new Guid();
            Guid sb = new Guid();
            Guid serviceId = Guid.Empty;
            int itemid = 0;

            string serviceName = string.Empty;
            int quantity = 0;
            int installed = 0;
            double unit = 0;
            double price = 0;
            string PageUrl = "";
            string Action = "";
            string ActionDisplyText = "";
            string UserIp = "";
            string UserAgent = "";
            string UserName = "";
            string UserId = "";
            string CustomerId = "";


            var re = Request;
            var headers = re.Headers;
            #region Log Header
            if (headers.Contains("pageurl"))
            {
                PageUrl = headers.GetValues("pageurl").First();
            }
            if (headers.Contains("action"))
            {
                Action = headers.GetValues("action").First();
            }
            if (headers.Contains("actiondisplytext"))
            {
                ActionDisplyText = headers.GetValues("actiondisplytext").First();
            }
            if (headers.Contains("userip"))
            {
                UserIp = headers.GetValues("userip").First();
            }
            if (headers.Contains("useragent"))
            {
                UserAgent = headers.GetValues("useragent").First();
            }
            if (headers.Contains("username"))
            {
                UserName = headers.GetValues("username").First();
            }
            if (headers.Contains("userid"))
            {
                UserId = headers.GetValues("userid").First();
            }
            if (headers.Contains("customerid"))
            {
                CustomerId = headers.GetValues("customerid").First();
            }
            #endregion
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            if (headers.Contains("userid"))
            {
                Guid.TryParse(headers.GetValues("userid").First(), out userid);
            }
            if (headers.Contains("itemid"))
            {
                int.TryParse(headers.GetValues("itemid").First(), out itemid);
            }


            if (headers.Contains("serviceId"))
            {
                Guid.TryParse(headers.GetValues("serviceId").First(), out serviceId);
            }
            if (headers.Contains("serviceName"))
            {
                serviceName = headers.GetValues("serviceName").First();
            }
            if (headers.Contains("quantity"))
            {
                int.TryParse(headers.GetValues("quantity").First(), out quantity);
            }
            if (headers.Contains("installed"))
            {
                int.TryParse(headers.GetValues("installed").First(), out installed);
            }
            if (headers.Contains("unit"))
            {
                double.TryParse(headers.GetValues("unit").First(), out unit);
            }
            if (headers.Contains("price"))
            {
                double.TryParse(headers.GetValues("price").First(), out price);
            }



            try
            {
                bool status = false;
                string errormessage = "";
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                    var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();
                    var Ticket = HSapiFacade.GetTicketByTicketId(ticketid);
                    string currentUser = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();

                    var cae = new CustomerAppointmentEquipment()
                    {
                        AppointmentId = ticketid,
                        EquipmentId = serviceId,
                        Quantity = quantity,
                        UnitPrice = unit,
                        TotalPrice = quantity * unit,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        CreatedBy = currentUser,
                        EquipName = serviceName,
                        EquipDetail = "",
                        IsEquipmentRelease = false,
                        IsService = true,
                        CreatedByUid = userid,
                        IsAgreementItem = false,
                        IsBilling = false,
                        IsBillingProcess = false,
                        InstalledByUid = Guid.Empty,
                        IsCopied = false
                    };

                    cae.Id = (int)HSapiFacade.InsertCustomerAppointmentEquipmentDetail(cae);

                    itemid = cae.Id;

                    CustomerPackageService CustomerPackageService = new CustomerPackageService()
                    {
                        CompanyId = companyId,
                        CustomerId = Ticket.CustomerId,
                        PackageId = Ticket.CustomerId,
                        EquipmentId = serviceId,
                        MonthlyRate = unit,
                        DiscountRate = 0,
                        Total = quantity * unit,
                        ManufacturerId = new Guid(),
                        LocationId = new Guid(),
                        TypeId = new Guid(),
                        ModelId = new Guid(),
                        FinishId = new Guid(),
                        CapacityId = new Guid(),
                        IsPackageService = false,
                        IsNonCommissionable = false,
                        AppointmentIntId = Ticket.Id,
                        AppointmentEquipmentIntId = itemid,
                    };
                    HSapiFacade.InsertCustomerPackageService(CustomerPackageService);

                    #region Add Log
                    if (!string.IsNullOrWhiteSpace(ActionDisplyText) && !string.IsNullOrWhiteSpace(Action) && !string.IsNullOrWhiteSpace(UserId))
                        AddUserActivityForCustomer(ActionDisplyText, PageUrl, Action, UserAgent, UserIp, UserName, UserId, CustomerId, "Ticket", ticketid.ToString());
                    #endregion
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(null));
                }

                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }

        [Route("update-service")]
        [HttpPost]
        public HttpResponseMessage UpdateTicketBookingServiceItems()
        {
            Guid ticketid = new Guid();
            Guid userid = new Guid();
            Guid serviceId = Guid.Empty;
            int id = 0;

            string serviceName = string.Empty;
            int quantity = 0;
            int installed = 0;
            double unitPrice = 0;
            double totalPrice = 0;
            string PageUrl = "";
            string Action = "";
            string ActionDisplyText = "";
            string UserIp = "";
            string UserAgent = "";
            string UserName = "";
            string UserId = "";
            string CustomerId = "";


            var re = Request;
            var headers = re.Headers;
            #region Log Header
            if (headers.Contains("pageurl"))
            {
                PageUrl = headers.GetValues("pageurl").First();
            }
            if (headers.Contains("action"))
            {
                Action = headers.GetValues("action").First();
            }
            if (headers.Contains("actiondisplytext"))
            {
                ActionDisplyText = headers.GetValues("actiondisplytext").First();
            }
            if (headers.Contains("userip"))
            {
                UserIp = headers.GetValues("userip").First();
            }
            if (headers.Contains("useragent"))
            {
                UserAgent = headers.GetValues("useragent").First();
            }
            if (headers.Contains("username"))
            {
                UserName = headers.GetValues("username").First();
            }
            if (headers.Contains("userid"))
            {
                UserId = headers.GetValues("userid").First();
            }
            if (headers.Contains("customerid"))
            {
                CustomerId = headers.GetValues("customerid").First();
            }
            #endregion
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            if (headers.Contains("userid"))
            {
                Guid.TryParse(headers.GetValues("userid").First(), out userid);
            }
            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }


            if (headers.Contains("serviceId"))
            {
                Guid.TryParse(headers.GetValues("serviceId").First(), out serviceId);
            }
            if (headers.Contains("serviceName"))
            {
                serviceName = headers.GetValues("serviceName").First();
            }
            if (headers.Contains("quantity"))
            {
                int.TryParse(headers.GetValues("quantity").First(), out quantity);
            }
            if (headers.Contains("installed"))
            {
                int.TryParse(headers.GetValues("installed").First(), out installed);
            }
            if (headers.Contains("unitPrice"))
            {
                double.TryParse(headers.GetValues("unitPrice").First(), out unitPrice);
            }
            if (headers.Contains("totalPrice"))
            {
                double.TryParse(headers.GetValues("totalPrice").First(), out totalPrice);
            }


            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string currentUser = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();

                if (!string.IsNullOrWhiteSpace(currentUser))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(
                        currentUser,
                        new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault())
                    );

                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Authorization Denied."));
                    }

                    var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                    var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();


                    if (id > 0)
                    {
                        var cae = HSapiFacade.GetCustomerAppointmentEquipmentById(id);

                        if (cae != null)
                        {
                            cae.EquipmentId = serviceId;
                            cae.Quantity = quantity;
                            cae.UnitPrice = unitPrice;
                            cae.TotalPrice = quantity * unitPrice;
                            cae.CreatedBy = currentUser;
                            cae.EquipName = serviceName;
                            cae.CreatedByUid = userid;

                            HSapiFacade.UpdateCustomerAppoinmentEquipment(cae);

                            var packageServiceList = HSapiFacade.GetCustomerPackageServiceAPIById(cae.Id);

                            if (packageServiceList != null && packageServiceList.Any())
                            {
                                foreach (var packageser in packageServiceList)
                                {
                                    packageser.MonthlyRate = unitPrice;
                                    packageser.Total = quantity * unitPrice;
                                    packageser.EquipmentId = serviceId;

                                    HSapiFacade.UpdateCustomerPackageService(packageser);
                                }

                                #region Add Log
                                if (!string.IsNullOrWhiteSpace(ActionDisplyText) && !string.IsNullOrWhiteSpace(Action) && !string.IsNullOrWhiteSpace(UserId))
                                {
                                    AddUserActivityForCustomer(ActionDisplyText, PageUrl, Action, UserAgent, UserIp, UserName, UserId, CustomerId, "Ticket", ticketid.ToString());
                                }
                                #endregion

                                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(null));
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, ApiResponse<object>.Error("Service not found."));
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, ApiResponse<object>.Error("Invalid ID provided."));
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("User not authenticated."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ApiResponse<object>.Error(ex.Message));
            }
        }

        [Route("delete-service")]
        [HttpDelete]
        public HttpResponseMessage DeleteTicketBookingServiceItems()
        {
            Guid ticketid = new Guid();
            int id = 0;
            string PageUrl = "";
            string Action = "";
            string ActionDisplyText = "";
            string UserIp = "";
            string UserAgent = "";
            string UserName = "";
            string UserId = "";
            string CustomerId = "";

            var re = Request;
            var headers = re.Headers;
            #region Log Header
            if (headers.Contains("pageurl"))
            {
                PageUrl = headers.GetValues("pageurl").First();
            }
            if (headers.Contains("action"))
            {
                Action = headers.GetValues("action").First();
            }
            if (headers.Contains("actiondisplytext"))
            {
                ActionDisplyText = headers.GetValues("actiondisplytext").First();
            }
            if (headers.Contains("userip"))
            {
                UserIp = headers.GetValues("userip").First();
            }
            if (headers.Contains("useragent"))
            {
                UserAgent = headers.GetValues("useragent").First();
            }
            if (headers.Contains("username"))
            {
                UserName = headers.GetValues("username").First();
            }
            if (headers.Contains("userid"))
            {
                UserId = headers.GetValues("userid").First();
            }
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            if (headers.Contains("customerid"))
            {
                CustomerId = headers.GetValues("customerid").First();
            }
            #endregion

            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string currentUser = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();

                if (!string.IsNullOrWhiteSpace(currentUser))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(
                        currentUser,
                        new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault())
                    );

                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Authorization Denied."));
                    }

                    var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                    var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    if (id > 0)
                    {
                        var packageServiceList = HSapiFacade.GetCustomerPackageServiceAPIById(id);

                        if (packageServiceList != null)
                        {
                            foreach (var packageser in packageServiceList)
                            {
                                HSapiFacade.DeleteCustomerPackageServiceById(packageser.Id);
                            }

                            HSapiFacade.DeleteCustomerAppoinmentEquipment(id);

                            #region Add Log
                            if (!string.IsNullOrWhiteSpace(ActionDisplyText) && !string.IsNullOrWhiteSpace(Action) && !string.IsNullOrWhiteSpace(UserId))
                            {
                                AddUserActivityForCustomer(ActionDisplyText, PageUrl, Action, UserAgent, UserIp, UserName, UserId, CustomerId, "Ticket", ticketid.ToString());
                            }
                            #endregion

                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(null));
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, ApiResponse<object>.Error("Service not found."));
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiResponse<object>.Error("Invalid ID provided."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ApiResponse<object>.Error(ex.Message));
            }
        }

        [Route("create-equipment")]
        [HttpPost]
        public HttpResponseMessage SaveTicketBookingItems()
        {
            Guid ticketid = new Guid();
            Guid userid = new Guid();
            Guid sb = new Guid();
            Guid equipmentId = Guid.Empty;
            int itemid = 0;

            string equipmentName = string.Empty;
            int quantity = 0;
            int installed = 0;
            double unitPrice = 0;
            double totalPrice = 0;
            string PageUrl = "";
            string Action = "";
            string ActionDisplyText = "";
            string UserIp = "";
            string UserAgent = "";
            string UserName = "";
            string UserId = "";
            string CustomerId = "";


            var re = Request;
            var headers = re.Headers;
            #region Log Header
            if (headers.Contains("pageurl"))
            {
                PageUrl = headers.GetValues("pageurl").First();
            }
            if (headers.Contains("action"))
            {
                Action = headers.GetValues("action").First();
            }
            if (headers.Contains("actiondisplytext"))
            {
                ActionDisplyText = headers.GetValues("actiondisplytext").First();
            }
            if (headers.Contains("userip"))
            {
                UserIp = headers.GetValues("userip").First();
            }
            if (headers.Contains("useragent"))
            {
                UserAgent = headers.GetValues("useragent").First();
            }
            if (headers.Contains("username"))
            {
                UserName = headers.GetValues("username").First();
            }
            if (headers.Contains("userid"))
            {
                UserId = headers.GetValues("userid").First();
            }
            if (headers.Contains("customerid"))
            {
                CustomerId = headers.GetValues("customerid").First();
            }
            #endregion
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            if (headers.Contains("userid"))
            {
                Guid.TryParse(headers.GetValues("userid").First(), out userid);
            }
            if (headers.Contains("itemid"))
            {
                int.TryParse(headers.GetValues("itemid").First(), out itemid);
            }


            if (headers.Contains("equipmentId"))
            {
                Guid.TryParse(headers.GetValues("equipmentId").First(), out equipmentId);
            }
            if (headers.Contains("equipmentName"))
            {
                equipmentName = headers.GetValues("equipmentName").First();
            }
            if (headers.Contains("quantity"))
            {
                int.TryParse(headers.GetValues("quantity").First(), out quantity);
            }
            if (headers.Contains("installed"))
            {
                int.TryParse(headers.GetValues("installed").First(), out installed);
            }
            if (headers.Contains("unitPrice"))
            {
                double.TryParse(headers.GetValues("unitPrice").First(), out unitPrice);
            }
            if (headers.Contains("totalPrice"))
            {
                double.TryParse(headers.GetValues("totalPrice").First(), out totalPrice);
            }



            try
            {
                bool status = false;
                string errormessage = "";
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                    var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();
                    var Ticket = HSapiFacade.GetTicketByTicketId(ticketid);
                    string currentUser = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();

                    var cae = new CustomerAppointmentEquipment()
                    {
                        AppointmentId = ticketid,
                        EquipmentId = equipmentId,
                        Quantity = quantity,
                        UnitPrice = unitPrice,
                        TotalPrice = quantity * unitPrice,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        CreatedBy = currentUser,
                        EquipName = equipmentName,
                        EquipDetail = "",
                        IsEquipmentRelease = false,
                        IsService = false,
                        CreatedByUid = userid,
                        IsAgreementItem = false,
                        IsBilling = false,
                        IsBillingProcess = false,
                        InstalledByUid = Guid.Empty,
                        IsCopied = false
                    };

                    cae.Id = (int)HSapiFacade.InsertCustomerAppointmentEquipmentDetail(cae);

                    itemid = cae.Id;

                    CustomerPackageEqp customerPackageEqp = new CustomerPackageEqp
                    {
                        CompanyId = companyId,
                        CustomerId = Ticket.CustomerId,
                        PackageId = Ticket.CustomerId,
                        EquipmentId = equipmentId,
                        IsIncluded = false,
                        IsDevice = false,
                        IsOptionalEqp = false,
                        Quantity = quantity,
                        UnitPrice = unitPrice,
                        DiscountUnitPricce = 0.0,
                        DiscountPckage = 0.0,
                        Total = quantity * unitPrice,
                        IsServiceEquipment = false,
                        ServiceId = Guid.Empty,
                        IsTransfered = false,
                        IsEqpExist = false,
                        IsPackageEqp = false,
                        IsNonCommissionable = false,
                        AppointmentIntId = Ticket.Id,
                        AppointmentEquipmentIntId = itemid
                    };

                    HSapiFacade.InsertCustomerPackageEqp(customerPackageEqp);
                    #region Add Log
                    if (!string.IsNullOrWhiteSpace(ActionDisplyText) && !string.IsNullOrWhiteSpace(Action) && !string.IsNullOrWhiteSpace(UserId))
                        AddUserActivityForCustomer(ActionDisplyText, PageUrl, Action, UserAgent, UserIp, UserName, UserId, CustomerId, "Ticket", ticketid.ToString());
                    #endregion
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(null));
                }

                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }

        [Route("update-equipment")]
        [HttpPost]
        public HttpResponseMessage UpdateTicketBookingItems()
        {
            Guid ticketid = new Guid();
            Guid userid = new Guid();
            Guid equipmentId = Guid.Empty;
            int id = 0;

            string equipmentName = string.Empty;
            int quantity = 0;
            int installed = 0;
            double unitPrice = 0;
            double totalPrice = 0;
            string PageUrl = "";
            string Action = "";
            string ActionDisplyText = "";
            string UserIp = "";
            string UserAgent = "";
            string UserName = "";
            string UserId = "";
            string CustomerId = "";


            var re = Request;
            var headers = re.Headers;
            #region Log Header
            if (headers.Contains("pageurl"))
            {
                PageUrl = headers.GetValues("pageurl").First();
            }
            if (headers.Contains("action"))
            {
                Action = headers.GetValues("action").First();
            }
            if (headers.Contains("actiondisplytext"))
            {
                ActionDisplyText = headers.GetValues("actiondisplytext").First();
            }
            if (headers.Contains("userip"))
            {
                UserIp = headers.GetValues("userip").First();
            }
            if (headers.Contains("useragent"))
            {
                UserAgent = headers.GetValues("useragent").First();
            }
            if (headers.Contains("username"))
            {
                UserName = headers.GetValues("username").First();
            }
            if (headers.Contains("userid"))
            {
                UserId = headers.GetValues("userid").First();
            }
            if (headers.Contains("customerid"))
            {
                CustomerId = headers.GetValues("customerid").First();
            }
            #endregion
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            if (headers.Contains("userid"))
            {
                Guid.TryParse(headers.GetValues("userid").First(), out userid);
            }
            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }


            if (headers.Contains("equipmentId"))
            {
                Guid.TryParse(headers.GetValues("equipmentId").First(), out equipmentId);
            }
            if (headers.Contains("equipmentName"))
            {
                equipmentName = headers.GetValues("equipmentName").First();
            }
            if (headers.Contains("quantity"))
            {
                int.TryParse(headers.GetValues("quantity").First(), out quantity);
            }
            if (headers.Contains("installed"))
            {
                int.TryParse(headers.GetValues("installed").First(), out installed);
            }
            if (headers.Contains("unitPrice"))
            {
                double.TryParse(headers.GetValues("unitPrice").First(), out unitPrice);
            }
            if (headers.Contains("totalPrice"))
            {
                double.TryParse(headers.GetValues("totalPrice").First(), out totalPrice);
            }


            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string currentUser = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();

                if (!string.IsNullOrWhiteSpace(currentUser))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(
                        currentUser,
                        new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault())
                    );

                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Authorization Denied."));
                    }

                    var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                    var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();


                    if (id > 0)
                    {
                        var cae = HSapiFacade.GetCustomerAppointmentEquipmentById(id);

                        if (cae != null)
                        {
                            cae.EquipmentId = equipmentId;
                            cae.Quantity = quantity;
                            cae.UnitPrice = unitPrice;
                            cae.TotalPrice = quantity * unitPrice;
                            cae.CreatedBy = currentUser;
                            cae.EquipName = equipmentName;
                            cae.CreatedByUid = userid;

                            HSapiFacade.UpdateCustomerAppoinmentEquipment(cae);

                            var packageEqpList = HSapiFacade.GetCustomerPackageEqpAPIById(cae.Id);

                            if (packageEqpList != null && packageEqpList.Any())
                            {
                                foreach (var packageeqp in packageEqpList)
                                {
                                    packageeqp.Quantity = quantity;
                                    packageeqp.UnitPrice = unitPrice;
                                    packageeqp.Total = quantity * unitPrice;
                                    packageeqp.EquipmentId = equipmentId;

                                    HSapiFacade.UpdateCustomerPackageEqp(packageeqp);
                                }

                                #region Add Log
                                if (!string.IsNullOrWhiteSpace(ActionDisplyText) && !string.IsNullOrWhiteSpace(Action) && !string.IsNullOrWhiteSpace(UserId))
                                {
                                    AddUserActivityForCustomer(ActionDisplyText, PageUrl, Action, UserAgent, UserIp, UserName, UserId, CustomerId, "Ticket", ticketid.ToString());
                                }
                                #endregion

                                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(null));
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.NotFound, ApiResponse<object>.Error("Equipment not found."));
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, ApiResponse<object>.Error("Invalid ID provided."));
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("User not authenticated."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ApiResponse<object>.Error(ex.Message));
            }
        }

        [Route("delete-equipment")]
        [HttpDelete]
        public HttpResponseMessage DeleteTicketBookingItems()
        {
            Guid ticketid = new Guid();
            int id = 0;
            string PageUrl = "";
            string Action = "";
            string ActionDisplyText = "";
            string UserIp = "";
            string UserAgent = "";
            string UserName = "";
            string UserId = "";
            string CustomerId = "";

            var re = Request;
            var headers = re.Headers;
            #region Log Header
            if (headers.Contains("pageurl"))
            {
                PageUrl = headers.GetValues("pageurl").First();
            }
            if (headers.Contains("action"))
            {
                Action = headers.GetValues("action").First();
            }
            if (headers.Contains("actiondisplytext"))
            {
                ActionDisplyText = headers.GetValues("actiondisplytext").First();
            }
            if (headers.Contains("userip"))
            {
                UserIp = headers.GetValues("userip").First();
            }
            if (headers.Contains("useragent"))
            {
                UserAgent = headers.GetValues("useragent").First();
            }
            if (headers.Contains("username"))
            {
                UserName = headers.GetValues("username").First();
            }
            if (headers.Contains("userid"))
            {
                UserId = headers.GetValues("userid").First();
            }
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            if (headers.Contains("customerid"))
            {
                CustomerId = headers.GetValues("customerid").First();
            }
            #endregion

            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string currentUser = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();

                if (!string.IsNullOrWhiteSpace(currentUser))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(
                        currentUser,
                        new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault())
                    );

                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Authorization Denied."));
                    }

                    var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                    var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    if (id > 0)
                    {
                        var packageEqpList = HSapiFacade.GetCustomerPackageEqpAPIById(id);

                        if (packageEqpList != null)
                        {
                            foreach (var packageeqp in packageEqpList)
                            {
                                HSapiFacade.DeleteCustomerPackageEqpById(packageeqp.Id);
                            }

                            HSapiFacade.DeleteCustomerAppoinmentEquipment(id);

                            #region Add Log
                            if (!string.IsNullOrWhiteSpace(ActionDisplyText) && !string.IsNullOrWhiteSpace(Action) && !string.IsNullOrWhiteSpace(UserId))
                            {
                                AddUserActivityForCustomer(ActionDisplyText, PageUrl, Action, UserAgent, UserIp, UserName, UserId, CustomerId, "Ticket", ticketid.ToString());
                            }
                            #endregion

                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(null));
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, ApiResponse<object>.Error("Equipment not found."));
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiResponse<object>.Error("Invalid ID provided."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ApiResponse<object>.Error(ex.Message));
            }
        }
        [Route("search-equipment")]
        [HttpGet]
        public HttpResponseMessage BookingExtraItemsSuggestion()
        {
            string key = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("key"))
            {
                key = headers.GetValues("key").First();
            }
            try
            {
                List<EquipmentSearchModel> model = new List<EquipmentSearchModel>();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();
                    int ItemsLoadCount = HSapiFacade.GetEquipmentSearchMaxLoad(usercontext.CompanyId);
                    var equipmentList = HSapiFacade.GetEqupmentListBySearchKeyAndCompanyId(key, usercontext.CompanyId, ItemsLoadCount, "")
                        .Where(e => e.EquipmentClassId != 2)
                            .Select(e => new EquipmentSuggestionResponseModel
                            {
                                id = e.Id,
                                equipmentGuid = e.EquipmentId,
                                name = e.EquipmentName,
                                sku = e.SKU,
                                barcode = e.Barcode,
                                point = e.Point,
                                onHand = e.QuantityOnHand,
                                unitCost = e.Equipmentvendorcost,
                                unitPrice = e.RetailPrice
                            }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<List<EquipmentSuggestionResponseModel>>.Success(equipmentList));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }
        [Route("search-equipment-barcode")]
        [HttpGet]
        public HttpResponseMessage BookingExtraItemsSuggestionBarcode()
        {
            string code = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("code"))
            {
                code = headers.GetValues("code").First();
            }
            try
            {
                List<EquipmentSearchModel> model = new List<EquipmentSearchModel>();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();
                    int ItemsLoadCount = HSapiFacade.GetEquipmentSearchMaxLoad(usercontext.CompanyId);
                    var equipmentList = HSapiFacade.GetEqupmentListBySearchKeyAndCompanyIdBarcode(code, usercontext.CompanyId, ItemsLoadCount, "")
                        .Where(e => e.EquipmentClassId != 2)
                            .Select(e => new EquipmentSuggestionResponseModel
                            {
                                id = e.Id,
                                equipmentGuid = e.EquipmentId,
                                name = e.EquipmentName,
                                sku = e.SKU,
                                barcode = e.Barcode,
                                point = e.Point,
                                onHand = e.QuantityOnHand,
                                unitCost = e.Equipmentvendorcost,
                                unitPrice = e.RetailPrice
                            }).ToList();

                    if (!equipmentList.Any())
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<List<EquipmentSuggestionResponseModel>>.Success(null));
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<List<EquipmentSuggestionResponseModel>>.Success(equipmentList));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }
        [Route("equipments")]
        [HttpGet]
        public HttpResponseMessage GetJobAttachBookingItems()
        {
            Guid ticketid = new Guid();
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            try
            {
                List<JobFileCustomModel> JobFileCustomModel = new List<JobFileCustomModel>();
                AttachBookingItemsModel model = new AttachBookingItemsModel();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();
                    Ticket _tk = HSapiFacade.GetTicketByTicketId(ticketid);
                    if (_tk != null)
                    {
                        var equipments = HSapiFacade.GetAllCustomerAppointmentEquipListByAppointmentId(ticketid)
                            .Where(e => e.EquipmentClassId != 2)
                            .Select(e => new CustomerAppointmentEquipmentApiResponse
                            {
                                id = e.id,
                                equipmentGuid = e.equipmentGuid,
                                name = e.name,
                                point = e.point,
                                barcode = e.barcode,
                                onHand = e.onHand,
                                installed = e.installed,
                                unitCost = e.unitCost,
                                unitPrice = e.unitPrice,
                                quantity = e.quantity,
                                totalPrice = e.totalPrice
                            }).ToList();

                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<List<CustomerAppointmentEquipmentApiResponse>>.Success(equipments));
                    }
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Ticket Not Found"));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }
        [Route("search-service")]
        [HttpGet]
        public HttpResponseMessage BookingExtraItemsSuggestionSearch()
        {
            string key = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("key"))
            {
                key = headers.GetValues("key").First();
            }
            try
            {
                List<EquipmentSearchModel> model = new List<EquipmentSearchModel>();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();
                    int ItemsLoadCount = HSapiFacade.GetEquipmentSearchMaxLoad(usercontext.CompanyId);
                    var serviceList = HSapiFacade.GetEqupmentListBySearchKeyAndCompanyId(key, usercontext.CompanyId, ItemsLoadCount, "")
                .Where(e => e.EquipmentClassId == 2)
                .Select(e => new ServiceSuggestionResponseModel
                {
                    id = e.Id,
                    serviceGuid = e.EquipmentId,
                    name = e.EquipmentName,
                    unitCost = e.Equipmentvendorcost,
                    unitPrice = e.RetailPrice
                }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<List<ServiceSuggestionResponseModel>>.Success(serviceList));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }

        [Route("services")]
        [HttpGet]
        public HttpResponseMessage GetBookingExtraItems()
        {
            Guid ticketid = new Guid();
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            try
            {
                List<JobFileCustomModel> JobFileCustomModel = new List<JobFileCustomModel>();
                AttachBookingItemsModel model = new AttachBookingItemsModel();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();
                    Ticket _tk = HSapiFacade.GetTicketByTicketId(ticketid);
                    if (_tk != null)
                    {
                        var equipments = HSapiFacade.GetAllCustomerAppointmentEquipListByAppointmentId(ticketid)
                            .Where(e => e.EquipmentClassId == 2)
                            .Select(e => new SimplifiedCustomerAppointmentEquipmentApi
                            {
                                id = e.id,
                                serviceGuid = e.equipmentGuid,
                                name = e.name,
                                quantity = e.quantity,
                                unitCost = e.unitCost,
                                unitPrice = e.unitPrice,
                                totalPrice = e.totalPrice
                            }).ToList();

                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<List<SimplifiedCustomerAppointmentEquipmentApi>>.Success(equipments));
                    }
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Ticket Not Found"));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }

        [Route("delete-file")]
        [HttpDelete]
        public HttpResponseMessage DeleteRuGImage()
        {
            int id = 0;
            string PageUrl = "";
            string Action = "";
            string ActionDisplyText = "";
            string UserIp = "";
            string UserAgent = "";
            string UserName = "";
            string UserId = "";
            string CustomerId = "";
            var re = Request;
            var headers = re.Headers;
            #region Log Header
            if (headers.Contains("pageurl"))
            {
                PageUrl = headers.GetValues("pageurl").First();
            }
            if (headers.Contains("action"))
            {
                Action = headers.GetValues("action").First();
            }
            if (headers.Contains("actiondisplytext"))
            {
                ActionDisplyText = headers.GetValues("actiondisplytext").First();
            }
            if (headers.Contains("userip"))
            {
                UserIp = headers.GetValues("userip").First();
            }
            if (headers.Contains("useragent"))
            {
                UserAgent = headers.GetValues("useragent").First();
            }
            if (headers.Contains("username"))
            {
                UserName = headers.GetValues("username").First();
            }
            if (headers.Contains("userid"))
            {
                UserId = headers.GetValues("userid").First();
            }
            if (headers.Contains("customerid"))
            {
                CustomerId = headers.GetValues("customerid").First();
            }
            #endregion
            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();
                    TicketFile ticketfile = HSapiFacade.GetTicketFileById(id);
                    var delstatus = HSapiFacade.DeleteTicketFileById(id);
                    #region Add Log
                    if (!string.IsNullOrWhiteSpace(ActionDisplyText) && !string.IsNullOrWhiteSpace(Action) && !string.IsNullOrWhiteSpace(UserId))
                        AddUserActivityForCustomer(ActionDisplyText, PageUrl, Action, UserAgent, UserIp, UserName, UserId, CustomerId, "Ticket", ticketfile.TicketId.ToString());
                    #endregion
                    if (delstatus)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(null));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Error("Failed to delete file."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }


        [Route("create-file")]
        public HttpResponseMessage ManageJobFiles()
        {
            string PageUrl = "";
            string Action = "";
            string ActionDisplyText = "";
            string UserIp = "";
            string UserAgent = "";
            string username = "";
            string UserId = "";
            string CustomerId = "";
            string rotateval = "";
            string filename = "";
            string filepath = "";
            string caption = "";
            int fileid = 0;
            int ticketbookingdetailsid = 0;
            Guid ticketid = Guid.Empty;
            Guid userid = Guid.Empty;
            Guid customerId = Guid.Empty;
            bool isUploaded = false;
            var re = Request;
            var headers = re.Headers;

            #region Log Header
            if (headers.Contains("pageurl")) PageUrl = headers.GetValues("pageurl").First();
            if (headers.Contains("action")) Action = headers.GetValues("action").First();
            if (headers.Contains("actiondisplytext")) ActionDisplyText = headers.GetValues("actiondisplytext").First();
            if (headers.Contains("userip")) UserIp = headers.GetValues("userip").First();
            if (headers.Contains("useragent")) UserAgent = headers.GetValues("useragent").First();
            if (headers.Contains("username")) username = headers.GetValues("username").First();
            if (headers.Contains("userid")) UserId = headers.GetValues("userid").First();
            if (headers.Contains("rotateval")) rotateval = headers.GetValues("rotateval").First();
            if (headers.Contains("ticketid")) Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            if (headers.Contains("userid")) Guid.TryParse(headers.GetValues("userid").First(), out userid);
            if (headers.Contains("customerId")) Guid.TryParse(headers.GetValues("customerId").First(), out customerId);
            if (headers.Contains("filename")) filename = headers.GetValues("filename").First();
            if (headers.Contains("filepath")) filepath = headers.GetValues("filepath").First();
            if (headers.Contains("caption")) caption = HttpUtility.UrlDecode(headers.GetValues("caption").First());
            if (headers.Contains("fileid")) int.TryParse(headers.GetValues("fileid").First(), out fileid);
            if (headers.Contains("ticketbookingdetailsid")) int.TryParse(headers.GetValues("ticketbookingdetailsid").First(), out ticketbookingdetailsid);
            #endregion

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                username = identity.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
                var userIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "userid")?.Value;
                var userId = Guid.TryParse(userIdClaim, out Guid parsedUserId) ? parsedUserId : Guid.Empty;
                var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;

                if (string.IsNullOrWhiteSpace(username) || companyId == Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "User is not authorized.");
                }

                var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(username, companyId);

                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");

                ComId = usercontext.CompanyId.ToString();
                APIInitialize();

                #region Validations
                if (ticketid == Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new
                    {
                        isUploaded = isUploaded,
                        filePath = "",
                        message = "Invalid ticket id."
                    });
                }

                var Ticket = HSapiFacade.GetTicketByTicketId(ticketid);
                if (Ticket == null || Ticket.CompanyId != companyId)
                {
                    return Request.CreateResponse(HttpStatusCode.Forbidden, new
                    {
                        isUploaded = isUploaded,
                        filePath = "",
                        message = "Access denied."
                    });
                }

                var Customer = HSapiFacade.GetCustomerByCustomerId(Ticket.CustomerId);
                if (Customer == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new
                    {
                        isUploaded = isUploaded,
                        filePath = "",
                        message = "Customer not found."
                    });
                }
                #endregion

                HttpPostedFile file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;

                string FileDescription = null;
                if (file != null)
                {
                    FileDescription = file.FileName;
                }
                // string ImageDomain = AppConfig.ImageDomain;
                string tempFolderName = ConfigurationManager.AppSettings["File.TicketFile"];
                var comname = HSapiFacade.GetCompanyByComapnyId(usercontext.CompanyId).CompanyName;
                tempFolderName = string.Format(tempFolderName, comname);
                tempFolderName += DateTime.Now.UTCCurrentTime().Year + "-" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

                Random rand = new Random();
                string FileName = rand.Next().ToString();
                FileName += "-___" + file.FileName.Replace(" ", "_");
                string filePath = "";

                string UploadExactpathimg = AppConfig.UploadExactPath;
                string tempFolderPath = UploadExactpathimg + tempFolderName;


                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    file.SaveAs(Path.Combine(tempFolderPath, FileName));
                    isUploaded = true;
                    filePath = string.Concat("/", tempFolderName, "/", FileName);
                    //filepath = ImageDomain + "/" + filepath;

                    var TicketFile = new TicketFile()
                    {
                        FileAddedBy = userId,
                        FileAddedDate = DateTime.Now.UTCCurrentTime(),
                        FileLocation = AppConfig.DomainSitePath + filePath,
                        FileName = file.FileName.ReplaceSpecialCharFile(),
                        Filesize = file.ContentLength,
                        TicketId = ticketid,
                        Description = caption,
                    };
                    HSapiFacade.InsertTicketFile(TicketFile);

                    var CustomerFile = new CustomerFile()
                    {
                        CompanyId = companyId,
                        FileId = Guid.NewGuid(),
                        CustomerId = Ticket.CustomerId,
                        FileDescription = file.FileName.ReplaceSpecialCharFile(),
                        Filename = filePath,
                        FileSize = file.ContentLength,
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        IsActive = true,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = userId,
                        UpdatedDate = DateTime.Now.UTCCurrentTime(),
                        WMStatus = LabelHelper.WatermarkStatus.Pending,
                        AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                    };
                    HSapiFacade.InsertCustomerFile(CustomerFile);

                    List<TicketUser> UserList = HSapiFacade.GetTicketUserListByTicketId(Ticket.TicketId);
                    if (UserList.Count() > 0)
                    {
                        #region Insert notification
                        Notification notification = new Notification()
                        {
                            CompanyId = companyId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            NotificationId = Guid.NewGuid(),
                            Type = LabelHelper.NotificationType.Employee,
                            Who = userId,
                            What = string.Format(@"{0} attached a file on Ticket Ticket #{1}", "{0}", Ticket.Id),
                            NotificationUrl = AppConfig.DomainSitePath + "/Ticket/AddTicket/?Id=" + Ticket.Id
                        };
                        HSapiFacade.InsertNotification(notification);
                        #endregion

                        List<Guid> assigned = UserList.Select(x => x.UserId).GroupBy(x => x).Select(x => x.Key).ToList();

                        if (assigned.Where(x => x == Ticket.CreatedBy).Count() == 0)
                        {
                            if (Ticket.CreatedBy != Ticket.CustomerId)
                            {
                                assigned.Add(Ticket.CreatedBy);
                            }
                        }
                        foreach (Guid item in assigned)
                        {
                            if (item != Ticket.CustomerId)
                            {
                                #region set user to notification
                                NotificationUser nu = new NotificationUser()
                                {
                                    NotificationId = notification.NotificationId,
                                    IsRead = false,
                                    NotificationPerson = item,
                                };
                                HSapiFacade.InsertNotificationUser(nu);
                                #endregion
                            }
                        }
                    }
                }

                if (isUploaded)
                {
                    #region Send Ticket Reply Notification Email

                    List<TicketUser> UserList = HSapiFacade.GetTicketUserListByTicketId(Ticket.TicketId);

                    string ToEmailList = "";
                    if (UserList != null && UserList.Count() > 0)
                    {
                        List<Guid> assigned = UserList.Select(x => x.UserId).GroupBy(x => x).Select(x => x.Key).ToList();
                        if (assigned.Where(x => x == Ticket.CreatedBy).Count() == 0)
                        {
                            if (Ticket.CreatedBy != Ticket.CustomerId)
                            {
                                assigned.Add(Ticket.CreatedBy);
                            }
                        }

                        foreach (Guid item in assigned)
                        {
                            if (item != Ticket.CustomerId)
                            {
                                Employee emp = HSapiFacade.GetEmployeeByUserId(item);
                                if (emp != null && emp.Email.IsValidEmailAddress())
                                {
                                    ToEmailList += string.Format("{0}>{1} {2};", emp.Email, emp.FirstName, emp.LastName);
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(ToEmailList))
                    {
                        string CustomerName = Customer.FirstName + " " + Customer.LastName;
                        if (Customer.Type == LabelHelper.CustomerType.Commercial && !string.IsNullOrWhiteSpace(Customer.BusinessName))
                        {
                            CustomerName = Customer.BusinessName;
                        }

                        TicketNotificationEmails TicketReplyNotificationEmail = new TicketNotificationEmails()
                        {
                            CompanyId = companyId,
                            CreatedByName = username,
                            TicketMessage = string.Format("Attached a file {0}", file.FileName.ReplaceSpecialCharFile()),
                            CreatedForCustomerName = CustomerName,
                            TicketNumber = string.Format("Ticket #{0}", Ticket.Id),
                            ToEmail = ToEmailList,
                            HeaderMessage = "A new file has been added",
                            Subject = string.Format("A new file has been added to Ticket #{0}", Ticket.Id),
                            BodyMessage = string.Format("A new file has been attached by {0} on Ticket #{1} for Customer {2}. {3}", username, Ticket.Id, Customer.FirstName + " " + Customer.LastName, FileDescription),
                            TicketUrl = string.Format("{2}{4}/Public/Open/?TicketId={0}&CompanyId={1}&Type={3}", Ticket.TicketId, companyId, AppConfig.SiteDomain, LabelHelper.PublicOpenTypes.Ticket, AppConfig.DomainSitePath)
                        };
                        HSapiFacade.SendTicketCreatedNotificationEmail(TicketReplyNotificationEmail);
                    }
                    #endregion
                }

                #region Add Log
                if (!string.IsNullOrWhiteSpace(ActionDisplyText) && !string.IsNullOrWhiteSpace(Action) && !string.IsNullOrWhiteSpace(UserId))
                    AddUserActivityForCustomer(ActionDisplyText, PageUrl, Action, UserAgent, UserIp, username, UserId, CustomerId, "Ticket", ticketid.ToString());
                #endregion

                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(null));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<string>.Error(ex.Message));
            }

            // Default return in case no other return was hit.
            return Request.CreateResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }



        [Route("files")]
        [HttpGet]
        public HttpResponseMessage GetJobFiles()
        {
            Guid ticketid = new Guid();
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            try
            {
                List<TicketFile> model = new List<TicketFile>();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    model = HSapiFacade.GetJobFilesByTicketId(ticketid);
                    var result = model.Select(file => new
                    {
                        id = file.Id,
                        userGuid = file.FileAddedBy,
                        url = file.FileLocation,
                        caption = file.Description,
                        date = file.FileAddedDate
                    }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<IEnumerable<object>>.Success(result));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<string>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<string>.Error(ex.Message));
            }
        }

        [Route("assigned-users")]
        [HttpGet]
        public HttpResponseMessage GetAllAssignedUsers()
        {
            Guid ticketid = new Guid();

            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }

            try
            {
                string PerGrpAssgnTicketId = "";
                Ticket model = new Ticket();
                List<EmployeeCustomModel> UserList = new List<EmployeeCustomModel>();
                var identity = (ClaimsIdentity)User.Identity;
                var userIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "userid")?.Value;
                var userId = Guid.TryParse(userIdClaim, out Guid parsedUserId) ? parsedUserId : Guid.Empty;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    model = HSapiFacade.GetTicketByTicketId(ticketid);
                    if (model != null)
                    {
                        GlobalSetting PerGrpAssgnTicket = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "TicketAssignPermissionGroup");
                        if (PerGrpAssgnTicket != null && !string.IsNullOrWhiteSpace(PerGrpAssgnTicket.Value))
                        {
                            PerGrpAssgnTicketId = PerGrpAssgnTicket.Value;
                        }
                        var TechnicianList = HSapiFacade.GetEmployeeByCompanyIdAndPerGrpIdAPI(usercontext.CompanyId, LabelHelper.UserTags.Technicians, userId, PerGrpAssgnTicketId);
                        UserList = TechnicianList.OrderBy(x => x.Name).ToList();
                    }
                    var userIds = UserList.Select(u => u.UserId).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResponse<List<Guid>>(userIds, true, null));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }
        }

        [Route("additional-users")]
        [HttpGet]
        public HttpResponseMessage GetAllAdditionalUsers()
        {
            Guid ticketid = new Guid();
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }

            try
            {
                Ticket model = new Ticket();
                List<EmployeeCustomModel> UserList = new List<EmployeeCustomModel>();
                var identity = (ClaimsIdentity)User.Identity;
                var userIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "userid")?.Value;
                var userId = Guid.TryParse(userIdClaim, out Guid parsedUserId) ? parsedUserId : Guid.Empty;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    model = HSapiFacade.GetTicketByTicketId(ticketid);
                    if (model != null)
                    {
                        List<Employee> EmpList = HSapiFacade.GetAllEmployee(usercontext.CompanyId).OrderBy(x => x.FirstName).ToList();
                        var TechnicianList = HSapiFacade.GetEmployeeByCompanyIdAndTagAndTechnician(usercontext.CompanyId, LabelHelper.UserTags.Technicians, new Guid());
                        bool chkadditionalmembertech = Convert.ToBoolean(HSapiFacade.GetTicketAdditionalMemberOnlyTechnicianByCompanyId(usercontext.CompanyId));
                        if (chkadditionalmembertech)
                        {
                            UserList = TechnicianList.OrderBy(x => x.Name).ToList();
                        }
                        else
                        {
                            UserList = EmpList.Select(x => new EmployeeCustomModel()
                            {
                                Name = x.FirstName + " " + x.LastName,
                                UserId = x.UserId
                            }).OrderBy(x => x.Name).ToList();
                        }
                    }
                    var userIds = UserList.Select(u => u.UserId).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResponse<List<Guid>>(userIds, true, null));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }
        }

        [Route("create-note")]
        [HttpPost]
        public HttpResponseMessage SaveJobReply()
        {
            int replyid = 0;
            Guid ticketid = new Guid();
            Guid userid = new Guid();
            bool isprivate = false;
            bool isoverview = false;
            string PageUrl = "";
            string Action = "";
            string ActionDisplyText = "";
            string UserIp = "";
            string UserAgent = "";
            string UserName = "";
            string UserId = "";
            string CustomerId = "";
            string message = "";
            var re = Request;
            var headers = re.Headers;
            #region Log Header
            if (headers.Contains("pageurl"))
            {
                PageUrl = headers.GetValues("pageurl").First();
            }
            if (headers.Contains("action"))
            {
                Action = headers.GetValues("action").First();
            }
            if (headers.Contains("actiondisplytext"))
            {
                ActionDisplyText = headers.GetValues("actiondisplytext").First();
            }
            if (headers.Contains("userip"))
            {
                UserIp = headers.GetValues("userip").First();
            }
            if (headers.Contains("useragent"))
            {
                UserAgent = headers.GetValues("useragent").First();
            }
            if (headers.Contains("username"))
            {
                UserName = headers.GetValues("username").First();
            }
            if (headers.Contains("userid"))
            {
                UserId = headers.GetValues("userid").First();
            }
            if (headers.Contains("customerid"))
            {
                CustomerId = headers.GetValues("customerid").First();
            }
            #endregion
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            if (headers.Contains("userid"))
            {
                Guid.TryParse(headers.GetValues("userid").First(), out userid);
            }
            if (headers.Contains("isprivate"))
            {
                Boolean.TryParse(headers.GetValues("isprivate").First(), out isprivate);
            }
            if (headers.Contains("isoverview"))
            {
                Boolean.TryParse(headers.GetValues("isoverview").First(), out isoverview);
            }
            if (headers.Contains("message"))
            {
                message = HttpUtility.UrlDecode(headers.GetValues("message").First());
            }
            if (headers.Contains("replyid"))
            {
                int.TryParse(headers.GetValues("replyid").First(), out replyid);
            }
            try
            {
                TicketReply model = new TicketReply();
                var identity = (ClaimsIdentity)User.Identity;
                var userIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "userid")?.Value;
                var userId = Guid.TryParse(userIdClaim, out Guid parsedUserId) ? parsedUserId : Guid.Empty;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    if (replyid > 0)
                    {
                        var objreply = HSapiFacade.GetTicketReplyById(replyid);
                        if (objreply != null)
                        {
                            objreply.Message = message;
                            objreply.IsPrivate = isprivate;
                            objreply.IsOverview = isoverview;
                            HSapiFacade.UpdateTicketReply(objreply);
                        }
                    }
                    else
                    {
                        model = new TicketReply()
                        {
                            TicketId = ticketid,
                            UserId = userid,
                            RepliedDate = DateTime.UtcNow,
                            Message = message,
                            IsPrivate = isprivate,
                            IsOverview = isoverview
                        };
                        HSapiFacade.InsertTicketReply(model);
                    }
                    #region InsertOnNoteRemainder
                    var objticket = HSapiFacade.GetTicketByTicketId(ticketid);
                    var objemp = HSapiFacade.GetEmployeeByEmployeeId(userid);
                    CustomerNote cusNote = new CustomerNote()
                    {
                        Notes = message,
                        NoteType = "Ticket",
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        CreatedBy = objemp != null ? objemp.FirstName + " " + objemp.LastName : "System",
                        CreatedByUid = objemp != null ? objemp.UserId : userid,
                        CustomerId = objticket != null ? objticket.CustomerId : Guid.Empty,
                        CompanyId = objticket != null ? objticket.CompanyId : Guid.Empty,
                        IsOverview = isoverview,
                        ReferenceTicketId = objticket != null ? objticket.Id : 0,
                    };
                    if (isprivate == true)
                    {
                        cusNote.IsActive = false;
                    }
                    else
                    {
                        cusNote.IsActive = true;
                    }
                    HSapiFacade.InsertCustomerNote(cusNote);
                    #endregion
                    #region Add Log
                    if (!string.IsNullOrWhiteSpace(ActionDisplyText) && !string.IsNullOrWhiteSpace(Action) && !string.IsNullOrWhiteSpace(UserId))
                        AddUserActivityForCustomer(ActionDisplyText, PageUrl, Action, UserAgent, UserIp, UserName, UserId, CustomerId, "Ticket", ticketid.ToString());
                    #endregion
                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResponse<object>(null, true, null));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }
        }

        [Route("notes")]
        [HttpGet]
        public HttpResponseMessage Notes()
        {
            Guid ticketid = new Guid();
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            try
            {
                List<TicketReplyNoteCustomModel> replies = new List<TicketReplyNoteCustomModel>();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    replies = HSapiFacade.GetJobNotesByTicketId(ticketid);
                    var result = replies.Select(reply => new NoteResponseModel
                    {
                        id = reply.Id,
                        userGuid = reply.UserId,
                        message = reply.Message,
                        date = reply.RepliedDate
                    }).ToList();


                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResponse<List<NoteResponseModel>>(result, true, null));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }
        }

        [Route("update-schedule")]
        public HttpResponseMessage SaveJobSchedule()
        {
            Guid ticketid = new Guid();
            DateTime date = new DateTime();
            string PageUrl = "";
            string Action = "";
            string ActionDisplyText = "";
            string UserIp = "";
            string UserAgent = "";
            string UserName = "";
            string UserId = "";
            string CustomerId = "";
            string startTime = "";
            string endTime = "";
            Guid assigned = Guid.Empty;
            string additionalUsers = "";
            string notifyingUsers = "";
            string status = "";
            string type = "";
            var re = Request;
            var headers = re.Headers;
            #region Log Header
            if (headers.Contains("pageurl"))
            {
                PageUrl = headers.GetValues("pageurl").First();
            }
            if (headers.Contains("action"))
            {
                Action = headers.GetValues("action").First();
            }
            if (headers.Contains("actiondisplytext"))
            {
                ActionDisplyText = headers.GetValues("actiondisplytext").First();
            }
            if (headers.Contains("userip"))
            {
                UserIp = headers.GetValues("userip").First();
            }
            if (headers.Contains("useragent"))
            {
                UserAgent = headers.GetValues("useragent").First();
            }
            if (headers.Contains("username"))
            {
                UserName = headers.GetValues("username").First();
            }
            if (headers.Contains("userid"))
            {
                UserId = headers.GetValues("userid").First();
            }
            if (headers.Contains("customerid"))
            {
                CustomerId = headers.GetValues("customerid").First();
            }
            #endregion
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            if (headers.Contains("date"))
            {
                DateTime.TryParse(headers.GetValues("date").First(), out date);
            }
            if (headers.Contains("startTime"))
            {
                startTime = headers.GetValues("startTime").First();
            }
            if (headers.Contains("endTime"))
            {
                endTime = headers.GetValues("endTime").First();
            }
            if (headers.Contains("assigned"))
            {
                Guid.TryParse(headers.GetValues("assigned").First(), out assigned);
            }
            if (headers.Contains("additionalUsers"))
            {
                additionalUsers = headers.GetValues("additionalUsers").First();
            }
            if (headers.Contains("notifyingUsers"))
            {
                notifyingUsers = headers.GetValues("notifyingUsers").First();
            }
            if (headers.Contains("status"))
            {
                status = headers.GetValues("status").First();
            }
            if (headers.Contains("type"))
            {
                type = headers.GetValues("type").First();
            }
            try
            {
                Ticket model = new Ticket();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<Ticket>.Error("Authorization Denied."));
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    var objuser = HSapiFacade.GetUserByUsername(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (objuser != null)
                    {
                        if (ticketid != new Guid())
                        {
                            model = HSapiFacade.GetTicketByTicketId(ticketid);
                            if (model != null)
                            {
                                model.CompletionDate = date;
                                model.AppointmentStartTime = startTime;
                                model.AppointmentEndTime = endTime;
                                model.Status = !string.IsNullOrWhiteSpace(status) ? status : model.Status;
                                model.TicketType = !string.IsNullOrWhiteSpace(type) ? type : model.TicketType;
                                Guid cusId = Guid.Empty;
                                if (model.Status == LabelHelper.TicketStatus.Completed && model.CompletedDate == null && !string.IsNullOrWhiteSpace(CustomerId) && Guid.TryParse(CustomerId, out cusId) && cusId != Guid.Empty && ticketid != Guid.Empty && assigned != Guid.Empty)
                                {

                                    model.CompletedDate = DateTime.UtcNow;
                                    ShortUrl ShortUrl = new ShortUrl();
                                    Customer cus = HSapiFacade.GetCustomerByCustomerId(cusId);
                                    if (cus != null && !string.IsNullOrWhiteSpace(cus.EmailAddress))
                                    {
                                        Employee emp = HSapiFacade.GetEmployeeByEmployeeId(assigned);
                                        if (emp == null)
                                        {
                                            emp = new Employee();
                                            emp = HSapiFacade.GetEmployeeByEmployeeId(objuser.UserId);
                                        }
                                        CustomSurvey customSurvey = HSapiFacade.GetFirstCustomSurvey();
                                        CustomSurveyUser SurveyTicket = new CustomSurveyUser();
                                        SurveyTicket.UserId = cus.CustomerId;
                                        SurveyTicket.SurveyUserId = Guid.NewGuid();
                                        SurveyTicket.SurveyId = customSurvey.SurveyId;
                                        SurveyTicket.Status = LabelHelper.CustomSurveyStatus.Created;
                                        SurveyTicket.AddedBy = emp.UserId;
                                        SurveyTicket.AddedDate = DateTime.Now.UTCCurrentTime();
                                        SurveyTicket.ReferenceId = ticketid.ToString();
                                        HSapiFacade.InsertCustomSurveyUser(SurveyTicket);
                                        string TicketUrl = string.Format("{2}{4}/Public/Open/?TicketId={0}&CompanyId={1}&Type={3}", SurveyTicket.SurveyUserId, usercontext.CompanyId, AppConfig.SiteDomain, LabelHelper.PublicOpenTypes.CustomSurvey, AppConfig.DomainSitePath);
                                        ShortUrl = HSapiFacade.GetSortUrlByUrl(TicketUrl, Guid.Empty);
                                        List<string> EmailList = new List<string>();
                                        if (cus.EmailAddress.IndexOf(";") > -1)
                                        {
                                            EmailList = cus.EmailAddress.Split(";").ToList();
                                        }
                                        else
                                        {
                                            EmailList.Add(cus.EmailAddress);
                                        }
                                        foreach (var item in EmailList)
                                        {
                                            SendSurveyEmail email = new SendSurveyEmail()
                                            {

                                                Name = !string.IsNullOrWhiteSpace(cus.FirstName) ? "Hi " + cus.FirstName + " " + cus.LastName : "Hello",
                                                shortLink = ShortUrl.Url,
                                                CompanyId = usercontext.CompanyId,
                                                ToEmail = item,
                                                SenderName = emp.FirstName,
                                                CompanyName = usercontext.CompanyName,
                                                Subject = string.Format("Satisfaction Survey for Technician {0}", emp.FirstName),
                                                Header = string.Format("Satisfaction Survey for Technician {0}", emp.FirstName),

                                            };
                                            try
                                            {
                                                HSapiFacade.SendSurveyEmail(email);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        ActionDisplyText += " and survey email sent successfully.";
                                    }
                                }
                            }

                        }
                    }
                    HSapiFacade.UpdateTicket(model);
                    var objappointment = HSapiFacade.GetCustomerAppointmentByTicketId(ticketid);
                    if (objappointment != null)
                    {
                        objappointment.AppointmentStartTime = startTime;
                        objappointment.AppointmentEndTime = endTime;
                        objappointment.AppointmentDate = date;
                    }
                    HSapiFacade.UpdateCustomerAppointment(objappointment);
                    var objassignedtikuser = HSapiFacade.GetAssignedTicketUserByTicketId(ticketid);
                    if (objassignedtikuser != null && assigned != Guid.Empty)
                    {
                        objassignedtikuser.UserId = assigned;

                        HSapiFacade.UpdateTicketUser(objassignedtikuser);
                    }

                    if (!string.IsNullOrWhiteSpace(additionalUsers))
                    {
                        HSapiFacade.DeleteAdditionalMembersAndAppointment(ticketid);
                        var spadditionalmembers = additionalUsers.Split(',');
                        if (spadditionalmembers.Length > 0)
                        {
                            foreach (var item in spadditionalmembers)
                            {
                                TicketUser TicketUser = new TicketUser()
                                {
                                    TiketId = ticketid,
                                    UserId = new Guid(item),
                                    IsPrimary = false,
                                    AddedDate = DateTime.UtcNow,
                                    AddedBy = objuser.UserId,
                                    NotificationOnly = false,
                                };
                                HSapiFacade.InsertTicketUser(TicketUser);
                            }
                        }


                    }
                    if (!string.IsNullOrWhiteSpace(notifyingUsers))
                    {
                        HSapiFacade.DeleteNotifyingMembersByTicketId(ticketid);
                        var spanotifyingmembers = notifyingUsers.Split(',');
                        if (spanotifyingmembers.Length > 0)
                        {
                            foreach (var item in spanotifyingmembers)
                            {
                                TicketUser TicketUser = new TicketUser()
                                {
                                    TiketId = ticketid,
                                    UserId = new Guid(item),
                                    IsPrimary = false,
                                    AddedDate = DateTime.UtcNow,
                                    AddedBy = objuser.UserId,
                                    NotificationOnly = true,
                                };
                                HSapiFacade.InsertTicketUser(TicketUser);
                            }
                        }
                    }
                    #region Add Log
                    if (!string.IsNullOrWhiteSpace(ActionDisplyText) && !string.IsNullOrWhiteSpace(Action) && !string.IsNullOrWhiteSpace(UserId))
                        AddUserActivityForCustomer(ActionDisplyText, PageUrl, Action, UserAgent, UserIp, UserName, UserId, CustomerId, "Ticket", ticketid.ToString());
                    #endregion
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<Ticket>.Success(null));

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<Ticket>.Error("Token Expired."));

                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<Ticket>.Error(ex.Message));

            }
        }


        [Authorize]
        [Route("GetAllCustomer")]
        public HttpResponseMessage GetCustomer()
        {
            APIInitialize();
            int PageNo = 0;
            int PageSize = 0;
            HttpRequestHeaders headers = Request.Headers;
            CustomerFilter filter = new CustomerFilter();
            if (headers.Contains("PageNo"))
            {
                int.TryParse(headers.GetValues("PageNo").First(), out PageNo);
                filter.PageNo = PageNo;
            }
            if (headers.Contains("PageSize"))
            {
                int.TryParse(headers.GetValues("PageSize").First(), out PageSize);
                filter.PageSize = PageSize;
            }
            if (headers.Contains("SearchText"))
            {
                filter.SearchText = headers.GetValues("SearchText").First();
            }
            if (headers.Contains("ResultType"))
            {
                string ResultType = headers.GetValues("ResultType").First();
                if (ResultType.ToLower() == "customer")
                {
                    filter.isLead = false;
                }
                else if (ResultType.ToLower() == "lead")
                {
                    filter.isLead = true;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed,
                    new BaseAPIResponse(false, "'ResultType' field is required. Set 'ResultType'='Customer' for the customer list and 'ResultType'='Lead' for the lead list", null));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed,
                    new BaseAPIResponse(false, "'ResultType' field is required. Set 'ResultType'='Customer' for the customer list and 'ResultType'='Lead' for the lead list", null));
            }
            try
            {
                var Userinfo = base.GetUserInfo();
                if (Userinfo == null)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Token Expired.", null));
                }
                CompanyConneciton usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(Userinfo.Username);
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied.", null));

                Guid UserId = Userinfo.Userid;

                #region LOG

                #region Create Directory if not exists
                try
                {
                    string subPath = "~/APILOG"; // your code goes here

                    bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
                    if (!exists)
                        System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
                    #endregion

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\APILOG\GetAllCustomer.txt"), true))
                    {
                        string LOGFile = @"IP ADDRESS: " + base.GetClientIp(Request) + " Username: " + Userinfo.Username + " Date: " + DateTime.Now.ToString("MM/dd/yy hh:mm:ss tt") + " Data: ";
                        LOGFile += " PageNo = " + filter.PageNo + " PageSize  = " + PageSize + " SearchText = " + filter.SearchText;

                        file.WriteLine(LOGFile);
                        file.Close();
                    }
                }
                catch (Exception)
                {

                }
                #endregion

                #region Partner Data
                PermissionGroup objRoleEmployee = HSapiFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(UserId, usercontext.CompanyId);
                if (objRoleEmployee == null)
                {
                    var objemp = HSapiFacade.GetEmployeeByUserName(Userinfo.Username);
                    if (objemp == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "token expired.", null));
                    objRoleEmployee = HSapiFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(objemp.UserId, usercontext.CompanyId);
                }
                filter.EmployeeRole = objRoleEmployee.Tag;
                filter.Partners = HSapiFacade.GetEmployeeByPartnerId(UserId);
                #endregion

                var CustomerList = HSapiFacade.GetCustomerByFilter(filter);

                return Request.CreateResponse(HttpStatusCode.OK, new BaseAPIResponse(true, "Successful", CustomerList));


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, ex.Message, null));
            }
        }
        [Route("Schedule")]
        [HttpGet]
        public HttpResponseMessage Schedule()
        {
            Guid ticketid = new Guid();
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            try
            {
                JobsCustomModel model = new JobsCustomModel();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    model = HSapiFacade.GetJobSchedulesByTicketId(ticketid);

                    if (model != null && model.ScheduleList.Any())
                    {
                        var schedule = model.ScheduleList.First();
                        var ticketStatus = model.TicketList
                                                              .Where(t => t.TicketId == ticketid)
                                                              .Select(t => t.Status)
                                                              .FirstOrDefault();

                        var result = new
                        {
                            date = schedule.AppointmentDate,
                            startTime = schedule.AppointmentStartTime,
                            endTime = schedule.AppointmentEndTime,
                            assignedUser = model.UserList
                            .Where(u => u.IsPrimary)
                            .Select(u => u.UserId)
                            .FirstOrDefault(),
                            additionalUsers = model.UserList
                            .Where(u => !u.IsPrimary && u.NotificationOnly == true)
                            .Select(u => u.UserId)
                            .ToList(),
                            notifyingUsers = model.UserList
                            .Where(u => !u.IsPrimary && u.NotificationOnly == false)
                            .Select(u => u.UserId)
                            .ToList(),
                            status = ticketStatus
                        };


                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResponse<object>(result, true, null));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, ApiResponse<object>.Error("Schedule not found."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }
        [Route("invoices")]
        [HttpGet]
        public HttpResponseMessage GetPaymentInvoices()
        {
            Guid customerid = new Guid();
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("customerid"))
            {
                Guid.TryParse(headers.GetValues("customerid").First(), out customerid);
            }
            try
            {
                List<Invoice> model = new List<Invoice>();
                Customer customer = new Customer();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    customer = HSapiFacade.GetCustomerByCustomerId(customerid);
                    model = HSapiFacade.GetAllInvoicesByCustomerId(usercontext.CompanyId, customerid);
                    var result = model.Select(invoice => new
                    {
                        id = invoice.Id,
                        invoiceId = invoice.InvoiceId,
                        billed = invoice.IsBill,
                        dueDate = invoice.DueDate,
                        due = invoice.BalanceDue,
                        total = invoice.TotalAmount
                    }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(result));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }
        [Route("equipment-advanced")]
        [HttpGet]
        public HttpResponseMessage LoadBadInventoryAndUserAssignPopup()
        {

            int id = 0;
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    CustomerAppointmentEquipment model = HSapiFacade.GetCustomerAppointmentEquipmentById(id);

                    var result = new
                    {
                        exists = model.IsEquipmentExist,
                        nonCommissionable = model.IsNonCommissionable,
                        seller = model.CreatedByUid,
                        installer = model.InstalledByUid
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(result));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }
        [Route("update-equipment-advanced")]
        [HttpPost]
        public HttpResponseMessage ChangeCreatedByUserEqp()
        {

            int id = 0;
            Guid sellerGuid = new Guid();
            Guid installerGuid = new Guid();
            bool exists = false;
            bool nonCommissionable = false;
            bool result = false;
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }
            if (headers.Contains("sellerGuid"))
            {
                Guid.TryParse(headers.GetValues("sellerGuid").First(), out sellerGuid);
            }
            if (headers.Contains("installerGuid"))
            {
                Guid.TryParse(headers.GetValues("installerGuid").First(), out installerGuid);
            }
            if (headers.Contains("exists"))
            {
                bool.TryParse(headers.GetValues("exists").First(), out exists);
            }
            if (headers.Contains("nonCommissionable"))
            {
                bool.TryParse(headers.GetValues("nonCommissionable").First(), out nonCommissionable);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    CustomerAppointmentEquipment model = HSapiFacade.GetCustomerAppointmentEquipmentById(id);

                    if (exists)
                    {
                        model.IsEquipmentExist = true;
                        model.IsEquipmentRelease = true;
                        model.UnitPrice = 0;
                        model.TotalPrice = 0;
                    }
                    else
                    {
                        model.IsEquipmentExist = false;
                    }

                    model.IsNonCommissionable = nonCommissionable;
                    model.CreatedByUid = sellerGuid;
                    model.InstalledByUid = installerGuid;
                    result = HSapiFacade.UpdateCustomerAppoinmentEquipment(model);


                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(result));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }

        [Route("service-advanced")]
        [HttpGet]
        public HttpResponseMessage LoadBadInventoryAndUserAssignServicePopup()
        {

            int id = 0;
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    CustomerAppointmentEquipment model = HSapiFacade.GetCustomerAppointmentEquipmentById(id);

                    var result = new
                    {
                        nonCommissionable = model.IsNonCommissionable,
                        seller = model.CreatedByUid
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(result));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }

        [Route("update-service-advanced")]
        [HttpPost]
        public HttpResponseMessage ChangeCreatedByUserService()
        {

            int id = 0;
            Guid sellerGuid = new Guid();
            bool nonCommissionable = false;
            bool result = false;
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }
            if (headers.Contains("sellerGuid"))
            {
                Guid.TryParse(headers.GetValues("sellerGuid").First(), out sellerGuid);
            }
            if (headers.Contains("nonCommissionable"))
            {
                bool.TryParse(headers.GetValues("nonCommissionable").First(), out nonCommissionable);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    CustomerAppointmentEquipment model = HSapiFacade.GetCustomerAppointmentEquipmentById(id);

                    model.IsNonCommissionable = nonCommissionable;
                    model.CreatedByUid = sellerGuid;
                    result = HSapiFacade.UpdateCustomerAppoinmentEquipment(model);


                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(result));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }
        [Route("emergency-contacts")]
        [HttpGet]
        public HttpResponseMessage EmergencyContactListPartial()
        {

            int id = 0;
            Guid customerid = new Guid();
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }
            if (headers.Contains("customerid"))
            {
                Guid.TryParse(headers.GetValues("customerid").First(), out customerid);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                    var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;
                    APIInitialize();
                    List<EmergencyContact> contacts = HSapiFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(customerid, companyId);

                    var result = contacts.Select(contact => new
                    {
                        id = contact.Id,
                        firstName = contact.FirstName,
                        lastName = contact.LastName,
                        relation = contact.RelationShip,
                        phone = contact.Phone
                    }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(result));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }
        [Route("installed-by-users")]
        [HttpGet]
        public HttpResponseMessage InstalledByUserListPartial()
        {

            int id = 0;
            Guid customerid = new Guid();
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }
            if (headers.Contains("customerid"))
            {
                Guid.TryParse(headers.GetValues("customerid").First(), out customerid);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                    var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;
                    APIInitialize();

                    var technicianList = HSapiFacade.GetEmployeeByCompanyIdAndTag(companyId, LabelHelper.UserTags.Technicians, Guid.Empty);
                    var result = technicianList.Select(t => t.UserId).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<List<Guid>>.Success(result));


                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }
        [Route("sold-by-users")]
        [HttpGet]
        public HttpResponseMessage SoldByUserListPartial()
        {

            int id = 0;
            Guid customerid = new Guid();
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }
            if (headers.Contains("customerid"))
            {
                Guid.TryParse(headers.GetValues("customerid").First(), out customerid);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                    var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;
                    APIInitialize();

                    var soldbyList = HSapiFacade.GetAllEmployeeUserAssign().OrderBy(x => x.FirstName);
                    var result = soldbyList.Select(t => t.UserId).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<List<Guid>>.Success(result));


                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }


        [Route("create-payment")]
        [HttpPost]
        public HttpResponseMessage ReceivePayment([FromBody] ReceivePaymentAPIModel Model)
        {
            string PageUrl = "";
            string Action = "";
            string ActionDisplyText = "";
            string UserIp = "";
            string UserAgent = "";
            string UserName = "";
            string UserId = "";
            string CustomerId = "";
            Guid userGuid = new Guid();
            string rid = "";
            var re = Request;
            var headers = re.Headers;
            #region Log Header
            if (headers.Contains("pageurl"))
            {
                PageUrl = headers.GetValues("pageurl").First();
            }
            if (headers.Contains("action"))
            {
                Action = headers.GetValues("action").First();
            }
            if (headers.Contains("actiondisplytext"))
            {
                ActionDisplyText = headers.GetValues("actiondisplytext").First();
            }
            if (headers.Contains("userip"))
            {
                UserIp = headers.GetValues("userip").First();
            }
            if (headers.Contains("useragent"))
            {
                UserAgent = headers.GetValues("useragent").First();
            }
            if (headers.Contains("username"))
            {
                UserName = headers.GetValues("username").First();
            }
            if (headers.Contains("userid"))
            {
                UserId = headers.GetValues("userid").First();
            }
            if (headers.Contains("customerid"))
            {
                CustomerId = headers.GetValues("customerid").First();
            }
            if (headers.Contains("userGuid"))
            {
                Guid.TryParse(headers.GetValues("userGuid").First(), out userGuid);
            }
            #endregion
            try
            {
                List<Invoice> model = new List<Invoice>();
                Customer customer = new Customer();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    Company com = HSapiFacade.GetCompanyByComapnyId(usercontext.CompanyId);
                    long PaymentInfoId = 0;
                    Model.CompanyId = usercontext.CompanyId;
                    Model.CompanyName = com.CompanyName;
                    string TransactionId = "";
                    string InvoiceList = "";
                    Employee emp = HSapiFacade.GetEmployeeByEmployeeId(userGuid);

                    var objcustomer = HSapiFacade.GetCustomerByCustomerId(Model.customerGuid);
                    if (objcustomer != null)
                        Model.customerId = objcustomer.Id;

                    Model.Description = string.Format("Received by {0}", emp.FirstName + " " + emp.LastName);

                    double AmountPaid = Model.amount;
                    AmountPaid = Math.Round(AmountPaid, 2);

                    #region Customer Credit Check
                    double InvoiceAmount = Model.transactions.Count() == 0 ? 0 : Model.transactions.Sum(x => x.Payment);
                    InvoiceAmount = Math.Round(InvoiceAmount, 2);
                    double CreditAmount = AmountPaid - InvoiceAmount;
                    #endregion

                    Customer CustomerDetails = HSapiFacade.GetCustomerById(Model.customerId);
                    #region Insert Into Transaction Queue
                    string Starttime = DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    string Endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    if (AmountPaid == 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Amount can't be zero.");
                    }
                    List<TransactionQueue> transqueList = new List<TransactionQueue>();
                    transqueList = HSapiFacade.GetTransactionQueueCustomerId(CustomerDetails.CustomerId, Starttime, Endtime, AmountPaid);
                    if (transqueList.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Duplicate transection requested with same amount. Please try after 1 minute.");
                    }
                    else
                    {
                        TransactionQueue transque = new TransactionQueue();
                        transque.CustomerId = CustomerDetails.CustomerId;
                        transque.Amount = AmountPaid;
                        transque.InvoiceId = Model.InvoiceList;
                        transque.CreatedBy = emp.UserId;
                        transque.CreatedDate = DateTime.Now;
                        HSapiFacade.InsertTransactionQueue(transque);
                    }

                    #endregion


                    #region Set InvoiceNo
                    List<Invoice> ListInvoice = new List<Invoice>();
                    foreach (var item in Model.transactions)
                    {
                        Invoice inv = HSapiFacade.GetInvoiceById(item.Id);
                        if (inv.Status == "Paid")
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Transaction unsuccessful. Some of invoice already paid.");

                        }
                        ListInvoice.Add(inv);
                        string balanceduestr = string.Format("{0:#.00}", inv.BalanceDue);
                        string Paymentstr = string.Format("{0:#.00}", inv.BalanceDue);
                        double BalaceDue = 0.0;
                        double Payment = 0.0;
                        double.TryParse(balanceduestr, out BalaceDue);
                        double.TryParse(Paymentstr, out Payment);
                        inv.BalanceDue = BalaceDue - Payment;
                        if (inv.BalanceDue < 0)
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Transaction unsuccessful. Balance can't be less than 0.");
                        }
                        string NewInvStr = InvoiceList + GenerateInvoiceNo(item.Id);
                        if (NewInvStr.Length <= 20)
                        {
                            InvoiceList += GenerateInvoiceNo(item.Id);
                        }
                    }
                    Model.InvoiceList = InvoiceList;
                    #endregion

                    if (Model.transactions.Count() > 0)
                    {
                        #region Transaction Count gt 0

                        List<int> invoiceIdList = Model.transactions.Select(x => x.Id).ToList();
                        string reference = "";
                        foreach (var item in invoiceIdList)
                        {
                            reference += item.GenerateInvoiceNo() + " ";
                        }
                        if (Model.card == null)
                        {
                            Model.card = new cardInfo();
                        }
                        Transaction tr = new Transaction()
                        {
                            Status = "Closed",
                            Type = "Payment",
                            TransacationDate = DateTime.Now.UTCCurrentTime(),
                            CustomerId = Model.customerGuid,
                            CompanyId = Model.CompanyId,
                            Amount = Math.Round(AmountPaid, 2),
                            CardTransactionId = Model.reference,
                            PaymentMethod = Model.paymentMethod,
                            ReferenceNo = !string.IsNullOrWhiteSpace(reference) ? reference : Model.reference,
                            CheckNo = Model.reference,
                            AddedBy = User.Identity.Name,
                            AddedDate = DateTime.Now.UTCCurrentTime(),
                            CreatedBy = emp.UserId,
                            PaymentInfoId = Convert.ToInt32(PaymentInfoId)
                        };
                        tr.Id = HSapiFacade.InsertTransaction(tr);
                        rid = tr.Id.ToString();
                        string PaymentLogMessage = "Payment received for " + reference + " by " + Model.paymentMethod.ToLower();
                        List<TransactionHistory> trhistory = new List<TransactionHistory>();
                        string CreditNote = "";
                        foreach (var item in Model.transactions)
                        {
                            Invoice inv = HSapiFacade.GetInvoiceById(item.Id);
                            if (string.IsNullOrWhiteSpace(CreditNote))
                            {
                                CreditNote = string.Format(@"Invoice# <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{0}')"">{1}</a>", inv.Id, inv.InvoiceId);
                            }
                            else
                            {
                                CreditNote = CreditNote + ",<br />" + string.Format(@"<a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{0}')"">{1}</a>", inv.Id, inv.InvoiceId);
                            }
                            trhistory.Add(new TransactionHistory()
                            {
                                Amout = Math.Round(item.Payment, 2),
                                InvoiceId = item.Id,
                                TransactionId = tr.Id,
                                Balance = Math.Round(inv.BalanceDue.HasValue ? inv.BalanceDue.Value : 0, 2),
                                ReceivedBy = emp.UserId,
                                InvoiceTotal = inv.TotalAmount.HasValue ? inv.TotalAmount.Value : 0,
                                InvoiceBalanceDue = inv.BalanceDue.HasValue ? inv.BalanceDue.Value : 0
                            });
                            double DueBalance = Math.Round(inv.BalanceDue.HasValue ? inv.BalanceDue.Value : 0, 2);
                            inv.BalanceDue = Math.Round(DueBalance - Math.Round(item.Payment, 2), 2);
                            if (item.Payment > 0)
                            {
                                if (inv.BalanceDue == 0)
                                {
                                    inv.Status = LabelHelper.InvoiceStatus.Paid;
                                }
                                else
                                {
                                    inv.Status = LabelHelper.InvoiceStatus.Partial;
                                }
                            }
                            inv.PaymentType = Model.paymentMethod;
                            HSapiFacade.UpdatePaymentInvoice(inv);

                            CustomerSnapshot objInvoicePayment = new CustomerSnapshot()
                            {
                                CustomerId = CustomerDetails.CustomerId,
                                CompanyId = Model.CompanyId,
                                Description = "Invoice " + string.Format("<a onclick=OpenTopToBottomModal('{3}/Invoice/AddInvoice?id={0}&CustomerId={1}') style='cursor: pointer;'>", inv.Id, CustomerDetails.Id, CustomerDetails.CustomerId, Framework.Utils.AppConfig.DomainSitePath) + "<b>" + inv.InvoiceId + "</b>" + "</a>" + " created for " + "<b>" + CustomerDetails.FirstName + " " + CustomerDetails.LastName + "</b>",
                                Logdate = DateTime.Now.UTCCurrentTime(),
                                Updatedby = emp.FirstName + " " + emp.LastName,
                                Type = "InvoicePaymentHistory"
                            };
                            HSapiFacade.InsertSnapshot(objInvoicePayment);
                        }
                        if (trhistory.Count() > 0)
                        {
                            HSapiFacade.InsertTransactionHistoryList(trhistory);
                        }
                        #endregion
                    }
                    #region Add Log
                    if (!string.IsNullOrWhiteSpace(ActionDisplyText) && !string.IsNullOrWhiteSpace(Action) && !string.IsNullOrWhiteSpace(UserId))
                        AddUserActivityForCustomer(ActionDisplyText, PageUrl, Action, UserAgent, UserIp, UserName, UserId, CustomerId, "Invoice", rid);
                    #endregion
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = true });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }
        }
        public static string GenerateInvoiceNo(int InvoiceId)
        {
            string InvoiceFormat = "00000000";
            InvoiceFormat = string.Concat(InvoiceFormat, InvoiceId);
            InvoiceFormat = string.Format("INV{0}", InvoiceFormat.Substring(InvoiceFormat.Length - 8));
            return InvoiceFormat;
        }

        [Route("generate-recreate-document")]
        [HttpPost]
        public HttpResponseMessage UpdateAgreementInfo()
        {

            int id = 0;
            Guid sellerGuid = new Guid();
            Guid customerId = new Guid();
            Guid ticketid = new Guid();

            string contractType = "";
            string contractTerm = "";
            int renewalTerm = 0;
            double activationFee = 0.0;
            double nonConfirmingFee = 0.0;
            double effectiveContractDate = 0.0;
            string verbalPassword = "";
            bool nonCommissionable = false;
            bool result = false;
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            if (headers.Contains("sellerGuid"))
            {
                Guid.TryParse(headers.GetValues("sellerGuid").First(), out sellerGuid);
            }
            if (headers.Contains("customerId"))
            {
                Guid.TryParse(headers.GetValues("customerId").First(), out customerId);
            }

            if (headers.Contains("contractType"))
            {
                contractType = headers.GetValues("contractType").First();
            }
            if (headers.Contains("contractTerm"))
            {
                contractTerm = headers.GetValues("contractTerm").First();
            }
            if (headers.Contains("renewalTerm"))
            {
                int.TryParse(headers.GetValues("renewalTerm").First(), out renewalTerm);
            }
            if (headers.Contains("activationFee"))
            {
                double.TryParse(headers.GetValues("activationFee").First(), out activationFee);
            }
            if (headers.Contains("nonConfirmingFee"))
            {
                double.TryParse(headers.GetValues("nonConfirmingFee").First(), out nonConfirmingFee);
            }
            if (headers.Contains("effectiveContractDate"))
            {
                double.TryParse(headers.GetValues("effectiveContractDate").First(), out effectiveContractDate);
            }
            if (headers.Contains("verbalPassword"))
            {
                verbalPassword = headers.GetValues("verbalPassword").First();
            }
            if (headers.Contains("nonCommissionable"))
            {
                bool.TryParse(headers.GetValues("nonCommissionable").First(), out nonCommissionable);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "userid")?.Value;
                var userId = Guid.TryParse(userIdClaim, out Guid parsedUserId) ? parsedUserId : Guid.Empty;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                    var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;

                    APIInitialize();

                    //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                    Ticket ticket = HSapiFacade.GetTicketByTicketId(ticketid);
                    string message = "";
                    Customer cus = HSapiFacade.GetCustomerByCustomerId(customerId);
                    cus.ContractTeam = contractTerm;
                    cus.Passcode = verbalPassword;
                    cus.ActivationFee = activationFee;
                    cus.RenewalTerm = renewalTerm;
                    //cus.OriginalContactDate = model.OrginalContractDate;
                    DateTime? OG_ContractDate = cus.OriginalContactDate;
                    HSapiFacade.UpdateCustomer(cus);

                    if (cus != null)
                    {
                        var cusExt = HSapiFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                        if (cusExt != null)
                        {
                            cusExt.ContractType = contractType;
                            OG_ContractDate = OG_ContractDate ?? cusExt.ContractStartDate;
                            cusExt.ContractStartDate = DateTime.Now;
                            HSapiFacade.UpdateCustomerExtended(cusExt);
                        }
                    }

                    PackageCustomer pc = new PackageCustomer();
                    pc = HSapiFacade.GetPackageCustomerByCustomerId(cus.CustomerId);
                    if (pc != null)
                    {
                        pc.ActivationFee = activationFee;
                        pc.NonConformingFee = nonConfirmingFee;
                        pc.NonConforming = true;
                        HSapiFacade.UpdatePackageCustomer(pc);
                        result = true;
                    }
                    else
                    {
                        pc = new PackageCustomer()
                        {
                            CustomerId = cus.CustomerId,
                            CompanyId = companyId,
                            PackageId = new Guid(),
                            NonConformingFee = nonConfirmingFee,
                            ActivationFee = activationFee,
                            MinCredit = 0,
                            MaxCredit = 0,
                            NonConforming = true
                        };
                        HSapiFacade.InsertPackageCustomer(pc);
                        result = true;
                    }
                    var smartLeadsResponse = GetSmartLeadsForPopUp(
                              LeadId: cus.Id,
                              grant: false,
                              templateid: null,
                              firstpage: cus.ContractTeam == "firstpage",
                              ticketid: ticket.Id,
                              recreate: true,
                              isinvoice: false,
                              invoiceid: null,
                              isestimator: false,
                              estid: 0,
                              userid: userId,
                              commercial: cus.ContractTeam == "commercial",
                              EstimatorId: ""
                                     );

                    string url = smartLeadsResponse.result.url;

                    // Return the URL in the response
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(new { url = url }));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }

        [Route("generate-addendum-document")]
        [HttpPost]
        public HttpResponseMessage CreateAddendumPdf()
        {

            int id = 0;

            Guid customerId = new Guid();
            Guid ticketid = new Guid();

            bool result = false;
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }

            if (headers.Contains("customerId"))
            {
                Guid.TryParse(headers.GetValues("customerId").First(), out customerId);
            }

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                    var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;

                    APIInitialize();

                    CustomerCompany custommerCompany = new CustomerCompany();
                    Guid CompanyId = new Guid();
                    var AgreementTax = 0.0;

                    Customer cus = HSapiFacade.GetCustomerByCustomerId(customerId);
                    Ticket ticket = HSapiFacade.GetTicketByTicketId(ticketid);
                    List<CustomerAppointmentEquipment> AppoinmentEqpList = HSapiFacade.GetAllCustomerAppointmentEquipListByAppointmentIdAPI(ticketid);
                    List<CustomerAppointmentEquipment> EqpmentList = new List<CustomerAppointmentEquipment>();
                    List<CustomerAppointmentEquipment> ServiceList = new List<CustomerAppointmentEquipment>();

                    if (AppoinmentEqpList != null && AppoinmentEqpList.Count() > 0)
                    {
                        EqpmentList = AppoinmentEqpList.Where(x => x.EquipmentClassId != 2 && x.IsEquipmentExist != true && x.IsEquipmentRelease != true).ToList();
                        ServiceList = AppoinmentEqpList.Where(x => x.EquipmentClassId == 2 && x.IsARBEnabled == true).ToList();
                    }
                    Company com = new Company();
                    CustomerAddendumModel cusAddendum = new CustomerAddendumModel();

                    if (cus != null && ticket != null)
                    {
                        if (User.Identity.IsAuthenticated)
                        {

                            CompanyId = companyId;
                        }
                        else
                        {
                            custommerCompany = HSapiFacade.GetCustomerCompanyByCustomerId(cus.Id);
                            CompanyId = custommerCompany.CompanyId;
                        }
                        com = HSapiFacade.GetCompanyByComapnyId(CompanyId);
                        var objcusaddendum = HSapiFacade.GetCustomerAddendumByCustomerIdAndTicketId(ticket.TicketId, cus.CustomerId);
                        GlobalSetting glbs = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "CompanySignature");

                        #region Tax

                        var GetSalesTax = HSapiFacade.GetSalesTax(com.CompanyId, cus.CustomerId);
                        if (GetSalesTax != null)
                        {
                            AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                        }
                        //}
                        CustomerSignature _cusSign = HSapiFacade.GetCustomerSignatureByCustomerId(cus.CustomerId);
                        if (_cusSign != null)
                        {
                            cus.CustomerSignatureDate = _cusSign.CreatedDate;
                        }
                        if (cus.CustomerSignatureDate == null)
                        {
                            Invoice _inv = HSapiFacade.GetInvoiceByCustomerIdAndStatus(cus.CustomerId, LabelHelper.EstimateStatus.Signed);
                            if (_inv != null)
                                cus.CustomerSignatureDate = _inv.SignatureDate.HasValue ? _inv.SignatureDate : new DateTime();
                        }
                        #endregion

                        cusAddendum = new CustomerAddendumModel()
                        {
                            CompanyLogo = HSapiFacade.GetCompanyLogoForPDFByCompanyId(com.CompanyId),
                            KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG"),
                            KazarLogoIcon = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo_icon.png"),
                            CompanyPhone = com.Phone,
                            CompanyId = com.CompanyId,
                            CompanyAddress = com.Address,
                            CompanyCity = com.City,
                            CompanyState = com.City,
                            CompanyZip = com.ZipCode,
                            FirstName = cus.FirstName,
                            LastName = cus.LastName,
                            EmailAddress = cus.EmailAddress,
                            CellPhone = cus.CellNo,
                            SitePhone = cus.PrimaryPhone,
                            AgreementSignDate = cus.CustomerSignatureDate.HasValue ? cus.CustomerSignatureDate.Value : new DateTime(),
                            AddendumCreateDate = DateTime.Now,
                            CustomerStreet = cus.Street,
                            CustomerCity = cus.City,
                            Tax = AgreementTax,
                            CustomerState = cus.State,
                            CustomerZip = cus.ZipCode,
                            InstallAddress = AddressHelper.MakeAddress(cus),
                            SalesRepresentative = !string.IsNullOrEmpty(cus.Soldby) ? HSapiFacade.GetEmployeeNumByEmployeeId(new Guid(cus.Soldby)).ToString() : "",
                            TicketId = ticket.Id,
                            WorkToBePerformed = ticket.WorkToBePerformed,
                            RecurringAmount = cus.MonthlyMonitoringFee,
                            ScheduleOn = ticket.CompletionDate,
                            CustomerId = cus.CustomerId,
                            TicketGuidId = ticket.TicketId,
                            CustomerAddendumSignature = objcusaddendum != null ? objcusaddendum.Signature : ""
                            //CustomerAddendumSignatureDate = objcusaddendum != null ? objcusaddendum.CreatedDate : DateTime.Now.UTCCurrentTime()
                        };
                        if (objcusaddendum != null && !string.IsNullOrWhiteSpace(objcusaddendum.Signature))
                        {
                            if (objcusaddendum.CreatedDate != null && objcusaddendum.CreatedDate != new DateTime())
                            {
                                cusAddendum.CustomerAddendumStringSignatureDate = objcusaddendum.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                            }
                            if (glbs != null && !string.IsNullOrWhiteSpace(glbs.Value))
                            {
                                cusAddendum.CompanySignature = glbs.Value;
                                if (objcusaddendum.CreatedDate != null && objcusaddendum.CreatedDate != new DateTime())
                                {
                                    cusAddendum.CompanySignatureDate = objcusaddendum.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                                }
                            }
                        }
                        cusAddendum.ServiceEqpList = ServiceList;
                        cusAddendum.EquipmentList = EqpmentList;
                        cusAddendum.CurrentCurrency = HSapiFacade.GetCurrentCurrencyByCompanyId(com.CompanyId);
                    }
                    string body = HSapiFacade.MakeCustomerAddendumPdf(cusAddendum);

                    var doc = new HtmlToPdfDocument()
                    {
                        GlobalSettings = {
                            ColorMode = ColorMode.Color,
                            Orientation = Orientation.Portrait,
                            PaperSize = PaperKind.A4,
                        },
                        Objects = {
                            new ObjectSettings() {
                                HtmlContent = body,
                            }
                        }
                    };

                    byte[] pdf = null;
                    try
                    {
                        pdf = Converter.Convert(doc);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error during PDF generation", ex);
                    }


                    #region File Save on AWS S3

                    Random rand = new Random();
                    string filename = ConfigurationManager.AppSettings["File.LeadToCustomerAgreement"];
                    filename = filename.TrimEnd('/');

                    var comname = HSapiFacade.GetCompanyByComapnyId(com.CompanyId).CompanyName.ReplaceSpecialChar();
                    string FilePath = string.Format(filename, comname);
                    string FileName = rand.Next().ToString() + customerId + "AgreementMail.pdf";
                    string FileKey = string.Format($"{FilePath}/{FileName}");

                    var task = Task.Run(async () =>
                    {
                        AWSS3ObjectService AWSobject = new AWSS3ObjectService();


                        await AWSobject.UploadFile(FileKey, pdf);


                        await AWSobject.MakePublic(FileName, FilePath);
                    });

                    task.Wait();

                    string returnUrl = string.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName) + FileKey;

                    doc = null;
                    pdf = null;

                    GC.Collect();

                    #endregion

                    //  return ApiResponse<SmartLeadsResponse>.Success(new SmartLeadsResponse { url = returnUrl });

                    // Return the URL in the response
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(new { url = returnUrl }));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }

        [Route("send-document")]
        [HttpPost]
        public HttpResponseMessage SendDocumentAPI()
        {
            int id = 0;
            Guid sellerGuid = new Guid();
            Guid customerId = new Guid();
            Guid ticketid = new Guid();
            string contractType = "";
            string contractTerm = "";
            string phone = "";
            string email = "";
            int renewalTerm = 0;
            double activationFee = 0.0;
            double nonConfirmingFee = 0.0;
            double effectiveContractDate = 0.0;
            string verbalPassword = "";
            bool nonCommissionable = false;
            bool recreate = false;
            bool addnum = false;
            bool firstpage = false;
            bool result = false;
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            if (headers.Contains("sellerGuid"))
            {
                Guid.TryParse(headers.GetValues("sellerGuid").First(), out sellerGuid);
            }
            if (headers.Contains("customerId"))
            {
                Guid.TryParse(headers.GetValues("customerId").First(), out customerId);
            }

            if (headers.Contains("contractType"))
            {
                contractType = headers.GetValues("contractType").First();
            }
            if (headers.Contains("contractTerm"))
            {
                contractTerm = headers.GetValues("contractTerm").First();
            }
            if (headers.Contains("phone"))
            {
                phone = headers.GetValues("phone").First();
            }
            if (headers.Contains("email"))
            {
                email = headers.GetValues("email").First();
            }
            if (headers.Contains("renewalTerm"))
            {
                int.TryParse(headers.GetValues("renewalTerm").First(), out renewalTerm);
            }
            if (headers.Contains("activationFee"))
            {
                double.TryParse(headers.GetValues("activationFee").First(), out activationFee);
            }
            if (headers.Contains("nonConfirmingFee"))
            {
                double.TryParse(headers.GetValues("nonConfirmingFee").First(), out nonConfirmingFee);
            }
            if (headers.Contains("effectiveContractDate"))
            {
                double.TryParse(headers.GetValues("effectiveContractDate").First(), out effectiveContractDate);
            }
            if (headers.Contains("verbalPassword"))
            {
                verbalPassword = headers.GetValues("verbalPassword").First();
            }
            if (headers.Contains("nonCommissionable"))
            {
                bool.TryParse(headers.GetValues("nonCommissionable").First(), out nonCommissionable);
            }
            if (headers.Contains("recreate"))
            {
                bool.TryParse(headers.GetValues("recreate").First(), out recreate);
            }
            if (headers.Contains("addnum"))
            {
                bool.TryParse(headers.GetValues("addnum").First(), out addnum);

            }
            if (headers.Contains("firstpage"))
            {
                bool.TryParse(headers.GetValues("firstpage").First(), out firstpage);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "userid")?.Value;
                var userId = Guid.TryParse(userIdClaim, out Guid parsedUserId) ? parsedUserId : Guid.Empty;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                    var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;

                    APIInitialize();

                    //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                    Ticket ticket = HSapiFacade.GetTicketByTicketId(ticketid);
                    string message = "";
                    Customer cus = HSapiFacade.GetCustomerByCustomerId(customerId);
                    cus.ContractTeam = contractTerm;
                    cus.Passcode = verbalPassword;
                    cus.ActivationFee = activationFee;
                    cus.RenewalTerm = renewalTerm;
                    //cus.OriginalContactDate = model.OrginalContractDate;
                    DateTime? OG_ContractDate = cus.OriginalContactDate;
                    HSapiFacade.UpdateCustomer(cus);

                    if (cus != null)
                    {
                        var cusExt = HSapiFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                        if (cusExt != null)
                        {
                            cusExt.ContractType = contractType;
                            OG_ContractDate = OG_ContractDate ?? cusExt.ContractStartDate;
                            cusExt.ContractStartDate = DateTime.Now;
                            HSapiFacade.UpdateCustomerExtended(cusExt);
                        }
                    }

                    PackageCustomer pc = new PackageCustomer();
                    pc = HSapiFacade.GetPackageCustomerByCustomerId(cus.CustomerId);
                    if (pc != null)
                    {
                        pc.ActivationFee = activationFee;
                        pc.NonConformingFee = nonConfirmingFee;
                        pc.NonConforming = true;
                        HSapiFacade.UpdatePackageCustomer(pc);
                        result = true;
                    }
                    else
                    {
                        pc = new PackageCustomer()
                        {
                            CustomerId = cus.CustomerId,
                            CompanyId = companyId,
                            PackageId = new Guid(),
                            NonConformingFee = nonConfirmingFee,
                            ActivationFee = activationFee,
                            MinCredit = 0,
                            MaxCredit = 0,
                            NonConforming = true
                        };
                        HSapiFacade.InsertPackageCustomer(pc);
                        result = true;
                    }
                    if (addnum)
                    {
                        bool isEmailSent = !string.IsNullOrEmpty(email);
                        bool isSmsSent = !string.IsNullOrEmpty(phone);
                        var smartLeadsResponse = SentEmailSMSAddendum(
                            CustomerId: customerId,
                            TicketId: ticketid,
                            SentEmail: isEmailSent,
                            SentSms: isSmsSent,
                            email: email,
                            phone: phone

                        );
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(phone))
                        {
                            var smartLeadsResponse = SMSAgreementLinkForPrintBlank(
                                leadid: cus.Id,
                                PrefferedNO: phone,
                                IsRecreate: recreate,
                                agreementtempid: null,
                                isinvoice: false,
                                invoiceid: null,
                                isestimator: false,
                                estid: 0,
                                userid: userId,
                                commercial: false,
                                firstpage: firstpage
                            );
                        }

                        if (!string.IsNullOrEmpty(email))
                        {
                            var smartLeadsResponseEmail = SmartLeadConvertedToCustomerPDFMail(
                                leadid: cus.Id,
                                PrefferedEmail: email,
                                IsRecreate: recreate,
                                agreementtempid: null,
                                firstpage: firstpage,
                                ticketid: ticket.Id,
                                recreate: recreate,
                                isinvoice: false,
                                invoiceid: null,
                                isestimator: false,
                                estid: 0,
                                userid: userId,
                                commercial: false,
                                EstimatorId: ""
                            );
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(true));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }

        #region
        public HttpResponseMessage SentEmailSMSAddendum(Guid CustomerId, Guid TicketId, bool? SentEmail, bool? SentSms, string email, string phone)
        {
            string FilePath = "";
            string FileKey = "";
            bool isUploaded = false;
            string tempFolderName = "";
            string FileName = "";
            var returnurl = "";
            byte[] data;
            decimal _fileSize = 1.00m;

            bool result = false;
            string message = "";
            var AgreementTax = 0.0;
            var identity = (ClaimsIdentity)User.Identity;
            var userIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "userid")?.Value;
            var userId = Guid.TryParse(userIdClaim, out Guid parsedUserId) ? parsedUserId : Guid.Empty;
            var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
            if (usercontext == null)
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
            ComId = usercontext.CompanyId.ToString();
            var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
            var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;
            string currentUser = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();

            Customer cus = HSapiFacade.GetCustomerByCustomerId(CustomerId);
            Ticket ticket = HSapiFacade.GetTicketByTicketId(TicketId);
            List<CustomerAppointmentEquipment> AppoinmentEqpList = HSapiFacade.GetAllCustomerAppointmentEquipListByAppointmentIdAPI(TicketId);
            List<CustomerAppointmentEquipment> EqpmentList = new List<CustomerAppointmentEquipment>();
            List<CustomerAppointmentEquipment> ServiceList = new List<CustomerAppointmentEquipment>();
            string file = "";
            if (AppoinmentEqpList != null && AppoinmentEqpList.Count() > 0)
            {
                EqpmentList = AppoinmentEqpList.Where(x => x.EquipmentClassId != 2 && x.IsEquipmentExist != true && x.IsEquipmentRelease != true).ToList();
                ServiceList = AppoinmentEqpList.Where(x => x.EquipmentClassId == 2).ToList();
            }
            Company com = HSapiFacade.GetCompanyByComapnyId(companyId);
            CustomerAddendumModel cusAddendum = new CustomerAddendumModel();
            List<string> ValidPrefferedEmail = new List<string>();
            if (!string.IsNullOrWhiteSpace(email))
            {
                string[] Emailadd = email.Split(';');
                if (Emailadd != null)
                {
                    foreach (var item in Emailadd)
                    {
                        if (item.IsValidEmailAddress())
                        {
                            ValidPrefferedEmail.Add(item);
                        }
                    }
                }
                if (ValidPrefferedEmail.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Invalid email address."));

                }

            }
            if (cus != null && ticket != null)
            {
                var objcusaddendum = HSapiFacade.GetCustomerAddendumByCustomerIdAndTicketId(ticket.TicketId, cus.CustomerId);
                GlobalSetting glbs = HSapiFacade.GetGlobalSettingsByKey(companyId, "CompanySignature");

                #region Tax
                //var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(cus.CustomerId, com.CompanyId);
                //if (GetCityTaxList.Count > 0)
                //{
                //    foreach (var item1 in GetCityTaxList)
                //    {
                //        AgreementTax = item1.Rate;
                //    }
                //}
                //else
                //{
                //    Guid CustomerId = new Guid();
                //    if (cus != null)
                //    {
                //        CustomerId = cus.CustomerId;
                //    }
                var GetSalesTax = HSapiFacade.GetSalesTax(com.CompanyId, cus.CustomerId);
                if (GetSalesTax != null)
                {
                    AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                }
                //}
                CustomerSignature _cusSign = HSapiFacade.GetCustomerSignatureByCustomerId(cus.CustomerId);
                if (_cusSign != null)
                {
                    cus.CustomerSignatureDate = _cusSign.CreatedDate;
                }
                if (cus.CustomerSignatureDate == null)
                {
                    Invoice _inv = HSapiFacade.GetInvoiceByCustomerIdAndStatus(cus.CustomerId, LabelHelper.EstimateStatus.Signed);
                    if (_inv != null)
                        cus.CustomerSignatureDate = _inv.SignatureDate.HasValue ? _inv.SignatureDate : new DateTime();
                }
                #endregion

                cusAddendum = new CustomerAddendumModel()
                {
                    CompanyLogo = HSapiFacade.GetCompanyLogoForPDFByCompanyId(com.CompanyId),
                    KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG"),
                    KazarLogoIcon = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo_icon.png"),
                    CompanyPhone = com.Phone,
                    CompanyId = com.CompanyId,
                    CompanyAddress = com.Address,
                    CompanyCity = com.City,
                    CompanyState = com.City,
                    CompanyZip = com.ZipCode,
                    FirstName = cus.FirstName,
                    LastName = cus.LastName,
                    EmailAddress = cus.EmailAddress,
                    CellPhone = cus.CellNo,
                    SitePhone = cus.PrimaryPhone,
                    AgreementSignDate = cus.CustomerSignatureDate.HasValue ? cus.CustomerSignatureDate.Value : new DateTime(),
                    AddendumCreateDate = DateTime.Now,
                    CustomerStreet = cus.Street,
                    CustomerCity = cus.City,
                    CustomerState = cus.State,
                    CustomerZip = cus.ZipCode,
                    Tax = AgreementTax,
                    InstallAddress = AddressHelper.MakeAddress(cus),
                    TicketId = ticket.Id,
                    SalesRepresentative = !string.IsNullOrEmpty(cus.Soldby) ? HSapiFacade.GetEmployeeNumByEmployeeId(new Guid(cus.Soldby)).ToString() : "",
                    WorkToBePerformed = ticket.WorkToBePerformed,
                    RecurringAmount = cus.MonthlyMonitoringFee,
                    ScheduleOn = ticket.CompletionDate,
                    CustomerId = cus.CustomerId,
                    TicketGuidId = ticket.TicketId,
                    CustomerAddendumSignature = objcusaddendum != null ? objcusaddendum.Signature : "",
                    //CustomerAddendumSignatureDate = objcusaddendum != null ? objcusaddendum.CreatedDate : DateTime.Now.UTCCurrentTime()
                };
                if (objcusaddendum != null && !string.IsNullOrWhiteSpace(objcusaddendum.Signature))
                {
                    if (objcusaddendum.CreatedDate != null && objcusaddendum.CreatedDate != new DateTime())
                    {
                        cusAddendum.CustomerAddendumStringSignatureDate = objcusaddendum.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                    }
                    if (glbs != null && !string.IsNullOrWhiteSpace(glbs.Value))
                    {
                        cusAddendum.CompanySignature = glbs.Value;
                        if (objcusaddendum.CreatedDate != null && objcusaddendum.CreatedDate != new DateTime())
                        {
                            cusAddendum.CompanySignatureDate = objcusaddendum.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                }
                cusAddendum.ServiceEqpList = ServiceList;
                cusAddendum.EquipmentList = EqpmentList;
                cusAddendum.CurrentCurrency = HSapiFacade.GetCurrentCurrencyByCompanyId(com.CompanyId);
                string body = HSapiFacade.MakeCustomerAddendumPdf(cusAddendum);
                // ViewBag.BodyContent = body;
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("CreateAddendumPdf")
                {
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };

                #region File Save Old

                //byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                //Random rand = new Random();
                //string filename = ConfigurationManager.AppSettings["File.Ticket"];
                //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                //var pdftempFolderName = string.Format(filename, comname) + ticket.Id + "_AddendumDocument.pdf";
                //string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
                //FileHelper.SaveFile(applicationPDFData, Serverfilename);

                #endregion

                #region File Save on AWS S3
                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    },
                    Objects = {
            new ObjectSettings() {
                HtmlContent = body,
            }
        }
                };

                byte[] pdf = null;
                try
                {
                    pdf = Converter.Convert(doc);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error during PDF generation", ex);
                }
                var comname = HSapiFacade.GetCompanyByComapnyId(companyId).CompanyName.ReplaceSpecialChar();
                tempFolderName = ConfigurationManager.AppSettings["File.Ticket"];

                var pdfTempFold = string.Format(tempFolderName, comname);
                tempFolderName = string.Format(tempFolderName, comname);
                tempFolderName = tempFolderName.TrimEnd('/');

                FileName = ticket.Id + "_AddendumDocument.pdf";


                FilePath = tempFolderName;
                FileKey = string.Format($"{FilePath}/{FileName}");

                var task = Task.Run(async () =>
                {
                    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                    await AWSobject.UploadFile(FileKey, pdf);
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
                //  Thread.Sleep(5000);

                /// "mayur" used thread for async s3 methods : End


                returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
                returnurl = returnurl + FileKey;


                isUploaded = true;

                _fileSize = (decimal)pdf.Length / 1024;
                _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);


                #endregion


                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(ticket.Id + "#" + cus.Id + "#" + companyId.ToString());
                string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Ticket/", encryptedurl);

                ShortUrl ShortUrl = HSapiFacade.GetSortUrlByUrl(fullurl, cus.CustomerId);
                string shortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code);


                if (SentEmail.HasValue && SentEmail.Value)
                {
                    if (ValidPrefferedEmail.Count == 0)
                    {
                        email = cus.EmailAddress;
                    }
                    else
                    {
                        email = string.Join(";", ValidPrefferedEmail);
                    }
                    if (ValidPrefferedEmail.Count > 0 || (!string.IsNullOrWhiteSpace(cus.EmailAddress) && cus.EmailAddress.IsValidEmailAddress()))
                    {
                        CustomerTicketAddendumEmail CustomerTicketAddendumEmail = new CustomerTicketAddendumEmail
                        {
                            CustomerName = cus.FirstName + " " + cus.LastName,
                            ToEmail = email,
                            BodyLink = shortUrl
                        };
                        result = HSapiFacade.EmailOnlyCustomerTicketAddendumDocument(CustomerTicketAddendumEmail, companyId);
                        if (result)
                        {
                            message += "Addendum sent to " + email;

                            LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                            {
                                CompanyId = companyId,
                                CustomerId = cus.CustomerId,
                                Type = "Email",
                                ToEmail = email,
                                BodyContent = "Addendum Document",
                                SentDate = DateTime.Now.UTCCurrentTime(),
                                LastUpdatedDate = DateTime.Now,
                                SentBy = userId,
                                Subject = "Addendum Document"
                            };
                            HSapiFacade.InsertCorrespondence(LeadCorrespondence);

                            #region file save to customer file
                            if (SentSms == null || (SentSms.HasValue && SentSms.Value == false))
                            {
                                file = "Customer_Addendum";
                                CustomerFile cfs = new CustomerFile()
                                {
                                    FileDescription = cus.Id + "_" + Regex.Replace(file, @"\s+", String.Empty) + "_Mail" + ".pdf",
                                    Filename = "/" + FileKey,
                                    FileId = Guid.NewGuid(),
                                    FileSize = (double)_fileSize,
                                    FileFullName = Regex.Replace(file, @"\s+", String.Empty) + ".pdf",
                                    Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                    CustomerId = cus.CustomerId,
                                    CompanyId = companyId,
                                    IsActive = true,
                                    CreatedBy = userId,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    UpdatedBy = userId,
                                    UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                    WMStatus = LabelHelper.WatermarkStatus.Pending,
                                    AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                                };
                                HSapiFacade.InsertCustomerFile(cfs);
                                string logMessage = Regex.Replace(file, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                                //  base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cus.Id, null);
                            }

                            #endregion
                        }

                    }
                    else
                    {
                        message = "Invalid email address.";
                    }
                }
                if (SentSms.HasValue && SentSms.Value)
                {
                    string ReceiverNumber = "";
                    List<string> ReceiverNumberList = new List<string>();
                    if (!string.IsNullOrWhiteSpace(phone))
                    {
                        ReceiverNumber = phone.Replace("-", "");
                    }
                    else if (!string.IsNullOrWhiteSpace(cus.PrimaryPhone))
                    {
                        ReceiverNumber = cus.PrimaryPhone.Replace("-", "");
                    }
                    else if (!string.IsNullOrWhiteSpace(cus.SecondaryPhone))
                    {
                        ReceiverNumber = cus.SecondaryPhone.Replace("-", "");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(message + " and no phone number available."));

                    }
                    ReceiverNumberList.Add(ReceiverNumber);
                    SMSAddendum SMSAddendum = new SMSAddendum();

                    SMSAddendum.ShortUrl = shortUrl;
                    SMSAddendum.ToNumber = ReceiverNumberList;
                    string phonenumber = string.Join(";", ReceiverNumberList);
                    if (HSapiFacade.SendAddendumSMS(SMSAddendum, userId, companyId, ReceiverNumberList, false, currentUser) == true)
                    {
                        result = true;
                        LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                        {
                            CompanyId = companyId,
                            CustomerId = cus.CustomerId,
                            Type = "SMS",
                            ToMobileNo = phonenumber,
                            BodyContent = "Addendum Document",
                            SentDate = DateTime.Now.UTCCurrentTime(),
                            LastUpdatedDate = DateTime.Now,
                            SentBy = userId
                        };
                        HSapiFacade.InsertCorrespondence(LeadCorrespondence);
                        #region file save to customer file
                        string pdfLastExt = "_SMS";
                        if (SentEmail.HasValue && SentEmail.Value)
                        {
                            pdfLastExt = "_Mail_SMS";
                        }
                        file = "Customer_Addendum";
                        CustomerFile cfs = new CustomerFile()
                        {
                            FileDescription = cus.Id + "_" + Regex.Replace(file, @"\s+", String.Empty) + pdfLastExt + ".pdf",
                            Filename = "/" + FileKey,
                            FileSize = (double)_fileSize,
                            FileId = Guid.NewGuid(),
                            FileFullName = Regex.Replace(file, @"\s+", String.Empty) + ".pdf",
                            Uploadeddate = DateTime.Now.UTCCurrentTime(),
                            CustomerId = cus.CustomerId,
                            CompanyId = companyId,
                            IsActive = true,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            UpdatedBy = userId,
                            UpdatedDate = DateTime.Now.UTCCurrentTime(),
                            WMStatus = LabelHelper.WatermarkStatus.Pending,
                            AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                        };
                        HSapiFacade.InsertCustomerFile(cfs);
                        string logMessage = Regex.Replace(file, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                        //base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cus.Id, null);

                        #endregion
                        message += "Addendum Sent to" + phonenumber;
                    }
                    else
                    {
                        result = false;

                        message += "Addendum Sending failed to" + phonenumber;

                    }
                }
                cus.Singature = "";
                HSapiFacade.UpdateCustomer(cus);
                doc = null;
                pdf = null;

                GC.Collect();
            }

            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Success(true));
        }

        public HttpResponseMessage SmartLeadConvertedToCustomerPDFMail(int? leadid, string PrefferedEmail, bool? IsRecreate, int? agreementtempid, bool? firstpage, int? ticketid, bool? recreate, bool? isinvoice, string invoiceid, bool? isestimator, int? estid, Guid? userid, bool? commercial, string EstimatorId)
        {
            //string EstimatorId = "";
            estid = 0;
            decimal _fileSize = 1.00m;
            WebClient webClient;
            byte[] fileBytes1;
            string Temp_FileName;

            List<string> ValidPrefferedEmail = new List<string>();
            if (!string.IsNullOrWhiteSpace(PrefferedEmail))
            {
                string[] Emailadd = PrefferedEmail.Split(';');
                if (Emailadd != null)
                {
                    foreach (var item in Emailadd)
                    {
                        if (item.IsValidEmailAddress())
                        {
                            ValidPrefferedEmail.Add(item);
                        }
                    }
                }
                if (ValidPrefferedEmail.Count == 0)
                {

                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Invalid email address."));

                }

            }
            string from = "";
            string file = "";
            DateTime FixDate = DateTime.Now.UTCCurrentTime();
            //int idlead = Convert.ToInt32(Lid);
            InstallationAgreementModel Model = new InstallationAgreementModel();
            Model.EmergencyContactList = new List<EmergencyContact>();
            Model.ListAgreementAnswer = new List<AgreementAnswer>();
            Model.EquipmentList = new List<Equipment>();
            Model.CustomerAgreement = new List<CustomerAgreement>();
            Model.SingleCustomerAgreement = new CustomerAgreement();
            var taxtotal = 0.0;

            var identity = (ClaimsIdentity)User.Identity;
            var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
            if (usercontext == null)
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
            ComId = usercontext.CompanyId.ToString();
            var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
            var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;
            string currentUser = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();

            bool AgreementResult = false;
            //var ActivationfeeValue = 0.0;
            //var IsActivationFee = _Util.Facade.ActivationFeeFacade.GetActivationFeeByCompanyId(CurrentUser.CompanyId.Value);
            //if (IsActivationFee != null)
            //{
            //    ActivationfeeValue = IsActivationFee.Fee;
            //}
            Customer Cus = new Customer();
            CustomerExtended CusExd = new CustomerExtended();
            Company Com = new Company();
            if (leadid.HasValue)
            {
                if (!HSapiFacade.CustomerIsInCompany(leadid.Value, companyId))
                {
                    return null;
                }
                Cus = HSapiFacade.GetCustomersById(leadid.Value);
                if (Cus != null)
                {
                    CusExd = HSapiFacade.GetCustomerExtendedByCustomerId(Cus.CustomerId);
                }
                Com = HSapiFacade.GetCompanyByComapnyId(companyId);

                string ContractTerm = "";
                string ContractTermInWord = "";
                if (!string.IsNullOrWhiteSpace(Cus.ContractTeam) && Cus.ContractTeam != "-1")
                {
                    if (Cus.ContractTeam.ToLower() == "month to month")
                    {
                        ContractTerm = Cus.ContractTeam;
                        ContractTermInWord = Cus.ContractTeam;
                    }
                    else
                    {
                        ContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))).ToString();
                        ContractTermInWord = NumberToWords((Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))));
                    }

                }

                #region Sold by change
                //Person who sends the mail to Customer will be counted as sold by
                //Cus.Soldby = CurrentUser.UserId.ToString();
                HSapiFacade.UpdateCustomer(Cus);
                #endregion

                var UpfrontAddOnTotal = 0.0;
                var UpfrontAddOnTotalPromo = 0.0;
                bool IsUpfrontPromo = false;
                bool IsServicePromo = false;
                var MonthlyServiceFeeTotal = 0.0;
                var TotalMonthlyMonitoring = 0.0;
                var NewSubTotal = 0.0;
                var TotalDueAtSigning = 0.0;
                var EquipmentTotalPrice = 0.0;
                var ServiceTotalPrice = 0.0;
                var AgreementSubtotal = 0.0;
                var AgreementTotal = 0.0;
                double ServiceTax = 0.0;
                var AgreementTax = 0.0;
                var NotARBEnabledTotalPrice = 0.0;
                string InstallTypeName = "";
                bool IsNonConfirming = false;
                var NonConfirmingFee = 0.0;
                var AdvanceServiceFeeTotal = 0.0;
                string contractCreatedDateVal = "";
                if (CusExd.ContractCreatedDate != null)
                {
                    contractCreatedDateVal = CusExd.ContractCreatedDate.Value.UTCToClientTime().ToString("M/d/yy");
                }
                else
                {
                    CusExd.ContractCreatedDate = DateTime.UtcNow;
                    HSapiFacade.UpdateCustomerExtended(CusExd);
                    contractCreatedDateVal = CusExd.ContractCreatedDate.Value.UTCToClientTime().ToString("M/d/yy");
                }
                if (Cus.CreditScoreValue == null)
                {
                    Cus.CreditScoreValue = 0;
                }
                var PackageCustomer = HSapiFacade.GetPackageCustomerByCustomerId(Cus.CustomerId);
                //if (PackageCustomer != null && PackageCustomer.NonConforming && PackageCustomer.NonConformingFee > 0 && (Cus.CreditScoreValue < PackageCustomer.MinCredit || Cus.CreditScoreValue > PackageCustomer.MaxCredit))
                if (PackageCustomer != null && PackageCustomer.NonConformingFee > 0 && (Cus.CreditScoreValue <= PackageCustomer.MinCredit || Cus.CreditScoreValue >= PackageCustomer.MaxCredit))
                {
                    IsNonConfirming = true;
                    NonConfirmingFee = PackageCustomer.NonConformingFee.Value;
                }
                var SmartPackageEquipmentServiceList = new List<SmartPackageEquipmentService>();
                var GetCityTaxList = HSapiFacade.GetCityTaxRate(Cus.CustomerId, Com.CompanyId);
                if (GetCityTaxList.Count > 0)
                {
                    foreach (var item1 in GetCityTaxList)
                    {
                        AgreementTax = item1.Rate;
                        ServiceTax = item1.Rate;
                    }
                }
                else
                {
                    Guid CustomerId = new Guid();
                    if (Cus != null)
                    {
                        CustomerId = Cus.CustomerId;
                    }
                    var GetSalesTax = HSapiFacade.GetSalesTax(Com.CompanyId, CustomerId);
                    if (GetSalesTax != null)
                    {
                        AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                        ServiceTax = Convert.ToDouble(GetSalesTax.Value);
                    }
                }
                #region Invoice
                Invoice invoice = new Invoice();
                CustomerProratedBill cpb = new CustomerProratedBill();
                cpb = HSapiFacade.GetCusProratedBillByCustomerId(Cus.CustomerId);
                List<InvoiceDetail> invoiceList = new List<InvoiceDetail>();
                string invDiagram = "";
                if (!string.IsNullOrWhiteSpace(invoiceid) && isinvoice == true)
                {
                    invoice = HSapiFacade.GetByInvoiceId(invoiceid);
                    if (invoice != null)
                    {
                        invDiagram = invoice.InvoiceContractDiagram;
                        invoiceList = HSapiFacade.GetInvoiceDetialsListByInvoiceId(invoice.InvoiceId);
                    }
                }
                #endregion
                #region Estimator
                Employee userInfo = new Employee();
                if (userid != Guid.Empty)
                {
                    userInfo = HSapiFacade.GetEmployeeByEmployeeId(userid.Value);
                }
                CreateEstimator createest = new CreateEstimator();
                if (isestimator.Value && estid > 0)
                {
                    CreateEstimator ca = new CreateEstimator();
                    ca.EstimatorSetting = new EstimatorSetting();
                    ca.Company = Com;

                    ca.Estimator = HSapiFacade.GetById(estid.Value);
                    ca._EstimatorPDFFilter = HSapiFacade.GetEstimatorPdfFilterByComIdCusIdUserId(Com.CompanyId, userid.Value, ca.Estimator.CustomerId);
                    ca.estimatorDetails = HSapiFacade.GetEstimatorDetailListByEstimatorId(ca.Estimator.EstimatorId);
                    ca.estimatorServices = HSapiFacade.GetEstimatorServicesByEstimatorId(ca.Estimator.EstimatorId);
                    if (ca.Estimator == null || ca.Estimator.CompanyId != Com.CompanyId)
                    {
                        return null;
                    }
                    if ((ca.estimatorDetails == null || ca.estimatorDetails.Count() == 0) && (ca.estimatorServices == null || ca.estimatorServices.Count() == 0))
                    {
                        return null;
                    }
                    Customer tempCUstomer = HSapiFacade.GetCustomerByCustomerId(ca.Estimator.CustomerId);
                    if (tempCUstomer == null)
                    {
                        return null;
                    }

                    CreateEstimator processedModel = GetEstimatorModelById(ca.Estimator, ca.estimatorDetails, ca.estimatorServices, Com, tempCUstomer, ca._EstimatorPDFFilter, Com.CompanyId);
                    Estimator estimator = HSapiFacade.GetEstimatorByEstimatorId(ca.Estimator.EstimatorId);
                    if (estimator != null)
                    {
                        // ViewBag.CoverLetter = estimator.CoverLetter;
                        processedModel.Estimator.CoverLetter = estimator.CoverLetter;
                        processedModel.Estimator.CoverLetterFile = estimator.CoverLetterFile;
                        processedModel.Estimator.ServicePlanType = estimator.ServicePlanType;
                        processedModel.Estimator.ServicePlanRate = estimator.ServicePlanRate;
                        processedModel.Estimator.ServicePlanAmount = estimator.ServicePlanAmount;
                        processedModel.Estimator.ServiceTaxAmount = estimator.ServiceTaxAmount;
                        processedModel.Estimator.ServiceTotalAmount = estimator.ServiceTotalAmount;
                        processedModel.Estimator.ShowServicePlan = estimator.ShowServicePlan;
                        processedModel.Estimator.ShowService = estimator.ShowService;
                        processedModel.Estimator.ServicePlanTypeName = "Service Plan";

                        //SelectListItem selectListItem = HSapiFacade.GetDropdownsByKey("ServicePlans").Where(x => x.Value == estimator.ServicePlanType).FirstOrDefault();
                        //if (selectListItem != null)
                        //{
                        //    processedModel.Estimator.ServicePlanTypeName = selectListItem.Text;
                        //}

                    }
                    if (ca.estimatorDetails != null)
                    {
                        foreach (var item in ca.estimatorDetails)
                        {
                            Manufacturer Manufacturer = HSapiFacade.GetManufacturerByManufacturerId(item.ManufacturerId);
                            if (Manufacturer != null)
                            {
                                item.Manufacturer = Manufacturer.Name;
                            }
                            item.CreatedDate = DateTime.Now.UTCCurrentTime();
                            ca.SubTotal = ca.SubTotal + (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
                            item.EquipmentFile = HSapiFacade.GetEquipmentFilesByEquipmentIdAndFileType(item.EquipmentId, LabelHelper.EquipmentFileType.ProfilePicture).FirstOrDefault();
                            if (item.EquipmentFile == null)
                            {
                                item.EquipmentFile = new EquipmentFile();
                            }
                        }
                    }
                    if (ca.estimatorServices != null)
                    {
                        foreach (var item in ca.estimatorServices)
                        {
                            processedModel.ServiceSubTotal += ca.ServiceSubTotal + item.Amount;
                        }
                        processedModel.TotalServiceAmount = processedModel.ServiceSubTotal + ca.ServiceTax;
                    }
                    createest = processedModel;
                    createest.eSecurityLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/eSecurity_logo.png");
                    createest.specializedLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/specialized_property_management_logo.png");
                    string EstimatorContractTerm = "";
                    if (!string.IsNullOrWhiteSpace(createest.Estimator.ContractTerm) && createest.Estimator.ContractTerm != "-1")
                    {
                        if (createest.Estimator.ContractTerm.ToLower() == "month to month")
                        {
                            EstimatorContractTerm = createest.Estimator.ContractTerm;
                        }
                        else
                        {
                            EstimatorContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(createest.Estimator.ContractTerm) * 12))).ToString() + " month";
                        }

                    }
                    createest.EstimatorContractTerm = EstimatorContractTerm;
                }
                #endregion
                var CustomEquipmentList = new List<Equipment>();
                if (firstpage == true || recreate == true)
                {
                    CustomEquipmentList = HSapiFacade.GetSmartEquipmentListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (CustomEquipmentList.Count > 0)
                    {
                        foreach (var item in CustomEquipmentList)
                        {
                            EquipmentTotalPrice += item.Total;
                            UpfrontAddOnTotal += item.Total;
                        }
                    }
                }
                else
                {
                    CustomEquipmentList = HSapiFacade.GetSmartEquipmentListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (CustomEquipmentList.Count > 0)
                    {
                        foreach (var item in CustomEquipmentList)
                        {
                            EquipmentTotalPrice += item.Total;
                            UpfrontAddOnTotal += item.Total;
                        }
                    }
                }
                var CustomServiceList = new List<Equipment>();
                if (firstpage == true || recreate == true)
                {
                    CustomServiceList = HSapiFacade.GetSmartServiceListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (CustomServiceList.Count > 0)
                    {
                        foreach (var item in CustomServiceList)
                        {
                            EquipmentTotalPrice += item.Total;
                            ServiceTotalPrice += item.Total;
                            MonthlyServiceFeeTotal += item.Total;
                        }
                    }
                }
                else
                {
                    CustomServiceList = HSapiFacade.GetSmartServiceListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (CustomServiceList.Count > 0)
                    {
                        foreach (var item in CustomServiceList)
                        {
                            EquipmentTotalPrice += item.Total;
                            ServiceTotalPrice += item.Total;
                            MonthlyServiceFeeTotal += item.Total;
                        }
                    }
                }
                var NotARBEnabledServiceList = new List<Equipment>();
                if (firstpage == true || recreate == true || commercial == true)
                {
                    NotARBEnabledServiceList = HSapiFacade.GetNotARBEnabledSmartServiceListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (NotARBEnabledServiceList.Count > 0)
                    {
                        foreach (var item in NotARBEnabledServiceList)
                        {
                            NotARBEnabledTotalPrice += item.Total;
                        }
                    }
                }
                else
                {
                    NotARBEnabledServiceList = HSapiFacade.GetNotARBEnabledSmartServiceListFromService(Cus.CustomerId, Com.CompanyId);
                    if (NotARBEnabledServiceList.Count > 0)
                    {
                        foreach (var item in NotARBEnabledServiceList)
                        {
                            NotARBEnabledTotalPrice += item.Total;
                        }
                    }
                }
                #region Advance Monitoring Service Month

                PaymentInfoCustomer paycus = new PaymentInfoCustomer();
                paycus = HSapiFacade.GetPaymentInfoCustomerByCustomerIdAndPayForService(Cus.CustomerId);
                int ForMonth = 1;
                if (paycus != null && paycus.ForMonths.HasValue)
                {
                    ForMonth = paycus.ForMonths.Value;
                }
                if (ForMonth > 1)
                {
                    AdvanceServiceFeeTotal = MonthlyServiceFeeTotal * (ForMonth - 1);

                }
                #endregion
                Cus.MonthlyMonitoringFee = Convert.ToString(ServiceTotalPrice);
                TotalMonthlyMonitoring = MonthlyServiceFeeTotal;
                NewSubTotal = TotalMonthlyMonitoring + UpfrontAddOnTotal;
                if (CustomServiceList.Count > 0 || CustomEquipmentList.Count > 0)
                {
                    if (PackageCustomer != null && PackageCustomer.ActivationFee.HasValue)
                    {
                        AgreementSubtotal = PackageCustomer.ActivationFee.Value + EquipmentTotalPrice;
                        NewSubTotal = PackageCustomer.ActivationFee.Value + EquipmentTotalPrice;
                    }
                    else
                    {
                        AgreementSubtotal = EquipmentTotalPrice;
                        NewSubTotal = EquipmentTotalPrice;
                    }
                }
                if (IsNonConfirming && NonConfirmingFee > 0)
                {
                    AgreementSubtotal = AgreementSubtotal + NonConfirmingFee;
                    NewSubTotal = NewSubTotal + NonConfirmingFee;
                }
                if (AgreementTax != 0.0)
                {
                    taxtotal = (AgreementSubtotal / 100) * AgreementTax;
                    Model.TaxTotal = taxtotal;
                    AgreementTotal = AgreementSubtotal + taxtotal;
                    TotalDueAtSigning = NewSubTotal + taxtotal;
                }
                else
                {
                    Model.TaxTotal = 0.0;
                    AgreementTotal = AgreementSubtotal;
                    TotalDueAtSigning = NewSubTotal;
                }
                /// "mayur" Discount change in activation
                if (ServiceTax != 0.0)
                {
                    taxtotal = (ServiceTotalPrice / 100) * ServiceTax;
                    //MonthlyServiceFeeTotal = ServiceTotalPrice + taxtotal;
                }
                else
                {
                }

                var PackageCustomerDetails = HSapiFacade.GetPackageCustomerByCustomerIdandCompanyId(Cus.CustomerId, companyId);
                if (PackageCustomerDetails != null)
                {
                    InstallTypeName = HSapiFacade.SmartInstallTypeNameByInstallTypeId(Convert.ToInt32(PackageCustomerDetails.SmartInstallTypeId));
                    SmartPackageEquipmentServiceList = HSapiFacade.GetAllSmartPackageIncludeEquipmentByPackageIdAndCompanyId(PackageCustomerDetails.PackageId, companyId);
                }
                var PaymentDetails = HSapiFacade.GetAllPaymentInfoByCustomerIdAndCompanyId(Cus.CustomerId, companyId).Where(m => m.PayFor == "First Month").FirstOrDefault();
                var agreementPayment = HSapiFacade.GetLeadAgreementPaymentInfoByCustomerId(Cus.CustomerId);
                string paymentoverviewheader = "";
                string paymentoverviewdata = "";
                if (agreementPayment != null && agreementPayment.Count > 0)
                {
                    paymentoverviewheader = "<table style='border-collapse:collapse; width:100%; font-family:Arial; table-layout:fixed; font-size:13px;'>{0}</table>";
                    foreach (var pay in agreementPayment)
                    {
                        var sppay = pay.Type.Split('_');
                        if (sppay.Length > 0)
                        {
                            if (sppay[0] == "CC")
                            {
                                var cardNumber = pay.CardNumber.Replace('-', ' ').Replace(" ", "");
                                if (cardNumber.Length == 16)
                                {
                                    paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Card Number: " + string.Concat("".PadLeft(12, '*'), cardNumber.Substring(cardNumber.Length - 4)) + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Expire Date: " + pay.CardExpireDate + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Security Code: " + string.Concat("".PadLeft(2, '*'), pay.CardSecurityCode.Substring(pay.CardSecurityCode.Length - 1)) + @"</td>
                                                        </tr>";
                                }
                                else
                                {
                                    paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Card Number: " + string.Concat("".PadLeft(11, '*'), cardNumber.Substring(cardNumber.Length - 4)) + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Expire Date: " + pay.CardExpireDate + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Security Code: " + string.Concat("".PadLeft(2, '*'), pay.CardSecurityCode.Substring(pay.CardSecurityCode.Length - 1)) + @"</td>
                                                        </tr>";
                                }
                            }
                            else if (sppay[0] == "ACH")
                            {
                                paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Type: " + pay.BankAccountType + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Routing No: " + pay.RoutingNo + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account No: " + string.Concat("".PadLeft(pay.AcountNo.Length - 4, '*'), pay.AcountNo.Substring(pay.AcountNo.Length - 4)) + @"</td>
                                                        </tr>";
                            }
                        }
                    }
                }
                var CustomerAddress = AddressHelper.MakeAddress(Cus);
                var CustomerInstallAddress = AddressHelper.MakeInstallAddress(Cus);
                CustomerSignature cs = new CustomerSignature();
                GlobalSetting glbs = HSapiFacade.GetGlobalSettingsByKey(companyId, "CompanySignature");
                string cusSignature = "";
                string cussignDate = "";
                DateTime cussignDateVal = new DateTime();
                if (firstpage.HasValue && firstpage.Value == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "First Page");
                }
                else if (commercial.HasValue && commercial.Value == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Commercial");
                }
                else if (isinvoice.HasValue && isinvoice.Value == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Estimate");
                }
                else if (isestimator.HasValue && isestimator.Value == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Estimator");
                }
                else if (recreate.HasValue && recreate.Value == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Recreate");
                }
                else if (Cus != null && agreementtempid.HasValue && agreementtempid.Value > 0)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Agreement File");
                }
                else
                {
                    cusSignature = Cus.Singature;
                    if (Cus.CustomerSignatureDate != null && Cus.CustomerSignatureDate != new DateTime())
                    {
                        cussignDate = Cus.CustomerSignatureDate.Value.UTCToClientTime().ToString("M/dd/yy");
                        cussignDateVal = Cus.CustomerSignatureDate.Value.UTCToClientTime();
                    }

                }
                if (cs != null && (agreementtempid != 0 || (firstpage.HasValue && firstpage.Value == true) || (recreate.HasValue && recreate.Value == true) || (commercial.HasValue && commercial.Value == true)))
                {
                    cusSignature = cs.Signature;
                    if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                    {
                        cussignDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        cussignDateVal = cs.CreatedDate.UTCToClientTime();
                    }

                }
                //(!string.IsNullOrWhiteSpace(Cus.MonthlyMonitoringFee) ? Convert.ToDouble(Cus.MonthlyMonitoringFee) : 1) * Convert.ToDouble(ContractTerm),

                Double MMR = 1;
                Double CTerm = 0;

                double.TryParse(Cus.MonthlyMonitoringFee, out MMR);
                double.TryParse(ContractTerm, out CTerm);
                //(!string.IsNullOrWhiteSpace() ? Convert.ToDouble(Cus.MonthlyMonitoringFee) : 1) * Convert.ToDouble(ContractTerm);
                Double TotalPayments = MMR * CTerm;
                #region For Promo Pyment Method
                List<PaymentInfoCustomer> paycusList = new List<PaymentInfoCustomer>();
                PaymentProfileCustomer paymentProfile = new PaymentProfileCustomer();
                paycusList = HSapiFacade.GetAllPaymentInfoCustomerByCustomerId(Cus.CustomerId);
                if (paycusList != null && paycusList.Count > 0)
                {
                    foreach (var item in paycusList)
                    {
                        paymentProfile = HSapiFacade.GetPaymentProfileByPaymentInfoId(item.PaymentInfoId);
                        if (paymentProfile != null && paymentProfile.Type == LabelHelper.PaymentMethod.Promo)
                        {
                            if (item.Payfor == "Activation Fee")
                            {
                                NonConfirmingFee = 0.0;

                                if (PackageCustomer != null)
                                {
                                    PackageCustomer.ActivationFee = 0.0;
                                }
                                NewSubTotal = NewSubTotal - (PackageCustomer.AdditionFee + NonConfirmingFee);
                            }
                            else if (item.Payfor == "Equipment")
                            {
                                IsUpfrontPromo = true;
                                NewSubTotal = NewSubTotal - UpfrontAddOnTotal;
                            }
                            else if (item.Payfor == "Service")
                            {
                                NewSubTotal = NewSubTotal - TotalMonthlyMonitoring;
                                IsServicePromo = true;

                            }


                        }

                    }

                }
                #endregion

                #region LeadSource
                string LeadSource = "";
                Lookup leadsource = HSapiFacade.GetLookupByKeyAndValueAndCompanyId("LeadSource", Cus.LeadSource, Com.CompanyId);
                LeadSource = leadsource != null && !string.IsNullOrWhiteSpace(leadsource.DisplayText) && leadsource.DataValue != "-1" ? leadsource.DisplayText : "";
                #endregion
                #region Insert Customer Agreement
                CustomerAgreement objCustomerAgreement = new CustomerAgreement()
                {
                    CompanyId = Com.CompanyId,
                    CustomerId = Cus.CustomerId,
                    IP = AppConfig.GetIP,
                    UserAgent = AppConfig.GetUserAgent,
                    Type = LabelHelper.CustomerAgreementLog.LoadAgreement,
                    AddedDate = DateTime.UtcNow
                };
                HSapiFacade.InsertCustomerAgreement(objCustomerAgreement);
                #endregion
                Model = new InstallationAgreementModel()
                {
                    CSIDNumber = Cus.Id,
                    LeadSource = LeadSource,
                    IsNonConfirming = IsNonConfirming,
                    NonConfirmingFee = NonConfirmingFee,
                    InstallDate = Cus.InstallDate != null ? Convert.ToDateTime(Cus.InstallDate).ToShortDateString() : "",
                    OriginalContactDate = (Cus.OriginalContactDate != null && Cus.OriginalContactDate.HasValue) ? Cus.OriginalContactDate.Value : new DateTime(),
                    AccountType = Cus.Type,
                    ContractType = CusExd != null && !string.IsNullOrWhiteSpace(CusExd.ContractType) ? CusExd.ContractType : "",
                    Referredby = Cus.ReferringCustomer != Guid.Empty ? HSapiFacade.GetCustomerNameById(Cus.ReferringCustomer) : "",
                    SocialSecurityNumber = Cus.SSN,
                    //Owner2ndPhone = Cus.SecondaryPhone,
                    Owner2ndPhone = Cus.PrimaryPhone,
                    InitialStreet = Cus.Street,
                    InitialCity = Cus.City,
                    InitialCountry = Cus.County,
                    InitialState = Cus.State,
                    InitialZip = Cus.ZipCode,
                    InitialApt = Cus.Appartment,
                    BillingCity = Cus.CityPrevious,
                    BillingState = Cus.StatePrevious,
                    BillingZip = Cus.ZipCodePrevious,
                    BillingCountry = Cus.CountryPrevious,
                    BillingStreet = Cus.StreetPrevious,
                    InstallTypeName = InstallTypeName,
                    SmartPackageEquipmentServiceList = SmartPackageEquipmentServiceList,
                    UpfrontAddOnTotal = UpfrontAddOnTotal,
                    UpfrontAddOnTotalPromo = UpfrontAddOnTotalPromo,
                    IsUpfrontPromo = IsUpfrontPromo,
                    IsServicePromo = IsServicePromo,
                    MonthlyServiceFeeTotal = MonthlyServiceFeeTotal,
                    TotalMonthlyMonitoring = TotalMonthlyMonitoring,
                    NewSubTotal = NewSubTotal,
                    TotalDueAtSigning = TotalDueAtSigning,
                    PaymentDetails = PaymentDetails != null ? PaymentDetails : new PaymentInfo(),
                    DisplayName = Cus.DisplayName,
                    BillingAddress = CustomerAddress,
                    OwnerAddress = CustomerAddress,
                    InstallAddress = CustomerInstallAddress,
                    OwnerEmail = Cus.EmailAddress,
                    //OwnerPhone = Cus.PrimaryPhone,
                    OwnerPhone = Cus.CellNo,
                    OwnerName = Cus.FirstName + " " + Cus.LastName,
                    FirstName = Cus.FirstName,
                    LastName = Cus.LastName,
                    EmergencyContactList = HSapiFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId),
                    CompanyName = Com.CompanyName,
                    CompanySate = string.Format("{0}, {1} {2}", Com.City, Com.State, Com.ZipCode),
                    CompanyStreet = Com.Street,
                    CompanyWebsite = Com.Website,
                    SubscribedMonths = ContractTerm,
                    SubscribedMonthsInWord = ContractTermInWord,
                    RenewalMonths = Cus.RenewalTerm.HasValue ? Cus.RenewalTerm.Value : 0,
                    Password = Cus.Passcode,
                    DateOfTransaction = FixDate.UTCToClientTime(),
                    CompanyLogo = HSapiFacade.GetCompanyEmailLogoByCompanyId(Com.CompanyId),
                    EquipmentList = CustomEquipmentList.ToList(),
                    ServiceList = CustomServiceList.ToList(),
                    ActivationFee = (PackageCustomer != null && PackageCustomer.ActivationFee.HasValue) ? PackageCustomer.ActivationFee.Value : 0,
                    BusinessName = Cus.BusinessName,
                    MonthlyMonitoringFee = Cus.MonthlyMonitoringFee,
                    EffectiveDate = FixDate.UTCToClientTime(),
                    CustomerSignature = cusSignature,
                    CustomerSignatureStringDate = cussignDate,
                    CustomerSignatureStringDateVal = cussignDateVal,
                    //ContractCreatedDateVal = contractCreatedDateVal,
                    ContractCreatedDateVal = (CusExd.ContractStartDate != null && CusExd.ContractStartDate.HasValue) ? CusExd.ContractStartDate.Value.ToShortDateString() : DateTime.Now.ToShortDateString(),
                    CustomerAgreement = HSapiFacade.GetCustomerAgreementByCompanyIdAndCustomerId1(Com.CompanyId, Cus.CustomerId),
                    Subtotal = AgreementSubtotal,
                    Tax = AgreementTax,
                    TaxTotal = taxtotal,
                    Total = AgreementTotal,
                    EContractId = Cus.Id,
                    ListAgreementAnswer = HSapiFacade.GetAllAgreementAnswerByCustomerId(Cus.CustomerId),
                    SalesRepresentative = !string.IsNullOrEmpty(Cus.Soldby) ? HSapiFacade.GetEmployeeNumByEmployeeId(new Guid(Cus.Soldby)).ToString() : "",
                    TotalPayments = TotalPayments,
                    SingleCustomerAgreement = HSapiFacade.GetIpAndUserAgentByCustomerIdAndCompanyId(Com.CompanyId, Cus.CustomerId),
                    ListContactEmergency = string.Format(paymentoverviewheader, paymentoverviewdata),
                    ListPaymentInfo = string.Format(paymentoverviewheader, paymentoverviewdata),
                    DoingBusinessAs = Cus.DBA,
                    DispalyName = Cus.DisplayName,
                    CompanyPhone = Com.Phone,
                    FirstPage = firstpage.HasValue ? firstpage.Value : false,
                    Commercial = commercial.HasValue ? commercial.Value : false,
                    IsInvoice = isinvoice.Value,
                    InvoiceId = invoiceid,
                    InvoiceDiagram = invDiagram,
                    InvoiceList = invoiceList,
                    IsEstimator = isestimator.HasValue ? isestimator.Value : false,
                    createEst = createest,
                    userInfo = userInfo,
                    inv = invoice,
                    NotARBEnabledServiceList = NotARBEnabledServiceList.ToList(),
                    NotARBEnabledTotalPrice = NotARBEnabledTotalPrice,
                    ProratedAmout = cpb != null ? Math.Round(cpb.Amount, 2, MidpointRounding.AwayFromZero) : 0.0,
                    FinancedAmout = Cus != null && Cus.FinancedAmount != null ? Math.Round(Cus.FinancedAmount.Value, 2, MidpointRounding.AwayFromZero) : 0.0,
                    MonthlyFinanceRate = CusExd != null && CusExd.MonthlyFinanceRate != null ? Math.Round(CusExd.MonthlyFinanceRate.Value, 2, MidpointRounding.AwayFromZero) : 0.0,
                    AdvanceServiceFeeTotal = AdvanceServiceFeeTotal
                };
                if (agreementtempid != 0)
                {
                    if (cs != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(cs.Signature))
                    {
                        Model.CompanySignature = glbs.Value;
                        if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                        {
                            Model.CompanySignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                }
                else if (firstpage == true || recreate == true || commercial == true)
                {
                    if (glbs != null && !string.IsNullOrWhiteSpace(glbs.Value))
                    {
                        Model.CompanySignature = glbs.Value;
                        Model.CompanySignatureDate = cussignDate;
                    }
                }
                else
                {
                    if (Cus != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(Cus.Singature))
                    {
                        Model.CompanySignature = glbs.Value;
                        if (Cus.CustomerSignatureDate != null && Cus.CustomerSignatureDate != new DateTime())
                        {
                            Model.CompanySignatureDate = Cus.CustomerSignatureDate.Value.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                }

                if (Model.EmergencyContactList == null)
                {
                    Model.EmergencyContactList = new List<EmergencyContact>();
                }
            }
            else
            {
                Model.CompanyLogo = HSapiFacade.GetCompanyLogoForPDFByCompanyId(companyId);
            }

            //  return View(Model);
            Model.KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG");
            // ViewBag.CompanyId = Com.CompanyId.ToString();
            Model.CompanyId = Com.CompanyId.ToString();
            Model.CurrentCurrency = HSapiFacade.GetCurrentCurrencyByCompanyId(Com.CompanyId);

            string body = HSapiFacade.MakeSmartAgreementPdf(Model, agreementtempid.HasValue ? agreementtempid.Value : 0);
            //ViewBag.Body = body;
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("SmartInstallationAgreement")
            {
                //FileName = "TestView.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };


            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                },
                Objects = {
                    new ObjectSettings() {
                        HtmlContent = body,
                    }
                }
            };

            byte[] pdf = null;
            try
            {
                pdf = Converter.Convert(doc);
            }
            catch (Exception ex)
            {
                throw new Exception("Error during PDF generation", ex);
            }

            #region File Save

            //Random rand = new Random();
            //string filename = ConfigurationManager.AppSettings["File.LeadToCustomerAgreement"];
            //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            //var pdftempFolderName = string.Format(filename, comname) + rand.Next().ToString() + leadid + "AgreementMail.pdf";
            //string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
            //FileHelper.SaveFile(applicationPDFData, Serverfilename);

            #endregion

            //// "mayur" AWS S3 Changes //// Start

            #region File Save on AWS S3

            string returnurl = "";
            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.LeadToCustomerAgreement"];
            filename = filename.TrimEnd('/');

            string comname = HSapiFacade.GetCompanyByComapnyId(companyId).CompanyName.ReplaceSpecialChar();

            string FilePath = string.Format(filename, comname);

            String FileName = rand.Next().ToString() + leadid + "AgreementMail.pdf";

            string FileKey = string.Format($"{FilePath}/{FileName}");

            /// "mayur" used thread for async s3 methods : start
            /// 

            var task = Task.Run(async () =>
            {
                AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                await AWSobject.UploadFile(FileKey, pdf);
                await AWSobject.MakePublic(FileName, FilePath);
            });

            task.Wait();

            //Thread thread = new Thread(async () => {

            //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

            //    await AWSobject.UploadFile(FileKey, applicationPDFData);
            //    await AWSobject.MakePublic(FileName, FilePath);

            //});
            //thread.Start();
            //Thread.Sleep(5000);
            /// "mayur" used thread for async s3 methods : End



            returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
            returnurl = returnurl + FileKey;

            //ViewBag.ReturnUrl = returnurl;
            //ViewBag.FileName = FileName;
            //ViewBag.FileKey = FileKey;

            #endregion

            //// "mayur" AWS S3 Changes //// End

            //// ""Mayur" Calculate File Size : start
            #region Calculate file size

            _fileSize = pdf.Length / 1024;

            #endregion
            //// ""Mayur" Calculate File Size : End

            //var cusinfo = _Util.Facade.CustomerFacade.GetById(leadid.Value);
            bool result = false;
            //string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(leadid + "#" + Cus.EmailAddress + "#" + CurrentUser.CompanyId.Value.ToString() + "#" + (IsRecreate.HasValue ? IsRecreate.Value : false) + "#" + (agreementtempid.HasValue ? agreementtempid.Value : 0) + "#" + (firstpage.HasValue ? firstpage.Value : false) + "#" + (ticketid.HasValue ? ticketid.Value : 0) + "#" + (isinvoice.HasValue ? isinvoice.Value : false) + "#" + (!string.IsNullOrWhiteSpace(invoiceid) ? invoiceid : "") + "#" + (isestimator.HasValue ? isestimator.Value : false) + "#" + (estid.HasValue ? estid.Value : 0) + "#" + (userid != Guid.Empty ? userid : new Guid()) + "#" + (commercial.HasValue ? commercial.Value : false) + "#" + (!string.IsNullOrWhiteSpace(EstimatorId) ? EstimatorId : ""));
            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(leadid + "#" + Cus.EmailAddress + "#" + companyId + "#" + (IsRecreate.HasValue ? IsRecreate.Value : false) + "#" + (agreementtempid.HasValue ? agreementtempid.Value : 0) + "#" + (firstpage.HasValue ? firstpage.Value : false) + "#" + (ticketid.HasValue ? ticketid.Value : 0) + "#" + (isinvoice.HasValue ? isinvoice.Value : false) + "#" + (!string.IsNullOrWhiteSpace(invoiceid) ? invoiceid : "") + "#" + (isestimator.HasValue ? isestimator.Value : false) + "#" + (estid.HasValue ? estid.Value : 0) + "#" + (userid != Guid.Empty ? userid : new Guid()) + "#" + (commercial.HasValue ? commercial.Value : false) + "#" + (!string.IsNullOrWhiteSpace(EstimatorId) ? EstimatorId : ""));
            //string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Agreement/", encryptedurl);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/Public/LeadsAgreementDocument/?code=", encryptedurl);

            ShortUrl ShortUrl = HSapiFacade.GetSortUrlByUrl(fullurl, Cus.CustomerId);
            string shortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code);
            string message = "";


            /// Mayur :: File Download to temp folder :start

            //webClient = new WebClient();
            //fileBytes1 = webClient.DownloadData(returnurl);

            //File(fileBytes1, System.Net.Mime.MediaTypeNames.Application.Octet, FileName).ToString();



            //Temp_FileName = Server.MapPath("~/EmailFileCache/tmp_" + FileName);

            //if (!System.IO.File.Exists(Temp_FileName))
            //{
            //    System.IO.File.WriteAllBytes(Temp_FileName, fileBytes1);
            //}
            //else
            //{
            //    System.IO.File.WriteAllBytes(Temp_FileName, fileBytes1);
            //}

            /// Mayur :: File Download to temp folder :End

            if (Cus != null)
            {

                string EmailAddress = PrefferedEmail;
                if (ValidPrefferedEmail.Count == 0)
                {
                    EmailAddress = Cus.EmailAddress;
                }
                else
                {
                    EmailAddress = string.Join(";", ValidPrefferedEmail);
                }
                if (ValidPrefferedEmail.Count > 0 || (!string.IsNullOrWhiteSpace(Cus.EmailAddress) && Cus.EmailAddress.IsValidEmailAddress()))
                {
                    result = true;
                    //// ""Mayur" Calculate File Size : start
                    #region Calculate file size


                    _fileSize = (decimal)pdf.Length / 1024;
                    _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);

                    #endregion
                    //// ""Mayur" Calculate File Size : End
                    ///
                    LeadsAggrement la = new LeadsAggrement
                    {
                        CustomerNum = Cus.DisplayName,
                        ToEmail = EmailAddress,
                        //LeadsAggrementpdf = new Attachment(Temp_FileName, MediaTypeNames.Application.Octet),
                        BodyLink = shortUrl,
                        CustomerId = Cus.CustomerId.ToString(),
                        EmployeeId = userid.ToString()
                    };
                    if (IsRecreate == true)
                    {
                        from = "Recreate Agreement";
                        file = "Recreate_Agreement";
                        AgreementResult = HSapiFacade.EmailOnlyLeadsAggrement(la, companyId, from);
                        #region file save to customer file
                        if (result)
                        {
                            CustomerFile cfs = new CustomerFile()
                            {
                                FileDescription = Cus.Id + "_" + Regex.Replace(file, @"\s+", String.Empty) + "_Mail" + ".pdf",
                                Filename = "/" + FileKey,
                                FileSize = (double)_fileSize,
                                FileId = Guid.NewGuid(),
                                FileFullName = Regex.Replace(file, @"\s+", String.Empty) + ".pdf",
                                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                CustomerId = Cus.CustomerId,
                                CompanyId = companyId,
                                IsActive = true,
                                CreatedBy = userid ?? Guid.Empty,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                UpdatedBy = userid ?? Guid.Empty,
                                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                WMStatus = LabelHelper.WatermarkStatus.Pending,
                                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                            };
                            HSapiFacade.InsertCustomerFile(cfs);
                            string logMessage = Regex.Replace(file, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                            //  base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, Cus.Id, null);
                        }
                        #endregion
                    }
                    else if (!string.IsNullOrWhiteSpace(EstimatorId))
                    {
                        from = "Estimator Agreement";
                        file = "Estimator_Agreement";
                        AgreementResult = HSapiFacade.EmailOnlyLeadsAggrement(la, companyId, from);
                        #region file save to customer file
                        if (result)
                        {
                            CustomerFile cfs = new CustomerFile()
                            {
                                FileDescription = Cus.Id + "_" + Regex.Replace(file, @"\s+", String.Empty) + "_Mail" + ".pdf",
                                Filename = "/" + FileKey,
                                FileSize = (double)_fileSize,
                                FileId = Guid.NewGuid(),
                                FileFullName = Regex.Replace(file, @"\s+", String.Empty) + ".pdf",
                                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                CustomerId = Cus.CustomerId,
                                CompanyId = companyId,
                                IsActive = true,
                                CreatedBy = userid ?? Guid.Empty,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                UpdatedBy = userid ?? Guid.Empty,
                                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                WMStatus = LabelHelper.WatermarkStatus.Pending,
                                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                            };
                            HSapiFacade.InsertCustomerFile(cfs);
                            string logMessage = Regex.Replace(file, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                            // base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, Cus.Id, null);
                        }
                        #endregion
                    }
                    else if (firstpage == true)
                    {
                        from = "First Page Agreement";
                        file = "FirstPage_Agreement";

                        AgreementResult = HSapiFacade.EmailOnlyLeadsAggrement(la, companyId, from);
                        #region file save to customer file
                        if (result)
                        {
                            CustomerFile cfs = new CustomerFile()
                            {
                                FileDescription = Cus.Id + "_" + Regex.Replace(file, @"\s+", String.Empty) + "_Mail" + ".pdf",
                                Filename = "/" + FileKey,
                                FileSize = (double)_fileSize,
                                FileId = Guid.NewGuid(),
                                FileFullName = Regex.Replace(file, @"\s+", String.Empty) + ".pdf",
                                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                CustomerId = Cus.CustomerId,
                                CompanyId = companyId,
                                IsActive = true,
                                CreatedBy = userid ?? Guid.Empty,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                UpdatedBy = userid ?? Guid.Empty,
                                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                WMStatus = LabelHelper.WatermarkStatus.Pending,
                                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                            };
                            HSapiFacade.InsertCustomerFile(cfs);
                            string logMessage = Regex.Replace(file, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                            // base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, Cus.Id, null);
                        }
                        #endregion
                    }
                    else if (CusExd.ContractType == "Commercial")
                    {
                        from = "Commercial Agreement";
                        file = "Commercial_Agreement";

                        AgreementResult = HSapiFacade.EmailOnlyLeadsAggrement(la, companyId, from);
                        #region file save to customer file
                        if (result)
                        {
                            CustomerFile cfs = new CustomerFile()
                            {
                                FileDescription = Cus.Id + "_" + Regex.Replace(file, @"\s+", String.Empty) + "_Mail" + ".pdf",
                                Filename = "/" + FileKey,
                                FileSize = (double)_fileSize,
                                FileId = Guid.NewGuid(),
                                FileFullName = Regex.Replace(file, @"\s+", String.Empty) + ".pdf",
                                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                CustomerId = Cus.CustomerId,
                                CompanyId = companyId,
                                IsActive = true,
                                CreatedBy = userid ?? Guid.Empty,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                UpdatedBy = userid ?? Guid.Empty,
                                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                WMStatus = LabelHelper.WatermarkStatus.Pending,
                                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                            };
                            HSapiFacade.InsertCustomerFile(cfs);
                            string logMessage = Regex.Replace(file, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                            // base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, Cus.Id, null);
                        }
                        #endregion
                    }
                    else if (CusExd.ContractType == "CommercialFire")
                    {
                        from = "Commercial Fire Agreement";
                        file = "Commercial_Fire_Agreement";

                        AgreementResult = HSapiFacade.EmailOnlyLeadsAggrement(la, companyId, from);
                        #region file save to customer file
                        if (result)
                        {
                            CustomerFile cfs = new CustomerFile()
                            {
                                FileDescription = Cus.Id + "_" + Regex.Replace(file, @"\s+", String.Empty) + "_Mail" + ".pdf",
                                Filename = "/" + FileKey,
                                FileSize = (double)_fileSize,
                                FileId = Guid.NewGuid(),
                                FileFullName = Regex.Replace(file, @"\s+", String.Empty) + ".pdf",
                                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                CustomerId = Cus.CustomerId,
                                CompanyId = companyId,
                                IsActive = true,
                                CreatedBy = userid ?? Guid.Empty,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                UpdatedBy = userid ?? Guid.Empty,
                                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                WMStatus = LabelHelper.WatermarkStatus.Pending,
                                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                            };
                            HSapiFacade.InsertCustomerFile(cfs);
                            string logMessage = Regex.Replace(file, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                            //base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, Cus.Id, null);
                        }
                        #endregion
                    }
                    else
                    {
                        from = "Smart Lead Agreement";
                        AgreementResult = HSapiFacade.EmailOnlyLeadsAggrement(la, companyId, from);
                        file = "Smart_Lead_Agreement";
                        #region file save to customer file
                        if (result)
                        {
                            CustomerFile cfs = new CustomerFile()
                            {
                                FileDescription = Cus.Id + "_" + Regex.Replace(file, @"\s+", String.Empty) + "_Mail" + ".pdf",
                                Filename = "/" + FileKey,
                                FileSize = (double)_fileSize,
                                FileId = Guid.NewGuid(),
                                FileFullName = Regex.Replace(file, @"\s+", String.Empty) + ".pdf",
                                Uploadeddate = DateTime.Now.UTCCurrentTime(),
                                CustomerId = Cus.CustomerId,
                                CompanyId = companyId,
                                IsActive = true,
                                CreatedBy = userid ?? Guid.Empty,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                UpdatedBy = userid ?? Guid.Empty,
                                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                                WMStatus = LabelHelper.WatermarkStatus.Pending,
                                AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                            };
                            HSapiFacade.InsertCustomerFile(cfs);
                            string logMessage = Regex.Replace(file, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                            // base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, Cus.Id, null);
                        }
                        #endregion
                    }
                    message = "Agreement sent to " + EmailAddress;
                    Customer cus2 = HSapiFacade.GetCustomerById(Cus.Id);
                    cus2.IsAgreementSend = true;
                    cus2.AgreementEmail = PrefferedEmail;
                    if (firstpage == true || recreate == true)
                    {
                        cus2.Singature = "";
                        cus2.IsContractSigned = false;
                        var objrecreate = HSapiFacade.GetRecreateCustomerSignatureByCustomerId(cus2.CustomerId);
                        if (objrecreate != null)
                        {
                            objrecreate.Signature = "";
                            HSapiFacade.UpdateCustomerSignature(objrecreate);
                        }
                        var objfirstpage = HSapiFacade.GetFirstPageCustomerSignatureByCustomerId(cus2.CustomerId);
                        if (objfirstpage != null)
                        {
                            objfirstpage.Signature = "";
                            HSapiFacade.UpdateCustomerSignature(objfirstpage);
                        }
                    }
                    HSapiFacade.UpdateCustomer(cus2);
                    if (commercial == true)
                    {
                        var objcommercial = HSapiFacade.GetCommercialCustomerSignatureByCustomerId(cus2.CustomerId);
                        if (objcommercial != null)
                        {
                            objcommercial.Signature = "";
                            HSapiFacade.UpdateCustomerSignature(objcommercial);
                        }
                    }
                    #region Agreement History
                    var cusAgrHistory = HSapiFacade.GetCustomerAgreementHistory(Cus.CustomerId, LabelHelper.CustomerAgreementHistory.AgreementSend);
                    if (cusAgrHistory == null)
                    {
                        CustomerAgreement cusAgrModel = new CustomerAgreement()
                        {
                            CompanyId = Com.CompanyId,
                            CustomerId = Cus.CustomerId,
                            IP = AppConfig.GetIP,
                            UserAgent = AppConfig.GetUserAgent,
                            Type = LabelHelper.CustomerAgreementHistory.AgreementSend,
                            AddedDate = DateTime.UtcNow
                        };
                        HSapiFacade.InsertCustomerAgreement(cusAgrModel);
                    }
                    #endregion
                }
                else
                {
                    result = false;
                    message = "Invalid email address.";
                }
            }
            doc = null;
            pdf = null;

            GC.Collect();
            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Success(true));

        }


        public HttpResponseMessage SMSAgreementLinkForPrintBlank(int leadid, string PrefferedNO, bool? IsRecreate, int? agreementtempid, bool? isinvoice, string invoiceid, bool? isestimator, int? estid, Guid? userid, bool? firstpage, bool? commercial)
        {

            var identity = (ClaimsIdentity)User.Identity;

            var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
            if (usercontext == null)
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
            ComId = usercontext.CompanyId.ToString();
            var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
            var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;
            var CurrentUser = companyId;
            List<string> ReceiverNumberList = new List<string>();

            if (!HSapiFacade.CustomerIsInCompany(leadid, companyId))
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("User is not authenticated."));

            }
            DateTime FixDate = DateTime.Now.UTCCurrentTime();

            var Cus = HSapiFacade.GetCustomerById(leadid);
            //Cus.Soldby = CurrentUser.UserId.ToString();
            HSapiFacade.UpdateCustomer(Cus);
            int? ticketid = 0;
            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(leadid + "#" + Cus.EmailAddress + "#" + companyId + "#" + (IsRecreate.HasValue ? IsRecreate.Value : false) + "#" + (agreementtempid.HasValue ? agreementtempid.Value : 0) + "#" + (firstpage.HasValue ? firstpage.Value : false) + "#" + (ticketid.HasValue ? ticketid.Value : 0) + "#" + (isinvoice.HasValue ? isinvoice : false) + "#" + (!string.IsNullOrWhiteSpace(invoiceid) ? invoiceid : "") + "#" + (isestimator.HasValue ? isestimator.Value : false) + "#" + (estid.HasValue ? estid.Value : 0) + "#" + (userid != Guid.Empty ? userid : new Guid()) + "#" + (commercial.HasValue ? commercial.Value : false));
            //string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(leadid + "#" + Cus.EmailAddress + "#" + CurrentUser.CompanyId.Value.ToString() + "#" + (IsRecreate.HasValue ? IsRecreate.Value : false) + "#" + (agreementtempid.HasValue ? agreementtempid.Value : 0) + "#" + (isinvoice.HasValue? isinvoice:false) + "#" + (!string.IsNullOrEmpty(invoiceid)?invoiceid:"") + "#" + (isestimator.HasValue ? isestimator.Value : false) + "#" + (estid.HasValue ? estid.Value : 0) + "#" + (userid !=Guid.Empty ? userid : new Guid()));
            //string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Agreement/", encryptedurl);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/public/LeadsAgreementDocument/?code=", encryptedurl);
            string shortUrl = "";
            string SMSText = "";
            string ReceiverNumber = "";
            ShortUrl ShortUrl = HSapiFacade.GetSortUrlByUrl(fullurl, Cus.CustomerId);
            shortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code);
            //SMSText = string.Concat("Here is your agreements", Environment.NewLine, shortUrl);
            #region ReceiverNumber Setup
            if (!string.IsNullOrWhiteSpace(PrefferedNO))
            {
                ReceiverNumber = PrefferedNO.Replace("-", "");
            }
            else if (!string.IsNullOrWhiteSpace(Cus.SecondaryPhone))
            {
                ReceiverNumber = Cus.SecondaryPhone.Replace("-", "");
            }
            else if (!string.IsNullOrWhiteSpace(Cus.PrimaryPhone))
            {
                ReceiverNumber = Cus.PrimaryPhone.Replace("-", "");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Lead has no phone number available."));

            }
            ReceiverNumberList.Add(ReceiverNumber);
            #endregion
            #region model for creating pdf
            InstallationAgreementModel Model = new InstallationAgreementModel();
            Model.EmergencyContactList = new List<EmergencyContact>();
            Model.ListAgreementAnswer = new List<AgreementAnswer>();
            Model.EquipmentList = new List<Equipment>();
            Model.CustomerAgreement = new List<CustomerAgreement>();
            Model.SingleCustomerAgreement = new CustomerAgreement();
            var taxtotal = 0.0;
            bool AgreementResult = false;
            //var ActivationfeeValue = 0.0;
            //var IsActivationFee = _Util.Facade.ActivationFeeFacade.GetActivationFeeByCompanyId(CurrentUser.CompanyId.Value);
            //if (IsActivationFee != null)
            //{
            //    ActivationfeeValue = IsActivationFee.Fee;
            //}
            Customer Cuss = new Customer();
            CustomerExtended CusExd = new CustomerExtended();
            Company Com = new Company();
            if (leadid > 0)
            {
                if (!HSapiFacade.CustomerIsInCompany(leadid, companyId))
                {
                    return null;
                }
                Cus = HSapiFacade.GetCustomersById(leadid);
                if (Cus != null)
                {
                    CusExd = HSapiFacade.GetCustomerExtendedByCustomerId(Cus.CustomerId);
                }
                Com = HSapiFacade.GetCompanyByComapnyId(companyId);

                string ContractTerm = "";
                string ContractTermInWord = "";
                if (!string.IsNullOrWhiteSpace(Cus.ContractTeam) && Cus.ContractTeam != "-1")
                {
                    if (Cus.ContractTeam.ToLower() == "month to month")
                    {
                        ContractTerm = Cus.ContractTeam;
                        ContractTermInWord = Cus.ContractTeam;
                    }
                    else
                    {
                        ContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))).ToString();
                        ContractTermInWord = NumberToWords((Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))));
                    }

                }

                #region Sold by change
                //Person who sends the mail to Customer will be counted as sold by
                //Cus.Soldby = CurrentUser.UserId.ToString();
                HSapiFacade.UpdateCustomer(Cus);
                #endregion

                var UpfrontAddOnTotal = 0.0;
                var UpfrontAddOnTotalPromo = 0.0;
                bool IsUpfrontPromo = false;
                bool IsServicePromo = false;
                var MonthlyServiceFeeTotal = 0.0;
                var TotalMonthlyMonitoring = 0.0;
                var NewSubTotal = 0.0;
                var TotalDueAtSigning = 0.0;
                var EquipmentTotalPrice = 0.0;
                var ServiceTotalPrice = 0.0;
                var AgreementSubtotal = 0.0;
                var AgreementTotal = 0.0;
                var AgreementTax = 0.0;
                var NotARBEnabledTotalPrice = 0.0;
                string InstallTypeName = "";
                bool IsNonConfirming = false;
                var NonConfirmingFee = 0.0;
                var AdvanceServiceFeeTotal = 0.0;
                if (Cus.CreditScoreValue == null)
                {
                    Cus.CreditScoreValue = 0;
                }
                var PackageCustomer = HSapiFacade.GetPackageCustomerByCustomerId(Cus.CustomerId);
                if (PackageCustomer != null && PackageCustomer.NonConforming && PackageCustomer.NonConformingFee > 0 && (Cus.CreditScoreValue < PackageCustomer.MinCredit || Cus.CreditScoreValue > PackageCustomer.MaxCredit))
                {
                    IsNonConfirming = true;
                    NonConfirmingFee = PackageCustomer.NonConformingFee.Value;
                }
                var SmartPackageEquipmentServiceList = new List<SmartPackageEquipmentService>();
                var GetCityTaxList = HSapiFacade.GetCityTaxRate(Cus.CustomerId, Com.CompanyId);
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
                    var GetSalesTax = HSapiFacade.GetSalesTax(Com.CompanyId, CustomerId);
                    if (GetSalesTax != null)
                    {
                        AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                    }
                }
                #region Invoice
                Invoice invoice = new Invoice();
                CustomerProratedBill cpb = new CustomerProratedBill();
                cpb = HSapiFacade.GetCusProratedBillByCustomerId(Cus.CustomerId);
                List<InvoiceDetail> invoiceList = new List<InvoiceDetail>();
                if (!string.IsNullOrWhiteSpace(invoiceid) && isinvoice == true)
                {
                    invoice = HSapiFacade.GetByInvoiceId(invoiceid);
                    if (invoice != null)
                    {
                        invoiceList = HSapiFacade.GetInvoiceDetialsListByInvoiceId(invoice.InvoiceId);
                    }
                }
                #endregion
                #region Estimator
                Employee userInfo = new Employee();
                if (userid != Guid.Empty)
                {
                    userInfo = HSapiFacade.GetEmployeeByEmployeeId(userid.Value);
                }
                CreateEstimator createest = new CreateEstimator();
                if (isestimator.Value && estid > 0)
                {
                    CreateEstimator ca = new CreateEstimator();
                    ca.EstimatorSetting = new EstimatorSetting();
                    ca.Company = Com;

                    ca.Estimator = HSapiFacade.GetById(estid.Value);
                    ca._EstimatorPDFFilter = HSapiFacade.GetEstimatorPdfFilterByComIdCusIdUserId(Com.CompanyId, userid.Value, ca.Estimator.CustomerId);
                    ca.estimatorDetails = HSapiFacade.GetEstimatorDetailListByEstimatorId(ca.Estimator.EstimatorId);
                    ca.estimatorServices = HSapiFacade.GetEstimatorServicesByEstimatorId(ca.Estimator.EstimatorId);
                    if (ca.Estimator == null || ca.Estimator.CompanyId != Com.CompanyId)
                    {
                        return null;
                    }
                    if ((ca.estimatorDetails == null || ca.estimatorDetails.Count() == 0) && (ca.estimatorServices == null || ca.estimatorServices.Count() == 0))
                    {
                        return null;
                    }
                    Customer tempCUstomer = HSapiFacade.GetCustomerByCustomerId(ca.Estimator.CustomerId);
                    if (tempCUstomer == null)
                    {
                        return null;
                    }

                    CreateEstimator processedModel = GetEstimatorModelById(ca.Estimator, ca.estimatorDetails, ca.estimatorServices, Com, tempCUstomer, ca._EstimatorPDFFilter, Com.CompanyId);
                    Estimator estimator = HSapiFacade.GetEstimatorByEstimatorId(ca.Estimator.EstimatorId);
                    if (estimator != null)
                    {
                        //ViewBag.CoverLetter = estimator.CoverLetter;
                        processedModel.Estimator.CoverLetter = estimator.CoverLetter;
                        processedModel.Estimator.CoverLetterFile = estimator.CoverLetterFile;
                        processedModel.Estimator.ServicePlanType = estimator.ServicePlanType;
                        processedModel.Estimator.ServicePlanRate = estimator.ServicePlanRate;
                        processedModel.Estimator.ServicePlanAmount = estimator.ServicePlanAmount;
                        processedModel.Estimator.ServiceTaxAmount = estimator.ServiceTaxAmount;
                        processedModel.Estimator.ServiceTotalAmount = estimator.ServiceTotalAmount;
                        processedModel.Estimator.ShowServicePlan = estimator.ShowServicePlan;
                        processedModel.Estimator.ShowService = estimator.ShowService;
                        processedModel.Estimator.ServicePlanTypeName = "Service Plan";

                        //SelectListItem selectListItem = HSapiFacade.GetDropdownsByKey("ServicePlans").Where(x => x.Value == estimator.ServicePlanType).FirstOrDefault();
                        //if (selectListItem != null)
                        //{
                        //    processedModel.Estimator.ServicePlanTypeName = selectListItem.Text;
                        //}

                    }
                    if (ca.estimatorDetails != null)
                    {
                        foreach (var item in ca.estimatorDetails)
                        {
                            Manufacturer Manufacturer = HSapiFacade.GetManufacturerByManufacturerId(item.ManufacturerId);
                            if (Manufacturer != null)
                            {
                                item.Manufacturer = Manufacturer.Name;
                            }
                            item.CreatedDate = DateTime.Now.UTCCurrentTime();
                            ca.SubTotal = ca.SubTotal + (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
                            item.EquipmentFile = HSapiFacade.GetEquipmentFilesByEquipmentIdAndFileType(item.EquipmentId, LabelHelper.EquipmentFileType.ProfilePicture).FirstOrDefault();
                            if (item.EquipmentFile == null)
                            {
                                item.EquipmentFile = new EquipmentFile();
                            }
                        }
                    }
                    if (ca.estimatorServices != null)
                    {
                        foreach (var item in ca.estimatorServices)
                        {
                            processedModel.ServiceSubTotal += ca.ServiceSubTotal + item.Amount;
                        }
                        processedModel.TotalServiceAmount = processedModel.ServiceSubTotal + ca.ServiceTax;
                    }
                    createest = processedModel;
                    createest.eSecurityLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/eSecurity_logo.png");
                    createest.specializedLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/specialized_property_management_logo.png");
                    string EstimatorContractTerm = "";
                    if (!string.IsNullOrWhiteSpace(createest.Estimator.ContractTerm) && createest.Estimator.ContractTerm != "-1")
                    {
                        if (createest.Estimator.ContractTerm.ToLower() == "month to month")
                        {
                            EstimatorContractTerm = createest.Estimator.ContractTerm;
                        }
                        else
                        {
                            EstimatorContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(createest.Estimator.ContractTerm) * 12))).ToString() + " month";
                        }

                    }
                    createest.EstimatorContractTerm = EstimatorContractTerm;
                }
                #endregion
                var CustomEquipmentList = new List<Equipment>();
                if (firstpage == true || IsRecreate == true)
                {
                    CustomEquipmentList = HSapiFacade.GetSmartEquipmentListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (CustomEquipmentList.Count > 0)
                    {
                        foreach (var item in CustomEquipmentList)
                        {
                            EquipmentTotalPrice += item.Total;
                            UpfrontAddOnTotal += item.Total;
                        }
                    }
                }
                else
                {
                    CustomEquipmentList = HSapiFacade.GetSmartEquipmentListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (CustomEquipmentList.Count > 0)
                    {
                        foreach (var item in CustomEquipmentList)
                        {
                            EquipmentTotalPrice += item.Total;
                            UpfrontAddOnTotal += item.Total;
                        }
                    }
                }
                var CustomServiceList = new List<Equipment>();
                if (firstpage == true || IsRecreate == true)
                {
                    CustomServiceList = HSapiFacade.GetSmartServiceListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (CustomServiceList.Count > 0)
                    {
                        foreach (var item in CustomServiceList)
                        {
                            EquipmentTotalPrice += item.Total;
                            ServiceTotalPrice += item.Total;
                            MonthlyServiceFeeTotal += item.Total;
                        }
                    }
                }
                else
                {
                    CustomServiceList = HSapiFacade.GetSmartServiceListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (CustomServiceList.Count > 0)
                    {
                        foreach (var item in CustomServiceList)
                        {
                            EquipmentTotalPrice += item.Total;
                            ServiceTotalPrice += item.Total;
                            MonthlyServiceFeeTotal += item.Total;
                        }
                    }
                }
                var NotARBEnabledServiceList = new List<Equipment>();
                if (firstpage == true || IsRecreate == true || commercial == true)
                {
                    NotARBEnabledServiceList = HSapiFacade.GetNotARBEnabledSmartServiceListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (NotARBEnabledServiceList.Count > 0)
                    {
                        foreach (var item in NotARBEnabledServiceList)
                        {
                            NotARBEnabledTotalPrice += item.Total;
                        }
                    }
                }
                else
                {
                    NotARBEnabledServiceList = HSapiFacade.GetNotARBEnabledSmartServiceListFromService(Cus.CustomerId, Com.CompanyId);
                    if (NotARBEnabledServiceList.Count > 0)
                    {
                        foreach (var item in NotARBEnabledServiceList)
                        {
                            NotARBEnabledTotalPrice += item.Total;
                        }
                    }
                }
                #region Advance Monitoring Service Month

                PaymentInfoCustomer paycus = new PaymentInfoCustomer();
                paycus = HSapiFacade.GetPaymentInfoCustomerByCustomerIdAndPayForService(Cus.CustomerId);
                int ForMonth = 1;
                if (paycus != null && paycus.ForMonths.HasValue)
                {
                    ForMonth = paycus.ForMonths.Value;
                }
                if (ForMonth > 1)
                {
                    AdvanceServiceFeeTotal = MonthlyServiceFeeTotal * (ForMonth - 1);

                }
                #endregion
                Cus.MonthlyMonitoringFee = Convert.ToString(ServiceTotalPrice);
                TotalMonthlyMonitoring = MonthlyServiceFeeTotal;
                NewSubTotal = TotalMonthlyMonitoring + UpfrontAddOnTotal;
                if (CustomServiceList.Count > 0 || CustomEquipmentList.Count > 0)
                {
                    if (PackageCustomer != null && PackageCustomer.ActivationFee.HasValue)
                    {
                        AgreementSubtotal = PackageCustomer.ActivationFee.Value + EquipmentTotalPrice;
                        NewSubTotal = PackageCustomer.ActivationFee.Value + EquipmentTotalPrice;
                    }
                    else
                    {
                        AgreementSubtotal = EquipmentTotalPrice;
                        NewSubTotal = EquipmentTotalPrice;
                    }
                }
                if (IsNonConfirming && NonConfirmingFee > 0)
                {
                    AgreementSubtotal = AgreementSubtotal + NonConfirmingFee;
                    NewSubTotal = NewSubTotal + NonConfirmingFee;
                }
                if (AgreementTax != 0.0)
                {
                    taxtotal = (AgreementSubtotal / 100) * AgreementTax;
                    Model.TaxTotal = taxtotal;
                    AgreementTotal = AgreementSubtotal + taxtotal;
                    TotalDueAtSigning = NewSubTotal + taxtotal;
                }
                else
                {
                    Model.TaxTotal = 0.0;
                    AgreementTotal = AgreementSubtotal;
                    TotalDueAtSigning = NewSubTotal;
                }
                var PackageCustomerDetails = HSapiFacade.GetPackageCustomerByCustomerIdandCompanyId(Cus.CustomerId, companyId);
                if (PackageCustomerDetails != null)
                {
                    InstallTypeName = HSapiFacade.SmartInstallTypeNameByInstallTypeId(Convert.ToInt32(PackageCustomerDetails.SmartInstallTypeId));
                    SmartPackageEquipmentServiceList = HSapiFacade.GetAllSmartPackageIncludeEquipmentByPackageIdAndCompanyId(PackageCustomerDetails.PackageId, companyId);
                }
                var PaymentDetails = HSapiFacade.GetAllPaymentInfoByCustomerIdAndCompanyId(Cus.CustomerId, companyId).Where(m => m.PayFor == "First Month").FirstOrDefault();
                var agreementPayment = HSapiFacade.GetLeadAgreementPaymentInfoByCustomerId(Cus.CustomerId);
                string paymentoverviewheader = "";
                string paymentoverviewdata = "";
                if (agreementPayment != null && agreementPayment.Count > 0)
                {
                    paymentoverviewheader = "<table style='border-collapse:collapse; width:100%; font-family:Arial; table-layout:fixed; font-size:13px;'>{0}</table>";
                    foreach (var pay in agreementPayment)
                    {
                        var sppay = pay.Type.Split('_');
                        if (sppay.Length > 0)
                        {
                            if (sppay[0] == "CC")
                            {
                                var cardNumber = pay.CardNumber.Replace('-', ' ').Replace(" ", "");
                                if (cardNumber.Length == 16)
                                {
                                    paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Card Number: " + string.Concat("".PadLeft(12, '*'), cardNumber.Substring(cardNumber.Length - 4)) + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Expire Date: " + pay.CardExpireDate + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Security Code: " + string.Concat("".PadLeft(2, '*'), pay.CardSecurityCode.Substring(pay.CardSecurityCode.Length - 1)) + @"</td>
                                                        </tr>";
                                }
                                else
                                {
                                    paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Card Number: " + string.Concat("".PadLeft(11, '*'), cardNumber.Substring(cardNumber.Length - 4)) + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Expire Date: " + pay.CardExpireDate + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Security Code: " + string.Concat("".PadLeft(2, '*'), pay.CardSecurityCode.Substring(pay.CardSecurityCode.Length - 1)) + @"</td>
                                                        </tr>";
                                }
                            }
                            else if (sppay[0] == "ACH")
                            {
                                paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Type: " + pay.BankAccountType + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Routing No: " + pay.RoutingNo + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account No: " + string.Concat("".PadLeft(pay.AcountNo.Length - 4, '*'), pay.AcountNo.Substring(pay.AcountNo.Length - 4)) + @"</td>
                                                        </tr>";
                            }
                        }
                    }
                }
                var CustomerAddress = AddressHelper.MakeAddress(Cus);
                var CustomerInstallAddress = AddressHelper.MakeInstallAddress(Cus);
                CustomerSignature cs = new CustomerSignature();
                GlobalSetting glbs = HSapiFacade.GetGlobalSettingsByKey(companyId, "CompanySignature");
                string cusSignature = "";
                string cussignDate = "";
                DateTime cussignDateVal = new DateTime();
                if (firstpage.HasValue && firstpage.Value == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "First Page");
                }
                else if (commercial.HasValue && commercial.Value == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Commercial");
                }
                else if (isinvoice.HasValue && isinvoice.Value == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Estimate");
                }
                else if (isestimator.HasValue && isestimator.Value == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Estimator");
                }
                else if (IsRecreate.HasValue && IsRecreate.Value == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Recreate");
                }
                else if (Cus != null && agreementtempid.HasValue && agreementtempid.Value > 0)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Agreement File");
                }
                else
                {
                    cusSignature = Cus.Singature;
                    if (Cus.CustomerSignatureDate != null && Cus.CustomerSignatureDate != new DateTime())
                    {
                        cussignDate = Cus.CustomerSignatureDate.Value.UTCToClientTime().ToString("M/dd/yy");
                        cussignDateVal = Cus.CustomerSignatureDate.Value.UTCToClientTime();
                    }

                }
                if (cs != null && (agreementtempid != 0 || (firstpage.HasValue && firstpage.Value == true) || (IsRecreate.HasValue && IsRecreate.Value == true) || (commercial.HasValue && commercial.Value == true)))
                {
                    cusSignature = cs.Signature;
                    if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                    {
                        cussignDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        cussignDateVal = cs.CreatedDate.UTCToClientTime();
                    }

                }
                //(!string.IsNullOrWhiteSpace(Cus.MonthlyMonitoringFee) ? Convert.ToDouble(Cus.MonthlyMonitoringFee) : 1) * Convert.ToDouble(ContractTerm),

                Double MMR = 1;
                Double CTerm = 0;

                double.TryParse(Cus.MonthlyMonitoringFee, out MMR);
                double.TryParse(ContractTerm, out CTerm);
                //(!string.IsNullOrWhiteSpace() ? Convert.ToDouble(Cus.MonthlyMonitoringFee) : 1) * Convert.ToDouble(ContractTerm);
                Double TotalPayments = MMR * CTerm;
                #region For Promo Pyment Method
                List<PaymentInfoCustomer> paycusList = new List<PaymentInfoCustomer>();
                PaymentProfileCustomer paymentProfile = new PaymentProfileCustomer();
                paycusList = HSapiFacade.GetAllPaymentInfoCustomerByCustomerId(Cus.CustomerId);
                if (paycusList != null && paycusList.Count > 0)
                {
                    foreach (var item in paycusList)
                    {
                        paymentProfile = HSapiFacade.GetPaymentProfileByPaymentInfoId(item.PaymentInfoId);
                        if (paymentProfile != null && paymentProfile.Type == LabelHelper.PaymentMethod.Promo)
                        {
                            if (item.Payfor == "Activation Fee")
                            {
                                NonConfirmingFee = 0.0;

                                if (PackageCustomer != null)
                                {
                                    PackageCustomer.ActivationFee = 0.0;
                                }
                                NewSubTotal = NewSubTotal - (PackageCustomer.AdditionFee + NonConfirmingFee);
                            }
                            else if (item.Payfor == "Equipment")
                            {
                                IsUpfrontPromo = true;
                                NewSubTotal = NewSubTotal - UpfrontAddOnTotal;
                            }
                            else if (item.Payfor == "Service")
                            {
                                NewSubTotal = NewSubTotal - TotalMonthlyMonitoring;
                                IsServicePromo = true;

                            }


                        }

                    }

                }
                #endregion

                #region LeadSource
                string LeadSource = "";
                Lookup leadsource = HSapiFacade.GetLookupByKeyAndValueAndCompanyId("LeadSource", Cus.LeadSource, Com.CompanyId);
                LeadSource = leadsource != null && !string.IsNullOrWhiteSpace(leadsource.DisplayText) && leadsource.DataValue != "-1" ? leadsource.DisplayText : "";
                #endregion

                Model = new InstallationAgreementModel()
                {
                    CSIDNumber = Cus.Id,
                    LeadSource = LeadSource,
                    IsNonConfirming = IsNonConfirming,
                    NonConfirmingFee = NonConfirmingFee,
                    InstallDate = Cus.InstallDate != null ? Convert.ToDateTime(Cus.InstallDate).ToShortDateString() : "",
                    OriginalContactDate = (Cus.OriginalContactDate != null && Cus.OriginalContactDate.HasValue) ? Cus.OriginalContactDate.Value : new DateTime(),
                    AccountType = Cus.Type,
                    Referredby = Cus.ReferringCustomer != Guid.Empty ? HSapiFacade.GetCustomerNameById(Cus.ReferringCustomer) : "",
                    SocialSecurityNumber = Cus.SSN,
                    //Owner2ndPhone = Cus.SecondaryPhone,
                    Owner2ndPhone = Cus.PrimaryPhone,
                    InitialStreet = Cus.Street,
                    InitialCity = Cus.City,
                    InitialCountry = Cus.County,
                    InitialState = Cus.State,
                    InitialZip = Cus.ZipCode,
                    InitialApt = Cus.Appartment,
                    BillingCity = Cus.CityPrevious,
                    BillingState = Cus.StatePrevious,
                    BillingZip = Cus.ZipCodePrevious,
                    BillingCountry = Cus.CountryPrevious,
                    BillingStreet = Cus.StreetPrevious,
                    InstallTypeName = InstallTypeName,
                    SmartPackageEquipmentServiceList = SmartPackageEquipmentServiceList,
                    UpfrontAddOnTotal = UpfrontAddOnTotal,
                    UpfrontAddOnTotalPromo = UpfrontAddOnTotalPromo,
                    IsUpfrontPromo = IsUpfrontPromo,
                    IsServicePromo = IsServicePromo,
                    MonthlyServiceFeeTotal = MonthlyServiceFeeTotal,
                    TotalMonthlyMonitoring = TotalMonthlyMonitoring,
                    NewSubTotal = NewSubTotal,
                    TotalDueAtSigning = TotalDueAtSigning,
                    PaymentDetails = PaymentDetails != null ? PaymentDetails : new PaymentInfo(),
                    DisplayName = Cus.DisplayName,
                    BillingAddress = CustomerAddress,
                    OwnerAddress = CustomerAddress,
                    InstallAddress = CustomerInstallAddress,
                    OwnerEmail = Cus.EmailAddress,
                    //OwnerPhone = Cus.PrimaryPhone,
                    OwnerPhone = Cus.CellNo,
                    OwnerName = Cus.FirstName + " " + Cus.LastName,
                    FirstName = Cus.FirstName,
                    LastName = Cus.LastName,
                    EmergencyContactList = HSapiFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId),
                    CompanyName = Com.CompanyName,
                    CompanySate = string.Format("{0}, {1} {2}", Com.City, Com.State, Com.ZipCode),
                    CompanyStreet = Com.Street,
                    CompanyWebsite = Com.Website,
                    SubscribedMonths = ContractTerm,
                    SubscribedMonthsInWord = ContractTermInWord,
                    RenewalMonths = Cus.RenewalTerm.HasValue ? Cus.RenewalTerm.Value : 0,
                    Password = Cus.Passcode,
                    DateOfTransaction = FixDate.UTCToClientTime(),
                    CompanyLogo = HSapiFacade.GetCompanyEmailLogoByCompanyId(Com.CompanyId),
                    EquipmentList = CustomEquipmentList.ToList(),
                    ServiceList = CustomServiceList.ToList(),
                    ActivationFee = (PackageCustomer != null && PackageCustomer.ActivationFee.HasValue) ? PackageCustomer.ActivationFee.Value : 0,
                    BusinessName = Cus.BusinessName,
                    MonthlyMonitoringFee = Cus.MonthlyMonitoringFee,
                    EffectiveDate = FixDate.UTCToClientTime(),
                    CustomerSignature = cusSignature,
                    CustomerSignatureStringDate = cussignDate,
                    CustomerSignatureStringDateVal = cussignDateVal,
                    CustomerAgreement = HSapiFacade.GetCustomerAgreementByCompanyIdAndCustomerId1(Com.CompanyId, Cus.CustomerId),
                    Subtotal = AgreementSubtotal,
                    Tax = AgreementTax,
                    TaxTotal = taxtotal,
                    Total = AgreementTotal,
                    EContractId = Cus.Id,
                    ListAgreementAnswer = HSapiFacade.GetAllAgreementAnswerByCustomerId(Cus.CustomerId),
                    SalesRepresentative = !string.IsNullOrEmpty(Cus.Soldby) ? HSapiFacade.GetEmployeeNumByEmployeeId(new Guid(Cus.Soldby)).ToString() : "",
                    TotalPayments = TotalPayments,
                    SingleCustomerAgreement = HSapiFacade.GetIpAndUserAgentByCustomerIdAndCompanyId(Com.CompanyId, Cus.CustomerId),
                    ListContactEmergency = string.Format(paymentoverviewheader, paymentoverviewdata),
                    ListPaymentInfo = string.Format(paymentoverviewheader, paymentoverviewdata),
                    DoingBusinessAs = Cus.DBA,
                    DispalyName = Cus.DisplayName,
                    CompanyPhone = Com.Phone,
                    FirstPage = firstpage.HasValue ? firstpage.Value : false,
                    Commercial = commercial.HasValue ? commercial.Value : false,
                    IsInvoice = isinvoice.Value,
                    InvoiceId = invoiceid,
                    InvoiceList = invoiceList,
                    IsEstimator = isestimator.HasValue ? isestimator.Value : false,
                    createEst = createest,
                    userInfo = userInfo,
                    inv = invoice,
                    ContractType = CusExd != null && !string.IsNullOrWhiteSpace(CusExd.ContractType) ? CusExd.ContractType : "",
                    NotARBEnabledServiceList = NotARBEnabledServiceList.ToList(),
                    NotARBEnabledTotalPrice = NotARBEnabledTotalPrice,
                    ProratedAmout = cpb != null ? Math.Round(cpb.Amount, 2, MidpointRounding.AwayFromZero) : 0.0,
                    FinancedAmout = Cus != null && Cus.FinancedAmount != null ? Math.Round(Cus.FinancedAmount.Value, 2, MidpointRounding.AwayFromZero) : 0.0,
                    MonthlyFinanceRate = CusExd != null && CusExd.MonthlyFinanceRate != null ? Math.Round(CusExd.MonthlyFinanceRate.Value, 2, MidpointRounding.AwayFromZero) : 0.0,
                    AdvanceServiceFeeTotal = AdvanceServiceFeeTotal
                };
                if (agreementtempid != 0)
                {
                    if (cs != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(cs.Signature))
                    {
                        Model.CompanySignature = glbs.Value;
                        if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                        {
                            Model.CompanySignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                }
                else if (firstpage == true || IsRecreate == true || commercial == true)
                {
                    if (glbs != null && !string.IsNullOrWhiteSpace(glbs.Value))
                    {
                        Model.CompanySignature = glbs.Value;
                        Model.CompanySignatureDate = cussignDate;
                    }
                }
                else
                {
                    if (Cus != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(Cus.Singature))
                    {
                        Model.CompanySignature = glbs.Value;
                        if (Cus.CustomerSignatureDate != null && Cus.CustomerSignatureDate != new DateTime())
                        {
                            Model.CompanySignatureDate = Cus.CustomerSignatureDate.Value.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                }

                if (Model.EmergencyContactList == null)
                {
                    Model.EmergencyContactList = new List<EmergencyContact>();
                }
            }
            else
            {
                Model.CompanyLogo = HSapiFacade.GetCompanyLogoForPDFByCompanyId(companyId);
            }

            //  return View(Model);
            Model.KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG");
            //  ViewBag.CompanyId = Com.CompanyId.ToString();
            Model.CompanyId = Com.CompanyId.ToString();
            Model.CurrentCurrency = HSapiFacade.GetCurrentCurrencyByCompanyId(Com.CompanyId);
            string body = HSapiFacade.MakeSmartAgreementPdf(Model, agreementtempid.HasValue ? agreementtempid.Value : 0);
            //  ViewBag.Body = body;
            #region File Save
            ViewAsPdf actionPDF;

            actionPDF = new Rotativa.ViewAsPdf("~/Views/SmartLeads/SmartInstallationAgreement.cshtml", Model)
            {
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
        ColorMode = ColorMode.Color,
        Orientation = Orientation.Portrait,
        PaperSize = PaperKind.A4,
    },
                Objects = {
        new ObjectSettings() {
            HtmlContent = body,
        }
    }
            };

            byte[] pdf = null;
            try
            {
                pdf = Converter.Convert(doc);
            }
            catch (Exception ex)
            {
                throw new Exception("Error during PDF generation", ex);
            }




            Random rand = new Random();

            string filename = ConfigurationManager.AppSettings["File.LeadToCustomerAgreement"];
            var comname = HSapiFacade.GetCompanyByComapnyId(companyId).CompanyName.ReplaceSpecialChar();
            var pdftempFolderName = string.Format(filename, comname) + rand.Next().ToString() + leadid + "AgreementMail.pdf";
            string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
            FileHelper.SaveFile(pdf, Serverfilename);



            doc = null;
            pdf = null;

            GC.Collect();


            #endregion
            #endregion

            SMSAgreement smsAgreement = new SMSAgreement();

            smsAgreement.ShortUrl = shortUrl;
            smsAgreement.CompanyName = usercontext.CompanyName;
            string phonenumber = string.Join(";", ReceiverNumberList);
            if (HSapiFacade.SendAgrementSMS(smsAgreement, userid ?? Guid.Empty, companyId, ReceiverNumberList, false, usercontext.UserName) == true)
            {
                #region insert LeadCorrespondence
                if (IsRecreate == true)
                {
                    LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                    {
                        CompanyId = companyId,
                        CustomerId = Cus.CustomerId,
                        Type = "SMS",
                        ToMobileNo = phonenumber,
                        BodyContent = "Recreate Agreement",
                        SentDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedDate = DateTime.Now,
                        SentBy = userid ?? Guid.Empty
                    };
                    HSapiFacade.InsertCorrespondence(LeadCorrespondence);
                }
                else if (firstpage == true)
                {
                    LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                    {
                        CompanyId = companyId,
                        CustomerId = Cus.CustomerId,
                        Type = "SMS",
                        ToMobileNo = phonenumber,
                        BodyContent = "FirstPage Agreement",
                        SentDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedDate = DateTime.Now,
                        SentBy = userid ?? Guid.Empty
                    };
                    HSapiFacade.InsertCorrespondence(LeadCorrespondence);
                }
                else if (commercial == true)
                {
                    LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                    {
                        CompanyId = companyId,
                        CustomerId = Cus.CustomerId,
                        Type = "SMS",
                        ToMobileNo = phonenumber,
                        BodyContent = "Commercial Agreement",
                        SentDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedDate = DateTime.Now,
                        SentBy = userid ?? Guid.Empty
                    };
                    HSapiFacade.InsertCorrespondence(LeadCorrespondence);
                }
                else
                {
                    LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                    {
                        CompanyId = companyId,
                        CustomerId = Cus.CustomerId,
                        Type = "SMS",
                        ToMobileNo = phonenumber,
                        BodyContent = "Smart Lead Agreement",
                        SentDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedDate = DateTime.Now,
                        SentBy = userid ?? Guid.Empty
                    };
                    HSapiFacade.InsertCorrespondence(LeadCorrespondence);
                }
                #endregion

                if (IsRecreate == true)
                {

                    string FileName = "Recreate_Agreement";
                    #region file save to customer file
                    CustomerFile cfs = new CustomerFile()
                    {
                        FileDescription = Cus.Id + "_" + Regex.Replace(FileName, @"\s+", String.Empty) + "_SMS" + ".pdf",
                        Filename = "/" + pdftempFolderName,
                        FileId = Guid.NewGuid(),
                        FileFullName = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf",
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = Cus.CustomerId,
                        CompanyId = companyId,
                        IsActive = true,
                        CreatedBy = userid ?? Guid.Empty,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = userid ?? Guid.Empty,
                        UpdatedDate = DateTime.Now.UTCCurrentTime(),
                        WMStatus = LabelHelper.WatermarkStatus.Pending,
                        AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                    };
                    HSapiFacade.InsertCustomerFile(cfs);
                    string logMessage = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf " + "file send to " + ReceiverNumber;
                    // base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, Cus.Id, null);
                    #endregion
                }
                else if (firstpage == true)
                {

                    string FileName = "Firstpage_Agreement";
                    #region file save to customer file
                    CustomerFile cfs = new CustomerFile()
                    {
                        FileDescription = Cus.Id + "_" + Regex.Replace(FileName, @"\s+", String.Empty) + "_SMS" + ".pdf",
                        Filename = "/" + pdftempFolderName,
                        FileId = Guid.NewGuid(),
                        FileFullName = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf",
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = Cus.CustomerId,
                        CompanyId = companyId,
                        IsActive = true,
                        CreatedBy = userid ?? Guid.Empty,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = userid ?? Guid.Empty,
                        UpdatedDate = DateTime.Now.UTCCurrentTime()
                    };
                    HSapiFacade.InsertCustomerFile(cfs);
                    string logMessage = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf " + "file send to " + ReceiverNumber;
                    // base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, Cus.Id, null);
                    #endregion
                }
                else if (commercial == true)
                {

                    string FileName = "Commercial_Agreement";
                    #region file save to customer file
                    CustomerFile cfs = new CustomerFile()
                    {
                        FileDescription = Cus.Id + "_" + Regex.Replace(FileName, @"\s+", String.Empty) + "_SMS" + ".pdf",
                        Filename = "/" + pdftempFolderName,
                        FileId = Guid.NewGuid(),
                        FileFullName = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf",
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = Cus.CustomerId,
                        CompanyId = companyId,
                        IsActive = true,
                        CreatedBy = userid ?? Guid.Empty,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = userid ?? Guid.Empty,
                        UpdatedDate = DateTime.Now.UTCCurrentTime()
                    };
                    HSapiFacade.InsertCustomerFile(cfs);
                    string logMessage = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf " + "file send to " + ReceiverNumber;
                    // base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, Cus.Id, null);
                    #endregion
                }
                else
                {
                    string FileName = "Smart_Lead_Agreement";
                    #region file save to customer file
                    CustomerFile cfs = new CustomerFile()
                    {
                        FileDescription = Cus.Id + "_" + Regex.Replace(FileName, @"\s+", String.Empty) + "_SMS" + ".pdf",
                        Filename = "/" + pdftempFolderName,
                        FileId = Guid.NewGuid(),
                        FileFullName = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf",
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = Cus.CustomerId,
                        CompanyId = companyId,
                        IsActive = true,
                        CreatedBy = userid ?? Guid.Empty,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = userid ?? Guid.Empty,
                        UpdatedDate = DateTime.Now.UTCCurrentTime()
                    };
                    HSapiFacade.InsertCustomerFile(cfs);
                    string logMessage = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf " + "file send to " + ReceiverNumber;
                    // base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, Cus.Id, null);
                    #endregion
                }
                Customer cus2 = HSapiFacade.GetCustomerById(Cus.Id);
                cus2.IsAgreementSend = true;
                if (IsRecreate == true)
                {
                    cus2.Singature = "";
                }

                HSapiFacade.UpdateCustomer(cus2);

                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("SMS sent to {0} successfully."));


            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Sending to {0} failed."));

            }

        }


        #endregion
        [Route("generate-first-page-document")]
        [HttpPost]
        public HttpResponseMessage UpdateAgreementFirstPageInfo()
        {

            int id = 0;
            Guid sellerGuid = new Guid();
            Guid customerId = new Guid();
            Guid ticketid = new Guid();
            string contractType = "";
            string contractTerm = "";
            int renewalTerm = 0;
            double activationFee = 0.0;
            double nonConfirmingFee = 0.0;
            double effectiveContractDate = 0.0;
            string verbalPassword = "";
            bool nonCommissionable = false;
            bool result = false;
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("id"))
            {
                int.TryParse(headers.GetValues("id").First(), out id);
            }
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            if (headers.Contains("sellerGuid"))
            {
                Guid.TryParse(headers.GetValues("sellerGuid").First(), out sellerGuid);
            }
            if (headers.Contains("customerId"))
            {
                Guid.TryParse(headers.GetValues("customerId").First(), out customerId);
            }
            if (headers.Contains("contractType"))
            {
                contractType = headers.GetValues("contractType").First();
            }
            if (headers.Contains("contractTerm"))
            {
                contractTerm = headers.GetValues("contractTerm").First();
            }
            if (headers.Contains("renewalTerm"))
            {
                int.TryParse(headers.GetValues("renewalTerm").First(), out renewalTerm);
            }
            if (headers.Contains("activationFee"))
            {
                double.TryParse(headers.GetValues("activationFee").First(), out activationFee);
            }
            if (headers.Contains("nonConfirmingFee"))
            {
                double.TryParse(headers.GetValues("nonConfirmingFee").First(), out nonConfirmingFee);
            }
            if (headers.Contains("effectiveContractDate"))
            {
                double.TryParse(headers.GetValues("effectiveContractDate").First(), out effectiveContractDate);
            }
            if (headers.Contains("verbalPassword"))
            {
                verbalPassword = headers.GetValues("verbalPassword").First();
            }
            if (headers.Contains("nonCommissionable"))
            {
                bool.TryParse(headers.GetValues("nonCommissionable").First(), out nonCommissionable);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var userIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "userid")?.Value;
                var userId = Guid.TryParse(userIdClaim, out Guid parsedUserId) ? parsedUserId : Guid.Empty;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                    var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;

                    APIInitialize();

                    //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                    Ticket ticket = HSapiFacade.GetTicketByTicketId(ticketid);
                    string message = "";
                    Customer cus = HSapiFacade.GetCustomerByCustomerId(customerId);
                    cus.ContractTeam = contractTerm;
                    cus.Passcode = verbalPassword;
                    cus.ActivationFee = activationFee;
                    cus.RenewalTerm = renewalTerm;
                    //cus.OriginalContactDate = model.OrginalContractDate;
                    DateTime? OG_ContractDate = cus.OriginalContactDate;
                    HSapiFacade.UpdateCustomer(cus);

                    if (cus != null)
                    {
                        var cusExt = HSapiFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                        if (cusExt != null)
                        {
                            cusExt.ContractType = contractType;
                            OG_ContractDate = OG_ContractDate ?? cusExt.ContractStartDate;
                            cusExt.ContractStartDate = DateTime.Now;
                            HSapiFacade.UpdateCustomerExtended(cusExt);
                        }
                    }

                    PackageCustomer pc = new PackageCustomer();
                    pc = HSapiFacade.GetPackageCustomerByCustomerId(cus.CustomerId);
                    if (pc != null)
                    {
                        pc.ActivationFee = activationFee;
                        pc.NonConformingFee = nonConfirmingFee;
                        pc.NonConforming = true;
                        HSapiFacade.UpdatePackageCustomer(pc);
                        result = true;
                    }
                    else
                    {
                        pc = new PackageCustomer()
                        {
                            CustomerId = cus.CustomerId,
                            CompanyId = companyId,
                            PackageId = new Guid(),
                            NonConformingFee = nonConfirmingFee,
                            ActivationFee = activationFee,
                            MinCredit = 0,
                            MaxCredit = 0,
                            NonConforming = true
                        };
                        HSapiFacade.InsertPackageCustomer(pc);
                        result = true;
                    }
                    var smartLeadsResponse = GetSmartLeadsForPopUp(
                              LeadId: cus.Id,
                              grant: false,
                              templateid: null,
                              firstpage: true,
                              ticketid: ticket.Id,
                              recreate: false,
                              isinvoice: false,
                              invoiceid: null,
                              isestimator: false,
                              estid: 0,
                              userid: userId,
                              commercial: cus.ContractTeam == "commercial",
                              EstimatorId: ""
                                     );

                    string url = smartLeadsResponse.result.url;

                    // Return the URL in the response
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(new { url = url }));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }
        public ApiResponse<SmartLeadsResponse> GetSmartLeadsForPopUp(int? LeadId, bool? grant, int? templateid, bool? firstpage, int? ticketid, bool? recreate, bool? isinvoice, string invoiceid, bool? isestimator, int? estid, bool? commercial, Guid userid, string EstimatorId)
        {


            estid = 0;
            DateTime ContractDate = new DateTime();
            var identity = (ClaimsIdentity)User.Identity;
            //var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));

            ComId = usercontext.CompanyId.ToString();
            var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
            var companyId = Guid.TryParse(companyIdClaim, out Guid compId) ? compId : Guid.Empty;
            GlobalSetting SingleContractAgreement = HSapiFacade.GetGlobalSettingsByKey(companyId, "ShowSingleContractAgreement");

            var response = new SmartLeadsResponse
            {
                LeadId = LeadId,
                InvoiceId = invoiceid == "0" ? "" : invoiceid,
                IsInvoice = isinvoice.HasValue && isinvoice.Value,
                FirstPage = firstpage.HasValue && firstpage.Value,
                Recreate = recreate.HasValue && recreate.Value,
                TicketId = ticketid.HasValue ? ticketid.Value : 0,
                IsEstimator = isestimator.HasValue && isestimator.Value,
                EstId = estid.HasValue ? estid.Value : 0,
                EstimatorId = EstimatorId,
                AgreementDocumentHeight = templateid.HasValue ? templateid.Value : 0,
                StringHeight = $"{HSapiFacade.GetAgreementDocumentHeightByCompanyId(companyId)}px",
                ContractDate = ContractDate.ToString("yyyy-MM-dd"), // Format as needed
                //MultipleDoc = new List<SelectListItem>()
            };



            if (LeadId.HasValue)
            {
                var cus = HSapiFacade.GetCustomerById(LeadId.Value);
                cus.CustomerExtended = HSapiFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                cus.PhoneNumberVal = !string.IsNullOrWhiteSpace(cus.PrimaryPhone) ? cus.PrimaryPhone :
                                    !string.IsNullOrWhiteSpace(cus.CellNo) ? cus.CellNo :
                                    !string.IsNullOrWhiteSpace(cus.SecondaryPhone) ? cus.SecondaryPhone : "";

                response.CustomerDetails = cus;

                var _cuscom = HSapiFacade.GetCustomerCompanyByCompanyIdAndCustomerId(companyId, cus.CustomerId);
                if (_cuscom != null && _cuscom.IsLead == false)
                {
                    response.IsRecreate = true;
                }
            }

            response.DocURL = HS.Framework.DESEncryptionDecryption.EncryptPlainTextToCipherText(
                $"{response.LeadId}#{response.AgreementDocumentHeight}#{response.FirstPage}#{response.TicketId}#{response.Recreate}#{response.IsInvoice}#{response.InvoiceId}#{response.IsEstimator}#{response.EstId}#{userid}#{commercial}#{response.EstimatorId}");

            if ((firstpage.HasValue && firstpage.Value) || (recreate.HasValue && recreate.Value))
            {
                var CustomerDetails = HSapiFacade.GetCustomerById(LeadId.HasValue ? LeadId.Value : 0);
                if (CustomerDetails != null)
                {
                    CustomerDetails.AgreementEmail = CustomerDetails.EmailAddress;
                    CustomerDetails.AgreementPhoneNo = !string.IsNullOrWhiteSpace(CustomerDetails.PrimaryPhone) ? CustomerDetails.PrimaryPhone :
                                                       !string.IsNullOrWhiteSpace(CustomerDetails.CellNo) ? CustomerDetails.CellNo :
                                                       !string.IsNullOrWhiteSpace(CustomerDetails.SecondaryPhone) ? CustomerDetails.SecondaryPhone : "";
                    CustomerDetails.Singature = "";
                    CustomerDetails.IsContractSigned = false;

                    HSapiFacade.DeleteAllSignatureByType(CustomerDetails.CustomerId, "Recreate");
                    var objfirstpage = HSapiFacade.GetFirstPageCustomerSignatureByCustomerId(CustomerDetails.CustomerId);
                    if (objfirstpage != null)
                    {
                        objfirstpage.Signature = "";
                        HSapiFacade.UpdateCustomerSignature(objfirstpage);
                    }
                    HSapiFacade.UpdateCustomer(CustomerDetails);

                    if (commercial == true)
                    {
                        var objcommercial = HSapiFacade.GetCommercialCustomerSignatureByCustomerId(CustomerDetails.CustomerId);
                        if (objcommercial != null)
                        {
                            objcommercial.Signature = "";
                            HSapiFacade.UpdateCustomerSignature(objcommercial);
                        }
                    }
                }
            }
            var smartInstallationAgreement = SmartInstallationAgreement(response.DocURL).Result;


            return smartInstallationAgreement;
        }

        #region Agreement
        public async Task<ApiResponse<SmartLeadsResponse>> SmartInstallationAgreement(string url)
        {
            int leadid = 0;
            int agreementtempid = 0;
            bool firstpage = false;
            bool commercial = false;
            int ticketid = 0;
            bool recreate = false;
            string invoiceid = "";
            string EstimatorId = "";
            bool isinvoice = false;
            bool isestimator = false;
            int estid = 0;
            Guid userid = Guid.Empty;
            bool isPublic = false;
            if (!string.IsNullOrWhiteSpace(url))
            {
                string[] spurl = DESEncryptionDecryption.DecryptCipherTextToPlainText(url).Split('#');
                if (spurl.Length == 8)
                {
                    leadid = Convert.ToInt32(spurl[0]);
                    if (!string.IsNullOrWhiteSpace(spurl[1]))
                    {
                        agreementtempid = Convert.ToInt32(spurl[1]);
                    }
                    firstpage = Convert.ToBoolean(spurl[2]);
                    ticketid = Convert.ToInt32(spurl[3]);
                    recreate = Convert.ToBoolean(spurl[4]);
                    invoiceid = Convert.ToString(spurl[6]);
                    isinvoice = Convert.ToBoolean(spurl[5]);
                    isPublic = Convert.ToBoolean(spurl[7]);
                }

                else if (spurl.Length == 13)
                {
                    leadid = Convert.ToInt32(spurl[0]);
                    if (!string.IsNullOrWhiteSpace(spurl[1]))
                    {
                        agreementtempid = Convert.ToInt32(spurl[1]);
                    }
                    firstpage = Convert.ToBoolean(spurl[2]);
                    ticketid = Convert.ToInt32(spurl[3]);
                    recreate = Convert.ToBoolean(spurl[4]);
                    invoiceid = Convert.ToString(spurl[6]);
                    isinvoice = Convert.ToBoolean(spurl[5]);
                    isPublic = Convert.ToBoolean(spurl[7]);
                    isestimator = Convert.ToBoolean(spurl[8]);
                    estid = Convert.ToInt32(spurl[9]);
                    userid = new Guid(spurl[10]);
                    commercial = Convert.ToBoolean(spurl[11]);
                    EstimatorId = Convert.ToString(spurl[12]);
                }
                else if (spurl.Length == 12)
                {
                    leadid = Convert.ToInt32(spurl[0]);
                    if (!string.IsNullOrWhiteSpace(spurl[1]))
                    {
                        agreementtempid = Convert.ToInt32(spurl[1]);
                    }
                    firstpage = Convert.ToBoolean(spurl[2]);
                    ticketid = Convert.ToInt32(spurl[3]);
                    recreate = Convert.ToBoolean(spurl[4]);
                    invoiceid = Convert.ToString(spurl[6]);
                    isinvoice = Convert.ToBoolean(spurl[5]);
                    isestimator = Convert.ToBoolean(spurl[7]);
                    estid = Convert.ToInt32(spurl[8]);
                    userid = new Guid(spurl[9]);
                    commercial = Convert.ToBoolean(spurl[10]);
                    EstimatorId = Convert.ToString(spurl[11]);
                }
                else
                {
                    leadid = Convert.ToInt32(spurl[0]);
                    if (!string.IsNullOrWhiteSpace(spurl[1]))
                    {
                        agreementtempid = Convert.ToInt32(spurl[1]);
                    }
                    firstpage = Convert.ToBoolean(spurl[2]);
                    ticketid = Convert.ToInt32(spurl[3]);
                    recreate = Convert.ToBoolean(spurl[4]);
                    invoiceid = Convert.ToString(spurl[6]);
                    isinvoice = Convert.ToBoolean(spurl[5]);
                    commercial = Convert.ToBoolean(spurl[6]);
                }
            }
            DateTime FixDate = DateTime.Now.UTCCurrentTime();
            InstallationAgreementModel Model = new InstallationAgreementModel();
            Model.EmergencyContactList = new List<EmergencyContact>();
            Model.ListAgreementAnswer = new List<AgreementAnswer>();
            Model.EquipmentList = new List<Equipment>();
            Model.InvoiceList = new List<InvoiceDetail>();
            Model.CustomerAgreement = new List<CustomerAgreement>();
            Model.SingleCustomerAgreement = new CustomerAgreement();
            var taxtotal = 0.0;
            Guid CompanyId = new Guid();
            if (User.Identity.IsAuthenticated)
            {
                CustomerCompany custommerCompany = HSapiFacade.GetCustomerCompanyByCustomerId(leadid);
                CompanyId = custommerCompany.CompanyId;

            }
            else
            {
                CustomerCompany custommerCompany = HSapiFacade.GetCustomerCompanyByCustomerId(leadid);
                CompanyId = custommerCompany.CompanyId;
            }
            Customer Cus = new Customer();
            CustomerExtended CusExd = new CustomerExtended();
            Company Com = new Company();
            if (leadid != 0)
            {
                if (!HSapiFacade.CustomerIsInCompany(leadid, CompanyId))
                {
                    return null;
                }

                Cus = HSapiFacade.GetCustomersById(leadid);
                if (Cus != null)
                {
                    CusExd = HSapiFacade.GetCustomerExtendedByCustomerId(Cus.CustomerId);
                }
                Com = HSapiFacade.GetCompanyByComapnyId(CompanyId);

                string ContractTerm = "";
                string ContractTermInWord = "";
                if (!string.IsNullOrWhiteSpace(Cus.ContractTeam) && Cus.ContractTeam != "-1")
                {
                    if (Cus.ContractTeam.ToLower() == "month to month")
                    {
                        ContractTerm = Cus.ContractTeam;
                        ContractTermInWord = Cus.ContractTeam;
                    }
                    else
                    {
                        ContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))).ToString();
                        ContractTermInWord = NumberToWords((Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))));
                    }

                }
                #region Agreement History
                var cusAgrHistory = HSapiFacade.GetCustomerAgreementHistory(Cus.CustomerId, "AgreementCreate");
                if (cusAgrHistory == null)
                {
                    CustomerAgreement cusAgrModel = new CustomerAgreement()
                    {
                        CompanyId = Com.CompanyId,
                        CustomerId = Cus.CustomerId,
                        IP = AppConfig.GetIP,
                        UserAgent = AppConfig.GetUserAgent,
                        Type = LabelHelper.CustomerAgreementHistory.AgreementCreate,
                        AddedDate = DateTime.UtcNow
                    };
                    HSapiFacade.InsertCustomerAgreement(cusAgrModel);
                }
                #endregion
                var objCusAgree = HSapiFacade.GetCustomerAgreementByComIdAndCusIsAndLoadAgreement(Com.CompanyId, Cus.CustomerId);
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
                    HSapiFacade.InsertCustomerAgreement(objCustomerAgreement);
                }
                var UpfrontAddOnTotal = 0.0;
                var UpfrontAddOnTotalPromo = 0.0;
                bool IsUpfrontPromo = false;
                bool IsServicePromo = false;
                var MonthlyServiceFeeTotal = 0.0;
                var TotalMonthlyMonitoring = 0.0;
                var NewSubTotal = 0.0;
                var TotalDueAtSigning = 0.0;
                var EquipmentTotalPrice = 0.0;
                double ServiceTotalPrice = 0.0;
                var AgreementSubtotal = 0.0;
                var AgreementTotal = 0.0;
                double ServiceTax = 0.0;
                var AgreementTax = 0.0;
                var NotARBEnabledTotalPrice = 0.0;
                string InstallTypeName = "";
                bool IsNonConfirming = false;
                var NonConfirmingFee = 0.0;
                var AdvanceServiceFeeTotal = 0.0;
                string contractCreatedDateVal = "";
                double discountamount = 0.00;
                if (CusExd.ContractCreatedDate != null)
                {
                    contractCreatedDateVal = CusExd.ContractCreatedDate.Value.ToString("M/d/yy");
                }
                else
                {
                    CusExd.ContractCreatedDate = DateTime.UtcNow;
                    HSapiFacade.UpdateCustomerExtended(CusExd);
                    contractCreatedDateVal = CusExd.ContractCreatedDate.Value.ToString("M/d/yy");
                }

                if (Cus.CreditScoreValue == null)
                {
                    Cus.CreditScoreValue = 0;
                }
                var PackageCustomer = HSapiFacade.GetPackageCustomerByCustomerId(Cus.CustomerId);
                if (PackageCustomer != null && PackageCustomer.NonConformingFee > 0 && (Cus.CreditScoreValue <= PackageCustomer.MinCredit || Cus.CreditScoreValue >= PackageCustomer.MaxCredit))
                {

                    IsNonConfirming = true;
                    NonConfirmingFee = PackageCustomer.NonConformingFee.Value;
                }
                else if (PackageCustomer != null && PackageCustomer.PackageId == Guid.Empty)
                {
                    IsNonConfirming = true;
                    NonConfirmingFee = PackageCustomer.NonConformingFee.Value;
                }

                var SmartPackageEquipmentServiceList = new List<SmartPackageEquipmentService>();
                var GetCityTaxList = HSapiFacade.GetCityTaxRate(Cus.CustomerId, Com.CompanyId);
                if (GetCityTaxList.Count > 0)
                {
                    foreach (var item1 in GetCityTaxList)
                    {
                        AgreementTax = item1.Rate;
                        ServiceTax = item1.Rate;
                    }
                }
                else
                {
                    Guid CustomerId = new Guid();
                    if (Cus != null)
                    {
                        CustomerId = Cus.CustomerId;
                    }
                    var GetSalesTax = HSapiFacade.GetSalesTax(Com.CompanyId, CustomerId);
                    if (GetSalesTax != null)
                    {
                        AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                        ServiceTax = Convert.ToDouble(GetSalesTax.Value);
                    }
                }
                #region Invoice
                Invoice invoice = new Invoice();
                CustomerProratedBill cpb = new CustomerProratedBill();
                cpb = HSapiFacade.GetCusProratedBillByCustomerId(Cus.CustomerId);
                List<InvoiceDetail> invoiceList = new List<InvoiceDetail>();
                string invDiagram = "";
                if (!string.IsNullOrWhiteSpace(invoiceid) && isinvoice == true)
                {
                    invoice = HSapiFacade.GetByInvoiceId(invoiceid);
                    if (invoice != null)
                    {
                        invDiagram = invoice.InvoiceContractDiagram;
                        invoiceList = HSapiFacade.GetInvoiceDetialsListByInvoiceId(invoice.InvoiceId);
                    }
                }
                #endregion
                #region Estimator
                Employee userInfo = new Employee();
                if (userid != Guid.Empty)
                {
                    userInfo = HSapiFacade.GetEmployeeByEmployeeId(userid);
                }
                CreateEstimator createest = new CreateEstimator();
                if (isestimator && estid > 0)
                {
                    CreateEstimator ca = new CreateEstimator();
                    ca.EstimatorSetting = new EstimatorSetting();
                    ca.Company = Com;

                    ca.Estimator = HSapiFacade.GetById(estid);
                    ca._EstimatorPDFFilter = HSapiFacade.GetEstimatorPdfFilterByComIdCusIdUserId(Com.CompanyId, userid, ca.Estimator.CustomerId);
                    ca.estimatorDetails = HSapiFacade.GetEstimatorDetailListByEstimatorId(ca.Estimator.EstimatorId);
                    ca.estimatorServices = HSapiFacade.GetEstimatorServicesByEstimatorId(ca.Estimator.EstimatorId);
                    if (ca.Estimator == null || ca.Estimator.CompanyId != CompanyId)
                    {
                        return null;
                    }
                    if ((ca.estimatorDetails == null || ca.estimatorDetails.Count() == 0) && (ca.estimatorServices == null || ca.estimatorServices.Count() == 0))
                    {
                        return null;
                    }
                    Customer tempCUstomer = HSapiFacade.GetCustomerByCustomerId(ca.Estimator.CustomerId);
                    if (tempCUstomer == null)
                    {
                        return null;
                    }

                    CreateEstimator processedModel = GetEstimatorModelById(ca.Estimator, ca.estimatorDetails, ca.estimatorServices, Com, tempCUstomer, ca._EstimatorPDFFilter, CompanyId);
                    Estimator estimator = HSapiFacade.GetEstimatorByEstimatorId(ca.Estimator.EstimatorId);
                    if (estimator != null)
                    {
                        //ViewBag.CoverLetter = estimator.CoverLetter;
                        processedModel.Estimator.CoverLetter = estimator.CoverLetter;
                        processedModel.Estimator.CoverLetterFile = estimator.CoverLetterFile;
                        processedModel.Estimator.ServicePlanType = estimator.ServicePlanType;
                        processedModel.Estimator.ServicePlanRate = estimator.ServicePlanRate;
                        processedModel.Estimator.ServicePlanAmount = estimator.ServicePlanAmount;
                        processedModel.Estimator.ServiceTaxAmount = estimator.ServiceTaxAmount;
                        processedModel.Estimator.ServiceTotalAmount = estimator.ServiceTotalAmount;
                        processedModel.Estimator.ShowServicePlan = estimator.ShowServicePlan;
                        processedModel.Estimator.ShowService = estimator.ShowService;
                        processedModel.Estimator.ServicePlanTypeName = "Service Plan";

                        var selectListItem = HSapiFacade.GetDropdownsByKey("ServicePlans").Where(x => x.Value == estimator.ServicePlanType).FirstOrDefault();
                        if (selectListItem != null)
                        {
                            processedModel.Estimator.ServicePlanTypeName = selectListItem.Text;
                        }

                    }
                    if (ca.estimatorDetails != null)
                    {
                        foreach (var item in ca.estimatorDetails)
                        {
                            Manufacturer Manufacturer = HSapiFacade.GetManufacturerByManufacturerId(item.ManufacturerId);
                            if (Manufacturer != null)
                            {
                                item.Manufacturer = Manufacturer.Name;
                            }
                            item.CreatedDate = DateTime.Now.UTCCurrentTime();
                            ca.SubTotal = ca.SubTotal + (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
                            item.EquipmentFile = HSapiFacade.GetEquipmentFilesByEquipmentIdAndFileType(item.EquipmentId, LabelHelper.EquipmentFileType.ProfilePicture).FirstOrDefault();
                            if (item.EquipmentFile == null)
                            {
                                item.EquipmentFile = new EquipmentFile();
                            }
                        }
                    }
                    if (ca.estimatorServices != null)
                    {
                        foreach (var item in ca.estimatorServices)
                        {
                            processedModel.ServiceSubTotal += ca.ServiceSubTotal + item.Amount;
                        }
                        processedModel.TotalServiceAmount = processedModel.ServiceSubTotal + ca.ServiceTax;
                    }


                    createest = processedModel;
                    createest.eSecurityLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/eSecurity_logo.png");
                    createest.specializedLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/specialized_property_management_logo.png");
                    string EstimatorContractTerm = "";
                    if (!string.IsNullOrWhiteSpace(createest.Estimator.ContractTerm) && createest.Estimator.ContractTerm != "-1")
                    {
                        if (createest.Estimator.ContractTerm.ToLower() == "month to month")
                        {
                            EstimatorContractTerm = createest.Estimator.ContractTerm;
                        }
                        else
                        {
                            EstimatorContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(createest.Estimator.ContractTerm) * 12))).ToString() + " month";
                        }

                    }
                    createest.EstimatorContractTerm = EstimatorContractTerm;
                }
                #endregion
                var CustomEquipmentList = new List<Equipment>();
                if (firstpage == true || recreate == true)
                {
                    CustomEquipmentList = HSapiFacade.GetSmartEquipmentListByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId, firstpage, ticketid);
                    if (CustomEquipmentList.Count > 0)
                    {
                        foreach (var item in CustomEquipmentList)
                        {
                            EquipmentTotalPrice += item.Total;
                            UpfrontAddOnTotal += item.Total;
                        }
                    }
                }
                else if (!string.IsNullOrWhiteSpace(EstimatorId))
                {
                    CustomEquipmentList = HSapiFacade.GetSmartEquipmentEstimatorListByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId, firstpage, ticketid, EstimatorId);
                    if (CustomEquipmentList.Count > 0)
                    {
                        foreach (var item in CustomEquipmentList)
                        {
                            EquipmentTotalPrice += item.Total;
                            UpfrontAddOnTotal += item.Total;
                        }
                    }
                }
                else
                {
                    CustomEquipmentList = HSapiFacade.GetSmartEquipmentListByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId, firstpage, ticketid);
                    if (CustomEquipmentList.Count > 0)
                    {
                        foreach (var item in CustomEquipmentList)
                        {
                            EquipmentTotalPrice += item.Total;
                            UpfrontAddOnTotal += item.Total;
                        }
                    }
                }
                var CustomEstimatorServiceList = new List<Estimator>();
                var CustomServiceList = new List<Equipment>();
                if (firstpage == true || recreate == true)
                {
                    CustomServiceList = HSapiFacade.GetSmartServiceListByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId, firstpage, ticketid);
                    if (CustomServiceList.Count > 0)
                    {
                        foreach (var item in CustomServiceList)
                        {
                            EquipmentTotalPrice += item.Total;
                            ServiceTotalPrice += item.Total;
                            MonthlyServiceFeeTotal += item.Total;
                        }
                    }

                }
                else if (!string.IsNullOrWhiteSpace(EstimatorId))
                {
                    CustomServiceList = HSapiFacade.GetSmartEstimatorServiceListByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId, firstpage, ticketid, EstimatorId);
                    if (CustomServiceList != null)
                    {
                        var IsRecurringList = CustomServiceList.Where(x => x.IsARBEnabled == true).ToList();
                        if (IsRecurringList != null && IsRecurringList.Count > 0)
                        {
                            foreach (var item in IsRecurringList)
                            {
                                EquipmentTotalPrice += item.Total;
                                ServiceTotalPrice += item.Total;
                                MonthlyServiceFeeTotal += item.Total;
                            }
                        }
                    }
                }
                else
                {
                    CustomServiceList = HSapiFacade.GetSmartServiceListByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId, firstpage, ticketid);
                    if (CustomServiceList.Count > 0)
                    {
                        foreach (var item in CustomServiceList)
                        {
                            EquipmentTotalPrice += item.Total;
                            ServiceTotalPrice += item.Total;
                            MonthlyServiceFeeTotal += item.Total;
                        }
                    }
                }
                var NotARBEnabledServiceList = new List<Equipment>();
                if (firstpage == true || recreate == true || commercial == true)
                {
                    NotARBEnabledServiceList = HSapiFacade.GetNotARBEnabledSmartServiceListByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId, firstpage, ticketid);
                    if (NotARBEnabledServiceList.Count > 0)
                    {
                        foreach (var item in NotARBEnabledServiceList)
                        {
                            NotARBEnabledTotalPrice += item.Total;
                        }
                    }
                }
                else if (!string.IsNullOrWhiteSpace(EstimatorId))
                {
                    NotARBEnabledServiceList = HSapiFacade.GetSmartEstimatorServiceListByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId, firstpage, ticketid, EstimatorId);
                    var onetimeserviceList = NotARBEnabledServiceList.Where(x => x.IsARBEnabled == false).ToList();
                    if (onetimeserviceList != null)
                    {
                        if (onetimeserviceList.Count > 0)
                        {
                            foreach (var item in onetimeserviceList)
                            {
                                NotARBEnabledTotalPrice += item.Total;
                            }
                        }
                    }

                }
                else
                {
                    NotARBEnabledServiceList = HSapiFacade.GetNotARBEnabledSmartServiceListFromService(Cus.CustomerId, CompanyId);
                    if (NotARBEnabledServiceList.Count > 0)
                    {
                        foreach (var item in NotARBEnabledServiceList)
                        {
                            NotARBEnabledTotalPrice += item.Total;
                        }
                    }
                }
                #region Advance Monitoring Service Month

                PaymentInfoCustomer paycus = new PaymentInfoCustomer();
                paycus = HSapiFacade.GetPaymentInfoCustomerByCustomerIdAndPayForService(Cus.CustomerId);


                int ForMonth = 1;
                if (paycus != null && paycus.ForMonths.HasValue)
                {
                    ForMonth = paycus.ForMonths.Value;
                }
                if (ForMonth > 1)
                {
                    AdvanceServiceFeeTotal = MonthlyServiceFeeTotal * (ForMonth - 1);

                }
                #endregion
                Cus.MonthlyMonitoringFee = Convert.ToString(ServiceTotalPrice);
                TotalMonthlyMonitoring = MonthlyServiceFeeTotal;
                NewSubTotal = TotalMonthlyMonitoring + UpfrontAddOnTotal;
                GlobalSetting glbsFee = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "HasLabourFee");
                if (CustomServiceList.Count > 0 || CustomEquipmentList.Count > 0)
                {
                    if (PackageCustomer != null && PackageCustomer.ActivationFee.HasValue && PackageCustomer.LabourFee.HasValue)
                    {
                        if (glbsFee != null && glbsFee.Value != "true")
                        {
                            PackageCustomer.LabourFee = 0;
                        }
                        else
                        {
                            PackageCustomer.LabourFee = PackageCustomer.LabourFee != null ? PackageCustomer.LabourFee : 0.0;
                        }
                        AgreementSubtotal = PackageCustomer.ActivationFee.Value + EquipmentTotalPrice + PackageCustomer.LabourFee.Value;
                        NewSubTotal = PackageCustomer.ActivationFee.Value + EquipmentTotalPrice + PackageCustomer.LabourFee.Value;
                    }
                    else if (PackageCustomer != null && PackageCustomer.ActivationFee.HasValue)
                    {
                        AgreementSubtotal = PackageCustomer.ActivationFee.Value + EquipmentTotalPrice;
                        NewSubTotal = PackageCustomer.ActivationFee.Value + EquipmentTotalPrice;
                    }
                    else
                    {
                        AgreementSubtotal = EquipmentTotalPrice;
                        NewSubTotal = EquipmentTotalPrice;
                    }
                }
                if (IsNonConfirming && NonConfirmingFee > 0)
                {
                    AgreementSubtotal = AgreementSubtotal + NonConfirmingFee;
                    NewSubTotal = NewSubTotal + NonConfirmingFee;
                }
                if (!string.IsNullOrWhiteSpace(EstimatorId))
                {
                    var data = HSapiFacade.GetSmartEstimatorServiceListByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId, firstpage, ticketid, EstimatorId);
                    if (data != null)
                    {
                        var totalestimatortax = data.Select(x => x.TotalestimatorTaxAmount).FirstOrDefault();
                        taxtotal = totalestimatortax;
                    }
                }
                else
                {
                    if (AgreementTax != 0.0)
                    {
                        double SubtotalForTax = 0.0;
                        if (glbsFee != null && glbsFee.Value == "true" && PackageCustomer.LabourFee.HasValue)
                        {
                            SubtotalForTax = AgreementSubtotal - PackageCustomer.LabourFee.Value;
                        }
                        else
                        {
                            SubtotalForTax = AgreementSubtotal;
                        }

                        taxtotal = (SubtotalForTax / 100) * AgreementTax;
                        Model.TaxTotal = taxtotal;
                        AgreementTotal = AgreementSubtotal + taxtotal;
                        TotalDueAtSigning = NewSubTotal + taxtotal;
                    }
                    else
                    {
                        Model.TaxTotal = 0.0;
                        AgreementTotal = AgreementSubtotal;
                        TotalDueAtSigning = NewSubTotal;
                    }
                    /// "Mayur" Activation total with tax calculation :Start
                    if (ServiceTax != 0.0)
                    {
                        taxtotal = (ServiceTotalPrice / 100) * ServiceTax;
                        // MonthlyServiceFeeTotal = ServiceTotalPrice + taxtotal;
                    }
                    else
                    {
                    }
                }


                /// "Mayur" Activation total with tax calculation :End

                var PackageCustomerDetails = HSapiFacade.GetPackageCustomerByCustomerIdandCompanyId(Cus.CustomerId, CompanyId);
                if (PackageCustomerDetails != null)
                {
                    InstallTypeName = HSapiFacade.SmartInstallTypeNameByInstallTypeId(Convert.ToInt32(PackageCustomerDetails.SmartInstallTypeId));
                    if (string.IsNullOrWhiteSpace(InstallTypeName))
                    {
                        InstallTypeName = "New Install";
                    }
                    SmartPackageEquipmentServiceList = HSapiFacade.GetAllSmartPackageIncludeEquipmentByPackageIdAndCompanyId(PackageCustomerDetails.PackageId, CompanyId);
                }
                var PaymentDetails = HSapiFacade.GetAllPaymentInfoByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId).Where(m => m.PayFor == "First Month").FirstOrDefault();
                var emercontact = HSapiFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId);
                string overviewheader = "";
                string overviewdata = "";
                if (emercontact != null && emercontact.Count > 0)
                {
                    overviewheader = @"<table style='border-collapse:collapse; width:100%;font-family:Arial;table-layout:fixed;font-size:13px;margin-top:20px;'>
                                                          <thread>
                                            <tr style='height:30px;'>
                                  <th style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'>Name</th>
   
                                   <th style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'> RelationShip </th>
    
                                    <th style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'> Phone </th>
     
                                     <th style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'> Type </th>
      
                                  </tr>
                                            </thread>
                                            <tbody>
                                            {0}
                                            </tbody>
                                        </table>";
                    foreach (var item in emercontact)
                    {
                        overviewdata += @"<tr style='height: 30px;'>
                                  <td style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'>" + item.FirstName + " " + item.LastName + @"</td>
   
                                   <td style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'>" + item.RelationShip + @"</td>
    
                                    <td style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'>" + item.Phone + @"</td>
     
                                     <td style ='border: 2px solid #000; font-weight:bold; width:45%; padding-left:10px;'>" + item.PhoneType + @"</td>
      
                                  </tr> ";
                    }
                }
                var agreementPayment = HSapiFacade.GetLeadAgreementPaymentInfoByCustomerId(Cus.CustomerId);
                string paymentoverviewheader = "";
                string paymentoverviewdata = "";
                if (agreementPayment != null && agreementPayment.Count > 0)
                {
                    paymentoverviewheader = "<table style='border-collapse:collapse; width:100%; font-family:Arial; table-layout:fixed; font-size:13px;'>{0}</table>";
                    foreach (var pay in agreementPayment)
                    {
                        var sppay = pay.Type.Split('_');
                        if (sppay.Length > 0)
                        {
                            if (sppay[0] == "CC")
                            {
                                var cardNumber = pay.CardNumber.Replace('-', ' ').Replace(" ", "");
                                if (cardNumber.Length == 16)
                                {
                                    paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Card Number: " + string.Concat("".PadLeft(12, '*'), cardNumber.Substring(cardNumber.Length - 4)) + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Expire Date: " + pay.CardExpireDate + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Security Code: " + string.Concat("".PadLeft(2, '*'), pay.CardSecurityCode.Substring(pay.CardSecurityCode.Length - 1)) + @"</td>
                                                        </tr>";
                                }
                                else
                                {
                                    paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Card Number: " + string.Concat("".PadLeft(11, '*'), cardNumber.Substring(cardNumber.Length - 4)) + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Expire Date: " + pay.CardExpireDate + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Security Code: " + string.Concat("".PadLeft(2, '*'), pay.CardSecurityCode.Substring(pay.CardSecurityCode.Length - 1)) + @"</td>
                                                        </tr>";
                                }
                            }
                            else if (sppay[0] == "ACH" && pay.AcountNo.Length > 4)
                            {
                                paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Type: " + pay.BankAccountType + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Routing No: " + pay.RoutingNo + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account No: " + string.Concat("".PadLeft(pay.AcountNo.Length - 4, '*'), pay.AcountNo.Substring(pay.AcountNo.Length - 4)) + @"</td>
                                                        </tr>";
                            }
                        }
                    }
                }

                var CustomerAddress = AddressHelper.MakeAddress(Cus);
                var CustomerInstallAddress = AddressHelper.MakeInstallAddress(Cus);
                CustomerSignature cs = new CustomerSignature();
                GlobalSetting glbs = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "CompanySignature");
                string cusSignature = "";
                string cussignDate = "";
                DateTime cussignDateVal = new DateTime();
                if (firstpage == true || recreate == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "First Page");
                    if (cs != null && isPublic == false)
                    {
                        cs.Signature = "";
                    }
                }
                else if (commercial == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Commercial");
                    if (cs != null && isPublic == false)
                    {
                        cs.Signature = "";
                    }
                }
                else if (isinvoice == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Estimate");
                    if (cs != null && isPublic == false)
                    {
                        cs.Signature = "";
                    }
                }
                else if (isestimator == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Estimator");
                    if (cs != null && isPublic == false)
                    {
                        cs.Signature = "";
                    }
                }
                else if (recreate == true)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Recreate");
                    if (cs != null && isPublic == false)
                    {
                        cs.Signature = "";
                    }
                }
                else if (Cus != null && agreementtempid != 0)
                {
                    cs = HSapiFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Agreement File");
                    if (cs != null && isPublic == false)
                    {
                        cs.Signature = "";
                    }
                }
                else
                {
                    cusSignature = Cus.Singature;
                    if (Cus.CustomerSignatureDate != null && Cus.CustomerSignatureDate != new DateTime())
                    {
                        cussignDate = Cus.CustomerSignatureDate.Value.UTCToClientTime().ToString("M/dd/yy");
                        cussignDateVal = Cus.CustomerSignatureDate.Value.UTCToClientTime();
                    }

                }
                if (cs != null && (agreementtempid != 0 || firstpage == true || recreate == true || commercial == true))
                {
                    cusSignature = cs.Signature;
                    if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                    {
                        cussignDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        cussignDateVal = cs.CreatedDate.UTCToClientTime();
                    }

                }

                Double MMR = 1;
                Double CTerm = 0;

                double.TryParse(Cus.MonthlyMonitoringFee, out MMR);
                double.TryParse(ContractTerm, out CTerm);
                Double TotalPayments = MMR * CTerm;
                #region Labour fee
                if (glbsFee != null && glbsFee.Value != "true")
                {
                    if (PackageCustomer != null)
                    {
                        PackageCustomer.LabourFee = 0.0;
                    }
                }
                else
                {
                    PackageCustomer.LabourFee = PackageCustomer.LabourFee != null ? PackageCustomer.LabourFee : 0.0;
                }
                #endregion


                #region For Promo Pyment Method
                List<PaymentInfoCustomer> paycusList = new List<PaymentInfoCustomer>();
                PaymentProfileCustomer paymentProfile = new PaymentProfileCustomer();
                paycusList = HSapiFacade.GetAllPaymentInfoCustomerByCustomerId(Cus.CustomerId);
                if (paycusList != null && paycusList.Count > 0)
                {
                    foreach (var item in paycusList)
                    {
                        paymentProfile = HSapiFacade.GetPaymentProfileByPaymentInfoId(item.PaymentInfoId);
                        if (paymentProfile != null && paymentProfile.Type == LabelHelper.PaymentMethod.Promo)
                        {
                            if (item.Payfor == "Activation Fee")
                            {
                                NonConfirmingFee = 0.0;

                                if (PackageCustomer != null)
                                {
                                    PackageCustomer.ActivationFee = 0.0;
                                }
                                NewSubTotal = NewSubTotal - (PackageCustomer.AdditionFee + NonConfirmingFee);
                            }
                            else if (item.Payfor == "Equipment")
                            {
                                IsUpfrontPromo = true;
                                NewSubTotal = NewSubTotal - UpfrontAddOnTotal;
                            }
                            else if (item.Payfor == "Service")
                            {
                                NewSubTotal = NewSubTotal - TotalMonthlyMonitoring;
                                IsServicePromo = true;


                            }


                        }

                    }

                }
                #endregion

                #region LeadSource
                string LeadSource = "";
                Lookup leadsource = HSapiFacade.GetLookupByKeyAndValueAndCompanyId("LeadSource", Cus.LeadSource, Com.CompanyId);
                LeadSource = leadsource != null && !string.IsNullOrWhiteSpace(leadsource.DisplayText) && leadsource.DataValue != "-1" ? leadsource.DisplayText : "";
                #endregion


                Model = new InstallationAgreementModel()
                {
                    EstimatorId = EstimatorId,
                    CSIDNumber = Cus.Id,
                    LeadSource = LeadSource,
                    IsNonConfirming = IsNonConfirming,
                    NonConfirmingFee = NonConfirmingFee,
                    InstallDate = (Cus.OriginalContactDate != null && Cus.OriginalContactDate.HasValue) ? Cus.OriginalContactDate.Value.ToShortDateString() : "",
                    OriginalContactDate = (Cus.OriginalContactDate != null && Cus.OriginalContactDate.HasValue) ? Cus.OriginalContactDate.Value : new DateTime(),
                    AccountType = Cus.Type,
                    ContractType = CusExd != null && !string.IsNullOrWhiteSpace(CusExd.ContractType) ? CusExd.ContractType : "",
                    Referredby = Cus.ReferringCustomer != Guid.Empty ? HSapiFacade.GetCustomerNameById(Cus.ReferringCustomer) : "",
                    SocialSecurityNumber = Cus.SSN,
                    Owner2ndPhone = Cus.PrimaryPhone,
                    InitialStreet = Cus.Street,
                    InitialCity = Cus.City,
                    InitialCountry = Cus.County,
                    InitialState = Cus.State,
                    InitialZip = Cus.ZipCode,
                    InitialApt = Cus.Appartment,
                    BillingCity = Cus.CityPrevious,
                    BillingState = Cus.StatePrevious,
                    BillingZip = Cus.ZipCodePrevious,
                    BillingCountry = Cus.CountryPrevious,
                    BillingStreet = Cus.StreetPrevious,
                    InstallTypeName = InstallTypeName,
                    SmartPackageEquipmentServiceList = SmartPackageEquipmentServiceList,
                    UpfrontAddOnTotal = UpfrontAddOnTotal,
                    UpfrontAddOnTotalPromo = UpfrontAddOnTotalPromo,
                    IsUpfrontPromo = IsUpfrontPromo,
                    IsServicePromo = IsServicePromo,
                    MonthlyServiceFeeTotal = MonthlyServiceFeeTotal,
                    TotalMonthlyMonitoring = TotalMonthlyMonitoring,
                    NewSubTotal = NewSubTotal,
                    TotalDueAtSigning = TotalDueAtSigning,
                    PaymentDetails = PaymentDetails != null ? PaymentDetails : new PaymentInfo(),
                    DisplayName = Cus.DisplayName,
                    BillingAddress = CustomerAddress,
                    OwnerAddress = CustomerAddress,
                    InstallAddress = CustomerInstallAddress,
                    OwnerEmail = Cus.EmailAddress,
                    OwnerPhone = Cus.CellNo,
                    OwnerName = Cus.FirstName + " " + Cus.LastName,
                    FirstName = Cus.FirstName,
                    LastName = Cus.LastName,
                    MiddleName = Cus.MiddleName,
                    EmergencyContactList = HSapiFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(Cus.CustomerId, CompanyId),
                    CompanyName = Com.CompanyName,
                    CompanySate = string.Format("{0}, {1} {2}", Com.City, Com.State, Com.ZipCode),
                    CompanyStreet = Com.Street,
                    CompanyWebsite = Com.Website,
                    SubscribedMonths = ContractTerm,
                    SubscribedMonthsInWord = ContractTermInWord,
                    RenewalMonths = Cus.RenewalTerm.HasValue ? Cus.RenewalTerm.Value : 0,
                    Password = Cus.Passcode,
                    DateOfTransaction = FixDate.UTCToClientTime(),
                    CompanyLogo = HSapiFacade.GetCompanyEmailLogoByCompanyId(CompanyId),
                    EquipmentList = CustomEquipmentList.ToList(),
                    ServiceList = CustomServiceList.ToList(),
                    ActivationFee = (PackageCustomer != null && PackageCustomer.ActivationFee.HasValue) ? PackageCustomer.ActivationFee.Value : 0,
                    LabourFee = (PackageCustomer != null && PackageCustomer.LabourFee.HasValue) ? PackageCustomer.LabourFee.Value : 0,
                    BusinessName = Cus.BusinessName,
                    MonthlyMonitoringFee = Cus.MonthlyMonitoringFee,
                    EffectiveDate = FixDate.UTCToClientTime(),
                    CustomerSignature = cusSignature,
                    CustomerSignatureStringDate = cussignDate,
                    CustomerSignatureStringDateVal = cussignDateVal,
                    ContractCreatedDateVal = (CusExd.ContractStartDate != null && CusExd.ContractStartDate.HasValue) ? CusExd.ContractStartDate.Value.ToShortDateString() : DateTime.Now.ToShortDateString(),
                    CustomerAgreement = HSapiFacade.GetCustomerAgreementByCompanyIdAndCustomerId1(CompanyId, Cus.CustomerId),
                    Subtotal = AgreementSubtotal,
                    Tax = AgreementTax,
                    TaxTotal = taxtotal,
                    Total = AgreementTotal,
                    EContractId = Cus.Id,
                    ListAgreementAnswer = HSapiFacade.GetAllAgreementAnswerByCustomerId(Cus.CustomerId),
                    SalesRepresentative = !string.IsNullOrEmpty(Cus.Soldby) ? HSapiFacade.GetEmployeeNumByEmployeeId(new Guid(Cus.Soldby)).ToString() : "",
                    TotalPayments = TotalPayments,
                    SingleCustomerAgreement = HSapiFacade.GetIpAndUserAgentByCustomerIdAndCompanyId(CompanyId, Cus.CustomerId),
                    ListContactEmergency = string.Format(overviewheader, overviewdata),
                    ListPaymentInfo = string.Format(paymentoverviewheader, paymentoverviewdata),
                    DoingBusinessAs = Cus.DBA,
                    DispalyName = Cus.DisplayName,
                    CompanyPhone = Com.Phone,
                    FirstPage = firstpage,
                    Commercial = commercial,
                    Recreate = recreate,
                    IsInvoice = isinvoice,
                    InvoiceId = invoiceid,
                    InvoiceDiagram = invDiagram,
                    InvoiceList = invoiceList,
                    IsEstimator = isestimator,
                    createEst = createest,
                    userInfo = userInfo,
                    inv = invoice,
                    NotARBEnabledServiceList = NotARBEnabledServiceList.ToList(),
                    NotARBEnabledTotalPrice = NotARBEnabledTotalPrice,
                    ProratedAmout = cpb != null ? Math.Round(cpb.Amount, 2, MidpointRounding.AwayFromZero) : 0.0,
                    FinancedAmout = Cus != null && Cus.FinancedAmount != null ? Math.Round(Cus.FinancedAmount.Value, 2, MidpointRounding.AwayFromZero) : 0.0,
                    MonthlyFinanceRate = CusExd != null && CusExd.MonthlyFinanceRate != null ? Math.Round(CusExd.MonthlyFinanceRate.Value, 2, MidpointRounding.AwayFromZero) : 0.0,
                    AdvanceServiceFeeTotal = AdvanceServiceFeeTotal
                };
                if (agreementtempid != 0)
                {
                    if (cs != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(cs.Signature))
                    {
                        Model.CompanySignature = glbs.Value;
                        if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                        {
                            Model.CompanySignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                }
                else if (firstpage == true || recreate == true || commercial == true)
                {
                    if (glbs != null && !string.IsNullOrWhiteSpace(glbs.Value))
                    {
                        Model.CompanySignature = glbs.Value;
                        Model.CompanySignatureDate = cussignDate;
                    }
                }
                else
                {
                    if (Cus != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(Cus.Singature))
                    {
                        Model.CompanySignature = glbs.Value;
                        if (Cus.CustomerSignatureDate != null && Cus.CustomerSignatureDate != new DateTime())
                        {
                            Model.CompanySignatureDate = Cus.CustomerSignatureDate.Value.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                }

            }
            else
            {
                Model.CompanyLogo = HSapiFacade.GetCompanyLogoForPDFByCompanyId(CompanyId);
            }


            Model.KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG");
            //ViewBag.CompanyId = Com.CompanyId.ToString();
            Model.CompanyId = Com.CompanyId.ToString();
            Model.CurrentCurrency = HSapiFacade.GetCurrentCurrencyByCompanyId(Com.CompanyId);

            string body = HSapiFacade.MakeSmartAgreementPdf(Model, agreementtempid);


            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
        ColorMode = ColorMode.Color,
        Orientation = Orientation.Portrait,
        PaperSize = PaperKind.A4,
    },
                Objects = {
        new ObjectSettings() {
            HtmlContent = body,
        }
    }
            };

            byte[] pdf = null;
            try
            {
                pdf = Converter.Convert(doc);
            }
            catch (Exception ex)
            {
                throw new Exception("Error during PDF generation", ex);
            }


            #region File Save on AWS S3

            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.LeadToCustomerAgreement"];
            filename = filename.TrimEnd('/');

            var comname = HSapiFacade.GetCompanyByComapnyId(Com.CompanyId).CompanyName.ReplaceSpecialChar();
            string FilePath = string.Format(filename, comname);
            string FileName = rand.Next().ToString() + leadid + "AgreementMail.pdf";
            string FileKey = string.Format($"{FilePath}/{FileName}");

            var task = Task.Run(async () =>
            {
                AWSS3ObjectService AWSobject = new AWSS3ObjectService();


                await AWSobject.UploadFile(FileKey, pdf);


                await AWSobject.MakePublic(FileName, FilePath);
            });

            task.Wait();

            string returnUrl = string.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName) + FileKey;

            doc = null;
            pdf = null;

            GC.Collect();

            #endregion

            /// <summary>
            /// This method kills any stuck wkhtmltopdf processes still running in memory
            /// </summary>


            return ApiResponse<SmartLeadsResponse>.Success(new SmartLeadsResponse { url = returnUrl });


            //}
            //return ApiResponse<SmartLeadsResponse>.Error("Invalid URL or insufficient data in URL");
        }

        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            words = textInfo.ToTitleCase(words);
            return words;
        }
        private CreateEstimator GetEstimatorModelById(Estimator Invoice, List<EstimatorDetail> InvoiceDetialList, List<EstimatorService> EstimatorServiceList, Company tempCom, Customer tempCUstomer, EstimatorPDFFilter EstimatorPDFFilters, Guid comid)
        {
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateEstimator Model = new CreateEstimator();
            Model.Estimator = Invoice;
            Model.estimatorDetails = InvoiceDetialList;
            Model.estimatorServices = EstimatorServiceList;
            Model.Estimator.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;

            //  Model.Invoice.IsEstimate = false;


            //Model.Invoice.InvoiceDate = Invoice.InvoiceDate.HasValue ? Invoice.InvoiceDate.Value : Model.Invoice.InvoiceDate.Value.ClientToUTCTime();
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.EstimateTerm) && Model.Invoice.EstimateTerm != "-1")
            //    Model.Invoice.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(Model.Invoice.EstimateTerm);
            //Model.Invoice.DueDate = Invoice.DueDate.HasValue ? Invoice.DueDate.Value : Model.Invoice.DueDate.Value.ClientToUTCTime();
            #region Discount Calculation 
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.DiscountType))
            //{
            //    if (Model.Invoice.DiscountType == "amount")
            //    {
            //        if (Invoice.Discountpercent != null)
            //        {
            //            Model.Discount = Invoice.Discountpercent.Value;
            //        }
            //    }
            //    else
            //    {
            //        if (Invoice.Discountpercent != null)
            //        {
            //            Model.Discount = ((Invoice.Discountpercent / 100) * Model.SubTotal).Value;
            //        }
            //    }
            //}
            #endregion

            #region making Name of Address Bold
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.BillingAddress))
            //{
            //    var split = Model.Invoice.BillingAddress.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.Invoice.BillingAddress = NewAddress;
            //    }
            //}
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.ShippingAddress))
            //{
            //    var split = Model.Invoice.ShippingAddress.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.Invoice.ShippingAddress = NewAddress;
            //    }
            //}
            //if (!string.IsNullOrWhiteSpace(Model.InvoiceShipping))
            //{
            //    var split = Model.InvoiceShipping.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.InvoiceShipping = NewAddress;
            //    }
            //}
            #endregion

            //customer name is customer business name here 
            if (tempCUstomer.BusinessName != "")
            {
                Model.CustomerName = tempCUstomer.BusinessName;
            }
            else
            {
                Model.CustomerName = Model.Estimator.CustomerName;
            }
            Model.CusBussinessName = tempCUstomer.BusinessName;

            Model.SubTotal = 0;
            if (Model.estimatorDetails != null)
            {
                foreach (var item in Model.estimatorDetails)
                {
                    //   item.CreatedBy = User.Identity.Name;
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    //   item.CompanyId = CurrentUser.CompanyId.Value;
                    Model.SubTotal = Model.SubTotal + item.TotalPrice.Value;
                }
            }

            //if (string.IsNullOrWhiteSpace(Model.Invoice.InvoiceMessage))
            //{
            //    Model.Invoice.InvoiceMessage = _Util.Facade.GlobalSettingsFacade.GetInvoiceMessageByCompanyId(CurrentUser.CompanyId.Value);
            //}
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
            #region Company Info
            Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
            Model.CompanyCity = tempCom.City.UppercaseFirst();
            Model.CompanyState = tempCom.State;
            Model.CompanyZip = tempCom.ZipCode;
            Model.CompanyPhone = tempCom.Phone;
            Model.CompanyEmail = tempCom.EmailAdress;
            Model.CompanyName = tempCom.CompanyName;
            Model.PhoneNo = tempCom.Phone;
            Model.CompanyWebsite = tempCom.Website;
            #endregion
            #region Customer Info
            Model.CustomerInfo = HSapiFacade.GetCustomerAddressFormat(comid);
            Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
            Model.CustomerState = tempCUstomer.State;
            Model.CustomerZipCode = tempCUstomer.ZipCode;
            Model.CustomerNo = tempCUstomer.CustomerNo;
            Model.CustomerStreet = tempCUstomer.Street;
            Model.CompanyInfo = HSapiFacade.GetCompanyAddressFormat(comid);
            //ViewBag.ShippingValue = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(tempCom.CompanyId);
            #endregion
            Model._EstimatorPDFFilter = EstimatorPDFFilters;

            Model.ShowEstimatorShippingAddress = HSapiFacade.GetShippingSettingCompanyId(comid).ToLower() == "true" ? true : false;

            if (string.IsNullOrWhiteSpace(tempCom.CompanyLogo))
            {
                tempCom.CompanyLogo = HSapiFacade.GetCompanyLogoForPDFByCompanyId(comid);
            }
            Model.CompanyLogo = tempCom.CompanyLogo;
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.DiscountType))
            //{
            //    if (Model.Invoice.DiscountType != "amount")
            //    {
            //        if (Model.Invoice.Discountpercent.HasValue && Model.Invoice.Discountpercent.Value > 0)
            //        {
            //            Model.Discount = ((Model.Invoice.Discountpercent / 100) * Model.SubTotal).Value;
            //        }
            //    }
            //}
            //if (Model.Invoice.BalanceDue > 0)
            //{
            //    Model.AmountInWord = NumberToWords(Model.Invoice.BalanceDue.Value); //NumberToWords(Model.Invoice.TotalAmount.Value);
            //}
            return Model;
        }

        #endregion

        [Route("surveys")]
        [HttpGet]
        public HttpResponseMessage Surveys()
        {
            Guid ticketid = new Guid();
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            try
            {
                JobsCustomModel model = new JobsCustomModel();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    List<CustomSurvey> surveyUser = HSapiFacade.GetAllCustomSurvey();
                    var surveyList = surveyUser.Select(x => new SurveyItem
                    {
                        name = x.SurveyName,
                        guid = x.SurveyId
                    }).ToList();

                    var sortedSurveyList = surveyList.OrderBy(x => x.name != "Select One").ThenBy(x => x.name).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, new ApiResponse<object>(sortedSurveyList, true, null));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, ApiResponse<object>.Error("User not authenticated."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ApiResponse<object>.Error(ex.Message));
            }
        }
        [Route("send-survey")]
        [HttpPost]
        public HttpResponseMessage SurveysSend()
        {
            DateTime date = new DateTime();
            string PageUrl = "";
            string Action = "";
            string ActionDisplyText = "";
            string UserIp = "";
            string UserAgent = "";
            string UserName = "";
            string UserId = "";
            string CustomerId = "";
            string startTime = "";
            string endTime = "";
            Guid ticketid = new Guid();
            Guid surveyId = new Guid();
            Guid customerId = new Guid();
            var re = Request;
            var headers = re.Headers;
            string email = headers.Contains("email") ? headers.GetValues("email").FirstOrDefault() : null;
            string phone = headers.Contains("phone") ? headers.GetValues("phone").FirstOrDefault() : null;
            #region Log Header
            if (headers.Contains("pageurl"))
            {
                PageUrl = headers.GetValues("pageurl").First();
            }
            if (headers.Contains("action"))
            {
                Action = headers.GetValues("action").First();
            }
            if (headers.Contains("actiondisplytext"))
            {
                ActionDisplyText = headers.GetValues("actiondisplytext").First();
            }
            if (headers.Contains("userip"))
            {
                UserIp = headers.GetValues("userip").First();
            }
            if (headers.Contains("useragent"))
            {
                UserAgent = headers.GetValues("useragent").First();
            }
            if (headers.Contains("username"))
            {
                UserName = headers.GetValues("username").First();
            }
            if (headers.Contains("userid"))
            {
                UserId = headers.GetValues("userid").First();
            }
            if (headers.Contains("customerid"))
            {
                CustomerId = headers.GetValues("customerid").First();
            }
            #endregion
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            if (headers.Contains("surveyId"))
            {
                Guid.TryParse(headers.GetValues("surveyId").First(), out surveyId);
            }
            if (headers.Contains("customerId"))
            {
                Guid.TryParse(headers.GetValues("customerId").First(), out customerId);
            }
            string message;
            bool result;
            try
            {
                JobsCustomModel model = new JobsCustomModel();
                var identity = (ClaimsIdentity)User.Identity;
                var username = identity.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
                var userIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "userid")?.Value;
                var userId = Guid.TryParse(userIdClaim, out Guid parsedUserId) ? parsedUserId : Guid.Empty;
                var companyidclam = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;
                var companyName = identity.Claims.FirstOrDefault(c => c.Type == "CompanyName")?.Value;

                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));

                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    Ticket ticket = HSapiFacade.GetTicketByTicketId(ticketid);
                    CustomSurvey customSurvey = HSapiFacade.GetCustomSurveyBySurveyId(surveyId);

                    if (surveyId == Guid.Empty)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Error("Please select a survey name."));
                    }

                    ShortUrl ShortUrl = new ShortUrl();
                    Customer cus = HSapiFacade.GetCustomerByCustomerId(ticket.CustomerId);
                    TicketReply tr = new TicketReply
                    {
                        Message = $"{username} sent {customSurvey.SurveyName} survey to {cus.FirstName}",
                        TicketId = ticketid,
                        RepliedDate = DateTime.Now.UTCCurrentTime(),
                        IsPrivate = true,
                        UserId = userId
                    };
                    HSapiFacade.InsertTicketReply(tr);

                    if (cus != null)
                    {
                        CustomSurveyUser surveyTicket = new CustomSurveyUser
                        {
                            SurveyUserId = Guid.NewGuid(),
                            Status = LabelHelper.CustomSurveyStatus.Created,
                            AddedBy = userId,
                            AddedDate = DateTime.Now.UTCCurrentTime(),
                            ReferenceId = ticketid.ToString(),
                            SurveyId = surveyId,
                            UserId = userId
                        };

                        HSapiFacade.InsertCustomSurveyUser(surveyTicket);
                        ticket.HasSurvey = true;
                        HSapiFacade.UpdateTicket(ticket);


                        string TicketUrl = string.Format("{2}{4}/Public/Open/?TicketId={0}&CompanyId={1}&Type={3}", surveyTicket.SurveyUserId, usercontext.CompanyId, AppConfig.SiteDomain, LabelHelper.PublicOpenTypes.CustomSurvey, AppConfig.DomainSitePath);
                        ShortUrl = HSapiFacade.GetSortUrlByUrl(TicketUrl, Guid.Empty);
                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            if (email.Contains(";"))
                            {
                                string[] emailList = email.Split(";");
                                foreach (var item in emailList)
                                {
                                    SendSurveyEmail emails = new SendSurveyEmail
                                    {
                                        Name = $"{cus.FirstName} {cus.LastName}",
                                        shortLink = ShortUrl.Url,
                                        CompanyId = new Guid(companyidclam),
                                        ToEmail = item,
                                        SenderName = username,
                                        CompanyName = companyName,
                                        Subject = $"Survey for Ticket#{ticket.Id}",
                                        Header = $"Survey for Ticket#{ticket.Id}"
                                    };
                                    try
                                    {
                                        HSapiFacade.SendSurveyEmail(emails);
                                    }
                                    catch (Exception)
                                    {
                                        return Request.CreateResponse(HttpStatusCode.InternalServerError, ApiResponse<object>.Error("Internal error. Please contact system admin."));
                                    }
                                }
                            }
                            else
                            {
                                SendSurveyEmail emails = new SendSurveyEmail
                                {
                                    Name = $"{cus.FirstName} {cus.LastName}",
                                    shortLink = ShortUrl.Url,
                                    CompanyId = new Guid(companyidclam),
                                    ToEmail = email,
                                    SenderName = username,
                                    CompanyName = companyName,
                                    Subject = $"Survey for Ticket#{ticket.Id}",
                                    Header = $"Survey for Ticket#{ticket.Id}"
                                };
                                try
                                {
                                    HSapiFacade.SendSurveyEmail(emails);
                                }
                                catch (Exception)
                                {
                                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ApiResponse<object>.Error("Internal error. Please contact system admin."));
                                }
                            }

                            // Log Activity
                            if (!string.IsNullOrWhiteSpace(ActionDisplyText) && !string.IsNullOrWhiteSpace(Action) && !string.IsNullOrWhiteSpace(UserId))
                                AddUserActivityForCustomer(ActionDisplyText, PageUrl, Action, UserAgent, UserIp, UserName, UserId, CustomerId, "Ticket", ticketid.ToString());
                        }

                        if (!string.IsNullOrWhiteSpace(phone))
                        {
                            List<string> receiverNumberList = phone.Contains(";") ? phone.Split(";").ToList() : new List<string> { phone };
                            try
                            {
                                HSapiFacade.SendSMS(new Guid(companyidclam), userId, ShortUrl.Url, receiverNumberList, false, username);
                            }
                            catch (Exception ex)
                            {
                                return Request.CreateResponse(HttpStatusCode.InternalServerError, ApiResponse<object>.Error("Internal error. Please contact system admin."));
                            }
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<object>.Success(null));
                    }

                    return Request.CreateResponse(HttpStatusCode.NotFound, ApiResponse<object>.Error("Customer not found."));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, ApiResponse<object>.Error("User not authenticated."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ApiResponse<object>.Error(ex.Message));
            }
        }

        [Route("view-mode")]
        [HttpGet]
        public HttpResponseMessage GetAllTechnicianUser()
        {
            Guid userId = Guid.Empty;
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("userId"))
            {
                Guid.TryParse(headers.GetValues("userId").First(), out userId);
            }

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var username = identity.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
                var companyIdClaim = identity.Claims.FirstOrDefault(c => c.Type == "CompanyId")?.Value;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(companyIdClaim))
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }

                var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(username, new Guid(companyIdClaim));
                if (usercontext == null)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                }

                string ComId = usercontext.CompanyId.ToString();
                APIInitialize();

              
                var model = HSapiFacade.GetAllUserMgmtListByCompanyId(ComId);

                if (model != null && model.UserMgmtList.Any())
                {
                    var userList = model.UserMgmtList
                        .Select(u => new
                        {
                            UserId = u.UserId,
                            Name = u.ContactName
                        })
                        .ToList();

                    var response = new
                    {
                        success = true,
                        error = (string)null,
                        result = userList
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { success = false, error = "Not found.", result = new List<object>() });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { success = false, error = ex.Message, result = new List<object>() });
            }
        }

        [Route("ticket")]
        [HttpGet]
        public HttpResponseMessage Ticket()
        {
            Guid ticketid = new Guid();
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("ticketid"))
            {
                Guid.TryParse(headers.GetValues("ticketid").First(), out ticketid);
            }
            try
            {
                JobsCustomModel model = new JobsCustomModel();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();

                    model = HSapiFacade.GetJobDetailByJobId(ticketid);
                    if (model != null && model.TicketList.Any())
                    {
                        var ticket = model.TicketList.First();

                        var result = new
                        {
                            id = ticket.Id,
                            ticketGuid = ticket.TicketId,
                            customerGuid = ticket.CustomerId,
                            soldBy = ticket.AssignedPerson,
                            type = ticket.TicketType,
                            package = ticket.Subject,
                            closed = ticket.IsClosed,
                            dispatched = ticket.IsDispatch
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResponse<object>(result, true, null));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, ApiResponse<object>.Error("Not found."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<object>.Error(ex.Message));
            }
        }

        [Route("tickets")]
        [HttpGet]
        public HttpResponseMessage Tickets()
        {
            string from = "";
            string to = "";
            Guid userid = new Guid();
            string status = "";
            string type = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("from"))
            {
                from = headers.GetValues("from").First();
            }
            if (headers.Contains("to"))
            {
                to = headers.GetValues("to").First();
            }
            if (headers.Contains("userid"))
            {
                Guid.TryParse(headers.GetValues("userid").First(), out userid);
            }
            if (headers.Contains("status"))
            {
                status = headers.GetValues("status").First();
            }
            if (headers.Contains("type"))
            {
                type = headers.GetValues("type").First();
            }
            try
            {
                JobsCustomModel model = new JobsCustomModel();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<string>.Error("Authorization Denied."));
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();
                    bool permission = HSapiFacade.CloseTicketPermission(userid, usercontext.CompanyId);
                    model = HSapiFacade.GetAllJobsDetailByFilter(usercontext.CompanyId, from, to, userid, status, type, permission);
                    if (model != null && model.TicketList.Count > 0)
                    {
                        var ticketIds = model.TicketList.Select(ticket => ticket.TicketId.ToString()).ToArray();

                        return Request.CreateResponse(HttpStatusCode.OK, new ApiResponse<string[]>(ticketIds, true, null));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string[]>.Success(new string[0]));
                    }

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<string>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ApiResponse<string>.Error(ex.Message));
            }
        }


        [Authorize]
        [Route("lookup")]
        [HttpGet]
        public HttpResponseMessage Lookup()
        {
            APIInitialize();
            var re = Request;
            var headers = re.Headers;
            string Key = "";
            if (headers.Contains("Key"))
            {
                Key = headers.GetValues("Key").First();
            }

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));

                    string[] splitkey = Key.Split(',');
                    List<LookupResponseModel> keyList = new List<LookupResponseModel>();

                    foreach (var item in splitkey)
                    {
                        var lookups = HSapiFacade.GetLookupByKey(item, usercontext.CompanyId);

                        foreach (var lookup in lookups)
                        {
                            keyList.Add(new LookupResponseModel
                            {
                                text = lookup.DisplayText,
                                value = lookup.DataValue,
                                order = lookup.DataOrder ?? 0,
                                active = lookup.IsActive ?? false
                            });
                        }
                    }

                    #region LOG
                    #region Create Directory if not exists
                    try
                    {
                        string subPath = "~/APILOG"; // your code goes here

                        bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
                        if (!exists)
                            System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
                    }
                    catch (Exception ex)
                    {
                        // Log exception if needed
                    }
                    #endregion

                    // Log the request to a file
                    try
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\APILOG\GetLookupbyKey.txt"), true))
                        {
                            string LOGFile = $"IP ADDRESS: {base.GetClientIp(Request)} Username: {identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()} Date: {DateTime.Now:MM/dd/yy hh:mm:ss tt} Data: ";
                            LOGFile += $" Key = {Key}";

                            file.WriteLine(LOGFile);
                            file.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log exception if needed
                    }
                    #endregion

                    var successResponse = ApiResponse<object>.Success(keyList);
                    return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, ex.Message));
            }
        }

        #endregion

        #region Customer Inspection
        [Authorize]
        [Route("SaveCustomerInspection")]
        public HttpResponseMessage CreateCustomerInspection()
        {
            APIInitialize();
            string CurrentOutsideConditions = "";
            double OutsideRelativeHumidity = 0;
            double OutsideTemperature = 0;
            double FirstFloorRelativeHumidity = 0;
            double FirstFloorTemperature = 0;
            string RelativeOther1 = "";
            string RelativeOther2 = "";
            string Heat = "";
            string Air = "";
            double BasementRelativeHumidity = 0;
            double BasementTemperature = 0;
            string BasementDehumidifier = "";
            string GroundWater = "";
            int GroundWaterRating = 0;
            string IronBacteria = "";
            int IronBacteriaRating = 0;
            string Condensation = "";
            int CondensationRating = 0;
            string WallCracks = "";
            int WallCracksRating = 0;
            string FloorCracks = "";
            int FloorCracksRating = 0;
            string ExistingSumpPump = "";
            string ExistingDrainageSystem = "";
            string ExistingRadonSystem = "";
            string DryerVentToCode = "";
            string FoundationType = "";
            string Bulkhead = "";
            string VisualBasementOther = "";
            string NoticedSmellsOrOdors = "";
            string NoticedSmellsOrOdorsComment = "";
            string NoticedMoldOrMildew = "";
            string NoticedMoldOrMildewComment = "";
            string BasementGoDown = "";
            string HomeSufferForRespiratory = "";
            string HomeSufferForrespiratoryComment = "";
            string ChildrenPlayInBasement = "";
            string ChildrenPlayInBasementComment = "";
            string PetsGoInBasement = "";
            string PetsGoInBasementComment = "";
            string NoticedBugsOrRodents = "";
            string NoticedBugsOrRodentsComment = "";
            string GetWater = "";
            string GetWaterComment = "";
            string RemoveWater = "";
            string SeeCondensationPipesDripping = "";
            string SeeCondensationPipesDrippingComment = "";
            string RepairsProblems = "";
            string RepairsProblemsComment = "";
            string LivingPlan = "";
            string SellPlaning = "";
            string PlansForBasementOnce = "";
            string HomeTestForPastRadon = "";
            string HomeTestForPastRadonComment = "";
            string LosePower = "";
            string LosePowerHowOften = "";
            string CustomerBasementOther = "";
            string Drawing = "";
            string Notes = "";
            string InspectionPhoto = "";
            string PMSignature = "";
            DateTime PMSignatureDate = new DateTime();
            string HomeOwnerSignature = "";
            DateTime HomeOwnerSignatureDate = new DateTime();
            Guid CreatedBy = Guid.NewGuid();
            Guid companyId = Guid.Empty;
            int Id = 0;
            Guid CustomerId = Guid.Empty;

            var re = Request;
            var headers = re.Headers;
            bool result = false;
            string drawing = "";
            string pmsignature = "";
            string homeownersignature = "";
            if (headers.Contains("Id"))
            {
                string IdSt = headers.GetValues("Id").First();
                int.TryParse(IdSt, out Id);
            }
            if (headers.Contains("CompanyId"))
            {
                string CustomerIdSt = headers.GetValues("CompanyId").First();
                Guid.TryParse(CustomerIdSt, out companyId);
            }
            if (headers.Contains("CustomerId"))
            {
                string CustomerIdSt = headers.GetValues("CustomerId").First();
                Guid.TryParse(CustomerIdSt, out CustomerId);
            }
            if (companyId == Guid.Empty || CustomerId == Guid.Empty)
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new { result });
            if (headers.Contains("CurrentOutsideConditions"))
            {
                CurrentOutsideConditions = headers.GetValues("CurrentOutsideConditions").First();
            }
            if (headers.Contains("OutsideRelativeHumidity"))
            {
                string OutsideRelativeHumiditySt = headers.GetValues("OutsideRelativeHumidity").First();
                double.TryParse(OutsideRelativeHumiditySt, out OutsideRelativeHumidity);
            }
            if (headers.Contains("OutsideTemperature"))
            {
                string StOutsideTemperature = headers.GetValues("OutsideTemperature").First();
                double.TryParse(StOutsideTemperature, out OutsideTemperature);
            }
            if (headers.Contains("FirstFloorRelativeHumidity"))
            {
                string StFirstFloorRelativeHumidity = headers.GetValues("FirstFloorRelativeHumidity").First();
                double.TryParse(StFirstFloorRelativeHumidity, out FirstFloorRelativeHumidity);
            }
            if (headers.Contains("FirstFloorTemperature"))
            {
                string StFirstFloorTemperature = headers.GetValues("FirstFloorTemperature").First();
                double.TryParse(StFirstFloorTemperature, out FirstFloorTemperature);
            }
            if (headers.Contains("RelativeOther1"))
            {
                RelativeOther1 = headers.GetValues("RelativeOther1").First();
            }
            if (headers.Contains("RelativeOther2"))
            {
                RelativeOther2 = headers.GetValues("RelativeOther2").First();
            }
            if (headers.Contains("Heat"))
            {
                Heat = headers.GetValues("Heat").First();
            }
            if (headers.Contains("Air"))
            {
                Air = headers.GetValues("Air").First();
            }
            if (headers.Contains("BasementRelativeHumidity"))
            {
                string StBasementRelativeHumidity = headers.GetValues("BasementRelativeHumidity").First();
                double.TryParse(StBasementRelativeHumidity, out BasementRelativeHumidity);
            }
            if (headers.Contains("BasementTemperature"))
            {
                string StBasementTemperature = headers.GetValues("BasementTemperature").First();
                double.TryParse(StBasementTemperature, out BasementTemperature);
            }
            if (headers.Contains("BasementDehumidifier"))
            {
                BasementDehumidifier = headers.GetValues("BasementDehumidifier").First();
            }
            if (headers.Contains("GroundWater"))
            {
                GroundWater = headers.GetValues("GroundWater").First();
            }
            if (headers.Contains("GroundWaterRating"))
            {
                string StGroundWaterRating = headers.GetValues("GroundWaterRating").First();
                int.TryParse(StGroundWaterRating, out GroundWaterRating);
            }
            if (headers.Contains("IronBacteria"))
            {
                IronBacteria = headers.GetValues("IronBacteria").First();
            }
            if (headers.Contains("IronBacteriaRating"))
            {
                string StIronBacteriaRating = headers.GetValues("IronBacteriaRating").First();
                int.TryParse(StIronBacteriaRating, out IronBacteriaRating);
            }
            if (headers.Contains("Condensation"))
            {
                Condensation = headers.GetValues("Condensation").First();
            }
            if (headers.Contains("CondensationRating"))
            {
                string StCondensationRating = headers.GetValues("CondensationRating").First();
                int.TryParse(StCondensationRating, out CondensationRating);
            }
            if (headers.Contains("WallCracks"))
            {
                WallCracks = headers.GetValues("WallCracks").First();
            }
            if (headers.Contains("WallCracksRating"))
            {
                string StWallCracksRating = headers.GetValues("WallCracksRating").First();
                int.TryParse(StWallCracksRating, out WallCracksRating);
            }
            if (headers.Contains("FloorCracks"))
            {
                FloorCracks = headers.GetValues("FloorCracks").First();
            }
            if (headers.Contains("FloorCracksRating"))
            {
                string StFloorCracksRating = headers.GetValues("FloorCracksRating").First();
                int.TryParse(StFloorCracksRating, out FloorCracksRating);
            }
            if (headers.Contains("ExistingSumpPump"))
            {
                ExistingSumpPump = headers.GetValues("ExistingSumpPump").First();
            }
            if (headers.Contains("ExistingDrainageSystem"))
            {
                ExistingDrainageSystem = headers.GetValues("ExistingDrainageSystem").First();
            }
            if (headers.Contains("ExistingRadonSystem"))
            {
                ExistingRadonSystem = headers.GetValues("ExistingRadonSystem").First();
            }
            if (headers.Contains("DryerVentToCode"))
            {
                DryerVentToCode = headers.GetValues("DryerVentToCode").First();
            }
            if (headers.Contains("FoundationType"))
            {
                FoundationType = headers.GetValues("FoundationType").First();
            }
            if (headers.Contains("Bulkhead"))
            {
                Bulkhead = headers.GetValues("Bulkhead").First();
            }
            if (headers.Contains("VisualBasementOther"))
            {
                VisualBasementOther = headers.GetValues("VisualBasementOther").First();
            }
            if (headers.Contains("NoticedSmellsOrOdors"))
            {
                NoticedSmellsOrOdors = headers.GetValues("NoticedSmellsOrOdors").First();
            }
            if (headers.Contains("NoticedSmellsOrOdorsComment"))
            {
                NoticedSmellsOrOdorsComment = headers.GetValues("NoticedSmellsOrOdorsComment").First();
            }
            if (headers.Contains("NoticedMoldOrMildew"))
            {
                NoticedMoldOrMildew = headers.GetValues("NoticedMoldOrMildew").First();
            }
            if (headers.Contains("NoticedMoldOrMildewComment"))
            {
                NoticedMoldOrMildewComment = headers.GetValues("NoticedMoldOrMildewComment").First();
            }
            if (headers.Contains("BasementGoDown"))
            {
                BasementGoDown = headers.GetValues("BasementGoDown").First();
            }
            if (headers.Contains("HomeSufferForRespiratory"))
            {
                HomeSufferForRespiratory = headers.GetValues("HomeSufferForRespiratory").First();
            }
            if (headers.Contains("HomeSufferForrespiratoryComment"))
            {
                HomeSufferForrespiratoryComment = headers.GetValues("HomeSufferForrespiratoryComment").First();
            }
            if (headers.Contains("ChildrenPlayInBasement"))
            {
                ChildrenPlayInBasement = headers.GetValues("ChildrenPlayInBasement").First();
            }
            if (headers.Contains("ChildrenPlayInBasementComment"))
            {
                ChildrenPlayInBasementComment = headers.GetValues("ChildrenPlayInBasementComment").First();
            }
            if (headers.Contains("PetsGoInBasement"))
            {
                PetsGoInBasement = headers.GetValues("PetsGoInBasement").First();
            }
            if (headers.Contains("PetsGoInBasementComment"))
            {
                PetsGoInBasementComment = headers.GetValues("PetsGoInBasementComment").First();
            }
            if (headers.Contains("NoticedBugsOrRodents"))
            {
                NoticedBugsOrRodents = headers.GetValues("NoticedBugsOrRodents").First();
            }
            if (headers.Contains("NoticedBugsOrRodentsComment"))
            {
                NoticedBugsOrRodentsComment = headers.GetValues("NoticedBugsOrRodentsComment").First();
            }
            if (headers.Contains("GetWater"))
            {
                GetWater = headers.GetValues("GetWater").First();
            }
            if (headers.Contains("GetWaterComment"))
            {
                GetWaterComment = headers.GetValues("GetWaterComment").First();
            }
            if (headers.Contains("RemoveWater"))
            {
                RemoveWater = headers.GetValues("RemoveWater").First();
            }
            if (headers.Contains("SeeCondensationPipesDripping"))
            {
                SeeCondensationPipesDripping = headers.GetValues("SeeCondensationPipesDripping").First();
            }
            if (headers.Contains("SeeCondensationPipesDrippingComment"))
            {
                SeeCondensationPipesDrippingComment = headers.GetValues("SeeCondensationPipesDrippingComment").First();
            }
            if (headers.Contains("RepairsProblems"))
            {
                RepairsProblems = headers.GetValues("RepairsProblems").First();
            }
            if (headers.Contains("RepairsProblemsComment"))
            {
                RepairsProblemsComment = headers.GetValues("RepairsProblemsComment").First();
            }
            if (headers.Contains("LivingPlan"))
            {
                LivingPlan = headers.GetValues("LivingPlan").First();
            }
            if (headers.Contains("SellPlaning"))
            {
                SellPlaning = headers.GetValues("SellPlaning").First();
            }
            if (headers.Contains("PlansForBasementOnce"))
            {
                PlansForBasementOnce = headers.GetValues("PlansForBasementOnce").First();
            }
            if (headers.Contains("HomeTestForPastRadon"))
            {
                HomeTestForPastRadon = headers.GetValues("HomeTestForPastRadon").First();
            }
            if (headers.Contains("HomeTestForPastRadonComment"))
            {
                HomeTestForPastRadonComment = headers.GetValues("HomeTestForPastRadonComment").First();
            }
            if (headers.Contains("LosePower"))
            {
                LosePower = headers.GetValues("LosePower").First();
            }
            if (headers.Contains("LosePowerHowOften"))
            {
                LosePowerHowOften = headers.GetValues("LosePowerHowOften").First();
            }
            if (headers.Contains("CustomerBasementOther"))
            {
                CustomerBasementOther = headers.GetValues("CustomerBasementOther").First();
            }
            if (headers.Contains("Drawing"))
            {
                Drawing = headers.GetValues("Drawing").First();
            }
            if (headers.Contains("Notes"))
            {
                Notes = headers.GetValues("Notes").First();
            }
            if (headers.Contains("PMSignature"))
            {
                PMSignature = headers.GetValues("PMSignature").First();
            }
            if (headers.Contains("PMSignatureDate"))
            {
                string StPMSignatureDate = headers.GetValues("PMSignatureDate").First();
                DateTime.TryParse(StPMSignatureDate, out PMSignatureDate);
            }
            if (headers.Contains("HomeOwnerSignature"))
            {
                HomeOwnerSignature = headers.GetValues("HomeOwnerSignature").First();
            }
            if (headers.Contains("HomeOwnerSignatureDate"))
            {
                string StHomeOwnerSignatureDate = headers.GetValues("HomeOwnerSignatureDate").First();
                DateTime.TryParse(StHomeOwnerSignatureDate, out HomeOwnerSignatureDate);
            }
            if (headers.Contains("drawing"))
            {
                drawing = headers.GetValues("drawing").First();
            }
            if (headers.Contains("pmsignature"))
            {
                pmsignature = headers.GetValues("pmsignature").First();
            }
            if (headers.Contains("homeownersignature"))
            {
                homeownersignature = headers.GetValues("homeownersignature").First();
            }
            if (headers.Contains("InspectionPhoto"))
            {
                InspectionPhoto = headers.GetValues("InspectionPhoto").First();
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    Customer customer = HSapiFacade.GetCustomerByCustomerId(CustomerId);
                    if (customer == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Customer Not Found.");
                    string imagedomain = ConfigurationManager.AppSettings["ImageDomain"];
                    var CustomerInspectionDetails = HSapiFacade.GetCustomerInspectionByIdAndComid(CustomerId, companyId);
                    if (CustomerInspectionDetails == null)
                    {
                        CustomerInspection cus = new CustomerInspection()
                        {
                            CompanyId = companyId,
                            CustomerId = CustomerId,
                            CurrentOutsideConditions = CurrentOutsideConditions,
                            OutsideRelativeHumidity = OutsideRelativeHumidity,
                            OutsideTemperature = OutsideTemperature,
                            FirstFloorRelativeHumidity = FirstFloorRelativeHumidity,
                            FirstFloorTemperature = FirstFloorTemperature,
                            RelativeOther1 = RelativeOther1,
                            RelativeOther2 = RelativeOther2,
                            Heat = Heat,
                            Air = Air,
                            BasementRelativeHumidity = BasementRelativeHumidity,
                            BasementTemperature = BasementTemperature,
                            BasementDehumidifier = BasementDehumidifier,
                            GroundWater = GroundWater,
                            GroundWaterRating = GroundWaterRating,
                            IronBacteria = IronBacteria,
                            IronBacteriaRating = IronBacteriaRating,
                            Condensation = Condensation,
                            CondensationRating = CondensationRating,
                            WallCracks = WallCracks,
                            WallCracksRating = WallCracksRating,
                            FloorCracks = FloorCracks,
                            FloorCracksRating = FloorCracksRating,
                            ExistingSumpPump = ExistingSumpPump,
                            ExistingDrainageSystem = ExistingDrainageSystem,
                            ExistingRadonSystem = ExistingRadonSystem,
                            DryerVentToCode = DryerVentToCode,
                            FoundationType = FoundationType,
                            Bulkhead = Bulkhead,
                            VisualBasementOther = VisualBasementOther,
                            NoticedSmellsOrOdors = NoticedSmellsOrOdors,
                            NoticedSmellsOrOdorsComment = NoticedSmellsOrOdorsComment,
                            NoticedMoldOrMildew = NoticedMoldOrMildew,
                            NoticedMoldOrMildewComment = NoticedMoldOrMildewComment,
                            BasementGoDown = BasementGoDown,
                            HomeSufferForRespiratory = HomeSufferForRespiratory,
                            HomeSufferForrespiratoryComment = HomeSufferForrespiratoryComment,
                            ChildrenPlayInBasement = ChildrenPlayInBasement,
                            ChildrenPlayInBasementComment = ChildrenPlayInBasementComment,
                            PetsGoInBasement = PetsGoInBasement,
                            PetsGoInBasementComment = PetsGoInBasementComment,
                            NoticedBugsOrRodents = NoticedBugsOrRodents,
                            NoticedBugsOrRodentsComment = NoticedBugsOrRodentsComment,
                            GetWater = GetWater,
                            GetWaterComment = GetWaterComment,
                            RemoveWater = RemoveWater,
                            SeeCondensationPipesDripping = SeeCondensationPipesDripping,
                            SeeCondensationPipesDrippingComment = SeeCondensationPipesDrippingComment,
                            RepairsProblems = RepairsProblems,
                            RepairsProblemsComment = RepairsProblemsComment,
                            LivingPlan = LivingPlan,
                            SellPlaning = SellPlaning,
                            PlansForBasementOnce = PlansForBasementOnce,
                            HomeTestForPastRadon = HomeTestForPastRadon,
                            HomeTestForPastRadonComment = HomeTestForPastRadonComment,
                            LosePower = LosePower,
                            LosePowerHowOften = LosePowerHowOften,
                            CustomerBasementOther = CustomerBasementOther,
                            Drawing = imagedomain + "/" + drawing,
                            Notes = Notes,
                            InspectionPhoto = InspectionPhoto,
                            PMSignature = imagedomain + "/" + pmsignature,
                            PMSignatureDate = PMSignatureDate,
                            HomeOwnerSignature = imagedomain + "/" + homeownersignature,
                            HomeOwnerSignatureDate = HomeOwnerSignatureDate,
                            CreatedBy = CreatedBy,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            LastUpdatedBy = Guid.Empty,
                            LastUpdatedDate = DateTime.Now.UTCCurrentTime()

                        };
                        result = HSapiFacade.InsertCustomerInspection(cus) > 0;
                    }
                    else
                    {
                        CustomerInspectionDetails.CurrentOutsideConditions = CurrentOutsideConditions;
                        CustomerInspectionDetails.OutsideRelativeHumidity = OutsideRelativeHumidity;
                        CustomerInspectionDetails.OutsideTemperature = OutsideTemperature;
                        CustomerInspectionDetails.FirstFloorRelativeHumidity = FirstFloorRelativeHumidity;
                        CustomerInspectionDetails.FirstFloorTemperature = FirstFloorTemperature;
                        CustomerInspectionDetails.RelativeOther1 = RelativeOther1;
                        CustomerInspectionDetails.RelativeOther2 = RelativeOther2;
                        CustomerInspectionDetails.Heat = Heat;
                        CustomerInspectionDetails.Air = Air;
                        CustomerInspectionDetails.BasementRelativeHumidity = BasementRelativeHumidity;
                        CustomerInspectionDetails.BasementTemperature = BasementTemperature;
                        CustomerInspectionDetails.BasementDehumidifier = BasementDehumidifier;
                        CustomerInspectionDetails.GroundWater = GroundWater;
                        CustomerInspectionDetails.GroundWaterRating = GroundWaterRating;
                        CustomerInspectionDetails.IronBacteria = IronBacteria;
                        CustomerInspectionDetails.IronBacteriaRating = IronBacteriaRating;
                        CustomerInspectionDetails.Condensation = Condensation;
                        CustomerInspectionDetails.CondensationRating = CondensationRating;
                        CustomerInspectionDetails.WallCracks = WallCracks;
                        CustomerInspectionDetails.WallCracksRating = WallCracksRating;
                        CustomerInspectionDetails.FloorCracks = FloorCracks;
                        CustomerInspectionDetails.FloorCracksRating = FloorCracksRating;
                        CustomerInspectionDetails.ExistingSumpPump = ExistingSumpPump;
                        CustomerInspectionDetails.ExistingDrainageSystem = ExistingDrainageSystem;
                        CustomerInspectionDetails.ExistingRadonSystem = ExistingRadonSystem;
                        CustomerInspectionDetails.DryerVentToCode = DryerVentToCode;
                        CustomerInspectionDetails.FoundationType = FoundationType;
                        CustomerInspectionDetails.Bulkhead = Bulkhead;
                        CustomerInspectionDetails.VisualBasementOther = VisualBasementOther;
                        CustomerInspectionDetails.NoticedSmellsOrOdors = NoticedSmellsOrOdors;
                        CustomerInspectionDetails.NoticedSmellsOrOdorsComment = NoticedSmellsOrOdorsComment;
                        CustomerInspectionDetails.NoticedMoldOrMildew = NoticedMoldOrMildew;
                        CustomerInspectionDetails.NoticedMoldOrMildewComment = NoticedMoldOrMildewComment;
                        CustomerInspectionDetails.BasementGoDown = BasementGoDown;
                        CustomerInspectionDetails.HomeSufferForRespiratory = HomeSufferForRespiratory;
                        CustomerInspectionDetails.HomeSufferForrespiratoryComment = HomeSufferForrespiratoryComment;
                        CustomerInspectionDetails.ChildrenPlayInBasement = ChildrenPlayInBasement;
                        CustomerInspectionDetails.ChildrenPlayInBasementComment = ChildrenPlayInBasementComment;
                        CustomerInspectionDetails.PetsGoInBasement = PetsGoInBasement;
                        CustomerInspectionDetails.PetsGoInBasementComment = PetsGoInBasementComment;
                        CustomerInspectionDetails.NoticedBugsOrRodents = NoticedBugsOrRodents;
                        CustomerInspectionDetails.NoticedBugsOrRodentsComment = NoticedBugsOrRodentsComment;
                        CustomerInspectionDetails.GetWater = GetWater;
                        CustomerInspectionDetails.GetWaterComment = GetWaterComment;
                        CustomerInspectionDetails.RemoveWater = RemoveWater;
                        CustomerInspectionDetails.SeeCondensationPipesDripping = SeeCondensationPipesDripping;
                        CustomerInspectionDetails.SeeCondensationPipesDrippingComment = SeeCondensationPipesDrippingComment;
                        CustomerInspectionDetails.RepairsProblems = RepairsProblems;
                        CustomerInspectionDetails.RepairsProblemsComment = RepairsProblemsComment;
                        CustomerInspectionDetails.LivingPlan = LivingPlan;
                        CustomerInspectionDetails.SellPlaning = SellPlaning;
                        CustomerInspectionDetails.PlansForBasementOnce = PlansForBasementOnce;
                        CustomerInspectionDetails.HomeTestForPastRadon = HomeTestForPastRadon;
                        CustomerInspectionDetails.HomeTestForPastRadonComment = HomeTestForPastRadonComment;
                        CustomerInspectionDetails.LosePower = LosePower;
                        CustomerInspectionDetails.LosePowerHowOften = LosePowerHowOften;
                        CustomerInspectionDetails.CustomerBasementOther = CustomerBasementOther;
                        CustomerInspectionDetails.Drawing = imagedomain + "/" + drawing;
                        CustomerInspectionDetails.Notes = Notes;
                        CustomerInspectionDetails.InspectionPhoto = InspectionPhoto;
                        CustomerInspectionDetails.PMSignature = imagedomain + "/" + pmsignature;
                        CustomerInspectionDetails.PMSignatureDate = PMSignatureDate;
                        CustomerInspectionDetails.HomeOwnerSignature = imagedomain + "/" + homeownersignature;
                        CustomerInspectionDetails.HomeOwnerSignatureDate = HomeOwnerSignatureDate;
                        CustomerInspectionDetails.LastUpdatedBy = Guid.Empty;
                        CustomerInspectionDetails.LastUpdatedDate = DateTime.Now.UTCCurrentTime();

                        result = HSapiFacade.UpdateCustomerInspection(CustomerInspectionDetails);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { result });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "token expired");
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }
        [Authorize]
        [Route("GetCustomerInspectionById")]
        public HttpResponseMessage GetCustomerInspectionId()
        {
            APIInitialize();
            Guid CustomerId = Guid.Empty;
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("CustomerID"))
            {
                string stID = headers.GetValues("CustomerID").First();
                Guid.TryParse(stID, out CustomerId);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    Customer customer = HSapiFacade.GetCustomerByCustomerId(CustomerId);
                    if (customer == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Customer Not Found.");

                    var CustomerInspectionDetails = HSapiFacade.GetCustomerInspectionById(CustomerId);
                    return Request.CreateResponse(HttpStatusCode.OK, new { CustomerInspectionDetails });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }
        [Authorize]
        [Route("GetCustomerInspectionByCustomerId")]
        public HttpResponseMessage GetCustomerInspectionCustomerId()
        {
            APIInitialize();
            Guid CustomerId = Guid.Empty;
            var re = Request;
            var headers = re.Headers;
            bool result = false;
            if (headers.Contains("CustomerId"))
            {
                string stCustomerId = headers.GetValues("CustomerId").First();
                Guid.TryParse(stCustomerId, out CustomerId);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    Customer customer = HSapiFacade.GetCustomerByCustomerId(CustomerId);
                    if (customer == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Customer Not Found.");

                    var CustomerInspectionList = HSapiFacade.GetCustomerInspectionById(CustomerId);
                    return Request.CreateResponse(HttpStatusCode.OK, new { CustomerInspectionList });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        #endregion

        public static string UploadFileFormData()
        {
            string filepath = "";
            try
            {
                var file = HttpContext.Current.Request.Files.Count > 0 ?
                    HttpContext.Current.Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    string tempFolderName = ConfigurationManager.AppSettings["File.CustomerFiles"];
                    tempFolderName += "/" + DateTime.Now.Month + "-" + DateTime.Now.Day;

                    Random rand = new Random();
                    string FileName = rand.Next().ToString();
                    FileName += "-___" + file.FileName;
                    string tempFolderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + tempFolderName);

                    if (CreateFolderIfNeeded(tempFolderPath))
                    {
                        try
                        {
                            string FilePath = Path.Combine(tempFolderPath, FileName);
                            file.SaveAs(FilePath);
                        }
                        catch (Exception ec)
                        {
                            return ec.Message;
                        }
                    }
                    filepath = string.Concat("/", tempFolderName, "/", FileName);
                }
                return filepath;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string GetUploadFileName()
        {
            string filename = "";
            try
            {
                var file = HttpContext.Current.Request.Files.Count > 0 ?
                    HttpContext.Current.Request.Files[0] : null;
                if (file != null && file.ContentLength > 0)
                {
                    filename = file.FileName;
                }
                return filename;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }

        #region Image

        [Route("UploadImageFile")]
        public HttpResponseMessage UploadImageFile([FromBody] FileType file)
        {
            APIInitialize();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    if (!string.IsNullOrWhiteSpace(file.filename) && !string.IsNullOrWhiteSpace(file.filepath))
                    {
                        var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                        if (usercontext == null)
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                        string tempFolderName = ConfigurationManager.AppSettings["File.UploadFiles"];
                        var comname = HSapiFacade.GetCompanyByCompanyId(usercontext.CompanyId).CompanyName;
                        tempFolderName = string.Format(tempFolderName, comname.ReplaceSpecialChar());
                        tempFolderName += "/" + DateTime.Now.Month + "-" + DateTime.Now.Day;

                        Random rand = new Random();
                        string FileName = rand.Next().ToString();
                        FileName += "-___" + file.filename;
                        string tempFolderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + tempFolderName);

                        if (CreateFolderIfNeeded(tempFolderPath))
                        {
                            try
                            {
                                string FilePath = Path.Combine(tempFolderPath, FileName);
                                file.filepath = file.filepath.Replace(' ', '+');
                                byte[] imageBytes = Convert.FromBase64String(file.filepath);
                                File.WriteAllBytes(FilePath, imageBytes);
                            }
                            catch (Exception ec)
                            {
                                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ec.Message);
                            }
                        }
                        string filePath = string.Concat("/", tempFolderName, "/", FileName);
                        return Request.CreateResponse(HttpStatusCode.OK, new { filePath = filePath, filename = file.filename, file = file.filename });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "filepath should not be empty");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "token expired");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Route("CustomerImageUpload")]
        public HttpResponseMessage CustomerImageUpload([FromBody] FileType file)
        {
            APIInitialize();
            string ImageDomain = AppConfig.ImageDomain;
            int CustomerId = 0;
            bool Result = false;

            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("CustomerId"))
            {
                int.TryParse(headers.GetValues("CustomerId").First(), out CustomerId);
            }


            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    if (!string.IsNullOrWhiteSpace(file.filename) && !string.IsNullOrWhiteSpace(file.filepath))
                    {
                        var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                        if (usercontext == null)
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                        var customer = HSapiFacade.GetCustomerById(CustomerId);
                        if (customer == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Customer Not Found.");
                        }
                        string tempFolderName = ConfigurationManager.AppSettings["File.TicketFile"];
                        var comname = HSapiFacade.GetCompanyByCompanyId(usercontext.CompanyId).CompanyName;
                        tempFolderName = string.Format(tempFolderName, comname.ReplaceSpecialChar());
                        tempFolderName += "/" + DateTime.Now.Month + "-" + DateTime.Now.Day;

                        Random rand = new Random();
                        string FileName = rand.Next().ToString();
                        FileName += "-___" + file.filename;
                        string tempFolderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + tempFolderName);

                        if (CreateFolderIfNeeded(tempFolderPath))
                        {
                            try
                            {
                                string FilePath = Path.Combine(tempFolderPath, FileName);
                                file.filepath = file.filepath.Replace(' ', '+');
                                byte[] imageBytes = Convert.FromBase64String(file.filepath);
                                File.WriteAllBytes(FilePath, imageBytes);
                            }
                            catch (Exception ec)
                            {
                                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ec.Message);
                            }
                        }
                        string filePath = string.Concat("/", tempFolderName, "/", FileName);

                        customer.ProfileImage = ImageDomain + "/" + filePath;
                        Result = HSapiFacade.UpdateCustomer(customer);
                        return Request.CreateResponse(HttpStatusCode.OK, new { filePath = filePath, filename = file.filename, file = file.filename, ImagePath = customer.ProfileImage });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "filepath should not be empty");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "token expired");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Route("CustomerInspectionImageUpload")]
        public HttpResponseMessage CustomerInspectionImageUpload([FromBody] FileType file)
        {
            APIInitialize();
            string ImageDomain = AppConfig.ImageDomain;
            Guid CustomerId = Guid.Empty;
            int InspectionId = 0;
            bool Result = false;

            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("CustomerId"))
            {
                string CustomerIdSt = headers.GetValues("CustomerId").First();
                Guid.TryParse(CustomerIdSt, out CustomerId);
            }
            if (headers.Contains("InspectionId"))
            {
                int.TryParse(headers.GetValues("InspectionId").First(), out InspectionId);
            }

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    if (!string.IsNullOrWhiteSpace(file.filename) && !string.IsNullOrWhiteSpace(file.filepath))
                    {
                        var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                        if (usercontext == null)
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                        var customer = HSapiFacade.GetCustomerByCustomerId(CustomerId);
                        if (customer == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Customer Not Found.");
                        }
                        var Inspection = HSapiFacade.GetCustomerInspectionByCustomerIdAndInspectionId(customer.CustomerId, InspectionId);
                        if (Inspection == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Customer Id & Inspection Id Not Match.");
                        }
                        string tempFolderName = ConfigurationManager.AppSettings["File.UploadFiles"];
                        var comname = HSapiFacade.GetCompanyByCompanyId(usercontext.CompanyId).CompanyName;
                        tempFolderName = string.Format(tempFolderName, comname.ReplaceSpecialChar());
                        tempFolderName += "/" + DateTime.Now.Month + "-" + DateTime.Now.Day;

                        Random rand = new Random();
                        string FileName = rand.Next().ToString();
                        FileName += "-___" + file.filename;
                        string tempFolderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + tempFolderName);

                        if (CreateFolderIfNeeded(tempFolderPath))
                        {
                            try
                            {
                                string FilePath = Path.Combine(tempFolderPath, FileName);
                                file.filepath = file.filepath.Replace(' ', '+');
                                byte[] imageBytes = Convert.FromBase64String(file.filepath);
                                File.WriteAllBytes(FilePath, imageBytes);
                            }
                            catch (Exception ec)
                            {
                                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ec.Message);
                            }
                        }
                        string filePath = string.Concat("/", tempFolderName, "/", FileName);
                        Inspection.InspectionPhoto = ImageDomain + "/" + filePath;
                        Result = HSapiFacade.UpdateCustomerInspection(Inspection);
                        return Request.CreateResponse(HttpStatusCode.OK, new { filePath = filePath, filename = file.filename, file = file.filename, ImagePath = Inspection.InspectionPhoto });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "filepath should not be empty");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "token expired");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Route("CustomerImageDelete")]
        public HttpResponseMessage CustomerImageDelete()
        {
            APIInitialize();
            int CustomerId = 0;
            bool result = false;
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("CustomerId"))
            {
                CustomerId = Int32.Parse(headers.GetValues("CustomerId").First());
            }
            var identity = (ClaimsIdentity)User.Identity;
            if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
            {
                var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                Customer Customer = HSapiFacade.GetCustomerById(CustomerId);
                if (Customer == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Customer Not Found.");

                if (Customer.ProfileImage != null && Customer.ProfileImage != string.Empty)
                {
                    Customer.ProfileImage = string.Empty;
                    result = HSapiFacade.UpdateCustomer(Customer);
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { result = result });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
            }
        }

        [Route("UploadImageFileFormData")]
        public HttpResponseMessage UploadImageFileFormData()
        {
            APIInitialize();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string tempFileName = "";
                var file = HttpContext.Current.Request.Files.Count > 0 ?
                HttpContext.Current.Request.Files[0] : null;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                        if (usercontext == null)
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                        string tempFolderName = ConfigurationManager.AppSettings["File.UploadFiles"];
                        var comname = HSapiFacade.GetCompanyByCompanyId(usercontext.CompanyId).CompanyName;
                        tempFolderName = string.Format(tempFolderName, comname.ReplaceSpecialChar());
                        tempFolderName += "/" + DateTime.Now.Month + "-" + DateTime.Now.Day;

                        Random rand = new Random();
                        string FileName = rand.Next().ToString();
                        FileName += "-___" + file.FileName;
                        string tempFolderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + tempFolderName);

                        if (CreateFolderIfNeeded(tempFolderPath))
                        {
                            try
                            {
                                string FilePath = Path.Combine(tempFolderPath, FileName);
                                file.SaveAs(FilePath);
                                var spimg = file.FileName.Split('.');
                                if (spimg.Length > 0 && (spimg[1].ToLower() == "png" || spimg[1].ToLower() == "jpg" || spimg[1].ToLower() == "jpeg"))
                                {
                                    string tempimgpath = System.Web.Hosting.HostingEnvironment.MapPath(string.Concat("/", tempFolderName, "/", FileName));
                                    Image img = Image.FromFile(tempimgpath);
                                    tempFileName = rand.Next().ToString();
                                    tempFileName += "-___" + file.FileName;
                                    img.Save(Path.Combine(tempFolderPath, tempFileName), ImageFormat.Jpeg);
                                }
                                else
                                {
                                    tempFileName = FileName;
                                }
                            }
                            catch (Exception ec)
                            {
                                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ec.Message);
                            }
                        }
                        string filePath = string.Concat("/", tempFolderName, "/", tempFileName);
                        return Request.CreateResponse(HttpStatusCode.OK, new { filePath = filePath, filename = file.FileName });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "filepath should not be empty");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "token expired");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        public static Bitmap GetImageResize(int maxWidth, int maxHeight, Image image)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            Graphics thumbGraph = Graphics.FromImage(newImage);

            thumbGraph.CompositingQuality = CompositingQuality.Default;
            thumbGraph.SmoothingMode = SmoothingMode.Default;

            //thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            thumbGraph.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
        #endregion

        #region User Employee API

        [Route("ForgetPassword")]
        public HttpResponseMessage ForgetPassword()
        {
            string EmailAddress = "";
            bool Status = false;
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("EmailAddress"))
            {
                EmailAddress = headers.GetValues("EmailAddress").First();
            }
            var identity = (ClaimsIdentity)User.Identity;
            var usercontext = new CompanyConneciton();
            try
            {
                ForgotPasswordAPIInitialize(EmailAddress);
                if (!string.IsNullOrWhiteSpace(ComId))
                {
                    usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(EmailAddress, new Guid(ComId));
                }
                else
                {
                    usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(EmailAddress);
                }
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                UserLogin ul = HSapiFacade.GetUserByUsernameAndCompanyId(EmailAddress, usercontext.CompanyId);
                if (ul == null)
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "User not found");
                }
                Employee emp = HSapiFacade.GetEmployeeByUserId(ul.UserId);
                if (emp == null)
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "User not found");
                }
                ul.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                HSapiFacade.UpdateUserLogin(ul);

                string cryptmessage = DESEncryptionDecryption.EncryptPlainTextToCipherText(ul.Id + "__" + ul.UserName + "__" + DateTime.Today.AddDays(1) + "__" + ul.LastUpdatedDate);

                //UtilHelper.GetCryptMessage(ulId + email + ul2.LastUpdatedDate.ToString());

                ResetPasswordEmail verifyEmail = new ResetPasswordEmail();
                verifyEmail.Name = string.Format("{0} {1}", emp.FirstName, emp.LastName);
                string SiteURL = ConfigurationManager.AppSettings["SiteURL"];
                verifyEmail.EmailVerificationLink = SiteURL + string.Format("/resetpass/{1}/{0}", ul.Id, cryptmessage);
                verifyEmail.ToEmail = ul.EmailAddress;
                var Result = HSapiFacade.SendEmailResetPassword(verifyEmail, usercontext.CompanyId);
                Status = true;

                return Request.CreateResponse(HttpStatusCode.OK, new { Status = Status, message = "Password reset email sent successfully!The email sent contains a password reset link which will expire after " + DateTime.Now.UTCCurrentTime().AddDays(1).ToString("MMM, dd yyyy") + ".", url = verifyEmail.EmailVerificationLink });
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, e.Message);
            }
        }

        [Authorize]
        [Route("ChangePassword")]
        public HttpResponseMessage ChangePassword()
        {
            APIInitialize();
            string UserName = "";
            string Password = "";
            string NewPassword = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("Username"))
            {
                UserName = headers.GetValues("Username").First();
            }
            if (headers.Contains("Password"))
            {
                Password = headers.GetValues("Password").First();
                Password = MD5Encryption.GetMD5HashData(Password);
            }
            if (headers.Contains("NewPassword"))
            {
                NewPassword = headers.GetValues("NewPassword").First();
                NewPassword = MD5Encryption.GetMD5HashData(NewPassword);
            }
            bool result = false;
            var identity = (ClaimsIdentity)User.Identity;
            var usercontext = HSMainApiFacade.GetUsersOrganizationListByUsername(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
            if (usercontext == null && usercontext.Count == 0)
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new { result = false, message = "No organization found." });
            foreach (var item in usercontext)
            {
                HSapiFacade = new HSApiFacade(item.ConnectionString);
                //UserLogin obUser = HSapiFacade.GetUserByUsernameAndPasswordAndCompanyId(UserName, Password, item.CompanyId);
                UserLogin obUser = HSapiFacade.GetUserByUsernameAndCompanyId(UserName, item.CompanyId);
                if (obUser == null)
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new { result = false, message = "User not found" });
                }
                Employee emp = HSapiFacade.GetEmployeeByUserId(obUser.UserId);
                if (emp == null)
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new { result = false, message = "User not found" });
                }
                obUser.Password = NewPassword;
                result = HSapiFacade.UpdateUserLogin(obUser);
            }

            if (result)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = true, message = "Password Changed successfully." });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new { result = true, message = "Something wrong." });
            }
        }

        [Route("profile")]
        [HttpGet]
        public HttpResponseMessage profile()
        {
            string Username = "";
            Guid guid = Guid.Empty;
            var re = Request;
            var headers = re.Headers;

            // Retrieve Username and Guid from headers
            if (headers.Contains("Username"))
            {
                Username = headers.GetValues("Username").First();
            }

            if (headers.Contains("guid"))
            {
                Guid.TryParse(headers.GetValues("guid").First(), out guid);
            }

            var identity = (ClaimsIdentity)User.Identity;
            var usercontext = new CompanyConneciton();

            // Check if the identity has the necessary claims
            if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
            {
                usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                if (usercontext == null)
                {
                    var errorResponse = ApiResponse<object>.Error("Authorization Denied.");
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, errorResponse);
                }
                ComId = usercontext.CompanyId.ToString();
                APIInitialize();


                // Use guid or username for retrieving the user
                UserLogin ul = guid != Guid.Empty
                    ? HSapiFacade.GetUserByGuidAndCompanyId(guid, usercontext.CompanyId)
                    : HSapiFacade.GetUserByUsernameAndCompanyId(Username, usercontext.CompanyId);

                if (ul == null)
                {
                    var errorResponse = ApiResponse<object>.Error("User not found");
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, errorResponse);
                }

                EmployeeAPIModel emp = HSapiFacade.GetEmployeeWithRoleByUserId(ul.UserId);
                if (emp == null)
                {
                    var errorResponse = ApiResponse<object>.Error("User not found");
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, errorResponse);
                }

                Company company = HSapiFacade.GetCompanyByCompanyId(usercontext.CompanyId);

                var result = new
                {
                    profile = new
                    {
                        id = emp.Id,
                        userGuid = emp.UserId,
                        firstName = emp.FirstName,
                        lastName = emp.LastName,
                        username = emp.UserName,
                        email = emp.Email,
                        phone = emp.Phone,
                        profilePicture = emp.ProfilePicture,
                    },
                    company = new
                    {
                        id = company.Id,
                        companyGuid = company.CompanyId,
                        name = company.CompanyName,
                        email = company.EmailAdress,
                        phone = company.Phone,
                        website = company.Website,
                        logo = company.CompanyLogo,
                        street = company.Street,
                        city = company.City,
                        state = company.State,
                        zip = company.ZipCode,
                    }
                };

                var successResponse = ApiResponse<object>.Success(result);
                return Request.CreateResponse(HttpStatusCode.OK, successResponse);
            }
            else
            {
                var errorResponse = ApiResponse<object>.Error("Token Expired.");
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, errorResponse);
            }
        }


        [Authorize]
        [Route("DeleteCustomer")]
        public HttpResponseMessage DeleteCustomer()
        {
            APIInitialize();
            int customerid = 0;
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("customerid"))
            {
                customerid = Int32.Parse(headers.GetValues("customerid").First());
            }
            var identity = (ClaimsIdentity)User.Identity;
            if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
            {
                var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                Customer customer = HSapiFacade.GetCustomerById(customerid);
                if (customer == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Customer Not Found.");

                bool result = HSapiFacade.DeleteCustomer(customerid);

                return Request.CreateResponse(HttpStatusCode.OK, new { result = result });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
            }

        }

        #endregion

        #region Estimate

        [Authorize]
        [Route("CreateEstimate")]
        public HttpResponseMessage CreateEstimate([FromBody] EstimateListModel estimate)
        {
            APIInitialize();
            int EstimateId = 0;
            Guid CustomerId = Guid.Empty;
            Guid CompanyId = Guid.Empty;
            int Quantity = 0;
            double UnitPrice = 0;
            double Discount = 0;
            double Amount = 0;
            double Tax = 0;
            double DiscountAmount = 0;
            double TotalAmount = 0;
            double DiscountPercent = 0;
            string Note = "";
            string EquipName = "";
            string EquipDetail = "";
            string DiscountType = "";
            string Drawingimage = "";
            string Cameraimage = "";
            string Signimage = "";
            string TaxType = "";
            string Term = "";
            DateTime DueDate = new DateTime();
            DateTime CreatedDate = new DateTime();
            bool sentemail = false;
            bool result = false;


            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("EstimateId"))
            {
                int.TryParse(headers.GetValues("EstimateId").First(), out EstimateId);
            }
            if (headers.Contains("Amount"))
            {
                string StAmount = headers.GetValues("Amount").First();
                double.TryParse(StAmount, out Amount);
            }
            if (headers.Contains("Tax"))
            {
                string StTax = headers.GetValues("Tax").First();
                double.TryParse(StTax, out Tax);
            }
            if (headers.Contains("DiscountAmount"))
            {
                string StDiscountAmount = headers.GetValues("DiscountAmount").First();
                double.TryParse(StDiscountAmount, out DiscountAmount);
            }
            if (headers.Contains("TotalAmount"))
            {
                string StTotalAmount = headers.GetValues("TotalAmount").First();
                double.TryParse(StTotalAmount, out TotalAmount);
            }
            if (headers.Contains("DiscountPercent"))
            {
                string StDiscountpercent = headers.GetValues("DiscountPercent").First();
                double.TryParse(StDiscountpercent, out DiscountPercent);
            }
            if (headers.Contains("DiscountType"))
            {
                DiscountType = headers.GetValues("DiscountType").First();
            }
            if (headers.Contains("Note"))
            {
                Note = headers.GetValues("Note").First();
            }
            if (headers.Contains("DueDate"))
            {
                string StDueDate = headers.GetValues("DueDate").First();
                DateTime.TryParse(StDueDate, out DueDate);
            }
            if (headers.Contains("CreatedDate"))
            {
                string StCreatedDate = headers.GetValues("CreatedDate").First();
                DateTime.TryParse(StCreatedDate, out CreatedDate);
            }
            if (headers.Contains("CustomerId"))
            {
                string CustomerIdSt = headers.GetValues("CustomerId").First();
                Guid.TryParse(CustomerIdSt, out CustomerId);
            }
            if (headers.Contains("EquipName"))
            {
                EquipName = headers.GetValues("EquipName").First();
            }
            if (headers.Contains("EquipDetail"))
            {
                EquipDetail = headers.GetValues("EquipDetail").First();
            }
            if (headers.Contains("UnitPrice"))
            {
                string StUnitPrice = headers.GetValues("UnitPrice").First();
                double.TryParse(StUnitPrice, out UnitPrice);
            }
            if (headers.Contains("Discount"))
            {
                string StDiscount = headers.GetValues("Discount").First();
                double.TryParse(StDiscount, out Discount);
            }
            if (headers.Contains("Quantity"))
            {
                int.TryParse(headers.GetValues("Quantity").First(), out Quantity);
            }
            if (headers.Contains("CompanyId"))
            {
                string CompanyIdSt = headers.GetValues("CompanyId").First();
                Guid.TryParse(CompanyIdSt, out CompanyId);
            }
            if (headers.Contains("Drawingimage"))
            {
                Drawingimage = headers.GetValues("Drawingimage").First();
            }
            if (headers.Contains("Cameraimage"))
            {
                Cameraimage = headers.GetValues("Cameraimage").First();
            }
            if (headers.Contains("Signimage"))
            {
                Signimage = headers.GetValues("Signimage").First();
            }
            if (headers.Contains("TaxType"))
            {
                TaxType = headers.GetValues("TaxType").First();
            }
            if (headers.Contains("Term"))
            {
                Term = headers.GetValues("Term").First();
            }
            if (headers.Contains("sentemail"))
            {
                sentemail = !string.IsNullOrWhiteSpace(headers.GetValues("sentemail").First()) ? Convert.ToBoolean(headers.GetValues("sentemail").First()) : false;
            }
            if (CustomerId == null || CustomerId == Guid.Empty)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new { result, message = "Please Insert Customer Id." });
            }
            if (CompanyId == null || CompanyId == Guid.Empty)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new { result, message = "Please Insert Company Id." });
            }
            try
            {
                string filename = "";
                CreateInvoice CreateInvoice = new CreateInvoice();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    Customer customer = HSapiFacade.GetCustomerByCustomerId(CustomerId);
                    if (customer == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Customer Not Found.");
                    Employee emp = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (emp == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "User Not Found.");
                    Company company = HSapiFacade.GetCompanyByCompanyId(CompanyId);
                    if (company == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Company Not Found.");
                    var EstimateDetails = HSapiFacade.GetInvoiceByEstimateIdAndCustomerId(EstimateId, customer.CustomerId);
                    if (EstimateDetails == null)
                    {
                        Invoice inv = new Invoice()
                        {
                            Amount = 0,
                            Tax = 0,
                            DiscountAmount = 0,
                            TotalAmount = 0,
                            Discountpercent = 0,
                            Description = "",
                            CreatedBy = CreatedBy,
                            DueDate = DateTime.Now.UTCCurrentTime(),
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            CustomerId = Guid.Empty,
                            CompanyId = Guid.Empty,
                            LastUpdatedDate = DateTime.Now,
                            LastUpdatedByUid = Guid.Empty,
                            Status = "Open",
                            IsEstimate = true,
                            Balance = 0,
                            BalanceDue = 0
                        };
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Invoice not found.");
                    }

                    else
                    {
                        EstimateDetails.Amount = Amount;
                        EstimateDetails.Tax = Tax;
                        EstimateDetails.DiscountAmount = DiscountAmount;
                        EstimateDetails.TotalAmount = TotalAmount;
                        EstimateDetails.Discountpercent = DiscountPercent;
                        EstimateDetails.Description = Note;
                        EstimateDetails.DueDate = DueDate;
                        EstimateDetails.Status = "Open";
                        EstimateDetails.BalanceDue = TotalAmount;
                        EstimateDetails.DiscountType = DiscountType;
                        EstimateDetails.TaxType = TaxType;
                        EstimateDetails.Terms = Term;
                        result = HSapiFacade.UpdateInvoice(EstimateDetails);


                        InvoiceDetail InvoiceDetail;

                        var ExistingInvoiceDetail = HSapiFacade.GetInvoiceDetailsByInvoiceId(EstimateDetails.InvoiceId);
                        if (ExistingInvoiceDetail != null)
                        {
                            HSapiFacade.DeleteExistingInvoiceDetail(EstimateDetails.InvoiceId);
                        }
                        if (estimate != null && estimate.ListEstimate != null && estimate.ListEstimate.Count > 0)
                        {
                            foreach (var item in estimate.ListEstimate)
                            {
                                InvoiceDetail = new InvoiceDetail()
                                {
                                    InvoiceId = EstimateDetails.InvoiceId,
                                    EquipName = item.EquipName,
                                    EquipDetail = item.EquipDetail,
                                    CompanyId = EstimateDetails.CompanyId,
                                    UnitPrice = item.UnitPrice,
                                    Quantity = item.Quantity,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    EquipmentId = item.EquipmentId,
                                    TotalPrice = item.TotalPrice,
                                    DiscountAmount = item.DiscountAmount,
                                    DiscountPercent = item.DiscountPercent,
                                    DiscountType = item.DiscountType
                                };

                                HSapiFacade.InsertInvoiceDetail(InvoiceDetail);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(Drawingimage))
                        {
                            var estimateimgobj = HSapiFacade.GetEstimateImageByInvoiceIdAndImageType(EstimateDetails.InvoiceId, "Draw");
                            if (estimateimgobj != null)
                            {
                                estimateimgobj.ImageLoc = Drawingimage;
                                estimateimgobj.UploadedDate = DateTime.Now.UTCCurrentTime();
                                HSapiFacade.UpdateEstimateImage(estimateimgobj);
                            }
                            else
                            {
                                EstimateImage EstimateImage = new EstimateImage()
                                {
                                    CompanyId = EstimateDetails.CompanyId,
                                    CustomerId = EstimateDetails.CustomerId,
                                    InvoiceId = EstimateDetails.InvoiceId,
                                    ImageLoc = Drawingimage,
                                    ImageType = "Draw",
                                    UploadedDate = DateTime.Now.UTCCurrentTime(),
                                    CreatedBy = emp.UserId
                                };
                                HSapiFacade.InsertEstimateImage(EstimateImage);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(Cameraimage))
                        {
                            var estimateimgobj = HSapiFacade.GetEstimateImageByInvoiceIdAndImageType(EstimateDetails.InvoiceId, "Camera");
                            if (estimateimgobj != null)
                            {
                                estimateimgobj.ImageLoc = Cameraimage;
                                estimateimgobj.UploadedDate = DateTime.Now.UTCCurrentTime();
                                HSapiFacade.UpdateEstimateImage(estimateimgobj);
                            }
                            else
                            {
                                EstimateImage EstimateImage = new EstimateImage()
                                {
                                    CompanyId = EstimateDetails.CompanyId,
                                    CustomerId = EstimateDetails.CustomerId,
                                    InvoiceId = EstimateDetails.InvoiceId,
                                    ImageLoc = Cameraimage,
                                    ImageType = "Camera",
                                    UploadedDate = DateTime.Now.UTCCurrentTime(),
                                    CreatedBy = emp.UserId
                                };
                                HSapiFacade.InsertEstimateImage(EstimateImage);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(Signimage))
                        {
                            var estimateimgobj = HSapiFacade.GetEstimateImageByInvoiceIdAndImageType(EstimateDetails.InvoiceId, "Sign");
                            if (estimateimgobj != null)
                            {
                                estimateimgobj.ImageLoc = Signimage;
                                estimateimgobj.SignDate = DateTime.Now.UTCCurrentTime();
                                HSapiFacade.UpdateEstimateImage(estimateimgobj);
                            }
                            else
                            {
                                EstimateImage EstimateImage = new EstimateImage()
                                {
                                    CompanyId = EstimateDetails.CompanyId,
                                    CustomerId = EstimateDetails.CustomerId,
                                    InvoiceId = EstimateDetails.InvoiceId,
                                    ImageLoc = Signimage,
                                    ImageType = "Sign",
                                    SignDate = DateTime.Now.UTCCurrentTime(),
                                    UploadedDate = DateTime.Now.UTCCurrentTime(),
                                    CreatedBy = emp.UserId
                                };
                                HSapiFacade.InsertEstimateImage(EstimateImage);
                            }
                        }
                        EstimateEmailModel EstimateEmailModel = new EstimateEmailModel();
                        EstimateSMSModel EstimateSMSModel = new EstimateSMSModel();
                        string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(EstimateDetails.InvoiceId + "#" + identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                        filename = ConfigurationManager.AppSettings["SiteURL"] + "/Estimate/GetPdf/?code=" + encryptedurl;
                        if (sentemail)
                        {
                            var predefinedemailtempobj = HSapiFacade.GetEmailTemplateByTemplateKeyAndCompanyId(usercontext.CompanyId, "EstimatePredefineEmailTemplate");
                            var comobj = HSapiFacade.GetCompanyByCompanyId(CompanyId);
                            if (predefinedemailtempobj != null)
                            {
                                EstimateEmailModel.email = customer.EmailAddress;
                                EstimateEmailModel.subject = "New Estimate from " + comobj.CompanyName + " " + EstimateDetails.InvoiceId;
                                EstimateEmailModel.bodycontent = HtmlToPlainText("<p><span data-sheets-value=\"{&quot;1&quot;:2,&quot;2&quot;:&quot;Dear " + customer.FirstName + " " + customer.LastName + ",\\n\\nWe appreciate the opportunity to give you an estimate on our products and services. Please click on the link below to review and approve the estimate. \\n\\n" + filename + " \\n\\nThis estimate is valid until " + (EstimateDetails.DueDate.HasValue ? EstimateDetails.DueDate.Value.ToString("MM/dd/yy") : "") + ". If you have any questions, please call me directly at " + emp.Phone + ". \\n\\nThank you for your business,\\n" + emp.FirstName + " " + emp.LastName + "\\n" + company.CompanyName + "&quot;}\" data-sheets-userformat=\"{&quot;2&quot;:4480,&quot;10&quot;:2,&quot;11&quot;:4,&quot;15&quot;:&quot;arial,sans,sans-serif&quot;}\"><span style=\"font-weight:600;\">Dear " + customer.FirstName + " " + customer.LastName + "</span>,<br /><br />We appreciate the opportunity to give you an estimate on our products and services. Please click on the link below to review and approve the estimate. <br /><br /><a style=\"line-height:2.6666666666666665rem;background-color:#2ca01c;color:#333;border:1px solid #d6d6d6;-webkit-transition:background-color .2s ease-in;transition:background-color .2s ease-in;font-size:1rem;font-family:'Open Sans',Helvetica,Arial,sans-serif;font-weight:400;display:block;height:2.8rem;-webkit-box-sizing:border-box;-moz-box-sizing: border-box;-o-box-sizing:border-box;-ms-box-sizing:border-box;box-sizing:border-box;-webkit-border-radius:4px;-moz-border-radius:4px;border-radius:4px;-moz-background-clip:padding;-webkit-background-clip:padding-box;background-clip:padding-box;text-align:center;vertical-align:middle;margin:0 auto 11px;padding:0 15px;white-space:nowrap;cursor:pointer;-webkit-user-select:none;-moz-user-select:none; -ms-user-select: none; -o-user-select: none; width: 100%; max-width: 165px; text-decoration:none; color:white; font-weight:600;\" href=\"" + AppConfig.SiteDomain + "/" + filename + "\">View Estimate</a><br /><br />This estimate is valid until " + EstimateDetails.DueDate + ". If you have any questions, please call me directly at " + emp.Phone + ". <br /><br />Thank you for your business,<br />" + emp.FirstName + " " + emp.LastName + "<br />" + company.CompanyName + "</span></p>");
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Predefined email template should not empty");
                            }
                            var predefinedsmstempobj = HSapiFacade.GetEmailTemplateByTemplateKeyAndCompanyId(usercontext.CompanyId, "EstimatePredefineSMSTemplate");
                            if (predefinedsmstempobj != null)
                            {
                                EstimateSMSModel.contactno = !string.IsNullOrWhiteSpace(customer.PrimaryPhone) ? customer.PrimaryPhone : !string.IsNullOrWhiteSpace(customer.SecondaryPhone) ? customer.SecondaryPhone : !string.IsNullOrWhiteSpace(customer.CellNo) ? customer.CellNo : "";
                                EstimateSMSModel.smsbody = HtmlToPlainText("<p><span data-sheets-value=\"{&quot;1&quot;:2,&quot;2&quot;:&quot;New Estimate from " + company.CompanyName + ": " + EstimateDetails.InvoiceId + "\\n\\n" + filename + "&quot;}\" data-sheets-userformat=\"{&quot;2&quot;:4224,&quot;10&quot;:2,&quot;15&quot;:&quot;arial,sans,sans-serif&quot;}\">New Estimate from " + company.CompanyName + ": " + EstimateDetails.InvoiceId + "<br /><br />" + filename + "</span></p>");
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Predefined sms template should not empty");
                            }

                        }
                        return Request.CreateResponse(HttpStatusCode.OK, new { result, EstimateEmailModel = EstimateEmailModel, EstimateSMSModel = EstimateSMSModel, filepath = filename });
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }

        }

        [Authorize]
        [Route("GenerateEstimate")]
        public HttpResponseMessage GenerateEstimate()
        {
            APIInitialize();
            int CustomerId = 0;
            bool Result = false;

            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("CustomerId"))
            {
                int.TryParse(headers.GetValues("CustomerId").First(), out CustomerId);
            }
            var cus = HSapiFacade.GetCustomerById(CustomerId);

            if (cus == null)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new { Result });
            }
            CreateInvoice model = new CreateInvoice();
            var identity = (ClaimsIdentity)User.Identity;
            if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
            {
                var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                var com = HSapiFacade.GetCompanyByCompanyId(usercontext.CompanyId);
                var user = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                model.EmailAddress = cus.EmailAddress;

                var AddressTemplate = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "CustomerAddressPdfFormat") != null ? HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "CustomerAddressPdfFormat").Value : "";

                model.Invoice = new Invoice()
                {
                    CustomerId = cus.CustomerId,
                    IsEstimate = true,
                    IsBill = false,
                    CompanyId = com.CompanyId,
                    Amount = 0,
                    BalanceDue = 0,
                    Balance = 0,
                    Deposit = 0,
                    Tax = 0,
                    LateFee = 0,
                    LateAmount = 0,
                    Status = "Init",
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CreatedBy = user.FirstName,
                    InvoiceFor = "Others",
                    InvoiceDate = DateTime.Now.UTCCurrentTime(),
                    DueDate = DateTime.Now.UTCCurrentTime(),
                    CreatedByUid = user.UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedByUid = user.UserId
                };
                if (!string.IsNullOrWhiteSpace(cus.BusinessName))
                {
                    model.CusBussinessName = cus.BusinessName;
                }
                if (cus.Type == "Commercial")
                {
                    model.CusType = cus.Type;
                }
                model.Invoice.BillingAddress = AddressHelper.MakeCustomerAddress(cus, "BillingAddress", AddressTemplate);
                model.Invoice.ShippingAddress = AddressHelper.MakeCustomerAddress(cus, "ShippingAddress", AddressTemplate);
                model.Invoice.LastUpdatedByUid = user.UserId;
                model.Invoice.Id = HSapiFacade.InsertInvoice(model.Invoice);
                model.Invoice.InvoiceId = model.Invoice.Id.GenerateEstimateNo();
                model.Invoice.InvoiceFor = "Estimate";
                Result = HSapiFacade.UpdateInvoice(model.Invoice);

                model.Invoice.InvoiceDate = model.Invoice.InvoiceDate.Value.UTCToClientTime();
                model.Invoice.DueDate = model.Invoice.DueDate.Value.UTCToClientTime();

                model.InvoiceDetailList = new List<InvoiceDetail>();
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Result = Result,
                    Taxrate = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "Sales Tax") != null ? HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "Sales Tax").Value : "0",
                    InvoiceId = model.Invoice.InvoiceId,
                    Invoice = model.Invoice
                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
            }
        }

        [Authorize]
        [Route("DeleteEstimate")]
        public HttpResponseMessage DeleteEstimate()
        {
            APIInitialize();
            int EstimateId = 0;
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("EstimateId"))
            {
                EstimateId = Int32.Parse(headers.GetValues("EstimateId").First());
            }

            var identity = (ClaimsIdentity)User.Identity;
            if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
            {
                var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                Invoice Estimate = HSapiFacade.GetInvoiceByEstimateId(EstimateId);
                if (Estimate == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Estimate Not Found.");

                bool result = HSapiFacade.DeleteEstimateByEstimateID(EstimateId);

                return Request.CreateResponse(HttpStatusCode.OK, new { result = result });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
            }

        }

        [Authorize]
        [Route("GetEstimateById")]
        public HttpResponseMessage GetEstimateById()
        {
            APIInitialize();
            string EstimateId = "";

            var re = Request;
            var headers = re.Headers;
            bool result = false;


            if (headers.Contains("EstimateId"))
            {
                EstimateId = headers.GetValues("EstimateId").First();
            }
            if (string.IsNullOrWhiteSpace(EstimateId))
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            try
            {
                string customerName = "";
                string customerEmail = "";
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");

                    var Estimate = HSapiFacade.GetInvoiceByInvoiceId(EstimateId);
                    var EstimateDetails = HSapiFacade.GetInvoiceDetialsListByInvoiceId(EstimateId);
                    var EstimateImage = HSapiFacade.GetEstimateImageListByInvoiceId(EstimateId);
                    if (Estimate != null)
                    {
                        Customer customer = HSapiFacade.GetCustomerByCustomerId(Estimate.CustomerId);
                        if (customer != null)
                        {
                            customerName = !string.IsNullOrWhiteSpace(customer.FirstName + " " + customer.LastName) ? customer.FirstName + " " + customer.LastName : customer.FirstName + " " + customer.LastName;
                            customerEmail = customer.EmailAddress;
                        }
                        result = true;
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = result, customerName = customerName, customerEmail = customerEmail, Estimate = Estimate, EstimateDetails = EstimateDetails, EstimateImage = EstimateImage });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Authorize]
        [Route("EstimateDuplicate")]
        public HttpResponseMessage EstimateDuplicate()
        {
            APIInitialize();
            int EstimateId = 0;
            Guid CompanyId = Guid.Empty;
            string InvoiceName = "Estimate";

            bool Result = false;
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("EstimateId"))
            {
                int.TryParse(headers.GetValues("EstimateId").First(), out EstimateId);
            }
            if (headers.Contains("CompanyId"))
            {
                CompanyId = new Guid(headers.GetValues("CompanyId").First());
            }

            var oldinv = HSapiFacade.GetInvoiceByEstimateId(EstimateId);
            Invoice inv = HSapiFacade.GetInvoiceByEstimateId(EstimateId);

            #region validations
            if (inv == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { Result });
            }
            else if (inv.IsEstimate && inv.Status == LabelHelper.EstimateStatus.Init)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { Result });
            }
            else if (inv.IsEstimate && inv.Status == LabelHelper.InvoiceStatus.Init)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { Result });
            }
            else if (oldinv.CompanyId != CompanyId)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { Result });
            }

            #endregion

            var identity = (ClaimsIdentity)User.Identity;
            if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
            {
                var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");

                inv.Id = HSapiFacade.InsertInvoice(inv);
                inv.InvoiceId = inv.Id.GenerateEstimateNo();
                inv.Status = "Open";
                Result = HSapiFacade.UpdateInvoice(inv);

                List<InvoiceDetail> InvDet = HSapiFacade.GetInvoiceDetailsByInvoiceId(oldinv.InvoiceId);
                #region Insert Invoice Details
                foreach (var item in InvDet)
                {
                    item.InvoiceId = inv.InvoiceId;
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    item.CreatedBy = User.Identity.Name;
                    item.Id = 0;
                    HSapiFacade.InsertInvoiceDetail(item);
                }
                #endregion

                List<EstimateImage> EsImage = HSapiFacade.GetimageInvoiceId(oldinv.InvoiceId);
                #region Insert Estimate Image
                foreach (var item in EsImage)
                {
                    item.InvoiceId = inv.InvoiceId;
                    HSapiFacade.InsertEstimateImage(item);
                }
                #endregion
                return Request.CreateResponse(HttpStatusCode.OK, new { Result = true, message = string.Format("Estimate cloned successfully with new {1} id {0}.", inv.InvoiceId, InvoiceName) });
            }
            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
        }

        #endregion

        #region Recommended Level

        [Authorize]
        [Route("RecommendedLevel")]
        public HttpResponseMessage RecommendedLevel()
        {
            APIInitialize();
            int ID = 0;
            int RecommendedLevel = 0;
            var re = Request;
            var headers = re.Headers;
            bool result = false;
            if (headers.Contains("ID"))
            {
                int.TryParse(headers.GetValues("ID").First(), out ID);
            }
            if (headers.Contains("RecommendedLevel"))
            {
                int.TryParse(headers.GetValues("RecommendedLevel").First(), out RecommendedLevel);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    Customer customer = HSapiFacade.GetCustomerById(ID);
                    if (customer == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Customer Not Found.");

                    var CustomerDetails = HSapiFacade.GetCustomerById(ID);
                    if (CustomerDetails != null)
                    {
                        CustomerDetails.RecommendedLevel = RecommendedLevel;
                        result = HSapiFacade.UpdateCustomer(CustomerDetails);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new { result });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Authorize]
        [Route("GetRecommendedLevel")]
        public HttpResponseMessage GetRecommendedLevel()
        {
            APIInitialize();
            int ID = 0;
            int Value = 0;
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("ID"))
            {
                string stID = headers.GetValues("ID").First();
                int.TryParse(stID, out ID);
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    Customer customer = HSapiFacade.GetCustomerById(ID);
                    if (customer == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Customer Not Found.");

                    var CustomerDetails = HSapiFacade.GetCustomerById(ID);
                    if (CustomerDetails != null && CustomerDetails.RecommendedLevel.HasValue)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { CustomerDetails.RecommendedLevel.Value });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { Value });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        #endregion

        #region Product

        [Authorize]
        [Route("GetEquipmentListByKey")]
        public HttpResponseMessage GetEquipmentList()
        {
            APIInitialize();
            string Key = "";
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("Key"))
            {
                Key = headers.GetValues("Key").First();
            }

            var identity = (ClaimsIdentity)User.Identity;
            if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
            {
                var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");

                var EquipmentList = HSapiFacade.GetEquipmentListByName(Key);
                return Request.CreateResponse(HttpStatusCode.OK, new { EquipmentList });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
            }
        }

        #endregion

        #region Organization

        [Authorize]
        [Route("ChangeDefaultCompany")]
        public HttpResponseMessage ChangeDefaultCompany()
        {
            string Companyid = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("Companyid"))
            {
                Companyid = headers.GetValues("Companyid").First();
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(Companyid))
                {

                    APIInitialize();
                    UserLogin ul = HSapiFacade.GetUserByUsernameAndCompanyId(username, new Guid(Companyid));
                    if (ul != null && ul.IsActive == true)
                    {
                        HSMainApiFacade.SetDefaultUserCompany(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(Companyid));
                        CompanyConneciton CC = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                        if (CC != null)
                        {
                            HSapiFacade = new HSApiFacade(CC.ConnectionString);
                            ConnStr = CC.ConnectionString;
                            ComId = CC.CompanyId.ToString();
                            return Request.CreateResponse(HttpStatusCode.OK, new { result = true });
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "object reference null");
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "This user is not activated for choosen company. Please contact with administrator.");
                    }

                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "object reference null");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Authorize]
        [Route("GetOrganizationList")]
        public HttpResponseMessage GetOrganizationList()
        {
            APIInitialize();
            List<UserOrganization> ListOrganization = new List<UserOrganization>();
            List<APISelectListItem> orglist = new List<APISelectListItem>();
            string defaultlogo = ConfigurationManager.AppSettings["Logo.DefaultWhiteLogo"];
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ListOrganization = HSMainApiFacade.GetUsersOrganizationListByUsername(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());

                    if (ListOrganization.Count() > 0)
                    {
                        UserOrganization Tempcom = ListOrganization.Where(x => x.IsActive == true).FirstOrDefault();
                        if (Tempcom == null)
                        {
                            Tempcom = ListOrganization.FirstOrDefault();
                        }
                        orglist = ListOrganization.Select(x =>
                        new APISelectListItem()
                        {
                            text = x.CompanyName.ToString(),
                            value = x.CompanyId.ToString(),
                            selected = x.IsActive,
                            //image = HSapiFacade.GetMainBranchByCompanyId(x.CompanyId) != null && !string.IsNullOrWhiteSpace(HSapiFacade.GetMainBranchByCompanyId(x.CompanyId).Logo) ? HSapiFacade.GetMainBranchByCompanyId(x.CompanyId).Logo : defaultlogo
                        }).ToList();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { orglist = orglist });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "token expired");
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        #endregion

        public string GetCompanyAddressFormat(Guid CompanyId)
        {
            string CompanyAddressPdfFormat = RMRCacheKey.CompanyAddressPdfFormat + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[CompanyAddressPdfFormat] == null)
            {
                string SearchKey = "CompanyAddressPdfFormat";
                GlobalSetting globalsetting = HSapiFacade.GetGlobalSettingsByKey(CompanyId, SearchKey);
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[CompanyAddressPdfFormat] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[CompanyAddressPdfFormat];
            }
            return result;
        }

        [Authorize]
        [Route("SendEmailEstimate")]
        public HttpResponseMessage SendEmailEstimate()
        {
            APIInitialize();
            string toemail = "";
            string ccmail = "";
            string subject = "";
            string body = "";
            string imageurl = "";
            Guid customerid = new Guid();
            string invoiceid = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("toemail"))
            {
                toemail = headers.GetValues("toemail").First();
            }
            if (headers.Contains("ccmail"))
            {
                ccmail = headers.GetValues("ccmail").First();
            }
            if (headers.Contains("subject"))
            {
                subject = headers.GetValues("subject").First();
            }
            if (headers.Contains("body"))
            {
                body = headers.GetValues("body").First();
            }
            if (headers.Contains("customerid"))
            {
                Guid.TryParse(headers.GetValues("customerid").First(), out customerid);
            }
            if (headers.Contains("invoiceid"))
            {
                invoiceid = headers.GetValues("invoiceid").First();
            }
            if (headers.Contains("imageurl"))
            {
                imageurl = headers.GetValues("imageurl").First();
            }
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    Customer customer = HSapiFacade.GetCustomerByCustomerId(customerid);
                    if (customer == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "customer not found");
                    Company company = HSapiFacade.GetCompanyByCompanyId(usercontext.CompanyId);
                    if (company == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "company not found");
                    Invoice invoice = HSapiFacade.GetInvoiceByInvoiceId(invoiceid);
                    if (invoice == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "invoice not found");
                    Employee emp = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (emp == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "user not found");
                    string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(invoice.InvoiceId + "##" + identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault() + "##" + toemail + "##" + ccmail + "##" + emp.UserId + "##" + imageurl);
                    string EmailUrl = ConfigurationManager.AppSettings["SiteURL"] + "/Estimate/SendEmailEstimatePdfAPI/?code=" + encryptedurl;
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = true, EmailUrl = EmailUrl });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "token expired");
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Authorize]
        [Route("GetFavouriteEquipmentList")]
        public HttpResponseMessage GetFavouriteEquipmentList()
        {
            APIInitialize();

            var re = Request;
            var headers = re.Headers;
            List<FavouriteEquipmentsModel> model = new List<FavouriteEquipmentsModel>();
            var identity = (ClaimsIdentity)User.Identity;
            if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
            {
                var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                var empobj = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                var EquipmentList = HSapiFacade.GetAllEquipmentsFavouriteByUserIdAndCompanyId(usercontext.CompanyId, empobj != null ? empobj.UserId : new Guid());
                model.AddRange(EquipmentList.Select(x => new FavouriteEquipmentsModel()
                {
                    Name = x.Name,
                    SKU = x.SKU,
                    Retail = x.Retail.HasValue ? x.Retail.Value : 0,
                    Id = x.Id,
                    EquipmentId = x.EquipmentId,
                    Description = x.Description
                }));
                return Request.CreateResponse(HttpStatusCode.OK, new { model });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
            }
        }

        [Authorize]
        [Route("GlobalSearchCustomerAndLead")]
        public HttpResponseMessage GlobalSearchCustomerAndLead()
        {
            APIInitialize();
            string searchkey = "";
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("searchkey"))
            {
                searchkey = headers.GetValues("searchkey").First();
            }
            List<CustomerCustomModel> CustomerCustomModel = new List<CustomerCustomModel>();
            var identity = (ClaimsIdentity)User.Identity;
            if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
            {
                var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                List<Customer> model = HSapiFacade.GetGlobalSearchCustomerAndLeadByKey(searchkey, usercontext.CompanyId);
                CustomerCustomModel.AddRange(model.Select(x => new CustomerCustomModel()
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    EmailAddress = x.EmailAddress,
                    Street = x.Street,
                    City = x.City,
                    State = x.State,
                    Zip = x.ZipCode,
                    StreetType = x.StreetType,
                    Apartment = x.Appartment,
                    PrimaryPhone = x.PrimaryPhone,
                    SecondaryPhone = x.SecondaryPhone,
                    CellNo = x.CellNo,
                    CustomerType = x.CustomerType
                }));
                return Request.CreateResponse(HttpStatusCode.OK, new { CustomerCustomModel });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
            }
        }

        #region Time Clock
        [Authorize]
        [Route("ClockIn")]
        public HttpResponseMessage ClockIn()
        {
            APIInitialize();

            #region Input Params
            int Id = 0;
            string ClockInLat = "";
            string ClockInLng = "";
            string ClockInNote = "";
            //Guid UserId = Guid.Empty;

            #endregion

            var re = Request;
            var headers = re.Headers;
            bool result = false;

            #region Retrive Data From Headers

            if (headers.Contains("ClockInLat"))
            {
                ClockInLat = headers.GetValues("ClockInLat").First();
            }
            if (headers.Contains("ClockInLng"))
            {
                ClockInLng = headers.GetValues("ClockInLng").First();
            }
            if (headers.Contains("ClockInNote"))
            {
                ClockInNote = headers.GetValues("ClockInNote").First();
            }
            //if(headers.Contains("UserId"))
            //{
            //    int UserLoginId = 0;
            //    if (int.TryParse(headers.GetValues("UserId").First(), out UserLoginId) && UserLoginId > 0)
            //    {
            //        UserLogin ul = HSapiFacade.GetUserLoginById(UserLoginId);
            //        if (ul == null)
            //        {
            //            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "No user found for 'UserId'."));
            //        }

            //        UserId = ul.UserId;
            //    }
            //}
            #endregion 
            string message = "";
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string Username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));

                    EmployeeTimeClock tc = new EmployeeTimeClock();

                    Employee employee = HSapiFacade.GetEmployeeByUsername(Username);
                    if (employee == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                    }

                    #region InsertTimeClockIn
                    tc = new EmployeeTimeClock()
                    {
                        UserId = employee.UserId,
                        ClockInLat = ClockInLat,
                        ClockInLng = ClockInLng,
                        ClockInTime = DateTime.Now.UTCCurrentTime(),
                        ClockInNote = ClockInNote,
                        ClockInCreatedBy = employee.UserId,
                        LastUpdateBy = employee.UserId,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime()
                    };
                    Id = HSapiFacade.InsertEmployeeTimeClock(tc);
                    result = Id > 0;
                    #endregion
                    message += "Clock In added successfully.";
                    return Request.CreateResponse(HttpStatusCode.OK, new BaseAPIResponse(true, message, new { Id = Id, UserName = Username }));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Token Expired.", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, ex.Message, null));
            }
        }

        [Authorize]
        [Route("ClockOut")]
        public HttpResponseMessage ClockOut()
        {
            APIInitialize();

            #region Input Params
            int Id = 0;
            string ClockOutLat = "";
            string ClockOutLng = "";
            string ClockOutNote = "";

            #endregion

            var re = Request;
            var headers = re.Headers;
            bool result = false;

            #region Retrive Data From Headers

            if (headers.Contains("ClockOutLat"))
            {
                ClockOutLat = headers.GetValues("ClockOutLat").First();
            }
            if (headers.Contains("ClockOutLng"))
            {
                ClockOutLng = headers.GetValues("ClockOutLng").First();
            }
            if (headers.Contains("ClockOutNote"))
            {
                ClockOutNote = headers.GetValues("ClockOutNote").First();
            }

            #endregion 
            string message = "";
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string Username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));

                    Employee employee = HSapiFacade.GetEmployeeByUsername(Username);
                    if (employee == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                    }

                    #region UpdateTimeClockOut
                    EmployeeTimeClock tc = HSapiFacade.GetEmployeeTimeClockByUserId(employee.UserId);
                    if (tc == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                    }
                    tc.ClockOutLat = ClockOutLat;
                    tc.ClockOutLng = ClockOutLng;
                    tc.ClockOutTime = DateTime.Now.UTCCurrentTime();
                    tc.ClockOutNote = ClockOutNote;
                    tc.ClockOutCreatedBy = employee.UserId;
                    tc.LastUpdateBy = employee.UserId;
                    tc.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    tc.ClockedInSeconds = (int)DateTime.Now.UTCCurrentTime().Subtract(tc.ClockInTime).TotalSeconds;
                    result = HSapiFacade.UpdateEmployeeTimeClock(tc);
                    #endregion
                    message += "Clock Out added successfully.";
                    Id = tc.Id;
                    return Request.CreateResponse(HttpStatusCode.OK, new BaseAPIResponse(true, message, new { Id = Id, UserName = Username }));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Token Expired.", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, ex.Message, null));
            }
        }

        [Authorize]
        [Route("GetTimeClockHistory")]
        public HttpResponseMessage GetTimeClockHistory()
        {
            APIInitialize();
            int pageno = 1;
            int pagesize = 50;
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();

            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("pageno"))
            {
                Int32.TryParse(headers.GetValues("pageno").First(), out pageno);
            }
            if (headers.Contains("pagesize"))
            {
                Int32.TryParse(headers.GetValues("pagesize").First(), out pagesize);
            }
            if (headers.Contains("StartDate"))
            {
                string StartDateSt = headers.GetValues("StartDate").First();
                DateTime.TryParse(StartDateSt, out StartDate);
            }
            if (headers.Contains("EndDate"))
            {
                string EndDateSt = headers.GetValues("EndDate").First();
                DateTime.TryParse(EndDateSt, out EndDate);
            }
            //if (headers.Contains("StartDate"))
            //{
            //    StartDate = headers.GetValues("StartDate").First(); //.ToString("yyyy-MM-dd 00:00:00.000");
            //}
            //if (headers.Contains("EndDate"))
            //{
            //    EndDate = headers.GetValues("EndDate").First(); //.ToString("yyyy-MM-dd 23:59:59.999");
            //}
            //DateTime 
            List<TimeClockHistory> model = new List<TimeClockHistory>();
            var identity = (ClaimsIdentity)User.Identity;
            if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
            {
                var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                var empobj = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                var TimeClockList = HSapiFacade.GetLastClocksByUserIdAndTimePeriod(empobj != null ? empobj.UserId : new Guid(), pageno, pagesize, StartDate, EndDate);
                int total = HSapiFacade.GetTimeClockHistoryCount(empobj != null ? empobj.UserId : new Guid(), StartDate, EndDate);
                model.AddRange(TimeClockList.Select(x => new TimeClockHistory()
                {
                    ClockInOutDate = x.ClockInTime.ToString("MM/dd/yy"),
                    ClockInTime = x.ClockInTime.ToString("hh:mm:ss tt"),
                    ClockInNote = x.ClockInNote,
                    ClockInPosition = x.ClockInLat + " " + x.ClockInLng,
                    ClockOutTime = (x.ClockOutTime.HasValue && x.ClockOutTime.Value != new DateTime() ? x.ClockOutTime.Value.ToString("hh:mm:ss tt") : ""),
                    ClockOutNote = x.ClockOutNote,
                    ClockOutPosition = (!string.IsNullOrWhiteSpace(x.ClockOutLat) ? x.ClockOutLat + " " : "") + (!string.IsNullOrWhiteSpace(x.ClockOutLng) ? x.ClockOutLng : ""),
                    TimeSpent = x.ClockedInSeconds.Value.ToString()
                }));
                return Request.CreateResponse(HttpStatusCode.OK, new { Total = total, model });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
            }
        }
        [Authorize]
        [Route("clock-history")]
        public HttpResponseMessage GetClockHistory()
        {
            APIInitialize();

            Guid userId;
            var headers = Request.Headers;

            if (!headers.Contains("userId") || !Guid.TryParse(headers.GetValues("userId").FirstOrDefault(), out userId))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    success = false,
                    error = "Invalid or missing userId.",
                    result = (object)null
                });
            }

            var identity = (ClaimsIdentity)User.Identity;
            string username = identity.Claims.FirstOrDefault(c => c.Type == "username")?.Value;

            if (string.IsNullOrWhiteSpace(username))
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new
                {
                    success = false,
                    error = "Token Expired.",
                    result = (object)null
                });
            }

            var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(username);
            if (usercontext == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new
                {
                    success = false,
                    error = "Authorization Denied.",
                    result = (object)null
                });
            }

            var timeClockList = HSapiFacade.GetLastClocksByUserId(userId);
            var model = timeClockList.Select(x => new
            {
               
                ClockInTime = x.ClockInTime,
                ClockInNote = x.ClockInNote,
                ClockInPosition = $"{x.ClockInLat},{x.ClockInLng}",
                ClockOutTime = (x.ClockOutTime.HasValue && x.ClockOutTime != DateTime.MinValue) ? x.ClockOutTime : null,
                ClockOutNote = x.ClockOutNote,
                ClockOutPosition = (!string.IsNullOrWhiteSpace(x.ClockOutLat) ? x.ClockOutLat + "," : "") + x.ClockOutLng
             
            }).ToList();
   

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true,
                error = (object)null,
                result = model
            });
        }

        [Authorize]
        [HttpPost]
        [Route("clock-in")]
        public HttpResponseMessage ClockInAPI()
        {
            APIInitialize();

            #region Input Params

            string latitude = "";
            string longitude = "";
            string note = "";
            Guid userId = Guid.Empty;

            #endregion

            var re = Request;
            var headers = re.Headers;
            bool result = false;

            #region Retrive Data From Headers

            if (headers.Contains("latitude"))
            {
                latitude = headers.GetValues("latitude").First();
            }
            if (headers.Contains("longitude"))
            {
                longitude = headers.GetValues("longitude").First();
            }

            if (headers.Contains("note"))
            {
                note = HttpUtility.UrlDecode(headers.GetValues("note").First());
            }
            if (headers.Contains("userId"))
            {
                string UserId = headers.GetValues("userId").First();
                Guid.TryParse(UserId, out userId);
            }
            #endregion
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string username = identity.Claims.FirstOrDefault(c => c.Type == "username")?.Value;

                if (!string.IsNullOrWhiteSpace(username))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(username);
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new
                        {
                            success = false,
                            error = "Authorization Denied.",
                            result = (object)null
                        });

                    #region Insert Time Clock-In
                    EmployeeTimeClock tc = new EmployeeTimeClock()
                    {
                        UserId = userId,
                        ClockInLat = latitude,
                        ClockInLng = longitude,
                        ClockInTime = DateTime.UtcNow,
                        ClockInNote = note,
                        ClockInCreatedBy = userId,
                        LastUpdateBy = userId,
                        LastUpdatedDate = DateTime.UtcNow
                    };

                    int id = HSapiFacade.InsertEmployeeTimeClock(tc);
                    if (id > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new
                        {
                            success = true,
                            error = (object)null,
                            result = (object)null
                        });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new
                        {
                            success = false,
                            error = "Clock In failed.",
                            result = (object)null
                        });
                    }
                    #endregion
                }

                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new
                {
                    success = false,
                    error = "Token Expired.",
                    result = (object)null
                });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new
                {
                    success = false,
                    error = ex.Message,
                    result = (object)null
                });
            }
        }
        [Authorize]
        [Route("clock-out")]
        [HttpPost]
        public HttpResponseMessage ClockOutAPI()
        {
            APIInitialize();

            #region Input Params

            string latitude = "";
            string longitude = "";
            string note = "";
            Guid userId = Guid.Empty;

            #endregion

            var re = Request;
            var headers = re.Headers;
            bool result = false;

            #region Retrieve Data From Headers

            if (headers.Contains("latitude"))
            {
                latitude = headers.GetValues("latitude").First();
            }
            if (headers.Contains("longitude"))
            {
                longitude = headers.GetValues("longitude").First();
            }

            if (headers.Contains("note"))
            {
                note = HttpUtility.UrlDecode(headers.GetValues("note").First());
            }
            if (headers.Contains("userId"))
            {
                string UserId = headers.GetValues("userId").First();
                Guid.TryParse(UserId, out userId);
            }

            #endregion
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string Username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new
                        {
                            success = false,
                            error = "Authorization Denied.",
                            result = (object)null
                        });

                    #region UpdateTimeClockOut
                    EmployeeTimeClock tc = HSapiFacade.GetEmployeeTimeClockByUserId(userId);
                    if (tc == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new
                        {
                            success = false,
                            error = "Authorization Denied.",
                            result = (object)null
                        });
                    }
                    tc.ClockOutLat = latitude;
                    tc.ClockOutLng = longitude;
                    tc.ClockOutTime = DateTime.Now.UTCCurrentTime();
                    tc.ClockOutNote = note;
                    tc.ClockOutCreatedBy = userId;
                    tc.LastUpdateBy = userId;
                    tc.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    tc.ClockedInSeconds = (int)DateTime.Now.UTCCurrentTime().Subtract(tc.ClockInTime).TotalSeconds;
                    result = HSapiFacade.UpdateEmployeeTimeClock(tc);
                    #endregion

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = true,
                        error = (object)null,
                        result = (object)null
                    });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new
                    {
                        success = false,
                        error = "Token Expired.",
                        result = (object)null
                    });
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new
                {
                    success = false,
                    error = ex.Message,
                    result = (object)null
                });
            }
        }



        #endregion

        #region Manage User
        [Authorize]
        [Route("ManageUserEdit")]
        public HttpResponseMessage ManageUserEdit()
        {
            APIInitialize();
            Guid ID = Guid.Empty;
            string FirstName = "";
            string LastName = "";
            string Email = "";
            string Phone = "";
            string Street1 = "";
            string ZipCode = "";
            string City = "";
            string State = "";

            var re = Request;
            var headers = re.Headers;
            bool result = false;

            if (headers.Contains("ID"))
            {
                string IDSt = headers.GetValues("ID").First();
                Guid.TryParse(IDSt, out ID);
            }
            if (headers.Contains("FirstName"))
            {
                FirstName = headers.GetValues("FirstName").First();
            }
            if (headers.Contains("LastName"))
            {
                LastName = headers.GetValues("LastName").First();
            }
            if (headers.Contains("Email"))
            {
                Email = headers.GetValues("Email").First();
            }
            if (headers.Contains("Phone"))
            {
                Phone = headers.GetValues("Phone").First();
            }
            if (headers.Contains("Street1"))
            {
                Street1 = headers.GetValues("Street1").First();
            }
            if (headers.Contains("ZipCode"))
            {
                ZipCode = headers.GetValues("ZipCode").First();
            }
            if (headers.Contains("City"))
            {
                City = headers.GetValues("City").First();
            }
            if (headers.Contains("State"))
            {
                State = headers.GetValues("State").First();
            }

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");

                    Employee employee = HSapiFacade.GetEmployeeByUserId(ID);
                    if (employee == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "User Not Found."));
                    }
                    else
                    {
                        employee.FirstName = FirstName;
                        employee.LastName = LastName;
                        employee.Email = Email;
                        employee.UserName = Email;
                        employee.Phone = Phone;
                        employee.Street = Street1;
                        employee.ZipCode = ZipCode;
                        employee.City = City;
                        employee.State = State;
                        employee.LastUpdatedBy = employee.UserName;
                        employee.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        result = HSapiFacade.UpdateEmployee(employee);

                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new { result, employee });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }
        #endregion

        #region DashboardAPI
        [Authorize]
        [Route("GetDashboardList")]
        public HttpResponseMessage GetDashboardList()
        {
            APIInitialize();
            Guid ID = Guid.Empty;
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();

            var re = Request;
            var headers = re.Headers;
            bool result = false;

            if (headers.Contains("ID"))
            {
                string IDSt = headers.GetValues("ID").First();
                Guid.TryParse(IDSt, out ID);
            }
            if (headers.Contains("StartDate"))
            {
                string StartDateSt = headers.GetValues("StartDate").First();
                DateTime.TryParse(StartDateSt, out StartDate);
            }
            if (headers.Contains("EndDate"))
            {
                string EndDateSt = headers.GetValues("EndDate").First();
                DateTime.TryParse(EndDateSt, out EndDate);
            }
            try
            {
                //string ClockinTimeStr = "";
                //string ClockOutTimeStr = "";
                //string ClockInNoteStr = "";
                //string ClockOutNoteStr = "";
                //List <string> ClockinList = new List<string>();
                //List<string> ClockOutList = new List<string>();
                //List<string> ClockInNoteList = new List<string>();
                //List<string> ClockOutNoteList = new List<string>();
                //List<TimeClockStatus> Status = new List<TimeClockStatus>();

                //private void ERPAPI()
                //{
                //    if (!string.IsNullOrWhiteSpace(ConnStr))
                //    {
                //        var identity = (ClaimsIdentity)User.Identity;
                //        CompanyConneciton CC = erpApi.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                //        ERPapiFacade = new ApiFacade(CC.ConnectionString);
                //    }
                //    else
                //    {
                //        var identity = (ClaimsIdentity)User.Identity;
                //        string connectionstring = identity.Claims.Where(c => c.Type == "connectionstring").Select(c => c.Value).SingleOrDefault();
                //        ERPapiFacade = new ApiFacade(connectionstring);
                //    }
                //}


                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");

                    EmployeeDashboardAPI employee = HSapiFacade.GetEmployeeByEmployeeIdAndCompanyId(ID, usercontext.CompanyId);
                    ClockinDetail ClockDetail = HSapiFacade.GetEmployeeClockInDetailByEmployeeId(ID, StartDate, EndDate);
                    #region Comment
                    //employee.ListEmployeeTimeClock = HSapiFacade.GetEmployeeTimeClockListByUserId(ID); /*Get Employee Time Clock lIst By Employee/UserId */
                    //if (employee.ListEmployeeTimeClock != null)  /*Check Null for  employee.ListEmployeeTimeClock */
                    //{

                    //    foreach (var item in employee.ListEmployeeTimeClock) /*Lop for employee.ListEmployeeTimeClock*/
                    //    {
                    //        if (item.ClockInTime != null & item.ClockInTime != new DateTime())
                    //        {

                    //            ClockinList.Add(item.ClockInTime.ToString("MM/dd/yyyy HH:mm tt"));

                    //        }
                    //        if (item.ClockOutTime != null & item.ClockOutTime != new DateTime())
                    //        {

                    //            ClockOutList.Add(item.ClockOutTime.ToString());

                    //        }
                    //        if (item.ClockInNote != null)
                    //        {

                    //            ClockInNoteList.Add(item.ClockInNote.ToString());

                    //        }
                    //        if (item.ClockOutNote != null)
                    //        {

                    //            ClockOutNoteList.Add(item.ClockOutNote.ToString());

                    //        }

                    //    }
                    //    ClockinTimeStr = string.Join(", ", ClockinList);
                    //    if (!string.IsNullOrWhiteSpace(ClockinTimeStr))
                    //    {
                    //        Status.Add(new TimeClockStatus()
                    //        {
                    //            Label = "Clock in Time",
                    //            Value = ClockinTimeStr
                    //        });
                    //    }
                    //    ClockOutTimeStr = string.Join(", ", ClockOutList);
                    //    if (!string.IsNullOrWhiteSpace(ClockOutTimeStr))
                    //    {
                    //        Status.Add(new TimeClockStatus()
                    //        {
                    //            Label = "Clock Out Time",
                    //            Value = ClockOutTimeStr
                    //        });
                    //    }
                    //    ClockInNoteStr = string.Join(", ", ClockInNoteList);
                    //    if (!string.IsNullOrWhiteSpace(ClockInNoteStr))
                    //    {
                    //        Status.Add(new TimeClockStatus()
                    //        {
                    //            Label = "Clock In Note",
                    //            Value = ClockInNoteStr
                    //        });
                    //    }
                    //    ClockOutNoteStr = string.Join(", ", ClockOutNoteList);
                    //    if (!string.IsNullOrWhiteSpace(ClockOutNoteStr))
                    //    {
                    //        Status.Add(new TimeClockStatus()
                    //        {
                    //            Label = "Clock Out Note",
                    //            Value = ClockOutNoteStr
                    //        });
                    //    }

                    //}
                    //else
                    //{
                    //    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Data Not Found."));
                    //}                               
                    #endregion
                    if (employee == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "User Not Found."));
                    }
                    else
                    {
                        result = true;
                        return Request.CreateResponse(HttpStatusCode.OK, new { result, employee, ClockDetail });
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }
        #endregion

        #region DeviceAPI
        [Authorize]
        [Route("SaveDeviceLog")]
        public HttpResponseMessage SaveDeviceLog()
        {
            APIInitialize();

            Guid CompanyId = Guid.Empty;
            Guid UserId = Guid.Empty;
            string DeviceId = "";

            var re = Request;
            var headers = re.Headers;
            bool result = false;

            if (headers.Contains("CompanyId"))
            {
                string CompanyIdSt = headers.GetValues("CompanyId").First();
                Guid.TryParse(CompanyIdSt, out CompanyId);
            }

            if (headers.Contains("UserId"))
            {
                string UserIdSt = headers.GetValues("UserId").First();
                Guid.TryParse(UserIdSt, out UserId);
            }

            if (headers.Contains("DeviceId"))
            {
                DeviceId = headers.GetValues("DeviceId").First();
            }

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");

                    Employee emp = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (emp == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "User Not Found.");
                    Company company = HSapiFacade.GetCompanyByCompanyId(CompanyId);
                    if (company == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Company Not Found.");

                    UserCompanyDevice UserCompanyLog = HSapiFacade.UserCompanyDeviceLogByDeviceId(DeviceId);

                    if (UserCompanyLog == null)
                    {
                        UserCompanyLog = new UserCompanyDevice()
                        {
                            CompanyId = CompanyId,
                            UserId = UserId,
                            DeviceId = DeviceId,
                            IsActive = true,
                            CreatedDate = DateTime.Now.UTCCurrentTime()
                        };
                        result = HSapiFacade.InsertUserCompanyDevice(UserCompanyLog);
                    }
                    else
                    {
                        UserCompanyLog.CompanyId = CompanyId;
                        UserCompanyLog.UserId = UserId;
                        UserCompanyLog.DeviceId = DeviceId;
                        UserCompanyLog.IsActive = true;
                        result = HSapiFacade.UpdateUserCompanyDevice(UserCompanyLog);
                    }


                    return Request.CreateResponse(HttpStatusCode.OK, new { result });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }

        }

        [Authorize]
        [Route("LogOutDevice")]
        public HttpResponseMessage LogOutDevice()
        {
            APIInitialize();

            Guid CompanyId = Guid.Empty;
            Guid UserId = Guid.Empty;
            string DeviceId = "";

            var re = Request;
            var headers = re.Headers;
            bool result = false;

            if (headers.Contains("CompanyId"))
            {
                string CompanyIdSt = headers.GetValues("CompanyId").First();
                Guid.TryParse(CompanyIdSt, out CompanyId);
            }

            if (headers.Contains("UserId"))
            {
                string UserIdSt = headers.GetValues("UserId").First();
                Guid.TryParse(UserIdSt, out UserId);
            }

            if (headers.Contains("DeviceId"))
            {
                DeviceId = headers.GetValues("DeviceId").First();
            }

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");

                    Employee emp = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (emp == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "User Not Found.");
                    Company company = HSapiFacade.GetCompanyByCompanyId(CompanyId);
                    if (company == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Company Not Found.");

                    UserCompanyDevice UserCompanyLog = HSapiFacade.UserCompanyDeviceLog(CompanyId, UserId, DeviceId);

                    if (UserCompanyLog != null)
                    {
                        UserCompanyLog.IsActive = false;
                        result = HSapiFacade.UpdateUserCompanyDevice(UserCompanyLog);
                    }
                    else
                    {
                        result = false;
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new { result });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }



        #endregion

        private static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }

        #region Ieatery API
        [Route("GetAllOrders")]
        public HttpResponseMessage GetAllOrders()
        {
            int pageno = 0;
            int pagesize = 0;
            string searchtext = "";
            string order = "";
            string companyid = "";
            string username = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("pageno"))
            {
                pageno = Convert.ToInt32(headers.GetValues("pageno").First());
            }
            if (headers.Contains("pagesize"))
            {
                pagesize = Convert.ToInt32(headers.GetValues("pagesize").First());
            }
            if (headers.Contains("searchtext"))
            {
                searchtext = headers.GetValues("searchtext").First();
            }
            if (headers.Contains("order"))
            {
                order = headers.GetValues("order").First();
            }
            if (headers.Contains("companyid"))
            {
                companyid = headers.GetValues("companyid").First();
            }
            if (headers.Contains("username"))
            {
                username = headers.GetValues("username").First();
            }
            try
            {
                var usercontext = new CompanyConneciton();
                int totalorder = 0;
                int totalacceptedorder = 0;
                int totalpendingorder = 0;
                int totalrejectedorder = 0;
                int totalcompletedorder = 0;
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    ComId = companyid;
                    UserNAME = username;
                    APIInitialize();
                    if (!string.IsNullOrWhiteSpace(ComId))
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(ComId));
                    }
                    else
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    }

                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    List<RestaurantOrderCustomModel> model = new List<RestaurantOrderCustomModel>();
                    model = HSapiFacade.GetAllResturantOrderList(usercontext.CompanyId, pageno, pagesize, searchtext, order);
                    var allorders = HSapiFacade.GetAllOrdersByCompanyId(usercontext.CompanyId);
                    if (allorders != null && allorders.Count > 0)
                    {
                        totalorder = allorders.Count;
                        totalacceptedorder = allorders.Where(x => x.Status.ToLower() == "accepted" || x.Status.ToLower() == "readypick" || x.Status.ToLower() == "readydeliver").ToList().Count;
                        totalpendingorder = allorders.Where(x => x.Status.ToLower() == "pending").ToList().Count;
                        totalrejectedorder = allorders.Where(x => x.Status.ToLower() == "rejected").ToList().Count;
                        totalcompletedorder = allorders.Where(x => x.Status.ToLower() == "pickedup" || x.Status.ToLower() == "delivered").ToList().Count;
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { restaurantorder = model, totalorder = totalorder, totalacceptedorder = totalacceptedorder, totalpendingorder = totalpendingorder, totalrejectedorder = totalrejectedorder, totalcompletedorder = totalcompletedorder });
                }
                else if (!string.IsNullOrWhiteSpace(companyid) && !string.IsNullOrWhiteSpace(username))
                {
                    ComId = companyid;
                    UserNAME = username;
                    APIInitialize();
                    if (!string.IsNullOrWhiteSpace(ComId))
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(UserNAME, new Guid(ComId));
                    }
                    else
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(UserNAME);
                    }

                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    List<RestaurantOrderCustomModel> model = new List<RestaurantOrderCustomModel>();
                    model = HSapiFacade.GetAllResturantOrderList(usercontext.CompanyId, pageno, pagesize, searchtext, order);
                    var allorders = HSapiFacade.GetAllOrdersByCompanyId(usercontext.CompanyId);
                    if (allorders != null && allorders.Count > 0)
                    {
                        totalorder = allorders.Count;
                        totalacceptedorder = allorders.Where(x => x.Status.ToLower() == "accepted" || x.Status.ToLower() == "readypick" || x.Status.ToLower() == "readydeliver").ToList().Count;
                        totalpendingorder = allorders.Where(x => x.Status.ToLower() == "pending").ToList().Count;
                        totalrejectedorder = allorders.Where(x => x.Status.ToLower() == "rejected").ToList().Count;
                        totalcompletedorder = allorders.Where(x => x.Status.ToLower() == "pickedup" || x.Status.ToLower() == "delivered").ToList().Count;
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { restaurantorder = model, totalorder = totalorder, totalacceptedorder = totalacceptedorder, totalpendingorder = totalpendingorder, totalrejectedorder = totalrejectedorder, totalcompletedorder = totalcompletedorder });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation, "Authorization Denied.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Route("GetOrderDetail")]
        public HttpResponseMessage GetOrderDetail()
        {
            Guid orderid = new Guid();
            string companyid = "";
            string username = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("orderid"))
            {
                Guid.TryParse(headers.GetValues("orderid").First(), out orderid);
            }
            if (headers.Contains("companyid"))
            {
                companyid = headers.GetValues("companyid").First();
            }
            if (headers.Contains("username"))
            {
                username = headers.GetValues("username").First();
            }
            try
            {
                var usercontext = new CompanyConneciton();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    ComId = companyid;
                    UserNAME = username;
                    APIInitialize();
                    if (!string.IsNullOrWhiteSpace(ComId))
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(ComId));
                    }
                    else
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    }
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    List<RestaurantOrderDetailCustomModel> model = new List<RestaurantOrderDetailCustomModel>();
                    model = HSapiFacade.GetAllResturantOrderDetailByOrderId(orderid);
                    //model.ResturantOrder = HSapiFacade.GetRestaurantOrderByOrderId(usercontext.CompanyId, orderid);
                    //model.Customer = HSapiFacade.GetCustomerByCustomerId(model.ResturantOrder.CustomerId);
                    //model.WebsiteLocation = HSapiFacade.GetWebsiteLocationByCompanyId(usercontext.CompanyId);
                    var objorder = HSapiFacade.GetResturantOrderByCompanyIdAndOrderId(usercontext.CompanyId, orderid);
                    if (objorder != null)
                    {
                        objorder.IsViewed = true;
                        HSapiFacade.UpdateResturantOrder(objorder);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { orderdetail = model });
                }
                else if (!string.IsNullOrWhiteSpace(companyid) && !string.IsNullOrWhiteSpace(username))
                {
                    ComId = companyid;
                    UserNAME = username;
                    APIInitialize();
                    if (!string.IsNullOrWhiteSpace(ComId))
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(UserNAME, new Guid(ComId));
                    }
                    else
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(UserNAME);
                    }
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    List<RestaurantOrderDetailCustomModel> model = new List<RestaurantOrderDetailCustomModel>();
                    model = HSapiFacade.GetAllResturantOrderDetailByOrderId(orderid);
                    //model.ResturantOrder = HSapiFacade.GetRestaurantOrderByOrderId(usercontext.CompanyId, orderid);
                    //model.Customer = HSapiFacade.GetCustomerByCustomerId(model.ResturantOrder.CustomerId);
                    //model.WebsiteLocation = HSapiFacade.GetWebsiteLocationByCompanyId(usercontext.CompanyId);
                    var objorder = HSapiFacade.GetResturantOrderByCompanyIdAndOrderId(usercontext.CompanyId, orderid);
                    if (objorder != null)
                    {
                        objorder.IsViewed = true;
                        HSapiFacade.UpdateResturantOrder(objorder);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { orderdetail = model });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation, "Authorization Denied.");
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Route("ChangeOrderStatus")]
        public HttpResponseMessage ChangeOrderStatus([FromBody] List<StockItemCustomModel> HasstockItem)
        {
            var subtotal = 0.0;
            var total = 0.0;
            var taxtotal = 0.0;
            Guid orderid = new Guid();
            string status = "";
            string accepttime = "";
            string rejectnote = "";
            string companyid = "";
            string username = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("orderid"))
            {
                Guid.TryParse(headers.GetValues("orderid").First(), out orderid);
            }
            if (headers.Contains("status"))
            {
                status = headers.GetValues("status").First();
            }
            if (headers.Contains("accepttime"))
            {
                accepttime = headers.GetValues("accepttime").First();
            }
            if (headers.Contains("rejectnote"))
            {
                rejectnote = headers.GetValues("rejectnote").First();
            }
            if (headers.Contains("companyid"))
            {
                companyid = headers.GetValues("companyid").First();
            }
            if (headers.Contains("username"))
            {
                username = headers.GetValues("username").First();
            }
            bool result = false;
            try
            {
                var usercontext = new CompanyConneciton();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    ComId = companyid;
                    UserNAME = username;
                    APIInitialize();
                    if (!string.IsNullOrWhiteSpace(ComId))
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(ComId));
                    }
                    else
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    }
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ResturantOrder model = new ResturantOrder();
                    WebsiteLocation WebsiteLocation = HSapiFacade.GetWebsiteLocationByCompanyId(usercontext.CompanyId);
                    var userobj = HSapiFacade.GetUserByUsername(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    model = HSapiFacade.GetResturantOrderByCompanyIdAndOrderId(usercontext.CompanyId, orderid);
                    if (model != null)
                    {
                        var objcoupon = HSapiFacade.GetCouponsByCompanyIdandCode(model.CompanyId, model.DiscountCode);
                        if (model.Status == "Accepted" && status == "Accepted")
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Already accepted");
                        }

                        if (model.Status == "Rejected" && status == "Rejected")
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Already rejected");
                        }

                        if (!string.IsNullOrWhiteSpace(status))
                        {
                            model.Status = status;

                        }
                        if (!string.IsNullOrWhiteSpace(rejectnote))
                        {
                            model.RejectedReason = rejectnote;
                            model.RejectedDate = DateTime.Now.UTCCurrentTime();
                        }
                        if (!string.IsNullOrWhiteSpace(accepttime))
                        {
                            model.AcceptTime = accepttime;
                            model.AcceptDate = Convert.ToDateTime((model.OrderDate.HasValue ? model.OrderDate.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy")) + " " + accepttime);
                        }
                        model.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        model.LastUpdatedBy = "App";
                        result = HSapiFacade.UpdateResturantOrder(model);
                        if (result)
                        {
                            if (HasstockItem != null && HasstockItem.Count > 1)
                            {
                                foreach (var item in HasstockItem)
                                {
                                    var objorderdetail = HSapiFacade.GetOrderDetailById(item.ItemId);
                                    if (objorderdetail != null)
                                    {
                                        objorderdetail.IsStock = item.IsAvailable;
                                        HSapiFacade.UpdateOrderDetail(objorderdetail);
                                        if (item.IsAvailable == true)
                                        {
                                            subtotal = subtotal + objorderdetail.ItemPrice;
                                        }
                                    }
                                }
                                if (objcoupon != null && subtotal >= Convert.ToDouble(objcoupon.MinimumOrder))
                                {
                                    if (objcoupon.DiscountType.ToLower() == "dollar")
                                    {
                                        subtotal = subtotal - Convert.ToDouble(objcoupon.Discount);
                                    }
                                    else
                                    {
                                        var disval = (subtotal * Convert.ToDouble(objcoupon.Discount)) / 100;
                                        subtotal = subtotal - disval;
                                    }
                                }
                                if (subtotal > 0)
                                {
                                    if ((model.TaxAmount.HasValue ? model.TaxAmount.Value : 0) > 0)
                                    {
                                        var taxrate = ((model.TaxAmount / ((model.Amount - (model.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0)) - model.TaxAmount)) * 100);
                                        taxtotal = Convert.ToDouble((subtotal * taxrate) / 100);
                                        total = subtotal + taxtotal + (model.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0);
                                    }
                                    else
                                    {
                                        total = subtotal + (model.TaxAmount.HasValue ? model.TaxAmount.Value : 0) + (model.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0);
                                    }
                                    model.Amount = total;
                                    model.TaxAmount = taxtotal;
                                    HSapiFacade.UpdateResturantOrder(model);
                                }
                            }
                            else if (HasstockItem != null && HasstockItem.Count == 1)
                            {
                                foreach (var item in HasstockItem)
                                {
                                    var objorderdetail = HSapiFacade.GetOrderDetailById(Convert.ToInt32(item.ItemId));
                                    if (objorderdetail != null)
                                    {
                                        objorderdetail.IsStock = true;
                                        HSapiFacade.UpdateOrderDetail(objorderdetail);
                                        if (item.IsAvailable == true)
                                        {
                                            subtotal = subtotal + objorderdetail.ItemPrice;
                                        }
                                    }
                                }
                                if (objcoupon != null && subtotal >= Convert.ToDouble(objcoupon.MinimumOrder))
                                {
                                    if (objcoupon.DiscountType.ToLower() == "dollar")
                                    {
                                        subtotal = subtotal - Convert.ToDouble(objcoupon.Discount);
                                    }
                                    else
                                    {
                                        var disval = (subtotal * Convert.ToDouble(objcoupon.Discount)) / 100;
                                        subtotal = subtotal - disval;
                                    }
                                }
                                if (subtotal > 0)
                                {
                                    if ((model.TaxAmount.HasValue ? model.TaxAmount.Value : 0) > 0)
                                    {
                                        var taxrate = ((model.TaxAmount / ((model.Amount - (model.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0)) - model.TaxAmount)) * 100);
                                        taxtotal = Convert.ToDouble((subtotal * taxrate) / 100);
                                        total = subtotal + taxtotal + (model.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0);
                                    }
                                    else
                                    {
                                        total = subtotal + (model.TaxAmount.HasValue ? model.TaxAmount.Value : 0) + (model.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0);
                                    }
                                    model.Amount = total;
                                    model.TaxAmount = taxtotal;
                                    HSapiFacade.UpdateResturantOrder(model);
                                }
                            }
                            else
                            {
                                model.Status = "Rejected";
                                model.RejectedDate = DateTime.Now.UTCCurrentTime();
                                model.RejectedReason = "Restaurant not accepted your order";
                                HSapiFacade.UpdateResturantOrder(model);
                            }
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = result, restaurantorder = model });
                }
                else if (!string.IsNullOrWhiteSpace(companyid) && !string.IsNullOrWhiteSpace(username))
                {
                    ComId = companyid;
                    UserNAME = username;
                    APIInitialize();
                    if (!string.IsNullOrWhiteSpace(ComId))
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(UserNAME, new Guid(ComId));
                    }
                    else
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(UserNAME);
                    }
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    ResturantOrder model = new ResturantOrder();
                    WebsiteLocation WebsiteLocation = HSapiFacade.GetWebsiteLocationByCompanyId(usercontext.CompanyId);
                    model = HSapiFacade.GetResturantOrderByCompanyIdAndOrderId(usercontext.CompanyId, orderid);
                    if (model != null)
                    {
                        var objcoupon = HSapiFacade.GetCouponsByCompanyIdandCode(model.CompanyId, model.DiscountCode);
                        if (model.Status == "Accepted" && status == "Accepted")
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Already accepted");
                        }

                        if (model.Status == "Rejected" && status == "Rejected")
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Already rejected");
                        }

                        if (!string.IsNullOrWhiteSpace(status))
                        {
                            model.Status = status;

                        }
                        if (!string.IsNullOrWhiteSpace(rejectnote))
                        {
                            model.RejectedReason = rejectnote;
                            model.RejectedDate = DateTime.Now.UTCCurrentTime();
                        }
                        if (!string.IsNullOrWhiteSpace(accepttime))
                        {
                            model.AcceptTime = accepttime;
                            model.AcceptDate = Convert.ToDateTime((model.OrderDate.HasValue ? model.OrderDate.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy")) + " " + accepttime);
                        }
                        result = HSapiFacade.UpdateResturantOrder(model);
                        if (result)
                        {
                            if (HasstockItem != null && HasstockItem.Count > 1)
                            {
                                foreach (var item in HasstockItem)
                                {
                                    var objorderdetail = HSapiFacade.GetOrderDetailById(item.ItemId);
                                    if (objorderdetail != null)
                                    {
                                        objorderdetail.IsStock = item.IsAvailable;
                                        HSapiFacade.UpdateOrderDetail(objorderdetail);
                                        if (item.IsAvailable == true)
                                        {
                                            subtotal = subtotal + objorderdetail.ItemPrice;
                                        }
                                    }
                                }
                                if (objcoupon != null && subtotal >= Convert.ToDouble(objcoupon.MinimumOrder))
                                {
                                    if (objcoupon.DiscountType.ToLower() == "dollar")
                                    {
                                        subtotal = subtotal - Convert.ToDouble(objcoupon.Discount);
                                    }
                                    else
                                    {
                                        var disval = (subtotal * Convert.ToDouble(objcoupon.Discount)) / 100;
                                        subtotal = subtotal - disval;
                                    }
                                }
                                if (subtotal > 0)
                                {
                                    if ((model.TaxAmount.HasValue ? model.TaxAmount.Value : 0) > 0)
                                    {
                                        var taxrate = ((model.TaxAmount / ((model.Amount - (model.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0)) - model.TaxAmount)) * 100);
                                        taxtotal = Convert.ToDouble((subtotal * taxrate) / 100);
                                        total = subtotal + taxtotal + (model.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0);
                                    }
                                    else
                                    {
                                        total = subtotal + (model.TaxAmount.HasValue ? model.TaxAmount.Value : 0) + (model.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0);
                                    }
                                    model.Amount = total;
                                    model.TaxAmount = taxtotal;
                                    HSapiFacade.UpdateResturantOrder(model);
                                }
                            }
                            else if (HasstockItem != null && HasstockItem.Count == 1)
                            {
                                foreach (var item in HasstockItem)
                                {
                                    var objorderdetail = HSapiFacade.GetOrderDetailById(Convert.ToInt32(item.ItemId));
                                    if (objorderdetail != null)
                                    {
                                        objorderdetail.IsStock = true;
                                        HSapiFacade.UpdateOrderDetail(objorderdetail);
                                        if (item.IsAvailable == true)
                                        {
                                            subtotal = subtotal + objorderdetail.ItemPrice;
                                        }
                                    }
                                }
                                if (objcoupon != null && subtotal >= Convert.ToDouble(objcoupon.MinimumOrder))
                                {
                                    if (objcoupon.DiscountType.ToLower() == "dollar")
                                    {
                                        subtotal = subtotal - Convert.ToDouble(objcoupon.Discount);
                                    }
                                    else
                                    {
                                        var disval = (subtotal * Convert.ToDouble(objcoupon.Discount)) / 100;
                                        subtotal = subtotal - disval;
                                    }
                                }
                                if (subtotal > 0)
                                {
                                    if ((model.TaxAmount.HasValue ? model.TaxAmount.Value : 0) > 0)
                                    {
                                        var taxrate = ((model.TaxAmount / ((model.Amount - (model.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0)) - model.TaxAmount)) * 100);
                                        taxtotal = Convert.ToDouble((subtotal * taxrate) / 100);
                                        total = subtotal + taxtotal + (model.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0);
                                    }
                                    else
                                    {
                                        total = subtotal + (model.TaxAmount.HasValue ? model.TaxAmount.Value : 0) + (model.OrderType.ToLower() == "delivery" && WebsiteLocation.DeliveryFee.HasValue ? WebsiteLocation.DeliveryFee.Value : 0);
                                    }
                                    model.Amount = total;
                                    model.TaxAmount = taxtotal;
                                    HSapiFacade.UpdateResturantOrder(model);
                                }
                            }
                            else
                            {
                                model.Status = "Rejected";
                                model.RejectedDate = DateTime.Now.UTCCurrentTime();
                                model.RejectedReason = "Restaurant not accepted your order";
                                HSapiFacade.UpdateResturantOrder(model);
                            }
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { result = result, restaurantorder = model });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation, "Authorization Denied.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Route("GetReportData")]
        public HttpResponseMessage GetReportData()
        {
            int itemid = new int();
            string companyid = "";
            string username = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("companyid"))
            {
                companyid = headers.GetValues("companyid").First();
            }
            if (headers.Contains("username"))
            {
                username = headers.GetValues("username").First();
            }
            try
            {
                var usercontext = new CompanyConneciton();
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    ComId = companyid;
                    UserNAME = username;
                    APIInitialize();
                    if (!string.IsNullOrWhiteSpace(ComId))
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(ComId));
                    }
                    else
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    }
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    List<SpaiderReport> spaiderReportData = new List<SpaiderReport>();
                    List<PieChart> PieReportData = new List<PieChart>();
                    spaiderReportData = HSapiFacade.GetAllReportData(usercontext.CompanyId);
                    DateTime now = DateTime.Now;
                    var startDate = new DateTime(now.Year, now.Month, 1);
                    var endDate = now.AddDays(1).Date;
                    List<LineChartReport> LineChartReportData = new List<LineChartReport>();
                    LineChartReportData = HSapiFacade.GetLineChartReport(usercontext.CompanyId, startDate, endDate);
                    PieReportData = HSapiFacade.GetAllPieReportData(usercontext.CompanyId);
                    PieReport pieReport = new PieReport();
                    int totalSale = 0;
                    if (spaiderReportData != null && spaiderReportData.Count > 0)
                    {
                        foreach (var item in spaiderReportData)
                        {
                            totalSale += item.AcceptCount;
                        }

                    }
                    pieReport.TotalSale = totalSale;
                    pieReport.PieChart = PieReportData;
                    return Request.CreateResponse(HttpStatusCode.OK, new { spaiderReportData = spaiderReportData, LineChartReportData = LineChartReportData, pieReport = pieReport });
                }
                else if (!string.IsNullOrWhiteSpace(companyid) && !string.IsNullOrWhiteSpace(username))
                {
                    ComId = companyid;
                    UserNAME = username;
                    APIInitialize();
                    if (!string.IsNullOrWhiteSpace(ComId))
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(UserNAME, new Guid(ComId));
                    }
                    else
                    {
                        usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(UserNAME);
                    }
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    List<SpaiderReport> spaiderReportData = new List<SpaiderReport>();
                    List<PieChart> PieReportData = new List<PieChart>();
                    spaiderReportData = HSapiFacade.GetAllReportData(usercontext.CompanyId);
                    DateTime now = DateTime.Now;
                    var startDate = new DateTime(now.Year, now.Month, 1);
                    var endDate = now.AddDays(1).Date;
                    List<LineChartReport> LineChartReportData = new List<LineChartReport>();
                    LineChartReportData = HSapiFacade.GetLineChartReport(usercontext.CompanyId, startDate, endDate);
                    PieReportData = HSapiFacade.GetAllPieReportData(usercontext.CompanyId);
                    PieReport pieReport = new PieReport();
                    int totalSale = 0;
                    if (spaiderReportData != null && spaiderReportData.Count > 0)
                    {
                        foreach (var item in spaiderReportData)
                        {
                            totalSale += item.AcceptCount;
                        }

                    }
                    pieReport.TotalSale = totalSale;
                    pieReport.PieChart = PieReportData;
                    return Request.CreateResponse(HttpStatusCode.OK, new { spaiderReportData = spaiderReportData, LineChartReportData = LineChartReportData, pieReport = pieReport });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation, "Authorization Denied.");
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Route("plivo/forward")]
        public IHttpActionResult forward()
        {
            string from_number = "12035023245";
            string to_number = "18172428806";
            string output = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("From"))
            {
                from_number = headers.GetValues("From").First();
            }
            if (headers.Contains("To"))
            {
                to_number = headers.GetValues("To").First();
            }
            try
            {
                string constr = ConfigurationManager.AppSettings["CallForwardingConStr"];
                HSApiFacade PHSApiFacade = new HSApiFacade(constr);
                if (from_number == null)
                {
                    from_number = "";
                }
                else
                {
                    from_number = "+" + from_number;
                }
                if (to_number == null)
                {
                    to_number = "";
                }
                else
                {
                    to_number = "+" + to_number;
                }
                Plivo.XML.Response resp = new Plivo.XML.Response();
                Plivo.XML.Dial dial = new Plivo.XML.Dial(new
                    Dictionary<string, string>(){
                {"callerId",from_number}
            });
                if (!string.IsNullOrWhiteSpace(to_number))
                {
                    var objtrack = PHSApiFacade.GetTrackingDetailsByTrackingPhone(to_number.Replace("+1", ""));
                    if (objtrack != null)
                    {
                        dial.AddNumber(objtrack.ForwardingNumber.Replace("-", "").Length == 10 ? "+1" + objtrack.ForwardingNumber.Replace("-", "") : objtrack.ForwardingNumber.Replace("-", ""),
                        new Dictionary<string, string>() { });
                        resp.Add(dial);
                    }
                    else
                    {
                        dial.AddNumber(to_number,
                        new Dictionary<string, string>() { });
                        resp.Add(dial);
                    }
                }

                output = resp.ToString();
                //System.IO.File.WriteAllText(System.Web.Hosting.HostingEnvironment.MapPath(@"~/PlivoCallerLog.txt"), "Success Log- " + from_number + " " + to_number);
                return Content(HttpStatusCode.OK, output, Configuration.Formatters.XmlFormatter);
            }
            catch (Exception e)
            {
                //System.IO.File.WriteAllText(System.Web.Hosting.HostingEnvironment.MapPath(@"~/PlivoCallerLog.txt"), "Error Log- " + e.Message);
                return Content(HttpStatusCode.OK, output, Configuration.Formatters.XmlFormatter);
            }
        }
        #endregion

        #region Rug API
        [Authorize]
        [Route("GetPackageList")]
        public HttpResponseMessage GetRugPackageList()
        {
            APIInitialize();

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string Username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));

                    var package = HSapiFacade.GetPackageAndInclude();
                    return Request.CreateResponse(HttpStatusCode.OK, new BaseAPIResponse(false, "Successful.", package));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Token Expired.", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, ex.Message, null));
            }
        }

        [Authorize]
        [Route("AddLeadBooking")]
        public HttpResponseMessage AddLeadBooking([FromBody] CreateBooking Model)
        {
            APIInitialize();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string Username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    Customer customer = HSapiFacade.GetCustomerByCustomerId(Model.Booking.CustomerId);
                    if (customer == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Customer Not Found.");
                    Employee emp = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (emp == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "User Not Found.");
                    Company company = HSapiFacade.GetCompanyByCompanyId(usercontext.CompanyId);
                    if (company == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Company Not Found.");
                    Booking tempBooking = new Booking();
                    if (!string.IsNullOrEmpty(Model.Booking.BookingId))
                    {
                        tempBooking = HSapiFacade.GetByBookingId(Model.Booking.BookingId);

                        if (tempBooking == null || tempBooking.CompanyId != usercontext.CompanyId)
                        {
                            return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, "Booking is null", null));

                        }
                    }
                    else
                    {
                        tempBooking = new Booking()
                        {
                            CustomerId = customer.CustomerId,
                            CustomerName = customer.FirstName + " " + customer.LastName,
                            CompanyId = usercontext.CompanyId,
                            Amount = 0,
                            Status = "Init",
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            CreatedBy = emp.UserId,
                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            LastUpdatedBy = emp.UserId,
                            EmailAddress = customer.EmailAddress
                        };
                        tempBooking.PickUpLocation = Model.Booking.BillingAddress;
                        tempBooking.DropOffLocation = Model.Booking.BillingAddress;
                        tempBooking.Id = HSapiFacade.InsertBooking(tempBooking);
                        tempBooking.BookingId = tempBooking.Id.GenerateBookingNo();
                        HSapiFacade.UpdateBooking(tempBooking);
                    }



                    Model.Booking.Id = tempBooking.Id;
                    Model.Booking.CustomerName = customer.FirstName + " " + customer.LastName;
                    Model.CustomerName = Model.Booking.CustomerName;
                    Model.Booking.CreatedBy = tempBooking.CreatedBy;
                    Model.Booking.CreatedDate = tempBooking.CreatedDate;
                    Model.Booking.CompanyId = tempBooking.CompanyId;
                    Model.Booking.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    Model.Booking.LastUpdatedBy = emp.UserId;
                    Model.Booking.BookingId = tempBooking.BookingId;
                    Model.Booking.Message = Model.Booking.Message;
                    if (string.IsNullOrWhiteSpace(Model.Booking.Status))
                        Model.Booking.Status = LabelHelper.BookingStatus.Created;
                    HSapiFacade.UpdateBooking(Model.Booking);

                    #region booking Extra Item 
                    HSapiFacade.DeleteAllBookingExtraItemByBookingId(Model.Booking.Id);
                    if (Model.BookingExtraItem != null && Model.BookingExtraItem.Count > 0)
                    {
                        foreach (var item in Model.BookingExtraItem)
                        {

                            item.CreatedBy = usercontext.UserName;
                            item.CreatedDate = DateTime.Now.UTCCurrentTime();
                            item.BookingId = Model.Booking.Id;
                            HSapiFacade.InsertBookingExtraItem(item);
                        }
                    }

                    #endregion


                    #region CustomerSnapshot
                    var objBookingSnapshot = HSapiFacade.GetCustomerSnapshotDetail(Model.Booking.BookingId.ToString());
                    if (objBookingSnapshot.Count == 0)
                    {
                        var empobj = HSapiFacade.GetEmployeeByEmployeeId(emp.UserId);
                        if (empobj != null)
                        {
                            var updatedate = Model.Booking.LastUpdatedDate.UTCToClientTime();
                            CustomerSnapshot BookingLogObj = new CustomerSnapshot()
                            {
                                CustomerId = customer.CustomerId,
                                CompanyId = usercontext.CompanyId,
                                Description = "Booking "
                                + string.Format
                                ("<a onclick=OpenTopToBottomModal('/Booking/AddLeadBooking?id={0}&CustomerId={1}') style='cursor: pointer;'>",
                                Model.Booking.Id, customer.Id, customer.CustomerId)
                                + "<b>" + Model.Booking.BookingId + "</b>" + "</a>",

                                Logdate = DateTime.Now.UTCCurrentTime(),
                                Updatedby = empobj.FirstName + " " + empobj.LastName,
                                Type = "BookingCreateHistory"
                            };
                            HSapiFacade.InsertSnapshot(BookingLogObj);
                        }
                    }
                    #endregion

                    #region Booking details
                    Model.SubTotal = 0;
                    HSapiFacade.DeleteAllBookingDetailsByBookingId(Model.Booking.BookingId);
                    if (Model.BookingDetailsList != null)
                    {
                        foreach (var item in Model.BookingDetailsList)
                        {
                            item.BookingId = Model.Booking.BookingId;
                            item.AddedBy = emp.UserId;
                            item.AddedDate = DateTime.Now.UTCCurrentTime();
                            item.CompanyId = usercontext.CompanyId;
                            HSapiFacade.InsertBookingDetails(item);
                        }
                        foreach (var item in Model.BookingDetailsList)
                        {
                            item.AddedBy = emp.UserId;
                            item.AddedDate = DateTime.Now.UTCCurrentTime();
                            item.CompanyId = usercontext.CompanyId;
                            Model.SubTotal = Model.SubTotal + (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
                        }
                    }

                    #endregion
                }

                return Request.CreateResponse(HttpStatusCode.OK, new BaseAPIResponse(true, "Booking Successfully Saved.", new { Id = Model.Booking.Id, BookingId = Model.Booking.BookingId }));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, ex.Message, null));
            }
        }

        #endregion

        #region RugTracker Online Booking API


        [Authorize]
        [Route("GetAllDropdownList")]
        public HttpResponseMessage GetAllRugDropdownList()
        {
            APIInitialize();

            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string Username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));

                    var KeyList = HSapiFacade.GetLookupByKey("RugShape", usercontext.CompanyId);
                    var EquipmentList = HSapiFacade.GetEquipmentListByName("");
                    var package = HSapiFacade.GetPackageAndInclude();
                    return Request.CreateResponse(HttpStatusCode.OK, new BaseAPIResponse(true, "Booking Successfully Saved.", new { ShapeList = KeyList, packageList = package, ServiceList = EquipmentList }));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Token Expired.", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, ex.Message, null));
            }
        }


        [Authorize]
        [Route("RugTrackerBookingCreateAndEmail")]
        public HttpResponseMessage RugTrackerBookingEmail([FromBody] CreateBooking Model)
        {
            APIInitialize();
            try
            {
                int Id = 0;
                string CustomerId = "";
                Employee emp = new Employee();
                double TotalPrice = Model.SubTotal;
                string FullName = Model.CustomerName;
                string[] Names = FullName.Split('+');
                string FirstName = Names[0];
                string LastName = Names[1];
                var identity = (ClaimsIdentity)User.Identity;
                string Username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                    emp = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (emp == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "User Not Found.");
                    Company company = HSapiFacade.GetCompanyByCompanyId(usercontext.CompanyId);
                    if (company == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Company Not Found.");

                    if (!string.IsNullOrEmpty(Model.PhoneNum))
                    {
                        #region Customer Create                        
                        var CustomerDetails = HSapiFacade.GetCustomerByPhoneNumber(Model.PhoneNum);
                        if (CustomerDetails == null)
                        {
                            #region InsertCustomer
                            CustomerDetails = new Customer()
                            {
                                CustomerId = Guid.NewGuid(),
                                FirstName = FirstName,
                                LastName = LastName,
                                CellNo = Model.PhoneNum,
                                PrimaryPhone = Model.PhoneNum,
                                EmailAddress = Model.EmailAddress,
                                Street = Model.CustomerStreet,
                                City = Model.CustomerCity,
                                State = Model.CustomerState,
                                ZipCode = Model.CustomerZipCode,
                                CreatedByUid = emp.UserId,
                                LastUpdatedBy = emp.UserName,
                                LastUpdatedByUid = emp.UserId,
                                Note = Model.EmailDescription,
                                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                JoinDate = DateTime.Now.UTCCurrentTime(),
                                IsActive = true,
                                PhoneType = "",
                                Carrier = "",
                                ReferringCustomer = Guid.Empty,
                                ChildOf = Guid.Empty,
                                LeadSource = "Online Booking",
                                Type = "Residential",
                                Status = "Appointment"
                            };
                            Id = HSapiFacade.InsertCustomers(CustomerDetails);
                            CustomerId = CustomerDetails.CustomerId.ToString();
                            #endregion

                            CustomerCompany cusCompany = new CustomerCompany()
                            {
                                CustomerId = CustomerDetails.CustomerId,
                                CompanyId = usercontext.CompanyId,
                                IsLead = false,
                                IsActive = true
                            };
                            var companycreateresult = HSapiFacade.InsertCustomerCompany(cusCompany);

                            CustomerSnapshot ObjCustomerSnapShot = new CustomerSnapshot
                            {
                                CustomerId = CustomerDetails.CustomerId,
                                CompanyId = usercontext.CompanyId,
                                Description = CustomerDetails.Note,
                                Logdate = DateTime.Now.UTCCurrentTime(),
                                Updatedby = emp.UserName,
                                Type = CustomerDetails.Type
                            };
                            var customersnapshotresult = HSapiFacade.InsertSnapshot(ObjCustomerSnapShot);
                        }
                        else
                        {
                            #region Update Customer
                            CustomerDetails.FirstName = FirstName;
                            CustomerDetails.LastName = LastName;
                            CustomerDetails.CellNo = Model.PhoneNum;
                            CustomerDetails.EmailAddress = Model.EmailAddress;
                            CustomerDetails.Street = Model.CustomerStreet;
                            CustomerDetails.City = Model.CustomerCity;
                            CustomerDetails.State = Model.CustomerState;
                            CustomerDetails.ZipCode = Model.CustomerZipCode;
                            CustomerDetails.CreatedByUid = emp.UserId;
                            CustomerDetails.PrimaryPhone = Model.PhoneNum;
                            CustomerDetails.LastUpdatedBy = emp.UserName;
                            CustomerDetails.LastUpdatedByUid = emp.UserId;
                            CustomerDetails.Note = Model.EmailDescription;
                            CustomerDetails.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            CustomerDetails.CreatedDate = DateTime.Now.UTCCurrentTime();
                            CustomerDetails.JoinDate = DateTime.Now.UTCCurrentTime();
                            CustomerDetails.IsActive = true;
                            CustomerDetails.PhoneType = "";
                            CustomerDetails.Carrier = "";
                            CustomerDetails.ReferringCustomer = Guid.Empty;
                            CustomerDetails.ChildOf = Guid.Empty;
                            CustomerDetails.LeadSource = "Online Booking";
                            CustomerDetails.Type = "Residential";
                            CustomerDetails.Status = "Appointment";
                            Id = CustomerDetails.Id;
                            CustomerId = CustomerDetails.CustomerId.ToString();
                            var customerupdateresult = HSapiFacade.UpdateCustomer(CustomerDetails);

                            CustomerSnapshot ObjCustomerSnapShot = new CustomerSnapshot
                            {
                                CustomerId = CustomerDetails.CustomerId,
                                CompanyId = usercontext.CompanyId,
                                Description = CustomerDetails.Note,
                                Logdate = DateTime.Now.UTCCurrentTime(),
                                Updatedby = emp.UserName,
                                Type = CustomerDetails.Type
                            };
                            var customersnapshotresult = HSapiFacade.InsertSnapshot(ObjCustomerSnapShot);

                            CustomerCompany customerCompany = HSapiFacade.GetCustomerCompanyByCustomerId(CustomerDetails.CustomerId, usercontext.CompanyId);
                            if (customerCompany != null)
                            {
                                customerCompany.IsLead = false;
                                customerCompany.ConvertionDate = DateTime.Now.UTCCurrentTime();
                                HSapiFacade.UpdateCustomerCompany(customerCompany);
                            }
                            else
                            {
                                CustomerCompany cusCompany = new CustomerCompany()
                                {
                                    CustomerId = CustomerDetails.CustomerId,
                                    CompanyId = usercontext.CompanyId,
                                    IsLead = false,
                                    IsActive = true
                                };
                                var companycreateresult = HSapiFacade.InsertCustomerCompany(cusCompany);
                            }
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Phone Number Not Found.");
                    }

                    Booking tempBooking = new Booking();
                    if (!string.IsNullOrEmpty(Model.Booking.BookingId))
                    {
                        tempBooking = HSapiFacade.GetByBookingId(Model.Booking.BookingId);

                        if (tempBooking == null || tempBooking.CompanyId != usercontext.CompanyId)
                        {
                            return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, "Booking is null", null));

                        }
                    }
                    else
                    {
                        if (CustomerId != "")
                        {
                            tempBooking = new Booking()
                            {

                                CustomerId = Guid.Parse(CustomerId),
                                CustomerName = FirstName + " " + LastName,
                                CompanyId = usercontext.CompanyId,
                                Amount = 0,
                                Status = "Init",
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CreatedBy = emp.UserId,
                                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                LastUpdatedBy = emp.UserId,
                                EmailAddress = Model.EmailAddress,
                                BookingSource = "Online"
                            };
                            tempBooking.PickUpLocation = Model.Booking.PickUpLocation;
                            tempBooking.DropOffLocation = Model.Booking.DropOffLocation;
                            tempBooking.Id = HSapiFacade.InsertBooking(tempBooking);
                            tempBooking.BookingId = tempBooking.Id.GenerateBookingNo();
                            HSapiFacade.UpdateBooking(tempBooking);
                        }
                    }



                    Model.Booking.Id = tempBooking.Id;
                    Model.Booking.CustomerName = FirstName + " " + LastName;
                    Model.CustomerName = Model.Booking.CustomerName;
                    Model.Booking.CreatedBy = tempBooking.CreatedBy;
                    Model.Booking.CustomerId = Guid.Parse(CustomerId);
                    Model.Booking.CreatedDate = tempBooking.CreatedDate;
                    Model.Booking.CompanyId = tempBooking.CompanyId;
                    Model.Booking.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    Model.Booking.LastUpdatedBy = emp.UserId;
                    Model.Booking.BookingId = tempBooking.BookingId;
                    Model.Booking.Message = Model.Booking.Message;
                    Model.Booking.BookingSource = "Online";
                    if (string.IsNullOrWhiteSpace(Model.Booking.Status))
                        Model.Booking.Status = LabelHelper.BookingStatus.Created;
                    HSapiFacade.UpdateBooking(Model.Booking);

                    #region booking Extra Item 
                    HSapiFacade.DeleteAllBookingExtraItemByBookingId(Model.Booking.Id);
                    if (Model.BookingExtraItem != null && Model.BookingExtraItem.Count > 0)
                    {
                        foreach (var item in Model.BookingExtraItem)
                        {

                            item.CreatedBy = usercontext.UserName;
                            item.CreatedDate = DateTime.Now.UTCCurrentTime();
                            item.BookingId = Model.Booking.Id;
                            HSapiFacade.InsertBookingExtraItem(item);
                        }
                    }

                    #endregion


                    #region CustomerSnapshot
                    var objBookingSnapshot = HSapiFacade.GetCustomerSnapshotDetail(Model.Booking.BookingId.ToString());
                    if (objBookingSnapshot.Count == 0)
                    {
                        var empobj = HSapiFacade.GetEmployeeByEmployeeId(emp.UserId);
                        if (empobj != null)
                        {
                            var updatedate = Model.Booking.LastUpdatedDate.UTCToClientTime();
                            CustomerSnapshot BookingLogObj = new CustomerSnapshot()
                            {
                                CustomerId = Guid.Parse(CustomerId),
                                CompanyId = usercontext.CompanyId,
                                Description = "Booking"
                                + string.Format
                                ("<a onclick=OpenTopToBottomModal('/Booking/AddLeadBooking?id={0}&CustomerId={1}') style='cursor: pointer;'>",
                                Model.Booking.Id, Id, CustomerId)
                                + "<b>" + Model.Booking.BookingId + "</b>" + "</a>",

                                Logdate = DateTime.Now.UTCCurrentTime(),
                                Updatedby = empobj.FirstName + " " + empobj.LastName,
                                Type = "BookingCreateHistory"
                            };
                            HSapiFacade.InsertSnapshot(BookingLogObj);
                        }
                    }
                    #endregion

                    #region Booking details
                    Model.SubTotal = 0;
                    HSapiFacade.DeleteAllBookingDetailsByBookingId(Model.Booking.BookingId);
                    if (Model.BookingDetailsList != null)
                    {
                        foreach (var item in Model.BookingDetailsList)
                        {
                            item.BookingId = Model.Booking.BookingId;
                            item.AddedBy = emp.UserId;
                            item.AddedDate = DateTime.Now.UTCCurrentTime();
                            item.CompanyId = usercontext.CompanyId;
                            HSapiFacade.InsertBookingDetails(item);
                        }
                        foreach (var item in Model.BookingDetailsList)
                        {
                            item.AddedBy = emp.UserId;
                            item.AddedDate = DateTime.Now.UTCCurrentTime();
                            item.CompanyId = usercontext.CompanyId;
                            Model.SubTotal = Model.SubTotal + (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
                        }
                    }
                    #endregion


                    #region Booking Email Send
                    string RugInformation = "";
                    int BookingRugCount = Model.BookingDetailsList.Count;

                    if (Model.BookingDetailsList != null)
                    {
                        int i = 1;
                        foreach (var item in Model.BookingDetailsList)
                        {

                            if (item.RugType == "Circle")
                            {
                                if (i == 1)
                                {
                                    RugInformation += "<div style = 'position: relative; border-top:1px solid #ccc; border-bottom:1px solid #ccc; padding:5px; background-color:white' ><div style = 'font-weight: 600;' >Rug Shape - " + item.RugType + "</div><div>" + (int)item.Radius + "\' " + (int)item.RadiusInch + "\" = " + ((double)item.TotalSize).ToString("n2") + " sq ft</div><div>" + item.Package + "($" + ((double)item.UnitPrice).ToString("n2") + "/sq ft)</div><div>Qty: " + (int)item.Quantity + "  -  Price: $" + ((double)item.TotalPrice).ToString("n2") + "</div></div>";
                                }
                                else
                                {
                                    RugInformation += "<div style = 'position: relative; border-bottom:1px solid #ccc; padding:5px;background-color:white' ><div style = 'font-weight: 600;' >Rug Shape - " + item.RugType + "</div><div>" + (int)item.Radius + "\' " + (int)item.RadiusInch + "\" = " + ((double)item.TotalSize).ToString("n2") + " sq ft</div><div>" + item.Package + "($" + ((double)item.UnitPrice).ToString("n2") + "/sq ft)</div><div>Qty: " + (int)item.Quantity + "  -  Price: $" + ((double)item.TotalPrice).ToString("n2") + "</div></div>";
                                }
                            }
                            else
                            {
                                if (i == 1)
                                {
                                    RugInformation += "<div style = 'position: relative; border-top:1px solid #ccc; border-bottom:1px solid #ccc; padding:5px;background-color:white' ><div style = 'font-weight: 600;' >Rug Shape - " + item.RugType + "</div><div>" + (int)item.Length + "\' " + (int)item.LengthInch + "\" x " + (int)item.Width + "\' " + (int)item.WidthInch + "\" = " + ((double)item.TotalSize).ToString("n2") + " sq ft</div><div>" + item.Package + "($" + ((double)item.UnitPrice).ToString("n2") + "/sq ft)</div><div>Qty: " + (int)item.Quantity + "  -  Price: $" + ((double)item.TotalPrice).ToString("n2") + "</div></div>";
                                }
                                else
                                {
                                    RugInformation += "<div style = 'position: relative; border-bottom:1px solid #ccc; padding:5px;background-color:white' ><div style = 'font-weight: 600;' >Rug Shape - " + item.RugType + "</div><div>" + (int)item.Length + "\' " + (int)item.LengthInch + "\" x " + (int)item.Width + "\' " + (int)item.WidthInch + "\" = " + ((double)item.TotalSize).ToString("n2") + " sq ft</div><div>" + item.Package + "($" + ((double)item.UnitPrice).ToString("n2") + "/sq ft)</div><div>Qty: " + (int)item.Quantity + "  -  Price: $" + ((double)item.TotalPrice).ToString("n2") + "</div></div>";
                                }
                            }
                            i++;
                        }
                    }
                    if (Model.BookingExtraItem != null)
                    {

                        foreach (var item in Model.BookingExtraItem)
                        {
                            if (BookingRugCount > 0)
                            {
                                RugInformation += "<div style = 'position: relative; border-bottom:1px solid #ccc; padding:5px;background-color:white' ><div style = 'font-weight: 600;' >Service</div><div>" + item.EquipName + "($" + ((double)item.UnitPrice).ToString("n2") + " / sq ft)</div><div> Qty: " + (int)item.Quantity + " - Price: $" + ((double)item.TotalPrice).ToString("n2") + " </div><div> Notes: " + item.EquipDetail + "</div></div>";
                            }
                            else
                            {
                                RugInformation += "<div style = 'position: relative; border-top:1px solid #ccc; border-bottom:1px solid #ccc; padding:5px;background-color:white' ><div style = 'font-weight: 600;' >Service</div><div>" + item.EquipName + "($" + ((double)item.UnitPrice).ToString("n2") + " / sq ft)</div><div> Qty: " + (int)item.Quantity + " - Price: $" + ((double)item.TotalPrice).ToString("n2") + " </div><div> Notes: " + item.EquipDetail + "</div></div>";
                            }

                        }
                    }

                    double price = TotalPrice;
                    double tax = (double)(price * (8.25 / 100));
                    RugTrackerBookingEmail BookingEmail = new RugTrackerBookingEmail();
                    BookingEmail.CustomerName = FirstName + " " + LastName;
                    BookingEmail.CustomerId = Id.ToString();
                    BookingEmail.OrderID = Model.Booking.Id.ToString();
                    BookingEmail.ToEmail = Model.EmailAddress;
                    BookingEmail.TotalAmount = ((double)Model.Booking.TotalAmount).ToString("n2");
                    BookingEmail.PickupDateTime = Extentions.RugDateFormat((DateTime)Model.Booking.PickUpDate);
                    BookingEmail.DropoffDateTime = Extentions.RugDateFormat((DateTime)Model.Booking.DropOffDate);
                    BookingEmail.Tax = tax.ToString("n2");
                    BookingEmail.Amount = price.ToString("n2");
                    BookingEmail.RugInformation = RugInformation;
                    BookingEmail.Phone = Model.PhoneNum;
                    BookingEmail.Street = Model.CustomerStreet;
                    BookingEmail.Address = Model.CustomerCity + ", " + Model.CustomerState + " " + Model.CustomerZipCode;
                    BookingEmail.CustomerLink = "https://www.myrugtracker.com/Customer/Customerdetail/?id=" + Id;

                    var SendEmailResult = HSapiFacade.SendBookingEmail(BookingEmail, usercontext.CompanyId);
                    #endregion
                }

                return Request.CreateResponse(HttpStatusCode.OK, new BaseAPIResponse(true, "Booking Successfully Saved.", new { Id = Model.Booking.Id, BookingId = Model.Booking.BookingId, CustomerId = Id }));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, ex.Message, null));
            }
        }


        #endregion

        #region GeeseRelief API

        [Authorize]
        [Route("GetRoutes")]
        public HttpResponseMessage GetRoutes()
        {
            APIInitialize();
            int PageNo = 0;
            int PageSize = 0;
            Guid UserId = new Guid();

            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("PageNo"))
            {
                int.TryParse(headers.GetValues("PageNo").First(), out PageNo);
            }
            if (headers.Contains("PageSize"))
            {
                int.TryParse(headers.GetValues("PageSize").First(), out PageSize);
            }
            if (headers.Contains("UserId"))
            {
                Guid.TryParse(headers.GetValues("UserId").First(), out UserId);
            }
            var identity = (ClaimsIdentity)User.Identity;
            if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
            {
                var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");

                //var RouteList = HSapiFacade.GetAllCustomerRoutes(PageNo, PageSize);
                if (UserId == null)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");
                }
                var UserRole = HSapiFacade.GetEmployeeWithRoleByUserId(UserId);
                if (UserRole != null)
                {
                    if (UserRole.Role == "Employee")
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, HSapiFacade.GetAllCustomerRoutes(PageNo, PageSize, UserId));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, HSapiFacade.GetAllCustomerRoutes(PageNo, PageSize, new Guid()));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "User Not Found.");
                }
                //return Request.CreateResponse(HttpStatusCode.OK, HSapiFacade.GetAllCustomerRoutes(PageNo, PageSize, UserId));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
            }
        }

        [Authorize]
        [Route("GetCustomers")]
        public HttpResponseMessage GetCustomers()
        {
            APIInitialize();
            int PageNo = 0;
            int PageSize = 0;
            Guid RouteId = new Guid();
            int TotalCustomer = 0;
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("PageNo"))
            {
                int.TryParse(headers.GetValues("PageNo").First(), out PageNo);
            }
            if (headers.Contains("PageSize"))
            {
                int.TryParse(headers.GetValues("PageSize").First(), out PageSize);
            }
            if (headers.Contains("RouteId"))
            {
                Guid.TryParse(headers.GetValues("RouteId").First(), out RouteId);
                if (RouteId == Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed,
                    new BaseAPIResponse(false, "'Route Id' field is required.", null));
                }
            }
            var identity = (ClaimsIdentity)User.Identity;
            if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
            {
                var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");

                List<GeeseCustomerDetailModel> CustomerDetail = new List<GeeseCustomerDetailModel>();
                var CustomerIdList = HSapiFacade.GetAllCustomerIdByRouteId(RouteId);
                if (CustomerIdList != null)
                {
                    foreach (var IdList in CustomerIdList)
                    {
                        CustomerDetail.Add(HSapiFacade.GetAllCustomerByRouteId(IdList.CustomerId, PageNo, PageSize));
                    }
                    TotalCustomer = CustomerIdList.Count();
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { CustomerDetail, TotalCustomer });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
            }
        }

        [Authorize]
        [Route("CheckIn")]
        public HttpResponseMessage CheckIn()
        {
            APIInitialize();

            int Id = 0;
            int GeeseCount = 0;
            Guid CustomerId = Guid.Empty;
            Guid UserId = Guid.Empty;

            var re = Request;
            var headers = re.Headers;
            bool result = false;

            if (headers.Contains("GeeseCount"))
            {
                int.TryParse(headers.GetValues("GeeseCount").First(), out GeeseCount);
                //if (GeeseCount == 0)
                //{
                //    return Request.CreateResponse(HttpStatusCode.PreconditionFailed,
                //    new BaseAPIResponse(false, "'Geese Count' Should Greater 0.", null));
                //}
            }
            if (headers.Contains("CustomerId"))
            {
                Guid.TryParse(headers.GetValues("CustomerId").First(), out CustomerId);
                if (CustomerId == Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed,
                    new BaseAPIResponse(false, "'Customer Id' field is required.", null));
                }
            }
            if (headers.Contains("UserId"))
            {
                Guid.TryParse(headers.GetValues("UserId").First(), out UserId);
                if (UserId == Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed,
                    new BaseAPIResponse(false, "'User Id' field is required.", null));
                }
            }
            string message = "";
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string Username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));

                    CustomerCheckLog CC = new CustomerCheckLog();
                    Customer Customer = HSapiFacade.GetCustomerByCustomerId(CustomerId);
                    if (Customer == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                    }
                    Employee Employee = HSapiFacade.GetEmployeeByEmployeeId(UserId);
                    if (Employee == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                    }
                    CustomerCheckLog CCL = HSapiFacade.GetCustomerCheckLogByCustomerId(CustomerId, UserId);
                    {
                        if (CCL != null)
                        {
                            if (CCL.CheckType == "In")
                            {
                                CCL.CheckType = "Out";
                                CCL.CheckOutTime = DateTime.Now.UTCCurrentTime();
                                CCL.ToatlTime = (int)DateTime.Now.UTCCurrentTime().Subtract(CCL.CheckInTime.Value).TotalSeconds;
                                result = HSapiFacade.UpdateCustomerCheckLog(CCL);
                            }
                        }
                    }
                    CustomerRoute CR = HSapiFacade.GetCustomerRouteByCustomerId(CustomerId);
                    if (CR == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                    }
                    CR.LastVisit = DateTime.Now.UTCCurrentTime();
                    result = HSapiFacade.UpdateCustomerRoute(CR);
                    CC = new CustomerCheckLog()
                    {
                        CustomerId = CustomerId,
                        UserId = UserId,
                        RouteId = CR.RouteId,
                        CheckInTime = DateTime.Now.UTCCurrentTime(),
                        CheckType = "In",
                        GeeseCount = GeeseCount,
                        CreadtedBy = UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime()
                    };
                    Id = HSapiFacade.InsertCustomerCheckLog(CC);
                    result = Id > 0;

                    message += "Check In Successfully.";
                    return Request.CreateResponse(HttpStatusCode.OK, new BaseAPIResponse(true, message, new { Id = Id, UserName = Username }));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Token Expired.", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, ex.Message, null));
            }
        }

        [Authorize]
        [Route("CheckOut")]
        public HttpResponseMessage CheckOut()
        {
            APIInitialize();
            int Id = 0;
            Guid CustomerId = Guid.Empty;
            Guid UserId = Guid.Empty;

            var re = Request;
            var headers = re.Headers;
            bool result = false;

            if (headers.Contains("CustomerId"))
            {
                Guid.TryParse(headers.GetValues("CustomerId").First(), out CustomerId);
                if (CustomerId == Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed,
                    new BaseAPIResponse(false, "'Customer Id' field is required.", null));
                }
            }
            if (headers.Contains("UserId"))
            {
                Guid.TryParse(headers.GetValues("UserId").First(), out UserId);
                if (UserId == Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed,
                    new BaseAPIResponse(false, "User Id' field is required.", null));
                }
            }

            string message = "";
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string Username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));


                    Customer Customer = HSapiFacade.GetCustomerByCustomerId(CustomerId);
                    if (Customer == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                    }
                    Employee Employee = HSapiFacade.GetEmployeeByEmployeeId(UserId);
                    if (Employee == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                    }
                    CustomerCheckLog CC = HSapiFacade.GetCustomerCheckLogByCustomerId(CustomerId, UserId);
                    if (CC == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                    }
                    CC.CheckOutTime = DateTime.Now.UTCCurrentTime();
                    CC.CheckType = "Out";
                    CC.ToatlTime = (int)DateTime.Now.UTCCurrentTime().Subtract(CC.CheckInTime.Value).TotalSeconds;
                    result = HSapiFacade.UpdateCustomerCheckLog(CC);

                    message += "Check Out Successfully.";
                    Id = CC.Id;
                    return Request.CreateResponse(HttpStatusCode.OK, new BaseAPIResponse(true, message, new { Id = Id, UserName = Username }));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Token Expired.", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, ex.Message, null));
            }
        }

        [Authorize]
        [Route("GetUserHistory")]
        public HttpResponseMessage GetUserHistory()
        {
            APIInitialize();
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            Guid CustomerId = new Guid();

            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("StartDate"))
            {
                string StartDateSt = headers.GetValues("StartDate").First();
                DateTime.TryParse(StartDateSt, out StartDate);
            }
            if (headers.Contains("EndDate"))
            {
                string EndDateSt = headers.GetValues("EndDate").First();
                DateTime.TryParse(EndDateSt, out EndDate);
            }
            if (headers.Contains("CustomerId"))
            {
                Guid.TryParse(headers.GetValues("CustomerId").First(), out CustomerId);
                if (CustomerId == Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed,
                    new BaseAPIResponse(false, "'Customer Id' field is required.", null));
                }
            }
            var identity = (ClaimsIdentity)User.Identity;
            if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
            {
                var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                if (usercontext == null)
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Authorization Denied.");

                Customer Customer = HSapiFacade.GetCustomerByCustomerId(CustomerId);
                if (Customer == null)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                }

                //List<GeeseCustomerDetailHistoryModel> CustomerDetail = new List<GeeseCustomerDetailHistoryModel>();
                //var CustomerIdList = HSapiFacade.GetAllCustomerIdByUserId(UserId);
                //if (CustomerIdList != null)
                //{
                //    foreach (var IdList in CustomerIdList)
                //    {
                //        CustomerDetail.Add(HSapiFacade.GetCustomerHistoryByCustomerId(IdList.CustomerId, StartDate, EndDate));
                //    }
                //}
                return Request.CreateResponse(HttpStatusCode.OK, HSapiFacade.GetCustomerHistoryByCustomerId(CustomerId, StartDate, EndDate));
                //return Request.CreateResponse(HttpStatusCode.OK, new { HSapiFacade.GetCustomerHistoryByCustomerId(IdList.CustomerId, StartDate, EndDate) });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Token Expired.");
            }
        }

        [Authorize]
        [Route("SendNote")]
        public HttpResponseMessage SendNote()
        {
            APIInitialize();

            long Id = 0;
            string Type = "Note";
            Guid CustomerId = Guid.Empty;
            string Note = "";


            var re = Request;
            var headers = re.Headers;
            bool result = false;

            if (headers.Contains("CustomerId"))
            {
                Guid.TryParse(headers.GetValues("CustomerId").First(), out CustomerId);
                if (CustomerId == Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed,
                    new BaseAPIResponse(false, "'Customer Id' field is required.", null));
                }
            }
            if (headers.Contains("Note"))
            {
                Note = headers.GetValues("Note").First();
            }
            string message = "";
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string Username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));

                    Customer Customer = HSapiFacade.GetCustomerByCustomerId(CustomerId);
                    if (Customer == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                    }
                    Employee Employee = HSapiFacade.GetEmployeeByUsername(Username);
                    if (Employee == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                    }
                    CustomerFile CusF = new CustomerFile();
                    CusF = new CustomerFile()
                    {
                        CustomerId = CustomerId,
                        FileId = Guid.NewGuid(),
                        CompanyId = usercontext.CompanyId,
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        GeeseFileType = Type,
                        FileDescription = Note,
                        CreatedBy = Employee.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = Employee.UserId,
                        UpdatedDate = DateTime.Now.UTCCurrentTime()
                    };
                    Id = HSapiFacade.InsertCustomerFile(CusF);
                    result = Id > 0;

                    message += "Note Insert Successfully.";
                    return Request.CreateResponse(HttpStatusCode.OK, new BaseAPIResponse(true, message, new { Id = Id, UserName = Username }));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Token Expired.", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, ex.Message, null));
            }
        }

        [Authorize]
        [Route("SaveMedia")]
        public HttpResponseMessage SaveMedia()
        {
            APIInitialize();

            long Id = 0;
            string Type = "Media";
            Guid CustomerId = Guid.Empty;
            string Path = "";
            string Note = "";
            string Address = "";


            var re = Request;
            var headers = re.Headers;
            bool result = false;

            if (headers.Contains("CustomerId"))
            {
                Guid.TryParse(headers.GetValues("CustomerId").First(), out CustomerId);
                if (CustomerId == Guid.Empty)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed,
                    new BaseAPIResponse(false, "'Customer Id' field is required.", null));
                }
            }
            if (headers.Contains("Path"))
            {
                Path = headers.GetValues("Path").First();
            }
            if (headers.Contains("Note"))
            {
                Note = headers.GetValues("Note").First();
            }
            if (headers.Contains("Address"))
            {
                Address = headers.GetValues("Address").First();
            }
            string message = "";
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string Username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (usercontext == null)
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));

                    Customer Customer = HSapiFacade.GetCustomerByCustomerId(CustomerId);
                    if (Customer == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                    }
                    Employee Employee = HSapiFacade.GetEmployeeByUsername(Username);
                    if (Employee == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Authorization Denied."));
                    }
                    CustomerFile CusF = new CustomerFile();
                    CusF = new CustomerFile()
                    {
                        CustomerId = CustomerId,
                        FileId = Guid.NewGuid(),
                        CompanyId = usercontext.CompanyId,
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        GeeseFileType = Type,
                        Filename = Path,
                        FileDescription = Note,
                        FileFullName = Address,
                        CreatedBy = Employee.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = Employee.UserId,
                        UpdatedDate = DateTime.Now.UTCCurrentTime()
                    };
                    Id = HSapiFacade.InsertCustomerFile(CusF);
                    result = Id > 0;

                    message += "Media Insert Successfully.";
                    return Request.CreateResponse(HttpStatusCode.OK, new BaseAPIResponse(true, message, new { Id = Id, UserName = Username }));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, new BaseAPIResponse(false, "Token Expired.", null));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, new BaseAPIResponse(false, ex.Message, null));
            }
        }
        #endregion

        #region UCC API
        [Authorize]
        [Route("details-ucc-customer")]
        [HttpGet]
        public HttpResponseMessage UCCCustomerDetails()
        {
            int cusId = 0;
            ThirdPartyCustomerAPIModel ReturnModel = new ThirdPartyCustomerAPIModel();
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("customerId"))
                {
                    int.TryParse(headers.GetValues("customerId").First(), out cusId);
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    Employee emp = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (emp == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("User Not Found."));
                    }
                    ComId = usercontext.CompanyId.ToString();
                    Customer cus = HSapiFacade.GetCustomerById(cusId);
                    if (cus != null && cus.Id > 0 && !string.IsNullOrWhiteSpace(cus.UCCRefId))
                    {
                        ResultUcc model = new ResultUcc();
                        #region Get Credential From GlobalSetting
                        var UserName = "";
                        var Password = "";
                        var UccUserName = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccUserName");
                        if (UccUserName != null)
                        {
                            UserName = UccUserName.Value;
                        }
                        var UccPassword = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccPassword");
                        if (UccPassword != null)
                        {
                            Password = UccPassword.Value;
                        }
                        #region Check Production or not
                        bool UccInProduction = false;
                        string UccUrl = "";
                        GlobalSetting globset2 = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccInProduction");
                        if (globset2 != null && globset2.Value.ToLower() == "true")
                        {
                            UccInProduction = true;
                        }
                        if (UccInProduction == true)
                        {
                            UccUrl = "https://dealer2.teamucc.com/SiteGroupGateway/OpenAPI/";
                        }
                        else
                        {
                            UccUrl = "https://dealer.teamucc.com/SiteGroupGatewayDev/OpenAPI/";
                        }
                        #endregion

                        #endregion
                        var client = new RestClient(UccUrl + "method/GetSite");
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("postman-token", "5f1e7e08-821d-d34f-ef2e-2a2a0fa04fe9");
                        request.AddHeader("cache-control", "no-cache");
                        request.AddParameter("application/json", "{\n\t\"UserName\":\"" + UserName + "\",\n\t\"Password\":\"" + Password + "\",\n\t\"TransmitterCode\":\"" + cus.UCCRefId + "\"\n\t\n}", ParameterType.RequestBody);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        try
                        {
                            IRestResponse response = client.Execute(request);
                            model = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultUcc>(response.Content);
                            if (model.Result != null)
                            {
                                ReturnModel.GeneralInformation = new ThirdPartyCustomerModel();
                                ReturnModel.EmergencyContacts = new List<EmergencyContactsModel>();
                                ReturnModel.Agencies = new List<AgenciesModel>();
                                ReturnModel.SecurityZones = new List<SecurityZonesModel>();
                                ReturnModel.Equipments = new List<UCCEquipmentModel>();

                                ReturnModel.GeneralInformation.SiteName = model.Result.SiteName;
                                ReturnModel.GeneralInformation.Address = model.Result.SiteAddress != null ? model.Result.SiteAddress.ToString() : "";
                                //ReturnModel.GeneralInformation.Address2 = model.Result.SiteAddr2.ToString();
                                ReturnModel.GeneralInformation.Street = model.Result.CrossStreet != null ? model.Result.CrossStreet.ToString() : "";
                                ReturnModel.GeneralInformation.City = model.Result.City != null ? model.Result.City.ToString() : "";
                                ReturnModel.GeneralInformation.State = model.Result.State != null ? model.Result.State.ToString() : "";
                                ReturnModel.GeneralInformation.ZipCode = model.Result.ZipCode != null ? model.Result.ZipCode.ToString() : "";
                                ReturnModel.GeneralInformation.County = model.Result.County != null ? model.Result.County.ToString() : "";
                                ReturnModel.GeneralInformation.SiteType = model.Result.SiteType != null ? model.Result.SiteType.ToString() : "";
                                ReturnModel.GeneralInformation.CSAccountNumber = cus.UCCRefId;
                                ReturnModel.GeneralInformation.AbortCode = model.Result.Codewords != null && model.Result.Codewords.Count() > 0 ? model.Result.Codewords.FirstOrDefault().Codeword : "";
                                ReturnModel.GeneralInformation.UccDispatchType = model.Result.DispatchTypes != null && model.Result.DispatchTypes.Count() > 0 ? string.Join(",", model.Result.DispatchTypes.Select(x => x.DispatchType)) : "";
                                ReturnModel.EmergencyContacts = model.Result.Contacts != null && model.Result.Contacts.Count() > 0 ? model.Result.Contacts.Select(x => new EmergencyContactsModel() { FirstName = x.FirstName, LastName = x.LastName, Phone = (x.Phones != null && x.Phones.Count() > 0 ? x.Phones.FirstOrDefault().PhoneNumber : "") }).ToList() : null;
                                ReturnModel.Equipments = model.Result.Devices != null && model.Result.Devices.Count() > 0 ? model.Result.Devices.Select(x => new UCCEquipmentModel() { TransmitterCode = x.TransmitterCode, DeviceType = x.DeviceType, ReceiverPhone = x.ReceiverPhone, PanelPhone = x.PanelPhone }).ToList() : null;
                                #region Syncing Zones
                                List<CustomerSecurityZones> securityZonesList = new List<CustomerSecurityZones>();
                                securityZonesList = HSapiFacade.GetAllCustomerSecurityZoneByCustomerId(cus.CustomerId, "'UCC'");
                                if (securityZonesList != null && securityZonesList.Count > 0)
                                {
                                    foreach (var item in securityZonesList)
                                    {
                                        HSapiFacade.DeleteCustomerSecurityZone(item.ID);
                                    }
                                }
                                if (model.Result.Devices != null && model.Result.Devices.Count > 0)
                                {
                                    ReturnModel.GeneralInformation.PanelPhone = model.Result.Devices[0].PanelPhone;
                                    ReturnModel.GeneralInformation.ReceiverPhone = model.Result.Devices[0].ReceiverPhone;
                                    ReturnModel.GeneralInformation.UccDeviceType = string.Join(",", model.Result.Devices.Select(x => x.DeviceType));
                                    foreach (var item in model.Result.Devices)
                                    {
                                        if (item.Points != null && item.Points.Count > 0)
                                        {
                                            foreach (var zones in item.Points)
                                            {
                                                CustomerSecurityZones securityZones = new CustomerSecurityZones();
                                                securityZones.CustomerId = cus.CustomerId;
                                                securityZones.ZoneNumber = zones.Point;
                                                securityZones.Platform = "UCC";
                                                securityZones.Location = zones.Description;
                                                securityZones.EventCode = zones.EventCode;
                                                securityZones.CreatedBy = emp.UserId;
                                                securityZones.CreatedDate = DateTime.Now.UTCCurrentTime();
                                                HSapiFacade.InsertCustomerSecurityZone(securityZones);
                                            }

                                        }
                                    }
                                }
                                #endregion
                                #region Syncing Agencies
                                List<CustomerThirdPartyAgency> agencyList = new List<CustomerThirdPartyAgency>();
                                agencyList = HSapiFacade.GetAllCustomerThirdPartyAgencyByCustomerId(cus.CustomerId, "'UCC'");
                                if (agencyList != null && agencyList.Count > 0)
                                {
                                    foreach (var item in agencyList)
                                    {
                                        HSapiFacade.DeleteCustomerThirdPartyAgency(item.Id);
                                    }
                                }
                                if (model.Result.SiteAgencies != null && model.Result.SiteAgencies.Count > 0)
                                {
                                    CustomerThirdPartyAgency agency = new CustomerThirdPartyAgency();
                                    foreach (var item in model.Result.SiteAgencies)
                                    {
                                        agency = new CustomerThirdPartyAgency();
                                        agency.AgencyName = item.AgencyName;
                                        agency.AgencyNo = item.AgencyNum > 0 ? item.AgencyNum.ToString() : "";
                                        agency.Agencytype = item.AgencyType;
                                        agency.CustomerId = cus.CustomerId;
                                        agency.Phone = item.AgencyPhone;
                                        agency.PermitNo = item.Permit != null ? item.Permit.ToString() : "";
                                        agency.Platform = "UCC";
                                        HSapiFacade.InsertCustomerAgency(agency);
                                    }
                                }
                                #endregion


                                List<CustomerSecurityZones> securityZoneList = HSapiFacade.GetAllCustomerSecurityZoneByCustomerId(cus.CustomerId, "'UCC'");
                                if (securityZoneList != null && securityZoneList.Count() > 0)
                                {
                                    ReturnModel.SecurityZones.AddRange(securityZoneList.Select(x => new SecurityZonesModel() { EventCode = x.EventCode, Location = x.Location, Number = x.ZoneNumber }));
                                }
                                List<CustomerThirdPartyAgency> thirdPartyAgency = HSapiFacade.GetAllCustomerThirdPartyAgencyByCustomerId(cus.CustomerId, "'UCC'");
                                if (thirdPartyAgency != null && thirdPartyAgency.Count() > 0)
                                {
                                    ReturnModel.Agencies.AddRange(thirdPartyAgency.Select(x => new AgenciesModel() { Name = x.AgencyName, Phone = x.Phone, Number = x.AgencyNo }));
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<ThirdPartyCustomerAPIModel>.Success(null));
                            }
                        }
                        catch (Exception ex)
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<ThirdPartyCustomerAPIModel>.Success(null));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Token Expired."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<ThirdPartyCustomerAPIModel>.Success(ReturnModel));
        }

        [Authorize]
        [Route("ucc-customer")]
        [HttpGet]
        public HttpResponseMessage AddUCCCustomer()
        {
            int cusId = 0;
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("customerId"))
                {
                    int.TryParse(headers.GetValues("customerId").First(), out cusId);
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    ThirdPartyCustomerAPIModel ReturnModel = new ThirdPartyCustomerAPIModel();
                    ReturnModel.GeneralInformation = new ThirdPartyCustomerModel();
                    ReturnModel.EmergencyContacts = new List<EmergencyContactsModel>();
                    ReturnModel.Agencies = new List<AgenciesModel>();
                    ReturnModel.SecurityZones = new List<SecurityZonesModel>();
                    ReturnModel.Equipments = new List<UCCEquipmentModel>();
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    Employee emp = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (emp == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("User Not Found."));
                    }
                    ComId = usercontext.CompanyId.ToString();
                    Customer cus = HSapiFacade.GetCustomerById(cusId);
                    if (cus != null && cus.Id > 0)
                    {
                        List<CustomerSecurityZones> securityZonesList = new List<CustomerSecurityZones>();
                        securityZonesList = HSapiFacade.GetAllCustomerSecurityZoneByCustomerId(cus.CustomerId, "'UCC'");
                        if (securityZonesList != null && securityZonesList.Count > 0)
                        {
                            foreach (var item in securityZonesList)
                            {
                                HSapiFacade.DeleteCustomerSecurityZone(item.ID);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(cus.UCCRefId))
                        {
                            ResultUcc model = new ResultUcc();
                            #region Get Credential From GlobalSetting
                            var UserName = "";
                            var Password = "";
                            var UccUserName = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccUserName");
                            if (UccUserName != null)
                            {
                                UserName = UccUserName.Value;
                            }
                            var UccPassword = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccPassword");
                            if (UccPassword != null)
                            {
                                Password = UccPassword.Value;
                            }
                            #region Check Production or not
                            bool UccInProduction = false;
                            string UccUrl = "";
                            GlobalSetting globset2 = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccInProduction");
                            if (globset2 != null && globset2.Value.ToLower() == "true")
                            {
                                UccInProduction = true;
                            }
                            if (UccInProduction == true)
                            {
                                UccUrl = "https://dealer2.teamucc.com/SiteGroupGateway/OpenAPI/";
                            }
                            else
                            {
                                UccUrl = "https://dealer.teamucc.com/SiteGroupGatewayDev/OpenAPI/";
                            }
                            #endregion

                            #endregion

                            var client = new RestClient(UccUrl + "method/GetSite");
                            var request = new RestRequest(Method.POST);
                            request.AddHeader("postman-token", "5f1e7e08-821d-d34f-ef2e-2a2a0fa04fe9");
                            request.AddHeader("cache-control", "no-cache");
                            request.AddParameter("application/json", "{\n\t\"UserName\":\"" + UserName + "\",\n\t\"Password\":\"" + Password + "\",\n\t\"TransmitterCode\":\"" + cus.UCCRefId + "\"\n\t\n}", ParameterType.RequestBody);
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                            try
                            {
                                IRestResponse response = client.Execute(request);
                                model = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultUcc>(response.Content);
                                if (model.Result != null)
                                {
                                    ReturnModel.GeneralInformation.SiteName = model.Result.SiteName;
                                    ReturnModel.GeneralInformation.Address = model.Result.SiteAddress != null ? model.Result.SiteAddress.ToString() : "";
                                    //ReturnModel.GeneralInformation.Address2 = model.Result.SiteAddr2.ToString();
                                    ReturnModel.GeneralInformation.Street = model.Result.CrossStreet != null ? model.Result.CrossStreet.ToString() : "";
                                    ReturnModel.GeneralInformation.City = model.Result.City != null ? model.Result.City.ToString() : "";
                                    ReturnModel.GeneralInformation.State = model.Result.State != null ? model.Result.State.ToString() : "";
                                    ReturnModel.GeneralInformation.ZipCode = model.Result.ZipCode != null ? model.Result.ZipCode.ToString() : "";
                                    ReturnModel.GeneralInformation.County = model.Result.County != null ? model.Result.County.ToString() : "";
                                    ReturnModel.GeneralInformation.SiteType = model.Result.SiteType != null ? model.Result.SiteType.ToString() : "";
                                    ReturnModel.GeneralInformation.CSAccountNumber = cus.UCCRefId;
                                    ReturnModel.GeneralInformation.AbortCode = model.Result.Codewords != null && model.Result.Codewords.Count() > 0 ? model.Result.Codewords.FirstOrDefault().Codeword : "";
                                    ReturnModel.GeneralInformation.UccDispatchType = model.Result.DispatchTypes != null && model.Result.DispatchTypes.Count() > 0 ? string.Join(",", model.Result.DispatchTypes.Select(x => x.DispatchType)) : "";
                                    ReturnModel.EmergencyContacts = model.Result.Contacts != null && model.Result.Contacts.Count() > 0 ? model.Result.Contacts.Select(x => new EmergencyContactsModel() { FirstName = x.FirstName, LastName = x.LastName, Phone = (x.Phones != null && x.Phones.Count() > 0 ? x.Phones.FirstOrDefault().PhoneNumber : "") }).ToList() : null;

                                    #region Syncing Zones

                                    if (model.Result.Devices != null && model.Result.Devices.Count > 0)
                                    {
                                        ReturnModel.GeneralInformation.PanelPhone = model.Result.Devices[0].PanelPhone;
                                        ReturnModel.GeneralInformation.ReceiverPhone = model.Result.Devices[0].ReceiverPhone;
                                        ReturnModel.GeneralInformation.UccDeviceType = string.Join(",", model.Result.Devices.Select(x => x.DeviceType));
                                        foreach (var item in model.Result.Devices)
                                        {
                                            if (item.Points != null && item.Points.Count > 0)
                                            {
                                                foreach (var zones in item.Points)
                                                {
                                                    CustomerSecurityZones securityZones = new CustomerSecurityZones();
                                                    securityZones.CustomerId = cus.CustomerId;
                                                    securityZones.ZoneNumber = zones.Point;
                                                    securityZones.Platform = "UCC";
                                                    securityZones.Location = zones.Description;
                                                    securityZones.EventCode = zones.EventCode;
                                                    securityZones.CreatedBy = emp.UserId;
                                                    securityZones.CreatedDate = DateTime.Now.UTCCurrentTime();
                                                    HSapiFacade.InsertCustomerSecurityZone(securityZones);
                                                    ReturnModel.SecurityZones.Add(new SecurityZonesModel() { Number = securityZones.ZoneNumber, EventCode = securityZones.EventCode, Location = securityZones.Location });
                                                }

                                            }
                                        }
                                    }
                                    #endregion

                                    #region Syncing Agencies
                                    List<CustomerThirdPartyAgency> agencyList = new List<CustomerThirdPartyAgency>();
                                    agencyList = HSapiFacade.GetAllCustomerThirdPartyAgencyByCustomerId(cus.CustomerId, "'UCC'");
                                    if (agencyList != null && agencyList.Count > 0)
                                    {
                                        foreach (var item in agencyList)
                                        {
                                            HSapiFacade.DeleteCustomerThirdPartyAgency(item.Id);
                                        }
                                    }
                                    if (model.Result.SiteAgencies != null && model.Result.SiteAgencies.Count > 0)
                                    {
                                        CustomerThirdPartyAgency agency = new CustomerThirdPartyAgency();
                                        foreach (var item in model.Result.SiteAgencies)
                                        {
                                            agency = new CustomerThirdPartyAgency();
                                            agency.AgencyName = item.AgencyName;
                                            agency.AgencyNo = item.AgencyNum > 0 ? item.AgencyNum.ToString() : "";
                                            agency.Agencytype = item.AgencyType;
                                            agency.CustomerId = cus.CustomerId;
                                            agency.Phone = item.AgencyPhone;
                                            agency.PermitNo = item.Permit != null ? item.Permit.ToString() : "";
                                            agency.Platform = "UCC";
                                            HSapiFacade.InsertCustomerAgency(agency);
                                            ReturnModel.Agencies.Add(new AgenciesModel() { Name = agency.AgencyName, Number = agency.AgencyNo, Phone = agency.Phone });
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("UCC Customer Not Found"));
                                }
                            }
                            catch (Exception ex)
                            {
                                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
                            }
                        }
                        else
                        {
                            string DisplayName = "";
                            if (!string.IsNullOrEmpty(cus.BusinessName))
                            {
                                DisplayName = cus.BusinessName;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(cus.FirstName))
                                {
                                    DisplayName = cus.FirstName + ' ' + cus.LastName;
                                }

                            }
                            #region ModelAlways filldup from customer
                            ReturnModel.GeneralInformation.SiteName = DisplayName;
                            ReturnModel.GeneralInformation.Street = cus.Street;
                            ReturnModel.GeneralInformation.Address = cus.Address2;
                            ReturnModel.GeneralInformation.City = cus.City;
                            ReturnModel.GeneralInformation.State = cus.State;
                            ReturnModel.GeneralInformation.ZipCode = cus.ZipCode;
                            ReturnModel.GeneralInformation.County = cus.County;
                            ReturnModel.GeneralInformation.SiteType = cus.Type;
                            ReturnModel.GeneralInformation.CSAccountNumber = GetCSNumber(cus, "UCC", usercontext.CompanyId);
                            ReturnModel.GeneralInformation.PanelPhone = cus.PrimaryPhone != "" ? cus.PrimaryPhone : cus.CellNo;
                            ReturnModel.GeneralInformation.AbortCode = cus.Passcode;
                            List<EmergencyContact> emergencyContactList = HSapiFacade.GetAllEmergencyContactByCustomerId(cus.CustomerId);
                            if (emergencyContactList != null && emergencyContactList.Count > 0)
                            {
                                foreach (var item in emergencyContactList)
                                {
                                    ReturnModel.EmergencyContacts.Add(new EmergencyContactsModel() { FirstName = item.FirstName, LastName = item.LastName, Phone = item.Phone });
                                }
                            }
                            #endregion
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<ThirdPartyCustomerAPIModel>.Success(ReturnModel));

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("UCC Customer Not Found"));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Token Expired."));
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }
        }

        [Authorize]
        [Route("create-ucc-customer")]
        [HttpPost]
        public HttpResponseMessage CreateUCCCustomer([System.Web.Http.FromBody] ThirdPartyCustomerAPIModel Model)
        {

            int cusId = 0;
            string Code = string.Empty;
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("customerId"))
            {
                int.TryParse(headers.GetValues("customerId").First(), out cusId);
            }
            if (headers.Contains("code"))
            {
                Code = headers.GetValues("code").First();
            }
            if (Model != null && Model.GeneralInformation != null && !string.IsNullOrWhiteSpace(Model.GeneralInformation.CSAccountNumber) && Code == Model.GeneralInformation.CSAccountNumber)
            {
                ThirdPartyCustomerModel GeneralInformation = new ThirdPartyCustomerModel();
                GeneralInformation = Model.GeneralInformation;
                List<EmergencyContactsModel> EmergencyContacts = new List<EmergencyContactsModel>();
                EmergencyContacts = Model.EmergencyContacts;
                List<AgenciesModel> Agencies = new List<AgenciesModel>();
                Agencies = Model.Agencies;
                List<SecurityZonesModel> SecurityZones = new List<SecurityZonesModel>();
                SecurityZones = Model.SecurityZones;
                try
                {
                    var identity = (ClaimsIdentity)User.Identity;
                    if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                    {
                        var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                        if (usercontext == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                        }
                        APIInitialize();
                        Employee emp = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                        if (emp == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("User Not Found."));
                        }
                        ComId = usercontext.CompanyId.ToString();
                        Customer cus = HSapiFacade.GetCustomerById(cusId);
                        if (cus != null && cus.Id > 0)
                        {

                            #region Check Cs number used or not

                            #region Get Credential From GlobalSetting
                            var UserName = "";
                            var Password = "";
                            int SiteNumber = 0;
                            var UccUserName = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccUserName");
                            if (UccUserName != null)
                            {
                                UserName = UccUserName.Value;
                            }
                            var UccPassword = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccPassword");
                            if (UccPassword != null)
                            {
                                Password = UccPassword.Value;
                            }
                            var UccSiteGroupNumber = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccSiteGroupNumber");
                            if (UccSiteGroupNumber != null)
                            {
                                int.TryParse(UccSiteGroupNumber.Value, out SiteNumber);
                            }

                            #region Check Production or not
                            bool UccInProduction = false;
                            string UccUrl = "";
                            GlobalSetting globset2 = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccInProduction");
                            if (globset2 != null && globset2.Value.ToLower() == "true")
                            {
                                UccInProduction = true;
                            }
                            if (UccInProduction == true)
                            {
                                UccUrl = "https://dealer2.teamucc.com/SiteGroupGateway/OpenAPI/";
                            }
                            else
                            {
                                UccUrl = "https://dealer.teamucc.com/SiteGroupGatewayDev/OpenAPI/";
                            }
                            #endregion

                            #endregion

                            CustomerSystemNo sysNo = HSapiFacade.GetCusSysNoByCustomerNo(GeneralInformation.CSAccountNumber);
                            if (sysNo != null && sysNo.IsUsed == true)
                            {
                                GeneralInformation.CSAccountNumber = GetCSNumber(cus, "UCC", usercontext.CompanyId);
                            }
                            #endregion
                            List<PhoneObject> phoneList = new List<PhoneObject>();
                            string PhoneType = "";
                            if (cus.PhoneType != "-1")
                            {
                                PhoneType = cus.PhoneType;
                            }
                            else
                            {
                                PhoneType = null;
                            }
                            string SiteType = "";
                            if (cus.Type != "-1" && cus.Type != null)
                            {
                                if (cus.Type == "Residential")
                                {
                                    SiteType = "R";
                                }
                                else
                                {
                                    SiteType = "C";
                                }
                            }
                            phoneList.Add(new PhoneObject()
                            {
                                PhoneNumber = cus.PrimaryPhone,
                                Extension = null
                            });
                            List<CodewordsObject> codeWordList = new List<CodewordsObject>();
                            codeWordList.Add(new CodewordsObject()
                            {
                                Codeword = GeneralInformation.AbortCode
                            });
                            List<SiteAgencies> UccAgencyList = new List<SiteAgencies>();
                            #region Add Site Agencies
                            if (Agencies != null && Agencies.Count() > 0)
                            {
                                UccAgencyList.AddRange(Agencies.Select(x => new SiteAgencies()
                                {
                                    AgencyNum = !string.IsNullOrWhiteSpace(x.Number) ? Convert.ToInt32(x.Number) : 0,
                                    AgencyName = x.Name,
                                    AgencyPhone = x.Phone
                                }));
                            }
                            #endregion

                            #region Insert Site Contact
                            List<EmergencyContact> Contacts = new List<EmergencyContact>();

                            List<ContactObject> contactsList = new List<ContactObject>();


                            if (EmergencyContacts != null && EmergencyContacts.Count() > 0)
                            {
                                foreach (var item in EmergencyContacts)
                                {
                                    ContactObject emgContact = new ContactObject();
                                    emgContact.FirstName = item.FirstName;
                                    emgContact.LastName = item.LastName;
                                    Phone phone = new Phone();
                                    if (!string.IsNullOrWhiteSpace(item.Phone))
                                    {
                                        phone.PhoneNumber = item.Phone;
                                        List<Phone> phoneNoList = new List<Phone>();
                                        phoneNoList.Add(phone);
                                        emgContact.Phones = phoneNoList;
                                    }
                                    contactsList.Add(emgContact);
                                    Contacts.Add(new EmergencyContact()
                                    {
                                        FirstName = item.FirstName,
                                        LastName = item.LastName,
                                        Phone = item.Phone,
                                        PhoneType = "F",
                                        CustomerId = cus.CustomerId,
                                        CompanyId = usercontext.CompanyId
                                    });
                                }
                            }
                            #endregion

                            List<Device> deviceList = new List<Device>();
                            List<CustomerSecurityZones> securityZonesList = new List<CustomerSecurityZones>();
                            securityZonesList = HSapiFacade.GetAllCustomerSecurityZoneByCustomerId(cus.CustomerId, "'UCC'");
                            List<UccZone> uccZoneList = new List<UccZone>();
                            if (!string.IsNullOrWhiteSpace(GeneralInformation.UccDeviceType))
                            {
                                List<string> DeviceTypeList = GeneralInformation.UccDeviceType.Split(',').ToList();
                                foreach (var item in DeviceTypeList)
                                {
                                    if (securityZonesList != null && securityZonesList.Count > 0)
                                    {
                                        foreach (var zone in securityZonesList)
                                        {
                                            if (zone.ZoneNumber != null && zone.EventCode != "-1" && zone.Location != "")
                                            {
                                                uccZoneList.Add(new UccZone()
                                                {
                                                    Point = zone.ZoneNumber.ToString(),
                                                    EventCode = zone.EventCode,
                                                    Description = zone.Location,

                                                });

                                            }
                                        }

                                    }
                                    if (!string.IsNullOrWhiteSpace(item))
                                    {
                                        deviceList.Add(new Device()
                                        {
                                            DeviceType = item.Trim(),
                                            TransmitterCode = GeneralInformation.CSAccountNumber,
                                            Points = uccZoneList,
                                            InServiceFlag = true,
                                            PanelPhone = GeneralInformation.PanelPhone,
                                            ReceiverPhone = GeneralInformation.ReceiverPhone
                                        });
                                    }
                                }
                            }
                            else
                            {
                                deviceList.Add(new Device()
                                {
                                    DeviceType = "",
                                    Points = uccZoneList,
                                    TransmitterCode = GeneralInformation.CSAccountNumber,
                                    PanelPhone = GeneralInformation.PanelPhone,
                                    ReceiverPhone = GeneralInformation.ReceiverPhone
                                });
                            }

                            List<DispatchTypesObject> dispatchTypesList = new List<DispatchTypesObject>();
                            if (!string.IsNullOrWhiteSpace(GeneralInformation.UccDispatchType))
                            {
                                List<string> DispatchTypesList = GeneralInformation.UccDispatchType.Split(',').ToList();
                                foreach (var item in DispatchTypesList)
                                {
                                    if (!string.IsNullOrWhiteSpace(item))
                                    {
                                        dispatchTypesList.Add(new DispatchTypesObject()
                                        {
                                            DispatchType = item
                                        });
                                    }
                                }
                            }

                            List<SiteGroup> siteGroupList = new List<SiteGroup>();
                            siteGroupList.Add(new SiteGroup()
                            {
                                SiteGroupNum = SiteNumber,
                                SiteGroupTypeOverride = null
                            });

                            var dataModel = new CreateUCCCustomer
                            {
                                UserName = UserName,
                                Password = Password,
                                Site = new UccResultList
                                {
                                    Phones = phoneList,
                                    Codewords = codeWordList,
                                    SiteInstructions = null,
                                    SiteAgencies = UccAgencyList,
                                    Contacts = contactsList,
                                    Devices = deviceList,
                                    DispatchTypes = dispatchTypesList,

                                    SiteAddress = GeneralInformation.Address,
                                    SiteName = GeneralInformation.SiteName,
                                    SiteAddr2 = GeneralInformation.Address2,
                                    City = GeneralInformation.City,
                                    State = GeneralInformation.State,
                                    ZipCode = GeneralInformation.ZipCode,
                                    County = GeneralInformation.County,
                                    Latitude = null,
                                    Longitude = null,
                                    TimeZoneNum = 12,
                                    CrossStreet = GeneralInformation.Street,
                                    SiteType = SiteType,
                                    Region = GeneralInformation.State,
                                    Info = "",
                                    Directions = null,
                                    Pets = null,
                                    Map = null,
                                    MapPage = null,
                                    MapCoordinates = null,
                                    SiteID1 = null,
                                    SiteID2 = null,
                                    Subdivision = null,
                                    ULCode = null,
                                    SiteLanguage = null,
                                    LockBoxCode = null,
                                    LockBoxLocation = null,
                                    BillingID = null,
                                    SiteGroups = siteGroupList,
                                    UDF = null,
                                    SiteNum = 0
                                }
                            };
                            try
                            {
                                string modeldata = JsonConvert.SerializeObject(dataModel);
                                var client = new RestClient(UccUrl + "method/ImportSite");
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("postman-token", "5f1e7e08-821d-d34f-ef2e-2a2a0fa04fe9");
                                request.AddHeader("cache-control", "no-cache");
                                request.AddParameter("application/json", modeldata, ParameterType.RequestBody);
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                                IRestResponse response = client.Execute(request);
                                if (response.StatusDescription == "OK")
                                {
                                    UccErrorModel errors = new UccErrorModel();
                                    errors = Newtonsoft.Json.JsonConvert.DeserializeObject<UccErrorModel>(response.Content);
                                    if (errors.ErrorMessage == null || errors.ErrorNum == "145" || errors.ErrorMessage.Contains("Required Data is Missing") || (!string.IsNullOrEmpty(errors.ErrorMessage) && errors.ErrorMessage.ToLower().Contains("unknown error url")))
                                    {
                                        if (!string.IsNullOrWhiteSpace(cus.UCCRefId))
                                        {
                                            #region update Into ThirdpartyCustomer table
                                            ThirdPartyCustomer tcustomer = new ThirdPartyCustomer();

                                            tcustomer = HSapiFacade.GetThirdPartyCustomerByCustomerId(cus.CustomerId);
                                            if (tcustomer != null)
                                            {
                                                tcustomer.Id = tcustomer.Id;
                                                tcustomer.City = GeneralInformation.City != null ? GeneralInformation.City.ToString() : "";
                                                tcustomer.State = GeneralInformation.State != null ? GeneralInformation.State.ToString() : "";
                                                tcustomer.ZipCode = GeneralInformation.ZipCode != null ? GeneralInformation.ZipCode.ToString() : "";
                                                tcustomer.SiteName = GeneralInformation.SiteName != null ? GeneralInformation.SiteName.ToString() : "";
                                                tcustomer.ReceiverPhone = GeneralInformation.ReceiverPhone != null ? GeneralInformation.ReceiverPhone.ToString() : "";
                                                tcustomer.PanelPhone = GeneralInformation.PanelPhone != null ? GeneralInformation.PanelPhone.ToString() : "";
                                                tcustomer.PanelLocation = "";
                                                tcustomer.InstallDate = cus.InstallDate.HasValue ? cus.InstallDate.Value : DateTime.UtcNow;
                                                tcustomer.CustomerId = cus.CustomerId;
                                                tcustomer.CrossStreet = GeneralInformation.Street != null ? GeneralInformation.Street.ToString() : "";
                                                tcustomer.CountryName = GeneralInformation.County != null ? GeneralInformation.County.ToString() : "";
                                                tcustomer.CodeWord = GeneralInformation.AbortCode != null ? GeneralInformation.AbortCode.ToString() : "";
                                                tcustomer.CustomerNumber = Convert.ToInt32(GeneralInformation.CSAccountNumber);
                                                tcustomer.SiteAddress = GeneralInformation.Address != null ? GeneralInformation.Address.ToString() : "";
                                                tcustomer.eContact = cus.EcontractId;
                                                HSapiFacade.UpdateThirdPartyCustomer(tcustomer);
                                            }
                                            else
                                            {
                                                #region Insert Into ThirdpartyCustomer table
                                                tcustomer = new ThirdPartyCustomer();
                                                tcustomer.AccountOnlineDate = DateTime.Now.UTCCurrentTime();
                                                tcustomer.City = GeneralInformation.City != null ? GeneralInformation.City.ToString() : "";
                                                tcustomer.State = GeneralInformation.State != null ? GeneralInformation.State.ToString() : "";
                                                tcustomer.ZipCode = GeneralInformation.ZipCode != null ? GeneralInformation.ZipCode.ToString() : "";
                                                tcustomer.SiteName = GeneralInformation.SiteName != null ? GeneralInformation.SiteName.ToString() : "";
                                                tcustomer.ReceiverPhone = GeneralInformation.ReceiverPhone != null ? GeneralInformation.ReceiverPhone.ToString() : "";
                                                tcustomer.PanelPhone = GeneralInformation.PanelPhone != null ? GeneralInformation.PanelPhone.ToString() : "";
                                                tcustomer.PanelLocation = "";
                                                tcustomer.InstallDate = cus.InstallDate.HasValue ? cus.InstallDate.Value : DateTime.UtcNow;
                                                tcustomer.CustomerId = cus.CustomerId;
                                                tcustomer.CrossStreet = GeneralInformation.Street != null ? GeneralInformation.Street.ToString() : "";
                                                tcustomer.CountryName = GeneralInformation.County != null ? GeneralInformation.County.ToString() : "";
                                                tcustomer.CodeWord = GeneralInformation.AbortCode != null ? GeneralInformation.AbortCode.ToString() : "";
                                                tcustomer.CustomerNumber = Convert.ToInt32(GeneralInformation.CSAccountNumber);
                                                //tcustomer.DealerNumber = BrinksModel.servco_no;
                                                tcustomer.SiteAddress = GeneralInformation.Address != null ? GeneralInformation.Address.ToString() : "";
                                                tcustomer.eContact = cus.EcontractId;
                                                tcustomer.CreatedBy = emp.UserId;
                                                tcustomer.Platform = "UCC";
                                                HSapiFacade.InsertThirdPartyCustomer(tcustomer);
                                                #endregion
                                            }

                                            #endregion
                                        }
                                        else
                                        {
                                            #region Insert Into ThirdpartyCustomer table
                                            ThirdPartyCustomer tcustomer = new ThirdPartyCustomer();
                                            tcustomer.AccountOnlineDate = DateTime.Now.UTCCurrentTime();
                                            tcustomer.City = GeneralInformation.City != null ? GeneralInformation.City.ToString() : "";
                                            tcustomer.State = GeneralInformation.State != null ? GeneralInformation.State.ToString() : "";
                                            tcustomer.ZipCode = GeneralInformation.ZipCode != null ? GeneralInformation.ZipCode.ToString() : "";
                                            tcustomer.SiteName = GeneralInformation.SiteName != null ? GeneralInformation.SiteName.ToString() : "";
                                            tcustomer.ReceiverPhone = GeneralInformation.ReceiverPhone != null ? GeneralInformation.ReceiverPhone.ToString() : "";
                                            tcustomer.PanelPhone = GeneralInformation.PanelPhone != null ? GeneralInformation.PanelPhone.ToString() : "";
                                            tcustomer.PanelLocation = "";
                                            tcustomer.InstallDate = cus.InstallDate.HasValue ? cus.InstallDate.Value : DateTime.UtcNow;
                                            tcustomer.CustomerId = cus.CustomerId;
                                            tcustomer.CrossStreet = GeneralInformation.Street != null ? GeneralInformation.Street.ToString() : "";
                                            tcustomer.CountryName = GeneralInformation.County != null ? GeneralInformation.County.ToString() : "";
                                            tcustomer.CodeWord = GeneralInformation.AbortCode != null ? GeneralInformation.AbortCode.ToString() : "";
                                            tcustomer.CustomerNumber = Convert.ToInt32(GeneralInformation.CSAccountNumber);
                                            //tcustomer.DealerNumber = BrinksModel.servco_no;
                                            tcustomer.SiteAddress = GeneralInformation.Address != null ? GeneralInformation.Address.ToString() : "";
                                            tcustomer.eContact = cus.EcontractId;
                                            tcustomer.CreatedBy = emp.UserId;
                                            tcustomer.Platform = "UCC";
                                            HSapiFacade.InsertThirdPartyCustomer(tcustomer);
                                            #endregion
                                        }

                                        cus.UCCRefId = GeneralInformation.CSAccountNumber;
                                        cus.CustomerNo = GeneralInformation.CSAccountNumber;
                                        cus.Passcode = GeneralInformation.AbortCode;
                                        HSapiFacade.UpdateCustomer(cus);
                                        List<EmergencyContact> EmContactList = HSapiFacade.GetAllEmergencyContactByCustomerId(cus.CustomerId);
                                        if (EmContactList != null)
                                        {
                                            foreach (var item in EmContactList)
                                            {
                                                HSapiFacade.DeleteEmergencyContactById(item.Id);
                                            }
                                        }
                                        if (Contacts != null)
                                        {
                                            foreach (var item in Contacts)
                                            {
                                                HSapiFacade.InsertEmergencyContact(item);
                                            }
                                        }
                                        CustomerSystemNo cusno = new CustomerSystemNo();
                                        cusno = HSapiFacade.GetCusSysNoByCustomerNo(cus.UCCRefId);
                                        if (cusno != null)
                                        {
                                            cusno.IsUsed = true;
                                            cusno.CustomerId = cus.Id;
                                            cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                                            HSapiFacade.UpdateCustomerSystemNo(cusno);
                                        }

                                        #region Completed for ticket
                                        try
                                        {
                                            #region Check do tickt completed or not 
                                            bool isticketcompletedfunction = false;
                                            GlobalSetting ticketcompletedForUCC = HSapiFacade.GetGlobalSettingsByOnlyKey("TicketCompletedForUCC");

                                            if (ticketcompletedForUCC != null && ticketcompletedForUCC.Value.ToLower() == "true")
                                            {
                                                isticketcompletedfunction = true;
                                            }
                                            else
                                            {
                                                isticketcompletedfunction = false;
                                            }
                                            #endregion

                                            if (isticketcompletedfunction)
                                            {
                                                Ticket _tick = HSapiFacade.GetInstallationTicketByCustomerId(cus.CustomerId);
                                                if (_tick != null)
                                                {
                                                    CalculatePayrollBrinks(_tick, emp.UserId);
                                                }
                                                List<CustomerAppointmentEquipment> TicketItemList = new List<CustomerAppointmentEquipment>();
                                                #region Check Default Billing Tax
                                                bool defaultBillTaxVal = true;
                                                GlobalSetting defaultBillTax = HSapiFacade.GetGlobalSettingsByOnlyKey("DefaultCustomerBillingTax");
                                                if (defaultBillTax != null)
                                                {
                                                    if (defaultBillTax.Value.ToLower() == "true")
                                                    {
                                                        defaultBillTaxVal = true;
                                                    }
                                                    else
                                                    {
                                                        defaultBillTaxVal = false;
                                                    }
                                                }
                                                #endregion
                                                if (_tick != null && _tick.Status != LabelHelper.TicketStatus.Completed)
                                                {
                                                    Customer CustomerDetails = HSapiFacade.GetCustomerByCustomerId(_tick.CustomerId);

                                                    if (CustomerDetails != null)
                                                    {
                                                        if (_tick.CompletedDate != null)
                                                        {
                                                            CustomerDetails.InstallDate = _tick.CompletedDate.Value;
                                                        }

                                                    }

                                                    var objticketlist = HSapiFacade.GetTicketListByCustomerIdAndCompanyIdAndNotCompleted(_tick.CustomerId, usercontext.CompanyId);
                                                    if (objticketlist != null && objticketlist.Count > 0)
                                                    {
                                                        foreach (var item in objticketlist)
                                                        {
                                                            var objappeqplist = HSapiFacade.GetAllCustomerAppointmentEquipmentByAppointmentIdAndIsBilling(item.TicketId);
                                                            if (objappeqplist != null && objappeqplist.Count > 0)
                                                            {
                                                                foreach (var app in objappeqplist)
                                                                {
                                                                    app.IsBilling = false;
                                                                    HSapiFacade.UpdateCustomerAppoinmentEquipment(app);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    var totalbillamount = 0.0;
                                                    var objeqpappoint = HSapiFacade.GetCustomerAppointmentEquipmentListByAppointmentId(_tick.TicketId);
                                                    if (objeqpappoint != null && objeqpappoint.Count > 0)
                                                    {
                                                        foreach (var item in objeqpappoint)
                                                        {
                                                            if (item.IsService == true && item.IsDefaultService == false)
                                                            {
                                                                item.IsBilling = true;
                                                                HSapiFacade.UpdateCustomerAppoinmentEquipment(item);
                                                                totalbillamount += item.TotalPrice;
                                                            }
                                                        }
                                                    }
                                                    //var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(_tick.CustomerId);
                                                    if (CustomerDetails != null)
                                                    {
                                                        var totalbillamountTax = 0.0;
                                                        GlobalSetting GetSalesTax = HSapiFacade.GetSalesTax(usercontext.CompanyId, _tick.CustomerId);
                                                        if (GetSalesTax != null)
                                                        {
                                                            totalbillamountTax = Math.Round((totalbillamount * Convert.ToDouble(GetSalesTax.Value)) / 100, 2);
                                                        }
                                                        CustomerDetails.MonthlyMonitoringFee = totalbillamount.ToString("#.##");
                                                        CustomerDetails.BillAmount = defaultBillTaxVal ? totalbillamount + totalbillamountTax : totalbillamount;
                                                        CustomerDetails.TotalTax = defaultBillTaxVal ? totalbillamountTax : 0;
                                                        CustomerDetails.BillTax = defaultBillTaxVal;
                                                        //_Util.Facade.CustomerFacade.UpdateCustomer(CustomerDetails);
                                                    }

                                                    TicketItemList = HSapiFacade.GetAllCustomerAppointmentEquipmentByTicketId(usercontext.CompanyId, _tick.TicketId);
                                                    if (TicketItemList != null && TicketItemList.Count() > 0)
                                                    {
                                                        foreach (var item in TicketItemList)
                                                        {
                                                            int totalQty = 0;
                                                            int techAddQty = 0;
                                                            int techReleaseQty = 0;
                                                            bool eqpRelease = false;
                                                            CustomerAppointmentEquipment CusAptEqpListDetails = HSapiFacade.GetAppoinmentEquipmentByAppoinmentIdAndEquipmentIdAndId(_tick.TicketId, item.EquipmentId, item.Id);
                                                            TicketUser tikuser = HSapiFacade.GetTicketUserByTicketIdAndPrimary(_tick.TicketId);
                                                            List<InventoryTech> objinvtech = HSapiFacade.GetAllInventoryTechByTechnicianIdAndEquipmentIdAndType(tikuser.UserId, item.EquipmentId);
                                                            if (objinvtech != null && objinvtech.Count > 0)
                                                            {
                                                                foreach (var invtech in objinvtech)
                                                                {
                                                                    if (invtech.Type == "Add")
                                                                    {
                                                                        techAddQty += invtech.Quantity;
                                                                    }
                                                                    if (invtech.Type == "Release")
                                                                    {
                                                                        techReleaseQty += invtech.Quantity;
                                                                    }
                                                                }
                                                                totalQty = techAddQty - techReleaseQty;
                                                            }

                                                            if (totalQty > 0 && totalQty >= item.Quantity)
                                                            {
                                                                item.QuantityLeftEquipment = item.Quantity;
                                                            }
                                                            else if (totalQty > 0 && totalQty < item.Quantity)
                                                            {
                                                                item.QuantityLeftEquipment = totalQty;
                                                            }
                                                            if (CusAptEqpListDetails != null && CusAptEqpListDetails.IsEquipmentRelease == false && totalQty > 0)
                                                            {
                                                                CusAptEqpListDetails.IsEquipmentRelease = true;
                                                                CustomerDetails.Installer = CusAptEqpListDetails.TechnicianId.ToString();
                                                                CusAptEqpListDetails.QuantityLeftEquipment = item.QuantityLeftEquipment;
                                                                HSapiFacade.UpdateCustomerAppoinmentEquipment(CusAptEqpListDetails);
                                                                var delres = HSapiFacade.DeleteInventoryTechByCustomerAppointmentEquipmentIdAndType(item.Id);
                                                                InventoryTech invtech = new InventoryTech()
                                                                {
                                                                    CompanyId = usercontext.CompanyId,
                                                                    TechnicianId = item.TechnicianId,
                                                                    EquipmentId = item.EquipmentId,
                                                                    Type = LabelHelper.InventoryType.Release,
                                                                    Quantity = item.QuantityLeftEquipment.HasValue ? item.QuantityLeftEquipment.Value : 0,
                                                                    LastUpdatedBy = emp.UserId,
                                                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                                                    Description = "Release from technician by ticket",
                                                                    CustomerAppointmentEquipmentId = item.Id
                                                                };
                                                                HSapiFacade.InsertInventoryTech(invtech);

                                                            }

                                                        }
                                                    }
                                                    _tick.LastUpdatedBy = emp.UserId;
                                                    _tick.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                                    _tick.CompletedDate = DateTime.Now.UTCCurrentTime();
                                                    _tick.Status = LabelHelper.TicketStatus.Completed;
                                                    HSapiFacade.UpdateTicket(_tick);
                                                    HSapiFacade.UpdateCustomer(CustomerDetails);
                                                }
                                            }
                                            else
                                            {
                                                Ticket _tick = HSapiFacade.GetInstallationTicketByCustomerId(cus.CustomerId);
                                                if (_tick != null)
                                                {
                                                    CalculatePayrollBrinks(_tick, emp.UserId);
                                                }
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
                                        }
                                        #endregion
                                        //if (cus != null && cus.CustomerId != new Guid())
                                        //{
                                        //    AddUserActivityForCustomer("Customer #" + cus.Id + "Update to UCC by " + (emp.FirstName + " " + emp.LastName), "sync-ucc-customer", LabelHelper.ActivityAction.Updateucc, null, null, emp.FirstName + " " + emp.LastName, emp.UserId.ToString(), cus.CustomerId.ToString(), "", "");

                                        //}

                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(errors.ErrorMessage));
                                    }

                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Save Failed."));
                                }
                            }
                            catch (Exception ex)
                            {
                                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer Not Found."));
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("CS Account Number Is Required."));
            }
            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Success("Successfully Save."));
        }
        [Authorize]
        [Route("sync-ucc-customer")]
        [HttpGet]
        public HttpResponseMessage SyncUCCCustomer()
        {
            int cusId = 0;
            string Code = "";
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("customerId"))
                {
                    int.TryParse(headers.GetValues("customerId").First(), out cusId);
                }
                if (headers.Contains("code"))
                {
                    Code = headers.GetValues("code").First();
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    Employee emp = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (emp == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("User Not Found."));
                    }
                    ComId = usercontext.CompanyId.ToString();
                    Customer cus = HSapiFacade.GetCustomerById(cusId);
                    if (cus != null && cus.Id > 0 && !string.IsNullOrWhiteSpace(Code))
                    {
                        #region CheckExistingSite

                        #region Get Credential From GlobalSetting
                        var UserName = "";
                        var Password = "";
                        var UccUserName = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccUserName");
                        if (UccUserName != null)
                        {
                            UserName = UccUserName.Value;
                        }
                        var UccPassword = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccPassword");
                        if (UccPassword != null)
                        {
                            Password = UccPassword.Value;
                        }

                        #region Check Production or not
                        bool UccInProduction = false;
                        string UccUrl = "";
                        GlobalSetting globset2 = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccInProduction");
                        if (globset2 != null && globset2.Value.ToLower() == "true")
                        {
                            UccInProduction = true;
                        }
                        if (UccInProduction == true)
                        {
                            UccUrl = "https://dealer2.teamucc.com/SiteGroupGateway/OpenAPI/";
                        }
                        else
                        {
                            UccUrl = "https://dealer.teamucc.com/SiteGroupGatewayDev/OpenAPI/";
                        }
                        #endregion

                        #endregion

                        var client = new RestClient(UccUrl + "method/GetSite");
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("postman-token", "5f1e7e08-821d-d34f-ef2e-2a2a0fa04fe9");
                        request.AddHeader("cache-control", "no-cache");
                        request.AddParameter("application/json", "{\n\t\"UserName\":\"" + UserName + "\",\n\t\"Password\":\"" + Password + "\",\n\t\"TransmitterCode\":\"" + Code + "\"\n\t\n}", ParameterType.RequestBody);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


                        if (!string.IsNullOrWhiteSpace(cus.UCCRefId))
                        {
                            Customer ExistUccCustomer = HSapiFacade.IsCustomerUccExistCheck(cus.UCCRefId).FirstOrDefault();
                            #endregion
                            if (ExistUccCustomer == null)
                            {
                                try
                                {
                                    ResultUcc model = new ResultUcc();
                                    IRestResponse response = client.Execute(request);
                                    model = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultUcc>(response.Content);
                                    if (model.Result != null)
                                    {
                                        cus.UCCRefId = Code;
                                        cus.CustomerNo = Code;
                                        HSapiFacade.UpdateCustomer(cus);
                                        #region Update Customer System No
                                        CustomerSystemNo cusno = new CustomerSystemNo();
                                        cusno = HSapiFacade.GetCusSysNoByCustomerNo(cus.UCCRefId);
                                        if (cusno != null)
                                        {
                                            cusno.IsUsed = true;
                                            cusno.CustomerId = cus.Id;
                                            cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                                            HSapiFacade.UpdateCustomerSystemNo(cusno);
                                        }
                                        #endregion
                                        //if (cus != null && cus.CustomerId != new Guid())
                                        //{
                                        //    AddUserActivityForCustomer("Customer" + "#" + cus.Id + " synced successfully to UCC by" + (cus.FirstName + " " + cus.LastName), "sync-ucc-customer", LabelHelper.ActivityAction.Updateucc, null, null, emp.FirstName + " " + emp.LastName, emp.UserId.ToString(), cus.CustomerId.ToString(), "", "");
                                        //}
                                    }
                                    else
                                    {
                                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("No Record Found."));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
                                }
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("This Ucc id already synced with " + ExistUccCustomer.FirstName + " " + ExistUccCustomer.LastName + "(Id:" + ExistUccCustomer.Id + ")"));
                            }
                        }
                        else
                        {
                            try
                            {
                                ResultUcc model = new ResultUcc();
                                IRestResponse response = client.Execute(request);
                                model = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultUcc>(response.Content);
                                if (model.Result != null)
                                {
                                    cus.UCCRefId = Code;
                                    cus.CustomerNo = Code;
                                    HSapiFacade.UpdateCustomer(cus);
                                    #region Update Customer System No
                                    CustomerSystemNo cusno = new CustomerSystemNo();
                                    cusno = HSapiFacade.GetCusSysNoByCustomerNo(cus.UCCRefId);
                                    if (cusno != null)
                                    {
                                        cusno.IsUsed = true;
                                        cusno.CustomerId = cus.Id;
                                        cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                                        HSapiFacade.UpdateCustomerSystemNo(cusno);
                                    }
                                    #endregion
                                    //if (cus != null && cus.CustomerId != new Guid())
                                    //{
                                    //    AddUserActivityForCustomer("Customer" + "#" + cus.Id + " synced successfully to UCC by" + (cus.FirstName + " " + cus.LastName), "sync-ucc-customer", LabelHelper.ActivityAction.Updateucc, null, null, emp.FirstName + " " + emp.LastName, emp.UserId.ToString(), cus.CustomerId.ToString(), "", "");
                                    //}
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("No Record Found."));
                                }
                            }
                            catch (Exception ex)
                            {
                                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
                            }
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer Not Found."));
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Success("Customer Sync Successfully."));
        }

        [Authorize]
        [Route("history-ucc-customer")]
        [HttpGet]
        public HttpResponseMessage UCCCustomerHistory()
        {
            int cusId = 0;
            string Code = "";
            DateTime startdate = new DateTime();
            DateTime enddate = new DateTime();
            UccCustomerHistory model = new UccCustomerHistory();
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("customerId"))
                {
                    int.TryParse(headers.GetValues("customerId").First(), out cusId);
                }
                if (headers.Contains("code"))
                {
                    Code = headers.GetValues("code").First();
                }
                if (headers.Contains("startdate"))
                {
                    DateTime.TryParse(headers.GetValues("startdate").First(), out startdate);
                }
                if (headers.Contains("enddate"))
                {
                    DateTime.TryParse(headers.GetValues("enddate").First(), out enddate);
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();
                    Customer cus = HSapiFacade.GetCustomerById(cusId);
                    if (cus != null && cus.Id > 0 && !string.IsNullOrWhiteSpace(Code))
                    {

                        #region Get Credential From GlobalSetting
                        var UserName = "";
                        var Password = "";
                        var UccUserName = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccUserName");
                        if (UccUserName != null)
                        {
                            UserName = UccUserName.Value;
                        }
                        var UccPassword = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccPassword");
                        if (UccPassword != null)
                        {
                            Password = UccPassword.Value;
                        }

                        #region Check Production or not
                        bool UccInProduction = false;
                        string UccUrl = "";
                        GlobalSetting globset2 = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "UccInProduction");
                        if (globset2 != null && globset2.Value.ToLower() == "true")
                        {
                            UccInProduction = true;
                        }
                        if (UccInProduction == true)
                        {
                            UccUrl = "https://dealer2.teamucc.com/SiteGroupGateway/OpenAPI/";
                        }
                        else
                        {
                            UccUrl = "https://dealer.teamucc.com/SiteGroupGatewayDev/OpenAPI/";
                        }
                        #endregion

                        #endregion

                        var client = new RestClient(UccUrl + "method/GetHistory");
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("postman-token", "5f1e7e08-821d-d34f-ef2e-2a2a0fa04fe9");
                        request.AddHeader("cache-control", "no-cache");
                        request.AddParameter("application/json", "{\r\n  \"UserName\":\"" + UserName + "\",\r\n  \"Password\":\"" + Password + "\",\r\n  \"TransmitterCode\":\"" + Code + "\",\r\n  \"StartDate\":\"" + startdate.SetZeroHour().ToString("s") + "\",\r\n  \"EndDate\":\"" + enddate.SetMaxHour().ToString("s") + "\"\r\n\r\n}", ParameterType.RequestBody);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        try
                        {
                            IRestResponse response = client.Execute(request);
                            model = Newtonsoft.Json.JsonConvert.DeserializeObject<UccCustomerHistory>(response.Content);
                        }
                        catch (Exception ex)
                        {
                            return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer Not Found."));
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<UccCustomerHistory>.Success(model));
        }

        [Authorize]
        [Route("unassociate-customer")]
        [HttpGet]
        public HttpResponseMessage RemoveUnassociateCus()
        {
            int cusId = 0;
            string Platform = "";
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("customerId"))
                {
                    int.TryParse(headers.GetValues("customerId").First(), out cusId);
                }
                if (headers.Contains("platform"))
                {
                    Platform = headers.GetValues("platform").First();
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    ComId = usercontext.CompanyId.ToString();
                    APIInitialize();
                    Employee emp = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (emp == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("User Not Found."));
                    }
                    Customer cus = HSapiFacade.GetCustomerById(cusId);
                    if (cus != null && cus.Id > 0 && !string.IsNullOrWhiteSpace(Platform))
                    {
                        CustomerExtended cusEx = HSapiFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                        if (Platform == "Alarm")
                        {

                            cus.AlarmRefId = "";
                            cus.CustomerNo = "";
                            List<AlarmCustomerSelectedAddon> OldAddon = new List<AlarmCustomerSelectedAddon>();
                            OldAddon = HSapiFacade.GetAllCutomerAlarmAddonsByCustomerId(cus.CustomerId);
                            if (OldAddon != null && OldAddon.Count > 0)
                            {
                                foreach (var item in OldAddon)
                                {
                                    HSapiFacade.DeleteCutomerAlarmAddons(item.Id);
                                }
                            }
                            if (cusEx != null)
                            {
                                cusEx.AlarmBasicPackage = "";
                                HSapiFacade.UpdateCustomerExtended(cusEx);
                            }
                            SetupAlarm alarm = HSapiFacade.GetSetupalarmByCustomerId(cus.CustomerId);
                            if (alarm != null)
                            {
                                HSapiFacade.DeleteSetupAlarm(alarm.Id);
                            }
                        }
                        else if (Platform == "Brinks")
                        {
                            cus.BrinksRefId = "";
                            cus.CustomerNo = "";
                            cus.AccountNo = "";
                        }
                        else if (Platform == "UCC")
                        {
                            cus.UCCRefId = "";
                            cus.CustomerNo = "";
                            cus.AccountNo = "";
                        }
                        else if (Platform == "NMC")
                        {
                            cusEx.NMCRefId = "";
                            cus.CustomerNo = "";
                            cus.AccountNo = "";
                            HSapiFacade.UpdateCustomerExtended(cusEx);
                        }
                        else if (Platform == "agmonitoring")
                        {
                            cusEx.AvantgradRefId = "";
                            cus.CustomerNo = "";
                            cus.AccountNo = "";
                            HSapiFacade.UpdateCustomerExtended(cusEx);
                        }
                        HSapiFacade.UpdateCustomer(cus);
                        //if (cus != null && cus.CustomerId != new Guid())
                        //{
                        //    AddUserActivityForCustomer("Unassociate Customer" + "#" + cus.Id + " removed by" + (emp.FirstName + " " + emp.LastName), "unassociate-customer", LabelHelper.ActivityAction.Terminate, null, null, emp.FirstName + " " + emp.LastName, emp.UserId.ToString(), cus.CustomerId.ToString(), "", "");

                        //}
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer not removed."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.PreconditionFailed, ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Success("Unassociate Customer removed successfully"));
        }

        public string GetCSNumber(Customer cus, string Platform, Guid CompanyId)
        {
            string CSNumber = "";
            #region Get CSNumber
            if (!string.IsNullOrEmpty(cus.CustomerNo))
            {
                CSNumber = cus.CustomerNo;
            }
            else
            {
                PackageCustomer packageCustomer = HSapiFacade.GetPackageCustomerByCustomerId(cus.CustomerId);
                if (packageCustomer != null)
                {
                    SmartPackage package = HSapiFacade.GetPackageByPackageIdAndCompanyId(packageCustomer.PackageId, CompanyId);
                    if (package != null)
                    {
                        CustomerSystemNo custNumber = HSapiFacade.GetAllOpenCustomerSystemNoByCompanyIdandPlatformPrifix(CompanyId, package.CustomerNumber).FirstOrDefault();
                        if (custNumber != null)
                        {
                            CSNumber = custNumber.CustomerNo;
                        }
                    }
                }
                if (string.IsNullOrEmpty(CSNumber))
                {

                    CustomerNoPrefix cusPrifix = HSapiFacade.GetAllNumberPrefixByCentralstationName(CompanyId, Platform);

                    List<CustomerSystemNo> customerSystemNoList = new List<CustomerSystemNo>();
                    if (cusPrifix != null)
                    {
                        customerSystemNoList = HSapiFacade.GetAllOpenCustomerSystemNoByCompanyIdandPlatformPrifix(CompanyId, cusPrifix.Name);

                        if (customerSystemNoList != null && customerSystemNoList.Count > 0)
                        {
                            CustomerSystemNo cusSysNo = customerSystemNoList.FirstOrDefault();
                            CSNumber = cusSysNo.CustomerNo;
                        }
                    }
                }
            }

            #endregion
            return CSNumber;
        }

        #region Calculate Payroll
        public void CalculatePayrollBrinks(Ticket ticket, Guid UserId)
        {

            if (ticket != null && ticket.TicketId != Guid.Empty && ticket.CustomerId != Guid.Empty)
            {
                var payrollBrinksDetail = HSapiFacade.GetPayrollBrinksByTicketId(ticket.TicketId);
                var CustomerDetails = HSapiFacade.GetCustomerByCustomerId(ticket.CustomerId);
                if (CustomerDetails != null)
                {
                    var SalesPayCalculation = HSapiFacade.GetSalesPayCalculationByTicketId(ticket.TicketId);
                    if (SalesPayCalculation != null)
                    {
                        var MonthlyMonitoringFee = 0.0;
                        double.TryParse(CustomerDetails.MonthlyMonitoringFee, out MonthlyMonitoringFee);
                        if (payrollBrinksDetail != null)
                        {
                            payrollBrinksDetail.SalesPersonId = CustomerDetails.Soldby1;
                            payrollBrinksDetail.MMR = MonthlyMonitoringFee;
                            payrollBrinksDetail.Multiple = SalesPayCalculation.TotalMultiple;
                            payrollBrinksDetail.GrossPay = MonthlyMonitoringFee * SalesPayCalculation.TotalMultiple;
                            payrollBrinksDetail.Deductions = SalesPayCalculation.Deductions;
                            payrollBrinksDetail.Adjustments = 0;
                            payrollBrinksDetail.NetPay = MonthlyMonitoringFee * SalesPayCalculation.TotalMultiple - SalesPayCalculation.Deductions;
                            payrollBrinksDetail.LastUpdateBy = UserId;
                            payrollBrinksDetail.LastUpdateDate = DateTime.Now.UTCCurrentTime();

                            HSapiFacade.UpdatePayrollBrinks(payrollBrinksDetail);
                        }
                        else
                        {
                            PayrollBrinks model = new PayrollBrinks()
                            {
                                PayrollBrinksId = Guid.NewGuid(),
                                CustomerId = CustomerDetails.CustomerId,
                                SalesPersonId = CustomerDetails.Soldby1,
                                TicketId = ticket.TicketId,
                                MMR = MonthlyMonitoringFee,
                                Multiple = SalesPayCalculation.TotalMultiple,
                                GrossPay = MonthlyMonitoringFee * SalesPayCalculation.TotalMultiple,
                                Deductions = SalesPayCalculation.Deductions,
                                Adjustments = 0,
                                NetPay = MonthlyMonitoringFee * SalesPayCalculation.TotalMultiple - SalesPayCalculation.Deductions,
                                CreatedBy = UserId,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                LastUpdateBy = UserId,
                                LastUpdateDate = DateTime.Now.UTCCurrentTime(),
                                FundingStatus = "Pending"
                            };
                            HSapiFacade.InsertPayrollBrinks(model);
                        }
                    }
                }
            }
        }
        #endregion

        #endregion

        #region Alarm API
        [Authorize]
        [Route("sync-alarm-api-customer")]
        [HttpGet]
        public HttpResponseMessage SyncAlarmCustomer()
        {

            int cusId = 0;
            int AlarmId = 0;
            string Code = string.Empty;
            string message = "";
            bool result = false;
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("customerId"))
                {
                    int.TryParse(headers.GetValues("customerId").First(), out cusId);
                }
                if (headers.Contains("code"))
                {
                    Code = headers.GetValues("code").First();
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    Employee emp = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (emp == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("User Not Found."));
                    }
                    ComId = usercontext.CompanyId.ToString();
                    Customer cus = HSapiFacade.GetCustomerById(cusId);
                    if (cus != null && cus.Id > 0 && !string.IsNullOrWhiteSpace(Code) && int.TryParse(Code, out AlarmId))
                    {
                        Customer ExistAlarmIdCustomer = HSapiFacade.IsCustomerAlarmIdExistCheck(AlarmId).FirstOrDefault();
                        CustomerExtended extended = new CustomerExtended();

                        extended = HSapiFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                        if (ExistAlarmIdCustomer == null)
                        {
                            Alarm.AlarmCustomer.CustomerManagement response = GetAlarmCustomerRespnse(cus, usercontext.CompanyId);
                            Alarm.AlarmCustomer.CustomerInfo cusresponse = new HS.Alarm.AlarmCustomer.CustomerInfo();
                            Alarm.AlarmCustomer.CustomerBestPracticesOutput practiceResponse = new Alarm.AlarmCustomer.CustomerBestPracticesOutput();
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            try
                            {
                                cusresponse = response.GetCustomerInfo(AlarmId);
                                result = true;
                                cus.AlarmRefId = AlarmId.ToString();
                                HSapiFacade.UpdateCustomer(cus);
                                message = "Customer synced successfully";
                                //   AddUserActivityForCustomer("Customer" + "#" + cus.Id + " synced successfully to Alarm by" + (emp.FirstName + " " + emp.LastName), LabelHelper.ActivityAction.AddAlarm_com, cus.CustomerId, null, null);
                                #region Update Customer System No

                                CustomerSystemNo cusno = new CustomerSystemNo();
                                if (cusresponse.CentralStationInfo != null && !string.IsNullOrEmpty(cusresponse.CentralStationInfo.AccountNumber))
                                {
                                    cusno = HSapiFacade.GetCusSysNoByCustomerNo(cusresponse.CentralStationInfo.AccountNumber);
                                }

                                if (cusno != null)
                                {
                                    cusno.IsUsed = true;
                                    cusno.IsReserved = true;
                                    cusno.CustomerId = cus.Id;
                                    cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                                    HSapiFacade.UpdateCustomerSystemNo(cusno);
                                }
                                #endregion
                                #region Update customer Packages
                                try
                                {
                                    int AutomationCount = 0;
                                    int videoCount = 0;
                                    int skybellCount = 0;
                                    int Svrcount = 0;
                                    AlarmCustomerSelectedAddon addon = new AlarmCustomerSelectedAddon();

                                    List<AlarmCustomerSelectedAddon> OldAddon = new List<AlarmCustomerSelectedAddon>();
                                    OldAddon = HSapiFacade.GetAllCutomerAlarmAddonsByCustomerId(cus.CustomerId);
                                    if (OldAddon != null && OldAddon.Count > 0)
                                    {
                                        foreach (var item in OldAddon)
                                        {
                                            HSapiFacade.DeleteCutomerAlarmAddons(item.Id);
                                        }
                                    }
                                    if (extended != null)
                                    {
                                        extended.AlarmBasicPackage = "";
                                        HSapiFacade.UpdateCustomerExtended(extended);
                                    }
                                    if (cusresponse.ServicePlanInfo.Addons != null && cusresponse.ServicePlanInfo.Addons.Count() > 0)
                                    {
                                        foreach (var item in cusresponse.ServicePlanInfo.Addons)
                                        {
                                            AlarmAddOnns addons = HSapiFacade.GeAddOnnsByName(item.ToString());
                                            if (addons.Value == 36 || addons.Value == 41 || addons.Value == 2 || addons.Value == 5 || addons.Value == 23 || addons.Value == 21 || addons.Value == 19 || addons.Value == 20 || addons.Value == 52)
                                            {
                                                AutomationCount++;
                                            }
                                            else if (addons.Value == 76 || addons.Value == 70)
                                            {
                                                skybellCount++;
                                            }
                                            else if (addons.Value == 12 || addons.Value == 13)
                                            {
                                                videoCount++;
                                            }
                                            else if (addons.Value == 43)
                                            {
                                                Svrcount++;
                                            }

                                        }

                                        if (AutomationCount == 1)
                                        {
                                            addon = new AlarmCustomerSelectedAddon();

                                            addon.CustomerId = cus.CustomerId;
                                            addon.AddonType = "Automation1";

                                            HSapiFacade.InsertCutomerAlarmAddons(addon);
                                        }
                                        if (AutomationCount == 2)
                                        {

                                            addon = new AlarmCustomerSelectedAddon();

                                            addon.CustomerId = cus.CustomerId;
                                            addon.AddonType = "Automation2";

                                            HSapiFacade.InsertCutomerAlarmAddons(addon);
                                        }
                                        if (AutomationCount == 3)
                                        {
                                            addon = new AlarmCustomerSelectedAddon();

                                            addon.CustomerId = cus.CustomerId;
                                            addon.AddonType = "Automation3";

                                            HSapiFacade.InsertCutomerAlarmAddons(addon);
                                        }
                                        if (AutomationCount > 3)
                                        {
                                            addon = new AlarmCustomerSelectedAddon();

                                            addon.CustomerId = cus.CustomerId;
                                            addon.AddonType = "Automation4";
                                            HSapiFacade.InsertCutomerAlarmAddons(addon);
                                        }
                                        if (Svrcount > 0)
                                        {
                                            addon = new AlarmCustomerSelectedAddon();

                                            addon.CustomerId = cus.CustomerId;
                                            addon.AddonType = "SVR";
                                            HSapiFacade.InsertCutomerAlarmAddons(addon);
                                        }
                                        if (skybellCount > 0 && videoCount > 0)
                                        {
                                            addon = new AlarmCustomerSelectedAddon();

                                            addon.CustomerId = cus.CustomerId;
                                            addon.AddonType = "ProVideo";
                                            HSapiFacade.InsertCutomerAlarmAddons(addon);
                                        }
                                        else
                                        {
                                            if (skybellCount > 0)
                                            {
                                                addon = new AlarmCustomerSelectedAddon();

                                                addon.CustomerId = cus.CustomerId;
                                                addon.AddonType = "SkyBell";
                                                HSapiFacade.InsertCutomerAlarmAddons(addon);
                                            }
                                            else if (videoCount > 0)
                                            {

                                                addon = new AlarmCustomerSelectedAddon();

                                                addon.CustomerId = cus.CustomerId;
                                                addon.AddonType = "ProVideo";
                                                HSapiFacade.InsertCutomerAlarmAddons(addon);
                                            }
                                        }

                                    }

                                    if (cusresponse.ServicePlanInfo.PackageId > 0)
                                    {
                                        AlarmCustomerSelectedAddon basicaddon = new AlarmCustomerSelectedAddon();
                                        if (cusresponse.ServicePlanInfo.PackageId == 208 || cusresponse.ServicePlanInfo.PackageId == 209 || cusresponse.ServicePlanInfo.PackageId == 193)
                                        {

                                            if (extended != null)
                                            {
                                                extended.AlarmBasicPackage = "Interactive";
                                                HSapiFacade.UpdateCustomerExtended(extended);
                                            }
                                            else
                                            {
                                                extended.CustomerId = cus.CustomerId;
                                                extended.AlarmBasicPackage = "Interactive";
                                                HSapiFacade.InsertCustomerExtended(extended);
                                            }
                                        }
                                    }


                                }
                                catch (Exception ex)
                                {
                                    result = false;
                                    message = ex.Message;
                                }
                                #endregion
                                #region Delete previous Security Zones
                                try
                                {

                                    List<Alarm.AlarmCustomer.PanelDevice> divicelist = new List<HS.Alarm.AlarmCustomer.PanelDevice>();
                                    divicelist = GetAlarmSystemInfoList(usercontext.CompanyId, cus);
                                    List<CustomerSecurityZones> securityZoneList = HSapiFacade.GetAllCustomerSecurityZoneByCustomerId(cus.CustomerId, "'Brinks'");
                                    if (securityZoneList != null && securityZoneList.Count > 0)
                                    {
                                        foreach (var item in securityZoneList)
                                        {
                                            HSapiFacade.DeleteCustomerSecurityZone(item.ID);
                                        }
                                    }
                                    CustomerSecurityZones securityZones = new CustomerSecurityZones();
                                    if (divicelist != null && divicelist.Count > 0)
                                    {
                                        foreach (var item in divicelist)
                                        {
                                            securityZones = new CustomerSecurityZones()
                                            {
                                                CustomerId = cus.CustomerId,
                                                ZoneNumber = item.DeviceId.ToString(),
                                                ZoneComment = item.WebSiteDeviceName,
                                                Platform = "Brinks"
                                            };
                                            HSapiFacade.InsertCustomerSecurityZone(securityZones);
                                        }
                                    }


                                }
                                catch (Exception ex)
                                {
                                    result = false;
                                    message = ex.Message;
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                message = ex.Message;
                            }
                        }
                        else
                        {
                            result = false;
                            message = "This alarm id already synced with " + ExistAlarmIdCustomer.FirstName + " " + ExistAlarmIdCustomer.LastName + "(Id:" + ExistAlarmIdCustomer.Id + ")";
                        }

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer Not Found."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("User Not Found."));
                }
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }
            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Success(message));
        }

        [Authorize]
        [Route("check-alarm-api-customer")]
        [HttpGet]
        public HttpResponseMessage AlarmCustomerDetails()
        {
            int cusId = 0;
            int AlarmId = 0;
            Alarm.AlarmCustomer.CustomerInfo result = new HS.Alarm.AlarmCustomer.CustomerInfo();
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("customerId"))
                {
                    int.TryParse(headers.GetValues("customerId").First(), out cusId);
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    ComId = usercontext.CompanyId.ToString();
                    Customer cus = HSapiFacade.GetCustomerById(cusId);
                    if (cus != null && cus.Id > 0 && !string.IsNullOrWhiteSpace(cus.AlarmRefId) && int.TryParse(cus.AlarmRefId, out AlarmId))
                    {
                        Alarm.AlarmCustomer.CustomerManagement response = GetAlarmCustomerRespnse(cus, usercontext.CompanyId);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        //Alarm.AlarmCustomer.CustomerBestPracticesOutput bestPractice = new Alarm.AlarmCustomer.CustomerBestPracticesOutput();
                        try
                        {
                            result = response.GetCustomerInfo(AlarmId);
                            if (result != null)
                            {
                                if (result.CentralStationInfo != null)
                                {
                                    if (result.IsTerminated != true)
                                    {
                                        cus.CustomerNo = result.CentralStationInfo.AccountNumber;
                                        HSapiFacade.UpdateCustomer(cus);
                                    }

                                }
                                //bestPractice = response.GetCustomerBestPractices(AlarmId);
                            }
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<Alarm.AlarmCustomer.CustomerInfo>.Success(result));
                        }
                        catch (Exception ex)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<GlobalSetting>.Success(null));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("User Not Found."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
            }
        }
        [Authorize]
        [Route("setup-alarm-api")]
        [HttpGet]
        public HttpResponseMessage AddAlarmCustomer()
        {
            int cusId = 0;
            GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
            GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();
            SetupAlarm AlarmModel = new SetupAlarm();
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("customerId"))
                {
                    int.TryParse(headers.GetValues("customerId").First(), out cusId);
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    ComId = usercontext.CompanyId.ToString();
                    Customer cus = HSapiFacade.GetCustomerById(cusId);
                    if (cus != null && cus.Id > 0)
                    {
                        HasMultipleAlarmUser = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "HasMultipleAlarmUser");
                        if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
                        {
                            if (cus != null && cus.Ownership == "Brinks")
                            {
                                AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "BrinksAlarmUserName");
                                AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "BrinksAlarmUserPassword");
                            }
                            else
                            {
                                AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "AlarmUsername");
                                AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "AlarmPassword");
                            }
                        }
                        else
                        {
                            AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "AlarmUsername");
                            AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "AlarmPassword");
                        }
                        AlarmDealerManager ad = new AlarmDealerManager();
                        HS.Alarm.AlarmDealer.GetPackageIdsOutput Basic = ad.GetBasicPackages(AlarmUserGlobalSetting.Value, AlarmPassGlobalSetting.Value);
                        List<HS.Alarm.AlarmDealer.PackageInfo> PackageDataList = new List<HS.Alarm.AlarmDealer.PackageInfo>();
                        PackageDataList.Add(new HS.Alarm.AlarmDealer.PackageInfo()
                        {
                            PackageDescription = "Select Feature",
                            PackageId = -1
                        });
                        PackageDataList.AddRange(Basic.Packages);
                        System.Web.HttpRuntime.Cache["GetPackageInfo"] = Basic;
                        List<AddOns> addonList = new List<AddOns>();
                        var myEnumMemberCount = Enum.GetNames(typeof(HS.Alarm.AlarmDealer.AddOnFeatureEnum)).Length;
                        AlarmAddOnns addons = new AlarmAddOnns();
                        List<AlarmAddOnns> addonsList = HSapiFacade.GetAllAddOnns();
                        if (addonsList.Count == 0)
                        {
                            for (int i = 0; i < myEnumMemberCount; i++)
                            {
                                addons = new AlarmAddOnns
                                {
                                    Name = Enum.GetName(typeof(HS.Alarm.AlarmDealer.AddOnFeatureEnum), i),
                                    Value = i
                                };
                                HSapiFacade.InsertAlarmAddOnns(addons);
                            }
                            addonsList = HSapiFacade.GetAllAddOnns();
                        }
                        List<PackageDataList> eventsToForwardList = new List<PackageDataList>();
                        eventsToForwardList.Add(new PackageDataList()
                        {
                            Display = "Alarms",
                            Value = "Alarms",
                            Checked = true
                        });
                        eventsToForwardList.Add(new PackageDataList()
                        {
                            Display = "Armings (Openings/Closings)",
                            Value = "Armings",
                            Checked = false
                        });
                        eventsToForwardList.Add(new PackageDataList()
                        {
                            Display = "Bypass",
                            Value = "Bypass",
                            Checked = false
                        });
                        eventsToForwardList.Add(new PackageDataList()
                        {
                            Display = "Cancels",
                            Value = "Cancels",
                            Checked = true
                        });
                        eventsToForwardList.Add(new PackageDataList()
                        {
                            Display = "Crash & Smash",
                            Value = "CrashAndSmash",
                            Checked = true
                        });
                        eventsToForwardList.Add(new PackageDataList()
                        {
                            Display = "Panel Not Responding",
                            Value = "PanelNotResponding",
                            Checked = true
                        });
                        eventsToForwardList.Add(new PackageDataList()
                        {
                            Display = "Panics",
                            Value = "Panics",
                            Checked = true
                        });
                        eventsToForwardList.Add(new PackageDataList()
                        {
                            Display = "Phone Communication Failures",
                            Value = "PhoneCommFailure",
                            Checked = true
                        });
                        eventsToForwardList.Add(new PackageDataList()
                        {
                            Display = "Phone Tests",
                            Value = "PhoneTests",
                            Checked = true
                        });
                        eventsToForwardList.Add(new PackageDataList()
                        {
                            Display = "Sensor Tampers",
                            Value = "SensorTampers",
                            Checked = true
                        });
                        eventsToForwardList.Add(new PackageDataList()
                        {
                            Display = "Trouble Restorals",
                            Value = "TroubleRestorals",
                            Checked = true
                        });
                        eventsToForwardList.Add(new PackageDataList()
                        {
                            Display = "Troubles",
                            Value = "Troubles",
                            Checked = true
                        });

                        AlarmModel = HSapiFacade.GetSetupalarmByCustomerId(cus.CustomerId);
                        if (AlarmModel == null)
                        {
                            AlarmModel = new SetupAlarm();
                            AlarmModel.CustomerStatus = LabelHelper.AlarmCustomerStatus.Init;
                            AlarmModel.EmailAddress = cus.EmailAddress;
                            AlarmModel.CentralStationAccountNo = cus.AccountNo;
                            AlarmModel.InsStreet = cus.Street;
                            AlarmModel.InsCity = cus.City;
                            AlarmModel.InsState = cus.State;
                            AlarmModel.InsZip = cus.ZipCode;
                            AlarmModel.Phone = cus.CellNo;
                            AlarmModel.CompanyName = usercontext.CompanyName;
                            AlarmModel.LoginName = cus.EmailAddress;
                            AlarmModel.CentralStationAccountNo = cus.CustomerNo;
                            AlarmModel.CustomerType = cus.Type;
                        }
                        #region ModelAlways filldup from customer
                        AlarmModel.Action = "CreateCustomer";
                        AlarmModel.FirstName = cus.FirstName;
                        AlarmModel.LastName = cus.LastName;
                        AlarmModel.Street = cus.Street;
                        AlarmModel.City = cus.City;
                        AlarmModel.State = cus.State;
                        AlarmModel.Zip = cus.ZipCode;
                        AlarmModel.AlarmRefId = cus.AlarmRefId;
                        AlarmModel.CompanyName = usercontext.CompanyName;
                        AlarmModel.Phone = cus.PrimaryPhone != "" ? cus.PrimaryPhone : cus.CellNo;
                        AlarmModel.CustomerId = cus.CustomerId;
                        AlarmModel.IsContractSigned = cus.IsContractSigned;
                        #endregion
                        List<PackageDataList> basePackage = new List<PackageDataList>();
                        List<AlarmAddOnns> addOns = new List<AlarmAddOnns>();
                        List<PackageDataList> eventsToForward = new List<PackageDataList>();
                        basePackage = PackageDataList.OrderBy(x => x.PackageDescription != "Select Feature").ThenBy(x => x.PackageDescription).ToList().Select(x =>
                                      new PackageDataList()
                                      {
                                          Display = x.PackageDescription.ToString(),
                                          Value = x.PackageId.ToString()
                                      }).ToList();
                        addOns = addonsList.OrderBy(x => x.Name).ToList();
                        eventsToForward = eventsToForwardList;
                        return Request.CreateResponse(HttpStatusCode.OK, new { Model = AlarmModel, BasePackage = basePackage, AddOns = addOns, EventsToForward = eventsToForward });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer Not Found."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
            }
        }
        [Authorize]
        [Route("close-alarm-api-customer")]
        [HttpGet]
        public HttpResponseMessage FreeAllUnassignedCsNumber()
        {
            int cusId = 0;
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("customerId"))
                {
                    int.TryParse(headers.GetValues("customerId").First(), out cusId);
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    ComId = usercontext.CompanyId.ToString();
                    Customer cus = HSapiFacade.GetCustomerById(cusId);
                    if (cus != null && cus.Id > 0)
                    {
                        if (string.IsNullOrEmpty(cus.CustomerNo))
                        {
                            List<CustomerSystemNo> cusNoLists = HSapiFacade.GetAllReservedCustomerSystemNoByCustomerId(cus.Id);
                            foreach (var item in cusNoLists)
                            {
                                item.IsReserved = false;
                                item.CustomerId = 0;
                                HSapiFacade.UpdateCustomerSystemNo(item);
                            }
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer Not Found."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
            }
            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Success("Closed."));
        }
        [Authorize]
        [Route("alarm-api-included-features")]
        [HttpGet]
        public HttpResponseMessage GetFeatures()
        {
            int cusId = 0;
            int PackageId = 0;
            GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
            GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();
            FeaturesReturnModel AlarmModel = new FeaturesReturnModel();
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("customerId"))
                {
                    int.TryParse(headers.GetValues("customerId").First(), out cusId);
                }
                if (headers.Contains("packageId"))
                {
                    int.TryParse(headers.GetValues("packageId").First(), out PackageId);
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    ComId = usercontext.CompanyId.ToString();
                    List<string> features = new List<string>();
                    List<string> addonfeatures = new List<string>();
                    AlarmModel.Features = new List<AddOns>();
                    AlarmModel.Addons = new List<AddOns>();
                    HS.Alarm.AlarmDealer.GetPackageIdsOutput packageInfo = (HS.Alarm.AlarmDealer.GetPackageIdsOutput)System.Web.HttpRuntime.Cache["GetPackageInfo"];
                    if (packageInfo != null)
                    {
                        var result = packageInfo.Packages.Where(x => x.PackageId == PackageId).FirstOrDefault().IncludedFeatures;
                        if (result != null)
                        {
                            foreach (var item in result.Select(y => y.Feature))
                            {
                                features.Add(Enum.GetName(typeof(HS.Alarm.AlarmDealer.AddOnFeatureEnum), item));
                            }
                            AlarmModel.Features.AddRange(features.Select(x => new AddOns() { Name = x.Trim() }).ToList());
                        }
                        var adonitem = packageInfo.Packages.Where(x => x.PackageId == PackageId).FirstOrDefault().FreeAddOns;
                        if (adonitem != null)
                        {
                            foreach (var item in adonitem.Select(y => y.Feature))
                            {
                                addonfeatures.Add(Enum.GetName(typeof(HS.Alarm.AlarmDealer.AddOnFeatureEnum), item));
                            }
                            AlarmModel.Addons.AddRange(addonfeatures.Select(x => new AddOns() { Name = x.Trim() }).ToList());
                        }
                    }
                    else
                    {
                        Customer cus = HSapiFacade.GetCustomerById(cusId);
                        if (cus != null && cus.Id > 0)
                        {
                            HasMultipleAlarmUser = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "HasMultipleAlarmUser");
                            if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
                            {
                                if (cus != null && cus.Ownership == "Brinks")
                                {
                                    AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "BrinksAlarmUserName");
                                    AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "BrinksAlarmUserPassword");
                                }
                                else
                                {
                                    AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "AlarmUsername");
                                    AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "AlarmPassword");
                                }
                            }
                            else
                            {
                                AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "AlarmUsername");
                                AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "AlarmPassword");
                            }
                            AlarmDealerManager ad = new AlarmDealerManager();
                            HS.Alarm.AlarmDealer.GetPackageIdsOutput Basic = ad.GetBasicPackages(AlarmUserGlobalSetting.Value, AlarmPassGlobalSetting.Value);
                            var result = Basic.Packages.Where(x => x.PackageId == PackageId).FirstOrDefault().IncludedFeatures;
                            if (result != null)
                            {
                                foreach (var item in result.Select(y => y.Feature).ToList())
                                {
                                    features.Add(Enum.GetName(typeof(HS.Alarm.AlarmDealer.AddOnFeatureEnum), item));
                                }
                                AlarmModel.Features.AddRange(features.Select(x => new AddOns() { Name = x.Trim() }).ToList());
                            }
                            var adonitem = Basic.Packages.Where(x => x.PackageId == PackageId).FirstOrDefault().FreeAddOns;
                            if (adonitem != null)
                            {
                                foreach (var item in adonitem.Select(y => y.Feature))
                                {
                                    addonfeatures.Add(Enum.GetName(typeof(HS.Alarm.AlarmDealer.AddOnFeatureEnum), item));
                                }
                                AlarmModel.Addons.AddRange(addonfeatures.Select(x => new AddOns() { Name = x.Trim() }).ToList());
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer Not Found."));
                        }
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
            }
            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<FeaturesReturnModel>.Success(AlarmModel));
        }
        [Authorize]
        [Route("alarm-api-equipment-list")]
        [HttpGet]
        public HttpResponseMessage AlarmEquipmentList()
        {
            string cusId = "";
            int code = 0;
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("code"))
                {
                    cusId = headers.GetValues("code").First();
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    ComId = usercontext.CompanyId.ToString();
                    Customer cus = HSapiFacade.GetDirectCustomerByAlarmRefId(cusId);
                    if (cus != null && cus.Id > 0 && int.TryParse(cusId, out code))
                    {
                        Alarm.AlarmCustomer.CustomerManagement response = GetAlarmCustomerRespnse(cus, usercontext.CompanyId);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        List<Alarm.AlarmCustomer.PanelDevice> divicelist = new List<HS.Alarm.AlarmCustomer.PanelDevice>();
                        try
                        {
                            Alarm.AlarmCustomer.PanelDevice[] result = response.GetFullEquipmentList(code);
                            divicelist = result.ToList();
                        }
                        catch (Exception ex)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<List<Alarm.AlarmCustomer.PanelDevice>>.Success(divicelist));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer Not Found."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
            }
        }
        [Authorize]
        [Route("alarm-api-run-system-check")]
        [HttpGet]
        public HttpResponseMessage RunSystemCheck()
        {
            string cusId = "";
            int code = 0;
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("code"))
                {
                    cusId = headers.GetValues("code").First();
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    ComId = usercontext.CompanyId.ToString();
                    Customer cus = HSapiFacade.GetDirectCustomerByAlarmRefId(cusId);
                    if (cus != null && cus.Id > 0 && int.TryParse(cusId, out code))
                    {
                        Alarm.AlarmCustomer.CustomerManagement response = GetAlarmCustomerRespnse(cus, usercontext.CompanyId);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        try
                        {
                            response.RunSystemCheck(code, SystemCheckTestCategoryEnum.Video);
                            response.RunSystemCheck(code, SystemCheckTestCategoryEnum.ImageSensor);
                            response.RunSystemCheck(code, SystemCheckTestCategoryEnum.Panel);
                            response.RunSystemCheck(code, SystemCheckTestCategoryEnum.Zwave);
                            response.RunSystemCheck(code, SystemCheckTestCategoryEnum.Engagement);
                            response.RunSystemCheck(code, SystemCheckTestCategoryEnum.Communications);
                            response.RunSystemCheck(code, SystemCheckTestCategoryEnum.AccessControl);
                            response.RunSystemCheck(code, SystemCheckTestCategoryEnum.Sensors);
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Success("System is runing system check"));
                        }
                        catch (Exception ex)
                        {

                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
                        }

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer Not Found."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
            }
        }
        [Authorize]
        [Route("alarm-api-system-run-test")]
        [HttpGet]
        public HttpResponseMessage DoSystemRunTestByCusId()
        {
            string cusId = "";
            int code = 0;
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("code"))
                {
                    cusId = headers.GetValues("code").First();
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    ComId = usercontext.CompanyId.ToString();
                    Customer cus = HSapiFacade.GetDirectCustomerByAlarmRefId(cusId);
                    if (cus != null && cus.Id > 0 && int.TryParse(cusId, out code))
                    {
                        Alarm.AlarmCustomer.CustomerManagement response = GetAlarmCustomerRespnse(cus, usercontext.CompanyId);
                        Alarm.AlarmCustomer.GetSystemCheckResultsOutput checkResponse = new Alarm.AlarmCustomer.GetSystemCheckResultsOutput();
                        GetSystemCheckResultsInput input = new GetSystemCheckResultsInput();
                        input.CustomerId = code;
                        List<SystemCheckCategoryOutput> systemCheckList = new List<SystemCheckCategoryOutput>();
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        try
                        {
                            checkResponse = response.GetSystemCheckResults(input);
                            if (checkResponse.Success == true)
                            {
                                SystemCheckCategoryOutput[] CheckCategoryList = checkResponse.SystemCheckResults.TestCategories;
                                systemCheckList = CheckCategoryList.ToList();
                                //if (cus != null && cus.CustomerId != new Guid())
                                //{
                                //    base.AddUserActivityForCustomer("Customer" + "#" + CustomerId + "is placed on test by " + (CurrentUser.FirstName + " " + CurrentUser.LastName), LabelHelper.ActivityAction.AddAlarm_com, cus.CustomerId, null, null);

                                //}

                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(checkResponse.ErrorMessage));
                            }
                        }
                        catch (Exception ex)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<List<SystemCheckCategoryOutput>>.Success(systemCheckList));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer Not Found."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
            }
        }
        [Authorize]
        [Route("alarm-api-open-customer-termination")]
        [HttpGet]
        public HttpResponseMessage CustomerTermination()
        {
            string cusId = "";
            int code = 0;
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("code"))
                {
                    cusId = headers.GetValues("code").First();
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    Customer cus = HSapiFacade.GetDirectCustomerByAlarmRefId(cusId);
                    if (cus != null && cus.Id > 0 && int.TryParse(cusId, out code))
                    {
                        CustomerInfo cusInfo = GetAlarmCustomerInfoByAlarmId(code, cus, usercontext.CompanyId);
                        if (cusInfo == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer Not Found."));
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<CustomerInfo>.Success(cusInfo));
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer Not Found."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
            }
        }
        [Authorize]
        [Route("alarm-api-termination-history-log")]
        [HttpGet]
        public HttpResponseMessage TerminationHistoryLog()
        {
            Guid cusId = Guid.Empty;
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("customerId"))
                {
                    Guid.TryParse(headers.GetValues("customerId").First(), out cusId);
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    List<AlarmCustomerTerminationViewModel> logList = HSapiFacade.GetAlarmTerminationHistoryByCustomerId(cusId);
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<List<AlarmCustomerTerminationViewModel>>.Success(logList));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
            }
        }
        [Authorize]
        [Route("alarm-api-terminate-customer")]
        [HttpPost]
        public HttpResponseMessage TerminateCustomer()
        {
            string cusId = "";
            int code = 0;
            string Reason = "";
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("code"))
                {
                    cusId = headers.GetValues("code").First();
                }
                if (headers.Contains("reason"))
                {
                    Reason = headers.GetValues("reason").First();
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    Employee emp = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (emp == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("User Not Found."));
                    }
                    Customer cus = HSapiFacade.GetDirectCustomerByAlarmRefId(cusId);
                    if (cus != null && cus.Id > 0 && int.TryParse(cusId, out code))
                    {
                        Alarm.AlarmCustomer.CustomerManagement response = GetAlarmCustomerRespnse(cus, usercontext.CompanyId);
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        try
                        {
                            response.TerminateCustomer(code, CustomerTerminateReasonEnum.NotUsingService);
                            //base.AddUserActivityForCustomer("Customer" + "#" + CustomerId + "is terminated by" + (CurrentUser.FirstName + " " + CurrentUser.LastName), LabelHelper.ActivityAction.Terminate, CustomerLoadGuid, null, null);

                            #region Insert into AlarmCustomerTermination
                            CustomerInfo cusinfo = GetAlarmCustomerInfoByAlarmId(code, cus, usercontext.CompanyId);
                            AlarmCustomerTermination cusTerm = new AlarmCustomerTermination()
                            {
                                CustomerId = cus.CustomerId,
                                AlarmId = code,
                                TerminationReason = Reason,
                                TerminationDate = cusinfo.TermDate.Value,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                CreatedBy = emp.UserId

                            };
                            HSapiFacade.InsertCustomerTermination(cusTerm);
                            #endregion

                            #region Reservation of Cs Number
                            GlobalSetting RemoveCustomerNoGlobal = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "RemoveCustomerNoForTermination");
                            if (RemoveCustomerNoGlobal != null && RemoveCustomerNoGlobal.Value.ToLower() == "true")
                            {
                                CustomerSystemNo sysNo = HSapiFacade.GetCustomerSystemNoObjectByNumberAndCompanyId(cus.CustomerNo, usercontext.CompanyId);
                                if (sysNo != null)
                                {
                                    sysNo.IsUsed = true;
                                    sysNo.IsReserved = true;
                                    HSapiFacade.UpdateCustomerSystemNo(sysNo);
                                }
                                cus.CustomerNo = "";
                                HSapiFacade.UpdateCustomer(cus);
                            }
                            #endregion
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Success("Customer terminated successfully"));
                        }
                        catch (Exception ex)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer Not Found."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
            }
        }
        [Authorize]
        [Route("create-alarm-api-customer")]
        [HttpPost]
        public HttpResponseMessage IntegrateToAlarm([System.Web.Http.FromBody] SetupAlarmModel Data)
        {
            int cusId = 0;
            GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
            GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmSalesRepGlobalSetting = new GlobalSetting();
            AlarmCustomerManager cc = new AlarmCustomerManager();
            Alarm.AlarmCustomer.CustomerInfo result = new HS.Alarm.AlarmCustomer.CustomerInfo();
            try
            {
                var re = Request;
                var headers = re.Headers;
                if (headers.Contains("customerId"))
                {
                    int.TryParse(headers.GetValues("customerId").First(), out cusId);
                }
                var identity = (ClaimsIdentity)User.Identity;
                if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                {
                    var usercontext = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(identity.Claims.Where(c => c.Type == "CompanyId").Select(c => c.Value).SingleOrDefault()));
                    if (usercontext == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                    }
                    APIInitialize();
                    ComId = usercontext.CompanyId.ToString();
                    Employee emp = HSapiFacade.GetEmployeeByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    if (emp == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("User Not Found."));
                    }
                    Customer cus = HSapiFacade.GetCustomerById(cusId);
                    if (cus != null && cus.Id > 0)
                    {
                        SetupAlarm Model = new SetupAlarm();

                        Model.PropertyType = Data.PropertyType;
                        Model.CompanyId = usercontext.CompanyId;
                        Model.CustomerId = cus.CustomerId;
                        Model.DealerCustomer = Data.DealerCustomer;
                        Model.InsStreet = Data.InsStreet;
                        Model.InsZip = Data.InsZip;
                        Model.InsState = Data.InsState;
                        Model.InsCity = Data.InsCity;
                        Model.Phone = Data.Phone;
                        Model.EmailAddress = Data.Emailaddress;
                        Model.FirstName = Data.FirstName;
                        Model.LastName = Data.LastName;
                        Model.CentralStationAccountNo = Data.CentralStationAccountNo;
                        Model.CentralStationForwardingOption = Data.CentralStationForwardingOption;
                        Model.CentralStationRecieverNumber = Data.CentralStationRecieverNumber;
                        Model.CentralStationName = Data.CentralStationName;
                        Model.PhoneLinePresent = Data.PhoneLinePresent;
                        Model.IgnoreLowCoverageError = Data.IgnoreLowCoverageError;
                        Model.LoginName = Data.LoginName;
                        Model.LoginPassword = Data.LoginPassword;
                        Model.PanelType = Data.PanelType;
                        Model.PanelVersion = Data.PanelVersion;
                        Model.Culture = Data.Culture;
                        Model.CustomerStatus = Data.CustomerStatus;
                        Model.ModelSerialNumber = Data.ModelSerialNumber;
                        Model.PackageId = Data.PackageId;
                        Model.City = Data.City;
                        Model.Street = Data.Street;
                        Model.State = Data.State;
                        Model.Zip = Data.Zip;
                        Model.InstallationDate = Data.InstallationDate;
                        Model.InstallerUserName = Data.InstallerUserName;
                        Model.Network = Data.Network;
                        Model.AlarmRefId = Data.AlarmRefId;
                        Model.Action = Data.Action;
                        Model.AuthUser = Data.AuthUser;
                        Model.AuthPass = Data.AuthPass;                        
                        Model.SameInsAddress = Data.SameInsAddress;
                        Model.CompanyName = Data.CompanyName;
                        Model.ForwardedEvents = Data.ForwardedEvents;
                        Model.adonitem = Data.Adonitem;
                        Model.CustomerType = Data.CustomerType;
                        Model.IsContractSigned = Data.IsContractSigned;



                        HasMultipleAlarmUser = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "HasMultipleAlarmUser");
                        if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
                        {
                            if (cus != null && cus.Ownership == "Brinks")
                            {
                                AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "BrinksAlarmUserName");
                                AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "BrinksAlarmUserPassword");
                                AlarmSalesRepGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "BrinksAlarmSalesRepName");
                            }
                            else
                            {
                                AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "AlarmUsername");
                                AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "AlarmPassword");
                                AlarmSalesRepGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "AlarmSalesRepName");
                            }
                        }
                        else
                        {
                            AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "AlarmUsername");
                            AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "AlarmPassword");
                            AlarmSalesRepGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(usercontext.CompanyId, "AlarmSalesRepName");
                        }
                        if (AlarmUserGlobalSetting == null || AlarmPassGlobalSetting == null || string.IsNullOrWhiteSpace(AlarmUserGlobalSetting.Value) || string.IsNullOrWhiteSpace(AlarmPassGlobalSetting.Value))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("alarm.com credentials are not configured."));
                        }
                        if (AlarmSalesRepGlobalSetting != null)
                        {
                            Model.SalesRepName = AlarmSalesRepGlobalSetting.Value;
                        }
                        else
                        {
                            Model.SalesRepName = Data.SalesRepName;
                        }
                        Model.InstallerLoginName = emp.UserName;
                        Model.AuthUser = AlarmUserGlobalSetting.Value;
                        Model.AuthPass = AlarmPassGlobalSetting.Value;
                        Model.CompanyId = usercontext.CompanyId;
                        #region Check Cs Number
                        CustomerSystemNo sysNo = HSapiFacade.GetCusSysNoByCustomerNo(Model.CentralStationAccountNo);
                        if (sysNo != null && sysNo.IsUsed == true)
                        {
                            Model.CentralStationAccountNo = GetCSNumber(cus, Model.CentralStationName, usercontext.CompanyId);

                        }
                        #endregion
                        Alarm.AlarmCustomer.CreateCustomerOutput response = new HS.Alarm.AlarmCustomer.CreateCustomerOutput();
                        if (Model.Action == LabelHelper.AlarmCustomerActions.CreateCustomer)
                        {
                            response = cc.CreateCustomer(Model);
                            #region Update customer Packages
                            CustomerExtended extended = new CustomerExtended();
                            extended = HSapiFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                            if (response.Success)
                            {
                                int AutomationCount = 0;
                                int videoCount = 0;
                                int skybellCount = 0;
                                int Svrcount = 0;
                                AlarmCustomerSelectedAddon addon = new AlarmCustomerSelectedAddon();
                                List<AlarmCustomerSelectedAddon> addons = new List<AlarmCustomerSelectedAddon>();
                                List<AlarmCustomerSelectedAddon> OldAddon = new List<AlarmCustomerSelectedAddon>();
                                OldAddon = HSapiFacade.GetAllCutomerAlarmAddonsByCustomerId(Model.CustomerId);
                                if (OldAddon != null && OldAddon.Count > 0)
                                {
                                    foreach (var item in OldAddon)
                                    {
                                        HSapiFacade.DeleteCutomerAlarmAddons(item.Id);
                                    }
                                }
                                if (extended != null)
                                {
                                    extended.AlarmBasicPackage = "";
                                    HSapiFacade.UpdateCustomerExtended(extended);
                                }
                                if (Model.adonitem != null && Model.adonitem.Count() > 0)
                                {
                                    foreach (var item in Model.adonitem)
                                    {
                                        if (item == 36 || item == 41 || item == 2 || item == 5 || item == 23 || item == 21 || item == 19 || item == 20 || item == 52)
                                        {
                                            AutomationCount++;
                                        }
                                        else if (item == 76 || item == 70)
                                        {
                                            skybellCount++;
                                        }
                                        else if (item == 12 || item == 13)
                                        {
                                            videoCount++;
                                        }
                                        else if (item == 43)
                                        {
                                            Svrcount++;
                                        }

                                    }

                                    if (AutomationCount == 1)
                                    {
                                        addon = new AlarmCustomerSelectedAddon();

                                        addon.CustomerId = Model.CustomerId;
                                        addon.AddonType = "Automation1";

                                        HSapiFacade.InsertCutomerAlarmAddons(addon);
                                    }
                                    if (AutomationCount == 2)
                                    {

                                        addon = new AlarmCustomerSelectedAddon();

                                        addon.CustomerId = Model.CustomerId;
                                        addon.AddonType = "Automation2";

                                        HSapiFacade.InsertCutomerAlarmAddons(addon);
                                    }
                                    if (AutomationCount == 3)
                                    {
                                        addon = new AlarmCustomerSelectedAddon();

                                        addon.CustomerId = Model.CustomerId;
                                        addon.AddonType = "Automation3";

                                        HSapiFacade.InsertCutomerAlarmAddons(addon);
                                    }
                                    if (AutomationCount > 3)
                                    {
                                        addon = new AlarmCustomerSelectedAddon();

                                        addon.CustomerId = Model.CustomerId;
                                        addon.AddonType = "Automation4";
                                        HSapiFacade.InsertCutomerAlarmAddons(addon);
                                    }
                                    if (Svrcount > 0)
                                    {
                                        addon = new AlarmCustomerSelectedAddon();

                                        addon.CustomerId = Model.CustomerId;
                                        addon.AddonType = "SVR";
                                        HSapiFacade.InsertCutomerAlarmAddons(addon);
                                    }
                                    if (skybellCount > 0 && videoCount > 0)
                                    {
                                        addon = new AlarmCustomerSelectedAddon();

                                        addon.CustomerId = Model.CustomerId;
                                        addon.AddonType = "ProVideo";
                                        HSapiFacade.InsertCutomerAlarmAddons(addon);
                                    }
                                    else
                                    {
                                        if (skybellCount > 0)
                                        {
                                            addon = new AlarmCustomerSelectedAddon();

                                            addon.CustomerId = Model.CustomerId;
                                            addon.AddonType = "SkyBell";
                                            HSapiFacade.InsertCutomerAlarmAddons(addon);
                                        }
                                        else if (videoCount > 0)
                                        {

                                            addon = new AlarmCustomerSelectedAddon();

                                            addon.CustomerId = Model.CustomerId;
                                            addon.AddonType = "ProVideo";
                                            HSapiFacade.InsertCutomerAlarmAddons(addon);
                                        }
                                    }

                                }

                                if (Model.PackageId.HasValue)
                                {
                                    AlarmCustomerSelectedAddon basicaddon = new AlarmCustomerSelectedAddon();
                                    if (Model.PackageId == 208 || Model.PackageId == 209 || Model.PackageId == 193)
                                    {

                                        if (extended != null)
                                        {
                                            extended.AlarmBasicPackage = "Interactive";
                                            HSapiFacade.UpdateCustomerExtended(extended);
                                        }
                                        else
                                        {
                                            extended.CustomerId = cus.CustomerId;
                                            extended.AlarmBasicPackage = "Interactive";
                                            HSapiFacade.InsertCustomerExtended(extended);
                                        }
                                    }
                                }

                                #region Update Customer System No
                                CustomerSystemNo cusno = new CustomerSystemNo();
                                cusno = HSapiFacade.GetCusSysNoByCustomerNo(cus.CustomerNo);
                                if (cusno != null)
                                {
                                    cusno.IsUsed = true;
                                    cusno.CustomerId = cus.Id;
                                    cusno.UsedDate = DateTime.Now.UTCCurrentTime();
                                    HSapiFacade.UpdateCustomerSystemNo(cusno);
                                }
                                #endregion
                            }
                            #endregion

                        }
                        else if (Model.Action == LabelHelper.AlarmCustomerActions.CreateCommiment)
                        {
                            response = cc.CreateCommitment(Model);
                        }

                        if (response.Success)
                        {
                            if (Model.Action == LabelHelper.AlarmCustomerActions.CreateCommiment)
                            {
                                Model.CustomerStatus = LabelHelper.AlarmCustomerStatus.Commited;
                            }
                            else if (Model.Action == LabelHelper.AlarmCustomerActions.CreateCustomer)
                            {
                                Model.CustomerStatus = LabelHelper.AlarmCustomerStatus.Created;
                            }
                            else
                            {
                                Model.CustomerStatus = LabelHelper.AlarmCustomerStatus.Init;
                            }

                            SetupAlarm temp = HSapiFacade.GetSetupalarmByCustomerId(Model.CustomerId);
                            if (temp == null)
                            {
                                Model.CreatedDate = DateTime.UtcNow;
                                Model.CreatedBy = emp.UserId;
                                Model.LastUpdatedBy = emp.UserId;
                                Model.LastUpdatedDate = DateTime.UtcNow;
                                Model.Id = HSapiFacade.InsertSetupAlarm(Model);
                                //if (cus != null && Model.CustomerId != new Guid())
                                //{
                                //    base.AddUserActivityForCustomer("Customer #" + cus.Id + " Added to alarm.com by " + (emp.FirstName + " " + emp.LastName), LabelHelper.ActivityAction.AddAlarm_com, Model.CustomerId, null, null);

                                //}
                            }
                            else
                            {

                                temp.LastUpdatedBy = emp.UserId;
                                temp.LastUpdatedDate = DateTime.UtcNow;

                                HSapiFacade.UpdateSetupAlarm(temp);
                                //if (cus != null && Model.CustomerId != new Guid())
                                //{
                                //    base.AddUserActivityForCustomer("Customer #" + cus.Id + "Alarm is updated by " + (emp.FirstName + " " + emp.LastName), LabelHelper.ActivityAction.UpdateAlarm_com, Model.CustomerId, null, null);

                                //}
                            }

                            Customer TempCustomer = HSapiFacade.GetCustomerByCustomerId(Model.CustomerId);
                            TempCustomer.AlarmRefId = response.CustomerId.ToString();
                            HSapiFacade.UpdateCustomer(TempCustomer);

                            string susccessMessage = string.Format("Customer added to alarm.com successfully. CustomerId: {0}", response.CustomerId);
                            Customer customer = HSapiFacade.GetCustomerByCustomerId(Model.CustomerId);
                            if (customer != null)
                            {

                                customer.AlarmRefId = response.CustomerId.ToString();
                                HSapiFacade.UpdateCustomer(customer);
                            }
                            #region Delete previous Security Zones
                            List<Alarm.AlarmCustomer.PanelDevice> divicelist = new List<HS.Alarm.AlarmCustomer.PanelDevice>();
                            divicelist = GetAlarmSystemInfoList(usercontext.CompanyId, cus);
                            List<CustomerSecurityZones> securityZoneList = HSapiFacade.GetAllCustomerSecurityZoneByCustomerId(cus.CustomerId, "'Brinks'");
                            if (securityZoneList != null && securityZoneList.Count > 0)
                            {
                                foreach (var item in securityZoneList)
                                {
                                    HSapiFacade.DeleteCustomerSecurityZone(item.ID);
                                }
                            }
                            CustomerSecurityZones securityZones = new CustomerSecurityZones();
                            foreach (var item in divicelist)
                            {
                                securityZones = new CustomerSecurityZones()
                                {
                                    CustomerId = cus.CustomerId,
                                    ZoneNumber = item.DeviceId.ToString(),
                                    ZoneComment = item.WebSiteDeviceName,
                                    Platform = "Brinks"
                                };
                                HSapiFacade.InsertCustomerSecurityZone(securityZones);
                            }
                            #endregion
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Success(susccessMessage));
                        }
                        else
                        {
                            Model.CustomerStatus = LabelHelper.AlarmCustomerStatus.Init;
                            return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(response.ErrorMessage));
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Customer Not Found."));
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error("Authorization Denied."));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApiResponse<string>.Error(ex.Message));
            }

        }
        public List<Alarm.AlarmCustomer.PanelDevice> GetAlarmSystemInfoList(Guid CompanyId, Customer cus)
        {
            List<Alarm.AlarmCustomer.PanelDevice> divicelist = new List<HS.Alarm.AlarmCustomer.PanelDevice>();
            if (cus != null && !string.IsNullOrEmpty(cus.AlarmRefId))
            {
                GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
                GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
                GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();

                HasMultipleAlarmUser = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "HasMultipleAlarmUser");
                if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
                {
                    if (cus != null && cus.Ownership == "Brinks")
                    {
                        AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "BrinksAlarmUserName");
                        AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "BrinksAlarmUserPassword");
                    }
                    else
                    {
                        AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "AlarmUsername");
                        AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "AlarmPassword");
                    }
                }
                else
                {
                    AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "AlarmUsername");
                    AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "AlarmPassword");
                }


                Alarm.AlarmCustomer.CustomerManagement response = new Alarm.AlarmCustomer.CustomerManagement()
                {
                    AuthenticationValue = new HS.Alarm.AlarmCustomer.Authentication()
                    {
                        User = AlarmUserGlobalSetting.Value,
                        Password = AlarmPassGlobalSetting.Value
                    }
                };
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                try
                {
                    Alarm.AlarmCustomer.PanelDevice[] result = response.GetFullEquipmentList(Convert.ToInt32(cus.AlarmRefId));

                    if (result != null && result.ToList().Count > 0)
                    {
                        divicelist = result.ToList();

                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return divicelist;
        }
        public CustomerInfo GetAlarmCustomerInfoByAlarmId(int alarmId, Customer cus, Guid CompanyId)
        {
            Alarm.AlarmCustomer.CustomerManagement response = GetAlarmCustomerRespnse(cus, CompanyId);
            Alarm.AlarmCustomer.CustomerInfo result = new HS.Alarm.AlarmCustomer.CustomerInfo();
            Alarm.AlarmCustomer.CustomerBestPracticesOutput practiceResponse = new Alarm.AlarmCustomer.CustomerBestPracticesOutput();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                result = response.GetCustomerInfo(alarmId);
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        public Alarm.AlarmCustomer.CustomerManagement GetAlarmCustomerRespnse(Customer cus, Guid CompanyId)
        {
            GlobalSetting HasMultipleAlarmUser = new GlobalSetting();
            GlobalSetting AlarmUserGlobalSetting = new GlobalSetting();
            GlobalSetting AlarmPassGlobalSetting = new GlobalSetting();

            HasMultipleAlarmUser = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "HasMultipleAlarmUser");
            if (HasMultipleAlarmUser != null && HasMultipleAlarmUser.Value.ToLower() == "true")
            {
                if (cus != null && cus.Ownership == "Brinks")
                {
                    AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "BrinksAlarmUserName");
                    AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "BrinksAlarmUserPassword");
                }
                else
                {
                    AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "AlarmUsername");
                    AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "AlarmPassword");
                }
            }
            else
            {
                AlarmUserGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "AlarmUsername");
                AlarmPassGlobalSetting = HSapiFacade.GetGlobalSettingsByKey(CompanyId, "AlarmPassword");
            }
            Alarm.AlarmCustomer.CustomerManagement response = new Alarm.AlarmCustomer.CustomerManagement()
            {
                AuthenticationValue = new HS.Alarm.AlarmCustomer.Authentication()
                {
                    User = AlarmUserGlobalSetting.Value,
                    Password = AlarmPassGlobalSetting.Value
                }
            };
            return response;
        }

        #endregion
    }
}