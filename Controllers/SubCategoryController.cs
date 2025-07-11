
using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Services;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubCategoryController : ControllerBase
    {
        private readonly SubCategoryService _subCategoryService;

        public SubCategoryController(SubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubCategory([FromBody] SubCategory subcategory)
        {
            if (string.IsNullOrWhiteSpace(subcategory.SubName))
                return BadRequest("SubName is required.");

            var existing = await _subCategoryService.GetByCategoryIdAndSubNameAsync(
                subcategory.CategoryId, subcategory.SubName);

            if (existing != null)
                return Conflict("SubCategory already exists under this category.");

            subcategory.CreatedAt = DateTime.Now;
            subcategory.UpdatedAt = DateTime.Now;
            await _subCategoryService.CreateSubCategoryAsync(subcategory);
            return CreatedAtAction(nameof(GetSubCategoryById), new { id = subcategory.Id }, subcategory);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetSubCategoryById(string id)
        {
            var subcategory = await _subCategoryService.GetByIdAsync(id);
            if (subcategory == null) return NotFound();
            return Ok(subcategory);
        }
        [HttpGet("with-category")]
        public async Task<IActionResult> GetWithCategoryNames([FromQuery] int page, [FromQuery] int pageSize)
        {
            int skip = (page - 1) * pageSize;
            var data = await _subCategoryService.GetAllWithCategoryNamesAsync(skip, pageSize);
            var totalCount = await _subCategoryService.GetTotalCountAsync();

            return Ok(new
            {
                data,
                totalCount
            });
        }
        [HttpGet("with-categoryName")]
        public async Task<IActionResult> GetCategoryNamesWithLabels([FromQuery] int page, [FromQuery] int pageSize)
        {
            int skip = (page - 1) * pageSize;
            var data = await _subCategoryService.GetAllCategoryNamesWithLabels(skip, pageSize);
            var totalCount = await _subCategoryService.GetTotalCountAsync();

            return Ok(new
            {
                data,
                totalCount
            });
        }


    }
}
