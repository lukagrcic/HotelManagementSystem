using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Reservations.Queries
{
    public record GetAllReservationsQuery() : IRequest<List<ReservationDto>>;

    public class GetAllReservationsQueryHandler : IRequestHandler<GetAllReservationsQuery, List<ReservationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllReservationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<ReservationDto>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
        {
            var reservations = _unitOfWork.Reservations.GetAll()
                .Select(r => new ReservationDto(
                    r.ReservationId,
                    r.DateFrom,
                    r.DateTo,
                    r.CreatedAt,
                    r.IsBreakfastIncluded,
                    r.Note,
                    r.GuestId,
                    r.Guest.FirstName,
                    r.Guest.LastName,
                    r.EmployeeId,
                    r.Employee.FirstName,
                    r.Employee.LastName,
                    r.ReservationRooms.Select(rr => new RoomSummaryDto(
                        rr.Room.RoomId,
                        rr.Room.RoomNumber,
                        rr.Room.RoomType.Category,
                        rr.Room.RoomType.PricePerNight
                    )).ToList()
                )).ToList();

            return Task.FromResult(reservations);
        }
    }
}
