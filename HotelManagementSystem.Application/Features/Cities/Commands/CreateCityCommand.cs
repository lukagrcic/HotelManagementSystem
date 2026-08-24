using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Cities.Commands
{
    public record CreateCityCommand(string CityName) : IRequest<int>;

    public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, int>
    {

        private readonly IUnitOfWork _unitOfWork;

        public CreateCityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            var city = new City
            {
                CityName = request.CityName
            };

            _unitOfWork.Cities.Add(city);
            _unitOfWork.SaveChanges();


            return Task.FromResult(city.CityId);
        }
    }

}
