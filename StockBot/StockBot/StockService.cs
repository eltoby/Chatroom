namespace StockBot
{
    using System.Net.Http;

    public class StockService : IStockService
    {
        private const string STOCK_URL = "https://stooq.com/q/l/?s=[stockCode]&f=sd2t2ohlcv&h&e=csv";
        private readonly IStockParser stockParser;

        public StockService(IStockParser stockParser)
        {
            this.stockParser = stockParser;
        }

        public Stock GetStock(string stockCode)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(this.GetStockUrl(stockCode)).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                return this.stockParser.Parse(content);
            }
        }

        private string GetStockUrl(string stockCode) => STOCK_URL.Replace("[stockCode]", stockCode);
    }
}
