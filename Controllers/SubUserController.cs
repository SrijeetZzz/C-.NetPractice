using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace LoginApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubUserController : ControllerBase
    {
        private readonly SubUserServices _subUserServices;
        public SubUserController(SubUserServices subUserServices)
        {
            _subUserServices = subUserServices;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateSubUser([FromBody] SubUserModel subUser)
        {
            await _subUserServices.CreateAsync(subUser);
            return Ok("Sub User Created Successfullly");
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetAllSubUser()
        {
            var subUsers = await _subUserServices.GetUsersAsync();
            return Ok(subUsers);
        }
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetSubUserById(string id)
        {
            var subUser = await _subUserServices.GetByIdAsync(id);
            if (subUser == null) return NotFound();
            return Ok(subUser);
        }
        [HttpGet("users-with-admin")]
        public async Task<IActionResult> GetAllSubUsersWithAdmin(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = "")
        {
            int skip = (page - 1) * pageSize;

            var subUserWithAdmin = await _subUserServices.GetAllUsersUnderAdminAsync(skip, pageSize, searchTerm);

            long totalCount = string.IsNullOrWhiteSpace(searchTerm)
                ? await _subUserServices.GetTotalCountAsync()
                : await _subUserServices.GetFilteredCountAsync(searchTerm);

            return Ok(new
            {
                subUserWithAdmin,
                totalCount
            });
        }
        [HttpPut("{id:length(24)}")]
      public async Task<IActionResult> UpdateProduct(string id, [FromBody] SubUserModel subUser)
        {
            var existingProduct = await _subUserServices.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            subUser.Id = id;
            var result = await _subUserServices.UpdateUserAsync(id, subUser);
            if (!result)
                return StatusCode(500, "Failed to update Category");
            return Ok(subUser);
        }
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var result = await _subUserServices.DeleteUserAsync(id);
            return result ? Ok() : NotFound();
        }



    }
}

