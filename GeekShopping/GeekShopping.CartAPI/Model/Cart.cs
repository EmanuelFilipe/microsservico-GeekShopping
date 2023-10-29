using GeekShopping.CartAPI.Data.DTO;

namespace GeekShopping.CartAPI.Model
{
    public class Cart
    {
        public CartHeaderDTO CartHeader { get; set; }

        public IEnumerable<CartDetailDTO> CartDetails { get; set; }
    }
}
