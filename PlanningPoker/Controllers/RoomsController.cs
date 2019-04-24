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

        public RoomsController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var rooms = _userService.GetRooms();
            return Ok(rooms);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Room room)
        {
            if (room != null)
            {
                room.CreatorId = _userService.GetConnectionByUserName(room.CreatorId);
                _userService.AddRoom(room);
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
            return  BadRequest();
        }

        [HttpGet("{id}/Votes")]
        public ActionResult GetVotesByRoom(string id) => Ok(_userService.GetVotesForRoom(id));

        [HttpPost("{id}/ResetVotes")]
        public void ResetVotesByRoom(string id) => _userService.ResetVote(id);

        [HttpPost("[action]")]
        public void UserVote([FromBody] UserVote userVote) => _userService.AddVote(userVote);

        [HttpPost("{id}/Roles")]
        public IEnumerable<string> GetRolesList([FromBody] string[] users, string id) => _userService.GetRoles(users, id);
    }
}
