using ChatroomApi.Domain;

namespace ChatroomApi.Service
{
    public interface IUserService
    {
        bool AddUser(User user);
        bool IsValidUser(string userName, string password);
    }
}