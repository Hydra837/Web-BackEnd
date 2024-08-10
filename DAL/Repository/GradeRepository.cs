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
    public class GradeRepository : RepositoryBase, IGradeRepository
    {
        private readonly Connection _connection;
        private readonly EFDbContextData _context;
        protected readonly DbSet<GradeData> _dbSet;

        public GradeRepository(Connection connection, EFDbContextData context) : base(connection)
        {
            _connection = connection;
            _context = context;
            _dbSet = _context.Set<GradeData>();
        }

        // Récupérer toutes les notes
        public async Task<IEnumerable<GradeData>> GetAllGradesAsync()
        {
            return await _dbSet
                .Include(g => g.Assignment) // Include Assignments
                .ToListAsync();
        }

        // Récupérer une note par ID utilisateur et ID d'assignement
        public async Task<GradeData> GetByUserIdAsync(int userId, int assignementsId)
        {
            return await _dbSet
                .Include(g => g.Assignment) // Include Assignments
                .FirstOrDefaultAsync(g => g.UserId == userId && g.AssignementsId == assignementsId);
        }

        // Récupérer toutes les notes par ID utilisateur
        public async Task<IEnumerable<GradeData>> GetAllByUserAsync(int userId)
        {
            return await _dbSet
                .Include(g => g.Assignment) // Include Assignments
                .Where(g => g.UserId == userId)
                .ToListAsync();
        }

        // Récupérer toutes les notes par ID de cours
        public async Task<IEnumerable<GradeData>> GetAllByCourseAsync(int courseId)
        {
            return await _dbSet
                .Include(g => g.Assignment) // Include Assignments
                .Where(g => g.Assignment.CoursId == courseId) // Filter by CourseId
                .ToListAsync();
        }

        // Récupérer toutes les notes pour un enseignant via Assignements et Courses
        public async Task<IEnumerable<GradeData>> GetAllByTeacherAsync(int teacherId)
        {
            return await _dbSet
                .Include(g => g.Assignment)
                .ThenInclude(a => a.Cours)
                .Where(g => g.Assignment.Cours.ProfesseurId == teacherId)
                .ToListAsync();
        }

        // Insérer une note
        public async Task InsertGradeAsync(GradeData gradeData)
        {
            // Vérifiez que l'assignement existe
            var assignement = await _context.Assignments.FindAsync(gradeData.AssignementsId);
            if (assignement == null)
            {
                throw new InvalidOperationException("L'assignement spécifié n'existe pas.");
            }

            _context.Attach(assignement);

            await _dbSet.AddAsync(gradeData);
            await _context.SaveChangesAsync();
        }


        // Supprimer une note par ID
        public async Task DeleteAsync(int id)
        {
            var grade = await _dbSet.FindAsync(id);
            if (grade != null)
            {
                _dbSet.Remove(grade);
                await _context.SaveChangesAsync();
            }
        }

        // Mettre à jour une note pour un utilisateur spécifique
        public async Task UpdateAsync(int userId, int grade)
        {
            var gradeData = await _dbSet.FirstOrDefaultAsync(g => g.UserId == userId);
            if (gradeData != null)
            {
                gradeData.Grade = grade;
                _context.Update(gradeData);
                await _context.SaveChangesAsync();
            }
        }

        // Mettre à jour une note spécifique (single object)
        public async Task UpdateAsync(GradeData gradeModel)
        {
            _dbSet.Update(gradeModel);
            await _context.SaveChangesAsync();
        }

        // Mettre à jour une note spécifique (par ID)
        public async Task UpdateGradeAsync(int id, int grade)
        {
            var gradeData = await _dbSet.FindAsync(id);
            if (gradeData != null)
            {
                gradeData.Grade = grade;
                _context.Update(gradeData);
                await _context.SaveChangesAsync();
            }
        }

        // Récupérer toutes les notes par ID de cours (une méthode redondante dans l'interface, selon les besoins)
        public async Task<IEnumerable<GradeData>> GetByCoursesAsync(int id)
        {
            return await _dbSet
                .Include(g => g.Assignment) // Include Assignments
                .Where(g => g.Assignment.CoursId == id) // Filter by CourseId
                .ToListAsync();
        }

        public async Task<IEnumerable<GradeData>> GetAllByAssignmentAsync(int assignementsId)
        {
            return await _dbSet
          .Where(g => g.AssignementsId == assignementsId)
          .ToListAsync();


        }
    }
}
