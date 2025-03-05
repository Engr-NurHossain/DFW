using System.Net.Http;
using System.Threading.Tasks;
using HS.Kickbox.Helpers;
using HS.Kickbox.Models;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System;

namespace HS.Kickbox
{
    public static class KickBoxApi
    {
        //[Shariful-23-9-19]
        //private static bool IsValidEmailAddress(this string email)
        //{
        //    try
        //    {
        //        var addr = new System.Net.Mail.MailAddress(email);
        //        return addr.Address == email;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        //[~Shariful-23-9-19]
        public static bool Verify(string emailAddress, string apiKey, int? timeout = null)
        {
            KickBoxResponse responseObject = VerifyWithResponse(emailAddress, apiKey, timeout);

            return responseObject.Result == Result.Deliverable;
        }

        public static ExtendedKickBoxResponse VerifyWithResponse(string emailAddress, string apiKey, int? timeout = null)
        {
            //[Shariful-23-9-19]
            //if (string.IsNullOrWhiteSpace(emailAddress) || !emailAddress.IsValidEmailAddress())
            //{
            //    return new ExtendedKickBoxResponse()
            //    {
            //        Result = Result.Unknown,
            //        Message = "Email address not found.",
            //        Reason = "Email address not found.",
            //        Success = false
            //    };
            //}
            //[~Shariful-23-9-19]
            string apiUrl = "https://api.kickbox.com/v2/verify";
            //string apiKey = "test_7f5bee7be1545be05a1b80099b4a971544c9cb5b4009b01835ee9b4decaa512b";

            var requestObject = new HS.Kickbox.Models.KickBoxRequest
            {
                ApiKey = apiKey,
                Email = emailAddress,
                Timeout = 300
            };
            string QueryString = requestObject.ToQueryString();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create($"{apiUrl}?{QueryString}");

            try
            {
                var response = (HttpWebResponse)myReq.GetResponse();
                string responseString;

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    responseString = sr.ReadToEnd();
                }

                ExtendedKickBoxResponse responseObject = JsonConvert.DeserializeObject<ExtendedKickBoxResponse>(responseString);

                return responseObject;
            }
            catch (Exception ex)
            {
                return new ExtendedKickBoxResponse()
                {
                    Result = Result.Unknown,
                    Message = "Insufficient balance",
                    Reason ="Server error",
                    Success = false
                };
            }
           
        }
    }
}
