using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Infrastructure.Repositories
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(HotelDbContext context) : base(context)
        {
        }

        public IEnumerable<Reservation> GetByEmployeeId(int employeeId)
        {
            return _dbSet.Include(r => r.ReservationRooms).ThenInclude(rr => rr.Room)
                .Where(r => r.EmployeeId == employeeId).ToList();
        }

        public IEnumerable<Reservation> GetByGuestId(int guestId)
        {
            return _dbSet.Include(r => r.ReservationRooms).ThenInclude(rr => rr.Room)
                .Where(r => r.GuestId == guestId).ToList();
        }
    }
}
