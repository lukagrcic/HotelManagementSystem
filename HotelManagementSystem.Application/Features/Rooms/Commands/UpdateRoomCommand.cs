using FluentValidation;
using HotelManagementSystem.Domain.Enums;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Rooms.Commands
{
    public record UpdateRoomCommand(string RoomNumber, int Floor, int Capacity, RoomStatus Status, int RoomTypeId) : IRequest<bool>
    {
        public int RoomId { get; set; }
    }

    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateRoomCommand> _validator;

        public UpdateRoomCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateRoomCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public Task<bool> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var room = _unitOfWork.Rooms.GetById(request.RoomId);
            if (room is null) return Task.FromResult(false);

            var roomType = _unitOfWork.RoomTypes.GetById(request.RoomTypeId);
            if (roomType is null) return Task.FromResult(false);

            room.RoomNumber = request.RoomNumber;
            room.Floor = request.Floor;
            room.Capacity = request.Capacity;
            room.Status = request.Status;
            room.RoomTypeId = request.RoomTypeId;

            _unitOfWork.Rooms.Update(room);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);

        }
    }

    public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
    {
        public UpdateRoomCommandValidator()
        {
            RuleFor(x => x.RoomNumber)
                .NotEmpty().WithMessage("Broj sobe je obavezan")
                .MaximumLength(3).WithMessage("Broj sobe ne sme biti duži od 3 karaktera");

            RuleFor(x => x.Floor)
                .GreaterThanOrEqualTo(0).WithMessage("Sprat ne može biti negativan");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Kapacitet mora biti veći od nule");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Nepostojeći status sobe");
        }
    }
}
