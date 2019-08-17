namespace StockBot
{
    using System.Text.RegularExpressions;

    public class CommandParser : ICommandParser
    {
        public BotCommand Parse(string message)
        {
            var cmd = new BotCommand
            {
                IsCommand = IsCommand(message)
            };

            if (cmd.IsCommand)
            {
                cmd.Key = this.GetCommandKey(message);
                cmd.Value = this.GetCommandValue(message);
            }
            return cmd;
        }

        private string GetCommandValue(string message)
        {
            return message.Substring(message.IndexOf("=") + 1);
        }

        private string GetCommandKey(string message)
        {
            return message.Substring(1, message.IndexOf("=") - 1);
        }

        private static bool IsCommand(string message)
        {
            var match = Regex.Match(message, @"/\w+=\w+");
            return match.Success;
        }
    }
}
