using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Forte
{
    public enum ForteEnvironment
    {
        LIVE,
        SANDBOX
    }


    public class ForteEnvironmentDetails
    {
        [Required]
        public string Environment { get; set; }
        [Required]
        public string Server { get; set; }
        [Required]
        public string AuthAccountid { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public string objJson { get; set; }

    }

    public class ForteResourceClass : ForteEnvironmentDetails
    {
        [JsonProperty("response")]
        public ForteResponse response
        {
            get;
            set;
        }

        [JsonProperty("links")]
        public ForteLinks links
        {
            get;
            set;
        }
    }
}
