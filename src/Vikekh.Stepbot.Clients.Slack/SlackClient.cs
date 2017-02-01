using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        private IDictionary<string, IModule> Modules { get; set; }

        public SlackClient() : base()
        {
            ManualResetEventSlim = new ManualResetEventSlim(false);
            Client = new SlackSocketClient(Config.Token);
        }

        private IModule GetModule(string name)
        {
            if (Modules == null) Modules = new Dictionary<string, IModule>();

            if (Modules.ContainsKey(name) && Modules[name] != null)
            {
                return Modules[name];
            }

            IModule module = null;

            if (name.Equals("WhereIs"))
            {
                module = new Modules.WhereIs.WhereIsModule();
            }

            if (module != null)
            {
                Modules[name] = module;
            }

            return module;
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
                Console.WriteLine("{0}: {1}", newMessage.user, newMessage.text);
                client.Route(Utils.ParseMessage(newMessage.text), newMessage.channel, newMessage.user);
            };

            client.ManualResetEventSlim.Wait();
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
