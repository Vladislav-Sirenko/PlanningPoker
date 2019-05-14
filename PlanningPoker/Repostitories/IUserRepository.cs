using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningPoker.Models;

namespace PlanningPoker.Repostitories
{
    public interface IUserRepository
    {
        User AddAsync(User entity);

        void DeleteAsync(string id);

        User GetByNameAsync(string name);

        void UpdateAsync(User entity);
        void UpdateRangeAsync(List<User> entity);
        IEnumerable<User> GetUsersByRoomId(string id);
        User GetUserByConnectionId(string id);
        void DeleteUserFromRoom(string userName);
    }
}
