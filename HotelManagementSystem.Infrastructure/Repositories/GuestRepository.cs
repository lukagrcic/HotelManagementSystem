using HotelManagementSystem.Domain.Entities;
using HotelManagementSystem.Domain.Repositories;
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

        public Guest? GetById(string jmbg)
        {
            return _dbSet.FirstOrDefault(g => g.Jmbg == jmbg);
        }
    }
}
