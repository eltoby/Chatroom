namespace StockBot
{
    using System.Collections.Generic;
    using System.Linq;

    public class StockParser : IStockParser
    {
        public Stock Parse(string content)
        {
            var keys = new Dictionary<string, int>();

            var lines = content.Split("\r\n");
            var cols = lines.First().Split(',');

            for (var i = 0; i < cols.Length; i++)
                keys.Add(cols[i], i);

            var values = lines[1].Split(',');

            var stock = new Stock
            {
                Symbol = values[keys["Symbol"]],
                Close = values[keys["Close"]]
            };

            return stock;
        }
    }
}
