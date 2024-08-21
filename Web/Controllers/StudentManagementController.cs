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
    public class StudentManagementController : ControllerBase
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
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "CoursFORM is null.",
                    Details = "Please provide a valid CoursFORM object."
                });
            }

            try
            {
                var coursModel = coursFORM.CoursToBLL();
                await _studentManagementService.CreateAsync(coursModel);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Course created successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while creating the course.",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete(nameof(Delete))]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid ID.",
                    Details = "ID must be a positive number."
                });
            }

            try
            {
                await _studentManagementService.DeleteAsync(id);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while deleting.",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("DeleteEnrollment")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteEnrollment(int studentId, int courseId)
        {
            if (studentId <= 0 || courseId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid student or course ID.",
                    Details = "Both IDs must be positive numbers."
                });
            }

            try
            {
                await _studentManagementService.DeleteAsync1(studentId, courseId);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Student unenrolled from course successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while unenrolling the student.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CoursDTO>>> GetAllCourseByUser(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid student ID.",
                    Details = "ID must be a positive number."
                });
            }

            try
            {
                var courses = await _studentManagementService.GetByIdAsync(id);
                var courseDtos = courses.CoursToApi();
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Courses = courseDtos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while retrieving the courses.",
                    Details = ex.Message
                });
            }
        }

        [HttpPost(nameof(InsertUserCourse))]
        [AllowAnonymous]
        public async Task<ActionResult> InsertUserCourse(int id, int courseId)
        {
            if (id <= 0 || courseId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid student or course ID.",
                    Details = "Both IDs must be positive numbers."
                });
            }

            try
            {
                await _studentManagementService.InsertUserCoursAsync(id, courseId);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "User enrolled in course successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while enrolling the user in the course.",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("InsertProf")]
        [Authorize(Roles = "Professeur,Admin")]
        public async Task<ActionResult> InsertUserAsync(int id, int courseId)
        {
            if (id <= 0 || courseId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid student or course ID.",
                    Details = "Both IDs must be positive numbers."
                });
            }

            try
            {
                await _studentManagementService.InsertUserCoursAsync(id, courseId);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "User enrolled in course successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while enrolling the user in the course.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("GetTeacher/{teacherId}")]
        [Authorize]
        public async Task<IActionResult> GetTeacherName(int teacherId)
        {
            if (teacherId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid teacher ID.",
                    Details = "ID must be a positive number."
                });
            }

            try
            {
                var teacher = await _studentManagementService.GetTeacherName(teacherId);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Teacher = teacher
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while retrieving the teacher information.",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("RemoveTeacher")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveTeacherFromCourse(int teacherId, int courseId)
        {
            if (teacherId <= 0 || courseId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid teacher or course ID.",
                    Details = "Both IDs must be positive numbers."
                });
            }

            try
            {
                await _studentManagementService.Deleteteacher(teacherId, courseId);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Teacher removed from course successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while removing the teacher from the course.",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("UpdateTeacher")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTeacherToCourse(int teacherId, int courseId)
        {
            if (teacherId <= 0 || courseId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid teacher or course ID.",
                    Details = "Both IDs must be positive numbers."
                });
            }

            try
            {
                await _studentManagementService.UpdateTeacherToCourse(teacherId, courseId);
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Teacher updated for the course successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while updating the teacher for the course.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("userAssignment/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserAssignementDTO>>> GetUserAssignments(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid user ID.",
                    Details = "ID must be a positive number."
                });
            }

            try
            {
                var assignments = await _studentManagementService.GetuserResult(userId);
                var assignmentDTOs = assignments.Select(a => a.TouserAssignmentDTO());
                return Ok(new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Assignments = assignmentDTOs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while retrieving user assignments.",
                    Details = ex.Message
                });
            }
        }
        [HttpGet("course/{courseId}/assignments")]
        public async Task<IActionResult> GetAllUsersAssignmentsGradesForCourse(int courseId)
        {
            try
            {
                var result = await _studentManagementService.GetAllUsersAssignmentsGradesForCourseAsync(courseId);
                if (result == null || result.Count == 0)
                {
                    return NotFound("Aucune donnée trouvée pour le cours spécifié.");
                }
               IEnumerable<UserAssignementDTO> a = result.Select( x => x.TouserAssignmentDTO());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Une erreur est survenue lors du traitement de la demande.");
            }
        }
    }
}
