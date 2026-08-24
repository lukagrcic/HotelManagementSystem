using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Guests.Commands
{
    public record DeleteGuestCommand(int GuestId) : IRequest<bool>;

    public class DeleteGuestCommandHandler : IRequestHandler<DeleteGuestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteGuestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> Handle(DeleteGuestCommand request, CancellationToken cancellationToken)
        {
            var guest = _unitOfWork.Guests.GetById(request.GuestId);

            if (guest is null) return Task.FromResult(false);

            _unitOfWork.Guests.Delete(guest);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
