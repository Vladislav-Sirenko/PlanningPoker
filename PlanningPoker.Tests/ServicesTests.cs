using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using PlanningPoker.Context;
using PlanningPoker.Models;
using PlanningPoker.Repostitories;
using PlanningPoker.Services;
using Xunit;

namespace PlanningPoker.Tests
{
    public class ServicesTests
    {
        private const string One = "1";
        private readonly IUserService _sutForUserService;
        private readonly IUserRepository _userRepository;
        private readonly IRoomsRepository _roomsRepository;
        private readonly IHubContext<LoopyHub> _hubContext;
        private readonly IRoomService _sutForRoomService;
        private readonly Room _room = new Room { Id = One, Name = One, CreatorName = One };

        private readonly User _user = new User
        {
            RoomId = One,
            Name = One,
            Id = 1,
            Vote = 1,
            ConnectionId = One,
            Email = One,
            Password = One
        };

        public ServicesTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _roomsRepository = Substitute.For<IRoomsRepository>();
            _hubContext = Substitute.For<IHubContext<LoopyHub>>();
            var unitOfWork = Substitute.For<IUnitOfWork>();
            _sutForUserService = new UserService(_hubContext, unitOfWork, _roomsRepository, _userRepository);
            _sutForRoomService = new RoomService(_hubContext, unitOfWork, _roomsRepository, _userRepository);
        }


        [Fact]
        public async Task RoomRepository_AddAsync_Should_Calls_And_Hubcontext_Should_Send_AddRoom_When_AddRoom_Called()
        {
            // Act
            await _sutForRoomService.AddRoom(_room);
            // Assert
            await _roomsRepository.Received(1).AddAsync(Arg.Any<Room>());
            await _hubContext.Received(1).Clients.All.SendAsync(Arg.Any<string>());
        }
        [Fact]
        public void UserRepository_AddAsync_Should_Calls_When_AddUser_Called()
        {
            // Act
            _sutForUserService.AddUser(_user);

            // Assert
            _userRepository.Received(1).AddAsync(Arg.Any<User>());
        }
        [Fact]
        public void UserRepository_UpdateAsync_Should_Calls_When_AddUserConnection_Called()
        {
            //Arrange
            _userRepository.GetByNameAsync(One).Returns(_user);
            // Act
            _sutForUserService.AddUserConnection(One, One, One);
            // Assert
            _userRepository.Received(1).GetByNameAsync(Arg.Any<string>());
            _userRepository.Received(1).Update(Arg.Any<User>());
        }
        [Fact]
        public void UserRepository_UpdateAsync_Should_Calls_And_Hubcontext_Should_Send_Vote_When_AddVote_Called()
        {
            // Arrange
            _userRepository.GetByNameAsync(One).Returns(_user);
            _roomsRepository.GetByIdAsync(One).Returns(_room);
            // Act
            _sutForUserService.AddVoteAsync(One, 1);
            //Assert
            _userRepository.Received(1).GetByNameAsync(Arg.Any<string>());
            _roomsRepository.Received(1).GetByIdAsync(Arg.Any<string>());
            _userRepository.Received(1).Update(Arg.Any<User>());
            _hubContext.Received(1).Clients.All.SendAsync(Arg.Any<string>());
        }

        [Fact]
        public void Rooms_Repository_DeleteAsync_Should_call_When_DeleteRoom_Called()
        {
            // Arrange
            _userRepository.GetUsersByRoomId(One).Returns(new List<User>());
            // Act
            _sutForRoomService.DeleteRoomAsync(One);
            //Assert
            _userRepository.Received(1).GetUsersByRoomId(Arg.Any<string>());
            _userRepository.Received(1).UpdateRange(Arg.Any<List<User>>());
            _roomsRepository.Received(1).DeleteAsync(Arg.Any<string>());
        }
        [Fact]
        public void UserRepository_DeleteUserFromRoom_Should_Call_When_DeleteUser_Called()
        {
            // Arrange
            _roomsRepository.GetByIdAsync(One).Returns(_room);
            _userRepository.GetByNameAsync(One).Returns(_user);
            _roomsRepository.GetByNameAsync(One).Returns(_room);
            // Act
            _sutForUserService.DeleteUserFromRoomAsync(One);
            //Assert
            _userRepository.Received(1).DeleteUserFromRoom(Arg.Any<string>());
            _roomsRepository.Received(1).GetByNameAsync(Arg.Any<string>());
            _hubContext.Received(1).Clients.All.SendAsync(Arg.Any<string>());
        }

        [Fact]
        public void GetRoomByUserName_Should_Get_User_And_Room_Correspondent_To_User()
        {
            // Arrange
            _roomsRepository.GetByIdAsync(One).Returns(new Room() { Name = One });
            _userRepository.GetByNameAsync(One).Returns(new User() { RoomId = One });
            // Act
            _sutForUserService.GetRoomByUserName(One);
            //Assert
            _userRepository.Received(1).GetByNameAsync(Arg.Any<string>());
            _roomsRepository.Received(1).GetByIdAsync(Arg.Any<string>());
        }

        [Fact]
        public void GetRooms_Should_Call_RoomRepository_GetRoomsAsync()
        {
            // Arrange
            _roomsRepository.GetRoomsAsync().Returns(new List<Room>());
            // Act
            _sutForRoomService.GetRooms();
            //Assert
            _roomsRepository.Received(1).GetRoomsAsync();
        }

        [Fact]
        public void ResetVote_should_Update_Users_And_Room()
        {
            // Arrange
            _userRepository.GetUsersByRoomId(One).Returns(new List<User>());
            _roomsRepository.GetByIdAsync(One).Returns(new Room());
            // Act
            _sutForUserService.ResetVoteAsync(One);
            //Assert
            _userRepository.Received(1).UpdateRange(Arg.Any<List<User>>());
            _roomsRepository.Received(1).UpdateAsync(Arg.Any<Room>());
        }
    }
}
