using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Enums;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Rooms.Commands
{
    public record CreateRoomCommand(string RoomNumber, int Floor, int Capacity, RoomStatus Status, int RoomTypeId) : IRequest<int>;

    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var roomType = _unitOfWork.RoomTypes.GetById(request.RoomTypeId);
            if(roomType is null)
            {
                throw new Exception("Tip sobe ne postoji");
            }

            var room = new Room
            {
                RoomNumber = request.RoomNumber,
                Floor = request.Floor,
                Capacity = request.Capacity,
                Status = request.Status,
                RoomTypeId = request.RoomTypeId
            };

            _unitOfWork.Rooms.Add(room);
            _unitOfWork.SaveChanges();

            return Task.FromResult(room.RoomId);
        }
    }

}
