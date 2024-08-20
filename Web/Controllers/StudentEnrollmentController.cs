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
using Microsoft.AspNetCore.Authorization;
using Azure.Core.GeoJson;

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
        [Authorize]
 
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
        [Authorize(Roles = "Admin, Etudiant")]
        public async Task<IActionResult> Insert(int studentId, int courseId)
        {
   
            if (studentId <= 0 || courseId <= 0)
                return BadRequest("Invalid student or course ID.");

            try
            {
  
                //bool isEnrolled = await _studentEnrollmentService.IsUserEnrolledInCourseAsync(studentId, courseId);
                //if (isEnrolled)
                //{
                //    return BadRequest("L'utilisateur est dejà inscrit");
                //}

          
                await _studentEnrollmentService.InsertStudentCourseAsync2(studentId, courseId);

                return Ok("Student inscrit au cours correctement.");
            }
            catch (Exception ex)
            {
            
                Console.WriteLine($"Erreur durant l'enrollement: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Probleme interne: {ex.Message}");
            }
        }


        [HttpGet("GetalluserCourse/{id}")]
         [Authorize]
   
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
        
        [HttpGet("EnrolledStudent/{id}")]
         [Authorize]
  
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
                    return NotFound("Aucun cours n'a été trouvé.");
                }

                return Ok(courses);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error retrieving enrolled courses for student: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
       
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
   
        [HttpGet("course/{courseId}")]
        [Authorize]
   
        public async Task<ActionResult<IEnumerable<EnrollementDTO>>> GetByCourseAsync(int courseId)
        {
            try
            {
                var enrollmentModel = await _studentEnrollmentService.GetByCourseAsync(courseId);

                if (enrollmentModel == null)
                    return NotFound();

                var enrollmentDto = enrollmentModel.Select(x => x.ToEnrollementAPI()); // Map to DTO
                return Ok(enrollmentDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("user/{userId}")]
          [Authorize]
     
        public async Task<ActionResult<IEnumerable<EnrollementDTO>>> GetByUserIdAsync(int userId)
        {
            try
            {
                var enrollmentModel = await _studentEnrollmentService.GetByUserIdAsync(userId);

                if (enrollmentModel == null)
                    return NotFound();

                var enrollmentDto = enrollmentModel.Select( x => x.ToEnrollementAPI()); // Map to DTO
                return Ok(enrollmentDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("UpdateGrade")]
        [Authorize(Roles = "Professeur,Admin")]
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

        [HttpPut("UpdateGrades")]
        [Authorize(Roles = "Professeur,Admin")]
       
        public async Task<IActionResult> UpdateGrade(int idUsers, int idCours, int grade)
        {
            var result = await _studentEnrollmentService.UpdateGradesAsync(idUsers, idCours, grade);

            if (result)
            {
                return Ok();
            }

            return NotFound();
        }
        [HttpGet("Etudiants")]
        //[Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> GetStudentAllCourseAsync()
        {
            try
            {
                var studentsModel = await _studentEnrollmentService.GetStudentAllCourseAsync();
                return Ok(studentsModel);
            }
            catch (Exception ex)
            {
                // Enregistrer l'exception si nécessaire
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }
        [HttpGet("Professeurs")]
        //[Authorize(Roles = "Professeur,Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTeacherAllCourseAsync()
        {
            try
            {
                var teachersModel = await _studentEnrollmentService.GetTeacherAllCourseAsync();
                return Ok(teachersModel);
            }
            catch (Exception ex)
            {
                // Enregistrer l'exception si nécessaire
                return StatusCode(500, $"Une erreur s'est produite : {ex.Message}");
            }
        }
        [HttpGet("{coursId}/users")]
        public async Task<IActionResult> GetCoursWithUsers(int coursId)
        {
            var coursWithUsers = await _studentEnrollmentService.GetCoursWithUsersAsync1(coursId);
            if (coursWithUsers == null)
            {
                return NotFound();
            }
            return Ok(coursWithUsers);
        }
        [HttpGet("{userId}/cours")]
        public async Task<IActionResult> GetUserWithCourses(int userId)
        {
            var userWithCourses = await _studentEnrollmentService.GetUserWithCoursesAsync1(userId);
            if (userWithCourses == null)
            {
                return NotFound();
            }
            return Ok(userWithCourses);
        }
        [HttpGet("professors")]
        public async Task<IActionResult> GetAllProfessorsWithCourses()
        {
            var professorsWithCourses = await _studentEnrollmentService.GetAllProfessorsWithCoursesAsync();
            return Ok(professorsWithCourses);
        }

        [HttpGet("professors/{professorId}")]
        public async Task<IActionResult> GetProfessorWithCourses(int professorId)
        {
            var professorWithCourses = await _studentEnrollmentService.GetProfessorWithCoursesAsync(professorId);
            if (professorWithCourses == null)
            {
                return NotFound();
            }
            return Ok(professorWithCourses);
        }
        [HttpGet("students")]
        public async Task<IActionResult> GetAllStudentsWithCourses()
        {
            var studentsWithCourses = await _studentEnrollmentService.GetAllStudentsWithCoursesAsync();
            return Ok(studentsWithCourses);
        }
        [HttpGet("with-courses-assignments-grades")]
        public async Task<ActionResult<List<UsersDTO>>> GetUsersWithCoursesAssignmentsAndGrades()
        {
            var users = await _studentEnrollmentService.GetUsersWithCoursesAssignmentsAndGradesAsync();
            var userDtos = users.Select(x => x.BllAccessToApi());
            return Ok(userDtos);
        }
    }
}
