using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.RoomTypes.Commands
{
    public record DeleteRoomTypeCommand(int RoomTypeId) : IRequest<bool>;

    public class DeleteRoomTypeCommandHandler : IRequestHandler<DeleteRoomTypeCommand, bool>
    {

        private readonly IUnitOfWork _unitOfWork;

        public DeleteRoomTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> Handle(DeleteRoomTypeCommand request, CancellationToken cancellationToken)
        {
            var roomType = _unitOfWork.RoomTypes.GetById(request.RoomTypeId);
            if (roomType is null)
                return Task.FromResult(false);

            _unitOfWork.RoomTypes.Delete(roomType);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
