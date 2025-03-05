using Newtonsoft.Json;

namespace Forte
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ForteAddress : ForteResourceClass
    {
        [JsonProperty("address_token")]
        public string address_token
        {
            get;
            set;
        }

        [JsonProperty("customer_token")]
        public string customer_token
        {
            get;
            set;
        }

        [JsonProperty("account_id")]
        public string account_id
        {
            get;
            set;
        }

        [JsonProperty("location_id")]
        public string location_id
        {
            get;
            set;
        }

        [JsonProperty("label")]
        public string label
        {
            get;
            set;
        }

        [JsonProperty("first_name")]
        public string first_name
        {
            get;
            set;
        }

        [JsonProperty("last_name")]
        public string last_name
        {
            get;
            set;
        }

        [JsonProperty("company_name")]
        public string company_name
        {
            get;
            set;
        }

        [JsonProperty("phone")]
        public string phone
        {
            get;
            set;
        }

        [JsonProperty("fax")]
        public string fax
        {
            get;
            set;
        }

        [JsonProperty("email")]
        public string email
        {
            get;
            set;
        }

        [JsonProperty("address_type")]
        public string address_type
        {
            get;
            set;
        }

        [JsonProperty("shipping_address_type")]
        public string shipping_address_type
        {
            get;
            set;
        }

        [JsonProperty("physical_address")]
        public FortePhysicalAddress physical_address
        {
            get;
            set;
        }

        public ForteAddress()
        {
            physical_address = new FortePhysicalAddress();
            links = new ForteLinks();
            response = new ForteResponse();
        }
    }
}
