using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Guests.Commands
{
    public record CreateGuestCommand(string Jmbg, string FirstName, string LastName, string Email, string PhoneNumber, int CityId) : IRequest<int>;

    public class CreateGuestCommandHandler : IRequestHandler<CreateGuestCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateGuestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> Handle(CreateGuestCommand request, CancellationToken cancellationToken)
        {
            var city = _unitOfWork.Cities.GetById(request.CityId);
            if(city is null)
            {
                throw new Exception("Grad ne postoji");
            }

            var guest = new Guest
            {
                Jmbg = request.Jmbg,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                CityId = request.CityId
            };

            _unitOfWork.Guests.Add(guest);
            _unitOfWork.SaveChanges();

            return Task.FromResult(guest.GuestId);
        }
    }
}
