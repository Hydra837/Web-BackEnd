using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Data
{
    [Table("Users")]  // Spécifie le nom de la table dans la base de données
    public class UsersData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)] // Limite la longueur de la chaîne à 50 caractères
        public string Nom { get; set; }

        [Required]
        [StringLength(50)] // Limite la longueur de la chaîne à 50 caractères
        public string Prenom { get; set; }

        [Required]
        [StringLength(20)] // Limite la longueur de la chaîne à 20 caractères
        public string Roles { get; set; }

        [Required]
        [StringLength(255)] // Considère une longueur plus longue pour les mots de passe hachés
        public string Passwd { get; set; }

        public virtual ICollection<GradeData> Grades { get; set; } = new HashSet<GradeData>();
    }
}
