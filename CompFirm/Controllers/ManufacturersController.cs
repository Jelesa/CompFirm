using CompFirm.DataManagement.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CompFirm.Domain.Constants;
using CompFirm.Dto.Manufacturers;

namespace CompFirm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturersController : ControllerBase
    {
        private readonly IAuthRepository authRepository;
        private readonly IManufacturersRepository manufacturersRepository;

        public ManufacturersController(
            IAuthRepository authRepository,
            IManufacturersRepository manufacturersRepository)
        {
            this.authRepository = authRepository;
            this.manufacturersRepository = manufacturersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetManufacturers()
        {
            var result = await this.manufacturersRepository.GetManufacturers();

            return this.Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateManufacturer(
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token,
            CreateManufacturerRequestDto request)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.manufacturersRepository.CreateManufacturer(payload, request);

            return this.Ok();
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteManufacturer(
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token,
            int id)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.manufacturersRepository.DeleteManufacturer(payload, id);

            return this.Ok();
        }
    }
}
