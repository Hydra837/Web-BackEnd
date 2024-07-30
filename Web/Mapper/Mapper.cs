using BLL.Models;
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
                Disponible = data.Disponible


            };
        }
        internal static CoursModel  CoursToBLL(this CoursFORM data)
        {
            return new CoursModel()
            {
              
                Nom = data.Nom,
                DateDebut = data.DateDebut,
                DateFin = data.DateFin


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
                Role = data.Roles
               


            };
        }
        internal static UsersModel BllAccessToApi(this UsersFORM data)
        {
            return new UsersModel()
            {
                Id= data.Id,
                Nom = data.Nom,
                Password = data.Password,
                Prenom = data.Prenom,
               Roles = data.Roles


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
               


            };
        }

    }
}
