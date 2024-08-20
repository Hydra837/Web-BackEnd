using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class UserAssignementsModel
    {
        public int? CoursId { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? CoursName { get; set; }
        public int? AssignementId { get; set; }
        public string? AssignementTitle { get; set; }
        public int? Grade { get; set; }
    }
}
