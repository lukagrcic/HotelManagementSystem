using FluentValidation;
using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Reservations.Commands
{
    public record CreateReservationCommand(
        int GuestId,
        int EmployeeId,
        List<int> RoomIds,
        DateTime DateFrom,
        DateTime DateTo,
        bool IsBreakfastIncluded,
        string? Note
    ) : IRequest<int>;

    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateReservationCommand> _validator;

        public CreateReservationCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateReservationCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public Task<int> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }


            var guest = _unitOfWork.Guests.GetById(request.GuestId);
            if (guest is null) throw new Exception("Gost ne postoji");

            var employee = _unitOfWork.Employees.GetById(request.EmployeeId);
            if (employee is null) throw new Exception("Zaposleni ne postoji");

            //if(request.DateFrom < DateTime.UtcNow || 
            //    request.DateTo < DateTime.UtcNow)
            //{
            //    throw new Exception("Ne moze se napraviti rezervacija za dan koji je prosao");
            //}

            //if(request.DateFrom >= request.DateTo)
            //{
            //    throw new Exception("Datum od mora biti pre datuma do");
            //}

            var allRoomsExists = request.RoomIds.All(
                r => _unitOfWork.Rooms.GetById(r) is not null);
            if (!allRoomsExists)
            {
                throw new Exception("Ne postovje sve izabrane sobe");
            }

            var allRoomsAreAvailable = request.RoomIds.All(
                r => _unitOfWork.Rooms.IsAvailable(
                    r, request.DateFrom, request.DateTo));
            if (!allRoomsAreAvailable)
            {
                throw new Exception("Nisu sve sbode dostupne u izabranom terminu");
            }



            var reservation = new Reservation
            {
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                CreatedAt = DateTime.UtcNow,
                IsBreakfastIncluded = request.IsBreakfastIncluded,
                Note = request.Note,
                EmployeeId = request.EmployeeId,
                GuestId = request.GuestId,
                ReservationRooms = request.RoomIds
                    .Select(roomId => new ReservationRoom
                    {
                        RoomId = roomId
                    }).ToList()
            };

            _unitOfWork.Reservations.Add(reservation);
            _unitOfWork.SaveChanges();

            return Task.FromResult(reservation.ReservationId);
        }
    }


    public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
    {
        public CreateReservationCommandValidator()
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
