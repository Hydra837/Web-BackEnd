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
        // POST: api/grade
        [HttpPost]
        public async Task<ActionResult> Create(GradeForm gradeForm)
        {
            // Validation des entrées
            if (gradeForm == null)
            {
                return BadRequest("Les données de la note ne peuvent pas être null.");
            }

            if (gradeForm.UserId <= 0 || gradeForm.AssignementsId <= 0)
            {
                return BadRequest("L'ID utilisateur et l'ID de l'assignement doivent être des nombres positifs.");
            }

            try
            {
                var gradeData = gradeForm.ToGradeForm();

                // Appel au service pour insérer les données
                await _gradeService.InsertGradeAsync(gradeData);

                // Retourner la réponse de succès avec un code de statut 201 (Created)
                return CreatedAtAction(nameof(GetById), new { id = gradeData.Id }, gradeForm);
            }
            catch (ArgumentException ex)
            {
                // Gérer les exceptions liées aux arguments invalides
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Gérer toutes les autres exceptions
                return StatusCode(500, $"Une erreur inattendue s'est produite: {ex.Message}");
            }
        }


        // PUT: api/grade/{id}
        //[HttpPut("{id}")]
        //public async Task<ActionResult> Update(int id, GradeForm gradeForm)
        //{
        //    var existingGrade = await _gradeService.GetByUserIdAsync(id);
        //    if (existingGrade == null)
        //    {
        //        return NotFound();
        //    }

        //    var gradeData = gradeForm.ToGradeForm();
        //    gradeData.Id = id; // Assurez-vous que l'ID est correct lors de la mise à jour
        //    await _gradeService.ud (gradeData);
        //    return NoContent();
        //}

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
        [HttpPut("upgrade/{id}")]
        public async Task<ActionResult> UpgradeGrade(int id, [FromBody] int newGrade)
        {
            var existingGrade = await _gradeService.GetByUserIdAsync(id);
            if (existingGrade == null)
            {
                return NotFound();
            }

            // Assuming the grade is represented by an integer and we only need to update the grade value
            await _gradeService.UpdateGradeAsync(id, newGrade);

            return NoContent();
        }
    }
}
