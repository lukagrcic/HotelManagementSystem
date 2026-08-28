using HotelManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Domain.Repositories
{
    public interface IRoomRepository : IRepository<Room>
    {
        bool IsAvailable(int roomId, DateTime dateFrom, DateTime dateTo, int? excludeReservationId = null);
        IEnumerable<Room> GetAvailableRooms(DateTime dateFrom, DateTime dateTo);
    }
}
