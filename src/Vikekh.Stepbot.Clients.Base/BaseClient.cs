using Vikekh.Stepbot.Interfaces;

namespace Vikekh.Stepbot.Clients.Base
{
    public abstract class BaseClient<TConfig> : IClient<TConfig> where TConfig : IConfig
    {
        public TConfig Config { get; set; }

        public abstract bool SendMessage(string channelId, string message);

        public BaseClient()
        {
            Config = Common.Config.GetConfig<TConfig>();
        }
    }
}
