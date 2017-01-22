using SlackAPI;
using System;
using System.Configuration;
using System.Threading;
using Vikekh.Stepbot.Interfaces;

namespace Vikekh.Stepbot
{
    public class SlackClient : IClient
    {
        private const string Version = "0.2.0";

        private SlackSocketClient Client { get; set; }

        private ManualResetEventSlim ManualResetEventSlim { get; set; }

        private Modules.WhereIs.WhereIsModule WhereIsModule { get; set; }

        public SlackClient()
        {
            var botAuthToken = ConfigurationManager.AppSettings["BotAuthToken"];
            ManualResetEventSlim = new ManualResetEventSlim(false);
            Client = new SlackSocketClient(botAuthToken);
        }

        public static void Main(string[] args)
        {
            var client = new SlackClient();

            client.Client.Connect((loginResponse) =>
            {
                // This is called once the client has emitted the RTM start command
                client.ManualResetEventSlim.Set();
            }, () =>
            {
                // This is called once the RTM client has connected to the end point
            });

            client.Client.OnMessageReceived += (newMessage) =>
            {
                // Handle each message as you receive them
                var text = newMessage.text.Trim();
                var id = string.Format("<@{0}>", client.Client.MySelf.id);

                if (!text.StartsWith(id))
                {
                    return;
                }

                Console.WriteLine("{0}: {1}", newMessage.user, newMessage.text);
                client.Route(text.Substring(id.Length).Trim(), newMessage.channel, newMessage.user);
            };

            client.ManualResetEventSlim.Wait();
            Console.ReadLine();
        }

        private bool Route(string route, string channelId, string userId)
        {
            if (route.ToLower().StartsWith("hello"))
            {
                return SendMessage(channelId, "Hej på dig!");
            }

            if (route.ToLower().StartsWith("whereis"))
            {
                if (WhereIsModule == null)
                {
                    WhereIsModule = new Modules.WhereIs.WhereIsModule();
                }

                return WhereIsModule.Exec(this, route.Substring("whereis".Length).Trim(), channelId, userId);
            }

            if (route.ToLower().StartsWith("version"))
            {
                return SendMessage(channelId, string.Format("Stepbot v{0} https://github.com/vikekh/stepbot", Version));
            }

            return false;
        }

        public bool SendMessage(string channelId, string message)
        {
            Client.SendMessage((messageReceived) => { }, channelId, message);
            return true;
        }
    }
}
