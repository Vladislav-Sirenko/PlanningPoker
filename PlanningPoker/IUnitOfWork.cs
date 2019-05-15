using System;
using System.Threading.Tasks;

namespace PlanningPoker
{
    public interface IUnitOfWork : IDisposable
    {
        void Complete();
    }
}