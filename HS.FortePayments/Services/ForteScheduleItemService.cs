using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Forte;
using System.Resources;
using Newtonsoft.Json;

namespace Forte
{
    public class ForteScheduleItemService
    {
        private string _strUser = "";
        private string _strPassword = "";
        private string _strAuthAccountID = "";
        private string _strAuthOrgID = "";
        private string _serverName = "";
        public string _url = "";

        public ForteScheduleItemService(ForteOptions createOptions)
        {
            createOptions.Resource = "scheduleitems";
            _serverName = GetServerDetails.Geturl(createOptions);
            _strUser = createOptions.UserId;
            _strPassword = createOptions.Password;
            _strAuthAccountID = createOptions.AuthAccountId;
            _strAuthOrgID = createOptions.Organization_ID;
            var urlparam = ParameterBuilder.ApplyAllParameters(createOptions);
            _url = _serverName + urlparam;
        }

        public IEnumerable<ForteScheduleItem> List()
        {
            ForteException errList = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "List"
            };

            string response = Requestor.Get(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errList.ForteError.ErrorType = "#ERROR#";
                errList.ForteError.Message = response;
                throw errList;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errList.ForteError.ErrorType = "NotFound";
                errList.ForteError.Message = response;
                throw errList;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errList.ForteError.ErrorType = "BadRequest";
                errList.ForteError.Message = response;
                throw errList;
            }
            else
            {
                ForteListResult<ForteScheduleItem> forteResults = JsonConvert.DeserializeObject<ForteListResult<ForteScheduleItem>>(response);
                return forteResults.results;
            }
        }

        public object Create(ForteScheduleItem forteScheduleItem)
        {
            ForteException errCreate = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "Create"
            };

            var response = Requestor.PostString(_url, forteScheduleItem, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errCreate.ForteError.ErrorType = "#ERROR#";
                errCreate.ForteError.Message = response;
                throw errCreate;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errCreate.ForteError.ErrorType = "NotFound";
                errCreate.ForteError.Message = response;
                throw errCreate;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errCreate.ForteError.ErrorType = "BadRequest";
                errCreate.ForteError.Message = response;
                throw errCreate;
            }
            else
            {
                return response;
            }
        }

        public ForteScheduleItem Get()
        {
            ForteScheduleItem forteScheduleItem = new ForteScheduleItem();

            ForteException errGetAddr = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "Get"
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
                forteScheduleItem = JsonConvert.DeserializeObject<ForteScheduleItem>(response);
                return forteScheduleItem;
            }
        }

        public virtual string Delete()
        {
            ForteException errDelete = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "Delete"
            };

            var response = Requestor.Delete(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errDelete.ForteError.ErrorType = "#ERROR#";
                errDelete.ForteError.Message = response;
                throw errDelete;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errDelete.ForteError.ErrorType = "NotFound";
                errDelete.ForteError.Message = response;
                throw errDelete;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errDelete.ForteError.ErrorType = "BadRequest";
                errDelete.ForteError.Message = response;
                throw errDelete;
            }
            else
            {
                return response;
            }
        }

        public string Update(ForteScheduleItem forteScheduleItem)
        {
            ForteException errUpdate = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "Update"
            };

            var response = Requestor.PutString(_url, forteScheduleItem, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errUpdate.ForteError.ErrorType = "#ERROR#";
                errUpdate.ForteError.Message = response;
                throw errUpdate;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errUpdate.ForteError.ErrorType = "NotFound";
                errUpdate.ForteError.Message = response;
                throw errUpdate;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errUpdate.ForteError.ErrorType = "BadRequest";
                errUpdate.ForteError.Message = response;
                throw errUpdate;
            }
            else
            {
                return response;
            }
        }
    }
}
