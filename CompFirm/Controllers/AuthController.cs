using CompFirm.DataManagement.Abstract;
using CompFirm.Dto.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CompFirm.Domain.Constants;

namespace CompFirm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IAuthRepository authRepository;

        public AuthController(
            ILogger<AuthController> logger,
            IAuthRepository authRepository)
        {
            this.logger = logger;
            this.authRepository = authRepository;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> Registration(CreateUserRequestDto createUserRequestDto)
        {
            await this.authRepository.SignUp(createUserRequestDto);

            return this.Ok();
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string login, string password)
        {
            var authResult = await this.authRepository.SignIn(login, password);

            return this.Ok(authResult);
        }

        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInfo(
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token)
        {
            var userInfo = await this.authRepository.GetUserInfo(token);

            return this.Ok(userInfo);
        }

        [HttpPut("update-user-info")]
        public async Task<IActionResult> UpdateUserInfo(
            [FromBody] UpdateUserInfoRequestDto fullUserInfo,
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token)
        {
            await this.authRepository.UpdateUserInfo(fullUserInfo, token);

            return this.Ok();
        }
    }
}
