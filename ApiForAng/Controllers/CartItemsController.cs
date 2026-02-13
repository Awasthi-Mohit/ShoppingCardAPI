using ApiForAng.ApplicationDbcontext;
using ApiForAng.DTO;
using ApiForAng.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiForAng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CartItemsController(ApplicationDbContext context)
        {
                _context = context;
        }
        [HttpPost("cartItems")]
        public async Task<IActionResult> CartItems(CartItemDto cartItem)
        {
            try
            {
                var cart = _context.carts.FirstOrDefault();
                var product = _context.products.FirstOrDefault();
                var cartitems = new Cartitems
                {
                    //CartId = cartItem.CartId,
                    //ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity
                };
                _context.cartitems.Add(cartitems);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex) { 
            return BadRequest(ex.Message);  
            }
        }
    }
}
