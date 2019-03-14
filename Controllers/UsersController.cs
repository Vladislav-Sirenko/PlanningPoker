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
        public List<string> GetUsers(string name)
        {
            List<string> users = _userService.GetUsersByRoom(name);
            return users;
        }

        [HttpGet("[action]")]
        public Dictionary<string, int> GetVotes(string name)
        {
            Dictionary<string, int> usersVotes = _userService.GetVotesForRoom(name);
            return usersVotes;
        }

        [HttpPost("[action]")]
        public void Vote([FromBody] UserVote userVote)
        {
            _userService.AddVote(userVote);

        }

        [HttpPost("[action]")]
        public void ResetVotes(string name)
        {
            _userService.ResetVote(name);

        }

        [HttpGet("[action]")]
        public List<Room> GetRooms()
        {
          return _userService.GetRooms();
        }
    }
}
