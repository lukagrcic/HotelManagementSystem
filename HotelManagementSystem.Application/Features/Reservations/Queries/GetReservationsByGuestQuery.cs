using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Reservations.Queries
{
    public record GetReservationsByGuestQuery(int GuestId) : IRequest<List<ReservationDto>>;

    public class GetReservationsByGuestQueryHandler : IRequestHandler<GetReservationsByGuestQuery, List<ReservationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetReservationsByGuestQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<ReservationDto>> Handle(GetReservationsByGuestQuery request, CancellationToken cancellationToken)
        {
            var reservations = _unitOfWork.Reservations.GetByGuestId(request.GuestId)
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
                        rr.RoomId,
                        rr.Room.RoomNumber,
                        rr.Room.RoomType.Category,
                        rr.Room.RoomType.PricePerNight
                    )).ToList()
                )).ToList();

            return Task.FromResult(reservations);
        }
    }
}
