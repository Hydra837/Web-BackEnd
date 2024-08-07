namespace Web.Models
{
    public class UserCoursDetailsDTO
    {
        public string? UserNom { get; set; }
        public string? UserPrenom { get; set; }
        public string? CoursNom { get; set; }
        public bool Disponible { get; set; }
        public string? ProfNom { get; set; }
        public string? ProfPrenom { get; set; }
        public int? coursId { get; set; }
        public int? Grade { get; set; }

    }
}
