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

        public MessageService(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings;
        }

        public void AddMessage(Message message)
        {
            using (var db = new ChatContext())
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
        }

        public IEnumerable<Message> GetLastMessages()
        {
            using (var db = new ChatContext())
            {
                var result = db.Messages.OrderByDescending(x => x.Timestamp).Take(this.appSettings.Value.MaxMessages).ToArray();
                return result;
            }
        }
    }
}
