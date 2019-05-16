using System;
using System.Threading.Tasks;

namespace PlanningPoker
{
    public interface IUnitOfWork 
    {
        Task CompleteAsync();
    }
}