using ChatroomApi.Domain;

namespace ChatroomApi.Service
{
    public interface IMessageService
    {
        void AddMessage(Message message);
    }
}