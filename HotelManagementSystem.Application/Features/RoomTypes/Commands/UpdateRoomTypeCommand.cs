using FluentValidation;
using HotelManagementSystem.Domain.Enums;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.RoomTypes.Commands
{
    public record UpdateRoomTypeCommand(RoomCategory Category, decimal PricePerNight, string? Description) : IRequest<bool>
    {
        public int RoomTypeId { get; set; }
    }

    public class UpdateRoomTypeCommandHandler : IRequestHandler<UpdateRoomTypeCommand, bool>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateRoomTypeCommand> _validator;


        public UpdateRoomTypeCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateRoomTypeCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public Task<bool> Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var roomType = _unitOfWork.RoomTypes.GetById(request.RoomTypeId);
            if(roomType is null)
            {
                return Task.FromResult(false);
            }

            roomType.Category = request.Category;
            roomType.PricePerNight = request.PricePerNight;
            roomType.Description = request.Description;

            _unitOfWork.RoomTypes.Update(roomType);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);                
        }
    }

    public class UpdateRoomTypeCommandValidator : AbstractValidator<UpdateRoomTypeCommand>
    {
        public UpdateRoomTypeCommandValidator()
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
