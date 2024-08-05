using Microsoft.EntityFrameworkCore;
using DAL.Data;

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
        public DbSet<GradeData> Grades { get; set; } // Ajout de GradeData

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
                .OnDelete(DeleteBehavior.SetNull);

            // Configuration pour UserCourseDetailsData
            modelBuilder.Entity<UserCourseDetailsData>(entity =>
            {
                entity.HasNoKey(); // Indique que cette entité n'a pas de clé primaire
            });

            // Configuration pour Student_EnrollementData
            modelBuilder.Entity<Student_EnrollementData>()
                .Property(se => se.Grade)
                .HasColumnType("int"); // Définir le type de colonne pour Grade si nécessaire
        }
    }
}
