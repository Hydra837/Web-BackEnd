using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
   public class AssignementsModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int? CoursId { get; set; }
        public bool? IsAvailable { get; set; }
        public CoursData? Cours { get; set; }
        public ICollection<GradeData>? Grades
        {
            get; set;
        }
    }
}
