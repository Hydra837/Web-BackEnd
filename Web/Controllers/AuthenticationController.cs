using BLL.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger,
                                        AuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UsersFORM request)
        {
            if (request == null)
            {
                return BadRequest("Invalid client request");
            }

            try
            {
                await _authenticationService.RegisterUserAsync(request.Pseudo, request.Password, request.Role);
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration");
                return BadRequest("An error occurred during registration");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
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
    }
}
