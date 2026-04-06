using Liquido.Data;
using Liquido.Models;
using Liquido.Models.Enums;
using Liquido.Services.Interfaces;
using Liquido.ViewModels.Orders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Liquido.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderService(ApplicationDbContext context, ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<int> PlaceOrderAsync(string userId, CheckoutVM model)
        {
            var cartItem = await _context.CartItems.Include(ci => ci.Product)
                                            .Where(ci => ci.UserId == userId)
                                            .ToListAsync();
            if(!cartItem.Any())
            {
                throw new InvalidOperationException("Cart is empty.");
            }
            
            foreach(var item in cartItem)
            {
                if(item.Product.StockQuantity < item.Quantity)
                {
                    throw new InvalidOperationException
                        ($"Not enough stock for product {item.Product.Name}.");
                }
            }
            
            var total = cartItem.Sum(ci => ci.Product.Price * ci.Quantity);
            var loyaltyPoints = (int)Math.Floor(total / 10);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = new Order
                {
                    UserId = userId,
                    ShippingAddress = model.ShippingAddress,
                    Notes = model.Notes,
                    TotalPrice = total,
                    LoyaltyPointsEarned = loyaltyPoints,
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    OrderItems = cartItem.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        UnitPrice = ci.Product.Price
                    }).ToList()
                };

                _context.Orders.Add(order);
                
                foreach(var item in cartItem)
                {
                    item.Product.StockQuantity -= item.Quantity;
                }
                
                var user = await _userManager.FindByIdAsync(userId);
                if(user != null)
                {
                    user.LoyaltyPoints += loyaltyPoints;
                    await _userManager.UpdateAsync(user);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                await _cartService.ClearCartAsync(userId);
                return order.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<OrderSummaryVM>> GetUserOrdersAsync(string userId)
        {
            return await _context.Orders.Include(o => o.OrderItems)
                                        .ThenInclude(oi => oi.Product)
                                        .Include(o => o.User)
                                        .Where(o => o.UserId == userId)
                                        .OrderByDescending(o => o.CreatedAt)
                                        .Select(o => MapToSummary(o))
                                        .ToListAsync();
        }

        public async Task<OrderSummaryVM?> GetOrderDetailsAsync(int orderId, string userId)
        {
            var order = await _context.Orders.Include(o => o.OrderItems)
                                            .ThenInclude(oi => oi.Product)
                                            .Include(o => o.User)
                                            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            return order is not null ? MapToSummary(order) : null;
        }
        public async Task<IEnumerable<OrderSummaryVM>> GetAllOrdersAsync()
        {
            return await _context.Orders.Include(o => o.OrderItems)
                                        .ThenInclude(oi => oi.Product)
                                        .Include(o => o.User)
                                        .OrderByDescending(o => o.CreatedAt)
                                        .Select(o => MapToSummary(o))
                                        .ToListAsync();
        }
        
        public async Task<OrderSummaryVM?> GetOrderDetailsAdminAsync(int orderId)
        {
            var order = await _context.Orders.Include(o => o.OrderItems)
                                            .ThenInclude(oi => oi.Product)
                                            .Include(o => o.User)
                                            .FirstOrDefaultAsync(o => o.Id == orderId);

            return order is not null ? MapToSummary(order) : null;
        }

        public async Task UpdateStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId)
             ?? throw new KeyNotFoundException($"Order {orderId} not found.");

            if (!Enum.TryParse<OrderStatus>(status, out var parsed))
            {
                throw new ArgumentException($"Invalid status: {status}");
            }

            order.Status = parsed;
            await _context.SaveChangesAsync();
        }


        private static OrderSummaryVM MapToSummary(Order o) => new()
        {
            Id = o.Id,
            CreatedAt = o.CreatedAt,
            Status = o.Status,
            TotalPrice = o.TotalPrice,
            ShippingAddress = o.ShippingAddress,
            Notes = o.Notes,
            LoyalityPointsEarned = o.LoyaltyPointsEarned,
            CustomerName = o.User is not null
            ? $"{o.User.FirstName} {o.User.LastName}"
            : "Unknown",
            CustomerEmail = o.User?.Email ?? string.Empty,
            Items = o.OrderItems.Select(oi => new OrderItemVM
            {
                ProductName = oi.Product?.Name ?? "Deleted Product",
                ProductImageUrl = oi.Product?.ImageUrl,
                Volume = oi.Product?.Volume,
                Quantity = oi.Quantity,
                Price = oi.UnitPrice
            })
        };

       
    }
}
