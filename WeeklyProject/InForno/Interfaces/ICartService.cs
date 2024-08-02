using InForno.Models;

namespace InForno.Interfaces
{
    public interface ICartService
    {
        Task<List<CartItem>> GetCartItemsAsync(int userId);
        Task AddToCartAsync(int userId, int productId, int quantity);
        Task RemoveFromCartAsync(int userId, int productId);
        Task UpdateCartItemAsync(int userId, int productId, int quantity);
        Task ClearCartAsync(int userId);
    }
}
