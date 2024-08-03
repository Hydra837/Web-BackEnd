using DAL.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mapper
{
    public static class Mapper
    {

        internal static CoursData ToCours(this IDataRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));
            return new CoursData()
            {
                Id = (int)record["Id"],
                Nom = (string)record["Nom"],
                Disponible = (bool)record["Prenom"],
                date_debut = (DateTime)record["date_debut"],
                date_fin = (DateTime)record["date_fin"], 
                Description = (string)record["Description"]
          
            };
        }
        internal static UsersData ToUser(this IDataRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            //// Convertir la valeur du champ "Roles" en UserRole
            //if (!Enum.TryParse<UserRole>((string)record["Roles"], out var userRole))
            //{
            //    // Gérer le cas où la valeur du rôle est invalide
            //    throw new ArgumentException($"Invalid role value: {record["Roles"]}");
            //}

            return new UsersData
            {
                Id = (int)record["Id"],
                Nom = (string)record["Nom"],
                Prenom = (string)record["Prenom"],
                Roles = (string)record["Roles"], // Utilisation de l'énumération UserRole
                Passwd = (string)record["Passwd"],
                Mail = (string)record["Mail"],
                Pseudo = (string)record["Pseudo"],
                Salt = record["Salt"] != DBNull.Value ? (string)record["Salt"] : null // Vérification de la valeur nulle pour Salt
            };
        }
    
    internal static UserCourseDetailsData ToUsersCoursesBLL(this IDataRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));
            return new UserCourseDetailsData()
            {
                UserId = (int)record["UserId"],
                UserPrenom = (string)record["UserPrenom"],
                UserNom = (string)record["UserNom"],
                ProfNom = (string)record["ProfNom"],
                ProfPrenom = (string)record["ProfPrenom"],
                CoursNom = (string)record["CoursNom"],
                Disponible = (bool)record["Disponible"]
                
            };
        }
        internal static GradeData ToGradeData(this IDataRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            return new GradeData()
            {
                Id = (int)record["Id"],
                Grade = (int)record["Grade"],
                UserId = (int)record["UserId"],
                CourseId = (int)record["CourseId"]
            };
        }


    }
}
