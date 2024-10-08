﻿using BLL.Interface;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.Mapper;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]

        public async Task<ActionResult<IEnumerable<GradeDTO>>> GetById(int id)
        {
            var grades = await _gradeService.GetByUserIdAsync(id);
            if (grades == null || !grades.Any())
            {
                return NotFound();
            }
            var gradeDTOs = grades.Select(g => g.ToGradeDTO());
            return Ok(gradeDTOs);
        }


        // GET: api/grade/course/{courseId}
        [HttpGet("course/{courseId}")]
         [Authorize]
     
        public async Task<ActionResult<IEnumerable<GradeDTO>>> GetByCourse(int courseId)
        {
            var grades = await _gradeService.GetByCoursesAsync(courseId);
            return Ok(grades);
        }

        // GET: api/grade
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GradeDTO>>> GetAllGrades()
        {
            var grades = await _gradeService.GetAllGradesAsync();
            return Ok(grades.Select(g => g.ToGradeDTO()));
        }

      
        [HttpPost]
        [Authorize(Roles ="Admin,Professeur")]
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
        // [Authorize(Roles ="Admin")]
        [Authorize(Roles ="Admin,Professeur")]
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
        [Authorize(Roles="Professeur,Admin")]
        public async Task<ActionResult> UpgradeGrade(int id, [FromBody] int newGrade)
        {
            // Validation de la note
            if (newGrade < 0 || newGrade > 20)
            {
                return BadRequest("La note doit être entre 0 et 20.");
            }

            // Vérification de l'existence de la note
            var existingGrade = await _gradeService.GetByUserIdAsync(id);
            if (existingGrade == null)
            {
                return NotFound($"La note pour l'utilisateur avec l'ID {id} n'a pas été trouvée.");
            }

            try
            {
                // Mise à jour de la note
                await _gradeService.UpdateGradeAsync(id, newGrade);
            }
            catch (Exception ex)
            {
                // Gestion des exceptions
                // Vous pouvez loguer l'erreur ici si nécessaire
                return StatusCode(500, $"Erreur lors de la mise à jour de la note : {ex.Message}");
            }

            return NoContent();
        }

        // GET api/grade/assignments/{assignementsId}
        [HttpGet("assignments/{assignementsId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GradeModel>>> GetByAssignmentAsync(int assignementsId)
        {
            var grades = await _gradeService.GetAllByAssignmentAsync(assignementsId);
            if (grades == null)
            {
                return NotFound();
            }
            return Ok(grades);
        }

        // GET api/grade/user/{userId}/assignments/{assignementsId}
        [HttpGet("user/{userId}/assignments/{assignementsId}")]
        [Authorize]

        public async Task<ActionResult<GradeModel>> GetByUserIdAsync(int userId, int assignementsId)
        {
            var grade = await _gradeService.GetByUserIdAsync(userId, assignementsId);
            if (grade == null)
            {
                return NotFound();
            }
            return Ok(grade);
        }
    }
}
