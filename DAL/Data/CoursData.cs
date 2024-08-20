using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DAL.Data
{
    [Table("Cours")]
    public class CoursData : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom { get; set; }

        [Required]
        public bool Disponible { get; set; }

        [Required]
        public DateTime date_debut { get; set; }

        [Required]
        public DateTime date_fin { get; set; }

        [Required]
        public string? Description { get; set; }

        [AllowNull]
        public int? ProfesseurId { get; set; }

        public virtual ICollection<Student_EnrollementData>? StudentEnrollements { get; set; }
        public ICollection<UsersData>? User { get; set; } 
        // Ajout de la propriété de navigation pour les affectations des instructeurs
        public virtual ICollection<Student_ManagementData> InstructorAssignments { get; set; } = new HashSet<Student_ManagementData>();
        public ICollection<AssigementsData>? Assignments { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (date_fin <= date_debut)
            {
                yield return new ValidationResult(
                    "La date de fin doit être après la date de début",
                    new[] { nameof(date_fin) }
                );
            }
        }
    }
}
