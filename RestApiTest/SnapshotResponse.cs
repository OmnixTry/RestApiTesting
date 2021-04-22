using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace RestApiTest
{
    public partial class SnapshotResponse
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("archived_snapshots")]
        public ArchivedSnapshots ArchivedSnapshots { get; set; }
    }

    public partial class ArchivedSnapshots
    {
        [JsonProperty("closest")]
        public Closest Closest { get; set; }
    }

    
}
