using BLL;
using BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using Web.Models;
using BLL.Models;
using Web.Mapper;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [AllowAnonymous]
        public async Task<IActionResult> Register(UsersFORM request) // FROM BODY à ajouter
        {
            if (request == null)
            {
                return BadRequest("Invalid client request");
            }

            try
            { UsersFORM a = request;
              a.BllAccessToApi();
               await _authenticationService.RegisterUserAsync(a);
                
               
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
        public async Task<IActionResult> Login( LoginFORM request) // from BODY
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
        [HttpGet("RefreshToken")]
        [AllowAnonymous]
        public IActionResult RefreshToken(string token)
        {
            try
            {
                var refreshedToken = _authenticationService.RefreshToken(token);
                return Ok(new { token = refreshedToken });
            }
            catch (SecurityTokenException)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
        }
        
    }
}
