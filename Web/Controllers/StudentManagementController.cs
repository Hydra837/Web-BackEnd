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
        public async Task<ActionResult> DeleteEnrollment(int studentId, int courseId)
        {
            if (studentId <= 0 || courseId <= 0)
            {
                return BadRequest("Invalid student or course ID.");
            }

            await _studentManagementService.DeleteAsync(courseId); //  modifie pour supprimer (2 param) 
            return Ok("Student unenrolled from course successfully.");
        }

        [HttpGet("{id}")]
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
    }
}
