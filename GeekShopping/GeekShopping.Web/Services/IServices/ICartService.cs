using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.IServices
{
    public interface ICartService
    {
        Task<CartViewModel> FindCartByUserId(string userId);
        Task<CartViewModel> AddItemToCart(CartViewModel cart);
        Task<CartViewModel> UpdateCart(CartViewModel cart);
        Task<CartViewModel> Checkout(CartHeaderViewModel cartHeader);
        Task<bool> RemoveFromCart(long id);
        Task<bool> ApplyCoupon(CartViewModel cart, string couponCode);
        //Task<bool> RemoveFromCart(string userId);
        Task<bool> ClearCart(string userId);
    }
}
