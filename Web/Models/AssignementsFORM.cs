using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class AssignementsFORM
    {
        // public int Id { get; set; }

        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
        public int CourseId { get; set; }

        // Vous pouvez inclure d'autres propriétés de validation ou de transformation spécifiques aux besoins du formulaire.
    }
}
