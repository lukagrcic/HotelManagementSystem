using HotelManagementSystem.Domain.Enums;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Rooms.Commands
{
    public record UpdateRoomCommand(string RoomNumber, int Floor, int Capacity, RoomStatus Status, int RoomTypeId) : IRequest<bool>
    {
        public int RoomId { get; set; }
    }

    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = _unitOfWork.Rooms.GetById(request.RoomId);
            if (room is null) return Task.FromResult(false);

            var roomType = _unitOfWork.RoomTypes.GetById(request.RoomTypeId);
            if (roomType is null) return Task.FromResult(false);

            room.RoomNumber = request.RoomNumber;
            room.Floor = request.Floor;
            room.Capacity = request.Capacity;
            room.Status = request.Status;
            room.RoomTypeId = request.RoomTypeId;

            _unitOfWork.Rooms.Update(room);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);

        }
    }
}
