using ApiForAng.ApplicationDbcontext;
using ApiForAng.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiForAng.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔐 Get logged-in UserId from JWT
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new Exception("User is not authenticated");

            return int.Parse(userIdClaim.Value);
        }

        // 🛒 GET: api/cart
        [HttpGet]
        public IActionResult GetCart()
        {
            int userId = GetUserId();

            var cartItems = _context.cartitems
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart)
                .Where(ci => ci.Cart.UserId == userId)
                .Select(ci => new
                {
                    ci.Id,
                    ci.ProductId,
                    ProductName = ci.Product.Name,
                    ci.Product.Price,
                    ci.Product.ImageUrl,
                    ci.Quantity
                })
                .ToList();

            return Ok(cartItems);
        }

        // ➕ POST: api/cart/add
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            int userId = GetUserId();

            var cart = _context.carts.FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var item = _context.cartitems
                .FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == productId);

            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                _context.cartitems.Add(new Cartitems
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _context.SaveChangesAsync();
            return Ok("Added to cart");
        }

        // 🔄 PUT: api/cart/update/{cartItemId}
        [HttpPut("update/{cartItemId}")]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            int userId = GetUserId();

            var item = _context.cartitems
                .Include(ci => ci.Cart)
                .FirstOrDefault(ci => ci.Id == cartItemId && ci.Cart.UserId == userId);

            if (item == null)
                return NotFound("Cart item not found");

            item.Quantity = quantity;
            await _context.SaveChangesAsync();

            return Ok("Quantity updated");
        }

        // ❌ DELETE: api/cart/remove/{cartItemId}
        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            int userId = GetUserId();

            var item = _context.cartitems
                .Include(ci => ci.Cart)
                .FirstOrDefault(ci => ci.Id == cartItemId && ci.Cart.UserId == userId);

            if (item == null)
                return NotFound("Item not found");

            _context.cartitems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok("Item removed");
        }
    }
}
