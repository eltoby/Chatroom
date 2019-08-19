namespace ChatroomApi.Domain
{
    public class AppSettings
    {
        public string BaseUrl { get; set; }

        public int TokenExpirationMinutes { get; set; }

        public int MaxMessages { get; set; }

        public string MqUrl { get; set; }

        public string MqUser { get; set; }

        public string MqPass { get; set; }

        public string MqQueue { get; set; }
    }
}
