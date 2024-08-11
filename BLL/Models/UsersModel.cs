using DAL.Data;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class UsersModel
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Role { get; set; }
        public string? Password { get; set; }
        public string? Mail { get; set; }
        public string? Pseudo {  get; set; }
        public string? Salt { get; set; }
        public ICollection<CoursModel>? Cours { get; set; }
    }
}
