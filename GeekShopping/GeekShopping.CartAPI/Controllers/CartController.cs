using GeekShopping.CartAPI.Data.DTO;
using GeekShopping.CartAPI.Model;
using GeekShopping.CartAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentException(nameof(cartRepository));
        }
        
        [HttpGet("find-cart/{id}")]
        public async Task<ActionResult> FindById(string userId)
        {
            var cart = await _cartRepository.FindCartByUserId(userId);

            if (cart == null) return NotFound();

            return Ok(cart);
        }

        [HttpPost("add-cart/{id}")]
        public async Task<ActionResult> AddCart(CartDTO dto)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(dto);

            if (cart == null) return NotFound();

            return Ok(cart);
        }

        [HttpPut("update-cart/{id}")]
        public async Task<ActionResult> UpdateCart(CartDTO dto)
        {
            var cart = await _cartRepository.SaveOrUpdateCart(dto);

            if (cart == null) return NotFound();

            return Ok(cart);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult> UpdateCart(int id)
        {
            var status = await _cartRepository.RemoveFromCart(id);

            if (!status) return BadRequest();

            return Ok(status);
        }


    }
}
