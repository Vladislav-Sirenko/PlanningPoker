using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PlanningPoker.Context
{
    internal class DbContextOptionsProvider<TContext> : IDbContextOptionsProvider<TContext>
        where TContext : DbContext
    {
        private readonly DbContextOptionsBuilder<TContext> _contextOptionsBuilder;

        public DbContextOptionsProvider(IConfiguration configuration)
        {
            _contextOptionsBuilder = new DbContextOptionsBuilder<TContext>();
            string connectionString = configuration.GetConnectionString("MyDbConnection");
            _contextOptionsBuilder.UseSqlServer(connectionString);
        }

        public DbContextOptions<TContext> Options => _contextOptionsBuilder.Options;
    }
}
