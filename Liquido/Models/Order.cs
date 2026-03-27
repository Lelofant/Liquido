using Liquido.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Liquido.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal TotalPrice { get; set; }

        [Required]
        [StringLength(300)]
        public string ShippingAddress { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Notes { get; set; }

        public int LoyaltyPointsEarned { get; set; } = 0;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
