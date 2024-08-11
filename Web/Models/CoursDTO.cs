namespace Web.Models
{
    public class CoursDTO
    {
        public int Id { get; set; }
        public string? Nom {  get; set; }
        public bool Disponible { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string? Description { get; set; }
        public int? ProfesseurId { get; set; }
      //  public ICollection<>

    }
}
