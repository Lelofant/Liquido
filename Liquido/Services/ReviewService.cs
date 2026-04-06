using Liquido.Data;
using Liquido.Models;
using Liquido.Services.Interfaces;
using Liquido.ViewModels.Reviews;
using Microsoft.EntityFrameworkCore;

namespace Liquido.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(string userId, ReviewsFormVM model)
        {
            var alreadyReviewed = await _context.Reviews.AnyAsync(r => r.UserId == userId && r.ProductId == model.ProductId);

            if (alreadyReviewed)
            {
                throw new InvalidOperationException("You have already reviewed this product.");
            }
            _context.Reviews.Add(new Review
            {
                UserId = userId,
                ProductId = model.ProductId,
                Rating = model.Rating,
                Comment = model.Comment,
                IsApproved = false,
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int reviewId,string userId)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId)
                ?? throw new KeyNotFoundException(
                    "Review not found or you do not own it.");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReviewsVM>> GetApprovedForProductAsync(int productId,string? currentUserId)
        {
            return await _context.Reviews.Include(r => r.User)
                                          .Where(r => r.ProductId == productId && r.IsApproved)
                                          .OrderByDescending(r => r.CreatedAt)
                                          .Select(r => new ReviewsVM
                                          {
                                              Id = r.Id,
                                              UserName = r.User.FirstName + " " + r.User.LastName,
                                              Rating = r.Rating,
                                              Comment = r.Comment,
                                              CreatedAt = r.CreatedAt,
                                              IsOwnReview = r.UserId == currentUserId
                                          })
                                          .ToListAsync();
        }
        public async Task<IEnumerable<ReviewsVM>> GetPendingAsync()
        {
            return await _context.Reviews.Include(r => r.User)
                                      .Where(r => !r.IsApproved)
                                      .OrderBy(r => r.CreatedAt)
                                      .Select(r => new ReviewsVM
                                      {
                                          Id = r.Id,
                                          UserName = r.User.FirstName + " " + r.User.LastName,
                                          Rating = r.Rating,
                                          Comment = r.Comment,
                                          CreatedAt = r.CreatedAt
                                      })
                                      .ToListAsync();
        }
        public async Task ApproveAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId)
                ?? throw new KeyNotFoundException("Review not found.");

            review.IsApproved = true;
            await _context.SaveChangesAsync();
        }

    }
}