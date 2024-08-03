using BLL.Interface;
using BLL.Mapper;
using BLL.Models;
using DAL.Data;
using DAL.Interface;
using DAL.Mapper;
using DAL.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class UsersService : IusersService
    {
        private readonly IusersRepository _userRepository;
        private IusersRepository _user;

        public UsersService(IusersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateAsync(UsersModel entity)
        {
            var userData = entity.ToUserDAL();
            await _userRepository.AddAsync(userData);
        }

        public async Task UpdateAsync(int id, UsersModel entity)
        {
            var existingEntity = await _userRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Mettre à jour les propriétés de l'entité existante avec les nouvelles valeurs
            // Exemple :
            existingEntity.Nom = entity.Nom;
            existingEntity.Prenom = entity.Prenom;
            existingEntity.Roles = entity.Roles;
            // Ajouter d'autres propriétés ici

            await _userRepository.UpdateAsync(existingEntity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _userRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            await _userRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<UsersModel>> GetAllAsync()
        {
            var userData = await _userRepository.GetAllAsync();

            var usersModel = userData.Select(user => user.ToUserBLL());

            // Retourner les données mappées
            return usersModel;
        }


        public async Task<UsersModel> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            var userModel = user.ToUserBLL();
            return userModel;
        }

        public void Login(string username, string password)
        {
            if (username == null || password == null) return;
            _user.Login(username, password);

        }

        public void Logout()
        {

        }

        public void Register(string username, string password)
        {
            if (username == null || password == null) { return; }
            _user.Register(username, password);
        }

        public void Update(int id)
        {
            throw new NotImplementedException();
        }



        public void UserRole(string role)
        {
            if (role == null) { return; }
            _user.UserRole(role);
        }

        public void Create(CoursModel cours)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserCourseDetailsModel>> GetUsersCoursesAsync()
        {
           var users = await _userRepository.GetUsersCoursesAsync();
            IEnumerable<UserCourseDetailsModel> a = users.Select(u => u.ToUsersCoursDetailBLL());
            return a;
        }

        public async Task<UsersModel> GetUsersByPseudo(string pseudo)
        {
           
            var userData = await _userRepository.GetUsersByPseudo(pseudo);

          
            if (userData == null)
            {
                return null;
            }


            var userModel = userData.ToUserBLL();

            return userModel;
        }
    }
    
}
