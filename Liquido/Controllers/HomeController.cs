using Liquido.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Liquido.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;

    public HomeController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var featured = await _productService.GetFeaturedAsync(6);
        return View(featured);
    }

    public IActionResult About()
    {
        return View();
    }

    [Route("Home/Error/{statusCode?}")]
    public IActionResult Error(int? statusCode)
    {
        if (statusCode == 404)
            return View("Error404");

        return View("Error");
    }

}