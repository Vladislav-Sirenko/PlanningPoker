using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.InMemory;
using PlanningPoker.Context;

namespace PlanningPoker.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public CustomWebApplicationFactory()
        {
            
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder.ConfigureServices(ConfigureDbContexts);

        protected override TestServer CreateServer(IWebHostBuilder builder) =>
            new TestServer(
                builder
                    .UseEnvironment("Development")
                    .UseContentRoot(System.IO.Directory.GetCurrentDirectory())
                    .UseStartup<Startup>());

        private static void ConfigureDbContexts(IServiceCollection services)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            services
                .AddDbContext<PokerContext>(
                    options =>
                    {
                        options.UseInMemoryDatabase(nameof(PokerContext));
                        options.UseInternalServiceProvider(serviceProvider);
                    });
        }
    }
}
