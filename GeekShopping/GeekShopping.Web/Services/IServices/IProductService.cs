using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> FindAll();
        Task<ProductViewModel> FindById(long id);
        Task<ProductViewModel> Create(ProductViewModel model);
        Task<ProductViewModel> Update(ProductViewModel model);
        Task<bool> Delete(long id);
    }
}