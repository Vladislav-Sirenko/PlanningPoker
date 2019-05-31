using System;
using System.Collections.Generic;
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
            //Arrange
            var client = Factory.CreateClient();
            var room = new Room() { Id = "1", CreatorName = "2", Name = "Room" };
            var content = GetResponseContent(room);
            //Act
            await client.PostAsync("api/Rooms", content);
            var response = await client.GetAsync("api/Rooms");
            //Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Reset_Votes_For_Room()
        {
            //Arrange
            var client = Factory.CreateClient();
            //Act
            var response = await client.DeleteAsync("api/Rooms/1/Votes");
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Post_Null_Rooms()
        {
            //Arrange
            var client = Factory.CreateClient();
            //Act
            var code = await client.PostAsync("api/Rooms", null);
            //Assert
            Assert.Equal(expected: HttpStatusCode.BadRequest, actual: code.StatusCode);
        }

        [Fact]
        public async Task Get_Users_For_Room()
        {
            //Arrange
            var client = Factory.CreateClient();
            //Act
            var code = await client.GetAsync("api/Rooms/1/users");
            //Assert
            Assert.Equal(expected: HttpStatusCode.OK, actual: code.StatusCode);
        }

        [Fact]
        public async Task Add_User_To_Room()
        {
            //Arrange
            var client = Factory.CreateClient();
            var user = new User()
            {
                Id = 1,
                Name = "Room",
                RoomId = "1",
                Vote = 1,
                ConnectionId = "1",
                Email = "123",
                Password = ""
            };
            var content = GetResponseContent(user);

            //Act
            await client.PostAsync("api/Users", content);
            var response = await client.GetAsync("api/Users");

            //Assert
            response.EnsureSuccessStatusCode();
        }

        public StringContent GetResponseContent(object Object)
        {
            string serializedObject = JsonConvert.SerializeObject(Object);
            var content = new StringContent(serializedObject);
            return content;
        }
    }
}
