namespace ChatroomApi.Data
{
    public class ChatContextBuilder : IChatContextBuilder
    {
        public IChatContext Create()
        {
            return new ChatContext();
        }
    }
}
