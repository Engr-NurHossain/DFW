using Newtonsoft.Json;

namespace Forte
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FortePhysicalAddress
    {
        [JsonProperty("street_line1")]
        public string street_line1
        {
            get;
            set;
        }

        [JsonProperty("street_line2")]
        public string street_line2
        {
            get;
            set;
        }

        [JsonProperty("locality")]
        public string locality
        {
            get;
            set;
        }

        [JsonProperty("region")]
        public string region
        {
            get;
            set;
        }

        [JsonProperty("postal_code")]
        public string postal_code
        {
            get;
            set;
        }
    }
}
