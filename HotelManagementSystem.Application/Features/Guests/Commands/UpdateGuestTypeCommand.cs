using FluentValidation;
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
        private readonly IValidator<UpdateGuestCommand> _validator;

        public UpdateGuestCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateGuestCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public Task<bool> Handle(UpdateGuestCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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

    public class UpdateGuestCommandValidator : AbstractValidator<UpdateGuestCommand>
    {
        public UpdateGuestCommandValidator()
        {
            RuleFor(x => x.Jmbg)
                .NotEmpty().WithMessage("JMBG je obavezan")
                .Length(13).WithMessage("JMBG mora imati tačno 13 cifara");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ime je obavezno")
                .MaximumLength(50).WithMessage("Ime ne sme biti duže od 50 karaktera");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Prezime je obavezno")
                .MaximumLength(50).WithMessage("Prezime ne sme biti duže od 50 karaktera");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email je obavezan")
                .EmailAddress().WithMessage("Email nije u ispravnom formatu");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Broj telefona je obavezan")
                .MaximumLength(20).WithMessage("Broj telefona ne sme biti duži od 20 karaktera");
        }
    }
}
