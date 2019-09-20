using ApiAndClient.Interfaces;

namespace ApiAndClientTests.Support
{
    public class TestConnectionStringProvider : IConnectionStringProvider
    {
        private readonly string _connectionString;

        public TestConnectionStringProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
