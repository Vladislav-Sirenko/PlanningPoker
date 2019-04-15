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
            var room = new Room() {id = "1", CreatorId = "2", name = "Room"};
            string serializedObject = JsonConvert.SerializeObject(room);
            var content = new StringContent(serializedObject);
            await client.PostAsync("api/Rooms", content);
            var response = await client.GetAsync("api/Rooms");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_Users_For_Room_Where_Nothing_Exist()
        {
            var client = Factory.CreateClient();
            var room = new Room() { id = "1", CreatorId = "2", name = "Room" };
            string serializedObject = JsonConvert.SerializeObject(room);
            var content = new StringContent(serializedObject);
            await client.PostAsync("api/Rooms", content);
            var response = await client.GetAsync("api/Rooms/1/Users");
            Assert.Equal(HttpStatusCode.InternalServerError,response.StatusCode);
        }
        [Fact]
        public async  Task Post_Null_Rooms()
        {
            var client = Factory.CreateClient();
            var code = await client.PostAsync("api/Rooms", null);
            Assert.Equal(expected: HttpStatusCode.BadRequest,actual: code.StatusCode);
        }
        
    }
}
