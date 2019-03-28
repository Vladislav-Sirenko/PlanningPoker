using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PlanningPoker.Models;
using PlanningPoker.Services;

namespace PlanningPoker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IUserService _userService;

        public RoomsController(IUserService userService) => _userService = userService;

        [HttpGet]
        public List<Room> Get() => _userService.GetRooms();

        [HttpGet("{id}/users")]
        public List<string> GetUsersByRoom(string id) => _userService.GetUsersByRoom(id);

        [HttpGet("{id}/Votes")]
        public Dictionary<string, int> GetVotesByRoom(string id) => _userService.GetVotesForRoom(id);

        [HttpPost("{id}/ResetVotes")]
        public void ResetVotesByRoom(string id) => _userService.ResetVote(id);

        [HttpPost("[action]")]
        public void UserVote([FromBody] UserVote userVote) => _userService.AddVote(userVote);
    }
}
