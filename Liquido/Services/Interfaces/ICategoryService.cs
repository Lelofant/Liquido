using Liquido.Models;
using Liquido.ViewModels.Products;

namespace Liquido.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<CategoryOptionVM>> GetAllAsOptionsAsync();
        Task<Category?> GetByIdAsync(int id);
        Task CreateAsync(string name, string? description, string? imageUrl);
        Task UpdateAsync(int id, string name, string? description);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
