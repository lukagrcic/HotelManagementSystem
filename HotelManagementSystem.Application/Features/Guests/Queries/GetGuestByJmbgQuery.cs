using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Guests.Queries
{
    public record GetGuestByJmbgQuery(string Jmbg) : IRequest<GuestDto?>;

    public class GetGuestByJmbgQueryHandler : IRequestHandler<GetGuestByJmbgQuery, GuestDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetGuestByJmbgQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<GuestDto?> Handle(GetGuestByJmbgQuery request, CancellationToken cancellationToken)
        {
            var guest = _unitOfWork.Guests.GetByJmbg(request.Jmbg);

            if (guest is null) return Task.FromResult<GuestDto?>(null);


            var dto = new GuestDto(guest.GuestId, guest.Jmbg, guest.FirstName, guest.LastName,
                                    guest.Email, guest.PhoneNumber, guest.CityId, guest.City.CityName);

            return Task.FromResult<GuestDto?>(dto);
        }
    }
}
