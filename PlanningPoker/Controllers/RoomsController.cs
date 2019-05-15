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
        private readonly IRoomService _roomService;

        public RoomsController(IUserService userService, PokerContext context,IRoomService roomService)
        {
            _userService = userService;
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var rooms = await _roomService.GetRooms();
            return Ok(rooms);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Room room)
        {
            if (room != null)
            {
                await _roomService.AddRoom(room);
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            if (id != null)
            {
                _roomService.DeleteRoom(id);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("{id}/Users")]
        public ActionResult<List<User>> GetUsersByRoom(string id)
        {
            if (id != null)
            {
                return Ok(_userService.GetUsersByRoom(id));
            }
            return BadRequest();
        }

        [HttpGet("{id}/Votes")]
        public ActionResult GetVotesByRoom(string id)
        {
            _userService.GetVotesForRoom(id);
            return Ok();
        }

        [HttpDelete("{id}/Votes")]
        public ActionResult ResetVotesByRoom(string id)
        {
            _userService.ResetVote(id);
            return Ok();
        }

        [HttpPost("{id}/Votes")]
        public ActionResult UserVote([FromRoute] string id, [FromBody] int vote)
        {
            _userService.AddVote(id, vote);
            return Ok();
        }

    }
}
