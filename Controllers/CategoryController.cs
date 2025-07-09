using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Services;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            await _categoryService.CreateCategoryAsync(category);
            return CreatedAtAction("GetById", new { id = category.Id }, category);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetById(string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }


        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] Category updatedCategory)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
                return NotFound();

            updatedCategory.Id = id;
            var result = await _categoryService.UpdateCategoryAsync(id, updatedCategory);

            if (!result)
                return StatusCode(500, "Failed to update category");

            return Ok(updatedCategory); 
        }


        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var success = await _categoryService.DeleteCategoryAsync(id);
            return success ? Ok() : NotFound();
        }
    }
}