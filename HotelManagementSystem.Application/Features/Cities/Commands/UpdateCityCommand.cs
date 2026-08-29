using FluentValidation;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Cities.Commands
{
    public record UpdateCityCommand(string CityName) : IRequest<bool>
    {
        public int CityId { get; set; }
    }

    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateCityCommand> _validator;

        public UpdateCityCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateCityCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public Task<bool> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }


            var city = _unitOfWork.Cities.GetById(request.CityId);
            if(city is null)
            {
                return Task.FromResult(false);
            }

            city.CityName = request.CityName;

            _unitOfWork.Cities.Update(city);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);
        }
    }


    public class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
    {
        public UpdateCityCommandValidator()
        {
            RuleFor(x => x.CityName)
                .NotEmpty().WithMessage("Naziv grada je obavezan")
                .MaximumLength(100).WithMessage("Naziv grada ne sme biti duži od 100 karaktera");
        }
    }
}
