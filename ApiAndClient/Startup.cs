using ApiAndClient.Interfaces;
using ApiAndClient.Repositories;
using ApiAndClient.Repositories.SqlRepository;
using ApiAndClient.Services;
using ApiAndClient.Support;
using AspNetCore.RouteAnalyzer;
using CollaborationTestExample.Jwt.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ApiAndClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
       
        public IConfiguration Configuration { get; }

        protected virtual void ConfigureDatabaseContext(IServiceCollection services)
        {
            services.AddSingleton<IApiRepositoryContextFactory, ApiContextFactory>();
        }

        public virtual void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = JwtTokenBuilder.ValidIssuer,
                            ValidAudience = JwtTokenBuilder.ValidAudience,
                            IssuerSigningKey =
                                JwtSecurityKey.Create(JwtTokenBuilder.KeySource)
                        };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireClaim("jobLevel", "Level9"));
                options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDatabaseContext(services);
            ConfigureAuthentication(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddRouteAnalyzer();

            services.AddScoped<ITagProvider, TagProvider>();
            services.AddScoped<IApiRepository, ApiRepository>();
            services.AddScoped<IApiService, ApiService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure
        (
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApplicationLifetime applicationLifetime,
            IRouteAnalyzer routeAnalyzer,
            ILoggerFactory loggerFactory
        )
        {
            //Uncomment the following to view an incoming request and its path
            //app.Use(async (context, next) =>
            //{
            //    // Do work that doesn't write to the Response.
            //    await next.Invoke();
            //    // Do logging or other work that doesn't write to the Response.
            //});

            app.UseAuthentication();
            app.UseMvc();

            // Uncomment the following to view the routes and their actions that the WebApi finds at startup
            //var routeInfo = routeAnalyzer.GetAllRouteInformations();
        }
    }
}
