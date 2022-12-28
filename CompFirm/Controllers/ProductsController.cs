using CompFirm.DataManagement.Abstract;
using CompFirm.Domain.Constants;
using CompFirm.Dto.Products;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CompFirm.Dto.SearchFilter;

namespace CompFirm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IAuthRepository authRepository;
        private readonly IProductsRepository productsRepository;

        public ProductsController(
            IAuthRepository authRepository,
            IProductsRepository productsRepository)
        {
            this.authRepository = authRepository;
            this.productsRepository = productsRepository;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] ProductsFilterDto filter)
        {
            var searchResult = await this.productsRepository.SearchProducts(filter);

            return this.Ok(searchResult);
        }

        [HttpGet("price-interval")]
        public async Task<IActionResult> GetPrices()
        {
            var searchResult = await this.productsRepository.GetPriceInterval();

            return this.Ok(searchResult);
        }

        [HttpDelete("delete-returned-product/{id}")]
        public async Task<IActionResult> DeleteReturnedProduct(int id,
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.productsRepository.DeleteReturnedProducts(id, payload);

            return this.Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetProductShortInfo(
            [FromQuery] BaseSearchFilterDto filter,
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            var searchResult = await this.productsRepository.GetProductShortInfo(filter, payload);

            return this.Ok(searchResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(
            int id,
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            var searchResult = await this.productsRepository.GetProduct(id, payload);

            return this.Ok(searchResult);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct(
            [FromBody] ProductInfoDto product,
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.productsRepository.CreateProduct(product, payload);

            return this.Ok();
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdateProduct(
            int id,
            [FromBody] ProductInfoDto product,
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            product.ProductId = id;

            await this.productsRepository.UpdateProduct(product, payload);

            return this.Ok();
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteProduct(int id,
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.productsRepository.DeleteProduct(id, payload);

            return this.Ok();
        }
    }
}
