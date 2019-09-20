using System.IO;
using ApiAndClient.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiAndClientTests.Support
{
    public static class TestServerBuilder
    {
        public static TestServer CreateServer<TStartup>(IConnectionStringProvider connectionStringProvider, INowProvider testNowProvider) where TStartup : class
        {
            var webHostBuilder = new WebHostBuilder()
                .UseStartup<TStartup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureServices(services =>
                {
                    services.AddSingleton(connectionStringProvider);
                    services.AddSingleton(testNowProvider);
                })
                //.ConfigureLogging((hostingContext, logging) =>
                //{
                //    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                //    logging.AddConsole();
                //    logging.AddDebug();
                //})
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    var env = builderContext.HostingEnvironment;
                    config.SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) //TODO OPTIONAL IS TRUE FOR NOW
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                });

            var server = new TestServer(webHostBuilder);
            return server;
        }
    }
}
