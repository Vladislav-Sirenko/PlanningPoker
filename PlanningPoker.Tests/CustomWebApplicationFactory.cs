using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.InMemory;
using NSubstitute;
using PlanningPoker.Context;

namespace PlanningPoker.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public CustomWebApplicationFactory()
        {
            
        }
        protected override TestServer CreateServer(IWebHostBuilder builder) =>
            new TestServer(
                builder
                    .UseEnvironment("Development")
                    .UseContentRoot(System.IO.Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .ConfigureTestServices(c => ConfigureDbContexts(c)));

        private static void ConfigureDbContexts(IServiceCollection services)
        {
            var optionsProvider = Substitute.For<IDbContextOptionsProvider<PokerContext>>();
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<PokerContext>()
                .UseInMemoryDatabase(nameof(PokerContext))
                .UseInternalServiceProvider(serviceProvider);

            optionsProvider.Options.Returns(builder.Options);

            services.AddSingleton(optionsProvider);
        }
    }
}
