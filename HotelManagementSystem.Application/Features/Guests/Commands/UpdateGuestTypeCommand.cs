using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Guests.Commands
{
    public record UpdateGuestCommand(string Jmbg, string FirstName, string LastName, string Email, string PhoneNumber, int CityId) : IRequest<bool>
    {
        public int GuestId { get; set; }

    }


    public class UpdateGuestCommandHandler : IRequestHandler<UpdateGuestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateGuestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> Handle(UpdateGuestCommand request, CancellationToken cancellationToken)
        {
            var guest = _unitOfWork.Guests.GetById(request.GuestId);
            if(guest is null)
            {
                return Task.FromResult(false);
            }

            var city = _unitOfWork.Cities.GetById(request.CityId);
            if(city is null)
            {
                throw new Exception("Grad ne postoji");
            }

            guest.Jmbg = request.Jmbg;
            guest.FirstName = request.FirstName;
            guest.LastName = request.LastName;
            guest.Email = request.Email;
            guest.PhoneNumber = request.PhoneNumber;
            guest.CityId = request.CityId;

            _unitOfWork.Guests.Update(guest);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
