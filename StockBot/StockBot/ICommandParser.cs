namespace StockBot
{
    public interface ICommandParser
    {
        BotCommand Parse(string message);
    }
}