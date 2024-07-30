using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
   public class UserCourseDetailsModel
    {
        public int UserId { get; set; }
        public string? UserNom { get; set; }
        public string? UserPrenom { get; set; }
        public string? CoursNom { get; set; }
        public bool Disponible { get; set; }
        public string? ProfNom { get; set; }
        public string? ProfPrenom { get; set; }


    }
}
