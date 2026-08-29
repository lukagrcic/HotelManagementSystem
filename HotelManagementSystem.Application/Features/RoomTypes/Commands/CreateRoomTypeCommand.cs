using FluentValidation;
using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Enums;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.RoomTypes.Commands
{
    public record CreateRoomTypeCommand(RoomCategory Category, decimal PricePerNight, string? Description) : IRequest<int>;


    public class CreateRoomTypeCommandHandler : IRequestHandler<CreateRoomTypeCommand, int>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateRoomTypeCommand> _validator;

        public CreateRoomTypeCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateRoomTypeCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public Task<int> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var roomType = new RoomType
            {
                Category = request.Category,
                PricePerNight = request.PricePerNight,
                Description = request.Description
            };

            _unitOfWork.RoomTypes.Add(roomType);
            _unitOfWork.SaveChanges();

            return Task.FromResult(roomType.RoomTypeId);

        }
    }

    public class CreateRoomTypeCommandValidator : AbstractValidator<CreateRoomTypeCommand>
    {
        public CreateRoomTypeCommandValidator()
        {
            RuleFor(x => x.Category)
                .IsInEnum().WithMessage("Nepostojeća kategorija sobe");

            RuleFor(x => x.PricePerNight)
                .GreaterThan(0).WithMessage("Cena po noćenju mora biti veća od nule");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Opis ne sme biti duži od 500 karaktera");
        }
    }

}
