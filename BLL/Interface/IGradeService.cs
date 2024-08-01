using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interface
{
    public interface IGradeService
    {

        Task<IEnumerable<GradeModel>> GetAllGradesAsync();
        Task<GradeModel> GetByUserIdAsync(int userId);
        Task<GradeModel> GetByCourseAsync(int courseId);
        Task<IEnumerable<GradeModel>> GetByCoursesAsync(int id);
        Task InsertGrade(int userid, int grade);
        Task DeleteAsync(int id);
        Task Update(int userid, int grade);
    }
}
