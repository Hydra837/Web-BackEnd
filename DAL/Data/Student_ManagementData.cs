using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data
{
    [Table("Student_Management")]
    public class Student_ManagementData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProfesseurId { get; set; }  // Clé étrangère pour UsersData

        [Required]
        public int CoursId { get; set; }  // Clé étrangère pour CoursData

        // Propriétés de navigation
        public virtual UsersData Instructor { get; set; }
        public virtual CoursData Cours { get; set; }
    }
}
