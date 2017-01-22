namespace Vikekh.Stepbot.Interfaces
{
    public interface IClient
    {
        bool SendMessage(string channelId, string message);
    }
}
