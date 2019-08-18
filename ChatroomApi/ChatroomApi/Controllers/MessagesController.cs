namespace ChatroomApi.Controllers
{
    using ChatroomApi.Domain;
    using ChatroomApi.Models;
    using ChatroomApi.Service;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    [Route("api/Messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IHubContext<ChatHub> hub;
        private readonly IMessageService messageService;

        public MessagesController(IHubContext<ChatHub> hub, IMessageService messageService)
        {
            this.hub = hub;
            this.messageService = messageService;
        }

        [Authorize]
        [Route("SendMessage")]
        public void SendMessage([FromBody]MessageModel message)
        {
            this.hub.Clients.All.SendAsync("sendToAll", message.Name, message.Message, message.Timestamp);

            var messageObj = new Message()
            {
                Nick = message.Name,
                Text = message.Message,
                Timestamp = message.Timestamp
            };

            this.messageService.AddMessage(messageObj);
        }

    }
}
