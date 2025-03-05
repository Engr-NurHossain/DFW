using HS.Entities;
using HS.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HS.API.Controllers
{
    [RoutePrefix("1.0")]
    public class RestaurantController : BaseAPIController
    {
        public HSApiFacade HSapiFacade = new HSApiFacade();
        public HSMainApiFacade HSMainApiFacade = new HSMainApiFacade();
        string constr = System.Configuration.ConfigurationManager.ConnectionStrings["IeateryCoonectionString"].ConnectionString;

        private void APIInitialize()
        {
            if (!string.IsNullOrWhiteSpace(constr))
            {
                HSapiFacade = new HSApiFacade(constr);
            }
        }

        [Route("GetAllOrders")]
        public HttpResponseMessage GetAllOrders()
        {
            APIInitialize();
            Guid companyid = new Guid();
            int pageno = 0;
            int pagesize = 0;
            string searchtext = "";
            string order = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("companyid"))
            {
                Guid.TryParse(headers.GetValues("companyid").First(), out companyid);
            }
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
            try
            {
                List<ResturantOrder> model = new List<ResturantOrder>();
                model = HSapiFacade.GetAllResturantOrderList(companyid, pageno, pagesize, searchtext, order);
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Route("GetOrderDetail")]
        public HttpResponseMessage GetOrderDetail()
        {
            APIInitialize();
            Guid companyid = new Guid();
            Guid orderid = new Guid();
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("companyid"))
            {
                Guid.TryParse(headers.GetValues("companyid").First(), out companyid);
            }
            if (headers.Contains("orderid"))
            {
                Guid.TryParse(headers.GetValues("orderid").First(), out orderid);
            }
            try
            {
                ResturantOrderCustomModel model = new ResturantOrderCustomModel();
                model.ListOrderDetail = HSapiFacade.GetAllResturantOrderDetailByOrderId(orderid);
                return Request.CreateResponse(HttpStatusCode.OK, model.ListOrderDetail);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }

        [Route("ChangeOrderStatus")]
        public HttpResponseMessage ChangeOrderStatus()
        {
            APIInitialize();
            Guid companyid = new Guid();
            Guid orderid = new Guid();
            string status = "";
            var re = Request;
            var headers = re.Headers;
            if (headers.Contains("companyid"))
            {
                Guid.TryParse(headers.GetValues("companyid").First(), out companyid);
            }
            if (headers.Contains("orderid"))
            {
                Guid.TryParse(headers.GetValues("orderid").First(), out orderid);
            }
            if (headers.Contains("status"))
            {
                status = headers.GetValues("status").First();
            }
            try
            {
                ResturantOrder model = new ResturantOrder();
                model = HSapiFacade.GetResturantOrderByCompanyIdAndOrderId(companyid, orderid);
                if(model != null)
                {
                    if (!string.IsNullOrWhiteSpace(status))
                    {
                        model.Status = status;
                        HSapiFacade.UpdateResturantOrder(model);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }
    }
}
