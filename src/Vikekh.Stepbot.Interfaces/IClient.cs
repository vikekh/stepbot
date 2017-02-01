namespace Vikekh.Stepbot.Interfaces
{
    public interface IClient
    {
        //bool IsRecipient();

        bool SendMessage(string channelId, string message);
    }
}
