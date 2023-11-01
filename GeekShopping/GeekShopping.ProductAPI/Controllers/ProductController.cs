using GeekShopping.ProductAPI.Data.DTO;
using GeekShopping.ProductAPI.RabbitMQSender;
using GeekShopping.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GeekShopping.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private IProductRepository _productRepository;
        private IRabbitMQMessageSender _rabbitMQMessageSender;

        public ProductController(IProductRepository productRepository, IRabbitMQMessageSender rabbitMQMessageSender)
        {
            _productRepository = productRepository ?? throw new ArgumentException(nameof(productRepository)); ;
            _rabbitMQMessageSender = rabbitMQMessageSender;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> FindAll()
        {
            var products = await _productRepository.FindAll();

            if (products == null) return NotFound();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> FindById(long id)
        {
            var product = await _productRepository.FindById(id);

            if (product.Id <= 0) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ProductDTO dto)
        {
            if (dto == null) return NotFound();

            var product = await _productRepository.Create(dto);

            // RABBITMQ

            _rabbitMQMessageSender.SendMessage(product, "productcreatedqueue");

            return Ok(product);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] ProductDTO dto)
        {
            if (dto == null) return NotFound();

            var product = await _productRepository.Update(dto);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var status = await _productRepository.Delete(id);

            if (!status) return BadRequest();

            return Ok(status);
        }
    }
}
