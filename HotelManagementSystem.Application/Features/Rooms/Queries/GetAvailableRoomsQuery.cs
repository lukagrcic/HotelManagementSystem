using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Rooms.Queries
{
    public record GetAvailableRoomsQuery(DateTime DateFrom, DateTime DateTo) : IRequest<List<RoomDto>>;

    public class GetAvailableRoomsQueryHandler : IRequestHandler<GetAvailableRoomsQuery, List<RoomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAvailableRoomsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<RoomDto>> Handle(GetAvailableRoomsQuery request, CancellationToken cancellationToken)
        {
            var rooms = _unitOfWork.Rooms.GetAvailableRooms(request.DateFrom, request.DateTo)
                .Select(r => new RoomDto(r.RoomId, r.RoomNumber, r.Floor, r.Capacity, r.Status, 
                        r.RoomTypeId, r.RoomType.Category, r.RoomType.PricePerNight)).ToList();

            return Task.FromResult(rooms);

        }
    }

}
