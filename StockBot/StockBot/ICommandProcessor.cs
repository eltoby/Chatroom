namespace StockBot
{
    public interface ICommandProcessor
    {
        void SetCommand(BotCommand cmd);

        string Process();
    }
}
