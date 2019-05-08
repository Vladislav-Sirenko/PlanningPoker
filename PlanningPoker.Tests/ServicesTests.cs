using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NSubstitute;
using PlanningPoker.Context;
using PlanningPoker.Models;
using PlanningPoker.Repostitories;
using PlanningPoker.Services;
using Xunit;

namespace PlanningPoker.Tests
{
    public class ServicesTests
    {
        private readonly IUserService _sut;
        private readonly Room _room = new Room() { Id = "123", Name = "Room1", CreatorName = "123456" };
        private readonly IUserRepository _userRepository;
        private readonly IRoomsRepository _roomsRepository;
        public ServicesTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _roomsRepository = Substitute.For<IRoomsRepository>();
            var hubContext = Substitute.For<IHubContext<LoopyHub>>();
            _sut = new UserService(hubContext, _roomsRepository, _userRepository);
        }


        [Fact]
        public async Task GetRooms_Shlould_Return_Romm_If_Room_Was_Added()
        {
            // Arrange
            // Act
            
            await _sut.AddRoom(_room);
            var rooms = await _sut.GetRooms();
            // Assert
            Assert.Equal(rooms.FirstOrDefault(), _room);
        }
        //[Fact]
        //public void GetUserByConnection_Should_return_UserName_If_User_was_added()
        //{
        //    // Arrange
        //    // Act
        //    _sut.AddUserConnection(_userConnection.ConnectionId, _userConnection.Name);
        //    var result = _sut.GetUserByConnection(_userConnection.ConnectionId);
        //    // Assert
        //    Assert.Equal(result, _userConnection.Name);
        //}
        //[Fact]
        //public void GetVotesForRoom_should_return_UserVote_If_Votes_Were_added()
        //{
        //    // Arrange
        //    // Act
        //    _sut.AddUserConnection(_userConnection.ConnectionId, _userConnection.Name);
        //    _sut.AddRoom(_room);
        //    _sut.AddVote(_userVote);
        //    _sut.AddUserToGroup(new UserRoom() { Name = _room.name, ConnectionId = _userConnection.ConnectionId });
        //    var result = _sut.GetVotesForRoom(_room.id);
        //    // Assert
        //    Assert.Equal(result.First().Key, _userVote.UserName);
        //    Assert.Equal(result.First().Value, _userVote.Vote);
        //}
        //[Fact]
        //public void GetRoles()
        //{
        //    // Arrange
        //    // Act
        //    _sut.AddUserConnection(_userConnection.ConnectionId, _userConnection.Name);
        //    _sut.AddUserConnection("234", "Vasya");
        //    _sut.AddRoom(_room);
        //    _sut.AddUserToGroup(new UserRoom() { Name = _room.name, ConnectionId = _userConnection.ConnectionId });
        //    _sut.AddUserToGroup(new UserRoom() { Name = _room.name, ConnectionId = "234" });
        //    string[] users = { "User", "Vasya" };
        //    var result = _sut.GetRoles(users, _room.id);
        //    // Assert
        //    Assert.Equal("Admin", result.First());
        //    Assert.Equal("Guest", result.Last());
        //}
        //[Fact]
        //public void ResetVotes()
        //{
        //    // Arrange
        //    // Act
        //    _sut.AddUserConnection(_userConnection.ConnectionId, _userConnection.Name);
        //    _sut.AddUserConnection("234", "Vasya");
        //    _sut.AddRoom(_room);
        //    _sut.AddUserToGroup(new UserRoom() { Name = _room.name, ConnectionId = _userConnection.ConnectionId });
        //    _sut.AddUserToGroup(new UserRoom() { Name = _room.name, ConnectionId = "234" });
        //    //  _sut.ResetVote(_room.id);
        //    var list = new Dictionary<string, int>();
        //    // Assert
        //    Assert.Equal(list, _sut.GetVotesForRoom(_room.id));
        //    //Assert.Equal("Guest", result.Last());
        //}
        //[Fact]
        //public void DeleteUser()
        //{
        //    // Arrange
        //    // Act
        //    _sut.AddUserConnection(_userConnection.ConnectionId, _userConnection.Name);
        //    _sut.AddUserConnection("234", "Vasya");
        //    _sut.AddRoom(_room);
        //    _sut.AddUserToGroup(new UserRoom() { Name = _room.name, ConnectionId = _userConnection.ConnectionId });
        //    _sut.AddUserToGroup(new UserRoom() { Name = _room.name, ConnectionId = "234" });
        //    _sut.DeleteUser(_userConnection.ConnectionId);
        //    var result = _sut.GetUsersByRoom(_room.id);
        //    var list = new List<string>() { "Vasya" };
        //    // Assert
        //    Assert.Equal(result, list);
        //}
    }
}
