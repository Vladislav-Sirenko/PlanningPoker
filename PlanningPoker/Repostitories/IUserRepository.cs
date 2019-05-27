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

        void Delete(string id);

        User GetByNameAsync(string name);

        void Update(User entity);
        void UpdateRange(IEnumerable<User> entity);
        IReadOnlyCollection<User> GetUsersByRoomId(string id);
        User GetUserByConnectionId(string id);
        void DeleteUserFromRoom(string userName);
    }
}
