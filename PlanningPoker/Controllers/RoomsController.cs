using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PlanningPoker.Context;
using PlanningPoker.Models;
using PlanningPoker.Services;
using Serilog;

namespace PlanningPoker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IUserService _userService;

        public RoomsController(IUserService userService, PokerContext context)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var rooms = await _userService.GetRooms();
            return Ok(rooms);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Room room)
        {
            if (room != null)
            {
                await _userService.AddRoom(room);
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            if (id != null)
            {
                _userService.DeleteRoom(id);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("{id}/users")]
        public ActionResult GetUsersByRoom(string id)
        {
            if (id != null)
            {
                return Ok(_userService.GetUsersByRoom(id));
            }
            return BadRequest();
        }

        [HttpPost("{id}/Votes")]
        public void GetVotesByRoom(string id) => _userService.GetVotesForRoom(id);

        [HttpPost("{id}/ResetVotes")]
        public void ResetVotesByRoom(string id) => _userService.ResetVote(id);

        [HttpPost("{id}/UserVote")]
        public void UserVote([FromRoute] string id, [FromBody] int vote) => _userService.AddVote(id, vote);

        [HttpPost("{id}/Roles")]
        public IEnumerable<string> GetRolesList([FromBody] string[] users, string id) => _userService.GetRoles(users, id);

        [HttpPost("[action]")]
        public void AddUser([FromBody] User user) => _userService.AddUser(user);

        [HttpPost("[action]")]
        public void DeleteUserFromRoom(string user)
        {
            _userService.DeleteUserFromRoom(user);
        }
    }
}
