using CompFirm.DataManagement.Abstract;
using CompFirm.Domain.Exceptions;
using CompFirm.Domain.Models;
using CompFirm.Dto.Groups;
using CompFirm.Dto.Manufacturers;
using CompFirm.Dto.Products;
using CompFirm.Dto.SearchResult;
using CompFirm.Dto.Users;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CompFirm.DataManagement.Concrete.Validation;
using CompFirm.DataManagement.Constants;
using CompFirm.Dto.SearchFilter;

namespace CompFirm.DataManagement.Concrete
{
    public class ProductsRepository : IProductsRepository
    {
        private const string RowCountParameterName = "p_totalRowCount";

        private readonly IDataAccess dataAccess;

        public ProductsRepository(
            IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<SearchResultDto<ProductInfoDto>> SearchProducts(ProductsFilterDto filter)
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                var parameters = GetParameters(filter);

                var dbRes = await connection.QueryAsync<ProductInfoDbModel>("PRODUCTS_SEARCH", parameters, commandType: CommandType.StoredProcedure);

                int totalRowCount = parameters.Get<int>(RowCountParameterName);

                var dto = new SearchResultDto<ProductInfoDto>
                {
                    Found = totalRowCount,
                    Result = GetDto(dbRes)
                };

                return dto;
            }
        }

        public async Task<PriceIntervalDto> GetPriceInterval()
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                var prices = await connection.QueryFirstOrDefaultAsync<PriceIntervalDto>(QueryTexts.Products.GetPriceInterval);

                return prices;
            }
        }

        public async Task DeleteReturnedProducts(int id, PayloadDto payload)
        {
            if (!payload.IsAdmin)
            {
                throw new UnauthorizedException("У вас нет прав на выполнение данной операции");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                await connection.ExecuteAsync(QueryTexts.Products.DeleteReturnedProduct,
                    new
                    {
                        id = id
                    });
            }
        }

        public async Task<IReadOnlyCollection<ProductShortInfoDto>> GetProductShortInfo(BaseSearchFilterDto filter, PayloadDto payload)
        {
            if (!payload.IsAdmin)
            {
                throw new UnauthorizedException("У вас нет прав на выполнение данного запроса");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                var products = await connection.QueryAsync<ProductShortInfoDto>(QueryTexts.Products.GetProductShortInfo);

                return products.AsList();
            }
        }

        public async Task CreateProduct(ProductInfoDto product, PayloadDto payload)
        {
            if (!payload.IsAdmin)
            {
                throw new UnauthorizedException("У вас нет прав на выполнение удаления товара или услуги");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                await ProductValidator.ValidateCreate(connection, product);

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    // добавляем продукт/услугу
                    var productId = await connection.QueryFirstOrDefaultAsync<int>(@"
INSERT INTO `products` (`name`, `price`, `image_url`, `id_group`, `id_manufacturer` ) VALUES
(@name, @price, @image_url, @groupId, @manufacturerId);
SELECT LAST_INSERT_ID();",
                        new
                        {
                            name = product.Name,
                            price = product.Price,
                            image_url = product.ImageUrl,
                            groupId = product.Group.Id,
                            manufacturerId = product.Manufacturer.Id,
                            productId = product.ProductId
                        });

                    await connection.ExecuteAsync(
                        @"INSERT INTO `product_characteristics` (id_product, id_characteristic, `value`)
                            VALUES (@productId, (select `id` from characteristics where `name` = 'Вид товара'), @value)",
                        new
                        {
                            productId = productId,
                            value = product.ProductType
                        });

                    foreach (var ch in product.Characteristics)
                    {
                        await connection.ExecuteAsync(
                            @"INSERT INTO `product_characteristics` (id_product, id_characteristic, `value`)
                            VALUES (@productId, @chId, @value)",
                            new
                            {
                                productId = productId,
                                chId = ch.CharacteristicId,
                                value = ch.Value
                            });
                    }

                    await transaction.CommitAsync();
                }
            }
        }

        public async Task UpdateProduct(ProductInfoDto product, PayloadDto payload)
        {
            if (!payload.IsAdmin)
            {
                throw new UnauthorizedException("У вас нет прав на выполнение удаления товара или услуги");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                await ProductValidator.ValidateEdit(connection, product);

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    // обновляем сам продукт / услугу
                    await connection.ExecuteAsync(@"
UPDATE `products` SET
`name`=@name,
`price`=@price,
`image_url`=@image_url,
`id_group`=@groupId,
`id_manufacturer`=@manufacturerId
WHERE `id`=@productId",
                        new
                        {
                            name = product.Name,
                            price = product.Price,
                            image_url = product.ImageUrl,
                            groupId = product.Group.Id,
                            manufacturerId = product.Manufacturer.Id,
                            productId = product.ProductId
                        });

                    // удаляем все старые характеристики
                    await connection.ExecuteAsync(
                        "DELETE FROM `product_characteristics` WHERE id_product = @productId",
                        new
                        {
                            productId = product.ProductId
                        });

                    await connection.ExecuteAsync(
                        @"INSERT INTO `product_characteristics` (id_product, id_characteristic, `value`)
                            VALUES (@productId, (select `id` from characteristics where `name` = 'Вид товара'), @value)",
                        new
                        {
                            productId = product.ProductId,
                            value = product.ProductType
                        });

                    foreach (var ch in product.Characteristics)
                    {
                        await connection.ExecuteAsync(
                            @"INSERT INTO `product_characteristics` (id_product, id_characteristic, `value`)
                            VALUES (@productId, @chId, @value)",
                            new
                            {
                                productId = product.ProductId,
                                chId = ch.CharacteristicId,
                                value = ch.Value
                            });
                    }

                    await transaction.CommitAsync();
                }
            }
        }

        public async Task DeleteProduct(int id, PayloadDto payload)
        {
            if (!payload.IsAdmin)
            {
                throw new UnauthorizedException("У вас нет прав на выполнение удаления товара или услуги");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var productMoving = await connection.QueryFirstOrDefaultAsync<int>(QueryTexts.Products.GetCountProductMoving,
                        new
                        {
                            id = id
                        });

                    var productItem = await connection.QueryFirstOrDefaultAsync<int>(QueryTexts.Products.GetCountProductOrdered,
                        new
                        {
                            id = id
                        });

                    if (productMoving > 0 || productItem > 0)
                    {
                        throw new BadRequestException("Товар или уcлуга не может быть удален т.к. был заказан или содержится на складе.");
                    }

                    await connection.ExecuteAsync(QueryTexts.Products.DeleteProduct,
                        new
                        {
                            id = id
                        });

                    await transaction.CommitAsync();
                }

            }
        }

        public async Task<ProductInfoDto> GetProduct(int id, PayloadDto payload)
        {
            if (!payload.IsAdmin)
            {
                throw new UnauthorizedException("У вас нет прав на выполнение данного запроса");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                var productInfo = (await connection.QueryAsync<ProductInfoDbModel>(QueryTexts.Products.GetProduct,
                    new
                    {
                        id = id
                    })).AsList();

                if (productInfo.Count == 0)
                {
                    throw new BadRequestException("Товар не найден");
                }

                return GetDto(productInfo).FirstOrDefault();
            }
        }

        private static IReadOnlyCollection<ProductInfoDto> GetDto(IEnumerable<ProductInfoDbModel> dbRes)
        {
            var dto = dbRes
                .GroupBy(x => x.ProductId)
                .Select(x => new ProductInfoDto
                {
                    ProductId = x.First().ProductId,
                    Name = x.First().ProductName,
                    Price = x.First().Price,
                    ImageUrl = x.First().ImageUrl,
                    Group = new GroupDto
                    {
                        Id = x.First().GroupId,
                        Name = x.First().GroupName
                    },
                    Manufacturer = new ManufacturerDto
                    {
                        Id = x.First().ManufacturerId,
                        Name = x.First().ManufacturerName
                    },
                    ProductType = x.First().ProductType,
                    Characteristics = x.Where(x => x.CharacteristicId != 0
                                                   || !string.IsNullOrWhiteSpace(x.CharacteristicName))
                        .Select(x => new CharacteristicValueDto
                        {
                            CharacteristicId = x.CharacteristicId,
                            CharacteristicName = x.CharacteristicName,
                            Value = x.CharacteristicValue,
                            Unit = x.CharacteristicUnit
                        }).ToArray()
                });

            return dto.ToArray();
        }

        private static DynamicParameters GetParameters(ProductsFilterDto filter)
        {
            var parameters = new DynamicParameters();

            parameters.Add("p_productName", filter.Name);
            parameters.Add("p_groupId", filter.GroupId);
            parameters.Add("p_productType", filter.ProductType, DbType.String);
            parameters.Add("p_minPrice", filter.MinPrice);
            parameters.Add("p_maxPrice", filter.MaxPrice);
            parameters.Add("p_limit", filter.Limit);
            parameters.Add("p_offset", filter.Offset);
            parameters.Add(RowCountParameterName, dbType: DbType.Int32, direction: ParameterDirection.Output);

            return parameters;
        }
    }
}
