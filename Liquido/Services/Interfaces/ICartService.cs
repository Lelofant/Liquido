using Liquido.ViewModels.Cart;

namespace Liquido.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartVM> GetCartAsync(string userId);
        Task AddToCartAsync(string userId, int productId, int quantity);
        Task UpdateCartItemAsync(string userId, int cartItemId, int quantity);
        Task RemoveFromCartAsync(string userId, int cartItemId);
        Task ClearCartAsync(string userId);
        Task<int> GetItemCountAsync(string userId); 
    }
}
