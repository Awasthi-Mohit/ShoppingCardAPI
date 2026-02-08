using ApiForAng.ApplicationDbcontext;
using ApiForAng.DTO;
using ApiForAng.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiForAng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = _context.products.ToList();
            return Ok(products);
        }

        [HttpPost("Products")]

        public async Task<IActionResult> CreateProduct([FromBody] ProductDto product)
        {
            if (product == null)
            {
                return BadRequest("Product data is null.");
            }
            var products = new Product
            {
                Name = product.Name
                ,
                Description = product.Description
                ,
                Price = product.Price
                ,
                Stock = product.Stock ?? 0,
                size = product.size ?? string.Empty,
                ImageUrl = product.ImageUrl ?? string.Empty,
                CategoryId = product.CategoryId

            };
            _context.products.Add(products);
            await _context.SaveChangesAsync();
            return Ok(product);
        }
        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto product)
        {
            if (product == null)
            {
                return BadRequest("Product data is null.");
            }
            var existingProduct = _context.products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock ?? 0;
            existingProduct.size = product.size ?? string.Empty;
            existingProduct.ImageUrl = product.ImageUrl ?? string.Empty;
            existingProduct.CategoryId = product.CategoryId;
            await _context.SaveChangesAsync();
            return Ok(existingProduct);
        }
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var existingProduct = _context.products.FirstOrDefault(p => p.Id == id);
                if (existingProduct == null)
                {
                    return NotFound("Product not found.");
                }
                _context.products.Remove(existingProduct);
                await _context.SaveChangesAsync();
                return Ok("Product deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
