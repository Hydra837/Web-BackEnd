using BLL.Models;
using DAL.Data;
using System.IO;
using Web.Models;

namespace Web.Mapper
{
    public static class Mapper
    {
        internal static CoursDTO CoursToApi(this CoursModel data)
        {
            return new CoursDTO ()
            {
                Id = data.Id,
                Nom = data.Nom,
                DateDebut = data.DateDebut,
                DateFin = data.DateFin,
                Disponible = data.Disponible,
                ProfesseurId = data.ProfesseurId,


            };
        }
        internal static CoursModel  CoursToBLL(this CoursFORM data)
        {
            return new CoursModel()
            {
              
                Description = data.Description,
                Nom = data.Nom,
                DateDebut = data.DateDebut,
                DateFin = data.DateFin,
               ProfesseurId = data.ProfesseurId,

    };
        }
        //internal static UserModel BllAccessToApi(this RoleaddFORM data) // AJOUTER ROLE ADD
        //{
        //    return new CoursModel()
        //    {
        //        Id = data.Id,
        //        Nom = data.Role
            

        //    };
        //}
        internal static UsersDTO BllAccessToApi(this UsersModel data)
        {
            return new UsersDTO()
            {
                Id = data.Id,
                Nom = data.Nom,
                Password = data.Password,
                Prenom = data.Prenom,
                Role = data.Role, 
                Mail = data.Mail,
                 Pseudo = data.Pseudo,
                coursDTOs = data.Cours?.Select(c => c.CoursToApi()).ToList()


            };
        }
        internal static UsersModel BllAccessToApi1(this UsersFORM data)
        {
            return new UsersModel()
            {
               // Id= data.Id,
                Nom = data.Nom,
                Password = data.Password,
                Prenom = data.Prenom,
               Role = data.Role, 
               Mail = data.Mail,
               Pseudo = data.Pseudo

            };
        }
        internal static UserCoursDetailsDTO ToApiCoursDetailsDTO(this UserCourseDetailsModel data)
        {
            return new UserCoursDetailsDTO()
            {
                UserNom = data.UserNom,
                UserPrenom = data.UserPrenom,  
                Disponible = data.Disponible,
                CoursNom = data.CoursNom,
                ProfNom = data.ProfNom,
                ProfPrenom = data.ProfPrenom, 
                coursId = data.CourseId,
                Grade = data.Grade,
               
            };
        }
        public static GradeDTO ToGradeDTO(this GradeModel gradeData)
        {
            return new GradeDTO
            {
                Id = gradeData.Id,
                Grade = gradeData.Grade,
                UserId = gradeData.UserId,
                AssignementsId = gradeData.AssignementsId,
              //  CourseId = gradeData.CourseId
            };
        }
        public static GradeModel ToGradeForm(this GradeForm gradeData)
        {
            return new GradeModel
            {
                AssignementsId = gradeData.AssignementsId,
                Grade = gradeData.Grade,
                UserId = gradeData.UserId,
                //CourseId = gradeData.CourseId
            };
        }
        public static Student_EnrollmentModel  ToEnrollementBLL(this EnrollementFORM dto)
        {
            if (dto == null)
                return null;

            return new Student_EnrollmentModel
            {
                Id = dto.Id,
                UserId = dto.UserId,
                CoursId = dto.CoursId,
                Grade = dto.Grade
               
            };
        }
        public static EnrollementDTO ToEnrollementAPI(this Student_EnrollmentModel dto)
        {
            if (dto == null)
                return null;

            return new EnrollementDTO
            {
                Id = dto.Id,
                UserId = dto.UserId,
                CoursId = dto.CoursId,
                Grade = dto.Grade

            };
        }
        public static AssignementsDTO ToAssignementsDTO(this AssignementsModel model)
        {
            if (model == null)
                return null;

            return new AssignementsDTO
            {
                Id = model.Id,
                Description = model.Description,
                CoursId = model.CoursId,
                IsAvailable = model.IsAvailable,
             //   Cours = model.Cours?.ToCoursDTO(),
              //  Grades = model.Grades?.Select(g => g .ToGradeDTO()).ToList()
            };
        }
        public static AssignementsModel ToAssignementsModel(this AssignementsFORM dto)
        {
            if (dto == null)
                return null;

            return new AssignementsModel
            {
                Id = dto.Id,
                Description = dto.Description,
                CoursId = dto.CoursId,
                IsAvailable = dto.IsAvailable,
                //Cours = dto.Cours?.ToCoursModel(),
                //Grades = dto.Grades?.Select(g => g.ToGradeModel()).ToList()
            };
        }

    }
}
