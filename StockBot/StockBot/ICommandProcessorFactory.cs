namespace StockBot
{
    public interface ICommandProcessorFactory
    {
        ICommandProcessor Create(BotCommand cmd);
    }
}