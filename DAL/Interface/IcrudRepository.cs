using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IcrudRepository 
    {
        public void Create(CoursData cours);
        public void Update(int id);
        public void Delete(int id);
        Task SaveChangesAsync();
    }
}
