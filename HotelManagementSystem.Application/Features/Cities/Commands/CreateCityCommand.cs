using FluentValidation;
using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Cities.Commands
{
    public record CreateCityCommand(string CityName) : IRequest<int>;

    public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, int>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCityCommand> _validator;

        public CreateCityCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateCityCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public Task<int> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }


            var city = new City
            {
                CityName = request.CityName
            };

            _unitOfWork.Cities.Add(city);
            _unitOfWork.SaveChanges();


            return Task.FromResult(city.CityId);
        }
    }


    public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
    {
        public CreateCityCommandValidator()
        {
            RuleFor(x => x.CityName)
                .NotEmpty().WithMessage("Naziv grada je obavezan")
                .MaximumLength(100).WithMessage("Naziv grada ne sme biti duži od 100 karaktera");
        }
    }

}
