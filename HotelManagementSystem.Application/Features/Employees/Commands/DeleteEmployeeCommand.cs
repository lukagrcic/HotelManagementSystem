using HotelManagementSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Employees.Commands
{
    public record DeleteEmployeeCommand(int EmployeeId) : IRequest<bool>;
    
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = _unitOfWork.Employees.GetById(request.EmployeeId);
            if (employee is null) return Task.FromResult(false);

            _unitOfWork.Employees.Delete(employee);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
