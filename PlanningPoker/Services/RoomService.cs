using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using PlanningPoker.Models;
using PlanningPoker.Repostitories;

namespace PlanningPoker.Services
{
    public class RoomService: IRoomService
    {
        private readonly IHubContext<LoopyHub> _hubContext;
        private readonly IRoomsRepository _roomsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoomService(IHubContext<LoopyHub> hubContext, IUnitOfWork unitOfWork, IRoomsRepository repository, IUserRepository userRepository)
        {
            _hubContext = hubContext;
            _roomsRepository = repository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async void DeleteRoom(string id)
        {
            var users = _userRepository.GetUsersByRoomId(id);
            foreach (var user in users)
            {
                user.RoomId = null;
            }
            _userRepository.UpdateRangeAsync(users.ToList());
            _roomsRepository.DeleteAsync(id);
            _unitOfWork.Complete();
            await _hubContext.Clients.All.SendAsync("DeleteRoom");
        }
        public async Task<List<Room>> GetRooms()
        {
            return await _roomsRepository.GetRoomsAsync();
        }
        public async Task<Room> AddRoom(Room room)
        {
            var entity = await _roomsRepository.AddAsync(room);
            _unitOfWork.Complete();
            await _hubContext.Clients.All.SendAsync("AddRoom");
            return entity;
        }

    }
}
