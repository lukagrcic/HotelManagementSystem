using HotelManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<City> Cities { get; }
        IRepository<Employee> Employees { get; }
        IRepository<RoomType> RoomTypes { get; }
        IRoomRepository Rooms { get; }
        IGuestRepository Guests { get; }
        IReservationRepository Reservations { get; }

        void SaveChanges();
    }
}
