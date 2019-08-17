﻿namespace ChatroomApi
{
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        public void SendToAll(string name, string message)
        {
            this.Clients.All.SendAsync("sendToAll", name, message);
        }
    }
}
