using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> FindAll(string token);
        Task<ProductViewModel> FindById(long id, string token);
        Task<ProductViewModel> Create(ProductViewModel model, string token);
        Task<ProductViewModel> Update(ProductViewModel model, string token);
        Task<bool> Delete(long id, string token);
    }
}