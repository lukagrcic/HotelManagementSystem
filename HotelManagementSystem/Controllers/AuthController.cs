using HotelManagementSystem.Authentication;
using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Repositories;
using HotelManagementSystem.DTOs.Auth;
using HotelManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HotelManagementSystem.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtOptions _jwtOptions;
        private readonly PasswordHasher<Employee> _passwordHasher = new();

        public AuthController(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService, IOptions<JwtOptions> jwtOptions)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
            _jwtOptions = jwtOptions.Value;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var employee = _unitOfWork.Employees.GetAll()
                .FirstOrDefault(e => e.Username == request.Username);

            if (employee is null)
                return Unauthorized();

            var result = _passwordHasher.VerifyHashedPassword(employee, employee.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized();

            var token = _jwtTokenService.CreateToken(employee);

            return Ok(new AuthResponse
            {
                Token = token,
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes)
            });
        }
    }
}
