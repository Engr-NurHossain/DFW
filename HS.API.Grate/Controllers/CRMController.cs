using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HS.Entities;
using HS.Entities.API;
using HS.Facade;
using System.Security.Claims;
using HS.API.Grate.Models;

namespace HS.API.Grate.Controllers
{
    [Authorize]
    [RoutePrefix("1.0")]
    public class CRMController : ApiController
    { 
        public ApiFacade ApiFacade = new ApiFacade(); 
        public static string ConnStr = "";
        private void InitFacade()
        {
            if (!string.IsNullOrWhiteSpace(ConnStr))
            {
                ApiFacade = new ApiFacade(ConnStr);
            }
            else
            {
                var identity = (ClaimsIdentity)User.Identity;
                string connectionstring = identity.Claims.Where(c => c.Type == "connectionstring").Select(c => c.Value).SingleOrDefault();
                ApiFacade = new ApiFacade(connectionstring);
            }
        }

        #region Opportunities
        [Authorize]
        [Route("Opportunities")]
        [HttpPost]
        public HttpResponseMessage Opportunities()
        {
            BaseAPIResponse response = new BaseAPIResponse()
            {
                success = false,
                message = LabelHelper.ResponseStatus.NoDataFound
            };
            try
            {
                InitFacade();
                List<OpportunityAPI.OpportunityAPIModels> OPModel = ApiFacade.GetAllOpportunities();
                if(OPModel !=null && OPModel.Count > 0)
                {
                    response.data = OPModel;
                    response.success = true;
                    response.message = LabelHelper.ResponseStatus.DataFound;
                }
            }
            catch (Exception)
            {
                response.message = LabelHelper.ResponseStatus.Error;
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        #endregion

        #region Customer
        [Authorize]
        [Route("Accounts")]
        [HttpPost]
        public HttpResponseMessage Accounts()
        {
            BaseAPIResponse response = new BaseAPIResponse()
            {
                success = false,
                message = LabelHelper.ResponseStatus.NoDataFound
            };
            try
            {
                InitFacade();
                var re = Request;
                var headers = re.Headers;

                int id = 0;
                Guid guid = new Guid();

                if (headers.Contains("id"))
                {
                    int.TryParse(headers.GetValues("id").FirstOrDefault(), out id);
                }
                else if (headers.Contains("guid"))
                {
                    Guid.TryParse(headers.GetValues("guid").FirstOrDefault(), out guid);
                }

                List<CustomerAPI.CustomerAPIModels> CustomerModel = ApiFacade.GetAllCustomer(id, guid);
                if(CustomerModel != null  && CustomerModel.Count > 0)
                {
                    response.data = CustomerModel;
                    response.success = true;
                    response.message = LabelHelper.ResponseStatus.DataFound;
                }
            }
            catch (Exception)
            {
                response.message = LabelHelper.ResponseStatus.Error;
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        #endregion

        #region Ticket
        [Authorize]
        [Route("Appointments")]
        [HttpPost]
        public HttpResponseMessage Appointments()
        {
            BaseAPIResponse response = new BaseAPIResponse()
            {
                success = false,
                message = LabelHelper.ResponseStatus.NoDataFound
            };
            try
            {
                InitFacade();
                Guid CustomerGuid = new Guid();
                int CustomerId = 0;
                int Id = 0;
                var re = Request;
                var headers = re.Headers;

                if (headers.Contains("id"))
                {
                    int.TryParse(headers.GetValues("id").FirstOrDefault(), out Id);
                }
                else if (headers.Contains("customerId"))
                {
                    int.TryParse(headers.GetValues("customerId").FirstOrDefault(), out CustomerId);
                }
                else if (headers.Contains("customerGuid"))
                {
                    Guid.TryParse(headers.GetValues("customerGuid").FirstOrDefault(), out CustomerGuid);
                }

                List<TicketAPI.TicketAPIModels> TicketModel = ApiFacade.GetAllTickets(Id,CustomerId,CustomerGuid);

                if(TicketModel != null && TicketModel.Count > 0)
                {
                    response.data = TicketModel;
                    response.message = LabelHelper.ResponseStatus.DataFound;
                    response.success = true;
                }
            }
            catch (Exception)
            {
                response.message = LabelHelper.ResponseStatus.Error;
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        #endregion

        #region sample Call
        //[Authorize]
        //[Route("SampleCall")]
        //[HttpPost]
        //public HttpResponseMessage SampleCall([FromBody] string Model)
        //{
        //    var re = Request;
        //    var headers = re.Headers;
        //    string Test = string.Empty;
        //    if (headers.Contains("Test"))
        //    {
        //        Test = headers.GetValues("Test").FirstOrDefault();
        //    }
        //    List<Customer> CustomerList = new List<Customer>();

        //    CustomerList.Add(new Customer() {
        //        AccountNo = "asd",
        //        Id = 123,
        //        IsActive = true,
        //        FirstName ="Inan"
        //    }); 
        //    return Request.CreateResponse(HttpStatusCode.OK, CustomerList);
        //}
        #endregion


    }
}
