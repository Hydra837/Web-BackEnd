using BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IGradeService
    {
       
        Task<IEnumerable<GradeModel>> GetAllGradesAsync();

      
        Task<GradeModel> GetByUserIdAsync(int userId);

        
        Task<GradeModel> GetByCourseAsync(int courseId);

  
        Task<IEnumerable<GradeModel>> GetByCoursesAsync(int courseId);

      
        Task InsertGradeAsync(GradeModel gradeModel);

        Task DeleteAsync(int id);

    
        Task UpdateGradeAsync(int id , int grade);

    
        Task UpdateAllGradesAsync(IEnumerable<GradeModel> gradeModels);


        Task<IEnumerable<GradeModel>> GetAllByTeacherAsync(int teacherId);
    }
}
