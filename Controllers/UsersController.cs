using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlanningPoker.Models;
using PlanningPoker.Services;

namespace PlanningPoker.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpGet("[action]")]
        public Dictionary<string, string> GetUsers()
        {
            Dictionary<string,string> users = _userService.GetUsers().ToDictionary(key => key.Key,value=>value.Value);
            return users;
        }

        [HttpPost("[action]")]
        public User AddUser([FromBody]User user)
        {
            _userService.AddUser(user);
            return user;
        }
        [HttpGet("[action]")]
        public Dictionary<string, int> GetVotes()
        {
            Dictionary<string, int> usersVotes = _userService.GetVotes().ToDictionary(key => key.Key, value => value.Value);
            return usersVotes;
        }

        [HttpPost("[action]")]
        public void Vote([FromBody] UserVote userVote)
        {
            _userService.AddVote(userVote);

        }

        [HttpPost("[action]")]
        public void ResetVotes()
        {
            _userService.ResetVote();

        }

        [HttpPost("[action]")]
        public void DeleteUser([FromBody] string user)
        {
            _userService.DeleteUser(user);
        }
    }
}
