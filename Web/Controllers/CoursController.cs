using BLL.Interface;
using BLL.Models;
using BLL.Service;
using DAL.Data;
using DAL.Interface;
using DAL.Repository;
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

        [HttpPost(nameof(InsertUserCourse))]
        public IActionResult InsertUserCourse(int id, int id_cours)
        {
            if ((id < 0 ) && (id_cours > 0))
            {
                return BadRequest("Les données sont incorrecte");
            }
            else
            {
                _cours.InsertUserCourse(id, id_cours);
                return Ok();
            }
        }

        //[HttpGet(nameof(GetDispo))]
        ////public ActionResult<IEnumerable<CoursFORM>> GetDispo()
        ////{

        ////  //  return Ok(_cours.GetAllAvailble().Select(x => x.CoursToApi()));
        ////}
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CoursDTO>>> GetCourses()
        {
            var courses = await _cours.GetAllAsync();
            return Ok(courses);
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<CoursModel>>> GetAllAvailable()
        {
            try
            {
                var courses = await _cours.GetAvailableCoursesAsync();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging framework)
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("Cours")]
        public async Task<CoursModel> CreateAsync(CoursFORM entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            CoursModel coursData = entity.CoursToBLL(); ;
            await _cours.CreateAsync(coursData);

            // Assuming the entity gets updated with an ID or other information after saving
          ;

            return coursData;
        }
        
         [HttpGet("{id}")]
         public async Task<IActionResult> GetByIdAsync(int id)
            {
              var course = await _cours.GetByIdAsync(id);
              if (course == null)
                {
                 return NotFound();
                }
               return Ok(course);
            }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CoursModel cours)
        {
            if (cours == null)
            {
                return BadRequest("Course data is null");
            }

            if (id != cours.Id)
            {
                return BadRequest("Course ID mismatch");
            }

            try
            {
                await UpdateCourse(id, cours);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Course not found");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();
        }
   


    }
}
