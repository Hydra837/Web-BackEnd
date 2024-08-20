using BLL.Interface;
using BLL.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Web.Mapper;
using Web.Models;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger,
                                        IAuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UsersFORM request)
        {
            if (request == null)
            {
                return BadRequest("Invalid client request");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userModel = request.BllAccessToApi1();
                await _authenticationService.RegisterUserAsync(userModel);
                return CreatedAtAction(nameof(Register), new { username = userModel.Pseudo }, "User registered successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during registration");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginFORM request)
        {
            if (request == null)
            {
                return BadRequest("Invalid client request");
            }

            try
            {
                var token = await _authenticationService.LoginAsync(request.Username, request.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login");
                return Unauthorized("Invalid username or password");
            }
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(int userId, string currentPassword, string newPassword)
        {
            try
            {
                await _authenticationService.ChangePasswordAsync(userId, currentPassword, newPassword);
                return Ok("Password changed successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswdFORM request)
        {
           
            try
            {
                await _authenticationService.ForgotPasswordAsync(request.Pseudo, request.NewPassword);
                return Ok("Mot de passe modifié avec succès");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("RefreshToken")]
        //public IActionResult RefreshToken([FromBody] string token)
        //{
        //    if (string.IsNullOrEmpty(token))
        //    {
        //        return BadRequest("Token is required");
        //    }

        //    try
        //    {
        //        var refreshedToken = _authenticationService.RefreshToken(token);
        //        return Ok(new { Token = refreshedToken });
        //    }
        //    catch (SecurityTokenException)
        //    {
        //        return Unauthorized(new { Message = "Invalid token" });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred during token refresh");
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during token refresh");
        //    }
        //}
    }
}
