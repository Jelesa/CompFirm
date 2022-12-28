using CompFirm.Dto.Products;
using CompFirm.Dto.SearchResult;
using CompFirm.Dto.Users;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompFirm.Dto.SearchFilter;

namespace CompFirm.DataManagement.Abstract
{
    public interface IProductsRepository
    {
        Task<SearchResultDto<ProductInfoDto>> SearchProducts(ProductsFilterDto filter);

        Task<PriceIntervalDto> GetPriceInterval();

        Task DeleteReturnedProducts(int id, PayloadDto payload);

        Task<IReadOnlyCollection<ProductShortInfoDto>> GetProductShortInfo(
            BaseSearchFilterDto filter,
            PayloadDto payload);

        Task<ProductInfoDto> GetProduct(int id, PayloadDto payload);

        Task CreateProduct(ProductInfoDto product, PayloadDto payload);

        Task UpdateProduct(ProductInfoDto product, PayloadDto payload);

        Task DeleteProduct(int id, PayloadDto payload);
    }
}
