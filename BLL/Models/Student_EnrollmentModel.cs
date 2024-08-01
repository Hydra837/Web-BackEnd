using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class Student_EnrollmentModel
    {
        public int Id { get; set; }
        public int Grade {  get; set; }
        public int UserId { get; set; }
        public int CoursId { get; set; }
    }
}
