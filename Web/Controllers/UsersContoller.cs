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
        [Route(nameof(Register))]
        public ActionResult Register([FromBody] UsersFORM user)
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
        public async Task<ActionResult> Create([FromBody] UsersFORM user)
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
                UsersModel model = user.BllAccessToApi();
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
        public async Task<ActionResult> Update(int id, [FromBody] UsersFORM user)
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
                UsersModel model = user.BllAccessToApi();
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
        public async Task<IEnumerable<UserCoursDetailsDTO>> GetUsersCoursesAsync()
        {
            // Récupérer les données depuis le repository DAL
           IEnumerable<UserCourseDetailsModel> userCourseDetailsData = await _userService.GetUsersCoursesAsync();
            IEnumerable<UserCoursDetailsDTO> test = userCourseDetailsData.Select( x => x.ToApiCoursDetailsDTO() );
      
            return test;
           
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
    }
}

