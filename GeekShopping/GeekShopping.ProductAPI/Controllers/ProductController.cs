using GeekShopping.ProductAPI.Data.DTO;
using GeekShopping.ProductAPI.RabbitMQSender;
using GeekShopping.ProductAPI.Repository;
using GeekShopping.ProductAPI.Utils;
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
        private readonly IConfiguration _configuracoes;

        public ProductController(IProductRepository productRepository, 
            IRabbitMQMessageSender rabbitMQMessageSender,
            IConfiguration configuracoes)
        {
            _productRepository = productRepository ?? throw new ArgumentException(nameof(productRepository)); ;
            _rabbitMQMessageSender = rabbitMQMessageSender;
            _configuracoes = configuracoes;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> FindAll()
        {
            var products = await _productRepository.FindAll();

            if (products == null) return NotFound();

            return Ok(products);
        }

        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult> FindById(long id)
        {
            var product = await _productRepository.FindById(id);

            if (product.Id <= 0) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        //[Authorize]
        public async Task<ActionResult> Create([FromBody] ProductDTO dto)
        {
            if (dto == null) return NotFound();

            var product = await _productRepository.Create(dto);

            // RabbitMQ
            //
            var executarMensageria = bool.Parse(_configuracoes["ExecutarRabbitMQ"]);

            if (executarMensageria)
                _rabbitMQMessageSender.SendMessage(product, "productcreatedqueue");

            return Ok(product);
        }

        [HttpPut]
        //[Authorize]
        public async Task<ActionResult> Update([FromBody] ProductDTO dto)
        {
            if (dto == null) return NotFound();

            var product = await _productRepository.Update(dto);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = Role.Admin)]
        public async Task<ActionResult> Delete(long id)
        {
            var status = await _productRepository.Delete(id);

            if (!status) return BadRequest();

            return Ok(status);
        }
    }
}
