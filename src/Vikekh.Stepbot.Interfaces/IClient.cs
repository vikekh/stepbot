namespace Vikekh.Stepbot.Interfaces
{
    public interface IClient<IConfig>
    {
        IConfig Config { get; set; }

        bool SendMessage(string channelId, string message);
    }
}
