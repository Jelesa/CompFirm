using CompFirm.Dto.Products;
using CompFirm.Dto.RequestItems;
using CompFirm.Dto.Requests;
using CompFirm.Dto.Users;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompFirm.Dto.SearchResult;
using CompFirm.Dto.Statuses;

namespace CompFirm.DataManagement.Abstract
{
    public interface IRequestsRepository
    {
        Task AddItemCard(CartItemDto cartItem, PayloadDto payload);

        Task<IReadOnlyCollection<RequestItemDto>> GetCartItems(PayloadDto payload);

        Task ConfirmRequest(PayloadDto payload);

        Task<SearchResultDto<RequestShortInfoDto>> SearchRequests(
            PayloadDto payload,
            RequestsFilterDto filter);

        Task<RequestFullInfoDto> GetRequestCard(int requestId);

        Task CancelRequest(PayloadDto payload, int requestId);

        Task UpdateRequestStatus(
            PayloadDto payload,
            UpdateRequestStatusDto requestStatus);

        Task<IReadOnlyCollection<RequestStatusDto>> GetStatusList();
    }
}
