namespace StockBot
{
    public class StockCommandProcessor : IStockCommandProcessor
    {
        private BotCommand cmd;
        private readonly IStockService stockService;

        public StockCommandProcessor(IStockService stockService)
        {
            this.stockService = stockService;
        }

        public void SetCommand(BotCommand cmd)
        {
            this.cmd = cmd;
        }

        public string Process()
        {
            var stock = this.stockService.GetStock(this.cmd.Value);

            if (stock == null)
                return $"Invalid stock: {this.cmd.Value}";

            if (stock.Close == "N/D")
                return $"{stock.Symbol} quote is not available";

            return $"{stock.Symbol} quote is ${stock.Close} per share";
        }
    }
}
