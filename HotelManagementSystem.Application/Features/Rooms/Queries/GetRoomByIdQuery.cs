using HotelManagementSystem.Application.Features.RoomTypes.Queries;
using HotelManagementSystem.Domain.Enums;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Rooms.Queries
{
    public record GetRoomByIdQuery(int RoomId) : IRequest<RoomDto?>;

    public record RoomDto(int RoomId, string RoomNumber, int Floor, int Capacity, RoomStatus Status, int RoomTypeId, RoomCategory Category, decimal PricePerNight);

    public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, RoomDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRoomByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<RoomDto?> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            var room = _unitOfWork.Rooms.GetById(request.RoomId);

            if (room is null) return Task.FromResult<RoomDto?>(null);

            var dto = new RoomDto(room.RoomId, room.RoomNumber, room.Floor, room.Capacity, room.Status, room.RoomTypeId, room.RoomType.Category, room.RoomType.PricePerNight);

            return Task.FromResult<RoomDto?>(dto);
        }
    }

}
