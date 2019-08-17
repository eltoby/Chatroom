namespace StockBot
{
    public class CommandProcessorFactory : ICommandProcessorFactory
    {
        private readonly IStockCommandProcessor stockCommandProcessor;

        public CommandProcessorFactory(IStockCommandProcessor stockCommandProcessor)
        {
            this.stockCommandProcessor = stockCommandProcessor;
        }

        public ICommandProcessor Create(BotCommand cmd)
        {
            ICommandProcessor result;

            switch (cmd.Key)
            {
                case "stock":
                    result = this.stockCommandProcessor;
                    break;
                default:
                    return null;
            }

            result.SetCommand(cmd);
            return result;
        }
    }
}
