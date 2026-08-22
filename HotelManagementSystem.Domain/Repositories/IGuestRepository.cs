using HotelManagementSystem.Domain.Entities;

namespace HotelManagementSystem.Domain.Repositories
{
    public interface IGuestRepository : IRepository<Guest>
    {
        Guest? GetById(string jmbg);
    }
}
