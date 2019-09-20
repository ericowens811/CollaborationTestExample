using ApiAndClient.Repositories.SqlRepository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ApiAndClientTests.Support
{
    public static class TestContextProvider
    {
        public static ApiRepositoryContext GetContext(SqliteConnection connection)
        {
            connection.Open();
            var options = new DbContextOptionsBuilder<ApiRepositoryContext>()
                .UseSqlite(connection)
                .Options;
            return new ApiRepositoryContext(options);
        }
    }
}
