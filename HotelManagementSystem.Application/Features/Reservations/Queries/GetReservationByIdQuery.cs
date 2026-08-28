using HotelManagementSystem.Domain.Enums;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Reservations.Queries
{
    public record GetReservationByIdQuery(int ReservationId) : IRequest<ReservationDto?>;

    public record ReservationDto(
        int ReservationId,
        DateTime DateFrom,
        DateTime DateTo,
        DateTime CreatedAt,
        bool IsBreakfastIncluded,
        string? Note,
        int GuestId,
        string GuestFirstName,
        string GuestLastName,
        int EmployeeId,
        string EmployeeFirstName,
        string EmployeeLastName,
        List<RoomSummaryDto> Rooms
    );

    public record RoomSummaryDto(
        int RoomId, 
        string RoomNumber, 
        RoomCategory Category, 
        decimal PricePerNight
    );

    public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, ReservationDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetReservationByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ReservationDto?> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
        {
            var res = _unitOfWork.Reservations.GetById(request.ReservationId);

            if (res is null)
                return Task.FromResult<ReservationDto?>(null);

            var dto = new ReservationDto(
                res.ReservationId,
                res.DateFrom,
                res.DateTo,
                res.CreatedAt,
                res.IsBreakfastIncluded,
                res.Note,
                res.GuestId,
                res.Guest.FirstName,
                res.Guest.LastName,
                res.EmployeeId,
                res.Employee.FirstName,
                res.Employee.LastName,
                res.ReservationRooms.Select(
                    rr => new RoomSummaryDto(
                        rr.Room.RoomId, rr.Room.RoomNumber, rr.Room.RoomType.Category, rr.Room.RoomType.PricePerNight
                    )).ToList()
            );

            return Task.FromResult<ReservationDto?>(dto);
        }
    }
}
