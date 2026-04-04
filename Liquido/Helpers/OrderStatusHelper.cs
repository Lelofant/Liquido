using Liquido.Models.Enums;

namespace Liquido.Helpers
{
    public class OrderStatusHelper
    {
        public static string GetBadgeClass(OrderStatus status) => status switch
        {
            OrderStatus.Pending => "bg-warning text-dark",
            OrderStatus.Confirmed => "bg-info text-dark",
            OrderStatus.Processing => "bg-primary",
            OrderStatus.Shipped => "bg-secondary",
            OrderStatus.Delivered => "bg-success",
            OrderStatus.Cancelled => "bg-danger",
            _ => "bg-secondary"
        };
    }
}
