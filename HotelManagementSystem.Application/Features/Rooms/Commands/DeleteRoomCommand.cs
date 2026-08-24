using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Rooms.Commands
{
    public record DeleteRoomCommand(int RoomId) : IRequest<bool>;

    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRoomCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            var room = _unitOfWork.Rooms.GetById(request.RoomId);
            if (room is null) return Task.FromResult(false);

            _unitOfWork.Rooms.Delete(room);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
