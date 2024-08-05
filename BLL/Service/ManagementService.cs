using BLL.Interface;
using BLL.Mapper;
using BLL.Models;
using DAL;
using DAL.Data;
using DAL.Interface;
using DAL.Mapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class ManagementService  // : IStudentManagmentService
        { 
        private readonly EFDbContextData _contextData;

        public ManagementService(EFDbContextData contextData)
        {
            _contextData = contextData;
        }

        public IEnumerable<CoursModel> CoursPourchaqueEtu()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CoursModel>> CoursPourchaqueEtuAsync(int studentId)
        {
            var courses = await _contextData.StudentEnrollements
                .Where(se => se.UserId == studentId)
                .Include(se => se.Cours)
                .Select(se => se.Cours)
                .ToListAsync();

            return courses.Select(c => c.ToCoursBLL());
        }

        public void Create(CoursModel cours)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(CoursModel entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CoursModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CoursModel> GetAllCourseByUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CoursModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void InsertStudentCourse(int id, int id_Course)
        {
            throw new NotImplementedException();
        }

        public async Task InsertUserCoursAsync(int studentId, int courseId)
        {
            // Vérifier si l'étudiant et le cours existent
            var studentExists = await _contextData.Users.AnyAsync(u => u.Id == studentId);
            var courseExists = await _contextData.Courses.AnyAsync(c => c.Id == courseId);

            if (!studentExists)
            {
                throw new ArgumentException("L'étudiant avec cet ID n'existe pas.");
            }

            if (!courseExists)
            {
                throw new ArgumentException("Le cours avec cet ID n'existe pas.");
            }

            // Créer une nouvelle inscription
            var enrollment = new Student_EnrollementData
            {
                UserId = studentId,
                CoursId = courseId
            };

            // Ajouter l'inscription à la base de données
            await _contextData.StudentEnrollements.AddAsync(enrollment);

            // Sauvegarder les changements dans la base de données
            await _contextData.SaveChangesAsync();
        }

        public void InsertUserCours(int id, int id_cours)
        {
            throw new NotImplementedException();
        }

        public void Update(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, CoursModel entity)
        {
            throw new NotImplementedException();
        }
    }
}
