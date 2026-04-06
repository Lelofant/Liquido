using Liquido.Data;
using Liquido.ViewModels.Cart;
using Liquido.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Liquido.Models;

namespace Liquido.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartVM> GetCartAsync(string userId)
        {
            var items = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .Select(ci => new CartItemVM
                {
                    CartItemId = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    ProductImageUrl = ci.Product.ImageUrl,
                    Volume = ci.Product.Volume,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    MaxQuantity = ci.Product.StockQuantity
                })
                .ToListAsync();

            return new CartVM { Items = items };
        }

        public async Task AddToCartAsync(string userId, int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId)
                ?? throw new KeyNotFoundException("Product not found");

            if (!product.IsActive || product.StockQuantity < quantity)
            {
                throw new InvalidOperationException("Product is not available");
            }

            var existing = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

            if (existing != null)
            {
                var newQnty = existing.Quantity * quantity;
                if (newQnty > product.StockQuantity)
                {
                    throw new InvalidOperationException("Not enough stock");
                }

                existing.Quantity = newQnty;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    AddedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(string userId, int cartItemId, int quantity)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.UserId == userId)
                ?? throw new KeyNotFoundException("Cart item not found");

            if (quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                if (quantity > cartItem.Product.StockQuantity)
                {
                    throw new InvalidOperationException("Not enough stock");
                }
                cartItem.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(string userId, int cartItemId)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci =>
            ci.Id == cartItemId && ci.UserId == userId)
                ?? throw new KeyNotFoundException("Cart item not found");

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(string userId)
        {
            var cartItems = await _context.CartItems.Where(ci => ci.UserId == userId)
                                                    .ToListAsync();

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetItemCountAsync(string userId)
        {
            return await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .SumAsync(ci => ci.Quantity);
        }
    }
}
