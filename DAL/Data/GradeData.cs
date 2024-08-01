using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DAL.Data
{
    [Table("Grade")]
    public class GradeData
    {
        [Key]
        public int Id { get; set; }

        [Required]
         // Vous pouvez spécifier une longueur maximale pour le grade
        public int Grade { get; set; }

        
        [AllowNull]
        public int UserId { get; set; }

        [Required]
        public int CourseId { get; set; }

        // Propriété de navigation pour la relation avec User
        public virtual UsersData User { get; set; }
    }
}
