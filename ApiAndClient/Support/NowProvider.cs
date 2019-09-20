using System;
using ApiAndClient.Interfaces;

namespace ApiAndClient.Support
{
    public class NowProvider : INowProvider
    {
        public long GetNowInMillisecond()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
