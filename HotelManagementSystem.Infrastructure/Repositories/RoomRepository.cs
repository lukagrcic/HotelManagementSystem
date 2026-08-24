using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Enums;
using HotelManagementSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Infrastructure.Repositories
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        public RoomRepository(HotelDbContext context) : base(context)
        {
        }

        public override Room? GetById(int id)
        {
            return _dbSet.Include(r => r.RoomType).FirstOrDefault(r => r.RoomId == id);
        }

        public override IEnumerable<Room> GetAll()
        {
            return _dbSet.Include(r => r.RoomType).ToList();
        }


        public IEnumerable<Room> GetAvailableRooms(DateTime dateFrom, DateTime dateTo)
        {
            return _dbSet
                .Include(r => r.RoomType)
                .Where(r => r.Status == RoomStatus.Available &&
                !r.ReservationRooms.Any(rr =>
                dateFrom < rr.Reservation.DateTo && dateTo > rr.Reservation.DateFrom))
                .ToList();
        }

        public bool IsAvailable(int roomId, DateTime dateFrom, DateTime dateTo)
        {
            return !_context.ReservationRooms
                .Where(rr => rr.RoomId == roomId)
                .Any(rr => dateFrom < rr.Reservation.DateTo
                    && dateTo > rr.Reservation.DateFrom);
        }
    }
}
