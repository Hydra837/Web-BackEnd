using BLL.Models;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ICRUDSerrvice<T>
    {
        public void Create(CoursModel cours);
        public void Update(int id);
        public void Delete(int id);
        Task CreateAsync(T entity);       
        Task UpdateAsync(int id, T entity); 
        Task DeleteAsync(int id);          
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
    }

}
