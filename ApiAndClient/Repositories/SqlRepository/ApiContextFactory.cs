using System.Data.SqlClient;
using ApiAndClient.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiAndClient.Repositories.SqlRepository
{
    public class ApiContextFactory : IApiRepositoryContextFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        public ApiContextFactory
        (
            IConnectionStringProvider connectionStringProvider
        )
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public ApiRepositoryContext Build()
        {
            var connection = new SqlConnection(_connectionStringProvider.GetConnectionString());
            var context = new ApiRepositoryContext(new DbContextOptionsBuilder<ApiRepositoryContext>().UseSqlServer(connection).Options);
            return context;
        }
    }
}
