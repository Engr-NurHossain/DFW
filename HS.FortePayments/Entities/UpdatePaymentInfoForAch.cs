using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forte.Entities
{
    public class UpdatePaymentInfoForAch
    {
        [JsonProperty("account_holder")]
        public string account_holder
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




        [JsonProperty("item_description")]
        public string item_description
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
    }
}
