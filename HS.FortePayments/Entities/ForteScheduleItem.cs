using Newtonsoft.Json;
using System;

namespace Forte
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ForteScheduleItem : ForteResourceClass
    {
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

        [JsonProperty("schedule_item_id")]
        public string schedule_item_id
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

        [JsonProperty("paymethod_token")]
        public string paymethod_token
        {
            get;
            set;
        }

        [JsonProperty("schedule_id")]
        public string schedule_id
        {
            get;
            set;
        }

        [JsonProperty("schedule_item_amount")]
        public string schedule_item_amount
        {
            get;
            set;
        }

        [JsonProperty("schedule_item_status")]
        public string schedule_item_status
        {
            get;
            set;
        }

        [JsonProperty("schedule_item_date")]
        public string schedule_item_date
        {
            get;
            set;
        }

        [JsonProperty("schedule_item_processed_date")]
        public string schedule_item_processed_date
        {
            get;
            set;
        }

        [JsonProperty("schedule_item_created_date")]
        public string schedule_item_created_date
        {
            get;
            set;
        }

        [JsonProperty("schedule_item_description")]
        public string schedule_item_description
        {
            get;
            set;
        }

        public ForteScheduleItem()
        {
            links = new ForteLinks();
            response = new ForteResponse();
        }
    }
}
