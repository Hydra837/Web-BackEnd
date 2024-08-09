using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DAL.Data;

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

        
        [Required]
        public int UserId { get; set; }

        //[Required]
        //public int CourseId { get; set; }
        [Required]
        [Key, Column(Order = 2)]
       public int AssignementsId { get; set; }


        // Propriété de navigation pour la relation avec User
        [Required]
        public virtual UsersData? User { get; set; }

        public AssigementsData? Assignment { get; set; }
    }
}
