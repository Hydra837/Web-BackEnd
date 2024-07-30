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
        [ForeignKey("Users")]
        public int ProfesseurId { get; set; }

        [Required]
        [ForeignKey("Cours")]
        public int CoursId { get; set; }

        // Propriétés de navigation
        public virtual UsersData Instructor { get; set; }
        public virtual CoursData Cours { get; set; }
    }
}
