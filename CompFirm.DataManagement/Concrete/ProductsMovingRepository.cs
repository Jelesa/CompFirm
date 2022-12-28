using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompFirm.DataManagement.Abstract;
using CompFirm.Domain.Exceptions;
using CompFirm.Domain.Models;
using CompFirm.Dto;
using CompFirm.Dto.ProductsMoving;
using CompFirm.Dto.SearchResult;
using CompFirm.Dto.Users;
using Dapper;

namespace CompFirm.DataManagement.Concrete
{
    public class ProductsMovingRepository : IProductsMovingRepository
    {
        private const string RowCountParameterName = "p_totalRowCount";

        private readonly Dictionary<string, int> ProductsMovingToMnoz = new Dictionary<string, int>
        {
            {"Выдача товара", -1},
            {"Поступление товара", 1},
            {"Возврат товара", 1},
        };

        private readonly IDataAccess dataAccess;

        public ProductsMovingRepository(
            IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<SearchResultDto<ProductsMovingDto>> GetProductsMoving(ProductsMovingFilterDto filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.DateFrom) && !DateTime.TryParse(filter.DateFrom, out _)
                || !string.IsNullOrWhiteSpace(filter.DateTo) && !DateTime.TryParse(filter.DateTo, out _))
            {
                throw new BadRequestException("Дата должна иметь формат ДД.ММ.ГГГГ");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                var parameters = GetParameters(filter);

                var dbRes = await connection.QueryAsync<ProductsMovingDto>("MOVINGS_SEARCH", parameters, commandType: CommandType.StoredProcedure);
                var list = dbRes.AsList();

                list.ForEach(x => x.Count = Math.Abs(x.Count));

                int totalRowCount = parameters.Get<int>(RowCountParameterName);

                var dto = new SearchResultDto<ProductsMovingDto>
                {
                    Found = totalRowCount,
                    Result = list
                };

                return dto;
            }
        }

        public async Task CreateProductsMoving(PayloadDto payload, IReadOnlyCollection<ProductsMovingDto> movings)
        {
            if (!payload.IsAdmin)
            {
                throw new UnauthorizedException("У вас нет прав на выполнение данной команды");
            }

            if (movings.GroupBy(x => x.MovingType).Distinct().Count() > 1)
            {
                throw new BadRequestException("В одном запросе могут быть движения только одного типа");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                var goodMovings = movings.Where(x => x.ProductId != 0 && x.Count != 0).ToArray();

                var mnoz = ProductsMovingToMnoz[goodMovings.First().MovingType];

                var movingTypeId = await connection.QueryFirstOrDefaultAsync<int>(
                    @"SELECT `id` from products_moving_type where `name` = @name",
                    new
                    {
                        name = goodMovings.First().MovingType
                    });

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    foreach (var m in goodMovings)
                    {
                        await connection.ExecuteAsync(
                            @"INSERT INTO products_moving (id_product, id_moving_type, action_date, count)
                            VALUES (@productId, @movingType, @actionDate, @count)",
                            new
                            {
                                productId = m.ProductId,
                                movingType = movingTypeId,
                                actionDate = m.ActionDate,
                                count = mnoz * m.Count,
                            });
                    }

                    await transaction.CommitAsync();
                }
            }
        }

        public async Task DeleteProductsMoving(PayloadDto payload, int id)
        {
            if (!payload.IsAdmin)
            {
                throw new UnauthorizedException("У вас нет прав на выполнение данной команды");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                        await connection.ExecuteAsync(
                            @"DELETE FROM products_moving WHERE `id`=@id",
                            new
                            {
                                id = id
                            });

                    await transaction.CommitAsync();
                }
            }
        }

        private static DynamicParameters GetParameters(ProductsMovingFilterDto filter)
        {
            var parameters = new DynamicParameters();

            parameters.Add("p_movingType", filter.MovingType);
            parameters.Add("p_dateFrom", filter.DateFrom);
            parameters.Add("p_dateTo", filter.DateTo);
            parameters.Add("p_limit", filter.Limit);
            parameters.Add("p_offset", filter.Offset);
            parameters.Add(RowCountParameterName, dbType: DbType.Int32, direction: ParameterDirection.Output);

            return parameters;
        }
    }
}
