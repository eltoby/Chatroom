namespace StockBot
{
    using System;
    using Microsoft.AspNetCore.SignalR.Client;

    public class ChatBotLauncher : IChatBotLauncher
    {
        private readonly IChatBot chatBot;
        private readonly ITimeStampGenerator timeStampGenerator;

        public ChatBotLauncher(IChatBot chatBot, ITimeStampGenerator timeStampGenerator)
        {
            this.chatBot = chatBot;
            this.timeStampGenerator = timeStampGenerator;
        }

        public void Launch()
        {
            var apiUrl = Environment.GetEnvironmentVariable("apiUrl");
            var connection = new HubConnectionBuilder().WithUrl(apiUrl).Build();


            connection.On<string, string, double>("sendToAll", (nick, message, timeStamp) =>
            {
                if (nick != "bot")
                {
                    var result = this.chatBot.Process(message);

                    if (!string.IsNullOrEmpty(result))
                        connection.InvokeAsync("sendToAll", "bot", result, this.timeStampGenerator.GetTimeStamp());
                }
            });

            connection.StartAsync();
        }
    }
}
