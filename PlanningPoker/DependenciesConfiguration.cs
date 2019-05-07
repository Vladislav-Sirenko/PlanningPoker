using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PlanningPoker.Context;
using PlanningPoker.Repostitories;

namespace PlanningPoker
{
    public static class DalDependenciesConfiguration
    {
        public static IServiceCollection RegisterDalDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<PokerContext>();
            serviceCollection.AddTransient<IDbContextOptionsProvider<PokerContext>, DbContextOptionsProvider<PokerContext>>();
            serviceCollection.AddTransient<IUserRepository, UserRepository>();
            serviceCollection.AddTransient<IRoomsRepository, RoomRepository>();
            return serviceCollection;
        }
    }
}
