using DAL.Data;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IStudent_EnrollmentRepository
    {
        Task InsertStudentCourseAsync(UsersData user, CoursData course);
        Task<IEnumerable<CoursData>> CoursPourchaqueEtuAsync(UsersData user);
        public Task InsertStudentCourseAsync2(int user, int course);
        public Task<IEnumerable<UsersData>> GetAlluserBycourse(int id);
    }
}
