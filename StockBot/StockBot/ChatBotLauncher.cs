namespace StockBot
{
    using System;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Options;

    public class ChatBotLauncher : IChatBotLauncher
    {
        private readonly IChatBot chatBot;
        private readonly ITimeStampGenerator timeStampGenerator;
        private readonly IOptions<AppSettings> appSettings;

        public ChatBotLauncher(IChatBot chatBot, ITimeStampGenerator timeStampGenerator, IOptions<AppSettings> appSettings)
        {
            this.chatBot = chatBot;
            this.timeStampGenerator = timeStampGenerator;
            this.appSettings = appSettings;
        }

        public void Launch()
        {
            var apiUrl = this.appSettings.Value.ChatApiUrl;
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
