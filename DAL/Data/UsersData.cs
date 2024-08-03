using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data
{
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

        public string Salt { get; set; }

        public virtual ICollection<GradeData> Grades { get; set; } = new HashSet<GradeData>();

  
        public UsersData() { }

      
        public UsersData(string pseudo, string passwd, string salt, string roles, string nom = null, string prenom = null, string mail = null)
        {
            Pseudo = pseudo;
            Passwd = passwd;
            Salt = salt;
            Roles = roles;
            Nom = nom;
            Prenom = prenom;
            Mail = mail;
        }
    }
}
