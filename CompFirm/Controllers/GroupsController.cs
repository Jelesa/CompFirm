using CompFirm.DataManagement.Abstract;
using CompFirm.Domain.Constants;
using CompFirm.Dto.Groups;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CompFirm.Dto.SearchFilter;

namespace CompFirm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IAuthRepository authRepository;
        private readonly IGroupsRepository groupsRepository;

        public GroupsController(
            IAuthRepository authRepository,
            IGroupsRepository groupsRepository)
        {
            this.authRepository = authRepository;
            this.groupsRepository = groupsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMainGroups()
        {
            var groups = await this.groupsRepository.GetMainGroups();

            return this.Ok(groups);
        }

        [HttpGet("{id}/search")]
        public async Task<IActionResult> GetChildGroups(int id)
        {
            var groups = await this.groupsRepository.GetChildGroups(id);

            return this.Ok(groups);
        }

        [HttpGet("{id}/product-types")]
        public async Task<IActionResult> GetProductTypes(int id)
        {
            var productTypes = await this.groupsRepository.GetProductTypes(id);

            return this.Ok(productTypes);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetGroups([FromQuery]BaseSearchFilterDto filter)
        {
            var groups = await this.groupsRepository.GetGroups(filter);

            return this.Ok(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupById(
            int id,
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            var res = await this.groupsRepository.GetGroupById(id, payload);

            return this.Ok(res);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup(
            [FromBody] GroupDto group,
            [FromHeader(Name = Constants.AuthorizationHeaderName)]
            string token)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.groupsRepository.CreateGroup(group, payload);

            return this.Ok();
        }


        [HttpPut("{id}/update")]
        public async Task<IActionResult> UpdataGroupById(
            int id,
            [FromBody] GroupDto group,
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            group.Id = (ulong)id;

            await this.groupsRepository.UpdateGroupById(group, payload);

            return this.Ok();
        }

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteGroupById(
            int id,
            [FromHeader(Name = Constants.AuthorizationHeaderName)] string token)
        {
            var payload = await this.authRepository.GetUserPayload(token);

            await this.groupsRepository.DeleteGroupById(id, payload);

            return this.Ok();
        }
    }
}
