using HotelManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Employees.Queries
{
    public record GetAllEmployeesQuery() : IRequest<List<EmployeeDto>>;

    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, List<EmployeeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllEmployeesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = _unitOfWork.Employees.GetAll()
                .Select(e => new EmployeeDto(e.EmployeeId, e.FirstName, e.LastName, e.Username))
                .ToList();

            return Task.FromResult(employees);
        }
    }
}
