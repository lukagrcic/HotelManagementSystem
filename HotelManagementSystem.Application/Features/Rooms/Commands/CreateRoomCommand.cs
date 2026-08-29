using FluentValidation;
using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Enums;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Rooms.Commands
{
    public record CreateRoomCommand(string RoomNumber, int Floor, int Capacity, RoomStatus Status, int RoomTypeId) : IRequest<int>;

    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateRoomCommand> _validator;

        public CreateRoomCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateRoomCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public Task<int> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var roomType = _unitOfWork.RoomTypes.GetById(request.RoomTypeId);
            if(roomType is null)
            {
                throw new Exception("Tip sobe ne postoji");
            }

            var room = new Room
            {
                RoomNumber = request.RoomNumber,
                Floor = request.Floor,
                Capacity = request.Capacity,
                Status = request.Status,
                RoomTypeId = request.RoomTypeId
            };

            _unitOfWork.Rooms.Add(room);
            _unitOfWork.SaveChanges();

            return Task.FromResult(room.RoomId);
        }
    }

    public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomCommandValidator()
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
