using System;
using System.Collections.Generic;
using System.Text;

namespace HotelManagementSystem.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        T? GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
