using Newtonsoft.Json;

namespace Forte
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ForteLinks
    {
        [JsonProperty("self")]
        public string self
        {
            get;
            set;
        }

        [JsonProperty("previous")]
        public string previous
        {
            get;
            set;
        }

        [JsonProperty("next")]
        public string next
        {
            get;
            set;
        }

        [JsonProperty("customers")]
        public string customers
        {
            get;
            set;
        }

        [JsonProperty("addresses")]
        public string addresses
        {
            get;
            set;
        }

        [JsonProperty("paymethods")]
        public string paymethods
        {
            get;
            set;
        }

        [JsonProperty("transactions")]
        public string transactions
        {
            get;
            set;
        }

        [JsonProperty("settlements")]
        public string settlements
        {
            get;
            set;
        }

        [JsonProperty("schedules")]
        public string schedules
        {
            get;
            set;
        }

        [JsonProperty("scheduleitems")]
        public string scheduleitems
        {
            get;
            set;
        }
    }
}
