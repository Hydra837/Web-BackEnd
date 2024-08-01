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
            return await _dbSet.ToListAsync();
        }

        // Récupérer les notes par ID utilisateur
        public async Task<GradeData> GetByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("L'ID utilisateur doit être un nombre positif.", nameof(userId));
            }

            return await _dbSet.FirstOrDefaultAsync(g => g.UserId == userId);
        }

        // Récupérer les notes par ID de cours
        public async Task<GradeData> GetByCourseAsync(int courseId)
        {
            if (courseId <= 0)
            {
                throw new ArgumentException("L'ID du cours doit être un nombre positif.", nameof(courseId));
            }

            return await _dbSet.FirstOrDefaultAsync(g => g.CourseId == courseId);
        }

        // Récupérer les notes par ID de cours
        public async Task<IEnumerable<GradeData>> GetByCoursesAsync(int courseId)
        {
            if (courseId <= 0)
            {
                throw new ArgumentException("L'ID du cours doit être un nombre positif.", nameof(courseId));
            }

            return await _dbSet.Where(g => g.CourseId == courseId).ToListAsync();
        }

        // Insérer une nouvelle note pour un utilisateur
        public async Task InsertGrade(int userId, int grade)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("L'ID utilisateur doit être un nombre positif.", nameof(userId));
            }

            var gradeData = new GradeData
            {
                UserId = userId,
                Grade = grade
            };

            await _dbSet.AddAsync(gradeData);
            await _context.SaveChangesAsync();
        }

        // Supprimer une note par ID
        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("L'ID de la note doit être un nombre positif.", nameof(id));
            }

            var gradeData = await _dbSet.FindAsync(id);
            if (gradeData == null)
            {
                throw new InvalidOperationException("La note à supprimer n'existe pas.");
            }

            _dbSet.Remove(gradeData);
            await _context.SaveChangesAsync();
        }

        // Mettre à jour une note existante pour un utilisateur
        public async Task Update(int userId, int grade)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("L'ID utilisateur doit être un nombre positif.", nameof(userId));
            }

            var gradeData = await _dbSet.FirstOrDefaultAsync(g => g.UserId == userId);
            if (gradeData == null)
            {
                throw new InvalidOperationException("La note à mettre à jour n'existe pas.");
            }

            gradeData.Grade = grade;

            _dbSet.Update(gradeData);
            await _context.SaveChangesAsync();
        }
    }
}
