using Newtonsoft.Json;

namespace Forte
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ForteResponse
    {
        [JsonProperty("environment")]
        public string environment
        {
            get;
            set;
        }

        [JsonProperty("response_type")]
        public string response_type
        {
            get;
            set;
        }

        [JsonProperty("response_code")]
        public string response_code
        {
            get;
            set;
        }

        [JsonProperty("response_desc")]
        public string response_desc
        {
            get;
            set;
        }
    }
}
