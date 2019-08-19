namespace ChatroomApi.Models
{
    using System;

    [Serializable]
    public class MessageModel
    {
        public string Nick { get; set; }

        public string Message { get; set; }

        public double Timestamp { get; set; }
    }
}
