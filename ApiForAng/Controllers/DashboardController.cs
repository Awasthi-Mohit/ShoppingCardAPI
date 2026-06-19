using ApiForAng.ApplicationDbcontext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiForAng.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class DashboardController : ControllerBase
        {

            private readonly ApplicationDbContext _context;
            public DashboardController(ApplicationDbContext context)
            {
                _context = context;
            }
            // GET: api/products
            [HttpGet("AllProducts")]
            public async Task<IActionResult> GetAllProducts()   
            {
                var products = await _context.products.ToListAsync();

                if (products == null || products.Count == 0)
                {
                    return NotFound("No products found.");
                }

                return Ok(products);
            }
        }
    }
