using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [Authorize]
        public async Task<IActionResult> ProductIndex()
        {
            var products = await _productService.FindAll(await GetAccessToken());
            return View(products);
        }

        

        public IActionResult ProductCreate()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.Create(model, await GetAccessToken());
             
                if (response != null)
                    return RedirectToAction(nameof(ProductIndex));
            }

            return View(model);
        }


        public async Task<IActionResult> ProductUpdate(long id)
        {
            var productModel = await _productService.FindById(id, await GetAccessToken());

            if (productModel != null) return View(productModel);

            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProductUpdate(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.Update(model, await GetAccessToken());

                if (response != null)
                    return RedirectToAction(nameof(ProductIndex));
            }

            return View(model);
        }


        public async Task<IActionResult> ProductDelete(long id)
        {

            var productModel = await _productService.FindById(id, await GetAccessToken());

            if (productModel != null) return View(productModel);

            return NotFound();
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ProductDelete(ProductViewModel model)
        {
            var response = await _productService.Delete(model.Id, await GetAccessToken());

            if (response) return RedirectToAction(nameof(ProductIndex));

            return View(model);
        }

        private async Task<string> GetAccessToken()
        {
            return await HttpContext.GetTokenAsync("access_token");
        }
    }
}
