using BLL.Interface;
using BLL.Models;
using BLL.Service;
using DAL.Data;
using DAL.Interface;
using DAL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
         [Authorize(Roles ="Professeur,Admin")]
      
        public async Task<CoursModel> CreateAsync(CoursFORM entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
          //  CoursModel = _cours.CourseExistsAsync(entity.)
            CoursModel coursData = entity.CoursToBLL(); 

            await _cours.CreateAsync(coursData);

          ;

            return coursData;
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
        public async Task<IActionResult> UpdateAsync(int id,  CoursFORM coursFORM)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid course ID.");
            }

            if (coursFORM == null)
            {
                return BadRequest("Course data is null.");
            }

            try
            {

                var coursModel = coursFORM.CoursToBLL();
                coursModel.Id = id; 

                await _cours.UpdateAsync(id , coursModel);

                return Ok("Course updated successfully.");
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
                    return NotFound();
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
                    return NotFound("No unenrolled courses found for the student.");
                }

                return Ok(courses);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("search")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> SearchCours([FromQuery] string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return BadRequest("Search term is required.");
            }

            var courses = await _cours.SearchCours(search);
            var coursesDTO = courses.Select(c => c.CoursToApi());
            return Ok(coursesDTO);
        }



    }
}
