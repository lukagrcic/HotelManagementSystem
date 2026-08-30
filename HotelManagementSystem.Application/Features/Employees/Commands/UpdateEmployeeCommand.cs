using FluentValidation;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
        private readonly IValidator<UpdateEmployeeCommand> _validator;
        private readonly PasswordHasher<Domain.Entities.Employee> _passwordHasher = new();

        public UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork, IValidator<UpdateEmployeeCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var employee = _unitOfWork.Employees.GetById(request.EmployeeId);
            if (employee is null) return Task.FromResult(false);

            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.Username = request.Username;
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                employee.PasswordHash = _passwordHasher.HashPassword(employee, request.Password);
            }

            _unitOfWork.Employees.Update(employee);
            _unitOfWork.SaveChanges();

            return Task.FromResult(true);
        }
    }


    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ime je obavezno")
                .MaximumLength(50).WithMessage("Ime ne sme biti duze od 50 karaktera");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Prezime je obavezno")
                .MaximumLength(50).WithMessage("Prezime ne sme biti duze od 50 karaktera");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Korisnicko ime je obavezno")
                .MinimumLength(4).WithMessage("Korisnicko ime mora imati bar 4 karaktera");

            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("Lozinka mora imati bar 6 karaktera")
                .When(x => !string.IsNullOrWhiteSpace(x.Password));
        }
    }

}
