using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CompFirm.DataManagement.Abstract;
using CompFirm.Domain.Constants;
using CompFirm.Dto.ProductsMoving;

namespace CompFirm.Controllers
{
    [Route("api/products-moving")]
    [ApiController]
    public class ProductsMovingController : ControllerBase
    {
        private readonly IAuthRepository authRepository;
        private readonly IProductsMovingRepository productsMovingRepository;

        public ProductsMovingController(
            IAuthRepository authRepository,
            IProductsMovingRepository productsMovingRepository)
        {
            this.authRepository = authRepository;
            this.productsMovingRepository = productsMovingRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductMoving([FromQuery] ProductsMovingFilterDto filter)
        {
            var res = await this.productsMovingRepository.GetProductsMoving(filter);

            return this.Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductsMoving(
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token,
            IReadOnlyCollection<ProductsMovingDto> movings)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.productsMovingRepository.CreateProductsMoving(payload, movings);

            return this.Ok();
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> CreateProductsMoving(
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token,
            int id)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.productsMovingRepository.DeleteProductsMoving(payload, id);

            return this.Ok();
        }
    }
}
