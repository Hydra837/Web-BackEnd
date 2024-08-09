using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class AssignementsFORM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Course ID is required.")]
        public int? CoursId { get; set; }

        public bool? IsAvailable { get; set; }

        // Vous pouvez inclure d'autres propriétés de validation ou de transformation spécifiques aux besoins du formulaire.
    }
}
