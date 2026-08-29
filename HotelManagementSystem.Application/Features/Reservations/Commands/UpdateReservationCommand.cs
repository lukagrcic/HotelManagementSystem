using FluentValidation;
using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Reservations.Commands
{
    public record UpdateReservationCommand(
        int GuestId,
        int EmployeeId,
        List<int> RoomIds,
        DateTime DateFrom,
        DateTime DateTo,
        bool IsBreakfastIncluded,
        string? Note
    ) : IRequest<bool>
    {
        public int ReservationId { get; set; }
    }

    public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateReservationCommand> _validator;

        public UpdateReservationCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateReservationCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public Task<bool> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }


            var reservation = _unitOfWork.Reservations.GetById(request.ReservationId);
            if (reservation is null)
                return Task.FromResult(false);

            var guest = _unitOfWork.Guests.GetById(request.GuestId);
            if (guest is null) throw new Exception("Gost ne postoji");

            var employee = _unitOfWork.Employees.GetById(request.EmployeeId);
            if (employee is null) throw new Exception("Zaposleni ne postoji");

            //if (request.DateFrom < DateTime.UtcNow ||
            //    request.DateTo < DateTime.UtcNow)
            //    throw new Exception("Ne moze se napraviti rezervacija za dan koji je prosao");

            //if(request.DateFrom >= request.DateTo)
            //    throw new Exception("Datum od mora biti pre datuma do");

            var allRoomsExist = request.RoomIds.All(
                r => _unitOfWork.Rooms.GetById(r) is not null);
            if(!allRoomsExist)
                throw new Exception("Ne postovje sve izabrane sobe");

            var allRoomsAreAvailable = request.RoomIds.All(
                r => _unitOfWork.Rooms.IsAvailable(r, request.DateFrom, request.DateTo, request.ReservationId));
            if(!allRoomsAreAvailable)
                throw new Exception("Nisu sve sbode dostupne u izabranom terminu");


            reservation.GuestId = request.GuestId;
            reservation.EmployeeId = request.EmployeeId;
            reservation.DateFrom = request.DateFrom;
            reservation.DateTo = request.DateTo;
            reservation.IsBreakfastIncluded = request.IsBreakfastIncluded;
            reservation.Note = request.Note;


            reservation.ReservationRooms = request.RoomIds
                .Select(roomId => new ReservationRoom
                {
                    RoomId = roomId
                }).ToList();

            _unitOfWork.Reservations.Update(reservation);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);
        }
    }

    public class UpdateReservationCommandValidator : AbstractValidator<UpdateReservationCommand>
    {
        public UpdateReservationCommandValidator()
        {
            RuleFor(x => x.RoomIds)
                .NotEmpty().WithMessage("Mora biti izabrana bar jedna soba");

            RuleFor(x => x.DateFrom)
                .LessThan(x => x.DateTo).WithMessage("Datum od mora biti pre datuma do");

            RuleFor(x => x.DateFrom)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Ne može se napraviti rezervacija za dan koji je prošao");
        }
    }
}
