using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Forte
{
    public class ForteOptions
    {
        [JsonProperty("Account_Id")]
        public string Account_Id { get; set; }

        [JsonProperty("Location_Id")]
        public string Location_Id { get; set; }

        [JsonProperty("Organization_Id")]
        public string Organization_ID { get; set; }

        [JsonProperty("Customer_Token")]
        public string Customer_Token { get; set; }

        [JsonProperty("Address_Id")]
        public string Address_Id { get; set; }

        [JsonProperty("Paymethod_Token")]
        public string Paymethod_Token { get; set; }

        [JsonProperty("Transaction_Id")]
        public string Transaction_Id { get; set; }

        [JsonProperty("Schedule_Id")]
        public string Schedule_Id { get; set; }

        [JsonProperty("Schedule_Item_Id")]
        public string Schedule_Item_Id { get; set; }

        [JsonProperty("Resource")]
        public string Resource { get; set; }

        [JsonProperty("Environment")]
        public ForteEnvironment Environment { get; set; }

        public string UserId { get; set; }

        public string Password { get; set; }

        public string AuthAccountId { get; set; }

        public string Server { get; set; }

    }
}
