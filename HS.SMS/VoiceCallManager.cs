using System;
using System.Collections.Generic;
using Plivo;
using Plivo.Exception;
using Plivo.Resource.Call;

namespace HS.SMS
{
    public class VoiceCallManager
    {
        public static CallCreateResponse MakeACall(List<string> to,string From,string AuthId,string AuthToken)
        {
            var api = new PlivoApi(AuthId, AuthToken);
            CallCreateResponse response = api.Call.Create(
                to: to,
                from: From,
                answerMethod: "GET",
                answerUrl: "http://s3.amazonaws.com/static.plivo.com/answer.xml"
            );
            //Console.WriteLine(response);
            return response;
        }
    }
}
