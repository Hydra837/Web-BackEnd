using DAL.Data;
using DAL.Interface;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Tools;

namespace DAL.Repository
{
    public class Student_ManagementRepository : RepositoryBase, IStudent_Management
    {
        private readonly EFDbContextData _context;
        private readonly DbSet<UserAssignementsData> _detailsDbSet;
        private readonly Connection _connection;
        public Student_ManagementRepository(Connection connection, EFDbContextData context) : base(connection)
        {
            _connection = connection;
            _context = context;
            _detailsDbSet = _context.UserAssignements;
        }

        public void Create(CoursData cours)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(CoursData cours)
        {
            try
            {
                await _context.Courses.AddAsync(cours);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Erreur de mise à jour de la base de données : " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur est survenue : " + ex.Message);
                throw;
            }
        }

        public Task CreateAsync(UsersData usersData)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync1(int studentId, int courseId)
        {
            try
            {
                var enrollment = await _context.StudentEnrollements
                    .FirstOrDefaultAsync(e => e.UserId == studentId && e.CoursId == courseId);

                if (enrollment != null)
                {
                    _context.StudentEnrollements.Remove(enrollment);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Erreur de mise à jour de la base de données : " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur est survenue : " + ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var student = await _context.Users.FindAsync(id);
                if (student != null)
                {
                    _context.Users.Remove(student);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Erreur de mise à jour de la base de données : " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur est survenue : " + ex.Message);
                throw;
            }
        }

        public async Task Deleteteacher(int idteacher, int course)
        {
            var courses = await _context.Courses.FindAsync(course);
            if (courses == null)
            {
                throw new Exception("Course not found");
            }

            if (courses.ProfesseurId == idteacher)
            {
                courses.ProfesseurId = null;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Teacher not assigned to this course");
            }
        }


        public IEnumerable<CoursData> GetAllCourseByUser(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CoursData>> GetAllCourseByUserAsync(int userId)
        {
            return await (from enrollment in _context.StudentEnrollements
                          join course in _context.Courses on enrollment.CoursId equals course.Id
                          where enrollment.UserId == userId
                          select course).ToListAsync();
        }

        public Task InsertStudentCoursesAsync(int user, int course)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTeacherToCourse(int teacherId, int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                throw new Exception("Course not found");
            }

            course.ProfesseurId = teacherId;
            await _context.SaveChangesAsync();
        }



        
        public async Task InsertUserCoursAsync(int id, int id_cours)
        {
         
            var studentManagement = new Student_ManagementData
            {
                ProfesseurId = id,
                CoursId = id_cours,
         
            };

            await _context.InstructorAssignments.AddAsync(studentManagement);

           
            await _context.SaveChangesAsync();
        }

        public async Task InsertUserCourseAsync(int userId, int courseId)
        {
            try
            {
                var existingEnrollment = await _context.StudentEnrollements
                    .AnyAsync(e => e.UserId == userId && e.CoursId == courseId);

                if (existingEnrollment)
                {
                    throw new InvalidOperationException("L'étudiant est déjà inscrit à ce cours.");
                }

                var enrollment = new Student_EnrollementData
                {
                    UserId = userId,
                    CoursId = courseId
                };

                await _context.StudentEnrollements.AddAsync(enrollment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Erreur de mise à jour de la base de données : " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur est survenue : " + ex.Message);
                throw;
            }
        }

        public void Update(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(int id, UsersData updatedUserData)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser != null)
                {
                    _context.Entry(existingUser).CurrentValues.SetValues(updatedUserData);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Erreur de mise à jour de la base de données : " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur est survenue : " + ex.Message);
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserAssignementsData>> GetuserResult(int userId)
        {
            string storedProcedure = "GetUserCoursesAssignmentsGrades";

            var parameters = new { UserId = userId };

            using (var connection = _context.Database.GetDbConnection())
            {
                connection.Open();
                var result = await connection.QueryAsync<UserAssignementsData>(
                    storedProcedure,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result;
            }
        }
    }
}
