using CompFirm.DataManagement.Abstract;
using CompFirm.Domain.Constants;
using CompFirm.Dto.Products;
using CompFirm.Dto.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CompFirm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IAuthRepository authRepository;
        private readonly IRequestsRepository requestsRepository;

        public RequestsController(
            IAuthRepository authRepository,
            IRequestsRepository requestsRepository)
        {
            this.authRepository = authRepository;
            this.requestsRepository = requestsRepository;
        }

        [HttpPut("addToCart")]
        public async Task<IActionResult> AddItemToCart(
            [FromBody] CartItemDto cartItem,
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.requestsRepository.AddItemCard(cartItem, payload);

            return this.Ok();
        }

        [HttpPut("confirm")]
        public async Task<IActionResult> ConfirmRequest(
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.requestsRepository.ConfirmRequest(payload);

            return this.Ok();
        }

        [HttpGet("cart")]
        public async Task<IActionResult> GetCartItems(
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            var res = await this.requestsRepository.GetCartItems(payload);

            return this.Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetRequestList(
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token,
            [FromQuery]RequestsFilterDto filter)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            var res = await this.requestsRepository.SearchRequests(payload, filter);

            return this.Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequestList(
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token,
            int id)
        {
            await this.authRepository.GetUserPayload(token);

            var res = await this.requestsRepository.GetRequestCard(id);

            return this.Ok(res);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelRequest(
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token,
            int id)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.requestsRepository.CancelRequest(payload, id);

            return this.Ok();
        }

        [HttpPut("status-update")]
        public async Task<IActionResult> UpdateRequestStatus(
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token,
            [FromBody] UpdateRequestStatusDto requestStatus)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.requestsRepository.UpdateRequestStatus(payload, requestStatus);

            return this.Ok();
        }

        [HttpGet("statuses")]
        public async Task<IActionResult> GetStatusList()
        {
            var res = await this.requestsRepository.GetStatusList();

            return this.Ok(res);
        }
    }
}
