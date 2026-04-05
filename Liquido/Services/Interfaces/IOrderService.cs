using Liquido.ViewModels.Orders;

namespace Liquido.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> PlaceOrderAsync(string userId, CheckoutVM model);
        Task<IEnumerable<OrderSummaryVM>> GetUserOrdersAsync(string userId);
        Task<OrderSummaryVM?> GetOrderDetailsAsync(int orderId, string userId);
        Task<IEnumerable<OrderSummaryVM>> GetAllOrdersAsync();
        Task<OrderSummaryVM?> GetOrderDetailsAdminAsync(int orderId);
        Task UpdateStatusAsync(int orderId, string status);
    }
}
