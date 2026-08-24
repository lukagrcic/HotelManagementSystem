using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Employees.Queries
{
    public record GetEmployeeByIdQuery(int EmployeeId) : IRequest<EmployeeDto?>;

    public record EmployeeDto(int EmployeeId, string FirstName, string LastName, string Username);

    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<EmployeeDto?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = _unitOfWork.Employees.GetById(request.EmployeeId);
            if (employee is null) return Task.FromResult<EmployeeDto?>(null);

            var dto = new EmployeeDto(employee.EmployeeId, employee.FirstName, employee.LastName, employee.Username);

            return Task.FromResult<EmployeeDto?>(dto);
        }
    }
}
