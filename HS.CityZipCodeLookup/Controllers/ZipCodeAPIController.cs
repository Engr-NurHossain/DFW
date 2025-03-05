using HS.Entities;
using HS.Facade;
using HS.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HS.CityZipCodeLookup.Controllers
{
    [RoutePrefix("1.0")]
    public class ZipCodeAPIController : ApiController
    {
        private readonly ZipCodeAPIFacade ZipCodeAPIFacade;
        public ZipCodeAPIController()
        {
            ZipCodeAPIFacade = new ZipCodeAPIFacade();
        }

        [Route("GetCityZipCodeLookupList")]
        public HttpResponseMessage GetCityZipCodeLookupList()
        {
            string key = "";
            string appname = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("key"))
            {
                key = headers.GetValues("key").First();
            }
            if (headers.Contains("appname"))
            {
                appname = headers.GetValues("appname").First();
            }
            try
            {
                if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(appname))
                {
                    List<CityZipCode> LCityState = ZipCodeAPIFacade.GetLeadCityStateListBySearchKey(key, 20);
                    CityZipCodeSearchLog objlog = new CityZipCodeSearchLog()
                    {
                        AppName = appname,
                        UserIP = AppConfig.GetIP,
                        SearchText = key,
                        SearchDate = DateTime.Now
                    };
                    ZipCodeAPIFacade.InsertSearchLog(objlog);
                    return Request.CreateResponse(HttpStatusCode.OK, LCityState);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.PreconditionFailed, "key should not be null");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Route("InsertBadgerMapData")]
        public bool InsertBadgerMapData()
        {
            string Type = "";
            string Comment = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("Type"))
            {
                Type = headers.GetValues("Type").First();
            }
            if (headers.Contains("Comment"))
            {
                Comment = headers.GetValues("Comment").First();
            }
            Lookup lookup = new Lookup();
            lookup.DisplayText = Type;
            lookup.DataValue = Comment;
            lookup.DataKey = "";
            lookup.CompanyId = new Guid();
            bool CustomerAptInsert = ZipCodeAPIFacade.InsertLookUp(lookup);
            return true;
        }
    }
}
