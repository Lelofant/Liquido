using Liquido.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Liquido.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalOrders = await _context.Orders.CountAsync();
            ViewBag.TotalProducts = await _context.Products.CountAsync(p => p.IsActive);
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.TotalRevenue = await _context.Orders.SumAsync(o => (decimal?)o.TotalPrice) ?? 0;
            ViewBag.RecentOrders = await _context.Orders.Include(o => o.User)
                                                  .OrderByDescending(o => o.CreatedAt)
                                                  .Take(5)
                                                  .ToListAsync();

                 return View();
        }
    }
}
