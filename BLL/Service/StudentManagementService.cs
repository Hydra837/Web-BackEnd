using BLL.Interface;
using BLL.Mapper;
using BLL.Models;
using DAL.Data;
using DAL.Interface;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class StudentManagementService : IcrudRepository, IStudentManagmentService
    {
        private IStudent_Management _stu;
        public StudentManagementService(IStudent_Management cours)
        {
            _stu = cours;
        }
        public void Create(CoursData cours)
        {

            if (cours == null)
                throw new ArgumentNullException(nameof(cours));

            // Example logic to create the course
            // This should be replaced with your actual implementation
            _stu.Create(cours);
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
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));
            _stu.Delete(id);
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
            IEnumerable<CoursModel> cours = _stu.GetAllCourseByUser(id).Select(w => w.ToCoursBLL());
            if (cours is null)
                return cours; // AJOUT EXCEPTUIN NULLE
            else
                return cours;
        }

        public Task<CoursModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
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

        public async Task InsertUserCoursAsync(int id, int id_cours)
        {
            // Appel au repository pour insérer l'inscription de l'utilisateur au cours
            await _stu.InsertUserCoursAsync(id, id_cours);
        }

        //IEnumerable<CoursData> GetAllCourseByUser(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
