using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.CSM.Models;

namespace HS.CSM
{
    public static class JupiterLeadMigration
    {
        public static GetLeadsResponseList GetLeads(GetLeadsRequest Request)
        {
            #region Request Params 
            string URL = "https://jupiter.centralstationmarketing.com/api/jupiter_lead_migration.php";
            string RequestString = @"siteId={0}&startDate={1}&endDate={2}&qty={3}&page={4}&token={5}";
            RequestString = string.Format(RequestString, 
                Request.siteId,
                Request.startDate.ToString("yyyy-MM-dd"),
                Request.endDate.ToString("yyyy-MM-dd"),
                Request.qty,
                Request.page,
                Request.token);
            #endregion

            var client = new RestClient(URL);
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "8c2816fb-a16c-dcab-28e2-74010040858e");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", RequestString, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            JupiterResponse result =  Newtonsoft.Json.JsonConvert.DeserializeObject<JupiterResponse>(response.Content);
            //result.GetLeadsResponse = result.GetLeadsResponse.OrderBy(x => x.timestamp).ToList();


            GetLeadsResponseList getLeadsResponseList = new GetLeadsResponseList()
            {
                //GetLeadsResponse = result.GetLeadsResponseList[0].GetLeadsResponse,
                //GetNotesResponse = result.GetLeadsResponseList[1].GetNotesResponse
            };

            foreach(var item in result.GetLeadsResponseList)
            {
                if(item.GetLeadsResponse != null)
                {
                    getLeadsResponseList.GetLeadsResponse = item.GetLeadsResponse;
                }
                if (item.GetNotesResponse != null)
                {
                    getLeadsResponseList.GetNotesResponse = item.GetNotesResponse;
                }
            }

            return getLeadsResponseList;//result;
        }
    }
}
