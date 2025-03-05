using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Forte;
using System.Resources;
using Newtonsoft.Json;
using Forte.Entities;

namespace Forte
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting Transactions in the vault
    /// </summary>
    public class ForteTransactionService
    {
        private string _strUser = "";
        private string _strPassword = "";
        private string _strAuthAccountID = "";
        private string _serverName = "";
        private string _strAuthOrgID = "";
        public string _url = "";

        public ForteTransactionService(ForteOptions createOptions)
        {
            createOptions.Resource = "transactions";
            _serverName = GetServerDetails.Geturl(createOptions);
            _strUser = createOptions.UserId;
            _strPassword = createOptions.Password;
            _strAuthAccountID = createOptions.Organization_ID;
            _strAuthOrgID = createOptions.Organization_ID;
            var urlparam = ParameterBuilder.ApplyAllParameters(createOptions);
            _url = _serverName + urlparam;
        }

        public virtual IEnumerable<ForteTransaction> ListTransaction()
        {
            ForteException errTran = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "ListTransaction"
            };

            string response = Requestor.Get(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errTran.ForteError.ErrorType = "#ERROR#";
                errTran.ForteError.Message = response;
                throw errTran;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errTran.ForteError.ErrorType = "NotFound";
                errTran.ForteError.Message = response;
                throw errTran;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errTran.ForteError.ErrorType = "BadRequest";
                errTran.ForteError.Message = response;
                throw errTran;
            }
            else
            {
                ForteListResult<ForteTransaction> forteResults = JsonConvert.DeserializeObject<ForteListResult<ForteTransaction>>(response);
                return forteResults.results;
            }
        }

        public FortePaymentCreate Create(ForteTransaction trans)
        {
                FortePaymentCreate forteresponse = new FortePaymentCreate();
                ForteException errTran = new ForteException()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Source = "CreateTransaction"
                };
                var response = Requestor.PostString(_url, trans, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

                if (!(response.IndexOf("#ERROR#") == -1))
                {
                    errTran.ForteError.ErrorType = "#ERROR#";
                    errTran.ForteError.Message = response;
                    forteresponse.Result = false;
                    forteresponse.Massege = response;
                     return forteresponse;
                }
                else if (!(response.IndexOf("NotFound") == -1))
                {
                    errTran.ForteError.ErrorType = "NotFound";
                    errTran.ForteError.Message = response;
                    forteresponse.Result = false;
                    forteresponse.Massege = response;
                    return forteresponse;
                }
                else if (!(response.IndexOf("BadRequest") == -1))
                {
                    errTran.ForteError.ErrorType = "BadRequest";
                    errTran.ForteError.Message = response;
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

        public FortePaymentGetwayResponse GetTransaction()
        {
            FortePaymentGetwayResponse forteresponse = new FortePaymentGetwayResponse(); 
            ForteException errTran = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "GetTransaction"
            };
            string strtranResult = Requestor.Get(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(strtranResult.IndexOf("#ERROR#") == -1))
            {
                errTran.ForteError.ErrorType = "#ERROR#";
                errTran.ForteError.Message = strtranResult;
                forteresponse.Result = false;
                forteresponse.Massege = strtranResult;
                return forteresponse;
            }
            else if (!(strtranResult.IndexOf("NotFound") == -1))
            {
                errTran.ForteError.ErrorType = "NotFound";
                errTran.ForteError.Message = strtranResult;
                forteresponse.Result = false;
                forteresponse.Massege = strtranResult;
                return forteresponse;
            }
            else if (!(strtranResult.IndexOf("BadRequest") == -1))
            {
                errTran.ForteError.ErrorType = "BadRequest";
                errTran.ForteError.Message = strtranResult;
                forteresponse.Result = false;
                forteresponse.Massege = strtranResult;
                return forteresponse;
            }
            else
            {
                forteresponse.Result = false;
                forteresponse.Massege = strtranResult;
                return forteresponse;

            }

        }

        public object Delete()
        {
            ForteException errTran = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "DeleteTransaction"
            };
            var response = Requestor.Delete(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errTran.ForteError.ErrorType = "#ERROR#";
                errTran.ForteError.Message = response;
                throw errTran;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errTran.ForteError.ErrorType = "NotFound";
                errTran.ForteError.Message = response;
                throw errTran;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errTran.ForteError.ErrorType = "BadRequest";
                errTran.ForteError.Message = response;
                throw errTran;
            }
            else
            {
                return response;
            }
        }

        public object Update(ForteTransaction forteTran)
        {
            ForteException errTran = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "UpdateTransaction"
            };
            var response = Requestor.PutString(_url, forteTran, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errTran.ForteError.ErrorType = "#ERROR#";
                errTran.ForteError.Message = response;
                throw errTran;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errTran.ForteError.ErrorType = "NotFound";
                errTran.ForteError.Message = response;
                throw errTran;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errTran.ForteError.ErrorType = "BadRequest";
                errTran.ForteError.Message = response;
                throw errTran;
            }
            else
            {
                return response;
            }
        }
    }
}
