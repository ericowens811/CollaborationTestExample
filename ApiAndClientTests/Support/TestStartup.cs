using ApiAndClient;
using ApiAndClient.Repositories.SqlRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiAndClientTests.Support
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureDatabaseContext(IServiceCollection services)
        {
            services.AddSingleton<IApiRepositoryContextFactory, TestApiContextFactory>();
        }
    }
}
