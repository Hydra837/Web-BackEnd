using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.Interface;
using Web.Models;
using Web.Mapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Service;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentEnrollmentController : ControllerBase
    {
        private readonly IStudentEnrollmentService _studentEnrollmentService;

        public StudentEnrollmentController(IStudentEnrollmentService studentEnrollmentService)
        {
            _studentEnrollmentService = studentEnrollmentService;
        }

        [HttpGet("GetAllCoursesForStudent/{studentId}")]
        public async Task<ActionResult<IEnumerable<CoursFORM>>> GetAllCoursesForStudent(int studentId)
        {
            if (studentId <= 0)
                return BadRequest("Invalid student ID.");

            var courses = await _studentEnrollmentService.CoursPourchaqueEtuAsync(studentId);
            if (!courses.Any())
                return NotFound("No courses found for the given student.");

            return Ok(courses.Select(x => x.CoursToApi()));
            
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(int studentId, int courseId)
        {
            if (studentId <= 0 || courseId <= 0)
                return BadRequest("Invalid student or course ID.");

            try
            {
                await _studentEnrollmentService.InsertStudentCourseAsync2(studentId, courseId);
                return Ok("Student enrolled successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging framework)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
        //[HttpPost("Insert2")]
        //public async Task<IActionResult> Insert2(UsersFORM studentId, CoursFORM courseId)
        //{
        //    if (studentId  is null || courseId is null )
        //        return BadRequest("Invalid student or course ID.");

        //    try
        //    {
        //        UsersModel user = studentId.BllAccessToApi();
        //        CoursModel cours = courseId.CoursToBLL();
        //        await _studentEnrollmentService.InsertStudentCourseAsync(user, cours);
        //        return Ok("Student enrolled successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (e.g., using a logging framework)
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        //    }
        //}

        //[HttpPost("Insert")]
        //public async Task<IActionResult> Insert1([FromBody] UsersFORM usr, [FromQuery] CoursFORM cours)
        //{
        //    // Valider l'état du modèle
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    // Valider que usr et cours ne sont pas null
        //    if (usr == null || cours == null)
        //        return BadRequest("Données d'étudiant ou de cours invalides.");

        //    try
        //    {
        //        // Convertir les formulaires en modèles BLL
        //        UsersModel userm = usr.BllAccessToApi();
        //        CoursModel courss = cours.CoursToBLL();

        //        // S'assurer que les modèles convertis sont valides
        //        if (userm == null || courss == null)
        //            return BadRequest("Erreur de conversion des formulaires en modèles.");

        //        // Tenter d'insérer l'inscription de l'étudiant au cours
        //        await _studentEnrollmentService.InsertStudentCourseAsync(userm, courss);
        //        return Ok("Étudiant inscrit avec succès.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Logger l'exception (décommentez la ligne du logger si vous avez configuré un logger)
        //        // _logger.LogError(ex, "Erreur lors de l'inscription de l'étudiant.");

        //        // Retourner une réponse 500 Internal Server Error
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Erreur interne du serveur : {ex.Message}");
        //    }
        //}
        [HttpGet("GetalluserCourse/{id}")]
        public async Task<IActionResult> GetAllUsersByCourse(int id)
        {
            try
            {
                // Appel du service pour obtenir les utilisateurs inscrits à ce cours
                var users = await _studentEnrollmentService.GetAlluserBycourse(id);

                // Vérifier si des utilisateurs ont été trouvés
                if (users == null || !users.Any())
                {
                    return NotFound(new { Message = "Aucun utilisateur trouvé pour ce cours." });
                }

                return Ok(users);
            }
            catch (ArgumentException ex)
            {
                // Gérer les erreurs d'argument
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Gérer les erreurs internes du serveur
                return StatusCode(500, new { Message = "Une erreur interne est survenue.", Details = ex.Message });
            }
        }
        //[HttpGet("AllcoursStudent/{id}")]
        //public async Task<ActionResult<IEnumerable<CoursModel>>> GetCoursesByUser(int id)
        //{
        //    // Appel au service BLL pour obtenir les cours de l'utilisateur
        //    var courses = await _studentEnrollmentService.GetallByUser(id);

        //    if (courses == null)
        //    {
        //        return NotFound(); // Retourne 404 si aucun cours trouvé
        //    }

        //    return Ok(courses); // Retourne 200 OK avec la liste des cours
        //}
        [HttpGet("EnrolledStudent/{id}")]
        public async Task<IActionResult> GetEnrolledStudentCourses(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid student ID.");
            }

            try
            {
                IEnumerable<CoursModel> courses = await _studentEnrollmentService.EnrolledStudentAsync(id);

                if (courses == null)
                {
                    return NotFound("No courses found for the given student ID.");
                }

                return Ok(courses);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error retrieving enrolled courses for student: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                await _studentEnrollmentService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        //[HttpPut]
        //public async Task<ActionResult> UpdateGrade([FromBody] EnrollementDTO enrollmentDto)
        //{
        //    if (enrollmentDto == null)
        //        return BadRequest("Enrollment data is null.");

        //    try
        //    {
        //        var enrollmentModel = enrollmentDto;// Map from DTO to BLL model
        //        await _studentEnrollmentService.UpdateGrade(enrollmentModel);
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<EnrollementDTO>> GetByCourseAsync(int courseId)
        {
            try
            {
                var enrollmentModel = await _studentEnrollmentService.GetByCourseAsync(courseId);

                if (enrollmentModel == null)
                    return NotFound();

                var enrollmentDto = enrollmentModel.ToEnrollementAPI(); // Map to DTO
                return Ok(enrollmentDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<EnrollementDTO>> GetByUserIdAsync(int userId)
        {
            try
            {
                var enrollmentModel = await _studentEnrollmentService.GetByUserIdAsync(userId);

                if (enrollmentModel == null)
                    return NotFound();

                var enrollmentDto = enrollmentModel.ToEnrollementAPI(); // Map to DTO
                return Ok(enrollmentDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("UpdateGrade")]
        public async Task<IActionResult> UpdateGrade(int id, int grade)
        {
            if (id <= 0 || grade < 0)
            {
                return BadRequest("Invalid student ID or grade.");
            }

            try
            {
                var result = await _studentEnrollmentService.UpdateGrade(id, grade);
                if (result)
                {
                    return Ok("Grade updated successfully.");
                }

                return NotFound("Enrollment not found.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error.");
            }
        }

    }
}
