using ApiForAng.ApplicationDbcontext;
using ApiForAng.Migrations;
using ApiForAng.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiForAng.Controllers
{
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

            var cartItems = _context.cartitems
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart).ToList();             //.Select(ci => new

                                                              //{
                                                              //    ci.Id,
                                                              //    ci.ProductId,
                                                              //    ProductName = ci.Product.Name,
                                                              //    ci.Product.Price,
                                                              //    ci.Product.ImageUrl,
                                                              //    ci.Quantity
                                                              //})
                                                              //.ToList();


            return Ok(cartItems);
        }

        // ➕ POST: api/cart/add
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            // 1. Get the current User ID from your helper method
            int userId = GetUserId();

            // 2. Fetch the cart and its items in ONE database call using .Include()
            // This prevents "N+1" query issues later.
            var cart = await _context.carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // 3. If no cart exists, create a new instance in memory
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.carts.Add(cart);
                // We do NOT call SaveChangesAsync here yet.
            }

            // 4. Check if the specific product is already in the cart items list
            var item = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == productId);

            if (item != null)
            {
                // Update existing item quantity
                item.Quantity += quantity;
            }
            else
            {
                var newItem = new ApiForAng.Models.Cartitems
                {
                    CartId = cart.Id,      
                    ProductId = productId,
                    Quantity = quantity
                };

                // 2. Add the specific object you just created
                _context.cartitems.Add(newItem);

                // 3. Save to database
                await _context.SaveChangesAsync();
            }

            // 5. Save all changes (New Cart + New/Updated Item) in a single transaction
            await _context.SaveChangesAsync();

            return Ok(new { message = "Item successfully added to cart" });
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
