using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CwApi
{
    public class CVE
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("cve_code")]
        public string CVECode { get; set; }

        [JsonPropertyName("cvss")]
        public _CVSS CVSS { get; set; }

        [JsonPropertyName("cvss_custom")]
        public string CVSSCustom { get; set; }

        [JsonPropertyName("cvss_v3")]
        public _CVSSV3 CVSSV3 { get; set; }

        [JsonPropertyName("exploit_code_maturity")]
        public string ExploitCodeMaturity { get; set; }

        [JsonPropertyName("exploitable")]
        public bool Exploitable { get; set; }

        [JsonPropertyName("level")]
        public string Level { get; set; }

        [JsonPropertyName("last_modified")]
        public DateTime? LastModified { get; set; }

        [JsonPropertyName("published")]
        public DateTime? Published { get; set; }

        [JsonPropertyName("score")]
        public float? Score { get; set; }

        [JsonPropertyName("score_v2")]
        public float? ScoreV2 { get; set; }

        [JsonPropertyName("score_v3")]
        public float? ScoreV3 { get; set; }

        [JsonPropertyName("score_custom")]
        public string CustomScore { get; set; }

        [JsonPropertyName("technologies")]
        public _Technology[] Technologies { get; set; }

        public class _CVSS
        {
            [JsonPropertyName("access_complexity")]
            public string AccessComplexity { get; set; }

            [JsonPropertyName("access_vector")]
            public string AccessVector { get; set; }

            [JsonPropertyName("authentication")]
            public string Authentication { get; set; }

            [JsonPropertyName("availability_impact")]
            public string AvailabilityImpact { get; set; }

            [JsonPropertyName("confidentiality_impact")]
            public string ConfidentalityImpact { get; set; }

            [JsonPropertyName("integrity_impact")]
            public string IntegrityImpact { get; set; }
        }


        public class _CVSSV3
        {
            [JsonPropertyName("access_complexity")]
            public string AccessComplexity { get; set; }

            [JsonPropertyName("access_vector")]
            public string AccessVector { get; set; }

            [JsonPropertyName("availability_impact")]
            public string AvailabilityImpact { get; set; }

            [JsonPropertyName("confidentiality_impact")]
            public string ConfidentalityImpact { get; set; }

            [JsonPropertyName("integrity_impact")]
            public string IntegrityImpact { get; set; }

            [JsonPropertyName("privileges_required")]
            public string PrivilegesRequired { get; set; }

            [JsonPropertyName("scope")]
            public string Scope { get; set; }

            [JsonPropertyName("user_interaction")]
            public string UserInteraction { get; set; }
        }

        public class _Technology
        {
            [JsonPropertyName("product")]
            public string Product { get; set; }

            [JsonPropertyName("vendor")]
            public string Vendor { get; set; }
        }
    }
}
