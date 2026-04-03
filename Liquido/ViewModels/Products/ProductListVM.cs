namespace Liquido.ViewModels.Products
{
    public class ProductListVM
    {
        public IEnumerable<ProductVM> Products { get; set; } = new List<ProductVM>();

        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public List<CategoryOptionVM> Categories { get; set; } = new List<CategoryOptionVM>();
    }
}
