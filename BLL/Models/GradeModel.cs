using DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class GradeModel
    {


        public int Id { get; set; }

        [Range(0, 20, ErrorMessage = "La note doit être comprise entre 0 et 100.")]
        public int Grade { get; set; }

        public int UserId { get; set; }
       // public int CourseId { get; set; }
        public int AssignementsId { get; set; }
       
    }
}
