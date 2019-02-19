using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlanningPoker.Models;

namespace PlanningPoker.Services
{
    public interface IUserService
    {
        User AddUser(User user);
        Dictionary<string, string> GetUsers();
    }
}
