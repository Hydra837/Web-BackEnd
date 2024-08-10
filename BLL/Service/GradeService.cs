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
            _gradeRepository = gradeRepository ?? throw new ArgumentNullException(nameof(gradeRepository));
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("L'ID de la note doit être un nombre positif.", nameof(id));
            }

            await _gradeRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GradeModel>> GetAllByTeacherAsync(int teacherId)
        {
            if (teacherId <= 0)
            {
                throw new ArgumentException("L'ID de l'enseignant doit être un nombre positif.", nameof(teacherId));
            }

            var gradeDataList = await _gradeRepository.GetAllByTeacherAsync(teacherId);
            return gradeDataList.Select(gradeData => gradeData.ToGradeModel());
        }

        public async Task<IEnumerable<GradeModel>> GetAllGradesAsync()
        {
            var gradeDataList = await _gradeRepository.GetAllGradesAsync();
            return gradeDataList.Select(gradeData => gradeData.ToGradeModel());
        }

        public async Task<IEnumerable<GradeModel>> GetByCoursesAsync(int courseId)
        {
            if (courseId <= 0)
            {
                throw new ArgumentException("L'ID du cours doit être un nombre positif.", nameof(courseId));
            }

            var gradeDataList = await _gradeRepository.GetByCoursesAsync(courseId);
            return gradeDataList.Select(gradeData => gradeData.ToGradeModel());
        }

        public async Task<IEnumerable<GradeModel>> GetByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("L'ID de l'utilisateur doit être un nombre positif.", nameof(userId));
            }

            IEnumerable<GradeData> gradeData = await _gradeRepository.GetAllByUserAsync(userId);
            return gradeData.Select(x => x.ToGradeModel());
        }


        public async Task InsertGradeAsync(GradeModel gradeModel)
        {
            if (gradeModel == null)
            {
                throw new ArgumentNullException(nameof(gradeModel));
            }

            var gradeData = gradeModel.ToGradeData();
            await _gradeRepository.InsertGradeAsync(gradeData);
        }

        public async Task UpdateGradeAsync(GradeModel gradeModel)
        {
            if (gradeModel == null)
            {
                throw new ArgumentNullException(nameof(gradeModel));
            }

            var gradeData = gradeModel.ToGradeData();
            await _gradeRepository.UpdateAsync(gradeData.Id, gradeData.Grade);
        }

        public async Task UpdateAllGradesAsync(IEnumerable<GradeModel> gradeModels)
        {
            if (gradeModels == null)
            {
                throw new ArgumentNullException(nameof(gradeModels));
            }

            var gradeDataList = gradeModels.Select(gm => gm.ToGradeData()).ToList();
            foreach (var gradeData in gradeDataList)
            {
                await _gradeRepository.UpdateAsync(gradeData.Id, gradeData.Grade);
            }
        }

        public async Task UpdateGradeAsync(int id, int grade)
        {
            if (id <= 0)
            {
                throw new ArgumentException("L'ID de la note doit être un nombre positif.", nameof(id));
            }

            if (grade < 0) 
            {
                throw new ArgumentException("La note doit être un nombre positif ou nul.", nameof(grade));
            }

            await _gradeRepository.UpdateGradeAsync(id, grade);
        }

        public Task<IEnumerable<GradeModel>> GetByCourseAsync(int courseId)
        {
            throw new NotImplementedException();
        }

        public async Task<GradeModel> GetByUserIdAsync(int userId, int assignementsId)
        {
            var gradeData = await _gradeRepository.GetByUserIdAsync(userId, assignementsId);


            if (gradeData == null)
            {
                return null;
            }
     
            var grade = gradeData.ToGradeModel();
            return grade;
        }

        public async Task<IEnumerable<GradeModel>> GetAllByAssignmentAsync(int assignementsId)
        {
            var grades = await _gradeRepository.GetAllByAssignmentAsync(assignementsId);
            return grades.Select(g => g.ToGradeModel()).ToList();
           
        }
    }
}
