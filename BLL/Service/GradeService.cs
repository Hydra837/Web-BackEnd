using BLL.Interface;
using BLL.Models;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;
using BLL.Mapper;

namespace BLL.Service
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepository;

        public GradeService(IGradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }

        public async Task DeleteAsync(int id)
        {
            await _gradeRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GradeModel>> GetAllGradesAsync()
        {
            var gradeDataList = await _gradeRepository.GetAllGradesAsync();
            return gradeDataList.Select(gradeData => gradeData.ToGradeModel());
        }

        public async Task<GradeModel> GetByCourseAsync(int courseId)
        {
            var gradeData = await _gradeRepository.GetByCourseAsync(courseId);
            return gradeData?.ToGradeModel();
        }

        public async Task<IEnumerable<GradeModel>> GetByCoursesAsync(int id)
        {
            var gradeDataList = await _gradeRepository.GetByCoursesAsync(id);
            return gradeDataList.Select(gradeData => gradeData.ToGradeModel());
        }

        public async Task<GradeModel> GetByUserIdAsync(int userId)
        {
            var gradeData = await _gradeRepository.GetByUserIdAsync(userId);
            return gradeData?.ToGradeModel();
        }

        public async Task InsertGrade(int userId, int grade)
        {
            await _gradeRepository.InsertGrade(userId, grade);
        }

        public async Task Update(int userId, int grade)
        {
            await _gradeRepository.Update(userId, grade);
        }
    }
}
