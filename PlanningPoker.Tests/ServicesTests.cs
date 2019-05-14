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
        private readonly IUserService _sut;
        private readonly IUserRepository _userRepository;
        private readonly IRoomsRepository _roomsRepository;
        private readonly IHubContext<LoopyHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        private const string One = "1";
        private readonly Room _room = new Room() { Id = One, Name = One, CreatorName = One };

        private readonly User _user = new User()
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
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _sut = new UserService(_hubContext, _unitOfWork, _roomsRepository, _userRepository);
        }


        [Fact]
        public async Task GetRooms_Shlould_Return_Romm_If_Room_Was_Added()
        {
            // Act
            await _sut.AddRoom(_room);
            // Assert
            Received.InOrder(async () => await _roomsRepository.AddAsync(_room));
            Received.InOrder(async () => await _hubContext.Clients.All.SendAsync("AddRoom"));
        }
        [Fact]
        public void GetUserByConnection_Should_return_UserName_If_User_was_added()
        {
            // Act
            _sut.AddUser(_user);

            // Assert
            Received.InOrder(() => _userRepository.AddAsync(_user));
            Assert.Single(_userRepository.ReceivedCalls());
        }
        [Fact]
        public void AddUserConnection_when_User_Doesnt_exist_in_db()
        {
            // Act
            _sut.AddUserConnection(One, One, One);
            // Assert
            Assert.Single(_userRepository.ReceivedCalls());
        }
        [Fact]
        public void AddUserConnection_when_User_exist_in_db()
        {
            // Arrange
            _userRepository.GetByNameAsync(One).Returns(new User());
            // Act
            _sut.AddUserConnection(One, One, One);
            //Assert
            Assert.Equal(2, _userRepository.ReceivedCalls().Count());
        }

        [Fact]
        public void AddVote()
        {
            // Arrange
            _userRepository.GetByNameAsync(One).Returns(new User() { RoomId = One });
            _roomsRepository.GetByIdAsync(One).Returns(new Room());
            // Act
            _sut.AddVote(One, 1);
            //Assert
            Assert.Equal(2, _userRepository.ReceivedCalls().Count());
            Assert.Single(_roomsRepository.ReceivedCalls());
            Assert.Single(_hubContext.ReceivedCalls());
        }

        [Fact]
        public void DeleteRoom()
        {
            // Arrange
            _userRepository.GetUsersByRoomId(One).Returns(new List<User>());
            // Act
            _sut.DeleteRoom(One);
            //Assert
            Assert.Equal(2, _userRepository.ReceivedCalls().Count());
            Assert.Single(_roomsRepository.ReceivedCalls());
            Assert.Single(_hubContext.ReceivedCalls());

        }
        [Fact]
        public void DeleteUser()
        {
            // Arrange
            _roomsRepository.GetByIdAsync(One).Returns(new Room() { Name = One });
            _userRepository.GetByNameAsync(One).Returns(new User() { RoomId = One });
            // Act
            _sut.DeleteUserFromRoom(One);
            //Assert
            Assert.Equal(2, _userRepository.ReceivedCalls().Count());
            Assert.Single(_hubContext.ReceivedCalls());

        }

        [Fact]
        public void GetRoomByUserName()
        {
            // Arrange
            _roomsRepository.GetByIdAsync(One).Returns(new Room() { Name = One });
            _userRepository.GetByNameAsync(One).Returns(new User() { RoomId = One });
            // Act
            _sut.GetRoomByUserName(One);
            //Assert
            Assert.Single(_userRepository.ReceivedCalls());
        }

        [Fact]
        public void GetRooms()
        {
            // Arrange
            _roomsRepository.GetRoomsAsync().Returns(new List<Room>());
            // Act
            _sut.GetRooms();
            //Assert
            Assert.Single(_roomsRepository.ReceivedCalls());
        }

        [Fact]
        public void ResetVote()
        {
            // Arrange
            _userRepository.GetUsersByRoomId(One).Returns(new List<User>());
            // Act
            _sut.ResetVote(One);
            //Assert
            Assert.Equal(2, _userRepository.ReceivedCalls().Count());
        }
    }
}
