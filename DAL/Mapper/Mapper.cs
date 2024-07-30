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
            return new CoursData()
            {
                Id = (int)record["Id"],
                Nom = (string)record["Nom"],
                Disponible = (bool)record["Prenom"],
                date_debut = (DateTime)record["date_debut"],
                date_fin = (DateTime)record["date_fin"]
          
            };
        }
        internal static UsersData ToUser(this IDataRecord record)
        {
            return new UsersData()
            {
                Id = (int)record["Id"],
                Nom = (string)record["Nom"],
                Prenom = (string)record["Prenom"],
                Roles = (string)record["Roles"],
                Passwd = (string)record["Passwd"]

            };
        }
        internal static UserCourseDetailsData ToUsersCoursesBLL(this IDataRecord record)
        {
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


    }
}
