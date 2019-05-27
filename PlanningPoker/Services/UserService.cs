using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Models;
using PlanningPoker.Repostitories;

namespace PlanningPoker.Services
{
    public class UserService : IUserService
    {
        private readonly IHubContext<LoopyHub> _hubContext;
        private readonly IRoomsRepository _roomsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IHubContext<LoopyHub> hubContext, IUnitOfWork unitOfWork, IRoomsRepository repository, IUserRepository userRepository)
        {
            _hubContext = hubContext;
            _roomsRepository = repository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task AddVoteAsync(string name, int vote)
        {
            var user = _userRepository.GetByNameAsync(name);
            user.Vote = vote;
            _userRepository.Update(user);
            var room = _roomsRepository.GetById(user.RoomId);
            _unitOfWork.Complete();
            await _hubContext.Clients.Group(room.Id).SendAsync("Vote", user.Name);
        }

        public List<User> GetUsersByRoom(string id)
        {
            return _userRepository.GetUsersByRoomId(id).ToList();
        }

        public async Task GetVotesForRoomAsync(string id)
        {
            if (id == null)
                return;
            var room = _roomsRepository.GetById(id);
            room.SessionEnded = true;
            _roomsRepository.Update(room);
            var users = _userRepository.GetUsersByRoomId(id).ToList();
            Dictionary<string, int> votes = new Dictionary<string, int>();
            foreach (var user in users)
            {
                if (user.Vote != null) votes.Add(user.Name, user.Vote.Value);
            }

            _unitOfWork.Complete();
            await _hubContext.Clients.Group(room.Id).SendAsync("GetVotes", votes);
        }


        public async Task ResetVoteAsync(string id)
        {
            var room = _roomsRepository.GetById(id);
            room.SessionEnded = false;
            _roomsRepository.Update(room);
            var userlist = _userRepository.GetUsersByRoomId(id).ToList();
            foreach (var user in userlist)
            {
                user.Vote = null;
            }
            _userRepository.UpdateRange(userlist);
            _unitOfWork.Complete();
            await _hubContext.Clients.Group(id).SendAsync("ResetVotes");
        }

        public User AddUserConnection(string id, string roomId, string userName)
        {
            var user = _userRepository.GetByNameAsync(userName);
            if (user != null)
            {
                user.ConnectionId = id;
                user.RoomId = roomId;
                _userRepository.Update(user);
            }
            _unitOfWork.Complete();
            return user;
        }

        public bool CheckSessionState(string id)
        {
            return _roomsRepository.GetById(id).SessionEnded;
        }

        public string GetRoomByUserName(string userName)
        {
            var user = _userRepository.GetByNameAsync(userName);
            var room = _roomsRepository.GetById(user.RoomId);
            return room.Name;
        }

        public User AddUser(User user)
        {
            var entity = _userRepository.AddAsync(user);
            _unitOfWork.Complete();
            return entity;
        }

        public User GetUserByConnectionId(string id)
        {
            return _userRepository.GetUserByConnectionId(id);
        }
        public async Task DeleteUserFromRoomAsync(string userName)
        {
            var roomName =  GetRoomByUserName(userName);
            _userRepository.DeleteUserFromRoom(userName);
            var roomId = _roomsRepository.GetByName(roomName).Id;
            _unitOfWork.Complete();
            await _hubContext.Clients.Group(roomId).SendAsync("Disconnect", userName);
        }
    }
}
