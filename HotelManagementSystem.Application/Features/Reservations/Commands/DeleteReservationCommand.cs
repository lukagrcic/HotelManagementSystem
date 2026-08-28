using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Reservations.Commands
{
    public record DeleteReservationCommand(int ReservationId) : IRequest<bool>;

    public class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteReservationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = _unitOfWork.Reservations.GetById(request.ReservationId);
            if (reservation is null)
                return Task.FromResult(false);

            _unitOfWork.Reservations.Delete(reservation);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);
        }
    }


}
