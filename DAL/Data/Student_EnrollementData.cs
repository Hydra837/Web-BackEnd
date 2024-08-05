using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data
{
    [Table("Student_Enrollement")]
    public class Student_EnrollementData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }  // Pas besoin de l'attribut ForeignKey ici

        [Required]
        public int CoursId { get; set; }  // Pas besoin de l'attribut ForeignKey ici

        // Permet à Grade d'être null
        public int? Grade { get; set; }

        // Propriétés de navigation
        public virtual UsersData User { get; set; }
        public virtual CoursData Cours { get; set; }
    }
}
