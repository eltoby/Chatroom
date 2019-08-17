using System;

namespace StockBot
{
    public class CommandParser
    {
        public BotCommand Parse(string message)
        {
            var cmd = new BotCommand
            {
                IsCommand = message.StartsWith("/")
            };
            return cmd;
        }
    }
}
