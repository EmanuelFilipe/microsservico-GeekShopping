using GeekShopping.Coupon.Data.DTO;
using GeekShopping.CouponAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CouponController : ControllerBase
    {
        private ICouponRepository _couponRepository;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository ?? throw new ArgumentException(nameof(couponRepository)); ;
        }

        [HttpGet("{couponCode}")]
        public async Task<ActionResult<CouponDTO>> FindAll(string couponCode)
        {
            var coupon = await _couponRepository.GetCouponByCouponCode(couponCode);

            if (coupon == null) return NotFound();

            return Ok(coupon);
        }
    }
}
