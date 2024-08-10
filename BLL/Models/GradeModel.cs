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
            public int UserId { get; set; }
            public int AssignementsId { get; set; }
            public int Grade { get; set; }

            // Conversion de GradeModel en GradeData
            public GradeData ToGradeData()
            {
                return new GradeData
                {
                    Id = Id,
                    UserId = this.UserId,
                    AssignementsId = this.AssignementsId,
                    Grade = this.Grade
                };
            }
        


    }
}
