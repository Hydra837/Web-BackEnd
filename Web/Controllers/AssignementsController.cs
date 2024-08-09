﻿using BLL.Interface;
using BLL.Models;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Mapper;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignementsController : ControllerBase
    {
        private readonly IAssignementsService _assignementsService;

        public AssignementsController(IAssignementsService assignementsService)
        {
            _assignementsService = assignementsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var models = await _assignementsService.GetAll();
                var dtos = models.Select(m => m.ToAssignementsDTO()).ToList();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var model = await _assignementsService.GetbyId(id);
                if (model == null)
                {
                    return NotFound($"Assignment with ID {id} not found.");
                }
                var dto = model.ToAssignementsDTO();
                return Ok(dto);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("by-course/{courseId}")]
        public async Task<IActionResult> GetByCourse(int courseId)
        {
            try
            {
                var models = await _assignementsService.GetAllByCourse(courseId);
                var dtos = models.Select(m => m.ToAssignementsDTO()).ToList();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            try
            {
                var models = await _assignementsService.GetAllByUser(userId);
                var dtos = models.Select(m => m.ToAssignementsDTO()).ToList();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("by-teacher/{userId}")]
        public async Task<IActionResult> GetByTeacher(int userId)
        {
            try
            {
                var models = await _assignementsService.GetAllByTeacher(userId);
                var dtos = models.Select(m => m.ToAssignementsDTO()).ToList();
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssignementsFORM form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var model = form.ToAssignementsModel();
                await _assignementsService.Insert(model);
                return CreatedAtAction(nameof(GetById), new { id = model.Id }, model.ToAssignementsDTO());
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AssignementsFORM form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingModel = await _assignementsService.GetbyId(id);
                if (existingModel == null)
                {
                    return NotFound($"Assignment with ID {id} not found.");
                }

                var model = form.ToAssignementsModel();
                model.Id = id; // Ensure the ID is set correctly
                await _assignementsService.Update(model);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingModel = await _assignementsService.GetbyId(id);
                if (existingModel == null)
                {
                    return NotFound($"Assignment with ID {id} not found.");
                }

                await _assignementsService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
