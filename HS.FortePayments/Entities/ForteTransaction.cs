using Forte.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Forte
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ForteTransaction : ForteResourceClass
    {
        //[JsonProperty("account_id")]
        //public string account_id
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("location_id")]
        //public string location_id
        //{
        //    get;
        //    set;
        //}

        [JsonProperty("action")]
        public string action
        {
            get;
            set;
        }

        //[JsonProperty("customer_token")]
        //public string customer_token
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("customer_id")]
        //public string customer_id
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("paymethod_token")]
        //public string paymethod_token
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("reference_id")]
        //public string reference_id
        //{
        //    get;
        //    set;
        //}

        [JsonProperty("authorization_amount")]
        public string authorization_amount
        {
            get;
            set;
        }

        //[JsonProperty("order_number")]
        //public string order_number
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("original_transaction_id")]
        //public string original_transaction_id
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("transaction_id")]
        //public string transaction_id
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("authorization_code")]
        //public string authorization_code
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("entered_by")]
        //public string entered_by
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("received_date")]
        //public string received_date
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("origination_date")]
        //public string origination_date
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("sales_tax_amount")]
        //public string sales_tax_amount
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("service_fee_amount")]
        //public string service_fee_amount
        //{
        //    get;
        //    set;
        //}
        [JsonProperty("billing_address")]
        public string billing_address
        {
            get;
            set;
        }

        public List<billing_addressClass> billing_addressList { get; set; }
        public class billing_addressClass
        {
            [JsonProperty(PropertyName = "first_name")]
            public string first_name { get; set; }
            [JsonProperty(PropertyName = "last_name")]
            public string last_name { get; set; }
         
        }

        public List<CardClass> CardList { get; set; }
        public List<ForteAchTrans> AchCardList { get; set; }
        public class CardClass
        {
            
            [JsonProperty(PropertyName = "name_on_card")]
            public string name_on_card { get; set; }
            [JsonProperty(PropertyName = "card_type")]
            public string card_type { get; set; }
            [JsonProperty(PropertyName = "account_number")]
            public string account_number { get; set; }
            [JsonProperty(PropertyName = "expire_month")]
            public string expire_month { get; set; }
            [JsonProperty(PropertyName = "expire_year")]
            public string expire_year { get; set; }
            [JsonProperty(PropertyName = "card_verification_value")]
            public string card_verification_value { get; set; }
       

        }
        //[JsonProperty("shipping_address")]
        //public string shipping_address
        //{
        //    get;
        //    set;
        //}

        [JsonProperty("card")]
        public string card
        {
            get;
            set;
        }
        [JsonProperty("echeck")]
        public string echeck
        {
            get;
            set;
        }
        //[JsonProperty("echeck")]
        //public string echeck
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("line_items")]
        //public ForteLineItems line_items
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("xdata")]
        //public ForteXData xdata
        //{
        //    get;
        //    set;
        //}

        //public ForteTransaction()
        //{
        //    line_items = new ForteLineItems();
        //    xdata = new ForteXData();
        //    links = new ForteLinks();
        //    response = new ForteResponse();
        //}
    }
}
