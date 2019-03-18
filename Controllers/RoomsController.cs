using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlanningPoker.Models;
using PlanningPoker.Services;

namespace PlanningPoker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IUserService _userService;

        public RoomsController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Rooms
        [HttpGet]
        public List<Room> Get()
        {
           return _userService.GetRooms();
        }

        // POST: api/Rooms
        [HttpPost]
        public void Post([FromBody] Room room)
        {
            _userService.AddRoom(room);
        }

        // PUT: api/Rooms/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _userService.DeleteRoom(id);
        }

        [HttpGet("[action]")]
        public List<string> GetUsersByRoom(string name)
        {
            List<string> users = _userService.GetUsersByRoom(name);
            return users;
        }

        [HttpGet("[action]")]
        public Dictionary<string, int> GetVotesByRoom(string name)
        {
            Dictionary<string, int> usersVotes = _userService.GetVotesForRoom(name);
            return usersVotes;
        }

        [HttpPost("[action]")]
        public void UserVote([FromBody] UserVote userVote)
        {
            _userService.AddVote(userVote);

        }

        [HttpPost("[action]")]
        public void ResetVotesByRoom(string name)
        {
            _userService.ResetVote(name);

        }
    }
}
