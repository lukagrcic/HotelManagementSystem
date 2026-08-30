using FluentValidation;
using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Application.Features.Employees.Commands
{
    public record CreateEmployeeCommand(string FirstName, string LastName, string Username, string Password) : IRequest<int>;

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateEmployeeCommand> _validator;
        private readonly PasswordHasher<Employee> _passwordHasher = new();

        public CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, IValidator<CreateEmployeeCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username
            };
            employee.PasswordHash = _passwordHasher.HashPassword(employee, request.Password);

            _unitOfWork.Employees.Add(employee);
            _unitOfWork.SaveChanges();

            return Task.FromResult(employee.EmployeeId);
        }

    }


    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
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
                .NotEmpty().WithMessage("Lozinka je obavezna")
                .MinimumLength(6).WithMessage("Lozinka mora imati bar 6 karaktera");
        }
    }
}
