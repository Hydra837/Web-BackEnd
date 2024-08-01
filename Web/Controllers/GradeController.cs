using BLL.Interface;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.Mapper;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        // GET: api/grade/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GradeDTO>> GetById(int id)
        {
            var grade = await _gradeService.GetByUserIdAsync(id);
            if (grade == null)
            {
                return NotFound();
            }
            return Ok(grade.ToGradeDTO());
        }

        // GET: api/grade/course/{courseId}
        [HttpGet("course/{courseId}")]
        public async Task<ActionResult<IEnumerable<GradeDTO>>> GetByCourse(int courseId)
        {
            var grades = await _gradeService.GetByCourseAsync(courseId);
            return Ok(grades);
        }

        // GET: api/grade
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GradeDTO>>> GetAllGrades()
        {
            var grades = await _gradeService.GetAllGradesAsync();
            return Ok(grades.Select(g => g.ToGradeDTO()));
        }

        // POST: api/grade
        [HttpPost]
        public async Task<ActionResult> Create(GradeForm gradeForm)
        {
            var gradeData = gradeForm.ToGradeForm();
            await _gradeService.InsertGrade(gradeData.UserId, gradeData.Grade);
            return CreatedAtAction(nameof(GetById), new { id = gradeData.Id }, gradeForm);
        }

        // PUT: api/grade/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, GradeForm gradeForm)
        {
            var existingGrade = await _gradeService.GetByUserIdAsync(id);
            if (existingGrade == null)
            {
                return NotFound();
            }

            var gradeData = gradeForm.ToGradeForm();
            gradeData.Id = id; // Assurez-vous que l'ID est correct lors de la mise à jour
            await _gradeService.Update(id, gradeData.Grade);
            return NoContent();
        }

        // DELETE: api/grade/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingGrade = await _gradeService.GetByUserIdAsync(id);
            if (existingGrade == null)
            {
                return NotFound();
            }

            await _gradeService.DeleteAsync(id);
            return NoContent();
        }
    }
}
