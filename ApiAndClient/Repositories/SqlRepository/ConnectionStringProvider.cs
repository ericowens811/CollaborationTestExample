using ApiAndClient.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ApiAndClient.Repositories.SqlRepository
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly string _connectionString;

        public ConnectionStringProvider(IConfiguration configuration, string section)
        {
            _connectionString = configuration.GetConnectionString(section);
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
