using System.Collections.Generic;
using ChatroomApi.Domain;

namespace ChatroomApi.Service
{
    public interface IMessageService
    {
        void AddMessage(Message message);

        IEnumerable<Message> GetLastMessages();
    }
}