namespace Vikekh.Stepbot.Interfaces
{
    public interface IModule
    {
        bool Exec(IClient client, string args, string channelId, string user);
    }
}
