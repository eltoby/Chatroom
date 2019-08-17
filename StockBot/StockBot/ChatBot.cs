namespace StockBot
{
    public class ChatBot : IChatBot
    {
        private readonly ICommandParser commandParser;

        public ChatBot(ICommandParser commandParser)
        {
            this.commandParser = commandParser;
        }

        public string Process(string message)
        {
            var cmd = this.commandParser.Parse(message);

            if (!cmd.IsCommand)
                return string.Empty;

            return "Message Processed!!!";
        }
    }
}
