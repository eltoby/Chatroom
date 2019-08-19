namespace ChatroomApi
{
    using System.Text;
    using ChatroomApi.Domain;
    using ChatroomApi.Models;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class MessageQueueHandler : IMessageQueueHandler
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IHubContext<ChatHub> hub;

        public MessageQueueHandler(IOptions<AppSettings> appSettings, IHubContext<ChatHub> hub)
        {
            this.appSettings = appSettings;
            this.hub = hub;
        }

        public void Launch()
        {
            var connFactory = new ConnectionFactory()
            {
                HostName = this.appSettings.Value.MqUrl,
                UserName = this.appSettings.Value.MqUser,
                Password = this.appSettings.Value.MqPass
            };


            var queue = this.appSettings.Value.MqQueue;

            var conn = connFactory.CreateConnection();
            var channel = conn.CreateModel();
            channel.QueueDeclare(queue, false, false, false, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += this.Consumer_Received;
            var result = channel.BasicConsume(queue: queue,
                         autoAck: true,
                         consumer: consumer);
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);

            var chatMessage = JsonConvert.DeserializeObject<MessageModel>(message);

            this.hub.Clients.All.SendAsync("sendToAll", chatMessage.Nick, chatMessage.Message, chatMessage.Timestamp);
        }
    }
}
