using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Employees.Commands
{
    public record UpdateEmployeeCommand(string FirstName, string LastName, string Username, string? Password) : IRequest<bool>
    {
        public int EmployeeId { get; set; }
    }

    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = _unitOfWork.Employees.GetById(request.EmployeeId);
            if (employee is null) return Task.FromResult(false);

            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.Username = request.Username;
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                employee.PasswordHash = request.Password;
            }

            _unitOfWork.Employees.Update(employee);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);
        }
    }

}
