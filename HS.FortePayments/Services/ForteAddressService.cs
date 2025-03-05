using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Forte;
using System.Resources;
using Newtonsoft.Json;

namespace Forte
{
    /// <summary>
    /// Provides operations for creating, finding, updating, and deleting Addresses in the vault, Customer_token is mandatory.
    /// </summary>
    public class ForteAddressService
    {
        private string _strUser = "";
        private string _strPassword = "";
        private string _strAuthAccountID = "";
        private string _strAuthOrgID = "";
        private string _serverName = "";
        public string _url = "";

        public ForteAddressService(ForteOptions createOptions)
        {
            createOptions.Resource = "addresses";
            _serverName = GetServerDetails.Geturl(createOptions);
            _strUser = createOptions.UserId;
            _strPassword = createOptions.Password;
            _strAuthAccountID = createOptions.AuthAccountId;
            _strAuthOrgID = createOptions.Organization_ID;
            var urlparam = ParameterBuilder.ApplyAllParameters(createOptions);
            _url = _serverName + urlparam;
        }

        public IEnumerable<ForteAddress> ListAddress()
        {
            ForteException errGetAddr = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "ListAddress"
            };

            string response = Requestor.Get(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errGetAddr.ForteError.ErrorType = "#ERROR#";
                errGetAddr.ForteError.Message = response;
                throw errGetAddr;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errGetAddr.ForteError.ErrorType = "NotFound";
                errGetAddr.ForteError.Message = response;
                throw errGetAddr;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errGetAddr.ForteError.ErrorType = "BadRequest";
                errGetAddr.ForteError.Message = response;
                throw errGetAddr;
            }
            else
            {
                ForteListResult<ForteAddress> forteResults = JsonConvert.DeserializeObject<ForteListResult<ForteAddress>>(response);
                return forteResults.results;
            }

        }

        public object Create(ForteAddress forteAddressFull)
        {
            ForteException errGetAddr = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "CreateAddress"
            };
            var response = Requestor.PostString(_url, forteAddressFull, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errGetAddr.ForteError.ErrorType = "#ERROR#";
                errGetAddr.ForteError.Message = response;
                throw errGetAddr;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errGetAddr.ForteError.ErrorType = "NotFound";
                errGetAddr.ForteError.Message = response;
                throw errGetAddr;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errGetAddr.ForteError.ErrorType = "BadRequest";
                errGetAddr.ForteError.Message = response;
                throw errGetAddr;
            }
            else
            {
                return response;
            }

        }

        public ForteAddress GetAddress()
        {
            ForteAddress forteaddr = new ForteAddress();

            ForteException errGetAddr = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "GetAddress"
            };

            string strAddrResult = Requestor.Get(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);


            if (!(strAddrResult.IndexOf("#ERROR#") == -1))
            {
                errGetAddr.ForteError.ErrorType = "#ERROR#";
                errGetAddr.ForteError.Message = strAddrResult;
                throw errGetAddr;
            }
            else if (!(strAddrResult.IndexOf("NotFound") == -1))
            {
                errGetAddr.ForteError.ErrorType = "NotFound";
                errGetAddr.ForteError.Message = strAddrResult;
                throw errGetAddr;
            }
            else if (!(strAddrResult.IndexOf("BadRequest") == -1))
            {
                errGetAddr.ForteError.ErrorType = "BadRequest";
                errGetAddr.ForteError.Message = strAddrResult;
                throw errGetAddr;
            }
            else
            {
                forteaddr = Mapper<ForteAddress>.MapFromJson(strAddrResult);
                return forteaddr;
            }

        }

        public virtual string Delete()
        {
            ForteException errGetAddr = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "DeleteAddress"
            };
            var response = Requestor.Delete(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errGetAddr.ForteError.ErrorType = "#ERROR#";
                errGetAddr.ForteError.Message = response;
                throw errGetAddr;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errGetAddr.ForteError.ErrorType = "NotFound";
                errGetAddr.ForteError.Message = response;
                throw errGetAddr;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errGetAddr.ForteError.ErrorType = "BadRequest";
                errGetAddr.ForteError.Message = response;
                throw errGetAddr;
            }
            else
            {
                return response;
            }

        }

        public string Update(ForteAddress forteAddr)
        {
            ForteException errGetAddr = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "UpdateAddress"
            };

            var response = Requestor.PutString(_url, forteAddr, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errGetAddr.ForteError.ErrorType = "#ERROR#";
                errGetAddr.ForteError.Message = response;
                throw errGetAddr;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errGetAddr.ForteError.ErrorType = "NotFound";
                errGetAddr.ForteError.Message = response;
                throw errGetAddr;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errGetAddr.ForteError.ErrorType = "BadRequest";
                errGetAddr.ForteError.Message = response;
                throw errGetAddr;
            }
            else
            {
                return response;
            }

        }
    }
}
