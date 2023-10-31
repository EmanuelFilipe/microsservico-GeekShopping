using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CartController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> CartIndex()
        {
            CartViewModel response = await FindUserCart();

            return View(response);
        }


        public async Task<IActionResult> Remove(int id)
        {
            var response = await _cartService.RemoveFromCart(id);

            if (response) return RedirectToAction(nameof(CartIndex));

            return View(response);
        }

        private async Task<CartViewModel> FindUserCart()
        {
            var response = await _cartService.FindCartByUserId("6c70bef8-d1ba-42a0-8107-d97ac051d88b");

            if (response?.CartHeader != null)
            {
                foreach (var detail in response.CartDetails)
                    response.CartHeader.PurchaseAmount += (detail.Product.Price * detail.Count);
            }

            return response;
        }
    }
}
