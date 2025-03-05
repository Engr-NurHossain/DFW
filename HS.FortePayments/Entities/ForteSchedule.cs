using Newtonsoft.Json;
using System;

namespace Forte
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ForteSchedule : ForteResourceClass
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

        //[JsonProperty("schedule_id")]
        //public string schedule_id
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("customer_token")]
        //public string customer_token
        //{
        //    get;
        //    set;
        //}

        [JsonProperty("paymethod_token")]
        public string paymethod_token
        {
            get;
            set;
        }

        [JsonProperty("action")]
        public string action
        {
            get;
            set;
        }

        [JsonProperty("schedule_quantity")]
        public string schedule_quantity
        {
            get;
            set;
        }

        [JsonProperty("schedule_frequency")]
        public string schedule_frequency
        {
            get;
            set;
        }

        [JsonProperty("schedule_amount")]
        public string schedule_amount
        {
            get;
            set;
        }

        [JsonProperty("schedule_start_date")]
        public string schedule_start_date
        {
            get;
            set;
        }

        //[JsonProperty("schedule_created_date")]
        //public string schedule_created_date
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("customer_acct_code")]
        //public string customer_acct_code
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("sec_code")]
        //public string sec_code
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("schedule_status")]
        //public string schedule_status
        //{
        //    get;
        //    set;
        //}

        //[JsonProperty("item_description")]
        //public string item_description
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

        //[JsonProperty("order_number")]
        //public string order_number
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

        //[JsonProperty("summary")]
        //public ForteSummary summary
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

        //public ForteSchedule()
        //{
        //    summary = new ForteSummary();
        //    xdata = new ForteXData();
        //    links = new ForteLinks();
        //    response = new ForteResponse();
        //}
    }
}
