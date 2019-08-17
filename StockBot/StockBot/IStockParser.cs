namespace StockBot
{
    public interface IStockParser
    {
        Stock Parse(string content);
    }
}