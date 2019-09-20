using ApiAndClient.Interfaces;
using ApiAndClient.Repositories.SqlRepository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ApiAndClientTests.Support
{
    public class TestApiContextFactory : IApiRepositoryContextFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public TestApiContextFactory
        (
            IConnectionStringProvider connectionStringProvider
        )
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public ApiRepositoryContext Build()
        {
            var connection = new SqliteConnection(_connectionStringProvider.GetConnectionString());
            var context = new ApiRepositoryContext(new DbContextOptionsBuilder<ApiRepositoryContext>().UseSqlite(connection).Options);
            return context;
        }
    }
}
