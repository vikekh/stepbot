namespace Vikekh.Stepbot.Interfaces
{
    public interface IModule
    {
        bool Exec(IClient<IConfig> client, string args, string channelId, string user);
    }
}
