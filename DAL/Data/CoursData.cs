using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data
{
    [Table("Cours")]  // Spécifie le nom de la table dans la base de données
    public class CoursData : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]  // Limite la longueur de la chaîne à 100 caractères
        public string Nom { get; set; }

        [Required]
        public bool Disponible { get; set; }

        [Required]
        public DateTime date_debut { get; set; }

        [Required]
        public DateTime date_fin { get; set; }

        [Required]
        public string Description { get; set; }

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

