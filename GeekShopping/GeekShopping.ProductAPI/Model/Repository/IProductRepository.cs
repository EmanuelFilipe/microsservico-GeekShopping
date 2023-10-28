using GeekShopping.ProductAPI.Data.DTO;

namespace GeekShopping.ProductAPI.Model.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>> FindAll();
        Task<ProductDTO> FindById(long id);
        Task<ProductDTO> Create(ProductDTO dto);
        Task<ProductDTO> Update(ProductDTO dto);
        Task<bool> Delete(long id);
    }
}
