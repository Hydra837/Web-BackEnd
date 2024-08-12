namespace Web.Models
{
    public class AssignementsDTO
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int? CoursId { get; set; }
        public bool? IsAvailable { get; set; }
       // public CoursDTO? Cours { get; set; } // DTO pour le cours
        //public ICollection<GradeDTO>? Grades { get; set; } // DTO pour les notes
    }
}
