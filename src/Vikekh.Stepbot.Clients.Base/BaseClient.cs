using Vikekh.Stepbot.Interfaces;

namespace Vikekh.Stepbot.Clients.Base
{
    public abstract class BaseClient : IClient
    {
        public abstract bool SendMessage(string channelId, string message);
    }
}
