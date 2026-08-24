using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Guests.Queries
{
    public record GetAllGuestsQuery() : IRequest<List<GuestDto>>;

    public class GetAllGuestsQueryHandler : IRequestHandler<GetAllGuestsQuery, List<GuestDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllGuestsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<GuestDto>> Handle(GetAllGuestsQuery request, CancellationToken cancellationToken)
        {
            var guests = _unitOfWork.Guests.GetAll()
                .Select(g => new GuestDto(g.GuestId, g.Jmbg, g.FirstName, g.LastName, g.Email, g.PhoneNumber, g.CityId, g.City.CityName))
                .ToList();

            return Task.FromResult(guests);
        }
    }
}
