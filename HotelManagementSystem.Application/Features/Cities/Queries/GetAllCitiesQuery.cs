using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Cities.Queries
{
    public record GetAllCitiesQuery() : IRequest<List<CityDto>>;

    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, List<CityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCitiesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<CityDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            var cities = _unitOfWork.Cities.GetAll()
                .Select(c => new CityDto(c.CityId, c.CityName))
                .ToList();

            return Task.FromResult(cities);
        }
    }
}
