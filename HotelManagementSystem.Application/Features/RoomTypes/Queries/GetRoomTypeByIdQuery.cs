using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Enums;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.RoomTypes.Queries
{
    public record GetRoomTypeByIdQuery(int Id) : IRequest<RoomTypeDto?>;

    public record RoomTypeDto(int RoomTypeId, RoomCategory Category, decimal PricePerNight, string? Description);

    public class GetRoomTypeByIdQueryHandler : IRequestHandler<GetRoomTypeByIdQuery, RoomTypeDto?>
    {

        private readonly IUnitOfWork _unitOfWork;

        public GetRoomTypeByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<RoomTypeDto?> Handle(GetRoomTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var roomType = _unitOfWork.RoomTypes.GetById(request.Id);

            if (roomType is null)
            {
                return Task.FromResult<RoomTypeDto?>(null);
            }

            var dto = new RoomTypeDto(roomType.RoomTypeId, roomType.Category, roomType.PricePerNight, roomType.Description);
            return Task.FromResult<RoomTypeDto?>(dto);
        }
    }
}
