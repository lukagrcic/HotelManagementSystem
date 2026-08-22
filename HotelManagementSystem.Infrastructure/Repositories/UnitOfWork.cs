using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly HotelDbContext _context;


        public IRepository<City>? _cities;

        public IRepository<Employee>? _employees;

        public IRepository<RoomType>? _roomTypes;

        public IRoomRepository? _rooms;

        public IGuestRepository? _guests;

        public IReservationRepository? _reservations;

        public UnitOfWork(HotelDbContext context)
        {
            _context = context;
        }

        public IRepository<City> Cities => _cities ??= new Repository<City>(_context);

        public IRepository<Employee> Employees => _employees ??= new Repository<Employee>(_context);

        public IRepository<RoomType> RoomTypes => _roomTypes ??= new Repository<RoomType>(_context);

        public IRoomRepository Rooms => _rooms ??= new RoomRepository(_context);

        public IGuestRepository Guests => _guests ??= new GuestRepository(_context);

        public IReservationRepository Reservations => _reservations ??= new ReservationRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
