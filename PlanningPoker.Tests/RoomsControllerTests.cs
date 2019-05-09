using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PlanningPoker.Models;
using Serilog;
using Xunit;

namespace PlanningPoker.Tests
{
    public class RoomsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private WebApplicationFactory<Startup> Factory { get; }
        public RoomsControllerTests(CustomWebApplicationFactory factory)
        {
            Factory = factory;
        }
        [Fact]
        public async Task Post_Rooms()
        {
            var client = Factory.CreateClient();
            var room = new Room() {Id = "1", CreatorName = "2", Name = "Room"};
            string serializedObject = JsonConvert.SerializeObject(room);
            var content = new StringContent(serializedObject);
            await client.PostAsync("api/Rooms", content);
            var response = await client.GetAsync("api/Rooms");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Reset_Votes_For_Room()
        {
            var client = Factory.CreateClient();
            var response = await client.PostAsync("api/Rooms/1/ResetVotes", null);
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        }
        
        [Fact]
        public async  Task Post_Null_Rooms()
        {
            var client = Factory.CreateClient();
            var code = await client.PostAsync("api/Rooms", null);
            Assert.Equal(expected: HttpStatusCode.BadRequest,actual: code.StatusCode);
        }

        [Fact]
        public async Task Get_Users_For_Room()
        {
            var client = Factory.CreateClient();
            var code = await client.GetAsync("api/Rooms/1/users");
            Assert.Equal(expected: HttpStatusCode.OK, actual: code.StatusCode);
        }

        //[Fact]
        //public async Task Get_Votes_For_Room()
        //{
        //    var client = Factory.CreateClient();
        //    var response = await client.PostAsync("api/Rooms/1/votes", null);
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

    }
}
