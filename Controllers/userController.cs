// using Microsoft.AspNetCore.Mvc;
// using MyApi.Models;
// using MyApi.Services;

// namespace MyApi.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class UserController : ControllerBase
//     {
//         private readonly UserService _userService;

//         public UserController(UserService userService)
//         {
//             _userService = userService;
//         }

//         [HttpPost("signup")]
//         public async Task<IActionResult> SignUp([FromBody] User user)
//         {
//             var existing = await _userService.GetByEmailAsync(user.Email);
//             if (existing != null)
//                 return BadRequest("User already exists with this email.");

//             await _userService.CreateAsync(user);
//             return Ok("User registered successfully.");
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAllUsers()
//         {
//             var users = await _userService.GetAsync();
//             return Ok(users);
//         }
        
//         [HttpPost("login")]
//         public async Task<IActionResult> Login([FromBody] User user)
//         {
//             if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
//                 return BadRequest("Email and password are required.");
//             var existingUser = await _userService.GetByEmailAsync(user.Email);
//             if (existingUser == null)
//                 return Unauthorized("Invalid email.");
//             else if (existingUser.Password != user.Password)
//                 return Unauthorized("Invalid Password.");
//             return Ok("Login successful.");
//         }

//     }
// }