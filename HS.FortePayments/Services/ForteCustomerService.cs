using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forte;
using System.Resources;
using Newtonsoft.Json;
using Forte.Entities;

namespace Forte
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting Customers in the vault
    /// </summary>
    public class ForteCustomerService
    {
        private string _strUser = "";
        private string _strPassword = "";
        private string _strAuthAccountID = "";
        private string _strAuthOrgID = "";
        private string _serverName = "";
        public string _url = "";

        public ForteCustomerService(ForteOptions createOptions)
        {
            createOptions.Resource = "customers";
            _serverName = GetServerDetails.Geturl(createOptions);
            _strUser = createOptions.UserId;
            _strPassword = createOptions.Password;
            _strAuthAccountID = createOptions.AuthAccountId;
            _strAuthOrgID = createOptions.Organization_ID;
            var urlparam = ParameterBuilder.ApplyAllParameters(createOptions);
            _url = _serverName + urlparam;
        }

        public virtual ForteCustomerCreate Create(ForteCustomer customer)
        {
            ForteCustomerCreate forteresponse = new ForteCustomerCreate();
            ForteException errCust = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "CreateCustomer"
            };
            var response = Requestor.PostString(_url, customer, _strUser, _strPassword, _strAuthAccountID,_strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errCust.ForteError.ErrorType = "#ERROR#";
                errCust.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = "Invalid Field Value";
                return forteresponse;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errCust.ForteError.ErrorType = "NotFound";
                errCust.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = "Invalid Field Value";
                return forteresponse;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errCust.ForteError.ErrorType = "BadRequest";
                errCust.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = "Invalid Field Value";
                return forteresponse;
            }
            else
            {
                forteresponse.Result = false;
                forteresponse.Massege = response;
                return forteresponse;
            }
        }
        public virtual ForteCustomer GetCustomer()
        {
            ForteCustomer custResult = new ForteCustomer();
            ForteException errCust = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "GetCustomer"
            };
            string strcustResult = Requestor.Get(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);


            if (!(strcustResult.IndexOf("#ERROR#") == -1))
            {
                errCust.ForteError.ErrorType = "#ERROR#";
                errCust.ForteError.Message = strcustResult;
                throw errCust;
            }
            else if (!(strcustResult.IndexOf("NotFound") == -1))
            {
                errCust.ForteError.ErrorType = "NotFound";
                errCust.ForteError.Message = strcustResult;
                throw errCust;
            }
            else if (!(strcustResult.IndexOf("BadRequest") == -1))
            {
                errCust.ForteError.ErrorType = "BadRequest";
                errCust.ForteError.Message = strcustResult;
                throw errCust;
            }
            else
            {
                custResult = Mapper<ForteCustomer>.MapFromJson(strcustResult);
                return custResult;
            }


        }
        public virtual ForteCustomerCreate Update(UpdateCustomerWithPaymentToken customer)
        {
            ForteCustomerCreate forteresponse = new ForteCustomerCreate();
            ForteException errCust = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "UpdateCustomer"
            };
            _url = _url + "/paymethods";
            var response = Requestor.PutString(_url, customer, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errCust.ForteError.ErrorType = "#ERROR#";
                errCust.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = "Invalid Field Value";
                return forteresponse;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errCust.ForteError.ErrorType = "NotFound";
                errCust.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = "Invalid Field Value";
                return forteresponse;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errCust.ForteError.ErrorType = "BadRequest";
                errCust.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = "Invalid Field Value";
                return forteresponse;
            }
            else
            {
                forteresponse.Result = false;
                forteresponse.Massege = response;
                return forteresponse;
            }
        }

        public virtual string Delete()
        {
            ForteException errCust = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "DeleteCustomer"
            };
            var response = Requestor.Delete(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errCust.ForteError.ErrorType = "#ERROR#";
                errCust.ForteError.Message = response;
                throw errCust;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errCust.ForteError.ErrorType = "NotFound";
                errCust.ForteError.Message = response;
                throw errCust;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errCust.ForteError.ErrorType = "BadRequest";
                errCust.ForteError.Message = response;
                throw errCust;
            }
            else
            {
                return response;
            }
        }

        public virtual IEnumerable<ForteCustomer> ListCustomer()
        {
            ForteException errCust = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "ListCustomer"
            };

            string response = Requestor.Get(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errCust.ForteError.ErrorType = "#ERROR#";
                errCust.ForteError.Message = response;
                throw errCust;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errCust.ForteError.ErrorType = "NotFound";
                errCust.ForteError.Message = response;
                throw errCust;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errCust.ForteError.ErrorType = "BadRequest";
                errCust.ForteError.Message = response;
                throw errCust;
            }
            else
            {
                ForteListResult<ForteCustomer> forteResults = JsonConvert.DeserializeObject<ForteListResult<ForteCustomer>>(response);
                return forteResults.results;
            }
        }
    }
}
