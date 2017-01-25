using Newtonsoft.Json;
using System.Collections.Generic;

namespace Vikekh.Stepbot.Clients.Slack
{
    public class Config
    {
        [JsonProperty("modules")]
        public IEnumerable<string> Modules { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
