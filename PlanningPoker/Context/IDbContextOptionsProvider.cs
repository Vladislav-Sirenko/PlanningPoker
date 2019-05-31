using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PlanningPoker.Context
{
    public interface IDbContextOptionsProvider<TContext>
        where TContext : DbContext
    {
        DbContextOptions<TContext> Options { get; }
    }
}
