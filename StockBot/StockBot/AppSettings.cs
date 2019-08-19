namespace StockBot
{
    public class AppSettings
    { 
        public string ChatApiUrl { get; set; }

        public bool UseMq { get; set; }

        public string MqUrl { get; set; }

        public string MqUser { get; set; }

        public string MqPass { get; set; }

        public string MqQueue { get; set; }
    }
}
