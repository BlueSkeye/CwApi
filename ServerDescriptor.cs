using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CwApi
{
    public class ServerDescriptor
    {
        public ServerDetails Details { get; set; }

        [JsonPropertyName("environment")]
        public _Environment Environment { get; set; }

        [JsonPropertyName("boot_at")]
        public string LastBoot { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("created_at")]
        public string Created { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("groups")]
        public List<_Group> Groups { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("last_communication")]
        public string LastCommunication { get; set; }

        [JsonPropertyName("os")]
        public _OperatingSystem OS { get; set; }

        [JsonPropertyName("reboot_required")]
        public bool? RebootRequired { get; set; }

        public class _Environment
        {
            [JsonPropertyName("availability_requirement")]
            public string Availability { get; set; }

            [JsonPropertyName("confidentiality_requirement")]
            public string Confidentiality { get; set; }

            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("integrity_requirement")]
            public string Integrity { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

        }

        public class _Group
        {
            [JsonPropertyName("color")]
            public string Color { get; set; }

            [JsonPropertyName("description")]
            public string Description { get; set; }

            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

        }

        public class _OperatingSystem
        {
            [JsonPropertyName("arch")]
            public string Architecture { get; set; }

            [JsonPropertyName("eol")]
            public string EndOfLife { get; set; }

            [JsonPropertyName("key")]
            public string Key { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("short_name")]
            public string ShortName { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }
        }
    }
}
