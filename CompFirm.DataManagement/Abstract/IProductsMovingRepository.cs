using System.Collections.Generic;
using System.Threading.Tasks;
using CompFirm.Dto.ProductsMoving;
using CompFirm.Dto.SearchResult;
using CompFirm.Dto.Users;

namespace CompFirm.DataManagement.Abstract
{
    public interface IProductsMovingRepository
    {
        Task<SearchResultDto<ProductsMovingDto>> GetProductsMoving(ProductsMovingFilterDto filter);

        Task CreateProductsMoving(PayloadDto payload, IReadOnlyCollection<ProductsMovingDto> movings);

        Task DeleteProductsMoving(PayloadDto payload, int id);
    }
}
