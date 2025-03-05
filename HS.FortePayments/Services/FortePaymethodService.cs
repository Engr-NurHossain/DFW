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
    /// Provides operations for creating, finding, updating, and deleting Paymethods in the vault
    /// </summary>
    public class FortePaymethodsService
    {
        private string _strUser = "";
        private string _strPassword = "";
        private string _strAuthAccountID = "";
        private string _strAuthOrgID = "";
        private string _serverName = "";
        public string _url = "";

        public FortePaymethodsService(ForteOptions createOptions)
        {
            createOptions.Resource = "paymethods";
            _serverName = GetServerDetails.Geturl(createOptions);
            _strUser = createOptions.UserId;
            _strPassword = createOptions.Password;
            _strAuthAccountID = createOptions.AuthAccountId;
            _strAuthOrgID = createOptions.Organization_ID;
            var urlparam = ParameterBuilder.ApplyAllParameters(createOptions);
            _url = _serverName + urlparam;
        }

        public virtual FortePaymentCreate Create(FortePaymethod paymethod)
        {
            FortePaymentCreate forteresponse = new FortePaymentCreate();
            ForteException errPay = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "CreatePaymethod"
            };
            var response = Requestor.PostString(_url, paymethod, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);
            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errPay.ForteError.ErrorType = "#ERROR#";
                errPay.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = response;
                return forteresponse;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errPay.ForteError.ErrorType = "NotFound";
                errPay.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = response;
                return forteresponse;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errPay.ForteError.ErrorType = "BadRequest";
                errPay.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = response;
                return forteresponse;
            }
            else
            {
                forteresponse.Result = false;
                forteresponse.Massege = response;
                return forteresponse;
            }
        }
        public virtual FortePaymethod GetPaymethod()
        {
            FortePaymethod payResult = new FortePaymethod();
            ForteException errPay = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "GetPaymethod"
            };
            string strpayResult = Requestor.Get(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(strpayResult.IndexOf("#ERROR#") == -1))
            {
                errPay.ForteError.ErrorType = "#ERROR#";
                errPay.ForteError.Message = strpayResult;
                throw errPay;
            }
            else if (!(strpayResult.IndexOf("NotFound") == -1))
            {
                errPay.ForteError.ErrorType = "NotFound";
                errPay.ForteError.Message = strpayResult;
                throw errPay;
            }
            else if (!(strpayResult.IndexOf("BadRequest") == -1))
            {
                errPay.ForteError.ErrorType = "BadRequest";
                errPay.ForteError.Message = strpayResult;
                throw errPay;
            }
            else
            {
                payResult = Mapper<FortePaymethod>.MapFromJson(strpayResult);
                return payResult;
            }

        }

        public virtual FortePaymentCreate Update<T>(T paymethod)
        {
            FortePaymentCreate forteresponse = new FortePaymentCreate();
            ForteException errPay = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "UpdatePaymethod"
            };
            var response = Requestor.PutString(_url, paymethod, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);
            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errPay.ForteError.ErrorType = "#ERROR#";
                errPay.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = "Invalid Field Value";
                return forteresponse;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errPay.ForteError.ErrorType = "NotFound";
                errPay.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = "Invalid Field Value";
                return forteresponse;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errPay.ForteError.ErrorType = "BadRequest";
                errPay.ForteError.Message = response;
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
            ForteException errPay = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "DeletePaymethod"
            };
            var response = Requestor.Delete(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);
            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errPay.ForteError.ErrorType = "#ERROR#";
                errPay.ForteError.Message = response;
                throw errPay;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errPay.ForteError.ErrorType = "NotFound";
                errPay.ForteError.Message = response;
                throw errPay;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errPay.ForteError.ErrorType = "BadRequest";
                errPay.ForteError.Message = response;
                throw errPay;
            }
            else
            {
                return response;
            }
        }

        public virtual IEnumerable<FortePaymethod> ListPaymethod()
        {
            ForteException errPay = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "ListPaymethod"
            };

            string response = Requestor.Get(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errPay.ForteError.ErrorType = "#ERROR#";
                errPay.ForteError.Message = response;
                throw errPay;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errPay.ForteError.ErrorType = "NotFound";
                errPay.ForteError.Message = response;
                throw errPay;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errPay.ForteError.ErrorType = "BadRequest";
                errPay.ForteError.Message = response;
                throw errPay;
            }
            else
            {
                ForteListResult<FortePaymethod> forteResults = JsonConvert.DeserializeObject<ForteListResult<FortePaymethod>>(response);
                return forteResults.results;
            }

        }

    }
}
