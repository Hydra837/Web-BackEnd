using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using BLL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpPost("Register")]
        [Authorize]
        public IActionResult Register([FromBody] UsersFORM user)
        {
            if (user == null)
            {
                return BadRequest(new { Message = "Les données de l'utilisateur ne peuvent pas être nulles." });
            }

            try
            {
                _userService.Register(user.Prenom, user.Nom);
                return Ok(new { Message = "Utilisateur enregistré avec succès." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de l'enregistrement.", Details = ex.Message });
            }
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                var userModels = users.Select(user => user.BllAccessToApi());
                return Ok(userModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de la récupération des utilisateurs.", Details = ex.Message });
            }
        }

        [HttpGet("GetById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { Message = "Utilisateur non trouvé." });
                }
                return Ok(user.BllAccessToApi());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de la récupération de l'utilisateur.", Details = ex.Message });
            }
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] UsersFORM user)
        {
            if (user == null)
            {
                return BadRequest(new { Message = "Les données de l'utilisateur ne peuvent pas être nulles." });
            }

            var existingUser = await _userService.GetUsersByPseudo(user.Pseudo);
            if (existingUser != null)
            {
                return Conflict(new { Message = "Ce pseudo est déjà pris." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Les données de l'utilisateur ne sont pas valides.", Details = ModelState });
            }

            try
            {
                var model = user.BllAccessToApi1();
                await _userService.CreateAsync(model);
                return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de la création de l'utilisateur.", Details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UsersFORM user)
        {
            if (user == null)
            {
                return BadRequest(new { Message = "Les données de l'utilisateur ne peuvent pas être nulles." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Les données de l'utilisateur ne sont pas valides.", Details = ModelState });
            }

            try
            {
                var model = user.BllAccessToApi1();
                await _userService.UpdateAsync(id, model);
                return Ok(new { Message = "Utilisateur mis à jour avec succès." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Utilisateur non trouvé." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de la mise à jour de l'utilisateur.", Details = ex.Message });
            }
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Utilisateur non trouvé." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de la suppression de l'utilisateur.", Details = ex.Message });
            }
        }

        [HttpGet("GetAllCourseEachCourse")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersCoursesAsync()
        {
            try
            {
                var userCourseDetailsData = await _userService.GetUsersCoursesAsync();
                var userCoursDetailsDTOs = userCourseDetailsData.Select(x => x.ToApiCoursDetailsDTO());
                return Ok(userCoursDetailsDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de la récupération des détails des cours des utilisateurs.", Details = ex.Message });
            }
        }

        [HttpGet("pseudo/{pseudo}")]
        [Authorize]
        public async Task<IActionResult> GetUserByPseudo(string pseudo)
        {
            try
            {
                var user = await _userService.GetUsersByPseudo(pseudo);
                if (user == null)
                {
                    return NotFound(new { Message = "Utilisateur non trouvé." });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de la récupération de l'utilisateur.", Details = ex.Message });
            }
        }

        [HttpGet("GetUserRole/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserRole(int userId)
        {
            try
            {
                var role = await _userService.GetUserRoleAsync(userId);
                if (role == null)
                {
                    return NotFound(new { Message = "Rôle de l'utilisateur non trouvé." });
                }
                return Ok(new { UserId = userId, Role = role });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de la récupération du rôle de l'utilisateur.", Details = ex.Message });
            }
        }

        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userService.GetUsersByPseudo(userId);
                if (user == null)
                {
                    return NotFound(new { Message = "Utilisateur non trouvé." });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de la récupération de l'utilisateur actuel.", Details = ex.Message });
            }
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> SearchUsers([FromQuery] string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return BadRequest(new { Message = "Le terme de recherche est requis." });
            }

            try
            {
                var users = await _userService.SearchUsers(search);
                var usersDTO = users.Select(u => u.BllAccessToApi());
                return Ok(usersDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur est survenue lors de la recherche des utilisateurs.", Details = ex.Message });
            }
        }
    }
}
