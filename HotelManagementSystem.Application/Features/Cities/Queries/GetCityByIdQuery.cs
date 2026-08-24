
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Cities.Queries
{
    public record GetCityByIdQuery(int Id) : IRequest<CityDto?>;

    public record CityDto(int CityId, string CityName);

    public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCityByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<CityDto?> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            var city = _unitOfWork.Cities.GetById(request.Id);
            
            if(city == null)
            {
                return Task.FromResult<CityDto?>(null);
            }

            var dto = new CityDto(city.CityId, city.CityName);
            return Task.FromResult<CityDto?>(dto);
        }
    }


}
