using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CwApi
{
    public class HostResult
    {
        [JsonPropertyName("created_at")]
        public string Created { get; set; }

        [JsonPropertyName("discovery")]
        public Discovery Disco { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("target")]
        public string IpAddress { get; set; }

        [JsonPropertyName("node_id")]
        public int NodeId { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("server_ids")]
        public List<int> ServerIds { get; set; }

        [JsonPropertyName("updated_at")]
        public string Updated { get; set; }

        public class Discovery
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }
        }
    }
}
