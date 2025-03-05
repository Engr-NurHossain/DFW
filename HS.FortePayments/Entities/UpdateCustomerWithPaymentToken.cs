using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forte.Entities
{
    public class UpdateCustomerWithPaymentToken
    {
        [JsonProperty("paymethod_token")]
        public string paymethod_token
        {
            get;
            set;
        }
    }
}
