﻿using Newtonsoft.Json;

namespace HS.Kickbox.Models
{
    public class KickBoxRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("apikey")]
        public string ApiKey { get; set; }

        [JsonProperty("timeout")]
        public int Timeout { get; set; }
    }
}
