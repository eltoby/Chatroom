namespace StockBot
{
    public interface IStockService
    {
        Stock GetStock(string stockCode);
    }
}