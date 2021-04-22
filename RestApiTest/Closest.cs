using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiTest
{
    public partial class Closest
    {
        [JsonProperty("status")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long Status { get; set; }

        [JsonProperty("available")]
        public bool Available { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}
