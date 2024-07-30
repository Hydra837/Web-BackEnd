﻿using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IStudent_Management  : IcrudRepository
    {
        public IEnumerable<CoursData> GetAllCourseByUser(int id);
        public void InsertUserCours(int id, int id_cours);
        public Task DeleteAsync(int id);
        public Task CreateAsync(UsersData usersData);
      
    }
}
