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

        public async Task<IActionResult> ProductIndex()
        {
            var products = await _productService.FindAll();
            return View(products);
        }

        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.Create(model);
             
                if (response != null)
                    return RedirectToAction(nameof(ProductIndex));
            }

            return View(model);
        }


        public async Task<IActionResult> ProductUpdate(long id)
        {
            var productModel = await _productService.FindById(id);

            if (productModel != null) return View(productModel);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductUpdate(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.Update(model);

                if (response != null)
                    return RedirectToAction(nameof(ProductIndex));
            }

            return View(model);
        }


        public async Task<IActionResult> ProductDelete(long id)
        {
            var productModel = await _productService.FindById(id);

            if (productModel != null) return View(productModel);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductModel model)
        {
            var response = await _productService.Delete(model.Id);

            if (response) return RedirectToAction(nameof(ProductIndex));

            return View(model);
        }        
    }
}
