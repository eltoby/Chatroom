namespace ChatroomApi.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using ChatroomApi.Data;
    using ChatroomApi.Domain;
    using Microsoft.Extensions.Options;

    public class MessageService : IMessageService
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IChatContextBuilder chatContextBuilder;

        public MessageService(IOptions<AppSettings> appSettings, IChatContextBuilder chatContextBuilder)
        {
            this.appSettings = appSettings;
            this.chatContextBuilder = chatContextBuilder;
        }

        public void AddMessage(Message message)
        {
            using (var db = this.chatContextBuilder.Create())
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
        }

        public IEnumerable<Message> GetLastMessages()
        {
            using (var db = this.chatContextBuilder.Create())
            {
                var result = db.Messages.OrderByDescending(x => x.Timestamp).Take(this.appSettings.Value.MaxMessages).ToArray();
                return result;
            }
        }
    }
}
