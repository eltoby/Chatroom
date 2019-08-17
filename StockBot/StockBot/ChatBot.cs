namespace StockBot
{
    public class ChatBot : IChatBot
    {
        private readonly ICommandParser commandParser;
        private readonly IValidCommandsCatalog validCommandsCatalog;
        private readonly ICommandProcessorFactory commandProcessorFactory;

        public ChatBot(ICommandParser commandParser, IValidCommandsCatalog validCommandsCatalog, ICommandProcessorFactory commandProcessorFactory)
        {
            this.commandParser = commandParser;
            this.validCommandsCatalog = validCommandsCatalog;
            this.commandProcessorFactory = commandProcessorFactory;
        }

        public string Process(string message)
        {
            var cmd = this.commandParser.Parse(message);

            if (!cmd.IsCommand)
                return string.Empty;

            if (!this.validCommandsCatalog.IsValidCommand(cmd.Key))
            {
                var validCommands = string.Join(",", this.validCommandsCatalog.GetValidCommands());
                return $"Invalid command. Please use a valid command (Valid commands: {validCommands})";
            }

            var cmdProcessor = this.commandProcessorFactory.Create(cmd);
            return cmdProcessor.Process();
        }
    }
}
