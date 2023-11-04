using GeekShopping.CartAPI.Data.DTO;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.RabbitMQSender;
using GeekShopping.CartAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GeekShopping.CartAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private ICartRepository _repository;
        private IRabbitMQMessageSender _rabbitMQMessageSender;

        public CartController(ICartRepository repository, IRabbitMQMessageSender rabbitMQMessageSender)
        {
            _repository = repository ?? throw new
                ArgumentNullException(nameof(repository)); ;
            _rabbitMQMessageSender = rabbitMQMessageSender;
        }

        [HttpGet("find-cart/{id}")]
        public async Task<ActionResult<CartDTO>> FindById(string id)
        {
            var cart = await _repository.FindCartByUserId(id);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartDTO>> AddCart(CartDTO dto)
        {
            var cart = await _repository.SaveOrUpdateCart(dto);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpPut("update-cart")]
        public async Task<ActionResult<CartDTO>> UpdateCart(CartDTO dto)
        {
            var cart = await _repository.SaveOrUpdateCart(dto);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartDTO>> RemoveCart(int id)
        {
            var status = await _repository.RemoveFromCart(id);
            if (!status) return BadRequest();
            return Ok(status);
        }

        [HttpPost("apply-coupon")]
        public async Task<ActionResult<CartDTO>> ApplyCoupon(CartDTO dto)
        {
            var status = await _repository.ApplyCoupon(dto.CartHeader.UserId, dto.CartHeader.CouponCode );
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpDelete("remove-coupon/{userId}")]
        public async Task<ActionResult<CartDTO>> RemoveCoupon(string userId)
        {
            var status = await _repository.RemoveCoupon(userId);
            if (!status) return NotFound();
            return Ok(status);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutHeaderDTO>> Checkout(CheckoutHeaderDTO dto)
        {
            if (dto.UserId == null) return BadRequest();

            var cart = await _repository.FindCartByUserId(dto.UserId);

            if (cart == null) return NotFound();
            
            dto.CartDetails = cart.CartDetails;
            dto.DateTime = DateTime.Now;
            //TODO: task rabbitmq

            _rabbitMQMessageSender.SendMessage(dto, "checkoutqueue");

            return Ok(dto);
        }
    }
}
