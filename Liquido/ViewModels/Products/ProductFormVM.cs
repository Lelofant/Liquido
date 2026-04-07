using System.ComponentModel.DataAnnotations;

namespace Liquido.ViewModels.Products
{
    public class ProductFormVM
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 200 characters")]
        [Display(Name = "Product Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, 9999.99, ErrorMessage = "Price must be between 0.01 and 9999.99")]
        [Display(Name = "Price (Euro)")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "Stock quantity must be 0 or more")]
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        [StringLength(50)]
        public string? Volume { get; set; }

        [Display(Name = "Product Image")]
        public IFormFile? ImageFile { get; set; }

        public string? ExistingImageUrl { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Featured on Homepage")]
        public bool IsFeatured { get; set; }

        [Display(Name = "Active (visible to users)")]
        public bool IsActive { get; set; } = true;

        public IEnumerable<CategoryOptionVM> Categories { get; set; } = new List<CategoryOptionVM>();
    }

    public class CategoryOptionVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
