using Liquido.Models;
using Liquido.Services.Interfaces;
using Liquido.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Liquido.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(
            IOrderService orderService,
            ICartService cartService,
            UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _cartService = cartService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = _userManager.GetUserId(User)!;
            var cart = await _cartService.GetCartAsync(userId);

            if (cart.IsEmpty)
                return RedirectToAction("Index", "Cart");

            var user = await _userManager.GetUserAsync(User);
            var model = new CheckoutVM
            {
                Cart = cart,
                ShippingAddress = user?.Address ?? string.Empty
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutVM model)
        {
            var userId = _userManager.GetUserId(User)!;

            model.Cart = await _cartService.GetCartAsync(userId);

            if (model.Cart.IsEmpty)
                return RedirectToAction("Index", "Cart");

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var orderId = await _orderService.PlaceOrderAsync(userId, model);
                TempData["Success"] = "Order placed successfully!";
                return RedirectToAction("MyOrders");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        public async Task<IActionResult> MyOrders()
        {
            var userId = _userManager.GetUserId(User)!;
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return View(orders);
        }
    }
}