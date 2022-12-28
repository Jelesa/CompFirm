using CompFirm.Dto.Groups;
using CompFirm.Dto.Users;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompFirm.Dto.SearchFilter;
using CompFirm.Dto.SearchResult;

namespace CompFirm.DataManagement.Abstract
{
    public interface IGroupsRepository
    {
        Task<IReadOnlyCollection<GroupDto>> GetMainGroups();

        Task<IReadOnlyCollection<GroupDto>> GetChildGroups(int id);

        Task<IReadOnlyCollection<ProductTypeDto>> GetProductTypes(int id);

        Task<SearchResultDto<GroupDto>> GetGroups(BaseSearchFilterDto filter);

        Task<GroupDto> GetGroupById(int id, PayloadDto payload);

        Task CreateGroup(GroupDto group, PayloadDto payload);

        Task UpdateGroupById(GroupDto group, PayloadDto payload);

        Task DeleteGroupById(int id, PayloadDto payload);
    }
}
