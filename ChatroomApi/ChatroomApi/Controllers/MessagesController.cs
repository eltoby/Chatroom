namespace ChatroomApi.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using ChatroomApi.Domain;
    using ChatroomApi.Models;
    using ChatroomApi.Service;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Options;

    [Route("api/Messages")]
    [ApiController]
    [Produces("application/json")]
    public class MessagesController : ControllerBase
    {
        private readonly IHubContext<ChatHub> hub;
        private readonly IMessageService messageService;
        private readonly IOptions<AppSettings> appSettings;

        public MessagesController(IHubContext<ChatHub> hub, IMessageService messageService)
        {
            this.hub = hub;
            this.messageService = messageService;
        }

        [Authorize, HttpPost, Route("SendMessage")]
        public void SendMessage([FromBody]MessageModel message)
        {
            this.hub.Clients.All.SendAsync("sendToAll", message.Nick, message.Message, message.Timestamp);

            var messageObj = new Message()
            {
                Nick = message.Nick,
                Text = message.Message,
                Timestamp = message.Timestamp
            };

            if (!message.Message.StartsWith("/"))
                this.messageService.AddMessage(messageObj);
        }

        [Route("GetLastMessages")]
        public IEnumerable<MessageModel> GetLastMessages()
        {
            var messages = this.messageService.GetLastMessages();
            var result = messages.Select(x => new MessageModel { Nick = x.Nick, Message = x.Text, Timestamp = x.Timestamp });
            return result;
        }

    }
}
