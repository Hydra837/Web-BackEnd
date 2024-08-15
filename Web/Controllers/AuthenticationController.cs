using BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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

        //[HttpPost("Register")]
        //public async Task<IActionResult> Register([FromBody] UsersFORM request)
        //{
        //    if (request == null)
        //    {
        //        return BadRequest("Invalid client request");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        var userModel = request.ToBllModel();
        //        await _authenticationService.RegisterUserAsync(userModel);
        //        return CreatedAtAction(nameof(Register), new { username = userModel.Pseudo }, "User registered successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred during registration");
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during registration");
        //    }
        //}

        [HttpPost("Login")]

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
