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
using Microsoft.EntityFrameworkCore;

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
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid student ID.",
                    Details = "The provided student ID must be a positive number."
                });
            }

            var courses = await _studentEnrollmentService.CoursPourchaqueEtuAsync(studentId);
            if (!courses.Any())
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "No courses found for the given student.",
                    Details = "Ensure that the student ID is correct and that the student is enrolled in courses."
                });
            }

            return Ok(courses.Select(x => x.CoursToApi()));
        }

        [HttpPost("Insert")]
        [Authorize ( Roles = "Admin" )]
        public async Task<IActionResult> InsertEnrollment([FromQuery] int studentId, [FromQuery] int courseId)
        {
            if (studentId <= 0 || courseId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid student or course ID.",
                    Details = "Both student and course IDs must be positive numbers."
                });
            }

            try
            {
                // Check if the student is already enrolled
                //bool isAlreadyEnrolled = await _studentEnrollmentService.IsUserEnrolledInCourseAsync(studentId, courseId);

                //if (isAlreadyEnrolled)
                //{
                //    return Conflict(new
                //    {
                //        StatusCode = 409,
                //        Message = "The student is already enrolled in this course.",
                //        Details = "Enrollment failed because the student is already enrolled in this course."
                //    });
                //}

                // Proceed with enrollment
                await _studentEnrollmentService.InsertStudentCourseAsync2(studentId, courseId);
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Enrollment successful."
                });
            }
            //catch (KeyNotFoundException ex)
            //{
            //    // Handle case where student or course is not found
            //    return NotFound(new
            //    {
            //        StatusCode = 404,
            //        Message = "Student or course not found.",
            //        Details = ex.Message
            //    });
            //}
            catch (InvalidOperationException ex)
            {
                // Handle any logical issues during enrollment
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Enrollment operation failed.",
                    Details = ex.Message
                });
            }
            //catch (Exception ex)
            //{
            //    // Log the error (using a logger instance)
            //   // _logger.LogError($"An error occurred during enrollment of student {studentId} in course {courseId}: {ex.Message}");

            //    // Return a 500 error with detailed information
            //    return StatusCode(500, new
            //    {
            //        StatusCode = 500,
            //        Message = "An error occurred during enrollment.",
            //        Details = "Please contact support if the issue persists."
            //    });
            //}
        }


        [HttpGet("GetalluserCourse/{id}")]
        [Authorize]
        public async Task<IActionResult> GetAllUsersByCourse(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid course ID.",
                    Details = "The provided course ID must be a positive number."
                });
            }

            try
            {
                var users = await _studentEnrollmentService.GetAlluserBycourse(id);

                if (users == null || !users.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "No users found for this course.",
                        Details = "Ensure that the course ID is correct and that users are enrolled in this course."
                    });
                }

                return Ok(users);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid argument.",
                    Details = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An internal server error occurred.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("EnrolledStudent/{id}")]
        [Authorize]
        public async Task<IActionResult> GetEnrolledStudentCourses(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid student ID.",
                    Details = "The provided student ID must be a positive number."
                });
            }

            try
            {
                IEnumerable<CoursModel> courses = await _studentEnrollmentService.EnrolledStudentAsync(id);

                if (courses == null || !courses.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "No courses found for the student.",
                        Details = "The student might not be enrolled in any courses."
                    });
                }

                return Ok(courses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An internal server error occurred.",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid ID.",
                    Details = "The provided ID must be a positive number."
                });
            }

            try
            {
                await _studentEnrollmentService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Enrollment not found.",
                    Details = "The enrollment with the specified ID does not exist."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An internal server error occurred.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("course/{courseId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EnrollementDTO>>> GetByCourseAsync(int courseId)
        {
            if (courseId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid course ID.",
                    Details = "The provided course ID must be a positive number."
                });
            }

            try
            {
                var enrollmentModel = await _studentEnrollmentService.GetByCourseAsync(courseId);

                if (enrollmentModel == null || !enrollmentModel.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "No enrollments found for this course.",
                        Details = "Ensure that the course ID is correct and that enrollments exist for this course."
                    });
                }

                var enrollmentDto = enrollmentModel.Select(x => x.ToEnrollementAPI());
                return Ok(enrollmentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An internal server error occurred.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EnrollementDTO>>> GetByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid user ID.",
                    Details = "The provided user ID must be a positive number."
                });
            }

            try
            {
                var enrollmentModel = await _studentEnrollmentService.GetByUserIdAsync(userId);

                if (enrollmentModel == null || !enrollmentModel.Any())
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "No enrollments found for this user.",
                        Details = "Ensure that the user ID is correct and that enrollments exist for this user."
                    });
                }

                var enrollmentDto = enrollmentModel.Select(x => x.ToEnrollementAPI());
                return Ok(enrollmentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An internal server error occurred.",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("UpdateGrade")]
        [Authorize(Roles = "Professeur,Admin")]
        public async Task<IActionResult> UpdateGrade(int id, int grade)
        {
            if (id <= 0 || grade < 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid student ID or grade.",
                    Details = "The provided student ID must be a positive number and the grade must be non-negative."
                });
            }

            try
            {
                var result = await _studentEnrollmentService.UpdateGrade(id, grade);
                if (result)
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Grade updated successfully."
                    });
                }

                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Enrollment not found.",
                    Details = "Ensure that the enrollment ID is correct."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An internal server error occurred.",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("UpdateGrades")]
        [Authorize(Roles = "Professeur,Admin")]
        public async Task<IActionResult> UpdateGrade(int idUsers, int idCours, int grade)
        {
            if (idUsers <= 0 || idCours <= 0 || grade < 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid input.",
                    Details = "User ID and course ID must be positive numbers, and the grade must be non-negative."
                });
            }

            try
            {
                var result = await _studentEnrollmentService.UpdateGradesAsync(idUsers, idCours, grade);
                if (result)
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "Grade updated successfully."
                    });
                }

                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Enrollment not found.",
                    Details = "Ensure that the user ID, course ID, and grade are correct."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An internal server error occurred.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("Etudiants")]
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
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving students.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("Professeurs")]
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
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving teachers.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("{coursId}/users")]
        public async Task<IActionResult> GetCoursWithUsers(int coursId)
        {
            if (coursId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid course ID.",
                    Details = "The provided course ID must be a positive number."
                });
            }

            try
            {
                var coursWithUsers = await _studentEnrollmentService.GetCoursWithUsersAsync1(coursId);
                if (coursWithUsers == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Course not found.",
                        Details = "Ensure that the course ID is correct."
                    });
                }

                return Ok(coursWithUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An internal server error occurred.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("{userId}/cours")]
        public async Task<IActionResult> GetUserWithCourses(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid user ID.",
                    Details = "The provided user ID must be a positive number."
                });
            }

            try
            {
                var userWithCourses = await _studentEnrollmentService.GetUserWithCoursesAsync1(userId);
                if (userWithCourses == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "User not found.",
                        Details = "Ensure that the user ID is correct."
                    });
                }

                return Ok(userWithCourses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An internal server error occurred.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("professors")]
        public async Task<IActionResult> GetAllProfessorsWithCourses()
        {
            try
            {
                var professorsWithCourses = await _studentEnrollmentService.GetAllProfessorsWithCoursesAsync();
                return Ok(professorsWithCourses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving professors.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("professors/{professorId}")]
        public async Task<IActionResult> GetProfessorWithCourses(int professorId)
        {
            if (professorId <= 0)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid professor ID.",
                    Details = "The provided professor ID must be a positive number."
                });
            }

            try
            {
                var professorWithCourses = await _studentEnrollmentService.GetProfessorWithCoursesAsync(professorId);
                if (professorWithCourses == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "Professor not found.",
                        Details = "Ensure that the professor ID is correct."
                    });
                }

                return Ok(professorWithCourses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An internal server error occurred.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("students")]
        public async Task<IActionResult> GetAllStudentsWithCourses()
        {
            try
            {
                var studentsWithCourses = await _studentEnrollmentService.GetAllStudentsWithCoursesAsync();
                return Ok(studentsWithCourses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving students.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("with-courses-assignments-grades")]
        public async Task<ActionResult<List<UsersDTO>>> GetUsersWithCoursesAssignmentsAndGrades()
        {
            try
            {
                var users = await _studentEnrollmentService.GetUsersWithCoursesAssignmentsAndGradesAsync();
                var userDtos = users.Select(x => x.BllAccessToApi());
                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "An error occurred while retrieving users.",
                    Details = ex.Message
                });
            }
        }
    }
}
