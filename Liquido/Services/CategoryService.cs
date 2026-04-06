using Liquido.Data;
using Liquido.Models;
using Liquido.Services.Interfaces;
using Liquido.ViewModels.Products;
using Microsoft.EntityFrameworkCore;

namespace Liquido.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                             .OrderBy(c => c.Name)
                             .ToListAsync();
        }

        public async Task<IEnumerable<CategoryOptionVM>> GetAllAsOptionsAsync()
        {
            return await _context.Categories
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.Name)
                    .Select(c => new CategoryOptionVM
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                    .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task CreateAsync(string name, string? description, string? imageUrl)
        {
            var category = new Category
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                IsActive = true
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, string name, string? description)
        {
            var category = await _context.Categories.FindAsync(id)
                ?? throw new KeyNotFoundException($"Category {id} not found");

            category.Name = name;
            category.Description = description;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id)
                ?? throw new KeyNotFoundException($"Category {id} not found");

            bool hasProducts = await _context.Products
                                             .AnyAsync(p => p.CategoryId == id);

            if (hasProducts)
                throw new InvalidOperationException(
                    "Cannot delete a category that has products assigned to it.");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
            => await _context.Categories.AnyAsync(c => c.Id == id);

    }
}