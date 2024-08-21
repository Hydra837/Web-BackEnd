using BLL.Interface;
using BLL.Models;
using BLL.Service;
using DAL.Data;
using DAL.Interface;
using DAL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Mapper;
using Web.Models;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursController : ControllerBase
    {
        private readonly ICoursService _cours;

        public CoursController(ICoursService coursRepository)
        {
            _cours = coursRepository;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CoursDTO>>> GetCourses()
        {
            var courses = await _cours.GetAllAsync();
            return Ok(courses);
        }

        [HttpGet("available")]
         [Authorize]
      
        public async Task<ActionResult<IEnumerable<CoursModel>>> GetAllAvailable()
        {
            try
            {
                var courses = await _cours.GetAvailableCoursesAsync();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("Cours")]
        [Authorize(Roles = "Professeur,Admin")]
        public async Task<IActionResult> CreateAsync(CoursFORM entity)
        {
            if (entity == null)
            {
                return BadRequest("Les données du cours ne peuvent pas être nulles.");
            }

            CoursModel coursData = entity.CoursToBLL();

            try
            {
                await _cours.CreateAsync(coursData);
                return Ok(coursData); // Retourne le cours créé avec un code 200
            }
            catch (DbUpdateException ex) when (ex.InnerException != null)
            {
                // Vérifiez les messages d'erreur spécifiques pour les contraintes de clé primaire, unique, et clé étrangère
                if (ex.InnerException.Message.Contains("PRIMARY KEY constraint"))
                {
                    return StatusCode(409, "Le cours que vous essayez d'ajouter existe déjà."); // Conflit
                }
                else if (ex.InnerException.Message.Contains("FOREIGN KEY constraint"))
                {
                    return StatusCode(400, "Violation de contrainte de clé étrangère. Veuillez vérifier les données."); // Mauvaise requête
                }
                else if (ex.InnerException.Message.Contains("UNIQUE constraint"))
                {
                    return StatusCode(409, "Les données doivent être uniques. Une entrée similaire existe déjà."); // Conflit
                }
                // Autres exceptions
                return StatusCode(500, "Erreur serveur lors de l'ajout du cours."); // Erreur serveur
            }
            catch (Exception ex)
            {
                // Gestion des exceptions générales
                return StatusCode(500, $"Erreur serveur: {ex.Message}"); // Erreur serveur
            }
        }

        [HttpGet("{id}")]
        [Authorize]
    
        public async Task<IActionResult> GetByIdAsync(int id)
            {
              var course = await _cours.GetByIdAsync(id);
              if (course == null)
                {
                 return NotFound();
                }
               return Ok(course);
            }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Professeur,Admin")]
        public async Task<IActionResult> UpdateAsync(int id, CoursFORM coursFORM)
        {
            if (id <= 0)
            {
                return BadRequest("ID de cours invalide.");
            }

            if (coursFORM == null)
            {
                return BadRequest("Les données du cours sont vides.");
            }

            try
            {
                var coursModel = coursFORM.CoursToBLL();
                coursModel.Id = id;

                await _cours.UpdateAsync(id, coursModel);

                return Ok("Cours mis à jour avec succès.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound("Cours non trouvé.");
            }
            catch (DbUpdateException ex) when (ex.InnerException != null)
            {
                if (ex.InnerException.Message.Contains("PRIMARY KEY constraint"))
                {
                    return Conflict("Le cours que vous essayez d'ajouter existe déjà.");
                }
                else if (ex.InnerException.Message.Contains("FOREIGN KEY constraint"))
                {
                    return BadRequest("Violation de contrainte de clé étrangère.");
                }
                else if (ex.InnerException.Message.Contains("UNIQUE constraint"))
                {
                    return Conflict("Les données doivent être uniques.");
                }
                return StatusCode(500, "Erreur serveur.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur serveur: {ex.Message}");
            }
        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
     
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid course ID.");
            }

            try
            {

                await _cours.DeleteAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound("Course not found.");
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"Error: {ex.Message}");

     
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet("cours/professeur/{teacherId}")]
         [Authorize(Roles = "Professeur,Admin")]
      
        public async Task<ActionResult<IEnumerable<CoursModel>>> GetCoursesByTeacher(int teacherId)
        {
            try
            {
                var courses = await _cours.GetAllByTeacher(teacherId);
                if (courses == null)
                {
                    return NotFound("Aucun cours trouvé");

                }
                return Ok(courses); 
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, "Internal server error"); 
            }
        }
        [HttpGet("UnenrolledCourses/{studentId}")]
        [Authorize]
 
        public async Task<ActionResult<IEnumerable<CoursDTO>>> GetUnenrolledCourses(int studentId)
        {
            try
            {
                var courses = await _cours.GetUnenrolledCoursesAsync(studentId);
                IEnumerable<CoursDTO> cours = courses.Select(x => x.CoursToApi());

                if (courses == null || !courses.Any())
                {
                    return NotFound("Aucun cours trouvé");
                }

                return Ok(courses);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "Erreur. Veuillez réessayer");
            }
        }

        [HttpGet("search")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> SearchCours([FromQuery] string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return BadRequest("veuillez entrez un nom.");
            }

            var courses = await _cours.SearchCours(search);
            var coursesDTO = courses.Select(c => c.CoursToApi());
            return Ok(coursesDTO);
        }



    }
}
