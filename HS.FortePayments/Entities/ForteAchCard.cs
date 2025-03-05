using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forte.Entities
{
    public class ForteAchCard
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

        [JsonProperty("account_number")]
        public string account_number
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
