using FluentValidation;
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
        private readonly IValidator<CreateGuestCommand> _validator;

        public CreateGuestCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateGuestCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public Task<int> Handle(CreateGuestCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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


    public class CreateGuestCommandValidator : AbstractValidator<CreateGuestCommand>
    {
        public CreateGuestCommandValidator()
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
