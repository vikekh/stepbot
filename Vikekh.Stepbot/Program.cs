using SlackAPI;
using System;
using System.Configuration;
using System.Threading;

namespace Vikekh.Stepbot
{
    public class Program
    {
        private SlackSocketClient Client { get; set; }

        private ManualResetEventSlim ManualResetEventSlim { get; set; }

        public Program()
        {
            var botAuthToken = ConfigurationManager.AppSettings["BotAuthToken"];
            ManualResetEventSlim = new ManualResetEventSlim(false);
            Client = new SlackSocketClient(botAuthToken);
        }

        public static void Main(string[] args)
        {
            var program = new Program();

            program.Client.Connect((loginResponse) =>
            {
                // This is called once the client has emitted the RTM start command
                program.ManualResetEventSlim.Set();
            }, () =>
            {
                // This is called once the RTM client has connected to the end point
            });

            program.Client.OnMessageReceived += (newMessage) =>
            {
                // Handle each message as you receive them
                var text = newMessage.text.Trim();
                var id = string.Format("<@{0}>", program.Client.MySelf.id);

                if (!text.StartsWith(id))
                {
                    return;
                }

                Console.WriteLine("{0}: {1}", newMessage.user, newMessage.text);
                var textData = program.Route(text.Substring(id.Length).Trim());

                if (textData != null)
                {
                    program.Client.SendMessage((messageReceived) => { }, newMessage.channel, textData);
                }
            };

            program.ManualResetEventSlim.Wait();
            Console.ReadLine();
        }

        private string Route(string route)
        {
            string text = null;

            if (route.ToLower().StartsWith("hello"))
            {
                text = "Hej på dig!";
            }

            return text;
        }
    }
}
