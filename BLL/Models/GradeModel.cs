using DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class GradeModel
    {
        
        public int Id { get; set; }
        public int Grade { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public virtual UsersData User { get; set; }
    }
}
