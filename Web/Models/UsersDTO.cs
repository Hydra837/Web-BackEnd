﻿using DAL.Data;

namespace Web.Models
{
    public class UsersDTO
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string Role { get; set; }
        public string? Password { get; set; }
        public string? Pseudo { get; set; }
        public string? Mail { get; set; }
        public string? Salt { get; set; }
        public ICollection<CoursDTO>? coursDTOs { get; set; }
    }
}
