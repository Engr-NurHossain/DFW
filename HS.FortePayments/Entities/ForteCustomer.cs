using System.Collections.Generic;
using Newtonsoft.Json;

namespace Forte
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ForteCustomer : ForteResourceClass
    {
        //[JsonProperty("account_id")]
        //public string account_id
        //{
        //    get;
        //    set;
        //}

        [JsonProperty("location_id")]
        public string location_id
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

 
        [JsonProperty("customer_id")]
        public string customer_id
        {
            get;
            set;
        }

        //[JsonProperty("default_paymethod_token")]
        //public string default_paymethod_token
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("default_billing_address_token")]
        //public string default_billing_address_token
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("default_shipping_address_token")]
        //public string default_shipping_address_token
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("status")]
        //public string status
        //{
        //    get;
        //    set;
        //}

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

        //[JsonProperty("paymethod")]
        //public FortePaymethod paymethod
        //{
        //    get;
        //    set;
        //}

        [JsonProperty("addresses")]
        public List<ForteAddress> addresses
        {
            get;
            set;
        }

        //public ForteCustomer()
        //{
        //    paymethod = new FortePaymethod();
        //    addresses = new List<ForteAddress>();
        //    response = new ForteResponse();
        //    links = new ForteLinks();
        //}
    }
}
