namespace StockBot
{
    using System;

    [Serializable]
    public class ChatMessage
    {
        public string Nick { get; set; }

        public double TimeStamp { get; set; }

        public string Message { get; set; }
    }
}
