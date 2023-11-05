using GeekShopping.CartAPI.Data.DTO;
using System.Threading.Tasks;

namespace GeekShopping.CartAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDTO> GetCoupon(string couponCode, string token);
    }
}
