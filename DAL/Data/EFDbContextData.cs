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

        // Configuration des entités et des relations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de la relation entre Student_EnrollementData et UsersData
            modelBuilder.Entity<Student_EnrollementData>()
                .HasOne(se => se.User)
                .WithMany() // Si vous souhaitez avoir une collection d'inscriptions sur l'entité User, utilisez .WithMany(u => u.StudentEnrollements)
                .HasForeignKey(se => se.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration de la relation entre Student_EnrollementData et CoursData
            modelBuilder.Entity<Student_EnrollementData>()
                .HasOne(se => se.Cours)
                .WithMany() // Si vous souhaitez avoir une collection d'inscriptions sur l'entité Cours, utilisez .WithMany(c => c.StudentEnrollements)
                .HasForeignKey(se => se.CoursId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration de la relation entre Instructor_AssignmentData et UsersData
            modelBuilder.Entity<Student_ManagementData>()
                .HasOne(ia => ia.Instructor)
                .WithMany() // Si vous souhaitez avoir une collection d'affectations sur l'entité User, utilisez .WithMany(u => u.InstructorAssignments)
                .HasForeignKey(ia => ia.ProfesseurId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration de la relation entre Instructor_AssignmentData et CoursData
            modelBuilder.Entity<Student_ManagementData>()
                .HasOne(ia => ia.Cours)
                .WithMany() // Si vous souhaitez avoir une collection d'affectations sur l'entité Cours, utilisez .WithMany(c => c.InstructorAssignments)
                .HasForeignKey(ia => ia.CoursId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserCourseDetailsData>(entity =>
            {
                entity.HasNoKey(); // Indique que cette entité n'a pas de clé primaire
            });

        }
    }
}
