using HotelManagementSystem.Domain.Entities;

namespace HotelManagementSystem.Domain.Repositories
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        IEnumerable<Reservation> GetByGuestId(int guestId);
        IEnumerable<Reservation> GetByEmployeeId(int employeeId);
    }
}
