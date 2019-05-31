using System.Collections.Generic;
using System.Threading.Tasks;
using PlanningPoker.Models;

namespace PlanningPoker.Services
{
    public interface IRoomService
    {
        Task AddRoom(Room room);
        Task DeleteRoomAsync(string id);
        Task<List<Room>> GetRooms();
    }
}
