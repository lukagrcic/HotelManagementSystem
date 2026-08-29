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
            return _dbSet
                .Include(r => r.Guest)
                .Include(r => r.Employee)
                .Include(r => r.ReservationRooms)
                    .ThenInclude(rr => rr.Room)
                        .ThenInclude(room => room.RoomType)
                .Where(r => r.EmployeeId == employeeId)
                .ToList();
        }

        public IEnumerable<Reservation> GetByGuestId(int guestId)
        {
            return _dbSet
                .Include(r => r.Guest)
                .Include(r => r.Employee)
                .Include(r => r.ReservationRooms)
                    .ThenInclude(rr => rr.Room)
                        .ThenInclude(room => room.RoomType)
                .Where(r => r.GuestId == guestId)
                .ToList();
        }


        public override Reservation? GetById(int id)
        {
            return _dbSet
                .Include(r => r.Guest)
                .Include(r => r.Employee)
                .Include(r => r.ReservationRooms)
                    .ThenInclude(rr => rr.Room)
                        .ThenInclude(room => room.RoomType)
                .FirstOrDefault(r => r.ReservationId == id);
        }


        public override IEnumerable<Reservation> GetAll()
        {
            return _dbSet
                .Include(r => r.Guest)
                .Include(r => r.Employee)
                .Include(r => r.ReservationRooms)
                    .ThenInclude(rr => rr.Room)
                        .ThenInclude(room => room.RoomType)
                .ToList();
        }
    }
}
