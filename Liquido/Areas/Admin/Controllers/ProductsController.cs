using Liquido.Services.Interfaces;
using Liquido.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;

namespace Liquido.Areas.Admin.Controllers
{
    public class ProductsController : BaseAdminController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _productService.GetPagedAsync(
                null, null, null, null, page: 1, pageSize: 50);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _productService.GetFormModelAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormVM model)
        {
            if (!ModelState.IsValid)
            {
                var refreshed = await _productService.GetFormModelAsync();
                model.Categories = refreshed.Categories;

                return View(model);
            }

            await _productService.CreateAsync(model);
            TempData["Success"] = "Product created successfully";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _productService.GetFormModelAsync(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductFormVM model)
        {
            if (!ModelState.IsValid)
            {
                var refreshed = await _productService.GetFormModelAsync();
                model.Categories = refreshed.Categories;

                return View(model);
            }

            await _productService.UpdateAsync(id, model);
            TempData["Success"] = "Product updated successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            TempData["Success"] = "Product deleted";
            return RedirectToAction("Index");
        }
    }
}
