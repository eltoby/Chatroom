namespace ChatroomApi
{
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        //[Authorize]
        public void SendToAll(string name, string message, double timestamp)
        {
            this.Clients.All.SendAsync("sendToAll", name, message, timestamp);
        }
    }
}
