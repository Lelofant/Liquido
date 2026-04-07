using Liquido.ViewModels.Products;

namespace Liquido.Services.Interfaces
{
    public interface IProductService
    {    
            Task<ProductListVM> GetPagedAsync(
                string? searchTerm,
                int? categoryId,
                decimal? minPrice,
                decimal? maxPrice,
                int page,
                int pageSize);
        
            Task<ProductDetailsVM?> GetDetailsByIdAsync(int id);
            Task<IEnumerable<ProductVM>> GetFeaturedAsync(int count = 6);
            Task<int> CreateAsync(ProductFormVM model);
            Task UpdateAsync(int id, ProductFormVM model);
            Task DeleteAsync(int id);
            Task<bool> ExistsAsync(int id);
            Task<ProductFormVM> GetFormModelAsync(int? id = null);
    }
}
