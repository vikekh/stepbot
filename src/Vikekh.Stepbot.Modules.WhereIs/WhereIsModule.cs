using System.Collections.Generic;
using Vikekh.Stepbot.Interfaces;

namespace Vikekh.Stepbot.Modules.WhereIs
{
    public class WhereIsModule : IModule
    {
        private IDictionary<string, string> Data { get; set; }

        public WhereIsModule()
        {
            Data = new Dictionary<string, string>();
        }

        public string Exec(string message, string user)
        {
            if (!message.StartsWith("<@"))
            {
                Data[user] = message;
                return null;
            }

            try
            {
                var otherUser = message.Substring(2, 9);

                if (!Data.ContainsKey(otherUser) || string.IsNullOrEmpty(Data[otherUser]))
                {
                    return string.Format("Jag vet inte var <@{0}> är", otherUser);
                }

                return string.Format("<@{0}> är {1}", otherUser, Data[otherUser]);
            }
            catch
            {
                return null;
            }
        }
    }
}
