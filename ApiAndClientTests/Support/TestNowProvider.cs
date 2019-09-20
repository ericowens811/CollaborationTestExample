using ApiAndClient.Interfaces;

namespace ApiAndClientTests.Support
{
    public class TestNowProvider: INowProvider
    {
        public long NextNow { get; set; }
        public long GetNowInMillisecond()
        {
            //return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            return NextNow;
        }
    }
}
