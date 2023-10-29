using GeekShopping.CartAPI.Data.DTO;
using GeekShopping.CartAPI.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CartAPI.Model
{
    [Table("cart_detail")]
    public class CartDetail : BaseEntity
    {
        [ForeignKey("CartHeaderId")]
        public long CartHeaderId { get; set; }

        [ForeignKey("ProductId")]
        public long ProductId { get; set; }

        public virtual CartHeaderDTO CartHeader { get; set; }

        public virtual ProductDTO Product { get; set; }

        [Column("count")]
        public int Count { get; set; }
    }
}
