using Liquido.Models;
using Liquido.Services.Interfaces;
using Liquido.ViewModels.Reviews;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Liquido.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(
            IReviewService reviewService,
            UserManager<ApplicationUser> userManager)
        {
            _reviewService = reviewService;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewsFormVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please select a rating before submitting.";
                return RedirectToAction("Details", "Products", new { id = model.ProductId });
            }

            var userId = _userManager.GetUserId(User)!;

            try
            {
                await _reviewService.CreateAsync(userId, model);
                TempData["Success"] = "Review submitted! It will appear after approval.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Details", "Products", new { id = model.ProductId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int reviewId, int productId)
        {
            var userId = _userManager.GetUserId(User)!;

            try
            {
                await _reviewService.DeleteAsync(reviewId, userId);
                TempData["Success"] = "Review deleted.";
            }
            catch (KeyNotFoundException)
            {
                TempData["Error"] = "Review not found.";
            }

            return RedirectToAction("Details", "Products", new { id = productId });
        }
    }
}
