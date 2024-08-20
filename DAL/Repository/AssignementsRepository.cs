using DAL.Data;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools;

namespace DAL.Repository
{
    public class AssignementsRepository : RepositoryBase, IAssignementsRepository
    {
        private readonly EFDbContextData _context;

        public AssignementsRepository(Connection connection, EFDbContextData context) : base(connection)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssigementsData>> GetAll()
        {
            return await _context.Assignments.ToListAsync();
        }
             

        public async Task<AssigementsData> GetbyId(int id)
        {
            return await _context.Assignments.FindAsync(id);
        }

        public async Task<IEnumerable<AssigementsData>> GetAllByCourse(int courseId)
        {
            return await _context.Assignments.Where(a => a.CoursId == courseId).ToListAsync();
        }

        public async Task<IEnumerable<AssigementsData>> GetAllByUser(int userId)
        {
            return await _context.Assignments
                .Join(
                    _context.StudentEnrollements,
                    assignment => assignment.CoursId,
                    enrollement => enrollement.CoursId,
                    (assignment, enrollement) => new { assignment, enrollement }
                )
                .Where(ae => ae.enrollement.UserId == userId)
                .Select(ae => ae.assignment)
                .ToListAsync();
        }

        public async Task<IEnumerable<AssigementsData>> GetAllByteacher(int teacherId)
        {
            return await _context.Assignments
                .Join(
                    _context.Courses,
                    assignment => assignment.CoursId,
                    course => course.Id,
                    (assignment, course) => new { assignment, course }
                )
                .Where(ac => ac.course.ProfesseurId == teacherId)
                .Select(ac => ac.assignment)
                .ToListAsync();
        }

        public async Task Insert(AssigementsData assignmentData)
        {
            _context.Assignments.Add(assignmentData);
            await _context.SaveChangesAsync();
        }

        public async Task Update(AssigementsData assignmentData)
        {
            _context.Assignments.Update(assignmentData);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}   

