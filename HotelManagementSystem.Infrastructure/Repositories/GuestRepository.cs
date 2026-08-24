using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Infrastructure.Repositories
{
    public class GuestRepository : Repository<Guest>, IGuestRepository
    {
        public GuestRepository(HotelDbContext context) : base(context)
        {
        }

        public override Guest? GetById(int id) =>
            _dbSet.Include(g => g.City).FirstOrDefault(g => g.GuestId == id);

        public override IEnumerable<Guest> GetAll() =>
            _dbSet.Include(g => g.City).ToList();

        public Guest? GetByJmbg(string jmbg) =>
            _dbSet.Include(g => g.City).FirstOrDefault(g => g.Jmbg == jmbg);
    }
}
