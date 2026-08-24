using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Guests.Queries
{
    public record GetGuestByIdQuery(int GuestId) : IRequest<GuestDto?>;

    public record GuestDto(int GuestId, string Jmbg, string FirstName, string LastName, string Email, string PhoneNumber, int CityId, string CityName);

    public class GetGuestByIdQueryHandler : IRequestHandler<GetGuestByIdQuery, GuestDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetGuestByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<GuestDto?> Handle(GetGuestByIdQuery request, CancellationToken cancellationToken)
        {
            var guest = _unitOfWork.Guests.GetById(request.GuestId);

            if (guest is null)
            {
                return Task.FromResult<GuestDto?>(null);
            }

            var dto = new GuestDto(guest.GuestId, guest.Jmbg, guest.FirstName, guest.LastName, guest.Email, guest.PhoneNumber, guest.CityId, guest.City.CityName);

            return Task.FromResult<GuestDto?>(dto);
        }
    }
}