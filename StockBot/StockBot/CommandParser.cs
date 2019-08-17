using System;
using System.Text.RegularExpressions;

namespace StockBot
{
    public class CommandParser
    {
        public BotCommand Parse(string message)
        {
            var cmd = new BotCommand
            {
                IsCommand = IsCommand(message)
            };
            return cmd;
        }

        private static bool IsCommand(string message)
        {
            var match = Regex.Match(message, @"/\w*=\w*");
            return match.Success;
        }
    }
}
