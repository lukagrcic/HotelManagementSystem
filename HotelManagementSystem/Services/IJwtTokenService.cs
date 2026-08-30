using HotelManagementSystem.Domain.Entities;

namespace HotelManagementSystem.Services
{
    public interface IJwtTokenService
    {   
        string CreateToken(Employee employee);
    }
}
