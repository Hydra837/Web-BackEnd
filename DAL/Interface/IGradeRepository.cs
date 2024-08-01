using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IGradeRepository
    {
        Task <IEnumerable<GradeData>> GetAllGradesAsync ();
        Task<GradeData> GetByUserIdAsync(int userId);
        Task <GradeData> GetByCourseAsync(int courseId);
        Task<IEnumerable<GradeData>> GetByCoursesAsync(int id);
        Task InsertGrade(int userid, int grade);
        Task DeleteAsync(int id);
        Task Update (int userid, int grade);

    }
}
