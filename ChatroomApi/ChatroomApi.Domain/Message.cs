using System;
using System.Collections.Generic;
using System.Text;

namespace ChatroomApi.Domain
{
    public class Message
    {
        public int MessageID { get; set; }

        public string Nick { get; set; }

        public string Text { get; set; }

        public double Timestamp { get; set; }
    }
}
