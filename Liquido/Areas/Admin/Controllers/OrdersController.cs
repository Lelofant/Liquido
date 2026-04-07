using Liquido.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Liquido.Areas.Admin.Controllers
{
    public class OrdersController : BaseAdminController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderDetailsAdminAsync(id);
            if (order is null) return NotFound();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int orderId, string status)
        {
            await _orderService.UpdateStatusAsync(orderId, status);
            TempData["Success"] = "Order status updated";
            return RedirectToAction("Details", new { id = orderId });
        }
    }
}
