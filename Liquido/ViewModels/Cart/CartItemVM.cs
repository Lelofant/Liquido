namespace Liquido.ViewModels.Cart
{
    public class CartItemVM
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImageUrl { get; set; } 
        public string? Volume { get; set; }
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal SubTotal => Price * Quantity;

        public int MaxQuantity { get; set; }
    }
}
