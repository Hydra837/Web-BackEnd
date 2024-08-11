using DAL.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("Users")]
public class UsersData
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Nom { get; set; }

    [Required]
    [StringLength(50)]
    public string Prenom { get; set; }

    [Required]
    [StringLength(20)]
    public string Roles { get; set; }

    [Required]
    [StringLength(255)]
    public string Passwd { get; set; }

    [Required]
    public string Pseudo { get; set; }

    [Required]
    [StringLength(255)]
    public string Mail { get; set; }

    public string? Salt { get; set; }

    public virtual ICollection<GradeData> Grades { get; set; } = new HashSet<GradeData>();
    public virtual ICollection<Student_EnrollementData> StudentEnrollements { get; set; } = new HashSet<Student_EnrollementData>(); 
    public virtual ICollection<Student_ManagementData> InstructorAssignments { get; set; } = new HashSet<Student_ManagementData>();
    public virtual ICollection<CoursData> Courses { get; set; } = new HashSet<CoursData>();
}
