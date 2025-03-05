using HS.Entities;
using HS.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web;
using HS.Framework.Utils;
using PhoneNumbers;
using System.Globalization;

namespace HS.Web.UI
{
    [RoutePrefix("1.0")]
    public class RmrSController : ApiController
    {
        [Authorize]
        [Route("GetCustomer")]
        public GETCustomerDataResponse Get()
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;
            #region Directory for saving reports
            string subPath = "~/APIReports"; // your code goes here
            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));

            #endregion

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\APIReports\Report.txt"), true))
            {
                file.WriteLine("GETCustomerData started at {0}", DateTime.Now.ToString("MM/dd/yyyy"));
            }
            string ANI = "";
            string DNIS = "";
            var re = Request;
            var headers = re.Headers;
            GETCustomerDataResponse customer = new GETCustomerDataResponse()
            {
                ExistingCustomer = false
            };

            if (headers.Contains("ANI"))
            {
                ANI = headers.GetValues("ANI").First();
            }
            if (headers.Contains("DNIS"))
            {
                DNIS = headers.GetValues("DNIS").First();
            }

            if (string.IsNullOrWhiteSpace(ANI))
            {
                return customer;
            }
            else if (ANI.Length > 9 && ANI.Length < 15)
            {
                try
                {
                    PhoneNumberUtil util = PhoneNumberUtil.GetInstance();
                    PhoneNumber number = util.Parse(ANI, "US");
                    ANI = util.Format(number, PhoneNumberFormat.E164);
                    if (ANI.IndexOf("+1") != 0)
                    {
                        return customer;
                    }
                }
                catch (Exception)
                {
                    return customer;
                }
            }
            else
            {
                return customer;
            }
            ANI = ANI.Substring(2, ANI.Length - 2);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\APIReports\Report.txt"), true))
            {
                file.WriteLine("ANI {0}  DNIS {1} IP {3} Time {2}", ANI, DNIS, DateTime.Now, AppConfig.GetIP);
            }
            ApiFacade apiFacd = new ApiFacade();
            var CustomerDetails = apiFacd.GetCustomerDetailsByApi(ANI);
            if (CustomerDetails != null)
            {
                customer.Hours = "Open";
                customer.Brand = "Pioneer";
                customer.ExistingCustomer = true;
                customer.CustomerNumber = CustomerDetails.CustomerNo;
                customer.AddressStreet = CustomerDetails.Street;
                customer.AddressCity = CustomerDetails.City;
                customer.AddressState = CustomerDetails.State;
                customer.AddressZip = CustomerDetails.ZipCode;
                PhoneNumberUtil util = PhoneNumberUtil.GetInstance();

                if (!string.IsNullOrWhiteSpace(CustomerDetails.PrimaryPhone))
                {
                    try
                    {
                        PhoneNumber number = util.Parse(CustomerDetails.PrimaryPhone, "US");
                        customer.MainPhone = util.Format(number, PhoneNumberFormat.E164);
                    }
                    catch (Exception) { }
                }

                if (!string.IsNullOrWhiteSpace(CustomerDetails.PrimaryPhone))
                {
                    try
                    {
                        PhoneNumber number = util.Parse(CustomerDetails.SecondaryPhone, "US");
                        customer.SecondaryPhone = util.Format(number, PhoneNumberFormat.E164);
                    }
                    catch (Exception) { }
                }
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\APIReports\Report.txt"), true))
            {
                file.WriteLine("Result sent");
            }
            return customer;
        }

        [Authorize]
        [Route("GetAppointmentLookupByCustomerNumber")]
        public GETCustomerLookUpResponse GetAptByCustomerNumber()
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;
            string CustomerNumber = "";
            var re = Request;
            var headers = re.Headers;
            GETCustomerLookUpResponse customer = new GETCustomerLookUpResponse()
            {
                ExistingAppointment = false
            };
            if (headers.Contains("CustomerNumber"))
            {
                CustomerNumber = headers.GetValues("CustomerNumber").First();
            }
            int result;
            if (string.IsNullOrWhiteSpace(CustomerNumber) || !int.TryParse(CustomerNumber, out result))
            {
                return customer;
            }
            ApiFacade apiFacd = new ApiFacade();
            var CustomerAptDetails = apiFacd.GetCustomerAptByNumber(CustomerNumber);
            if (CustomerAptDetails != null && CustomerAptDetails.AppointmentDate != null)
            {
                customer.ExistingAppointment = true;
                customer.ServiceArea = "Out";
                customer.AppointmentDay = Convert.ToDateTime(CustomerAptDetails.AppointmentDate).DayOfWeek.ToString();
                customer.AppointmentMonth = Convert.ToDateTime(CustomerAptDetails.AppointmentDate).ToString("MMMM", CultureInfo.InvariantCulture);
                customer.AppointmentDate = Convert.ToDateTime(CustomerAptDetails.AppointmentDate).Day;
                customer.AppointmentTimeframe = CustomerAptDetails.AppointmentStartTime + "-" + CustomerAptDetails.AppointmentEndTime;
            }
            return customer;
        }

        [Authorize]
        [Route("GetAppointmentLookupByZipcode")]
        public GETCustomerLookUpResponse GetAptByZipCode()
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;
            string ZipCode = "";
            var re = Request;
            var headers = re.Headers;
            GETCustomerLookUpResponse customer = new GETCustomerLookUpResponse()
            {
                ExistingAppointment = false
            };
            if (headers.Contains("ZipCode"))
            {
                ZipCode = headers.GetValues("ZipCode").First();
            }
            if (string.IsNullOrWhiteSpace(ZipCode))
            {
                return customer;
            }
            ApiFacade apiFacd = new ApiFacade();
            var CustomerAptDetails = apiFacd.GetCustomerAptListByZipCode(ZipCode);
            customer = GetCustomerLookup("", CustomerAptDetails, DateTime.Now);
            return customer;
        }

        [Authorize]
        [Route("GetAppointmentLookupByDate")]
        public GETCustomerLookUpResponse GetAptByDate()
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;
            string ZipCode = "";
            string AppointmentDay = "";
            string AppointmentMonth = "";
            string AppointmentDate = "";
            string AppointmentTimeframe = "";
            string Token = "";
            DateTime AppointmentDateVal;
            var re = Request;
            var headers = re.Headers;
            GETCustomerLookUpResponse customer = new GETCustomerLookUpResponse()
            {
                ExistingAppointment = false
            };
            if (headers.Contains("ZipCode"))
            {
                ZipCode = headers.GetValues("ZipCode").First();
            }
            if (headers.Contains("AppointmentDay"))
            {
                AppointmentDay = headers.GetValues("AppointmentDay").First();
            }
            if (headers.Contains("AppointmentMonth"))
            {
                AppointmentMonth = headers.GetValues("AppointmentMonth").First();
            }
            if (headers.Contains("AppointmentDate"))
            {
                AppointmentDate = headers.GetValues("AppointmentDate").First();
            }
            if (headers.Contains("AppointmentTimeframe"))
            {
                AppointmentTimeframe = headers.GetValues("AppointmentTimeframe").First();
            }
            if (headers.Contains("Token"))
            {
                Token = headers.GetValues("Token").First();
            }
            int result;
            ApiFacade apiFacd = new ApiFacade();
            List<CustomerAppointment> CustomerAptDetails = new List<CustomerAppointment>();
            if (!string.IsNullOrWhiteSpace(ZipCode) && !string.IsNullOrWhiteSpace(AppointmentMonth) && !string.IsNullOrWhiteSpace(AppointmentDate) && int.TryParse(AppointmentDate, out result))
            {
                int AppointmentMonthInt = DateTime.ParseExact(AppointmentMonth, "MMMM", CultureInfo.CurrentCulture).Month;
                int AppointmentDateInt = Convert.ToInt32(AppointmentDate);
                if (AppointmentMonthInt > 0 && AppointmentMonthInt < 13 && AppointmentDateInt > 0 && AppointmentDateInt < 32)
                {
                    AppointmentDateVal = new DateTime(DateTime.Now.Year, AppointmentMonthInt, AppointmentDateInt);
                    CustomerAptDetails = apiFacd.GetCustomerAptByDateAndZipCode(AppointmentDateVal, ZipCode);
                    customer = GetCustomerLookup(Token, CustomerAptDetails, AppointmentDateVal);
                    if (CustomerAptDetails.Count == 0)
                    {
                        customer.AppointmentDay = AppointmentDay;
                        customer.AppointmentMonth = AppointmentMonth;
                        customer.AppointmentDate = AppointmentDateInt;
                    }
                    if (!string.IsNullOrWhiteSpace(customer.AppointmentTimeframe))
                    {
                        var StartTime = customer.AppointmentTimeframe.Split('-')[0];
                        var EndTime = customer.AppointmentTimeframe.Split('-')[1];
                        if (!string.IsNullOrWhiteSpace(StartTime) && !string.IsNullOrWhiteSpace(EndTime))
                        {
                            DateTime StartTimeDate = Convert.ToDateTime(StartTime);
                            DateTime EndDate = Convert.ToDateTime(EndTime);
                            customer.AppointmentTimeframe = StartTimeDate.ToString("hh:mm tt") + " - " + EndDate.ToString("hh:mm tt");
                        }

                    }
                }
            }
            else if (!string.IsNullOrWhiteSpace(ZipCode))
            {
                CustomerAptDetails = apiFacd.GetCustomerAptListByZipCode(ZipCode);
                customer = GetCustomerLookup(Token, CustomerAptDetails, DateTime.Now);
                if (!string.IsNullOrWhiteSpace(customer.AppointmentTimeframe))
                {
                    var StartTime = customer.AppointmentTimeframe.Split('-')[0];
                    var EndTime = customer.AppointmentTimeframe.Split('-')[1];
                    if (!string.IsNullOrWhiteSpace(StartTime) && !string.IsNullOrWhiteSpace(EndTime))
                    {
                        DateTime StartTimeDate = Convert.ToDateTime(StartTime);
                        DateTime EndDate = Convert.ToDateTime(EndTime);
                        customer.AppointmentTimeframe = StartTimeDate.ToString("hh:mm tt") + " - " + EndDate.ToString("hh:mm tt");
                    }

                }
            }
            else if (!string.IsNullOrWhiteSpace(AppointmentMonth) && !string.IsNullOrWhiteSpace(AppointmentDate) && int.TryParse(AppointmentDate, out result))
            {
                int AppointmentMonthInt = DateTime.ParseExact(AppointmentMonth, "MMMM", CultureInfo.CurrentCulture).Month;
                int AppointmentDateInt = Convert.ToInt32(AppointmentDate);
                if (AppointmentMonthInt > 0 && AppointmentMonthInt < 13 && AppointmentDateInt > 0 && AppointmentDateInt < 32)
                {
                    AppointmentDateVal = new DateTime(DateTime.Now.Year, AppointmentMonthInt, AppointmentDateInt);
                    CustomerAptDetails = apiFacd.GetCustomerAptByDate(AppointmentDateVal);
                    customer = GetCustomerLookup(Token, CustomerAptDetails, AppointmentDateVal);
                    if (CustomerAptDetails.Count == 0)
                    {
                        customer.AppointmentDay = AppointmentDay;
                        customer.AppointmentMonth = AppointmentMonth;
                        customer.AppointmentDate = AppointmentDateInt;
                    }
                    if (!string.IsNullOrWhiteSpace(customer.AppointmentTimeframe))
                    {
                        var StartTime = customer.AppointmentTimeframe.Split('-')[0];
                        var EndTime = customer.AppointmentTimeframe.Split('-')[1];
                        if (!string.IsNullOrWhiteSpace(StartTime) && !string.IsNullOrWhiteSpace(EndTime))
                        {
                            DateTime StartTimeDate = Convert.ToDateTime(StartTime);
                            DateTime EndDate = Convert.ToDateTime(EndTime);
                            customer.AppointmentTimeframe = StartTimeDate.ToString("hh:mm tt") + " - " + EndDate.ToString("hh:mm tt");
                        }

                    }
                }
            }
            return customer;
        }
        private GETCustomerLookUpResponse GetCustomerLookup(string Token, List<CustomerAppointment> CustomerAptDetails, DateTime AptDate)
        {
            GETCustomerLookUpResponse customer = new GETCustomerLookUpResponse()
            {
                ExistingAppointment = false
            };
            ApiFacade apiFacd = new ApiFacade();
            var StartEndLookUpTime = apiFacd.GetLookupByKey("Arrival").Where(x => x.IsActive == true && x.DataValue != "-1").ToList();
            List<string> OneHourList = new List<string>();
            List<string> OneHourListFinal = new List<string>();
            for (int i = 0; i < StartEndLookUpTime.Count; i = i + 4)
            {
                var Increse = i + 5;
                if (Increse <= StartEndLookUpTime.Count)
                {
                    var ListItem = StartEndLookUpTime[i].DataValue + "-" + StartEndLookUpTime[i + 4].DataValue;
                    OneHourList.Add(ListItem);
                    OneHourListFinal.Add(ListItem);
                    if (StartEndLookUpTime[i + 4].DataValue == "23:00")
                    {
                        ListItem = "23:00-00:00";
                        OneHourList.Add(ListItem);
                        OneHourListFinal.Add(ListItem);
                    }
                }
            }
            List<CustomerAppointment> finalAppointmentList = new List<CustomerAppointment>();
            foreach (var OneHourListItem in OneHourList)
            {
                var OneHourStart = OneHourListItem.Split('-')[0];
                var OneHourEnd = OneHourListItem.Split('-')[1];
                TimeSpan OneHourStartTime = new TimeSpan(Convert.ToInt32(OneHourStart.Split(':')[0]), 0, 0);
                TimeSpan OneHourEndTime = new TimeSpan(Convert.ToInt32(OneHourEnd.Split(':')[0]), 0, 0);
                TimeSpan now = DateTime.Now.TimeOfDay;
                if (now > OneHourStartTime && DateTime.Now.Date == AptDate.Date)
                {
                    OneHourListFinal.Remove(OneHourListItem);
                }
                foreach (var CustomerAptDetailsItem in CustomerAptDetails)
                {
                    TimeSpan AptStartTime = new TimeSpan(Convert.ToInt32(CustomerAptDetailsItem.AppointmentStartTime.Split(':')[0]), Convert.ToInt32(CustomerAptDetailsItem.AppointmentStartTime.Split(':')[1]), 0);
                    TimeSpan AptEndTime = new TimeSpan(Convert.ToInt32(CustomerAptDetailsItem.AppointmentEndTime.Split(':')[0]), Convert.ToInt32(CustomerAptDetailsItem.AppointmentEndTime.Split(':')[1]), 0);
                    if (OneHourStartTime == AptStartTime && OneHourEndTime == AptEndTime)
                    {
                        OneHourListFinal.Remove(OneHourListItem);
                        finalAppointmentList.Add(CustomerAptDetailsItem);
                    }
                    if ((AptStartTime > OneHourStartTime) && (AptStartTime < OneHourEndTime))
                    {
                        OneHourListFinal.Remove(OneHourListItem);
                        finalAppointmentList.Add(CustomerAptDetailsItem);
                    }
                    if ((AptEndTime > OneHourStartTime) && (AptEndTime < OneHourEndTime))
                    {
                        OneHourListFinal.Remove(OneHourListItem);
                        finalAppointmentList.Add(CustomerAptDetailsItem);
                    }
                }
            }
            var ReturnToken = "";
            string AppointmentDay = "";
            string AppointmentMonth = "";
            int AppointmentDateInt = 0;
            DateTime tommowo = DateTime.Now.AddDays(1);
            if (string.IsNullOrEmpty(Token))
            {
                ReturnToken = EncodeNumber(1);
                if (OneHourListFinal.Count > 0)
                {
                    customer.AppointmentTimeframe = OneHourListFinal[0];
                    if (finalAppointmentList.Count > 0)
                    {
                        AppointmentDay = finalAppointmentList[0].AppointmentDate.Value.ToString("dddd");
                        AppointmentMonth = finalAppointmentList[0].AppointmentDate.Value.ToString("MMMM");
                        AppointmentDateInt = Convert.ToInt32(finalAppointmentList[0].AppointmentDate.Value.ToString("dd"));
                    }
                    else
                    {
                        AppointmentDay = tommowo.ToString("dddd");
                        AppointmentMonth = tommowo.ToString("MMMM");
                        AppointmentDateInt = Convert.ToInt32(tommowo.ToString("dd"));

                    }
                }
            }
            else
            {
                var decodeToken = DecodeNumber(Token);
                if (OneHourListFinal.Count > decodeToken)
                {
                    customer.AppointmentTimeframe = OneHourListFinal[decodeToken];
                    if (finalAppointmentList.Count > 0)
                    {
                        AppointmentDay = finalAppointmentList[decodeToken].AppointmentDate.Value.ToString("dddd");
                        AppointmentMonth = finalAppointmentList[decodeToken].AppointmentDate.Value.ToString("MMMM");
                        AppointmentDateInt = Convert.ToInt32(finalAppointmentList[decodeToken].AppointmentDate.Value.ToString("dd"));
                    }
                    else
                    {
                        AppointmentDay = tommowo.ToString("dddd");
                        AppointmentMonth = tommowo.ToString("MMMM");
                        AppointmentDateInt = Convert.ToInt32(tommowo.ToString("dd"));

                    }
                }
                else
                {
                    return customer;
                }
                ReturnToken = EncodeNumber(decodeToken + 1);
            }
            customer.ExistingAppointment = false;
            customer.ServiceArea = "Out";
            customer.AppointmentDay = AppointmentDay;
            customer.AppointmentMonth = AppointmentMonth;
            customer.AppointmentDate = AppointmentDateInt;
            customer.Token = ReturnToken;

            return customer;
        }
        [Authorize]
        [Route("GetAppointmentScheduler")]
        public GETCustomerSchedulerResponse GetAppointmentScheduler()
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;
            ApiFacade apiFacd = new ApiFacade();
            GlobalSetting CompanyIdDetails = apiFacd.GetGlobalSettingsByOnlyKey("CompanyIdDefault");
            GlobalSetting EmployeeIdDetails = apiFacd.GetGlobalSettingsByOnlyKey("EmployeeIdDefault");
            string CustomerNumber = "";
            string AppointmentDay = "";
            string AppointmentMonth = "";
            string AppointmentDate = "";
            string AppointmentTimeframe = "";
            string Brand = "";
            DateTime AppointmentDateVal;
            var re = Request;
            var headers = re.Headers;
            GETCustomerSchedulerResponse customer = new GETCustomerSchedulerResponse()
            {
                Success = false
            };
            if (headers.Contains("CustomerNumber"))
            {
                CustomerNumber = headers.GetValues("CustomerNumber").First();
            }
            if (headers.Contains("AppointmentDay"))
            {
                AppointmentDay = headers.GetValues("AppointmentDay").First();
            }
            if (headers.Contains("AppointmentMonth"))
            {
                AppointmentMonth = headers.GetValues("AppointmentMonth").First();
            }
            if (headers.Contains("AppointmentDate"))
            {
                AppointmentDate = headers.GetValues("AppointmentDate").First();
            }
            if (headers.Contains("AppointmentTimeframe"))
            {
                AppointmentTimeframe = headers.GetValues("AppointmentTimeframe").First();
            }
            if (headers.Contains("Brand"))
            {
                Brand = headers.GetValues("Brand").First();
            }
            int result;
            if (string.IsNullOrWhiteSpace(CustomerNumber) || string.IsNullOrWhiteSpace(AppointmentDay) || string.IsNullOrWhiteSpace(AppointmentMonth) || string.IsNullOrWhiteSpace(AppointmentDate) || !int.TryParse(AppointmentDate, out result))
            {
                return customer;
            }
            else
            {
                var CustomerDetails = apiFacd.GetCustomerDetailsBycNumber(CustomerNumber);
                if (CustomerDetails == null)
                {
                    return customer;
                }

                int AppointmentMonthInt = DateTime.ParseExact(AppointmentMonth, "MMMM", CultureInfo.CurrentCulture).Month;
                int AppointmentDateInt = Convert.ToInt32(AppointmentDate);
                if (AppointmentMonthInt > 0 && AppointmentMonthInt < 13 && AppointmentDateInt > 0 && AppointmentDateInt < 32)
                {
                   AppointmentDateVal = new DateTime(DateTime.Now.Year, AppointmentMonthInt, AppointmentDateInt);
                }
                else
                {
                    return customer;
                }

                var StartTime = AppointmentTimeframe.Split('-')[0];
                var EndTime = AppointmentTimeframe.Split('-')[1];
                DateTime StartTimeDate = Convert.ToDateTime(StartTime);
                DateTime EndTimeDate = Convert.ToDateTime(EndTime);
                var AppointmentStartTime = StartTimeDate.ToString("HH:mm");
                var AppointmentEndTime = EndTimeDate.ToString("HH:mm");

                var CustomerAptDetails = apiFacd.GetCustomerAptByDateAndCustomerNumberTimeFrame(AppointmentDateVal, CustomerNumber, AppointmentStartTime, AppointmentEndTime);
                if (CustomerAptDetails != null)
                {
                    return customer;
                }
                else
                {
                    CustomerAppointment cstApt = new CustomerAppointment();
                    cstApt.AppointmentId = Guid.NewGuid();
                    cstApt.CompanyId = CompanyIdDetails != null ? new Guid(CompanyIdDetails.Value) : Guid.Empty;
                    cstApt.CustomerId = CustomerDetails.CustomerId;
                    cstApt.EmployeeId = EmployeeIdDetails != null ? new Guid(EmployeeIdDetails.Value) : Guid.Empty; ;
                    cstApt.AppointmentType = "Installation";
                    cstApt.AppointmentDate = AppointmentDateVal;
                    cstApt.AppointmentStartTime = AppointmentStartTime;
                    cstApt.AppointmentEndTime = AppointmentEndTime;
                    cstApt.IsAllDay = false;
                    cstApt.CreatedBy = "API";
                    cstApt.LastUpdatedBy = "API";
                    cstApt.LastUpdatedDate = DateTime.Now;
                    bool CustomerAptInsert = apiFacd.InsertCustomerApt(cstApt);
                    if(CustomerAptInsert)
                    {
                        customer.Success = true;
                    }
                }
            }
            return customer;
        }

        [Authorize]
        [Route("GetAppointmentDateValidator")]
        public GETCustomerDateValidatorResponse GetAppointmentDateValidator()
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;
            string ZipCode = "";
            string Date = "";
            var re = Request;
            var headers = re.Headers;
            GETCustomerDateValidatorResponse customer = new GETCustomerDateValidatorResponse()
            {
                ValidDate = false
            };
            if (headers.Contains("ZipCode"))
            {
                ZipCode = headers.GetValues("ZipCode").First();
            }
            if (headers.Contains("Date"))
            {
                Date = headers.GetValues("Date").First();
            }
            if (string.IsNullOrWhiteSpace(Date))
            {
                return customer;
            }
            else
            {
                DateTime pDateInput = DateTime.ParseExact(Date, "M-dd-yyyy", CultureInfo.InvariantCulture);
                var DayName = pDateInput.DayOfWeek.ToString();
                if (DayName != "Saturday" && DayName != "Sunday")
                {
                    customer.ValidDate = true;
                }
            }
            return customer;
        }

        [Authorize]
        [Route("GetPartialAppointmentScheduler")]
        public GETCustomerSchedulerResponse GetPartialAppointmentScheduler()
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;
            ApiFacade apiFacd = new ApiFacade();

            string ANI = "";
            string CustomerNumber = "";
            string AppointmentDay = "";
            string AppointmentMonth = "";
            string AppointmentDate = "";
            string AppointmentTimeframe = "";
            string Brand = "";
            DateTime AppointmentDateVal;
            var re = Request;
            var headers = re.Headers;
            GETCustomerSchedulerResponse customer = new GETCustomerSchedulerResponse()
            {
                Success = false
            };
            if (headers.Contains("ANI"))
            {
                ANI = headers.GetValues("ANI").First();
            }
            if (headers.Contains("CustomerNumber"))
            {
                CustomerNumber = headers.GetValues("CustomerNumber").First();
            }
            if (headers.Contains("AppointmentDay"))
            {
                AppointmentDay = headers.GetValues("AppointmentDay").First();
            }
            if (headers.Contains("AppointmentMonth"))
            {
                AppointmentMonth = headers.GetValues("AppointmentMonth").First();
            }
            if (headers.Contains("AppointmentDate"))
            {
                AppointmentDate = headers.GetValues("AppointmentDate").First();
            }
            if (headers.Contains("AppointmentTimeframe"))
            {
                AppointmentTimeframe = headers.GetValues("AppointmentTimeframe").First();
            }
            if (headers.Contains("Brand"))
            {
                Brand = headers.GetValues("Brand").First();
            }
            var CustomerDetails = new Customer();
            if (!string.IsNullOrEmpty(ANI) && !string.IsNullOrEmpty(CustomerNumber))
            {
                CustomerDetails = apiFacd.GetCustomerDetailsByApicNumber(ANI, CustomerNumber);
            }
            else if (!string.IsNullOrEmpty(ANI))
            {
                CustomerDetails = apiFacd.GetCustomerDetailsByApi(ANI);
            }
            else if (!string.IsNullOrEmpty(CustomerNumber))
            {
                CustomerDetails = apiFacd.GetCustomerDetailsBycNumber(CustomerNumber);
            }
            else
            {
                customer.Success = false;
                return customer;
            }
            GlobalSetting CompanyIdDetails = apiFacd.GetGlobalSettingsByOnlyKey("CompanyIdDefault");
            GlobalSetting EmployeeIdDetails = apiFacd.GetGlobalSettingsByOnlyKey("EmployeeIdDefault");
            if (CustomerDetails == null || CustomerDetails.CustomerId == Guid.Empty)
            {
                var UserId = Guid.NewGuid();
                CustomerDetails = new Customer()
                {
                    CustomerId = UserId,
                    FirstName = "Caller",
                    LastName = "Name",
                    Type = "Residential",
                    PrimaryPhone = ANI,
                    CreatedByUid = UserId,
                    LastUpdatedBy = "API",
                    LastUpdatedByUid = UserId,
                    LastUpdatedDate = DateTime.Now,
                    JoinDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    PhoneType = "",
                    Carrier = "",
                    ReferringCustomer = Guid.Empty,
                    IsActive = true
                };
                bool CustomerInsert = apiFacd.InsertCustomer(CustomerDetails);
                var CustomerCompany = new CustomerCompany()
                {
                    CustomerId = UserId,
                    CompanyId = CompanyIdDetails != null ? new Guid(CompanyIdDetails.Value) : Guid.Empty,
                    IsLead = false,
                    ConvertionDate = DateTime.UtcNow,
                };
                bool CustomerCompanyInsert = apiFacd.InsertCustomerCompany(CustomerCompany);
            }
            CustomerAppointment cstApt = new CustomerAppointment();
            cstApt.AppointmentId = Guid.NewGuid();
            cstApt.CompanyId = CompanyIdDetails != null ? new Guid(CompanyIdDetails.Value) : Guid.Empty;
            cstApt.CustomerId = CustomerDetails.CustomerId;
            cstApt.EmployeeId = EmployeeIdDetails != null ? new Guid(EmployeeIdDetails.Value) : Guid.Empty; ;
            cstApt.AppointmentType = "Installation";
            cstApt.IsAllDay = false;
            cstApt.CreatedBy = "API";
            cstApt.LastUpdatedBy = "API";
            cstApt.LastUpdatedDate = DateTime.Now;
            int result;
            if (!string.IsNullOrWhiteSpace(AppointmentMonth) && !string.IsNullOrWhiteSpace(AppointmentDate) && int.TryParse(AppointmentDate, out result))
            {
                int AppointmentMonthInt = DateTime.ParseExact(AppointmentMonth, "MMMM", CultureInfo.CurrentCulture).Month;
                int AppointmentDateInt = Convert.ToInt32(AppointmentDate);
                if (AppointmentMonthInt > 0 && AppointmentMonthInt < 13 && AppointmentDateInt > 0 && AppointmentDateInt < 32)
                {
                    cstApt.AppointmentDate = new DateTime(DateTime.Now.Year, AppointmentMonthInt, AppointmentDateInt);
                }
            }
            if (!string.IsNullOrEmpty(AppointmentTimeframe))
            {
                var StartTime = AppointmentTimeframe.Split('-')[0];
                var EndTime = AppointmentTimeframe.Split('-')[1];
                DateTime StartTimeDate = Convert.ToDateTime(StartTime);
                DateTime EndTimeDate = Convert.ToDateTime(EndTime);
                cstApt.AppointmentStartTime = StartTimeDate.ToString("HH:mm");
                cstApt.AppointmentEndTime = EndTimeDate.ToString("HH:mm");
            }
            bool CustomerAptInsert = apiFacd.InsertCustomerApt(cstApt);
            customer.Success = CustomerAptInsert;
            return customer;
        }


        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        private string alphanums = "abcdefghijklmnopqrstuvwxyz0123456789";
        private const int codeLen = 6;
        public string EncodeNumber(int num)
        {
            if (num < 1 || num > 999999) //or throw an exception
                return "";
            int[] nums = new int[codeLen];
            int pos = 0;

            while (!(num == 0))
            {
                nums[pos] = num % alphanums.Length;
                num /= alphanums.Length;
                pos += 1;
            }

            string result = "";
            foreach (int numIndex in nums)
                result = alphanums[numIndex].ToString() + result;

            return result;
        }

        public int DecodeNumber(string str)
        {
            //Check for invalid string
            if (str.Length != codeLen) //Or throw an exception
                return -1;
            long num = 0;

            foreach (char ch in str)
            {
                num *= alphanums.Length;
                num += alphanums.IndexOf(ch);
            }

            //Check for invalid number
            if (num < 1 || num > 999999) //or throw exception
                return -1;
            return System.Convert.ToInt32(num);
        }
    }
}