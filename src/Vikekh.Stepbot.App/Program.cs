using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vikekh.Stepbot.Clients.Slack;

namespace Vikekh.Stepbot.App
{
    class Program
    {
        private SlackClient SlackClient { get; set; }

        public void InitSlack(ManualResetEventSlim manualResetEventSlim, Action<string> Log)
        {
            SlackClient = new SlackClient(manualResetEventSlim, Log);
            //manualResetEventSlim.Wait();
        }

        static void Main(string[] args)
        {
            var program = new Program();
            var manualResetEventSlim = new ManualResetEventSlim(false);
            program.InitSlack(manualResetEventSlim, (s) => { Console.WriteLine(s); });
            manualResetEventSlim.Wait();
            Console.ReadLine();
        }
    }
}
