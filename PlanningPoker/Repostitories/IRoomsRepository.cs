using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningPoker.Models;

namespace PlanningPoker.Repostitories
{
    public interface IRoomsRepository
    {
        Task<Room> AddAsync(Room entity);

        void DeleteAsync(string id);

        Room GetByIdAsync(string id);

        void UpdateAsync(Room entity);
        Task<List<Room>> GetRoomsAsync();
        Room GetByNameAsync(string name);
    }
}
