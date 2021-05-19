using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CwApi
{
    public class ServerDetails
    {
        [JsonPropertyName("addresses")]
        public string[] NetworkAddresses { get; set; }

        [JsonPropertyName("analyzed_at")]
        public DateTime? LastAnalysis { get; set; }

        [JsonPropertyName("boot_at")]
        public DateTime? BootTime { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("compliance_groups")]
        public List<_ComplianceGroup> ComplianceGroups { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreationTime { get; set; }

        [JsonPropertyName("cve_announcements")]
        public List<CVE> CVEs { get; set; }

        [JsonPropertyName("cve_announcements_count")]
        public int? CVECount { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("environment")]
        public _Environment Environment { get; set; }

        [JsonPropertyName("groups")]
        public List<_Group> Groups { get; set; }

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("last_communication")]
        public DateTime? LastCommunication { get; set; }

        [JsonPropertyName("os")]
        public _OperatingSystem OperatingSystem { get; set; }

        [JsonPropertyName("prioritized_cve_announcements_count")]
        public int? PriorityCVECount { get; set; }

        [JsonPropertyName("reboot_required")]
        public bool? RebootRequired { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("updates_count")]
        public int? UpdatesCount { get; set; }

        [JsonPropertyName("updates")]
        public List<_Update> Updates { get; set; }

        public class _ComplianceGroup
        {

            [JsonExtensionData]
            public IDictionary<string, object> ExtensionData
            {
                get { return _extensionData; }
                set
                {
                    if (!(value is ExtensionDataHandler<_ComplianceGroup>)) {
                        foreach(KeyValuePair<string, object> pair in value) {
                            _extensionData.Add(pair.Key, pair.Value);
                        }
                    }
                }
            }
            private static ExtensionDataHandler<_ComplianceGroup> _extensionData =
                new ExtensionDataHandler<_ComplianceGroup>();
        }
        
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
            public DateTime? EndOfLife { get; set; }

            [JsonPropertyName("key")]
            public string Key { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("short_name")]
            public string ShortName { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }
        }

        public class _SecurityIssue
        {
            [JsonPropertyName("description")]
            public string Description { get; set; }

            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("level")]
            public string Level { get; set; }

            [JsonPropertyName("sid")]
            public string SecurityId { get; set; }

            [JsonPropertyName("title")]
            public string Title { get; set; }
        }

        public class _Update
        {
            [JsonPropertyName("current")]
            public _Productdescriptor Current { get; set; }

            [JsonPropertyName("cve_announcements")]
            public string[] CveAnnouncements { get; set; }

            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("ignored")]
            public bool? Ignored { get; set; }

            [JsonPropertyName("patchable")]
            public bool? Patchable { get; set; }

            [JsonPropertyName("target")]
            public _Productdescriptor Target { get; set; }

            public class _Productdescriptor
            {
                [JsonPropertyName("product")]
                public string Product { get; set; }

                [JsonPropertyName("type")]
                public string Type { get; set; }

                [JsonPropertyName("vendor")]
                public string Vendor { get; set; }

                [JsonPropertyName("version")]
                public string Version { get; set; }
            }
        }
    }
}
