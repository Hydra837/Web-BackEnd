using Microsoft.EntityFrameworkCore;
using DAL.Data;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class EFDbContextData : DbContext
    {
        public EFDbContextData(DbContextOptions<EFDbContextData> options) : base(options) { }

        // DbSets pour les entités
        public DbSet<UsersData> Users { get; set; }
        public DbSet<CoursData> Courses { get; set; }
        public DbSet<Student_EnrollementData> StudentEnrollements { get; set; }
        public DbSet<Student_ManagementData> InstructorAssignments { get; set; }
        public DbSet<UserCourseDetailsData> UserCourseDetails { get; set; }
        public DbSet<GradeData> Grades { get; set; }
        public DbSet<AssigementsData> Assignments { get; set; }

        // Configuration des entités et des relations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de la relation entre Student_EnrollementData et UsersData
            modelBuilder.Entity<Student_EnrollementData>()
                .HasOne(se => se.User)
                .WithMany(u => u.StudentEnrollements)
                .HasForeignKey(se => se.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration de la relation entre Student_EnrollementData et CoursData
            modelBuilder.Entity<Student_EnrollementData>()
                .HasOne(se => se.Cours)
                .WithMany(c => c.StudentEnrollements)
                .HasForeignKey(se => se.CoursId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration de la relation entre Student_ManagementData et UsersData
            modelBuilder.Entity<Student_ManagementData>()
                .HasOne(ia => ia.Instructor)
                .WithMany(u => u.InstructorAssignments)
                .HasForeignKey(ia => ia.ProfesseurId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration de la relation entre Student_ManagementData et CoursData
            modelBuilder.Entity<Student_ManagementData>()
                .HasOne(ia => ia.Cours)
                .WithMany(c => c.InstructorAssignments)
                .HasForeignKey(ia => ia.CoursId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration de la relation entre GradeData et UsersData
            modelBuilder.Entity<GradeData>()
                .HasOne(g => g.User)
                .WithMany(u => u.Grades)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration de la relation entre GradeData et AssigementsData
            modelBuilder.Entity<GradeData>()
                .HasOne(g => g.Assignment)
                .WithMany(a => a.Grades)
                .HasForeignKey(g => g.AssignementsId)
                .OnDelete(DeleteBehavior.Cascade);

            // Définir la clé primaire pour GradeData
            modelBuilder.Entity<GradeData>()
                .HasKey(g => g.Id);

            // Assurez-vous que la propriété Id est auto-générée
            modelBuilder.Entity<GradeData>()
                .Property(g => g.Id)
                .ValueGeneratedOnAdd();

            // Configuration pour UserCourseDetailsData
            modelBuilder.Entity<UserCourseDetailsData>(entity =>
            {
                entity.HasNoKey(); // Indique que cette entité n'a pas de clé primaire
            });

            // Configuration pour Student_EnrollementData
            modelBuilder.Entity<Student_EnrollementData>()
                .Property(se => se.Grade)
                .HasColumnType("int"); // Définir le type de colonne pour Grade si nécessaire

            // Configuration pour AssigementsData
            modelBuilder.Entity<AssigementsData>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<AssigementsData>()
                .Property(a => a.Id)
                .ValueGeneratedOnAdd(); // Assurez-vous que l'Id est généré automatiquement
        }

        // Pour activer le logging des requêtes SQL pour le débogage
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("YourConnectionStringHere") // Remplacez par votre chaîne de connexion
                    .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                    .EnableSensitiveDataLogging(); // Active le logging des données sensibles, à utiliser avec précaution
            }
        }

        // Méthode pour récupérer les étudiants avec leurs cours
        public List<UsersData> GetStudentsWithCourses()
        {
            return Users
                .Include(u => u.StudentEnrollements)
                    .ThenInclude(se => se.Cours)
                .Where(u => u.Roles == "Etudiant")
                .ToList();
        }

        // Méthode pour récupérer les professeurs avec leurs cours
        public List<UsersData> GetInstructorsWithCourses()
        {
            return Users
                .Include(u => u.InstructorAssignments)
                    .ThenInclude(ia => ia.Cours)
                .Where(u => u.Roles == "Professeur")
                .ToList();
        }
    }
}
