using System.Text.Json.Serialization;

namespace CwApi
{
    public class PingResult
    {
        [JsonInclude]
        public string uid;
    }
}
