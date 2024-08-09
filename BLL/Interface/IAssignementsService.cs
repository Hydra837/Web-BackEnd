using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interface
{
    public interface IAssignementsService
    {
        Task<AssignementsModel> GetbyId(int id);
        Task<IEnumerable<AssignementsModel>> GetAll();
        Task<IEnumerable<AssignementsModel>> GetAllByCourse(int courseId);
        Task<IEnumerable<AssignementsModel>> GetAllByUser(int userId);
        Task<IEnumerable<AssignementsModel>> GetAllByTeacher(int userId);
        Task Insert(AssignementsModel assigementsData);
        Task Update(AssignementsModel assigementsData);
        Task Delete(int id);
    }
}
