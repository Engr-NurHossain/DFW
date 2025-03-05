using Newtonsoft.Json;

namespace Forte
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ForteEcheck
    {
        [JsonProperty("account_holder")]
        public string account_holder
        {
            get;
            set;
        }

        [JsonProperty("account_number")]
        public string account_number
        {
            get;
            set;
        }

        [JsonProperty("masked_account_number")]
        public string masked_account_number
        {
            get;
            set;
        }

        [JsonProperty("routing_number")]
        public string routing_number
        {
            get;
            set;
        }

        [JsonProperty("account_type")]
        public string account_type
        {
            get;
            set;
        }

        [JsonProperty("check_number")]
        public string check_number
        {
            get;
            set;
        }

        [JsonProperty("sec_code")]
        public string sec_code
        {
            get;
            set;
        }
    }
}
