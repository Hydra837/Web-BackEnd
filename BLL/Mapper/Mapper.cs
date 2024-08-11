using BLL.Models;
using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mapper
{
    public static class Mapper
    {
        internal static CoursModel ToCoursBLL(this CoursData record)
        {
            return new CoursModel()
            {
                Id = record.Id,
                Nom = record.Nom,
                DateDebut = record.date_debut,
                DateFin = record.date_fin,
                Disponible = record.Disponible,
                Description = record.Description, 
                ProfesseurId = record.ProfesseurId
            };

        }
        internal static CoursData ToCoursDAL(this CoursModel record)
        {
#pragma warning disable CS8601 // Existence possible d'une assignation de référence null.
            return new CoursData()
            {
                Id = record.Id,
                Nom = record.Nom,
                date_debut = record.DateDebut,
                date_fin = record.DateFin,
                Disponible = record.Disponible,
                Description = record.Description,
                ProfesseurId = record.ProfesseurId, 
              
            };
#pragma warning restore CS8601 // Existence possible d'une assignation de référence null.

        }
        internal static UsersModel ToUserBLL(this UsersData record)
        {
            return new UsersModel()
            {
                Id = record.Id,
                Nom = record.Nom,
                Prenom = record.Prenom,
                Password = record.Passwd,
                Role= record.Roles,
                Pseudo = record.Pseudo,
                Mail = record.Mail,
                Cours = record.Courses?.Select(c => c.ToCoursBLL()).ToList()
            };

        }
        internal static UsersData ToUserDAL(this UsersModel record)
        {
            return new UsersData()
            {
               // Id = record.Id,
                Nom = record.Nom,
                Prenom = record.Prenom,
                Passwd = record.Password,
                Roles = record.Role, 
                Pseudo = record.Pseudo, 
                Mail = record.Mail
            };

        }
        internal static UserCourseDetailsModel ToUsersCoursDetailBLL(this UserCourseDetailsData record)
        {
            return new UserCourseDetailsModel()
            {
                UserId = record.UserId,
                Disponible = record.Disponible,
                CoursNom = record.CoursNom,
                ProfNom = record.ProfNom,
                ProfPrenom = record.ProfPrenom,
                UserNom = record.UserNom,
                UserPrenom = record.UserPrenom, 
                CourseId = record.CourseId,
                Grade = record.Grade,


            };
            
        }
        
        internal static GradeModel ToGradeModel(this GradeData record)
        {
            return new GradeModel()
            {
                Id = record.Id,
                Grade = record.Grade,
                UserId = record.UserId,
                //CourseId = record.CoursId , 
                AssignementsId = record.AssignementsId
            };
        }

  
        internal static GradeData ToGradeData(this GradeModel model)
        {
            return new GradeData()
            {
                Id = model.Id,
                Grade = model.Grade,
                UserId = model.UserId,
               // CourseId = model.CourseId, 
                AssignementsId = model.AssignementsId
            };
        }
        internal static Student_EnrollmentModel ToEnrollementBLL(this Student_EnrollementData model)
        {
            return new Student_EnrollmentModel()
            {
                Id = model.Id,
                Grade = model.Grade,
                UserId = model.UserId,
                CoursId = model.CoursId 
            };
        }
        internal static Student_EnrollementData ToEnrollementDAL(this Student_EnrollmentModel model)
        {
            return new Student_EnrollementData()
            {
                Id = model.Id,
                Grade = model.Grade,
                UserId = model.UserId,
                CoursId = model.CoursId 
            };
        }
        public static AssigementsData AssignementsToDAL(this AssignementsModel model)
        {
            return new AssigementsData()
            {
                Id = model.Id,
                Description = model.Description, // Mapping de la description
                CoursId = model.CoursId,
                IsAvailable = model.IsAvailable,
                Cours = model.Cours, // Attention : cette ligne suppose que le cours est déjà dans le bon format DAL
                Grades = model.Grades?.ToList() // Si la collection n'est pas null, on la transforme en List
            };
        }
        public static AssignementsModel ToBLL(this AssigementsData data)
        {
            return new AssignementsModel()
            {
                Id = data.Id,
                Description = data.Description,
                CoursId = data.CoursId,
                IsAvailable = data.IsAvailable,
                Cours = data.Cours,
                Grades = data.Grades?.ToList() 
            };
        }
    }
}
