using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiForAng.ApplicationDbcontext;
using ApiForAng.DTO;
using ApiForAng.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApiForAng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize("Admin")]
        [HttpGet("Category")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = _context.categories.ToList();
            return Ok(categories);
        }
        [HttpPost("Category")]
        [Authorize("Admin")]

        public IActionResult CreateCategory([FromBody] CategoryDto category)
        {
            if (category == null)
            {
                return BadRequest("Category data is null.");
            }
            var categoryEntity = new Category
            {
                id = category.id,
                Name = category.Name
            };
            _context.categories.Add(categoryEntity);
            _context.SaveChanges();
            return Ok(categoryEntity);
        }
        [HttpPut("UpdateCategory")]
        [Authorize("Admin")]

        public IActionResult UpdateCategory([FromBody] CategoryDto category)
        {
            if (category == null)
            {
                return BadRequest("Category data is null.");
            }
            var existingCategory = _context.categories.FirstOrDefault(c => c.id == category.id);
            if (existingCategory == null)
            {
                return NotFound("Category not found.");
            }
            existingCategory.Name = category.Name;
            _context.SaveChanges();
            return Ok(existingCategory);


        }
    }
}
