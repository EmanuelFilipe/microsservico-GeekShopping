using GeekShopping.CartAPI.Data.DTO;
using System.Threading.Tasks;

namespace GeekShopping.CartAPI.Repository
{
    public interface ICartRepository
    {
        Task<CartDTO> FindCartByUserId(string userId);
        Task<CartDTO> SaveOrUpdateCart(CartDTO cart);
        Task<bool> RemoveFromCart(long cartDetailsId);
        Task<bool> ApplyCoupon(string userId, string couponCode);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);
    }
}
