﻿using DAL.Data;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IStudent_EnrollmentRepository
    {
        Task InsertStudentCourseAsync(UsersData user, CoursData course);
        Task InsertStudentCoursesAsync(int userid, int courseid);
        Task<IEnumerable<CoursData>> CoursPourchaqueEtuAsync(int id);
        public Task InsertStudentCourseAsync2(int user, int course);
        public Task<IEnumerable<UsersData>> GetAlluserBycourse(int id);
        public Task<IEnumerable<CoursData>> EnrolledStudent(int id);
        Task<IEnumerable<Student_EnrollementData>> GetAllGradesAsync();
        Task<IEnumerable<Student_EnrollementData>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Student_EnrollementData>> GetByCourseAsync(int courseId);
        //   Task<IEnumerable<GradeData>> GetByCoursesAsync(int id);
        Task InsertGrade(int id, int grade);
        Task DeleteAsync(int id);
        Task UpdateGrade(int id, int grade);
        Task UpdateGrade(Student_EnrollementData student);
        Task<bool> UpdateGradesAsync(int userId, int courseId, int grade);
        Task<IEnumerable<UsersData>> GetUsersWithCoursesAsync();
        Task<IEnumerable<UsersData>> GetStudentsWithCourses();
        public Task<IEnumerable<UsersData>> GetInstructorsWithCourses(); 
        public Task<CoursData> GetCoursWithUsersAsync(int coursId);
        public Task<UsersData> GetUserWithCoursesAsync(int userId);

        public Task<List<UsersData>> GetAllStudentsWithCoursesAsync();
        public Task<List<UsersData>> GetAllProfessorsWithCoursesAsync();
        public Task<UsersData> GetProfessorWithCoursesAsync1(int professorId);
        Task SaveChangesAsync();


    }
      
        
}
