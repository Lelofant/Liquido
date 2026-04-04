using Liquido.ViewModels.Reviews;

namespace Liquido.ViewModels.Products
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Volume { get; set; }
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; } 
        public int StockQuantity { get; set; }
        public bool InStock => StockQuantity > 0;
        public double AverageRating { get; set; }

        public IEnumerable<ReviewsVM> Reviews { get; set; } = new List<ReviewsVM>();

        public ReviewsFormVM ReviewForm { get; set; } = new ReviewsFormVM();
    }
}
