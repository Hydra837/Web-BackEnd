using BLL.Models;
using DAL.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IGradeService
    {
       
        Task<IEnumerable<GradeModel>> GetAllGradesAsync();

      
        Task<IEnumerable<GradeModel>> GetByUserIdAsync(int userId);

        
        Task<IEnumerable<GradeModel>> GetByCourseAsync(int courseId);

  
        Task<IEnumerable<GradeModel>> GetByCoursesAsync(int courseId);

      
        Task InsertGradeAsync(GradeModel gradeModel);

        Task DeleteAsync(int id);

    
        Task UpdateGradeAsync(int id , int grade);

    
        Task UpdateAllGradesAsync(IEnumerable<GradeModel> gradeModels);


        Task<IEnumerable<GradeModel>> GetAllByTeacherAsync(int teacherId);
        public Task<IEnumerable<GradeModel>> GetAllByAssignmentAsync(int assignementsId);
        Task<GradeModel> GetByUserIdAsync(int userId, int assignementsId);
    }
}
