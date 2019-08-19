namespace ChatroomApi.Service
{
    using System.Linq;
    using ChatroomApi.Data;
    using ChatroomApi.Domain;

    public class UserService : IUserService
    {
        private readonly IChatContextBuilder chatContextBuilder;

        public UserService(IChatContextBuilder chatContextBuilder)
        {
            this.chatContextBuilder = chatContextBuilder;
        }

        public bool AddUser(User user)
        {
            using (var db = this.chatContextBuilder.Create())
            {
                if (db.Users.Any(x => x.Username == user.Username))
                    return false;

                user.Salt = CryptoHelper.GetSalt(10);
                user.Password = CryptoHelper.EncodePassword(user.Password, user.Salt);
                db.Users.Add(user);
                db.SaveChanges();
                return true;
            }
        }

        public bool IsValidUser(string userName, string password)
        {
            using (var db = this.chatContextBuilder.Create())
            {
                var user = db.Users.SingleOrDefault(x => x.Username == userName);

                if (user == null)
                    return false;

                var encodedPass = CryptoHelper.EncodePassword(password, user.Salt);

                return encodedPass == user.Password;
            }
        }
    }
}
