using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class UserCourse
    {
        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
    }
}
