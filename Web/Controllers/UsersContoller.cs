using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using BLL.Interface;
using BLL.Models;
using BLL.Service;
using Web.Mapper;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersContoller : ControllerBase
    {
        private IusersService _userService;
        public UsersContoller(IusersService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Route(nameof(Register))]
        public ActionResult Register(UsersFORM user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            else
            {

                _userService.Register(user.Prenom, user.Nom);
                return Ok(); // Changer Password, Prenom en UserModel
            }
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<UsersDTO>>> GetAll()
        {
            // Récupérer tous les utilisateurs du service
            var users = await _userService.GetAllAsync();

            // Mapper les utilisateurs si nécessaire
            // Supposons que ToBLLMapper est la méthode de mappage pour convertir en UsersModel
            var userModels = users.Select(user => user.BllAccessToApi());

            // Retourner les utilisateurs mappés dans une réponse HTTP 200 OK
            return Ok(userModels);
        }

        // GET: api/users/{id}
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<UsersDTO>> GetById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                user.BllAccessToApi();
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "User not found" });
            }
        }

        // POST: api/users
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] UsersModel user)
        {
            if (user == null)
            {
                return BadRequest(new { Message = "User data is null" });
            }

            await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UsersModel user)
        {
            if (user == null)
            {
                return BadRequest(new { Message = "User data is null" });
            }

            try
            {
                await _userService.UpdateAsync(id, user);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "User not found" });
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

