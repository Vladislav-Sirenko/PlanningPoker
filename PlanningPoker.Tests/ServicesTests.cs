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
        private readonly IUserRepository _userRepository;
        private readonly IRoomsRepository _roomsRepository;
        private readonly IHubContext<LoopyHub> hubContext;
        public ServicesTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _roomsRepository = Substitute.For<IRoomsRepository>();
            hubContext = Substitute.For<IHubContext<LoopyHub>>();
            _sut = new UserService(hubContext, _roomsRepository, _userRepository);
        }


        [Fact]
        public async Task GetRooms_Shlould_Return_Romm_If_Room_Was_Added()
        {
            // Act
            await _sut.AddRoom(new Room());
            // Assert
            Assert.Single(_roomsRepository.ReceivedCalls());
            Assert.Single(hubContext.ReceivedCalls());
        }
        [Fact]
        public void GetUserByConnection_Should_return_UserName_If_User_was_added()
        {
            // Act
            _sut.AddUser(new User());

            // Assert
            Assert.Single(_userRepository.ReceivedCalls());
        }
        [Fact]
        public void GetVotesForRoom_should_return_UserVote_If_Votes_Were_added()
        {
            // Arrange
            // Act
            _sut.GetRoles(new string[2], "1");
            // Assert
            Assert.Single(hubContext.ReceivedCalls());
        }
        [Fact]
        public void AddUserConnection_when_User_Doesnt_exist_in_db()
        {
            // Act
            _sut.AddUserConnection("1", "1", "1");
            Assert.Single(_userRepository.ReceivedCalls());
        }
        [Fact]
        public void AddUserConnection_when_User_exist_in_db()
        {
            // Act
            _userRepository.GetByNameAsync("1").Returns(new User());
            _sut.AddUserConnection("1", "1", "1");
            Assert.Equal(2,_userRepository.ReceivedCalls().Count());
        }
        [Fact]
        public void AddVote()
        {
            _userRepository.GetByNameAsync("1").Returns(new User(){RoomId = "1"});
            _roomsRepository.GetByIdAsync("1").Returns(new Room());
            _sut.AddVote("1",1);
            Assert.Equal(2,_userRepository.ReceivedCalls().Count());
            Assert.Single(_roomsRepository.ReceivedCalls());
            Assert.Single(hubContext.ReceivedCalls());
        }
        [Fact]
        public void DeleteUser()
        {
            // Arrange
            // Act
            _sut.DeleteRoom("1");

        }
    }
}
