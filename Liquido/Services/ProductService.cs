using Liquido.Data;
using Liquido.Models;
using Liquido.Services.Interfaces;
using Liquido.ViewModels.Products;
using Microsoft.EntityFrameworkCore;

namespace Liquido.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;

        public ProductService(ApplicationDbContext context, ICategoryService categoryService)
        {
            this._context = context;
            this._categoryService = categoryService;
        }

        public async Task<ProductListVM> GetProductAsync(string? searchTerm, int? categoryId, decimal? minPrice, decimal? maxPrice, int page, int pageSize)
        {
            var query = _context.Products.Include(p => p.Category)
                                        .Include(p => p.Reviews)
                                        .Where(p => p.IsActive)
                                        .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(term) ||
                    (p.Description != null &&
                     p.Description.ToLower().Contains(term)));
            }

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            var totalCount = await query.CountAsync();

            var products = await query.OrderBy(p => p.Name)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .Select(p => new ProductVM
                                      {
                                          Id = p.Id,
                                          Name = p.Name,
                                          Price = p.Price,
                                          Volume = p.Volume,
                                          ImageUrl = p.ImageUrl,
                                          CategoryName = p.Category.Name,
                                          StockQuantity = p.StockQuantity,
                                          AverageRating = p.Reviews.Any(r => r.IsApproved) ? p.Reviews.Where(r => r.IsApproved)
                                                                                    .Average(r => r.Rating) : 0,
                                          ReviewCount = p.Reviews.Count(r => r.IsApproved)
                                      }).ToListAsync();

            var categories = await _categoryService.GetAllAsOptionsAsync();

            return new ProductListVM
            {
                Products = products,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Categories = categories.ToList()
            };
        }

        public async Task<IEnumerable<ProductVM>> GetFeaturedAsync(int count = 6)
       => await _context.Products
           .Include(p => p.Category)
           .Include(p => p.Reviews)
           .Where(p => p.IsActive && p.IsFeatured)
           .Take(count)
           .Select(p => new ProductVM
           {
               Id = p.Id,
               Name = p.Name,
               Price = p.Price,
               Volume = p.Volume,
               ImageUrl = p.ImageUrl,
               CategoryName = p.Category.Name,
               StockQuantity = p.StockQuantity,
               AverageRating = p.Reviews.Any(r => r.IsApproved)
                   ? p.Reviews.Where(r => r.IsApproved)
                              .Average(r => r.Rating)
                   : 0,
               ReviewCount = p.Reviews.Count(r => r.IsApproved)
           })
           .ToListAsync();

        public async Task<int> CreateAsync(ProductFormVM model)
        {
            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                StockQuantity = model.StockQuantity,
                Volume = model.Volume,
                CategoryId = model.CategoryId,
                IsActive = model.ISActive,
                IsFeatured = model.IsFeatured,
                ImageUrl = model.ExistingImageUrl
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task UpdateAsync(int id, ProductFormVM model)
        {
            var product = await _context.Products.FindAsync(id)
                ?? throw new KeyNotFoundException($"Product {id} not found");

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.StockQuantity = model.StockQuantity;
            product.Volume = model.Volume;
            product.CategoryId = model.CategoryId;
            product.IsActive = model.ISActive;
            product.IsFeatured = model.IsFeatured;

            if (model.ExistingImageUrl is not null)
                product.ImageUrl = model.ExistingImageUrl;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id)
                ?? throw new KeyNotFoundException($"Product {id} not found");

           
            product.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
            => await _context.Products.AnyAsync(p => p.Id == id);

        public async Task<ProductFormVM> GetFormModelAsync(int? id = null)
        {
            var categories = await _categoryService.GetAllAsOptionsAsync();
            var model = new ProductFormVM
            {
                Categories = categories.ToList()
            };

            if (id.HasValue)
            {
                var product = await _context.Products.FindAsync(id.Value)
                    ?? throw new KeyNotFoundException($"Product {id} not found");

                model.Name = product.Name;
                model.Description = product.Description;
                model.Price = product.Price;
                model.StockQuantity = product.StockQuantity;
                model.Volume = product.Volume;
                model.CategoryId = product.CategoryId;
                model.ISActive = product.IsActive;
                model.IsFeatured = product.IsFeatured;
                model.ExistingImageUrl = product.ImageUrl;
            }

            return model;
        }
    }
}

