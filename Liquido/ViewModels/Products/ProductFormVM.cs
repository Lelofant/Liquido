namespace Liquido.ViewModels.Products
{
    public class ProductFormVM
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } 
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? Volume { get; set; }
        public IFormFile? ImageFile { get; set; }

        public string? ExistingImageUrl { get; set; }

        public int CategoryId { get; set; }
        public bool IsFeatured { get; set; }
        public bool ISActive { get; set; } = false;

        public List<CategoryOptionVM> Categories { get; set; } = new List<CategoryOptionVM>();
    }
}
