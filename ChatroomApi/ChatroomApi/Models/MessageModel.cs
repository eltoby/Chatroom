using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatroomApi.Models
{
    public class MessageModel
    {
        public string Name { get; set; }

        public string Message { get; set; }

        public double Timestamp { get; set; }
    }
}
