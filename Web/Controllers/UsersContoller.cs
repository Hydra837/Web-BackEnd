using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using BLL.Interface;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Service;
using Web.Mapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IusersService _userService;

        public UsersController(IusersService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        //[Authorize]
        [AllowAnonymous]
        [Route(nameof(Register))]
        public ActionResult Register( UsersFORM user)
        {
            if (user == null)
            {
                return BadRequest(new { Message = "User cannot be null" });
            }

            try
            {
                _userService.Register(user.Prenom, user.Nom);
                return Ok(new { Message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception here if you have a logging mechanism
                return StatusCode(500, new { Message = "An error occurred during registration", Details = ex.Message });
            }
        }

        [HttpGet("GetAll")]
        //   [Authorize]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UsersDTO>>> GetAll()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                var userModels = users.Select(user => user.BllAccessToApi());
                return Ok(userModels);
            }
            catch (Exception ex)
            {
                // Log the exception here if you have a logging mechanism
                return StatusCode(500, new { Message = "An error occurred while retrieving users", Details = ex.Message });
            }
        }

        [HttpGet("GetById/{id}")]
        // [Authorize]
        [AllowAnonymous]
        public async Task<ActionResult<UsersDTO>> GetById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { Message = "User not found" });
                }
                var userModel = user.BllAccessToApi();
                return Ok(userModel);
            }
            catch (Exception ex)
            {
                // Log the exception here if you have a logging mechanism
                return StatusCode(500, new { Message = "An error occurred while retrieving the user", Details = ex.Message });
            }
        }

        [HttpPost("Create")]
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<ActionResult> Create( UsersFORM user)
        {
            if (user == null)
            {
                return BadRequest(new { Message = "User cannot be null" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                UsersModel model = user.BllAccessToApi1();
                await _userService.CreateAsync(model);
                return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                // Log the exception here if you have a logging mechanism
                return StatusCode(500, new { Message = "An error occurred while creating the user", Details = ex.Message });
            }
        }


        // PUT: api/users/{id}
        [HttpPut("{id}")]
        //  [Authorize]
        [AllowAnonymous]
        public async Task<ActionResult> Update(int id,UsersFORM user)
        {
            if (user == null)
            {
                return BadRequest(new { Message = "User data is null" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                UsersModel model = user.BllAccessToApi1();
                await _userService.UpdateAsync(id, model);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "User not found" });
            }
            catch (Exception ex)
            {
                // Log the exception here if you have a logging mechanism
                return StatusCode(500, new { Message = "An error occurred while updating the user", Details = ex.Message });
            }
        }
        // DELETE: api/users/{id}
        [HttpDelete("Delete/{id}")]
        //   [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "User not found" });
            }
        }
        [HttpGet("GetAllCourseEachCourse")]
        // [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IEnumerable<UserCoursDetailsDTO>> GetUsersCoursesAsync()
        {
            try
            {
                // Récupération des détails des cours depuis le service
                IEnumerable<UserCourseDetailsModel> userCourseDetailsData = await _userService.GetUsersCoursesAsync();

                // Conversion des modèles en DTOs
                IEnumerable<UserCoursDetailsDTO> userCoursDetailsDTOs = userCourseDetailsData
                    .Select(x => x.ToApiCoursDetailsDTO());

                return userCoursDetailsDTOs;
            }
            catch (KeyNotFoundException ex)
            {
                // Log l'erreur ou gérer le cas où aucune donnée n'est trouvée
                // Par exemple, loggez l'erreur dans un fichier ou un système de log
                // Vous pouvez également retourner une réponse HTTP appropriée si vous êtes dans un contrôleur
               // Log.Error("Détails de l'erreur: ", ex); // Exemple de logging
                throw; // Relance l'exception pour être capturée ailleurs si nécessaire
            }
            catch (Exception ex)
            {
                // Gérer d'autres exceptions qui pourraient survenir
                // Log l'erreur et retournez un message d'erreur général
                // Log.Error("Détails de l'erreur: ", ex); // Exemple de logging
                throw new ApplicationException("Une erreur inattendue est survenue lors de la récupération des données.", ex);
            }

        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    try
        //    {
        //        await  _userService.DeleteAsync(id);
        //        return NoContent(); // 204 No Content
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return NotFound(new { message = "User not found" }); // 404 Not Found
        //    }
        //    catch (ArgumentOutOfRangeException)
        //    {
        //        return BadRequest(new { message = "Invalid user ID" }); // 400 Bad Request
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "An error occurred while deleting the user", details = ex.Message }); // 500 Internal Server Error
        //    }
        //}
        [HttpGet("pseudo/{pseudo}")]
        //  [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserByPseudo(string pseudo)
        {
            try
            {
                var user = await _userService.GetUsersByPseudo(pseudo);
                if (user == null)
                {
                    return NotFound(new { Message = "l'utilisateur est introuvable." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                return StatusCode(500, new { Message = "Une erreur à été détecté.", Details = ex.Message });
            }
        }
        [HttpGet("GetUserRole/{userId}")]
        //   [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserRole(int userId)
        {
            var role = await _userService.GetUserRoleAsync(userId);

            if (role == null)
            {
                return NotFound("L'utilisateur n'a pas été trouvé.");
            }

            return Ok(new { UserId = userId, Role = role });
        }

        [HttpGet("GetCurrentUser")]
        // [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Extrait l'ID de l'utilisateur du token
            var user = await _userService.GetUsersByPseudo(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}

