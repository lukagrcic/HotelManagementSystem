using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.RoomTypes.Queries
{
    public record GetAllRoomTypesQuery() : IRequest<List<RoomTypeDto>>;

    public class GetAllRoomTypesQueryHandler : IRequestHandler<GetAllRoomTypesQuery, List<RoomTypeDto>>
    {

        private readonly IUnitOfWork _unitOfWork;

        public GetAllRoomTypesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<RoomTypeDto>> Handle(GetAllRoomTypesQuery request, CancellationToken cancellationToken)
        {
            var roomTypes = _unitOfWork.RoomTypes.GetAll()
                .Select(rt => new RoomTypeDto(rt.RoomTypeId, rt.Category, rt.PricePerNight, rt.Description))
                .ToList();

            return Task.FromResult(roomTypes);
        }
    }

}
