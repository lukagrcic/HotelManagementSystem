using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Cities.Commands
{
    public record DeleteCityCommand(int CityId) : IRequest<bool>;

    public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            var city = _unitOfWork.Cities.GetById(request.CityId);
            if(city is null)
            {
                return Task.FromResult(false);
            }

            _unitOfWork.Cities.Delete(city);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
