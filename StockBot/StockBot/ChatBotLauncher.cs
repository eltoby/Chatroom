namespace StockBot
{
    using System.Text;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using RabbitMQ.Client;

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
                        if (this.appSettings.Value.UseMq)
                            this.SendMqMessage(result);
                        else
                            connection.InvokeAsync("sendToAll", "bot", result, this.timeStampGenerator.GetTimeStamp());
                }
            });

            connection.StartAsync();
        }

        private void SendMqMessage(string message)
        {
            var connFactory = new ConnectionFactory()
            {
                HostName = this.appSettings.Value.MqUrl,
                UserName = this.appSettings.Value.MqUser,
                Password = this.appSettings.Value.MqPass
            };


            var queue = this.appSettings.Value.MqQueue;

            using (var conn = connFactory.CreateConnection())
            {
                var channel = conn.CreateModel();
                channel.QueueDeclare(queue, false, false, false, null);

                var chatMessage = new ChatMessage()
                {
                    Nick = "bot",
                    Message = message,
                    TimeStamp = this.timeStampGenerator.GetTimeStamp()
                };


                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(chatMessage));

                channel.ExchangeDeclare("ChatroomExchange", "direct", false, false, null);
                channel.QueueBind(queue, "ChatroomExchange", "rkey", null);
                channel.BasicPublish("ChatroomExchange", "rkey", body: body);
            }
        }
    }
}
