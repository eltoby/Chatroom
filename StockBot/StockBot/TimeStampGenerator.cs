namespace StockBot
{
    using System;

    public class TimeStampGenerator : ITimeStampGenerator
    {
        public double GetTimeStamp()
        {
            return DateTime.UtcNow
               .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
               .TotalMilliseconds;
        }
    }
}
