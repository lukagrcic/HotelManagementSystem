using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Cities.Commands
{
    public record UpdateCityCommand(string CityName) : IRequest<bool>
    {
        public int CityId { get; set; }
    }

    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            var city = _unitOfWork.Cities.GetById(request.CityId);
            if(city is null)
            {
                return Task.FromResult(false);
            }

            city.CityName = request.CityName;

            _unitOfWork.Cities.Update(city);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
