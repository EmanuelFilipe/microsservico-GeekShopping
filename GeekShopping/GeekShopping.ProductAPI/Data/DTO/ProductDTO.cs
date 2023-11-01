using GeekShopping.MessageBus;

namespace GeekShopping.ProductAPI.Data.DTO
{
    public class ProductDTO : BaseMessage
    {
        //public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
    }
}
