using Liquido.ViewModels.Reviews;

namespace Liquido.Services.Interfaces
{
    public interface IReviewService
    {
        Task CreateAsync(string userId, ReviewsFormVM model);
        Task DeleteAsync(int reviewId, string userId);
        Task<IEnumerable<ReviewsVM>> GetApprovedForProductAsync(
            int productId, string? currentUserId);
        Task<IEnumerable<ReviewsVM>> GetPendingAsync();
        Task ApproveAsync(int reviewId);

    }
}
