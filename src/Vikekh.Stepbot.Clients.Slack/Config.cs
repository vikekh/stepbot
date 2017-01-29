using System.Collections.Generic;
using Vikekh.Stepbot.Interfaces;

namespace Vikekh.Stepbot.Clients.Slack
{
    public class Config : IConfig
    {
        public IEnumerable<string> Modules { get; set; }
        
        public string Token { get; set; }
    }
}
