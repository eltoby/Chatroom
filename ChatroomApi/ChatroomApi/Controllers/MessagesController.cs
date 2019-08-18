namespace ChatroomApi.Controllers
{
    using ChatroomApi.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    [Route("api/Messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IHubContext<ChatHub> hub;

        public MessagesController(IHubContext<ChatHub> hub)
        {
            this.hub = hub;
        }

        [Authorize]
        [Route("SendMessage")]
        public void SendMessage([FromBody]MessageModel message)
        {
            this.hub.Clients.All.SendAsync("sendToAll", message.Name, message.Message, message.Timestamp);
        }

    }
}
