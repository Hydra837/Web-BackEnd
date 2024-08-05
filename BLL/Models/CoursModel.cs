﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class CoursModel
    {

        public int Id { get; set; }
        public string? Nom { get; set; }
        public bool Disponible { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string? Description { get; set; }
        public int? ProfesseurId { get; set; }


    }
}
