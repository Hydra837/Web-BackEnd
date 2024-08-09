using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IAssignementsRepository
    {
        Task<AssigementsData> GetbyId(int id);
        Task<IEnumerable<AssigementsData>> GetAll();
        Task <IEnumerable<AssigementsData>> GetAllByCourse(int courseId);
        Task <IEnumerable<AssigementsData>> GetAllByUser(int userId);
        Task<IEnumerable<AssigementsData>> GetAllByteacher(int userId);
        Task Insert(AssigementsData assigementsData);
        Task Update(AssigementsData assigementsData);
        Task Delete (int id);


    }
}
