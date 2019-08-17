namespace StockBot
{
    using System;
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
                cmd.CommandKey = this.GetCommandKey(message);
            }

            return cmd;
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
