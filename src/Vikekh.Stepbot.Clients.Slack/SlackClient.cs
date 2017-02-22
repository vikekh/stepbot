using SlackAPI;
using System;
using System.Linq;
using System.Threading;
using Vikekh.Stepbot.Clients.Base;
using Vikekh.Stepbot.Common;
using Vikekh.Stepbot.Interfaces;

namespace Vikekh.Stepbot.Clients.Slack
{
    public class SlackClient : BaseClient<Config>, IClient
    {
        private SlackSocketClient Client { get; set; }

        private ManualResetEventSlim ManualResetEventSlim { get; set; }

        public SlackClient(Action<string> Log = null) : base()
        {
            //var manualResetEventSlim = new ManualResetEventSlim(false);
            Client = new SlackSocketClient(Config.Token);
            Log("Created client.");

            Client.Connect((loginResponse) =>
            {
                Log("Connected.");
                // This is called once the client has emitted the RTM start command
                //manualResetEventSlim.Set();
            }, () =>
            {
                Log("Socket connected.");
                // This is called once the RTM client has connected to the end point
            });

            Client.OnMessageReceived += (newMessage) =>
            {
                Log("Message recieved.");
                // Handle each message as you receive them
                Route(Utils.ParseMessage(newMessage.text), newMessage.channel, newMessage.user);
            };

            //manualResetEventSlim.Wait();
        }

        public static void Main(string[] args)
        {
            //var manualResetEventSlim = new ManualResetEventSlim(false);
            new SlackClient((s) => { Console.WriteLine(s); });
            Console.ReadLine();
        }

        private bool Route(string[] args, string channelId, string userId)
        {
            if (args[0].StartsWith(string.Format("<@{0}>", Client.MySelf.id)))
            {
                args = args.Skip(1).ToArray();
            }
            else
            {
                return false;
            }

            args[0] = args[0].ToLower();

            if (args[0].Equals("whereis"))
            {
                args = args.Skip(1).ToArray();
                return GetModule("WhereIs").Exec((IClient)this, args, channelId, userId);
            }

            if (args[0].Equals("version"))
            {
                return SendMessage(channelId, string.Format("Stepbot Slack v{0} https://github.com/vikekh/stepbot", Version));
            }

            return false;
        }

        public override bool SendMessage(string channelId, string message)
        {
            Client.SendMessage((messageReceived) => { }, channelId, message);
            return true;
        }
    }
}
