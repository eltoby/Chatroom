namespace ChatroomApi.Service
{
    using ChatroomApi.Data;
    using ChatroomApi.Domain;

    public class MessageService : IMessageService
    {
        public void AddMessage(Message message)
        {
            using (var db = new ChatContext())
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }
        }
    }
}
