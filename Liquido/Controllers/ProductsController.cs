using Liquido.Models;
using Liquido.Services.Interfaces;
using Liquido.ViewModels.Reviews;
using Microsoft.AspNetCore.Mvc;

namespace Liquido.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly IReviewService _reviewService;

    public ProductsController(
        IProductService productService,
        IReviewService reviewService)
    {
        _productService = productService;
        _reviewService = reviewService;
    }

    public async Task<IActionResult> Index(
        string? searchTerm,
        int? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        int page = 1)
    {
        var model = await _productService.GetPagedAsync(
            searchTerm, categoryId, minPrice, maxPrice, page, pageSize: 9);

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var model = await _productService.GetDetailsByIdAsync(id);

        if (model is null)
            return NotFound();

        var userId = User.Identity?.IsAuthenticated == true
            ? (await HttpContext.RequestServices
                .GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>()
                .GetUserAsync(User))?.Id
            : null;

        model.Reviews = await _reviewService.GetApprovedForProductAsync(id, userId);
        model.ReviewForm = new ReviewsFormVM { ProductId = id };

        return View(model);
    }
}
