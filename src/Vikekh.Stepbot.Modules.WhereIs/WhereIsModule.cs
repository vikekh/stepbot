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

        public bool Exec(IClient client, string[] args, string channelId, string userId)
        {
            if (!args[0].StartsWith("<@"))
            {
                Data[userId] = args[0];
                return client.SendMessage(channelId, "OK!");
            }

            try
            {
                var otherUser = args[0].Substring(2, 9);

                if (!Data.ContainsKey(otherUser) || string.IsNullOrEmpty(Data[otherUser]))
                {
                    return client.SendMessage(channelId, string.Format("Jag vet inte var <@{0}> är", otherUser));
                }

                return client.SendMessage(channelId, string.Format("<@{0}> {1}", otherUser, Data[otherUser]));
            }
            catch
            {
                return false;
            }
        }
    }
}
