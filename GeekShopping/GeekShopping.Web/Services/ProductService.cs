using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Services
{
    public class ProductService : IProductService
    {
        
        private readonly HttpClient _client;
        //é pego no controller do projeto API que irá chamar + o nome do controller
        public const string BasePath = "api/v1/product";

        public ProductService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<ProductViewModel>> FindAll()
        {
            
            var response = await _client.GetAsync(BasePath);
            // tem que passar o tipo que quer que seja o retorno
            return await response.ReadContentAs<List<ProductViewModel>>();
        }

        public async Task<ProductViewModel> FindById(long id)
        {
            
            var response = await _client.GetAsync($"{BasePath}/{id}");
            return await response.ReadContentAs<ProductViewModel>();
        }

        public async Task<ProductViewModel> Create(ProductViewModel model)
        {
            
            var response = await _client.PostAsJson(BasePath, model);
            
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProductViewModel>();
            else
                throw new Exception("Something went wrong when calling API");
        }

        public async Task<ProductViewModel> Update(ProductViewModel model)
        {
            
            var response = await _client.PutAsJson(BasePath, model);

            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<ProductViewModel>();
            else
                throw new Exception("Something went wrong when calling API");
        }

        public async Task<bool> Delete(long id)
        {
            
            var response = await _client.DeleteAsync($"{BasePath}/{id}");
         
            if (response.IsSuccessStatusCode) 
                return await response.ReadContentAs<bool>();
            else
                throw new Exception("Something went wrong when calling API");
        }

        public void SetTokenInHeader(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

    }
}
