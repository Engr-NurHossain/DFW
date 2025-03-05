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
    public class ForteScheduleService
    {
        private string _strUser = "";
        private string _strPassword = "";
        private string _strAuthAccountID = "";
        private string _strAuthOrgID = "";
        private string _serverName = "";
        public string _url = "";

        public ForteScheduleService(ForteOptions createOptions)
        {
            createOptions.Resource = "schedules";
            _serverName = GetServerDetails.Geturl(createOptions);
            _strUser = createOptions.UserId;
            _strPassword = createOptions.Password;
            _strAuthAccountID = createOptions.AuthAccountId;
            _strAuthOrgID = createOptions.Organization_ID;
            var urlparam = ParameterBuilder.ApplyAllParameters(createOptions);
            _url = _serverName + urlparam;
        }

        public IEnumerable<ForteSchedule> List()
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
                ForteListResult<ForteSchedule> forteResults = JsonConvert.DeserializeObject<ForteListResult<ForteSchedule>>(response);
                return forteResults.results;
            }
        }

        public ForteResponseDetails Create(ForteSchedule forteSchedule)
        {
            ForteResponseDetails forteresponse = new ForteResponseDetails();
            ForteException errCreate = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "Create"
            };
            var response = Requestor.PostString(_url, forteSchedule, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errCreate.ForteError.ErrorType = "#ERROR#";
                errCreate.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = response;
                return forteresponse;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errCreate.ForteError.ErrorType = "NotFound";
                errCreate.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = response;
                return forteresponse;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errCreate.ForteError.ErrorType = "BadRequest";
                errCreate.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = response;
                return forteresponse;
            }
            else
            {
                forteresponse.Result = false;
                forteresponse.Massege = response; ;
                return forteresponse;
            }
        }

        public ForteSchedule Get()
        {
            ForteSchedule forteSchedule = new ForteSchedule();

            ForteException errGet = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "Get"
            };

            string response = Requestor.Get(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errGet.ForteError.ErrorType = "#ERROR#";
                errGet.ForteError.Message = response;
                throw errGet;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errGet.ForteError.ErrorType = "NotFound";
                errGet.ForteError.Message = response;
                throw errGet;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errGet.ForteError.ErrorType = "BadRequest";
                errGet.ForteError.Message = response;
                throw errGet;
            }
            else
            {
                forteSchedule = JsonConvert.DeserializeObject<ForteSchedule>(response);
                return forteSchedule;
            }
        }


        public FortePaymentGetwayResponse GetSchedule()
        {
            FortePaymentGetwayResponse forteSchedule = new FortePaymentGetwayResponse();

            ForteException errGet = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "Get"
            };

            string response = Requestor.Get(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errGet.ForteError.ErrorType = "#ERROR#";
                errGet.ForteError.Message = response;
                throw errGet;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errGet.ForteError.ErrorType = "NotFound";
                errGet.ForteError.Message = response;
                throw errGet;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errGet.ForteError.ErrorType = "BadRequest";
                errGet.ForteError.Message = response;
                throw errGet;
            }
            else
            {
                //forteSchedule = JsonConvert.DeserializeObject<ForteSchedule>(response);\
                forteSchedule.Massege = response;
                forteSchedule.Result = true;
                return forteSchedule;
            }
        }


        public virtual string Delete()
        {
            ForteException errGet = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "Delete"
            };

            var response = Requestor.Delete(_url, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errGet.ForteError.ErrorType = "#ERROR#";
                errGet.ForteError.Message = response;
                throw errGet;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errGet.ForteError.ErrorType = "NotFound";
                errGet.ForteError.Message = response;
                throw errGet;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errGet.ForteError.ErrorType = "BadRequest";
                errGet.ForteError.Message = response;
                throw errGet;
            }
            else
            {
                return response;
            }
        }

        public ForteResponseDetails Update(ForteSchedule forteSchedule)
        {
            ForteResponseDetails forteresponse = new ForteResponseDetails();
            ForteException errUpdate = new ForteException()
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK,
                Source = "Update"
            };

            var response = Requestor.PutString(_url, forteSchedule, _strUser, _strPassword, _strAuthAccountID, _strAuthOrgID);

            if (!(response.IndexOf("#ERROR#") == -1))
            {
                errUpdate.ForteError.ErrorType = "#ERROR#";
                errUpdate.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = response;
                return forteresponse;
            }
            else if (!(response.IndexOf("NotFound") == -1))
            {
                errUpdate.ForteError.ErrorType = "NotFound";
                errUpdate.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = response;
                return forteresponse;
            }
            else if (!(response.IndexOf("BadRequest") == -1))
            {
                errUpdate.ForteError.ErrorType = "BadRequest";
                errUpdate.ForteError.Message = response;
                forteresponse.Result = false;
                forteresponse.Massege = response;
                return forteresponse;
            }
            else
            {
                forteresponse.Result = false;
                forteresponse.Massege = response; ;
                return forteresponse;
            }
        }
    }
}
