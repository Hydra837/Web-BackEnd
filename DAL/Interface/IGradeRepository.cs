using DAL.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IGradeRepository
    {
        // Récupérer toutes les notes
        Task<IEnumerable<GradeData>> GetAllGradesAsync();

        // Récupérer une note par ID utilisateur
        Task<GradeData> GetByUserIdAsync(int userId, int assignementsId);

        // Récupérer toutes les notes par ID utilisateur
        Task<IEnumerable<GradeData>> GetAllByUserAsync(int userId);

        // Récupérer toutes les notes par ID de cours
        Task<IEnumerable<GradeData>> GetAllByCourseAsync(int courseId);

        // Récupérer une note par ID de cours via Assignments
      //  Task<GradeData> GetByCourseAsync(int courseId);

        // Récupérer toutes les notes pour un enseignant via Assignments et Courses
        Task<IEnumerable<GradeData>> GetAllByTeacherAsync(int teacherId);

        // Insérer une note
        Task InsertGradeAsync(GradeData gradeData);

        // Supprimer une note par ID
        Task DeleteAsync(int id);

        // Mettre à jour une note pour un utilisateur spécifique
        Task UpdateAsync(int userId, int grade);

        // Mettre à jour une note spécifique (single object)
        Task UpdateAsync(GradeData gradeModel);

        // Mettre à jour une note spécifique (par ID)
        Task UpdateGradeAsync(int id, int grade);

        // Récupérer toutes les notes par ID de cours (une méthode redondante dans l'interface, selon les besoins)
        Task<IEnumerable<GradeData>> GetByCoursesAsync(int id);
      
        

        // Méthodes de récupération des notes par ID utilisateur et ID cours qui sont déjà définies
        // dans GetAllByUserAsync et GetAllByCourseAsync
    }
}
