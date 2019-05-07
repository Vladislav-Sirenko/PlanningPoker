using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PlanningPoker.Services;

namespace PlanningPoker
{
    internal static class ApplicationDependenciesConfiguration
    {
        public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IUserService, UserService>();

            return serviceCollection;
        }
    }
}
