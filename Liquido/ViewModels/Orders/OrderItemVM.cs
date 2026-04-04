namespace Liquido.ViewModels.Orders
{
    public class OrderItemVM
    {
        public string ProductName { get; set; } = string.Empty;

        public string? ProductImageUrl { get; set; }

        public string? Volume { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal SubTotal => Price * Quantity;
    }
}
