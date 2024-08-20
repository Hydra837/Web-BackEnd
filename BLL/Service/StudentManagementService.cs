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
        private ICoursRepository _cours;
        private IusersRepository _users;
        public StudentManagementService(IStudent_Management cours, ICoursRepository courss, IusersRepository users)
        {
            _stu = cours;
            _cours = courss;
            _users = users;
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

        public async Task DeleteAsync1(int id, int coursid)
        {
            if ((id < 0 ) || (coursid < 0)) throw new ArgumentOutOfRangeException(nameof(coursid));
           await _stu.DeleteAsync1(id, coursid);
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
        //public Task GetCoursWithTeacher(int coursid, int userid)
        //{

        //}
        public async Task InsertUserCoursAsync(int id, int id_cours)
        {
            // Appel au repository pour insérer l'inscription de l'utilisateur au cours
            await _stu.InsertStudentCoursesAsync(id, id_cours);
        }

        //public async Task DeleteTeacher(int teacherId, int courseId)
        //{
        //    try
        //    {
        //        await _stu.Deleteteacher(teacherId, courseId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Une erreur à été rencontrer.", ex);
        //    }
        //}

        public async Task UpdateTeacherToCourse(int teacherId, int courseId)
        {
            try
            {
                await _stu.UpdateTeacherToCourse(teacherId, courseId);
            }
            catch (Exception ex)
            {
                ;

                
                throw new Exception("Une erreur à été rencontrer.", ex);
            }
        }


        public async Task Deleteteacher(int idteacher, int course)
        {
            try
            {
                await _stu.Deleteteacher(idteacher, course);
            }
            catch (Exception ex)
            {
                throw new Exception("Une erreur à été rencontrer.", ex);
            }
        }

        public async Task<UsersModel> GetTeacherName(int teacherId)
        {
            var prof = await _users.GetByIdAsync(teacherId);

            if (prof == null)
            {
                throw new Exception("Teacher not found.");
            }

            UsersModel model = prof.ToUserBLL();

            return model;
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserAssignementsModel>> GetuserResult(int id)
        {
            var assignmentsData = await _stu.GetuserResult(id);
            var result = assignmentsData.Select(x => x.ToUserAssignementBLL());
            return result;
        }

    }
}
