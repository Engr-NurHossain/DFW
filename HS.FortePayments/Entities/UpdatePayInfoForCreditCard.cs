using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forte.Entities
{
    public class UpdatePayInfoForCreditCard
    {
        [JsonProperty("card_type")]
        public string card_type
        {
            get;
            set;
        }

        [JsonProperty("name_on_card")]
        public string name_on_card
        {
            get;
            set;
        }

       
      

        [JsonProperty("expire_month")]
        public string expire_month
        {
            get;
            set;
        }

        [JsonProperty("expire_year")]
        public string expire_year
        {
            get;
            set;
        }

      

       
    }
}
