using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.Interface;
using Web.Models;
using BLL.Models;
using Web.Mapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Service;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentManagementController :ControllerBase
    {
        private readonly IStudentManagmentService _studentManagementService;

        public StudentManagementController(IStudentManagmentService studentManagementService)
        {
            _studentManagementService = studentManagementService;
        }

        [HttpPost(nameof(Create))]
        [Authorize(Roles = "Professeur,Admin")]
      
        public async Task<ActionResult> Create(CoursFORM coursFORM)
        {
            if (coursFORM == null)
            {
                return BadRequest("CoursFORM is null.");
            }

            var coursModel = coursFORM.CoursToBLL();
            await _studentManagementService.CreateAsync(coursModel);
            return Ok("Course created successfully.");
        }

        [HttpDelete(nameof(Delete))]
        [Authorize(Roles = "Admin")]
       
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID.");
            }

            await _studentManagementService.DeleteAsync(id);
            return Ok("Deleted successfully.");
        }

        [HttpDelete("DeleteEnrollment")]
        [Authorize(Roles = "Admin")]
       
        public async Task<ActionResult> DeleteEnrollment(int studentId, int courseId)
        {
            if (studentId <= 0 || courseId <= 0)
            {
                return BadRequest("Invalid student or course ID.");
            }

            await _studentManagementService.DeleteAsync1(studentId , courseId); //  modifie pour supprimer (2 param) 
            return Ok("Student unenrolled from course successfully.");
        }

        [HttpGet("{id}")]
         [Authorize]
     
        public async Task<ActionResult<IEnumerable<CoursDTO>>> GetAllCourseByUser(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid student ID.");
            }

            var courses = await _studentManagementService.GetByIdAsync(id);
            var courseDtos = courses.CoursToApi();
            return Ok(courseDtos);
        }

        [HttpPost(nameof(InsertUserCourse))]
        //    [Authorize]
        [AllowAnonymous]
        public async Task<ActionResult> InsertUserCourse(int id, int courseId)
        {
            if (id <= 0 || courseId <= 0)
            {
                return BadRequest("Invalid student or course ID.");
            }

            //await
            //   _studentManagementService.InsertUserCours(id, courseId);
            return Ok("User enrolled in course successfully.");
        }
        [HttpPost("InsertProf")]
          [Authorize(Roles = "Professeur,Admin")]
        public async Task<ActionResult> InsertUserAsync(int id, int courseId)
        {
            try
            {
                await _studentManagementService.InsertUserCoursAsync(id, courseId);
                return Ok(new { Message = "L'utilisateur a été inscrit au cours avec succès." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Une erreur interne est survenue.", Details = ex.Message });
            }
        }
        [HttpGet("GetTeacher/{teacherId}")]
        [Authorize]
        public async Task<IActionResult> GetTeacherName(int teacherId)
        {
            try
            {
                var teacher = await _studentManagementService.GetTeacherName(teacherId);
                return Ok(teacher);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
        [HttpDelete("RemoveTeacher")]
          [Authorize(Roles = "Admin")]
  
        public async Task<IActionResult> RemoveTeacherFromCourse(int teacherId, int courseId)
        {
            try
            {
                await _studentManagementService.Deleteteacher(teacherId, courseId);
                return Ok(new { Message = "Teacher removed from course successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPut("UpdateTeacher")]
        [Authorize(Roles ="Admin")]
        // [Authorize(Roles = "Professeur,Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateTeacherToCourse(int teacherId, int courseId)
        {
            try
            {
                await _studentManagementService.UpdateTeacherToCourse(teacherId, courseId);
                return Ok(new { Message = "Teacher updated for the course successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("userAssignment/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserAssignementDTO>>> GetUserAssignments(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("L'identifiant de l'utilisateur doit être positif.");
            }

            try
            {
                // Get user assignments from the service
                IEnumerable<UserAssignementsModel> assignments = await _studentManagementService.GetuserResult(userId);

                // Map the model to DTO
                IEnumerable<UserAssignementDTO> assignmentDTOs = assignments.Select(a => a.TouserAssignmentDTO());

                return Ok(assignmentDTOs);
            }
            catch (Exception ex)
            { 
                return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur est survenue lors de la récupération des assignements.");
            }
        }
    }
}
