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
            var users = _userService.GetUsers();
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
            var usersVotes = _userService.GetVotes();
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
