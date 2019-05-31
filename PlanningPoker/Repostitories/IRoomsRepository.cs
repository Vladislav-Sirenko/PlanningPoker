using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningPoker.Models;

namespace PlanningPoker.Repostitories
{
    public interface IRoomsRepository
    {
        Task AddAsync(Room entity);

        void Delete(string id);

        Room GetById(string id);

        void Update(Room entity);
        Task<List<Room>> GetRoomsAsync();
        Room GetByName(string name);
    }
}
