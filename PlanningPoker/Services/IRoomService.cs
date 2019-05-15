using System.Collections.Generic;
using System.Threading.Tasks;
using PlanningPoker.Models;

namespace PlanningPoker.Services
{
    public interface IRoomService
    {
        Task<Room> AddRoom(Room room);
        void DeleteRoom(string id);
        Task<List<Room>> GetRooms();
    }
}
