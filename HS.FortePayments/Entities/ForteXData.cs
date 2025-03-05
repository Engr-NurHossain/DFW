using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Forte
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ForteXData
    {
        [JsonProperty("xdata_1")]
        public string xdata_1
        {
            get;
            set;
        }

        [JsonProperty("xdata_2")]
        public string xdata_2
        {
            get;
            set;
        }

        [JsonProperty("xdata_3")]
        public string xdata_3
        {
            get;
            set;
        }

        [JsonProperty("xdata_4")]
        public string xdata_4
        {
            get;
            set;
        }

        [JsonProperty("xdata_5")]
        public string xdata_5
        {
            get;
            set;
        }

        [JsonProperty("xdata_6")]
        public string xdata_6
        {
            get;
            set;
        }

        [JsonProperty("xdata_7")]
        public string xdata_7
        {
            get;
            set;
        }

        [JsonProperty("xdata_8")]
        public string xdata_8
        {
            get;
            set;
        }

        [JsonProperty("xdata_9")]
        public string xdata_9
        {
            get;
            set;
        }
    }
}
