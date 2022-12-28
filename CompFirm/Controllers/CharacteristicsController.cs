using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CompFirm.DataManagement.Abstract;
using CompFirm.Domain.Constants;
using CompFirm.Dto.Characteristics;

namespace CompFirm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacteristicsController : ControllerBase
    {
        private readonly IAuthRepository authRepository;
        private readonly ICharacteristicsRepository characteristicsRepository;

        public CharacteristicsController(
            IAuthRepository authRepository,
            ICharacteristicsRepository characteristicsRepository)
        {
            this.authRepository = authRepository;
            this.characteristicsRepository = characteristicsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCharacteristics()
        {
            var res = await this.characteristicsRepository.GetCharacteristics();

            return this.Ok(res);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token,
            CreateCharacteristicRequestDto request)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.characteristicsRepository.Create(payload, request);

            return this.Ok();
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> Delete(
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token,
            int id)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.characteristicsRepository.Delete(payload, id);

            return this.Ok();
        }
    }
}
