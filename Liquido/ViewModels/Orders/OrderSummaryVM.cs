using Liquido.Helpers;
using Liquido.Models.Enums;

namespace Liquido.ViewModels.Orders
{
    public class OrderSummaryVM
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public string? StatusDisplay => Status.ToString();

        public string StatusBadgeClass => OrderStatusHelper.GetBadgeClass(Status);

        public decimal TotalPrice { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string? Notes  { get; set; }
        public int LoyalityPointsEarned { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        public IEnumerable<OrderItemVM> Items { get; set; } = new List<OrderItemVM>();
    }
}
