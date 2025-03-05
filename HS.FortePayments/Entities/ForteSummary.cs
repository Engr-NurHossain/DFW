using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Forte
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ForteSummary
    {
        [JsonProperty("schedule_next_date")]
        public string schedule_next_date
        {
            get;
            set;
        }

        [JsonProperty("schedule_next_amount")]
        public string schedule_next_amount
        {
            get;
            set;
        }

        [JsonProperty("schedule_successful_amount")]
        public string schedule_successful_amount
        {
            get;
            set;
        }

        [JsonProperty("schedule_successful_quantity")]
        public string schedule_successful_quantity
        {
            get;
            set;
        }

        [JsonProperty("schedule_failed_amount")]
        public string schedule_failed_amount
        {
            get;
            set;
        }

        [JsonProperty("schedule_failed_quantity")]
        public string schedule_failed_quantity
        {
            get;
            set;
        }

        [JsonProperty("scheduled_remaining_amount")]
        public string scheduled_remaining_amount
        {
            get;
            set;
        }

        [JsonProperty("scheduled_remaining_quantity")]
        public string scheduled_remaining_quantity
        {
            get;
            set;
        }

        [JsonProperty("scheduled_suspended_amount")]
        public string scheduled_suspended_amount
        {
            get;
            set;
        }

        [JsonProperty("scheduled_suspended_quantity")]
        public string scheduled_suspended_quantity
        {
            get;
            set;
        }
    }
}
