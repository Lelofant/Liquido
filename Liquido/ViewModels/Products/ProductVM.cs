namespace Liquido.ViewModels.Products
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Volume { get; set; }
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public bool InStock => StockQuantity > 0;
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
