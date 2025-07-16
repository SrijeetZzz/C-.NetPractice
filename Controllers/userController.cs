using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Services;
using MyApi.Helpers;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {


        private readonly UserService _userService;
        private readonly IConfiguration _config;
        public UserController(UserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {

            var existing = await _userService.GetByEmailAsync(user.Email);
            if (existing != null)
                return BadRequest("User already exists with this email.");

            await _userService.CreateAsync(user);
            var token = JwtHelper.GenerateJwtToken(user, _config);
            return Ok(new { message = "User registered successfully.", token });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAsync();
            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest("Email and password are required.");
            var existingUser = await _userService.GetByEmailAsync(user.Email);
            if (existingUser == null)
                return Unauthorized("Invalid email.");
            else if (existingUser.Password != user.Password)
                return Unauthorized("Invalid Password.");
            var token = JwtHelper.GenerateJwtToken(existingUser, _config);
            return Ok(new { message = "Login successful.", token });
        }

    }
}