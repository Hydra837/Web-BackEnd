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
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("Cours")]
        public int CoursId { get; set; }
        public int Grade {  get; set; }

        // Propriétés de navigation
        public virtual UsersData User { get; set; }
        public virtual CoursData Cours { get; set; }
    }
}
