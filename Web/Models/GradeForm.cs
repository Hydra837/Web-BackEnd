using DAL.Data;

namespace Web.Models
{
    public class GradeForm
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public int Grade {  get; set; }
    }
}
