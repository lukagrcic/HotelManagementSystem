using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Employees.Commands
{
    public record CreateEmployeeCommand(string FirstName, string LastName, string Username, string Password) : IRequest<int>;

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                PasswordHash = request.Password
            };

            _unitOfWork.Employees.Add(employee);
            _unitOfWork.SaveChanges();

            return Task.FromResult(employee.EmployeeId);
        }
    }
}
