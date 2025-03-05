using HS.Entities;
using HS.Web.UI.Helper;
using Newtonsoft.Json;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using HS.Payments.TransactionReporting;
using HS.Payments.RecurringBilling;
using HS.Payments.CustomerProfiles;
using AuthorizeNet.Api.Contracts.V1;
using HS.Framework;
using PhoneNumbers;
using System.Globalization;
using HS.Kickbox;
using System.Net;
using HS.Kickbox.Helpers;
using System.IO;
using System.Xml;
using HS.Kickbox.Models;
using System.Security.Authentication;
using RestSharp;
using HS.CSM;
using HS.CSM.Models;
using System.Net.Http;
using System.Text;
using System.IO.Compression;
using Forte;
using EO.Internal;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using HS.Framework.Utils;
using Forte.Entities;
using System.Text.RegularExpressions;
using NLog;
//using iTextSharp.text;
//using iTextSharp.text.pdf; 
//using HS.UCC.com.teamucc.dealer;
using OS.AWS.S3;
using OS.AWS.S3.Services;
using System.Threading;
using System.Threading.Tasks;

namespace HS.Web.UI.Controllers
{
    public class JsonRequestController : BaseController
    {
        public JsonRequestController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        #region PDF Sharp
        public object RotativaTest()
        {
            using (PdfSharp.Pdf.PdfDocument one = PdfReader.Open(Server.MapPath("/Files/1.pdf"), PdfDocumentOpenMode.Import))
            using (PdfSharp.Pdf.PdfDocument two = PdfReader.Open(Server.MapPath("/Files/2.pdf"), PdfDocumentOpenMode.Import))
            using (PdfSharp.Pdf.PdfDocument outPdf = new PdfDocument())
            {
                CopyPages(one, outPdf);
                CopyPages(two, outPdf);

                //outPdf.Save(Server.MapPath("/Files/file1and2.pdf"));
                MemoryStream stream = new MemoryStream();
                // Saves the document as stream
                outPdf.Save(stream);
                outPdf.Dispose();
                // Converts the PdfDocument object to byte form.
                byte[] docBytes = stream.ToArray();
                return File(docBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "MargedPDF.pdf");

            }
            return null;
        }

        void CopyPages(PdfSharp.Pdf.PdfDocument from, PdfSharp.Pdf.PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
        }
        #endregion

        #region iTextSharp
        //private static byte[] combineViewData(List<byte[]> viewData)
        //{
        //    byte[] combinedViewData = null;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (Document document = new Document())
        //        {
        //            using (PdfCopy copy = new PdfCopy(document, ms))
        //            {
        //                document.Open();

        //                foreach (byte[] arr in viewData)
        //                {
        //                    using (MemoryStream viewStream = new MemoryStream(arr))
        //                    {
        //                        using (iTextSharp.text.pdf.pdfReader reader = new iTextSharp.text.pdf.pdfReader(viewStream))
        //                        {
        //                            int n = reader.NumberOfPages;
        //                            for (int page = 0; page < n;)
        //                            {
        //                                copy.AddPage(copy.GetImportedPage(reader, ++page));
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        combinedViewData = ms.ToArray();
        //    }
        //    return combinedViewData;
        //}

        //public ActionResult Combine()
        //{
        //    //Create single views
        //    ViewAsPdf view = new ViewAsPdf("ViewName");
        //    byte[] viewData = view.BuildFile(ControllerContext);

        //    //Add them to array
        //    List<byte[]> viewDatas = new List<byte[]>();
        //    viewDatas.Add(viewData);

        //    //Combine them
        //    byte[] combinedViewData = combineViewData(viewDatas);

        //    return null;
        //}
        #endregion

        #region Emplopyee Pto Hours
        public double? GetPtoAccrualRate(int Days, string PayType)
        {
            double? Rate = 0.0;
            if(Days > 0 && !string.IsNullOrWhiteSpace(PayType))
            {
                EmployeePtoAccrualRate PtoAccrualRate = _Util.Facade.EmployeeFacade.GetEmployeePtoAccrualRate(Days, PayType);

                if (PtoAccrualRate != null)
                {
                    if (Days >= PtoAccrualRate.MinimumDay && Days <= PtoAccrualRate.MaximumDay)
                    {
                        Rate = PtoAccrualRate.AccrualRate;
                    }
                }
            }
            
            return Rate;
        }
         
        public JsonResult SaveEmployeePtohoursschedule()
        {
            bool result = false;
            var message = "Employee pto hour not save successfully";
            var Today =  DateTime.Now.AddDays(3);
            double? PtoRate = 0.0;  
            if ((int)Today.DayOfWeek == 0)
            {
                DateTime FromDay = Today.AddDays(-14).SetZeroHour();
                DateTime EndDay = FromDay.AddDays(6).SetZeroHour(); 
                List<Employee> list = _Util.Facade.EmployeeFacade.GetAllEmployeeByHireDate(FromDay,EndDay);  
                if (list != null && list.Count > 0)
                {
                    foreach (var hireitem in list)
                    {
                            TimeSpan difference = EndDay - hireitem.HireDate.Value;
                            int totalDays = difference.Days;
                            PtoRate = GetPtoAccrualRate(totalDays, hireitem.PayType);
                            EmployeePTOHourLog employeePTOHourLog = new EmployeePTOHourLog()
                            {
                                UserId = hireitem.UserId,
                                FromDate = FromDay,
                                EndDate = EndDay,
                                PTOHour = PtoRate,
                                LastUpdatedDate = DateTime.UtcNow,
                                CreatedDate = DateTime.UtcNow,
                            };
                            result = _Util.Facade.EmployeeFacade.InsertEmployeePTOHourLog(employeePTOHourLog) > 0;
                            if(result)
                            {
                                var ptodatas = _Util.Facade.EmployeeFacade.GetEmployeeTotalPtoHour(hireitem.UserId).FirstOrDefault();
                                hireitem.PtoHour = ptodatas.TotalPTOHour;
                                _Util.Facade.EmployeeFacade.UpdateEmployee(hireitem);
                            }
                            message = "Employee pto hour save successfully";
                    }
                } 
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion Employe Pto Hours
        public JsonResult LineNumberTest()
        {

            int lineNumber = (new System.Diagnostics.StackFrame(0, true)).GetFileLineNumber();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            return Json(true, JsonRequestBehavior.AllowGet);

        }

        public JsonResult RegexText(string T)
        {
            string strRegex = "^\\([0-9]{3}\\) [0-9]{3}-[0-9]{4}$";
            //string strRegex = @"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9]
            //    {2}[0-9]{8}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)";

            // Class Regex Represents an
            // immutable regular expression.
            //   Format                Pattern
            // xxxxxxxxxx           ^[0 - 9]{ 10}$
            // +xx xx xxxxxxxx     ^\+[0 - 9]{ 2}\s +[0 - 9]{ 2}\s +[0 - 9]{ 8}$
            // xxx - xxxx - xxxx   ^[0 - 9]{ 3} -[0 - 9]{ 4}-[0 - 9]{ 4}$
            Regex re = new Regex(strRegex);

            // The IsMatch method is used to validate
            // a string or to ensure that a string
            // conforms to a particular pattern.
            if (re.IsMatch(T))
                return Json(1, JsonRequestBehavior.AllowGet);
            else
                return Json(0, JsonRequestBehavior.AllowGet);
             
        }
        public JsonResult apiTEST()
        {
            var client = new RestClient("http://api-rug.rmrcloud.com/SaveCustomer");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "d2a7fa25-4c68-1f71-8127-daa403b7f950");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("leadsource", "Protect America");
            request.AddHeader("abortcode", "abc123");
            request.AddHeader("leadstatus", "Appointment");
            request.AddHeader("customerstatus", "LEAD");
            request.AddHeader("saleslocation", "Retention");
            request.AddHeader("ownership", "Clean-Up");
            request.AddHeader("alternateid", "AlternateId");
            request.AddHeader("csaccountnumber", "CSAccountNumber");
            request.AddHeader("csprovider", "CSProvider");
            request.AddHeader("county", "Providance");
            request.AddHeader("ssn", "SSN");
            request.AddHeader("dba", "Doing Business As");
            request.AddHeader("firstname", "Test");
            request.AddHeader("lastname", "Inan 34");
            request.AddHeader("businessname", "Inan 34");
            request.AddHeader("type", "Commercial");
            request.AddHeader("primaryphone", "+88017415440");
            request.AddHeader("secondaryphone", "+8801521445444");
            request.AddHeader("cellno", "+8801303755444");
            request.AddHeader("email", "inan2454@inanbd.me");
            request.AddHeader("zipcode", "02907");
            request.AddHeader("state", "RI");
            request.AddHeader("city", "Providance");
            request.AddHeader("street", "233 Pearl St.");
            request.AddHeader("description", "Test Lead From API");
            request.AddHeader("islead", "false");
            request.AddHeader("authorization", "bearer XorNK0tyXMteMXCtEnKxKD8NY4LwwwWD6O_Kh7Bd6GqzeBeFXa63BV0GEMQkVehU6fdDlmilOVcEyqq7woJ-D192GQdID3_yhtNTM1Q18jDE0VjQ6yyRJ_03tF5HPhs7-XfODpFwxh5wRqpC6ndgzvQbWwC08l8RnP6kpem5rh3LfTkckpSDMlpCoqR0H66bH3sSqx4o4C0aSreVhiLl_gW88-rHmhXlD_h4Ke2_bkCdqqL9JinF9IT-JDUQjofoSa2LUN2J0e4Hsu9OgrLyOMVWjanBEGp3f6dg79I-64PTnTOQ9q-dqE8xyt8eqjMr3nwdOtQad9_-0hfZGVB0ixn9xdv3ioYe-y3qZCPgDV6HNxYJohSixbQGa_KWzlId2caPDohoOizn6NKBXROU7hwG0hd0zfeLYtja9xuDXqU5Gz_urpmAbD02vPTAu0ViK1xidyMyH0xdsYCepgl2SyBxIOs-x3KmNLkmeLXimZo");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            IRestResponse response = client.Execute(request);


            Double test = Convert.ToDouble("1,000.00");


            return Json(1);
        }
        public JsonResult JupiterTest()
        {

            string response = @"{""Data"":[{""Leads"":[{""id"":""965965"",""timestamp"":""2020-08-18 08:40:39 pm"",""firstName"":""DANIELS"",""lastName"":""RON"",""phone"":"" + 18326207699"",""email"":"""",""street"":"""",""city"":""HOUSTON"",""state"":""TX"",""zip"":""77069"",""comment"":"""",""notes"":[],""campaignID"":""0"",""campaign"":""SEO"",""ServiceType"":null,""country"":"""",""formType"":""phone"",""callDuration"":""32"",""recordingUrl"":""https:\/\/ api.twilio.com\/ 2010-04-01\/ Accounts\/ AC3117046b0d0ef51f1adfd53df40e9842\/ Recordings\/ RE3ae331b093f06dfb9b85cf94bcb05122"",""valid"":""1"",""billable"":""1"",""leadCapturedOn"":""0000-00-00"",""soldStatus"":""0"",""isAppointmentSet"":""0"",""appointmentDate"":""0000-00-00""},{""id"":""965860"",""timestamp"":""2020-08-18 07:17:02 pm"",""firstName"":""JSB"",""lastName"":""AND ASSOCIA"",""phone"":"" + 12814999003"",""email"":"""",""street"":"""",""city"":""STAFFORD"",""state"":""TX"",""zip"":""77489"",""comment"":"""",""notes"":[],""campaignID"":""0"",""campaign"":""SEO"",""ServiceType"":null,""country"":"""",""formType"":""phone"",""callDuration"":""81"",""recordingUrl"":""https:\/\/ api.twilio.com\/ 2010-04-01\/ Accounts\/ AC3117046b0d0ef51f1adfd53df40e9842\/ Recordings\/ REd26f060b307205dee6313b78341cd2b6"",""valid"":""1"",""billable"":""1"",""leadCapturedOn"":""0000-00-00"",""soldStatus"":""0"",""isAppointmentSet"":""0"",""appointmentDate"":""0000-00-00""},{""id"":""965840"",""timestamp"":""2020-08-18 07:00:27 pm"",""firstName"":""NESBIT"",""lastName"":""MEMORIAL"",""phone"":"" + 19797323392"",""email"":"""",""street"":"""",""city"":""COLUMBUS"",""state"":""TX"",""zip"":""78950"",""comment"":"""",""notes"":[],""campaignID"":""0"",""campaign"":""SEO"",""ServiceType"":null,""country"":"""",""formType"":""phone"",""callDuration"":""170"",""recordingUrl"":""https:\/\/ api.twilio.com\/ 2010-04-01\/ Accounts\/ AC3117046b0d0ef51f1adfd53df40e9842\/ Recordings\/ REe12d2320234472fc277894aec003357b"",""valid"":""1"",""billable"":""1"",""leadCapturedOn"":""0000-00-00"",""soldStatus"":""0"",""isAppointmentSet"":""0"",""appointmentDate"":""0000-00-00""},{""id"":""965783"",""timestamp"":""2020-08-18 06:24:53 pm"",""firstName"":""PAMELA"",""lastName"":""ZUNIGA"",""phone"":"" + 18326050693"",""email"":"""",""street"":"""",""city"":""Houston"",""state"":""TX"",""zip"":""77097"",""comment"":"""",""notes"":[{""noteID"":""178850"",""userId"":""47"",""userName"":""Allen Alam"",""userEmail"":""rabfire @gmail.com"",""comment"":""Sales Call. RB."",""dateTime"":""2020-08-18 06:33:59 pm""}],""campaignID"":""0"",""campaign"":""SEO"",""ServiceType"":null,""country"":"""",""formType"":""phone"",""callDuration"":""69"",""recordingUrl"":""https:\/\/ api.twilio.com\/ 2010-04-01\/ Accounts\/ AC3117046b0d0ef51f1adfd53df40e9842\/ Recordings\/ REa99c31b5dfa32c5db74d677da107edaf"",""valid"":""1"",""billable"":""0"",""leadCapturedOn"":""0000-00-00"",""soldStatus"":""0"",""isAppointmentSet"":""0"",""appointmentDate"":""0000-00-00""},{""id"":""965672"",""timestamp"":""2020-08-18 05:12:05 pm"",""firstName"":""JSB"",""lastName"":""AND ASSOCIA"",""phone"":"" + 12814999003"",""email"":"""",""street"":"""",""city"":""STAFFORD"",""state"":""TX"",""zip"":""77489"",""comment"":"""",""notes"":[],""campaignID"":""0"",""campaign"":""SEO"",""ServiceType"":null,""country"":"""",""formType"":""phone"",""callDuration"":""32"",""recordingUrl"":""https:\/\/ api.twilio.com\/ 2010-04-01\/ Accounts\/ AC3117046b0d0ef51f1adfd53df40e9842\/ Recordings\/ REb349656b1b0382a4c0402baa33b90793"",""valid"":""1"",""billable"":""1"",""leadCapturedOn"":""0000-00-00"",""soldStatus"":""0"",""isAppointmentSet"":""0"",""appointmentDate"":""0000-00-00""},{""id"":""965541"",""timestamp"":""2020-08-18 03:27:56 pm"",""firstName"":""EVANS"",""lastName"":""KIMBERLY"",""phone"":"" + 18326000004"",""email"":"""",""street"":"""",""city"":""HOUSTON"",""state"":""TX"",""zip"":""77098"",""comment"":"""",""notes"":[],""campaignID"":""0"",""campaign"":""SEO"",""ServiceType"":null,""country"":"""",""formType"":""phone"",""callDuration"":""200"",""recordingUrl"":""https:\/\/ api.twilio.com\/ 2010-04-01\/ Accounts\/ AC3117046b0d0ef51f1adfd53df40e9842\/ Recordings\/ REf0822c101ea4369316267efc7364cea3"",""valid"":""1"",""billable"":""1"",""leadCapturedOn"":""0000-00-00"",""soldStatus"":""0"",""isAppointmentSet"":""0"",""appointmentDate"":""0000-00-00""},{""id"":""965335"",""timestamp"":""2020-08-18 01:16:46 pm"",""firstName"":""JSB"",""lastName"":""AND ASSOCIA"",""phone"":"" + 12814999003"",""email"":"""",""street"":"""",""city"":""STAFFORD"",""state"":""TX"",""zip"":""77489"",""comment"":"""",""notes"":[],""campaignID"":""0"",""campaign"":""SEO"",""ServiceType"":null,""country"":"""",""formType"":""phone"",""callDuration"":""75"",""recordingUrl"":""https:\/\/ api.twilio.com\/ 2010-04-01\/ Accounts\/ AC3117046b0d0ef51f1adfd53df40e9842\/ Recordings\/ REace3efdb666e5b7cdc98131accff481d"",""valid"":""1"",""billable"":""1"",""leadCapturedOn"":""0000-00-00"",""soldStatus"":""0"",""isAppointmentSet"":""0"",""appointmentDate"":""0000-00-00""}]},{""Notes"":[{""Lead ID"":""965226"",""notes"":[{""noteID"":""178762"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 11:22:30 am""}]},{""Lead ID"":""965195"",""notes"":[{""noteID"":""178763"",""userId"":""1329"",""userName"":""Brenda Johnson"",""userEmail"":""brenda @basementandradonsolutions.com"",""comment"":null,""dateTime"":""2020-08-18 12:15:30 pm""}]},{""Lead ID"":""964493"",""notes"":[{""noteID"":""178764"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 12:36:45 pm""}]},{""Lead ID"":""964502"",""notes"":[{""noteID"":""178765"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 12:42:51 pm""}]},{""Lead ID"":""962191"",""notes"":[{""noteID"":""178343"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-13 06:04:55 pm""},{""noteID"":""178497"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-14 08:07:29 pm""},{""noteID"":""178766"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 12:50:53 pm""}]},{""Lead ID"":""964501"",""notes"":[{""noteID"":""178767"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 12:55:05 pm""}]},{""Lead ID"":""964522"",""notes"":[{""noteID"":""178768"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 12:55:32 pm""}]},{""Lead ID"":""963088"",""notes"":[{""noteID"":""178436"",""userId"":""956"",""userName"":""Tracy Martin"",""userEmail"":""tracy.martin @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-14 04:45:41 pm""},{""noteID"":""178629"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 02:52:53 pm""},{""noteID"":""178769"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 01:10:07 pm""}]},{""Lead ID"":""964524"",""notes"":[{""noteID"":""178770"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 01:17:36 pm""}]},{""Lead ID"":""964542"",""notes"":[{""noteID"":""178771"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 01:21:50 pm""}]},{""Lead ID"":""965220"",""notes"":[{""noteID"":""178772"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 01:23:52 pm""},{""noteID"":""178795"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 02:57:37 pm""}]},{""Lead ID"":""964354"",""notes"":[{""noteID"":""178622"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 02:47:34 pm""},{""noteID"":""178773"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 01:28:02 pm""},{""noteID"":""178802"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 03:21:38 pm""}]},{""Lead ID"":""965210"",""notes"":[{""noteID"":""178774"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 01:30:25 pm""}]},{""Lead ID"":""964556"",""notes"":[{""noteID"":""178775"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 01:42:48 pm""}]},{""Lead ID"":""964637"",""notes"":[{""noteID"":""178776"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 01:43:13 pm""}]},{""Lead ID"":""964673"",""notes"":[{""noteID"":""178777"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 01:46:44 pm""}]},{""Lead ID"":""964715"",""notes"":[{""noteID"":""178778"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 01:47:13 pm""}]},{""Lead ID"":""964705"",""notes"":[{""noteID"":""178779"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 01:47:30 pm""}]},{""Lead ID"":""965219"",""notes"":[{""noteID"":""178780"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 01:48:31 pm""}]},{""Lead ID"":""964749"",""notes"":[{""noteID"":""178781"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 01:49:07 pm""}]},{""Lead ID"":""964754"",""notes"":[{""noteID"":""178782"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 01:49:37 pm""}]},{""Lead ID"":""964760"",""notes"":[{""noteID"":""178783"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 01:51:47 pm""}]},{""Lead ID"":""965324"",""notes"":[{""noteID"":""178784"",""userId"":""1105"",""userName"":""James Keegan Sr"",""userEmail"":""jamesk5595 @aol.com"",""comment"":null,""dateTime"":""2020-08-18 01:51:48 pm""}]},{""Lead ID"":""965363"",""notes"":[{""noteID"":""178785"",""userId"":""3073"",""userName"":""Elizabeth Reagles"",""userEmail"":""elizabeth @brspros.net"",""comment"":null,""dateTime"":""2020-08-18 01:53:13 pm""},{""noteID"":""178786"",""userId"":""3073"",""userName"":""Elizabeth Reagles"",""userEmail"":""elizabeth @brspros.net"",""comment"":null,""dateTime"":""2020-08-18 01:53:30 pm""}]},{""Lead ID"":""962587"",""notes"":[{""noteID"":""178542"",""userId"":""1329"",""userName"":""Brenda Johnson"",""userEmail"":""brenda @basementandradonsolutions.com"",""comment"":null,""dateTime"":""2020-08-15 01:02:12 pm""},{""noteID"":""178669"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-17 07:17:32 pm""},{""noteID"":""178787"",""userId"":""3073"",""userName"":""Elizabeth Reagles"",""userEmail"":""elizabeth @brspros.net"",""comment"":null,""dateTime"":""2020-08-18 01:53:56 pm""}]},{""Lead ID"":""965290"",""notes"":[{""noteID"":""178788"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 01:55:34 pm""},{""noteID"":""178801"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 03:20:17 pm""}]},{""Lead ID"":""964789"",""notes"":[{""noteID"":""178789"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 01:56:55 pm""}]},{""Lead ID"":""963398"",""notes"":[{""noteID"":""178473"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-14 06:35:10 pm""},{""noteID"":""178790"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 01:58:46 pm""}]},{""Lead ID"":""960911"",""notes"":[{""noteID"":""177989"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-11 06:14:33 pm""},{""noteID"":""178108"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-12 02:21:42 pm""},{""noteID"":""178340"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-13 05:59:46 pm""},{""noteID"":""178364"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-13 06:44:59 pm""},{""noteID"":""178626"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-17 02:51:14 pm""},{""noteID"":""178627"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-17 02:51:29 pm""},{""noteID"":""178628"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-17 02:51:44 pm""},{""noteID"":""178704"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-17 08:57:47 pm""},{""noteID"":""178705"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-17 08:58:28 pm""},{""noteID"":""178791"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 02:07:38 pm""}]},{""Lead ID"":""963985"",""notes"":[{""noteID"":""178632"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 02:56:02 pm""},{""noteID"":""178643"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 03:17:53 pm""},{""noteID"":""178648"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-17 03:43:52 pm""},{""noteID"":""178792"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 02:12:57 pm""}]},{""Lead ID"":""965410"",""notes"":[{""noteID"":""178793"",""userId"":""669"",""userName"":""Leslie Chaffie"",""userEmail"":""lchaffie @ecospect.com"",""comment"":null,""dateTime"":""2020-08-18 02:22:31 pm""}]},{""Lead ID"":""964787"",""notes"":[{""noteID"":""178794"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 02:24:24 pm""}]},{""Lead ID"":""961770"",""notes"":[{""noteID"":""178142"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-12 06:30:34 pm""},{""noteID"":""178303"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-13 02:13:21 pm""},{""noteID"":""178796"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 03:01:34 pm""}]},{""Lead ID"":""964066"",""notes"":[{""noteID"":""178614"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 02:41:50 pm""},{""noteID"":""178650"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 04:08:43 pm""},{""noteID"":""178797"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 03:02:52 pm""}]},{""Lead ID"":""964314"",""notes"":[{""noteID"":""178584"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 01:17:39 pm""},{""noteID"":""178798"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 03:09:07 pm""}]},{""Lead ID"":""963760"",""notes"":[{""noteID"":""178600"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 02:13:16 pm""},{""noteID"":""178799"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 03:10:04 pm""}]},{""Lead ID"":""963807"",""notes"":[{""noteID"":""178602"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 02:14:44 pm""},{""noteID"":""178800"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 03:12:29 pm""}]},{""Lead ID"":""964791"",""notes"":[{""noteID"":""178803"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 03:26:43 pm""}]},{""Lead ID"":""964814"",""notes"":[{""noteID"":""178804"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 03:27:29 pm""}]},{""Lead ID"":""964881"",""notes"":[{""noteID"":""178805"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 03:28:10 pm""}]},{""Lead ID"":""964944"",""notes"":[{""noteID"":""178806"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 03:29:03 pm""}]},{""Lead ID"":""964945"",""notes"":[{""noteID"":""178807"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 03:29:16 pm""}]},{""Lead ID"":""964960"",""notes"":[{""noteID"":""178808"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 03:32:43 pm""}]},{""Lead ID"":""964073"",""notes"":[{""noteID"":""178638"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 03:05:28 pm""},{""noteID"":""178809"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 03:36:22 pm""},{""noteID"":""178820"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 04:05:13 pm""}]},{""Lead ID"":""964083"",""notes"":[{""noteID"":""178639"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 03:09:07 pm""},{""noteID"":""178810"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 03:37:12 pm""},{""noteID"":""178825"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 04:21:11 pm""}]},{""Lead ID"":""961473"",""notes"":[{""noteID"":""178110"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-12 02:44:02 pm""},{""noteID"":""178811"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 03:38:47 pm""},{""noteID"":""178827"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 04:28:10 pm""}]},{""Lead ID"":""964271"",""notes"":[{""noteID"":""178641"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 03:11:52 pm""},{""noteID"":""178812"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 03:40:21 pm""}]},{""Lead ID"":""965293"",""notes"":[{""noteID"":""178813"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 03:43:14 pm""}]},{""Lead ID"":""964981"",""notes"":[{""noteID"":""178814"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 03:44:25 pm""}]},{""Lead ID"":""965423"",""notes"":[{""noteID"":""178815"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 03:57:59 pm""},{""noteID"":""178834"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 04:48:34 pm""}]},{""Lead ID"":""961709"",""notes"":[{""noteID"":""178816"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 03:59:55 pm""}]},{""Lead ID"":""961716"",""notes"":[{""noteID"":""178817"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:02:15 pm""}]},{""Lead ID"":""961732"",""notes"":[{""noteID"":""178818"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:03:53 pm""}]},{""Lead ID"":""961733"",""notes"":[{""noteID"":""178819"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:04:22 pm""}]},{""Lead ID"":""961741"",""notes"":[{""noteID"":""178821"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:07:30 pm""}]},{""Lead ID"":""961745"",""notes"":[{""noteID"":""178822"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:08:35 pm""}]},{""Lead ID"":""961759"",""notes"":[{""noteID"":""178823"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:11:40 pm""}]},{""Lead ID"":""961771"",""notes"":[{""noteID"":""178824"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:13:13 pm""}]},{""Lead ID"":""961778"",""notes"":[{""noteID"":""178826"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:24:00 pm""}]},{""Lead ID"":""961779"",""notes"":[{""noteID"":""178828"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:29:15 pm""}]},{""Lead ID"":""961795"",""notes"":[{""noteID"":""178829"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:35:44 pm""}]},{""Lead ID"":""961801"",""notes"":[{""noteID"":""178830"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:36:23 pm""}]},{""Lead ID"":""961802"",""notes"":[{""noteID"":""178831"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:37:02 pm""}]},{""Lead ID"":""961812"",""notes"":[{""noteID"":""178832"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:42:30 pm""}]},{""Lead ID"":""961818"",""notes"":[{""noteID"":""178833"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:47:39 pm""}]},{""Lead ID"":""961814"",""notes"":[{""noteID"":""178835"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 04:48:39 pm""}]},{""Lead ID"":""965640"",""notes"":[{""noteID"":""178836"",""userId"":""669"",""userName"":""Leslie Chaffie"",""userEmail"":""lchaffie @ecospect.com"",""comment"":null,""dateTime"":""2020-08-18 04:49:54 pm""}]},{""Lead ID"":""964993"",""notes"":[{""noteID"":""178837"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 05:04:51 pm""}]},{""Lead ID"":""964200"",""notes"":[{""noteID"":""178653"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 04:20:48 pm""},{""noteID"":""178838"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 05:06:00 pm""},{""noteID"":""178870"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 09:26:06 pm""},{""noteID"":""178880"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 11:14:29 pm""}]},{""Lead ID"":""965013"",""notes"":[{""noteID"":""178839"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 05:07:34 pm""}]},{""Lead ID"":""965239"",""notes"":[{""noteID"":""178840"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 05:07:41 pm""}]},{""Lead ID"":""965059"",""notes"":[{""noteID"":""178841"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 05:09:10 pm""}]},{""Lead ID"":""965075"",""notes"":[{""noteID"":""178842"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 05:09:31 pm""}]},{""Lead ID"":""965082"",""notes"":[{""noteID"":""178843"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 05:10:17 pm""}]},{""Lead ID"":""965108"",""notes"":[{""noteID"":""178844"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 05:12:39 pm""}]},{""Lead ID"":""965115"",""notes"":[{""noteID"":""178845"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 05:13:30 pm""}]},{""Lead ID"":""965162"",""notes"":[{""noteID"":""178846"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 05:13:50 pm""}]},{""Lead ID"":""965165"",""notes"":[{""noteID"":""178847"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 05:14:03 pm""}]},{""Lead ID"":""965258"",""notes"":[{""noteID"":""178848"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 05:15:09 pm""}]},{""Lead ID"":""965311"",""notes"":[{""noteID"":""178849"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 05:16:38 pm""}]},{""Lead ID"":""965783"",""notes"":[{""noteID"":""178850"",""userId"":""47"",""userName"":""Allen Alam"",""userEmail"":""rabfire @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 06:33:59 pm""}]},{""Lead ID"":""965315"",""notes"":[{""noteID"":""178851"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 06:43:01 pm""}]},{""Lead ID"":""961821"",""notes"":[{""noteID"":""178852"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 07:05:16 pm""}]},{""Lead ID"":""965409"",""notes"":[{""noteID"":""178853"",""userId"":""3192"",""userName"":""Bianca Radice"",""userEmail"":""biancajulietradish @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 07:17:55 pm""}]},{""Lead ID"":""963328"",""notes"":[{""noteID"":""178854"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 07:24:36 pm""}]},{""Lead ID"":""963224"",""notes"":[{""noteID"":""178855"",""userId"":""3073"",""userName"":""Elizabeth Reagles"",""userEmail"":""elizabeth @brspros.net"",""comment"":null,""dateTime"":""2020-08-18 07:49:03 pm""}]},{""Lead ID"":""964276"",""notes"":[{""noteID"":""178856"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 07:55:04 pm""}]},{""Lead ID"":""965338"",""notes"":[{""noteID"":""178857"",""userId"":""2745"",""userName"":""Vicky Hayon"",""userEmail"":""vicky @reedssprayfoam.com"",""comment"":null,""dateTime"":""2020-08-18 08:10:47 pm""}]},{""Lead ID"":""965348"",""notes"":[{""noteID"":""178858"",""userId"":""2745"",""userName"":""Vicky Hayon"",""userEmail"":""vicky @reedssprayfoam.com"",""comment"":null,""dateTime"":""2020-08-18 08:13:38 pm""}]},{""Lead ID"":""965362"",""notes"":[{""noteID"":""178859"",""userId"":""2556"",""userName"":""Angela Rotthoff"",""userEmail"":""angelarotthoff @hotmail.com"",""comment"":null,""dateTime"":""2020-08-18 08:53:54 pm""}]},{""Lead ID"":""965711"",""notes"":[{""noteID"":""178860"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 08:54:21 pm""}]},{""Lead ID"":""965488"",""notes"":[{""noteID"":""178861"",""userId"":""2556"",""userName"":""Angela Rotthoff"",""userEmail"":""angelarotthoff @hotmail.com"",""comment"":null,""dateTime"":""2020-08-18 08:54:27 pm""}]},{""Lead ID"":""965542"",""notes"":[{""noteID"":""178862"",""userId"":""2556"",""userName"":""Angela Rotthoff"",""userEmail"":""angelarotthoff @hotmail.com"",""comment"":null,""dateTime"":""2020-08-18 08:55:03 pm""}]},{""Lead ID"":""961829"",""notes"":[{""noteID"":""178863"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 08:55:16 pm""}]},{""Lead ID"":""965550"",""notes"":[{""noteID"":""178864"",""userId"":""2556"",""userName"":""Angela Rotthoff"",""userEmail"":""angelarotthoff @hotmail.com"",""comment"":null,""dateTime"":""2020-08-18 08:55:24 pm""}]},{""Lead ID"":""965627"",""notes"":[{""noteID"":""178865"",""userId"":""2556"",""userName"":""Angela Rotthoff"",""userEmail"":""angelarotthoff @hotmail.com"",""comment"":null,""dateTime"":""2020-08-18 08:56:30 pm""}]},{""Lead ID"":""965661"",""notes"":[{""noteID"":""178866"",""userId"":""2556"",""userName"":""Angela Rotthoff"",""userEmail"":""angelarotthoff @hotmail.com"",""comment"":null,""dateTime"":""2020-08-18 08:56:45 pm""}]},{""Lead ID"":""965768"",""notes"":[{""noteID"":""178867"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 08:56:49 pm""}]},{""Lead ID"":""965765"",""notes"":[{""noteID"":""178868"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 08:59:21 pm""}]},{""Lead ID"":""965653"",""notes"":[{""noteID"":""178869"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-18 09:01:16 pm""}]},{""Lead ID"":""961833"",""notes"":[{""noteID"":""178871"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 10:54:07 pm""}]},{""Lead ID"":""961852"",""notes"":[{""noteID"":""178872"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 10:55:51 pm""}]},{""Lead ID"":""963776"",""notes"":[{""noteID"":""178651"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 04:17:11 pm""},{""noteID"":""178873"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 10:56:09 pm""}]},{""Lead ID"":""963782"",""notes"":[{""noteID"":""178652"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 04:19:35 pm""},{""noteID"":""178874"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-18 10:57:29 pm""}]},{""Lead ID"":""961864"",""notes"":[{""noteID"":""178875"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 10:58:33 pm""}]},{""Lead ID"":""961875"",""notes"":[{""noteID"":""178876"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 11:02:50 pm""}]},{""Lead ID"":""961894"",""notes"":[{""noteID"":""178877"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 11:03:40 pm""}]},{""Lead ID"":""961906"",""notes"":[{""noteID"":""178878"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 11:05:26 pm""}]},{""Lead ID"":""961914"",""notes"":[{""noteID"":""178879"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-18 11:05:49 pm""}]},{""Lead ID"":""961908"",""notes"":[{""noteID"":""178881"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-19 12:19:56 am""}]},{""Lead ID"":""961922"",""notes"":[{""noteID"":""178882"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-19 12:21:19 am""}]},{""Lead ID"":""961924"",""notes"":[{""noteID"":""178883"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-19 12:25:52 am""}]},{""Lead ID"":""961939"",""notes"":[{""noteID"":""178884"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-19 12:27:17 am""}]},{""Lead ID"":""961942"",""notes"":[{""noteID"":""178885"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-19 12:28:30 am""}]},{""Lead ID"":""961947"",""notes"":[{""noteID"":""178886"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-19 12:45:11 am""}]},{""Lead ID"":""961955"",""notes"":[{""noteID"":""178887"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-19 12:55:45 am""}]},{""Lead ID"":""961961"",""notes"":[{""noteID"":""178888"",""userId"":""215"",""userName"":""Shayra"",""userEmail"":""pritu0909 @gmail.com"",""comment"":null,""dateTime"":""2020-08-19 01:02:22 am""}]},{""Lead ID"":""964485"",""notes"":[{""noteID"":""178649"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 03:53:27 pm""},{""noteID"":""178655"",""userId"":""945"",""userName"":""Kathy Dizek"",""userEmail"":""Kathy.Dizek @aeroseal.com"",""comment"":null,""dateTime"":""2020-08-17 05:02:44 pm""},{""noteID"":""178889"",""userId"":""0"",""userName"":null,""userEmail"":null,""comment"":null,""dateTime"":""2020-08-19 01:24:03 am""}]}]}]}";

            JupiterResponse result = Newtonsoft.Json.JsonConvert.DeserializeObject<JupiterResponse>(response);

            return Json(1);
        }
        #region Forte Test 
        [Authorize]
        public JsonResult UpdateForteCustomerSubs()
        {

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetForteTransactions()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            Guid CompanyId = CurrentUser.CompanyId.Value;
            HS.Facade.SchedulerFacade SchedulerFacade = new HS.Facade.SchedulerFacade("Data Source=localhost;Initial Catalog=RMRDemo2;Integrated Security=True;Max Pool Size=600");

            bool ForteInProduction = false;
            GlobalSetting globset2 = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("ForteInProduction", CompanyId);
            if (globset2 != null && globset2.Value.ToLower() == "true")
            {
                ForteInProduction = true;
            }

            string ForteTransactionKey = SchedulerFacade.GetForteTransactionKeyByCompanyId(CompanyId, false);
            string ForteAPILoginId = SchedulerFacade.GetForteAPILoginIdByCompanyId(CompanyId, false);
            string ForteOrganizationId = SchedulerFacade.GetForteOrganizationIdByCompanyId(CompanyId, false);
            string ForteLocationId = SchedulerFacade.GetForteLocationIdByCompanyId(CompanyId, false);
            string ForteAuthAccountId = SchedulerFacade.GetForteAuthAccountIdByCompanyId(CompanyId, false);

            ForteOptions forte = new ForteOptions();
            forte.Organization_ID = ForteOrganizationId;
            forte.Location_Id = ForteLocationId;
            forte.AuthAccountId = ForteAuthAccountId;

            forte.UserId = ForteAPILoginId;
            forte.Password = ForteTransactionKey;

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3
                               | System.Net.SecurityProtocolType.Tls
                               | System.Net.SecurityProtocolType.Tls11
                               | System.Net.SecurityProtocolType.Tls12;

            if (ForteInProduction == true)
            {
                forte.Server = "https://api.forte.net/v3";
            }
            else
            {
                forte.Server = "https://sandbox.forte.net/api/v3";
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateForteSubs()
        {
            #region Prduction or Live
            bool ForteInProduction = true;

            #endregion

            #region Forte 
            #region Thompson
            string ForteTransactionKey = "3f0e0a3e812d112f7efa8f23f9ef42fb";
            string ForteAPILoginId = "7a5c830aa9a6c65ea150c85eeff29778";
            string ForteOrganizationId = "org_390720";
            string ForteLocationId = "loc_253561";
            string ForteAuthAccountId = "act_390720";
            #endregion
            #region Mine 
            //string ForteTransactionKey = "5f63629278d9662f910abba0d728b3b4";
            //string ForteAPILoginId = "66385ca85ee77fcb5624d73829820009";
            //string ForteOrganizationId = "org_386333";
            //string ForteLocationId = "loc_247889";
            //string ForteAuthAccountId = "act_367599";
            #endregion

            ForteOptions forte = new ForteOptions();
            forte.Organization_ID = ForteOrganizationId;
            forte.Location_Id = ForteLocationId;
            forte.AuthAccountId = ForteAuthAccountId;

            forte.UserId = ForteAPILoginId;
            forte.Password = ForteTransactionKey;

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3
                               | System.Net.SecurityProtocolType.Tls
                               | System.Net.SecurityProtocolType.Tls11
                               | System.Net.SecurityProtocolType.Tls12;

            if (ForteInProduction == true)
            {
                forte.Server = "https://api.forte.net/v3";
            }
            else
            {
                forte.Server = "https://sandbox.forte.net/api/v3";
            }



            #endregion
            List<Customer> customers = _Util.Facade.CustomerFacade.GetForteSubscribedCustomerForSync();
            foreach (var item in customers)
            {
                forte.Schedule_Id = item.ScheduleToken;

                #region Schedule
                ForteSchedule forteSchedule = new ForteSchedule();
                ForteScheduleService forteScheduleService = new ForteScheduleService(forte);
                ForteScheduleResponse forteScheduleResponse = new ForteScheduleResponse();
                try
                {
                    FortePaymentGetwayResponse response = forteScheduleService.GetSchedule();
                    forteScheduleResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ForteScheduleResponse>(response.Massege);

                    #region Bill Cycle
                    item.BillCycle = forteScheduleResponse.schedule_frequency;
                    if (forteScheduleResponse.schedule_frequency == "monthly")
                    {
                        item.BillCycle = "Monthly";

                    }
                    else
                    {

                    }

                    #endregion

                    #region Total Amount
                    item.BillAmount = Math.Round(forteScheduleResponse.schedule_amount, 2);
                    item.BillTax = false;
                    item.MonthlyMonitoringFee = forteScheduleResponse.schedule_amount.ToString();
                    //item.PaymentToken

                    #endregion

                    #region Payment Method
                    item.PaymentMethod = "Credit Card";
                    #endregion
                    #region Billing Start Date
                    item.FirstBilling = forteScheduleResponse.schedule_start_date;
                    #endregion
                    _Util.Facade.CustomerFacade.UpdateCustomer(item);


                }
                catch (Exception ex)
                {

                }

                #endregion

            }


            //forte.Customer_Token = "zH9deq4F_0qmuYZDI5TGtA"; //"cst_UXpoI-opOUCHvK7z84CIKA";
            //forte.Schedule_Id = "3f349eab-725c-43ef-99c5-eb403ffe8039"; //token;






            #region Transaction
            ForteTransaction forteTrans = new ForteTransaction();
            ForteTransactionService forteTransaction = new ForteTransactionService(forte);
            ForteTransectionResponse transResponse = new ForteTransectionResponse();
            try
            {
                // FortePaymentGetwayResponse response2 = forteTransaction.GetTransaction();
                //transResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ForteTransectionResponse>(response2.Massege);


            }
            #endregion

            catch (Exception) { }
            return Json(null);

        }

        public JsonResult ForteTransactionResp(string token)
        {


            #region Prduction or Live
            bool ForteInProduction = false;

            #endregion

            #region Forte 
            #region Thompson
            //string ForteTransactionKey = "3f0e0a3e812d112f7efa8f23f9ef42fb";
            //string ForteAPILoginId = "7a5c830aa9a6c65ea150c85eeff29778";
            //string ForteOrganizationId = "org_390720";
            //string ForteLocationId = "loc_253561";
            //string ForteAuthAccountId = "act_390720";
            #endregion
            #region Mine 
            string ForteTransactionKey = "5f63629278d9662f910abba0d728b3b4";
            string ForteAPILoginId = "66385ca85ee77fcb5624d73829820009";
            string ForteOrganizationId = "org_386333";
            string ForteLocationId = "loc_247889";
            string ForteAuthAccountId = "act_367599";
            #endregion

            ForteOptions forte = new ForteOptions();
            forte.Organization_ID = ForteOrganizationId;
            forte.Location_Id = ForteLocationId;
            forte.AuthAccountId = ForteAuthAccountId;

            forte.UserId = ForteAPILoginId;
            forte.Password = ForteTransactionKey;

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3
                               | System.Net.SecurityProtocolType.Tls
                               | System.Net.SecurityProtocolType.Tls11
                               | System.Net.SecurityProtocolType.Tls12;

            if (ForteInProduction == true)
            {
                forte.Server = "https://api.forte.net/v3";
            }
            else
            {
                forte.Server = "https://sandbox.forte.net/api/v3";
            }


            #endregion
            //forte.Customer_Token = "zH9deq4F_0qmuYZDI5TGtA"; //"cst_UXpoI-opOUCHvK7z84CIKA";
            forte.Schedule_Id = "3f349eab-725c-43ef-99c5-eb403ffe8039"; //token;


            #region Schedule
            ForteSchedule forteSchedule = new ForteSchedule();
            ForteScheduleService forteScheduleService = new ForteScheduleService(forte);
            ForteScheduleResponse forteScheduleResponse = new ForteScheduleResponse();
            try
            {
                FortePaymentGetwayResponse response = forteScheduleService.GetSchedule();
                forteScheduleResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ForteScheduleResponse>(response.Massege);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }




            #endregion



            #region Transaction
            ForteTransaction forteTrans = new ForteTransaction();
            ForteTransactionService forteTransaction = new ForteTransactionService(forte);
            ForteTransectionResponse transResponse = new ForteTransectionResponse();
            try
            {
                // FortePaymentGetwayResponse response2 = forteTransaction.GetTransaction();
                //transResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ForteTransectionResponse>(response2.Massege);


            }
            #endregion

            catch (Exception) { }
            return Json(null);
        }
        #endregion

        #region Authorize.net Test
        [Authorize]
        public JsonResult UpdateAutorizeCustomerSubs(string TransactionId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            /*
            string TransactionKey = _Util.Facade.GlobalSettingsFacade.GetAuthTransactionKeyByCompanyId(CurrentUser.CompanyId.Value,true);
            string APILoginId = _Util.Facade.GlobalSettingsFacade.GetAuthAPILoginIdByCompanyId(CurrentUser.CompanyId.Value, true);

            ARBGetSubscriptionResponse response = GetSubscription.Run(APILoginId, TransactionKey, TransactionId, false);
            if (response.subscription != null)
            {
                
            }
            */

            List<Customer> SbscribedCustomer = _Util.Facade.CustomerFacade.GetAllAuthorizeCustomer();

            foreach (Customer customer in SbscribedCustomer)
            {
                string TransactionKey = _Util.Facade.GlobalSettingsFacade.GetAuthTransactionKeyByCompanyId(CurrentUser.CompanyId.Value, (customer.PaymentMethod == LabelHelper.PaymentMethod.ACH));
                string APILoginId = _Util.Facade.GlobalSettingsFacade.GetAuthAPILoginIdByCompanyId(CurrentUser.CompanyId.Value, (customer.PaymentMethod == LabelHelper.PaymentMethod.ACH));

                ARBGetSubscriptionResponse response = GetSubscription.Run(APILoginId, TransactionKey, customer.AuthorizeRefId, true);
                if (response.subscription != null)
                {
                    customer.SubscriptionStatus = response.subscription.status.ToString();
                    customer.BillAmount = (double)response.subscription.amount;
                    customer.MonthlyMonitoringFee = response.subscription.amount.ToString();
                    customer.BillTax = false;
                    if (response.subscription.profile != null)
                    {
                        customer.AuthorizeCusProfileId = response.subscription.profile.customerProfileId;
                        customer.AuthorizeDescription = response.subscription.profile.description;
                        if (response.subscription.profile.paymentProfile != null)
                        {
                            customer.AuthorizeCusPaymentProfileId = response.subscription.profile.paymentProfile.customerPaymentProfileId;

                            if (response.subscription.profile.paymentProfile.payment != null
                                && response.subscription.profile.paymentProfile.payment.Item != null)
                            {
                                Type type = response.subscription.profile.paymentProfile.payment.Item.GetType();
                                if (type == typeof(creditCardMaskedType))
                                {
                                    customer.PaymentMethod = LabelHelper.PaymentMethod.CreditCard;
                                }
                                if (type == typeof(bankAccountMaskedType))
                                {
                                    customer.PaymentMethod = LabelHelper.PaymentMethod.ACH;
                                }

                                //object item = response.subscription.profile.paymentProfile.payment.Item;
                                //object asd = ne
                                //item.GetType().GetProperty("cardNumber").GetValue(asd);
                                //var test2 = item.GetType().GetProperty("cardNumber");

                                //creditCardMaskedType cardMaskedTyp

                                //var card = Convert.ChangeType(item, typeof(PaymentMethodCard));
                            }

                            //response.subscription.profile.paymentProfile.payment.Item

                        }
                    }
                    if (response.subscription.paymentSchedule != null)
                    {
                        customer.FirstBilling = response.subscription.paymentSchedule.startDate;
                        if (response.subscription.paymentSchedule.interval.unit == ARBSubscriptionUnitEnum.months)
                        {
                            if (response.subscription.paymentSchedule.interval.length == 1)
                            {
                                customer.BillCycle = LabelHelper.BillCycle.Monthly;
                            }
                            else if (response.subscription.paymentSchedule.interval.length == 12)
                            {
                                customer.BillCycle = LabelHelper.BillCycle.Annually;
                            }
                            else if (response.subscription.paymentSchedule.interval.length == 6)
                            {
                                customer.BillCycle = LabelHelper.BillCycle.SemiAnnual;
                            }
                            else if (response.subscription.paymentSchedule.interval.length == 3)
                            {
                                customer.BillCycle = LabelHelper.BillCycle.Quarterly;
                            }
                        }
                    }
                    _Util.Facade.CustomerFacade.UpdateCustomer(customer);
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region General Test

        public string Zip(string str)
        {
            return HS.Framework.ZipUnzip.Zip(str);
        }

        public string Unzip(string str)
        {
            return HS.Framework.ZipUnzip.Unzip(str);
        }
        public JsonResult Phone(string P)
        {
            return Json(P.PhoneNumberFormat(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Date Test

        public JsonResult DateCheck3()
        {
            DateTime dateTime = new DateTime(2019, 1, 1);
            int Month = dateTime.Month;

            int modCheck = 12 % 3;
            int modCheck2 = 6 % 3;
            int modCheck23 = 3 % 3;

            int modCheck4 = 12 % 2;
            int modCheck6 = 6 % 2;
            int modCheck5 = 3 % 2;

            string asd = "2020-10-30";
            DateTime date = asd.ToDateTime();

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Agreement()
        {
            return View();
        }
        public JsonResult DateCheck2()
        {
            //int DayOfWeek = (int)DateTime.Now.AddDays(-4).DayOfWeek;
            //DayOfWeek = (int)DateTime.Now.AddDays(-3).DayOfWeek;
            //DayOfWeek = (int)DateTime.Now.AddDays(-2).DayOfWeek;
            //DayOfWeek = (int)DateTime.Now.AddDays(-1).DayOfWeek;
            //DayOfWeek = (int)DateTime.Now.DayOfWeek;
            //DayOfWeek = (int)DateTime.Now.AddDays(1).DayOfWeek;
            //DayOfWeek = (int)DateTime.Now.AddDays(2).DayOfWeek;
            //DayOfWeek = (int)DateTime.Now.AddDays(3).DayOfWeek;
            //DayOfWeek = (int)DateTime.Now.AddDays(4).DayOfWeek;
            //DayOfWeek = (int)DateTime.Now.AddDays(5).DayOfWeek;


            DateTime currentDate = DateTime.Now;
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DateTime currentUTC = localZone.ToUniversalTime(currentDate);
            TimeSpan currentOffset = localZone.GetUtcOffset(currentDate);

            DateTime now = DateTime.UtcNow.UTCToServerTime();


            return Json(new { DateTime.Now.AddDays(-3).DayOfWeek, DateTime.Now.Date },
                JsonRequestBehavior.AllowGet);
        }

        #endregion



        [Authorize]
        public JsonResult ADSCustomerFilesMap()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string FolderLocation = "/Files/ADS/DirectProtect/";
            string[] files = Directory.GetFiles(Server.MapPath(FolderLocation), "*_*.*", SearchOption.AllDirectories);
            int i = 0;
            foreach (var item in files)
            {
                //string FileLocation = FolderLocation + item;
                string CustomerId = item.Split('_')[0];
                CustomerId = CustomerId.Split("DirectProtect\\")[1];
                int ReferenceId = 0;
                if (int.TryParse(CustomerId, out ReferenceId) && ReferenceId > 0)
                {
                    CustomerMigration cm = _Util.Facade.CustomerFacade.GetCustomerMigrationByReferenceId(ReferenceId, "Point Tier32");
                    if (cm != null)
                    {
                        // Load file meta data with FileInfo
                        FileInfo fileInfo = new FileInfo(item);

                        #region
                        // The byte[] to save the data in
                        //byte[] data = new byte[fileInfo.Length];

                        // Load a filestream and put its content into the byte[]
                        //using (FileStream fs = fileInfo.OpenRead())
                        //{
                        //    fs.Read(data, 0, data.Length);
                        //}

                        // Delete the temporary file
                        //fileInfo.Delete();

                        // Post byte[] to database
                        #endregion

                        CustomerFile cf = new CustomerFile()
                        {
                            CustomerId = cm.CustomerId,
                            FileId = Guid.NewGuid(),
                            CompanyId = CurrentUser.CompanyId.Value,
                            FileDescription = fileInfo.Name,
                            IsActive = true,
                            Filename = FolderLocation + fileInfo.Name,
                            FileFullName = fileInfo.Name,
                            FileSize = fileInfo.Length,
                            Uploadeddate = fileInfo.LastWriteTimeUtc,
                            CreatedBy = CurrentUser.UserId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            UpdatedBy = CurrentUser.UserId,
                            UpdatedDate = DateTime.Now.UTCCurrentTime(),
                            WMStatus = LabelHelper.WatermarkStatus.Pending,
                            AWSProcessStatus = LabelHelper.AWSProcessStatus.Local

                        };
                        _Util.Facade.CustomerFileFacade.InsertCustomerFile(cf);
                    }
                    else
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\POINT Customer File Import Err.txt"), true))
                        {
                            file.WriteLine("Customer Not Found for:  " + CustomerId);
                            file.Close();
                        }
                    }

                }
                i++;
            }

            return Json(1);
        }

        private static IRestResponse GetCreditReportResponse(CustomerCreditScore CreditScore)
        {

            string url = "https://api.testdatasolutions.com/";
            string Name = CreditScore.FirstName + " " + CreditScore.LastName;
            CreditScore.ACCOUNT = "CENTRAL19";
            CreditScore.PASSWD = "Gn-!9uKQG*w5g96zKxOj";
            CreditScore.PASS = "2";
            CreditScore.PROCESS = "PCCREDIT";
            CreditScore.PRODUCT = "CREDIT";
            CreditScore.BUREAU = "EFX";
            string Content = string.Format("------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ACCOUNT\"\r\n\r\n{0}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PASSWD\"\r\n\r\n{1}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PASS\"\r\n\r\n{2}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PROCESS\"\r\n\r\n{3}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"BUREAU\"\r\n\r\n{4}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PRODUCT\"\r\n\r\n{5}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"NAME\"\r\n\r\n{6}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"SSN\"\r\n\r\n{7}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ADDRESS\"\r\n\r\n{8}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"CITY\"\r\n\r\n{9}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"STATE\"\r\n\r\n{10}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ZIP\"\r\n\r\n{11}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", CreditScore.ACCOUNT, CreditScore.PASSWD, CreditScore.PASS, CreditScore.PROCESS, CreditScore.BUREAU, CreditScore.PRODUCT, Name, CreditScore.SSN, CreditScore.ADDRESS, CreditScore.CITY, CreditScore.STATE, CreditScore.ZIP);
            var client = new RestClient("https://api.testdatasolutions.com/");
            var request = new RestRequest(Method.POST);
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            request.AddHeader("postman-token", "ee0be9f9-72be-ca18-ee60-78c989280310");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
            request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW," + Content, ParameterType.RequestBody);

            request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ACCOUNT\"\r\n\r\n" + CreditScore.ACCOUNT + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PASSWD\"\r\n\r\n" + CreditScore.PASSWD + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PASS\"\r\n\r\n" + CreditScore.PASS + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PROCESS\"\r\n\r\n" + CreditScore.PROCESS + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"BUREAU\"\r\n\r\n" + CreditScore.BUREAU + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PRODUCT\"\r\n\r\n" + CreditScore.PRODUCT + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"NAME\"\r\n\r\n" + Name + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"SSN\"\r\n\r\n" + CreditScore.SSN + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ADDRESS\"\r\n\r\n" + CreditScore.ADDRESS + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"CITY\"\r\n\r\n" + CreditScore.CITY + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"STATE\"\r\n\r\n" + CreditScore.STATE + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ZIP\"\r\n\r\n" + CreditScore.ZIP + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return response;
        }


        internal static string GetResponse(string URL, string RequestUrl)
        {
            HttpResponseMessage response;
            HttpResponseMessage responseCus;
            using (var client = new HttpClient())
            {
                response = client.GetAsync(URL).Result;
                responseCus = client.GetAsync(RequestUrl).Result;
            }
            string resultMessage = "";
            if (response != null && response.IsSuccessStatusCode)
            {
                resultMessage = responseCus.Content.ReadAsStringAsync().Result;
            }
            return resultMessage;
        }

        #region Data Migration from Agemni  

        public JsonReader PullAgemniCustomerMissingData()
        {

            int start = 1;
            int end = 999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";
            string RequestCountUrl = "https://www.sattracks.net/stsync/api/customer/totalcount";
            var authCountResponse = GetResponse(AuthUrl, RequestCountUrl);
            TotalDataCount TotalCount = Newtonsoft.Json.JsonConvert.DeserializeObject<TotalDataCount>(authCountResponse);
            var LoopSize = (TotalCount.Data.TotalCount / 999) + 5;
            for (int i = 1; i < LoopSize; i++)
            {
                string RequestUrl = "https://www.sattracks.net/stsync/api/customer?start=" + start + "&count=" + end;

                AgemniCustomerList agemniCustomer = new AgemniCustomerList();

                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                var authResponse = GetResponse(AuthUrl, RequestUrl);


                agemniCustomer = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemniCustomerList>(authResponse);
                Customer cus = new Customer();

                CustomerMigration cusMigrate = new CustomerMigration();

                foreach (var item in agemniCustomer.Data)
                {
                    #region Customer Data update
                    int CusId = new int();
                    Int32.TryParse(item.CustomerId, out CusId);

                    cus.Id = CusId;
                    cusMigrate = _Util.Facade.CustomerFacade.GetAgemniCustomerByReferenceId(CusId);
                    if (cusMigrate != null)
                    {
                        cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(cusMigrate.CustomerId);
                        try
                        {
                            #region Data

                            DateTime SalesDate = new DateTime();
                            DateTime.TryParse(item.SoldDate, out SalesDate);

                            int SellsPersonId = new int();
                            Int32.TryParse(item.SalesPersonId, out SellsPersonId);

                            cus.CSProvider = item.ReferralId;
                            cus.RenewalTerm = SellsPersonId;
                            cus.Website = item.CreatedById;
                            cus.SalesDate = SalesDate;

                            #endregion

                            #region Update Customer
                            try
                            {
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            catch (Exception ex)
                            {
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\AGEMNI-CustomerImportError.txt"), true))
                                {
                                    file.WriteLine(DateTime.Now.ToString("Update Eror For Customer: " + item.CustomerId));
                                    file.Close();
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\AGEMNI-CustomerImportError.txt"), true))
                            {
                                file.WriteLine(DateTime.Now.ToString("Parsing Eror For Customer: " + item.CustomerId));
                                file.Close();
                            }
                        }
                    }


                    #endregion
                }
                start = start + 999;
            }
            return null;
        }

        public JsonReader PullAgemniCustomer(int? PageNo)

        {

            int start = 1;
            int end = 999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";
            string RequestCountUrl = "https://www.sattracks.net/stsync/api/customer/totalcount";
            var authCountResponse = GetResponse(AuthUrl, RequestCountUrl);
            TotalDataCount TotalCount = Newtonsoft.Json.JsonConvert.DeserializeObject<TotalDataCount>(authCountResponse);
            var LoopSize = (TotalCount.Data.TotalCount / 999) + 5;
            for (int i = 1; i < LoopSize; i++)
            {
                if (PageNo.HasValue && PageNo > 0 && i < PageNo)
                {
                    start = start + 999;
                    continue;
                }

                string RequestUrl = "https://www.sattracks.net/stsync/api/customer?start=" + start + "&count=" + end;


                string RequestUrlLeadSorurce = "https://www.sattracks.net/stsync/api/customer/advertisement";
                AgemniLeadSourceList LeadSourceLookup = new AgemniLeadSourceList();




                AgemniCustomerList agemniCustomer = new AgemniCustomerList();

                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                var authResponse = GetResponse(AuthUrl, RequestUrl);
                var LeadSourceResponse = GetResponse(AuthUrl, RequestUrlLeadSorurce);
                LeadSourceLookup = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemniLeadSourceList>(LeadSourceResponse);

                agemniCustomer = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemniCustomerList>(authResponse);
                Customer cus = new Customer();
                CustomerCompany cuscom = new CustomerCompany();
                CustomerMigration cusMigrate = new CustomerMigration();
                CustomerNote cusNote = new CustomerNote();
                PaymentInfo payInfo = new PaymentInfo();
                string HeadNote = "<span style='font-weight:bold'>Customer Data:</span></br>";
                foreach (var item in agemniCustomer.Data)
                {
                    #region Customer Data Insert
                    int CusId = new int();
                    Int32.TryParse(item.CustomerId, out CusId);

                    cus.Id = CusId;
                    cusMigrate = _Util.Facade.CustomerFacade.GetAgemniCustomerByReferenceId(CusId);
                    if (cusMigrate != null)
                    {
                        continue;
                    }

                    try
                    {
                        #region Data
                        DateTime BirthDate = new DateTime();
                        DateTime.TryParse(item.BirthDate, out BirthDate);
                        DateTime CreatedDate = new DateTime();
                        DateTime.TryParse(item.CreateDate, out CreatedDate);
                        DateTime LastUpdatedDate = new DateTime();
                        DateTime.TryParse(item.LastOpenedDate, out LastUpdatedDate);
                        DateTime CancellationDate = new DateTime();
                        DateTime.TryParse(item.CancelDate, out CancellationDate);
                        DateTime CloseDate = new DateTime();
                        DateTime.TryParse(item.CloseDate, out CloseDate);
                        DateTime DisconnectServiceDate = new DateTime();
                        DateTime.TryParse(item.DisconnectServiceDate, out DisconnectServiceDate);
                        DateTime DoNotCallDate = new DateTime();
                        DateTime.TryParse(item.DoNotCallDate, out DoNotCallDate);
                        DateTime ChangedDate = new DateTime();
                        DateTime.TryParse(item.ChangedDate, out ChangedDate);
                        DateTime SalesDate = new DateTime();
                        DateTime.TryParse(item.SoldDate, out SalesDate);

                        int VendorId = new int();
                        Int32.TryParse(item.VendorId, out VendorId);

                        int LeadPersonID = new int();
                        Int32.TryParse(item.LeadPersonID, out LeadPersonID);

                        int PromotionId = new int();
                        Int32.TryParse(item.PromotionId, out PromotionId);

                        int JobTypeId = new int();
                        Int32.TryParse(item.JobTypeId, out JobTypeId);

                        int SalesLocationId = new int();
                        Int32.TryParse(item.SalesLocationId, out SalesLocationId);

                        int Phone1TypeId = new int();
                        Int32.TryParse(item.Phone1TypeId, out Phone1TypeId);

                        int InstallAddressTypeId = new int();
                        Int32.TryParse(item.InstallAddressTypeId, out InstallAddressTypeId);

                        int MiscPhone1TypeId = new int();
                        Int32.TryParse(item.MiscPhone1TypeId, out MiscPhone1TypeId);

                        int MiscPhone2TypeId = new int();
                        Int32.TryParse(item.MiscPhone2TypeId, out MiscPhone2TypeId);

                        int SellsPersonId = new int();
                        Int32.TryParse(item.SalesPersonId, out SellsPersonId);

                        cus.CustomerId = Guid.NewGuid();

                        cus.FirstName = item.FirstName;
                        cus.LastName = item.LastName;
                        cus.EmailAddress = item.Email;

                        cus.BusinessName = item.BusinessName;

                        cus.City = item.InstallCity;
                        cus.State = item.InstallState;
                        cus.ZipCode = item.InstallZipCode;
                        cus.Street = item.InstallAddress;
                        cus.Country = item.InstallCounty;
                        if (item.IsHomeOwner == "true")
                        {
                            cus.HomeVerified = true;
                        }
                        else
                        {
                            cus.HomeVerified = false;

                        }
                        cus.DateofBirth = BirthDate;
                        if (!string.IsNullOrEmpty(item.Phone1))
                        {

                            cus.PrimaryPhone = item.Phone1;
                        }
                        else
                        {
                            cus.PrimaryPhone = item.MiscPhone1;
                        }

                        cus.CellNo = item.MiscPhone2;
                        cus.CreatedDate = CreatedDate;
                        cus.LastUpdatedDate = LastUpdatedDate;
                        cus.CancellationDate = CancellationDate;

                        cus.LastOpenedDate = LastUpdatedDate;
                        cus.BillingPhone = item.BillingPhone;
                        cus.BillingEmail = item.BillingEmail;
                        cus.BillingContact = item.BillingContact;
                        if (SalesLocationId > 0)
                        {
                            cus.SalesLocation = SalesLocationId.ToString();
                        }

                        if (PromotionId > 0)
                        {
                            AgemniLeadSource leadsource = LeadSourceLookup.Data.Where(x => x.AdvertisementID == PromotionId.ToString()).FirstOrDefault();
                            if (leadsource != null)
                            {
                                cus.LeadSource = leadsource.Code;
                            }

                        }
                        else
                        {
                            cus.LeadSource = "-1";
                        }
                        if (item.BestTimeToCallId != null)
                        {
                            if (item.BestTimeToCallId == "1")
                            {
                                cus.BestTimeToCall = "Morning";
                            }
                            else if (item.BestTimeToCallId == "2")
                            {
                                cus.BestTimeToCall = "Evening";
                            }
                            else if (item.BestTimeToCallId == "3")
                            {
                                cus.BestTimeToCall = "AllDay";
                            }
                            else if (item.BestTimeToCallId == "4")
                            {
                                cus.BestTimeToCall = "Night";
                            }

                        }
                        cus.CSProvider = item.ReferralId;
                        cus.RenewalTerm = SellsPersonId;
                        cus.Website = item.CreatedById;
                        cus.SalesDate = SalesDate;

                        cus.SalesDate = SalesDate;
                        cus.CreatedByUid = new Guid();

                        cus.ChildOf = new Guid();
                        cus.AccessGivenTo = new Guid();

                        cus.DuplicateCustomer = new Guid();
                        cus.SoldBy2 = new Guid();
                        cus.SoldBy3 = new Guid();
                        cus.LastUpdatedBy = "00000000-0000-0000-0000-000000000000";
                        cus.ReferringCustomer = new Guid();
                        cus.LastUpdatedByUid = new Guid();
                        #endregion

                        #region Insert Customer
                        try
                        {
                            _Util.Facade.CustomerFacade.InsertAgemniCustomer(cus);

                            #region Insert Customer Company
                            cuscom = new CustomerCompany
                            {
                                CustomerId = cus.CustomerId,
                                CompanyId = CurrentUser.CompanyId.Value,
                                IsLead = false,
                                IsActive = true
                            };
                            _Util.Facade.CustomerFacade.InsertCustomerCompany(cuscom);

                            #endregion

                            #region InsertCustomerMigrate
                            cusMigrate = new CustomerMigration
                            {
                                CustomerId = cus.CustomerId,
                                CompanyId = CurrentUser.CompanyId.Value,
                                RefenrenceId = CusId,
                                Platform = LabelHelper.CustomerMigrationPlatforms.Agemni,
                                CreatedBy = CurrentUser.UserId,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                Note = HeadNote + "VendorId: " + VendorId + "</br>LeadPersonID: " + LeadPersonID + "</br>PromotionId: " + PromotionId + "</br>SalesLocationId: " + SalesLocationId + "</br>JobTypeId: " + JobTypeId +
                                "</br>Phone1TypeId: " + Phone1TypeId + "</br>InstallAddressTypeId: " + InstallAddressTypeId + "</br>IsHomeOwner: " + item.IsHomeOwner + "</br>MiscPhone1: " + item.MiscPhone1 +
                                "</br>MiscPhone1TypeId: " + MiscPhone1TypeId + "</br>MiscPhone2: " + item.MiscPhone2 + "</br>MiscPhone2TypeId: " + MiscPhone2TypeId + "</br>BillingContact: " + item.BillingContact + "</br>BillingPhone: " + item.BillingPhone +
                                "</br>BillingEmail: " + item.BillingEmail + "</br>BillingCounty: " + item.BillingEmail + "</br>ClosedDate: " + CloseDate + "</br>ChangedDate: " + ChangedDate + "</br>DisconnectServiceDate: " + DisconnectServiceDate +
                                "</br>DoNotCallDate: " + DoNotCallDate + "</br>"

                            };
                            _Util.Facade.CustomerFacade.InsertCustomerMigration(cusMigrate);
                            #endregion

                            #region Insert PaymentInfo
                            payInfo = new PaymentInfo
                            {
                                AccountName = item.BillingFirstName + " " + item.BillingLastName,
                                CustomerId = cus.CustomerId
                            };
                            _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(payInfo);
                            #endregion

                        }
                        catch (Exception ex)
                        {
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\AGEMNI-CustomerImportError.txt"), true))
                            {
                                file.WriteLine(DateTime.Now.ToString("Insert Eror For Customer: " + item.CustomerId));
                                file.Close();
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\AGEMNI-CustomerImportError.txt"), true))
                        {
                            file.WriteLine(DateTime.Now.ToString("Parsing Eror For Customer: " + item.CustomerId));
                            file.Close();
                        }
                    }
                    #endregion
                }
                start = start + 999;
            }
            return null;
        }

        public JsonReader DevideAgemniLeadsOrCustomer(int? PageNo)
        {

            int start = 1;
            int end = 999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";
            string RequestCountUrl = "https://www.sattracks.net/stsync/api/tcustomer/totalcount";
            var authCountResponse = GetResponse(AuthUrl, RequestCountUrl);
            TotalDataCount TotalCount = Newtonsoft.Json.JsonConvert.DeserializeObject<TotalDataCount>(authCountResponse);
            var LoopSize = (TotalCount.Data.TotalCount / 999) + 5;
            List<CustomerMigration> customerMigrations = _Util.Facade.CustomerFacade.GetAllCustomerMigration();
            for (int i = 1; i < LoopSize; i++)
            {
                if (PageNo.HasValue && PageNo > 0 && i < PageNo)
                {
                    start = start + 999;
                    continue;
                }

                string RequestUrl = "https://www.sattracks.net/stsync/api/tcustomer?start=" + start + "&count=" + end;

                AgemnitCustomerList agemniCustomer = new AgemnitCustomerList();

                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                var authResponse = GetResponse(AuthUrl, RequestUrl);

                agemniCustomer = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemnitCustomerList>(authResponse);
                Customer cus = new Customer();
                CustomerCompany cuscom = new CustomerCompany();
                CustomerMigration cusMigrate = new CustomerMigration();

                foreach (var item in agemniCustomer.Data)
                {

                    #region Update Customer
                    int CusId = new int();
                    Int32.TryParse(item.CustomerId, out CusId);

                    int ContactStatusId = new int();
                    Int32.TryParse(item.ContactStatusId, out ContactStatusId);
                    if (CusId > 0)
                    {
                        CustomerMigration cusAgemni = customerMigrations.Where(x => x.RefenrenceId == CusId).FirstOrDefault(); //_Util.Facade.CustomerFacade.GetAgemniCustomerByReferenceId(CusId);
                        cus = cusAgemni != null ? _Util.Facade.CustomerFacade.GetCustomerByCustomerId(cusAgemni.CustomerId) : null;
                        if (cus != null)
                        {
                            cus.CityPrevious = item.BillingCity;
                            cus.StatePrevious = item.BillingState;
                            cus.ZipCodePrevious = item.BillingZipCode;
                            cus.StatePrevious = item.BillingState;

                            if (string.IsNullOrEmpty(cus.EmailAddress))
                            {
                                cus.EmailAddress = item.Email1;
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            #region Conditions
                            if (ContactStatusId == 1)
                            {
                                cus.Status = "Lead";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 2)
                            {
                                cus.Status = "Lead-Unknown";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 3)
                            {
                                cus.Status = "Lead-Dead";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 4)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);

                            }
                            else if (ContactStatusId == 5)
                            {
                                cus.Status = "DONOTCALL";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 6)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 7)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 8)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 9)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                cus.IsActive = false;
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 10)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 11)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 12)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 13)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 14)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 15)
                            {
                                cus.Status = "Lead-Referral";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 16)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 17)
                            {
                                cus.Status = "Lead-CallBack";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 18)
                            {
                                cus.Status = "Lead-LVM/Email";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 19)
                            {
                                cus.Status = "Lead-Shopping/Price";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 20)
                            {
                                cus.Status = "Lead-Spouse";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 21)
                            {
                                cus.Status = "Lead-Undecided";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 22)
                            {
                                cus.Status = "Lead-ClosingDate";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 23)
                            {
                                cus.Status = "Lead-VerifyContract";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 24)
                            {
                                cus.Status = "Lead-SystemCheck";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 25)
                            {
                                cus.Status = "Lead-WalkThrough";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 26)
                            {
                                cus.Status = "Lead-FailedCredit";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 27)
                            {
                                cus.Status = "Lead-FollowUp";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 28)
                            {
                                cus.Status = "Lead-Retread";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 29)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                cus.BranchId = 1002;
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 30)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                cus.BranchId = 1002;
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }

                            else if (ContactStatusId == 31)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                cus.BranchId = 1003;
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 32)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                cus.BranchId = 1003;
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 33)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 34)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 35)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                cus.IsActive = false;
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 36)
                            {

                                cus.Status = "Lead-Out of Service";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 37)
                            {
                                cus.Status = "Lead-Over_10,000_sq_ft";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 38)
                            {
                                cus.Status = "Lead-Renter";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 39)
                            {
                                cus.Status = "Lead-Under Contract";
                                cuscom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cus.Id);
                                if (cuscom != null)
                                {
                                    cuscom.IsLead = true;
                                }
                                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cuscom);
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 40)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.BranchId = 1004;
                                cus.Status = "Imported_Lead_Status";

                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            else if (ContactStatusId == 41)
                            {
                                cus.CustomerStatus = ContactStatusId.ToString();
                                cus.Status = "Imported_Lead_Status";
                                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                            }
                            #endregion
                        }
                    }

                    #endregion

                }
                start = start + 999;

            }

            return null;
        }
        public JsonReader PullAgemniAditionalContact(int? PageNo)
        {
            int start = 1;
            int end = 999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";
            string RequestCountUrl = "https://www.sattracks.net/stsync/api/tcustomer/totalcount";
            var authCountResponse = GetResponse(AuthUrl, RequestCountUrl);
            TotalDataCount TotalCount = Newtonsoft.Json.JsonConvert.DeserializeObject<TotalDataCount>(authCountResponse);
            var LoopSize = (TotalCount.Data.TotalCount / 999) + 5;
            List<CustomerMigration> customerMigrations = _Util.Facade.CustomerFacade.GetAllCustomerMigration();
            for (int i = 1; i < LoopSize; i++)
            {


                if (PageNo.HasValue && PageNo > 0 && i < PageNo)
                {
                    start = start + 999;
                    continue;
                }
                string RequestUrl = "https://www.sattracks.net/stsync/api/tcustomer?start=" + start + "&count=" + end;

                AgemnitCustomerList agemniCustomer = new AgemnitCustomerList();

                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                var authResponse = GetResponse(AuthUrl, RequestUrl);

                agemniCustomer = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemnitCustomerList>(authResponse);

                CustomerCompany cuscom = new CustomerCompany();
                CustomerMigration cusMigrate = new CustomerMigration();
                EmergencyContact emergencyContact = new EmergencyContact();

                foreach (var item in agemniCustomer.Data)
                {
                    #region Insert Customer Additional Contact
                    int CusId = new int();
                    Int32.TryParse(item.CustomerId, out CusId);

                    int ContactStatusId = new int();
                    Int32.TryParse(item.ContactStatusId, out ContactStatusId);
                    if (CusId > 0)
                    {
                        CustomerMigration cusAgemni = customerMigrations.Where(x => x.RefenrenceId == CusId).FirstOrDefault(); //_Util.Facade.CustomerFacade.GetAgemniCustomerByReferenceId(CusId);

                        if (item.AlternateFirstName != null || item.AlternateLastName != null)
                        {
                            string phoneType = "";
                            if (item.AlternatePhone1TypeId == "3")
                            {
                                phoneType = "Home";
                            }
                            else if (item.AlternatePhone1TypeId == "1")
                            {
                                phoneType = "Work";
                            }
                            else if (item.AlternatePhone1TypeId == "5")
                            {
                                phoneType = "Toll-Free";
                            }
                            else if (item.AlternatePhone1TypeId == "2")
                            {
                                phoneType = "Fax";
                            }
                            else if (item.AlternatePhone1TypeId == "4")
                            {
                                phoneType = "Cell";
                            }
                            else if (item.AlternatePhone1TypeId == "6")
                            {
                                phoneType = "Other";
                            }
                            CustomerAdditionalContact additionalContact = new CustomerAdditionalContact()
                            {

                                CustomerId = cusAgemni.CustomerId,
                                FirstName = item.AlternateFirstName,
                                LastName = item.AlternateLastName,
                                Email = item.AlternateEmail,
                                Phone = item.AlternatePhone1,
                                PhoneType = phoneType
                            };
                            _Util.Facade.AdditionalContactFacade.InsertAdditionalContact(additionalContact);
                        }
                    }
                    #endregion
                }
                start = start + 999;

            }

            return null;
        }


        public JsonReader PullAgemniJobType()
        {
            string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";

            string RequestUrl = "https://www.sattracks.net/stsync/api/customer/jobtypes";
            AgemniJobTypeList agemniCustomer = new AgemniJobTypeList();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var authResponse = GetResponse(AuthUrl, RequestUrl);

            var agemniCustomer2 = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemniJobTypeList>(authResponse);
            return null;
        }
        public JsonReader UpdateSeller()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<Customer> cusList = _Util.Facade.CustomerFacade.GetAllCustomers();
            List<AgemniEmployeeMapper> empList = _Util.Facade.EmployeeFacade.GetAllAgemniEmpMapper();

            foreach (var item in cusList)
            {
                int RefId = new int();
                Int32.TryParse(item.CSProvider, out RefId);

                int CreatedById = new int();
                Int32.TryParse(item.Website, out CreatedById);

                if (empList.Find(x => x.AgID == item.RenewalTerm) != null)
                {
                    item.Soldby = empList.Find(x => x.AgID == item.RenewalTerm).RMRID.ToString();
                    _Util.Facade.CustomerFacade.UpdateCustomer(item);
                }
                if (empList.Find(x => x.AgID == CreatedById) != null)
                {
                    item.CreatedByUid = empList.Find(x => x.AgID == CreatedById).RMRID;
                    _Util.Facade.CustomerFacade.UpdateCustomer(item);
                }
                if (cusList.Find(x => x.Id == RefId) != null)
                {
                    item.ReferringCustomer = cusList.Find(x => x.Id == RefId).CustomerId;
                    _Util.Facade.CustomerFacade.UpdateCustomer(item);
                }
            }
            return null;
        }
        public JsonReader PullAgemniPhoneType()
        {
            string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";

            string RequestUrl = "https://www.sattracks.net/stsync/api/customer/phonetypes";
            AgemniPhoneTypeList agemniCustomer = new AgemniPhoneTypeList();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var authResponse = GetResponse(AuthUrl, RequestUrl);

            var agemniCustomer2 = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemniPhoneTypeList>(authResponse);
            return null;
        }
        public JsonReader PullBestTimeToCall()
        {
            string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";

            string RequestUrl = "https://www.sattracks.net/stsync/api/customer/besttimetocall";
            AgemniBestTimeToCallList agemniCustomer = new AgemniBestTimeToCallList();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var authResponse = GetResponse(AuthUrl, RequestUrl);

            var agemniCustomer2 = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemniBestTimeToCallList>(authResponse);
            return null;
        }
        public JsonReader PullAgemniLeadSource()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";

            string RequestUrl = "https://www.sattracks.net/stsync/api/customer/advertisement";
            AgemniLeadSourceList agemniCustomer = new AgemniLeadSourceList();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var authResponse = GetResponse(AuthUrl, RequestUrl);

            agemniCustomer = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemniLeadSourceList>(authResponse);
            //System.Web.HttpRuntime.Cache["GetAgemniLeadSourceList"] = agemniCustomer.Data;
            Lookup leadsourceLookup = new Lookup();
            List<Lookup> LeadSourceList = _Util.Facade.LookupFacade.GetLookUpByKey("LeadSource");

            int order = 1;
            foreach (var item in agemniCustomer.Data)
            {
                if (LeadSourceList.Where(x => x.DataValue.ToLower() == item.Code.ToLower()).FirstOrDefault() == null)
                {
                    leadsourceLookup.DataKey = "LeadSource";
                    leadsourceLookup.ParentDataKey = "AgemniLeadSource";
                    leadsourceLookup.DataValue = item.Code;
                    leadsourceLookup.DisplayText = item.Name;

                    leadsourceLookup.CompanyId = CurrentUser.CompanyId.Value;

                    _Util.Facade.LookupFacade.InsertLookup(leadsourceLookup);
                }

            }
            return null;
        }

        public JsonReader PullAgemniSalesLocation()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";

            string RequestUrl = "https://www.sattracks.net/stsync/api/location";
            AgemniSalesLocationList agemniCustomer = new AgemniSalesLocationList();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var authResponse = GetResponse(AuthUrl, RequestUrl);

            agemniCustomer = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemniSalesLocationList>(authResponse);
            //System.Web.HttpRuntime.Cache["GetAgemniLeadSourceList"] = agemniCustomer.Data;

            Lookup leadsourceLookup = new Lookup();
            int order = 0;
            foreach (var item in agemniCustomer.Data)
            {

                leadsourceLookup.DataKey = "SalesLocation";
                //leadsourceLookup.ParentDataKey = "AgemniLeadSource";
                leadsourceLookup.DataValue = item.LocationID;
                leadsourceLookup.DisplayText = item.Name;

                leadsourceLookup.CompanyId = CurrentUser.CompanyId.Value;
                leadsourceLookup.DataOrder = order;
                order++;

                _Util.Facade.LookupFacade.InsertLookup(leadsourceLookup);


            }
            return null;
        }
        public JsonReader PullAgemniEmployee()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";

            string RequestUrl = "https://www.sattracks.net/stsync/api/user?count=500";
            AgemniEmployeeList agemniCustomer = new AgemniEmployeeList();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var authResponse = GetResponse(AuthUrl, RequestUrl);
            int existCount = 0;
            AgemniEmployeeList agemniCustomer2 = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemniEmployeeList>(authResponse);
            List<Employee> empList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);
            foreach (var item in agemniCustomer2.Data)
            {
                int AgId = new int();
                Int32.TryParse(item.UserID, out AgId);

                if (empList.Find(x => x.Email == item.Email) != null)
                {
                    existCount++;
                    AgemniEmployeeMapper agemniMapper = new AgemniEmployeeMapper();
                    agemniMapper.AgID = AgId;
                    agemniMapper.RMRID = empList.Find(x => x.Email == item.Email).UserId;
                    _Util.Facade.EmployeeFacade.InsertAgemniEmployeeMapper(agemniMapper);
                }
                else
                {
                    DateTime BirthDate = new DateTime();
                    DateTime.TryParse(item.BirthDate, out BirthDate);
                    DateTime HireDate = new DateTime();
                    DateTime.TryParse(item.hireDate, out HireDate);
                    DateTime UpdatedDate = new DateTime();
                    DateTime.TryParse(item.UpdatedDate, out UpdatedDate);
                    DateTime CreatedDate = new DateTime();
                    DateTime.TryParse(item.OriginDate, out CreatedDate);
                    DateTime TerminationDate = new DateTime();
                    DateTime.TryParse(item.TerminationDate, out TerminationDate);
                    Double HourlyRate = 0.0;
                    Double.TryParse(item.HourlyRate, out HourlyRate);

                    Employee emp = new Employee();
                    emp.UserId = Guid.NewGuid();
                    emp.FirstName = item.FirstName;
                    emp.LastName = item.LastName;
                    emp.DOB = BirthDate;
                    emp.Email = item.Email;
                    emp.UserName = item.login;
                    emp.PtoRate = HourlyRate;
                    emp.City = item.City;
                    emp.State = item.State;
                    emp.ZipCode = item.Zip;
                    emp.Street = item.Address1;
                    emp.SSN = item.ssnumber;
                    emp.IsActive = false;
                    emp.HireDate = HireDate;
                    emp.LastUpdatedDate = UpdatedDate;
                    emp.CreatedDate = UpdatedDate;
                    emp.Recruited = true;
                    emp.IsCurrentEmployee = true;
                    emp.TerminationDate = TerminationDate;
                    emp.CompanyId = CurrentUser.CompanyId.Value;
                    if (item.Active == "true")
                    {
                        emp.IsActive = true;
                    }
                    else
                    {
                        emp.IsActive = false;

                    }

                    _Util.Facade.EmployeeFacade.InsertEmployee(emp);

                    UserLogin userlogin = new UserLogin();
                    userlogin.UserId = emp.UserId;
                    userlogin.UserName = item.login;
                    userlogin.LastUpdatedDate = UpdatedDate;
                    userlogin.PhoneNumber = item.Phone1;
                    userlogin.EmailAddress = item.Email;
                    userlogin.FirstName = item.FirstName;
                    userlogin.LastName = item.LastName;
                    userlogin.CompanyId = CurrentUser.CompanyId.Value;
                    if (item.password != null)
                    {
                        userlogin.Password = MD5Encryption.GetMD5HashData(item.password);
                        userlogin.IsActive = false;
                    }
                    else
                    {
                        userlogin.IsActive = emp.IsActive;
                    }
                    _Util.Facade.UserLoginFacade.InsertUserLogin(userlogin);

                    UserPermission up = new UserPermission();
                    up.UserId = userlogin.UserId;
                    up.CompanyId = CurrentUser.CompanyId.Value;
                    up.PermissionGroupId = 16;
                    _Util.Facade.PermissionFacade.InsertUserPermission(up);

                    AgemniEmployeeMapper agemniMapper = new AgemniEmployeeMapper();
                    agemniMapper.AgID = AgId;
                    agemniMapper.RMRID = emp.UserId;
                    _Util.Facade.EmployeeFacade.InsertAgemniEmployeeMapper(agemniMapper);

                    UserCompany uc = new UserCompany();
                    uc.CompanyId = CurrentUser.CompanyId.Value;
                    uc.UserId = emp.UserId;
                    uc.IsDefault = true;
                    _Util.Facade.UserCompanyFacade.InsertUserCompany(uc);

                    UserOrganization ug = new UserOrganization();
                    ug.CompanyId = CurrentUser.CompanyId.Value;
                    ug.UserName = userlogin.UserName;
                    ug.IsActive = emp.IsActive;
                    _Util.Facade.UserOrganizationFacade.InsertUserOrganization(ug);

                }
            }
            return null;
        }

        public JsonReader PullAgemniWorkOrder(int? PageNo)
        {
            int start = 1;
            int end = 999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";
            string RequestCountUrl = "https://www.sattracks.net/stsync/api/tworkOrder/totalcount";
            var authCountResponse = GetResponse(AuthUrl, RequestCountUrl);
            TotalDataCount TotalCount = Newtonsoft.Json.JsonConvert.DeserializeObject<TotalDataCount>(authCountResponse);
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var LoopSize = (TotalCount.Data.TotalCount / 999) + 5;
            for (int i = 1; i < LoopSize; i++)
            {

                if (PageNo.HasValue && PageNo > 0 && i < PageNo)
                {
                    start = start + 999;
                    continue;
                }

                string RequestUrl = "https://www.sattracks.net/stsync/api/tworkOrder?start=" + start + "&count=" + end;
                //AgemniWorkOrderList agemniCustomer = new AgemniWorkOrderList();


                var authResponse = GetResponse(AuthUrl, RequestUrl);
                int existCount = 0;
                AgemniWorkOrderList agemniWorkOrder = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemniWorkOrderList>(authResponse);

                Ticket agemniTicket = new Ticket();
                CustomerAppointment cusAppointment = new CustomerAppointment();
                Customer cus = new Customer();
                CustomerMigration cusMigrate = new CustomerMigration();

                TicketUser ticketUser = new TicketUser();
                TicketReply ticreply = new TicketReply();
                CustomerNote cusNote = new CustomerNote();

                string HeadNote = "<span style='font-weight:bold'>WorkOrder Data:</span></br>";
                string Notes = "";
                bool IsSameWorkOrder = false;
                //List<AgemniWorkOrder> empList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);
                foreach (var item in agemniWorkOrder.Data)
                {

                    int Id = new int();
                    Int32.TryParse(item.Id, out Id);

                    int CustomerId = new int();
                    Int32.TryParse(item.CustomerId, out CustomerId);

                    try
                    {
                        if (CustomerId > 0)
                        {

                            Notes = "<p>Id: " + item.Id + "</br>InvoiceId: " + item.InvoiceId + "</br> WorkOrderType: " + item.WorkOrderType + "</br> TypeId: " + item.TypeId + "</br> StatusId: " + item.StatusId + "</br> LocationId: " + item.LocationId + "</br> InstallerId: " + item.InstallerId + "</br> InstallDate: " + item.InstallDate + " </br>PanelType: " + item.PanelType + "</br> TakeOverType: " + item.TakeOverType + "</br> CommType: " + item.CommType + "</br> ReceiverPhone: " + item.ReceiverPhone + "</br> MonitoringAccount: " + item.MonitoringAccount + "</br> AlarmAccount: " + item.AlarmAccount + "</br> CellModemSerial: " + item.CellModemSerial + "</br> AbortCode: " + item.AbortCode + "</br> MasterPin: " + item.MasterPin + "</br> FirstName1: " + item.FirstName1 + "</br> LastName1: " + item.LastName1 + "</br> Phone1: " + item.Phone1 + "</br> Phone1Type: " + item.Phone1Type + "</br> ECV1: " + item.ECV1 + "</br> FirstName2: " + item.FirstName2 + "</br> LastName1: " + item.LastName2 + "</br> Phone2: " + item.Phone2 + "</br> Phone1Type: " + item.Phone2Type + "<br> ECV2: " + item.ECV2 + "</br> FirstName3: " + item.FirstName3 + "</br> LastName3: " + item.LastName3 + "</br> Phone3: " + item.Phone3 + "</br> Phone1Type: " + item.Phone3Type + "<br> ECV3: " + item.ECV3 + "</br> TestDateTime: " + item.TestDateTime + "</br> TestDuration: " + item.TestDuration + "</br> CustomFields: " + item.CustomFields + "</br> AlarmDesiredLogin: " + item.AlarmDesiredLogin + "</br> AlarmDesiredPassword: " + item.AlarmDesiredPassword + "</br> CellPlanId: " + item.CellPlanId + "</br> PackageId: " + item.PackageId + "<br> SummaryOfService: " + item.SummaryOfService + "</br> ProvisioningAccount: " + item.ProvisioningAccount + "</br> WarrantyEffectiveDate: " + item.WarrantyEffectiveDate + "</br> WarrantyField: " + item.WarrantyField + "</br> WorkOrderPriorityId: " + item.WorkOrderPriorityId + "<br> WorkOrderStateId: " + item.WorkOrderStateId + "</br> WOName: " + item.WOName + "</br> TotalBillableUnits: " + item.TotalBillableUnits + "</br> AgemniId: " + item.AgemniId + "</br> WarrantyEffectiveDateTo: " + item.WarrantyEffectiveDateTo + "</br> PaymentTerm: " + item.PaymentTerm + "</br> WODueDate: " + item.WODueDate + "</p>";
                            IsSameWorkOrder = _Util.Facade.CustomerFacade.IsSameWorkOrder(item.Id);
                            if (IsSameWorkOrder == false)
                            {
                                cusMigrate = _Util.Facade.CustomerFacade.GetAgemniCustomerByReferenceId(CustomerId);
                                if (cusMigrate != null)
                                {
                                    //cusMigrate.Note = cusMigrate.Note + HeadNote + Notes;
                                    //_Util.Facade.CustomerFacade.UpdateCustomerMigration(cusMigrate);
                                    cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(cusMigrate.CustomerId);

                                    #region If customer exists in DB
                                    if (cus.CustomerId != new Guid())
                                    {
                                        agemniTicket = new Ticket()
                                        {
                                            TicketId = Guid.NewGuid(),
                                            CompanyId = CurrentUser.CompanyId.Value,
                                            CustomerId = cus.CustomerId,
                                            TicketType = LabelHelper.TicketType.Service,
                                            Status = LabelHelper.TicketStatus.Completed,
                                            CreatedBy = CurrentUser.UserId,
                                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                                            CompletionDate = DateTime.Now.UTCCurrentTime(),
                                            LastUpdatedBy = CurrentUser.UserId,
                                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                            CompletedDate = DateTime.UtcNow.UTCToClientTime().SetZeroHour(),
                                            Message = Notes,
                                        };
                                        _Util.Facade.TicketFacade.InsertTicket(agemniTicket);
                                        cusAppointment = new CustomerAppointment()
                                        {
                                            AppointmentId = agemniTicket.TicketId,
                                            CompanyId = CurrentUser.CompanyId.Value,
                                            CustomerId = agemniTicket.CustomerId,
                                            EmployeeId = new Guid(),
                                            AppointmentType = LabelHelper.TicketType.Service,
                                            AppointmentStartTime = "-1",
                                            AppointmentEndTime = "-1",
                                            LastUpdatedBy = "Administrator",
                                            CreatedBy = "Administrator",
                                            LastUpdatedDate = DateTime.Now,
                                            AppointmentDate = DateTime.Now,
                                            IsAllDay = false


                                        };
                                        _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(cusAppointment);

                                        ticketUser = new TicketUser()
                                        {
                                            TiketId = agemniTicket.TicketId,
                                            UserId = CurrentUser.UserId,
                                            IsPrimary = true,
                                            AddedBy = CurrentUser.UserId,
                                            AddedDate = DateTime.Now,
                                            NotificationOnly = false
                                        };
                                        _Util.Facade.TicketFacade.InsertTicketUser(ticketUser);

                                        //ticreply = new TicketReply()
                                        //{
                                        //    TicketId = agemniTicket.TicketId,
                                        //    UserId = CurrentUser.UserId,
                                        //    IsPrivate = false,
                                        //    IsOverview = false,
                                        //    Message = Notes,
                                        //    RepliedDate = DateTime.Now.UTCCurrentTime()
                                        //};
                                        //_Util.Facade.TicketFacade.InsertTicketReply(ticreply);

                                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\FinalInsertAgemniWorkOrder_" + i + ".txt"), true))
                                        {
                                            file.WriteLine("Inserted at: " + start);
                                            file.Close();
                                        }

                                    }
                                    #endregion
                                }

                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\FinalNotInsertAgemniWorkOrder_" + i + ".txt"), true))
                        {
                            file.WriteLine("Inserted at: " + start);
                            file.Close();
                        }
                    }


                }
                start = start + 999;
            }
            return null;
        }

        public JsonReader PullAgemniEmergencyContact(int? PageNo)
        {
            int start = 1;
            int end = 999;
            List<CustomerMigration> customerMigrations = _Util.Facade.CustomerFacade.GetAllCustomerMigration();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";
            string RequestCountUrl = "https://www.sattracks.net/stsync/api/tworkOrder/totalcount";
            var authCountResponse = GetResponse(AuthUrl, RequestCountUrl);
            TotalDataCount TotalCount = Newtonsoft.Json.JsonConvert.DeserializeObject<TotalDataCount>(authCountResponse);
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var LoopSize = (TotalCount.Data.TotalCount / 999) + 5;
            for (int i = 1; i < LoopSize; i++)
            {

                if (PageNo.HasValue && PageNo > 0 && i < PageNo)
                {
                    start = start + 999;
                    continue;
                }

                string RequestUrl = "https://www.sattracks.net/stsync/api/tworkOrder?start=" + start + "&count=" + end;
                //AgemniWorkOrderList agemniCustomer = new AgemniWorkOrderList();

                var authResponse = GetResponse(AuthUrl, RequestUrl);

                AgemniWorkOrderList agemniWorkOrder = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemniWorkOrderList>(authResponse);

                CustomerMigration cusMigrate = new CustomerMigration();
                EmergencyContact emergencyContact = new EmergencyContact();


                foreach (var item in agemniWorkOrder.Data)
                {

                    int Id = new int();
                    Int32.TryParse(item.Id, out Id);

                    int CustomerId = new int();
                    Int32.TryParse(item.CustomerId, out CustomerId);
                    try
                    {
                        if (CustomerId > 0)
                        {
                            cusMigrate = customerMigrations.Where(x => x.RefenrenceId == CustomerId).FirstOrDefault();
                            //_Util.Facade.CustomerFacade.GetAgemniCustomerByReferenceId(CustomerId);
                            if (cusMigrate != null)
                            {
                                if (item.FirstName1 != null || item.LastName1 != null)
                                {
                                    string phoneType = "";
                                    emergencyContact = new EmergencyContact()
                                    {
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        CustomerId = cusMigrate.CustomerId,
                                        FirstName = item.FirstName1,
                                        LastName = item.LastName1,
                                        Phone = item.Phone1,
                                    };
                                    _Util.Facade.EmergencyContactFacade.InsertEmergencyContact(emergencyContact);
                                }

                                if (item.FirstName2 != null || item.LastName2 != null)
                                {

                                    emergencyContact = new EmergencyContact()
                                    {
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        CustomerId = cusMigrate.CustomerId,
                                        FirstName = item.FirstName2,
                                        LastName = item.LastName2,
                                        Phone = item.Phone2,
                                    };
                                    _Util.Facade.EmergencyContactFacade.InsertEmergencyContact(emergencyContact);
                                }

                                if (item.FirstName3 != null || item.LastName3 != null)
                                {
                                    emergencyContact = new EmergencyContact()
                                    {
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        CustomerId = cusMigrate.CustomerId,
                                        FirstName = item.FirstName3,
                                        LastName = item.LastName3,
                                        Phone = item.Phone3,
                                    };
                                    _Util.Facade.EmergencyContactFacade.InsertEmergencyContact(emergencyContact);
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
                start = start + 999;
            }
            return null;
        }

        public JsonReader PullAgemniNote(int? PageNo)
        {
            int start = 1;
            int end = 999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";
            string RequestCountUrl = "https://www.sattracks.net/stsync/api/note/totalcount";
            var authCountResponse = GetResponse(AuthUrl, RequestCountUrl);
            TotalDataCount TotalCount = Newtonsoft.Json.JsonConvert.DeserializeObject<TotalDataCount>(authCountResponse);
            var LoopSize = (TotalCount.Data.TotalCount / 999) + 5;
            for (int i = 1; i < LoopSize; i++)
            {
                if (PageNo.HasValue && PageNo > 0 && i < PageNo)
                {
                    start = start + 999;
                    continue;
                }

                string RequestUrl = "https://www.sattracks.net/stsync/api/note?start=" + start + "&count=" + end;

                AgemniNoteList agemniCustomer = new AgemniNoteList();


                var authResponse = GetResponse(AuthUrl, RequestUrl);
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                try
                {

                    var agemniCustomer2 = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemniNoteList>(authResponse);
                    List<CustomerMigration> customerMigrations = _Util.Facade.CustomerFacade.GetAllCustomerMigration();

                    foreach (var item in agemniCustomer2.Data)
                    {
                        try
                        {
                            #region Insert
                            int CustomerId = new int();
                            Int32.TryParse(item.CustomerID, out CustomerId);
                            DateTime CreatedDate = new DateTime();
                            DateTime.TryParse(item.NoteDate, out CreatedDate);

                            DateTime UpdatedDate = new DateTime();
                            DateTime.TryParse(item.UpdatedDate, out UpdatedDate);



                            CustomerNote cusNote = new CustomerNote();
                            if (CustomerId > 0)
                            {
                                CustomerMigration cusAgemni = customerMigrations.Where(x => x.RefenrenceId == CustomerId).FirstOrDefault();//_Util.Facade.CustomerFacade.GetAgemniCustomerByReferenceId(CustomerId);
                                if (cusAgemni != null)
                                {
                                    //Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(cusAgemni.CustomerId);
                                    //if (cus != null)
                                    //{
                                    #region InsertOnNoteRemainder
                                    if (string.IsNullOrWhiteSpace(item.NoteHtml))
                                    {
                                        cusNote = new CustomerNote()
                                        {

                                            NoteType = item.notetype,
                                            CreatedDate = CreatedDate,
                                            CreatedBy = CurrentUser.UserId.ToString(),
                                            CreatedByUid = CurrentUser.UserId,
                                            CustomerId = cusAgemni.CustomerId,
                                            CompanyId = CurrentUser.CompanyId.Value,
                                            // Notes = "ID : " + item.Id + "</br> WorkOrderId : " + item.WorkOrderId + "</br> InvoiceID : " + item.InvoiceID + "</br> NoteText : " + item.NoteText + " </br> NoteType : " + item.notetype + " </br> ParentNoteId : " + item.ParentNoteId + "</br> UserID : " + item.UserID + " </br> AppointmentID : " + item.AppointmentID + "</br> ShowOnContract : " + item.ShowOnContract + "</br> ProgrammingQuoteID : " + item.ProgrammingQuoteID + "</br> PanelID : " + item.PanelID + "</br> NoteHtml : " + item.NoteHtml + "</br> UpdatedDate : " + item.UpdatedDate + "</br> Removed: " + item.Removed,
                                            Notes = item.NoteText,
                                        };
                                    }
                                    else
                                    {
                                        cusNote = new CustomerNote()
                                        {

                                            NoteType = "Agemni Note",
                                            CreatedDate = CreatedDate,
                                            CreatedBy = CurrentUser.UserId.ToString(),
                                            CreatedByUid = CurrentUser.UserId,
                                            CustomerId = cusAgemni.CustomerId,
                                            CompanyId = CurrentUser.CompanyId.Value,
                                            // Notes = "ID : " + item.Id + "</br> WorkOrderId : " + item.WorkOrderId + "</br> InvoiceID : " + item.InvoiceID + "</br> NoteText : " + item.NoteText + " </br> NoteType : " + item.notetype + " </br> ParentNoteId : " + item.ParentNoteId + "</br> UserID : " + item.UserID + " </br> AppointmentID : " + item.AppointmentID + "</br> ShowOnContract : " + item.ShowOnContract + "</br> ProgrammingQuoteID : " + item.ProgrammingQuoteID + "</br> PanelID : " + item.PanelID + "</br> NoteHtml : " + item.NoteHtml + "</br> UpdatedDate : " + item.UpdatedDate + "</br> Removed: " + item.Removed,
                                            Notes = item.NoteHtml,
                                        };
                                    }

                                    _Util.Facade.NotesFacade.InsertCustomerNote(cusNote);
                                    #endregion
                                    //}

                                }
                            }
                            #endregion
                        }
                        catch (Exception e)
                        {
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\InsertAgemniNote\NotInsertAgemniNote_Error_" + i + ".txt"), true))
                            {
                                file.WriteLine("Error at Note: " + item.Id);
                                file.Close();
                            }
                        }

                    }
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\InsertAgemniNote_" + i + ".txt"), true))
                    {
                        file.WriteLine("Inserted at: " + start);
                        file.Close();
                    }
                }
                catch (Exception ex)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\InsertAgemniNote\NotInsertAgemniNote_Error_" + i + ".txt"), true))
                    {
                        file.WriteLine("Not Inserted at: " + start);
                        file.Close();
                    }
                }

                start = start + 999;

            }

            return null;
        }

        public JsonReader AddMissingAgemniNote()
        {
            int start = 1;
            int end = 999;
            for (int i = 1; i < 800; i++)
            {
                string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";

                string RequestUrl = "https://www.sattracks.net/stsync/api/note?startDate=11/14/2019&endDate=12/04/2019&start=" + start + "&count=" + end;

                AgemniNoteList agemniCustomer = new AgemniNoteList();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var authResponse = GetResponse(AuthUrl, RequestUrl);
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                try
                {
                    var agemniCustomer2 = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemniNoteList>(authResponse);
                    foreach (var item in agemniCustomer2.Data)
                    {

                        int CustomerId = new int();
                        Int32.TryParse(item.CustomerID, out CustomerId);
                        DateTime CreatedDate = new DateTime();
                        DateTime.TryParse(item.NoteDate, out CreatedDate);

                        DateTime UpdatedDate = new DateTime();
                        DateTime.TryParse(item.UpdatedDate, out UpdatedDate);

                        CustomerNote cusNote = new CustomerNote();
                        List<CustomerNote> cusNoteList = new List<CustomerNote>();
                        if (CustomerId > 0)
                        {
                            CustomerMigration cusAgemni = _Util.Facade.CustomerFacade.GetAgemniCustomerByReferenceId(CustomerId);
                            if (cusAgemni != null)
                            {
                                Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(cusAgemni.CustomerId);

                                int flag = 1;
                                if (cus != null)
                                {
                                    cusNoteList = _Util.Facade.NotesFacade.GetAssignedNotesListByCustomerId(cus.CustomerId);
                                    if (cusNoteList.Count > 0)
                                    {
                                        foreach (var cusNoteitem in cusNoteList)
                                        {
                                            if (cusNoteitem.Notes == item.NoteHtml)
                                            {
                                                flag = 0;
                                            }
                                            else if (cusNoteitem.Notes == item.NoteText)
                                            {
                                                flag = 0;
                                            }
                                        }
                                        if (flag == 1)
                                        {
                                            #region InsertOnNoteRemainder
                                            if (string.IsNullOrWhiteSpace(item.NoteHtml))
                                            {
                                                cusNote = new CustomerNote()
                                                {

                                                    NoteType = item.notetype,
                                                    CreatedDate = CreatedDate,
                                                    CreatedBy = CurrentUser.UserId.ToString(),
                                                    CreatedByUid = CurrentUser.UserId,
                                                    CustomerId = cus.CustomerId,
                                                    CompanyId = CurrentUser.CompanyId.Value,
                                                    // Notes = "ID : " + item.Id + "</br> WorkOrderId : " + item.WorkOrderId + "</br> InvoiceID : " + item.InvoiceID + "</br> NoteText : " + item.NoteText + " </br> NoteType : " + item.notetype + " </br> ParentNoteId : " + item.ParentNoteId + "</br> UserID : " + item.UserID + " </br> AppointmentID : " + item.AppointmentID + "</br> ShowOnContract : " + item.ShowOnContract + "</br> ProgrammingQuoteID : " + item.ProgrammingQuoteID + "</br> PanelID : " + item.PanelID + "</br> NoteHtml : " + item.NoteHtml + "</br> UpdatedDate : " + item.UpdatedDate + "</br> Removed: " + item.Removed,
                                                    Notes = item.NoteText,
                                                };
                                            }
                                            else
                                            {
                                                cusNote = new CustomerNote()
                                                {

                                                    NoteType = item.notetype,
                                                    CreatedDate = CreatedDate,
                                                    CreatedBy = CurrentUser.UserId.ToString(),
                                                    CreatedByUid = CurrentUser.UserId,
                                                    CustomerId = cus.CustomerId,
                                                    CompanyId = CurrentUser.CompanyId.Value,
                                                    // Notes = "ID : " + item.Id + "</br> WorkOrderId : " + item.WorkOrderId + "</br> InvoiceID : " + item.InvoiceID + "</br> NoteText : " + item.NoteText + " </br> NoteType : " + item.notetype + " </br> ParentNoteId : " + item.ParentNoteId + "</br> UserID : " + item.UserID + " </br> AppointmentID : " + item.AppointmentID + "</br> ShowOnContract : " + item.ShowOnContract + "</br> ProgrammingQuoteID : " + item.ProgrammingQuoteID + "</br> PanelID : " + item.PanelID + "</br> NoteHtml : " + item.NoteHtml + "</br> UpdatedDate : " + item.UpdatedDate + "</br> Removed: " + item.Removed,
                                                    Notes = item.NoteHtml,
                                                };
                                            }

                                            _Util.Facade.NotesFacade.InsertCustomerNote(cusNote);
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        #region InsertOnNoteRemainder
                                        if (string.IsNullOrWhiteSpace(item.NoteHtml))
                                        {
                                            cusNote = new CustomerNote()
                                            {

                                                NoteType = item.notetype,
                                                CreatedDate = CreatedDate,
                                                CreatedBy = CurrentUser.UserId.ToString(),
                                                CreatedByUid = CurrentUser.UserId,
                                                CustomerId = cus.CustomerId,
                                                CompanyId = CurrentUser.CompanyId.Value,
                                                // Notes = "ID : " + item.Id + "</br> WorkOrderId : " + item.WorkOrderId + "</br> InvoiceID : " + item.InvoiceID + "</br> NoteText : " + item.NoteText + " </br> NoteType : " + item.notetype + " </br> ParentNoteId : " + item.ParentNoteId + "</br> UserID : " + item.UserID + " </br> AppointmentID : " + item.AppointmentID + "</br> ShowOnContract : " + item.ShowOnContract + "</br> ProgrammingQuoteID : " + item.ProgrammingQuoteID + "</br> PanelID : " + item.PanelID + "</br> NoteHtml : " + item.NoteHtml + "</br> UpdatedDate : " + item.UpdatedDate + "</br> Removed: " + item.Removed,
                                                Notes = item.NoteText,
                                            };
                                        }
                                        else
                                        {
                                            cusNote = new CustomerNote()
                                            {

                                                NoteType = item.notetype,
                                                CreatedDate = CreatedDate,
                                                CreatedBy = CurrentUser.UserId.ToString(),
                                                CreatedByUid = CurrentUser.UserId,
                                                CustomerId = cus.CustomerId,
                                                CompanyId = CurrentUser.CompanyId.Value,
                                                // Notes = "ID : " + item.Id + "</br> WorkOrderId : " + item.WorkOrderId + "</br> InvoiceID : " + item.InvoiceID + "</br> NoteText : " + item.NoteText + " </br> NoteType : " + item.notetype + " </br> ParentNoteId : " + item.ParentNoteId + "</br> UserID : " + item.UserID + " </br> AppointmentID : " + item.AppointmentID + "</br> ShowOnContract : " + item.ShowOnContract + "</br> ProgrammingQuoteID : " + item.ProgrammingQuoteID + "</br> PanelID : " + item.PanelID + "</br> NoteHtml : " + item.NoteHtml + "</br> UpdatedDate : " + item.UpdatedDate + "</br> Removed: " + item.Removed,
                                                Notes = item.NoteHtml,
                                            };
                                        }

                                        _Util.Facade.NotesFacade.InsertCustomerNote(cusNote);
                                        #endregion
                                    }

                                }

                            }
                        }


                    }
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\InsertAgemniNote_"+i+".txt"), true))
                    //{
                    //    file.WriteLine("Inserted at: " + start);
                    //    file.Close();
                    //}
                }
                catch (Exception ex)
                {
                    //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\InsertAgemniNote\NotInsertAgemniNote_"+i+".txt"), true))
                    //{
                    //    file.WriteLine("Not Inserted at: " + start);
                    //    file.Close();
                    //}
                }

                start = start + 999;

            }

            return null;
        }

        public JsonReader UpdateAgemniNote()
        {
            int start = 1;
            int end = 999;
            for (int i = 1; i < 800; i++)
            {
                string AuthUrl = "https://www.sattracks.net/stauth/api/login?sitedirname=st402722&login=DFWAPI&pwd=DfW2020";

                string RequestUrl = "https://www.sattracks.net/stsync/api/note?start=" + start + "&count=" + end;

                AgemniNoteList agemniCustomer = new AgemniNoteList();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var authResponse = GetResponse(AuthUrl, RequestUrl);
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                try
                {
                    var agemniCustomer2 = Newtonsoft.Json.JsonConvert.DeserializeObject<AgemniNoteList>(authResponse);
                    foreach (var item in agemniCustomer2.Data)
                    {


                        int CustomerId = new int();
                        Int32.TryParse(item.CustomerID, out CustomerId);
                        DateTime CreatedDate = new DateTime();
                        DateTime.TryParse(item.NoteDate, out CreatedDate);

                        DateTime UpdatedDate = new DateTime();
                        DateTime.TryParse(item.UpdatedDate, out UpdatedDate);

                        List<CustomerNote> cusNoteList = new List<CustomerNote>();
                        cusNoteList = _Util.Facade.NotesFacade.GetAllCustomerNote();
                        MissingNote missingNote = new MissingNote();

                        if (cusNoteList.Where(x => x.Notes == item.NoteText) != null)
                        {

                        }
                        else if (cusNoteList.Where(x => x.Notes == item.NoteHtml) != null)
                        {

                        }
                        else
                        {
                            missingNote = new MissingNote()
                            {
                                CustomerID = CustomerId,
                                NoteHtml = item.NoteHtml,
                                NoteText = item.NoteText,
                                CreatedDate = CreatedDate,
                                NoteType = item.notetype

                            };
                            _Util.Facade.NotesFacade.InsertMissingNote(missingNote);
                        }





                    }

                }
                catch (Exception ex)
                {

                }

                start = start + 999;

            }

            return null;
        }
        #endregion

        #region Data Migration(Employee) for Onit
        public JsonReader PullOnitEmployee()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<AAEmployeeDump> empDumpList = _Util.Facade.EmployeeFacade.AAEmployeeDumpList();
            Employee emp = new Employee();
            string FirstName = "";
            string LastName = "";
            string[] Name;
            if (empDumpList != null)
            {
                foreach (var item in empDumpList)
                {

                    Employee oldemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmailAddress(item.Email);
                    if (oldemp == null)
                    {
                        #region Insert Employee Table
                        if (!string.IsNullOrEmpty(item.Name))
                        {
                            Name = item.Name.Split(" ");
                            if (Name.Count() > 0)
                            {
                                FirstName = Name[0].ToString();
                                LastName = Name[1].ToString();
                            }
                            else
                            {
                                FirstName = item.Name;
                            }
                        }

                        emp = new Employee();
                        emp.UserId = Guid.NewGuid();
                        emp.FirstName = FirstName;
                        emp.LastName = LastName;
                        emp.Email = item.Email;
                        emp.Phone = item.Mobile;
                        emp.Street = item.Address;
                        emp.BrinksDealerUser = item.BrinksUserName;
                        emp.UserName = item.Email;
                        emp.Recruited = true;
                        emp.CompanyId = CurrentUser.CompanyId.Value;
                        emp.CreatedDate = item.Created;
                        emp.LastUpdatedDate = item.Updated;
                        if (item.Status == "Active")
                        {
                            emp.IsActive = true;
                        }
                        else
                        {
                            emp.IsActive = false;
                        }
                        _Util.Facade.EmployeeFacade.InsertEmployee(emp);
                        #endregion
                        #region Insert Userlogin Table
                        UserLogin userlogin = new UserLogin();
                        userlogin.UserId = emp.UserId;
                        userlogin.UserName = item.Email;
                        userlogin.LastUpdatedDate = item.Updated;
                        userlogin.PhoneNumber = item.Mobile;
                        userlogin.EmailAddress = item.Email;
                        userlogin.FirstName = FirstName;
                        userlogin.LastName = LastName;
                        userlogin.CompanyId = CurrentUser.CompanyId.Value;
                        if (item.Status == "Active")
                        {

                            userlogin.IsActive = true;
                        }
                        else
                        {
                            userlogin.IsActive = false;
                        }
                        long ulId = _Util.Facade.UserLoginFacade.InsertUserLogin(userlogin);
                        #endregion
                        #region Insert UserPermission Table
                        UserPermission up = new UserPermission();
                        up.UserId = userlogin.UserId;
                        up.CompanyId = CurrentUser.CompanyId.Value;
                        if (item.Roles == "csr")
                        {
                            up.PermissionGroupId = 18;
                        }
                        else if (item.Roles == "sales")
                        {
                            up.PermissionGroupId = 23;
                        }
                        else if (item.Roles == "tech")
                        {
                            up.PermissionGroupId = 5;
                        }
                        else
                        {
                            up.PermissionGroupId = 33;
                        }

                        _Util.Facade.PermissionFacade.InsertUserPermission(up);
                        #endregion

                        #region Insert UserCompany Table


                        UserCompany uc = new UserCompany();
                        uc.CompanyId = CurrentUser.CompanyId.Value;
                        uc.UserId = emp.UserId;
                        uc.IsDefault = true;
                        _Util.Facade.UserCompanyFacade.InsertUserCompany(uc);

                        UserOrganization ug = new UserOrganization();
                        ug.CompanyId = CurrentUser.CompanyId.Value;
                        ug.UserName = userlogin.UserName;
                        ug.IsActive = emp.IsActive;
                        _Util.Facade.UserOrganizationFacade.InsertUserOrganization(ug);
                        #endregion

                        #region Send Email
                        string cryptmessage = DESEncryptionDecryption.EncryptPlainTextToCipherText(ulId + "__" + item.Email + "__" + userlogin.LastUpdatedDate.ToString() + "__" + CurrentUser.CompanyId.Value);

                        //UtilHelper.GetCryptMessage(ulId + email + ul2.LastUpdatedDate.ToString());

                        VerifyEmail verifyEmail = new VerifyEmail();
                        verifyEmail.Name = string.Format("{0} {1}", FirstName, LastName);
                        string SiteURL = ConfigurationManager.AppSettings["SiteURL"];
                        verifyEmail.EmailVerificationLink = AppConfig.SiteDomain + string.Format("/accountverification/{1}/{0}", ulId, cryptmessage);
                        verifyEmail.ToEmail = item.Email;
                        bool Result = _Util.Facade.MailFacade.SendEmailVerify(verifyEmail, CurrentUser.CompanyId.Value);
                        #endregion
                    }

                }
            }

            return null;
        }
        #endregion

        public JsonResult GetCreditScore_v2(CustomerCreditScore CreditScore)
        {

            string result = "false";
            string responseContent = "";
            string score = "";
            string filehit = "";
            string TransId = "";
            string HtmlReports = "";
            double MinCreditScore = 0;
            CustomerCreditCheck creditcheck = new CustomerCreditCheck();
            try
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CustomerAdditionalContact secondaryCredit = new CustomerAdditionalContact();
                if (CreditScore.ContactId.HasValue && CreditScore.ContactId > 0)
                {
                    secondaryCredit = _Util.Facade.AdditionalContactFacade.GetById(CreditScore.ContactId.Value);
                    if (secondaryCredit != null)
                    {
                        CreditScore.FirstName = secondaryCredit.FirstName;
                        CreditScore.LastName = secondaryCredit.LastName;
                        CreditScore.STATE = secondaryCredit.CorpState;
                        CreditScore.ZIP = secondaryCredit.CorpZipCode;
                        CreditScore.CITY = secondaryCredit.CorpCity;
                        CreditScore.ADDRESS = secondaryCredit.CorpAddress;
                        CreditScore.SSN = secondaryCredit.SSN;
                        CreditScore.CustomerId = CreditScore.CustomerId;
                        CreditScore.BUREAU = CreditScore.BUREAU;
                        
                    }
                }
                IRestResponse response = _Util.Facade.CustomerFacade.GetCreditReportResponse(CreditScore, CurrentUser.CompanyId.Value);
                responseContent = response.Content;
                //System.IO.File.WriteAllText(HttpContext.Server.MapPath("~/Creditscore.txt"), responseContent);
                //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CreditScoreReport.txt"), true))
                //{
                //    file.WriteLine(string.Format("Credit Report: ", responseContent));
                //}
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseContent);
                var scoreXML = doc.SelectSingleNode("//score");
                var TransIdXML = doc.SelectSingleNode("//Transid");
                var HtmlReportXML = doc.SelectSingleNode("//HTML_Reports");
                Customer cus = new Customer();
                cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CreditScore.CustomerId);
                double CreditScoreValue = 0;

                if (scoreXML != null)
                {
                    score = scoreXML.InnerText;
                    double.TryParse(score, out CreditScoreValue);
                }
                if (TransIdXML != null)
                {
                    TransId = TransIdXML.InnerText;
                    CustomerExtended extended = new CustomerExtended();
                    extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                    if (extended != null)
                    {
                        extended.CreditTransectionId = TransId;
                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                    }
                    else
                    {
                        extended.CustomerId = cus.CustomerId;
                        extended.CreditTransectionId = TransId;
                        _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
                    }

                }
                PackageCustomer packageCus = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerId(CreditScore.CustomerId);
                if (packageCus != null)
                {
                    SmartPackage smartPackage = _Util.Facade.PackageFacade.GetSmartPackageByIdAndCompanyId(packageCus.PackageId, CurrentUser.CompanyId.Value);
                    if (smartPackage != null)
                    {
                        if (smartPackage.MinCredit.HasValue)
                        {
                            MinCreditScore = smartPackage.MinCredit.Value;
                        }
                    }
                }
                if (MinCreditScore == 0)
                {
                    filehit = "Undefined";
                }
                else if (MinCreditScore > CreditScoreValue)
                {
                    filehit = "Fail";

                }
                else
                {
                    filehit = "Pass";
                }
                if (HtmlReportXML != null)
                {
                    HtmlReports = HtmlReportXML.InnerText;
                }


                int ScoreValue = 0;
                int.TryParse(score, out ScoreValue);
                cus.CreditScoreValue = ScoreValue;

                if (cus.CreditScoreValue != null && cus.CreditScoreValue > 0)
                {
                    CreditScoreGrade creditscoreGrade = _Util.Facade.CustomerFacade.GetCreditScoreGradeByScoreRange(cus.CreditScoreValue.Value);
                    if (creditscoreGrade != null)
                    {
                        cus.CreditScore = creditscoreGrade.ID.ToString();
                    }

                }
                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                CustomerCreditScoreReport creditscoreBody = new CustomerCreditScoreReport();
                creditscoreBody.ContentBody = HtmlReports;
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Leads/ViewCreditScore.cshtml", creditscoreBody)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                string filename = ConfigurationManager.AppSettings["File.CreditReports"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                filename = string.Format(filename, comname);
                Random rand = new Random();
                string ReportName = cus.Id + "_CreditReport_" + rand.Next().ToString() + ".pdf";
                filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + ReportName;
                string Serverfilename = FileHelper.GetFileFullPath(filename);
                FileHelper.SaveFile(applicationPDFData, Serverfilename);


                creditcheck.CustomerId = CreditScore.CustomerId;
                creditcheck.FirstName = CreditScore.FirstName;
                creditcheck.LastName = CreditScore.LastName;
                creditcheck.CreditAddress = CreditScore.ADDRESS;
                creditcheck.CreditBureau = CreditScore.BUREAU;
                creditcheck.CreditCity = CreditScore.CITY;
                creditcheck.CreditState = CreditScore.STATE;
                creditcheck.CreditZipCode = CreditScore.ZIP;
                creditcheck.ReportPdfLink = filename;
                creditcheck.RepontPdfName = ReportName;
                creditcheck.DateOfBirth = DateTime.Now;
                creditcheck.CreatedBy = CurrentUser.UserId;
                creditcheck.CreditCheckDate = DateTime.Now;
                creditcheck.Hit = filehit;
                creditcheck.Score = score;
                creditcheck.TransectionId = TransId;
                if(CreditScore.IsSoftCheck == true)
                {
                    creditcheck.CreditCheckDesc = CreditScore.BUREAU + " Soft Pull.";
                }
                else
                {
                    creditcheck.CreditCheckDesc = CreditScore.BUREAU + " Hard Pull.";
                }
            
         
                creditcheck.CompanyId = CurrentUser.CompanyId.Value;
                if(CreditScore.IsSoftCheck == true)
                {
                    creditcheck.CreditCheckDesc = CreditScore.BUREAU + "Soft Pull.";
                }
                else
                {
                    creditcheck.CreditCheckDesc = CreditScore.BUREAU + "Hard Pull.";
                }
               
              
                _Util.Facade.CustomerFacade.InsertCustomerCreditCheck(creditcheck);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = "true";
                }
                #region Update SecondaryContact
                List<CustomerAdditionalContact> contactList = new List<CustomerAdditionalContact>();
                if (CreditScore.ContactId.HasValue && secondaryCredit != null)
                {
                    contactList = _Util.Facade.AdditionalContactFacade.GetAllAdditionalContactByCustomerId(creditcheck.CustomerId);
                    if (contactList != null)
                    {
                        foreach (var item in contactList)
                        {
                            item.IsCreditUsed = false;
                            _Util.Facade.AdditionalContactFacade.UpdateAdditionalContact(item);
                        }
                    }
                    secondaryCredit.IsCreditUsed = true;
                    secondaryCredit.CreditScore = cus.CreditScoreValue.ToString();
                    secondaryCredit.ReportPdfLink = filename;
                    _Util.Facade.AdditionalContactFacade.UpdateAdditionalContact(secondaryCredit);
                }
                #endregion
            }
            catch (Exception ex)
            {

            }

            return Json(new { result = result, response = responseContent, Score = score, ReportName = creditcheck.RepontPdfName, ReportLink = creditcheck.ReportPdfLink, HitStatus = filehit });
        }

        public JsonResult GetCreditScore(CustomerCreditScore CreditScore)
        {

            string result = "false";
            string responseContent = "";
            string score = "";
            string filehit = "";
            string TransId = "";
            string HtmlReports = "";
            double MinCreditScore = 0;
            string FileKey = string.Empty;
            CustomerCreditCheck creditcheck = new CustomerCreditCheck();
            try
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CustomerAdditionalContact secondaryCredit = new CustomerAdditionalContact();
                if (CreditScore.ContactId.HasValue && CreditScore.ContactId > 0)
                {
                    secondaryCredit = _Util.Facade.AdditionalContactFacade.GetById(CreditScore.ContactId.Value);
                    if (secondaryCredit != null)
                    {
                        CreditScore.FirstName = secondaryCredit.FirstName;
                        CreditScore.LastName = secondaryCredit.LastName;
                        CreditScore.STATE = secondaryCredit.CorpState;
                        CreditScore.ZIP = secondaryCredit.CorpZipCode;
                        CreditScore.CITY = secondaryCredit.CorpCity;
                        CreditScore.ADDRESS = secondaryCredit.CorpAddress;
                        CreditScore.SSN = secondaryCredit.SSN;
                        CreditScore.CustomerId = CreditScore.CustomerId;
                        CreditScore.BUREAU = CreditScore.BUREAU;

                    }
                }
                IRestResponse response = _Util.Facade.CustomerFacade.GetCreditReportResponse(CreditScore, CurrentUser.CompanyId.Value);
                responseContent = response.Content;
                //System.IO.File.WriteAllText(HttpContext.Server.MapPath("~/Creditscore.txt"), responseContent);
                //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CreditScoreReport.txt"), true))
                //{
                //    file.WriteLine(string.Format("Credit Report: ", responseContent));
                //}
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseContent);
                var scoreXML = doc.SelectSingleNode("//score");
                var TransIdXML = doc.SelectSingleNode("//Transid");
                var HtmlReportXML = doc.SelectSingleNode("//HTML_Reports");
                Customer cus = new Customer();
                cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CreditScore.CustomerId);
                double CreditScoreValue = 0;

                if (scoreXML != null)
                {
                    score = scoreXML.InnerText;
                    double.TryParse(score, out CreditScoreValue);
                }
                if (TransIdXML != null)
                {
                    TransId = TransIdXML.InnerText;
                    CustomerExtended extended = new CustomerExtended();
                    extended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(cus.CustomerId);
                    if (extended != null)
                    {
                        extended.CreditTransectionId = TransId;
                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(extended);
                    }
                    else
                    {
                        extended.CustomerId = cus.CustomerId;
                        extended.CreditTransectionId = TransId;
                        _Util.Facade.CustomerFacade.InsertCustomerExtended(extended);
                    }

                }
                PackageCustomer packageCus = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerId(CreditScore.CustomerId);
                if (packageCus != null)
                {
                    SmartPackage smartPackage = _Util.Facade.PackageFacade.GetSmartPackageByIdAndCompanyId(packageCus.PackageId, CurrentUser.CompanyId.Value);
                    if (smartPackage != null)
                    {
                        if (smartPackage.MinCredit.HasValue)
                        {
                            MinCreditScore = smartPackage.MinCredit.Value;
                        }
                    }
                }
                if (MinCreditScore == 0)
                {
                    filehit = "Undefined";
                }
                else if (MinCreditScore > CreditScoreValue)
                {
                    filehit = "Fail";

                }
                else
                {
                    filehit = "Pass";
                }
                if (HtmlReportXML != null)
                {
                    HtmlReports = HtmlReportXML.InnerText;
                }


                int ScoreValue = 0;
                int.TryParse(score, out ScoreValue);
                cus.CreditScoreValue = ScoreValue;

                if (cus.CreditScoreValue != null && cus.CreditScoreValue > 0)
                {
                    CreditScoreGrade creditscoreGrade = _Util.Facade.CustomerFacade.GetCreditScoreGradeByScoreRange(cus.CreditScoreValue.Value);
                    if (creditscoreGrade != null)
                    {
                        cus.CreditScore = creditscoreGrade.ID.ToString();
                    }

                }
                _Util.Facade.CustomerFacade.UpdateCustomer(cus);
                CustomerCreditScoreReport creditscoreBody = new CustomerCreditScoreReport();
                creditscoreBody.ContentBody = HtmlReports;
                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Leads/ViewCreditScore.cshtml", creditscoreBody)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);


                #region File save

                //string filename = ConfigurationManager.AppSettings["File.CreditReports"];
                //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                //filename = string.Format(filename, comname);
                //Random rand = new Random();
                //string ReportName = cus.Id + "_CreditReport_" + rand.Next().ToString() + ".pdf";

                //filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + ReportName;
                //string Serverfilename = FileHelper.GetFileFullPath(filename);
                //FileHelper.SaveFile(applicationPDFData, Serverfilename);
                #endregion

                //// "mayur" AWS S3 Changes //// Start

                #region File Save on AWS S3

                 Random rand = new Random();
                 string filename = ConfigurationManager.AppSettings["File.CreditReports"];
                 var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                 string FilePath = string.Format(filename, comname);
           
                 string ReportName = cus.Id + "_CreditReport_" + rand.Next().ToString() + ".pdf";
                 FilePath += DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString();

             
                 FileKey = string.Format($"{FilePath}/{ReportName}");

                var returnurl = "";

                var task = Task.Run(async () => {
                    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                    await AWSobject.UploadFile(FileKey, applicationPDFData);
                    await AWSobject.MakePublic(ReportName, FilePath);
                });

                task.Wait();

                /// "mayur" used thread for async s3 methods : start

                //Thread thread = new Thread(async () =>
                //{
                //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();
                //    await AWSobject.UploadFile(FileKey, applicationPDFData);
                //    await AWSobject.MakePublic(ReportName, FilePath);
                //});
                //thread.Start();

                // "mayur" used thread for async s3 methods : End
                //Thread.Sleep(5000);
                returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
                returnurl = returnurl + FileKey;
                // ViewBag.ReturnUrl = returnurl;
                // ViewBag.FileName = ReportName;

                #endregion

                //// "mayur" AWS S3 Changes //// End
                creditcheck.CustomerId = CreditScore.CustomerId;
                creditcheck.FirstName = CreditScore.FirstName;
                creditcheck.LastName = CreditScore.LastName;
                creditcheck.CreditAddress = CreditScore.ADDRESS;
                creditcheck.CreditBureau = CreditScore.BUREAU;
                creditcheck.CreditCity = CreditScore.CITY;
                creditcheck.CreditState = CreditScore.STATE;
                creditcheck.CreditZipCode = CreditScore.ZIP;
                creditcheck.ReportPdfLink = FileKey;
                creditcheck.RepontPdfName = ReportName;
                creditcheck.DateOfBirth = DateTime.Now;
                creditcheck.CreatedBy = CurrentUser.UserId;
                creditcheck.CreditCheckDate = DateTime.Now;
                creditcheck.Hit = filehit;
                creditcheck.Score = score;
                creditcheck.TransectionId = TransId;
                if (CreditScore.IsSoftCheck == true)
                {
                    creditcheck.CreditCheckDesc = CreditScore.BUREAU + " Soft Pull.";
                }
                else
                {
                    creditcheck.CreditCheckDesc = CreditScore.BUREAU + " Hard Pull.";
                }

                creditcheck.CompanyId = CurrentUser.CompanyId.Value;
                if (CreditScore.IsSoftCheck == true)
                {
                    creditcheck.CreditCheckDesc = CreditScore.BUREAU + "Soft Pull.";
                }
                else
                {
                    creditcheck.CreditCheckDesc = CreditScore.BUREAU + "Hard Pull.";
                }

                _Util.Facade.CustomerFacade.InsertCustomerCreditCheck(creditcheck);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result = "true";
                }
                #region Update SecondaryContact
                List<CustomerAdditionalContact> contactList = new List<CustomerAdditionalContact>();
                if (CreditScore.ContactId.HasValue && secondaryCredit != null)
                {
                    contactList = _Util.Facade.AdditionalContactFacade.GetAllAdditionalContactByCustomerId(creditcheck.CustomerId);
                    if (contactList != null)
                    {
                        foreach (var item in contactList)
                        {
                            item.IsCreditUsed = false;
                            _Util.Facade.AdditionalContactFacade.UpdateAdditionalContact(item);
                        }
                    }
                    secondaryCredit.IsCreditUsed = true;
                    secondaryCredit.CreditScore = cus.CreditScoreValue.ToString();
                    secondaryCredit.ReportPdfLink = filename;
                    _Util.Facade.AdditionalContactFacade.UpdateAdditionalContact(secondaryCredit);
                }
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(FileKey+" | "+ex);
            }

            return Json(new { result = result, response = responseContent, Score = score, ReportName = creditcheck.RepontPdfName, ReportLink = creditcheck.ReportPdfLink, HitStatus = filehit });
        }
        public ActionResult AgreementPdf()
        {
            return new Rotativa.ViewAsPdf("Agreement")
            {
                //FileName = "TestView.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = new Margins(10, 2, 10, 3),
            };
        }

        public ActionResult Time()
        {
            var value = "zUQ3eolkYjyuZCWQEBe5HQ==";
            var byteValye = Convert.FromBase64String(value);
            var byteValye2 = Convert.ToString(byteValye[20], 2).PadLeft(8, '0');
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }
        public int asd()
        {
            DateTime dt = new DateTime(2018, 8, 1);
            string asd = DateTime.Now.ToString("HH:mm:ss");
            return GetIso8601WeekOfYear(dt);
        }
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public string zxc(int week)
        {
            //en-US
            //bn-BD
            //fr-FR

            DateTime ads = new DateTime(2019, 3, 1);

            CultureInfo ci = new CultureInfo("fr-FR");
            return FirstDateOfWeek(2018, week, ci).ToString("MM//dd//yyyy");
        }



        #region Forte

        public JsonResult GetForteAuthorization(string APIAccessID, string APISecureKey)
        {
            //https://explore.postman.com/api/1172/forte-rest-api-v3

            string authheadertext = Convert.ToBase64String(Encoding.Default.GetBytes(APIAccessID + ":" + APISecureKey)).Trim();


            return Json(authheadertext, JsonRequestBehavior.AllowGet);
        }

        #endregion






























































































































        //public JsonResult KickBoxTest(string EmailAddress, string CompanyId)
        //{
        //    //string apiKey = "test_7f5bee7be1545be05a1b80099b4a971544c9cb5b4009b01835ee9b4decaa512b";
        //    string apiKey = "";
        //    bool IsValidEmail = false;
        //    GlobalSetting GlobSet = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("KickBoxApiKey", CompanyId);
        //    if(GlobSet != null && !string.IsNullOrWhiteSpace(GlobSet.Value) )
        //    {
        //        apiKey = GlobSet.Value;
        //    }
        //    else
        //    {
        //        return Json(IsValidEmail, JsonRequestBehavior.AllowGet);
        //    }

        //    //production key;inan.piistech@gmail.com;live_efd7d9a033c4c0d75c186fcdfa4c371517fd70c5d3d0e1295ce3e430065c78d8;

        //    //var asd = HS.Kickbox.KickBoxApi.VerifyWithResponse(EmailAddress, apiKey);
        //    if (EmailAddress.IsValidEmailAddress())
        //    {
        //        ExtendedKickBoxResponse response = HS.Kickbox.KickBoxApi.VerifyWithResponse(EmailAddress, apiKey);
        //        IsValidEmail = response.Result == Result.Deliverable;
        //        //return Json(response, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(IsValidEmail, JsonRequestBehavior.AllowGet);

        //}
        //public JsonResult UCCAA()
        //{
        //    InterfaceService iser = new InterfaceService();
        //    string userName = "RMRCloudDev";
        //    string password = "C$M@rketingR0x";
        //    string newPassword = "";
        //    string applicationName = "";
        //    string applicationVersion = "";
        //    string clientPlatform = "";
        //    string impersonatedUserName = "";
        //    string captchaResponse = "";
        //    byte[] loginProcessId = new byte[0];
        //    string securityCode = "";

        //    //SiteGroupNumber: 5017
        //    //AccountRange: 50170000 - 50179999

        //    var Response = iser.Login(userName, password, newPassword, applicationName, applicationVersion,
        //        clientPlatform, impersonatedUserName, captchaResponse, loginProcessId, securityCode);

        //    return Json(true);
        //}
        public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }
        public JsonResult CMS(string text)
        {
            string Date = DateTime.Now.ToString("MMMM-yyyy");
            return Json(MD5Encryption.GetMD5HashData(text), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHash()
        {
            List<int> HashList = new List<int>();
            List<int> Common = new List<int>();
            for (int i = 0; i < 100000; i++)
            {
                int hash = Guid.NewGuid().GetHashCode();
                if (HashList.Contains(hash))
                {
                    Common.Add(hash);
                }
                else
                {
                    HashList.Add(hash);
                }

            }

            return Json(Common, JsonRequestBehavior.AllowGet);
        }
        public string Encode(string Id)
        {
            return DESEncryptionDecryption.EncryptPlainTextToCipherText(Id);
        }

        public JsonResult Auth(string trKey = "6k36G55NuMbz4UjM", string ApiId = "8bh6VC5dj", string Prod = "0")
        {

            #region No longer in use
            ////string AuthTransactionKey = "6k36G55NuMbz4UjM";
            ////string ApiLoginId = "8bh6VC5dj"; 
            //string[] SubscriptionIds = { "37162451"
            //        //, "32393634", "36576976", "36747787", "37493606", "32392627","36570138", "32370890", "32347535", "37642169", "35797627", "36570310", "32392580", "32331289", "32341693", "35834110", "32371597"
            //    //,"32371434", "32347804", "36524277", "36394283", "32289331", "40744204", "35863135","32365683", "40613362", "38501939", "37669140", "37539368", "32341974", "35803449","32341676", "35206881", "32341485", "32416814", "32365960"
            //    //,"32365987", "35810940","32365605", "32365587", "32342248", "36461488", "32341895","32347570", "32414447", "32347441", "32342005", "32341971", "32341805", "32809040", "32328006","32268715", "32328253", "32328743", "32318429" 
            //    //,"32273187", "32329217", "35796726","32400335", "32329254", "35943414", "32329310", "35833257", "32328810"
            //    ,"34550486"
            //};
            #endregion

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            //string TransactionKey = _Util.Facade.GlobalSettingsFacade.GetAuthTransactionKeyByCompanyId(CurrentUser.CompanyId.Value, false);
            //string APILoginId = _Util.Facade.GlobalSettingsFacade.GetAuthAPILoginIdByCompanyId(CurrentUser.CompanyId.Value, false);


            bool AuthorizeInProduction = false;
            GlobalSetting globset2 = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("Authorize.NetInProduction", CurrentUser.CompanyId.Value);
            if (globset2 != null && globset2.Value.ToLower() == "true")
            {
                AuthorizeInProduction = true;
            }
            string ApiLoginIdCC = /*"7xZCH5KsE2e"*/_Util.Facade.GlobalSettingsFacade.GetAuthAPILoginIdByCompanyId(CurrentUser.CompanyId.Value, false);
            string TransactionKeyCC = /*"232t7YP63K25tnN7";*/_Util.Facade.GlobalSettingsFacade.GetAuthTransactionKeyByCompanyId(CurrentUser.CompanyId.Value, false);

            string ApiLoginACH = /*"7k29Xb2ueQ7";*/_Util.Facade.GlobalSettingsFacade.GetAuthAPILoginIdByCompanyId(CurrentUser.CompanyId.Value, true);
            string TransactionKeyACH = /*"8Nm8Jg7eu7P2T8V4";*/_Util.Facade.GlobalSettingsFacade.GetAuthTransactionKeyByCompanyId(CurrentUser.CompanyId.Value, true);



            List<Customer> SubscribedCustomer = _Util.Facade.CustomerFacade.GetSubscribedAllCustomer(true);

            List<string> Status = new List<string>();
            foreach (var item in SubscribedCustomer)
            {
                if (item.PaymentMethod == LabelHelper.PaymentMethod.ACH)
                {
                    trKey = TransactionKeyACH;
                    ApiId = ApiLoginACH;
                    var GetSub = GetSubscription.Run(ApiId, trKey, item.AuthorizeRefId, AuthorizeInProduction);
                    if (GetSub == null || GetSub.subscription == null)
                    {
                        //trying with CC
                        trKey = TransactionKeyCC;
                        ApiId = ApiLoginIdCC;
                        GetSub = GetSubscription.Run(ApiId, trKey, item.AuthorizeRefId, AuthorizeInProduction);
                    }

                    if (GetSub != null && GetSub.subscription != null)
                    {
                        if (item.SubscriptionStatus != GetSub.subscription.status.ToString())
                        {
                            item.SubscriptionStatus = GetSub.subscription.status.ToString();
                            _Util.Facade.CustomerFacade.UpdateCustomer(item);
                        }
                    }
                }
                else if (item.PaymentMethod == LabelHelper.PaymentMethod.CreditCard)
                {
                    trKey = TransactionKeyCC;
                    ApiId = ApiLoginIdCC;
                    var GetSub = GetSubscription.Run(ApiId, trKey, item.AuthorizeRefId, AuthorizeInProduction);
                    if (GetSub == null || GetSub.subscription == null)
                    {
                        //trying with ach
                        trKey = TransactionKeyACH;
                        ApiId = ApiLoginACH;
                        GetSub = GetSubscription.Run(ApiId, trKey, item.AuthorizeRefId, AuthorizeInProduction);
                    }
                    if (GetSub != null && GetSub.subscription != null)
                    {
                        if (item.SubscriptionStatus != GetSub.subscription.status.ToString())
                        {
                            item.SubscriptionStatus = GetSub.subscription.status.ToString();
                            _Util.Facade.CustomerFacade.UpdateCustomer(item);
                        }
                    }
                }

            }

            return Json(new { result = "Ki r komu", Status = Status }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult SubscriptionStatus()
        {

            //string[] SubscriptionIds = { "34917142","35594132","34208725","32785204","34967626","37477828","40798542","34935707","35595063",
            // "36191786","36570619","35217403","36461488","35790324","36743074","37798417","32341151","33714794","40455966",
            // "36917818","34931642","35867657","35893973","35910996","32365657","32365657","32365657","37744175","35000712",
            // "32365657","32365657","32365657","32414810","35795531","32289299","32318107","34919667","32392442",
            // "34937660","34921661","35608161","35973570","36987129","35031874","34507928","34914393","34894164",
            // "35867570","36099495","36097825","36174453","36332337","36998553","36720402","37081659","37661856" };

            string[] SubscriptionIds ={ "34917142","35594132","34208725","32785204","34967626","37477828","40798542","34919667",
                                        "34935707","36191786","36570619","35217403","36461488","35790324","36743074","37798417",
                                        "32341151","36917818","34931642","35867657","35893973","35910996","32365657","37167077",
                                        "32341184","37436793","35915841","36191830","32414810","35795531","32289299","32318107",
                                        "32392442","34937660","34921661","35608161","35973570","36987129","35031874","34507928",
                                        "34914393","34894164","35000712","37744175","35595063","35867570","36099495","36097825",
                                        "36174453","36332337","36998553","36720402","37081659","37661856","40455966","33714794" };

            string ApiLoginIdCC = "7xZCH5KsE2e";
            string TransactionKeyCC = "232t7YP63K25tnN7";

            string ApiLoginACH = "7k29Xb2ueQ7";
            string TransactionKeyACH = "8Nm8Jg7eu7P2T8V4";
            bool AuthorizeInProduction = true;
            int i = 1;
            foreach (var item in SubscriptionIds)
            {
                string trKey = TransactionKeyACH;
                string ApiId = ApiLoginACH;
                string Method = "ACH";
                var GetSub = GetSubscription.Run(ApiId, trKey, item, AuthorizeInProduction);
                if (GetSub == null || GetSub.subscription == null)
                {
                    Method = "CC";
                    //trying with CC
                    trKey = TransactionKeyCC;
                    ApiId = ApiLoginIdCC;
                    GetSub = GetSubscription.Run(ApiId, trKey, item, AuthorizeInProduction);
                }

                if (GetSub != null && GetSub.subscription != null)
                {
                    //GetSub.subscription.name
                    //Method

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\Subscription.txt"), true))
                    {
                        file.WriteLine(string.Format("{0}.ID: {5} Method: {1} Name: {2} Amount: {3} Status: {4}", i, Method, GetSub.subscription.name, GetSub.subscription.amount, GetSub.subscription.status, item));
                    }

                }
                i++;
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public JsonResult EmployeeTimeClockSet()
        {
            int result = 0;
            List<TimeClock> TimeClockList = new List<TimeClock>();

            TimeClockList = _Util.Facade.TimeClockFacade.GetAllTimeClock().OrderBy(x => x.UserId).ThenBy(s => s.Time).ToList();

            List<EmployeeTimeClock> EmployeeTimeClockList = new List<EmployeeTimeClock>();

            int checkTimeClockList = 0;
            foreach (var item in TimeClockList)
            {
                if (EmployeeTimeClockList.LastOrDefault() != null && EmployeeTimeClockList.LastOrDefault().UserId == item.UserId && item.Type == "Clock Out")
                {
                    if (EmployeeTimeClockList.LastOrDefault().ClockOutTime != null)
                    {
                        EmployeeTimeClock NewEmployeeTimeClock = new EmployeeTimeClock();
                        NewEmployeeTimeClock.UserId = item.UserId;
                        NewEmployeeTimeClock.ClockInTime = new DateTime(2000, 1, 1);
                        NewEmployeeTimeClock.ClockOutTime = item.Time;
                        NewEmployeeTimeClock.ClockOutLat = item.Lat;
                        NewEmployeeTimeClock.ClockOutLng = item.Lng;
                        NewEmployeeTimeClock.ClockOutNote = item.Note;
                        NewEmployeeTimeClock.ClockOutCreatedBy = item.CreatedBy;
                        NewEmployeeTimeClock.ClockedInSeconds = 0;
                        NewEmployeeTimeClock.LastUpdateBy = item.LastUpdateBy;
                        NewEmployeeTimeClock.LastUpdatedDate = item.LastUpdatedDate;
                        EmployeeTimeClockList.Add(NewEmployeeTimeClock);
                    }
                    else
                    {
                        EmployeeTimeClock OldEmployeeTimeClock = new EmployeeTimeClock();
                        // EmployeeTimeClockList.LastOrDefault().UserId = item.UserId;
                        EmployeeTimeClockList.LastOrDefault().ClockOutTime = item.Time;
                        EmployeeTimeClockList.LastOrDefault().ClockOutLat = item.Lat;
                        EmployeeTimeClockList.LastOrDefault().ClockOutLng = item.Lng;
                        EmployeeTimeClockList.LastOrDefault().ClockOutNote = item.Note;

                        EmployeeTimeClockList.LastOrDefault().ClockOutCreatedBy = item.CreatedBy;
                        EmployeeTimeClockList.LastOrDefault().ClockedInSeconds = item.ClockedInMinutes;
                        EmployeeTimeClockList.LastOrDefault().LastUpdateBy = item.LastUpdateBy;
                        EmployeeTimeClockList.LastOrDefault().LastUpdatedDate = item.LastUpdatedDate;

                    }

                }
                else
                {
                    EmployeeTimeClock NewEmployeeTimeClock = new EmployeeTimeClock();
                    NewEmployeeTimeClock.UserId = item.UserId;
                    NewEmployeeTimeClock.ClockInTime = item.Time;
                    //NewEmployeeTimeClock.clockin = item.Type;
                    NewEmployeeTimeClock.ClockInLat = item.Lat;
                    NewEmployeeTimeClock.ClockInLng = item.Lng;
                    NewEmployeeTimeClock.ClockInNote = item.Note;

                    NewEmployeeTimeClock.ClockInCreatedBy = item.CreatedBy;
                    NewEmployeeTimeClock.ClockedInSeconds = item.ClockedInMinutes;
                    NewEmployeeTimeClock.LastUpdateBy = item.LastUpdateBy;
                    NewEmployeeTimeClock.LastUpdatedDate = item.LastUpdatedDate;
                    EmployeeTimeClockList.Add(NewEmployeeTimeClock);


                }

            }
            foreach (var item in EmployeeTimeClockList)
            {
                if (item.ClockInTime == null || item.ClockOutTime == null)
                {
                    item.ClockedInSeconds = 0;
                }
                else
                {
                    TimeSpan? t = item.ClockOutTime - item.ClockInTime;
                    item.ClockedInSeconds = (int)t.Value.TotalSeconds;
                }

                result = _Util.Facade.EmployeeTimeClockFacade.InsertEmployeeTimeClock(item);


            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public string DateCheck(string Start, string End)
        {
            DateTime d1 = "".ToDateTime();
            DateTime d2 = "08/30/2018".ToDateTime();
            DateTime d3 = "08/32/2018".ToDateTime();
            DateTime d4 = "08/30/2018 ".ToDateTime();
            DateTime d5 = "08/30/2018 12:12".ToDateTime();
            DateTime d6 = "08/30/2018 13:12".ToDateTime();
            DateTime d7 = "08/30/2018 13:12:12".ToDateTime();
            DateTime d8 = "08/30/2018 10:12:12 AM".ToDateTime();
            DateTime d9 = "08/30/2018 10:12:12 PM".ToDateTime();
            DateTime d10 = "08/30/2018 13:12:12 PM".ToDateTime();

            return "";
        }
        public string E164(string N)
        {
            PhoneNumberUtil util = PhoneNumberUtil.GetInstance();
            PhoneNumber number = util.Parse(N, "US");
            string no = util.Format(number, PhoneNumberFormat.E164);
            return no;
        }
        //public JsonResult checkmail()
        //{
        //   _Util.Facade.MailFacade.HelloEmail("inan@piistech.com", "EmailSubject", "EmailBody").Wait();

        //    return Json(true, JsonRequestBehavior.AllowGet);
        //}

        //[Shariful-16-9-19]
        //public JsonResult checkmail()
        //{
        //    var result = false;
        //    Customer objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(new Guid("864ac8b1-7b00-409b-a36d-46e7d47b6571"));
        //    DeclineMail DeclineMail = new DeclineMail();
        //    DeclineMail.CustomerName = objcus.FirstName + " " + objcus.LastName;
        //    DeclineMail.ToEmail = objcus.EmailAddress;
        //    DeclineMail.DeclinationReason = "You didn't complete payment";

        //    result=_Util.Facade.MailFacade.DeclineMail(DeclineMail, new Guid("ef30d449-e244-405e-a286-867b267edbdf")); 

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        //[~Shariful-16-9-19]

        [Authorize]
        public JsonResult Decrypt(string text)
        {
            return Json(new { result = DESEncryptionDecryption.DecryptCipherTextToPlainText(text) }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public JsonResult Encrypt(string text)
        {
            return Json(new { result = DESEncryptionDecryption.EncryptPlainTextToCipherText(text) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult HaHa()
        {

            int i = 0;
            double j = 30;
            double k = 30;
            for (i = 0; ; i++)
            {
                k = k / 2;
                j += k;
                i++;
                if (j >= 60 || i == 999999999)
                {
                    return Json(i);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GS(string A)
        {
            string TransactionKey = "232t7YP63K25tnN7";
            string APILoginId = "7xZCH5KsE2e";

            var result = GetSubscription.Run(APILoginId, TransactionKey, A, true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SubscriptionList(string RefId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //string TransactionKey = "8Nm8Jg7eu7P2T8V4";
            //string APILoginId = "7k29Xb2ueQ7";

            string TransactionKey = "232t7YP63K25tnN7";
            string APILoginId = "7xZCH5KsE2e";


            #region SubscriptionList
            string CCSubscriptionList = @"'32268715','32269607','32270444','32270478','32270525','32270722','32272115','32272167','32272366','32272494'
                ,'32272797','32272872','32273008','32273163','32273187','32273472','32288513','32289299','32289331','32289841'
                ,'32296333','32296410','32296591','32296897','32316209','32316243','32318034','32318107','32318396','32318429'
                ,'32319955','32320067','32320129','32320162','32320582','32320917','32321619','32327981','32328006','32328253'
                ,'32328743','32328810','32329217','32329254','32329310','32330177','32330531','32331289','32333357','32341151'
                ,'32341184','32341485','32341604','32341627','32341652','32341676','32341693','32341715','32341805','32341895'
                ,'32341971','32341974','32342005','32342248','32347441','32347570','32347804','32365564','32365587','32365605'
                ,'32365657','32365683','32365960','32365987','32366157','32366591','32368487','32370890','32371203','32371434'
                ,'32371597','32392251','32392384','32392442','32392580','32392627','32392675','32393134','32393634','32394248'
                ,'32394274','32400335','32402580','32402843','32414447','32414810','32416814','32418067','32478649','32672857'
                ,'32735391','32809040','32907557','32908126','32908414','32950996','32975692','33331346','33420670','33421596'
                ,'33714794','33804535','33806025','33938676','34207959','34257765','34271367','34325174','34507928','34893931'
                ,'34947848','34967626','35000712','35175463','35206881','35465992','35532394','35587910','35593923','35594327'
                ,'35595063','35608328','35612027','35612227','35662191','35790124','35790324','35795531','35796726','35797627'
                ,'35798159','35803449','35809424','35809784','35810940','35813474','35813568','35814726','35829076','35831846'
                ,'35833257','35834110','35859652','35860778','35863135','35867570','35867657','35893973','35910996','35912778'
                ,'35915841','35919641','35942584','35943414','35973570','36097825','36174453','36191786','36291791','36394283'
                ,'36441568','36455104','36461488','36524277','36570138','36570310','36570619','36576976','36650730','36703276'
                ,'36745849','36747787','36748309','36749229','36796696','36814022','36868706','36917818','36966732','37068281'
                ,'37080003','37088337','37138113','37159272','37162451','37179680','37199094','37292282','37295441','37363921'
                ,'37363966','37441715','37493606','37539368','37573558','37576788','37576899','37642169','37661856','37759314'
                ,'37787932','37798417','37904039','37920026','37958361','38045826','38241852','38280893','38375127','38375563'
                ,'38403435','38416686','38426158','38426421','38501939','38690211','38711689','38899074','39923720','40061246'
                ,'40134042','40175970'";

            string ACHSubscriptionList = @"'33514792','33516475','33583605','33803279','33805042','33966010','33971932','33972692','33989327','34143463'
                ,'34144680','34145388','34146632','34208725','34254058','34254182','34319950','34536747','34536810','34536977'
                ,'34537016','34547870','34547902','34548014','34548041','34548065','34548087','34548288','34548308','34548412'
                ,'34548502','34548541','34548833','34548858','34548982','34549019','34549066','34549098','34549145','34549266'
                ,'34549410','34549717','34549870','34550024','34550224','34550257','34893568','34893877','34911631','34912125'
                ,'34912164','34912192','34912207','34912310','34912333','34912654','34912690','34912730','34912867','34912912'
                ,'34912941','34912980','34913087','34913154','34914393','34914689','34914800','34914867','34914913','34914950'
                ,'34915195','34915231','34915275','34915434','34915478','34915520','34915564','34915716','34915749','34915783'
                ,'34915825','34915864','34915897','34915986','34916107','34916483','34916538','34916777','34916890','34916928'
                ,'34917041','34917086','34917119','34917142','34917193','34917463','34917508','34917577','34919667','34919708'
                ,'34919760','34919806','34919832','34919861','34919890','34920272','34920516','34921206','34921235','34921287'
                ,'34921633','34921661','34921909','34921929','34921957','34921992','34922575','34922593','34922621','34922649'
                ,'34922693','34931581','34931642','34931689','34931910','34931966','34932053','34932349','34932470','34932526'
                ,'34932585','34932649','34932755','34932873','34933243','34933286','34933656','34933720','34933772','34933927'
                ,'34933978','34934086','34934303','34934424','34934474','34934506','34934582','34934669','34934811','34934853'
                ,'34935273','34935386','34935440','34935487','34935665','34935707','34937316','34937355','34937398','34937660'
                ,'34937819','34937958','34938098','34938128','34938151','34938193','34938238','34938307','34938365','34938446'
                ,'34938539','34938598','34938699','34938740','34938777','34938809','34938956','34939047','34939084','34939182'
                ,'34939236','34939319','34939353','34939401','34939429','34939470','34939509','34939538','34956021','34962451'
                ,'35030701','35031874','35217403','35458438','35755643','35759130','35759678','35763592','35832987','35861392'
                ,'35880145','35894121','35941375','35954396','35954483','36022772','36051591','36099495','36232343','36233325'
                ,'36290487','36292793','36293184','36332337','36332833','36392731','36455289','36462761','36464055','36465087'
                ,'36465565','36519935','36577086','36648187','36662021','36662258','36692339','36693637','36698093','36717078'
                ,'36717440','36719295','36720260','36720402','36720503','36730041','36730982','36743074','36743161','36749774'
                ,'36760493','36773178','36774406','36832207','36899761','36900472','36903280','36916977','36933514','36965268'
                ,'36998494','36998553','37068036','37081051','37081659','37095790','37096187','37097030','37106620','37130788'
                ,'37158744','37159670','37167077','37176561','37176996','37197973','37275870','37279597','37300578','37316712'
                ,'37319949','37320210','37363750','37376286','37391120','37436793','37437176','37451042','37451304','37451537'
                ,'37477828','37709513','37744175','37762924','37860474','37880291','37882023','37882142','37920157','37920757'
                ,'37954877','38031351','38099004','38128665','38279745','38294480','38476936','38478250','38493639','38533759'
                ,'38535014','38712557','38784814','38792714','38834647','38838383','38842000','39006153','39009263','39010667'
                ,'39011578','39020330','39020941','39128475','39130179','39131906','39165036','39189368','39246605','39362857'
                ,'39416214','39665535','39666472','39669173','39680648','39831892','39832370','39870671','39872470','39888695'
                ,'39948635','39979035','39979385','40046363','40087960','40159925','40191135','40191878','40192308','40213931'
                ,'40217417','40217927";
            #endregion

            List<Customer> CustomerList = _Util.Facade.CustomerFacade.GetCustomerListBySubscriptionIdList(ACHSubscriptionList);

            //foreach(var item in CustomerList)
            //{
            //    #region Report
            //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\ARBACHReport.txt"), true))
            //    {
            //        file.WriteLine(string.Format("Proceding for: {0}, {1}", item.AuthorizeRefId, item.FirstName + " " + item.LastName ));
            //    }
            //    #endregion
            //    try
            //    {
            //        ARBGetSubscriptionResponse result = GetSubscription.Run(APILoginId, TransactionKey, item.AuthorizeRefId, true);
            //        ARBSubscription Model = new ARBSubscription()
            //        {
            //            Invoice = item.Id + "",
            //            CustomerId = item.Id + "",
            //            Description = result.subscription.order.description,
            //            EmailAddress = result.subscription.profile.email,
            //            FirstName = result.subscription.name,
            //        };
            //        var result2 = UpdateSubscription.Run(APILoginId, TransactionKey, item.AuthorizeRefId, Model, true);

            //        #region Report
            //        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\ARBACHReport.txt"), true))
            //        {
            //            file.WriteLine(string.Format("Done for: {0}, {1}", item.AuthorizeRefId, item.FirstName + " " + item.LastName));
            //        }
            //        #endregion
            //    }
            //    catch (Exception)
            //    {
            //        #region Report
            //        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\ARBACHReport.txt"), true))
            //        {
            //            file.WriteLine(string.Format("Exception for: {0}, {1}", item.AuthorizeRefId, item.FirstName + " " + item.LastName));
            //        }
            //        #endregion
            //    }

            //}


            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSubscriptionIdList()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string TransactionKey = "232t7YP63K25tnN7";
            string APILoginId = "7xZCH5KsE2e";
            ARBGetSubscriptionListResponse result = GetListOfSubscriptions.Run(APILoginId, TransactionKey, true);

            foreach (var item in result.subscriptionDetails)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CC.txt"), true))
                {
                    file.WriteLine("SId:{0}, PPID:{1},PID{2} \n", item.id, item.customerPaymentProfileId, item.customerProfileId);
                }
            }
            return Json(result.subscriptionDetails.Select(x => x.id).ToArray(), JsonRequestBehavior.AllowGet);

        }


        public JsonResult TransactionListTest(string Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string TransactionKey = _Util.Facade.GlobalSettingsFacade.GetAuthTransactionKeyByCompanyId(CurrentUser.CompanyId.Value, false);
            string APILoginId = _Util.Facade.GlobalSettingsFacade.GetAuthAPILoginIdByCompanyId(CurrentUser.CompanyId.Value, false);
            //var response =  GetCustomerPaymentProfileList.Run(APILoginId, TransactionKey);
            //ARBGetSubscriptionResponse response = (ARBGetSubscriptionResponse)GetSubscription.Run(APILoginId, TransactionKey, Id); //ok
            //return Json(GetCustomerProfileTransactionList.Run(APILoginId,TransactionKey,Id),JsonRequestBehavior.AllowGet);
            //return Json(GetSubscription.Run(APILoginId, TransactionKey, Id), JsonRequestBehavior.AllowGet);

            //var response = GetCustomerProfileTransactionList.Run(APILoginId, TransactionKey, Id);
            //var response = GetSubscription.Run(APILoginId, TransactionKey, Id);
            //var response = GetSettledBatchList.Run(APILoginId,TransactionKey);
            bool AuthInProduction = false;
            GlobalSetting globset = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "Authorize.NetInProduction");
            if (globset != null && globset.Value.ToLower() == "true")
            {
                AuthInProduction = true;
            }
            //bool ForteInProduction = false;
            //GlobalSetting ForteGlobset = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ForteInProduction");
            //if (globset != null && globset.Value.ToLower() == "true")
            //{
            //    ForteInProduction = true;
            //}
            var response = GetTransactionList.Run(APILoginId, TransactionKey, Id, AuthInProduction);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SubscribtionList()
        {
            //return Json(false);
            //bool ForAch = true;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string TransactionKey = "8Nm8Jg7eu7P2T8V4";
            string APILoginId = "7k29Xb2ueQ7";
            //var response =  GetCustomerPaymentProfileList.Run(APILoginId, TransactionKey);
            //ARBGetSubscriptionResponse response = (ARBGetSubscriptionResponse)GetSubscription.Run(APILoginId, TransactionKey, Id); //ok
            //return Json(GetCustomerProfileTransactionList.Run(APILoginId,TransactionKey,Id),JsonRequestBehavior.AllowGet);
            //return Json(GetSubscription.Run(APILoginId, TransactionKey, Id), JsonRequestBehavior.AllowGet);

            //var response = GetCustomerProfileTransactionList.Run(APILoginId, TransactionKey, Id);
            //var response = GetSubscription.Run(APILoginId, TransactionKey, Id);
            //var response = GetSettledBatchList.Run(APILoginId,TransactionKey);
            bool AuthInProduction = true;
            var response = GetListOfSubscriptions.Run(APILoginId, TransactionKey, AuthInProduction);

            string type = "CC";
            type = "ACH";
            //type = "CC";

            string sql = "";
            foreach (var item in response.subscriptionDetails)
            {
                sql += string.Format(@"INSERT INTO [AuthData] ([AuthRefId],[FirstName],[Lastname],[Name],[Amount],[CustomerProfileId],[PaymentprofileId],[CustomerNo],[Status],[Type]) 
                                         VALUES ('{0}',N'{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}',N'{8}','{9}');{10}"
                                        , item.id
                                        , item.firstName
                                        , item.lastName
                                        , item.name.Replace("'", "''")
                                        , item.amount
                                        , item.customerProfileId
                                        , item.customerPaymentProfileId
                                        , item.invoice
                                        , item.status
                                        , type
                                        , Environment.NewLine);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private enum DATA247_STATUS
        {
            OK,
            INVALID_CREDENTIALS,
            INVALID_PHONE_NUMBER,
            NOT_WIRELESS_NUMBER,
            UNKNOWN_ERROR
        };

        // Get the SMS and MMS addresses for a phone number from Data 24-7
        // If returned status is OK, addresses are in m_SMSAddress and m_MMSAddress_
        public JsonResult GetData247Info(string Number)
        {
            string m_CarrierName = "";
            string m_SMSAddress = "";
            string m_MMSAddress = "";
            string m_PhoneNumber = "4014974470";
            string m_D247UserName = "inan.piistech";
            string m_D247UserPassword = "Inaninan0";
            string m_UnknownError = "";
            var response = "";

            XmlNode xn = null;

            try
            {
                // Create a base url to send the phone number(s) to Data24-7 to get the sms & mms addresses
                string url = @"http://api.data24-7.com/v/2.0?user=" + m_D247UserName + "&pass=" + m_D247UserPassword + "&api=T";

                // Add this number to the url
                url += "&p1=" + m_PhoneNumber;

                // Get the response from Data24-7
                using (var wb = new WebClient())
                {
                    response = wb.DownloadString(url);

                    // Check the response status
                    switch (response)
                    {
                        case "ERROR: D247_INVALID_CREDENTIALS":
                            // Not a Data24-7 account
                            return Json(DATA247_STATUS.INVALID_CREDENTIALS);
                        default:
                            // No error
                            break;
                    }
                }

                // Parse the Data24-7 response and get the SMS and MMS addresses
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(response);
                xn = xml.SelectSingleNode("/response/results/result");

                // Verify the status for this contact is ok
                string status = xn["status"].InnerText;
                if (status.ToUpper() != "OK")
                {
                    // Status is not ok
                    switch (status)
                    {
                        case "D247_INVALID_PHONE":
                            return Json(DATA247_STATUS.INVALID_PHONE_NUMBER);
                        default:
                            m_UnknownError = status;
                            return Json(DATA247_STATUS.UNKNOWN_ERROR);
                    }
                }
                else
                {
                    // Verify this is a wireless number
                    string wireless = xn["wless"].InnerText;
                    if (wireless.ToUpper() != "Y")
                    {
                        // Not a wireless number
                        return Json(DATA247_STATUS.NOT_WIRELESS_NUMBER);
                    }

                    // Get the carrier name
                    m_CarrierName = xn["carrier_name"].InnerText;

                    // Get the sms and mms addresses
                    m_SMSAddress = xn["sms_address"].InnerText;
                    m_MMSAddress = xn["mms_address"].InnerText;
                }
                return Json(DATA247_STATUS.OK);
            }
            catch (Exception ex)
            {
                m_UnknownError = ex.Message;
                return Json(DATA247_STATUS.UNKNOWN_ERROR);
            }
        }


        public JsonResult UnsetteledList()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            GlobalSetting globset = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("TransactionPullDayCountACH", CurrentUser.CompanyId.Value);
            int GetTransactionsForDaysACH = 7;
            if (globset != null)
            {
                int.TryParse(globset.Value, out GetTransactionsForDaysACH);
                globset = null;
            }
            int GetTransactionsForDaysCC = 7;
            globset = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("TransactionPullDayCountCC", CurrentUser.CompanyId.Value);
            if (globset != null)
            {
                int.TryParse(globset.Value, out GetTransactionsForDaysCC);
                globset = null;
            }
            bool AuthInProduction = false;
            GlobalSetting globset2 = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("Authorize.NetInProduction", CurrentUser.CompanyId.Value);
            if (globset2 != null && globset2.Value.ToLower() == "true")
            {
                AuthInProduction = true;
            }
            //bool ForteInProduction = false;
            //GlobalSetting ForteGlobset = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "ForteInProduction");
            //if (globset != null && globset.Value.ToLower() == "true")
            //{
            //    ForteInProduction = true;
            //}
            string TransactionKey = _Util.Facade.GlobalSettingsFacade.GetAuthTransactionKeyByCompanyId(CurrentUser.CompanyId.Value, true);
            string APILoginId = _Util.Facade.GlobalSettingsFacade.GetAuthAPILoginIdByCompanyId(CurrentUser.CompanyId.Value, true);
            //GetTransactionsForDaysCC,
            getUnsettledTransactionListResponse BatchListResponse = GetUnsettledTransactionList.Run(APILoginId, TransactionKey, AuthInProduction);
            return Json(false);
        }

        #region Alif Security Data

        public JsonResult AlifData()
        {

            List<AAAAlifSecuirtyCCInfo2> data = _Util.Facade.PaymentInfoFacade.GetAllAAAAlifSecuirtyCCInfo2s();

            foreach (var item in data)
            {
                item.CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(item.CardNumber);
                item.SecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(item.SecurityCode);

                _Util.Facade.PaymentInfoFacade.UpdateAAAAlifSecuirtyCCInfo2(item);
            }


            return Json(false);
        }


        #endregion

    }
    //public class UCCTicket: InterfaceService
    //{
    //   public ContactInsertQueryResult ContactInsert()
    //    {

    //        return null;

    //    }
    //}
}
public class PaymentMethodCard
{
    string cardArt { set; get; }
    string cardNumber { set; get; }
    string cardType { set; get; }
    string expirationDate { set; get; }
    string issuerNumber { set; get; }
}

