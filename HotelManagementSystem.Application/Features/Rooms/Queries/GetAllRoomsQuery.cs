using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Rooms.Queries
{
    public record GetAllRoomsQuery() : IRequest<List<RoomDto>>;

    public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, List<RoomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllRoomsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<RoomDto>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {
            var rooms = _unitOfWork.Rooms.GetAll()
                .Select(r => new RoomDto(r.RoomId, r.RoomNumber, r.Floor, r.Capacity, r.Status, r.RoomTypeId, r.RoomType.Category, r.RoomType.PricePerNight))
                .ToList();
            return Task.FromResult(rooms);
        }
    }
}
