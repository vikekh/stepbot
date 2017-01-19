﻿using SlackAPI;
using System;
using System.Threading;

namespace Vikekh.Stepbot
{
	class Program
    {
        static void Main(string[] args)
        {
			var botAuthToken = "";
			var userAuthToken = "";
			var name = "@stepdot";
			var age = (new DateTime(2017, 1, 18) - DateTime.Now).Days / 365.0;
			var ageString = age.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
			var version = "0.1.0";
			var mommy = "@vem";
			ManualResetEventSlim clientReady = new ManualResetEventSlim(false);
			SlackSocketClient client = new SlackSocketClient(botAuthToken);
			client.Connect((connected) =>
			{
				// This is called once the client has emitted the RTM start command
				clientReady.Set();
			}, () =>
			{
				// This is called once the RTM client has connected to the end point
			});
			client.OnMessageReceived += (message) =>
			{
				// Handle each message as you receive them
				Console.WriteLine(message.text);
				var textData = string.Format("hello w0rld my name is {0} I am {1} years old and my version is {2} and my mommy is {3}", name, ageString, version, mommy);
				client.SendMessage((x) => { }, message.channel, textData);
			};
			clientReady.Wait();
			Console.ReadLine();
		}
    }
}
